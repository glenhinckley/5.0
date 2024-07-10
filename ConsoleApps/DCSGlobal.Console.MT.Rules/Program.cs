using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;
using System;
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
using DCSGlobal.Rules.FireRules;








using System.Reflection;

using System.Reflection.Emit;



using System.Runtime.InteropServices;   //GuidAttribute
using System.Reflection;                //Assembly
using System.Security.AccessControl;    //MutexAccessRule
using System.Security.Principal;        //SecurityIdentifier


namespace DCSGlobal.Console.MT.Rules
{
    class Program
    {

        private static int RunningTasks = 0;
        private static int numThreads = 4;
        private static int iEDIKey = 0;
        private static int iEDICKey = 0;
        private static configEX c = new configEX();
        private static string _AppName = "DCSGlobal.Console.MT.Rules";
        private static Int32 MaxCount;
        private static Int32 TotalThreadCount = 0;
        private static logExecption log = new logExecption();
        private static ApplicationLog al = new ApplicationLog();
        private static LogToEventLog el = new LogToEventLog();
        private static StringStuff ss = new StringStuff();

        private static System.Timers.Timer _timer; // From System.Timers
        private static System.Timers.Timer _MinRulesTimer; // From System.Timers
        private static System.Timers.Timer _DropDeadTimer; // From System.Timers

        private static System.Timers.Timer _MKC; // From System.Timers


        private static string dbTempString = string.Empty;
        private static ManualResetEvent _quitEvent = new ManualResetEvent(false);



        // Here we create a DataTable with four columns.
        private static DataTable table = new DataTable();



        private static int _TotalNumberOfRulesRan = 0;
        private static int _TotalNumberOfRulesReportedComplete = 0;
        private static int _TotalRulesToFire = 0;
        private static int _NumberOfRows = 0;

        private static int _BatchCount = 0;
        private static int _BatchID = 0;


        private static int _TimeToWiatForThreadsToCompleteOnRecycle = 0;

        private static int _Monitoring = 0;
        private static int _Metrics = 0;
        private static string _MetricsString = string.Empty;
        private static int MaxQueDepth = 500;

        private static int _MinRulesTime = 2000;
        private static int _MinRules = 0;
        // chached datasets
        private static DataSet _dsRulesToFire = new DataSet();

        private static CustomThreadPool MyPool;

        private static int JobCount = 0;
        private static int JobID = 0;

        private static Mutex _m;
        private static MutexAccessRule allowEveryoneRule = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow);
        private static MutexSecurity securitySettings = new MutexSecurity();
        private static bool createdNew;
        public static bool hasHandle = false;
        private static Mutex mutex;

        private static int TaskCount = 0;
        private static int TaskCountIntervalTimeout = 0;
        private static int MAXTaskCountIntervalTimeout = 0;




        private static int ThreadIntervalTimeout = 0;

        private static bool _DropDead = false;
        private static int _RecycleWaitTime = 600000;
        private static int _RecycleWaitTimeReset = 0;
        private static int _RecycleWaitTimeResetMAXCount = 0;
        private static int _RecycleWaitTimeResetCount = 0;



        ~Program()
        {


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

                log.Dispose();
                el.Dispose();
                al.Dispose();

                // free other managed objects that implement
                // IDisposable only
            }

            c = null;
            // release any unmanaged objects
            // set the object references to null

