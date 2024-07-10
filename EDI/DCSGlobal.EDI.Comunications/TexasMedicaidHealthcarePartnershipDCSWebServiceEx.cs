using System;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;
using DCSGlobal.EDI.Comunications;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using DCSGlobal.BusinessRules.Logging;
using System.Xml;
using System.Security.Cryptography.X509Certificates;
namespace DCSGlobal.EDI.Comunications
{
    class TexasMedicaidHealthcarePartnershipDCSWebServiceEx : IDisposable
    {
        private StringStuff ss = new StringStuff();
        private logExecption log = new logExecption();

        private string _wsUrl = string.Empty;
        private string _ContentType = string.Empty;
        private string _Method = string.Empty;
        private int _ServiceTimeOut = 0;

        private string _ConnectionString = string.Empty;
        private static string _login = string.Empty;
        private static string _password = string.Empty;

        long _elapsedTicks = 0;

        private string _RequestContent = string.Empty;
        private string _vendor_input_type = string.Empty;

        private string _VendorInputType = string.Empty;
        private string _VendorName = string.Empty;
        private string _PayorCode = string.Empty;

        private string _ProtocolType = string.Empty;
 
        private string _REQ = string.Empty;
        private string _RES = string.Empty;
        private int _VMetricsLogging = 0;
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


        ~TexasMedicaidHealthcarePartnershipDCSWebServiceEx()
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

        public string ConnectionString
        {

            set
            {

                _ConnectionString = value;
                log.ConnectionString = value;
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




        public string password
        {

            set
            {

                _password = value;
            }
        }



        public string login
        {

            set
            {

                _login = value;
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

        public string ContentType
        {

            set
            {

                _ContentType = value;
            }
        }


        public string Method
        {

            set
            {

                _Method = value;
            }
        }

        private HttpWebRequest CreateWebRequest(string url)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.ContentType = _ContentType;
            webRequest.Method = _Method;
            webRequest.Timeout = _ServiceTimeOut;

            //webRequest.ContentType = "text/xml;charset='utf-8'";
            //webRequest.Method =  "POST";
            //webRequest.Timeout = _ServiceTimeOut;  


            return webRequest;
        }

        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
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
         
            _RES = string.Empty;
            DateTime StartTime = DateTime.Now;
            DateTime Endtime = default(DateTime);

            string httpData = null;
           
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

                log.ConnectionString = _ConnectionString;

                //Random rnd = new Random();
                int m = 0; //rnd.Next(0, 1000);
                //  m = m * 5;
                Thread.Sleep(_WaitTime);

                sTimeStamp = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:fffZ");


                //log.ExceptionDetails("5.TXMCDWS-GetResponse", Convert.ToString(_wsUrl));

                XmlDocument soapEnvelopeXml = CreateSoapEnvelope(_REQ);

                //log.ExceptionDetails("6.CreateSoapEnvelope", Convert.ToString(wsUrl));

                HttpWebRequest webRequest = CreateWebRequest(_wsUrl);

                //log.ExceptionDetails("7.CreateWebRequest", Convert.ToString(wsUrl));

                InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

                //log.ExceptionDetails("8.Build Soap Envelop", Convert.ToString(wsUrl));

                HttpWebResponse resp = (HttpWebResponse)webRequest.GetResponse();

                //log.ExceptionDetails("10.HttpWebResponse-", Convert.ToString(wsUrl));

                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("TXMCD_DCSWS Sent at " + Convert.ToString(sTimeStamp) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _REQ);
                }

                stime = sTimeStamp;

                Stream S = resp.GetResponseStream();
                //log.ExceptionDetails("11.GetResponseStream", Convert.ToString(wsUrl));
                StreamReader sr = new StreamReader(S);
                //log.ExceptionDetails("12.Stream reader", Convert.ToString(wsUrl));
                dynamic _with3 = sr;
                httpData = _with3.ReadToEnd();
                _OrigRes = httpData;
                //log.ExceptionDetails("13.httpData", Convert.ToString(httpData));
                _with3.Close();
                S.Close();
                resp.Close();

                rtime = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:fffZ");


                XmlDocument XMLEDI271 = new XmlDocument();
                XMLEDI271.LoadXml(httpData);
                XmlNodeList elemList = XMLEDI271.GetElementsByTagName("getEligVendorEdiResponseResult");
                if (elemList.Count > 0)
                {
                    _RES = elemList[0].InnerText;

                    sPayload = _RES;

                    r = 0;
                    if (_VMetricsLogging == 1)
                    {
                        log.ExceptionDetails("TXMCD_DCSWS REcived  at " + Convert.ToString(rtime) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _RES);
                    }
                }
                else
                {

                    _RES = httpData;
                    MLOG_ID = log.MLOG(_VendorName, ErrorMsg, _EBRID, stime, rtime, B_RES, B_REQ, PayloadIDTX, PayloadIDRX, _RES, _REQ);
                    r = -5;
                    log.ExceptionDetails("13.r=-1", Convert.ToString(r));
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
                    r = -2;
                }

                if (B_RES != B_REQ)
                {
                    MLOG_ID = log.MLOG(_VendorName, ErrorMsg, _EBRID, stime, rtime, B_RES, B_REQ, PayloadIDTX, PayloadIDRX, sPayload, _REQ);
                    r = -3;
                    log.ExceptionDetails(_VendorName + " BHT03 mismatch req and res are in MLOG with MLOG_ID " + Convert.ToString(MLOG_ID), Convert.ToString(DateTime.Now) + " | PayloadID=" + sPayloadID + " | Error Status= " + ErrorMsg + " Response = " + sPayload + " | Vendor Name =  " + _VendorName + "| Payor Code=" + _PayorCode + "|" + "waitstate=" + Convert.ToString(m) + "|" + _wsUrl);

                }

                if (r < -4)   //we don't have Request GUID & Response GUID.
                {
                    //if (PayloadIDTX != PayloadIDRX)
                    //{
                    //    MLOG_ID = log.MLOG(_VendorName, ErrorMsg, _EBRID, stime, rtime, B_RES, B_REQ, PayloadIDTX, PayloadIDRX, sPayload, _REQ);
                    //    r = -4;
                    //    log.ExceptionDetails(_VendorName + "  Payload mismatch req and res are in MLOG with MLOG_ID " + Convert.ToString(MLOG_ID), Convert.ToString(DateTime.Now) + " | PayloadID=" + sPayloadID + " | Error Status= " + ErrorMsg + " Response = " + sPayload + " | Vendor Name =  " + _VendorName + "| Payor Code=" + _PayorCode + "|" + "waitstate=" + Convert.ToString(m) + "|" + _wsUrl);

                    //}
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

        private static XmlDocument CreateSoapEnvelope(string edi)
        {

            string soap = "";
            XmlDocument soapEnvelop = new XmlDocument();

            soap = "";

            soap = soap + Convert.ToString("<soap:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'>");
            soap = soap + Convert.ToString("<soap:Body>");
            soap = soap + Convert.ToString("<getEligVendorEdiResponse xmlns='http://tempuri.org/'>");
            soap = soap + Convert.ToString("<login>" + _login + "</login>");
            soap = soap + Convert.ToString("<password>" + _password + "</password>");
            soap = soap + Convert.ToString("<RequestContent>" + edi + "</RequestContent>");
            soap = soap + Convert.ToString("</getEligVendorEdiResponse>");
            soap = soap + Convert.ToString("</soap:Body>");
            soap = soap + Convert.ToString("</soap:Envelope>");


            soapEnvelop.LoadXml(soap);

            return soapEnvelop;


        }

    }
}
