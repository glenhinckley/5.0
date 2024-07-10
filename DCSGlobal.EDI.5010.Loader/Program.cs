using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;



using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using DCSGlobal.BusinessRules.CoreLibraryII;
using DCSGlobal.BusinessRules.CoreLibrary;
using DCSGlobal.BusinessRules.Logging;
using DCSGlobal.EDI;

using System.IO.Pipes;




using System.Threading;
using System.Runtime.InteropServices;   //GuidAttribute
using System.Reflection;                //Assembly
using System.Security.AccessControl;    //MutexAccessRule
using System.Security.Principal;        //SecurityIdentifier

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//


namespace DCSGlobal.EDI.ConsoleLoader
{




    class Program
    {


        private static logExecption log = new logExecption();
        private static AppSettingsReader app = new AppSettingsReader();

        private static StringStuff ss = new StringStuff();

        private static string _AppName = string.Empty;
        private static string _ProgramName = string.Empty;
        private static string _InstanceName = string.Empty;



        private static string _ConnectionString = string.Empty;
        private static int _Verbose = 0;
        private static string _SourcePath = string.Empty;
        private static string _LogFilePath = string.Empty;

        private static string _SMTPServer = string.Empty;
        private static string _FromMailAddress = string.Empty;
        private static string _ToMailAddress = string.Empty;
        private static string _WaitForEnterToExit = string.Empty;
        private static int _ParseTree = 0;
        private static string _ProcessYearMonth = string.Empty;
        private static string _OverrideProcessYearMonth = string.Empty;

        private static string _LOG_EDI = "N";

        private static DataTable dirs = new DataTable();
        private static int _ForceClearTextPasswd = 0;

        private static string dbTempString = string.Empty;
        private static int _SchedulerLogID = 0;


        private static int _270Load = 0;
        private static int _271Load = 0;
        private static int _276Load = 0;
        private static int _277Load = 0;
        private static int _278Load = 0;
        private static int _835Load = 0;
        private static int _837Load = 0;
        private static int _939Load = 0;
        private static int _HL7Load = 0;

        private static string _FilePath = "";
        private static string _FileName = "";
        private static string _ClientName = "";
        private static string _HOSP_CODE = "";


        private static int _ParseTree270 = 0;
        private static string _FileFilter270 = string.Empty;
        private static string _270Input = string.Empty;
        private static string _270Failed = string.Empty;
        private static string _270Success = string.Empty;
        private static string _270Duplicate = string.Empty;
        private static string _270_VAULT_ROOT_DIRECTORY = string.Empty;
        private static string _270_VAULT_CLIENT_DIRECTORY = string.Empty;
        private static string _270_VAULT_FILE_PATH = string.Empty;

        private static int _ParseTree271 = 0;
        private static string _FileFilter271 = string.Empty;
        private static string _271Input = string.Empty;
        private static string _271Failed = string.Empty;
        private static string _271Success = string.Empty;
        private static string _271Duplicate = string.Empty;
        private static string _271_VAULT_ROOT_DIRECTORY = string.Empty;
        private static string _271_VAULT_CLIENT_DIRECTORY = string.Empty;
        private static string _271_VAULT_FILE_PATH = string.Empty;

        private static int _ParseTree276 = 0;
        private static string _FileFilter276 = string.Empty;
        private static string _276Input = string.Empty;
        private static string _276Failed = string.Empty;
        private static string _276Success = string.Empty;
        private static string _276Duplicate = string.Empty;
        private static string _276_VAULT_ROOT_DIRECTORY = string.Empty;
        private static string _276_VAULT_CLIENT_DIRECTORY = string.Empty;
        private static string _276_VAULT_FILE_PATH = string.Empty;

        private static int _ParseTree277 = 0;
        private static string _FileFilter277 = string.Empty;
        private static string _277Input = string.Empty;
        private static string _277Failed = string.Empty;
        private static string _277Success = string.Empty;
        private static string _277Duplicate = string.Empty;
        private static string _277_VAULT_ROOT_DIRECTORY = string.Empty;
        private static string _277_VAULT_CLIENT_DIRECTORY = string.Empty;
        private static string _277_VAULT_FILE_PATH = string.Empty;

