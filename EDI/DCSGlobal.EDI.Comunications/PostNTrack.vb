Imports Microsoft.VisualBasic
Imports System.Security.Cryptography.X509Certificates
Imports System.Collections.Specialized
Imports System.Text.RegularExpressions
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Net.Security
Imports System.Threading
Imports System.Timers
Imports System.Web
Imports System.Net
Imports System.IO
Imports System.Data
Imports System.Data.SqlTypes
Imports System.Reflection
Imports System.Diagnostics
Imports System.Globalization
Imports DCSGlobal.BusinessRules.CoreLibrary
Imports DCSGlobal.BusinessRules.Logging



Public Class PostNTrack


    Public Function ProcessPNT(ByVal s270Contents As String) As String
        Dim httpData As String = Nothing
        Try
            Dim wsurl As String = ""
            'sTime.Text = DateTime.Now.ToString()
            'wsurl = ConfigurationManager.AppSettings("wsurl")
            'string url = 
            Dim url As String = "https://realtime1.post-n-track.com/realtime/request_x12.aspx"
            Dim req As HttpWebRequest = Nothing
            Dim resp As HttpWebResponse = Nothing
            '
            ' Username and Password must be in ISA02 and ISA04 of the request
            '
            Dim strRequest As String = s270Contents
            'errLog.WriteEvent("270 RAW:" & vbCrLf & strRequest)
            req = DirectCast(WebRequest.Create(url), HttpWebRequest)
            req.Method = "POST"
            req.ContentType = "application/x-www-form-urlencoded"
            Dim up As Stream = req.GetRequestStream()
            Dim sw As New StreamWriter(up)
            sw.Write(strRequest)
            sw.Flush()
            sw.Close()
            resp = DirectCast(req.GetResponse(), HttpWebResponse)
            Dim s As Stream = resp.GetResponseStream()
            Dim sr As New StreamReader(s)
            httpData = sr.ReadToEnd()
            sr.Close()
            s.Close()
            '
            'The response is now in the httpData string
            'If an error occurred in processing on the server side
            'the httpData string will start with ERR=
            '
            resp.Close()
            'eTime.Text = DateTime.Now.ToString()
        Catch ex As System.Exception
            ' log.ExceptionDetails(appName + " ProcessPNT", ex)
        End Try

        Return httpData
    End Function



End Class
