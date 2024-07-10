using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Configuration;
using System.Data;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Data.SqlClient;

using DCSGlobal.Comunications.IPC.NetPipes;
using DCSGlobal.BusinessRules.Logging;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;

using System.IO.Pipes;
using System.IO;


using System.Diagnostics;  //  For the StackTrace.
using System.Xml.Linq;
using System.Xml;





using System.Runtime.InteropServices;   //GuidAttribute
using System.Reflection;                //Assembly
using System.Security.AccessControl;    //MutexAccessRule
using System.Security.Principal;        //SecurityIdentifier


namespace DCSGlobal.RealAlert.Server
{

    public class RealAlertListnerNamedPipe : IDisposable
    {

        public void Dispose()
        {
            Console.WriteLine("Dispose called from outside");
            // If this function is being called the user wants to release the
            // resources. lets call the Dispose which will do this for us.
            Dispose(true);

            // Now since we have done the cleanup already there is nothing left
            // for the Finalizer to do. So lets tell the GC not to call it later.
            GC.SuppressFinalize(this);
        }



        protected virtual void Dispose(bool disposing)
        {
            Console.WriteLine("Actual Dispose called with a " + disposing.ToString());
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
          //  Console.WriteLine("Releasing Managed Resources");
            //if (tr != null)
            {
                // tr.Dispose();
            }
        }

        void ReleaseUnmangedResources()
        {
          //  Console.WriteLine("Releasing Unmanaged Resources");
        }
        //static 

        static logExecption log = new logExecption();
       // static logFile lof = new logFile();

        static DataTable TheTable = new DataTable();
        static Boolean LockServer = true;
        static string _AppName;
        private static System.Timers.Timer aTimer;
        //  private static System.Timers.Timer GCTimer;
        static string _client_code = string.Empty;
        static string _ConnectionString;
        static int _RefreshRate = 20000;
        static string _InstanceName = "TestMode";
        static bool NoData = true;




        private static bool _DropDead = false;



        private static Mutex _m;
        private static MutexAccessRule allowEveryoneRule = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow);
        private static MutexSecurity securitySettings = new MutexSecurity();
        private static bool createdNew;
        public static bool hasHandle = false;
        private static Mutex mutex;




        private static void DataRefreshTimerEvent(object source, ElapsedEventArgs e)
        {
            Console.WriteLine(" Refresh timer fired");
            if (!LockServer)
            {
                // start and update thread for TheTable
                Console.WriteLine(" starting thread to refresh data");
                Thread threadRefresh = new Thread(RefreshData);
                threadRefresh.Start();
                //    RefreshData();




            }


        }

        private static void GarbageCollectionTimerEvent(object source, ElapsedEventArgs e)
        {
            if (!LockServer)
            {
            }
        }






        public RealAlertListnerNamedPipe()     //Constructor
        {
            if (IsSingleInstance())
            {
                // Get Out already rungin
            }
        }

        ~RealAlertListnerNamedPipe()  // destructor
        {
            // flush the check point to disk
            // Shut down any runing threads
            // Last release the mutex and shut the instance down

            try
            {
                _m.ReleaseMutex();
                _m.Close();
            }
            catch (Exception ex)
            {


            }

        }




        public void Go()
        {


            // get the client setting
            // GetClientSettings();


            // seth up the table
            BuildDataTable();

            // Load the data into it.
            RefreshData();
            DumpTheTable();
            aTimer = new System.Timers.Timer(20000);


            // start and set and event timer to refresh the data
            aTimer.Elapsed += new ElapsedEventHandler(DataRefreshTimerEvent);

            // Set the Interval to the desired rate.
            aTimer.Interval = _RefreshRate;
            aTimer.Enabled = true;
            GC.KeepAlive(aTimer);




        }







