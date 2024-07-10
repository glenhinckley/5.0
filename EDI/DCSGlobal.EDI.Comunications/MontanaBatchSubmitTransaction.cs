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
    class MontanaBatchSubmitTransaction : IDisposable
    {
    
        private logExecption log = new logExecption();
        private StringStuff ss = new StringStuff();

        private string _ConnectionString = string.Empty;
        private string _VendorName = "MONTSUB";
        private string _REQ = string.Empty;
        private string _RES = string.Empty;
        private string _PayorCode = string.Empty;
        private string _wsUrl = string.Empty;
        private string _UserName = string.Empty;
        private string _Passwd = string.Empty;
        private long _ServiceTimeOut = 30;
        private long _ReceiveTimeout = 30;


        private string sTimeStamp = string.Empty;
        private string sProcessingMode = string.Empty;
        private string sPayloadType = string.Empty;
        private string sSenderID = string.Empty;
        private string sReceiverID = string.Empty;
        private string sCORERuleVersion = string.Empty;



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
        private string _ProtocolType = string.Empty;

        long _elapsedTicks = 0;




        bool _disposed;

        private int _TaskID = 0;
        private string _EBRID = string.Empty;
        private int _VendorRetrys = 1;
        private int MLOG_ID = 0;
        private int _WaitTime = 0;
        private string B_REQ = string.Empty;

        private string stime = string.Empty;
        private string rtime = string.Empty;

        private string sPayload = string.Empty;
        private string B_RES = string.Empty;
        private string PayloadIDTX = string.Empty;
        private string PayloadIDRX = string.Empty;
        private string sPayloadID = string.Empty;

        ~MontanaBatchSubmitTransaction()
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
            string httpData = string.Empty;
            _RES = string.Empty;

            XmlDocument XMLEDI271 = new XmlDocument();
            string Err = string.Empty;

            DateTime StartTime = DateTime.Now;
            DateTime Endtime = default(DateTime);

            stime = string.Empty;
            rtime = string.Empty;
            string soapResult = string.Empty;

            try
            {

                sPayloadID = Guid.NewGuid().ToString();  //_PayloadID;
                _PayloadID = sPayloadID;

                PayloadIDTX = sPayloadID;

                int m = 0; //rnd.Next(0, 1000);

                Thread.Sleep(_WaitTime);

                ServicePointManager.Expect100Continue = true;

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




                _REQ = "ISA*00*          *00*          *ZZ*7230865*ZZ*100000         *180627*1544*^*00501*692612817*0*P*:~GS*HS*7230865*77039*20180627*1544*589486*X*005010X279A1~ST*270*589486*005010X279A1~BHT*0022*13*6324581441CC436*20180627*1544~HL*1**20*1~NM1*PR*2*Montana Medicaid*****PI*77039~HL*2*1*21*1~NM1*1P*2*RCHP*****XX*1346642899~HL*3*2*22*0~NM1*IL*1*PORTER*MEASHA*K***MI*483781947~DMG*D8*19690326~DTP*291*D8*20180627~EQ*30~SE*12*589486~GE*1*589486~IEA*1*692612817~";


              

                XmlDocument soapEnvelopeXml = CreateSoapEnvelope(_REQ);
                HttpWebRequest webRequest = CreateWebRequest(_wsUrl);


                Thread.Sleep(_WaitTime);

                InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

                sTimeStamp = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:fffZ");

                stime = sTimeStamp;

                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("MontanaBatchSubmitTransaction sent at " + Convert.ToString(sTimeStamp) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _REQ);
                }

                HttpWebResponse resp = (HttpWebResponse)webRequest.GetResponse();
                Stream S = resp.GetResponseStream();
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader sr = new StreamReader(S, encode);

                dynamic _with3 = sr;
                soapResult = _with3.ReadToEnd();
                _OrigRes = soapResult;

                rtime = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:fffZ");

                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("MontanaBatchSubmitTransaction Response Recived  at " + Convert.ToString(DateTime.Now), soapResult);
                }

                if (!string.IsNullOrEmpty(soapResult))
                {
                    try
                    {
                        httpData = CleanResponse(soapResult, "<soapenv:Body>", "</soapenv:Body>");

                        XMLEDI271.LoadXml(httpData);

                        var xnError = XMLEDI271.DocumentElement.GetElementsByTagName("ErrorCode");
                        if (xnError != null)
                        {
                            Err = xnError[0].InnerText;
                            if (_VMetricsLogging == 1 && Err != "Success")
                            {
                                log.ExceptionDetails("MontanaBatchSubmitTransaction Response ErrorCode ", Err);
                            }
                             
                        }


                        if (Err != string.Empty && Err == "Success")
                        {
                            r = 0;
                        }
                        if (Err != string.Empty && Err != "Success")
                        {
                            MLOG_ID = log.MLOG(_VendorName, Err, _EBRID, stime, rtime, B_RES, B_REQ, PayloadIDTX, PayloadIDRX, _OrigRes, _REQ);
                            // log.ExceptionDetails(_VendorName + " ErrorMSG = '" + ErrorMsg + "'  req and res are in MLOG with MLOG_ID " + Convert.ToString(MLOG_ID), Convert.ToString(DateTime.Now) + " | PayloadID=" + sPayloadID + " | Error Status= " + ErrorMsg + " Response = " + sPayload + " | Vendor Name =  " + _VendorName + "| Payor Code=" + _PayorCode + "|" + "waitstate=" + Convert.ToString(m) + "|" + _wsUrl);
                            r = -2;
                        }




                        var oPayloadID = XMLEDI271.DocumentElement.GetElementsByTagName("PayloadID");
                        if (oPayloadID != null)
                        {
                            PayloadIDRX = oPayloadID[0].InnerText;
                        }

                        var oPayloadType = XMLEDI271.DocumentElement.GetElementsByTagName("PayloadType");
                        if (oPayloadType != null)
                        {
                            sPayloadType = oPayloadType[0].InnerText;
                        }

                        //using (ValidateEDI vedi = new ValidateEDI())
                        //{
                        //    vedi.ConnectionString = _ConnectionString;
                        //    vedi.byString(sPayload);
                        //    B_RES = vedi.ReferenceIdentification;
                        //}
                     

                        //if (B_RES != B_REQ)
                        //{

                        //    MLOG_ID = log.MLOG(_VendorName, Err, _EBRID, stime, rtime, B_RES, B_REQ, PayloadIDTX, PayloadIDRX, sPayload, _REQ);
                        //    r = -4;
                        //    log.ExceptionDetails(_VendorName + " BHT03 mismatch req and res are in MLOG with MLOG_ID " + Convert.ToString(MLOG_ID), Convert.ToString(DateTime.Now) + " | PayloadID=" + sPayloadID + " | Error Status= " + ErrorMsg + " Response = " + sPayload + " | Vendor Name =  " + _VendorName + "| Payor Code=" + _PayorCode + "|" + "waitstate=" + Convert.ToString(m) + "|" + _wsUrl);

                        //}

                        //if (r < -3)
                        //{
                        //    if (PayloadIDTX != PayloadIDRX)
                        //    {
                        //        MLOG_ID = log.MLOG(_VendorName, Err, _EBRID, stime, rtime, B_RES, B_REQ, PayloadIDTX, PayloadIDRX, sPayload, _REQ);
                        //        //    log.MLOG(_EBRID, "", "", PayloadIDTX, PayloadIDRX, sPayload);
                        //        r = -5;
                        //        log.ExceptionDetails(_VendorName + "  Payload mismatch req and res are in MLOG with MLOG_ID " + Convert.ToString(MLOG_ID), Convert.ToString(DateTime.Now) + " | PayloadID=" + sPayloadID + " | Error Status= " + ErrorMsg + " Response = " + sPayload + " | Vendor Name =  " + _VendorName + "| Payor Code=" + _PayorCode + "|" + "waitstate=" + Convert.ToString(m) + "|" + _wsUrl);

                        //    }
                        //}
                    }
                    catch (Exception ex)
                    {
                        r = -3;
                        log.ExceptionDetails("MontanaBatchSubmitTransaction Reading Response Failed ", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                r = -1;
                Endtime = DateTime.Now;
                _elapsedTicks = Endtime.Ticks - StartTime.Ticks;
                log.ExceptionDetails("MontanaBatchSubmitTransaction GetResponse Failed Total Elasped ticks" + Convert.ToString(_elapsedTicks) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl + "|" + _REQ, ex);
            }
            return r;
        }

        

       private XmlDocument CreateSoapEnvelope(string edi)
        {
            string sGuid = string.Empty;
            string timestamp = string.Empty;
            string strResult = string.Empty;
            int payloadLength = 0;

            strResult = Base64Encode(edi);
            payloadLength = edi.Length;

            string sChkSum = string.Empty;
            sChkSum = BitConverter.ToString(System.Security.Cryptography.SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(edi)));
            sChkSum = sChkSum.Replace("-", "");




            Guid g;
            g = Guid.NewGuid();

            sGuid = g.ToString();

            string soap = "";

            timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:ffZ");


            XmlDocument soapEnvelop = new XmlDocument();


           
            soap = soap + Convert.ToString("<soap:Envelope xmlns:soap='http://www.w3.org/2003/05/soap-envelope' xmlns:cor='http://www.caqh.org/SOAP/WSDL/CORERule2.2.0.xsd'>");
            soap = soap + Convert.ToString("<soap:Header>");
            soap = soap + Convert.ToString("<wsse:Security soap:mustUnderstand='true' xmlns:wsse='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd' xmlns:wsu='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd'>");
            soap = soap + Convert.ToString("<wsse:UsernameToken wsu:Id='UsernameToken-78487AC9AAA3F96B18146076044685612'>");
            soap = soap + Convert.ToString("<wsse:Username>TMP669374</wsse:Username>");
            soap = soap + Convert.ToString("<wsse:Password Type='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText'>KQKHTZEJ2</wsse:Password>");
            soap = soap + Convert.ToString("</wsse:UsernameToken>");
            soap = soap + Convert.ToString("</wsse:Security>");
            soap = soap + Convert.ToString("</soap:Header>");
            soap = soap + Convert.ToString("<soap:Body>");
            soap = soap + Convert.ToString("<cor:COREEnvelopeBatchSubmission>");
            soap = soap + Convert.ToString("<PayloadType>X12_270_Request_005010X279A1</PayloadType>");
            soap = soap + Convert.ToString("<ProcessingMode>Batch</ProcessingMode>");
            soap = soap + Convert.ToString("<PayloadID>" + sGuid + "</PayloadID>");
            soap = soap + Convert.ToString("<PayloadLength>" + Convert.ToString(payloadLength) + "</PayloadLength>");
            soap = soap + Convert.ToString("<TimeStamp>2018-07-23T17:34:00-0400</TimeStamp>");

            //Neet to check how to get time stamp with above format...This is not like general time stamp.
            //soap = soap + Convert.ToString("<TimeStamp>" + timestamp + "</TimeStamp>");

            soap = soap + Convert.ToString("<SenderID>7230865</SenderID>");
            soap = soap + Convert.ToString("<ReceiverID>100000</ReceiverID>");
            soap = soap + Convert.ToString("<CORERuleVersion>2.2.0</CORERuleVersion>");
            soap = soap + Convert.ToString("<CheckSum>" + sChkSum + "</CheckSum>");
            soap = soap + Convert.ToString("<Payload>" + strResult + "</Payload>");
            soap = soap + Convert.ToString("</cor:COREEnvelopeBatchSubmission>");
            soap = soap + Convert.ToString("</soap:Body>");
            soap = soap + Convert.ToString("</soap:Envelope>");

            soapEnvelop.LoadXml(soap);
            return soapEnvelop;


        }



        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }

        }

        private HttpWebRequest CreateWebRequest(string url)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.ContentType = "application/soap+xml;charset=UTF-8;action=BatchSubmitTransaction";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            webRequest.Timeout = Convert.ToInt32(600000);

            return webRequest;
        }


        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
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
