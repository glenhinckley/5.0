using System;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;
using DCSGlobal.EDI.Comunications;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using DCSGlobal.BusinessRules.Logging;
using System.Xml;

namespace DCSGlobal.EDI.Comunications
{
     class PARAMOUNTHEALTH : IDisposable 
    {

       
                 private StringStuff ss = new StringStuff();
        private logExecption log = new logExecption();



        private string _ConnectionString = string.Empty;


        long _elapsedTicks = 0;


        private string _VendorInputType = string.Empty;
        private string _VendorName = string.Empty;
        private string _PayorCode = string.Empty;

        private string _AppName = string.Empty;


        private string _SUBMISSION_TIMEOUT = "0.00:00:30";
        private string _SYNC_TIMEOUT = "0.00:00:30";
        private string SYNC_TIMEOUT = string.Empty;
        private string SUBMISSION_TIMEOUT = string.Empty;
        private string _myResponses = string.Empty;
        private string _myResponses1 = string.Empty;

        private  string _REQ = string.Empty;
        private  string _RES = string.Empty;
        private string _ProtocolType = string.Empty;

        private string _wsUrl = string.Empty;
        private string _ContentType = string.Empty;
        private string _Method = string.Empty;
        private int _ServiceTimeOut = 0;
        private int _TimeToWait = 0;
        private string _Accept = string.Empty;
        private string _LogToREQRES = string.Empty;
        private string _Token = string.Empty;
        private int _VMetricsLogging = 0;

        ~PARAMOUNTHEALTH()
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


