
Imports Csla
Imports Csla.Data
Imports System.Xml
Imports pbs.Helper


Namespace MC

    <Serializable()>
    Public Class APPOINTMENTInfoList
        Inherits Csla.ReadOnlyListBase(Of APPOINTMENTInfoList, APPOINTMENTInfo)

#Region " Transfer and Report Function "

        Public Shared Function TransferOut(ByVal Code_From As String, ByVal Code_To As String, ByVal FileName As String) As Integer
            Dim _dt As New DataTable(GetType(APPOINTMENT).ToString)
            Dim oa As New ObjectAdapter()

            For Each info As APPOINTMENTInfo In APPOINTMENTInfoList.GetAPPOINTMENTInfoList
                If info.Code >= Code_From AndAlso info.Code <= Code_To Then
                    oa.Fill(_dt, APPOINTMENT.GetBO(info.ToString))
                End If
            Next
            Try
                _dt.Columns.Remove("IsNew")
                _dt.Columns.Remove("IsValid")
                _dt.Columns.Remove("IsSavable")
                _dt.Columns.Remove("IsDeleted")
                _dt.Columns.Remove("IsDirty")
                _dt.Columns.Remove("BrokenRulesCollection")
            Catch ex As Exception
            End Try

            For Each col As DataColumn In _dt.Columns
                col.ColumnMapping = MappingType.Attribute
            Next

            _dt.WriteXml(FileName)

            Return _dt.Rows.Count

        End Function

        Public Shared Function GetMyReportDataset() As List(Of DataTable)
            APPOINTMENTInfoList.InvalidateCache()

            Dim thelist = APPOINTMENTInfoList.GetAPPOINTMENTInfoList.ToList()

            Dim shr = New pbs.BO.ObjectShredder(Of APPOINTMENTInfo)
            Dim dts As New List(Of DataTable)
            dts.Add(shr.Shred(thelist, Nothing, LoadOption.OverwriteChanges))

            Return dts
        End Function

#End Region

#Region " Business Properties and Methods "
        Private Shared _DTB As String = String.Empty
        Const _SUNTB As String = ""
        Private Shared _list As APPOINTMENTInfoList
#End Region 'Business Properties and Methods

#Region " Factory Methods "

        Private Sub New()
            _DTB = Context.CurrentBECode
        End Sub

        Public Shared Function GetAPPOINTMENTInfo(ByVal pLineNo As String) As APPOINTMENTInfo
            Dim Info As APPOINTMENTInfo = APPOINTMENTInfo.EmptyAPPOINTMENTInfo(pLineNo)
            ContainsCode(pLineNo, Info)
            Return Info
        End Function

        Public Shared Function GetDescription(ByVal pLineNo As String) As String
            Return GetAPPOINTMENTInfo(pLineNo).Description
        End Function

        Public Shared Function GetAPPOINTMENTInfoList() As APPOINTMENTInfoList
            If _list Is Nothing Or _DTB <> Context.CurrentBECode Then

                _DTB = Context.CurrentBECode
                _list = DataPortal.Fetch(Of APPOINTMENTInfoList)(New FilterCriteria())

            End If
            Return _list
        End Function

        Public Shared Sub InvalidateCache()
            _list = Nothing
            _appointmentDic = Nothing
        End Sub

        Public Shared Sub ResetCache()
            _list = Nothing
            _appointmentDic = Nothing
        End Sub

        'Private Shared invalidateLock As New Object
        'Public Shared Sub InvalidateCache()
        '    SyncLock invalidateLock
        '        If Not SettingsProvider.SoftInvalidateCache Then
        '            ResetCache()
        '        Else
        '            Dim thelist = GetAPPOINTMENTInfoList_Full()
        '            If thelist.Count > GetServerRecordCount() Then
        '                'someone delete some record on server. need to reload everything
        '                ResetCache()
        '            Else
        '                If thelist IsNot Nothing Then thelist.UpdatedInfoList()
        '            End If
        '        End If
        '    End SyncLock
        'End Sub

        Public Shared Function ContainsCode(ByVal pLineNo As String, Optional ByRef RetInfo As APPOINTMENTInfo = Nothing) As Boolean

            RetInfo = APPOINTMENTInfo.EmptyAPPOINTMENTInfo(pLineNo)
            If GetAPPOINTMENTDic.ContainsKey(pLineNo) Then
                RetInfo = GetAPPOINTMENTDic(pLineNo)
                Return True
            End If
            Return False
        End Function

        'Public Shared Function ContainsCode(ByVal Target As Object, ByVal e As Validation.RuleArgs) As Boolean
        '    Dim value As String = CType(CallByName(Target, e.PropertyName, CallType.Get), String)
        '    'no thing to check
        '    If String.IsNullOrEmpty(value) Then Return True

        '    If ContainsCode(value) Then
        '        Return True
        '    Else
        '        e.Description = String.Format(ResStr(Msg.NOSUCHITEM), ResStr("APPOINTMENT"), value)
        '        Return False
        '    End If
        'End Function

