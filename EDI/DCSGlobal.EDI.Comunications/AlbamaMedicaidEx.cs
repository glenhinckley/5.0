﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DCSGlobal.BusinessRules.Logging;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using System.ServiceModel;
using System.ServiceModel.Channels;
using DCSGlobal.EDI.Comunications.AlbamaMedicaidproxy;
using System.Threading;
using System.Net;
namespace DCSGlobal.EDI.Comunications
{
    class AlbamaMedicaidEx : IDisposable
    {
        private logExecption log = new logExecption();
        private StringStuff ss = new StringStuff();

        private string _ConnectionString = string.Empty;
        private string _VendorName = "ARMCD";
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

      
 
        private string sPayloadType = string.Empty;
    
      

        ~AlbamaMedicaidEx()
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
            int i = 0;
            _RES = string.Empty;
            DateTime StartTime = DateTime.Now;
            DateTime Endtime = default(DateTime);

            PayloadIDTX = string.Empty;
            PayloadIDRX = string.Empty;
            sPayloadType = string.Empty;

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
                sPayloadType = _PayloadType;

                AlbamaMedicaidproxy.COREEnvelopeRealTimeRequest realTimeTrans = new AlbamaMedicaidproxy.COREEnvelopeRealTimeRequest();
                realTimeTrans.PayloadType = _PayloadType;
                realTimeTrans.ProcessingMode = _ProcessingMode;
                realTimeTrans.PayloadID = sPayloadID;
                realTimeTrans.CORERuleVersion = _CORERuleVersion;
                realTimeTrans.SenderID = _SenderID;
                realTimeTrans.ReceiverID = _ReceiverID;
                realTimeTrans.TimeStamp = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:22Z");
                //realTimeTrans.Payload = "ISA*00*          *00*          *ZZ*MC028720      *30*716007869      *160913*1211*^*00501*100589486*0*P*:~GS*HS*MC028720*716007869*20160913*1211*589486*X*005010X279A1~ST*270*589486*005010X279A1~BHT*0022*13*F6C10D60A4EC494*20160913*1211~HL*1**20*1~NM1*PR*2*AR MEDICAID*****PI*716007869~HL*2*1*21*1~NM1*1P*2*Cornerstone Hospitals*****XX*1346630589~HL*3*2*22*0~NM1*IL*1*******522296589~DMG*D8**~DTP*291*D8*20160913~EQ*30~SE*12*589486~GE*1*589486~IEA*1*100589486~"
                //realTimeTrans.Payload = "ISA*00*          *00*          *ZZ*MC028720       *30*716007869      *160913*1211*^*00501*100589486*0*P*:~GS*HS*MC028720*716007869*20160913*1211*589486*X*005010X279A1~ST*270*589486*005010X279A1~BHT*0022*13*F6C10D60A4EC494*20160913*1211~HL*1**20*1~NM1*PR*2*AR MEDICAID*****PI*716007869~HL*2*1*21*1~NM1*1P*2*Cornerstone Hospitals*****XX*1346630589~HL*3*2*22*0~NM1*IL*1*******522296589~DMG*D8*20030808*~DTP*291*D8*20160913~EQ*30~SE*12*589486~GE*1*589486~IEA*1*100589486~"
                //realTimeTrans.Payload = "ISA*00*          *00*          *ZZ*MC028720       *30*716007869      *160913*1211*^*00501*200589486*0*P*:~GS*HS*MC028720*716007869*20160913*1211*589486*X*005010X279A1~ST*270*589486*005010X279A1~BHT*0022*13*F6C10D60A4EC494*20160913*1211~HL*1**20*1~NM1*PR*2*AR MEDICAID*****PI*716007869~HL*2*1*21*1~NM1*1P*2*Cornerstone Hospitals*****XX*1346630589~HL*3*2*22*0~NM1*IL*1*CORAL*BILLY****MI*522296589~DMG*D8*20030808*M~DTP*291*D8*20160913~EQ*30~SE*12*589486~GE*1*589486~IEA*1*200589486~"

                realTimeTrans.Payload = _REQ;
                string _serviceUri = _wsUrl;

                Uri _uri = new Uri(_serviceUri);
                EndpointAddress EndPoint = new EndpointAddress(_uri);

                Binding Binding = CreateCustomBinding();

                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("Albama Medicaid Sent at " + Convert.ToString(StartTime) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _REQ);
                }

                AlbamaMedicaidproxy.CoreRealTimeClient soap12Client = new AlbamaMedicaidproxy.CoreRealTimeClient(Binding, EndPoint);
                soap12Client.ClientCredentials.UserName.UserName = _UserName;
                soap12Client.ClientCredentials.UserName.Password = _Passwd;

                //Random rnd = new Random();
                int m = 0; //rnd.Next(0, 1000);
                //  m = m * 5;
                Thread.Sleep(_WaitTime);

