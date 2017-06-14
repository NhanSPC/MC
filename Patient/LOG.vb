Imports pbs.Helper
Imports System.Data

Imports Csla
Imports Csla.Data
Imports Csla.Validation
Imports pbs.BO.DataAnnotations
Imports pbs.BO.Script
Imports pbs.Helper.Interfaces
Imports pbs.BO.BusinessRules

Namespace MC

    <Serializable()> _
    <DB(TableName:="pbs_MC_LOG_XXX")>
    <PhoebusCommand(Desc:="Patient Log")>
    Public Class LOG
        Inherits Csla.BusinessBase(Of LOG)
        Implements Interfaces.IGenPartObject
        Implements IComparable
        Implements IDocLink
        Implements ISupportQueryInfoList
        Implements ISupportCachedLookup

#Region "Property Changed"
        Protected Overrides Sub OnDeserialized(context As Runtime.Serialization.StreamingContext)
            MyBase.OnDeserialized(context)
            AddHandler Me.PropertyChanged, AddressOf BO_PropertyChanged
        End Sub

        Private Sub BO_PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Handles Me.PropertyChanged
            'Select Case e.PropertyName

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

            'End Select

            pbs.BO.Rules.CalculationRules.Calculator(sender, e)
        End Sub
#End Region

#Region " Business Properties and Methods "
        Private _DTB As String = String.Empty

        Private _lineNo As Integer
        <System.ComponentModel.DataObjectField(True, True)> _
        Public ReadOnly Property LineNo() As Integer
            Get
                Return _lineNo
            End Get
        End Property

        Private _logType As String = String.Empty
        <CellInfo(GroupName:="Short Info")>
        Public Property LogType() As String
            Get
                Return _logType
            End Get
            Set(ByVal value As String)
                CanWriteProperty("LogType", True)
                If value Is Nothing Then value = String.Empty
                If Not _logType.Equals(value) Then
                    _logType = value
                    PropertyHasChanged("LogType")
                End If
            End Set
        End Property

        Private _logDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate("T")
        <CellInfo(LinkCode.Calendar, GroupName:="Short Info")>
        <Rule(Required:=True)>
        Public Property LogDate() As String
            Get
                Return _logDate.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("LogDate", True)
                If value Is Nothing Then value = String.Empty
                If Not _logDate.Equals(value) Then
                    _logDate.Text = value
                    PropertyHasChanged("LogDate")
                End If
            End Set
        End Property

        Private _logTime As pbs.Helper.SmartTime = New pbs.Helper.SmartTime("N")
        <CellInfo("Hour", GroupName:="Short Info")>
        Public Property LogTime() As String
            Get
                Return _logTime.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("LogTime", True)
                If value Is Nothing Then value = String.Empty
                If Not _logTime.Equals(value) Then
                    _logTime.InputText = value
                    PropertyHasChanged("LogTime")
                End If
            End Set
        End Property

        Private _logObject As String = String.Empty
        <CellInfo(Hidden:=True)>
        Public Property LogObject() As String
            Get
                Return _logObject
            End Get
            Set(ByVal value As String)
                CanWriteProperty("LogObject", True)
                If value Is Nothing Then value = String.Empty
                If Not _logObject.Equals(value) Then
                    _logObject = value
                    PropertyHasChanged("LogObject")
                End If
            End Set
        End Property

        Private _logMessage As String = String.Empty
        <Rule(Required:=True)>
        <CellInfo(ControlType:=Forms.CtrlType.MemoEdit)>
        Public Property LogMessage() As String
            Get
                Return _logMessage
            End Get
            Set(ByVal value As String)
                CanWriteProperty("LogMessage", True)
                If value Is Nothing Then value = String.Empty
                If Not _logMessage.Equals(value) Then
                    _logMessage = value
                    PropertyHasChanged("LogMessage")
                End If
            End Set
        End Property

        Private _description As String = String.Empty
        <CellInfo(ControlType:=Forms.CtrlType.MemoEdit)>
        Public Property Description() As String
            Get
                Return _description
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Description", True)
                If value Is Nothing Then value = String.Empty
                If Not _description.Equals(value) Then
                    _description = value
                    PropertyHasChanged("Description")
                End If
            End Set
        End Property

        Private _priority As String = String.Empty
        Public Property Priority() As String
            Get
                Return _priority
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Priority", True)
                If value Is Nothing Then value = String.Empty
                If Not _priority.Equals(value) Then
                    _priority = value
                    PropertyHasChanged("Priority")
                End If
            End Set
        End Property

        Private _bphNo As Integer
        <CellInfo(Hidden:=True)>
        Public Property BphNo() As Integer
            Get
                Return _bphNo
            End Get
            Set(ByVal value As Integer)
                CanWriteProperty("BphNo", True)
                'DELETED_ME
                If Not _bphNo.Equals(value) Then
                    _bphNo = value
                    PropertyHasChanged("BphNo")
                End If
            End Set
        End Property

        Private _formDataId As Integer
        <CellInfo(Hidden:=True)>
        Public Property FormDataId() As Integer
            Get
                Return _formDataId
            End Get
            Set(ByVal value As Integer)
                CanWriteProperty("FormDataId", True)
                'DELETED_ME
                If Not _formDataId.Equals(value) Then
                    _formDataId = value
                    PropertyHasChanged("FormDataId")
                End If
            End Set
        End Property

        Private _patientCode As String = String.Empty
        <CellInfo("pbs.BO.MC.PATIENT")>
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

        Private _nextAction As String = String.Empty
        <CellInfo(GroupName:="Actions", ControlType:=Forms.CtrlType.MemoEdit)>
        Public Property NextAction() As String
            Get
                Return _nextAction
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NextAction", True)
                If value Is Nothing Then value = String.Empty
                If Not _nextAction.Equals(value) Then
                    _nextAction = value
                    PropertyHasChanged("NextAction")
                End If
            End Set
        End Property

        Private _taskId As Integer
        <CellInfo(GroupName:="Actions")>
        Public Property TaskId() As Integer
            Get
                Return _taskId
            End Get
            Set(ByVal value As Integer)
                CanWriteProperty("TaskId", True)
                'DELETED_ME
                If Not _taskId.Equals(value) Then
                    _taskId = value
                    PropertyHasChanged("TaskId")
                End If
            End Set
        End Property

        Private _assignedTo As String = String.Empty
        <CellInfo(LinkCode.Employee, GroupName:="Actions")>
        Public Property AssignedTo() As String
            Get
                Return _assignedTo
            End Get
            Set(ByVal value As String)
                CanWriteProperty("AssignedTo", True)
                If value Is Nothing Then value = String.Empty
                If Not _assignedTo.Equals(value) Then
                    _assignedTo = value
                    PropertyHasChanged("AssignedTo")
                End If
            End Set
        End Property

        Private _assignedDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo(LinkCode.Calendar, GroupName:="Actions")>
        Public Property AssignedDate() As String
            Get
                Return _assignedDate.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("AssignedDate", True)
                If value Is Nothing Then value = String.Empty
                If Not _assignedDate.Equals(value) Then
                    _assignedDate.Text = value
                    PropertyHasChanged("AssignedDate")
                End If
            End Set
        End Property

        Private _deadline As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo(LinkCode.Calendar, GroupName:="Actions")>
        Public Property Deadline() As String
            Get
                Return _deadline.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Deadline", True)
                If value Is Nothing Then value = String.Empty
                If Not _deadline.Equals(value) Then
                    _deadline.Text = value
                    PropertyHasChanged("Deadline")
                End If
            End Set
        End Property

        Private _notes As String = String.Empty
        <CellInfo(ControlType:=Forms.CtrlType.MemoEdit)>
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
        <CellInfo(Hidden:=True)>
        Public ReadOnly Property Updated() As String
            Get
                Return _updated.Text
            End Get
        End Property

        Private _updatedBy As String = String.Empty
        <CellInfo(Hidden:=True)>
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
            Dim pLineNo As Integer = ID.Trim.ToInteger
            If _lineNo < pLineNo Then Return -1
            If _lineNo > pLineNo Then Return 1
            Return 0
        End Function

