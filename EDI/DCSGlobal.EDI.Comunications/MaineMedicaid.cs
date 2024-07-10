using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using DCSGlobal.EDI.Comunications.MainMedicaidProxy;

//using System.Net;
//using System.Xml;
//using System.IO;
//using System;
//using System.Web.Services.Protocols;
//using System.ServiceModel;
//using System.Runtime.Serialization;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using DCSGlobal.BusinessRules.Logging;
//using DCSGlobal.EDI.Comunications.MainMedicaidProxy;
//using System.Web.Services.Protocols;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.Net;
namespace DCSGlobal.EDI.Comunications
{
    class MaineMedicaid : IDisposable
    {
        private logExecption log = new logExecption();
        private StringStuff ss = new StringStuff();

        private int _WaitTime = 0;

        private string _BindingName = string.Empty;


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

        private long _elapsedTicks = 0;

        private string ErrorMsg = "";

        private string sPayloadID = string.Empty;
        private string sTimeStamp = string.Empty;
        private string sProcessingMode = string.Empty;
        private string sPayloadType = string.Empty;
        private string sSenderID = string.Empty;
        private string sReceiverID = string.Empty;
        private string sCORERuleVersion = string.Empty;
        private string sPayload = string.Empty;
        private string _ProtocolType = string.Empty;
        private int _VMetricsLogging = 0;

        private string _EBRID = string.Empty;

        private string _PayloadID = string.Empty;
        private string _TimeStamp = string.Empty;
        private string _ProcessingMode = string.Empty;
        private string _PayloadType = string.Empty;
        private string _SenderID = string.Empty;
        private string _ReceiverID = string.Empty;
        private string _CORERuleVersion = string.Empty;
        private string _Payload = string.Empty;
        private int _TaskID = 0;
        private string B_REQ = string.Empty;
        private string B_RES = string.Empty;
        private int _VendorRetrys = 1;
        private int MLOG_ID = 0;

        private string PayloadIDTX = string.Empty;
        private string PayloadIDRX = string.Empty;
        private string stime = string.Empty;
        private string rtime = string.Empty;


        bool _disposed;
        ~MaineMedicaid()
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


        public string BHT03
        {

            set
            {

                B_REQ = value;
            }
        }

        public string EBRID
        {

            set
            {

                _EBRID = value;
            }
        }



        public int WaitTime
        {

            set
            {

                _WaitTime = value;
            }
        }


