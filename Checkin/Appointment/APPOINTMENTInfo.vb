
Imports pbs.Helper
Imports pbs.Helper.Interfaces
Imports System.Data
Imports Csla
Imports Csla.Data
Imports Csla.Validation

Namespace MC

    <Serializable()>
    Public Class APPOINTMENTInfo
        Inherits Csla.ReadOnlyBase(Of APPOINTMENTInfo)
        Implements IComparable
        Implements IInfo
        Implements IDocLink
        'Implements IInfoStatus


#Region " Business Properties and Methods "


        Private _lineNo As Integer
        Public ReadOnly Property LineNo() As Integer
            Get
                Return _lineNo
            End Get
        End Property

        Private _clinicCode As String = String.Empty
        Public ReadOnly Property ClinicCode() As String
            Get
                Return _clinicCode
            End Get
        End Property

        Private _patientCode As String = String.Empty
        Public ReadOnly Property PatientCode() As String
            Get
                Return _patientCode
            End Get
        End Property

        Private _name As String = String.Empty
        Public ReadOnly Property Name() As String
            Get
                Return _name
            End Get
        End Property

        Private _dob As pbs.Helper.SmartDate = New pbs.Helper.SmartDate(0)
        Public ReadOnly Property Dob() As String
            Get
                Return _dob.DateViewFormat
            End Get
        End Property

        Private _gender As String = String.Empty
        Public ReadOnly Property Gender() As String
            Get
                Return _gender
            End Get
        End Property

        Private _phone As String = String.Empty
        Public ReadOnly Property Phone() As String
            Get
                Return _phone
            End Get
        End Property

        Private _email As String = String.Empty
        Public ReadOnly Property Email() As String
            Get
                Return _email
            End Get
        End Property

        Private _address As String = String.Empty
        Public ReadOnly Property Address() As String
            Get
                Return _address
            End Get
        End Property

        Private _ward As String = String.Empty
        Public ReadOnly Property Ward() As String
            Get
                Return _ward
            End Get
        End Property

        Private _district As String = String.Empty
        Public ReadOnly Property District() As String
            Get
                Return _district
            End Get
        End Property

        Private _city As String = String.Empty
        Public ReadOnly Property City() As String
            Get
                Return _city
            End Get
        End Property

        Private _status As String = String.Empty
        Public ReadOnly Property Status() As String
            Get
                Return _status
            End Get
        End Property

        Private _createDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property CreateDate() As String
            Get
                Return _createDate.DateViewFormat
            End Get
        End Property

        Private _appointmentDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property AppointmentDate() As String
            Get
                Return _appointmentDate.DateViewFormat
            End Get
        End Property

        Private _appointmentType As String = String.Empty
        Public ReadOnly Property AppointmentType() As String
            Get
                Return _appointmentType
            End Get
        End Property

        Private _reminderDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property ReminderDate() As String
            Get
                Return _reminderDate.DateViewFormat
            End Get
        End Property

        Private _notes As String = String.Empty
        Public ReadOnly Property Notes() As String
            Get
                Return _notes
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
            Dim pLineNo As String = ID.Trim.ToInteger
            If _lineNo < pLineNo Then Return -1
            If _lineNo > pLineNo Then Return 1
            Return 0
        End Function

        Public ReadOnly Property Code As String Implements IInfo.Code
            Get
                Return _lineNo
            End Get
        End Property

        Public ReadOnly Property LookUp As String Implements IInfo.LookUp
            Get
                Return _patientCode
            End Get
        End Property

        Public ReadOnly Property Description As String Implements IInfo.Description
            Get
                Return Nz(_notes, String.Format(ResStr("Appointment of {0} at {1} {2}"), Name, Status, _appointmentDate.Text))
            End Get
        End Property


        Public Sub InvalidateCache() Implements IInfo.InvalidateCache
            APPOINTMENTInfoList.InvalidateCache()
        End Sub


#End Region 'Business Properties and Methods

#Region " Factory Methods "

        Friend Shared Function GetAPPOINTMENTInfo(ByVal dr As SafeDataReader) As APPOINTMENTInfo
            Return New APPOINTMENTInfo(dr)
        End Function

        Friend Shared Function EmptyAPPOINTMENTInfo(Optional ByVal pLineNo As String = "") As APPOINTMENTInfo
            Dim info As APPOINTMENTInfo = New APPOINTMENTInfo
            With info
                ._lineNo = pLineNo

            End With
            Return info
        End Function

        Private Sub New(ByVal dr As SafeDataReader)
            _lineNo = dr.GetInt32("LINE_NO")
            _clinicCode = dr.GetString("CLINIC_CODE").TrimEnd
            _patientCode = dr.GetString("PATIENT_CODE").TrimEnd
            _name = dr.GetString("NAME").TrimEnd
            _dob.Text = dr.GetInt32("DOB")
            _phone = dr.GetString("PHONE").TrimEnd
            _email = dr.GetString("EMAIL").TrimEnd
            _address = dr.GetString("ADDRESS").TrimEnd
            _ward = dr.GetString("WARD").TrimEnd
            _district = dr.GetString("DISTRICT").TrimEnd
            _city = dr.GetString("CITY").TrimEnd
            _gender = dr.GetString("GENDER").TrimEnd
            _status = dr.GetString("STATUS").TrimEnd
            _createDate.Text = dr.GetInt32("CREATE_DATE")
            _appointmentDate.Text = dr.GetInt32("APPOINTMENT_DATE")
            _appointmentType = dr.GetString("APPOINTMENT_TYPE").TrimEnd
            _reminderDate.Text = dr.GetInt32("REMINDER_DATE")
            _notes = dr.GetString("NOTES").TrimEnd
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