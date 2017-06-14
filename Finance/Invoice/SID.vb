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
    <DB(TableName:="pbs_MC_INVOICE_DET_XXX")>
    Public Class SID
        Inherits Csla.BusinessBase(Of SID)
        Implements Interfaces.IGenPartObject
        Implements IComparable
        Implements IDocLink

#Region "Property Changed"
        Protected Overrides Sub OnDeserialized(context As Runtime.Serialization.StreamingContext)
            MyBase.OnDeserialized(context)
            AddHandler Me.PropertyChanged, AddressOf BO_PropertyChanged
        End Sub

        Private Sub BO_PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Handles Me.PropertyChanged

            'Dim SI = CType(Me.Parent, SIDs)._parent
            Select Case e.PropertyName

                Case "ItemCode"

                    For Each itm In pbs.BO.LKUDVInfoList.GetLKUDVInfoList
                        If _itemCode = itm.Code Then
                            _descriptn = itm.Descriptn
                            _price.Float = itm.Value01
                            _itemGroup = itm.Category

                            Dim ir = pbs.BO.PB.IRInfoList.GetIRinfo(_itemCode)
                            _unit = ir.UnitStock
                        End If
                    Next

                    ''get item price
                    'Dim func = New pbs.BO.ReportTags.DVLookup_Imp()

                    'Dim params = New List(Of Object)
                    'params.Add("INV_TYPE")
                    'params.Add(_itemCode)
                    'params.Add(1)
                    'params.Add("T")

                    'Dim price = DNz(func.Evaluate(params.ToArray), 0).ToDecimal
                    '_price.Text = price


                Case "Price", "Qty"
                    '_net.Float = _price.Float * _qty.Float
                    Recalculate(0)
                    CType(Me.Parent, SIDs)._parent.CalculatePayment()
                Case "PercentCover"
                    '_coveredByInsurance.Float = _net.Float * _percentCover.Float
                    '_paidByPatient.Float = _net.Float - (_coveredByInsurance.Float + _discount.Float)
                    'If PaidByPatient < 0 Then
                    '    _paidByPatient.Text = 0
                    'End If
                    Recalculate(1)
                    CType(Me.Parent, SIDs)._parent.CalculatePayment()
                Case "CoveredByInsurance", "Discount"
                    '_paidByPatient.Float = _net.Float - (_coveredByInsurance.Float + _discount.Float)
                    'If PaidByPatient < 0 Then
                    '    _paidByPatient.Text = 0
                    'End If
                    Recalculate(2)
                    CType(Me.Parent, SIDs)._parent.CalculatePayment()
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

        Friend Function Recalculate(calStep As Integer) As SmartFloat
            Dim invType = CType(Me.Parent, SIDs)._parent.InvoiceType

            Select Case calStep
                Case 0
                    'Calculate Net when price and quantity changed
                    _net.Float = _price.Float * _qty.Float
                    _coveredByInsurance.Float = _net.Float * _percentCover.Float
                    _paidByPatient.Float = _net.Float - (_coveredByInsurance.Float + _discount.Float)
                    _coveredByInsurance.Float = _net.Float * _percentCover.Float
                    _paidByPatient.Float = _net.Float - (_coveredByInsurance.Float + _discount.Float)
                    'If _paidByPatient < 0 AndAlso invType = "D" Then
                    '    _paidByPatient.Text = 0
                    'End If
                Case 1
                    'Calculate CoveredByInsurance and PaidByPatient
                    _coveredByInsurance.Float = _net.Float * _percentCover.Float
                    _paidByPatient.Float = _net.Float - (_coveredByInsurance.Float + _discount.Float)
                    'If _paidByPatient < 0 AndAlso invType = "D" Then
                    '    _paidByPatient.Text = 0
                    'End If

                Case 2
                    'recalculate PaidByPatient when Discount is changed
                    _paidByPatient.Float = _net.Float - (_coveredByInsurance.Float + _discount.Float)
                    'If _paidByPatient < 0 AndAlso invType = "D" Then
                    '    _paidByPatient.Text = 0
                    'End If

            End Select
        End Function

        ''Calculate total payment of patient
        'Friend Sub CalculatePayment()
        '    Dim docNo = CType(Me.Parent, SIDs)._parent.DocumentNo
        '    For Each itm In SIDInfoList.GetSIDInfoList()
        '        If docNo = itm.DocumentNo Then
        '            CType(Me.p_totalCost.Int = _totalCost.Int + _totalCost.Int
        '            '_totalCoveredByInsurance.Int = 1
        '            '_totalDiscount.Int = 1
        '            '_totalPaidByPatient.Int = 1
        '            '_totalReceived.Int = 1
        '            '_totalReturn.Int = _totalReceived.Int - (_totalCost.Int - _totalCoveredByInsurance.Int - _totalDiscount.Int)
        '    Next
        'End Sub

