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








using System.Reflection;

using System.Reflection.Emit;



using System.Runtime.InteropServices;   //GuidAttribute
using System.Reflection;                //Assembly
using System.Security.AccessControl;    //MutexAccessRule
using System.Security.Principal;        //SecurityIdentifier


namespace Owl.Console.Report4
{
    class Program
    {

        private static int RunningTasks = 0;
        private static int numThreads = 4;

        private static configEX c = new configEX();
        private static string _AppName = "OWL.Console.Report4";
        private static Int32 MaxCount;
        private static Int32 TotalThreadCount = 0;
        private static logExecption log = new logExecption();
        private static ApplicationLog al = new ApplicationLog();
        private static LogToEventLog el = new LogToEventLog();
        private static StringStuff ss = new StringStuff();



        private static string dbTempString = string.Empty;
        private static ManualResetEvent _quitEvent = new ManualResetEvent(false);





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

        static void Main(string[] args)
        {
            GetAppConfig();


            //{
        }
        //    securitySettings.AddAccessRule(allowEveryoneRule);

        //  //  GetAppConfig();
        //    //IsSingleInstance();

        //    _AppName = c.ConsoleName;

        //  //  SingleGlobalInstance(3000);

        //    if (!hasHandle)
        //    {
        //        Environment.Exit(0);

        //    }
        //    DBUtility dbu = new DBUtility();

        //  //  dbTempString = dbu.getConnectionString(c.ConnectionString);






        //    if (dbTempString == "err" || dbTempString.Contains("Base-64"))
        //    {



        //        if (dbu.ValidateConString(c.ConnectionString))
        //        {

        //            dbTempString = c.ConnectionString;
        //            el.WriteEventWarning(_AppName + "  running with clear text constring", 1);

        //            if (c.ForceClearTextPasswd == 0)
        //            {
        //                if (c.WaitForEnterToExit == "1")
        //                {

        //                    System.Console.WriteLine("\n\rRuning with clear text passwd and ForceClearTextPasswd set to 0");
        //                    System.Console.WriteLine("\n\rPress Any Key to Exit");
        //                    System.Console.ReadKey();
        //                    System.Console.WriteLine("\n\rShutting down threads and Exiting");

        //                    Environment.Exit(0);

        //                }
        //                else
        //                {
        //                    Environment.Exit(0);

        //                }

        //            }





        //        }
        //        else
        //        {

        //            el.WriteEventError(_AppName + "  decode constring failed", 1);
        //            // realease the mutex and dsipose of it only thing left is to exit
        //            if (mutex != null)
        //            {
        //                if (hasHandle)
        //                    mutex.ReleaseMutex();
        //                mutex.Dispose();
        //            }
        //            Environment.Exit(0);
        //        }
        //    }

        //    // dbTempString = c.ConnectionString;

        //    log.ConnectionString = dbTempString;
        //    al.ConnectionString = dbTempString;

        //    using (SchedulerLog _AL = new SchedulerLog())
        //    {
        //        _AL.ConnectionString = dbTempString;
        //        JobID = _AL.AddLogEntry(_AppName, "OWL Report 4 Console");
        //        _AL.UpdateLogEntry(_AppName, "Start up for JobID: " + Convert.ToString(JobID), JobID);
        //    }






        //        using (SchedulerLog _AL = new SchedulerLog())
        //        {
        //            _AL.ConnectionString = dbTempString;
        //            _AL.UpdateLogEntry(_AppName, "Get Datasets failed", JobID);
        //        }

        //        // realease the mutex and dsipose of it only thing left is to exit
        //        if (mutex != null)
        //        {
        //            if (hasHandle)
        //                mutex.ReleaseMutex();
        //            mutex.Dispose();
        //        }

        //}



        //private static void Go_Single()
        //{
        //    int r = 0; // retrun from the UCLA class to see if its zerop
        //    //    int i = 0; // sanity count
        //    int _id = 0; // sanity count
        //    //    int x = 0; // sanity count         

        //    _BatchID = 0;
        //    _NumberOfRows = 0;

