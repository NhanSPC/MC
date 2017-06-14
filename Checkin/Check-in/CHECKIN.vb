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
    <DB(TableName:="pbs_MC_CHECKIN_{XXX}")>
    <PhoebusCommand(Desc:="Patient Check-In")>
    Public Class CHECKIN
        Inherits Csla.BusinessBase(Of CHECKIN)
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

                Case "DiagnosisCode"
                    For Each itm In LOOKUPInfoList.GetLOOKUPInfoList_ByCategory("ICD-10", True)
                        If DiagnosisCode = itm.Code Then
                            Diagnosis = itm.Descriptn1

                        End If
                    Next

                Case "AppointmentNo"
                    Dim appoint = APPOINTMENTInfoList.GetAPPOINTMENTInfo(AppointmentNo)
                    PatientCode = appoint.PatientCode

                    'If String.IsNullOrEmpty(appoint.PatientCode) Then
                    '    Dim UIcmd = New pbsCmdArgs(String.Format("pbs.BO.MC.APPOINTMENT?LineNo={0}&$action=amend", AppointmentNo))
                    '    pbs.Helper.UIServices.RunURLService.Run(UIcmd)
                    '    PatientCode = appoint.PatientCode
                    'Else

                    'End If

                Case "PatientCode"
                    GetInsuranceInfo(PatientCode)

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


        Private _lineNo As String = String.Empty
        <System.ComponentModel.DataObjectField(True, True)>
        Public ReadOnly Property LineNo() As String
            Get
                Return _lineNo
            End Get
        End Property

        Private _appointmentNo As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        <CellInfo("pbs.BO.MC.APPOINTMENT", GroupName:="General Info", Tips:="Enter appointment number")>
        Public Property AppointmentNo() As String
            Get
                Return _appointmentNo.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("AppointmentNo", True)
                If value Is Nothing Then value = String.Empty
                If Not _appointmentNo.Equals(value) Then
                    _appointmentNo.Text = value
                    PropertyHasChanged("AppointmentNo")
                End If
            End Set
        End Property

        Private _referenceNo As String = String.Empty
        <CellInfo(GroupName:="General Info", Tips:="Enter number of stored files")>
        Public ReadOnly Property ReferenceNo() As String
            Get
                Return _referenceNo
            End Get
            'Set(ByVal value As String)
            '    CanWriteProperty("ReferenceNo", True)
            '    If value Is Nothing Then value = String.Empty
            '    If Not _referenceNo.Equals(value) Then
            '        _referenceNo = value
            '        PropertyHasChanged("ReferenceNo")
            '    End If
            'End Set
        End Property

        Private _medicalRecordCode As String = String.Empty
        <CellInfo(GroupName:="General Info", Tips:="Enter national medical code. First 3 characters is city code. The next 3 characters is hopital code. The next 2 is year code. Last 6 characters is number of patient's stored file.")>
        Public ReadOnly Property MedicalRecordCode() As String
            Get
                Return _medicalRecordCode
            End Get
            'Set(ByVal value As String)
            '    CanWriteProperty("MedicalRecordCode", True)
            '    If value Is Nothing Then value = String.Empty
            '    If Not _medicalRecordCode.Equals(value) Then
            '        _medicalRecordCode = value
            '        PropertyHasChanged("MedicalRecordCode")
            '    End If
            'End Set
        End Property

        Private _patientCode As String = String.Empty
        <CellInfo("pbs.BO.MC.PATIENT", GroupName:="General Info", Tips:="Enter patient code")>
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

        Private _checkinDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate(0)
        <CellInfo("CALENDAR", GroupName:="General Info", Tips:="Enter check-in code")>
        <Rule(Required:=True)>
        Public Property CheckinDate() As String
            Get
                Return _checkinDate.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("CheckinDate", True)
                If value Is Nothing Then value = String.Empty
                If Not _checkinDate.Equals(value) Then
                    _checkinDate.Text = value
                    PropertyHasChanged("CheckinDate")
                End If
            End Set
        End Property

        Private _checkinTime As pbs.Helper.SmartTime = New pbs.Helper.SmartTime()
        <CellInfo("HOUR", GroupName:="Check-in Info", Tips:="Enter check-in time")>
        Public Property CheckinTime() As String
            Get
                Return _checkinTime.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("CheckinTime", True)
                If value Is Nothing Then value = String.Empty
                If Not _checkinTime.Equals(value) Then
                    _checkinTime.Text = value
                    PropertyHasChanged("CheckinTime")
                End If
            End Set
        End Property

        Private _checkinType As String = String.Empty
        <CellInfo("DIRECT_CIN", GroupName:="Check-in Info", Tips:="Enter check-in type")>
        <Rule(Required:=True)>
        Public Property CheckinType() As String
            Get
                Return _checkinType
            End Get
            Set(ByVal value As String)
                CanWriteProperty("CheckinType", True)
                If value Is Nothing Then value = String.Empty
                If Not _checkinType.Equals(value) Then
                    _checkinType = value
                    PropertyHasChanged("CheckinType")
                End If
            End Set
        End Property

        Private _transferFrom As String = String.Empty
        <CellInfo("REFER_BY", GroupName:="Check-in Info", Tips:="Chose from list")>
        Public Property TransferFrom() As String
            Get
                Return _transferFrom
            End Get
            Set(ByVal value As String)
                CanWriteProperty("TransferFrom", True)
                If value Is Nothing Then value = String.Empty
                If Not _transferFrom.Equals(value) Then
                    _transferFrom = value
                    PropertyHasChanged("TransferFrom")
                End If
            End Set
        End Property

        Private _transferFromName As String = String.Empty
        <CellInfo(GroupName:="Check-in Info", Tips:="Enter name of place patient transfered from")>
        Public Property TransferFromName() As String
            Get
                Return _transferFromName
            End Get
            Set(ByVal value As String)
                CanWriteProperty("TransferFromName", True)
                If value Is Nothing Then value = String.Empty
                If Not _transferFromName.Equals(value) Then
                    _transferFromName = value
                    PropertyHasChanged("TransferFromName")
                End If
            End Set
        End Property

        Private _department As String = String.Empty
        <CellInfo("MEDI_DEPT", GroupName:="Check-in Info", Tips:="Enter name of department patient check-in into")>
        Public Property Department() As String
            Get
                Return _department
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Department", True)
                If value Is Nothing Then value = String.Empty
                If Not _department.Equals(value) Then
                    _department = value
                    PropertyHasChanged("Department")
                End If
            End Set
        End Property

        Private _deptCheckinTime As pbs.Helper.SmartTime = New pbs.Helper.SmartTime()
        <CellInfo("HOUR", GroupName:="Check-in Info", Tips:="Enter department check-in time")>
        Public Property DeptCheckinTime() As String
            Get
                Return _deptCheckinTime.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("DeptCheckinTime", True)
                If value Is Nothing Then value = String.Empty
                If Not _deptCheckinTime.Equals(value) Then
                    _deptCheckinTime.Text = value
                    PropertyHasChanged("DeptCheckinTime")
                End If
            End Set
        End Property

        Private _deptCheckinDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo("CALENDAR", GroupName:="Check-in Info", Tips:="Enter department checkin date")>
        Public Property DeptCheckinDate() As String
            Get
                Return _deptCheckinDate.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("DeptCheckinDate", True)
                If value Is Nothing Then value = String.Empty
                If Not _deptCheckinDate.Equals(value) Then
                    _deptCheckinDate.Text = value
                    PropertyHasChanged("DeptCheckinDate")
                End If
            End Set
        End Property

        Private _cabinetNo As String = String.Empty
        <CellInfo("MEDI_ROOM", GroupName:="Check-in Info", Tips:="Enter cabinet number")>
        Public Property CabinetNo() As String
            Get
                Return _cabinetNo
            End Get
            Set(ByVal value As String)
                CanWriteProperty("CabinetNo", True)
                If value Is Nothing Then value = String.Empty
                If Not _cabinetNo.Equals(value) Then
                    _cabinetNo = value
                    PropertyHasChanged("CabinetNo")
                End If
            End Set
        End Property

        Private _checkoutTime As pbs.Helper.SmartTime = New pbs.Helper.SmartTime()
        <CellInfo("HOUR", GroupName:="Check-out Info", Tips:="Enter check-out time")>
        Public Property CheckoutTime() As String
            Get
                Return _checkoutTime.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("CheckoutTime", True)
                If value Is Nothing Then value = String.Empty
                If Not _checkoutTime.Equals(value) Then
                    _checkoutTime.Text = value
                    PropertyHasChanged("CheckoutTime")
                End If
            End Set
        End Property

        Private _checkoutDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo("CALENDAR", GroupName:="Check-out Info", Tips:="Enter check-out date")>
        Public Property CheckoutDate() As String
            Get
                Return _checkoutDate.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("CheckoutDate", True)
                If value Is Nothing Then value = String.Empty
                If Not _checkoutDate.Equals(value) Then
                    _checkoutDate.Text = value
                    PropertyHasChanged("CheckoutDate")
                End If
            End Set
        End Property

        Private _checkoutType As String = String.Empty
        <CellInfo("COUT_TYPE", GroupName:="Check-out Info", Tips:="Enter check-out type")>
        Public Property CheckoutType() As String
            Get
                Return _checkoutType
            End Get
            Set(ByVal value As String)
                CanWriteProperty("CheckoutType", True)
                If value Is Nothing Then value = String.Empty
                If Not _checkoutType.Equals(value) Then
                    _checkoutType = value
                    PropertyHasChanged("CheckoutType")
                End If
            End Set
        End Property

        Private _treatmentResult As String = String.Empty
        <CellInfo(GroupName:="Result", Tips:="Enter patient's treatment result")>
        Public Property TreatmentResult() As String
            Get
                Return _treatmentResult
            End Get
            Set(ByVal value As String)
                CanWriteProperty("TreatmentResult", True)
                If value Is Nothing Then value = String.Empty
                If Not _treatmentResult.Equals(value) Then
                    _treatmentResult = value
                    PropertyHasChanged("TreatmentResult")
                End If
            End Set
        End Property

        Private _doctor As String = String.Empty
        <CellInfo("pbs.BO.HR.EMP", GroupName:="Advice", Tips:="Enter doctor treatment code")>
        Public Property Doctor() As String
            Get
                Return _doctor
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Doctor", True)
                If value Is Nothing Then value = String.Empty
                If Not _doctor.Equals(value) Then
                    _doctor = value
                    PropertyHasChanged("Doctor")
                End If
            End Set
        End Property

        Private _hospitalizedReason As String = String.Empty
        <CellInfo(GroupName:="Medical record", Tips:="Enter patient's hospitalize reason")>
        Public Property HospitalizedReason() As String
            Get
                Return _hospitalizedReason
            End Get
            Set(ByVal value As String)
                CanWriteProperty("HospitalizedReason", True)
                If value Is Nothing Then value = String.Empty
                If Not _hospitalizedReason.Equals(value) Then
                    _hospitalizedReason = value
                    PropertyHasChanged("HospitalizedReason")
                End If
            End Set
        End Property

        Private _dayOfDisease As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        <CellInfo(GroupName:="Medical record", Tips:="Enter day of disease")>
        Public Property DayOfDisease() As String
            Get
                Return _dayOfDisease.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("DayOfDisease", True)
                If value Is Nothing Then value = String.Empty
                If Not _dayOfDisease.Equals(value) Then
                    _dayOfDisease.Text = value
                    PropertyHasChanged("DayOfDisease")
                End If
            End Set
        End Property

        Private _symptomsCode As String = String.Empty
        <CellInfo("ICD-10", GroupName:="Medical record", Tips:="Enter symptoms code")>
        Public Property SymptomsCode() As String
            Get
                Return _symptomsCode
            End Get
            Set(ByVal value As String)
                CanWriteProperty("SymptomsCode", True)
                If value Is Nothing Then value = String.Empty
                If Not _symptomsCode.Equals(value) Then
                    _symptomsCode = value
                    PropertyHasChanged("SymptomsCode")
                End If
            End Set
        End Property

        Private _symptoms As String = String.Empty
        <CellInfo(GroupName:="Medical record", Tips:="Enter symptoms", ControlType:=Forms.CtrlType.MemoEdit)>
        Public Property Symptoms() As String
            Get
                Return _symptoms
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Symptoms", True)
                If value Is Nothing Then value = String.Empty
                If Not _symptoms.Equals(value) Then
                    _symptoms = value
                    PropertyHasChanged("Symptoms")
                End If
            End Set
        End Property

        Private _history As String = String.Empty
        <CellInfo(GroupName:="Medical record", Tips:="Enter patient's medical history", ControlType:=Forms.CtrlType.MemoEdit)>
        Public Property History() As String
            Get
                Return _history
            End Get
            Set(ByVal value As String)
                CanWriteProperty("History", True)
                If value Is Nothing Then value = String.Empty
                If Not _history.Equals(value) Then
                    _history = value
                    PropertyHasChanged("History")
                End If
            End Set
        End Property

        Private _allergic As String = String.Empty
        <CellInfo(GroupName:="Medical record", Tips:="Enter patient's allergic")>
        Public Property Allergic() As String
            Get
                Return _allergic
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Allergic", True)
                If value Is Nothing Then value = String.Empty
                If Not _allergic.Equals(value) Then
                    _allergic = value
                    PropertyHasChanged("Allergic")
                End If
            End Set
        End Property

        Private _allergens As String = String.Empty
        <CellInfo(GroupName:="Medical record", Tips:="Enter patient's allergens")>
        Public Property Allergens() As String
            Get
                Return _allergens
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Allergens", True)
                If value Is Nothing Then value = String.Empty
                If Not _allergens.Equals(value) Then
                    _allergens = value
                    PropertyHasChanged("Allergens")
                End If
            End Set
        End Property

        Private _family As String = String.Empty
        <CellInfo(GroupName:="Medical record", Tips:="Enter patient's family medical history", ControlType:=Forms.CtrlType.MemoEdit)>
        Public Property Family() As String
            Get
                Return _family
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Family", True)
                If value Is Nothing Then value = String.Empty
                If Not _family.Equals(value) Then
                    _family = value
                    PropertyHasChanged("Family")
                End If
            End Set
        End Property

        Private _mainCause As String = String.Empty
        <CellInfo(GroupName:="Result", Tips:="Enter main cause of disease", ControlType:=Forms.CtrlType.MemoEdit)>
        Public Property MainCause() As String
            Get
                Return _mainCause
            End Get
            Set(ByVal value As String)
                CanWriteProperty("MainCause", True)
                If value Is Nothing Then value = String.Empty
                If Not _mainCause.Equals(value) Then
                    _mainCause = value
                    PropertyHasChanged("MainCause")
                End If
            End Set
        End Property

        Private _relateCause As String = String.Empty
        <CellInfo(GroupName:="Result", Tips:="Enter relate cause", ControlType:=Forms.CtrlType.MemoEdit)>
        Public Property RelateCause() As String
            Get
                Return _relateCause
            End Get
            Set(ByVal value As String)
                CanWriteProperty("RelateCause", True)
                If value Is Nothing Then value = String.Empty
                If Not _relateCause.Equals(value) Then
                    _relateCause = value
                    PropertyHasChanged("RelateCause")
                End If
            End Set
        End Property

        Private _distinction As String = String.Empty
        <CellInfo(GroupName:="Result", Tips:="Distinction")>
        Public Property Distinction() As String
            Get
                Return _distinction
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Distinction", True)
                If value Is Nothing Then value = String.Empty
                If Not _distinction.Equals(value) Then
                    _distinction = value
                    PropertyHasChanged("Distinction")
                End If
            End Set
        End Property

        Private _diagnosisCode As String = String.Empty
        <CellInfo("ICD-10", GroupName:="Result", Tips:="Enter diagnosis code")>
        Public Property DiagnosisCode() As String
            Get
                Return _diagnosisCode
            End Get
            Set(ByVal value As String)
                CanWriteProperty("DiagnosisCode", True)
                If value Is Nothing Then value = String.Empty
                If Not _diagnosisCode.Equals(value) Then
                    _diagnosisCode = value
                    PropertyHasChanged("DiagnosisCode")
                End If
            End Set
        End Property

        Private _diagnosis As String = String.Empty
        <CellInfo("ICD-10", GroupName:="Result", Tips:="Enter diagnosis", ControlType:=Forms.CtrlType.MemoEdit)>
        Public Property Diagnosis() As String
            Get
                Return _diagnosis
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Diagnosis", True)
                If value Is Nothing Then value = String.Empty
                If Not _diagnosis.Equals(value) Then
                    _diagnosis = value
                    PropertyHasChanged("Diagnosis")
                End If
            End Set
        End Property

        Private _treatment As String = String.Empty
        <CellInfo(GroupName:="Result", Tips:="Enter treatment for disease", ControlType:=Forms.CtrlType.MemoEdit)>
        Public Property Treatment() As String
            Get
                Return _treatment
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Treatment", True)
                If value Is Nothing Then value = String.Empty
                If Not _treatment.Equals(value) Then
                    _treatment = value
                    PropertyHasChanged("Treatment")
                End If
            End Set
        End Property

        Private _doctorAdvice As String = String.Empty
        <CellInfo(GroupName:="Advice", Tips:="Enter doctor's advice", ControlType:=Forms.CtrlType.MemoEdit)>
        Public Property DoctorAdvice() As String
            Get
                Return _doctorAdvice
            End Get
            Set(ByVal value As String)
                CanWriteProperty("DoctorAdvice", True)
                If value Is Nothing Then value = String.Empty
                If Not _doctorAdvice.Equals(value) Then
                    _doctorAdvice = value
                    PropertyHasChanged("DoctorAdvice")
                End If
            End Set
        End Property

        Private _reexamDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo("CALENDAR", GroupName:="Advice", Tips:="Enter re-exam date")>
        Public Property ReexamDate() As String
            Get
                Return _reexamDate.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ReexamDate", True)
                If value Is Nothing Then value = String.Empty
                If Not _reexamDate.Equals(value) Then
                    _reexamDate.Text = value
                    PropertyHasChanged("ReexamDate")
                End If
            End Set
        End Property

        Private _dead As String = String.Empty
        <CellInfo(GroupName:="Result", Tips:="Check if patient is dead", ControlType:=Forms.CtrlType.ToggleSwitch)>
        Public Property Dead() As Boolean
            Get
                Return _dead.ToBoolean
            End Get
            Set(ByVal value As Boolean)
                CanWriteProperty("Dead", True)
                If Not _dead.Equals(value) Then
                    _dead = If(value, "Y", "N")
                    PropertyHasChanged("Dead")
                End If
            End Set
        End Property

        Private _surgery As String = String.Empty
        <CellInfo(GroupName:="Result", Tips:="Check if patient have surgery", ControlType:=Forms.CtrlType.ToggleSwitch)>
        Public Property Surgery() As Boolean
            Get
                Return _surgery.ToBoolean
            End Get
            Set(ByVal value As Boolean)
                CanWriteProperty("Surgery", True)
                If Not _surgery.Equals(value) Then
                    _surgery = If(value, "Y", "N")
                    PropertyHasChanged("Surgery")
                End If
            End Set
        End Property

        Private _notes As String = String.Empty
        <CellInfo(GroupName:="Advice", Tips:="Notes", ControlType:=Forms.CtrlType.MemoEdit)>
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

        Private _status As String = String.Empty
        <CellInfo(GroupName:="General Info", Tips:="Enter patient's fee status")>
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

        Private _ncCi9 As String = String.Empty
        Public Property NcCi9() As String
            Get
                Return _ncCi9
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcCi9", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncCi9.Equals(value) Then
                    _ncCi9 = value
                    PropertyHasChanged("NcCi9")
                End If
            End Set
        End Property

        Private _ncCi8 As String = String.Empty
        Public Property NcCi8() As String
            Get
                Return _ncCi8
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcCi8", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncCi8.Equals(value) Then
                    _ncCi8 = value
                    PropertyHasChanged("NcCi8")
                End If
            End Set
        End Property

        Private _ncCi7 As String = String.Empty
        Public Property NcCi7() As String
            Get
                Return _ncCi7
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcCi7", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncCi7.Equals(value) Then
                    _ncCi7 = value
                    PropertyHasChanged("NcCi7")
                End If
            End Set
        End Property

        Private _ncCi6 As String = String.Empty
        Public Property NcCi6() As String
            Get
                Return _ncCi6
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcCi6", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncCi6.Equals(value) Then
                    _ncCi6 = value
                    PropertyHasChanged("NcCi6")
                End If
            End Set
        End Property

        Private _ncCi5 As String = String.Empty
        Public Property NcCi5() As String
            Get
                Return _ncCi5
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcCi5", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncCi5.Equals(value) Then
                    _ncCi5 = value
                    PropertyHasChanged("NcCi5")
                End If
            End Set
        End Property

        Private _ncCi4 As String = String.Empty
        Public Property NcCi4() As String
            Get
                Return _ncCi4
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcCi4", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncCi4.Equals(value) Then
                    _ncCi4 = value
                    PropertyHasChanged("NcCi4")
                End If
            End Set
        End Property

        Private _ncCi3 As String = String.Empty
        Public Property NcCi3() As String
            Get
                Return _ncCi3
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcCi3", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncCi3.Equals(value) Then
                    _ncCi3 = value
                    PropertyHasChanged("NcCi3")
                End If
            End Set
        End Property

        Private _ncCi2 As String = String.Empty
        Public Property NcCi2() As String
            Get
                Return _ncCi2
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcCi2", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncCi2.Equals(value) Then
                    _ncCi2 = value
                    PropertyHasChanged("NcCi2")
                End If
            End Set
        End Property

        Private _ncCi1 As String = String.Empty
        Public Property NcCi1() As String
            Get
                Return _ncCi1
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcCi1", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncCi1.Equals(value) Then
                    _ncCi1 = value
                    PropertyHasChanged("NcCi1")
                End If
            End Set
        End Property

        Private _ncCi0 As String = String.Empty
        Public Property NcCi0() As String
            Get
                Return _ncCi0
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcCi0", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncCi0.Equals(value) Then
                    _ncCi0 = value
                    PropertyHasChanged("NcCi0")
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

        Private _pulse As String
        <CellInfo(GroupName:="General check")>
        Public ReadOnly Property Pulse() As String
            Get
                Return _pulse
            End Get
        End Property

        Private _temperature As String
        <CellInfo(GroupName:="General check")>
        Public ReadOnly Property Temperature() As String
            Get
                Return _temperature
            End Get
        End Property

        Private _bloodPressure As String
        <CellInfo(GroupName:="General check")>
        Public ReadOnly Property BloodPressure() As String
            Get
                Return _bloodPressure
            End Get
        End Property

        Private _breathingRate As String
        <CellInfo(GroupName:="General check")>
        Public ReadOnly Property BreathingRate() As String
            Get
                Return _breathingRate
            End Get
        End Property

        Private _weight As String
        <CellInfo(GroupName:="General check")>
        Public ReadOnly Property Weight() As String
            Get
                Return _weight
            End Get
        End Property

        Private _height As String
        <CellInfo(GroupName:="General check")>
        Public ReadOnly Property Height() As String
            Get
                Return _height
            End Get
        End Property

        Private _bloodgroup As String = String.Empty
        <CellInfo(GroupName:="General check")>
        Public ReadOnly Property Bloodgroup() As String
            Get
                Return _bloodgroup
            End Get
        End Property

        Private _patientType As String = String.Empty
        <CellInfo(GroupName:="Insurance Info")>
        Public ReadOnly Property PatientType() As String
            Get
                Return _patientType
            End Get
        End Property

        Private _healthInsuranceNo As String = String.Empty
        <CellInfo(GroupName:="Insurance Info")>
        Public ReadOnly Property HealthInsuranceNo() As String
            Get
                Return _healthInsuranceNo
            End Get
        End Property

        Private _validTo As pbs.Helper.SmartDate = New pbs.Helper.SmartDate(0)
        <CellInfo(GroupName:="Insurance Info")>
        Public ReadOnly Property ValidTo() As String
            Get
                Return _validTo.Text
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

            'Sample dependent property. when check one , check the other as well
            'ValidationRules.AddDependantProperty("AccntCode", "AnalT0")
        End Sub

        Protected Overrides Sub AddBusinessRules()
            AddSharedCommonRules()

            For Each _field As ClassField In ClassSchema(Of CHECKIN)._fieldList
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
            _checkinDate.Text = ToDay.ToSunDate
            _checkinTime.Text = Now.ToTimeString
            _deptCheckinDate.Text = ToDay.ToSunDate
            _deptCheckinTime.Text = Now.ToTimeString
            _checkinType = "EXAM"
        End Sub

        Public Shared Function BlankCHECKIN() As CHECKIN
            Return New CHECKIN
        End Function

        Public Shared Function NewCHECKIN(ByVal pLineNo As String) As CHECKIN
            If KeyDuplicated(pLineNo) Then ExceptionThower.BusinessRuleStop(String.Format(ResStr(ResStrConst.NOACCESS), ResStr("CHECKIN")))
            Return DataPortal.Create(Of CHECKIN)(New Criteria(pLineNo))
        End Function

        Public Shared Function NewBO(ByVal ID As String) As CHECKIN
            Dim pLineNo As String = ID.Trim

            Return NewCHECKIN(pLineNo)
        End Function

        Public Shared Function GetCHECKIN(ByVal pLineNo As String) As CHECKIN
            Return DataPortal.Fetch(Of CHECKIN)(New Criteria(pLineNo))
        End Function

        Public Shared Function GetBO(ByVal ID As String) As CHECKIN
            Dim pLineNo As String = ID.Trim

            Return GetCHECKIN(pLineNo)
        End Function

        Public Shared Sub DeleteCHECKIN(ByVal pLineNo As String)
            DataPortal.Delete(New Criteria(pLineNo))
        End Sub

        Public Overrides Function Save() As CHECKIN
            If Not IsDirty Then ExceptionThower.NotDirty(ResStr(ResStrConst.NOTDIRTY))
            If Not IsSavable Then Throw New Csla.Validation.ValidationException(String.Format(ResStr(ResStrConst.INVALID), ResStr("CHECKIN")))

            Me.ApplyEdit()
            CHECKINInfoList.InvalidateCache()
            Return MyBase.Save()
        End Function

        Public Function CloneCHECKIN(ByVal pLineNo As String) As CHECKIN

            If CHECKIN.KeyDuplicated(pLineNo) Then ExceptionThower.BusinessRuleStop(ResStr(ResStrConst.CreateAlreadyExists), Me.GetType.ToString.Leaf.Translate)

            Dim cloningCHECKIN As CHECKIN = MyBase.Clone
            cloningCHECKIN._lineNo = pLineNo

            'Todo:Remember to reset status of the new object here 
            cloningCHECKIN.MarkNew()
            cloningCHECKIN.ApplyEdit()

            cloningCHECKIN.ValidationRules.CheckRules()

            Return cloningCHECKIN
        End Function

