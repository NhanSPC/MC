Imports pbs.BO.Script
Imports pbs.Helper.Interfaces
Imports pbs.Helper
Imports System.Text.RegularExpressions

Namespace MC
    ''' <summary>
    ''' Detail Patient inquiry. Called form Patient balance
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PatientStatement
        Implements IQueryBO
        Implements ISupportScripts
        Implements ISupportQueryInfoList

        Private _PatientCode As String = String.Empty
        Private _calculationDate As String = "T"
        Private _userFilters As New Dictionary(Of String, String)

#Region "IQueryBO"
        Public Function Syntax() As String Implements IQueryBO.Syntax
            Return Nothing
        End Function

        Public Sub AddQueryFilters(pDic As Dictionary(Of String, String)) Implements IQueryBO.AddQueryFilters
            If pDic IsNot Nothing Then _userFilters = pDic
        End Sub

        Public Function CanExcecute(cmd As String) As Boolean? Implements Interfaces.ISupportCommandAuthorization.CanExecute
            Dim theCmd = Regex.Replace(cmd, "^act_", String.Empty)
            Return pbs.UsrMan.Permission.isPermited(String.Format("{0}.{1}", Me.GetType.ToString, theCmd))
        End Function

        Public ReadOnly Property ChildrenType As Type Implements IQueryBO.ChildrenType
            Get
                Return GetType(pbs.BO.MC.MCLDGInfo)
            End Get
        End Property

        Public Function GetBOList() As IList Implements IQueryBO.GetBOList
            Dim theFilters = _userFilters.CloneDic

            If String.IsNullOrEmpty(_PatientCode) Then pbs.Helper.UIServices.AlertService.Alert(ResStr("You need to select a Patient in order to run Patient Inquiry"))

            If pbs.BO.MC.Settings.GetSettings.UseFirstDueFirstPay Then
                Return pbs.BO.MC.MCLDGInfoList.GetFirstDueFirstPayMCLDG(_PatientCode, _calculationDate, theFilters)
            Else
                Return pbs.BO.MC.MCLDGInfoList.GetPatientDetail(_PatientCode, _calculationDate, theFilters)
            End If
            _selectedLines = Nothing
        End Function

        Public Sub InvalidateCacheList() Implements IQueryBO.InvalidateCacheList
            '_list = Nothing
        End Sub

        Public Function GetBOListStatus() As String Implements IQueryBO.GetBOListStatus
            Return String.Empty
        End Function

        Public Function GetDoubleClickCommand() As String Implements IQueryBO.GetDoubleClickCommand
            Return String.Format("{0}?LineNo=[LineNo]&$Action=Examine", GetType(pbs.BO.MC.MCLDG).ToString)
        End Function

        Public Function GetMyQD() As SQLBuilder.QD Implements IQueryBO.GetMyQD
            Return Nothing
        End Function
        Public Sub UpdateQD(pQD As SQLBuilder.QD) Implements IQueryBO.UpdateQD
            '_qd = pQD
        End Sub
        Public Function GetMyTitle() As String Implements IQueryBO.GetMyTitle
            Return String.Format("Statement: {0}", _PatientCode)
        End Function

        Public Function GetSelectCommand() As String Implements IQueryBO.GetSelectCommand
            Return String.Empty
        End Function

        Public Function GetSelectionChangedCommand() As String Implements IQueryBO.GetSelectionChangedCommand
            Return String.Empty
        End Function

        Public Function GetVariables() As Dictionary(Of String, String) Implements IQueryBO.GetVariables
            Dim dic = _userFilters.CloneDic
            dic = dic.Merge("PatientCode", _PatientCode)
            Return dic
        End Function

        Public Sub SetParameters(pDic As Dictionary(Of String, String)) Implements IQueryBO.SetParameters
            _PatientCode = pDic.GetValueByKey("PatientCode", _PatientCode)
            _calculationDate = pDic.GetValueByKey("CalculationDate", _calculationDate)
            _currentLineNo = pDic.GetValueByKey("CurrentObjectId", _currentLineNo)
        End Sub

        Private _currentLineNo As String = String.Empty

        Private _selectedLines As IEnumerable
        Public Sub UpdateSelectedLines(pLines As IEnumerable) Implements IQueryBO.UpdateSelectedLines
            _selectedLines = pLines
        End Sub
        Private Sub UpdateCurrentLine(pLine As Object) Implements IQueryBO.UpdateCurrentLine
        End Sub
        Public Sub ZReset() Implements IQueryBO.ZReset
        End Sub
#End Region

