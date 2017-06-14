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
    <DB(TableName:="pbs_MC_NOTES_XXX")>
    Public Class NOTE
        Inherits Csla.BusinessBase(Of NOTE)
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

        Friend _lineNo As Integer
        <System.ComponentModel.DataObjectField(True, True)> _
        Public ReadOnly Property LineNo() As Integer
            Get
                Return _lineNo
            End Get
        End Property

        Private _patientCode As String = String.Empty
        <CellInfo("pbs.BO.MC.PATIENT", GroupName:="Header")>
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

        Private _topic As String = String.Empty
        Public Property Topic() As String
            Get
                Return _topic
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Topic", True)
                If value Is Nothing Then value = String.Empty
                If Not _topic.Equals(value) Then
                    _topic = value
                    PropertyHasChanged("Topic")
                End If
            End Set
        End Property

        Private _description As String = String.Empty
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

        Private _note As String = String.Empty
        <CellInfo(ControlType:=Forms.CtrlType.MemoEdit)>
        Public Property Note() As String
            Get
                Return _note
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Note", True)
                If value Is Nothing Then value = String.Empty
                If Not _note.Equals(value) Then
                    _note = value
                    PropertyHasChanged("Note")
                End If
            End Set
        End Property

        Private _category As String = String.Empty
        Public Property Category() As String
            Get
                Return _category
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Category", True)
                If value Is Nothing Then value = String.Empty
                If Not _category.Equals(value) Then
                    _category = value
                    PropertyHasChanged("Category")
                End If
            End Set
        End Property

        Private _sortNo As String = String.Empty
        Public Property SortNo() As String
            Get
                Return _sortNo
            End Get
            Set(ByVal value As String)
                CanWriteProperty("SortNo", True)
                If value Is Nothing Then value = String.Empty
                If Not _sortNo.Equals(value) Then
                    _sortNo = value
                    PropertyHasChanged("SortNo")
                End If
            End Set
        End Property

        Private _noteValue As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public Property NoteValue() As String
            Get
                Return _noteValue.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NoteValue", True)
                If value Is Nothing Then value = String.Empty
                If Not _noteValue.Equals(value) Then
                    _noteValue.Text = value
                    PropertyHasChanged("NoteValue")
                End If
            End Set
        End Property

        Private _unit As String = String.Empty
        <CellInfo(LinkCode.UnitCode)>
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

        Private _extDesc1 As String = String.Empty
        <CellInfo(Hidden:=True)>
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
        <CellInfo(Hidden:=True)>
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
        <CellInfo(Hidden:=True)>
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
        <CellInfo(Hidden:=True)>
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

        Private _extDesc5 As String = String.Empty
        <CellInfo(Hidden:=True)>
        Public Property ExtDesc5() As String
            Get
                Return _extDesc5
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ExtDesc5", True)
                If value Is Nothing Then value = String.Empty
                If Not _extDesc5.Equals(value) Then
                    _extDesc5 = value
                    PropertyHasChanged("ExtDesc5")
                End If
            End Set
        End Property

        Private _noteValue1 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(Hidden:=True)>
        Public Property NoteValue1() As String
            Get
                Return _noteValue1.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NoteValue1", True)
                If value Is Nothing Then value = String.Empty
                If Not _noteValue1.Equals(value) Then
                    _noteValue1.Text = value
                    PropertyHasChanged("NoteValue1")
                End If
            End Set
        End Property

        Private _noteValue2 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(Hidden:=True)>
        Public Property NoteValue2() As String
            Get
                Return _noteValue2.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NoteValue2", True)
                If value Is Nothing Then value = String.Empty
                If Not _noteValue2.Equals(value) Then
                    _noteValue2.Text = value
                    PropertyHasChanged("NoteValue2")
                End If
            End Set
        End Property

        Private _noteValue3 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(Hidden:=True)>
        Public Property NoteValue3() As String
            Get
                Return _noteValue3.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NoteValue3", True)
                If value Is Nothing Then value = String.Empty
                If Not _noteValue3.Equals(value) Then
                    _noteValue3.Text = value
                    PropertyHasChanged("NoteValue3")
                End If
            End Set
        End Property

        Private _noteValue4 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(Hidden:=True)>
        Public Property NoteValue4() As String
            Get
                Return _noteValue4.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NoteValue4", True)
                If value Is Nothing Then value = String.Empty
                If Not _noteValue4.Equals(value) Then
                    _noteValue4.Text = value
                    PropertyHasChanged("NoteValue4")
                End If
            End Set
        End Property

        Private _noteValue5 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(Hidden:=True)>
        Public Property NoteValue5() As String
            Get
                Return _noteValue5.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NoteValue5", True)
                If value Is Nothing Then value = String.Empty
                If Not _noteValue5.Equals(value) Then
                    _noteValue5.Text = value
                    PropertyHasChanged("NoteValue5")
                End If
            End Set
        End Property

        Private _noteDate1 As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo(LinkCode.Calendar, Hidden:=True)>
        Public Property NoteDate1() As String
            Get
                Return _noteDate1.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NoteDate1", True)
                If value Is Nothing Then value = String.Empty
                If Not _noteDate1.Equals(value) Then
                    _noteDate1.Text = value
                    PropertyHasChanged("NoteDate1")
                End If
            End Set
        End Property

        Private _noteDate2 As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo(LinkCode.Calendar, Hidden:=True)>
        Public Property NoteDate2() As String
            Get
                Return _noteDate2.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NoteDate2", True)
                If value Is Nothing Then value = String.Empty
                If Not _noteDate2.Equals(value) Then
                    _noteDate2.Text = value
                    PropertyHasChanged("NoteDate2")
                End If
            End Set
        End Property

        Private _noteDate3 As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo(LinkCode.Calendar, Hidden:=True)>
        Public Property NoteDate3() As String
            Get
                Return _noteDate3.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NoteDate3", True)
                If value Is Nothing Then value = String.Empty
                If Not _noteDate3.Equals(value) Then
                    _noteDate3.Text = value
                    PropertyHasChanged("NoteDate3")
                End If
            End Set
        End Property

        Private _noteDate4 As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo(LinkCode.Calendar, Hidden:=True)>
        Public Property NoteDate4() As String
            Get
                Return _noteDate4.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NoteDate4", True)
                If value Is Nothing Then value = String.Empty
                If Not _noteDate4.Equals(value) Then
                    _noteDate4.Text = value
                    PropertyHasChanged("NoteDate4")
                End If
            End Set
        End Property

        Private _noteDate5 As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo(LinkCode.Calendar, Hidden:=True)>
        Public Property NoteDate5() As String
            Get
                Return _noteDate5.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NoteDate5", True)
                If value Is Nothing Then value = String.Empty
                If Not _noteDate5.Equals(value) Then
                    _noteDate5.Text = value
                    PropertyHasChanged("NoteDate5")
                End If
            End Set
        End Property

        Private _notePeriod1 As SmartPeriod = New pbs.Helper.SmartPeriod()
        <CellInfo(LinkCode.Period, Hidden:=True)>
        Public Property NotePeriod1() As String
            Get
                Return _notePeriod1.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NotePeriod1", True)
                If value Is Nothing Then value = String.Empty
                If Not _notePeriod1.Equals(value) Then
                    _notePeriod1.Text = value
                    PropertyHasChanged("NotePeriod1")
                End If
            End Set
        End Property

        Private _notePeriod2 As SmartPeriod = New pbs.Helper.SmartPeriod()
        <CellInfo(LinkCode.Period, Hidden:=True)>
        Public Property NotePeriod2() As String
            Get
                Return _notePeriod2.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NotePeriod2", True)
                If value Is Nothing Then value = String.Empty
                If Not _notePeriod2.Equals(value) Then
                    _notePeriod2.Text = value
                    PropertyHasChanged("NotePeriod2")
                End If
            End Set
        End Property

        Private _notePeriod3 As SmartPeriod = New pbs.Helper.SmartPeriod()
        <CellInfo(LinkCode.Period, Hidden:=True)>
        Public Property NotePeriod3() As String
            Get
                Return _notePeriod3.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NotePeriod3", True)
                If value Is Nothing Then value = String.Empty
                If Not _notePeriod3.Equals(value) Then
                    _notePeriod3.Text = value
                    PropertyHasChanged("NotePeriod3")
                End If
            End Set
        End Property

        Private _notePeriod4 As SmartPeriod = New pbs.Helper.SmartPeriod()
        <CellInfo(LinkCode.Period, Hidden:=True)>
        Public Property NotePeriod4() As String
            Get
                Return _notePeriod4.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NotePeriod4", True)
                If value Is Nothing Then value = String.Empty
                If Not _notePeriod4.Equals(value) Then
                    _notePeriod4.Text = value
                    PropertyHasChanged("NotePeriod4")
                End If
            End Set
        End Property

        Private _notePeriod5 As SmartPeriod = New pbs.Helper.SmartPeriod()
        <CellInfo(LinkCode.Period, Hidden:=True)>
        Public Property NotePeriod5() As String
            Get
                Return _notePeriod5.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NotePeriod5", True)
                If value Is Nothing Then value = String.Empty
                If Not _notePeriod5.Equals(value) Then
                    _notePeriod5.Text = value
                    PropertyHasChanged("NotePeriod5")
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
                If Not _formDataId.Equals(value) Then
                    _formDataId = value
                    PropertyHasChanged("FormDataId")
                End If
            End Set
        End Property

        Private _updatedBy As String = String.Empty
        Public ReadOnly Property UpdatedBy() As String
            Get
                Return _updatedBy
            End Get
        End Property

        Private _updated As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property Updated() As String
            Get
                Return _updated.Text
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

