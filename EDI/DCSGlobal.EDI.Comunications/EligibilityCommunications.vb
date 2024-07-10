Imports Microsoft.VisualBasic
Imports System.Security.Cryptography.X509Certificates
Imports Rx = System.Text.RegularExpressions
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
Imports DCSGlobal.PARAMOUNTHEALTH.GetResponse









Public Class EligibilityCommunications

    Dim SQL_CONNECTION_STRING As String = ""
    Dim sGetAllDataSp As String = ""
    Dim sSYNC_TIMEOUT As String = ""
    Dim sSUBMISSION_TIMEOUT As String = ""
    Dim sisParseVBorDB As String = ""
    Dim sisEmdeonLookUp As String = ""
    Dim sreRunEligAttempts As String = ""
    Dim suspEdiRequest As String = ""
    Dim suspEdiDbImport As String = ""
    Dim sCommandTimeOut As String = "90"
    Dim iThreadNumber As Integer = 0



    Dim ts1, ts2, ts3, ts4, ts5, ts6 As DateTime

    Dim ss As New StringStuff


    Dim log As New logExecption
    Dim appName As String = "ProcessEligilbilty class libary v13"
    Dim sPatientAccountNumber As String
    Dim sUserID As String
    Dim sInsuranceCode As String
    Dim sInsurancetype As String
    Dim sHospitalCode As String
    Dim sPatientHospitalCode As String
    Dim sVendorInputType As String
    Dim sRequestString270 As String
    Dim sID As String
    Dim sVendorName As String
    Dim sVendorPayorID As String
    Dim sRowData As String
    Dim sBatchID As String


    Public Property CommandTimeOut As String
        Get

        End Get
        Set(value As String)
            sCommandTimeOut = value
        End Set
    End Property

    Public Property ThreadNumber As Integer
        Get

        End Get
        Set(value As Integer)
            iThreadNumber = value
        End Set
    End Property

    Public Property ConnectionString As String
        Get

        End Get
        Set(value As String)
            SQL_CONNECTION_STRING = value
        End Set
    End Property

    Public Property getAllDataSp As String
        Get

        End Get
        Set(value As String)
            sGetAllDataSp = value
        End Set
    End Property


    Public Property SYNC_TIMEOUT As String
        Get

        End Get
        Set(value As String)
            sSYNC_TIMEOUT = value
        End Set
    End Property

    Public Property SUBMISSION_TIMEOUT As String
        Get

        End Get
        Set(value As String)
            sSUBMISSION_TIMEOUT = sSUBMISSION_TIMEOUT
        End Set
    End Property

    Public Property isParseVBorDB As String
        Get

        End Get
        Set(value As String)
            sisParseVBorDB = value
        End Set
    End Property

    Public Property isEmdeonLookUp As String
        Get

        End Get
        Set(value As String)
            sisEmdeonLookUp = value
        End Set
    End Property

    Public Property reRunEligAttempts As String
        Get

        End Get
        Set(value As String)
            sreRunEligAttempts = value

        End Set
    End Property

    Public Property uspEdiRequest As String
        Get

        End Get
        Set(value As String)
            suspEdiRequest = value
        End Set
    End Property

    Public Property uspEdiDbImport As String
        Get

        End Get
        Set(value As String)
            suspEdiDbImport = value
        End Set
    End Property

    Public Property LogPath As String
        Get

        End Get
        Set(value As String)

        End Set
    End Property

    Public Property LogPipe As String
        Get

        End Get
        Set(value As String)

        End Set
    End Property

    Public Property SettingPipe As String
        Get

        End Get
        Set(value As String)

        End Set
    End Property

    Public Property ErrorPipe As String
        Get

        End Get
        Set(value As String)

        End Set
    End Property

    Public Property PayorID As String
        Get

        End Get
        Set(value As String)
            sVendorPayorID = value
        End Set
    End Property

    Public Property VendorName As String
        Get

        End Get
        Set(value As String)
            sVendorName = value
        End Set
    End Property

    Public Property ID As String
        Get

        End Get
        Set(value As String)
            sID = value
        End Set
    End Property




    Public Property Insurancetype As String
        Get

        End Get
        Set(value As String)
            sInsurancetype = value
        End Set
    End Property

    Public Property RequestString270 As String
        Get

        End Get
        Set(value As String)
            sRequestString270 = value
        End Set
    End Property

    Public Property VendorInputType As String
        Get

        End Get
        Set(value As String)
            sVendorInputType = value
        End Set
    End Property

    Public Property PatientHospitalCode As String
        Get

        End Get
        Set(value As String)
            sPatientHospitalCode = value
        End Set
    End Property

    Public Property HospitalCode As String
        Get

        End Get
        Set(value As String)
            sHospitalCode = value
        End Set
    End Property

    Public Property InsuranceCode As String
        Get

        End Get
        Set(value As String)
            sInsuranceCode = value
        End Set
    End Property

    Public Property UserID As String
        Get

        End Get
        Set(value As String)
            sUserID = value
        End Set
    End Property

    Public Property AccountNumber As String
        Get

        End Get
        Set(value As String)
            sPatientAccountNumber = value
        End Set
    End Property


    Public Property RowData As String
        Get

        End Get
        Set(value As String)
            sRowData = value
        End Set
    End Property


    Public Property BatchID As String
        Get

        End Get
        Set(value As String)
            sBatchID = value
        End Set
    End Property


    Public Enum AuditType
        Request = 10
        Response = 20
        Both = 30
    End Enum



    Public Function getEligVendorEdiResponse(ByVal RequestContent As String, ByVal PayorCode As String, ByVal VendorName As String, ByVal vendor_input_type As String) As String
        Dim StrResponse As String = ""
        Dim strReq As String = RequestContent
        Dim strReqRaw As String = strReq
        Dim GlobalBatchID As Integer = 2002
        Dim myResponses As String() = {""}





        Try




            ts1 = DateTime.Now() 'subba-101913
            Const VS_PROXY_SERVER As String = ""
            Const VS_PROXY_PORT As Int32 = 80
            If VendorName = "VISIONSHARE" Then
                Const VS_HOST_ADDRESS As String = "204.11.46.227"
                Dim VS_SES_PORT As Int32 = 4090
                If PayorCode = "00007" Or PayorCode = "CMS" Then 'SUBBA-100413
                    VS_SES_PORT = 4091
                    myResponses(0) = HttpProcessWrapper(strReq, VS_HOST_ADDRESS, VS_SES_PORT, VS_PROXY_SERVER, VS_PROXY_PORT, "dcsghets", "dcsg821z", "SYNC_HETS_PROD")
                ElseIf PayorCode = "00002" Or PayorCode = "00003" Or PayorCode = "00032" Then
                    myResponses(0) = HttpProcessWrapper(strReq, VS_HOST_ADDRESS, VS_SES_PORT, VS_PROXY_SERVER, VS_PROXY_PORT, "dcsgwpnt", "Dx9m3fHvy", "SYNC_WELLPOINT")
                End If
            ElseIf VendorName = "POST-N-TRACK" Then
                'timer
                myResponses(0) = ProcessPNT(strReq)
                'after
            ElseIf VendorName = "PARAMOUNTEDI" Then 'subba-101913
                Try


                    myResponses(0) = DCSGlobal.PARAMOUNTHEALTH.GetResponse.CallWebService(strReq)
                    'If (strTypeAssmblyParamount = "DCSGlobal.PARAMOUNTHEALTH.GetResponse") Then myResponses(0) = PARAMOUNTHEALTH.GetResponse.CallWebService(strReq)
                Catch ex As Exception
                    log.ExceptionDetails(appName + " getEligVendorEdiResponse", ex)
                    '   logExceptionGeneric(ex.Message + "_" + "PARAMOUNTHEALTH.dll-Missing-SubmitEligibilityRequestbyEdiRequest_new", ex.StackTrace)
                End Try
            ElseIf VendorName = "IVANS" Then
                'Select Case PayorCode
                '    Case "00059"
                '        'strReq = strReq.Replace("ISA*00*          *00*          *ZZ*               *ZZ*RECEIVER     ID*", "ISA*00*0000017925*00*7CF8W1JK4W*ZZ*               *ZZ*87726          *")
                '        'ProcessIVANS(strReq)
                '    Case "CMS", "CMS_RR", "00007"
                '        'strReq = strReq.Replace("ISA*00*          *00*          *ZZ*               *ZZ*RECEIVER     ID*", "ISA*00*0000017925*00*7CF8W1JK4W*ZZ*               *ZZ*87726          *")
                '        'Additional String Manupulation May be needed for CMS
                '        'ProcessIVANS(strReq)
                '    Case Else
                '        'ProcessIVANS(strReq)
                'End Select
                myResponses(0) = ProcessIVANS(strReq)
                myResponses(0) = myResponses(0).Replace("IVANS-CMS-RT", String.Empty.PadLeft(12, "X"))
                myResponses(0) = myResponses(0).Replace("EO-IVANS-RT", String.Empty.PadLeft(11, "X"))
                myResponses(0) = myResponses(0).Replace("IVANS-DO-RT", String.Empty.PadLeft(11, "X"))

            ElseIf VendorName = "AVAILITY" Then
                myResponses(0) = ProcessAvaility(strReq)
            ElseIf VendorName = "EMDEON" Then
                Dim bEmdeonLookUp As Boolean = False
                If (sisEmdeonLookUp = "Y") Then
                    ' myResponses(0) = ProcessEmdeonAssistant(EMDEON_API_KEY, subscriberlastname, subscriberfirstname, subscriberdob, DateTime.Now.ToString("MM/dd/yyyy"), EBRID, sUserID, sPat_hosp_Code, "EmdeonLookup", 0, sHosp_Code, "EmdeonLookup", sInsType, sPatAcctNum, PayorCode, ConnStr)
                    If myResponses(0).Split("@@")(0) = "NOT_FOUND" Then
                        bEmdeonLookUp = False
                    Else
                        bEmdeonLookUp = True
                    End If
                End If
                If bEmdeonLookUp = False Then
                    myResponses(0) = ProcessEmdeonDirect(strReq)
                End If
            ElseIf VendorName = "MEDDATA" Then
                Try
                    Dim myService As New com.meddatahealth.services.MedDataExternalSubmissionPortal()
                    Dim securityHeader As New com.meddatahealth.services.SecurityHeader()
                    With securityHeader
                        .UserName = "2019275"
                        .Password = "fT~H31]m"
                    End With

                    'Dim securityHeader As New com.meddatahealth.services.SecurityHeader() With {.UserName = "2019275", .Password = "fT~H31]m"} 
                    'Dim myService As New com.meddatahealth.services.MedDataExternalSubmissionPortal() With {.Proxy = Nothing}
                    myService.SecurityHeaderValue = securityHeader
                    Dim SYNC_TIMEOUT As String = "0.00:00:35"
                    Dim SUBMISSION_TIMEOUT As String = "0.00:00:35"
                    Try
                        SYNC_TIMEOUT = sSYNC_TIMEOUT
                        SUBMISSION_TIMEOUT = sSUBMISSION_TIMEOUT
                    Catch ex As System.Exception
                        SYNC_TIMEOUT = "0.00:00:35"
                        SUBMISSION_TIMEOUT = "0.00:00:35"
                    End Try
                    myResponses(0) = myService.SubmitSync(strReq, vendor_input_type, "Edi", SYNC_TIMEOUT, SUBMISSION_TIMEOUT)
                Catch ex As System.Exception

                    log.ExceptionDetails(appName + " getEligVendorEdiResponse", ex)


                    '    logExceptionGeneric(ex.Message, ex.StackTrace)
                End Try
            End If
            StrResponse = myResponses(0)
        Catch ex As Exception
            StrResponse = ""
        End Try

        Return StrResponse
    End Function

    Public Function ProcessAvaility(ByVal s270Content As String) As String
        Dim httpData As String = Nothing
        Dim url As String = ""
        Try
            url = "https://qa-apps.availity.com/availity/B2BHCTransactionServlet"
            '    Case "PROD"
            'url = "https://apps.availity.com/availity/B2BHCTransactionServlet"
            Dim req As HttpWebRequest = Nothing
            Dim resp As HttpWebResponse = Nothing
            '
            ' Username and Password must be in ISA02 and ISA04 of the request
            '
            Dim strRequest As String = s270Content
            req = DirectCast(WebRequest.Create(url), HttpWebRequest)
            With req
                .Method = "POST"
                .ContentType = "application/x-www-form-urlencoded"
            End With
            Dim up = req.GetRequestStream()
            Dim sw As New StreamWriter(up)
            With sw
                .Write(strRequest)
                .Flush()
                .Close()
            End With
            resp = DirectCast(req.GetResponse(), HttpWebResponse)
            Dim s = resp.GetResponseStream()
            Dim sr As StreamReader = New StreamReader(s)
            With sr
                httpData = .ReadToEnd()
                .Close()
                s.Close()
            End With
            resp.Close()
        Catch ex As Exception
            log.ExceptionDetails(appName + " ProcessAvaility", ex)

            httpData = ex.Message
        End Try
        Return httpData
    End Function


    Public Function ProcessIVANS(ByVal s270Contents As String) As String
        Dim Res As String = ""
        Try
            Dim e1 As New com.ivans.limeservices.EligibilityOne()

            Dim a As New com.ivans.limeservices.IvansWSAuthentication()
            a.User = "8DC00005"
            a.Password = "DC$Gl0bal//2013"
            a.ClientId = "f9e52687-a2e0-4d9c-8591-59516c9fd026"



            e1.PreAuthenticate = True
            e1.IvansWSAuthenticationValue = a
            '.PreAuthenticate = False
            e1.Timeout = CInt(Mid(sSYNC_TIMEOUT, 9, 2) * 1000)


            Res = e1.SendCommercialEligibilityFormRequest(s270Contents).Replace(vbCr, "").Replace(vbLf, "")
        Catch ex As Exception


            log.ExceptionDetails(appName + " ProcessIVANS", ex)

            Res = ex.Message
        End Try
        Return Res
    End Function


    Public Function RES(ByVal REQ) As String

        Dim r As String = ""

        'log.ExceptionDetails("MEDDATA " + _VendorInputType, REQ)



        'Try
        '    Dim myService As New com.meddatahealth.services.MedDataExternalSubmissionPortal()
        '    Dim securityHeader As New com.meddatahealth.services.SecurityHeader()

        '    'User name and password
        '    securityHeader.UserName = "2019275"
        '    securityHeader.Password = "fT~H31]m"



        '    'Dim securityHeader As New com.meddatahealth.services.SecurityHeader() With {.UserName = "2019275", .Password = "fT~H31]m"} 
        '    'Dim myService As New com.meddatahealth.services.MedDataExternalSubmissionPortal() With {.Proxy = Nothing}


        '    myService.SecurityHeaderValue = securityHeader

        '    Try
        '        SYNC_TIMEOUT = sSYNC_TIMEOUT
        '        SUBMISSION_TIMEOUT = sSUBMISSION_TIMEOUT
        '    Catch ex As Exception
        '        SYNC_TIMEOUT = "0.00:00:35"
        '        SUBMISSION_TIMEOUT = "0.00:00:35"
        '    End Try
        '    myResponses(0) = myService.SubmitSync(REQ, _VendorInputType, "Edi", SYNC_TIMEOUT, SUBMISSION_TIMEOUT)



        'Catch ex As Exception

        '    log.ExceptionDetails(_AppName + " getEligVendorEdiResponse MEDDATA", ex)


        '    '    logExceptionGeneric(ex.Message, ex.StackTrace)
        'End Try



        Return r
    End Function

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
            log.ExceptionDetails(appName + " ProcessPNT", ex)
        End Try

        Return httpData
    End Function



    Public Function HttpProcessWrapper(ByVal strReq As String, ByVal VS_HOST_ADDRESS As String, ByVal VS_SES_PORT As Integer, ByVal VS_PROXY_SERVER As String, ByVal VS_PROXY_PORT As Integer, ByVal uid As String, ByVal pwd As String, ByVal conn As String) As String
        Dim oHttp As HTTPProcess
        Dim RetVal As String = ""
        Try
            oHttp = New HTTPProcess(VS_HOST_ADDRESS, VS_SES_PORT, VS_PROXY_SERVER, VS_PROXY_PORT, uid, pwd, conn)
            Dim data As ResponseData = oHttp.Process(strReq)
            RetVal = data.Content

        Catch ex As System.Exception

            log.ExceptionDetails(appName + " ProcessPNT", ex)
            RetVal = ex.Message
        End Try
        Return RetVal
    End Function


    Public Function ProcessEmdeonDirect(ByVal s270Contents As String) As String

        Dim httpData As String = Nothing


        Try
            Dim client As New raEmdeon2014.AWSSoapClient
            httpData = System.Text.ASCIIEncoding.ASCII.GetString(System.Convert.FromBase64String(Convert.ToBase64String(client.RunTransaction("PMNADCSGL", "TMURwvb3", System.Text.Encoding.UTF8.GetBytes(s270Contents)))))

        Catch ex As Exception

            httpData = ex.Message
            log.ExceptionDetails(appName + " ProcessEmdeonDirect", ex)

        End Try

        Return httpData



    End Function
End Class
