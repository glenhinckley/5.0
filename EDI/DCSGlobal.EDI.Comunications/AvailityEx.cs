using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Threading;
using DCSGlobal.EDI.Comunications;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using DCSGlobal.BusinessRules.Logging;


namespace DCSGlobal.EDI.Comunications
{
    class AvailityEx : IDisposable
    {


        private StringStuff ss = new StringStuff();
        private logExecption log = new logExecption();



        private string _ConnectionString = string.Empty;

        private string _wsUrl = string.Empty;
        private string _UserName = string.Empty;
        private string _Passwd = string.Empty;
        private string _apiKey = string.Empty;
        private string _ContentType = string.Empty;
        private string _Method = string.Empty;
        private int _ServiceTimeOut = 0;
        private string _LogToREQRES = string.Empty;
        private string _Token = string.Empty;

        private string _VendorInputType = string.Empty;
        private string _VendorName = string.Empty;
        private string _PayorCode = string.Empty;

        private string _AppName = string.Empty;

        private string _PayloadID = string.Empty;
        private string _PayloadType = string.Empty;


        private string _REQ = string.Empty;
        private string _RES = string.Empty;

        private TimeSpan _ElapsedSpan = new TimeSpan();

        long _elapsedTicks = 0;
        private int _VMetricsLogging = 0;
        private string _ProtocolType = string.Empty;

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



        ~AvailityEx()
        {
            Dispose(false);
        }

        bool _disposed;

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

        public string PayloadID
        {

            set
            {

                _PayloadID = value;
            }
        }


        public string PayloadType
        {

            set
            {

                _PayloadType = value;
            }
        }