#End Region ' Factory Methods

#Region " Data Access "

#Region " Filter Criteria "

        <Serializable()>
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

                        cm.CommandText = <SqlText>SELECT * FROM pbs_MC_APPOINTMENT_<%= _DTB %></SqlText>.Value.Trim

                       

                        Using dr As New SafeDataReader(cm.ExecuteReader)
                            While dr.Read
                                Dim info = APPOINTMENTInfo.GetAPPOINTMENTInfo(dr)
                                Me.Add(info)
                            End While
                        End Using

                    End Using

                  

                End Using
                IsReadOnly = True
                RaiseListChangedEvents = True
            End SyncLock
        End Sub

#End Region ' Data Access                   
#Region "APPOINTMENT Dictionary"

        Private Shared _appointmentDic As Dictionary(Of String, APPOINTMENTInfo)

        Private Shared Function GetAPPOINTMENTDic() As Dictionary(Of String, APPOINTMENTInfo)
            If _appointmentDic Is Nothing OrElse _DTB <> Context.CurrentBECode Then
                _appointmentDic = New Dictionary(Of String, APPOINTMENTInfo)

                For Each itm In APPOINTMENTInfoList.GetAPPOINTMENTInfoList
                    _appointmentDic.Add(itm.Code, itm)
                Next
            End If

            Return _appointmentDic

        End Function

#End Region

#Region "TimeStamp"
        'Private _listTimeStamp() As Byte

        'Private Sub UpdatedInfoList()
        '    'get new updated notes by row stamp
        '    Dim newInfos = DataPortal.Fetch(Of PODInfoList)(New FilterCriteria() With {._timeStamp = _listTimeStamp})

        '    If newInfos.Count = 0 Then Exit Sub

        '    'merge new notes with the old one

        '    Dim oldPODs = GetPODInfoList_Full()
        '    Dim oldDic = New Dictionary(Of String, PODInfo)
        '    For Each itm In oldPODs
        '        oldDic.Add(itm.Code, itm)
        '    Next

        '    oldPODs.IsReadOnly = False
        '    oldPODs.RaiseListChangedEvents = False

        '    For Each info In newInfos
        '        If oldDic.ContainsKey(info.Code) Then

        '            Dim oldNote = oldDic(info.Code)
        '            oldPODs.Remove(oldNote)
        '            oldPODs.Add(info)

        '            oldDic(info.Code) = info
        '        Else
        '            oldPODs.Add(info)

        '            oldDic.Add(info.Code, info)
        '        End If
        '    Next


        '    oldPODs.IsReadOnly = False
        '    oldPODs.RaiseListChangedEvents = False

        '    _podDic = oldDic
        '    _list = oldPODs
        '    _list._listTimeStamp = newInfos._listTimeStamp

        'End Sub

        'Private Shared Function GetServerRecordCount() As Integer
        '    Dim script = SELECT count(*) FROM pbs_MC_APPOINTMENT_DEM  _DTB .Value.Tri
        '    Return SQLCommander.GetScalarInteger(script)
        'End Function
#End Region

    End Class

End Namespace