        private static int _ParseTree278 = 0;
        private static string _FileFilter278 = string.Empty;
        private static string _278Input = string.Empty;
        private static string _278Failed = string.Empty;
        private static string _278Success = string.Empty;
        private static string _278Duplicate = string.Empty;
        private static string _278_VAULT_ROOT_DIRECTORY = string.Empty;
        private static string _278_VAULT_CLIENT_DIRECTORY = string.Empty;
        private static string _278_VAULT_FILE_PATH = string.Empty;


        private static int _ParseTree835 = 0;
        private static string _FileFilter835 = string.Empty;
        private static string _835Input = string.Empty;
        private static string _835Failed = string.Empty;
        private static string _835Success = string.Empty;
        private static string _835Duplicate = string.Empty;
        private static string _835_VAULT_ROOT_DIRECTORY = string.Empty;
        private static string _835_VAULT_CLIENT_DIRECTORY = string.Empty;
        private static string _835_VAULT_FILE_PATH = string.Empty;

        private static int _ParseTree837 = 0;
        private static string _FileFilter837 = string.Empty;
        private static string _837Input = string.Empty;
        private static string _837Failed = string.Empty;
        private static string _837Success = string.Empty;
        private static string _837Duplicate = string.Empty;
        private static string _837_VAULT_ROOT_DIRECTORY = string.Empty;
        private static string _837_VAULT_CLIENT_DIRECTORY = string.Empty;
        private static string _837_VAULT_FILE_PATH = string.Empty;
        private static int _837_USE_VAULT = 0;



        private static int _ParseTree999 = 0;
        private static string _FileFilter999 = string.Empty;
        private static string _999Input = string.Empty;
        private static string _999Failed = string.Empty;
        private static string _999Success = string.Empty;
        private static string _999Duplicate = string.Empty;




        private static MutexAccessRule allowEveryoneRule = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow);
        private static MutexSecurity securitySettings = new MutexSecurity();
        private static bool createdNew;
        public static bool hasHandle = false;
        private static Mutex mutex;




