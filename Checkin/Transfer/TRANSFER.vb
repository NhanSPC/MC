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
    <DB(TableName:="pbs_MC_TRANSFER_{XXX}")>
    <PhoebusCommand(Desc:="Patient Transfer")>
    Public Class TRANSFER
        Inherits Csla.BusinessBase(Of TRANSFER)
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
                    Dim cin = CHECKINInfoList.GetCHECKINInfo(_checkinNo.ToString)
                    _patientCode = cin.PatientCode
                    PropertyHasChanged("PatientCode")

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

        Private _patientCode As String = String.Empty
        <CellInfo("pbs.BO.MC.PATIENT", Tips:="Enter patient code")>
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
        <CellInfo("pbs.BO.MC.CHECKIN", Tips:="Enter check-in number")>
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

        Private _transferGroup As String = String.Empty
        <CellInfo(Tips:="Enter type of transfer. To other department or to other hospital.")>
        Public Property TransferGroup() As String
            Get
                Return _transferGroup
            End Get
            Set(ByVal value As String)
                CanWriteProperty("TransferGroup", True)
                If value Is Nothing Then value = String.Empty
                If Not _transferGroup.Equals(value) Then
                    _transferGroup = value
                    PropertyHasChanged("TransferGroup")
                End If
            End Set
        End Property

        Private _transToDept As String = String.Empty
        <CellInfo("MEDI_DEPT", Tips:="Enter transfer to department code")>
        Public Property TransToDept() As String
            Get
                Return _transToDept
            End Get
            Set(ByVal value As String)
                CanWriteProperty("TransToDept", True)
                If value Is Nothing Then value = String.Empty
                If Not _transToDept.Equals(value) Then
                    _transToDept = value
                    PropertyHasChanged("TransToDept")
                End If
            End Set
        End Property

        Private _transTime As pbs.Helper.SmartTime = New pbs.Helper.SmartTime()
        <CellInfo("HOUR", Tips:="Enter transfer time")>
        Public Property TransTime() As String
            Get
                Return _transTime.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("TransTime", True)
                If value Is Nothing Then value = String.Empty
                If Not _transTime.Equals(value) Then
                    _transTime.Text = value
                    PropertyHasChanged("TransTime")
                End If
            End Set
        End Property

        Private _transDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo("CALENDAR", Tips:="Enter transfer date")>
        Public Property TransDate() As String
            Get
                Return _transDate.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("TransDate", True)
                If value Is Nothing Then value = String.Empty
                If Not _transDate.Equals(value) Then
                    _transDate.Text = value
                    PropertyHasChanged("TransDate")
                End If
            End Set
        End Property

        Private _deptTreatmentDays As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        <CellInfo(Tips:="Enter total treament day")>
        Public Property DeptTreatmentDays() As String
            Get
                Return _deptTreatmentDays.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("DeptTreatmentDays", True)
                If value Is Nothing Then value = String.Empty
                If Not _deptTreatmentDays.Equals(value) Then
                    _deptTreatmentDays.Text = value
                    PropertyHasChanged("DeptTreatmentDays")
                End If
            End Set
        End Property

        Private _transferType As String = String.Empty
        <CellInfo("TRANS_TYPE", Tips:="Chose type of transfer from list")>
        Public Property TransferType() As String
            Get
                Return _transferType
            End Get
            Set(ByVal value As String)
                CanWriteProperty("TransferType", True)
                If value Is Nothing Then value = String.Empty
                If Not _transferType.Equals(value) Then
                    _transferType = value
                    PropertyHasChanged("TransferType")
                End If
            End Set
        End Property

        Private _transferTo As String = String.Empty
        <CellInfo(Tips:="Enter transfer to name")>
        Public Property TransferTo() As String
            Get
                Return _transferTo
            End Get
            Set(ByVal value As String)
                CanWriteProperty("TransferTo", True)
                If value Is Nothing Then value = String.Empty
                If Not _transferTo.Equals(value) Then
                    _transferTo = value
                    PropertyHasChanged("TransferTo")
                End If
            End Set
        End Property

        Private _transferReason As String = String.Empty
        <CellInfo(Tips:="Enter reason of transfer")>
        Public Property TransferReason() As String
            Get
                Return _transferReason
            End Get
            Set(ByVal value As String)
                CanWriteProperty("TransferReason", True)
                If value Is Nothing Then value = String.Empty
                If Not _transferReason.Equals(value) Then
                    _transferReason = value
                    PropertyHasChanged("TransferReason")
                End If
            End Set
        End Property

        Private _transportation As String = String.Empty
        <CellInfo(Tips:="Transportation")>
        Public Property Transportation() As String
            Get
                Return _transportation
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Transportation", True)
                If value Is Nothing Then value = String.Empty
                If Not _transportation.Equals(value) Then
                    _transportation = value
                    PropertyHasChanged("Transportation")
                End If
            End Set
        End Property

        Private _transporter As String = String.Empty
        <CellInfo("pbs.BO.HR.EMP", Tips:="Enter transporter code")>
        Public Property Transporter() As String
            Get
                Return _transporter
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Transporter", True)
                If value Is Nothing Then value = String.Empty
                If Not _transporter.Equals(value) Then
                    _transporter = value
                    PropertyHasChanged("Transporter")
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

            For Each _field As ClassField In ClassSchema(Of TRANSFER)._fieldList
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

        Public Shared Function BlankTRANSFER() As TRANSFER
            Return New TRANSFER
        End Function

        Public Shared Function NewTRANSFER(ByVal pLineNo As String) As TRANSFER
            Return DataPortal.Create(Of TRANSFER)(New Criteria(pLineNo))
        End Function

        Public Shared Function NewBO(ByVal ID As String) As TRANSFER
            Dim pLineNo As String = ID.Trim

            Return NewTRANSFER(pLineNo)
        End Function

        Public Shared Function GetTRANSFER(ByVal pLineNo As String) As TRANSFER
            Return DataPortal.Fetch(Of TRANSFER)(New Criteria(pLineNo))
        End Function

        Public Shared Function GetBO(ByVal ID As String) As TRANSFER
            Dim pLineNo As String = ID.Trim

            Return GetTRANSFER(pLineNo)
        End Function

        Public Shared Sub DeleteTRANSFER(ByVal pLineNo As String)
            DataPortal.Delete(New Criteria(pLineNo))
        End Sub

        Public Overrides Function Save() As TRANSFER
            If Not IsDirty Then ExceptionThower.NotDirty(ResStr(ResStrConst.NOTDIRTY))
            If Not IsSavable Then Throw New Csla.Validation.ValidationException(String.Format(ResStr(ResStrConst.INVALID), ResStr("TRANSFER")))

            Me.ApplyEdit()
            TRANSFERInfoList.InvalidateCache()
            Return MyBase.Save()
        End Function

        Public Function CloneTRANSFER(ByVal pLineNo As String) As TRANSFER

            If TRANSFER.KeyDuplicated(pLineNo) Then ExceptionThower.BusinessRuleStop(ResStr(ResStrConst.CreateAlreadyExists), Me.GetType.ToString.Leaf.Translate)

            Dim cloningTRANSFER As TRANSFER = MyBase.Clone
            cloningTRANSFER._lineNo = pLineNo.ToInteger
            cloningTRANSFER._DTB = Context.CurrentBECode
            'Todo:Remember to reset status of the new object here 
            cloningTRANSFER.MarkNew()
            cloningTRANSFER.ApplyEdit()

            cloningTRANSFER.ValidationRules.CheckRules()

            Return cloningTRANSFER
        End Function

