Imports pbs.BO.Script
Imports pbs.Helper.Interfaces
Imports pbs.BO.DataAnnotations
Imports pbs.BO.Data
Imports pbs.Helper

Namespace MC

    ''' <summary>
    ''' Overdue payments or Patient
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PatientAgingOverdue
        Implements IQueryBO
        Implements ISupportScripts
        Implements ISupportQueryInfoList
        Implements ISupportDynamicCaption

        <Serializable>
        Class PatientOverdue
            Implements IInfo

            Property CalculationDate As String

            Property PatientCode As String
            Property PatientName As String = String.Empty
            Property Campus As String = String.Empty
            Property ClassId As String = String.Empty

            Property DueAmount As Decimal
            Property Band_01 As Decimal
            Property Band_02 As Decimal
            Property Band_03 As Decimal
            Property Band_04 As Decimal
            Property Band_05 As Decimal

            Property UnDue As Decimal

            ReadOnly Property TotalValue As Decimal
                Get
                    Return DueAmount + Band_01 + Band_02 + Band_03 + Band_04 + Band_05 + UnDue
                End Get
            End Property

            Public ReadOnly Property Code As String Implements IInfo.Code
                Get
                    Return PatientCode
                End Get
            End Property

            Public ReadOnly Property Description As String Implements IInfo.Description
                Get
                    Return PatientName
                End Get
            End Property

            Public Sub InvalidateCache() Implements IInfo.InvalidateCache

            End Sub

            Public ReadOnly Property LookUp As String Implements IInfo.LookUp
                Get
                    Return Campus
                End Get
            End Property

            Public Function IsOverdued() As Boolean
                Return Band_01 <> 0 OrElse Band_02 <> 0 OrElse Band_03 <> 0 OrElse Band_04 <> 0 OrElse Band_05 <> 0
            End Function

            Public Function GetTotalOverdue() As Decimal
                Return Band_01 + Band_02 + Band_03 + Band_04 + Band_05
            End Function
        End Class

        Private _calculationDate As New pbs.Helper.SmartDate("T")

        Private clientND = String.Empty

        Public _band01 As Integer = 30
        Public _band02 As Integer = 60
        Public _band03 As Integer = 90
        Public _band04 As Integer = 120
        Public _band05 As Integer = 150

