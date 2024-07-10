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

    Public Class RunViaDLL


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






        Private dss As New DataSets()
        Private log As New logExecption()
        Private sch As New SchedulerLog()
        Private ss As New StringStuff()
        '   Private RA As New GetSettings()
        Private Met As New Metrics()





        Private _dll_URI As String = String.Empty

        Private _ConnectionString As String = String.Empty

        Private sbAllRuleInsert As StringBuilder
        Private dataRowAuditTrail As DataRow
        Private dataAuditTrailTable As New DataTable
        Private dsGetAllTankData As DataSet
        Private filteredRows() As DataRow
        Private _dsRulesToFire As New DataSet



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


        '******************************************************************************************************************************





        '*********************************************************************************************
        '  end parameter list
        '*********************************************************************************************


        Public Function RunAllRules(ByVal dataRulesAuditTrailRow As DataRow) As Integer


            '************************************************************************************************************************************
            '
            '   Begin Rules
            '
            '
            '
            '
            '************************************************************************************************************************************

            'Dim rg As New Guid
            'rg = Met.AddMetricPair("codeblock", "begin rules ")



            Dim dlCol As DataColumn



            If _RUN_RULES = "Y" Then



                'Dim RULE_ID As Integer = 0


                'Dim RULE_NAME As String = String.Empty
                'Dim RULE_DEFINITION As String = String.Empty
                'Dim RULE_COMMAND As String = String.Empty

                'Dim RULE_FAILED As Boolean = False


                'Dim TEST_RULE_ERROR_CODE As Integer = -10000


                'Dim OPERATOR_ID As String = String.Empty
                'Dim DIVISION_CODE As String = String.Empty
                'Dim REGION_CODE As String = String.Empty
                'Dim ADMIT_DATE As String = String.Empty

                'SUBBA-20160802 
                dsGetAllTankData = dss.getAllDataTankByID(_PATIENT_AUDIT_TRAIL_ID, _PAT_HOSP_CODE) 'subba-20160802 'usp_get_all_data_tank_ByID 5629, "023556299990-WIP"

                If dss.NoData Then
                    Return 0
                    Exit Function
                End If








                If Not (IsNothing(dsGetAllTankData)) Then '*******'subba-041110




                    If (dsGetAllTankData.Tables(0).Rows.Count = 1) Then
                        'xx080307 - Manoj- Make sure returns one Row always or skip to next row with logging-  dsGetAllTankData.Tables.Rows.Count



                        '_RULE_ID = 0

                        '_TEST_RULE_ERROR_CODE = -10000

                        '_RULE_NAME = String.Empty
                        '_RULE_DEFINITION = String.Empty
                        '_RULE_COMMAND = String.Empty


                        '_RULE_FAILED = False

                        '_OPERATOR_ID = String.Empty
                        '_DIVISION_CODE = String.Empty
                        '_REGION_CODE = String.Empty
                        '_ADMIT_DATE = String.Empty


                        '******************************************************************************************************************************************
                        ' begin sanity check for nulls
                        '******************************************************************************************************************************************

                        If Not IsDBNull(dsGetAllTankData.Tables(0).Rows(0)("region_code")) Then
                            _REGION_CODE = Convert.ToString(dsGetAllTankData.Tables(0).Rows(0)("region_code"))
                        Else
                            _REGION_CODE = String.Empty
                        End If


                        If Not IsDBNull(dsGetAllTankData.Tables(0).Rows(0)("division_code")) Then
                            _DIVISION_CODE = Convert.ToString(dsGetAllTankData.Tables(0).Rows(0)("division_code"))
                        Else
                            _DIVISION_CODE = String.Empty
                        End If



                        If Not IsDBNull(dsGetAllTankData.Tables(0).Rows(0)("operator_id")) Then
                            _OPERATOR_ID = Convert.ToString(dsGetAllTankData.Tables(0).Rows(0)("operator_id"))

                        Else
                            '  SaveTextToFile("OperatorID is blank:" + Now.ToString + "  strPatient_audit_trail_id:" + Convert.ToString(strPatient_audit_trail_id) + " strPatHospCode: " + strPatHospCode + " strPid:" + strPid.ToString, sErrLogPath, "")
                            _OPERATOR_ID = "XXX"  'xxx 080307 manoj - exception -- log and continue

                        End If




                        If Not IsDBNull(dsGetAllTankData.Tables(0).Rows(0)("admitting_date")) Then
                            _ADMIT_DATE = Convert.ToString(dsGetAllTankData.Tables(0).Rows(0)("admitting_date"))
                        Else
                            _ADMIT_DATE = String.Empty
                        End If



                        If Not IsDBNull(dsGetAllTankData.Tables(0).Rows(0)("patient_number")) Then
                            _PATIENT_ACCOUNT_NUMBER = Convert.ToString(dsGetAllTankData.Tables(0).Rows(0)("patient_number"))

                        Else
                            _PATIENT_ACCOUNT_NUMBER = String.Empty

                        End If

                        '==========s121007=====================



                        If Not IsDBNull(dsGetAllTankData.Tables(0).Rows(0)("patient_last_name")) Then
                            _PATIENT_LAST_NAME = Convert.ToString(dsGetAllTankData.Tables(0).Rows(0)("patient_last_name"))
                        Else
                            _PATIENT_LAST_NAME = String.Empty
                        End If




                        If Not IsDBNull(dsGetAllTankData.Tables(0).Rows(0)("patient_first_name")) Then
                            _PATIENT_FIRST_NAME = Convert.ToString(dsGetAllTankData.Tables(0).Rows(0)("patient_first_name"))
                        Else
                            _PATIENT_FIRST_NAME = String.Empty
                        End If




                        If Not IsDBNull(dsGetAllTankData.Tables(0).Rows(0)("discharge_date")) Then
                            _DISCHARGE_DATE = Convert.ToString(dsGetAllTankData.Tables(0).Rows(0)("discharge_date"))
                        Else
                            _DISCHARGE_DATE = String.Empty
                        End If




                        If (_DISCHARGE_DATE <> "" And IsDate(_DISCHARGE_DATE)) Then
                            Dim dtDischarge As Date = Convert.ToDateTime(_DISCHARGE_DATE)
                            _DISCHARGE_DATE = dtDischarge.ToString("yyyyMMdd")
                        End If
                        '=========e121007========================




                        If Not IsDBNull(dsGetAllTankData.Tables(0).Rows(0)("event_datetime")) Then
                            _EVENT_DATE = Convert.ToString(dsGetAllTankData.Tables(0).Rows(0)("event_datetime"))
                        Else
                            _EVENT_DATE = String.Empty
                        End If

                        '******************************************************************************************************************************************
                        '  end sanity checks
                        '******************************************************************************************************************************************


                        ' check for no rules
                        ' _TotalRulesToFire


                        _INSURACE_TYPE = "0" 'HARDCODED - manoj 080207 
                        'SUBBA-20160708-LOOK


                        If _TOTAL_RULE_TO_FIRE = 0 Then
                            deleteRuleResultsForEachDataRow(_RULE_ID, _RULE_CONTEXT_ID, _
                        _PATIENT_ACCOUNT_NUMBER, _PAT_HOSP_CODE, _CLIENT_FACILITY_CODE, _
                        _TEST_RULE_ERROR_CODE, _OPERATOR_ID, _ADMIT_DATE, _
                        _EVENT_DATE, _PID, _INSURACE_TYPE, _I_BATCH_ID) 'SUBBA-20160708





                            sbAllRuleInsert = New StringBuilder("<root>")

                            sbAllRuleInsert.Append(buildXMLRuleInsert(0, _RULE_CONTEXT_ID, _PATIENT_ACCOUNT_NUMBER, _
                                        _PAT_HOSP_CODE, _CLIENT_FACILITY_CODE, 0, _OPERATOR_ID, _ADMIT_DATE, _
                                        _EVENT_DATE, _PID, _INSURACE_TYPE, _FINANCIAL_CLASS, _PATIENT_TYPE, _
                                        _PRIMARY_PLAN_NUMBER, _SOURCE, _EVENT_TYPE, _I_BATCH_ID, _PATIENT_FIRST_NAME, _
                                        _PATIENT_LAST_NAME, _DISCHARGE_DATE)) ' All Rule passed


                            sbAllRuleInsert.Append("</root>")


                            _I_DATA_RULES = insertAllRuleResultsByXML(sbAllRuleInsert.ToString(), _PAT_HOSP_CODE, _PID, _PATIENT_ID, _CREATE_DATE, _PATIENT_AUDIT_TRAIL_ID)



                        Else






                            '  strHospCode = _CLIENT_FACILITY_CODE


                            Dim filteredRows() As DataRow

                            '' this filetes ddd.getrules to fire
                            Dim sFilterExp As String = "facility_code = '" + _CLIENT_FACILITY_CODE + "' OR " + "facility_code = '" + _REGION_CODE + "'" + " OR " + "facility_code = '" + _DIVISION_CODE + "'"
                            Dim strSort As String = "facility_code desc"





                            '******************************************************************************************************************************************
                            '  begin loop for each rule in 
                            '******************************************************************************************************************************************
                            '   Dim fe As New Guid

                            '  fe = Met.AddMetricPair("codeblock", "begin  for each dataAuditTrailTable ")

                            For Each dataAuditTrailTable In dsGetAllTankData.Tables 'dsRulesToFireFiltered





                                _I_DATA_RULES = 0



                                '******************************************************************************************************************************************
                                '  delete rules for current row
                                '******************************************************************************************************************************************
                                ''_Guid = Met.AddMetricPair("db", "begin  delete rule")

                                'xxx080307 - get all values from dsGetAllTankData.Tables(0).Rows(0)
                                deleteRuleResultsForEachDataRow(_RULE_ID, _RULE_CONTEXT_ID, _
                                                            _PATIENT_ACCOUNT_NUMBER, _PAT_HOSP_CODE, _CLIENT_FACILITY_CODE, _
                                                            _TEST_RULE_ERROR_CODE, _OPERATOR_ID, _ADMIT_DATE, _
                                                            _EVENT_DATE, _PID, _INSURACE_TYPE, _I_BATCH_ID) 'SUBBA-20160708

                                ' Met.AddMetric("db", "end  delete rule", _Guid)
                                '_Guid = Guid.Empty



                                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''




                                '******************************************************************************************************************************************
                                '   start xml root
                                '******************************************************************************************************************************************

                                sbAllRuleInsert = New StringBuilder("<root>")

                                'lookDebug1  -- verify  080307
                                '        If (_sDebugMode = "Y") Then SaveTextToFile("Before FILTER on " + sAssemblyProcessID + "_dataAuditTrailTable: " + Now.ToString + "   " + "   " + Convert.ToString(dsRulesToFire.Tables(0).Rows.Count), sErrLogPath, "")

                                filteredRows = _dsRulesToFire.Tables(0).Select(sFilterExp, strSort)

                                'lookDebug1
                                '         If (_DebugMode = "Y") Then SaveTextToFile("After FILTER on " + sAssemblyProcessID + "_dataRulesFireTable:" + Now.ToString + "   " + "   " + Convert.ToString(filteredRows.GetUpperBound(0)) + " : " + sFilterExp, sErrLogPath, "")











                                '******************************************************************************************************************************************
                                ' begin tiltered rules
                                '******************************************************************************************************************************************
                                ' Dim fr As New Guid

                                ' fr = Met.AddMetricPair("codeblock", "begin  filtered ruels")


                                ' if there are no rules we still need to insert a blank row
                                If (filteredRows.Count = 0) Then

                                Else


                                    For Each dataRulesAuditTrailRow In filteredRows

                                        _RULE_ID = 0
                                        _RULE_NAME = String.Empty
                                        _RULE_DEFINITION = String.Empty
                                        _RULE_FAILED = False



                                        _RULE_ID = Convert.ToInt32(dataRulesAuditTrailRow("rule_id"))
                                        _RULE_NAME = Convert.ToString(dataRulesAuditTrailRow("rule_name"))
                                        _RULE_DEFINITION = Convert.ToString(dataRulesAuditTrailRow("rule_def"))




                                        '_Guid = Met.AddMetricPair("db", "begin   dataRowAuditTrail = dataAuditTrailTable.Rows(0)")

                                        dataRowAuditTrail = dataAuditTrailTable.Rows(0)

                                        'Met.AddMetric("db", "end   dataRowAuditTrail = dataAuditTrailTable.Rows(0)", _Guid)
                                        '_Guid = Guid.Empty



                                        'look-Subba-080607 pri_plan_number ???
                                        If Not IsDBNull(dataRowAuditTrail("pri_ins_number")) Then
                                            _PRIMARY_PLAN_NUMBER = Convert.ToString(dataRowAuditTrail("pri_ins_number"))
                                        Else
                                            _PRIMARY_PLAN_NUMBER = String.Empty
                                        End If

                                        If Not IsDBNull(dataRowAuditTrail("source")) Then
                                            _SOURCE = Convert.ToString(dataRowAuditTrail("source"))
                                        Else
                                            _SOURCE = String.Empty
                                        End If

                                        _INSURACE_TYPE = "0"



                                        '  _Guid = Met.AddMetricPair("codeblock", "rulerplace")

                                        _RULE_COMMAND = _RULE_DEFINITION.Replace("return", "testRule=")



                                        For Each dlCol In dsGetAllTankData.Tables(0).Columns
                                            If Not IsDBNull(dataRowAuditTrail(dlCol.ColumnName)) Then
                                                _RULE_COMMAND = _RULE_COMMAND.Replace("#" + dlCol.ColumnName + "#", """" + Convert.ToString(dataRowAuditTrail(dlCol.ColumnName)) + """")
                                            Else
                                                _RULE_COMMAND = _RULE_COMMAND.Replace("#" + dlCol.ColumnName + "#", """" + "" + """")
                                            End If
                                            If (_RULE_COMMAND.IndexOf("#") < 0) Then Exit For 'look
                                        Next

                                        'Met.AddMetric("codeblock", "end  rulerplace", _Guid)
                                        '_Guid = Guid.Empty


                                        '******************************************************************************************************************************************
                                        ' begin run vbscript rule
                                        '******************************************************************************************************************************************
                                        'Verify VbScipt Syntax 'SUBBA-20160708-LOOK
                                        Try




                                            If (_RULE_COMMAND <> "" And _RULE_COMMAND.IndexOf("##") < 0) Then

                                                '     _Guid = Met.AddMetricPair("rule", "begin  runrule")

                                                scriptVBValue = "Function testRule()" & vbCrLf & _RULE_COMMAND & vbCrLf & "End Function"
                                                _scriptCtl.AddCode(scriptVBValue)
                                                _TEST_RULE_ERROR_CODE = Convert.ToInt32(_scriptCtl.Run("testRule"))

                                                ' Met.AddMetric("rule", "end  runrule", _Guid)
                                                ' _Guid = Guid.Empty







                                                If _TEST_RULE_ERROR_CODE < 0 Then
                                                    _RULE_FAILED = True
                                                    _RULE_FAILED_FLAG = True
                                                    'If (_Verbose = 1) Then
                                                    '    Console.WriteLine("rule failed")
                                                    'End If

                                                End If


                                                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                                                ' SERIOUS WHAT THE f IS THIS WHY ARE WE LOKING FOR THE LEN OF A INT32 CONVERTED TO A STRING AND THEN BACK TO AN INT 32
                                                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                                                If Len(_TEST_RULE_ERROR_CODE) = 0 Then
                                                    _TEST_RULE_ERROR_CODE = -10000
                                                End If

                                                'lookDebug1
                                                'If (_DebugMode = "Y" And Convert.ToInt32(strTestRuleErrodCode) < 0) Then SaveTextToFile("strTestRuleErrodCodeFrom Script:" + Now.ToString + "  ScriptResult:" + Convert.ToString(strTestRuleErrodCode) + " RuleID: " + strRuleID, sErrLogPath, "")

                                                ' 'lookDebug1
                                                '  If (_DebugMode = "Y") Then SaveTextToFile("strTestRuleErrodCode:" + ":ConsoleFireRules-MAIN:" + Now.ToString + "   " + "   " + Convert.ToString(strTestRuleErrodCode), _sErrLogPath, "")




                                                '_Guid = Met.AddMetricPair("XML", "begin  sbAllRuleInsert.Append")


                                                '******************************************************************************************************************************************
                                                ' begin build xml rule
                                                '******************************************************************************************************************************************
                                                If (_RESULT_ALL = "Y") Then
                                                    sbAllRuleInsert.Append(buildXMLRuleInsert(_RULE_ID, _RULE_CONTEXT_ID, _
                                                                                              _PATIENT_ACCOUNT_NUMBER, _PAT_HOSP_CODE, _CLIENT_FACILITY_CODE, _
                                                                                              _TEST_RULE_ERROR_CODE, _
                                                                                              _OPERATOR_ID, _ADMIT_DATE, _EVENT_DATE, _PID, _INSURACE_TYPE, _
                                                                                              _FINANCIAL_CLASS, _PATIENT_TYPE, _PRIMARY_PLAN_NUMBER, _SOURCE, _
                                                                                              _EVENT_TYPE, _I_BATCH_ID, _PATIENT_FIRST_NAME, _PATIENT_LAST_NAME, _
                                                                                              _DISCHARGE_DATE))
                                                    'lookDebug1
                                                    '  If (_DebugMode = "Y") Then SaveTextToFile("sbAllRuleInsert.Append-buildXMLRuleInsert:" + Now.ToString + "   " + Convert.ToString("RuleID:" + strRuleID + " RuleCtxId:" + strRuleContextID + " patAcc:" + strPatientAccount + " strOperatorId:" + strOperatorId), sErrLogPath, "")
                                                Else
                                                    If (_TEST_RULE_ERROR_CODE < 0) Then
                                                        bAllRuleDataRowSuccess = False   'SUBBA-20160711
                                                        sbAllRuleInsert.Append(buildXMLRuleInsert(_RULE_ID, _RULE_CONTEXT_ID, _PATIENT_ACCOUNT_NUMBER, _
                                                                                                  _PAT_HOSP_CODE, _CLIENT_FACILITY_CODE, _TEST_RULE_ERROR_CODE, _
                                                                                                  _OPERATOR_ID, _ADMIT_DATE, _EVENT_DATE, _PID, _
                                                                                                  _INSURACE_TYPE, _FINANCIAL_CLASS, _PATIENT_TYPE, _PRIMARY_PLAN_NUMBER, _
                                                                                                  _SOURCE, _EVENT_TYPE, _I_BATCH_ID, _PATIENT_FIRST_NAME,
                                                                                                  _PATIENT_LAST_NAME, _
                                                                                                  _DISCHARGE_DATE))
                                                        'If (_Verbose > 1) Then
                                                        '    insertToRulesResultsDebug(RULE_ID, _RULE_CONTEXT_ID, _PAT_HOSP_CODE, _
                                                        '                              PATIENT_ACCOUNT_NUMBER, _CLIENT_FACILITY_CODE, OPERATOR_ID, ADMIT_DATE, EVENT_DATE, _
                                                        '                              TEST_RULE_ERROR_CODE, _PID, INSURACE_TYPE, IBatchID)
                                                        'End If
                                                    End If
                                                End If
                                                '******************************************************************************************************************************************
                                                ' end xml rule
                                                '******************************************************************************************************************************************

                                                '     Met.AddMetric("XML", "end  sbAllRuleInsert.Append", _Guid)
                                                '    _Guid = Guid.Empty



                                            Else
                                                _TEST_RULE_ERROR_CODE = -2000
                                                ''RuleFailedFlag = True
                                            End If

                                            'this is actually needs to only run at the end it no rules fail
                                            '_Guid = Met.AddMetricPair("XML", "begin  sbAllRuleInsert.Append(buildXMLRuleInsert... rule failed")
                                            'If RULE_FAILED = False Then
                                            '    sbAllRuleInsert.Append(buildXMLRuleInsert(0, _RULE_CONTEXT_ID, PATIENT_ACCOUNT_NUMBER, _
                                            '                                              _PAT_HOSP_CODE, _CLIENT_FACILITY_CODE, 0, OPERATOR_ID, ADMIT_DATE, _
                                            '                                              EVENT_DATE, _PID, INSURACE_TYPE, FINANCIAL_CLASS, PATIENT_TYPE, _
                                            '                                              PRIMARY_PLAN_NUMBER, SOURCE, _EVENT_TYPE, iBatchID, PATIENT_FIRST_NAME, _
                                            '                                              PATIENT_LAST_NAME, DISCHARGE_DATE)) ' All Rule passed
                                            'End If

                                            '   Met.AddMetric("XML", "end  sbAllRuleInsert.Append(buildXMLRuleInsert... rule failed", _Guid)
                                            '  _Guid = Guid.Empty
                                        Catch ex As Exception

                                            _TEST_RULE_ERROR_CODE = -1000
                                            _RULE_FAILED_FLAG = True

                                            If (ex.Message.IndexOf("InteropServices") > 0) Then
                                                _TEST_RULE_ERROR_CODE = -3000    'subba-20160204
                                                '   If (_DebugMode = "Y") Then SaveTextToFile("strTestRuleErrodCodeEXCEPTION:System.Runtime.InteropServices.COMException" + strPid + ":" + strPatientAccount + ":" + strPatHospCode + ":" + ":" + Now.ToString + "   :" + +":" + scriptCtl.Error.Source + ":" + scriptCtl.Error.Description + ":" + scriptCtl.Error.Column.ToString() + ":" + scriptCtl.Error.Text + " : " + ex.Message.ToString() + "_:" + Left(ex.StackTrace.ToString(), 500) + ":TestRuleErrorCode:" + Convert.ToString(strTestRuleErrodCode), sErrLogPath, "")
                                            End If



                                            ' _Guid = Met.AddMetricPair("XML", "begin  sbAllRuleInsert.Append(buildXMLRuleInsert...  bAllRuleDataRowSuccess = False")

                                            bAllRuleDataRowSuccess = False

                                            sbAllRuleInsert.Append(buildXMLRuleInsert(_RULE_ID, _RULE_CONTEXT_ID, _PATIENT_ACCOUNT_NUMBER, _
                                                                                      _PAT_HOSP_CODE, _CLIENT_FACILITY_CODE, _TEST_RULE_ERROR_CODE, _OPERATOR_ID, _
                                                                                      _ADMIT_DATE, _EVENT_DATE, _PID, _INSURACE_TYPE, _FINANCIAL_CLASS, _
                                                                                      _PATIENT_TYPE, _PRIMARY_PLAN_NUMBER, _SOURCE, _EVENT_TYPE, _I_BATCH_ID, _PATIENT_FIRST_NAME, _
                                                                                        _PATIENT_LAST_NAME, _DISCHARGE_DATE))

                                            '  Met.AddMetric("XML", "end  sbAllRuleInsert.Append(buildXMLRuleInsert... bAllRuleDataRowSuccess = False", _Guid)
                                            '  _Guid = Guid.Empty



                                            'If (_Verbose > 1) Then
                                            '    insertToRulesResultsDebug(RULE_ID, _RULE_CONTEXT_ID, _PAT_HOSP_CODE, _
                                            '                              PATIENT_ACCOUNT_NUMBER, _CLIENT_FACILITY_CODE, OPERATOR_ID, ADMIT_DATE, EVENT_DATE, _
                                            '                              TEST_RULE_ERROR_CODE, _PID, INSURACE_TYPE, IBatchID)
                                            'End If
                                            '     If (_DebugMode = "Y") Then SaveTextToFile("strTestRuleErrodCodeEXCEPTION:" + strPid + ":" + strPatientAccount + ":" + strPatHospCode + ":" + ":" + Now.ToString + "   :" + ex.Message.ToString() + "_:" + Left(ex.StackTrace.ToString(), 500) + ":" + scriptCtl.Error.Source + ":" + scriptCtl.Error.Description + ":" + scriptCtl.Error.Column.ToString() + ":" + scriptCtl.Error.Text + ":TestRuleErrorCode:" + Convert.ToString(strTestRuleErrodCode), sErrLogPath, "") ''subba-20160204
                                        End Try
                                        '******************************************************************************************************************************************
                                        ' end run vbscript rule
                                        '******************************************************************************************************************************************







                                    Next 'For-Each-dataRulesFireRow-In_filteredRows-ends-here

                                End If
                                ' Met.AddMetric("XML", "end  filtered rules", fr)
                                ' fr = Guid.Empty
                                '******************************************************************************************************************************************
                                ' end tiltered rules
                                '******************************************************************************************************************************************





                                '_Guid = Met.AddMetricPair("XML", "begin  sbAllRuleInsert.Append(buildXMLRuleInsert... rule failed")
                                If _RULE_FAILED_FLAG = False Then
                                    sbAllRuleInsert.Append(buildXMLRuleInsert(0, _RULE_CONTEXT_ID, _PATIENT_ACCOUNT_NUMBER, _
                                                                            _PAT_HOSP_CODE, _CLIENT_FACILITY_CODE, 0, _OPERATOR_ID, _ADMIT_DATE, _
                                                                         _EVENT_DATE, _PID, _INSURACE_TYPE, _FINANCIAL_CLASS, _PATIENT_TYPE, _
                                                                         _PRIMARY_PLAN_NUMBER, _SOURCE, _EVENT_TYPE, _I_BATCH_ID, _PATIENT_FIRST_NAME, _
                                                                         _PATIENT_LAST_NAME, _DISCHARGE_DATE)) ' All Rule passed
                                End If

                                'Met.AddMetric("XML", "end  sbAllRuleInsert.Append(buildXMLRuleInsert... rule failed", _Guid)
                                '_Guid = Guid.Empty




                                If (_RULE_SUCCESS = "Y" And bAllRuleDataRowSuccess) Then
                                    'lookDebug1
                                    'If (_DebugMode = "Y") Then
                                    '    '        SaveTextToFile("All Rules passed:" + Now.ToString + "  RuleID:" + Convert.ToString(strRuleID) + " patNum: " + strPatientAccount + " BatchID:" + iBatchID.ToString, sErrLogPath, "")
                                    '    '        SaveTextToFile("************** End of Processed Batch ID:" + iBatchID.ToString() + "*****************************************************************************", sErrLogPath, "")
                                    'End If

                                    'If (_Verbose > 1) Then
                                    '    insertToRulesResultsDebug(0, _RULE_CONTEXT_ID, _PAT_HOSP_CODE, PATIENT_ACCOUNT_NUMBER, _
                                    '                              _CLIENT_FACILITY_CODE, OPERATOR_ID, ADMIT_DATE, EVENT_DATE, TEST_RULE_ERROR_CODE, _
                                    '                              _PID, INSURACE_TYPE, IBatchID)
                                    'End If

                                End If

                                sbAllRuleInsert.Append("</root>")

                                '******************************************************************************************************************************************
                                '  end xml /root
                                '******************************************************************************************************************************************



                                '_Guid = Met.AddMetricPair("db", "begin insertAllRuleResultsByXML")
                                _I_DATA_RULES = insertAllRuleResultsByXML(sbAllRuleInsert.ToString(), _PAT_HOSP_CODE, _PID, _PATIENT_ID, _CREATE_DATE, _PATIENT_AUDIT_TRAIL_ID)
                                ' Met.AddMetric("db", "end insertAllRuleResultsByXML", _Guid)
                                '  _Guid = Guid.Empty





                            Next







                            ' Met.AddMetric("code block", "end for each dataAuditTrailTable ", fe)
                            ' _Guid = Guid.Empty
                            '******************************************************************************************************************************************
                            '  end loop for each rule in 
                            '******************************************************************************************************************************************

                        End If



                    End If ' row.count = 1
                End If '*******'subba-041110


                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


            Else
                dsGetAllTankData = Nothing

            End If

            '     Met.AddMetric("codeblock", "end rules", rg)

            '************************************************************************************************************************************
            '
            '   end Rules
            '
            '
            '
            '
            '************************************************************************************************************************************' by pass run rules 











        End Function
        'Dim dsRulesData As New DataSet
        'Dim dsAuditTrail As New DataSet
        'Dim dataRowAuditTrail As System.Data.DataRow
        ''Dim dataRulesAuditTrailRow As System.Data.DataRow
        'Dim dataAuditTrailTable As System.Data.DataTable
        'Dim dsGetAllTankData As DataSet
        'Dim dsGetAddressTankData As DataSet 'subba-042408

        'Dim dlCol As DataColumn



        'Dim RULE_ID As Integer = 0


        'Dim RULE_NAME As String = String.Empty
        'Dim RULE_DEFINITION As String = String.Empty
        'Dim RULE_COMMAND As String = String.Empty

        'Dim RULE_FAILED As Boolean = False


        'Dim TEST_RULE_ERROR_CODE As Integer = -10000


        'Dim OPERATOR_ID As String = String.Empty
        'Dim DIVISION_CODE As String = String.Empty
        'Dim REGION_CODE As String = String.Empty
        'Dim ADMIT_DATE As String = String.Empty

        ''SUBBA-20160802 
        'dsGetAllTankData = dss.getAllDataTankByID(_PATIENT_AUDIT_TRAIL_ID, _PAT_HOSP_CODE) 'subba-20160802 'usp_get_all_data_tank_ByID 5629, "023556299990-WIP"

        'If dss.NoData Then
        '    Return 0
        '    Exit Function
        'End If





        ''************************************************************************************************************************************
        ''
        ''   Begin Rules
        ''
        ''
        ''
        ''
        ''************************************************************************************************************************************

        'Dim rg As New Guid
        'rg = Met.AddMetricPair("codeblock", "begin rules ")







        'If _RUN_RULES = "Y" Then



        '    Dim RULE_ID As Integer = 0


        '    Dim RULE_NAME As String = String.Empty
        '    Dim RULE_DEFINITION As String = String.Empty
        '    Dim RULE_COMMAND As String = String.Empty

        '    Dim RULE_FAILED As Boolean = False


        '    Dim TEST_RULE_ERROR_CODE As Integer = -10000


        '    Dim OPERATOR_ID As String = String.Empty
        '    Dim DIVISION_CODE As String = String.Empty
        '    Dim REGION_CODE As String = String.Empty
        '    Dim ADMIT_DATE As String = String.Empty

        '    'SUBBA-20160802 
        '    dsGetAllTankData = dss.getAllDataTankByID(_PATIENT_AUDIT_TRAIL_ID, _PAT_HOSP_CODE) 'subba-20160802 'usp_get_all_data_tank_ByID 5629, "023556299990-WIP"

        '    If dss.NoData Then
        '        Return 0
        '        Exit Function
        '    End If








        '    If Not (IsNothing(dsGetAllTankData)) Then '*******'subba-041110




        '        If (dsGetAllTankData.Tables(0).Rows.Count = 1) Then
        '            'xx080307 - Manoj- Make sure returns one Row always or skip to next row with logging-  dsGetAllTankData.Tables.Rows.Count



        '            RULE_ID = 0

        '            TEST_RULE_ERROR_CODE = -10000

        '            RULE_NAME = String.Empty
        '            RULE_DEFINITION = String.Empty
        '            RULE_COMMAND = String.Empty


        '            RULE_FAILED = False

        '            OPERATOR_ID = String.Empty
        '            DIVISION_CODE = String.Empty
        '            REGION_CODE = String.Empty
        '            ADMIT_DATE = String.Empty


        '            '******************************************************************************************************************************************
        '            ' begin sanity check for nulls
        '            '******************************************************************************************************************************************

        '            If Not IsDBNull(dsGetAllTankData.Tables(0).Rows(0)("region_code")) Then
        '                REGION_CODE = Convert.ToString(dsGetAllTankData.Tables(0).Rows(0)("region_code"))
        '            Else
        '                REGION_CODE = String.Empty
        '            End If


        '            If Not IsDBNull(dsGetAllTankData.Tables(0).Rows(0)("division_code")) Then
        '                DIVISION_CODE = Convert.ToString(dsGetAllTankData.Tables(0).Rows(0)("division_code"))
        '            Else
        '                DIVISION_CODE = String.Empty
        '            End If



        '            If Not IsDBNull(dsGetAllTankData.Tables(0).Rows(0)("operator_id")) Then
        '                OPERATOR_ID = Convert.ToString(dsGetAllTankData.Tables(0).Rows(0)("operator_id"))

        '            Else
        '                '  SaveTextToFile("OperatorID is blank:" + Now.ToString + "  strPatient_audit_trail_id:" + Convert.ToString(strPatient_audit_trail_id) + " strPatHospCode: " + strPatHospCode + " strPid:" + strPid.ToString, sErrLogPath, "")
        '                OPERATOR_ID = "XXX"  'xxx 080307 manoj - exception -- log and continue

        '            End If




        '            If Not IsDBNull(dsGetAllTankData.Tables(0).Rows(0)("admitting_date")) Then
        '                ADMIT_DATE = Convert.ToString(dsGetAllTankData.Tables(0).Rows(0)("admitting_date"))
        '            Else
        '                ADMIT_DATE = String.Empty
        '            End If



        '            If Not IsDBNull(dsGetAllTankData.Tables(0).Rows(0)("patient_number")) Then
        '                PATIENT_ACCOUNT_NUMBER = Convert.ToString(dsGetAllTankData.Tables(0).Rows(0)("patient_number"))

        '            Else
        '                PATIENT_ACCOUNT_NUMBER = String.Empty

        '            End If

        '            '==========s121007=====================



        '            If Not IsDBNull(dsGetAllTankData.Tables(0).Rows(0)("patient_last_name")) Then
        '                PATIENT_LAST_NAME = Convert.ToString(dsGetAllTankData.Tables(0).Rows(0)("patient_last_name"))
        '            Else
        '                PATIENT_LAST_NAME = String.Empty
        '            End If




        '            If Not IsDBNull(dsGetAllTankData.Tables(0).Rows(0)("patient_first_name")) Then
        '                PATIENT_FIRST_NAME = Convert.ToString(dsGetAllTankData.Tables(0).Rows(0)("patient_first_name"))
        '            Else
        '                PATIENT_FIRST_NAME = String.Empty
        '            End If




        '            If Not IsDBNull(dsGetAllTankData.Tables(0).Rows(0)("discharge_date")) Then
        '                DISCHARGE_DATE = Convert.ToString(dsGetAllTankData.Tables(0).Rows(0)("discharge_date"))
        '            Else
        '                DISCHARGE_DATE = String.Empty
        '            End If




        '            If (DISCHARGE_DATE <> "" And IsDate(DISCHARGE_DATE)) Then
        '                Dim dtDischarge As Date = Convert.ToDateTime(DISCHARGE_DATE)
        '                DISCHARGE_DATE = dtDischarge.ToString("yyyyMMdd")
        '            End If
        '            '=========e121007========================




        '            If Not IsDBNull(dsGetAllTankData.Tables(0).Rows(0)("event_datetime")) Then
        '                EVENT_DATE = Convert.ToString(dsGetAllTankData.Tables(0).Rows(0)("event_datetime"))
        '            Else
        '                EVENT_DATE = String.Empty
        '            End If

        '            '******************************************************************************************************************************************
        '            '  end sanity checks
        '            '******************************************************************************************************************************************


        '            ' check for no rules
        '            ' _TotalRulesToFire




        '            INSURACE_TYPE = "0" 'HARDCODED - manoj 080207 
        '            'SUBBA-20160708-LOOK


        '            If _TotalRulesToFire = 0 Then
        '                deleteRuleResultsForEachDataRow(RULE_ID, _RULE_CONTEXT_ID, _
        '            PATIENT_ACCOUNT_NUMBER, _PAT_HOSP_CODE, _CLIENT_FACILITY_CODE, _
        '            TEST_RULE_ERROR_CODE, OPERATOR_ID, ADMIT_DATE, _
        '            EVENT_DATE, _PID, INSURACE_TYPE, IBatchID) 'SUBBA-20160708





        '                sbAllRuleInsert = New StringBuilder("<root>")

        '                sbAllRuleInsert.Append(buildXMLRuleInsert(0, _RULE_CONTEXT_ID, PATIENT_ACCOUNT_NUMBER, _
        '                            _PAT_HOSP_CODE, _CLIENT_FACILITY_CODE, 0, OPERATOR_ID, ADMIT_DATE, _
        '                            EVENT_DATE, _PID, INSURACE_TYPE, FINANCIAL_CLASS, PATIENT_TYPE, _
        '                            PRIMARY_PLAN_NUMBER, Source, _EVENT_TYPE, IBatchID, PATIENT_FIRST_NAME, _
        '                            PATIENT_LAST_NAME, DISCHARGE_DATE)) ' All Rule passed


        '                sbAllRuleInsert.Append("</root>")


        '                iDataRules = insertAllRuleResultsByXML(sbAllRuleInsert.ToString(), _PAT_HOSP_CODE, _PID, _PATIENT_ID, CREATE_DATE, _PATIENT_AUDIT_TRAIL_ID)



        '            Else






        '                '  strHospCode = _CLIENT_FACILITY_CODE


        '                Dim filteredRows() As DataRow

        '                '' this filetes ddd.getrules to fire
        '                Dim sFilterExp As String = "facility_code = '" + _CLIENT_FACILITY_CODE + "' OR " + "facility_code = '" + REGION_CODE + "'" + " OR " + "facility_code = '" + DIVISION_CODE + "'"
        '                Dim strSort As String = "facility_code desc"





        '                '******************************************************************************************************************************************
        '                '  begin loop for each rule in 
        '                '******************************************************************************************************************************************
        '                '   Dim fe As New Guid

        '                '  fe = Met.AddMetricPair("codeblock", "begin  for each dataAuditTrailTable ")


        '                For Each dataRulesAuditTrailRow In filteredRows



        '                    _RULE_ID = 0
        '                    _RULE_NAME = String.Empty
        '                    _RULE_DEFINITION = String.Empty
        '                    _RULE_FAILED = False



        '                    _RULE_ID = Convert.ToInt32(dataRulesAuditTrailRow("rule_id"))
        '                    _RULE_NAME = Convert.ToString(dataRulesAuditTrailRow("rule_name"))
        '                    _RULE_DEFINITION = Convert.ToString(dataRulesAuditTrailRow("rule_def"))




        '                    '  _Guid = Met.AddMetricPair("db", "begin   dataRowAuditTrail = dataAuditTrailTable.Rows(0)")

        '                    dataRowAuditTrail = dataAuditTrailTable.Rows(0)

        '                    'Met.AddMetric("db", "end   dataRowAuditTrail = dataAuditTrailTable.Rows(0)", _Guid)
        '                    '_Guid = Guid.Empty



        '                    'look-Subba-080607 pri_plan_number ???
        '                    If Not IsDBNull(dataRowAuditTrail("pri_ins_number")) Then
        '                        _PRIMARY_PLAN_NUMBER = Convert.ToString(dataRowAuditTrail("pri_ins_number"))
        '                    Else
        '                        _PRIMARY_PLAN_NUMBER = String.Empty
        '                    End If

        '                    If Not IsDBNull(dataRowAuditTrail("source")) Then
        '                        _SOURCE = Convert.ToString(dataRowAuditTrail("source"))
        '                    Else
        '                        _SOURCE = String.Empty
        '                    End If

        '                    _INSURACE_TYPE = "0"



        '                    '_Guid = Met.AddMetricPair("codeblock", "rulerplace")

        '                    _RULE_COMMAND = _RULE_DEFINITION.Replace("return", "testRule=")


        '                    For Each dlCol In dsGetAllTankData.Tables(0).Columns
        '                        If Not IsDBNull(dataRowAuditTrail(dlCol.ColumnName)) Then
        '                            _RULE_COMMAND = _RULE_COMMAND.Replace("#" + dlCol.ColumnName + "#", """" + Convert.ToString(dataRowAuditTrail(dlCol.ColumnName)) + """")
        '                        Else
        '                            _RULE_COMMAND = _RULE_COMMAND.Replace("#" + dlCol.ColumnName + "#", """" + "" + """")
        '                        End If
        '                        If (_RULE_COMMAND.IndexOf("#") < 0) Then Exit For 'look
        '                    Next

        '                    'Met.AddMetric("codeblock", "end  rulerplace", _Guid)
        '                    '_Guid = Guid.Empty


        '                    '******************************************************************************************************************************************
        '                    ' begin run vbscript rule
        '                    '******************************************************************************************************************************************
        '                    'Verify VbScipt Syntax 'SUBBA-20160708-LOOK
        '                    Try




        '                        If (_RULE_COMMAND <> "" And _RULE_COMMAND.IndexOf("##") < 0) Then

        '                            '     _Guid = Met.AddMetricPair("rule", "begin  runrule")

        '                            _scriptVBValue = "Function testRule()" & vbCrLf & _RULE_COMMAND & vbCrLf & "End Function"
        '                            _scriptCtl.AddCode(_scriptVBValue)
        '                            _TEST_RULE_ERROR_CODE = Convert.ToInt32(_scriptCtl.Run("testRule"))

        '                            ' Met.AddMetric("rule", "end  runrule", _Guid)
        '                            ' _Guid = Guid.Empty







        '                            If _TEST_RULE_ERROR_CODE < 0 Then
        '                                _RULE_FAILED = True
        '                            End If


        '                            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '                            ' SERIOUS WHAT THE f IS THIS WHY ARE WE LOKING FOR THE LEN OF A INT32 CONVERTED TO A STRING AND THEN BACK TO AN INT 32
        '                            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '                            'If Len(TEST_RULE_ERROR_CODE) = 0 Then

        '                            '    _TEST_RULE_ERROR_CODE = -10000
        '                            'End If

        '                            'lookDebug1
        '                            'If (_DebugMode = "Y" And Convert.ToInt32(strTestRuleErrodCode) < 0) Then SaveTextToFile("strTestRuleErrodCodeFrom Script:" + Now.ToString + "  ScriptResult:" + Convert.ToString(strTestRuleErrodCode) + " RuleID: " + strRuleID, sErrLogPath, "")

        '                            ' 'lookDebug1
        '                            '  If (_DebugMode = "Y") Then SaveTextToFile("strTestRuleErrodCode:" + ":ConsoleFireRules-MAIN:" + Now.ToString + "   " + "   " + Convert.ToString(strTestRuleErrodCode), _sErrLogPath, "")




        '                            '_Guid = Met.AddMetricPair("XML", "begin  sbAllRuleInsert.Append")


        '                            '******************************************************************************************************************************************
        '                            ' begin build xml rule   1331
        '                            '******************************************************************************************************************************************
        '                            If (_RESULT_ALL = "Y") Then
        '                                _SB_ALL_RULE_INSERT.Append(buildXMLRuleInsert(_RULE_ID, _RULE_CONTEXT_ID, _
        '                                                                             _PATIENT_ACCOUNT_NUMBER, _PAT_HOSP_CODE, _CLIENT_FACILITY_CODE, _
        '                                                                             _TEST_RULE_ERROR_CODE, _
        '                                                                             _OPERATOR_ID, _ADMIT_DATE, _EVENT_DATE, _PID, _INSURACE_TYPE, _
        '                                                                             _FINANCIAL_CLASS, _PATIENT_TYPE, _PRIMARY_PLAN_NUMBER, _SOURCE, _
        '                                                                             _EVENT_TYPE, _I_BATCH_ID, _PATIENT_FIRST_NAME, _PATIENT_LAST_NAME, _
        '                                                                             _DISCHARGE_DATE))
        '                                'lookDebug1
        '                                '  If (_DebugMode = "Y") Then SaveTextToFile("sbAllRuleInsert.Append-buildXMLRuleInsert:" + Now.ToString + "   " + Convert.ToString("RuleID:" + strRuleID + " RuleCtxId:" + strRuleContextID + " patAcc:" + strPatientAccount + " strOperatorId:" + strOperatorId), sErrLogPath, "")
        '                            Else
        '                                If (_TEST_RULE_ERROR_CODE < 0) Then
        '                                    _ALL_RULE_DATA_ROW_SUCCESS = False   'SUBBA-20160711
        '                                    _SB_ALL_RULE_INSERT.Append(buildXMLRuleInsert(_RULE_ID, _RULE_CONTEXT_ID, _PATIENT_ACCOUNT_NUMBER, _
        '                                                                              _PAT_HOSP_CODE, _CLIENT_FACILITY_CODE, _TEST_RULE_ERROR_CODE, _
        '                                                                              _OPERATOR_ID, _ADMIT_DATE, _EVENT_DATE, _PID, _
        '                                                                              _INSURACE_TYPE, _FINANCIAL_CLASS, _PATIENT_TYPE, _PRIMARY_PLAN_NUMBER, _
        '                                                                              _SOURCE, _EVENT_TYPE, _I_BATCH_ID, _PATIENT_FIRST_NAME, _PATIENT_LAST_NAME, _
        '                                                                              _DISCHARGE_DATE))
        '                                    'If (_Verbose > 1) Then
        '                                    '    insertToRulesResultsDebug(RULE_ID, _RULE_CONTEXT_ID, _PAT_HOSP_CODE, _
        '                                    '                              PATIENT_ACCOUNT_NUMBER, _CLIENT_FACILITY_CODE, OPERATOR_ID, ADMIT_DATE, EVENT_DATE, _
        '                                    '                              TEST_RULE_ERROR_CODE, _PID, INSURACE_TYPE, iBatchID)
        '                                    'End If
        '                                End If
        '                            End If
        '                            '******************************************************************************************************************************************
        '                            ' end xml rule
        '                            '******************************************************************************************************************************************

        '                            '     Met.AddMetric("XML", "end  sbAllRuleInsert.Append", _Guid)
        '                            '    _Guid = Guid.Empty



        '                        Else
        '                            _TEST_RULE_ERROR_CODE = -2000
        '                            ''RuleFailedFlag = True
        '                        End If

        '                        'this is actually needs to only run at the end it no rules fail
        '                        '_Guid = Met.AddMetricPair("XML", "begin  sbAllRuleInsert.Append(buildXMLRuleInsert... rule failed")
        '                        'If RULE_FAILED = False Then
        '                        '    sbAllRuleInsert.Append(buildXMLRuleInsert(0, _RULE_CONTEXT_ID, PATIENT_ACCOUNT_NUMBER, _
        '                        '                                              _PAT_HOSP_CODE, _CLIENT_FACILITY_CODE, 0, OPERATOR_ID, ADMIT_DATE, _
        '                        '                                              EVENT_DATE, _PID, INSURACE_TYPE, FINANCIAL_CLASS, PATIENT_TYPE, _
        '                        '                                              PRIMARY_PLAN_NUMBER, SOURCE, _EVENT_TYPE, iBatchID, PATIENT_FIRST_NAME, _
        '                        '                                              PATIENT_LAST_NAME, DISCHARGE_DATE)) ' All Rule passed
        '                        'End If

        '                        '   Met.AddMetric("XML", "end  sbAllRuleInsert.Append(buildXMLRuleInsert... rule failed", _Guid)
        '                        '  _Guid = Guid.Empty
        '                    Catch ex As Exception

        '                        _TEST_RULE_ERROR_CODE = -1000
        '                        _RULE_FAILED_FLAG = True

        '                        If (ex.Message.IndexOf("InteropServices") > 0) Then
        '                            _TEST_RULE_ERROR_CODE = -3000    'subba-20160204
        '                            '   If (_DebugMode = "Y") Then SaveTextToFile("strTestRuleErrodCodeEXCEPTION:System.Runtime.InteropServices.COMException" + strPid + ":" + strPatientAccount + ":" + strPatHospCode + ":" + ":" + Now.ToString + "   :" + +":" + scriptCtl.Error.Source + ":" + scriptCtl.Error.Description + ":" + scriptCtl.Error.Column.ToString() + ":" + scriptCtl.Error.Text + " : " + ex.Message.ToString() + "_:" + Left(ex.StackTrace.ToString(), 500) + ":TestRuleErrorCode:" + Convert.ToString(strTestRuleErrodCode), sErrLogPath, "")
        '                        End If



        '                        ' _Guid = Met.AddMetricPair("XML", "begin  sbAllRuleInsert.Append(buildXMLRuleInsert...  bAllRuleDataRowSuccess = False")

        '                        _ALL_RULE_DATA_ROW_SUCCESS = False

        '                        _SB_ALL_RULE_INSERT.Append(buildXMLRuleInsert(_RULE_ID, _RULE_CONTEXT_ID, _PATIENT_ACCOUNT_NUMBER, _
        '                                                                  _PAT_HOSP_CODE, _CLIENT_FACILITY_CODE, _TEST_RULE_ERROR_CODE, _OPERATOR_ID, _
        '                                                                  _ADMIT_DATE, _EVENT_DATE, _PID, _INSURACE_TYPE, _FINANCIAL_CLASS, _
        '                                                                  _PATIENT_TYPE, _PRIMARY_PLAN_NUMBER, _SOURCE, _EVENT_TYPE, _I_BATCH_ID, _PATIENT_FIRST_NAME, _
        '                                                                    _PATIENT_LAST_NAME, _DISCHARGE_DATE))

        '                        '  Met.AddMetric("XML", "end  sbAllRuleInsert.Append(buildXMLRuleInsert... bAllRuleDataRowSuccess = False", _Guid)
        '                        '  _Guid = Guid.Empty



        '                        'If (_Verbose > 1) Then
        '                        '    insertToRulesResultsDebug(RULE_ID, _RULE_CONTEXT_ID, _PAT_HOSP_CODE, _
        '                        '                              PATIENT_ACCOUNT_NUMBER, _CLIENT_FACILITY_CODE, OPERATOR_ID, ADMIT_DATE, EVENT_DATE, _
        '                        '                              TEST_RULE_ERROR_CODE, _PID, INSURACE_TYPE, iBatchID)
        '                        'End If
        '                        '     If (_DebugMode = "Y") Then SaveTextToFile("strTestRuleErrodCodeEXCEPTION:" + strPid + ":" + strPatientAccount + ":" + strPatHospCode + ":" + ":" + Now.ToString + "   :" + ex.Message.ToString() + "_:" + Left(ex.StackTrace.ToString(), 500) + ":" + scriptCtl.Error.Source + ":" + scriptCtl.Error.Description + ":" + scriptCtl.Error.Column.ToString() + ":" + scriptCtl.Error.Text + ":TestRuleErrorCode:" + Convert.ToString(strTestRuleErrodCode), sErrLogPath, "") ''subba-20160204
        '                    End Try
        '                    '******************************************************************************************************************************************
        '                    ' end run vbscript rule
        '                    '******************************************************************************************************************************************

        '                Next





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



        Public WriteOnly Property dll_URI As String

            Set(value As String)
                _dll_URI = value
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


        'Public WriteOnly Property DivisionCode As String

        '    Set(value As String)
        '        _DIVISION_CODE = value

        '    End Set
        'End Property


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

