
Imports pbs.Helper
Imports pbs.Helper.Interfaces
Imports System.Data
Imports Csla
Imports Csla.Data
Imports Csla.Validation

Namespace MC

    <Serializable()> _
    Public Class LOGInfo
        Inherits Csla.ReadOnlyBase(Of LOGInfo)
        Implements IComparable
        Implements IInfo
        Implements IDocLink
        'Implements IInfoStatus


#Region " Business Properties and Methods "


        Private _lineNo As Integer
        Public ReadOnly Property LineNo() As Integer
            Get
                Return _lineNo
            End Get
        End Property

        Private _logType As String = String.Empty
        Public ReadOnly Property LogType() As String
            Get
                Return _logType
            End Get
        End Property

        Private _logDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property LogDate() As String
            Get
                Return _logDate.DateViewFormat
            End Get
        End Property

        Private _logTime As pbs.Helper.SmartTime = New pbs.Helper.SmartTime()
        Public ReadOnly Property LogTime() As String
            Get
                Return _logTime.Text
            End Get
        End Property

        Private _logObject As String = String.Empty
        Public ReadOnly Property LogObject() As String
            Get
                Return _logObject
            End Get
        End Property

        Private _logMessage As String = String.Empty
        Public ReadOnly Property LogMessage() As String
            Get
                Return _logMessage
            End Get
        End Property

        Private _description As String = String.Empty

        Private _priority As String = String.Empty
        Public ReadOnly Property Priority() As String
            Get
                Return _priority
            End Get
        End Property

        Private _bphNo As Integer
        Public ReadOnly Property BphNo() As Integer
            Get
                Return _bphNo
            End Get
        End Property

        Private _formDataId As Integer
        Public ReadOnly Property FormDataId() As Integer
            Get
                Return _formDataId
            End Get
        End Property

        Private _patientCode As String = String.Empty
        Public ReadOnly Property PatientCode() As String
            Get
                Return _patientCode
            End Get
        End Property

        Private _nextAction As String = String.Empty
        Public ReadOnly Property NextAction() As String
            Get
                Return _nextAction
            End Get
        End Property

        Private _taskId As Integer
        Public ReadOnly Property TaskId() As Integer
            Get
                Return _taskId
            End Get
        End Property

        Private _assignedTo As String = String.Empty
        Public ReadOnly Property AssignedTo() As String
            Get
                Return _assignedTo
            End Get
        End Property

        Private _assignedDate As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property AssignedDate() As String
            Get
                Return _assignedDate.DateViewFormat
            End Get
        End Property

        Private _deadline As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property Deadline() As String
            Get
                Return _deadline.DateViewFormat
            End Get
        End Property

        Private _notes As String = String.Empty
        Public ReadOnly Property Notes() As String
            Get
                Return _notes
            End Get
        End Property

        Private _updated As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property Updated() As String
            Get
                Return _updated.DateViewFormat
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
                Return _patientCode
            End Get
        End Property

        Public ReadOnly Property Description As String Implements IInfo.Description
            Get
                Return _description
            End Get
        End Property


        Public Sub InvalidateCache() Implements IInfo.InvalidateCache
            LOGInfoList.InvalidateCache()
        End Sub


#End Region 'Business Properties and Methods

#Region "Extended"

       

        Private _ncLg0 As String = String.Empty
        Public ReadOnly Property NcLg0() As String
            Get
                Return _ncLg0
            End Get
        End Property

        Private _ncLg1 As String = String.Empty
        Public ReadOnly Property NcLg1() As String
            Get
                Return _ncLg1
            End Get
        End Property

        Private _ncLg2 As String = String.Empty
        Public ReadOnly Property NcLg2() As String
            Get
                Return _ncLg2
            End Get
        End Property

        Private _ncLg3 As String = String.Empty
        Public ReadOnly Property NcLg3() As String
            Get
                Return _ncLg3
            End Get
        End Property

        Private _ncLg4 As String = String.Empty
        Public ReadOnly Property NcLg4() As String
            Get
                Return _ncLg4
            End Get
        End Property

        Private _ncLg5 As String = String.Empty
        Public ReadOnly Property NcLg5() As String
            Get
                Return _ncLg5
            End Get
        End Property

        Private _ncLg6 As String = String.Empty
        Public ReadOnly Property NcLg6() As String
            Get
                Return _ncLg6
            End Get
        End Property

        Private _ncLg7 As String = String.Empty
        Public ReadOnly Property NcLg7() As String
            Get
                Return _ncLg7
            End Get
        End Property

        Private _ncLg8 As String = String.Empty
        Public ReadOnly Property NcLg8() As String
            Get
                Return _ncLg8
            End Get
        End Property

        Private _ncLg9 As String = String.Empty
        Public ReadOnly Property NcLg9() As String
            Get
                Return _ncLg9
            End Get
        End Property

        Private _extDesc0 As String = String.Empty
        Public ReadOnly Property ExtDesc0() As String
            Get
                Return _extDesc0
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

        Private _extDate0 As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property ExtDate0() As String
            Get
                Return _extDate0.DateViewFormat
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

        Private _extValue0 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property ExtValue0() As String
            Get
                Return _extValue0.Text
            End Get
        End Property

        Private _extValue1 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property ExtValue1() As String
            Get
                Return _extValue1.Text
            End Get
        End Property

        Private _extValue2 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property ExtValue2() As String
            Get
                Return _extValue2.Text
            End Get
        End Property

        Private _extValue3 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property ExtValue3() As String
            Get
                Return _extValue3.Text
            End Get
        End Property

        Private _extValue4 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property ExtValue4() As String
            Get
                Return _extValue4.Text
            End Get
        End Property

