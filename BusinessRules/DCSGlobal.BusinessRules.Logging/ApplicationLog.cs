using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using System.Data.SqlClient;
using DCSGlobal.BusinessRules.CoreLibraryII;


namespace DCSGlobal.BusinessRules.Logging
{
    public class ApplicationLog : IDisposable
    {


        private bool _iSet = false;
        private bool _FailOver = false;
        private string _ipAddress = string.Empty;
        private string _AppPath = string.Empty;
        private string _AppName = string.Empty;
        private string _HostName = string.Empty;

        private string _PreFixx = "DCS_AL_LOGv6";

        StringStuff objSS = new StringStuff();

        string _ConnectionString = "";
        string _spAPPLOG = "usp_insert_into_Scheduler_log";



        ~ApplicationLog()
        {
            Dispose(false);
        }


        public ApplicationLog(string ipAddress, string AppPath, string AppName, string HostName)
        {

            _ipAddress = ipAddress;
            _AppPath = AppPath;
            _AppName = AppName;
            _HostName = HostName;
            _iSet = true;
            _PreFixx = _PreFixx + "|" + _HostName + "|" + _AppName + "|" + _AppPath + "|" + _ipAddress + "|" + "MODE=passed" + "|" + "EVENT=";

        }



        public ApplicationLog(string AppName)
        {
            try
            {


                using (MachineID g = new MachineID())
                {

                    _AppPath = g.GetExecutingAssemblyPath;
                    _HostName = g.GetHostName;
                    _ipAddress = g.GetIPAddress;
                }

                _PreFixx = _PreFixx + "|" + _HostName + "|" + AppName + "|" + _AppPath + "|" + _ipAddress + "|" + "MODE=app" + "|" + "EVENT=";


                //       _ipAddress = ipAddress;
                //_AppPath = AppPath;
                //_AppName = AppName;

                _iSet = true;
            }
            catch (Exception ex)
            {

                using (LogToEventLog el = new LogToEventLog())
                {
                    el.WriteEventError("Log to ED Failed constructor ApplicationLog(string AppName)", 1, ex.Message);

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

        public ApplicationLog()
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



        public int AddSechdeulerEntry(string ProgramName, string StartTime, string EndTime, string Comment)
        {

            int r = 0;






            try
            {
                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(_spAPPLOG, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@program_name", SqlDbType.Int).Value = ProgramName;
                        cmd.Parameters.Add("@start_ts", SqlDbType.VarChar).Value = StartTime;
                        cmd.Parameters.Add("@end_ts", SqlDbType.VarChar).Value = EndTime;
                        cmd.Parameters.Add("@nbr_rows", SqlDbType.Int).Value = 0;
                        cmd.Parameters.Add("@nbr_rules", SqlDbType.Int).Value = 0;
                        cmd.Parameters.Add("@batch_id", SqlDbType.Int).Value = 0;
                        cmd.Parameters.Add("@comments", SqlDbType.VarChar).Value = _PreFixx + Comment;
                        //    con.Open();
                        //    cmd.ExecuteNonQuery();
                    }


                    // con.Close();  


                }


            }
            catch (Exception ex)
            {
                r = -1;
                if (_FailOver)
                {
                    using (LogToEventLog el = new LogToEventLog())
                    {
                        el.WriteEventError("Log to APP Failed", 1, ex.Message);

                    }
                }
            }



            finally
            {

            }


            return r;

        }

        public int AppStart(string ProgramName, string Comment)
        {

            int r = 0;

            //            @program_name varchar(100),
            //@start_ts datetime,
            //@end_ts datetime,
            //@nbr_rows int, 
            //@nbr_rules int, 
            //@batch_id int OUT ,
            //@comments Varchar(1000)='NA',
            //@end_tank_ts datetime = '1/1/1900'  


            try
            {
                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(_spAPPLOG, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@program_name", SqlDbType.VarChar).Value = ProgramName;
                        cmd.Parameters.Add("@start_ts", SqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@end_ts", SqlDbType.DateTime).Value = "00:00:00";
                        cmd.Parameters.Add("@nbr_rows", SqlDbType.Int).Value = 0;
                        cmd.Parameters.Add("@nbr_rules", SqlDbType.Int).Value = 0;
                        cmd.Parameters.Add("@batch_id", SqlDbType.Int).Value = 0;
                        cmd.Parameters.Add("@comments", SqlDbType.VarChar).Value = _PreFixx + Comment;


                        var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                        returnParameter.Direction = ParameterDirection.ReturnValue;

                        con.Open();
                        cmd.ExecuteNonQuery();
                        var resnult = returnParameter.Value;

                        con.Close();
                    }
                }
            }


            catch (Exception ex)
            {
                r = -1;
                if (_FailOver)
                {
                    using (LogToEventLog el = new LogToEventLog())
                    {
                        el.WriteEventError("Log to APP Failed", 1, ex.Message);

                    }
                }
            }



            return r;

        }



        public int AppEnd(string ProgramName, string Comment)
        {

            int r = 0;

            try
            {
                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(_spAPPLOG, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@program_name", SqlDbType.VarChar).Value = ProgramName;
                        cmd.Parameters.Add("@start_ts", SqlDbType.DateTime).Value = "00:00:00";
                        cmd.Parameters.Add("@end_ts", SqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@nbr_rows", SqlDbType.Int).Value = 0;
                        cmd.Parameters.Add("@nbr_rules", SqlDbType.Int).Value = 0;
                        cmd.Parameters.Add("@batch_id", SqlDbType.Int).Value = 0;
                        cmd.Parameters.Add("@comments", SqlDbType.VarChar).Value = _PreFixx + Comment;
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }


                    con.Close();


                }


            }


            catch (Exception ex)
            {
                r = -1;

                if (_FailOver)
                {
                    using (LogToEventLog el = new LogToEventLog())
                    {
                        el.WriteEventError("Log to APP Failed", 1, ex.Message);

                    }
                }
            }




            return r;

        }


        public int AppEnd(string ProgramName, string Comment, int BatchID)
        {

            int r = 0;

            try
            {
                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(_spAPPLOG, con))
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
            catch (Exception ex)
            {
                r = -1;


                if (_FailOver)
                {
                    using (LogToEventLog el = new LogToEventLog())
                    {
                        el.WriteEventError("Log to APP Failed", 1, ex.Message);

                    }
                }
            }

            return r;

        }


        public int AddApplicationLogEntry(int _DOMAIN_ID, int _CAMPUS_ID, int _APPLICATION_ID, int _BUILD_ID,
                                            int _APP_USER_ID, string _SUBJECT, string _BODY)
        {

            int r = 0;


            //@_DOMAIN_ID INT,
            //@_CAMPUS_ID INT,
            //@_APPLICATION_ID INT,
            //@_BUILD_ID INT,
            //@_APP_USER_ID INT,
            //@_SUBJECT VARCHAR(255) ,
            //@_BODY VARCHAR(MAX)



            //--[sp_ADD_APPLICATION_LOG_ENTRY] 1,1,2,2,51,'TEST','NOTE IN A VARCHAR MAX THAT MOANOJ SAYS IS BAD'

            using (SqlConnection con = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ADD_APPLICATION_LOG_ENTRY", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@_DOMAIN_ID", SqlDbType.Int).Value = _DOMAIN_ID;
                    cmd.Parameters.Add("@_CAMPUS_ID", SqlDbType.Int).Value = _CAMPUS_ID;
                    cmd.Parameters.Add("@_APPLICATION_ID", SqlDbType.Int).Value = _APPLICATION_ID;
                    cmd.Parameters.Add("@_BUILD_ID", SqlDbType.Int).Value = _BUILD_ID;
                    cmd.Parameters.Add("@_APP_USER_ID", SqlDbType.Int).Value = _APP_USER_ID;
                    cmd.Parameters.Add("@_SUBJECT", SqlDbType.VarChar).Value = _SUBJECT;
                    cmd.Parameters.Add("@_BODY", SqlDbType.VarChar).Value = _PreFixx + _BODY;
                    // con.Open();
                    // cmd.ExecuteNonQuery();
                }

                //con.Close();

                return r;

            }






        }
    }
}
