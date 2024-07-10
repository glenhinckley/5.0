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

using DCSGlobal.Eligibility.Control;
using DCSGlobal.BusinessRules.CoreLibraryII;
using DCSGlobal.BusinessRules.CoreLibrary;
using DCSGlobal.BusinessRules.GeneratePDF;
 
 

using DCSGlobal.BusinessRules.Logging;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;

namespace DCSGlobal.Console.Eligibility.HTMLtoPDF
{
    class Program
    {

        private static int numThreads = 4;
        private static int iEDIKey = 0;
        private static int iEDICKey = 0;
        private static config c = new config();
        private static string AppName = "DCSGlobal.Console.Eligbility.HTMLtoPDF";
        private static Int32 MaxCount;
        private static Int32 TotalThreadCount = 0;
        private static logExecption log = new logExecption();
        private static ApplicationLog al = new ApplicationLog();
        private static LogToEventLog el = new LogToEventLog();

        private static StringStuff ss = new StringStuff();

        

        private static System.Timers.Timer _Recycletimer; // From System.Timers
        private static bool _Recycle = false;


        private static System.Timers.Timer _timer; // From System.Timers
        private static string dbTempString = string.Empty;
        private static ManualResetEvent _quitEvent = new ManualResetEvent(false);



        private static CustomThreadPool MyPool;
        private static Dictionary<int, string> _dID = new Dictionary<int, string>();
        private static int JobCount = 0;
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
            //    var allowEveryoneRule = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow);
            //   var securitySettings = new MutexSecurity();
            //   securitySettings.AddAccessRule(allowEveryoneRule);

            GetAppConfig();
            //    IsSingleInstance();





            // stops race condition needs to be about 3-5 secs
            SingleGlobalInstance(3000);

            if (!hasHandle)
            {
                Environment.Exit(0);

            }








            AppName = c.ConsoleName;



            DBUtility dbu = new DBUtility();


            if (c.ForceClearTextPasswd == 1)
            {
                dbTempString = c.ConnectionString;
            }
            else
            {

                //dbTempString = dbu.getConnectionString(c.ConnectionString);


                //dbTempString = c.ConnectionString;

                //if (dbTempString == "err")
                //{
                //    if (dbu.ValidateConString(c.ConnectionString))
                //    {
                //        dbTempString = c.ConnectionString;
                //        //el.WriteEventWarning(AppName + "  running with clear text constring", 1);
                //    }
                //    else
                //    {
                //        el.WriteEventError(AppName + "  decode constring failed", 1);

                //        if (mutex != null)
                //        {
                //            if (hasHandle)
                //                mutex.ReleaseMutex();
                //            mutex.Dispose();
                //        }
                //        Environment.Exit(0);
                //    }
                //}
                dbTempString = dbu.getConnectionString(c.ConnectionString);

              
                if (dbTempString.Contains("Base-64"))
                {

                    System.Console.WriteLine("Failed to decrypt constring EXIT");
                    Environment.Exit(0);

                }

                else
                {

                    c.ConnectionString = dbTempString;

                }

            }
            using (SchedulerLog _SL = new SchedulerLog())
            {
                _SL.ConnectionString = c.ConnectionString;
                JobID = _SL.AddLogEntry(AppName, "Eligibility PDF");
            }


            if (c.verbose == 1)
            {
                System.Console.WriteLine("Task started  " + AppName);


            }

            log.ConnectionString = c.ConnectionString;
            al.ConnectionString = c.ConnectionString;

            if (c.verbose == 1)
            {
                log.ExceptionDetails(AppName, " log.connection");


            }

            // recycle timer

            //_Recycletimer = new System.Timers.Timer(c.dbPollTimeMilliSeconeds);
            //_Recycletimer.Elapsed += new ElapsedEventHandler(_RecycleTimerElaspesed);
            //if (c.ForceRecycle == 1)
            //{
            //    _Recycletimer.Enabled = true; // Enable it
            //}

            //_Recycletimer.Enabled = true; // Enable it




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
                _timer.Enabled = true; // Enable it



                GO();

                _quitEvent.WaitOne();

            }


            else
            {
                GoSingle();

            }

            if (c.verbose == 1)
            {
                System.Console.WriteLine("All tasks have terminated on there own.");
            }

            using (SchedulerLog _AL = new SchedulerLog())
            {
                _AL.ConnectionString = dbTempString;
                _AL.UpdateLogEntry(AppName, "This is the end", JobID);
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
                System.Console.WriteLine("\n\rTimer Fired  current job count  " + Convert.ToString(JobCount));
            }


