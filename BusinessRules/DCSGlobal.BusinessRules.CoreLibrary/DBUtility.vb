Imports Microsoft.VisualBasic
Imports System.Security.Cryptography
Imports System.IO
Imports System
'Imports DCSGlobal.BusinessRules.Logging

Public Class DBUtility
    Implements IDisposable




    Private Const ENCRYPT_DECRYPT_KEY As String = "&%#@?,:*#$$($#$#(%%%(%($(#$#(@(##)@$$(*%"
    Private Const STRING_ENCRYPT_DECRYPT_KEY = "&%#@?,:*"

    ' Private el As New LogToEventLog()



    Public Sub Dispose() Implements System.IDisposable.Dispose

        GC.SuppressFinalize(Me)

        ''  Console.WriteLine("Object " & GetHashCode() & " disposed.")
    End Sub

    Protected Overrides Sub Finalize()
        '  el = Nothing
        ' oD = Nothing
        ' em = Nothing
        Dispose()
        '' Console.WriteLine("Object " & GetHashCode() & " finalized.")
    End Sub









    ''' <summary>
    ''' This is legacy and is obsolete please use  EncryptConString and  DecryptConString
    ''' </summary>
    ''' <param name="connString"></param>
    ''' <returns>err if could not get it other wise clear text string.</returns>
    ''' <remarks></remarks>
    Public Function getConnectionString(ByVal connString As String) As String
        Try
            Dim originalPwd As String = connString
            Dim parsedPwd As String = String.Empty
            Dim newPwd As String = String.Empty
            Dim sIndex As Integer = originalPwd.IndexOf(";Password=")
            Dim eIndex As Integer = originalPwd.IndexOf(";Initial")
            newPwd = originalPwd.Substring(sIndex, eIndex - sIndex)
            parsedPwd = newPwd.Replace(";Password=", "")
            Dim decryptPass As String = Decrypt(parsedPwd)
            Return originalPwd.Replace(parsedPwd, decryptPass)
        Catch ex As Exception

            '  el.WriteEventError("getConnectionString", 1, connString)
            Return "err"
        End Try





    End Function

    Public Function ValidateConString(ByVal TheString) As Boolean

        Return True

    End Function





    Public Function EncryptConString(ByVal TheString) As String

        Return TheString

    End Function

    Public Function DecryptConString(ByVal TheString) As String

        Return TheString

    End Function



    Public Function Encrypt(ByVal strText As String) As String
        Dim byKey() As Byte = {}
        Dim IV() As Byte = {&H0, &H2, &H4, &H6, &H8, &H10, &H12, &H14, &H16, &H18, &H20, &H22, &H24, &H26, &H28, &H30}
        Try
            byKey = System.Text.Encoding.UTF8.GetBytes(ENCRYPT_DECRYPT_KEY.Substring(0, 16))
            Dim rjnd As New RijndaelManaged()
            Dim ks As New KeySizes(16, 16, 2)
            Dim inputByteArray() As Byte = System.Text.Encoding.UTF8.GetBytes(strText)
            Dim ms As New MemoryStream
            Dim cs As New CryptoStream(ms, rjnd.CreateEncryptor(byKey, IV), CryptoStreamMode.Write)
            cs.Write(inputByteArray, 0, inputByteArray.Length)
            cs.FlushFinalBlock()
            Return Convert.ToBase64String(ms.ToArray())
        Catch ex As System.Exception
            Return ex.Message
        End Try

    End Function

    Public Function Decrypt(ByVal strText As String) As String
        Dim byKey() As Byte = {}
        Dim IV() As Byte = {&H0, &H2, &H4, &H6, &H8, &H10, &H12, &H14, &H16, &H18, &H20, &H22, &H24, &H26, &H28, &H30}

        Try
            byKey = System.Text.Encoding.UTF8.GetBytes(ENCRYPT_DECRYPT_KEY.Substring(0, 16))
            Dim rjnd As New RijndaelManaged()
            Dim ks As New KeySizes(16, 16, 2)
            Dim inputByteArray As Byte() = Convert.FromBase64String(strText)
            Dim ms As New MemoryStream(strText.Length)
            Dim cs As New CryptoStream(ms, rjnd.CreateDecryptor(byKey, IV), CryptoStreamMode.Write)
            cs.Write(inputByteArray, 0, inputByteArray.Length)
            cs.FlushFinalBlock()
            Dim encoding As System.Text.Encoding = System.Text.Encoding.UTF8
            Return encoding.GetString(ms.ToArray())
        Catch ex As System.Exception
            Return ex.Message
        End Try

    End Function

    Public Function StringEncrypt(ByVal strText As String, ByVal strEncrKey As String) As String
        Dim byKey() As Byte = {}
        Dim IV() As Byte = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}
        Dim sEncrypted As String = ""

        Try
            byKey = System.Text.Encoding.UTF8.GetBytes(Left(strEncrKey, 8))
            'byKey = System.Text.Encoding.UTF8.GetBytes(Left(strEncrKey, 64))

            Dim des As New DESCryptoServiceProvider
            Dim inputByteArray() As Byte = System.Text.Encoding.UTF8.GetBytes(strText)
            Dim ms As New MemoryStream
            Dim cs As New CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write)
            cs.Write(inputByteArray, 0, inputByteArray.Length)
            cs.FlushFinalBlock()

            sEncrypted = Convert.ToBase64String(ms.ToArray()) 'subba-021009
            'Return Convert.ToBase64String(ms.ToArray())
        Catch ex As System.Exception
            sEncrypted = "Error Encryption"
            '[Global].insertExceptionDetails("914-alLogin-postback-GeneralFuncs.vb-Encrypt():" + ex.Message, "GeneralFuncs-Encrypt()-", "Encrypt()")
            'Return ex.Message
        End Try
        Return sEncrypted
    End Function

    Public Function StringDecrypt(ByVal strText As String, ByVal sDecrKey As String) As String
        Dim byKey() As Byte = {}
        Dim IV() As Byte = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}
        strText = strText.Replace(" ", "+")
        Dim inputByteArray(strText.Length) As Byte
        Dim sDecrypted As String = ""

        Try
            byKey = System.Text.Encoding.UTF8.GetBytes(Left(sDecrKey, 8))
            'byKey = System.Text.Encoding.UTF8.GetBytes(Left(sDecrKey, 64))
            Dim des As New DESCryptoServiceProvider
            inputByteArray = Convert.FromBase64String(strText)
            Dim ms As New MemoryStream
            Dim cs As New CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write)

            cs.Write(inputByteArray, 0, inputByteArray.Length)
            cs.FlushFinalBlock()
            Dim encoding As System.Text.Encoding = System.Text.Encoding.UTF8

            'Return encoding.GetString(ms.ToArray())
            sDecrypted = encoding.GetString(ms.ToArray()) 'subba-021009

        Catch ex As System.Exception
            sDecrypted = "Error Decryption"
            '[Global].insertExceptionDetails("915_GeneralFuncs.Decrypt()error:" + ex.Message, "GeneralFuncs.Decrypt()-" + "-" + Left(ex.StackTrace, 300), "GeneralFuncs.Decrypt()")
            Return sDecrypted

        End Try

        Return sDecrypted

    End Function

End Class
