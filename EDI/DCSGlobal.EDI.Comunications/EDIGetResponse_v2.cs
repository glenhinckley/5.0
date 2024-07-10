using System;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;
using DCSGlobal.BusinessRules.Logging;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using DCSGlobal.BusinessRules.CoreLibraryII;
using DCSGlobal.EDI;
using System.Data;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Data.SqlClient;

namespace DCSGlobal.EDI.Comunications
{
    public class EDIGetResponse_v2 : IDisposable
    {



        private bool Is999 = false;
        logExecption log = new logExecption();
        StringStuff ss = new StringStuff();

        private int _WaitTime = 0;
        private int _Mutiplyer = 5;

        private bool _Verbose = false;
        private string _ConnectionString = string.Empty;

        private string _REQ = string.Empty;
        private string _RES = string.Empty;
        private long _BatchID = 0;
        private string _EBRID = string.Empty;

        private string _LoginUserID = string.Empty;

        private int norespid = -1;

        private string _subscriberfirstname =string.Empty; 
        private string _subscriberlastname=string.Empty;
        private string _subscriberdob=string.Empty;
        private string _dateofservice = string.Empty;
        private string _PatientHospitalCode = string.Empty;
        private string _SarchType = string.Empty;

        private string _HospCode = string.Empty;

        private string _InsType = string.Empty;
        private string _PatAcctNum = string.Empty;

        
        
        private string _LookupUrl = string.Empty; 

        

        private string _TransactionSetIdentifierCode = string.Empty;


        private string B_REQ = string.Empty;
        private string B_RES = string.Empty;

        private int _TaskID = 0;

        private string _CommandTimeOut = "90";

        private string _XMLParameters = string.Empty;
        private bool _FallBack = false;
        private bool _AllowFallback = true;
        private bool _isTest = false;
        private bool doRESValidation = false;
        private bool isISA = false;
        private bool isXML = false;

        private int _EmdeonSmartDelete = 0;
       
        private int _VendorRetrys = 3;

        private string _OrigRES = string.Empty;

        private bool _disposed;

        ~EDIGetResponse_v2()
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



        public int TaskID
        {

            set
            {
                _TaskID = value;
            }
        }



        public int VendorRetrys
        {

            set
            {
                _VendorRetrys = value;
            }
        }




        public long BatchID
        {

            set
            {
                _BatchID = value;
            }
        }


        public string EBRID
        {

            set
            {
                _EBRID = Convert.ToString(value);
            }
        }

        public string LoginUserID
        {

            set
            {
                _LoginUserID = Convert.ToString(value);
            }
        }
        

        public string subscriberfirstname
        {

            set
            {
                _subscriberfirstname = Convert.ToString(value);
            }
        }

        public string subscriberlastname
        {

            set
            {
                _subscriberlastname = Convert.ToString(value);
            }
        }

        public string subscriberdob
        {

            set
            {
                _subscriberdob = Convert.ToString(value);
            }
        }

        public string  dateofservice
        {

            set
            {
                _dateofservice = Convert.ToString(value);
            }
        }

        public string PatientHospitalCode
        {

            set
            {
                _PatientHospitalCode = Convert.ToString(value);
            }
        }

        public string SarchType
        {

            set
            {
                _SarchType = Convert.ToString(value);
            }
        }
        
        public string PatAcctNum
        {

            set
            {
                _PatAcctNum = Convert.ToString(value);
            }
        }


        public string HospCode
        {

            set
            {
                _HospCode = Convert.ToString(value);
            }
        }

        public string InsType
        {

            set
            {
                _InsType = Convert.ToString(value);
            }
        }
        
        

        public string LookupUrl
        {

            set
            {
                _LookupUrl = Convert.ToString(value);
            }
        }

        public int EmdeonSmartDelete
        {
            get
            {
                return _EmdeonSmartDelete;
            }
            set
            {
                try
                {
                    _EmdeonSmartDelete = value;

                }
                catch (Exception ex)
                {

                }
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
            set
            {
                _Verbose = Convert.ToBoolean(value);
            }
        }

        public bool isTest
        {

            set
            {
                _isTest = Convert.ToBoolean(value);
            }
        }




        public bool AllowFallback
        {

            set
            {
                _AllowFallback = Convert.ToBoolean(value);
            }
        }


        public string XMLParameters
        {
            set
            {
                _XMLParameters = value;
            }
        }


        public int Mutiplyer
        {

            set
            {

                _Mutiplyer = value;
            }
        }

        public int GetXMLParameters()
        {
            int r = -1;
            try
            {

                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {
                    con.Open();
                    //
                    // Create new SqlCommand object.
                    //
                    using (SqlCommand cmd = new SqlCommand("usp_get_system_preferences", con))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@moduleName", "EDIGetResponse");
                        cmd.Parameters.AddWithValue("@lovName ", "_XMLParameters");

                        using (SqlDataReader idr = cmd.ExecuteReader())
                        {
                            //  count = idr.VisibleFieldCount;




                            if (idr.HasRows)
                            {
                                while (idr.Read())
                                {

                                    _XMLParameters = Convert.ToString(idr["lov_value"]);
                                    if (_Verbose)
                                    {
                                        log.ExceptionDetails("read system preferences xml from Communication dll instead of external(not from exe or FE) :"  , "usp_get_system_preferences");
                                    }
                                    r = 0;
                                }
                            }
                        }
                    }
                }

            }
            catch (SqlException sx)
            {

                log.ExceptionDetails("GetXMLParameters-", sx.Message);
              
            }

            catch (Exception ex)
            {
                log.ExceptionDetails("GetXMLParameters-", ex.Message);
        
            }


            return r;

        }

