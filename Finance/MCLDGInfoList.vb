
Imports Csla
Imports Csla.Data
Imports System.Xml
Imports pbs.Helper

Imports pbs.BO.SQLBuilder

Namespace MC

    <Serializable()> _
    Public Class MCLDGInfoList
        Inherits Csla.ReadOnlyListBase(Of MCLDGInfoList, MCLDGInfo)

        Private Shared _DTB As String = String.Empty

#Region " Factory Methods "

        Private Sub New()
            _DTB = Context.CurrentBECode
        End Sub

        Friend Shared Function GetMCLDGInfo(ByVal lineNo As String) As MCLDGInfo
            Dim dic = New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
            dic.Add("LineNo", lineNo.ToInteger)
            Dim ret = GetMCLDGInfoList(dic)
            If ret IsNot Nothing AndAlso ret.Count > 0 Then
                Return ret(0)
            Else
                Return MCLDGInfo.EmptyMCLDGInfo(lineNo)
            End If
        End Function

        Public Shared Function GetEmptyMCLDGInfoList() As MCLDGInfoList
            Return New MCLDGInfoList
        End Function

        Public Shared Function GetMCLDGInfoList() As MCLDGInfoList
            Return GetEmptyMCLDGInfoList()
        End Function

        Public Shared Function GetMCLDGInfoList(pFilters As Dictionary(Of String, String)) As MCLDGInfoList
            Dim _qd = Query.BuildQDByDic(pFilters)
            Dim _sqlText = _qd.BuildSQL

            Return DataPortal.Fetch(Of MCLDGInfoList)(New FilterCriteria() With {._sqlText = _sqlText})

        End Function

        Public Shared Function GetMCLDGInfoList(SQLScript As String) As MCLDGInfoList
            If String.IsNullOrEmpty(SQLScript) Then
                Return MCLDGInfoList.GetEmptyMCLDGInfoList
            Else
                Return DataPortal.Fetch(Of MCLDGInfoList)(New FilterCriteria() With {._sqlText = SQLScript})
            End If
        End Function

        Public Shared Sub InvalidateCache()
        End Sub

#End Region ' Factory Methods

#Region " Data Access "

#Region " Filter Criteria "

        <Serializable()> _
        Private Class FilterCriteria
            Friend _sqlText As String = Nothing
            Public Sub New()
            End Sub
        End Class

#End Region
        Private Shared _lockObj As New Object

        Private Overloads Sub DataPortal_Fetch(ByVal criteria As FilterCriteria)
            SyncLock _lockObj
                RaiseListChangedEvents = False
                IsReadOnly = False
                  Using ctx = ConnectionFactory.GetDBConnection(True)
                    Using cm = ctx.CreateSQLCommand

                        cm.CommandText = criteria._sqlText
                        cm.CommandTimeout = 300
                        If Not String.IsNullOrEmpty(criteria._sqlText) Then
                            Using dr As New SafeDataReader(cm.ExecuteReader)
                                While dr.Read
                                    Dim info = MCLDGInfo.GetMCLDGInfo(dr)
                                    Me.Add(info)
                                End While
                            End Using
                        End If

                    End Using

                End Using
                IsReadOnly = True
                RaiseListChangedEvents = True
            End SyncLock
        End Sub

#End Region ' Data Access                   

