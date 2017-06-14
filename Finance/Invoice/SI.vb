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
    <DB(TableName:="pbs_MC_INVOICE_XXX")>
    Public Class SI
        Inherits Csla.BusinessBase(Of SI)
        Implements Interfaces.IGenPartObject
        Implements IComparable
        Implements IDocLink
        Implements Interfaces.ISupportDocumentDataSet



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
                    GetInsuranceInfo(_patientCode)
                    CalculatePayment()

                Case "DocumentDate"
                    InvoiceDate = DocumentDate

                Case "InvoiceDate"
                    InvoicePeriod = Month(InvoiceDate)

                Case "InvoiceType"
                    If _invoiceType = "C" Or _invoiceType = "c" Then
                        For Each d In Details
                            d._qty.Float = -Math.Abs(d._qty.Float)
                            d.Recalculate(0)

                        Next
                    Else
                        For Each d In Details
                            d._qty.Float = Math.Abs(d._qty.Float)
                            d.Recalculate(0)
                        Next
                    End If
                    CalculatePayment()

                Case "TotalReceived"
                    CalculatePayment()

                    'Case "HealthInsuranceNo"


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

        'calculate % and amount of money insurance cover
        Function CalculateInsurance() As Decimal
            Dim percent As Decimal
            If String.IsNullOrEmpty(_healthInsuranceNo) Then
                percent = 0
            Else
                Select Case _healthInsuranceNo.Substring(2, 1)
                    Case "1"
                        percent = 1.0
                    Case "2"
                        percent = 1.0
                    Case "3"
                        percent = 0.95
                    Case "4"
                        percent = 0.8
                    Case "5"
                        percent = 1.0
                    Case Else
                        percent = 0
                End Select
            End If

            Return percent
        End Function

#End Region

        'Calculate total payment of patient
        Friend Sub CalculatePayment()
            _totalCost.Float = 0
            _totalCoveredByInsurance.Float = 0
            _totalDiscount.Float = 0
            _totalPaidByPatient.Float = 0

            For Each itm In Me.Details
                _totalCost.Float += itm._net.Float
                _totalCoveredByInsurance.Float += itm._coveredByInsurance.Float
                _totalDiscount.Float += itm._discount.Float
                _totalPaidByPatient.Float += itm._paidByPatient.Float

            Next

            _totalReturn.Float = _totalReceived.Float - (_totalCost.Float - _totalCoveredByInsurance.Float - _totalDiscount.Float)
            If _totalReturn < 0 Then
                _totalReturn.Float = 0
            End If


            PropertyHasChanged("TotalCost")
        End Sub

