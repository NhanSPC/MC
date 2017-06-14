Imports pbs.BO.Script
Imports pbs.BO.SQLBuilder
Imports pbs.Helper
Imports System.Text.RegularExpressions

Namespace MC

    ''' <summary>
    ''' Patient Ledger Inquiry. Can query multiple Patients
    ''' </summary>
    ''' <remarks></remarks>
    Public Class LQ
        Implements IQueryBO
        Implements ISupportScripts

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
            Dim theQD = GetMyQD()
            Return pbs.BO.MC.MCLDGInfoList.GetMCLDGInfoList(theQD.BuildSQL)
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

        Private _qd As QD
        Private _InitialQueryId As String = String.Empty

        Public Function GetMyQD() As SQLBuilder.QD Implements IQueryBO.GetMyQD
            If _qd Is Nothing Then
                If Not String.IsNullOrEmpty(_InitialQueryId) AndAlso QD.Exists(_InitialQueryId) Then
                    _qd = QD.GetQD(_InitialQueryId)
                Else
                    _qd = pbs.BO.MC.MCLDGInfoList.Query.BuildQDByDic(_userFilters)
                End If
            End If
            Return _qd
        End Function
        Public Sub UpdateQD(pQD As SQLBuilder.QD) Implements IQueryBO.UpdateQD
            _qd = pQD
        End Sub
        Public Function GetMyTitle() As String Implements IQueryBO.GetMyTitle
            Return String.Format("Receivable Ledger Inquiry")
        End Function

        Public Function GetSelectCommand() As String Implements IQueryBO.GetSelectCommand
            Return String.Empty
        End Function

        Public Function GetSelectionChangedCommand() As String Implements IQueryBO.GetSelectionChangedCommand
            Return String.Empty
        End Function

        Public Function GetVariables() As Dictionary(Of String, String) Implements IQueryBO.GetVariables
            Dim dic = _userFilters.CloneDic
            Return dic
        End Function

        Public Sub SetParameters(pDic As Dictionary(Of String, String)) Implements IQueryBO.SetParameters
            _currentLineNo = pDic.GetValueByKey("CurrentObjectId", _currentLineNo)
            _InitialQueryId = pDic.GetSystemKey("$Query", String.Empty)
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

#Region "Script"

        'Private Sub AskCriteria()
        '    Dim theQD = GetMyQD()
        '    If pbs.Helper.UIServices.QDFiltersService.AskCriteria(theQD) = True Then
        '        _qd = theQD
        '    End If
        'End Sub

        'Private Function Select_Imp() As UITasks

        '    Dim scripts = New Script.UITasks(Me)

        '    scripts.IconName = "Find/Find"
        '    scripts.CaptionKey = "-"
        '    scripts.RefreshUIWhenFinish = True

        '    scripts.AddCallMethod(1, "AskCriteria") ' set journal datatable to param slot #1

        '    Return scripts

        'End Function

        Public Function GetScriptDictionary() As Dictionary(Of String, Script.UITasks) Implements ISupportScripts.GetScriptDictionary
            Dim _scripts = New Dictionary(Of String, UITasks)(StringComparer.OrdinalIgnoreCase)

            '_scripts.Add("Select", Select_Imp)

            _scripts.Add(pbs.Helper.Action._MatchBaseAmount, MatchBaseAmount_Imp)

            _scripts.Add("RemoveAllocation", RemoveAllocation_Imp)

            _scripts.Add("Split", Split_Imp)


            Return _scripts
        End Function

#End Region

#Region "Matching"
        Private Sub MatchBaseAmount()

            Dim theList = New List(Of MCLDGInfo)
            If _selectedLines IsNot Nothing Then
                For Each info In _selectedLines
                    theList.Add(info)
                Next
            End If

            If theList.Count > 0 Then
                MCLDGInfoList.AllocationMatching(theList)
            Else
                pbs.Helper.UIServices.AlertService.Alert("Please select at least 2 lines for matching")
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

            scripts.IconName = "monoUnMatch "
            scripts.CaptionKey = "RemoveAllocation"
            scripts.ButtonLocation = ScriptButtonLocation.Navigation_Right
            scripts.RefreshUIWhenFinish = True

            scripts.AddCallMethod(1, "UnMatch") ' set journal datatable to param slot #1

            Return scripts

        End Function
#End Region

#Region "Splitting"
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
#End Region
    End Class

End Namespace

