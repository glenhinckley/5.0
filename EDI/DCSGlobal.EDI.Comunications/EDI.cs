using System;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;


using DCSGlobal.BusinessRules.Logging;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;


namespace DCSGlobal.EDI.Comunications
{
    public class EDI : IDisposable
    {


        logExecption log = new logExecption();
        StringStuff ss = new StringStuff();


        private bool _Verbose = false;
        string _ConnectionString = string.Empty;
        string sGetAllDataSp = string.Empty;
        string sSYNC_TIMEOUT = string.Empty;
        string sSUBMISSION_TIMEOUT = string.Empty;
        string sisParseVBorDB = string.Empty;
        string sisEmdeonLookUp = string.Empty;
        string sreRunEligAttempts = string.Empty;
        string suspEdiRequest = string.Empty;
        string suspEdiDbImport = string.Empty;
        string sCommandTimeOut = "90";
        int iThreadNumber = 0;

        private static string _REQ = string.Empty;
        private static string _RES = string.Empty;
        private string _Accept = string.Empty;
        string sVendorName = string.Empty;
        string sVendorPayorID = string.Empty;
      
       

        
          

        //Dim ts1, ts2, ts3, ts4, ts5, ts6 As DateTime

        //Dim ss As New StringStuff


        private static string _ParameterFilePath = string.Empty;




        //string log As New logExecption





        string appName = "DCSGlobal.EDI.Comunications";
        string sPatientAccountNumber = string.Empty;
        string sUserID = string.Empty;
        string sInsuranceCode = string.Empty;
        string sInsurancetype = string.Empty;
        string sHospitalCode = string.Empty;
        string sPatientHospitalCode = string.Empty;
        string sVendorInputType = string.Empty;
        string sRequestString270 = string.Empty;
        string sID = string.Empty;
        string sRowData = string.Empty;
        string sBatchID = string.Empty;










        ~EDI()
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






        public bool Verbose
        {
            get
            {
                return Convert.ToBoolean(_Verbose);
            }
            set
            {
                _Verbose = Convert.ToBoolean(value);
            }
        }


