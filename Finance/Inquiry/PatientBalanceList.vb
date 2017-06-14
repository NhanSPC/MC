Imports Csla.Core
Imports pbs.BO.DataAnnotations
Imports pbs.Helper.Interfaces
Imports pbs.BO.Script
Imports pbs.Helper

Namespace MC
    Public Class PatientBalanceList
        Implements IQueryBO
        Implements ISupportScripts
        Implements ISupportQueryInfoList

        Friend _reset As Boolean = False

        Private _calculationDate As String = "T"
        Private _list = New List(Of PatientBalance)

        Private _userFilters As Dictionary(Of String, String)

#Region "IQueryBO"

        Public Function Syntax() As String Implements IQueryBO.Syntax
            Return Nothing
        End Function

        Public Sub AddQueryFilters(pDic As Dictionary(Of String, String)) Implements IQueryBO.AddQueryFilters
            _userFilters = pDic
        End Sub

        Public Function CanExcecute(cmd As String) As Boolean? Implements Interfaces.ISupportCommandAuthorization.CanExecute
            Return pbs.UsrMan.Permission.isPermited(Me.GetType.ToString)
        End Function

        Public ReadOnly Property ChildrenType As Type Implements IQueryBO.ChildrenType
            Get
                Return GetType(PatientBalance)
            End Get
        End Property

        Private _listDate As String = String.Empty

        Public Function GetBOList() As IList Implements IQueryBO.GetBOList
            Try
                _listDate = _calculationDate

                pbs.Helper.UIServices.WaitingPanelService.Wait("Calculate Balance", String.Format("till date {0}", pbs.Helper.SmartDate.Parse(_calculationDate).Text))

                Dim _QD = pbs.BO.MC.MCLDGInfoList.Query.BuildPatientBalanceAtDate(_calculationDate)
                _QD.AddFilterDictionary(_userFilters)
                Dim dt = _QD.Extract

                Dim theOutput = New Dictionary(Of String, PatientBalance)

                For Each row As DataRow In dt.Rows
                    Dim sb = PatientBalance.Fetch(row.DataRow2Dic)
                    sb._calculationDate.Text = _calculationDate

                    'Dim theKey = String.Format("{0}.{1}", sb._PatientCode, sb._candidateId)
                    Dim theKey = sb.PatientCode

                    If theOutput.ContainsKey(theKey) Then
                        Dim bl = theOutput(theKey)
                        bl._debit += sb.Debit
                        bl._credit += sb.Credit
                    Else
                        theOutput.Add(theKey, sb)
                    End If
                Next

                _list = theOutput.Values.ToList

            Catch ex As Exception
                pbs.Helper.UIServices.AlertService.ShowError(ex)
            Finally
                pbs.Helper.UIServices.WaitingPanelService.Done()
            End Try

            Return _list

        End Function

        Public Sub InvalidateCacheList() Implements IQueryBO.InvalidateCacheList
            '_list = Nothing
        End Sub

        Public Function GetBOListStatus() As String Implements IQueryBO.GetBOListStatus
            Return String.Empty
        End Function

        Public Function GetDoubleClickCommand() As String Implements IQueryBO.GetDoubleClickCommand
            Return "pbs.BO.MC.PatientStatement/#layout#?PatientCode=[PatientCode]&CandidateId=[CandidateId]&CalculationDate=[CalculationDate]"
        End Function

        Public Function GetMyQD() As SQLBuilder.QD Implements IQueryBO.GetMyQD
            Return Nothing
        End Function
        Public Sub UpdateQD(pQD As SQLBuilder.QD) Implements IQueryBO.UpdateQD
            '_qd = pQD
        End Sub
        Public Function GetMyTitle() As String Implements IQueryBO.GetMyTitle
            Return ResStr("Patient Balance")
        End Function

        Public Function GetSelectCommand() As String Implements IQueryBO.GetSelectCommand
            Return String.Empty
        End Function

        Public Function GetSelectionChangedCommand() As String Implements IQueryBO.GetSelectionChangedCommand
            Return String.Empty
        End Function

        Public Function GetVariables() As Dictionary(Of String, String) Implements IQueryBO.GetVariables
            Dim ret = New Dictionary(Of String, String)
            ret.Add("CalculationDate", pbs.Helper.SmartDate.Parse(_calculationDate).DateViewFormat)
            Return ret
        End Function

        Public Sub SetParameters(pDic As Dictionary(Of String, String)) Implements IQueryBO.SetParameters
            _calculationDate = pDic.GetValueByKey("CalculationDate", _calculationDate)
            If String.IsNullOrEmpty(_calculationDate) Then
                _calculationDate = Date.MaxValue.ToSunDate
            Else
                _calculationDate = New pbs.Helper.SmartDate(_calculationDate).DBValueInt
            End If
        End Sub

        Public Sub UpdateSelectedLines(pLines As IEnumerable) Implements IQueryBO.UpdateSelectedLines

        End Sub
        Private Sub UpdateCurrentLine(pLine As Object) Implements IQueryBO.UpdateCurrentLine
        End Sub
        Public Sub ZReset() Implements IQueryBO.ZReset

        End Sub