#Region "Usage - MCLDG"

        Public Shared Function GetPatientDetail(pPatientCode As String, pDate As String, pFilters As Dictionary(Of String, String)) As IList

            Dim dic = New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
            dic = dic.Merge(pFilters, True)

            dic.InsertUpdate("PatientCode", pPatientCode)

            Dim CalculationDate = pbs.Helper.SmartDate.Parse(pDate).Date.ToSunDate
            dic.InsertUpdate("TransDate", String.Format("<<!..{0}", CalculationDate))

            'If PatientCode is given, we should ignore CandidateId, because many other transaction may have empty CandidateId
            If Not String.IsNullOrEmpty(pPatientCode) AndAlso dic.ContainsKey("CandidateId") Then dic.InsertUpdate("CandidateId", "<ALL>")

            Return MCLDGInfoList.GetMCLDGInfoList(dic)

        End Function

        ''' <summary>
        ''' 1. List all transactions up to pDate, sorted by Due date (TransDate) <para/>
        ''' 2. Split to collectable List and collected list <para/>
        ''' 3. Scan from beginning. Perform allocation - set result to Status, Pending Amount, Payment Ref <para/>
        ''' </summary>
        ''' <param name="pPatientCode"></param>
        ''' <param name="pDate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetFirstDueFirstPayMCLDG(pPatientCode As String, pDate As String, pFilters As Dictionary(Of String, String)) As List(Of MCLDGInfo)

            Dim dic = New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
            dic = dic.Merge(pFilters, True)

            dic.InsertUpdate("PatientCode", pPatientCode)

            Dim CalculationDate = pbs.Helper.SmartDate.Parse(pDate).Date.ToSunDate
            dic.InsertUpdate("TransDate", String.Format("<<!..{0}", CalculationDate))

            'If PatientCode is given, we should ignore CandidateId, because many other transaction may have empty CandidateId
            'If Not String.IsNullOrEmpty(pPatientCode) AndAlso dic.ContainsKey("CandidateId") Then dic.InsertUpdate("CandidateId", "<ALL>")

            Dim theList = MCLDGInfoList.GetMCLDGInfoList(dic)
            For Each info In theList
                info.CalculationDate = CalculationDate
                info.PendingAmount = 0
            Next
            Dim markers = ManualAllocationMarkers()

            Dim theDebits = (From info In theList Where info.DC.Equals("D", StringComparison.OrdinalIgnoreCase) _
                Order By info.GetSortingKey).ToList
            Dim theCredits = (From info In theList Where Not info.DC.Equals("D", StringComparison.OrdinalIgnoreCase) _
                Order By info.GetSortingKey).ToList

            Dim TotalCollected = 0
            For Each info In theCredits
                If markers.Contains(info.Allocation) Then
                    info.SetManualPaymentStatus()
                Else
                    info.OverPaid = Math.Abs(info.Amount)
                    TotalCollected += Math.Abs(info.Amount)
                End If
            Next

            For Each Debit In theDebits

                If markers.Contains(Debit.Allocation) Then
                    Debit.SetManualPaymentStatus()
                Else
                    Debit.PaymentStatus = "UNPAID"
                    Debit.PendingAmount = Debit.Amount
                    If TotalCollected > 0 Then
                        AllocatePaymentFrom(theCredits, Debit)
                        TotalCollected -= Math.Abs(Debit.Amount)
                    End If
                End If

            Next

            Return theList.ToList

        End Function

        Private Shared Function ManualAllocationMarkers() As List(Of String)
            Return New String() {"A", "C", "P", "R"}.ToList
        End Function

        Friend Shared Function AllocatedMarkers() As List(Of String)
            Return New String() {"A", "C", "P"}.ToList
        End Function

        Private Shared Sub AllocatePaymentFrom(pCredits As List(Of MCLDGInfo), pDebit As MCLDGInfo)

            For Each Credit In pCredits

                If pDebit.PendingAmount = 0 Then Exit For 'this receivable has been paid fully

                If Credit.OverPaid > 0 Then 'there is some amount un allocated remains

                    If Credit.OverPaid >= Math.Abs(pDebit.PendingAmount) Then 'enough for this 

                        Credit.OverPaid -= Math.Abs(pDebit.PendingAmount) 'deduct by allocation amount

                        pDebit.PendingAmount = 0
                        pDebit.PaymentStatus = "PAID"

                        If Not pDebit._collectionDate.Contains(Credit.TransDate) Then pDebit._collectionDate.Add(Credit.TransDate)
                        If Not pDebit._collectionReference.Contains(Credit.Reference) Then pDebit._collectionReference.Add(Credit.Reference)

                        Exit For 'done, full allocation to ToReceived

                    Else 'does not enough for full allocation. it is partial payment

                        pDebit.PendingAmount = pDebit.PendingAmount + Math.Abs(Credit.OverPaid) 'debit is negative so + will be deduction
                        pDebit.PaymentStatus = "PARTIAL"

                        Credit.OverPaid = 0
                        If Not pDebit._collectionDate.Contains(Credit.TransDate) Then pDebit._collectionDate.Add(Credit.TransDate)
                        If Not pDebit._collectionReference.Contains(Credit.Reference) Then pDebit._collectionReference.Add(Credit.Reference)

                    End If

                End If
            Next
        End Sub

        Friend Shared Sub UnMatch(pList As List(Of MCLDGInfo))

            'RemoveMarkers(pList)

            Dim theAllocated = (From info In pList Where AllocatedMarkers.Contains(info.Allocation)).ToList

            Dim Total = Aggregate info In theAllocated Into Sum(info.Amount)

            If Total.RoundBA <> 0 Then
                If Context.IsIronMan Then
                    If Not pbs.Helper.UIServices.ConfirmService.Confirm("Allocated transactions is unbalance. Clear markers anyway  ?") Then
                        Exit Sub
                    End If
                Else
                    pbs.Helper.UIServices.AlertService.Alert("Allocated transactions is unbalance. Only consultant can unmatch")
                    Exit Sub
                End If
            End If

            Dim scripts = New List(Of String)
            For Each info In theAllocated
                scripts.Add(info.GetUnAllocationScript)
            Next

            If scripts.Count > 0 Then
                Dim script = String.Join(Environment.NewLine, scripts.ToArray)
                SQLCommander.RunInsertUpdate(script)
            End If

        End Sub

        'Private Shared Sub RemoveMarkers(pList As List(Of MCLDGInfo))
        '    Dim theMarked = (From info In pList Where Not AllocatedMarkers.Contains(info.Allocation) AndAlso info.Allocation <> "").ToList
        '    Dim scripts = New List(Of String)
        '    For Each info In theMarked
        '        scripts.Add(info.GetUnAllocationScript)
        '    Next

        '    If scripts.Count > 0 Then
        '        Dim script = String.Join(Environment.NewLine, scripts.ToArray)
        '        SQLCommander.RunInsertUpdate(script)
        '    End If

        'End Sub

        ''' <summary>
        ''' Persist all allocated transactions (marked with Y marker) using list of script
        ''' </summary>
        ''' <param name="pList"></param>
        ''' <remarks></remarks>
        Private Shared Sub PostAllocated(pList As IEnumerable(Of MCLDGInfo))
            Dim AutoMatched = From info In pList Where info.Allocation = "Y" Group By info.AllocRef Into Group
            Dim scripts = New List(Of String)

            For Each gr In AutoMatched

                Dim total = Aggregate info In gr.Group Into Sum(info.Amount)

                If total = 0 Then
                    For Each info In gr.Group
                        scripts.Add(info.GetPostAllocationScript)
                    Next
                    If scripts.Count > 0 Then
                        Dim script = String.Join(Environment.NewLine, scripts.ToArray)
                        SQLCommander.RunInsertUpdate(script)
                    End If
                Else
                    pbs.Helper.UIServices.AlertService.Alert(ResStr("Allocation group with reference {0} does not balance and was not posted"), gr.AllocRef)
                End If

            Next

            'Bulk matching
            scripts = New List(Of String)
            Dim BulkAmount As Decimal = 0
            Dim FirstLineNo As String = String.Empty

            For Each info In pList
                If info.Allocation <> "Y" Then
                    If String.IsNullOrEmpty(FirstLineNo) Then FirstLineNo = info.LineNo
                    BulkAmount += info.Amount
                    info.MarkAsAllocated("R", FirstLineNo, "T")
                    scripts.Add(info.GetPostAllocationScript)
                End If
            Next

            If BulkAmount.RoundBA = 0 AndAlso scripts.Count > 0 Then
                Dim script = String.Join(Environment.NewLine, scripts.ToArray)
                SQLCommander.RunInsertUpdate(script)
            End If
        End Sub
