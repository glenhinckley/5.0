using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Configuration;
using System.Data;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using System.Data.SqlClient;
using DCSGlobal.BusinessRules.CoreLibraryII;

namespace DCSGlobal.BusinessRules.Logging
{
    public class SchedulerLog : IDisposable
    {

        StringStuff objSS = new StringStuff();
        SqlConnection CON = new SqlConnection();
        logExecption log = new logExecption();
        string _ConnectionString = "";

        private bool _iSet = false;

        private string _ipAddress = string.Empty;
        private string _AppPath = string.Empty;
        private string _AppName = string.Empty;
        private string _HostName = string.Empty;
        private string _InstanceName = string.Empty;

        private string _PreFixx = "DCS_SL_LOGv6";

        private bool _FailOver = false;

        public SchedulerLog(string ipAddress, string AppPath, string AppName, string HostName)
        {

            _ipAddress = ipAddress;
            _AppPath = AppPath;
            _AppName = AppName;
            _HostName = HostName;
            _iSet = true;
            _PreFixx = _PreFixx + "|" + _HostName + "|" + _AppName + "|" + _AppPath + "|" + _ipAddress + "|" ;

        }



        public SchedulerLog(string ipAddress, string AppPath, string AppName, string HostName, string InstanceName)
        {

            _ipAddress = ipAddress;
            _AppPath = AppPath;
            _AppName = AppName;
            _HostName = HostName;
            _iSet = true;
            _PreFixx = _PreFixx + "|" + _HostName + "|" + _AppName + "|" + _AppPath + "|" + _ipAddress + "|";

        }


        public SchedulerLog(string AppName)
        {
            try
            {


                using (MachineID g = new MachineID())
                {

                    _AppPath = g.GetExecutingAssemblyPath;
                    _HostName = g.GetHostName;
                    _ipAddress = g.GetIPAddress;
                }

                _PreFixx = _PreFixx + "|" + _HostName + "|" + AppName + "|" + _AppPath + "|" + _ipAddress + "|" ;


                //       _ipAddress = ipAddress;
                //_AppPath = AppPath;
                //_AppName = AppName;

                _iSet = true;
            }
            catch (Exception ex)
            {



                using (LogToEventLog el = new LogToEventLog())
                {
                    el.WriteEventError("Log to ED Failed constructor scheduleo(string AppName)", 1, ex.Message);

                }
                _iSet = false;


            }
        }



        public SchedulerLog()
        {
            try
            {


                using (MachineID g = new MachineID())
                {

                    _AppPath = g.GetExecutingAssemblyPath;
                    _HostName = g.GetHostName;
                    _ipAddress = g.GetIPAddress;
                }

                _PreFixx = _PreFixx + "|" + _HostName + "|" + _AppName + "|" + _AppPath + "|" + _ipAddress + "|";


                //       _ipAddress = ipAddress;
                //_AppPath = AppPath;
                //_AppName = AppName;

                _iSet = true;
            }
            catch (Exception ex)
            {

                using (LogToEventLog el = new LogToEventLog())
                {
                    el.WriteEventError("Log to ED Failed constructor scheduler()", 1, ex.Message);

                }
                _iSet = false;


            }
        }


        public bool FailOver
        {

            set
            {

                _FailOver = value;
            }
        }

        ~SchedulerLog()
        {
            Dispose(false);
        }



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
                CON.Dispose();
                log.Dispose();
                // free other managed objects that implement
                // IDisposable only
            }

            objSS = null;

            // release any unmanaged objects
            // set the object references to null

