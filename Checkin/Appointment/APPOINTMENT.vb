Imports pbs.Helper
Imports System.Data

Imports Csla
Imports Csla.Data
Imports Csla.Validation
Imports pbs.BO.DataAnnotations
Imports pbs.BO.Script
Imports pbs.BO.BusinessRules

Namespace MC

    <Serializable()>
    <DB(TableName:="pbs_MC_APPOINTMENT_XXX")>
    Public Class APPOINTMENT
        Inherits Csla.BusinessBase(Of APPOINTMENT)
        Implements Interfaces.IGenPartObject
        Implements IComparable
        Implements IDocLink

#Region "Property Changed"
        Protected Overrides Sub OnDeserialized(context As Runtime.Serialization.StreamingContext)
            MyBase.OnDeserialized(context)
            AddHandler Me.PropertyChanged, AddressOf BO_PropertyChanged
        End Sub

        Private Sub BO_PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Handles Me.PropertyChanged

            Select Case e.PropertyName
                Case "PatientCode"
                    Dim pt = PATIENTInfoList.GetPATIENTInfo(PatientCode)
                    Gender = pt.Gender
                    Phone = pt.Phone
                    Name = pt.Fullname
                    Dob = pt.Dob
                    Email = pt.Email
                    Address = pt.Address
                    Ward = pt.Ward
                    District = pt.District
                    City = pt.City

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


        Private _lineNo As Integer
        <System.ComponentModel.DataObjectField(True, True)>
        Public ReadOnly Property LineNo() As String
            Get
                Return _lineNo
            End Get
        End Property

        Private _clinicCode As String = String.Empty
        <CellInfo(GroupName:="", Tips:="")>
        Public Property ClinicCode() As String
            Get
                Return _clinicCode
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ClinicCode", True)
                If value Is Nothing Then value = String.Empty
                If Not _clinicCode.Equals(value) Then
                    _clinicCode = value
                    PropertyHasChanged("ClinicCode")
                End If
            End Set
        End Property

        Private _patientCode As String = String.Empty
        <CellInfo("pbs.BO.MC.PATIENT", GroupName:="General Info", Tips:="Enter patient code")>
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

        Private _name As String = String.Empty
        <CellInfo(GroupName:="Appointment Info", Tips:="Enter name of person make an appointment")>
        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Name", True)
                If value Is Nothing Then value = String.Empty
                If Not _name.Equals(value) Then
                    _name = value
                    PropertyHasChanged("Name")
                End If
            End Set
        End Property

        Private _dob As pbs.Helper.SmartDate = New pbs.Helper.SmartDate(0)
        <CellInfo(GroupName:="Appointment Info", Tips:="Enter birthday of person make an appointment")>
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

        Private _gender As String = String.Empty
        <CellInfo("GENDER", GroupName:="Appointment Info", Tips:="Enter gender.")>
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

        Private _phone As String = String.Empty
        <CellInfo(GroupName:="Contact", Tips:="Enter telephone number")>
        <Rule(Required:=True)>
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
        <CellInfo(GroupName:="Contact", Tips:="Enter email")>
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

        Private _address As String = String.Empty
        <CellInfo(GroupName:="Contact", Tips:="Enter address")>
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
        <CellInfo("pbs.BO.CRM.Ward?DistrictCode=[DISTRICT]", GroupName:="Contact", Tips:="Enter ward")>
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
        <CellInfo("pbs.BO.CRM.District?CityCode=[CITY]", GroupName:="Contact", Tips:="Enter district")>
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
        <CellInfo("CITY", GroupName:="Contact", Tips:="Enter city")>
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

        Private _status As String = String.Empty
        <CellInfo(GroupName:="Appointment Info", Tips:="Enter status of appointment")>
        Public Property Status() As String
            Get
                Return _status
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Status", True)
                If value Is Nothing Then value = String.Empty
                If Not _status.Equals(value) Then
                    _status = value
                    PropertyHasChanged("Status")
                End If
            End Set
        End Property

        Private _createDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo(LinkCode.Calendar, GroupName:="Appointment Info", Tips:="Enter create date")>
        Public Property CreateDate() As String
            Get
                Return _createDate.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("CreateDate", True)
                If value Is Nothing Then value = String.Empty
                If Not _createDate.Equals(value) Then
                    _createDate.Text = value
                    PropertyHasChanged("CreateDate")
                End If
            End Set
        End Property

        Private _appointmentDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo(LinkCode.Calendar, GroupName:="Appointment Info", Tips:="Enter appointment date")>
        <Rule(Required:=True)>
        Public Property AppointmentDate() As String
            Get
                Return _appointmentDate.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("AppointmentDate", True)
                If value Is Nothing Then value = String.Empty
                If Not _appointmentDate.Equals(value) Then
                    _appointmentDate.Text = value
                    PropertyHasChanged("AppointmentDate")
                End If
            End Set
        End Property

        Private _appointmentType As String = String.Empty
        <CellInfo(GroupName:="Appointment Info", Tips:="Enter appointment type")>
        Public Property AppointmentType() As String
            Get
                Return _appointmentType
            End Get
            Set(ByVal value As String)
                CanWriteProperty("AppointmentType", True)
                If value Is Nothing Then value = String.Empty
                If Not _appointmentType.Equals(value) Then
                    _appointmentType = value
                    PropertyHasChanged("AppointmentType")
                End If
            End Set
        End Property

        Private _reminderDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo(LinkCode.Calendar, GroupName:="Appointment Info", Tips:="Enter date to remind")>
        Public Property ReminderDate() As String
            Get
                Return _reminderDate.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ReminderDate", True)
                If value Is Nothing Then value = String.Empty
                If Not _reminderDate.Equals(value) Then
                    _reminderDate.Text = value
                    PropertyHasChanged("ReminderDate")
                End If
            End Set
        End Property

        Private _notes As String = String.Empty
        <CellInfo(GroupName:="Appointment Info", Tips:="Notes", ControlType:=Forms.CtrlType.MemoEdit)>
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