        public string CommandTimeOut
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                sCommandTimeOut = value;
            }
        }



        public int ThreadNumber
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                iThreadNumber = value;
            }
        }





        public string getAllDataSp
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                sGetAllDataSp = value;
            }
        }


        public string SYNC_TIMEOUT
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                sSYNC_TIMEOUT = value;
            }
        }

        public string SUBMISSION_TIMEOUT
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                sSUBMISSION_TIMEOUT = value;
            }
        }

        public string isParseVBorDB
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                sisParseVBorDB = value;
            }
        }

        public string isEmdeonLookUp
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                sisEmdeonLookUp = value;
            }
        }

        public string reRunEligAttempts
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                sreRunEligAttempts = value;
            }
        }

        public string uspEdiRequest
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                suspEdiRequest = value;
            }
        }

        public string uspEdiDbImport
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                suspEdiDbImport = value;
            }
        }



        public string PayorID
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                sVendorPayorID = value;
            }
        }
        public string VendorName
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                sVendorName = value;
            }
        }

        public string ID
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                sID = value;
            }
        }




        public string Insurancetype
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                sInsurancetype = value;
            }
        }

        public string RequestString270
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                sRequestString270 = value;
            }
        }
        public string VendorInputType
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                sVendorInputType = value;
            }
        }

        public string PatientHospitalCode
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                sPatientHospitalCode = value;
            }
        }

        public string HospitalCode
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                sHospitalCode = value;
            }
        }

        public string InsuranceCode
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                sInsuranceCode = value;
            }
        }

        public string UserID
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                sUserID = value;
            }
        }

        public string AccountNumber
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                sPatientAccountNumber = value;
            }
        }


        public string RowData
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                sRowData = value;
            }
        }


        public string BatchID
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                sBatchID = value;
            }
        }



        public string ParameterFilePath
        {
            get
            {
                return _ParameterFilePath;
            }
            set
            {
                _ParameterFilePath = value;
            }
        }



        public enum AuditType
        {
            Request = 10,
            Response = 20,
            Both = 30

        }



        public string getEligVendorEdiResponse(string RequestContent, string PayorCode, string VendorName, string vendor_input_type)
        {
           
            string _wsurl = string.Empty;
            string _MethodName = string.Empty;
            string _ContentType = string.Empty;
            string _username = string.Empty;
            string _password = string.Empty;
            string _apiKey = string.Empty;
            string _ServiceTimeOut = string.Empty;
            string _TimeToWait = string.Empty;
            string _LogToREQRES = string.Empty;
            string _Token = string.Empty;
            string _PreAuthenticate = string.Empty;
            string _ClientId = string.Empty;
            string _Connection = string.Empty;
            string _Connection1 = string.Empty;
            string _username1 = string.Empty;
            string _password1 = string.Empty;
            string _VS_PROXY_PORT = string.Empty;
            string _VS_PROXY_SERVER = string.Empty;
            string _VS_HOST_ADDRESS = string.Empty;
            string _VS_SES_PORT = string.Empty;
            string _VS_SES_PORT1 = string.Empty;

            sVendorName = VendorName;
            sVendorPayorID = PayorCode;
            _REQ = RequestContent;
           _RES = String.Empty;
            
             if (!string.IsNullOrEmpty(_ConnectionString) )
             {
                 log.ConnectionString = _ConnectionString;
                 if (_Verbose)
                 {
                     log.ExceptionDetails("1-The Given File Path is:", Convert.ToString(_ParameterFilePath) + ":" + VendorName.Trim());
                 }
                 
             }
             int r = -1;
            using(config sConfig = new config(VendorName.Trim()))
            {
                sConfig.ConnectionString = _ConnectionString;
                sConfig.ParameterPath = _ParameterFilePath;
                r = sConfig.LoadConfig();
                if (r == 0)
                {

                    _MethodName = sConfig.Method;
                    _wsurl = sConfig.wsUrl;
                    _ContentType = sConfig.ContentType;

                    _ClientId = sConfig.ClientId;
                    _ServiceTimeOut = sConfig.ServiceTimeOut;
                    _PreAuthenticate = sConfig.PreAuthenticate;
                    _LogToREQRES = sConfig.LogToREQRES;
                    _TimeToWait = sConfig.TimeToWait;
                    _Token = sConfig.Token;
                    _apiKey = sConfig.apiKey;

                    _Connection = sConfig.Connection;
                    _Connection1 = sConfig.Connection1;

                    _username = sConfig.username;
                    _username1 = sConfig.username1;

                    _VS_SES_PORT = sConfig.VS_SES_PORT;
                    _VS_SES_PORT1 = sConfig.VS_SES_PORT1;

                    _password = sConfig.password;
                    _password1 = sConfig.password1;

                    _VS_HOST_ADDRESS = sConfig.VS_HOST_ADDRESS;
                    _VS_PROXY_PORT = sConfig.VS_PROXY_PORT;
                    _VS_PROXY_SERVER = sConfig.VS_PROXY_SERVER;
                    _VS_HOST_ADDRESS = sConfig.VS_HOST_ADDRESS;
                    _Accept = sConfig.Accept;
                    // got to be a bettter way to do this. 
                    switch (VendorName)
                    {
                        case "PARAMFILENOTFOUND":
                            _RES = "There is some problem with reading Parameter file.";
                            break;

                        case "IVANS":
                            using (IVANS ivns = new IVANS())
                            {
                                ivns.VendorName = sVendorName;
                                ivns.PayorCode = sVendorPayorID;
                                ivns.ConnectionString = _ConnectionString;
                                ivns.wsUrl = _wsurl;
                                ivns.username = _username;
                                ivns.password = _password;
                                ivns.ClientId = _ClientId;
                                ivns.PreAuthenticate = _PreAuthenticate;
                                ivns.Method = _MethodName;
                                ivns.ContentType = _ContentType;
                                ivns.ServiceTimeOut = _ServiceTimeOut;
                                ivns.PreAuthenticate = _PreAuthenticate;
                                ivns.REQ = RequestContent;
                                ivns.GetResponse();
                                _RES = ivns.RES;

                            }

                            break;
                        case "VISIONSHARE":
                            using (VISIONSHARE vision = new VISIONSHARE())
                            {
                                vision.VendorName = VendorName;
                                vision.PayorCode = PayorCode;
                                vision.ConnectionString = _ConnectionString;

                                vision.connection = _Connection;
                                vision.connection1 = _Connection1;

                                vision.username = _username;
                                vision.password = _password;

                                vision.username1 = _username1;
                                vision.password1 = _password1;

                                vision.VS_SES_PORT = _VS_SES_PORT;
                                vision.VS_SES_PORT1 = _VS_SES_PORT1;

                                vision.VS_PROXY_PORT = _VS_PROXY_PORT;

                                vision.VS_PROXY_SERVER = _VS_PROXY_SERVER;
                                vision.VS_HOST_ADDRESS = _VS_HOST_ADDRESS;

                                vision.REQ = _REQ;
                                vision.GetResponse();
                                _RES = vision.RES;

                                //avlt.wsUrl = _wsurl;
                                //avlt.Method = _MethodName;
                                //avlt.ContentType = _ContentType;
                                //avlt.REQ = RequestContent;

                                //avlt.GetResponse();

                                //_RES = avlt.RES;

                            }
                            break;

                        case "AVAILITY":

                            using (AVAILITY avlt = new AVAILITY())
                            {
                                avlt.VendorName = VendorName;
                                avlt.PayorCode = PayorCode;
                                avlt.ConnectionString = _ConnectionString;
                                avlt.wsUrl = _wsurl;
                                avlt.Method = _MethodName;
                                avlt.ContentType = _ContentType;
                                avlt.REQ = RequestContent;
                                avlt.ServiceTimeOut = _ServiceTimeOut;
                                avlt.GetResponse();

                                _RES = avlt.RES;

                            }


                            break;

                        case "POST-N-TRACK":
                        case "PNT":

                            using (PNT pnt = new PNT())
                            {
                                pnt.VendorName = VendorName;
                                pnt.PayorCode = PayorCode;
                                pnt.ConnectionString = _ConnectionString;
                                pnt.wsUrl = _wsurl;
                                pnt.Method = _MethodName;
                                pnt.ContentType = _ContentType;

                                pnt.REQ = RequestContent;

                                pnt.GetResponse();
                                _RES = pnt.RES;
                            }

                            break;





                        case "EMDEON":

                            using (EMDEON emd = new EMDEON())
                            {
                                emd.VendorName = VendorName;
                                emd.PayorCode = PayorCode;
                                emd.ConnectionString = _ConnectionString;
                                emd.REQ = RequestContent;
                                emd.ServiceTimeOut = _ServiceTimeOut;
                                emd.wsUrl = _wsurl;
                                emd.GetResponse();
                                _RES = emd.RES;
                            }

                            break;




                        case "DORADO":

                            using (DoradoSystems dor = new DoradoSystems())
                            {
                                dor.VendorName = VendorName;
                                dor.PayorCode = PayorCode;
                                dor.ConnectionString = _ConnectionString;
                                dor.wsUrl = _wsurl;
                                dor.ContentType = _ContentType;
                                dor.Method = _MethodName;
                                dor.REQ = RequestContent;

                                dor.GetResponse();
                                _RES = dor.RES;
                            }

                            break;



                        case "PARAMOUNTEDI":
                            //using (GetResponse pnt = new GetResponse())
                            //{
                            //    _RES = pnt.CallWebService(_REQ, 1, "", "", "");

                            //}
                            // break;
                            using (PARAMOUNTHEALTH pmount = new PARAMOUNTHEALTH())
                            {
                                pmount.VendorName = VendorName;
                                pmount.PayorCode = PayorCode;
                                pmount.ConnectionString = _ConnectionString;
                                pmount.wsUrl = _wsurl;
                                pmount.ContentType = _ContentType;
                                pmount.Method = _MethodName;
                                pmount.Accept = _Accept;
                                pmount.REQ = RequestContent;
                                pmount.ServiceTimeOut = _ServiceTimeOut;
                                pmount.GetResponse();
                                _RES = pmount.RES;
                            }
                            break;


                        case "TXMCD":
                            using (TexasMedicaidHealthcarePartnership txs = new TexasMedicaidHealthcarePartnership())
                            {
                                txs.VendorName = VendorName;
                                txs.PayorCode = PayorCode;
                                txs.ConnectionString = _ConnectionString;
                                txs.wsUrl = _wsurl;
                                txs.ContentType = _ContentType;
                                txs.Method = _MethodName;
                                txs.Accept = _Accept;
                                txs.ServiceTimeOut = _ServiceTimeOut;
                                txs.REQ = RequestContent;
                                //log.ExceptionDetails("5.Submitting-GetResponse", Convert.ToString(_wsurl));
                                txs.GetResponse();
                                txs.RES = txs.RES;
                            }

                            break;
                        case "MEDDATA":

                            using (MEDDATA md = new MEDDATA())
                            {
                                md.VendorName = VendorName;
                                md.PayorCode = PayorCode;
                                md.ConnectionString = _ConnectionString;
                                md.wsUrl = _wsurl;
                                md.username = _username;
                                md.password = _password;
                                md.REQ = _REQ;
                                md.GetResponse();
                                _RES = md.RES;
                            }


                            break;

                        case "SCRAPE":

                            _RES = "your an idiot i dont handle screen scrapping. its beter than what MKC said to say.";

                            break;


                        default:


                            _RES = "your an idiot its beter than what MKC said to say. ";

                            break;
                    }
                }
                else
                {
                    _RES = "There is some problem with reading Parameter file.";
                }
            }
            //log.ExceptionDetails("EDI.Communication.GetResponse for " + VendorName + " is : ", _RES );
            return _RES ;

        }

    }
}
