Imports System
Imports System.Data

Imports Csla
Imports Csla.Data
Imports pbs.Helper

Namespace MC

    <Serializable()> _
    Public Class PrescriptionDetails
        Inherits Csla.BusinessListBase(Of PrescriptionDetails, PrescriptionDetail)

#Region " Business Methods"
        Friend _parent As PRESCRIPTION = Nothing

        Protected Overrides Function AddNewCore() As Object
            If _parent IsNot Nothing Then
                Dim theNewLine = PrescriptionDetail.NewPrescriptionDetailChild(_parent.LineNo)
                AddNewLine(theNewLine)
                theNewLine.CheckRules()
                Return theNewLine
            Else
                Return MyBase.AddNewCore
            End If
        End Function

        Friend Sub AddNewLine(ByVal pline As PrescriptionDetail)
            If pline Is Nothing Then Exit Sub

            'get the next line number
            Dim nextnumber As Integer = 1
            If Me.Count > 0 Then
                Dim allNumbers = (From line In Me Select line._lineNo).ToList
                nextnumber = allNumbers.Max + 1
            End If
            pline._lineNo = nextnumber

            '_line.{LINE_NO} = String.Format("{0:00000}", nextnumber)

            'Populate _line with info from parent here

            Me.Add(pline)

        End Sub

#End Region
#Region " Factory Methods "

        Friend Shared Function NewPrescriptionDetails(ByVal pParent As PRESCRIPTION) As PrescriptionDetails
            Return New PrescriptionDetails(pParent)
        End Function

        Friend Shared Function GetPrescriptionDetails(ByVal dr As SafeDataReader, ByVal parent As PRESCRIPTION) As PrescriptionDetails
            Dim ret = New PrescriptionDetails(dr, parent)
            ret.MarkAsChild()
            Return ret
        End Function

        Private Sub New(ByVal parent As PRESCRIPTION)
            _parent = parent
            MarkAsChild()
        End Sub

        Private Sub New(ByVal dr As SafeDataReader, ByVal parent As PRESCRIPTION)
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
                Dim Line = PrescriptionDetail.GetChildPrescriptionDetail(dr)
                Me.Add(Line)
            End While

            RaiseListChangedEvents = True
        End Sub

        Friend Sub Update(ByVal cn As IDbConnection, ByVal parent As PRESCRIPTION)
            RaiseListChangedEvents = False

            ' loop through each deleted child object
            For Each deletedChild As PrescriptionDetail In DeletedList
                deletedChild._DTB = parent._DTB
                deletedChild.DeleteSelf(cn)
            Next
            DeletedList.Clear()

            ' loop through each non-deleted child object
            For Each child As PrescriptionDetail In Me
                child._DTB = parent._DTB
                child.PrescriptionNo = parent.LineNo
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