#End Region ' Factory Methods

#Region " Data Access "

        <Serializable()> _
        Private Class Criteria
            Public _lineNo As Integer

            Public Sub New(ByVal pLineNo As String)
                _lineNo = pLineNo.ToInteger

            End Sub
        End Class

        <RunLocal()> _
        Private Overloads Sub DataPortal_Create(ByVal criteria As Criteria)
            _lineNo = criteria._lineNo

            ValidationRules.CheckRules()
        End Sub

        Private Overloads Sub DataPortal_Fetch(ByVal criteria As Criteria)
            Using ctx = ConnectionFactory.GetDBConnection(True)
                Using cm = ctx.CreateSQLCommand

                    cm.CommandText = <SqlText>SELECT * FROM pbs_MC_CHECKIN_<%= _DTB %> WHERE LINE_NO= '<%= criteria._lineNo %>' </SqlText>.Value.Trim

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

            GetGencheck(LineNo)
            GetInsuranceInfo(PatientCode)

        End Sub

        'This sub return Gencheck information by Check-in LineNo
        Private Sub GetGencheck(ByVal lineno As Integer)
            For Each itm In GENCHECKInfoList.GetGENCHECKInfoList
                If itm.CheckinNo = lineno Then
                    _pulse = itm.Pulse
                    _temperature = itm.Temperature
                    _bloodPressure = itm.BloodPressure
                    _breathingRate = itm.BreathingRate
                    _weight = itm.Weight
                    _height = itm.Height
                    _bloodgroup = itm.Bloodgroup

                End If
            Next
        End Sub

        'return patient's insurance information
        Private Sub GetInsuranceInfo(ByVal pPatientCode As String)
            For Each itm In PATIENTInfoList.GetPATIENTInfoList
                If PatientCode = itm.Code Then
                    _patientType = itm.PatientType
                    _healthInsuranceNo = itm.HealthInsuranceNo
                    _validTo.Text = itm.ValidTo
                End If
            Next

        End Sub

        Private Shared _lockObj As New Object
        Protected Overrides Sub DataPortal_Insert()
            SyncLock _lockObj
                Using ctx = ConnectionFactory.GetDBConnection(True)
                    Using cm = ctx.CreateSQLCommand

                        cm.CommandType = CommandType.StoredProcedure
                        cm.CommandText = String.Format("pbs_MC_CHECKIN_{0}_Insert", _DTB)

                        cm.Parameters.AddWithValue("@LINE_NO", _lineNo.Trim.ToInteger, ParameterDirection.Output)
                        AddInsertParameters(cm)
                        cm.ExecuteNonQuery()

                        _lineNo = cm.GetReturnIdAsInteger("@LINE_NO")
                    End Using
                End Using
            End SyncLock
        End Sub

        Private Sub AddInsertParameters(ByVal cm As IDBCommand)

            cm.Parameters.AddWithValue("@APPOINTMENT_NO", _appointmentNo.DBValue)
            cm.Parameters.AddWithValue("@REFERENCE_NO", _referenceNo.Trim)
            'cm.Parameters.AddWithValue("@MEDICAL_RECORD_CODE", _medicalRecordCode.Trim)
            cm.Parameters.AddWithValue("@MEDICAL_RECORD_CODE", String.Format("179MBV{0}{1}", Right(ToDay.Year.ToString, 2), ReferenceNo))
            cm.Parameters.AddWithValue("@PATIENT_CODE", _patientCode.Trim)
            cm.Parameters.AddWithValue("@CHECKIN_DATE", _checkinDate.DBValue)
            cm.Parameters.AddWithValue("@CHECKIN_TIME", _checkinTime.DBValue)
            cm.Parameters.AddWithValue("@CHECKIN_TYPE", _checkinType.Trim)
            cm.Parameters.AddWithValue("@TRANSFER_FROM", _transferFrom.Trim)
            cm.Parameters.AddWithValue("@TRANSFER_FROM_NAME", _transferFromName.Trim)
            cm.Parameters.AddWithValue("@DEPARTMENT", _department.Trim)
            cm.Parameters.AddWithValue("@DEPT_CHECKIN_TIME", _deptCheckinTime.DBValue)
            cm.Parameters.AddWithValue("@DEPT_CHECKIN_DATE", _deptCheckinDate.DBValue)
            cm.Parameters.AddWithValue("@CABINET_NO", _cabinetNo.Trim)
            cm.Parameters.AddWithValue("@CHECKOUT_TIME", _checkoutTime.DBValue)
            cm.Parameters.AddWithValue("@CHECKOUT_DATE", _checkoutDate.DBValue)
            cm.Parameters.AddWithValue("@CHECKOUT_TYPE", _checkoutType.Trim)
            cm.Parameters.AddWithValue("@TREATMENT_RESULT", _treatmentResult.Trim)
            cm.Parameters.AddWithValue("@DOCTOR", _doctor.Trim)
            cm.Parameters.AddWithValue("@HOSPITALIZED_REASON", _hospitalizedReason.Trim)
            cm.Parameters.AddWithValue("@DAY_OF_DISEASE", _dayOfDisease.DBValue)
            cm.Parameters.AddWithValue("@SYMPTOMS_CODE", _symptomsCode.Trim)
            cm.Parameters.AddWithValue("@SYMPTOMS", _symptoms.Trim)
            cm.Parameters.AddWithValue("@HISTORY", _history.Trim)
            cm.Parameters.AddWithValue("@ALLERGIC", _allergic.Trim)
            cm.Parameters.AddWithValue("@ALLERGENS", _allergens.Trim)
            cm.Parameters.AddWithValue("@FAMILY", _family.Trim)
            cm.Parameters.AddWithValue("@MAIN_CAUSE", _mainCause.Trim)
            cm.Parameters.AddWithValue("@RELATE_CAUSE", _relateCause.Trim)
            cm.Parameters.AddWithValue("@DISTINCTION", _distinction.Trim)
            cm.Parameters.AddWithValue("@DIAGNOSIS_CODE", _diagnosisCode.Trim)
            cm.Parameters.AddWithValue("@DIAGNOSIS", _diagnosis.Trim)
            cm.Parameters.AddWithValue("@TREATMENT", _treatment.Trim)
            cm.Parameters.AddWithValue("@DOCTOR_ADVICE", _doctorAdvice.Trim)
            cm.Parameters.AddWithValue("@REEXAM_DATE", _reexamDate.DBValue)
            cm.Parameters.AddWithValue("@DEAD", _dead.Trim)
            cm.Parameters.AddWithValue("@SURGERY", _surgery.Trim)
            cm.Parameters.AddWithValue("@NOTES", _notes.Trim)
            cm.Parameters.AddWithValue("@STATUS", _status.Trim)
            cm.Parameters.AddWithValue("@NC_CI9", _ncCi9.Trim)
            cm.Parameters.AddWithValue("@NC_CI8", _ncCi8.Trim)
            cm.Parameters.AddWithValue("@NC_CI7", _ncCi7.Trim)
            cm.Parameters.AddWithValue("@NC_CI6", _ncCi6.Trim)
            cm.Parameters.AddWithValue("@NC_CI5", _ncCi5.Trim)
            cm.Parameters.AddWithValue("@NC_CI4", _ncCi4.Trim)
            cm.Parameters.AddWithValue("@NC_CI3", _ncCi3.Trim)
            cm.Parameters.AddWithValue("@NC_CI2", _ncCi2.Trim)
            cm.Parameters.AddWithValue("@NC_CI1", _ncCi1.Trim)
            cm.Parameters.AddWithValue("@NC_CI0", _ncCi0.Trim)
            cm.Parameters.AddWithValue("@UPDATED", ToDay.ToSunDate)
            cm.Parameters.AddWithValue("@UPDATED_BY", Context.CurrentUserCode)
        End Sub


        Protected Overrides Sub DataPortal_Update()
            SyncLock _lockObj
                Using ctx = ConnectionFactory.GetDBConnection(True)
                    Using cm = ctx.CreateSQLCommand

                        cm.CommandType = CommandType.StoredProcedure
                        cm.CommandText = String.Format("pbs_MC_CHECKIN_{0}_Update", _DTB)

                        cm.Parameters.AddWithValue("@LINE_NO", _lineNo.Trim)
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
                    cm.CommandText = <SqlText>DELETE FROM pbs_MC_CHECKIN_<%= _DTB %> WHERE LINE_NO= '<%= criteria._lineNo %>' </SqlText>.Value.Trim
                    cm.ExecuteNonQuery()

                End Using
            End Using

        End Sub

        Protected Overrides Sub DataPortal_OnDataPortalInvokeComplete(ByVal e As Csla.DataPortalEventArgs)
            If Csla.ApplicationContext.ExecutionLocation = ExecutionLocations.Server Then
                CHECKINInfoList.InvalidateCache()
            End If
        End Sub


