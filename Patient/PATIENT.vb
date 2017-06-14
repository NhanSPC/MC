Imports pbs.Helper
Imports System.Data

Imports Csla
Imports Csla.Data
Imports Csla.Validation
Imports pbs.BO.DataAnnotations
Imports pbs.BO.Script
Imports pbs.BO.BusinessRules


Namespace MC

    <Serializable()> _
    <DB(TableName:="pbs_MC_PATIENT_XXX")>
    Public Class PATIENT
        Inherits Csla.BusinessBase(Of PATIENT)
        Implements Interfaces.IGenPartObject
        Implements IComparable
        Implements IDocLink



#Region "Property Changed"
        Protected Overrides Sub OnDeserialized(context As Runtime.Serialization.StreamingContext)
            MyBase.OnDeserialized(context)
            AddHandler Me.PropertyChanged, AddressOf BO_PropertyChanged
        End Sub

        Private Sub BO_PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Handles Me.PropertyChanged
            Select e.PropertyName

                Case "FirstName", "LastName"
                    Me._fullname = String.Format("{0} {1}", Me._firstName, Me._lastName)

                    '    Case "OrderType"
                    '        If Not Me.GetOrderTypeInfo.ManualRef Then
                    '            Me._orderNo = POH.AutoReference
                    '        End If

                    '    Case "OrderDate"
                    '        If String.IsNullOrEmpty(Me.OrderPrd) Then Me._orderPrd.Text = Me._orderDate.Date.ToSunPeriod

                    '    Case "SuppCode"
                    '        For Each line In Lines
                    '            line._suppCode = Me.SuppCode
                    '        Next

                    '    Case "ConvCode"
                    '        If String.IsNullOrEmpty(Me.ConvCode) Then
                    '            _convRate.Float = 0
                    '        Else
                    '            Dim conv = pbs.BO.LA.CVInfoList.GetConverter(Me.ConvCode, _orderPrd, String.Empty)
                    '            If conv IsNot Nothing Then
                    '                _convRate.Float = conv.DefaultRate
                    '            End If
                    '        End If

                    '    Case Else

            End Select

            pbs.BO.Rules.CalculationRules.Calculator(sender, e)
        End Sub
#End Region