#Region "IBOQuery"
        Public Function Syntax() As String Implements IQueryBO.Syntax
            Return Nothing
        End Function

        Private _userFilters As Dictionary(Of String, String)
        Public Sub AddQueryFilters(pDic As Dictionary(Of String, String)) Implements IQueryBO.AddQueryFilters
            _userFilters = pDic
            Dim settings = pbs.BO.MC.Settings.GetSettings

            _band01 = pDic.GetValueByKey("Band01", Nz(settings.AgingBand01, 30)).ToInteger
            _band02 = pDic.GetValueByKey("Band02", Nz(settings.AgingBand02, 60)).ToInteger
            _band03 = pDic.GetValueByKey("Band03", Nz(settings.AgingBand03, 90)).ToInteger
            _band04 = pDic.GetValueByKey("Band04", Nz(settings.AgingBand04, 120)).ToInteger
            _band05 = pDic.GetValueByKey("Band05", Nz(settings.AgingBand05, 150)).ToInteger
        End Sub

        Public Function CanExcecute(cmd As String) As Boolean? Implements Interfaces.ISupportCommandAuthorization.CanExecute
            Return pbs.UsrMan.Permission.isPermited(Me.GetType.ToString)
        End Function

        Public ReadOnly Property ChildrenType As Type Implements IQueryBO.ChildrenType
            Get
                Return GetType(PatientOverdue)
            End Get
        End Property

        Public Function GetBOList() As IList Implements IQueryBO.GetBOList

            Dim theDic = New Dictionary(Of String, PatientOverdue)(StringComparer.OrdinalIgnoreCase)

            Dim ForecastQD = CreatePatientAgingQD()
            AddFilters(ForecastQD)
            Dim data = ForecastQD.Extract

            FillToBands(data, theDic)

            Dim ret = theDic.Values.ToList

            For Each itm In ret
                Dim info As pbs.BO.MC.PATIENTInfo = Nothing
                If pbs.BO.MC.PATIENTInfoList.ContainsCode(itm.PatientCode, info) Then
                    itm.PatientName = info.Description
                End If
            Next

            Return ret
        End Function

        Public Sub InvalidateCacheList() Implements IQueryBO.InvalidateCacheList
            '_list = Nothing
        End Sub

        Public Function GetBOListStatus() As String Implements IQueryBO.GetBOListStatus
            Return String.Empty
        End Function

        Public Function GetDoubleClickCommand() As String Implements IQueryBO.GetDoubleClickCommand
            Return String.Format("{0}?PatientCode=[PatientCode]&CalculationDate=[CalculationDate]", GetType(pbs.BO.MC.PatientStatement).ToString)
        End Function

        Public Function GetMyQD() As SQLBuilder.QD Implements IQueryBO.GetMyQD
            Return Nothing
        End Function
        Public Sub UpdateQD(pQD As SQLBuilder.QD) Implements IQueryBO.UpdateQD
            '_qd = pQD
        End Sub
        Public Function GetMyTitle() As String Implements IQueryBO.GetMyTitle
            Return String.Format(ResStr("Payment overdue at date: {0}"), _calculationDate.Text)
        End Function

        Public Function GetSelectCommand() As String Implements IQueryBO.GetSelectCommand
            Return String.Empty
        End Function

        Public Function GetSelectionChangedCommand() As String Implements IQueryBO.GetSelectionChangedCommand
            Return String.Empty
        End Function

        Public Function GetVariables() As Dictionary(Of String, String) Implements IQueryBO.GetVariables
            Dim dic = New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
            dic.Add("CalculationDate", _calculationDate.DBValue)
            dic = dic.Merge(_userFilters, False)
            Return dic
        End Function

        Public Sub SetParameters(pDic As Dictionary(Of String, String)) Implements IQueryBO.SetParameters
            _calculationDate.Text = pDic.GetValueByKey("CalculationDate", _calculationDate.Text)
        End Sub

        Private _selectedLines As IEnumerable
        Public Sub UpdateSelectedLines(pLines As IEnumerable) Implements IQueryBO.UpdateSelectedLines
            _selectedLines = pLines
        End Sub
        Private Sub UpdateCurrentLine(pLine As Object) Implements IQueryBO.UpdateCurrentLine
        End Sub
        Public Sub ZReset() Implements IQueryBO.ZReset
            'update account balance in CA ?
        End Sub
#End Region

