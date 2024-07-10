using System;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Text;
using System.Threading;
using DCSGlobal.EDI.Comunications;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using DCSGlobal.BusinessRules.Logging;



namespace DCSGlobal.EDI.Comunications
{



     class config : IDisposable
    {

        private StringStuff ss = new StringStuff();
        private logExecption log = new logExecption();

        private string _ConnectionString = string.Empty;
        private string _ParameterPath = string.Empty;
        private string _VendorName = string.Empty;
        private string _appName = "DCSGlobal.EDI.Comunications";

        private string _XMLParameters = string.Empty;




        private string ConsoleName = string.Empty;
        private string getAllDataSp = string.Empty;

        private int _CommandTimeOut = 0;
        private int _SYNC_TIMEOUT = 0;
        private int _SUBMISSION_TIMEOUT = 0;
        private string _ErrorLog = string.Empty;
        private string _isParseVBorDB = string.Empty;
        private string _isEmdeonLookUp = string.Empty;
        private string _reRunEligAttempts = string.Empty;
        private string _uspEdiRequest = string.Empty;
        private string _uspEdiDbImport = string.Empty;
        private string _LogPath = string.Empty;
        private string _ClientSettingsProviderServiceUri = string.Empty;
        private int _verbose = 0;



        private string _MaxCount = string.Empty;
        private string _ThreadCount = string.Empty;
        private string _ThreadsON = string.Empty;
        private string _MaxThreads = string.Empty;
        private string _WaitForEnterToExit = string.Empty;
        private int _NoDataTimeOut = 0;
        private int _DeadLockRetrys = 0;
        private int _STARTTIME = 0;
        private int _ENDTIME = 0;
        private bool _cVendorFound = false;


        private string _Accept;
        private string _wsUrl;
        private string _TestwsUrl;
        private string _username;
        private string _password;
        private string _apiKey;
        private string _ContentType;
        private string _Method;
        private int _ServiceTimeOut;
        private int _TimeToWait = 0;
        private int _LogToREQRES = 0;
        private string _Token;
        private bool _PreAuthenticate = false;
        private string _ClientId;
        private string _ResponseFormat = string.Empty;
        private string _RequestFormat = string.Empty;
        private string _Connection = string.Empty;
        private string _login = string.Empty;

        bool _VendorFound = false;

        private bool _disposed;
        private string _username1;
        private string _Connection1;
        private string _password1;


        private int _VS_PROXY_PORT = 0;

        private string _VS_PROXY_SERVER = string.Empty;

        private int _VS_SES_PORT = 0;
        private int _VS_SES_PORT1 = 0;

        private string _VS_HOST_ADDRESS = "";



        private string _PayloadID = string.Empty;
        private string _TimeStamp = string.Empty;
        private string _ProcessingMode = string.Empty;
        private string _PayloadType = string.Empty;
        private string _SenderID = string.Empty;
        private string _ReceiverID = string.Empty;
        private string _CORERuleVersion = string.Empty;
        private string _Payload = string.Empty;
        private long _SendTimeout = 30;
        private long _ReceiveTimeout = 30;
        private int _VMetricsLogging = 0;
        private string _LookupUrl = string.Empty;
        private string _Url276 = string.Empty;
        private string _ProtocolType = string.Empty;
         


        ~config()
        {
            Dispose(false);
        }




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
            ss = null;
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

        public string XMLParameters
        {

            set
            {

                _XMLParameters = value;

            }
        }

        public bool VendorFound
        {

            get
            {

                return _VendorFound;

            }
        }


        public string VendorName
        {

            set
            {

                _VendorName = value;

            }
        }

        //public config(string Vendor)
        //{
        //    _Vendor = Vendor;
        //}


