
Imports pbs.Helper
Imports pbs.Helper.Interfaces
Imports System.Data
Imports Csla
Imports Csla.Data
Imports Csla.Validation

Namespace MC

    <Serializable()>
    Public Class LABInfo
        Inherits Csla.ReadOnlyBase(Of LABInfo)
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

        Private _cabinetNo As String = String.Empty
        Public ReadOnly Property CabinetNo() As String
            Get
                Return _cabinetNo
            End Get
        End Property

        Private _waitingNumber As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        Public ReadOnly Property WaitingNumber() As String
            Get
                Return _waitingNumber.Text
            End Get
        End Property

        Private _labPackageCode As String = String.Empty
        Public ReadOnly Property LabPackageCode() As String
            Get
                Return _labPackageCode
            End Get
        End Property

        Private _labName As String = String.Empty
        Public ReadOnly Property LabName() As String
            Get
                Return _labName
            End Get
        End Property

        Private _analysisDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property AnalysisDate() As String
            Get
                Return _analysisDate.Text
            End Get
        End Property

        Private _analysisTime As pbs.Helper.SmartTime = New pbs.Helper.SmartTime()
        Public ReadOnly Property AnalysisTime() As String
            Get
                Return _analysisTime.Text
            End Get
        End Property

        Private _doctorAppointed As String = String.Empty
        Public ReadOnly Property DoctorAppointed() As String
            Get
                Return _doctorAppointed
            End Get
        End Property

        Private _result As String = String.Empty
        Public ReadOnly Property Result() As String
            Get
                Return _result
            End Get
        End Property

        Private _laboratoryTechnician As String = String.Empty
        Public ReadOnly Property LaboratoryTechnician() As String
            Get
                Return _laboratoryTechnician
            End Get
        End Property

        Private _status As String = String.Empty
        Public ReadOnly Property Status() As String
            Get
                Return _status
            End Get
        End Property

        Private _ncLb9 As String = String.Empty
        Public ReadOnly Property NcLb9() As String
            Get
                Return _ncLb9
            End Get
        End Property

        Private _ncLb8 As String = String.Empty
        Public ReadOnly Property NcLb8() As String
            Get
                Return _ncLb8
            End Get
        End Property

        Private _ncLb7 As String = String.Empty
        Public ReadOnly Property NcLb7() As String
            Get
                Return _ncLb7
            End Get
        End Property

        Private _ncLb6 As String = String.Empty
        Public ReadOnly Property NcLb6() As String
            Get
                Return _ncLb6
            End Get
        End Property

        Private _ncLb5 As String = String.Empty
        Public ReadOnly Property NcLb5() As String
            Get
                Return _ncLb5
            End Get
        End Property

        Private _ncLb4 As String = String.Empty
        Public ReadOnly Property NcLb4() As String
            Get
                Return _ncLb4
            End Get
        End Property

        Private _ncLb3 As String = String.Empty
        Public ReadOnly Property NcLb3() As String
            Get
                Return _ncLb3
            End Get
        End Property

        Private _ncLb2 As String = String.Empty
        Public ReadOnly Property NcLb2() As String
            Get
                Return _ncLb2
            End Get
        End Property

        Private _ncLb1 As String = String.Empty
        Public ReadOnly Property NcLb1() As String
            Get
                Return _ncLb1
            End Get
        End Property

        Private _ncLb0 As String = String.Empty
        Public ReadOnly Property NcLb0() As String
            Get
                Return _ncLb0
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
                Return _lineNo
            End Get
        End Property

        Public ReadOnly Property Description As String Implements IInfo.Description
            Get
                Return _labName
            End Get
        End Property


        Public Sub InvalidateCache() Implements IInfo.InvalidateCache
            LABInfoList.InvalidateCache()
        End Sub


#End Region 'Business Properties and Methods

#Region " Factory Methods "

        Friend Shared Function GetLABInfo(ByVal dr As SafeDataReader) As LABInfo
            Return New LABInfo(dr)
        End Function

        Friend Shared Function EmptyLABInfo(Optional ByVal pLineNo As String = "") As LABInfo
            Dim info As LABInfo = New LABInfo
            With info
                ._lineNo = pLineNo

            End With
            Return info
        End Function

        Private Sub New(ByVal dr As SafeDataReader)
            _lineNo = dr.GetInt32("LINE_NO")
            _patientCode = dr.GetString("PATIENT_CODE").TrimEnd
            _checkinNo.Text = dr.GetInt32("CHECKIN_NO")
            _cabinetNo = dr.GetString("CABINET_NO").TrimEnd
            _waitingNumber.Text = dr.GetInt32("WAITING_NUMBER")
            _labPackageCode = dr.GetString("LAB_PACKAGE_CODE").TrimEnd
            _labName = dr.GetString("LAB_NAME").TrimEnd
            _analysisDate.Text = dr.GetInt32("ANALYSIS_DATE")
            _analysisTime.Text = dr.GetInt32("ANALYSIS_TIME")
            _doctorAppointed = dr.GetString("DOCTOR_APPOINTED").TrimEnd
            _result = dr.GetString("RESULT").TrimEnd
            _laboratoryTechnician = dr.GetString("LABORATORY_TECHNICIAN").TrimEnd
            _status = dr.GetString("STATUS").TrimEnd
            _ncLb9 = dr.GetString("NC_LB9").TrimEnd
            _ncLb8 = dr.GetString("NC_LB8").TrimEnd
            _ncLb7 = dr.GetString("NC_LB7").TrimEnd
            _ncLb6 = dr.GetString("NC_LB6").TrimEnd
            _ncLb5 = dr.GetString("NC_LB5").TrimEnd
            _ncLb4 = dr.GetString("NC_LB4").TrimEnd
            _ncLb3 = dr.GetString("NC_LB3").TrimEnd
            _ncLb2 = dr.GetString("NC_LB2").TrimEnd
            _ncLb1 = dr.GetString("NC_LB1").TrimEnd
            _ncLb0 = dr.GetString("NC_LB0").TrimEnd
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