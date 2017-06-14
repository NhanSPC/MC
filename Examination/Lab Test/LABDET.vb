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
    <DB(TableName:="pbs_MC_LAB_DETAIL_{XXX}")>
    Public Class LABDET
        Inherits Csla.BusinessBase(Of LABDET)
        Implements Interfaces.IGenPartObject
        Implements IComparable
        Implements IDocLink



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
        Friend _DTB As String = String.Empty


        Friend _lineNo As String = String.Empty
        <System.ComponentModel.DataObjectField(True, True)>
        Public ReadOnly Property LineNo() As String
            Get
                Return _lineNo
            End Get
        End Property

        Private _labId As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0) '<Rule(Required:=True)>
        <CellInfo(GroupName:="Lab info", Tips:="Enter test id")>
        Public Property LabId() As String
            Get
                Return _labId.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("LabId", True)
                If value Is Nothing Then value = String.Empty
                If Not _labId.Equals(value) Then
                    _labId.Text = value
                    PropertyHasChanged("LabId")
                End If
            End Set
        End Property

        Private _labCode As String = String.Empty
        <CellInfo(GroupName:="Lab info", Tips:="Enter group of test")>
        Public Property LabCode() As String
            Get
                Return _labCode
            End Get
            Set(ByVal value As String)
                CanWriteProperty("LabCode", True)
                If value Is Nothing Then value = String.Empty
                If Not _labCode.Equals(value) Then
                    _labCode = value
                    PropertyHasChanged("LabCode")
                End If
            End Set
        End Property

        Private _testCode As String = String.Empty
        <CellInfo(GroupName:="Lab info", Tips:="Enter test name")>
        Public Property TestCode() As String
            Get
                Return _testCode
            End Get
            Set(ByVal value As String)
                CanWriteProperty("TestCode", True)
                If value Is Nothing Then value = String.Empty
                If Not _testCode.Equals(value) Then
                    _testCode = value
                    PropertyHasChanged("TestCode")
                End If
            End Set
        End Property

        Private _testName As String = String.Empty
        <CellInfo(GroupName:="Lab info", Tips:="Enter test name")>
        Public Property TestName() As String
            Get
                Return _testName
            End Get
            Set(ByVal value As String)
                CanWriteProperty("TestName", True)
                If value Is Nothing Then value = String.Empty
                If Not _testName.Equals(value) Then
                    _testName = value
                    PropertyHasChanged("TestName")
                End If
            End Set
        End Property

        Private _unit As String = String.Empty
        <CellInfo(GroupName:="Lab info", Tips:="Enter test's unit")>
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

        Private _value As String = String.Empty
        <CellInfo(GroupName:="Lab info", Tips:="Enter test value")>
        Public Property Value() As String
            Get
                Return _value
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Value", True)
                If value Is Nothing Then value = String.Empty
                If Not _value.Equals(value) Then
                    _value = value
                    PropertyHasChanged("Value")
                End If
            End Set
        End Property

        Private _minValue As String = String.Empty
        <CellInfo(GroupName:="Value", Tips:="Enter min value")>
        Public Property MinValue() As String
            Get
                Return _minValue
            End Get
            Set(ByVal value As String)
                CanWriteProperty("MinValue", True)
                If value Is Nothing Then value = String.Empty
                If Not _minValue.Equals(value) Then
                    _minValue = value
                    PropertyHasChanged("MinValue")
                End If
            End Set
        End Property

        Private _maxValue As String = String.Empty
        <CellInfo(GroupName:="Value", Tips:="Enter max value")>
        Public Property MaxValue() As String
            Get
                Return _maxValue
            End Get
            Set(ByVal value As String)
                CanWriteProperty("MaxValue", True)
                If value Is Nothing Then value = String.Empty
                If Not _maxValue.Equals(value) Then
                    _maxValue = value
                    PropertyHasChanged("MaxValue")
                End If
            End Set
        End Property

        Private _minValueMale As String = String.Empty
        <CellInfo(GroupName:="Male Value", Tips:="Enter min value")>
        Public Property MinValueMale() As String
            Get
                Return _minValueMale
            End Get
            Set(ByVal value As String)
                CanWriteProperty("MinValueMale", True)
                If value Is Nothing Then value = String.Empty
                If Not _minValueMale.Equals(value) Then
                    _minValueMale = value
                    PropertyHasChanged("MinValueMale")
                End If
            End Set
        End Property

        Private _maxValueMale As String = String.Empty
        <CellInfo(GroupName:="Male value", Tips:="Enter max value")>
        Public Property MaxValueMale() As String
            Get
                Return _maxValueMale
            End Get
            Set(ByVal value As String)
                CanWriteProperty("MaxValueMale", True)
                If value Is Nothing Then value = String.Empty
                If Not _maxValueMale.Equals(value) Then
                    _maxValueMale = value
                    PropertyHasChanged("MaxValueMale")
                End If
            End Set
        End Property

        Private _minValueFemale As String = String.Empty
        <CellInfo(GroupName:="Female value", Tips:="Enter min value")>
        Public Property MinValueFemale() As String
            Get
                Return _minValueFemale
            End Get
            Set(ByVal value As String)
                CanWriteProperty("MinValueFemale", True)
                If value Is Nothing Then value = String.Empty
                If Not _minValueFemale.Equals(value) Then
                    _minValueFemale = value
                    PropertyHasChanged("MinValueFemale")
                End If
            End Set
        End Property

        Private _maxValueFemale As String = String.Empty
        <CellInfo(GroupName:="Female value", Tips:="Enter max value")>
        Public Property MaxValueFemale() As String
            Get
                Return _maxValueFemale
            End Get
            Set(ByVal value As String)
                CanWriteProperty("MaxValueFemale", True)
                If value Is Nothing Then value = String.Empty
                If Not _maxValueFemale.Equals(value) Then
                    _maxValueFemale = value
                    PropertyHasChanged("MaxValueFemale")
                End If
            End Set
        End Property

        Private _interpretation As String = String.Empty
        <CellInfo(GroupName:="Result", Tips:="Enter interpretation")>
        Public Property Interpretation() As String
            Get
                Return _interpretation
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Interpretation", True)
                If value Is Nothing Then value = String.Empty
                If Not _interpretation.Equals(value) Then
                    _interpretation = value
                    PropertyHasChanged("Interpretation")
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

            For Each _field As ClassField In ClassSchema(Of LABDET)._fieldList
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

        Public Shared Function BlankLABDET() As LABDET
            Return New LABDET
        End Function

        Public Shared Function NewLABDET(ByVal pLineNo As String) As LABDET
            If KeyDuplicated(pLineNo) Then ExceptionThower.BusinessRuleStop(String.Format(ResStr(ResStrConst.NOACCESS), ResStr("LABDET")))
            Return DataPortal.Create(Of LABDET)(New Criteria(pLineNo))
        End Function

        Public Shared Function NewBO(ByVal ID As String) As LABDET
            Dim pLineNo As String = ID.Trim

            Return NewLABDET(pLineNo)
        End Function

        Public Shared Function GetLABDET(ByVal pLineNo As String) As LABDET
            Return DataPortal.Fetch(Of LABDET)(New Criteria(pLineNo))
        End Function

        Public Shared Function GetBO(ByVal ID As String) As LABDET
            Dim pLineNo As String = ID.Trim

            Return GetLABDET(pLineNo)
        End Function

        Public Shared Sub DeleteLABDET(ByVal pLineNo As String)
            DataPortal.Delete(New Criteria(pLineNo))
        End Sub

        Public Overrides Function Save() As LABDET
            If Not IsDirty Then ExceptionThower.NotDirty(ResStr(ResStrConst.NOTDIRTY))
            If Not IsSavable Then Throw New Csla.Validation.ValidationException(String.Format(ResStr(ResStrConst.INVALID), ResStr("LABDET")))

            Me.ApplyEdit()
            LABDETInfoList.InvalidateCache()
            Return MyBase.Save()
        End Function

        Public Function CloneLABDET(ByVal pLineNo As String) As LABDET

            If LABDET.KeyDuplicated(pLineNo) Then ExceptionThower.BusinessRuleStop(ResStr(ResStrConst.CreateAlreadyExists), Me.GetType.ToString.Leaf.Translate)

            Dim cloningLABDET As LABDET = MyBase.Clone
            cloningLABDET._lineNo = pLineNo

            'Todo:Remember to reset status of the new object here 
            cloningLABDET.MarkNew()
            cloningLABDET.ApplyEdit()

            cloningLABDET.ValidationRules.CheckRules()

            Return cloningLABDET
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

                    cm.CommandText = <SqlText>SELECT * FROM pbs_MC_LAB_DETAIL_<%= _DTB %> WHERE LINE_NO= '<%= criteria._lineNo %>' </SqlText>.Value.Trim

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
            _labId.Text = dr.GetInt32("LAB_ID")
            _labCode = dr.GetString("LAB_CODE").TrimEnd
            _testCode = dr.GetString("TEST_CODE").TrimEnd
            _testName = dr.GetString("TEST_NAME").TrimEnd
            _unit = dr.GetString("UNIT").TrimEnd
            _value = dr.GetString("VALUE").TrimEnd
            _minValue = dr.GetString("MIN_VALUE").TrimEnd
            _maxValue = dr.GetString("MAX_VALUE").TrimEnd
            _minValueMale = dr.GetString("MIN_VALUE_MALE").TrimEnd
            _maxValueMale = dr.GetString("MAX_VALUE_MALE").TrimEnd
            _minValueFemale = dr.GetString("MIN_VALUE_FEMALE").TrimEnd
            _maxValueFemale = dr.GetString("MAX_VALUE_FEMALE").TrimEnd
            _interpretation = dr.GetString("INTERPRETATION").TrimEnd
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

            cm.Parameters.AddWithValue("@LAB_ID", _labId.DBValue)
            cm.Parameters.AddWithValue("@LAB_CODE", _labCode.Trim)
            cm.Parameters.AddWithValue("@TEST_CODE", _testCode.Trim)
            cm.Parameters.AddWithValue("@TEST_NAME", _testName.Trim)
            cm.Parameters.AddWithValue("@UNIT", _unit.Trim)
            cm.Parameters.AddWithValue("@VALUE", _value.Trim)
            cm.Parameters.AddWithValue("@MIN_VALUE", _minValue.Trim)
            cm.Parameters.AddWithValue("@MAX_VALUE", _maxValue.Trim)
            cm.Parameters.AddWithValue("@MIN_VALUE_MALE", _minValueMale.Trim)
            cm.Parameters.AddWithValue("@MAX_VALUE_MALE", _maxValueMale.Trim)
            cm.Parameters.AddWithValue("@MIN_VALUE_FEMALE", _minValueFemale.Trim)
            cm.Parameters.AddWithValue("@MAX_VALUE_FEMALE", _maxValueFemale.Trim)
            cm.Parameters.AddWithValue("@INTERPRETATION", _interpretation.Trim)
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

                    cm.CommandText = <SqlText>DELETE FROM pbs_MC_LAB_DETAIL_<%= _DTB %> WHERE LINE_NO= '<%= criteria._lineNo %>' </SqlText>.Value.Trim
                    cm.ExecuteNonQuery()

                End Using
            End Using

        End Sub

        Protected Overrides Sub DataPortal_OnDataPortalInvokeComplete(ByVal e As Csla.DataPortalEventArgs)
            If Csla.ApplicationContext.ExecutionLocation = ExecutionLocations.Server Then
                LABDETInfoList.InvalidateCache()
            End If
        End Sub