            _disposed = true;
        }


        public string ConnectionString
        {

            set
            {

                _ConnectionString = value;
                log.ConnectionString = value;
            }
        }













        public int AppEnd(string ProgramName, string Comment, int BatchID)
        {

            int r = 0;

            string sqlQuery;
            sqlQuery = "usp_insert_into_Scheduler_log";

            try
            {
                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@program_name", SqlDbType.VarChar).Value = ProgramName;
                        cmd.Parameters.Add("@start_ts", SqlDbType.DateTime).Value = "00:00:00";
                        cmd.Parameters.Add("@end_ts", SqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@nbr_rows", SqlDbType.Int).Value = 0;
                        cmd.Parameters.Add("@nbr_rules", SqlDbType.Int).Value = 0;
                        cmd.Parameters.Add("@batch_id", SqlDbType.Int).Value = BatchID;
                        cmd.Parameters.Add("@comments", SqlDbType.VarChar).Value = _PreFixx + Comment;
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }


                    con.Close();


                }


            }
            catch (SqlException sx)
            {
                r = -2;

                if (_FailOver)
                {
                    using (LogToEventLog el = new LogToEventLog())
                    {
                        el.WriteEventError("Log to APP Failed with SQL Execption", 1, sx.Message);

                    }
                }

            }


            return r;

        }







        public int UpdateLogEntry(string _ProgramName, string _Comment, int _BatchID)
        {
            int r = -1;
            try
            {
                string sqlQuery;
                sqlQuery = "usp_insert_into_Scheduler_log";
                r = 0;

                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {
                    con.Open();
                    //
                    // Create new SqlCommand object.
                    //
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@program_name", SqlDbType.VarChar, 100).Value = _ProgramName;
                        cmd.Parameters.Add("@start_ts", SqlDbType.DateTime).Value = DBNull.Value;
                        cmd.Parameters.Add("@end_ts", SqlDbType.DateTime).Value = DBNull.Value;
                        cmd.Parameters.Add("@nbr_rows", SqlDbType.Int).Value = DBNull.Value;
                        cmd.Parameters.Add("@nbr_rules", SqlDbType.Int).Value = DBNull.Value;
                        cmd.Parameters.Add("@batch_id", SqlDbType.Int).Value = _BatchID;
                        cmd.Parameters.Add("@comments", SqlDbType.VarChar, 1000).Value = _PreFixx + _Comment;
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                if (_FailOver)
                {
                    using (LogToEventLog el = new LogToEventLog())
                    {
                        log.ExceptionDetails(_ProgramName + ": Update Log Entry", ex);
                    }
                }
                r = -1;
            }

            return r;


        }

        public int UpdateLogEntry(string _ProgramName, string _Comment, int _BatchID, int _NumberOfRows, int NumberOfRules)
        {

            int r = -1;
            try
            {
                string sqlQuery;
                sqlQuery = "usp_insert_into_Scheduler_log";
                r = 0;

                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {
                    con.Open();
                    //
                    // Create new SqlCommand object.
                    //
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@program_name", SqlDbType.VarChar, 100).Value = _ProgramName;
                        cmd.Parameters.Add("@start_ts", SqlDbType.DateTime).Value = DBNull.Value;
                        cmd.Parameters.Add("@end_ts", SqlDbType.DateTime).Value = DBNull.Value;
                        cmd.Parameters.Add("@nbr_rows", SqlDbType.Int).Value = _NumberOfRows;
                        cmd.Parameters.Add("@nbr_rules", SqlDbType.Int).Value = NumberOfRules;
                        cmd.Parameters.Add("@batch_id", SqlDbType.Int).Value = _BatchID;
                        cmd.Parameters.Add("@comments", SqlDbType.VarChar, 1000).Value = _PreFixx + _Comment;

                        cmd.ExecuteNonQuery();

                    }
                    con.Close();
                }


            }
            catch (Exception ex)
            {
                if (_FailOver)
                {
                    using (LogToEventLog el = new LogToEventLog())
                    {
                        log.ExceptionDetails(_ProgramName + ": Update Log Entry NR NR", ex);

                    }
                }



                r = -1;
            }


            return r;


        }

        public int UpdateLogEntry(string _ProgramName, string _Comment, int _BatchID, DateTime end_ts, int _NumberOfRows, int NumberOfRules)
        {

            int r = -1;
            try
            {
                string sqlQuery;
                sqlQuery = "usp_insert_into_Scheduler_log";
                r = 0;

                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {
                    con.Open();
                    //
                    // Create new SqlCommand object.
                    //
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@program_name", SqlDbType.VarChar, 100).Value = _ProgramName;
                        cmd.Parameters.Add("@start_ts", SqlDbType.DateTime).Value = DBNull.Value;
                        cmd.Parameters.Add("@end_ts", SqlDbType.DateTime).Value = end_ts;
                        cmd.Parameters.Add("@nbr_rows", SqlDbType.Int).Value = _NumberOfRows;
                        cmd.Parameters.Add("@nbr_rules", SqlDbType.Int).Value = NumberOfRules;
                        cmd.Parameters.Add("@batch_id", SqlDbType.Int).Value = _BatchID;
                        cmd.Parameters.Add("@comments", SqlDbType.VarChar, 1000).Value = _PreFixx + _Comment;

                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }



            }
            catch (Exception ex)
            {
                if (_FailOver)
                {
                    using (LogToEventLog el = new LogToEventLog())
                    {
                        log.ExceptionDetails(_ProgramName + ": Update Log Entry NR NR", ex);

                    }
                }



                r = -1;
            }


            return r;


        }


        private int InsertPrefixData(string ProgramName, string Comment, int BatchID)
        {

            int r = -1;
            try
            {
                string sqlQuery;
                sqlQuery = "usp_insert_into_Scheduler_log";
                r = 0;

                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {
                    con.Open();
                    //
                    // Create new SqlCommand object.
                    //
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@program_name", SqlDbType.VarChar, 100).Value = ProgramName;
                        cmd.Parameters.Add("@start_ts", SqlDbType.DateTime).Value = DBNull.Value;
                        cmd.Parameters.Add("@end_ts", SqlDbType.DateTime).Value = DBNull.Value;
                        cmd.Parameters.Add("@nbr_rows", SqlDbType.Int).Value = DBNull.Value;
                        cmd.Parameters.Add("@nbr_rules", SqlDbType.Int).Value = DBNull.Value;
                        cmd.Parameters.Add("@batch_id", SqlDbType.Int).Value = BatchID;
                        cmd.Parameters.Add("@comments", SqlDbType.VarChar, 1000).Value = _PreFixx + Comment;

                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }



            }
            catch (Exception ex)
            {
                if (_FailOver)
                {
                    using (LogToEventLog el = new LogToEventLog())
                    {
                        log.ExceptionDetails(ProgramName + ": Update prefix data Entry NR NR", ex);

                    }
                }



                r = -1;
            }


            return r;


        }

        public int AddLogEntry(string _ProgramName, string _Comment)
        {

            int r = -1;
            try
            {
                string sqlQuery;
                sqlQuery = "usp_insert_into_Scheduler_log";
                r = 0;

                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {
                    con.Open();
                    //
                    // Create new SqlCommand object.
                    //
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@program_name", SqlDbType.VarChar, 100).Value = _ProgramName;
                        cmd.Parameters.Add("@start_ts", SqlDbType.DateTime).Value = DBNull.Value;
                        cmd.Parameters.Add("@end_ts", SqlDbType.DateTime).Value = DBNull.Value;
                        cmd.Parameters.Add("@nbr_rows", SqlDbType.Int).Value = DBNull.Value;
                        cmd.Parameters.Add("@nbr_rules", SqlDbType.Int).Value = DBNull.Value;
                        cmd.Parameters.Add("@batch_id", SqlDbType.Int).Value = 0;
                        cmd.Parameters.Add("@comments", SqlDbType.VarChar, 1000).Value =  _Comment;


                        // cmd.Parameters.Add("@batch_id", SqlDbType.Int);
                        cmd.Parameters["@batch_id"].Direction = ParameterDirection.InputOutput;

                        cmd.ExecuteNonQuery();

                        r = Convert.ToInt32(cmd.Parameters["@batch_id"].Value);
                    }
                    con.Close();

                    InsertPrefixData(_ProgramName, _Comment, r);

                }

            }
            catch (Exception ex)
            {
                if (_FailOver)
                {
                    using (LogToEventLog el = new LogToEventLog())
                    {
                        log.ExceptionDetails(_ProgramName + ": Add Log Entry", ex);

                    }
                }

                r = -1;
            }


            return r;





        }


        //      iRetScheduler = sch.AddLogEntry(sAssemblyProcessID, Convert.ToString(dtStart_ts), "", iRowsToProcess, iTotalRulesToFire, iBatchID)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_ProgramName"></param>
        /// <param name="_Comment"></param>
        /// <param name="_BatchID"></param>
        /// <param name="start_ts"></param>
        /// <param name="end_ts"></param>
        /// <param name="NumberOfRows"></param>
        /// <param name="NumberOfRules"></param>
        /// <returns></returns>
        public int AddLogEntry(string _ProgramName, string _Comment, int _BatchID, DateTime start_ts, DateTime end_ts, int NumberOfRows, int NumberOfRules)
        {

            int r = -1;
            try
            {
                string sqlQuery;
                sqlQuery = "usp_insert_into_Scheduler_log";
                r = 0;

                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {
                    con.Open();
                    //
                    // Create new SqlCommand object.
                    //
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@program_name", SqlDbType.VarChar, 100).Value = _ProgramName;
                        cmd.Parameters.Add("@start_ts", SqlDbType.DateTime).Value = start_ts;
                        cmd.Parameters.Add("@end_ts", SqlDbType.DateTime).Value = end_ts;
                        cmd.Parameters.Add("@nbr_rows", SqlDbType.Int).Value = NumberOfRows;
                        cmd.Parameters.Add("@nbr_rules", SqlDbType.Int).Value = NumberOfRules;
                        cmd.Parameters.Add("@batch_id", SqlDbType.Int).Value = _BatchID;
                        cmd.Parameters.Add("@comments", SqlDbType.VarChar, 1000).Value = _PreFixx + _Comment;


                        // cmd.Parameters.Add("@batch_id", SqlDbType.Int);
                        cmd.Parameters["@batch_id"].Direction = ParameterDirection.InputOutput;

                        // execute your query successfully



                        cmd.ExecuteNonQuery();

                        r = Convert.ToInt32(cmd.Parameters["@batch_id"].Value);
                    }
                    con.Close();
                }

            }
            catch (Exception ex)
            {


                if (_FailOver)
                {
                    using (LogToEventLog el = new LogToEventLog())
                    {
                        log.ExceptionDetails(_ProgramName + ": Add Log Entry", ex);

                    }
                }

                r = -1;
            }
            return r;


        }

        public int AddLogEntry(string _ProgramName, string _Comment, int _BatchID, DateTime start_ts, int NumberOfRows, int NumberOfRules)
        {

            int r = -1;
            try
            {
                string sqlQuery;
                sqlQuery = "usp_insert_into_Scheduler_log";
                r = 0;

                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {
                    con.Open();
                    //
                    // Create new SqlCommand object.
                    //
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@program_name", SqlDbType.VarChar, 100).Value = _ProgramName;
                        cmd.Parameters.Add("@start_ts", SqlDbType.DateTime).Value = start_ts;
                        cmd.Parameters.Add("@end_ts", SqlDbType.DateTime).Value = DBNull.Value;
                        cmd.Parameters.Add("@nbr_rows", SqlDbType.Int).Value = NumberOfRows;
                        cmd.Parameters.Add("@nbr_rules", SqlDbType.Int).Value = NumberOfRules;
                        cmd.Parameters.Add("@batch_id", SqlDbType.Int).Value = _BatchID;
                        cmd.Parameters.Add("@comments", SqlDbType.VarChar, 1000).Value = _PreFixx + _Comment;


                        // cmd.Parameters.Add("@batch_id", SqlDbType.Int);
                        cmd.Parameters["@batch_id"].Direction = ParameterDirection.InputOutput;

                        // execute your query successfully



                        cmd.ExecuteNonQuery();

                        r = Convert.ToInt32(cmd.Parameters["@batch_id"].Value);
                    }
                    con.Close();
                }

            }
            catch (Exception ex)
            {

                if (_FailOver)
                {
                    using (LogToEventLog el = new LogToEventLog())
                    {
                        log.ExceptionDetails(_ProgramName + ": Add Log Entry", ex);

                    }
                }

                r = -1;
            }

            return r;


        }
    }
}



