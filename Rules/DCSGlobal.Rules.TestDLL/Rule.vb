Option Explicit On
Option Compare Text
Option Strict On

Imports System.Text
Imports System.Text.RegularExpressions




Namespace RuleEngine








    Public Class Go






        '        Public Function vb_1009_Pat_Phone_Invalid(ByVal patient_home_phone_number As String) As Int32

        '            Dim r As int32 = -1
        '            'rule_id = 1009
        '            'rule_name = 1009_Pat_Phone_Invalid
        '            'rule_description = PATIENT PHONE # CANNOT HAVE THE SAME DIGIT REPEATED FOR ALL DIGITS AND IT CAN ONLY BE 10 DIGITS AND PATIENT'S HOME PHONE 650-992-4000 IS INVALID




        '            'decs*****************************************************
        '            'decs
        '            'decs*****************************************************
        '            Dim phone As String = String.Empty




        '            'assignments*****************************************************
        '            'assignments
        '            'assignments*****************************************************
        '            phone = patient_home_phone_number




        '            'Begin dims
        '            Dim phone As String = String.Empty
        '            Dim objReg As Regex = New Regex()



        '            'Begin code
        '            Dim phone, objReg
        'phone = trim(#patient_home_phone_number#)
        '            phone = Replace(phone, "(", "")
        '            phone = Replace(phone, ")", "")
        '            phone = Replace(phone, "-", "")
        '            objReg = New RegExp
        '            objReg.pattern = "6509924000|0{10}|1{10}|2{10}|3{10}|4{10}|5{10}|6{10}|7{10}|8{10}|9{10}"
        '            If len(phone) > 0 And (len(phone) <> 10 Or objReg.test(phone)) Then
        '                Return -1009
        '            Else : Return 1
        '            End If

        '            Return r


        '        End Function






        Public Function RunRule(ByVal RulePayload As String) As Integer

            Dim e As Integer = 1
            Dim TargetRule As String = String.Empty

            Dim TargetPayload As String = String.Empty


            TargetRule = ParseDemlimtedString(RulePayload, "~", 1)
            TargetPayload = ParseDemlimtedString(RulePayload, "~", 2)



            Select Case TargetRule

                Case "LNAME"

                    e = LNAME(TargetPayload)


                Case "FNAME"

                    e = FNAME(TargetPayload)

                Case Else

                    e = 999999

            End Select






            Return e
        End Function




        Private Function LNAME(ByVal RulePayload As String) As Integer
            Dim e As Integer = 1

            Dim Pattern As String = "^[^A-Z]"
            Dim objReg As Regex = New Regex(Pattern)
            Dim match As Match = objReg.Match(RulePayload)


            If match.Success Then



            End If

            If System.Text.RegularExpressions.Regex.IsMatch(RulePayload, "^[A-Z]+$") Then

                e = 2
            End If


            ' LName = #patient_last_name#

            'objReg.Matches("sdfsf", 
            'objReg.ignorecase = True

            'If objReg.Test(LNAME) Then e = -1001 Else e = 1



            Return e
        End Function


        Private Function FNAME(ByVal RulePayload As String) As Integer
            Dim e As Integer = 1

            Dim Pattern As String = "^[^A-Z]"
            Dim objReg As Regex = New Regex(Pattern)
            Dim match As Match = objReg.Match(RulePayload)


            If match.Success Then



            End If

            If System.Text.RegularExpressions.Regex.IsMatch(RulePayload, "^[A-Z]+$") Then

                e = 2
            End If


            ' LName = #patient_last_name#

            'objReg.Matches("sdfsf", 
            'objReg.ignorecase = True

            'If objReg.Test(LNAME) Then e = -1001 Else e = 1



            Return e
        End Function




        Private Function ParseDemlimtedString(str As String, Delimter As String, position As Integer) As String
            Dim Tempstr() As String
            Dim r As String = ""

            Try
                If position >= 1 Then
                    Tempstr = Split(str, Delimter)


                    r = Tempstr(position - 1).Trim

                End If
            Catch ex As Exception

                If ex.Message = "Index was outside the bounds of the array." Then

                    r = ""


                End If

            End Try


            Return r

        End Function

    End Class
End Namespace