#Region "Build the list"

        Private Function GetPatientWithCreditBalance() As List(Of String)
            Dim QD = SQLBuilder.QD.NewQD("Closing")

            QD.Descriptn = "Closing"
            QD.AnalQ0 = "SMLDG"

            QD.AddSelectedField(New pbs.BO.SQLBuilder.Field("", "SMLDG\Patient_CODE", "", ""))
            QD.AddSelectedField(New pbs.BO.SQLBuilder.Field("SUM", "SMLDG\AMOUNT", "", ""))


            QD.Filters.Clear()

            Dim dic = New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
            dic.Add("Allocation", "not in (A,P,C)")

            If _userFilters IsNot Nothing AndAlso _userFilters.ContainsKey("PatientCode") Then
                dic.Add("PatientCode", _userFilters.GetValueByKey("PatientCode", "!"))
            End If

            dic = dic.Merge(_userFilters)

            QD.AddFilterDictionary(dic)

            Dim ret = New List(Of String)

            Dim dt = QD.Extract
            For Each dr As DataRow In dt.Rows
                If DNz(dr(1), 0) >= 0 Then
                    ret.Add(DNz(dr(0), String.Empty).Trim)
                End If
            Next

            Return ret
        End Function

        Private Function CreatePatientAgingQD() As pbs.BO.SQLBuilder.QD
            Dim ClosingQD = SQLBuilder.QD.NewQD("Closing")
            ClosingQD.Descriptn = "Closing"
            ClosingQD.AnalQ0 = "SMLDG"

            ClosingQD.AddSelectedField(New pbs.BO.SQLBuilder.Field("", "SMLDG\Patient_CODE", "", ""))
            ClosingQD.AddSelectedField(New pbs.BO.SQLBuilder.Field("", "SMLDG\TRANS_DATE", "", ""))
            ClosingQD.AddSelectedField(New pbs.BO.SQLBuilder.Field("", "SMLDG\EXT_DATE1", "", ""))

            ClosingQD.AddSelectedField(New pbs.BO.SQLBuilder.Field("SUM", "SMLDG\AMOUNT", "", ""))

            Return ClosingQD
        End Function

        Private Sub AddFilters(QD As SQLBuilder.QD)
            QD.Filters.Clear()

            Dim dic = New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
            dic.Add("Allocation", "not in (A,P,C)")

            dic.Add("TransDate", String.Format("<<19900101..{0}", _calculationDate.DBValueInt))

            If _userFilters IsNot Nothing AndAlso _userFilters.ContainsKey("PatientCode") Then
                dic.Add("PatientCode", _userFilters.GetValueByKey("PatientCode", "!"))
            End If

            dic = dic.Merge(_userFilters)

            QD.AddFilterDictionary(dic)

        End Sub

        Private Sub FillToBands(ByVal pData As DataTable, pDic As Dictionary(Of String, PatientOverdue))

            If pDic Is Nothing Then pDic = New Dictionary(Of String, PatientOverdue)(StringComparer.OrdinalIgnoreCase)

            Dim CreditBalancePatients = GetPatientWithCreditBalance()

            For Each row As DataRow In pData.Rows

                Dim thePatientCode = DNz(row("Patient_CODE"), String.Empty)

                If CreditBalancePatients.Contains(thePatientCode) Then Continue For

                Dim ab As PatientOverdue = Nothing
                If Not pDic.ContainsKey(thePatientCode.Trim) Then
                    ab = New PatientOverdue
                    ab.PatientCode = thePatientCode
                    ab.CalculationDate = _calculationDate.Text
                    pDic.Add(thePatientCode, ab)
                Else
                    ab = pDic(thePatientCode)
                End If

                Dim transDate = New pbs.Helper.SmartDate(DNz(row.Item("TRANS_DATE"), String.Empty))
                Dim dueDate = New pbs.Helper.SmartDate(DNz(row.Item("EXT_DATE1"), String.Empty))
                If String.IsNullOrEmpty(dueDate.Text) Then dueDate.Date = transDate.Date

                Dim theAmount = New pbs.Helper.SmartFloat(DNz(row.Item("AMOUNT"), 0))

                If _band05 > 0 AndAlso dueDate.DBValueInt <= _calculationDate.Date.AddDays(_band05 * -1).ToSunDate Then
                    ab.Band_05 += theAmount.Float.RoundBA
                ElseIf _band04 > 0 AndAlso dueDate.DBValueInt.ToString <= _calculationDate.Date.AddDays(_band04 * -1).ToSunDate Then
                    ab.Band_04 += theAmount.Float.RoundBA
                ElseIf _band03 > 0 AndAlso dueDate.DBValueInt.ToString <= _calculationDate.Date.AddDays(_band03 * -1).ToSunDate Then
                    ab.Band_03 += theAmount.Float.RoundBA
                ElseIf _band02 > 0 AndAlso dueDate.DBValueInt.ToString <= _calculationDate.Date.AddDays(_band02 * -1).ToSunDate Then
                    ab.Band_02 += theAmount.Float.RoundBA
                ElseIf _band01 > 0 AndAlso dueDate.DBValueInt.ToString <= _calculationDate.Date.AddDays(_band01 * -1).ToSunDate Then
                    ab.Band_01 += theAmount.Float.RoundBA
                ElseIf dueDate.DBValueInt <= _calculationDate.DBValueInt Then
                    ab.DueAmount += theAmount.Float.RoundBA
                Else
                    ab.UnDue += theAmount.Float.RoundBA
                End If


            Next

        End Sub

#End Region