#End Region

#Region "Allocation matching algorithm"

        ''' <summary>
        ''' Manual matching. ignore matching keys
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub AllocationMatchingByPatientCodeOnly(pList As List(Of MCLDGInfo))
            Dim groups = From info In pList Group By info.PatientCode Into Group, Total = Sum(info.Amount)

            Dim Patients = New Dictionary(Of String, List(Of MCLDGInfo))(StringComparer.OrdinalIgnoreCase)

            For Each gr In groups
                If Not Patients.ContainsKey(gr.PatientCode) Then
                    If gr.Total = 0 Then
                        Patients.Add(gr.PatientCode, gr.Group.ToList)
                    End If
                End If
            Next

            Dim postedMsg = New List(Of String)
            If Patients.Count > 0 Then
                Dim msg = String.Format(ResStr("Patients listed below having matching amount = zero.{0}{1}{0}. Do you want to perform allocation for them without concerning about matching keys?"), Environment.NewLine, String.Join(",", Patients.Keys.ToArray))
                If pbs.Helper.UIServices.ConfirmService.Confirm(msg) Then
                    For Each gr In Patients.Values

                        If gr.Count > 0 Then
                            Dim ref = gr(0).LineNo
                            For Each itm In gr
                                itm.MarkAsAllocated("Y", ref, SmartPeriod.CurrentPeriod.DBValue)
                            Next

                            PostAllocated(gr)

                            postedMsg.Add(String.Format("Allocation posted for Patient {0}", gr(0).PatientCode))
                        End If

                    Next
                End If
            End If

            If postedMsg.Count > 0 Then pbs.Helper.UIServices.AlertService.Alert(String.Join(Environment.NewLine, postedMsg.ToArray))

        End Sub

        ''' <summary>
        ''' Try matching Debit and Credit side or transactions
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub AllocationMatching(pList As List(Of MCLDGInfo))
            Dim groups = From info In pList Group By info.GetMatchingKey Into Group
            For Each gr In groups
                AllocationGroupMatching(gr.Group.ToList)
            Next
        End Sub

        ''' <summary>
        ''' Try matching Debit and Credit side or transactions in one matching group
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared Sub AllocationGroupMatching(pList As IEnumerable(Of MCLDGInfo))
            If pList Is Nothing OrElse pList.Count = 0 Then Exit Sub

            Dim markers = AllocatedMarkers()

            Dim theTotal = Aggregate info In pList Where Not markers.Contains(info.Allocation) Into Sum(info.Amount)

            If theTotal = 0 Then
                'bulk - match all
                Dim ref = pList(0).LineNo

                For Each itm In pList
                    If Not markers.Contains(itm.Allocation) Then
                        itm.MarkAsAllocated("Y", ref, SmartPeriod.CurrentPeriod.DBValue)
                    End If
                Next

            Else
                Dim theDebits = (From info In pList Where Not markers.Contains(info.Allocation) AndAlso info.DC.Equals("D", StringComparison.OrdinalIgnoreCase) _
                              Order By info.LineNo).ToList
                Dim theCredits = (From info In pList Where Not markers.Contains(info.Allocation) AndAlso Not info.DC.Equals("D", StringComparison.OrdinalIgnoreCase) _
                    Order By info.LineNo).ToList

                'match one to one 
                For Each Debit In theDebits
                    If Not markers.Contains(Debit.Allocation) AndAlso Not Debit.Allocation.Equals("Y", StringComparison.OrdinalIgnoreCase) Then
                        MatchBaseAmountOneDebitToOneCredit(theCredits, Debit)
                    End If
                Next

                'match one to many credit
                For Each Debit In theDebits
                    If Not markers.Contains(Debit.Allocation) AndAlso Not Debit.Allocation.Equals("Y", StringComparison.OrdinalIgnoreCase) Then
                        MatchBaseAmountOneDebitToManyCredit(theCredits, Debit)
                    End If
                Next

                'match one to many debit
                For Each Credit In theCredits
                    If Not markers.Contains(Credit.Allocation) AndAlso Not Credit.Allocation.Equals("Y", StringComparison.OrdinalIgnoreCase) Then
                        MatchBaseAmountOneCreditToManyDebit(theDebits, Credit)
                    End If
                Next
            End If

            PostAllocated(pList)

        End Sub

        Private Shared Sub MatchBaseAmountOneDebitToOneCredit(pCredits As List(Of MCLDGInfo), pDebit As MCLDGInfo)
            Dim markers = AllocatedMarkers()
            For Each credit In pCredits
                If Not markers.Contains(credit.Allocation) AndAlso Not credit.Allocation.Equals("Y", StringComparison.OrdinalIgnoreCase) Then
                    If Math.Abs(credit.Amount).RoundBA = Math.Abs(pDebit.Amount).RoundBA Then
                        Dim ref = pDebit.LineNo.ToString
                        pDebit.MarkAsAllocated("Y", ref, credit.Period)
                        credit.MarkAsAllocated("Y", ref, credit.Period)
                        Exit Sub
                    End If
                End If
            Next
        End Sub

        Private Shared Sub MatchBaseAmountOneDebitToManyCredit(pCredits As List(Of MCLDGInfo), pDebit As MCLDGInfo)
            Dim markers = AllocatedMarkers()
            Dim cummulated As Decimal = 0
            Dim CreditLinesInProgress = New List(Of MCLDGInfo)

            For Each credit In pCredits
                If Not markers.Contains(credit.Allocation) AndAlso Not credit.Allocation.Equals("Y", StringComparison.OrdinalIgnoreCase) Then
                    cummulated += credit.Amount
                    CreditLinesInProgress.Add(credit)

                    If Math.Abs(cummulated).RoundBA = Math.Abs(pDebit.Amount).RoundBA Then
                        Dim ref = pDebit.LineNo.ToString
                        pDebit.MarkAsAllocated("Y", ref, credit.Period)
                        For Each line In CreditLinesInProgress
                            line.MarkAsAllocated("Y", ref, credit.Period)
                        Next
                        Exit Sub
                    End If

                End If
            Next
        End Sub

        Private Shared Sub MatchBaseAmountOneCreditToManyDebit(pDebits As List(Of MCLDGInfo), pCredit As MCLDGInfo)
            Dim markers = AllocatedMarkers()
            Dim cummulated As Decimal = 0
            Dim DebitLinesInProgress = New List(Of MCLDGInfo)

            For Each debit In pDebits
                If Not markers.Contains(debit.Allocation) AndAlso Not debit.Allocation.Equals("Y", StringComparison.OrdinalIgnoreCase) Then
                    cummulated += debit.Amount
                    DebitLinesInProgress.Add(debit)

                    If Math.Abs(cummulated).RoundBA = Math.Abs(pCredit.Amount).RoundBA Then
                        Dim ref = pCredit.LineNo.ToString
                        pCredit.MarkAsAllocated("Y", ref, pCredit.Period)
                        For Each line In DebitLinesInProgress
                            line.MarkAsAllocated("Y", ref, pCredit.Period)
                        Next
                        Exit Sub
                    End If

                End If
            Next
        End Sub

