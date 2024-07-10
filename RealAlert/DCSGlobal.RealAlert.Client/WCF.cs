using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DCSGlobal.BusinessRules.Logging;

namespace DCSGlobal.RealAlert.Client
{
    public class WCF :  IDisposable
    {


        ~WCF()
        {
            Dispose(false);
        }


        public void Dispose()
        {
            //  Console.WriteLine("Dispose called from outside");
            // If this function is being called the user wants to release the
            // resources. lets call the Dispose which will do this for us.
            Dispose(true);

            // Now since we have done the cleanup already there is nothing left
            // for the Finalizer to do. So lets tell the GC not to call it later.
            GC.SuppressFinalize(this);
        }



        protected virtual void Dispose(bool disposing)
        {
            // Console.WriteLine("Actual Dispose called with a " + disposing.ToString());
            if (disposing == true)
            {
                //someone want the deterministic release of all resources
                //Let us release all the managed resources
                ReleaseManagedResources();
            }
            else
            {
                // Do nothing, no one asked a dispose, the object went out of
                // scope and finalized is called so lets next round of GC 
                // release these resources
            }

            // Release the unmanaged resource in any case as they will not be 
            // released by GC
            ReleaseUnmangedResources();

        }

        void ReleaseManagedResources()
        {
            //      Console.WriteLine("Releasing Managed Resources");
            //if (tr != null)
            {
                // tr.Dispose();
            }
        }

        void ReleaseUnmangedResources()
        {
            // Console.WriteLine("Releasing Unmanaged Resources");
        }





        private string _EnpointURI = string.Empty;
        private string _ConnectionString = string.Empty;



        private static logExecption log = new logExecption();


        public string ConnectionString
        {
            set
            {
                _ConnectionString = value;
                log.ConnectionString = value;
            }
        }



        public string GetRealMessages(string OperatorID, string HospCode)
        {

            string r = "ERR";

            //using (srvWCFRealAleart.SampleServiceClient s = new srvWCFRealAleart.SampleServiceClient())
            //{

            //    r = s.GetRealMessages(OperatorID, HospCode, OperatorID);


            //}

            return r;



        }


        public int Login(string OperatorID, string HospCode)
        {
            int r = -1;




            using (srv.SampleServiceClient s = new srv.SampleServiceClient())
            {

               r =  s.Login(OperatorID, HospCode);


            }


            return r;

        }

        public int KillList(string theString)
        {
            int r = -1;

            //using (srvWCFRealAleart.SampleServiceClient s = new srvWCFRealAleart.SampleServiceClient())
            //{

            //    r = s.KillList(theString);


            //}



            return r;

        }



    }
}