#End Region 'Business Properties and Methods

#Region "Extended"

        Private _ncLg0 As String = String.Empty
        Public Property NcLg0() As String
            Get
                Return _ncLg0
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcLg0", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncLg0.Equals(value) Then
                    _ncLg0 = value
                    PropertyHasChanged("NcLg0")
                End If
            End Set
        End Property

        Private _ncLg1 As String = String.Empty
        Public Property NcLg1() As String
            Get
                Return _ncLg1
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcLg1", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncLg1.Equals(value) Then
                    _ncLg1 = value
                    PropertyHasChanged("NcLg1")
                End If
            End Set
        End Property

        Private _ncLg2 As String = String.Empty
        Public Property NcLg2() As String
            Get
                Return _ncLg2
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcLg2", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncLg2.Equals(value) Then
                    _ncLg2 = value
                    PropertyHasChanged("NcLg2")
                End If
            End Set
        End Property

        Private _ncLg3 As String = String.Empty
        Public Property NcLg3() As String
            Get
                Return _ncLg3
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcLg3", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncLg3.Equals(value) Then
                    _ncLg3 = value
                    PropertyHasChanged("NcLg3")
                End If
            End Set
        End Property

        Private _ncLg4 As String = String.Empty
        Public Property NcLg4() As String
            Get
                Return _ncLg4
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcLg4", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncLg4.Equals(value) Then
                    _ncLg4 = value
                    PropertyHasChanged("NcLg4")
                End If
            End Set
        End Property

        Private _ncLg5 As String = String.Empty
        Public Property NcLg5() As String
            Get
                Return _ncLg5
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcLg5", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncLg5.Equals(value) Then
                    _ncLg5 = value
                    PropertyHasChanged("NcLg5")
                End If
            End Set
        End Property

        Private _ncLg6 As String = String.Empty
        Public Property NcLg6() As String
            Get
                Return _ncLg6
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcLg6", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncLg6.Equals(value) Then
                    _ncLg6 = value
                    PropertyHasChanged("NcLg6")
                End If
            End Set
        End Property

        Private _ncLg7 As String = String.Empty
        Public Property NcLg7() As String
            Get
                Return _ncLg7
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcLg7", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncLg7.Equals(value) Then
                    _ncLg7 = value
                    PropertyHasChanged("NcLg7")
                End If
            End Set
        End Property

        Private _ncLg8 As String = String.Empty
        Public Property NcLg8() As String
            Get
                Return _ncLg8
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcLg8", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncLg8.Equals(value) Then
                    _ncLg8 = value
                    PropertyHasChanged("NcLg8")
                End If
            End Set
        End Property

        Private _ncLg9 As String = String.Empty
        Public Property NcLg9() As String
            Get
                Return _ncLg9
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcLg9", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncLg9.Equals(value) Then
                    _ncLg9 = value
                    PropertyHasChanged("NcLg9")
                End If
            End Set
        End Property

        Private _extDesc0 As String = String.Empty
        <CellInfo(GroupName:="Extended Info")>
        Public Property ExtDesc0() As String
            Get
                Return _extDesc0
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ExtDesc0", True)
                If value Is Nothing Then value = String.Empty
                If Not _extDesc0.Equals(value) Then
                    _extDesc0 = value
                    PropertyHasChanged("ExtDesc0")
                End If
            End Set
        End Property

        Private _extDesc1 As String = String.Empty
        <CellInfo(GroupName:="Extended Info")>
        Public Property ExtDesc1() As String
            Get
                Return _extDesc1
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ExtDesc1", True)
                If value Is Nothing Then value = String.Empty
                If Not _extDesc1.Equals(value) Then
                    _extDesc1 = value
                    PropertyHasChanged("ExtDesc1")
                End If
            End Set
        End Property

        Private _extDesc2 As String = String.Empty
        <CellInfo(GroupName:="Extended Info")>
        Public Property ExtDesc2() As String
            Get
                Return _extDesc2
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ExtDesc2", True)
                If value Is Nothing Then value = String.Empty
                If Not _extDesc2.Equals(value) Then
                    _extDesc2 = value
                    PropertyHasChanged("ExtDesc2")
                End If
            End Set
        End Property

        Private _extDesc3 As String = String.Empty
        <CellInfo(GroupName:="Extended Info")>
        Public Property ExtDesc3() As String
            Get
                Return _extDesc3
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ExtDesc3", True)
                If value Is Nothing Then value = String.Empty
                If Not _extDesc3.Equals(value) Then
                    _extDesc3 = value
                    PropertyHasChanged("ExtDesc3")
                End If
            End Set
        End Property

        Private _extDesc4 As String = String.Empty
        <CellInfo(GroupName:="Extended Info")>
        Public Property ExtDesc4() As String
            Get
                Return _extDesc4
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ExtDesc4", True)
                If value Is Nothing Then value = String.Empty
                If Not _extDesc4.Equals(value) Then
                    _extDesc4 = value
                    PropertyHasChanged("ExtDesc4")
                End If
            End Set
        End Property

        Private _extDate0 As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo(LinkCode.Calendar, GroupName:="Extended Info")>
        Public Property ExtDate0() As String
            Get
                Return _extDate0.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ExtDate0", True)
                If value Is Nothing Then value = String.Empty
                If Not _extDate0.Equals(value) Then
                    _extDate0.Text = value
                    PropertyHasChanged("ExtDate0")
                End If
            End Set
        End Property

        Private _extDate1 As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo(LinkCode.Calendar, GroupName:="Extended Info")>
        Public Property ExtDate1() As String
            Get
                Return _extDate1.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ExtDate1", True)
                If value Is Nothing Then value = String.Empty
                If Not _extDate1.Equals(value) Then
                    _extDate1.Text = value
                    PropertyHasChanged("ExtDate1")
                End If
            End Set
        End Property

        Private _extDate2 As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo(LinkCode.Calendar, GroupName:="Extended Info")>
        Public Property ExtDate2() As String
            Get
                Return _extDate2.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ExtDate2", True)
                If value Is Nothing Then value = String.Empty
                If Not _extDate2.Equals(value) Then
                    _extDate2.Text = value
                    PropertyHasChanged("ExtDate2")
                End If
            End Set
        End Property

        Private _extDate3 As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo(LinkCode.Calendar, GroupName:="Extended Info")>
        Public Property ExtDate3() As String
            Get
                Return _extDate3.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ExtDate3", True)
                If value Is Nothing Then value = String.Empty
                If Not _extDate3.Equals(value) Then
                    _extDate3.Text = value
                    PropertyHasChanged("ExtDate3")
                End If
            End Set
        End Property

        Private _extDate4 As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo(LinkCode.Calendar, GroupName:="Extended Info")>
        Public Property ExtDate4() As String
            Get
                Return _extDate4.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ExtDate4", True)
                If value Is Nothing Then value = String.Empty
                If Not _extDate4.Equals(value) Then
                    _extDate4.Text = value
                    PropertyHasChanged("ExtDate4")
                End If
            End Set
        End Property

        Private _extValue0 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="Extended Info")>
        Public Property ExtValue0() As String
            Get
                Return _extValue0.SignedText
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ExtValue0", True)
                If value Is Nothing Then value = String.Empty
                If Not _extValue0.Equals(value) Then
                    _extValue0.Text = value
                    PropertyHasChanged("ExtValue0")
                End If
            End Set
        End Property

        Private _extValue1 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="Extended Info")>
        Public Property ExtValue1() As String
            Get
                Return _extValue1.SignedText
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ExtValue1", True)
                If value Is Nothing Then value = String.Empty
                If Not _extValue1.Equals(value) Then
                    _extValue1.Text = value
                    PropertyHasChanged("ExtValue1")
                End If
            End Set
        End Property

        Private _extValue2 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="Extended Info")>
        Public Property ExtValue2() As String
            Get
                Return _extValue2.SignedText
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ExtValue2", True)
                If value Is Nothing Then value = String.Empty
                If Not _extValue2.Equals(value) Then
                    _extValue2.Text = value
                    PropertyHasChanged("ExtValue2")
                End If
            End Set
        End Property

        Private _extValue3 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="Extended Info")>
        Public Property ExtValue3() As String
            Get
                Return _extValue3.SignedText
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ExtValue3", True)
                If value Is Nothing Then value = String.Empty
                If Not _extValue3.Equals(value) Then
                    _extValue3.Text = value
                    PropertyHasChanged("ExtValue3")
                End If
            End Set
        End Property

        Private _extValue4 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="Extended Info")>
        Public Property ExtValue4() As String
            Get
                Return _extValue4.SignedText
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ExtValue4", True)
                If value Is Nothing Then value = String.Empty
                If Not _extValue4.Equals(value) Then
                    _extValue4.Text = value
                    PropertyHasChanged("ExtValue4")
                End If
            End Set
        End Property