        //
        //Summary:
        //The CreateNamedPipeServerToListen is used to create a named pipe as MyTestPipe.
        //To create the pipe, i have used NamedPipeServerStream class of .Net framework 4.x
        //PipeDirection.InOut : Specifies that the pipe direction is two-way.
        //NamedPipeServerStream.MaxAllowedServerInstances :  Represents the maximum number of server instances that the system resources allow
        private static void CreateNamedPipeServerToListen()
        {

            int threadSpecificData;
            threadSpecificData = Thread.CurrentThread.ManagedThreadId;

           // Console.WriteLine("Listener started for instace :" + _InstanceName + "  on htread :" + Convert.ToString(threadSpecificData));

            NamedPipeServerStream pipeServer =
               new NamedPipeServerStream(_InstanceName, PipeDirection.InOut, NamedPipeServerStream.MaxAllowedServerInstances);

            pipeServer.WaitForConnection();

           // Console.WriteLine("Client tried to cnonect");
            // before processing message run another thread yes this is recursive get over it.
            Thread thread = new Thread(CreateNamedPipeServerToListen);
            thread.Start();
            //Process the incoming request from the connected client
           // ProcessMessage(pipeServer);
        }

        private static void ProcessMessage(NamedPipeServerStream pipeServer)
        {


            int threadSpecificData;
            threadSpecificData = Thread.CurrentThread.ManagedThreadId;

         //   Console.WriteLine("ProcessMessage is lsiting on thread number:  " + Convert.ToString(threadSpecificData));
            string _Operator_ID = string.Empty;

            try
            {

                StreamString sss = new StreamString(pipeServer);
                // DataTable TempTable = new DataTable();

                // get the client operator ID

                _Operator_ID = sss.ReadString();
           //     Console.WriteLine("_Operator_ID :" + _Operator_ID);




                if (_Operator_ID != String.Empty)
                {


                    if (!LockServer)
                    {
                        DataView dvTheTable = TheTable.DefaultView;

                        dvTheTable.RowFilter = "operator_id = '" + _Operator_ID + "'";
                        dvTheTable.Sort = "id";

                        // DataTable dtNI = dvTheTable.ToTable("NEwsss", true, "operator_id");
                        // DataView dvNI = new DataView(dtNI);
                        string response = string.Empty;
                        int MsgCount = TheTable.Rows.Count;
                        for (int ii = 0; ii < dvTheTable.Count; ii++)
                        {


                            if (ii == 0)
                            { response = "RES" + Convert.ToString((dvTheTable[ii]["paitent_number"])); }
                            else
                            { response = response + Convert.ToString((dvTheTable[ii]["paitent_number"])); }

                            response = response + "|" + Convert.ToString((dvTheTable[ii]["facility_code"]));
                            response = response + "|" + Convert.ToString((dvTheTable[ii]["PID"]));
                            response = response + "|" + Convert.ToString((dvTheTable[ii]["DT"]));
                            response = response + "|" + Convert.ToString((dvTheTable[ii]["pat_hosp_code"]));
                            response = response + "|" + Convert.ToString((dvTheTable[ii]["PatName"]));
                            response = response + "|" + Convert.ToString((dvTheTable[ii]["id"]));
                            response = response + "|" + Convert.ToString((dvTheTable[ii]["operator_id"]));
                            response = response + "~";
                            // MsgCount = ii;
                            if (ii > 100)
                                break;
                        }





                        response = response + "^";

                        //  response = Convert.ToString(dvNI.Count);

                        sss.WriteString(Convert.ToString(response));

                    }
                    else
                    {
                        sss.WriteString("WAT");
                    }



                }
                else
                {

                    sss.WriteString("ONF");

                }





                //    sss.WriteString(SendNextPacket());

            }
            catch (IOException e)
            {

                log.ExceptionDetails(_InstanceName, Convert.ToString(e));

            }
            catch (Exception ex)
            {

                log.ExceptionDetails(_InstanceName, ex);
            }

            pipeServer.Close();
        }



        static bool IsSingleInstance()
        {
            try
            {
                // Try to open existing mutex.
                Mutex.OpenExisting(_InstanceName);
                //exit thread

                //Environment.Exit(0);
            }
            catch
            {
                // If exception occurred, there is no such mutex.
                _m = new Mutex(true, _InstanceName);

                // Only one instance.
                return true;
            }
            // More than one instance.
            return false;
        }


