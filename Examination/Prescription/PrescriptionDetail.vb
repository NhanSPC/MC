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
    <DB(TableName:="pbs_MC_PRESCRIPTION_DETAIL_{XXX}")>
    Public Class PrescriptionDetail
        Inherits Csla.BusinessBase(Of PrescriptionDetail)
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

                Case "ItemCode"
                    For Each itm In pbs.BO.PB.IRInfoList.GetIRInfoList
                        If ItemCode = itm.ItemCode Then
                            ItemName = itm.Descriptn

                        End If
                    Next

                Case "Morning", "Noon", "Afternoon", "Evening", "BeforeBreakfast", "BeforeLunch", "BeforeSupper", "BeforeDinner"
                    _instruction = PrescriptionInstruction()


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

        Friend Function PrescriptionInstruction()
            'Morning
            Dim bBreakfast As String
            If BeforeBreakfast AndAlso Not String.IsNullOrEmpty(_morning.Text) Then
                bBreakfast = " Trước ăn"
            Else
                bBreakfast = String.Empty
                _beforeBreakfast = "N"
            End If

            Dim strMorning As String = If(String.IsNullOrEmpty(_morning.Text), String.Empty, String.Format("Sáng {0} viên", _morning))

            'Noon
            Dim bLunch As String
            If BeforeLunch AndAlso Not String.IsNullOrEmpty(_noon.Text) Then
                bLunch = " Trước ăn"
            Else
                bLunch = String.Empty
                _beforeLunch = "N"
            End If

            Dim strNoon As String = If(String.IsNullOrEmpty(_noon.Text), String.Empty, String.Format("Trưa {0} viên", _noon))

            'Afternoon
            Dim bSupper As String
            If BeforeLunch AndAlso Not String.IsNullOrEmpty(_afternoon.Text) Then
                bSupper = " Trước ăn"
            Else
                bSupper = String.Empty
                _beforeSupper = "N"
            End If

            Dim strAfternoon As String = If(String.IsNullOrEmpty(_afternoon.Text), String.Empty, String.Format("Chiều {0} viên", _afternoon))

            'Evening
            Dim bDinner As String = If(BeforeDinner, " Trước ăn", String.Empty)
            If BeforeDinner AndAlso Not String.IsNullOrEmpty(_evening.Text) Then
                bDinner = " Trước ăn"
            Else
                bDinner = String.Empty
                _beforeDinner = "N"
            End If
            Dim strEvening As String = If(String.IsNullOrEmpty(_evening.Text), String.Empty, String.Format("Tối {0} viên", _evening))

            Return String.Format("{0}{1}, {2}{3}, {4}{5}, {6}{7}.", strMorning, bBreakfast, strNoon, bLunch, strAfternoon, bSupper, strEvening, bDinner)

        End Function