        public int VendorRetrys
        {

            set
            {

                _VendorRetrys = value;
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
                    //Console.WriteLine(_EBRID + " retry " + Convert.ToString(VendorTrys) + " Error Message " + ErrorMsg);
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



        public Binding CreateCustomBinding()
        {


            CustomBinding binding = new CustomBinding();
            binding.Name = B_REQ;

            TimeSpan _tSendTimeout = TimeSpan.FromSeconds(_ServiceTimeOut);   //new TimeSpan(0, 0, 30);
            TimeSpan _tReceiveTimeout = TimeSpan.FromSeconds(_ReceiveTimeout); //new TimeSpan(0, 0, 30);

            binding.SendTimeout = _tSendTimeout;
            binding.ReceiveTimeout = _tReceiveTimeout;
            MtomMessageEncodingBindingElement mTom = new MtomMessageEncodingBindingElement();
            mTom.MessageVersion = MessageVersion.Soap12;


            SecurityBindingElement sbe = SecurityBindingElement.CreateUserNameOverTransportBindingElement();
            sbe.MessageSecurityVersion = MessageSecurityVersion.WSSecurity11WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11BasicSecurityProfile10;

            sbe.SecurityHeaderLayout = SecurityHeaderLayout.Strict;
            sbe.IncludeTimestamp = false;
            sbe.SetKeyDerivation(true);
            sbe.KeyEntropyMode = System.ServiceModel.Security.SecurityKeyEntropyMode.ServerEntropy;
            binding.Elements.Add(sbe);
            binding.Elements.Add(mTom);
            binding.Elements.Add(new HttpsTransportBindingElement());
            return new CustomBinding(binding);
        }




        private int GoToVendor()
        {
            DateTime StartTime = DateTime.Now;
            DateTime Endtime = default(DateTime);

            int r = -1;
            sPayloadType = string.Empty;
            _RES = string.Empty;


            PayloadIDTX = string.Empty;
            PayloadIDRX = string.Empty;
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
                   // ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;
                }

                // _VMetricsLogging = 1;


                sPayloadType = _PayloadType;
                sProcessingMode = _ProcessingMode;
                sPayloadID = Guid.NewGuid().ToString();  //_PayloadID;
                //_TimeStamp;
                sSenderID = _SenderID;
                sReceiverID = _ReceiverID;
                sCORERuleVersion = _CORERuleVersion;
                sPayload = _REQ;
                ErrorMsg = "";

                sPayload = string.Empty;

                string _serviceUri = _wsUrl;     //"https://mainecare.maine.gov/CAQH_SOAPService/SOAPService.svc";

                Uri _uri = new Uri(_serviceUri);
                EndpointAddress EndPoint = new EndpointAddress(_uri);

                Binding Binding = CreateCustomBinding();
                MainMedicaidProxy.SOAPServiceClient client = new MainMedicaidProxy.SOAPServiceClient(Binding, EndPoint);

                client.ClientCredentials.UserName.UserName = _UserName;  //  "Dcsglobal";
                client.ClientCredentials.UserName.Password = _Passwd;  // "Dcsglobal#1";
                // client.ConnectionString = _ConnectionString;


                sPayload = _REQ;
                //Random rnd = new Random();
                int m = 0; //rnd.Next(0, 1000);
                //  m = m * 5;
                Thread.Sleep(_WaitTime);

                sTimeStamp = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:fffZ");

                // client.BatchSubmitTransaction()
                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails(_VendorName + " Request sent at = " + Convert.ToString(StartTime) + " | PayloadID=" + sPayloadID + " | Vendor Name= " + _VendorName + " | Payor Code= " + _PayorCode + " | URL=" + _wsUrl + " | sPayloadType =" + sPayloadType + " | ProcessingMode=" + _ProcessingMode + " | TimeStamp=" + sTimeStamp + " | SenderID=" + _SenderID + " | ReceiverID=" + _ReceiverID + " | ServiceTimeOut=" + _ServiceTimeOut + " | CORERuleVersion=" + _CORERuleVersion + " | WaitTime " + Convert.ToString(_WaitTime), _REQ);
                }

                stime = sTimeStamp;
                PayloadIDTX = sPayloadID;

                ErrorMsg = client.RealTimeTransaction(sPayloadType, sProcessingMode, ref  sPayloadID, ref sTimeStamp, sSenderID, sReceiverID, sCORERuleVersion, ref sPayload, out ErrorMsg);

                PayloadIDRX = sPayloadID;
                rtime = sTimeStamp;

                if (ErrorMsg == "Success")
                {
                    _RES = sPayload;
                    r = 0;
                }
                else
                {
                    MLOG_ID = log.MLOG(_VendorName, ErrorMsg, _EBRID, stime, rtime, B_RES, B_REQ, PayloadIDTX, PayloadIDRX, sPayload, _REQ);
                    // log.ExceptionDetails(_VendorName + " ErrorMSG = '" + ErrorMsg + "'  req and res are in MLOG with MLOG_ID " + Convert.ToString(MLOG_ID), Convert.ToString(DateTime.Now) + " | PayloadID=" + sPayloadID + " | Error Status= " + ErrorMsg + " Response = " + sPayload + " | Vendor Name =  " + _VendorName + "| Payor Code=" + _PayorCode + "|" + "waitstate=" + Convert.ToString(m) + "|" + _wsUrl);

                    r = -1;
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
                    r = -2;
                    log.ExceptionDetails(_VendorName + " BHT03 mismatch req and res are in MLOG with MLOG_ID " + Convert.ToString(MLOG_ID), Convert.ToString(DateTime.Now) + " | PayloadID=" + sPayloadID + " | Error Status= " + ErrorMsg + " Response = " + sPayload + " | Vendor Name =  " + _VendorName + "| Payor Code=" + _PayorCode + "|" + "waitstate=" + Convert.ToString(m) + "|" + _wsUrl);

                }

                if (r < -2)
                {
                    if (PayloadIDTX != PayloadIDRX)
                    {
                        MLOG_ID = log.MLOG(_VendorName, ErrorMsg, _EBRID, stime, rtime, B_RES, B_REQ, PayloadIDTX, PayloadIDRX, sPayload, _REQ);
                        //    log.MLOG(_EBRID, "", "", PayloadIDTX, PayloadIDRX, sPayload);
                        r = -3;
                        log.ExceptionDetails(_VendorName + "  Payload mismatch req and res are in MLOG with MLOG_ID " + Convert.ToString(MLOG_ID), Convert.ToString(DateTime.Now) + " | PayloadID=" + sPayloadID + " | Error Status= " + ErrorMsg + " Response = " + sPayload + " | Vendor Name =  " + _VendorName + "| Payor Code=" + _PayorCode + "|" + "waitstate=" + Convert.ToString(m) + "|" + _wsUrl);

                    }
                }


                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails(_VendorName + "  Response Received  at " + Convert.ToString(DateTime.Now) + " | PayloadID=" + sPayloadID + " | Error Status= " + ErrorMsg + " Response = " + sPayload + " | Vendor Name =  " + _VendorName + "| Payor Code=" + _PayorCode + "|" + "waitstate=" + Convert.ToString(m) + "|" + _wsUrl, _RES);
                }



            }
            catch (Exception ex)
            {
                r = -1;
                Endtime = DateTime.Now;
                _elapsedTicks = Endtime.Ticks - StartTime.Ticks;
                log.MLOG(_VendorName, ex.Message, _EBRID, stime, rtime, B_RES, B_REQ, PayloadIDTX, PayloadIDRX, sPayload, _REQ);
                log.ExceptionDetails(_VendorName + "Traped Execption REQ and RES are in MLOG with MLOG_ID " + Convert.ToString(MLOG_ID) + " GetResponse Failed Total Elasped ticks " + Convert.ToString(_elapsedTicks) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl + "|" + _REQ, ex);
            }


            return r;

        }

    }



}
