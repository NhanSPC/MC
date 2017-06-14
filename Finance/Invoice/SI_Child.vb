Imports pbs.BO.DataAnnotations

Namespace MC

    Partial Class SI



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

        Private _details As SIDs = Nothing

        <TableRangeInfo()>
        ReadOnly Property Details As SIDs
            Get
                If _details Is Nothing Then
                    _details = SIDs.NewSIDS(Me)
                End If
                Return _details
            End Get
        End Property


    End Class

End Namespace
