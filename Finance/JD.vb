Imports pbs.Helper
Imports System.Data

Imports Csla
Imports Csla.Data
Imports Csla.Validation
Imports pbs.BO.DataAnnotations
Imports System.Xml.Linq
Imports pbs.BO
Imports pbs.BO.PS
Imports pbs.BO.SQLBuilder
Imports pbs.BO.BusinessRules

Namespace MC

    ''' <summary>
    ''' Journal Definition
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    <DB(TableName:="pbs_RFMSC")>
    <PhoebusCommand(Desc:="Journal Definition (MC)")>
    Public Class JD
        Inherits Csla.BusinessBase(Of JD)
        Implements Interfaces.IGenPartObject
        Implements IDocLink
        Implements ILookupProvider

#Region "Property Changed"
        Protected Overrides Sub OnDeserialized(context As Runtime.Serialization.StreamingContext)
            MyBase.OnDeserialized(context)
            AddHandler Me.PropertyChanged, AddressOf BO_PropertyChanged
        End Sub

        Private Sub BO_PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Handles Me.PropertyChanged
            pbs.BO.Rules.CalculationRules.Calculator(sender, e)
        End Sub
#End Region

#Region " Business Properties and Methods "
        Private _DTB As String = String.Empty
        Friend Const _SUNTB As String = RFMSC_TB.MCJournalDefinition
        Private _journalType As String = String.Empty
        Private _lookup As String = String.Empty
        Private _updated As pbs.Helper.SmartDate = New pbs.Helper.SmartDate(ToDay)
        Private _journalName As String = String.Empty

        Private _conversion As String = String.Empty
        Private _dag As String = String.Empty
        Private _reportDefinition As String = String.Empty
        Private _trefSequence As String = String.Empty

        <System.ComponentModel.DataObjectField(True, False)> _
    <Rule(Required:=True)>
        Public ReadOnly Property JournalType() As String
            Get
                CanReadProperty("JournalType", True)
                Return _journalType
            End Get
        End Property

        Public Property Lookup() As String
            Get
                CanReadProperty("Lookup", True)
                Return _lookup
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Lookup", True)
                If value Is Nothing Then value = String.Empty
                If Not _lookup.Equals(value) Then
                    _lookup = value
                    PropertyHasChanged("Lookup")
                End If
            End Set
        End Property

        <Rule(Required:=True)>
        Public Property JournalName() As String
            Get
                CanReadProperty("JournalName", True)
                Return _journalName
            End Get
            Set(ByVal value As String)
                CanWriteProperty("JournalName", True)
                If value Is Nothing Then value = String.Empty
                If Not _journalName.Equals(value) Then
                    _journalName = value
                    PropertyHasChanged("JournalName")
                End If
            End Set
        End Property

        Private _masterFields As String = String.Empty
        <CellInfo(ControlType:=Forms.CtrlType.Lookup, Tips:="Select fields, which detail lines always take from journal header")>
        Public Property MasterFields() As String
            Get
                CanReadProperty("MasterFields", True)
                Return _masterFields
            End Get
            Set(ByVal value As String)
                CanWriteProperty("MasterFields", True)
                If value Is Nothing Then value = String.Empty
                If Not _masterFields.Equals(value) Then
                    _masterFields = value
                    PropertyHasChanged("MasterFields")
                End If
            End Set
        End Property

        Private _printOnPosting As String = String.Empty
        <CellInfo(ControlType:=Forms.CtrlType.ToggleSwitch, Tips:="Phoebus will print this cash receipt when user press Post button")>
        Public Property PrintOnPosting As Boolean
            Get
                Return _printOnPosting.ToBoolean
            End Get
            Set(value As Boolean)
                If PrintOnPosting <> value Then
                    _printOnPosting = If(value, "Y", "N")
                    PropertyHasChanged("PrintOnPosting")
                End If
            End Set
        End Property

        Private _hardPostingOnly As String = String.Empty
        <CellInfo(ControlType:=Forms.CtrlType.ToggleSwitch, Tips:="Phoebus allows Post only. Hide Save button")>
        Public Property HardPostingOnly As Boolean
            Get
                Return _hardPostingOnly.ToBoolean
            End Get
            Set(value As Boolean)

                If HardPostingOnly <> value Then
                    _hardPostingOnly = If(value, "Y", "N")
                    PropertyHasChanged("HardPostingOnly")
                End If

            End Set
        End Property

        Private _data As String = String.Empty

        <CellInfo(LinkCode.Conversion, controlType:=Forms.CtrlType.ToggleSwitch, Tips:="Force transaction must enter the currency code")>
        Public Property Conversion() As Boolean
            Get
                CanReadProperty("Conversion", True)
                Return _conversion = "Y"
            End Get
            Set(ByVal value As Boolean)
                CanWriteProperty("Conversion", True)
                If Not ((_conversion = "Y") = value) Then
                    If value Then _conversion = "Y" Else _conversion = String.Empty
                    PropertyHasChanged("Conversion")
                End If
            End Set
        End Property

        <CellInfo(LinkCode.DAG)>
        Public Property Dag() As String
            Get
                CanReadProperty("Dag", True)
                Return _dag
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Dag", True)
                If value Is Nothing Then value = String.Empty
                If Not _dag.Equals(value) Then
                    _dag = value
                    PropertyHasChanged("Dag")
                End If
            End Set
        End Property

        <CellInfo(LinkCode.ReportDef)>
        Public Property ReportDefinition() As String
            Get
                CanReadProperty("ReportDefinition", True)
                Return _reportDefinition
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ReportDefinition", True)
                If value Is Nothing Then value = String.Empty
                If Not _reportDefinition.Equals(value) Then
                    _reportDefinition = value
                    PropertyHasChanged("ReportDefinition")
                End If
            End Set
        End Property

        <CellInfo("pbs.BO.SEQ", Tips:="The sequence profile will fill in the Reference right before posting.")>
        Public Property TrefSequence() As String
            Get
                CanReadProperty("TrefSequence", True)
                Return _trefSequence
            End Get
            Set(ByVal value As String)
                CanWriteProperty("TrefSequence", True)
                If value Is Nothing Then value = String.Empty
                If Not _trefSequence.Equals(value) Then
                    _trefSequence = value
                    PropertyHasChanged("TrefSequence")
                End If
            End Set
        End Property

        Private _debitCredit As String = String.Empty
        <CellInfo(Tips:="Enter D-Cash Bank Receive or C - Payable")>
        <Rule(Required:=True, RegexRule:="^[D,C]$")>
        Public Property DebitCredit() As String
            Get
                Return _debitCredit
            End Get
            Set(ByVal value As String)
                CanWriteProperty("DebitCredit", True)
                value = Nz(value, String.Empty)
                If Not (_debitCredit = value) Then
                    _debitCredit = Nz(value, String.Empty)
                    PropertyHasChanged("DebitCredit")
                End If
            End Set
        End Property

        Private _requireItemCode As String = "Y"
        <CellInfo(ControlType:=Forms.CtrlType.ToggleSwitch, Tips:="Inventory or cash receipt will need the item code")>
        Public Property RequireItemCode() As Boolean
            Get
                Return _requireItemCode.ToBoolean
            End Get
            Set(ByVal value As Boolean)
                CanWriteProperty("RequireItemCode", True)
                value = Nz(value, String.Empty)
                If Not (RequireItemCode = value) Then
                    _requireItemCode = If(value, "Y", "N")
                    PropertyHasChanged("RequireItemCode")
                End If
            End Set
        End Property

        Public ReadOnly Property Updated() As String
            Get
                CanReadProperty("Updated", True)
                Return _updated.Text
            End Get
        End Property

        Protected Overrides Function GetIdValue() As Object
            Return _journalType
        End Function

