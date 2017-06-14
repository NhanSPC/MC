Imports FlexCel.Core
Imports FlexCel.Report
Imports pbs.BO.SQLBuilder
Imports System.Data
Imports pbs.Helper
Imports pbs.BO

Namespace ReportTags
    ''' <summary>
    ''' Calculate PatientBalance
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PatientBalance_Imp
        Inherits TFlexCelUserFunction

        Public Sub New()
        End Sub

        ReadOnly Property Syntax As String
            Get
                Return "<#PatientBalance(pPatienCode As String; pDate As String; ValueCol as string)>"
            End Get
        End Property

        ReadOnly Property Description As String
            Get
                Return "Return PatientBalance at a specific date"
            End Get
        End Property

        ReadOnly Property Group As String
            Get
                Return "MC"
            End Get
        End Property

        Public Overloads Overrides Function Evaluate(ByVal parameters As Object()) As Object
            Try

                If parameters Is Nothing OrElse parameters.Length < 1 Then
                    ExceptionThower.BusinessRuleStop("Need 1-3 parameters : <#PatientBalance(pPatientCode As String; pDate As String; ValueCol as string)>")
                End If

                Dim pPatientCode As String = DNz(parameters(0), String.Empty).Trim
                Dim pDate As String = String.Empty
                If parameters.Count > 1 Then pDate = DNz(parameters(1), String.Empty).Trim

                Dim ValueCol As String = String.Empty
                If parameters.Count > 2 Then ValueCol = DNz(parameters(2), String.Empty).Trim

                Dim ret As Decimal = 0
                Dim dicID = String.Format("__{0}__{1}_{2}", pPatientCode, pDate, ValueCol)
                If GetCachedDic.ContainsKey(dicID) Then
                    ret = GetCachedDic(dicID)
                Else
                    ret = pbs.BO.MC.MCLDGInfoList.Query.GetPatientBalanceAtDate(pPatientCode, pDate, ValueCol)

                    GetCachedDic.Add(dicID, ret)
                End If

                Return ret

            Catch ex As Exception
                '  TextLogger.Log("Evaluate Csum failed")
                Return 0
                If Context.DebugMode Then TextLogger.Log(ex)
            End Try
        End Function

        Private Shared Function GetCachedDic() As Dictionary(Of String, Decimal)
            Dim _cachedLookup = Csla.ClientContext("____PatientBalanceAtDate____")
            If _cachedLookup Is Nothing Then
                _cachedLookup = New Dictionary(Of String, Decimal)()
                Csla.ClientContext("____PatientBalanceAtDate____") = _cachedLookup
            End If

            Return _cachedLookup
        End Function

        ''' <summary>
        ''' Call this on refreshuipart
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub InvalidateUIContextCache()
            Csla.ClientContext("____PatientBalanceAtDate____") = Nothing
        End Sub

    End Class

    ''' <summary>
    ''' Student Balance
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PatientBalance
        Inherits TUserDefinedFunction

        Public Sub New(ByVal functionName As String)
            MyBase.New(functionName)
        End Sub

        Public Overrides Function Evaluate(ByVal arguments As FlexCel.Core.TUdfEventArgs, ByVal parameters() As Object) As Object
            Dim Err As TFlxFormulaErrorValue = TFlxFormulaErrorValue.ErrValue

            Try
                If parameters Is Nothing OrElse parameters.Length < 1 Then
                    ExceptionThower.BusinessRuleStop("PatientBalance(pPatientCode As String, pDate As String, ValueCol as string)")
                End If

                Dim params As New List(Of Object)

                params.Add(ParseCellValue.GetParameterValue(parameters(0), arguments.Xls))

                If parameters.Count > 1 Then params.Add(ParseCellValue.GetParameterValue(parameters(1), arguments.Xls))
                If parameters.Count > 2 Then params.Add(ParseCellValue.GetParameterValue(parameters(2), arguments.Xls))

                Dim _Imp As New PatientBalance_Imp

                Return _Imp.Evaluate(params.ToArray)

            Catch ex As Exception
                TextLogger.Log(ex)
                Return Err
            End Try

        End Function
    End Class

End Namespace



