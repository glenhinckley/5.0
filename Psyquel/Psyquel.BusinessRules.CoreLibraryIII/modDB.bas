
'-------------------------------------------------------------------------------
'Module Name: modDB
'Author: Dave Richkun
'Date: 11/04/1999
'Description: This module is intended to encapsulate generic database routines
'             that may be used in any data-aware application.
'-------------------------------------------------------------------------------
'Revision History:
'
'-------------------------------------------------------------------------------

Option Explicit

Public Enum DataTypes
   typString = 1
   typNumber = 2
   typDate = 3
End Enum


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
    Do While intPos <> 0
        strCopy = Mid(strCopy, 1, intPos) & "'" & Mid(strCopy, intPos + 1) ' append quote
        intPos = InStr(intPos + 2, strCopy, "'", 1) 'search for another single quote
    End do

        ParseSQL = strCopy 'return parsed string back to calling routine

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

    If IsNull(varValue) Then
        IfNull = strReplacement
    Else
        IfNull = varValue
    End If

End Function





End Function


