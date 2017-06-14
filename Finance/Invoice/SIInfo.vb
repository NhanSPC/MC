
Imports pbs.Helper
Imports pbs.Helper.Interfaces
Imports System.Data
Imports Csla
Imports Csla.Data
Imports Csla.Validation

Namespace MC

    <Serializable()> _
    Public Class SIInfo
        Inherits Csla.ReadOnlyBase(Of SIInfo)
        Implements IComparable
        Implements IInfo
        Implements IDocLink
        'Implements IInfoStatus


#Region " Business Properties and Methods "


        Private _documentNo As String = String.Empty
        Public ReadOnly Property DocumentNo() As String
            Get
                Return _documentNo
            End Get
        End Property

        Private _documentDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property DocumentDate() As String
            Get
                Return _documentDate.Text
            End Get
        End Property

        Private _checkinNo As pbs.Helper.SmartInt32 = New SmartInt32()
        Public ReadOnly Property CheckinNo() As String
            Get
                Return _checkinNo.Text
            End Get
        End Property

        Private _patientCode As String = String.Empty
        Public ReadOnly Property PatientCode() As String
            Get
                Return _patientCode
            End Get
        End Property

        Private _invoicePrefix As String = String.Empty
        Public ReadOnly Property InvoicePrefix() As String
            Get
                Return _invoicePrefix
            End Get
        End Property

        Private _invoiceNo As String = String.Empty
        Public ReadOnly Property InvoiceNo() As String
            Get
                Return _invoiceNo
            End Get
        End Property

        Private _invoiceSerial As String = String.Empty
        Public ReadOnly Property InvoiceSerial() As String
            Get
                Return _invoiceSerial
            End Get
        End Property

        Private _invoiceDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property InvoiceDate() As String
            Get
                Return _invoiceDate.Text
            End Get
        End Property

        Private _invoicePeriod As SmartPeriod = New pbs.Helper.SmartPeriod()
        Public ReadOnly Property InvoicePeriod() As String
            Get
                Return _invoicePeriod.Text
            End Get
        End Property

        Private _invoiceType As String = String.Empty
        Public ReadOnly Property InvoiceType() As String
            Get
                Return _invoiceType
            End Get
        End Property

        Private _clientId As String = String.Empty
        Public ReadOnly Property ClientId() As String
            Get
                Return _clientId
            End Get
        End Property

        Private _purchName As String = String.Empty
        Public ReadOnly Property PurchName() As String
            Get
                Return _purchName
            End Get
        End Property

        Private _address As String = String.Empty
        Public ReadOnly Property Address() As String
            Get
                Return _address
            End Get
        End Property

        Private _bankCode As String = String.Empty
        Public ReadOnly Property BankCode() As String
            Get
                Return _bankCode
            End Get
        End Property

        Private _bankDetail As String = String.Empty
        Public ReadOnly Property BankDetail() As String
            Get
                Return _bankDetail
            End Get
        End Property

        Private _payMethod As String = String.Empty
        Public ReadOnly Property PayMethod() As String
            Get
                Return _payMethod
            End Get
        End Property

        Private _taxCode As String = String.Empty
        Public ReadOnly Property TaxCode() As String
            Get
                Return _taxCode
            End Get
        End Property

        Private _salesman As String = String.Empty
        Public ReadOnly Property Salesman() As String
            Get
                Return _salesman
            End Get
        End Property

        Private _contractId As String = String.Empty
        Public ReadOnly Property ContractId() As String
            Get
                Return _contractId
            End Get
        End Property

        Private _convCode As String = String.Empty
        Public ReadOnly Property ConvCode() As String
            Get
                Return _convCode
            End Get
        End Property

        Private _convRate As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property ConvRate() As String
            Get
                Return _convRate.Text
            End Get
        End Property

        Private _notes As String = String.Empty
        Public ReadOnly Property Notes() As String
            Get
                Return _notes
            End Get
        End Property

        Private _status As String = String.Empty
        Public ReadOnly Property Status() As String
            Get
                Return _status
            End Get
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
            Return _documentNo
        End Function

        'IComparable
        Public Function CompareTo(ByVal IDObject) As Integer Implements System.IComparable.CompareTo
            Dim ID = IDObject.ToString
            Dim pDocumentNo As String = id.Trim
            If _documentNo.Trim < pDocumentNo Then Return -1
            If _documentNo.Trim > pDocumentNo Then Return 1
            Return 0
        End Function

        Public ReadOnly Property Code As String Implements IInfo.Code
            Get
                Return _documentNo
            End Get
        End Property

        Public ReadOnly Property LookUp As String Implements IInfo.LookUp
            Get
                Return _documentNo
            End Get
        End Property

        Public ReadOnly Property Description As String Implements IInfo.Description
            Get
                Return _notes
            End Get
        End Property


        Public Sub InvalidateCache() Implements IInfo.InvalidateCache
            SIInfoList.InvalidateCache()
        End Sub


#End Region 'Business Properties and Methods

#Region " Factory Methods "

        Friend Shared Function GetSIInfo(ByVal dr As SafeDataReader) As SIInfo
            Return New SIInfo(dr)
        End Function

        Friend Shared Function EmptySIInfo(Optional ByVal pDocumentNo As String = "") As SIInfo
            Dim info As SIInfo = New SIInfo
            With info
                ._documentNo = pDocumentNo

            End With
            Return info
        End Function

        Private Sub New(ByVal dr As SafeDataReader)
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

        End Sub

        Private Sub New()
        End Sub


#End Region ' Factory Methods

#Region "IDoclink"
        Public Function Get_DOL_Reference() As String Implements IDocLink.Get_DOL_Reference
            Return String.Format("{0}#{1}", Get_TransType, _documentNo)
        End Function

        Public Function Get_TransType() As String Implements IDocLink.Get_TransType
            Return Me.GetType.ToClassSchemaName.Leaf
        End Function
#End Region

    End Class

End Namespace