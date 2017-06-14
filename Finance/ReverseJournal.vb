Imports pbs.Helper.Interfaces
Imports pbs.Helper

Namespace MC
    Public Class ReverseJournal
        Implements IRunable

        Public ReadOnly Property Notes As String Implements IRunable.Notes
            Get
                Return <text>Syntax: pbs.BO.MC.ReverseJournal?JrnalNo=xxx&amp;Period=
Reverse a journal and mark it as correction. Journal with allocated transactions is not allowed to reverse. User must remove allocation marker first.
If Period is not defined then the the reversal journal having the same period as original journal
</text>.Value.RemoveDoubleSpaces
            End Get
        End Property

        Public Sub Run(args As pbsCmdArgs) Implements IRunable.Run
            Dim originalJn As Integer = args.GetValueByKey("JrnalNo", args.GetDefaultParameter).ToInteger
            Dim period = args.GetValueByKey("Period", String.Empty)
            If pbs.BO.MC.JE.Exists(originalJn) Then
                Dim original = pbs.BO.MC.JE.GetJE(originalJn)
                original.ReverseJournal(period)
            End If
        End Sub
    End Class

    Public Class ReverseJournalLine
        Implements IRunable

        Public ReadOnly Property Notes As String Implements IRunable.Notes
            Get
                Return <text>Syntax: pbs.BO.MC.ReverseJournalLine?LineNo=xxx&amp;Period=
Reverse a journal line and mark it as correction. Journal Lines with allocated transactions are not allowed to reverse. User must remove allocation marker first.
If Period is not defined then the the reversal Line having the same period as original line
</text>.Value.RemoveDoubleSpaces
            End Get
        End Property

        Public Sub Run(args As pbsCmdArgs) Implements IRunable.Run
            Dim originalJn As Integer = args.GetValueByKey("LineNo", 0).ToInteger

            If originalJn > 0 Then
                Dim period = args.GetValueByKey("Period", String.Empty)
                If pbs.BO.MC.MCLDG.Exists(originalJn) Then
                    Dim original = pbs.BO.MC.MCLDG.GetMCLDG(originalJn)
                    original.ReverseJournalLine(period)
                End If
            End If

        End Sub
    End Class
End Namespace

