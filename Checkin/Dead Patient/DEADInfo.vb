
Imports pbs.Helper
Imports pbs.Helper.Interfaces
Imports System.Data
Imports Csla
Imports Csla.Data
Imports Csla.Validation

Namespace MC

    <Serializable()>
    Public Class DEADInfo
        Inherits Csla.ReadOnlyBase(Of DEADInfo)
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

        Private _checkinNo As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
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

        Private _deadTime As pbs.Helper.SmartTime = New pbs.Helper.SmartTime()
        Public ReadOnly Property DeadTime() As String
            Get
                Return _deadTime.Text
            End Get
        End Property

        Private _deadDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property DeadDate() As String
            Get
                Return _deadDate.Text
            End Get
        End Property

        Private _deadReason As String = String.Empty
        Public ReadOnly Property DeadReason() As String
            Get
                Return _deadReason
            End Get
        End Property

        Private _mainReason As String = String.Empty
        Public ReadOnly Property MainReason() As String
            Get
                Return _mainReason
            End Get
        End Property

        Private _mainReasonCode As String = String.Empty
        Public ReadOnly Property MainReasonCode() As String
            Get
                Return _mainReasonCode
            End Get
        End Property

        Private _autopsy As String = String.Empty
        Public ReadOnly Property Autopsy() As String
            Get
                Return _autopsy
            End Get
        End Property

        Private _autopsyResult As String = String.Empty
        Public ReadOnly Property AutopsyResult() As String
            Get
                Return _autopsyResult
            End Get
        End Property

        Private _autopsyResultCode As String = String.Empty
        Public ReadOnly Property AutopsyResultCode() As String
            Get
                Return _autopsyResultCode
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
                Return _autopsyResult
            End Get
        End Property


        Public Sub InvalidateCache() Implements IInfo.InvalidateCache
            DEADInfoList.InvalidateCache()
        End Sub


#End Region 'Business Properties and Methods

#Region " Factory Methods "

        Friend Shared Function GetDEADInfo(ByVal dr As SafeDataReader) As DEADInfo
            Return New DEADInfo(dr)
        End Function

        Friend Shared Function EmptyDEADInfo(Optional ByVal pLineNo As String = "") As DEADInfo
            Dim info As DEADInfo = New DEADInfo
            With info
                ._lineNo = pLineNo

            End With
            Return info
        End Function

        Private Sub New(ByVal dr As SafeDataReader)
            _lineNo = dr.GetInt32("LINE_NO")
            _checkinNo.Text = dr.GetInt32("CHECKIN_NO")
            _patientCode = dr.GetString("PATIENT_CODE").TrimEnd
            _deadTime.Text = dr.GetInt32("DEAD_TIME")
            _deadDate.Text = dr.GetInt32("DEAD_DATE")
            _deadReason = dr.GetString("DEAD_REASON").TrimEnd
            _mainReason = dr.GetString("MAIN_REASON").TrimEnd
            _mainReasonCode = dr.GetString("MAIN_REASON_CODE").TrimEnd
            _autopsy = dr.GetString("AUTOPSY").TrimEnd
            _autopsyResult = dr.GetString("AUTOPSY_RESULT").TrimEnd
            _autopsyResultCode = dr.GetString("AUTOPSY_RESULT_CODE").TrimEnd
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