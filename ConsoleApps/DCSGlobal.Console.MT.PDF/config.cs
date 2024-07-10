using System;
using System.Text;


namespace DCSGlobal.Console.MT.PDF
{
    public class config
    {
        public string ConsoleName { get; set; }
        public string getAllDataSp { get; set; }
        public string ConnectionString { get; set; }
        public string CommandTimeOut { get; set; }
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
        public int verbose { get; set; }
        public int dbPoll { get; set; }

        public int dbPollTimeMilliSeconeds { get; set; }


        public string HospCode { get; set; }



        public string USP_GET_PAT_AUDIT_TRAIL_HL7_ROWS { get; set; }

        public string USP_GET_PAT_AUDIT_TRAIL_HL7_STARTER_IDS { get; set; }

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


        public string CfgUspGetAllData { get; set; }
        public string CfgUspGetAllDataHL7Rows { get; set; }
        public string CfgUspInsertSchedulerlog { get; set; }
        public string CfgUspGetRuleMsg { get; set; }
        public string CfgUspRulesToFire { get; set; }
        public string CfgUspApplyPatAuditTrial { get; set; }
        public string CfgUspTankAddress { get; set; }

        public string CfgUspAddCorrectAddress { get; set; }
        public string CfgUspAddMultipleAddress { get; set; }
        public string CfgUspFormatAddress { get; set; }
        public string CfgUspAddressValidationTrail { get; set; }
        public string CfgUspGetAllTankById { get; set; }
        public string CfgUspRuleInsert { get; set; }
        public string CfgUspRuleResultDelete { get; set; }
        public string CfgUspRuleInsertByXmlTank { get; set; }
        public string CfgUspRuleInsertDebug { get; set; }



        public string addrSvcUrl1 { get; set; }
        public string addrSvcUrl2 { get; set; }
        public string addressRunMode { get; set; }
        public string addrSvcPingLogMin { get; set; }
        public string customerID { get; set; }
        public string licenseKey { get; set; }


        public string MinThreads { get; set; }
        public string MaxThreads { get; set; }
        public string MinWait { get; set; }
        public string MaxWait { get; set; }
        public string CleanupInterval { get; set; }
        public string SchedulingInterval { get; set; }



        public string MaxCount { get; set; }
        public string ThreadCount { get; set; }
        public string ThreadsON { get; set; }




        public string WaitForEnterToExit { get; set; }

        public string NoDataTimeOut { get; set; }

        public int DeadLockRetrys { get; set; }
    }
}
