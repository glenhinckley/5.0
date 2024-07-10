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
    public class PerformanceAudit : IDisposable
    {








        ~PerformanceAudit()
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
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        static string sConnectionString = "";

        public bool isInitialized;
        public PerformanceAudit()
        {
            isInitialized = true;
        }


        public PerformanceAudit(string ConncectionString)
        {

            sConnectionString = ConncectionString;
            isInitialized = true;
        }

        
        public string ConnectionString
        {

            set
            {

                sConnectionString = value;
            }
        }

        StringStuff objSS = new StringStuff();
        SqlConnection CON = new SqlConnection();
        logExecption log = new logExecption();

        public int HIPAAPerformanceAudit(int BatchID, int EBR_ID, DateTime ts1, DateTime ts2, DateTime ts3, DateTime ts4)
        {

            log.ConnectionString = sConnectionString;

            int r = -1;
            try
            {

              string input = String.Format("<perf><BATCH_ID>{0}</BATCH_ID><EBR_ID>{1}</EBR_ID><REQUEST_SENT_TS>{2}</REQUEST_SENT_TS><RESPONSE_RECEIVE_TS>{3}</RESPONSE_RECEIVE_TS><PARSE_BEGIN_TS>{4}</PARSE_BEGIN_TS><PARSE_END_TS>{5}</PARSE_END_TS></perf>", BatchID.ToString(), EBR_ID.ToString(), ts1.ToString("yyyy-MM-dd HH:mm:ss:fff"), ts2.ToString("yyyy-MM-dd HH:mm:ss:fff"), ts3.ToString("yyyy-MM-dd HH:mm:ss:fff"), ts4.ToString("yyyy-MM-dd HH:mm:ss:fff"));

                string sqlQuery;

                sqlQuery = "USP_INSERT_HIPAA_PERFORMANCE_AUDIT_v2";

                CON.ConnectionString = sConnectionString;
                SqlCommand cmd = new SqlCommand(sqlQuery, CON);
                cmd.CommandType = CommandType.StoredProcedure;
                CON.Open();
                cmd.Parameters.Add("@BATCH_ID", SqlDbType.Int).Value = BatchID;
                cmd.Parameters.Add("@XML", SqlDbType.Xml).Value = input;
 
                cmd.ExecuteNonQuery();

                r = 0;
            }
            catch(Exception ex)
            {

                log.ExceptionDetails("PerformanceAudit", ex);
                r = -1;
            }

            finally
            {

                CON.Close();

            }
            return r;
        }

    }
}