#Region " Business Properties and Methods "
        Private _DTB As String = String.Empty


        Private _patientCode As String = String.Empty
        <System.ComponentModel.DataObjectField(True, False)> _
        <CellInfo(GroupName:="Patient Info", Tips:="Enter patient code.")>
        <Rule(Required:=True)>
        Public Property PatientCode() As String
            Get
                Return _patientCode
            End Get
            Set(ByVal value As String)
                CanWriteProperty("PatientCode", True)
                If value Is Nothing Then value = String.Empty
                If Not _patientCode.Equals(value) Then
                    _patientCode = value
                    PropertyHasChanged("PatientCode")
                End If
            End Set
        End Property

        Private _firstName As String = String.Empty
        <CellInfo(GroupName:="Patient Info", Tips:="Enter first name.")>
        Public Property FirstName() As String
            Get
                Return _firstName
            End Get
            Set(ByVal value As String)
                CanWriteProperty("FirstName", True)
                If value Is Nothing Then value = String.Empty
                If Not _firstName.Equals(value) Then
                    _firstName = value
                    PropertyHasChanged("FirstName")
                End If
            End Set
        End Property

        Private _middleName As String = String.Empty
        <CellInfo(GroupName:="Patient Info", Tips:="Enter middle name.")>
        Public Property MiddleName() As String
            Get
                Return _middleName
            End Get
            Set(ByVal value As String)
                CanWriteProperty("MiddleName", True)
                If value Is Nothing Then value = String.Empty
                If Not _middleName.Equals(value) Then
                    _middleName = value
                    PropertyHasChanged("MiddleName")
                End If
            End Set
        End Property

        Private _lastName As String = String.Empty
        <CellInfo(GroupName:="Patient Info", Tips:="Enter last name.")>
        Public Property LastName() As String
            Get
                Return _lastName
            End Get
            Set(ByVal value As String)
                CanWriteProperty("LastName", True)
                If value Is Nothing Then value = String.Empty
                If Not _lastName.Equals(value) Then
                    _lastName = value
                    PropertyHasChanged("LastName")
                End If
            End Set
        End Property

        Private _fullname As String = String.Empty
        <CellInfo(GroupName:="Patient Info", Tips:="Full name of patient.")>
        Public Property Fullname() As String
            Get
                Return _fullname
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Fullname", True)
                If value Is Nothing Then value = String.Empty
                If Not _fullname.Equals(value) Then
                    _fullname = value
                    PropertyHasChanged("Fullname")
                End If
            End Set
        End Property

        Private _gender As String = String.Empty
        <CellInfo("GENDER", GroupName:="Patient Info", Tips:="Enter gender of patient.")>
        <Rule(Required:=True)>
        Public Property Gender() As String
            Get
                Return _gender
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Gender", True)
                If value Is Nothing Then value = String.Empty
                If Not _gender.Equals(value) Then
                    _gender = value
                    PropertyHasChanged("Gender")
                End If
            End Set
        End Property

        Private _dob As pbs.Helper.SmartDate = New pbs.Helper.SmartDate
        <CellInfo("Calendar", GroupName:="Patient Info", Tips:="Enter patient's day of birth.")>
        <Rule(Required:=True)>
        Public Property Dob() As String
            Get
                Return _dob.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Dob", True)
                If value Is Nothing Then value = String.Empty
                If Not _dob.Equals(value) Then
                    _dob.Text = value
                    PropertyHasChanged("Dob")
                End If
            End Set
        End Property

        Private _job As String = String.Empty
        <CellInfo("MEDI_JOB", GroupName:="Patient Info", Tips:="Enter patient's job.")>
        Public Property Job() As String
            Get
                Return _job
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Job", True)
                If value Is Nothing Then value = String.Empty
                If Not _job.Equals(value) Then
                    _job = value
                    PropertyHasChanged("Job")
                End If
            End Set
        End Property

        Private _ethnic As String = String.Empty
        <CellInfo("ETHNIC", GroupName:="Patient Info", Tips:="Enter patient's ethnic.")>
        Public Property Ethnic() As String
            Get
                Return _ethnic
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Ethnic", True)
                If value Is Nothing Then value = String.Empty
                If Not _ethnic.Equals(value) Then
                    _ethnic = value
                    PropertyHasChanged("Ethnic")
                End If
            End Set
        End Property

        Private _nationality As String = String.Empty
        <CellInfo("QUOCTICH", GroupName:="Patient Info", Tips:="Enter patient's nationality.")>
        Public Property Nationality() As String
            Get
                Return _nationality
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Nationality", True)
                If value Is Nothing Then value = String.Empty
                If Not _nationality.Equals(value) Then
                    _nationality = value
                    PropertyHasChanged("Nationality")
                End If
            End Set
        End Property

        Private _address As String = String.Empty
        <CellInfo(GroupName:="Patient's contact", Tips:="Enter patient's address.")>
        Public Property Address() As String
            Get
                Return _address
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Address", True)
                If value Is Nothing Then value = String.Empty
                If Not _address.Equals(value) Then
                    _address = value
                    PropertyHasChanged("Address")
                End If
            End Set
        End Property

        Private _ward As String = String.Empty
        <CellInfo("pbs.BO.CRM.Ward?DistrictCode=[DISTRICT]", GroupName:="Patient's contact", Tips:="Enter patient's ward.")>
        Public Property Ward() As String
            Get
                Return _ward
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Ward", True)
                If value Is Nothing Then value = String.Empty
                If Not _ward.Equals(value) Then
                    _ward = value
                    PropertyHasChanged("Ward")
                End If
            End Set
        End Property

        Private _district As String = String.Empty
        <CellInfo("pbs.BO.CRM.District?CityCode=[CITY]", GroupName:="Patient's contact", Tips:="Enter patient's district.")>
        Public Property District() As String
            Get
                Return _district
            End Get
            Set(ByVal value As String)
                CanWriteProperty("District", True)
                If value Is Nothing Then value = String.Empty
                If Not _district.Equals(value) Then
                    _district = value
                    PropertyHasChanged("District")
                End If
            End Set
        End Property

        Private _city As String = String.Empty
        <CellInfo("CITY", GroupName:="Patient's contact", Tips:="Enter patient's city.")>
        Public Property City() As String
            Get
                Return _city
            End Get
            Set(ByVal value As String)
                CanWriteProperty("City", True)
                If value Is Nothing Then value = String.Empty
                If Not _city.Equals(value) Then
                    _city = value
                    PropertyHasChanged("City")
                End If
            End Set
        End Property

        Private _workplace As String = String.Empty
        <CellInfo(GroupName:="Patient's contact", Tips:="Enter patient's workplace.")>
        Public Property Workplace() As String
            Get
                Return _workplace
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Workplace", True)
                If value Is Nothing Then value = String.Empty
                If Not _workplace.Equals(value) Then
                    _workplace = value
                    PropertyHasChanged("Workplace")
                End If
            End Set
        End Property

        Private _phone As String = String.Empty
        <CellInfo(GroupName:="Patient's contact", Tips:="Enter patient's workplace.")>
        Public Property Phone() As String
            Get
                Return _phone
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Phone", True)
                If value Is Nothing Then value = String.Empty
                If Not _phone.Equals(value) Then
                    _phone = value
                    PropertyHasChanged("Phone")
                End If
            End Set
        End Property

        Private _email As String = String.Empty
        <CellInfo(GroupName:="Patient's contact", Tips:="Enter patient's workplace.")>
        Public Property Email() As String
            Get
                Return _email
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Email", True)
                If value Is Nothing Then value = String.Empty
                If Not _email.Equals(value) Then
                    _email = value
                    PropertyHasChanged("Email")
                End If
            End Set
        End Property

        Private _emergencyContact As String = String.Empty
        <CellInfo(GroupName:="Emergency", Tips:="Enter contact used in emergency case.")>
        Public Property EmergencyContact() As String
            Get
                Return _emergencyContact
            End Get
            Set(ByVal value As String)
                CanWriteProperty("EmergencyContact", True)
                If value Is Nothing Then value = String.Empty
                If Not _emergencyContact.Equals(value) Then
                    _emergencyContact = value
                    PropertyHasChanged("EmergencyContact")
                End If
            End Set
        End Property

        Private _emergencyContactName As String = String.Empty
        <CellInfo(GroupName:="Emergency", Tips:="Enter name of person that can contact to in case of emergency.")>
        Public Property EmergencyContactName() As String
            Get
                Return _emergencyContactName
            End Get
            Set(ByVal value As String)
                CanWriteProperty("EmergencyContactName", True)
                If value Is Nothing Then value = String.Empty
                If Not _emergencyContactName.Equals(value) Then
                    _emergencyContactName = value
                    PropertyHasChanged("EmergencyContactName")
                End If
            End Set
        End Property

        Private _emergencyContactPhone As String = String.Empty
        <CellInfo(GroupName:="Emergency", Tips:="Enter telephone number used in case of emergency.")>
        Public Property EmergencyContactPhone() As String
            Get
                Return _emergencyContactPhone
            End Get
            Set(ByVal value As String)
                CanWriteProperty("EmergencyContactPhone", True)
                If value Is Nothing Then value = String.Empty
                If Not _emergencyContactPhone.Equals(value) Then
                    _emergencyContactPhone = value
                    PropertyHasChanged("EmergencyContactPhone")
                End If
            End Set
        End Property

        Private _patientType As String = String.Empty
        <CellInfo("P_TYPE", GroupName:="Other Info", Tips:="Enter patient type.")>
        Public Property PatientType() As String
            Get
                Return _patientType
            End Get
            Set(ByVal value As String)
                CanWriteProperty("PatientType", True)
                If value Is Nothing Then value = String.Empty
                If Not _patientType.Equals(value) Then
                    _patientType = value
                    PropertyHasChanged("PatientType")
                End If
            End Set
        End Property

        Private _healthInsuranceNo As String = String.Empty
        <CellInfo(GroupName:="Other Info", Tips:="Enter health insurance number.")>
        Public Property HealthInsuranceNo() As String
            Get
                Return _healthInsuranceNo
            End Get
            Set(ByVal value As String)
                CanWriteProperty("HealthInsuranceNo", True)
                If value Is Nothing Then value = String.Empty
                If Not _healthInsuranceNo.Equals(value) Then
                    _healthInsuranceNo = value
                    PropertyHasChanged("HealthInsuranceNo")
                End If
            End Set
        End Property

        Private _validTo As pbs.Helper.SmartDate = New pbs.Helper.SmartDate
        <CellInfo("CALENDAR", GroupName:="Other Info", Tips:="Enter date that Health insurance valid to.")>
        Public Property ValidTo() As String
            Get
                Return _validTo.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ValidTo", True)
                If value Is Nothing Then value = String.Empty
                If Not _validTo.Equals(value) Then
                    _validTo.Text = value
                    PropertyHasChanged("ValidTo")
                End If
            End Set
        End Property

        Private _notes As String = String.Empty
        <CellInfo(GroupName:="Other Info", Tips:="Patient's notes.", ControlType:=Forms.CtrlType.MemoEdit)>
        Public Property Notes() As String
            Get
                Return _notes
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Notes", True)
                If value Is Nothing Then value = String.Empty
                If Not _notes.Equals(value) Then
                    _notes = value
                    PropertyHasChanged("Notes")
                End If
            End Set
        End Property

