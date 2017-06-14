
Imports pbs.Helper
Imports pbs.Helper.Interfaces
Imports System.Data
Imports Csla
Imports Csla.Data
Imports Csla.Validation

Namespace MC

    <Serializable()> _
    Public Class MCLDGInfo
        Inherits Csla.ReadOnlyBase(Of MCLDGInfo)
        Implements IComparable
        Implements IInfo
        Implements IDocLink
        Implements ILockable

        'Implements IInfoStatus

#Region "Behavior"

        Public ReadOnly Property PatientName As String
            Get
                If Not String.IsNullOrEmpty(_patientCode) Then
                    Return GetStudendInfo.Description
                Else
                    Return GetCANInfo.Description
                End If
            End Get
        End Property

        <NonSerialized>
        Private _canInfo As HR.CDDInfo = Nothing
        Public Function GetCANInfo() As HR.CDDInfo
            If _canInfo Is Nothing OrElse Not _canInfo.CddCode.Equals(_candidateId.Int) Then
                _canInfo = HR.CDDInfoList.GetCDDInfo(_candidateId.Int)
            End If
            Return _canInfo
        End Function

        <NonSerialized>
        Private _patientInfo As PATIENTInfo = Nothing
        Public Function GetStudendInfo() As PATIENTInfo
            If _patientInfo Is Nothing OrElse Not _patientInfo.PatientCode.Equals(_patientCode) Then
                _patientInfo = PATIENTInfoList.GetPATIENTInfo(_patientCode)
            End If
            Return _patientInfo
        End Function

        <NonSerialized>
        Private _jdInfo As JDInfo = Nothing
        Public Function GetJrnalTypeInfo() As JDInfo
            If _jdInfo Is Nothing OrElse Not _jdInfo.JournalType.Equals(_transType) Then
                _jdInfo = JDInfoList.GetJDInfo(_transType)
            End If
            Return _jdInfo
        End Function

#End Region

