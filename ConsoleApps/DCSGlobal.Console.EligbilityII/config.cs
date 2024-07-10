using System;
using System.Text;


namespace DCSGlobal.Console.EligbilityII
{
    public class config
    {
        public string ConsoleName { get; set; }
        public string ConnectionString { get; set; }
        public int DeadLockRetrys { get; set; }
        public int dbCommandTimeOut { get; set; }
        

   



        public string cfgUspUpdateEligAdt { get; set; }

       
 
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

        public int dbPoll { get; set; }

  

        public int VendorRetrys { get; set; }

        public string ParameterFilePath { get; set; }
        public int isXMLREQ { get; set; }

         

        public string getAllDataSp { get; set; }
        public string Usp_eligibility_parse_get_req { get; set; }
        public string Usp_eligibility_parse_get_res { get; set; }
        public string Usp_eligibility_set_parse_status { get; set; }
        public string USP_GET_ROWS { get; set; }
        public string USP_GET_REQRES { get; set; }
        public string Usp_eligibility_log_EDI_res { get; set; }



        public string MinThreads { get; set; }
        public string MaxThreads { get; set; }
        public string MinWait { get; set; }
        public string MaxWait { get; set; }
        public string CleanupInterval { get; set; }
        public string SchedulingInterval { get; set; }

        public string MaxCount { get; set; }
        public string ThreadCount { get; set; }
        public string ThreadsON { get; set; }

        public int dbPollTimeMilliSeconeds { get; set; }
        public int RecycleTime { get; set; }

        public int PoolPollTime { get; set; }
        public int RecycleWaitTime { get; set; }

        
        public int verbose { get; set; }
        
        
        public int WaitForEnterToExit { get; set; }
        public int ForceClearTextPasswd { get; set; }


        public string NoDataTimeOut { get; set; }

  








    }
}
