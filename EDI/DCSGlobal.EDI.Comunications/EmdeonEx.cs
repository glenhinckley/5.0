using System;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;
using DCSGlobal.EDI.Comunications;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using DCSGlobal.BusinessRules.Logging;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Globalization;
 

using System.Xml;


namespace DCSGlobal.EDI.Comunications
{
    class EmdeonEx : IDisposable
    {
        private long GlobalBatchID = 0;
        private StringStuff ss = new StringStuff();
        private logExecption log = new logExecption();

        private string _subscriberfirstname = string.Empty;
        private string _subscriberlastname = string.Empty;
        private string _subscriberdob = string.Empty;
        private string _dateofservice = string.Empty;

        private string _ConnectionString = string.Empty;
        private string _LookupUrl = "https://assistant.emdeon.com/Services/Assistant";

        private string _EmdReq = string.Empty;
        private string _EmdRes = string.Empty;

        private string _VendorInputType = string.Empty;
        private string _VendorName = string.Empty;
        private string _PayorCode = string.Empty;

        private string _AppName = string.Empty;

        private int _EmdeonSmartDelete = 0;

        private string SYNC_TIMEOUT = string.Empty;
        private string SUBMISSION_TIMEOUT = string.Empty;
        private string _myResponses = string.Empty;
        private string _myResponses1 = string.Empty;

        private string _REQ = string.Empty;
        private string _RES = string.Empty;

        private string _wsUrl = string.Empty;
        private string _UserName = string.Empty;
        private string _Passwd = string.Empty;
        private string _apiKey = "jhZiUkWOgY";
        private string _ContentType = string.Empty;
        private string _Method = string.Empty;
        private int _ServiceTimeOut = 0;
        private string _LogToREQRES = string.Empty;
        private string _Token = string.Empty;
        long _elapsedTicks = 0;
        private int _VMetricsLogging = 0;

        private string _LoginUserID = string.Empty;

        private string _PatientHospitalCode = string.Empty;

        private string _Hosp_Code = string.Empty;

        private string _InsType = string.Empty;

        private string _PatAcctNum = string.Empty;
        private string _ProtocolType = string.Empty;
        private string sVendorName = string.Empty;
        
        private int _TaskID = 0;
        private string _EBRID = string.Empty;
        private int _VendorRetrys = 1;
        private int MLOG_ID = 0;
        private int _WaitTime = 0;
        private string B_REQ = string.Empty;
        private string ErrorMsg = "";
        private string stime = string.Empty;
        private string rtime = string.Empty;
        private string sTimeStamp = string.Empty;
        private string sPayload = string.Empty;
        private string B_RES = string.Empty;
        private string PayloadIDTX = string.Empty;
        private string PayloadIDRX = string.Empty;
        private string sPayloadID = string.Empty;
        private bool ResEmdeonAssistant = false;
        private long _BatchID = 0;
        private long _ReceiveTimeout = 30;
         
        private int norespid = -1;
        ~EmdeonEx()
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


        private string _OrigRes = string.Empty;

        public string OrigRes
        {
            get
            {
                return ss.StripCRLF(_OrigRes);
            }
            set
            {
                try
                {
                    _OrigRes = ss.StripCRLF(value);

                }
                catch (Exception ex)
                {

                }
            }
        }
        public long BatchID
        {

            set
            {

                _BatchID = value;
            }
        }

        public string LoginUserID
        {

            set
            {

                _LoginUserID = value;
            }
        }

        public string BHT03
        {

            set
            {

                B_REQ = value;
            }
        }
 

        public int VendorRetrys
        {

            set
            {

                _VendorRetrys = value;
            }
        }

        public int WaitTime
        {

            set
            {

                _WaitTime = value;
            }
        }

        public int TaskID
        {

            set
            {
                _TaskID = value;
            }
        }

        public string EBRID
        {

            set
            {

                _EBRID = value;
            }
        }

        public string subscriberfirstname
        {

            set
            {

                _subscriberfirstname = value;
            }
        }

        public string subscriberlastname
        {

            set
            {

                _subscriberlastname = value;
            }
        }

        public string subscriberdob
        {

            set
            {

                _subscriberdob = value;
            }
        }

