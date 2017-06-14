Imports pbs.Helper
Imports pbs.Helper.Interfaces
Imports System.Data

Imports Csla
Imports Csla.Data
Imports Csla.Validation
Imports pbs.BO.DataAnnotations
Imports pbs.BO.Script
Imports pbs.BO.LA
Imports pbs.BO.PB
Imports pbs.BO.BusinessRules

Namespace MC

    ''' <summary>
    ''' Clinic Management Ledger
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    <DB(TableName:="pbs_MC_LEDGER_XXX")>
    Public Class MCLDG
        Inherits Csla.BusinessBase(Of MCLDG)
        Implements Interfaces.IGenPartObject
        Implements IComparable
        Implements IDocLink
        Implements ISupportQueryInfoList
        Implements ISupportCachedLookup
        Implements Interfaces.INeedPrepareForEditing

#Region "Property Changed"

        Private _suspendCalculation As Boolean = False
        Public Sub PrepareForEditing() Implements INeedPrepareForEditing.PrepareForEditing
            _suspendCalculation = False
        End Sub
        Public Sub SuspendCalculation()
            _suspendCalculation = True
        End Sub

        Protected Overrides Sub OnDeserialized(context As Runtime.Serialization.StreamingContext)
            MyBase.OnDeserialized(context)
            AddHandler Me.PropertyChanged, AddressOf BO_PropertyChanged
        End Sub

        Private Sub BO_PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Handles Me.PropertyChanged

            Select Case e.PropertyName

                Case "PaymentDate"
                    If Not String.IsNullOrEmpty(PaymentDate) Then
                        If String.IsNullOrEmpty(PaymentPeriod) Then _paymentPeriod.Text = _paymentDate.Date.ToSunPeriod
                    End If

                Case "TransType"
                    _dC = GetJDInfo.DebitCredit
                    If String.IsNullOrEmpty(_reference) AndAlso Not String.IsNullOrEmpty(GetJDInfo.TrefSequence) Then
                        Reference = String.Format("?{0}", GetJDInfo.TrefSequence)
                    End If
                Case "ItemCode"

                    _unitCode = ItemInfo.UnitSale
                    _description = Nz(ItemInfo.Descriptn, _description)

                    If CATEGORYInfoList.ContainsCode("PRICE_BOOK") Then
                        _unitPrice.Float = pbs.BO.LKUDVInfoList.LookFor("PRICE_BOOK", _itemCode, "Value01", _transDate.DBValueInt)

                        PropertyHasChanged("UnitPrice")

                    ElseIf Not String.IsNullOrEmpty(_itemCode) Then
                        Dim info As IRInfo = Nothing
                        If pbs.BO.PB.IRInfoList.ContainsCode(_itemCode, info) Then
                            _unitPrice.Float = pbs.BO.PB.IRInfoList.GetIRinfo(_itemCode).BasePrice1
                            If _unitPrice.Float = 0 Then _unitPrice.Float = pbs.BO.PB.IRInfoList.GetIRinfo(_itemCode).StdCost

                            PropertyHasChanged("UnitPrice")
                        End If
                    End If

                    PropertyHasChanged("UnitCode")

                Case "Quantity"
                    If _suspendCalculation Then Exit Sub

                    If _unitPrice.Float = 0 Then
                        TransAmt = 0
                    Else
                        TransAmt = _quantity.Float * Math.Abs(_unitPrice.Float).RoundOA(_convCode)
                    End If

                Case "UnitPrice"
                    If _suspendCalculation Then Exit Sub

                    If _quantity.Float = 0 Then
                        TransAmt = _unitPrice.Float.RoundOA(_convCode)
                    Else
                        TransAmt = _quantity.Float * Math.Abs(_unitPrice.Float).RoundOA(_convCode)
                    End If

                Case "PatientCode"

                    Dim info = GetPatientInfo()


                    'If Not String.IsNullOrEmpty(info.TaxId) Then
                    '    Me._directInvoice = "Y"
                    '    Me._invoiceInfo = info.TaxId
                    '    Me._payMethod = Nz(info.PaymentMethod, _payMethod)
                    'End If

                Case "ConvCode"
                    If _suspendCalculation Then Exit Sub

                    If _convRate <> Math.Abs(CurrencyConverter.DefaultRate) Then
                        _convRate.Float = Math.Abs(CurrencyConverter.DefaultRate)

                        If _amount.Float <> 0 AndAlso _transAmt.Float = 0 Then
                            _transAmt.Float = CurrencyConverter.Convert2OtherAmount(_amount.Float, _convRate.Float).RoundOA(_convCode)
                        Else
                            _amount.Float = CurrencyConverter.Convert2BaseAmount(_transAmt.Float, _convRate.Float).RoundBA
                        End If
                    End If

                Case "TransAmt"
                    If _suspendCalculation Then Exit Sub

                    _transAmt.Float = _transAmt.Float.RoundOA(_convCode)
                    If String.IsNullOrEmpty(_convCode) Then
                        _amount.Float = _transAmt.Float
                    Else
                        _amount.Float = CurrencyConverter.Convert2BaseAmount(_transAmt.Float, ConvRate).RoundBA
                    End If

                    ReCalculateVAT()

                Case "ConvRate"
                    If _suspendCalculation Then Exit Sub

                    If _amount.Float <> 0 AndAlso _transAmt.Float = 0 Then
                        _transAmt.Float = CurrencyConverter.Convert2OtherAmount(_amount.Float, ConvRate).RoundOA(_convCode)
                    Else
                        _amount.Float = CurrencyConverter.Convert2BaseAmount(_transAmt.Float, ConvRate).RoundBA
                    End If

                    ReCalculateVAT()

                Case "Amount"
                    If _suspendCalculation Then Exit Sub

                    If _transAmt.Float = 0 Then
                        _transAmt.Float = CurrencyConverter.Convert2OtherAmount(_amount.Float, ConvRate).RoundOA(_convCode)
                    Else
                        If _amount.Float <> 0 Then _convRate.Float = Math.Abs(CurrencyConverter.SpotRate(_amount.Float, _transAmt.Float))
                    End If
                    ReCalculateVAT()

                Case "VatRate"
                    If _suspendCalculation Then Exit Sub

                    ReCalculateVAT()

            End Select

            pbs.BO.Rules.CalculationRules.Calculator(sender, e)
        End Sub

        'Friend _isDiscountLine As Boolean = False

        Private Sub ReCalculateVAT()
            If _amount.Float <> 0 Then
                _vatAmount.Float = _amount.Float * _vatRate.ToDecimal / 100
            End If
        End Sub

#End Region

#Region "Currency conversion "

        Private _patientInfo As PATIENTInfo = Nothing
        Private Function GetPatientInfo() As PATIENTInfo

            If _patientInfo Is Nothing OrElse _patientCode.Trim <> _patientInfo.PatientCode.Trim Then
                If String.IsNullOrEmpty(_patientCode) Then
                    Return PATIENTInfo.EmptyPATIENTInfo(_patientCode)
                Else
                    _patientInfo = PATIENTInfoList.GetPATIENTInfo(_patientCode)
                End If
            End If
            Return _patientInfo

        End Function

        Private _converter As ICurrencyConverter = Nothing
        Private Function CurrencyConverter() As ICurrencyConverter

            If _converter Is Nothing OrElse _convCode.Trim <> _converter.CurrencyCode.Trim Then
                If String.IsNullOrEmpty(_convCode) Then
                    Return New EmptyConverter(_convCode)
                Else
                    _converter = DCInfoList.GetConverter(_convCode, _transDate, String.Empty)
                End If
            End If
            Return _converter

        End Function

        Private _itmInfo As IRInfo = Nothing
        <ComponentModel.Browsable(False)>
        Public ReadOnly Property ItemInfo() As IRInfo
            Get
                If _itmInfo Is Nothing OrElse _itmInfo.ItemCode <> _itemCode.Trim Then
                    _itmInfo = IRInfoList.GetIRinfo(_itemCode)
                End If
                Return _itmInfo
            End Get
        End Property
