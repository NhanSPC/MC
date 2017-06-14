
Imports pbs.Helper
Imports pbs.Helper.Interfaces
Imports System.Data
Imports Csla
Imports Csla.Data
Imports Csla.Validation

Namespace MC

    <Serializable()> _
    Public Class GENCHECKInfo
        Inherits Csla.ReadOnlyBase(Of GENCHECKInfo)
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

        Private _waitingNumber As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        Public ReadOnly Property WaitingNumber() As String
            Get
                Return _waitingNumber.Text
            End Get
        End Property

        Private _pulse As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        Public ReadOnly Property Pulse() As String
            Get
                Return _pulse.Text
            End Get
        End Property

        Private _temperature As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property Temperature() As String
            Get
                Return _temperature.Text
            End Get
        End Property

        Private _bloodPressure As String = String.Empty
        Public ReadOnly Property BloodPressure() As String
            Get
                Return _bloodPressure
            End Get
        End Property

        Private _breathingRate As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        Public ReadOnly Property BreathingRate() As String
            Get
                Return _breathingRate.Text
            End Get
        End Property

        Private _weight As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property Weight() As String
            Get
                Return _weight.Text
            End Get
        End Property

        Private _height As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property Height() As String
            Get
                Return _height.Text
            End Get
        End Property

        Private _bloodgroup As String = String.Empty
        Public ReadOnly Property Bloodgroup() As String
            Get
                Return _bloodgroup
            End Get
        End Property

        Private _checkBy As String = String.Empty
        Public ReadOnly Property CheckBy() As String
            Get
                Return _checkBy
            End Get
        End Property

        Private _checkDate As pbs.Helper.SmartDate = New Helper.SmartDate()
        Public ReadOnly Property CheckDate() As String
            Get
                Return _checkDate.Text
            End Get
        End Property

        Private _extDecs5 As String = String.Empty
        Public ReadOnly Property ExtDecs5() As String
            Get
                Return _extDecs5
            End Get
        End Property

        Private _extDecs4 As String = String.Empty
        Public ReadOnly Property ExtDecs4() As String
            Get
                Return _extDecs4
            End Get
        End Property

        Private _extDecs3 As String = String.Empty
        Public ReadOnly Property ExtDecs3() As String
            Get
                Return _extDecs3
            End Get
        End Property

        Private _extDecs2 As String = String.Empty
        Public ReadOnly Property ExtDecs2() As String
            Get
                Return _extDecs2
            End Get
        End Property

        Private _extDecs1 As String = String.Empty
        Public ReadOnly Property ExtDecs1() As String
            Get
                Return _extDecs1
            End Get
        End Property

        Private _extVal5 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property ExtVal5() As String
            Get
                Return _extVal5.Text
            End Get
        End Property

        Private _extVal4 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property ExtVal4() As String
            Get
                Return _extVal4.Text
            End Get
        End Property

        Private _extVal3 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property ExtVal3() As String
            Get
                Return _extVal3.Text
            End Get
        End Property

        Private _extVal2 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property ExtVal2() As String
            Get
                Return _extVal2.Text
            End Get
        End Property

        Private _extVal1 As pbs.Helper.SmartFloat = New pbs.Helper.SmartFloat(0)
        Public ReadOnly Property ExtVal1() As String
            Get
                Return _extVal1.Text
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
                Return _patientCode
            End Get
        End Property


        Public Sub InvalidateCache() Implements IInfo.InvalidateCache
            GENCHECKInfoList.InvalidateCache()
        End Sub


#End Region 'Business Properties and Methods

#Region " Factory Methods "

        Friend Shared Function GetGENCHECKInfo(ByVal dr As SafeDataReader) As GENCHECKInfo
            Return New GENCHECKInfo(dr)
        End Function

        Friend Shared Function EmptyGENCHECKInfo(Optional ByVal pLineNo As String = "") As GENCHECKInfo
            Dim info As GENCHECKInfo = New GENCHECKInfo
            With info
                ._lineNo = pLineNo

            End With
            Return info
        End Function

        Private Sub New(ByVal dr As SafeDataReader)
            _lineNo = dr.GetInt32("LINE_NO")
            _patientCode = dr.GetString("PATIENT_CODE").TrimEnd
            _checkinNo.Text = dr.GetInt32("CHECKIN_NO")
            _waitingNumber.Text = dr.GetInt32("WAITING_NUMBER")
            _pulse.Text = dr.GetInt32("PULSE")
            _temperature.Text = dr.GetDecimal("TEMPERATURE")
            _bloodPressure = dr.GetString("BLOOD_PRESSURE")
            _breathingRate.Text = dr.GetInt32("BREATHING_RATE")
            _weight.Text = dr.GetDecimal("WEIGHT")
            _height.Text = dr.GetDecimal("HEIGHT")
            _bloodgroup = dr.GetString("BLOOD_GROUP").TrimEnd
            _checkDate.Text = dr.GetInt32("CHECK_DATE")
            _checkBy = dr.GetString("CHECK_BY")
            _extDecs5 = dr.GetString("EXT_DECS5").TrimEnd
            _extDecs4 = dr.GetString("EXT_DECS4").TrimEnd
            _extDecs3 = dr.GetString("EXT_DECS3").TrimEnd
            _extDecs2 = dr.GetString("EXT_DECS2").TrimEnd
            _extDecs1 = dr.GetString("EXT_DECS1").TrimEnd
            _extVal5.Text = dr.GetDecimal("EXT_VAL5")
            _extVal4.Text = dr.GetDecimal("EXT_VAL4")
            _extVal3.Text = dr.GetDecimal("EXT_VAL3")
            _extVal2.Text = dr.GetDecimal("EXT_VAL2")
            _extVal1.Text = dr.GetDecimal("EXT_VAL1")
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