#Region " Business Properties and Methods "

        Private _lineNo As Integer
        Public ReadOnly Property LineNo() As Integer
            Get
                Return _lineNo
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

        Private _candidateId As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        'Public ReadOnly Property CandidateId() As String
        '    Get
        '        Return _candidateId.Text
        '    End Get
        'End Property

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

        Private _itemCode As String = String.Empty
        Public ReadOnly Property ItemCode() As String
            Get
                Return _itemCode
            End Get
        End Property

        Private _description As String = String.Empty

        Private _quantity As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property Quantity() As Decimal
            Get
                Return _quantity.Float
            End Get
        End Property

        Private _unitCode As String = String.Empty
        Public ReadOnly Property UnitCode() As String
            Get
                Return _unitCode
            End Get
        End Property

        Private _unitPrice As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property UnitPrice() As Decimal
            Get
                Return _unitPrice.Float
            End Get
        End Property

        Private _transAmt As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property TransAmt() As Decimal
            Get
                Return _transAmt.Float
            End Get
        End Property

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

        Private _amount As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property Amount() As Decimal
            Get
                Return _amount.Float
            End Get
        End Property

        Public ReadOnly Property Debit() As Decimal
            Get
                If _dC.Equals("C", StringComparison.OrdinalIgnoreCase) Then
                    Return 0
                Else
                    Return Amount
                End If
            End Get
        End Property

        Public ReadOnly Property Credit() As Decimal
            Get
                If Not _dC.Equals("C", StringComparison.OrdinalIgnoreCase) Then
                    Return 0
                Else
                    Return Amount
                End If
            End Get
        End Property

        Private _dC As String = String.Empty
        Public ReadOnly Property DC() As String
            Get
                Return _dC
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

        Private _vatAmount As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property VatAmount() As Decimal
            Get
                Return _vatAmount.Float
            End Get
        End Property

        Public ReadOnly Property LineValue() As Decimal
            Get
                Return (_vatAmount.Float + _amount.Float).RoundBA
            End Get
        End Property

        Private _ncPl0 As String = String.Empty
        Public ReadOnly Property NcSl0() As String
            Get
                Return _ncPl0
            End Get
        End Property

        Private _ncPl1 As String = String.Empty
        Public ReadOnly Property NcSl1() As String
            Get
                Return _ncPl1
            End Get
        End Property

        Private _ncPl2 As String = String.Empty
        Public ReadOnly Property NcSl2() As String
            Get
                Return _ncPl2
            End Get
        End Property

        Private _ncPl3 As String = String.Empty
        Public ReadOnly Property NcSl3() As String
            Get
                Return _ncPl3
            End Get
        End Property

        Private _ncPl4 As String = String.Empty
        Public ReadOnly Property NcSl4() As String
            Get
                Return _ncPl4
            End Get
        End Property

        Private _ncPl5 As String = String.Empty
        Public ReadOnly Property NcSl5() As String
            Get
                Return _ncPl5
            End Get
        End Property

        Private _ncPl6 As String = String.Empty
        Public ReadOnly Property NcSl6() As String
            Get
                Return _ncPl6
            End Get
        End Property

        Private _ncPl7 As String = String.Empty
        Public ReadOnly Property NcSl7() As String
            Get
                Return _ncPl7
            End Get
        End Property

        Private _ncPl8 As String = String.Empty
        Public ReadOnly Property NcSl8() As String
            Get
                Return _ncPl8
            End Get
        End Property

        Private _ncPl9 As String = String.Empty
        'Public ReadOnly Property NcSl9() As String
        '    Get
        '        Return _ncPl9
        '    End Get
        'End Property

        Public ReadOnly Property SubscriptionId() As String
            Get
                Return _ncPl9
            End Get
        End Property

        Private _allocation As String = String.Empty
        Public ReadOnly Property Allocation() As String
            Get
                Return _allocation
            End Get
        End Property

        Private _allocRef As Integer
        Public ReadOnly Property AllocRef() As Integer
            Get
                Return _allocRef
            End Get
        End Property

        Private _allocDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property AllocDate() As String
            Get
                Return _allocDate.DateViewFormat
            End Get
        End Property

        Private _allocPeriod As SmartPeriod = New pbs.Helper.SmartPeriod()
        Public ReadOnly Property AllocPeriod() As String
            Get
                Return _allocPeriod.PeriodViewFormat
            End Get
        End Property

        Private _status As String = String.Empty
        Public ReadOnly Property Status() As String
            Get
                Return _status
            End Get
        End Property

        Private _lockFlag As String = String.Empty
        Public ReadOnly Property LockFlag() As String
            Get
                Return _lockFlag
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

        Private _holdOpId As String = String.Empty
        Public ReadOnly Property HoldOpId() As String
            Get
                Return _holdOpId
            End Get
        End Property

        Private _bphNo As Integer
        Public ReadOnly Property BphNo() As Integer
            Get
                Return _bphNo
            End Get
        End Property

        Private _pfdNo As Integer
        Public ReadOnly Property PfdNo() As Integer
            Get
                Return _pfdNo
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

        Public ReadOnly Property Grade() As String
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
        Public ReadOnly Property DueDate() As String
            Get
                If _dC = "C" OrElse _extDate1.IsEmpty Then
                    Return _transDate.DateViewFormat
                Else
                    Return _extDate1.DateViewFormat
                End If
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

        Public ReadOnly Property Code As String Implements IInfo.Code
            Get
                Return _lineNo
            End Get
        End Property

        Public ReadOnly Property LookUp As String Implements IInfo.LookUp
            Get
                Return _reference
            End Get
        End Property

        Public ReadOnly Property Description As String Implements IInfo.Description
            Get
                Return _description
            End Get
        End Property


        Public Sub InvalidateCache() Implements IInfo.InvalidateCache
            MCLDGInfoList.InvalidateCache()
        End Sub


#End Region 'Business Properties and Methods

#Region " Factory Methods "

        Friend Shared Function GetMCLDGInfo(ByVal dr As SafeDataReader) As MCLDGInfo
            Return New MCLDGInfo(dr)
        End Function

        Friend Shared Function EmptyMCLDGInfo(Optional ByVal pLineNo As String = "") As MCLDGInfo
            Dim info As MCLDGInfo = New MCLDGInfo
            With info
                ._lineNo = pLineNo.ToInteger

            End With
            Return info
        End Function

        Private Sub New(ByVal dr As SafeDataReader)
            _lineNo = dr.GetInt32("LINE_NO")
            _transType = dr.GetString("TRANS_TYPE").TrimEnd
            _reference = dr.GetString("REFERENCE").TrimEnd
            _transDate.Text = dr.GetInt32("TRANS_DATE")
            _period.Text = dr.GetInt32("PERIOD")
            _candidateId.Text = dr.GetInt32("CANDIDATE_ID")
            _patientCode = dr.GetString("PATIENT_CODE").TrimEnd
            _clinic = dr.GetString("CLINIC").TrimEnd
            '_transCampus = dr.GetString("TRANS_CAMPUS").TrimEnd
            '_program = dr.GetString("PROGRAM").TrimEnd
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

        Private Sub New()
        End Sub