Private _ncPt9 As String  = String.Empty 
        Public Property NcPt9() As String
            Get
                Return _ncPt9
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcPt9", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncPt9.Equals(value) Then
                    _ncPt9 = value
                    PropertyHasChanged("NcPt9")
                End If
            End Set
        End Property

        Private _ncPt8 As String = String.Empty
        Public Property NcPt8() As String
            Get
                Return _ncPt8
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcPt8", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncPt8.Equals(value) Then
                    _ncPt8 = value
                    PropertyHasChanged("NcPt8")
                End If
            End Set
        End Property

        Private _ncPt7 As String = String.Empty
        Public Property NcPt7() As String
            Get
                Return _ncPt7
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcPt7", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncPt7.Equals(value) Then
                    _ncPt7 = value
                    PropertyHasChanged("NcPt7")
                End If
            End Set
        End Property

        Private _ncPt6 As String = String.Empty
        Public Property NcPt6() As String
            Get
                Return _ncPt6
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcPt6", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncPt6.Equals(value) Then
                    _ncPt6 = value
                    PropertyHasChanged("NcPt6")
                End If
            End Set
        End Property

        Private _ncPt5 As String = String.Empty
        Public Property NcPt5() As String
            Get
                Return _ncPt5
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcPt5", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncPt5.Equals(value) Then
                    _ncPt5 = value
                    PropertyHasChanged("NcPt5")
                End If
            End Set
        End Property

        Private _ncPt4 As String = String.Empty
        Public Property NcPt4() As String
            Get
                Return _ncPt4
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcPt4", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncPt4.Equals(value) Then
                    _ncPt4 = value
                    PropertyHasChanged("NcPt4")
                End If
            End Set
        End Property

        Private _ncPt3 As String = String.Empty
        Public Property NcPt3() As String
            Get
                Return _ncPt3
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcPt3", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncPt3.Equals(value) Then
                    _ncPt3 = value
                    PropertyHasChanged("NcPt3")
                End If
            End Set
        End Property

        Private _ncPt2 As String = String.Empty
        Public Property NcPt2() As String
            Get
                Return _ncPt2
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcPt2", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncPt2.Equals(value) Then
                    _ncPt2 = value
                    PropertyHasChanged("NcPt2")
                End If
            End Set
        End Property

        Private _ncPt1 As String = String.Empty
        Public Property NcPt1() As String
            Get
                Return _ncPt1
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcPt1", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncPt1.Equals(value) Then
                    _ncPt1 = value
                    PropertyHasChanged("NcPt1")
                End If
            End Set
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
            Return _patientCode
        End Function

        'IComparable
        Public Function CompareTo(ByVal IDObject) As Integer Implements System.IComparable.CompareTo
            Dim ID = IDObject.ToString
            Dim pPatientCode As String = ID.Trim
            If _patientCode.Trim < pPatientCode Then Return -1
            If _patientCode.Trim > pPatientCode Then Return 1
            Return 0
        End Function

