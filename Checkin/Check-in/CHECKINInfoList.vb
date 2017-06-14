
Imports Csla
Imports Csla.Data
Imports System.Xml
Imports pbs.Helper


Namespace MC

    <Serializable()> _
    Public Class CHECKINInfoList
        Inherits Csla.ReadOnlyListBase(Of CHECKINInfoList, CHECKINInfo)

#Region " Transfer and Report Function "

        Public Shared Function TransferOut(ByVal Code_From As String, ByVal Code_To As String, ByVal FileName As String) As Integer
            Dim _dt As New DataTable(GetType(CHECKIN).ToString)
            Dim oa As New ObjectAdapter()

            For Each info As CHECKINInfo In CHECKINInfoList.GetCHECKINInfoList
                If info.Code >= Code_From AndAlso info.Code <= Code_To Then
                    oa.Fill(_dt, CHECKIN.GetBO(info.ToString))
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
            CHECKINInfoList.InvalidateCache()

            Dim thelist = CHECKINInfoList.GetCHECKINInfoList.ToList()

            Dim shr = New pbs.BO.ObjectShredder(Of CHECKINInfo)
            Dim dts As New List(Of DataTable)
            dts.Add(shr.Shred(thelist, Nothing, LoadOption.OverwriteChanges))

            Return dts
        End Function

#End Region

#Region " Business Properties and Methods "
        Private Shared _DTB As String = String.Empty
        Const _SUNTB As String = ""
        Private Shared _list As CHECKINInfoList

        'Private _fromDate As Helper.SmartDate = New Helper.SmartDate(0)
        'Public Property FromDate() As String
        '    Get
        '        Return _fromDate.ToString
        '    End Get
        '    Set(value As String)
        '        If String.IsNullOrEmpty(value) Then value = String.Empty
        '        If Not _fromDate.Equals(value) Then
        '            _fromDate.Text = value
        '        End If
        '    End Set
        'End Property

        'Private _toDate As Helper.SmartDate = New Helper.SmartDate(0)
        'Public Property ToDate() As String
        '    Get
        '        Return _toDate.ToString
        '    End Get
        '    Set(value As String)
        '        If String.IsNullOrEmpty(value) Then value = String.Empty
        '        If Not _toDate.Equals(value) Then
        '            _toDate.Text = value
        '        End If
        '    End Set
        'End Property

#End Region 'Business Properties and Methods

#Region " Factory Methods "

        Private Sub New()
            _DTB = Context.CurrentBECode
        End Sub

        Public Shared Function GetCHECKINInfo(ByVal pLineNo As String) As CHECKINInfo
            Dim Info As CHECKINInfo = CHECKINInfo.EmptyCHECKINInfo(pLineNo)
            ContainsCode(pLineNo, Info)
            Return Info
        End Function

        Public Shared Function GetDescription(ByVal pLineNo As String) As String
            Return GetCHECKINInfo(pLineNo).Description
        End Function

        Public Shared Function GetCHECKINInfoList() As CHECKINInfoList
            If _list Is Nothing Or _DTB <> Context.CurrentBECode Then

                _DTB = Context.CurrentBECode
                _list = DataPortal.Fetch(Of CHECKINInfoList)(New FilterCriteria())

            End If
            Return _list
        End Function

        Public Shared Sub InvalidateCache()
            _list = Nothing
            _checkinDic = Nothing
        End Sub

        Public Shared Sub ResetCache()
            _list = Nothing
            _checkinDic = Nothing
        End Sub

        'Private Shared invalidateLock As New Object
        'Public Shared Sub InvalidateCache()
        '    SyncLock invalidateLock
        '        If Not SettingsProvider.SoftInvalidateCache Then
        '            ResetCache()
        '        Else
        '            Dim thelist = GetCHECKINInfoList_Full()
        '            If thelist.Count > GetServerRecordCount() Then
        '                'someone delete some record on server. need to reload everything
        '                ResetCache()
        '            Else
        '                If thelist IsNot Nothing Then thelist.UpdatedInfoList()
        '            End If
        '        End If
        '    End SyncLock
        'End Sub

        Public Shared Function ContainsCode(ByVal pLineNo As String, Optional ByRef RetInfo As CHECKINInfo = Nothing) As Boolean

            RetInfo = CHECKINInfo.EmptyCHECKINInfo(pLineNo)
            If GetCHECKINDic.ContainsKey(pLineNo) Then
                RetInfo = GetCHECKINDic(pLineNo)
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
        '        e.Description = String.Format(ResStr(Msg.NOSUCHITEM), ResStr("CHECKIN"), value)
        '        Return False
        '    End If
        'End Function

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

                        cm.CommandText = <SqlText>SELECT * FROM pbs_MC_CHECKIN_<%= _DTB %></SqlText>.Value.Trim
 

                        Using dr As New SafeDataReader(cm.ExecuteReader)
                            While dr.Read
                                Dim info = CHECKINInfo.GetCHECKINInfo(dr)
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
#Region "CHECKIN Dictionary"

        Private Shared _checkinDic As Dictionary(Of String, CHECKINInfo)

        Private Shared Function GetCHECKINDic() As Dictionary(Of String, CHECKINInfo)
            If _checkinDic Is Nothing OrElse _DTB <> Context.CurrentBECode Then
                _checkinDic = New Dictionary(Of String, CHECKINInfo)

                For Each itm In CHECKINInfoList.GetCHECKINInfoList
                    _checkinDic.Add(itm.Code, itm)
                Next
            End If

            Return _checkinDic

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
        '    Dim script = SELECT count(*) FROM pbs_MC_CHECKIN_DEM  _DTB .Value.Tri
        '    Return SQLCommander.GetScalarInteger(script)
        'End Function
#End Region

    End Class

End Namespace