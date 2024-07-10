using System;
using System.Text;
using System.Collections;


namespace DCSGlobal.Console.RealAlertServer
{
    public class config
    {
        public string ConsoleName { get; set; }
        public string getAllDataSp { get; set; }
        public string ConnectionString { get; set; }
        public int CommandTimeOut { get; set; }
        public string SYNC_TIMEOUT { get; set; }
        public string SUBMISSION_TIMEOUT { get; set; }
        public string ErrorLog { get; set; }
        public string isParseVBorDB { get; set; }
        public string isEmdeonLookUp { get; set; }
        public string reRunEligAttempts { get; set; }
        public string uspEdiRequest { get; set; }
        public string uspEdiDbImport { get; set; }
        public string LogPath { get; set; }
        public string ClientSettingsProviderServiceUri { get; set; }

        public string CfgAddrSvcUrl { get; set; }




        public int Verbose { get; set; }

        public int ForceClearTextPasswd { get; set; }



        public string LogFilePath { get; set; }



        public string sAddrFilter { get; set; }
        public string sAddrGetMultipleAddress { get; set; }




        public string RULE_MESSAGE_DELIMETER { get; set; }



        public string RA_VERSION { get; set; }
        public string RA_SERVER_PORT { get; set; }
        public string RA_PROTOCOL { get; set; }
        public string INSERT_INTO_RA_DEBUG_FLAG { get; set; }
        public string RA_MSG_DELIMITER { get; set; }


        public int Metrics { get; set; }

        public int Monitoring { get; set; }




        public int dbPoll { get; set; }

        public int dbPollTimeMilliSeconeds { get; set; }


        public string HospCode { get; set; }



        public string USP_GET_PAT_AUDIT_TRAIL_HL7_ROWS { get; set; }

        public string USP_GET_PAT_AUDIT_TRAIL_HL7_STARTER_IDS { get; set; }



        public int iMaxRowsToProcess { get; set; }
        public string ContextDesc { get; set; }

        public string cfgContextID { get; set; }
        public string cfgHospCode { get; set; }
        public string errorLogPath { get; set; }
        public string debugMode { get; set; }
        public string ruleDebugMode { get; set; }
        public string runMode { get; set; }

        public string resultAll { get; set; }
        public string ruleSuccess { get; set; }
        public string raClientIP { get; set; }
        public string ruleDisplayLimit { get; set; }

        public int ThreadIntervalTimeout { get; set; }





        public string MinThreads { get; set; }
        public string MaxThreads { get; set; }
        public string MinWait { get; set; }
        public string MaxWait { get; set; }
        public string CleanupInterval { get; set; }
        public string SchedulingInterval { get; set; }



        public string MaxCount { get; set; }
        public string ThreadCount { get; set; }
        public string ThreadsON { get; set; }


        public int RecycleWaitTime { get; set; }

        public string WaitForEnterToExit { get; set; }

        public string NoDataTimeOut { get; set; }

        public int DeadLockRetrys { get; set; }
    }
}