        static public void BuildDataTable()
        {



            TheTable.Columns.Add("paitent_number", typeof(string));
            TheTable.Columns.Add("facility_code", typeof(string));
            TheTable.Columns.Add("PID", typeof(Int32));
            TheTable.Columns.Add("DT", typeof(DateTime));
            TheTable.Columns.Add("pat_hosp_code", typeof(string));
            TheTable.Columns.Add("PatName", typeof(string));
            TheTable.Columns.Add("id", typeof(Int32));
            TheTable.Columns.Add("operator_id", typeof(string));


        }

        //refresh data



        static public void DumpTheTable()
        {

            DataView dvTheTable = TheTable.DefaultView;

            //  dvTheTable.RowFilter = "operator_id = 'CDELRIO'";
            dvTheTable.Sort = "id";

            //  DataTable dtNI = dvTheTable.ToTable("NEwsss", true, "operator_id");
            //  DataView dvNI = new DataView(dtNI);
            string response = string.Empty;
            int MsgCount = TheTable.Rows.Count;
            for (int ii = 1; ii < dvTheTable.Count; ii++)
            {



                response = Convert.ToString((dvTheTable[ii]["paitent_number"]));
                response = response + "|" + Convert.ToString((dvTheTable[ii]["facility_code"]));
                response = response + "|" + Convert.ToString((dvTheTable[ii]["PID"]));
                response = response + "|" + Convert.ToString((dvTheTable[ii]["DT"]));
                response = response + "|" + Convert.ToString((dvTheTable[ii]["pat_hosp_code"]));
                response = response + "|" + Convert.ToString((dvTheTable[ii]["PatName"]));
                response = response + "|" + Convert.ToString((dvTheTable[ii]["id"]));
                response = response + "|" + Convert.ToString((dvTheTable[ii]["operator_id"]));
                Console.WriteLine(response);

                if (ii > 5000)
                    break;
            }



        }

