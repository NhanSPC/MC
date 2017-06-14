
Imports pbs.Helper
Imports pbs.Helper.Interfaces
Imports System.Data
Imports Csla
Imports Csla.Data
Imports Csla.Validation

Namespace MC

    <Serializable()>
    Public Class PRESCRIPTIONInfo
        Inherits Csla.ReadOnlyBase(Of PRESCRIPTIONInfo)
        Implements IComparable
        Implements IInfo
        Implements IDocLink
        'Implements IInfoStatus


#Region " Business Properties and Methods "


        Private _lineNo As String = String.Empty
        Public ReadOnly Property LineNo() As String
            Get
                Return _lineNo
            End Get
        End Property

        Private _patientCode As String = String.Empty
        Public ReadOnly Property PatientCode() As String
            Get
                Return _patientCode
            End Get
        End Property

        Private _checkinNo As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        Public ReadOnly Property CheckinNo() As String
            Get
                Return _checkinNo.Text
            End Get
        End Property

        Private _doctorAppointed As String = String.Empty
        Public ReadOnly Property DoctorAppointed() As String
            Get
                Return _doctorAppointed
            End Get
        End Property

        Private _prescriptionDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property PrescriptionDate() As String
            Get
                Return _prescriptionDate.Text
            End Get
        End Property

        Private _samplePrescriptionCode As String = String.Empty
        Public ReadOnly Property SamplePrescriptionCode() As String
            Get
                Return _samplePrescriptionCode
            End Get
        End Property

        Private _updated As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property Updated() As String
            Get
                Return _updated.Text
            End Get
        End Property

        Private _updatedBy As String = String.Empty
        Public ReadOnly Property UpdatedBy() As String
            Get
                Return _updatedBy
            End Get
        End Property

        'Get ID
        Protected Overrides Function GetIdValue() As Object
            Return _lineNo
        End Function

        'IComparable
        Public Function CompareTo(ByVal IDObject) As Integer Implements System.IComparable.CompareTo
            Dim ID = IDObject.ToString
            Dim pLineNo As String = ID.Trim
            If _lineNo < pLineNo Then Return -1
            If _lineNo > pLineNo Then Return 1
            Return 0
        End Function

        Public ReadOnly Property Code As String Implements IInfo.Code
            Get
                Return _lineNo.ToString
            End Get
        End Property

        Public ReadOnly Property LookUp As String Implements IInfo.LookUp
            Get
                Return _patientCode
            End Get
        End Property

        Public ReadOnly Property Description As String Implements IInfo.Description
            Get
                Return _doctorAppointed
            End Get
        End Property


        Public Sub InvalidateCache() Implements IInfo.InvalidateCache
            PRESCRIPTIONInfoList.InvalidateCache()
        End Sub


#End Region 'Business Properties and Methods

#Region " Factory Methods "

        Friend Shared Function GetPRESCRIPTIONInfo(ByVal dr As SafeDataReader) As PRESCRIPTIONInfo
            Return New PRESCRIPTIONInfo(dr)
        End Function

        Friend Shared Function EmptyPRESCRIPTIONInfo(Optional ByVal pLineNo As String = "") As PRESCRIPTIONInfo
            Dim info As PRESCRIPTIONInfo = New PRESCRIPTIONInfo
            With info
                ._lineNo = pLineNo

            End With
            Return info
        End Function

        Private Sub New(ByVal dr As SafeDataReader)
            _lineNo = dr.GetInt32("LINE_NO")
            _patientCode = dr.GetString("PATIENT_CODE").TrimEnd
            _checkinNo.Text = dr.GetInt32("CHECKIN_NO")
            _doctorAppointed = dr.GetString("DOCTOR_APPOINTED").TrimEnd
            _prescriptionDate.Text = dr.GetInt32("PRESCRIPTION_DATE")
            _samplePrescriptionCode = dr.GetString("SAMPLE_PRESCRIPTION_CODE").TrimEnd
            _updated.Text = dr.GetInt32("UPDATED")
            _updatedBy = dr.GetString("UPDATED_BY").TrimEnd

        End Sub

        Private Sub New()
        End Sub


#End Region ' Factory Methods

#Region "IDoclink"
        Public Function Get_DOL_Reference() As String Implements IDocLink.Get_DOL_Reference
            Return String.Format("{0}#{1}", Get_TransType, _lineNo)
        End Function

        Public Function Get_TransType() As String Implements IDocLink.Get_TransType
            Return Me.GetType.ToClassSchemaName.Leaf
        End Function
#End Region

    End Class

End Namespace