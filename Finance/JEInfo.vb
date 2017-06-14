
Imports pbs.Helper
Imports pbs.Helper.Interfaces
Imports System.Data
Imports Csla
Imports Csla.Data
Imports Csla.Validation
Imports pbs.BO.SM

Namespace MC

    <Serializable()> _
    Public Class JEInfo
        Inherits Csla.ReadOnlyBase(Of JEInfo)
        Implements IComparable
        Implements IInfo
        Implements IDocLink
        Implements ILockable
        Implements IAutoDetectSubform

        'Implements IInfoStatus

#Region " Business Properties and Methods "

        Private _jrnalNo As Integer
        Public ReadOnly Property JrnalNo() As Integer
            Get
                Return _jrnalNo
            End Get
        End Property

        Private _transType As String = String.Empty
        Public ReadOnly Property TransType() As String
            Get
                Return _transType
            End Get
        End Property

        Private _reference As String = String.Empty
        Public ReadOnly Property Reference() As String
            Get
                Return _reference
            End Get
        End Property

        Private _transDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property TransDate() As String
            Get
                Return _transDate.DateViewFormat
            End Get
        End Property

        Private _period As SmartPeriod = New pbs.Helper.SmartPeriod()
        Public ReadOnly Property Period() As String
            Get
                Return _period.PeriodViewFormat
            End Get
        End Property

        Private _description As String = String.Empty

        Private _candidateId As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        Public ReadOnly Property CandidateId() As String
            Get
                Return _candidateId.Text
            End Get
        End Property

        Private _patientCode As String = String.Empty
        Public ReadOnly Property PatientCode() As String
            Get
                Return _patientCode
            End Get
        End Property

        'Private _schoolYear As String = String.Empty
        'Public ReadOnly Property SchoolYear() As String
        '    Get
        '        Return _schoolYear
        '    End Get
        'End Property

        Private _clinic As String = String.Empty
        Public ReadOnly Property Clinic() As String
            Get
                Return _clinic
            End Get
        End Property

        'Private _transCampus As String = String.Empty
        'Public ReadOnly Property TransCampus() As String
        '    Get
        '        Return _transCampus
        '    End Get
        'End Property

        'Private _program As String = String.Empty
        'Public ReadOnly Property Program() As String
        '    Get
        '        Return _program
        '    End Get
        'End Property

        'Private _classId As String = String.Empty
        'Public ReadOnly Property ClassId() As String
        '    Get
        '        Return _classId
        '    End Get
        'End Property
        Private _convCode As String = String.Empty
        Public ReadOnly Property ConvCode() As String
            Get
                Return _convCode
            End Get
        End Property

        Private _convRate As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property ConvRate() As Decimal
            Get
                Return _convRate.Float
            End Get
        End Property

        Private _paymentRef As String = String.Empty
        Public ReadOnly Property PaymentRef() As String
            Get
                Return _paymentRef
            End Get
        End Property

        Private _payMethod As String = String.Empty
        Public ReadOnly Property PayMethod() As String
            Get
                Return _payMethod
            End Get
        End Property

        Private _paymentDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property PaymentDate() As String
            Get
                Return _paymentDate.DateViewFormat
            End Get
        End Property

        Private _paymentPeriod As SmartPeriod = New pbs.Helper.SmartPeriod()
        Public ReadOnly Property PaymentPeriod() As String
            Get
                Return _paymentPeriod.PeriodViewFormat
            End Get
        End Property

        Private _directInvoice As String = String.Empty
        Public ReadOnly Property DirectInvoice() As String
            Get
                Return _directInvoice
            End Get
        End Property

        Private _invoiceInfo As String = String.Empty
        Public ReadOnly Property InvoiceInfo() As String
            Get
                Return _invoiceInfo
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

        Private _invoiceBook As String = String.Empty
        Public ReadOnly Property InvoiceBook() As String
            Get
                Return _invoiceBook
            End Get
        End Property

        Private _invoiceDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property InvoiceDate() As String
            Get
                Return _invoiceDate.DateViewFormat
            End Get
        End Property

        Private _invoicePeriod As SmartPeriod = New pbs.Helper.SmartPeriod()
        Public ReadOnly Property InvoicePeriod() As String
            Get
                Return _invoicePeriod.PeriodViewFormat
            End Get
        End Property

        Private _vatRate As String = String.Empty
        Public ReadOnly Property VatRate() As String
            Get
                Return _vatRate
            End Get
        End Property

        Private _ncPl0 As String = String.Empty
        Public ReadOnly Property ncPl0() As String
            Get
                Return _ncPl0
            End Get
        End Property

        Private _ncPl1 As String = String.Empty
        Public ReadOnly Property ncPl1() As String
            Get
                Return _ncPl1
            End Get
        End Property

        Private _ncPl2 As String = String.Empty
        Public ReadOnly Property ncPl2() As String
            Get
                Return _ncPl2
            End Get
        End Property

        Private _ncPl3 As String = String.Empty
        Public ReadOnly Property ncPl3() As String
            Get
                Return _ncPl3
            End Get
        End Property

        Private _ncPl4 As String = String.Empty
        Public ReadOnly Property ncPl4() As String
            Get
                Return _ncPl4
            End Get
        End Property

        Private _ncPl5 As String = String.Empty
        Public ReadOnly Property ncPl5() As String
            Get
                Return _ncPl5
            End Get
        End Property

        Private _ncPl6 As String = String.Empty
        Public ReadOnly Property ncPl6() As String
            Get
                Return _ncPl6
            End Get
        End Property

        Private _ncPl7 As String = String.Empty
        Public ReadOnly Property ncPl7() As String
            Get
                Return _ncPl7
            End Get
        End Property

        Private _ncPl8 As String = String.Empty
        Public ReadOnly Property ncPl8() As String
            Get
                Return _ncPl8
            End Get
        End Property

        Private _ncPl9 As String = String.Empty
        Public ReadOnly Property ncPl9() As String
            Get
                Return _ncPl9
            End Get
        End Property

        Private _status As String = String.Empty
        Public ReadOnly Property Status() As String
            Get
                Return _status
            End Get
        End Property

        Private _postingDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property PostingDate() As String
            Get
                Return _postingDate.DateViewFormat
            End Get
        End Property

        Private _postedBy As String = String.Empty
        Public ReadOnly Property PostedBy() As String
            Get
                Return _postedBy
            End Get
        End Property

        Private _extDesc1 As String = String.Empty
        Public ReadOnly Property ExtDesc1() As String
            Get
                Return _extDesc1
            End Get
        End Property

        Private _extDesc2 As String = String.Empty
        Public ReadOnly Property ExtDesc2() As String
            Get
                Return _extDesc2
            End Get
        End Property

        Private _extDesc3 As String = String.Empty
        Public ReadOnly Property ExtDesc3() As String
            Get
                Return _extDesc3
            End Get
        End Property

        Private _extDesc4 As String = String.Empty
        Public ReadOnly Property ExtDesc4() As String
            Get
                Return _extDesc4
            End Get
        End Property

        Private _extDesc5 As String = String.Empty
        Public ReadOnly Property ExtDesc5() As String
            Get
                Return _extDesc5
            End Get
        End Property

        Private _extDate1 As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property ExtDate1() As String
            Get
                Return _extDate1.DateViewFormat
            End Get
        End Property

        Private _extDate2 As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property ExtDate2() As String
            Get
                Return _extDate2.DateViewFormat
            End Get
        End Property

        Private _extDate3 As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property ExtDate3() As String
            Get
                Return _extDate3.DateViewFormat
            End Get
        End Property

        Private _extDate4 As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property ExtDate4() As String
            Get
                Return _extDate4.DateViewFormat
            End Get
        End Property

        Private _extDate5 As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property ExtDate5() As String
            Get
                Return _extDate5.DateViewFormat
            End Get
        End Property

        Private _extVal1 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property ExtVal1() As String
            Get
                Return _extVal1.Text
            End Get
        End Property

        Private _extVal2 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property ExtVal2() As String
            Get
                Return _extVal2.Text
            End Get
        End Property

        Private _extVal3 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property ExtVal3() As String
            Get
                Return _extVal3.Text
            End Get
        End Property

        Private _extVal4 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property ExtVal4() As String
            Get
                Return _extVal4.Text
            End Get
        End Property

        Private _extVal5 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property ExtVal5() As String
            Get
                Return _extVal5.Text
            End Get
        End Property

        Private _entryDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property EntryDate() As String
            Get
                Return _entryDate.Text
            End Get
        End Property

        Private _entryBy As String = String.Empty
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

        Public ReadOnly Property Code As String Implements IInfo.Code
            Get
                Return _jrnalNo
            End Get
        End Property

        Public ReadOnly Property LookUp As String Implements IInfo.LookUp
            Get
                Return _transType
            End Get
        End Property

        Public ReadOnly Property Description As String Implements IInfo.Description
            Get
                Return _description
            End Get
        End Property

        Public Sub InvalidateCache() Implements IInfo.InvalidateCache
            JEInfoList.InvalidateCache()
        End Sub


