using System;
using System.Text;


namespace DCSGlobal.Console.Eligibility.HTMLtoPDF
{
    public class config
    {
        public string ConsoleName { get; set; }
        public string ConnectionString { get; set; }
        public string CommandTimeOut { get; set; }
        public int DeadLockRetrys { get; set; }
        public int dbCommandTimeOut { get; set; }
        

   



        public string PDFSaveLocation { get; set; }
        public string cfgUspUpdateEligAdt { get; set; }
        public string cfgUspGetData { get; set; }
        public string UserID { get; set; }


        public int LeftMargin { get; set; }
        public int  RightMargin { get; set; }
        public int TopMargin { get; set; }
        public int BottomMargin { get; set; }


        public bool isCentered { get; set; }
        public bool isPrint { get; set; }
        public bool showOSD { get; set; }
        public bool showOCR { get; set; }
        public bool use800 { get; set; }
        public bool showSummary { get; set; }

        


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

        public int ForceClearTextPasswd { get; set; }

        

        public int verbose { get; set; }
        public string WaitForEnterToExit { get; set; }

        public int ForceRecycle { get; set; }
        public int DisplayHeader { get; set; }

        public string PdfDBProc { get; set; }

        public string doc_id { get; set; }


        
    }
}