#End Region

#Region "Validation Rules"

        Private Sub AddSharedCommonRules()
            'Sample simple custom rule
            'ValidationRules.AddRule(AddressOf LDInfo.ContainsValidPeriod, "Period", 1)           

            'Sample dependent property. when check one , check the other as well
            'ValidationRules.AddDependantProperty("AccntCode", "AnalT0")
        End Sub

        Protected Overrides Sub AddBusinessRules()
            AddSharedCommonRules()

            For Each _field As ClassField In ClassSchema(Of LOG)._fieldList
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

        Public Shared Function BlankLOG() As LOG
            Return New LOG
        End Function

        Public Shared Function NewLOG(ByVal pLineNo As String) As LOG
            Return DataPortal.Create(Of LOG)(New Criteria(pLineNo))
        End Function

        Public Shared Function NewBO(ByVal ID As String) As LOG
            Dim pLineNo As Integer = ID.Trim.ToInteger

            Return NewLOG(pLineNo)
        End Function

        Public Shared Function GetLOG(ByVal pLineNo As String) As LOG
            Return DataPortal.Fetch(Of LOG)(New Criteria(pLineNo))
        End Function

        Public Shared Function GetBO(ByVal ID As String) As LOG
            Dim pLineNo As Integer = ID.Trim.ToInteger

            Return GetLOG(pLineNo)
        End Function

        Public Shared Sub DeleteLOG(ByVal pLineNo As String)
            DataPortal.Delete(New Criteria(pLineNo))
            'If Not Context.IsBatchSavingMode Then LOGInfoList.RemoveInfo(pLineNo)
        End Sub

        Public Overrides Function Save() As LOG
            If Not IsDirty Then ExceptionThower.NotDirty(ResStr(ResStrConst.NOTDIRTY))
            If Not IsSavable Then Throw New Csla.Validation.ValidationException(String.Format(ResStr(ResStrConst.INVALID), ResStr("LOG")))

            Me.ApplyEdit()

            Dim ret = MyBase.Save()
            If Not Context.IsBatchSavingMode Then LOGInfoList.InvalidateCache()
            Return ret
        End Function

        Public Function CloneLOG(ByVal pLineNo As String) As LOG

            If LOG.KeyDuplicated(pLineNo) Then ExceptionThower.BusinessRuleStop(ResStr(ResStrConst.CreateAlreadyExists), Me.GetType.ToString.Leaf.Translate)

            Dim cloning As LOG = MyBase.Clone
            cloning._lineNo = pLineNo
            cloning._DTB = Context.CurrentBECode
            'Todo:Remember to reset status of the new object here 
            cloning.MarkNew()
            cloning.ApplyEdit()

            cloning.ValidationRules.CheckRules()

            Return cloning
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

                    cm.CommandText = <SqlText>SELECT * FROM pbs_MC_LOG_<%= _DTB %> WHERE LINE_NO= <%= criteria._lineNo %></SqlText>.Value.Trim

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
            _logType = dr.GetString("LOG_TYPE").TrimEnd
            _logDate.Text = dr.GetInt32("LOG_DATE")
            _logTime.Text = dr.GetInt32("LOG_TIME")
            _logObject = dr.GetString("LOG_OBJECT").TrimEnd
            _logMessage = dr.GetString("LOG_MESSAGE").TrimEnd
            _description = dr.GetString("DESCRIPTION").TrimEnd
            _priority = dr.GetString("PRIORITY").TrimEnd
            _bphNo = dr.GetInt32("BPH_NO")
            _formDataId = dr.GetInt32("FORM_DATA_ID")
          
            _patientCode = dr.GetString("PATIENT_CODE").TrimEnd
            _nextAction = dr.GetString("NEXT_ACTION").TrimEnd
            _taskId = dr.GetInt32("TASK_ID")
            _assignedTo = dr.GetString("ASSIGNED_TO").TrimEnd
            _assignedDate.Text = dr.GetInt32("ASSIGNED_DATE")
            _deadline.Text = dr.GetInt32("DEADLINE")
            _notes = dr.GetString("NOTES").TrimEnd
            _updated.Text = dr.GetInt32("UPDATED")
            _updatedBy = dr.GetString("UPDATED_BY").TrimEnd


            _ncLg0 = dr.GetString("NC_LG0").TrimEnd
            _ncLg1 = dr.GetString("NC_LG1").TrimEnd
            _ncLg2 = dr.GetString("NC_LG2").TrimEnd
            _ncLg3 = dr.GetString("NC_LG3").TrimEnd
            _ncLg4 = dr.GetString("NC_LG4").TrimEnd
            _ncLg5 = dr.GetString("NC_LG5").TrimEnd
            _ncLg6 = dr.GetString("NC_LG6").TrimEnd
            _ncLg7 = dr.GetString("NC_LG7").TrimEnd
            _ncLg8 = dr.GetString("NC_LG8").TrimEnd
            _ncLg9 = dr.GetString("NC_LG9").TrimEnd

            _extDesc0 = dr.GetString("EXT_DESC0").TrimEnd
            _extDesc1 = dr.GetString("EXT_DESC1").TrimEnd
            _extDesc2 = dr.GetString("EXT_DESC2").TrimEnd
            _extDesc3 = dr.GetString("EXT_DESC3").TrimEnd
            _extDesc4 = dr.GetString("EXT_DESC4").TrimEnd
            _extDate0.Text = dr.GetInt32("EXT_DATE0")
            _extDate1.Text = dr.GetInt32("EXT_DATE1")
            _extDate2.Text = dr.GetInt32("EXT_DATE2")
            _extDate3.Text = dr.GetInt32("EXT_DATE3")
            _extDate4.Text = dr.GetInt32("EXT_DATE4")
            _extValue0.Text = dr.GetDecimal("EXT_VALUE0")
            _extValue1.Text = dr.GetDecimal("EXT_VALUE1")
            _extValue2.Text = dr.GetDecimal("EXT_VALUE2")
            _extValue3.Text = dr.GetDecimal("EXT_VALUE3")
            _extValue4.Text = dr.GetDecimal("EXT_VALUE4")

        End Sub

        Private Shared _lockObj As New Object
        Protected Overrides Sub DataPortal_Insert()
            SyncLock _lockObj
                Using ctx = ConnectionFactory.GetDBConnection(True)
                    Using cm = ctx.CreateSQLCommand

                        cm.CommandType = CommandType.StoredProcedure
                        cm.CommandText = String.Format("pbs_MCLOG_{0}_Insert", _DTB)

                        cm.Parameters.AddWithValue("@LINE_NO", _lineNo, ParameterDirection.Output)

                        AddInsertParameters(cm)
                        cm.ExecuteNonQuery()

                        _lineNo = cm.Parameters("@LINE_NO").Value.ToString.ToInteger
                    End Using

                End Using
            End SyncLock
        End Sub

        Private Sub AddInsertParameters(ByVal cm As IDBCommand)

            cm.Parameters.AddWithValue("@LOG_TYPE", _logType.Trim)
            cm.Parameters.AddWithValue("@LOG_DATE", _logDate.DBValue)
            cm.Parameters.AddWithValue("@LOG_TIME", _logTime.DBValue)
            cm.Parameters.AddWithValue("@LOG_OBJECT", _logObject.Trim)
            cm.Parameters.AddWithValue("@LOG_MESSAGE", _logMessage.Trim)
            cm.Parameters.AddWithValue("@DESCRIPTION", _description.Trim)
            cm.Parameters.AddWithValue("@PRIORITY", _priority.Trim)
            cm.Parameters.AddWithValue("@BPH_NO", _bphNo)
            cm.Parameters.AddWithValue("@FORM_DATA_ID", _formDataId)

            cm.Parameters.AddWithValue("@PATIENT_CODE", _patientCode.Trim)
            cm.Parameters.AddWithValue("@NEXT_ACTION", _nextAction.Trim)
            cm.Parameters.AddWithValue("@TASK_ID", _taskId)
            cm.Parameters.AddWithValue("@ASSIGNED_TO", _assignedTo.Trim)
            cm.Parameters.AddWithValue("@ASSIGNED_DATE", _assignedDate.DBValue)
            cm.Parameters.AddWithValue("@DEADLINE", _deadline.DBValue)
            cm.Parameters.AddWithValue("@NOTES", _notes.Trim)
            cm.Parameters.AddWithValue("@UPDATED", ToDay.ToSunDate)
            cm.Parameters.AddWithValue("@UPDATED_BY", Context.CurrentUserCode)


            cm.Parameters.AddWithValue("@NC_LG0", _ncLg0.Trim)
            cm.Parameters.AddWithValue("@NC_LG1", _ncLg1.Trim)
            cm.Parameters.AddWithValue("@NC_LG2", _ncLg2.Trim)
            cm.Parameters.AddWithValue("@NC_LG3", _ncLg3.Trim)
            cm.Parameters.AddWithValue("@NC_LG4", _ncLg4.Trim)
            cm.Parameters.AddWithValue("@NC_LG5", _ncLg5.Trim)
            cm.Parameters.AddWithValue("@NC_LG6", _ncLg6.Trim)
            cm.Parameters.AddWithValue("@NC_LG7", _ncLg7.Trim)
            cm.Parameters.AddWithValue("@NC_LG8", _ncLg8.Trim)
            cm.Parameters.AddWithValue("@NC_LG9", _ncLg9.Trim)
            cm.Parameters.AddWithValue("@EXT_DESC0", _extDesc0.Trim)
            cm.Parameters.AddWithValue("@EXT_DESC1", _extDesc1.Trim)
            cm.Parameters.AddWithValue("@EXT_DESC2", _extDesc2.Trim)
            cm.Parameters.AddWithValue("@EXT_DESC3", _extDesc3.Trim)
            cm.Parameters.AddWithValue("@EXT_DESC4", _extDesc4.Trim)
            cm.Parameters.AddWithValue("@EXT_DATE0", _extDate0.DBValue)
            cm.Parameters.AddWithValue("@EXT_DATE1", _extDate1.DBValue)
            cm.Parameters.AddWithValue("@EXT_DATE2", _extDate2.DBValue)
            cm.Parameters.AddWithValue("@EXT_DATE3", _extDate3.DBValue)
            cm.Parameters.AddWithValue("@EXT_DATE4", _extDate4.DBValue)
            cm.Parameters.AddWithValue("@EXT_VALUE0", _extValue0.DBValue)
            cm.Parameters.AddWithValue("@EXT_VALUE1", _extValue1.DBValue)
            cm.Parameters.AddWithValue("@EXT_VALUE2", _extValue2.DBValue)
            cm.Parameters.AddWithValue("@EXT_VALUE3", _extValue3.DBValue)
            cm.Parameters.AddWithValue("@EXT_VALUE4", _extValue4.DBValue)

        End Sub

        Protected Overrides Sub DataPortal_Update()
            Using ctx = ConnectionFactory.GetDBConnection(True)
                Using cm = ctx.CreateSQLCommand

                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = String.Format("pbs_MCLOG_{0}_Update", _DTB)

                    cm.Parameters.AddWithValue("@LINE_NO", _lineNo)

                    AddInsertParameters(cm)
                    cm.ExecuteNonQuery()

                End Using


            End Using
        End Sub

        Protected Overrides Sub DataPortal_DeleteSelf()
            DataPortal_Delete(New Criteria(_lineNo))
        End Sub

        Private Overloads Sub DataPortal_Delete(ByVal criteria As Criteria)
           Using ctx = ConnectionFactory.GetDBConnection(True)
                Using cm = ctx.CreateSQLCommand

                    cm.CommandText = <SqlText>DELETE FROM pbs_MC_LOG_<%= _DTB %> WHERE LINE_NO= <%= criteria._lineNo %></SqlText>.Value.Trim
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
            Dim theLine As Integer = pLineNo.ToInteger
            If theLine <= 0 Then Return False
            Dim SqlText = <SqlText>SELECT COUNT(*) FROM pbs_MC_LOG_<%= Context.CurrentBECode %> WHERE LINE_NO= <%= theLine %></SqlText>.Value.Trim
            Return SQLCommander.GetScalarInteger(SqlText) > 0

        End Function