#End Region ' Factory Methods

#Region " Data Access "

        <Serializable()>
        Private Class Criteria
            Public _lineNo As Integer

            Public Sub New(ByVal pLineNo As String)
                _lineNo = pLineNo.ToInteger

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

                    cm.CommandText = <SqlText>SELECT * FROM pbs_MC_TRANSFER_<%= _DTB %> WHERE LINE_NO= '<%= criteria._lineNo %>' </SqlText>.Value.Trim

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
            _transferGroup = dr.GetString("TRANSFER_GROUP").TrimEnd
            _transToDept = dr.GetString("TRANS_TO_DEPT").TrimEnd
            _transTime.Text = dr.GetInt32("TRANS_TIME")
            _transDate.Text = dr.GetInt32("TRANS_DATE")
            _deptTreatmentDays.Text = dr.GetInt32("DEPT_TREATMENT_DAYS")
            _transferType = dr.GetString("TRANSFER_TYPE").TrimEnd
            _transferTo = dr.GetString("TRANSFER_TO").TrimEnd
            _transferReason = dr.GetString("TRANSFER_REASON").TrimEnd
            _transportation = dr.GetString("TRANSPORTATION").TrimEnd
            _transporter = dr.GetString("TRANSPORTER").TrimEnd
            _updated.Text = dr.GetInt32("UPDATED")
            _updatedBy = dr.GetString("UPDATED_BY").TrimEnd

        End Sub

        Private Shared _lockObj As New Object
        Protected Overrides Sub DataPortal_Insert()
            SyncLock _lockObj
                Using ctx = ConnectionFactory.GetDBConnection(True)
                    Using cm = ctx.CreateSQLCommand

                        cm.CommandType = CommandType.StoredProcedure
                        cm.CommandText = String.Format("pbs_MC_TRANSFER_{0}_Insert", _DTB)

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
            cm.Parameters.AddWithValue("@TRANSFER_GROUP", _transferGroup.Trim)
            cm.Parameters.AddWithValue("@TRANS_TO_DEPT", _transToDept.Trim)
            cm.Parameters.AddWithValue("@TRANS_TIME", _transTime.DBValue)
            cm.Parameters.AddWithValue("@TRANS_DATE", _transDate.DBValue)
            cm.Parameters.AddWithValue("@DEPT_TREATMENT_DAYS", _deptTreatmentDays.DBValue)
            cm.Parameters.AddWithValue("@TRANSFER_TYPE", _transferType.Trim)
            cm.Parameters.AddWithValue("@TRANSFER_TO", _transferTo.Trim)
            cm.Parameters.AddWithValue("@TRANSFER_REASON", _transferReason.Trim)
            cm.Parameters.AddWithValue("@TRANSPORTATION", _transportation.Trim)
            cm.Parameters.AddWithValue("@TRANSPORTER", _transporter.Trim)
            cm.Parameters.AddWithValue("@UPDATED", ToDay.ToSunDate)
            cm.Parameters.AddWithValue("@UPDATED_BY", Context.CurrentUserCode)
        End Sub


        Protected Overrides Sub DataPortal_Update()
            SyncLock _lockObj
                Using ctx = ConnectionFactory.GetDBConnection(True)
                    Using cm = ctx.CreateSQLCommand

                        cm.CommandType = CommandType.StoredProcedure
                        cm.CommandText = String.Format("pbs_MC_TRANSFER_{0}_Update", _DTB)

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

                    cm.CommandText = <SqlText>DELETE FROM pbs_MC_TRANSFER_<%= _DTB %> WHERE LINE_NO= '<%= criteria._lineNo %>' </SqlText>.Value.Trim
                    cm.ExecuteNonQuery()

                End Using
            End Using

        End Sub

        Protected Overrides Sub DataPortal_OnDataPortalInvokeComplete(ByVal e As Csla.DataPortalEventArgs)
            If Csla.ApplicationContext.ExecutionLocation = ExecutionLocations.Server Then
                TRANSFERInfoList.InvalidateCache()
            End If
        End Sub


