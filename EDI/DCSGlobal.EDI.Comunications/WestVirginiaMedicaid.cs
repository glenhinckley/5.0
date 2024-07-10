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
    class WestVirginiaMedicaid : IDisposable
    {
         private logExecption log = new logExecption();
        private StringStuff ss = new StringStuff();

        private   string _ConnectionString = string.Empty;
        private   string _VendorName = "ARMCD";
        private   string _REQ = string.Empty;
        private   string _RES = string.Empty;
        private   string _PayorCode = string.Empty;
        private   string _wsUrl = string.Empty;
        private   string _UserName = string.Empty;
        private   string _Passwd = string.Empty;
        private   long _ServiceTimeOut = 30;
        private   long _ReceiveTimeout = 30;

        private string sPayloadID = string.Empty;
        private string sTimeStamp = string.Empty;
        private string sProcessingMode = string.Empty;
        private string sPayloadType = string.Empty;
        private string sSenderID = string.Empty;
        private string sReceiverID = string.Empty;
        private string sCORERuleVersion = string.Empty;
        private string sPayload = string.Empty;


        private   string _PayloadID = string.Empty;
        private   string _TimeStamp = string.Empty;
        private   string _ProcessingMode = string.Empty;
        private   string _PayloadType = string.Empty;
        private   string _SenderID = string.Empty;
        private   string _ReceiverID = string.Empty;
        private   string _CORERuleVersion = string.Empty;
        private   string _Payload = string.Empty;

        private int _VMetricsLogging = 0;
        private string ErrorMsg = "";
        private string _ProtocolType = string.Empty;

        long _elapsedTicks = 0;




        bool _disposed;

        ~WestVirginiaMedicaid()
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
         




        public Binding CreateCustomBinding()
        {


            CustomBinding binding = new CustomBinding();
            binding.Name = "CustomBinding_ISOAPService";

            TimeSpan _tSendTimeout = TimeSpan.FromSeconds(_ServiceTimeOut);   //new TimeSpan(0, 0, 30);

            TimeSpan _tReceiveTimeout = TimeSpan.FromSeconds(_ReceiveTimeout); //new TimeSpan(0, 0, 30);

            binding.SendTimeout = _tSendTimeout;
            binding.ReceiveTimeout = _tReceiveTimeout;
       
            TextMessageEncodingBindingElement txtMsg = new TextMessageEncodingBindingElement();
            txtMsg.MessageVersion = MessageVersion.Soap12;
            txtMsg.WriteEncoding = System.Text.Encoding.UTF8;

             

            SecurityBindingElement sbe = SecurityBindingElement.CreateUserNameOverTransportBindingElement();
            sbe.MessageSecurityVersion = MessageSecurityVersion.WSSecurity11WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11;

            sbe.SecurityHeaderLayout = SecurityHeaderLayout.Strict;
            sbe.IncludeTimestamp = false;
            sbe.SetKeyDerivation(true);
            sbe.KeyEntropyMode = System.ServiceModel.Security.SecurityKeyEntropyMode.ServerEntropy;
            
            
            binding.Elements.Add(sbe);
            binding.Elements.Add(txtMsg);
            binding.Elements.Add(new HttpsTransportBindingElement());
            return new CustomBinding(binding);
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
                XmlDocument soapEnvelopeXml = CreateSoapEnvelope(_REQ);
                HttpWebRequest webRequest = CreateWebRequest(_wsUrl);
                InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);
                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("WVMCD Request Sent at " +  sTimeStamp , soapEnvelopeXml.InnerXml.ToString());
                }
                 
                HttpWebResponse resp = (HttpWebResponse)webRequest.GetResponse();
                Stream S = resp.GetResponseStream();
                //StreamReader sr = new StreamReader(S);
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader sr = new StreamReader(S,encode);
                  
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

                // sample response --   \r\n--uuid:798c582a-6d98-407f-99b4-ccb0bfdfe643+id=106\r\nContent-ID: <http://tempuri.org/0>\r\nContent-Transfer-Encoding: 8bit\r\nContent-Type: application/xop+xml;charset=utf-8;type=\"application/soap+xml\"\r\n\r\n<soapenv:Envelope xmlns:soapenv=\"http://www.w3.org/2003/05/soap-envelope\"><soapenv:Body><ns1:COREEnvelopeRealTimeResponse xmlns:ns1=\"http://www.caqh.org/SOAP/WSDL/CORERule2.2.0.xsd\"><PayloadType>X12_271_Response_005010X279A1</PayloadType><ProcessingMode>RealTime</ProcessingMode><PayloadID>6dd863c8-f314-4033-b4db-9ed2bfd5add9</PayloadID><TimeStamp>2016-10-28T15:08:42Z</TimeStamp><SenderID>WV_MMIS_4MOLINA</SenderID><ReceiverID>WVTPID006574</ReceiverID><CORERuleVersion>2.2.0</CORERuleVersion><Payload>ISA*00*          *00*          *ZZ*WV_MMIS_4MOLINA*ZZ*WVTPID006574   *161009*0930*^*00501*124429480*0*P*:~GS*HB*WV_MMIS_4MOLINA*WVTPID006574*20161028*1108*529486*X*005010X279A1~ST*271*589486*005010X279A1~BHT*0022*11*12X34*20161028*110845~HL*1**20*1~NM1*PR*2*WV_MMIS_4MOLINA*****PI*WV_MMIS_4MOLINA~HL*2*1*21*1~NM1*1P*2*GRANT MEMORIAL CAH*****XX*1598763666~HL*3*2*22*0~TRN*1*16302M000012737*9MOLINAELG~TRN*2*WVTPID006574-44263367-1*9MolinaGWI~NM1*IL*1*CORAL*BILLY****MI*7787262221~AAA*N**72*C~DMG*D8*19790807*M~SE*13*589486~GE*1*529486~IEA*1*124429480~</Payload><ErrorCode>Success</ErrorCode><ErrorMessage>Envelope was processed successfully.</ErrorMessage></ns1:COREEnvelopeRealTimeResponse></soapenv:Body></soapenv:Envelope>\r\n--uuid:798c582a-6d98-407f-99b4-ccb0bfdfe643+id=106--\r\n

                // sample response - TA1 - "\r\n--uuid:798c582a-6d98-407f-99b4-ccb0bfdfe643+id=76\r\nContent-ID: <http://tempuri.org/0>\r\nContent-Transfer-Encoding: 8bit\r\nContent-Type: application/xop+xml;charset=utf-8;type=\"application/soap+xml\"\r\n\r\n<soapenv:Envelope xmlns:soapenv=\"http://www.w3.org/2003/05/soap-envelope\"><soapenv:Body><ns1:COREEnvelopeRealTimeResponse xmlns:ns1=\"http://www.caqh.org/SOAP/WSDL/CORERule2.2.0.xsd\"><PayloadType>X12_TA1_Response_00501X231A1</PayloadType><ProcessingMode>RealTime</ProcessingMode><PayloadID>587ba37e-6366-40ee-8bab-b0a3360f7a7e</PayloadID><TimeStamp>2016-10-28T14:01:53Z</TimeStamp><SenderID>WV_MMIS_4MOLINA</SenderID><ReceiverID>WVTPID006574</ReceiverID><CORERuleVersion>2.2.0</CORERuleVersion><Payload>ISA*00*          *00*          *ZZ*WV_MMIS_4MOLINA*ZZ*WVTPID006574   *161028*1001*^*00501*000000104*0*P*:~TA1*124429472*161009*0930*R*025~IEA*1*000000104~</Payload><ErrorCode></ErrorCode><ErrorMessage></ErrorMessage></ns1:COREEnvelopeRealTimeResponse></soapenv:Body></soapenv:Envelope>\r\n--uuid:798c582a-6d98-407f-99b4-ccb0bfdfe643+id=76--\r\n"

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
                                log.ExceptionDetails("WVMCD ERR  " , errMsg);
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
                log.ExceptionDetails("WVMCD GetResponse Failed Total Elasped ticks" + Convert.ToString(_elapsedTicks) , ex);
            }
            return r;
        }





        private HttpWebRequest CreateWebRequest(string url)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            //webRequest.Headers.Add("SOAPAction", action);

            webRequest.ContentType = "application/soap+xml;charset=UTF-8;action=RealTimeTransaction";    // "multipart/related";  //"text/xml;charset='utf-8'";
            webRequest.Accept = "text/xml" ;  // "application/xop+xm"; // "application/soap+xml"; // "text/xml"; //"application/soap+xml";
            webRequest.Method = "POST";
            webRequest.Timeout = Convert.ToInt32(600000);
            return webRequest;
        }

        private   XmlDocument CreateSoapEnvelope(string edi)
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
            soap = soap + "<wsse:Password Type='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText'>" + _Passwd +  "</wsse:Password>";
            soap = soap + "</wsse:UsernameToken>";
            soap = soap + "</wsse:Security>";
            soap = soap + "</soapenv:Header>";
            soap = soap + "<soapenv:Body>";
            soap = soap + "<ns1:COREEnvelopeRealTimeRequest xmlns:ns1='http://www.caqh.org/SOAP/WSDL/CORERule2.2.0.xsd'>";
            soap = soap + "<PayloadType>X12_270_Request_005010X279A1</PayloadType>";
            soap = soap + "<ProcessingMode>RealTime</ProcessingMode>";
            soap = soap + "<PayloadID>" + Convert.ToString(g) + "</PayloadID>";
            soap = soap + "<TimeStamp>" + DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:fffZ")   + "</TimeStamp>";
            //soap = soap + "<SenderID>METPID005914</SenderID>";
            soap = soap + "<SenderID>" + _SenderID + "</SenderID>";
            soap = soap + "<ReceiverID>" +  _ReceiverID   + "</ReceiverID>";
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
        public int GetResponse_old()
        {
            int r = -1;
            //int i = 0;


            //_VMetricsLogging = 0;

            //_RES = string.Empty;
            //DateTime StartTime = DateTime.Now;
            //DateTime Endtime = default(DateTime);

            try
            {
                //    sPayloadType = _PayloadType;
                //    sProcessingMode = _ProcessingMode;
                //    sPayloadID = Guid.NewGuid().ToString();  //_PayloadID;
                //    sTimeStamp = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:fffZ");   //_TimeStamp;
                //    sSenderID = _SenderID;
                //    sReceiverID = _ReceiverID;
                //    sCORERuleVersion = _CORERuleVersion;
                //    sPayload = _REQ;
                //    ErrorMsg = "";

                //    string _serviceUri = _wsUrl;

                //    Uri _uri = new Uri(_serviceUri);
                //    EndpointAddress EndPoint = new EndpointAddress(_uri);

                //    Binding Binding = CreateCustomBinding();
                //    WestVirginaProxy.SOAPServiceClient client = new WestVirginaProxy.SOAPServiceClient(Binding, EndPoint);

                //    client.ClientCredentials.UserName.UserName = _UserName;
                //    client.ClientCredentials.UserName.Password = _Passwd;

                //    // client.BatchSubmitTransaction()
                //    if (_VMetricsLogging == 1)
                //    {
                //        log.ExceptionDetails("WEST VERGINIA  MEDICAID Request Details -  Request sent at = " + Convert.ToString(StartTime) + " | PayloadID=" + _PayloadID + " | Vendor Name= " + _VendorName + " | Payor Code= " + _PayorCode + " | URL=" + _wsUrl + " | sPayloadType =" + sPayloadID + " | ProcessingMode=" + _ProcessingMode + " | TimeStamp=" + sTimeStamp + " | SenderID=" + _SenderID + " | ReceiverID=" + _ReceiverID + " | ServiceTimeOut=" + _ServiceTimeOut + " | CORERuleVersion=" + _CORERuleVersion, _REQ);
                //    }

                //    ErrorMsg = client.RealTimeTransaction(ref sPayloadType, ref sProcessingMode, ref sPayloadID, ref sTimeStamp, ref sSenderID, ref sReceiverID, ref sCORERuleVersion, ref sPayload, out ErrorMsg);

                //    _RES = sPayload;


                //    if (_VMetricsLogging == 1)
                //    {

                //        log.ExceptionDetails("WEST VERGINIA MEDICAID ERR MSG", ErrorMsg + "|" + sPayload);

                //        log.ExceptionDetails("WEST VERGINIA MEDICAID Response Received  at " + Convert.ToString(DateTime.Now) + " | PayloadID=" + _PayloadID + " | Error Status= " + ErrorMsg + " Response = " + sPayload + " | Vendor Name =  " + _VendorName + "| Payor Code=" + _PayorCode + "|" + _wsUrl, _RES);
                //    }
                //    sPayload = "";
                r = 0;
            }
            catch (Exception ex)
            {
                r = -1;
                //Endtime = DateTime.Now;
                //_elapsedTicks = Endtime.Ticks - StartTime.Ticks;
                //log.ExceptionDetails("GetResponse Failed Total Elasped ticks" + Convert.ToString(_elapsedTicks) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl + "|" + _REQ, ex);
            }


            return r;
        }
        public int GetResponse_ContentTypeIssue()
        {
            int r = -1;
            int i = 0;


            _VMetricsLogging = 0;

            _RES = string.Empty;
            DateTime StartTime = DateTime.Now;
            DateTime Endtime = default(DateTime);

            try
            {
                //sPayloadType = _PayloadType;
                //sProcessingMode = _ProcessingMode;
                //sPayloadID = Guid.NewGuid().ToString();  //_PayloadID;
                //sTimeStamp = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:fffZ");   //_TimeStamp;
                //sSenderID = _SenderID;
                //sReceiverID = _ReceiverID;
                //sCORERuleVersion = _CORERuleVersion;
                //sPayload = _REQ;
                //ErrorMsg = "";

                //WestVirginaProxy.COREEnvelopeRealTimeRequest rq = new COREEnvelopeRealTimeRequest();
                //rq.PayloadType = sPayloadType;
                //rq.PayloadID = sPayloadID;
                //rq.TimeStamp = sTimeStamp;
                //rq.SenderID = sSenderID;
                //rq.ReceiverID = sReceiverID;
                //rq.CORERuleVersion = sCORERuleVersion;
                //rq.Payload = sPayload;
                //rq.ProcessingMode = sProcessingMode;


                //string _serviceUri = _wsUrl;

                //Uri _uri = new Uri(_serviceUri);
                //EndpointAddress EndPoint = new EndpointAddress(_uri);

                //Binding Binding = CreateCustomBinding();

                //WestVirginaProxy.CoreRealTimeClient cr = new CoreRealTimeClient(Binding, EndPoint);
                //cr.ClientCredentials.UserName.UserName = _UserName;
                //cr.ClientCredentials.UserName.Password = _Passwd;




                //// client.BatchSubmitTransaction()
                //if (_VMetricsLogging == 1)
                //{
                //    log.ExceptionDetails("WEST VERGINIA  MEDICAID Request Details -  Request sent at = " + Convert.ToString(StartTime) + " | PayloadID=" + _PayloadID + " | Vendor Name= " + _VendorName + " | Payor Code= " + _PayorCode + " | URL=" + _wsUrl + " | sPayloadType =" + sPayloadID + " | ProcessingMode=" + _ProcessingMode + " | TimeStamp=" + sTimeStamp + " | SenderID=" + _SenderID + " | ReceiverID=" + _ReceiverID + " | ServiceTimeOut=" + _ServiceTimeOut + " | CORERuleVersion=" + _CORERuleVersion, _REQ);
                //}

                //WestVirginaProxy.COREEnvelopeRealTimeResponse rs = new COREEnvelopeRealTimeResponse();
                //rs = cr.RealTimeTransaction(rq);
                //ErrorMsg = rs.ErrorMessage;
                //_RES = rs.Payload;

                //if (_VMetricsLogging == 1)
                //{
                //    log.ExceptionDetails("WEST VERGINIA MEDICAID ERR MSG", ErrorMsg + "|" + sPayload);
                //    log.ExceptionDetails("WEST VERGINIA MEDICAID Response Received  at " + Convert.ToString(DateTime.Now) + " | PayloadID=" + _PayloadID + " | Error Status= " + ErrorMsg + " Response = " + sPayload + " | Vendor Name =  " + _VendorName + "| Payor Code=" + _PayorCode + "|" + _wsUrl, _RES);
                //}
                //sPayload = "";
                r = 0;
            }
            catch (WebException ex)
            {
                _RES = ex.Message;
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    _RES = "Status Code : " + ((HttpWebResponse)ex.Response).StatusCode;
                    _RES = _RES + "Status Description : " + ((HttpWebResponse)ex.Response).StatusDescription;

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
    }
}