#End Region

        Class Query

            Shared Function GetPatientBalanceAtDate(pPatient As String, Optional pDate As String = "", Optional pCol As String = "") As Decimal
                Dim _qd = OnePatientBalanceAtDate(pPatient, pDate, pCol)
                Dim ret = SQLCommander.GetScalarDecimal(_qd.BuildSQL)
                Return ret
            End Function

            ''' <summary>
            ''' Return table with 3 columns :Patient Code, D_C , Amount
            ''' </summary>
            ''' <param name="pDate"></param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Private Shared Function OnePatientBalanceAtDate(pPatientCode As String, ByVal pDate As String, Optional pCol As String = "") As QD

                Dim _QD As QD = QD.SysNewQD("MCLDGSB")
                _QD.AnalQ0 = "MCLDG"
                _QD.Descriptn = "Clinic Ledger"

                'output
                If String.IsNullOrEmpty(pCol) Then
                    _QD.Selected.AddField(New pbs.BO.SQLBuilder.Field("SUM", "MCLDG\AMOUNT", "", ""))
                Else
                    Dim f As Field = Nothing
                    If SelectionList.ContainsField("MCLDG", String.Format("MCLDG\{0}", pCol.ToUpper), f) Then
                        _QD.Selected.AddField(New pbs.BO.SQLBuilder.Field("SUM", f.TreeCode, "", ""))
                    Else
                        _QD.Selected.AddField(New pbs.BO.SQLBuilder.Field("SUM", "MCLDG\AMOUNT", "", ""))
                    End If
                End If

                'filters
                Dim theDateFilter = New pbs.BO.SQLBuilder.Field("", "MCLDG\TRANS_DATE", "Calculation Date", "SDN")
                theDateFilter.FilterFrom = "!"
                theDateFilter.FilterTo = pDate
                _QD.AddFilter(theDateFilter)

                'filters
                Dim theStdFilter = New pbs.BO.SQLBuilder.Field("", "MCLDG\PATIENT_CODE", "Patient Code", "")
                theStdFilter.FilterFrom = Nz(pPatientCode, "!")
                _QD.AddFilter(theStdFilter)


                Return _QD

            End Function

            ''' <summary>
            ''' Return table with 3 columns :Patient Code, D_C , Amount
            ''' </summary>
            ''' <param name="pDate"></param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Shared Function BuildPatientBalanceAtDate(ByVal pDate As String) As QD

                Dim _QD As QD = QD.SysNewQD("MCLDG")
                _QD.AnalQ0 = "MCLDG"
                _QD.Descriptn = "Clinic Ledger"
                'output
                _QD.Selected.AddField(New pbs.BO.SQLBuilder.Field("", "MCLDG\PATIENT_CODE", "", ""))
                _QD.Selected.AddField(New pbs.BO.SQLBuilder.Field("", "MCLDG\D_C", "", ""))
                _QD.Selected.AddField(New pbs.BO.SQLBuilder.Field("SUM", "MCLDG\AMOUNT", "", ""))

                'filters
                Dim theDateFilter = New pbs.BO.SQLBuilder.Field("", "MCLDG\TRANS_DATE", "Calculation Date", "SDN")
                theDateFilter.FilterFrom = "!"
                theDateFilter.FilterTo = pDate
                theDateFilter._forcedAND = True
                theDateFilter._IsFixFilter = True

                _QD.AddFilter(theDateFilter)

                Return _QD

            End Function

            Public Shared Function BuildQDByDic(ByVal dic As Dictionary(Of String, String)) As QD

                Dim _QD As QD = QD.SysNewQD("MCLDG")

                MakeUp_Query(_QD)

                If dic IsNot Nothing Then
                    'If dic.ContainsKey("SubscriptionId") Then dic.Merge("NcSl9", dic("SubscriptionId"))
                    If dic.ContainsKey("DueDate") Then dic.Merge("ExtDate1", dic("DueDate"))
                End If

                _QD.AddFilterDictionary(dic)

                If _QD.Filters.Count = 0 Then
                    ' no filters - no record
                    _QD.AddFilter("MCLDG\LINE_NO", "", "<ALL>")
                End If

                Return _QD
            End Function

            Private Shared Sub MakeUp_Query(ByRef _QD As QD)

                'Build Definition
                If _QD Is Nothing Then Exit Sub
                If _QD.DAG <> "SYSTM" Then _QD.Descriptn = "System Query - MC ledger"
                _QD.AnalQ0 = "MCLDG"
                _QD.Share = False

                'Build Output

                _QD.Selected.AddField(New pbs.BO.SQLBuilder.Field("", "*", "", ""))

            End Sub

        End Class

    End Class

End Namespace