        public string VendorInputType
        {

            set
            {

                _VendorInputType = value;
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



        public string Accept
        {
            set
            {
                _Accept = value;
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
         

 

    

 
        public int GetResponse()
        {
            int r = -1;
            _RES = string.Empty;
            DateTime StartTime = DateTime.Now;
            DateTime Endtime;

            string httpData = null;


            long _elapsedTicks = 0;

            //var WS_Action = "";

            //wsUrl = "https://phcedi.promedica.org:50043/x12/realtime/soap?request_type=X12_270_Request_005010X279A1";

            //wsUrl = "https://phceditest.promedica.org:50043/x12/realtime/soap?request_type=X12_270_Request_005010X279A1";

            // WS_Action = "https://phcedi.promedica.org:50043/x12/realtime/soap?request_type=X12_270_Request_005010X279A1";



            try
            {
                XmlDocument soapEnvelopeXml = CreateSoapEnvelope(_REQ);
                HttpWebRequest req = null;
                HttpWebResponse resp = null;
                //
                // Username and Password must be in ISA02 and ISA04 of the request
                //
                //string strRequest = _REQ;
                req = (HttpWebRequest)WebRequest.Create(_wsUrl);
                var _with1 = req;
                _with1.Timeout = _ServiceTimeOut;

                //_with1.ContentType = "text/xml;charset=\"utf-8\"";
                //_with1.Accept = "text/xml";
                //_with1.Method = "POST";

                _with1.ContentType = _ContentType;
                _with1.Accept = _Accept;
                _with1.Method = _Method;

                dynamic up = req.GetRequestStream();
                StreamWriter sw = new StreamWriter(up);
                var _with2 = sw;
                //_with2.Write(strRequest);
                _with2.Write(soapEnvelopeXml);
                _with2.Flush();
                _with2.Close();
                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("PARAMOUNT Sent at " + Convert.ToString(StartTime) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _REQ);
                }

                resp = (HttpWebResponse)req.GetResponse();
                dynamic s = resp.GetResponseStream();
                StreamReader sr = new StreamReader(s);
                var _with3 = sr;
                httpData = _with3.ReadToEnd();
                _with3.Close();
                s.Close();
                resp.Close();
                //httpData = CleanResponse(httpData, "<BODY>", "</BODY>");
                _RES = httpData;
                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("PARAMOUNT REcived  at " + Convert.ToString(DateTime.Now) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _RES);
                }
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
    
    public int GetData(int Test)
    {
       int r =-1;
        string S;
        string err;
        string errMsg;
        try
        {

            // https://phcedi.promedica.org:50043/x12/realtime/soap?request_type=X12_270_Request_005010X279A1


            var WS_URL = "";
            var WS_Action = "";
            
            switch( Test)
           
            {
                case 0:

                    WS_URL = "https://phcedi.promedica.org:50043/x12/realtime/soap?request_type=X12_270_Request_005010X279A1";
                    WS_Action = "https://phcedi.promedica.org:50043/x12/realtime/soap?request_type=X12_270_Request_005010X279A1";


                    break;

                case 1:

                    WS_URL = "https://phceditest.promedica.org:50043/x12/realtime/soap?request_type=X12_270_Request_005010X279A1";
                    WS_Action = "https://phceditest.promedica.org:50043/x12/realtime/soap?request_type=X12_270_Request_005010X279A1";

                    break;
               
                
                default:
                   

                    break;

            }


            XmlDocument soapEnvelopeXml = CreateSoapEnvelope(_REQ);
            HttpWebRequest webRequest = CreateWebRequest(WS_URL, WS_Action);
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);
            XmlDocument XMLEDI271 = new XmlDocument();


            // begin async call to web request.
            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

            // suspend this thread until call is complete. You might want to
            // do something usefull here like update your UI.
            asyncResult.AsyncWaitHandle.WaitOne();

            // get the response from the completed web request.
            string soapResult;
            using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
            {
                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                {
                    soapResult = rd.ReadToEnd();
                }

                XMLEDI271.LoadXml(soapResult);


                err = XMLEDI271.GetElementsByTagName("ErrorCode")[0].InnerText;
                errMsg = XMLEDI271.GetElementsByTagName("ErrorMessage")[0].InnerText;
                if (err == "Success")
                {
                    S = XMLEDI271.GetElementsByTagName("Payload")[0].InnerText;
                }
                else
                {
                    S = "boken" + " err: " + err + " " + errMsg;
                }

                  _RES = S;

                r = 0;

            }
        }
        catch(Exception ex)
        {
            r =-1;
        }
        return r;
      
    
    }


    private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            //webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

    private static XmlDocument CreateSoapEnvelope(string edi)
        {
           
            string soap = "";
            string passwd = "";

            passwd = "p96gumCR";
            // test password
            //Tf7gJz54
            //passwd = "Tf7gJz54";
        
        
            XmlDocument soapEnvelop = new XmlDocument();
            
            soap = "<soapenv:Envelope xmlns:soapenv='http://www.w3.org/2003/05/soap-envelope'>";
            soap = soap + "<soapenv:Header>";
            soap = soap + "<wsse:Security xmlns:wsse='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd' soapenv:mustUnderstand='true'>";
            soap = soap + "<wsse:UsernameToken xmlns:wsu='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd' wsu:Id='UsernameToken-21621663'>";
            soap = soap + "<wsse:Username>E4049DCS</wsse:Username>";
            soap = soap + "<wsse:Password Type='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText'>" + passwd +"</wsse:Password>";
            soap = soap + "</wsse:UsernameToken>";
            soap = soap + "</wsse:Security>";
            soap = soap + "</soapenv:Header>";
            soap = soap + "<soapenv:Body>";
            soap = soap + "<ns1:COREEnvelopeRealTimeRequest xmlns:ns1='http://www.caqh.org/SOAP/WSDL/CORERule2.2.0.xsd'>";
            soap = soap + "<PayloadType>X12_270_Request_005010X279A1</PayloadType>";
            soap = soap + "<ProcessingMode>RealTime</ProcessingMode>";
            soap = soap + "<PayloadID>33061F6D-4AC2-0080-8000-005056A2002B</PayloadID>";
            soap = soap + "<TimeStamp>2012-11-19T15:58:13</TimeStamp>";
            soap = soap + "<SenderID>4049</SenderID>";
            soap = soap + "<ReceiverID>PARAMOUNTHEALTH</ReceiverID>";
            soap = soap + "<CORERuleVersion>2.2.0</CORERuleVersion>";
            soap = soap + "<Payload><![CDATA[" + edi + "]]></Payload>";
            soap = soap + "</ns1:COREEnvelopeRealTimeRequest>";
            soap = soap + "</soapenv:Body>";
            soap = soap + "</soapenv:Envelope>";

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
    
    }
}
