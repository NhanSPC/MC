Imports pbs.Helper
Imports System.Data

Imports Csla
Imports Csla.Data
Imports Csla.Validation
Imports pbs.BO.DataAnnotations
Imports pbs.BO.Script
Imports pbs.Helper.Interfaces
Imports pbs.BO.BusinessRules
Imports System.Text.RegularExpressions
Imports pbs.BO.SM

Namespace MC

    <Serializable()> _
    <DB(TableName:="pbs_MC_JRNAL_XXX")>
    <PhoebusCommand(Desc:="Journal Entry (MC)")>
    Public Class JE
        Inherits Csla.BusinessBase(Of JE)
        Implements Interfaces.IGenPartObject
        Implements IComparable
        Implements IDocLink
        Implements ISupportQueryInfoList
        Implements ILockable
        Implements ISupportScripts
        Implements ISupportCommandAuthorization
        Implements Interfaces.ISupportBrokenRuleMsg
        Implements ISupportDocumentDataSet
        Implements ISupportPreset


#Region "Property Changed"
        Protected Overrides Sub OnDeserialized(context As Runtime.Serialization.StreamingContext)
            MyBase.OnDeserialized(context)
            AddHandler Me.PropertyChanged, AddressOf BO_PropertyChanged
            AddHandler _lines.ListChanged, AddressOf _lines_ListChanged
        End Sub

        Private Sub BO_PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Handles Me.PropertyChanged
            Select Case e.PropertyName

                Case "PatientCode"
                    UpdatePatientInfo()

                Case "TransDate"
                    For Each itm In Lines
                        itm.TransDate = Me.TransDate
                        itm.Period = Me.Period
                    Next

                Case "Period"
                    For Each itm In Lines
                        itm.Period = Me.Period
                    Next

                Case "TransType"
                    If String.IsNullOrEmpty(Me._description) Then Me._description = GetJrnalTypeInfo.JournalName
                    PresetReferenceIfNeeded()

                Case "ConvCode"
                    If String.IsNullOrEmpty(Me.ConvCode) Then
                        _convRate.Float = 0
                    Else
                        Try
                            Dim conv = pbs.BO.LA.DCInfoList.GetConverter(Me.ConvCode, _transDate, String.Empty)
                            If conv IsNot Nothing Then
                                _convRate.Float = conv.DefaultRate
                            End If
                        Catch ex As Exception
                        End Try
                    End If

                Case "TotalReceived"
                    _totalReturn.Float = _totalReceived.Float - _net
                    If _totalReturn.Float < 0 Then
                        _totalReturn.Float = 0
                    End If
                    '    Case Else

            End Select

            pbs.BO.Rules.CalculationRules.Calculator(sender, e)

            If GetJrnalTypeInfo.GetMasterFields.Contains(e.PropertyName) Then
                PopulateMasterDataToDetails()
            End If

        End Sub

        ''' <summary>
        ''' Copy data input from header to all detail lines
        ''' </summary>
        ''' <remarks></remarks>
        ''' 
        Public Sub PopulateMasterDataToDetails()
            'todo: check this latyer

            Dim thePresetDic = BOFactory.Obj2ShapedDictionary(Me, GetJrnalTypeInfo.Get_Detail_From_Master_Mapping)

            If thePresetDic Is Nothing OrElse thePresetDic.Count = 0 Then Exit Sub

            Dim NeedReverse As Boolean = False
            For Each itm In Lines
                BOFactory.ApplyPreset(itm, thePresetDic)

                itm.SetDC()
                itm.TransDate = Me.TransDate
                itm.Period = Me.Period
                itm._postedBy = Me.PostedBy
                itm._postingDate.Date = Me._postingDate.Date
            Next

            _lines.ResetBindings()

        End Sub

        Private Sub UpdatePatientInfo()

            ' If String.IsNullOrEmpty(CandidateId) Then
            ' _candidateId.Text = GetCANDIDATEInfo.CddCode
            'End If
            '_PatientName = GetStudendInfo.Description
            '_patientName = String.Format("{0} {1} {2}", GetPatientInfo.FirstName, GetPatientInfo.MiddleName, GetPatientInfo.LastName)
            _patientName = String.Format("{0} {1}", GetPatientInfo.FirstName, GetPatientInfo.LastName)
            '_clinic = GetPATIENTInfo.CurrentSchoolId
            '_classId = GetStudendInfo.CurrentClassId
            '_program = GetStudendInfo.GetCLAInfo.Program
            '_invoiceInfo = GetStudendInfo.TaxId

            If Not String.IsNullOrEmpty(_invoiceInfo) Then
                _directInvoice = "Y"
            End If
        End Sub
#End Region

#Region "Behavior"

        <NonSerialized>
        Private _PATIENTInfo As PATIENTInfo = Nothing
        Public Function GetPatientInfo() As PATIENTInfo
            If _PATIENTInfo Is Nothing OrElse Not _PATIENTInfo.PatientCode.Equals(_patientCode) Then
                _PATIENTInfo = PATIENTInfoList.GetPATIENTInfo(_patientCode)
            End If
            Return _PATIENTInfo
        End Function

        <NonSerialized>
        Private _jdInfo As JDInfo = Nothing
        Public Function GetJrnalTypeInfo() As JDInfo
            If _jdInfo Is Nothing OrElse Not _jdInfo.JournalType.Equals(_transType) Then
                _jdInfo = JDInfoList.GetJDInfo(_transType)
            End If
            Return _jdInfo
        End Function

        Private Sub PresetReferenceIfNeeded()
            If Not String.IsNullOrEmpty(GetJrnalTypeInfo.TrefSequence) Then
                Reference = "?" & SEQ.EstimatenextNumber(GetJrnalTypeInfo.TrefSequence)
            End If
        End Sub

        ''' <summary>
        ''' Set number right before saving
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub TakeReferenceIfNeeded()
            If _reference.StartsWith("?") AndAlso Not String.IsNullOrEmpty(GetJrnalTypeInfo.TrefSequence) Then
                _reference = SEQ.TakeNumber(GetJrnalTypeInfo.TrefSequence)
            End If
        End Sub

#End Region

#Region " Business Properties and Methods "
        Friend _DTB As String = String.Empty

        Private _jrnalNo As Integer
        <System.ComponentModel.DataObjectField(True, True)> _
        Public ReadOnly Property JrnalNo() As String
            Get
                Return _jrnalNo
            End Get
        End Property

        Protected Overrides Function GetSubForm() As String
            Return _transType
        End Function

        Private _transType As String = String.Empty
        <CellInfo("pbs.BO.MC.JD", GroupName:="General", Tips:="Select Journal Type from the list")>
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
                    'CType(Me, Csla.Core.ISupportSubform).GetSubForm(_transType)
                    PropertyHasChanged("TransType")
                End If
            End Set
        End Property

        Private _reference As String = String.Empty '<Rule(Required:=True)>
        <CellInfo(GroupName:="General", Tips:="Enter Document Reference to this journal")>
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

        Friend _transDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate() '<Rule(Required:=True)>
        <CellInfo(LinkCode.Calendar, GroupName:="General")>
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
                    If String.IsNullOrEmpty(Period) AndAlso Not String.IsNullOrEmpty(TransDate) Then
                        Period = _transDate.Date.ToSunPeriod
                    End If
                End If
            End Set
        End Property

        Friend _period As SmartPeriod = New pbs.Helper.SmartPeriod() '<Rule(Required:=True)>
        <CellInfo(LinkCode.Period, GroupName:="General")>
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

        Private _oldDescription As String = String.Empty
        Private _description As String = String.Empty
        <CellInfo(GroupName:="General")>
        Public Property Description() As String
            Get
                Return _description
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Description", True)
                If value Is Nothing Then value = String.Empty
                If Not _description.Equals(value) Then
                    If String.IsNullOrEmpty(_oldDescription) Then _oldDescription = _description
                    _description = value
                    PropertyHasChanged("Description")
                End If
            End Set
        End Property

        Private _candidateId As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        '<CellInfo("pbs.BO.MC.CAN", GroupName:="Payer")>
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
        <CellInfo("pbs.BO.MC.PATIENT", GroupName:="Payer")>
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

        Private _patientName As String = String.Empty
        Public ReadOnly Property PatientName As String
            Get
                Return _patientName
            End Get
        End Property

        Private _clinic As String = String.Empty
        <CellInfo(GroupName:="Clinic")>
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

        Private _convCode As String = String.Empty
        <CellInfo(LinkCode.Conversion)>
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
        Public Property ConvRate() As Decimal
            Get
                Return _convRate.Float
            End Get
            Set(ByVal value As Decimal)
                CanWriteProperty("ConvRate", True)
                If Not _convRate.Equals(value) Then
                    _convRate.Float = Math.Abs(value)
                    PropertyHasChanged("ConvRate")
                End If
            End Set
        End Property

        Private _paymentRef As String = String.Empty
        <CellInfo(GroupName:="Other Reference")>
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
        <CellInfo(GroupName:="Other Reference")>
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
        <CellInfo(LinkCode.Calendar, GroupName:="Other Reference")>
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

                    If String.IsNullOrEmpty(PaymentPeriod) AndAlso Not String.IsNullOrEmpty(PaymentDate) Then
                        PaymentPeriod = _paymentDate.Date.ToSunPeriod
                    End If
                End If
            End Set
        End Property

        Private _paymentPeriod As SmartPeriod = New pbs.Helper.SmartPeriod()
        <CellInfo(GroupName:="Other Reference")>
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
        <CellInfo(GroupName:="Billing")>
        Public Property DirectInvoice() As Boolean
            Get
                Return _directInvoice.ToBoolean
            End Get
            Set(ByVal value As Boolean)
                If Not DirectInvoice.Equals(value) Then
                    _directInvoice = If(value, "Y", "N")
                    PropertyHasChanged("DirectInvoice")
                End If
            End Set
        End Property

        Private _invoiceInfo As String = String.Empty
        <CellInfo(GroupName:="Billing")>
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
        <CellInfo(GroupName:="Billing")>
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
        <CellInfo(GroupName:="Billing")>
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
        <CellInfo(GroupName:="Billing")>
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
        <CellInfo(GroupName:="Billing")>
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

                    If String.IsNullOrEmpty(InvoicePeriod) AndAlso Not String.IsNullOrEmpty(InvoiceDate) Then
                        InvoicePeriod = _invoiceDate.Date.ToSunPeriod
                    End If

                End If
            End Set
        End Property

        Private _invoicePeriod As SmartPeriod = New pbs.Helper.SmartPeriod()
        <CellInfo(GroupName:="Billing")>
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
        <CellInfo(GroupName:="Billing")>
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

        Private _ncPl0 As String = String.Empty
        Public Property NcPl0() As String
            Get
                Return _ncPl0
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcPl0", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncPl0.Equals(value) Then
                    _ncPl0 = value
                    PropertyHasChanged("NcPl0")
                End If
            End Set
        End Property

        Private _ncPl1 As String = String.Empty
        Public Property NcPl1() As String
            Get
                Return _ncPl1
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcPl1", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncPl1.Equals(value) Then
                    _ncPl1 = value
                    PropertyHasChanged("NcPl1")
                End If
            End Set
        End Property

        Private _ncPl2 As String = String.Empty
        Public Property NcPl2() As String
            Get
                Return _ncPl2
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcPl2", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncPl2.Equals(value) Then
                    _ncPl2 = value
                    PropertyHasChanged("NcPl2")
                End If
            End Set
        End Property

        Private _ncPl3 As String = String.Empty
        Public Property NcPl3() As String
            Get
                Return _ncPl3
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcPl3", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncPl3.Equals(value) Then
                    _ncPl3 = value
                    PropertyHasChanged("NcPl3")
                End If
            End Set
        End Property

        Private _ncPl4 As String = String.Empty
        Public Property NcPl4() As String
            Get
                Return _ncPl4
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcPl4", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncPl4.Equals(value) Then
                    _ncPl4 = value
                    PropertyHasChanged("NcPl4")
                End If
            End Set
        End Property

        Private _ncPl5 As String = String.Empty
        Public Property NcPl5() As String
            Get
                Return _ncPl5
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcPl5", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncPl5.Equals(value) Then
                    _ncPl5 = value
                    PropertyHasChanged("NcPl5")
                End If
            End Set
        End Property

        Private _ncPl6 As String = String.Empty
        Public Property NcPl6() As String
            Get
                Return _ncPl6
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcPl6", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncPl6.Equals(value) Then
                    _ncPl6 = value
                    PropertyHasChanged("NcPl6")
                End If
            End Set
        End Property

        Private _ncPl7 As String = String.Empty
        Public Property NcPl7() As String
            Get
                Return _ncPl7
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcPl7", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncPl7.Equals(value) Then
                    _ncPl7 = value
                    PropertyHasChanged("NcPl7")
                End If
            End Set
        End Property

        Private _ncPl8 As String = String.Empty
        Public Property NcPl8() As String
            Get
                Return _ncPl8
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcPl8", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncPl8.Equals(value) Then
                    _ncPl8 = value
                    PropertyHasChanged("NcPl8")
                End If
            End Set
        End Property

        Private _ncPl9 As String = String.Empty
        Public Property NcPl9() As String
            Get
                Return _ncPl9
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcPl9", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncPl9.Equals(value) Then
                    _ncPl9 = value
                    PropertyHasChanged("NcPl9")
                End If
            End Set
        End Property

        Private _status As String = String.Empty
        <CellInfo(GroupName:="System", Hidden:=True)>
        Public ReadOnly Property Status() As String
            Get
                Return _status
            End Get
        End Property

        Private _postingDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo(GroupName:="System", Hidden:=True)>
        Public ReadOnly Property PostingDate() As String
            Get
                Return _postingDate.Text
            End Get
        End Property

        Private _postedBy As String = String.Empty
        <CellInfo(GroupName:="System", Hidden:=True)>
        Public ReadOnly Property PostedBy() As String
            Get
                Return _postedBy
            End Get
        End Property

        Private _extDesc1 As String = String.Empty
        <CellInfo(GroupName:="Extended Info", Hidden:=True)>
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
        <CellInfo(GroupName:="Extended Info", Hidden:=True)>
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
        <CellInfo(GroupName:="Extended Info", Hidden:=True)>
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
        <CellInfo(GroupName:="Extended Info", Hidden:=True)>
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
        <CellInfo(GroupName:="Extended Info", Hidden:=True)>
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
        <CellInfo(LinkCode.Calendar, GroupName:="Extended Info", Hidden:=True)>
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
        <CellInfo(LinkCode.Calendar, GroupName:="Extended Info", Hidden:=True)>
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
        <CellInfo(LinkCode.Calendar, GroupName:="Extended Info", Hidden:=True)>
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
        <CellInfo(LinkCode.Calendar, GroupName:="Extended Info", Hidden:=True)>
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
        <CellInfo(LinkCode.Calendar, GroupName:="Extended Info", Hidden:=True)>
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
        <CellInfo(GroupName:="Extended Info", Hidden:=True)>
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

        Private _extVal2 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="Extended Info", Hidden:=True)>
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

        Private _extVal3 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="Extended Info", Hidden:=True)>
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

        Private _extVal4 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="Extended Info", Hidden:=True)>
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

        Private _extVal5 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="Extended Info", Hidden:=True)>
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

        Private _entryDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        <CellInfo(GroupName:="System", Hidden:=True)>
        Public ReadOnly Property EntryDate() As String
            Get
                Return _entryDate.Text
            End Get
        End Property

        Private _entryBy As String = String.Empty
        <CellInfo(GroupName:="System", Hidden:=True)>
        Public ReadOnly Property EntryBy() As String
            Get
                Return _entryBy
            End Get
        End Property

        'Get ID
        Protected Overrides Function GetIdValue() As Object
            Return _jrnalNo
        End Function

        'IComparable
        Public Function CompareTo(ByVal IDObject) As Integer Implements System.IComparable.CompareTo
            Dim ID = IDObject.ToString
            Dim pJrnalNo As Integer = ID.Trim.ToInteger
            If _jrnalNo < pJrnalNo Then Return -1
            If _jrnalNo > pJrnalNo Then Return 1
            Return 0
        End Function

#End Region 'Business Properties and Methods

#Region "Total"
        Private _total As Decimal = 0
        <CellInfo(GroupName:="Total")>
        ReadOnly Property Total As Decimal
            Get
                Return _total
            End Get
        End Property


        Private _VAT As Decimal = 0
        <CellInfo(GroupName:="Total")>
        ReadOnly Property VAT As Decimal
            Get
                Return _VAT
            End Get
        End Property

        Private _net As Decimal = 0
        <CellInfo(GroupName:="Total")>
        ReadOnly Property NET As Decimal
            Get
                Return _net
            End Get
        End Property

        Private _totalReceived As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="Total")>
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
        <CellInfo(GroupName:="Total")>
        Public ReadOnly Property TotalReturn() As String
            Get
                Return _totalReturn.Text
            End Get
        End Property
#End Region

#Region "Children properties"
        ' declare child member(s)
        Private WithEvents _lines As MCLDGs = MCLDGs.NewMCLDGs(Me)

        <TableRangeInfo("Lines")>
        Public ReadOnly Property Lines() As MCLDGs
            Get
                If _lines Is Nothing Then
                    _lines = MCLDGs.NewMCLDGs(Me)
                End If
                Return _lines
            End Get
        End Property

        <ComponentModel.Browsable(False)>
        Public Overrides ReadOnly Property IsValid() As Boolean
            Get
                Return MyBase.IsValid AndAlso Lines.IsValid
            End Get
        End Property

        <ComponentModel.Browsable(False)>
        Public Overrides ReadOnly Property IsDirty() As Boolean
            Get
                Return MyBase.IsDirty OrElse Lines.IsDirty
            End Get
        End Property

        Private Sub _lines_ListChanged(sender As Object, e As ComponentModel.ListChangedEventArgs) Handles _lines.ListChanged
            RecalculateTotal()

            'If e.ListChangedType = ComponentModel.ListChangedType.ItemChanged AndAlso e.PropertyDescriptor.DisplayName = "Description" Then
            '    RenderJournalDescription()
            'End If

            PropertyHasChanged()
        End Sub

        Private Sub RenderDefaultJournalDescription()
            Try
                If String.IsNullOrEmpty(_oldDescription) Then
                    'user did not change the description
                    Dim desc = (From line As MCLDG In Me.Lines Where Not String.IsNullOrEmpty(line.ItemCode) OrElse line.ItemInfo.StdCost >= 0 Select line.Description Distinct)

                    If desc.Count > 0 Then
                        _description = String.Join(",", desc.ToArray)
                    End If
                End If

            Catch ex As Exception
            End Try
        End Sub

        Private Sub RecalculateTotal()
            _VAT = 0
            _net = 0
            _total = 0

            Lines.RaiseListChangedEvents = False

            For Each line In Lines
                _VAT += (line._vatAmount.Float).RoundBA
                _net += (line._amount.Float).RoundBA
                _total = (_VAT + _net).RoundBA
            Next

            Lines.RaiseListChangedEvents = True
        End Sub

#End Region

#Region "Validation Rules"

        Public Function BrokenRulesMsg() As String Implements ISupportBrokenRuleMsg.BrokenRulesMsg
            If IsValid Then Return String.Empty
            Dim _brokenrules As New System.Text.StringBuilder

            If Not Me.IsValid Then
                _brokenrules.Append(Me.BrokenRulesCollection.ToString)
                _brokenrules.AppendLine()
                For Each line In Me.Lines
                    If Not line.IsValid Then
                        _brokenrules.AppendFormat(ResStr("Line {0} contains invalid data :"), line.LineNo)
                        _brokenrules.AppendLine()
                        _brokenrules.AppendFormat("---- {0}", line.BrokenRulesCollection.ToString())
                        _brokenrules.AppendLine()
                    End If

                Next
            End If

            Return _brokenrules.ToString
        End Function

        'Private Shared Function PayeeValidator(target As Object, r As RuleArgs) As Boolean
        '    Dim _j As JE = target
        '    If String.IsNullOrEmpty(_j.PatientCode) AndAlso String.IsNullOrEmpty(_j.CandidateId) Then
        '        r.Description = String.Format(ResStr(ResStrConst.InputRequired), r.PropertyName)
        '        Return False
        '    End If
        '    Return True
        'End Function

        Private Sub AddSharedCommonRules()
            'Sample simple custom rule
            'ValidationRules.AddRule(AddressOf PayeeValidator, "PatientCode", 1)
            'ValidationRules.AddRule(AddressOf PayeeValidator, "CandidateId", 1)

            'Sample dependent property. when check one , check the other as well

            ValidationRules.AddRule(AddressOf SM.Settings.ContainsValidPeriod, "Period", 1)

            ValidationRules.AddRule(AddressOf SM.Settings.ContainsValidDate, "TransDate", 1)
        End Sub

        Protected Overrides Sub AddBusinessRules()
            AddSharedCommonRules()

            For Each _field As ClassField In ClassSchema(Of JE)._fieldList
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
                        Case "PatientCode"
                            ValidationRules.AddRule(AddressOf PATIENTInfoList.ContainsCode, _field.FieldName)
                        Case "TransType"
                            ValidationRules.AddRule(AddressOf JDInfoList.ContainsCode, _field.FieldName)
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
            _transDate.Text = ToDay.ToSunDate
            _period.Text = ToDay.Month
        End Sub

        Public Shared Function BlankJE() As JE
            Return New JE
        End Function

        Public Shared Function NewJE(ByVal pJrnalNo As String) As JE
            Return DataPortal.Create(Of JE)(New Criteria(pJrnalNo.ToInteger))

        End Function

        Public Shared Function NewBO(ByVal ID As String) As JE
            Return NewJE(0)
        End Function

        Public Shared Function GetJE(ByVal pJrnalNo As String) As JE
            Dim ret = DataPortal.Fetch(Of JE)(New Criteria(pJrnalNo))

            If Not String.IsNullOrEmpty(ret._patientCode) Then
                ret._patientName = ret.GetPatientInfo.Description
                'ElseIf Not String.IsNullOrEmpty(ret.CandidateId) Then
                '    ret._PatientName = ret.GetHR.CDDInfo.Description
            End If

            Return ret
        End Function

        Public Shared Function GetBO(ByVal ID As String) As JE
            Dim pJrnalNo As Integer = ID.Trim

            Return GetJE(pJrnalNo)
        End Function

        Public Shared Sub DeleteJE(ByVal pJrnalNo As String)
            DataPortal.Delete(New Criteria(pJrnalNo.ToInteger))
        End Sub

        Public Overrides Function Save() As JE
            If Not IsDirty Then ExceptionThower.NotDirty(ResStr(ResStrConst.NOTDIRTY))
            If Not IsSavable Then Throw New Csla.Validation.ValidationException(Me.BrokenRulesMsg)

            RenderDefaultJournalDescription()

            Me.ApplyEdit()

            TakeReferenceIfNeeded()

            PopulateMasterDataToDetails()

            For Each line In Me.Lines
                line.SetDC()
            Next

            Dim ret = MyBase.Save()
            If Not Context.IsBatchSavingMode Then JEInfoList.InvalidateCache()

            ret.OffsetAllocationIfNeeded()

            pbs.Helper.DataServices.LogService.LogAsync(ret, Action._Post)

            For Each line In ret.Lines
                pbs.Helper.DataServices.LogService.LogAsync(line, Action._Post)
            Next

            ret.Lines.SetLineDCAfterFetching()

            Return ret

        End Function

        Public Function CloneJE(ByVal pJrnalNo As String) As JE

            Dim cloning As JE = MyBase.Clone
            cloning._jrnalNo = 0
            cloning._status = String.Empty
            cloning._postedBy = String.Empty
            cloning._postingDate.Text = String.Empty
            cloning._DTB = Context.CurrentBECode
            'Todo:Remember to reset status of the new object here 
            cloning.MarkNew()
            cloning.ApplyEdit()

            For Each itm In cloning.Lines
                itm.MarkAsNewClone()
            Next

            cloning.ValidationRules.CheckRules()

            Return cloning
        End Function

        Private Function CloneAReversal() As JE

            Dim cloningJE As JE = MyBase.Clone

            cloningJE._jrnalNo = 0
            cloningJE._status = String.Empty
            cloningJE._postedBy = Context.CurrentUserCode
            cloningJE._postingDate.Text = ToDay.ToSunDate
            cloningJE._DTB = Context.CurrentBECode
            'Todo:Remember to reset status of the new object here 
            cloningJE.MarkNew()
            cloningJE.ApplyEdit()

            For Each itm In cloningJE.Lines
                itm.MarkAsNewClone()
                itm.ReverseMe()
            Next

            Return cloningJE
        End Function
#End Region ' Factory Methods

#Region " Data Access "

        <Serializable()> _
        Private Class Criteria
            Public _jrnalNo As Integer

            Public Sub New(ByVal pJrnalNo As String)
                _jrnalNo = pJrnalNo.ToInteger

            End Sub
        End Class

        <RunLocal()> _
        Private Overloads Sub DataPortal_Create(ByVal criteria As Criteria)
            _jrnalNo = criteria._jrnalNo

            ValidationRules.CheckRules()
        End Sub

        Private Overloads Sub DataPortal_Fetch(ByVal criteria As Criteria)
            Using ctx = ConnectionFactory.GetDBConnection(True)
                Using cm = ctx.CreateSQLCommand

                    cm.CommandText = <SqlText>SELECT * FROM pbs_MC_JRNAL_<%= _DTB %> WHERE JRNAL_NO=<%= criteria._jrnalNo %></SqlText>.Value.Trim
                    Using dr As New SafeDataReader(cm.ExecuteReader)
                        If dr.Read Then
                            FetchObject(dr)
                            MarkOld()
                        End If
                    End Using

                    cm.CommandText = <SqlText>SELECT * FROM pbs_MC_LEDGER_<%= _DTB %> WHERE PFD_NO=<%= criteria._jrnalNo %></SqlText>.Value.Trim
                    Using dr As New SafeDataReader(cm.ExecuteReader)
                        _lines = MCLDGs.GetMCLDGs(dr, Me)
                        RecalculateTotal()
                    End Using
                End Using
            End Using
        End Sub

        Private Sub FetchObject(ByVal dr As SafeDataReader)
            _jrnalNo = dr.GetInt32("JRNAL_NO")
            _transType = dr.GetString("TRANS_TYPE").TrimEnd
            _reference = dr.GetString("REFERENCE").TrimEnd
            _transDate.Text = dr.GetInt32("TRANS_DATE")
            _period.Text = dr.GetInt32("PERIOD")
            _description = dr.GetString("DESCRIPTION").TrimEnd
            _candidateId.Text = dr.GetInt32("CANDIDATE_ID")
            _patientCode = dr.GetString("PATIENT_CODE").TrimEnd
            _clinic = dr.GetString("CLINIC").TrimEnd
            _convCode = dr.GetString("CONV_CODE").TrimEnd
            _convRate.Text = dr.GetDecimal("CONV_RATE")
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
            _status = dr.GetString("STATUS").TrimEnd
            _postingDate.Text = dr.GetInt32("POSTING_DATE")
            _postedBy = dr.GetString("POSTED_BY").TrimEnd
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
            _entryDate.Text = dr.GetInt32("ENTRY_DATE")
            _entryBy = dr.GetString("ENTRY_BY")
        End Sub

        Private Shared _lockObj As New Object
        Protected Overrides Sub DataPortal_Insert()
            SyncLock _lockObj
                Using ctx = ConnectionFactory.GetDBConnection(True)
                    Using cm = ctx.CreateSQLCommand

                        cm.CommandType = CommandType.StoredProcedure
                        cm.CommandText = String.Format("pbs_MC_JRNAL_{0}_Insert", _DTB)

                        cm.Parameters.AddWithValue("@JRNAL_NO", _jrnalNo, ParameterDirection.Output)
                        AddInsertParameters(cm)
                        cm.ExecuteNonQuery()

                        _jrnalNo = CInt(cm.Parameters("@JRNAL_NO").Value)
                    End Using

                    Lines.Update(ctx, Me)
                    MarkOld()
                End Using
            End SyncLock
        End Sub

        Private Sub AddInsertParameters(ByVal cm As IDBCommand)

            cm.Parameters.AddWithValue("@TRANS_TYPE", _transType.Trim)
            cm.Parameters.AddWithValue("@REFERENCE", _reference.Trim)
            cm.Parameters.AddWithValue("@TRANS_DATE", _transDate.DBValue)
            cm.Parameters.AddWithValue("@PERIOD", _period.DBValue)
            cm.Parameters.AddWithValue("@DESCRIPTION", _description.Trim)
            cm.Parameters.AddWithValue("@CANDIDATE_ID", _candidateId.DBValue)
            cm.Parameters.AddWithValue("@PATIENT_CODE", _patientCode.Trim)
            cm.Parameters.AddWithValue("@CLINIC", _clinic.Trim)
            cm.Parameters.AddWithValue("@CONV_CODE", _convCode.Trim)
            cm.Parameters.AddWithValue("@CONV_RATE", _convRate.DBValue)
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
            cm.Parameters.AddWithValue("@STATUS", _status.Trim)
            cm.Parameters.AddWithValue("@POSTING_DATE", _postingDate.DBValueInt)
            cm.Parameters.AddWithValue("@POSTED_BY", _postedBy)
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
            cm.Parameters.AddWithValue("@ENTRY_DATE", ToDay.ToSunDate)
            cm.Parameters.AddWithValue("@ENTRY_BY", Context.CurrentUserCode)

        End Sub

        Protected Overrides Sub DataPortal_Update()
            Using ctx = ConnectionFactory.GetDBConnection(True)
                Using cm = ctx.CreateSQLCommand

                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = String.Format("pbs_MC_JRNAL_{0}_Update", _DTB)

                    cm.Parameters.AddWithValue("@JRNAL_NO", _jrnalNo)

                    AddInsertParameters(cm)
                    cm.ExecuteNonQuery()

                End Using

                Lines.Update(ctx, Me)
                MarkOld()
            End Using
        End Sub

        Protected Overrides Sub DataPortal_DeleteSelf()
            If Not Me._status.Equals("Posted", StringComparison.OrdinalIgnoreCase) Then
                DataPortal_Delete(New Criteria(_jrnalNo))
            Else
                TextLogger.Debug("Can not delete a posted journal")
            End If
        End Sub

        Private Overloads Sub DataPortal_Delete(ByVal criteria As Criteria)

          Using ctx = ConnectionFactory.GetDBConnection(True)
                Using cm = ctx.CreateSQLCommand

                    cm.CommandText = <SqlText>DELETE FROM pbs_MC_JRNAL_<%= _DTB %> WHERE JRNAL_NO=<%= criteria._jrnalNo %></SqlText>.Value.Trim
                    cm.ExecuteNonQuery()

                    cm.CommandText = <SqlText>DELETE pbs_MC_LEDGER_<%= _DTB %> WHERE PFD_NO= <%= criteria._jrnalNo %></SqlText>.Value.Trim
                    cm.ExecuteNonQuery()
                End Using
            End Using

        End Sub


#End Region 'Data Access                           

#Region " Exists "
        Public Shared Function Exists(ByVal pJrnalNo As String) As Boolean
            Return KeyDuplicated(pJrnalNo.ToInteger)
        End Function

        Public Shared Function KeyDuplicated(ByVal pJrnalNo As Integer) As Boolean
            If pJrnalNo <= 0 Then Return False
            Dim SqlText = <SqlText>SELECT COUNT(*) FROM pbs_MC_JRNAL_<%= Context.CurrentBECode %> WHERE JRNAL_NO=<%= pJrnalNo %></SqlText>.Value.Trim
            Return SQLCommander.GetScalarInteger(SqlText) > 0
        End Function
#End Region

#Region " IGenpart "

        Public Function CloneBO(ByVal id As String) As Object Implements Interfaces.IGenPartObject.CloneBO
            Return CloneJE(id)
        End Function

        Public Function getBO1(ByVal id As String) As Object Implements Interfaces.IGenPartObject.GetBO
            Return GetBO(id)
        End Function

        Public Function myCommands() As String() Implements Interfaces.IGenPartObject.myCommands
            Return pbs.Helper.Action.StandardReferenceCommands
        End Function

        Public Function myFullName() As String Implements Interfaces.IGenPartObject.myFullName
            Return GetType(JE).ToString
        End Function

        Public Function myName() As String Implements Interfaces.IGenPartObject.myName
            Return GetType(JE).ToString.Leaf
        End Function

        Public Function myQueryList() As IList Implements Interfaces.IGenPartObject.myQueryList
            Return Nothing
        End Function
#End Region

#Region "IDoclink"
        Public Function Get_DOL_Reference() As String Implements IDocLink.Get_DOL_Reference
            Return String.Format("{0}#{1}", Get_DOLTransType, _reference)
        End Function

        Public Function Get_DOLTransType() As String Implements IDocLink.Get_TransType
            Return GetType(MCLDG).ToString.Leaf
        End Function
#End Region

#Region "ISupportQueryInfoList"
        Public Function GetJEInfoList(pFilters As Dictionary(Of String, String)) As IEnumerable Implements ISupportQueryInfoList.GetInfoList
            Return JEInfoList.GetJEInfoList(pFilters)
        End Function
#End Region

#Region "ILockable"
        Public Function isLocked() As Boolean Implements ILockable.isLocked
            Return _status = STR_Post
        End Function

        Public Function isLockedBy() As String Implements ILockable.isLockedBy
            Return _postedBy
        End Function

        Public Function isLockedsomeWhereInFamily() As Boolean Implements ILockable.isLockedsomeWhereInFamily
            Return False
        End Function

        Public Function LockingMessage() As String Implements ILockable.LockingMessage
            Return String.Format(ResStrConst.CLOSED, "Journal", _jrnalNo)
        End Function

        Public Function LockMe() As Boolean Implements ILockable.LockMe
            Return True
        End Function

        Public Function LockMyFamily() As Boolean Implements ILockable.LockMyFamily
            Return True
        End Function
#End Region

#Region "ISupportScript"

        Private Sub FormValidationCheck()

            Dim msg = String.Empty
            If Not Rules.FormValidationRule.CheckRules(Me, msg) Then
                ExceptionThower.BusinessRuleStop(Nz(msg, String.Format(ResStr("{0}:{1} having in valid data."), Me.GetType.ToString.Translate, Me.ToString)))
            End If

        End Sub

        Private Function Post() As Integer

            If Not Me.IsValid Then
                pbs.Helper.UIServices.ConfirmService.Confirm(ResStr("Journal is not valid yet. {0}{1}"), Environment.NewLine, Me.BrokenRulesMsg)
                Return 0
            End If

            FormValidationCheck()

            If Lines.Count = 0 Then
                If Not pbs.Helper.UIServices.ConfirmService.Confirm(ResStr("You did not input any transaction on this journal. Post an empty journal ?")) Then
                    Return 0
                End If
            End If

            Try
                If IsNew Then Rules.NumberingRules.SetNumber(Me)

                _postedBy = Context.CurrentUserCode
                _postingDate.Date = ToDay()
                _status = STR_Post

                MarkDirty()

                Dim ret = Me.Save()

                PostChildren()

                pbs.Helper.UIServices.AlertService.Alert(ResStr(ResStrConst.JPosted), ret.JrnalNo)

                If GetJrnalTypeInfo.PrintOnPosting Then
                    If pbs.Helper.UIServices.ConfirmService.Confirm(ResStr("Print journal?")) Then
                        PrintJournal(ret)
                    End If
                End If

                Return ret.JrnalNo
            Catch ex As Exception
                TextLogger.Debug(ex.Message)
                Return 0
            End Try

        End Function

        Private Shared Sub PrintJournal(pJournal As JE)
            Try
                Dim xlsFile As FlexCel.XlsAdapter.XlsFile = Nothing
                Dim templateId = pJournal.GetJrnalTypeInfo.ReportDefinition
                Dim ds = DataSetFactory.CoerceDataObjectToDataSet(pJournal)
                FlexelReporter.CreateReportInMemory(xlsFile, ds, templateId)
                pbs.Helper.UIServices.PrintPreviewService.ShowDialog(xlsFile, templateId)
            Catch ex As Exception
                TextLogger.Debug(ex.Message)
            End Try
        End Sub

        Private Sub PostChildren()
            If _status = STR_Post Then

                Dim script = <SqlText>
UPDATE pbs_MC_LEDGER_<%= _DTB %> SET STATUS='<%= STR_Post %>', LOCK_FLAG='Y' WHERE PFD_NO=<%= _jrnalNo %>
                             </SqlText>.Value.Trim

                SQLCommander.RunInsertUpdate(script)

            End If

        End Sub

        Private Function Post_Imp() As UITasks

            Dim scripts = New Script.UITasks(Me)
            scripts.IconName = "Post"
            scripts.CaptionKey = ResStr("Post")
            scripts.RefreshUIWhenFinish = True

            scripts.AddCallFunction(1, "Post")

            scripts.AddLogicSwitch(5, "Input=0", 10, 20)

            scripts.AddStop(10)

            scripts.AddCloseUI(20)

            Return scripts

        End Function

        Private Function Reverse_Imp() As UITasks

            Dim scripts = New Script.UITasks(Me)

            scripts.IconName = "History/Undo"

            scripts.CaptionKey = ResStr("Reverse")

            scripts.ButtonLocation = ScriptButtonLocation.Navigation_Right

            scripts.RefreshUIWhenFinish = True

            scripts.AddAskUserCommand(5, "&$dialog=ReversalPeriod") ' set journal datatable to param slot #1

            scripts.AddLogicSwitch(6, "IsNull(Input)", 7, 10)

            scripts.AddStop(7)

            scripts.AddCallFunction(10, "ReverseJournal(#5)") ' set journal datatable to param slot #1

            scripts.AddCloseUI(20)

            Return scripts

        End Function

        Public Function GetScriptDictionary() As Dictionary(Of String, Script.UITasks) Implements ISupportScripts.GetScriptDictionary
            Dim _scripts = New Dictionary(Of String, UITasks)(StringComparer.OrdinalIgnoreCase)

            _scripts.Add("Post", Post_Imp)

            _scripts.Add("Reverse", Reverse_Imp)

            Return _scripts
        End Function
#End Region

#Region "IsupportCommandAuthorization"
        Friend Const STR_Post = "Posted"

        Public Function CanExecute(cmd As String) As Boolean? Implements ISupportCommandAuthorization.CanExecute
            cmd = Regex.Replace(cmd, "^act_", String.Empty)
            Select Case cmd
                Case pbs.Helper.Action._Post
                    If Not pbs.UsrMan.Permission.isPermited(String.Format("{0}.{1}", GetType(MCLDG).ToString, cmd)) Then
                        Return False
                    Else
                        Return Not _status.Equals(STR_Post)
                    End If

                Case pbs.Helper.Action._Amend, pbs.Helper.Action._Delete
                    If _status = STR_Post Then
                        Return False
                    ElseIf Not pbs.UsrMan.Permission.isPermited(String.Format("{0}.{1}", GetType(MCLDG).ToString, cmd)) Then
                        Return False
                    End If

                Case "Save"
                    If Me.Status = STR_Post Then
                        Return False
                    ElseIf Me.GetJrnalTypeInfo.HardPostingOnly Then
                        Return False
                    End If

                Case Else

                    Return pbs.UsrMan.Permission.isPermited(String.Format("{0}.{1}", GetType(MCLDG).ToString, cmd))

            End Select
        End Function
#End Region

#Region "ISupportDocumentDataSet"

        Function GetTemplateCode() As String Implements Interfaces.ISupportDocumentDataSet.GetTemplateCode
            Dim template = GetJrnalTypeInfo.ReportDefinition
            If String.IsNullOrEmpty(template) Then
                Return RD.GetOneLayout(SimpleInfoLists.ReportLayoutsType.STR_JLSM)
            Else
                Return template
            End If
        End Function

        Public Function GetDataSet() As DataSet Implements Interfaces.ISupportDocumentDataSet.GetDataSet

            Dim master = List2Table.CreateTableFromSingleObject(Me, "SMJE")

            Dim detail = List2Table.Obj2Table(Me.Lines, "MCLDG")

            Dim ds = New DataSet("SMJournal")

            ds.Tables.Add(master)
            ds.Tables.Add(detail)

            detail.Columns.Add(New DataColumn("_SYSTEM_", GetType(String)) With {.DefaultValue = Me.JrnalNo})
            master.Columns.Add(New DataColumn("_SYSTEM_", GetType(String)) With {.DefaultValue = Me.JrnalNo})

            Dim relationship = New System.Data.DataRelation("->JrnalLines", master.Columns("_SYSTEM_"), detail.Columns("_SYSTEM_"), True)
            ds.Relations.Add(relationship)

            Return ds

        End Function
#End Region

#Region "Preset"

        ''' <summary>
        ''' 1. Create detail lines based on the header data url and parameters
        ''' 2. Preset Data may come from pbsCmdArgs.BO
        ''' if preset data is dataset then it take the table with table name = Lines
        ''' </summary>
        ''' <param name="PresetData"></param>
        ''' <remarks></remarks>
        Public Sub ApplyPreset(PresetData As Object, pTransformationDic As Dictionary(Of String, String)) Implements ISupportPreset.ApplyPreset

            Dim dt As DataTable = Nothing

            Dim ds = TryCast(PresetData, DataSet)
            If ds IsNot Nothing Then
                TextLogger.Debug("Applying preset form a dataset ....")
                dt = ds.TryGetTable("Lines")
                If dt IsNot Nothing Then TextLogger.Debug("Found table {0} for applying preset to target list", dt.TableName)
            Else
                dt = TryCast(PresetData, DataTable)
            End If

            If dt IsNot Nothing Then
                'data come from Args.BO
                TextLogger.Debug("Applying preset form a datatable ....")

                Dim theTransformDic = pTransformationDic.ExtractTransformationRulesFromDictionary("Lines")

                Dim datalist As List(Of Dictionary(Of String, String))

                If theTransformDic IsNot Nothing AndAlso theTransformDic.Count > 0 Then
                    datalist = BOFactory.ConvertDataTable2Dictionary(dt, theTransformDic)
                Else
                    datalist = BOFactory.ConvertDataTable2Dictionary(dt)
                End If

                ApplyPreset(datalist)

            Else
                TextLogger.Debug("Applying preset form a VUH profile or report template ....")
                'data come from VUH profile
                Dim datalist = TryCast(PresetData, List(Of Dictionary(Of String, String)))

                If datalist IsNot Nothing Then ApplyPreset(datalist)

            End If

        End Sub

        ''' <summary>
        ''' data table will be apply to SINVLine list
        ''' </summary>
        ''' <param name="pList">list of dictionary item</param>
        ''' <remarks></remarks>
        Private Sub ApplyPreset(pList As List(Of Dictionary(Of String, String)))
            _lines.RaiseListChangedEvents = False
            Dim seqProfiles = pbs.BO.Rules.NumberingRules.GetPresetNumbering(GetType(pbs.BO.MC.MCLDG).ToString)

            For Each itm In pList
                Dim newLine = Lines.AddNew()
                newLine.SuspendCalculation()
                Dim theDic = itm.Merge(seqProfiles, True)
                BOFactory.ApplyPreset(newLine, theDic)
            Next

            PopulateMasterDataToDetails()
            _lines.RaiseListChangedEvents = True
        End Sub


        Public Sub PresetReadonlyProperties(pPreset As Dictionary(Of String, String)) Implements ISupportPreset.PresetReadonlyProperties
            _status = pPreset.GetValueByKey("Status", String.Empty)
        End Sub
#End Region

#Region "Journal Reversal"

        Public Function ReverseJournal(pPeriod As String) As Integer
            If JrnalNo <= 0 OrElse Me.IsNew Then
                ExceptionThower.BusinessRuleStop("Please select an existing journal first")
                Return 0
            ElseIf Me.Lines.Count = 0 Then
                ExceptionThower.BusinessRuleStop("Original journal has no transactions")
                Return 0
            ElseIf Me.HasAllocatedLines Then
                ExceptionThower.BusinessRuleStop(String.Format(ResStr("CANNOTREVERSEALLOCATED"), JrnalNo))
            Else
                If String.IsNullOrEmpty(pPeriod) Then pPeriod = Me.Period

                Dim reversal = Me.CloneAReversal

                reversal.ValidationRules.CheckRules("Period")
                reversal.ValidationRules.CheckRules("TransactionDate")

                reversal._status = STR_Post
                reversal = reversal.Save
                reversal.PostChildren()

                Dim allocRef As Integer = Me.Lines(0).LineNo

                JE.MarkAsPostedCorrection(JrnalNo, reversal.JrnalNo, allocRef)

                ALOG.Log(Me, "Reverse")

                pbs.Helper.UIServices.AlertService.Alert("Journal {0} has been reversed by a new journal {1}", JrnalNo, reversal.JrnalNo)

                Return reversal.JrnalNo
            End If
            Return 0
        End Function

        ''' <summary>
        ''' use script to marked bot journal as Correction
        ''' </summary>
        ''' <param name="original"></param>
        ''' <param name="ReversalNo"></param>
        ''' <remarks></remarks>
        Private Shared Sub MarkAsPostedCorrection(original As Integer, ReversalNo As Integer, AllocRef As Integer)
            If original = 0 OrElse ReversalNo = 0 Then Exit Sub

            Dim det = <detail>UPDATE pbs_MC_LEDGER_<%= Context.CurrentBECode %> SET ALLOCATION='C' , ALLOC_REF=<%= AllocRef %>, ALLOC_DATE=<%= ToDay.ToSunDate %>, STATUS='Reverse', LOCK_FLAG='Y' WHERE PFD_NO=<%= original %> OR  PFD_NO=<%= ReversalNo %></detail>.Value.Trim
            SQLCommander.RunInsertUpdate(det)

            det = <detail>UPDATE pbs_MC_JRNAL_<%= Context.CurrentBECode %> SET DESCRIPTION=substring('REVERSE-' + DESCRIPTION,1,200)  WHERE JRNAL_NO=<%= original %> OR  JRNAL_NO=<%= ReversalNo %></detail>.Value.Trim
            SQLCommander.RunInsertUpdate(det)
        End Sub

        Public Function HasAllocatedLines() As Boolean

            For Each line As MCLDG In Lines
                If Regex.IsMatch(line.Allocation, "[A,P,C]") Then Return True
            Next
            Return False

        End Function
#End Region

    End Class

End Namespace