        static public void RefreshData()
        {



            NoData = true;
   //         Console.WriteLine("Refresh Data Started");



            DataTable tmpTable = new DataTable();

            tmpTable.Clear();

            tmpTable.Columns.Add("paitent_number", typeof(string));
            tmpTable.Columns.Add("facility_code", typeof(string));
            tmpTable.Columns.Add("PID", typeof(Int32));
            tmpTable.Columns.Add("DT", typeof(DateTime));
            tmpTable.Columns.Add("pat_hosp_code", typeof(string));
            tmpTable.Columns.Add("PatName", typeof(string));
            tmpTable.Columns.Add("id", typeof(Int32));
            tmpTable.Columns.Add("operator_id", typeof(string));

            try
            {
                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {

                 



                    using (SqlCommand cmd = new SqlCommand("usp_real_alert_50", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@client_code", _client_code);
                        con.Open();
                        using (SqlDataReader idr = cmd.ExecuteReader())
                        {
                            if (idr.HasRows)
                            {

                                int x = 0;

                                while (idr.Read())
                                {
                                    DataRow nrow = tmpTable.NewRow();

                                    try
                                    {



                                        if (idr["PATIENT_NUMBER"] != System.DBNull.Value)
                                        {
                                            nrow["paitent_number"] = Convert.ToString(idr["PATIENT_NUMBER"]);
                                        }
                                        else
                                        {
                                            // objEligibility.RES_PAYER_NAME = NullText;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //  objEligibility.RES_PAYER_NAME = "Field Not found";


                                        log.ExceptionDetails((Convert.ToString(_AppName) + " PATIENT_NUMBER " + _client_code), ex);


                                    }





                                    try
                                    {



                                        if (idr["FACILITY_CODE"] != System.DBNull.Value)
                                        {
                                            nrow["facility_code"] = (string)idr["FACILITY_CODE"];
                                        }
                                        else
                                        {
                                            // objEligibility.RES_PAYER_NAME = NullText;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //  objEligibility.RES_PAYER_NAME = "Field Not found";


                                        log.ExceptionDetails((Convert.ToString(_AppName) + " FACILITY_CODE " + _client_code), ex);


                                    }



                                    try
                                    {



                                        if (idr["PID"] != System.DBNull.Value)
                                        {
                                            nrow["PID"] = (Int32)idr["PID"];
                                        }
                                        else
                                        {
                                            // objEligibility.RES_PAYER_NAME = NullText;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //  objEligibility.RES_PAYER_NAME = "Field Not found";


                                        log.ExceptionDetails((Convert.ToString(_AppName) + " PID " + _client_code), ex);


                                    }



                                    try
                                    {



                                        if (idr["DT"] != System.DBNull.Value)
                                        {
                                            nrow["DT"] = (DateTime)idr["DT"];
                                        }
                                        else
                                        {
                                            // objEligibility.RES_PAYER_NAME = NullText;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //  objEligibility.RES_PAYER_NAME = "Field Not found";


                                        log.ExceptionDetails((Convert.ToString(_AppName) + " DT " + _client_code), ex);


                                    }



                                    try
                                    {



                                        if (idr["PAT_HOSP_CODE"] != System.DBNull.Value)
                                        {
                                            nrow["pat_hosp_code"] = (string)idr["PAT_HOSP_CODE"];
                                        }
                                        else
                                        {
                                            // objEligibility.RES_PAYER_NAME = NullText;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //  objEligibility.RES_PAYER_NAME = "Field Not found";


                                        log.ExceptionDetails((Convert.ToString(_AppName) + " PAT_HOSP_CODE " + _client_code), ex);


                                    }



                                    try
                                    {



                                        if (idr["PATNAME"] != System.DBNull.Value)
                                        {
                                            nrow["PatName"] = (string)idr["PATNAME"];
                                        }
                                        else
                                        {
                                            // objEligibility.RES_PAYER_NAME = NullText;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //  objEligibility.RES_PAYER_NAME = "Field Not found";


                                        log.ExceptionDetails((Convert.ToString(_AppName) + " PATNAME " + _client_code), ex);


                                    }





                                    try
                                    {



                                        if (idr["ID"] != System.DBNull.Value)
                                        {
                                            nrow["id"] = (Int32)idr["Id"];
                                        }
                                        else
                                        {
                                            // objEligibility.RES_PAYER_NAME = NullText;
                                        }
                                    }

                                    catch (Exception ex)
                                    {
                                        //  objEligibility.RES_PAYER_NAME = "Field Not found";


                                        log.ExceptionDetails((Convert.ToString(_AppName) + " ID " + _client_code), ex);


                                    }


                                    try
                                    {



                                        if (idr["OPERATOR_ID"] != System.DBNull.Value)
                                        {
                                            nrow["operator_id"] = (string)idr["OPERATOR_ID"];
                                        }
                                        else
                                        {
                                            // objEligibility.RES_PAYER_NAME = NullText;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //  objEligibility.RES_PAYER_NAME = "Field Not found";


                                        log.ExceptionDetails((Convert.ToString(_AppName) + " OPERATOR_ID " + _client_code), ex);


                                    }




                                    tmpTable.Rows.Add(nrow);
                                    x++;
                                    if (x > 2000)
                                        break;
                                }
                            }


                        }
                        con.Close();

                    }
                }
                NoData = false;
            }
            catch  (SqlException sql)
            {
                log.ExceptionDetails((Convert.ToString(_AppName) + " db SQL error " + _client_code), sql);
               // Console.WriteLine("SQL erorr in table update: will retry in 20 secs " + sql.Message );
                NoData = true;
                Thread.CurrentThread.Abort();
            }

            catch (Exception ex)
            {
                log.ExceptionDetails((Convert.ToString(_AppName) + " db Execption error " + _client_code), ex);
            //    Console.WriteLine("Exception erorr in table update:  will retry in 20 secs " + ex.Message);
                NoData = true;
                Thread.CurrentThread.Abort();
            }
         //   Console.WriteLine("Temp Table Cleared");


            if (!NoData)
            {
                LockServer = true;                                                                 
            //    Console.WriteLine("Lock server = true");

                if (LockServer)
                {


                    try
                    {
                        TheTable.Clear();
                   //     Console.WriteLine("TheTable Clear sucseded");

                        TheTable = tmpTable.Copy();

                   //     Console.WriteLine("TheTable copy sucseded");


                    }

                    catch (Exception ex)
                    {
                        LockServer = false;
                        log.ExceptionDetails((Convert.ToString(_AppName) + " clearing table Execption error " + _client_code), ex);
                   //     Console.WriteLine("TheTable Clear  failed :   aborting thread will retry in 20 secs");
                        Thread.CurrentThread.Abort();
                    }

                }
            }






        //    Console.WriteLine("Refresh Complete");
            LockServer = false;
         //   Console.WriteLine("Lock server = false");

         //   Console.WriteLine("Refresh Data comlete");
        }





        private static void KillPIDs()
        {

            //USP_REAL_ALERT_50_CLIENT @INPUT VARCHAR(1000)

            //USP_REAL_ALERT_50_CLIENT '40010516016|JHS|766203|2015-03-04 04:45:05.200|40010516016-JHS|MERCEDES MONTERREY|4348215|ANDREA.BASTIDAS'



            SqlConnection db = new SqlConnection(_ConnectionString);
            SqlTransaction transaction;

            db.Open();
            transaction = db.BeginTransaction();
            try
            {


                // foreach(r in rows) 
                {
                    new SqlCommand("USP_REAL_ALERT_50_CLIENT " + "", db, transaction).ExecuteNonQuery();
                }


                transaction.Commit();
            }
            catch (SqlException sqlError)
            {
                transaction.Rollback();
                //sqlError.

            }
            catch (Exception ex)
            {
                transaction.Rollback();
            }

            db.Close();






        }




        public string ConnectionString
        {
      
            set
            {
                _ConnectionString = value;
                log.ConnectionString = value;

            }
        }

        public string InstanceName
        {
   
            set
            {
                _InstanceName = value;
            }
        }



        public int RefreshRate
        {

            set
            {
                _RefreshRate = value;
            }

        }


        public string client_code
        {
            get
            {
                return _client_code;
            }
            set
            {
                _client_code = value;
            }
        }





        public bool Shutdown
        {
            get
            {
                return _DropDead;
            }

        }


  



         
        static void InitMutex()
        {
            string appGuid = ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false).GetValue(0)).Value.ToString();
            string mutexId = string.Format("Global\\{{{0}}}", _InstanceName + appGuid);

            mutex = new Mutex(true, mutexId);

            var allowEveryoneRule = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow);
            var securitySettings = new MutexSecurity();
            securitySettings.AddAccessRule(allowEveryoneRule);
            mutex.SetAccessControl(securitySettings);

            var sid = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
            var mutexsecurity = new MutexSecurity();
            mutexsecurity.AddAccessRule(new MutexAccessRule(sid, MutexRights.FullControl, AccessControlType.Allow));
            mutexsecurity.AddAccessRule(new MutexAccessRule(sid, MutexRights.ChangePermissions, AccessControlType.Deny));
            mutexsecurity.AddAccessRule(new MutexAccessRule(sid, MutexRights.Delete, AccessControlType.Deny));
            //  _mutex = new Mutex(false, "Global\\YourAppName-{add-your-random-chars}", out created, mutexsecurity);


        }


        static void SingleGlobalInstance(int timeOut)
        {
            InitMutex();
            try
            {
                if (timeOut < 0)
                    hasHandle = mutex.WaitOne(Timeout.Infinite, false);
                else
                    hasHandle = mutex.WaitOne(timeOut, false);

                if (hasHandle == false)
                {
                    Environment.Exit(0);
                }
                //  throw new TimeoutException("Timeout waiting for exclusive access on SingleInstance");
            }
            catch (AbandonedMutexException)
            {
                hasHandle = true;
            }
        }







     }




}
