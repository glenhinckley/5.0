Imports System
Imports System.Text.RegularExpressions
Imports System.Xml




Namespace StringHandlingStuff


    Public Class StringStuff

        Inherits EncodeDecodeStuff.StringEncodeDecode


        Public EMsg As String 'used to return potential error messages in the functions below 




        Public Function GetFirstWord(ByVal line As String) As String

            Dim words As String() = line.Split(New Char() {" "c})

            ' Use For Each loop over words and display them.
            Dim _word As String = String.Empty
            Dim Count As Integer = 1

            For Each word In words
                Count = Count + 1
                _word = word
                If Count > 1 Then
                    Exit For
                End If
            Next

            Return _word
        End Function



        '        /// <summary>
        '/// .
        ' </summary>
        ''' <summary>
        ''' Get string value between [first] a and [last] b
        ''' </summary>
        ''' <param name="value"></param>
        ''' <param name="a"></param>
        ''' <param name="b"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Between(ByVal value As String, ByVal a As String, ByVal b As String) As String

            Dim r As String = String.Empty

            Dim posA As Integer = value.IndexOf(a)
            Dim posB As Integer = value.LastIndexOf(b)
            Dim adjustedPosA As Integer = 0
            Dim _Match As Boolean = False


            If (posA = -1) Then
                r = String.Empty
                _Match = True
            End If


            If (posB = -1) Then
                r = String.Empty
                _Match = True
            End If


            If _Match = False Then

                adjustedPosA = posA + a.Length

                If (adjustedPosA >= posB) Then
                    r = String.Empty
                    _Match = True
                End If

                r = value.Substring(adjustedPosA, posB - adjustedPosA)

            End If


            Return r

        End Function


        '/// <summary>
        '/// Get string value after [first] a.
        '/// </summary>
        ''' <summary>
        ''' Get string value after [first] a.
        ''' </summary>
        ''' <param name="value"></param>
        ''' <param name="a"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Before(ByVal value As String, ByVal a As String) As String

            Dim r As String = String.Empty
            Dim posA As Integer = value.IndexOf(a)
            Dim _Match As Boolean = False


            If (posA = -1) Then
                r = String.Empty
                _Match = True
            End If


            If _Match = False Then

                r = value.Substring(0, posA)
            End If


            Return r

        End Function



        '/// <summary>
        '/// Get string value after [last] a.
        '/// </summary>
        ''' <summary>
        ''' Get string value after [last] a.
        ''' </summary>
        ''' <param name="value"></param>
        ''' <param name="a"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function After(ByVal value As String, ByVal a As String) As String

            Dim r As String = String.Empty
            Dim posA As Integer = value.IndexOf(a)
            Dim adjustedPosA As Integer = 0
            Dim _Match As Boolean = False

            If (posA = -1) Then
                r = String.Empty
                _Match = True
            End If

            adjustedPosA = posA + a.Length

            If (adjustedPosA >= value.Length) Then
                r = String.Empty
                _Match = True
            End If

            If _Match = False Then
                r = value.Substring(adjustedPosA)
            End If


            Return r

        End Function




        ''' <summary>
        ''' Converts string to int or returns -999999999
        ''' </summary>
        ''' <param name="s"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ToInteger(ByVal s As String) As Integer
            ' An invalid number string.

            ' Try to parse it.
            ' ... If it isn't a number, use -1.
            Dim num As Integer
            If Not Integer.TryParse(s, num) Then
                num = -999999999
            Else
                num = Convert.ToInt32(s)
            End If

            Return num
        End Function


        ''' <summary>
        ''' Strip HTML tags.
        ''' </summary>
        Public Function StripHTMLTags(ByVal html As String) As String
            ' Remove HTML tags.
            Return Regex.Replace(html, "<.*?>", "")
        End Function


        Public Function ContainsMoreThan(ByVal text As String, ByVal count As Integer, ByVal value As String, ByVal comparison As StringComparison) As Boolean

            Dim r As Boolean = False
            Dim go As Boolean = True


            If String.IsNullOrEmpty(value) Then

                go = False
            End If




            Dim contains As Integer = 0
            Dim index As Integer = 0

            If go Then

                While (index = text.IndexOf(value, index, text.Length - index, comparison)) <> -1
                    If (contains = contains + 1) > count Then
                        r = True
                    End If

                    index = index + 1
                End While
            End If

            Return r


        End Function






        'Public Function Ver(ByVal test As String) As String

        '    Dim v As String = "24"

        '    Return v + test



        'End Function

        Public Function GetLastChr(ByVal TheString As String) As String

            Dim r As String

            Try

                r = TheString(TheString.Length - 1)

            Catch ex As Exception


                r = TheString

            End Try


            Return r

        End Function

        Public Function Formatltgt(ByVal r As String) As String

            With r

                r = .Replace("&gt;", ">")

                r = .Replace("&lt;", "<")

            End With

            Return r

        End Function

        Public Function ProcessAVAILITYXML(ByVal Kludge As String, ByVal Node As String) As String
            'RESPONSE_RAW = LEFT(@RESPONSE,CHARINDEX('<BODY>',@RESPONSE)+5)+'</BODY></ENVELOPE>'

            Dim x As New XmlDocument()

            Dim t As String = ""

            x.LoadXml(Kludge)



            Dim elemList As XmlNodeList = x.GetElementsByTagName(Node)

            t = elemList(0).InnerXml


            Kludge = t



            Return Kludge.Trim



        End Function


        Public Function ParseDemlimtedStringEDI(str As String, Delimter As String, position As Integer) As String
            Dim Tempstr() As String
            Dim r As String = ""

            Try
                If position >= 1 Then
                    Tempstr = Split(str, Delimter)


                    r = Tempstr(position - 1).Trim


                    If r = "" Then
                        r = String.Empty
                    End If



                End If
            Catch ex As Exception

                If ex.Message = "Index was outside the bounds of the array." Then
                    r = ""
                End If

            End Try


            Return r

        End Function




        Public Function ParseDemlimtedString(str As String, Delimter As String, position As Integer) As String
            Dim Tempstr() As String
            Dim r As String = ""

            Try
                If position >= 1 Then
                    Tempstr = Split(str, Delimter)


                    r = Tempstr(position - 1).Trim

                End If
            Catch ex As Exception

                If ex.Message = "Index was outside the bounds of the array." Then

                    r = ""


                End If

            End Try


            Return r

        End Function

        Private Function FormatRequest(ByVal r As String) As String

            With r

                r = .Replace("&gt;", ">")

                r = .Replace("&lt;", "<")

            End With

            Return r

        End Function


        Public Function CheckAZ09(ByVal TheString As String) As Boolean

            Dim b As Boolean = False
            Try

                Dim pattern As Regex = New Regex("[a-zA-Z0-9]")

                b = pattern.IsMatch(TheString)

            Catch


            End Try
            Return b
        End Function

        Public Function FormatAZ09(ByVal TheString As String) As String

            Dim r As String = String.Empty


            Try
                Dim pattern As Regex = New Regex("[a-zA-Z0-9]")

                If (pattern.IsMatch(TheString)) Then
                    r = TheString
                Else
                    r = "&nbsp;"
                End If


            Catch ex As Exception

            End Try

            Return r
        End Function


        Function CheckString(ByVal s As String, ByVal endchar As String) As String
            Dim pos As Integer

            pos = InStr(s, "'")
            While pos > 0
                s = Mid(s, 1, pos) & "'" & Mid(s, pos + 1)
                pos = InStr(pos + 2, s, "'")
            End While
            CheckString = "'" & s & "'" & endchar
        End Function

        Public Function Strip(ByVal T As String) As String


            T = Regex.Replace(T, "[^\u0000-\u007F]+", String.Empty)

            T = Trim(T)
            T = Replace(T, Chr(10), "", 1)
            T = Replace(T, Chr(13), "", 1)
            T = Replace(T, "'", "", 1)
            T = Replace(T, ",", "", 1)
            T = Replace(T, "&", "and", 1)
            T = Replace(T, Chr(34), "", 1)
            T = Replace(T, "-", "", 1)
            T = Replace(T, "(", "", 1)
            T = Replace(T, ")", "", 1)
            Strip = T

        End Function

        Public Function StripNONASCII(ByVal T As String) As String


            T = Regex.Replace(T, "[^\u0000-\u007F]+", String.Empty)

        End Function





        Public Function ReplaceRowDelimiter(ByVal T As String, ByVal RowDelimiter As String) As String


            T = StripCRLF(T)


            T = Trim(T)
            T = Replace(T, RowDelimiter, "~" + Chr(10), 1)


            Return T

        End Function



        Public Function StripCRLF(ByVal T As String) As String


            T = Trim(T)
            T = Replace(T, Chr(10), "", 1)
            T = Replace(T, Chr(13), "", 1)

            Return T

        End Function


        Public Function FormatZipCode(ByVal ZipCode As String)

            Dim z As String

            z = Strip(ZipCode)

            If (Len(z) = 9) Then

                Dim z1 As String
                Dim z2 As String


                z1 = Left(z, 5)
                z2 = (Right(z, 4))

                z = z1 & "-" & z2

            End If


            Return z

        End Function



        Public Function FormatLine(ByVal Line As String) As String
            Dim obj As New PhoneStuff.clsPhone

            obj.PhoneNumber = Line
            FormatLine = obj.FormatedNumber
            obj = Nothing


        End Function

        Public Function StripLine(ByVal T As String) As String
            Dim obj As New PhoneStuff.clsPhone
            obj.PhoneNumber = T
            StripLine = obj.StripedNumber

            obj = Nothing

        End Function

        Public Function StripEmail(ByVal T As String) As String


            T = Trim(T)
            T = Replace(T, "'", "", 1)
            T = Replace(T, ",", "", 1)
            T = Replace(T, "&", "and", 1)
            T = Replace(T, Chr(34), "", 1)
            T = Replace(T, "(", "", 1)
            T = Replace(T, ")", "", 1)
            StripEmail = T

        End Function

        Public Function PCase(ByVal strInput As String) As String

            Dim iPosition As Integer = 0 ' Our current position in the string (First character = 1)
            Dim iSpace As Integer = 0     ' The position of the next space after our iPosition
            Dim strOutput As String = ""  ' Our temporary string used to build the function's output

            ' Set our position variable to the start of the string.
            iPosition = 1

            ' We loop through the string checking for spaces.
            ' If there are unhandled spaces left, we handle them...
            Do While InStr(iPosition, strInput, " ", 1) <> 0
                ' To begin with, we find the position of the offending space.
                iSpace = InStr(iPosition, strInput, " ", 1)

                ' We uppercase (and append to our output) the first character after
                ' the space which was handled by the previous run through the loop.
                strOutput = strOutput & UCase(Mid(strInput, iPosition, 1))

                ' We lowercase (and append to our output) the rest of the string
                ' up to and including the current space.
                strOutput = strOutput & LCase(Mid(strInput, iPosition + 1, iSpace - iPosition))

                iPosition = iSpace + 1

            Loop

            strOutput = strOutput & UCase(Mid(strInput, iPosition, 1))
            strOutput = strOutput & LCase(Mid(strInput, iPosition + 1))

            ' That's it - Set our return value and exit
            PCase = strOutput

        End Function

        Public Function CountCharacters(ByVal TheString As String, _
        ByVal CharactersToCheckFor As String) As Integer

            Dim chr As String
            Dim ReturnAgain As Boolean
            CountCharacters = 0
            Dim i As Integer


            For i = 1 To Len(TheString)


                If i < (Len(TheString) + 1 - Len(CharactersToCheckFor)) Then
                    chr = Mid(TheString, i, Len(CharactersToCheckFor))
                    ReturnAgain = True
                Else
                    chr = Mid(TheString, i)
                    ReturnAgain = False
                End If

                If chr = CharactersToCheckFor Then CountCharacters = CountCharacters + 1
                If ReturnAgain = False Then GoTo NextPos
            Next i

NextPos:

        End Function

        Public Function AltCaps(ByVal TheString As String, _
        Optional ByVal StartWithFirstCharacter As Boolean = True) As String

            Dim LastCap As Boolean
            AltCaps = ""
            If StartWithFirstCharacter = False Then LastCap = True
            Dim i As Integer


            For i = 1 To Len(TheString)


                If LastCap = False Then
                    AltCaps = AltCaps & UCase(Mid(TheString, i, 1))
                    LastCap = True
                Else
                    AltCaps = AltCaps & LCase(Mid(TheString, i, 1))
                    LastCap = False
                End If

            Next i

        End Function


        Public Function ReverseString(ByVal TheString As String) As String

            ReverseString = ""
            Dim i As Integer


            For i = 0 To Len(TheString) - 1
                ReverseString = ReverseString & Mid(TheString, Len(TheString) - i, 1)
            Next i

        End Function


        Public Function RemoveExtraSpaces(ByVal TheString As String) As String

            Dim LastChar As String
            Dim NextChar As String
            LastChar = Left(TheString, 1)
            RemoveExtraSpaces = LastChar
            Dim i As Integer


            For i = 2 To Len(TheString)
                NextChar = Mid(TheString, i, 1)


                If NextChar = " " And LastChar = " " Then
                Else
                    RemoveExtraSpaces = RemoveExtraSpaces & NextChar

                End If

                LastChar = NextChar
            Next i

        End Function


        Public Function DelimitString(ByVal TheString As String, ByVal Delimiter As String) As String

            DelimitString = ""
            Dim i As Integer


            For i = 1 To Len(TheString)


                If i <> Len(TheString) Then
                    DelimitString = DelimitString & Mid(TheString, i, 1) & Delimiter
                Else
                    DelimitString = DelimitString & Mid(TheString, i, 1)
                End If

            Next i

        End Function


        Public Function ParseDemlimtedStringOLD(ByVal Message As String, ByVal delimiter As String, ByVal index As Integer) As String

            Dim curIndex As Integer = 0
            Dim pos As Integer = 1
            Dim prevPos As Integer = 0
            Dim result As String = ""

            Dim TAB As String
            Dim LF As String
            Dim CR As String
            Dim NL As String





            TAB = Chr(9)
            LF = Chr(10)
            CR = Chr(13)
            NL = Chr(13) + Chr(10)




            While (pos > 0)


                'pos = CHARINDEX(delimiter, Message, prevPos)

                If (pos > 0) Then
                    '   result = SUBSTRING(Message, prevPos, pos - prevPos)

                Else

                    'result = SUBSTRING(Message, prevPos, Len(Message))
                End If


                If (index = curIndex) Then

                    result = Replace(Replace(Replace(LTrim(RTrim(result)), NL, ""), LF, ""), CR, "")
                    result = Right(result, Len(result) - 2)
                    If (result = "") Then

                        result = "EOF"

                    End If
                End If



                prevPos = pos + 1
                curIndex = curIndex + 1

            End While



            Return result



        End Function





        Public Function ScriptCheck(ByVal TheString As String) As Boolean
            Dim Reg As Regex = New Regex("[^0-9a-zA-Z]")
            Dim M As Match

            ' find a match in the string
            M = Reg.Match(TheString)
            If M.Success Then
                Return True
            Else
                Return False
            End If


        End Function

        ' Public Function ICANN_EXTisOK(ByVal sEXT As String) As Integer

        '  Dim objICANN As New supersub.WebMail
        '
        '       ICANN_EXTisOK = objICANN.ICANN_EXTisOK(sEXT)

        'End Function

        ' Public Function ValidateEmail(ByVal strEmail As String) As Integer

        '      Dim objWM As New supersub.WebMail

        '     ValidateEmail = objWM.ValidateEmail(strEmail)
        '    EMsg = objWM.eMsg


        'End Function

        Public Function ProfanityCheck(ByVal sEXT As String) As Integer

            '            Dim EXT As String, X As Long


            ' return zero if nothing found so just set it there now
            ProfanityCheck = 0

            If InStr(1, UCase(sEXT), "FUCK", vbBinaryCompare) = 1 Then
                ProfanityCheck = 1
                Exit Function
            ElseIf InStr(1, UCase(sEXT), "SHIT", vbBinaryCompare) = 1 Then
                ProfanityCheck = 1
                Exit Function
            ElseIf InStr(1, UCase(sEXT), "FUCKER", vbBinaryCompare) = 1 Then
                ProfanityCheck = 1
                Exit Function
            ElseIf InStr(1, UCase(sEXT), "PUSSY", vbBinaryCompare) = 1 Then
                ProfanityCheck = 1
                Exit Function
            ElseIf InStr(1, UCase(sEXT), "CUNT", vbBinaryCompare) = 1 Then
                ProfanityCheck = 1
                Exit Function
            End If


        End Function



        ''' <summary>
        ''' method for determining is the user provided a valid email address
        ''' We use regular expressions in this check, as it is a more thorough
        ''' way of checking the address provided
        ''' </summary>
        ''' <param name="email">email address to validate</param>
        ''' <returns>true is valid, false if not valid</returns>
        Public Function IsValidEmail(ByVal email As String) As Boolean
            'regular expression pattern for valid email
            'addresses, allows for the following domains:
            'com,edu,info,gov,int,mil,net,org,biz,name,museum,coop,aero,pro,tv
            Dim pattern As String = "^[-a-zA-Z0-9][-.a-zA-Z0-9]*@[-.a-zA-Z0-9]+(\.[-.a-zA-Z0-9]+)*\." & _
            "(com|edu|info|gov|int|mil|net|org|biz|name|museum|coop|aero|pro|tv|[a-zA-Z]{2})$"
            'Regular expression object
            Dim check As New Text.RegularExpressions.Regex(pattern, RegexOptions.IgnorePatternWhitespace)
            'boolean variable to return to calling method
            Dim valid As Boolean = False

            'make sure an email address was provided
            If String.IsNullOrEmpty(email) Then
                valid = False
            Else
                'use IsMatch to validate the address
                valid = check.IsMatch(email)
            End If
            'return the value to the calling method
            Return valid
        End Function




    End Class




End Namespace