#End Region 'Data Access                           

#Region " Exists "
        Public Shared Function Exists(ByVal pLineNo As String) As Boolean
            Return CHECKINInfoList.ContainsCode(pLineNo)
        End Function

        Public Shared Function KeyDuplicated(ByVal pLineNo As String) As Boolean
            Dim SqlText = <SqlText>SELECT COUNT(*) FROM pbs_MC_CHECKIN_<%= Context.CurrentBECode %> WHERE LINE_NO= '<%= pLineNo %>'</SqlText>.Value.Trim
            Return SQLCommander.GetScalarInteger(SqlText) > 0
        End Function
#End Region

#Region " IGenpart "

        Public Function CloneBO(ByVal id As String) As Object Implements Interfaces.IGenPartObject.CloneBO
            Return CloneCHECKIN(id)
        End Function

        Public Function getBO1(ByVal id As String) As Object Implements Interfaces.IGenPartObject.GetBO
            Return GetBO(id)
        End Function

        Public Function myCommands() As String() Implements Interfaces.IGenPartObject.myCommands
            Return pbs.Helper.Action.StandardReferenceCommands
        End Function

        Public Function myFullName() As String Implements Interfaces.IGenPartObject.myFullName
            Return GetType(CHECKIN).ToString
        End Function

        Public Function myName() As String Implements Interfaces.IGenPartObject.myName
            Return GetType(CHECKIN).ToString.Leaf
        End Function

        Public Function myQueryList() As IList Implements Interfaces.IGenPartObject.myQueryList
            Return CHECKINInfoList.GetCHECKINInfoList
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