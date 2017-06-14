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
    <DB(TableName:="pbs_MC_PRESCRIPTION_{XXX}")>
    Public Class PRESCRIPTION
        Inherits Csla.BusinessBase(Of PRESCRIPTION)
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

                Case "CheckinNo"
                    For Each itm In CHECKINInfoList.GetCHECKINInfoList
                        If CheckinNo = itm.LineNo Then
                            PatientCode = itm.PatientCode
                        End If
                    Next

                Case "SamplePrescriptionCode"
                    SamplePrescription()

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

        Sub SamplePrescription()
            If String.IsNullOrEmpty(SamplePrescriptionCode) Then Exit Sub

            If Me.Details.Count > 0 Then
                If Helper.UIServices.ConfirmService.Confirm("Details for this Prescription has already exist. Do you want to reset it?") Then
                    Details.Clear()
                End If
            End If

            For Each itm In SAMPLEPRESCInfoList.GetSAMPLEPRESCInfoList
                If SamplePrescriptionCode = itm.SamplePrescriptionCode Then
                    Dim newDetail = Details.AddNew
                    newDetail.ItemCode = itm.ItemCode
                    'newDetail.PrescriptionNo = LineNo
                    newDetail.ItemName = itm.ItemName
                    newDetail.ActiveIngrendient = itm.ActiveIngrendient
                    newDetail.Unit = itm.Unit
                    newDetail.DateOfIssue = itm.DateOfIssue
                    newDetail.Morning = itm.Morning
                    newDetail.Noon = itm.Noon
                    newDetail.Afternoon = itm.Afternoon
                    newDetail.Evening = itm.Evening
                    newDetail.Quantity = itm.Quantity
                    newDetail.BeforeBreakfast = itm.BeforeBreakfast
                    newDetail.BeforeLunch = itm.BeforeLunch
                    newDetail.BeforeSupper = itm.BeforeSupper
                    newDetail.BeforeDinner = itm.BeforeDinner
                    newDetail.Instruction = itm.Instruction

                End If
            Next

            For Each itm In Me.Details
                itm.Instruction = itm.PrescriptionInstruction
            Next


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
        <CellInfo("pbs.BO.MC.PATIENT", GroupName:="Prescription Info", Tips:="Enter patient code")>
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
        <CellInfo("pbs.BO.MC.CHECKIN", GroupName:="Prescription Info", Tips:="Enter check-in number")>
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

        Private _doctorAppointed As String = String.Empty
        <CellInfo(GroupName:="Prescription Info", Tips:="Enter doctor appointed code")>
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

        Private _prescriptionDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo("CALENDAR", GroupName:="Prescription Info", Tips:="Enter date of prescription")>
        <Rule(Required:=True)>
        Public Property PrescriptionDate() As String
            Get
                Return _prescriptionDate.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("PrescriptionDate", True)
                If value Is Nothing Then value = String.Empty
                If Not _prescriptionDate.Equals(value) Then
                    _prescriptionDate.Text = value
                    PropertyHasChanged("PrescriptionDate")
                End If
            End Set
        End Property

        Private _samplePrescriptionCode As String = String.Empty
        <CellInfo("SamplePres", GroupName:="Prescription Info", Tips:="Enter sample prescription code")>
        Public Property SamplePrescriptionCode() As String
            Get
                Return _samplePrescriptionCode
            End Get
            Set(ByVal value As String)
                CanWriteProperty("SamplePrescriptionCode", True)
                If value Is Nothing Then value = String.Empty
                If Not _samplePrescriptionCode.Equals(value) Then
                    _samplePrescriptionCode = value
                    PropertyHasChanged("SamplePrescriptionCode")
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

            For Each _field As ClassField In ClassSchema(Of PRESCRIPTION)._fieldList
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
            _prescriptionDate.Text = ToDay.ToSunDate
        End Sub

        Public Shared Function BlankPRESCRIPTION() As PRESCRIPTION
            Return New PRESCRIPTION
        End Function

        Public Shared Function NewPRESCRIPTION(ByVal pLineNo As String) As PRESCRIPTION
            If KeyDuplicated(pLineNo) Then ExceptionThower.BusinessRuleStop(String.Format(ResStr(ResStrConst.NOACCESS), ResStr("PRESCRIPTION")))
            Return DataPortal.Create(Of PRESCRIPTION)(New Criteria(pLineNo))
        End Function

        Public Shared Function NewBO(ByVal ID As String) As PRESCRIPTION
            Dim pLineNo As String = ID.Trim

            Return NewPRESCRIPTION(pLineNo)
        End Function

        Public Shared Function GetPRESCRIPTION(ByVal pLineNo As String) As PRESCRIPTION
            Return DataPortal.Fetch(Of PRESCRIPTION)(New Criteria(pLineNo))
        End Function

        Public Shared Function GetBO(ByVal ID As String) As PRESCRIPTION
            Dim pLineNo As String = ID.Trim

            Return GetPRESCRIPTION(pLineNo)
        End Function

        Public Shared Sub DeletePRESCRIPTION(ByVal pLineNo As String)
            DataPortal.Delete(New Criteria(pLineNo))
        End Sub

        Public Overrides Function Save() As PRESCRIPTION
            If Not IsDirty Then ExceptionThower.NotDirty(ResStr(ResStrConst.NOTDIRTY))
            If Not IsSavable Then Throw New Csla.Validation.ValidationException(String.Format(ResStr(ResStrConst.INVALID), ResStr("PRESCRIPTION")))

            Me.ApplyEdit()
            PRESCRIPTIONInfoList.InvalidateCache()
            Return MyBase.Save()
        End Function

        Public Function ClonePRESCRIPTION(ByVal pLineNo As String) As PRESCRIPTION

            If PRESCRIPTION.KeyDuplicated(pLineNo) Then ExceptionThower.BusinessRuleStop(ResStr(ResStrConst.CreateAlreadyExists), Me.GetType.ToString.Leaf.Translate)

            Dim cloningPRESCRIPTION As PRESCRIPTION = MyBase.Clone
            cloningPRESCRIPTION._lineNo = pLineNo

            'Todo:Remember to reset status of the new object here 
            cloningPRESCRIPTION.MarkNew()
            cloningPRESCRIPTION.ApplyEdit()

            cloningPRESCRIPTION.ValidationRules.CheckRules()

            Return cloningPRESCRIPTION
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

                    cm.CommandText = <SqlText>SELECT * FROM pbs_MC_PRESCRIPTION_<%= _DTB %> WHERE LINE_NO= '<%= criteria._lineNo %>' </SqlText>.Value.Trim
                    Using dr As New SafeDataReader(cm.ExecuteReader)
                        If dr.Read Then
                            FetchObject(dr)
                            MarkOld()
                        End If
                    End Using

                    cm.CommandText = <SqlText>SELECT * FROM pbs_MC_PRESCRIPTION_DETAIL_<%= _DTB %> WHERE PRESCRIPTION_NO = '<%= criteria._lineNo %>'</SqlText>.Value.Trim
                    Using dr As New SafeDataReader(cm.ExecuteReader)
                        _details = PrescriptionDetails.GetPrescriptionDetails(dr, Me)
                    End Using
                End Using
            End Using
        End Sub

        Private Sub FetchObject(ByVal dr As SafeDataReader)
            _lineNo = dr.GetInt32("LINE_NO")
            _patientCode = dr.GetString("PATIENT_CODE").TrimEnd
            _checkinNo.Text = dr.GetInt32("CHECKIN_NO")
            _doctorAppointed = dr.GetString("DOCTOR_APPOINTED").TrimEnd
            _prescriptionDate.Text = dr.GetInt32("PRESCRIPTION_DATE")
            _samplePrescriptionCode = dr.GetString("SAMPLE_PRESCRIPTION_CODE").TrimEnd
            _updated.Text = dr.GetInt32("UPDATED")
            _updatedBy = dr.GetString("UPDATED_BY").TrimEnd

        End Sub

        Private Shared _lockObj As New Object
        Protected Overrides Sub DataPortal_Insert()
            SyncLock _lockObj
                Using ctx = ConnectionFactory.GetDBConnection(True)
                    Using cm = ctx.CreateSQLCommand

                        cm.CommandType = CommandType.StoredProcedure
                        cm.CommandText = String.Format("pbs_MC_PRESCRIPTION_{0}_Insert", _DTB)

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
            cm.Parameters.AddWithValue("@DOCTOR_APPOINTED", _doctorAppointed.Trim)
            cm.Parameters.AddWithValue("@PRESCRIPTION_DATE", _prescriptionDate.DBValue)
            cm.Parameters.AddWithValue("@SAMPLE_PRESCRIPTION_CODE", _samplePrescriptionCode)
            cm.Parameters.AddWithValue("@UPDATED", _updated.DBValue)
            cm.Parameters.AddWithValue("@UPDATED_BY", _updatedBy.Trim)
        End Sub


        Protected Overrides Sub DataPortal_Update()
            SyncLock _lockObj
                Using ctx = ConnectionFactory.GetDBConnection(True)
                    Using cm = ctx.CreateSQLCommand

                        cm.CommandType = CommandType.StoredProcedure
                        cm.CommandText = String.Format("pbs_MC_PRESCRIPTION_{0}_Update", _DTB)

                        cm.Parameters.AddWithValue("@LINE_NO", _lineNo.Trim)
                        AddInsertParameters(cm)
                        cm.ExecuteNonQuery()

                        _lineNo = cm.GetReturnIdAsInteger("@LINE_NO")
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

                    cm.CommandText = <SqlText>DELETE FROM pbs_MC_PRESCRIPTION_<%= _DTB %> WHERE LINE_NO= '<%= criteria._lineNo %>' </SqlText>.Value.Trim
                    cm.ExecuteNonQuery()

                End Using
            End Using

        End Sub

        Protected Overrides Sub DataPortal_OnDataPortalInvokeComplete(ByVal e As Csla.DataPortalEventArgs)
            If Csla.ApplicationContext.ExecutionLocation = ExecutionLocations.Server Then
                PRESCRIPTIONInfoList.InvalidateCache()
            End If
        End Sub


