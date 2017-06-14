
Imports pbs.Helper
Imports pbs.Helper.Interfaces
Imports System.Data
Imports Csla
Imports Csla.Data
Imports Csla.Validation

Namespace MC

    <Serializable()> _
    Public Class SIDInfo
        Inherits Csla.ReadOnlyBase(Of SIDInfo)
        Implements IComparable
        Implements IInfo
        Implements IDocLink
        'Implements IInfoStatus


#Region " Business Properties and Methods "


        Private _lineNo As String = String.Empty
        Public ReadOnly Property LineNo() As String
            Get
                Return _lineNo
            End Get
        End Property

        Private _documentNo As String = String.Empty
        Public ReadOnly Property DocumentNo() As String
            Get
                Return _documentNo
            End Get
        End Property

        Private _itemCode As String = String.Empty
        Public ReadOnly Property ItemCode() As String
            Get
                Return _itemCode
            End Get
        End Property

        Private _itemGroup As String = String.Empty
        Public ReadOnly Property ItemGroup() As String
            Get
                Return _itemGroup
            End Get
        End Property

        Private _descriptn As String = String.Empty
        Public ReadOnly Property Descriptn() As String
            Get
                Return _descriptn
            End Get
        End Property

        Private _unit As String = String.Empty
        Public ReadOnly Property Unit() As String
            Get
                Return _unit
            End Get
        End Property

        Private _qty As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property Qty() As String
            Get
                Return _qty.Text
            End Get
        End Property

        Private _price As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property Price() As String
            Get
                Return _price.Text
            End Get
        End Property

        Private _net As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property Net() As String
            Get
                Return _net.Text
            End Get
        End Property

        Private _vatRate As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property VatRate() As String
            Get
                Return _vatRate.Text
            End Get
        End Property

        Private _vat As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property Vat() As String
            Get
                Return _vat.Text
            End Get
        End Property

        Private _lineValue As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property LineValue() As String
            Get
                Return _lineValue.Text
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

        Private _baseAmount As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property BaseAmount() As String
            Get
                Return _baseAmount.Text
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

        Private _ncSi0 As String = String.Empty
        Public ReadOnly Property NcSi0() As String
            Get
                Return _ncSi0
            End Get
        End Property

        Private _ncSi1 As String = String.Empty
        Public ReadOnly Property NcSi1() As String
            Get
                Return _ncSi1
            End Get
        End Property

        Private _ncSi2 As String = String.Empty
        Public ReadOnly Property NcSi2() As String
            Get
                Return _ncSi2
            End Get
        End Property

        Private _ncSi3 As String = String.Empty
        Public ReadOnly Property NcSi3() As String
            Get
                Return _ncSi3
            End Get
        End Property

        Private _ncSi4 As String = String.Empty
        Public ReadOnly Property NcSi4() As String
            Get
                Return _ncSi4
            End Get
        End Property

        Private _ncSi5 As String = String.Empty
        Public ReadOnly Property NcSi5() As String
            Get
                Return _ncSi5
            End Get
        End Property

        Private _ncSi6 As String = String.Empty
        Public ReadOnly Property NcSi6() As String
            Get
                Return _ncSi6
            End Get
        End Property

        Private _ncSi7 As String = String.Empty
        Public ReadOnly Property NcSi7() As String
            Get
                Return _ncSi7
            End Get
        End Property

        Private _ncSi8 As String = String.Empty
        Public ReadOnly Property NcSi8() As String
            Get
                Return _ncSi8
            End Get
        End Property

        Private _ncSi9 As String = String.Empty
        Public ReadOnly Property NcSi9() As String
            Get
                Return _ncSi9
            End Get
        End Property

        Private _coveredByInsurance As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property CoveredByInsurance() As String
            Get
                Return _coveredByInsurance.Text
            End Get
        End Property

        Private _discount As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property Discount() As String
            Get
                Return _discount.Text
            End Get
        End Property

        Private _paidByPatient As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property PaidByPatient() As String
            Get
                Return _paidByPatient.Text
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

        Private _percentCover As String = String.Empty
        Public ReadOnly Property PercentCover() As String
            Get
                Return _percentCover
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

        Public ReadOnly Property Code As String Implements IInfo.Code
            Get
                Return _lineNo
            End Get
        End Property

        Public ReadOnly Property LookUp As String Implements IInfo.LookUp
            Get
                Return _documentNo
            End Get
        End Property

        Public ReadOnly Property Description As String Implements IInfo.Description
            Get
                Return _descriptn
            End Get
        End Property


        Public Sub InvalidateCache() Implements IInfo.InvalidateCache
            SIDInfoList.InvalidateCache()
        End Sub


#End Region 'Business Properties and Methods

#Region " Factory Methods "

        Friend Shared Function GetSIDInfo(ByVal dr As SafeDataReader) As SIDInfo
            Return New SIDInfo(dr)
        End Function

        Friend Shared Function EmptySIDInfo(Optional ByVal pLineNo As String = "") As SIDInfo
            Dim info As SIDInfo = New SIDInfo
            With info
                ._lineNo = pLineNo

            End With
            Return info
        End Function

        Private Sub New(ByVal dr As SafeDataReader)
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

        Private Sub New()
        End Sub


#End Region ' Factory Methods

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