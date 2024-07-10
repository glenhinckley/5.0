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
    class PNT  : IDisposable
    {

        //private static ManualResetEvent _allDone = new ManualResetEvent(false);
        private  logExecption log = new logExecption();
        private  StringStuff ss = new StringStuff();


        private string _wsUrl = string.Empty;
        private string _UserName = string.Empty;
        private string _Passwd  = string.Empty;
        private string _apiKey = string.Empty;
        private string _ContentType = string.Empty;
        private string _Method = string.Empty;
        private int _ServiceTimeOut = 0;
        private int _TimeToWait = 0;
        private string _LogToREQRES = string.Empty;
        private string _Token = string.Empty;

        private string _ConnectionString = string.Empty;


        private int _VMetricsLogging = 0;

        private string _ProtocolType = string.Empty;
        private string _VendorInputType = string.Empty;
        private string _VendorName = string.Empty;
        private string _PayorCode = string.Empty;

        private string _AppName = string.Empty;


        private string _SUBMISSION_TIMEOUT = "0.00:00:30";
        private string _SYNC_TIMEOUT =  "0.00:00:30";
        private string SYNC_TIMEOUT = string.Empty;
        private string SUBMISSION_TIMEOUT = string.Empty;
        private string _myResponses = string.Empty;
        private string _myResponses1 = string.Empty;

        private string _REQ = string.Empty;
        private  string _RES = string.Empty;
        long _elapsedTicks = 0;



        ~PNT()
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

        public string wsUrl
        {

            set
            {
                _wsUrl = value;
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




        public int TimeToWait
        {

            set
            {

                _TimeToWait = value;
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
                    log.ExceptionDetails("PNT.StripREQ Failed", ex);
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
                    log.ExceptionDetails("PNT.StripRES Failed", ex);
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
            _RES = string.Empty;
            DateTime StartTime = DateTime.Now;
            DateTime Endtime;
            try
            {
              

                // Create a request using a URL that can receive a post. 
                WebRequest request = WebRequest.Create(_wsUrl);
                // Set the Method property of the request to POST.
                request.Method = _Method;
                request.Timeout = _ServiceTimeOut;
                // Create POST data and convert it to a byte array.
                // string postData = "This is a test that posts this string to a Web server.";
                byte[] byteArray = Encoding.UTF8.GetBytes(_REQ);
                // Set the ContentType property of the WebRequest.
                request.ContentType = _ContentType;
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;

                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();

                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("PNT Sent at " + Convert.ToString(StartTime) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _REQ);
                }

                // Get the response.
                WebResponse response = request.GetResponse();
                // Display the status.
                //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                // Console.WriteLine(responseFromServer);
                // Clean up the streams.
                reader.Close();
                dataStream.Close();
                response.Close();


                _RES = responseFromServer;
                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("PNT REcived  at " + Convert.ToString(DateTime.Now) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _RES);
                }
                
               r = 0;
            }
            catch (Exception ex)
            {
                r = -1;
                Endtime = DateTime.Now;
                _elapsedTicks = Endtime.Ticks - StartTime.Ticks;
                log.ExceptionDetails("GetResponse Failed Total Elasped ticks" + Convert.ToString(_elapsedTicks) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl + "|" + _REQ , ex);

                //_RES = ex.Message;
                
            }

     

            return r;
        }

        
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
