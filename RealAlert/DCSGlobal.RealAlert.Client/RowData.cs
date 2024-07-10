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

using System.Data.Sql;
using System.Data.SqlClient;

//using TXLLabs.SuperSub.StringHandlingStuff;



using System.Diagnostics;  //  For the StackTrace.


namespace DCSGlobal.RealAlert.Client
{


    public enum ConnectionMethods
    {
        NamedPipe,
        TCP,
        WCF,
        ForceDirect

    }

    public class RowData : IDisposable
    {


        ~RowData()
        {
            Dispose(false);
        }


        public void Dispose()
        {
            //  Console.WriteLine("Dispose called from outside");
            // If this function is being called the user wants to release the
            // resources. lets call the Dispose which will do this for us.
            Dispose(true);

            // Now since we have done the cleanup already there is nothing left
            // for the Finalizer to do. So lets tell the GC not to call it later.
            GC.SuppressFinalize(this);
        }



        protected virtual void Dispose(bool disposing)
        {
            // Console.WriteLine("Actual Dispose called with a " + disposing.ToString());
            if (disposing == true)
            {
                //someone want the deterministic release of all resources
                //Let us release all the managed resources
                ReleaseManagedResources();
            }
            else
            {
                // Do nothing, no one asked a dispose, the object went out of
                // scope and finalized is called so lets next round of GC 
                // release these resources
            }

            // Release the unmanaged resource in any case as they will not be 
            // released by GC
            ReleaseUnmangedResources();

        }

        void ReleaseManagedResources()
        {
            //      Console.WriteLine("Releasing Managed Resources");
            //if (tr != null)
            {
                // tr.Dispose();
            }
        }

        void ReleaseUnmangedResources()
        {
            // Console.WriteLine("Releasing Unmanaged Resources");
        }


        private static logExecption log = new logExecption();

        private static string _ClientCode = string.Empty;
        private static string _ConnectionString = string.Empty;
        private static string _InstanceName = string.Empty;
        private static string _operator_id = string.Empty;
        private static string _RealAlertServerInstance = string.Empty;
        private static string _ConnectionMethod = "NamedPipe";
        private static string _SPName = string.Empty;

        private static bool _ForceDirect = false;

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

        public string RealAlertServerInstance
        {
            set
            {
                _RealAlertServerInstance = value;

            }

        }

        public string SPName
        {
            set
            {
                _SPName = value;

            }

        }



        public ConnectionMethods ConnectionMethod
        {
            set
            {
                _ConnectionMethod = value.ToString();
            }
        }



        public bool ForceDirect
        {
            set
            {
                _ForceDirect = value;
            }
        }


        public string GetRowData()
        {

            string r = string.Empty;

            switch (_ConnectionMethod)
            {
                case "NamedPipe":

                    r = GetRowDataNamedPipe();

                    break;


                case "TCP":

                    r = "NA";

                    break;


                case "SignalR":

                    r = "NA";

                    break;



                case "ForceDirect":

                    r = GetRowDataDirect();

                    break;


                default:

                    ;
                    // s = sss;
                    break;       // break necessary on default
            }


            return r;

        }


        private string GetRowDataNamedPipe()
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


