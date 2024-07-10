using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCSGlobal.RA.EligibilityServer
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

        public string ClientCode { get; set; }


        






        public string MaxCount { get; set; }

        public string ThreadCount { get; set; }

        public string ThreadsON { get; set; }

        public string MaxThreads { get; set; }

        public string WaitForEnterToExit { get; set; }

        public string NoDataTimeOut { get; set; }
    }
}
