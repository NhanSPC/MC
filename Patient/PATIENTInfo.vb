
Imports pbs.Helper
Imports pbs.Helper.Interfaces
Imports System.Data
Imports Csla
Imports Csla.Data
Imports Csla.Validation

Namespace MC

    <Serializable()> _
    Public Class PATIENTInfo
        Inherits Csla.ReadOnlyBase(Of PATIENTInfo)
        Implements IComparable
        Implements IInfo
        Implements IDocLink


#Region " Business Properties and Methods "


        Private _patientCode As String = String.Empty
        Public ReadOnly Property PatientCode() As String
            Get
                Return _patientCode
            End Get
        End Property

        Private _firstName As String = String.Empty
        Public ReadOnly Property FirstName() As String
            Get
                Return _firstName
            End Get
        End Property

        Private _middleName As String = String.Empty
        Public ReadOnly Property MiddleName() As String
            Get
                Return _middleName
            End Get
        End Property

        Private _lastName As String = String.Empty
        Public ReadOnly Property LastName() As String
            Get
                Return _lastName
            End Get
        End Property

        Private _fullname As String = String.Empty
        Public ReadOnly Property Fullname() As String
            Get
                Return _fullname
            End Get
        End Property

        Private _gender As String = String.Empty
        Public ReadOnly Property Gender() As String
            Get
                Return _gender
            End Get
        End Property

        Private _dob As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        Public ReadOnly Property Dob() As String
            Get
                Return _dob.Text
            End Get
        End Property

        Private _job As String = String.Empty
        Public ReadOnly Property Job() As String
            Get
                Return _job
            End Get
        End Property

        Private _ethnic As String = String.Empty
        Public ReadOnly Property Ethnic() As String
            Get
                Return _ethnic
            End Get
        End Property

        Private _nationality As String = String.Empty
        Public ReadOnly Property Nationality() As String
            Get
                Return _nationality
            End Get
        End Property

        Private _address As String = String.Empty
        Public ReadOnly Property Address() As String
            Get
                Return _address
            End Get
        End Property

        Private _ward As String = String.Empty
        Public ReadOnly Property Ward() As String
            Get
                Return _ward
            End Get
        End Property

        Private _district As String = String.Empty
        Public ReadOnly Property District() As String
            Get
                Return _district
            End Get
        End Property

        Private _city As String = String.Empty
        Public ReadOnly Property City() As String
            Get
                Return _city
            End Get
        End Property

        Private _workplace As String = String.Empty
        Public ReadOnly Property Workplace() As String
            Get
                Return _workplace
            End Get
        End Property

        Private _phone As String = String.Empty
        Public ReadOnly Property Phone() As String
            Get
                Return _phone
            End Get
        End Property

        Private _email As String = String.Empty

        Public ReadOnly Property Email() As String
            Get
                Return _email
            End Get
        End Property

        Private _emergencyContact As String = String.Empty
        Public ReadOnly Property EmergencyContact() As String
            Get
                Return _emergencyContact
            End Get
        End Property

        Private _emergencyContactName As String = String.Empty
        Public ReadOnly Property EmergencyContactName() As String
            Get
                Return _emergencyContactName
            End Get
        End Property

        Private _emergencyContactPhone As String = String.Empty
        Public ReadOnly Property EmergencyContactPhone() As String
            Get
                Return _emergencyContactPhone
            End Get
        End Property

        Private _patientType As String = String.Empty
        Public ReadOnly Property PatientType() As String
            Get
                Return _patientType
            End Get
        End Property

        Private _healthInsuranceNo As String = String.Empty
        Public ReadOnly Property HealthInsuranceNo() As String
            Get
                Return _healthInsuranceNo
            End Get
        End Property

        Private _validTo As pbs.Helper.SmartInt32 = New pbs.Helper.SmartInt32(0)
        Public ReadOnly Property ValidTo() As String
            Get
                Return _validTo.Text
            End Get
        End Property

        Private _notes As String = String.Empty
        Public ReadOnly Property Notes() As String
            Get
                Return _notes
            End Get
        End Property

        Private _ncPt9 As String = String.Empty
        Public ReadOnly Property NcPt9() As String
            Get
                Return _ncPt9
            End Get
        End Property

        Private _ncPt8 As String = String.Empty
        Public ReadOnly Property NcPt8() As String
            Get
                Return _ncPt8
            End Get
        End Property

        Private _ncPt7 As String = String.Empty
        Public ReadOnly Property NcPt7() As String
            Get
                Return _ncPt7
            End Get
        End Property

        Private _ncPt6 As String = String.Empty
        Public ReadOnly Property NcPt6() As String
            Get
                Return _ncPt6
            End Get
        End Property

        Private _ncPt5 As String = String.Empty
        Public ReadOnly Property NcPt5() As String
            Get
                Return _ncPt5
            End Get
        End Property

        Private _ncPt4 As String = String.Empty
        Public ReadOnly Property NcPt4() As String
            Get
                Return _ncPt4
            End Get
        End Property

        Private _ncPt3 As String = String.Empty
        Public ReadOnly Property NcPt3() As String
            Get
                Return _ncPt3
            End Get
        End Property

        Private _ncPt2 As String = String.Empty
        Public ReadOnly Property NcPt2() As String
            Get
                Return _ncPt2
            End Get
        End Property

        Private _ncPt1 As String = String.Empty
        Public ReadOnly Property NcPt1() As String
            Get
                Return _ncPt1
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
            Return _patientCode
        End Function

        'IComparable
        Public Function CompareTo(ByVal IDObject) As Integer Implements System.IComparable.CompareTo
            Dim ID = IDObject.ToString
            Dim pPatientCode As String = ID.Trim
            If _patientCode.Trim < pPatientCode Then Return -1
            If _patientCode.Trim > pPatientCode Then Return 1
            Return 0
        End Function

        Public ReadOnly Property Code As String Implements IInfo.Code
            Get
                Return _patientCode
            End Get
        End Property

        Public ReadOnly Property LookUp As String Implements IInfo.LookUp
            Get
                Return _patientCode
            End Get
        End Property

        Public ReadOnly Property Description As String Implements IInfo.Description
            Get
                Dim info = pbs.BO.MC.Settings.GetSettings
                If info.NameCombination.Equals("123") Then
                    If String.IsNullOrEmpty(_middleName) Then
                        Return String.Format("{0} {1}", _firstName, _lastName)
                    Else
                        Return String.Format("{0} {1} {2}", _firstName, _middleName, _lastName)
                    End If
                ElseIf info.NameCombination.MatchesRegExp("321") Then
                    If String.IsNullOrEmpty(_middleName) Then
                        Return String.Format("{0} {1}", _lastName, _firstName)
                    Else
                        Return String.Format("{0} {1} {2}", _lastName, _middleName, _firstName)
                    End If
                ElseIf info.NameCombination.MatchesRegExp("213") Then
                    If String.IsNullOrEmpty(_middleName) Then
                        Return String.Format("{0} {1}", _firstName, _lastName)
                    Else
                        Return String.Format("{0} {1} {2}", _middleName, _firstName, _lastName)
                    End If

                ElseIf info.NameCombination.MatchesRegExp("231") Then
                    If String.IsNullOrEmpty(_middleName) Then
                        Return String.Format("{0} {1}", _lastName, _firstName)
                    Else
                        Return String.Format("{0} {1} {2}", _middleName, _lastName, _firstName)
                    End If

                Else
                    If String.IsNullOrEmpty(_middleName) Then
                        Return String.Format("{0} {1}", _firstName, _lastName)
                    Else
                        Return String.Format("{0} {1} {2}", _firstName, _middleName, _lastName)
                    End If

                End If
            End Get
        End Property

        Public Sub InvalidateCache() Implements IInfo.InvalidateCache
            PATIENTInfoList.InvalidateCache()
        End Sub