#End Region ' Business Properties and Methods

#Region " Validation Rules "

        Private Shared Function JournalTypeValidator(ByVal Target As Object, ByVal e As RuleArgs) As Boolean
            Dim _jd As JD = Target
            If _jd.JournalType.Equals(pbs.Helper.SystemJT) Then
                e.Description = String.Format(ResStr("{0} is reserved as system journal type. Select another name"), pbs.Helper.SystemJT)
                Return False
            Else
                Return True
            End If
        End Function

        Private Sub AddSharedCommonRules()

            ' JournalType rules
            ValidationRules.AddRule(AddressOf JournalTypeValidator, "JournalType", 1)
            ' Lookup rules
            ' Dag rules
            ValidationRules.AddRule(AddressOf UsrMan.DAInfoList.ContainsDAG, "Dag")
            ValidationRules.AddRule(AddressOf RDInfoList.LiveOrEmptyCode, "ReportDefinition")

        End Sub

        Protected Overrides Sub AddBusinessRules()

            AddSharedCommonRules()

            For Each _field As ClassField In ClassSchema(Of JD)._fieldList
                If _field.Required Then
                    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, _field.FieldName, 0)
                End If
                If Not String.IsNullOrEmpty(_field.RegexPattern) Then
                    ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.RegExMatch, New RegExRuleArgs(_field.FieldName, _field.RegexPattern), 1)
                End If
                '----------using lookup, if no user lookup defined, fallback to predefined by developer----------------------------
                If CATMAPInfoList.ContainsCode(_field) Then
                    ValidationRules.AddRule(AddressOf LKUInfoList.ContainsLiveCode, _field.FieldName, 2)
                Else
                    'Select Case _field.FieldName
                    '        Case "Status"
                    '            ValidationRules.AddRule(Of LOC, AnalRuleArg)(AddressOf LOOKUPInfoList.ContainsSysCode, New AnalRuleArg(_field.FieldName, SysCats.LocationStatus))
                    'End Select
                End If
            Next
            Rules.BusinessRules.RegisterBusinessRules(Me)

            MyBase.AddBusinessRules()
        End Sub

