using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using DCSGlobal.BusinessRules.Logging;
using System.Threading;


namespace DCSGlobal.EDI.Comunications
{
    class GeorgiaMedicaidEx : IDisposable
    {

        private StringStuff ss = new StringStuff();
        private logExecption log = new logExecption();

        private string _VendorInputType = string.Empty;
        private string _VendorName = string.Empty;
        private string _PayorCode = string.Empty;
        private string _ProtocolType = string.Empty;

        private string _REQ = string.Empty;
        private string _RES = string.Empty;

        private int _VMetricsLogging = 0;
        private string _wsUrl = string.Empty;
        private string _ContentType = string.Empty;
        private string _Method = string.Empty;
        private int _ServiceTimeOut = 0;
        private string _Accept = string.Empty;
        private string _LogToREQRES = string.Empty;

        private string _ConnectionString = string.Empty;

        private string _PayloadID = string.Empty;
        private string _TimeStamp = string.Empty;
        private string _ProcessingMode = string.Empty;
        private string _PayloadType = string.Empty;
        private string _SenderID = string.Empty;
        private string _ReceiverID = string.Empty;
        private string _CORERuleVersion = string.Empty;
        private string _Payload = string.Empty;

        private string _UserName = string.Empty;
        private string _Passwd = string.Empty;

        long _elapsedTicks = 0;

        bool _disposed;

        private int _TaskID = 0;
        private string _EBRID = string.Empty;
        private int _VendorRetrys = 1;
        private int MLOG_ID = 0;
        private int _WaitTime = 0;
        private string B_REQ = string.Empty;
        private string ErrorMsg = "";
        private string stime = string.Empty;
        private string rtime = string.Empty;
        private string sTimeStamp = string.Empty;
        private string sPayload = string.Empty;
        private string B_RES = string.Empty;
        private string PayloadIDTX = string.Empty;
        private string PayloadIDRX = string.Empty;
        private string sPayloadID = string.Empty;


        ~GeorgiaMedicaidEx()
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

        public string VendorInputType
        {

            set
            {

                _VendorInputType = value;
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

        public string ContentType
        {

            set
            {

                _ContentType = value;
            }
        }





        public string Accept
        {

            set
            {

                _Accept = value;
            }
        }



        public string Method
        {

            set
            {

                _Method = value;
            }
        }

        public string wsUrl
        {
            set
            {
                _wsUrl = value;
            }
        }

        public int ServiceTimeOut
        {

            set
            {

                _ServiceTimeOut = value;
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
            string httpData = null;
            string s1 = null;

            _RES = string.Empty;

            XmlDocument XMLEDI271 = new XmlDocument();
            string Err = string.Empty;
            string errMsg = string.Empty;
            DateTime StartTime = DateTime.Now;
            DateTime Endtime = default(DateTime);

            stime = string.Empty;
            rtime = string.Empty;

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
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;
                }


                sPayloadID = Guid.NewGuid().ToString();  //_PayloadID;
                _PayloadID = sPayloadID;

                PayloadIDTX = sPayloadID;

                //wsUrl = "https://hts.betammis.georgia.gov/GA_ACA1104WCFServices/GA_ACA1104WCFServices.svc";
                //wsUrl = "https://hts.mmis.georgia.gov/GA_ACA1104WCFServices/GA_ACA1104WCFServices.svc"

                //Random rnd = new Random();
                int m = 0; //rnd.Next(0, 1000);
                //  m = m * 5;
                Thread.Sleep(_WaitTime);

              
                XmlDocument soapEnvelopeXml = CreateGASoapEnvelope(_REQ);


                HttpWebRequest webRequest = CreateGAWebRequest(_wsUrl);
                InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

                sTimeStamp = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:fffZ");

                stime = sTimeStamp;

                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("GA MEDICAID sent at " + Convert.ToString(sTimeStamp) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _REQ);
                }

                HttpWebResponse resp = (HttpWebResponse)webRequest.GetResponse();


                Stream S = resp.GetResponseStream();
                StreamReader sr = new StreamReader(S);
                dynamic _with3 = sr;
                httpData = _with3.ReadToEnd();
                _OrigRes = httpData;
                rtime = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:fffZ");


                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("GA Original http Data REcived  at " + Convert.ToString(rtime) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, httpData);
                }


                _with3.Close();
                S.Close();
                resp.Close();

                if (!string.IsNullOrEmpty(httpData))
                {
                    XMLEDI271.LoadXml(httpData);
                    Err = XMLEDI271.GetElementsByTagName("ErrorCode")[0].InnerText;
                    errMsg = XMLEDI271.GetElementsByTagName("ErrorMessage")[0].InnerText;
                    PayloadIDRX = XMLEDI271.GetElementsByTagName("PayloadID")[0].InnerText;
                    if (Err == "Success")
                    {
                        s1 = XMLEDI271.GetElementsByTagName("Payload")[0].InnerText;
                        _RES = s1;
                        sPayload = _RES;
                        if (_VMetricsLogging == 1)
                        {
                            log.ExceptionDetails("GA REcived  at " + Convert.ToString(rtime) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _RES);
                        }
                        r = 0;
                    }
                    else
                    {
                        MLOG_ID = log.MLOG(_VendorName, errMsg, _EBRID, stime, rtime, B_RES, B_REQ, PayloadIDTX, PayloadIDRX, _RES, _REQ);
                        r = -2;
                    }
                }
 
                using (ValidateEDI vedi = new ValidateEDI())
                {
                    vedi.ConnectionString = _ConnectionString;
                    vedi.byString(sPayload);
                    B_RES = vedi.ReferenceIdentification;
                }

               
                if (!_RES.Contains("ISA"))
                {
                    MLOG_ID = log.MLOG(_VendorName, ErrorMsg, _EBRID, stime, rtime, B_RES, B_REQ, PayloadIDTX, PayloadIDRX, _RES, _REQ);
                    r = -3;
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
                r = -1;
                Endtime = DateTime.Now;
                _elapsedTicks = Endtime.Ticks - StartTime.Ticks;
                log.ExceptionDetails("GetResponse Failed Total Elasped ticks" + Convert.ToString(_elapsedTicks) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl + "|" + _REQ, ex);
            }
            return r;
        }

        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {


                soapEnvelopeXml.Save(stream);
            }

        }

