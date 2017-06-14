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
    <DB(TableName:="pbs_MC_LAB_{XXX}")>
    Public Class LAB
        Inherits Csla.BusinessBase(Of LAB)
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

                Case "CheckinNo"
                    For Each ci In CHECKINInfoList.GetCHECKINInfoList
                        If CheckinNo = ci.LineNo Then
                            PatientCode = ci.PatientCode
                            DoctorAppointed = ci.Doctor
                        End If
                    Next

                Case "LabPackageCode"
                    For Each itm In LOOKUPInfoList.GetLOOKUPInfoList_ByCategory("LAB_SET", True)
                        If LabPackageCode = itm.Code Then
                            LabName = itm.Descriptn

                        End If
                    Next
                    PopulateDetails()
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


        Private Sub PopulateDetails()
            If String.IsNullOrEmpty(Me.LabPackageCode) Then Exit Sub

            If Me.Details.Count > 0 Then
                If pbs.Helper.UIServices.ConfirmService.Confirm("This lab already contains details. Update this list?") Then
                    Details.Clear()

                    For Each itm In LABTESTInfoList.GetLABTESTInfoList()
                        If itm.LabCode = LabPackageCode Then
                            Dim newLabDet = Details.AddNew

                            newLabDet.LabCode = itm.LabCode
                            newLabDet.TestCode = itm.Code
                            newLabDet.TestName = itm.Descriptn
                            newLabDet.Unit = itm.Unit
                            newLabDet.Value = String.Empty
                            newLabDet.MinValue = itm.MinValue
                            newLabDet.MaxValue = itm.MaxValue
                            newLabDet.MinValueFemale = itm.MinValueFemale
                            newLabDet.MaxValueFemale = itm.MaxValueFemale
                            newLabDet.MinValueMale = itm.MinValueMale
                            newLabDet.MaxValueMale = itm.MaxValueMale
                            newLabDet.Interpretation = String.Empty

                        End If

                        'newLabDet.LabId = itm.LookupAlt
                        'newLabDet.LabName = String.Format("{0}.{1}", itm.Code, itm.Descriptn)
                    Next

                End If
            Else

                For Each itm In LABTESTInfoList.GetLABTESTInfoList()
                    If itm.LabCode = LabPackageCode Then
                        Dim newLabDet = Details.AddNew

                        newLabDet.LabCode = itm.LabCode
                        newLabDet.TestCode = itm.TestCode
                        newLabDet.TestName = itm.Descriptn
                        newLabDet.Unit = itm.Unit
                        newLabDet.Value = String.Empty
                        newLabDet.MinValue = itm.MinValue
                        newLabDet.MaxValue = itm.MaxValue
                        newLabDet.MinValueFemale = itm.MinValueFemale
                        newLabDet.MaxValueFemale = itm.MaxValueFemale
                        newLabDet.MinValueMale = itm.MinValueMale
                        newLabDet.MaxValueMale = itm.MaxValueMale
                        newLabDet.Interpretation = String.Empty

                    End If
                Next

            End If


        End Sub