#Region " Business Properties and Methods "
        Friend _DTB As String = String.Empty

        Private _documentNo As String = String.Empty
        <System.ComponentModel.DataObjectField(True, False)> _
        <CellInfo(GroupName:="General Info", Tips:="Enter document number")>
        Public ReadOnly Property DocumentNo() As String
            Get
                Return _documentNo
            End Get
        End Property

        Private _documentDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo(GroupName:="General Info", Tips:="Enter document date")>
        Public Property DocumentDate() As String
            Get
                Return _documentDate.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("DocumentDate", True)
                If value Is Nothing Then value = String.Empty
                If Not _documentDate.Equals(value) Then
                    _documentDate.Text = value
                    PropertyHasChanged("DocumentDate")
                End If
            End Set
        End Property

        Private _checkinNo As pbs.Helper.SmartInt32 = New SmartInt32()
        <CellInfo("pbs.BO.MC.CHECKIN", GroupName:="General Info", Tips:="Enter check-in number")>
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
        <CellInfo("pbs.BO.MC.PATIENT", GroupName:="Client Info", Tips:="Enter patient's name")>
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

        Private _invoicePrefix As String = String.Empty
        <CellInfo(GroupName:="Invoice Info", Tips:="Enter InvoicePrefix")>
        Public Property InvoicePrefix() As String
            Get
                Return _invoicePrefix
            End Get
            Set(ByVal value As String)
                CanWriteProperty("InvoicePrefix", True)
                If value Is Nothing Then value = String.Empty
                If Not _invoicePrefix.Equals(value) Then
                    _invoicePrefix = value
                    PropertyHasChanged("InvoicePrefix")
                End If
            End Set
        End Property

        Private _invoiceNo As String = String.Empty
        <CellInfo(GroupName:="Invoice Info", Tips:="Enter invoice number")>
        Public Property InvoiceNo() As String
            Get
                Return _invoiceNo
            End Get
            Set(ByVal value As String)
                CanWriteProperty("InvoiceNo", True)
                If value Is Nothing Then value = String.Empty
                If Not _invoiceNo.Equals(value) Then
                    _invoiceNo = value
                    PropertyHasChanged("InvoiceNo")
                End If
            End Set
        End Property

        Private _invoiceSerial As String = String.Empty
        <CellInfo(GroupName:="Invoice Info", Tips:="Enter invoice serial")>
        Public Property InvoiceSerial() As String
            Get
                Return _invoiceSerial
            End Get
            Set(ByVal value As String)
                CanWriteProperty("InvoiceSerial", True)
                If value Is Nothing Then value = String.Empty
                If Not _invoiceSerial.Equals(value) Then
                    _invoiceSerial = value
                    PropertyHasChanged("InvoiceSerial")
                End If
            End Set
        End Property

        Private _invoiceDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo(GroupName:="Invoice Info", Tips:="Enter invoice date")>
        Public Property InvoiceDate() As String
            Get
                Return _invoiceDate.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("InvoiceDate", True)
                If value Is Nothing Then value = String.Empty
                If Not _invoiceDate.Equals(value) Then
                    _invoiceDate.Text = value
                    PropertyHasChanged("InvoiceDate")
                End If
            End Set
        End Property

        Private _invoicePeriod As SmartPeriod = New pbs.Helper.SmartPeriod()
        <CellInfo(GroupName:="Invoice Info", Tips:="Enter invoice period")>
        Public Property InvoicePeriod() As String
            Get
                Return _invoicePeriod.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("InvoicePeriod", True)
                If value Is Nothing Then value = String.Empty
                If Not _invoicePeriod.Equals(value) Then
                    _invoicePeriod.Text = value
                    PropertyHasChanged("InvoicePeriod")
                End If
            End Set
        End Property

        Private _invoiceType As String = String.Empty
        <CellInfo("INV_TYPE", GroupName:="Invoice Info", Tips:="Enter invoice type")>
        Public Property InvoiceType() As String
            Get
                Return _invoiceType
            End Get
            Set(ByVal value As String)
                CanWriteProperty("InvoiceType", True)
                If value Is Nothing Then value = String.Empty
                If Not _invoiceType.Equals(value) Then
                    _invoiceType = value
                    PropertyHasChanged("InvoiceType")
                End If
            End Set
        End Property

        Private _clientId As String = String.Empty
        <CellInfo(GroupName:="Client Info", Tips:="Enter client ID")>
        Public Property ClientId() As String
            Get
                Return _clientId
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ClientId", True)
                If value Is Nothing Then value = String.Empty
                If Not _clientId.Equals(value) Then
                    _clientId = value
                    PropertyHasChanged("ClientId")
                End If
            End Set
        End Property

        Private _purchName As String = String.Empty
        <CellInfo(GroupName:="Client Info", Tips:="Enter name of purchaser")>
        Public Property PurchName() As String
            Get
                Return _purchName
            End Get
            Set(ByVal value As String)
                CanWriteProperty("PurchName", True)
                If value Is Nothing Then value = String.Empty
                If Not _purchName.Equals(value) Then
                    _purchName = value
                    PropertyHasChanged("PurchName")
                End If
            End Set
        End Property

        Private _address As String = String.Empty
        <CellInfo(GroupName:="Client Info", Tips:="Enter address of client")>
        Public Property Address() As String
            Get
                Return _address
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Address", True)
                If value Is Nothing Then value = String.Empty
                If Not _address.Equals(value) Then
                    _address = value
                    PropertyHasChanged("Address")
                End If
            End Set
        End Property

        Private _bankCode As String = String.Empty
        <CellInfo(GroupName:="Payment Info", Tips:="Enter bank code")>
        Public Property BankCode() As String
            Get
                Return _bankCode
            End Get
            Set(ByVal value As String)
                CanWriteProperty("BankCode", True)
                If value Is Nothing Then value = String.Empty
                If Not _bankCode.Equals(value) Then
                    _bankCode = value
                    PropertyHasChanged("BankCode")
                End If
            End Set
        End Property

        Private _bankDetail As String = String.Empty
        <CellInfo(GroupName:="Payment Info", Tips:="Enter bank detail")>
        Public Property BankDetail() As String
            Get
                Return _bankDetail
            End Get
            Set(ByVal value As String)
                CanWriteProperty("BankDetail", True)
                If value Is Nothing Then value = String.Empty
                If Not _bankDetail.Equals(value) Then
                    _bankDetail = value
                    PropertyHasChanged("BankDetail")
                End If
            End Set
        End Property

        Private _payMethod As String = String.Empty
        <CellInfo(GroupName:="Payment Info", Tips:="Enter payment methods")>
        Public Property PayMethod() As String
            Get
                Return _payMethod
            End Get
            Set(ByVal value As String)
                CanWriteProperty("PayMethod", True)
                If value Is Nothing Then value = String.Empty
                If Not _payMethod.Equals(value) Then
                    _payMethod = value
                    PropertyHasChanged("PayMethod")
                End If
            End Set
        End Property

        Private _taxCode As String = String.Empty
        <CellInfo(GroupName:="Payment Info", Tips:="Enter tax code")>
        Public Property TaxCode() As String
            Get
                Return _taxCode
            End Get
            Set(ByVal value As String)
                CanWriteProperty("TaxCode", True)
                If value Is Nothing Then value = String.Empty
                If Not _taxCode.Equals(value) Then
                    _taxCode = value
                    PropertyHasChanged("TaxCode")
                End If
            End Set
        End Property

        Private _salesman As String = String.Empty
        <CellInfo(GroupName:="Sales Info", Tips:="Enter salesman code")>
        Public Property Salesman() As String
            Get
                Return _salesman
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Salesman", True)
                If value Is Nothing Then value = String.Empty
                If Not _salesman.Equals(value) Then
                    _salesman = value
                    PropertyHasChanged("Salesman")
                End If
            End Set
        End Property

        Private _contractId As String = String.Empty
        <CellInfo(GroupName:="Sales Info", Tips:="Enter contract ID")>
        Public Property ContractId() As String
            Get
                Return _contractId
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ContractId", True)
                If value Is Nothing Then value = String.Empty
                If Not _contractId.Equals(value) Then
                    _contractId = value
                    PropertyHasChanged("ContractId")
                End If
            End Set
        End Property

        Private _convCode As String = String.Empty
        <CellInfo(GroupName:="Sales Info", Tips:="Enter conversion code")>
        Public Property ConvCode() As String
            Get
                Return _convCode
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ConvCode", True)
                If value Is Nothing Then value = String.Empty
                If Not _convCode.Equals(value) Then
                    _convCode = value
                    PropertyHasChanged("ConvCode")
                End If
            End Set
        End Property

        Private _convRate As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="Sales Info", Tips:="Enter conversion rate")>
        Public Property ConvRate() As String
            Get
                Return _convRate.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ConvRate", True)
                If value Is Nothing Then value = String.Empty
                If Not _convRate.Equals(value) Then
                    _convRate.Text = value
                    PropertyHasChanged("ConvRate")
                End If
            End Set
        End Property

        Private _notes As String = String.Empty
        <CellInfo(GroupName:="General Info", Tips:="Notes", ControlType:=Forms.CtrlType.MemoEdit)>
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

        Private _status As String = String.Empty
        <CellInfo(GroupName:="General Info", Tips:="Status")>
        Public Property Status() As String
            Get
                Return _status
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Status", True)
                If value Is Nothing Then value = String.Empty
                If Not _status.Equals(value) Then
                    _status = value
                    PropertyHasChanged("Status")
                End If
            End Set
        End Property

        Private _updated As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo(GroupName:="System")>
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
        <CellInfo(GroupName:="System")>
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

        Private _patientType As String = String.Empty
        <CellInfo(GroupName:="Insurance Info")>
        Public ReadOnly Property PatientType() As String
            Get
                Return _patientType
            End Get
        End Property

        Private _healthInsuranceNo As String = String.Empty
        <CellInfo(GroupName:="Insurance Info")>
        Public ReadOnly Property HealthInsuranceNo() As String
            Get
                Return _healthInsuranceNo
            End Get
        End Property

        Private _validTo As pbs.Helper.SmartDate = New pbs.Helper.SmartDate(0)
        <CellInfo(GroupName:="Insurance Info")>
        Public ReadOnly Property ValidTo() As String
            Get
                Return _validTo.Text
            End Get
        End Property

        Private _totalCost As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="Payment")>
        Public ReadOnly Property TotalCost() As String
            Get
                Return _totalCost.Text
            End Get
        End Property

        Private _totalCoveredByInsurance As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="Payment")>
        Public ReadOnly Property TotalCoveredByInsurance() As String
            Get
                Return _totalCoveredByInsurance.Text
            End Get
        End Property

        Private _totalDiscount As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="Payment")>
        Public ReadOnly Property TotalDiscount() As String
            Get
                Return _totalDiscount.Text
            End Get
        End Property

        Private _totalPaidByPatient As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="Payment")>
        Public ReadOnly Property TotalPaidByPatient() As String
            Get
                Return _totalPaidByPatient.Text
            End Get
        End Property

        Private _totalReceived As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="Payment")>
        Public Property TotalReceived() As String
            Get
                Return _totalReceived.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("TotalReceived", True)
                If value Is Nothing Then value = String.Empty
                If Not _totalReceived.Equals(value) Then
                    _totalReceived.Text = value
                    PropertyHasChanged("TotalReceived")
                End If
            End Set
        End Property

        Private _totalReturn As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="Payment")>
        Public ReadOnly Property TotalReturn() As String
            Get
                Return _totalReturn.Text
            End Get
        End Property

        'Get ID
        Protected Overrides Function GetIdValue() As Object
            Return _documentNo
        End Function

        'IComparable
        Public Function CompareTo(ByVal IDObject) As Integer Implements System.IComparable.CompareTo
            Dim ID = IDObject.ToString
            Dim pDocumentNo As String = ID.Trim
            If _documentNo.Trim < pDocumentNo Then Return -1
            If _documentNo.Trim > pDocumentNo Then Return 1
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

            For Each _field As ClassField In ClassSchema(Of SI)._fieldList
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

        Friend Sub New()
            _DTB = Context.CurrentBECode
            _documentDate.Text = ToDay.ToSunDate
            _invoiceDate.Text = ToDay.ToSunDate
            _invoicePeriod.Text = Month(ToDay)
            _invoiceType = "D"
        End Sub

        Public Shared Function BlankSI() As SI
            Return New SI
        End Function

        Public Shared Function NewSI(ByVal pDocumentNo As String) As SI
            If KeyDuplicated(pDocumentNo) Then ExceptionThower.BusinessRuleStop(String.Format(ResStr(ResStrConst.NOACCESS), ResStr("INVOICE")))
            Return DataPortal.Create(Of SI)(New Criteria(pDocumentNo))
        End Function

        Public Shared Function NewBO(ByVal ID As String) As SI
            Dim pDocumentNo As String = ID.Trim

            Return NewSI(pDocumentNo)
        End Function

        Public Shared Function GetSI(ByVal pDocumentNo As String) As SI
            Return DataPortal.Fetch(Of SI)(New Criteria(pDocumentNo))
        End Function

        Public Shared Function GetBO(ByVal ID As String) As SI
            Dim pDocumentNo As String = ID.Trim

            Return GetSI(pDocumentNo)
        End Function

        Public Shared Sub DeleteSI(ByVal pDocumentNo As String)
            DataPortal.Delete(New Criteria(pDocumentNo))
        End Sub

        Public Overrides Function Save() As SI
            If Not IsDirty Then ExceptionThower.NotDirty(ResStr(ResStrConst.NOTDIRTY))
            If Not IsSavable Then Throw New Csla.Validation.ValidationException(String.Format(ResStr(ResStrConst.INVALID), ResStr("INVOICE")))

            Me.ApplyEdit()
            SIInfoList.InvalidateCache()
            Return MyBase.Save()
        End Function

        Public Function CloneSI(ByVal pDocumentNo As String) As SI

            If SI.KeyDuplicated(pDocumentNo) Then ExceptionThower.BusinessRuleStop(ResStr(ResStrConst.CreateAlreadyExists), Me.GetType.ToString.Leaf.Translate)

            Dim cloningSI As SI = MyBase.Clone
            cloningSI._documentNo = pDocumentNo

            'Todo:Remember to reset status of the new object here 
            cloningSI.MarkNew()

            For Each line In cloningSI.Details
                line.MarkAsNewClone()
            Next

            cloningSI.ApplyEdit()

            cloningSI.ValidationRules.CheckRules()

            Return cloningSI
        End Function

