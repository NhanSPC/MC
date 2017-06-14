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
    <DB(TableName:="pbs_MC_LAB_TEST_{XXX}")>
    Public Class LABTEST
        Inherits Csla.BusinessBase(Of LABTEST)
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
        Private _DTB As String = String.Empty


        Private _lineNo As String = String.Empty
        <System.ComponentModel.DataObjectField(True, False)>
        Public ReadOnly Property LineNo() As String
            Get
                Return _lineNo
            End Get
        End Property

        Private _labCode As String = String.Empty
        <CellInfo(GroupName:="")>
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

        Private _descriptn As String = String.Empty
        Public Property Descriptn() As String
            Get
                Return _descriptn
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Descriptn", True)
                If value Is Nothing Then value = String.Empty
                If Not _descriptn.Equals(value) Then
                    _descriptn = value
                    PropertyHasChanged("Descriptn")
                End If
            End Set
        End Property

        Private _unit As String = String.Empty
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

        Private _minValue As String = String.Empty
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

        Private _notes As String = String.Empty
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

            For Each _field As ClassField In ClassSchema(Of LABTEST)._fieldList
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

        Public Shared Function BlankLABTEST() As LABTEST
            Return New LABTEST
        End Function

        Public Shared Function NewLABTEST(ByVal pLineNo As String) As LABTEST
            If KeyDuplicated(pLineNo) Then ExceptionThower.BusinessRuleStop(String.Format(ResStr(ResStrConst.NOACCESS), ResStr("LABTEST")))
            Return DataPortal.Create(Of LABTEST)(New Criteria(pLineNo))
        End Function

        Public Shared Function NewBO(ByVal ID As String) As LABTEST
            Dim pLineNo As String = ID.Trim

            Return NewLABTEST(pLineNo)
        End Function

        Public Shared Function GetLABTEST(ByVal pLineNo As String) As LABTEST
            Return DataPortal.Fetch(Of LABTEST)(New Criteria(pLineNo))
        End Function

        Public Shared Function GetBO(ByVal ID As String) As LABTEST
            Dim pLineNo As String = ID.Trim

            Return GetLABTEST(pLineNo)
        End Function

        Public Shared Sub DeleteLABTEST(ByVal pLineNo As String)
            DataPortal.Delete(New Criteria(pLineNo))
        End Sub

        Public Overrides Function Save() As LABTEST
            If Not IsDirty Then ExceptionThower.NotDirty(ResStr(ResStrConst.NOTDIRTY))
            If Not IsSavable Then Throw New Csla.Validation.ValidationException(String.Format(ResStr(ResStrConst.INVALID), ResStr("LABTEST")))

            Me.ApplyEdit()
            LABTESTInfoList.InvalidateCache()
            Return MyBase.Save()
        End Function

        Public Function CloneLABTEST(ByVal pLineNo As String) As LABTEST

            If LABTEST.KeyDuplicated(pLineNo) Then ExceptionThower.BusinessRuleStop(ResStr(ResStrConst.CreateAlreadyExists), Me.GetType.ToString.Leaf.Translate)

            Dim cloningLABTEST As LABTEST = MyBase.Clone
            cloningLABTEST._lineNo = pLineNo

            'Todo:Remember to reset status of the new object here 
            cloningLABTEST.MarkNew()
            cloningLABTEST.ApplyEdit()

            cloningLABTEST.ValidationRules.CheckRules()

            Return cloningLABTEST
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

                    cm.CommandText = <SqlText>SELECT * FROM pbs_MC_LAB_TEST_<%= _DTB %> WHERE LINE_NO= '<%= criteria._lineNo %>' </SqlText>.Value.Trim

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
            _labCode = dr.GetString("LAB_CODE").TrimEnd
            _testCode = dr.GetString("TEST_CODE").TrimEnd
            _descriptn = dr.GetString("DESCRIPTN").TrimEnd
            _unit = dr.GetString("UNIT").TrimEnd
            _minValue = dr.GetString("MIN_VALUE").TrimEnd
            _maxValue = dr.GetString("MAX_VALUE").TrimEnd
            _minValueMale = dr.GetString("MIN_VALUE_MALE").TrimEnd
            _maxValueMale = dr.GetString("MAX_VALUE_MALE").TrimEnd
            _minValueFemale = dr.GetString("MIN_VALUE_FEMALE").TrimEnd
            _maxValueFemale = dr.GetString("MAX_VALUE_FEMALE").TrimEnd
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
                        cm.CommandText = String.Format("pbs_MC_LAB_TEST_{0}_Insert", _DTB)

                        cm.Parameters.AddWithValue("@LINE_NO", _lineNo.Trim.ToInteger, ParameterDirection.Output)
                        AddInsertParameters(cm)
                        cm.ExecuteNonQuery()

                        _lineNo = cm.GetReturnIdAsInteger("@LINE_NO")
                    End Using
                End Using
            End SyncLock
        End Sub

        Private Sub AddInsertParameters(ByVal cm As IDBCommand)

            cm.Parameters.AddWithValue("@LAB_CODE", _labCode.Trim)
            cm.Parameters.AddWithValue("@TEST_CODE", _testCode.Trim)
            cm.Parameters.AddWithValue("@DESCRIPTN", _descriptn.Trim)
            cm.Parameters.AddWithValue("@UNIT", _unit.Trim)
            cm.Parameters.AddWithValue("@MIN_VALUE", _minValue.Trim)
            cm.Parameters.AddWithValue("@MAX_VALUE", _maxValue.Trim)
            cm.Parameters.AddWithValue("@MIN_VALUE_MALE", _minValueMale.Trim)
            cm.Parameters.AddWithValue("@MAX_VALUE_MALE", _maxValueMale.Trim)
            cm.Parameters.AddWithValue("@MIN_VALUE_FEMALE", _minValueFemale.Trim)
            cm.Parameters.AddWithValue("@MAX_VALUE_FEMALE", _maxValueFemale.Trim)
            cm.Parameters.AddWithValue("@NOTES", _notes.Trim)
            cm.Parameters.AddWithValue("@UPDATED", _updated.DBValue)
            cm.Parameters.AddWithValue("@UPDATED_BY", _updatedBy.Trim)
        End Sub


        Protected Overrides Sub DataPortal_Update()
            SyncLock _lockObj
                Using ctx = ConnectionFactory.GetDBConnection(True)
                    Using cm = ctx.CreateSQLCommand

                        cm.CommandType = CommandType.StoredProcedure
                        cm.CommandText = String.Format("pbs_MC_LAB_TEST_{0}_Update", _DTB)

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

                    cm.CommandText = <SqlText>DELETE FROM pbs_MC_LAB_TEST_<%= _DTB %> WHERE LINE_NO= '<%= criteria._lineNo %>' </SqlText>.Value.Trim
                    cm.ExecuteNonQuery()

                End Using
            End Using

        End Sub

        Protected Overrides Sub DataPortal_OnDataPortalInvokeComplete(ByVal e As Csla.DataPortalEventArgs)
            If Csla.ApplicationContext.ExecutionLocation = ExecutionLocations.Server Then
                LABTESTInfoList.InvalidateCache()
            End If
        End Sub