        public int getEligVendorEdiResponse(string RequestContent, string PayorCode, string VendorName, string vendor_input_type)
        {

            if (_Verbose)
            {
                log.ExceptionDetails("getEligVendorEdiResponseEx-" + VendorName, "Started");
            }

            int r = -1;

            int x = -1;
            _REQ = RequestContent;

            string NPI = string.Empty;
            string ServiceTypeCode = string.Empty;
            string AAAFailureCode = string.Empty;
            // get NPI eq 

            string REQ_TransactionSetIdentifierCode = String.Empty;
            string RES_TransactionSetIdentifierCode = String.Empty;
            //   TransactionSetIdentifierCode

            string sub = _REQ.Substring(0, 3);
            try
            {
                if (sub == "ISA")
                {
                    isISA = true;
                    using (ValidateEDI vedi = new ValidateEDI())
                    {
                        vedi.ConnectionString = _ConnectionString;
                        vedi.byString(_REQ);
                        _TransactionSetIdentifierCode = vedi.TransactionSetIdentifierCode;
                        B_REQ = vedi.ReferenceIdentification;
                        REQ_TransactionSetIdentifierCode = vedi.TransactionSetIdentifierCode;
                        NPI = vedi.NPI;
                        ServiceTypeCode = vedi.ServiceTypeCode;
                    }
                }
                else
                {
                    //for Meddata i.e in xml format
                    try
                    {
                        //service type code
                        using (ValidateEDI vedi = new ValidateEDI())
                        {
                            vedi.ConnectionString = _ConnectionString;
                            ServiceTypeCode = ss.ProcessAVAILITYXML(_REQ, "serviceTypeCode");
                        }
                    }
                    catch (Exception ex)
                    {
                        // change to something usefull
                        //log.ExceptionDetails("1000-DCSGlobal.EDI.Comunications.EDIGetResponse.getEligVendorEdiResponseEx", "serviceTypeCode Not Found  | " + VendorName);
                        ServiceTypeCode = "";
                    }
                    try
                    {
                        //NPI
                        using (ValidateEDI vedi = new ValidateEDI())
                        {
                            vedi.ConnectionString = _ConnectionString;
                            NPI = ss.ProcessAVAILITYXML(_REQ, "providerId");
                        }
                    }
                    catch (Exception ex)
                    {
                        // change to something usefull
                        //log.ExceptionDetails("1010-DCSGlobal.EDI.Comunications.EDI.EDIGetResponse.getEligVendorEdiResponseEx", "Vendor Name Not Found  | " + VendorName);
                        NPI = "";
                    }
                }
            }
            catch (Exception ex)
            {
                // change to something usefull
                //log.ExceptionDetails("1020-DCSGlobal.EDI.Comunications.EDI.EDIGetResponse.getEligVendorEdiResponseEx", "Vendor Name Not Found  | " + VendorName);
            }




            using (HighEntropyRamdonNumberGenerator rnd = new HighEntropyRamdonNumberGenerator())
            {
                rnd.Mutiplyer = 1; // _Mutiplyer;
                _WaitTime = rnd.Next();
                //  _WaitTime = 0;
            }

           
            _OrigRES = string.Empty;
            _RES = String.Empty;
            config c = new config();
            c.VendorName = VendorName.ToUpper();

            x = GetXMLParameters();   //assign _XMLParameters
            c.XMLParameters = _XMLParameters;
            c.LoadConfig();

            if (c.VendorFound)
            {
                using (AuditResponseLogging al = new AuditResponseLogging())
                {

                    al.ConnectionString = _ConnectionString;
                    al.EligibilityUpdateEBRProcessedFlagBeforeVendorCall(_EBRID);
                }

                switch (VendorName.ToUpper())
                {
                    
                    case "AVAILITY_OLD":

                        using (AvailityEx avlt = new AvailityEx())
                        {
                            avlt.VendorName = VendorName;
                            avlt.PayorCode = PayorCode;
                            avlt.ConnectionString = _ConnectionString;

                            avlt.TaskID = _TaskID;
                            avlt.EBRID = _EBRID;
                            avlt.WaitTime = _WaitTime;
                            avlt.BHT03 = B_REQ;
                            avlt.VendorRetrys = _VendorRetrys;


                            if (!_isTest)
                            { avlt.wsUrl = c.wsUrl; }
                            else
                            { avlt.wsUrl = c.TestwsUrl; }

                            avlt.Method = c.Method;
                            avlt.ContentType = c.ContentType;
                            avlt.REQ = RequestContent;
                            avlt.PayloadID = B_REQ;
                            avlt.ServiceTimeOut = c.ServiceTimeOut;
                            avlt.VMetricsLogging = c.VMetricsLogging;
                            avlt.PayloadType = _TransactionSetIdentifierCode;
                            r = avlt.GetResponse();
                            _RES = avlt.RES;
                            if (r == 0)
                            {
                                _RES = avlt.RES;
                                _OrigRES = avlt.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = avlt.OrigRes;
                                log.ExceptionDetails("1030-DCSGlobal.EDI.Comunications.EDI.AVAILITYEx.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;

                    case "AVAILITY":

                        using (AvailitySoap avltsoap = new AvailitySoap())
                        {
                            avltsoap.VendorName = VendorName;
                            avltsoap.PayorCode = PayorCode;
                            avltsoap.ConnectionString = _ConnectionString;
                            avltsoap.wsUrl = c.wsUrl;

                            avltsoap.UserName = c.username;
                            avltsoap.Passwd = c.password;

                            avltsoap.REQ = RequestContent;

                            avltsoap.TaskID = _TaskID;
                            avltsoap.EBRID = _EBRID;
                            avltsoap.WaitTime = _WaitTime;
                            avltsoap.BHT03 = B_REQ;
                            avltsoap.VendorRetrys = _VendorRetrys;


                            avltsoap.PayloadType = c.PayloadType;
                            avltsoap.ProcessingMode = c.ProcessingMode;
                            avltsoap.PayloadID = B_REQ; //  c.PayloadID;
                            avltsoap.TimeStamp = c.TimeStamp;
                            avltsoap.SenderID = c.SenderID;
                            avltsoap.ReceiverID = c.ReceiverID;
                            avltsoap.CORERuleVersion = c.CORERuleVersion;
                            avltsoap.SendTimeout = c.ServiceTimeOut;
                            avltsoap.ReceiveTimeout = c.ReceiveTimeout;
                            avltsoap.VMetricsLogging = c.VMetricsLogging;



                            if (!_isTest)
                            { avltsoap.wsUrl = c.wsUrl; }
                            else
                            { avltsoap.wsUrl = c.TestwsUrl; }

                            r = avltsoap.GetResponse();
                            if (r == 0)
                            {
                                _RES = avltsoap.RES;
                                _OrigRES = avltsoap.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = avltsoap.OrigRes;
                                log.ExceptionDetails("1165-DCSGlobal.EDI.Comunications.EDI.AVAILITY.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;

                   

                    case "IVANS":
                        using (IvansEx ivns = new IvansEx())
                        {
                            ivns.VendorName = VendorName;
                            ivns.PayorCode = PayorCode;
                            ivns.ConnectionString = _ConnectionString;

                            ivns.TaskID = _TaskID;
                            ivns.EBRID = _EBRID;
                            ivns.WaitTime = _WaitTime;
                            ivns.BHT03 = B_REQ;
                            ivns.VendorRetrys = _VendorRetrys;


                            if (!_isTest)
                            { ivns.wsdl = c.wsUrl; }
                            else
                            { ivns.wsdl = c.TestwsUrl; }

                            ivns.UserName = c.username;
                            ivns.Passwd = c.password;
                            ivns.ClientId = c.ClientId;
                            ivns.PreAuthenticate = c.PreAuthenticate;
                            ivns.Method = c.Method;
                            ivns.ContentType = c.ContentType;
                            ivns.ServiceTimeOut = c.ServiceTimeOut;
                            ivns.REQ = RequestContent;
                            ivns.VMetricsLogging = c.VMetricsLogging;
                            r = ivns.GetResponse();
                            if (r == 0)
                            {
                                _RES = ivns.RES;
                                _OrigRES = ivns.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = ivns.OrigRes;
                                log.ExceptionDetails("1040-DCSGlobal.EDI.Comunications.EDI.IVANSEx.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;

                    case "VISIONSHARE":
                        using (VisionShareEx vision = new VisionShareEx())
                        {
                            vision.VendorName = VendorName;
                            vision.PayorCode = PayorCode;
                            vision.ConnectionString = _ConnectionString;

                            vision.Connection = c.Connection;
                            vision.Connection1 = c.Connection1;

                            vision.UserName = c.username;
                            vision.Passwd = c.password;

                            vision.UserName1 = c.username1;
                            vision.Passwd1 = c.password1;

                            vision.VS_SES_PORT = c.VS_SES_PORT;
                            vision.VS_SES_PORT1 = c.VS_SES_PORT1;

                            vision.VS_PROXY_PORT = c.VS_PROXY_PORT;

                            vision.VS_PROXY_SERVER = c.VS_PROXY_SERVER;
                            vision.VS_HOST_ADDRESS = c.VS_HOST_ADDRESS;
                            vision.VMetricsLogging = c.VMetricsLogging;
                            vision.REQ = _REQ;
                            r = vision.GetResponse();
                            if (r == 0)
                            {
                                _RES = vision.RES;
                                _OrigRES = vision.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = vision.OrigRes;
                                log.ExceptionDetails("1050-DCSGlobal.EDI.Comunications.EDI.VISIONSHAREEx.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;

                    case "POST-N-TRACK":
                    case "PNT":

                        using (PNTEx pnt = new PNTEx())
                        {
                            pnt.VendorName = VendorName;
                            pnt.PayorCode = PayorCode;
                            pnt.ConnectionString = _ConnectionString;

                            pnt.TaskID = _TaskID;
                            pnt.EBRID = _EBRID;
                            pnt.WaitTime = _WaitTime;
                            pnt.BHT03 = B_REQ;
                            pnt.VendorRetrys = _VendorRetrys;

                            if (!_isTest)
                            { pnt.wsUrl = c.wsUrl; }
                            else
                            { pnt.wsUrl = c.TestwsUrl; }

                            pnt.Method = c.Method;
                            pnt.ContentType = c.ContentType;
                            pnt.ServiceTimeOut = c.ServiceTimeOut;
                            pnt.REQ = RequestContent;
                            pnt.VMetricsLogging = c.VMetricsLogging;
                            r = pnt.GetResponse();
                            if (r == 0)
                            {
                                _RES = pnt.RES;
                                _OrigRES = pnt.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = pnt.OrigRes;
                                log.ExceptionDetails("1060-DCSGlobal.EDI.Comunications.EDI.PNTEx.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;
                    case "EMDEONLOOKUP":
                        using (EmdeonEx emdlkp = new EmdeonEx())
                        {
                            emdlkp.VendorName = VendorName;
                            emdlkp.PayorCode = PayorCode;
                            emdlkp.ConnectionString = _ConnectionString;
                            emdlkp.REQ = RequestContent;
                            emdlkp.ServiceTimeOut = c.ServiceTimeOut;
                            emdlkp.UserName = c.username;
                            emdlkp.Passwd = c.password;
                            emdlkp.VMetricsLogging = c.VMetricsLogging;

                            emdlkp.subscriberlastname = _subscriberlastname;
                            emdlkp.subscriberfirstname = _subscriberfirstname;
                            emdlkp.subscriberdob = _subscriberdob;
                            emdlkp.dateofservice = _dateofservice;
                            emdlkp.apiKey = c.apiKey;
                            emdlkp.PatAcctNum = _PatAcctNum;

                            emdlkp.PatientHospitalCode = _PatientHospitalCode;
                            emdlkp.Hosp_Code = _HospCode;
                            emdlkp.InsType = _InsType;
                            emdlkp.LoginUserID = _LoginUserID;
                            emdlkp.TaskID = _TaskID;
                            emdlkp.EBRID = _EBRID;
                            emdlkp.WaitTime = _WaitTime;
                            emdlkp.BHT03 = B_REQ;
                            emdlkp.VendorRetrys = 1;  //_VendorRetrys;  --As per suresh set to 1
                            emdlkp.BatchID = _BatchID;
                            emdlkp.LookupUrl = c.LookupUrl;

                            if (!_isTest)
                            { emdlkp.wsUrl = c.wsUrl; }
                            else
                            { emdlkp.wsUrl = c.TestwsUrl; }

                            r = emdlkp.GetResponse();
                            if (r == 0)
                            {
                                //_RES = emd.RES;
                                //doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = emdlkp.OrigRes;
                                //log.ExceptionDetails("1060-DCSGlobal.EDI.Comunications.EDI.EMDEON.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;

                    case "EMDEON":
                        using (EmdeonOnly emdonly = new EmdeonOnly())
                        {
                            emdonly.VendorName = VendorName;
                            emdonly.PayorCode = PayorCode;
                            emdonly.ConnectionString = _ConnectionString;
                            emdonly.REQ = RequestContent;
                            emdonly.ServiceTimeOut = c.ServiceTimeOut;
                            emdonly.UserName = c.username;
                            emdonly.Passwd = c.password;
                            emdonly.VMetricsLogging = c.VMetricsLogging;

                            emdonly.subscriberlastname = _subscriberlastname;
                            emdonly.subscriberfirstname = _subscriberfirstname;
                            emdonly.subscriberdob = _subscriberdob;
                            emdonly.dateofservice = _dateofservice;
                            emdonly.apiKey = c.apiKey;
                            emdonly.PatAcctNum = _PatAcctNum;

                            emdonly.PatientHospitalCode = _PatientHospitalCode;
                            emdonly.Hosp_Code = _HospCode;
                            emdonly.InsType = _InsType;
                            emdonly.LoginUserID = _LoginUserID;
                            emdonly.TaskID = _TaskID;
                            emdonly.EBRID = _EBRID;
                            emdonly.WaitTime = _WaitTime;
                            emdonly.BHT03 = B_REQ;
                            emdonly.VendorRetrys = _VendorRetrys;   
                            emdonly.BatchID = _BatchID;
                            emdonly.LookupUrl = c.LookupUrl;

                            if (!_isTest)
                            { emdonly.wsUrl = c.wsUrl; }
                            else
                            { emdonly.wsUrl = c.TestwsUrl; }

                            r = emdonly.GetResponse();
                            if (r == 0)
                            {
                                _RES = emdonly.RES;
                                _OrigRES = emdonly.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = emdonly.OrigRes;
                                log.ExceptionDetails("1060-DCSGlobal.EDI.Comunications.EDI.Emdonly.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;
                    case "DORADO":

                        using (DoradoSystemsEx dor = new DoradoSystemsEx())
                        {
                            dor.VendorName = VendorName;
                            dor.PayorCode = PayorCode;
                            dor.ConnectionString = _ConnectionString;

                            dor.TaskID = _TaskID;
                            dor.EBRID = _EBRID;
                            dor.WaitTime = _WaitTime;
                            dor.BHT03 = B_REQ;
                            dor.VendorRetrys = _VendorRetrys;


                            if (!_isTest)
                            { dor.wsUrl = c.wsUrl; }
                            else
                            { dor.wsUrl = c.TestwsUrl; }

                            dor.ContentType = c.ContentType;
                            dor.Method = c.Method;
                            dor.REQ = RequestContent;
                            dor.ServiceTimeOut = c.ServiceTimeOut;
                            dor.VMetricsLogging = c.VMetricsLogging;
                            dor.Url276 = c.Url276;
                            r = dor.GetResponse();
                            if (r == 0)
                            {
                                _RES = dor.RES;
                                _OrigRES = dor.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                _OrigRES = dor.OrigRes;
                                r = -1;
                                log.ExceptionDetails("1080-DCSGlobal.EDI.Comunications.EDI.DORADO.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;

                    case "PARAMOUNTEDI":
                        using (ParamountHealthEx paramount = new ParamountHealthEx())
                        {

                            paramount.VendorName = VendorName;
                            paramount.PayorCode = PayorCode;
                            paramount.ConnectionString = _ConnectionString;
                            paramount.wsUrl = c.wsUrl;

                            paramount.UserName = c.username;
                            paramount.Passwd = c.password;

                            paramount.REQ = RequestContent;

                            paramount.TaskID = _TaskID;
                            paramount.EBRID = _EBRID;
                            paramount.WaitTime = _WaitTime;
                            paramount.BHT03 = B_REQ;
                            paramount.VendorRetrys = _VendorRetrys;


                            paramount.PayloadType = c.PayloadType;
                            paramount.ProcessingMode = c.ProcessingMode;
                            paramount.PayloadID = B_REQ; //  c.PayloadID;
                            paramount.TimeStamp = c.TimeStamp;
                            paramount.SenderID = c.SenderID;
                            paramount.ReceiverID = c.ReceiverID;
                            paramount.CORERuleVersion = c.CORERuleVersion;
                            paramount.SendTimeout = c.ServiceTimeOut;
                            paramount.ReceiveTimeout = c.ReceiveTimeout;
                            paramount.VMetricsLogging = c.VMetricsLogging;



                            if (!_isTest)
                            { paramount.wsUrl = c.wsUrl; }
                            else
                            { paramount.wsUrl = c.TestwsUrl; }

                            r = paramount.GetResponse();
                            if (r == 0)
                            {
                                _RES = paramount.RES;
                                _OrigRES = paramount.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = paramount.OrigRes;
                                log.ExceptionDetails("1165-DCSGlobal.EDI.Comunications.EDI.CENTENE.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;


                    case "TXMCD_DIRECT":
                        using (TexasMedicaidHealthcarePartnershipEx txs = new TexasMedicaidHealthcarePartnershipEx())
                        {
                            txs.VendorName = VendorName;
                            txs.PayorCode = PayorCode;
                            txs.ConnectionString = _ConnectionString;
                            txs.wsUrl = c.wsUrl;

                            txs.TaskID = _TaskID;
                            txs.EBRID = _EBRID;
                            txs.WaitTime = _WaitTime;
                            txs.BHT03 = B_REQ;
                            txs.VendorRetrys = _VendorRetrys;

                            if (!_isTest)
                            { txs.wsUrl = c.wsUrl; }
                            else
                            { txs.wsUrl = c.TestwsUrl; }
                            txs.VMetricsLogging = c.VMetricsLogging;
                            txs.ContentType = c.ContentType;
                            txs.Method = c.Method;
                            txs.Accept = c.Accept;
                            txs.ServiceTimeOut = c.ServiceTimeOut;
                            txs.VMetricsLogging = c.VMetricsLogging;
                            txs.REQ = RequestContent;
                            r = txs.GetResponse();
                            if (r == 0)
                            {
                                _RES = txs.RES;
                                _OrigRES = txs.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = txs.OrigRes;
                                log.ExceptionDetails("1100-DCSGlobal.EDI.Comunications.EDI.TXMCD_DIRECT.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }

                        break;

                    case "TXMCD":
                        using (TexasMedicaidHealthcarePartnershipDCSWebServiceEx dtxs = new TexasMedicaidHealthcarePartnershipDCSWebServiceEx())
                        {
                            dtxs.VendorName = VendorName;
                            dtxs.PayorCode = PayorCode;
                            dtxs.ConnectionString = _ConnectionString;
                            dtxs.wsUrl = c.wsUrl;

                            dtxs.TaskID = _TaskID;
                            dtxs.EBRID = _EBRID;
                            dtxs.WaitTime = _WaitTime;
                            dtxs.BHT03 = B_REQ;
                            dtxs.VendorRetrys = _VendorRetrys;

                            dtxs.login = c.login;
                            dtxs.password = c.password;


                            if (!_isTest)
                            { dtxs.wsUrl = c.wsUrl; }
                            else
                            { dtxs.wsUrl = c.TestwsUrl; }

                            dtxs.ContentType = c.ContentType;
                            dtxs.Method = c.Method;
                            dtxs.VMetricsLogging = c.VMetricsLogging;
                            dtxs.ServiceTimeOut = c.ServiceTimeOut;
                            dtxs.REQ = RequestContent;
                            r = dtxs.GetResponse();
                            if (r == 0)
                            {
                                _RES = dtxs.RES;
                                _OrigRES = dtxs.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = dtxs.OrigRes;
                                log.ExceptionDetails("1110-DCSGlobal.EDI.Comunications.EDI.TXMCD.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;

                    case "MEDDATA":

                        isXML = true;

                        using (MedDataEx md = new MedDataEx())
                        {
                            md.VendorName = VendorName;
                            md.PayorCode = PayorCode;
                            md.ConnectionString = _ConnectionString;

                            md.TaskID = _TaskID;
                            md.EBRID = _EBRID;
                            md.WaitTime = _WaitTime;
                            md.BHT03 = B_REQ;
                            md.VendorRetrys = _VendorRetrys;



                            if (!_isTest)
                            { md.wsUrl = c.wsUrl; }
                            else
                            { md.wsUrl = c.TestwsUrl; }


                            md.UserName = c.username;
                            md.Passwd = c.password;
                            md.RequestFormat = c.RequestFormat;
                            md.ResponseFormat = c.ResponseFormat;
                            md.VMetricsLogging = c.VMetricsLogging;
                            md.REQ = _REQ;
                            r = md.GetResponse();
                            if (r == 0)
                            {
                                _RES = md.RES;
                                _OrigRES = md.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = md.OrigRes;
                                log.ExceptionDetails("1120-DCSGlobal.EDI.Comunications.EDI.MEDDATA.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;

                    case "MEMCD":
                        using (MaineMedicaidEx memcd = new MaineMedicaidEx())
                        {
                            memcd.VendorName = VendorName;
                            memcd.PayorCode = PayorCode;
                            memcd.ConnectionString = _ConnectionString;
                            memcd.wsUrl = c.wsUrl;

                            memcd.TaskID = _TaskID;

                            memcd.UserName = c.username;
                            memcd.Passwd = c.password;
                            memcd.EBRID = _EBRID;
                            memcd.REQ = RequestContent;
                            memcd.BHT03 = B_REQ;
                            memcd.VendorRetrys = _VendorRetrys;
                            memcd.PayloadType = c.PayloadType;
                            memcd.ProcessingMode = c.ProcessingMode;
                            memcd.PayloadID = B_REQ; //Convert.ToString( Guid.NewGuid()) ;
                            memcd.TimeStamp = c.TimeStamp;
                            memcd.SenderID = c.SenderID;
                            memcd.ReceiverID = c.ReceiverID;
                            memcd.CORERuleVersion = c.CORERuleVersion;
                            memcd.SendTimeout = c.SendTimeout;
                            memcd.ReceiveTimeout = c.ReceiveTimeout;
                            memcd.VMetricsLogging = c.VMetricsLogging;
                            memcd.WaitTime = _WaitTime;
                            memcd.BatchID = _BatchID;
                            if (!_isTest)
                            { memcd.wsUrl = c.wsUrl; }
                            else
                            { memcd.wsUrl = c.TestwsUrl; }




                            r = memcd.GetResponse();
                            if (r == 0)
                            {
                                _RES = memcd.RES;
                                _OrigRES = memcd.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = memcd.OrigRes;
                                log.ExceptionDetails("1130-DCSGlobal.EDI.Comunications.EDI.MEMCD.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;

                    case "ARMCD":
                        using (ArkansasMedicaidEx armcd = new ArkansasMedicaidEx())
                        {

                            armcd.VendorName = VendorName;
                            armcd.PayorCode = PayorCode;
                            armcd.ConnectionString = _ConnectionString;
                            armcd.wsUrl = c.wsUrl;

                            armcd.UserName = c.username;
                            armcd.Passwd = c.password;

                            armcd.REQ = RequestContent;

                            armcd.TaskID = _TaskID;
                            armcd.EBRID = _EBRID;
                            armcd.WaitTime = _WaitTime;
                            armcd.BHT03 = B_REQ;
                            armcd.VendorRetrys = _VendorRetrys;



                            armcd.PayloadType = c.PayloadType;
                            armcd.ProcessingMode = c.ProcessingMode;
                            armcd.PayloadID = B_REQ; //  c.PayloadID;
                            armcd.TimeStamp = c.TimeStamp;
                            armcd.SenderID = c.SenderID;
                            armcd.ReceiverID = c.ReceiverID;
                            armcd.CORERuleVersion = c.CORERuleVersion;
                            armcd.SendTimeout = c.SendTimeout;
                            armcd.ReceiveTimeout = c.ReceiveTimeout;
                            armcd.VMetricsLogging = c.VMetricsLogging;

                            if (!_isTest)
                            { armcd.wsUrl = c.wsUrl; }
                            else
                            { armcd.wsUrl = c.TestwsUrl; }

                            r = armcd.GetResponse();
                            if (r == 0)
                            {
                                _RES = armcd.RES;
                                _OrigRES = armcd.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = armcd.OrigRes;
                                log.ExceptionDetails("1140-DCSGlobal.EDI.Comunications.EDI.ARMCD.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;

                    case "NEMCD":
                        using (NebraskaMedicaid nemcd = new NebraskaMedicaid())
                        {

                            nemcd.VendorName = VendorName;
                            nemcd.PayorCode = PayorCode;
                            nemcd.ConnectionString = _ConnectionString;
                            nemcd.wsUrl = c.wsUrl;
                            nemcd.EBRID = _EBRID;
                            nemcd.UserName = c.username;
                            nemcd.Passwd = c.password;
                            nemcd.VendorRetrys = _VendorRetrys;
                            nemcd.REQ = RequestContent;
                            nemcd.BHT03 = B_REQ;
                            nemcd.SenderID = c.SenderID;
                            nemcd.ReceiverID = c.ReceiverID;
                            nemcd.VMetricsLogging = c.VMetricsLogging;

                            if (!_isTest)
                            { nemcd.wsUrl = c.wsUrl; }
                            else
                            { nemcd.wsUrl = c.TestwsUrl; }

                            r = nemcd.GetResponse();
                            if (r == 0)
                            {
                                _RES = nemcd.RES;
                                _OrigRES = nemcd.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = nemcd.OrigRes;
                                log.ExceptionDetails("1140-DCSGlobal.EDI.Comunications.EDI.NEMCD.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;
                    case "GAMCD":

                        using (GeorgiaMedicaidEx grmcd = new GeorgiaMedicaidEx())
                        {
                            grmcd.VendorName = VendorName;
                            grmcd.PayorCode = PayorCode;
                            grmcd.ConnectionString = _ConnectionString;
                            grmcd.wsUrl = c.wsUrl;
                            grmcd.UserName = c.username;
                            grmcd.Passwd = c.password;
                            grmcd.REQ = RequestContent;

                            grmcd.TaskID = _TaskID;
                            grmcd.EBRID = _EBRID;
                            grmcd.WaitTime = _WaitTime;
                            grmcd.BHT03 = B_REQ;
                            grmcd.VendorRetrys = _VendorRetrys;



                            grmcd.PayloadType = c.PayloadType;
                            grmcd.ProcessingMode = c.ProcessingMode;
                            grmcd.SenderID = c.SenderID;
                            grmcd.ReceiverID = c.ReceiverID;
                            grmcd.CORERuleVersion = c.CORERuleVersion;
                            grmcd.ServiceTimeOut = c.ServiceTimeOut;
                            grmcd.VMetricsLogging = c.VMetricsLogging;
                            if (!_isTest)
                            { grmcd.wsUrl = c.wsUrl; }
                            else
                            { grmcd.wsUrl = c.TestwsUrl; }
                            r = grmcd.GetResponse();
                            if (r == 0)
                            {
                                _RES = grmcd.RES;
                                _OrigRES = grmcd.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = grmcd.OrigRes;
                                log.ExceptionDetails("1150-DCSGlobal.EDI.Comunications.EDI.GAMCD.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;

                    case "ALMCD":
                        using (AlbamaMedicaidEx albama = new AlbamaMedicaidEx())
                        {

                            albama.VendorName = VendorName;
                            albama.PayorCode = PayorCode;
                            albama.ConnectionString = _ConnectionString;
                            albama.wsUrl = c.wsUrl;

                            albama.UserName = c.username;
                            albama.Passwd = c.password;

                            albama.REQ = RequestContent;

                            albama.TaskID = _TaskID;
                            albama.EBRID = _EBRID;
                            albama.WaitTime = _WaitTime;
                            albama.BHT03 = B_REQ;
                            albama.VendorRetrys = _VendorRetrys;

                            albama.PayloadType = c.PayloadType;
                            albama.ProcessingMode = c.ProcessingMode;
                            albama.PayloadID = B_REQ; //  c.PayloadID;
                            albama.TimeStamp = c.TimeStamp;
                            albama.SenderID = c.SenderID;
                            albama.ReceiverID = c.ReceiverID;
                            albama.CORERuleVersion = c.CORERuleVersion;
                            albama.SendTimeout = c.SendTimeout;
                            albama.ReceiveTimeout = c.ReceiveTimeout;
                            albama.VMetricsLogging = c.VMetricsLogging;

                            if (!_isTest)
                            { albama.wsUrl = c.wsUrl; }
                            else
                            { albama.wsUrl = c.TestwsUrl; }

                            r = albama.GetResponse();
                            if (r == 0)
                            {
                                _RES = albama.RES;
                                _OrigRES = albama.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = albama.OrigRes;
                                log.ExceptionDetails("1160-DCSGlobal.EDI.Comunications.EDI.ALMCD.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;

                    case "WVMCD":
                        using (WestVirginiaMedicaidEx wvmcd = new WestVirginiaMedicaidEx())
                        {

                            wvmcd.VendorName = VendorName;
                            wvmcd.PayorCode = PayorCode;
                            wvmcd.ConnectionString = _ConnectionString;
                            wvmcd.wsUrl = c.wsUrl;

                            wvmcd.UserName = c.username;
                            wvmcd.Passwd = c.password;

                            wvmcd.TaskID = _TaskID;
                            wvmcd.EBRID = _EBRID;
                            wvmcd.WaitTime = _WaitTime;
                            wvmcd.BHT03 = B_REQ;
                            wvmcd.VendorRetrys = _VendorRetrys;

                            wvmcd.REQ = RequestContent;

                            wvmcd.PayloadType = c.PayloadType;
                            wvmcd.ProcessingMode = c.ProcessingMode;
                            wvmcd.PayloadID = B_REQ; //  c.PayloadID;
                            wvmcd.TimeStamp = c.TimeStamp;
                            wvmcd.SenderID = c.SenderID;
                            wvmcd.ReceiverID = c.ReceiverID;
                            wvmcd.CORERuleVersion = c.CORERuleVersion;
                            wvmcd.SendTimeout = c.SendTimeout;
                            wvmcd.ReceiveTimeout = c.ReceiveTimeout;
                            wvmcd.VMetricsLogging = c.VMetricsLogging;

                            if (!_isTest)
                            { wvmcd.wsUrl = c.wsUrl; }
                            else
                            { wvmcd.wsUrl = c.TestwsUrl; }

                            r = wvmcd.GetResponse();
                            if (r == 0)
                            {
                                _RES = wvmcd.RES;
                                _OrigRES = wvmcd.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = wvmcd.OrigRes;
                                log.ExceptionDetails("1165-DCSGlobal.EDI.Comunications.EDI.WVMCD.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;

                    case "CTMCD":
                        using (ConnecticutMedicaidEx ctmcd = new ConnecticutMedicaidEx())
                        {

                            ctmcd.VendorName = VendorName;
                            ctmcd.PayorCode = PayorCode;
                            ctmcd.ConnectionString = _ConnectionString;
                            ctmcd.wsUrl = c.wsUrl;

                            ctmcd.UserName = c.username;
                            ctmcd.Passwd = c.password;

                            ctmcd.REQ = RequestContent;

                            ctmcd.TaskID = _TaskID;
                            ctmcd.EBRID = _EBRID;
                            ctmcd.WaitTime = _WaitTime;
                            ctmcd.BHT03 = B_REQ;
                            ctmcd.VendorRetrys = _VendorRetrys;


                            ctmcd.PayloadType = c.PayloadType;
                            ctmcd.ProcessingMode = c.ProcessingMode;
                            ctmcd.PayloadID = B_REQ; //  c.PayloadID;
                            ctmcd.TimeStamp = c.TimeStamp;
                            ctmcd.SenderID = c.SenderID;
                            ctmcd.ReceiverID = c.ReceiverID;
                            ctmcd.CORERuleVersion = c.CORERuleVersion;
                            ctmcd.SendTimeout = c.SendTimeout;
                            ctmcd.ReceiveTimeout = c.ReceiveTimeout;
                            ctmcd.VMetricsLogging = c.VMetricsLogging;

                            if (!_isTest)
                            { ctmcd.wsUrl = c.wsUrl; }
                            else
                            { ctmcd.wsUrl = c.TestwsUrl; }

                            r = ctmcd.GetResponse();
                            if (r == 0)
                            {
                                _RES = ctmcd.RES;
                                _OrigRES = ctmcd.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = ctmcd.OrigRes;
                                log.ExceptionDetails("1165-DCSGlobal.EDI.Comunications.EDI.CTMCD.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;
                    case "SCMCD_DCSWS":
                        using (SCMCDDCSWebService dcsscmcd = new SCMCDDCSWebService())
                        {

                            dcsscmcd.VendorName = VendorName;
                            dcsscmcd.PayorCode = PayorCode;
                            dcsscmcd.ConnectionString = _ConnectionString;
                            dcsscmcd.wsUrl = c.wsUrl;

                            dcsscmcd.UserName = c.username;
                            dcsscmcd.login = c.login;
                            dcsscmcd.password = c.password;

                            dcsscmcd.REQ = RequestContent;

                            dcsscmcd.TaskID = _TaskID;
                            dcsscmcd.EBRID = _EBRID;
                            dcsscmcd.WaitTime = _WaitTime;
                            dcsscmcd.BHT03 = B_REQ;
                            dcsscmcd.VendorRetrys = _VendorRetrys;


                            dcsscmcd.PayloadType = c.PayloadType;
                            dcsscmcd.ProcessingMode = c.ProcessingMode;
                            dcsscmcd.PayloadID = B_REQ; //  c.PayloadID;
                            dcsscmcd.TimeStamp = c.TimeStamp;
                            dcsscmcd.SenderID = c.SenderID;
                            dcsscmcd.ReceiverID = c.ReceiverID;
                            dcsscmcd.CORERuleVersion = c.CORERuleVersion;
                            dcsscmcd.SendTimeout = c.ServiceTimeOut;
                            dcsscmcd.ReceiveTimeout = c.ReceiveTimeout;
                            dcsscmcd.VMetricsLogging = c.VMetricsLogging;



                            if (!_isTest)
                            { dcsscmcd.wsUrl = c.wsUrl; }
                            else
                            { dcsscmcd.wsUrl = c.TestwsUrl; }

                            r = dcsscmcd.GetResponse();
                            if (r == 0)
                            {
                                _RES = dcsscmcd.RES;
                                _OrigRES = dcsscmcd.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = dcsscmcd.OrigRes;
                                log.ExceptionDetails("1165-DCSGlobal.EDI.Comunications.EDI.HARVARD.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;
                         

                    case "SCMCD":
                        using (SouthCarolinaMedicaid scmcd = new SouthCarolinaMedicaid())
                        {

                            scmcd.VendorName = VendorName;
                            scmcd.PayorCode = PayorCode;
                            scmcd.ConnectionString = _ConnectionString;
                            scmcd.wsUrl = c.wsUrl;

                            scmcd.UserName = c.username;
                            scmcd.Passwd = c.password;

                            scmcd.REQ = RequestContent;

                            scmcd.TaskID = _TaskID;
                            scmcd.EBRID = _EBRID;
                            scmcd.WaitTime = _WaitTime;
                            scmcd.BHT03 = B_REQ;
                            scmcd.VendorRetrys = _VendorRetrys;


                            scmcd.PayloadType = c.PayloadType;
                            scmcd.ProcessingMode = c.ProcessingMode;
                            scmcd.PayloadID = B_REQ; //  c.PayloadID;
                            scmcd.TimeStamp = c.TimeStamp;
                            scmcd.SenderID = c.SenderID;
                            scmcd.ReceiverID = c.ReceiverID;
                            scmcd.CORERuleVersion = c.CORERuleVersion;
                            scmcd.SendTimeout = c.SendTimeout;
                            scmcd.ReceiveTimeout = c.ReceiveTimeout;
                            scmcd.VMetricsLogging = c.VMetricsLogging;

                            if (!_isTest)
                            { scmcd.wsUrl = c.wsUrl; }
                            else
                            { scmcd.wsUrl = c.TestwsUrl; }

                            r = scmcd.GetResponse();
                            if (r == 0)
                            {
                                _RES = scmcd.RES;
                                _OrigRES = scmcd.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = scmcd.OrigRes;
                                log.ExceptionDetails("1165-DCSGlobal.EDI.Comunications.EDI.SCMCD.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;

                    case "BCNC":
                        using (BCBSNorthCarolinaMedicaid bcnc = new BCBSNorthCarolinaMedicaid())
                        {

                            bcnc.VendorName = VendorName;
                            bcnc.PayorCode = PayorCode;
                            bcnc.ConnectionString = _ConnectionString;
                            bcnc.wsUrl = c.wsUrl;

                            bcnc.UserName = c.username;
                            bcnc.Passwd = c.password;

                            bcnc.REQ = RequestContent;

                            bcnc.TaskID = _TaskID;
                            bcnc.EBRID = _EBRID;
                            bcnc.WaitTime = _WaitTime;
                            bcnc.BHT03 = B_REQ;
                            bcnc.VendorRetrys = _VendorRetrys;


                            bcnc.PayloadType = c.PayloadType;
                            bcnc.ProcessingMode = c.ProcessingMode;
                            bcnc.PayloadID = B_REQ; //  c.PayloadID;
                            bcnc.TimeStamp = c.TimeStamp;
                            bcnc.SenderID = c.SenderID;
                            bcnc.ReceiverID = c.ReceiverID;
                            bcnc.CORERuleVersion = c.CORERuleVersion;
                            bcnc.SendTimeout = c.SendTimeout;
                            bcnc.ReceiveTimeout = c.ReceiveTimeout;
                            bcnc.VMetricsLogging = c.VMetricsLogging;

                            if (!_isTest)
                            { bcnc.wsUrl = c.wsUrl; }
                            else
                            { bcnc.wsUrl = c.TestwsUrl; }

                            r = bcnc.GetResponse();
                            if (r == 0)
                            {
                                _RES = bcnc.RES;
                                _OrigRES = bcnc.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = bcnc.OrigRes;
                                log.ExceptionDetails("1165-DCSGlobal.EDI.Comunications.EDI.BCNC.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;

                    case "WAMCD":
                        using (WashingtonMedicaid wamcd = new WashingtonMedicaid())
                        {

                            wamcd.VendorName = VendorName;
                            wamcd.PayorCode = PayorCode;
                            wamcd.ConnectionString = _ConnectionString;
                            wamcd.wsUrl = c.wsUrl;

                            wamcd.UserName = c.username;
                            wamcd.Passwd = c.password;

                            wamcd.REQ = RequestContent;

                            wamcd.TaskID = _TaskID;
                            wamcd.EBRID = _EBRID;
                            wamcd.WaitTime = _WaitTime;
                            wamcd.BHT03 = B_REQ;
                            wamcd.VendorRetrys = _VendorRetrys;


                            wamcd.PayloadType = c.PayloadType;
                            wamcd.ProcessingMode = c.ProcessingMode;
                            wamcd.PayloadID = B_REQ; //  c.PayloadID;
                            wamcd.TimeStamp = c.TimeStamp;
                            wamcd.SenderID = c.SenderID;
                            wamcd.ReceiverID = c.ReceiverID;
                            wamcd.CORERuleVersion = c.CORERuleVersion;
                            wamcd.SendTimeout = c.ServiceTimeOut;
                            wamcd.ReceiveTimeout = c.ReceiveTimeout;
                            wamcd.VMetricsLogging = c.VMetricsLogging;
                            
                            

                            if (!_isTest)
                            { wamcd.wsUrl = c.wsUrl; }
                            else
                            { wamcd.wsUrl = c.TestwsUrl; }

                            r = wamcd.GetResponse();
                            if (r == 0)
                            {
                                _RES = wamcd.RES;
                                _OrigRES = wamcd.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = wamcd.OrigRes;
                                log.ExceptionDetails("1165-DCSGlobal.EDI.Comunications.EDI.WAMCD.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;

                    case "MISSOULA":
                        using (MissoulaMedicaid missoula = new MissoulaMedicaid())
                        {

                            missoula.VendorName = VendorName;
                            missoula.PayorCode = PayorCode;
                            missoula.ConnectionString = _ConnectionString;
                            missoula.wsUrl = c.wsUrl;

                            missoula.UserName = c.username;
                            missoula.Passwd = c.password;

                            missoula.REQ = RequestContent;

                            missoula.TaskID = _TaskID;
                            missoula.EBRID = _EBRID;
                            missoula.WaitTime = _WaitTime;
                            missoula.BHT03 = B_REQ;
                            missoula.VendorRetrys = _VendorRetrys;


                            missoula.PayloadType = c.PayloadType;
                            missoula.ProcessingMode = c.ProcessingMode;
                            missoula.PayloadID = B_REQ; //  c.PayloadID;
                            missoula.TimeStamp = c.TimeStamp;
                            missoula.SenderID = c.SenderID;
                            missoula.ReceiverID = c.ReceiverID;
                            missoula.CORERuleVersion = c.CORERuleVersion;
                            missoula.SendTimeout = c.ServiceTimeOut;
                            missoula.ReceiveTimeout = c.ReceiveTimeout;
                            missoula.VMetricsLogging = c.VMetricsLogging;



                            if (!_isTest)
                            { missoula.wsUrl = c.wsUrl; }
                            else
                            { missoula.wsUrl = c.TestwsUrl; }

                            r = missoula.GetResponse();
                            if (r == 0)
                            {
                                _RES = missoula.RES;
                                _OrigRES = missoula.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = missoula.OrigRes;
                                log.ExceptionDetails("1165-DCSGlobal.EDI.Comunications.EDI.MISSOULA.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;

                    case "NCMCD":
                        using (NorthCarolinaMedicaid ncmcd = new NorthCarolinaMedicaid())
                        {

                            ncmcd.VendorName = VendorName;
                            ncmcd.PayorCode = PayorCode;
                            ncmcd.ConnectionString = _ConnectionString;
                            ncmcd.wsUrl = c.wsUrl;

                            ncmcd.UserName = c.username;
                            ncmcd.Passwd = c.password;

                            ncmcd.REQ = RequestContent;

                            ncmcd.TaskID = _TaskID;
                            ncmcd.EBRID = _EBRID;
                            ncmcd.WaitTime = _WaitTime;
                            ncmcd.BHT03 = B_REQ;
                            ncmcd.VendorRetrys = _VendorRetrys;


                            ncmcd.PayloadType = c.PayloadType;
                            ncmcd.ProcessingMode = c.ProcessingMode;
                            ncmcd.PayloadID = B_REQ; //  c.PayloadID;
                            ncmcd.TimeStamp = c.TimeStamp;
                            ncmcd.SenderID = c.SenderID;
                            ncmcd.ReceiverID = c.ReceiverID;
                            ncmcd.CORERuleVersion = c.CORERuleVersion;
                            ncmcd.SendTimeout = c.ServiceTimeOut;
                            ncmcd.ReceiveTimeout = c.ReceiveTimeout;
                            ncmcd.VMetricsLogging = c.VMetricsLogging;



                            if (!_isTest)
                            { ncmcd.wsUrl = c.wsUrl; }
                            else
                            { ncmcd.wsUrl = c.TestwsUrl; }

                            r = ncmcd.GetResponse();
                            if (r == 0)
                            {
                                _RES = ncmcd.RES;
                                _OrigRES = ncmcd.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = ncmcd.OrigRes;
                                log.ExceptionDetails("1165-DCSGlobal.EDI.Comunications.EDI.NCMCD.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;

                    case "DEMCD":
                        using (DelawareMedicaid demcd = new DelawareMedicaid())
                        {

                            demcd.VendorName = VendorName;
                            demcd.PayorCode = PayorCode;
                            demcd.ConnectionString = _ConnectionString;
                            demcd.wsUrl = c.wsUrl;

                            demcd.UserName = c.username;
                            demcd.Passwd = c.password;

                            demcd.REQ = RequestContent;

                            demcd.TaskID = _TaskID;
                            demcd.EBRID = _EBRID;
                            demcd.WaitTime = _WaitTime;
                            demcd.BHT03 = B_REQ;
                            demcd.VendorRetrys = _VendorRetrys;


                            demcd.PayloadType = c.PayloadType;
                            demcd.ProcessingMode = c.ProcessingMode;
                            demcd.PayloadID = B_REQ; //  c.PayloadID;
                            demcd.TimeStamp = c.TimeStamp;
                            demcd.SenderID = c.SenderID;
                            demcd.ReceiverID = c.ReceiverID;
                            demcd.CORERuleVersion = c.CORERuleVersion;
                            demcd.SendTimeout = c.ServiceTimeOut;
                            demcd.ReceiveTimeout = c.ReceiveTimeout;
                            demcd.VMetricsLogging = c.VMetricsLogging;



                            if (!_isTest)
                            { demcd.wsUrl = c.wsUrl; }
                            else
                            { demcd.wsUrl = c.TestwsUrl; }

                            r = demcd.GetResponse();
                            if (r == 0)
                            {
                                _RES = demcd.RES;
                                _OrigRES = demcd.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = demcd.OrigRes;
                                log.ExceptionDetails("1165-DCSGlobal.EDI.Comunications.EDI.DEMCD.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;

                    case "BCAL":
                        using (BCBSAlbama bcal = new BCBSAlbama())
                        {

                            bcal.VendorName = VendorName;
                            bcal.PayorCode = PayorCode;
                            bcal.ConnectionString = _ConnectionString;
                            bcal.wsUrl = c.wsUrl;

                            bcal.UserName = c.username;
                            bcal.Passwd = c.password;

                            bcal.REQ = RequestContent;

                            bcal.TaskID = _TaskID;
                            bcal.EBRID = _EBRID;
                            bcal.WaitTime = _WaitTime;
                            bcal.BHT03 = B_REQ;
                            bcal.VendorRetrys = _VendorRetrys;


                            bcal.PayloadType = c.PayloadType;
                            bcal.ProcessingMode = c.ProcessingMode;
                            bcal.PayloadID = B_REQ; //  c.PayloadID;
                            bcal.TimeStamp = c.TimeStamp;
                            bcal.SenderID = c.SenderID;
                            bcal.ReceiverID = c.ReceiverID;
                            bcal.CORERuleVersion = c.CORERuleVersion;
                            bcal.SendTimeout = c.ServiceTimeOut;
                            bcal.ReceiveTimeout = c.ReceiveTimeout;
                            bcal.VMetricsLogging = c.VMetricsLogging;



                            if (!_isTest)
                            { bcal.wsUrl = c.wsUrl; }
                            else
                            { bcal.wsUrl = c.TestwsUrl; }

                            r = bcal.GetResponse();
                            if (r == 0)
                            {
                                _RES = bcal.RES;
                                _OrigRES = bcal.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = bcal.OrigRes;
                                log.ExceptionDetails("1165-DCSGlobal.EDI.Comunications.EDI.BCAL.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;

                    case "ANTHEM":
                        using (Anthem antm = new Anthem())
                        {

                            antm.VendorName = VendorName;
                            antm.PayorCode = PayorCode;
                            antm.ConnectionString = _ConnectionString;
                            antm.wsUrl = c.wsUrl;

                            antm.UserName = c.username;
                            antm.Passwd = c.password;

                            antm.REQ = RequestContent;

                            antm.TaskID = _TaskID;
                            antm.EBRID = _EBRID;
                            antm.WaitTime = _WaitTime;
                            antm.BHT03 = B_REQ;
                            antm.VendorRetrys = _VendorRetrys;


                            antm.PayloadType = c.PayloadType;
                            antm.ProcessingMode = c.ProcessingMode;
                            antm.PayloadID = B_REQ; //  c.PayloadID;
                            antm.TimeStamp = c.TimeStamp;
                            antm.SenderID = c.SenderID;
                            antm.ReceiverID = c.ReceiverID;
                            antm.CORERuleVersion = c.CORERuleVersion;
                            antm.SendTimeout = c.ServiceTimeOut;
                            antm.ReceiveTimeout = c.ReceiveTimeout;
                            antm.VMetricsLogging = c.VMetricsLogging;



                            if (!_isTest)
                            { antm.wsUrl = c.wsUrl; }
                            else
                            { antm.wsUrl = c.TestwsUrl; }

                            r = antm.GetResponse();
                            if (r == 0)
                            {
                                _RES = antm.RES;
                                _OrigRES = antm.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = antm.OrigRes;
                                log.ExceptionDetails("1165-DCSGlobal.EDI.Comunications.EDI.antm.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;

                    case "BCTN":
                        using (TennesseeMedicaid bcbst = new TennesseeMedicaid())
                        {

                            bcbst.VendorName = VendorName;
                            bcbst.PayorCode = PayorCode;
                            bcbst.ConnectionString = _ConnectionString;
                            bcbst.wsUrl = c.wsUrl;

                            bcbst.UserName = c.username;
                            bcbst.Passwd = c.password;

                            bcbst.REQ = RequestContent;

                            bcbst.TaskID = _TaskID;
                            bcbst.EBRID = _EBRID;
                            bcbst.WaitTime = _WaitTime;
                            bcbst.BHT03 = B_REQ;
                            bcbst.VendorRetrys = _VendorRetrys;


                            bcbst.PayloadType = c.PayloadType;
                            bcbst.ProcessingMode = c.ProcessingMode;
                            bcbst.PayloadID = B_REQ; //  c.PayloadID;
                            bcbst.TimeStamp = c.TimeStamp;
                            bcbst.SenderID = c.SenderID;
                            bcbst.ReceiverID = c.ReceiverID;
                            bcbst.CORERuleVersion = c.CORERuleVersion;
                            bcbst.SendTimeout = c.ServiceTimeOut;
                            bcbst.ReceiveTimeout = c.ReceiveTimeout;
                            bcbst.VMetricsLogging = c.VMetricsLogging;



                            if (!_isTest)
                            { bcbst.wsUrl = c.wsUrl; }
                            else
                            { bcbst.wsUrl = c.TestwsUrl; }

                            r = bcbst.GetResponse();
                            if (r == 0)
                            {
                                _RES = bcbst.RES;
                                _OrigRES = bcbst.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = bcbst.OrigRes;
                                log.ExceptionDetails("1165-DCSGlobal.EDI.Comunications.EDI.BCTN.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;
                    case "BCDE":
                        using (HighMark hmark = new HighMark())
                        {

                            hmark.VendorName = VendorName;
                            hmark.PayorCode = PayorCode;
                            hmark.ConnectionString = _ConnectionString;
                            hmark.wsUrl = c.wsUrl;

                            hmark.UserName = c.username;
                            hmark.Passwd = c.password;

                            hmark.REQ = RequestContent;

                            hmark.TaskID = _TaskID;
                            hmark.EBRID = _EBRID;
                            hmark.WaitTime = _WaitTime;
                            hmark.BHT03 = B_REQ;
                            hmark.VendorRetrys = _VendorRetrys;


                            hmark.PayloadType = c.PayloadType;
                            hmark.ProcessingMode = c.ProcessingMode;
                            hmark.PayloadID = B_REQ; //  c.PayloadID;
                            hmark.TimeStamp = c.TimeStamp;
                            hmark.SenderID = c.SenderID;
                            hmark.ReceiverID = c.ReceiverID;
                            hmark.CORERuleVersion = c.CORERuleVersion;
                            hmark.SendTimeout = c.ServiceTimeOut;
                            hmark.ReceiveTimeout = c.ReceiveTimeout;
                            hmark.VMetricsLogging = c.VMetricsLogging;



                            if (!_isTest)
                            { hmark.wsUrl = c.wsUrl; }
                            else
                            { hmark.wsUrl = c.TestwsUrl; }

                            r = hmark.GetResponse();
                            if (r == 0)
                            {
                                _RES = hmark.RES;
                                _OrigRES = hmark.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = hmark.OrigRes;
                                log.ExceptionDetails("1165-DCSGlobal.EDI.Comunications.EDI.HIMARK.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;
                    case "MOMCD":
                        using (MissouriMedicaid momcd = new MissouriMedicaid())
                        {

                            momcd.VendorName = VendorName;
                            momcd.PayorCode = PayorCode;
                            momcd.ConnectionString = _ConnectionString;
                            momcd.wsUrl = c.wsUrl;

                            momcd.UserName = c.username;
                            momcd.Passwd = c.password;

                            momcd.REQ = RequestContent;

                            momcd.TaskID = _TaskID;
                            momcd.EBRID = _EBRID;
                            momcd.WaitTime = _WaitTime;
                            momcd.BHT03 = B_REQ;
                            momcd.VendorRetrys = _VendorRetrys;


                            momcd.PayloadType = c.PayloadType;
                            momcd.ProcessingMode = c.ProcessingMode;
                            momcd.PayloadID = B_REQ; //  c.PayloadID;
                            momcd.TimeStamp = c.TimeStamp;
                            momcd.SenderID = c.SenderID;
                            momcd.ReceiverID = c.ReceiverID;
                            momcd.CORERuleVersion = c.CORERuleVersion;
                            momcd.SendTimeout = c.ServiceTimeOut;
                            momcd.ReceiveTimeout = c.ReceiveTimeout;
                            momcd.VMetricsLogging = c.VMetricsLogging;



                            if (!_isTest)
                            { momcd.wsUrl = c.wsUrl; }
                            else
                            { momcd.wsUrl = c.TestwsUrl; }

                            r = momcd.GetResponse();
                            if (r == 0)
                            {
                                _RES = momcd.RES;
                                _OrigRES = momcd.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = momcd.OrigRes;
                                log.ExceptionDetails("1165-DCSGlobal.EDI.Comunications.EDI.MOMCD.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;

                    case "CENTENE":
                        using (Centene cent = new Centene())
                        {

                            cent.VendorName = VendorName;
                            cent.PayorCode = PayorCode;
                            cent.ConnectionString = _ConnectionString;
                            cent.wsUrl = c.wsUrl;

                            cent.UserName = c.username;
                            cent.Passwd = c.password;

                            cent.REQ = RequestContent;

                            cent.TaskID = _TaskID;
                            cent.EBRID = _EBRID;
                            cent.WaitTime = _WaitTime;
                            cent.BHT03 = B_REQ;
                            cent.VendorRetrys = _VendorRetrys;


                            cent.PayloadType = c.PayloadType;
                            cent.ProcessingMode = c.ProcessingMode;
                            cent.PayloadID = B_REQ; //  c.PayloadID;
                            cent.TimeStamp = c.TimeStamp;
                            cent.SenderID = c.SenderID;
                            cent.ReceiverID = c.ReceiverID;
                            cent.CORERuleVersion = c.CORERuleVersion;
                            cent.SendTimeout = c.ServiceTimeOut;
                            cent.ReceiveTimeout = c.ReceiveTimeout;
                            cent.VMetricsLogging = c.VMetricsLogging;



                            if (!_isTest)
                            { cent.wsUrl = c.wsUrl; }
                            else
                            { cent.wsUrl = c.TestwsUrl; }

                            r = cent.GetResponse();
                            if (r == 0)
                            {
                                _RES = cent.RES;
                                _OrigRES = cent.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = cent.OrigRes;
                                log.ExceptionDetails("1165-DCSGlobal.EDI.Comunications.EDI.CENTENE.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;
                    case "HARVARD_DIRECT":
                        using (HarvardPilgrim harvard = new HarvardPilgrim())
                        {

                            harvard.VendorName = VendorName;
                            harvard.PayorCode = PayorCode;
                            harvard.ConnectionString = _ConnectionString;
                            harvard.wsUrl = c.wsUrl;

                            harvard.UserName = c.username;
                            harvard.Passwd = c.password;

                            harvard.REQ = RequestContent;

                            harvard.TaskID = _TaskID;
                            harvard.EBRID = _EBRID;
                            harvard.WaitTime = _WaitTime;
                            harvard.BHT03 = B_REQ;
                            harvard.VendorRetrys = _VendorRetrys;


                            harvard.PayloadType = c.PayloadType;
                            harvard.ProcessingMode = c.ProcessingMode;
                            harvard.PayloadID = B_REQ; //  c.PayloadID;
                            harvard.TimeStamp = c.TimeStamp;
                            harvard.SenderID = c.SenderID;
                            harvard.ReceiverID = c.ReceiverID;
                            harvard.CORERuleVersion = c.CORERuleVersion;
                            harvard.SendTimeout = c.ServiceTimeOut;
                            harvard.ReceiveTimeout = c.ReceiveTimeout;
                            harvard.VMetricsLogging = c.VMetricsLogging;



                            if (!_isTest)
                            { harvard.wsUrl = c.wsUrl; }
                            else
                            { harvard.wsUrl = c.TestwsUrl; }

                            r = harvard.GetResponse();
                            if (r == 0)
                            {
                                _RES = harvard.RES;
                                _OrigRES = harvard.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = harvard.OrigRes;
                                log.ExceptionDetails("1165-DCSGlobal.EDI.Comunications.EDI.HARVARDDIRECT.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;
                    case "HARVARD":
                        using (HarvardPilgrimDCSWebService dcsharvard = new HarvardPilgrimDCSWebService())
                        {

                            dcsharvard.VendorName = VendorName;
                            dcsharvard.PayorCode = PayorCode;
                            dcsharvard.ConnectionString = _ConnectionString;
                            dcsharvard.wsUrl = c.wsUrl;

                            dcsharvard.UserName = c.username;
                            dcsharvard.login = c.login;
                            dcsharvard.password = c.password;

                            dcsharvard.REQ = RequestContent;

                            dcsharvard.TaskID = _TaskID;
                            dcsharvard.EBRID = _EBRID;
                            dcsharvard.WaitTime = _WaitTime;
                            dcsharvard.BHT03 = B_REQ;
                            dcsharvard.VendorRetrys = _VendorRetrys;


                            dcsharvard.PayloadType = c.PayloadType;
                            dcsharvard.ProcessingMode = c.ProcessingMode;
                            dcsharvard.PayloadID = B_REQ; //  c.PayloadID;
                            dcsharvard.TimeStamp = c.TimeStamp;
                            dcsharvard.SenderID = c.SenderID;
                            dcsharvard.ReceiverID = c.ReceiverID;
                            dcsharvard.CORERuleVersion = c.CORERuleVersion;
                            dcsharvard.SendTimeout = c.ServiceTimeOut;
                            dcsharvard.ReceiveTimeout = c.ReceiveTimeout;
                            dcsharvard.VMetricsLogging = c.VMetricsLogging;



                            if (!_isTest)
                            { dcsharvard.wsUrl = c.wsUrl; }
                            else
                            { dcsharvard.wsUrl = c.TestwsUrl; }

                            r = dcsharvard.GetResponse();
                            if (r == 0)
                            {
                                _RES = dcsharvard.RES;
                                _OrigRES = dcsharvard.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = dcsharvard.OrigRes;
                                log.ExceptionDetails("1165-DCSGlobal.EDI.Comunications.EDI.HARVARD.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;
                    case "BCRI":
                        using (BcRhodeIsland bcri = new BcRhodeIsland())
                        {

                            bcri.VendorName = VendorName;
                            bcri.PayorCode = PayorCode;
                            bcri.ConnectionString = _ConnectionString;
                            bcri.wsUrl = c.wsUrl;

                            bcri.UserName = c.username;
                            bcri.Passwd = c.password;

                            bcri.REQ = RequestContent;

                            bcri.TaskID = _TaskID;
                            bcri.EBRID = _EBRID;
                            bcri.WaitTime = _WaitTime;
                            bcri.BHT03 = B_REQ;
                            bcri.VendorRetrys = _VendorRetrys;


                            bcri.PayloadType = c.PayloadType;
                            bcri.ProcessingMode = c.ProcessingMode;
                            bcri.PayloadID = B_REQ; //  c.PayloadID;
                            bcri.TimeStamp = c.TimeStamp;
                            bcri.SenderID = c.SenderID;
                            bcri.ReceiverID = c.ReceiverID;
                            bcri.CORERuleVersion = c.CORERuleVersion;
                            bcri.SendTimeout = c.ServiceTimeOut;
                            bcri.ReceiveTimeout = c.ReceiveTimeout;
                            bcri.VMetricsLogging = c.VMetricsLogging;



                            if (!_isTest)
                            { bcri.wsUrl = c.wsUrl; }
                            else
                            { bcri.wsUrl = c.TestwsUrl; }

                            r = bcri.GetResponse();
                            if (r == 0)
                            {
                                _RES = bcri.RES;
                                _OrigRES = bcri.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = bcri.OrigRes;
                                log.ExceptionDetails("1165-DCSGlobal.EDI.Comunications.EDI.BCRI.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;
                    case "BCWV":
                        using (BCWestVerginia bcwv = new BCWestVerginia())
                        {

                            bcwv.VendorName = VendorName;
                            bcwv.PayorCode = PayorCode;
                            bcwv.ConnectionString = _ConnectionString;
                            bcwv.wsUrl = c.wsUrl;

                            bcwv.UserName = c.username;
                            //bcwv.login = c.login;
                            bcwv.Passwd  = c.password;

                            bcwv.REQ = RequestContent;

                            bcwv.TaskID = _TaskID;
                            bcwv.EBRID = _EBRID;
                            bcwv.WaitTime = _WaitTime;
                            bcwv.BHT03 = B_REQ;
                            bcwv.VendorRetrys = _VendorRetrys;


                            bcwv.PayloadType = c.PayloadType;
                            bcwv.ProcessingMode = c.ProcessingMode;
                            bcwv.PayloadID = B_REQ; //  c.PayloadID;
                            bcwv.TimeStamp = c.TimeStamp;
                            bcwv.SenderID = c.SenderID;
                            bcwv.ReceiverID = c.ReceiverID;
                            bcwv.CORERuleVersion = c.CORERuleVersion;
                            bcwv.SendTimeout = c.ServiceTimeOut;
                            bcwv.ReceiveTimeout = c.ReceiveTimeout;
                            bcwv.VMetricsLogging = c.VMetricsLogging;



                            if (!_isTest)
                            { bcwv.wsUrl = c.wsUrl; }
                            else
                            { bcwv.wsUrl = c.TestwsUrl; }

                            r = bcwv.GetResponse();
                            if (r == 0)
                            {
                                _RES = bcwv.RES;
                                _OrigRES = bcwv.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = bcwv.OrigRes;
                                log.ExceptionDetails("1165-DCSGlobal.EDI.Comunications.EDI.BCWV.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;
                    case "BCWVDCS":
                        using (BCWestVerginiaDCSWebService bcwvdcs = new BCWestVerginiaDCSWebService())
                        {

                            bcwvdcs.VendorName = VendorName;
                            bcwvdcs.PayorCode = PayorCode;
                            bcwvdcs.ConnectionString = _ConnectionString;
                            bcwvdcs.wsUrl = c.wsUrl;

                            bcwvdcs.UserName = c.username;
                            bcwvdcs.login = c.login;
                            bcwvdcs.password = c.password;

                            bcwvdcs.REQ = RequestContent;

                            bcwvdcs.TaskID = _TaskID;
                            bcwvdcs.EBRID = _EBRID;
                            bcwvdcs.WaitTime = _WaitTime;
                            bcwvdcs.BHT03 = B_REQ;
                            bcwvdcs.VendorRetrys = _VendorRetrys;


                            bcwvdcs.PayloadType = c.PayloadType;
                            bcwvdcs.ProcessingMode = c.ProcessingMode;
                            bcwvdcs.PayloadID = B_REQ; //  c.PayloadID;
                            bcwvdcs.TimeStamp = c.TimeStamp;
                            bcwvdcs.SenderID = c.SenderID;
                            bcwvdcs.ReceiverID = c.ReceiverID;
                            bcwvdcs.CORERuleVersion = c.CORERuleVersion;
                            bcwvdcs.SendTimeout = c.ServiceTimeOut;
                            bcwvdcs.ReceiveTimeout = c.ReceiveTimeout;
                            bcwvdcs.VMetricsLogging = c.VMetricsLogging;



                            if (!_isTest)
                            { bcwvdcs.wsUrl = c.wsUrl; }
                            else
                            { bcwvdcs.wsUrl = c.TestwsUrl; }

                            r = bcwvdcs.GetResponse();
                            if (r == 0)
                            {
                                _RES = bcwvdcs.RES;
                                _OrigRES = bcwvdcs.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = bcwvdcs.OrigRes;
                                log.ExceptionDetails("1165-DCSGlobal.EDI.Comunications.EDI.BCWVDCS.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;

                    case "WELLMARK":
                        using (Wellmark wmc = new Wellmark())
                        {

                            wmc.VendorName = VendorName;
                            wmc.PayorCode = PayorCode;
                            wmc.ConnectionString = _ConnectionString;
                            wmc.wsUrl = c.wsUrl;

                            wmc.UserName = c.username;
                            //bcwv.login = c.login;
                            wmc.Passwd = c.password;

                            wmc.REQ = RequestContent;

                            wmc.TaskID = _TaskID;
                            wmc.EBRID = _EBRID;
                            wmc.WaitTime = _WaitTime;
                            wmc.BHT03 = B_REQ;
                            wmc.VendorRetrys = _VendorRetrys;


                            wmc.PayloadType = c.PayloadType;
                            wmc.ProcessingMode = c.ProcessingMode;
                            wmc.PayloadID = B_REQ; //  c.PayloadID;
                            wmc.TimeStamp = c.TimeStamp;
                            wmc.SenderID = c.SenderID;
                            wmc.ReceiverID = c.ReceiverID;
                            wmc.CORERuleVersion = c.CORERuleVersion;
                            wmc.SendTimeout = c.ServiceTimeOut;
                            wmc.ReceiveTimeout = c.ReceiveTimeout;
                            wmc.VMetricsLogging = c.VMetricsLogging;



                            if (!_isTest)
                            { wmc.wsUrl = c.wsUrl; }
                            else
                            { wmc.wsUrl = c.TestwsUrl; }

                            r = wmc.GetResponse();
                            if (r == 0)
                            {
                                _RES = wmc.RES;
                                _OrigRES = wmc.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = wmc.OrigRes;
                                log.ExceptionDetails("1165-DCSGlobal.EDI.Comunications.EDI.Wellmark.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;
                    case "KYMCD":
                        using (KentuckyMedicaid kymcd = new KentuckyMedicaid())
                        {

                            kymcd.VendorName = VendorName;
                            kymcd.PayorCode = PayorCode;
                            kymcd.ConnectionString = _ConnectionString;
                            kymcd.wsUrl = c.wsUrl;

                            kymcd.UserName = c.username;
                            //bcwv.login = c.login;
                            kymcd.Passwd = c.password;

                            kymcd.REQ = RequestContent;

                            kymcd.TaskID = _TaskID;
                            kymcd.EBRID = _EBRID;
                            kymcd.WaitTime = _WaitTime;
                            kymcd.BHT03 = B_REQ;
                            kymcd.VendorRetrys = _VendorRetrys;


                            kymcd.PayloadType = c.PayloadType;
                            kymcd.ProcessingMode = c.ProcessingMode;
                            kymcd.PayloadID = B_REQ; //  c.PayloadID;
                            kymcd.TimeStamp = c.TimeStamp;
                            kymcd.SenderID = c.SenderID;
                            kymcd.ReceiverID = c.ReceiverID;
                            kymcd.CORERuleVersion = c.CORERuleVersion;
                            kymcd.SendTimeout = c.ServiceTimeOut;
                            kymcd.ReceiveTimeout = c.ReceiveTimeout;
                            kymcd.VMetricsLogging = c.VMetricsLogging;

                            if (!_isTest)
                            { kymcd.wsUrl = c.wsUrl; }
                            else
                            { kymcd.wsUrl = c.TestwsUrl; }

                            r = kymcd.GetResponse();
                            if (r == 0)
                            {
                                _RES = kymcd.RES;
                                _OrigRES = kymcd.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = kymcd.OrigRes;
                                log.ExceptionDetails("1165-DCSGlobal.EDI.Comunications.EDI.Wellmark.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;
                    case "BCNE":
                        using (BCNebraska bcnemcd = new BCNebraska())
                        {

                            bcnemcd.VendorName = VendorName;
                            bcnemcd.PayorCode = PayorCode;
                            bcnemcd.ConnectionString = _ConnectionString;
                            bcnemcd.wsUrl = c.wsUrl;

                            bcnemcd.UserName = c.username;
                            //bcwv.login = c.login;
                            bcnemcd.Passwd = c.password;

                            bcnemcd.REQ = RequestContent;

                            bcnemcd.TaskID = _TaskID;
                            bcnemcd.EBRID = _EBRID;
                            bcnemcd.WaitTime = _WaitTime;
                            bcnemcd.BHT03 = B_REQ;
                            bcnemcd.VendorRetrys = _VendorRetrys;


                            bcnemcd.PayloadType = c.PayloadType;
                            bcnemcd.ProcessingMode = c.ProcessingMode;
                            bcnemcd.PayloadID = B_REQ; //  c.PayloadID;
                            bcnemcd.TimeStamp = c.TimeStamp;
                            bcnemcd.SenderID = c.SenderID;
                            bcnemcd.ReceiverID = c.ReceiverID;
                            bcnemcd.CORERuleVersion = c.CORERuleVersion;
                            bcnemcd.SendTimeout = c.ServiceTimeOut;
                            bcnemcd.ReceiveTimeout = c.ReceiveTimeout;
                            bcnemcd.VMetricsLogging = c.VMetricsLogging;

                            if (!_isTest)
                            { bcnemcd.wsUrl = c.wsUrl; }
                            else
                            { bcnemcd.wsUrl = c.TestwsUrl; }

                            r = bcnemcd.GetResponse();
                            if (r == 0)
                            {
                                _RES = bcnemcd.RES;
                                _OrigRES = bcnemcd.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = bcnemcd.OrigRes;
                                log.ExceptionDetails("1165-DCSGlobal.EDI.Comunications.EDI.Wellmark.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;
                    case "BCPA":
                        using (PennsylvaniaMedicaid bcpa = new PennsylvaniaMedicaid())
                        {

                            bcpa.VendorName = VendorName;
                            bcpa.PayorCode = PayorCode;
                            bcpa.ConnectionString = _ConnectionString;
                            bcpa.wsUrl = c.wsUrl;

                            bcpa.UserName = c.username;
                            //bcwv.login = c.login;
                            bcpa.Passwd = c.password;

                            bcpa.REQ = RequestContent;

                            bcpa.TaskID = _TaskID;
                            bcpa.EBRID = _EBRID;
                            bcpa.WaitTime = _WaitTime;
                            bcpa.BHT03 = B_REQ;
                            bcpa.VendorRetrys = _VendorRetrys;


                            bcpa.PayloadType = c.PayloadType;
                            bcpa.ProcessingMode = c.ProcessingMode;
                            bcpa.PayloadID = B_REQ; //  c.PayloadID;
                            bcpa.TimeStamp = c.TimeStamp;
                            bcpa.SenderID = c.SenderID;
                            bcpa.ReceiverID = c.ReceiverID;
                            bcpa.CORERuleVersion = c.CORERuleVersion;
                            bcpa.SendTimeout = c.ServiceTimeOut;
                            bcpa.ReceiveTimeout = c.ReceiveTimeout;
                            bcpa.VMetricsLogging = c.VMetricsLogging;



                            if (!_isTest)
                            { bcpa.wsUrl = c.wsUrl; }
                            else
                            { bcpa.wsUrl = c.TestwsUrl; }

                            r = bcpa.GetResponse();
                            if (r == 0)
                            {
                                _RES = bcpa.RES;
                                _OrigRES = bcpa.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = bcpa.OrigRes;
                                log.ExceptionDetails("1165-DCSGlobal.EDI.Comunications.EDI.BCWV.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;
                    case "FLMCD_DCS":
                        using (FloridaDCSWebService dcsflmcd = new FloridaDCSWebService())
                        {

                            dcsflmcd.VendorName = VendorName;
                            dcsflmcd.PayorCode = PayorCode;
                            dcsflmcd.ConnectionString = _ConnectionString;
                            dcsflmcd.wsUrl = c.wsUrl;

                            dcsflmcd.UserName = c.username;
                            dcsflmcd.login = c.login;
                            dcsflmcd.password = c.password;

                            dcsflmcd.REQ = RequestContent;

                            dcsflmcd.TaskID = _TaskID;
                            dcsflmcd.EBRID = _EBRID;
                            dcsflmcd.WaitTime = _WaitTime;
                            dcsflmcd.BHT03 = B_REQ;
                            dcsflmcd.VendorRetrys = _VendorRetrys;


                            dcsflmcd.PayloadType = c.PayloadType;
                            dcsflmcd.ProcessingMode = c.ProcessingMode;
                            dcsflmcd.PayloadID = B_REQ; //  c.PayloadID;
                            dcsflmcd.TimeStamp = c.TimeStamp;
                            dcsflmcd.SenderID = c.SenderID;
                            dcsflmcd.ReceiverID = c.ReceiverID;
                            dcsflmcd.CORERuleVersion = c.CORERuleVersion;
                            dcsflmcd.SendTimeout = c.ServiceTimeOut;
                            dcsflmcd.ReceiveTimeout = c.ReceiveTimeout;
                            dcsflmcd.VMetricsLogging = c.VMetricsLogging;



                            if (!_isTest)
                            { dcsflmcd.wsUrl = c.wsUrl; }
                            else
                            { dcsflmcd.wsUrl = c.TestwsUrl; }

                            r = dcsflmcd.GetResponse();
                            if (r == 0)
                            {
                                _RES = dcsflmcd.RES;
                                _OrigRES = dcsflmcd.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = dcsflmcd.OrigRes;
                                log.ExceptionDetails("1165-DCSGlobal.EDI.Comunications.EDI.DCSFLMCD.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;
                         
                    case "FLMCD":
                        using (FloridaMedicaid fmcd = new FloridaMedicaid())
                        {

                            fmcd.VendorName = VendorName;
                            fmcd.PayorCode = PayorCode;
                            fmcd.ConnectionString = _ConnectionString;
                            fmcd.wsUrl = c.wsUrl;

                            fmcd.UserName = c.username;
                            //bcwv.login = c.login;
                            fmcd.Passwd = c.password;

                            fmcd.REQ = RequestContent;

                            fmcd.TaskID = _TaskID;
                            fmcd.EBRID = _EBRID;
                            fmcd.WaitTime = _WaitTime;
                            fmcd.BHT03 = B_REQ;
                            fmcd.VendorRetrys = _VendorRetrys;


                            fmcd.PayloadType = c.PayloadType;
                            fmcd.ProcessingMode = c.ProcessingMode;
                            fmcd.PayloadID = B_REQ; //  c.PayloadID;
                            fmcd.TimeStamp = c.TimeStamp;
                            fmcd.SenderID = c.SenderID;
                            fmcd.ReceiverID = c.ReceiverID;
                            fmcd.CORERuleVersion = c.CORERuleVersion;
                            fmcd.SendTimeout = c.ServiceTimeOut;
                            fmcd.ReceiveTimeout = c.ReceiveTimeout;
                            fmcd.VMetricsLogging = c.VMetricsLogging;



                            if (!_isTest)
                            { fmcd.wsUrl = c.wsUrl; }
                            else
                            { fmcd.wsUrl = c.TestwsUrl; }

                            r = fmcd.GetResponse();
                            if (r == 0)
                            {
                                _RES = fmcd.RES;
                                _OrigRES = fmcd.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = fmcd.OrigRes;
                                log.ExceptionDetails("1165-DCSGlobal.EDI.Comunications.EDI.BCWV.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;
                    //case "TRICARE":
                    //    using (Tricare tricare = new Tricare())
                    //    {

                    //        tricare.VendorName = VendorName;
                    //        tricare.PayorCode = PayorCode;
                    //        tricare.ConnectionString = _ConnectionString;
                    //        tricare.wsUrl = c.wsUrl;

                    //        tricare.UserName = c.username;
                    //        //bcwv.login = c.login;
                    //        tricare.Passwd = c.password;

                    //        tricare.REQ = RequestContent;

                    //        tricare.TaskID = _TaskID;
                    //        tricare.EBRID = _EBRID;
                    //        tricare.WaitTime = _WaitTime;
                    //        tricare.BHT03 = B_REQ;
                    //        tricare.VendorRetrys = _VendorRetrys;


                    //        tricare.PayloadType = c.PayloadType;
                    //        tricare.ProcessingMode = c.ProcessingMode;
                    //        tricare.PayloadID = B_REQ; //  c.PayloadID;
                    //        tricare.TimeStamp = c.TimeStamp;
                    //        tricare.SenderID = c.SenderID;
                    //        tricare.ReceiverID = c.ReceiverID;
                    //        tricare.CORERuleVersion = c.CORERuleVersion;
                    //        tricare.SendTimeout = c.ServiceTimeOut;
                    //        tricare.ReceiveTimeout = c.ReceiveTimeout;
                    //        tricare.VMetricsLogging = c.VMetricsLogging;



                    //        if (!_isTest)
                    //        { tricare.wsUrl = c.wsUrl; }
                    //        else
                    //        { tricare.wsUrl = c.TestwsUrl; }

                    //        r = tricare.GetResponse();
                    //        if (r == 0)
                    //        {
                    //            _RES = tricare.RES;
                    //            _OrigRES = tricare.OrigRes;
                    //            doRESValidation = true;
                    //        }
                    //        else
                    //        {
                    //            r = -1;
                    //            _OrigRES = tricare.OrigRes;
                    //            log.ExceptionDetails("1165-DCSGlobal.EDI.Comunications.EDI.BCWV.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                    //        }
                    //    }
                    //    break;
                    case "OHMCD_DIRECT":
                        using (OhioMedicaid ohmcd = new OhioMedicaid())
                        {

                            ohmcd.VendorName = VendorName;
                            ohmcd.PayorCode = PayorCode;
                            ohmcd.ConnectionString = _ConnectionString;
                            ohmcd.wsUrl = c.wsUrl;

                            ohmcd.UserName = c.username;
                            ohmcd.Passwd = c.password;

                            ohmcd.REQ = RequestContent;

                            ohmcd.TaskID = _TaskID;
                            ohmcd.EBRID = _EBRID;
                            ohmcd.WaitTime = _WaitTime;
                            ohmcd.BHT03 = B_REQ;
                            ohmcd.VendorRetrys = _VendorRetrys;


                            ohmcd.PayloadType = c.PayloadType;
                            ohmcd.ProcessingMode = c.ProcessingMode;
                            ohmcd.PayloadID = B_REQ; //  c.PayloadID;
                            ohmcd.TimeStamp = c.TimeStamp;
                            ohmcd.SenderID = c.SenderID;
                            ohmcd.ReceiverID = c.ReceiverID;
                            ohmcd.CORERuleVersion = c.CORERuleVersion;
                            ohmcd.SendTimeout = c.ServiceTimeOut;
                            ohmcd.ReceiveTimeout = c.ReceiveTimeout;
                            ohmcd.VMetricsLogging = c.VMetricsLogging;



                            if (!_isTest)
                            { ohmcd.wsUrl = c.wsUrl; }
                            else
                            { ohmcd.wsUrl = c.TestwsUrl; }

                            r = ohmcd.GetResponse();
                            if (r == 0)
                            {
                                _RES = ohmcd.RES;
                                _OrigRES = ohmcd.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = ohmcd.OrigRes;
                                log.ExceptionDetails("1165-DCSGlobal.EDI.Comunications.EDI.OHMCD_DIRECT.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;
                    case "OHMCD":
                        using (OhioDCSWebService dcsohmcd = new OhioDCSWebService())
                        {

                            dcsohmcd.VendorName = VendorName;
                            dcsohmcd.PayorCode = PayorCode;
                            dcsohmcd.ConnectionString = _ConnectionString;
                            dcsohmcd.wsUrl = c.wsUrl;

                            dcsohmcd.UserName = c.username;
                            dcsohmcd.login = c.login;
                            dcsohmcd.password = c.password;

                            dcsohmcd.REQ = RequestContent;

                            dcsohmcd.TaskID = _TaskID;
                            dcsohmcd.EBRID = _EBRID;
                            dcsohmcd.WaitTime = _WaitTime;
                            dcsohmcd.BHT03 = B_REQ;
                            dcsohmcd.VendorRetrys = _VendorRetrys;


                            dcsohmcd.PayloadType = c.PayloadType;
                            dcsohmcd.ProcessingMode = c.ProcessingMode;
                            dcsohmcd.PayloadID = B_REQ; //  c.PayloadID;
                            dcsohmcd.TimeStamp = c.TimeStamp;
                            dcsohmcd.SenderID = c.SenderID;
                            dcsohmcd.ReceiverID = c.ReceiverID;
                            dcsohmcd.CORERuleVersion = c.CORERuleVersion;
                            dcsohmcd.SendTimeout = c.ServiceTimeOut;
                            dcsohmcd.ReceiveTimeout = c.ReceiveTimeout;
                            dcsohmcd.VMetricsLogging = c.VMetricsLogging;



                            if (!_isTest)
                            { dcsohmcd.wsUrl = c.wsUrl; }
                            else
                            { dcsohmcd.wsUrl = c.TestwsUrl; }

                            r = dcsohmcd.GetResponse();
                            if (r == 0)
                            {
                                _RES = dcsohmcd.RES;
                                _OrigRES = dcsohmcd.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = dcsohmcd.OrigRes;
                                log.ExceptionDetails("1165-DCSGlobal.EDI.Comunications.EDI.OHMCD.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;
                    case "NYMCD":
                        using (NewYorkMedicaid nymcd = new NewYorkMedicaid())
                        {

                            nymcd.VendorName = VendorName;
                            nymcd.PayorCode = PayorCode;
                            nymcd.ConnectionString = _ConnectionString;
                            nymcd.wsUrl = c.wsUrl;

                            nymcd.UserName = c.username;
                            //nymcd.login = c.login;
                            //nymcd.password = c.password;
                            nymcd.Passwd = c.password;
                            

                            nymcd.REQ = RequestContent;

                            nymcd.TaskID = _TaskID;
                            nymcd.EBRID = _EBRID;
                            nymcd.WaitTime = _WaitTime;
                            nymcd.BHT03 = B_REQ;
                            nymcd.VendorRetrys = _VendorRetrys;


                            nymcd.PayloadType = c.PayloadType;
                            nymcd.ProcessingMode = c.ProcessingMode;
                            nymcd.PayloadID = B_REQ; //  c.PayloadID;
                            nymcd.TimeStamp = c.TimeStamp;
                            nymcd.SenderID = c.SenderID;
                            nymcd.ReceiverID = c.ReceiverID;
                            nymcd.CORERuleVersion = c.CORERuleVersion;
                            nymcd.SendTimeout = c.ServiceTimeOut;
                            nymcd.ReceiveTimeout = c.ReceiveTimeout;
                            nymcd.VMetricsLogging = c.VMetricsLogging;



                            if (!_isTest)
                            { nymcd.wsUrl = c.wsUrl; }
                            else
                            { nymcd.wsUrl = c.TestwsUrl; }

                            r = nymcd.GetResponse();
                            if (r == 0)
                            {
                                _RES = nymcd.RES;
                                _OrigRES = nymcd.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = nymcd.OrigRes;
                                log.ExceptionDetails("1165-DCSGlobal.EDI.Comunications.EDI.nymcd.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;
                    case "COMCD":
                        using (ColoradoMedicaid colorado = new ColoradoMedicaid())
                        {

                            colorado.VendorName = VendorName;
                            colorado.PayorCode = PayorCode;
                            colorado.ConnectionString = _ConnectionString;
                            colorado.wsUrl = c.wsUrl;
                            colorado.UserName = c.username;
                            colorado.Passwd = c.password;
                            colorado.REQ = RequestContent;
                            colorado.TaskID = _TaskID;
                            colorado.EBRID = _EBRID;
                            colorado.WaitTime = _WaitTime;
                            colorado.BHT03 = B_REQ;
                            colorado.VendorRetrys = _VendorRetrys;
                            colorado.PayloadType = c.PayloadType;
                            colorado.ProcessingMode = c.ProcessingMode;
                            colorado.PayloadID = B_REQ; //  c.PayloadID;
                            colorado.TimeStamp = c.TimeStamp;
                            colorado.SenderID = c.SenderID;
                            colorado.ReceiverID = c.ReceiverID;
                            colorado.CORERuleVersion = c.CORERuleVersion;
                            colorado.SendTimeout = c.ServiceTimeOut;
                            colorado.ReceiveTimeout = c.ReceiveTimeout;
                            colorado.VMetricsLogging = c.VMetricsLogging;

                            if (!_isTest)
                            { colorado.wsUrl = c.wsUrl; }
                            else
                            { colorado.wsUrl = c.TestwsUrl; }

                            r = colorado.GetResponse();
                            if (r == 0)
                            {
                                _RES = colorado.RES;
                                _OrigRES = colorado.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = colorado.OrigRes;
                                log.ExceptionDetails("1165-DCSGlobal.EDI.Comunications.EDI.colorado.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;
                    case "BCLA":
                        using (BCBSLouisiana bcbsla = new BCBSLouisiana())
                        {

                            bcbsla.VendorName = VendorName;
                            bcbsla.PayorCode = PayorCode;
                            bcbsla.ConnectionString = _ConnectionString;
                            bcbsla.wsUrl = c.wsUrl;
                            bcbsla.UserName = c.username;
                            bcbsla.Passwd = c.password;
                            bcbsla.REQ = RequestContent;
                            bcbsla.TaskID = _TaskID;
                            bcbsla.EBRID = _EBRID;
                            bcbsla.WaitTime = _WaitTime;
                            bcbsla.BHT03 = B_REQ;
                            bcbsla.VendorRetrys = _VendorRetrys;
                            bcbsla.PayloadType = c.PayloadType;
                            bcbsla.ProcessingMode = c.ProcessingMode;
                            bcbsla.PayloadID = B_REQ; //  c.PayloadID;
                            bcbsla.TimeStamp = c.TimeStamp;
                            bcbsla.SenderID = c.SenderID;
                            bcbsla.ReceiverID = c.ReceiverID;
                            bcbsla.CORERuleVersion = c.CORERuleVersion;
                            bcbsla.SendTimeout = c.ServiceTimeOut;
                            bcbsla.ReceiveTimeout = c.ReceiveTimeout;
                            bcbsla.VMetricsLogging = c.VMetricsLogging;

                            if (!_isTest)
                            { bcbsla.wsUrl = c.wsUrl; }
                            else
                            { bcbsla.wsUrl = c.TestwsUrl; }

                            r = bcbsla.GetResponse();
                            if (r == 0)
                            {
                                _RES = bcbsla.RES;
                                _OrigRES = bcbsla.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = bcbsla.OrigRes;
                                log.ExceptionDetails("1165-DCSGlobal.EDI.Comunications.EDI.BCLA.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;
                    case "PAMCD":
                        using (PAMCD pamcd = new PAMCD())
                        {

                            pamcd.VendorName = VendorName;
                            pamcd.PayorCode = PayorCode;
                            pamcd.ConnectionString = _ConnectionString;
                            pamcd.wsUrl = c.wsUrl;
                            pamcd.UserName = c.username;
                            pamcd.Passwd = c.password;
                            pamcd.REQ = RequestContent;
                            pamcd.TaskID = _TaskID;
                            pamcd.EBRID = _EBRID;
                            pamcd.WaitTime = _WaitTime;
                            pamcd.BHT03 = B_REQ;
                            pamcd.VendorRetrys = _VendorRetrys;
                            pamcd.PayloadType = c.PayloadType;
                            pamcd.ProcessingMode = c.ProcessingMode;
                            pamcd.PayloadID = B_REQ; //  c.PayloadID;
                            pamcd.TimeStamp = c.TimeStamp;
                            pamcd.SenderID = c.SenderID;
                            pamcd.ReceiverID = c.ReceiverID;
                            pamcd.CORERuleVersion = c.CORERuleVersion;
                            pamcd.SendTimeout = c.ServiceTimeOut;
                            pamcd.ReceiveTimeout = c.ReceiveTimeout;
                            pamcd.VMetricsLogging = c.VMetricsLogging;

                            if (!_isTest)
                            { pamcd.wsUrl = c.wsUrl; }
                            else
                            { pamcd.wsUrl = c.TestwsUrl; }

                            r = pamcd.GetResponse();
                            
                            if (r == 0)
                            {
                                _RES = pamcd.RES;
                                _OrigRES = pamcd.OrigRes;
                                doRESValidation = true;
                            }
                            else
                            {
                                r = -1;
                                _OrigRES = pamcd.OrigRes;
                                log.ExceptionDetails("1165-DCSGlobal.EDI.Comunications.EDI.BCLA.GetResponse Failed. Batch ID:" + Convert.ToString(_BatchID), "GetResponse Failed r =" + Convert.ToString(r));
                            }
                        }
                        break;

                    case "SCRAPE":
                        r = -1;
                        //  _RES = "This is screen scrape record and will not process at this time.";
                        log.ExceptionDetails("1170-DCSGlobal.EDI.Comunications.EDI.MEDDATA.SCRAPEEx", "SCRAPE Record");
                        break;

                    default:

                        r = -1;
                        //  _RES = "This is default Vendor. And it is not a valid Vendor name.";
                        log.ExceptionDetails("1180-DCSGlobal.EDI.Comunications.EDI.default.Ex", "Vendor Name Not Found  | " + VendorName);
                        break;
                }
            }

             
            try
            {

                if (r == 0)
                {
                    if (!_RES.Contains("ISA"))
                    {
                        doRESValidation = false;

                    }


                    if (_RES.Length < 109)
                    {
                        doRESValidation = false;

                    }


                }
                else
                {
                    if (VendorName.ToUpper()=="EMDEONLOOKUP") 
                    {
                        //do nothing..since we have already taken care at Emdeon.Ex.

                    }
                    else
                    {
                        norespid = -1;
                        using (NoResponse nr = new NoResponse())
                        {
                            nr.ConnectionString = _ConnectionString;
                            norespid = nr.LogNoResponse(_BatchID);
                            if (norespid == 0)
                            {
                                _RES = nr.RES;
                                doRESValidation = false;
                                r = 0;
                            }
                            else
                            {
                                log.ExceptionDetails(VendorName + "Traped Execption in NORESPONSE With Batch ID " + Convert.ToString(_BatchID), " RES:" + _RES);
                            }
                        }
                    }
                }



                if (doRESValidation)
                {


                    using (ValidateEDI vedi = new ValidateEDI())
                    {
                        vedi.ConnectionString = _ConnectionString;
                        vedi.byString(_RES);
                        B_RES = vedi.ReferenceIdentification;
                        RES_TransactionSetIdentifierCode = vedi.TransactionSetIdentifierCode;
                        AAAFailureCode = vedi.AAAFailureCode;
                    }

                }



                if (doRESValidation)
                {


                    switch (RES_TransactionSetIdentifierCode)
                    {

                        //case "270":
                            
                        //    break;
                        case "997":

                            r = -1997;

                            break;

                        case "999":


                           // Is999 = true;

                            //If response is success(r==0) with ST*999 (AK9) error  then log EE response.
                            if (r == 0)
                            {
                                norespid = -1;
                                using (NoResponse nr = new NoResponse())
                                {
                                    nr.ConnectionString = _ConnectionString;
                                    norespid = nr.LogNoResponse(_BatchID);
                                    if (norespid == 0)
                                    {
                                        _RES = nr.RES;
                                        doRESValidation = false;
                                        r = 0;
                                    }
                                    else
                                    {
                                        log.ExceptionDetails(VendorName + "Traped Execption in NORESPONSE With Batch ID " + Convert.ToString(_BatchID), " RES:" + _RES);
                                    }
                                }
                            }

                            //If response is success(r==0) with ST*999 (AK9) error  then log EE response.
                            //Commented below line. Because we are setting r=0 .
                            //r = -1999;

                            break;


                        case "278":

                            break;


                        case "277":
                            if (!isXML)
                            {
                                if (REQ_TransactionSetIdentifierCode == "276")
                                {
                                    if (isISA)
                                    {
                                        if (B_RES != B_REQ)
                                        {
                                            //20170724
                                            //Commented below lines for Dallas Children Claim process. Because , we are getting bht missmatch.
                                            //Claim process( 276 REQ & 277 RES) will use Communication dll for FE and will pass request 276 as input for Claim Status screen.

                                            //// this is a  real mismatch so log it change to something useful
                                            //log.ExceptionDetails("1192-DCSGlobal.EDI.Comunications.EDI.default.Ex", "BHT  mismatch BREQ '" + B_REQ + "'" + " B_RES '" + B_RES + "'");
                                            //r = -6;
                                        }
                                    }
                                }
                                else
                                {
                                    log.ExceptionDetails("1191-DCSGlobal.EDI.Comunications.EDI.default.Ex", " sent 277  got   '" + RES_TransactionSetIdentifierCode + " ' '" + _RES);
                                    r = -7;

                                }
                            }

                            break;

                        case "271":
                            if (!isXML)
                            {
                                if (REQ_TransactionSetIdentifierCode == "270")
                                {
                                    if (isISA)
                                    {
                                        if (B_RES != B_REQ)
                                        {
                                            // this is a  real mismatch so log it change to something useful
                                            log.ExceptionDetails("1190-DCSGlobal.EDI.Comunications.EDI.default.Ex. Batch ID:" + Convert.ToString(_BatchID), "BHT  mismatch BREQ '" + B_REQ + "'" + " B_RES '" + B_RES + "'");
                                            r = -5;
                                        }
                                    }

                                }
                                else
                                {
                                    log.ExceptionDetails("1191-DCSGlobal.EDI.Comunications.EDI.default.Ex. Batch ID:" + Convert.ToString(_BatchID), " expected 271 got '" + RES_TransactionSetIdentifierCode + " '  '" + _RES);
                                    r = -8;

                                }

                            }
                            break;

                        case "TA1":
                            log.ExceptionDetails("1191-DCSGlobal.EDI.Comunications.EDI.default.Ex. Batch ID:" + Convert.ToString(_BatchID), "Vendor Returned TA1");


                            break;

                        default:

                            // this is a  real mismatch so log it change to something useful
                            log.ExceptionDetails("1200-DCSGlobal.EDI.Comunications.EDI.default.Ex. Batch ID:" + Convert.ToString(_BatchID), "Unable to detremine TransactionSetIdentifierCode or no RES | " + VendorName);
                            r = -4;

                            break;

                    }

                }

            }

            catch (Exception ex)
            {

                log.ExceptionDetails("1250-DCSGlobal.EDI.Comunications.getEligVendorEdiResponse switch.Ex. Batch ID:" + Convert.ToString(_BatchID), ex);
            }

 

            if (VendorName.ToUpper() == "EMDEONLOOKUP") 
            {
                //do nothing..since we have already taken care at Emdeon.Ex.

            }
            else if (VendorName.ToUpper() == "MEDDATA")
            {
                string sreq = _REQ.Substring(0, 3);
                if (sreq != "ISA")   //this is only for meddata in xml format
                {
                    //This line is added because of Adena client using xml format and not able to login to RESPONSE TEMP
                    //Because of REQ_TransactionSetIdentifierCode is null
                    using (AuditResponseLogging arl = new AuditResponseLogging())
                    {
                        //log.ExceptionDetails("AuditResponseLogging for Batch ID: " + Convert.ToString(_BatchID) + " for the Vendor: " + VendorName + " with Response is : ", _RES);
                        arl.ConnectionString = _ConnectionString;
                        //arl.Log271(_BatchID, PayorCode, _RES, _EBRID, VendorName, AAAFailureCode, NPI, ServiceTypeCode);
                        arl.Log271(_BatchID, PayorCode, _OrigRES, _EBRID, VendorName, AAAFailureCode, NPI, ServiceTypeCode);

                    }
                }
            }
            else
            {
                //Dallas  Children - 276 Claim console will not log EDI Response in this table.
                if (REQ_TransactionSetIdentifierCode == "270")
                {
                    using (AuditResponseLogging arl = new AuditResponseLogging())
                    {
                        //log.ExceptionDetails("AuditResponseLogging for Batch ID: " + Convert.ToString(_BatchID) + " for the Vendor: " + VendorName + " with Response is : ", _RES);
                        arl.ConnectionString = _ConnectionString;
                        //arl.Log271(_BatchID, PayorCode, _RES, _EBRID, VendorName, AAAFailureCode, NPI, ServiceTypeCode);
                        arl.Log271(_BatchID, PayorCode, _OrigRES, _EBRID, VendorName, AAAFailureCode, NPI, ServiceTypeCode);

                    }
                }
            }

           


          

            if (_Verbose)
            {
                log.ExceptionDetails("getEligVendorEdiResponseEx. Batch ID:" + Convert.ToString(_BatchID) + "-" + VendorName, "Completed");
            }


            //log.ExceptionDetails("EDI.Communication.GetResponse for Batch ID: " + Convert.ToString(_BatchID) + " for the Vendor: " + VendorName + " with Response is : ", _RES );
            return r;
        }
    }

}





