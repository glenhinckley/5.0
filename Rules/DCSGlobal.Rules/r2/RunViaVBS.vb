Option Strict On
Option Explicit On

Imports System
Imports System.Data.DataColumn
Imports System.Data

Imports System.Data.SqlClient
Imports System.IO
Imports System.Text.RegularExpressions
Imports MSScriptControl.ScriptControlClass
Imports System.Collections
Imports System.Text



Imports System.Xml
Imports System.Xml.XPath
Imports System.IO.TextReader
Imports System.IO.Pipes
Imports System.Security.Principal

Imports DCSGlobal.BusinessRules.Logging
Imports DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff
Imports DCSGlobal.BusinessRules.CoreLibraryII
Imports DCSGlobal.Rules.FireRules



'Imports DCSGlobal.RealAlert.DCSGlobal.RealAlert

Namespace DCSGlobal.Rules

    Public Class RunViaVBS


        Implements IDisposable
        Private disposedValue As Boolean ' To detect redundant calls
        Dim scriptVBValue As String

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                End If
                _scriptCtl = Nothing
                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            Me.disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

        Public WriteOnly Property ConnectionString As String

            Set(value As String)
                _ConnectionString = value
                log.ConnectionString = value
                sch.ConnectionString = value
                dss.ConnectionString = value
                '    RA.ConnectionString = value
                Met.ConnectionString = value
            End Set
        End Property


        Public WriteOnly Property DLL As String

            Set(value As String)
                _DLL = value

            End Set
        End Property

        Private dss As New DataSets()
        Private log As New logExecption()
        Private sch As New SchedulerLog()
        Private ss As New StringStuff()
        '   Private RA As New GetSettings()
        Private Met As New Metrics()

        Private _DLL As String = String.Empty

        Private _ConnectionString As String = String.Empty

        Private sbAllRuleInsert As StringBuilder
        Private dataRowAuditTrail As DataRow
        Private dataAuditTrailTable As New DataTable
        Private dsGetAllTankData As DataSet
        Private filteredRows() As DataRow
        Private _dsRulesToFire As New DataSet

        Dim strScriptError As String
        ' Dim dlCol As New DataColumn

        Private _CfgUspRuleResultDelete As String = String.Empty
        Private _CfgUspRuleInsertByXmlTank As String = String.Empty

        Private bAllRuleDataRowSuccess As Boolean = False


        Private _COMMAND_TIMEOUT As Integer = 90


        Private _scriptVBValue As String = String.Empty
        'Private _colNameVal As System.Collections.Specialized.NameValueCollection
        'Private _htRuleMsgs As Hashtable
        Private _scriptCtl As MSScriptControl.ScriptControl = New MSScriptControl.ScriptControl
        'Private _dsRulesToFire As New DataSet


        Private _RULE_RESULT_MESSAGE As String = String.Empty
        Private _RESULT_ALL As String = String.Empty
        Private _RULE_CONTEXT_ID As Integer = 0
        Private _RULE_COMMAND As String = String.Empty
        Private _RULE_DEFINITION As String = String.Empty
        Private _RULE_FAILED As Boolean = False
        Private _RULE_FAILED_FLAG As Boolean = False
        Private _RULE_ID As Integer = 0
        Private _RULE_NAME As String = String.Empty
        Private _RUN_RULES As String = "N"
        Private _RULE_SUCCESS As String = String.Empty



        Private _TEST_RULE_ERROR_CODE As Integer = -10000
        Private _OPERATOR_ID As String = String.Empty
        Private _DIVISION_CODE As String = String.Empty
        Private _REGION_CODE As String = String.Empty



        Private _PRIMARY_PLAN_NUMBER As String = String.Empty
        Private _SOURCE As String = String.Empty
        Private _INSURACE_TYPE As String = String.Empty

        Private _PAT_HOSP_CODE As String = String.Empty


        Private _PATIENT_ACCOUNT_NUMBER As String = String.Empty
        Private _PATIENT_TYPE As String = String.Empty
        Private _PATIENT_FIRST_NAME As String = String.Empty
        Private _PATIENT_LAST_NAME As String = String.Empty
        Private _PID As Integer = 0
        Private _PATIENT_ID As Integer = 0
        Private _FINANCIAL_CLASS As String = String.Empty

        Private _I_BATCH_ID As Integer = 0


        Private _I_DATA_RULES As Integer = 0

        Private _CLIENT_FACILITY_CODE As String = String.Empty
        Private _ADMIT_DATE As String = String.Empty
        Private _DISCHARGE_DATE As String = String.Empty
        Private _EVENT_DATE As String = String.Empty
        Private _EVENT_TYPE As String = String.Empty
        Private _EVENT_DETAIL As String = String.Empty
        Private _CREATE_DATE As String = String.Empty




        Private _SB_ALL_RULE_INSERT As StringBuilder

        Private _TOTAL_RULE_TO_FIRE As Integer = 0
        Private _ALL_RULE_DATA_ROW_SUCCESS As Boolean = False

        Private _PATIENT_AUDIT_TRAIL_ID As Integer = 0



        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="strScript"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function RunRule(ByVal strScript As String, ByVal HOSP_CODE As String, ByVal AcountNumberorPID As String, ByVal patientNum As String, ByVal rdoPAT As Boolean) As Integer

            Dim r As Integer = -999999999

            Dim dsPatientData As New DataSet
            Dim dtPatientData As DataTable
            Dim drPatient As DataRow

            Dim alRules As New ArrayList
            Dim alFacilities As New ArrayList


            Dim strResult As String

            Dim iDataRow As Integer = 0
            Dim strCol As DataColumn
            Dim strTemp As String
            Dim sInvalidElements As String = ""

            'If ((txtAcctCode.Text.Trim = "" Or txtPid.Text.Trim = "")  ddlHospCode.SelectedValue.Trim = "" Or txtQueryDef.Text.Trim = "") Then
            '    strResult = "Hospital code, PatientID, PID and Script text required"
            '    VerifySyntax = strResult
            '    Return strResult
            'End If

            If rdoPAT Then
                dsPatientData = getDummyPatientDataPatient(patientNum, AcountNumberorPID, HOSP_CODE)
            Else
                dsPatientData = getDummyPatientDataPid(AcountNumberorPID, HOSP_CODE)
            End If

            If (Not dsPatientData Is Nothing) Then

                If dsPatientData.Tables.Count > 0 Then

                    If (dsPatientData.Tables(0).Rows.Count > 0) Then

                        dtPatientData = New DataTable(dsPatientData.Tables(0).ToString())

                        Dim arrPatientColumns(dsPatientData.Tables(0).Columns.Count) As String

                        strScript = strScript.Replace("return", "testRule=")

                        For Each dtPatientData In dsPatientData.Tables
                            For Each drPatient In dtPatientData.Rows
                                For Each strCol In dsPatientData.Tables(0).Columns
                                    strTemp = "#" + strCol.ColumnName + "#"
                                    If Not IsDBNull(drPatient(strCol.ColumnName)) Then
                                        strScript = strScript.Replace(strTemp, """" + CStr(drPatient(strCol.ColumnName)) + """")
                                    Else
                                        strScript = strScript.Replace(strTemp, """" + "" + """")
                                    End If
                                Next
                            Next
                        Next

                        ' Regular Expression - exclude "[@<>^&/~#]"
                        '     If (strScript.IndexOf("#") < 0) Then
                        If (strScript.IndexOf("#") < 0 Or (strScript.IndexOf("#") = strScript.IndexOf("#]"))) Then



                            Dim scriptCtl As MSScriptControl.ScriptControl = New MSScriptControl.ScriptControl
                            scriptCtl.Language = "vbscript"
                            scriptCtl.AddCode("Function testRule()" & vbCrLf & strScript & vbCrLf & "End Function")


                            strResult = CStr(scriptCtl.Run("testRule"))




                        Else
                            strResult = "-9998"
                        End If
                        ''--------------------
                        'If (ConfigurationManager.AppSettings("ruleInValidDataCheck") = "Y") Then
                        '    If (strScript.IndexOf("#") < 0) Then
                        '        sInvalidElements = testInvalidDataRuleFire(strScript)
                        '    Else
                        '        strResult = "-888888"
                        '    End If

                        'End If
                        ''--------------------
                    Else
                        strResult = "-9997"
                    End If
                Else
                    strResult = "-9999"
                End If



                r = ss.ToInteger(strResult)

                Select Case r

                    Case -999999999
                        _RULE_RESULT_MESSAGE = "strResult to integer failed"
                    Case -9999
                        _RULE_RESULT_MESSAGE = "Not Verified - Insuffcient Data"
                    Case -9998
                        _RULE_RESULT_MESSAGE = "Invalid database column mapping found in Script"
                    Case -9997
                        _RULE_RESULT_MESSAGE = "No Data Exists."
                    Case -9996 To -1001
                        _RULE_RESULT_MESSAGE = "Script run OK with return code: " + strResult
                    Case -1000
                        _RULE_RESULT_MESSAGE = "Scripting Error"
                    Case Is >= 1
                        _RULE_RESULT_MESSAGE = "" 'add return here
                    Case Else
                        _RULE_RESULT_MESSAGE = "Errors in Script, please verify syntax:" + strScriptError + sInvalidElements
                End Select



                'If (strResult = "1000") Then
                If (strResult <> "-1000" And strResult <> "-9998" And strResult <> "-9999" And strResult <> "-9997") Then
                    _RULE_RESULT_MESSAGE = "Script run OK with return code: " + strResult
                Else
                    If (strResult = "-9999") Then

                    ElseIf (strResult = "-9997") Then
                        _RULE_RESULT_MESSAGE = "No Data Exists."
                    ElseIf (strResult = "-9998") Then
                        _RULE_RESULT_MESSAGE = "Invalid database column mapping found in Script"
                    ElseIf (strResult = "-1000") Then
                        _RULE_RESULT_MESSAGE = "Scripting Error"
                    Else
                        _RULE_RESULT_MESSAGE = "Errors in Script, please verify syntax:" + strScriptError + sInvalidElements
                    End If
                End If
            Else
                _RULE_RESULT_MESSAGE = "Not Verified - Invalid Data"
                strResult = "-9999"
            End If ' isNothing





            Return r


        End Function

        Public Function testRulesFireCall(ByVal inputQuery As String) As String
            Try
                Dim scriptCtl As MSScriptControl.ScriptControl = New MSScriptControl.ScriptControl
                scriptCtl.Language = "vbscript"
                scriptCtl.AddCode("Function testRule()" & vbCrLf & inputQuery & vbCrLf & "End Function")
                testRulesFireCall = CStr(scriptCtl.Run("testRule"))
                ' txtRuleText.Text = inputQuery
                'testRulesFireCall = "1000"
            Catch ex As System.Exception
                strScriptError = ex.Message
                testRulesFireCall = "-1000"

            End Try
        End Function


        Public Function PrepRuleVBS(ByVal strScript As String, ByVal dsPatientData As DataSet) As String

            Dim dtPatientData As DataTable
            Dim strTemp As String = String.Empty
            Dim drPatient As DataRow

            Dim strReturn As String = String.Empty

            Dim strCol As DataColumn

            strScript = strScript.Replace("return", "testRule=")

            For Each dtPatientData In dsPatientData.Tables
                For Each drPatient In dtPatientData.Rows
                    For Each strCol In dsPatientData.Tables(0).Columns
                        strTemp = "#" + strCol.ColumnName + "#"
                        If Not IsDBNull(drPatient(strCol.ColumnName)) Then
                            strScript = strScript.Replace(strTemp, """" + CStr(drPatient(strCol.ColumnName)) + """")
                        Else
                            strScript = strScript.Replace(strTemp, """" + "" + """")
                        End If
                    Next
                Next
            Next





            Return strScript
        End Function


        ''' <summary>
        ''' 
        ''' </summary>
        ' ''' <param name="inputQuery"></param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Private Functionxxxxx testRulesFireCall(ByVal inputQuery As String) As String
        '    Try
        'Dim scriptCtl As MSScriptControl.ScriptControl = New MSScriptControl.ScriptControl
        '        scriptCtl.Language = "vbscript"
        '        scriptCtl.AddCode("Function testRule()" & vbCrLf & inputQuery & vbCrLf & "End Function")
        '        testRulesFireCall = CStr(scriptCtl.Run("testRule"))







        '        txtRuleText.Text = inputQuery



        ''testRulesFireCall = "1000"
        '    Catch ex As System.Exception
        '        strScriptError = ex.Message
        '        testRulesFireCall = "-1000"

        '    End Try
        'End Function


        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="strPID"></param>
        ''' <param name="strHospCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function getDummyPatientDataPid(ByVal strPID As String, ByVal strHospCode As String) As DataSet

            '// Dim da As SqlDataAdapter
            Dim dsDummyPatient = New DataSet

            Dim iReturnReocords As Integer


            Using con As New SqlConnection(_ConnectionString)
                con.Open()
                Using cmd As New SqlCommand("usp_Rules_validation", con)

                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@pid", SqlDbType.Decimal).Value = CDec(strPID)


                    Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)

                    dsDummyPatient = New DataSet
                    iReturnReocords = da.Fill(dsDummyPatient, "DUMMY_PATIENT")


                End Using
                con.Close()
            End Using



            Return dsDummyPatient
        End Function



        Public Function getPatientEvents(ByVal patient_number As String, ByVal strHospCode As String) As Dictionary(Of String, String)

            Dim dictionary As New Dictionary(Of String, String)

            Using con As New SqlConnection(_ConnectionString)
                con.Open()
                Using cmd As New SqlCommand("usp_get_patient_events", con)

                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@pid", SqlDbType.Int).Value = ss.ToInteger(patient_number)
                    cmd.Parameters.Add("@patient_number", SqlDbType.VarChar).Value = patient_number
                    cmd.Parameters.Add("@hosp_code", SqlDbType.VarChar).Value = strHospCode

                    cmd.CommandType = CommandType.StoredProcedure
                    Using idr = cmd.ExecuteReader()
                        '          Console.WriteLine("16")

                        If idr.HasRows Then

                            Dim key As String = String.Empty
                            Dim value As String = String.Empty


                            Do While idr.Read
                                'pid
                                'event_type

                                '                Console.WriteLine("18")

                                key = String.Empty
                                value = String.Empty


                                If Not IsDBNull(idr.Item("pid")) Then
                                    key = Convert.ToString(idr.Item("pid"))
                                End If


                                If Not IsDBNull(idr.Item("event_type")) Then
                                    value = Convert.ToString(idr.Item("event_type"))
                                End If



                                dictionary.Add(key, value)



                            Loop
                        End If
                    End Using


                End Using
                con.Close()
            End Using






            Return dictionary



            'Return dsDummyPatient


            'Try
            '    SQLConn.ConnectionString = HttpContext.Current.Application("ConnectionString") 'Global.GlbConnStr
            '    SQLConn.Open()

            '    ''sqlString = "usp_get_patient_events"
            '    sqlComm = New SqlCommand("usp_get_patient_events", SQLConn)
            '    sqlComm.CommandType = CommandType.StoredProcedure
            '    sqlComm.CommandTimeout = CInt(ConfigurationManager.AppSettings("CommandTimeOut"))


            '    sqlComm.Parameters.Add("@patient_number", SqlDbType.VarChar)
            '    sqlComm.Parameters("@patient_number").Direction = ParameterDirection.Input
            '    sqlComm.Parameters("@patient_number").Value = txtAcctCode.Text

            '    sqlComm.Parameters.Add("@hospital_code", SqlDbType.VarChar)
            '    sqlComm.Parameters("@hospital_code").Direction = ParameterDirection.Input
            '    sqlComm.Parameters("@hospital_code").Value = ddlHospCode.SelectedValue

            '    sqlReader = sqlComm.ExecuteReader()

            '    ddlPatientEvents.Items.Clear()

            '    eventlstItem = New ListItem("Current Event", "0")
            '    ddlPatientEvents.Items.Add(eventlstItem)

            '    While sqlReader.Read()
            '        If Not IsDBNull(Trim(sqlReader.GetValue(0))) Then
            '            eventlstItem = New ListItem(Global.GeneralFuncs.ReplaceQuote(Trim(sqlReader.GetValue(1)), "'", "''"), Global.GeneralFuncs.ReplaceQuote(Trim(sqlReader.GetValue(0)), "'", "''"))
            '            ddlPatientEvents.Items.Add(eventlstItem)
            '            'dicHospCodes.Add(Global.GeneralFuncs.ReplaceQuote(Trim(hospReader.GetValue(0)), "'", "''"), Global.GeneralFuncs.ReplaceQuote(Trim(hospReader.GetValue(0)), "'", "''"))
            '        End If
            '    End While

            '    'Session("dicHospCodes") = dicHospCodes

            'Catch ex As System.Exception
            '    If Application("debug") = "Y" Then
            '        Response.Write(ex.Message)
            '    Else
            '        Session("stackTrace") = ex.StackTrace
            '        Response.Redirect("../qaGeneral/qaErrors.aspx?errID=1001&Msg=" & ex.Message.Replace(Environment.NewLine, " ") & "&Pg=validateRule.aspx&Md=Rules&Pr=getPatientEvents")
            '    End If
            'Finally
            '    SQLConn.Close()
            '    SQLConn = Nothing
            '    sqlComm = Nothing
            '    sqlReader = Nothing
            'End Try


        End Function


        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="RULE_ID"></param>
        ''' <param name="CONTEXT_ID"></param>
        ''' <param name="PATIENT_NUMBER"></param>
        ''' <param name="PAT_HOSP_CODE"></param>
        ''' <param name="FACILITY_CODE"></param>
        ''' <param name="MESSAGE_ID"></param>
        ''' <param name="OPERATOR_ID"></param>
        ''' <param name="ADMIT_DATE"></param>
        ''' <param name="EVENT_DATE"></param>
        ''' <param name="PATIENT_PID"></param>
        ''' <param name="INS_TYPE"></param>
        ''' <param name="BATCH_ID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function deleteRuleResultsForEachDataRow(ByVal RULE_ID As Integer, ByVal CONTEXT_ID As Integer, ByVal PATIENT_NUMBER As String, _
                                                 ByVal PAT_HOSP_CODE As String, ByVal FACILITY_CODE As String, ByVal MESSAGE_ID As Integer, _
                                                 ByVal OPERATOR_ID As String, ByVal ADMIT_DATE As String, ByVal EVENT_DATE As String, _
                                                 ByVal PATIENT_PID As Integer, ByVal INS_TYPE As String, ByVal BATCH_ID As Integer) As Integer




            'Dim sqlConn As SqlConnection = New SqlConnection
            'Dim ruleComm As SqlCommand
            '    Dim __sqlString As String = String.Empty
            Dim __errMessage As String = String.Empty
            Dim __iReturnReocords As Integer = 0
            Dim __event_datetime As String = String.Empty
            Dim __admit_date As String = String.Empty

            Try

                Dim y As String = String.Empty
                Dim m As String = String.Empty
                Dim d As String = String.Empty
                Dim h As String = String.Empty
                Dim mm As String = String.Empty


                If Not IsDBNull(ADMIT_DATE) And ADMIT_DATE.Length > 11 Then
                    y = ADMIT_DATE.Substring(0, 4)
                    m = ADMIT_DATE.Substring(4, 2)
                    d = ADMIT_DATE.Substring(6, 2)
                    h = ADMIT_DATE.Substring(8, 2)
                    mm = ADMIT_DATE.Substring(10, 2)
                    If (h = "24") Then
                        h = "23"
                        mm = "59"
                    End If
                    __admit_date = y + "-" + m + "-" + d + " " + h + ":" + mm
                ElseIf Not IsDBNull(ADMIT_DATE) And ADMIT_DATE.Length = 8 Then 'subba-020608
                    y = ADMIT_DATE.Substring(0, 4)
                    m = ADMIT_DATE.Substring(4, 2)
                    d = ADMIT_DATE.Substring(6, 2)
                    __admit_date = y + "-" + m + "-" + d + " " + "00" + ":" + "01"
                Else
                    __admit_date = "1/1/1909"  'default value
                End If

                If Not IsDBNull(EVENT_DATE) And EVENT_DATE.Length > 11 Then
                    y = EVENT_DATE.Substring(0, 4)
                    m = EVENT_DATE.Substring(4, 2)
                    d = EVENT_DATE.Substring(6, 2)
                    h = EVENT_DATE.Substring(8, 2)
                    mm = EVENT_DATE.Substring(10, 2)
                    If (h = "24") Then
                        h = "23"
                        mm = "59"
                    End If
                    __event_datetime = y + "-" + m + "-" + d + " " + h + ":" + mm
                Else
                    __event_datetime = "1/1/1908" ' default value
                End If

                'If (sqlConn.State = ConnectionState.Closed) Then
                '    sqlConn.ConnectionString = SQL_CONNECTION_STRING
                '    sqlConn.Open()
                'End If

                '  sqlString = sCfgUspRuleResultDelete 'subba-022509 '"usp_rule_result_delete"
                '   ruleComm = New SqlCommand(sqlString, sqlConn)
                '*********************************************************************************************************


                ''CfgUspRuleResultDelete  '' = System.Configuration.ConfigurationSettings.AppSettings("CfgUspRuleResultDelete").Trim() 'SUBBA-20160708


                '*********************************************************************************************************






                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using ruleComm As New SqlCommand(_CfgUspRuleResultDelete, Con)

                        ruleComm.CommandType = CommandType.StoredProcedure
                        ruleComm.CommandTimeout = _COMMAND_TIMEOUT

                        ruleComm.Parameters.Add("@rule_id", SqlDbType.Int).Value = RULE_ID
                        ruleComm.Parameters.Add("@context_id", SqlDbType.Int).Value = CONTEXT_ID
                        ruleComm.Parameters.Add("@pat_hosp_code", SqlDbType.VarChar, 20).Value = PAT_HOSP_CODE
                        ruleComm.Parameters.Add("@patient_number", SqlDbType.VarChar, 20).Value = PATIENT_NUMBER
                        ruleComm.Parameters.Add("@facility_code", SqlDbType.VarChar, 20).Value = FACILITY_CODE
                        ruleComm.Parameters.Add("@message_id", SqlDbType.Int).Value = MESSAGE_ID
                        ruleComm.Parameters.Add("@operator_id", SqlDbType.VarChar, 20).Value = OPERATOR_ID
                        ruleComm.Parameters.Add("@admit_date", SqlDbType.DateTime).Value = __admit_date
                        ruleComm.Parameters.Add("@event_datetime", SqlDbType.DateTime).Value = __event_datetime
                        ruleComm.Parameters.Add("@patient_pid", SqlDbType.Int).Value = PATIENT_PID
                        ruleComm.Parameters.Add("@ins_type", SqlDbType.VarChar, 5).Value = INS_TYPE
                        ruleComm.Parameters.Add("@Batch_id", SqlDbType.Int).Value = BATCH_ID

                        __iReturnReocords = ruleComm.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using

            Catch sx As SqlException
                __errMessage = sx.Message
                performDBOperation("Update Patient_audit_trail set modified_date = getdate(), error_desc='" + Left(__errMessage, 249) + "' where pat_hosp_code = '" + PAT_HOSP_CODE + "' and patient_pid = " + Convert.ToString(PATIENT_PID))
                log.ExceptionDetails(":ConsoleFireRules-deleteRuleResultsForEachDataRow-usp_rule_result_delete:", sx)
                __iReturnReocords = 0



            Catch ex As Exception
                __errMessage = ex.Message
                performDBOperation("Update Patient_audit_trail set modified_date = getdate(), error_desc='" + Left(__errMessage, 249) + "' where pat_hosp_code = '" + PAT_HOSP_CODE + "' and patient_pid = " + Convert.ToString(PATIENT_PID))
                log.ExceptionDetails(":ConsoleFireRules-deleteRuleResultsForEachDataRow-usp_rule_result_delete:", ex)
                __iReturnReocords = 0


            End Try

            Return __iReturnReocords
        End Function
        'subba-20160711


        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="COMMAND_STRING"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function performDBOperation(ByVal COMMAND_STRING As String) As Integer


            Dim __strErrorMsg As String = String.Empty
            Dim __r As Integer = -1

            Try


                Using _PBcon As New SqlConnection(_ConnectionString)
                    _PBcon.Open()
                    Using _PBcmd As New SqlCommand(COMMAND_STRING, _PBcon)

                        _PBcmd.CommandType = CommandType.Text
                        _PBcmd.ExecuteNonQuery()

                    End Using
                    _PBcon.Close()
                End Using



            Catch sx As SqlException
                __strErrorMsg = sx.Message
                log.ExceptionDetails("performDBOperation", sx)

            Catch ex As Exception
                __strErrorMsg = ex.Message
                log.ExceptionDetails("performDBOperation", ex)
            End Try


            Return __r

        End Function



        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="XML_RULES_DATA"></param>
        ''' <param name="PAT_HOUSE_CODE"></param>
        ''' <param name="PAITENT_PID"></param>
        ''' <param name="PATIENT_ID"></param>
        ''' <param name="CREATE_DATE"></param>
        ''' <param name="PATINET_AUDIT_TRAIL_ID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function insertAllRuleResultsByXML(ByVal XML_RULES_DATA As String, ByVal PAT_HOUSE_CODE As String, ByVal PAITENT_PID As Integer, _
                                           ByVal PATIENT_ID As Integer, ByVal CREATE_DATE As String, _
                                           ByVal PATINET_AUDIT_TRAIL_ID As Integer) As Integer




            '     Dim __sqlString As String = String.Empty
            Dim __errMessage As String = String.Empty
            Dim __iReturnReocords As Integer = 0
            Dim _event_datetime As String = String.Empty


            ''     sqlString = sCfgUspRuleInsertByXmlTank 'subba-022509 '"usp_rule_insert_ByXML_Tank" '''"usp_rule_insert_ByXML_byTank"  'manoj

            Try

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open() 'SUBBA-20160711
                    Using ruleComm As New SqlCommand(_CfgUspRuleInsertByXmlTank, Con)

                        ruleComm.CommandType = CommandType.StoredProcedure
                        ruleComm.CommandTimeout = _COMMAND_TIMEOUT

                        ruleComm.Parameters.Add("@data", SqlDbType.NText).Value = IIf(Len(Trim(XML_RULES_DATA)) > 0, Trim(XML_RULES_DATA), "")
                        ruleComm.Parameters.Add("@pat_hosp_code", SqlDbType.VarChar, 20).Value = PAT_HOUSE_CODE
                        ruleComm.Parameters.Add("@patient_pid", SqlDbType.Int).Value = PAITENT_PID
                        ruleComm.Parameters.Add("@patient_id", SqlDbType.Int).Value = PATIENT_ID
                        ruleComm.Parameters.Add("@create_date", SqlDbType.VarChar, 25).Value = CREATE_DATE
                        ruleComm.Parameters.Add("@patient_audit_trail_id", SqlDbType.Int).Value = PATINET_AUDIT_TRAIL_ID

                        __iReturnReocords = ruleComm.ExecuteNonQuery()
                    End Using
                End Using


            Catch sx As SqlException
                __errMessage = sx.Message
                log.ExceptionDetails("insertAllRuleResultsByXML", sx)
                performDBOperation("Update Patient_audit_trail set modified_date = getdate(), error_desc='" + Left(__errMessage, 249) + "' where pat_hosp_code = '" + PAT_HOUSE_CODE + "' and patient_pid = " + Convert.ToString(PAITENT_PID))
                log.ExceptionDetails(":ConsoleFireRules-:", sx)
                __iReturnReocords = 0

            Catch ex As Exception
                __errMessage = ex.Message
                log.ExceptionDetails("insertAllRuleResultsByXML", ex)
                performDBOperation("Update Patient_audit_trail set modified_date = getdate(), error_desc='" + Left(__errMessage, 249) + "' where pat_hosp_code = '" + PAT_HOUSE_CODE + "' and patient_pid = " + Convert.ToString(PAITENT_PID))
                log.ExceptionDetails(":ConsoleFireRules-:", ex)
                __iReturnReocords = 0

            End Try

            Return __iReturnReocords
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="strPID"></param>
        ''' <param name="strHospCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function getDummyPatientDataPatient(ByVal patientNum As String, ByVal strPID As String, ByVal strHospCode As String) As DataSet



            Dim ruleComm As SqlCommand
            ' Dim sqlString As String
            Dim iReturnReocords As Integer




            '// Dim da As SqlDataAdapter
            Dim dsDummyPatient = New DataSet



            Using con As New SqlConnection(_ConnectionString)
                con.Open()
                Using cmd As New SqlCommand("usp_get_all_data", con)

                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@pid", SqlDbType.Int).Value = ss.ToInteger(strPID)
                    cmd.Parameters.Add("@patientNum", SqlDbType.VarChar).Value = patientNum
                    cmd.Parameters.Add("@hosp_code", SqlDbType.VarChar).Value = strHospCode

                    Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)

                    dsDummyPatient = New DataSet
                    iReturnReocords = da.Fill(dsDummyPatient, "DUMMY_PATIENT")


                End Using
                con.Close()
            End Using

            Return dsDummyPatient
        End Function

        ''' <summary>
        ''' '
        ''' </summary>
        ''' <param name="RULE_ID"></param>
        ''' <param name="CONTEXT_ID"></param>
        ''' <param name="PATINET_NUMBER"></param>
        ''' <param name="PAT_HOSE_CODE"></param>
        ''' <param name="FACILITY_CODE"></param>
        ''' <param name="MESSAGE_ID"></param>
        ''' <param name="OPERATOR_ID"></param>
        ''' <param name="ADMIT_DATE"></param>
        ''' <param name="EVENT_DATE"></param>
        ''' <param name="PATIENT_PID"></param>
        ''' <param name="INS_TYPE"></param>
        ''' <param name="FINACIAL_CLASS"></param>
        ''' <param name="PATIENT_TYPE"></param>
        ''' <param name="PRI_PLAN_NUMBER"></param>
        ''' <param name="SOURCE"></param>
        ''' <param name="EVENT_TYPE"></param>
        ''' <param name="BATCH_ID"></param>
        ''' <param name="FIRST_NAME"></param>
        ''' <param name="LAST_NAME"></param>
        ''' <param name="DISCHARGE_DATE"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function buildXMLRuleInsert(ByVal xml_RULE_ID As Integer, ByVal xml_RULE_CONTEXT_ID As Integer, ByVal xml_PATINET_NUMBER As String, _
                                    ByVal xml_PAT_HOSE_CODE As String, ByVal xml_FACILITY_CODE As String, ByVal xml_MESSAGE_ID As Integer, _
                                    ByVal xml_OPERATOR_ID As String, ByVal xml_ADMIT_DATE As String, ByVal xml_EVENT_DATE As String, _
                                    ByVal xml_PATIENT_PID As Integer, ByVal xml_INS_TYPE As String, ByVal xml_FINACIAL_CLASS As String, _
                                    ByVal xml_PATIENT_TYPE As String, ByVal xml_PRI_PLAN_NUMBER As String, ByVal xml_SOURCE As String, _
                                    ByVal xml_EVENT_TYPE As String, ByVal xml_BATCH_ID As Integer, ByVal xml_FIRST_NAME As String, _
                                    ByVal xml_LAST_NAME As String, ByVal xml_DISCHARGE_DATE As String) As String
            '   Dim ruleConn As SqlConnection = New SqlConnection





            '   Dim __errMessage As String
            Dim __event_datetime As String = String.Empty
            Dim __strXMLBuilder As New StringBuilder

            Try

                Dim __admit_date As String = String.Empty

                Dim y As String = String.Empty
                Dim m As String = String.Empty
                Dim d As String = String.Empty
                Dim h As String = String.Empty
                Dim mm As String = String.Empty

                If Not IsDBNull(xml_ADMIT_DATE) And xml_ADMIT_DATE.Length > 11 Then
                    y = xml_ADMIT_DATE.Substring(0, 4)
                    m = xml_ADMIT_DATE.Substring(4, 2)
                    d = xml_ADMIT_DATE.Substring(6, 2)
                    h = xml_ADMIT_DATE.Substring(8, 2)
                    mm = xml_ADMIT_DATE.Substring(10, 2)
                    If (h = "24") Then
                        h = "23"
                        mm = "59"
                    End If
                    __admit_date = y + "-" + m + "-" + d + " " + h + ":" + mm
                ElseIf Not IsDBNull(__admit_date) And __admit_date.Length = 8 Then  'subba-020608
                    y = xml_ADMIT_DATE.Substring(0, 4)
                    m = xml_ADMIT_DATE.Substring(4, 2)
                    d = xml_ADMIT_DATE.Substring(6, 2)
                    __admit_date = y + "-" + m + "-" + d + " " + "00" + ":" + "01"
                Else
                    __admit_date = "1/1/1909"  ' default value
                End If

                If Not IsDBNull(xml_EVENT_DATE) And xml_EVENT_DATE.Length > 11 Then
                    y = xml_EVENT_DATE.Substring(0, 4)
                    m = xml_EVENT_DATE.Substring(4, 2)
                    d = xml_EVENT_DATE.Substring(6, 2)
                    h = xml_EVENT_DATE.Substring(8, 2)
                    mm = xml_EVENT_DATE.Substring(10, 2)
                    If (h = "24") Then
                        h = "23"
                        mm = "59"
                    End If
                    __event_datetime = y + "-" + m + "-" + d + " " + h + ":" + mm
                ElseIf Not IsDBNull(xml_EVENT_DATE) And xml_EVENT_DATE.Length = 8 Then
                    y = xml_EVENT_DATE.Substring(0, 4)
                    m = xml_EVENT_DATE.Substring(4, 2)
                    d = xml_EVENT_DATE.Substring(6, 2)
                    h = "00"
                    mm = "01"
                    __event_datetime = y + "-" + m + "-" + d + " " + h + ":" + mm
                Else
                    __event_datetime = "1/1/1908" ' default value
                End If

                __strXMLBuilder.Append("<rule_results>")
                __strXMLBuilder.Append("<rule_id>" & xml_RULE_ID.ToString() & "</rule_id>")
                __strXMLBuilder.Append("<context_id>" & xml_RULE_CONTEXT_ID & "</context_id>")
                __strXMLBuilder.Append("<pat_hosp_code>" & xml_PAT_HOSE_CODE & "</pat_hosp_code>")
                __strXMLBuilder.Append("<patient_number>" & xml_PATINET_NUMBER & "</patient_number>")
                __strXMLBuilder.Append("<facility_code>" & xml_FACILITY_CODE & "</facility_code>")
                __strXMLBuilder.Append("<operator_id>" & xml_OPERATOR_ID & "</operator_id>")
                __strXMLBuilder.Append("<admit_date>" & __admit_date & "</admit_date>")
                __strXMLBuilder.Append("<PID>" & xml_PATIENT_PID & "</PID>")
                __strXMLBuilder.Append("<ins_type>" & xml_INS_TYPE & "</ins_type>")
                'strXMLBuilder.Append("<financial_class>" & financial_class & "</financial_class>")
                'strXMLBuilder.Append("<patient_type>" & patient_type & "</patient_type>")
                __strXMLBuilder.Append("<source>" & xml_SOURCE & "</source>")
                __strXMLBuilder.Append("<event_type>" & xml_EVENT_TYPE & "</event_type>")
                __strXMLBuilder.Append("<pri_plan_number>" & xml_PRI_PLAN_NUMBER & "</pri_plan_number>")
                __strXMLBuilder.Append("<batch_id>" & xml_BATCH_ID & "</batch_id>")
                __strXMLBuilder.Append("<message_id>" & xml_MESSAGE_ID & "</message_id>")
                __strXMLBuilder.Append("<modified_date>" & Now().ToString() & "</modified_date>")
                __strXMLBuilder.Append("<event_datetime>" & __event_datetime & "</event_datetime>")
                __strXMLBuilder.Append("<patient_first_name>" & xml_FIRST_NAME & "</patient_first_name>")  'subba-121107
                __strXMLBuilder.Append("<patient_last_name>" & xml_LAST_NAME & "</patient_last_name>")
                __strXMLBuilder.Append("<discharge_date>" & xml_DISCHARGE_DATE & "</discharge_date>")

                __strXMLBuilder.Append("</rule_results>")

            Catch ex As Exception


                log.ExceptionDetails("buildXMLRuleInsert", ex)

            End Try

            Return __strXMLBuilder.ToString()
        End Function



        Public WriteOnly Property PatientAuditTrailID As Integer

            Set(value As Integer)
                _PATIENT_AUDIT_TRAIL_ID = value
            End Set
        End Property


        Public WriteOnly Property PatHospCode As String

            Set(value As String)
                _PAT_HOSP_CODE = value
            End Set

        End Property


        Public WriteOnly Property RunRules As String

            Set(value As String)
                _RUN_RULES = value
            End Set

        End Property



        'Public WriteOnly Property RuleFailed As Boolean


        '    Set(value As Boolean)
        '        _RULE_FAILED = value
        '    End Set

        'End Property

        'Public WriteOnly Property RuleFailedflag As Boolean

        '    Set(value As Boolean)
        '        _RULE_FAILED_FLAG = value
        '    End Set

        'End Property



        Public WriteOnly Property CommandTimeout As Integer

            Set(value As Integer)
                _COMMAND_TIMEOUT = value
            End Set

        End Property


        'Public WriteOnly Property RuleName As String

        '    Set(value As String)
        '        _RULE_NAME = value
        '    End Set

        'End Property




        'Public WriteOnly Property CfgUspRuleResultDelete As Integer

        '    Set(value As Integer)
        '        _TEST_RULE_ERROR_CODE = value
        '    End Set

        'End Property



        Public WriteOnly Property dsRulesToFire As DataSet

            Set(value As DataSet)
                _dsRulesToFire = value
            End Set
        End Property


        Public WriteOnly Property CfgUspRuleInsertByXmlTank As String

            Set(value As String)
                _CfgUspRuleInsertByXmlTank = value
            End Set

        End Property


        Public WriteOnly Property CfgUspRuleResultDelete As String

            Set(value As String)
                _CfgUspRuleResultDelete = value
            End Set

        End Property


        Public WriteOnly Property RuleResultMessage As String

            Set(value As String)
                _RULE_RESULT_MESSAGE = value

            End Set
        End Property


        'Public WriteOnly Property RegionCode As String

        '    Set(value As String)
        '        _REGION_CODE = value
        '    End Set
        'End Property


        'Public WriteOnly Property PrimaryPlanNumber As String

        '    Set(value As String)
        '        _PRIMARY_PLAN_NUMBER = value
        '    End Set
        'End Property

        'Public WriteOnly Property Source As String
        '    Set(value As String)
        '        _SOURCE = value
        '    End Set
        'End Property

        'Public WriteOnly Property InuranceType As String

        '    Set(value As String)
        '        _INSURACE_TYPE = value
        '    End Set
        'End Property



        'Public WriteOnly Property PatHospCode As String

        '    Set(value As String)
        '        _PAT_HOSP_CODE = value
        '    End Set
        'End Property




        'Public WriteOnly Property PatientAccountNumber As String

        '    Set(value As String)
        '        _PATIENT_ACCOUNT_NUMBER = value
        '    End Set
        'End Property


        'Public WriteOnly Property PatientType As String

        '    Set(value As String)
        '        _PATIENT_TYPE = value
        '    End Set
        'End Property




        'Public WriteOnly Property PatientFirstName As String

        '    Set(value As String)
        '        _PATIENT_FIRST_NAME = value
        '    End Set
        'End Property


        'Public WriteOnly Property PatientLastName As String

        '    Set(value As String)
        '        _PATIENT_LAST_NAME = value
        '    End Set
        'End Property



        'Public WriteOnly Property PID As Integer

        '    Set(value As Integer)
        '        _PID = value
        '    End Set
        'End Property



        'Public WriteOnly Property PatientID As Integer

        '    Set(value As Integer)
        '        _PATIENT_ID = value
        '    End Set
        'End Property






        'Public WriteOnly Property FinancialClass As String

        '    Set(value As String)
        '        _FINANCIAL_CLASS = value
        '    End Set

        'End Property





        'Public WriteOnly Property IBatchID As Integer

        '    Set(value As Integer)
        '        _I_BATCH_ID = value
        '    End Set
        'End Property


        'Public WriteOnly Property ClientFacilityCode As String

        '    Set(value As String)
        '        _CLIENT_FACILITY_CODE = value
        '    End Set
        'End Property


        'Public WriteOnly Property AdmitDate As String

        '    Set(value As String)
        '        _ADMIT_DATE = value
        '    End Set
        'End Property


        'Public WriteOnly Property DischargeDate As String

        '    Set(value As String)
        '        _DISCHARGE_DATE = value
        '    End Set
        'End Property


        'Public WriteOnly Property EventDate As String

        '    Set(value As String)
        '        _EVENT_DATE = value
        '    End Set
        'End Property



        'Public WriteOnly Property EventType As String

        '    Set(value As String)
        '        _EVENT_TYPE = value
        '    End Set
        'End Property


        'Public WriteOnly Property EventDetail As String

        '    Set(value As String)
        '        _EVENT_DETAIL = value
        '    End Set
        'End Property

        'Public WriteOnly Property CreateDate As String

        '    Set(value As String)
        '        _CREATE_DATE = value
        '    End Set
        'End Property


        'Public WriteOnly Property ResultAll As String

        '    Set(value As String)
        '        _RESULT_ALL = value
        '    End Set
        'End Property



    End Class
End Namespace

