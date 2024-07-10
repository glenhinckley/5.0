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
     class AVAILITY : IDisposable
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

        ~AVAILITY()
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
        public int GetResponse()
        {


            int r = -1;
            DateTime StartTime = DateTime.Now;
            DateTime Endtime;


            string url = string.Empty;
            _RES = string.Empty;
       
        
            
            try
            {

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
               
               
                //var xmlRequest = xd.OuterXml;
                 
                

                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("AVAILITY Sent at " + Convert.ToString(StartTime) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _REQ);
                }
                var xmlResponse = GetResponseEX(httpRequest);







                if (!string.IsNullOrEmpty(xmlResponse))
                {
                    xd.LoadXml(xmlResponse);
                    var xnTemp = xd.DocumentElement.SelectSingleNode("BODY");
                    if (xnTemp != null)
                    {
                        var xnError = xnTemp.SelectSingleNode("Interchange/Error");
                        if (xnError != null)
                        {
                            _RES = xnError.InnerText;
                            r = -1;
                        }
                        else
                        {   
                            
                            r = 0;
                            _RES = xnTemp.InnerText;
                            if (_VMetricsLogging == 1)
                            {
                                log.ExceptionDetails("AVAILITY REcived  at " + Convert.ToString(DateTime.Now) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _RES);
                            }
                        }
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



        //private static string CleanResponse(string haystack, string needle1, string needle2)
        //{
        //    //int istart = String.InStr(haystack, needle1);
        //    int istart = haystack.IndexOf(needle1, 0);
        //    if (istart > 0)
        //    {
        //        //int istop = String.InStr(istart, haystack, needle2);
        //        int istop = haystack.IndexOf(needle2, istart);
        //        if (istop > 0)
        //        {
        //            //string value = haystack.Substring(istart + Strings.Len(needle1) - 1, istop - istart - Strings.Len(needle1));
        //            string value = haystack.Substring(istart + needle1.Length, istop - istart - needle1.Length);
        //            return value;
        //        }
        //    }
        //    return null;
        //}


    }
}