#Region "Validation Rules"

        Friend Sub CheckRules()
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

            For Each _field As ClassField In ClassSchema(Of NOTE)._fieldList
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

        Public Shared Function BlankNOTE() As NOTE
            Return New NOTE
        End Function

        Public Shared Function NewNOTE(ByVal pLineNo As Integer) As NOTE
            'If KeyDuplicated(pLineNo) Then ExceptionThower.BusinessRuleStop(String.Format(ResStr(ResStrConst.NOACCESS), ResStr("NOTE")))
            Return DataPortal.Create(Of NOTE)(New Criteria(pLineNo))
        End Function

        Public Shared Function NewBO(ByVal ID As String) As NOTE
            Dim pLineNo As Integer = ID.Trim.ToInteger

            Return NewNOTE(pLineNo)
        End Function

        Public Shared Function GetNOTE(ByVal pLineNo As Integer) As NOTE
            Return DataPortal.Fetch(Of NOTE)(New Criteria(pLineNo))
        End Function

        Public Shared Function GetBO(ByVal ID As String) As NOTE
            Dim pLineNo As Integer = ID.Trim.ToInteger

            Return GetNOTE(pLineNo)
        End Function

        Public Shared Sub DeleteNOTE(ByVal pLineNo As Integer)
            DataPortal.Delete(New Criteria(pLineNo))
            NOTEInfoList.RemoveInfo(pLineNo)
        End Sub

        Public Overrides Function Save() As NOTE
            If Not IsDirty Then ExceptionThower.NotDirty(ResStr(ResStrConst.NOTDIRTY))
            If Not IsSavable Then Throw New Csla.Validation.ValidationException(String.Format(ResStr(ResStrConst.INVALID), ResStr("NOTE")))

            Me.ApplyEdit()

            Dim ret = MyBase.Save()

            If Not Context.IsBatchSavingMode Then NOTEInfoList.InvalidateCache()

            Return ret

        End Function

        Public Function CloneNOTE(ByVal pLineNo As String) As NOTE

            Dim cloning As NOTE = MyBase.Clone
            cloning._lineNo = 0
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

            Public Sub New(ByVal pLineNo As Integer)
                _lineNo = pLineNo

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

                    cm.CommandText = <SqlText>SELECT * FROM pbs_MC_NOTES_<%= _DTB %>  WHERE LINE_NO= '<%= criteria._lineNo %>' </SqlText>.Value.Trim

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
            '_leadId.Text = dr.GetInt32("LEAD_ID")
            '_candidateId.Text = dr.GetInt32("CANDIDATE_ID")
            _patientCode = dr.GetString("PATIENT_CODE").TrimEnd
            _topic = dr.GetString("TOPIC").TrimEnd
            _description = dr.GetString("DESCRIPTION").TrimEnd
            _note = dr.GetString("NOTE").TrimEnd
            _category = dr.GetString("CATEGORY").TrimEnd
            _sortNo = dr.GetString("SORT_NO").TrimEnd
            _noteValue.Text = dr.GetDecimal("NOTE_VALUE")
            _unit = dr.GetString("UNIT").TrimEnd
            _extDesc1 = dr.GetString("EXT_DESC1").TrimEnd
            _extDesc2 = dr.GetString("EXT_DESC2").TrimEnd
            _extDesc3 = dr.GetString("EXT_DESC3").TrimEnd
            _extDesc4 = dr.GetString("EXT_DESC4").TrimEnd
            _extDesc5 = dr.GetString("EXT_DESC5").TrimEnd
            _noteValue1.Text = dr.GetDecimal("NOTE_VALUE1")
            _noteValue2.Text = dr.GetDecimal("NOTE_VALUE2")
            _noteValue3.Text = dr.GetDecimal("NOTE_VALUE3")
            _noteValue4.Text = dr.GetDecimal("NOTE_VALUE4")
            _noteValue5.Text = dr.GetDecimal("NOTE_VALUE5")
            _noteDate1.Text = dr.GetInt32("NOTE_DATE1")
            _noteDate2.Text = dr.GetInt32("NOTE_DATE2")
            _noteDate3.Text = dr.GetInt32("NOTE_DATE3")
            _noteDate4.Text = dr.GetInt32("NOTE_DATE4")
            _noteDate5.Text = dr.GetInt32("NOTE_DATE5")
            _notePeriod1.Text = dr.GetInt32("NOTE_PERIOD1")
            _notePeriod2.Text = dr.GetInt32("NOTE_PERIOD2")
            _notePeriod3.Text = dr.GetInt32("NOTE_PERIOD3")
            _notePeriod4.Text = dr.GetInt32("NOTE_PERIOD4")
            _notePeriod5.Text = dr.GetInt32("NOTE_PERIOD5")
            _bphNo = dr.GetInt32("BPH_NO")
            _formDataId = dr.GetInt32("FORM_DATA_ID")
            _updatedBy = dr.GetString("UPDATED_BY").TrimEnd
            _updated.Text = dr.GetInt32("UPDATED")

        End Sub

        Private Shared _lockObj As New Object
        Protected Overrides Sub DataPortal_Insert()
            SyncLock _lockObj
                Using ctx = ConnectionFactory.GetDBConnection(True)
                 
                    ExecuteInsert(ctx)
                    End Using
            End SyncLock
        End Sub

        Private Sub AddInsertParameters(ByVal cm As IDBCommand)

            'cm.Parameters.AddWithValue("@LEAD_ID", _leadId.DBValue)
            'cm.Parameters.AddWithValue("@CANDIDATE_ID", _candidateId.DBValue)
            cm.Parameters.AddWithValue("@PATIENT_CODE", _patientCode.Trim)
            cm.Parameters.AddWithValue("@TOPIC", _topic.Trim)
            cm.Parameters.AddWithValue("@DESCRIPTION", _description.Trim)
            cm.Parameters.AddWithValue("@NOTE", _note.Trim)
            cm.Parameters.AddWithValue("@CATEGORY", _category.Trim)
            cm.Parameters.AddWithValue("@SORT_NO", _sortNo.Trim)
            cm.Parameters.AddWithValue("@NOTE_VALUE", _noteValue.DBValue)
            cm.Parameters.AddWithValue("@UNIT", _unit.Trim)
            cm.Parameters.AddWithValue("@EXT_DESC1", _extDesc1.Trim)
            cm.Parameters.AddWithValue("@EXT_DESC2", _extDesc2.Trim)
            cm.Parameters.AddWithValue("@EXT_DESC3", _extDesc3.Trim)
            cm.Parameters.AddWithValue("@EXT_DESC4", _extDesc4.Trim)
            cm.Parameters.AddWithValue("@EXT_DESC5", _extDesc5.Trim)
            cm.Parameters.AddWithValue("@NOTE_VALUE1", _noteValue1.DBValue)
            cm.Parameters.AddWithValue("@NOTE_VALUE2", _noteValue2.DBValue)
            cm.Parameters.AddWithValue("@NOTE_VALUE3", _noteValue3.DBValue)
            cm.Parameters.AddWithValue("@NOTE_VALUE4", _noteValue4.DBValue)
            cm.Parameters.AddWithValue("@NOTE_VALUE5", _noteValue5.DBValue)
            cm.Parameters.AddWithValue("@NOTE_DATE1", _noteDate1.DBValue)
            cm.Parameters.AddWithValue("@NOTE_DATE2", _noteDate2.DBValue)
            cm.Parameters.AddWithValue("@NOTE_DATE3", _noteDate3.DBValue)
            cm.Parameters.AddWithValue("@NOTE_DATE4", _noteDate4.DBValue)
            cm.Parameters.AddWithValue("@NOTE_DATE5", _noteDate5.DBValue)
            cm.Parameters.AddWithValue("@NOTE_PERIOD1", _notePeriod1.DBValue)
            cm.Parameters.AddWithValue("@NOTE_PERIOD2", _notePeriod2.DBValue)
            cm.Parameters.AddWithValue("@NOTE_PERIOD3", _notePeriod3.DBValue)
            cm.Parameters.AddWithValue("@NOTE_PERIOD4", _notePeriod4.DBValue)
            cm.Parameters.AddWithValue("@NOTE_PERIOD5", _notePeriod5.DBValue)
            cm.Parameters.AddWithValue("@BPH_NO", _bphNo)
            cm.Parameters.AddWithValue("@FORM_DATA_ID", _formDataId)
            cm.Parameters.AddWithValue("@UPDATED_BY", Context.CurrentUserCode)
            cm.Parameters.AddWithValue("@UPDATED", Now.ToSunDate)

        End Sub

        Protected Overrides Sub DataPortal_Update()
            Using ctx = ConnectionFactory.GetDBConnection(True)
            
                ExecuteUpdate(ctx)
                End Using
        End Sub

        Protected Overrides Sub DataPortal_DeleteSelf()
            DataPortal_Delete(New Criteria(_lineNo))
        End Sub

        Private Overloads Sub DataPortal_Delete(ByVal criteria As Criteria)
          Using ctx = ConnectionFactory.GetDBConnection(True)
                Using cm = ctx.CreateSQLCommand

                    cm.CommandText = <SqlText>DELETE FROM pbs_MC_NOTES_<%= _DTB %>  WHERE LINE_NO= '<%= criteria._lineNo %>' </SqlText>.Value.Trim
                    cm.ExecuteNonQuery()

                End Using
            End Using

        End Sub