#End Region

#Region " Business Properties and Methods "
        Friend _DTB As String = String.Empty

        Private _lineNo As Integer
        <System.ComponentModel.DataObjectField(True, True)> _
        <CellInfo(Hidden:=True)>
        Public ReadOnly Property LineNo() As Integer
            Get
                Return _lineNo
            End Get
        End Property

        Protected Overrides Function GetSubForm() As String
            Return _transType
        End Function

        Private _transType As String = String.Empty
        <CellInfo("pbs.BO.MC.JD", GroupName:="Transaction", Tips:="Transaction is classified to different group: Receivable, Cash Payment, Bank Payment using this transactions type")>
        <Rule(Required:=True)>
        Public Property TransType() As String
            Get
                Return _transType
            End Get
            Set(ByVal value As String)
                CanWriteProperty("TransType", True)
                If value Is Nothing Then value = String.Empty
                If Not _transType.Equals(value) Then
                    _transType = value
                    PropertyHasChanged("TransType")
                End If
            End Set
        End Property

        Private _jdInfo As JDInfo = Nothing
        Private Function GetJDInfo() As JDInfo

            If _jdInfo Is Nothing OrElse _transType.Trim <> _jdInfo.JournalType.Trim Then
                If String.IsNullOrEmpty(_transType.Trim) Then
                    Return JDInfoList.GetJDInfo("")
                Else
                    _jdInfo = JDInfoList.GetJDInfo(_transType)
                End If
            End If
            Return _jdInfo

        End Function

        Private _reference As String = String.Empty
        <CellInfo(GroupName:="Transaction", Tips:="Transaction Reference")>
        Public Property Reference() As String
            Get
                Return _reference
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Reference", True)
                If value Is Nothing Then value = String.Empty
                If Not _reference.Equals(value) Then
                    _reference = value
                    PropertyHasChanged("Reference")
                End If
            End Set
        End Property

        Private _transDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo(LinkCode.Calendar, GroupName:="Transaction")>
        <Rule(Required:=True)>
        Public Property TransDate() As String
            Get
                Return _transDate.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("TransDate", True)
                If value Is Nothing Then value = String.Empty
                If Not _transDate.Equals(value) Then
                    _transDate.Text = value

                    If String.IsNullOrEmpty(Period) Then
                        _period.Text = _transDate.Date.ToSunPeriod
                        PropertyHasChanged("Period")
                    End If

                    PropertyHasChanged("TransDate")
                End If
            End Set
        End Property

        Private _period As SmartPeriod = New pbs.Helper.SmartPeriod()
        <CellInfo(LinkCode.Period, GroupName:="Transaction")>
        <Rule(Required:=True)>
        Public Property Period() As String
            Get
                Return _period.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Period", True)
                If value Is Nothing Then value = String.Empty
                If Not _period.Equals(value) Then
                    _period.Text = value
                    PropertyHasChanged("Period")
                End If
            End Set
        End Property

        Private _candidateId As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        '<CellInfo("pbs.BO.MC.CAN", GroupName:="Transaction")>
        'Public Property CandidateId() As String
        '    Get
        '        Return _candidateId.Text
        '    End Get
        '    Set(ByVal value As String)
        '        CanWriteProperty("CandidateId", True)
        '        If value Is Nothing Then value = String.Empty
        '        If Not _candidateId.Equals(value) Then
        '            _candidateId.Text = value
        '            PropertyHasChanged("CandidateId")
        '        End If
        '    End Set
        'End Property

        Private _patientCode As String = String.Empty
        <CellInfo("pbs.BO.MC.PATIENT", GroupName:="Transaction")>
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

        'Private _ClinicYear As String = String.Empty
        '<CellInfo("pbs.BO.MC.ClinicYear", GroupName:="Clinic", Tips:="Phoebus will fill in the current Clinic year to this field when create a new line")>
        'Public Property ClinicYear() As String
        '    Get
        '        Return _ClinicYear
        '    End Get
        '    Set(ByVal value As String)
        '        CanWriteProperty("ClinicYear", True)
        '        If value Is Nothing Then value = String.Empty
        '        If Not _ClinicYear.Equals(value) Then
        '            _ClinicYear = value
        '            PropertyHasChanged("ClinicYear")
        '        End If
        '    End Set
        'End Property

        Private _clinic As String = String.Empty
        <CellInfo(GroupName:="Clinic", Tips:="The Clinic where Patient registered. Phoebus will fill in the Clinic of Patient to this field when create a new line")>
        Public Property Clinic() As String
            Get
                Return _clinic
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Clinic", True)
                If value Is Nothing Then value = String.Empty
                If Not _clinic.Equals(value) Then
                    _clinic = value
                    PropertyHasChanged("Clinic")
                End If
            End Set
        End Property

        'Private _transCampus As String = String.Empty
        '<CellInfo(GroupName:="Clinic", Tips:="The campus, where transaction happens. Phoebus will fill in the campus of input operator to this field when create a new line")>
        'Public Property TransCampus() As String
        '    Get
        '        Return _transCampus
        '    End Get
        '    Set(ByVal value As String)
        '        CanWriteProperty("TransCampus", True)
        '        If value Is Nothing Then value = String.Empty
        '        If Not _transCampus.Equals(value) Then
        '            _transCampus = value
        '            PropertyHasChanged("TransCampus")
        '        End If
        '    End Set
        'End Property

        'Private _program As String = String.Empty
        '<CellInfo(GroupName:="Clinic", Tips:="The program where Patient registered. Phoebus will fill in the program from item code when create a new line")>
        'Public Property Program() As String
        '    Get
        '        Return _program
        '    End Get
        '    Set(ByVal value As String)
        '        CanWriteProperty("Program", True)
        '        If value Is Nothing Then value = String.Empty
        '        If Not _program.Equals(value) Then
        '            _program = value
        '            PropertyHasChanged("Program")
        '        End If
        '    End Set
        'End Property

        'Private _classId As String = String.Empty
        '<CellInfo("pbs.BO.MC.CLA", GroupName:="Clinic", Tips:="The program where Patient registered. Phoebus will fill in the program from item code when create a new line")>
        'Public Property ClassId() As String
        '    Get
        '        Return _classId
        '    End Get
        '    Set(ByVal value As String)
        '        CanWriteProperty("ClassId", True)
        '        If value Is Nothing Then value = String.Empty
        '        If Not _classId.Equals(value) Then
        '            _classId = value
        '            PropertyHasChanged("ClassId")
        '        End If
        '    End Set
        'End Property

        Private _itemCode As String = String.Empty
        <CellInfo("pbs.BO.PB.IR", GroupName:="Service Item", Tips:="The service code, which Patient subscribed to and pay for")>
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

        Private _description As String = String.Empty
        <CellInfo(GroupName:="Service Item", Tips:="Description for one off services (code = -)")>
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

        Friend _quantity As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(1)
        <CellInfo(GroupName:="Service Item", Tips:="Item Quantity")>
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

        Private _unitCode As String = String.Empty
        <CellInfo("UNIT_CODE", GroupName:="Service Item", Tips:="Item Quantity")>
        Public Property UnitCode() As String
            Get
                Return _unitCode
            End Get
            Set(ByVal value As String)
                CanWriteProperty("UnitCode", True)
                If value Is Nothing Then value = String.Empty
                If Not _unitCode.Equals(value) Then
                    _unitCode = value
                    PropertyHasChanged("UnitCode")
                End If
            End Set
        End Property

        Private _unitPrice As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="Service Item", Tips:="Item unit price")>
        Public Property UnitPrice() As String
            Get
                Return _unitPrice.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("UnitPrice", True)
                If value Is Nothing Then value = String.Empty
                If Not _unitPrice.Equals(value) Then
                    _unitPrice.Text = value
                    _unitPrice.Float = Math.Abs(_unitPrice.Float)
                    PropertyHasChanged("UnitPrice")
                End If
            End Set
        End Property

        Private _transAmt As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="Amount", Tips:="Service amount in transaction currency")>
        Public Property TransAmt() As String
            Get
                Return _transAmt.Text
            End Get
            Set(ByVal value As String)

                If value Is Nothing Then value = 0
                If Not _transAmt.Equals(value) Then
                    _transAmt.Text = value
                    _transAmt.Float = _transAmt.Float.RoundOA(_convCode)
                    PropertyHasChanged("TransAmt")
                End If
            End Set
        End Property

        Private _convCode As String = String.Empty
        <CellInfo(LinkCode.Conversion, GroupName:="Amount", Tips:="Conversion currency. The conversion rate will be calculated using this code and Transaction Date")>
        Public Property ConvCode() As String
            Get
                Return _convCode
            End Get
            Set(ByVal value As String)

                If value Is Nothing Then value = String.Empty
                If Not _convCode.Equals(value) Then
                    _convCode = value
                    PropertyHasChanged("ConvCode")
                End If
            End Set
        End Property

        Private _convRate As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(1)
        <CellInfo(GroupName:="Amount", Tips:="Conversion rate")>
        Public Property ConvRate() As Decimal
            Get
                Return _convRate.Float
            End Get
            Set(ByVal value As Decimal)

                If Not _convRate.Equals(value) Then
                    _convRate.Float = Math.Abs(value)
                    PropertyHasChanged("ConvRate")
                End If
            End Set
        End Property

        Friend _amount As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="Amount", Tips:="Transaction amount in base currency")>
        Public Property Amount() As String
            Get
                Return _amount.Text
            End Get
            Set(ByVal value As String)

                If value Is Nothing Then value = String.Empty
                If Not _amount.Equals(value) Then
                    _amount.Text = value
                    _amount.Float = _amount.Float.RoundBA
                    PropertyHasChanged("Amount")
                End If
            End Set
        End Property

        Private _dC As String = String.Empty
        <CellInfo(GroupName:="Amount", Tips:="Debit/Credit. Debit is negative in database format")>
        Public Property DC() As String
            Get
                Return _dC
            End Get
            Set(ByVal value As String)
                CanWriteProperty("DC", True)
                If value Is Nothing Then value = String.Empty
                If Not _dC.Equals(value) Then
                    _dC = value
                    PropertyHasChanged("DC")
                End If
            End Set
        End Property

        <CellInfo(GroupName:="Amount", Tips:="Absolute amount if DC is Debit")>
        Public Property Debit() As Decimal
            Get
                Return If(_dC.Trim.ToUpper = "D", Math.Abs(_amount.Float), 0)
            End Get
            Set(ByVal value As Decimal)
                If value = 0 AndAlso _amount.Float <> 0 AndAlso _dC = "C" Then
                    'already debit 
                Else
                    Amount = value.RoundBA
                    _dC = "D"
                    PropertyHasChanged("Debit")
                End If
            End Set
        End Property

        <CellInfo(GroupName:="Amount", Tips:="Absolute amount if DC is Credit")>
        Public Property Credit() As Decimal
            Get
                Return If(_dC.Trim.ToUpper = "C", Math.Abs(_amount.Float), 0)
            End Get
            Set(ByVal value As Decimal)

                If value = 0 AndAlso _amount.Float <> 0 AndAlso _dC = "D" Then
                    'already debit 
                Else
                    Amount = value.RoundBA
                    _dC = "C"
                    PropertyHasChanged("Credit")
                End If

            End Set
        End Property

        Private _paymentRef As String = String.Empty
        <CellInfo(GroupName:="Payment", Tips:="Used for generate payment or manual payment")>
        Public Property PaymentRef() As String
            Get
                Return _paymentRef
            End Get
            Set(ByVal value As String)
                CanWriteProperty("PaymentRef", True)
                If value Is Nothing Then value = String.Empty
                If Not _paymentRef.Equals(value) Then
                    _paymentRef = value
                    PropertyHasChanged("PaymentRef")
                End If
            End Set
        End Property

        Private _payMethod As String = String.Empty
        <CellInfo(GroupName:="Payment", Tips:="Bank TT/Cash/Credit Card")>
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

        Private _paymentDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo(LinkCode.Calendar, GroupName:="Payment", Tips:="The payment date")>
        Public Property PaymentDate() As String
            Get
                Return _paymentDate.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("PaymentDate", True)
                If value Is Nothing Then value = String.Empty
                If Not _paymentDate.Equals(value) Then
                    _paymentDate.Text = value
                    PropertyHasChanged("PaymentDate")
                End If
            End Set
        End Property

        Private _paymentPeriod As SmartPeriod = New pbs.Helper.SmartPeriod()
        <CellInfo(LinkCode.Period, GroupName:="Payment", Tips:="The payment period")>
        Public Property PaymentPeriod() As String
            Get
                Return _paymentPeriod.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("PaymentPeriod", True)
                If value Is Nothing Then value = String.Empty
                If Not _paymentPeriod.Equals(value) Then
                    _paymentPeriod.Text = value
                    PropertyHasChanged("PaymentPeriod")
                End If
            End Set
        End Property

        Private _directInvoice As String = String.Empty
        <CellInfo(GroupName:="Invoice", Tips:="Y - Invoice will be issued to the payer. Non-Direct invoices will be collected and issued in batch at the month end.")>
        Public Property DirectInvoice() As Boolean
            Get
                Return _directInvoice.ToBoolean
            End Get
            Set(ByVal value As Boolean)
                CanWriteProperty("DirectInvoice", True)
                If Not DirectInvoice.Equals(value) Then
                    _directInvoice = If(value, "Y", "N")
                    PropertyHasChanged("DirectInvoice")
                End If
            End Set
        End Property

        Private _invoiceInfo As String = String.Empty
        <CellInfo(GroupName:="Invoice", Tips:="Link to a list, keeping the invoice info of customer. If empty, Phoebus will get the info from the Patient record")>
        Public Property InvoiceInfo() As String
            Get
                Return _invoiceInfo
            End Get
            Set(ByVal value As String)
                CanWriteProperty("InvoiceInfo", True)
                If value Is Nothing Then value = String.Empty
                If Not _invoiceInfo.Equals(value) Then
                    _invoiceInfo = value
                    PropertyHasChanged("InvoiceInfo")
                End If
            End Set
        End Property

        Private _invoiceNo As String = String.Empty
        <CellInfo(GroupName:="Invoice", Tips:="Invoice No")>
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
        <CellInfo(GroupName:="Invoice", Tips:="Invoice Prefix (Serial)")>
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

        Private _invoiceBook As String = String.Empty
        <CellInfo(GroupName:="Invoice", Tips:="The Book Number")>
        Public Property InvoiceBook() As String
            Get
                Return _invoiceBook
            End Get
            Set(ByVal value As String)
                CanWriteProperty("InvoiceBook", True)
                If value Is Nothing Then value = String.Empty
                If Not _invoiceBook.Equals(value) Then
                    _invoiceBook = value
                    PropertyHasChanged("InvoiceBook")
                End If
            End Set
        End Property

        Private _invoiceDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo(LinkCode.Calendar, GroupName:="Invoice")>
        Public Property InvoiceDate() As String
            Get
                Return _invoiceDate.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("InvoiceDate", True)
                If value Is Nothing Then value = String.Empty
                If Not _invoiceDate.Equals(value) Then
                    _invoiceDate.Text = value

                    If Not String.IsNullOrEmpty(_invoiceDate.Text) Then
                        If String.IsNullOrEmpty(InvoicePeriod) Then _invoicePeriod.Text = _invoiceDate.Date.ToSunPeriod
                    End If

                    PropertyHasChanged("InvoiceDate")
                End If
            End Set
        End Property

        Private _invoicePeriod As SmartPeriod = New pbs.Helper.SmartPeriod()
        <CellInfo(LinkCode.Period, GroupName:="Invoice")>
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

        Private _vatRate As String = String.Empty
        <CellInfo(GroupName:="Invoice", Tips:="The VAT rate to apply")>
        Public Property VatRate() As String
            Get
                Return _vatRate
            End Get
            Set(ByVal value As String)
                CanWriteProperty("VatRate", True)
                If value Is Nothing Then value = String.Empty
                If Not _vatRate.Equals(value) Then
                    _vatRate = value
                    PropertyHasChanged("VatRate")
                End If
            End Set
        End Property

        Friend _vatAmount As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="Invoice", Tips:="The VAT rate value")>
        Public Property VatAmount() As String
            Get
                Return _vatAmount.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("VatAmount", True)
                If value Is Nothing Then value = String.Empty
                If Not _vatAmount.Equals(value) Then
                    _vatAmount.Text = value
                    PropertyHasChanged("VatAmount")
                End If
            End Set
        End Property

        <CellInfo(GroupName:="Invoice", Tips:="Amount + VAT")>
        Public ReadOnly Property LineValue() As Decimal
            Get
                Return (_vatAmount.Float + _amount.Float).RoundBA
            End Get
        End Property

        Private _ncPl0 As String = String.Empty
        Public Property NcSl0() As String
            Get
                Return _ncPl0
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcSl0", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncPl0.Equals(value) Then
                    _ncPl0 = value
                    PropertyHasChanged("NcSl0")
                End If
            End Set
        End Property

        Private _ncPl1 As String = String.Empty
        Public Property NcSl1() As String
            Get
                Return _ncPl1
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcSl1", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncPl1.Equals(value) Then
                    _ncPl1 = value
                    PropertyHasChanged("NcSl1")
                End If
            End Set
        End Property

        Private _ncPl2 As String = String.Empty
        Public Property NcSl2() As String
            Get
                Return _ncPl2
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcSl2", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncPl2.Equals(value) Then
                    _ncPl2 = value
                    PropertyHasChanged("NcSl2")
                End If
            End Set
        End Property

        Private _ncPl3 As String = String.Empty
        Public Property NcSl3() As String
            Get
                Return _ncPl3
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcSl3", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncPl3.Equals(value) Then
                    _ncPl3 = value
                    PropertyHasChanged("NcSl3")
                End If
            End Set
        End Property

        Private _ncPl4 As String = String.Empty
        Public Property NcSl4() As String
            Get
                Return _ncPl4
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcSl4", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncPl4.Equals(value) Then
                    _ncPl4 = value
                    PropertyHasChanged("NcSl4")
                End If
            End Set
        End Property

        Private _ncPl5 As String = String.Empty
        Public Property NcSl5() As String
            Get
                Return _ncPl5
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcSl5", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncPl5.Equals(value) Then
                    _ncPl5 = value
                    PropertyHasChanged("NcSl5")
                End If
            End Set
        End Property

        Private _ncPl6 As String = String.Empty
        Public Property NcSl6() As String
            Get
                Return _ncPl6
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcSl6", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncPl6.Equals(value) Then
                    _ncPl6 = value
                    PropertyHasChanged("NcSl6")
                End If
            End Set
        End Property

        Private _ncPl7 As String = String.Empty
        Public Property NcSl7() As String
            Get
                Return _ncPl7
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcSl7", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncPl7.Equals(value) Then
                    _ncPl7 = value
                    PropertyHasChanged("NcSl7")
                End If
            End Set
        End Property

        Private _ncPl8 As String = String.Empty
        Public Property NcSl8() As String
            Get
                Return _ncPl8
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcSl8", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncPl8.Equals(value) Then
                    _ncPl8 = value
                    PropertyHasChanged("NcSl8")
                End If
            End Set
        End Property

        Friend _ncPl9 As String = String.Empty
        <CellInfo(GroupName:="System", Hidden:=True, Tips:="This field contains the LineNo of subscription. Phoebus fill in LineNo when generate receivable")>
        Public ReadOnly Property SubscriptionId() As String
            Get
                Return _ncPl9
            End Get
        End Property

        Private _allocation As String = String.Empty
        <CellInfo(LinkCode.Allocation, GroupName:="Allocation", Tips:="A-Allocated, P-Paid, R-Reconciled,C-Correction....")>
        Public Property Allocation() As String
            Get
                Return _allocation
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Allocation", True)
                If value Is Nothing Then value = String.Empty
                If Not _allocation.Equals(value) Then
                    _allocation = value
                    PropertyHasChanged("Allocation")
                End If
            End Set
        End Property

        Private _allocRef As Integer
        <CellInfo(LinkCode.Allocation, GroupName:="Allocation", Tips:="A-Allocated, P-Paid, R-Reconciled,C-Correction....")>
        Public Property AllocRef() As Integer
            Get
                Return _allocRef
            End Get
            Set(ByVal value As Integer)
                CanWriteProperty("AllocRef", True)
                'DELETED_ME
                If Not _allocRef.Equals(value) Then
                    _allocRef = value
                    PropertyHasChanged("AllocRef")
                End If
            End Set
        End Property

        Private _allocDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo(LinkCode.Calendar, GroupName:="Allocation", Hidden:=True)>
        Public Property AllocDate() As String
            Get
                Return _allocDate.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("AllocDate", True)
                If value Is Nothing Then value = String.Empty
                If Not _allocDate.Equals(value) Then
                    _allocDate.Text = value
                    PropertyHasChanged("AllocDate")
                End If
            End Set
        End Property

        Private _allocPeriod As SmartPeriod = New pbs.Helper.SmartPeriod()
        <CellInfo(LinkCode.Period, GroupName:="Allocation", Hidden:=True)>
        Public Property AllocPeriod() As String
            Get
                Return _allocPeriod.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("AllocPeriod", True)
                If value Is Nothing Then value = String.Empty
                If Not _allocPeriod.Equals(value) Then
                    _allocPeriod.Text = value
                    PropertyHasChanged("AllocPeriod")
                End If
            End Set
        End Property

        Private _status As String = String.Empty
        <CellInfo(GroupName:="System", Hidden:=True)>
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

        Private _lockFlag As String = String.Empty
        <CellInfo(GroupName:="System", Hidden:=True, Tips:="Y - mean hard posting. no more editing, deleting ....")>
        Public ReadOnly Property LockFlag() As String
            Get
                Return _lockFlag
            End Get
        End Property

        Public Function HardPosted() As Boolean
            If IsNew Then Return False
            Return _lockFlag.Equals("Y", StringComparison.OrdinalIgnoreCase)
        End Function

        Friend _postingDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo(GroupName:="System", Hidden:=True)>
        Public ReadOnly Property PostingDate() As String
            Get
                Return _postingDate.Text
            End Get
        End Property

        Friend _postedBy As String = String.Empty
        <CellInfo(GroupName:="System", Hidden:=True)>
        Public ReadOnly Property PostedBy() As String
            Get
                Return _postedBy
            End Get
        End Property

        Private _holdOpId As String = String.Empty

        Private _bphNo As Integer
        <CellInfo(GroupName:="System", Hidden:=True)>
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

        Private _pfdNo As Integer
        <CellInfo(GroupName:="System", Hidden:=True)>
        Public ReadOnly Property PfdNo() As Integer
            Get
                Return _pfdNo
            End Get
        End Property

        Private _extDesc1 As String = String.Empty
        <CellInfo(GroupName:="System", Hidden:=True)>
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
        <CellInfo(GroupName:="System", Hidden:=True)>
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
        <CellInfo(GroupName:="System", Hidden:=True)>
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
        <CellInfo(GroupName:="System", Hidden:=True)>
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
        <CellInfo(GroupName:="System", Hidden:=True)>
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

        Private _extDate1 As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo(LinkCode.Calendar, GroupName:="System", Tips:="Due date is the start date of service")>
        Public Property DueDate() As String
            Get
                Return _extDate1.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("DueDate", True)
                If value Is Nothing Then value = String.Empty
                If Not _extDate1.Equals(value) Then
                    _extDate1.Text = value
                    PropertyHasChanged("DueDate")
                End If
            End Set
        End Property

        Private _extDate2 As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo(LinkCode.Calendar, GroupName:="System", Hidden:=True)>
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
        <CellInfo(LinkCode.Calendar, GroupName:="System", Hidden:=True)>
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
        <CellInfo(LinkCode.Calendar, GroupName:="System", Hidden:=True)>
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

        Private _extDate5 As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo(LinkCode.Calendar, GroupName:="System", Hidden:=True)>
        Public Property ExtDate5() As String
            Get
                Return _extDate5.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ExtDate5", True)
                If value Is Nothing Then value = String.Empty
                If Not _extDate5.Equals(value) Then
                    _extDate5.Text = value
                    PropertyHasChanged("ExtDate5")
                End If
            End Set
        End Property

        Private _extVal1 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="System", Hidden:=True)>
        Public Property ExtVal1() As String
            Get
                Return _extVal1.SignedText
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

        Private _extVal2 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="System", Hidden:=True)>
        Public Property ExtVal2() As String
            Get
                Return _extVal2.SignedText
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

        Private _extVal3 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="System", Hidden:=True)>
        Public Property ExtVal3() As String
            Get
                Return _extVal3.SignedText
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

        Private _extVal4 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="System", Hidden:=True)>
        Public Property ExtVal4() As String
            Get
                Return _extVal4.SignedText
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

        Private _extVal5 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="System", Hidden:=True)>
        Public Property ExtVal5() As String
            Get
                Return _extVal5.SignedText
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

        'Conversion Code validator
        Private Shared Function ConvCodeValidator(ByVal Target As Object, ByVal e As Validation.RuleArgs) As Boolean
            Dim line As MCLDG
            line = CType(Target, MCLDG)

            If String.IsNullOrEmpty(line.ConvCode) Then

                'System journal allow conversion code be empty. 
                If line._transType.Equals(pbs.Helper.SystemJT) Then Return True

                If line.GetJDInfo.Conversion Then
                    e.Description = String.Format(ResStr("You must enter conversion code against transaction type {0}"), line._transType)
                    Return False
                Else
                    Return True
                End If

            Else
                'conversion code is entered. it must be valid
                Dim info As CNInfo = CNInfoList.GetCNInfo(line._convCode)

                If Not CNInfoList.ContainsCode(line._convCode.Trim, info) Then
                    Return CNInfoList.ContainsCode(Target, e)
                    'no code 
                End If
                'and it can  provide a converter without any exception
                Try

                    line._converter = DCInfoList.GetConverter(line._convCode, line._transDate, String.Empty)
                    Return True

                Catch ex As Exception
                    e.Description = ex.Message
                    Return False
                End Try

            End If

        End Function
        'Conversion tolerance validation
        Private Shared Function RateValidator(ByVal Target As Object, ByVal e As Validation.RuleArgs) As Boolean
            Dim line As MCLDG = Target

            'not multicurrency
            If String.IsNullOrEmpty(line._convCode) Then Return True
            If Not line.GetJDInfo.Conversion Then Return True

            Dim defaultRate As Decimal = line.CurrencyConverter.DefaultRate
            Dim tolerance As Decimal = LDInfo.GetLDInfo.ConvTolerance

            'no tolerance setting
            If tolerance <= 0 Then Return True

            'suppress other amount 
            If line._convRate = 0 Then Return True

            If defaultRate <> 0 Then
                If Math.Abs(line._convRate.Float - defaultRate) * 100 / defaultRate <= tolerance Then
                    Return True
                Else
                    e.Description = String.Format(ResStr("RATE_OVER_TOLERANCE"), tolerance)
                    Return False
                End If
            Else
                Return True
            End If

        End Function

        Private Shared Function ItemCodeRequiredValidator(ByVal Target As Object, ByVal e As Validation.RuleArgs) As Boolean
            Dim line As MCLDG = Target

            If line.GetJDInfo.RequireItemCode AndAlso String.IsNullOrEmpty(line.ItemCode) Then
                e.Description = ResStrConst.SelectItemText("ItemCode")
                Return False
            Else
                Return True
            End If


        End Function

        Private Sub AddSharedCommonRules()

            ValidationRules.AddRule(AddressOf ConvCodeValidator, "ConvCode", 1)

            ValidationRules.AddRule(AddressOf RateValidator, "ConvRate", 1)

            ValidationRules.AddRule(AddressOf LA.LDInfo.ContainsValidPeriod, "Period", 1)

            ValidationRules.AddRule(AddressOf LA.LDInfo.ContainsValidDate, "TransDate", 1)

            'Sample simple custom rule
            'ValidationRules.AddRule(AddressOf LDInfo.ContainsValidPeriod, "Period", 1)           

            'Sample dependent property. when check one , check the other as well
            'ValidationRules.AddDependantProperty("AccntCode", "AnalT0")
        End Sub

        Private Shared Function ValidateJournalType(target As Object, e As RuleArgs) As Boolean
            Dim line As MCLDG = target
            If line.IsChild Then
                Return True
            Else
                Return pbs.BO.MC.JDInfoList.ContainsCode(target, e)
            End If
        End Function

        Protected Overrides Sub AddBusinessRules()
            AddSharedCommonRules()

            For Each _field As ClassField In ClassSchema(Of MCLDG)._fieldList
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
                    Select Case _field.FieldName
                        Case "TransType"
                            ValidationRules.AddRule(AddressOf ValidateJournalType, _field.FieldName)
                        Case "PatientCode"
                            ValidationRules.AddRule(AddressOf pbs.BO.MC.PATIENTInfoList.ContainsCode, _field.FieldName)
                        Case "ItemCode"
                            ValidationRules.AddRule(AddressOf ItemCodeRequiredValidator, "ItemCode", 0)
                            'Case "UnitCode"
                            '    ValidationRules.AddRule(Of MCLDG, AnalRuleArg)(AddressOf LOOKUPInfoList.ContainsSysCode, New AnalRuleArg(_field.FieldName, SysCats.UnitCode))
                            '        Case "Status"
                            '            ValidationRules.AddRule(Of LOC, AnalRuleArg)(AddressOf LOOKUPInfoList.ContainsSysCode, New AnalRuleArg(_field.FieldName, SysCats.LocationStatus))
                    End Select
                End If
            Next
            Rules.BusinessRules.RegisterBusinessRules(Me)
            MyBase.AddBusinessRules()
        End Sub

