Imports pbs.Helper
Imports System.Data

Imports Csla
Imports Csla.Data
Imports Csla.Validation

Namespace MC

    Partial Public Class NOTE
        Inherits Csla.BusinessBase(Of NOTE)

#Region " Factory Methods - Child"

        Friend Sub MarkAsNewClone()
            Me.MarkNew()
        End Sub

        Friend Shared Function NewChildNOTE() As NOTE
            Dim ret = New NOTE()
            ret.MarkAsChild()
            Return ret
        End Function

        Friend Shared Function GetChildNOTE(ByVal dr As SafeDataReader, Optional ByVal SuppressValidation As Boolean = False) As NOTE
            Dim ret = New NOTE(dr, SuppressValidation)
            ret.MarkOld()
            ret.MarkAsChild()
            Return ret
        End Function

        Private Sub New(ByVal dr As SafeDataReader, Optional ByVal SuppressValidation As Boolean = False)
            MarkAsChild()
            FetchObject(dr)
            If Not SuppressValidation Then ValidationRules.CheckRules()
        End Sub

#End Region ' Factory Methods

#Region " Data Access - children"

#Region " Data Access - Insert "

        Friend Sub Insert(ByVal cn As IDbConnection)
            If Not IsDirty Then Return
            ExecuteInsert(cn)
            MarkOld()
        End Sub

        Private Sub ExecuteInsert(ByVal cn As IDbConnection)
            Using cm = cn.CreateSQLCommand

                cm.CommandType = CommandType.StoredProcedure
                cm.CommandText = String.Format("pbs_MCNOTE_{0}_Insert", _DTB)

                cm.Parameters.AddWithValue("@LINE_NO", _lineNo, ParameterDirection.Output)
                AddInsertParameters(cm)
                cm.ExecuteNonQuery()

                _lineNo = DNz(cm.Parameters("@LINE_NO").Value, 0).ToInteger
            End Using
        End Sub

        Friend Sub Update(ByVal cn As IDbConnection)
            If Not IsDirty Then Return
            ExecuteUpdate(cn)
            MarkOld()
        End Sub

        Private Sub ExecuteUpdate(ByVal cn As IDbConnection)
            Using cm = cn.CreateSQLCommand

                cm.CommandType = CommandType.StoredProcedure
                cm.CommandText = String.Format("pbs_MCNOTE_{0}_Update", _DTB)

                cm.Parameters.AddWithValue("@LINE_NO", _lineNo)
                AddInsertParameters(cm)
                cm.ExecuteNonQuery()

            End Using
        End Sub


#End Region ' Data Access - Insert Update

#Region " Data Access - Delete "

        Friend Sub DeleteSelf(ByVal cn As IDbConnection)
            If Not IsDirty Then Return
            If IsNew Then Return
            Dim _DTB = Context.CurrentBECode

            Dim sqlText = <SqlText>DELETE pbs_MC_NOTES_<%= _DTB %>  WHERE LINE_NO= <%= _lineNo %></SqlText>.Value.Trim
            Using cm = cn.CreateSQLCommand
                cm.CommandType = CommandType.Text
                cm.CommandText = sqlText.Trim

                cm.ExecuteNonQuery()
            End Using

            MarkNew()
        End Sub


#End Region ' Data Access - Delete

#End Region 'Data Access     

    End Class

End Namespace