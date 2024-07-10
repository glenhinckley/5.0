
Namespace NameParseStuff

    Public Class clsFullNameParse

        Private mstrFirstName As String
        Private mstrLastName As String
        Private mstrFullName As String

        Public Property FirstName() As String
            Get
                Return mstrFirstName
            End Get
            Set(ByVal Value As String)
                mstrFirstName = Value
            End Set
        End Property

        Public Property LastName() As String
            Get
                Return mstrLastName
            End Get
            Set(ByVal Value As String)
                mstrLastName = Value
            End Set
        End Property

        Public Property FullName() As String
            Get
                Return mstrFullName
            End Get
            Set(ByVal Value As String)
                mstrFullName = Value
                SplitNameIntoFirstLast(mstrFullName)
            End Set
        End Property

        Public Function Ver(ByVal test As String) As String

            Dim v As String = "24"

            Return v + test



        End Function


        Private Sub SplitNameIntoFirstLast(ByVal Value As String)
            If Value.Length > 0 Then
                ' First check for a comma to see if they
                ' entered Last, First
                If Value.IndexOf(",") > 0 Then
                    mstrLastName = _
                     Value.Substring(0, Value.IndexOf(",")).Trim()
                    mstrFirstName = _
                     Value.Substring(Value.IndexOf(",") + 1).Trim()
                ElseIf Value.IndexOf(" ") > 0 Then
                    mstrFirstName = _
                     Value.Substring(0, Value.LastIndexOf(" ")).Trim()
                    mstrLastName = _
                     Value.Substring(Value.LastIndexOf(" ") + 1).Trim()
                End If
            End If
        End Sub
    End Class

End Namespace
