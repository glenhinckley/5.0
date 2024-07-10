
Imports System
Imports System.Text.RegularExpressions

Namespace EncodeDecodeStuff


    Public Class StringEncodeDecode

        Public Function Ver(ByVal test As String) As String

            Dim v As String = "24"

            Return v + test



        End Function

        Public Function dbDecode(ByVal StringToEncode As String) As String


            StringToEncode = Trim(StringToEncode)
            StringToEncode = Replace(StringToEncode, Chr(10), Chr(200), 1)
            StringToEncode = Replace(StringToEncode, Chr(13), Chr(201), 1)
            StringToEncode = Replace(StringToEncode, "'", Chr(202), 1)
            StringToEncode = Replace(StringToEncode, ",", Chr(203), 1)
            StringToEncode = Replace(StringToEncode, "&", Chr(204), 1)
            StringToEncode = Replace(StringToEncode, Chr(34), Chr(205), 1)
            StringToEncode = Replace(StringToEncode, "-", Chr(206), 1)
            StringToEncode = Replace(StringToEncode, "(", Chr(207), 1)
            StringToEncode = Replace(StringToEncode, ")", Chr(208), 1)
            dbDecode = StringToEncode

        End Function



        Public Function w3cDecode(ByVal StringToEncode As String) As String


            StringToEncode = Trim(StringToEncode)
            StringToEncode = Replace(StringToEncode, Chr(200), Chr(10), 1)
            StringToEncode = Replace(StringToEncode, Chr(201), Chr(13), 1)
            StringToEncode = Replace(StringToEncode, Chr(202), "'", 1)
            StringToEncode = Replace(StringToEncode, Chr(203), ",", 1)
            StringToEncode = Replace(StringToEncode, Chr(204), "&amp;", 1)
            StringToEncode = Replace(StringToEncode, Chr(205), "&quot;", 1)
            StringToEncode = Replace(StringToEncode, Chr(206), "-", 1)
            StringToEncode = Replace(StringToEncode, Chr(207), "(", 1)
            StringToEncode = Replace(StringToEncode, Chr(208), ")", 1)
            w3cDecode = StringToEncode

        End Function

        Public Function textDecode(ByVal StringToEncode As String) As String


            StringToEncode = Trim(StringToEncode)
            StringToEncode = Replace(StringToEncode, Chr(200), Chr(10), 1)
            StringToEncode = Replace(StringToEncode, Chr(201), Chr(13), 1)
            StringToEncode = Replace(StringToEncode, Chr(202), "'", 1)
            StringToEncode = Replace(StringToEncode, Chr(203), ",", 1)
            StringToEncode = Replace(StringToEncode, Chr(204), Chr(38), 1)
            StringToEncode = Replace(StringToEncode, Chr(205), Chr(34), 1)
            StringToEncode = Replace(StringToEncode, Chr(206), "-", 1)
            StringToEncode = Replace(StringToEncode, Chr(207), "(", 1)
            StringToEncode = Replace(StringToEncode, Chr(208), ")", 1)
            textDecode = StringToEncode

        End Function


        Public Function streamDecode(ByVal StringToEncode As String) As String


            StringToEncode = Trim(StringToEncode)
            StringToEncode = Replace(StringToEncode, Chr(200), Chr(10), 1)
            StringToEncode = Replace(StringToEncode, Chr(201), Chr(13), 1)
            StringToEncode = Replace(StringToEncode, Chr(202), "'", 1)
            StringToEncode = Replace(StringToEncode, Chr(203), ",", 1)
            StringToEncode = Replace(StringToEncode, Chr(204), Chr(38), 1)
            StringToEncode = Replace(StringToEncode, Chr(205), Chr(34), 1)
            StringToEncode = Replace(StringToEncode, Chr(206), "-", 1)
            StringToEncode = Replace(StringToEncode, Chr(207), "(", 1)
            StringToEncode = Replace(StringToEncode, Chr(208), ")", 1)
            StringToEncode = Replace(StringToEncode, Chr(209), "|", 1)
            streamDecode = StringToEncode

        End Function



        Public Function dbEncode(ByVal StringToEncode As String) As String


            StringToEncode = Trim(StringToEncode)
            StringToEncode = Replace(StringToEncode, Chr(10), Chr(200), 1)
            StringToEncode = Replace(StringToEncode, Chr(13), Chr(201), 1)
            StringToEncode = Replace(StringToEncode, "'", Chr(202), 1)
            StringToEncode = Replace(StringToEncode, ",", Chr(203), 1)
            StringToEncode = Replace(StringToEncode, "&", Chr(204), 1)
            StringToEncode = Replace(StringToEncode, Chr(34), Chr(205), 1)
            StringToEncode = Replace(StringToEncode, "-", Chr(206), 1)
            StringToEncode = Replace(StringToEncode, "(", Chr(207), 1)
            StringToEncode = Replace(StringToEncode, ")", Chr(208), 1)
            dbEncode = StringToEncode

        End Function

        Public Function w3cEncode(ByVal StringToEncode As String) As String


            StringToEncode = Trim(StringToEncode)
            StringToEncode = Replace(StringToEncode, Chr(200), Chr(10), 1)
            StringToEncode = Replace(StringToEncode, Chr(201), Chr(13), 1)
            StringToEncode = Replace(StringToEncode, Chr(202), "'", 1)
            StringToEncode = Replace(StringToEncode, Chr(203), ",", 1)
            StringToEncode = Replace(StringToEncode, Chr(204), "&amp;", 1)
            StringToEncode = Replace(StringToEncode, Chr(205), "&quot;", 1)
            StringToEncode = Replace(StringToEncode, Chr(206), "-", 1)
            StringToEncode = Replace(StringToEncode, Chr(207), "(", 1)
            StringToEncode = Replace(StringToEncode, Chr(208), ")", 1)
            w3cEncode = StringToEncode

        End Function

        Public Function textEncode(ByVal StringToEncode As String) As String


            StringToEncode = Trim(StringToEncode)
            StringToEncode = Replace(StringToEncode, Chr(200), Chr(10), 1)
            StringToEncode = Replace(StringToEncode, Chr(201), Chr(13), 1)
            StringToEncode = Replace(StringToEncode, Chr(202), "'", 1)
            StringToEncode = Replace(StringToEncode, Chr(203), ",", 1)
            StringToEncode = Replace(StringToEncode, Chr(204), Chr(38), 1)
            StringToEncode = Replace(StringToEncode, Chr(205), Chr(34), 1)
            StringToEncode = Replace(StringToEncode, Chr(206), "-", 1)
            StringToEncode = Replace(StringToEncode, Chr(207), "(", 1)
            StringToEncode = Replace(StringToEncode, Chr(208), ")", 1)
            textEncode = StringToEncode

        End Function

        Public Function StraemEncode(ByVal StringToEncode As String) As String


            StringToEncode = Trim(StringToEncode)
            StringToEncode = Replace(StringToEncode, Chr(10), Chr(200), 1)
            StringToEncode = Replace(StringToEncode, Chr(13), Chr(201), 1)
            StringToEncode = Replace(StringToEncode, "'", Chr(202), 1)
            StringToEncode = Replace(StringToEncode, ",", Chr(203), 1)
            StringToEncode = Replace(StringToEncode, Chr(38), Chr(204), 1)
            StringToEncode = Replace(StringToEncode, Chr(34), Chr(205), 1)
            StringToEncode = Replace(StringToEncode, "-", Chr(206), 1)
            StringToEncode = Replace(StringToEncode, "(", Chr(207), 1)
            StringToEncode = Replace(StringToEncode, ")", Chr(208), 1)
            StringToEncode = Replace(StringToEncode, "|", Chr(209), 1)
            StraemEncode = StringToEncode

        End Function



    End Class





End Namespace



