using System;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;
using DCSGlobal.EDI.Comunications;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using DCSGlobal.BusinessRules.Logging;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Globalization;


using System.Xml;


namespace DCSGlobal.EDI.Comunications
{
    class EmdeonOnly : IDisposable
    {

        private long GlobalBatchID = 0;
        private StringStuff ss = new StringStuff();
        private logExecption log = new logExecption();

        private string _subscriberfirstname = string.Empty;
        private string _subscriberlastname = string.Empty;
        private string _subscriberdob = string.Empty;
        private string _dateofservice = string.Empty;

        private string _ConnectionString = string.Empty;
        private string _LookupUrl = "https://assistant.emdeon.com/Services/Assistant";

        private string _EmdReq = string.Empty;
        private string _EmdRes = string.Empty;

        private string _VendorInputType = string.Empty;
        private string _VendorName = string.Empty;
        private string _PayorCode = string.Empty;

        private string _AppName = string.Empty;

        private int _EmdeonSmartDelete = 0;

        private string SYNC_TIMEOUT = string.Empty;
        private string SUBMISSION_TIMEOUT = string.Empty;
        private string _myResponses = string.Empty;
        private string _myResponses1 = string.Empty;

        private string _REQ = string.Empty;
        private string _RES = string.Empty;

        private string _wsUrl = string.Empty;
        private string _UserName = string.Empty;
        private string _Passwd = string.Empty;
        private string _apiKey = "jhZiUkWOgY";
        private string _ContentType = string.Empty;
        private string _Method = string.Empty;
        private int _ServiceTimeOut = 0;
        private string _LogToREQRES = string.Empty;
        private string _Token = string.Empty;
        long _elapsedTicks = 0;
        private int _VMetricsLogging = 0;
        private string _ProtocolType = string.Empty;
        private string _LoginUserID = string.Empty;

        private string _PatientHospitalCode = string.Empty;

        private string _Hosp_Code = string.Empty;

        private string _InsType = string.Empty;

        private string _PatAcctNum = string.Empty;

        private string sVendorName = string.Empty;

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
        private bool ResEmdeonAssistant = false;
        private long _BatchID = 0;
        private long _ReceiveTimeout = 30;

        private int norespid = -1;



        ~EmdeonOnly()
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
        public long BatchID
        {

            set
            {

                _BatchID = value;
            }
        }

