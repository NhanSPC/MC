Imports Csla.Core
Imports pbs.BO.DataAnnotations
Imports Csla
Imports pbs.Helper

Namespace MC

    <Helper.PhoebusCommand(Hide:=True)>
    Public Class PatientBalance
        Inherits ReadOnlyBase(Of PatientBalance)
        Implements Interfaces.IInfo

        Protected Overrides Function GetIdValue() As Object
            Return _PatientCode
        End Function

        Friend _candidateId As String = String.Empty
        '<CellInfo("pbs.BO.SM.CAN")>
        'Public ReadOnly Property CandidateId() As String
        '    Get
        '        Return _candidateId
        '    End Get
        'End Property

        Friend _PatientCode As String = String.Empty
        <CellInfo("pbs.BO.MC.PATIENT")>
        Public ReadOnly Property PatientCode() As String
            Get
                Return _PatientCode
            End Get
        End Property

        Public ReadOnly Property PatientName() As String
            Get
                Return GetPatientInfo.Description
            End Get
        End Property

        Private _PatientInfo As PATIENTInfo = Nothing
        Private Function GetPatientInfo() As PATIENTInfo

            If _PatientInfo Is Nothing OrElse _PatientCode.Trim <> _PatientInfo.PatientCode.Trim Then
                If String.IsNullOrEmpty(_PatientCode) Then
                    Return PatientInfo.EmptyPatientInfo(_PatientCode)
                Else
                    _PatientInfo = PatientInfoList.GetPatientInfo(_PatientCode)
                End If
            End If
            Return _PatientInfo

        End Function

        'Private _canInfo As CANInfo = Nothing
        'Private Function GetCandidateInfo() As CANInfo

        '    If _canInfo Is Nothing OrElse _candidateId.Trim <> _canInfo.LineNo.ToString Then
        '        If String.IsNullOrEmpty(_candidateId) Then
        '            Return CANInfo.EmptyCANInfo()
        '        Else
        '            _canInfo = CANInfoList.GetCANInfo(_candidateId)
        '        End If
        '    End If
        '    Return _canInfo

        'End Function

        Friend _calculationDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate(False)
        Public ReadOnly Property CalculationDate() As String
            Get
                Return _calculationDate.DateViewFormat
            End Get
        End Property

        Friend _debit As Decimal
        Public ReadOnly Property Debit() As Decimal
            Get
                Return _debit
            End Get
        End Property

        Friend _credit As Decimal
        Public ReadOnly Property Credit() As Decimal
            Get
                Return _credit
            End Get
        End Property

        Public ReadOnly Property Balance() As Decimal
            Get
                Return _debit + _credit
            End Get
        End Property

        Public ReadOnly Property DebitCredit As String
            Get
                Return If(Balance > 0, "D", "C")
            End Get
        End Property

        Friend Shared Function Fetch(row As Dictionary(Of String, String)) As PatientBalance
            Dim ret = New PatientBalance
            ret._PatientCode = row.GetValueByKey("PATIENT_CODE", String.Empty)

            Dim dc = row.GetValueByKey("D_C", String.Empty)
            Dim amount = row.GetValueByKey("AMOUNT", 0).ToDecimal
            If dc.Equals("C", StringComparison.OrdinalIgnoreCase) Then
                ret._credit = amount
            Else
                ret._debit = amount
            End If
            Return ret
        End Function

        Public ReadOnly Property Code As String Implements Interfaces.IInfo.Code
            Get
                Return _PatientCode
            End Get
        End Property

        Public ReadOnly Property Description As String Implements Interfaces.IInfo.Description
            Get
                Return PatientName
            End Get
        End Property

        Public Sub InvalidateCache() Implements Interfaces.IInfo.InvalidateCache

        End Sub

        Public ReadOnly Property LookUp As String Implements Interfaces.IInfo.LookUp
            Get
                Return String.Empty
            End Get
        End Property

    End Class
End Namespace