        //  //  GetDataSets();
        //    using (SchedulerLog _SL = new SchedulerLog())
        //    {
        //        _SL.ConnectionString = dbTempString;
        //        _BatchID = _SL.AddLogEntry(_AppName, "Begin batch for JobID " + Convert.ToString(JobID));
        //        _SL.UpdateLogEntry(_AppName, "Batch ID:  " + Convert.ToString(_BatchID) + "  for JobID " + Convert.ToString(JobID), _BatchID, 0, _TotalRulesToFire);
        //    }

        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(dbTempString))
        //        {
        //            con.Open();
        //            //
        //            // Create new SqlCommand object.
        //            //
        //            using (SqlCommand cmd = new SqlCommand(c.USP_GET_PAT_AUDIT_TRAIL_HL7_STARTER_IDS, con))
        //            {
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                using (SqlDataReader idr = cmd.ExecuteReader())
        //                {
        //                    //  count = idr.VisibleFieldCount;
        //                    if (idr.HasRows)
        //                    {
        //                        while (idr.Read())
        //                        {

        //                            _NumberOfRows++;
        //                            TaskCount++;
        //                            _id = Convert.ToInt32(idr["PATIENT_AUDIT_TRAIL_ID"]);







        //                            Guid tguid = Guid.NewGuid();
        //                            string sguid = string.Empty;

        //                            sguid = Convert.ToString(tguid);

        //                            table.Rows.Add(JobID, _BatchID, tguid, DateTime.Now);





        //                                c.sguid = sguid;
        //                                c.USP_GET_PAT_AUDIT_TRAIL_HL7_ROWS = c.USP_GET_PAT_AUDIT_TRAIL_HL7_ROWS;
        //                                //  c.CfgUspGetAllDataHL7Rows = c.CfgUspGetAllDataHL7Rows; //   ' //SUBBA-20160713
        //                                c.AppName = _AppName;
        //                                c.ConnectionString = dbTempString;
        //                                c.cfgContextID = c.cfgContextID;
        //                                c.HospCode = c.HospCode; //'= ConfigurationManager.AppSettings["cfgHospCode"]
        //                                c.errorLogPath = c.errorLogPath; //' = ConfigurationManager.AppSettings["errorLogPath"]
        //                                c.debugMode = c.debugMode;  //'// = ConfigurationManager.AppSettings["debugMode"]
        //                                c.ruleDebugMode = c.ruleDebugMode; //' = ConfigurationManager.AppSettings["ruleDebugMode"]
        //                                c.runMode = c.runMode; //' = ConfigurationManager.AppSettings["runMode"]
        //                                c.resultAll = c.resultAll; //'  **** = ConfigurationManager.AppSettings["resultAll"]
        //                                c.raClientIP = c.raClientIP; //'= ConfigurationManager.AppSettings["raClientIP"]
        //                                c.ruleSuccess = c.ruleSuccess; //' = ConfigurationManager.AppSettings["ruleSuccess"]
        //                                c.AddressRunMode = c.AddressRunMode; //'= ConfigurationManager.AppSettings["addressRunMode"]
        //                                c.addrSvcUrl1 = c.addrSvcUrl1; //' = ConfigurationManager.AppSettings["addrSvcUrl1"]
        //                                c.addrSvcUrl2 = c.addrSvcUrl2; //' = ConfigurationManager.AppSettings["isParseVBorDB"]
        //                                c.addrSvcPingLogMin = c.addrSvcPingLogMin; // ' = ConfigurationManager.AppSettings["addrSvcPingLogMin"]
        //                                //c.CfgUspGetAllData = c.CfgUspGetAllData; // ' = ConfigurationManager.AppSettings["CfgUspGetAllData"]
        //                                ////   c.CfgUspGetAllDataHL7Rows = c.CfgUspGetAllDataHL7Rows;
        //                                //c.CfgUspInsertSchedulerlog = c.CfgUspInsertSchedulerlog; // ' = ConfigurationManager.AppSettings["CfgUspInsertSchedulerlog"]
        //                                //c.CfgUspGetRuleMsg = c.CfgUspGetRuleMsg; //'= ConfigurationManager.AppSettings["CfgUspGetRuleMsg"]
        //                                //c.CfgUspRulesToFire = c.CfgUspRulesToFire;  // '// = ConfigurationManager.AppSettings["CfgUspRulesToFire"]
        //                                //c.CfgUspApplyPatAuditTrial = c.CfgUspApplyPatAuditTrial; // ' = ConfigurationManager.AppSettings["CfgUspApplyPatAuditTrial"]
        //                                //c.CfgUspTankAddress = c.CfgUspTankAddress;
        //                                //c.CfgUspAddCorrectAddress = c.CfgUspAddCorrectAddress; // ' = ConfigurationManager.AppSettings["CfgUspAddCorrectAddress"]
        //                                //c.CfgUspAddMultipleAddress = c.CfgUspAddMultipleAddress;
        //                                //c.CfgUspFormatAddress = c.CfgUspFormatAddress;
        //                                //c.CfgUspAddressValidationTrail = c.CfgUspAddressValidationTrail;
        //                                //c.CfgUspGetAllTankById = c.CfgUspGetAllTankById;
        //                                //c.CfgUspRuleInsert = c.CfgUspRuleInsert;
        //                                //c.CfgUspRuleResultDelete = c.CfgUspRuleResultDelete;
        //                                //c.CfgUspRuleInsertByXmlTank = c.CfgUspRuleInsertByXmlTank;  // '
        //                                //c.CfgUspRuleInsertDebug = c.CfgUspRuleInsertDebug;
        //                                //c.ruleDisplayLimit = c.ruleDisplayLimit; // ' = ConfigurationManager.AppSettings["ruleDisplayLimit"]
        //                                //c.customerID = c.customerID; // ' = ConfigurationManager.AppSettings["customerID"]
        //                                //c.licenseKey = c.licenseKey; // '= ConfigurationManager.AppSettings["licenseKey"]
        //                                //c.dsRulesToFire = _dsRulesToFire;
        //                                //c.TotalRulesToFire = _TotalRulesToFire;
        //                                //c.iMaxRowsToProcess = c.iMaxRowsToProcess;
        //                                //c.RULE_MESSAGE_DELIMETER = c.RULE_MESSAGE_DELIMETER;
        //                                //c.JobID = JobID;
        //                                //c.TotalRulesToFire = _TotalRulesToFire;