#Region " Business Properties and Methods "
        Friend _DTB As String = String.Empty


        Friend _lineNo As String = String.Empty
        <System.ComponentModel.DataObjectField(True, True)>
        Public ReadOnly Property LineNo() As String
            Get
                Return _lineNo
            End Get
        End Property

        Private _prescriptionNo As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        <CellInfo("pbs.BO.MC.PRESCRIPTION", Tips:="Enter prescription number")>
        Public Property PrescriptionNo() As String
            Get
                Return _prescriptionNo.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("PrescriptionNo", True)
                If value Is Nothing Then value = String.Empty
                If Not _prescriptionNo.Equals(value) Then
                    _prescriptionNo.Text = value
                    PropertyHasChanged("PrescriptionNo")
                End If
            End Set
        End Property

        Private _itemCode As String = String.Empty
        <CellInfo("pbs.BO.PB.IR", Tips:="Enter medicine code")>
        <Rule(Required:=True)>
        Public Property ItemCode() As String
            Get
                Return _itemCode
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ItemCode", True)
                If value Is Nothing Then value = String.Empty
                If Not _itemCode.Equals(value) Then
                    _itemCode = value
                    PropertyHasChanged("ItemCode")
                End If
            End Set
        End Property

        Private _itemName As String = String.Empty
        <CellInfo(Tips:="Enter medicine name")>
        Public Property ItemName() As String
            Get
                Return _itemName
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ItemName", True)
                If value Is Nothing Then value = String.Empty
                If Not _itemName.Equals(value) Then
                    _itemName = value
                    PropertyHasChanged("ItemName")
                End If
            End Set
        End Property

        Private _activeIngrendient As String = String.Empty
        <CellInfo(Tips:="Active ingrendient of medicine ")>
        Public Property ActiveIngrendient() As String
            Get
                Return _activeIngrendient
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ActiveIngrendient", True)
                If value Is Nothing Then value = String.Empty
                If Not _activeIngrendient.Equals(value) Then
                    _activeIngrendient = value
                    PropertyHasChanged("ActiveIngrendient")
                End If
            End Set
        End Property

        Private _unit As String = String.Empty
        <CellInfo(Tips:="Unit")>
        Public Property Unit() As String
            Get
                Return _unit
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Unit", True)
                If value Is Nothing Then value = String.Empty
                If Not _unit.Equals(value) Then
                    _unit = value
                    PropertyHasChanged("Unit")
                End If
            End Set
        End Property

        Private _dateOfIssue As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        <CellInfo(Tips:="Enter date of issue")>
        Public Property DateOfIssue() As String
            Get
                Return _dateOfIssue.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("DateOfIssue", True)
                If value Is Nothing Then value = String.Empty
                If Not _dateOfIssue.Equals(value) Then
                    _dateOfIssue.Text = value
                    PropertyHasChanged("DateOfIssue")
                End If
            End Set
        End Property

        Private _morning As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        <CellInfo(Tips:="Enter amound of medicine used in morning")>
        Public Property Morning() As String
            Get
                Return _morning.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Morning", True)
                If value Is Nothing Then value = String.Empty
                If Not _morning.Equals(value) Then
                    _morning.Text = value
                    PropertyHasChanged("Morning")
                End If
            End Set
        End Property

        Private _noon As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        <CellInfo(Tips:="Enter amound of medicine used in noon")>
        Public Property Noon() As String
            Get
                Return _noon.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Noon", True)
                If value Is Nothing Then value = String.Empty
                If Not _noon.Equals(value) Then
                    _noon.Text = value
                    PropertyHasChanged("Noon")
                End If
            End Set
        End Property

        Private _afternoon As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        <CellInfo(Tips:="Enter amound of medicine used in afternoon")>
        Public Property Afternoon() As String
            Get
                Return _afternoon.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Afternoon", True)
                If value Is Nothing Then value = String.Empty
                If Not _afternoon.Equals(value) Then
                    _afternoon.Text = value
                    PropertyHasChanged("Afternoon")
                End If
            End Set
        End Property

        Private _evening As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        <CellInfo(Tips:="Enter amound of medicine used in evening")>
        Public Property Evening() As String
            Get
                Return _evening.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Evening", True)
                If value Is Nothing Then value = String.Empty
                If Not _evening.Equals(value) Then
                    _evening.Text = value
                    PropertyHasChanged("Evening")
                End If
            End Set
        End Property

        Private _quantity As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        <CellInfo(Tips:="Total of drug")>
        Public Property Quantity() As String
            Get
                Return _quantity.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Quantity", True)
                If value Is Nothing Then value = String.Empty
                If Not _quantity.Equals(value) Then
                    _quantity.Text = value
                    PropertyHasChanged("Quantity")
                End If
            End Set
        End Property

        Private _beforeBreakfast As String = String.Empty
        <CellInfo(Tips:="Check if this drug is used before breakfast", ControlType:=Forms.CtrlType.ToggleSwitch)>
        Public Property BeforeBreakfast() As Boolean
            Get
                Return _beforeBreakfast.ToBoolean
            End Get
            Set(ByVal value As Boolean)
                CanWriteProperty("BeforeBreakfast", True)
                If Not _beforeBreakfast.Equals(value) Then
                    _beforeBreakfast = If(value, "Y", "N")
                End If
                PropertyHasChanged("BeforeBreakfast")
            End Set
        End Property

        Private _beforeLunch As String = String.Empty
        <CellInfo(Tips:="Check if this drug is used before lunch", ControlType:=Forms.CtrlType.ToggleSwitch)>
        Public Property BeforeLunch() As Boolean
            Get
                Return _beforeLunch.ToBoolean
            End Get
            Set(ByVal value As Boolean)
                CanWriteProperty("BeforeLunch", True)
                If Not _beforeLunch.Equals(value) Then
                    _beforeLunch = If(value, "Y", "N")
                    PropertyHasChanged("BeforeLunch")
                End If
            End Set
        End Property

        Private _beforeSupper As String = String.Empty
        <CellInfo(Tips:="Check if this drug is used before supper", ControlType:=Forms.CtrlType.ToggleSwitch)>
        Public Property BeforeSupper() As Boolean
            Get
                Return _beforeSupper.ToBoolean
            End Get
            Set(ByVal value As Boolean)
                CanWriteProperty("BeforeSupper", True)
                If Not _beforeSupper.Equals(value) Then
                    _beforeSupper = If(value, "Y", "N")
                    PropertyHasChanged("BeforeSupper")
                End If
            End Set
        End Property

        Private _beforeDinner As String = String.Empty
        <CellInfo(Tips:="Check if this drug is used before dinner", ControlType:=Forms.CtrlType.ToggleSwitch)>
        Public Property BeforeDinner() As Boolean
            Get
                Return _beforeDinner.ToBoolean
            End Get
            Set(ByVal value As Boolean)
                CanWriteProperty("BeforeDinner", True)
                If Not _beforeDinner.Equals(value) Then
                    _beforeDinner = If(value, "Y", "N")
                    PropertyHasChanged("BeforeDinner")
                End If
            End Set
        End Property

        Private _instruction As String = String.Empty
        <CellInfo(Tips:="Drug instruction")>
        Public Property Instruction() As String
            Get
                Return _instruction
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Instruction", True)
                If value Is Nothing Then value = String.Empty
                If Not _instruction.Equals(value) Then
                    _instruction = value
                    PropertyHasChanged("Instruction")
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
        Sub CheckRules()
            ValidationRules.CheckRules()
        End Sub
        Private Sub AddSharedCommonRules()
            'Sample simple custom rule
            'ValidationRules.AddRule(AddressOf LDInfo.ContainsValidPeriod, "Period", 1)           

            'Sample dependent property. when check one , check the other as well
            'ValidationRules.AddDependantProperty("AccntCode", "AnalT0")
        End Sub

        Protected Overrides Sub AddBusinessRules()
            AddSharedCommonRules()

            For Each _field As ClassField In ClassSchema(Of PrescriptionDetail)._fieldList
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

        Public Shared Function BlankPrescriptionDetail() As PrescriptionDetail
            Return New PrescriptionDetail
        End Function

        Public Shared Function NewPrescriptionDetail(ByVal pLineNo As String) As PrescriptionDetail
            'If KeyDuplicated(pLineNo) Then ExceptionThower.BusinessRuleStop(String.Format(ResStr(ResStrConst.NOACCESS), ResStr("PrescriptionDetail")))
            Return DataPortal.Create(Of PrescriptionDetail)(New Criteria(pLineNo))
        End Function

        Public Shared Function NewBO(ByVal ID As String) As PrescriptionDetail
            Dim pLineNo As String = ID.Trim

            Return NewPrescriptionDetail(pLineNo)
        End Function

        Public Shared Function GetPrescriptionDetail(ByVal pLineNo As String) As PrescriptionDetail
            Return DataPortal.Fetch(Of PrescriptionDetail)(New Criteria(pLineNo))
        End Function

        Public Shared Function GetBO(ByVal ID As String) As PrescriptionDetail
            Dim pLineNo As String = ID.Trim

            Return GetPrescriptionDetail(pLineNo)
        End Function

        Public Shared Sub DeletePrescriptionDetail(ByVal pLineNo As String)
            DataPortal.Delete(New Criteria(pLineNo))
        End Sub

        Public Overrides Function Save() As PrescriptionDetail
            If Not IsDirty Then ExceptionThower.NotDirty(ResStr(ResStrConst.NOTDIRTY))
            If Not IsSavable Then Throw New Csla.Validation.ValidationException(String.Format(ResStr(ResStrConst.INVALID), ResStr("PrescriptionDetail")))

            Me.ApplyEdit()
            PrescriptionDetailInfoList.InvalidateCache()
            Return MyBase.Save()
        End Function

        Public Function ClonePrescriptionDetail(ByVal pLineNo As String) As PrescriptionDetail

            If PrescriptionDetail.KeyDuplicated(pLineNo) Then ExceptionThower.BusinessRuleStop(ResStr(ResStrConst.CreateAlreadyExists), Me.GetType.ToString.Leaf.Translate)

            Dim cloningPrescriptionDetail As PrescriptionDetail = MyBase.Clone
            cloningPrescriptionDetail._lineNo = pLineNo

            'Todo:Remember to reset status of the new object here 
            cloningPrescriptionDetail.MarkNew()
            cloningPrescriptionDetail.ApplyEdit()

            cloningPrescriptionDetail.ValidationRules.CheckRules()

            Return cloningPrescriptionDetail
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

                    cm.CommandText = <SqlText>SELECT * FROM pbs_MC_PRESCRIPTION_DETAIL_<%= _DTB %> WHERE LINE_NO = '<%= criteria._lineNo %>' 
                                     </SqlText>.Value.Trim

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
            _prescriptionNo.Text = dr.GetInt32("PRESCRIPTION_NO")
            _itemCode = dr.GetString("ITEM_CODE").TrimEnd
            _itemName = dr.GetString("ITEM_NAME").TrimEnd
            _activeIngrendient = dr.GetString("ACTIVE_INGRENDIENT").TrimEnd
            _unit = dr.GetString("UNIT").TrimEnd
            _dateOfIssue.Text = dr.GetInt32("DATE_OF_ISSUE")
            _morning.Text = dr.GetInt32("MORNING")
            _noon.Text = dr.GetInt32("NOON")
            _afternoon.Text = dr.GetInt32("AFTERNOON")
            _evening.Text = dr.GetInt32("EVENING")
            _quantity.Text = dr.GetInt32("QUANTITY")
            _beforeBreakfast = dr.GetString("BEFORE_BREAKFAST").TrimEnd
            _beforeLunch = dr.GetString("BEFORE_LUNCH").TrimEnd
            _beforeSupper = dr.GetString("BEFORE_SUPPER").TrimEnd
            _beforeDinner = dr.GetString("BEFORE_DINNER").TrimEnd
            _instruction = dr.GetString("INSTRUCTION").TrimEnd
            _updated.Text = dr.GetInt32("UPDATED")
            _updatedBy = dr.GetString("UPDATED_BY").TrimEnd

        End Sub

        Private Shared _lockObj As New Object
        Protected Overrides Sub DataPortal_Insert()
            SyncLock _lockObj
                Using ctx = ConnectionFactory.GetDBConnection(True)


                    Insert(ctx)
                End Using
            End SyncLock
        End Sub

        Private Sub AddInsertParameters(ByVal cm As IDBCommand)

            cm.Parameters.AddWithValue("@PRESCRIPTION_NO", _prescriptionNo.DBValue)
            cm.Parameters.AddWithValue("@ITEM_CODE", _itemCode.Trim)
            cm.Parameters.AddWithValue("@ITEM_NAME", _itemName.Trim)
            cm.Parameters.AddWithValue("@ACTIVE_INGRENDIENT", _activeIngrendient.Trim)
            cm.Parameters.AddWithValue("@UNIT", _unit.Trim)
            cm.Parameters.AddWithValue("@DATE_OF_ISSUE", _dateOfIssue.DBValue)
            cm.Parameters.AddWithValue("@MORNING", _morning.DBValue)
            cm.Parameters.AddWithValue("@NOON", _noon.DBValue)
            cm.Parameters.AddWithValue("@AFTERNOON", _afternoon.DBValue)
            cm.Parameters.AddWithValue("@EVENING", _evening.DBValue)
            cm.Parameters.AddWithValue("@QUANTITY", _quantity.DBValue)
            cm.Parameters.AddWithValue("@BEFORE_BREAKFAST", _beforeBreakfast.Trim)
            cm.Parameters.AddWithValue("@BEFORE_LUNCH", _beforeLunch.Trim)
            cm.Parameters.AddWithValue("@BEFORE_SUPPER", _beforeSupper.Trim)
            cm.Parameters.AddWithValue("@BEFORE_DINNER", _beforeDinner.Trim)
            cm.Parameters.AddWithValue("@INSTRUCTION", _instruction.Trim)
            cm.Parameters.AddWithValue("@UPDATED", ToDay.ToSunDate)
            cm.Parameters.AddWithValue("@UPDATED_BY", Context.CurrentUserCode)
        End Sub


        Protected Overrides Sub DataPortal_Update()
            SyncLock _lockObj
                Using ctx = ConnectionFactory.GetDBConnection(True)
                 
                    Update(ctx)
                    End Using
            End SyncLock
        End Sub

        Protected Overrides Sub DataPortal_DeleteSelf()
            DataPortal_Delete(New Criteria(_lineNo))
        End Sub

        Private Overloads Sub DataPortal_Delete(ByVal criteria As Criteria)
       Using ctx = ConnectionFactory.GetDBConnection(True)
                Using cm = ctx.CreateSQLCommand

                    cm.CommandText = <SqlText>DELETE FROM pbs_MC_PRESCRIPTION_DETAIL_<%= _DTB %> WHERE LINE_NO= '<%= criteria._lineNo %>' </SqlText>.Value.Trim
                    cm.ExecuteNonQuery()

                End Using
            End Using

        End Sub

        Protected Overrides Sub DataPortal_OnDataPortalInvokeComplete(ByVal e As Csla.DataPortalEventArgs)
            If Csla.ApplicationContext.ExecutionLocation = ExecutionLocations.Server Then
                PrescriptionDetailInfoList.InvalidateCache()
            End If
        End Sub