#End Region ' Validation Rules

#Region " Authorization Rules "

        Protected Overrides Sub AddAuthorizationRules()
        End Sub

#End Region ' Authorization Rules

#Region " Factory Methods "

        Private Sub New()
            ' require use of factory method 
            _DTB = Context.CurrentBECode
        End Sub

        Public Shared Function BlankJD() As JD
            Dim _obj = New JD
            _obj.ValidationRules.CheckRules()
            Return _obj
        End Function

        Public Shared Function NewJD(ByVal journalType As String) As JD
            ' Used for DAG Busines Object check duplicate of key
            If KeyDuplicated(journalType) Then
                ExceptionThower.BusinessRuleStop(ResStr(ResStrConst.NOACCESS), ResStr("JD"))
            End If
            Return DataPortal.Create(Of JD)(New Criteria(journalType))
        End Function

        Public Shared Function NewBO(ByVal journalType As String) As JD
            Return NewJD(journalType)
        End Function

        Public Shared Function GetJD(ByVal journalType As String) As JD

            Return DataPortal.Fetch(Of JD)(New Criteria(journalType))
        End Function

        Public Shared Function GetBO(ByVal journalType As String) As JD
            Return GetJD(journalType)
        End Function

        Public Shared Sub DeleteJD(ByVal journalType As String)

            DataPortal.Delete(New Criteria(journalType))
        End Sub

        Public Overrides Function Save() As JD

            If Not IsDirty Then
                ExceptionThower.BusinessRuleStop(ResStr(ResStrConst.NOTDIRTY))
            End If
            If Not IsSavable Then
                ExceptionThower.BusinessRuleStop(String.Format(ResStr(ResStrConst.INVALID), ResStr("JD")))
            End If
            Me.ApplyEdit()
            JDInfoList.InvalidateCache()
            Return MyBase.Save()
        End Function

        Public Function CloneJD(ByVal CloningCode As String) As JD
            ' Used for DAG Busines Object check duplicate of key
            If KeyDuplicated(CloningCode) Then
                ExceptionThower.BusinessRuleStop(ResStr(ResStrConst.NOACCESS), ResStr("JD"))
            End If

            Dim cloning As JD = MyBase.Clone
            cloning._DTB = Context.CurrentBECode
            cloning._journalType = CloningCode
            cloning.MarkNew()
            cloning.ValidationRules.CheckRules()
            cloning.ApplyEdit()
            Return cloning
        End Function


