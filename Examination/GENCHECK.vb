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
    <DB(TableName:="pbs_MC_GENCHECK_{XXX}")>
    Public Class GENCHECK
        Inherits Csla.BusinessBase(Of GENCHECK)
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
        <CellInfo("pbs.BO.MC.PATIENT", GroupName:="General Check", Tips:="Enter patient code.")>
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
        <CellInfo("pbs.BO.MC.CHECKIN", GroupName:="General Check", Tips:="Enter check-in code.")>
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

        Private _waitingNumber As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        <CellInfo(GroupName:="General Check", Tips:="Waiting number of patient")>
        Public Property WaitingNumber() As String
            Get
                Return _waitingNumber.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("WaitingNumber", True)
                If value Is Nothing Then value = String.Empty
                If Not _waitingNumber.Equals(value) Then
                    _waitingNumber.Text = value
                    PropertyHasChanged("WaitingNumber")
                End If
            End Set
        End Property

        Private _pulse As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        <CellInfo(GroupName:="General Check", Tips:="Patient's pulse (times/min).")>
        Public Property Pulse() As String
            Get
                Return _pulse.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Pulse", True)
                If value Is Nothing Then value = String.Empty
                If Not _pulse.Equals(value) Then
                    _pulse.Text = value
                    PropertyHasChanged("Pulse")
                End If
            End Set
        End Property

        Private _temperature As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="General Check", Tips:="Patient's temperature (celsius).")>
        Public Property Temperature() As String
            Get
                Return _temperature.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Temperature", True)
                If value Is Nothing Then value = String.Empty
                If Not _temperature.Equals(value) Then
                    _temperature.Text = value
                    PropertyHasChanged("Temperature")
                End If
            End Set
        End Property

        Private _bloodPressure As String = String.Empty
        <CellInfo(GroupName:="General Check", Tips:="Patient's blood pressure (mmHg).")>
        Public Property BloodPressure() As String
            Get
                Return _bloodPressure
            End Get
            Set(ByVal value As String)
                CanWriteProperty("BloodPressure", True)
                If value Is Nothing Then value = String.Empty
                If Not _bloodPressure.Equals(value) Then
                    _bloodPressure = value
                    PropertyHasChanged("BloodPressure")
                End If
            End Set
        End Property

        Private _breathingRate As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        <CellInfo(GroupName:="General Check", Tips:="Patient's breathing rate (times/min).")>
        Public Property BreathingRate() As String
            Get
                Return _breathingRate.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("BreathingRate", True)
                If value Is Nothing Then value = String.Empty
                If Not _breathingRate.Equals(value) Then
                    _breathingRate.Text = value
                    PropertyHasChanged("BreathingRate")
                End If
            End Set
        End Property

        Private _weight As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="General Check", Tips:="Patient's weight (Kg).")>
        Public Property Weight() As String
            Get
                Return _weight.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Weight", True)
                If value Is Nothing Then value = String.Empty
                If Not _weight.Equals(value) Then
                    _weight.Text = value
                    PropertyHasChanged("Weight")
                End If
            End Set
        End Property

        Private _height As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="General Check", Tips:="Patient's height (cm).")>
        Public Property Height() As String
            Get
                Return _height.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Height", True)
                If value Is Nothing Then value = String.Empty
                If Not _height.Equals(value) Then
                    _height.Text = value
                    PropertyHasChanged("Height")
                End If
            End Set
        End Property

        Private _bloodgroup As String = String.Empty
        <CellInfo(GroupName:="General Check", Tips:="Patient's blood group.")>
        Public Property Bloodgroup() As String
            Get
                Return _bloodgroup
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Bloodgroup", True)
                If value Is Nothing Then value = String.Empty
                If Not _bloodgroup.Equals(value) Then
                    _bloodgroup = value
                    PropertyHasChanged("Bloodgroup")
                End If
            End Set
        End Property

        Private _checkBy As String = String.Empty
        <CellInfo("pbs.BO.HR.EMP", GroupName:="General Check", Tips:="Enter checker's name.")>
        Public Property CheckBy() As String
            Get
                Return _checkBy
            End Get
            Set(ByVal value As String)
                CanWriteProperty("CheckBy", True)
                If value Is Nothing Then value = String.Empty
                If Not _checkBy.Equals(value) Then
                    _checkBy = value
                    PropertyHasChanged("CheckBy")
                End If
            End Set
        End Property

        Private _checkDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo(GroupName:="General Check", Tips:="Enter check date.")>
        Public Property CheckDate() As String
            Get
                Return _checkDate.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("CheckDate", True)
                If value Is Nothing Then value = String.Empty
                If Not _checkDate.Equals(value) Then
                    _checkDate.Text = value
                    PropertyHasChanged("CheckDate")
                End If
            End Set
        End Property

        Private _extDecs5 As String = String.Empty
        Public Property ExtDecs5() As String
            Get
                Return _extDecs5
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ExtDecs5", True)
                If value Is Nothing Then value = String.Empty
                If Not _extDecs5.Equals(value) Then
                    _extDecs5 = value
                    PropertyHasChanged("ExtDecs5")
                End If
            End Set
        End Property

        Private _extDecs4 As String = String.Empty
        Public Property ExtDecs4() As String
            Get
                Return _extDecs4
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ExtDecs4", True)
                If value Is Nothing Then value = String.Empty
                If Not _extDecs4.Equals(value) Then
                    _extDecs4 = value
                    PropertyHasChanged("ExtDecs4")
                End If
            End Set
        End Property

        Private _extDecs3 As String = String.Empty
        Public Property ExtDecs3() As String
            Get
                Return _extDecs3
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ExtDecs3", True)
                If value Is Nothing Then value = String.Empty
                If Not _extDecs3.Equals(value) Then
                    _extDecs3 = value
                    PropertyHasChanged("ExtDecs3")
                End If
            End Set
        End Property

        Private _extDecs2 As String = String.Empty
        Public Property ExtDecs2() As String
            Get
                Return _extDecs2
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ExtDecs2", True)
                If value Is Nothing Then value = String.Empty
                If Not _extDecs2.Equals(value) Then
                    _extDecs2 = value
                    PropertyHasChanged("ExtDecs2")
                End If
            End Set
        End Property

        Private _extDecs1 As String = String.Empty
        Public Property ExtDecs1() As String
            Get
                Return _extDecs1
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ExtDecs1", True)
                If value Is Nothing Then value = String.Empty
                If Not _extDecs1.Equals(value) Then
                    _extDecs1 = value
                    PropertyHasChanged("ExtDecs1")
                End If
            End Set
        End Property

        Private _extVal5 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public Property ExtVal5() As String
            Get
                Return _extVal5.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ExtVal5", True)
                If value Is Nothing Then value = String.Empty
                If Not _extVal5.Equals(value) Then
                    _extVal5.Text = value
                    PropertyHasChanged("ExtVal5")
                End If
            End Set
        End Property

        Private _extVal4 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public Property ExtVal4() As String
            Get
                Return _extVal4.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ExtVal4", True)
                If value Is Nothing Then value = String.Empty
                If Not _extVal4.Equals(value) Then
                    _extVal4.Text = value
                    PropertyHasChanged("ExtVal4")
                End If
            End Set
        End Property

        Private _extVal3 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public Property ExtVal3() As String
            Get
                Return _extVal3.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ExtVal3", True)
                If value Is Nothing Then value = String.Empty
                If Not _extVal3.Equals(value) Then
                    _extVal3.Text = value
                    PropertyHasChanged("ExtVal3")
                End If
            End Set
        End Property

        Private _extVal2 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public Property ExtVal2() As String
            Get
                Return _extVal2.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ExtVal2", True)
                If value Is Nothing Then value = String.Empty
                If Not _extVal2.Equals(value) Then
                    _extVal2.Text = value
                    PropertyHasChanged("ExtVal2")
                End If
            End Set
        End Property

        Private _extVal1 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public Property ExtVal1() As String
            Get
                Return _extVal1.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ExtVal1", True)
                If value Is Nothing Then value = String.Empty
                If Not _extVal1.Equals(value) Then
                    _extVal1.Text = value
                    PropertyHasChanged("ExtVal1")
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

            For Each _field As ClassField In ClassSchema(Of GENCHECK)._fieldList
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
            _checkDate.Text = ToDay.ToSunDate
            _checkBy = Context.CurrentUserCode
        End Sub

        Public Shared Function BlankGENCHECK() As GENCHECK
            Return New GENCHECK
        End Function

        Public Shared Function NewGENCHECK(ByVal pLineNo As String) As GENCHECK
            If KeyDuplicated(pLineNo) Then ExceptionThower.BusinessRuleStop(String.Format(ResStr(ResStrConst.NOACCESS), ResStr("GENCHECK")))
            Return DataPortal.Create(Of GENCHECK)(New Criteria(pLineNo))
        End Function

        Public Shared Function NewBO(ByVal ID As String) As GENCHECK
            Dim pLineNo As String = ID.Trim

            Return NewGENCHECK(pLineNo)
        End Function

        Public Shared Function GetGENCHECK(ByVal pLineNo As String) As GENCHECK
            Return DataPortal.Fetch(Of GENCHECK)(New Criteria(pLineNo))
        End Function

        Public Shared Function GetBO(ByVal ID As String) As GENCHECK
            Dim pLineNo As String = ID.Trim

            Return GetGENCHECK(pLineNo)
        End Function

        Public Shared Sub DeleteGENCHECK(ByVal pLineNo As String)
            DataPortal.Delete(New Criteria(pLineNo))
        End Sub

        Public Overrides Function Save() As GENCHECK
            If Not IsDirty Then ExceptionThower.NotDirty(ResStr(ResStrConst.NOTDIRTY))
            If Not IsSavable Then Throw New Csla.Validation.ValidationException(String.Format(ResStr(ResStrConst.INVALID), ResStr("GENCHECK")))

            Me.ApplyEdit()
            GENCHECKInfoList.InvalidateCache()
            Return MyBase.Save()
        End Function

        Public Function CloneGENCHECK(ByVal pLineNo As String) As GENCHECK

            If GENCHECK.KeyDuplicated(pLineNo) Then ExceptionThower.BusinessRuleStop(ResStr(ResStrConst.CreateAlreadyExists), Me.GetType.ToString.Leaf.Translate)

            Dim cloningGENCHECK As GENCHECK = MyBase.Clone
            cloningGENCHECK._lineNo = pLineNo

            'Todo:Remember to reset status of the new object here 
            cloningGENCHECK.MarkNew()
            cloningGENCHECK.ApplyEdit()

            cloningGENCHECK.ValidationRules.CheckRules()

            Return cloningGENCHECK
        End Function