#End Region 'Business Properties and Methods

#Region "Details"
        Friend _amount As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property Amount As Decimal
            Get
                Return _amount.Float
            End Get
        End Property

        Friend _vatAmount As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property VatAmount() As Decimal
            Get
                Return _vatAmount.Float
            End Get
        End Property

#End Region

#Region " Factory Methods "

        Friend Shared Function GetJEInfo(ByVal dr As SafeDataReader) As JEInfo
            Return New JEInfo(dr)
        End Function

        Friend Shared Function EmptyJEInfo(Optional ByVal pJrnalNo As String = "") As JEInfo
            Dim info As JEInfo = New JEInfo
            With info
                ._jrnalNo = 0

            End With
            Return info
        End Function

        Private Sub New(ByVal dr As SafeDataReader)
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

        Private Sub New()
        End Sub


#End Region ' Factory Methods

#Region "IDoclink"
        Public Function Get_DOL_Reference() As String Implements IDocLink.Get_DOL_Reference
            Return String.Format("{0}#{1}", Get_DOLTransType, _reference)
        End Function

        Public Function Get_DOLTransType() As String Implements IDocLink.Get_TransType
            Return GetType(MCLDG).ToString.Leaf
        End Function
#End Region

#Region "ILockable"
        Public Function isLocked() As Boolean Implements ILockable.isLocked
            Return _status = JE.STR_Post
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

#Region "ISupportDocumentDataSet"
        ''' <summary>
        ''' Form the access URL for this object
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetURLShortCut() As String Implements IAutoDetectSubform.GetURLShortCut
            If Not String.IsNullOrEmpty(_transType) Then
                Return String.Format("{0}/{1}", Me.GetType.ToClassSchemaName, Me._transType)
            Else
                Dim subform = pbs.Helper.UIServices.ValueSelectorService.SelectID(JDInfoList.GetJDInfoList, ResStrConst.SelectItemText("TransType"))
                Return String.Format("{0}/{1}?TransType={1}", Me.GetType.ToClassSchemaName, Nz(subform, String.Empty))
            End If
        End Function
#End Region




    End Class

End Namespace