        private string GetRowDataDirect()
        {

            string r = string.Empty;

            r = "ND";
            try
            {
                using (SqlConnection con = new SqlConnection(_ConnectionString))
                {

                    using (SqlCommand cmd = new SqlCommand(_SPName, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@client_code", _ClientCode);
                        con.Open();
                        using (SqlDataReader idr = cmd.ExecuteReader())
                        {
                            if (idr.HasRows)
                            {
                                r = string.Empty;
                                int x = 0;

                                while (idr.Read())
                                {


                                    try
                                    {

                                        if (idr["PATIENT_NUMBER"] != System.DBNull.Value)
                                        {
                                            r = r + Convert.ToString(idr["PATIENT_NUMBER"]) + "|";
                                        }
                                        else
                                        {
                                            // objEligibility.RES_PAYER_NAME = NullText;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //  objEligibility.RES_PAYER_NAME = "Field Not found";


                                        log.ExceptionDetails((Convert.ToString(_InstanceName) + " PATIENT_NUMBER " + _ClientCode), ex);

                                    }





                                    try
                                    {



                                        if (idr["FACILITY_CODE"] != System.DBNull.Value)
                                        {
                                            r = r + (string)idr["FACILITY_CODE"] + "|";
                                        }
                                        else
                                        {
                                            // objEligibility.RES_PAYER_NAME = NullText;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //  objEligibility.RES_PAYER_NAME = "Field Not found";


                                        log.ExceptionDetails((Convert.ToString(_InstanceName) + " FACILITY_CODE " + _ClientCode), ex);


                                    }



                                    try
                                    {



                                        if (idr["PID"] != System.DBNull.Value)
                                        {
                                            r = r + (Int32)idr["PID"] + "|";
                                        }
                                        else
                                        {
                                            // objEligibility.RES_PAYER_NAME = NullText;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //  objEligibility.RES_PAYER_NAME = "Field Not found";


                                        log.ExceptionDetails((Convert.ToString(_InstanceName) + " PID " + _ClientCode), ex);


                                    }



                                    try
                                    {



                                        if (idr["DT"] != System.DBNull.Value)
                                        {
                                            r = r + (DateTime)idr["DT"] + "|";
                                        }
                                        else
                                        {
                                            // objEligibility.RES_PAYER_NAME = NullText;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //  objEligibility.RES_PAYER_NAME = "Field Not found";


                                        log.ExceptionDetails((Convert.ToString(_InstanceName) + " DT " + _ClientCode), ex);


                                    }



                                    try
                                    {



                                        if (idr["PAT_HOSP_CODE"] != System.DBNull.Value)
                                        {
                                            r = r + (string)idr["PAT_HOSP_CODE"] + "|";
                                        }
                                        else
                                        {
                                            // objEligibility.RES_PAYER_NAME = NullText;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //  objEligibility.RES_PAYER_NAME = "Field Not found";


                                        log.ExceptionDetails((Convert.ToString(_InstanceName) + " PAT_HOSP_CODE " + _ClientCode), ex);


                                    }



                                    try
                                    {



                                        if (idr["PATNAME"] != System.DBNull.Value)
                                        {
                                            r = r + (string)idr["PATNAME"] + "|";
                                        }
                                        else
                                        {
                                            // objEligibility.RES_PAYER_NAME = NullText;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //  objEligibility.RES_PAYER_NAME = "Field Not found";


                                        log.ExceptionDetails((Convert.ToString(_InstanceName) + " PATNAME " + _ClientCode), ex);


                                    }





                                    try
                                    {



                                        if (idr["ID"] != System.DBNull.Value)
                                        {
                                            r = r + (Int32)idr["Id"] + "|";
                                        }
                                        else
                                        {
                                            // objEligibility.RES_PAYER_NAME = NullText;
                                        }
                                    }

                                    catch (Exception ex)
                                    {
                                        //  objEligibility.RES_PAYER_NAME = "Field Not found";


                                        log.ExceptionDetails((Convert.ToString(_InstanceName) + " ID " + _ClientCode), ex);


                                    }


                                    try
                                    {



                                        if (idr["OPERATOR_ID"] != System.DBNull.Value)
                                        {
                                            r = r + (string)idr["OPERATOR_ID"] + "|";
                                        }
                                        else
                                        {
                                            // objEligibility.RES_PAYER_NAME = NullText;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //  objEligibility.RES_PAYER_NAME = "Field Not found";


                                        log.ExceptionDetails((Convert.ToString(_InstanceName) + " OPERATOR_ID " + _ClientCode), ex);


                                    }





                                    x++;
                                    if (x > 2000)
                                        break;
                                }
                            }


                        }
                        con.Close();

                    }




                }
                r = r + "^";

            }
            catch (SqlException sql)
            {
                log.ExceptionDetails((Convert.ToString(_InstanceName) + " db SQL error " + _ClientCode), sql);


            }

            catch (Exception ex)
            {
                log.ExceptionDetails((Convert.ToString(_InstanceName) + " db Execption error " + _ClientCode), ex);


            }

            return r;


        }

    }






}