        private XmlDocument CreateGASoapEnvelope(string edi)
        {
           
            string timestamp = string.Empty;

            string soap = "";

            timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:22Z");

            XmlDocument soapEnvelop = new XmlDocument();


            soap = "";

            soap = soap + Convert.ToString("<soapenv:Envelope xmlns:soapenv=\"http://www.w3.org/2003/05/soap-envelope\">");

            soap = soap + Convert.ToString("<soapenv:Header>");

            soap = soap + Convert.ToString("<wsse:Security xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" soapenv:mustUnderstand=\"true\">");

            soap = soap + Convert.ToString("<wsse:UsernameToken xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wsswssecurity-utility-1.0.xsd\"  wsu:Id=\"UsernameToken-21621663\">");

            soap = soap + Convert.ToString("<wsse:Username>" + _UserName + "</wsse:Username>");

            soap = soap + Convert.ToString("<wsse:Password Type=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-usernametoken-profile-1.0#PasswordText\">" + _Passwd + "</wsse:Password>");

            soap = soap + Convert.ToString("</wsse:UsernameToken>");

            soap = soap + Convert.ToString("</wsse:Security>");

            soap = soap + Convert.ToString("</soapenv:Header>");

            soap = soap + Convert.ToString("<soapenv:Body>");

            soap = soap + Convert.ToString("<ns1:COREEnvelopeRealTimeRequest xmlns:ns1=\"http://www.caqh.org/SOAP/WSDL/CORERule2.2.0.xsd\">");

            soap = soap + Convert.ToString("<PayloadType>" + _PayloadType + "</PayloadType>");

            soap = soap + Convert.ToString("<ProcessingMode>" + _ProcessingMode + "</ProcessingMode>");

            soap = soap + Convert.ToString("<PayloadID>" + Convert.ToString(_PayloadID) + "</PayloadID>");

            soap = soap + Convert.ToString("<TimeStamp>" + timestamp + "</TimeStamp>");

            soap = soap + Convert.ToString("<SenderID>" + _SenderID + "</SenderID>");

            soap = soap + Convert.ToString("<ReceiverID>" + _ReceiverID + "</ReceiverID>");

            soap = soap + Convert.ToString("<CORERuleVersion>" + _CORERuleVersion + "</CORERuleVersion>");

            soap = soap + Convert.ToString("<Payload><![CDATA[") + edi + "]]></Payload>";

            soap = soap + Convert.ToString("</ns1:COREEnvelopeRealTimeRequest>");

            soap = soap + Convert.ToString("</soapenv:Body>");

            soap = soap + Convert.ToString("</soapenv:Envelope>");



            soapEnvelop.LoadXml(soap);

            return soapEnvelop;

        }

        private HttpWebRequest CreateGAWebRequest(string url)
        {

            HttpWebRequest webRequest__1 = (HttpWebRequest)WebRequest.Create(url);
            webRequest__1.ContentType = "application/soap+xml;charset=UTF-8;action=RealTimeTransaction";
            webRequest__1.Accept = "text/xml";
            webRequest__1.Method = "POST";
            webRequest__1.Timeout = _ServiceTimeOut;

            return webRequest__1;

        }

    }
}