#End Region ' Factory Methods

#Region " Data Access "

#Region " Criteria "

        <Serializable()> _
        Private Class Criteria

            Public JournalType As String

            Public Sub New(ByVal journalType As String)
                Me.JournalType = journalType
            End Sub
        End Class

#End Region 'Criteria

        <RunLocal()> _
        Private Overloads Sub DataPortal_Create(ByVal criteria As Criteria)
            _journalType = criteria.JournalType
            ValidationRules.CheckRules()
        End Sub

        Private Overloads Sub DataPortal_Fetch(ByVal criteria As Criteria)
              Using ctx = ConnectionFactory.GetDBConnection(True)
                Using cm = ctx.Get_RFMSCFetchCommand(_DTB, RFMSC_TB.MCJournalDefinition, criteria.JournalType)

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
            _journalType = dr.GetString("KEY_FIELDS").TrimEnd
            _lookup = dr.GetString("LOOKUP").TrimEnd
            _updated.Text = dr.GetString("UPDATED").TrimEnd

            Dim data = dr.GetString("PSM_DATA").TrimEnd.Decompress
            Try

                Dim xele = XElement.Parse(data)

                _journalName = DNz(xele...<_journalName>.Value, String.Empty)
                _conversion = DNz(xele...<_conversion>.Value, String.Empty)
                _dag = DNz(xele...<_dag>.Value, String.Empty)
                _reportDefinition = DNz(xele...<_reportDefinition>.Value, String.Empty)
                _trefSequence = DNz(xele...<_trefSequence>.Value, String.Empty)
                _debitCredit = DNz(xele...<_debitCredit>.Value, String.Empty)
                _masterFields = DNz(xele...<_masterFields>.Value, String.Empty)
                _printOnPosting = DNz(xele...<_printOnPosting>.Value, String.Empty)
                _hardPostingOnly = DNz(xele...<_hardPostingOnly>.Value, String.Empty)
                _requireItemCode = DNz(xele...<_requireItemCode>.Value, "Y")

            Catch ex As Exception
            End Try

        End Sub

        Private Shared _lockObj As New Object
        Protected Overrides Sub DataPortal_Insert()
            SyncLock _lockObj
                Using ctx = ConnectionFactory.GetDBConnection(True)
                    Using cm = ctx.CreateSQLCommand

                        cm.CommandType = CommandType.StoredProcedure
                        cm.CommandText = "pbs_RFMSC_InsertUpdate"

                        AddInsertParameters(cm)
                        cm.ExecuteNonQuery()

                        MarkOld()
                    End Using
                End Using
            End SyncLock
        End Sub

        Private Sub AddInsertParameters(ByVal cm As IDBCommand)

            Dim _key As String = _journalType

            Dim _data = <rules>
                            <_journalName><%= _journalName %></_journalName>
                            <_conversion><%= _conversion %></_conversion>
                            <_dag><%= _dag %></_dag>
                            <_reportDefinition><%= _reportDefinition %></_reportDefinition>
                            <_trefSequence><%= _trefSequence %></_trefSequence>
                            <_debitCredit><%= _debitCredit %></_debitCredit>
                            <_masterFields><%= _masterFields %></_masterFields>
                            <_printOnPosting><%= _printOnPosting %></_printOnPosting>
                            <_hardPostingOnly><%= _hardPostingOnly %></_hardPostingOnly>
                            <_requireItemCode><%= _requireItemCode %></_requireItemCode>
                        </rules>

            cm.Parameters.AddWithValue("@PBS_DB", _DTB)
            cm.Parameters.AddWithValue("@PBS_TB", RFMSC_TB.MCJournalDefinition)
            cm.Parameters.AddWithValue("@KEY_FIELDS", _key.Trim)
            cm.Parameters.AddWithValue("@LOOKUP", _lookup.Trim)
            cm.Parameters.AddWithValue("@UPDATED", ToDay().ToSunDate)
            cm.Parameters.AddWithValue("@PSM_DATA", _data.ToString.Compress)
        End Sub

        Protected Overrides Sub DataPortal_Update()
            DataPortal_Insert()
        End Sub

        Protected Overrides Sub DataPortal_DeleteSelf()
            DataPortal_Delete(New Criteria(_journalType))
        End Sub

        Private Overloads Sub DataPortal_Delete(ByVal criteria As Criteria)
            Using ctx = ConnectionFactory.GetDBConnection(True)
                Using cm = ctx.CreateSQLCommand

                    cm.CommandText = <Script>DELETE FROM pbs_RFMSC WHERE PBS_DB='<%= _DTB %>' AND PBS_TB='<%= _SUNTB %>' AND KEY_FIELDS='<%= criteria.JournalType %>'</Script>.Value.Trim
                    cm.ExecuteNonQuery()
                End Using
            End Using

        End Sub


