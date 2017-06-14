Imports System
Imports System.Data

Imports Csla
Imports Csla.Data
Imports pbs.Helper

Namespace MC

    <Serializable()> _
    Public Class SIDs
        Inherits Csla.BusinessListBase(Of SIDs, SID)

#Region " Business Methods"
        Friend _parent As SI = Nothing

        Protected Overrides Function AddNewCore() As Object
            If _parent IsNot Nothing Then
                Dim theNewLine = SID.NewSID(_parent.DocumentNo)
                theNewLine._percentCover.Text = _parent.CalculateInsurance
                AddNewLine(theNewLine)
                theNewLine.CheckRules()
                Return theNewLine
            Else
                Return MyBase.AddNewCore
            End If
        End Function

        Friend Sub AddNewLine(ByVal pline As SID)
            If pline Is Nothing Then Exit Sub

            'get the next line number
            Dim nextnumber As Integer = 1
            If Me.Count > 0 Then
                Dim allNumbers = (From line In Me Select line.LineNo).ToList
                nextnumber = allNumbers.Max + 1
            End If

            pline._lineNo = nextnumber

            'Populate _line with info from parent here

            Me.Add(pline)

        End Sub

#End Region
#Region " Factory Methods "

        Friend Shared Function NewSIDS(ByVal pParent As SI) As SIDs
            Return New SIDs(pParent)
        End Function

        Friend Shared Function GetSIDS(ByVal dr As SafeDataReader, ByVal parent As SI) As SIDs
            Dim ret = New SIDs(dr, parent)
            ret.MarkAsChild()
            Return ret
        End Function

        Private Sub New(ByVal parent As SI)
            _parent = parent
            MarkAsChild()
        End Sub

        Private Sub New(ByVal dr As SafeDataReader, ByVal parent As SI)
            _parent = parent
            MarkAsChild()
            Fetch(dr)
        End Sub

#End Region ' Factory Methods
#Region " Data Access "

        Private Sub Fetch(ByVal dr As SafeDataReader)
            RaiseListChangedEvents = False

            'Dim suppressChildValidation = True
            While dr.Read()
                Dim line = SID.GetChildSID(dr)
                Me.Add(line)
            End While

            RaiseListChangedEvents = True
        End Sub

        Friend Sub Update(ByVal cn As IDbConnection, ByVal parent As SI)
            RaiseListChangedEvents = False

            ' loop through each deleted child object
            For Each deletedChild As SID In DeletedList
                deletedChild._DTB = parent._DTB
                deletedChild.DeleteSelf(cn)
            Next
            DeletedList.Clear()

            ' loop through each non-deleted child object
            For Each child As SID In Me
                child._DTB = parent._DTB
                child.DocumentNo = parent.DocumentNo
                'child.OrderNo = parent.OrderNo
                If child.IsNew Then
                    child.Insert(cn)
                Else
                    child.Update(cn)
                End If
            Next

            RaiseListChangedEvents = True
        End Sub

#End Region ' Data Access                   
    End Class

End Namespace
