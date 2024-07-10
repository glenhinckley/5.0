Option Explicit On
Imports Logging
Namespace dbStuff






    Public Enum DataTypes As Integer
        typString = 1
        typNumber = 2
        typDate = 3
    End Enum

    Public Class db
        ' Logging Class
        '  Private Operation As New LogExecption


        Public Function ParseSQL(ByVal strValue As String) As String


            '-----------------------------------------------------------------------------------
            'Author: Dave Richkun
            'Date: 03/17/1998
            'Description: When inserting or updating text values that contain a single quote
            '             into many database tables. i.e. "Can't do this", if the single quote
            '             is not prefixed with another single quote, the SQL statement will fail.
            '             This procedure searches for single quotes in the strValue parameter
            '             and precedes each quote with an additional quote.  e.g. "Can''t do this"
            'Parameters:  strValue - The string that will be searched for single quotes
            'Returns:     The modified string value formatted with consecutive single quotes
            '             where required.
            '-----------------------------------------------------------------------------------
            'Revision History:
            '



            '-----------------------------------------------------------------------------------

            Dim intPos As Integer
            Dim strCopy As String



            strCopy = strValue 'Copy parameter to a local variable

            intPos = InStr(1, strCopy, "'", 1) 'search for single quote
            While intPos <> 0
                strCopy = Mid(strCopy, 1, intPos) & "'" & Mid(strCopy, intPos + 1) ' append quote
                intPos = InStr(intPos + 2, strCopy, "'", 1) 'search for another single quote
            End While

            ParseSQL = strCopy 'return parsed string back to ing routine

        End Function


        Public Function IfNull(ByVal varValue As Object, ByVal strReplacement As String) As String
            '-----------------------------------------------------------------------------------
            'Author: Dave Richkun
            'Date: 03/17/1998
            'Description: Replaces a NULL value with a string value.
            'Parameters:  varValue - A Object data type that is checked for a value of NULL.
            '             strReplacement - The string that will replace the original value, if the
            '               value contains NULL.
            'Returns:     The strReplacement parameter only if varValue is identified as NULL,
            '             otherwise varValue is returned.
            '-----------------------------------------------------------------------------------
            'Revision History:
            '
            '-----------------------------------------------------------------------------------

            If varValue Is DBNull.Value Then
                IfNull = strReplacement
            Else
                IfNull = varValue
            End If

        End Function


        Public Function GetStringValue(ByVal value As Object) As String



            If value Is DBNull.Value Then
                Return String.Empty
            Else
                Return Convert.ToString(value)
            End If




        End Function


        Public Function IfNull2(ByVal varValue As Object, ByVal strReplacement As String, Optional ByVal lngDataType As DataTypes = DataTypes.typString) As String
            '-----------------------------------------------------------------------------------
            'Author: Dave Richkun
            'Date: 02/01/2000
            'Description: Replaces a NULL value with a string value.  This function was designed
            '               specifiy to accomodate the various challenges dealing with data
            '               conversion from the Psyquel MS-Access database to the SQL Server
            '               database.
            'Parameters:  varValue - A Object data type that is checked for a value of NULL.
            '             strReplacement - The string that will replace the original value, if the
            '               value contains NULL.
            '             lngDataType - Identifies the data type of the value being checked.  This
            '               parameter determines if values are to be enclosed within single quotes.
            'Returns:     The strReplacement parameter only if varValue is identified as NULL,
            '             otherwise varValue is returned.
            '-----------------------------------------------------------------------------------
            'Revision History:
            '
            '-----------------------------------------------------------------------------------

            Dim DT As Boolean = False
            Dim r As String = String.Empty


            'typString = 1
            'typNumber = 2
            'typDate = 3

            Select Case lngDataType   ' Must be a primitive data type
                Case 1    'typString = 1

                    If varValue Is DBNull.Value Or varValue.ToString = String.Empty Then
                        r = strReplacement

                    End If


                Case 2  'typNumber = 2

                    If varValue = 0 Then
                        r = strReplacement
                    End If

                    'If varValue Is DBNull.Value Or varValue.ToString Then
                    '    IfNull2 = strReplacement
                    'Else
                    '    If lngDataType = DataTypes.typDate Or lngDataType = DataTypes.typString Then
                    '        IfNull2 = "'" & ParseSQL(varValue) & "'"
                    '    Else
                    '        IfNull2 = varValue
                    '    End If
                    'End If






                Case 3 'typDate = 3



                    If varValue < "01/01/1753" Then 'Enforce SmallDateTime data type restriction
                        r = strReplacement
                    End If



            End Select



            Return r


        End Function


    End Class


End Namespace



