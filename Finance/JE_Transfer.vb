Imports pbs.Helper.Interfaces
Imports pbs.Helper

Namespace MC
    Partial Class JE
        Implements IXMLRestorable

        Public Function RestoreFromXML(xEle As Xml.Linq.XElement) As Object Implements IXMLRestorable.RestoreFromXML
            Dim dic = New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
            For Each att In xEle.Attributes()
                If Not dic.ContainsKey(att.Name.ToString) Then
                    dic.Add(att.Name.ToString, att.Value.TrimEnd)
                End If
            Next

            Dim obj = JE.NewJE(0)
            BOFactory.ApplyPreset(obj, dic)

            If obj IsNot Nothing AndAlso obj.IsNew Then
                For Each ele In xEle...<bo>
                    Dim newLine = obj.Lines.AddNew
                    pbs.Helper.ApplyPresetFormXml(newLine, ele, True)
                    newLine.MarkAsNewClone()
                Next
            End If

            Return obj
        End Function

    End Class
End Namespace

