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
    public class AuditTransactions
    {

        static string _AppName = "DCSGlobal.BusinessRules.Logging.AuditTransactions";
        static string sConnectionString = "";

        public bool isInitialized;
        public AuditTransactions()
        {
            isInitialized = true;
        }


        public AuditTransactions(string ConncectionString)
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
        private string strUid;
        private string strHc;
        private string strPhc;
        private string strPatAcct;
        private string InsType;

        public enum AuditType
        {
            Request = 10,
            Response = 20,
            Both = 30,
        }

        public string UserID
        {

            set
            {

                strUid = value;
            }
        }
        public string HospitalCode
        {

            set
            {

                strHc = value;
            }
        }


        public string PatientHospitalCode
        {

            set
            {

                strPhc = value;
            }
        }

        public string PatientAccountNumber
        {

            set
            {

                strPatAcct = value;
            }
        }


        public string InsuranceType
        {

            set
            {

                InsType = value;
            }
        }


        public string EdiDbImportRequest(string strContent, string strContentResponse, string strContentRaw, string PayorID,
                                                        AuditType at, string ConStr, string sVendorName, string suspEdiRequest)
        {
           

      
            string strRetBatchReqID = "0";

         //   string sqlString = ;

     
            try
            {
               string sqlQuery;
               sqlQuery = suspEdiRequest;


                CON.ConnectionString = sConnectionString;
                SqlCommand cmd = new SqlCommand(sqlQuery, CON);
                cmd.CommandType = CommandType.StoredProcedure;
                CON.Open();
                cmd.Parameters.Add("@PAYOR_ID", SqlDbType.VarChar).Value = PayorID;
                cmd.Parameters.Add("@REQUEST", SqlDbType.VarChar).Value = strContent;
                cmd.Parameters.Add("@Debug", SqlDbType.VarChar).Value = "N";
                cmd.Parameters.Add("@user_id", SqlDbType.VarChar).Value = strUid;
                cmd.Parameters.Add("@hosp_code", SqlDbType.VarChar).Value = strHc;
                cmd.Parameters.Add("@pat_hosp_code", SqlDbType.VarChar).Value = strPhc;
                cmd.Parameters.Add("@patient_number", SqlDbType.VarChar).Value = strPatAcct;
                cmd.Parameters.Add("@ins_type", SqlDbType.VarChar).Value = InsType;
                cmd.Parameters.Add("@VendorName", SqlDbType.VarChar).Value = sVendorName;

          

                SqlDataReader sqlReader = cmd.ExecuteReader();



                sqlReader.Read();

                if (sqlReader.HasRows)
                {
                  if (sqlReader[0] != System.DBNull.Value)
                    {
                        strRetBatchReqID = (string)sqlReader[0];
                    }
    

                }

            // //   r = "0";
            }
            catch (Exception ex)
            {
                log.ExceptionDetails(_AppName + " EdiDbImportRequest", ex);
            }

            finally
            {
                CON.Close();
            }
          
            return strRetBatchReqID;
        }


        public int EdiDbImport(string strContent, string strContentResponse, string strContentRaw, string PayorID,
                                                AuditType at, string sLoopAgainStatus, string sVendorName, string ConStr)
        {






            string rEdiDbImport = "0";

            //  sqlString = suspEdiRequest;

           // int r = -1;
            try
            {
                string sqlQuery;
                sqlQuery = "usp_insert_exceptions_details";


                CON.ConnectionString = sConnectionString;
                SqlCommand cmd = new SqlCommand(sqlQuery, CON);
                cmd.CommandType = CommandType.StoredProcedure;
                CON.Open();
                cmd.Parameters.Add("@PAYOR_ID", SqlDbType.VarChar).Value = PayorID;
                cmd.Parameters.Add("@REQUEST", SqlDbType.VarChar).Value = strContent;
                cmd.Parameters.Add("@Debug", SqlDbType.VarChar).Value = "N";
                cmd.Parameters.Add("@user_id", SqlDbType.VarChar).Value = strUid;
                cmd.Parameters.Add("@hosp_code", SqlDbType.VarChar).Value = strHc;
                cmd.Parameters.Add("@pat_hosp_code", SqlDbType.VarChar).Value = strPhc;
                cmd.Parameters.Add("@patient_number", SqlDbType.VarChar).Value = strPatAcct;
                cmd.Parameters.Add("@ins_type", SqlDbType.VarChar).Value = InsType;
                cmd.Parameters.Add("@VendorName", SqlDbType.VarChar).Value = sVendorName;



                SqlDataReader sqlReader = cmd.ExecuteReader();



                sqlReader.Read();

                if (sqlReader.HasRows)
                {
                    if (sqlReader[0] != System.DBNull.Value)
                    {
                        rEdiDbImport = (string)sqlReader[0];
                    }


                }

                //   r = "0";
            }
            catch (Exception ex)
            {
                log.ExceptionDetails(_AppName + " AuditTransactionsEdiDbImportRequest", ex);
            }

            finally
            {
                CON.Close();
            }
            //return r;
         //   return rEdiDbImport;
            
            
            
            
            
            int RetVal = 0;




            //Dim sInnerExMsg As String = "", ConnEx As SqlConnection = Nothing, Comm As SqlCommand = Nothing, CommEx As SqlCommand = Nothing, sqlString As String = ""

            //Dim RequestXML As String = ""
            //Dim ResponseXML As String = ""
            //Dim accReader As SqlDataReader
            //Dim sqlStrUSp1 As String = ""
            //Dim sqlStrUSp2 As String = ""

            //Dim ConnExEdiImp271 As SqlConnection = Nothing
            //Dim CommExEdiImp271 As SqlCommand = Nothing
            //Dim ediImpReader271 As SqlDataReader = Nothing
            //Dim strUspEdiDbImp271 As String = Nothing

            //'exec USP_INSERT_AUDIT_ELIGIBILITY_270_271 @PAYOR_ID='00007',@RESPONSE='ISA*00*          *00*          *ZZ*dcsglo         *ZZ*CMS            *131015*1100*^*00501*034041300*0*P*:~GS*HS*dcsglo*CMS*20131015*1100*372*X*005010X279A1~ST*270*99999*005010X279A1~BHT*0022*13*87F4E022133145B*20131015*1100~HL*1**20*1~NM1*PR*2*CMS*****PI*CMS~HL*2*1*21*1~NM1*1P*2*LIMA MEMORIAL HLTH SYS*****XX*1336144732~HL*3*2*22*0~TRN*1*87F4E022133145B*9954719251~NM1*IL*1*LEONDUDAKIS*ACHILLES****MI*567090674A~DMG*D8*19220617~DTP*291*D8*20090919~EQ*AG~EQ*30~EQ*47~EQ*42~EQ*14~EQ*45~SE*18*99999~GE*1*372~IEA*1*034041300~',@batch_id=32050,@EBRID=1564,@Debug='N',@user_id='ConsoleEligV11271'

            //Try
            //    strUspEdiDbImp271 = ConfigurationManager.AppSettings("uspEdiDbImport") 'USP_INSERT_AUDIT_ELIGIBILITY_270_271
            //    ConnExEdiImp271 = New SqlConnection()
            //    With ConnExEdiImp271
            //        .ConnectionString = SQL_CONNECTION_STRING
            //        .Open()
            //    End With

            //    CommExEdiImp271 = New SqlCommand(strUspEdiDbImp271, ConnExEdiImp271) 'subba-101413 '"USP_INSERT_AUDIT_ELIGIBILITY_270_271"
            //    CommExEdiImp271.CommandType = CommandType.StoredProcedure
            //    CommExEdiImp271.CommandTimeout = 0

            //    CommExEdiImp271.Parameters.Add("@PAYOR_ID", SqlDbType.VarChar)
            //    CommExEdiImp271.Parameters("@PAYOR_ID").Direction = ParameterDirection.Input
            //    CommExEdiImp271.Parameters("@PAYOR_ID").Value = Payor

            //    CommExEdiImp271.Parameters.Add("@RESPONSE", SqlDbType.VarChar)
            //    CommExEdiImp271.Parameters("@RESPONSE").Direction = ParameterDirection.Input
            //    CommExEdiImp271.Parameters("@RESPONSE").Value = strContentResponse

            //    CommExEdiImp271.Parameters.Add("@batch_id", SqlDbType.BigInt)
            //    CommExEdiImp271.Parameters("@batch_id").Direction = ParameterDirection.Input
            //    CommExEdiImp271.Parameters("@batch_id").Value = strRetBatchReqID

            //    CommExEdiImp271.Parameters.Add("@EBRID", SqlDbType.BigInt)
            //    CommExEdiImp271.Parameters("@EBRID").Direction = ParameterDirection.Input
            //    CommExEdiImp271.Parameters("@EBRID").Value = EBRID

            //    CommExEdiImp271.Parameters.Add("@Debug", SqlDbType.VarChar)
            //    CommExEdiImp271.Parameters("@Debug").Direction = ParameterDirection.Input
            //    CommExEdiImp271.Parameters("@Debug").Value = "N"

            //    CommExEdiImp271.Parameters.Add("@user_id", SqlDbType.VarChar)
            //    CommExEdiImp271.Parameters("@user_id").Direction = ParameterDirection.Input
            //    CommExEdiImp271.Parameters("@user_id").Value = strUid

            //    CommExEdiImp271.Parameters.Add("@ts1", SqlDbType.VarChar)
            //    CommExEdiImp271.Parameters("@ts1").Direction = ParameterDirection.Input
            //    CommExEdiImp271.Parameters("@ts1").Value = strTS1

            //    CommExEdiImp271.Parameters.Add("@ts2", SqlDbType.VarChar)
            //    CommExEdiImp271.Parameters("@ts2").Direction = ParameterDirection.Input
            //    CommExEdiImp271.Parameters("@ts2").Value = strTS2

            //    CommExEdiImp271.Parameters.Add("@loop_counter", SqlDbType.Int) 'subba-111113
            //    CommExEdiImp271.Parameters("@loop_counter").Direction = ParameterDirection.Input
            //    CommExEdiImp271.Parameters("@loop_counter").Value = CInt(strLoopAttempt)

            //    ediImpReader271 = CommExEdiImp271.ExecuteReader()
            //    While ediImpReader271.Read()
            //        If Not IsDBNull((ediImpReader271.GetValue(0))) Then
            //            RetVal = strRetBatchReqID 'ediImpReader271.GetValue(0) 'ediImpReader.GetOrdinal("GLOBAL_ID")
            //            sLoopAgainStatus = ediImpReader271.GetValue(ediImpReader271.GetOrdinal("loop_again")) 'subba-111113
            //        End If
            //    End While
            //    RetVal = strRetBatchReqID
            //    CommExEdiImp271.Dispose()
            //    ConnExEdiImp271.Close()
            //    CommExEdiImp271 = Nothing
            //Catch ex As System.Exception
            //    'WriteEvent(ex.Message)




            //    CommExEdiImp271.Dispose()
            //    ConnExEdiImp271.Close()
            //    CommExEdiImp271 = Nothing

            //    log.ExceptionDetails(_AppName + " AuditTransactionsEdiDbImport", ex)


            //End Try
            return RetVal;
        }




        public int RequestResponse(string strContent, string strContentResponse, string strContentRaw,
                                                 string Payor, AuditType at, string ConStr, string sVendorName)
        {






            int RetVal = 0;

            //sInnerExMsg As String = "", ConnEx As SqlConnection = Nothing, Comm As SqlCommand = Nothing, CommEx As SqlCommand = Nothing, sqlString As String = ""

            //    Dim RequestXML As String = ""
            //    Dim ResponseXML As String = ""
            //    Dim accReader As SqlDataReader

            //    Try

            //        Dim ws As New HipaaParser.Service1.HipaaParser()

            //        'HipaaParser.HipaaParserSoapClient

            //        If strContent.IndexOf("<requests>") >= 0 Then
            //            RequestXML = strContent
            //        Else
            //            RequestXML = ws.ParseXMLMode(strContent)
            //        End If
            //        ts3 = DateTime.Now()
            //        'RequestXML = ws.ParseXMLMode(strContent)
            //        ResponseXML = ws.ParseXMLMode(strContentResponse)
            //        ts4 = DateTime.Now()
            //        'ws.Dispose()
            //    Catch ex As System.Exception
            //        '  WriteEvent(ex.Message)

            //        log.ExceptionDetails(_AppName + " AuditTransactions", ex)

            //        ' logExceptionGeneric(ex.Message, ex.StackTrace)
            //    End Try


            //    'LOOK-subba-100213--only response will be sent toDB  usp_edi_import_271
            //    If at = AuditType.Request Then
            //        sqlString = "USP_INSERT_AUDIT_ELIGIBILITY_REQUEST"
            //    ElseIf at = AuditType.Response Then
            //        sqlString = "USP_INSERT_AUDIT_ELIGIBILITY_RESPONSE"
            //    ElseIf at = AuditType.Both Then
            //        sqlString = "USP_INSERT_AUDIT_ELIGIBILITY_REQUEST_RESPONSE"
            //    End If

            //    Try
            //        ts5 = DateTime.Now()
            //        ConnEx = New SqlConnection()
            //        With ConnEx
            //            .ConnectionString = ConStr
            //            .Open()
            //        End With
            //        Comm = New SqlCommand(sqlString, ConnEx)
            //        With Comm
            //            .CommandType = CommandType.StoredProcedure
            //            .CommandTimeout = 90
            //        End With
            //        Dim param1 As SqlParameter = Comm.Parameters.Add("@PAYOR_ID", SqlDbType.VarChar)
            //        param1.Value = Payor
            //        If at = AuditType.Request Then
            //            With Comm.Parameters
            //                Dim param2 As SqlParameter = .Add("@REQUEST", SqlDbType.VarChar)
            //                param2.Value = strContent
            //                Dim paramrr As SqlParameter = .Add("@REQUEST_RAW", SqlDbType.VarChar)
            //                paramrr.Value = strContentRaw
            //                Dim paramrl As SqlParameter = .Add("@REVENUE_LOC", SqlDbType.VarChar)
            //                paramrl.Value = RL
            //                Dim paramCSN As SqlParameter = .Add("@CSN", SqlDbType.VarChar)
            //                paramCSN.Value = CSN
            //                Dim paramHAR As SqlParameter = .Add("@HAR", SqlDbType.VarChar)
            //                paramHAR.Value = HAR
            //                Dim paramInsType As SqlParameter = .Add("@INS_TYPE", SqlDbType.VarChar)
            //                paramInsType.Value = InsType
            //                Dim paramMRN As SqlParameter = .Add("@MRN", SqlDbType.VarChar)
            //                paramMRN.Value = MRN
            //            End With
            //        ElseIf at = AuditType.Response Then
            //            With Comm.Parameters
            //                Dim param2 As SqlParameter = .Add("@RESPONSE", SqlDbType.VarChar, -1)
            //                param2.Value = strContent
            //                Dim param3 As SqlParameter = .Add("@ID", SqlDbType.Int)
            //                param3.Value = ID
            //            End With
            //        ElseIf at = AuditType.Both Then
            //            With Comm.Parameters
            //                Dim param2 As SqlParameter = .Add("@RESPONSE", SqlDbType.VarChar, -1)
            //                param2.Value = strContentResponse
            //                Dim param3 As SqlParameter = .Add("@REQUEST", SqlDbType.VarChar, -1)
            //                param3.Value = strContent
            //                Dim param4 As SqlParameter = .Add("@RESPONSE_XML", SqlDbType.VarChar, -1)
            //                param4.Value = ResponseXML
            //                Dim param5 As SqlParameter = .Add("@REQUEST_XML", SqlDbType.VarChar, -1)
            //                param5.Value = RequestXML
            //                Dim param6 As SqlParameter = .Add("@EBRID", SqlDbType.VarChar)
            //                param6.Value = EBRID
            //                Dim param7 As SqlParameter = .Add("@VendorName", SqlDbType.VarChar) 'subba-021914
            //                param7.Value = sVendorName
            //            End With
            //        End If
            //        ' RetVal = CInt(Comm.ExecuteScalar())

            //        accReader = Comm.ExecuteReader()
            //        While accReader.Read()
            //            If Not IsDBNull((accReader.GetValue(0))) Then
            //                RetVal = accReader.GetValue(accReader.GetOrdinal("GLOBAL_ID"))
            //            End If
            //        End While
            //        accReader.Close()
            //        Comm.Dispose()
            //        ConnEx.Close()
            //        Comm = Nothing
            //        ts6 = DateTime.Now()
            //    Catch ex As System.Exception
            //        '  sInnerExMsg = AuditException.Message
            //        'WriteEvent(sInnerExMsg)
            //        '  logExceptionGeneric(AuditException.Message, AuditException.StackTrace)

            //        log.ExceptionDetails(_AppName + " AuditTransactions", ex)

            //        If ConnEx IsNot Nothing Then
            //            ConnEx.Close()
            //        End If
            //        CommEx = Nothing
            //    Finally
            //        'PerformanceAudit(RetVal) 'subba-112614
            //    End Try

            return RetVal;
        }


    }
}
