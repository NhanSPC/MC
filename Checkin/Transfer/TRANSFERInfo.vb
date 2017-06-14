
Imports pbs.Helper
Imports pbs.Helper.Interfaces
Imports System.Data
Imports Csla
Imports Csla.Data
Imports Csla.Validation

Namespace MC

    <Serializable()>
    Public Class TRANSFERInfo
        Inherits Csla.ReadOnlyBase(Of TRANSFERInfo)
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

        Private _patientCode As String = String.Empty
        Public ReadOnly Property PatientCode() As String
            Get
                Return _patientCode
            End Get
        End Property

        Private _checkinNo As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        Public ReadOnly Property CheckinNo() As String
            Get
                Return _checkinNo.Text
            End Get
        End Property

        Private _transferGroup As String = String.Empty
        Public ReadOnly Property TransferGroup() As String
            Get
                Return _transferGroup
            End Get
        End Property

        Private _transToDept As String = String.Empty
        Public ReadOnly Property TransToDept() As String
            Get
                Return _transToDept
            End Get
        End Property

        Private _transTime As pbs.Helper.SmartTime = New pbs.Helper.SmartTime()
        Public ReadOnly Property TransTime() As String
            Get
                Return _transTime.Text
            End Get
        End Property

        Private _transDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property TransDate() As String
            Get
                Return _transDate.Text
            End Get
        End Property

        Private _deptTreatmentDays As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        Public ReadOnly Property DeptTreatmentDays() As String
            Get
                Return _deptTreatmentDays.Text
            End Get
        End Property

        Private _transferType As String = String.Empty
        Public ReadOnly Property TransferStyle() As String
            Get
                Return _transferType
            End Get
        End Property

        Private _transferTo As String = String.Empty
        Public ReadOnly Property TransferTo() As String
            Get
                Return _transferTo
            End Get
        End Property

        Private _transferReason As String = String.Empty
        Public ReadOnly Property TransferReason() As String
            Get
                Return _transferReason
            End Get
        End Property

        Private _transportation As String = String.Empty
        Public ReadOnly Property Transportation() As String
            Get
                Return _transportation
            End Get
        End Property

        Private _transporter As String = String.Empty
        Public ReadOnly Property Transporter() As String
            Get
                Return _transporter
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
                Return _lineNo.ToString
            End Get
        End Property

        Public ReadOnly Property LookUp As String Implements IInfo.LookUp
            Get
                Return _patientCode
            End Get
        End Property

        Public ReadOnly Property Description As String Implements IInfo.Description
            Get
                Return _transferReason
            End Get
        End Property


        Public Sub InvalidateCache() Implements IInfo.InvalidateCache
            TRANSFERInfoList.InvalidateCache()
        End Sub


#End Region 'Business Properties and Methods

#Region " Factory Methods "

        Friend Shared Function GetTRANSFERInfo(ByVal dr As SafeDataReader) As TRANSFERInfo
            Return New TRANSFERInfo(dr)
        End Function

        Friend Shared Function EmptyTRANSFERInfo(Optional ByVal pLineNo As String = "") As TRANSFERInfo
            Dim info As TRANSFERInfo = New TRANSFERInfo
            With info
                ._lineNo = pLineNo

            End With
            Return info
        End Function

        Private Sub New(ByVal dr As SafeDataReader)
            _lineNo = dr.GetInt32("LINE_NO")
            _patientCode = dr.GetString("PATIENT_CODE").TrimEnd
            _checkinNo.Text = dr.GetInt32("CHECKIN_NO")
            _transferGroup = dr.GetString("TRANSFER_GROUP").TrimEnd
            _transToDept = dr.GetString("TRANS_TO_DEPT").TrimEnd
            _transTime.Text = dr.GetInt32("TRANS_TIME")
            _transDate.Text = dr.GetInt32("TRANS_DATE")
            _deptTreatmentDays.Text = dr.GetInt32("DEPT_TREATMENT_DAYS")
            _transferType = dr.GetString("TRANSFER_TYPE").TrimEnd
            _transferTo = dr.GetString("TRANSFER_TO").TrimEnd
            _transferReason = dr.GetString("TRANSFER_REASON").TrimEnd
            _transportation = dr.GetString("TRANSPORTATION").TrimEnd
            _transporter = dr.GetString("TRANSPORTER").TrimEnd
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