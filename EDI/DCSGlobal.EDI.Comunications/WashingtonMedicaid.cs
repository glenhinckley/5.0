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
    class WashingtonMedicaid : IDisposable
    {
        private logExecption log = new logExecption();
        private StringStuff ss = new StringStuff();

        private int _TaskID = 0;
        private string _EBRID = string.Empty;
        private int _VendorRetrys = 1;
        private int _WaitTime = 0;
        private string B_REQ = string.Empty;

        private string _ConnectionString = string.Empty;
        private string _VendorName = "WAMCD";
        private string _REQ = string.Empty;
        private string _RES = string.Empty;
        private string _PayorCode = string.Empty;
        private string _wsUrl = string.Empty;
        private string _UserName = string.Empty;
        private string _Passwd = string.Empty;
        private long _ServiceTimeOut = 180000;
        private long _ReceiveTimeout = 180000;

        private string sPayloadID = string.Empty;
        private string sTimeStamp = string.Empty;
        private string sProcessingMode = string.Empty;
        private string sPayloadType = string.Empty;
        private string sSenderID = string.Empty;
        private string sReceiverID = string.Empty;
        private string sCORERuleVersion = string.Empty;
        private string sPayload = string.Empty;

        private string _ProtocolType = string.Empty;
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


        long _elapsedTicks = 0;




        bool _disposed;

        ~WashingtonMedicaid()
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
            _RES = string.Empty;
            DateTime StartTime = DateTime.Now;
            DateTime Endtime = default(DateTime);
            string sTimeStamp = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:fffZ");  
            string httpData = null;
            
            string errMsg = null;
           
          
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

                XmlDocument soapEnvelopeXml = CreateSoapEnvelope(_REQ);
                HttpWebRequest webRequest = CreateWebRequest(_wsUrl);
                InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);
                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("WAMCD Request Sent at " +  sTimeStamp , soapEnvelopeXml.InnerXml.ToString());
                }
                 
                HttpWebResponse resp = (HttpWebResponse)webRequest.GetResponse();
                Stream S = resp.GetResponseStream();
                //StreamReader sr = new StreamReader(S);
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader sr = new StreamReader(S,encode);
                  
                dynamic _with3 = sr;
                httpData = _with3.ReadToEnd();
                _OrigRes = httpData;
                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("WAMCD Response Recived  at " + Convert.ToString(DateTime.Now), httpData);
                }

                // sample response --   <s:Envelope xmlns:s="http://www.w3.org/2003/05/soap-envelope"><s:Body><ns0:COREEnvelopeRealTimeResponse xmlns:ns0="http://www.caqh.org/SOAP/WSDL/CORERule2.2.0.xsd"><PayloadType>X12_271_Response_005010X279A1</PayloadType><ProcessingMode>RealTime</ProcessingMode><PayloadID>54941536-c061-445d-bb67-5db680629ff6</PayloadID><TimeStamp>2016-10-28T17:36:40Z</TimeStamp><SenderID>445498161</SenderID><ReceiverID>209001838</ReceiverID><CORERuleVersion>2.2.0</CORERuleVersion><Payload><![CDATA[ISA*00*          *00*          *ZZ*445498161      *ZZ*209001838      *161028*1336*^*00501*302190159*0*P*:~GS*HB*445498161*209001838*20161028*133639*1*X*005010X279A1~ST*271*589486*005010X279A1~BHT*0022*11*12X34*20161028*1336~HL*1**20*1~NM1*PR*2*CT MCD*****PI*445498161~HL*2*1*21*1~NM1*1P*2*WATERBURY*****XX*1477902641~HL*3*2*22*0~TRN*1*1630202QRR*9445498161~NM1*IL*1*CORAL*BILLY****MI*7787262221~AAA*N**72*C~DMG*D8*19790807*M~DTP*291*RD8*20161015-20161015~SE*13*589486~GE*1*1~IEA*1*302190159~]]></Payload><ErrorCode>               
                _with3.Close();
                S.Close();
                resp.Close();

                var xd = new XmlDocument();

                if (!string.IsNullOrEmpty(httpData))
                {
                    try
                    {
                        xd.LoadXml(httpData);
                        var xnError = xd.DocumentElement.GetElementsByTagName("ErrorCode");
                        if (xnError != null)
                        {
                            errMsg = xnError[0].InnerText;
                            if (_VMetricsLogging == 1)
                            {
                                log.ExceptionDetails("WAMCD Response ErrorCode ", errMsg);
                            }
                        }
                        var payLoad = xd.DocumentElement.GetElementsByTagName("Payload");
                        if (payLoad != null)
                        {
                            _RES = payLoad[0].InnerText;
                            r = 0;
                            if (_VMetricsLogging == 1)
                            {
                                log.ExceptionDetails("WAMCD Response Payload  ", _RES);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        r = -2;
                        log.ExceptionDetails("WAMCD Reading Response Failed ", ex);             
                    }
                }
               
            }
            catch (Exception ex)
            {
                _RES = "";
                r = -1;
                Endtime = DateTime.Now;
                _elapsedTicks = Endtime.Ticks - StartTime.Ticks;
                log.ExceptionDetails("WAMCD GetResponse Failed Total Elasped ticks" + Convert.ToString(_elapsedTicks), ex);
            }
            return r;
        }

         
        private HttpWebRequest CreateWebRequest(string url)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            //webRequest.Headers.Add("SOAPAction", action);

            webRequest.ContentType = "application/soap+xml";    // "multipart/related";  //"text/xml;charset='utf-8'";
            webRequest.Accept = "text/xml" ;  // "application/xop+xm"; // "application/soap+xml"; // "text/xml"; //"application/soap+xml";
            webRequest.Method = "POST";
            webRequest.Timeout = Convert.ToInt32(_ServiceTimeOut);
            //webRequest.Timeout = Convert.ToInt32(600000);
            return webRequest;
        }

        private  XmlDocument CreateSoapEnvelope(string edi)
        {

            string soap = "";
         
            Guid g;
            g = Guid.NewGuid();
        
            XmlDocument soapEnvelop = new XmlDocument();

            soap = "";
            soap = soap  +  Convert.ToString("<soap:Envelope xmlns:soap='http://www.w3.org/2003/05/soap-envelope' xmlns:cor='http://www.caqh.org/SOAP/WSDL/CORERule2.2.0.xsd'>");
            soap = soap  +  Convert.ToString("<soap:Header>");
            soap = soap  +  Convert.ToString("<wsse:Security soap:mustUnderstand='1' xmlns:wsse='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd' xmlns:wsu='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd' >");
            soap = soap  +  Convert.ToString("<wsse:UsernameToken wsu:Id='UsernameToken-600'>");
            soap = soap  +  Convert.ToString("<wsse:Username>SnideS</wsse:Username>");
            soap = soap  +  Convert.ToString("<wsse:Password Type='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText'>Rangers2017!</wsse:Password>");
            soap = soap  +  Convert.ToString("</wsse:UsernameToken>");
            soap = soap  +  Convert.ToString("</wsse:Security>");
            soap = soap  +  Convert.ToString("</soap:Header>");
            soap = soap  +  Convert.ToString("<soap:Body>");
            soap = soap  +  Convert.ToString("<cor:COREEnvelopeRealTimeRequest>");
            soap = soap  +  Convert.ToString("<PayloadType>X12_270_Request_005010X279A1</PayloadType>");
            soap = soap  +  Convert.ToString("<ProcessingMode>RealTime</ProcessingMode>");
            soap = soap  +  Convert.ToString("<PayloadID>" + Guid.NewGuid().ToString() + "</PayloadID>");
            soap = soap  +  Convert.ToString("<TimeStamp>2014-07-09T11:35:45+03:00</TimeStamp>");
            soap = soap  +  Convert.ToString("<SenderID>207008700</SenderID>");
            soap = soap  +  Convert.ToString("<ReceiverID>77045</ReceiverID>");
            soap = soap  +  Convert.ToString("<CORERuleVersion>2.2.0</CORERuleVersion>");
            //soap = soap + "<Payload><![CDATA[ISA*00*          *00*          *ZZ*207008700      *ZZ*77045          *170209*1058*^*00501*673896488*0*P*:~GS*HS*207008700*77045*20170209*1058*589486*X*005010X279A1~ST*270*589486*005010X279A1~BHT*0022*13*943BAC5B72A34B2*20170209*1058~HL*1**20*1~NM1*PR*2*WA State DSHS*****PI*77045~HL*2*1*21*1~NM1*1P*2*SNOQUALMIE*****XX*1902846546~HL*3*2*22*0~NM1*IL*1*SMITH*BILL****MI*1123156321~DMG*D8*19260429~DTP*291*D8*20170209~EQ*30~SE*12*589486~GE*1*589486~IEA*1*673896488~]]></Payload>";
            soap = (soap  +  Convert.ToString("<Payload><![CDATA[")) + edi + "]]></Payload>";
            soap = soap  +  Convert.ToString("</cor:COREEnvelopeRealTimeRequest>");
            soap = soap  +  Convert.ToString("</soap:Body>");
            soap = soap  +  Convert.ToString("</soap:Envelope>");
             

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