#Region " Business Properties and Methods "
        Friend _DTB As String = String.Empty


        Private _lineNo As String = String.Empty
        <System.ComponentModel.DataObjectField(True, True)>
        Public ReadOnly Property LineNo() As String
            Get
                Return _lineNo
            End Get
        End Property

        Private _patientCode As String = String.Empty
        <CellInfo("pbs.BO.MC.PATIENT", GroupName:="General Info", Tips:="Nhập mã bệnh nhân")>
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

        Private _checkinNo As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        <CellInfo("pbs.BO.MC.CHECKIN", GroupName:="General Info", Tips:="Nhập mã nhập viện")>
        <Rule(Required:=True)>
        Public Property CheckinNo() As String
            Get
                Return _checkinNo.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("CheckinNo", True)
                If value Is Nothing Then value = String.Empty
                If Not _checkinNo.Equals(value) Then
                    _checkinNo.Text = value
                    PropertyHasChanged("CheckinNo")
                End If
            End Set
        End Property

        Private _cabinetNo As String = String.Empty
        <CellInfo(GroupName:="General Info", Tips:="Nhập số phòng")>
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

        Private _waitingNumber As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        <CellInfo(GroupName:="General Info", Tips:="Nhập số thứ tự của bệnh nhân")>
        Public Property WaitingNumber() As String
            Get
                Return _waitingNumber.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("WaitingNumber", True)
                If value Is Nothing Then value = String.Empty
                If Not _waitingNumber.Equals(value) Then
                    _waitingNumber.Text = value
                    PropertyHasChanged("WaitingNumber")
                End If
            End Set
        End Property

        Private _labPackageCode As String = String.Empty
        <CellInfo("LAB_SET", GroupName:="Lab Info", Tips:="Nhập mã nhóm xét nghiệm")>
        Public Property LabPackageCode() As String
            Get
                Return _labPackageCode
            End Get
            Set(ByVal value As String)
                CanWriteProperty("LabPackageCode", True)
                If value Is Nothing Then value = String.Empty
                If Not _labPackageCode.Equals(value) Then
                    _labPackageCode = value
                    PropertyHasChanged("LabPackageCode")
                End If
            End Set
        End Property

        Private _labName As String = String.Empty
        <CellInfo(GroupName:="Lab Info", Tips:="Nhập tên nhóm xét nghiệm")>
        Public Property LabName() As String
            Get
                Return _labName
            End Get
            Set(ByVal value As String)
                CanWriteProperty("LabName", True)
                If value Is Nothing Then value = String.Empty
                If Not _labName.Equals(value) Then
                    _labName = value
                    PropertyHasChanged("LabName")
                End If
            End Set
        End Property

        Private _analysisDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo(GroupName:="Lab Info", Tips:="Nhập ngày xét nghiệm")>
        Public Property AnalysisDate() As String
            Get
                Return _analysisDate.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("AnalysisDate", True)
                If value Is Nothing Then value = String.Empty
                If Not _analysisDate.Equals(value) Then
                    _analysisDate.Text = value
                    PropertyHasChanged("AnalysisDate")
                End If
            End Set
        End Property

        Private _analysisTime As pbs.Helper.SmartTime = New pbs.Helper.SmartTime()
        <CellInfo(GroupName:="Lab Info", Tips:="Nhập thời gian xét nghiệm")>
        Public Property AnalysisTime() As String
            Get
                Return _analysisTime.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("AnalysisTime", True)
                If value Is Nothing Then value = String.Empty
                If Not _analysisTime.Equals(value) Then
                    _analysisTime.Text = value
                    PropertyHasChanged("AnalysisTime")
                End If
            End Set
        End Property

        Private _doctorAppointed As String = String.Empty
        <CellInfo(GroupName:="Lab Info", Tips:="Tên BS. chỉ định")>
        Public Property DoctorAppointed() As String
            Get
                Return _doctorAppointed
            End Get
            Set(ByVal value As String)
                CanWriteProperty("DoctorAppointed", True)
                If value Is Nothing Then value = String.Empty
                If Not _doctorAppointed.Equals(value) Then
                    _doctorAppointed = value
                    PropertyHasChanged("DoctorAppointed")
                End If
            End Set
        End Property

        Private _result As String = String.Empty
        <CellInfo(GroupName:="Lab Info", Tips:="Nhập kết quả", ControlType:=Forms.CtrlType.MemoEdit)>
        Public Property Result() As String
            Get
                Return _result
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Result", True)
                If value Is Nothing Then value = String.Empty
                If Not _result.Equals(value) Then
                    _result = value
                    PropertyHasChanged("Result")
                End If
            End Set
        End Property

        Private _laboratoryTechnician As String = String.Empty
        <CellInfo(GroupName:="Lab Info", Tips:="Nhập kết quả")>
        Public Property LaboratoryTechnician() As String
            Get
                Return _laboratoryTechnician
            End Get
            Set(ByVal value As String)
                CanWriteProperty("LaboratoryTechnician", True)
                If value Is Nothing Then value = String.Empty
                If Not _laboratoryTechnician.Equals(value) Then
                    _laboratoryTechnician = value
                    PropertyHasChanged("LaboratoryTechnician")
                End If
            End Set
        End Property

        Private _status As String = String.Empty
        <CellInfo(GroupName:="Lab Info", Tips:="Tình trạng thu phí")>
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

        Private _ncLb9 As String = String.Empty
        Public Property NcLb9() As String
            Get
                Return _ncLb9
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcLb9", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncLb9.Equals(value) Then
                    _ncLb9 = value
                    PropertyHasChanged("NcLb9")
                End If
            End Set
        End Property

        Private _ncLb8 As String = String.Empty
        Public Property NcLb8() As String
            Get
                Return _ncLb8
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcLb8", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncLb8.Equals(value) Then
                    _ncLb8 = value
                    PropertyHasChanged("NcLb8")
                End If
            End Set
        End Property

        Private _ncLb7 As String = String.Empty
        Public Property NcLb7() As String
            Get
                Return _ncLb7
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcLb7", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncLb7.Equals(value) Then
                    _ncLb7 = value
                    PropertyHasChanged("NcLb7")
                End If
            End Set
        End Property

        Private _ncLb6 As String = String.Empty
        Public Property NcLb6() As String
            Get
                Return _ncLb6
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcLb6", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncLb6.Equals(value) Then
                    _ncLb6 = value
                    PropertyHasChanged("NcLb6")
                End If
            End Set
        End Property

        Private _ncLb5 As String = String.Empty
        Public Property NcLb5() As String
            Get
                Return _ncLb5
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcLb5", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncLb5.Equals(value) Then
                    _ncLb5 = value
                    PropertyHasChanged("NcLb5")
                End If
            End Set
        End Property

        Private _ncLb4 As String = String.Empty
        Public Property NcLb4() As String
            Get
                Return _ncLb4
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcLb4", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncLb4.Equals(value) Then
                    _ncLb4 = value
                    PropertyHasChanged("NcLb4")
                End If
            End Set
        End Property

        Private _ncLb3 As String = String.Empty
        Public Property NcLb3() As String
            Get
                Return _ncLb3
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcLb3", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncLb3.Equals(value) Then
                    _ncLb3 = value
                    PropertyHasChanged("NcLb3")
                End If
            End Set
        End Property

        Private _ncLb2 As String = String.Empty
        Public Property NcLb2() As String
            Get
                Return _ncLb2
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcLb2", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncLb2.Equals(value) Then
                    _ncLb2 = value
                    PropertyHasChanged("NcLb2")
                End If
            End Set
        End Property

        Private _ncLb1 As String = String.Empty
        Public Property NcLb1() As String
            Get
                Return _ncLb1
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcLb1", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncLb1.Equals(value) Then
                    _ncLb1 = value
                    PropertyHasChanged("NcLb1")
                End If
            End Set
        End Property

        Private _ncLb0 As String = String.Empty
        Public Property NcLb0() As String
            Get
                Return _ncLb0
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcLb0", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncLb0.Equals(value) Then
                    _ncLb0 = value
                    PropertyHasChanged("NcLb0")
                End If
            End Set
        End Property

        Private _updated As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public Property Updated() As String
            Get
                Return _updated.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Updated", True)
                If value Is Nothing Then value = String.Empty
                If Not _updated.Equals(value) Then
                    _updated.Text = value
                    PropertyHasChanged("Updated")
                End If
            End Set
        End Property

        Private _updatedBy As String = String.Empty
        Public Property UpdatedBy() As String
            Get
                Return _updatedBy
            End Get
            Set(ByVal value As String)
                CanWriteProperty("UpdatedBy", True)
                If value Is Nothing Then value = String.Empty
                If Not _updatedBy.Equals(value) Then
                    _updatedBy = value
                    PropertyHasChanged("UpdatedBy")
                End If
            End Set
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

            For Each _field As ClassField In ClassSchema(Of LAB)._fieldList
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
            '_analysisDate.Text = ToDay.ToSunDate
            '_analysisTime.Text = Now.ToTimeString
            _laboratoryTechnician = Context.CurrentUserCode
        End Sub

        Public Shared Function BlankLAB() As LAB
            Return New LAB
        End Function

        Public Shared Function NewLAB(ByVal pLineNo As String) As LAB
            Dim ret = DataPortal.Create(Of LAB)(New Criteria(pLineNo))
            Return ret
        End Function

        Public Shared Function NewBO(ByVal ID As String) As LAB
            Dim pLineNo As String = ID.Trim

            Return NewLAB(pLineNo)
        End Function

        Public Shared Function GetLAB(ByVal pLineNo As String) As LAB
            Return DataPortal.Fetch(Of LAB)(New Criteria(pLineNo))
        End Function

        Public Shared Function GetBO(ByVal ID As String) As LAB
            Dim pLineNo As String = ID.Trim

            Return GetLAB(pLineNo)
        End Function

        Public Shared Sub DeleteLAB(ByVal pLineNo As String)
            DataPortal.Delete(New Criteria(pLineNo))
        End Sub

        Public Overrides Function Save() As LAB
            If Not IsDirty Then ExceptionThower.NotDirty(ResStr(ResStrConst.NOTDIRTY))
            If Not IsSavable Then Throw New Csla.Validation.ValidationException(String.Format(ResStr(ResStrConst.INVALID), ResStr("LAB")))

            Me.ApplyEdit()
            LABInfoList.InvalidateCache()
            Return MyBase.Save()
        End Function

        Public Function CloneLAB(ByVal pLineNo As String) As LAB

            If LAB.KeyDuplicated(pLineNo) Then ExceptionThower.BusinessRuleStop(ResStr(ResStrConst.CreateAlreadyExists), Me.GetType.ToString.Leaf.Translate)

            Dim cloningLAB As LAB = MyBase.Clone
            cloningLAB._lineNo = pLineNo

            'Todo:Remember to reset status of the new object here 
            cloningLAB.MarkNew()
            cloningLAB.ApplyEdit()

            cloningLAB.ValidationRules.CheckRules()

            Return cloningLAB
        End Function

