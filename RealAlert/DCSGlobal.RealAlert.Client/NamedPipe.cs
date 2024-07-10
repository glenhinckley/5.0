using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Pipes;
using System.IO;
using System.Threading;
using System.Data;
using System.Data;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Data.SqlClient;



using DCSGlobal.BusinessRules.Logging;
using DCSGlobal.BusinessRules.CoreLibraryII;
using System.Security.Principal;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;



using System.Diagnostics;  //  For the StackTrace.


namespace DCSGlobal.RealAlert.Client
{

    public class NamedPipe
    {


        private static logExecption log = new logExecption();
        private static string _AppName;
        private static string ClientCode = string.Empty;
        private static string _ConnectionString = string.Empty;
        private static string _InstanceName = string.Empty;
        private static string _operator_id = string.Empty;
        private static string _ServerName = ".";

        public string ConnectionString
        {
            set
            {
                _ConnectionString = value;
                log.ConnectionString = value;
            }
        }

        public string InstanceName
        {
            set
            {
                _InstanceName = value;
            }
        }

        public string operator_id
        {
            set
            {
                _operator_id = value;
            }
        }

        public string ServerName
        {
            set
            {
                _ServerName = value;
            }
        }



        

        /// <summary>
        /// builds a memory stream
        /// </summary>
        /// <returns></returns>
        static MemoryStream BuildStream()
        {

            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            //writer.Write((System.Text.Encoding.UTF8.GetBytes(GetNextERBID())));
            //   writer.Write(GetNextERBID());

            writer.Flush();
            stream.Position = 0;



            return stream;

        }


        public int Login(string USER_ID, string HospCode)
        {


            int r = -1;

            try
            {
                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {





                    using (SqlCommand cmd = new SqlCommand("USP_REAL_ALERT_INITIATE", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@USER_ID", USER_ID);
                        cmd.Parameters.AddWithValue("@HOSP_CODE", HospCode);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        r = 0;
                    }
                }


            }
            catch (Exception ex)
            {
                log.ExceptionDetails("RAC", ex);
                r = 1;
            }

            return r;



        }



        public int KillList(string TheString)
        {
            // ALTER PROCEDURE [dbo].[usp_real_alert_50_client] (@INPUT VARCHAR(1000))

            int r = -1;

            try
            {
                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {





                    using (SqlCommand cmd = new SqlCommand("usp_real_alert_50_client", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@INPUT", TheString);
                 
                        con.Open();
                        cmd.ExecuteNonQuery();
                        r = 0;
                    }
                }


            }
            catch (Exception ex)
            {
                log.ExceptionDetails("RAC", ex);
                r = 1;
            }

            return r;



        }








        public string GetRowData()
        {

            string s = string.Empty;
            string sss = string.Empty;
            sss = "UNK";

            NamedPipeClientStream pipeClient = new NamedPipeClientStream(_ServerName, _InstanceName, PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.Impersonation);

            //' Give the server process some time to return results before going on.
            Thread.Sleep(100);

            // start a watch dog timer to kill the the read thread if it has not returned in time set

            //asign ss.rad string to a thread

            try
            {
                pipeClient.Connect(1000);
                StreamString ss = new StreamString(pipeClient);
                ss.WriteString(_operator_id);
                sss = ss.ReadString();
            }

            catch (System.ArgumentOutOfRangeException ex)
            {
                log.ExceptionDetails("RANamedPipeClient " + _InstanceName + " " + _operator_id, ex);

                sss = "BND " + Convert.ToString(ex.Message);
            }

            catch (System.TimeoutException ex)
            {
                log.ExceptionDetails("RANamedPipeClient " + _InstanceName + " " + _operator_id, ex);

                sss = "TMO " + Convert.ToString(ex.Message);
            }
            catch (Exception ex)
            {
                log.ExceptionDetails("RANamedPipeClient " + _InstanceName + " " + _operator_id, ex);

                sss = "ERR " + Convert.ToString(ex.Message);
            }

            string ret = string.Empty;

            string OriginalMSG = string.Empty;

            ret = sss.Substring(0, Math.Min(sss.Length, 3));

            OriginalMSG = sss;


            switch (ret)
            {
                case "WAT":

                    s = "WAT";

                    break;

                case "ONF":

                    s = "ONF";

                    break;


                case "TMO":

                    s = "TMO";

                    break;

                case "BND":

                    s = "BND";

                    break;

                case "NDD":

                    s = "NDD";

                    break;

                case "RES":

                    s = sss;

                    break;

                case "ERR":

                    s = sss;

                    break;


                default:

                    s = "UNK :" + OriginalMSG;
                    // s = sss;
                    break;       // break necessary on default
            }


            pipeClient.Close();
            return s;

        }
    }
}