        //                                //c.htRuleMsgs = c.htRuleMsgs;

        //                                ////c.AddressFilter = c.sAddrFilter;
        //                                //c.GetMultipleAddress = c.sAddrGetMultipleAddress;




        //                                c.LogFilePath = c.LogFilePath;
        //                                c.Verbose = c.Verbose;

        //                                c.Metrics = _Metrics;

        //                                //c.Go(_id); //'SUBBA-20160713


        //                                //  c.Go(pair.Value); //'SUBBA-20160713
        //                                //  c.Go(_id); //'SUBBA-20160713

        //                            }

        //                        }
        //                    }
        //                }
        //                // 
        //                r = 0;
        //            }
        //            con.Close();
        //        }

        //    }
        //    catch (SqlException sx)
        //    {

        //        log.ExceptionDetails(_AppName + " Main" + " SQL Exception - Failed DCSGlobal.Console.c  ", sx);
        //        using (SchedulerLog _AL = new SchedulerLog())
        //        {

        //            _AL.ConnectionString = dbTempString;
        //            _AL.UpdateLogEntry(_AppName, "FATAL ERROR SEE EVENT LOG", JobID);
        //            Environment.Exit(0);
        //            //        ExitFlag = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        log.ExceptionDetails(_AppName + " Main" + " Exception - Failed DCSGlobal.Console.c  ", ex);
        //           static void MainEX(string[] args)     Environment.Exit(0);

        //        // _quitEvent.Set();

        //    }
        //    // _timer.Enabled = true;

        //}}



        ///// <summary>
        ///// gets all the app config entires and loads them into the config class
        ///// </summary>
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










    }
}