#End Region

#Region " Business Properties and Methods "
        Friend _DTB As String = String.Empty


        Friend _lineNo As String = String.Empty
        <System.ComponentModel.DataObjectField(True, True)> _
        Public ReadOnly Property LineNo() As String
            Get
                Return _lineNo
            End Get
        End Property

        Private _documentNo As String = String.Empty
        <CellInfo(GroupName:="Details", Tips:="Enter document number")>
        Public Property DocumentNo() As String
            Get
                Return _documentNo
            End Get
            Set(ByVal value As String)
                CanWriteProperty("DocumentNo", True)
                If value Is Nothing Then value = String.Empty
                If Not _documentNo.Equals(value) Then
                    _documentNo = value
                    PropertyHasChanged("DocumentNo")
                End If
            End Set
        End Property

        Private _itemCode As String = String.Empty
        <CellInfo("pbs.BO.PB.IR", GroupName:="Details", Tips:="Enter item code")>
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

        Private _itemGroup As String = String.Empty
        <CellInfo("pbs.BO.PB.IR", GroupName:="Details", Tips:="Enter item code")>
        Public Property ItemGroup() As String
            Get
                Return _itemGroup
            End Get
            Set(ByVal value As String)
                CanWriteProperty("ItemGroup", True)
                If value Is Nothing Then value = String.Empty
                If Not _itemGroup.Equals(value) Then
                    _itemGroup = value
                    PropertyHasChanged("ItemGroup")
                End If
            End Set
        End Property

        Private _descriptn As String = String.Empty
        <CellInfo(GroupName:="Details", Tips:="Enter description")>
        Public Property Descriptn() As String
            Get
                Return _descriptn
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Descriptn", True)
                If value Is Nothing Then value = String.Empty
                If Not _descriptn.Equals(value) Then
                    _descriptn = value
                    PropertyHasChanged("Descriptn")
                End If
            End Set
        End Property

        Private _unit As String = String.Empty
        <CellInfo("UNIT_CODE", GroupName:="Details", Tips:="Enter item unit measure")>
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

        Friend _qty As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="Details", Tips:="Enter item's quantity")>
        Public Property Qty() As String
            Get
                Return _qty.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Qty", True)
                If value Is Nothing Then value = String.Empty
                If Not _qty.Equals(value) Then
                    _qty.Text = value
                    PropertyHasChanged("Qty")
                End If
            End Set
        End Property

        Private _price As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="Details", Tips:="Enter item's price")>
        Public Property Price() As String
            Get
                Return _price.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Price", True)
                If value Is Nothing Then value = String.Empty
                If Not _price.Equals(value) Then
                    _price.Text = value
                    PropertyHasChanged("Price")
                End If
            End Set
        End Property

        Friend _net As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="Details", Tips:="")>
        Public Property Net() As String
            Get
                Return _net.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Net", True)
                If value Is Nothing Then value = String.Empty
                If Not _net.Equals(value) Then
                    _net.Text = value
                    PropertyHasChanged("Net")
                End If
            End Set
        End Property

        Private _vatRate As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="Details", Tips:="Enter vat rate")>
        Public Property VatRate() As String
            Get
                Return _vatRate.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("VatRate", True)
                If value Is Nothing Then value = String.Empty
                If Not _vatRate.Equals(value) Then
                    _vatRate.Text = value
                    PropertyHasChanged("VatRate")
                End If
            End Set
        End Property

        Private _vat As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="Details", Tips:="")>
        Public Property Vat() As String
            Get
                Return _vat.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Vat", True)
                If value Is Nothing Then value = String.Empty
                If Not _vat.Equals(value) Then
                    _vat.Text = value
                    PropertyHasChanged("Vat")
                End If
            End Set
        End Property

        Private _lineValue As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="Details", Tips:="")>
        Public Property LineValue() As String
            Get
                Return _lineValue.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("LineValue", True)
                If value Is Nothing Then value = String.Empty
                If Not _lineValue.Equals(value) Then
                    _lineValue.Text = value
                    PropertyHasChanged("LineValue")
                End If
            End Set
        End Property

        Private _convCode As String = String.Empty
        <CellInfo(GroupName:="Details", Tips:="Enter conversion code")>
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
        <CellInfo(GroupName:="Details", Tips:="Enter conversion rate")>
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

        Private _baseAmount As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="Details", Tips:="Enter base amount")>
        Public Property BaseAmount() As String
            Get
                Return _baseAmount.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("BaseAmount", True)
                If value Is Nothing Then value = String.Empty
                If Not _baseAmount.Equals(value) Then
                    _baseAmount.Text = value
                    PropertyHasChanged("BaseAmount")
                End If
            End Set
        End Property

        Private _extDesc1 As String = String.Empty
        <CellInfo(GroupName:="Ext", Tips:="")>
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
        <CellInfo(GroupName:="Ext", Tips:="")>
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
        <CellInfo(GroupName:="Ext", Tips:="")>
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

        Private _ncSi0 As String = String.Empty
        <CellInfo(GroupName:="ncSi", Tips:="")>
        Public Property NcSi0() As String
            Get
                Return _ncSi0
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcSi0", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncSi0.Equals(value) Then
                    _ncSi0 = value
                    PropertyHasChanged("NcSi0")
                End If
            End Set
        End Property

        Private _ncSi1 As String = String.Empty
        <CellInfo(GroupName:="ncSi", Tips:="")>
        Public Property NcSi1() As String
            Get
                Return _ncSi1
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcSi1", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncSi1.Equals(value) Then
                    _ncSi1 = value
                    PropertyHasChanged("NcSi1")
                End If
            End Set
        End Property

        Private _ncSi2 As String = String.Empty
        <CellInfo(GroupName:="ncSi", Tips:="")>
        Public Property NcSi2() As String
            Get
                Return _ncSi2
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcSi2", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncSi2.Equals(value) Then
                    _ncSi2 = value
                    PropertyHasChanged("NcSi2")
                End If
            End Set
        End Property

        Private _ncSi3 As String = String.Empty
        <CellInfo(GroupName:="ncSi", Tips:="")>
        Public Property NcSi3() As String
            Get
                Return _ncSi3
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcSi3", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncSi3.Equals(value) Then
                    _ncSi3 = value
                    PropertyHasChanged("NcSi3")
                End If
            End Set
        End Property

        Private _ncSi4 As String = String.Empty
        <CellInfo(GroupName:="ncSi", Tips:="")>
        Public Property NcSi4() As String
            Get
                Return _ncSi4
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcSi4", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncSi4.Equals(value) Then
                    _ncSi4 = value
                    PropertyHasChanged("NcSi4")
                End If
            End Set
        End Property

        Private _ncSi5 As String = String.Empty
        <CellInfo(GroupName:="ncSi", Tips:="")>
        Public Property NcSi5() As String
            Get
                Return _ncSi5
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcSi5", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncSi5.Equals(value) Then
                    _ncSi5 = value
                    PropertyHasChanged("NcSi5")
                End If
            End Set
        End Property

        Private _ncSi6 As String = String.Empty
        <CellInfo(GroupName:="ncSi", Tips:="")>
        Public Property NcSi6() As String
            Get
                Return _ncSi6
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcSi6", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncSi6.Equals(value) Then
                    _ncSi6 = value
                    PropertyHasChanged("NcSi6")
                End If
            End Set
        End Property

        Private _ncSi7 As String = String.Empty
        <CellInfo(GroupName:="ncSi", Tips:="")>
        Public Property NcSi7() As String
            Get
                Return _ncSi7
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcSi7", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncSi7.Equals(value) Then
                    _ncSi7 = value
                    PropertyHasChanged("NcSi7")
                End If
            End Set
        End Property

        Private _ncSi8 As String = String.Empty
        <CellInfo(GroupName:="ncSi", Tips:="")>
        Public Property NcSi8() As String
            Get
                Return _ncSi8
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcSi8", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncSi8.Equals(value) Then
                    _ncSi8 = value
                    PropertyHasChanged("NcSi8")
                End If
            End Set
        End Property

        Private _ncSi9 As String = String.Empty
        <CellInfo(GroupName:="ncSi", Tips:="")>
        Public Property NcSi9() As String
            Get
                Return _ncSi9
            End Get
            Set(ByVal value As String)
                CanWriteProperty("NcSi9", True)
                If value Is Nothing Then value = String.Empty
                If Not _ncSi9.Equals(value) Then
                    _ncSi9 = value
                    PropertyHasChanged("NcSi9")
                End If
            End Set
        End Property

        Friend _coveredByInsurance As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="Details", Tips:="Enter amount of money covered by Health insurance")>
        Public Property CoveredByInsurance() As String
            Get
                Return _coveredByInsurance.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("CoveredByInsurance", True)
                If value Is Nothing Then value = String.Empty
                If Not _coveredByInsurance.Equals(value) Then
                    _coveredByInsurance.Text = value
                    PropertyHasChanged("CoveredByInsurance")
                End If
            End Set
        End Property

        Friend _discount As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="Details", Tips:="Enter discounted amount")>
        Public Property Discount() As String
            Get
                Return _discount.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("Discount", True)
                If value Is Nothing Then value = String.Empty
                If Not _discount.Equals(value) Then
                    _discount.Text = value
                    PropertyHasChanged("Discount")
                End If
            End Set
        End Property

        Friend _paidByPatient As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="Details", Tips:="Paid by patient")>
        Public Property PaidByPatient() As String
            Get
                Return _paidByPatient.Text
            End Get
            Set(ByVal value As String)
                CanWriteProperty("PaidByPatient", True)
                If value Is Nothing Then value = String.Empty
                If Not _paidByPatient.Equals(value) Then
                    _paidByPatient.Text = value
                    PropertyHasChanged("PaidByPatient")
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

        Friend _percentCover As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        <CellInfo(GroupName:="Details")>
        Public ReadOnly Property PercentCover() As String
            Get
                Return _percentCover.Text
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

        Sub CheckRules()
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

            For Each _field As ClassField In ClassSchema(Of SID)._fieldList
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
        End Sub

        Public Shared Function BlankSID() As SID
            Return New SID
        End Function

        Public Shared Function NewSID(ByVal pLineNo As String) As SID
            'If KeyDuplicated(pLineNo) Then ExceptionThower.BusinessRuleStop(String.Format(ResStr(ResStrConst.NOACCESS), ResStr("SID")))
            Return DataPortal.Create(Of SID)(New Criteria(pLineNo))
        End Function

        Public Shared Function NewBO(ByVal ID As String) As SID
            Dim pLineNo As String = ID.Trim

            Return NewSID(pLineNo)
        End Function

        Public Shared Function GetSID(ByVal pLineNo As String) As SID
            Return DataPortal.Fetch(Of SID)(New Criteria(pLineNo))
        End Function

        Public Shared Function GetBO(ByVal ID As String) As SID
            Dim pLineNo As String = ID.Trim

            Return GetSID(pLineNo)
        End Function

        Public Shared Sub DeleteSID(ByVal pLineNo As String)
            DataPortal.Delete(New Criteria(pLineNo))
        End Sub

        Public Overrides Function Save() As SID
            If Not IsDirty Then ExceptionThower.NotDirty(ResStr(ResStrConst.NOTDIRTY))
            If Not IsSavable Then Throw New Csla.Validation.ValidationException(String.Format(ResStr(ResStrConst.INVALID), ResStr("SID")))

            Me.ApplyEdit()
            SIDInfoList.InvalidateCache()
            Return MyBase.Save()
        End Function

        Public Function CloneSID(ByVal pLineNo As String) As SID

            'If SID.KeyDuplicated(pLineNo) Then ExceptionThower.BusinessRuleStop(ResStr(ResStrConst.CreateAlreadyExists), Me.GetType.ToString.Leaf.Translate)

            Dim cloningSID As SID = MyBase.Clone
            cloningSID._lineNo = pLineNo

            'Todo:Remember to reset status of the new object here 
            cloningSID.MarkNew()
            cloningSID.ApplyEdit()

            cloningSID.ValidationRules.CheckRules()

            Return cloningSID
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

                    cm.CommandText = <SqlText>SELECT * FROM pbs_MC_INVOICE_DET_<%= _DTB %> WHERE LINE_NO= '<%= criteria._lineNo %>' </SqlText>.Value.Trim

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
            _documentNo = dr.GetString("DOCUMENT_NO").TrimEnd
            _itemCode = dr.GetString("ITEM_CODE").TrimEnd
            _itemGroup = dr.GetString("ITEM_GROUP").TrimEnd
            _descriptn = dr.GetString("DESCRIPTN").TrimEnd
            _unit = dr.GetString("UNIT").TrimEnd
            _qty.Text = dr.GetDecimal("QTY")
            _price.Text = dr.GetDecimal("PRICE")
            _net.Text = dr.GetDecimal("NET")
            _vatRate.Text = dr.GetDecimal("VAT_RATE")
            _vat.Text = dr.GetDecimal("VAT")
            _lineValue.Text = dr.GetDecimal("LINE_VALUE")
            _convCode = dr.GetString("CONV_CODE").TrimEnd
            _convRate.Text = dr.GetDecimal("CONV_RATE")
            _baseAmount.Text = dr.GetDecimal("BASE_AMOUNT")
            _extDesc1 = dr.GetString("EXT_DESC1").TrimEnd
            _extDesc2 = dr.GetString("EXT_DESC2").TrimEnd
            _extDesc3 = dr.GetString("EXT_DESC3").TrimEnd
            _ncSi0 = dr.GetString("NC_SI0").TrimEnd
            _ncSi1 = dr.GetString("NC_SI1").TrimEnd
            _ncSi2 = dr.GetString("NC_SI2").TrimEnd
            _ncSi3 = dr.GetString("NC_SI3").TrimEnd
            _ncSi4 = dr.GetString("NC_SI4").TrimEnd
            _ncSi5 = dr.GetString("NC_SI5").TrimEnd
            _ncSi6 = dr.GetString("NC_SI6").TrimEnd
            _ncSi7 = dr.GetString("NC_SI7").TrimEnd
            _ncSi8 = dr.GetString("NC_SI8").TrimEnd
            _ncSi9 = dr.GetString("NC_SI9").TrimEnd
            _coveredByInsurance.Text = dr.GetDecimal("COVERED_BY_INSURANCE")
            _discount.Text = dr.GetDecimal("DISCOUNT")
            _paidByPatient.Text = dr.GetDecimal("PAID_BY_PATIENT")
            _updated.Text = dr.GetInt32("UPDATED")
            _updatedBy = dr.GetString("UPDATED_BY").TrimEnd

        End Sub

        Private Shared _lockObj As New Object
        Protected Overrides Sub DataPortal_Insert()
            SyncLock _lockObj
                Using ctx = ConnectionFactory.GetDBConnection(True)


                    Insert(ctx)

                End Using
            End SyncLock
        End Sub

        Private Sub AddInsertParameters(ByVal cm As IDBCommand)

            cm.Parameters.AddWithValue("@DOCUMENT_NO", _documentNo.Trim)
            cm.Parameters.AddWithValue("@ITEM_CODE", _itemCode.Trim)
            cm.Parameters.AddWithValue("@ITEM_GROUP", _itemGroup.Trim)
            cm.Parameters.AddWithValue("@DESCRIPTN", _descriptn.Trim)
            cm.Parameters.AddWithValue("@UNIT", _unit.Trim)
            cm.Parameters.AddWithValue("@QTY", _qty.DBValue)
            cm.Parameters.AddWithValue("@PRICE", _price.DBValue)
            cm.Parameters.AddWithValue("@NET", _net.DBValue)
            cm.Parameters.AddWithValue("@VAT_RATE", _vatRate.DBValue)
            cm.Parameters.AddWithValue("@VAT", _vat.DBValue)
            cm.Parameters.AddWithValue("@LINE_VALUE", _lineValue.DBValue)
            cm.Parameters.AddWithValue("@CONV_CODE", _convCode.Trim)
            cm.Parameters.AddWithValue("@CONV_RATE", _convRate.DBValue)
            cm.Parameters.AddWithValue("@BASE_AMOUNT", _baseAmount.DBValue)
            cm.Parameters.AddWithValue("@EXT_DESC1", _extDesc1.Trim)
            cm.Parameters.AddWithValue("@EXT_DESC2", _extDesc2.Trim)
            cm.Parameters.AddWithValue("@EXT_DESC3", _extDesc3.Trim)
            cm.Parameters.AddWithValue("@NC_SI0", _ncSi0.Trim)
            cm.Parameters.AddWithValue("@NC_SI1", _ncSi1.Trim)
            cm.Parameters.AddWithValue("@NC_SI2", _ncSi2.Trim)
            cm.Parameters.AddWithValue("@NC_SI3", _ncSi3.Trim)
            cm.Parameters.AddWithValue("@NC_SI4", _ncSi4.Trim)
            cm.Parameters.AddWithValue("@NC_SI5", _ncSi5.Trim)
            cm.Parameters.AddWithValue("@NC_SI6", _ncSi6.Trim)
            cm.Parameters.AddWithValue("@NC_SI7", _ncSi7.Trim)
            cm.Parameters.AddWithValue("@NC_SI8", _ncSi8.Trim)
            cm.Parameters.AddWithValue("@NC_SI9", _ncSi9.Trim)
            cm.Parameters.AddWithValue("@COVERED_BY_INSURANCE", _coveredByInsurance.DBValue)
            cm.Parameters.AddWithValue("@DISCOUNT", _discount.DBValue)
            cm.Parameters.AddWithValue("@PAID_BY_PATIENT", _paidByPatient.DBValue)
            cm.Parameters.AddWithValue("@UPDATED", _updated.DBValue)
            cm.Parameters.AddWithValue("@UPDATED_BY", _updatedBy.Trim)
        End Sub


        Protected Overrides Sub DataPortal_Update()
            SyncLock _lockObj
                Using ctx = ConnectionFactory.GetDBConnection(True)
                    Update(ctx)

                    End Using
            End SyncLock
        End Sub

        Protected Overrides Sub DataPortal_DeleteSelf()
            DataPortal_Delete(New Criteria(_lineNo))
        End Sub

        Private Overloads Sub DataPortal_Delete(ByVal criteria As Criteria)
           Using ctx = ConnectionFactory.GetDBConnection(True)
                Using cm = ctx.CreateSQLCommand

                    cm.CommandText = <SqlText>DELETE FROM pbs_MC_INVOICE_DET_<%= _DTB %> WHERE LINE_NO= '<%= criteria._lineNo %>' </SqlText>.Value.Trim
                    cm.ExecuteNonQuery()

                End Using
            End Using

        End Sub

        Protected Overrides Sub DataPortal_OnDataPortalInvokeComplete(ByVal e As Csla.DataPortalEventArgs)
            If Csla.ApplicationContext.ExecutionLocation = ExecutionLocations.Server Then
                SIDInfoList.InvalidateCache()
            End If
        End Sub


