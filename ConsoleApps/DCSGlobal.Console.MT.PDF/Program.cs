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


using System.Runtime.InteropServices;   //GuidAttribute
using System.Reflection;                //Assembly
using System.Security.AccessControl;    //MutexAccessRule
using System.Security.Principal;        //SecurityIdentifier


namespace  DCSGlobal.Console.MT.PDF
{
    class Program
    {


        private static int numThreads = 4;
        private static int iEDIKey = 0;
        private static int iEDICKey = 0;
        private static config c = new config();
        private static string AppName = "DCSGlobal.Console.MT.PDF";
        private static Int32 MaxCount;
        private static Int32 TotalThreadCount = 0;
        private static logExecption log = new logExecption();
        private static ApplicationLog al = new ApplicationLog();
        private static LogToEventLog el = new LogToEventLog();

        private static System.Timers.Timer _timer; // From System.Timers
        private static string dbTempString = string.Empty;
        private static ManualResetEvent _quitEvent = new ManualResetEvent(false);



        private static CustomThreadPool MyPool;
        private static Dictionary<int, int> _dID = new Dictionary<int, int>();
        private static int JobCount = 0;
        private static int JobID = 0;

        private static Mutex _m;
        private static MutexAccessRule allowEveryoneRule = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow);
        private static MutexSecurity securitySettings = new MutexSecurity();
        private static bool createdNew;
        public static bool hasHandle = false;
        private static Mutex mutex;

        private static int TaskCount = 0;
        private static int ThreadIntervalTimeout = 0;
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

