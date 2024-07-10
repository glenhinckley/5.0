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


namespace DCSGlobal.Console.RealAlertServer
{
    class Program
    {

        private static int RunningTasks = 0;
        private static int numThreads = 4;
        private static int iEDIKey = 0;
        private static int iEDICKey = 0;
        private static config c = new config();
        private static string _AppName = "DCSGlobal.Console.RealAlert.Server";



        private static logExecption log = new logExecption();
        private static ApplicationLog al = new ApplicationLog();
        private static LogToEventLog el = new LogToEventLog();


        private static DataTable TheTable = new DataTable();
        private static Boolean LockServer = true;

        private static string _client_code = string.Empty;

        private static int _RefreshRate = 20000;
        private static string _InstanceName = "TestMode";
        static bool NoData = true;



        private static bool DropDead = false;
        private static int _TimeToWiatForThreadsToCompleteOnRecycle = 0;


        private static System.Timers.Timer _timer; // From System.Timers
        private static System.Timers.Timer _MinRulesTimer; // From System.Timers
        private static System.Timers.Timer _DropDeadTimer; // From System.Timers

        private static System.Timers.Timer _MKC; // From System.Timers


        private static string dbTempString = string.Empty;
        private static ManualResetEvent _quitEvent = new ManualResetEvent(false);







        private static CustomThreadPool MyPool;

        private static int JobID = 0;

        private static Mutex _m;
        private static MutexAccessRule allowEveryoneRule = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow);
        private static MutexSecurity securitySettings = new MutexSecurity();
        private static bool createdNew;
        public static bool hasHandle = false;
        private static Mutex mutex;








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

            _AppName = c.ConsoleName;
            _InstanceName = c.ConsoleName;
            SingleGlobalInstance(3000);

            if (!hasHandle)
            {
                Environment.Exit(0);

            }




            DBUtility dbu = new DBUtility();


            if (c.ForceClearTextPasswd != 1)
            {
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
            }
            else
            {

                dbTempString = c.ConnectionString;
            }
            // 

            log.ConnectionString = dbTempString;
            al.ConnectionString = dbTempString;

            using (SchedulerLog _AL = new SchedulerLog())
            {
                _AL.ConnectionString = dbTempString;
                JobID = _AL.AddLogEntry(_AppName, "RealAlert Server Console");
                ///AL.UpdateLogEntry(_AppName, "Start up for JobID: " + Convert.ToString(JobID), JobID);
            }





            GO();







        }



        static void GO()
        {

            BuildDataTable();
            RefreshData();
            DumpTheTable();

            // check to see if the drop dead flag was set if not start the listner 
            if (!DropDead)
            {
                Thread thread = new Thread(CreateNamedPipeServerToListen);
                thread.Start();

            }


            do
            {
                Thread.Sleep(2000);
                try
                {


                    ///

                    // Load the data into it.

                    //  DumpTheTable();

                    if (!LockServer)
                    {
                        System.Console.WriteLine("refresh data");
                        RefreshData();
                        // Thread thread = new Thread(RefreshData);
                        // thread.Start();
                    }



                }
                catch (Exception ex)
                {

                    log.ExceptionDetails(_AppName, ex);
                    DropDead = true;
                }

            }

            while (!DropDead);


            using (SchedulerLog _AL = new SchedulerLog())
            {
                _AL.ConnectionString = dbTempString;
                // JobID = _AL.AddLogEntry(_AppName, "RealAlert Server Console");
                _AL.UpdateLogEntry(_AppName, "RealAlert Server Console ShutDown ", 0, 0, 0);
            }


            if (mutex != null)
            {
                if (hasHandle)
                    mutex.ReleaseMutex();
                mutex.Dispose();
            }
            Environment.Exit(0);



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

                c.ContextDesc = ConfigurationManager.AppSettings["ContextDesc"];

                c.iMaxRowsToProcess = Convert.ToInt32(ConfigurationManager.AppSettings["iMaxRowsToProcess"]);








                c.MinThreads = ConfigurationManager.AppSettings["MinThreads"];
                c.MaxThreads = ConfigurationManager.AppSettings["MaxThreads"];
                c.MinWait = ConfigurationManager.AppSettings["MinWait"];
                c.MaxWait = ConfigurationManager.AppSettings["MaxWait"];
                c.SchedulingInterval = ConfigurationManager.AppSettings["SchedulingInterval"];
                c.CleanupInterval = ConfigurationManager.AppSettings["CleanupInterval"];





                c.MaxCount = ConfigurationManager.AppSettings["MaxCount"];
                c.MaxThreads = ConfigurationManager.AppSettings["MaxThreads"];
                c.ThreadsON = ConfigurationManager.AppSettings["ThreadsON"];

                c.WaitForEnterToExit = ConfigurationManager.AppSettings["WaitForEnterToExit"];
                c.NoDataTimeOut = ConfigurationManager.AppSettings["CommandTimeOut"];
                c.Verbose = Convert.ToInt32(ConfigurationManager.AppSettings["verbose"]);














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
                System.Console.WriteLine(response);

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
                using (SqlConnection con = new SqlConnection(dbTempString))
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
            catch (SqlException sql)
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






            System.Console.WriteLine("Refresh Complete");
            LockServer = false;
            //   Console.WriteLine("Lock server = false");

            //   Console.WriteLine("Refresh Data comlete");
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


    }
}