#End Region 'Business Properties and Methods

#Region "Validation Rules"

        Private Sub AddSharedCommonRules()
            'Sample simple custom rule
            'ValidationRules.AddRule(AddressOf LDInfo.ContainsValidPeriod, "Period", 1)           
            ValidationRules.AddRule(AddressOf CheckName, "Name", 1)
            ValidationRules.AddRule(AddressOf CheckName, "PatientCode", 1)

            'Sample dependent property. when check one , check the other as well
            'ValidationRules.AddDependantProperty("AccntCode", "AnalT0")

            ValidationRules.AddDependantProperty("Name", "PatientCode", True)

        End Sub

        Private Shared Function CheckName(target As Object, e As RuleArgs)
            Dim _appointment As APPOINTMENT = target
            If String.IsNullOrEmpty(_appointment._name) AndAlso String.IsNullOrEmpty(_appointment._patientCode) Then
                e.Description() = ResStr("You must enter Name or Patient code")
                Return False
            End If
            Return True
        End Function

        Protected Overrides Sub AddBusinessRules()
            AddSharedCommonRules()

            For Each _field As ClassField In ClassSchema(Of APPOINTMENT)._fieldList
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

        'Private Sub New()
        Friend Sub New()
            _DTB = Context.CurrentBECode
            _createDate.Text = ToDay.ToSunDate
        End Sub

        Public Shared Function BlankAPPOINTMENT() As APPOINTMENT
            Return New APPOINTMENT
        End Function

        Public Shared Function NewAPPOINTMENT(ByVal pLineNo As String) As APPOINTMENT
            Return DataPortal.Create(Of APPOINTMENT)(New Criteria(pLineNo))
        End Function

        Public Shared Function NewBO(ByVal ID As String) As APPOINTMENT
            Dim pLineNo As String = ID.Trim

            Return NewAPPOINTMENT(pLineNo)
        End Function

        Public Shared Function GetAPPOINTMENT(ByVal pLineNo As String) As APPOINTMENT
            Return DataPortal.Fetch(Of APPOINTMENT)(New Criteria(pLineNo))
        End Function

        Public Shared Function GetBO(ByVal ID As String) As APPOINTMENT
            Dim pLineNo As String = ID.Trim

            Return GetAPPOINTMENT(pLineNo)
        End Function

        Public Shared Sub DeleteAPPOINTMENT(ByVal pLineNo As String)
            DataPortal.Delete(New Criteria(pLineNo))
        End Sub

        Public Overrides Function Save() As APPOINTMENT
            If Not IsDirty Then ExceptionThower.NotDirty(ResStr(ResStrConst.NOTDIRTY))
            If Not IsSavable Then Throw New Csla.Validation.ValidationException(String.Format(ResStr(ResStrConst.INVALID), ResStr("APPOINTMENT")))

            Me.ApplyEdit()
            APPOINTMENTInfoList.InvalidateCache()
            Return MyBase.Save()
        End Function

        Public Function CloneAPPOINTMENT(ByVal pLineNo As String) As APPOINTMENT

            If APPOINTMENT.KeyDuplicated(pLineNo) Then ExceptionThower.BusinessRuleStop(ResStr(ResStrConst.CreateAlreadyExists), Me.GetType.ToString.Leaf.Translate)

            Dim cloningAPPOINTMENT As APPOINTMENT = MyBase.Clone
            cloningAPPOINTMENT._lineNo = pLineNo.ToInteger
            cloningAPPOINTMENT._DTB = Context.CurrentBECode
            'Todo:Remember to reset status of the new object here 
            cloningAPPOINTMENT.MarkNew()
            cloningAPPOINTMENT.ApplyEdit()

            cloningAPPOINTMENT.ValidationRules.CheckRules()

            Return cloningAPPOINTMENT
        End Function

