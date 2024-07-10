using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
//using DCSGlobal.BusinessRules.CoreLibrary;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using System.Data.SqlClient;
using System.IO;
using DCSGlobal.BusinessRules.CoreLibraryII;

namespace DCSGlobal.Rules.DLLBuilder
{
    public class LogFailedRule : IDisposable
    {


        StringStuff objSS = new StringStuff();

        // StringExt t = new StringExt();

        string _ConnectionString = string.Empty;
        //  string _spLOGED = "usp_insert_exceptions_details";


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

        ~LogFailedRule()
        {
            Dispose(false);
        }


        public LogFailedRule()
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





        /// <summary>
        /// Quick Log ex.message, ex.messages0urce , Function Name
        /// </summary>
        /// <remarks>Quick log</remarks>
        public int Insert(int RuleID, string Parse, string Build, string CompilerResults, DateTime RuleModified, DateTime RuleBuilt)
        {

            int r = -1;


            string sql = string.Empty;


            sql = "insert into rules_failed_vb  ";
            sql = sql + "( ";
            sql = sql + "RuleID, ";
            sql = sql + "VBParse, ";
            sql = sql + "VBBuild, ";
            sql = sql + "VBCompilerResults, ";
            sql = sql + "modified_by, ";
            sql = sql + "RuleBuilt, ";
            sql = sql + "PreFix ";
            sql = sql + ") values ";

            sql = sql + "( ";
            sql = sql + " " + RuleID + ",";
            sql = sql + " '" + Parse + "',";
            sql = sql + " '" + Build + "',";
            sql = sql + " '" + CompilerResults + "',";
            sql = sql + " '" + RuleModified.ToString()  + "',";
            sql = sql + " '" + RuleBuilt.ToString() + "',";
            sql = sql + "''";
            sql = sql + ") ";

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
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();//
                        r = 0;
                    }
                    con.Close();
                }

            }
            catch
            {


            }


            return r;

        }
    }
}
