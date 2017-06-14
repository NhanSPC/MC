
Imports pbs.Helper
Imports pbs.Helper.Interfaces
Imports System.Data
Imports Csla
Imports Csla.Data
Imports Csla.Validation

Namespace MC

    <Serializable()> _
    Public Class CHECKINInfo
        Inherits Csla.ReadOnlyBase(Of CHECKINInfo)
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

        Private _appointmentNo As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        Public ReadOnly Property AppointmentNo() As String
            Get
                Return _appointmentNo.Text
            End Get
        End Property

        Private _referenceNo As String = String.Empty
        Public ReadOnly Property ReferenceNo() As String
            Get
                Return _referenceNo
            End Get
        End Property

        Private _medicalRecordCode As String = String.Empty
        Public ReadOnly Property MedicalRecordCode() As String
            Get
                Return _medicalRecordCode
            End Get
        End Property

        Private _patientCode As String = String.Empty
        Public ReadOnly Property PatientCode() As String
            Get
                Return _patientCode
            End Get
        End Property

        Private _checkinDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate(0)
        Public ReadOnly Property CheckinDate() As String
            Get
                Return _checkinDate.DateViewFormat
            End Get
        End Property

        Private _checkinTime As pbs.Helper.SmartTime = New pbs.Helper.SmartTime()
        Public ReadOnly Property CheckinTime() As String
            Get
                Return _checkinTime.Text
            End Get
        End Property

        Private _checkinType As String = String.Empty
        Public ReadOnly Property CheckinType() As String
            Get
                Return _checkinType
            End Get
        End Property

        Private _transferFrom As String = String.Empty
        Public ReadOnly Property TransferFrom() As String
            Get
                Return _transferFrom
            End Get
        End Property

        Private _transferFromName As String = String.Empty
        Public ReadOnly Property TransferFromName() As String
            Get
                Return _transferFromName
            End Get
        End Property

        Private _department As String = String.Empty
        Public ReadOnly Property Department() As String
            Get
                Return _department
            End Get
        End Property

        Private _deptCheckinTime As pbs.Helper.SmartTime = New pbs.Helper.SmartTime()
        Public ReadOnly Property DeptCheckinTime() As String
            Get
                Return _deptCheckinTime.Text
            End Get
        End Property

        Private _deptCheckinDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property DeptCheckinDate() As String
            Get
                Return _deptCheckinDate.Text
            End Get
        End Property

        Private _cabinetNo As String = String.Empty
        Public ReadOnly Property CabinetNo() As String
            Get
                Return _cabinetNo
            End Get
        End Property

        Private _checkoutTime As pbs.Helper.SmartTime = New pbs.Helper.SmartTime()
        Public ReadOnly Property CheckoutTime() As String
            Get
                Return _checkoutTime.Text
            End Get
        End Property

        Private _checkoutDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property CheckoutDate() As String
            Get
                Return _checkoutDate.Text
            End Get
        End Property

        Private _checkoutType As String = String.Empty
        Public ReadOnly Property CheckoutType() As String
            Get
                Return _checkoutType
            End Get
        End Property

        Private _treatmentResult As String = String.Empty
        Public ReadOnly Property TreatmentResult() As String
            Get
                Return _treatmentResult
            End Get
        End Property

        Private _doctor As String = String.Empty
        Public ReadOnly Property Doctor() As String
            Get
                Return _doctor
            End Get
        End Property

        Private _hospitalizedReason As String = String.Empty
        Public ReadOnly Property HospitalizedReason() As String
            Get
                Return _hospitalizedReason
            End Get
        End Property

        Private _dayOfDisease As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        Public ReadOnly Property DayOfDisease() As String
            Get
                Return _dayOfDisease.Text
            End Get
        End Property

        Private _symptomsCode As String = String.Empty
        Public ReadOnly Property SymptomsCode() As String
            Get
                Return _symptomsCode
            End Get
        End Property

        Private _symptoms As String = String.Empty
        Public ReadOnly Property Symptoms() As String
            Get
                Return _symptoms
            End Get
        End Property

        Private _history As String = String.Empty
        Public ReadOnly Property History() As String
            Get
                Return _history
            End Get
        End Property

        Private _allergic As String = String.Empty
        Public ReadOnly Property Allergic() As String
            Get
                Return _allergic
            End Get
        End Property

        Private _allergens As String = String.Empty
        Public ReadOnly Property Allergens() As String
            Get
                Return _allergens
            End Get
        End Property

        Private _family As String = String.Empty
        Public ReadOnly Property Family() As String
            Get
                Return _family
            End Get
        End Property

        Private _mainCause As String = String.Empty
        Public ReadOnly Property MainCause() As String
            Get
                Return _mainCause
            End Get
        End Property

        Private _relateCause As String = String.Empty
        Public ReadOnly Property RelateCause() As String
            Get
                Return _relateCause
            End Get
        End Property

        Private _distinction As String = String.Empty
        Public ReadOnly Property Distinction() As String
            Get
                Return _distinction
            End Get
        End Property

        Private _diagnosisCode As String = String.Empty
        Public ReadOnly Property DiagnosisCode() As String
            Get
                Return _diagnosisCode
            End Get
        End Property

        Private _diagnosis As String = String.Empty
        Public ReadOnly Property Diagnosis() As String
            Get
                Return _diagnosis
            End Get
        End Property

        Private _treatment As String = String.Empty
        Public ReadOnly Property Treatment() As String
            Get
                Return _treatment
            End Get
        End Property

        Private _doctorAdvice As String = String.Empty
        Public ReadOnly Property DoctorAdvice() As String
            Get
                Return _doctorAdvice
            End Get
        End Property

        Private _reexamDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property ReexamDate() As String
            Get
                Return _reexamDate.Text
            End Get
        End Property

        Private _dead As String = String.Empty
        Public ReadOnly Property Dead() As Boolean
            Get
                Return _dead.ToBoolean
            End Get
        End Property

        Private _surgery As String = String.Empty
        Public ReadOnly Property Surgery() As Boolean
            Get
                Return _surgery.ToBoolean
            End Get
        End Property

        Private _notes As String = String.Empty
        Public ReadOnly Property Notes() As String
            Get
                Return _notes
            End Get
        End Property

        Private _status As String = String.Empty
        Public ReadOnly Property Status() As String
            Get
                Return _status
            End Get
        End Property

        Private _ncCi9 As String = String.Empty
        Public ReadOnly Property NcCi9() As String
            Get
                Return _ncCi9
            End Get
        End Property

        Private _ncCi8 As String = String.Empty
        Public ReadOnly Property NcCi8() As String
            Get
                Return _ncCi8
            End Get
        End Property

        Private _ncCi7 As String = String.Empty
        Public ReadOnly Property NcCi7() As String
            Get
                Return _ncCi7
            End Get
        End Property

        Private _ncCi6 As String = String.Empty
        Public ReadOnly Property NcCi6() As String
            Get
                Return _ncCi6
            End Get
        End Property

        Private _ncCi5 As String = String.Empty
        Public ReadOnly Property NcCi5() As String
            Get
                Return _ncCi5
            End Get
        End Property

        Private _ncCi4 As String = String.Empty
        Public ReadOnly Property NcCi4() As String
            Get
                Return _ncCi4
            End Get
        End Property

        Private _ncCi3 As String = String.Empty
        Public ReadOnly Property NcCi3() As String
            Get
                Return _ncCi3
            End Get
        End Property

        Private _ncCi2 As String = String.Empty
        Public ReadOnly Property NcCi2() As String
            Get
                Return _ncCi2
            End Get
        End Property

        Private _ncCi1 As String = String.Empty
        Public ReadOnly Property NcCi1() As String
            Get
                Return _ncCi1
            End Get
        End Property

        Private _ncCi0 As String = String.Empty
        Public ReadOnly Property NcCi0() As String
            Get
                Return _ncCi0
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

        Private _pulse As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        Public ReadOnly Property Pulse() As String
            Get
                Return _pulse.Text
            End Get
        End Property

        Private _temperature As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property Temperature() As String
            Get
                Return _temperature.Text
            End Get
        End Property

        Private _bloodPressure As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property BloodPressure() As String
            Get
                Return _bloodPressure.Text
            End Get
        End Property

        Private _breathingRate As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        Public ReadOnly Property BreathingRate() As String
            Get
                Return _breathingRate.Text
            End Get
        End Property

        Private _weight As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property Weight() As String
            Get
                Return _weight.Text
            End Get
        End Property

        Private _height As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property Height() As String
            Get
                Return _height.Text
            End Get
        End Property

        Private _bloodgroup As String = String.Empty
        Public ReadOnly Property Bloodgroup() As String
            Get
                Return _bloodgroup
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
                Return String.Format("Check-in date: {0}, Patient: {1}.{2}", _checkinDate, _patientCode, PatientName())
            End Get
        End Property


        Public Sub InvalidateCache() Implements IInfo.InvalidateCache
            CHECKINInfoList.InvalidateCache()
        End Sub

        'this function is used to return patient name
        Private Function PatientName() As String
            Dim patient = PATIENTInfoList.GetPATIENTInfo(_patientCode)
            Return patient.Fullname
        End Function


