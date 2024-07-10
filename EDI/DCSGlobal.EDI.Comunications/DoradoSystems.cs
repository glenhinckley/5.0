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
     class DoradoSystems : IDisposable
    {


        private StringStuff ss = new StringStuff();
        private logExecption log = new logExecption();



        private string _ConnectionString = string.Empty;


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


        ~DoradoSystems()
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

                _VMetricsLogging  = value;
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

            try
            {



                string wsurl = string.Empty;
                strBlankFields = "&first_name=&last_name=&dob=&gender=&member_id=&ssn=&carrier_code=&eff_date_start=&eff_date_end=&npi=&isa13=&service_types=";
                strEDIData = "&data=" + _REQ;
                url = _wsUrl + strBlankFields + strEDIData;
                //fffffffffffffffffffffffffffffffff

                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("DARADO Sent at " + Convert.ToString(StartTime) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + url, _REQ);
                }

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
                
                _RES = httpData;
                //fffffffffffffffffffffffffffffffff
                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("DARADO REcived  at " + Convert.ToString(DateTime.Now) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + url, _RES);
                }

                
                r = 0;





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

        public int GetResponseDNU()
        {
            int r = -1;
            DateTime StartTime = DateTime.Now;
            DateTime Endtime;

            string url = string.Empty;
            _RES = string.Empty;

            try
            {
                // 'Begin - Request url need to send in below format 

                string strBlankFields = "&first_name=&last_name=&dob=&gender=&member_id=&ssn=&carrier_code=&eff_date_start=&eff_date_end=&npi=&isa13=&service_types=";



                string strEDIData = "&data=" + _REQ;



                url = _wsUrl + strBlankFields + strEDIData;



                // Create a request using a URL that can receive a post. 
                WebRequest request = WebRequest.Create(url);
                // Set the Method property of the request to POST.
                request.Method = _Method;

                // Create POST data and convert it to a byte array.
                string postData = "This is a test that posts this string to a Web server.";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
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
                r = 0;
            }

            catch (Exception ex)
            {
                r = -1;
                log.ExceptionDetails("DORADO.GetResponse Failed " + _VendorName + "-" + _PayorCode + "-" + _wsUrl + "-", ex);

            }

            Endtime = DateTime.Now;

            _elapsedTicks = Endtime.Ticks - StartTime.Ticks;

            return r;


        }

        /*
        public int GetClaimStatus()
        {


            string wsurl = "https://api.doradosystems.com/rt/claim_status?api_key=yccQdicTcceNbLQbPesciCrKH";



            // 'Begin - Request url need to send in below format 

            string strBlankFields = "&first_name=&last_name=&dob=&gender=&member_id=&ssn=&carrier_code=&eff_date_start=&eff_date_end=&npi=&isa13=&service_types=";



            string strEDIData = "&data=" + _REQ;



            wsurl = wsurl + strBlankFields + strEDIData;



            // Create a request using a URL that can receive a post. 
            WebRequest request = WebRequest.Create(wsurl);
            // Set the Method property of the request to POST.
            request.Method = "POST";

            // Create POST data and convert it to a byte array.
            string postData = "This is a test that posts this string to a Web server.";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
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

            return 0;


        }

        */


    }
}
