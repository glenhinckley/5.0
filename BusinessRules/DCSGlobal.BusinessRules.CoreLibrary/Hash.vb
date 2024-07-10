Namespace HASHStuff

    Public Class Hash


        Public Function Ver(ByVal test As String) As String

            Dim v As String = "24"

            Return v + test



        End Function

        Public Function MakeKey() As String
            Dim s As String
            Dim x As String
            Dim y As String
            Try

                Dim rnd As New Random


                x = rnd.Next(100000, 200000)
                y = rnd.Next(100000, 200000)
                'z = x * y



                s = "0200304973EF3D1DF1CA23DBC" & x & y & Now()
                s = Trim(s)

                s = Replace(s, " ", "")
                s = Replace(s, ".", "")
                s = Replace(s, "/", "")
                s = Replace(s, "\", "")
                s = Replace(s, ":", "")
                s = Replace(s, "-", "")

                Return s
            Catch ex As Exception
                Return "0"
            End Try






        End Function

    End Class
End Namespace