#End Region 'Business Properties and Methods

#Region "Validation Rules"

        Private Sub AddSharedCommonRules()
            'Sample simple custom rule
            'ValidationRules.AddRule(AddressOf LDInfo.ContainsValidPeriod, "Period", 1)           

            'Sample dependent property. when check one , check the other as well
            'ValidationRules.AddDependantProperty("AccntCode", "AnalT0")
        End Sub

        Protected Overrides Sub AddBusinessRules()
            AddSharedCommonRules()

            For Each _field As ClassField In ClassSchema(Of PATIENT)._fieldList
                If _field.Required Then
                    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, _field.FieldName, 0)
                End If
                If Not String.IsNullOrEmpty(_field.RegexPattern) Then
                    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.RegExMatch, New RegExRuleArgs(_field.FieldName, _field.RegexPattern), 1)
                End If
                '----------using lookup, if no user lookup defined, fallback to predefined by developer----------------------------
                If CATMAPInfoList.ContainsCode(_field) Then
                    ValidationRules.AddRule(AddressOf LKUInfoList.ContainsLiveCode, _field.FieldName, 2)
                    'Else
                    '    Select Case _field.FieldName
                    '        Case "LocType"
                    '            ValidationRules.AddRule(Of LOC, AnalRuleArg)(AddressOf LOOKUPInfoList.ContainsSysCode, New AnalRuleArg(_field.FieldName, SysCats.LocationType))
                    '        Case "Status"
                    '            ValidationRules.AddRule(Of LOC, AnalRuleArg)(AddressOf LOOKUPInfoList.ContainsSysCode, New AnalRuleArg(_field.FieldName, SysCats.LocationStatus))
                    '    End Select
                End If
            Next
            Rules.BusinessRules.RegisterBusinessRules(Me)
            MyBase.AddBusinessRules()
        End Sub
