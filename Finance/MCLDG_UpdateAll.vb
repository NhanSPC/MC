Imports pbs.Helper
Imports pbs.BO.LA

Namespace MC

    Partial Class MCLDG
        Implements Interfaces.ISupportUpdateAll

        Private Function UpdateAll(pFilters As Dictionary(Of String, String), pUpdatingData As Dictionary(Of String, String)) As Integer Implements Interfaces.ISupportUpdateAll.UpdateAll

            Dim UpdateableFields = New String() {"InvoiceBook", "InvoiceDate", "InvoiceInfo", "InvoiceNo", "InvoicePeriod", "InvoiceSerial", _
                                                 "ExtDesc1", "ExtDesc2", "ExtDesc3", "ExtDesc4", "ExtDesc5", _
                                                 "ExtDate1", "ExtDate2", "ExtDate3", "ExtDate4", "ExtDate5", _
                                                 "ExtVal1", "ExtVal1", "ExtVal1", "ExtVal1", "ExtVal1", _
                                                 "Description", _
                                                 "NcSl0", "NcSl1", "NcSl2", "NcSl3", "NcSl4", "NcSl5", "NcSl6", "NcSl7", "NcSl8", _
                                                 "PaymentDate", "PaymentPeriod", "PaymentRef", "PayMethod"}

            Dim updateable = New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
            For Each itm In pUpdatingData
                If UpdateableFields.Contains(itm.Key) Then
                    updateable.Add(itm.Key, itm.Value)
                End If
            Next


            Dim updated As Integer = 0

            Dim ProcessingList = MCLDGInfoList.GetMCLDGInfoList(pFilters)

            If ProcessingList IsNot Nothing AndAlso ProcessingList.Count > 0 Then
                Try
                    Dim ret = New List(Of String)

                    Context.InWorkflowActionMode = True 'ignore locking rule

                    For Each info In ProcessingList
                        Dim obj = MCLDG.GetMCLDG(info.LineNo)

                        BOFactory.ApplyPreset(obj, updateable)

                        If obj.IsSavable Then
                            obj = CType(obj, Csla.Core.ISavable).Save()
                            updated += 1

                            Rules.AlertRule.AlertOnCondition(obj)
                            Rules.TriggerRule.RunTriggerOnUpdate(obj)

                            If AuditTrail.AuditType(obj.GetType.ToString) Then ALOG.Log(obj, "Update")

                            ret.Add(String.Format(ResStr(ResStrConst.Result_Saved), obj.GetType.ToString.Translate))

                        ElseIf Not obj.IsDirty Then
                            ret.Add(ResStr(ResStrConst.NOTDIRTY))
                        ElseIf Not obj.IsValid Then

                            Dim SupportErrorMsg = TryCast(obj, Interfaces.ISupportBrokenRuleMsg)
                            If SupportErrorMsg IsNot Nothing Then
                                ret.Add(SupportErrorMsg.BrokenRulesMsg)
                            Else
                                ret.Add(obj.BrokenRulesCollection.ToString)
                            End If

                        End If
                    Next

                    If Not pFilters.GetSystemKey("$noalert", String.Empty).ToBoolean Then
                        pbs.Helper.UIServices.AlertService.Alert(String.Join(Environment.NewLine, ret.ToArray))
                    End If

                Catch ex As Exception
                Finally
                    Context.InWorkflowActionMode = False
                End Try
            End If

            Return updated
        End Function

    End Class
End Namespace

