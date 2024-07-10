﻿  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;
using DCSGlobal.BusinessRules.Logging;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using System.ServiceModel;
using System.ServiceModel.Channels;

using System.Xml;


namespace DCSGlobal.EDI.Comunications
{
    class Anthem : IDisposable
    {
    
        private logExecption log = new logExecption();
        private StringStuff ss = new StringStuff();

        private string _ConnectionString = string.Empty;
        private string _VendorName = "ANTHEM";
        private string _REQ = string.Empty;
        private string _RES = string.Empty;
        private string _PayorCode = string.Empty;
        private string _wsUrl = string.Empty;
        private string _UserName = string.Empty;
        private string _Passwd = string.Empty;
        private long _ServiceTimeOut = 30;
        private long _ReceiveTimeout = 30;


        private string sTimeStamp = string.Empty;
        private string sProcessingMode = string.Empty;
        private string sPayloadType = string.Empty;
        private string sSenderID = string.Empty;
        private string sReceiverID = string.Empty;
        private string sCORERuleVersion = string.Empty;



        private string _PayloadID = string.Empty;
        private string _TimeStamp = string.Empty;
        private string _ProcessingMode = string.Empty;
        private string _PayloadType = string.Empty;
        private string _SenderID = string.Empty;
        private string _ReceiverID = string.Empty;
        private string _CORERuleVersion = string.Empty;
        private string _Payload = string.Empty;

        private int _VMetricsLogging = 0;
        private string ErrorMsg = "";
        private string _ProtocolType = string.Empty;

        long _elapsedTicks = 0;




        bool _disposed;

        private int _TaskID = 0;
        private string _EBRID = string.Empty;
        private int _VendorRetrys = 1;
        private int MLOG_ID = 0;
        private int _WaitTime = 0;
        private string B_REQ = string.Empty;

        private string stime = string.Empty;
        private string rtime = string.Empty;

        private string sPayload = string.Empty;
        private string B_RES = string.Empty;
        private string PayloadIDTX = string.Empty;
        private string PayloadIDRX = string.Empty;
        private string sPayloadID = string.Empty;

        ~Anthem()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                log.Dispose();

                // free other managed objects that implement
                // IDisposable only
            }



            // release any unmanaged objects
            // set the object references to null

