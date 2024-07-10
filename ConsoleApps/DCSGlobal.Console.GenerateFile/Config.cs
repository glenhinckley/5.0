using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 


namespace DCSGlobal.Console.GenerateFile
{
    class Config
    {
        public string ConsoleName { get; set; }
        public string ConnectionString { get; set; }
        public string MinThreads { get; set; }
        public string MaxThreads { get; set; }
        public string ThreadsON { get; set; }
        public int verbose { get; set; }
        public string SP_NAME_FETCH { get; set; }
        public string SP_NAME_UPDATE { get; set; }
        public string MinWait { get; set; }
        public string MaxWait { get; set; }
        public string CleanupInterval { get; set; }
        public string SchedulingInterval { get; set; }
        public int dbPollTimeMilliSeconeds { get; set; }
        public int LoadFromDB { get; set; }
        public string DEST_FOLDER { get; set; }
        public int ForceClearTextPasswd { get; set; }
        public int RecycleWaitTime { get; set; }
        public int PoolPollTime { get; set; }
        public string WaitForEnterToExit { get; set; }
        public string ARCHIVE_FOLDER { get; set; }
        public int RecycleTime { get; set; }

    }
}