#End Region 'Data Access

#Region " Exists "

        Public Shared Function Exists(ByVal journalType As String) As Boolean
            Return JDInfoList.ContainsCode(journalType)
        End Function

        Public Shared Function KeyDuplicated(ByVal pJournalType As String) As Boolean
            If String.IsNullOrEmpty(pJournalType) Then Return False
            Dim SqlText = <SqlText>SELECT COUNT(*) FROM pbs_RFMSC WHERE PBS_DB='<%= Context.CurrentBECode %>'  AND PBS_TB='<%= RFMSC_TB.MCJournalDefinition %>' AND KEY_FIELDS='<%= pJournalType %>'</SqlText>.Value.Trim
            Return SQLCommander.GetScalarInteger(SqlText) > 0
        End Function

#End Region

#Region " IGenpart "
        Public Function CloneBO(newId As String) As Object Implements Interfaces.IGenPartObject.CloneBO
            Return CloneJD(newId)
        End Function

        Public Function getBO1(ByVal id As String) As Object Implements Interfaces.IGenPartObject.GetBO
            Return GetJD(id)
        End Function

        Public Function myCommands() As String() Implements Interfaces.IGenPartObject.myCommands
            Return Action.StandardReferenceCommands
        End Function

        Public Function myFullName() As String Implements Interfaces.IGenPartObject.myFullName
            Return Me.GetType.ToString
        End Function

        Public Function myName() As String Implements Interfaces.IGenPartObject.myName
            Return "JD"
        End Function

        Public Function myQueryList() As IList Implements Interfaces.IGenPartObject.myQueryList
            Return JDInfoList.GetJDInfoList
        End Function
#End Region

        Public Function Get_DOL_Reference() As String Implements IDocLink.Get_DOL_Reference
            Return String.Format("{0}#{1}", Get_TransType, _journalType)
        End Function

        Public Function Get_TransType() As String Implements IDocLink.Get_TransType
            Return GetType(JD).ToString.Leaf
        End Function

#Region "ILookupProvider"
        Public Function GetLookupListForProperty(pName As String) As IEnumerable Implements ILookupProvider.GetLookupListForProperty
            If pName.Equals("MasterFields") Then
                Dim fields = BOFactory.GetPropInfoList(GetType(pbs.BO.MC.JE))
                Return fields
            Else
                Return Nothing
            End If
        End Function

        Public Function GetLookupURL(pName As String) As String Implements ILookupProvider.GetLookupURL
            Return Nothing
        End Function
#End Region


    End Class

End Namespace