#End Region ' Factory Methods

#Region "IDoclink"
        Public Function Get_DOL_Reference() As String Implements IDocLink.Get_DOL_Reference
            Return String.Format("{0}#{1}", Get_TransDOLType, _reference)
        End Function

        Public Function Get_TransDOLType() As String Implements IDocLink.Get_TransType
            Return Me.GetType.ToClassSchemaName.Leaf
        End Function
#End Region

#Region "ILockable"
        Public Function isLocked() As Boolean Implements ILockable.isLocked
            Return _lockFlag.Equals("Y", StringComparison.OrdinalIgnoreCase)
        End Function

        Public Function isLockedBy() As String Implements ILockable.isLockedBy
            If _lockFlag.Equals("Y", StringComparison.OrdinalIgnoreCase) Then
                Return _postedBy
            ElseIf _status.Equals("ALLOCATION", StringComparison.OrdinalIgnoreCase) Then
                Return _holdOpId
            End If
            Return String.Empty
        End Function

        Public Function isLockedsomeWhereInFamily() As Boolean Implements ILockable.isLockedsomeWhereInFamily
            If _bphNo <= 0 Then Return True
            Return BatchIsLocked(_bphNo)
        End Function

        Public Function LockingMessage() As String Implements ILockable.LockingMessage
            Return String.Format(ResStr("Ledger transaction has been locked by {0}. Please wait untill he/she finish editing"), _holdOpId)
        End Function

        Public Function LockMe() As Boolean Implements ILockable.LockMe
            'Journal already locked by me
            If _holdOpId = Context.CurrentUserCode AndAlso _status = "ALLOCATION" Then Return True

            'before locking. make sure again this line isnot locked by someone else
            Dim sqlText1 = <Script>
SELECT COUNT(*) FROM  pbs_MC_LEDGER_<%= Context.CurrentBECode %> WHERE [BPH_NO]=<%= _bphNo %> AND [LINE_NO]=<%= _lineNo %> AND [STATUS] = 'ALLOCATION' AND [HOLD_OP_ID] &lt;&gt; '<%= Context.CurrentUserCode %>'
                           </Script>.Value

            Dim lockedBySomeoneElse = SQLCommander.GetScalarInteger(sqlText1) > 0

            If lockedBySomeoneElse Then
                _status = "ALLOCATION"
                _holdOpId = "N/A"
                Return False
            End If

            'Process locking
            _status = "ALLOCATION"
            _holdOpId = Context.CurrentUserCode
            Dim sqlText = <Script>
UPDATE pbs_MC_LEDGER_<%= Context.CurrentBECode %> SET [STATUS] = 'ALLOCATION', HOLD_OP_ID = '<%= _holdOpId %>' WHERE [LINE_NO] = <%= _lineNo %>
                          </Script>.Value

            SQLCommander.RunInsertUpdate(sqlText)
            Return True
        End Function

        Private Shared Function BatchIsLocked(ByVal BatchNumber As Integer) As Boolean
            Dim DBTableName = String.Format("pbs_MC_LEDGER_{0}", Context.CurrentBECode)

            Dim user = Context.CurrentUserCode

            Dim sqlText = <Script>
SELECT COUNT(*) FROM  <%= DBTableName %> WHERE [BPH_NO]=<%= BatchNumber %> AND [STATUS] = 'ALLOCATION' AND [HOLD_OP_ID] &lt;&gt; '<%= user %>'
                          </Script>.Value

            Return SQLCommander.GetScalarInteger(sqlText) > 0
        End Function

        Public Function LockMyFamily() As Boolean Implements ILockable.LockMyFamily
            If _bphNo <= 0 Then Return True

            If BatchIsLocked(_bphNo) Then Return False

            Dim DBTableName = String.Format("pbs_MC_LEDGER_{0}", Context.CurrentBECode)

            Dim holdOpId = Context.CurrentUserCode

            Dim sqlText = <Script>
