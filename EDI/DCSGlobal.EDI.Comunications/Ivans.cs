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
     class IVANS : IDisposable
    {


        private static logExecption log = new logExecption();
        private StringStuff ss = new StringStuff();

        long _elapsedTicks = 0;
        private string _ConnectionString = string.Empty;
        private string _appName = "DCSGlobal.EDI.Comunications.";
        private string _vendorName = "IVANS";
        private string _REQ = string.Empty;
        private string _RES = string.Empty;
        private string _VendorName = string.Empty;
        private string _PayorCode = string.Empty;

        private string _wsUrl = string.Empty;
        private string _ClientId = string.Empty;
        private bool _PreAuthenticate = true;

        private string _UserName = string.Empty;
        private string _Passwd = string.Empty;
        private string _apiKey = string.Empty;
        private string _ContentType = string.Empty;
        private string _Method = string.Empty;
        private int _ServiceTimeOut = 0;
        private string _TimeToWait = string.Empty;
        private string _LogToREQRES = string.Empty;
        private string _Token = string.Empty;
        private string _wsdl = string.Empty;
        private int _VMetricsLogging = 0;
        private string _ProtocolType = string.Empty;




        ~IVANS()
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

        public string ClientId
        {

            set
            {

                _ClientId = value;
            }
        }




        public bool PreAuthenticate
        {

            set
            {

                _PreAuthenticate = value;
            }
        }


        public string wsdl
        {

            set
            {

                _wsdl = value;
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
            _RES = string.Empty;
            DateTime StartTime = DateTime.Now;
            DateTime Endtime;
            try
            {

                IvansPxy.EligibilityOne e1 = new IvansPxy.EligibilityOne();
                IvansPxy.IvansWSAuthentication a = new IvansPxy.IvansWSAuthentication();

                a.User = _UserName;  //"8DC00005";
                a.Password = _Passwd; // "DC$Gl0bal//2013";
                a.ClientId = _ClientId;   // "f9e52687-a2e0-4d9c-8591-59516c9fd026";

                e1.PreAuthenticate = _PreAuthenticate; //  true;
                e1.Url = _wsdl;
                e1.IvansWSAuthenticationValue = a;
                e1.Timeout = _ServiceTimeOut;  // 120000;

                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("IVANS Sent at " + Convert.ToString(StartTime) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _REQ);
                }
                _RES = e1.SendCommercialEligibilityFormRequest(_REQ);
               
                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("IVANS REcived  at " + Convert.ToString(DateTime.Now) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _RES);
                }
                r = 0;
            }
            catch (Exception ex)
            {
                r = -1;
                Endtime = DateTime.Now;
                _elapsedTicks = Endtime.Ticks - StartTime.Ticks;
                log.ExceptionDetails("GetResponse Failed Total Elasped ticks" + Convert.ToString(_elapsedTicks) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl + "|" + _REQ, ex);
                //_RES = ex.Message;

            }



            return r;
        }
        // public int GetResponse_old()
        //{


        //    int r = -1;

        //    //DateTime StartTime = DateTime.Now;
        //    //DateTime Endtime;
        //    //try
        //    //{

        //    //    DCSGlobal.EDI.Comunications.com.ivans.limeservices.EligibilityOne e1 = new DCSGlobal.EDI.Comunications.com.ivans.limeservices.EligibilityOne();
        //    //    IvansWSAuthentication a = new IvansWSAuthentication();

        //    //    a.User =  username;  //"8DC00005";
        //    //    a.Password =  password; // "DC$Gl0bal//2013";
        //    //    a.ClientId = ClientId;   // "f9e52687-a2e0-4d9c-8591-59516c9fd026";

        //    //    e1.PreAuthenticate = Convert.ToBoolean(PreAuthenticate); //  true;
        //    //    e1.Url = wsUrl;
        //    //    e1.IvansWSAuthenticationValue = a;
        //    //    e1.Timeout = Convert.ToInt32(ServiceTimeOut);  // 120000;

        //    //    _RES = e1.SendCommercialEligibilityFormRequest(_REQ);

        //    //    return 0;
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    log.ExceptionDetails("IVANS.GetResponse Failed", ex);
        //    //    //_RES = ex.Message;
        //    //    r = -1;
        //    //}

        //    //Endtime = DateTime.Now;

        //    //_elapsedTicks = Endtime.Ticks - StartTime.Ticks;

        //    return r;
        //}




        //private static void ReadCallback(IAsyncResult asynchronousResult)
        //{
        //    RequestState myRequestState = (RequestState)asynchronousResult.AsyncState;
        //    WebRequest myWebRequest = myRequestState.request;

        //    // End the Asynchronus request.
        //    Stream streamResponse = myWebRequest.EndGetRequestStream(asynchronousResult);

        //    // Create a string that is to be posted to the uri.

        //    string postData = _REQ;
        //    // Convert the string into a byte array.
        //    byte[] byteArray = Encoding.UTF8.GetBytes(postData);

        //    // Write the data to the stream.
        //    streamResponse.Write(byteArray, 0, postData.Length);
        //    streamResponse.Close();
        //    _allDone.Set();
        //}




    }
}
