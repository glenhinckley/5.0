Imports System
Imports System.Text.RegularExpressions

Namespace dbCheckStuff

    Public Class CodeInjection

        Private _KEY_WORDLIST As String = String.Empty


        Public Function Ver(ByVal test As String) As String

            Dim v As String = "24"

            Return v + test



        End Function

        Public ReadOnly Property KEY_WORDLIST() As String
            Get
                Return _KEY_WORDLIST
            End Get

        End Property

        'Defines the set of characters that will be checked.
        'You can add to this list, or remove items from this list, as appropriate for your site
        Public Shared SQLblackList As String() = {"--", ";--", ";", "/*", "*/", "@@", _
                                               "@", "char", "nchar", "varchar", "nvarchar", "alter", _
                                               "begin", "cast", "create", "cursor", "declare", "delete", _
                                               "drop", "exec", "execute", "fetch", "insert", _
                                               "kill", "open", "shutdown", "sys", "sysobjects", "syscolumns", _
                                               "table", "update"}

        Public Shared JAVAblackList As String() = {"--", ";--", ";", "/*", "*/", "@@", _
                                               "@", "char", "nchar", "varchar", "nvarchar", "alter", _
                                               "begin", "cast", "create", "cursor", "declare", "delete", _
                                               "drop", "exec", "execute", "fetch", "insert", _
                                               "kill", "open", "select", "shutdown", "sys", "sysobjects", "syscolumns", _
                                               "table", "update"}

        Public Shared VBCSblackList As String() = {"--", ";--", ";", "/*", "*/", "@@", _
                                       "@", "char", "nchar", "varchar", "nvarchar", "alter", _
                                       "begin", "cast", "create", "cursor", "declare", "delete", _
                                       "drop", "end", "exec", "execute", "fetch", "insert", _
                                       "kill", "open", "select", "shutdown", "sys", "sysobjects", "syscolumns", _
                                       "table", "update"}


        'For each incoming request, check the query-string, form and cookie values for suspicious values.
        '   Private Sub app_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        'Dim Request As HttpRequest = TryCast(sender, HttpApplication).Context.Request

        '   For Each key As String In Request.QueryString
        '     CheckInput(Request.QueryString(key))
        '  Next
        'For Each key As String In Request.Form
        '   CheckInput(Request.Form(key))
        ' Next
        ' For Each key As String In Request.Cookies
        '     CheckInput(Request.Cookies(key).Value)
        ' Next
        ' End Sub
        '
        'The utility method that performs the blacklist comparisons
        'You can change the error handling, and error redirect location to whatever makes sense for your site.
        Public Function CheckInput(ByVal parameter As String) As Integer
            _KEY_WORDLIST = String.Empty

            Dim iReturn As Integer = 0

            For i As Integer = 0 To SQLblackList.Length - 1

                If (parameter.IndexOf(SQLblackList(i), StringComparison.OrdinalIgnoreCase) >= 0) Then

                    _KEY_WORDLIST = _KEY_WORDLIST + "  " + SQLblackList(i)

                    iReturn = 1
                End If
            Next

            Return iReturn

        End Function



    End Class


















End Namespace