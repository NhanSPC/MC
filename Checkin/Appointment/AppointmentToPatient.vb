Imports pbs.BO
Imports pbs.Helper
Imports pbs.Helper.Interfaces

Namespace MC

    Public Class AppointmentToPatient
        Implements IRunable

        Public ReadOnly Property Notes As String Implements IRunable.Notes
            Get '&amp; 
                Return <syntax>
                           This function is used to create Patient record from Appointment.
                           Syntax: pbs.BO.MC.AppointmentToPatient?LineNo=[LineNo]&amp;$show=Y/N
                       </syntax>
            End Get
        End Property

        Public Sub Run(args As Helper.pbsCmdArgs) Implements IRunable.Run

            'get input parameters
            Dim _lineNo = args.GetValueByKey("LineNo", String.Empty).ToInteger
            Dim _show = args.GetValueBySystemKey("$Show", "N").ToBoolean

            Dim _patient = PATIENT.NewPATIENT(String.Empty)
            Dim _appointment = pbs.BO.MC.APPOINTMENTInfoList.GetAPPOINTMENTInfo(_lineNo)

            'check if patientcode is existing. if it's existing, show user.
            If String.IsNullOrEmpty(_appointment.PatientCode) Then
                'if patient code is null then check name.
                'if name = null then exist sub
                If String.IsNullOrEmpty(_appointment.Name) Then
                    ExceptionThower.BusinessRuleStop("You must enter Name or Patient code!")
                Else
                    'create appointment record from current appointment
                    _patient.Fullname = _appointment.Name
                    _patient.Phone = _appointment.Phone
                    _patient.Email = _appointment.Email
                    _patient.Dob = _appointment.Dob
                    _patient.Gender = _appointment.Gender
                    _patient.PatientCode = pbs.BO.SEQ.TakeNumber("PatientCode")
                    _patient.Address = _appointment.Address
                    _patient.Ward = _appointment.Ward
                    _patient.District = _appointment.District
                    _patient.City = _appointment.City

                End If

                'show to user
                If _show Then
                    Dim UIcmd = New pbsCmdArgs("pbs.BO.MC.PATIENT?$action=create")
                    UIcmd._bo = _patient
                    pbs.Helper.UIServices.RunURLService.Run(UIcmd)
                End If
            Else
                Dim UIcmd = New pbsCmdArgs(String.Format("pbs.BO.MC.PATIENT?PatientCode={0}&$action=view", _appointment.PatientCode))
                'UIcmd._bo = _patient
                pbs.Helper.UIServices.RunURLService.Run(UIcmd)
            End If


        End Sub
    End Class
End Namespace