#End Region 'Data Access                           

#Region " Exists "
        Public Shared Function Exists(ByVal pLineNo As String) As Boolean
            Return SIDInfoList.ContainsCode(pLineNo)
        End Function

        'Public Shared Function KeyDuplicated(ByVal pLineNo As String) As Boolean
        '    Dim SqlText = <SqlText>SELECT COUNT(*) FROM pbs_MC_INVOICE_DET_<%= Context.CurrentBECode %> WHERE LINE_NO= '<%= pLineNo %>'</SqlText>.Value.Trim
        '    Return SQLCommander.GetScalarInteger(SqlText) > 0
        'End Function
#End Region

#Region " IGenpart "

        Public Function CloneBO(ByVal id As String) As Object Implements Interfaces.IGenPartObject.CloneBO
            Return CloneSID(id)
        End Function

        Public Function getBO1(ByVal id As String) As Object Implements Interfaces.IGenPartObject.GetBO
            Return GetBO(id)
        End Function

        Public Function myCommands() As String() Implements Interfaces.IGenPartObject.myCommands
            Return pbs.Helper.Action.StandardReferenceCommands
        End Function

        Public Function myFullName() As String Implements Interfaces.IGenPartObject.myFullName
            Return GetType(SID).ToString
        End Function

        Public Function myName() As String Implements Interfaces.IGenPartObject.myName
            Return GetType(SID).ToString.Leaf
        End Function

        Public Function myQueryList() As IList Implements Interfaces.IGenPartObject.myQueryList
            Return SIDInfoList.GetSIDInfoList
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

