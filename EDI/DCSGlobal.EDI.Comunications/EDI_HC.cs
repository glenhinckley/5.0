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
        private string _ConnectionString = string.Empty;
   
        private string _REQ = string.Empty;
        private string _RES = string.Empty;
        private string _Accept = string.Empty;
  
        private string _ParameterFilePath = string.Empty;

        private string _SYNC_TIMEOUT = string.Empty;
        private string _SUBMISSION_TIMEOUT = string.Empty;
        private string _CommandTimeOut = "90";


         
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
            ss = null;
            _disposed = true;
        }


        public string SYNC_TIMEOUT
        {
        
            set
            {
                _SYNC_TIMEOUT = value;
            }
        }

        public string SUBMISSION_TIMEOUT
        {
           
            set
            {
                _SUBMISSION_TIMEOUT = value;
            }
        }
        public string CommandTimeOut
        {
          
            set
            {
                _CommandTimeOut = value;
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

 

        public int getEligVendorEdiResponse(string RequestContent, string PayorCode, string VendorName, string vendor_input_type)
        {

            int r = -1;
             
            _REQ = RequestContent;
            _RES = String.Empty;

            if (!string.IsNullOrEmpty(_ConnectionString))
            {
                log.ConnectionString = _ConnectionString;
                if (_Verbose)
                {
                    log.ExceptionDetails("1-The Given File Path is:",  _ParameterFilePath + ":" + VendorName.Trim());
                }

            }
            switch (VendorName)
            {
                case "AVAILITY":

                    using (AVAILITY avlt = new AVAILITY())
                    {
                        avlt.VendorName = VendorName;
                        avlt.PayorCode = PayorCode;
                        avlt.ConnectionString = _ConnectionString;
                        avlt.wsUrl = "https://qa-apps.availity.com/availity/B2BHCTransactionServlet";
                        avlt.Method = "POST";
                        avlt.ContentType = "application/x-www-form-urlencoded";
                        avlt.REQ = RequestContent;
                        avlt.ServiceTimeOut = 120000;
                        r = avlt.GetResponse();
                        if (r == 0)
                        {
                           
                            _RES = avlt.RES;
                             
                        }
                        else 
                        {
                            r = -1;
                            log.ExceptionDetails("DCSGlobal.EDI.Comunications.EDI.AVAILITY.GetResponse Failed", "GetResponse Failed");
                        }
                        

                    }


                    break;


                case "IVANS":
                    using (IVANS ivns = new IVANS())
                    {
                        ivns.VendorName = VendorName;
                        ivns.PayorCode = PayorCode;
                        ivns.ConnectionString = _ConnectionString;
                        ivns.wsdl = "https://eligibilityone.abilitynetwork.com/EligibilityOne.asmx";
                        ivns.UserName = "8DC00005";
                        ivns.Passwd = "DC$Gl0bal//2013";
                        ivns.ClientId = "f9e52687-a2e0-4d9c-8591-59516c9fd026";
                        ivns.PreAuthenticate = true;
                        ivns.Method = "POST";
                        ivns.ContentType = "";
                        ivns.ServiceTimeOut = 120000;
                        ivns.REQ = RequestContent;
                        r = ivns.GetResponse();
                        if (r == 0)
                        {
                            _RES = ivns.RES;
                            
                        }
                        else
                        {
                            r = -1;
                            log.ExceptionDetails("DCSGlobal.EDI.Comunications.EDI.IVANS.GetResponse Failed", "GetResponse Failed");
                        }

                    }

                    break;
                case "VISIONSHARE":
                    using (VISIONSHARE vision = new VISIONSHARE())
                    {
                        vision.VendorName = VendorName;
                        vision.PayorCode = PayorCode;
                        vision.ConnectionString = _ConnectionString;

                        vision.Connection = "SYNC_WELLPOINT";
                        vision.Connection1 = "SYNC_HETS_PROD";

                        vision.UserName = "dcsgwpnt";
                        vision.Passwd = "Dx9m3fHvy";

                        vision.UserName1 = "dcsghets";
                        vision.Passwd1 = "dcsg821z";

                        vision.VS_SES_PORT = 4090;
                        vision.VS_SES_PORT1 = 4091;

                        vision.VS_PROXY_PORT = 80;

                        vision.VS_PROXY_SERVER = "";
                        vision.VS_HOST_ADDRESS = "204.11.46.227";

                        vision.REQ = _REQ;
                        r = vision.GetResponse();
                        if (r == 0)
                        {
                            _RES = vision.RES;
                             
                        }
                        else
                        {
                            r = -1;
                            log.ExceptionDetails("DCSGlobal.EDI.Comunications.EDI.VISIONSHARE.GetResponse Failed", "GetResponse Failed");
                        }
 
                    }
                    break;



                case "POST-N-TRACK":
                case "PNT":

                    using (PNT pnt = new PNT())
                    {
                        pnt.VendorName = VendorName;
                        pnt.PayorCode = PayorCode;
                        pnt.ConnectionString = _ConnectionString;
                        pnt.wsUrl = "https://realtime1.post-n-track.com/realtime/request_x12.aspx";
                        pnt.Method = "POST";
                        pnt.ContentType = "application/x-www-form-urlencoded";

                        pnt.REQ = RequestContent;

                        r = pnt.GetResponse();
                        if (r == 0)
                        {
                            _RES = pnt.RES;
                        }
                        else
                        {
                            r = -1;
                            log.ExceptionDetails("DCSGlobal.EDI.Comunications.EDI.PNT.GetResponse Failed", "GetResponse Failed");

                        }
                    }

                    break;
                     
                case "EMDEON":

                    using (EMDEON emd = new EMDEON())
                    {
                        emd.VendorName = VendorName;
                        emd.PayorCode = PayorCode;
                        emd.ConnectionString = _ConnectionString;
                        emd.REQ = RequestContent;
                        emd.ServiceTimeOut = 240000;
                        emd.UserName = "PMNADCSGL";
                        emd.Passwd = "TMURwvb3";
                        emd.wsUrl = "https://ra.emdeon.com/astwebservice/aws.asmx";
                        r = emd.GetResponse();
                        if (r == 0)
                        {
                            _RES = emd.RES;
                        }
                        else
                        {
                            r = -1;
                            log.ExceptionDetails("DCSGlobal.EDI.Comunications.EDI.EMDEON.GetResponse Failed", "GetResponse Failed");
                        }
                        
                    }

                    break;

                case "DORADO":

                    using (DoradoSystems dor = new DoradoSystems())
                    {
                        dor.VendorName = VendorName;
                        dor.PayorCode = PayorCode;
                        dor.ConnectionString = _ConnectionString;
                        dor.wsUrl = "https://api.doradosystems.com/rt/validate?api_key=yccQdicTcceNbLQbPesciCrKH";
                        dor.ContentType = "application/x-www-form-urlencoded";
                        dor.Method = "POST";
                        dor.REQ = RequestContent;
                        r = dor.GetResponse();
                        if (r == 0)
                        {
                            _RES = dor.RES;
                        }
                        else
                        {
                            r = -1;
                            log.ExceptionDetails("DCSGlobal.EDI.Comunications.EDI.DoradoSystems.GetResponse Failed", "GetResponse Failed");
                        }
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
                        pmount.wsUrl = "https://phcedi.promedica.org:50043/x12/realtime/soap?request_type=X12_270_Request_005010X279A1";
                        pmount.ContentType = "text/xml;charset=\"utf-8\"";
                        pmount.Method = "POST";
                        pmount.Accept = "text/xml";
                        pmount.REQ = RequestContent;
                        pmount.ServiceTimeOut = 600000;
                        r = pmount.GetResponse();
                        //r = pmount.GetData(1);
                        if (r == 0)
                        {
                            _RES = pmount.RES;
                        }
                        else
                        {
                            r = -1;
                            log.ExceptionDetails("DCSGlobal.EDI.Comunications.EDI.PARAMOUNTHEALTH.GetResponse Failed", "GetResponse Failed");
                        }
                        
                    }
                    break;


                case "TXMCD":
                    using (TexasMedicaidHealthcarePartnership txs = new TexasMedicaidHealthcarePartnership())
                    {
                        txs.VendorName = VendorName;
                        txs.PayorCode = PayorCode;
                        txs.ConnectionString = _ConnectionString;
                        txs.wsUrl = "http://edi-web.tmhp.com/TMHP/Request";
                        txs.ContentType = "text/xml;charset=\"utf-8\"";
                        txs.Method = "POST";
                        txs.Accept = _Accept;
                        txs.ServiceTimeOut = 600000;
                        txs.REQ = RequestContent;
                        //log.ExceptionDetails("5.Submitting-GetResponse", Convert.ToString(_wsurl));
                        r = txs.GetResponse();
                        if (r == 0)
                        {
                            _RES = txs.RES;
                        }
                        else
                        {
                            r = -1;
                            log.ExceptionDetails("DCSGlobal.EDI.Comunications.EDI.TXMCD.GetResponse Failed", "GetResponse Failed");
                        
                        }
                        
                    }

                    break;
                case "MEDDATA":

                    using (MEDDATA md = new MEDDATA())
                    {
                        md.VendorName = VendorName;
                        md.PayorCode = PayorCode;
                        md.ConnectionString = _ConnectionString;
                        md.wsUrl = "https://services.meddatahealth.com/submissionportal/submissionportal.asmx?wsdl";
                        md.UserName = "2019275";
                        md.Passwd = "fT~H31]m";
                        md.RequestFormat = "FlatXml";
                        md.ResponseFormat = "Edi";
                        md.REQ = _REQ;
                        r = md.GetResponse();
                        if (r == 0)
                        {
                            _RES = md.RES;
                        }
                        else
                        {
                            r = -1;
                            log.ExceptionDetails("DCSGlobal.EDI.Comunications.EDI.MEDDATA.GetResponse Failed", "GetResponse Failed");
               
                        }
                        
                    }


                    break;

                case "SCRAPE":
                    r = -1;
                    _RES = "This is screen scrape record and will not process at this movement.";
                    log.ExceptionDetails("DCSGlobal.EDI.Comunications.EDI.MEDDATA.SCRAPE", "SCRAPE Record");

                    break;


                default:

                    r = -1;
                    _RES = "This is default Vendor. And it is not a valid Vendor name.";
                    log.ExceptionDetails("DCSGlobal.EDI.Comunications.EDI.default", "Vendor Name Not Found");
                    break;
            }
            //log.ExceptionDetails("EDI.Communication.GetResponse for " + VendorName + " is : ", _RES );
            return r;
        }

    }


}


