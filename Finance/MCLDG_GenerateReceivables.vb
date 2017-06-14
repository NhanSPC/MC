Imports pbs.Helper
Imports System.Data

Imports Csla
Imports Csla.Data
Imports Csla.Validation

Namespace MC

    Partial Public Class MCLDG

        Friend _offsetLineNo As Integer

        Friend Sub OffsetAllocationIfNeeded()
            Try
                If _offsetLineNo > 0 Then
                    Dim dic = New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
                    dic.Add("LineNo", String.Format("<<{0},{1}", LineNo, _offsetLineNo))
                    Dim theList = MCLDGInfoList.GetMCLDGInfoList(dic)
                    MCLDGInfoList.AllocationMatching(theList.ToList)
                End If
            Catch ex As Exception
            End Try
        End Sub

        ''' <summary>
        ''' Reverse the sign o transaction. Create offset line for Corporate allocation, for example
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ReverseMe()
            _transAmt.Float = _transAmt.Float * -1
            _amount.Float = _amount.Float * -1
            _quantity.Float = _quantity.Float * -1
            _vatAmount.Float = _vatAmount.Float * -1
            Normalize_DC()
        End Sub

        Private Sub Normalize_DC()

            If _amount.Float > 0 Then
                _transAmt.Float = Math.Abs(_transAmt.Float)
                _quantity.Float = Math.Abs(_quantity.Float)
                _vatAmount.Float = Math.Abs(_vatAmount.Float)
                _dC = "C"
            ElseIf _amount.Float < 0 Then
                _transAmt.Float = Decimal.Negate(Math.Abs(_transAmt.Float))
                _quantity.Float = Decimal.Negate(Math.Abs(_quantity.Float))
                _vatAmount.Float = Decimal.Negate(Math.Abs(_vatAmount.Float))
                _dC = "D"
            Else
                If _transAmt.Float > 0 Then
                    _dC = "C"
                ElseIf _transAmt.Float < 0 Then
                    _dC = "D"
                Else 'other amout is zero. the sign of amount is used
                    If _quantity.Float <= 0 Then
                        _dC = "D"
                    Else
                        _dC = "C"
                    End If
                End If
            End If

        End Sub

        Friend Sub SetDC()
            Dim newD_C = _dC

            Dim isDiscountLine As Boolean

            If (Me._quantity = 0 AndAlso _amount.Float < 0) _
                OrElse ItemInfo.StdCost < 0 Then isDiscountLine = True

            Normalize_DC()

            If isDiscountLine Then
                ReverseMe()
            Else
                If Not String.IsNullOrEmpty(newD_C) Then
                    If newD_C.MatchesRegExp("^[C,D,c,d,R,r]$") AndAlso Not _dC.Equals(newD_C, StringComparison.OrdinalIgnoreCase) Then
                        ReverseMe()
                    End If
                End If
            End If


        End Sub


        ''' <summary>
        ''' pList is the spliting list, which user confirm via split editor 
        ''' This sub will confirm spliting before posting
        ''' </summary>
        ''' <param name="pList"></param>
        ''' <remarks></remarks>
        Public Sub SplitByAmountTo(pList As IEnumerable(Of MCLDG))
            If pList Is Nothing OrElse pList.Count < 1 Then Exit Sub
            For Each line In pList
                If line.LineNo <> Me.LineNo Then
                    ExceptionThower.BusinessRuleStop("Splitted list does not come from the LineNo: {0}", Me.LineNo)
                End If
            Next

            Dim CheckSum = Aggregate val In pList Into Sum(Math.Abs(val._amount.Float))

            If Math.Abs(Me._amount.Float) = CheckSum Then

                Dim qty As Decimal = 0
                Dim other As Decimal = 0
                Dim amt As Decimal = 0

                For idx = 0 To pList.Count - 1
                    pList(idx).AdjustOtherValuesByAmount(CheckSum)
                    qty += pList(idx)._quantity.Float
                    other += pList(idx)._transAmt.Float
                    amt += pList(idx)._amount.Float
                Next

                'round up the last line of splitting list
                pList(pList.Count - 1)._quantity.Float += (qty - Me._quantity.Float)
                pList(pList.Count - 1)._transAmt.Float += (other - Me._transAmt.Float)
                pList(pList.Count - 1)._amount.Float += (amt - Me._amount.Float)

                'Posting
                pList(0).MarkDirty()
                pList(0)._extDesc5 = String.Format("SplittedFrom_{0}", LineNo)
                pList(0).Save()

                For idx = 1 To pList.Count - 1
                    pList(idx)._lineNo = 0
                    pList(idx).MarkNew()
                    pList(idx)._status = String.Format("SPL_{0}", LineNo)
                    pList(idx).Save()
                Next

            End If

        End Sub

        Private Sub AdjustOtherValuesByAmount(pTotal As Decimal)
            If pTotal = 0 Then Exit Sub

            Dim ratio = Math.Abs(Me._amount.Float / pTotal)
            Me._quantity.Float = Me._quantity.Float * ratio
            Me._transAmt.Float = Me._transAmt.Float * ratio
            Me._vatAmount.Float = Me._vatAmount.Float * ratio
            Me._extVal5.Float = pTotal

        End Sub

    End Class

End Namespace