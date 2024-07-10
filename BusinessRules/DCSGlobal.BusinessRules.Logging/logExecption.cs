using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Configuration;
using System.Data;
//using DCSGlobal.BusinessRules.CoreLibrary;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using System.Data.SqlClient;
using System.IO;
using DCSGlobal.BusinessRules.CoreLibraryII;

namespace DCSGlobal.BusinessRules.Logging
{




    public class logExecption : IDisposable
    {


        StringStuff objSS = new StringStuff();

        // StringExt t = new StringExt();

        string _ConnectionString = string.Empty;
        string _spLOGED = "usp_insert_exceptions_details";


        private bool _iSet = false;
        private bool _FailOver = false;

        private bool _MLOG = false;

        private string _ipAddress = string.Empty;
        private string _AppPath = string.Empty;
        private string _AppName = string.Empty;
        private string _HostName = string.Empty;

        private string _PreFixx = "DCS_ED_LOGv6";
        private string _StackTracePreFixx = "DCS_ST_LOGv6";
        private string _StackTrace = string.Empty;

        int _MAX_LENGHT = 2000;

        ~logExecption()
        {
            Dispose(false);
        }


        public logExecption(string ipAddress, string AppPath, string AppName, string HostName)
        {

            _ipAddress = ipAddress;
            _AppPath = AppPath;
            _AppName = AppName;
            _HostName = HostName;
            _iSet = true;
            _PreFixx = _PreFixx + "|" + _HostName + "|" + _AppName + "|" + _AppPath + "|" + _ipAddress + "|";

        }