#End Region

#Region " Factory Methods "

        Friend Shared Function GetLOGInfo(ByVal dr As SafeDataReader) As LOGInfo
            Return New LOGInfo(dr)
        End Function

        Friend Shared Function EmptyLOGInfo(Optional ByVal pLineNo As Integer = 0) As LOGInfo
            Dim info As LOGInfo = New LOGInfo
            With info
                ._lineNo = pLineNo

            End With
            Return info
        End Function

        Private Sub New(ByVal dr As SafeDataReader)
            _lineNo = dr.GetInt32("LINE_NO")
            _logType = dr.GetString("LOG_TYPE").TrimEnd
            _logDate.Text = dr.GetInt32("LOG_DATE")
            _logTime.Text = dr.GetInt32("LOG_TIME")
            _logObject = dr.GetString("LOG_OBJECT").TrimEnd
            _logMessage = dr.GetString("LOG_MESSAGE").TrimEnd
            _description = dr.GetString("DESCRIPTION").TrimEnd
            _priority = dr.GetString("PRIORITY").TrimEnd
            _bphNo = dr.GetInt32("BPH_NO")
            _formDataId = dr.GetInt32("FORM_DATA_ID")
         
            _patientCode = dr.GetString("PATIENT_CODE").TrimEnd
            _nextAction = dr.GetString("NEXT_ACTION").TrimEnd
            _taskId = dr.GetInt32("TASK_ID")
            _assignedTo = dr.GetString("ASSIGNED_TO").TrimEnd
            _assignedDate.Text = dr.GetInt32("ASSIGNED_DATE")
            _deadline.Text = dr.GetInt32("DEADLINE")
            _notes = dr.GetString("NOTES").TrimEnd
            _updated.Text = dr.GetInt32("UPDATED")
            _updatedBy = dr.GetString("UPDATED_BY").TrimEnd


            _ncLg0 = dr.GetString("NC_LG0").TrimEnd
            _ncLg1 = dr.GetString("NC_LG1").TrimEnd
            _ncLg2 = dr.GetString("NC_LG2").TrimEnd
            _ncLg3 = dr.GetString("NC_LG3").TrimEnd
            _ncLg4 = dr.GetString("NC_LG4").TrimEnd
            _ncLg5 = dr.GetString("NC_LG5").TrimEnd
            _ncLg6 = dr.GetString("NC_LG6").TrimEnd
            _ncLg7 = dr.GetString("NC_LG7").TrimEnd
            _ncLg8 = dr.GetString("NC_LG8").TrimEnd
            _ncLg9 = dr.GetString("NC_LG9").TrimEnd
            _extDesc0 = dr.GetString("EXT_DESC0").TrimEnd
            _extDesc1 = dr.GetString("EXT_DESC1").TrimEnd
            _extDesc2 = dr.GetString("EXT_DESC2").TrimEnd
            _extDesc3 = dr.GetString("EXT_DESC3").TrimEnd
            _extDesc4 = dr.GetString("EXT_DESC4").TrimEnd
            _extDate0.Text = dr.GetInt32("EXT_DATE0")
            _extDate1.Text = dr.GetInt32("EXT_DATE1")
            _extDate2.Text = dr.GetInt32("EXT_DATE2")
            _extDate3.Text = dr.GetInt32("EXT_DATE3")
            _extDate4.Text = dr.GetInt32("EXT_DATE4")
            _extValue0.Text = dr.GetDecimal("EXT_VALUE0")
            _extValue1.Text = dr.GetDecimal("EXT_VALUE1")
            _extValue2.Text = dr.GetDecimal("EXT_VALUE2")
            _extValue3.Text = dr.GetDecimal("EXT_VALUE3")
            _extValue4.Text = dr.GetDecimal("EXT_VALUE4")

        End Sub

        Private Sub New()
        End Sub


#End Region ' Factory Methods

#Region "IDoclink"
        Public Function Get_DOL_Reference() As String Implements IDocLink.Get_DOL_Reference
            Return String.Format("{0}#{1}", Get_TransType, _patientCode)
        End Function

        Public Function Get_TransType() As String Implements IDocLink.Get_TransType
            Return Me.GetType.ToClassSchemaName.Leaf
        End Function
#End Region

    End Class

End Namespace