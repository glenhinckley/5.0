using System;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;
using DCSGlobal.BusinessRules.Logging;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using DCSGlobal.EDI;


namespace DCSGlobal.EDI.Comunications
{
    public class ValidateRawRES : IDisposable
    {


        logExecption log = new logExecption();
        StringStuff ss = new StringStuff();

        private bool _Verbose = false;
        private string _ConnectionString = string.Empty;

        private string _REQ = string.Empty;
        private string _RES = string.Empty;
        private long _BatchID = 0;
        private string _EBRID = string.Empty;

        private string _TransactionSetIdentifierCode = string.Empty;


        private string B_REQ = string.Empty;
        private string B_RES = string.Empty;

        private int _TaskID = 0;

        private int _CommandTimeOut = 90;

        private string _XMLParameters = string.Empty;
        private bool _FallBack = false;
        private bool _AllowFallback = true;
        private bool _isTest = false;
        private bool doRESValidation = false;


        private bool _disposed;

        ~ValidateRawRES()
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


        public int CommandTimeOut
        {

            set
            {
                _CommandTimeOut = value;
                
         
            }
        }



        public int TaskID
        {

            set
            {
                _TaskID = value;
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

    }
}