        static void Main(string[] args)
        {



            int r = 0;

            BuildTable();

            GetSettings();


            SingleGlobalInstance(3000);

            if (!hasHandle)
            {
                Environment.Exit(0);

            }


            //************************************************************************************************************
            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.BelowNormal;



            DBUtility dbu = new DBUtility();

            if (_ForceClearTextPasswd == 1)
            {
                dbTempString = _ConnectionString;
            }
            else
            {
                dbTempString = dbu.getConnectionString(_ConnectionString);

                // System.Console.WriteLine(dbTempString);
                //  System.Console.ReadLine();


                if (dbTempString.Contains("Base-64"))
                {

                    System.Console.WriteLine("Failed to decrypt constring EXIT");
                    Environment.Exit(0);

                }

                else
                {

                    _ConnectionString = dbTempString;

                }

            }




            using (SchedulerLog _AL = new SchedulerLog(_AppName))
            {
                _AL.ConnectionString = dbTempString;
                _SchedulerLogID = _AL.AddLogEntry(_AppName, "App Start");
            }





            if (!IsValidProcessYearMonth())
            {
                using (SchedulerLog _AL = new SchedulerLog())
                {
                    _AL.ConnectionString = dbTempString;
                    _AL.UpdateLogEntry(_AppName, "IsValidProcessYearMonth Failed", _SchedulerLogID);
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




            log.ConnectionString = dbTempString;



            if (_Verbose != 0)
            {
                log.ExceptionDetails(_AppName, "App Start");
                Console.Write(_AppName + "App Start");
            }


            if (TouchFolders() != 0)
            {
                Console.Write("Touch Folders failed");
                log.ExceptionDetails(_AppName, "Touch Folders failed make sure folders exist and or in the app.config correct");

                using (SchedulerLog _AL = new SchedulerLog())
                {
                    _AL.ConnectionString = dbTempString;
                    _AL.UpdateLogEntry(_AppName, "Touch Folders Failed", _SchedulerLogID);
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


            if (_Verbose != 0)
            {
                Console.Write("Load EDI exit code: " + Convert.ToString(r) + "\n\r");
            }



            int g = 0;
            //  Environment.Exit(0);
            g = Go();

            // Go2();

            if (g != 0)
            {

                using (SchedulerLog _AL = new SchedulerLog())
                {
                    _AL.ConnectionString = dbTempString;
                    _AL.UpdateLogEntry(_AppName, "Completed but Go returned no 0 see ED GO:" + Convert.ToString(g), _SchedulerLogID);
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
            else
            {
                using (SchedulerLog _AL = new SchedulerLog())
                {
                    _AL.ConnectionString = dbTempString;
                    _AL.UpdateLogEntry(_AppName, "Completed No Errors", _SchedulerLogID);
                }

            }


            if (_WaitForEnterToExit == "1")
            {

                System.Console.WriteLine("\n\rPress Any Key to Exit");
                Console.ReadKey();
                System.Console.WriteLine("\n\rShutting down threads and Exiting");
            }



            // realease the mutex and dsipose of it only thing left is to exit
            if (mutex != null)
            {
                if (hasHandle)
                    mutex.ReleaseMutex();
                mutex.Dispose();
            }
            // so aff code here to wait for all threads to complete then call Enviroment.Exit(0)
            // to tell windows to froce kill any thing left in memory any way just do it 



            Environment.Exit(0);
            //  return 0;

        }





        static bool IsValidProcessYearMonth()
        {
            bool bStatus = false;

            string sProcessYearMonth = _ProcessYearMonth;
            try
            {
                int value;
                if (sProcessYearMonth.Trim().Length == 0)
                {
                    bStatus = true;
                    return bStatus;
                }
                else if (sProcessYearMonth.Trim().Length != 4)
                {
                    Console.WriteLine("\n\rProcess Yera Month should be YYMM format. Please correct it in  Configuration Section.");
                    bStatus = false;
                    return bStatus;
                }
                else if (int.TryParse(sProcessYearMonth, out value))
                {
                    //Continue reading each file and process if the Year Month matches.
                    bStatus = true;
                    return bStatus;
                }
                else
                {
                    Console.WriteLine("\n\rProcess Yera Month should be YYMM format. Please correct it in  Configuration Section.");
                    bStatus = false;
                    return bStatus;
                }
            }
            catch (Exception ex)
            {
                Console.Write("Is Valid Process Year Month Fail" + ex.Message + "\n\r");
                Environment.Exit(0);
            }
            return bStatus;
        }



        static void BuildTable()
        {

            dirs.Columns.Add("EDI_TYPE_LOAD", typeof(int));
            dirs.Columns.Add("EDI_Type", typeof(string));
            dirs.Columns.Add("FileFilter", typeof(string));
            dirs.Columns.Add("InputFolder", typeof(string));
            dirs.Columns.Add("FailedFolder", typeof(string));
            dirs.Columns.Add("SuccessFolder", typeof(string));
            dirs.Columns.Add("DuplicateFolder", typeof(string));
            dirs.Columns.Add("VAULT_ROOT_DIRECTORY", typeof(string));
            dirs.Columns.Add("VAULT_CLIENT_DIRECTORY", typeof(string));
            dirs.Columns.Add("VAULT_FILE_PATH", typeof(string));
            dirs.Columns.Add("USE_VAULT", typeof(int));
        }






        static int Go()
        {

            int r = 0;

            // ... Loop over all rows.
            foreach (DataRow row in dirs.Rows)
            {
                // ... Write value of first field as integer.
                Console.WriteLine(row.Field<string>("EDI_Type"));
                
                
                if (row.Field<int>("EDI_TYPE_LOAD") == 1)
                {
                    using (EDI_5010_LOAD edi = new EDI_5010_LOAD())
                    {


                        Console.Write("\n\r" + _AppName + row.Field<string>("FileFilter"));
                        Console.Write("\n\r" + _AppName + row.Field<string>("InputFolder"));
                        Console.Write("\n\r" + _AppName + row.Field<string>("FailedFolder"));
                        Console.Write("\n\r" + _AppName + row.Field<string>("SuccessFolder"));
                     

                        edi.FileFilter = row.Field<string>("FileFilter");
                        edi.InputFolder = row.Field<string>("InputFolder");
                        edi.FailedFolder = row.Field<string>("FailedFolder");
                        edi.SuccessFolder = row.Field<string>("SuccessFolder");
                        edi.CONSOLE_NAME = _AppName;
                     //   edi.USE_VALUT = row.Field<int>("USE_VAULT");

                        edi.ConnectionString = _ConnectionString;

                        edi.LoadFolder();


                    }
                }

            }

            return r;

        }







        static void GetSettings()
        {

            Console.Write("Get Settings Start\n\r");
            try
            {

                _ConnectionString = Convert.ToString(app.GetValue("ConnStr", _ConnectionString.GetType()));
               // _SourcePath = Convert.ToString(app.GetValue("SourcePath", _SourcePath.GetType())); //ConfigurationSettings.AppSettings["BaseURL"].ToString();
                _Verbose = Convert.ToInt32(app.GetValue("Verbose", _Verbose.GetType())); //ConfigurationSettings.AppSettings["BaseURL"].ToString();
               // _LogFilePath = Convert.ToString(app.GetValue("LogFilePath", _LogFilePath.GetType()));
                _AppName = Convert.ToString(app.GetValue("InstanceName", _AppName.GetType()));
                _WaitForEnterToExit = Convert.ToString(app.GetValue("WaitForEnterToExit", _WaitForEnterToExit.GetType()));
                //_ParseTree = Convert.ToInt32(app.GetValue("ParseTree", _ParseTree.GetType()));



                //  _LOG_EDI = Convert.ToString(app.GetValue("_LOG_EDI", _LOG_EDI.GetType()));


                _ForceClearTextPasswd = Convert.ToInt32(app.GetValue("ForceClearTextPasswd", _ForceClearTextPasswd.GetType()));


//                _ProcessYearMonth = Convert.ToString(app.GetValue("ProcessYearMonth", _ProcessYearMonth.GetType()));

                //_OverrideProcessYearMonth = Convert.ToString(app.GetValue("OverrideProcessYearMonth", _OverrideProcessYearMonth.GetType()));



                //_FilePath = Convert.ToString(app.GetValue("FilePath", _FilePath.GetType()));
                //_FileName = Convert.ToString(app.GetValue("FileName", _FileName.GetType()));
                //_ClientName = Convert.ToString(app.GetValue("ClientName", _ClientName.GetType()));
                //_HOSP_CODE = Convert.ToString(app.GetValue("HOSP_CODE", _HOSP_CODE.GetType()));




                //_SMTPServer = Convert.ToString(app.GetValue("SMTPServer", _SMTPServer.GetType())); //ConfigurationSettings.AppSettings["BaseURL"].ToString();
                //_FromMailAddress = Convert.ToString(app.GetValue("FromMailAddress", _FromMailAddress.GetType())); //ConfigurationSettings.AppSettings["BaseURL"].ToString();
                //_ToMailAddress = Convert.ToString(app.GetValue("ToMailAddress", _ToMailAddress.GetType()));




                //// 270

                //_270Load = Convert.ToInt32(app.GetValue("Load270", _270Load.GetType()));
                //_FileFilter270 = Convert.ToString(app.GetValue("FileFilter270", _FileFilter270.GetType()));
                //_270Input = Convert.ToString(app.GetValue("Input270", _270Input.GetType()));
                //_270Failed = Convert.ToString(app.GetValue("Failed270", _270Failed.GetType()));
                //_270Success = Convert.ToString(app.GetValue("Success270", _270Success.GetType()));
                //_270Duplicate = Convert.ToString(app.GetValue("Duplicate270", _270Duplicate.GetType()));
                //_270_VAULT_ROOT_DIRECTORY = Convert.ToString(app.GetValue("VAULT_ROOT_DIRECTORY_270", _270_VAULT_ROOT_DIRECTORY.GetType()));
                //_270_VAULT_CLIENT_DIRECTORY = Convert.ToString(app.GetValue("VAULT_CLIENT_DIRECTORY_270", _270_VAULT_CLIENT_DIRECTORY.GetType()));
                //_270_VAULT_FILE_PATH = Convert.ToString(app.GetValue("VAULT_FILE_PATH_270", _270_VAULT_FILE_PATH.GetType()));

                //dirs.Rows.Add(_270Load, "270", _FileFilter270, _270Input, _270Failed, _270Success, _270Duplicate, _270_VAULT_ROOT_DIRECTORY, _270_VAULT_CLIENT_DIRECTORY, _270_VAULT_FILE_PATH);



                ////271

                //_271Load = Convert.ToInt32(app.GetValue("Load271", _271Load.GetType()));
                //_FileFilter271 = Convert.ToString(app.GetValue("FileFilter271", _FileFilter271.GetType()));
                //_271Input = Convert.ToString(app.GetValue("Input271", _271Input.GetType()));
                //_271Failed = Convert.ToString(app.GetValue("Failed271", _271Failed.GetType()));
                //_271Success = Convert.ToString(app.GetValue("Success271", _271Success.GetType()));
                //_271Duplicate = Convert.ToString(app.GetValue("Duplicate271", _271Duplicate.GetType()));
                //_271_VAULT_ROOT_DIRECTORY = Convert.ToString(app.GetValue("VAULT_ROOT_DIRECTORY_271", _271_VAULT_ROOT_DIRECTORY.GetType()));
                //_271_VAULT_CLIENT_DIRECTORY = Convert.ToString(app.GetValue("VAULT_CLIENT_DIRECTORY_271", _271_VAULT_CLIENT_DIRECTORY.GetType()));
                //_271_VAULT_FILE_PATH = Convert.ToString(app.GetValue("VAULT_FILE_PATH_271", _271_VAULT_FILE_PATH.GetType()));

                //dirs.Rows.Add(_271Load, "271", _FileFilter271, _271Input, _271Failed, _271Success, _271Duplicate, _271_VAULT_ROOT_DIRECTORY, _271_VAULT_CLIENT_DIRECTORY, _271_VAULT_FILE_PATH);

                ///276

                _276Load = Convert.ToInt32(app.GetValue("Load276", _276Load.GetType()));

                _FileFilter276 = Convert.ToString(app.GetValue("FileFilter276", _FileFilter276.GetType()));
                _276Input = Convert.ToString(app.GetValue("Input276", _276Input.GetType()));
                _276Failed = Convert.ToString(app.GetValue("Failed276", _276Failed.GetType()));
                _276Success = Convert.ToString(app.GetValue("Success276", _276Success.GetType()));
                _276Duplicate = Convert.ToString(app.GetValue("Duplicate276", _276Duplicate.GetType()));
                _276_VAULT_ROOT_DIRECTORY = Convert.ToString(app.GetValue("VAULT_ROOT_DIRECTORY_276", _276_VAULT_ROOT_DIRECTORY.GetType()));
                _276_VAULT_CLIENT_DIRECTORY = Convert.ToString(app.GetValue("VAULT_CLIENT_DIRECTORY_276", _276_VAULT_CLIENT_DIRECTORY.GetType()));
                _276_VAULT_FILE_PATH = Convert.ToString(app.GetValue("VAULT_FILE_PATH_276", _276_VAULT_FILE_PATH.GetType()));

                dirs.Rows.Add(
                    _276Load,
                    "276",
                    _FileFilter276,
                    _276Input,
                    _276Failed,
                    _276Success,
                    _276Duplicate,
                    _276_VAULT_ROOT_DIRECTORY,
                    _276_VAULT_CLIENT_DIRECTORY,
                    _276_VAULT_FILE_PATH
                    );

                ////277

                _277Load = Convert.ToInt32(app.GetValue("Load277", _277Load.GetType()));

                _FileFilter277 = Convert.ToString(app.GetValue("FileFilter277", _FileFilter277.GetType()));
                _277Input = Convert.ToString(app.GetValue("Input277", _277Input.GetType()));
                _277Failed = Convert.ToString(app.GetValue("Failed277", _277Failed.GetType()));
                _277Success = Convert.ToString(app.GetValue("Success277", _277Success.GetType()));
                _277Duplicate = Convert.ToString(app.GetValue("Duplicate277", _277Duplicate.GetType()));
                _277_VAULT_ROOT_DIRECTORY = Convert.ToString(app.GetValue("VAULT_ROOT_DIRECTORY_277", _277_VAULT_ROOT_DIRECTORY.GetType()));
                _277_VAULT_CLIENT_DIRECTORY = Convert.ToString(app.GetValue("VAULT_CLIENT_DIRECTORY_277", _277_VAULT_CLIENT_DIRECTORY.GetType()));
                _277_VAULT_FILE_PATH = Convert.ToString(app.GetValue("VAULT_FILE_PATH_277", _277_VAULT_FILE_PATH.GetType()));

                dirs.Rows.Add(
                    _277Load, 
                    "277", 
                    _FileFilter277,
                    _277Input,
                    _277Failed,
                    _277Success, 
                    _277Duplicate, 
                    _277_VAULT_ROOT_DIRECTORY, 
                    _277_VAULT_CLIENT_DIRECTORY,
                    _277_VAULT_FILE_PATH
                    );



                //// 278

                //_278Load = Convert.ToInt32(app.GetValue("Load278", _278Load.GetType()));

                //_ParseTree278 = Convert.ToInt32(app.GetValue("ParseTree278", _ParseTree278.GetType()));
                //_FileFilter278 = Convert.ToString(app.GetValue("FileFilter278", _FileFilter278.GetType()));
                //_278Input = Convert.ToString(app.GetValue("Input278", _278Input.GetType()));
                //_278Failed = Convert.ToString(app.GetValue("Failed278", _278Failed.GetType()));
                //_278Success = Convert.ToString(app.GetValue("Success278", _278Success.GetType()));
                //_278Duplicate = Convert.ToString(app.GetValue("Duplicate278", _278Duplicate.GetType()));
                //_278_VAULT_ROOT_DIRECTORY = Convert.ToString(app.GetValue("VAULT_ROOT_DIRECTORY_278", _278_VAULT_ROOT_DIRECTORY.GetType()));
                //_278_VAULT_CLIENT_DIRECTORY = Convert.ToString(app.GetValue("VAULT_CLIENT_DIRECTORY_278", _278_VAULT_CLIENT_DIRECTORY.GetType()));
                //_278_VAULT_FILE_PATH = Convert.ToString(app.GetValue("VAULT_FILE_PATH_278", _278_VAULT_FILE_PATH.GetType()));

                //dirs.Rows.Add(_278Load, "278", _FileFilter278, _278Input, _278Failed, _278Success, _278Duplicate, _278_VAULT_ROOT_DIRECTORY, _278_VAULT_CLIENT_DIRECTORY, _278_VAULT_FILE_PATH);



                //// 835

                _835Load = Convert.ToInt32(app.GetValue("Load835", _835Load.GetType()));

                //_ParseTree835 = Convert.ToInt32(app.GetValue("ParseTree835", _ParseTree835.GetType()));
                _FileFilter835 = Convert.ToString(app.GetValue("FileFilter835", _FileFilter835.GetType()));
                _835Input = Convert.ToString(app.GetValue("Input835", _835Input.GetType()));
                _835Failed = Convert.ToString(app.GetValue("Failed835", _835Failed.GetType()));
                _835Success = Convert.ToString(app.GetValue("Success835", _835Success.GetType()));
                _835Duplicate = Convert.ToString(app.GetValue("Duplicate835", _835Duplicate.GetType()));
                _835_VAULT_ROOT_DIRECTORY = Convert.ToString(app.GetValue("VAULT_ROOT_DIRECTORY_835", _835_VAULT_ROOT_DIRECTORY.GetType()));
                _835_VAULT_CLIENT_DIRECTORY = Convert.ToString(app.GetValue("VAULT_CLIENT_DIRECTORY_835", _835_VAULT_CLIENT_DIRECTORY.GetType()));
                _835_VAULT_FILE_PATH = Convert.ToString(app.GetValue("VAULT_FILE_PATH_835", _835_VAULT_FILE_PATH.GetType()));

             //   dirs.Rows.Add(_835Load, "835", _FileFilter835, _835Input, _835Failed, _835Success, _835Duplicate, _835_VAULT_ROOT_DIRECTORY, _835_VAULT_CLIENT_DIRECTORY, _835_VAULT_FILE_PATH);


                dirs.Rows.Add(
                    _835Load,
                    "835",
                    _FileFilter835,
                    _835Input,
                    _835Failed,
                    _835Success,
                    _835Duplicate,
                    _835_VAULT_ROOT_DIRECTORY,
                    _835_VAULT_CLIENT_DIRECTORY,
                    _835_VAULT_FILE_PATH
                    );




                _837Load = Convert.ToInt32(app.GetValue("Load837", _837Load.GetType()));

                //_ParseTree835 = Convert.ToInt32(app.GetValue("ParseTree835", _ParseTree835.GetType()));
                _FileFilter837 = Convert.ToString(app.GetValue("FileFilter837", _FileFilter837.GetType()));
                _837Input = Convert.ToString(app.GetValue("Input837", _837Input.GetType()));
                _837Failed = Convert.ToString(app.GetValue("Failed837", _837Failed.GetType()));
                _837Success = Convert.ToString(app.GetValue("Success837", _837Success.GetType()));
                _837Duplicate = Convert.ToString(app.GetValue("Duplicate837", _837Duplicate.GetType()));
                _837_VAULT_ROOT_DIRECTORY = Convert.ToString(app.GetValue("VAULT_ROOT_DIRECTORY_837", _837_VAULT_ROOT_DIRECTORY.GetType()));
                _837_VAULT_CLIENT_DIRECTORY = Convert.ToString(app.GetValue("VAULT_CLIENT_DIRECTORY_837", _837_VAULT_CLIENT_DIRECTORY.GetType()));
                _837_VAULT_FILE_PATH = Convert.ToString(app.GetValue("VAULT_FILE_PATH_837", _837_VAULT_FILE_PATH.GetType()));

                //   dirs.Rows.Add(_835Load, "835", _FileFilter835, _835Input, _835Failed, _835Success, _835Duplicate, _835_VAULT_ROOT_DIRECTORY, _835_VAULT_CLIENT_DIRECTORY, _835_VAULT_FILE_PATH);


                dirs.Rows.Add(
                    _837Load,
                    "837",
                    _FileFilter837,
                    _837Input,
                    _837Failed,
                    _837Success,
                    _837Duplicate,
                    _837_VAULT_ROOT_DIRECTORY,
                    _837_VAULT_CLIENT_DIRECTORY,
                    _837_VAULT_FILE_PATH
                    );



            }
            catch (Exception ex)
            {


                Console.Write("Get Settings Fail" + ex.Message + "\n\r");
                Environment.Exit(0);

            }
            Console.Write("Runing as: " + _AppName + "\n\r");
            Console.Write("Get Settings Complete" + "\n\r");

        }


        static int TouchFolders()
        {

            bool TouchFailed = false;


            int RE = -1;

            if (_270Load == 1)
            { }
            if (_271Load == 1)
            { }
            if (_276Load == 1)
            {
                if (Touch(_276Input + "\\touch") < 0)
                {
                    TouchFailed = true;
                }

                if (Touch(_276Failed + "\\touch") < 0)
                {
                    TouchFailed = true;
                }


                if (Touch(_276Success + "\\touch") < 0)
                {
                    TouchFailed = true;
                }

                if (Touch(_276Duplicate + "\\touch") < 0)
                {
                    TouchFailed = true;
                }


            }
            if (_277Load == 1)
            {
                if (Touch(_277Input + "\\touch") < 0)
                {
                    TouchFailed = true;
                }

                if (Touch(_277Failed + "\\touch") < 0)
                {
                    TouchFailed = true;
                }


                if (Touch(_277Success + "\\touch") < 0)
                {
                    TouchFailed = true;
                }

                if (Touch(_277Duplicate + "\\touch") < 0)
                {
                    TouchFailed = true;
                }

            }
            if (_278Load == 1)
            {

                if (Touch(_278Input + "\\touch") < 0)
                {
                    TouchFailed = true;
                }

                if (Touch(_278Failed + "\\touch") < 0)
                {
                    TouchFailed = true;
                }


                if (Touch(_278Success + "\\touch") < 0)
                {
                    TouchFailed = true;
                }

                if (Touch(_278Duplicate + "\\touch") < 0)
                {
                    TouchFailed = true;
                }
            }
            if (_835Load == 1)
            {

                if (Touch(_835Input + "\\touch") < 0)
                {
                    TouchFailed = true;
                }

                if (Touch(_835Failed + "\\touch") < 0)
                {
                    TouchFailed = true;
                }


                if (Touch(_835Success + "\\touch") < 0)
                {
                    TouchFailed = true;
                }

                if (Touch(_835Duplicate + "\\touch") < 0)
                {
                    TouchFailed = true;
                }
            }
            if (_837Load == 1)
            {

                if (Touch(_837Input + "\\touch") < 0)
                {
                    TouchFailed = true;
                }

                if (Touch(_837Failed + "\\touch") < 0)
                {
                    TouchFailed = true;
                }


                if (Touch(_837Success + "\\touch") < 0)
                {
                    TouchFailed = true;
                }

                if (Touch(_837Duplicate + "\\touch") < 0)
                {
                    TouchFailed = true;
                }
            }
            if (_939Load == 1)
            {


            }
            if (_HL7Load == 1)
            { }

            if (TouchFailed)
            {
                RE = -1;
            }
            else
            {
                RE = 0;
            }


            //RE = 0;
            return RE;

        }



        ////
        ////Summary:
        ////The CreateNamedPipeServerToListen is used to create a named pipe as MyTestPipe.
        ////To create the pipe, i have used NamedPipeServerStream class of .Net framework 4.x
        ////PipeDirection.InOut : Specifies that the pipe direction is two-way.
        ////NamedPipeServerStream.MaxAllowedServerInstances :  Represents the maximum number of server instances that the system resources allow
        ///// <summary>
        ///// 
        ///// </summary>
        //private static void CreateNamedPipeServerToListen()
        //{

        //    try
        //    {

        //        NamedPipeServerStream pipeServer = new NamedPipeServerStream(_AppName, PipeDirection.InOut, NamedPipeServerStream.MaxAllowedServerInstances);
        //        pipeServer.WaitForConnection();
        //        // before processing message run another thread
        //        Thread thread = new Thread(CreateNamedPipeServerToListen);
        //        thread.Start();
        //        //Process the incoming request from the connected client
        //        ProcessMessage(pipeServer);

        //        if (_Verbose == 1)
        //        {
        //            //   log.ExceptionDetails(_AppName, " named pipe listner is running ");


        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        log.ExceptionDetails(_AppName + " named pipe listner failed to start ", ex);
        //        DropDead = true;
        //        //_quitEvent.Set();
        //    }

        //}



        ///// <summary>
        /////   this handles call back from the threads
        ///// </summary>
        ///// <param name="pipeServer"></param>
        //private static void ProcessMessage(NamedPipeServerStream pipeServer)
        //{

        //    try
        //    {
        //        string NPMsg = string.Empty;
        //        string verb = string.Empty;
        //        string payload = string.Empty;


        //        StreamString sss = new StreamString(pipeServer);

        //        NPMsg = sss.ReadString();


        //        verb = ss.ParseDemlimtedString(NPMsg, "|", 1);
        //        payload = ss.ParseDemlimtedString(NPMsg, "|", 2);



        //        switch (verb)
        //        {

        //            case "BEGIN":

        //                //    TaskCount++;

        //                break;



        //            case "END":

        //                TaskCount--;
        //                _NumberOfRowsCompleted++;


        //                if (TaskCount == 0)
        //                {
        //                    using (SchedulerLog _AL = new SchedulerLog())
        //                    {
        //                        _AL.ConnectionString = dbTempString;
        //                        _AL.UpdateLogEntry(_AppName, "All threads compled End in batchID " + Convert.ToString(_BatchID) + "  for JobID " + Convert.ToString(JobID), _BatchID, _NumberOfRows, _TotalRulesToFire);
        //                    }
        //                }
        //                break;


        //            case "999":

        //                using (SchedulerLog _AL = new SchedulerLog())
        //                {
        //                    _AL.ConnectionString = dbTempString;
        //                    _AL.UpdateLogEntry(_AppName, "Recieved a Panic from a running thread or the kill switch and shutting down", JobID);
        //                }
        //                DropDead = true;
        //                break;
        //        }


        //    }
        //    catch (Exception ex)
        //    {

        //        log.ExceptionDetails(_AppName, ex);
        //        DropDead = true;
        //    }
        //    pipeServer.Close();
        //}



        static int Touch(string fileName)
        {

            int r = -1;

            try
            {

                FileStream myFileStream = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                myFileStream.Close();
                myFileStream.Dispose();
                File.SetLastWriteTimeUtc(fileName, DateTime.UtcNow);
                File.Delete(fileName);
                r = 0;
            }
            catch (Exception ex)
            {
                log.ExceptionDetails(_AppName + " Touch Failed", ex);

            }

            return r;
        }



        static void InitMutex()
        {
            string appGuid = ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false).GetValue(0)).Value.ToString();
            string mutexId = string.Format("Global\\{{{0}}}", _AppName + appGuid);

            mutex = new Mutex(true, mutexId);

            var allowEveryoneRule = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow);
            var securitySettings = new MutexSecurity();
            securitySettings.AddAccessRule(allowEveryoneRule);
            mutex.SetAccessControl(securitySettings);
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