#End Region ' Factory Methods

#Region " Data Access "

        <Serializable()> _
        Private Class Criteria
            Public _documentNo As String = String.Empty

            Public Sub New(ByVal pDocumentNo As String)
                _documentNo = pDocumentNo

            End Sub
        End Class

        <RunLocal()> _
        Private Overloads Sub DataPortal_Create(ByVal criteria As Criteria)
            _documentNo = criteria._documentNo

            ValidationRules.CheckRules()
        End Sub

        Private Overloads Sub DataPortal_Fetch(ByVal criteria As Criteria)
            Using ctx = ConnectionFactory.GetDBConnection(True)
                Using cm = ctx.CreateSQLCommand

                    cm.CommandText = <SqlText>SELECT * FROM pbs_MC_INVOICE_<%= _DTB %> WHERE DOCUMENT_NO= '<%= criteria._documentNo %>' </SqlText>.Value.Trim
                    Using dr As New SafeDataReader(cm.ExecuteReader)
                        If dr.Read Then
                            FetchObject(dr)
                            MarkOld()
                        End If
                    End Using

                    cm.CommandText = <SqlText>SELECT * FROM pbs_MC_INVOICE_DET_<%= _DTB %> WHERE DOCUMENT_NO = '<%= criteria._documentNo %>'</SqlText>.Value.Trim
                    Using dr As New SafeDataReader(cm.ExecuteReader)
                        _details = SIDs.GetSIDS(dr, Me)
                    End Using

                End Using
            End Using
        End Sub

        Private Sub FetchObject(ByVal dr As SafeDataReader)
            _documentNo = dr.GetString("DOCUMENT_NO").TrimEnd
            _documentDate.Text = dr.GetInt32("DOCUMENT_DATE")
            _checkinNo.Text = dr.GetInt32("CHECKIN_NO")
            _patientCode = dr.GetString("PATIENT_CODE").TrimEnd
            _invoicePrefix = dr.GetString("INVOICE_PREFIX").TrimEnd
            _invoiceNo = dr.GetString("INVOICE_NO").TrimEnd
            _invoiceSerial = dr.GetString("INVOICE_SERIAL").TrimEnd
            _invoiceDate.Text = dr.GetInt32("INVOICE_DATE")
            _invoicePeriod.Text = dr.GetInt32("INVOICE_PERIOD")
            _invoiceType = dr.GetString("INVOICE_TYPE")
            _clientId = dr.GetString("CLIENT_ID").TrimEnd
            _purchName = dr.GetString("PURCH_NAME").TrimEnd
            _address = dr.GetString("ADDRESS").TrimEnd
            _bankCode = dr.GetString("BANK_CODE").TrimEnd
            _bankDetail = dr.GetString("BANK_DETAIL").TrimEnd
            _payMethod = dr.GetString("PAY_METHOD").TrimEnd
            _taxCode = dr.GetString("TAX_CODE").TrimEnd
            _salesman = dr.GetString("SALESMAN").TrimEnd
            _contractId = dr.GetString("CONTRACT_ID").TrimEnd
            _convCode = dr.GetString("CONV_CODE").TrimEnd
            _convRate.Text = dr.GetDecimal("CONV_RATE")
            _notes = dr.GetString("NOTES").TrimEnd
            _status = dr.GetString("STATUS").TrimEnd
            _updated.Text = dr.GetInt32("UPDATED")
            _updatedBy = dr.GetString("UPDATED_BY").TrimEnd
            GetInsuranceInfo(_patientCode)
        End Sub

        'get insurance info from Patient table
        Private Sub GetInsuranceInfo(patientCode As String)

            Dim itm = PATIENTInfoList.GetPATIENTInfo(_patientCode)

            _patientType = itm.PatientType

            If Not _healthInsuranceNo = itm.HealthInsuranceNo Then
                _healthInsuranceNo = itm.HealthInsuranceNo

                For Each line In Details
                    line._percentCover.Float = CalculateInsurance()
                    line.Recalculate(1)
                Next
            End If



            _validTo.Text = itm.ValidTo


        End Sub

        Private Shared _lockObj As New Object
        Protected Overrides Sub DataPortal_Insert()
            SyncLock _lockObj
                Using ctx = ConnectionFactory.GetDBConnection(True)
                    Using cm = ctx.CreateSQLCommand

                        cm.CommandType = CommandType.StoredProcedure
                        cm.CommandText = String.Format("pbs_MC_INVOICE_{0}_InsertUpdate", _DTB)

                        AddInsertParameters(cm)
                        cm.ExecuteNonQuery()

                    End Using

                    Me.Details.Update(ctx, Me)
                End Using
            End SyncLock
        End Sub

        Private Sub AddInsertParameters(ByVal cm As IDBCommand)
            cm.Parameters.AddWithValue("@DOCUMENT_NO", _documentNo.Trim)
            cm.Parameters.AddWithValue("@DOCUMENT_DATE", _documentDate.DBValue)
            cm.Parameters.AddWithValue("@CHECKIN_NO", _checkinNo.DBValue)
            cm.Parameters.AddWithValue("@PATIENT_CODE", _patientCode.Trim)
            cm.Parameters.AddWithValue("@INVOICE_PREFIX", _invoicePrefix.Trim)
            cm.Parameters.AddWithValue("@INVOICE_NO", _invoiceNo.Trim)
            cm.Parameters.AddWithValue("@INVOICE_SERIAL", _invoiceSerial.Trim)
            cm.Parameters.AddWithValue("@INVOICE_DATE", _invoiceDate.DBValue)
            cm.Parameters.AddWithValue("@INVOICE_PERIOD", _invoicePeriod.DBValue)
            cm.Parameters.AddWithValue("@INVOICE_TYPE", _invoiceType)
            cm.Parameters.AddWithValue("@CLIENT_ID", _clientId.Trim)
            cm.Parameters.AddWithValue("@PURCH_NAME", _purchName.Trim)
            cm.Parameters.AddWithValue("@ADDRESS", _address.Trim)
            cm.Parameters.AddWithValue("@BANK_CODE", _bankCode.Trim)
            cm.Parameters.AddWithValue("@BANK_DETAIL", _bankDetail.Trim)
            cm.Parameters.AddWithValue("@PAY_METHOD", _payMethod.Trim)
            cm.Parameters.AddWithValue("@TAX_CODE", _taxCode.Trim)
            cm.Parameters.AddWithValue("@SALESMAN", _salesman.Trim)
            cm.Parameters.AddWithValue("@CONTRACT_ID", _contractId.Trim)
            cm.Parameters.AddWithValue("@CONV_CODE", _convCode.Trim)
            cm.Parameters.AddWithValue("@CONV_RATE", _convRate.DBValue)
            cm.Parameters.AddWithValue("@NOTES", _notes.Trim)
            cm.Parameters.AddWithValue("@STATUS", _status.Trim)
            cm.Parameters.AddWithValue("@UPDATED", ToDay.ToSunDate)
            cm.Parameters.AddWithValue("@UPDATED_BY", Context.CurrentUserCode)
        End Sub


        Protected Overrides Sub DataPortal_Update()
            DataPortal_Insert()
        End Sub

        Protected Overrides Sub DataPortal_DeleteSelf()
            DataPortal_Delete(New Criteria(_documentNo))
        End Sub

        Private Overloads Sub DataPortal_Delete(ByVal criteria As Criteria)
          Using ctx = ConnectionFactory.GetDBConnection(True)
                Using cm = ctx.CreateSQLCommand

                    cm.CommandText = <SqlText>DELETE FROM pbs_MC_INVOICE_<%= _DTB %> WHERE DOCUMENT_NO= '<%= criteria._documentNo %>' </SqlText>.Value.Trim
                    cm.ExecuteNonQuery()

                End Using
            End Using

        End Sub

        Protected Overrides Sub DataPortal_OnDataPortalInvokeComplete(ByVal e As Csla.DataPortalEventArgs)
            If Csla.ApplicationContext.ExecutionLocation = ExecutionLocations.Server Then
                SIInfoList.InvalidateCache()
            End If
        End Sub