#Region "ISupportScripts"
        Private Shared Function GetSurvey() As String
            Dim SurveyXml = <survey>
                                <SURVEY_ID>PatientStatementSurvey</SURVEY_ID>
                                <TITLE>Please enter filter values</TITLE>
                                <NAME>Selection criteria</NAME>
                                <Questions>
                                    <question>
                                        <LINE_NO>001</LINE_NO>
                                        <SORTING_NO>01</SORTING_NO>
                                        <DATASOURCE_URL>PatientBalance</DATASOURCE_URL>
                                        <MULTI_VALUE></MULTI_VALUE>
                                        <QUESTION_TYPE></QUESTION_TYPE>
                                        <GROUP_NAME></GROUP_NAME>
                                        <QUESTION>Please enter one Patient code</QUESTION>
                                        <EXPLANATION></EXPLANATION>
                                        <ANSWER_OPTIONS></ANSWER_OPTIONS>
                                    </question>
                                    <question>
                                        <LINE_NO>002</LINE_NO>
                                        <SORTING_NO>02</SORTING_NO>
                                        <DATASOURCE_URL>Calendar</DATASOURCE_URL>
                                        <MULTI_VALUE></MULTI_VALUE>
                                        <QUESTION_TYPE></QUESTION_TYPE>
                                        <GROUP_NAME></GROUP_NAME>
                                        <QUESTION>Please enter the date</QUESTION>
                                        <EXPLANATION>Patient liabilities status will be calculated till this date</EXPLANATION>
                                        <ANSWER_OPTIONS></ANSWER_OPTIONS>
                                    </question>
                                </Questions>

                            </survey>.ToString
            Return SurveyXml
        End Function

        Private Sub AskUser()

            Dim theDic = New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
            theDic.Add("Q001", _PatientCode)
            theDic.Add("Q002", _calculationDate)

            Dim askResult = pbs.Helper.UIServices.SurveyService.SurveyByXML(GetSurvey, theDic)

            _PatientCode = askResult.GetValueByKey("Q001", _PatientCode)
            _calculationDate = askResult.GetValueByKey("Q002", _calculationDate)

        End Sub

        Private Function Select_Imp() As UITasks

            Dim scripts = New Script.UITasks(Me)

            scripts.IconName = "monoPatient"
            scripts.CaptionKey = "-"
            scripts.RefreshUIWhenFinish = True

            scripts.AddCallMethod(1, "AskUser") ' set journal datatable to param slot #1

            Return scripts

        End Function

        Private Sub MatchBaseAmount()
            Dim markers = MCLDGInfoList.AllocatedMarkers()

            Dim SumOfSelected As Decimal = 0
            Dim theList = New List(Of MCLDGInfo)
            If _selectedLines IsNot Nothing Then
                For Each info As MCLDGInfo In _selectedLines

                    If Not markers.Contains(info.Allocation) Then
                        theList.Add(info)
                        SumOfSelected += info.Amount.RoundBA
                    End If

                Next
            End If

            If theList.Count > 0 Then

                If SumOfSelected = 0 Then
                    MCLDGInfoList.AllocationMatchingByPatientCodeOnly(theList)
                Else
                    MCLDGInfoList.AllocationMatching(theList)
                End If

            Else
                pbs.Helper.UIServices.AlertService.Alert(ResStr("Please select at least 2 lines for matching"))
            End If
        End Sub

        Private Function MatchBaseAmount_Imp() As UITasks

            Dim scripts = New Script.UITasks(Me)

            scripts.IconName = "monoMatch"
            scripts.CaptionKey = "Match"
            scripts.RefreshUIWhenFinish = True

            scripts.AddCallMethod(1, "MatchBaseAmount") ' set journal datatable to param slot #1

            Return scripts

        End Function

        Private Sub UnMatch()

            Dim theList = New List(Of MCLDGInfo)
            For Each info In _selectedLines
                theList.Add(info)
            Next

            If theList.Count > 0 Then
                MCLDGInfoList.UnMatch(theList)
            Else
                pbs.Helper.UIServices.AlertService.Alert("Please select at least 2 lines for matching")
            End If
        End Sub

        Private Function RemoveAllocation_Imp() As UITasks

            Dim scripts = New Script.UITasks(Me)

            scripts.IconName = "monoUnMatch"
            scripts.CaptionKey = "RemoveAllocation"
            scripts.ButtonLocation = ScriptButtonLocation.Navigation_Right
            scripts.RefreshUIWhenFinish = True

            scripts.AddCallMethod(1, "UnMatch") ' set journal datatable to param slot #1

            Return scripts

        End Function

        Private Shared Function Allocated() As List(Of String)
            Return New String() {"A", "C", "P"}.ToList
        End Function

        Private Sub Collect()

            If _selectedLines Is Nothing Then Exit Sub

            Dim theList = New List(Of MCLDGInfo)

            Dim SelectDebit As Boolean = False
            Dim SelectAllocated As Boolean = False

            For Each info As MCLDGInfo In _selectedLines
                If Not Allocated.Contains(info.Allocation) Then
                    If info.DC = "D" Then
                        theList.Add(info)
                    Else
                        SelectDebit = True
                    End If
                Else
                    SelectAllocated = True
                End If
            Next

            If SelectDebit OrElse SelectAllocated Then pbs.Helper.UIServices.AlertService.Alert(ResStr("Please select debit and un-allocated transactions for collection journal"))

            If theList.Count = 0 AndAlso Not SelectDebit AndAlso Not SelectAllocated Then
                pbs.Helper.UIServices.AlertService.Alert(ResStr("Please select debit and un-allocated transactions for collection journal"))
            Else
                Dim theJournal = pbs.BO.MC.JE.CreateCollectionJournal(theList)

                If theJournal IsNot Nothing Then
                    Dim args = New pbsCmdArgs(theJournal.GetURLShortCut)
                    args._bo = theJournal
                    args.Action = pbs.Helper.Action._Create

                    pbs.Helper.UIServices.RunURLService.Run(args)

                Else
                    pbs.Helper.UIServices.AlertService.Alert(ResStr("Please select debit and un-allocated transactions for collection journal"))
                End If

            End If

        End Sub

        Private Function Collect_Imp() As UITasks

            Dim scripts = New Script.UITasks(Me)

            scripts.IconName = "monoMoney"
            scripts.CaptionKey = "Collect"
            scripts.RefreshUIWhenActive = True

            scripts.AddCallMethod(1, "Collect")

            Return scripts

        End Function

        Private Sub Refund()

            If _selectedLines Is Nothing Then Exit Sub

            Dim theList = New List(Of MCLDGInfo)

            Dim SelectCredit As Boolean = False
            Dim SelectAllocated As Boolean = False

            For Each info As MCLDGInfo In _selectedLines
                If Not Allocated.Contains(info.Allocation) Then
                    If info.DC = "C" Then
                        theList.Add(info)
                    Else
                        SelectCredit = True
                    End If
                Else
                    SelectAllocated = True
                End If
            Next

            If SelectCredit OrElse SelectAllocated Then pbs.Helper.UIServices.AlertService.Alert(ResStr("Please select credit and un-allocated transactions for refund journal"))

            If theList.Count = 0 AndAlso Not SelectCredit AndAlso Not SelectAllocated Then
                pbs.Helper.UIServices.AlertService.Alert(ResStr("Please select credit and un-allocated transactions for refund journal"))
            Else
                Dim theJournal = pbs.BO.MC.JE.CreateRefundJournal(theList)

                If theJournal IsNot Nothing Then
                    Dim args = New pbsCmdArgs(theJournal.GetURLShortCut)
                    args._bo = theJournal
                    args.Action = pbs.Helper.Action._Create

                    pbs.Helper.UIServices.RunURLService.Run(args)

                Else
                    pbs.Helper.UIServices.AlertService.Alert(ResStr("Please select debit and un-allocated transactions for collection journal"))
                End If

            End If

        End Sub

        Private Function Refund_Imp() As UITasks

            Dim scripts = New Script.UITasks(Me)

            scripts.IconName = "monoMoney"
            scripts.CaptionKey = "Refund"
            scripts.RefreshUIWhenActive = True

            scripts.AddCallMethod(1, "Refund")

            Return scripts

        End Function

        Private Function Split_Imp() As UITasks

            Dim scripts = New Script.UITasks(Me)

            scripts.IconName = "monoShare"
            scripts.CaptionKey = "Split"
            scripts.ButtonLocation = ScriptButtonLocation.Navigation_Right

            scripts.RefreshUIWhenFinish = True

            scripts.AddCallMethod(1, "Split")

            Return scripts

        End Function

        Private Sub Split()
            If _currentLineNo.ToInteger > 0 Then
                Dim theOrigin = MCLDG.GetMCLDG(_currentLineNo)
                Dim dic = New Dictionary(Of String, String)
                dic.Add("$LayoutCode", "MCLDGSplit")
                dic.Add("$Columns", "LineNo,(Description),(Amount)")
                Dim ret = pbs.Helper.UIServices.SplitValueService.Spit(Of MCLDG)(theOrigin, "Amount", dic)

                theOrigin.SplitByAmountTo(ret)

            End If
        End Sub

        Public Function GetScriptDictionary() As Dictionary(Of String, Script.UITasks) Implements ISupportScripts.GetScriptDictionary
            Dim _scripts = New Dictionary(Of String, UITasks)(StringComparer.OrdinalIgnoreCase)

            _scripts.Add(pbs.Helper.Action._Select, Select_Imp)

            _scripts.Add("Collect", Collect_Imp)
            _scripts.Add("Refund", Refund_Imp)

            _scripts.Add(pbs.Helper.Action._MatchBaseAmount, MatchBaseAmount_Imp)

            _scripts.Add("RemoveAllocation", RemoveAllocation_Imp)

            _scripts.Add("Split", Split_Imp)

            Return _scripts
        End Function

#End Region

#Region "ISupportQueryInfoList"

        Public Function GetInfoList(pFilter As Dictionary(Of String, String)) As IEnumerable Implements ISupportQueryInfoList.GetInfoList
            _PatientCode = pFilter.GetValueByKey("PatientCode", _PatientCode)
            _calculationDate = pFilter.GetValueByKey("CalculationDate", "99990101")
            Dim _showPendingOnly = pFilter.GetSystemKey("$PendingOnly", "N").ToBoolean

            _userFilters = pFilter

            Dim theList = GetBOList()
            If _showPendingOnly Then
                Return (From info As MCLDGInfo In theList Where _
                     Not info.PaymentStatus.Equals("PAID", StringComparison.OrdinalIgnoreCase) _
                     OrElse info.OverPaid > 0).ToList
            Else
                Return theList
            End If

        End Function

#End Region


    End Class

End Namespace
