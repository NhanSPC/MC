Imports pbs.Helper
Imports pbs.BO.SM

Namespace MC
    Partial Class JE
        Implements IAutoDetectSubform

#Region "IAutoDetectSubform"
        ''' <summary>
        ''' Form the access URL for this object
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetURLShortCut() As String Implements IAutoDetectSubform.GetURLShortCut
            If Not String.IsNullOrEmpty(_transType) Then
                Return String.Format("{0}/{1}", Me.GetType.ToClassSchemaName, Me._transType)
            Else
                Dim subform = pbs.Helper.UIServices.ValueSelectorService.SelectID(JDInfoList.GetJDInfoList, ResStrConst.SelectItemText("TransType"))
                Return String.Format("{0}/{1}?TransType={1}", Me.GetType.ToClassSchemaName, Nz(subform, String.Empty))
            End If
        End Function
#End Region

        Private Function GetPresetCode() As Dictionary(Of String, String)
            Dim theUIRInfo = Rules.UIRInfoList.GetUIRInfo(GetType(JE).ToString, GetSubForm)
            Dim PresetProfile = theUIRInfo.DefaultPresetCode

            Dim info As pbs.BO.Rules.PresetInfo = Nothing

            If Not String.IsNullOrEmpty(PresetProfile) AndAlso pbs.BO.Rules.PresetInfoList.ContainsCode(PresetProfile, info) Then
                Return info.GetEvaluatedPresetDic
            End If

            Return Nothing
        End Function

        Public Shared Function CreateCollectionJournal(pReceivables As List(Of MCLDGInfo)) As JE

            If pReceivables Is Nothing OrElse pReceivables.Count = 0 Then Return Nothing

            Dim Patients = (From info In pReceivables Select info.PatientCode Distinct).ToList

            If Patients.Count > 1 Then ExceptionThower.BusinessRuleStop(ResStr("Create collection journal can process one Patient per journal"))

            Dim sample = pReceivables(0)

            Dim theJournal = NewJE(0)

            theJournal.TransType = pbs.BO.MC.Settings.GetSettings.GetCollectionJournalType
            theJournal.TransDate = "T"
            theJournal.Period = "T"
            'theJournal.SchoolYear = sample.SchoolYear
            theJournal._status = String.Empty

            Dim thePreset As Dictionary(Of String, String) = Nothing
            'Header ----------------------
            Dim TransformHeader As pbs.BO.Rules.TransformInfo = pbs.BO.Rules.Transform.GetSystemTransformProfile("MCREC2CO", GetType(pbs.BO.MC.MCLDG), GetType(pbs.BO.MC.JE))
            If TransformHeader IsNot Nothing Then thePreset = TransformHeader.GetTranformTargetData(sample)

            thePreset = thePreset.Merge(theJournal.GetPresetCode, True)
            BOFactory.ApplyPreset(theJournal, thePreset)

            'theJournal._candidateId.Text = sample.CandidateId
            theJournal.PatientCode = sample.PatientCode
            theJournal._description = String.Format(ResStr("Collection journal for {0}"), theJournal.GetPatientInfo.Description)

            'Detail --------------------
            For Each original In pReceivables

                Dim thekid = theJournal.Lines.AddNew
                BOFactory.ApplyPreset(thekid, BOFactory.Obj2Dictionary(original))
                thekid._amount.Float = original.Amount
                thekid._quantity.Text = original.Quantity

                thekid.ReverseMe()

                thekid._quantity.Float = Math.Abs(thekid._quantity.Float)
                'thekid.SchoolYear = original.SchoolYear

                thekid._offsetLineNo = original.LineNo

            Next

            theJournal.RecalculateTotal()
            theJournal.PropertyHasChanged()

            Return theJournal

        End Function

        ''' <summary>
        ''' When a journal is generated as a cash collection. After posting, it try to perform allocation
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub OffsetAllocationIfNeeded()
            If Me.IsNew Then Exit Sub 'only posted journal can perform OffsetAllocation
            For Each kid In Me.Lines
                kid.OffsetAllocationIfNeeded()
            Next
        End Sub

        Public Shared Function CreateRefundJournal(pReceived As List(Of MCLDGInfo)) As JE

            If pReceived Is Nothing OrElse pReceived.Count = 0 Then Return Nothing

            Dim Patients = (From info In pReceived Select info.PatientCode Distinct).ToList

            If Patients.Count > 1 Then ExceptionThower.BusinessRuleStop(ResStr("Create refund journal can process one Patient per journal"))

            Dim sample = pReceived(0)

            Dim theJournal = NewJE(0)

            theJournal.TransType = pbs.BO.MC.Settings.GetSettings.GetRefundJournalType
            theJournal.TransDate = "T"
            theJournal.Period = "T"
            'theJournal._candidateId.Text = sample.CandidateId
            theJournal.PatientCode = sample.PatientCode

            theJournal._description = String.Format(ResStr("Refund journal for {0}"), theJournal.GetPatientInfo.Description)
            'theJournal.SchoolYear = sample.SchoolYear
            theJournal._status = String.Empty

            'Header ----------------------
            Dim TransformHeader As pbs.BO.Rules.TransformInfo = Nothing
            If pbs.BO.Rules.TransformInfoList.ContainsCode("MCREC2RF", TransformHeader) Then
                Dim theDic = TransformHeader.GetTranformTargetData(sample)
                BOFactory.ApplyPreset(theJournal, theDic)
            Else
                Dim newTransform = pbs.BO.Rules.Transform.NewTransform("MCREC2RF")
                newTransform.Name = "This transform profile is used by function pbs.BO.MC.PatientStatement. button Refund"
                newTransform.SourceClassName = GetType(pbs.BO.MC.MCLDG).ToString
                newTransform.ClassName = GetType(pbs.BO.MC.JE).ToString
                newTransform.Save()
            End If
            theJournal.PatientCode = sample.PatientCode

            'Detail --------------------
            For Each original In pReceived

                Dim thekid = theJournal.Lines.AddNew
                BOFactory.ApplyPreset(thekid, BOFactory.Obj2Dictionary(original))
                thekid._amount.Float = original.Amount
                thekid._quantity.Text = original.Quantity

                thekid.ReverseMe()

                thekid._quantity.Float = Math.Abs(thekid._quantity.Float)
                'thekid.SchoolYear = original.SchoolYear

                thekid._offsetLineNo = original.LineNo

            Next

            theJournal.RecalculateTotal()
            theJournal.PropertyHasChanged()

            Return theJournal

        End Function

    End Class

End Namespace