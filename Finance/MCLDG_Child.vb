Imports pbs.Helper
Imports System.Data

Imports Csla
Imports Csla.Data
Imports Csla.Validation

Namespace MC

    Partial Public Class MCLDG
        Implements ILockable


#Region " Factory Methods - Child"
        Friend Sub MarkAsClean()
            Me.MarkClean()
        End Sub

        Friend Sub MarkAsNewClone()
            _lineNo = _lineNo * -1
            _lockFlag = String.Empty
            _postedBy = String.Empty
            _postingDate.Text = String.Empty
            _status = String.Empty
            _allocation = String.Empty
            _allocDate.Text = String.Empty
            _allocPeriod.Text = String.Empty
            _allocRef = 0

            MarkNew()
        End Sub

        Friend Shared Function NewChildMCLDG() As MCLDG
            Dim ret = New MCLDG()
            ret.MarkAsChild()
            Return ret
        End Function

        Friend Shared Function GetChildMCLDG(ByVal dr As SafeDataReader, Optional ByVal SuppressValidation As Boolean = False) As MCLDG
            Dim ret = New MCLDG(dr, SuppressValidation)
            ret.MarkOld()
            ret.MarkAsChild()
            Return ret
        End Function

        Private Sub New(ByVal dr As SafeDataReader, Optional ByVal SuppressValidation As Boolean = False)
            _DTB = Context.CurrentBECode
            MarkAsChild()
            _suspendCalculation = True
            FetchObject(dr)
            _suspendCalculation = False

            If Not SuppressValidation Then ValidationRules.CheckRules()
        End Sub

        Friend Sub CopyParentInfo(pParent As JE)
            _DTB = pParent._DTB
            _pfdNo = pParent.JrnalNo
            _transType = pParent.TransType
            _reference = pParent.Reference
            _transDate.Text = pParent._transDate.DBValueInt
            _period.Text = pParent._period.DBValue

            If pParent.Status = JE.STR_Post Then
                _lockFlag = "Y"
            End If
        End Sub
#End Region ' Factory Methods

#Region " Data Access - children"

#Region " Data Access - Insert "

        Friend Sub Insert(ByVal cn As IDbConnection)
            If Not IsDirty Then Return
            ExecuteInsert(cn)
            MarkOld()
        End Sub

        Friend Sub Update(ByVal cn As IDbConnection)
            If Not IsDirty Then Return
            ExecuteUpdate(cn)
            MarkOld()
        End Sub

#End Region ' Data Access - Insert Update

#Region " Data Access - Delete "

        Friend Sub DeleteSelf(ByVal cn As IDbConnection)
            If Not IsDirty Then Return
            If IsNew Then Return
            Dim _DTB = Context.CurrentBECode

            Using cm = cn.CreateSQLCommand
                cm.CommandType = CommandType.Text
                cm.CommandText = <SqlText>DELETE FROM pbs_MC_LEDGER_<%= _DTB %> WHERE LINE_NO= <%= _lineNo %></SqlText>.Value.Trim
                cm.ExecuteNonQuery()
            End Using

            MarkNew()
        End Sub

#End Region ' Data Access - Delete

#End Region 'Data Access     


#Region "Lockable"
        Public Function isLocked() As Boolean Implements ILockable.isLocked
            If Me.IsNew Then Return False
            Return HardPosted() OrElse _allocation.MatchesRegExp("^[A|P|C]$")
        End Function

        Public Function isLockedBy() As String Implements ILockable.isLockedBy
            If HardPosted() Then
                Return "HardPosting"
            ElseIf _allocation.MatchesRegExp("^[A|P|C]$") Then
                Return "Allocated"
            Else
                Return String.Empty
            End If
        End Function

        Public Function isLockedsomeWhereInFamily() As Boolean Implements ILockable.isLockedsomeWhereInFamily
            Return False
        End Function

        Public Function LockingMessage() As String Implements ILockable.LockingMessage
            If HardPosted() Then
                Return ResStr("Changing a hard posted line is not allowed")
            ElseIf _allocation.MatchesRegExp("^[A|P|C]$") Then
                Return ResStr("Changing of an allocated line is not allowed")
            Else
                Return ResStr("Changing of a locked line is not allowed")
            End If
        End Function

        Public Function LockMe() As Boolean Implements ILockable.LockMe
            Return False
        End Function

        Public Function LockMyFamily() As Boolean Implements ILockable.LockMyFamily
            Return False
        End Function
#End Region

    End Class

End Namespace