        public string dateofservice
        {

            set
            {

                _dateofservice = value;
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

        public string PatientHospitalCode
        {

            set
            {

                _PatientHospitalCode = value;
            }
        }

        public string Hosp_Code
        {

            set
            {

                _Hosp_Code = value;
            }
        }
        
        
        public int VMetricsLogging
        {

            set
            {

                _VMetricsLogging = value;
            }
        }

        public string ProtocolType
        {

            set
            {

                _ProtocolType = value;
            }
        }

        public string LookupUrl
        {
            set
            {
                _LookupUrl = value;
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


         public string InsType
        {
            set
            {
                _InsType = value;
            }
        }

        public string PatAcctNum
        {
            set
            {
                _PatAcctNum = value;
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
                    log.ExceptionDetails("emdeon.EmdeonSmartDelete Failed", ex);
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
            int i = 0;
            int VendorTrys = 0;

            while (i < _VendorRetrys)
            {

                r = GoToVendor();


                if (r == 0)
                {
                    
                    VendorTrys++;
                    i = 1 + _VendorRetrys;
                }
                else
                {
                     
                    VendorTrys++;
                    // Console.WriteLine(_EBRID + " retry " + Convert.ToString(VendorTrys) + " Error Message " + ErrorMsg);
                    log.ExceptionDetails(_VendorName, _EBRID + " retry " + Convert.ToString(VendorTrys) + " Error Message " + ErrorMsg);

                    _WaitTime = _WaitTime + 250;
                }
                i++;
            }
           
            if (_VendorRetrys == VendorTrys)
            {

                log.ExceptionDetails(_VendorName + "  Exceded Ventor Retrys giving up with  VendorTrys  at " + Convert.ToString(VendorTrys), _EBRID);
                r = -5;
            }

            r = -999;   //In this case, we are not doing any process logic in EDIGetResponse.
           

            return r;

        }

        //public int GoToVendor_working()
        //{
        //    int r = -1;
        //    DateTime StartTime = DateTime.Now;
        //    DateTime Endtime;
        //    _RES = string.Empty;

        //    stime = string.Empty;
        //    rtime = string.Empty;

        //    try
        //    {
        //        string httpData = string.Empty;

        //        sPayloadID = Guid.NewGuid().ToString();  //_PayloadID;


        //        //Random rnd = new Random();
        //        int m = 0; //rnd.Next(0, 1000);
        //        //  m = m * 5;
        //        Thread.Sleep(_WaitTime);
        //        //var xmlRequest = xd.OuterXml;


        //        sTimeStamp = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:fffZ");


        //        EmdeonPxy.AWS client = new EmdeonPxy.AWS();
        //        client.Url = _wsUrl;
        //        client.Timeout = _ServiceTimeOut;
        //        if (_VMetricsLogging == 1)
        //        {
        //            log.ExceptionDetails("EMDEON Sent at " + Convert.ToString(sTimeStamp) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _REQ);
        //        }

        //        stime = sTimeStamp;

        //        httpData = ASCIIEncoding.ASCII.GetString(Convert.FromBase64String(Convert.ToBase64String(client.RunTransaction(_UserName, _Passwd, Encoding.UTF8.GetBytes(_REQ)))));
        //        _RES = httpData;

        //        sPayload = _RES;


        //        rtime = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:fffZ");

        //        if (_VMetricsLogging == 1)
        //        {
        //            log.ExceptionDetails("EMDEON REcived  at " + Convert.ToString(rtime) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _RES);
        //        }

        //        using (ValidateEDI vedi = new ValidateEDI())
        //        {
        //            vedi.ConnectionString = _ConnectionString;
        //            vedi.byString(sPayload);
        //            B_RES = vedi.ReferenceIdentification;
        //        }

        //        r = 0;
        //        if (!_RES.Contains("ISA"))
        //        {
        //            MLOG_ID = log.MLOG(_VendorName, ErrorMsg, _EBRID, stime, rtime, B_RES, B_REQ, PayloadIDTX, PayloadIDRX, _RES, _REQ);
        //            r = -2;
        //        }


        //        if (B_RES != B_REQ)
        //        {
        //            MLOG_ID = log.MLOG(_VendorName, ErrorMsg, _EBRID, stime, rtime, B_RES, B_REQ, PayloadIDTX, PayloadIDRX, sPayload, _REQ);
        //            r = -3;
        //            log.ExceptionDetails(_VendorName + " BHT03 mismatch req and res are in MLOG with MLOG_ID " + Convert.ToString(MLOG_ID), Convert.ToString(DateTime.Now) + " | PayloadID=" + sPayloadID + " | Error Status= " + ErrorMsg + " Response = " + sPayload + " | Vendor Name =  " + _VendorName + "| Payor Code=" + _PayorCode + "|" + "waitstate=" + Convert.ToString(m) + "|" + _wsUrl);

        //        }

        //        if (r < -4)   //we don't have Request GUID & Response GUID.
        //        {
        //            //if (PayloadIDTX != PayloadIDRX)
        //            //{
        //            //    MLOG_ID = log.MLOG(_VendorName, ErrorMsg, _EBRID, stime, rtime, B_RES, B_REQ, PayloadIDTX, PayloadIDRX, sPayload, _REQ);
        //            //    r = -4;
        //            //    log.ExceptionDetails(_VendorName + "  Payload mismatch req and res are in MLOG with MLOG_ID " + Convert.ToString(MLOG_ID), Convert.ToString(DateTime.Now) + " | PayloadID=" + sPayloadID + " | Error Status= " + ErrorMsg + " Response = " + sPayload + " | Vendor Name =  " + _VendorName + "| Payor Code=" + _PayorCode + "|" + "waitstate=" + Convert.ToString(m) + "|" + _wsUrl);

        //            //}
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        r = -1;
        //        Endtime = DateTime.Now;
        //        _elapsedTicks = Endtime.Ticks - StartTime.Ticks;
        //        log.ExceptionDetails("GetResponse Failed Total Elasped ticks" + Convert.ToString(_elapsedTicks) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl + "|" + _REQ, ex);

        //    }


        //    return r;



        //}

        public int ProcessEmdeon()
        {
            int x = -1;
            int r = -1;
            DateTime StartTime = DateTime.Now;
            DateTime Endtime;
            _RES = string.Empty;

            stime = string.Empty;
            rtime = string.Empty;

            _EmdReq = "";
            _EmdRes = "";

          

            try
            {

                if (_ProtocolType.ToUpper() == "TLS12")
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                }
                else if (_ProtocolType.ToUpper() == "TLS11")
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11;
                }
                else if (_ProtocolType.ToUpper() == "TLS")
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                }
                else if (_ProtocolType.ToUpper() == "SSL3")
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                }
                else
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;
                }



                string httpData = string.Empty;

                sPayloadID = Guid.NewGuid().ToString();  //_PayloadID;


                //Random rnd = new Random();
                int m = 0; //rnd.Next(0, 1000);
                //  m = m * 5;
                Thread.Sleep(_WaitTime);
                //var xmlRequest = xd.OuterXml;


                sTimeStamp = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:fffZ");


                EmdeonPxy.AWS client = new EmdeonPxy.AWS();
                client.Url = _wsUrl;
                client.Timeout = _ServiceTimeOut;
                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("EMDEON Sent at " + Convert.ToString(sTimeStamp) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _REQ);
                }

                stime = sTimeStamp;

                _EmdReq = _REQ;

                httpData = ASCIIEncoding.ASCII.GetString(Convert.FromBase64String(Convert.ToBase64String(client.RunTransaction(_UserName, _Passwd, Encoding.UTF8.GetBytes(_REQ)))));
                _RES = httpData;

               
                sPayload = _RES;

                _EmdRes = _RES;


                rtime = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:fffZ");

                if (_VMetricsLogging == 1)
                {
                    log.ExceptionDetails("EMDEON REcived  at " + Convert.ToString(rtime) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl, _RES);
                }

                using (ValidateEDI vedi = new ValidateEDI())
                {
                    vedi.ConnectionString = _ConnectionString;
                    vedi.byString(sPayload);
                    B_RES = vedi.ReferenceIdentification;
                }

                r = 0;
                if (!_RES.Contains("ISA"))
                {
                    MLOG_ID = log.MLOG(_VendorName, ErrorMsg, _EBRID, stime, rtime, B_RES, B_REQ, PayloadIDTX, PayloadIDRX, _RES, _REQ);
                    r = -2;
                }


                if (B_RES != B_REQ)
                {
                    MLOG_ID = log.MLOG(_VendorName, ErrorMsg, _EBRID, stime, rtime, B_RES, B_REQ, PayloadIDTX, PayloadIDRX, sPayload, _REQ);
                    r = -3;
                    log.ExceptionDetails(_VendorName + " BHT03 mismatch req and res are in MLOG with MLOG_ID " + Convert.ToString(MLOG_ID), Convert.ToString(DateTime.Now) + " | PayloadID=" + sPayloadID + " | Error Status= " + ErrorMsg + " Response = " + sPayload + " | Vendor Name =  " + _VendorName + "| Payor Code=" + _PayorCode + "|" + "waitstate=" + Convert.ToString(m) + "|" + _wsUrl);

                }

                if (r < -4)   //we don't have Request GUID & Response GUID.
                {
                    //if (PayloadIDTX != PayloadIDRX)
                    //{
                    //    MLOG_ID = log.MLOG(_VendorName, ErrorMsg, _EBRID, stime, rtime, B_RES, B_REQ, PayloadIDTX, PayloadIDRX, sPayload, _REQ);
                    //    r = -4;
                    //    log.ExceptionDetails(_VendorName + "  Payload mismatch req and res are in MLOG with MLOG_ID " + Convert.ToString(MLOG_ID), Convert.ToString(DateTime.Now) + " | PayloadID=" + sPayloadID + " | Error Status= " + ErrorMsg + " Response = " + sPayload + " | Vendor Name =  " + _VendorName + "| Payor Code=" + _PayorCode + "|" + "waitstate=" + Convert.ToString(m) + "|" + _wsUrl);

                    //}
                }

            }
            catch (Exception ex)
            {
                r = -1;
                Endtime = DateTime.Now;
                _elapsedTicks = Endtime.Ticks - StartTime.Ticks;
                log.ExceptionDetails("GetResponse Failed Total Elasped ticks" + Convert.ToString(_elapsedTicks) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl + "|" + _REQ, ex);

            }

