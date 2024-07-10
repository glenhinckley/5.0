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

using ProviderDB;






using System.Reflection;

using System.Reflection.Emit;



using System.Runtime.InteropServices;   //GuidAttribute
using System.Reflection;                //Assembly
using System.Security.AccessControl;    //MutexAccessRule
using System.Security.Principal;
using System.Diagnostics;        //SecurityIdentifier


namespace Reports
{
    internal class Program
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

        private static Dictionary<int, string> _dID = new Dictionary<int, string>();
        private static List<Provider> ProviderList = new List<Provider>();
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



        static void Main(string[] args)
        {

            GetAppConfig();



            /// *********************************************************************************************************************
            /// Get all the partics groups and slam them in the new table
            /// *********************************************************************************************************************
            getPracticeGroups();
            getProviders();
            AddProviders();


            Console.WriteLine("Done press any key to exit");
            Console.ReadLine(); 



        }


        static void AddProviders()
        {

            string ProviderInsert = string.Empty;
            foreach (Provider p in ProviderList)
            {




                try
                {
                    using (SqlConnection con2 = new SqlConnection(c.ConnectionString))
                    {
                        con2.Open();
                        string PracticeGroupInsert = string.Empty;

                        ProviderInsert = "INSERT INTO ProviderGroup(ProviderID, ProviderGroupID, DomainID,LegacyID,LastName,FirstName,NPI)";
                        ProviderInsert = ProviderInsert + " values( ";
                        ProviderInsert = ProviderInsert + " values (1, " + pair.Key + ", '" + ss.dbEncode(pair.Value) + "')";
                        ProviderInsert = ProviderInsert + " )";




                        //  ProviderID ProviderGroupID     DomainID LegacyID     LastName FirstName     NPI



                        using (SqlCommand cmd2 = new SqlCommand(PracticeGroup, con2))
                        {

                            cmd2.CommandType = CommandType.Text;
                            //    cmd2.ExecuteNonQuery();
                            x++;
                            Console.WriteLine("inserting " + Convert.ToString(x) + "  of " + Convert.ToString(_dID.Count));
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
                finally
                {

                }








            }


        }


        static void getPracticeGroups()
        {



            using (SqlConnection con = new SqlConnection(c.ConnectionString))
            {
                con.Open();
                //
                // Create new SqlCommand object.
                //

                string PracticeGroup = string.Empty;

                PracticeGroup = "select fldgroupID, fldGroupName from PsyquelProd.dbo.tblProviderGroup";

                using (SqlCommand cmd = new SqlCommand(PracticeGroup, con))
                {

                    cmd.CommandType = CommandType.Text;

                    using (SqlDataReader idr = cmd.ExecuteReader())
                    {

                        if (idr.HasRows)
                        {



                            // Call Read before accessing data. 
                            while (idr.Read())
                            {


                                string PraticeGroupName = string.Empty;
                                int PracticeGroupID = 0;

                                bool pass = true;


                                if (idr["fldgroupID"] != System.DBNull.Value)
                                {
                                    PracticeGroupID = Convert.ToInt32((int)idr["fldgroupID"]);
                                }
                                else
                                {
                                    //  log.ExceptionDetails(_AppName, Convert.ToString((int)idr["id"]) + " vendor_input_type  = null");
                                    pass = false;
                                }

                                if (idr["fldGroupName"] != System.DBNull.Value)
                                {
                                    PraticeGroupName = Convert.ToString((string)idr["fldGroupName"]);
                                }
                                else
                                {
                                    //log.ExceptionDetails(_AppName, Convert.ToString((int)idr["id"]) + " pat_hosp_code  = null");
                                    pass = false;
                                }



                                // if pass = ture add them to the diconary id not fail it and log it.

                                if (pass)
                                {
                                    _dID.Add(PracticeGroupID, PraticeGroupName);

                                }
                                else
                                {
                                    log.ExceptionDetails(_AppName, Convert.ToString((int)idr["id"]) + " Request is null, either Screen scrape or Request string is not correct.");
                                }
                            }
                        }  // end wilewhile
                        con.Close();
                    }



                    int x = 0;
                    foreach (KeyValuePair<int, string> pair in _dID)
                    {
                        /*************************************************************************************************************************
                         * do what ever start here
                         * ***********************************************************************************************************************/
                        try
                        {
                            using (SqlConnection con2 = new SqlConnection(c.ConnectionString))
                            {
                                con2.Open();
                                string PracticeGroupInsert = string.Empty;

                                PracticeGroup = "INSERT INTO ProviderGroup(DomainID, LegacyID, GroupName)";
                                PracticeGroup = PracticeGroup + " values (1, " + pair.Key + ", '" + ss.dbEncode(pair.Value) + "')";

                                using (SqlCommand cmd2 = new SqlCommand(PracticeGroup, con2))
                                {

                                    cmd2.CommandType = CommandType.Text;
                                    //    cmd2.ExecuteNonQuery();
                                    x++;
                                    Console.WriteLine("inserting " + Convert.ToString(x) + "  of " + Convert.ToString(_dID.Count));
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
                        finally
                        {

                        }
                    }

                    Console.WriteLine(" all done ");
                };
            }


        }


        static void getProviders()
        {

            string ProviderInsert = string.Empty;
            string ProviderFetch = string.Empty;
            string ProviderDelete = string.Empty;
            string ProviderCreate = string.Empty;


            Provider _provider = new Provider();
            _provider.Clear();
            string PracticeGroup = string.Empty;
            int PracticeGroupID = 0;
            int x = 0;


            foreach (KeyValuePair<int, string> pair in _dID)
            {
                x++;
                PracticeGroupID = pair.Key;
                Console.WriteLine("inserting " + Convert.ToString(x) + "-" + Convert.ToString(PracticeGroupID) + "  of " + Convert.ToString(_dID.Count));
                using (SqlConnection con = new SqlConnection(c.ConnectionString))
                {
                    con.Open();
                    //
                    // Create new SqlCommand object.
                    //
                    ProviderFetch = string.Empty;

                    /*************************************************************************************************************************
                     * do what ever start here
                     * ***********************************************************************************************************************/
                    try
                    {


                        ProviderFetch = ProviderFetch + "SELECT p.[fldProviderID], u.fldLastName, u.fldFirstName ,p.fldProviderNPI  FROM ";
                        ProviderFetch = ProviderFetch + "[PsyquelProd].[dbo].[tblProviderGroup] PG ";
                        ProviderFetch = ProviderFetch + "inner join  [PsyquelProd].[dbo].[tblProvider] P  on pg.fldGroupID =  p.fldGroupID ";
                        ProviderFetch = ProviderFetch + "inner join [PsyquelProd].[dbo].[tblUser] U  on p.fldProviderID = u.fldUserID where p.fldGroupID =  " + PracticeGroupID + " ";
                        ProviderFetch = ProviderFetch + "group by p.[fldProviderID],pg.fldGroupName, u.fldLastName ,fldFirstName ,p.fldProviderNPI ";
                        ProviderFetch = ProviderFetch + "order by 1 ";


                        using (SqlCommand cmd = new SqlCommand(ProviderFetch, con))
                        {


                            cmd.CommandType = CommandType.Text;
                            using (SqlDataReader idr = cmd.ExecuteReader())
                            {
                                if (idr.HasRows)
                                {
                                    // Call Read before accessing data. 
                                    while (idr.Read())
                                    {
                                        _provider.Clear();
                                        _provider.ProviderGroupID = PracticeGroupID;
                                        _provider.ProviderDomainID = 1;

                                        if (idr["fldProviderID"] != System.DBNull.Value)
                                        {
                                            _provider.ProviderLegacyID = Convert.ToInt32((int)idr["fldProviderID"]);
                                        }
                                        else
                                        {
                                            //  log.ExceptionDetails(_AppName, Convert.ToString((int)idr["id"]) + " vendor_input_type  = null");
                                            //pass = false;
                                        }




                                        if (idr["fldFirstName"] != System.DBNull.Value)
                                        {
                                            _provider.FirstName = Convert.ToString((string)idr["fldFirstName"]);
                                        }
                                        else
                                        {
                                            //  log.ExceptionDetails(_AppName, Convert.ToString((int)idr["id"]) + " vendor_input_type  = null");
                                            //pass = false;
                                        }

                                        if (idr["fldLastName"] != System.DBNull.Value)
                                        {
                                            _provider.LastName = Convert.ToString((string)idr["fldLastName"]);
                                        }
                                        else
                                        {
                                            //  log.ExceptionDetails(_AppName, Convert.ToString((int)idr["id"]) + " vendor_input_type  = null");
                                            //pass = false;
                                        }


                                        if (idr["fldProviderNPI"] != System.DBNull.Value)
                                        {
                                            _provider.NPI = Convert.ToString((string)idr["fldProviderNPI"]);
                                        }
                                        else
                                        {
                                            //  log.ExceptionDetails(_AppName, Convert.ToString((int)idr["id"]) + " vendor_input_type  = null");
                                            //pass = false;
                                        }


                                        ProviderList.Add(_provider);

                                       Console.WriteLine("Added " + Convert.ToString(_provider.ProviderLegacyID) + " " + _provider.LastName + "," + _provider.FirstName);
                                    }
                                      Console.WriteLine("ProviderList " +Convert.ToString( ProviderList.Count));    





                                }  // end wilewhile




                                // con.Close();
                            }




                        };
                    }
                    catch (Exception ex)
                    {



                    }



                }

            }


            Console.WriteLine("Total Providers added to List " + Convert.ToString(ProviderList.Count));






        }


        static void GetAppConfig()
        {
            try
            {


                // begin db dntries

                c.ConnectionString = ConfigurationManager.AppSettings["ConnStr"];
                c.CommandTimeOut = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeOut"]);
                c.ForceClearTextPasswd = Convert.ToInt32(ConfigurationManager.AppSettings["ForceClearTextPasswd"]);






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



                //    _TimeToWiatForThreadsToCompleteOnRecycle = Convert.ToInt32(ConfigurationManager.AppSettings["TimeToWiatForThreadsToCompleteOnRecycle"]);



            }

            catch
            { }

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