#End Region ' Factory Methods

#Region " Data Access "

        <Serializable()>
        Private Class Criteria
            Public _lineNo As String = String.Empty

            Public Sub New(ByVal pLineNo As String)
                _lineNo = pLineNo

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

                    cm.CommandText = <SqlText>SELECT * FROM pbs_MC_LAB_<%= _DTB %> WHERE LINE_NO= '<%= criteria._lineNo %>' </SqlText>.Value.Trim
                    Using dr As New SafeDataReader(cm.ExecuteReader)
                        If dr.Read Then
                            FetchObject(dr)
                            MarkOld()
                        End If
                    End Using

                    cm.CommandText = <SqlText>SELECT * FROM pbs_MC_LAB_DETAIL_<%= _DTB %> WHERE LAB_ID = '<%= criteria._lineNo %>'</SqlText>.Value.Trim
                    Using dr As New SafeDataReader(cm.ExecuteReader)
                        _details = LabDets.GetLabDets(dr, Me)
                    End Using
                End Using
            End Using
        End Sub

        Private Sub FetchObject(ByVal dr As SafeDataReader)
            _lineNo = dr.GetInt32("LINE_NO")
            _patientCode = dr.GetString("PATIENT_CODE").TrimEnd
            _checkinNo.Text = dr.GetInt32("CHECKIN_NO")
            _cabinetNo = dr.GetString("CABINET_NO").TrimEnd
            _waitingNumber.Text = dr.GetInt32("WAITING_NUMBER")
            _labPackageCode = dr.GetString("LAB_PACKAGE_CODE").TrimEnd
            _labName = dr.GetString("LAB_NAME").TrimEnd
            _analysisDate.Text = dr.GetInt32("ANALYSIS_DATE")
            _analysisTime.Text = dr.GetInt32("ANALYSIS_TIME")
            _doctorAppointed = dr.GetString("DOCTOR_APPOINTED").TrimEnd
            _result = dr.GetString("RESULT").TrimEnd
            _laboratoryTechnician = dr.GetString("LABORATORY_TECHNICIAN").TrimEnd
            _status = dr.GetString("STATUS").TrimEnd
            _ncLb9 = dr.GetString("NC_LB9").TrimEnd
            _ncLb8 = dr.GetString("NC_LB8").TrimEnd
            _ncLb7 = dr.GetString("NC_LB7").TrimEnd
            _ncLb6 = dr.GetString("NC_LB6").TrimEnd
            _ncLb5 = dr.GetString("NC_LB5").TrimEnd
            _ncLb4 = dr.GetString("NC_LB4").TrimEnd
            _ncLb3 = dr.GetString("NC_LB3").TrimEnd
            _ncLb2 = dr.GetString("NC_LB2").TrimEnd
            _ncLb1 = dr.GetString("NC_LB1").TrimEnd
            _ncLb0 = dr.GetString("NC_LB0").TrimEnd
            _updated.Text = dr.GetInt32("UPDATED")
            _updatedBy = dr.GetString("UPDATED_BY").TrimEnd

        End Sub

        Private Shared _lockObj As New Object
        Protected Overrides Sub DataPortal_Insert()
            SyncLock _lockObj
                Using ctx = ConnectionFactory.GetDBConnection(True)
                    Using cm = ctx.CreateSQLCommand

                        cm.CommandType = CommandType.StoredProcedure
                        cm.CommandText = String.Format("pbs_MC_LAB_{0}_Insert", _DTB)

                        cm.Parameters.AddWithValue("@LINE_NO", _lineNo.Trim.ToInteger, ParameterDirection.Output)
                        AddInsertParameters(cm)
                        cm.ExecuteNonQuery()

                        _lineNo = cm.GetReturnIdAsInteger("@LINE_NO")
                    End Using

                    Me.Details.Update(ctx, Me)

                End Using
            End SyncLock
        End Sub

        Private Sub AddInsertParameters(ByVal cm As IDBCommand)
            cm.Parameters.AddWithValue("@PATIENT_CODE", _patientCode.Trim)
            cm.Parameters.AddWithValue("@CHECKIN_NO", _checkinNo.DBValue)
            cm.Parameters.AddWithValue("@CABINET_NO", _cabinetNo.Trim)
            cm.Parameters.AddWithValue("@WAITING_NUMBER", _waitingNumber.DBValue)
            cm.Parameters.AddWithValue("@LAB_PACKAGE_CODE", _labPackageCode.Trim)
            cm.Parameters.AddWithValue("@LAB_NAME", _labName.Trim)
            cm.Parameters.AddWithValue("@ANALYSIS_DATE", _analysisDate.DBValue)
            cm.Parameters.AddWithValue("@ANALYSIS_TIME", _analysisTime.DBValue)
            cm.Parameters.AddWithValue("@DOCTOR_APPOINTED", _doctorAppointed.Trim)
            cm.Parameters.AddWithValue("@RESULT", _result.Trim)
            cm.Parameters.AddWithValue("@LABORATORY_TECHNICIAN", _laboratoryTechnician.Trim)
            cm.Parameters.AddWithValue("@STATUS", _status.Trim)
            cm.Parameters.AddWithValue("@NC_LB9", _ncLb9.Trim)
            cm.Parameters.AddWithValue("@NC_LB8", _ncLb8.Trim)
            cm.Parameters.AddWithValue("@NC_LB7", _ncLb7.Trim)
            cm.Parameters.AddWithValue("@NC_LB6", _ncLb6.Trim)
            cm.Parameters.AddWithValue("@NC_LB5", _ncLb5.Trim)
            cm.Parameters.AddWithValue("@NC_LB4", _ncLb4.Trim)
            cm.Parameters.AddWithValue("@NC_LB3", _ncLb3.Trim)
            cm.Parameters.AddWithValue("@NC_LB2", _ncLb2.Trim)
            cm.Parameters.AddWithValue("@NC_LB1", _ncLb1.Trim)
            cm.Parameters.AddWithValue("@NC_LB0", _ncLb0.Trim)
            cm.Parameters.AddWithValue("@UPDATED", _updated.DBValue)
            cm.Parameters.AddWithValue("@UPDATED_BY", _updatedBy.Trim)
        End Sub


        Protected Overrides Sub DataPortal_Update()
            SyncLock _lockObj
                Using ctx = ConnectionFactory.GetDBConnection(True)
                    Using cm = ctx.CreateSQLCommand

                        cm.CommandType = CommandType.StoredProcedure
                        cm.CommandText = String.Format("pbs_MC_LAB_{0}_Update", _DTB)

                        cm.Parameters.AddWithValue("@LINE_NO", _lineNo.Trim)
                        AddInsertParameters(cm)
                        cm.ExecuteNonQuery()

                    End Using

                    Me.Details.Update(ctx, Me)

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
                    cm.CommandText = <SqlText>DELETE FROM pbs_MC_LAB_<%= _DTB %> WHERE LINE_NO= '<%= criteria._lineNo %>' </SqlText>.Value.Trim
                    cm.ExecuteNonQuery()

                End Using
            End Using

        End Sub

        Protected Overrides Sub DataPortal_OnDataPortalInvokeComplete(ByVal e As Csla.DataPortalEventArgs)
            If Csla.ApplicationContext.ExecutionLocation = ExecutionLocations.Server Then
                LABInfoList.InvalidateCache()
            End If
        End Sub