#End Region 'Data Access                           

#Region " Exists "
        Public Shared Function Exists(ByVal pLineNo As String) As Boolean
            Return PrescriptionDetailInfoList.ContainsCode(pLineNo)
        End Function

        Public Shared Function KeyDuplicated(ByVal pLineNo As String) As Boolean
            Dim SqlText = <SqlText>SELECT COUNT(*) FROM pbs_MC_PRESCRIPTION_DETAIL_<%= Context.CurrentBECode %> WHERE LINE_NO= '<%= pLineNo %>'</SqlText>.Value.Trim
            Return SQLCommander.GetScalarInteger(SqlText) > 0
        End Function
#End Region

#Region " IGenpart "

        Public Function CloneBO(ByVal id As String) As Object Implements Interfaces.IGenPartObject.CloneBO
            Return ClonePrescriptionDetail(id)
        End Function

        Public Function getBO1(ByVal id As String) As Object Implements Interfaces.IGenPartObject.GetBO
            Return GetBO(id)
        End Function

        Public Function myCommands() As String() Implements Interfaces.IGenPartObject.myCommands
            Return pbs.Helper.Action.StandardReferenceCommands
        End Function

        Public Function myFullName() As String Implements Interfaces.IGenPartObject.myFullName
            Return GetType(PrescriptionDetail).ToString
        End Function

        Public Function myName() As String Implements Interfaces.IGenPartObject.myName
            Return GetType(PrescriptionDetail).ToString.Leaf
        End Function

        Public Function myQueryList() As IList Implements Interfaces.IGenPartObject.myQueryList
            Return PrescriptionDetailInfoList.GetPrescriptionDetailInfoList
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