#End Region ' Factory Methods

#Region " Data Access "

        <Serializable()>
        Private Class Criteria
            Public _lineNo As Integer

            Public Sub New(ByVal pLineNo As String)
                _lineNo = pLineNo.ToInteger

            End Sub
        End Class

        <RunLocal()>
        Private Overloads Sub DataPortal_Create(ByVal criteria As Criteria)
            _lineNo = criteria._lineNo

            ValidationRules.CheckRules()
        End Sub

        Private Overloads Sub DataPortal_Fetch(ByVal criteria As Criteria)
            Using ctx = ConnectionFactory.GetDBConnection(True)
                Using cm = ctx.CreateSQLCommand

                    cm.CommandText = <SqlText>SELECT * FROM pbs_MC_APPOINTMENT_<%= _DTB %> WHERE LINE_NO= <%= criteria._lineNo %></SqlText>.Value.Trim

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
            _lineNo = dr.GetInt32("LINE_NO")
            _clinicCode = dr.GetString("CLINIC_CODE").TrimEnd
            _patientCode = dr.GetString("PATIENT_CODE").TrimEnd
            _name = dr.GetString("NAME").TrimEnd
            _dob.Text = dr.GetInt32("DOB")
            _gender = dr.GetString("GENDER").TrimEnd
            _phone = dr.GetString("PHONE").TrimEnd
            _email = dr.GetString("EMAIL").TrimEnd
            _address = dr.GetString("ADDRESS").TrimEnd
            _ward = dr.GetString("WARD").TrimEnd
            _district = dr.GetString("DISTRICT").TrimEnd
            _city = dr.GetString("CITY").TrimEnd
            _status = dr.GetString("STATUS").TrimEnd
            _createDate.Text = dr.GetInt32("CREATE_DATE")
            _appointmentDate.Text = dr.GetInt32("APPOINTMENT_DATE")
            _appointmentType = dr.GetString("APPOINTMENT_TYPE").TrimEnd
            _reminderDate.Text = dr.GetInt32("REMINDER_DATE")
            _notes = dr.GetString("NOTES").TrimEnd
            _updated.Text = dr.GetInt32("UPDATED")
            _updatedBy = dr.GetString("UPDATED_BY").TrimEnd

        End Sub

        Private Shared _lockObj As New Object
        Protected Overrides Sub DataPortal_Insert()
            SyncLock _lockObj
                Using ctx = ConnectionFactory.GetDBConnection(True)
                    Using cm = ctx.CreateSQLCommand

                        cm.CommandType = CommandType.StoredProcedure
                        cm.CommandText = String.Format("pbs_MC_APPOINTMENT_{0}_Insert", _DTB)

                        cm.Parameters.AddWithValue("@LINE_NO", _lineNo, ParameterDirection.Output)
                        AddInsertParameters(cm)
                        cm.ExecuteNonQuery()

                        _lineNo = cm.GetReturnIdAsInteger("@LINE_NO")
                    End Using
                End Using
            End SyncLock
        End Sub

        Private Sub AddInsertParameters(ByVal cm As IDBCommand)

            cm.Parameters.AddWithValue("@CLINIC_CODE", _clinicCode.Trim)
            cm.Parameters.AddWithValue("@PATIENT_CODE", _patientCode.Trim)
            cm.Parameters.AddWithValue("@NAME", _name.Trim)
            cm.Parameters.AddWithValue("@DOB", _dob.DBValue)
            cm.Parameters.AddWithValue("@GENDER", _gender.Trim)
            cm.Parameters.AddWithValue("@PHONE", _phone.Trim)
            cm.Parameters.AddWithValue("@EMAIL", _email.Trim)
            cm.Parameters.AddWithValue("@ADDRESS", _address.Trim)
            cm.Parameters.AddWithValue("@WARD", _ward.Trim)
            cm.Parameters.AddWithValue("@DISTRICT", _district.Trim)
            cm.Parameters.AddWithValue("@CITY", _city.Trim)
            cm.Parameters.AddWithValue("@STATUS", _status.Trim)
            cm.Parameters.AddWithValue("@CREATE_DATE", _createDate.DBValue)
            cm.Parameters.AddWithValue("@APPOINTMENT_DATE", _appointmentDate.DBValue)
            cm.Parameters.AddWithValue("@APPOINTMENT_TYPE", _appointmentType.Trim)
            cm.Parameters.AddWithValue("@REMINDER_DATE", _reminderDate.DBValue)
            cm.Parameters.AddWithValue("@NOTES", _notes.Trim)
            cm.Parameters.AddWithValue("@UPDATED", _updated.DBValue)
            cm.Parameters.AddWithValue("@UPDATED_BY", _updatedBy.Trim)
        End Sub


        Protected Overrides Sub DataPortal_Update()
            SyncLock _lockObj
                Using ctx = ConnectionFactory.GetDBConnection(True)
                    Using cm = ctx.CreateSQLCommand

                        cm.CommandType = CommandType.StoredProcedure
                        cm.CommandText = String.Format("pbs_MC_APPOINTMENT_{0}_Update", _DTB)

                        cm.Parameters.AddWithValue("@LINE_NO", _lineNo)
                        AddInsertParameters(cm)
                        cm.ExecuteNonQuery()

                        _lineNo = cm.GetReturnIdAsInteger("@LINE_NO")
                    End Using
                End Using
            End SyncLock
        End Sub

        Protected Overrides Sub DataPortal_DeleteSelf()
            DataPortal_Delete(New Criteria(_lineNo))
        End Sub

        Private Overloads Sub DataPortal_Delete(ByVal criteria As Criteria)
            Using ctx = ConnectionFactory.GetDBConnection(True)
                Using cm = ctx.CreateSQLCommand

                    cm.CommandType = CommandType.Text
                    cm.CommandText = <SqlText>DELETE FROM pbs_MC_APPOINTMENT_<%= _DTB %> WHERE LINE_NO= <%= criteria._lineNo %></SqlText>.Value.Trim
                    cm.ExecuteNonQuery()

                End Using
            End Using

        End Sub



