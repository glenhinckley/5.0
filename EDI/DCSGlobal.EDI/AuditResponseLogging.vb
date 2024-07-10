Option Explicit On


Imports DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff
Imports DCSGlobal.BusinessRules.CoreLibrary.IO

Imports System.Text.RegularExpressions
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Collections.Generic

Imports System.Threading
Imports System.Collections



Imports DCSGlobal.BusinessRules.Logging

Namespace DCSGlobal.EDI
    Public Class AuditResponseLogging
        Implements IDisposable

        ' Keep track of when the object is disposed. 
        Protected disposed As Boolean = False





        Private _REQ As String = String.Empty
        Private _RES As String = String.Empty
        Private _PatientHospitalCode As String = String.Empty

        Private log As New logExecption
        Private ss As New StringStuff
        Private oD As New Declarations
        Private _ConnectionString As String = String.Empty
        Dim _BATCH_ID As Integer


        ' This method disposes the base object's resources. 
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposed Then
                If disposing Then


                    log = Nothing
                    'email = Nothing
                    ' Insert code to free managed resources. 
                End If
                ' Insert code to free unmanaged resources. 
            End If
            Me.disposed = True
        End Sub


        ' Do not change or add Overridable to these methods. 
        ' Put cleanup code in Dispose(ByVal disposing As Boolean). 
        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

        Public WriteOnly Property REQ As String

            Set(value As String)

                _REQ = value
            End Set
        End Property

        Public WriteOnly Property RES As String

            Set(value As String)

                _RES = value
            End Set
        End Property

        Public WriteOnly Property PatientHospitalCode As String

            Set(value As String)

                _PatientHospitalCode = value
            End Set
        End Property


        Public WriteOnly Property ConnectionString As String

            Set(value As String)
                _ConnectionString = value
                oD._ConnectionString = value
                log.ConnectionString = value
            End Set
        End Property




        Public Function LOG270(ByVal RAW270 As String, ByVal EBR_ID As Double, ByVal VendorName As String, ByVal PayorID As String _
                               ) As Integer


            Dim r As Integer = -1





            Dim _batch_id As String = String.Empty
            Dim _EDI As String = String.Empty
            Dim _Res_Reject_Reason_Code As String = String.Empty
            Dim res_npi As String = String.Empty
            Dim _req_ServiceType_Code As String = String.Empty








            Try

                Dim sqlString = String.Empty

                sqlString = "usp_eligibility_log_EDI_270_request"



                Using vedi As New ValidateEDI()

                    r = vedi.byString(RAW270)
                    If r = 0 Then
                        _req_ServiceType_Code = vedi.ServiceTypeCode




                    End If



                End Using


                '@Payor_id varchar(20)= null,
                '@request_xml varchar(max)= null,
                '@Patient_number varchar(20)= null,
                '@hosp_code varchar(10) = null,
                '@pat_hosp_code varchar(20)= null,
                '@ins_type varchar(20)= null,
                '@user_id varchar(20)= null,


                '@requested_service_type varchar(100) = null



                Using Con1 As New SqlConnection(_ConnectionString)
                    Con1.Open()
                    Using cmd As New SqlCommand(sqlString, Con1)

                        cmd.CommandType = CommandType.StoredProcedure




                        cmd.Parameters.AddWithValue("@PAYOR_ID", PayorID)
                        cmd.Parameters.AddWithValue("@vendor_name", VendorName)
                        cmd.Parameters.AddWithValue("@ebr_id", EBR_ID)
                        'cmd.Parameters.AddWithValue("@batch_id", BatchID)
                        'cmd.Parameters.AddWithValue("@PAYOR_ID", PAYOR_ID)
                        'cmd.Parameters.AddWithValue("@EDI", EDI_271)
                        'cmd.Parameters.AddWithValue("@ebr_id", EBR_ID)

                        'cmd.Parameters.AddWithValue("@vendor_id", VendorID)
                        'cmd.Parameters.AddWithValue("@Res_Reject_Reason_Code", AAAFailureCode)
                        'cmd.Parameters.AddWithValue("@res_npi", NPI)
                        'cmd.Parameters.AddWithValue("@req_ServiceType_Code", reqServiceTypeCodes)

                        cmd.ExecuteNonQuery()


                        r = 0

                    End Using
                    Con1.Close()
                End Using







            Catch exsql As SqlException



                log.ExceptionDetails("AuditResponseLogging.Log270" + " " + "DCSGlobal.EDI.import to 270 loge to db failed for ebr_id : " + Convert.ToString(EBR_ID), exsql)

                r = -2



            Catch ex As Exception

                r = -3

                log.ExceptionDetails("AuditResponseLogging.Log270" + " " + "DCSGlobal.EDI.import to 270 loge to db failed for EBRid : " + Convert.ToString(EBR_ID), ex)

            Finally

                'sqlConn.Close()
            End Try







            Return r

        End Function


        Public Function AuditEmdeonLookup(ByVal EBR_ID As String, ByVal LOOKUP_PARAM_XML As String, ByVal MODIFIED_USER As String)
            Dim r As Integer = -1
            Dim sqlString = String.Empty
            Try

                sqlString = "USP_AUDIT_EMDEON_LOOKUP"

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(sqlString, Con)
                        cmd.CommandTimeout = 90
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@ebr_id", EBR_ID)
                        cmd.Parameters.AddWithValue("@LOOKUP_PARAM_XML", LOOKUP_PARAM_XML)
                        cmd.Parameters.AddWithValue("@MODIFIED_DATE", DateTime.Now)
                        cmd.Parameters.AddWithValue("@MODIFIED_BY", MODIFIED_USER)


                        cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using
            Catch exsql As SqlException
                log.ExceptionDetails("AuditResponseLogging.AuditEmdeonLookup" + " " + "DCSGlobal.EDI.import - AuditEmdeonLookup db failed for EBR ID : " + Convert.ToString(EBR_ID), exsql)
                r = -2
            Catch ex As Exception
                r = -3
                log.ExceptionDetails("AuditResponseLogging.AuditEmdeonLookup" + " " + "DCSGlobal.EDI.import -  AuditEmdeonLookup db failed for EBR ID : " + Convert.ToString(EBR_ID), ex)
            Finally

                'sqlConn.Close()
            End Try
            Return r
        End Function
        Public Function EmdeonSmartDelete(ByVal BatchID As Int64, ByVal EBR_ID As String) As Integer

            Dim r As Integer = -1
            Dim sqlString = String.Empty

            Try

                sqlString = "USP_EMDEON_SMART_DELETE"

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(sqlString, Con)
                        cmd.CommandTimeout = 90
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@batch_id", BatchID)
                        cmd.Parameters.AddWithValue("@ebr_id", EBR_ID)
                        cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using
            Catch exsql As SqlException
                log.ExceptionDetails("AuditResponseLogging.EmdeonSmartDelete" + " " + "DCSGlobal.EDI.import - EmdeonSmartDelete db failed for batchid : " + Convert.ToString(BatchID) + " , EBR_ID: " + Convert.ToString(EBR_ID), exsql)
                r = -2
            Catch ex As Exception
                r = -3
                log.ExceptionDetails("AuditResponseLogging.EmdeonSmartDelete" + " " + "DCSGlobal.EDI.import -  EmdeonSmartDelete db failed for batchid : " + Convert.ToString(BatchID) + " , EBR_ID: " + Convert.ToString(EBR_ID), ex)
            Finally

                'sqlConn.Close()
            End Try
            Return r
        End Function



        Public Function InsertEmdeonAssistantReqRes()
            Dim r As Integer = -1
            Dim sqlString = String.Empty
            Dim _batch_id As String = String.Empty
            Dim _EDI As String = String.Empty
            Dim _Res_Reject_Reason_Code As String = String.Empty
            Try

                sqlString = "usp_emdeon_log_Req_Res"

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(sqlString, Con)
                        cmd.CommandTimeout = 90
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@PAT_HOSP_CODE", _PatientHospitalCode)
                        cmd.Parameters.AddWithValue("@REQUEST", _REQ)
                        cmd.Parameters.AddWithValue("@RESPONSE", _RES)
                        cmd.ExecuteNonQuery()
                    End Using
                    Con.Close()
                End Using
            Catch exsql As SqlException
                log.ExceptionDetails("AuditResponseLogging.InsertEmdeonAssistantReqRes" + " " + "DCSGlobal.EDI.import -InsertEmdeonAssistantReqRes - db failed for PatHospCode : " + Convert.ToString(_PatientHospitalCode), exsql)
                r = -2
            Catch ex As Exception
                r = -3
                log.ExceptionDetails("AuditResponseLogging.InsertEmdeonAssistantReqRes" + " " + "DCSGlobal.EDI.import -InsertEmdeonAssistantReqRes - db failed for PatHospCode : " + Convert.ToString(_PatientHospitalCode), ex)
            Finally
                'sqlConn.Close()
            End Try
            Return r
        End Function

        Public Function Log271(ByVal BatchID As Int64, ByVal PAYOR_ID As String, ByVal EDI_271 As String, ByVal EBR_ID As String, _
                               ByVal VENDOR_NAME As String, ByVal AAAFailureCode As String, ByVal NPI As String, ByVal ServiceTypeCode As String _
                               ) As Integer

            Dim r As Integer = -1



            Dim sqlString = String.Empty





            Dim _batch_id As String = String.Empty
            Dim _EDI As String = String.Empty
            Dim _Res_Reject_Reason_Code As String = String.Empty



            Try

                sqlString = "usp_eligibility_log_EDI_271_response"

                Using Con As New SqlConnection(oD._ConnectionString)
                    Con.Open()




                    Using cmd As New SqlCommand(sqlString, Con)
                        cmd.CommandTimeout = 90

                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.AddWithValue("@batch_id", BatchID)
                        cmd.Parameters.AddWithValue("@PAYOR_ID", PAYOR_ID)
                        cmd.Parameters.AddWithValue("@EDI", EDI_271)
                        cmd.Parameters.AddWithValue("@ebr_id", EBR_ID)
                        '       cmd.Parameters.AddWithValue("@vendor_id", VENDOR_NAME)

                        cmd.Parameters.AddWithValue("@vendor_id", VENDOR_NAME)
                        cmd.Parameters.AddWithValue("@Res_Reject_Reason_Code", AAAFailureCode)
                        cmd.Parameters.AddWithValue("@res_npi", NPI)
                        cmd.Parameters.AddWithValue("@req_ServiceType_Code", ServiceTypeCode)
                 
                        cmd.ExecuteNonQuery()



                    End Using
                    Con.Close()
                End Using




            Catch exsql As SqlException



                log.ExceptionDetails("AuditResponseLogging.Log271" + " " + "DCSGlobal.EDI.import to 271 loge to db failed for batchid : " + Convert.ToString(BatchID) + "  edi: " + EDI_271, exsql)

                r = -2



            Catch ex As Exception

                r = -3

                log.ExceptionDetails("AuditResponseLogging.Log271" + " " + "DCSGlobal.EDI.import to 271 loge to db failed for batchid : " + Convert.ToString(BatchID) + "  edi: " + EDI_271, ex)

            Finally

                'sqlConn.Close()
            End Try


            Return r



        End Function


        Public Function EligibilityUpdateEBRProcessedFlagBeforeVendorCall(ByVal EBR_ID As String) As Integer

            Dim r As Integer = -1



            Dim sqlString = String.Empty





            Try

                sqlString = "usp_eligibility_update_ebr_processed_flag_before_vendor_call"

                Using Con As New SqlConnection(oD._ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(sqlString, Con)

                        cmd.CommandType = CommandType.StoredProcedure


                        cmd.Parameters.AddWithValue("@ebr_id", EBR_ID)


                        cmd.ExecuteNonQuery()

                        r = 0

                    End Using
                    Con.Close()
                End Using




            Catch exsql As SqlException



                log.ExceptionDetails("AuditResponseLogging.Log271" + " " + "usp_eligibility_update_ebr_processed_flag_before_vendor_call : " + Convert.ToString(EBR_ID), exsql)

                r = -2



            Catch ex As Exception

                r = -3

                log.ExceptionDetails("AuditResponseLogging.Log271" + " " + "usp_eligibility_update_ebr_processed_flag_before_vendor_call " + Convert.ToString(EBR_ID), ex)

            Finally

                'sqlConn.Close()
            End Try


            Return r



        End Function









        'Public Function Log271(ByVal BatchID As Int64, ByVal PAYOR_ID As String, ByVal EDI_271 As String, ByVal EBR_ID As String _
        '                      , ByVal VendorID As String, ByVal AAAFailureCode As String, ByVal NPI As String, ByVal reqServiceTypeCodes As String) As Integer

        '    Dim r As Integer = -1

        '    Try

        '        Dim sqlString = String.Empty

        '        sqlString = "usp_eligibility_log_EDI_271_response"



        '        Using Con1 As New SqlConnection(_ConnectionString)
        '            Con1.Open()
        '            Using cmd As New SqlCommand(sqlString, Con1)

        '                cmd.CommandType = CommandType.StoredProcedure


        '                cmd.Parameters.AddWithValue("@batch_id", BatchID)
        '                cmd.Parameters.AddWithValue("@PAYOR_ID", PAYOR_ID)
        '                cmd.Parameters.AddWithValue("@EDI", EDI_271)
        '                cmd.Parameters.AddWithValue("@ebr_id", EBR_ID)

        '                cmd.Parameters.AddWithValue("@vendor_id", VendorID)
        '                cmd.Parameters.AddWithValue("@Res_Reject_Reason_Code", AAAFailureCode)
        '                cmd.Parameters.AddWithValue("@res_npi", NPI)
        '                cmd.Parameters.AddWithValue("@req_ServiceType_Code", reqServiceTypeCodes)
        '                cmd.ExecuteNonQuery()


        '                r = 0

        '            End Using
        '            Con1.Close()
        '        End Using







        '    Catch exsql As SqlException



        '        log.ExceptionDetails("AuditResponseLogging.Log271" + " " + "DCSGlobal.EDI.import to 271 loge to db failed for batchid : " + Convert.ToString(BatchID) + "  edi: " + EDI_271, exsql)

        '        r = -2



        '    Catch ex As Exception

        '        r = -3

        '        log.ExceptionDetails("AuditResponseLogging.Log271" + " " + "DCSGlobal.EDI.import to 271 loge to db failed for batchid : " + Convert.ToString(BatchID) + "  edi: " + EDI_271, ex)

        '    Finally

        '        'sqlConn.Close()
        '    End Try


        '    Return r



        'End Function


        Public Function Log277(ByVal BatchID As Int64, ByVal PAYOR_ID As String, ByVal EDI_271 As String, ByVal cbr_id As String) As Int32
            Dim r As Int32 = 0

            Try

                Dim sqlString = String.Empty

                sqlString = "usp_Claim_status_log_EDI_277_response"



                Using Con1 As New SqlConnection(_ConnectionString)
                    Con1.Open()
                    Using cmd As New SqlCommand(sqlString, Con1)

                        cmd.CommandType = CommandType.StoredProcedure


                        cmd.Parameters.AddWithValue("@batch_id", BatchID)
                        cmd.Parameters.AddWithValue("@PAYOR_ID", PAYOR_ID)
                        cmd.Parameters.AddWithValue("@EDI", EDI_271)
                        cmd.Parameters.AddWithValue("@cbr_id", cbr_id)


                        cmd.ExecuteNonQuery()




                    End Using
                    Con1.Close()
                End Using







            Catch exsql As SqlException



                log.ExceptionDetails("AuditResponseLogging.Log277" + " " + "DCSGlobal.EDI.import to 277 loge to db failed for batchid : " + Convert.ToString(BatchID) + "  edi: " + EDI_271, exsql)


                r = 1


            Catch ex As Exception

                r = 1

                log.ExceptionDetails("AuditResponseLogging.Log277" + " " + "DCSGlobal.EDI.import to 277 loge to db failed for batchid : " + Convert.ToString(BatchID) + "  edi: " + EDI_271, ex)

            Finally

                'sqlConn.Close()
            End Try



            Return r


        End Function


        Public Function LOG278toREQ(ByVal EDI_278 As String, ByVal DELETE_FLAG_278 As String, _
                                        ByVal AUTH_ID As String, ByVal USER_ID As String, ByVal HOSP_CODE As String, ByVal SOURCE As String,
                                            ByVal PAYOR_ID As String, ByVal VENDOR_NAME As String, ByVal INS_TYPE As String, _
                                            ByVal account_number As String, ByVal PatHospCode As String) As Integer
            Dim r = -1

            Try








                Dim sqlString = String.Empty

                sqlString = "usp_auth_log_EDI_278_request"

                Using Con1 As New SqlConnection(_ConnectionString)
                    Con1.Open()
                    Using cmd As New SqlCommand(sqlString, Con1)
                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.AddWithValue("@PAYOR_ID", PAYOR_ID)
                        cmd.Parameters.AddWithValue("@request_xml", EDI_278)
                        cmd.Parameters.AddWithValue("@Patient_number", account_number)

                        cmd.Parameters.AddWithValue("@pat_hosp_code", PatHospCode)
                        cmd.Parameters.AddWithValue("@hosp_code", HOSP_CODE)

                        cmd.Parameters.AddWithValue("@ins_type", INS_TYPE)
                        cmd.Parameters.AddWithValue("@user_id", USER_ID)
                        cmd.Parameters.AddWithValue("@abr_id", AUTH_ID)
                        cmd.Parameters.AddWithValue("@vendor_name", VENDOR_NAME)
                        '    cmd.Parameters.AddWithValue("requested_service_type", requested_service_type)
                        ''   cmd.Parameters.AddWithValue("@req_ServiceType_Code", _reqServiceTypeCodes)

                        cmd.Parameters.Add("@BATCH_ID", SqlDbType.BigInt)
                        cmd.Parameters("@BATCH_ID").Direction = ParameterDirection.Output


                        cmd.ExecuteNonQuery()

                        _BATCH_ID = Convert.ToInt32(cmd.Parameters("@BATCH_ID").Value)


                        r = _BATCH_ID



                    End Using
                    Con1.Close()
                End Using

            Catch exsql As SqlException



                log.ExceptionDetails(oD.Version + "  " + "LOG278toREQ" + " " + " 278 loge to db failed", exsql)


                r = -2


            Catch ex As Exception

                log.ExceptionDetails(oD.Version + "  " + "LOG278toREQ" + " " + " 278 loge to db failed", ex)

                '   log.ExceptionDetails(oD.Version + "  " + "Import 271" + " " + "DCSGlobal.EDI.import to 271 loge to db failed for batchid : " + Convert.ToString(BatchID) + "  edi: " + EDI_271, ex)

                r = -3
            End Try

            Return r

        End Function



        Public Function LOG278toRES(ByVal EDI_278 As String, ByVal DELETE_FLAG_278 As String, ByVal AUTH_ID As String, _
                                        ByVal BATCH_ID As String, ByVal USER_ID As String, ByVal HOSP_CODE As String, ByVal SOURCE As String,
                                            ByVal PAYOR_ID As String, ByVal VENDOR_NAME As String, ByVal INS_TYPE As String, _
                                            ByVal account_number As String, ByVal PatHospCode As String) As Integer
            Dim r = -1

            Try




                '@abr_id bigint =0,
                '@vendor_id varchar(20) = null,
                '@Res_Reject_Reason_Code varchar(20) = null,
                '@res_npi varchar(20)  = null ,
                '@req_ServiceType_Code varchar(100) = null



                Dim sqlString = String.Empty

                sqlString = "usp_auth_log_EDI_278_response"

                Using Con1 As New SqlConnection(_ConnectionString)
                    Con1.Open()
                    Using cmd As New SqlCommand(sqlString, Con1)
                        cmd.CommandType = CommandType.StoredProcedure


                        cmd.Parameters.AddWithValue("@batch_id", BATCH_ID)
                        cmd.Parameters.AddWithValue("@PAYOR_ID", PAYOR_ID)
                        cmd.Parameters.AddWithValue("@EDI", EDI_278)

                        cmd.Parameters.AddWithValue("@abr_id", AUTH_ID)

                        cmd.Parameters.AddWithValue("@Patient_number", account_number)

                        cmd.Parameters.AddWithValue("@pat_hosp_code", PatHospCode)
                        cmd.Parameters.AddWithValue("@hosp_code", HOSP_CODE)

                        cmd.Parameters.AddWithValue("@ins_type", INS_TYPE)
                        cmd.Parameters.AddWithValue("@user_id", USER_ID)

                        ''   cmd.Parameters.AddWithValue("@req_ServiceType_Code", _reqServiceTypeCodes)

                        'cmd.Parameters.Add("@BATCH_ID", SqlDbType.BigInt)
                        'cmd.Parameters("@BATCH_ID").Direction = ParameterDirection.Output


                        cmd.ExecuteNonQuery()

                        '_BATCH_ID = Convert.ToInt32(cmd.Parameters("@BATCH_ID").Value)

                        ' this need to get srt to 0
                        r = 0



                    End Using
                    Con1.Close()
                End Using

            Catch exsql As SqlException



                '    log.ExceptionDetails(oD.Version + "  " + "Import 271" + " " + "DCSGlobal.EDI.import to 271 loge to db failed for batchid : " + Convert.ToString(BatchID) + "  edi: " + EDI_271, exsql)


                r = -2


            Catch ex As Exception



                '   log.ExceptionDetails(oD.Version + "  " + "Import 271" + " " + "DCSGlobal.EDI.import to 271 loge to db failed for batchid : " + Convert.ToString(BatchID) + "  edi: " + EDI_271, ex)

                r = -3
            End Try

            Return r

        End Function












    End Class
End Namespace

