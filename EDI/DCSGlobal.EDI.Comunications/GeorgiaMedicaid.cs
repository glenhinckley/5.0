using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using DCSGlobal.BusinessRules.Logging;

 

namespace DCSGlobal.EDI.Comunications
{
    class GeorgiaMedicaid : IDisposable 
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

         ~GeorgiaMedicaid()
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
            string httpData = null;
            string s1 = null;
             
             _RES = string.Empty;

            XmlDocument XMLEDI271 = new XmlDocument();
            string Err = string.Empty;
            string errMsg = string.Empty;
            DateTime StartTime = DateTime.Now;
            DateTime Endtime = default(DateTime);

            try
            {
                //wsUrl = "https://hts.betammis.georgia.gov/GA_ACA1104WCFServices/GA_ACA1104WCFServices.svc";
                //wsUrl = "https://hts.mmis.georgia.gov/GA_ACA1104WCFServices/GA_ACA1104WCFServices.svc"

                XmlDocument soapEnvelopeXml = CreateGASoapEnvelope(_REQ);


                HttpWebRequest webRequest = CreateGAWebRequest(_wsUrl);
                InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("GA MEDICAID sent at " + Convert.ToString(StartTime) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _REQ);
                }

                HttpWebResponse resp = (HttpWebResponse)webRequest.GetResponse();
 

                Stream S = resp.GetResponseStream();
                StreamReader sr = new StreamReader(S);
                dynamic _with3 = sr;
                httpData = _with3.ReadToEnd();

                _with3.Close();
                S.Close();
                resp.Close();

                XMLEDI271.LoadXml(httpData);

                Err = XMLEDI271.GetElementsByTagName("ErrorCode")[0].InnerText;
                errMsg = XMLEDI271.GetElementsByTagName("ErrorMessage")[0].InnerText;
                if (Err == "Success")
                {
                    s1 = XMLEDI271.GetElementsByTagName("Payload")[0].InnerText;
                }
                else
                {
                    s1 = "boken" + " err: " + Err + " " + errMsg;
                }
                 
                _RES = s1;
                if (_VMetricsLogging == 1)
                {

                    log.ExceptionDetails("GA MEDICAID REcived  at " + Convert.ToString(DateTime.Now) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _RES);
                }
                r = 0;
            }
            catch (Exception ex)
            {
                //do shawns 271 what ever
                
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
            Guid g;
            g = Guid.NewGuid();

            string timestamp = string.Empty;
             
            string soap = "";

            timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:22Z");
             
            XmlDocument soapEnvelop = new XmlDocument();


            soap = "";

            soap = soap + Convert.ToString("<soapenv:Envelope xmlns:soapenv=\"http://www.w3.org/2003/05/soap-envelope\">");

            soap = soap + Convert.ToString("<soapenv:Header>");

            soap = soap + Convert.ToString("<wsse:Security xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" soapenv:mustUnderstand=\"true\">");

            soap = soap + Convert.ToString("<wsse:UsernameToken xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wsswssecurity-utility-1.0.xsd\"  wsu:Id=\"UsernameToken-21621663\">");

            soap = soap + Convert.ToString("<wsse:Username>" + _UserName  + "</wsse:Username>");

            soap = soap + Convert.ToString("<wsse:Password Type=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-usernametoken-profile-1.0#PasswordText\">" +  _Passwd  + "</wsse:Password>");

            soap = soap + Convert.ToString("</wsse:UsernameToken>");

            soap = soap + Convert.ToString("</wsse:Security>");

            soap = soap + Convert.ToString("</soapenv:Header>");

            soap = soap + Convert.ToString("<soapenv:Body>");

            soap = soap + Convert.ToString("<ns1:COREEnvelopeRealTimeRequest xmlns:ns1=\"http://www.caqh.org/SOAP/WSDL/CORERule2.2.0.xsd\">");

            soap = soap + Convert.ToString("<PayloadType>" + _PayloadType  + "</PayloadType>");

            soap = soap + Convert.ToString("<ProcessingMode>" +  _ProcessingMode   + "</ProcessingMode>");

            soap = soap + Convert.ToString("<PayloadID>" + Convert.ToString(g) + "</PayloadID>");

            soap = soap + Convert.ToString("<TimeStamp>" + timestamp + "</TimeStamp>");

            soap = soap + Convert.ToString("<SenderID>" + _SenderID   + "</SenderID>");

            soap = soap + Convert.ToString("<ReceiverID>" +  _ReceiverID   + "</ReceiverID>");

            soap = soap + Convert.ToString("<CORERuleVersion>" +  _CORERuleVersion  + "</CORERuleVersion>");

            soap = soap + Convert.ToString("<Payload><![CDATA[")  + edi + "]]></Payload>";

            soap = soap + Convert.ToString("</ns1:COREEnvelopeRealTimeRequest>");

            soap = soap + Convert.ToString("</soapenv:Body>");

            soap = soap + Convert.ToString("</soapenv:Envelope>");



            soapEnvelop.LoadXml(soap);

            return soapEnvelop;

        }

        private  HttpWebRequest CreateGAWebRequest(string url)
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