#End Region 'Data Access                           

#Region " Exists "
        Public Shared Function Exists(ByVal pLineNo As String) As Boolean
            Return LABTESTInfoList.ContainsCode(pLineNo)
        End Function

        Public Shared Function KeyDuplicated(ByVal pLineNo As String) As Boolean
            Dim SqlText = <SqlText>SELECT COUNT(*) FROM pbs_MC_LAB_TEST_<%= Context.CurrentBECode %> WHERE LINE_NO= '<%= pLineNo %>'</SqlText>.Value.Trim
            Return SQLCommander.GetScalarInteger(SqlText) > 0
        End Function
#End Region

#Region " IGenpart "

        Public Function CloneBO(ByVal id As String) As Object Implements Interfaces.IGenPartObject.CloneBO
            Return CloneLABTEST(id)
        End Function

        Public Function getBO1(ByVal id As String) As Object Implements Interfaces.IGenPartObject.GetBO
            Return GetBO(id)
        End Function

        Public Function myCommands() As String() Implements Interfaces.IGenPartObject.myCommands
            Return pbs.Helper.Action.StandardReferenceCommands
        End Function

        Public Function myFullName() As String Implements Interfaces.IGenPartObject.myFullName
            Return GetType(LABTEST).ToString
        End Function

        Public Function myName() As String Implements Interfaces.IGenPartObject.myName
            Return GetType(LABTEST).ToString.Leaf
        End Function

        Public Function myQueryList() As IList Implements Interfaces.IGenPartObject.myQueryList
            Return LABTESTInfoList.GetLABTESTInfoList
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