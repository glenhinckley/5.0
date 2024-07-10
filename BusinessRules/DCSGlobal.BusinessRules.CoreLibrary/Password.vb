Imports System.Text

Namespace PasswordStuff

    Public Class PasswordGenerator
        Public Function Ver(ByVal test As String) As String

            Dim v As String = "24"

            Return v + test



        End Function
        Public Function GeneratePassword(ByVal PasswordLength As Integer) As String
            Dim Vowels() As Char = New Char() {"a", "e", "i", "o", "u"}
            Dim Consonants() As Char = New Char() {"b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "r", "s", "t", "v"}
            Dim DoubleConsonants() As Char = New Char() {"c", "d", "f", "g", "l", "m", "n", "p", "r", "s", "t"}

            Dim wroteConsonant As Boolean  'boolean
            Dim counter As Integer

            Dim rnd As New Random
            Dim passwordBuffer As New StringBuilder

            wroteConsonant = False
            For counter = 0 To PasswordLength
                If passwordBuffer.Length > 0 And (wroteConsonant = False) And (rnd.Next(100) < 10) Then
                    passwordBuffer.Append(DoubleConsonants(rnd.Next(DoubleConsonants.Length)), 2)
                    counter += 1
                    wroteConsonant = True
                Else
                    If (wroteConsonant = False) And (rnd.Next(100) < 90) Then
                        passwordBuffer.Append(Consonants(rnd.Next(Consonants.Length)))
                        wroteConsonant = True
                    Else
                        passwordBuffer.Append(Vowels(rnd.Next(Vowels.Length)))
                        wroteConsonant = False
                    End If
                End If
            Next

            'size the buffer
            passwordBuffer.Length = PasswordLength
            Return passwordBuffer.ToString
        End Function
    End Class

End Namespace
