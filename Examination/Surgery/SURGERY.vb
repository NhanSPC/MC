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
    <DB(TableName:="pbs_MC_SURGERY_{XXX}")>
    Public Class SURGERY
        Inherits Csla.BusinessBase(Of SURGERY)
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
                    GetPatientCode(CheckinNo)
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

        Private Sub GetPatientCode(ByVal pCheckinNo As Integer)
            For Each itm In CHECKINInfoList.GetCHECKINInfoList
                If pCheckinNo = itm.LineNo Then
                    PatientCode = itm.PatientCode
                End If
            Next
        End Sub


#Region " Business Properties and Methods "
        Private _DTB As String = String.Empty


        Private _lineNo As String = String.Empty
        <System.ComponentModel.DataObjectField(True, True)>
        Public ReadOnly Property LineNo() As String
            Get
                Return _lineNo
            End Get
        End Property

        Private _patientCode As String = String.Empty
        <CellInfo("pbs.BO.mc.PATIENT", GroupName:="General Info", Tips:="Chose patient code")>
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
        <CellInfo("pbs.BO.mc.CHECKIN", GroupName:="General Info", Tips:="Chose check-in code")>
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

        Private _gencheckNo As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        <CellInfo("pbs.BO.mc.GENCHECK", GroupName:="General Info", Tips:="Chose general check code")>
        Public Property GencheckNo() As String
            Get
                Return _gencheckNo.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("GencheckNo", True)
                If value Is Nothing Then value = String.Empty
                If Not _gencheckNo.Equals(value) Then
                    _gencheckNo.Text = value
                    PropertyHasChanged("GencheckNo")
                End If
            End Set
        End Property

        Private _surgeryDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo("CALENDAR", GroupName:="Surgery Info", Tips:="Enter surgery date")>
        Public Property SurgeryDate() As String
            Get
                Return _surgeryDate.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("SurgeryDate", True)
                If value Is Nothing Then value = String.Empty
                If Not _surgeryDate.Equals(value) Then
                    _surgeryDate.Text = value
                    PropertyHasChanged("SurgeryDate")
                End If
            End Set
        End Property

        Private _surgeryTime As pbs.Helper.SmartTime = New pbs.Helper.SmartTime()
        <CellInfo("HOUR", GroupName:="Surgery Info", Tips:="Enter surgery time")>
        Public Property SurgeryTime() As String
            Get
                Return _surgeryTime.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("SurgeryTime", True)
                If value Is Nothing Then value = String.Empty
                If Not _surgeryTime.Equals(value) Then
                    _surgeryTime.Text = value
                    PropertyHasChanged("SurgeryTime")
                End If
            End Set
        End Property

        Private _preoperativeDiagnos As String = String.Empty
        <CellInfo(GroupName:="Surgery Info", Tips:="Enter preoperative diagnos", ControlType:=Forms.CtrlType.MemoEdit)>
        Public Property PreoperativeDiagnos() As String
            Get
                Return _preoperativeDiagnos
            End Get
            Set(ByVal value As String)
                CanWriteProperty("PreoperativeDiagnos", True)
                If value Is Nothing Then value = String.Empty
                If Not _preoperativeDiagnos.Equals(value) Then
                    _preoperativeDiagnos = value
                    PropertyHasChanged("PreoperativeDiagnos")
                End If
            End Set
        End Property

        Private _postoperativeDiagnos As String = String.Empty
        <CellInfo(GroupName:="Surgery Info", Tips:="Enter postoperative diagnos", ControlType:=Forms.CtrlType.MemoEdit)>
        Public Property PostoperativeDiagnos() As String
            Get
                Return _postoperativeDiagnos
            End Get
            Set(ByVal value As String)
                CanWriteProperty("PostoperativeDiagnos", True)
                If value Is Nothing Then value = String.Empty
                If Not _postoperativeDiagnos.Equals(value) Then
                    _postoperativeDiagnos = value
                    PropertyHasChanged("PostoperativeDiagnos")
                End If
            End Set
        End Property

        Private _surgicalType As String = String.Empty
        <CellInfo(GroupName:="Surgery Info", Tips:="Enter surgery type")>
        Public Property SurgicalType() As String
            Get
                Return _surgicalType
            End Get
            Set(ByVal value As String)
                CanWriteProperty("SurgicalType", True)
                If value Is Nothing Then value = String.Empty
                If Not _surgicalType.Equals(value) Then
                    _surgicalType = value
                    PropertyHasChanged("SurgicalType")
                End If
            End Set
        End Property

        Private _surgicalMethod As String = String.Empty
        <CellInfo(GroupName:="Surgery Info", Tips:="Enter surgery method")>
        Public Property SurgicalMethod() As String
            Get
                Return _surgicalMethod
            End Get
            Set(ByVal value As String)
                CanWriteProperty("SurgicalMethod", True)
                If value Is Nothing Then value = String.Empty
                If Not _surgicalMethod.Equals(value) Then
                    _surgicalMethod = value
                    PropertyHasChanged("SurgicalMethod")
                End If
            End Set
        End Property

        Private _surgeon As String = String.Empty
        <CellInfo(GroupName:="Doctor Info", Tips:="Enter surgeon code")>
        Public Property Surgeon() As String
            Get
                Return _surgeon
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Surgeon", True)
                If value Is Nothing Then value = String.Empty
                If Not _surgeon.Equals(value) Then
                    _surgeon = value
                    PropertyHasChanged("Surgeon")
                End If
            End Set
        End Property

        Private _anaesthetist As String = String.Empty
        <CellInfo(GroupName:="Doctor Info", Tips:="Enter anaesthetists code")>
        Public Property Anaesthetist() As String
            Get
                Return _anaesthetist
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Anaesthetist", True)
                If value Is Nothing Then value = String.Empty
                If Not _anaesthetist.Equals(value) Then
                    _anaesthetist = value
                    PropertyHasChanged("Anaesthetist")
                End If
            End Set
        End Property

        Private _surgeryDiagram As String = String.Empty
        <CellInfo(GroupName:="Surgery detail", Tips:="Surgery diagram", ControlType:=Forms.CtrlType.MemoEdit)>
        Public Property SurgeryDiagram() As String
            Get
                Return _surgeryDiagram
            End Get
            Set(ByVal value As String)
                CanWriteProperty("SurgeryDiagram", True)
                If value Is Nothing Then value = String.Empty
                If Not _surgeryDiagram.Equals(value) Then
                    _surgeryDiagram = value
                    PropertyHasChanged("SurgeryDiagram")
                End If
            End Set
        End Property

        Private _operationSteps As String = String.Empty
        <CellInfo(GroupName:="Surgery detail", Tips:="Operation steps", ControlType:=Forms.CtrlType.MemoEdit)>
        Public Property OperationSteps() As String
            Get
                Return _operationSteps
            End Get
            Set(ByVal value As String)
                CanWriteProperty("OperationSteps", True)
                If value Is Nothing Then value = String.Empty
                If Not _operationSteps.Equals(value) Then
                    _operationSteps = value
                    PropertyHasChanged("OperationSteps")
                End If
            End Set
        End Property

        Private _result As String = String.Empty
        <CellInfo(GroupName:="Surgery detail", Tips:="Attachment files", ControlType:=Forms.CtrlType.MemoEdit)>
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

        Private _treatment As String = String.Empty
        <CellInfo(GroupName:="Surgery detail", Tips:="Attachment files", ControlType:=Forms.CtrlType.MemoEdit)>
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

            For Each _field As ClassField In ClassSchema(Of SURGERY)._fieldList
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

        Public Shared Function BlankSURGERY() As SURGERY
            Return New SURGERY
        End Function

        Public Shared Function NewSURGERY(ByVal pLineNo As String) As SURGERY
            If KeyDuplicated(pLineNo) Then ExceptionThower.BusinessRuleStop(String.Format(ResStr(ResStrConst.NOACCESS), ResStr("SURGERY")))
            Return DataPortal.Create(Of SURGERY)(New Criteria(pLineNo))
        End Function

        Public Shared Function NewBO(ByVal ID As String) As SURGERY
            Dim pLineNo As String = ID.Trim

            Return NewSURGERY(pLineNo)
        End Function

        Public Shared Function GetSURGERY(ByVal pLineNo As String) As SURGERY
            Return DataPortal.Fetch(Of SURGERY)(New Criteria(pLineNo))
        End Function

        Public Shared Function GetBO(ByVal ID As String) As SURGERY
            Dim pLineNo As String = ID.Trim

            Return GetSURGERY(pLineNo)
        End Function

        Public Shared Sub DeleteSURGERY(ByVal pLineNo As String)
            DataPortal.Delete(New Criteria(pLineNo))
        End Sub

        Public Overrides Function Save() As SURGERY
            If Not IsDirty Then ExceptionThower.NotDirty(ResStr(ResStrConst.NOTDIRTY))
            If Not IsSavable Then Throw New Csla.Validation.ValidationException(String.Format(ResStr(ResStrConst.INVALID), ResStr("SURGERY")))

            Me.ApplyEdit()
            SURGERYInfoList.InvalidateCache()
            Return MyBase.Save()
        End Function

        Public Function CloneSURGERY(ByVal pLineNo As String) As SURGERY

            If SURGERY.KeyDuplicated(pLineNo) Then ExceptionThower.BusinessRuleStop(ResStr(ResStrConst.CreateAlreadyExists), Me.GetType.ToString.Leaf.Translate)

            Dim cloningSURGERY As SURGERY = MyBase.Clone
            cloningSURGERY._lineNo = pLineNo

            'Todo:Remember to reset status of the new object here 
            cloningSURGERY.MarkNew()
            cloningSURGERY.ApplyEdit()

            cloningSURGERY.ValidationRules.CheckRules()

            Return cloningSURGERY
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

                    cm.CommandText = <SqlText>SELECT * FROM pbs_MC_SURGERY_<%= _DTB %> WHERE LINE_NO= '<%= criteria._lineNo %>' </SqlText>.Value.Trim

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
            _patientCode = dr.GetString("PATIENT_CODE").TrimEnd
            _checkinNo.Text = dr.GetInt32("CHECKIN_NO")
            _gencheckNo.Text = dr.GetInt32("GENCHECK_NO")
            _surgeryDate.Text = dr.GetInt32("SURGERY_DATE")
            _surgeryTime.Text = dr.GetInt32("SURGERY_TIME")
            _preoperativeDiagnos = dr.GetString("PREOPERATIVE_DIAGNOS").TrimEnd
            _postoperativeDiagnos = dr.GetString("POSTOPERATIVE_DIAGNOS").TrimEnd
            _surgicalType = dr.GetString("SURGICAL_TYPE").TrimEnd
            _surgicalMethod = dr.GetString("SURGICAL_METHOD").TrimEnd
            _surgeon = dr.GetString("SURGEON").TrimEnd
            _anaesthetist = dr.GetString("ANAESTHETIST").TrimEnd
            _surgeryDiagram = dr.GetString("SURGERY_DIAGRAM").TrimEnd
            _operationSteps = dr.GetString("OPERATION_STEPS").TrimEnd
            _result = dr.GetString("RESULT").TrimEnd
            _treatment = dr.GetString("TREATMENT").TrimEnd
            _updated.Text = dr.GetInt32("UPDATED")
            _updatedBy = dr.GetString("UPDATED_BY").TrimEnd

        End Sub

        Private Shared _lockObj As New Object
        Protected Overrides Sub DataPortal_Insert()
            SyncLock _lockObj
                Using ctx = ConnectionFactory.GetDBConnection(True)
                    Using cm = ctx.CreateSQLCommand

                        cm.CommandType = CommandType.StoredProcedure
                        cm.CommandText = String.Format("pbs_MC_SURGERY_{0}_Insert", _DTB)

                        cm.Parameters.AddWithValue("@LINE_NO", _lineNo.Trim.ToInteger, ParameterDirection.Output)
                        AddInsertParameters(cm)
                        cm.ExecuteNonQuery()

                        _lineNo = cm.GetReturnIdAsInteger("@LINE_NO")
                    End Using
                End Using
            End SyncLock
        End Sub

        Private Sub AddInsertParameters(ByVal cm As IDBCommand)

            cm.Parameters.AddWithValue("@PATIENT_CODE", _patientCode.Trim)
            cm.Parameters.AddWithValue("@CHECKIN_NO", _checkinNo.DBValue)
            cm.Parameters.AddWithValue("@GENCHECK_NO", _gencheckNo.DBValue)
            cm.Parameters.AddWithValue("@SURGERY_DATE", _surgeryDate.DBValue)
            cm.Parameters.AddWithValue("@SURGERY_TIME", _surgeryTime.DBValue)
            cm.Parameters.AddWithValue("@PREOPERATIVE_DIAGNOS", _preoperativeDiagnos.Trim)
            cm.Parameters.AddWithValue("@POSTOPERATIVE_DIAGNOS", _postoperativeDiagnos.Trim)
            cm.Parameters.AddWithValue("@SURGICAL_TYPE", _surgicalType.Trim)
            cm.Parameters.AddWithValue("@SURGICAL_METHOD", _surgicalMethod.Trim)
            cm.Parameters.AddWithValue("@SURGEON", _surgeon.Trim)
            cm.Parameters.AddWithValue("@ANAESTHETIST", _anaesthetist.Trim)
            cm.Parameters.AddWithValue("@SURGERY_DIAGRAM", _surgeryDiagram.Trim)
            cm.Parameters.AddWithValue("@OPERATION_STEPS", _operationSteps.Trim)
            cm.Parameters.AddWithValue("@RESULT", _result.Trim)
            cm.Parameters.AddWithValue("@TREATMENT", _treatment.Trim)
            cm.Parameters.AddWithValue("@UPDATED", ToDay.ToSunDate)
            cm.Parameters.AddWithValue("@UPDATED_BY", Context.CurrentUserCode)
        End Sub


        Protected Overrides Sub DataPortal_Update()
            SyncLock _lockObj
                Using ctx = ConnectionFactory.GetDBConnection(True)
                    Using cm = ctx.CreateSQLCommand

                        cm.CommandType = CommandType.StoredProcedure
                        cm.CommandText = String.Format("pbs_MC_SURGERY_{0}_Update", _DTB)

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

                    cm.CommandText = <SqlText>DELETE FROM pbs_MC_SURGERY_<%= _DTB %> WHERE LINE_NO= '<%= criteria._lineNo %>' </SqlText>.Value.Trim
                    cm.ExecuteNonQuery()

                End Using
            End Using

        End Sub

        Protected Overrides Sub DataPortal_OnDataPortalInvokeComplete(ByVal e As Csla.DataPortalEventArgs)
            If Csla.ApplicationContext.ExecutionLocation = ExecutionLocations.Server Then
                SURGERYInfoList.InvalidateCache()
            End If
        End Sub


