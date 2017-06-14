
Imports pbs.Helper
Imports pbs.Helper.Interfaces
Imports System.Data
Imports Csla
Imports Csla.Data
Imports Csla.Validation

Namespace MC

    <Serializable()>
    Public Class SURGERYInfo
        Inherits Csla.ReadOnlyBase(Of SURGERYInfo)
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

        Private _gencheckNo As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        Public ReadOnly Property GencheckNo() As String
            Get
                Return _gencheckNo.Text
            End Get
        End Property

        Private _surgeryDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property SurgeryDate() As String
            Get
                Return _surgeryDate.Text
            End Get
        End Property

        Private _surgeryTime As pbs.Helper.SmartTime = New pbs.Helper.SmartTime()
        Public ReadOnly Property SurgeryTime() As String
            Get
                Return _surgeryTime.Text
            End Get
        End Property

        Private _preoperativeDiagnos As String = String.Empty
        Public ReadOnly Property PreoperativeDiagnos() As String
            Get
                Return _preoperativeDiagnos
            End Get
        End Property

        Private _postoperativeDiagnos As String = String.Empty
        Public ReadOnly Property PostoperativeDiagnos() As String
            Get
                Return _postoperativeDiagnos
            End Get
        End Property

        Private _surgicalType As String = String.Empty
        Public ReadOnly Property SurgicalType() As String
            Get
                Return _surgicalType
            End Get
        End Property

        Private _surgicalMethod As String = String.Empty
        Public ReadOnly Property SurgicalMethod() As String
            Get
                Return _surgicalMethod
            End Get
        End Property

        Private _surgeon As String = String.Empty
        Public ReadOnly Property Surgeon() As String
            Get
                Return _surgeon
            End Get
        End Property

        Private _anaesthetist As String = String.Empty
        Public ReadOnly Property Anaesthetist() As String
            Get
                Return _anaesthetist
            End Get
        End Property

        Private _surgeryDiagram As String = String.Empty
        Public ReadOnly Property SurgeryDiagram() As String
            Get
                Return _surgeryDiagram
            End Get
        End Property

        Private _operationSteps As String = String.Empty
        Public ReadOnly Property OperationSteps() As String
            Get
                Return _operationSteps
            End Get
        End Property

        Private _result As String = String.Empty
        Public ReadOnly Property Result() As String
            Get
                Return _result
            End Get
        End Property

        Private _treatment As String = String.Empty
        Public ReadOnly Property Treatment() As String
            Get
                Return _treatment
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
                Return _patientCode
            End Get
        End Property

        Public ReadOnly Property Description As String Implements IInfo.Description
            Get
                Return _preoperativeDiagnos
            End Get
        End Property


        Public Sub InvalidateCache() Implements IInfo.InvalidateCache
            SURGERYInfoList.InvalidateCache()
        End Sub


#End Region 'Business Properties and Methods

#Region " Factory Methods "

        Friend Shared Function GetSURGERYInfo(ByVal dr As SafeDataReader) As SURGERYInfo
            Return New SURGERYInfo(dr)
        End Function

        Friend Shared Function EmptySURGERYInfo(Optional ByVal pLineNo As String = "") As SURGERYInfo
            Dim info As SURGERYInfo = New SURGERYInfo
            With info
                ._lineNo = pLineNo

            End With
            Return info
        End Function

        Private Sub New(ByVal dr As SafeDataReader)
            _lineNo = dr.GetInt32("LINE_NO")
            _patientCode = dr.GetString("PATIENT_CODE").TrimEnd
            _checkinNo.Text = dr.GetInt32("CHECKIN_NO")
            _gencheckNo.Text = dr.GetInt32("GENCHECK_NO")
            _surgeryDate.Text = dr.GetInt32("SURGERY_DATE")
            _surgeryTime.Text = dr.GetInt32("SURGERY_TIME")
            _preoperativeDiagnos = dr.GetString("PREOPERATIVE_DIAGNOS").TrimEnd
            _postoperativeDiagnos = dr.GetString("POSTOPERATIVE_DIAGNOS").TrimEnd
            _surgicalType = dr.GetString("SURGICAL_TYPE").TrimEnd
            _surgicalMethod = dr.GetString("SURGICAL_METHOD").TrimEnd
            _surgeon = dr.GetString("SURGEON").TrimEnd
            _anaesthetist = dr.GetString("ANAESTHETIST").TrimEnd
            _surgeryDiagram = dr.GetString("SURGERY_DIAGRAM").TrimEnd
            _operationSteps = dr.GetString("OPERATION_STEPS").TrimEnd
            _result = dr.GetString("RESULT").TrimEnd
            _treatment = dr.GetString("TREATMENT").TrimEnd
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