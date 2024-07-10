using System;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using DCSGlobal.BusinessRules.Logging;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;
using DCSGlobal.BusinessRules.Logging;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using System.ServiceModel;
using System.ServiceModel.Channels;

using System.Net;







namespace DCSGlobal.EDI.Comunications
{

    public class MaineMedicaidSoap : IDisposable
    {

        private logExecption log = new logExecption();
        private StringStuff ss = new StringStuff();
        private string _ConnectionString = string.Empty;
        private string _VendorName = "MEMCD";
        private string _REQ = string.Empty;
        private string _RES = string.Empty;
        private string _PayorCode = string.Empty;
        private string _wsUrl = string.Empty;
        private string _UserName = string.Empty;
        private string _Passwd = string.Empty;
        private long _ServiceTimeOut = 30;
        private long _ReceiveTimeout = 30;
        long _elapsedTicks = 0;
        private string ErrorMsg = "";
        private string sPayloadID = string.Empty;
        private string sTimeStamp = string.Empty;
        private string sProcessingMode = string.Empty;
        private string sPayloadType = string.Empty;
        private string sSenderID = string.Empty;
        private string sReceiverID = string.Empty;
        private string sCORERuleVersion = string.Empty;

        private string sPayload = string.Empty;
        private int _VMetricsLogging = 0;
        private string _PayloadID = string.Empty;
        private string _TimeStamp = string.Empty;
        private string _ProcessingMode = string.Empty;
        private string _PayloadType = string.Empty;
        private string _SenderID = string.Empty;
        private string _ReceiverID = string.Empty;
        private string _CORERuleVersion = string.Empty;
        private string _Payload = string.Empty;
        private int _TaskID = 0;
        private string _ProtocolType = string.Empty;






        bool _disposed;

        ~MaineMedicaidSoap()
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

        public int TaskID
        {



            set
            {

                _TaskID = value;

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
                    // ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;
                }




                XmlDocument soapEnvelopeXml = CreateSoapEnvelope(_REQ);
                HttpWebRequest webRequest = CreateWebRequest(_wsUrl);

                InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);
                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("WVMCD Request Sent at " + sTimeStamp, soapEnvelopeXml.InnerXml.ToString());
                }



                HttpWebResponse resp = (HttpWebResponse)webRequest.GetResponse();

                Stream S = resp.GetResponseStream();

                //StreamReader sr = new StreamReader(S);
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader sr = new StreamReader(S, encode);



                dynamic _with3 = sr;

                httpData = _with3.ReadToEnd();

                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("WVMCD Response Recived  at " + Convert.ToString(DateTime.Now), httpData);
                }

                _with3.Close();
                S.Close();
                resp.Close();



                httpData = CleanResponse(httpData, "<soapenv:Body>", "</soapenv:Body>");



                var xd = new XmlDocument();

                if (!string.IsNullOrEmpty(httpData))
                {

                    try
                    {

                        xd.LoadXml(httpData);

                        //var xnTemp = xd.DocumentElement.SelectSingleNode("BODY");

                        //if (xnTemp != null)

                        //{



                        //}

                        var xnError = xd.DocumentElement.SelectSingleNode("ErrorCode");

                        if (xnError != null)
                        {

                            errMsg = xnError.InnerText;

                            if (_VMetricsLogging == 1)
                            {

                                log.ExceptionDetails("WVMCD ERR  ", errMsg);

                            }

                        }

                        var payLoad = xd.DocumentElement.SelectSingleNode("Payload");

                        if (payLoad != null)
                        {

                            _RES = payLoad.InnerText;

                        }

                    }

                    catch (Exception ex)
                    {



                        log.ExceptionDetails("WVMCD Response Read Exception ", ex);

                    }



                    r = 0;

                }

            }



            catch (Exception ex)
            {

                _RES = "";

                r = -1;

                Endtime = DateTime.Now;

                _elapsedTicks = Endtime.Ticks - StartTime.Ticks;

                log.ExceptionDetails("WVMCD GetResponse Failed Total Elasped ticks" + Convert.ToString(_elapsedTicks), ex);

            }

            return r;

        }











        private HttpWebRequest CreateWebRequest(string url)
        {

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
           //webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "application/soap+xml;charset=UTF-8;action=RealTimeTransaction";    // "multipart/related";  //"text/xml;charset='utf-8'";
            webRequest.Accept = "text/xml";  // "application/xop+xm"; // "application/soap+xml"; // "text/xml"; //"application/soap+xml";
            webRequest.Method = "POST";
            webRequest.Timeout = Convert.ToInt32(600000);
            return webRequest;

        }



        private XmlDocument CreateSoapEnvelope(string edi)
        {



            string soap = "";
          
            Guid g;
            g = Guid.NewGuid();
          
            XmlDocument soapEnvelop = new XmlDocument();
            soap = "<soapenv:Envelope xmlns:soapenv='http://www.w3.org/2003/05/soap-envelope'>";
            soap = soap + "<soapenv:Header>";
            soap = soap + "<wsse:Security xmlns:wsse='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd' soapenv:mustUnderstand='true'>";
            soap = soap + "<wsse:UsernameToken xmlns:wsu='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd' wsu:Id='UsernameToken-21621663'>";
            soap = soap + "<wsse:Username>" + _UserName + "</wsse:Username>";
            soap = soap + "<wsse:Password Type='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText'>" + _Passwd + "</wsse:Password>";
            soap = soap + "</wsse:UsernameToken>";
            soap = soap + "</wsse:Security>";
            soap = soap + "</soapenv:Header>";
            soap = soap + "<soapenv:Body>";
            soap = soap + "<ns1:COREEnvelopeRealTimeRequest xmlns:ns1='http://www.caqh.org/SOAP/WSDL/CORERule2.2.0.xsd'>";
            soap = soap + "<PayloadType>X12_270_Request_005010X279A1</PayloadType>";
            soap = soap + "<ProcessingMode>RealTime</ProcessingMode>";
            soap = soap + "<PayloadID>" + Convert.ToString(g) + "</PayloadID>";
            soap = soap + "<TimeStamp>" + DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:fffZ") + "</TimeStamp>";
            //soap = soap + "<SenderID>METPID005914</SenderID>";
            soap = soap + "<SenderID>" + _SenderID + "</SenderID>";
            soap = soap + "<ReceiverID>" + _ReceiverID + "</ReceiverID>";
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
