Imports pbs.BO.DataAnnotations

Namespace MC

    Partial Class PRESCRIPTION

        <ComponentModel.Browsable(False)>
        Public Overrides ReadOnly Property IsDirty As Boolean
            Get
                Return MyBase.IsDirty OrElse Details.IsDirty
            End Get
        End Property

        <ComponentModel.Browsable(False)>
        Public Overrides ReadOnly Property IsValid As Boolean
            Get
                Return MyBase.IsValid AndAlso Details.IsValid
            End Get
        End Property

        Private _details As PrescriptionDetails = Nothing

        <TableRangeInfo()>
        ReadOnly Property Details As PrescriptionDetails
            Get
                If _details Is Nothing Then
                    _details = PrescriptionDetails.NewPrescriptionDetails(Me)
                End If
                Return _details
            End Get
        End Property


    End Class

End Namespace

