using System.IO;
using System.Text;
using System.Threading;
using DCSGlobal.BusinessRules.Logging;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
  

namespace DCSGlobal.EDI.Comunications 
{
     class BCBSLouisiana : IDisposable
    {
        private logExecption log = new logExecption();
        private StringStuff ss = new StringStuff();

        private string _ConnectionString = string.Empty;
        private string _VendorName = "BCBSLA";
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


        ~BCBSLouisiana()
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

            XmlDocument xd = new XmlDocument();
            string Err = string.Empty;

            DateTime StartTime = DateTime.Now;
            DateTime Endtime = default(DateTime);

            stime = string.Empty;
            rtime = string.Empty;
            string soapResult = string.Empty;
            try
            {

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
                 //   ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;
                }

                sPayloadID = Guid.NewGuid().ToString();  //_PayloadID;
                _PayloadID = sPayloadID;

                PayloadIDTX = sPayloadID;

                int m = 0; //rnd.Next(0, 1000);

                Thread.Sleep(_WaitTime);

               // ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
                //ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

                XmlDocument soapEnvelopeXml = CreateSoapEnvelope(_REQ);
                HttpWebRequest webRequest = CreateWebRequest(_wsUrl, _wsUrl);
                InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

                sTimeStamp = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:fffZ");

                stime = sTimeStamp;

                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("BCBS Louisiana  MEDICAID sent at " + Convert.ToString(sTimeStamp) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _REQ);
                }


                HttpWebResponse resp = (HttpWebResponse)webRequest.GetResponse();
                Stream S = resp.GetResponseStream();
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader sr = new StreamReader(S, encode);

                dynamic _with3 = sr;
                soapResult = _with3.ReadToEnd();
                _OrigRes = soapResult;