        public string wsUrl
        {

            set
            {

                _wsUrl = value;
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


        public string apiKey
        {

            set
            {

                _apiKey = value;
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



        public int ServiceTimeOut
        {

            set
            {

                _ServiceTimeOut = value;
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



        public string VendorInputType
        {

            set
            {
                _VendorInputType = value;
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
                    log.ExceptionDetails("AVAILITY.StripREQ Failed", ex);
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
                    log.ExceptionDetails("AVAILITY.StripRES Failed", ex);
                }
            }
        }


        public long ElaspedTicks
        {

            get
            {
                return _elapsedTicks;

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


        //public class B2BXmlClient
        //{

        /// <summary>
        /// Send 270/276 transaction to Aries realtime web service
        /// </summary>
        /// <param name="payloadId">unique identifier</param>
        /// <param name="payload">ANSI 270 or 276 request</param>
        /// <param name="transactionCode">transaction code (either '270' or '276')</param>
        /// <param name="receiverCode">AVAILITY</param>
        /// <returns>ANSI 271 or 277 response</returns>
        public int GoToVendor()
        {


            int r = -1;
            DateTime StartTime = DateTime.Now;
            DateTime Endtime;


            string url = string.Empty;
            _RES = string.Empty;

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

                var httpRequest = (HttpWebRequest)WebRequest.Create(_wsUrl);
                httpRequest.Method = "POST";
                httpRequest.ContentType = "application/x-www-form-urlencoded";
                httpRequest.Timeout = _ServiceTimeOut;
                httpRequest.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;




                var xd = new XmlDocument();

                if (_REQ.StartsWith("ISA"))
                {

                    xd.LoadXml("<?xml version=\"1.0\"?><ENVELOPE/>");
                    var xnHeader = xd.DocumentElement.AppendChild(xd.CreateElement("HEADER"));
                    xnHeader.AppendChild(xd.CreateElement("TranCode")).InnerText = _PayloadType;
                    xnHeader.AppendChild(xd.CreateElement("MessageFormat")).InnerText = "X12";
                    xnHeader.AppendChild(xd.CreateElement("Sender")).InnerText = _UserName;
                    xnHeader.AppendChild(xd.CreateElement("Receiver")).InnerText = _VendorName;
                    xnHeader.AppendChild(xd.CreateElement("Session"));
                    xnHeader.AppendChild(xd.CreateElement("ProviderOfficeNbr")).InnerText = "NONE";
                    xnHeader.AppendChild(xd.CreateElement("ProviderTransID")).InnerText = _PayloadID;
                    xd.DocumentElement.AppendChild(xd.CreateElement("BODY")).InnerText = _REQ;
                    var xmlRequest = xd.OuterXml.Replace("<Session />", "<Session></Session>");
                    SendRequest(httpRequest, xmlRequest);
                }
                else
                {
                    //do nothing
                    //we are expecting the request is in xml format.
                    SendRequest(httpRequest, _REQ);

                }


                //Random rnd = new Random();
                int m = 0; //rnd.Next(0, 1000);
                //  m = m * 5;
                Thread.Sleep(_WaitTime);
                //var xmlRequest = xd.OuterXml;


                sTimeStamp = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:fffZ");

                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("AVAILITY Sent at " + Convert.ToString(sTimeStamp) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _REQ);
                }

                stime = sTimeStamp;

                var xmlResponse = GetResponseEX(httpRequest);

                _OrigRes = xmlResponse;
                rtime = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:fffZ");

                PayloadIDTX = _PayloadID;



                if (!string.IsNullOrEmpty(xmlResponse))
                {
                    xd.LoadXml(xmlResponse);
                    var xnTemp = xd.DocumentElement.SelectSingleNode("BODY");
                    {
                        _RES = xnTemp.InnerText;
                        sPayload = _RES;
                        r = 0;
                    }

                    var xnPayLoadId = xd.DocumentElement.SelectSingleNode("HEADER/ProviderTransID");  
                    if (xnPayLoadId != null)
                    {
                        PayloadIDRX = xnPayLoadId.InnerText;
                    }
                    var xnError = xd.DocumentElement.SelectSingleNode("Interchange/Error");  
                    if (xnError != null)
                    {
                        r = -2;
                        //_RES = xnError.InnerText;
                        ErrorMsg = xnError.InnerText;
                        MLOG_ID = log.MLOG(_VendorName, ErrorMsg, _EBRID, stime, rtime, B_RES, B_REQ, PayloadIDTX, PayloadIDRX, _RES, _REQ);
                    }                       
                }

                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("AVAILITY REcived  at " + Convert.ToString(rtime) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _RES);
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
            catch (System.Exception ex)
            {
                r = -1;
                Endtime = DateTime.Now;
                _elapsedTicks = Endtime.Ticks - StartTime.Ticks;
                log.ExceptionDetails("GetResponse Failed Total Elasped ticks" + Convert.ToString(_elapsedTicks) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl + "|" + _REQ, ex);
            }



            return r;
        }





        protected virtual void SendRequest(HttpWebRequest request, string payload)
        {
            using (var writer = new StreamWriter(request.GetRequestStream(), Encoding.Default))
            {
                writer.Write(payload);
            }
        }





        protected virtual string GetResponseEX(HttpWebRequest request)
        {
            var response = (HttpWebResponse)request.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                return reader.ReadToEnd();
            }
        }






        public int GetResponse_OLD()
        {
            int r = -1;
            _RES = string.Empty;
            DateTime StartTime = DateTime.Now;
            DateTime Endtime;
            XmlDocument XMLEDI271 = new XmlDocument();
            string httpData = null;

            try
            {

                HttpWebRequest req = null;
                HttpWebResponse resp = null;

                //_wsUrl = "https://apps.availity.com/availity/B2BHCTransactionServlet";
                string strRequest = _REQ;
                req = (HttpWebRequest)WebRequest.Create(_wsUrl);
                req.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;

                var _with1 = req;
                _with1.Timeout = _ServiceTimeOut;
                _with1.Method = _Method;
                _with1.ContentType = _ContentType;




                dynamic up = req.GetRequestStream();



                StreamWriter sw = new StreamWriter(up);
                var _with2 = sw;
                _with2.Write(strRequest);
                _with2.Flush();
                _with2.Close();

                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("AVAILITY sent at " + Convert.ToString(StartTime) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _REQ);
                }

                resp = (HttpWebResponse)req.GetResponse();
                dynamic s = resp.GetResponseStream();
                StreamReader sr = new StreamReader(s);
                var _with3 = sr;
                httpData = _with3.ReadToEnd();
                _with3.Close();
                s.Close();
                resp.Close();
                string Err = string.Empty;
                XMLEDI271.LoadXml(httpData);
                XmlNodeList ErrorCode = XMLEDI271.GetElementsByTagName("ErrorCode");
                if (ErrorCode.Count > 0)
                {
                    log.ExceptionDetails("AVAILITY FAILED RESPONSE  REcived  at " + Convert.ToString(DateTime.Now) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, httpData);
                }
                else
                {
                    _RES = XMLEDI271.GetElementsByTagName("BODY")[0].InnerText;
                }


                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("AVAILITY  REcived  at " + Convert.ToString(DateTime.Now) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _RES);
                }

                //httpData = CleanResponse(httpData, "<BODY>", "</BODY>");
                r = 0;
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

 

    }
}