#Region "Child"

        Sub MarkAsNewClone()
            MarkNew()
        End Sub

        Shared Function GetChildSID(dr As SafeDataReader)
            Dim child = New SID
            child.FetchObject(dr)
            child.MarkAsChild()
            child.MarkOld()
            Return child
        End Function

        Sub DeleteSelf(cn As IDbConnection)
            Using cm = cn.CreateSQLCommand
                cm.CommandType = CommandType.Text
                cm.CommandText = <sqltext>DELETE FROM pbs_MC_INVOICE_DET_<%= _DTB %> WHERE LINE_NO = <%= _lineNo %></sqltext>
                cm.ExecuteNonQuery()
            End Using
        End Sub

        Sub Insert(cn As IDbConnection)
            Using cm = cn.CreateSQLCommand()

                cm.CommandType = CommandType.StoredProcedure
                cm.CommandText = String.Format("pbs_MC_INVOICE_DET_{0}_Insert", _DTB)

                cm.Parameters.AddWithValue("@LINE_NO", _lineNo.Trim.ToInteger, ParameterDirection.Output)
                AddInsertParameters(cm)
                cm.ExecuteNonQuery()

                _lineNo = cm.GetReturnIdAsInteger("@LINE_NO")

            End Using
        End Sub

        Sub Update(cn As IDbConnection)

            Using cm = cn.CreateSQLCommand()

                cm.CommandType = CommandType.StoredProcedure
                cm.CommandText = String.Format("pbs_MC_INVOICE_DET_{0}_Update", _DTB)

                cm.Parameters.AddWithValue("@LINE_NO", _lineNo.Trim)
                AddInsertParameters(cm)
                cm.ExecuteNonQuery()

            End Using

        End Sub

#End Region
    End Class

End Namespace