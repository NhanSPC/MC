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
    <DB(TableName:="pbs_MC_DEAD_XXX")>
    Public Class DEAD
        Inherits Csla.BusinessBase(Of DEAD)
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

                Case "MainReasonCode"
                    For Each itm In LOOKUPInfoList.GetLOOKUPInfoList_ByCategory("ICD-10", True)
                        If MainReasonCode = itm.Code Then
                            MainReason = itm.Descriptn1
                        End If
                    Next

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

        Private _checkinNo As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        <CellInfo("pbs.BO.MC.CHECKIN", GroupName:="General info", Tips:="Enter check-in code")>
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

        Private _patientCode As String = String.Empty
        <CellInfo("pbs.BO.MC.PATIENT", GroupName:="General info", Tips:="Enter patient code")>
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

        Private _deadTime As pbs.Helper.SmartTime = New pbs.Helper.SmartTime()
        <CellInfo(GroupName:="Dead info", Tips:="Enter dead time")>
        Public Property DeadTime() As String
            Get
                Return _deadTime.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("DeadTime", True)
                If value Is Nothing Then value = String.Empty
                If Not _deadTime.Equals(value) Then
                    _deadTime.Text = value
                    PropertyHasChanged("DeadTime")
                End If
            End Set
        End Property

        Private _deadDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo("CALENDAR", GroupName:="Dead info", Tips:="Enter dead date")>
        Public Property DeadDate() As String
            Get
                Return _deadDate.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("DeadDate", True)
                If value Is Nothing Then value = String.Empty
                If Not _deadDate.Equals(value) Then
                    _deadDate.Text = value
                    PropertyHasChanged("DeadDate")
                End If
            End Set
        End Property

        Private _deadReason As String = String.Empty
        <CellInfo("ICD-10", GroupName:="Dead info", Tips:="Enter dead reason")>
        Public Property DeadReason() As String
            Get
                Return _deadReason
            End Get
            Set(ByVal value As String)
                CanWriteProperty("DeadReason", True)
                If value Is Nothing Then value = String.Empty
                If Not _deadReason.Equals(value) Then
                    _deadReason = value
                    PropertyHasChanged("DeadReason")
                End If
            End Set
        End Property

        Private _mainReason As String = String.Empty
        <CellInfo(GroupName:="Dead info", Tips:="Enter main dead reason")>
        Public Property MainReason() As String
            Get
                Return _mainReason
            End Get
            Set(ByVal value As String)
                CanWriteProperty("MainReason", True)
                If value Is Nothing Then value = String.Empty
                If Not _mainReason.Equals(value) Then
                    _mainReason = value
                    PropertyHasChanged("MainReason")
                End If
            End Set
        End Property

        Private _mainReasonCode As String = String.Empty
        <CellInfo("ICD-10", GroupName:="Dead info", Tips:="Enter main code reason code")>
        Public Property MainReasonCode() As String
            Get
                Return _mainReasonCode
            End Get
            Set(ByVal value As String)
                CanWriteProperty("MainReasonCode", True)
                If value Is Nothing Then value = String.Empty
                If Not _mainReasonCode.Equals(value) Then
                    _mainReasonCode = value
                    PropertyHasChanged("MainReasonCode")
                End If
            End Set
        End Property

        Private _autopsy As String = String.Empty
        <CellInfo(GroupName:="Autopsy info", Tips:="Check if autopsy", ControlType:=Forms.CtrlType.ToggleSwitch)>
        Public Property Autopsy() As String
            Get
                Return _autopsy
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Autopsy", True)
                If value Is Nothing Then value = String.Empty
                If Not _autopsy.Equals(value) Then
                    _autopsy = value
                    PropertyHasChanged("Autopsy")
                End If
            End Set
        End Property

        Private _autopsyResult As String = String.Empty
        <CellInfo(GroupName:="Autopsy info", Tips:="Enter autopsy result")>
        Public Property AutopsyResult() As String
            Get
                Return _autopsyResult
            End Get
            Set(ByVal value As String)
                CanWriteProperty("AutopsyResult", True)
                If value Is Nothing Then value = String.Empty
                If Not _autopsyResult.Equals(value) Then
                    _autopsyResult = value
                    PropertyHasChanged("AutopsyResult")
                End If
            End Set
        End Property

        Private _autopsyResultCode As String = String.Empty
        <CellInfo("ICD-10", GroupName:="Autopsy info", Tips:="Enter autopsy result code")>
        Public Property AutopsyResultCode() As String
            Get
                Return _autopsyResultCode
            End Get
            Set(ByVal value As String)
                CanWriteProperty("AutopsyResultCode", True)
                If value Is Nothing Then value = String.Empty
                If Not _autopsyResultCode.Equals(value) Then
                    _autopsyResultCode = value
                    PropertyHasChanged("AutopsyResultCode")
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

            For Each _field As ClassField In ClassSchema(Of DEAD)._fieldList
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

        Public Shared Function BlankDEAD() As DEAD
            Return New DEAD
        End Function

        Public Shared Function NewDEAD(ByVal pLineNo As String) As DEAD
            If KeyDuplicated(pLineNo) Then ExceptionThower.BusinessRuleStop(String.Format(ResStr(ResStrConst.NOACCESS), ResStr("DEAD")))
            Return DataPortal.Create(Of DEAD)(New Criteria(pLineNo))
        End Function

        Public Shared Function NewBO(ByVal ID As String) As DEAD
            Dim pLineNo As String = ID.Trim

            Return NewDEAD(pLineNo)
        End Function

        Public Shared Function GetDEAD(ByVal pLineNo As String) As DEAD
            Return DataPortal.Fetch(Of DEAD)(New Criteria(pLineNo))
        End Function

        Public Shared Function GetBO(ByVal ID As String) As DEAD
            Dim pLineNo As String = ID.Trim

            Return GetDEAD(pLineNo)
        End Function

        Public Shared Sub DeleteDEAD(ByVal pLineNo As String)
            DataPortal.Delete(New Criteria(pLineNo))
        End Sub

        Public Overrides Function Save() As DEAD
            If Not IsDirty Then ExceptionThower.NotDirty(ResStr(ResStrConst.NOTDIRTY))
            If Not IsSavable Then Throw New Csla.Validation.ValidationException(String.Format(ResStr(ResStrConst.INVALID), ResStr("DEAD")))

            Me.ApplyEdit()
            DEADInfoList.InvalidateCache()
            Return MyBase.Save()
        End Function

        Public Function CloneDEAD(ByVal pLineNo As String) As DEAD

            If DEAD.KeyDuplicated(pLineNo) Then ExceptionThower.BusinessRuleStop(ResStr(ResStrConst.CreateAlreadyExists), Me.GetType.ToString.Leaf.Translate)

            Dim cloningDEAD As DEAD = MyBase.Clone
            cloningDEAD._lineNo = pLineNo

            'Todo:Remember to reset status of the new object here 
            cloningDEAD.MarkNew()
            cloningDEAD.ApplyEdit()

            cloningDEAD.ValidationRules.CheckRules()

            Return cloningDEAD
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

                    cm.CommandText = <SqlText>SELECT * FROM pbs_MC_DEAD_<%= _DTB %> WHERE LINE_NO= '<%= criteria._lineNo %>' </SqlText>.Value.Trim

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
            _checkinNo.Text = dr.GetInt32("CHECKIN_NO")
            _patientCode = dr.GetString("PATIENT_CODE").TrimEnd
            _deadTime.Text = dr.GetInt32("DEAD_TIME")
            _deadDate.Text = dr.GetInt32("DEAD_DATE")
            _deadReason = dr.GetString("DEAD_REASON").TrimEnd
            _mainReason = dr.GetString("MAIN_REASON").TrimEnd
            _mainReasonCode = dr.GetString("MAIN_REASON_CODE").TrimEnd
            _autopsy = dr.GetString("AUTOPSY").TrimEnd
            _autopsyResult = dr.GetString("AUTOPSY_RESULT").TrimEnd
            _autopsyResultCode = dr.GetString("AUTOPSY_RESULT_CODE").TrimEnd
            _updated.Text = dr.GetInt32("UPDATED")
            _updatedBy = dr.GetString("UPDATED_BY").TrimEnd

        End Sub

        Private Shared _lockObj As New Object
        Protected Overrides Sub DataPortal_Insert()
            SyncLock _lockObj
                Using ctx = ConnectionFactory.GetDBConnection(True)
                    Using cm = ctx.CreateSQLCommand

                        cm.CommandType = CommandType.StoredProcedure
                        cm.CommandText = String.Format("pbs_MC_DEAD_{0}_Insert", _DTB)

                        cm.Parameters.AddWithValue("@LINE_NO", _lineNo.Trim.ToInteger, ParameterDirection.Output)
                        AddInsertParameters(cm)
                        cm.ExecuteNonQuery()

                        _lineNo = cm.GetReturnIdAsInteger("@LINE_NO")
                    End Using
                End Using
            End SyncLock
        End Sub

        Private Sub AddInsertParameters(ByVal cm As IDBCommand)

            cm.Parameters.AddWithValue("@CHECKIN_NO", _checkinNo.DBValue)
            cm.Parameters.AddWithValue("@PATIENT_CODE", _patientCode.Trim)
            cm.Parameters.AddWithValue("@DEAD_TIME", _deadTime.DBValue)
            cm.Parameters.AddWithValue("@DEAD_DATE", _deadDate.DBValue)
            cm.Parameters.AddWithValue("@DEAD_REASON", _deadReason.Trim)
            cm.Parameters.AddWithValue("@MAIN_REASON", _mainReason.Trim)
            cm.Parameters.AddWithValue("@MAIN_REASON_CODE", _mainReasonCode.Trim)
            cm.Parameters.AddWithValue("@AUTOPSY", _autopsy.Trim)
            cm.Parameters.AddWithValue("@AUTOPSY_RESULT", _autopsyResult.Trim)
            cm.Parameters.AddWithValue("@AUTOPSY_RESULT_CODE", _autopsyResultCode.Trim)
            cm.Parameters.AddWithValue("@UPDATED", _updated.DBValue)
            cm.Parameters.AddWithValue("@UPDATED_BY", _updatedBy.Trim)
        End Sub


        Protected Overrides Sub DataPortal_Update()
            SyncLock _lockObj
                Using ctx = ConnectionFactory.GetDBConnection(True)
                    Using cm = ctx.CreateSQLCommand

                        cm.CommandType = CommandType.StoredProcedure
                        cm.CommandText = String.Format("pbs_MC_DEAD_{0}_Update", _DTB)

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

                    cm.CommandText = <SqlText>DELETE FROM pbs_MC_DEAD_<%= _DTB %> WHERE LINE_NO= '<%= criteria._lineNo %>' </SqlText>.Value.Trim
                    cm.ExecuteNonQuery()

                End Using
            End Using

        End Sub

        Protected Overrides Sub DataPortal_OnDataPortalInvokeComplete(ByVal e As Csla.DataPortalEventArgs)
            If Csla.ApplicationContext.ExecutionLocation = ExecutionLocations.Server Then
                DEADInfoList.InvalidateCache()
            End If
        End Sub


