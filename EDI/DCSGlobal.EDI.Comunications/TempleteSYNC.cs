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
     class TempleteSYNC : IDisposable
    {

        private static ManualResetEvent _allDone = new ManualResetEvent(false);
        private static logExecption log = new logExecption();
        private static StringStuff ss = new StringStuff();


        private static System.Timers.Timer _timer; 


        private static string _appName = "DCSGlobal.EDI.Comunications.";
        private static string _vendorName = "Availity";
    

        private static bool _TimeOut = false;

        private string _ConnectionString = string.Empty;


        private static string _REQ = string.Empty;
        private static string _RES = string.Empty;


        private string _VendorInputType = string.Empty;
        private string _VendorName = string.Empty;
        private string _PayorCode = string.Empty;

        private string _AppName = string.Empty;

        ~TempleteSYNC()
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

   



           


            // Create a request using a URL that can receive a post. 
            WebRequest request = WebRequest.Create(url);
            // Set the Method property of the request to POST.
            request.Method = "POST";

            // Create POST data and convert it to a byte array.
   
            byte[] byteArray = Encoding.UTF8.GetBytes(_REQ);
            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/x-www-form-urlencoded";
            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;

            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();

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


            return rr;

           

        }







    }
}
