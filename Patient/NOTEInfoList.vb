
Imports Csla
Imports Csla.Data
Imports System.XML
Imports pbs.Helper


Namespace MC

    <Serializable()> _
    Public Class NOTEInfoList
        Inherits Csla.ReadOnlyListBase(Of NOTEInfoList, NOTEInfo)

#Region " Business Properties and Methods "
        Private Shared _DTB As String = String.Empty
        Const _SUNTB As String = ""
        Private Shared _list As NOTEInfoList
#End Region 'Business Properties and Methods

#Region " Factory Methods "

        Private Sub New()
            _DTB = Context.CurrentBECode
        End Sub

        Public Shared Function GetNOTEInfo(ByVal pLineNo As Integer) As NOTEInfo
            Dim Info As NOTEInfo = NOTEInfo.EmptyNOTEInfo(pLineNo)
            ContainsCode(pLineNo, Info)
            Return Info
        End Function

        Public Shared Function GetDescription(ByVal pLineNo As Integer) As String
            Return GetNOTEInfo(pLineNo).Description
        End Function

        Public Shared Function GetNOTEInfoList() As NOTEInfoList
            If _list Is Nothing Or _DTB <> Context.CurrentBECode Then

                _DTB = Context.CurrentBECode
                _list = DataPortal.Fetch(Of NOTEInfoList)(New FilterCriteria())

            End If
            Return _list
        End Function

        Public Shared Sub ResetCache()
            _list = Nothing
            _noteDic = Nothing
        End Sub

        Private Shared invalidateLock As New Object
        Public Shared Sub InvalidateCache()
            SyncLock invalidateLock
                If Not SettingsProvider.SoftInvalidateCache Then
                    ResetCache()
                Else
                    Dim thelist = GetNOTEInfoList()
                    If thelist.Count > GetServerRecordCount() Then
                        'someone delete some record on server. need to reload everything
                        ResetCache()
                    Else
                        If thelist IsNot Nothing Then thelist.UpdatedInfoList()
                    End If
                End If
            End SyncLock
        End Sub

        Public Shared Function ContainsCode(ByVal pLineNo As Integer, Optional ByRef RetInfo As NOTEInfo = Nothing) As Boolean

            RetInfo = NOTEInfo.EmptyNOTEInfo(pLineNo)
            If GetNOTEDic.ContainsKey(pLineNo) Then
                RetInfo = GetNOTEDic(pLineNo)
                Return True
            End If

            Return False
        End Function

        Public Shared Function ContainsCode(ByVal Target As Object, ByVal e As Validation.RuleArgs) As Boolean
            Dim value As String = CType(CallByName(Target, e.PropertyName, CallType.Get), String)
            'no thing to check
            If String.IsNullOrEmpty(value) Then Return True

            If ContainsCode(value) Then
                Return True
            Else
                e.Description = String.Format(ResStr(ResStrConst.NOSUCHITEM), ResStr("NOTE"), value)
                Return False
            End If
        End Function

#End Region ' Factory Methods

#Region " Data Access "

#Region " Filter Criteria "

        <Serializable()> _
        Private Class FilterCriteria
            Friend _timeStamp() As Byte
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

                        If criteria._timeStamp IsNot Nothing Then
                            cm.CommandText = <SqlText>SELECT * FROM pbs_MC_NOTES_<%= _DTB %> WHERE TIME_STAMP > @CurrentTimeStamp</SqlText>.Value.Trim
                            cm.Parameters.AddWithValue("@CurrentTimeStamp", criteria._timeStamp)
                        Else
                            cm.CommandText = <SqlText>SELECT * FROM pbs_MC_NOTES_<%= _DTB %></SqlText>.Value.Trim
                        End If

                        Using dr As New SafeDataReader(cm.ExecuteReader)
                            While dr.Read
                                Dim info = NOTEInfo.GetNOTEInfo(dr)
                                Me.Add(info)
                            End While
                        End Using

                    End Using

                    'read the current version of the list
                    Using cm = ctx.CreateSQLCommand
                        cm.CommandText = <SqlText>SELECT max(TIME_STAMP) FROM pbs_MC_NOTES_<%= _DTB %></SqlText>.Value.Trim
                        Dim ret = cm.ExecuteScalar
                        If ret IsNot Nothing Then _listTimeStamp = ret
                    End Using

                End Using
                IsReadOnly = True
                RaiseListChangedEvents = True
            End SyncLock
        End Sub

#End Region ' Data Access     

#Region "NOTE Dictionary"

        Public Shared Sub RemoveInfo(pLineNo As Integer)
            SyncLock _lockObj
                Try
                    If _list Is Nothing Then Exit Sub

                    Dim info As NOTEInfo = Nothing
                    If ContainsCode(pLineNo, info) Then
                        _list.IsReadOnly = False
                        _list.Remove(info)
                        _list.IsReadOnly = True
                        If _noteDic IsNot Nothing Then _noteDic.Remove(pLineNo)
                    End If
                Catch ex As Exception
                    TextLogger.Log(ex)
                End Try
            End SyncLock

        End Sub

        Private Shared _noteDic As Dictionary(Of String, NOTEInfo)

        Private Shared Function GetNOTEDic() As Dictionary(Of String, NOTEInfo)
            If _noteDic Is Nothing OrElse _DTB <> Context.CurrentBECode Then
                _noteDic = New Dictionary(Of String, NOTEInfo)

                For Each itm In NOTEInfoList.GetNOTEInfoList
                    _noteDic.Add(itm.Code, itm)
                Next
            End If

            Return _noteDic

        End Function

#End Region

#Region "TimeStamp"
        Private _listTimeStamp() As Byte

        Private Sub UpdatedInfoList()
            'get new updated notes by row stamp
            Dim newInfos = DataPortal.Fetch(Of NOTEInfoList)(New FilterCriteria() With {._timeStamp = _listTimeStamp})

            If newInfos.Count = 0 Then Exit Sub

            'merge new notes with the old one

            Dim oldNotes = GetNOTEInfoList()
            Dim oldDic = New Dictionary(Of String, NOTEInfo)
            For Each itm In oldNotes
                oldDic.Add(itm.Code, itm)
            Next

            oldNotes.IsReadOnly = False
            oldNotes.RaiseListChangedEvents = False

            For Each info In newInfos
                If oldDic.ContainsKey(info.Code) Then

                    Dim oldNote = oldDic(info.Code)
                    oldNotes.Remove(oldNote)
                    oldNotes.Add(info)

                    oldDic(info.Code) = info
                Else
                    oldNotes.Add(info)

                    oldDic.Add(info.Code, info)
                End If
            Next


            oldNotes.IsReadOnly = False
            oldNotes.RaiseListChangedEvents = False

            _noteDic = oldDic
            _list = oldNotes
            _list._listTimeStamp = newInfos._listTimeStamp

        End Sub

        Private Shared Function GetServerRecordCount() As Integer
            Dim script = <SqlText>SELECT * FROM pbs_SM_NOTES_<%= _DTB %></SqlText>.Value.Trim
            Return SQLCommander.GetScalarInteger(script)
        End Function
#End Region

    End Class

End Namespace