using System;
using System.Collections.Generic;
using System.Linq;

using System.Data;
using System.Data.SqlClient;



using DCSGlobal.BusinessRules.CoreLibrary;
using DCSGlobal.BusinessRules.Security;
using DCSGlobal.BusinessRules.Logging;



namespace DCSGlobal.BusinessRules.Security
{
    private class User : IDisposable
    {

        private static logExecption log = new logExecption();




        private static string _ConnectionString = string.Empty;
        private static string _CommandString = string.Empty;

        bool _disposed;
        private string _EndPoint = string.Empty;
        private bool _UseWSDL = false;

        ~User()
        {
            Dispose(false);
        }

        public string ConnectionString
        {

            set
            {

                _ConnectionString = value;
            }
        }


        public string CommandString
        {

            set
            {

                _CommandString = value;
            }
        }


        public bool UseWSDL
        {

            set
            {
                _UseWSDL = value;
            }
        }

        public string EndPoint
        {

            set
            {
                _EndPoint = value;
            }
        }

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
                log.Dispose();

                // free other managed objects that implement
                // IDisposable only
            }



            // release any unmanaged objects
            // set the object references to null

            _disposed = true;
        }


        public int checkValidUserAl(string UserName, string Passwd, string DebugValue, string ID)
        {


            int r = -1;



            // string r = string.Empty;
            string strError = string.Empty;
            string strActive = string.Empty;
          //  string strPwd = string.Empty;
            string strPwdStatus = string.Empty;
            string strLastAccess = string.Empty;
            string strPast1 = string.Empty;
            string strPast2 = string.Empty;
            string strPast3 = string.Empty;
            string strPast4 = string.Empty;
            string strPast5 = string.Empty;
            string strPast6 = string.Empty;

            DateTime dtLastAccess;
            int iDaysOld = 0;
            string strEncryptedPwd = string.Empty;
            string sPwdAgeError = string.Empty;
            string sPwdExpStatus = string.Empty;
            string strSQLQaUSer = string.Empty;

            string sOldPwdCheckError = string.Empty;
            string strPwdActualStatus = string.Empty;
            string sMyDbValue = string.Empty;

            DateTime dateOldDate = System.DateTime.Today.AddDays(-45);

            //   usp_secure_check_valid_user_al


            if (!_UseWSDL)
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
                        using (SqlCommand cmd = new SqlCommand(_CommandString, con))
                        {
                            cmd.Parameters.Add("@userid", SqlDbType.VarChar).Value = UserName;
                            cmd.Parameters.Add("@userPwd", SqlDbType.VarChar).Value = Passwd;



                            cmd.CommandType = CommandType.StoredProcedure;
                            using (SqlDataReader idr = cmd.ExecuteReader())
                            {
                                //  count = idr.VisibleFieldCount;
                                if (idr.HasRows)
                                {
                                    while (idr.Read())
                                    {



                                        if (idr["active"] != System.DBNull.Value)
                                        {
                                            strActive = (string)idr["active"];
                                        }

                                        if (idr["pwd"] != System.DBNull.Value)
                                        {
                                            Passwd = (string)idr["pwd"];
                                        }

                                        if (idr["pwd_Status"] != System.DBNull.Value)
                                        {
                                            strPwdStatus = (string)idr["pwd_Status"];
                                        }


                                        //if (idr["lastaccess"] != System.DBNull.Value)
                                        //{
                                        //    strLastAccess = (string)idr["lastaccess"];
                                        //}


                                        //                  Try 'subba-040913
                                        //                      If Not IsDBNull(SQLReader.GetValue(SQLReader.GetOrdinal("lastaccess"))) Then
                                        //                          strLastAccess = SQLReader.GetValue(SQLReader.GetOrdinal("lastaccess"))
                                        //                          dtLastAccess = Convert.ToDateTime(strLastAccess)
                                        //                      Else
                                        //                          dtLastAccess = Nothing '040313-here 'don't change
                                        //                      End If
                                        //                  Catch ex As System.Exception
                                        //                      dtLastAccess = Nothing
                                        //                      strError = "SystemErrorOccured-"
                                        //                  End Try




                                        if (idr["past1"] != System.DBNull.Value)
                                        {
                                            strPast1 = (string)idr["past1"];
                                        }

                                        if (idr["past2"] != System.DBNull.Value)
                                        {
                                            strPast2 = (string)idr["past2"];
                                        }

                                        if (idr["past3"] != System.DBNull.Value)
                                        {
                                            strPast3 = (string)idr["past3"];
                                        }

                                        if (idr["past4"] != System.DBNull.Value)
                                        {
                                            strPast4 = (string)idr["past4"];
                                        }

                                        if (idr["past5"] != System.DBNull.Value)
                                        {
                                            strPast5 = (string)idr["past5"];
                                        }

                                        if (idr["past6"] != System.DBNull.Value)
                                        {
                                            strPast6 = (string)idr["past6"];
                                        }



                                        if (strActive == "Y")
                                        {
                                            r = 0;

                                        }


                                    }
                                }
                            }


                            //    cmd.ExecuteNonQuery();//
                            ////// Invoke ExecuteReader method.
                            // 
                            // r = 0;
                        }
                        con.Close();
                    }








                }
                catch (SqlException sx)
                {


                }
                catch (Exception ex)
                {




                }
            }

            else
            {




            }



            //          If SQLReader.HasRows() = True Then
            //              While SQLReader.Read()
            //                  If Not IsDBNull(SQLReader.GetValue(SQLReader.GetOrdinal("active"))) Then  ' Active = Y ,N
            //                      strActive = SQLReader.GetValue(SQLReader.GetOrdinal("active"))
            //                  End If
            //                  If Not IsDBNull(SQLReader.GetValue(SQLReader.GetOrdinal("pwd"))) Then
            //                      strPwd = SQLReader.GetValue(SQLReader.GetOrdinal("pwd"))
            //                      HttpContext.Current.Session("strPwd") = strPwd
            //                  End If
            //                  If Not IsDBNull(SQLReader.GetValue(SQLReader.GetOrdinal("pwd_Status"))) Then 'R-revoke, L-Lock, A-Active
            //                      strPwdStatus = SQLReader.GetValue(SQLReader.GetOrdinal("pwd_Status"))
            //                  End If
            //                  Try 'subba-040913
            //                      If Not IsDBNull(SQLReader.GetValue(SQLReader.GetOrdinal("lastaccess"))) Then
            //                          strLastAccess = SQLReader.GetValue(SQLReader.GetOrdinal("lastaccess"))
            //                          dtLastAccess = Convert.ToDateTime(strLastAccess)
            //                      Else
            //                          dtLastAccess = Nothing '040313-here 'don't change
            //                      End If
            //                  Catch ex As System.Exception
            //                      dtLastAccess = Nothing
            //                      strError = "SystemErrorOccured-"
            //                  End Try

            //                  If Not IsDBNull(SQLReader.GetValue(SQLReader.GetOrdinal("past1"))) Then
            //                      strPast1 = SQLReader.GetValue(SQLReader.GetOrdinal("past1"))
            //                  End If
            //                  If Not IsDBNull(SQLReader.GetValue(SQLReader.GetOrdinal("past2"))) Then
            //                      strPast2 = SQLReader.GetValue(SQLReader.GetOrdinal("past2"))
            //                  End If
            //                  If Not IsDBNull(SQLReader.GetValue(SQLReader.GetOrdinal("past3"))) Then
            //                      strPast3 = SQLReader.GetValue(SQLReader.GetOrdinal("past3"))
            //                  End If
            //                  If Not IsDBNull(SQLReader.GetValue(SQLReader.GetOrdinal("past4"))) Then
            //                      strPast4 = SQLReader.GetValue(SQLReader.GetOrdinal("past4"))
            //                  End If
            //                  If Not IsDBNull(SQLReader.GetValue(SQLReader.GetOrdinal("past5"))) Then
            //                      strPast5 = SQLReader.GetValue(SQLReader.GetOrdinal("past5"))
            //                  End If
            //                  If Not IsDBNull(SQLReader.GetValue(SQLReader.GetOrdinal("past6"))) Then
            //                      strPast6 = SQLReader.GetValue(SQLReader.GetOrdinal("past6"))
            //                  End If
            //              End While

            //              'strError = strError  'Y expired, G grace period
            //          Else
            //              strPwdActualStatus = GeneralFuncs.getStatus("QAUSER", "PWD_STATUS", strUs, "")  'subba-101608
            //              If (strPwdActualStatus = "R") Then 'subba-101608
            //                  strError = "Your password has been reset, try login with Reset password"
            //              ElseIf (strPwdActualStatus = "L") Then 'subba-101608
            //                  strError = "Your password has been Revoked, please contact your Administrator"
            //              Else
            //                  strError = "Your login or password may be incorrect." '' Please try again!!!!" 'subba-030915
            //              End If
            //          End If

            //          If Not String.IsNullOrEmpty(strActive) AndAlso strActive <> "Y" Then
            //              strError = "Your account is not activated yet. Please call support!!!!"
            //          End If

            //          Dim iDaysSinceLogin As Integer = 0
            //          Try
            //              If (dtLastAccess <> Nothing) Then
            //                  iDaysOld = DateDiff(DateInterval.Day, dtLastAccess, Now()) - 1
            //                  sMyDbValue = getDbValue("system_preferences", "lov_value", "lov_name", "passwordDaysLastLogin")
            //                  If (sMyDbValue <> "") Then Integer.TryParse(sMyDbValue, iDaysSinceLogin) 'subba-030515
            //                  'iDaysSinceLogin = getDbValue("system_preferences", "lov_value", "lov_name", "passwordDaysLastLogin")

            //                  If (isAdminUserRole(strUs) = False) Then ' subba-021108
            //                      If iDaysOld > iDaysSinceLogin Then 'Revoke the pwd
            //                          strError = RevokePasswordAl(strUs, strDebugVal)
            //                          strError = "Your password is more than " + iDaysSinceLogin.ToString() + "days old, " & strError & " Please call support!!!"
            //                      End If
            //                  End If
            //              End If
            //          Catch ex As System.Exception
            //              strError = "SystemErrorOccured.." '040313-HERE
            //              iDaysSinceLogin = -1 'subba-030515
            //              [Global].insertExceptionDetails("9210_alLogin_postback-GeneralFuncs.checkValidUserAl()-usp_secure_check_valid_user_al:" + ex.Message, "GeneralFuncs.checkValidUserAl()-" + strUs + ":" + strPass + ":-:" + strPwdActualStatus + ":" + Left(ex.StackTrace, 300), "GeneralFuncs.checkValidUserAl()")
            //          End Try

            //          SQLReader.Close()
            //          SQLReader = Nothing
            //          SQLComm = Nothing
            //          SQLConn1.Close()
            //          SQLConn1 = Nothing
            //      Catch ex As System.Exception
            //          strError = ex.Message
            //          strError = "SystemErrorOccured." 'strActive lastaccess-DateNotRetrieved-IsDbNull-040313-here 'Conversion from string "A" to type "Integer" is not valid  -bhsf
            //          [Global].insertExceptionDetails("9211_alLogin_postback-GeneralFuncs.checkValidUserAl()-usp_secure_check_valid_user_al__iisreset:" + ex.Message, "GeneralFuncs.checkValidUserAl()-" + strUs + ":" + strPass + "-" + Left(ex.StackTrace, 300), "GeneralFuncs.checkValidUserAl()")
            //      End Try

            //      Return strError
            //  End Function
            return r;

        }

    }
}