#End Region 'Data Access                           

#Region " Exists "
        Public Shared Function Exists(ByVal pLineNo As String) As Boolean
            Return SURGERYInfoList.ContainsCode(pLineNo)
        End Function

        Public Shared Function KeyDuplicated(ByVal pLineNo As String) As Boolean
            Dim SqlText = <SqlText>SELECT COUNT(*) FROM pbs_MC_SURGERY_<%= Context.CurrentBECode %> WHERE LINE_NO= '<%= pLineNo %>'</SqlText>.Value.Trim
            Return SQLCommander.GetScalarInteger(SqlText) > 0
        End Function
#End Region

#Region " IGenpart "

        Public Function CloneBO(ByVal id As String) As Object Implements Interfaces.IGenPartObject.CloneBO
            Return CloneSURGERY(id)
        End Function

        Public Function getBO1(ByVal id As String) As Object Implements Interfaces.IGenPartObject.GetBO
            Return GetBO(id)
        End Function

        Public Function myCommands() As String() Implements Interfaces.IGenPartObject.myCommands
            Return pbs.Helper.Action.StandardReferenceCommands
        End Function

        Public Function myFullName() As String Implements Interfaces.IGenPartObject.myFullName
            Return GetType(SURGERY).ToString
        End Function

        Public Function myName() As String Implements Interfaces.IGenPartObject.myName
            Return GetType(SURGERY).ToString.Leaf
        End Function

        Public Function myQueryList() As IList Implements Interfaces.IGenPartObject.myQueryList
            Return SURGERYInfoList.GetSURGERYInfoList
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