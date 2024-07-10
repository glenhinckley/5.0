using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using DCSGlobal.BusinessRules.CoreLibrary;
//using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using System.Data.SqlClient;
using System.IO;


namespace DCSGlobal.BusinessRules.Logging
{
    public class Metrics : IDisposable
    {
        // constucvtor
        public Metrics()
        {

        }


        // constucvtor
        public Metrics(string MonitorServer)
        {

        }


        //desturctor
        ~Metrics()
        {
            Dispose(false);
        }



        //  private StringStuff objSS = new StringStuff();

        // StringExt t = new StringExt();

        private string _ConnectionString = string.Empty;
       // private string _spLOGED = "usp_insert_exceptions_details";
        private string _MetricsString = string.Empty;
        private int _MetricKey = 0;
        //private string _MetricsServer = "localhost";
       // private string _MonitorServer = "localhost";
        private string _AppName = string.Empty;


       // int _MAX_LENGHT = 1000;




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
                // free other managed objects that implement
                // IDisposable only
            }

            // release any unmanaged objects
            // set the object references to null

            _disposed = true;
        }


        public string ConnectionString
        {

            set
            {

                _ConnectionString = value;
            }
        }



        public string AppName
        {

            set
            {

                _AppName = value;
            }
        }


        public string MetricsString
        {

            get
            {

                return _MetricsString;
            }


        }




        public int AddMetric(string MSG)
        {
            int r = -1;
            _MetricKey++;

            DateTime TimeStamp = new DateTime();
            TimeStamp = DateTime.Now;


            _MetricsString = _MetricsString + Convert.ToString(_MetricKey) + "|" + Convert.ToString(TimeStamp.ToString("MM/dd/yyyy hh:mm:ss.fff tt")) + "|" + MSG + "~";




            return r;
        }


        public int AddMetric(string MSG, string Payload)
        {
            int r = -1;
            _MetricKey++;
            DateTime TimeStamp = new DateTime();
            TimeStamp = DateTime.Now;

            _MetricsString = _MetricsString + Convert.ToString(_MetricKey) + "|" + Convert.ToString(TimeStamp.ToString("MM/dd/yyyy hh:mm:ss.fff tt")) + "|" + MSG + "|" + Payload + "~";


            return r;
        }


        public Guid AddMetricPair(string MSG, string Payload)
        {
            Guid r = Guid.NewGuid();

            DateTime TimeStamp = new DateTime();
            TimeStamp = DateTime.Now;

            _MetricKey++;


            _MetricsString = _MetricsString + Convert.ToString(_MetricKey) + "|" + Convert.ToString(TimeStamp.ToString("MM/dd/yyyy hh:mm:ss.fff tt")) + "|" + MSG + "|" + Payload + "~";


            return r;
        }

        public int AddMetric(string MSG, string Payload, Guid PGUID)
        {
            int r = -1;
            _MetricKey++;
            DateTime TimeStamp = new DateTime();
            TimeStamp = DateTime.Now;

            _MetricsString = _MetricsString + Convert.ToString(_MetricKey) + "|" + Convert.ToString(TimeStamp.ToString("MM/dd/yyyy hh:mm:ss.fff tt")) + "|" + MSG + "|" + Payload + "|" + Convert.ToString(PGUID) + "~";


            return r;
        }



        public int AddSubMetric(string Payload, Guid PGUID)
        {
            int r = -1;
            DateTime TimeStamp = new DateTime();
            TimeStamp = DateTime.Now;

            return r;
        }



        public int FlushMetricStringToED()
        {

            int r = -1;

            using (logExecption log = new logExecption())
            {
                log.ConnectionString = _ConnectionString;
                log.ExceptionDetailsMetrics(_AppName, _MetricsString);

            }


            return r;

        }


        public int FlushMetricStringToDisc(string Path)
        {

            int r = -1;

            using (LogToFile log = new LogToFile())
            {

                log.WriteToFile(_MetricsString, Path);

            }


            return r;

        }


        //  WriteToFile



        public void Clear()
        {
            _MetricsString = string.Empty;
        }



    }
}