#End Region

#Region "Scripting"

        Private Sub AskUserDate()
            Dim newDate = pbs.Helper.UIServices.ValueSelectorService.SelectValue(LinkCode.Calendar, ResStrConst.SelectItemText("Date"), String.Empty)
            If Not newDate = String.Empty Then
                _calculationDate = newDate
            End If
        End Sub

        Private Function Select_Imp() As UITasks

            Dim scripts = New Script.UITasks(Me)

            scripts.IconName = "Find/Find"
            scripts.CaptionKey = "-"
            scripts.RefreshUIWhenFinish = True

            scripts.AddCallMethod(1, "AskUserDate") ' set journal datatable to param slot #1

            Return scripts

        End Function

        Public Function GetScriptDictionary() As Dictionary(Of String, Script.UITasks) Implements ISupportScripts.GetScriptDictionary
            Dim _scripts = New Dictionary(Of String, UITasks)(StringComparer.OrdinalIgnoreCase)

            _scripts.Add(pbs.Helper.Action._Select, Select_Imp)

            Return _scripts
        End Function

#End Region

#Region "User table"

        Friend Shared Function GetPatientBalancesAtDate(pDate As String, Optional pFilters As Dictionary(Of String, String) = Nothing) As List(Of PatientBalance)
            Dim _QD = pbs.BO.MC.MCLDGInfoList.Query.BuildPatientBalanceAtDate(pDate)

            _QD.AddFilterDictionary(pFilters)

            Dim dt = _QD.Extract

            Dim theOutput = New Dictionary(Of String, PatientBalance)

            For Each row As DataRow In dt.Rows
                Dim sb = PatientBalance.Fetch(row.DataRow2Dic)
                sb._calculationDate.Text = pDate

                'Dim theKey = String.Format("{0}.{1}", sb._PatientCode, sb._candidateId)
                Dim theKey = sb.PatientCode
                If String.IsNullOrEmpty(theKey) Then theKey = String.Format("{0}.{1}", sb._PatientCode, sb._candidateId)

                If theOutput.ContainsKey(theKey) Then
                    Dim bl = theOutput(theKey)
                    bl._debit += sb.Debit
                    bl._credit += sb.Credit
                Else
                    theOutput.Add(theKey, sb)
                End If
            Next

            Return theOutput.Values.ToList

        End Function

#End Region

#Region "InfoList"
        Public Shared Function GetInfoList() As List(Of PatientBalance)
            Return GetPatientBalancesAtDate("99990101")
        End Function

        Public Function GetInfoList(pFilter As Dictionary(Of String, String)) As IEnumerable Implements ISupportQueryInfoList.GetInfoList
            _calculationDate = pFilter.GetValueByKey("CalculationDate", "99990101")
            Return GetBOList()
        End Function

#End Region


    End Class
End Namespace