#End Region ' Validation

#Region " Factory Methods "

        Private Sub New()
            _DTB = Context.CurrentBECode
        End Sub

        Public Shared Function BlankPATIENT() As PATIENT
            Return New PATIENT
        End Function

        Public Shared Function NewPATIENT(ByVal pPatientCode As String) As PATIENT
            If KeyDuplicated(pPatientCode) Then ExceptionThower.BusinessRuleStop(String.Format(ResStr(ResStrConst.NOACCESS), ResStr("PATIENT")))
            Return DataPortal.Create(Of PATIENT)(New Criteria(pPatientCode))
        End Function

        Public Shared Function NewBO(ByVal ID As String) As PATIENT
            Dim pPatientCode As String = ID.Trim

            Return NewPATIENT(pPatientCode)
        End Function

        Public Shared Function GetPATIENT(ByVal pPatientCode As String) As PATIENT
            Return DataPortal.Fetch(Of PATIENT)(New Criteria(pPatientCode))
        End Function

        Public Shared Function GetBO(ByVal ID As String) As PATIENT
            Dim pPatientCode As String = ID.Trim

            Return GetPATIENT(pPatientCode)
        End Function

        Public Shared Sub DeletePATIENT(ByVal pPatientCode As String)
            DataPortal.Delete(New Criteria(pPatientCode))
        End Sub

        Public Overrides Function Save() As PATIENT
            If Not IsDirty Then ExceptionThower.NotDirty(ResStr(ResStrConst.NOTDIRTY))
            If Not IsSavable Then Throw New Csla.Validation.ValidationException(String.Format(ResStr(ResStrConst.INVALID), ResStr("PATIENT")))

            Me.ApplyEdit()
            PATIENTInfoList.InvalidateCache()
            Return MyBase.Save()
        End Function

        Public Function ClonePATIENT(ByVal pPatientCode As String) As PATIENT

            If PATIENT.KeyDuplicated(pPatientCode) Then ExceptionThower.BusinessRuleStop(ResStr(ResStrConst.CreateAlreadyExists), Me.GetType.ToString.Leaf.Translate)

            Dim cloningPATIENT As PATIENT = MyBase.Clone
            cloningPATIENT._patientCode = pPatientCode

            'Todo:Remember to reset status of the new object here 
            cloningPATIENT.MarkNew()
            cloningPATIENT.ApplyEdit()

            cloningPATIENT.ValidationRules.CheckRules()

            Return cloningPATIENT
        End Function

