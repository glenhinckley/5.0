

Namespace EDI


    Public Class EDIFormating
        Public Function Ver(ByVal test As String) As String

            Dim v As String = "24"

            Return v + test



        End Function


        Public Function MakeHuman(ByVal T As String) As String



            T = Trim(T)
            T = Replace(T, "~", "~" + Chr(10), 1)



            Return T

        End Function

        Public Function StripHuman(ByVal T As String) As String


            T = Trim(T)
            T = Replace(T, Chr(10), "", 1)
            T = Replace(T, Chr(13), "", 1)

            Return T

        End Function



    End Class
End Namespace