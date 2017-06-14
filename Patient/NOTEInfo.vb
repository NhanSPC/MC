
Imports pbs.Helper
Imports pbs.Helper.Interfaces
Imports System.Data
Imports Csla
Imports Csla.Data
Imports Csla.Validation

Namespace MC

    <Serializable()> _
    Public Class NOTEInfo
        Inherits Csla.ReadOnlyBase(Of NOTEInfo)
        Implements IComparable
        Implements IInfo
        Implements IDocLink
        Implements ICheckable

#Region "Checkable"

        Public Property Checked As Boolean

        Public Sub Check() Implements ICheckable.Check
            Checked = True
        End Sub

        Public Sub Uncheck() Implements ICheckable.Uncheck
            Checked = False
        End Sub
#End Region

#Region " Business Properties and Methods "

        Private _lineNo As Integer
        Public ReadOnly Property LineNo() As Integer
            Get
                Return _lineNo
            End Get
        End Property

        'Private _leadId As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        'Public ReadOnly Property LeadId() As Integer
        '    Get
        '        Return _leadId.Int
        '    End Get
        'End Property

        'Private _candidateId As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        'Public ReadOnly Property CandidateId() As String
        '    Get
        '        Return _candidateId.DocNumber
        '    End Get
        'End Property

        Private _patientCode As String = String.Empty
        Public ReadOnly Property PatientCode() As String
            Get
                Return _patientCode
            End Get
        End Property

        Private _topic As String = String.Empty
        Public ReadOnly Property Topic() As String
            Get
                Return _topic
            End Get
        End Property

        Private _description As String = String.Empty

        Private _note As String = String.Empty
        Public ReadOnly Property Note() As String
            Get
                Return _note
            End Get
        End Property

        Private _category As String = String.Empty
        Public ReadOnly Property Category() As String
            Get
                Return _category
            End Get
        End Property

        Private _sortNo As String = String.Empty
        Public ReadOnly Property SortNo() As String
            Get
                Return _sortNo
            End Get
        End Property

        Private _noteValue As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property NoteValue() As String
            Get
                Return _noteValue.Text
            End Get
        End Property

        Private _unit As String = String.Empty
        Public ReadOnly Property Unit() As String
            Get
                Return _unit
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

        Private _noteValue1 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property NoteValue1() As String
            Get
                Return _noteValue1.Text
            End Get
        End Property

        Private _noteValue2 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property NoteValue2() As String
            Get
                Return _noteValue2.Text
            End Get
        End Property

        Private _noteValue3 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property NoteValue3() As String
            Get
                Return _noteValue3.Text
            End Get
        End Property

        Private _noteValue4 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property NoteValue4() As String
            Get
                Return _noteValue4.Text
            End Get
        End Property

        Private _noteValue5 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property NoteValue5() As String
            Get
                Return _noteValue5.Text
            End Get
        End Property

        Private _noteDate1 As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property NoteDate1() As String
            Get
                Return _noteDate1.DateViewFormat
            End Get
        End Property

        Private _noteDate2 As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property NoteDate2() As String
            Get
                Return _noteDate2.DateViewFormat
            End Get
        End Property

        Private _noteDate3 As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property NoteDate3() As String
            Get
                Return _noteDate3.DateViewFormat
            End Get
        End Property

        Private _noteDate4 As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property NoteDate4() As String
            Get
                Return _noteDate4.DateViewFormat
            End Get
        End Property

        Private _noteDate5 As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property NoteDate5() As String
            Get
                Return _noteDate5.DateViewFormat
            End Get
        End Property

        Private _notePeriod1 As SmartPeriod = New pbs.Helper.SmartPeriod()
        Public ReadOnly Property NotePeriod1() As String
            Get
                Return _notePeriod1.Text
            End Get
        End Property

        Private _notePeriod2 As SmartPeriod = New pbs.Helper.SmartPeriod()
        Public ReadOnly Property NotePeriod2() As String
            Get
                Return _notePeriod2.Text
            End Get
        End Property

        Private _notePeriod3 As SmartPeriod = New pbs.Helper.SmartPeriod()
        Public ReadOnly Property NotePeriod3() As String
            Get
                Return _notePeriod3.Text
            End Get
        End Property

        Private _notePeriod4 As SmartPeriod = New pbs.Helper.SmartPeriod()
        Public ReadOnly Property NotePeriod4() As String
            Get
                Return _notePeriod4.Text
            End Get
        End Property

        Private _notePeriod5 As SmartPeriod = New pbs.Helper.SmartPeriod()
        Public ReadOnly Property NotePeriod5() As String
            Get
                Return _notePeriod5.Text
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

        Private _updatedBy As String = String.Empty
        Public ReadOnly Property UpdatedBy() As String
            Get
                Return _updatedBy
            End Get
        End Property

        Private _updated As pbs.Helper.SmartDate = New pbs.Helper.SmartDate()
        Public ReadOnly Property Updated() As String
            Get
                Return _updated.Text
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
                Return _sortNo
            End Get
        End Property

        Public ReadOnly Property Description As String Implements IInfo.Description
            Get
                Return _description
            End Get
        End Property


        Public Sub InvalidateCache() Implements IInfo.InvalidateCache
            NOTEInfoList.InvalidateCache()
        End Sub


