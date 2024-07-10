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

using System.Reflection.Emit;



using System.Runtime.InteropServices;   //GuidAttribute
using System.Reflection;                //Assembly
using System.Security.AccessControl;    //MutexAccessRule
using System.Security.Principal;        //SecurityIdentifier


namespace DCSGlobal.Console.MT.Rules
{
    class Programv3
    {

        //   private static int RunningTasks = 0;

        private static Config c = new Config();
        private static string _AppName = "DCSGlobal.Console.MT.Rules";

        private static logExecption log = new logExecption();
        private static ApplicationLog al = new ApplicationLog();
        private static LogToEventLog el = new LogToEventLog();
        private static StringStuff ss = new StringStuff();


        private static string dbTempString = string.Empty;


        private static bool DropDead = false;

        // Here we create a DataTable with four columns.
        private static DataTable table = new DataTable();



        private static DateTime _StartTime = DateTime.Now;
        private static DateTime _CurrentTime = DateTime.Now;
        private static DateTime _ShutDownTime;
        private static TimeSpan _ElapsedTime;
        private static TimeSpan _ElapsedWaitTimeToRecycle;
        private static int _TimeToWiatForThreadsToCompleteOnRecycle = 0;
        private static bool _TimeToDie = false;
        private static int _RecycleTime = 0;

        private static int _TotalNumberOfRulesRan = 0;
        private static int _NumberOfRowsCompleted = 0;
        private static int _TotalNumberOfRowsSent = 0;
        private static int _NumberOfRows = 0;

        private static int _TotalRulesToFire = 0;


        private static int _BatchID = 0;




        //private static int _Monitoring = 0;
        // private static int _Metrics = 0;
        // private static string _MetricsString = string.Empty;
        //private static int MaxQueDepth = 500;

        // private static int _MinRulesTime = 2000;
        // private static int _MinRules = 0;
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
        //    private static int TaskCountIntervalTimeout = 0;
        //   private static int MAXTaskCountIntervalTimeout = 0;




        //  private static int ThreadIntervalTimeout = 0;

        //  private static bool _DropDead = false;
        // private static int _RecycleWaitTime = 600000;
        // private static int _RecycleWaitTimeReset = 0;
        // private static int _RecycleWaitTimeResetMAXCount = 0;
        // private static int _RecycleWaitTimeResetCount = 0;



        ~Programv3()
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

        static void Main(string[] args)
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




            //DBUtility dbu = new DBUtility();

            //dbTempString = dbu.getConnectionString(c.ConnectionString);






            //if (dbTempString == "err" || dbTempString.Contains("Base-64"))
            //{



            //    if (dbu.ValidateConString(c.ConnectionString))
            //    {

            //        dbTempString = c.ConnectionString;
            //        el.WriteEventWarning(_AppName + "  running with clear text constring", 1);

            //        if (c.ForceClearTextPasswd == 0)
            //        {
            //            if (c.WaitForEnterToExit == "1")
            //            {

            //                System.Console.WriteLine("\n\rRuning with clear text passwd and ForceClearTextPasswd set to 0");
            //                System.Console.WriteLine("\n\rPress Any Key to Exit");
            //                System.Console.ReadKey();
            //                System.Console.WriteLine("\n\rShutting down threads and Exiting");

            //                Environment.Exit(0);

            //            }
            //            else
            //            {
            //                Environment.Exit(0);

            //            }

            //        }





            //    }
            //    else
            //    {

            //        el.WriteEventError(_AppName + "  decode constring failed", 1);
            //        // realease the mutex and dsipose of it only thing left is to exit
            //        if (mutex != null)
            //        {
            //            if (hasHandle)
            //                mutex.ReleaseMutex();
            //            mutex.Dispose();
            //        }
            //        Environment.Exit(0);
            //    }
            //}


            dbTempString = "Data Source=10.1.1.120;Initial Catalog=al60_eastmaine_prod;Persist Security Info=True;User ID=al60_seton_lite_developer_user;Password=al60_seton_lite_developer_password";


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
                // Environment.Exit(0);

            }







            MyPool = CustomThreadPool.Instance;

            MyPool.MinThreads = Convert.ToInt32(c.MinThreads);
            MyPool.MaxThreads = Convert.ToInt32(c.MaxThreads);
            MyPool.MinWait = Convert.ToInt32(c.MinWait);
            MyPool.MaxWait = Convert.ToInt32(c.MaxWait);
            MyPool.SchedulingInterval = Convert.ToInt32(c.SchedulingInterval);
            MyPool.CleanupInterval = Convert.ToInt32(c.CleanupInterval);

            Thread.Sleep(c.dbPollTimeMilliSeconeds);




            _CurrentTime = DateTime.Now;