#End Region 'Business Properties and Methods

#Region " Factory Methods "

        Friend Shared Function GetPATIENTInfo(ByVal dr As SafeDataReader) As PATIENTInfo
            Return New PATIENTInfo(dr)
        End Function

        Friend Shared Function EmptyPATIENTInfo(Optional ByVal pPatientCode As String = "") As PATIENTInfo
            Dim info As PATIENTInfo = New PATIENTInfo
            With info
                ._patientCode = pPatientCode

            End With
            Return info
        End Function



        Private Sub New(ByVal dr As SafeDataReader)
            _patientCode = dr.GetString("PATIENT_CODE").TrimEnd
            _firstName = dr.GetString("FIRST_NAME").TrimEnd
            _middleName = dr.GetString("MIDDLE_NAME").TrimEnd
            _lastName = dr.GetString("LAST_NAME").TrimEnd
            _fullname = dr.GetString("FULLNAME").TrimEnd
            _gender = dr.GetString("GENDER").TrimEnd
            _dob.Text = dr.GetInt32("DOB")
            _job = dr.GetString("JOB").TrimEnd
            _ethnic = dr.GetString("ETHNIC").TrimEnd
            _nationality = dr.GetString("NATIONALITY").TrimEnd
            _address = dr.GetString("ADDRESS").TrimEnd
            _ward = dr.GetString("WARD").TrimEnd
            _district = dr.GetString("DISTRICT").TrimEnd
            _city = dr.GetString("CITY").TrimEnd
            _workplace = dr.GetString("WORKPLACE").TrimEnd
            _phone = dr.GetString("PHONE").TrimEnd
            _email = dr.GetString("EMAIL").TrimEnd
            _emergencyContact = dr.GetString("EMERGENCY_CONTACT").TrimEnd
            _emergencyContactName = dr.GetString("EMERGENCY_CONTACT_NAME").TrimEnd
            _emergencyContactPhone = dr.GetString("EMERGENCY_CONTACT_PHONE").TrimEnd
            _patientType = dr.GetString("PATIENT_TYPE").TrimEnd
            _healthInsuranceNo = dr.GetString("HEALTH_INSURANCE_NO").TrimEnd
            _validTo.Text = dr.GetInt32("VALID_TO")
            _notes = dr.GetString("NOTES").TrimEnd
            _ncPt9 = dr.GetString("NC_Pt9").TrimEnd
            _ncPt8 = dr.GetString("NC_Pt8").TrimEnd
            _ncPt7 = dr.GetString("NC_Pt7").TrimEnd
            _ncPt6 = dr.GetString("NC_Pt6").TrimEnd
            _ncPt5 = dr.GetString("NC_Pt5").TrimEnd
            _ncPt4 = dr.GetString("NC_Pt4").TrimEnd
            _ncPt3 = dr.GetString("NC_Pt3").TrimEnd
            _ncPt2 = dr.GetString("NC_Pt2").TrimEnd
            _ncPt1 = dr.GetString("NC_Pt1").TrimEnd
            _updated.Text = dr.GetInt32("UPDATED")
            _updatedBy = dr.GetString("UPDATED_BY").TrimEnd

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