                rtime = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:fffZ");

                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("BCBS Louisiana  Response Recived  at " + Convert.ToString(DateTime.Now), soapResult);
                }



                if (!string.IsNullOrEmpty(soapResult))
                {
                    try
                    {
                        httpData = CleanResponse(soapResult, "<s:Body>", "</s:Body>");

                        xd.LoadXml(httpData);
                        var xnError = xd.DocumentElement.GetElementsByTagName("b:ErrorCode");
                        if (xnError != null)
                        {
                            //Don't check error tag. Because, we are getting valid response with VT04_034 error code.
                            //Err = xnError[0].InnerText;
                            //if (_VMetricsLogging == 1 && Err != "Success")
                            //{
                            //    log.ExceptionDetails("AVAILITY Response ErrorCode ", Err);
                            //}
                        }
                        var payLoad = xd.DocumentElement.GetElementsByTagName("b:Payload");
                        if (payLoad != null)
                        {
                            _RES = payLoad[0].InnerText.Replace("<![CDATA[", "").Replace("]]>", "");
                            sPayload = _RES;
                            r = 0;
                            if (_VMetricsLogging == 1)
                            {
                                log.ExceptionDetails("AVAILITY Response Payload  ", _RES);
                            }
                        }

                        if (Err != string.Empty && Err != "Success")
                        {
                            MLOG_ID = log.MLOG(_VendorName, Err, _EBRID, stime, rtime, B_RES, B_REQ, PayloadIDTX, PayloadIDRX, sPayload, _REQ);
                            // log.ExceptionDetails(_VendorName + " ErrorMSG = '" + ErrorMsg + "'  req and res are in MLOG with MLOG_ID " + Convert.ToString(MLOG_ID), Convert.ToString(DateTime.Now) + " | PayloadID=" + sPayloadID + " | Error Status= " + ErrorMsg + " Response = " + sPayload + " | Vendor Name =  " + _VendorName + "| Payor Code=" + _PayorCode + "|" + "waitstate=" + Convert.ToString(m) + "|" + _wsUrl);
                            r = -2;
                        }
                        if (!_RES.Contains("ISA"))
                        {
                            MLOG_ID = log.MLOG(_VendorName, ErrorMsg, _EBRID, stime, rtime, B_RES, B_REQ, PayloadIDTX, PayloadIDRX, _RES, _REQ);
                            r = -3;
                        }


                        var oPayloadID = xd.DocumentElement.GetElementsByTagName("b:PayloadID");
                        if (oPayloadID != null)
                        {
                            PayloadIDRX = oPayloadID[0].InnerText;
                        }

                        var oPayloadType = xd.DocumentElement.GetElementsByTagName("b:PayloadType");
                        if (oPayloadType != null)
                        {
                            sPayloadType = oPayloadType[0].InnerText;
                        }

                        using (ValidateEDI vedi = new ValidateEDI())
                        {
                            vedi.ConnectionString = _ConnectionString;
                            vedi.byString(sPayload);
                            B_RES = vedi.ReferenceIdentification;
                        }


                        if (B_RES != B_REQ)
                        {

                            MLOG_ID = log.MLOG(_VendorName, Err, _EBRID, stime, rtime, B_RES, B_REQ, PayloadIDTX, PayloadIDRX, sPayload, _REQ);
                            r = -4;
                            log.ExceptionDetails(_VendorName + " BHT03 mismatch req and res are in MLOG with MLOG_ID " + Convert.ToString(MLOG_ID), Convert.ToString(DateTime.Now) + " | PayloadID=" + sPayloadID + " | Error Status= " + ErrorMsg + " Response = " + sPayload + " | Vendor Name =  " + _VendorName + "| Payor Code=" + _PayorCode + "|" + "waitstate=" + Convert.ToString(m) + "|" + _wsUrl);

                        }

                        if (r < -3)
                        {
                            if (PayloadIDTX != PayloadIDRX)
                            {
                                MLOG_ID = log.MLOG(_VendorName, Err, _EBRID, stime, rtime, B_RES, B_REQ, PayloadIDTX, PayloadIDRX, sPayload, _REQ);
                                //    log.MLOG(_EBRID, "", "", PayloadIDTX, PayloadIDRX, sPayload);
                                r = -5;
                                log.ExceptionDetails(_VendorName + "  Payload mismatch req and res are in MLOG with MLOG_ID " + Convert.ToString(MLOG_ID), Convert.ToString(DateTime.Now) + " | PayloadID=" + sPayloadID + " | Error Status= " + ErrorMsg + " Response = " + sPayload + " | Vendor Name =  " + _VendorName + "| Payor Code=" + _PayorCode + "|" + "waitstate=" + Convert.ToString(m) + "|" + _wsUrl);

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        r = -3;
                        log.ExceptionDetails("AVAILITY Reading Response Failed ", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                r = -1;
                Endtime = DateTime.Now;
                _elapsedTicks = Endtime.Ticks - StartTime.Ticks;
                log.ExceptionDetails("BCBS Louisiana  MCD GetResponse Failed Total Elasped ticks" + Convert.ToString(_elapsedTicks) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl + "|" + _REQ, ex);
            }
            return r;
        }

        private static HttpWebRequest CreateWebRequest(string url, string action)
        {

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);

            webRequest.ContentType = "application/soap+xml;charset=UTF-8;";  //action='RealTimeTransaction'

            webRequest.Accept = "text/xml";

            webRequest.Method = "POST";

            webRequest.Timeout = 600000;

            return webRequest;
        }
   

        private XmlDocument CreateSoapEnvelope(string edi)
        {

            string soap = "";

            Guid g;
           
            XmlDocument soapEnvelop = new XmlDocument();

            soap = soap + "<soap:Envelope xmlns:soap='http://www.w3.org/2003/05/soap-envelope' xmlns:tem='http://tempuri.org/' xmlns:bcb='http://schemas.datacontract.org/2004/07/Bcbsla.CoreRules.Service.BusinessEntities'>";
            soap = soap + "<soap:Header xmlns:wsa='http://www.w3.org/2005/08/addressing'>";
            soap = soap + "<wsa:Action>http://tempuri.org/CORETransactions/RealTimeTransaction</wsa:Action>";
            soap = soap + "<wsa:To>https://exportalservices.bcbsla.com/CoreRules/CoreRuleService.svc</wsa:To>";
            soap = soap + "</soap:Header> ";
            soap = soap + "<soap:Body>";            soap = soap + "<tem:RealTimeTransaction>";
            soap = soap + "<tem:request>";
            soap = soap + "<bcb:COREEnvelopeRealTimeRequest>";
            soap = soap + "<bcb:CORERuleVersion>2.2.0</bcb:CORERuleVersion>";
            soap = soap + "<bcb:IsSoapRequest>true</bcb:IsSoapRequest>";
            soap = soap + "<bcb:Password>" + _Passwd + "</bcb:Password>";
            soap = soap + "<bcb:Payload>" + edi + "</bcb:Payload>";
            soap = soap + "<bcb:PayloadID>" + sPayloadID + "</bcb:PayloadID>";
            soap = soap + "<bcb:PayloadType>X12_270_Request_005010X279A1</bcb:PayloadType>";
            soap = soap + "<bcb:ProcessingMode>RealTime</bcb:ProcessingMode>";
            soap = soap + "<bcb:ReceiverID>" + _ReceiverID + "</bcb:ReceiverID>";
            soap = soap + "<bcb:SenderID>" + _SenderID + "</bcb:SenderID>";
            soap = soap + "<bcb:TimeStamp>" + DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:22Z") + "</bcb:TimeStamp>";
            soap = soap + "<bcb:UserName>" + _UserName +   "</bcb:UserName>";
            soap = soap + "</bcb:COREEnvelopeRealTimeRequest>";
            soap = soap + "</tem:request>";
            soap = soap + "</tem:RealTimeTransaction>";
            soap = soap + "</soap:Body>";
            soap = soap + "</soap:Envelope>";
              
            soapEnvelop.LoadXml(@soap);

        return soapEnvelop;
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