#End Region 'Business Properties and Methods

#Region " Factory Methods "

        Friend Shared Function GetCHECKINInfo(ByVal dr As SafeDataReader) As CHECKINInfo
            Return New CHECKINInfo(dr)
        End Function

        Friend Shared Function EmptyCHECKINInfo(Optional ByVal pLineNo As String = "") As CHECKINInfo
            Dim info As CHECKINInfo = New CHECKINInfo
            With info
                ._lineNo = pLineNo

            End With
            Return info
        End Function

        Private Sub New(ByVal dr As SafeDataReader)
            _lineNo = dr.GetInt32("LINE_NO")
            _appointmentNo.Text = dr.GetInt32("APPOINTMENT_NO")
            _referenceNo = dr.GetString("REFERENCE_NO").TrimEnd
            _medicalRecordCode = dr.GetString("MEDICAL_RECORD_CODE").TrimEnd
            _patientCode = dr.GetString("PATIENT_CODE").TrimEnd
            _checkinDate.Text = dr.GetInt32("CHECKIN_DATE")
            _checkinTime.Text = dr.GetInt32("CHECKIN_TIME")
            _checkinType = dr.GetString("CHECKIN_TYPE").TrimEnd
            _transferFrom = dr.GetString("TRANSFER_FROM").TrimEnd
            _transferFromName = dr.GetString("TRANSFER_FROM_NAME").TrimEnd
            _department = dr.GetString("DEPARTMENT").TrimEnd
            _deptCheckinTime.Text = dr.GetInt32("DEPT_CHECKIN_TIME")
            _deptCheckinDate.Text = dr.GetInt32("DEPT_CHECKIN_DATE")
            _cabinetNo = dr.GetString("CABINET_NO").TrimEnd
            _checkoutTime.Text = dr.GetInt32("CHECKOUT_TIME")
            _checkoutDate.Text = dr.GetInt32("CHECKOUT_DATE")
            _checkoutType = dr.GetString("CHECKOUT_TYPE").TrimEnd
            _treatmentResult = dr.GetString("TREATMENT_RESULT").TrimEnd
            _doctor = dr.GetString("DOCTOR").TrimEnd
            _hospitalizedReason = dr.GetString("HOSPITALIZED_REASON").TrimEnd
            _dayOfDisease.Text = dr.GetInt32("DAY_OF_DISEASE")
            _symptomsCode = dr.GetString("SYMPTOMS_CODE").TrimEnd
            _symptoms = dr.GetString("SYMPTOMS").TrimEnd
            _history = dr.GetString("HISTORY").TrimEnd
            _allergic = dr.GetString("ALLERGIC").TrimEnd
            _allergens = dr.GetString("ALLERGENS").TrimEnd
            _family = dr.GetString("FAMILY").TrimEnd
            _mainCause = dr.GetString("MAIN_CAUSE").TrimEnd
            _relateCause = dr.GetString("RELATE_CAUSE").TrimEnd
            _distinction = dr.GetString("DISTINCTION").TrimEnd
            _diagnosisCode = dr.GetString("DIAGNOSIS_CODE").TrimEnd
            _diagnosis = dr.GetString("DIAGNOSIS").TrimEnd
            _treatment = dr.GetString("TREATMENT").TrimEnd
            _doctorAdvice = dr.GetString("DOCTOR_ADVICE").TrimEnd
            _reexamDate.Text = dr.GetInt32("REEXAM_DATE")
            _dead = dr.GetString("DEAD").TrimEnd
            _surgery = dr.GetString("SURGERY").TrimEnd
            _notes = dr.GetString("NOTES").TrimEnd
            _status = dr.GetString("STATUS").TrimEnd
            _ncCi9 = dr.GetString("NC_CI9").TrimEnd
            _ncCi8 = dr.GetString("NC_CI8").TrimEnd
            _ncCi7 = dr.GetString("NC_CI7").TrimEnd
            _ncCi6 = dr.GetString("NC_CI6").TrimEnd
            _ncCi5 = dr.GetString("NC_CI5").TrimEnd
            _ncCi4 = dr.GetString("NC_CI4").TrimEnd
            _ncCi3 = dr.GetString("NC_CI3").TrimEnd
            _ncCi2 = dr.GetString("NC_CI2").TrimEnd
            _ncCi1 = dr.GetString("NC_CI1").TrimEnd
            _ncCi0 = dr.GetString("NC_CI0").TrimEnd
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