#End Region 'Data Access                           

#Region " Exists "
        Public Shared Function Exists(ByVal pLineNo As String) As Boolean
            Return KeyDuplicated(pLineNo)
        End Function

        Public Shared Function KeyDuplicated(ByVal pLineNo As String) As Boolean
            Dim theLine = pLineNo.ToInteger
            If theLine <= 0 Then Return False

            Dim SqlText = <SqlText>SELECT COUNT(*) FROM pbs_MC_APPOINTMENT_<%= Context.CurrentBECode %> WHERE LINE_NO= <%= theLine %></SqlText>.Value.Trim
            Return SQLCommander.GetScalarInteger(SqlText) > 0
        End Function
#End Region

#Region " IGenpart "

        Public Function CloneBO(ByVal id As String) As Object Implements Interfaces.IGenPartObject.CloneBO
            Return CloneAPPOINTMENT(id)
        End Function

        Public Function getBO1(ByVal id As String) As Object Implements Interfaces.IGenPartObject.GetBO
            Return GetBO(id)
        End Function

        Public Function myCommands() As String() Implements Interfaces.IGenPartObject.myCommands
            Return pbs.Helper.Action.StandardReferenceCommands
        End Function

        Public Function myFullName() As String Implements Interfaces.IGenPartObject.myFullName
            Return GetType(APPOINTMENT).ToString
        End Function

        Public Function myName() As String Implements Interfaces.IGenPartObject.myName
            Return GetType(APPOINTMENT).ToString.Leaf
        End Function

        Public Function myQueryList() As IList Implements Interfaces.IGenPartObject.myQueryList
            Return APPOINTMENTInfoList.GetAPPOINTMENTInfoList
        End Function
#End Region

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