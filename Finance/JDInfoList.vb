Imports System.Data

Imports Csla
Imports Csla.Data
Imports System.XML
Imports System.XML.Xsl
Imports pbs.Helper

Namespace MC

    <Serializable()> _
    Public Class JDInfoList
        Inherits Csla.ReadOnlyListBase(Of JDInfoList, JDInfo)

        Private Shared _list As JDInfoList

        Private Shared _DTB As String = String.Empty

#Region " Factory Methods "

        Private Sub New()
            'require use of factory method
            _DTB = Context.CurrentBECode
        End Sub
        Private Shared _lockObj As New Object

        Public Shared Function GetJDInfoList() As JDInfoList
            Dim full = GetJDInfoList_Full()

            Dim thelist = New JDInfoList

            thelist.RaiseListChangedEvents = False
            thelist.IsReadOnly = False

            For Each info In full
                If pbs.UsrMan.DAG.CanAccess(info.Dag) Then
                    thelist.Add(info)
                End If
            Next

            thelist.RaiseListChangedEvents = True
            thelist.IsReadOnly = True

            Return thelist
        End Function

        Private Shared Function GetJDInfoList_Full() As JDInfoList
            If _list Is Nothing OrElse _DTB <> Context.CurrentBECode Then
                SyncLock _lockObj
                    _DTB = Context.CurrentBECode
                    _list = DataPortal.Fetch(Of JDInfoList)(New FilterCriteria())

                End SyncLock
            End If
            Return _list
        End Function

        Public Shared Sub InvalidateCache()
            ' Csla.ClientContext("_JDInfoList_") = Nothing
            'Csla.ClientContext("_RJDDic_") = Nothing

            _list = Nothing
            _jdDic = Nothing
        End Sub

        Public Shared Function ContainsCode(ByVal Code As String, Optional ByRef RetInfo As JDInfo = Nothing) As Boolean
            Code = Code.Trim

            If Code = pbs.Helper.SystemJT Then
                RetInfo = JDInfo.SYSTMJDInfo
                Return True
          
            End If

            If GetJDDic.ContainsKey(Code) Then
                RetInfo = GetJDDic(Code)
                If pbs.UsrMan.DAG.CanAccess(RetInfo.Dag) Then
                    Return True
                Else
                    Return False
                End If
            Else
                RetInfo = JDInfo.EmptyJDInfo()
                Return False
            End If

        End Function

        Public Shared Function ContainsCode(ByVal Target As Object, ByVal e As Validation.RuleArgs) As Boolean
            Dim value As String = CType(CallByName(Target, e.PropertyName, CallType.Get), String)
            If String.IsNullOrEmpty(value) Then Return True

            If ContainsCode(value) Then
                Return True
            ElseIf pbs.BO.MC.Settings.GetSettings.RefundJournalType.Contains(value) Then
                Return True
            ElseIf pbs.BO.MC.Settings.GetSettings.CollectionJournalType.Contains(value) Then
                Return True
            Else
                e.Description = String.Format(ResStr(ResStrConst.NOSUCHITEM), ResStr("JD"), value)
                Return False
            End If
        End Function

        Public Shared Function GetJDInfo(ByVal JDCode As String) As JDInfo
            Dim Info As JDInfo = JDInfo.EmptyJDInfo()
            ContainsCode(JDCode, Info)
            Return Info
        End Function
        Public Shared Function GetDescription(ByVal JDCode As String) As String
            Return GetJDInfo(JDCode).JournalName
        End Function

#End Region ' Factory Methods

#Region " Data Access "

#Region " Filter Criteria "

        <Serializable()> _
        Private Class FilterCriteria
            Public Sub New()
            End Sub
        End Class

#End Region

#Region " Data Access - Fetch "

        Private Overloads Sub DataPortal_Fetch(ByVal criteria As FilterCriteria)
            SyncLock _lockObj
                RaiseListChangedEvents = False
                IsReadOnly = False
                Dim dic As New Dictionary(Of String, JDInfo)(StringComparer.OrdinalIgnoreCase)

                Using ctx = ConnectionFactory.GetDBConnection(True)
                    Using cm = ctx.Get_RFMSC_FetchAllCommand(_DTB, RFMSC_TB.MCJournalDefinition)

                        Using dr As New SafeDataReader(cm.ExecuteReader)
                            While dr.Read
                                Dim info = JDInfo.GetJDInfo(dr)

                                If dic.ContainsKey(info.ToString) Then Me.Remove(dic(info.ToString))

                                Me.Add(info)

                            End While
                        End Using

                    End Using
                End Using
                IsReadOnly = True
                RaiseListChangedEvents = True
            End SyncLock
        End Sub

#End Region ' Data Access - Fetch

#End Region ' Data Access

#Region "JD Dictionary"

        Private Shared _jdDic As Dictionary(Of String, JDInfo)

        Private Shared Function GetJDDic() As Dictionary(Of String, JDInfo)
            ' Dim _jdDic As Dictionary(Of String, JDInfo) = Csla.ClientContext("_RJDDic_")

            If _jdDic Is Nothing OrElse _DTB <> Context.CurrentBECode Then
                Dim thelist = JDInfoList.GetJDInfoList_Full
                _jdDic = New Dictionary(Of String, JDInfo)

                For Each itm In thelist
                    _jdDic.Add(itm.JournalType, itm)
                Next

            End If
            'Csla.ClientContext("_RJDDic_") = _jdDic
            Return _jdDic

        End Function

#End Region

    End Class

End Namespace