#End Region ' Validation

#Region " Factory Methods "

        Private Sub New()
            _DTB = Context.CurrentBECode
            '_ClinicYear = pbs.BO.MC.Settings.GetSettings.CurrentClinicYear
            '_transDate.Text = ToDay.ToSunDate
        End Sub

        Public Shared Function BlankMCLDG() As MCLDG
            Return New MCLDG
        End Function

        Public Shared Function NewMCLDG(ByVal pLineNo As String) As MCLDG
            Return DataPortal.Create(Of MCLDG)(New Criteria(pLineNo))
        End Function

        Public Shared Function NewBO(ByVal ID As String) As MCLDG
            Dim pLineNo As Integer = ID.Trim.ToInteger

            Return NewMCLDG(pLineNo)
        End Function

        Public Shared Function GetMCLDG(ByVal pLineNo As String) As MCLDG
            Return DataPortal.Fetch(Of MCLDG)(New Criteria(pLineNo))
        End Function

        Public Shared Function GetBO(ByVal ID As String) As MCLDG
            Dim pLineNo As Integer = ID.Trim.ToInteger

            Return GetMCLDG(pLineNo)
        End Function

        Public Shared Sub DeleteMCLDG(ByVal pLineNo As String)
            DataPortal.Delete(New Criteria(pLineNo))
        End Sub

        Public Overrides Function Save() As MCLDG
            If Not IsDirty Then ExceptionThower.NotDirty(ResStr(ResStrConst.NOTDIRTY))
            If Not IsSavable Then Throw New Csla.Validation.ValidationException(Me.BrokenRulesCollection.ToString)

            Me.ApplyEdit()

            TakeSequenceNumber()

            _postedBy = Context.CurrentUserCode
            _postingDate.Text = ToDay.ToSunDate

            SetDC()

            Dim ret = MyBase.Save()

            If Not Context.IsBatchSavingMode Then MCLDGInfoList.InvalidateCache()

            Return ret
        End Function

        Private Sub TakeSequenceNumber()
            If _reference.StartsWith("?") Then
                Dim theSequence As SEQInfo = Nothing
                If pbs.BO.SEQInfoList.ContainsCode(GetJDInfo.TrefSequence, theSequence) Then
                    pbs.BO.Rules.NumberingRules.SetNumber(Me)
                    _reference = pbs.BO.SEQ.TakeNumber(theSequence.SequenceCode)
                End If
            End If
        End Sub

        Public Function CloneMCLDG(ByVal pLineNo As String) As MCLDG

            Dim cloning As MCLDG = MyBase.Clone
            cloning._lineNo = 0
            cloning._DTB = Context.CurrentBECode
            cloning._status = String.Empty
            cloning._lockFlag = String.Empty
            cloning._postedBy = String.Empty
            cloning._postingDate.Text = String.Empty

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

                    cm.CommandText = <SqlText>SELECT * FROM pbs_MC_LEDGER_<%= _DTB %> WHERE LINE_NO=<%= criteria._lineNo %></SqlText>.Value.Trim

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
            _transType = dr.GetString("TRANS_TYPE").TrimEnd
            _reference = dr.GetString("REFERENCE").TrimEnd
            _transDate.Text = dr.GetInt32("TRANS_DATE")
            _period.Text = dr.GetInt32("PERIOD")
            _candidateId.Text = dr.GetInt32("CANDIDATE_ID")
            _patientCode = dr.GetString("PATIENT_CODE").TrimEnd
            '_ClinicYear = dr.GetString("Clinic_YEAR").TrimEnd
            _clinic = dr.GetString("CLINIC").TrimEnd
            '_transCampus = dr.GetString("TRANS_CAMPUS").TrimEnd
            '_program = dr.GetString("PROGRAM").TrimEnd
            '_classId = dr.GetString("CLASS_ID").TrimEnd
            _itemCode = dr.GetString("ITEM_CODE").TrimEnd
            _description = dr.GetString("DESCRIPTION").TrimEnd
            _quantity.Text = dr.GetDecimal("QUANTITY")
            _unitCode = dr.GetString("UNIT_CODE").TrimEnd
            _unitPrice.Text = dr.GetDecimal("UNIT_PRICE")
            _transAmt.Text = dr.GetDecimal("TRANS_AMT")
            _convCode = dr.GetString("CONV_CODE").TrimEnd
            _convRate.Text = dr.GetDecimal("CONV_RATE")
            _amount.Text = dr.GetDecimal("AMOUNT")
            _dC = dr.GetString("D_C").TrimEnd
            _paymentRef = dr.GetString("PAYMENT_REF").TrimEnd
            _payMethod = dr.GetString("PAY_METHOD").TrimEnd
            _paymentDate.Text = dr.GetInt32("PAYMENT_DATE")
            _paymentPeriod.Text = dr.GetInt32("PAYMENT_PERIOD")
            _directInvoice = dr.GetString("DIRECT_INVOICE").TrimEnd
            _invoiceInfo = dr.GetString("INVOICE_INFO").TrimEnd
            _invoiceNo = dr.GetString("INVOICE_NO").TrimEnd
            _invoiceSerial = dr.GetString("INVOICE_SERIAL").TrimEnd
            _invoiceBook = dr.GetString("INVOICE_BOOK").TrimEnd
            _invoiceDate.Text = dr.GetInt32("INVOICE_DATE")
            _invoicePeriod.Text = dr.GetInt32("INVOICE_PERIOD")
            _vatRate = dr.GetString("VAT_RATE").TrimEnd
            _vatAmount.Text = dr.GetDecimal("VAT_AMOUNT")
            _ncPl0 = dr.GetString("NC_PL0").TrimEnd
            _ncPl1 = dr.GetString("NC_PL1").TrimEnd
            _ncPl2 = dr.GetString("NC_PL2").TrimEnd
            _ncPl3 = dr.GetString("NC_PL3").TrimEnd
            _ncPl4 = dr.GetString("NC_PL4").TrimEnd
            _ncPl5 = dr.GetString("NC_PL5").TrimEnd
            _ncPl6 = dr.GetString("NC_PL6").TrimEnd
            _ncPl7 = dr.GetString("NC_PL7").TrimEnd
            _ncPl8 = dr.GetString("NC_PL8").TrimEnd
            _ncPl9 = dr.GetString("NC_PL9").TrimEnd
            _allocation = dr.GetString("ALLOCATION").TrimEnd
            _allocRef = dr.GetInt32("ALLOC_REF")
            _allocDate.Text = dr.GetInt32("ALLOC_DATE")
            _allocPeriod.Text = dr.GetInt32("ALLOC_PERIOD")
            _status = dr.GetString("STATUS").TrimEnd
            _lockFlag = dr.GetString("LOCK_FLAG").TrimEnd
            _postingDate.Text = dr.GetInt32("POSTING_DATE")
            _postedBy = dr.GetString("POSTED_BY").TrimEnd
            _holdOpId = dr.GetString("HOLD_OP_ID").TrimEnd
            _bphNo = dr.GetInt32("BPH_NO")
            _pfdNo = dr.GetInt32("PFD_NO")
            _extDesc1 = dr.GetString("EXT_DESC1").TrimEnd
            _extDesc2 = dr.GetString("EXT_DESC2").TrimEnd
            _extDesc3 = dr.GetString("EXT_DESC3").TrimEnd
            _extDesc4 = dr.GetString("EXT_DESC4").TrimEnd
            _extDesc5 = dr.GetString("EXT_DESC5").TrimEnd
            _extDate1.Text = dr.GetInt32("EXT_DATE1")
            _extDate2.Text = dr.GetInt32("EXT_DATE2")
            _extDate3.Text = dr.GetInt32("EXT_DATE3")
            _extDate4.Text = dr.GetInt32("EXT_DATE4")
            _extDate5.Text = dr.GetInt32("EXT_DATE5")
            _extVal1.Text = dr.GetDecimal("EXT_VAL1")
            _extVal2.Text = dr.GetDecimal("EXT_VAL2")
            _extVal3.Text = dr.GetDecimal("EXT_VAL3")
            _extVal4.Text = dr.GetDecimal("EXT_VAL4")
            _extVal5.Text = dr.GetDecimal("EXT_VAL5")

        End Sub

        Private Shared _lockObj As New Object
        Protected Overrides Sub DataPortal_Insert()
            SyncLock _lockObj
                Using ctx = ConnectionFactory.GetDBConnection(True)
                    
                    ExecuteInsert(ctx)
                    End Using

            End SyncLock
        End Sub

        Private Sub ExecuteInsert(ByVal cn As IDbConnection)
            Using cm = cn.CreateSQLCommand()
                cm.CommandType = CommandType.StoredProcedure
                cm.CommandText = String.Format("pbs_MC_LEDGER_{0}_Insert", _DTB)

                cm.Parameters.AddWithValue("@LINE_NO", _lineNo, ParameterDirection.Output)
                AddInsertParameters(cm)
                cm.ExecuteNonQuery()

                _lineNo = cm.GetReturnIdAsInteger("@LINE_NO")

                MarkOld()
            End Using
        End Sub

        Private Sub AddInsertParameters(ByVal cm As IDBCommand)

            If _status.Equals(JE.STR_Post) Then _lockFlag = "Y"


            cm.Parameters.AddWithValue("@TRANS_TYPE", _transType.Trim)
            cm.Parameters.AddWithValue("@REFERENCE", _reference.Trim)
            cm.Parameters.AddWithValue("@TRANS_DATE", _transDate.DBValue)
            cm.Parameters.AddWithValue("@PERIOD", _period.DBValue)
            cm.Parameters.AddWithValue("@CANDIDATE_ID", _candidateId.DBValue)
            cm.Parameters.AddWithValue("@PATIENT_CODE", _patientCode.Trim)
            cm.Parameters.AddWithValue("@CLINIC", _clinic.Trim)
            'cm.Parameters.AddWithValue("@TRANS_CAMPUS", _transCampus.Trim)
            'cm.Parameters.AddWithValue("@PROGRAM", _program.Trim)
            cm.Parameters.AddWithValue("@ITEM_CODE", _itemCode.Trim)
            cm.Parameters.AddWithValue("@DESCRIPTION", _description.Trim)
            cm.Parameters.AddWithValue("@QUANTITY", _quantity.Float)
            cm.Parameters.AddWithValue("@UNIT_CODE", _unitCode.Trim)
            cm.Parameters.AddWithValue("@UNIT_PRICE", _unitPrice.DBValue)
            cm.Parameters.AddWithValue("@TRANS_AMT", _transAmt.DBValue)
            cm.Parameters.AddWithValue("@CONV_CODE", _convCode.Trim)
            cm.Parameters.AddWithValue("@CONV_RATE", _convRate.DBValue)
            cm.Parameters.AddWithValue("@AMOUNT", _amount.DBValue)

            cm.Parameters.AddWithValue("@D_C", _dC.Trim)

            cm.Parameters.AddWithValue("@PAYMENT_REF", _paymentRef.Trim)
            cm.Parameters.AddWithValue("@PAY_METHOD", _payMethod.Trim)
            cm.Parameters.AddWithValue("@PAYMENT_DATE", _paymentDate.DBValue)
            cm.Parameters.AddWithValue("@PAYMENT_PERIOD", _paymentPeriod.DBValue)
            cm.Parameters.AddWithValue("@DIRECT_INVOICE", _directInvoice.Trim)
            cm.Parameters.AddWithValue("@INVOICE_INFO", _invoiceInfo.Trim)
            cm.Parameters.AddWithValue("@INVOICE_NO", _invoiceNo.Trim)
            cm.Parameters.AddWithValue("@INVOICE_SERIAL", _invoiceSerial.Trim)
            cm.Parameters.AddWithValue("@INVOICE_BOOK", _invoiceBook.Trim)
            cm.Parameters.AddWithValue("@INVOICE_DATE", _invoiceDate.DBValue)
            cm.Parameters.AddWithValue("@INVOICE_PERIOD", _invoicePeriod.DBValue)
            cm.Parameters.AddWithValue("@VAT_RATE", _vatRate.Trim)
            cm.Parameters.AddWithValue("@VAT_AMOUNT", _vatAmount.DBValue)
            cm.Parameters.AddWithValue("@NC_PL0", _ncPl0.Trim)
            cm.Parameters.AddWithValue("@NC_PL1", _ncPl1.Trim)
            cm.Parameters.AddWithValue("@NC_PL2", _ncPl2.Trim)
            cm.Parameters.AddWithValue("@NC_PL3", _ncPl3.Trim)
            cm.Parameters.AddWithValue("@NC_PL4", _ncPl4.Trim)
            cm.Parameters.AddWithValue("@NC_PL5", _ncPl5.Trim)
            cm.Parameters.AddWithValue("@NC_PL6", _ncPl6.Trim)
            cm.Parameters.AddWithValue("@NC_PL7", _ncPl7.Trim)
            cm.Parameters.AddWithValue("@NC_PL8", _ncPl8.Trim)
            cm.Parameters.AddWithValue("@NC_PL9", _ncPl9.Trim)
            cm.Parameters.AddWithValue("@ALLOCATION", _allocation.Trim)
            cm.Parameters.AddWithValue("@ALLOC_REF", _allocRef)
            cm.Parameters.AddWithValue("@ALLOC_DATE", _allocDate.DBValue)
            cm.Parameters.AddWithValue("@ALLOC_PERIOD", _allocPeriod.DBValue)
            cm.Parameters.AddWithValue("@STATUS", _status.Trim)

            cm.Parameters.AddWithValue("@LOCK_FLAG", _lockFlag.Trim)

            If Me.IsChild Then
                cm.Parameters.AddWithValue("@POSTING_DATE", _postingDate.DBValue)
                cm.Parameters.AddWithValue("@POSTED_BY", _postedBy.Trim)
            Else
                cm.Parameters.AddWithValue("@POSTING_DATE", ToDay.ToSunDate)
                cm.Parameters.AddWithValue("@POSTED_BY", Context.CurrentUserCode)
            End If

            cm.Parameters.AddWithValue("@HOLD_OP_ID", _holdOpId.Trim)
            cm.Parameters.AddWithValue("@BPH_NO", _bphNo)
            cm.Parameters.AddWithValue("@PFD_NO", _pfdNo)
            cm.Parameters.AddWithValue("@EXT_DESC1", _extDesc1.Trim)
            cm.Parameters.AddWithValue("@EXT_DESC2", _extDesc2.Trim)
            cm.Parameters.AddWithValue("@EXT_DESC3", _extDesc3.Trim)
            cm.Parameters.AddWithValue("@EXT_DESC4", _extDesc4.Trim)
            cm.Parameters.AddWithValue("@EXT_DESC5", _extDesc5.Trim)
            cm.Parameters.AddWithValue("@EXT_DATE1", _extDate1.DBValue)
            cm.Parameters.AddWithValue("@EXT_DATE2", _extDate2.DBValue)
            cm.Parameters.AddWithValue("@EXT_DATE3", _extDate3.DBValue)
            cm.Parameters.AddWithValue("@EXT_DATE4", _extDate4.DBValue)
            cm.Parameters.AddWithValue("@EXT_DATE5", _extDate5.DBValue)
            cm.Parameters.AddWithValue("@EXT_VAL1", _extVal1.DBValue)
            cm.Parameters.AddWithValue("@EXT_VAL2", _extVal2.DBValue)
            cm.Parameters.AddWithValue("@EXT_VAL3", _extVal3.DBValue)
            cm.Parameters.AddWithValue("@EXT_VAL4", _extVal4.DBValue)
            cm.Parameters.AddWithValue("@EXT_VAL5", _extVal5.DBValue)
        End Sub

        Protected Overrides Sub DataPortal_Update()
            Using ctx = ConnectionFactory.GetDBConnection(True)
                
                ExecuteUpdate(ctx)
                End Using
        End Sub

        Private Sub ExecuteUpdate(ByVal cn As IDbConnection)
            Using cm = cn.CreateSQLCommand
                cm.CommandType = CommandType.StoredProcedure
                cm.CommandText = String.Format("pbs_MC_LEDGER_{0}_Update", _DTB)
                cm.Parameters.AddWithValue("@LINE_NO", _lineNo)
                AddInsertParameters(cm)
                cm.ExecuteNonQuery()
            End Using
        End Sub

        Protected Overrides Sub DataPortal_DeleteSelf()
            DataPortal_Delete(New Criteria(_lineNo))
        End Sub

        Private Overloads Sub DataPortal_Delete(ByVal criteria As Criteria)
        Using ctx = ConnectionFactory.GetDBConnection(True)
                Using cm = ctx.CreateSQLCommand

                    cm.CommandText = <SqlText>DELETE FROM pbs_MC_LEDGER_<%= _DTB %> WHERE LINE_NO= <%= criteria._lineNo %> AND NOT LOCK_FLAG='Y'</SqlText>.Value.Trim
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

            Dim SqlText = <SqlText>SELECT COUNT(*) FROM pbs_MC_LEDGER_<%= Context.CurrentBECode %> WHERE LINE_NO=<%= pLineNo %></SqlText>.Value.Trim

            Return SQLCommander.GetScalarInteger(SqlText) > 0
        End Function
