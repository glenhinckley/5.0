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
    class TexasMedicaidHealthcarePartnershipDCSWebService : IDisposable 
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
 
        private  string _REQ = string.Empty;
        private  string _RES = string.Empty;
        private int _VMetricsLogging = 0;
        bool _disposed;

        ~TexasMedicaidHealthcarePartnershipDCSWebService()
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

    private  HttpWebRequest CreateWebRequest(string url)
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
        int i=0;
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
            log.ConnectionString = _ConnectionString;
            
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
                log.ExceptionDetails("TXMCD_DCSWS Sent at " + Convert.ToString(StartTime) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _REQ);
            }


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

            XmlDocument XMLEDI271 = new XmlDocument();
            XMLEDI271.LoadXml(httpData);
            XmlNodeList elemList = XMLEDI271.GetElementsByTagName("getEligVendorEdiResponseResult");
            if (elemList.Count > 0)
            {
                _RES = elemList[0].InnerText;
                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("TXMCD_DCSWS REcived  at " + Convert.ToString(DateTime.Now) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _RES);
                }
                r = 0;
            }
            else
            {
                _RES = "";
                r = -1;
                log.ExceptionDetails("13.r=-1", Convert.ToString(r));
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
        soap = soap + Convert.ToString("<login>"  + _login  + "</login>");
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