            if (r == 0)
            {
                x = Import("EMDEON",false);
                if (x < 0)
                {
                    log.ExceptionDetails("25: " + _AppName + " EMDEON  Import  failed for EBR ID: " + Convert.ToString(_EBRID), "sr1", "EMDEON  Import", _TaskID, 749);
                }
            }


            return r;



        }

        public int GoToVendor()
        {
            int x = -1;
            int i = -1;
            int r = -1;
            DateTime StartTime = DateTime.Now;
            DateTime Endtime;
            string EmdeonResponse = string.Empty;
             
            try
            {
                if (_VendorName.ToUpper() == "EMDEONLOOKUP")
                {
                    r = ProcessEmdeonAssistant();
                    if (r < 0)
                    {
                        r = ProcessEmdeon();
                    }
                }
                else
                {
                    r = ProcessEmdeon();
                }
                //r = GoToVendor();

                if (r == -1)
                {
                    x =  ImportWithNoResponse("EMDEON",false);
                }
                
            }
            catch (Exception ex)
            {
                r = -1;
                Endtime = DateTime.Now;
                _elapsedTicks = Endtime.Ticks - StartTime.Ticks;
                log.ExceptionDetails("GoToVendor Failed Total Elasped ticks" + Convert.ToString(_elapsedTicks) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl + "|" + _REQ, ex);

            }
            return r;
        }

 
 

         