            GO();
            //   GC.Collect();
        }



        static void _RecycleTimerElaspesed(object sender, ElapsedEventArgs e)
        {
            if (c.verbose == 1)
            {
                System.Console.WriteLine("\n\rTimer Fired  current job count  " + Convert.ToString(JobCount));
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

            string rtnFile = string.Empty;
            string Scontext = string.Empty;
            string PatHospCode = string.Empty;
            string sFullFilePath = string.Empty;



       
            try
            {
                using (SqlConnection con = new SqlConnection(dbTempString))
                {
                    con.Open();
                    //
                    // Create new SqlCommand object.
                    //
                    using (SqlCommand cmd = new SqlCommand(c.cfgUspGetData, con))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader idr = cmd.ExecuteReader())
                        {

                            if (idr.HasRows)
                            {
                                while (idr.Read())
                                {
                                    RowData = string.Empty;
                                    OutFile = string.Empty;

                                    try
                                    {
                                        if (idr["BATCH_ID"] != System.DBNull.Value)
                                        {
                                            BatchId = Convert.ToDouble((long)idr["BATCH_ID"]);
                                        }
                                        else
                                        {
                                            log.ExceptionDetails(AppName, ("loop number " + Convert.ToString(x) + " BatchID null"));
                                        }
                                    }
                                    catch
                                    {
                                        log.ExceptionDetails(AppName, " BatchID not in record set");
                                    }


                                    try
                                    {
                                        if (idr["rpt_filename"] != System.DBNull.Value)
                                        {
                                            OutFile = (string)idr["rpt_filename"];
                                        }
                                        else
                                        {
                                            log.ExceptionDetails(AppName, ("BatchID " + Convert.ToString(BatchId) + " rpt_filename is null"));
                                        }
                                    }
                                    catch
                                    {
                                        log.ExceptionDetails(AppName, ("loop number " + Convert.ToString(x) + " rpt_filename not in record set"));
                                    }


                                    RowData = Convert.ToString(BatchId) + "|" + OutFile;


                                 //   System.Console.WriteLine("Added to did " + Convert.ToString(BatchId) + OutFile);
                                    _dID.Add(x, RowData);
                                    x++;

                                }
                            }
                        }
                        // 
                        r = 0;
                    }
                    con.Close();
                }




                foreach (KeyValuePair<int, string> pair in _dID)
                {



                    /*************************************************************************************************************************
                     * do what ever start here
                     * ***********************************************************************************************************************/

                    tRowData = string.Empty;
                    tRowData = pair.Value;

                    double tBatchID = 0;
                    string tOutFile = string.Empty;


                    tBatchID = Convert.ToDouble(ss.ParseDemlimtedStringEDI(tRowData, "|", 1));
                    tOutFile = ss.ParseDemlimtedStringEDI(tRowData, "|", 2);




                    try
                    {
                        using (RenderEligibility renderEligibility = new RenderEligibility())
                        {

                            renderEligibility.ConnectionString = dbTempString;
                            renderEligibility.Use800600(c.use800);
                            renderEligibility.DisplayOSD(c.showOSD);
                            renderEligibility.dbCommandTimeOut = c.dbCommandTimeOut;
                            renderEligibility.isCentered = c.isCentered;
                            renderEligibility.ShowDocumentID(c.showOCR);
                            renderEligibility.BatchID = tBatchID;
                            renderEligibility.UserID = c.UserID;
                            renderEligibility.ShowSummary = c.showSummary;
                            
                            html = renderEligibility.RenderContents();


                            string mystring = string.Empty;

                             

                            using (PDFConverter pdfConverter = new PDFConverter())
                            {

                                pdfConverter.BottomMargin = c.BottomMargin;
                                pdfConverter.TopMargin = c.TopMargin;
                                pdfConverter.LeftMargin = c.LeftMargin;
                                pdfConverter.RightMargin = c.RightMargin;
                    
                                pdfConverter.toFileFromString(html, (c.PDFSaveLocation + tOutFile));
                            }

                            //Added by Mohan -12/08/2016  to integrate Header details in 1st page in PDF.
                            
                            
                            sFullFilePath=c.PDFSaveLocation + tOutFile;

                            if (c.DisplayHeader == 1)
                            {
                                using (DCSPDFGen o = new DCSPDFGen())
                                {
                                    o.ConnectionString = dbTempString;
                                    o.PdfDBProc = c.PdfDBProc;
                                    rtnFile = o.createPDFFile(c.doc_id, Convert.ToString(tBatchID),sFullFilePath,c.UserID);
                                }
                            }









                            using (SqlConnection con1 = new SqlConnection(dbTempString))
                            {
                                using (SqlCommand cmd = new SqlCommand(c.cfgUspUpdateEligAdt, con1))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@id", tBatchID);
                                    con1.Open();
                                    cmd.ExecuteNonQuery();


                                }
                            }
                        }
                    }

                    catch (Exception ex)
                    {

                        //    Console.Write("Build PDF Fail " + ex.Message + "\n\r");

                        log.ExceptionDetails(AppName, ex);
                        log.ExceptionDetails(AppName, (AppName + Convert.ToString(tBatchID)) + "build pdf failed");

                    }


                    /*************************************************************************************************************************
                    * do what ever end here
                    * ***********************************************************************************************************************/

                }
                _dID.Clear();

            }
            catch (SqlException sx)
            {
                log.ExceptionDetails(AppName + " GO " + " SQL Exception - Failed DCSGlobal.Console.Eligibility.HTMLtoPDF ", sx);
            }
            catch (Exception ex)
            {

                log.ExceptionDetails(AppName + " GO " + " Exception - Failed DCSGlobal.Console.Eligibility.HTMLtoPDF ", ex);
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
            _timer.Enabled = false;




            try
            {



                using (SqlConnection con = new SqlConnection(dbTempString))
                {
                    con.Open();
                    //
                    // Create new SqlCommand object.
                    //
                    using (SqlCommand cmd = new SqlCommand(c.cfgUspGetData, con))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader idr = cmd.ExecuteReader())
                        {
                            //  count = idr.VisibleFieldCount;




                            if (idr.HasRows)
                            {
                                while (idr.Read())
                                {
                                    RowData = string.Empty;
                                    OutFile = string.Empty;

                                    try
                                    {
                                        if (idr["BATCH_ID"] != System.DBNull.Value)
                                        {
                                            BatchId = Convert.ToDouble((long)idr["BATCH_ID"]);
                                        }
                                        else
                                        {
                                            log.ExceptionDetails(AppName, ("loop number " + Convert.ToString(x) + " BatchID null"));
                                        }
                                    }
                                    catch
                                    {
                                        log.ExceptionDetails(AppName, " BatchID not in record set");
                                    }


                                    try
                                    {
                                        if (idr["rpt_filename"] != System.DBNull.Value)
                                        {
                                            OutFile = (string)idr["rpt_filename"];
                                        }
                                        else
                                        {
                                            log.ExceptionDetails(AppName, ("BatchID " + Convert.ToString(BatchId) + " rpt_filename is null"));
                                        }
                                    }
                                    catch
                                    {
                                        log.ExceptionDetails(AppName, ("loop number " + Convert.ToString(x) + " rpt_filename not in record set"));
                                    }


                                    RowData = Convert.ToString(BatchId) + "|" + OutFile;


                                    System.Console.WriteLine("Added to did " + Convert.ToString(BatchId) + OutFile);
                                    _dID.Add(x, RowData);
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


                foreach (KeyValuePair<int, string> pair in _dID)
                {

                    MyPool.QueueUserTask(() =>
                    {



                        {

                            /*************************************************************************************************************************
                             * do what ever start here
                             * ***********************************************************************************************************************/

                            tRowData = string.Empty;
                            tRowData = pair.Value;

                            double tBatchID = 0;
                            string tOutFile = string.Empty;


                            tBatchID = Convert.ToDouble(ss.ParseDemlimtedStringEDI(tRowData, "|", 1));
                            tOutFile = ss.ParseDemlimtedStringEDI(tRowData, "|", 2);

                            System.Console.WriteLine("Added to pool " + Convert.ToDouble(ss.ParseDemlimtedStringEDI(tRowData, "|", 1)) + ss.ParseDemlimtedStringEDI(tRowData, "|", 2));



                            try
                            {
                                using (RenderEligibility renderEligibility = new RenderEligibility())
                                {

                                    renderEligibility.ConnectionString = dbTempString;
                                    renderEligibility.Use800600(c.use800);
                                    renderEligibility.DisplayOSD(c.showOSD);
                                    renderEligibility.dbCommandTimeOut = c.dbCommandTimeOut;
                                    renderEligibility.isCentered = c.isCentered;
                                    renderEligibility.ShowDocumentID(c.showOCR);
                                    renderEligibility.BatchID = tBatchID;
                                    renderEligibility.UserID = c.UserID;
                                    renderEligibility.ShowSummary = c.showSummary;

                                    html = renderEligibility.RenderContents();
                                    string mystring = string.Empty;


                                    mystring = "";/// what ever kishore give me

                                    //get the data from kisore and append the html to it


                                    ///  html = mystring + html;

                                    using (PDFConverter pdfConverter = new PDFConverter())
                                    {

                                        pdfConverter.BottomMargin = c.BottomMargin;
                                        pdfConverter.TopMargin = c.TopMargin;
                                        pdfConverter.LeftMargin = c.LeftMargin;
                                        pdfConverter.RightMargin = c.RightMargin;
                                        //   pdfConverter.toFileFromString(Program.h, string.Concat(Program.outPath, OutFile));
                                        System.Console.WriteLine(c.PDFSaveLocation + tOutFile);
                                        pdfConverter.toFileFromString(html, (c.PDFSaveLocation + tOutFile));
                                    }



                                    using (SqlConnection con1 = new SqlConnection(dbTempString))
                                    {
                                        using (SqlCommand cmd = new SqlCommand(c.cfgUspUpdateEligAdt, con1))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.AddWithValue("@id", tBatchID);
                                            con1.Open();
                                            cmd.ExecuteNonQuery();
                                            //  Console.Write("Batch Id : " + _BatchId + " Marked Complete" + "\n\r");

                                        }
                                    }
                                }
                            }

                            catch (Exception ex)
                            {

                                //    Console.Write("Build PDF Fail " + ex.Message + "\n\r");

                                log.ExceptionDetails(AppName, ex);
                                log.ExceptionDetails(AppName, (AppName + Convert.ToString(tBatchID)) + "build pdf failed");

                            }


                            /*************************************************************************************************************************
                            * do what ever end here
                            * ***********************************************************************************************************************/

                        }




                    },
                          (ts) =>
                          {
                              if (c.verbose == 1)
                              {

                                  log.ExceptionDetails(AppName, "Thread for + " + Convert.ToString(pair.Value) + " completed");
                              }
                              JobCount--;
                              //showMessage(ts.Success.ToString());
                          });


                }

                _dID.Clear();


            }
            catch (SqlException sx)
            {

                log.ExceptionDetails(AppName + " GO " + " SQL Exception - Failed DCSGlobal.Console.Eligibility.HTMLtoPDF ", sx);
                _quitEvent.Set();
            }
            catch (Exception ex)
            {

                log.ExceptionDetails(AppName + " GO " + " Exception - Failed DCSGlobal.Console.Eligibility.HTMLtoPDF ", ex);

                _quitEvent.Set();

            }


            if (_Recycle)
            {

                _quitEvent.Set();
            }
            else
            {
                _timer.Enabled = true;
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



                c.cfgUspGetData = ConfigurationManager.AppSettings["cfgUspGetData"];
                c.cfgUspUpdateEligAdt = ConfigurationManager.AppSettings["cfgUspUpdateEligAdt"];

                c.PDFSaveLocation = ConfigurationManager.AppSettings["PDFSaveLocation"];
                c.UserID = ConfigurationManager.AppSettings["UserID"];

                c.LeftMargin = Convert.ToInt32(ConfigurationManager.AppSettings["LeftMargin"]);
                c.RightMargin = Convert.ToInt32(ConfigurationManager.AppSettings["RightMargin"]);
                c.TopMargin = Convert.ToInt32(ConfigurationManager.AppSettings["TopMargin"]);
                c.BottomMargin = Convert.ToInt32(ConfigurationManager.AppSettings["BottomMargin"]);



                c.isCentered = Convert.ToBoolean(ConfigurationManager.AppSettings["isCentered"]);
                c.isPrint = Convert.ToBoolean(ConfigurationManager.AppSettings["isPrint"]);
                c.showOSD = Convert.ToBoolean(ConfigurationManager.AppSettings["showOSD"]);
                c.showOCR = Convert.ToBoolean(ConfigurationManager.AppSettings["showOCR"]);
                c.use800 = Convert.ToBoolean(ConfigurationManager.AppSettings["use800"]);
                c.showSummary = Convert.ToBoolean(ConfigurationManager.AppSettings["showSummary"]);


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

                c.WaitForEnterToExit = ConfigurationManager.AppSettings["WaitForEnterToExit"];

                c.verbose = Convert.ToInt32(ConfigurationManager.AppSettings["verbose"]);

                c.ForceRecycle = Convert.ToInt32(ConfigurationManager.AppSettings["ForceRecycle"]);
                c.DisplayHeader = Convert.ToInt32(ConfigurationManager.AppSettings["DisplayHeader"]);

                c.PdfDBProc =  ConfigurationManager.AppSettings["PdfDBProc"];
                c.doc_id = ConfigurationManager.AppSettings["doc_id"];
               


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