#End Region ' Factory Methods

#Region " Data Access "

        <Serializable()> _
        Private Class Criteria
            Public _lineNo As String = String.Empty

            Public Sub New(ByVal pLineNo As String)
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

                    cm.CommandText = <SqlText>SELECT * FROM pbs_MC_GENCHECK_<%= _DTB %> WHERE LINE_NO= '<%= criteria._lineNo %>' </SqlText>.Value.Trim

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
            _waitingNumber.Text = dr.GetInt32("WAITING_NUMBER")
            _pulse.Text = dr.GetInt32("PULSE")
            _temperature.Text = dr.GetDecimal("TEMPERATURE")
            _bloodPressure = dr.GetString("BLOOD_PRESSURE")
            _breathingRate.Text = dr.GetInt32("BREATHING_RATE")
            _weight.Text = dr.GetDecimal("WEIGHT")
            _height.Text = dr.GetDecimal("HEIGHT")
            _bloodgroup = dr.GetString("BLOOD_GROUP").TrimEnd
            _checkDate.Text = dr.GetInt32("CHECK_DATE")
            _checkBy = dr.GetString("CHECK_BY")
            _extDecs5 = dr.GetString("EXT_DECS5").TrimEnd
            _extDecs4 = dr.GetString("EXT_DECS4").TrimEnd
            _extDecs3 = dr.GetString("EXT_DECS3").TrimEnd
            _extDecs2 = dr.GetString("EXT_DECS2").TrimEnd
            _extDecs1 = dr.GetString("EXT_DECS1").TrimEnd
            _extVal5.Text = dr.GetDecimal("EXT_VAL5")
            _extVal4.Text = dr.GetDecimal("EXT_VAL4")
            _extVal3.Text = dr.GetDecimal("EXT_VAL3")
            _extVal2.Text = dr.GetDecimal("EXT_VAL2")
            _extVal1.Text = dr.GetDecimal("EXT_VAL1")
            _updated.Text = dr.GetInt32("UPDATED")
            _updatedBy = dr.GetString("UPDATED_BY").TrimEnd

        End Sub

        Private Shared _lockObj As New Object
        Protected Overrides Sub DataPortal_Insert()
            SyncLock _lockObj
                Using ctx = ConnectionFactory.GetDBConnection(True)
                    Using cm = ctx.CreateSQLCommand

                        cm.CommandType = CommandType.StoredProcedure
                        cm.CommandText = String.Format("pbs_MC_GENCHECK_{0}_Insert", _DTB)

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
            cm.Parameters.AddWithValue("@WAITING_NUMBER", _waitingNumber.DBValue)
            cm.Parameters.AddWithValue("@PULSE", _pulse.DBValue)
            cm.Parameters.AddWithValue("@TEMPERATURE", _temperature.DBValue)
            cm.Parameters.AddWithValue("@BLOOD_PRESSURE", _bloodPressure.Trim)
            cm.Parameters.AddWithValue("@BREATHING_RATE", _breathingRate.DBValue)
            cm.Parameters.AddWithValue("@WEIGHT", _weight.DBValue)
            cm.Parameters.AddWithValue("@HEIGHT", _height.DBValue)
            cm.Parameters.AddWithValue("@BLOOD_GROUP", _bloodgroup.Trim)
            cm.Parameters.AddWithValue("@CHECK_BY", _checkBy.Trim)
            cm.Parameters.AddWithValue("@CHECK_DATE", _checkDate.DBValue)
            cm.Parameters.AddWithValue("@EXT_DECS5", _extDecs5.Trim)
            cm.Parameters.AddWithValue("@EXT_DECS4", _extDecs4.Trim)
            cm.Parameters.AddWithValue("@EXT_DECS3", _extDecs3.Trim)
            cm.Parameters.AddWithValue("@EXT_DECS2", _extDecs2.Trim)
            cm.Parameters.AddWithValue("@EXT_DECS1", _extDecs1.Trim)
            cm.Parameters.AddWithValue("@EXT_VAL5", _extVal5.DBValue)
            cm.Parameters.AddWithValue("@EXT_VAL4", _extVal4.DBValue)
            cm.Parameters.AddWithValue("@EXT_VAL3", _extVal3.DBValue)
            cm.Parameters.AddWithValue("@EXT_VAL2", _extVal2.DBValue)
            cm.Parameters.AddWithValue("@EXT_VAL1", _extVal1.DBValue)
            cm.Parameters.AddWithValue("@UPDATED", ToDay.ToSunDate)
            cm.Parameters.AddWithValue("@UPDATED_BY", Context.CurrentUserCode)
        End Sub


        Protected Overrides Sub DataPortal_Update()
            SyncLock _lockObj
                Using ctx = ConnectionFactory.GetDBConnection(True)
                    Using cm = ctx.CreateSQLCommand

                        cm.CommandType = CommandType.StoredProcedure
                        cm.CommandText = String.Format("pbs_MC_GENCHECK_{0}_Update", _DTB)

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


                    cm.CommandText = <SqlText>DELETE FROM pbs_MC_GENCHECK_<%= _DTB %> WHERE LINE_NO= '<%= criteria._lineNo %>' </SqlText>.Value.Trim
                    cm.ExecuteNonQuery()

                End Using
            End Using

        End Sub

        Protected Overrides Sub DataPortal_OnDataPortalInvokeComplete(ByVal e As Csla.DataPortalEventArgs)
            If Csla.ApplicationContext.ExecutionLocation = ExecutionLocations.Server Then
                GENCHECKInfoList.InvalidateCache()
            End If
        End Sub