        public logExecption()
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
                _iSet = true;
            }
            catch (Exception ex)
            {

                using (LogToEventLog el = new LogToEventLog())
                {
                    el.WriteEventError("Log to ED Failed constructor  logExecption()", 1, ex.Message);

                }
                _iSet = false;

            }
        }



        public logExecption(string AppName)
        {
            try
            {
                using (MachineID g = new MachineID())
                {

                    _AppPath = g.GetExecutingAssemblyPath;
                    _HostName = g.GetHostName;
                    _ipAddress = g.GetIPAddress;
                }
                _PreFixx = _PreFixx + "|" + _HostName + "|" + AppName + "|" + _AppPath + "|" + _ipAddress + "|";
                _iSet = true;
            }
            catch (Exception ex)
            {

                using (LogToEventLog el = new LogToEventLog())
                {
                    el.WriteEventError("Log to ED Failed constructor logExecption(string AppName)", 1, ex.Message);

                }
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

        public bool FailOver
        {

            set
            {

                _FailOver = value;
            }
        }


        public bool MLOG_WRITE
        {

            set
            {

                _MLOG = value;
            }
        }
        public string spLOGED
        {

            set
            {

                _spLOGED = value;
            }
        }


        /// <summary>
        /// Quick Log ex.message, ex.messages0urce , Function Name
        /// </summary>
        /// <remarks>Quick log</remarks>
        public int ExceptionDetails(string ReferenceID, string exMessage, string exStackTrace, string ExceptionProcedure)
        {

            int r = -1;


            try
            {

                // Create new SqlConnection object.
                //
                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {
                    con.Open();
                    //
                    // Create new SqlCommand object.
                    //
                    using (SqlCommand cmd = new SqlCommand(_spLOGED, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@ExceptionReferenceID", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(ReferenceID, _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionModule", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        cmd.Parameters.Add("@ExceptionPage", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        cmd.Parameters.Add("@ExceptionFromUserID", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        cmd.Parameters.Add("@ExceptionProcedure", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(ExceptionProcedure, _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionOriginalMsg", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(exMessage, _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionStackTrace", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(_PreFixx + exStackTrace, _MAX_LENGHT);

                        cmd.ExecuteNonQuery();//
                        // Invoke ExecuteReader method.
                        // 
                        r = 0;
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
                        el.WriteEventError("Log to ED Failed", 1, ex.Message);

                    }
                }
            }

            return r;
        }



        /// <summary>
        /// Full Log
        /// </summary>
        /// <remarks>Full log - this fills out every thing</remarks>
        public int ExceptionDetailsEX(string ReferenceID, string exMessage, string exStackTrace,
       string FunctionNameOrModule, string ExceptionProcedure, string User_ID, string ExceptionPage)
        {



            int r = -1;


            try
            {

                // Create new SqlConnection object.
                //
                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {
                    con.Open();
                    //
                    // Create new SqlCommand object.
                    //
                    using (SqlCommand cmd = new SqlCommand(_spLOGED, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@ExceptionReferenceID", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(ReferenceID, _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionModule", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(FunctionNameOrModule, _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionPage", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(ExceptionPage, _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionFromUserID", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(User_ID, _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionProcedure", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(ExceptionProcedure, _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionOriginalMsg", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(exMessage, _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionStackTrace", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(_PreFixx + exStackTrace, _MAX_LENGHT);

                        cmd.ExecuteNonQuery();//
                        // Invoke ExecuteReader method.
                        // 
                        r = 0;
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
                        el.WriteEventError("Log to ED Failed", 1, ex.Message);

                    }
                }
            }

            return r;


        }

        /// <summary>
        /// Full Log
        /// </summary>
        /// <remarks>Full log - this fills out every thing</remarks>
        public int ExceptionDetails(string ReferenceID, string exMessage, string exStackTrace,
       string FunctionNameOrModule, string ExceptionProcedure, string User_ID, string ExceptionPage)
        {



            int r = -1;


            try
            {

                // Create new SqlConnection object.
                //
                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {
                    con.Open();
                    //
                    // Create new SqlCommand object.
                    //
                    using (SqlCommand cmd = new SqlCommand(_spLOGED, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@ExceptionReferenceID", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(ReferenceID, _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionModule", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(FunctionNameOrModule, _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionPage", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(ExceptionPage, _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionFromUserID", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(User_ID, _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionProcedure", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(ExceptionProcedure, _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionOriginalMsg", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(exMessage, _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionStackTrace", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(_PreFixx + exStackTrace, _MAX_LENGHT);

                        cmd.ExecuteNonQuery();//
                        // Invoke ExecuteReader method.
                        // 
                        r = 0;
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
                        el.WriteEventError("Log to ED Failed", 1, ex.Message);

                    }
                }
            }

            return r;


        }


        /// <summary>
        ///  ExceptionDetails Generic
        /// </summary>
        /// <remarks>Logs a message</remarks>
        public int ExceptionDetailsMetrics(string _ReferenceID, string _exMessage)
        {



            int r = -1;


            try
            {

                // Create new SqlConnection object.
                //
                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {
                    con.Open();
                    //
                    // Create new SqlCommand object.
                    //
                    using (SqlCommand cmd = new SqlCommand(_spLOGED, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ExceptionReferenceID", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(_ReferenceID, _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionModule", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        cmd.Parameters.Add("@ExceptionPage", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        cmd.Parameters.Add("@ExceptionFromUserID", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        cmd.Parameters.Add("@ExceptionProcedure", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        cmd.Parameters.Add("@ExceptionOriginalMsg", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        cmd.Parameters.Add("@ExceptionStackTrace", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(_PreFixx + _exMessage, 8000);
                        cmd.ExecuteNonQuery();//
                        // Invoke ExecuteReader method.
                        // 
                        r = 0;
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
                        el.WriteEventError("Log to ED Failed", 1, StringEXT.Truncate(_PreFixx + _exMessage, 8000));

                    }
                }
            }
            finally
            {

            }
            //
            return r;



        }


        /// <summary>
        ///  ExceptionDetails Generic
        /// </summary>
        /// <remarks>Logs a message</remarks>
        public int ExceptionDetails(string _ReferenceID, string _exMessage)
        {



            int r = -1;


            try
            {

                // Create new SqlConnection object.
                //
                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {
                    con.Open();
                    //
                    // Create new SqlCommand object.
                    //
                    using (SqlCommand cmd = new SqlCommand(_spLOGED, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ExceptionReferenceID", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(_ReferenceID, _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionModule", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        cmd.Parameters.Add("@ExceptionPage", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        cmd.Parameters.Add("@ExceptionFromUserID", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        cmd.Parameters.Add("@ExceptionProcedure", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        cmd.Parameters.Add("@ExceptionOriginalMsg", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(_exMessage, _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionStackTrace", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(_PreFixx, _MAX_LENGHT);
                        cmd.ExecuteNonQuery();//
                        // Invoke ExecuteReader method.
                        // 
                        r = 0;
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
                        el.WriteEventError("Log to ED Failed", 1, (_PreFixx + _exMessage));

                    }
                }
            }

            return r;



        }


        /// <summary>
        ///  ExceptionDetails Generic with  execption parse
        /// </summary>
        /// <remarks>Logs a message</remarks>
        public int ExceptionDetails(string CONSOLE_NAME, string CLASS_NAME, string FUNCTION_NAME, string MSG, Exception exD)
        {


            int r = -1;
            try
            {

                // Create new SqlConnection object.
                //
                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {
                    con.Open();
                    //
                    // Create new SqlCommand object.
                    //
                    using (SqlCommand cmd = new SqlCommand(_spLOGED, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ExceptionReferenceID", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(MSG, _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionModule", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(CLASS_NAME, _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionPage", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(FUNCTION_NAME, _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionFromUserID", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(CONSOLE_NAME, _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionProcedure", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(Convert.ToString(exD.TargetSite), _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionOriginalMsg", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(_StackTracePreFixx + Convert.ToString(exD.Message), _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionStackTrace", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(Convert.ToString(_PreFixx + exD.StackTrace), _MAX_LENGHT);
                        cmd.ExecuteNonQuery();//
                        // Invoke ExecuteReader method.
                        // 
                        r = 0;
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
                        el.WriteEventError("Log to ED Failed", 1, (_PreFixx + ex.Message));

                    }
                }
            }

            return r;



        }


        /// <summary>
        ///  ExceptionDetails Generic with  execption parse
        /// </summary>
        /// <remarks>Logs a message</remarks>
        public int ExceptionDetails(string _ReferenceID, Exception exD, string Message)
        {

            using (ParseStackTrace p = new ParseStackTrace())
            {
                p.Go(exD);
                _StackTracePreFixx = p.ReturnString;
            }
            int r = -1;
            try
            {

                // Create new SqlConnection object.
                //
                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {
                    con.Open();
                    //
                    // Create new SqlCommand object.
                    //
                    using (SqlCommand cmd = new SqlCommand(_spLOGED, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ExceptionReferenceID", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(_ReferenceID, _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionModule", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        cmd.Parameters.Add("@ExceptionPage", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(Convert.ToString(Message), _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionFromUserID", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        cmd.Parameters.Add("@ExceptionProcedure", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(Convert.ToString(exD.TargetSite), _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionOriginalMsg", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(_StackTracePreFixx + Convert.ToString(exD.Message), _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionStackTrace", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(_PreFixx + Convert.ToString(exD.StackTrace), _MAX_LENGHT);
                        cmd.ExecuteNonQuery();//
                        // Invoke ExecuteReader method.
                        // 
                        r = 0;
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
                        el.WriteEventError("Log to ED Failed", 1, (_PreFixx + ex.Message));

                    }
                }
            }

            return r;



        }




        /// <summary>
        ///  ExceptionDetails Generic
        /// </summary>
        /// <remarks>Logs a message</remarks>
        public int ExceptionDetails(string _ReferenceID, string _ReferenceCode, string _exMessage, int LineNumber)
        {



            int r = -1;


            try
            {

                // Create new SqlConnection object.
                //
                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {
                    con.Open();
                    //
                    // Create new SqlCommand object.
                    //
                    using (SqlCommand cmd = new SqlCommand(_spLOGED, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ExceptionReferenceID", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(_ReferenceID, _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionModule", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(_ReferenceCode, _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionPage", SqlDbType.VarChar, 8000).Value = Convert.ToString(LineNumber);
                        cmd.Parameters.Add("@ExceptionFromUserID", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        cmd.Parameters.Add("@ExceptionProcedure", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        cmd.Parameters.Add("@ExceptionOriginalMsg", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(_exMessage, _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionStackTrace", SqlDbType.VarChar, 8000).Value = _PreFixx;
                        cmd.ExecuteNonQuery();//
                        // Invoke ExecuteReader method.
                        // 
                        r = 0;
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
                        el.WriteEventError("Log to ED Failed", 1, (_PreFixx + ex.Message));

                    }
                }
            }

            return r;



        }


        /// <summary>
        ///  ExceptionDetails Generic
        /// </summary>
        /// <remarks>Logs a message</remarks>
        public int ExceptionDetails(string _ReferenceID, string _ReferenceCode, string _exMessage, int ThreadID, int LineNumber)
        {



            int r = -1;


            try
            {

                // Create new SqlConnection object.
                //
                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {
                    con.Open();
                    //
                    // Create new SqlCommand object.
                    //
                    using (SqlCommand cmd = new SqlCommand(_spLOGED, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ExceptionReferenceID", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(_ReferenceID, _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionModule", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(_ReferenceCode, _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionPage", SqlDbType.VarChar, 8000).Value = Convert.ToString(ThreadID);
                        cmd.Parameters.Add("@ExceptionFromUserID", SqlDbType.VarChar, 8000).Value = Convert.ToString(LineNumber);
                        cmd.Parameters.Add("@ExceptionProcedure", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        cmd.Parameters.Add("@ExceptionOriginalMsg", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(_exMessage, _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionStackTrace", SqlDbType.VarChar, 8000).Value = _PreFixx;
                        cmd.ExecuteNonQuery();//
                        // Invoke ExecuteReader method.
                        // 
                        r = 0;
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
                        el.WriteEventError("Log to ED Failed", 1, (_PreFixx + ex.Message));

                    }
                }
            }

            return r;



        }

        /// <summary>
        ///  System.ArgumentOutOfRangeException Details just pass the execption to it and it does all the rest
        ///  
        /// </summary>
        /// <remarks>Log</remarks>
        public int ExceptionDetails(string _ReferenceID, System.ArgumentOutOfRangeException exORE)
        {
            int r = -1;


            using (ParseStackTrace p = new ParseStackTrace())
            {
                p.Go(exORE);
                _StackTracePreFixx = p.ReturnString;
            }
            
            try
            {

                // Create new SqlConnection object.
                //
                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {
                    con.Open();
                    //
                    // Create new SqlCommand object.
                    //
                    using (SqlCommand cmd = new SqlCommand(_spLOGED, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ExceptionReferenceID", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(_ReferenceID, _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionModule", SqlDbType.VarChar, 8000).Value = Convert.ToString(exORE.TargetSite);
                        cmd.Parameters.Add("@ExceptionPage", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        cmd.Parameters.Add("@ExceptionFromUserID", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(Convert.ToString(exORE.Source), _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionProcedure", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        cmd.Parameters.Add("@ExceptionOriginalMsg", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(_StackTracePreFixx + Convert.ToString(exORE.Message), _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionStackTrace", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(_PreFixx + Convert.ToString(exORE.StackTrace), _MAX_LENGHT);
                        cmd.ExecuteNonQuery();//
                        // Invoke ExecuteReader method.
                        // 
                        r = 0;
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
                        el.WriteEventError("Log to ED Failed", 1, (_PreFixx + ex.Message));

                    }
                }
            }

            return r;



        }




        /// <summary>
        ///  System.TimeoutException Details just pass the execption to it and it does all the rest
        ///  
        /// </summary>
        /// <remarks>Log</remarks>
        public int ExceptionDetails(string _ReferenceID, System.TimeoutException exTO)
        {

            //   Marines#1

            int r = -1;

            using (ParseStackTrace p = new ParseStackTrace())
            {
                p.Go(exTO);
                _StackTracePreFixx = p.ReturnString;
            }


            try
            {

                // Create new SqlConnection object.
                //
                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {
                    con.Open();
                    //
                    // Create new SqlCommand object.
                    //
                    using (SqlCommand cmd = new SqlCommand(_spLOGED, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ExceptionReferenceID", SqlDbType.VarChar, 8000).Value = _ReferenceID;
                        cmd.Parameters.Add("@ExceptionModule", SqlDbType.VarChar, 8000).Value = Convert.ToString(exTO.TargetSite);
                        cmd.Parameters.Add("@ExceptionPage", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        cmd.Parameters.Add("@ExceptionFromUserID", SqlDbType.VarChar, 8000).Value = Convert.ToString(exTO.Source);
                        cmd.Parameters.Add("@ExceptionProcedure", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        cmd.Parameters.Add("@ExceptionOriginalMsg", SqlDbType.VarChar, 8000).Value = Convert.ToString(_StackTracePreFixx + exTO.Message);
                        cmd.Parameters.Add("@ExceptionStackTrace", SqlDbType.VarChar, 8000).Value = _PreFixx + Convert.ToString(exTO.StackTrace);
                        cmd.ExecuteNonQuery();//
                        // Invoke ExecuteReader method.
                        // 
                        r = 0;
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
                        el.WriteEventError("Log to ED Failed", 1, (_PreFixx + ex.Message));

                    }
                }
            }

            return r;

        }

        /// <summary>
        ///  SQL Exception Details just pass the execption to it and it does all the rest
        ///  
        /// </summary>
        /// <remarks>Log</remarks>
        public int ExceptionDetails(string _ReferenceID, SqlException sqlex)
        {


            int r = -1;


            using (ParseStackTrace p = new ParseStackTrace())
            {
                p.Go(sqlex);
                _StackTracePreFixx = p.ReturnString;
            }

            try
            {

                // Create new SqlConnection object.
                //
                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {
                    con.Open();
                    //
                    // Create new SqlCommand object.
                    //
                    using (SqlCommand cmd = new SqlCommand(_spLOGED, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ExceptionReferenceID", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(_ReferenceID, _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionModule", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(Convert.ToString(sqlex.Procedure), _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionPage", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(Convert.ToString(sqlex.LineNumber), _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionFromUserID", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(Convert.ToString(sqlex.Source), _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionProcedure", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(Convert.ToString(sqlex.Procedure), _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionOriginalMsg", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(_StackTracePreFixx + Convert.ToString(sqlex.Message), _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionStackTrace", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(_PreFixx + Convert.ToString(sqlex.StackTrace), _MAX_LENGHT);
                        cmd.ExecuteNonQuery();//
                        // Invoke ExecuteReader method.
                        // 
                        r = 0;
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
                        el.WriteEventError("Log to ED Failed", 1, (_PreFixx + ex.Message));

                    }
                }
            }
            return r;

        }




        /// <summary>
        ///  ExceptionDetails just pass the execption to it and it does all the rest
        ///  
        /// </summary>
        /// <remarks>Log</remarks>
        public int ExceptionDetails(string _ReferenceID, Exception exD)
        {
            using (ParseStackTrace p = new ParseStackTrace())
            {
                p.Go(exD);
                _StackTracePreFixx = p.ReturnString;
            }

            int r = -1;
            try
            {

                // Create new SqlConnection object.
                //
                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {
                    con.Open();
                    //
                    // Create new SqlCommand object.
                    //
                    using (SqlCommand cmd = new SqlCommand(_spLOGED, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ExceptionReferenceID", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(_ReferenceID, _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionModule", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        cmd.Parameters.Add("@ExceptionPage", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        cmd.Parameters.Add("@ExceptionFromUserID", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        cmd.Parameters.Add("@ExceptionProcedure", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(Convert.ToString(exD.TargetSite), _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionOriginalMsg", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(_StackTracePreFixx + Convert.ToString(exD.Message), _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionStackTrace", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(_PreFixx + Convert.ToString(exD.StackTrace), _MAX_LENGHT);
                        cmd.ExecuteNonQuery();//
                        // Invoke ExecuteReader method.
                        // 
                        r = 0;
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
                        el.WriteEventError("Log to ED Failed", 1, (_PreFixx + ex.Message));

                    }
                }
            }
            return r;

        }







        /// <summary>
        ///  System.IO.DirectoryNotFoundException just pass the execption to it and it does all the rest
        ///  
        /// </summary>
        /// <remarks>Log</remarks>
        public int ExceptionDetails(string _ReferenceID, System.IO.DirectoryNotFoundException exDNF)
        {

            using (ParseStackTrace p = new ParseStackTrace())
            {
                p.Go(exDNF);
                _StackTracePreFixx = p.ReturnString;
            }

            int r = -1;
            try
            {

                // Create new SqlConnection object.
                //
                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {
                    con.Open();
                    //
                    // Create new SqlCommand object.
                    //
                    using (SqlCommand cmd = new SqlCommand(_spLOGED, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ExceptionReferenceID", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(_ReferenceID, _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionModule", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        cmd.Parameters.Add("@ExceptionPage", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        cmd.Parameters.Add("@ExceptionFromUserID", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        cmd.Parameters.Add("@ExceptionProcedure", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(Convert.ToString(exDNF.TargetSite), _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionOriginalMsg", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(_StackTracePreFixx + Convert.ToString(exDNF.Message), _MAX_LENGHT);
                        cmd.Parameters.Add("@ExceptionStackTrace", SqlDbType.VarChar, 8000).Value = StringEXT.Truncate(_PreFixx + Convert.ToString(exDNF.StackTrace), _MAX_LENGHT);
                        cmd.ExecuteNonQuery();//
                        // Invoke ExecuteReader method.
                        // 
                        r = 0;
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
                        el.WriteEventError("Log to ED Failed", 1, (_PreFixx + ex.Message));

                    }
                }

            }

            return r;

        }












        /// <summary>
        ///  System.IO.FileNotFoundException just pass the execption to it and it does all the rest
        ///  
        /// </summary>
        /// <remarks>Log</remarks>
        public int ExceptionDetails(string _ReferenceID, System.IO.FileNotFoundException exFNF)
        {


            int r = -1;

            using (ParseStackTrace p = new ParseStackTrace())
            {
                p.Go(exFNF);
                _StackTracePreFixx = p.ReturnString;
            }


            try
            {

                // Create new SqlConnection object.
                //
                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {
                    con.Open();
                    //
                    // Create new SqlCommand object.
                    //
                    using (SqlCommand cmd = new SqlCommand(_spLOGED, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ExceptionReferenceID", SqlDbType.VarChar, 8000).Value = _ReferenceID;
                        cmd.Parameters.Add("@ExceptionModule", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        cmd.Parameters.Add("@ExceptionPage", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        cmd.Parameters.Add("@ExceptionFromUserID", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        cmd.Parameters.Add("@ExceptionProcedure", SqlDbType.VarChar, 8000).Value = Convert.ToString(exFNF.TargetSite);
                        cmd.Parameters.Add("@ExceptionOriginalMsg", SqlDbType.VarChar, 8000).Value = _StackTracePreFixx + Convert.ToString(exFNF.Message);
                        cmd.Parameters.Add("@ExceptionStackTrace", SqlDbType.VarChar, 8000).Value = Convert.ToString(_PreFixx + exFNF.StackTrace);
                        cmd.ExecuteNonQuery();//
                        // Invoke ExecuteReader method.
                        // 
                        r = 0;
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
                        el.WriteEventError("Log to ED Failed", 1, (_PreFixx + ex.Message));

                    }
                }
            }
            return r;
        }




        /// <summary>
        ///  System.IO.FileLoadException just pass the execption to it and it does all the rest
        ///  
        /// </summary>
        /// <remarks>Log</remarks>
        public int ExceptionDetails(string _ReferenceID, System.IO.FileLoadException exFLE)
        {

            int r = -1;


            using (ParseStackTrace p = new ParseStackTrace())
            {
                p.Go(exFLE);
                _StackTracePreFixx = p.ReturnString;
            }

            try
            {

                // Create new SqlConnection object.
                //
                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {
                    con.Open();
                    //
                    // Create new SqlCommand object.
                    //
                    using (SqlCommand cmd = new SqlCommand(_spLOGED, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ExceptionReferenceID", SqlDbType.VarChar, 8000).Value = _ReferenceID;
                        cmd.Parameters.Add("@ExceptionModule", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        cmd.Parameters.Add("@ExceptionPage", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        cmd.Parameters.Add("@ExceptionFromUserID", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        cmd.Parameters.Add("@ExceptionProcedure", SqlDbType.VarChar, 8000).Value = Convert.ToString(exFLE.TargetSite);
                        cmd.Parameters.Add("@ExceptionOriginalMsg", SqlDbType.VarChar, 8000).Value = _StackTracePreFixx + Convert.ToString(exFLE.Message);
                        cmd.Parameters.Add("@ExceptionStackTrace", SqlDbType.VarChar, 8000).Value = _PreFixx + Convert.ToString(exFLE.StackTrace);
                        cmd.ExecuteNonQuery();//
                        // Invoke ExecuteReader method.
                        // 
                        r = 0;
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
                        el.WriteEventError("Log to ED Failed", 1, (_PreFixx + ex.Message));

                    }
                }
            }
            return r;

        }




        /// <summary>
        ///  System.IO.PathTooLongException just pass the execption to it and it does all the rest
        ///  
        /// </summary>
        /// <remarks>Log</remarks>
        public int ExceptionDetails(string _ReferenceID, System.IO.PathTooLongException exPTL)
        {



            int r = -1;

            using (ParseStackTrace p = new ParseStackTrace())
            {
                p.Go(exPTL);
                _StackTracePreFixx = p.ReturnString;
            }

            try
            {

                // Create new SqlConnection object.
                //
                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {
                    con.Open();
                    //
                    // Create new SqlCommand object.
                    //
                    using (SqlCommand cmd = new SqlCommand(_spLOGED, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ExceptionReferenceID", SqlDbType.VarChar, 8000).Value = _ReferenceID;
                        cmd.Parameters.Add("@ExceptionModule", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        cmd.Parameters.Add("@ExceptionPage", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        cmd.Parameters.Add("@ExceptionFromUserID", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        cmd.Parameters.Add("@ExceptionProcedure", SqlDbType.VarChar, 8000).Value = Convert.ToString(exPTL.TargetSite);
                        cmd.Parameters.Add("@ExceptionOriginalMsg", SqlDbType.VarChar, 8000).Value = _StackTracePreFixx + Convert.ToString(exPTL.Message);
                        cmd.Parameters.Add("@ExceptionStackTrace", SqlDbType.VarChar, 8000).Value = _PreFixx + Convert.ToString(exPTL.StackTrace);
                        cmd.ExecuteNonQuery();//
                        // Invoke ExecuteReader method.
                        // 
                        r = 0;
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
                        el.WriteEventError("Log to ED Failed", 1, (_PreFixx + ex.Message));

                    }
                }
            }
            return r;
        }





        /// <summary>
        ///  System.IO.DriveNotFoundException just pass the execption to it and it does all the rest
        ///  
        /// </summary>
        /// <remarks>Log</remarks>
        public int ExceptionDetails(string _ReferenceID, System.IO.DriveNotFoundException exDNF)
        {


            int r = -1;

            using (ParseStackTrace p = new ParseStackTrace())
            {
                p.Go(exDNF);
                _StackTracePreFixx = p.ReturnString;
            }

            try
            {

                // Create new SqlConnection object.
                //
                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {
                    con.Open();
                    //
                    // Create new SqlCommand object.
                    //
                    using (SqlCommand cmd = new SqlCommand(_spLOGED, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ExceptionReferenceID", SqlDbType.VarChar, 8000).Value = _ReferenceID;
                        cmd.Parameters.Add("@ExceptionModule", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        cmd.Parameters.Add("@ExceptionPage", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        cmd.Parameters.Add("@ExceptionFromUserID", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        cmd.Parameters.Add("@ExceptionProcedure", SqlDbType.VarChar, 8000).Value = Convert.ToString(exDNF.TargetSite);
                        cmd.Parameters.Add("@ExceptionOriginalMsg", SqlDbType.VarChar, 8000).Value = _StackTracePreFixx + Convert.ToString(exDNF.Message);
                        cmd.Parameters.Add("@ExceptionStackTrace", SqlDbType.VarChar, 8000).Value = _PreFixx + Convert.ToString(exDNF.StackTrace);
                        cmd.ExecuteNonQuery();//
                        // Invoke ExecuteReader method.
                        // 
                        r = 0;
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
                        el.WriteEventError("Log to ED Failed", 1, (_PreFixx + ex.Message));

                    }
                }

            }
            finally
            {

            }


            return r;

        }


        /// <summary>
        /// Full Log
        /// </summary>
        /// <remarks>Full log - this fills out every thing</remarks>
        public int MLOG(string VendorName, string VendorError, string REQID, string stime, string rtime, string REQBH03, string RESBH03, string SP, string RP, string RES, string REQ)
        {

            string MLOG_SPL = "usp_insert_into_mlog";

            int r = -1;

            if (_MLOG)
            {
                try
                {


                    // Create new SqlConnection object.
                    //
                    using (SqlConnection con = new SqlConnection(_ConnectionString))
                    {
                        con.Open();
                        //
                        // Create new SqlCommand object.
                        //
                        using (SqlCommand cmd = new SqlCommand(MLOG_SPL, con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@VendorName", SqlDbType.VarChar, 50).Value = VendorName;
                            cmd.Parameters.Add("@VendorError", SqlDbType.VarChar, 255).Value = VendorError;
                            cmd.Parameters.Add("@REQID", SqlDbType.BigInt).Value = Convert.ToInt32(REQID);
                            cmd.Parameters.Add("@Stime", SqlDbType.VarChar, 50).Value = stime;
                            cmd.Parameters.Add("@Rtime", SqlDbType.VarChar, 50).Value = rtime;
                            cmd.Parameters.Add("@REQBH03 ", SqlDbType.VarChar, 50).Value = REQBH03;
                            cmd.Parameters.Add("@RESBH03 ", SqlDbType.VarChar, 50).Value = RESBH03;
                            cmd.Parameters.Add("@SP", SqlDbType.VarChar, 50).Value = SP; //Convert.ToString(exDNF.TargetSite);
                            cmd.Parameters.Add("@RP", SqlDbType.VarChar, 50).Value = RP;  //Convert.ToString(exDNF.Message);
                            cmd.Parameters.Add("@REQ", SqlDbType.VarChar).Value = _PreFixx + REQ; // Convert.ToString(exDNF.StackTrace);
                            cmd.Parameters.Add("@RES", SqlDbType.VarChar).Value = _PreFixx + RES; // Convert.ToString(exDNF.StackTrace);

                            cmd.Parameters.Add("@MLOG_ID", SqlDbType.BigInt, 1);
                            cmd.Parameters["@MLOG_ID"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();//
                            // Invoke ExecuteReader method.
                            // 
                            r = Convert.ToInt32(cmd.Parameters["@MLOG_ID"].Value.ToString());
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
                            el.WriteEventError(_PreFixx + "Log to MLOG Failed", 1, (_PreFixx + ex.Message));

                        }
                    }

                }
            }
            return r;


        }


        public static int LineNumber(Exception e)
        {

            int linenum = -1;

            try
            {

                linenum = Convert.ToInt32(e.StackTrace.Substring(e.StackTrace.LastIndexOf(":line") + 5));

            }

            catch
            {

                //Stack trace is not available!

            }

            return linenum;

        }




        public int LogToITS()
        {




            return 0;
        }




        private string Replace(string sExMsg, string p1, string p2)
        {

            return "";
            // throw new NotImplementedException();
        }
    }
}
