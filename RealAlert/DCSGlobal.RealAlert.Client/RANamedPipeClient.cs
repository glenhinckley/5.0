using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Pipes;
using System.IO;
using System.Threading;
using System.Data;
using DCSGlobal.BusinessRules.Logging;
using DCSGlobal.Comunications.IPC.NetPipes;
using System.Security.Principal;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;



using System.Diagnostics;  //  For the StackTrace.


namespace DCSGlobal.RealAlert.Client
{

    public class RANamedPipeClient
    {


        private static logExecption log = new logExecption();
        private static string _AppName;
        private static string ClientCode = string.Empty;
        private static string _ConnectionString = string.Empty;
        private static string _InstanceName = string.Empty;
        private static string _operator_id = string.Empty;


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

        public string GetRowData()
        {

            string s = string.Empty;
            string sss = string.Empty;
            sss = "UNK";

            NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", _InstanceName, PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.Impersonation);

            //' Give the server process some time to return results before going on.
            Thread.Sleep(100);

            // start a watch dog timer to kill the the read thread if it has not returned in time set

            //asign ss.rad string to a thread

            try
            {
                pipeClient.Connect(100);
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
