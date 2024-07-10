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

namespace DCSGlobal.Console.GenerateFile
{
    class Program
    {


        private static int iEDIKey = 0;

        private static Config c = new Config();
        private static string _AppName = "DCSGlobal.Console.GenerateFile";


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
        //bool _disposed;
        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}


        //protected virtual void Dispose(bool disposing)
        //{
        //    if (_disposed)
        //        return;

        //    if (disposing)
        //    {

        //        log.Dispose();

        //        // free other managed objects that implement
        //        // IDisposable only
        //    }

        //    c = null;
        //    // release any unmanaged objects
        //    // set the object references to null

        //    _disposed = true;
        //}

        static void Main_test(string[] args)
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
                JobID = _SL.AddLogEntry(_AppName, "GenerateFile");
            }



            log.ConnectionString = dbTempString;
            al.ConnectionString = dbTempString;



            // recycle timer

            _Recycletimer = new System.Timers.Timer(c.RecycleWaitTime);
            _Recycletimer.Elapsed += new ElapsedEventHandler(_RecycleTimerElaspesed);
            _Recycletimer.Enabled = true; // Enable it


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


            if (c.WaitForEnterToExit == "1")
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

            int r = 0;
            int i = 0;
            int _id = 0;
            int x = 0;
            string tRowData = string.Empty;
            string tContent = string.Empty;

            string tFileName = string.Empty;


            using (SqlConnection con = new SqlConnection(dbTempString))
            {
                con.Open();
                //
                // Create new SqlCommand object.
                //
                using (SqlCommand cmd = new SqlCommand(c.SP_NAME_FETCH, con))
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
                                int rSSP = 0;
                                bool pass = true;
                                _id = Convert.ToInt32(idr["id"]);
                                EDI = Convert.ToString(idr["id"]);

                                if (idr["EDI"] != System.DBNull.Value)
                                {
                                    EDI = EDI + "|" + Convert.ToString((string)idr["EDI"]);
                                }
                                else
                                {
                                    pass = false;
                                    log.ExceptionDetails(c.ConsoleName, Convert.ToString((int)idr["EDI"]) + " EDI  = null");
                                }
                                if (idr["FILE_NAME"] != System.DBNull.Value)
                                {
                                    EDI = EDI + "|" + Convert.ToString((string)idr["FILE_NAME"]);
                                }
                                else
                                {
                                    pass = false;
                                    log.ExceptionDetails(c.ConsoleName, Convert.ToString((int)idr["FILE_NAME"]) + " FILE_NAME  = null");
                                }
                                if (pass)
                                {
                                    using (DCSGlobal.EDI.GenerateFile sf = new DCSGlobal.EDI.GenerateFile())
                                    {
                                        sf.ConnectionString = dbTempString;
                                        sf.SP_SET_PROCESS_STATUS = c.SP_NAME_UPDATE;
                                        rSSP = sf.SetFlag("I", _id);
                                    }
                                    if (rSSP == 0)
                                    {
                                        _dID.Add(x, EDI);
                                        x++;
                                    }
                                    else
                                    {
                                        log.ExceptionDetails(_AppName, "Set File Generate Status flag for + " + Convert.ToString(_id) + " Failed");
                                    }
                                }
                                else
                                {
                                    using (DCSGlobal.EDI.GenerateFile sf = new DCSGlobal.EDI.GenerateFile())
                                    {
                                        sf.ConnectionString = dbTempString;
                                        sf.SP_SET_PROCESS_STATUS = c.SP_NAME_UPDATE;
                                        rSSP = sf.SetFlag("F", _id);
                                    }
                                    log.ExceptionDetails(c.ConsoleName, Convert.ToString((int)idr["id"]) + " File Name or EDI is null,  EDI string is not correct. Updated Process Flag to F.");
                                }
                            }
                        }  // end wilewhile
                    }
                    r = 0;
                }
                con.Close();
            }

            if (c.verbose == 1)
            {
                if (c.ThreadsON == "1")
                {
                    System.Console.Write("pool count " + Convert.ToString(MyPool.PoolCount));
                }
            }

            foreach (KeyValuePair<int, string> pair in _dID)
            {
                /*************************************************************************************************************************
                    * do what ever start here
                    * ***********************************************************************************************************************/
                try
                {

                    tRowData = string.Empty;
                    tRowData = pair.Value;

                    long tBatchID = 0;
                    tContent = string.Empty;


                    tBatchID = Convert.ToInt64(ss.ParseDemlimtedStringEDI(tRowData, "|", 1));
                    tContent = ss.ParseDemlimtedStringEDI(tRowData, "|", 2);
                    tFileName = ss.ParseDemlimtedStringEDI(tRowData, "|", 3);


                    if (c.LoadFromDB == 1)
                    {
                        using (DCSGlobal.EDI.GenerateFile gf = new DCSGlobal.EDI.GenerateFile())
                        {
                            gf.ConnectionString = dbTempString;
                            gf.ID = tBatchID;
                            gf.Content = tContent;
                            gf.SP_NAME_UPDATE = c.SP_NAME_UPDATE;
                            gf.ARCHIVE_FOLDER = c.ARCHIVE_FOLDER;
                            // gf.Folder = c.EDIFileSaveLocation;
                            gf.FileName = c.DEST_FOLDER + tFileName;
                            gf.Verbose = Convert.ToBoolean(c.verbose);
                            gf.Go();
                        }
                    }
                }

                catch (Exception ex)
                {
                    log.ExceptionDetails(_AppName, ex);
                }
                /*************************************************************************************************************************
                * do what ever end here
                * *********************************************************************************************************************/

            }
            _dID.Clear();
        }

        static void GetAppConfig()
        {
            try
            {

                c.ConnectionString = ConfigurationManager.AppSettings["ConnStr"];
                c.ConsoleName = ConfigurationManager.AppSettings["ConsoleName"];

                c.MinThreads = ConfigurationManager.AppSettings["MinThreads"];
                c.MaxThreads = ConfigurationManager.AppSettings["MaxThreads"];
                c.ThreadsON = ConfigurationManager.AppSettings["ThreadsON"];
                c.MinWait = ConfigurationManager.AppSettings["MinWait"];
                c.MaxWait = ConfigurationManager.AppSettings["MaxWait"];
                c.dbPollTimeMilliSeconeds = Convert.ToInt32(ConfigurationManager.AppSettings["dbPollTimeMiliSeconeds"]);
                c.verbose = Convert.ToInt32(ConfigurationManager.AppSettings["verbose"]);
                c.SchedulingInterval = ConfigurationManager.AppSettings["SchedulingInterval"];
                c.CleanupInterval = ConfigurationManager.AppSettings["CleanupInterval"];
                c.LoadFromDB = Convert.ToInt32(ConfigurationManager.AppSettings["LoadFromDB"]);
                c.SP_NAME_UPDATE = ConfigurationManager.AppSettings["SP_NAME_UPDATE"];
                c.SP_NAME_FETCH = ConfigurationManager.AppSettings["SP_NAME_FETCH"];
                c.verbose = Convert.ToInt32(ConfigurationManager.AppSettings["verbose"]);
                c.DEST_FOLDER = ConfigurationManager.AppSettings["DEST_FOLDER"];
                c.WaitForEnterToExit = ConfigurationManager.AppSettings["WaitForEnterToExit"];
                c.dbPollTimeMilliSeconeds = Convert.ToInt32(ConfigurationManager.AppSettings["dbPollTimeMilliSeconeds"]);
                c.PoolPollTime = Convert.ToInt32(ConfigurationManager.AppSettings["PoolPollTime"]);
                c.RecycleWaitTime = Convert.ToInt32(ConfigurationManager.AppSettings["RecycleWaitTime"]);
                c.ForceClearTextPasswd = Convert.ToInt32(ConfigurationManager.AppSettings["ForceClearTextPasswd"]);
                c.ARCHIVE_FOLDER = ConfigurationManager.AppSettings["ARCHIVE_FOLDER"];  
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

            int r = 0;
            int i = 0;
            int _id = 0;
            int x = 0;
            string tRowData = string.Empty;
            string tContent = string.Empty;
            //_timer.Enabled = false;
            string tFileName = string.Empty;

            try
            {
                using (SqlConnection con = new SqlConnection(dbTempString))
                {
                    con.Open();
                    //
                    // Create new SqlCommand object.
                    //
                    using (SqlCommand cmd = new SqlCommand(c.SP_NAME_FETCH, con))
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
                                    string File_Name = "";
                                    int rSSP = 0;
                                    bool pass = true;
                                    _id = Convert.ToInt32(idr["id"]);
                                    EDI = Convert.ToString(idr["id"]);

                                    if (idr["EDI"] != System.DBNull.Value)
                                    {
                                        EDI = EDI + "|" + Convert.ToString((string)idr["EDI"]);
                                    }
                                    else
                                    {
                                        pass = false;
                                        log.ExceptionDetails(c.ConsoleName, Convert.ToString((int)idr["EDI"]) + " EDI  = null");
                                    }
                                    if (idr["FILE_NAME"] != System.DBNull.Value)
                                    {
                                        EDI = EDI + "|" + Convert.ToString((string)idr["FILE_NAME"]);
                                    }
                                    else
                                    {
                                        pass = false;
                                        log.ExceptionDetails(c.ConsoleName, Convert.ToString((int)idr["FILE_NAME"]) + " FILE_NAME  = null");
                                    }
                                    if (pass)
                                    {
                                        using (DCSGlobal.EDI.GenerateFile sf = new DCSGlobal.EDI.GenerateFile())
                                        {
                                            sf.ConnectionString = dbTempString;
                                            sf.SP_SET_PROCESS_STATUS = c.SP_NAME_UPDATE;
                                            rSSP = sf.SetFlag("I", _id);
                                        }
                                        if (rSSP == 0)
                                        {
                                            _dID.Add(x, EDI);
                                            x++;
                                        }
                                        else
                                        {
                                            log.ExceptionDetails(_AppName, "Set File Generate Status flag for + " + Convert.ToString(_id) + " Failed");
                                        }
                                    }
                                    else
                                    {
                                        log.ExceptionDetails(c.ConsoleName, Convert.ToString((int)idr["id"]) + " File Name or EDI is null,  EDI string is not correct.");
                                    }
                                }
                            }  // end wilewhile
                        }
                        con.Close();
                        r = 0;
                    }
                   
                }
                if (c.verbose == 1)
                {
                    if (c.ThreadsON == "1")
                    {
                        System.Console.Write("pool count " + Convert.ToString(MyPool.PoolCount));
                    }

                }

                foreach (KeyValuePair<int, string> pair in _dID)
                {
                    MyPool.QueueUserTask(() =>
                    {
                        /*************************************************************************************************************************
                         * do what ever start here
                         * ***********************************************************************************************************************/
                        try
                        {

                            tRowData = string.Empty;
                            tRowData = pair.Value;

                            long tBatchID = 0;
                            tContent = string.Empty;


                            tBatchID = Convert.ToInt64(ss.ParseDemlimtedStringEDI(tRowData, "|", 1));
                            tContent = ss.ParseDemlimtedStringEDI(tRowData, "|", 2);
                            tFileName = ss.ParseDemlimtedStringEDI(tRowData, "|", 3);


                            if (c.LoadFromDB == 1)
                            {
                                using (DCSGlobal.EDI.GenerateFile gf = new DCSGlobal.EDI.GenerateFile())
                                {
                                    gf.ConnectionString = dbTempString;
                                    gf.ID = tBatchID;
                                    gf.Content = tContent;
                                    gf.SP_NAME_UPDATE = c.SP_NAME_UPDATE;
                                    gf.ARCHIVE_FOLDER = c.ARCHIVE_FOLDER;
                                    // gf.Folder = c.EDIFileSaveLocation;
                                    gf.FileName = c.DEST_FOLDER + tFileName;
                                    gf.Verbose = Convert.ToBoolean(c.verbose);
                                    gf.Go();
                                }
                            }
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
            catch (SqlException sx)
            {
                log.ExceptionDetails(_AppName + " Main" + " SQL Exception - Failed DCSGlobal.Console.Childerns  ", sx);
                _quitEvent.Set();
            }
            catch (Exception ex)
            {
                log.ExceptionDetails(_AppName + " Main" + " Exception - Failed DCSGlobal.Console.Childerns  ", ex);
                _quitEvent.Set();
            }
        }
    }
}


