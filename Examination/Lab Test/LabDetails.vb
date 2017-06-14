Imports System
Imports System.Data

Imports System.Text.RegularExpressions
Imports Csla
Imports Csla.Data
Imports pbs.Helper

Namespace MC

    <Serializable()>
    Public Class LabDets
        Inherits Csla.BusinessListBase(Of LabDets, LABDET)

#Region " Business Methods"
        Friend _parent As LAB = Nothing

        Protected Overrides Function AddNewCore() As Object
            If _parent IsNot Nothing Then
                Dim theNewLine = LABDET.NewLABDET(_parent.LineNo)
                AddNewLine(theNewLine)
                theNewLine.CheckRules()
                Return theNewLine
            Else
                Return MyBase.AddNewCore
            End If
        End Function

        Friend Sub AddNewLine(ByVal pline As LABDET)
            If pline Is Nothing Then Exit Sub

            'get the next line number
            Dim nextnumber As Integer = 1
            If Me.Count > 0 Then
                Dim allNumbers = (From line In Me Select line._lineNo).ToList
                nextnumber = allNumbers.Max + 1
            End If

            pline._lineNo = nextnumber

            'Populate _line with info from parent here

            Me.Add(pline)

        End Sub

#End Region

#Region " Factory Methods "

        Friend Shared Function NewLabDets(ByVal pParent As LAB) As LabDets
            Return New LabDets(pParent)
        End Function

        Friend Shared Function GetLabDets(ByVal dr As SafeDataReader, ByVal parent As LAB) As LabDets
            Dim ret = New LabDets(dr, parent)
            ret.MarkAsChild()
            Return ret
        End Function

        Private Sub New(ByVal parent As LAB)
            _parent = parent
            MarkAsChild()
        End Sub

        Private Sub New(ByVal dr As SafeDataReader, ByVal parent As LAB)
            _parent = parent
            MarkAsChild()
            Fetch(dr)
        End Sub

#End Region ' Factory Methods

#Region " Data Access "

        Private Sub Fetch(ByVal dr As SafeDataReader)
            RaiseListChangedEvents = False

            Dim suppressChildValidation = True
            While dr.Read()
                Dim Line = LABDET.GetChilLABDET(dr)
                Me.Add(Line)
            End While

            RaiseListChangedEvents = True
        End Sub

        Friend Sub Update(ByVal cn As IDbConnection, ByVal parent As LAB)
            RaiseListChangedEvents = False

            ' loop through each deleted child object
            For Each deletedChild As LABDET In DeletedList
                deletedChild._DTB = parent._DTB
                deletedChild.DeleteSelf(cn)
            Next
            DeletedList.Clear()

            ' loop through each non-deleted child object
            For Each child As LABDET In Me
                child._DTB = parent._DTB
                child.LabId = parent.LineNo
                'child.OrderNo = parent.OrderNo
                If child.IsNew Then
                    child.Insert(cn)
                Else
                    'child.Update(cn)
                    child.Update(cn)
                End If
            Next

            RaiseListChangedEvents = True
        End Sub

#End Region ' Data Access                   
    End Class

End Namespace