        static void Main(string[] args)
        {

            securitySettings.AddAccessRule(allowEveryoneRule);

            GetAppConfig();
            //IsSingleInstance();

            AppName = c.ConsoleName;

            SingleGlobalInstance(3000);

            if (!hasHandle)
            {
                Environment.Exit(0);

            }


            MyPool = CustomThreadPool.Instance;


            DBUtility dbu = new DBUtility();

            dbTempString = dbu.getConnectionString(c.ConnectionString);



            if (dbTempString == "err")
            {



                if (dbu.ValidateConString(c.ConnectionString))
                {

                    dbTempString = c.ConnectionString;
                    el.WriteEventWarning(AppName + "  running with clear text constring", 1);
                }
                else
                {

                    el.WriteEventError(AppName + "  decode constring failed", 1);
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

            dbTempString = c.ConnectionString;

            log.ConnectionString = dbTempString;
            al.ConnectionString = dbTempString;

            using (SchedulerLog _SL = new SchedulerLog())
            {
                _SL.ConnectionString = dbTempString;
                JobID = _SL.AddLogEntry(AppName, "MT Rules Console");
            }


            Thread thread = new Thread(CreateNamedPipeServerToListen);
            thread.Start();

            if (c.verbose == 1)
            {
                log.ExceptionDetails(AppName, " listner start");


            }







            MyPool.MinThreads = Convert.ToInt32(c.MinThreads);
            MyPool.MaxThreads = Convert.ToInt32(c.MaxThreads);
            MyPool.MinWait = Convert.ToInt32(c.MinWait);
            MyPool.MaxWait = Convert.ToInt32(c.MaxWait);
            MyPool.SchedulingInterval = Convert.ToInt32(c.SchedulingInterval);
            MyPool.CleanupInterval = Convert.ToInt32(c.CleanupInterval);

            // dosomehting between here



            if (c.ThreadsON == "1")
            {

                Thread.Sleep(c.dbPollTimeMilliSeconeds);
                _timer = new System.Timers.Timer(c.dbPollTimeMilliSeconeds);
                _timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
                _timer.Enabled = false; // Enable it


                if (c.verbose == 1)
                {
                    System.Console.WriteLine("\n\rThreads ON");
                }
                GO_MT();
                _quitEvent.WaitOne();
            }
            else
            {
                if (c.verbose == 1)
                {
                    System.Console.WriteLine("\n\rThreads OFF");
                }
                Go_Single();
            }




            using (SchedulerLog _AL = new SchedulerLog())
            {
                _AL.ConnectionString = dbTempString;
                _AL.UpdateLogEntry(AppName, "This is the end", JobID);
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



        static void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (c.verbose == 1)
            {
                System.Console.WriteLine("\n\rTimer Fired  current job count  " + Convert.ToString(JobCount));
            }

            if (TaskCount == 0)
            {
                ThreadIntervalTimeout = 0;
                GO_MT();

            }
            else
            {
                ThreadIntervalTimeout++;

            }

            if (ThreadIntervalTimeout == c.ThreadIntervalTimeout)
            {
                log.ExceptionDetails(AppName, "ThreadIntervalTimeout interval exceeded ");
                _quitEvent.Set();
            }


            //   GC.Collect();
        }

        private static void Go_Single()
        {
            int r = 0; // retrun from the UCLA class to see if its zerop
            int i = 0; // sanity count
            int _id = 0; // sanity count
            int x = 0; // sanity count         

            //     _timer.Enabled = false;
            try
            {

                /******************************************************************************************
                 * Do what ever between here and here
                 * dont actually type just call a class in a dll and get it runing 
                 * if is vb it must use option strict and option explicit 
                 * and it needs to implement Idisposable
                 * do not put in the exe
                 * ****************************************************************************************/







                /******************************************************************************************
                 * Do what ever between here and here
                * ****************************************************************************************/




            }
            catch (SqlException sx)
            {

                log.ExceptionDetails(AppName + " Main" + " SQL Exception - Failed DCSGlobal.Console.mfr  ", sx);
                _quitEvent.Set();
            }
            catch (Exception ex)
            {

                log.ExceptionDetails(AppName + " Main" + " Exception - Failed DCSGlobal.Console.mfr  ", ex);

                _quitEvent.Set();

            }
            // _timer.Enabled = true;

        }



        private static void GO_MT()
        {
            int r = 0; // retrun from the UCLA class to see if its zerop
            int i = 0; // sanity count
            int _id = 0; // sanity count
            int x = 0; // sanity count         

            _timer.Enabled = false;
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


                if (c.verbose == 1)
                {
                    System.Console.Write("pool count " + Convert.ToString(MyPool.PoolCount));
                }


                foreach (KeyValuePair<int, int> pair in _dID)
                {
                    MyPool.QueueUserTask(() =>
                    {
                        JobCount++;
                        if (c.verbose == 1)
                        {
                            System.Console.Write("Thread for + " + Convert.ToString(pair.Value) + " started");
                        }


                    },
                          (ts) =>
                          {

                                /******************************************************************************************
                                * Do what ever between here and here
                                * dont actually type just call a class in a dll and get it runing 
                                * if is vb it must use option strict and option explicit 
                                * and it needs to implement Idisposable
                                * do not put in the exe
                                 * 
                                 * if you need to pass a key USE pair.value
                                * ****************************************************************************************/







                              /******************************************************************************************
                               * Do what ever between here and here
                              * ****************************************************************************************/
                              JobCount--;
                              //showMessage(ts.Success.ToString());
                          });


                }

                _dID.Clear();


            }
            catch (SqlException sx)
            {

                log.ExceptionDetails(AppName + " Main" + " SQL Exception - Failed DCSGlobal.Console.Rules.inline  ", sx);
                _quitEvent.Set();
            }
            catch (Exception ex)
            {

                log.ExceptionDetails(AppName + " Main" + " Exception - Failed DCSGlobal.Console.Rules.inline    ", ex);

                _quitEvent.Set();

            }
            //      _timer.Enabled = true;

        }







        static void GetAppConfig()
        {
            try
            {
                c.ConnectionString = ConfigurationManager.AppSettings["ConnStr"];
                c.ConsoleName = ConfigurationManager.AppSettings["ConsoleName"];
                c.DeadLockRetrys = Convert.ToInt32(ConfigurationManager.AppSettings["DeadLockRetrys"]);



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


                c.CfgUspGetAllData = ConfigurationManager.AppSettings["CfgUspGetAllData"];
                c.CfgUspGetAllDataHL7Rows = ConfigurationManager.AppSettings["CfgUspGetAllDataHL7Rows"]; //SUBBA-20160713
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
                c.addressRunMode = ConfigurationManager.AppSettings["addressRunMode"];
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
                c.verbose = Convert.ToInt32(ConfigurationManager.AppSettings["verbose"]);


                c.dbPoll = Convert.ToInt32(ConfigurationManager.AppSettings["dbPoll"]);
                c.dbPollTimeMilliSeconeds = Convert.ToInt32(ConfigurationManager.AppSettings["dbPollTimeMiliSeconeds"]);
                c.ThreadIntervalTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["ThreadIntervalTimeout"]);



            }
            catch (Exception ex)
            {
                el.WriteEventError(AppName + "  Unable to laod Configuration Data app exited", 1);
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



        //
        //Summary:
        //The CreateNamedPipeServerToListen is used to create a named pipe as MyTestPipe.
        //To create the pipe, i have used NamedPipeServerStream class of .Net framework 4.x
        //PipeDirection.InOut : Specifies that the pipe direction is two-way.
        //NamedPipeServerStream.MaxAllowedServerInstances :  Represents the maximum number of server instances that the system resources allow
        private static void CreateNamedPipeServerToListen()
        {
            NamedPipeServerStream pipeServer =
               new NamedPipeServerStream(c.ConsoleName, PipeDirection.InOut, NamedPipeServerStream.MaxAllowedServerInstances);
            pipeServer.WaitForConnection();
            // before processing message run another thread
            Thread thread = new Thread(CreateNamedPipeServerToListen);
            thread.Start();
            //Process the incoming request from the connected client
            ProcessMessage(pipeServer);

            if (c.verbose == 1)
            {
                log.ExceptionDetails(AppName, " named pipe listner is running ");


            }

        }

        private static void ProcessMessage(NamedPipeServerStream pipeServer)
        {

            try
            {



                StreamString sss = new StreamString(pipeServer);

                //   sss.WriteString(GetNextERBID());

            }
            catch (IOException e)
            {

                log.ExceptionDetails(AppName, Convert.ToString(e));

            }
            pipeServer.Close();
        }



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




    }
}

