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
    class EMDEON : IDisposable
    {

        private StringStuff ss = new StringStuff();
        private logExecption log = new logExecption();



        private string _ConnectionString = string.Empty;



        private string _VendorInputType = string.Empty;
        private string _VendorName = string.Empty;
        private string _PayorCode = string.Empty;

        private string _AppName = string.Empty;


       
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
        private string _LogToREQRES = string.Empty;
        private string _Token = string.Empty;
        long _elapsedTicks = 0;
        private int _VMetricsLogging = 0;
        private string _ProtocolType = string.Empty;


        ~EMDEON()
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
                    log.ExceptionDetails("emdeon.StripREQ Failed", ex);
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
                    log.ExceptionDetails("emdeon.StripRES Failed", ex);
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
            DateTime StartTime = DateTime.Now;
            DateTime Endtime;
            _RES = string.Empty;

            try
            {
                string httpData = string.Empty;
                EmdeonPxy.AWS client = new EmdeonPxy.AWS();
                client.Url = _wsUrl;
                client.Timeout = _ServiceTimeOut;
                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("EMDEON Sent at " + Convert.ToString(StartTime) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _REQ);
                }
                httpData = ASCIIEncoding.ASCII.GetString(Convert.FromBase64String(Convert.ToBase64String(client.RunTransaction(_UserName, _Passwd, Encoding.UTF8.GetBytes(_REQ)))));
                _RES = httpData;


                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("EMDEON REcived  at " + Convert.ToString(DateTime.Now) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _RES);
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


        //public int GetResponse_old()
        //{
        //    int r = -1;
        //    //DateTime StartTime = DateTime.Now;
        //    //DateTime Endtime;


        //    //try
        //    //{
        //    //    string httpData = string.Empty;
        //    //    com.emdeon.ra.AWS client = new com.emdeon.ra.AWS();
        //    //    client.Url = wsUrl;
        //    //    client.Timeout = Convert.ToInt32(ServiceTimeOut); 
        //    //    httpData = ASCIIEncoding.ASCII.GetString(Convert.FromBase64String(Convert.ToBase64String(client.RunTransaction("PMNADCSGL", "TMURwvb3", Encoding.UTF8.GetBytes(_REQ)))));
        //    //    _RES = httpData;
        //    //    r = 0;
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    log.ExceptionDetails("emdeon.GetResponse Failed", ex);
        //    //    r = -1;
        //    //}

        //    //Endtime = DateTime.Now;

        //    //_elapsedTicks = Endtime.Ticks - StartTime.Ticks;

        //    return r;



        //}



    }
}