#End Region

#Region " IGenpart "

        Public Function CloneBO(ByVal id As String) As Object Implements Interfaces.IGenPartObject.CloneBO
            Return CloneLOG(id.ToInteger)
        End Function

        Public Function getBO1(ByVal id As String) As Object Implements Interfaces.IGenPartObject.GetBO
            Return GetBO(id)
        End Function

        Public Function myCommands() As String() Implements Interfaces.IGenPartObject.myCommands
            Return pbs.Helper.Action.StandardReferenceCommands
        End Function

        Public Function myFullName() As String Implements Interfaces.IGenPartObject.myFullName
            Return GetType(LOG).ToString
        End Function

        Public Function myName() As String Implements Interfaces.IGenPartObject.myName
            Return GetType(LOG).ToString.Leaf
        End Function

        Public Function myQueryList() As IList Implements Interfaces.IGenPartObject.myQueryList
            Return LOGInfoList.GetLOGInfoList
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

#Region "ISupportQueryInfoList"
        Public Function GetLOGInfoList(pFilters As Dictionary(Of String, String)) As IEnumerable Implements ISupportQueryInfoList.GetInfoList
            Return LOGInfoList.GetLOGInfoList(pFilters)
        End Function
#End Region

#Region "Isupport Cached Lookup"
        Public Function LookupFor(KeyValue As String, ReturnField As String) As String Implements ISupportCachedLookup.LookupFor
            Dim info As LOGInfo = Nothing
            If LOGInfoList.ContainsCode(KeyValue.ToInteger, info) Then
                If String.IsNullOrEmpty(ReturnField) Then
                    Return info.Description
                ElseIf ReturnField.StartsWith("=") Then
                    Return PhoebusAPI.Evaluate(ReturnField.RegExpReplace("^=", String.Empty), info)
                Else
                    Return pbs.Helper.DataMapper.GetPropertyValue(info, ReturnField, String.Empty)
                End If
            End If
            Return String.Empty
        End Function
