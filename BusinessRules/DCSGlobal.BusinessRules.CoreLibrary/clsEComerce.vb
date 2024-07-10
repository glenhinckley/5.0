Imports System.Text


Namespace CreditCardStuff


    Public Class clsEComerce

        Public Function Ver(ByVal test As String) As String

            Dim v As String = "24"

            Return v + test



        End Function

        Function IsValidCreditCardNumber(ByVal Value As Object, Optional ByVal IsRequired As Boolean = True) As Boolean
            Dim strTemp As String = String.Empty
            Dim intCheckSum As Integer = 0
            Dim blnDoubleFlag As Boolean = False
            Dim intDigit As Integer = 0
            Dim i As Integer = -1

            On Error GoTo ErrorHandler

            IsValidCreditCardNumber = True
            Value = Trim$(Value)

            If IsRequired And Len(Value) = 0 Then
                IsValidCreditCardNumber = False
            End If

            ' If after stripping out non-numerics, there is nothing left,
            '  they entered junk
            For i = 1 To Len(Value)
                If IsNumeric(Mid$(Value, i, 1)) Then strTemp = strTemp & Mid$(Value, i, _
                    1)
            Next
            If IsRequired And Len(strTemp) = 0 Then
                IsValidCreditCardNumber = False
            End If

            'Handle different lengths for different credit card types
            Select Case Mid$(strTemp, 1, 1)
                Case "3"    'Amex
                    If Len(strTemp) <> 15 Then
                        IsValidCreditCardNumber = False
                    Else
                        Value = Mid$(strTemp, 1, 4) & "-" & Mid$(strTemp, 5, _
                            6) & "-" & Mid$(strTemp, 11, 5)
                    End If
                Case "4"    'Visa
                    If Len(strTemp) <> 16 Then
                        IsValidCreditCardNumber = False
                    Else
                        Value = Mid$(strTemp, 1, 4) & "-" & Mid$(strTemp, 5, _
                            4) & "-" & Mid$(strTemp, 9, 4) & "-" & Mid$(strTemp, 13, 4)
                    End If
                Case "5"    'Mastercard
                    If Len(strTemp) <> 16 Then
                        IsValidCreditCardNumber = False
                    Else
                        Value = Mid$(strTemp, 1, 4) & "-" & Mid$(strTemp, 5, _
                            4) & "-" & Mid$(strTemp, 9, 4) & "-" & Mid$(strTemp, 13, 4)
                    End If
                Case Else      'Discover - Dont know rules yet
                    If Len(strTemp) > 20 Then
                        IsValidCreditCardNumber = False
                    End If
            End Select

            'Now check for Check Sum (Mod 10)
            intCheckSum = 0
            ' Start with 0 intCheckSum
            blnDoubleFlag = 0
            ' Start with a non-doubling
            For i = Len(strTemp) To 1 Step -1                   ' Working backwards
                intDigit = Asc(Mid$(strTemp, i, 1))             ' Isolate character
                If intDigit > 47 Then                           ' Skip if not a intDigit
                    If intDigit < 58 Then
                        intDigit = intDigit - 48                ' Remove ASCII bias
                        If blnDoubleFlag Then
                            ' If in the "double-add" phase
                            intDigit = intDigit + intDigit      '   then double first
                            If intDigit > 9 Then
                                intDigit = intDigit - 9         ' Cast nines
                            End If
                        End If
                        blnDoubleFlag = Not blnDoubleFlag       ' Flip doubling flag
                        intCheckSum = intCheckSum + intDigit    ' Add to running sum
                        If intCheckSum > 9 Then                 ' Cast tens
                            intCheckSum = intCheckSum - 10      ' (same as MOD 10 but 
                            ' faster)
                        End If
                    End If
                End If
            Next

            If intCheckSum <> 0 Then                            '  Must sum to zero
                IsValidCreditCardNumber = False
            End If

ExitMe:
            Exit Function
ErrorHandler:

        End Function
    End Class

End Namespace