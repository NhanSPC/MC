Imports System
Imports System.Data

Imports Csla
Imports Csla.Data
Imports pbs.Helper

Namespace MC

    ''' <summary>
    ''' Children of SMJE
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public Class MCLDGs
        Inherits Csla.BusinessListBase(Of MCLDGs, MCLDG)

#Region " Business Methods"
        Friend _parent As JE = Nothing

        Protected Overrides Function AddNewCore() As Object
            If _parent IsNot Nothing Then
                Dim theNewLine = MCLDG.NewChildMCLDG()
                theNewLine._DTB = _parent._DTB
                AddNewLine(theNewLine)
                theNewLine.CheckRules()
                Return theNewLine
            Else
                Return MyBase.AddNewCore
            End If
        End Function

        Friend Sub AddNewLine(ByVal _line As MCLDG)
            If _line Is Nothing Then Exit Sub

            'Populate _line with info from parent here
            If _parent IsNot Nothing Then
                Dim pdic = BOFactory.Obj2Dictionary(_parent)
                pdic = pdic.Merge(pbs.BO.Rules.NumberingRules.GetPresetNumbering(_line.GetType.ToString), True)
                BOFactory.ApplyPreset(_line, pdic)
                _line.TransType = _parent.TransType

            End If

            Me.Add(_line)

        End Sub

#End Region

#Region " Factory Methods "

        Friend Shared Function NewMCLDGs(ByVal pParent As JE) As MCLDGs
            Return New MCLDGs(pParent)
        End Function

        Friend Shared Function GetMCLDGs(ByVal dr As SafeDataReader, ByVal parent As JE) As MCLDGs
            Dim ret = New MCLDGs(dr, parent)
            ret.MarkAsChild()
            Return ret
        End Function

        Private Sub New(ByVal parent As JE)
            _parent = parent
            MarkAsChild()
        End Sub

        Private Sub New(ByVal dr As SafeDataReader, ByVal parent As JE)
            _parent = parent
            MarkAsChild()
            Fetch(dr)
        End Sub

#End Region ' Factory Methods

#Region " Data Access "

        Friend Sub SetLineDCAfterFetching()
            If _parent IsNot Nothing Then

                Me.RaiseListChangedEvents = False

                For Each line In Me
                    If line.DC = _parent.GetJrnalTypeInfo.DebitCredit Then
                        line.Quantity = Math.Abs(line._quantity.Float)
                    Else
                        line.Amount = Math.Abs(line._amount.Float) * -1
                    End If
                    line.MarkAsClean()
                Next

                Me.RaiseListChangedEvents = True

            End If
        End Sub

        Private Sub Fetch(ByVal dr As SafeDataReader)
            RaiseListChangedEvents = False

            Dim suppressChildValidation = True
            While dr.Read()

                Dim Line = MCLDG.GetChildMCLDG(dr, suppressChildValidation)

                If _parent IsNot Nothing Then
                    Line._DTB = _parent._DTB
                    If Line.DC = _parent.GetJrnalTypeInfo.DebitCredit Then
                        Line.Quantity = Math.Abs(Line._quantity.Float)
                    Else
                        Line.Amount = Math.Abs(Line._amount.Float) * -1
                    End If
                    Line.MarkAsClean()
                End If
                Me.Add(Line)
            End While

            RaiseListChangedEvents = True
        End Sub

        Friend Sub Update(ByVal cn As IDbConnection, ByVal parent As JE)
            RaiseListChangedEvents = False

            ' loop through each deleted child object
            For Each deletedChild As MCLDG In DeletedList
                deletedChild.CopyParentInfo(parent)
                deletedChild.DeleteSelf(cn)
            Next
            DeletedList.Clear()

            ' loop through each non-deleted child object
            For Each child As MCLDG In Me
                child.CopyParentInfo(parent)

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