        public string LoginUserID
        {

            set
            {

                _LoginUserID = value;
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

        public string subscriberfirstname
        {

            set
            {

                _subscriberfirstname = value;
            }
        }

        public string subscriberlastname
        {

            set
            {

                _subscriberlastname = value;
            }
        }

        public string subscriberdob
        {

            set
            {

                _subscriberdob = value;
            }
        }

        public string dateofservice
        {

            set
            {

                _dateofservice = value;
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

        public string PatientHospitalCode
        {

            set
            {

                _PatientHospitalCode = value;
            }
        }

        public string Hosp_Code
        {

            set
            {

                _Hosp_Code = value;
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



        public string LookupUrl
        {
            set
            {
                _LookupUrl = value;
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



        public string PayorCode
        {
            set
            {
                _PayorCode = value;
            }
        }


        public string InsType
        {
            set
            {
                _InsType = value;
            }
        }

        public string PatAcctNum
        {
            set
            {
                _PatAcctNum = value;
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


        public string apiKey
        {

            set
            {

                _apiKey = value;
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



        public int ServiceTimeOut
        {

            set
            {

                _ServiceTimeOut = value;
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
                    log.ExceptionDetails("EmdeonOnly.StripREQ Failed", ex);
                }
            }
        }

        public int EmdeonSmartDelete
        {
            get
            {
                return _EmdeonSmartDelete;
            }
            set
            {
                try
                {
                    _EmdeonSmartDelete = value;

                }
                catch (Exception ex)
                {
                    log.ExceptionDetails("EmdeonOnly.EmdeonSmartDelete Failed", ex);
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
                    log.ExceptionDetails("EmdeonOnly.StripRES Failed", ex);
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
                    log.ExceptionDetails(_VendorName, _EBRID + " EmdeonOnly - retry " + Convert.ToString(VendorTrys) + " Error Message " + ErrorMsg);

                    _WaitTime = _WaitTime + 250;
                }
                i++;
            }

            if (_VendorRetrys == VendorTrys)
            {

                log.ExceptionDetails(_VendorName + " - EmdeonOnly  Exceded Ventor Retrys giving up with  VendorTrys  at " + Convert.ToString(VendorTrys), _EBRID);
                r = -5;
            }

            return r;

        }

        public int GoToVendor()
        {
            int x = -1;
            int r = -1;
            DateTime StartTime = DateTime.Now;
            DateTime Endtime;
            _RES = string.Empty;

            stime = string.Empty;
            rtime = string.Empty;

            _EmdReq = "";
            _EmdRes = "";



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


                string httpData = string.Empty;

                sPayloadID = Guid.NewGuid().ToString();  //_PayloadID;


                //Random rnd = new Random();
                int m = 0; //rnd.Next(0, 1000);
                //  m = m * 5;
                Thread.Sleep(_WaitTime);
                //var xmlRequest = xd.OuterXml;
                //ServicePointManager.Expect100Continue = true;
                //ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => true;
                // Skip validation of SSL/TLS certificate
                //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                //ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;

                sTimeStamp = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:fffZ");


                EmdeonPxy.AWS client = new EmdeonPxy.AWS();
                client.Url = _wsUrl;
                client.Timeout = _ServiceTimeOut;
                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("EmdeonOnly Sent at " + Convert.ToString(sTimeStamp) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _REQ);
                }

                stime = sTimeStamp;

                _EmdReq = _REQ;

                httpData = ASCIIEncoding.ASCII.GetString(Convert.FromBase64String(Convert.ToBase64String(client.RunTransaction(_UserName, _Passwd, Encoding.UTF8.GetBytes(_REQ)))));

                _OrigRes = httpData;
                _RES = httpData;


                sPayload = _RES;

                _EmdRes = _RES;


                rtime = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:fffZ");

                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("EmdeonOnly REcived  at " + Convert.ToString(rtime) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _RES);
                }

                using (ValidateEDI vedi = new ValidateEDI())
                {
                    vedi.ConnectionString = _ConnectionString;
                    vedi.byString(sPayload);
                    B_RES = vedi.ReferenceIdentification;
                }

                r = 0;
                if (!_RES.Contains("ISA"))
                {
                    MLOG_ID = log.MLOG(_VendorName, ErrorMsg, _EBRID, stime, rtime, B_RES, B_REQ, PayloadIDTX, PayloadIDRX, _RES, _REQ);
                    r = -2;
                }


                if (B_RES != B_REQ)
                {
                    MLOG_ID = log.MLOG(_VendorName, ErrorMsg, _EBRID, stime, rtime, B_RES, B_REQ, PayloadIDTX, PayloadIDRX, sPayload, _REQ);
                    r = -3;
                    log.ExceptionDetails(_VendorName + " - EmdeonOnly BHT03 mismatch req and res are in MLOG with MLOG_ID " + Convert.ToString(MLOG_ID), Convert.ToString(DateTime.Now) + " | PayloadID=" + sPayloadID + " | Error Status= " + ErrorMsg + " Response = " + sPayload + " | Vendor Name =  " + _VendorName + "| Payor Code=" + _PayorCode + "|" + "waitstate=" + Convert.ToString(m) + "|" + _wsUrl);

                }

            }
            catch (Exception ex)
            {
                r = -1;
                Endtime = DateTime.Now;
                _elapsedTicks = Endtime.Ticks - StartTime.Ticks;
                log.ExceptionDetails("EmdeonOnly-GetResponse Failed Total Elasped ticks" + Convert.ToString(_elapsedTicks) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl + "|" + _REQ, ex);

            }

            return r;

        }

    }
}