#End Region 'Data Access                           

#Region " Exists "
        Public Shared Function Exists(ByVal pLineNo As String) As Boolean
            Return LABInfoList.ContainsCode(pLineNo)
        End Function

        Public Shared Function KeyDuplicated(ByVal pLineNo As String) As Boolean
            Dim SqlText = <SqlText>SELECT COUNT(*) FROM pbs_MC_LAB_<%= Context.CurrentBECode %> WHERE LINE_NO= '<%= pLineNo %>'</SqlText>.Value.Trim
            Return SQLCommander.GetScalarInteger(SqlText) > 0
        End Function
#End Region

#Region " IGenpart "

        Public Function CloneBO(ByVal id As String) As Object Implements Interfaces.IGenPartObject.CloneBO
            Return CloneLAB(id)
        End Function

        Public Function getBO1(ByVal id As String) As Object Implements Interfaces.IGenPartObject.GetBO
            Return GetBO(id)
        End Function

        Public Function myCommands() As String() Implements Interfaces.IGenPartObject.myCommands
            Return pbs.Helper.Action.StandardReferenceCommands
        End Function

        Public Function myFullName() As String Implements Interfaces.IGenPartObject.myFullName
            Return GetType(LAB).ToString
        End Function

        Public Function myName() As String Implements Interfaces.IGenPartObject.myName
            Return GetType(LAB).ToString.Leaf
        End Function

        Public Function myQueryList() As IList Implements Interfaces.IGenPartObject.myQueryList
            Return LABInfoList.GetLABInfoList
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