        public int ProcessEmdeonAssistant()
        {
            
            int i = -1;
            int x = 0;
            DateTime StartTime = DateTime.Now;
            DateTime Endtime;
 

            _EmdeonSmartDelete = 0;
            string input = string.Empty;
            string _wsAssistURL = string.Empty;
            _RES = string.Empty;
            //_LookupUrl = "https://assistant.emdeon.com/Services/Assistant";

            try
            {
                try
                {

                    if (_ProtocolType.ToUpper() == "TLS12")
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    }
                    else if (_ProtocolType.ToUpper() == "TLS11")
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11;
                    }
                    else if (_ProtocolType.ToUpper() == "TLS")
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                    }
                    else if (_ProtocolType.ToUpper() == "SSL3")
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                    }
                    else
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;
                    }



                    input = string.Format("<emdeon_lookup><patient_last_name>{0}</patient_last_name><patient_first_name>{1}</patient_first_name><patient_dob>{2}</patient_dob><transaction_date>{3}</transaction_date></emdeon_lookup>", _subscriberlastname, _subscriberfirstname, _subscriberdob, _dateofservice);
                    using (AuditResponseLogging arl = new AuditResponseLogging())
                    {
                        arl.ConnectionString = _ConnectionString;
                        arl.AuditEmdeonLookup(_EBRID, input, _LoginUserID);

                    }
                }
                catch (Exception ex)
                {
                    //WriteEvent(ex.Message)
                }


                com.emdeon.assistant.Assistant objas = new com.emdeon.assistant.Assistant();
                objas.Url = _LookupUrl;
                 
               // objas.Credentials = new NetworkCredential("PMNADCSGL", "TMURwvb3");
          

                com.emdeon.assistant.DtoSearchField[] InputFlds = new com.emdeon.assistant.DtoSearchField[8];
                 
                 
                for (int q = 0; q <= 7; q++)
                {
                    InputFlds[q] = new com.emdeon.assistant.DtoSearchField();
                }


                
                ///// <remarks/>
                //AccountNumber,

                ///// <remarks/>
                //MedicalRecordNumber,

                ///// <remarks/>
                //PatientLastName,

                ///// <remarks/>
                //PatientFirstName,

                ///// <remarks/>
                //PatientDateOfBirth,

                ///// <remarks/>
                //RecipientId,

                ///// <remarks/>
                //DateOfService,

                ///// <remarks/>
                //EmdeonPayerCode,

                //InputFlds[0].FieldType = com.emdeon.assistant.SearchFields.AccountNumber;
                //InputFlds[0].Value = "228816377";

                //InputFlds[1].FieldType = com.emdeon.assistant.SearchFields.MedicalRecordNumber;
                //InputFlds[1].Value = "228816377";

                InputFlds[2].FieldType = com.emdeon.assistant.SearchFields.PatientLastName;
                InputFlds[2].Value = _subscriberlastname;
                // "LEBRUN"

                InputFlds[3].FieldType = com.emdeon.assistant.SearchFields.PatientFirstName;
                InputFlds[3].Value = _subscriberfirstname;
                // "CLAUDE"

                InputFlds[4].FieldType = com.emdeon.assistant.SearchFields.PatientDateOfBirth;
                InputFlds[4].Value = DateTime.ParseExact(_subscriberdob, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");   // "1983-11-02T09:00:00"; // _subscriberdob;
                //"11/26/1962"

                //InputFlds[5].FieldType = com.emdeon.assistant.SearchFields.RecipientId;
                //InputFlds[5].Value = "228816377";

                InputFlds[6].FieldType = com.emdeon.assistant.SearchFields.DateOfService;
                InputFlds[6].Value = DateTime.ParseExact(_dateofservice, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");   //"2016-12-14T09:00:00"; // ;
                // "02/17/2014"

                //InputFlds[7].FieldType = com.emdeon.assistant.SearchFields.EmdeonPayerCode;
                //InputFlds[7].Value = "00002";

                com.emdeon.assistant.DtoRequest objReq = new com.emdeon.assistant.DtoRequest();
                objReq.ApiKey = _apiKey;
                objReq.DtoSearchFields = InputFlds;


                //Sample valid data point for EmdeonAssistant - BMC JAX - EBR ID: 306 
                //  subscriberlastname: ALLMAN , subscriberfirstname: BRUCE , dateofbirth:19600808, dateofservice: 20160817
                com.emdeon.assistant.DtoResponse objDtoResponse = new com.emdeon.assistant.DtoResponse();
                objDtoResponse = objas.DtoTransactionSearch(objReq);
                if (objDtoResponse.DtoTransactions.Count() > 0)
                {
                    
                    ResEmdeonAssistant = true;
                    //log.ExceptionDetails(_VendorName + "  emdeon.assistant - found a record. EBR ID: " + Convert.ToString(_EBRID) + ", Status: " + objDtoResponse.StatusMessage, _EBRID);
                    for (x = 0; x <= objDtoResponse.DtoTransactions.Length ; ++x)
                    {
                        _EmdReq = "";
                        _EmdRes = "";
                        _EmdReq = objDtoResponse.DtoTransactions[x].RequestString;
                        _EmdRes = objDtoResponse.DtoTransactions[x].ResponseString;
                        //log.ExceptionDetails(_VendorName + "  Loop: " + Convert.ToString(x) + ", Req: " + _EmdReq, _EmdRes);
                        i = Import("EMDEONLookup", true);
                        if (i < 0)
                        {
                            log.ExceptionDetails(_VendorName + "  emdeon.assistant.Import process Failed : " + Convert.ToString(x),"Import ProcessFailed");
                        }
                    }
                    
                    i = 0;

                    using (AuditResponseLogging arl = new AuditResponseLogging())
                    {
                        arl.ConnectionString = _ConnectionString;
                        arl.EmdeonSmartDelete(GlobalBatchID, _EBRID);
                    }
                    //if (objDtoResponse.StatusMessage == "OK")
                    //{

                    //    //log.ExceptionDetails(_VendorName + "  emdeon.assistant - found a record. EBR ID: " + Convert.ToString(_EBRID), _EBRID);

                    //    //EmdeonRequest = objDtoResponse.DtoTransactions[0].RequestString;
                    //}
                }
                else
                {
                    _EmdReq = "";
                    _EmdRes = "";
                    i = -3;
                    //log.ExceptionDetails(_VendorName + "  emdeon.assistant - Record Status for EBR ID: " + Convert.ToString(_EBRID) + ", Status: " + objDtoResponse.StatusMessage, _EBRID);
                }


               // _RES = _EmdRes;

            }
            catch (Exception ex)
            {
                i = -2;
               
                Endtime = DateTime.Now;
                _elapsedTicks = Endtime.Ticks - StartTime.Ticks;
                log.ExceptionDetails("GetResponse -EmdeonAssistant  Failed Total Elasped ticks" + Convert.ToString(_elapsedTicks) + " Service time out  | " + _ServiceTimeOut + " | " + _VendorName + "|" + _PayorCode + "|" + _wsUrl + "|" + _EmdReq, ex);

                //EmdeonResponse = "";
                // ex.Message
            }
            return i;
        }

        private int Import(string VendorName,bool IsEmdeonLookup)
        {
            int i = 0;
            long TransactionID = 0;
           
            string BHT03_req = string.Empty;
            string BHT03_res = string.Empty;
            string RES_TransactionSetIdentifierCode = string.Empty;
            string REQ_TransactionSetIdentifierCode = string.Empty;
            string AAAFailureCode = string.Empty;
            string NPI = string.Empty;
            string ServiceTypeCode = string.Empty;
            bool isISA = false;
            bool DoParseFlag = false; //we set this to true or false afer every step and it mus be true to do the next step
            bool doRESValidation = true;
            GlobalBatchID = 0;

            string sub = _EmdReq.Substring(0, 3);

            if (IsEmdeonLookup == true)
            {

            }
            else
            {
                TransactionID = _BatchID;
                GlobalBatchID = TransactionID;
            }

            try
            {

                //Log Req & Res

                try
                {
                    if (IsEmdeonLookup == true)
                    {
                        using (AuditResponseLogging arl = new AuditResponseLogging())
                        {
                            //log.ExceptionDetails("AuditResponseLogging for Batch ID: " + Convert.ToString(_BatchID) + " for the Vendor: " + VendorName + " with Response is : ", _RES);
                            arl.REQ = _EmdReq;
                            arl.RES = _EmdRes;
                            arl.PatientHospitalCode = _PatientHospitalCode;
                            arl.ConnectionString = _ConnectionString;
                            arl.InsertEmdeonAssistantReqRes();
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.ExceptionDetails("21: " + _AppName + " EMDEON INSERT REQ-RES failed for EBR ID: " + Convert.ToString(_EBRID), "sr1", "INSERT REQ-RES", _TaskID, 749);
                }


                //Allways Call Import 270 for both Emdeon & Emdeon Lookup
                //Because we are not taking care of this in Process Eligibility
                if (IsEmdeonLookup == true)
                {
                    try
                    {
                        using (Parse imp = new Parse())
                        {
                            imp.ConnectionString = _ConnectionString;
                            TransactionID = imp.Import270(_EmdReq, "N", Convert.ToDouble(_EBRID), _LoginUserID, _Hosp_Code, "Console", _PayorCode, VendorName, _InsType, _PatAcctNum, _PatientHospitalCode, GlobalBatchID);
                            GlobalBatchID = TransactionID;
                            DoParseFlag = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        DoParseFlag = false;
                        log.ExceptionDetails("6: " + _AppName + " EMDEON parse 270 failes for ", "sr1", Convert.ToString(TransactionID) + " 270 parse Failed", _TaskID, 749);
                    }
                }


                //BHT REQUEST
                try
                {

                    if (sub == "ISA")
                    {
                        isISA = true;
                        using (ValidateEDI vedi = new ValidateEDI())
                        {
                            vedi.ConnectionString = _ConnectionString;
                            vedi.byString(_EmdReq);
                            //_TransactionSetIdentifierCode = vedi.TransactionSetIdentifierCode;
                            BHT03_req = vedi.ReferenceIdentification;
                            REQ_TransactionSetIdentifierCode = vedi.TransactionSetIdentifierCode;
                            NPI = vedi.NPI;
                            ServiceTypeCode = vedi.ServiceTypeCode;
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.ExceptionDetails("20: " + _AppName + " EMDEON BHT -REQUEST ReferenceIdentification failed for EBR ID: " + Convert.ToString(_EBRID), "sr1", "BHT REQ Parse", _TaskID, 749);
                }

                 
                //BHT RESPONSE
                try
                {
                    using (ValidateEDI vedi = new ValidateEDI())
                    {
                        vedi.ConnectionString = _ConnectionString;
                        vedi.byString(_EmdRes);
                        BHT03_res = vedi.ReferenceIdentification;
                        RES_TransactionSetIdentifierCode = vedi.TransactionSetIdentifierCode;
                        AAAFailureCode = vedi.AAAFailureCode;
                        DoParseFlag = true;
                    }
                }
                catch (Exception ex)
                {
                    DoParseFlag = false;
                    log.ExceptionDetails("21: " + _AppName + " EMDEON BHT-RESPONSE ReferenceIdentification failed for EBR ID: " + Convert.ToString(_EBRID), "sr1", "BHT RES Parse", _TaskID, 749);
                }

                //insert into ELIGIBILITY_AUDIT_RESPONSE_TEMP 
                try
                {
                    using (AuditResponseLogging arl = new AuditResponseLogging())
                    {
                        //log.ExceptionDetails("AuditResponseLogging for Batch ID: " + Convert.ToString(_BatchID) + " for the Vendor: " + VendorName + " with Response is : ", _RES);
                        arl.ConnectionString = _ConnectionString;
                        arl.Log271(TransactionID, _PayorCode, _EmdRes, _EBRID, VendorName, AAAFailureCode, NPI, ServiceTypeCode);

                    }

                }
                catch (Exception ex)
                {
                    log.ExceptionDetails("6: " + _AppName + " insert into ELIGIBILITY_AUDIT_RESPONSE_TEMP  failes for ", "sr1", Convert.ToString(TransactionID) + " 270 parse Failed", _TaskID, 749);
                }
  
                //ISA Segment validation
                if (!_EmdRes.Contains("ISA"))
                {
                    DoParseFlag = false;
                    //GlobalBatchID = -7;
                    log.ExceptionDetails("11: " + _AppName + "EMDEON Raw check failed for : strRetRequestTransID " + Convert.ToString(TransactionID), _EmdRes);
                }

                //Length  validation
                if (_EmdRes.Length < 109)
                {

                    DoParseFlag = false;
                    //GlobalBatchID = -6;
                    log.ExceptionDetails("11: " + _AppName + "EMDEON RES is not at least 109 : strRetRequestTransID " + Convert.ToString(TransactionID), _EmdRes);
                }


                //TA1 validation
                if (RES_TransactionSetIdentifierCode == "TA1")
                {
                    //i = -3;
                    log.ExceptionDetails("1191-DCSGlobal.EDI.Comunications.EDI.EMDEON.Ex. Batch ID:" + Convert.ToString(TransactionID), "Vendor Returned TA1");

                }

            

                if (DoParseFlag == true)
                {
                    if (REQ_TransactionSetIdentifierCode == "270")
                    {
                        if (isISA)
                        {
                            if (BHT03_res == BHT03_req)
                            {
                                DoParseFlag = true;
                            }
                            else
                            {
                                DoParseFlag = false;
                                i = -5;
                                GlobalBatchID = -5;
                                // this is a  real mismatch so log it change to something useful
                                log.ExceptionDetails("1190-DCSGlobal.EDI.Comunications.EDI.EMDEON.Ex. Batch ID:" + Convert.ToString(TransactionID), "BHT  mismatch BREQ '" + _EmdReq + "'" + " B_RES '" + _EmdRes + "'");

                            }
                        }
                        else
                        {
                            i = -6;
                            DoParseFlag = false;
                            log.ExceptionDetails("1190-DCSGlobal.EDI.Comunications.EDI.EMDEON.Ex. Batch ID:" + Convert.ToString(TransactionID), "BHT  mismatch BREQ '" + _EmdReq + "'" + " B_RES '" + _EmdRes + "'");
                        }

                    }
                    else
                    {
                        DoParseFlag = false;
                        i = -8;
                        log.ExceptionDetails("1191-DCSGlobal.EDI.Comunications.EDI.EMDEON.Ex. Batch ID:" + Convert.ToString(TransactionID), " expected 270 but  got '" + REQ_TransactionSetIdentifierCode + " '  '" + _EmdReq);
                    }
                }
                  
                //Call Import 271
                try
                {
                    if (DoParseFlag == true)
                    {
                        using (Parse imp = new Parse())
                        {
                            imp.ConnectionString = _ConnectionString;
                            GlobalBatchID = imp.Import271(_EmdRes, "N", _EBRID, _LoginUserID, _Hosp_Code, "Console", _PayorCode, VendorName, "Y", TransactionID);
                           
                        }
                        DoParseFlag = true;
                    }
                    
                }
                catch(Exception ex)
                {
                      GlobalBatchID = -8;
                      DoParseFlag = false;
                      log.ExceptionDetails("14: " + _AppName + "EMDEON parse 271 failes for " + Convert.ToString(GlobalBatchID), _EmdRes);
                }

              
                

            }
            catch (Exception ex)
            {

                i = -2;
            }

            return i;
        
        }
        private int ImportWithNoResponse(string VendorName, bool IsEmdeonLookup)
        {
            int i = -5;
            long TransactionID = 0;

            string BHT03_req = string.Empty;
            string BHT03_res = string.Empty;
            string RES_TransactionSetIdentifierCode = string.Empty;
            string REQ_TransactionSetIdentifierCode = string.Empty;
            string AAAFailureCode = string.Empty;
            string NPI = string.Empty;
            string ServiceTypeCode = string.Empty;
            bool isISA = false;
            bool DoParseFlag = false; //we set this to true or false afer every step and it mus be true to do the next step
            bool doRESValidation = true;
            GlobalBatchID = 0;

            string sub = _EmdReq.Substring(0, 3);

            try
            {

                if (IsEmdeonLookup == true)
                {

                }
                else
                {
                    TransactionID = _BatchID;
                    GlobalBatchID = TransactionID;
                }
               
                //Because we are taking care of this in Process Eligibility
                //try
                //{
                //    using (Parse imp = new Parse())
                //    {
                //        imp.ConnectionString = _ConnectionString;
                //        TransactionID = imp.Import270(_EmdReq, "N", Convert.ToDouble(_EBRID), _LoginUserID, _Hosp_Code, "Console", _PayorCode, VendorName, _InsType, _PatAcctNum, _PatientHospitalCode, GlobalBatchID);
                //        GlobalBatchID = TransactionID;
                //        DoParseFlag = true;
                //    }
                //}
                //catch (Exception ex)
                //{
                //    DoParseFlag = false;
                //    log.ExceptionDetails("6: " + _AppName + " EMDEON parse 270 failes for ", "sr1", Convert.ToString(TransactionID) + " 270 parse Failed", _TaskID, 749);
                //}


                //BHT REQUEST
                try
                {

                    if (sub == "ISA")
                    {
                        isISA = true;
                        using (ValidateEDI vedi = new ValidateEDI())
                        {
                            vedi.ConnectionString = _ConnectionString;
                            vedi.byString(_EmdReq);
                            //_TransactionSetIdentifierCode = vedi.TransactionSetIdentifierCode;
                            BHT03_req = vedi.ReferenceIdentification;
                            REQ_TransactionSetIdentifierCode = vedi.TransactionSetIdentifierCode;
                            NPI = vedi.NPI;
                            ServiceTypeCode = vedi.ServiceTypeCode;
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.ExceptionDetails("20: " + _AppName + " EMDEON BHT -REQUEST ReferenceIdentification failed for EBR ID: " + Convert.ToString(_EBRID), "sr1", "BHT REQ Parse", _TaskID, 749);
                }

                //Call No Response and insert into RES
                norespid = -1;
                using (NoResponse nr = new NoResponse())
                {
                    nr.ConnectionString = _ConnectionString;
                    norespid = nr.LogNoResponse(TransactionID);
                    if (norespid == 0)
                    {
                        _RES = nr.RES;
                        _EmdRes = _RES;
                        i = 0;
                    }
                    else
                    {
                        log.ExceptionDetails(_VendorName + "Traped Execption in NORESPONSE With Batch ID " + Convert.ToString(TransactionID), " RES:" + _RES);
                    }
                }

                //BHT RESPONSE
                try
                {
                    using (ValidateEDI vedi = new ValidateEDI())
                    {
                        vedi.ConnectionString = _ConnectionString;
                        vedi.byString(_EmdRes);
                        BHT03_res = vedi.ReferenceIdentification;
                        RES_TransactionSetIdentifierCode = vedi.TransactionSetIdentifierCode;
                        AAAFailureCode = vedi.AAAFailureCode;
                        DoParseFlag = true;
                    }
                }
                catch (Exception ex)
                {
                    DoParseFlag = false;
                    log.ExceptionDetails("21: " + _AppName + " EMDEON BHT-RESPONSE ReferenceIdentification failed for EBR ID: " + Convert.ToString(_EBRID), "sr1", "BHT RES Parse", _TaskID, 749);
                }

                //insert into ELIGIBILITY_AUDIT_RESPONSE_TEMP 
                try
                {
                    using (AuditResponseLogging arl = new AuditResponseLogging())
                    {
                        //log.ExceptionDetails("AuditResponseLogging for Batch ID: " + Convert.ToString(_BatchID) + " for the Vendor: " + VendorName + " with Response is : ", _RES);
                        arl.ConnectionString = _ConnectionString;
                        arl.Log271(TransactionID, _PayorCode, _EmdRes, _EBRID, VendorName, AAAFailureCode, NPI, ServiceTypeCode);
                    }

                }
                catch (Exception ex)
                {
                    log.ExceptionDetails("6: " + _AppName + " insert into ELIGIBILITY_AUDIT_RESPONSE_TEMP  failes for ", "sr1", Convert.ToString(TransactionID) + " 270 parse Failed", _TaskID, 749);
                }

                //Call Import 271
                try
                {
                    using (Parse imp = new Parse())
                    {
                        imp.ConnectionString = _ConnectionString;
                        GlobalBatchID = imp.Import271(_EmdRes, "N", _EBRID, _LoginUserID, _Hosp_Code, "Console", _PayorCode, VendorName, "Y", TransactionID);
                    }
                        
                     

                }
                catch (Exception ex)
                {
                    GlobalBatchID = -8;
                    DoParseFlag = false;
                    log.ExceptionDetails("14: " + _AppName + "EMDEON parse 271 failes for " + Convert.ToString(GlobalBatchID), _EmdRes);
                }
 
            }
            catch (Exception ex)
            {

                i = -2;
            }

            return i;

        }
    }
}