UPDATE  <%= DBTableName %> SET [STATUS] = 'ALLOCATION',  HOLD_OP_ID = '<%= holdOpId %>' WHERE [BPH_NO] = <%= _bphNo %>
                          </Script>.Value

            SQLCommander.RunInsertUpdate(sqlText)

            Return True
        End Function
#End Region

       
#Region "Usage - First Due First Pay matching"
        Public Property CalculationDate As String
        Public Property PaymentStatus As String
        Public Property PendingAmount As Decimal
        Public Property OverPaid As Decimal

        Friend _collectionDate As New List(Of String)
        Public ReadOnly Property CollectionDate As String
            Get
                If _collectionDate.Count = 0 Then
                    Return String.Empty
                Else
                    Return String.Join(",", _collectionDate.ToArray)
                End If
            End Get
        End Property

        Friend _collectionReference As New List(Of String)
        Public ReadOnly Property CollectionReference As String
            Get
                If _collectionReference.Count = 0 Then
                    Return String.Empty
                Else
                    Return String.Join(",", _collectionReference.ToArray)
                End If
            End Get
        End Property

        Private _fdfpExpression As String = Nothing
        Private _sortingKey As String = Nothing
        ''' <summary>
        ''' Transactions will be sorted with those fields before perform matching
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Friend Function GetSortingKey() As String
            If _sortingKey Is Nothing Then
                If _fdfpExpression Is Nothing Then _fdfpExpression = pbs.BO.MC.Settings.GetSettings.GetFDFPExpression
                Return PhoebusAPI.Evaluate(_fdfpExpression, Me)
            Else
                Return _sortingKey
            End If

            Return String.Empty
        End Function

        Friend Sub SetManualPaymentStatus()
            Select Case _allocation
                Case "C", "c"
                    PaymentStatus = "CANCEL"
                Case "A", "a"
                    PaymentStatus = "PAID"
                Case "P", "p"
                    PaymentStatus = "PAID"
                Case "0" To "9", "R", "W"
                    PaymentStatus = "WAITING"

            End Select

        End Sub

        Public ReadOnly Property SortingKey As String
            Get
                Return GetSortingKey()
            End Get
        End Property
#End Region

#Region "Allocation Matching"
        Private _matchingKey As String = Nothing
        Private _matchingExpression As String = Nothing
        ''' <summary>
        ''' Transactions will be match with those fields as pair
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Friend Function GetMatchingKey() As String
            If _matchingKey Is Nothing Then
                If _matchingExpression Is Nothing Then _matchingExpression = pbs.BO.MC.Settings.GetSettings.GetMatchingExpression
                Return PhoebusAPI.Evaluate(_matchingExpression, Me)
            Else
                Return _matchingKey
            End If
            Return String.Empty
        End Function

        Friend Sub MarkAsAllocated(pMarker As String, ref As String, pAllocationPeriod As String)
            Me._allocRef = ref
            Me._allocDate.Text = "T"
            Me._allocPeriod.Text = Nz(pAllocationPeriod, "T")
            Me._allocation = Nz(pMarker, "Y")
        End Sub

        Friend Function GetPostAllocationScript() As String

            Return <script>UPDATE pbs_MC_LEDGER_<%= Context.CurrentBECode %> SET ALLOCATION='A', ALLOC_REF='<%= _allocRef %>', ALLOC_DATE=<%= _allocDate.DBValueInt %>, ALLOC_PERIOD=<%= _allocPeriod.DBValue %> WHERE LINE_NO=<%= _lineNo %>
                   </script>.Value.Trim

        End Function

        Friend Function GetUnAllocationScript() As String

            Return <script>UPDATE pbs_MC_LEDGER_<%= Context.CurrentBECode %> SET ALLOCATION='', ALLOC_REF='', ALLOC_DATE=0, ALLOC_PERIOD=0 WHERE LINE_NO=<%= _lineNo %>
                   </script>.Value.Trim

        End Function

#End Region
    End Class

End Namespace