#Region "Child"

        Shared Function NewPrescriptionDetailChild(pParentId As String) As PrescriptionDetail
            Dim ret = New PrescriptionDetail
            ret.PrescriptionNo = pParentId
            ret.MarkAsChild()
            Return ret
        End Function
        Shared Function GetChildPrescriptionDetail(dr As SafeDataReader) As Object
            Dim child = New PrescriptionDetail
            child.FetchObject(dr)
            child.MarkAsChild()
            child.MarkOld()
            Return child
        End Function

        Sub DeleteSelf(cn As IDbConnection)
            Using cm = cn.CreateSQLCommand
                cm.CommandType = CommandType.Text
                cm.CommandText = <sqltext>DELETE FROM pbs_MC_PRESCRIPTION_DETAIL_<%= _DTB %> WHERE LINE_NO = <%= _lineNo %></sqltext>
                cm.ExecuteNonQuery()
            End Using
        End Sub

        Sub Insert(cn As IDbConnection)
            Using cm = cn.CreateSQLCommand()

                cm.CommandType = CommandType.StoredProcedure
                cm.CommandText = String.Format("pbs_MC_Prescription_Detail_{0}_Insert", _DTB)

                cm.Parameters.AddWithValue("@LINE_NO", _lineNo.Trim.ToInteger, ParameterDirection.Output)
                AddInsertParameters(cm)
                cm.ExecuteNonQuery()

                _lineNo = cm.GetReturnIdAsInteger("@LINE_NO")
            End Using
        End Sub

        Sub Update(cn As IDbConnection)

            Using cm = cn.CreateSQLCommand()

                cm.CommandType = CommandType.StoredProcedure
                cm.CommandText = String.Format("pbs_MC_Prescription_Detail_{0}_Update", _DTB)

                cm.Parameters.AddWithValue("@LINE_NO", _lineNo.Trim)
                AddInsertParameters(cm)
                cm.ExecuteNonQuery()

                _lineNo = cm.GetReturnIdAsInteger("@LINE_NO")
            End Using

        End Sub
#End Region
    End Class

End Namespace