#End Region

#Region " IGenpart "

        Public Function CloneBO(ByVal id As String) As Object Implements Interfaces.IGenPartObject.CloneBO
            Return CloneMCLDG(id)
        End Function

        Public Function getBO1(ByVal id As String) As Object Implements Interfaces.IGenPartObject.GetBO
            Return GetBO(id)
        End Function

        Public Function myCommands() As String() Implements Interfaces.IGenPartObject.myCommands
            Return pbs.Helper.Action.StandardReferenceCommands
        End Function

        Public Function myFullName() As String Implements Interfaces.IGenPartObject.myFullName
            Return GetType(MCLDG).ToString
        End Function

        Public Function myName() As String Implements Interfaces.IGenPartObject.myName
            Return GetType(MCLDG).ToString.Leaf
        End Function

        Public Function myQueryList() As IList Implements Interfaces.IGenPartObject.myQueryList
            Return MCLDGInfoList.GetMCLDGInfoList
        End Function
#End Region

#Region "IDoclink"
        Public Function Get_DOL_Reference() As String Implements IDocLink.Get_DOL_Reference
            Return String.Format("{0}#{1}", GetType(MCLDG).ToString, _reference)
        End Function

        Public Function Get_DOLTransType() As String Implements IDocLink.Get_TransType
            Return Me.GetType.ToClassSchemaName.Leaf
        End Function
