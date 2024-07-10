using System;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;
using DCSGlobal.EDI.Comunications;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using DCSGlobal.BusinessRules.Logging;


namespace DCSGlobal.EDI.Comunications
{
    class DoradoSystemsEx : IDisposable
    {


        private StringStuff ss = new StringStuff();
        private logExecption log = new logExecption();



        private string _ConnectionString = string.Empty;

        private string _276Url = string.Empty;
        private string _wsUrl = string.Empty;
        private string _ClientId = string.Empty;
        private string _PreAuthenticate = string.Empty;

        private string _UserName = string.Empty;
        private string _passwd = string.Empty;
        private string _apiKey = string.Empty;
        private string _ContentType = string.Empty;
        private string _Method = string.Empty;
        private int _ServiceTimeOut = 120000;
        private string _Token = string.Empty;
        private string _ProtocolType = string.Empty;

        private int _VMetricsLogging = 0;

        private string _VendorInputType = string.Empty;
        private string _VendorName = string.Empty;
        private string _PayorCode = string.Empty;

        private string _AppName = string.Empty;


        private string SYNC_TIMEOUT = string.Empty;
        private string SUBMISSION_TIMEOUT = string.Empty;
        private string _myResponses = string.Empty;
        private string _myResponses1 = string.Empty;

        private string _REQ = string.Empty;
        private string _RES = string.Empty;



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




        ~DoradoSystemsEx()
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

        public string Url276
        {

            set
            {

                _276Url = value;
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
                if (i != _VendorRetrys+2)
                {
                log.ExceptionDetails(_VendorName + "  Exceded Ventor Retrys giving up with  VendorTrys  at " + Convert.ToString(VendorTrys), _EBRID);
                r = -5;
                }
            }

            return r;

        }

        public int GoToVendor()
        {
            string httpData = null;
            HttpWebRequest req = null;
            HttpWebResponse resp = null;
            string strBlankFields = string.Empty;
            string strEDIData = string.Empty;

            int r = -1;
            DateTime StartTime = DateTime.Now;
            DateTime Endtime;


            string url = string.Empty;
            _RES = string.Empty;


            stime = string.Empty;
            rtime = string.Empty;
            string REQ_TransactionSetIdentifierCode = string.Empty;

            try
            {

                sPayloadID = Guid.NewGuid().ToString();  //_PayloadID;

                string wsurl = string.Empty;

                using (ValidateEDI vedi = new ValidateEDI())
                {
                    vedi.ConnectionString = _ConnectionString;
                    vedi.byString(_REQ);
                    REQ_TransactionSetIdentifierCode = vedi.TransactionSetIdentifierCode;
                }


                if(REQ_TransactionSetIdentifierCode =="276")
                {
                    strBlankFields = "&first_name=&last_name=&dob=&gender=&member_id=&ssn=&carrier_code=&eff_date_start=&eff_date_end=&npi=&isa13=&service_types=";
                    strEDIData = "&data=" + _REQ;
                    url = _276Url + strBlankFields + strEDIData;

                
                }
                else
                {
                    strBlankFields = "&first_name=&last_name=&dob=&gender=&member_id=&ssn=&carrier_code=&eff_date_start=&eff_date_end=&npi=&isa13=&service_types=";
                    strEDIData = "&data=" + _REQ;
                    url = _wsUrl + strBlankFields + strEDIData;
                    //fffffffffffffffffffffffffffffffff
                }

           

             





                //

                //Random rnd = new Random();
                int m = 0; //rnd.Next(0, 1000);
                //  m = m * 5;
                Thread.Sleep(_WaitTime);

                sTimeStamp = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:fffZ");


                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("DARADO Sent at " + Convert.ToString(sTimeStamp) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + url, _REQ);
                }



                stime = sTimeStamp;


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




               // ServicePointManager.SecurityProtocol =  (SecurityProtocolType)3072 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3 ;
                req = (HttpWebRequest)WebRequest.Create(url);
                req.Timeout = _ServiceTimeOut;
                req.Method = _Method;
                req.ContentType = _ContentType;
                Stream up = req.GetRequestStream();

                resp = (HttpWebResponse)req.GetResponse();
                Stream s = resp.GetResponseStream();
                StreamReader sr = new StreamReader(s);
                httpData = sr.ReadToEnd();
                sr.Close();
                s.Close();
                resp.Close();

                _OrigRes = httpData;
                _RES = httpData;

                sPayload = _RES;

                rtime = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:fffZ");

                //fffffffffffffffffffffffffffffffff
                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("DARADO REcived  at " + Convert.ToString(rtime) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + url, _RES);
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
                    log.ExceptionDetails(_VendorName + " BHT03 mismatch req and res are in MLOG with MLOG_ID " + Convert.ToString(MLOG_ID), Convert.ToString(DateTime.Now) + " | PayloadID=" + sPayloadID + " | Error Status= " + ErrorMsg + " Response = " + sPayload + " | Vendor Name =  " + _VendorName + "| Payor Code=" + _PayorCode + "|" + "waitstate=" + Convert.ToString(m) + "|" + _wsUrl);

                }

                if (r < -4)   //we don't have Request GUID & Response GUID.
                {
                    //if (PayloadIDTX != PayloadIDRX)
                    //{
                    //    MLOG_ID = log.MLOG(_VendorName, ErrorMsg, _EBRID, stime, rtime, B_RES, B_REQ, PayloadIDTX, PayloadIDRX, sPayload, _REQ);
                    //    r = -4;
                    //    log.ExceptionDetails(_VendorName + "  Payload mismatch req and res are in MLOG with MLOG_ID " + Convert.ToString(MLOG_ID), Convert.ToString(DateTime.Now) + " | PayloadID=" + sPayloadID + " | Error Status= " + ErrorMsg + " Response = " + sPayload + " | Vendor Name =  " + _VendorName + "| Payor Code=" + _PayorCode + "|" + "waitstate=" + Convert.ToString(m) + "|" + _wsUrl);

                    //}
                }
  
            }
            catch (System.Exception ex)
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