#Region "ISupportScripts"

        Private Sub AskUser()

            'Dim newPeriod = pbs.Helper.UIServices.ValueSelectorService.SelectValue(LinkCode.Period, String.Format(ResStr(ResStrConst.SelectOneItemFromList), ResStr("Period")), String.Empty)

            Dim newPeriod = pbs.Helper.UIServices.ValueSelectorService.SelectValue(LinkCode.Calendar, ResStrConst.SelectItemText("Date"), String.Empty)

            If Not newPeriod = String.Empty Then
                _calculationDate.Text = newPeriod
            End If

        End Sub

        Private Sub PreviousPeriod()
            _calculationDate.Date = _calculationDate.Date.AddMonths(-1)
        End Sub
        Private Sub NextPeriod()
            _calculationDate.Date = _calculationDate.Date.AddMonths(1)
        End Sub

        Private Function Previous_Imp() As UITasks
            Dim scripts = New Script.UITasks(Me)

            scripts.IconName = "Arrows/Prev"
            scripts.CaptionKey = "-"
            scripts.RefreshUIWhenFinish = True

            scripts.AddCallMethod(1, "PreviousPeriod") ' set journal datatable to param slot #1

            Return scripts
        End Function

        Private Function Select_Imp() As UITasks

            Dim scripts = New Script.UITasks(Me)

            scripts.IconName = "Find/Find"
            scripts.CaptionKey = "-"
            scripts.RefreshUIWhenFinish = True

            scripts.AddCallMethod(1, "AskUser") ' set journal datatable to param slot #1

            Return scripts

        End Function

        Private Function Next_Imp() As UITasks

            Dim scripts = New Script.UITasks(Me)

            scripts.IconName = "Arrows/Next"
            scripts.CaptionKey = "-"
            scripts.RefreshUIWhenFinish = True

            scripts.AddCallMethod(1, "NextPeriod") ' set journal datatable to param slot #1

            Return scripts

        End Function

        Public Function GetScriptDictionary() As Dictionary(Of String, Script.UITasks) Implements ISupportScripts.GetScriptDictionary
            Dim _scripts = New Dictionary(Of String, UITasks)(StringComparer.OrdinalIgnoreCase)

            _scripts.Add(pbs.Helper.Action._Next, Next_Imp)
            _scripts.Add(pbs.Helper.Action._Select, Select_Imp)
            _scripts.Add(pbs.Helper.Action._Previous, Previous_Imp)
            Return _scripts

        End Function

#End Region

#Region "ISupportQueryInfoList"
        Public Function GetInfoList(pFilter As Dictionary(Of String, String)) As IEnumerable Implements ISupportQueryInfoList.GetInfoList
            Return GetBOList()
        End Function
#End Region

#Region "ISupportDynamicCaption"
        Public Function GetCaption(key As String) As String Implements ISupportDynamicCaption.GetCaption

            Select Case key
                Case "DueAmount"
                    Return "T=" & _calculationDate.Text
                Case "Band_01"
                    If _band01 > 0 Then Return "T-" & _band01
                Case "Band_02"
                    If _band02 > 0 Then Return "T-" & _band02
                Case "Band_03"
                    If _band03 > 0 Then Return "T-" & _band03
                Case "Band_04"
                    If _band04 > 0 Then Return "T-" & _band04
                Case "Band_05"
                    If _band05 > 0 Then Return "T-" & _band05
            End Select

            Return String.Empty
        End Function

        Public Function GetToolTips(key As String) As String Implements ISupportDynamicCaption.GetToolTips
            Select Case key
                Case "DueAmount"
                    Return "T=" & _calculationDate.Text
                Case "Band_01"
                    If _band01 > 0 Then Return String.Format("{0} days before {1}", _band01, _calculationDate.Text)
                Case "Band_02"
                    If _band02 > 0 Then Return String.Format("{0} days before {1}", _band02, _calculationDate.Text)
                Case "Band_03"
                    If _band03 > 0 Then Return String.Format("{0} days before {1}", _band03, _calculationDate.Text)
                Case "Band_04"
                    If _band04 > 0 Then Return String.Format("{0} days before {1}", _band04, _calculationDate.Text)
                Case "Band_05"
                    If _band05 > 0 Then Return String.Format("{0} days before {1}", _band05, _calculationDate.Text)
            End Select

            Return String.Empty
        End Function

        Public Function HighLight(key As String) As Boolean Implements ISupportDynamicCaption.HighLight
            Return False
        End Function
#End Region

    End Class

End Namespace