#End Region 'Data Access                           

#Region " Exists "
        Public Shared Function Exists(ByVal pLineNo As String) As Boolean
            Return LABDETInfoList.ContainsCode(pLineNo)
        End Function

        Public Shared Function KeyDuplicated(ByVal pLineNo As String) As Boolean
            Dim SqlText = <SqlText>SELECT COUNT(*) FROM pbs_MC_LAB_DETAIL_<%= Context.CurrentBECode %> WHERE LINE_NO= '<%= pLineNo %>'</SqlText>.Value.Trim
            Return SQLCommander.GetScalarInteger(SqlText) > 0
        End Function
#End Region

#Region " IGenpart "

        Public Function CloneBO(ByVal id As String) As Object Implements Interfaces.IGenPartObject.CloneBO
            Return CloneLABDET(id)
        End Function

        Public Function getBO1(ByVal id As String) As Object Implements Interfaces.IGenPartObject.GetBO
            Return GetBO(id)
        End Function

        Public Function myCommands() As String() Implements Interfaces.IGenPartObject.myCommands
            Return pbs.Helper.Action.StandardReferenceCommands
        End Function

        Public Function myFullName() As String Implements Interfaces.IGenPartObject.myFullName
            Return GetType(LABDET).ToString
        End Function

        Public Function myName() As String Implements Interfaces.IGenPartObject.myName
            Return GetType(LABDET).ToString.Leaf
        End Function

        Public Function myQueryList() As IList Implements Interfaces.IGenPartObject.myQueryList
            Return LABDETInfoList.GetLABDETInfoList
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
        Shared Function GetChilLABDET(dr As SafeDataReader) As Object
            Dim child = New LABDET
            child.FetchObject(dr)
            child.MarkAsChild()
            child.MarkOld()
            Return child
        End Function

        Sub DeleteSelf(cn As IDbConnection)
            Using cm = cn.CreateSQLCommand
                cm.CommandType = CommandType.Text
                cm.CommandText = <sqltext>DELETE FROM pbs_MC_LABDET_<%= _DTB %> WHERE LINE_NO = <%= _lineNo %></sqltext>
                cm.ExecuteNonQuery()
            End Using
        End Sub

        Sub Insert(cn As IDbConnection)
            Using cm = cn.CreateSQLCommand()

                cm.CommandType = CommandType.StoredProcedure
                cm.CommandText = String.Format("pbs_MC_LAB_DETAIL_{0}_Insert", _DTB)

                cm.Parameters.AddWithValue("@LINE_NO", _lineNo.Trim.ToInteger, ParameterDirection.Output)
                AddInsertParameters(cm)
                cm.ExecuteNonQuery()

                _lineNo = cm.GetReturnIdAsInteger("@LINE_NO")
            End Using
        End Sub

        Sub Update(cn As IDbConnection)

            Using cm = cn.CreateSQLCommand()

                cm.CommandType = CommandType.StoredProcedure
                cm.CommandText = String.Format("pbs_MC_LAB_DETAIL_{0}_Update", _DTB)

                cm.Parameters.AddWithValue("@LINE_NO", _lineNo.Trim)
                AddInsertParameters(cm)
                cm.ExecuteNonQuery()

                _lineNo = cm.GetReturnIdAsInteger("@LINE_NO")
            End Using

        End Sub
#End Region
    End Class
End Namespace