#End Region

        '#Region "Special user table / Report Tags"

        '        ''' <summary>
        '        ''' Return candidate status from log records 
        '        ''' If there are many log records for one log type. Only the latest log is counted.
        '        ''' Usage : Get a List of all candidate, which confirm for registration at a given date
        '        ''' </summary>
        '        ''' <param name="arg"></param>
        '        ''' <returns></returns>
        '        ''' <remarks></remarks>
        '        Public Shared Function GetLatestCandidateLog(arg As pbsCmdArgs) As DataTable
        '            Dim filteredList = LOGInfoList.GetLOGInfoList(arg.GetDictionary)

        '            Dim groups = From info In filteredList Group By info.CandidateId Into Group

        '            Dim ret = New List(Of LOGInfo)

        '            For Each gritm In groups
        '                Dim latest = (From itm In gritm.Group Order By itm.LogDate Descending, itm.LineNo Descending).FirstOrDefault
        '                If latest IsNot Nothing Then ret.Add(latest)
        '            Next

        '            Return List2Table.CreateTableFromList(ret)

        '        End Function

        '        Public Shared Function GetLatestStudentLog(arg As pbsCmdArgs) As DataTable
        '            Dim filteredList = LOGInfoList.GetLOGInfoList(arg.GetDictionary)

        '            Dim groups = From info In filteredList Group By info.PatientCode Into Group

        '            Dim ret = New List(Of LOGInfo)

        '            For Each gritm In groups
        '                Dim latest = (From itm In gritm.Group Order By itm.LogDate Descending, itm.LineNo Descending).FirstOrDefault
        '                If latest IsNot Nothing Then ret.Add(latest)
        '            Next

        '            Return List2Table.CreateTableFromList(ret)

        '        End Function

        '        ''' <summary>
        '        ''' Return the latest log record of a student or Candidate or Lead at a date. 
        '        ''' </summary>
        '        ''' <param name="pDate">the checking Date</param>
        '        ''' <param name="pPatientCode"></param>
        '        ''' <param name="pReturnColumnName">If empty. Return the LineNo of log record</param>
        '        ''' <returns></returns>
        '        ''' <remarks></remarks>
        '        Public Shared Function GetStudentLog(pDate As String, pLogType As String, pPatientCode As String, Optional pReturnColumnName As String = "") As String
        '            If String.IsNullOrEmpty(pPatientCode) Then Return String.Empty

        '            Dim Logs = LOGInfoList.GetStudentLogs(pDate, pLogType, pPatientCode)
        '            Dim latest = (From itm In Logs Order By itm.LogDate Descending, itm.LineNo Descending).FirstOrDefault
        '            If latest IsNot Nothing Then
        '                If String.IsNullOrEmpty(pReturnColumnName) Then
        '                    Return pbs.Helper.DataMapper.GetPropertyValue(latest, pReturnColumnName, latest.LineNo)
        '                Else
        '                    Return latest.LineNo
        '                End If
        '            Else
        '                Return String.Empty
        '            End If
        '        End Function

        '        Public Shared Function GetCandidateLog(pDate As String, pLogType As String, pCandidate As String, Optional pReturnColumnName As String = "") As String
        '            If pCandidate.ToInteger = 0 Then Return String.Empty

        '            Dim Logs = LOGInfoList.GetCandidateLogs(pDate, pLogType, pCandidate)
        '            Dim latest = (From itm In Logs Order By itm.LogDate Descending, itm.LineNo Descending).FirstOrDefault
        '            If latest IsNot Nothing Then
        '                If String.IsNullOrEmpty(pReturnColumnName) Then
        '                    Return pbs.Helper.DataMapper.GetPropertyValue(latest, pReturnColumnName, latest.LineNo)
        '                Else
        '                    Return latest.LineNo
        '                End If
        '            Else
        '                Return String.Empty
        '            End If
        '        End Function

        '        Public Shared Function GetLeadLog(pDate As String, pLogType As String, pLeadId As String, Optional pReturnColumnName As String = "") As String

        '            If pLeadId.ToInteger = 0 Then Return String.Empty

        '            Dim Logs = LOGInfoList.GetLeadLogs(pDate, pLogType, pLeadId)
        '            Dim latest = (From itm In Logs Order By itm.LogDate Descending, itm.LineNo Descending).FirstOrDefault
        '            If latest IsNot Nothing Then
        '                If String.IsNullOrEmpty(pReturnColumnName) Then
        '                    Return pbs.Helper.DataMapper.GetPropertyValue(latest, pReturnColumnName, latest.LineNo)
        '                Else
        '                    Return latest.LineNo
        '                End If
        '            Else
        '                Return String.Empty
        '            End If
        '        End Function
        '#End Region


    End Class

End Namespace