Imports System.Data

Imports Csla
Imports Csla.Data
Imports Csla.Validation
Imports System.Xml.Linq
Imports pbs.Helper

Namespace MC

    <Serializable()> _
    Public Class JDInfo
        Inherits Csla.ReadOnlyBase(Of JDInfo)
        Implements Interfaces.IInfo
        Implements IComparable

#Region " Business Properties and Methods "

        ' declare members
        Private _journalType As String = String.Empty
        Friend _journalName As String = String.Empty
        Private _conversion As String = String.Empty
        Private _dag As String = String.Empty
        Private _reportDefinition As String = String.Empty
        Private _trefSequence As String = String.Empty

        ''' <summary>
        ''' Get the JournalType of the current object.
        ''' </summary>
        ''' <value>A String that represents the JournalType of the current object.</value>
        ''' <returns>String representing the return value.</returns>
        ''' <remarks></remarks>
        <System.ComponentModel.DataObjectField(True, False)> _
        Public ReadOnly Property JournalType() As String
            Get
                Return _journalType
            End Get
        End Property
        Public ReadOnly Property Code() As String Implements Interfaces.IInfo.Code
            Get
                Return _journalType
            End Get
        End Property

        Public ReadOnly Property JournalName() As String Implements Interfaces.IInfo.Description
            Get
                Return _journalName.Trim
            End Get
        End Property

        Public ReadOnly Property LookUp() As String Implements Interfaces.IInfo.LookUp
            Get
                Return String.Empty
            End Get
        End Property

        Public ReadOnly Property Conversion() As Boolean
            Get
                Return _conversion.Trim.ToUpper = "Y"
            End Get
        End Property

        Public ReadOnly Property Dag() As String
            Get
                Return _dag
            End Get
        End Property

        Public ReadOnly Property ReportDefinition() As String
            Get
                Return Nz(_reportDefinition, JournalType)
            End Get
        End Property

        Public ReadOnly Property TrefSequence() As String
            Get
                Return _trefSequence
            End Get

        End Property

        Private _debitCredit As String = String.Empty
        Public ReadOnly Property DebitCredit() As String
            Get
                Return _debitCredit
            End Get
        End Property

        Private _masterFields As String = String.Empty
        Public ReadOnly Property MasterFields() As String
            Get
                Return _masterFields
            End Get
        End Property

        Private _printOnPosting As String = String.Empty
        Public ReadOnly Property PrintOnPosting As Boolean
            Get
                Return _printOnPosting.ToBoolean
            End Get
        End Property

        Private _hardPostingOnly As String = String.Empty
        Public ReadOnly Property HardPostingOnly As Boolean
            Get
                If String.IsNullOrEmpty(_hardPostingOnly) Then
                    Return pbs.BO.MC.Settings.GetSettings.HardPostingOnly
                Else
                    Return _hardPostingOnly.ToBoolean
                End If
            End Get
        End Property

        Private _requireItemCode As String = String.Empty
        Public ReadOnly Property RequireItemCode As Boolean
            Get
                Return _requireItemCode.ToBoolean
            End Get
        End Property

        Protected Overrides Function GetIdValue() As Object
            Return _journalType
        End Function

        Public Function CompareTo(ByVal ID) As Integer Implements IComparable.CompareTo
            If _journalType < ID.ToString Then Return -1
            If _journalType > ID.ToString Then Return 1
            Return 0
        End Function

        Public Sub InvalidateCache() Implements Interfaces.IInfo.InvalidateCache
            JDInfoList.InvalidateCache()
        End Sub

#End Region ' Business Properties and Methods

        Public Function GetMasterFields() As List(Of String)
            Dim ret = New List(Of String)

            If Not String.IsNullOrEmpty(_masterFields) Then
                ret = _masterFields.Split(New Char() {"|", ",", ";"}, StringSplitOptions.RemoveEmptyEntries).ToList
            End If

            Return ret
        End Function

        Friend Function Get_Detail_From_Master_Mapping() As Dictionary(Of String, String)
            Dim _detailFromMasterMapping = New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
            For Each itm In GetMasterFields()
                If Not String.IsNullOrEmpty(itm) Then
                    _detailFromMasterMapping.Add(itm, itm)
                End If
            Next
            Return _detailFromMasterMapping
        End Function

#Region " Factory Methods "

        Friend Shared Function EmptyJDInfo() As JDInfo
            Return New JDInfo
        End Function
        Friend Shared Function SYSTMJDInfo() As JDInfo
            Dim info As JDInfo = New JDInfo
            info._journalType = pbs.Helper.SystemJT
            info._journalName = ResStr(pbs.Helper.SystemJT)
            Return info
        End Function

        'Friend Shared Function ReceivablesJDInfo() As JDInfo
        '    Dim info As JDInfo = New JDInfo
        '    info._journalType = GenerateReceivables.JournalType
        '    info._journalName = ResStr("System generated journal - Receivables")
        '    info._debitCredit = "D"
        '    Return info
        'End Function

        Private Sub New()
            Me._journalName = ResStr(ResStrConst.NODATAFOUND)
        End Sub

        Friend Shared Function GetJDInfo(ByVal dr As SafeDataReader) As JDInfo
            Return New JDInfo(dr)
        End Function

#End Region ' Factory Methods

#Region " Data Access "

#Region " Data Access - Fetch "

        Private Sub New(ByVal dr As SafeDataReader)

            _journalType = dr.GetString("KEY_FIELDS").TrimEnd

            Dim data = dr.GetString("PSM_DATA").TrimEnd.Decompress
            Try

                Dim xele = XElement.Parse(data)

                _journalName = DNz(xele...<_journalName>.Value, String.Empty)
                _conversion = DNz(xele...<_conversion>.Value, String.Empty)
                _dag = DNz(xele...<_dag>.Value, String.Empty)
                _reportDefinition = DNz(xele...<_reportDefinition>.Value, String.Empty)
                _trefSequence = DNz(xele...<_trefSequence>.Value, String.Empty)
                _debitCredit = DNz(xele...<_debitCredit>.Value, String.Empty)
                _masterFields = DNz(xele...<_masterFields>.Value, String.Empty)
                _printOnPosting = DNz(xele...<_printOnPosting>.Value, String.Empty)
                _hardPostingOnly = DNz(xele...<_hardPostingOnly>.Value, String.Empty)
                _requireItemCode = DNz(xele...<_requireItemCode>.Value, "Y")
            Catch ex As Exception
            End Try

        End Sub

#End Region ' Data Access - Fetch

#End Region

    End Class

End Namespace
