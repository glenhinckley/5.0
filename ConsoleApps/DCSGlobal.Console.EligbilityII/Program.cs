using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;
using System;


using System.Runtime.InteropServices;   //GuidAttribute
using System.Reflection;                //Assembly
using System.Security.AccessControl;    //MutexAccessRule
using System.Security.Principal;        //SecurityIdentifier


using System.Configuration;
using System.Data;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Data.SqlClient;

using System.IO.Pipes;


using DCSGlobal.BusinessRules.CoreLibraryII;
using DCSGlobal.BusinessRules.CoreLibrary;

using DCSGlobal.BusinessRules.Logging;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using DCSGlobal.Eligibility.ProcessEligibility;

namespace DCSGlobal.Console.EligbilityII
{
    class Program
    {


        private static int iEDIKey = 0;

        private static config c = new config();
        private static string _AppName = "DCSGlobal.Console.EligbilityII";


        private static logExecption log = new logExecption();
        private static ApplicationLog al = new ApplicationLog();
        private static LogToEventLog el = new LogToEventLog();

        private static StringStuff ss = new StringStuff();



        private static System.Timers.Timer _Recycletimer; // From System.Timers
        private static bool _Recycle = false;


        private static System.Timers.Timer _timer; // From System.Timers


        private static string _XMLParameters = string.Empty;

        private static string dbTempString = string.Empty;
        private static ManualResetEvent _quitEvent = new ManualResetEvent(false);



        private static CustomThreadPool MyPool;
        private static Dictionary<int, string> _dID = new Dictionary<int, string>();
        private static int JobCount = 0;
        private static int JobID = 0;
        private static int JobTimeCount = 0;

        private static Mutex _m;
        private static MutexAccessRule allowEveryoneRule = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow);
        private static MutexSecurity securitySettings = new MutexSecurity();
        private static bool createdNew;
        public static bool hasHandle = false;
        private static Mutex mutex;


        static void Main(string[] args)
        {
            //    var allowEveryoneRule = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow);
            //   var securitySettings = new MutexSecurity();
            //   securitySettings.AddAccessRule(allowEveryoneRule);

            GetAppConfig();
            //    IsSingleInstance();







            _AppName = c.ConsoleName;




            if (c.ForceClearTextPasswd == 1)
            {
                dbTempString = c.ConnectionString;
            }
            else
            {
                using (DBUtility dbu = new DBUtility())
                {
                    dbTempString = dbu.getConnectionString(c.ConnectionString);

                    if (dbTempString == "err")
                    {
                        if (dbu.ValidateConString(c.ConnectionString))
                        {
                            dbTempString = c.ConnectionString;
                        }
                        else
                        {
                            el.WriteEventError(_AppName + "  decode constring failed", 1);
                            Environment.Exit(0);
                        }
                    }
                }
            }






            // stops race condition needs to be about 3-5 secs
            SingleGlobalInstance(3000);

            if (!hasHandle)
            {


                Environment.Exit(0);

            }




                        using (SchedulerLog _SL = new SchedulerLog())
            {
                _SL.ConnectionString = dbTempString;
                JobID = _SL.AddLogEntry(_AppName, "EligibilityII");
            }



            if (GetXMLParameters() == -1)
            {
                System.Console.WriteLine("Failed to get _XMLParamters");
                Environment.Exit(0);
            }






            log.ConnectionString = dbTempString;
            al.ConnectionString = dbTempString;



            // recycle timer

            _Recycletimer = new System.Timers.Timer(c.RecycleWaitTime);
            _Recycletimer.Elapsed += new ElapsedEventHandler(_RecycleTimerElaspesed);
            _Recycletimer.Enabled = true; // Enable it

            // GoSingle();


            if (c.ThreadsON == "1")
            {
                MyPool = CustomThreadPool.Instance;

                MyPool.MinThreads = Convert.ToInt32(c.MinThreads);
                MyPool.MaxThreads = Convert.ToInt32(c.MaxThreads);
                MyPool.MinWait = Convert.ToInt32(c.MinWait);
                MyPool.MaxWait = Convert.ToInt32(c.MaxWait);
                MyPool.SchedulingInterval = Convert.ToInt32(c.SchedulingInterval);
                MyPool.CleanupInterval = Convert.ToInt32(c.CleanupInterval);

                Thread.Sleep(c.dbPollTimeMilliSeconeds);
                _timer = new System.Timers.Timer(c.PoolPollTime);
                _timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
                _timer.Enabled = true; // Enable it



                GO();

                _quitEvent.WaitOne();

            }


            else
            {
                GoSingle();

            }








            using (SchedulerLog _AL = new SchedulerLog())
            {
                _AL.ConnectionString = dbTempString;
                _AL.UpdateLogEntry(_AppName, "Exited Normaly", JobID);
            }


            if (c.WaitForEnterToExit == 1)
            {

                System.Console.WriteLine("\n\rPress Any Key to Exit");
                System.Console.ReadKey();
                System.Console.WriteLine("\n\rShutting down threads and Exiting");
            }


            ss = null;

            // realease the mutex and dsipose of it only thing left is to exit
            if (mutex != null)
            {
                if (hasHandle)
                    mutex.ReleaseMutex();
                mutex.Dispose();
            }

            Environment.Exit(0);
        }



