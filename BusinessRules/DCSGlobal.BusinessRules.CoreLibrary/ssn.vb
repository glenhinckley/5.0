Imports System.Text



Namespace SSNStuff



    Public Class FormatSSN

        Public Function Ver(ByVal test As String) As String

            Dim v As String = "24"

            Return v + test



        End Function


        Public Function FormatSSN(ByVal SSN As String) As String

            Dim s As String


            Dim objSS As New StringHandlingStuff.StringStuff


            s = objSS.Strip(SSN)




            Return ""
        End Function


    End Class


End Namespace
