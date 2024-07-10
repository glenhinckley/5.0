Imports Microsoft.VisualBasic
Imports System.Security.Cryptography
Imports System.Text
Imports System.IO





Public Class DCSEncrypt
    Inherits DBUtility

    Const PASSWD_ENCRYPT_DECRYPT_KEY As String = "&%#@?,:*"

    Const ENCRYPT_KEY As String = "111"





    Private Shared DES As New TripleDESCryptoServiceProvider
    Private Shared MD5 As New MD5CryptoServiceProvider



    Private Shared Function MD5Hash(ByVal value As String) As Byte()


        Return MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(value))

    End Function




    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="PlainTextPasswd"></param>
    ''' <param name="Key"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function EncryptPasswd(ByVal PlainTextPasswd As String, ByVal Key As String) As String
        Dim r As String = String.Empty


        r = StringEncrypt(PlainTextPasswd, PASSWD_ENCRYPT_DECRYPT_KEY)

        Return r

    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="EncryptedTextPasswd"></param>
    ''' <param name="Key"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DecryptPasswd(ByVal EncryptedTextPasswd As String, ByVal Key As String) As String
        Dim r As String = String.Empty

        r = StringDecrypt(EncryptedTextPasswd, PASSWD_ENCRYPT_DECRYPT_KEY)


        Return r

    End Function



End Class
