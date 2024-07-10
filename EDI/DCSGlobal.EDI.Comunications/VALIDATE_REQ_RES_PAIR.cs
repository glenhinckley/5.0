using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;


using DCSGlobal.BusinessRules.Logging;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using DCSGlobal.BusinessRules.CoreLibraryII;
using DCSGlobal.EDI;


namespace DCSGlobal.EDI.Comunications
{
    public class VALIDATE_REQ_RES_PAIR : IDisposable 
    {


        private logExecption log = new logExecption();
        private StringStuff ss = new StringStuff();

        bool _disposed;



        private string _ConnectionString = string.Empty;


        private string _REQ_TransactionSetIdentifierCode = string.Empty;
        private string _RES_TransactionSetIdentifierCode = string.Empty;

        private string _ErrorMessage = string.Empty;

        ~VALIDATE_REQ_RES_PAIR()
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


        public string ErrorMessage
        {

            get
            {

                return _ErrorMessage;
            }
        }


        public int WhatTheHellAmI(string REQ, string RES)
        {

            int i = 0;

            string BHT03_RES = String.Empty;
            string BHT03_REQ = String.Empty;
            bool isMissingSegments = false;
            bool isTA1 = false;
            bool isAAA = false;

            try
            {

                if (i == 0)
                {
                    if (RES == string.Empty)
                    {
                        i = 1;
                        _ErrorMessage = "No Res";
                    }
                }


                if (i == 0)
                {
                    if (!RES.Contains("ISA"))
                    {
                        i = 2;
                        _ErrorMessage = "No ISA";
                    }
                }



                if (i == 0)
                {
                    if (RES.Length < 109)
                    {
                        i = 3;
                        _ErrorMessage = "RES TO SHORT";
                    }
                }


                if (i == 0)
                {
                    if (!RES.Contains("IEA"))
                    {
                        i = 4;
                        _ErrorMessage = "No IEA";
                    }
                }


                if (i == 0)
                {
                    using (EDI_5010_VALIDATE vedi = new EDI_5010_VALIDATE())
                    {
                        vedi.ConnectionString = _ConnectionString;
                        vedi.byString(REQ);
                        BHT03_REQ = vedi._BHT03_ReferenceIdentification;
                        _REQ_TransactionSetIdentifierCode = vedi.TransactionSetIdentifierCode;
                        //AAAFailureCode = vedi.AAAFailureCode;
                    }

                }



                if (i == 0)
                {
                    using (EDI_5010_VALIDATE vedi = new EDI_5010_VALIDATE())
                    {
                        vedi.ConnectionString = _ConnectionString;
                        vedi.byString(RES);
                        BHT03_RES = vedi._BHT03_ReferenceIdentification;
                        _RES_TransactionSetIdentifierCode = vedi.TransactionSetIdentifierCode;
                        isTA1 = vedi.isTA1;
                        isAAA = vedi.isAAA;
                        isMissingSegments = vedi.isMissingSegments;
                    }

                }


                if (i == 0)
                {
                    if (isMissingSegments)
                    {
                        i = -7;
                        _ErrorMessage = "Has Missing Segments";
                    }
                }

                if (i == 0)
                {
                    if (isTA1)
                    {
                        i = -3;
                        _ErrorMessage = "Has TA1";
                    }
                }


                if (i == 0)
                {
                    if (_RES_TransactionSetIdentifierCode == "999")
                    {
                        i = -999;

                    }
                }

                if (i == 0)
                {
                    if (_RES_TransactionSetIdentifierCode == "997")
                    {
                        i = -997;

                    }
                }

                if (i == 0)
                {
                    if (BHT03_RES != BHT03_REQ)
                    {

                        if (_RES_TransactionSetIdentifierCode == "271" && isAAA)
                        {
                            _ErrorMessage = "271 AAA";
                            i = -4;

                        }
                        else
                        {
                            i = -5;
                            _ErrorMessage = "BHT03 Mismatch";
                        }
                    }
                    else
                    {
                        i = 0;
                    }
                }


                if (i == 0)
                {
                    if (isAAA)
                    {
                        i = -6;
                        _ErrorMessage = "Has AAA";
                    }
                }



            }

            catch(Exception ex)
            {
                i = -666;
                _ErrorMessage = "DOOM AND DESPAIR LOOK IN ED";
                log.ExceptionDetails("COMUNICATIONS.DLL", "RESPONSE_PARSER", "WhatTheHellAmI", "", ex);

            }


            return i;

        }



    }
}
