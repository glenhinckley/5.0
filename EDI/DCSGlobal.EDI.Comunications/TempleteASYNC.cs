using System;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;
using System.Timers;
using DCSGlobal.EDI.Comunications;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;

using DCSGlobal.BusinessRules.Logging;

namespace DCSGlobal.EDI.Comunications
{
     class TempleteASYNC : IDisposable
    {

        private static ManualResetEvent _allDone = new ManualResetEvent(false);
        private static logExecption log = new logExecption();
        private static StringStuff ss = new StringStuff();


        private static System.Timers.Timer _timer; 


        private static string _ConnectionString = string.Empty;
        private static string _appName = "DCSGlobal.EDI.Comunications.";
        private static string _vendorName = "Availity";


        private static bool _TimeOut = false;



        private static string _REQ = string.Empty;
        private static string _RES = string.Empty;


        private string _VendorInputType = string.Empty;
        private string _VendorName = string.Empty;
        private string _PayorCode = string.Empty;

        private string _AppName = string.Empty;


        ~TempleteASYNC()
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


        public string VendorInputType
        {

            set
            {

                _VendorInputType = value;
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


        static void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {



        }


        public static int GetResponse(string Request)
        {


            int rr = -1;
            string _httpData = string.Empty;
            string url = string.Empty;

            try
            {



                // Create a new request to the mentioned URL.    
                WebRequest myWebRequest = WebRequest.Create("http://www.contoso.com");

                // Create an instance of the RequestState and assign 
                // 'myWebRequest' to it's request field.
                // just go with this also
                RequestState myRequestState = new RequestState();
                myRequestState.request = myWebRequest;
                myWebRequest.ContentType = "application/x-www-form-urlencoded";

                // Set the 'Method' property  to 'POST' to post data to a Uri.
                myRequestState.request.Method = "POST";




                // Start the Asynchronous 'BeginGetRequestStream' method call. 
                // I call it this way so i can kill it the time out happens and the process dos not hang
                // yea i know its a bit much but its much better this way
                IAsyncResult r = (IAsyncResult)myWebRequest.BeginGetRequestStream(new AsyncCallback(ReadCallback), myRequestState);

      
                // Pause the current thread until the async operation completes.
                _allDone.WaitOne();
      
                // Assign the response object of 'WebRequest' to a 'WebResponse' variable.
                WebResponse myWebResponse = myWebRequest.GetResponse();
     

                Stream streamResponse = myWebResponse.GetResponseStream();
                StreamReader streamRead = new StreamReader(streamResponse);
                Char[] readBuff = new Char[256];
                int count = streamRead.Read(readBuff, 0, 256);
            

                while (count > 0)
                {
                    String outputData = new String(readBuff, 0, count);
                    Console.Write(outputData);
                    count = streamRead.Read(readBuff, 0, 256);
                }

                // Close the Stream Object.
                streamResponse.Close();
                streamRead.Close();


                // Release the HttpWebResponse Resource.
                myWebResponse.Close();

            }
            catch (Exception ex)
            {

                log.ExceptionDetails(_appName + _vendorName, ex);

                _httpData = ex.Message;
            }


            return rr;

        }


        private static void ReadCallback(IAsyncResult asynchronousResult)
        {
            RequestState myRequestState = (RequestState)asynchronousResult.AsyncState;
            WebRequest myWebRequest = myRequestState.request;

            // End the Asynchronus request.
            Stream streamResponse = myWebRequest.EndGetRequestStream(asynchronousResult);

            // Create a string that is to be posted to the uri.

            string postData = _REQ;
            // Convert the string into a byte array.
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            // Write the data to the stream.
            streamResponse.Write(byteArray, 0, postData.Length);
            streamResponse.Close();
            _allDone.Set();
        }




    }
}