            _disposed = true;
        }



        //  private static 

        static void MainEX(string[] args)
        {

            securitySettings.AddAccessRule(allowEveryoneRule);

            GetAppConfig();
            //IsSingleInstance();

            _AppName = c.ConsoleName;

            SingleGlobalInstance(3000);

            if (!hasHandle)
            {
                Environment.Exit(0);

            }




            DBUtility dbu = new DBUtility();

            dbTempString = dbu.getConnectionString(c.ConnectionString);






            if (dbTempString == "err" || dbTempString.Contains("Base-64"))
            {



                if (dbu.ValidateConString(c.ConnectionString))
                {

                    dbTempString = c.ConnectionString;
                    el.WriteEventWarning(_AppName + "  running with clear text constring", 1);

                    if (c.ForceClearTextPasswd == 0)
                    {
                        if (c.WaitForEnterToExit == "1")
                        {

                            System.Console.WriteLine("\n\rRuning with clear text passwd and ForceClearTextPasswd set to 0");
                            System.Console.WriteLine("\n\rPress Any Key to Exit");
                            System.Console.ReadKey();
                            System.Console.WriteLine("\n\rShutting down threads and Exiting");

                            Environment.Exit(0);

                        }
                        else
                        {
                            Environment.Exit(0);

                        }

                    }





                }
                else
                {

                    el.WriteEventError(_AppName + "  decode constring failed", 1);
                    // realease the mutex and dsipose of it only thing left is to exit
                    if (mutex != null)
                    {
                        if (hasHandle)
                            mutex.ReleaseMutex();
                        mutex.Dispose();
                    }
                    Environment.Exit(0);
                }
            }

            // dbTempString = c.ConnectionString;

            log.ConnectionString = dbTempString;
            al.ConnectionString = dbTempString;

            using (SchedulerLog _AL = new SchedulerLog())
            {
                _AL.ConnectionString = dbTempString;
                JobID = _AL.AddLogEntry(_AppName, "MT Rules Console");
                _AL.UpdateLogEntry(_AppName, "Start up for JobID: " + Convert.ToString(JobID), JobID);
            }


            Thread thread = new Thread(CreateNamedPipeServerToListen);
            thread.Start();


            BuildDataTable();





            _MKC = new System.Timers.Timer(240000);
            _MKC.Elapsed += new ElapsedEventHandler(_MKC_timer_Elapsed);
            _MKC.Enabled = true; // Enable it

            // dosomehting between here


            c.htRuleMsgs = new System.Collections.Hashtable();

            if (GetDataSets() != 0)
            {

                using (SchedulerLog _AL = new SchedulerLog())
                {
                    _AL.ConnectionString = dbTempString;
                    _AL.UpdateLogEntry(_AppName, "Get Datasets failed", JobID);
                }

                // realease the mutex and dsipose of it only thing left is to exit
                if (mutex != null)
                {
                    if (hasHandle)
                        mutex.ReleaseMutex();
                    mutex.Dispose();
                }
                Environment.Exit(0);

            }




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




                _timer = new System.Timers.Timer(c.dbPollTimeMilliSeconeds);
                _timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);





                if (MAXTaskCountIntervalTimeout != 0)
                {
                    _timer.Enabled = true; // Enable it
                }










                _MinRulesTimer = new System.Timers.Timer(_MinRulesTime);
                _MinRulesTimer.Elapsed += new ElapsedEventHandler(_MinRulesTimer_Elapsed);
                _MinRulesTimer.Enabled = true; // Enable it

                _DropDeadTimer = new System.Timers.Timer(_RecycleWaitTime);
                _DropDeadTimer.Elapsed += new ElapsedEventHandler(_DropDead_timer_Elapsed);
                _DropDeadTimer.Enabled = true; // Enable it





                if (c.Verbose == 1)
                {
                    System.Console.WriteLine("\n\rThreads ON");
                }
                GO_MT();
                _quitEvent.WaitOne();
            }
            else
            {
                if (c.Verbose == 1)
                {
                    System.Console.WriteLine("\n\rThreads OFF");
                }



                _MinRulesTimer = new System.Timers.Timer(_MinRulesTime);
                _MinRulesTimer.Elapsed += new ElapsedEventHandler(_MinRulesTimer_Elapsed);
                _MinRulesTimer.Enabled = true; // Enable it
                Go_Single();
            }



            //if (c.WaitForEnterToExit == "1")
            //{

            //    System.Console.WriteLine("\n\rPress Any Key to Exit");
            //    System.Console.ReadKey();
            //    System.Console.WriteLine("\n\rShutting down threads and Exiting");
            //}





            //// realease the mutex and dsipose of it only thing left is to exit
            //if (mutex != null)
            //{
            //    if (hasHandle)
            //        mutex.ReleaseMutex();
            //    mutex.Dispose();
            //}
            //Environment.Exit(0);


        }



        static void _timer_Elapsedex(object sender, ElapsedEventArgs e)
        {



            if (!_DropDead)
            {
                // Set Core 1
                //  process.ProcessorAffinity = new IntPtr(0x0002);

                int numThreads = Environment.ProcessorCount;


                if (MyPool.PoolCount != Convert.ToInt32(c.MinThreads))
                {

                    if (c.Verbose == 1)
                    {
                        System.Console.WriteLine("\n\rTaskCount was reset by watchdog timer due to hung threads ");
                        log.ExceptionDetails(_AppName, "TaskCount was reset by watchdog timer due to one or more threads not reporting END");

                    }
                    ThreadIntervalTimeout = 0;



                    using (SchedulerLog _AL = new SchedulerLog())
                    {
                        _AL.ConnectionString = dbTempString;
                        _AL.UpdateLogEntry(_AppName, "TaskCount was reset by watchdog timer due to one or more threads not reporting END in BatchID  " + Convert.ToString(_BatchID) + "  for JobID " + Convert.ToString(JobID), _BatchID, _NumberOfRows, _TotalRulesToFire);
                    }



                    TaskCount = 0;
                    //  GO_MT();

                }
                else
                {
                    // ' ThreadIntervalTimeout++;
                    if (c.Verbose == 1)
                    {
                        System.Console.WriteLine("\n\rTimer Fired JOBS NOT DONE ");
                    }


                }

                if (ThreadIntervalTimeout == c.ThreadIntervalTimeout)
                {
                    log.ExceptionDetails(_AppName, "ThreadIntervalTimeout interval exceeded ");
                    Environment.Exit(0);
                    // _quitEvent.Set();
                }

            }
            //   GC.Collect();
        }








        static void _MinRulesTimer_Elapsed(object sender, ElapsedEventArgs e)
        {

            if (table.Rows.Count == 0)
            {

                if (!_DropDead)
                {

                    TaskCountIntervalTimeout = 0;
                    GO_MT();
                }


                //DataTable does not contain records
            }


            //if (TaskCount == _MinRules)
            //{
            //    //using (SchedulerLog _AL = new SchedulerLog())
            //    //{

            //    //    //_AL.ConnectionString = dbTempString;
            //    //    //_AL.UpdateLogEntry(_AppName, "Not enough exceded kickstart", JobID);
            //    //    //  Environment.Exit(0);
            //    //    //        ExitFlag = true;
            //    //}

            //    //  TaskCount = 0;
            //    System.Console.WriteLine("Job Count Zero going to MT");
            // //   Go_Single();
            //    //  GO_MT();

            //}
            //else
            //{
            //    System.Console.WriteLine("_MinRulesTimer_Elapsed Wating for all threads to complete threads left: " + Convert.ToString(TaskCount));

            //}





        }




        static void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {





            TaskCountIntervalTimeout++;


            if (MAXTaskCountIntervalTimeout == TaskCountIntervalTimeout)
            {
                using (SchedulerLog _AL = new SchedulerLog())
                {

                    _AL.ConnectionString = dbTempString;
                    _AL.UpdateLogEntry(_AppName, "MAXTaskCountIntervalTimeout exceded shutting down to wait for recycle", JobID);
                    Environment.Exit(0);
                    //        ExitFlag = true;
                }
                TaskCount = 0;
                //  GO_MT();

            }





        }




        static void _MKC_timer_Elapsed(object sender, ElapsedEventArgs e)
        {


            using (SchedulerLog _AL = new SchedulerLog())
            {

                _AL.ConnectionString = dbTempString;
                _AL.UpdateLogEntry(_AppName, "MKC time expried", JobID, _TotalNumberOfRulesReportedComplete, _TotalNumberOfRulesRan);

             
            }

            Environment.Exit(0);

        }

        static void _DropDead_timer_Elapsed(object sender, ElapsedEventArgs e)
        {


            _RecycleWaitTimeResetCount++;
            bool ExitFlag = false;


            if (_RecycleWaitTimeResetMAXCount == _RecycleWaitTimeResetCount)
            {
                MyPool.MinThreads = 0;
                _DropDead = true;
                _timer.Enabled = false;
                int i = 0;

                for (i = 0; i <= 10; i++)
                {
                    if (c.Verbose == 1)
                        System.Console.WriteLine("Wating for all threads to complete threads left: " + Convert.ToString(TaskCount));


                    Thread.Sleep(1000);
                    if (table.Rows.Count == 0)
                    {

                        continue;
                    }
                }


                if (table.Rows.Count == 0)
                {

                    using (SchedulerLog _AL = new SchedulerLog())
                    {

                        _AL.ConnectionString = dbTempString;
                        //  _AL.UpdateLogEntry(_AppName, "Shut down total runtime elapsed, all threads reported done", JobID);


                        _AL.UpdateLogEntry(_AppName, "Shut down total runtime elapsed, all threads reported done", JobID, _TotalNumberOfRulesReportedComplete, _TotalNumberOfRulesRan);

                        ExitFlag = true;
                    }
                }
                else
                {
                    using (SchedulerLog _AL = new SchedulerLog())
                    {

                        _AL.ConnectionString = dbTempString;
                        _AL.UpdateLogEntry(_AppName, "Shut down forced one or more threads not done", JobID, _TotalNumberOfRulesReportedComplete, _TotalNumberOfRulesRan);

                        ExitFlag = true;
                    }
                }

            }


            if (ExitFlag)
            {
                Environment.Exit(0);
                //   _quitEvent.Set();
            }
            else
            {
                _timer.Enabled = true;
                _DropDead = false;
            }



        }




        private static void Go_Single()
        {
            int r = 0; // retrun from the UCLA class to see if its zerop
            //    int i = 0; // sanity count
            int _id = 0; // sanity count
            //    int x = 0; // sanity count         

            _BatchID = 0;
            _NumberOfRows = 0;

            GetDataSets();
            using (SchedulerLog _SL = new SchedulerLog())
            {
                _SL.ConnectionString = dbTempString;
                _BatchID = _SL.AddLogEntry(_AppName, "Begin batch for JobID " + Convert.ToString(JobID));
                _SL.UpdateLogEntry(_AppName, "Batch ID:  " + Convert.ToString(_BatchID) + "  for JobID " + Convert.ToString(JobID), _BatchID, 0, _TotalRulesToFire);
            }

            try
            {
                using (SqlConnection con = new SqlConnection(dbTempString))
                {
                    con.Open();
                    //
                    // Create new SqlCommand object.
                    //
                    using (SqlCommand cmd = new SqlCommand(c.USP_GET_PAT_AUDIT_TRAIL_HL7_STARTER_IDS, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader idr = cmd.ExecuteReader())
                        {
                            //  count = idr.VisibleFieldCount;
                            if (idr.HasRows)
                            {
                                while (idr.Read())
                                {

                                    _NumberOfRows++;
                                    TaskCount++;
                                    _id = Convert.ToInt32(idr["PATIENT_AUDIT_TRAIL_ID"]);







                                    Guid tguid = Guid.NewGuid();
                                    string sguid = string.Empty;

                                    sguid = Convert.ToString(tguid);

                                    table.Rows.Add(JobID, _BatchID, tguid, DateTime.Now);



                                    using (Inline mfr = new Inline())
                                    {

                                        mfr.sguid = sguid;
                                        mfr.USP_GET_PAT_AUDIT_TRAIL_HL7_ROWS = c.USP_GET_PAT_AUDIT_TRAIL_HL7_ROWS;
                                        //  mfr.CfgUspGetAllDataHL7Rows = c.CfgUspGetAllDataHL7Rows; //   ' //SUBBA-20160713
                                        mfr.AppName = _AppName;
                                        mfr.ConnectionString = dbTempString;
                                        mfr.cfgContextID = c.cfgContextID;
                                        mfr.HospCode = c.HospCode; //'= ConfigurationManager.AppSettings["cfgHospCode"]
                                        mfr.errorLogPath = c.errorLogPath; //' = ConfigurationManager.AppSettings["errorLogPath"]
                                        mfr.debugMode = c.debugMode;  //'// = ConfigurationManager.AppSettings["debugMode"]
                                        mfr.ruleDebugMode = c.ruleDebugMode; //' = ConfigurationManager.AppSettings["ruleDebugMode"]
                                        mfr.runMode = c.runMode; //' = ConfigurationManager.AppSettings["runMode"]
                                        mfr.resultAll = c.resultAll; //'  **** = ConfigurationManager.AppSettings["resultAll"]
                                        mfr.raClientIP = c.raClientIP; //'= ConfigurationManager.AppSettings["raClientIP"]
                                        mfr.ruleSuccess = c.ruleSuccess; //' = ConfigurationManager.AppSettings["ruleSuccess"]
                                        mfr.AddressRunMode = c.AddressRunMode; //'= ConfigurationManager.AppSettings["addressRunMode"]
                                        mfr.addrSvcUrl1 = c.addrSvcUrl1; //' = ConfigurationManager.AppSettings["addrSvcUrl1"]
                                        mfr.addrSvcUrl2 = c.addrSvcUrl2; //' = ConfigurationManager.AppSettings["isParseVBorDB"]
                                        mfr.addrSvcPingLogMin = c.addrSvcPingLogMin; // ' = ConfigurationManager.AppSettings["addrSvcPingLogMin"]
                                        mfr.CfgUspGetAllData = c.CfgUspGetAllData; // ' = ConfigurationManager.AppSettings["CfgUspGetAllData"]
                                        //   mfr.CfgUspGetAllDataHL7Rows = c.CfgUspGetAllDataHL7Rows;
                                        mfr.CfgUspInsertSchedulerlog = c.CfgUspInsertSchedulerlog; // ' = ConfigurationManager.AppSettings["CfgUspInsertSchedulerlog"]
                                        mfr.CfgUspGetRuleMsg = c.CfgUspGetRuleMsg; //'= ConfigurationManager.AppSettings["CfgUspGetRuleMsg"]
                                        mfr.CfgUspRulesToFire = c.CfgUspRulesToFire;  // '// = ConfigurationManager.AppSettings["CfgUspRulesToFire"]
                                        mfr.CfgUspApplyPatAuditTrial = c.CfgUspApplyPatAuditTrial; // ' = ConfigurationManager.AppSettings["CfgUspApplyPatAuditTrial"]
                                        mfr.CfgUspTankAddress = c.CfgUspTankAddress;
                                        mfr.CfgUspAddCorrectAddress = c.CfgUspAddCorrectAddress; // ' = ConfigurationManager.AppSettings["CfgUspAddCorrectAddress"]
                                        mfr.CfgUspAddMultipleAddress = c.CfgUspAddMultipleAddress;
                                        mfr.CfgUspFormatAddress = c.CfgUspFormatAddress;
                                        mfr.CfgUspAddressValidationTrail = c.CfgUspAddressValidationTrail;
                                        mfr.CfgUspGetAllTankById = c.CfgUspGetAllTankById;
                                        mfr.CfgUspRuleInsert = c.CfgUspRuleInsert;
                                        mfr.CfgUspRuleResultDelete = c.CfgUspRuleResultDelete;
                                        mfr.CfgUspRuleInsertByXmlTank = c.CfgUspRuleInsertByXmlTank;  // '
                                        mfr.CfgUspRuleInsertDebug = c.CfgUspRuleInsertDebug;
                                        mfr.ruleDisplayLimit = c.ruleDisplayLimit; // ' = ConfigurationManager.AppSettings["ruleDisplayLimit"]
                                        mfr.customerID = c.customerID; // ' = ConfigurationManager.AppSettings["customerID"]
                                        mfr.licenseKey = c.licenseKey; // '= ConfigurationManager.AppSettings["licenseKey"]
                                        mfr.dsRulesToFire = _dsRulesToFire;
                                        mfr.TotalRulesToFire = _TotalRulesToFire;
                                        mfr.iMaxRowsToProcess = c.iMaxRowsToProcess;
                                        mfr.RULE_MESSAGE_DELIMETER = c.RULE_MESSAGE_DELIMETER;
                                        mfr.JobID = JobID;
                                        mfr.TotalRulesToFire = _TotalRulesToFire;





                                        mfr.htRuleMsgs = c.htRuleMsgs;

                                        mfr.AddressFilter = c.sAddrFilter;
                                        mfr.GetMultipleAddress = c.sAddrGetMultipleAddress;




                                        mfr.LogFilePath = c.LogFilePath;
                                        mfr.Verbose = c.Verbose;

                                        mfr.Metrics = _Metrics;

                                        mfr.Go(_id); //'SUBBA-20160713


                                        //  mfr.Go(pair.Value); //'SUBBA-20160713
                                        //  mfr.Go(_id); //'SUBBA-20160713

                                    }

                                }
                            }
                        }
                        // 
                        r = 0;
                    }
                    con.Close();
                }

            }
            catch (SqlException sx)
            {

                log.ExceptionDetails(_AppName + " Main" + " SQL Exception - Failed DCSGlobal.Console.mfr  ", sx);
                using (SchedulerLog _AL = new SchedulerLog())
                {

                    _AL.ConnectionString = dbTempString;
                    _AL.UpdateLogEntry(_AppName, "FATAL ERROR SEE EVENT LOG", JobID);
                    Environment.Exit(0);
                    //        ExitFlag = true;
                }
            }
            catch (Exception ex)
            {

                log.ExceptionDetails(_AppName + " Main" + " Exception - Failed DCSGlobal.Console.mfr  ", ex);
                Environment.Exit(0);

                // _quitEvent.Set();

            }
            // _timer.Enabled = true;

        }


        /// <summary>
        /// this runs inline in a MT role
        /// </summary>
        private static void GO_MT()
        {

            _timer.Enabled = false;
            // _MinRulesTimer.Enabled  = false; 

            int r = 0; // retrun from the UCLA class to see if its zerop
            int i = 0; // sanity count
            int _id = 0; // sanity count
            int x = 0; // sanity count         
            bool NoRows = true;


            Dictionary<int, int> _dID = new Dictionary<int, int>();


            _BatchID = 0;
            _NumberOfRows = 0;


            GetDataSets();
            using (SchedulerLog _SL = new SchedulerLog())
            {
                _SL.ConnectionString = dbTempString;
                _BatchID = _SL.AddLogEntry(_AppName, "Begin batch for JobID " + Convert.ToString(JobID));
                _SL.UpdateLogEntry(_AppName, "Batch ID:  " + Convert.ToString(_BatchID) + "  for JobID " + Convert.ToString(JobID), _BatchID, 0, _TotalRulesToFire);
            }

            try
            {

                using (SqlConnection con = new SqlConnection(dbTempString))
                {
                    con.Open();
                    //
                    // Create new SqlCommand object.
                    //
                    using (SqlCommand cmd = new SqlCommand(c.USP_GET_PAT_AUDIT_TRAIL_HL7_STARTER_IDS, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader idr = cmd.ExecuteReader())
                        {
                            //  count = idr.VisibleFieldCount;
                            if (idr.HasRows)
                            {
                                while (idr.Read())
                                {
                                    _id = Convert.ToInt32(idr["PATIENT_AUDIT_TRAIL_ID"]);

                                    NoRows = false;

                                    _dID.Add(x, _id);
                                    x++;

                                }
                            }
                        }
                        // 
                        r = 0;
                    }
                    con.Close();
                }


                if (c.Verbose == 1)
                {
                    System.Console.Write("pool count " + Convert.ToString(MyPool.PoolCount));
                }


                foreach (KeyValuePair<int, int> pair in _dID)
                {

                    _TotalNumberOfRulesRan++;
                    _NumberOfRows++;
                    TaskCount++;

                    Guid tguid = Guid.NewGuid();
                    string sguid = string.Empty;

                    sguid = Convert.ToString(tguid);

                    table.Rows.Add(JobID, _BatchID, tguid, DateTime.Now);
                    //   mfr.sguid = sguid;

                    MyPool.QueueUserTask(() =>
                    {
                        JobCount++;





                    },
                          (ts) =>
                          {
                              if (c.Verbose == 1)
                              {
                                  System.Console.Write("Thread for + " + Convert.ToString(pair.Value) + " started");
                              }

                              using (Inline mfr = new Inline())
                              {

                                  mfr.USP_GET_PAT_AUDIT_TRAIL_HL7_ROWS = c.USP_GET_PAT_AUDIT_TRAIL_HL7_ROWS;
                                  // mfr.CfgUspGetAllDataHL7Rows = c.CfgUspGetAllDataHL7Rows; //   ' //SUBBA-20160713
                                  mfr.AppName = _AppName;
                                  mfr.ConnectionString = dbTempString;
                                  mfr.cfgContextID = c.cfgContextID;
                                  mfr.HospCode = c.HospCode; //'= ConfigurationManager.AppSettings["cfgHospCode"]
                                  mfr.errorLogPath = c.errorLogPath; //' = ConfigurationManager.AppSettings["errorLogPath"]
                                  mfr.debugMode = c.debugMode;  //'// = ConfigurationManager.AppSettings["debugMode"]
                                  mfr.ruleDebugMode = c.ruleDebugMode; //' = ConfigurationManager.AppSettings["ruleDebugMode"]
                                  mfr.runMode = c.runMode; //' = ConfigurationManager.AppSettings["runMode"]
                                  mfr.resultAll = c.resultAll; //'  **** = ConfigurationManager.AppSettings["resultAll"]
                                  mfr.raClientIP = c.raClientIP; //'= ConfigurationManager.AppSettings["raClientIP"]
                                  mfr.ruleSuccess = c.ruleSuccess; //' = ConfigurationManager.AppSettings["ruleSuccess"]
                                  mfr.AddressRunMode = c.AddressRunMode; //'= ConfigurationManager.AppSettings["addressRunMode"]
                                  mfr.addrSvcUrl1 = c.addrSvcUrl1; //' = ConfigurationManager.AppSettings["addrSvcUrl1"]
                                  mfr.addrSvcUrl2 = c.addrSvcUrl2; //' = ConfigurationManager.AppSettings["isParseVBorDB"]
                                  mfr.addrSvcPingLogMin = c.addrSvcPingLogMin; // ' = ConfigurationManager.AppSettings["addrSvcPingLogMin"]
                                  mfr.CfgUspGetAllData = c.CfgUspGetAllData; // ' = ConfigurationManager.AppSettings["CfgUspGetAllData"]
                                  //   mfr.CfgUspGetAllDataHL7Rows = c.CfgUspGetAllDataHL7Rows;
                                  mfr.CfgUspInsertSchedulerlog = c.CfgUspInsertSchedulerlog; // ' = ConfigurationManager.AppSettings["CfgUspInsertSchedulerlog"]
                                  mfr.CfgUspGetRuleMsg = c.CfgUspGetRuleMsg; //'= ConfigurationManager.AppSettings["CfgUspGetRuleMsg"]
                                  mfr.CfgUspRulesToFire = c.CfgUspRulesToFire;  // '// = ConfigurationManager.AppSettings["CfgUspRulesToFire"]
                                  mfr.CfgUspApplyPatAuditTrial = c.CfgUspApplyPatAuditTrial; // ' = ConfigurationManager.AppSettings["CfgUspApplyPatAuditTrial"]
                                  mfr.CfgUspTankAddress = c.CfgUspTankAddress;
                                  mfr.CfgUspAddCorrectAddress = c.CfgUspAddCorrectAddress; // ' = ConfigurationManager.AppSettings["CfgUspAddCorrectAddress"]
                                  mfr.CfgUspAddMultipleAddress = c.CfgUspAddMultipleAddress;
                                  mfr.CfgUspFormatAddress = c.CfgUspFormatAddress;
                                  mfr.CfgUspAddressValidationTrail = c.CfgUspAddressValidationTrail;
                                  mfr.CfgUspGetAllTankById = c.CfgUspGetAllTankById;
                                  mfr.CfgUspRuleInsert = c.CfgUspRuleInsert;
                                  mfr.CfgUspRuleResultDelete = c.CfgUspRuleResultDelete;
                                  mfr.CfgUspRuleInsertByXmlTank = c.CfgUspRuleInsertByXmlTank;  // '
                                  mfr.CfgUspRuleInsertDebug = c.CfgUspRuleInsertDebug;
                                  mfr.ruleDisplayLimit = c.ruleDisplayLimit; // ' = ConfigurationManager.AppSettings["ruleDisplayLimit"]
                                  mfr.customerID = c.customerID; // ' = ConfigurationManager.AppSettings["customerID"]
                                  mfr.licenseKey = c.licenseKey; // '= ConfigurationManager.AppSettings["licenseKey"]
                                  mfr.dsRulesToFire = _dsRulesToFire;
                                  mfr.TotalRulesToFire = _TotalRulesToFire;
                                  mfr.iMaxRowsToProcess = c.iMaxRowsToProcess;
                                  mfr.RULE_MESSAGE_DELIMETER = c.RULE_MESSAGE_DELIMETER;
                                  mfr.JobID = JobID;
                                  mfr.TotalRulesToFire = _TotalRulesToFire;

                                  mfr.tguid = tguid;

                                  mfr.sguid = sguid;

                                  mfr.htRuleMsgs = c.htRuleMsgs;

                                  mfr.AddressFilter = c.sAddrFilter;
                                  mfr.GetMultipleAddress = c.sAddrGetMultipleAddress;




                                  mfr.LogFilePath = c.LogFilePath;
                                  mfr.Verbose = c.Verbose;

                                  mfr.Metrics = _Metrics;

                                  mfr.Go(pair.Value); //'SUBBA-20160713

                              }



                              if (c.Verbose == 1)
                              {
                                  System.Console.Write("Thread for + " + Convert.ToString(pair.Value) + " complete");
                              }
                              JobCount--;
                              //showMessage(ts.Success.ToString());
                          });


                }


                if (c.Verbose == 1)
                {
                    System.Console.WriteLine("\n\rQueCount :" + Convert.ToString(MyPool.QueueCount));
                    System.Console.WriteLine("\n\rPoolCount :" + Convert.ToString(MyPool.PoolCount));
                }
                Thread.Sleep(2000);

                _dID.Clear();


            }
            catch (SqlException sx)
            {
                MAXTaskCountIntervalTimeout = 1;
                using (SchedulerLog _AL = new SchedulerLog())
                {

                    _AL.ConnectionString = dbTempString;
                    _AL.UpdateLogEntry(_AppName, "FATAL ERROR SEE EVENT LOG", JobID);
                    Environment.Exit(0);
                    //        ExitFlag = true;
                }
            }
            catch (Exception ex)
            {
                MAXTaskCountIntervalTimeout = 1;
                using (SchedulerLog _AL = new SchedulerLog())
                {

                    _AL.ConnectionString = dbTempString;
                    _AL.UpdateLogEntry(_AppName, "FATAL ERROR SEE EVENT LOG", JobID);
                    Environment.Exit(0);
                    //        ExitFlag = true;
                }

            }
            finally
            {
                TaskCountIntervalTimeout = 0;
            }

            if (MAXTaskCountIntervalTimeout != 0)
            {
                _timer.Enabled = true;
            }


            //if (!NoRows)
            //{

            //    using (SchedulerLog _AL = new SchedulerLog())
            //    {
            //        _AL.ConnectionString = dbTempString;
            //        _AL.UpdateLogEntry(_AppName, "No rows for " + Convert.ToString(_BatchID) + "  for JobID " + Convert.ToString(JobID), _BatchID, 0, _TotalRulesToFire);
            //    }


            //}



        }


        /// <summary>
        /// preloads and caches data sets
        /// </summary>
        /// <returns></returns>
        private static int GetDataSets()
        {
            int r = -1;


            //  LoadFireAddressParameters

            try
            {

                _dsRulesToFire.Clear();
                using (CachedDataSets cd = new CachedDataSets())
                {

                    int i = -1;

                    cd.ConnectionString = dbTempString;
                    cd.CfgUspRulesToFire = c.CfgUspRulesToFire;

                    _dsRulesToFire.Clear();
                    _dsRulesToFire = cd.getRulesToFire(c.ContextDesc);

                    _TotalRulesToFire = _dsRulesToFire.Tables[0].Rows.Count;



                    i = cd.LoadFireAddressParameters();

                    if (i == 0)
                    {


                        c.sAddrFilter = cd.AddrFilter;
                        c.sAddrGetMultipleAddress = cd.AddrGetMultipleAddress;


                    }



                    i = -1;
                    cd.CfgUspGetRuleMsg = c.CfgUspGetRuleMsg;

                    i = cd.populateRuleMessagesHT();

                    if (i == 0)
                    {

                        c.htRuleMsgs.Clear();
                        c.htRuleMsgs = cd.htRuleMsgs;

                    }



                    GC.Collect();
                    r = 0;
                }
            }
            catch (Exception ex)
            {
                r = -1;
                log.ExceptionDetails(_AppName, ex);
            }




            return r;

        }


        /// <summary>
        /// gets all the app config entires and loads them into the config class
        /// </summary>
        static void GetAppConfig()
        {
            try
            {


                // begin db dntries

                c.ConnectionString = ConfigurationManager.AppSettings["ConnStr"];
                c.CommandTimeOut = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeOut"]);
                c.ForceClearTextPasswd = Convert.ToInt32(ConfigurationManager.AppSettings["ForceClearTextPasswd"]);
                c.DeadLockRetrys = Convert.ToInt32(ConfigurationManager.AppSettings["DeadLockRetrys"]);


                c.Metrics = Convert.ToInt32(ConfigurationManager.AppSettings["Metrics"]);
                c.Monitoring = Convert.ToInt32(ConfigurationManager.AppSettings["Monitoring"]);


                c.LogFilePath = ConfigurationManager.AppSettings["LogFilePath"];



                // begin db dntries


                c.cfgContextID = ConfigurationManager.AppSettings["cfgContextID"];
                c.HospCode = ConfigurationManager.AppSettings["cfgHospCode"];
                c.errorLogPath = ConfigurationManager.AppSettings["errorLogPath"];
                c.debugMode = ConfigurationManager.AppSettings["debugMode"];
                c.ruleDebugMode = ConfigurationManager.AppSettings["ruleDebugMode"];
                c.runMode = ConfigurationManager.AppSettings["runMode"];

                c.resultAll = ConfigurationManager.AppSettings["resultAll"];
                c.ruleSuccess = ConfigurationManager.AppSettings["ruleSuccess"];
                c.raClientIP = ConfigurationManager.AppSettings["raClientIP"];
                c.ruleDisplayLimit = ConfigurationManager.AppSettings["ruleDisplayLimit"];


                c.ConsoleName = ConfigurationManager.AppSettings["ConsoleName"];

                c.ContextDesc = ConfigurationManager.AppSettings["ContextDesc"];

                c.iMaxRowsToProcess = Convert.ToInt32(ConfigurationManager.AppSettings["iMaxRowsToProcess"]);

                c.CfgUspGetAllData = ConfigurationManager.AppSettings["CfgUspGetAllData"];
                //    c.CfgUspGetAllDataHL7Rows = ConfigurationManager.AppSettings["CfgUspGetAllDataHL7Rows"]; //SUBBA-20160713
                c.CfgUspInsertSchedulerlog = ConfigurationManager.AppSettings["CfgUspInsertSchedulerlog"];
                c.CfgUspGetRuleMsg = ConfigurationManager.AppSettings["CfgUspGetRuleMsg"];
                c.CfgUspRulesToFire = ConfigurationManager.AppSettings["CfgUspRulesToFire"];
                c.CfgUspApplyPatAuditTrial = ConfigurationManager.AppSettings["CfgUspApplyPatAuditTrial"];
                c.CfgUspTankAddress = ConfigurationManager.AppSettings["CfgUspTankAddress"];



                c.CfgUspAddCorrectAddress = ConfigurationManager.AppSettings["CfgUspAddCorrectAddress"];
                c.CfgUspRuleInsertByXmlTank = ConfigurationManager.AppSettings["CfgUspRuleInsertByXmlTank"];

                c.CfgUspRuleInsertDebug = ConfigurationManager.AppSettings["CfgUspRuleInsertDebug"];
                c.CfgUspGetAllTankById = ConfigurationManager.AppSettings["CfgUspGetAllTankById"];
                c.CfgUspTankAddress = ConfigurationManager.AppSettings["CfgUspTankAddress"];
                c.CfgUspRuleInsert = ConfigurationManager.AppSettings["CfgUspRuleInsert"];
                c.CfgUspRuleResultDelete = ConfigurationManager.AppSettings["CfgUspRuleResultDelete"];


                c.CfgUspAddMultipleAddress = ConfigurationManager.AppSettings["CfgUspAddMultipleAddress"];
                c.CfgUspAddressValidationTrail = ConfigurationManager.AppSettings["CfgUspAddressValidationTrail"];
                c.CfgUspFormatAddress = ConfigurationManager.AppSettings["CfgUspFormatAddress"];

                c.USP_GET_PAT_AUDIT_TRAIL_HL7_STARTER_IDS = ConfigurationManager.AppSettings["USP_GET_PAT_AUDIT_TRAIL_HL7_STARTER_IDS"];
                c.USP_GET_PAT_AUDIT_TRAIL_HL7_ROWS = ConfigurationManager.AppSettings["USP_GET_PAT_AUDIT_TRAIL_HL7_ROWS"];




                c.addrSvcUrl1 = ConfigurationManager.AppSettings["addrSvcUrl1"];
                c.addrSvcUrl2 = ConfigurationManager.AppSettings["addrSvcUrl2"];
                c.AddressRunMode = ConfigurationManager.AppSettings["AddressRunMode"];
                c.addrSvcPingLogMin = ConfigurationManager.AppSettings["addrSvcPingLogMin"];
                c.customerID = ConfigurationManager.AppSettings["customerID"];
                c.licenseKey = ConfigurationManager.AppSettings["licenseKey"];






                c.MinThreads = ConfigurationManager.AppSettings["MinThreads"];
                c.MaxThreads = ConfigurationManager.AppSettings["MaxThreads"];
                c.MinWait = ConfigurationManager.AppSettings["MinWait"];
                c.MaxWait = ConfigurationManager.AppSettings["MaxWait"];
                c.SchedulingInterval = ConfigurationManager.AppSettings["SchedulingInterval"];
                c.CleanupInterval = ConfigurationManager.AppSettings["CleanupInterval"];



                c.ThreadsON = ConfigurationManager.AppSettings["ThreadsON"];

                c.MaxCount = ConfigurationManager.AppSettings["MaxCount"];
                c.MaxThreads = ConfigurationManager.AppSettings["MaxThreads"];
                c.ThreadsON = ConfigurationManager.AppSettings["ThreadsON"];

                c.WaitForEnterToExit = ConfigurationManager.AppSettings["WaitForEnterToExit"];
                c.NoDataTimeOut = ConfigurationManager.AppSettings["CommandTimeOut"];
                c.Verbose = Convert.ToInt32(ConfigurationManager.AppSettings["verbose"]);


                c.dbPoll = Convert.ToInt32(ConfigurationManager.AppSettings["dbPoll"]);
                c.dbPollTimeMilliSeconeds = Convert.ToInt32(ConfigurationManager.AppSettings["dbPollTimeMiliSeconeds"]);
                c.ThreadIntervalTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["ThreadIntervalTimeout"]);



                _RecycleWaitTime = Convert.ToInt32(ConfigurationManager.AppSettings["RecycleWaitTime"]);
                _RecycleWaitTimeReset = Convert.ToInt32(ConfigurationManager.AppSettings["RecycleWaitTimeReset"]);
                _RecycleWaitTimeResetMAXCount = Convert.ToInt32(ConfigurationManager.AppSettings["RecycleWaitTimeResetMAXCount"]);




                _Metrics = Convert.ToInt32(ConfigurationManager.AppSettings["Metrics"]);

                MAXTaskCountIntervalTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["MAXTaskCountIntervalTimeout"]);




                c.RULE_MESSAGE_DELIMETER = ConfigurationManager.AppSettings["RULE_MESSAGE_DELIMETER"];

                //    _TimeToWiatForThreadsToCompleteOnRecycle = Convert.ToInt32(ConfigurationManager.AppSettings["TimeToWiatForThreadsToCompleteOnRecycle"]);



            }
            catch (Exception ex)
            {
                el.WriteEventError(_AppName + "  Unable to laod Configuration Data app exited", 1);
                if (c.Verbose == 1)
                {
                    System.Console.WriteLine("Unable to laod Configuration Data app exited");
                }
                Environment.Exit(0);
            }

            if (c.Verbose == 1)
            {
                System.Console.WriteLine("Get Settings Complete" + "\n\r");
            }


        }



        //
        //Summary:
        //The CreateNamedPipeServerToListen is used to create a named pipe as MyTestPipe.
        //To create the pipe, i have used NamedPipeServerStream class of .Net framework 4.x
        //PipeDirection.InOut : Specifies that the pipe direction is two-way.
        //NamedPipeServerStream.MaxAllowedServerInstances :  Represents the maximum number of server instances that the system resources allow
        /// <summary>
        /// 
        /// </summary>
        private static void CreateNamedPipeServerToListen()
        {

            try
            {

                NamedPipeServerStream pipeServer = new NamedPipeServerStream(_AppName, PipeDirection.InOut, NamedPipeServerStream.MaxAllowedServerInstances);
                pipeServer.WaitForConnection();
                // before processing message run another thread
                Thread thread = new Thread(CreateNamedPipeServerToListen);
                thread.Start();
                //Process the incoming request from the connected client
                ProcessMessage(pipeServer);

                if (c.Verbose == 1)
                {
                    //   log.ExceptionDetails(_AppName, " named pipe listner is running ");


                }

            }
            catch (Exception ex)
            {
                log.ExceptionDetails(_AppName + " named pipe listner failed to start ", ex);
                Environment.Exit(0);
                //_quitEvent.Set();
            }

        }



        /// <summary>
        ///   this handles call back from the threads
        /// </summary>
        /// <param name="pipeServer"></param>
        private static void ProcessMessage(NamedPipeServerStream pipeServer)
        {

            try
            {
                string NPMsg = string.Empty;
                string verb = string.Empty;
                string payload = string.Empty;


                StreamString sss = new StreamString(pipeServer);

                NPMsg = sss.ReadString();


                verb = ss.ParseDemlimtedString(NPMsg, "|", 1);
                payload = ss.ParseDemlimtedString(NPMsg, "|", 2);



                switch (verb)
                {

                    case "BEGIN":

                        //    TaskCount++;

                        break;



                    case "END":

                        TaskCount--;

                        _TotalNumberOfRulesReportedComplete++;
                        TaskCountIntervalTimeout = 0;


                        // Guid t = Guid.Parse(payload);



                        for (int i = table.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow dr = table.Rows[i];

                            string s = dr["TaskGuid"].ToString();

                            if (s == payload)
                                dr.Delete();
                        }

                        if (table.Rows.Count == 0)
                        {



                            using (SchedulerLog _AL = new SchedulerLog())
                            {
                                _AL.ConnectionString = dbTempString;
                                _AL.UpdateLogEntry(_AppName, "All threads compled End in batchID " + Convert.ToString(_BatchID) + "  for JobID " + Convert.ToString(JobID), _BatchID, _NumberOfRows, _TotalRulesToFire);
                            }

                            if (c.Verbose == 1)
                            {
                                System.Console.WriteLine("\n\r\n\r\n\r\n\r\n\rTaskCount:" + Convert.ToString(TaskCount));
                                System.Console.WriteLine("\n\rQueCount :" + Convert.ToString(MyPool.QueueCount));
                                System.Console.WriteLine("\n\rPoolCount :" + Convert.ToString(MyPool.PoolCount));
                            }
                            if (!_DropDead)
                            {
                                //  Thread.Sleep(3000);
                                // GO_MT();
                            }


                            //DataTable does not contain records
                        }


                        break;


                    case "999":




                        _timer.Enabled = false;
                        MyPool.MinThreads = 0;

                        //    _dID.Clear();
                        Thread.Sleep(3000);

                        using (SchedulerLog _AL = new SchedulerLog())
                        {
                            _AL.ConnectionString = dbTempString;
                            _AL.UpdateLogEntry(_AppName, "Recieved a Panic from a running thread or the kill switch and shutting down", JobID);
                        }



                        Environment.Exit(0);



                        break;
                }


            }
            catch (Exception ex)
            {

                log.ExceptionDetails(_AppName, ex);
                Environment.Exit(0);
            }
            pipeServer.Close();
        }


        /// <summary>
        /// builds a memory stream
        /// </summary>
        /// <returns></returns>
        static MemoryStream BuildStream()
        {

            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            //writer.Write((System.Text.Encoding.UTF8.GetBytes(GetNextERBID())));
            //   writer.Write(GetNextERBID());

            writer.Flush();
            stream.Position = 0;



            return stream;

        }


        /// <summary>
        /// this creates the mutex and sets up all the security 
        /// </summary>
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

        /// <summary>
        /// this looks for a mutex to see if another copy is running
        /// </summary>
        /// <param name="timeOut"></param>
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



        static void BuildDataTable()
        {


            table.Columns.Add("JobID", typeof(int));
            table.Columns.Add("BatchID", typeof(int));
            table.Columns.Add("TaskGuid", typeof(string));
            table.Columns.Add("Date", typeof(DateTime));

            // Here we add five DataRows.
            //table.Rows.Add(25, "Indocin", "David", DateTime.Now);
            //table.Rows.Add(50, "Enebrel", "Sam", DateTime.Now);
            //table.Rows.Add(10, "Hydralazine", "Christoff", DateTime.Now);
            //table.Rows.Add(21, "Combivent", "Janet", DateTime.Now);
            //table.Rows.Add(100, "Dilantin", "Melanie", DateTime.Now);


        }






    }
}