#End Region ' Factory Methods

#Region " Data Access "

        <Serializable()> _
        Private Class Criteria
            Public _patientCode As String = String.Empty

            Public Sub New(ByVal pPatientCode As String)
                _patientCode = pPatientCode

            End Sub
        End Class

        <RunLocal()> _
        Private Overloads Sub DataPortal_Create(ByVal criteria As Criteria)
            _patientCode = criteria._patientCode

            ValidationRules.CheckRules()
        End Sub

        Private Overloads Sub DataPortal_Fetch(ByVal criteria As Criteria)
            Using ctx = ConnectionFactory.GetDBConnection(True)
                Using cm = ctx.CreateSQLCommand

                    cm.CommandText = <SqlText>SELECT * FROM pbs_MC_PATIENT_<%= _DTB %> WHERE PATIENT_CODE= '<%= criteria._patientCode %>' </SqlText>.Value.Trim

                    Using dr As New SafeDataReader(cm.ExecuteReader)
                        If dr.Read Then
                            FetchObject(dr)
                            MarkOld()
                        End If
                    End Using

                End Using
            End Using
        End Sub

        Private Sub FetchObject(ByVal dr As SafeDataReader)
            _patientCode = dr.GetString("PATIENT_CODE").TrimEnd
            _firstName = dr.GetString("FIRST_NAME").TrimEnd
            _middleName = dr.GetString("MIDDLE_NAME").TrimEnd
            _lastName = dr.GetString("LAST_NAME").TrimEnd
            _fullname = dr.GetString("FULLNAME").TrimEnd
            _gender = dr.GetString("GENDER").TrimEnd
            _dob.Text = dr.GetInt32("DOB")
            _job = dr.GetString("JOB").TrimEnd
            _ethnic = dr.GetString("ETHNIC").TrimEnd
            _nationality = dr.GetString("NATIONALITY").TrimEnd
            _address = dr.GetString("ADDRESS").TrimEnd
            _ward = dr.GetString("WARD").TrimEnd
            _district = dr.GetString("DISTRICT").TrimEnd
            _city = dr.GetString("CITY").TrimEnd
            _workplace = dr.GetString("WORKPLACE").TrimEnd
            _phone = dr.GetString("PHONE").TrimEnd
            _email = dr.GetString("EMAIL").TrimEnd
            _emergencyContact = dr.GetString("EMERGENCY_CONTACT").TrimEnd
            _emergencyContactName = dr.GetString("EMERGENCY_CONTACT_NAME").TrimEnd
            _emergencyContactPhone = dr.GetString("EMERGENCY_CONTACT_PHONE").TrimEnd
            _patientType = dr.GetString("PATIENT_TYPE").TrimEnd
            _healthInsuranceNo = dr.GetString("HEALTH_INSURANCE_NO").TrimEnd
            _validTo.Text = dr.GetInt32("VALID_TO")
            _notes = dr.GetString("NOTES").TrimEnd
            _ncPt9 = dr.GetString("NC_Pt9").TrimEnd
            _ncPt8 = dr.GetString("NC_Pt8").TrimEnd
            _ncPt7 = dr.GetString("NC_Pt7").TrimEnd
            _ncPt6 = dr.GetString("NC_Pt6").TrimEnd
            _ncPt6 = dr.GetString("NC_Pt5").TrimEnd
            _ncPt4 = dr.GetString("NC_Pt4").TrimEnd
            _ncPt3 = dr.GetString("NC_Pt3").TrimEnd
            _ncPt2 = dr.GetString("NC_Pt2").TrimEnd
            _ncPt1 = dr.GetString("NC_Pt1").TrimEnd
            _updated.Text = dr.GetInt32("UPDATED")
            _updatedBy = dr.GetString("UPDATED_BY").TrimEnd

        End Sub

        Private Shared _lockObj As New Object
        Protected Overrides Sub DataPortal_Insert()
            SyncLock _lockObj
                Using ctx = ConnectionFactory.GetDBConnection(True)
                    Using cm = ctx.CreateSQLCommand

                        cm.CommandType = CommandType.StoredProcedure
                        cm.CommandText = String.Format("pbs_MC_PATIENT_{0}_InsertUpdate", _DTB)

                        AddInsertParameters(cm)
                        cm.ExecuteNonQuery()

                    End Using
                End Using
            End SyncLock
        End Sub

        Private Sub AddInsertParameters(ByVal cm As IDBCommand)
            cm.Parameters.AddWithValue("@PATIENT_CODE", _patientCode.Trim)
            cm.Parameters.AddWithValue("@FIRST_NAME", _firstName.Trim)
            cm.Parameters.AddWithValue("@MIDDLE_NAME", _middleName.Trim)
            cm.Parameters.AddWithValue("@LAST_NAME", _lastName.Trim)
            cm.Parameters.AddWithValue("@FULLNAME", _fullname.Trim)
            cm.Parameters.AddWithValue("@GENDER", _gender.Trim)
            cm.Parameters.AddWithValue("@DOB", _dob.DBValue)
            cm.Parameters.AddWithValue("@JOB", _job.Trim)
            cm.Parameters.AddWithValue("@ETHNIC", _ethnic.Trim)
            cm.Parameters.AddWithValue("@NATIONALITY", _nationality.Trim)
            cm.Parameters.AddWithValue("@ADDRESS", _address.Trim)
            cm.Parameters.AddWithValue("@WARD", _ward.Trim)
            cm.Parameters.AddWithValue("@DISTRICT", _district.Trim)
            cm.Parameters.AddWithValue("@CITY", _city.Trim)
            cm.Parameters.AddWithValue("@WORKPLACE", _workplace.Trim)
            cm.Parameters.AddWithValue("@PHONE", _phone.Trim)
            cm.Parameters.AddWithValue("@EMAIL", _email.Trim)
            cm.Parameters.AddWithValue("@EMERGENCY_CONTACT", _emergencyContact.Trim)
            cm.Parameters.AddWithValue("@EMERGENCY_CONTACT_NAME", _emergencyContactName.Trim)
            cm.Parameters.AddWithValue("@EMERGENCY_CONTACT_PHONE", _emergencyContactPhone.Trim)
            cm.Parameters.AddWithValue("@PATIENT_TYPE", _patientType.Trim)
            cm.Parameters.AddWithValue("@HEALTH_INSURANCE_NO", _healthInsuranceNo.Trim)
            cm.Parameters.AddWithValue("@VALID_TO", _validTo.DBValue)
            cm.Parameters.AddWithValue("@NOTES", _notes.Trim)
            cm.Parameters.AddWithValue("@NC_PT9", _ncPt9.Trim)
            cm.Parameters.AddWithValue("@NC_PT8", _ncPt8.Trim)
            cm.Parameters.AddWithValue("@NC_PT7", _ncPt7.Trim)
            cm.Parameters.AddWithValue("@NC_PT6", _ncPt6.Trim)
            cm.Parameters.AddWithValue("@NC_PT5", _ncPt5.Trim)
            cm.Parameters.AddWithValue("@NC_PT4", _ncPt4.Trim)
            cm.Parameters.AddWithValue("@NC_PT3", _ncPt3.Trim)
            cm.Parameters.AddWithValue("@NC_PT2", _ncPt2.Trim)
            cm.Parameters.AddWithValue("@NC_PT1", _ncPt1.Trim)
            cm.Parameters.AddWithValue("@UPDATED", ToDay.ToSunDate)
            cm.Parameters.AddWithValue("@UPDATED_BY", Context.CurrentUserCode)
        End Sub


        Protected Overrides Sub DataPortal_Update()
            DataPortal_Insert()
        End Sub

        Protected Overrides Sub DataPortal_DeleteSelf()
            DataPortal_Delete(New Criteria(_patientCode))
        End Sub

        Private Overloads Sub DataPortal_Delete(ByVal criteria As Criteria)
        Using ctx = ConnectionFactory.GetDBConnection(True)
                Using cm = ctx.CreateSQLCommand

                    cm.CommandText = <SqlText>DELETE FROM pbs_MC_PATIENT_<%= _DTB %> WHERE PATIENT_CODE= '<%= criteria._patientCode %>' </SqlText>.Value.Trim
                    cm.ExecuteNonQuery()

                End Using
            End Using

        End Sub

        'Protected Overrides Sub DataPortal_OnDataPortalInvokeComplete(ByVal e As Csla.DataPortalEventArgs)
        '    If Csla.ApplicationContext.ExecutionLocation = ExecutionLocations.Server Then
        '        PATIENTInfoList.InvalidateCache()
        '    End If
        'End Sub


