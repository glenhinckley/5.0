'Imports Microsoft.VisualBasic
Imports System.Security.Cryptography.X509Certificates
Imports System.Configuration
Imports System.Net.Security
Imports System.Web
Imports System.Net
Imports System.IO

Imports DCSGlobal.BusinessRules.Logging






Public Class HTTPProcess
    Public SESHost As String
    Public SESPort As Int32
    Public ProxyHost As String
    Public ProxyPort As Int32
    Public UserName As String
    Public Password As String
    Public Connection As String
    Public UseClientCert As Boolean
    Private HttpProps As String() = {"POST", "application", "octet-stream", "A null HTTP response object was returned.", "Neutralus"}

    Dim log As New logExecption
    Dim appName As String = "ProcessEligilbilty class libary v13 HTTPProcess"




    Public Sub New(ByVal sSESHost As String, ByVal nSESPort As Int32, ByVal sProxyServer As String, ByVal nProxyPort As Int32, ByVal sUserName As String, ByVal sPassword As String, _
    ByVal sConnection As String)
        SESHost = sSESHost
        SESPort = nSESPort
        ProxyHost = sProxyServer
        ProxyPort = nProxyPort
        UserName = sUserName
        Password = sPassword
        Connection = sConnection
        UseClientCert = False
    End Sub

    Public Function Process(ByVal s270Contents As String) As ResponseData
        'ErrorLogger.WriteEvent("HttpProcess-ProcessMethod-BEGIN")
        Dim oRetVal As New ResponseData()
        oRetVal.HttpResponse = 200


        Dim sURL As String = String.Format("https://{0}:{1}/ses/upload?username={2}&password={3}&connection={4}", SESHost, SESPort, HttpUtility.UrlEncode(UserName), HttpUtility.UrlEncode(Password), HttpUtility.UrlEncode(Connection))
        Dim sw As StreamWriter = Nothing
        Dim objRequest As HttpWebRequest = DirectCast(WebRequest.Create(sURL), HttpWebRequest)
        objRequest.KeepAlive = True  '2012-feb-01



        Try
            System.Net.ServicePointManager.ServerCertificateValidationCallback = New System.Net.Security.RemoteCertificateValidationCallback(AddressOf OnValidateServerCertificate)
            'ServicePointManager.ServerCertificateValidationCallback = New net.Security.RemoteCertificateValidationCallback(AddressOf OnValidateServerCertificate)
            With objRequest
                .Method = HttpProps(0)
                .ServicePoint.Expect100Continue = False
                .ContentLength = s270Contents.Length
                .ContentType = String.Format("{0}/{1}", HttpProps(1), HttpProps(2))
            End With

            If UseClientCert Then
                Dim cert As X509Certificate2 = SelectLocalCertificate()
                If cert IsNot Nothing Then
                    objRequest.ClientCertificates.Add(cert)
                End If
            End If

            If Not String.IsNullOrEmpty(ProxyHost) Then
                objRequest.Proxy = New WebProxy(ProxyHost, ProxyPort)
            End If

            sw = New StreamWriter(objRequest.GetRequestStream())
            sw.Write(s270Contents)

        Catch ex As System.Exception
            'errLog.WriteEvent(HttpPostException.Message) 

            log.ExceptionDetails(appName + " Process", ex)
            'Throw New System.Exception(HttpPostException.Message, HttpPostException.InnerException)
        Finally
            If sw IsNot Nothing Then
                sw.Close()
            End If
        End Try

        Dim objResponse As HttpWebResponse = Nothing
        Dim sr As StreamReader = Nothing





        Try
            objResponse = DirectCast(objRequest.GetResponse(), HttpWebResponse)
            If objResponse IsNot Nothing Then
                sr = New StreamReader(objResponse.GetResponseStream())
                oRetVal.Content = sr.ReadToEnd()
            Else

                log.ExceptionDetails(appName + " Process objResponse", HttpProps(3))

                Throw New System.Exception(HttpProps(3))
            End If

        Catch ex As System.Exception
            'errLog.WriteEvent(ProtocolsSoapException.Message)



            log.ExceptionDetails(appName + " Process objResponse", ex)
        Finally
            If sr IsNot Nothing Then
                sr.Close()
            End If
        End Try






        If objResponse IsNot Nothing Then
            Try
                oRetVal.HttpResponse = Convert.ToInt32(objResponse.StatusCode)
            Catch ex As System.Exception



                log.ExceptionDetails(appName + " Process objResponse", ex)


                '  Throw New System.Exception(SystemExecutionEngineException.Message, SystemExecutionEngineException.InnerException)
            Finally
                objResponse.Close()
            End Try
        End If












        Return oRetVal
    End Function

    Public Function SelectLocalCertificate() As X509Certificate2
        Try
            Dim certStore As New X509Store(StoreName.My)
            certStore.Open(OpenFlags.OpenExistingOnly And OpenFlags.[ReadOnly])
            Dim clientCertificates As X509Certificate2Collection = certStore.Certificates
            For Each oCert As X509Certificate2 In clientCertificates
                If oCert.Issuer.Contains(HttpProps(4)) Then
                    Return oCert
                End If
            Next

        Catch ae As System.Security.Authentication.AuthenticationException

            log.ExceptionDetails(appName + " SelectLocalCertificate", ae.Message)

        End Try


        Return Nothing


    End Function

    Public Shared Function OnValidateServerCertificate(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean
        Return True
    End Function

End Class