                sTimeStamp = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:fffZ");

                stime = sTimeStamp;
                PayloadIDTX = sPayloadID;


                // client.BatchSubmitTransaction()
                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails(_VendorName + " Request sent at = " + Convert.ToString(sTimeStamp) + " | PayloadID=" + sPayloadID + " | Vendor Name= " + _VendorName + " | Payor Code= " + _PayorCode + " | URL=" + _wsUrl + " | sPayloadType =" + sPayloadType + " | ProcessingMode=" + _ProcessingMode + " | TimeStamp=" + sTimeStamp + " | SenderID=" + _SenderID + " | ReceiverID=" + _ReceiverID + " | ServiceTimeOut=" + _ServiceTimeOut + " | CORERuleVersion=" + _CORERuleVersion + " | WaitTime " + Convert.ToString(_WaitTime), _REQ);
                }



                AlbamaMedicaidproxy.COREEnvelopeRealTimeResponse realTimeResp = soap12Client.RealTimeTransaction(realTimeTrans);
                sPayload = realTimeResp.Payload;
                _OrigRes = realTimeResp.Payload;
                PayloadIDRX = realTimeResp.PayloadID;

                rtime = realTimeResp.TimeStamp;

                ErrorMsg = realTimeResp.ErrorCode;

                //if (realTimeResp.ErrorCode.Equals("Success"))
                //{
                //    _RES = realTimeResp.Payload;

                //}
                //else
                //{
                //    _RES = realTimeResp.ErrorCode;
                //}
                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("Albama Medicaid REcived  at " + Convert.ToString(rtime) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _RES);
                }

             
                if (ErrorMsg == "Success")
                {
                    _RES = sPayload;
                    r = 0;
                }
                else
                {
                    MLOG_ID = log.MLOG(_VendorName, ErrorMsg, _EBRID, stime, rtime, B_RES, B_REQ, PayloadIDTX, PayloadIDRX, sPayload, _REQ);
                    // log.ExceptionDetails(_VendorName + " ErrorMSG = '" + ErrorMsg + "'  req and res are in MLOG with MLOG_ID " + Convert.ToString(MLOG_ID), Convert.ToString(DateTime.Now) + " | PayloadID=" + sPayloadID + " | Error Status= " + ErrorMsg + " Response = " + sPayload + " | Vendor Name =  " + _VendorName + "| Payor Code=" + _PayorCode + "|" + "waitstate=" + Convert.ToString(m) + "|" + _wsUrl);

                    r = -2;
                }

                using (ValidateEDI vedi = new ValidateEDI())
                {
                    vedi.ConnectionString = _ConnectionString;
                    vedi.byString(sPayload);
                    B_RES = vedi.ReferenceIdentification;
                }

                if (!_RES.Contains("ISA"))
                {
                    MLOG_ID = log.MLOG(_VendorName, ErrorMsg, _EBRID, stime, rtime, B_RES, B_REQ, PayloadIDTX, PayloadIDRX, _RES, _REQ);
                    r = -3;
                }
                if (B_RES != B_REQ)
                {
                    MLOG_ID = log.MLOG(_VendorName, ErrorMsg, _EBRID, stime, rtime, B_RES, B_REQ, PayloadIDTX, PayloadIDRX, sPayload, _REQ);
                    r = -4;
                    log.ExceptionDetails(_VendorName + " BHT03 mismatch req and res are in MLOG with MLOG_ID " + Convert.ToString(MLOG_ID), Convert.ToString(DateTime.Now) + " | PayloadID=" + sPayloadID + " | Error Status= " + ErrorMsg + " Response = " + sPayload + " | Vendor Name =  " + _VendorName + "| Payor Code=" + _PayorCode + "|" + "waitstate=" + Convert.ToString(m) + "|" + _wsUrl);

                }

                if (r < -3)
                {
                    if (PayloadIDTX != PayloadIDRX)
                    {
                        MLOG_ID = log.MLOG(_VendorName, ErrorMsg, _EBRID, stime, rtime, B_RES, B_REQ, PayloadIDTX, PayloadIDRX, sPayload, _REQ);
                        //    log.MLOG(_EBRID, "", "", PayloadIDTX, PayloadIDRX, sPayload);
                        r = -5;
                        log.ExceptionDetails(_VendorName + "  Payload mismatch req and res are in MLOG with MLOG_ID " + Convert.ToString(MLOG_ID), Convert.ToString(DateTime.Now) + " | PayloadID=" + sPayloadID + " | Error Status= " + ErrorMsg + " Response = " + sPayload + " | Vendor Name =  " + _VendorName + "| Payor Code=" + _PayorCode + "|" + "waitstate=" + Convert.ToString(m) + "|" + _wsUrl);

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
    }
}