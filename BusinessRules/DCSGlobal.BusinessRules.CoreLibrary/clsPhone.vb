Imports System
Imports System.Text.RegularExpressions


Namespace PhoneStuff

    Public Class clsPhone

        Private LineEX As String
        Private NPAEX As String
        Private NXXEX As String
        Private ANIEX As String
        Private StripedLineEX As String
        Private FormatedLineEX As String

        Private Sub StringPrep()

            StripLine()
            FormatLine()

        End Sub

        Public Function Ver(ByVal test As String) As String

            Dim v As String = "24"

            Return v + test



        End Function


        Sub ValidatePhone()
            Dim input As String = ""
            Dim pattern As String = "(\d\d\d-){1,2}\d\d\d\d"


            While Not input.ToUpper() = "Q"
                Console.WriteLine("Enter a string, or Q to exit.")
                input = Console.ReadLine()
                If Regex.IsMatch(input, pattern) Then
                    'Console.WriteLine("String contains a valid phone number")
                Else
                    'Console.WriteLine("String does NOT contain a valid phone number")
                End If
            End While
        End Sub







        Private Sub FormatLine()



            If Len(LineEX) = 10 Then


                NPAEX = Mid(LineEX, 1, 3)
                NXXEX = Mid(LineEX, 4, 3)
                ANIEX = Mid(LineEX, 7, 4)

                FormatedLineEX = "(" & NPAEX & ") "
                FormatedLineEX = FormatedLineEX & NXXEX & "-"
                FormatedLineEX = FormatedLineEX & ANIEX

            End If


        End Sub

        Private Sub StripLine()


            LineEX = Trim(LineEX)
            LineEX = Replace(LineEX, " ", "", 1)
            LineEX = Replace(LineEX, "'", "", 1)
            LineEX = Replace(LineEX, ",", "", 1)
            LineEX = Replace(LineEX, "&", "and", 1)
            LineEX = Replace(LineEX, Chr(34), "", 1)
            LineEX = Replace(LineEX, "-", "", 1)
            LineEX = Replace(LineEX, "(", "", 1)
            LineEX = Replace(LineEX, ")", "", 1)

        End Sub


        Public Function StripLineEX(ByVal Line As String) As String

            LineEX = Line
            StripLine()

            Return LineEX

        End Function


        Public WriteOnly Property PhoneNumber() As String

            Set(ByVal Value As String)
                LineEX = Value
                StringPrep()

            End Set
        End Property

        Public ReadOnly Property FormatedNumber() As String
            Get
                Return FormatedLineEX
            End Get

        End Property



        Public ReadOnly Property StripedNumber() As String
            Get
                Return LineEX
            End Get

        End Property

        Public ReadOnly Property NPA() As String
            Get
                Return NPAEX
            End Get

        End Property

        Public ReadOnly Property NXX() As String
            Get
                Return NXXEX
            End Get

        End Property

        Public ReadOnly Property ANI() As String
            Get
                Return ANIEX
            End Get

        End Property



    End Class

End Namespace
