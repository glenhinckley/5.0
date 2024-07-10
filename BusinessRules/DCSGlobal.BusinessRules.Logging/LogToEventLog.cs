using System;
using System.Text;
using System.Diagnostics;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using System.Data.SqlClient;
using System.IO;
using DCSGlobal.BusinessRules.CoreLibraryII;



namespace DCSGlobal.BusinessRules.Logging
{
    public class LogToEventLog : IDisposable
    {

        ~LogToEventLog()
        {
            Dispose(false);
        }


      
        private string _ipAddress = string.Empty;
        private string _AppPath = string.Empty;
        private string _AppName = string.Empty;
        private string _HostName = string.Empty;

        private string _PreFixx = "DCS_EV_LOGv6";


          private bool _iSet = false;

        bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }



        
        public LogToEventLog(string ipAddress, string AppPath, string AppName, string HostName)
        {

            _ipAddress = ipAddress;
            _AppPath = AppPath;
            _AppName = AppName;
            _HostName = HostName;  
            _iSet = true;
            _PreFixx = _PreFixx + "|" + _HostName + "|" + _AppName + "|" + _AppPath + "|" + _ipAddress + "|" + "MODE=passed" + "|" + "EVENT=";
     
        }

        public LogToEventLog()
        {
            try
            {


                using (MachineID g = new MachineID())
                {

                    _AppPath = g.GetExecutingAssemblyPath;
                    _HostName = g.GetHostName;
                    _ipAddress = g.GetIPAddress;
                }

                _PreFixx = _PreFixx + "|" + _HostName + "|" + _AppName + "|" + _AppPath + "|" + _ipAddress + "|" + "MODE=dll" + "|" + "EVENT=";


                //       _ipAddress = ipAddress;
                //_AppPath = AppPath;
                //_AppName = AppName;

                _iSet = true;
            }
            catch (Exception ex)
            {
                _iSet = false;


            }
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


        private static string _Source = "DCSGLOBAL";
        private static string _Log = "LOG";


        //sSource = "dotNET Sample App";
        //sLog = "Application";
        //Event = "Sample Event";
        public void WriteEventWarning(string Event, int ID)
        {
            try
            {
                if (!EventLog.SourceExists(_Source))
                {
                    EventLog.CreateEventSource(_Source, _Log);
                }
                //   EventLog.WriteEntry(_Source, Event);
                EventLog.WriteEntry(_Source, _PreFixx  + Event, EventLogEntryType.Warning, ID);
            }
            catch (Exception ex)
            {
                using (LogToFile lf = new LogToFile())
                {
                    lf.LogFile("WriteEventWarning", Event, "", 1, ex.Message);
                }
            }
        }


        public void WriteEventWarning(string Event, int ID, string RawData)
        {
            try
            {

                byte[] b;



                _PreFixx = _PreFixx + RawData;

                b = Encoding.ASCII.GetBytes(RawData);



                if (!EventLog.SourceExists(_Source))
                {
                    EventLog.CreateEventSource(_Source, _Log);
                }
                //   EventLog.WriteEntry(_Source, Event);
                EventLog.WriteEntry(_Source, _PreFixx + Event, EventLogEntryType.Warning, ID, 1, b);
            }
            catch (Exception ex)
            {
                using (LogToFile lf = new LogToFile())
                {
                    lf.LogFile("WriteEventWarning", Event, RawData, 1, ex.Message );
                }
            }
        }

        public void WriteEventError(string Event, int ID)
        {
            try
            {

                if (!EventLog.SourceExists(_Source))
                {
                    EventLog.CreateEventSource(_Source, _Log);
                }
                //  EventLog.WriteEntry(_Source, Event);
                EventLog.WriteEntry(_Source, _PreFixx + Event, EventLogEntryType.Error, ID);
            }
            catch (Exception ex)
            {
                using (LogToFile lf = new LogToFile())
                {
                    lf.LogFile("WriteEventError", Event, "", 1, ex.Message);
                }
            }
        }


        public void WriteEventError(string Event, int ID, string RawData)
        {

            try
            {
                byte[] b;

                if (RawData == null)
                {
                    RawData = " ";
                }

                _PreFixx  = _PreFixx + RawData;

                b = Encoding.ASCII.GetBytes(RawData);


                if (!EventLog.SourceExists(_Source))
                {
                    EventLog.CreateEventSource(_Source, _Log);
                }
                //  EventLog.WriteEntry(_Source, Event);
                EventLog.WriteEntry(_Source, _PreFixx + Event, EventLogEntryType.Error, ID, 1, b);
            }
            catch (Exception ex)
            {
                using (LogToFile lf = new LogToFile())
                {
                    lf.LogFile("WriteEventError", Event, RawData, 1, ex.Message);
                }
            }
        }



        public void WriteEventInformation(string Event, int ID)
        {
            try
            {
                if (!EventLog.SourceExists(_Source))
                {
                    EventLog.CreateEventSource(_Source, _Log);
                }
                // EventLog.WriteEntry(_Source, Event);
                EventLog.WriteEntry(_Source, _PreFixx + Event, EventLogEntryType.Information, ID);
            }
            catch (Exception ex)
            {
                using (LogToFile lf = new LogToFile())
                {
                    lf.LogFile("WriteEventInformation", Event, "", 1, ex.Message);
                }
            }
        }



        public void WriteEventInformation(string Event, int ID, string RawData)
        {
            try
            {
                byte[] b;

                _PreFixx = _PreFixx + RawData;

                b = Encoding.ASCII.GetBytes(RawData);

                if (!EventLog.SourceExists(_Source))
                {
                    EventLog.CreateEventSource(_Source, _Log);
                }
                // EventLog.WriteEntry(_Source, Event);
                EventLog.WriteEntry(_Source, _PreFixx + Event, EventLogEntryType.Information, ID, 1, b);
            }
            catch (Exception ex)
            {
                using (LogToFile lf = new LogToFile())
                {
                    lf.LogFile("WriteEventInformation", Event, RawData, 1, ex.Message);
                }
            }
        }






        public void WriteEventFailureAudit(string Event, int ID)
        {
            try
            {
                if (!EventLog.SourceExists(_Source))
                {
                    EventLog.CreateEventSource(_Source, _Log);
                }
                //    EventLog.WriteEntry(_Source, Event);
                EventLog.WriteEntry(_Source, _PreFixx + Event, EventLogEntryType.FailureAudit, ID);
                // EventLog.WriteEntry()
            }
            catch (Exception ex)
            {

                using (LogToFile lf = new LogToFile())
                {
                    lf.LogFile("WriteEventFailureAudit", Event, "", 1, ex.Message);
                }
            }
        }



        public void WriteEventFailureAudit(string Event, int ID, string RawData)
        {
            try
            {
                byte[] b;

                _PreFixx = _PreFixx + RawData;

                b = Encoding.ASCII.GetBytes(RawData);

                if (!EventLog.SourceExists(_Source))
                {
                    EventLog.CreateEventSource(_Source, _Log);
                }
                //    EventLog.WriteEntry(_Source, Event);
                EventLog.WriteEntry(_Source, _PreFixx + Event, EventLogEntryType.FailureAudit, ID, 1, b);
                // EventLog.WriteEntry()
            }
            catch (Exception ex)
            {
                using (LogToFile lf = new LogToFile())
                {
                    lf.LogFile("WriteEventFailureAudit", Event, RawData, 1, ex.Message);
                }
            }
        }



        public void WriteEventSuccessAudit(string Event, int ID)
        {
            try
            {
                if (!EventLog.SourceExists(_Source))
                {
                    EventLog.CreateEventSource(_Source, _Log);
                }
                //  EventLog.WriteEntry(_Source, Event);
                EventLog.WriteEntry(_Source, _PreFixx + Event, EventLogEntryType.SuccessAudit, ID);
            }
            catch (Exception ex)
            {
                using (LogToFile lf = new LogToFile())
                {
                    lf.LogFile("WriteEventSuccessAudit", Event, "", 1, ex.Message);
                }
            }
        }


        public void WriteEventSuccessAudit(string Event, int ID, string RawData)
        {
            try
            {
                byte[] b;

                b = Encoding.ASCII.GetBytes(RawData);

                if (!EventLog.SourceExists(_Source))
                {
                    EventLog.CreateEventSource(_Source, _Log);
                }
                //  EventLog.WriteEntry(_Source, Event);
                EventLog.WriteEntry(_Source, _PreFixx + Event, EventLogEntryType.SuccessAudit, ID, 1, b);
            }
            catch (Exception ex)
            {
                using (LogToFile lf = new LogToFile())
                {
                    lf.LogFile("WriteEventSuccessAudit", Event, RawData, 1, ex.Message);
                }
            }
        }




    }
}