#End Region 'Data Access                           

#Region " Exists "
        Public Shared Function Exists(ByVal pLineNo As String) As Boolean
            Return DEADInfoList.ContainsCode(pLineNo)
        End Function

        Public Shared Function KeyDuplicated(ByVal pLineNo As String) As Boolean
            Dim SqlText = <SqlText>SELECT COUNT(*) FROM pbs_MC_DEAD_<%= Context.CurrentBECode %> WHERE LINE_NO= '<%= pLineNo %>'</SqlText>.Value.Trim
            Return SQLCommander.GetScalarInteger(SqlText) > 0
        End Function
#End Region

#Region " IGenpart "

        Public Function CloneBO(ByVal id As String) As Object Implements Interfaces.IGenPartObject.CloneBO
            Return CloneDEAD(id)
        End Function

        Public Function getBO1(ByVal id As String) As Object Implements Interfaces.IGenPartObject.GetBO
            Return GetBO(id)
        End Function

        Public Function myCommands() As String() Implements Interfaces.IGenPartObject.myCommands
            Return pbs.Helper.Action.StandardReferenceCommands
        End Function

        Public Function myFullName() As String Implements Interfaces.IGenPartObject.myFullName
            Return GetType(DEAD).ToString
        End Function

        Public Function myName() As String Implements Interfaces.IGenPartObject.myName
            Return GetType(DEAD).ToString.Leaf
        End Function

        Public Function myQueryList() As IList Implements Interfaces.IGenPartObject.myQueryList
            Return DEADInfoList.GetDEADInfoList
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