#End Region 'Data Access                           

#Region " Exists "
        Public Shared Function Exists(ByVal pDocumentNo As String) As Boolean
            Return SIInfoList.ContainsCode(pDocumentNo)
        End Function

        Public Shared Function KeyDuplicated(ByVal pDocumentNo As String) As Boolean
            Dim SqlText = <SqlText>SELECT COUNT(*) FROM pbs_MC_INVOICE_<%= Context.CurrentBECode %> WHERE DOCUMENT_NO= '<%= pDocumentNo %>'</SqlText>.Value.Trim
            Return SQLCommander.GetScalarInteger(SqlText) > 0
        End Function
#End Region

#Region " IGenpart "

        Public Function CloneBO(ByVal id As String) As Object Implements Interfaces.IGenPartObject.CloneBO
            Return CloneSI(id)
        End Function

        Public Function getBO1(ByVal id As String) As Object Implements Interfaces.IGenPartObject.GetBO
            Return GetBO(id)
        End Function

        Public Function myCommands() As String() Implements Interfaces.IGenPartObject.myCommands
            Return pbs.Helper.Action.StandardReferenceCommands
        End Function

        Public Function myFullName() As String Implements Interfaces.IGenPartObject.myFullName
            Return GetType(SI).ToString
        End Function

        Public Function myName() As String Implements Interfaces.IGenPartObject.myName
            Return GetType(SI).ToString.Leaf
        End Function

        Public Function myQueryList() As IList Implements Interfaces.IGenPartObject.myQueryList
            Return SIInfoList.GetSIInfoList
        End Function
#End Region

#Region "IDoclink"
        Public Function Get_DOL_Reference() As String Implements IDocLink.Get_DOL_Reference
            Return String.Format("{0}#{1}", Get_TransType, _documentNo)
        End Function

        Public Function Get_TransType() As String Implements IDocLink.Get_TransType
            Return Me.GetType.ToClassSchemaName.Leaf
        End Function
#End Region

#Region "Report Dataset"
        Public Function GetDataSet() As DataSet Implements Interfaces.ISupportDocumentDataSet.GetDataSet
            Dim ds As New DataSet("SI")

            Dim h As DataTable = List2Table.CreateTableFromSingleObject(Me, "Header")
            Dim d As DataTable = List2Table.CreateTableFromList(Me.Details, "Details")

            ds.Tables.Add(h)
            ds.Tables.Add(d)

            Return ds
        End Function

        Public Function GetTemplateCode() As String Implements Interfaces.ISupportDocumentDataSet.GetTemplateCode
            Dim Rp As String = GetType(SI).ToString & ".Form"
            Return Rp
        End Function
#End Region

    End Class

End Namespace