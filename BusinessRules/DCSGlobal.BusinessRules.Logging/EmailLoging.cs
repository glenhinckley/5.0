using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Configuration;
using System.Data;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using System.Data.SqlClient;

namespace DCSGlobal.BusinessRules.Logging
{
    public class EmailLoging : IDisposable
    {


        SqlConnection CON = new SqlConnection();
        private string _ConnectionString = "";

        public string ConnectionString
        {

            set
            {

                _ConnectionString = value;
            }
        }

        ~EmailLoging()
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
                // free other managed objects that implement
                // IDisposable only
            }

            // release any unmanaged objects
            // set the object references to null

            _disposed = true;
        }
        public int InsertEmail(string ReferenceID, string exMessage, string exStackTrace, string ExceptionProcedure)
        {
            string sqlQuery = string.Empty;

            int r = -1;
            sqlQuery = "usp_EMAIL_INSERT_MESSAGE_BY_USER";

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
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@MESSAGE_UID", SqlDbType.VarChar).Value = ReferenceID;
                        cmd.Parameters.Add("@MESSAGE_SUBJECT", SqlDbType.VarChar).Value = DBNull.Value;
                        cmd.Parameters.Add("@MESSAGE_BODY", SqlDbType.VarChar).Value = DBNull.Value;
                        cmd.Parameters.Add("@RAW_MESSAGE", SqlDbType.Text).Value = DBNull.Value;
                        cmd.Parameters.Add("@EMAIL_USER_ID", SqlDbType.Int).Value = ExceptionProcedure;
                        cmd.ExecuteNonQuery();

                    }
                }

            }
            catch (Exception ex)
            {
                using (LogToFile lf = new LogToFile())
                {
                    lf.LogFile("InsertEmail", exMessage, ExceptionProcedure, 1, ex.Message);
                }
            }

            return r;
        }


        public string GetLastUID(int EmailUserID)
        {


            string sqlQuery = string.Empty;

              string r = string.Empty;
            sqlQuery = "usp_EMAIL_INSERT_MESSAGE_BY_USER";

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
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@EMAIL_USER_ID", SqlDbType.Int).Value = EmailUserID;
                        cmd.Parameters.Add("@LAST_UID", SqlDbType.VarChar, 4000);

                        cmd.Parameters["@LAST_UID"].Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                        r = (string)cmd.Parameters["@LAST_UID"].Value;
                    }
                }

            }
            catch (Exception ex)
            {
                using (LogToFile lf = new LogToFile())
                {
                    lf.LogFile("InsertEmail", "", "", EmailUserID, ex.Message);
                }
            }

            
            return r;
        }


    }
}
