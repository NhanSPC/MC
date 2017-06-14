
Imports Csla
Imports Csla.Data
Imports System.Xml
Imports pbs.Helper

Imports pbs.BO.SQLBuilder

Namespace MC

    <Serializable()> _
    Public Class JEInfoList
        Inherits Csla.ReadOnlyListBase(Of JEInfoList, JEInfo)

#Region " Business Properties and Methods "
        Private Shared _DTB As String = String.Empty

#End Region 'Business Properties and Methods

#Region " Factory Methods "

        Private Sub New()
            _DTB = Context.CurrentBECode
        End Sub

        Public Shared Function GetJEInfo(ByVal pJrnalNo As Integer) As JEInfo
            Dim dic = New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
            dic.Add("JrnalNo", pJrnalNo)
            Dim thelist = GetJEInfoList(dic)
            If thelist.Count > 0 Then
                Return thelist(0)
            End If
            Return JEInfo.EmptyJEInfo
        End Function

        Public Shared Function GetJEInfoList() As JEInfoList
            Return New JEInfoList
        End Function

        Public Shared Function GetJEInfoList(pFilters As Dictionary(Of String, String)) As JEInfoList
            If pFilters Is Nothing OrElse pFilters.Count = 0 Then UIServices.AlertService.Alert(ResStrConst.CannotCallWithoutParameter, ResStr("MC Journal"))

            Dim _QD = Query.BuildQDByDic(pFilters)
            Dim SqlText = _QD.BuildSQL
            Return DataPortal.Fetch(Of JEInfoList)(New FilterCriteria() With {._sqlText = SqlText})
        End Function

        Public Shared Sub InvalidateCache()

        End Sub

#End Region ' Factory Methods

#Region " Data Access "

#Region " Filter Criteria "

        <Serializable()> _
        Private Class FilterCriteria
            'Friend _sqlText As String = String.Empty
            Friend _sqlText As String = <SQL>SELECT * FROM pbs_MC_JRNAL_<%= _DTB %></SQL>.Value
            Public Sub New()
            End Sub
        End Class

#End Region

        Private Shared _lockObj As New Object

        Private Overloads Sub DataPortal_Fetch(ByVal criteria As FilterCriteria)
            SyncLock _lockObj
                RaiseListChangedEvents = False
                IsReadOnly = False
                Dim jrnals = New List(Of String)
                  Using ctx = ConnectionFactory.GetDBConnection(True)
                    Using cm = ctx.CreateSQLCommand

                        cm.CommandText = criteria._sqlText

                        Using dr As New SafeDataReader(cm.ExecuteReader)
                            While dr.Read
                                Dim info = JEInfo.GetJEInfo(dr)
                                Me.Add(info)
                                jrnals.Add(info.JrnalNo)
                            End While
                        End Using
                    End Using

                    'calculate Detail aggregate
                    If jrnals.Count > 0 Then
                        Dim journals = String.Format("{0}", String.Join(",", jrnals.ToArray))
                        Using cm = ctx.CreateSQLCommand

                            cm.CommandText = <sqltext>SELECT PFD_NO, SUM(AMOUNT) AS AMOUNT, SUM(VAT_AMOUNT) AS VAT_AMOUNT FROM pbs_MC_LEDGER_<%= _DTB %> WHERE PFD_NO IN (<%= journals %>) GROUP BY PFD_NO</sqltext>.Value.Trim

                            Dim AmountDic = New Dictionary(Of Integer, AmountTotal)
                            Using dr As New SafeDataReader(cm.ExecuteReader)
                                While dr.Read
                                    Dim key = dr.GetInt32("PFD_NO")
                                    Dim value1 = dr.GetDecimal("AMOUNT")
                                    Dim value2 = dr.GetDecimal("VAT_AMOUNT")
                                    AmountDic.Add(key, New AmountTotal With {.Amount = value1, .VatAmount = value2})
                                End While
                            End Using

                            For Each _j In Me
                                Dim amt As AmountTotal = Nothing
                                If AmountDic.TryGetValue(_j.JrnalNo, amt) Then
                                    _j._amount.Float = amt.Amount
                                    _j._vatAmount.Float = amt.VatAmount
                                End If
                            Next
                        End Using
                    End If

                End Using
                IsReadOnly = True
                RaiseListChangedEvents = True
            End SyncLock
        End Sub

        Private Class AmountTotal
            Property Amount As Decimal
            Property VatAmount As Decimal
        End Class
#End Region ' Data Access   


        Class Query

            Shared Function BuildQDByDic(ByVal dic As Dictionary(Of String, String)) As QD

                Dim _QD As QD = QD.SysNewQD("SMJE")

                MakeUp_Query(_QD)

                If dic.ContainsKey("SubscriptionId") Then dic.Merge("NcPl9", dic("SubscriptionId"))
                If dic.ContainsKey("DueDate") Then dic.Merge("ExtDate1", dic("DueDate"))

                _QD.AddFilterDictionary(dic)

                If _QD.Filters.Count = 0 Then
                    ' no filters - no record
                    _QD.AddFilter("MCJE\ENTRY_BY", "", Context.CurrentUserCode)
                    _QD.AddFilter("MCJE\PERIOD", "SPN", "T")
                End If

                Return _QD
            End Function

            Private Shared Sub MakeUp_Query(ByRef _QD As QD)

                'Build Definition
                If _QD Is Nothing Then Exit Sub
                If _QD.DAG <> "SYSTM" Then _QD.Descriptn = "System Query - MC journal"
                _QD.AnalQ0 = "MCJE"
                _QD.Share = False

                'Build Output
                _QD.Selected.AddField(New pbs.BO.SQLBuilder.Field("", "*", "", ""))

            End Sub
        End Class

    End Class

End Namespace