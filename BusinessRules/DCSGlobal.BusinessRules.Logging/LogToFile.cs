using System;
using System.Text;
using System.IO;


namespace DCSGlobal.BusinessRules.Logging
{
    public class LogToFile : IDisposable
    {


        private string sLogFormat;
        private string sErrorTime;

        //  private StringStuff objSS = new StringStuff();

        // StringExt t = new StringExt();

        private string _ConnectionString = string.Empty;
        //     private string _spLOGED = "usp_insert_exceptions_details";
        private string _MetricsString = string.Empty;
        //   private int _MetricKey = 0;
        //   private string _MetricsServer = "localhost";
        //   private string _MonitorServer = "localhost";
        private string _AppName = string.Empty;


        //   int _MAX_LENGHT = 1000;




        bool _disposed;








        // constucvtor
        public LogToFile()
        {

        }


        // constucvtor
        public LogToFile(string MonitorServer)
        {

        }


        //desturctor
        ~LogToFile()
        {
            Dispose(false);
        }





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
                // free other managed objects that implement
                // IDisposable only
            }

            // release any unmanaged objects
            // set the object references to null

            _disposed = true;
        }











        public void CreateLogFile()
        {
            //sLogFormat used to create log files format :
            // dd/mm/yyyy hh:mm:ss AM/PM ==> Log Message
            sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";

            //this variable used to create log filename format "
            //for example filename : ErrorLogYYYYMMDD
            string sYear = DateTime.Now.Year.ToString();
            string sMonth = DateTime.Now.Month.ToString();
            string sDay = DateTime.Now.Day.ToString();
            sErrorTime = sYear + sMonth + sDay;
        }




        public void LogFile(string sExceptionName, string sEventName, string sControlName, int nErrorLineNo, string sFormName)
        {

            StreamWriter log;

            if (!File.Exists("logfile.txt"))
            {

                log = new StreamWriter("logfile.txt");

            }

            else
            {

                log = File.AppendText("logfile.txt");

            }

            // Write to the file:

            log.WriteLine("Data Time:" + DateTime.Now);
            log.WriteLine("Exception Name:" + sExceptionName);
            log.WriteLine("Event Name:" + sEventName);
            log.WriteLine("Control Name:" + sControlName);
            log.WriteLine("Error Line No.:" + nErrorLineNo);
            log.WriteLine("Form Name:" + sFormName);

            // Close the stream:

            log.Close();

        }





        public void WriteToFile(string text, string Path)
        {
            string path = Path;




            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(text);
                writer.Close();
            }
        }












    }
}
