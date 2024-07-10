using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using DCSGlobal.BusinessRules.Logging;
using System.Threading;
using System.ServiceModel;
using System.ServiceModel.Channels;



namespace DCSGlobal.EDI.Comunications
{
    class NebraskaMedicaid : IDisposable
    {


        private StringStuff ss = new StringStuff();
        private logExecption log = new logExecption();

        private long _ServiceTimeOut = 120000;
        private long _ReceiveTimeout = 120000;

        private string _PayloadID = string.Empty;
        private string _ConnectionString = string.Empty;


        private string _wsUrl = string.Empty;
        private string _ClientId = string.Empty;
        private string _PreAuthenticate = string.Empty;

        private string _UserName = string.Empty;
 
        private string _Passwd = string.Empty;
        private string _apiKey = string.Empty;
        private string _ContentType = string.Empty;
        private string _Method = string.Empty;
      
        private string _Token = string.Empty;

        private string _ProtocolType = string.Empty;
        private int _VMetricsLogging = 0;
       
        private string _Accept = string.Empty;
        private string _LogToREQRES = string.Empty;

        private string _VendorInputType = string.Empty;
        private string _VendorName = "NEMCD";
        private string _PayorCode = string.Empty;

        private string _AppName = string.Empty;


        private string SYNC_TIMEOUT = string.Empty;
        private string SUBMISSION_TIMEOUT = string.Empty;
        private string _myResponses = string.Empty;
        private string _myResponses1 = string.Empty;

        private string _REQ = string.Empty;
        private string _RES = string.Empty;

        
        private string _TimeStamp = string.Empty;
        private string _ProcessingMode = string.Empty;
        private string _PayloadType = string.Empty;
        private string _SenderID = string.Empty;
        private string _ReceiverID = string.Empty;
        private string _CORERuleVersion = string.Empty;
        private string _Payload = string.Empty;
 

        long _elapsedTicks = 0;

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

        ~NebraskaMedicaid()
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


        public string VendorInputType
        {

            set
            {

                _VendorInputType = value;
            }
        }


        public string wsUrl
        {
            set
            {
                _wsUrl = value;
            }
        }



        public string VendorName
        {

            set
            {

                _VendorName = value;
            }
        }



        public int ServiceTimeOut
        {
            set { _ServiceTimeOut = value; }
        }