#End Region 'Data Access                           

#Region " Exists "
        Public Shared Function Exists(ByVal pLineNo As String) As Boolean
            Return KeyDuplicated(pLineNo.ToInteger)
        End Function

        Public Shared Function KeyDuplicated(ByVal pLineNo As Integer) As Boolean
            If pLineNo <= 0 Then Return False
            Dim SqlText = <SqlText>SELECT COUNT(*) FROM pbs_MC_NOTES_<%= Context.CurrentBECode %> WHERE LINE_NO=<%= pLineNo %></SqlText>.Value.Trim
            Return SQLCommander.GetScalarInteger(SqlText) > 0
        End Function
#End Region

#Region " IGenpart "

        Public Function CloneBO(ByVal id As String) As Object Implements Interfaces.IGenPartObject.CloneBO
            Return CloneNOTE(id)
        End Function

        Public Function getBO1(ByVal id As String) As Object Implements Interfaces.IGenPartObject.GetBO
            Return GetBO(id)
        End Function

        Public Function myCommands() As String() Implements Interfaces.IGenPartObject.myCommands
            Return pbs.Helper.Action.StandardReferenceCommands
        End Function

        Public Function myFullName() As String Implements Interfaces.IGenPartObject.myFullName
            Return GetType(NOTE).ToString
        End Function

        Public Function myName() As String Implements Interfaces.IGenPartObject.myName
            Return GetType(NOTE).ToString.Leaf
        End Function

        Public Function myQueryList() As IList Implements Interfaces.IGenPartObject.myQueryList
            Return NOTEInfoList.GetNOTEInfoList
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

    End Class

End Namespace