#End Region 'Data Access                           

#Region " Exists "
        Public Shared Function Exists(ByVal pLineNo As String) As Boolean
            Return GENCHECKInfoList.ContainsCode(pLineNo)
        End Function

        Public Shared Function KeyDuplicated(ByVal pLineNo As String) As Boolean
            Dim SqlText = <SqlText>SELECT COUNT(*) FROM pbs_MC_GENCHECK_<%= Context.CurrentBECode %> WHERE LINE_NO= '<%= pLineNo %>'</SqlText>.Value.Trim
            Return SQLCommander.GetScalarInteger(SqlText) > 0
        End Function
#End Region

#Region " IGenpart "

        Public Function CloneBO(ByVal id As String) As Object Implements Interfaces.IGenPartObject.CloneBO
            Return CloneGENCHECK(id)
        End Function

        Public Function getBO1(ByVal id As String) As Object Implements Interfaces.IGenPartObject.GetBO
            Return GetBO(id)
        End Function

        Public Function myCommands() As String() Implements Interfaces.IGenPartObject.myCommands
            Return pbs.Helper.Action.StandardReferenceCommands
        End Function

        Public Function myFullName() As String Implements Interfaces.IGenPartObject.myFullName
            Return GetType(GENCHECK).ToString
        End Function

        Public Function myName() As String Implements Interfaces.IGenPartObject.myName
            Return GetType(GENCHECK).ToString.Leaf
        End Function

        Public Function myQueryList() As IList Implements Interfaces.IGenPartObject.myQueryList
            Return GENCHECKInfoList.GetGENCHECKInfoList
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