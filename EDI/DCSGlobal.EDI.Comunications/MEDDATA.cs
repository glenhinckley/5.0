using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using DCSGlobal.BusinessRules.Logging;

namespace DCSGlobal.EDI.Comunications
{
     class MEDDATA : IDisposable
    {


        private StringStuff ss = new StringStuff();
        private logExecption log = new logExecption();



        private string _ConnectionString = string.Empty;
        long _elapsedTicks = 0;



        private string _RequestFormat = string.Empty;
        private string _ResponseFormat = string.Empty;
        private string _VendorInputType = string.Empty;
        private string _VendorName = string.Empty;
        private string _PayorCode = string.Empty;
        private string _ProtocolType = string.Empty;
        private string _AppName = string.Empty;

        private int _VMetricsLogging = 0;
        private string _SUBMISSION_TIMEOUT = "0.00:00:30";
        private string _SYNC_TIMEOUT =  "0.00:00:30";
        private string SYNC_TIMEOUT = string.Empty;
        private string SUBMISSION_TIMEOUT = string.Empty;
        private string _myResponses = string.Empty;
        private string _myResponses1 = string.Empty;

        private  string _REQ = string.Empty;
        private  string _RES = string.Empty;

        private string _wsUrl = string.Empty;
        private string _UserName = string.Empty;
        private string _Passwd = string.Empty;
        private string _apiKey = string.Empty;
        private string _ContentType = string.Empty;
        private string _Method = string.Empty;
        private int _ServiceTimeOut = 0;
        private int _TimeToWait = 0;
        private string _LogToREQRES { get; set; }
        private string _Token { get; set; }




        ~MEDDATA()
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


        public int ServiceTimeOut
        {

            set
            {

                _ServiceTimeOut = value;
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

        public string RequestFormat
        {

            set
            {

                _RequestFormat = value;
            }
        }

        public string ResponseFormat
        {

            set
            {

                _ResponseFormat = value;
            }
        }

        public int GetResponse()
        {

            int r = -1;
            _RES = string.Empty;
            DateTime StartTime = DateTime.Now;
            DateTime Endtime;

            try
            {
                MedDataPxy.MedDataExternalSubmissionPortal myService = new MedDataPxy.MedDataExternalSubmissionPortal();

                MedDataPxy.SecurityHeader securityHeader = new MedDataPxy.SecurityHeader();
                myService.Url = _wsUrl;
                //securityHeader.UserName = "2019275";
                //securityHeader.Password = "fT~H31]m";

                securityHeader.UserName = _UserName ;
                securityHeader.Password = _Passwd;
            
                myService.SecurityHeaderValue = securityHeader;
                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("MEDDATA sent at " + Convert.ToString(StartTime) + " Service time out  | " + _SUBMISSION_TIMEOUT + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _REQ);
                }
                _RES = myService.SubmitSync(_REQ, _RequestFormat, _ResponseFormat, _SYNC_TIMEOUT, _SUBMISSION_TIMEOUT);
                //_RES = myService.SubmitSync(_REQ, "FlatXml", "Edi", _SYNC_TIMEOUT, _SUBMISSION_TIMEOUT);
                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("MEDDATA REcived  at " + Convert.ToString(DateTime.Now) + " Service time out  | " + _SUBMISSION_TIMEOUT + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _RES);
                }
                r = 0;
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


        public int GetResponse_old()
        {

            int r = -1;

            //DateTime StartTime = DateTime.Now;
            //DateTime Endtime;

            //try
            //{
            //    com.meddatahealth.services.MedDataExternalSubmissionPortal myService = new com.meddatahealth.services.MedDataExternalSubmissionPortal();
            //    com.meddatahealth.services.SecurityHeader securityHeader = new com.meddatahealth.services.SecurityHeader();
            //    myService.Url = wsUrl;
            //    //securityHeader.UserName = "2019275";
            //    //securityHeader.Password = "fT~H31]m";

            //    securityHeader.UserName = username;
            //    securityHeader.Password = password;

            //    myService.SecurityHeaderValue = securityHeader;

            //    _RES = myService.SubmitSync(_REQ, "FlatXml", "Edi", _SYNC_TIMEOUT, _SUBMISSION_TIMEOUT);

            //    r = 0;
            //}
            //catch (Exception ex)
            //{
            //    log.ExceptionDetails("MEDDATA.GetResponse Failed", ex);
            //    r = -1;
            //}

            //Endtime = DateTime.Now;

            //_elapsedTicks = Endtime.Ticks - StartTime.Ticks;

            return r;

        }





    }
}