        public string PayorCode
        {

            set
            {

                _PayorCode = value;
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

        public long ElaspedTicks
        {

            get
            {
                return _elapsedTicks;

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


        //public static string CallWebService(string edi)
        //{

        //    return GetData(edi, 0);

        //}





        //public static string CallWebService(string edi, int Test)
        //{

        //    return GetData(edi, 1);

        //}



        //public static string CallWebService(string edi, int Test, string URL, string UserName, string Passwd)
        //{

        //    return GetData(edi, 2);

        //}

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
           
            string s1 = null;

            _RES = string.Empty;

            XmlDocument XMLEDI271 = new XmlDocument();
            string Err = string.Empty;
           
            DateTime StartTime = DateTime.Now;
            DateTime Endtime = default(DateTime);

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
                _PayloadID = sPayloadID;

                PayloadIDTX = sPayloadID;

                int m = 0; //rnd.Next(0, 1000);

                Thread.Sleep(_WaitTime);

                XmlDocument soapEnvelopeXml = CreateSoapEnvelope(_REQ);
                HttpWebRequest webRequest = CreateWebRequest(_wsUrl, _wsUrl);
                InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

                sTimeStamp = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:fffZ");

                stime = sTimeStamp;

                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("NE MEDICAID sent at " + Convert.ToString(sTimeStamp) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _REQ);
                }


                
                IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);
                asyncResult.AsyncWaitHandle.WaitOne();

                // get the response from the completed web request.

                string soapResult;
                using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
                {
                    using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                    {
                        soapResult = rd.ReadToEnd();
                        _OrigRes = soapResult;
                    }
                    
                    rtime = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:fffZ");

                    if (_VMetricsLogging == 1)
                    {
                        log.ExceptionDetails("NE MCD Response Recived  at " + Convert.ToString(DateTime.Now), soapResult);
                    }
                }

               
                if (!string.IsNullOrEmpty(soapResult))
                {
                    try
                    {
                        XMLEDI271.LoadXml(soapResult);
                        Err = XMLEDI271.GetElementsByTagName("ErrorCode")[0].InnerText;
                        ErrorMsg = XMLEDI271.GetElementsByTagName("ErrorMessage")[0].InnerText;
                        PayloadIDRX = XMLEDI271.GetElementsByTagName("PayloadID")[0].InnerText;
                        if (Err == "Success")
                        {
                            s1 = XMLEDI271.GetElementsByTagName("Payload")[0].InnerText;
                            _RES = s1;
                            sPayload = _RES;
                            if (_VMetricsLogging == 1)
                            {
                                log.ExceptionDetails("NE MCD REcived  at " + Convert.ToString(rtime) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _RES);
                            }
                            r = 0;
                        }
                        if (ErrorMsg != string.Empty && ErrorMsg != "Success")
                        {
                            MLOG_ID = log.MLOG(_VendorName, ErrorMsg, _EBRID, stime, rtime, B_RES, B_REQ, PayloadIDTX, PayloadIDRX, _RES, _REQ);
                            r = -2;
                        }
                        if (!_RES.Contains("ISA"))
                        {
                            MLOG_ID = log.MLOG(_VendorName, ErrorMsg, _EBRID, stime, rtime, B_RES, B_REQ, PayloadIDTX, PayloadIDRX, _RES, _REQ);
                            r = -3;
                        }

                        using (ValidateEDI vedi = new ValidateEDI())
                        {
                            vedi.ConnectionString = _ConnectionString;
                            vedi.byString(sPayload);
                            B_RES = vedi.ReferenceIdentification;
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
                    catch (Exception ex)
                    {
                        r = -1;
                        log.ExceptionDetails("NE MCD Reading Response Failed ", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                r = -1;
                Endtime = DateTime.Now;
                _elapsedTicks = Endtime.Ticks - StartTime.Ticks;
                log.ExceptionDetails("NE MCD GetResponse Failed Total Elasped ticks" + Convert.ToString(_elapsedTicks) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl + "|" + _REQ, ex);
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

            webRequest.Timeout = 600000;

            return webRequest;

        }




        private XmlDocument CreateSoapEnvelope(string edi)
        {
            string strTimeStamp = string.Empty;
            string soap = "";

           
            XmlDocument soapEnvelop = new XmlDocument();
            
            soap = "";

            soap = soap + "<soapenv:Envelope xmlns:soapenv='http://www.w3.org/2003/05/soap-envelope'>";

            soap = soap + "<soapenv:Header>";

            soap = soap + "<wsse:Security xmlns:wsse='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd' soapenv:mustUnderstand='true'>";

            soap = soap + "<wsse:UsernameToken xmlns:wsu='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wsswssecurity-utility-1.0.xsd'  wsu:Id='UsernameToken-21621663'>";

            soap = soap + Convert.ToString("<wsse:Username>" + _UserName + "</wsse:Username>");

            soap = soap + Convert.ToString("<wsse:Password Type=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-usernametoken-profile-1.0#PasswordText\">" + _Passwd + "</wsse:Password>");

            soap = soap + "</wsse:UsernameToken>";

            soap = soap + "</wsse:Security>";

            soap = soap + "</soapenv:Header>";

            soap = soap + "<soapenv:Body>";

            soap = soap + "<ns1:COREEnvelopeRealTimeRequest xmlns:ns1='http://www.caqh.org/SOAP/WSDL/CORERule2.2.0.xsd'>";

            soap = soap + "<PayloadType>X12_270_Request_005010X279A1</PayloadType>";
            soap = soap + "<ProcessingMode>RealTime</ProcessingMode>";
            soap = soap + "<PayloadID>" + Convert.ToString(sPayloadID) + "</PayloadID>";
            //soap = soap + "<TimeStamp>" + DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:fffZ") + "</TimeStamp>";
            //strTimeStamp = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:ff") + "+03:00";
            //soap = soap + "<TimeStamp>" + strTimeStamp + "</TimeStamp>";
            soap = soap + Convert.ToString("<TimeStamp>2014-07-09T11:35:45+03:00</TimeStamp>"); 
           

            soap = soap + "<SenderID>" + _SenderID + "</SenderID>";
            soap = soap + "<ReceiverID>" + _ReceiverID + "</ReceiverID>";
            soap = soap + "<CORERuleVersion>2.2.0</CORERuleVersion>";
            soap = soap + "<Payload><![CDATA[" + edi + "]]></Payload>";
            //soap = soap + "<Payload><![CDATA[ISA*00*          *00*          *ZZ*DCSGLOBALEDIEXG*ZZ*MMISNEBR       *150709*1030*^*00501*100589486*0*P*>~GS*HS*DCSGLOBALEDIEXG*MMISNEBR*20150709*103000*932221455*X*005010X279A1~ST*270*589486*005010X279A1~BHT*0022*13*TRANSACTIONID124*20150709*1251~HL*1**20*1~NM1*PR*2*NEBRASKA MEDICAID*****PI*NEMEDICAID~HL*2*1*21*1~NM1*1P*2*BRYAN HOSPITAL*****XX*1528006103~HL*3*2*22*0~NM1*IL*1*GOETZ*TIFFANY****MI*50621206101~DMG*D8*19820112~DTP*291*D8*20150709~EQ*30~SE*12*589486~GE*1*932221455~IEA*1*100589486]]></Payload>";
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