#End Region

#Region "ISupportQuery"
        Public Function GetInfoList(pFilter As Dictionary(Of String, String)) As IEnumerable Implements ISupportQueryInfoList.GetInfoList
            'If pFilter Is Nothing OrElse pFilter.Count = 0 Then pbs.Helper.UIServices.AlertService.Alert("Patient Receivable ledger return empty list without criteria")
            Return MCLDGInfoList.GetMCLDGInfoList(pFilter)
        End Function
#End Region

#Region "ISupportCachedLookup"
        Public Function LookupFor(KeyValue As String, ReturnField As String) As String Implements ISupportCachedLookup.LookupFor
            Dim info = MCLDGInfoList.GetMCLDGInfo(KeyValue.ToInteger)
            If info IsNot Nothing Then
                If String.IsNullOrEmpty(ReturnField) Then
                    Return info.Description
                ElseIf ReturnField.StartsWith("=") Then
                    Return PhoebusAPI.Evaluate(ReturnField.RegExpReplace("^=", String.Empty), info)
                Else
                    Return pbs.Helper.DataMapper.GetPropertyValue(info, ReturnField, String.Empty)
                End If
            Else
                Return String.Empty
            End If
        End Function
#End Region

#Region "Line Reversal"

        Public Function ReverseJournalLine(pPeriod As String) As Integer
            If LineNo <= 0 OrElse Me.IsNew Then
                ExceptionThower.BusinessRuleStop("Please select an existing journal Line first")
                Return 0
            ElseIf Me.Allocation.MatchesRegExp("^[A|P|C]$") Then
                ExceptionThower.BusinessRuleStop(String.Format(ResStr("Line {0} has been allocated."), LineNo))
                Return 0
            Else
                If String.IsNullOrEmpty(pPeriod) Then pPeriod = Me.Period

                Dim reversal = Me.Clone
                reversal.MarkAsNewClone()
                reversal.ReverseMe()

                reversal.Period = pPeriod
                'reversal.TransDate = "T"

                reversal.ValidationRules.CheckRules("Period")
                reversal.ValidationRules.CheckRules("TransactionDate")

                reversal._status = JE.STR_Post
                reversal = reversal.Save

                Dim allocRef As Integer = Me.LineNo

                MarkAsPostedCorrection(Me.LineNo, reversal.LineNo, allocRef)

                ALOG.Log(Me, "Reverse")

                pbs.Helper.UIServices.AlertService.Alert("Line# {0} has been reversed by a new Line {1}", LineNo, reversal.LineNo)

                Return reversal.LineNo

            End If
        End Function

        ''' <summary>
        ''' use script to marked bot journal as Correction
        ''' </summary>
        ''' <param name="original"></param>
        ''' <param name="ReversalNo"></param>
        ''' <remarks></remarks>
        Private Shared Sub MarkAsPostedCorrection(original As Integer, ReversalNo As Integer, AllocRef As Integer)
            If original = 0 OrElse ReversalNo = 0 Then Exit Sub
            Dim det = <detail>UPDATE pbs_MC_LEDGER_<%= Context.CurrentBECode %> SET DESCRIPTION=substring('REVERSE-' + DESCRIPTION,1,200) , ALLOCATION='C' , ALLOC_REF=<%= AllocRef %>, ALLOC_DATE=<%= ToDay.ToSunDate %>, STATUS='Reverse', LOCK_FLAG='Y' WHERE LINE_NO=<%= original %> OR LINE_NO=<%= ReversalNo %></detail>.Value.Trim

            SQLCommander.RunInsertUpdate(det)
        End Sub

#End Region
    End Class

End Namespace