        static void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (c.verbose == 1)
            {
                System.Console.WriteLine("\n\rTimer Fired current pool count  " + Convert.ToString(MyPool.PoolCount));
            }

            JobTimeCount++;
            if (JobTimeCount > 2)
            {
                if (MyPool.PoolCount == 0)
                {
                    _Recycle = true;
                }
            }

            if (_Recycle)
            {


                using (SchedulerLog _AL = new SchedulerLog())
                {
                    _AL.ConnectionString = dbTempString;

                    if (MyPool.PoolCount == 0)
                    {
                        _AL.UpdateLogEntry(_AppName, "Exited normally ", JobID);
                    }
                    else
                    {
                        _AL.UpdateLogEntry(_AppName, "Exited due to recyle timer with " + Convert.ToString(MyPool.PoolCount) + " in que", JobID);
                    }

                }
                Environment.Exit(0);
      
            }
 
        }



        static void _RecycleTimerElaspesed(object sender, ElapsedEventArgs e)
        {
            if (c.verbose == 1)
            {
                System.Console.WriteLine("\n\rTimer Fired  to shut down there are still pending jobs " + Convert.ToString(JobCount));
            }


            _Recycle = true;
            //   GC.Collect();
        }



        private static void GoSingle()
        {



            int r = 0; // retrun from the UCLA class to see if its zerop
            int i = 0; // sanity count
            int _id = 0; // sanity count
            int x = 0; // sanity count         
            string html = string.Empty;
            string OutFile = string.Empty;
            string RowData = string.Empty;
            double BatchId = 10;


            string tRowData = string.Empty;

            //try
            //{
            using (SqlConnection con = new SqlConnection(dbTempString))
            {
                con.Open();
                //
                // Create new SqlCommand object.
                //
                using (SqlCommand cmd = new SqlCommand(c.getAllDataSp, con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader idr = cmd.ExecuteReader())
                    {

                        if (idr.HasRows)
                        {



                            // Call Read before accessing data. 
                            while (idr.Read())
                            {


                                string EDI = "";
                                int EDIKey = 0;
                                Guid t = Guid.NewGuid();
                                bool pass = true;

                                iEDIKey++;




                                int id = 0;


                                EDIKey = iEDIKey;

                                EDI = Convert.ToString((int)idr["id"]);
                                id = Convert.ToInt32((int)idr["id"]);



                                if (idr["vendor_input_type"] != System.DBNull.Value)
                                {
                                    EDI = EDI + "|" + Convert.ToString((string)idr["vendor_input_type"]);
                                }
                                else
                                {
                                    log.ExceptionDetails(_AppName, Convert.ToString((int)idr["id"]) + " vendor_input_type  = null");
                                    pass = false;
                                }

                                if (idr["pat_hosp_code"] != System.DBNull.Value)
                                {
                                    EDI = EDI + "|" + Convert.ToString((string)idr["pat_hosp_code"]);
                                }
                                else
                                {
                                    log.ExceptionDetails(_AppName, Convert.ToString((int)idr["id"]) + " pat_hosp_code  = null");
                                    pass = false;
                                }

                                if (idr["hosp_code"] != System.DBNull.Value)
                                {
                                    EDI = EDI + "|" + Convert.ToString((string)idr["hosp_code"]);
                                }
                                else
                                {
                                    log.ExceptionDetails(_AppName, Convert.ToString((int)idr["id"]) + " hosp_code  = null");
                                    pass = false;
                                }


                                if (idr["ins_type"] != System.DBNull.Value)
                                {
                                    EDI = EDI + "|" + Convert.ToString((string)idr["ins_type"]);
                                }
                                else
                                {
                                    log.ExceptionDetails(_AppName, Convert.ToString((int)idr["id"]) + " ins_type  = null");
                                    pass = false;
                                }


                                if (idr["user_id"] != System.DBNull.Value)
                                {
                                    EDI = EDI + "|" + Convert.ToString((string)idr["user_id"]);
                                }
                                else
                                {
                                    log.ExceptionDetails(_AppName, Convert.ToString((int)idr["id"]) + " user_id  = null");
                                    pass = false;
                                }


                                if (idr["account_number"] != System.DBNull.Value)
                                {
                                    EDI = EDI + "|" + Convert.ToString((string)idr["account_number"]);
                                }
                                else
                                {
                                    log.ExceptionDetails(_AppName, Convert.ToString((int)idr["id"]) + " account_number  = null");
                                    pass = false;
                                }

                                if (idr["vendor_name"] != System.DBNull.Value)
                                {
                                    EDI = EDI + "|" + Convert.ToString((string)idr["vendor_name"]);
                                }
                                else
                                {
                                    log.ExceptionDetails(_AppName, Convert.ToString((int)idr["id"]) + " vendor_name  = null");
                                    pass = false;
                                }


                                if (idr["payor_code"] != System.DBNull.Value)
                                {
                                    EDI = EDI + "|" + Convert.ToString((string)idr["payor_code"]);
                                }
                                else
                                {
                                    log.ExceptionDetails(_AppName, Convert.ToString((int)idr["id"]) + " payor_code  = null");
                                    pass = false;
                                }

                                if (idr["RequestString"] != System.DBNull.Value)
                                {
                                    EDI = EDI + "|" + ss.StraemEncode(Convert.ToString((string)idr["RequestString"]));
                                }
                                else
                                {
                                    log.ExceptionDetails(_AppName, Convert.ToString((int)idr["id"]) + " request_string  = null");
                                    pass = false;
                                }


                                if (idr["subscriberfirstname"] != System.DBNull.Value)
                                {


                                    EDI = EDI + "|" + ss.StraemEncode(Convert.ToString((string)idr["subscriberfirstname"]));
                                }
                                else
                                {
                                    log.ExceptionDetails(_AppName, Convert.ToString((int)idr["id"]) + " subscriberfirstname  = null");
                                    pass = false;
                                }

                                if (idr["subscriberlastname"] != System.DBNull.Value)
                                {


                                    EDI = EDI + "|" + ss.StraemEncode(Convert.ToString((string)idr["subscriberlastname"]));
                                }
                                else
                                {
                                    log.ExceptionDetails(_AppName, Convert.ToString((int)idr["id"]) + " subscriberlastname  = null");
                                    pass = false;
                                }

                                if (idr["subscriberdob"] != System.DBNull.Value)
                                {


                                    EDI = EDI + "|" + ss.StraemEncode(Convert.ToString((string)idr["subscriberdob"]));
                                }
                                else
                                {
                                    log.ExceptionDetails(_AppName, Convert.ToString((int)idr["id"]) + " subscriberdob  = null");
                                    pass = false;
                                }

                                if (idr["dateofservice"] != System.DBNull.Value)
                                {


                                    EDI = EDI + "|" + ss.StraemEncode(Convert.ToString((string)idr["dateofservice"]));
                                }
                                else
                                {
                                    log.ExceptionDetails(_AppName, Convert.ToString((int)idr["id"]) + " dateofservice  = null");
                                    pass = false;
                                }

                                //try
                                //{

                                //    // Create new SqlConnection object.
                                //    //
                                //    using (SqlConnection conUP = new SqlConnection(dbTempString))
                                //    {
                                //        con.Open();
                                //        //
                                //        // Create new SqlCommand object.
                                //        //
                                //        using (SqlCommand cmdUP = new SqlCommand("usp_eligibility_update_ebr_processed_flag", conUP))
                                //        {
                                //            cmdUP.CommandType = CommandType.StoredProcedure;

                                //            cmdUP.Parameters.Add("@ebr_id", SqlDbType.BigInt).Value = id; /// StringEXT.Truncate(ReferenceID, _MAX_LENGHT);


                                //            cmdUP.ExecuteNonQuery();//
                                //            // Invoke ExecuteReader method.
                                //            // 

                                //        }
                                //        con.Close();
                                //    }

                                //}
                                //catch (Exception ex)
                                //{
                                //    log.ExceptionDetails(_AppName + " " + id, ex);

                                //}




                                // if pass = ture add them to the diconary id not fail it and log it.

                                if (pass)
                                {
                                    _dID.Add(x, EDI);
                                    x++;
                                }
                                else
                                {
                                    log.ExceptionDetails(_AppName, Convert.ToString((int)idr["id"]) + " Request is null, either Screen scrape or Request string is not correct.");
                                }


                            }





                        }  // end wilewhile




                        con.Close();
                    }




                    foreach (KeyValuePair<int, string> pair in _dID)
                    {
                        /*************************************************************************************************************************
                         * do what ever start here
                         * ***********************************************************************************************************************/
                        try
                        {

                            ProcessEligilbiltyMainNPLazyUpdatev8Single pt = new ProcessEligilbiltyMainNPLazyUpdatev8Single();
                            pt.ConsoleName = c.ConsoleName;

                            pt.WorkUnit = pair.Value;
                            //     pt.TaskID = ii;
                            pt.ConnectionString = dbTempString;
                            //     pt.SYNC_TIMEOUT = c.SYNC_TIMEOUT;
                            //     pt.SUBMISSION_TIMEOUT = c.SUBMISSION_TIMEOUT;
                            //     pt.CommandTimeOut = c.CommandTimeOut;
                            //     pt.getAllDataSp = c.getAllDataSp;
                            //     pt.isParseVBorDB = c.isParseVBorDB;
                            //     pt.isEmdeonLookUp = c.isEmdeonLookUp;
                            //     pt.reRunEligAttempts = c.reRunEligAttempts;
                            //     pt.uspEdiDbImport = c.uspEdiDbImport;
                            pt.DeadlockRetrys = c.DeadLockRetrys;
                            pt.isXMLREQ = c.isXMLREQ;
                            pt.VendorRetrys = c.VendorRetrys;
                            //     pt.LogPath = c.LogPath;
                            pt.Verbose = c.verbose;
                            pt.XMLParameters = _XMLParameters;
                            pt.Main();
                        }

                        catch (Exception ex)
                        {
                            log.ExceptionDetails(_AppName, ex);
                        }
                        /*************************************************************************************************************************
                        * do what ever end here
                        * ***********************************************************************************************************************/

                    }
                    _dID.Clear();

                };
            }
        }










        static void GetAppConfig()
        {
            try
            {
               
                c.ConnectionString = ConfigurationManager.AppSettings["ConnStr"];
                c.ConsoleName = ConfigurationManager.AppSettings["ConsoleName"];
                c.DeadLockRetrys = Convert.ToInt32(ConfigurationManager.AppSettings["DeadLockRetrys"]);
                c.dbCommandTimeOut = Convert.ToInt32(ConfigurationManager.AppSettings["dbCommandTimeOut"]);

                c.getAllDataSp = ConfigurationManager.AppSettings["getAllDataSp"];
                c.cfgUspUpdateEligAdt = ConfigurationManager.AppSettings["cfgUspUpdateEligAdt"];

                c.MinThreads = ConfigurationManager.AppSettings["MinThreads"];
                c.MaxThreads = ConfigurationManager.AppSettings["MaxThreads"];
                c.MinWait = ConfigurationManager.AppSettings["MinWait"];
                c.MaxWait = ConfigurationManager.AppSettings["MaxWait"];
                c.SchedulingInterval = ConfigurationManager.AppSettings["SchedulingInterval"];
                c.MaxCount = ConfigurationManager.AppSettings["MaxCount"];
                c.MaxThreads = ConfigurationManager.AppSettings["MaxThreads"];
                c.ThreadsON = ConfigurationManager.AppSettings["ThreadsON"];
                c.dbPollTimeMilliSeconeds = Convert.ToInt32(ConfigurationManager.AppSettings["dbPollTimeMilliSeconeds"]);

                c.ForceClearTextPasswd = Convert.ToInt32(ConfigurationManager.AppSettings["ForceClearTextPasswd"]);

                c.WaitForEnterToExit = Convert.ToInt32(ConfigurationManager.AppSettings["WaitForEnterToExit"]);

                c.verbose = Convert.ToInt32(ConfigurationManager.AppSettings["verbose"]);

                c.ConnectionString = ConfigurationManager.AppSettings["ConnStr"];
                c.dbCommandTimeOut = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeOut"]);
                c.SYNC_TIMEOUT = ConfigurationManager.AppSettings["SYNC_TIMEOUT"];
                c.SUBMISSION_TIMEOUT = ConfigurationManager.AppSettings["SUBMISSION_TIMEOUT"];
                c.getAllDataSp = ConfigurationManager.AppSettings["getAllDataSp"];
                c.ErrorLog = ConfigurationManager.AppSettings["ErrorLog"];
                c.isParseVBorDB = ConfigurationManager.AppSettings["isParseVBorDB"];
                c.isEmdeonLookUp = ConfigurationManager.AppSettings["isEmdeonLookUp"];
                c.reRunEligAttempts = ConfigurationManager.AppSettings["reRunEligAttempts"];
                c.uspEdiRequest = ConfigurationManager.AppSettings["uspEdiRequest"];
                c.uspEdiDbImport = ConfigurationManager.AppSettings["uspEdiDbImport"];
                c.LogPath = ConfigurationManager.AppSettings["LogPath"];
                c.ConsoleName = ConfigurationManager.AppSettings["ConsoleName"];
                c.MaxCount = ConfigurationManager.AppSettings["MaxCount"];
                c.MaxThreads = ConfigurationManager.AppSettings["MaxThreads"];
                c.ThreadsON = ConfigurationManager.AppSettings["ThreadsON"];

                c.NoDataTimeOut = ConfigurationManager.AppSettings["CommandTimeOut"];
                c.verbose = Convert.ToInt32(ConfigurationManager.AppSettings["verbose"]);
                c.DeadLockRetrys = Convert.ToInt32(ConfigurationManager.AppSettings["DeadLockRetrys"]);
                c.ParameterFilePath = ConfigurationManager.AppSettings["ParameterFilePath"];
                c.isXMLREQ = Convert.ToInt32(ConfigurationManager.AppSettings["isXMLREQ"]);
                c.ForceClearTextPasswd = Convert.ToInt32(ConfigurationManager.AppSettings["ForceClearTextPasswd"]);
                c.VendorRetrys = Convert.ToInt32(ConfigurationManager.AppSettings["VendorRetrys"]);


                c.PoolPollTime = Convert.ToInt32(ConfigurationManager.AppSettings["PoolPollTime"]);
                c.RecycleWaitTime = Convert.ToInt32(ConfigurationManager.AppSettings["RecycleWaitTime"]);




            }
            catch (Exception ex)
            {
                el.WriteEventError(_AppName + "  Unable to laod Configuration Data app exited", 1);
                if (c.verbose == 1)
                {
                    System.Console.WriteLine("Unable to laod Configuration Data app exited");
                }
                Environment.Exit(0);
            }

            if (c.verbose == 1)
            {
                System.Console.WriteLine("Get Settings Complete" + "\n\r");
            }


        }



        static int GetXMLParameters()
        {
            int r = -1;
            try
            {

                using (SqlConnection con = new SqlConnection(dbTempString))
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
                                    r = 0;
                                }
                            }
                        }
                    }
                }

            }
            catch (SqlException sx)
            {

                log.ExceptionDetails(_AppName, sx);
            }

            catch (Exception ex)
            {

                log.ExceptionDetails(_AppName, ex);
            }


            return r;

        }




        static void InitMutex()
        {
            string appGuid = ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false).GetValue(0)).Value.ToString();
            string mutexId = string.Format("Global\\{{{0}}}", c.ConsoleName + appGuid);

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

        private static void GO()
        {

            int r = 0; // retrun from the UCLA class to see if its zerop
            int i = 0; // sanity count
            int _id = 0; // sanity count
            int x = 0; // sanity count         
            string html = string.Empty;
            string OutFile = string.Empty;
            string RowData = string.Empty;
            double BatchId = 10;


            string tRowData = string.Empty;

            //try
            //{
            using (SqlConnection con = new SqlConnection(dbTempString))
            {
                con.Open();
                //
                // Create new SqlCommand object.
                //
                using (SqlCommand cmd = new SqlCommand(c.getAllDataSp, con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader idr = cmd.ExecuteReader())
                    {

                        if (idr.HasRows)
                        {



                            // Call Read before accessing data. 
                            while (idr.Read())
                            {


                                string EDI = "";
                                int EDIKey = 0;
                                Guid t = Guid.NewGuid();
                                bool pass = true;

                                iEDIKey++;




                                int id = 0;


                                EDIKey = iEDIKey;

                                EDI = Convert.ToString((int)idr["id"]);
                                id = Convert.ToInt32((int)idr["id"]);



                                if (idr["vendor_input_type"] != System.DBNull.Value)
                                {
                                    EDI = EDI + "|" + Convert.ToString((string)idr["vendor_input_type"]);
                                }
                                else
                                {
                                    log.ExceptionDetails(_AppName, Convert.ToString((int)idr["id"]) + " vendor_input_type  = null");
                                    pass = false;
                                }

                                if (idr["pat_hosp_code"] != System.DBNull.Value)
                                {
                                    EDI = EDI + "|" + Convert.ToString((string)idr["pat_hosp_code"]);
                                }
                                else
                                {
                                    log.ExceptionDetails(_AppName, Convert.ToString((int)idr["id"]) + " pat_hosp_code  = null");
                                    pass = false;
                                }

                                if (idr["hosp_code"] != System.DBNull.Value)
                                {
                                    EDI = EDI + "|" + Convert.ToString((string)idr["hosp_code"]);
                                }
                                else
                                {
                                    log.ExceptionDetails(_AppName, Convert.ToString((int)idr["id"]) + " hosp_code  = null");
                                    pass = false;
                                }


                                if (idr["ins_type"] != System.DBNull.Value)
                                {
                                    EDI = EDI + "|" + Convert.ToString((string)idr["ins_type"]);
                                }
                                else
                                {
                                    log.ExceptionDetails(_AppName, Convert.ToString((int)idr["id"]) + " ins_type  = null");
                                    pass = false;
                                }


                                if (idr["user_id"] != System.DBNull.Value)
                                {
                                    EDI = EDI + "|" + Convert.ToString((string)idr["user_id"]);
                                }
                                else
                                {
                                    log.ExceptionDetails(_AppName, Convert.ToString((int)idr["id"]) + " user_id  = null");
                                    pass = false;
                                }


                                if (idr["account_number"] != System.DBNull.Value)
                                {
                                    EDI = EDI + "|" + Convert.ToString((string)idr["account_number"]);
                                }
                                else
                                {
                                    log.ExceptionDetails(_AppName, Convert.ToString((int)idr["id"]) + " account_number  = null");
                                    pass = false;
                                }

                                if (idr["vendor_name"] != System.DBNull.Value)
                                {
                                    EDI = EDI + "|" + Convert.ToString((string)idr["vendor_name"]);
                                }
                                else
                                {
                                    log.ExceptionDetails(_AppName, Convert.ToString((int)idr["id"]) + " vendor_name  = null");
                                    pass = false;
                                }


                                if (idr["payor_code"] != System.DBNull.Value)
                                {
                                    EDI = EDI + "|" + Convert.ToString((string)idr["payor_code"]);
                                }
                                else
                                {
                                    log.ExceptionDetails(_AppName, Convert.ToString((int)idr["id"]) + " payor_code  = null");
                                    pass = false;
                                }

                                if (idr["RequestString"] != System.DBNull.Value)
                                {
                                    EDI = EDI + "|" + ss.StraemEncode(Convert.ToString((string)idr["RequestString"]));
                                }
                                else
                                {
                                    log.ExceptionDetails(_AppName, Convert.ToString((int)idr["id"]) + " request_string  = null");
                                    pass = false;
                                }


                                if (idr["subscriberfirstname"] != System.DBNull.Value)
                                {


                                    EDI = EDI + "|" + ss.StraemEncode(Convert.ToString((string)idr["subscriberfirstname"]));
                                }
                                else
                                {
                                    log.ExceptionDetails(_AppName, Convert.ToString((int)idr["id"]) + " subscriberfirstname  = null");
                                    pass = false;
                                }

                                if (idr["subscriberlastname"] != System.DBNull.Value)
                                {


                                    EDI = EDI + "|" + ss.StraemEncode(Convert.ToString((string)idr["subscriberlastname"]));
                                }
                                else
                                {
                                    log.ExceptionDetails(_AppName, Convert.ToString((int)idr["id"]) + " subscriberlastname  = null");
                                    pass = false;
                                }

                                if (idr["subscriberdob"] != System.DBNull.Value)
                                {


                                    EDI = EDI + "|" + ss.StraemEncode(Convert.ToString((string)idr["subscriberdob"]));
                                }
                                else
                                {
                                    log.ExceptionDetails(_AppName, Convert.ToString((int)idr["id"]) + " subscriberdob  = null");
                                    pass = false;
                                }

                                if (idr["dateofservice"] != System.DBNull.Value)
                                {


                                    EDI = EDI + "|" + ss.StraemEncode(Convert.ToString((string)idr["dateofservice"]));
                                }
                                else
                                {
                                    log.ExceptionDetails(_AppName, Convert.ToString((int)idr["id"]) + " dateofservice  = null");
                                    pass = false;
                                }

                                if (pass)
                                {
                                    _dID.Add(x, EDI);
                                    x++;
                                }
                                else
                                {
                                    log.ExceptionDetails(_AppName, Convert.ToString((int)idr["id"]) + " Request is null, either Screen scrape or Request string is not correct.");
                                }
                            }
                        }  // end wilewhile
                        con.Close();
                    }
                }
            }

            //sql using





            foreach (KeyValuePair<int, string> pair in _dID)
            {
                MyPool.QueueUserTask(() =>
                      {
                          /*************************************************************************************************************************
                           * do what ever start here
                           * ***********************************************************************************************************************/
                          try
                          {
                              JobCount++;
                              ProcessEligilbiltyMainNPLazyUpdatev8Single pt = new ProcessEligilbiltyMainNPLazyUpdatev8Single();
                              pt.ConsoleName = c.ConsoleName;

                              pt.WorkUnit = pair.Value;
                              //     pt.TaskID = ii;
                              pt.ConnectionString = dbTempString;
                              //     pt.SYNC_TIMEOUT = c.SYNC_TIMEOUT;
                              //     pt.SUBMISSION_TIMEOUT = c.SUBMISSION_TIMEOUT;
                              //     pt.CommandTimeOut = c.CommandTimeOut;
                              //     pt.getAllDataSp = c.getAllDataSp;
                              //     pt.isParseVBorDB = c.isParseVBorDB;
                              //     pt.isEmdeonLookUp = c.isEmdeonLookUp;
                              //     pt.reRunEligAttempts = c.reRunEligAttempts;
                              //     pt.uspEdiDbImport = c.uspEdiDbImport;
                              pt.DeadlockRetrys = c.DeadLockRetrys;
                              pt.isXMLREQ = c.isXMLREQ;
                              pt.VendorRetrys = c.VendorRetrys;
                              //     pt.LogPath = c.LogPath;
                              pt.Verbose = c.verbose;
                              pt.XMLParameters = _XMLParameters;
                              pt.Main();
                          }

                          catch (Exception ex)
                          {
                              log.ExceptionDetails(_AppName, ex);
                          }
                          /*************************************************************************************************************************
                          * do what ever end here
                          * ***********************************************************************************************************************/
                      },
                 (ts) =>
                 {
                     if (c.verbose == 1)
                     {

                         log.ExceptionDetails(_AppName, "Thread for + " + Convert.ToString(pair.Value) + " completed");
                     }
                     JobCount--;
                     //showMessage(ts.Success.ToString());
                 });

            
            }


            _dID.Clear();

          
         
        }

    }


}