        public bool LoadConfig()
        {
            bool r = false;
            string sssss = string.Empty;




            string sName = string.Empty;
            string sValue = string.Empty;
            string sElement = string.Empty;


            try
            {
                //_ParameterPath
                //using (XmlReader reader = XmlReader.Create("parameters.xml"))
                using (XmlReader reader = XmlReader.Create(new StringReader(_XMLParameters)))
                {
                    while (reader.Read())
                    {


                        // Only detect start elements.
                        if (reader.IsStartElement())
                        {

                            // look for vendor name




                            sssss = reader.Name;

                            if (sssss == "vendor")
                            {
                                //find the vendor name

                                string t = reader["name"];

                      
                                    if (_VendorName == reader["name"])
                                    {

                                        _VendorFound = true;
                                        _cVendorFound = true;
                                    }
                                    else
                                    {

                                        _cVendorFound = false;
                                        //log.ExceptionDetails(_appName, "Vendor not found " + _VendorName);

                                    }
                                }






                            if (_cVendorFound)
                            {
                                //log.ExceptionDetails("5.Vendor Found:", Convert.ToString(_Vendor));
                                switch (reader.Name)
                                {
                                    case "wsdl":
                                        // Detect this article element.
                                        //Console.WriteLine("Start <article> element.");
                                        // Search for the attribute name on this current node.
                                        //string attribute = reader["name"];
                                        string attribute = reader.GetAttribute("url");
                                        if (attribute != null)
                                        {
                                            //if (reader.Read())
                                            //{
                                            //    wsUrl = reader.Value;
                                            //}
                                        }
                                        if (reader.Read())
                                        {
                                            _wsUrl = reader.Value;
                                            //log.ExceptionDetails("5.wsUrl-", Convert.ToString(wsUrl));
                                        }
                                        // Next read will contain text.
                                        break;

                                    case "username":
                                        // Detect this element.
                                        if (reader.Read())
                                        {
                                            _username = reader.Value.Trim();
                                        }
                                        else
                                        {
                                            _username = string.Empty;
                                        }
                                        break;

                                    case "password":
                                        // Detect this element.
                                        if (reader.Read())
                                        {
                                            _password = reader.Value.Trim();
                                        }
                                        else
                                        {
                                            _password = string.Empty;
                                        }
                                        break;

                                    case "apiKey":
                                        // Detect this element.
                                        if (reader.Read())
                                        {
                                            _apiKey = reader.Value.Trim();
                                        }
                                        else
                                        {
                                            _apiKey = string.Empty;
                                        }
                                        break;





                                    case "ResponseFormat":
                                        // Detect this element.
                                        if (reader.Read())
                                        {
                                            _ResponseFormat = reader.Value.Trim();
                                        }
                                        else
                                        {
                                            _ResponseFormat = string.Empty;
                                        }
                                        break;

                                    case "RequestFormat":
                                        // Detect this element.
                                        if (reader.Read())
                                        {
                                            _RequestFormat = reader.Value.Trim();
                                        }
                                        else
                                        {
                                            _RequestFormat = string.Empty;
                                        }
                                        break;




                                    case "Token":
                                        // Detect this element.
                                        if (reader.Read())
                                        {
                                            _Token = reader.Value.Trim();
                                        }
                                        else
                                        {
                                            _Token = string.Empty;
                                        }
                                        break;


                                    case "ContentType":
                                        // Detect this element.
                                        if (reader.Read())
                                        {
                                            _ContentType = reader.Value.Trim();
                                        }
                                        else
                                        {
                                            _ContentType = string.Empty;
                                        }
                                        break;

                                    case "Accept":
                                        if (reader.Read())
                                        {
                                            _Accept = reader.Value.Trim();
                                        }
                                        else
                                        {
                                            _Accept = "";
                                        }
                                        break;
                                    case "Method":
                                        // Detect this element.
                                        if (reader.Read())
                                        {
                                            _Method = reader.Value.Trim();
                                        }
                                        else
                                        {
                                            _Method = string.Empty;
                                        }
                                        break;

                                    case "ServiceTimeOut":
                                        // Detect this element.
                                        if (reader.Read())
                                        {
                                            _ServiceTimeOut = Convert.ToInt32(reader.Value.Trim());
                                        }
                                        else
                                        {
                                            _ServiceTimeOut = 60000;
                                        }
                                        break;

                                    case "TimeToWait":
                                        // Detect this element.
                                        if (reader.Read())
                                        {
                                            _TimeToWait = Convert.ToInt32(reader.Value.Trim());
                                        }
                                        else
                                        {
                                            _TimeToWait = 60000;
                                        }
                                        break;

                                    case "LogToREQRES":
                                        // Detect this element.
                                        if (reader.Read())
                                        {
                                            _LogToREQRES = Convert.ToInt32(reader.Value.Trim());
                                        }
                                        else
                                        {
                                            _LogToREQRES = 0;
                                        }
                                        break;

                                    case "PreAuthenticate":
                                        if (reader.Read())
                                        {
                                            _PreAuthenticate = Convert.ToBoolean(reader.Value.Trim());
                                        }
                                        else
                                        {
                                            _PreAuthenticate = false;
                                        }
                                        break;
                                    case "ClientId":
                                        if (reader.Read())
                                        {
                                            _ClientId = reader.Value.Trim();
                                        }
                                        else
                                        {
                                            _ClientId = string.Empty;
                                        }
                                        break;

                                    case "connection":
                                        if (reader.Read())
                                        {
                                            _Connection = reader.Value.Trim();
                                        }
                                        else
                                        {
                                            _Connection = string.Empty;
                                        }
                                        break;

                                    case "connection1":
                                        if (reader.Read())
                                        {
                                            _Connection1 = reader.Value.Trim();
                                        }
                                        else
                                        {
                                            _Connection1 = string.Empty;
                                        }
                                        break;

                                    case "username1":
                                        if (reader.Read())
                                        {
                                            _username1 = reader.Value.Trim();
                                        }
                                        else
                                        {
                                            _username1 = string.Empty;
                                        }
                                        break;


                                    case "login":
                                        if (reader.Read())
                                        {
                                            _login = reader.Value.Trim();
                                        }
                                        else
                                        {
                                            _login = string.Empty;
                                        }
                                        break;


                                    case "password1":
                                        if (reader.Read())
                                        {
                                            _password1 = reader.Value.Trim();
                                        }
                                        else
                                        {
                                            _password1 = string.Empty;
                                        }
                                        break;

                                    case "VS_PROXY_PORT":
                                        if (reader.Read())
                                        {
                                            _VS_PROXY_PORT = Convert.ToInt32(reader.Value.Trim());
                                        }
                                        else
                                        {
                                            _VS_PROXY_PORT = 0;
                                        }
                                        break;

                                    case "VS_PROXY_SERVER":
                                        if (reader.Read())
                                        {
                                            _VS_PROXY_SERVER = reader.Value.Trim();
                                        }
                                        else
                                        {
                                            _VS_PROXY_SERVER = string.Empty;
                                        }
                                        break;

                                    case "VS_HOST_ADDRESS":
                                        if (reader.Read())
                                        {
                                            _VS_HOST_ADDRESS = reader.Value.Trim();
                                        }
                                        else
                                        {
                                            _VS_HOST_ADDRESS = string.Empty;
                                        }
                                        break;

                                    case "VS_SES_PORT":
                                        if (reader.Read())
                                        {
                                            _VS_SES_PORT = Convert.ToInt32(reader.Value.Trim());
                                        }
                                        else
                                        {
                                            _VS_SES_PORT = 0;
                                        }
                                        break;

                                    case "VS_SES_PORT1":
                                        if (reader.Read())
                                        {
                                            _VS_SES_PORT1 = Convert.ToInt32(reader.Value.Trim());
                                        }
                                        else
                                        {
                                            _VS_SES_PORT1 = 0;
                                        }
                                        break;
                                    case "PayloadID":
                                        if (reader.Read())
                                        {
                                            _PayloadID = reader.Value.Trim();
                                        }
                                        else
                                        {
                                            _PayloadID = string.Empty;
                                        }
                                        break;
                                    case "TimeStamp":
                                        if (reader.Read())
                                        {
                                            _TimeStamp = reader.Value.Trim();
                                        }
                                        else
                                        {
                                            _TimeStamp = string.Empty;
                                        }
                                        break;
                                    case "ProcessingMode":
                                        if (reader.Read())
                                        {
                                            _ProcessingMode = reader.Value.Trim();
                                        }
                                        else
                                        {
                                            _ProcessingMode = string.Empty;
                                        }
                                        break;
                                    case "PayloadType":
                                        if (reader.Read())
                                        {
                                            _PayloadType = reader.Value.Trim();
                                        }
                                        else
                                        {
                                            _PayloadType = string.Empty;
                                        }
                                        break;

                                    case "SenderID":
                                        if (reader.Read())
                                        {
                                            _SenderID = reader.Value.Trim();
                                        }
                                        else
                                        {
                                            _SenderID = string.Empty;
                                        }
                                        break;

                                    case "ReceiverID":
                                        if (reader.Read())
                                        {
                                            _ReceiverID = reader.Value.Trim();
                                        }
                                        else
                                        {
                                            _ReceiverID = string.Empty;
                                        }
                                        break;

                                    case "CORERuleVersion":
                                        if (reader.Read())
                                        {
                                            _CORERuleVersion = reader.Value.Trim();
                                        }
                                        else
                                        {
                                            _CORERuleVersion = string.Empty;
                                        }
                                        break;
                                    case "SendTimeout":
                                        if (reader.Read())
                                        {
                                            _SendTimeout = Convert.ToInt32(reader.Value.Trim());
                                        }
                                        else
                                        {
                                            _SendTimeout = 30;
                                        }
                                        break;
                                    case "ReceiveTimeout":
                                        if (reader.Read())
                                        {
                                            _ReceiveTimeout = Convert.ToInt32(reader.Value.Trim());
                                        }
                                        else
                                        {
                                            _ReceiveTimeout = 30;
                                        }
                                        break;

                                    case "VMetricsLogging":
                                        if (reader.Read())
                                        {
                                            _VMetricsLogging = Convert.ToInt32(reader.Value.Trim());
                                        }
                                        else
                                        {
                                            _VMetricsLogging = 0;
                                        }
                                        break;
                                    case "ProtocolType":
                                        if (reader.Read())
                                        {
                                            _ProtocolType = reader.Value.Trim();
                                        }
                                        else
                                        {
                                            _ProtocolType = string.Empty;
                                        }
                                        break;
                                    case "LookupUrl":
                                        if (reader.Read())
                                        {
                                            _LookupUrl = reader.Value.Trim();
                                        }
                                        else
                                        {
                                            _LookupUrl = string.Empty;
                                        }
                                        break;
                                    case "Url276":
                                        if (reader.Read())
                                        {
                                            _Url276 = reader.Value.Trim();
                                        }
                                        else
                                        {
                                            _Url276 = string.Empty;
                                        }
                                        break;

                                }
                            }


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                r = false;

                log.ExceptionDetails("6" + _appName + " Config", ex);
            }
            return r;
        }

        public int CommandTimeOut
        {
            get
            {
                return _CommandTimeOut;
            }

        }

        public int SYNC_TIMEOUT
        {
            get
            {
                return _SYNC_TIMEOUT;
            }

        }

        public int SUBMISSION_TIMEOUT
        {
            get
            {
                return _SUBMISSION_TIMEOUT;
            }

        }

        public string Url276
        {
            get
            {
                return _Url276;
            }

        }



        public string wsUrl
        {
            get
            {
                return _wsUrl;
            }

        }

        public string TestwsUrl
        {
            get
            {
                return _TestwsUrl;
            }

        }

        public string username
        {
            get
            {
                return _username;
            }

        }

        public string login
        {
            get
            {
                return _login;
            }

        }


        public string password
        {
            get
            {
                return _password;
            }

        }

        public string apiKey
        {
            get
            {
                return _apiKey;
            }

        }

        public string ContentType
        {
            get
            {
                return _ContentType;
            }

        }

        public string Method
        {
            get
            {
                return _Method;
            }

        }

        public int ServiceTimeOut
        {
            get
            {
                return _ServiceTimeOut;
            }

        }

        public int TimeToWait
        {
            get
            {
                return _TimeToWait;
            }

        }



        public string Token
        {
            get
            {
                return _Token;
            }
            set
            {
            }
        }

        public bool PreAuthenticate
        {
            get
            {
                return _PreAuthenticate;
            }
            set
            {
            }
        }

        public string ClientId
        {
            get
            {
                return _ClientId;
            }

        }

        public string Connection
        {
            get
            {
                return _Connection;
            }

        }

        public string Connection1
        {
            get
            {
                return _Connection1;
            }

        }

        public string username1
        {
            get
            {
                return _username1;
            }

        }

        public string password1
        {
            get
            {
                return _password1;
            }

        }

        public int VS_PROXY_PORT
        {
            get
            {
                return _VS_PROXY_PORT;
            }

        }

        public string VS_PROXY_SERVER
        {
            get
            {
                return _VS_PROXY_SERVER;
            }

        }

        public string VS_HOST_ADDRESS
        {
            get
            {
                return _VS_HOST_ADDRESS;
            }

        }

        public int VS_SES_PORT
        {
            get
            {
                return _VS_SES_PORT;
            }

        }

        public int VS_SES_PORT1
        {
            get
            {
                return _VS_SES_PORT1;
            }

        }

        public string Accept
        {
            get
            {
                return _Accept;
            }

        }





        public string RequestFormat
        {
            get
            {
                return _RequestFormat;
            }

        }




        public string ResponseFormat
        {
            get
            {
                return _ResponseFormat;
            }

        }

        public string PayloadID
        {
            get
            {
                return _PayloadID;
            }

        }

        public string TimeStamp
        {
            get
            {
                return _TimeStamp;
            }

        }

        public string ProcessingMode
        {
            get
            {
                return _ProcessingMode;
            }

        }

        public string PayloadType
        {
            get
            {
                return _PayloadType;
            }

        }

        public string SenderID
        {
            get
            {
                return _SenderID;
            }

        }

        public string ReceiverID
        {
            get
            {
                return _ReceiverID;
            }

        }

        public string CORERuleVersion
        {
            get
            {
                return _CORERuleVersion;
            }

        }

        public long SendTimeout
        {
            get
            {
                return _SendTimeout;
            }

        }

        public long ReceiveTimeout
        {
            get
            {
                return _ReceiveTimeout;
            }

        }
        public string ProtocolType
        {

            get
            {
                return _ProtocolType;
            }
        }
        public int VMetricsLogging
        {
            get
            {
                return _VMetricsLogging;
            }

        }

        public string LookupUrl
        {
            get
            {
                return _LookupUrl;
            }

        }

    }
}
