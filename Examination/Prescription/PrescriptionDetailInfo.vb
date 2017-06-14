
Imports pbs.Helper
Imports pbs.Helper.Interfaces
Imports System.Data
Imports Csla
Imports Csla.Data
Imports Csla.Validation

Namespace MC

    <Serializable()>
    Public Class PrescriptionDetailInfo
        Inherits Csla.ReadOnlyBase(Of PrescriptionDetailInfo)
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

        Private _prescriptionNo As pbs.Helper.SmartInt32 = New SmartInt32(0)
        Public ReadOnly Property PrescriptionNo() As String
            Get
                Return _prescriptionNo.Text
            End Get
        End Property

        Private _itemCode As String = String.Empty
        Public ReadOnly Property ItemCode() As String
            Get
                Return _itemCode
            End Get
        End Property

        Private _itemName As String = String.Empty
        Public ReadOnly Property ItemName() As String
            Get
                Return _itemName
            End Get
        End Property

        Private _activeIngrendient As String = String.Empty
        Public ReadOnly Property ActiveIngrendient() As String
            Get
                Return _activeIngrendient
            End Get
        End Property

        Private _unit As String = String.Empty
        Public ReadOnly Property Unit() As String
            Get
                Return _unit
            End Get
        End Property

        Private _dateOfIssue As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        Public ReadOnly Property DateOfIssue() As String
            Get
                Return _dateOfIssue.Text
            End Get
        End Property

        Private _morning As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        Public ReadOnly Property Morning() As String
            Get
                Return _morning.Text
            End Get
        End Property

        Private _noon As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        Public ReadOnly Property Noon() As String
            Get
                Return _noon.Text
            End Get
        End Property

        Private _afternoon As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        Public ReadOnly Property Afternoon() As String
            Get
                Return _afternoon.Text
            End Get
        End Property

        Private _evening As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        Public ReadOnly Property Evening() As String
            Get
                Return _evening.Text
            End Get
        End Property

        Private _quantity As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        Public ReadOnly Property Quantity() As String
            Get
                Return _quantity.Text
            End Get
        End Property

        Private _beforeBreakfast As String = String.Empty
        Public ReadOnly Property BeforeBreakfast() As Boolean
            Get
                Return _beforeBreakfast.ToBoolean
            End Get
        End Property

        Private _beforeLunch As String = String.Empty
        Public ReadOnly Property BeforeLunch() As String
            Get
                Return _beforeLunch
            End Get
        End Property

        Private _beforeSupper As String = String.Empty
        Public ReadOnly Property BeforeSupper() As String
            Get
                Return _beforeSupper
            End Get
        End Property

        Private _beforeDinner As String = String.Empty
        Public ReadOnly Property BeforeDinner() As String
            Get
                Return _beforeDinner
            End Get
        End Property

        Private _instruction As String = String.Empty
        Public ReadOnly Property Instruction() As String
            Get
                Return _instruction
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
                Return _prescriptionNo.ToString
            End Get
        End Property

        Public ReadOnly Property Description As String Implements IInfo.Description
            Get
                Return _instruction
            End Get
        End Property


        Public Sub InvalidateCache() Implements IInfo.InvalidateCache
            PrescriptionDetailInfoList.InvalidateCache()
        End Sub


#End Region 'Business Properties and Methods

#Region " Factory Methods "

        Friend Shared Function GetPrescriptionDetailInfo(ByVal dr As SafeDataReader) As PrescriptionDetailInfo
            Return New PrescriptionDetailInfo(dr)
        End Function

        Friend Shared Function EmptyPrescriptionDetailInfo(Optional ByVal pLineNo As String = "") As PrescriptionDetailInfo
            Dim info As PrescriptionDetailInfo = New PrescriptionDetailInfo
            With info
                ._lineNo = pLineNo

            End With
            Return info
        End Function

        Private Sub New(ByVal dr As SafeDataReader)
            _lineNo = dr.GetInt32("LINE_NO")
            _prescriptionNo.Text = dr.GetInt32("PRESCRIPTION_NO")
            _itemCode = dr.GetString("ITEM_CODE").TrimEnd
            _itemName = dr.GetString("ITEM_NAME").TrimEnd
            _activeIngrendient = dr.GetString("ACTIVE_INGRENDIENT").TrimEnd
            _unit = dr.GetString("UNIT").TrimEnd
            _dateOfIssue.Text = dr.GetInt32("DATE_OF_ISSUE")
            _morning.Text = dr.GetInt32("MORNING")
            _noon.Text = dr.GetInt32("NOON")
            _afternoon.Text = dr.GetInt32("AFTERNOON")
            _evening.Text = dr.GetInt32("EVENING")
            _quantity.Text = dr.GetInt32("QUANTITY")
            _beforeBreakfast = dr.GetString("BEFORE_BREAKFAST").TrimEnd
            _beforeLunch = dr.GetString("BEFORE_LUNCH").TrimEnd
            _beforeSupper = dr.GetString("BEFORE_SUPPER").TrimEnd
            _beforeDinner = dr.GetString("BEFORE_DINNER").TrimEnd
            _instruction = dr.GetString("INSTRUCTION").TrimEnd
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