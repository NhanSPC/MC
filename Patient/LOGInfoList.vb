
Imports Csla
Imports Csla.Data
Imports System.Xml
Imports pbs.Helper

Imports pbs.Helper.Interfaces
Imports pbs.BO.SQLBuilder

Namespace MC

    <Serializable()> _
    Public Class LOGInfoList
        Inherits Csla.ReadOnlyListBase(Of LOGInfoList, LOGInfo)
        Implements ISupportQueryInfoList

#Region " Business Properties and Methods "
        Private Shared _DTB As String = String.Empty
        Const _SUNTB As String = ""
        Private Shared _list As LOGInfoList
#End Region 'Business Properties and Methods

#Region " Factory Methods "

        Private Sub New()
            _DTB = Context.CurrentBECode
        End Sub

        Public Shared Function GetLOGInfo(ByVal pLineNo As String) As LOGInfo
            Dim Info As LOGInfo = LOGInfo.EmptyLOGInfo(pLineNo)
            ContainsCode(pLineNo, Info)
            Return Info
        End Function

        Public Shared Function GetDescription(ByVal pLineNo As String) As String
            Return GetLOGInfo(pLineNo).Description
        End Function

        Public Shared Function GetLOGInfoList() As LOGInfoList
            If _list Is Nothing Or _DTB <> Context.CurrentBECode Then

                _DTB = Context.CurrentBECode
                _list = DataPortal.Fetch(Of LOGInfoList)(New FilterCriteria())

            End If
            Return _list
        End Function

        Public Shared Sub ResetCache()
            _list = Nothing
            _logDic = Nothing
        End Sub

        Public Shared Sub InvalidateCache()
            ResetCache()
        End Sub

        Public Shared Function ContainsCode(ByVal pLineNo As String, Optional ByRef RetInfo As LOGInfo = Nothing) As Boolean

            RetInfo = LOGInfo.EmptyLOGInfo(pLineNo.ToInteger)
            If GetLOGDic.ContainsKey(pLineNo) Then
                RetInfo = GetLOGDic(pLineNo)
                Return True
            End If
            Return False
        End Function


#End Region ' Factory Methods

#Region " Data Access "

#Region " Filter Criteria "

        <Serializable()> _
        Private Class FilterCriteria

            Friend _sqlText As String = String.Empty

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

                        If Not String.IsNullOrEmpty(criteria._sqlText) Then
                            cm.CommandText = criteria._sqlText
                        Else
                            cm.CommandText = <SqlText>SELECT * FROM pbs_MC_LOG_<%= _DTB %></SqlText>.Value.Trim
                        End If

                        Using dr As New SafeDataReader(cm.ExecuteReader)
                            While dr.Read
                                Dim info = LOGInfo.GetLOGInfo(dr)
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

#Region "LOG Dictionary"

        Private Shared _logDic As Dictionary(Of String, LOGInfo)

        Private Shared Function GetLOGDic() As Dictionary(Of String, LOGInfo)
            If _logDic Is Nothing OrElse _DTB <> Context.CurrentBECode Then
                _logDic = New Dictionary(Of String, LOGInfo)

                For Each itm In LOGInfoList.GetLOGInfoList
                    _logDic.Add(itm.Code, itm)
                Next
            End If

            Return _logDic

        End Function

#End Region

#Region "ISupportQueryInfoList"

        Public Shared Function GetLOGInfoList(pFilters As Dictionary(Of String, String)) As LOGInfoList

            If pFilters Is Nothing OrElse pFilters.Count = 0 Then UIServices.AlertService.Alert(ResStrConst.CannotCallWithoutParameter, ResStr("LEAD"))

            Dim _QD = Query.BuildQDByDic(pFilters)
            Dim SqlText = _QD.BuildSQL

            Return DataPortal.Fetch(Of LOGInfoList)(New FilterCriteria() With {._sqlText = SqlText})

        End Function

        Public Function GetInfoList(pFilters As Dictionary(Of String, String)) As IEnumerable Implements ISupportQueryInfoList.GetInfoList
            Return LOGInfoList.GetLOGInfoList(pFilters)
        End Function
#End Region

        ' ''' <summary>
        ' ''' Return the latest log record of a Patient or Candidate or Lead at a date. 
        ' ''' </summary>
        ' ''' <param name="pDate">the checking Date</param>
        ' ''' <param name="pPatientCode"></param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Shared Function GetPatientLogs(pDate As String, pLogType As String, pPatientCode As String) As LOGInfoList

        '    Dim _QD = Query.BuildQDByDic(pDate, pLogType, pPatientCode)

        '    Dim SqlText = _QD.BuildSQL

        '    If String.IsNullOrEmpty(SqlText) Then
        '        Return New LOGInfoList
        '    Else
        '        Return DataPortal.Fetch(Of LOGInfoList)(New FilterCriteria() With {._sqlText = SqlText})
        '    End If

        'End Function


        Private Class Query

            Shared Function BuildQDByDic(pFilters As Dictionary(Of String, String)) As QD
                Dim _QD As QD = QD.SysNewQD("LOG0001")
                _QD.Descriptn = "Medicare Logs"
                _QD.AnalQ0 = "MCLOG"

                _QD.AddSelectedField("*")

                _QD.AddFilterDictionary(pFilters)

                If _QD.Filters.Count = 0 Then
                    _QD.AddFilter("MCLOG\LINE_NO", "N1", "<ALL>")
                End If

                Return _QD
            End Function

        End Class

    End Class

End Namespace