#End Region 'Business Properties and Methods

#Region " Factory Methods "

        Friend Shared Function GetNOTEInfo(ByVal dr As SafeDataReader) As NOTEInfo
            Return New NOTEInfo(dr)
        End Function

        Friend Shared Function EmptyNOTEInfo(Optional ByVal pLineNo As Integer = 0) As NOTEInfo
            Dim info As NOTEInfo = New NOTEInfo
            With info
                ._lineNo = pLineNo

            End With
            Return info
        End Function

        Private Sub New(ByVal dr As SafeDataReader)
            _lineNo = dr.GetInt32("LINE_NO")
            '_leadId.Text = dr.GetInt32("LEAD_ID")
            '_candidateId.Text = dr.GetInt32("CANDIDATE_ID")
            _patientCode = dr.GetString("Patient_CODE").TrimEnd
            _topic = dr.GetString("TOPIC").TrimEnd
            _description = dr.GetString("DESCRIPTION").TrimEnd
            _note = dr.GetString("NOTE").TrimEnd
            _category = dr.GetString("CATEGORY").TrimEnd
            _sortNo = dr.GetString("SORT_NO").TrimEnd
            _noteValue.Text = dr.GetDecimal("NOTE_VALUE")
            _unit = dr.GetString("UNIT").TrimEnd
            _extDesc1 = dr.GetString("EXT_DESC1").TrimEnd
            _extDesc2 = dr.GetString("EXT_DESC2").TrimEnd
            _extDesc3 = dr.GetString("EXT_DESC3").TrimEnd
            _extDesc4 = dr.GetString("EXT_DESC4").TrimEnd
            _extDesc5 = dr.GetString("EXT_DESC5").TrimEnd
            _noteValue1.Text = dr.GetDecimal("NOTE_VALUE1")
            _noteValue2.Text = dr.GetDecimal("NOTE_VALUE2")
            _noteValue3.Text = dr.GetDecimal("NOTE_VALUE3")
            _noteValue4.Text = dr.GetDecimal("NOTE_VALUE4")
            _noteValue5.Text = dr.GetDecimal("NOTE_VALUE5")
            _noteDate1.Text = dr.GetInt32("NOTE_DATE1")
            _noteDate2.Text = dr.GetInt32("NOTE_DATE2")
            _noteDate3.Text = dr.GetInt32("NOTE_DATE3")
            _noteDate4.Text = dr.GetInt32("NOTE_DATE4")
            _noteDate5.Text = dr.GetInt32("NOTE_DATE5")
            _notePeriod1.Text = dr.GetInt32("NOTE_PERIOD1")
            _notePeriod2.Text = dr.GetInt32("NOTE_PERIOD2")
            _notePeriod3.Text = dr.GetInt32("NOTE_PERIOD3")
            _notePeriod4.Text = dr.GetInt32("NOTE_PERIOD4")
            _notePeriod5.Text = dr.GetInt32("NOTE_PERIOD5")
            _bphNo = dr.GetInt32("BPH_NO")
            _formDataId = dr.GetInt32("FORM_DATA_ID")
            _updatedBy = dr.GetString("UPDATED_BY").TrimEnd
            _updated.Text = dr.GetInt32("UPDATED")

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