using System;

using System.Text;
using System.Xml;
using System.IO;
using System.Net;

namespace DCSGlobal.EDI.Comunications
{

    
    public class ParamountHelth
    {



        /// <summary>
        /// PRODUCTION Webservice Call for PARAMOUNTHEALTH pass it a raw 270 get back a raw 271
        /// </summary>
        /// <param name="edi"> PRODUCTION Webservice Call for PARAMOUNTHEALTH pass it a raw 270 get back a raw 271</param>
        /// <returns>Raw EDI 271 String</returns>
    public static string CallWebService(string edi)
        {
          return GetData(edi, 0);
        }
    
        /// <summary>
        /// !!!!! TEST MODE Webservice Call for TEST MORE PARAMOUNTHEALTH pass it a raw 270 get back a raw 271
        /// </summary>
    /// <param name="edi"> !!!!! TEST MODE Webservice Call for TEST MORE PARAMOUNTHEALTH pass it a raw 270 get back a raw 271</param>
        /// <returns>Raw EDI 271 String</returns>
    public static string CallWebService(string edi, int Test)
    {
        return GetData(edi , 1);
    }
    /// <summary>
    /// !!!!! Advanced
    /// /// </summary>
    /// <param name="edi"> !!!!! Advanced MODE do not use
    /// <returns>Raw EDI 271 String</returns>
    public static string CallWebService(string edi, int Test, string URL, string UserName, string Passwd)
    {
        return GetData(edi, 2);
    }   
    private static string GetData(string EDI, int Test)
    {

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


            XmlDocument soapEnvelopeXml = CreateSoapEnvelope(EDI);
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




                return S;





            }
        }
        catch
        {

            return "boken";

        }
      
    
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