#End Region 'Data Access                           

#Region " Exists "
        Public Shared Function Exists(ByVal pPatientCode As String) As Boolean
            Return PATIENTInfoList.ContainsCode(pPatientCode)
        End Function

        Public Shared Function KeyDuplicated(ByVal pPatientCode As String) As Boolean
            Dim SqlText = <SqlText>SELECT COUNT(*) FROM pbs_MC_PATIENT_<%= Context.CurrentBECode %> WHERE PATIENT_CODE= '<%= pPatientCode %>'</SqlText>.Value.Trim
            Return SQLCommander.GetScalarInteger(SqlText) > 0
        End Function
#End Region

#Region " IGenpart "

        Public Function CloneBO(ByVal id As String) As Object Implements Interfaces.IGenPartObject.CloneBO
            Return ClonePATIENT(id)
        End Function

        Public Function getBO1(ByVal id As String) As Object Implements Interfaces.IGenPartObject.GetBO
            Return GetBO(id)
        End Function

        Public Function myCommands() As String() Implements Interfaces.IGenPartObject.myCommands
            Return pbs.Helper.Action.StandardReferenceCommands
        End Function

        Public Function myFullName() As String Implements Interfaces.IGenPartObject.myFullName
            Return GetType(PATIENT).ToString
        End Function

        Public Function myName() As String Implements Interfaces.IGenPartObject.myName
            Return GetType(PATIENT).ToString.Leaf
        End Function

        Public Function myQueryList() As IList Implements Interfaces.IGenPartObject.myQueryList
            Return PATIENTInfoList.GetPATIENTInfoList
        End Function
#End Region

#Region "IDoclink"
        Public Function Get_DOL_Reference() As String Implements IDocLink.Get_DOL_Reference
            Return String.Format("{0}#{1}", Get_TransType, _patientCode)
        End Function

        Public Function Get_TransType() As String Implements IDocLink.Get_TransType
            Return Me.GetType.ToClassSchemaName.Leaf
        End Function
#End Region

    End Class

End Namespace