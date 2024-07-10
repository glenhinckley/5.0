using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DCSGlobal.BusinessRules.Logging;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using System.ServiceModel;
using System.ServiceModel.Channels;
using DCSGlobal.EDI.Comunications.AlbamaMedicaidproxy;
using System.Xml;
using System.Net;

namespace DCSGlobal.EDI.Comunications
{
    class MissoulaMedicaid : IDisposable
    {
        private logExecption log = new logExecption();
        private StringStuff ss = new StringStuff();

        private int _TaskID = 0;
        private string _EBRID = string.Empty;
        private int _VendorRetrys = 1;
        private int _WaitTime = 0;
        private string B_REQ = string.Empty;
        private string ErrorMsg = "";
        private string _ConnectionString = string.Empty;
        private string _VendorName = "MISSOULA";
        private string _REQ = string.Empty;
        private string _RES = string.Empty;
        private string _PayorCode = string.Empty;
        private string _wsUrl = string.Empty;
        private string _UserName = string.Empty;
        private string _Passwd = string.Empty;
        private long _ServiceTimeOut = 30;
        private long _ReceiveTimeout = 30;

        private string _PayloadID = string.Empty;
        private string _TimeStamp = string.Empty;
        private string _ProcessingMode = string.Empty;
        private string _PayloadType = string.Empty;
        private string _SenderID = string.Empty;
        private string _ReceiverID = string.Empty;
        private string _CORERuleVersion = string.Empty;
        private string _Payload = string.Empty;

        private int _VMetricsLogging = 0;

        private string _ProtocolType = string.Empty;

        long _elapsedTicks = 0;




        bool _disposed;

        ~MissoulaMedicaid()
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
        public int VendorRetrys
        {

            set
            {

                _VendorRetrys = value;
            }
        }

        public string BHT03
        {

            set
            {

                B_REQ = value;
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
            int i = 0;
            _RES = string.Empty;
            DateTime StartTime = DateTime.Now;
            DateTime Endtime = default(DateTime);
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


                MissoulaWrapper.COREEnvelopeRealTimeRequest realTimeTrans = new MissoulaWrapper.COREEnvelopeRealTimeRequest();
                realTimeTrans.PayloadType = _PayloadType;
                realTimeTrans.ProcessingMode = _ProcessingMode;
                realTimeTrans.PayloadID = Guid.NewGuid().ToString();
                realTimeTrans.CORERuleVersion = _CORERuleVersion;
                realTimeTrans.SenderID = _SenderID;
                realTimeTrans.ReceiverID = _ReceiverID;
                realTimeTrans.TimeStamp = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:22Z");
                //realTimeTrans.Payload = "ISA*00*          *00*          *ZZ*DCS            *ZZ*RECEIVER     ID*170301*1520*^*00501*100589486*0*P*:~GS*HS*DCS*RECEIVER     ID*20170301*1520*589486*X*005010X279A1~ST*270*589486*005010X279A1~BHT*0022*13*7794309658F84B7*20170301*1520~HL*1**20*1~NM1*PR*2*MISSOULA COUNTY*****PI*810396279~HL*2*1*21*1~NM1*1P*2*RCHP*****XX*1346642899~HL*3*2*22*0~NM1*IL*1*NOSON*LINDA****MI*546642372~DMG*D8*19470612~DTP*291*D8*20170301~EQ*30~SE*12*589486~GE*1*589486~IEA*1*100589486~"

                realTimeTrans.Payload = _REQ;
                string _serviceUri = _wsUrl;

                Uri _uri = new Uri(_serviceUri);
                EndpointAddress EndPoint = new EndpointAddress(_uri);

                Binding Binding = CreateCustomBinding();

                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("Missoula Medicaid Sent at " + Convert.ToString(StartTime) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _REQ);
                }

                MissoulaWrapper.CaqhCoreTransactionsClient soap12Client = new MissoulaWrapper.CaqhCoreTransactionsClient(Binding, EndPoint);
                soap12Client.ClientCredentials.UserName.UserName = _UserName;
                soap12Client.ClientCredentials.UserName.Password = _Passwd;

                MissoulaWrapper.COREEnvelopeRealTimeResponse realTimeResp = soap12Client.RealTimeTransaction(realTimeTrans);
                httpData = realTimeResp.Payload;
                _OrigRes = httpData;
                ErrorMsg = realTimeResp.ErrorCode;
                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("Missoula Response Recived  at " + Convert.ToString(DateTime.Now), httpData);
                }

                if (realTimeResp.ErrorCode.Equals("Success"))
                {
                    _RES = realTimeResp.Payload;
                    r = 0;
                    if (_VMetricsLogging == 1)
                    {
                        log.ExceptionDetails("Missoula Response Payload  ", _RES);
                    }

                }
                else
                {
                    errMsg = realTimeResp.ErrorCode;
                    if (_VMetricsLogging == 1)
                    {
                        log.ExceptionDetails("Missoula Response ErrorCode ", errMsg);
                    }
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

        public Binding CreateCustomBinding()
        {


            CustomBinding binding = new CustomBinding();
            binding.Name = "CustomBinding_ISOAPService";

            TimeSpan _tSendTimeout = TimeSpan.FromSeconds(_ServiceTimeOut);   //new TimeSpan(0, 0, 30);

            TimeSpan _tReceiveTimeout = TimeSpan.FromSeconds(_ReceiveTimeout); //new TimeSpan(0, 0, 30);

            binding.SendTimeout = _tSendTimeout;
            binding.ReceiveTimeout = _tReceiveTimeout;

            TextMessageEncodingBindingElement txtMsg = new TextMessageEncodingBindingElement();
            txtMsg.MessageVersion = MessageVersion.Soap12WSAddressing10;
            txtMsg.WriteEncoding = System.Text.Encoding.UTF8;


            SecurityBindingElement sbe = SecurityBindingElement.CreateUserNameOverTransportBindingElement();
            sbe.MessageSecurityVersion = MessageSecurityVersion.WSSecurity10WSTrust13WSSecureConversation13WSSecurityPolicy12BasicSecurityProfile10;

            sbe.SecurityHeaderLayout = SecurityHeaderLayout.Strict;
            sbe.IncludeTimestamp = false;
            sbe.SetKeyDerivation(true);
            sbe.KeyEntropyMode = System.ServiceModel.Security.SecurityKeyEntropyMode.ServerEntropy;
            binding.Elements.Add(sbe);
            binding.Elements.Add(txtMsg);
            binding.Elements.Add(new HttpsTransportBindingElement());
            return new CustomBinding(binding);
        }
    }
}
