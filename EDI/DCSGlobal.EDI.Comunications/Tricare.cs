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



using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml;





namespace DCSGlobal.EDI.Comunications
{
    class Tricare : IDisposable
    {

        private logExecption log = new logExecption();
        private StringStuff ss = new StringStuff();

        private string _ConnectionString = string.Empty;
        private string _VendorName = "TRICARE";
        private string _REQ = string.Empty;
        private string _RES = string.Empty;
        private string _PayorCode = string.Empty;
        private string _wsUrl = string.Empty;
        private string _UserName = string.Empty;
        private string _Passwd = string.Empty;
        private long _ServiceTimeOut = 30;
        private long _ReceiveTimeout = 30;
        private int _TTL = 30000;


        private DateTime _CreatedTimeStamp = DateTime.Now;


        //private string _TimeStamp = string.Empty;
        //private string _ProcessingMode = string.Empty;
        //private string _PayloadType = string.Empty;
        //private string _SenderID = string.Empty;
        //private string _ReceiverID = string.Empty;
        //private string _CORERuleVersion = string.Empty;



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

        ~Tricare()
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

        /// <summary>
        /// time to live in millseconds
        /// defaults to 30,000 millesecs or 30 sec
        /// </summary>
        public int TTL
        {

            set
            {

                _TTL = value;
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


        private int GoToVendor()
        {

            int r = -1;

            string soap = string.Empty;

            string SoapHeaders = string.Empty;
            string SoapHeader = string.Empty;
            string SoapBody = string.Empty;
            string DigestMethod = string.Empty;

            DateTime CurrentTimeStamp = DateTime.Now;
            DateTime ExpiresTimeStamp = _CreatedTimeStamp.AddMilliseconds(_TTL); 

            /*******************************************************************************************************************************************************************
            * build the soap header
            *******************************************************************************************************************************************************************/
            SoapHeaders = SoapHeaders + "<headers xmlns:date=\"http://exslt.org/dates-and-times\">";
            SoapHeaders = SoapHeaders + "	<Content-Type>application/soap+xml; charset=utf-8; action=\"RealTimeTransaction\"</Content-Type>";
            SoapHeaders = SoapHeaders + "	<Cache-Control>no-cache</Cache-Control>";
            SoapHeaders = SoapHeaders + "	<Pragma>no-cache</Pragma>";
            SoapHeaders = SoapHeaders + "	<User-Agent>NET/4.0</User-Agent>";
            SoapHeaders = SoapHeaders + "	<Host>production.commercial.services.bcbssc.com</Host>";
            SoapHeaders = SoapHeaders + "	<Accept>text/html, image/gif, image/jpeg, *; q=.2, */*; q=.2</Accept>";
            SoapHeaders = SoapHeaders + "	<Content-Length>xxxx</Content-Length>";
            SoapHeaders = SoapHeaders + "	<X-Forwarded-For>xx.xxx.xx.xxx,xx.xxx.x.xx</X-Forwarded-For>";
            SoapHeaders = SoapHeaders + "	<X-EXTERNAL-CONSUMER>: TRUE</X-EXTERNAL-CONSUMER>";
            SoapHeaders = SoapHeaders + "	<Via>1.1 xxxxxxxxxxx-</Via>";
            SoapHeaders = SoapHeaders + "	<X-Client-IP>xx.xxx.xx.xxx</X-Client-IP>";
            SoapHeaders = SoapHeaders + "	<X-Global-Transaction-ID>xxxxxxxxxx</X-Global-Transaction-ID>";
            SoapHeaders = SoapHeaders + "	<UUID>xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx</UUID>";
            SoapHeaders = SoapHeaders + "</headers>";

            

            /*******************************************************************************************************************************************************************
             * build the soap body
             *******************************************************************************************************************************************************************/ 
           // SoapBody = SoapBody + "<soap:Body wsu:Id=\"id-xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\">";




            SoapBody = SoapBody + "<soap:Body xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\" wsu:Id=\"Id-c74338f8-88df-4ec1-ac5d-d1c748525d32\">";
            SoapBody = SoapBody + "	<core2:COREEnvelopeRealTimeRequest xmlns:cor=\"http://www.caqh.org/SOAP/WSDL/CORERule2.2.0.xsd\">";
            SoapBody = SoapBody + "		<PayloadType>"  +  _PayloadType  + "</PayloadType>";
            SoapBody = SoapBody + "		<ProcessingMode>" + _ProcessingMode + "</ProcessingMode>";
            SoapBody = SoapBody + "		<PayloadID>" +  _PayloadID  +  "</PayloadID>";
            SoapBody = SoapBody + "		<TimeStamp>" + String.Format("{0:u}", CurrentTimeStamp) + "</TimeStamp>";
            SoapBody = SoapBody + "		<SenderID>" + _SenderID + "</SenderID>";
            SoapBody = SoapBody + "		<ReceiverID>" + _ReceiverID + "</ReceiverID>";
            SoapBody = SoapBody + "		<CORERuleVersion>2.2.0</CORERuleVersion>";
            SoapBody = SoapBody + "		<Payload>" + _REQ + "</Payload>";
            SoapBody = SoapBody + "	</core2:COREEnvelopeRealTimeRequest>";
            SoapBody = SoapBody + "</soap:Body>";



            /*******************************************************************************************************************************************************************
            * build the soap header with the security info 
            *******************************************************************************************************************************************************************/ 

            SoapHeader = SoapHeader + "<soap:Header>";
           
            SoapHeader = SoapHeader + "	<bcbs:transactionUUID xmlns:bcbs=\"http://services.bcbssc.com\">xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx</bcbs:transactionUUID>";
           
            
            
            
            SoapHeader = SoapHeader + "	<a:MessageID wsu:Id=\"id-022061d6-70c0-41d0-89f3-db256d3c04fd\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\" xmlns:a=\"http://www.w3.org/2005/08/addressing\">xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx</a:MessageID>";
           
            
            SoapHeader = SoapHeader + "	<wsse:Security soap:mustUnderstand=\"true\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\" xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\">";
            
            
            SoapHeader = SoapHeader + "		<wsu:Timestamp wsu:Id=\"TS-0a1bc389-d8e5-4f3f-97b7-48a2cce15138\">";
            SoapHeader = SoapHeader + "			<wsu:Created>" + String.Format("{0:u}", _CreatedTimeStamp) + "</wsu:Created>";
            SoapHeader = SoapHeader + "			<wsu:Expires>" + String.Format("{0:u}", ExpiresTimeStamp) + "</wsu:Expires>";
            SoapHeader = SoapHeader + "		</wsu:Timestamp>";
          
            
            
            SoapHeader = SoapHeader + "		<wsse:BinarySecurityToken EncodingType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary\" ValueType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3\" wsu:Id=\"X509-5f4c96e6-c6ef-49cd-a6d2-f30a1eafdde2\">YOUR PEM FILE (CERT) CONTENTS GO HERE (VERY LONG STRING)xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx...</wsse:BinarySecurityToken>";
           
            
            
            SoapHeader = SoapHeader + "		<ds:Signature Id=\"SIG-xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\">";
            SoapHeader = SoapHeader + "	    <Signature xmlns=\"http://www.w3.org/2000/09/xmldsig#\">";
            
            
            
            
            SoapHeader = SoapHeader + "			<ds:SignedInfo>";
            SoapHeader = SoapHeader + "				<ds:CanonicalizationMethod Algorithm=\"http://www.w3.org/TR/2001/REC-xml-c14n-20010315\"/>";
            SoapHeader = SoapHeader + "				<ds:SignatureMethod Algorithm=\"http://www.w3.org/2001/04/xmldsig-more#rsa-sha256\"/>";




            SoapHeader = SoapHeader + "				<ds:Reference URI=\"#Id-13978df4-83e1-4136-8fe1-55987eb55753\">";
                       
            SoapHeader = SoapHeader + "					<ds:Transforms>";
            SoapHeader = SoapHeader + "						<ds:Transform Algorithm=\"http://www.w3.org/2001/10/xml-exc-c14n#\">";
            //SoapHeader = SoapHeader + "							<InclusiveNamespaces PrefixList=\"\" xmlns=\"http://www.w3.org/2001/10/xml-exc-c14n#\"/>";
            SoapHeader = SoapHeader + "						</ds:Transform>";
            SoapHeader = SoapHeader + "					</ds:Transforms>";
            
            
            
            SoapHeader = SoapHeader + "					<ds:DigestMethod Algorithm=\"http://www.w3.org/2001/04/xmlenc#sha256\"/>";
            SoapHeader = SoapHeader + "					<ds:DigestValue>xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx</ds:DigestValue>";
           
            SoapHeader = SoapHeader + "				</ds:Reference>";
            
            
            
            
            SoapHeader = SoapHeader + "				<ds:Reference URI=\"#id-022061d6-70c0-41d0-89f3-db256d3c04fd\">";
            SoapHeader = SoapHeader + "					<ds:Transforms>";
           
            SoapHeader = SoapHeader + "						<ds:Transform Algorithm=\"http://www.w3.org/2001/10/xml-exc-c14n#\">";
           // SoapHeader = SoapHeader + "							<InclusiveNamespaces PrefixList=\"soap\" xmlns=\"http://www.w3.org/2001/10/xml-exc-c14n#\"/>";
            SoapHeader = SoapHeader + "						</ds:Transform>";
           
            SoapHeader = SoapHeader + "					</ds:Transforms>";
            
            SoapHeader = SoapHeader + "					<ds:DigestMethod Algorithm=\"http://www.w3.org/2001/04/xmlenc#sha256\"/>";
            SoapHeader = SoapHeader + "					<ds:DigestValue>xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx</ds:DigestValue>";
            
            
            SoapHeader = SoapHeader + "				</ds:Reference>";
            
            SoapHeader = SoapHeader + "			</ds:SignedInfo>"; 
            
            
            SoapHeader = SoapHeader + "			<ds:SignatureValue>xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx==</ds:SignatureValue>";
 
            SoapHeader = SoapHeader + "  <KeyInfo>";
            SoapHeader = SoapHeader + "     <wsse:SecurityTokenReference xmlns=\"\">";
            SoapHeader = SoapHeader + "         <wsse:Reference ValueType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3\" URI=\"#SecurityToken-406dcf36-4ade-4503-857f-4b2ddbbf3f11\"/>";
            SoapHeader = SoapHeader + "     </wsse:SecurityTokenReference>";
            SoapHeader = SoapHeader + "  </KeyInfo>";
            
            
            
            
            //SoapHeader = SoapHeader + "				<ds:Reference URI=\"#TS-0a1bc389-d8e5-4f3f-97b7-48a2cce15138\">";
            //SoapHeader = SoapHeader + "					<ds:Transforms>";
            //SoapHeader = SoapHeader + "						<ds:Transform Algorithm=\"http://www.w3.org/2001/10/xml-exc-c14n#\">";
            //SoapHeader = SoapHeader + "							<InclusiveNamespaces PrefixList=\"wsse soap\" xmlns=\"http://www.w3.org/2001/10/xml-exc-c14n#\"/>";
            //SoapHeader = SoapHeader + "						</ds:Transform>";
            //SoapHeader = SoapHeader + "					</ds:Transforms>";
            //SoapHeader = SoapHeader + "					<ds:DigestMethod Algorithm=\"http://www.w3.org/2001/04/xmlenc#sha256\"/>";
            //SoapHeader = SoapHeader + "					<ds:DigestValue>xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx</ds:DigestValue>";
            //SoapHeader = SoapHeader + "				</ds:Reference>";

            //SoapHeader = SoapHeader + "			<ds:KeyInfo Id=\"KI-da5196e0-90c1-41fc-b297-f04716b33b02\">";
            //SoapHeader = SoapHeader + "				<wsse:SecurityTokenReference wsu:Id=\"STR-xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx\">";
            //SoapHeader = SoapHeader + "					<wsse:Reference URI=\"#X509-xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx\" ValueType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3\"/>";
            //SoapHeader = SoapHeader + "				</wsse:SecurityTokenReference>";
            //SoapHeader = SoapHeader + "			</ds:KeyInfo>";
            SoapHeader = SoapHeader + " </ds:Signature>";
            SoapHeader = SoapHeader + "</wsse:Security>";
          
            
            SoapHeader = SoapHeader + "</soap:Header>";


            /*******************************************************************************************************************************************************************
             * build the soap msg
             *******************************************************************************************************************************************************************/
            soap = soap + "<?xml version='1.0'?>";
            soap = soap + "<message>";
        //    soap = soap + SoapHeaders;
            soap = soap + "<xml xmlns:date=\"http://exslt.org/dates-and-times\">";
            soap = soap + "<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\">";
            soap = soap + SoapHeader;
            soap = soap + SoapBody;
            soap = soap + "</soap:Envelope>";
            soap = soap + "</message>";
            soap = soap + "</xml>";









            return r;

        }


        private static XmlElement EncryptMessage(XmlElement msgBody)
        {
            StoreName storeName = (StoreName)Enum.Parse(typeof(StoreName), "My");
            StoreLocation storeLocation = (StoreLocation)Enum.Parse(typeof(StoreLocation), "LocalMachine");

            X509Certificate2 cert = X509Helper.GetCertificate(storeName, storeLocation, "CN=Something");
            SignedXml signedXml = new SignedXml(msgBody);

            signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;
            signedXml.SigningKey = cert.PrivateKey;
            signedXml.KeyInfo.AddClause(new System.Security.Cryptography.Xml.KeyInfoX509Data(cert));

            Reference tRef = new Reference("");

            XmlDsigExcC14NTransform env = new XmlDsigExcC14NTransform();

            tRef.AddTransform(env);

            signedXml.AddReference(tRef);
            signedXml.ComputeSignature();

            XmlElement xmlDsig = signedXml.GetXml();
            xmlDsig.SetAttribute("Id", "Signature-1");

            return xmlDsig;
        }



        private HttpWebRequest CreateWebRequest(string url)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.ContentType = "application/soap+xml;charset=UTF-8;action=RealTimeTransaction";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            webRequest.Timeout = Convert.ToInt32(_TTL);
            webRequest.Headers.Add("X-SSL-Client-Name", "DCS Global Systems");
            webRequest.ServicePoint.Expect100Continue = false;
            X509Certificate2 cert = new X509Certificate2();
            cert = SelectTRICARELocalCertificate();

            if (cert != null)
            {
                webRequest.ClientCertificates.Add(cert);
            }

            return webRequest;
        }

        private XmlDocument CreateSoapEnvelope(string edi)
        {

            string soap = "";



            XmlDocument soapEnvelop = new XmlDocument();


            soap = soap + Convert.ToString("<soap:Envelope xmlns:soap='http://www.w3.org/2003/05/soap-envelope'>");
            soap = soap + Convert.ToString("<soap:Body>");
            soap = soap + Convert.ToString("<cor:COREEnvelopeRealTimeRequest  xmlns:cor='http://www.caqh.org/SOAP/WSDL/CORERule4.0.0.xsd'>");
            soap = soap + Convert.ToString("<PayloadType>X12_270_Request_005010X279A1</PayloadType>");
            soap = soap + Convert.ToString("<ProcessingMode>RealTime</ProcessingMode>");
            soap = soap + Convert.ToString("<PayloadID>" + sPayloadID + "</PayloadID>");
            soap = soap + Convert.ToString("<TimeStamp>" + DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:22Z") + "</TimeStamp>");
            soap = soap + Convert.ToString("<SenderID>" + _SenderID + "</SenderID>");
            soap = soap + Convert.ToString("<ReceiverID>" + _ReceiverID + "</ReceiverID>");
            soap = soap + Convert.ToString("<CORERuleVersion>4.0.0</CORERuleVersion>");
            soap = soap + Convert.ToString("<Payload>" + edi + "</Payload>");
            soap = soap + Convert.ToString("</cor:COREEnvelopeRealTimeRequest>");
            soap = soap + Convert.ToString("</soap:Body>");
            soap = soap + Convert.ToString("</soap:Envelope>");


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

        private static string CleanISAResponse(string haystack, string needle1, string needle2)
        {
            //int istart = String.InStr(haystack, needle1);
            int istart = haystack.IndexOf(needle1, 0);
            if (istart >= 0)
            {
                //int istop = String.InStr(istart, haystack, needle2);
                int istop = haystack.IndexOf(needle2, istart);
                if (istop > 0)
                {
                    //string value = haystack.Substring(istart + needle1.Length, istop - istart - needle1.Length);
                    string value = haystack.Substring(istart, istop - istart);
                    return value;
                }
            }
            return null;
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


        public static X509Certificate2 SelectTRICARELocalCertificate()
        {
            //Dim HttpProps As String() = {"POST", "application", "octet-stream", "A null HTTP response object was returned.", "priv"}
            try
            {
                X509Store certStore = new X509Store(StoreName.My);
                certStore.Open(OpenFlags.OpenExistingOnly & OpenFlags.ReadOnly);
                X509Certificate2Collection clientCertificates = certStore.Certificates;
                foreach (X509Certificate2 oCert in clientCertificates)
                {
                    if (oCert.Issuer.Contains("HPHC2048"))
                    {
                        return oCert;
                    }
                }

            }
            catch (System.Security.Authentication.AuthenticationException ae)
            {
                //errLog.WriteEvent(ae.Message)
            }
            return null;


        }
    }
}