#End Region 'Data Access                           

#Region " Exists "
        Public Shared Function Exists(ByVal pLineNo As String) As Boolean
            Return KeyDuplicated(pLineNo)
        End Function

        Public Shared Function KeyDuplicated(ByVal pLineNo As String) As Boolean
            Dim theLine = pLineNo.ToInteger
            If theLine <= 0 Then Return False
            Dim SqlText = <SqlText>SELECT COUNT(*) FROM pbs_MC_TRANSFER_<%= Context.CurrentBECode %> WHERE LINE_NO= <%= theLine %></SqlText>.Value.Trim
            Return SQLCommander.GetScalarInteger(SqlText) > 0
        End Function
#End Region

#Region " IGenpart "

        Public Function CloneBO(ByVal id As String) As Object Implements Interfaces.IGenPartObject.CloneBO
            Return CloneTRANSFER(id)
        End Function

        Public Function getBO1(ByVal id As String) As Object Implements Interfaces.IGenPartObject.GetBO
            Return GetBO(id)
        End Function

        Public Function myCommands() As String() Implements Interfaces.IGenPartObject.myCommands
            Return pbs.Helper.Action.StandardReferenceCommands
        End Function

        Public Function myFullName() As String Implements Interfaces.IGenPartObject.myFullName
            Return GetType(TRANSFER).ToString
        End Function

        Public Function myName() As String Implements Interfaces.IGenPartObject.myName
            Return GetType(TRANSFER).ToString.Leaf
        End Function

        Public Function myQueryList() As IList Implements Interfaces.IGenPartObject.myQueryList
            Return TRANSFERInfoList.GetTRANSFERInfoList
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