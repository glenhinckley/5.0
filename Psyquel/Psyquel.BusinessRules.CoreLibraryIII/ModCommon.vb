

Namespace dbStuff






    Public Class ModCommon

        Public Enum DataTypes
            typString = 1
            typNumber = 2
            typDate = 3
        End Enum

        Public Function ParseTrim(ByRef Source As String, ByVal Delim As Integer) As String
            '------------------------------------------------------------
            'Author: Rick "Boom boom" Segura, supplied by Ken White     '
            'Date: 03/08/2000                                           '
            'Description: Returns the substring from the the begining of'
            '             to the first insyance of the delimeter and    '
            '             removes the token plus the 1st delimeter from '
            '             the original string                           '
            'Returns: Substring                                         '
            '------------------------------------------------------------
            'Revision History:                                          '
            '                                                           '
            '------------------------------------------------------------

            Dim d As String
            Dim Dlnxt As Integer
            Dim Slen As Integer

            d = Chr(Delim)

            Slen = Len(Source)
            Dlnxt = InStr(Source, d)

            Select Case Dlnxt
                Case 0
                    ParseTrim = Source
                    Source = ""
                Case 1
                    ParseTrim = ""
                    If Slen >= 1 Then
                        ParseTrim = ""
                        Source = Trim(Right(Source, Slen - 1))
                    End If
                Case Slen
                    If Slen > 1 Then
                        ParseTrim = Trim(Left(Source, Slen - 1))
                    Else
                        ParseTrim = ""
                    End If
                    Source = ""
                Case Else
                    ParseTrim = Trim(Left(Source, Dlnxt - 1))
                    Source = Right(Source, Slen - Dlnxt)
            End Select

        End Function





        Public Function IfNull2(ByVal varValue As Object, ByVal strReplacement As String, Optional ByVal lngDataType As Long = 0) As String
            '-----------------------------------------------------------------------------------
            'Author: Dave Richkun
            'Date: 02/01/2000
            'Description: Replaces a NULL value with a string value.  This function was designed
            '               specifically to accomodate the various challenges dealing with data
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
            Dim _db As New db

            Dim _e As New DataTypes

            If lngDataType = 3 Then
                If varValue = 0 Then
                    IfNull2 = strReplacement
                    Exit Function
                ElseIf varValue < "01/01/1753" Then 'Enforce SmallDateTime data type restriction
                    IfNull2 = strReplacement
                    Exit Function
                End If
            End If

            If varValue.IsNullorEmpty() Then
                IfNull2 = strReplacement
            Else
                If lngDataType = 3 Or lngDataType = 1 Then
                    IfNull2 = "'" & _db.ParseSQL(varValue) & "'"
                Else
                    IfNull2 = varValue
                End If
            End If
        End Function

        Public Function FormatPhoneNumber(ByVal varPhoneNum As Object) As Object
            '-----------------------------------------------------------------------------------
            'Author: Dave Richkun
            'Date: 12/18/1997
            'Description: This procedure formats a string representing a phone number
            '             based on the number of characters passed to it.
            'Parameters: strPhoneNum - The string value representing the phone number to be
            '             formatted.
            'Returns: Null
            '-----------------------------------------------------------------------------------
            'Revision History:
            '
            '-----------------------------------------------------------------------------------

            Dim strString As String
            Dim intLength As Integer
            Dim strPhone As String
            Dim intCTR As Integer
            Dim strTest As String
            Dim strPhoneNum As String

            'varValue Is DBNull.Value
            If varPhoneNum Is DBNull.Value Then
                FormatPhoneNumber = ""
                Exit Function
            Else
                strPhoneNum = CStr(varPhoneNum)
            End If

            intLength = Len(strPhoneNum)

            For intCTR = 1 To intLength
                If IsNumeric(Mid(strPhoneNum, intCTR)) Then
                    strString = strString & Mid(strPhoneNum, intCTR, 1)
                End If
            Next intCTR

            intLength = Len(strString)

            'Prevent phone numbers consisting of all zeroes from making it through.
            For intCTR = 1 To intLength
                strTest = strTest & "0"
            Next intCTR
            If strString = strTest Then
                FormatPhoneNumber = ""
                Exit Function
            End If

            Select Case intLength
                Case 10
                    strPhone = "(" & Mid(strString, 1, 3) & ") " & Mid(strString, 4, 3) & "-" & Mid(strString, 7)
                Case 7
                    strPhone = Mid(strString, 1, 3) & "-" & Mid(strString, 4)
                Case Is > 10
                    strPhone = Mid(strString, 1, intLength - 10) & " " & Mid(strString, intLength - 9, 3) & " " & Mid(strString, intLength - 6, 3) & "-" & Mid(strString, intLength - 3)
                Case Else
                    strPhone = strString
            End Select

            FormatPhoneNumber = strPhone

        End Function

        Public Function FormatZipCode(ByVal varZipCode As Object) As Object
            '-----------------------------------------------------------------------------------
            'Author: Eric Pena
            'Date: 4/28/2000
            'Description: This procedure formats a string representing a zip code
            '             based on the number of characters passed to it.
            'Parameters: strZipCode - The string value representing the zip code to be
            '             formatted.
            'Returns: Formatted zip code
            '-----------------------------------------------------------------------------------
            'Revision History:
            '
            '-----------------------------------------------------------------------------------

            Dim strString As String
            Dim intLength As Integer
            Dim strZip As String
            Dim intCTR As Integer
            Dim strTest As String
            Dim strZipCode As String

            If varZipCode Is DBNull.Value Then
                FormatZipCode = ""
                Exit Function
            Else
                strZipCode = Trim(CStr(varZipCode))
            End If

            intLength = Len(strZipCode)
            strString = strZipCode

            'Get rid of non-numeric chars
            For intCTR = 1 To intLength
                If Not IsNumeric(Mid(strZipCode, intCTR)) Then
                    strString = Replace(strZipCode, Mid(strZipCode, intCTR), "")
                End If
            Next intCTR

            intLength = Len(strString)

            'Prevent zip codes consisting of all zeroes or nines from making it through.
            For intCTR = 1 To intLength
                strTest = strTest & "0"
            Next intCTR

            If strString = strTest Then
                FormatZipCode = ""
                Exit Function
            End If

            For intCTR = 1 To intLength
                strTest = strTest & "9"
            Next intCTR

            If strString = strTest Then
                FormatZipCode = ""
                Exit Function
            End If

            Select Case intLength
                Case Is > 5
                    strZip = Left(strString, 5) & "-" & Mid(strString, 6, Len(strString) - 5)
                Case Else
                    strZip = strString
            End Select

            FormatZipCode = strZip

        End Function


        Public Sub ForceUpperCase(ByRef intKeyPress As Integer)
            '-----------------------------------------------------------------------------------
            'Author: Dave Richkun
            'Date: 12/18/1997
            'Description: This procedure forces the passed KeyCode characer to uppercase.
            'Parameters: intKeyPress - Key code representing the character to be forced to
            '               uppercase.
            'Returns: Null
            '-----------------------------------------------------------------------------------
            'Revision History:
            '
            '-----------------------------------------------------------------------------------

            intKeyPress = Asc(UCase(Chr(intKeyPress)))

        End Sub


        Public Function FormatSSN(ByVal varSSN As String) As Object
            '-----------------------------------------------------------------------------------
            'Author: Dave Richkun
            'Date: 02/01/2000
            'Description: This procedure formats a string representing a social security number
            '             (SSN) based on the number of characters passed to it.
            'Parameters: strSSN - The string value representing the SSN to be
            '             formatted.
            'Returns: Null
            '-----------------------------------------------------------------------------------
            'Revision History:
            '
            '-----------------------------------------------------------------------------------

            Dim strString As String
            Dim intLength As Integer
            Dim strSSNumber As String
            Dim strSSN As String


            If IsDBNull(varSSN) Then

                FormatSSN = ""
                Exit Function
            Else
                strSSN = Trim(CStr(varSSN))
            End If

            strString = NumbersOnly(strSSN)
            intLength = Len(strString)

            Select Case intLength
                Case 11
                    'Remove preceding 2 digits - for military status only - no longer used.
                    strSSNumber = Mid(strString, 3, 3) & "-" & Mid(strString, 6, 2) & "-" & Mid(strString, 8)
                Case 9
                    strSSNumber = Mid(strString, 1, 3) & "-" & Mid(strString, 4, 2) & "-" & Mid(strString, 6)
                Case Else
                    strSSNumber = strString
            End Select

            FormatSSN = strSSNumber

        End Function



        Public Function FormatCC(ByVal varCC As String) As Object
            '-----------------------------------------------------------------------------------
            'Author: Duane C Orth
            'Date: 06/01/2018
            'Description: This procedure formats a string representing a Credit Card number
            '             (CC) based on the number of characters passed to it.
            'Parameters: strCC - The string value representing the CC to be
            '             formatted.
            'Returns: Null
            '-----------------------------------------------------------------------------------
            'Revision History:
            '
            '-----------------------------------------------------------------------------------

            Dim strString As String
            Dim intLength As Integer
            Dim strCCNumber As String
            Dim strCC As String


            If IsDBNull(varCC) Then
                FormatCC = ""
                Exit Function
            Else
                strCC = Trim(CStr(varCC))
            End If

            strString = NumbersOnly(strCC)
            intLength = Len(strString)

            Select Case intLength
                Case 14
                    'Remove preceding 2 digits - for military status only - no longer used.
                    strCCNumber = "****" & " " & "****" & " " & "****" & " " & " " & Mid(strString, 13, 2)
                Case 15
                    'Remove preceding 2 digits - for military status only - no longer used.
                    strCCNumber = "****" & " " & "****" & " " & "****" & " " & " " & Mid(strString, 13, 3)
                Case 16
                    '      strCCNumber = Mid(strString, 1, 4) & " " & Mid(strString, 5, 4) & " " & Mid(strString, 9, 4) & " " & Mid(strString, 13, 4)
                    strCCNumber = "****" & " " & "****" & " " & "****" & " " & Mid(strString, 13, 4)
                Case Else
                    strCCNumber = strString
            End Select

            FormatCC = strCCNumber

        End Function


        Public Function NumbersOnly(ByVal varValue As Object) As Object
            '-------------------------------------------------------------------------------
            'Author: Dave Richkun
            'Date: 04/19/2000
            'Description: This procedure parses a Object parameter and returns only
            '             the numeric characters to the ing procedure.
            'Parameters: varValue - The Object parameter to be parsed.
            'Returns: The parsed value containing only the numeric characters
            '-------------------------------------------------------------------------------
            'Revision History:
            '
            '-------------------------------------------------------------------------------

            Dim intCTR As Integer
            Dim strValue As String
            Dim intLength As Integer
            Dim strNumber As String
            Try


                'If varValue.IsNullorEmpty Then
                '    NumbersOnly = ""
                '    Exit Function
                'End If

                strValue = CStr(varValue)
                intLength = Len(strValue)

                For intCTR = 1 To intLength
                    If IsNumeric(Mid(strValue, intCTR, 1)) Then
                        strNumber = strNumber & Mid(strValue, intCTR, 1)
                    End If
                Next intCTR

            Catch ex As Exception



            End Try


            NumbersOnly = strNumber



        End Function

        Public Function IsAnArray(ByVal varAry As Object) As Boolean
            '--------------------------------------------------------------------------------
            'Author: Rick "Boom Boom" Segura                                                '
            'Date: 05/18/2000                                                               '
            'Description: This procedure determines wether or not the passed argument is an '
            '             array.  Created  because VB did a piss poor job on IsArray().     '
            'Parameters: varAry - Object to be checked                                     '
            'Returns: True if passed argument is an array, false otherwise                  '
            '--------------------------------------------------------------------------------
            'Revision History:                                                              '
            '                                                                               '
            '--------------------------------------------------------------------------------

            ' Assume parameter is not an array
            IsAnArray = False

            If varAry.IsNullorEmpty Then
                Exit Function
            End If

            If TypeName(varAry) = "Nothing" Then
                Exit Function
            End If

            If varAry.IsNullorEmpty Then
                Exit Function
            End If


            If IsArray(varAry) Then IsAnArray = True


        End Function


        Public Function NNs(ByVal varString As Object) As String
            '--------------------------------------------------------------------------------
            'Author: Rick "Boom Boom" Segura                                                '
            'Date: 01/25/2001                                                               '
            'Description: Ensures that a value is Not Null                                  '
            'Parameters: A non-Null string value                                            '
            '--------------------------------------------------------------------------------
            'Revision History:                                                              '
            '                                                                               '
            '--------------------------------------------------------------------------------
            If IsDBNull(varString) Then
                NNs = ""
            Else
                NNs = CStr(varString)
            End If
        End Function


        Public Function RPad(ByVal strString As String, ByVal intPad As Integer) As String
            '--------------------------------------------------------------------------------
            'Author: Dave Richkun
            'Date: 09/17/2001
            'Description: Pads space characters to the end of a string
            'Parameters: strString - The original string
            '            intPad - The number of space characters to append to the string
            'Returns: The padded string
            '--------------------------------------------------------------------------------
            'Revision History:                                                              '
            '                                                                               '
            '--------------------------------------------------------------------------------
            Dim strCopy As String
            Dim intCTR As Integer

            strCopy = strString

            For intCTR = 1 To intPad
                strCopy = strCopy & Chr(32)
            Next intCTR

            'Return the padded string
            RPad = strCopy

        End Function
        Public Function ParamType(ByVal strReportName As String, ByVal lngParamNum As Long) As String
            '--------------------------------------------------------------------------------
            'Author: Eric Pena
            'Date: 10/04/2001
            'Description: Returns a datatype of a specific paramater in a crystal report
            'Parameters: strReportName - name of crystal report
            '            lngParamNum - Parameter number to check
            'Returns: datatype name (string)
            '--------------------------------------------------------------------------------
            'Revision History:                                                              '
            '                                                                               '
            '--------------------------------------------------------------------------------


            Dim r As String = String.Empty



            Select Case strReportName
                Case "rptProviderDenial", "rptOutstandingPatAcct", "rptPatientInvoice", "rptProgressNote", "rptMisdirectedPmtRejected", "rptNewProviderStats", "rptAgedClaims", "rptHCFA", "rptUB04"
                    r = "Long"

                Case "rptBookClosing", "rptProviderAR", "rptCommission", "rptCollectOrigins", "rptCollectionsHistogram", "rptProjectedRevenue", "rptClaimCount", "rptEmployeeStats", "rptEmployeeStatsDetail", "rptMPPostings"
                    ParamType = "Date"

                Case "rptSuperbill", "rptPayerSummary", "rptARAgingProviders"
                    If lngParamNum = 1 Then ParamType = "Date" Else ParamType = "Long"

                Case "rptCPCSummary", "rptPostingHistory", "rptProviderIncome", "rptBillingAccountDetail", "rptPatientPaymentLog", "rptPayerSessions", "rptSvcGrpPostings", "rptDenialLog", "rptWriteoffLog"
                    If lngParamNum < 3 Then ParamType = "Date" Else ParamType = "Long"

                Case "rptHCFAReprint"
                    If lngParamNum > 1 Then ParamType = "Date" Else ParamType = "Long"

                Case "rptOutInsSummary"
                    If lngParamNum = 3 Or lngParamNum = 4 Or lngParamNum = 7 Then ParamType = "Long" Else ParamType = "Date"

                Case "rptBilledContacts"
                    If lngParamNum < 3 Then
                        ParamType = "Date"
                    ElseIf lngParamNum = 4 Then
                        ParamType = "String"
                    Else
                        ParamType = "Long"
                    End If

                Case "rptARAgingSummary"
                    If lngParamNum = 2 Then
                        ParamType = "Date"
                    Else
                        ParamType = "Long"
                    End If

                Case "rptARPatientAgingDetail"
                    If lngParamNum <= 4 Then
                        ParamType = "Long"
                    Else
                        ParamType = "Date"
                    End If
            End Select





            Return r



        End Function
        Public Function getNextParam(Optional ByVal strRptParams As String = "", Optional ByVal blnReset As Boolean = False) As String
            '-------------------------------------------------------------------------------
            'Author: Eric Pena
            'Date: 05/14/2001
            'Description: This procedure returns the next paramater in the given delimited string
            'Parameters: strRptParams - the delimited string to use (if blnReset is set)
            '               blnReset - true to reset static parameter string, false otherwise
            'Returns: The next parameter
            '-------------------------------------------------------------------------------
            'Revision History:
            '-------------------------------------------------------------------------------
            Static strParams As String
            Dim lngLastPos As Long

            If blnReset Then strParams = strRptParams

            lngLastPos = InStr(strParams, ";")
            If lngLastPos < 1 Then
                If InStr(2, strParams, ";") > 0 Then
                    'there are more params, this one is ''
                    getNextParam = ""
                Else
                    getNextParam = strParams
                    strParams = ""
                End If
            Else
                getNextParam = Left(strParams, lngLastPos - 1)
            End If
            strParams = Right(strParams, Len(strParams) - lngLastPos)
        End Function


        Public Function Max(ByVal lngVal1 As Long, ByVal lngVal2 As Long) As Long
            '--------------------------------------------------------------------------------
            'Author: Rick "Boom Boom" Segura                                                '
            'Date: 01/25/2001                                                               '
            'Description: Calculates the maximum of 2 numbers                               '
            'Parameters: lngVal1 - 1st value of comparison pair                             '
            '            lngVal2 - 2nd value of comparison pair                             '
            'Returns: The maximum of the pair                                               '
            '--------------------------------------------------------------------------------
            'Revision History:                                                              '
            '                                                                               '
            '--------------------------------------------------------------------------------
            Max = IIf((lngVal2 > lngVal1), lngVal2, lngVal1)
        End Function

        Public Function Min(ByVal lngVal1 As Long, ByVal lngVal2 As Long) As Long
            '--------------------------------------------------------------------------------
            'Author: Rick "Boom Boom" Segura                                                '
            'Date: 01/25/2001                                                               '
            'Description: Calculates the minimum of 2 numbers                               '
            'Parameters: lngVal1 - 1st value of comparison pair                             '
            '            lngVal2 - 2nd value of comparison pair                             '
            'Returns: The minimum of the pair                                               '
            '--------------------------------------------------------------------------------
            'Revision History:                                                              '
            '                                                                               '
            '--------------------------------------------------------------------------------
            Min = IIf((lngVal2 > lngVal1), lngVal1, lngVal2)
        End Function

        Public Sub StrCat(ByRef strMain As String, ByVal strSub As String)
            '--------------------------------------------------------------------------------
            'Author: Rick "Boom Boom" Segura                                                '
            'Date: 01/25/2001                                                               '
            'Description: Concetenates two strings                                          '
            'Parameters: strMain - string to perform concatenation on                       '
            '            strSub -  string to concatenate                                    '
            '--------------------------------------------------------------------------------
            'Revision History:                                                              '
            '                                                                               '
            '--------------------------------------------------------------------------------
            If strMain > "" Then
                strMain = strMain & strSub
            Else
                strMain = strSub
            End If
        End Sub


        Public Sub QuickSort(ByRef varArray As Object, Optional ByVal lngFirst As Long = -1, _
                     Optional ByVal lngLast As Long = -1)
            '--------------------------------------------------------------------
            'Date: 10/23/2000
            'Author: Dave Richkun
            'Description: QuickSort algorithm used to sort the items in the varArray array.
            'Parameters: varArray - The array to be sorted
            '            lngFirst - Optional value identifying the first element to begin sorting with
            '            lngLast - Optional value identifying the last element to begin sorting with
            'Returns: The sorted array by reference parameter
            '--------------------------------------------------------------------

            Dim lngLow As Long
            Dim lngHigh As Long
            Dim lngMiddle As Long
            Dim varTempVal As Object
            Dim varTestVal As Object

            If lngFirst = -1 Then lngFirst = LBound(varArray)
            If lngLast = -1 Then lngLast = UBound(varArray)

            If lngFirst < lngLast Then
                lngMiddle = (lngFirst + lngLast) / 2
                varTestVal = varArray(lngMiddle)
                lngLow = lngFirst
                lngHigh = lngLast
                Do
                    Do While varArray(lngLow) < varTestVal
                        lngLow = lngLow + 1
                    Loop
                    Do While varArray(lngHigh) > varTestVal
                        lngHigh = lngHigh - 1
                    Loop
                    If (lngLow <= lngHigh) Then
                        varTempVal = varArray(lngLow)
                        varArray(lngLow) = varArray(lngHigh)
                        varArray(lngHigh) = varTempVal
                        lngLow = lngLow + 1
                        lngHigh = lngHigh - 1
                    End If
                Loop While (lngLow <= lngHigh)

                If lngFirst < lngHigh Then QuickSort(varArray, lngFirst, lngHigh)
                If lngLow < lngLast Then QuickSort(varArray, lngLow, lngLast)
            End If

        End Sub
        Public Sub QuickSort2D(ByRef varArray As Object, ByVal lng2DWidth As Long, ByVal lngSortKey As Long, Optional ByVal lngFirst As Long = -1, _
                             Optional ByVal lngLast As Long = -1)
            '--------------------------------------------------------------------
            'Date: 10/23/2000
            'Author: Dave Richkun
            'Description: Modified "QuickSort" (function defined above) to sort the items in a 2D varArray array.
            'Parameters: varArray - The 2D array to be sorted
            '            lngFirst - Optional value identifying the first element to begin sorting with
            '            lngLast - Optional value identifying the last element to begin sorting with
            'Returns: The sorted array by reference parameter
            '--------------------------------------------------------------------

            Dim lngLow As Long
            Dim lngHigh As Long
            Dim lngMiddle As Long
            Dim varTempVal() As Object
            Dim varTestVal As Object
            Dim lngCtr

            If lngFirst = -1 Then lngFirst = LBound(varArray)
            If lngLast = -1 Then lngLast = UBound(varArray)
            ReDim varTempVal(lng2DWidth)

            If lngFirst < lngLast Then
                lngMiddle = (lngFirst + lngLast) / 2
                varTestVal = varArray(lngMiddle, lngSortKey)
                lngLow = lngFirst
                lngHigh = lngLast
                Do
                    Do While varArray(lngLow, lngSortKey) < varTestVal
                        lngLow = lngLow + 1
                    Loop
                    Do While varArray(lngHigh, lngSortKey) > varTestVal
                        lngHigh = lngHigh - 1
                    Loop
                    If (lngLow <= lngHigh) Then
                        'copy to temp array
                        For lngCtr = 0 To lng2DWidth
                            varTempVal(lngCtr) = varArray(lngLow, lngCtr)
                        Next lngCtr

                        'swap positions
                        For lngCtr = 0 To lng2DWidth
                            varArray(lngLow, lngCtr) = varArray(lngHigh, lngCtr)
                        Next lngCtr

                        'restore from temp array
                        For lngCtr = 0 To lng2DWidth
                            varArray(lngHigh, lngCtr) = varTempVal(lngCtr)
                        Next lngCtr

                        lngLow = lngLow + 1
                        lngHigh = lngHigh - 1
                    End If
                Loop While (lngLow <= lngHigh)

                If lngFirst < lngHigh Then QuickSort2D(varArray, lng2DWidth, lngSortKey, lngFirst, lngHigh)
                If lngLow < lngLast Then QuickSort2D(varArray, lng2DWidth, lngSortKey, lngLow, lngLast)
            End If

        End Sub




        Public Sub StrCatDel(ByRef strMain As String, ByVal strSub As String, Optional ByVal strDel As String = ",")
            '--------------------------------------------------------------------------------
            'Author: Rick "Boom Boom" Segura                                                '
            'Date: 01/25/2001                                                               '
            'Description: Concetenates two strings with a delimeter                         '
            'Parameters: strMain - string to perform concatenation on                       '
            '            strSub -  string to concatenate                                    '
            '            strDel - delimeter to use                                          '
            '--------------------------------------------------------------------------------
            'Revision History:                                                              '
            '                                                                               '
            '--------------------------------------------------------------------------------
            If strMain > "" Then
                strMain = strMain & strDel & strSub
            Else
                strMain = strSub
            End If
        End Sub

    End Class
End Namespace
