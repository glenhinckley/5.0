using System;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;
using DCSGlobal.EDI.Comunications;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using DCSGlobal.BusinessRules.Logging;
using System.Net;
using System.Web;


namespace DCSGlobal.EDI.Comunications
{
    class VISIONSHARE : IDisposable
    {

        private  logExecption log = new logExecption();
        private  StringStuff ss = new StringStuff();



        private int _VS_PROXY_PORT = 0;
        private string _VS_PROXY_SERVER = string.Empty;
        private string _VS_HOST_ADDRESS = string.Empty;
        private int _VS_SES_PORT = 0;
        private string _UserName = string.Empty;
        private string _Passwd = string.Empty;
        private string _Connection = string.Empty;
        private int _VS_SES_PORT1 = 0;
        private string _UserName1 = string.Empty;
        private string _Passwd1 = string.Empty;
        private string _Connection1 = string.Empty;
        private int _ServiceTimeOut = 0;
        private int _TimeToWait = 0;
        private string _LogToREQRES = string.Empty;
        private string _Token = string.Empty;


        private string _ConnectionString = string.Empty;


        private bool HTTP_RETURN_FLAG = false;


        private string _VendorInputType = string.Empty;
        private string _VendorName = string.Empty;
        private string _PayorCode = string.Empty;

        private string _AppName = string.Empty;

        private string _ProtocolType = string.Empty;
        private string _SUBMISSION_TIMEOUT = "0.00:00:30";
        private string _SYNC_TIMEOUT = "0.00:00:30";
        private string SYNC_TIMEOUT = string.Empty;
        private string SUBMISSION_TIMEOUT = string.Empty;
        private string _myResponses = string.Empty;
        private string _myResponses1 = string.Empty;

        private string _REQ = string.Empty;
        private string _RES = string.Empty;
        long _elapsedTicks = 0;
        private int _VMetricsLogging = 0;

        ~VISIONSHARE()
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
        public string ConnectionString
        {

            set
            {

                _ConnectionString = value;
                log.ConnectionString = value;

            }
        }

        public int VS_PROXY_PORT
        {

            set
            {

                _VS_PROXY_PORT = value;
            }
        }

        public string VS_PROXY_SERVER
        {

            set
            {

                _VS_PROXY_SERVER = value;
            }
        }


        public string VS_HOST_ADDRESS
        {

            set
            {

                _VS_HOST_ADDRESS = value;
            }
        }




        public int VS_SES_PORT
        {

            set
            {

                _VS_SES_PORT = value;
            }
        }

        public int VS_SES_PORT1
        {

            set
            {

                _VS_SES_PORT1 = value;
            }
        }





        public string VendorInputType
        {

            set
            {

                _VendorInputType = value;
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


        public string UserName1
        {

            set
            {

                _UserName1 = value;
            }
        }

        public string Passwd1
        {

            set
            {

                _Passwd1 = value;
            }
        }


        public string Token
        {

            set
            {

                _Token = value;
            }
        }


        public int ServiceTimeOut
        {

            set
            {

                _ServiceTimeOut = value;
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


        public string Connection
        {

            set
            {

                _Connection = value;
            }
        }

        public string Connection1
        {

            set
            {

                _Connection1 = value;
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


                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("VISIONSHARE Sent at " + Convert.ToString(StartTime) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _VS_HOST_ADDRESS, _REQ);
                }

                switch (_PayorCode)
                {
                    case "00007":
                    case "CMS":
                    case "CMSRR":
                            _RES = HttpProcessWrapper(_REQ, _VS_HOST_ADDRESS, _VS_SES_PORT1, _VS_PROXY_SERVER,_VS_PROXY_PORT, _UserName1, _Passwd1, _Connection1);
                            break;
                    case "00002":
                    case "00003":
                    case "00032":
                        _RES = HttpProcessWrapper(_REQ, _VS_HOST_ADDRESS, _VS_SES_PORT, _VS_PROXY_SERVER, _VS_PROXY_PORT, _UserName, _Passwd, _Connection);
                        break;
                    default:
                        r = -1;
                        _RES = string.Empty;
                        break;
                }
                if (HTTP_RETURN_FLAG)
                { 
                    r = 0;
                    if (_VMetricsLogging == 1)
                    {
                        log.ExceptionDetails("VISIONSHARE Medicaid REcived  at " + Convert.ToString(DateTime.Now) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _VS_HOST_ADDRESS, _RES);
                    }
                }
                else
                { 
                    r = -1;
                    log.ExceptionDetails("VISIONSHARE.HttpProcessWrapper Failed ", "HttpProcessWrapper Failed");
                }
            }
            catch (Exception ex)
            {
                r = -1;
                Endtime = DateTime.Now;
                _elapsedTicks = Endtime.Ticks - StartTime.Ticks;
                log.ExceptionDetails("GetResponse Failed Total Elasped ticks" + Convert.ToString(_elapsedTicks) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _VS_HOST_ADDRESS + "|" + _REQ, ex);
               
            }



            return r;
        }

        public string HttpProcessWrapper(string strReq, string __VS_HOST_ADDRESS, int __VS_SES_PORT, string __VS_PROXY_SERVER, int __VS_PROXY_PORT, string __uid, string __pwd, string __conn)
        {
            HTTP_RETURN_FLAG = false;
            HTTPProcess oHttp = default(HTTPProcess);
            string RetVal = "";
            try
            {
                oHttp = new HTTPProcess(__VS_HOST_ADDRESS, __VS_SES_PORT, __VS_PROXY_SERVER, __VS_PROXY_PORT, __uid, __pwd, __conn);
                ResponseData data = oHttp.Process(_REQ);
                RetVal = data.Content;
                HTTP_RETURN_FLAG = true;

            }
            catch (Exception ex)
            {
                //log.ExceptionDetails("ProcessVISIONSHARE", ex);
                RetVal = ex.Message;
                HTTP_RETURN_FLAG = false;
                log.ExceptionDetails("VISIONSHARE.HttpProcessWrapper.GetResponse Failed"+ _VendorName + "-" + _PayorCode, ex);
            }
            return RetVal;
        }


    }
}
