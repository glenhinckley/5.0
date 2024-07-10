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
     class TexasMedicaidHealthcarePartnership : IDisposable 
    {

        private StringStuff ss = new StringStuff();
        private logExecption log = new logExecption();



        private string _ConnectionString = string.Empty;


        long _elapsedTicks = 0;


        private string _VendorInputType = string.Empty;
        private string _VendorName = string.Empty;
        private string _PayorCode = string.Empty;


        private  string _REQ = string.Empty;
        private  string _RES = string.Empty;


        private string _wsUrl = string.Empty;
        private string _ContentType = string.Empty;
        private string _Method = string.Empty;
        private int _ServiceTimeOut = 0;
        private string _Accept = string.Empty;
        private string _LogToREQRES = string.Empty;
        private string _Token = string.Empty;
        bool _disposed;
        private int _VMetricsLogging = 0;

        private string _ProtocolType = string.Empty;
        ~TexasMedicaidHealthcarePartnership()
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

        
         

    public int GetResponse()
    {
	        int r = -1;
            _RES = string.Empty;
	        DateTime StartTime = DateTime.Now;
	        DateTime Endtime = default(DateTime);

	        string httpData = null;
	        string s1 = null;

	        string err = null;
	        string errMsg = null;
	        //string wsUrl = string.Empty;
	        try 
            {
                //log.ConnectionString = _ConnectionString;
                //log.ExceptionDetails("5.Started-GetResponse", Convert.ToString(wsUrl));
		        XmlDocument soapEnvelopeXml = CreateSoapEnvelope(_REQ);
                //log.ExceptionDetails("6.CreateSoapEnvelope", Convert.ToString(wsUrl));
		        HttpWebRequest webRequest = CreateWebRequest(_wsUrl);
                //log.ExceptionDetails("7.CreateWebRequest", Convert.ToString(wsUrl));
		        InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);
                //log.ExceptionDetails("8.Build Soap Envelop", Convert.ToString(wsUrl));
                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("TXMCD_DIRECT Sent at " + Convert.ToString(StartTime) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _REQ);
                }

		        HttpWebResponse resp = (HttpWebResponse)webRequest.GetResponse();
                //log.ExceptionDetails("10.HttpWebResponse-", Convert.ToString(wsUrl));
		        Stream S = resp.GetResponseStream();
                //log.ExceptionDetails("11.GetResponseStream", Convert.ToString(wsUrl));
		        StreamReader sr = new StreamReader(S);
                //log.ExceptionDetails("12.Stream reader", Convert.ToString(wsUrl));
		        dynamic _with3 = sr;
		        httpData = _with3.ReadToEnd();
                //log.ExceptionDetails("13.httpData", Convert.ToString(httpData));
		        _with3.Close();
		        S.Close();
		        resp.Close();
		        _RES = httpData;
                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("TXMCD_DIRECT REcived  at " + Convert.ToString(DateTime.Now) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _RES);
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



  

        private  HttpWebRequest CreateWebRequest(string url)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            //webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = _ContentType; // "text/xml;charset='utf-8'";
            webRequest.Accept = _Accept;  // "application/soap+xml";
            webRequest.Method = _Method;   // "POST";
            webRequest.Timeout = _ServiceTimeOut; //Convert.ToInt32(600000);
            return webRequest;
        }

        private static XmlDocument CreateSoapEnvelope(string edi)
        {

            string soap = "";
            //string passwd = "";

            //passwd = "p96gumCR";
            // test password
            //Tf7gJz54
            //passwd = "Tf7gJz54";
            Guid g;
            g = Guid.NewGuid();
            string sPwd = string.Empty;
            sPwd =  @"PS\wPZ7/DqBy";

            XmlDocument soapEnvelop = new XmlDocument();

            soap = "<soapenv:Envelope xmlns:soapenv='http://www.w3.org/2003/05/soap-envelope'>";
            soap = soap + "<soapenv:Header>";
            soap = soap + "<wsse:Security xmlns:wsse='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd' soapenv:mustUnderstand='true'>";
            soap = soap + "<wsse:UsernameToken xmlns:wsu='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd' wsu:Id='UsernameToken-21621663'>";
            soap = soap + "<wsse:Username>XEC0532PRT</wsse:Username>";
            soap = soap + "<wsse:Password Type='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText'>" + sPwd + "</wsse:Password>";
            soap = soap + "</wsse:UsernameToken>";
            soap = soap + "</wsse:Security>";
            soap = soap + "</soapenv:Header>";
            soap = soap + "<soapenv:Body>";
            soap = soap + "<ns1:COREEnvelopeRealTimeRequest xmlns:ns1='http://www.caqh.org/SOAP/WSDL/CORERule2.2.0.xsd'>";
            soap = soap + "<PayloadType>X12_270_Request_005010X279A1</PayloadType>";
            soap = soap + "<ProcessingMode>RealTime</ProcessingMode>";
            soap = soap + "<PayloadID>" + Convert.ToString(g)  +"</PayloadID>";
            soap = soap + "<TimeStamp>2016-07-28T15:58:13</TimeStamp>";
            //soap = soap + "<SenderID>METPID005914</SenderID>";
            soap = soap + "<SenderID>DCSGLOBALEDIEXG</SenderID>";
            soap = soap + "<ReceiverID>MMISNEBR</ReceiverID>";
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