            _disposed = true;
        }

        private string _OrigRes = string.Empty;

        public string OrigRes
        {
            get
            {
                return ss.StripCRLF(_OrigRes);
            }
            set
            {
                try
                {
                    _OrigRes = ss.StripCRLF(value);

                }
                catch (Exception ex)
                {

                }
            }
        }
        public string BHT03
        {

            set
            {

                B_REQ = value;
            }
        }
        public int VendorRetrys
        {

            set
            {

                _VendorRetrys = value;
            }
        }

        public int WaitTime
        {

            set
            {

                _WaitTime = value;
            }
        }

        public int TaskID
        {

            set
            {
                _TaskID = value;
            }
        }

        public string EBRID
        {

            set
            {

                _EBRID = value;
            }
        }

        public string ConnectionString
        {

            set
            {

                _ConnectionString = value;
                log.ConnectionString = value;
            }
        }
        public string ProtocolType
        {

            set
            {

                _ProtocolType = value;
            }
        }
        public int VMetricsLogging
        {

            set
            {

                _VMetricsLogging = value;
            }
        }

        public string wsUrl
        {

            set
            {

                _wsUrl = value;
            }
        }

        public string PayloadID
        {

            set
            {

                _PayloadID = value;
            }
        }


        public string TimeStamp
        {

            set
            {

                _TimeStamp = value;
            }
        }

        public string ProcessingMode
        {

            set
            {

                _ProcessingMode = value;
            }
        }

        public string PayloadType
        {

            set
            {

                _PayloadType = value;
            }
        }

        public string SenderID
        {

            set
            {

                _SenderID = value;
            }
        }

        public string ReceiverID
        {

            set
            {

                _ReceiverID = value;
            }
        }

        public string CORERuleVersion
        {

            set
            {

                _CORERuleVersion = value;
            }
        }

        public string Payload
        {

            set
            {

                _Payload = value;
            }
        }

        public string UserName
        {

            set
            {

                _UserName = value;
            }
        }


        public string Passwd
        {

            set
            {

                _Passwd = value;
            }
        }
        public long SendTimeout
        {

            set
            {

                _ServiceTimeOut = value;
            }
        }
        public long ReceiveTimeout
        {

            set
            {

                _ReceiveTimeout = value;
            }
        }
        public string VendorName
        {

            set
            {

                _VendorName = value;
            }
        }
        public string PayorCode
        {

            set
            {

                _PayorCode = value;
            }
        }

        public string REQ
        {
            get
            {
                return ss.StripCRLF(_REQ);
            }
            set
            {
                try
                {
                    _REQ = ss.StripCRLF(value);

                }
                catch (Exception ex)
                {

                }
            }
        }

        public string RES
        {
            get
            {
                return ss.StripCRLF(_RES);
            }
            set
            {
                try
                {
                    _RES = ss.StripCRLF(value);

                }
                catch (Exception ex)
                {

                }
            }
        }

        public int GetResponse()
        {
            int r = -1;
            int i = 0;
            int VendorTrys = 0;

            while (i < _VendorRetrys)
            {

                r = GoToVendor();


                if (r == 0)
                {
                    VendorTrys++;
                    i = 1 + _VendorRetrys;
                }
                else
                {

                    VendorTrys++;
                    // Console.WriteLine(_EBRID + " retry " + Convert.ToString(VendorTrys) + " Error Message " + ErrorMsg);
                    log.ExceptionDetails(_VendorName, _EBRID + " retry " + Convert.ToString(VendorTrys) + " Error Message " + ErrorMsg);

                    _WaitTime = _WaitTime + 250;
                }
                i++;
            }

            if (_VendorRetrys == VendorTrys)
            {

                log.ExceptionDetails(_VendorName + "  Exceded Ventor Retrys giving up with  VendorTrys  at " + Convert.ToString(VendorTrys), _EBRID);
                r = -5;
            }

            return r;

        }

        public int GoToVendor()
        {
            int r = -1;

            string s1 = null;
            string httpData = string.Empty;
            _RES = string.Empty;

            XmlDocument XMLEDI271 = new XmlDocument();
            string Err = string.Empty;

            DateTime StartTime = DateTime.Now;
            DateTime Endtime = default(DateTime);

            stime = string.Empty;
            rtime = string.Empty;
            string soapResult = string.Empty;

            try
            {

                sPayloadID = Guid.NewGuid().ToString();  //_PayloadID;
                _PayloadID = sPayloadID;

                PayloadIDTX = sPayloadID;

                int m = 0; //rnd.Next(0, 1000);

                Thread.Sleep(_WaitTime);

                if (_ProtocolType.ToUpper() == "TLS12")
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                }
                else if (_ProtocolType.ToUpper() == "TLS11")
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11;
                }
                else if (_ProtocolType.ToUpper() == "TLS")
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                }
                else if (_ProtocolType.ToUpper() == "SSL3")
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                }
                else
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;
                }

                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                //HttpWebRequest webRequest = CreateWebRequest(_wsUrl, _wsUrl);

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(_wsUrl);
                webRequest.ContentType = "application/soap+xml;charset=UTF-8;action=RealTimeTransactionV2.2.0" ;
                webRequest.Method = "POST";
                webRequest.KeepAlive = true;
                webRequest.Credentials = new NetworkCredential(_UserName, _Passwd);
                webRequest.Timeout = 600000;
                string sAuthorization = string.Empty;
                sAuthorization = "login:" + _Passwd;
                var toEncodeAsBytes = System.Text.Encoding.UTF8.GetBytes(sAuthorization);
                var returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
                webRequest.Headers.Add(Convert.ToString("Authorization: Basic ") + returnValue);

                string StrSoapEnvelope = CreateSoapEnvelope(_REQ);
                var stringBytes = System.Text.Encoding.UTF8.GetBytes(StrSoapEnvelope.ToString());

                using (Stream stream = webRequest.GetRequestStream())
                {
                    stream.Write(stringBytes, 0, stringBytes.Length);
                }

                sTimeStamp = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:fffZ");

                stime = sTimeStamp;

                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("ANTHEM  sent at " + Convert.ToString(sTimeStamp) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _REQ);
                }



                using (WebResponse webResponse = webRequest.GetResponse())
                {
                    using (Stream S = webResponse.GetResponseStream())
                    {
                        StreamReader sr = new StreamReader(S);
                        soapResult = sr.ReadToEnd();
                        _OrigRes = soapResult;
                    }
                }
              

                rtime = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:fffZ");

               
                if (!string.IsNullOrEmpty(soapResult))
                {
                    try
                    {
                        XMLEDI271.LoadXml(soapResult);
                        XmlNodeList elemList = XMLEDI271.GetElementsByTagName("Payload");
                        if (elemList.Count > 0)
                        {
                            _RES = elemList[0].InnerText;

                            sPayload = _RES;
                            r = 0;
                            if (_VMetricsLogging == 1)
                            {
                                log.ExceptionDetails("ANTHEM REcived  at " + Convert.ToString(rtime) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _RES);
                            }
                        }
                        else
                        {
                            _RES = soapResult;
                            MLOG_ID = log.MLOG(_VendorName, ErrorMsg, _EBRID, stime, rtime, B_RES, B_REQ, PayloadIDTX, PayloadIDRX, soapResult, _REQ);
                           r = -2; 

                        }
                         
                        using (ValidateEDI vedi = new ValidateEDI())
                        {
                            vedi.ConnectionString = _ConnectionString;
                            vedi.byString(sPayload);
                            B_RES = vedi.ReferenceIdentification;
                        }
                         
                        if (B_RES != B_REQ)
                        {
                            MLOG_ID = log.MLOG(_VendorName, ErrorMsg, _EBRID, stime, rtime, B_RES, B_REQ, PayloadIDTX, PayloadIDRX, sPayload, _REQ);
                            r = -4;
                            log.ExceptionDetails(_VendorName + " BHT03 mismatch req and res are in MLOG with MLOG_ID " + Convert.ToString(MLOG_ID), Convert.ToString(DateTime.Now) + " | PayloadID=" + sPayloadID + " | Error Status= " + ErrorMsg + " Response = " + sPayload + " | Vendor Name =  " + _VendorName + "| Payor Code=" + _PayorCode + "|" + "waitstate=" + Convert.ToString(m) + "|" + _wsUrl);

                        }
                       
                        if (r < -3)   //we don't have Request GUID & Response GUID.
                        {
                            if (PayloadIDTX != PayloadIDRX)
                            {
                                MLOG_ID = log.MLOG(_VendorName, ErrorMsg, _EBRID, stime, rtime, B_RES, B_REQ, PayloadIDTX, PayloadIDRX, sPayload, _REQ);
                                r = -5;
                                log.ExceptionDetails(_VendorName + "  Payload mismatch req and res are in MLOG with MLOG_ID " + Convert.ToString(MLOG_ID), Convert.ToString(DateTime.Now) + " | PayloadID=" + sPayloadID + " | Error Status= " + ErrorMsg + " Response = " + sPayload + " | Vendor Name =  " + _VendorName + "| Payor Code=" + _PayorCode + "|" + "waitstate=" + Convert.ToString(m) + "|" + _wsUrl);

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        r = -3;
                        MLOG_ID = log.MLOG(_VendorName, ErrorMsg, _EBRID, stime, rtime, B_RES, B_REQ, PayloadIDTX, PayloadIDRX, sPayload, _REQ);
                        log.ExceptionDetails("ANTHEM Reading Response Failed ", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                r = -1;
                Endtime = DateTime.Now;
                _elapsedTicks = Endtime.Ticks - StartTime.Ticks;
                log.ExceptionDetails("ANTHEM GetResponse Failed Total Elasped ticks" + Convert.ToString(_elapsedTicks) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl + "|" + _REQ, ex);
            }
            return r;
        }

        

        private string CreateSoapEnvelope(string edi)
        {

            string soap = "";

            Guid g;
            g = Guid.NewGuid();

          

            soap = "";

            //soap = soap + Convert.ToString("<soapenv:Envelope xmlns:soapenv='http://www.w3.org/2003/05/soap-envelope'>");
            //soap = soap + Convert.ToString("<soapenv:Header>");
            //soap = soap + Convert.ToString("<wsse:Security soapenv:mustUnderstand='true' xmlns:wsse='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd' >");
            //soap = soap + Convert.ToString("<wsse:UsernameToken xmlns:wsu='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wsswssecurity-utility-1.0.xsd'  wsu:Id='UsernameToken-21621663'>");
            //soap = soap + Convert.ToString("<wsse:Username>RT00452C</wsse:Username>");
            //soap = soap + Convert.ToString("<wsse:Password Type='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-usernametoken-profile-1.0#PasswordText'>27xaccess</wsse:Password>");
            //soap = soap + Convert.ToString("</wsse:UsernameToken>");
            //soap = soap + Convert.ToString("</wsse:Security>");
            //soap = soap + Convert.ToString("</soapenv:Header>");
            //soap = soap + Convert.ToString("<soapenv:Body>");
            //soap = soap + Convert.ToString("<ns1:COREEnvelopeRealTimeRequest xmlns:ns1='http://www.caqh.org/SOAP/WSDL/CORERule2.2.0.xsd'>");
            //soap = soap + Convert.ToString("<PayloadType>" + _PayloadType + "</PayloadType>");
            //soap = soap + Convert.ToString("<ProcessingMode>RealTime</ProcessingMode>");
            //soap = soap + Convert.ToString("<PayloadID>" + sPayloadID + "</PayloadID>");
            //soap = soap + Convert.ToString("<TimeStamp>" + DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:22Z") + "</TimeStamp>");
            //soap = soap + Convert.ToString("<SenderID>" + _SenderID + "</SenderID>");
            //soap = soap + Convert.ToString("<ReceiverID>" + _ReceiverID + "</ReceiverID>");
            //soap = soap + Convert.ToString("<CORERuleVersion>2.2.0</CORERuleVersion>");
            //soap = soap + Convert.ToString("<Payload>" + edi + "</Payload>");
            //'soap = (soap  +  Convert.ToString("<Payload><![CDATA[")) + EDI + "]]></Payload>"
            //soap = soap + Convert.ToString("</ns1:COREEnvelopeRealTimeRequest>");
            //soap = soap + Convert.ToString("</soapenv:Body>");
            //soap = soap + Convert.ToString("</soapenv:Envelope>");

            soap = soap + Convert.ToString("<soap:Envelope xmlns:soap='http://www.w3.org/2003/05/soap-envelope' xmlns:cor='http://www.caqh.org/SOAP/WSDL/CORERule2.2.0.xsd'>");
            soap = soap + Convert.ToString("<soap:Header>");
            soap = soap + Convert.ToString("<wsse:Security xmlns:wsse='http://docs.oasis-open.org/wss/oasis-wss-wssecurity-secext-1.1.xsd'>");
            soap = soap + Convert.ToString("<wsse:UsernameToken wsu:Id='UsernameToken-1' xmlns:wsu='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd'>");
            soap = soap + Convert.ToString("<wsse:Username>" +  _UserName  + "</wsse:Username>");
            soap = soap + Convert.ToString("<wsse:Password Type='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText'>" +  _Passwd   + "</wsse:Password>");
            //soap = soap + Convert.ToString("<wsse:Nonce EncodingType='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary'>A7bj5aHws8VzkybUygCO3Q==</wsse:Nonce>");
            //soap = soap + Convert.ToString("<wsu:Created>2010-07-28T20:49:11.194Z</wsu:Created>");
            soap = soap + Convert.ToString("</wsse:UsernameToken>");
            soap = soap + Convert.ToString("</wsse:Security>");
            soap = soap + Convert.ToString("</soap:Header>");
            soap = soap + Convert.ToString("<soap:Body>");
            soap = soap + Convert.ToString("<cor:COREEnvelopeRealTimeRequest>");
            soap = soap + Convert.ToString("<PayloadType>" + _PayloadType + "</PayloadType>");
            soap = soap + Convert.ToString("<ProcessingMode>RealTime</ProcessingMode>");
            soap = soap + Convert.ToString("<PayloadID>" + sPayloadID + "</PayloadID>");
            soap = soap + Convert.ToString("<TimeStamp>" + DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:22Z") + "</TimeStamp>");
            soap = soap + Convert.ToString("<SenderID>" +  _SenderID    + "</SenderID>");
            soap = soap + Convert.ToString("<ReceiverID>" + _ReceiverID + "</ReceiverID>");
            soap = soap + Convert.ToString("<CORERuleVersion>2.2.0</CORERuleVersion>");
            soap = soap + Convert.ToString("<Payload>" + edi + "</Payload>");
            soap = soap + Convert.ToString("</cor:COREEnvelopeRealTimeRequest>");
            soap = soap + Convert.ToString("</soap:Body>");
            soap = soap + Convert.ToString("</soap:Envelope>");

            return soap;


        }



        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }

        }

        private static string CleanResponse(string haystack, string needle1, string needle2)
        {
            //int istart = String.InStr(haystack, needle1);
            int istart = haystack.IndexOf(needle1, 0);
            if (istart > 0)
            {
                //int istop = String.InStr(istart, haystack, needle2);
                int istop = haystack.IndexOf(needle2, istart);
                if (istop > 0)
                {
                    //string value = haystack.Substring(istart + Strings.Len(needle1) - 1, istop - istart - Strings.Len(needle1));
                    string value = haystack.Substring(istart + needle1.Length, istop - istart - needle1.Length);
                    return value;
                }
            }
            return null;
        }
    }
    
}