#End Region 'Data Access                           

#Region " Exists "
        Public Shared Function Exists(ByVal pLineNo As String) As Boolean
            Return PRESCRIPTIONInfoList.ContainsCode(pLineNo)
        End Function

        Public Shared Function KeyDuplicated(ByVal pLineNo As String) As Boolean
            Dim SqlText = <SqlText>SELECT COUNT(*) FROM pbs_MC_PRESCRIPTION_<%= Context.CurrentBECode %> WHERE LINE_NO= '<%= pLineNo %>'</SqlText>.Value.Trim
            Return SQLCommander.GetScalarInteger(SqlText) > 0
        End Function
#End Region

#Region " IGenpart "

        Public Function CloneBO(ByVal id As String) As Object Implements Interfaces.IGenPartObject.CloneBO
            Return ClonePRESCRIPTION(id)
        End Function

        Public Function getBO1(ByVal id As String) As Object Implements Interfaces.IGenPartObject.GetBO
            Return GetBO(id)
        End Function

        Public Function myCommands() As String() Implements Interfaces.IGenPartObject.myCommands
            Return pbs.Helper.Action.StandardReferenceCommands
        End Function

        Public Function myFullName() As String Implements Interfaces.IGenPartObject.myFullName
            Return GetType(PRESCRIPTION).ToString
        End Function

        Public Function myName() As String Implements Interfaces.IGenPartObject.myName
            Return GetType(PRESCRIPTION).ToString.Leaf
        End Function

        Public Function myQueryList() As IList Implements Interfaces.IGenPartObject.myQueryList
            Return PRESCRIPTIONInfoList.GetPRESCRIPTIONInfoList
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