            GO();


        }


        /// <summary>
        /// this runs inline in a MT role
        /// </summary>
        private static void GO()
        {

            do
            {
                Thread.Sleep(2000);
                _CurrentTime = DateTime.Now;
                _ElapsedTime = _CurrentTime - _StartTime;

                _ElapsedWaitTimeToRecycle = _CurrentTime - _ShutDownTime;


                double Minutes = _ElapsedTime.TotalMinutes;
                double Seconds = _ElapsedWaitTimeToRecycle.TotalSeconds;

                if (_TimeToDie)
                {
                    if (Seconds > _TimeToWiatForThreadsToCompleteOnRecycle)
                    {
                        DropDead = true;
                    }
                }

                if (Minutes > _RecycleTime)
                {
                    if (!_TimeToDie)
                    {
                        _ShutDownTime = DateTime.Now;
                        _TimeToDie = true;
                    }

                }






                //_TimeToWiatForThreadsToCompleteOnRecycle

                //_ShutDownTime


                if (TaskCount == 0)
                {
                    if (!_TimeToDie)
                    {
                        GO_MT();
                    }
                    else
                    {
                        DropDead = true;
                    }
                }
                if (TaskCount < 0)
                {
                    DropDead = true;
                }



            }

            while (!DropDead);



            using (SchedulerLog _AL = new SchedulerLog())
            {
                _AL.ConnectionString = dbTempString;
                _AL.UpdateLogEntry(_AppName, "Shutdown for JobID: " + Convert.ToString(JobID), JobID, _NumberOfRowsCompleted, _TotalNumberOfRowsSent);
            }


            Environment.Exit(0);





        }





        /// <summary>
        /// this runs inline in a MT role
        /// </summary>
        private static void GO_MT()
        {



            int r = 0; // retrun from the UCLA class to see if its zerop
            // int i = 0; // sanity count
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
                    _TotalNumberOfRowsSent++;
                    _NumberOfRows++;
                    TaskCount++;

                    Guid tguid = Guid.NewGuid();
                    string sguid = string.Empty;

                    sguid = Convert.ToString(tguid);

                    table.Rows.Add(JobID, _BatchID, tguid, DateTime.Now);
                    //   mfr.sguid = sguid;

                    //MyPool.QueueUserTask(() =>
                    //{
                    //    JobCount++;





                    //},
                    //      (ts) =>
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

                            mfr.useDLL = c.useDLL; 


                            mfr.LogFilePath = c.LogFilePath;
                            mfr.Verbose = c.Verbose;

                            //   mfr.Metrics = _Metrics;

                            mfr.Go(pair.Value); //'SUBBA-20160713  341

                        }



                        if (c.Verbose == 1)
                        {
                            System.Console.Write("Thread for + " + Convert.ToString(pair.Value) + " complete");
                        }
                        JobCount--;
                        //showMessage(ts.Success.ToString());
                        //});


                        //}


                        if (c.Verbose == 1)
                        {
                            System.Console.WriteLine("\n\rQueCount :" + Convert.ToString(MyPool.QueueCount));
                            System.Console.WriteLine("\n\rPoolCount :" + Convert.ToString(MyPool.PoolCount));
                        }
                        Thread.Sleep(2000);

                        _dID.Clear();


                    }
                }
            }

            catch (Exception ex)
            {

                log.ExceptionDetails(_AppName, ex);

                using (SchedulerLog _AL = new SchedulerLog())
                {

                    _AL.ConnectionString = dbTempString;
                    _AL.UpdateLogEntry(_AppName, "FATAL ERROR SEE EVENT LOG", JobID);
                    DropDead = true;
                    //        ExitFlag = true;
                }

            }





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
                DropDead = true;

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
                c.ConsoleName = ConfigurationManager.AppSettings["ConsoleName"];
                c.Verbose = Convert.ToInt32(ConfigurationManager.AppSettings["verbose"]);


                _RecycleTime = Convert.ToInt32(ConfigurationManager.AppSettings["RecycleTime"]);

                _TimeToWiatForThreadsToCompleteOnRecycle = Convert.ToInt32(ConfigurationManager.AppSettings["TimeToWiatForThreadsToCompleteOnRecycle"]);



                c.ThreadsON = ConfigurationManager.AppSettings["ThreadsON"];
                c.MinThreads = ConfigurationManager.AppSettings["MinThreads"];
                c.MaxThreads = ConfigurationManager.AppSettings["MaxThreads"];
                c.MinWait = ConfigurationManager.AppSettings["MinWait"];
                c.MaxWait = ConfigurationManager.AppSettings["MaxWait"];
                c.CleanupInterval = ConfigurationManager.AppSettings["CleanupInterval"];
                c.SchedulingInterval = ConfigurationManager.AppSettings["SchedulingInterval"];


                //    c.CfgUspGetAllDataHL7Rows = ConfigurationManager.AppSettings["CfgUspGetAllDataHL7Rows"]; //SUBBA-20160713





                // begin the rules section
                c.iMaxRowsToProcess = Convert.ToInt32(ConfigurationManager.AppSettings["iMaxRowsToProcess"]);
                c.ContextDesc = ConfigurationManager.AppSettings["ContextDesc"];
                c.RULE_MESSAGE_DELIMETER = ConfigurationManager.AppSettings["RULE_MESSAGE_DELIMETER"];

                c.USP_GET_PAT_AUDIT_TRAIL_HL7_STARTER_IDS = ConfigurationManager.AppSettings["USP_GET_PAT_AUDIT_TRAIL_HL7_STARTER_IDS"];
                c.USP_GET_PAT_AUDIT_TRAIL_HL7_ROWS = ConfigurationManager.AppSettings["USP_GET_PAT_AUDIT_TRAIL_HL7_ROWS"];

                c.CfgUspInsertSchedulerlog = ConfigurationManager.AppSettings["CfgUspInsertSchedulerlog"];




                // c.CfgUspGetAllData = ConfigurationManager.AppSettings["CfgUspGetAllData"];
                c.CfgUspGetRuleMsg = ConfigurationManager.AppSettings["CfgUspGetRuleMsg"];
                c.CfgUspRulesToFire = ConfigurationManager.AppSettings["CfgUspRulesToFire"];

                c.CfgUspApplyPatAuditTrial = ConfigurationManager.AppSettings["CfgUspApplyPatAuditTrial"];


                c.CfgUspTankAddress = ConfigurationManager.AppSettings["CfgUspTankAddress"];
                c.CfgUspAddCorrectAddress = ConfigurationManager.AppSettings["CfgUspAddCorrectAddress"];
                c.CfgUspAddMultipleAddress = ConfigurationManager.AppSettings["CfgUspAddMultipleAddress"];
                c.CfgUspFormatAddress = ConfigurationManager.AppSettings["CfgUspFormatAddress"];
                c.CfgUspAddressValidationTrail = ConfigurationManager.AppSettings["CfgUspAddressValidationTrail"];

                c.CfgUspGetAllTankById = ConfigurationManager.AppSettings["CfgUspGetAllTankById"];


                c.CfgUspRuleInsert = ConfigurationManager.AppSettings["CfgUspRuleInsert"];
                c.CfgUspRuleResultDelete = ConfigurationManager.AppSettings["CfgUspRuleResultDelete"];
                c.CfgUspRuleInsertByXmlTank = ConfigurationManager.AppSettings["CfgUspRuleInsertByXmlTank"];
                c.CfgUspRuleInsertDebug = ConfigurationManager.AppSettings["CfgUspRuleInsertDebug"];




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



                c.AddressRunMode = ConfigurationManager.AppSettings["AddressRunMode"];

                c.addrSvcPingLogMin = ConfigurationManager.AppSettings["addrSvcPingLogMin"];
                c.customerID = ConfigurationManager.AppSettings["customerID"];
                c.licenseKey = ConfigurationManager.AppSettings["licenseKey"];

                c.CfgAddrSvcUrl = ConfigurationManager.AppSettings["CfgAddrSvcUrl"];
                c.addrSvcUrl1 = ConfigurationManager.AppSettings["addrSvcUrl1"];
                c.addrSvcUrl2 = ConfigurationManager.AppSettings["addrSvcUrl2"];



                string _useDLL = string.Empty;
                _useDLL = ConfigurationManager.AppSettings["useDLL"];


                if (_useDLL == "1")
                {
                    c.useDLL = true;
                }
                else
                {
                    c.useDLL = false;
                }













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
                DropDead = true;
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
                        _NumberOfRowsCompleted++;


                        if (TaskCount == 0)
                        {
                            using (SchedulerLog _AL = new SchedulerLog())
                            {
                                _AL.ConnectionString = dbTempString;
                                _AL.UpdateLogEntry(_AppName, "All threads compled End in batchID " + Convert.ToString(_BatchID) + "  for JobID " + Convert.ToString(JobID), _BatchID, _NumberOfRows, _TotalRulesToFire);
                            }
                        }
                        break;


                    case "999":

                        using (SchedulerLog _AL = new SchedulerLog())
                        {
                            _AL.ConnectionString = dbTempString;
                            _AL.UpdateLogEntry(_AppName, "Recieved a Panic from a running thread or the kill switch and shutting down", JobID);
                        }
                        DropDead = true;
                        break;
                }


            }
            catch (Exception ex)
            {

                log.ExceptionDetails(_AppName, ex);
                DropDead = true;
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

