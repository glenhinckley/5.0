Option Strict On
Option Explicit On





Imports System
Imports System.Data
Imports System.Data.SqlClient
'Imports System.Diagnostics
Imports System.IO
'Imports Microsoft.Win32
Imports System.Text.RegularExpressions
'Imports System.Reflection
Imports MSScriptControl.ScriptControlClass
'Imports System.Configuration
Imports System.Collections
Imports System.Text
'Imports System.Runtime.InteropServices
'mports System.Collections.Specialized.NameValueCollection





'Imports System.Net
'Imports System.Net.Sockets


Imports System.Xml
Imports System.Xml.XPath
Imports System.IO.TextReader
'Imports System.Security.Cryptography


'Imports System.Collections.Specialized
'Imports System.Collections.Generic

'Imports System.Net.Security
'Imports System.Threading
'Imports System.Timers
'Imports System.Web


'Imports System.Data.SqlTypes



'Imports System.Globalization


Imports System.IO.Pipes
Imports System.Security.Principal

Imports DCSGlobal.BusinessRules.Logging
Imports DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff
Imports DCSGlobal.BusinessRules.CoreLibraryII
'Imports DCSGlobal.RealAlert.DCSGlobal.RealAlert

Namespace FireRules

    Public Class modFireRulesAddrv3


        Implements IDisposable
        Private disposedValue As Boolean ' To detect redundant calls

        Private dss As New DataSets()
        Private log As New logExecption()
        Private sch As New SchedulerLog()
        Private ss As New StringStuff()
        '    Private RA As New GetSettings()





        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                End If
                scriptCtl = Nothing
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
                '   RA.ConnectionString = value
            End Set
        End Property






        '*********************************************************************************************
        '  begin parameter list
        '*********************************************************************************************





        'Private _RuleFailed As Boolean = True



        Private _Metrics As Integer = 0
        Private _Monitoring As Integer = 0

        Private _MetricString As String = String.Empty



        Private _JobID As Integer = 0
        Private _iMaxRowsToProcess As Integer = 0
        Private _Verbose As Integer = 0

        Private _LogFilePath As String = String.Empty

        Private _ConnectionString As String = String.Empty
        Private _CommandTimeout As Integer = 180


        Private _AppName As String = String.Empty



        Private _colNameVal As System.Collections.Specialized.NameValueCollection
        Private _htRuleMsgs As Hashtable
        Private scriptCtl As MSScriptControl.ScriptControl = New MSScriptControl.ScriptControl
        Private _dsRulesToFire As New DataSet




        Private _RuleFailed As Boolean = False
        Private bAllRuleDataRowSuccess As Boolean = False

        Private bAvsRunFlag As Boolean
        Private sRaClientIP As String
        'Dim _RuleDebugMode As String
        Private _resultAll As String
        Private _ruleSuccess As String
        Private _ruleDisplayLimit As String





        Private iRowsToProcess As Integer = 0
        Private iTotalRulesToFire As Integer = 0
        '   Private iCmdTimeout As Integer = 180
        Private _DebugMode As String = String.Empty
        Private _ErrorLogPath As String = String.Empty
        Private _bAvsRunFlag As String = String.Empty




        '   Dim dtStart_ts As Date = Now()
        '   Dim dtEnd_ts As Date
        ' Private RULE_MESSAGE_DELIMETER As String = "," '
        Private sRunMode As String = String.Empty



        Private _iRowsToProcess As Integer = 0
        Private _iTotalRulesToFire As Integer = 0
        ' Private _iCmdTimeout As Integer = 180   'subba-080125-yymmdd

        ' property WithEvents Srv As DCSErrorService.ServerClass
        '   property Const ENCRYPT_DECRYPT_KEY As String = "&%#@?,:*#$$($#$#(%%%(%($(#$#(@(##)@$$(*%"
        Private _RULE_MESSAGE_DELIMETER As String = "," 'subba-020108

        ''''Dim _RA_TIMEOUT_MINUTES As String = "1"  'SUBBA-20160802

        Private _AddrUspsFormat As String = String.Empty    'subba-012109
        Private _AddrUspsFormatErr As String = String.Empty 'subba-012109
        Private _AddrSvcPingLogMin As String = String.Empty 'subba-022409
        Private _iAddrSvcPingLogMin As Integer = 10

        'subba-062912-ucla-060508----------
        'Private LICENSE_KEY As String = "21"
        Private _CUSTOMER_ID As String = String.Empty
        Private _LICENSE_KEY As String = String.Empty


        Private CfgUspGetAllData As String = String.Empty 'subba-022409
        Private CfgUspGetAllDataHL7Rows As String = String.Empty 'SUBBA-20160713
        Private GetAllDataSp As String = String.Empty
        Private CfgUspInsertSchedulerlog As String = String.Empty
        Private CfgUspGetRuleMsg As String = String.Empty
        Private CfgUspRulesToFire As String = String.Empty
        Private CfgUspApplyPatAuditTrial As String = String.Empty
        Private CfgUspTankAddress As String = String.Empty
        Private CfgUspAddCorrectAddress As String = String.Empty
        Private CfgUspAddMultipleAddress As String = String.Empty
        Private CfgUspFormatAddress As String = String.Empty
        Private CfgUspAddressValidationTrail As String = String.Empty
        Private CfgUspGetAllTankById As String = String.Empty
        Private CfgUspRuleInsert As String = String.Empty
        Private CfgUspRuleResultDelete As String = String.Empty
        Private CfgUspRuleInsertByXmlTank As String = String.Empty
        Private CfgUspRuleInsertDebug As String = String.Empty
        Private CfgHospCode As String = String.Empty
        'Private CfgContextID As String = String.Empty


        Private _RULE_CONTEXT_ID As Integer = 0



        Private sAssemblyProcessID As String = String.Empty

        Private CfgAddrSvcUrl As String = "https://address.auditlogix.net/dcsaddress.asmx" 'subba-062312
        Private CfgAddrSvcUrl1 As String = String.Empty 'subba-062312
        Private CfgAddrSvcUrl2 As String = String.Empty 'subba-062312
        Private CfgAddrSvcUrl3 As String = String.Empty 'subba-062312
        Private CfgAddrSvcLoop As String = String.Empty 'subba-062312







        ''''Dim RA_TIMEOUT_MINUTES As String = "1" 'SUBBA-20160802



        Private _PATIENT_AUDIT_TRAIL_ID As Integer = 0
        Private _PAT_HOSP_CODE As String = String.Empty
        '  Private _HOSP_CODE As String
        Private _PID As Integer = 0
        Private _PATIENT_ID As Integer = 0
        Private _EVENT_DETAIL As String = String.Empty
        Private _EVENT_TYPE As String = String.Empty
        Private _CLIENT_FACILITY_CODE As String = String.Empty
        Private _CREATE_DATE As String = String.Empty
        Private _ADMITTING_DATE As String = String.Empty
        Private _DISCHARGE_DATE As String = String.Empty

        Private _MED_RECORD_NUMBER As String = String.Empty
        Private _RUN_AVS_OUT As String = String.Empty



        Private _AddressFilter As String = String.Empty
        Private _GetMultipleAddress As String = String.Empty

        Private _AddressRunMode As String = String.Empty



        '*********************************************************************************************
        '  end parameter list
        '*********************************************************************************************











        'SUBBA-20160708 'SUBBA-20160713
        Public Function Go(ByVal PATIENT_AUDIT_TRAIL_ID As Integer) As Integer    '(ByVal Args() As String)




            Dim dsRulesData As New DataSet
            Dim dsAuditTrail As New DataSet
            Dim dataRowAuditTrail As System.Data.DataRow
            Dim dataRulesAuditTrailRow As System.Data.DataRow
            Dim dataAuditTrailTable As System.Data.DataTable
            Dim dsGetAllTankData As DataSet
            Dim dsGetAddressTankData As DataSet 'subba-042408

            Dim dlCol As DataColumn


            Dim alRules As New ArrayList
            Dim alFacilities As New ArrayList




            Dim r As Integer = -1



            Dim _GetRaMsgDelimeter As String = String.Empty
            Dim _isAesAddrSuccess As Boolean = False
            Dim _strPat_hosp_code As String = String.Empty

            Dim _strVerifyAddr As String = String.Empty

            ' NEED TO GET THIS FROM CONFIG 
            Dim strContextDesc As String = "short summary"



            Dim _sAddrXmlOut As String = String.Empty
            Dim _AddrTypeFilter As String = _AddressFilter
            Dim _AddrGetMultipleAddress As String = _GetMultipleAddress





            Dim iDataRules As Integer


            Dim PATIENT_FIRST_NAME As String = String.Empty
            Dim PATIENT_LAST_NAME As String = String.Empty
            Dim EVENT_DATE As String = String.Empty
            Dim INSURACE_TYPE As String = String.Empty

            Dim CREATE_DATE As String = String.Empty
            '   Dim strPatient_audit_trail_id As String = String.Empty
            ' Dim strEvent_type As String = String.Empty
            '  Dim strEventDetail As String = String.Empty
            Dim SOURCE As String = String.Empty
            'Dim strPatient_last_name As String = String.Empty
            'Dim strPatient_first_name As String = String.Empty
            Dim DISCHARGE_DATE As String 'subba-121107
            Dim PRIMARY_PLAN_NUMBER As String = String.Empty
            Dim PATIENT_TYPE As String = String.Empty
            Dim FINANCIAL_CLASS As String = String.Empty
            ' Dim strMed_record_number As String = String.Empty
            ' Dim strPatientAccount As String = String.Empty 'subba-20160429
            'Dim strOperatorId As String = String.Empty
            '  Dim strPid As String = String.Empty



            Dim scriptVBValue As String = String.Empty
            Dim sbAllRuleInsert As StringBuilder
            Dim iReturnId As Integer = 0

            Dim sGetRaUID As String = String.Empty
            Dim sRaGetClientIP As String = String.Empty
            Dim sGetRaCookie As String = String.Empty

            Dim sRunAVSOut As String = "Y" 'subba-082310



            Dim iDataRow As Integer = 0
            Dim iBatchID As Integer = 0
            Dim bAddrServerLive As Boolean = False 'SUBBA-060508p


            Dim _isAttemptsSuccess As Boolean = False



            Dim _scriptVBValue As String = String.Empty



            Dim _RunMode As String = String.Empty
            Dim _Contents As String = String.Empty
            Dim _RaProtocol As String = String.Empty
            Dim _StartTime As String = String.Empty
            '      Dim strRuleContextID As String = String.Empty

            '       Dim strTestRuleErrodCode As String = "-10000" 'String.Empty 'SUBBA-20160708

            Dim sAddrType As String = String.Empty
            Dim sAddressType As String = String.Empty
            ' Dim strHospCode As String = ""


            scriptCtl.Language = "vbscript"



            'this needs to go away
            'Try


            If _CREATE_DATE <> String.Empty Then
                CREATE_DATE = _CREATE_DATE
            End If


            ' strHospCode = CfgHospCode  ' System.Configuration.ConfigurationSettings.AppSettings("cfgHospCode").Trim() 'SUBBA-20160708

            If (_iAddrSvcPingLogMin = 0) Then _iAddrSvcPingLogMin = 10

            Dim sCUSTOMER_ID As String = _CUSTOMER_ID
            Dim sLICENSE_KEY As String = _LICENSE_KEY
            '********************************************************  depricated  *********************************************************************
            '''''LoadRealAlertParams(_RaVersion, _RaServerPort, _RaProtocol, _GetInsertRaDebugFlag, _GetRaMsgDelimeter) ' Reading from DB 'SUBBA-20160802
            'LoadAddressVerifyParams(_AddrTypeFilter, _AddrGetMultipleAddress)
            '*******************************************************************************************************************************************







            Dim PATIENT_ACCOUNT_NUMBER As String = String.Empty
            Dim _RUN_RULES As String = "N"







            If (_GetRaMsgDelimeter.Trim <> "") Then _RULE_MESSAGE_DELIMETER = _GetRaMsgDelimeter.Trim() 'subba-020108


            _bAvsRunFlag = _RUN_AVS_OUT 'SUBBA-20160803

            If (_bAvsRunFlag = "Y") Then
                bAddrServerLive = True 'subba-042911
            End If


            dss.sCfgUspGetAllTankById = CfgUspGetAllTankById
            dss.sCfgUspGetAllData = CfgUspGetAllData
            dss.sCfgUspGetAllDataHL7Rows = CfgUspGetAllDataHL7Rows 'SUBBA-20160713
            dss.sCfgUspRulesToFire = CfgUspRulesToFire
            dss.sCfgUspTankAddress = CfgUspTankAddress
            sAssemblyProcessID = "RULES_MT_CONSOLE_ASSEMBLYID" 'SUBBA-20160802
            iRowsToProcess = _iMaxRowsToProcess ' 1000 'HARDCODED 'SUBBA-20160802 'need to pass mtProgram







            '************************************************************************************************************************************
            '
            '  begin  rules context is this even needed
            '
            '
            '
            '
            '************************************************************************************************************************************
            If (_RULE_CONTEXT_ID <> 0) Then


                ' this was stupid circular reference
                '   dsRulesToFire = _dsRulesToFire


                iBatchID = _JobID


                'strPatient_audit_trail_id = Convert.ToString(_PATIENT_AUDIT_TRAIL_ID)
                ' strPatHospCode = _PAT_HOSP_CODE
                ' strEventDetail = _EVENT_DETAIL
                ' strPatient_id = _PATIENT_ID
                ' strEvent_type = _EVENT_TYPE
                'strHospCode = _CLIENT_FACILITY_CODE
                '  _strPid = _PID
                '  strPid = _strPid
                '   strMed_record_number = _MED_RECORD_NUMBER
                ' sRunAVSOut = _RUN_AVS_OUT

                iReturnId = getDataByPatientAuditTrailByPatientID(_PATIENT_AUDIT_TRAIL_ID, _PAT_HOSP_CODE, _EVENT_DETAIL, _EVENT_TYPE, _
                                                                  _CLIENT_FACILITY_CODE, _PID, _PATIENT_ID, _MED_RECORD_NUMBER, _
                                                                  sRunAVSOut, _RUN_RULES) 'SUBBA-20160708 'subba-20160429


                If (_Verbose = 1) Then
                    System.Console.WriteLine("Begin Rules for Patinet audit trail id  " + Convert.ToString(_PATIENT_AUDIT_TRAIL_ID) + " start")
                End If


                '************************************************************************************************************************************
                '
                '   begin adress verification
                '
                '
                '
                '
                '************************************************************************************************************************************
                Try

                    If (sRunAVSOut = "Y") Then






                        dsGetAddressTankData = dss.getAddresDataTankByHospCode(_PAT_HOSP_CODE)  '--[usp_get_tank_address]

                        If Not (IsNothing(dsGetAddressTankData)) Then '*******'subba-041110

                            Try

                                ' If (_bAvsRunFlag = "Y" And _AddressRunMode = "Y") Then


                                If (sRunAVSOut = "Y") Then

                                    If Not IsDBNull(dsGetAddressTankData.Tables(0).Rows(0)("verify_address")) Then
                                        _strVerifyAddr = Convert.ToString(dsGetAddressTankData.Tables(0).Rows(0)("verify_address"))
                                    Else
                                        _strVerifyAddr = "" 'Subba-20160803
                                    End If



                                    '    If (sRunAVSOut = "Y") Then

                                    If (_strVerifyAddr = "Y" And _AddressRunMode = "Y") Then 'SUBBA-060508

                                        If ((_AddrTypeFilter.Trim() <> "")) And (_AddrTypeFilter.Split(","c).Length > 0) Then
                                            If (_Verbose = 1) Then
                                                System.Console.Write("Begin address for  + " + Convert.ToString(_PATIENT_AUDIT_TRAIL_ID) + " start")
                                            End If


                                            ' begin for each loop to loop thru   _AddrType
                                            ' cahange from nested if to select case neted ifs or ver expensive
                                            ' AND PAIN IN THE ASS TO FLOOW
                                            ' ALSO IF THEN IS FOR EVALUATION NOT PROGRAM FLOW CONTROL
                                            'COM ON PEOPLE LEARN SOMETHIGN NEW
                                            For Each _AddrType As String In _AddrTypeFilter.Split(CChar(",")) 'order G, P,C 'subba-101813


                                                Select Case (_AddrType)

                                                    Case "G"
                                                        If (_Verbose = 1) Then
                                                            System.Console.Write("adress  G" + Convert.ToString(_PATIENT_AUDIT_TRAIL_ID) + " start")
                                                        End If


                                                        Using G As New Address

                                                            Try
                                                                G.Clear()

                                                                G.AddressType = "Guarantor"

                                                                If Not IsDBNull(dsGetAddressTankData.Tables(0).Rows(0)("guarantor_street_address")) Then
                                                                    G.StreetAddress = Convert.ToString(dsGetAddressTankData.Tables(0).Rows(0)("guarantor_street_address"))
                                                                Else
                                                                    G.StreetAddress = String.Empty
                                                                End If



                                                                If Not IsDBNull(dsGetAddressTankData.Tables(0).Rows(0)("guarantor_address2")) Then
                                                                    G.StreetAddress2 = Convert.ToString(dsGetAddressTankData.Tables(0).Rows(0)("guarantor_address2"))
                                                                Else
                                                                    G.StreetAddress2 = String.Empty
                                                                End If



                                                                If Not IsDBNull(dsGetAddressTankData.Tables(0).Rows(0)("guarantor_city")) Then
                                                                    G.City = Convert.ToString(dsGetAddressTankData.Tables(0).Rows(0)("guarantor_city"))
                                                                Else
                                                                    G.City = String.Empty
                                                                End If


                                                                If Not IsDBNull(dsGetAddressTankData.Tables(0).Rows(0)("guarantor_state")) Then
                                                                    G.State = Convert.ToString(dsGetAddressTankData.Tables(0).Rows(0)("guarantor_state"))
                                                                Else
                                                                    G.State = String.Empty
                                                                End If




                                                                If Not IsDBNull(dsGetAddressTankData.Tables(0).Rows(0)("guarantor_zip_code")) Then
                                                                    G.ZipCode = Convert.ToString(dsGetAddressTankData.Tables(0).Rows(0)("guarantor_zip_code"))

                                                                Else
                                                                    G.ZipCode = String.Empty
                                                                End If




                                                                ' this mess just needs to get moved to the address class 

                                                                If (G.StreetAddress <> "" And (G.ZipCode <> "" Or G.StreetAddress <> "")) Then
                                                                    ' ''LOOK-HARDCODED-'subba-071513-novant-127833747-FMC-issue-differentInvalidAddressResults
                                                                    ''strPatient_street_address = "MALLARD RIDGE ASSISTED LIVING"
                                                                    ''strPatient_address2 = "9420 N CAROLINA 150"
                                                                    ''strPatient_city = "CLEMMONS"
                                                                    ''strPatient_state = "NC"
                                                                    ''strPatient_zip_code = "27012"
                                                                    _isAesAddrSuccess = verifyAddressSaveAPI(_PAT_HOSP_CODE, G.AddressType, G.StreetAddress, _
                                                                                                             G.StreetAddress2, G.City, G.State, G.ZipCode, _
                                                                                                             _sAddrXmlOut, _PATIENT_ID, _
                                                                                                             _PID, iBatchID) 'subba-060508
                                                                Else
                                                                    'INAVALID-BAD-ADDRESS
                                                                    insertAddressValidatedAuditTrail(_PAT_HOSP_CODE, "I", "InValid Address", "DCSAddress- " + G.AddressType, _
                                                                                                     "ConsoleDCSAddress", G.StreetAddress, G.StreetAddress2, G.City, G.State, G.ZipCode, _
                                                                                                     _PATIENT_ID, _PID, "", iBatchID) 'subba-071513
                                                                End If

                                                            Catch ex As Exception
                                                                'subba-101813

                                                                log.ExceptionDetails("AddressValidationDCSAddressGUARANTOR_verifyAddressSaveAPI()", ex)

                                                                '  If (_DebugMode = "Y") Then insertExceptionDetails(ex.Message, ex.StackTrace, "AddressValidationDCSAddressGUARANTOR_verifyAddressSaveAPI()") 'subba-101813
                                                            End Try


                                                        End Using


                                                    Case "P"




                                                        Using P As New Address

                                                            Try
                                                                P.Clear()

                                                                If (_Verbose = 1) Then
                                                                    System.Console.Write("adress  P" + Convert.ToString(_PATIENT_AUDIT_TRAIL_ID) + " start")
                                                                End If


                                                                P.AddressType = "Patient"
                                                                ' sAddrXmlP = String.Concat("<address><addresstype>Patient</addresstype><address1>", strPatient_street_address, "</address1><address2>", strPatient_address2, "</address2><city>", strPatient_city, "</city><state>", strPatient_state, "</state><zip>", strPatient_zip_code, "</zip><dpvnotes>", "", "</dpvnotes></address>")

                                                                If Not IsDBNull(dsGetAddressTankData.Tables(0).Rows(0)("patient_street_address")) Then
                                                                    P.StreetAddress = Convert.ToString(dsGetAddressTankData.Tables(0).Rows(0)("patient_street_address"))
                                                                Else
                                                                    P.StreetAddress = String.Empty
                                                                End If


                                                                If Not IsDBNull(dsGetAddressTankData.Tables(0).Rows(0)("patient_address2")) Then
                                                                    P.StreetAddress2 = Convert.ToString(dsGetAddressTankData.Tables(0).Rows(0)("patient_address2"))
                                                                Else
                                                                    P.StreetAddress2 = String.Empty
                                                                End If



                                                                If Not IsDBNull(dsGetAddressTankData.Tables(0).Rows(0)("patient_city")) Then
                                                                    P.City = Convert.ToString(dsGetAddressTankData.Tables(0).Rows(0)("patient_city"))
                                                                Else
                                                                    P.City = String.Empty
                                                                End If


                                                                If Not IsDBNull(dsGetAddressTankData.Tables(0).Rows(0)("patient_state")) Then
                                                                    P.State = Convert.ToString(dsGetAddressTankData.Tables(0).Rows(0)("patient_state"))
                                                                Else
                                                                    P.State = String.Empty
                                                                End If


                                                                If Not IsDBNull(dsGetAddressTankData.Tables(0).Rows(0)("patient_zip_code")) Then
                                                                    P.ZipCode = Convert.ToString(dsGetAddressTankData.Tables(0).Rows(0)("patient_zip_code"))

                                                                Else
                                                                    P.ZipCode = String.Empty
                                                                End If




                                                                ' this mess just needs to get moved to the address class 

                                                                If (P.StreetAddress <> "" And (P.ZipCode <> "" Or P.StreetAddress <> "")) Then
                                                                    ' ''LOOK-HARDCODED-'subba-071513-novant-127833747-FMC-issue-differentInvalidAddressResults
                                                                    ''strPatient_street_address = "MALLARD RIDGE ASSISTED LIVING"
                                                                    ''strPatient_address2 = "9420 N CAROLINA 150"
                                                                    ''strPatient_city = "CLEMMONS"
                                                                    ''strPatient_state = "NC"
                                                                    ''strPatient_zip_code = "27012"
                                                                    _isAesAddrSuccess = verifyAddressSaveAPI(_PAT_HOSP_CODE, P.AddressType, P.StreetAddress, _
                                                                                                             P.StreetAddress2, P.City, P.State, P.ZipCode, _
                                                                                                             _sAddrXmlOut, _PATIENT_ID, _
                                                                                                             _PID, iBatchID) 'subba-060508
                                                                Else
                                                                    'INAVALID-BAD-ADDRESS
                                                                    insertAddressValidatedAuditTrail(_PAT_HOSP_CODE, "I", "InValid Address", "DCSAddress- " + P.AddressType, _
                                                                                                     "ConsoleDCSAddress", P.StreetAddress, P.StreetAddress2, P.City, P.State, P.ZipCode, _
                                                                                                     _PATIENT_ID, _PID, "", iBatchID) 'subba-071513
                                                                End If





                                                            Catch ex As Exception
                                                                'subba-101813
                                                                log.ExceptionDetails("AddressValidationDCSAddressPATIENT_verifyAddressSaveAPI()", ex) 'subba-101813
                                                            End Try

                                                        End Using



                                                    Case "C"

                                                        Using C As New Address

                                                            Try
                                                                C.Clear()


                                                                If (_Verbose = 1) Then
                                                                    System.Console.Write("adress  C" + Convert.ToString(_PATIENT_AUDIT_TRAIL_ID) + " start")
                                                                End If

                                                                sAddressType = "Contact"

                                                                If Not IsDBNull(dsGetAddressTankData.Tables(0).Rows(0)("contact_street_address")) Then
                                                                    C.StreetAddress = Convert.ToString(dsGetAddressTankData.Tables(0).Rows(0)("contact_street_address"))
                                                                Else
                                                                    C.StreetAddress = String.Empty
                                                                End If
                                                                If Not IsDBNull(dsGetAddressTankData.Tables(0).Rows(0)("contact_address2")) Then
                                                                    C.StreetAddress2 = Convert.ToString(dsGetAddressTankData.Tables(0).Rows(0)("contact_address2"))
                                                                Else
                                                                    C.StreetAddress2 = String.Empty
                                                                End If
                                                                If Not IsDBNull(dsGetAddressTankData.Tables(0).Rows(0)("contact_city")) Then
                                                                    C.City = Convert.ToString(dsGetAddressTankData.Tables(0).Rows(0)("contact_city"))
                                                                Else
                                                                    C.City = String.Empty
                                                                End If
                                                                If Not IsDBNull(dsGetAddressTankData.Tables(0).Rows(0)("contact_state")) Then
                                                                    C.State = Convert.ToString(dsGetAddressTankData.Tables(0).Rows(0)("contact_state"))
                                                                Else
                                                                    C.State = String.Empty
                                                                End If
                                                                If Not IsDBNull(dsGetAddressTankData.Tables(0).Rows(0)("contact_zip_code")) Then
                                                                    C.ZipCode = Convert.ToString(dsGetAddressTankData.Tables(0).Rows(0)("contact_zip_code"))

                                                                Else
                                                                    C.ZipCode = String.Empty
                                                                End If



                                                                ' this mess just needs to get moved to the address class 

                                                                If (C.StreetAddress <> "" And (C.ZipCode <> "" Or C.StreetAddress <> "")) Then
                                                                    ' ''LOOK-HARDCODED-'subba-071513-novant-127833747-FMC-issue-differentInvalidAddressResults
                                                                    ''strPatient_street_address = "MALLARD RIDGE ASSISTED LIVING"
                                                                    ''strPatient_address2 = "9420 N CAROLINA 150"
                                                                    ''strPatient_city = "CLEMMONS"
                                                                    ''strPatient_state = "NC"
                                                                    ''strPatient_zip_code = "27012"
                                                                    _isAesAddrSuccess = verifyAddressSaveAPI(_PAT_HOSP_CODE, C.AddressType, C.StreetAddress, _
                                                                                                             C.StreetAddress2, C.City, C.State, C.ZipCode, _
                                                                                                             _sAddrXmlOut, _PATIENT_ID, _
                                                                                                             _PID, iBatchID) 'subba-060508
                                                                Else
                                                                    'INAVALID-BAD-ADDRESS
                                                                    insertAddressValidatedAuditTrail(_PAT_HOSP_CODE, "I", "InValid Address", "DCSAddress- " + C.AddressType, _
                                                                                                     "ConsoleDCSAddress", C.StreetAddress, C.StreetAddress2, C.City, C.State, C.ZipCode, _
                                                                                                     _PATIENT_ID, _PID, "", iBatchID) 'subba-071513
                                                                End If






                                                            Catch ex As Exception
                                                                'subba-101813
                                                                log.ExceptionDetails("AddressValidationDCSAddressCONTACT_verifyAddressSaveAPI()", ex) 'subba-101813
                                                            End Try

                                                        End Using


                                                End Select
                                            Next
                                            ' end for each loop to loop thru   _AddrType




                                        End If

                                    End If

                                End If
                            Catch ex As Exception
                                'insertExceptionDetails(ex.Message, ex.StackTrace, "ConsoleFireRulesByTankAddress-verifyAddressSaveAPI-AddressProcessEndsHere-RulesContinue..()")
                                log.ExceptionDetails("ConsoleFireRulesByTankAddress-verifyAddressSaveAPI-AddressProcessEndsHere-RulesContinue..()", ex) 'subba-062912
                            End Try
                        End If  '*******'subba-041110
                    End If '***sRunAVSOut result 'subba-082310

                    If (_Verbose = 1) Then
                        System.Console.Write("adress  for " + Convert.ToString(_PATIENT_AUDIT_TRAIL_ID) + " end")
                    End If


                Catch ex As Exception




                End Try

                '************************************************************************************************************************************
                '
                '   end address verfication
                '
                '
                '
                '
                '************************************************************************************************************************************







                '************************************************************************************************************************************
                '
                '   Begin Rules
                '
                '
                '
                '
                '************************************************************************************************************************************
                If _RUN_RULES = "Y" Then



                    Dim RULE_ID As Integer = 0


                    Dim RULE_NAME As String = String.Empty
                    Dim RULE_DEFINITION As String = String.Empty
                    Dim RULE_COMMAND As String = String.Empty

                    Dim RULE_FAILED As Boolean = False


                    Dim TEST_RULE_ERROR_CODE As Integer = -10000


                    Dim OPERATOR_ID As String = String.Empty
                    Dim DIVISION_CODE As String = String.Empty
                    Dim REGION_CODE As String = String.Empty
                    Dim ADMIT_DATE As String = String.Empty

                    'SUBBA-20160802 
                    dsGetAllTankData = dss.getAllDataTankByID(_PATIENT_AUDIT_TRAIL_ID, _PAT_HOSP_CODE) 'subba-20160802 'usp_get_all_data_tank_ByID 5629, "023556299990-WIP"

                    If Not (IsNothing(dsGetAllTankData)) Then '*******'subba-041110




                        If (dsGetAllTankData.Tables(0).Rows.Count = 1) Then
                            'xx080307 - Manoj- Make sure returns one Row always or skip to next row with logging-  dsGetAllTankData.Tables.Rows.Count



                            RULE_ID = 0

                            TEST_RULE_ERROR_CODE = -10000

                            RULE_NAME = String.Empty
                            RULE_DEFINITION = String.Empty
                            RULE_COMMAND = String.Empty


                            RULE_FAILED = False

                            OPERATOR_ID = String.Empty
                            DIVISION_CODE = String.Empty
                            REGION_CODE = String.Empty
                            ADMIT_DATE = String.Empty


                            '******************************************************************************************************************************************
                            ' begin sanity check for nulls
                            '******************************************************************************************************************************************

                            If Not IsDBNull(dsGetAllTankData.Tables(0).Rows(0)("region_code")) Then
                                REGION_CODE = Convert.ToString(dsGetAllTankData.Tables(0).Rows(0)("region_code"))
                            Else
                                REGION_CODE = String.Empty
                            End If


                            If Not IsDBNull(dsGetAllTankData.Tables(0).Rows(0)("division_code")) Then
                                DIVISION_CODE = Convert.ToString(dsGetAllTankData.Tables(0).Rows(0)("division_code"))
                            Else
                                DIVISION_CODE = String.Empty
                            End If



                            If Not IsDBNull(dsGetAllTankData.Tables(0).Rows(0)("operator_id")) Then
                                OPERATOR_ID = Convert.ToString(dsGetAllTankData.Tables(0).Rows(0)("operator_id"))

                            Else
                                '  SaveTextToFile("OperatorID is blank:" + Now.ToString + "  strPatient_audit_trail_id:" + Convert.ToString(strPatient_audit_trail_id) + " strPatHospCode: " + strPatHospCode + " strPid:" + strPid.ToString, sErrLogPath, "")
                                OPERATOR_ID = "XXX"  'xxx 080307 manoj - exception -- log and continue

                            End If




                            If Not IsDBNull(dsGetAllTankData.Tables(0).Rows(0)("admitting_date")) Then
                                ADMIT_DATE = Convert.ToString(dsGetAllTankData.Tables(0).Rows(0)("admitting_date"))
                            Else
                                ADMIT_DATE = String.Empty
                            End If



                            If Not IsDBNull(dsGetAllTankData.Tables(0).Rows(0)("patient_number")) Then
                                PATIENT_ACCOUNT_NUMBER = Convert.ToString(dsGetAllTankData.Tables(0).Rows(0)("patient_number"))

                            Else
                                PATIENT_ACCOUNT_NUMBER = String.Empty

                            End If

                            '==========s121007=====================



                            If Not IsDBNull(dsGetAllTankData.Tables(0).Rows(0)("patient_last_name")) Then
                                PATIENT_LAST_NAME = Convert.ToString(dsGetAllTankData.Tables(0).Rows(0)("patient_last_name"))
                            Else
                                PATIENT_LAST_NAME = String.Empty
                            End If




                            If Not IsDBNull(dsGetAllTankData.Tables(0).Rows(0)("patient_first_name")) Then
                                PATIENT_FIRST_NAME = Convert.ToString(dsGetAllTankData.Tables(0).Rows(0)("patient_first_name"))
                            Else
                                PATIENT_FIRST_NAME = String.Empty
                            End If




                            If Not IsDBNull(dsGetAllTankData.Tables(0).Rows(0)("discharge_date")) Then
                                DISCHARGE_DATE = Convert.ToString(dsGetAllTankData.Tables(0).Rows(0)("discharge_date"))
                            Else
                                DISCHARGE_DATE = String.Empty
                            End If




                            If (DISCHARGE_DATE <> "" And IsDate(DISCHARGE_DATE)) Then
                                Dim dtDischarge As Date = Convert.ToDateTime(DISCHARGE_DATE)
                                DISCHARGE_DATE = dtDischarge.ToString("yyyyMMdd")
                            End If
                            '=========e121007========================




                            If Not IsDBNull(dsGetAllTankData.Tables(0).Rows(0)("event_datetime")) Then
                                EVENT_DATE = Convert.ToString(dsGetAllTankData.Tables(0).Rows(0)("event_datetime"))
                            Else
                                EVENT_DATE = String.Empty
                            End If

                            '******************************************************************************************************************************************
                            '  end sanity checks
                            '******************************************************************************************************************************************








                            INSURACE_TYPE = "0" 'HARDCODED - manoj 080207 
                            'SUBBA-20160708-LOOK

                            '  strHospCode = _CLIENT_FACILITY_CODE


                            Dim filteredRows() As DataRow

                            '' this filetes ddd.getrules to fire
                            Dim sFilterExp As String = "facility_code = '" + _CLIENT_FACILITY_CODE + "' OR " + "facility_code = '" + REGION_CODE + "'" + " OR " + "facility_code = '" + DIVISION_CODE + "'"
                            Dim strSort As String = "facility_code desc"





                            '******************************************************************************************************************************************
                            '  begin loop for each rule in 
                            '******************************************************************************************************************************************
                            For Each dataAuditTrailTable In dsGetAllTankData.Tables 'dsRulesToFireFiltered





                                iDataRules = 0



                                '******************************************************************************************************************************************
                                '  delete rules for current row
                                '******************************************************************************************************************************************

                                'xxx080307 - get all values from dsGetAllTankData.Tables(0).Rows(0)
                                deleteRuleResultsForEachDataRow(RULE_ID, _RULE_CONTEXT_ID, _
                                                            PATIENT_ACCOUNT_NUMBER, _PAT_HOSP_CODE, _CLIENT_FACILITY_CODE, _
                                                            TEST_RULE_ERROR_CODE, OPERATOR_ID, ADMIT_DATE, _
                                                            EVENT_DATE, _PID, INSURACE_TYPE, iBatchID) 'SUBBA-20160708




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
                                For Each dataRulesAuditTrailRow In filteredRows

                                    RULE_ID = 0
                                    RULE_NAME = String.Empty
                                    RULE_DEFINITION = String.Empty
                                    RULE_FAILED = False



                                    RULE_ID = Convert.ToInt32(dataRulesAuditTrailRow("rule_id"))
                                    RULE_NAME = Convert.ToString(dataRulesAuditTrailRow("rule_name"))
                                    RULE_DEFINITION = Convert.ToString(dataRulesAuditTrailRow("rule_def"))



                                    dataRowAuditTrail = dataAuditTrailTable.Rows(0)

                                    'look-Subba-080607 pri_plan_number ???
                                    If Not IsDBNull(dataRowAuditTrail("pri_ins_number")) Then
                                        PRIMARY_PLAN_NUMBER = Convert.ToString(dataRowAuditTrail("pri_ins_number"))
                                    Else
                                        PRIMARY_PLAN_NUMBER = String.Empty
                                    End If

                                    If Not IsDBNull(dataRowAuditTrail("source")) Then
                                        SOURCE = Convert.ToString(dataRowAuditTrail("source"))
                                    Else
                                        SOURCE = String.Empty
                                    End If

                                    INSURACE_TYPE = "0"





                                    RULE_COMMAND = RULE_DEFINITION.Replace("return", "testRule=")
                                    For Each dlCol In dsGetAllTankData.Tables(0).Columns
                                        If Not IsDBNull(dataRowAuditTrail(dlCol.ColumnName)) Then
                                            RULE_COMMAND = RULE_COMMAND.Replace("#" + dlCol.ColumnName + "#", """" + Convert.ToString(dataRowAuditTrail(dlCol.ColumnName)) + """")
                                        Else
                                            RULE_COMMAND = RULE_COMMAND.Replace("#" + dlCol.ColumnName + "#", """" + "" + """")
                                        End If
                                        If (RULE_COMMAND.IndexOf("#") < 0) Then Exit For 'look
                                    Next




                                    '******************************************************************************************************************************************
                                    ' begin run vbscript rule
                                    '******************************************************************************************************************************************
                                    'Verify VbScipt Syntax 'SUBBA-20160708-LOOK
                                    Try

                                        If (RULE_COMMAND <> "" And RULE_COMMAND.IndexOf("##") < 0) Then
                                            scriptVBValue = "Function testRule()" & vbCrLf & RULE_COMMAND & vbCrLf & "End Function"
                                            scriptCtl.AddCode(scriptVBValue)
                                            TEST_RULE_ERROR_CODE = Convert.ToInt32(scriptCtl.Run("testRule"))



                                            If TEST_RULE_ERROR_CODE < 0 Then
                                                RULE_FAILED = False
                                                If (_Verbose = 1) Then
                                                    Console.WriteLine("rule failed")
                                                End If

                                            End If


                                            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                                            ' SERIOUS WHAT THE f IS THIS WHY ARE WE LOKING FOR THE LEN OF A INT32 CONVERTED TO A STRING AND THEN BACK TO AN INT 32
                                            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                                            If Len(TEST_RULE_ERROR_CODE) = 0 Then
                                                TEST_RULE_ERROR_CODE = -10000
                                            End If

                                            'lookDebug1
                                            'If (_DebugMode = "Y" And Convert.ToInt32(strTestRuleErrodCode) < 0) Then SaveTextToFile("strTestRuleErrodCodeFrom Script:" + Now.ToString + "  ScriptResult:" + Convert.ToString(strTestRuleErrodCode) + " RuleID: " + strRuleID, sErrLogPath, "")

                                            ' 'lookDebug1
                                            '  If (_DebugMode = "Y") Then SaveTextToFile("strTestRuleErrodCode:" + ":ConsoleFireRules-MAIN:" + Now.ToString + "   " + "   " + Convert.ToString(strTestRuleErrodCode), _sErrLogPath, "")






                                            '******************************************************************************************************************************************
                                            ' begin build xml rule
                                            '******************************************************************************************************************************************
                                            If (_resultAll = "Y") Then
                                                sbAllRuleInsert.Append(buildXMLRuleInsert(RULE_ID, _RULE_CONTEXT_ID, _
                                                                                          PATIENT_ACCOUNT_NUMBER, _PAT_HOSP_CODE, _CLIENT_FACILITY_CODE, _
                                                                                          TEST_RULE_ERROR_CODE, _
                                                                                          OPERATOR_ID, ADMIT_DATE, EVENT_DATE, _PID, INSURACE_TYPE, _
                                                                                          FINANCIAL_CLASS, PATIENT_TYPE, PRIMARY_PLAN_NUMBER, SOURCE, _
                                                                                          _EVENT_TYPE, iBatchID, PATIENT_FIRST_NAME, PATIENT_LAST_NAME, _
                                                                                          DISCHARGE_DATE))
                                                'lookDebug1
                                                '  If (_DebugMode = "Y") Then SaveTextToFile("sbAllRuleInsert.Append-buildXMLRuleInsert:" + Now.ToString + "   " + Convert.ToString("RuleID:" + strRuleID + " RuleCtxId:" + strRuleContextID + " patAcc:" + strPatientAccount + " strOperatorId:" + strOperatorId), sErrLogPath, "")
                                            Else
                                                If (TEST_RULE_ERROR_CODE < 0) Then
                                                    bAllRuleDataRowSuccess = False   'SUBBA-20160711
                                                    sbAllRuleInsert.Append(buildXMLRuleInsert(RULE_ID, _RULE_CONTEXT_ID, PATIENT_ACCOUNT_NUMBER, _
                                                                                              _PAT_HOSP_CODE, _CLIENT_FACILITY_CODE, TEST_RULE_ERROR_CODE, _
                                                                                              OPERATOR_ID, ADMIT_DATE, EVENT_DATE, _PID, _
                                                                                              INSURACE_TYPE, FINANCIAL_CLASS, PATIENT_TYPE, PRIMARY_PLAN_NUMBER, _
                                                                                              SOURCE, _EVENT_TYPE, iBatchID, PATIENT_FIRST_NAME, PATIENT_LAST_NAME, _
                                                                                              DISCHARGE_DATE))
                                                    If (_Verbose > 1) Then
                                                        insertToRulesResultsDebug(RULE_ID, _RULE_CONTEXT_ID, _PAT_HOSP_CODE, _
                                                                                  PATIENT_ACCOUNT_NUMBER, _CLIENT_FACILITY_CODE, OPERATOR_ID, ADMIT_DATE, EVENT_DATE, _
                                                                                  TEST_RULE_ERROR_CODE, _PID, INSURACE_TYPE, iBatchID)
                                                    End If
                                                End If
                                            End If
                                            '******************************************************************************************************************************************
                                            ' end xml rule
                                            '******************************************************************************************************************************************





                                        Else
                                            TEST_RULE_ERROR_CODE = -2000
                                        End If


                                    Catch ex As Exception

                                        TEST_RULE_ERROR_CODE = -1000


                                        If (ex.Message.IndexOf("InteropServices") > 0) Then
                                            TEST_RULE_ERROR_CODE = -3000    'subba-20160204
                                            '   If (_DebugMode = "Y") Then SaveTextToFile("strTestRuleErrodCodeEXCEPTION:System.Runtime.InteropServices.COMException" + strPid + ":" + strPatientAccount + ":" + strPatHospCode + ":" + ":" + Now.ToString + "   :" + +":" + scriptCtl.Error.Source + ":" + scriptCtl.Error.Description + ":" + scriptCtl.Error.Column.ToString() + ":" + scriptCtl.Error.Text + " : " + ex.Message.ToString() + "_:" + Left(ex.StackTrace.ToString(), 500) + ":TestRuleErrorCode:" + Convert.ToString(strTestRuleErrodCode), sErrLogPath, "")
                                        End If





                                        bAllRuleDataRowSuccess = False

                                        sbAllRuleInsert.Append(buildXMLRuleInsert(RULE_ID, _RULE_CONTEXT_ID, PATIENT_ACCOUNT_NUMBER, _
                                                                                  _PAT_HOSP_CODE, _CLIENT_FACILITY_CODE, TEST_RULE_ERROR_CODE, OPERATOR_ID, _
                                                                                  ADMIT_DATE, EVENT_DATE, _PID, INSURACE_TYPE, FINANCIAL_CLASS, _
                                                                                  PATIENT_TYPE, PRIMARY_PLAN_NUMBER, SOURCE, _EVENT_TYPE, iBatchID, PATIENT_FIRST_NAME, _
                                                                                  PATIENT_LAST_NAME, DISCHARGE_DATE))

                                        If (_Verbose > 1) Then
                                            insertToRulesResultsDebug(RULE_ID, _RULE_CONTEXT_ID, _PAT_HOSP_CODE, _
                                                                      PATIENT_ACCOUNT_NUMBER, _CLIENT_FACILITY_CODE, OPERATOR_ID, ADMIT_DATE, EVENT_DATE, _
                                                                      TEST_RULE_ERROR_CODE, _PID, INSURACE_TYPE, iBatchID)
                                        End If
                                        '     If (_DebugMode = "Y") Then SaveTextToFile("strTestRuleErrodCodeEXCEPTION:" + strPid + ":" + strPatientAccount + ":" + strPatHospCode + ":" + ":" + Now.ToString + "   :" + ex.Message.ToString() + "_:" + Left(ex.StackTrace.ToString(), 500) + ":" + scriptCtl.Error.Source + ":" + scriptCtl.Error.Description + ":" + scriptCtl.Error.Column.ToString() + ":" + scriptCtl.Error.Text + ":TestRuleErrorCode:" + Convert.ToString(strTestRuleErrodCode), sErrLogPath, "") ''subba-20160204
                                    End Try
                                    '******************************************************************************************************************************************
                                    ' end run vbscript rule
                                    '******************************************************************************************************************************************







                                Next 'For-Each-dataRulesFireRow-In_filteredRows-ends-here
                                '******************************************************************************************************************************************
                                ' end tiltered rules
                                '******************************************************************************************************************************************








                                If RULE_FAILED Then
                                    sbAllRuleInsert.Append(buildXMLRuleInsert(0, _RULE_CONTEXT_ID, PATIENT_ACCOUNT_NUMBER, _
                                                                              _PAT_HOSP_CODE, _CLIENT_FACILITY_CODE, 0, OPERATOR_ID, ADMIT_DATE, _
                                                                              EVENT_DATE, _PID, INSURACE_TYPE, FINANCIAL_CLASS, PATIENT_TYPE, _
                                                                              PRIMARY_PLAN_NUMBER, SOURCE, _EVENT_TYPE, iBatchID, PATIENT_FIRST_NAME, _
                                                                              PATIENT_LAST_NAME, DISCHARGE_DATE)) ' All Rule passed
                                End If






                                If (_ruleSuccess = "Y" And bAllRuleDataRowSuccess) Then
                                    'lookDebug1
                                    'If (_DebugMode = "Y") Then
                                    '    '        SaveTextToFile("All Rules passed:" + Now.ToString + "  RuleID:" + Convert.ToString(strRuleID) + " patNum: " + strPatientAccount + " BatchID:" + iBatchID.ToString, sErrLogPath, "")
                                    '    '        SaveTextToFile("************** End of Processed Batch ID:" + iBatchID.ToString() + "*****************************************************************************", sErrLogPath, "")
                                    'End If

                                    If (_Verbose > 1) Then
                                        insertToRulesResultsDebug(0, _RULE_CONTEXT_ID, _PAT_HOSP_CODE, PATIENT_ACCOUNT_NUMBER, _
                                                                  _CLIENT_FACILITY_CODE, OPERATOR_ID, ADMIT_DATE, EVENT_DATE, TEST_RULE_ERROR_CODE, _
                                                                  _PID, INSURACE_TYPE, iBatchID)
                                    End If

                                End If

                                sbAllRuleInsert.Append("</root>")

                                '******************************************************************************************************************************************
                                '  end xml /root
                                '******************************************************************************************************************************************



                                'realy WTF
                                'strCreate_date = strCreate_date

                                iDataRules = insertAllRuleResultsByXML(sbAllRuleInsert.ToString(), _PAT_HOSP_CODE, _PID, _PATIENT_ID, CREATE_DATE, _PATIENT_AUDIT_TRAIL_ID)






                            Next
                            '******************************************************************************************************************************************
                            '  end loop for each rule in 
                            '******************************************************************************************************************************************





                        End If ' row.count = 1
                    End If '*******'subba-041110





                Else
                    dsGetAllTankData = Nothing

                End If

                '************************************************************************************************************************************
                '
                '   end Rules
                '
                '
                '
                '
                '************************************************************************************************************************************' by pass run rules 






                If (_Verbose = 1) Then
                    System.Console.Write("All done" + Convert.ToString(_PATIENT_AUDIT_TRAIL_ID) + " end")
                End If



            End If
            '************************************************************************************************************************************
            '
            '  end  rules context is this even needed
            '
            '
            '
            '
            '************************************************************************************************************************************


            r = 0



            Return r
        End Function




        Private Function getDataByPatientAuditTrailByPatientID(ByVal PATIENT_AUDIT_TRAIL_ID As Integer, ByVal PAT_HOSP_CODE As String, _
                                                          ByVal EVENT_DETAIL As String, ByVal EVENT_TYPE As String, _
                                                          ByVal CLIENT_FACILITY_CODE As String, ByVal PID As Integer, _
                                                          ByVal PATIENT_ID As Integer, ByVal MRN As String, _
                                                          ByRef RUN_AV_SCOUT As String, ByRef RUN_RULES As String) As Integer





            '    Dim sqlConn As SqlConnection = New SqlConnection
            '   Dim ds As DataSet
            '     Dim ruleComm As SqlCommand
            '   Dim __sqlString As String = String.Empty
            Dim __errMessage As String = String.Empty
            Dim __iAuditId As Integer = 0

            Try
                'If (sqlConn.State = ConnectionState.Closed) Then
                '    sqlConn.ConnectionString = SQL_CONNECTION_STRING
                '    sqlConn.Open()
                'End If

                '       __sqlString = CfgUspApplyPatAuditTrial 'subba-022509 '"usp_apply_patient_audit_trail_by_patient_id"
                ' ruleComm = New SqlCommand(sqlString, sqlConn)




                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using ruleComm As New SqlCommand(CfgUspApplyPatAuditTrial, Con)
                        ruleComm.CommandType = CommandType.StoredProcedure

                        ruleComm.Parameters.Add("@patient_audit_trail_id", SqlDbType.Int).Value = PATIENT_AUDIT_TRAIL_ID
                        ruleComm.Parameters.Add("@pat_hosp_code", SqlDbType.VarChar).Value = PAT_HOSP_CODE
                        ruleComm.Parameters.Add("@event_detail", SqlDbType.VarChar).Value = EVENT_DETAIL
                        ruleComm.Parameters.Add("@event_type", SqlDbType.VarChar).Value = EVENT_TYPE
                        ruleComm.Parameters.Add("@client_facility_code", SqlDbType.VarChar).Value = CLIENT_FACILITY_CODE
                        ruleComm.Parameters.Add("@pid", SqlDbType.Int).Value = PID
                        ruleComm.Parameters.Add("@patient_id", SqlDbType.Int).Value = PATIENT_ID
                        ruleComm.Parameters.Add("@mrn", SqlDbType.VarChar).Value = MRN '

                        ruleComm.Parameters.Add("@run_avs", SqlDbType.VarChar) 'subba-082310
                        ruleComm.Parameters("@run_avs").Direction = ParameterDirection.Output
                        ruleComm.Parameters("@run_avs").Value = String.Empty



                        ruleComm.Parameters.Add("@RUN_RULES", SqlDbType.VarChar) 'subba-082310
                        ruleComm.Parameters("@RUN_RULES").Direction = ParameterDirection.Output
                        ruleComm.Parameters("@RUN_RULES").Value = String.Empty


                        __iAuditId = ruleComm.ExecuteNonQuery()

                        If Not IsDBNull(ruleComm.Parameters("@run_avs").Value) Then 'subba-082310
                            RUN_AV_SCOUT = Convert.ToString(ruleComm.Parameters("@run_avs").Value)
                        Else
                            RUN_AV_SCOUT = String.Empty
                        End If



                        If Not IsDBNull(ruleComm.Parameters("@RUN_RULES").Value) Then 'subba-082310
                            RUN_RULES = Convert.ToString(ruleComm.Parameters("@RUN_RULES").Value)
                        Else
                            RUN_RULES = String.Empty
                        End If

                    End Using
                    Con.Close()
                End Using


            Catch sx As SqlException
                '   __errMessage = sx.Message
                log.ExceptionDetails(":ConsoleFireRules-getDataByPatientAuditTrailByPatientID():", sx)
            Catch ex As Exception
                '  __errMessage = ex.Message
                log.ExceptionDetails(":ConsoleFireRules-getDataByPatientAuditTrailByPatientID():", ex)
            End Try






            Return __iAuditId
            'Return ds
        End Function



        Private Function insertRuleResults(ByVal RULE_ID As Integer, ByVal CONTEXT_ID As Integer, ByVal PATIENT_NUMBER As String, _
                                           ByVal PAT_HOSP_CODE As String, ByVal FACILITY_CODE As String, ByVal MESSAGE_ID As Integer, _
                                           ByVal OPERATOR_ID As String, ByVal ADMIT_DATE As String, ByVal EVENT_DATE As String, _
                                           ByVal PATIENT_PID As Integer, ByVal INS_TYPE As String, ByVal BATCH_ID As Integer) As Integer



            'Dim sqlConn As SqlConnection = New SqlConnection
            'Dim ruleComm As SqlCommand
            '    Dim __sqlString As String = String.Empty
            Dim __errMessage As String = String.Empty
            Dim __iReturnReocords As Integer
            Dim __sbExMsg As StringBuilder
            Dim __admit_date As String = String.Empty
            Dim __event_datetime As String = String.Empty

            Try

                Dim y As String = String.Empty
                Dim m As String = String.Empty
                Dim d As String = String.Empty
                Dim h As String = String.Empty
                Dim mm As String = String.Empty

                Try
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
                        ADMIT_DATE = y + "-" + m + "-" + d + " " + h + ":" + mm
                    ElseIf Not IsDBNull(ADMIT_DATE) And ADMIT_DATE.Length = 8 Then 'subba-020608
                        y = ADMIT_DATE.Substring(0, 4)
                        m = ADMIT_DATE.Substring(4, 2)
                        d = ADMIT_DATE.Substring(6, 2)
                        h = "00"
                        mm = "01"
                        __admit_date = y + "-" + m + "-" + d + " " + h + ":" + mm
                    Else
                        __admit_date = "1/1/1909"  ' default value
                    End If
                Catch ex As Exception
                    log.ExceptionDetails(":ConsoleFireRules-insertRuleResults:" + Now.ToString + "try blcok 1", ex)
                End Try


                Try
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
                    ElseIf Not IsDBNull(EVENT_DATE) And EVENT_DATE.Length = 8 Then '20080208 yyyymmdd 'subba-020608
                        y = EVENT_DATE.Substring(0, 4)
                        m = EVENT_DATE.Substring(4, 2)
                        d = EVENT_DATE.Substring(6, 2)
                        __event_datetime = y + "-" + m + "-" + d + " " + "00" + ":" + "01"
                    Else
                        __event_datetime = "1/1/1908" ' default value
                    End If
                Catch ex As Exception
                    log.ExceptionDetails(":ConsoleFireRules-insertRuleResults:" + Now.ToString + "try blcok 2", ex)
                End Try


                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using ruleComm As New SqlCommand(CfgUspRuleInsert, Con)

                        ruleComm.CommandType = CommandType.StoredProcedure
                        ruleComm.CommandTimeout = _CommandTimeout

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


                __sbExMsg = New StringBuilder
                __sbExMsg.Append("rule_id:" + RULE_ID.ToString() + "|")
                __sbExMsg.Append("context_id:" + CONTEXT_ID.ToString() + "|")
                __sbExMsg.Append("pat_hosp_code:" + PAT_HOSP_CODE.ToString() + "|")
                __sbExMsg.Append("patient_number:" + PATIENT_NUMBER.ToString() + "|")
                __sbExMsg.Append("facility_code:" + FACILITY_CODE.ToString() + "|")
                __sbExMsg.Append("message_id:" + MESSAGE_ID.ToString() + "|")
                __sbExMsg.Append("operator_id:" + OPERATOR_ID.ToString() + "|")
                __sbExMsg.Append("admit_date:" + ADMIT_DATE.ToString() + "|")
                __sbExMsg.Append("event_datetime:" + __event_datetime.ToString() + "|")
                __sbExMsg.Append("patient_pid:" + PATIENT_PID.ToString() + "|")
                __sbExMsg.Append("ins_type:" + INS_TYPE.ToString() + "|")
                __sbExMsg.Append("Batch_id:" + BATCH_ID.ToString() + "|")
                performDBOperation("Update Patient_audit_trail set modified_date = getdate(), error_desc='" + Left(__errMessage, 249) + "' where pat_hosp_code = '" + PAT_HOSP_CODE + "' and patient_pid = " + Convert.ToString(PATIENT_PID))

                log.ExceptionDetails(":ConsoleFireRules-insertRuleResults:", sx)





                __iReturnReocords = 0


            Catch ex As Exception

                __errMessage = ex.Message


                __sbExMsg = New StringBuilder
                __sbExMsg.Append("rule_id:" + RULE_ID.ToString() + "|")
                __sbExMsg.Append("context_id:" + CONTEXT_ID.ToString() + "|")
                __sbExMsg.Append("pat_hosp_code:" + PAT_HOSP_CODE.ToString() + "|")
                __sbExMsg.Append("patient_number:" + PATIENT_NUMBER.ToString() + "|")
                __sbExMsg.Append("facility_code:" + FACILITY_CODE.ToString() + "|")
                __sbExMsg.Append("message_id:" + MESSAGE_ID.ToString() + "|")
                __sbExMsg.Append("operator_id:" + OPERATOR_ID.ToString() + "|")
                __sbExMsg.Append("admit_date:" + ADMIT_DATE.ToString() + "|")
                __sbExMsg.Append("event_datetime:" + __event_datetime.ToString() + "|")
                __sbExMsg.Append("patient_pid:" + PATIENT_PID.ToString() + "|")
                __sbExMsg.Append("ins_type:" + INS_TYPE.ToString() + "|")
                __sbExMsg.Append("Batch_id:" + BATCH_ID.ToString() + "|")
                performDBOperation("Update Patient_audit_trail set modified_date = getdate(), error_desc='" + Left(__errMessage, 249) + "' where pat_hosp_code = '" + PAT_HOSP_CODE + "' and patient_pid = " + Convert.ToString(PATIENT_PID))

                log.ExceptionDetails(":ConsoleFireRules-insertRuleResults:", ex)





                __iReturnReocords = 0

            End Try

            Return __iReturnReocords
        End Function



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
                    Using ruleComm As New SqlCommand(CfgUspRuleResultDelete, Con)

                        ruleComm.CommandType = CommandType.StoredProcedure
                        ruleComm.CommandTimeout = _CommandTimeout

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
                    Using ruleComm As New SqlCommand(CfgUspRuleInsertByXmlTank, Con)

                        ruleComm.CommandType = CommandType.StoredProcedure
                        ruleComm.CommandTimeout = _CommandTimeout

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
        ''' <param name="RULE_ID"></param>
        ''' <param name="RULE_CONTEXT_ID"></param>
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
        Private Function buildXMLRuleInsert(ByVal RULE_ID As Integer, ByVal RULE_CONTEXT_ID As Integer, ByVal PATINET_NUMBER As String, _
                                    ByVal PAT_HOSE_CODE As String, ByVal FACILITY_CODE As String, ByVal MESSAGE_ID As Integer, _
                                    ByVal OPERATOR_ID As String, ByVal ADMIT_DATE As String, ByVal EVENT_DATE As String, _
                                    ByVal PATIENT_PID As Integer, ByVal INS_TYPE As String, ByVal FINACIAL_CLASS As String, _
                                    ByVal PATIENT_TYPE As String, ByVal PRI_PLAN_NUMBER As String, ByVal SOURCE As String, _
                                    ByVal EVENT_TYPE As String, ByVal BATCH_ID As Integer, ByVal FIRST_NAME As String, _
                                    ByVal LAST_NAME As String, ByVal DISCHARGE_DATE As String) As String
            '   Dim ruleConn As SqlConnection = New SqlConnection





            '   Dim __errMessage As String
            Dim __event_datetime As String = String.Empty
            Dim __strXMLBuilder As New System.Text.StringBuilder

            Try

                Dim __admit_date As String = String.Empty

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
                ElseIf Not IsDBNull(__admit_date) And __admit_date.Length = 8 Then  'subba-020608
                    y = ADMIT_DATE.Substring(0, 4)
                    m = ADMIT_DATE.Substring(4, 2)
                    d = ADMIT_DATE.Substring(6, 2)
                    __admit_date = y + "-" + m + "-" + d + " " + "00" + ":" + "01"
                Else
                    __admit_date = "1/1/1909"  ' default value
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
                ElseIf Not IsDBNull(EVENT_DATE) And EVENT_DATE.Length = 8 Then
                    y = EVENT_DATE.Substring(0, 4)
                    m = EVENT_DATE.Substring(4, 2)
                    d = EVENT_DATE.Substring(6, 2)
                    h = "00"
                    mm = "01"
                    __event_datetime = y + "-" + m + "-" + d + " " + h + ":" + mm
                Else
                    __event_datetime = "1/1/1908" ' default value
                End If

                __strXMLBuilder.Append("<rule_results>")
                __strXMLBuilder.Append("<rule_id>" & RULE_ID.ToString() & "</rule_id>")
                __strXMLBuilder.Append("<context_id>" & RULE_CONTEXT_ID & "</context_id>")
                __strXMLBuilder.Append("<pat_hosp_code>" & PAT_HOSE_CODE & "</pat_hosp_code>")
                __strXMLBuilder.Append("<patient_number>" & PATINET_NUMBER & "</patient_number>")
                __strXMLBuilder.Append("<facility_code>" & FACILITY_CODE & "</facility_code>")
                __strXMLBuilder.Append("<operator_id>" & OPERATOR_ID & "</operator_id>")
                __strXMLBuilder.Append("<admit_date>" & __admit_date & "</admit_date>")
                __strXMLBuilder.Append("<PID>" & PATIENT_PID & "</PID>")
                __strXMLBuilder.Append("<ins_type>" & INS_TYPE & "</ins_type>")
                'strXMLBuilder.Append("<financial_class>" & financial_class & "</financial_class>")
                'strXMLBuilder.Append("<patient_type>" & patient_type & "</patient_type>")
                __strXMLBuilder.Append("<source>" & SOURCE & "</source>")
                __strXMLBuilder.Append("<event_type>" & EVENT_TYPE & "</event_type>")
                __strXMLBuilder.Append("<pri_plan_number>" & PRI_PLAN_NUMBER & "</pri_plan_number>")
                __strXMLBuilder.Append("<batch_id>" & BATCH_ID & "</batch_id>")
                __strXMLBuilder.Append("<message_id>" & MESSAGE_ID & "</message_id>")
                __strXMLBuilder.Append("<modified_date>" & Now().ToString() & "</modified_date>")
                __strXMLBuilder.Append("<event_datetime>" & __event_datetime & "</event_datetime>")
                __strXMLBuilder.Append("<patient_first_name>" & FIRST_NAME & "</patient_first_name>")  'subba-121107
                __strXMLBuilder.Append("<patient_last_name>" & LAST_NAME & "</patient_last_name>")
                __strXMLBuilder.Append("<discharge_date>" & DISCHARGE_DATE & "</discharge_date>")

                __strXMLBuilder.Append("</rule_results>")

            Catch ex As Exception


                log.ExceptionDetails("buildXMLRuleInsert", ex)

            End Try

            Return __strXMLBuilder.ToString()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="RULE_ID"></param>
        ''' <param name="CONTEXT_ID"></param>
        ''' <param name="PAT_HOSP_CODE"></param>
        ''' <param name="PATIENT_NUMBER"></param>
        ''' <param name="FACILITY_CODE"></param>
        ''' <param name="OPERATOR_ID"></param>
        ''' <param name="ADMIT_DATE"></param>
        ''' <param name="EVENT_DATE"></param>
        ''' <param name="RULE_ERROR_CODE"></param>
        ''' <param name="PATIENT_ID"></param>
        ''' <param name="INS_TYPE"></param>
        ''' <param name="BATCH_ID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function insertToRulesResultsDebug(ByVal RULE_ID As Integer, ByVal RULE_CONTEXT_ID As Integer, ByVal PAT_HOSP_CODE As String,
                                           ByVal PATIENT_NUMBER As String, ByVal FACILITY_CODE As String, ByVal OPERATOR_ID As String, _
                                           ByVal ADMIT_DATE As String, ByVal EVENT_DATE As String, ByVal RULE_ERROR_CODE As Integer, _
                                           ByVal PATIENT_ID As Integer, ByVal INS_TYPE As String, ByVal BATCH_ID As Integer) As Integer
            Dim r As Integer = -1

            ' Dim sqlString As String
            Dim sExMsg As String

            Try

                Dim iReturnReocords As Integer

                Dim y As String = String.Empty
                Dim m As String = String.Empty
                Dim d As String = String.Empty
                Dim h As String = String.Empty
                Dim mm As String = String.Empty
                Dim sqlDateAdmit As String = String.Empty
                Dim yy As String = String.Empty
                Dim mmm As String = String.Empty
                Dim dd As String = String.Empty
                Dim hh As String = String.Empty
                Dim mmmm As String = String.Empty
                Dim sqlDateEvent As String = String.Empty

                If Not IsDBNull(ADMIT_DATE) And ADMIT_DATE.Length > 11 Then
                    y = ADMIT_DATE.Substring(0, 4)
                    m = ADMIT_DATE.Substring(4, 2)
                    d = ADMIT_DATE.Substring(6, 2)
                    h = ADMIT_DATE.Substring(8, 2)
                    mm = ADMIT_DATE.Substring(10, 2)
                    sqlDateAdmit = y + "-" + m + "-" + d + " " + h + ":" + mm
                ElseIf Not IsDBNull(ADMIT_DATE) And ADMIT_DATE.Length = 8 Then   'subba-020608
                    y = ADMIT_DATE.Substring(0, 4)
                    m = ADMIT_DATE.Substring(4, 2)
                    d = ADMIT_DATE.Substring(6, 2)
                    h = "00"
                    mm = "01"
                    sqlDateAdmit = y + "-" + m + "-" + d + " " + h + ":" + mm
                Else
                    sqlDateAdmit = ADMIT_DATE
                End If

                If Not IsDBNull(EVENT_DATE) And EVENT_DATE.Length > 11 Then
                    yy = EVENT_DATE.Substring(0, 4)
                    mmm = EVENT_DATE.Substring(4, 2)
                    dd = EVENT_DATE.Substring(6, 2)
                    hh = EVENT_DATE.Substring(8, 2)
                    mmmm = EVENT_DATE.Substring(10, 2)
                    sqlDateEvent = yy + "-" + mmm + "-" + dd + " " + hh + ":" + mmmm
                ElseIf Not IsDBNull(EVENT_DATE) And EVENT_DATE.Length = 8 Then
                    yy = EVENT_DATE.Substring(0, 4)
                    mmm = EVENT_DATE.Substring(4, 2)
                    dd = EVENT_DATE.Substring(6, 2)
                    hh = "00"
                    mmmm = "01"
                    sqlDateEvent = yy + "-" + mmm + "-" + dd + " " + hh + ":" + mmmm
                Else
                    sqlDateEvent = EVENT_DATE
                End If



                '   sqlString = CfgUspRuleInsertDebug 'subba-022509 '"usp_rule_insert_debug"


                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(CfgUspRuleInsertDebug, Con)

                        cmd.CommandType = CommandType.StoredProcedure


                        cmd.Parameters.Add("@rule_id", SqlDbType.Int).Value = RULE_ID
                        cmd.Parameters.Add("@context_id", SqlDbType.Int).Value = RULE_CONTEXT_ID
                        cmd.Parameters.Add("@pat_hosp_code", SqlDbType.VarChar, 20).Value = PAT_HOSP_CODE
                        cmd.Parameters.Add("@patient_number", SqlDbType.VarChar, 50).Value = PATIENT_NUMBER
                        cmd.Parameters.Add("@facility_code", SqlDbType.VarChar, 3).Value = FACILITY_CODE
                        cmd.Parameters.Add("@message_id", SqlDbType.Int).Value = RULE_ERROR_CODE
                        cmd.Parameters.Add("@operator_id", SqlDbType.VarChar, 20).Value = OPERATOR_ID
                        cmd.Parameters.Add("@admit_date", SqlDbType.DateTime).Value = sqlDateAdmit
                        cmd.Parameters.Add("@event_datetime", SqlDbType.DateTime).Value = sqlDateEvent
                        cmd.Parameters.Add("@patient_pid", SqlDbType.Int).Value = PATIENT_ID
                        cmd.Parameters.Add("@ins_type", SqlDbType.VarChar, 5).Value = INS_TYPE
                        cmd.Parameters.Add("@batch_id", SqlDbType.Int).Value = BATCH_ID

                        iReturnReocords = cmd.ExecuteNonQuery()
                    End Using
                    Con.Close()
                End Using



                r = 0

            Catch sx As SqlException
                sExMsg = sx.Message
                log.ExceptionDetails("insertToRulesResultsDebug", sx)

            Catch ex As Exception
                sExMsg = ex.Message
                log.ExceptionDetails("insertToRulesResultsDebug", ex)
            End Try

            Return r

        End Function






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


                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(COMMAND_STRING, Con)

                        cmd.CommandType = CommandType.Text
                        cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
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
        ''' <param name="COMMAND_NAME"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function performDBexecuteSp(ByVal COMMAND_NAME As String) As Integer


            Dim __strErrorMsg As String = String.Empty
            Dim __r As Integer = -1
            '    Dim sqlConn As SqlConnection = New SqlConnection
            '    Dim ruleComm As New SqlCommand

            Try

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(COMMAND_NAME, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using
                __r = 0


            Catch sx As SqlException
                __strErrorMsg = sx.Message
                log.ExceptionDetails("performDBexecuteSp", sx)

            Catch ex As Exception
                __strErrorMsg = ex.Message
                log.ExceptionDetails("performDBexecuteSp", ex)

                __strErrorMsg = ex.Message

            End Try




            Return __r
        End Function


        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="XML"></param>
        ''' <param name="ElEMENT_NAME"></param>
        ''' <param name="PAT_HOSP_CODE"></param>
        ''' <param name="PAT_NAME"></param>
        ''' <param name="RA_CLINET_IP"></param>
        ''' <param name="GET_RA_UID"></param>
        ''' <param name="GET_RA_COOKIE"></param>
        ''' <param name="OPR_ID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function processXML(ByVal XML As String, ByVal ElEMENT_NAME As String, ByVal PAT_HOSP_CODE As String, _
                                    ByVal PAT_NAME As String, ByVal RA_CLINET_IP As String, ByVal GET_RA_UID As String, _
                                    ByVal GET_RA_COOKIE As String, ByVal OPR_ID As String) As String




            Dim __sErrMsg As String = String.Empty
            Dim __sTemp As String = String.Empty
            Dim __sPID As String = String.Empty
            Dim __sPatNum As String = String.Empty
            Dim __sModfiedDate As String = String.Empty
            Dim __sMsgSend As String = String.Empty
            Dim __sHospCode As String = String.Empty
            Dim __i As Integer = 0
            Dim __sNodeListXML As String = String.Empty

            Dim XMLdoc As XmlDocument = New XmlDocument


            Try

                XMLdoc.LoadXml(XML)
                Dim RootNode As XmlElement = XMLdoc.DocumentElement
                Dim nodeList As XmlNodeList = RootNode.GetElementsByTagName(ElEMENT_NAME)
                Dim nodeListPat As XmlNodeList = RootNode.GetElementsByTagName("patient_number")
                __sPatNum = nodeListPat.Item(0).InnerXml

                Dim nodeListModifiedDate As XmlNodeList = RootNode.GetElementsByTagName("modified_date")
                __sModfiedDate = nodeListModifiedDate.Item(0).InnerXml


                For __i = 0 To nodeList.Count - 1
                    'sTemp = Trim(nodeList.Item(i).InnerXml).Replace("-", "") + "~" + Convert.ToString(htRuleMsgs.Item(Convert.ToInt32(nodeList.Item(i).InnerXml.Trim))) + RULE_MESSAGE_DELIMETER + sTemp 'subba-020108
                    'sTemp = Trim(nodeList.Item(i).InnerXml).Replace("-", "") + "~" + Convert.ToString(htRuleMsgs.Item(Convert.ToInt32(nodeList.Item(i).InnerXml.Trim))).Replace(RULE_MESSAGE_DELIMETER, " ") + RULE_MESSAGE_DELIMETER + sTemp 'subba-020108

                    __sNodeListXML = Trim(nodeList.Item(__i).InnerXml.Replace("|", "").Replace("~", "").Replace(_RULE_MESSAGE_DELIMETER, " ").Replace("-", "").Replace(":", "")) 'subba-020408
                    'sTemp = Trim(nodeList.Item(i).InnerXml).Replace("-", "") + "~" + Convert.ToString(htRuleMsgs.Item(Convert.ToInt32(nodeList.Item(i).InnerXml.Trim))).Replace(RULE_MESSAGE_DELIMETER, " ") + RULE_MESSAGE_DELIMETER + sTemp 'subba-020108
                    'comment previous line/Uncomment follwing line- to send only rule_message_ids 'subba-020608
                    __sTemp = __sNodeListXML + "~" + "" + _RULE_MESSAGE_DELIMETER + __sTemp 'subba-020408 'only send rule Message IDs , descriptions will be parsed at realAlert Client
                Next __i

                PAT_NAME = PAT_NAME.Replace("-", "")
                'Clear our object
                XMLdoc = Nothing
                If __sTemp.EndsWith(",") Then __sTemp = __sTemp.Remove(__sTemp.Length - 1, 1)

                If (PAT_HOSP_CODE.Split(CChar("-")).Length > 1) Then __sHospCode = PAT_HOSP_CODE.Split(CChar("-"))(1)

                'subba-020408
                __sModfiedDate = __sModfiedDate.Replace("|", " ").Replace("~", " ").Replace("^", " ").Replace(":", " ").Replace("-", " ")
                __sHospCode = __sHospCode.Replace("|", " ").Replace("~", " ").Replace("^", " ").Replace(":", " ").Replace("-", " ")
                __sPatNum = __sPatNum.Replace("|", " ").Replace("~", " ").Replace("^", " ").Replace(":", " ").Replace("-", " ")
                PAT_NAME = PAT_NAME.Replace("|", " ").Replace("~", " ").Replace("^", " ").Replace(":", " ").Replace("-", " ")

                ' WHERE DOES THIS 
                __sMsgSend = __sModfiedDate + "|" + __sHospCode + "|" + __sPatNum + "|" + PAT_NAME + "|" + __sTemp + "|" + RA_CLINET_IP + "|" + GET_RA_UID + "|" + OPR_ID + "|" + GET_RA_COOKIE

            Catch ex As Exception
                __sErrMsg = ex.ToString

                log.ExceptionDetails("processXML", ex)

                __sMsgSend = String.Empty
                'Return ""
            End Try

            Return __sMsgSend

        End Function





        ''' <summary>
        ''' LABELED WEB SERVICE
        ''' </summary>
        ''' <param name="PAT_HOSP_CODE"></param>
        ''' <param name="ADDRESS_TYPE"></param>
        ''' <param name="PATIENT_STREET_ADDRESS"></param>
        ''' <param name="PATIENT_ADDRESS_2"></param>
        ''' <param name="PATIENT_CITY"></param>
        ''' <param name="PATIENT_STATE"></param>
        ''' <param name="PATIENT_ZIP_CODE"></param>
        ''' <param name="ADDRESS_XML_OUT"></param>
        ''' <param name="PAITIENT_ID"></param>
        ''' <param name="PATIENT_PID"></param>
        ''' <param name="ADDRESS_BATCH_ID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function verifyAddressSaveAPI(ByVal PAT_HOSP_CODE As String, ByVal ADDRESS_TYPE As String, ByVal PATIENT_STREET_ADDRESS As String, _
                                             ByVal PATIENT_ADDRESS_2 As String, ByVal PATIENT_CITY As String, ByVal PATIENT_STATE As String, _
                                             ByVal PATIENT_ZIP_CODE As String, ByRef ADDRESS_XML_OUT As String, ByVal PAITIENT_ID As Integer, _
                                             ByVal PATIENT_PID As Integer, ByVal ADDRESS_BATCH_ID As Integer) As Boolean


            '  Dim _iRowsToProcess As Integer = 0
            Dim _bSuccess As Boolean = False

            Try
                '    Dim __sResultAll As String = String.Empty
                '   Dim __sRaClientIP As String = String.Empty
                '  Dim __sRaServerPort As String = String.Empty
                '   Dim __sRuleSuccess As String = String.Empty
                '   Dim __sRaVersion As String = String.Empty
                '   Dim __sAddrTypeFilter As String = String.Empty
                'Dim __sAddrMultipleAddressFlag As String = String.Empty
                'subba-032008 
                '      Dim __sGetInsertRaDebugFlag As String = String.Empty
                '    Dim __sGetRaMsgDelimeter As String = String.Empty
                '    Dim __strRuleContextID As String = String.Empty
                'string sErrLogPath = string.empty; 

                '   Dim __sRuleDebugMode As String = String.Empty
                '    Dim __strHospCode As String = String.Empty
                '    Dim __sRunMode As String = String.Empty
                '     Dim __sRaProtocol As String = String.Empty
                '    Dim __sAddressXmlOut1 As String = String.Empty


                '   Dim ___outNumber As Integer = 0

                ' from system_preferences 
                '   LoadFireAddressParams(__sAddrTypeFilter, __sAddrMultipleAddressFlag, __sRaServerPort, __sRaProtocol, __sGetInsertRaDebugFlag, __sGetRaMsgDelimeter)


                'sAddrTypeFilter 'P,G,C 'subba-030308 
                'string sAddressType = string.Empty; 
                Dim ___sAddrXmlP As String = String.Empty
                Dim ___sAddrXmlG As String = String.Empty
                Dim ___sAddrXmlC As String = String.Empty




                If ADDRESS_TYPE.Trim().ToLower() = "guarantor" Then
                    ___sAddrXmlG = [String].Concat("<address><addresstype>", ADDRESS_TYPE, "</addresstype><address1>", PATIENT_STREET_ADDRESS, "</address1><address2>", PATIENT_ADDRESS_2, _
                    "</address2><city>", PATIENT_CITY, "</city><state>", PATIENT_STATE, "</state><zip>", PATIENT_ZIP_CODE, _
                    "</zip><dpvnotes>", "", "</dpvnotes></address>")
                End If
                'subba-042308 'subba-1018113 'order G,P,C address
                If ADDRESS_TYPE.Trim().ToLower() = "patient" Then
                    ___sAddrXmlP = String.Concat("<address><addresstype>", ADDRESS_TYPE, "</addresstype><address1>", PATIENT_STREET_ADDRESS, "</address1><address2>", PATIENT_ADDRESS_2, _
                    "</address2><city>", PATIENT_CITY, "</city><state>", PATIENT_STATE, "</state><zip>", PATIENT_ZIP_CODE, _
                    "</zip><dpvnotes>", "", "</dpvnotes></address>")
                End If
                If ADDRESS_TYPE.Trim().ToLower() = "contact" Then
                    ___sAddrXmlC = [String].Concat("<address><addresstype>", ADDRESS_TYPE, "</addresstype><address1>", PATIENT_STREET_ADDRESS, "</address1><address2>", PATIENT_ADDRESS_2, _
                    "</address2><city>", PATIENT_CITY, "</city><state>", PATIENT_STATE, "</state><zip>", PATIENT_ZIP_CODE, _
                    "</zip><dpvnotes>", "", "</dpvnotes></address>")
                End If

                Dim ___sAddrXmlIn As String = String.Concat("<addressesin>", ___sAddrXmlP, ___sAddrXmlG, ___sAddrXmlC, "</addressesin>")
                Dim ___sAddrXmlOut As String = String.Empty
                Dim ___bIsAddrValidated As Boolean = False

                If ((_GetMultipleAddress IsNot Nothing)) And (_GetMultipleAddress = "Y") Then

                    ___bIsAddrValidated = RunVerifyAddressesMultipleOnDemand(PAT_HOSP_CODE, ADDRESS_TYPE, PATIENT_STREET_ADDRESS, _
                                                                             PATIENT_ADDRESS_2, PATIENT_CITY, PATIENT_STATE, _
                                                                             PATIENT_ZIP_CODE, ADDRESS_XML_OUT, _CUSTOMER_ID, _
                                                                             _LICENSE_KEY, _ConnectionString, PAITIENT_ID, PATIENT_PID, _
                                                                             ADDRESS_BATCH_ID)


                End If
                _bSuccess = True
            Catch ex As Exception
                _bSuccess = False


                log.ExceptionDetails("AddressValidationDCSAddress.verifyAddressSaveAPI()", ex)


            End Try

            _RuleFailed = _bSuccess
            Return _bSuccess
        End Function


        'subba-060908 - gets MutipleAddressMatches call //subba-053008 
        Public Function RunVerifyAddressesMultipleOnDemand(ByVal sPatHospCode As String, ByVal sAddrType As String, ByVal sAddr1 As String, ByVal sAddr2 As String, _
                                                           ByVal sCity As String, ByVal sState As String, ByVal sZip As String, ByRef sAddrXmlOut As String, _
                                                           ByVal sCustomerID As String, ByVal sLicenseKey As String, ByVal sqlConnString As String, _
                                                           ByVal iPatIdM As Integer, ByVal iPatPidM As Integer, ByVal iBatchIdAddr As Integer) As Boolean





            If (sAddrType Is Nothing) Then sAddrType = "guarantor" 'subba-objectReferenceError 'subba-082112

            '   dtStart_ts = System.DateTime.Now
            Dim ts As TimeSpan
            Dim sbAddressesOut As New StringBuilder("<addressout>")
            '  SQL_CONNECTION_STRING = sqlConnString
            Dim bResult As Boolean = False 'subba-021910

            Dim isAttemptsSuccess As Boolean = False
            Dim sAddrUspsFormatErr As String = String.Empty

            Dim sOriginalAddr1 As String = sAddr1

            Try
                Dim iMultipleAddrMatch As Integer = 0
                Dim isAddrErr As Boolean = False
                Dim sAddrXmlIn As String = String.Empty
                Dim sDpvValidDesc As String = String.Empty
                Dim isBadInputAddr As Boolean = False
                Dim sTemp1 As String = String.Empty
                Dim sTemp2 As String = String.Empty


                sAddrXmlIn = String.Concat("<addressesin><address><address1>", sAddr1, "</address1><address2>", sAddr2, "</address2><city>", sCity, _
                "</city><state>", sState, "</state><zip>", sZip, "</zip><dpvnotes></dpvnotes></address></addressesin>")

                'subba-052708 //Call Web Service http://10.1.1.58:8080/AddressWs.asmx 

                'SRAO-060508
                Dim addresses() As dcsAddrWS.AESRawAddress ' = New dcsAddrWS.AESRawAddress() 'subba-060508
                Dim objAddrWsIn As dcsAddrWS.DcsAddress = New dcsAddrWS.DcsAddress() 'subba-060508

                Try
                    'SUBBA-062312
                    'If (_DebugMode = "Y") Then SaveTextToFile(errMessage + ":ConsoleFireRules-:" + Now.ToString + " ", sErrLogPath, "")
                    addresses = objAddrWsIn.VerifyAddressEnhanced(sAddr1, sAddr2, sCity, sState, sZip, sCustomerID, sLicenseKey)
                    'If (_DebugMode = "Y") Then insertExceptionDetails(sConsoleProgName + "-CUSTOMER_ACCESS", "dcsAddrWS.DcsAddress-VerifyAddressEnhanced", "ADDRESS_SERVER_DEBUG-Customer-" + sCustomerID + "~" + sLicenseKey + "__" + sAddr1 + ":" + sAddr2 + ":" + sCity + ":" + sState + ":" + sZip) 'subba-030810
                Catch ex As Exception
                    Dim bSvrErr As Boolean = False
                    If (ex.Message.Trim().ToLower().Contains("unable")) Then
                        addresses = Nothing 'SUBBA-062312 '"Error-X-Unable to connect to the remote server"
                        bSvrErr = True
                    End If
                    If (ex.Message.Trim().ToLower().Contains("unavailable")) Then
                        addresses = Nothing
                        bSvrErr = True
                    End If
                    If (ex.Message.Trim().ToLower().Contains("timed-out")) Then
                        addresses = Nothing
                        bSvrErr = True
                    End If

                    If (ex.Message.Trim().ToLower().Contains("tcpclient")) Then
                        addresses = Nothing
                        bSvrErr = True
                    End If

                    If (ex.Message.Trim().ToLower().Contains("error")) Then
                        addresses = Nothing
                        bSvrErr = True
                    End If

                    If (addresses Is Nothing) Then  'BadAddress-10365 E VOLTAIVE AVENUE, SCOTTSDALE ,AZ 85260 - scottsdale
                        If (bSvrErr = True) Then insertAddressValidatedAuditTrail(sPatHospCode, "X", "AESSystemError:" + ex.Message + "-:-" + ":SwitchToSecServer:", "DCSAddress-", "ConsoleDCSAddress", sAddr1, sAddr2, sCity, sState, sZip, iPatIdM, iPatPidM, "", iBatchIdAddr)
                        If (bSvrErr = False) Then insertAddressValidatedAuditTrail(sPatHospCode, "E", "NullAddressReturned-BadAddress1:-", "DCSAddress-G", "ConsoleDCSAddress", sAddr1, sAddr2, sCity, sState, sZip, iPatIdM, iPatPidM, "", iBatchIdAddr) 'SUBBA-082112 'subba-060508
                    End If 'subba-060508 
                End Try

                If (addresses Is Nothing) Then
                    'subba-062312
                    ''' insertAddressValidatedAuditTrail(sPatHospCode, "X", "AddressSystemError-TrySec2:-" + objAddrWsIn.Url.ToString(), "DCSAddress-" + sAddrType, "ConsoleDCSAddress", sAddr1, sAddr2, sCity, sState, sZip, iPatIdM, iPatPidM, "", iBatchIdAddr) 'subba-060508
                    objAddrWsIn.Url = CfgAddrSvcUrl2 'SUBBA-062312
                    addresses = objAddrWsIn.VerifyAddressEnhanced(sAddr1, sAddr2, sCity, sState, sZip, sCustomerID, sLicenseKey)
                    'If (addresses.Length = 0) Then Throw New NullReferenceException("DCSAddressValidateDPV Server Exception") 'SUBBA-082112
                    If (addresses.Length = 0) Then
                        'insertAddressValidatedAuditTrail(sPatHospCode, "E", "NullAddressReturned-BadAddress2:-" + objAddrWsIn.Url.ToString() + addresses(0).error, "DCSAddress-" + sAddrType, "ConsoleDCSAddress", sAddr1, sAddr2, sCity, sState, sZip, iPatIdM, iPatPidM, "", iBatchIdAddr) 'subba-060508
                        insertAddressValidatedAuditTrail(sPatHospCode, "E", "NullAddressReturned-BadAddress2:-", "DCSAddress-", "ConsoleDCSAddress", sAddr1, sAddr2, sCity, sState, sZip, iPatIdM, iPatPidM, "", iBatchIdAddr) 'subba-082112
                    End If





                    ' Throw New NullReferenceException("DCSAddressValidateDPV Server Exception")
                End If

                If (addresses.Length > 0) Then  'subba-042911
                    If Not (addresses(0).error Is Nothing) Then
                        If (addresses(0).error.Length > 5) Then
                            ''objAddrWsIn.Url = sCfgAddrSvcUrl2 'SUBBA-062312
                            'insertAddressValidatedAuditTrail(sPatHospCode, "X", "AddressSystemError-TrySecServer:-" + objAddrWsIn.Url.ToString() + addresses(0).error, "DCSAddress-" + sAddrType, "ConsoleDCSAddress", sAddr1, sAddr2, sCity, sState, sZip, iPatIdM, iPatPidM, "", iBatchIdAddr) 'subba-060508
                            insertAddressValidatedAuditTrail(sPatHospCode, "S", "AddressSystemError-TrySecServer:-" + "::", "DCSAddress-G", "ConsoleDCSAddress", sAddr1, sAddr2, sCity, sState, sZip, iPatIdM, iPatPidM, "", iBatchIdAddr) 'subba-060508

                            'Throw New NullReferenceException("DCSAddressValidateDPV Server Exception")
                            objAddrWsIn.Url = CfgAddrSvcUrl2 'SUBBA-062312
                            addresses = objAddrWsIn.VerifyAddressEnhanced(sAddr1, sAddr2, sCity, sState, sZip, sCustomerID, sLicenseKey) 'subba-062312
                            If (addresses.Length = 0) Then
                                insertAddressValidatedAuditTrail(sPatHospCode, "E", "NullAddressReturned-BadAddressError2:-" + ":-:", "DCSAddress-G", "ConsoleDCSAddress", sAddr1, sAddr2, sCity, sState, sZip, iPatIdM, iPatPidM, "", iBatchIdAddr) 'subba-082112-'subba-060508 'PNE, CITY , STATE





                                '''''''Throw New NullReferenceException("AddrCustomException") 'SUBBA-082112
                            End If 'SUBBA-062312
                        End If
                    End If
                End If
                objAddrWsIn.Url = CfgAddrSvcUrl1 'SUBBA-062312 'Revertback to Primary

                Dim cszSplit As String() = Nothing
                Dim diagnosticSplit As String() = Nothing
                Dim sKEY As String = String.Empty, sREL As String = String.Empty, sAQF As String = String.Empty, sCRT As String = String.Empty, sDPV As String = String.Empty, sCNTY As String = String.Empty
                Dim cszAddr As String = String.Empty, cszCity As String = String.Empty, cszState As String = String.Empty, cszZip As String = String.Empty

                If (addresses.Length = 1) Then
                    sTemp1 = "Address Match"
                    sTemp2 = "Valid Address"

                    If addresses(0).diagnostics.Trim().Length > 24 Then
                        diagnosticSplit = addresses(0).diagnostics.ToUpper().Replace("//", "").Split(" "c)
                    End If

                    If diagnosticSplit.Length > 0 Then
                        sKEY = diagnosticSplit(0).Replace("KEY=", "")
                    End If
                    If diagnosticSplit.Length > 1 Then
                        sREL = diagnosticSplit(1).Replace("REL=", "")
                    End If
                    If diagnosticSplit.Length > 2 Then
                        sAQF = diagnosticSplit(2).Replace("AQF=", "")
                    End If
                    If diagnosticSplit.Length > 3 Then
                        sCRT = diagnosticSplit(3).Replace("CRT=", "")
                    End If
                    If diagnosticSplit.Length > 4 Then
                        sDPV = diagnosticSplit(4).Replace("DPV=", "")
                    End If
                    If diagnosticSplit.Length > 5 Then
                        sCNTY = diagnosticSplit(5).Replace("CNTY=", "")
                    End If

                    If addresses(0).csz.Trim().Length > 3 Then
                        cszSplit = addresses(0).csz.Split(" "c)
                    End If
                    If cszSplit.Length = 3 Then
                        cszZip = cszSplit(cszSplit.Length - 1)
                        cszState = cszSplit(cszSplit.Length - 2)
                        cszCity = cszSplit(cszSplit.Length - 3)
                    End If
                    If cszSplit.Length = 4 Then
                        cszZip = cszSplit(cszSplit.Length - 1)
                        cszState = cszSplit(cszSplit.Length - 2)
                        cszCity = cszSplit(cszSplit.Length - 4) + " " + cszSplit(cszSplit.Length - 3)
                    End If
                    If cszSplit.Length = 5 Then
                        cszZip = cszSplit(cszSplit.Length - 1)
                        cszState = cszSplit(cszSplit.Length - 2)
                        cszCity = cszSplit(cszSplit.Length - 5) + " " + cszSplit(cszSplit.Length - 4) + " " + cszSplit(cszSplit.Length - 3)
                    End If
                    If cszSplit.Length = 6 Then
                        cszZip = cszSplit(cszSplit.Length - 1)
                        cszState = cszSplit(cszSplit.Length - 2)
                        cszCity = cszSplit(cszSplit.Length - 6) + " " + cszSplit(cszSplit.Length - 5) + " " + cszSplit(cszSplit.Length - 4) + " " + cszSplit(cszSplit.Length - 3)
                    End If

                    sbAddressesOut.Append("<address>")
                    sbAddressesOut.Append("<addresstype>" + sAddrType + "</addresstype>")
                    sbAddressesOut.Append("<address1>" + addresses(0).address + "</address1>")
                    sbAddressesOut.Append("<address2>" + addresses(0).line2 + "</address2>")
                    sbAddressesOut.Append("<city>" + cszCity + "</city>")
                    'addresses[0].city 
                    sbAddressesOut.Append("<state>" + cszState + "</state>")
                    'addresses[0].state 
                    sbAddressesOut.Append("<zip>" + cszZip + "</zip>")
                    'addresses[0].zip 
                    sbAddressesOut.Append("<dpvnotes>" + addresses(0).diagnostics + "</dpvnotes>")
                    sbAddressesOut.Append("<BarcodeDigits>" + addresses(0).barcode + "</BarcodeDigits>")
                    '----------------------------------diagnostic info ------------------- 
                    If diagnosticSplit.Length > 0 Then
                        sbAddressesOut.Append("<ReliabilityKey>" + sKEY + "</ReliabilityKey>")
                    Else
                        sbAddressesOut.Append("<ReliabilityKey>" + "" + "</ReliabilityKey>")
                    End If

                    If diagnosticSplit.Length > 1 Then
                        sbAddressesOut.Append("<ReliabilityNumber>" + sREL + "</ReliabilityNumber>")
                    Else
                        sbAddressesOut.Append("<ReliabilityNumber>" + "" + "</ReliabilityNumber>")
                    End If

                    If diagnosticSplit.Length > 2 Then
                        sbAddressesOut.Append("<AddressQuality>" + sAQF + "</AddressQuality>")
                    Else
                        sbAddressesOut.Append("<AddressQuality>" + "" + "</AddressQuality>")
                    End If

                    If diagnosticSplit.Length > 3 Then
                        sbAddressesOut.Append("<CarrierRoute>" + sCRT + "</CarrierRoute>")
                    Else
                        sbAddressesOut.Append("<CarrierRoute>" + "" + "</CarrierRoute>")
                    End If

                    If diagnosticSplit.Length > 4 Then
                        sbAddressesOut.Append("<DPV>" + sDPV + "</DPV>")
                    Else
                        sbAddressesOut.Append("<DPV>" + "" + "</DPV>")
                    End If

                    If diagnosticSplit.Length > 5 Then
                        sbAddressesOut.Append("<County>" + sCNTY + "</County>")
                    Else
                        sbAddressesOut.Append("<County>" + "" + "</County>")
                    End If

                    sbAddressesOut.Append("</address>")

                    '  dtEnd_ts = DateTime.Now
                    ' ts = dtEnd_ts.Subtract(dtStart_ts)

                    sDpvValidDesc = GetDpvValidCodesDesc(sDPV)
                    'subba-033108 

                    'System.Drawing.Graphics g = this.pictureBox1.CreateGraphics(); //BARCODE 
                    'HandlePostnetDrawing(g); 

                    insertCorrectedAddressDetail(sPatHospCode, sAddrType, addresses(0).address + " , " + addresses(0).line2, cszCity, cszState, cszZip, _
                    "", "", "", Convert.ToString(ts.Milliseconds), "Y", sbAddressesOut.ToString(), _
                    sAddrXmlIn, addresses(0).barcode, "", "", "", "", _
                    "", "", "", "", "", "", _
                    "", "", sDpvValidDesc.Substring(0, 1), sDpvValidDesc, sKEY, sREL, _
                    sAQF, sCRT, sDPV, sCNTY, sAddr1, sAddr2, _
                    sCity, sState, sZip, iPatIdM, iPatPidM)
                    'insertAddressValidatedAuditTrail(sPatHospCode, "DPV", testWsOutDPV.DPVNotesDesc, "DCSAddress3", "ConsoleAddress") 
                    insertAddressValidatedAuditTrail(sPatHospCode, "C", "Valid Address", "DCSAddress- " + sAddrType, "ConsoleDCSAddress", sAddr1, _
                    sAddr2, sCity, sState, sZip, iPatIdM, iPatPidM, _
                    sDPV, iBatchIdAddr)
                Else

                    If addresses.Length > 1 Then
                        insertAddressValidatedAuditTrail(sPatHospCode, "M", "valid Address Not found_multipleMatch", "DCSAddressM- " + sAddrType, "ConsoleDCSAddress", sAddr1, _
                        sAddr2, sCity, sState, sZip, iPatIdM, iPatPidM, _
                        sDPV, iBatchIdAddr)
                    End If
                    'subba-040408 
                    If addresses.Length = 0 Then
                        isAttemptsSuccess = False 'SUBBA-102113
                        'subba-052208 
                        If Not isAttemptsSuccess Then
                            ValidateAddressBySwappingAddressFields(1, sPatHospCode, sAddr1, sAddr2, sCity, sState, sZip, sCustomerID, sLicenseKey, iPatIdM, iPatPidM, sAddrType, iBatchIdAddr) 'Subba-061808 - added sAddrType as field
                        End If
                        'subba-040408 - swap Addr1, Addr2 fields 
                        If Not isAttemptsSuccess Then
                            ValidateAddressBySwappingAddressFields(2, sPatHospCode, sAddr1, sAddr2, sCity, sState, sZip, sCustomerID, sLicenseKey, iPatIdM, iPatPidM, sAddrType, iBatchIdAddr) 'Subba-061808 - added sAddrType as field
                        End If
                        'subba-040408 - swap Addr1, Addr2 fields 
                        If Not isAttemptsSuccess Then
                            ValidateAddressBySwappingAddressFields(3, sPatHospCode, sAddr1, sAddr2, sCity, sState, sZip, sCustomerID, sLicenseKey, iPatIdM, iPatPidM, sAddrType, iBatchIdAddr) 'Subba-061808 - added sAddrType as field
                        End If

                        If Not isAttemptsSuccess Then 'subba-012109 'finalAttemptWithUspsStreetFormat '1500 CENITH DR  APT 302 D , NORTH MYRTLE BE  SC 29582
                            sAddrUspsFormatErr = getUspsFormatAddressToReprocess(sAddr1, sAddr2) 'subba-012109 'ByRef  'sAddr1 gets combined sAddr1+sAddr2
                            ValidateAddressBySwappingAddressFields(4, sPatHospCode, sAddr1, "", sCity, sState, sZip, sCustomerID, sLicenseKey, iPatIdM, iPatPidM, sAddrType, iBatchIdAddr) 'Subba-012109 'NoSwapping Address fileds, feed the UspsFormated address
                        End If
                        'subba-040408 - swap Addr1, Addr2 fields 
                        If isAttemptsSuccess = False Then
                            If (sAddrUspsFormatErr <> "") Then
                                'subba-061212
                                insertAddressValidatedAuditTrail(sPatHospCode, "E", "Error-NoAddressMatch-Empty Result", "DCSAddressE- " + sAddrType, "ConsoleDCSAddress-USPS", sOriginalAddr1, _
                                "", sCity, sState, sZip, iPatIdM, iPatPidM, _
                                sDPV, iBatchIdAddr)

                            Else
                                'subba-061212
                                insertAddressValidatedAuditTrail(sPatHospCode, "E", "Error-NoAddressMatch-Empty Result", "DCSAddressE- " + sAddrType, "ConsoleDCSAddress", sOriginalAddr1, _
                                sAddr2, sCity, sState, sZip, iPatIdM, iPatPidM, _
                                sDPV, iBatchIdAddr)

                            End If
                            sAddrUspsFormatErr = String.Empty

                        End If
                    End If

                    For Each address As dcsAddrWS.AESRawAddress In addresses 'DcsAddressLib.AESRawAddress  '060508
                        'subba-052708 //subba-060408 //AddressWS.AESRawAddress address in addresses 
                        If (address.address IsNot Nothing) Then
                            iMultipleAddrMatch = iMultipleAddrMatch + 1

                            If address.csz.Trim().Length > 3 Then
                                cszSplit = address.csz.Split(" "c)

                                If cszSplit.Length = 3 Then
                                    cszZip = cszSplit(cszSplit.Length - 1)
                                    cszState = cszSplit(cszSplit.Length - 2)
                                    'cszAddr = cszSplit[cszSplit.Length - 4]; 
                                    cszCity = cszSplit(cszSplit.Length - 3)
                                End If
                                If cszSplit.Length = 4 Then
                                    cszZip = cszSplit(cszSplit.Length - 1)
                                    cszState = cszSplit(cszSplit.Length - 2)
                                    cszCity = cszSplit(cszSplit.Length - 4) + " " + cszSplit(cszSplit.Length - 3)
                                End If
                                If cszSplit.Length = 5 Then
                                    cszZip = cszSplit(cszSplit.Length - 1)
                                    cszState = cszSplit(cszSplit.Length - 2)
                                    cszCity = cszSplit(cszSplit.Length - 5) + " " + cszSplit(cszSplit.Length - 4) + " " + cszSplit(cszSplit.Length - 3)
                                End If
                            End If

                            If address.diagnostics.Trim().Length > 24 Then
                                diagnosticSplit = address.diagnostics.ToUpper().Replace("//", "").Split(" "c)
                            End If

                            If diagnosticSplit.Length > 0 Then
                                sKEY = diagnosticSplit(0).Replace("KEY=", "")
                            End If
                            If diagnosticSplit.Length > 1 Then
                                sREL = diagnosticSplit(1).Replace("REL=", "")
                            End If
                            If diagnosticSplit.Length > 2 Then
                                sAQF = diagnosticSplit(2).Replace("AQF=", "")
                            End If
                            If diagnosticSplit.Length > 3 Then
                                sCRT = diagnosticSplit(3).Replace("CRT=", "")
                            End If
                            If diagnosticSplit.Length > 4 Then
                                sDPV = diagnosticSplit(4).Replace("DPV=", "")
                            End If
                            If diagnosticSplit.Length > 5 Then
                                sCNTY = diagnosticSplit(5).Replace("CNTY=", "")
                            End If

                            sbAddressesOut.Append("<address>")
                            sbAddressesOut.Append("<addresstype>" + sAddrType + "</addresstype>")
                            sbAddressesOut.Append("<address1>" + address.address + "</address1>")
                            sbAddressesOut.Append("<address2>" + address.line2 + "</address2>")

                            If cszSplit.Length > 0 Then
                                sbAddressesOut.Append("<city>" + cszCity + "</city>")
                            Else
                                sbAddressesOut.Append("<city>" + "" + "</city>")
                            End If

                            If cszSplit.Length > 1 Then
                                sbAddressesOut.Append("<state>" + cszState + "</state>")
                            Else
                                sbAddressesOut.Append("<state>" + "" + "</state>")
                            End If

                            If cszSplit.Length > 2 Then
                                sbAddressesOut.Append("<zip>" + cszZip + "</zip>")
                            Else
                                sbAddressesOut.Append("<zip>" + "" + "</zip>")
                            End If

                            sbAddressesOut.Append("<dpvnotes>" + address.diagnostics + "</dpvnotes>")
                            sbAddressesOut.Append("<BarcodeDigits>" + address.barcode + "</BarcodeDigits>")

                            '----------------------------------diagnostic info ------------------- 
                            If diagnosticSplit.Length > 0 Then
                                sbAddressesOut.Append("<ReliabilityKey>" + sKEY + "</ReliabilityKey>")
                            Else
                                sbAddressesOut.Append("<ReliabilityKey>" + "" + "</ReliabilityKey>")
                            End If

                            If diagnosticSplit.Length > 1 Then
                                sbAddressesOut.Append("<ReliabilityNumber>" + sREL + "</ReliabilityNumber>")
                            Else
                                sbAddressesOut.Append("<ReliabilityNumber>" + "" + "</ReliabilityNumber>")
                            End If

                            If diagnosticSplit.Length > 2 Then
                                sbAddressesOut.Append("<AddressQuality>" + sAQF + "</AddressQuality>")
                            Else
                                sbAddressesOut.Append("<AddressQuality>" + "" + "</AddressQuality>")
                            End If

                            If diagnosticSplit.Length > 3 Then
                                sbAddressesOut.Append("<CarrierRoute>" + sCRT + "</CarrierRoute>")
                            Else
                                sbAddressesOut.Append("<CarrierRoute>" + "" + "</CarrierRoute>")
                            End If

                            If diagnosticSplit.Length > 4 Then
                                sbAddressesOut.Append("<DPV>" + sDPV + "</DPV>")
                            Else
                                sbAddressesOut.Append("<DPV>" + "" + "</DPV>")
                            End If

                            If diagnosticSplit.Length > 5 Then
                                sbAddressesOut.Append("<County>" + sCNTY + "</County>")
                            Else
                                sbAddressesOut.Append("<County>" + "" + "</County>")
                            End If

                            sbAddressesOut.Append("</address>")
                            '   dtEnd_ts = DateTime.Now
                            '  ts = dtEnd_ts.Subtract(dtStart_ts)

                            sDpvValidDesc = GetDpvValidCodesDesc(sDPV)
                            'subba-033108 

                            'if ((iMultipleAddrMatch == 1)) insertAddressValidatedAuditTrail(sPatHospCode, "M", "MutipleAddressesMatch ", "DCSAddressM", "ConsoleAddress"); 
                            insertCorrectedAddressDetailMultiple(sPatHospCode, sAddrType, address.address + " , " + address.line2, cszCity, cszState, cszZip, _
                            "", "", "", Convert.ToString(ts.Milliseconds), "M" + iMultipleAddrMatch.ToString(), sbAddressesOut.ToString(), _
                            sAddrXmlIn, address.barcode, "", "", "", "", _
                            "", "", "", "", "", "", _
                            "", "", sDpvValidDesc.Substring(0, 1), sDpvValidDesc, sKEY, sREL, _
                            sAQF, sCRT, sDPV, sCNTY, sAddr1, sAddr2, _
                            sCity, sState, sZip, iPatIdM, iPatPidM)
                        Else
                            isAddrErr = True
                            sbAddressesOut.Append("<address>")
                            sbAddressesOut.Append("<addresstype>" + sAddrType + "</addresstype>")
                            sbAddressesOut.Append("<address1>" + "" + "</address1>")
                            sbAddressesOut.Append("<address2>" + "" + "</address2>")
                            sbAddressesOut.Append("<city>" + "" + "</city>")
                            sbAddressesOut.Append("<state>" + "" + "</state>")
                            sbAddressesOut.Append("<zip>" + "" + "</zip>")
                            sbAddressesOut.Append("<dpvnotes>" + "Invalid Address" + " " + "Returns Empty Result" + "</dpvnotes>")
                            sbAddressesOut.Append("<BarcodeDigits>" + "" + "</BarcodeDigits>")
                            sbAddressesOut.Append("<CarrierRoute>" + "" + "</CarrierRoute>")
                            sbAddressesOut.Append("<CongressCode>" + "" + "</CongressCode>")
                            'sbAddressesOut.Append("<Corrections>" + testWsOut.Corrections + "</Corrections>") 
                            'sbAddressesOut.Append("<CorrectionsDesc>" + testWsOut.CorrectionsDesc + "</CorrectionsDesc>") 
                            sbAddressesOut.Append("<Fragment>" + "" + "</Fragment>")
                            sbAddressesOut.Append("<FragmentHouse>" + "" + "</FragmentHouse>")
                            sbAddressesOut.Append("<FragmentPMBNumber>" + "" + "</FragmentPMBNumber>")
                            sbAddressesOut.Append("<FragmentPMBPrefix>" + "" + "</FragmentPMBPrefix>")
                            sbAddressesOut.Append("<FragmentPostDir>" + "" + "</FragmentPostDir>")
                            sbAddressesOut.Append("<FragmentPreDir>" + "" + "</FragmentPreDir>")
                            sbAddressesOut.Append("<FragmentStreet>" + "" + "</FragmentStreet>")
                            sbAddressesOut.Append("<FragmentSuffix>" + "" + "</FragmentSuffix>")
                            sbAddressesOut.Append("<FragmentUnit>" + "" + "</FragmentUnit>")
                            sbAddressesOut.Append("<Source>" + "" + "</Source>")
                            sbAddressesOut.Append("</address>")
                            '    dtEnd_ts = DateTime.Now
                            '   ts = dtEnd_ts.Subtract(dtStart_ts)

                            insertCorrectedAddressDetailMultiple(sPatHospCode, sAddrType, "" + " , " + "", "", "", "", _
                            "", sTemp1, sTemp2, Convert.ToString(ts.Milliseconds), "N" + iMultipleAddrMatch.ToString(), Convert.ToString(sbAddressesOut), _
                            sAddrXmlIn, "", "", "", "", "", _
                            "", "", "", "", "", "", _
                            "", "", "", sDpvValidDesc, "", "", _
                            "", "", "", "", sAddr1, sAddr2, _
                            sCity, sState, sZip, iPatIdM, iPatPidM)
                        End If
                    Next
                End If

                sbAddressesOut.Append("</addressout>")
                sAddrXmlOut = sbAddressesOut.ToString()
                bResult = True 'Return True





            Catch ex As Exception
                sAddrXmlOut = Nothing 'AddrServerError
                If (ex.Message <> "AddrCustomException") Then
                    insertAddressValidatedAuditTrail(sPatHospCode, "X", "Error-X-" + ex.Message, "DCSAddressX", "DCSAddressSvrErr", sAddr1, sAddr2, sCity, sState, sZip, iPatIdM, iPatPidM, "0", iBatchIdAddr)
                    If (ex.Message.ToLower().IndexOf("object reference") > -1) Then
                        'R-RerunRequired-ObjectReferenceDataError, sAddrType null
                        insertAddressValidatedAuditTrail(sPatHospCode, "R", "Error-R-" + ex.Message, "DCSAddressR", "DCSAddressSvrObject", sAddr1, sAddr2, sCity, sState, sZip, iPatIdM, iPatPidM, "0", iBatchIdAddr) 'subba-082112




                        log.ExceptionDetails("ConsoleFireRulesByTankAddress-AddressProcessEndsHere:" + sPatHospCode + ":" + sAddr1 + "," + sAddr2 + "," + sCity + "," + sState + "," + sZip, ex) 'subba-062912




                    End If



                End If
                bResult = False 'Return False
            End Try






            _RuleFailed = bResult


            Return bResult 'subba-021910










        End Function

        Private Function insertCorrectedAddressDetail(ByVal sPat_hosp_code As String, ByVal sAddressType As String, ByVal sAddress As String, ByVal sCity As String, _
                                                      ByVal sState As String, ByVal sZip As String, ByVal sDpv_code As String, ByVal sDpv_desc As String,
                                                      ByVal sDpv_notes As String, ByVal sTimeUsed As String, ByVal sStatus As String, ByVal sOutputXml As String, _
                                                      ByVal sInputXml As String, ByVal sBar_code_digits As String, ByVal sCarrier_route As String, _
                                                      ByVal sCongress_code As String, ByVal sCorrections As String, ByVal sCorrections_desc As String, _
                                                      ByVal sFragment As String, ByVal sFragment_house As String, ByVal sFragment_pmb_number As String,
                                                      ByVal sFragment_pmb_prefix As String, ByVal sFragment_post_dir As String, ByVal sFragment_pre_dir As String, _
                                                      ByVal sFragment_street As String, ByVal sFragment_suffix As String, ByVal sFragment_unit As String, _
                                                      ByVal sSource As String, ByVal sReliability_key As String, ByVal sReliability_number As String, _
                                                      ByVal sAddress_quality As String, ByVal sUsps_carrier_route As String, ByVal sAdress_dpv As String, _
                                                      ByVal sCounty_code As String, ByVal strAddr1 As String, ByVal strAddr2 As String, _
                                                      ByVal strCity As String, ByVal strState As String, ByVal strZip As String, ByVal iPatientId As Integer, _
                                                      ByVal iPid As Integer) As Object




            'Dim ruleConn As New SqlConnection()
            'Dim ruleComm As SqlCommand
            '  Dim sqlString As String
            Dim errMessage As String
            Dim iReturnReocords As Integer = 0

            Try
                If sAddress.Trim().EndsWith(",") Then
                    sAddress = sAddress.Trim().TrimEnd(","c)
                End If
                '     sqlString = CfgUspAddCorrectAddress 'subba-022509 ' "usp_add_corrected_address_detail"



                'ruleConn.ConnectionString = SQL_CONNECTION_STRING
                'ruleConn.Open()


                'ruleComm = New SqlCommand(sqlString, ruleConn)

                'ruleComm.CommandType = CommandType.StoredProcedure



                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using ruleComm As New SqlCommand(CfgUspAddCorrectAddress, Con)
                        ruleComm.CommandType = CommandType.StoredProcedure

                        ruleComm.Parameters.Add("@pat_hosp_code", SqlDbType.VarChar).Value = sPat_hosp_code
                        ruleComm.Parameters.Add("@address_type", SqlDbType.VarChar).Value = sAddressType
                        ruleComm.Parameters.Add("@address", SqlDbType.VarChar).Value = sAddress
                        ruleComm.Parameters.Add("@city", SqlDbType.VarChar).Value = sCity
                        ruleComm.Parameters.Add("@state", SqlDbType.VarChar).Value = sState
                        ruleComm.Parameters.Add("@zip", SqlDbType.VarChar).Value = sZip
                        ruleComm.Parameters.Add("@dpv_code", SqlDbType.VarChar).Value = sDpv_code
                        ruleComm.Parameters.Add("@dpv_desc", SqlDbType.VarChar).Value = sDpv_desc

                        ruleComm.Parameters.Add("@dpv_notes", SqlDbType.VarChar).Value = sDpv_notes
                        ruleComm.Parameters.Add("@timeUsed", SqlDbType.VarChar).Value = sTimeUsed
                        ruleComm.Parameters.Add("@status", SqlDbType.VarChar).Value = sStatus
                        ruleComm.Parameters.Add("@outputXml", SqlDbType.VarChar).Value = sOutputXml
                        ruleComm.Parameters.Add("@inputXml", SqlDbType.VarChar).Value = sInputXml
                        ruleComm.Parameters.Add("@bar_code_digits", SqlDbType.VarChar).Value = sBar_code_digits
                        ruleComm.Parameters.Add("@carrier_route", SqlDbType.VarChar).Value = sCarrier_route
                        ruleComm.Parameters.Add("@congress_code", SqlDbType.VarChar).Value = sCongress_code

                        ruleComm.Parameters.Add("@corrections", SqlDbType.VarChar).Value = sCorrections
                        ruleComm.Parameters.Add("@corrections_desc", SqlDbType.VarChar).Value = sCorrections_desc
                        ruleComm.Parameters.Add("@fragment", SqlDbType.VarChar).Value = sFragment
                        ruleComm.Parameters.Add("@fragment_pmb_number", SqlDbType.VarChar).Value = sFragment_pmb_number
                        ruleComm.Parameters.Add("@fragment_pmb_prefix", SqlDbType.VarChar).Value = sFragment_pmb_prefix

                        ruleComm.Parameters.Add("@fragment_post_dir", SqlDbType.VarChar).Value = sFragment_post_dir
                        ruleComm.Parameters.Add("@fragment_pre_dir", SqlDbType.VarChar).Value = sFragment_pre_dir
                        ruleComm.Parameters.Add("@fragment_street", SqlDbType.VarChar).Value = sFragment_street
                        ruleComm.Parameters.Add("@fragment_suffix", SqlDbType.VarChar).Value = sFragment_suffix
                        ruleComm.Parameters.Add("@fragment_unit", SqlDbType.VarChar).Value = sFragment_unit
                        ruleComm.Parameters.Add("@source", SqlDbType.VarChar).Value = sSource

                        ruleComm.Parameters.Add("@reliability_key", SqlDbType.VarChar).Value = sReliability_key
                        ruleComm.Parameters.Add("@reliability_number", SqlDbType.VarChar).Value = sReliability_number
                        ruleComm.Parameters.Add("@address_quality", SqlDbType.VarChar).Value = sAddress_quality
                        ruleComm.Parameters.Add("@usps_carrier_route", SqlDbType.VarChar).Value = sUsps_carrier_route
                        ruleComm.Parameters.Add("@address_dpv", SqlDbType.VarChar).Value = sAdress_dpv
                        ruleComm.Parameters.Add("@county_code", SqlDbType.VarChar).Value = sCounty_code

                        ruleComm.Parameters.Add("@orig_address", SqlDbType.VarChar).Value = strAddr1
                        'subba-032808 
                        ruleComm.Parameters.Add("@orig_address2", SqlDbType.VarChar).Value = strAddr2
                        ruleComm.Parameters.Add("@orig_city", SqlDbType.VarChar).Value = strCity
                        ruleComm.Parameters.Add("@orig_state", SqlDbType.VarChar).Value = strState
                        ruleComm.Parameters.Add("@orig_zip", SqlDbType.VarChar).Value = strZip
                        ruleComm.Parameters.Add("@pid", SqlDbType.Int).Value = iPid
                        'subba-052108 
                        ruleComm.Parameters.Add("@patient_id", SqlDbType.Int).Value = iPatientId

                        iReturnReocords = ruleComm.ExecuteNonQuery()
                    End Using
                    Con.Close()
                End Using


            Catch sx As SqlException
                errMessage = sx.Message
                'insertExceptionDetails(ex.Message, ex.StackTrace, "AddressValidationWsLib.insertAddressValidatedAuditTrail()") 
                log.ExceptionDetails(_AppName + " insertCorrectedAddressDetail", sx)

            Catch ex As Exception

                errMessage = ex.Message
                'insertExceptionDetails(ex.Message, ex.StackTrace, "AddressValidationWsLib.insertAddressValidatedAuditTrail()") 
                log.ExceptionDetails(_AppName + " insertCorrectedAddressDetail", ex) 'subba-062912

            End Try

            Return iReturnReocords
        End Function


        Private Function insertCorrectedAddressDetailMultiple(ByVal sPat_hosp_code As String, ByVal sAddressType As String, ByVal sAddress As String, _
                                                              ByVal sCity As String, ByVal sState As String, ByVal sZip As String, ByVal sDpv_code As String, _
                                                              ByVal sDpv_desc As String, ByVal sDpv_notes As String, ByVal sTimeUsed As String, _
                                                              ByVal sStatus As String, ByVal sOutputXml As String, ByVal sInputXml As String, _
                                                              ByVal sBar_code_digits As String, ByVal sCarrier_route As String, ByVal sCongress_code As String, _
                                                              ByVal sCorrections As String, ByVal sCorrections_desc As String, ByVal sFragment As String, _
                                                              ByVal sFragment_house As String, ByVal sFragment_pmb_number As String, ByVal sFragment_pmb_prefix As String, _
                                                              ByVal sFragment_post_dir As String, ByVal sFragment_pre_dir As String, _
                                                              ByVal sFragment_street As String, ByVal sFragment_suffix As String, ByVal sFragment_unit As String, _
                                                              ByVal sSource As String, ByVal sReliability_key As String, ByVal sReliability_number As String, _
                                                              ByVal sAddress_quality As String, ByVal sUsps_carrier_route As String, ByVal sAdress_dpv As String, _
                                                              ByVal sCounty_code As String, ByVal strAddr1 As String, ByVal strAddr2 As String, _
                                                              ByVal strCity As String, ByVal strState As String, ByVal strZip As String, ByVal iPatientId As Integer, _
                                                              ByVal iPid As Integer) As Object




            'Dim ruleConn As New SqlConnection()
            'Dim ruleComm As SqlCommand
            '  Dim sqlString As String = String.Empty
            '   Dim errMessage As String
            Dim iReturnReocords As Integer = 0

            Try


                If sAddress.Trim().EndsWith(",") Then
                    sAddress = sAddress.Trim().TrimEnd(","c)
                End If

                ' sqlString = _CfgUspAddMultipleAddress 'subba-022509 '"usp_add_corrected_address_multiple_detail"


                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using ruleComm As New SqlCommand(CfgUspAddMultipleAddress, Con)
                        ruleComm.CommandType = CommandType.StoredProcedure


                        ruleComm.Parameters.Add("@pat_hosp_code", SqlDbType.VarChar).Value = sPat_hosp_code
                        ruleComm.Parameters.Add("@address_type", SqlDbType.VarChar).Value = sAddressType
                        ruleComm.Parameters.Add("@Address", SqlDbType.VarChar).Value = sAddress
                        ruleComm.Parameters.Add("@city", SqlDbType.VarChar).Value = sCity
                        ruleComm.Parameters.Add("@State", SqlDbType.VarChar).Value = sState
                        ruleComm.Parameters.Add("@zip", SqlDbType.VarChar).Value = sZip
                        ruleComm.Parameters.Add("@dpv_code", SqlDbType.VarChar).Value = sDpv_code
                        ruleComm.Parameters.Add("@dpv_desc", SqlDbType.VarChar).Value = sDpv_desc
                        ruleComm.Parameters.Add("@dpv_notes", SqlDbType.VarChar).Value = sDpv_notes
                        ruleComm.Parameters.Add("@timeUsed", SqlDbType.VarChar).Value = sTimeUsed
                        ruleComm.Parameters.Add("@status", SqlDbType.VarChar).Value = sStatus

                        ruleComm.Parameters.Add("@outputXml", SqlDbType.VarChar).Value = sOutputXml
                        ruleComm.Parameters.Add("@inputXml", SqlDbType.VarChar).Value = sInputXml
                        ruleComm.Parameters.Add("@bar_code_digits", SqlDbType.VarChar).Value = sBar_code_digits
                        ruleComm.Parameters.Add("@carrier_route", SqlDbType.VarChar).Value = sCarrier_route
                        ruleComm.Parameters.Add("@congress_code", SqlDbType.VarChar).Value = sCongress_code

                        ruleComm.Parameters.Add("@corrections", SqlDbType.VarChar).Value = sCorrections
                        ruleComm.Parameters.Add("@corrections_desc", SqlDbType.VarChar).Value = sCorrections_desc
                        ruleComm.Parameters.Add("@fragment", SqlDbType.VarChar).Value = sFragment_house
                        ruleComm.Parameters.Add("@fragment_pmb_number", SqlDbType.VarChar).Value = sFragment_pmb_number
                        ruleComm.Parameters.Add("@fragment_pmb_prefix", SqlDbType.VarChar).Value = sFragment_pmb_prefix

                        ruleComm.Parameters.Add("@fragment_post_dir", SqlDbType.VarChar).Value = sFragment_post_dir
                        ruleComm.Parameters.Add("@fragment_pre_dir", SqlDbType.VarChar).Value = sFragment_pre_dir
                        ruleComm.Parameters.Add("@fragment_street", SqlDbType.VarChar).Value = sFragment_street
                        ruleComm.Parameters.Add("@fragment_suffix", SqlDbType.VarChar).Value = sFragment_suffix
                        ruleComm.Parameters.Add("@fragment_unit", SqlDbType.VarChar).Value = sFragment_unit
                        ruleComm.Parameters.Add("@source", SqlDbType.VarChar).Value = sSource

                        ruleComm.Parameters.Add("@reliability_key", SqlDbType.VarChar).Value = sReliability_key
                        ruleComm.Parameters.Add("@reliability_number", SqlDbType.VarChar).Value = sReliability_number
                        ruleComm.Parameters.Add("@address_quality", SqlDbType.VarChar).Value = sAddress_quality
                        ruleComm.Parameters.Add("@usps_carrier_route", SqlDbType.VarChar).Value = sUsps_carrier_route
                        ruleComm.Parameters.Add("@address_dpv", SqlDbType.VarChar).Value = sAdress_dpv
                        ruleComm.Parameters.Add("@county_code", SqlDbType.VarChar).Value = sCounty_code

                        ruleComm.Parameters.Add("@orig_address", SqlDbType.VarChar).Value = strAddr1
                        'subba-032808 
                        ruleComm.Parameters.Add("@orig_address2", SqlDbType.VarChar).Value = strAddr2
                        ruleComm.Parameters.Add("@orig_city", SqlDbType.VarChar).Value = strCity
                        ruleComm.Parameters.Add("@orig_state", SqlDbType.VarChar).Value = strState
                        ruleComm.Parameters.Add("@orig_zip", SqlDbType.VarChar).Value = strZip
                        ruleComm.Parameters.Add("@pid", SqlDbType.Int).Value = iPid
                        'subba-052108 
                        ruleComm.Parameters.Add("@patient_id", SqlDbType.Int).Value = iPatientId
                        iReturnReocords = ruleComm.ExecuteNonQuery()


                    End Using
                    Con.Close()
                End Using


            Catch sx As SqlException

                '   errMessage = sx.Message
                log.ExceptionDetails(_AppName + " insertCorrectedAddressDetailMultiple", sx) 'subba-062912




            Catch ex As Exception

                '      errMessage = ex.Message
                log.ExceptionDetails(_AppName + " insertCorrectedAddressDetailMultiple", ex) 'subba-062912

            End Try

            Return iReturnReocords



        End Function

        Private Function insertAddressValidatedAuditTrail(ByVal sPat_hosp_code As String, ByVal sValidation_code As String, ByVal sValidation_result As String, _
                                                          ByVal sProvider_source As String, ByVal sModified_by As String, ByVal strAddr1 As String, _
                                                          ByVal strAddr2 As String, ByVal strCity As String, ByVal strState As String, ByVal strZip As String, _
                                                          ByVal iPatientID As Integer, ByVal iPatientPID As Integer, ByVal strDpv As String, _
                                                          ByVal iBatchIdAddress As Integer) As Object


            'Dim ruleConn As New SqlConnection()
            'Dim ruleComm As SqlCommand
            '  Dim sqlString As String
            ' Dim errMessage As String
            Dim iReturnReocords As Integer = 0

            Try

                '  sqlString = CfgUspAddressValidationTrail 'subba-022509 '"usp_add_address_validation_audit_trail"

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using ruleComm As New SqlCommand(CfgUspAddressValidationTrail, Con)
                        ruleComm.CommandType = CommandType.StoredProcedure

                        ruleComm.Parameters.Add("@pat_hosp_code", SqlDbType.VarChar).Value = sPat_hosp_code
                        ruleComm.Parameters.Add("@validation_code", SqlDbType.VarChar).Value = sValidation_code
                        ruleComm.Parameters.Add("@validation_result", SqlDbType.VarChar).Value = sValidation_result
                        ruleComm.Parameters.Add("@provider_source", SqlDbType.VarChar).Value = sProvider_source
                        ruleComm.Parameters.Add("@modified_by", SqlDbType.VarChar).Value = sModified_by

                        ruleComm.Parameters.Add("@orig_address", SqlDbType.VarChar).Value = strAddr1
                        'subba-032808 
                        ruleComm.Parameters.Add("@orig_address2", SqlDbType.VarChar).Value = strAddr2
                        ruleComm.Parameters.Add("@orig_city", SqlDbType.VarChar).Value = strCity
                        ruleComm.Parameters.Add("@orig_state", SqlDbType.VarChar).Value = strState
                        ruleComm.Parameters.Add("@orig_zip", SqlDbType.VarChar).Value = strZip
                        ruleComm.Parameters.Add("@patient_id", SqlDbType.Int).Value = iPatientID
                        ruleComm.Parameters.Add("@patient_pid", SqlDbType.Int).Value = iPatientPID
                        ruleComm.Parameters.Add("@address_dpv", SqlDbType.VarChar).Value = strDpv
                        ruleComm.Parameters.Add("@batch_id", SqlDbType.Int).Value = iBatchIdAddress 'subba-060508

                        iReturnReocords = ruleComm.ExecuteNonQuery()
                    End Using
                    Con.Close()
                End Using



            Catch sx As SqlException
                ' errMessage = sx.Message
                log.ExceptionDetails(_AppName + " insertAddressValidatedAuditTrail", sx)
            Catch ex As Exception

                '  errMessage = ex.Message
                log.ExceptionDetails(_AppName + " insertAddressValidatedAuditTrail", ex)

            End Try

            Return iReturnReocords
        End Function

        'Subba-061908 - added sAddrType as field
        Public Function ValidateAddressBySwappingAddressFields(ByVal iAttempt As Integer, ByVal sPatHospCode As String, ByVal sAddr1 As String, ByVal sAddr2 As String, _
                                                          ByVal sCity As String, ByVal sState As String, ByVal sZip As String, ByVal sCustomerID As String, _
                                                          ByVal sLicenseKey As String, ByVal iPatientId As Integer, ByVal iPid As Integer, ByVal sAddrType As String, _
                                                          ByVal iBatchId As Integer) As Integer



            Dim r As Integer = -1
            Dim ___isAttemptsSuccess As Boolean = False
            Dim ___AddrUspsFormat As String = String.Empty
            Dim sbAddressesOut As New StringBuilder("")
            'subba-040408 
            'Dim sAddrType As String = "PATIENT"
            Dim sAddrXmlIn As String = String.Empty
            Dim ts As TimeSpan
            Dim sOrigAddr1 As String = sAddr1, sOrigAddr2 As String = sAddr2

            Try
                sAddrXmlIn = String.Concat("<addressesin><address><address1>", sAddr1, "</address1><address2>", sAddr2, "</address2><city>", sCity, _
                "</city><state>", sState, "</state><zip>", sZip, "</zip><dpvnotes></dpvnotes></address></addressesin>")

                Dim cost As Integer = 0
                Select Case iAttempt
                    Case 1
                        sAddr2 = sOrigAddr1
                        sAddr1 = sOrigAddr2
                        Exit Select
                    Case 2
                        sAddr2 = String.Empty
                        'sAddr1 = sOrigAddr1; 
                        Exit Select
                    Case 3
                        sAddr1 = sAddr2
                        sAddr2 = String.Empty
                        Exit Select
                    Case Else
                        ___AddrUspsFormat = "usps"  'subba-012109
                        'if (Char.IsDigit(sAddr1, 0)) // street number starts with isNumeric 
                        '{ 
                        ' if (sAddr2.Length > 15) sAddr2 = string.empty; 
                        '} 
                        'else 
                        '{ 
                        ' sAddr1 = sAddr2; sAddr2 = string.empty; 
                        '} 

                        'if (!Char.IsDigit(sAddr1, 0)) return; //Don't even try address verification 

                        'int outStNumberOnly = 0; // Only digits in sAddr1 not acceptable 
                        'if (Int32.TryParse(sAddr1, out outStNumberOnly)) 
                        '{ 
                        ' if (outStNumberOnly != 0) return; //Don't even try address verification when address field has only numbers 
                        '} 
                        Exit Select
                End Select

                Dim addresses() As dcsAddrWS.AESRawAddress ' = New dcsAddrWS.AESRawAddress() 'subba-060508
                Dim objAddrWsIn As dcsAddrWS.DcsAddress = New dcsAddrWS.DcsAddress() 'subba-060508
                'addresses = objAddrWsIn.VerifyDCSAddressRaw("1255 15th st", "", "Plano", "TX", "", "21", "192.168.11.211", "7500", "")
                addresses = objAddrWsIn.VerifyAddressEnhanced(sAddr1, sAddr2, sCity, sState, sZip, sCustomerID, sLicenseKey)

                If (addresses Is Nothing) Then   'subba-022510 commented 'just write log  'subba-042911 'commented
                    objAddrWsIn.Url = CfgAddrSvcUrl2 'SUBBA-062312
                    addresses = objAddrWsIn.VerifyAddressEnhanced(sAddr1, sAddr2, sCity, sState, sZip, sCustomerID, sLicenseKey) 'subba-062112
                    If (addresses.Length = 0) Then Throw New NullReferenceException("DCSAddressValidateDPV Server Exception-TrySec") 'SUBBA-062312
                    'Throw New NullReferenceException("DCSAddressValidateDPV Server Exception")
                End If
                'If (addresses Is Nothing) Then 'subba-042911
                '    insertAddressValidatedAuditTrail(sPatHospCode, "X", "AddressSystemErrorSwappingAddressFields-" + iAttempt.ToString(), "DCSAddress-" + sAddrType, "ConsoleDCSAddress", sAddr1, sAddr2, sCity, sState, sZip, iPatientId, iPid, "", iBatchId) 'subba-042911
                '    Throw New NullReferenceException("DCSAddressValidateDPV Server Exception")
                'End If
                objAddrWsIn.Url = CfgAddrSvcUrl1  'SUBBA-062312 'revert back

                Dim cszSplit As String() = Nothing
                Dim diagnosticSplit As String() = Nothing
                Dim sKEY As String = String.Empty, sREL As String = String.Empty, sAQF As String = String.Empty, sCRT As String = String.Empty, sDPV As String = String.Empty, sCNTY As String = String.Empty
                Dim cszCity As String = String.Empty, cszState As String = String.Empty, cszZip As String = String.Empty
                '******************************************************************************************************************



                If (addresses Is Nothing) Then
                    Exit Function 'subba-022510
                End If


                If (Not addresses Is Nothing And addresses.Length = 1 AndAlso addresses(0).csz.Trim().Length > 3) Then 'subba-022510   If  (addresses.Length = 1) Then
                    If addresses(0).diagnostics.Trim().Length > 24 Then
                        diagnosticSplit = addresses(0).diagnostics.ToUpper().Replace("//", "").Split(" "c)
                    End If

                    If diagnosticSplit.Length > 0 Then
                        sKEY = diagnosticSplit(0).Replace("KEY=", "")
                    End If
                    If diagnosticSplit.Length > 1 Then
                        sREL = diagnosticSplit(1).Replace("REL=", "")
                    End If
                    If diagnosticSplit.Length > 2 Then
                        sAQF = diagnosticSplit(2).Replace("AQF=", "")
                    End If
                    If diagnosticSplit.Length > 3 Then
                        sCRT = diagnosticSplit(3).Replace("CRT=", "")
                    End If
                    If diagnosticSplit.Length > 4 Then
                        sDPV = diagnosticSplit(4).Replace("DPV=", "")
                    End If
                    If diagnosticSplit.Length > 5 Then
                        sCNTY = diagnosticSplit(5).Replace("CNTY=", "")
                    End If

                    If addresses(0).csz.Trim().Length > 3 Then
                        cszSplit = addresses(0).csz.Split(" "c)
                    End If
                    If cszSplit.Length = 3 Then
                        cszZip = cszSplit(cszSplit.Length - 1)
                        cszState = cszSplit(cszSplit.Length - 2)
                        cszCity = cszSplit(cszSplit.Length - 3)
                    End If
                    If cszSplit.Length = 4 Then
                        cszZip = cszSplit(cszSplit.Length - 1)
                        cszState = cszSplit(cszSplit.Length - 2)
                        cszCity = cszSplit(cszSplit.Length - 4) + " " + cszSplit(cszSplit.Length - 3)
                    End If
                    If cszSplit.Length = 5 Then
                        cszZip = cszSplit(cszSplit.Length - 1)
                        cszState = cszSplit(cszSplit.Length - 2)
                        cszCity = cszSplit(cszSplit.Length - 5) + " " + cszSplit(cszSplit.Length - 4) + " " + cszSplit(cszSplit.Length - 3)
                    End If
                    If cszSplit.Length = 6 Then
                        cszZip = cszSplit(cszSplit.Length - 1)
                        cszState = cszSplit(cszSplit.Length - 2)
                        cszCity = cszSplit(cszSplit.Length - 6) + " " + cszSplit(cszSplit.Length - 5) + " " + cszSplit(cszSplit.Length - 4) + " " + cszSplit(cszSplit.Length - 3)
                    End If

                    sbAddressesOut.Append("<address>")
                    sbAddressesOut.Append("<addresstype>" + sAddrType.ToUpper() + "</addresstype>")
                    sbAddressesOut.Append("<address1>" + addresses(0).address + "</address1>")
                    sbAddressesOut.Append("<address2>" + addresses(0).line2 + "</address2>")
                    sbAddressesOut.Append("<city>" + cszCity + "</city>")
                    'addresses[0].city 
                    sbAddressesOut.Append("<state>" + cszState + "</state>")
                    'addresses[0].state 
                    sbAddressesOut.Append("<zip>" + cszZip + "</zip>")
                    'addresses[0].zip 
                    sbAddressesOut.Append("<dpvnotes>" + addresses(0).diagnostics + "</dpvnotes>")
                    sbAddressesOut.Append("<BarcodeDigits>" + addresses(0).barcode + "</BarcodeDigits>")
                    '----------------------------------diagnostic info ------------------- 
                    If diagnosticSplit.Length > 0 Then
                        sbAddressesOut.Append("<ReliabilityKey>" + sKEY + "</ReliabilityKey>")
                    Else
                        sbAddressesOut.Append("<ReliabilityKey>" + "" + "</ReliabilityKey>")
                    End If

                    If diagnosticSplit.Length > 1 Then
                        sbAddressesOut.Append("<ReliabilityNumber>" + sREL + "</ReliabilityNumber>")
                    Else
                        sbAddressesOut.Append("<ReliabilityNumber>" + "" + "</ReliabilityNumber>")
                    End If

                    If diagnosticSplit.Length > 2 Then
                        sbAddressesOut.Append("<AddressQuality>" + sAQF + "</AddressQuality>")
                    Else
                        sbAddressesOut.Append("<AddressQuality>" + "" + "</AddressQuality>")
                    End If

                    If diagnosticSplit.Length > 3 Then
                        sbAddressesOut.Append("<CarrierRoute>" + sCRT + "</CarrierRoute>")
                    Else
                        sbAddressesOut.Append("<CarrierRoute>" + "" + "</CarrierRoute>")
                    End If

                    If diagnosticSplit.Length > 4 Then
                        sbAddressesOut.Append("<DPV>" + sDPV + "</DPV>")
                    Else
                        sbAddressesOut.Append("<DPV>" + "" + "</DPV>")
                    End If

                    If diagnosticSplit.Length > 5 Then
                        sbAddressesOut.Append("<County>" + sCNTY + "</County>")
                    Else
                        sbAddressesOut.Append("<County>" + "" + "</County>")
                    End If

                    sbAddressesOut.Append("</address>")

                    'dtEnd_ts = DateTime.Now
                    ' ts = dtEnd_ts.Subtract(dtStart_ts)
                    Dim sDpvValidDesc As String = GetDpvValidCodesDesc(sDPV)
                    'subba-033108 
                    'System.Drawing.Graphics g = this.pictureBox1.CreateGraphics(); //BARCODE 
                    'HandlePostnetDrawing(g); 

                    insertCorrectedAddressDetail(sPatHospCode, sAddrType, addresses(0).address + " , " + addresses(0).line2, cszCity, cszState, cszZip, _
                    "", "", "", Convert.ToString(ts.Milliseconds), "Y", sbAddressesOut.ToString(), _
                    sAddrXmlIn, addresses(0).barcode, "", "", "", "", _
                    "", "", "", "", "", "", _
                    "", "", sDpvValidDesc.Substring(0, 1), sDpvValidDesc, sKEY, sREL, _
                    sAQF, sCRT, sDPV, sCNTY, sOrigAddr1, sOrigAddr2, _
                    sCity, sState, sZip, iPatientId, iPid)
                    'don't do further attempts  
                    'corrected address 
                    'insertAddressValidatedAuditTrail(sPatHospCode, "C1", "Error-NoAddressMatch-Empty Result", "DCSAddressC1", "ConsoleAddress", sOrigAddr1, sOrigAddr2, sCity, sState, sZip); 
                    If (___AddrUspsFormat = "usps") Then 'subba-012109
                        insertAddressValidatedAuditTrail(sPatHospCode, "C", "Valid Address", "DCSAddress-" + sAddrType, "ConsoleDCSAddress-VerifyAddressEnhanced-usps", sAddr1, sAddr2, sCity, sState, sZip, iPatientId, iPid, sDPV, iBatchId) 'subba-061908
                    Else
                        insertAddressValidatedAuditTrail(sPatHospCode, "C", "Valid Address", "DCSAddress-" + sAddrType, "ConsoleDCSAddress-VerifyAddressEnhanced", sAddr1, sAddr2, sCity, sState, sZip, iPatientId, iPid, sDPV, iBatchId) 'subba-061908
                    End If
                    'insertAddressValidatedAuditTrail(sPatHospCode, "C", "Valid Address", "DCSAddress-" + sAddrType, "ConsoleDCSAddress-VerifyAddressEnhanced", sAddr1, sAddr2, sCity, sState, sZip, iPatientId, iPid, sDPV, iBatchId) 'subba-061908
                    ___isAttemptsSuccess = True
                Else
                    ___isAttemptsSuccess = False
                End If




            Catch ex As Exception

                log.ExceptionDetails("ConsoleDCSAddressValidateAddressBySwappingAddressFields-4Attempts-AddressNullObj-CustomException-" + sPatHospCode, ex) 'subba-022510 'Not required to log failed attempts due to swapping address fields 
            End Try
            ___AddrUspsFormat = String.Empty



            Return r

        End Function


        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="ADDRESS_1"></param>
        ''' <param name="ADDRESS_2"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getUspsFormatAddressToReprocess(ByRef ADDRESS_1 As String, ByVal ADDRESS_2 As String) As String



            Dim fullSteetAddress As String = (ADDRESS_1 & " ") + ADDRESS_2
            Dim resultString As String = Nothing



            Try
                'RULE:100 "INVALID_SPACE"--(?<name>\d+ \w )
                resultString = Regex.Match(fullSteetAddress, "(?<RULE_100>\d+ \w )", RegexOptions.IgnoreCase).Groups("RULE_100").Value
                If resultString.Length > 0 Then
                    fullSteetAddress = fullSteetAddress.ToString().Replace(resultString, "|%RULE_100%|")
                    resultString = resultString.ToString().Replace(" ", "")
                    resultString = resultString & " "
                    fullSteetAddress = fullSteetAddress.ToString().Replace("|%RULE_100%|", resultString)
                End If

                resultString = Regex.Match(fullSteetAddress, "(?<RULE_150>\d+ \w$)", RegexOptions.IgnoreCase).Groups("RULE_150").Value
                If resultString.Length > 0 Then
                    'RULE:150 "INVALID_SPACE"--(?<name>\d+ \w$)
                    fullSteetAddress = fullSteetAddress.ToString().Replace(resultString, "|%RULE_150%|")
                    'resultString.Replace(" ", ""); 
                    resultString = resultString.ToString().Replace(" ", "")
                    fullSteetAddress = fullSteetAddress.ToString().Replace("|%RULE_150%|", resultString)
                    'fullSteetAddress.ToString().Replace("%RULE_150%", resultString); 
                End If

                resultString = Regex.Match(fullSteetAddress, "(?<RULE_200> lot#\d*)", RegexOptions.IgnoreCase).Groups("RULE_200").Value
                If resultString.Length > 0 Then
                    'RULE:200 "LOT#" 
                    fullSteetAddress = fullSteetAddress.ToString().Replace(resultString, "|%RULE_200%|")
                    'resultString.Replace("#", ""); 
                    resultString = resultString.ToString().Replace("#", "")
                    fullSteetAddress = fullSteetAddress.ToString().Replace("|%RULE_200%|", resultString)
                    'fullSteetAddress.ToString().Replace("%RULE_200%", resultString); 
                End If

                'RULE:2 PREFIX_ABBREVATION --> INVALID per USPS
                If ADDRESS_1.IndexOf("'") > 0 Then
                    ADDRESS_1 = ADDRESS_1.Replace("'", "''")
                End If

                If ADDRESS_2.IndexOf("'") > 0 Then
                    ADDRESS_2 = ADDRESS_2.Replace("'", "''")
                End If




                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(CfgUspFormatAddress & " '" & fullSteetAddress & "'", Con)
                        fullSteetAddress = cmd.ExecuteScalar().ToString()
                    End Using
                    Con.Close()
                End Using




            Catch sx As SqlException
                log.ExceptionDetails(_AppName + " getUspsFormatAddressToReprocess :", sx, fullSteetAddress)


            Catch ex As Exception
                log.ExceptionDetails(_AppName + " getUspsFormatAddressToReprocess :", ex, fullSteetAddress)
            End Try

            ADDRESS_1 = fullSteetAddress 'subba-012109 ' Byref 
            Return fullSteetAddress

        End Function





        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="RAW_DPV_CODE_IN"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetDpvValidCodesDesc(ByVal RAW_DPV_CODE_IN As String) As String





            Dim strDpvDesc As String = String.Empty
            Dim sDPVCode As String = "INVALID DPV CODE"
            Dim iRawDpvCode As Integer = Integer.Parse(RAW_DPV_CODE_IN)
            Dim isDPVValid1 As Boolean = False
            Dim isDPVValid2 As Boolean = False
            Dim sErrMsg As String = String.Empty

            Dim iCode1 As Integer = Integer.Parse(RAW_DPV_CODE_IN) And 7
            ' BitWise operator 
            Dim iCode2 As Integer = Integer.Parse(RAW_DPV_CODE_IN) And 3
            Dim iCode3 As Integer = Integer.Parse(RAW_DPV_CODE_IN) And 4
            ' CMRA - commercial Mail Receipient Address 




            Try

                If iCode1 = 0 Then
                    sDPVCode = "N~DPV~Certified but not DPV Valid"
                    isDPVValid1 = False
                Else
                    isDPVValid1 = True
                End If

                If isDPVValid1 Then
                    Select Case iCode2
                        Case 1
                            sDPVCode = "D~DPV~Certified as default Zip+4 record"
                            isDPVValid2 = True
                            Exit Select
                        Case 2
                            sDPVCode = "S~DPV~Certified and DPV Valid dropping secondary number"
                            isDPVValid2 = True
                            Exit Select
                        Case 3
                            sDPVCode = "Y~DPV~Certified and DPV valid all Components"
                            isDPVValid2 = True
                            Exit Select
                            'goto case 0; 
                            'default: 
                            ' Console.WriteLine("Invalid selection. Please select 1, 2, or 3."); 
                            ' break; 

                    End Select
                End If

                If iRawDpvCode > 3 Then
                    If isDPVValid2 Then
                        Select Case iCode3
                            Case 0
                                sDPVCode = "N~CMRA~DPV Valid but not a CMRA location" + sDPVCode
                                Exit Select
                            Case 4
                                sDPVCode = "Y~CMRA~DPV Valid and is CMRA location" + sDPVCode
                                Exit Select
                        End Select
                    Else
                        sDPVCode = "N~DPV~INVALID DPV CODE"

                    End If


                End If
            Catch ex As Exception
                sErrMsg = ex.Message
                sDPVCode = "UNKNOWN"
                log.ExceptionDetails(_AppName + " GetDpvValidCodesDesc", ex)


            End Try
            Return sDPVCode

        End Function





        Private Function SendRowsToProcess() As Integer


            Dim r As Integer = 0
            Try

                Dim pipeClient As New NamedPipeClientStream(".", _AppName, PipeDirection.Out, PipeOptions.Asynchronous, TokenImpersonationLevel.Impersonation)

                pipeClient.Connect()

                Dim sss As New StreamString(pipeClient)

                '      sss.WriteString("iRulesToRun|" + Convert.ToString(iRulesToRun))
                pipeClient.Close()

                sss = Nothing

            Catch ex As Exception
                r = -1
                log.ExceptionDetails(_AppName, ex)
            End Try

            Return r


        End Function








        Public WriteOnly Property htRuleMsgs As Hashtable

            Set(value As Hashtable)
                _htRuleMsgs = value
            End Set

        End Property


        Public WriteOnly Property AddressFilter As String

            Set(value As String)
                _AddressFilter = value
            End Set

        End Property



        Public WriteOnly Property GetMultipleAddress As String

            Set(value As String)
                _GetMultipleAddress = value
            End Set

        End Property


        Public WriteOnly Property iMaxRowsToProcess As Integer

            Set(value As Integer)
                _iMaxRowsToProcess = value
            End Set

        End Property


        Public WriteOnly Property JobID As Integer

            Set(value As Integer)
                _JobID = value
            End Set

        End Property


        Public WriteOnly Property AppName As String

            Set(value As String)
                _AppName = value
            End Set
        End Property


        'Public WriteOnly Property USP_GET_PAT_AUDIT_TRAIL_HL7_ROWS As String

        '    Set(value As String)
        '        _USP_GET_PAT_AUDIT_TRAIL_HL7_ROWS = value
        '    End Set
        'End Property

        Public WriteOnly Property PATIENT_AUDIT_TRAIL_ID As Integer

            Set(value As Integer)
                _PATIENT_AUDIT_TRAIL_ID = value
            End Set
        End Property


        Public WriteOnly Property PAT_HOSP_CODE As String

            Set(value As String)
                _PAT_HOSP_CODE = value
            End Set
        End Property



        Public WriteOnly Property PID As Integer

            Set(value As Integer)
                _PID = value
            End Set
        End Property

        Public WriteOnly Property PATIENT_ID As Integer

            Set(value As Integer)
                _PATIENT_ID = value
            End Set
        End Property


        Public WriteOnly Property EVENT_DETAIL As String

            Set(value As String)
                _EVENT_DETAIL = value
            End Set
        End Property


        Public WriteOnly Property EVENT_TYPE As String

            Set(value As String)
                _EVENT_TYPE = value
            End Set
        End Property


        Public WriteOnly Property CLIENT_FACILITY_CODE As String

            Set(value As String)
                _CLIENT_FACILITY_CODE = value
            End Set
        End Property


        Public WriteOnly Property CREATE_DATE As String

            Set(value As String)
                _CREATE_DATE = value
            End Set
        End Property


        Public WriteOnly Property ADMITTING_DATE As String

            Set(value As String)
                _ADMITTING_DATE = value
            End Set
        End Property

        Public WriteOnly Property DISCHARGE_DATE As String

            Set(value As String)
                _DISCHARGE_DATE = value
            End Set
        End Property



        Public WriteOnly Property ContextID As Integer

            Set(value As Integer)
                _RULE_CONTEXT_ID = value
            End Set
        End Property

        Public WriteOnly Property HospCode As String

            Set(value As String)
                CfgHospCode = value
            End Set
        End Property

        Public WriteOnly Property ErrorLogPath As String

            Set(value As String)
                _ErrorLogPath = value
            End Set
        End Property

        Public WriteOnly Property DebugMode As String

            Set(value As String)
                _DebugMode = value
            End Set
        End Property

        'Public WriteOnly Property RuleDebugMode As String

        '    Set(value As String)
        '        _RuleDebugMode = value
        '    End Set
        'End Property

        Public WriteOnly Property runMode As String

            Set(value As String)
                sRunMode = value
            End Set
        End Property

        Public WriteOnly Property resultAll As String

            Set(value As String)
                _resultAll = value
            End Set
        End Property

        Public WriteOnly Property RuleSuccess As String

            Set(value As String)
                _ruleSuccess = value
            End Set
        End Property

        Public WriteOnly Property raClientIP As String

            Set(value As String)
                sRaClientIP = value
            End Set
        End Property

        Public WriteOnly Property ruleDisplayLimit As String

            Set(value As String)
                _ruleDisplayLimit = value
            End Set
        End Property

        Public WriteOnly Property UspGetAllData As String

            Set(value As String)
                CfgUspGetAllData = value
            End Set
        End Property
        'SUBBA-20160713
        Public WriteOnly Property UspGetAllDataHL7Rows As String

            Set(value As String)
                CfgUspGetAllDataHL7Rows = value 'SUBBA-20160713
            End Set
        End Property

        Public WriteOnly Property UspInsertSchedulerlog As String

            Set(value As String)
                CfgUspInsertSchedulerlog = value
            End Set
        End Property

        Public WriteOnly Property UspGetRuleMsg As String

            Set(value As String)
                CfgUspGetRuleMsg = value
            End Set
        End Property

        Public WriteOnly Property UspRulesToFire As String

            Set(value As String)
                CfgUspRulesToFire = value
            End Set
        End Property

        Public WriteOnly Property UspApplyPatAuditTrial As String

            Set(value As String)
                CfgUspApplyPatAuditTrial = value
            End Set
        End Property

        Public WriteOnly Property UspTankAddress As String

            Set(value As String)
                CfgUspTankAddress = value
            End Set
        End Property

        Public WriteOnly Property UspAddCorrectAddress As String

            Set(value As String)

                CfgUspAddCorrectAddress = value

            End Set
        End Property

        Public WriteOnly Property UspAddMultipleAddress As String

            Set(value As String)
                CfgUspAddMultipleAddress = value
            End Set
        End Property

        Public WriteOnly Property UspFormatAddress As String

            Set(value As String)
                CfgUspFormatAddress = value
            End Set
        End Property

        Public WriteOnly Property UspAddressValidationTrail As String

            Set(value As String)
                CfgUspAddressValidationTrail = value
            End Set
        End Property

        Public WriteOnly Property UspGetAllTankById As String

            Set(value As String)
                CfgUspGetAllTankById = value
            End Set
        End Property

        Public WriteOnly Property UspRuleInsert As String

            Set(value As String)
                CfgUspRuleInsert = value

            End Set
        End Property

        Public WriteOnly Property UspRuleResultDelete As String

            Set(value As String)
                CfgUspRuleResultDelete = value
            End Set
        End Property

        Public WriteOnly Property UspRuleInsertByXmlTank As String

            Set(value As String)
                CfgUspRuleInsertByXmlTank = value
            End Set
        End Property

        Public WriteOnly Property UspRuleInsertDebug As String

            Set(value As String)
                CfgUspRuleInsertDebug = value

            End Set
        End Property

        Public WriteOnly Property addrSvcUrl1 As String

            Set(value As String)
                CfgAddrSvcUrl1 = value
            End Set
        End Property

        Public WriteOnly Property addrSvcUrl2 As String

            Set(value As String)
                CfgAddrSvcUrl2 = value
            End Set
        End Property

        Public WriteOnly Property AddressRunMode As String

            Set(value As String)

                _AddressRunMode = value

            End Set
        End Property

        Public WriteOnly Property addrSvcPingLogMin As String

            Set(value As String)
                _AddrSvcPingLogMin = value
            End Set
        End Property

        Public WriteOnly Property CustomerID As String

            Set(value As String)
                _CUSTOMER_ID = value
            End Set
        End Property

        Public WriteOnly Property LicenseKey As String

            Set(value As String)
                _LICENSE_KEY = value
            End Set
        End Property



        Public WriteOnly Property MED_RECORD_NUMBER As String

            Set(value As String)
                _MED_RECORD_NUMBER = value
            End Set
        End Property

        Public WriteOnly Property RUN_AVS_OUT As String

            Set(value As String)
                _RUN_AVS_OUT = value
            End Set
        End Property


        Public WriteOnly Property Verbose As Integer

            Set(value As Integer)
                _Verbose = value
            End Set
        End Property

        Public WriteOnly Property LogFilePath As String

            Set(value As String)
                _LogFilePath = value
            End Set
        End Property




        Public WriteOnly Property RULE_MESSAGE_DELIMETER As String

            Set(value As String)
                _RULE_MESSAGE_DELIMETER = value
            End Set
        End Property

        Public WriteOnly Property dsRulesToFire As DataSet

            Set(value As DataSet)
                _dsRulesToFire = value
            End Set
        End Property




        Public WriteOnly Property Metrics As Integer

            Set(value As Integer)
                _Metrics = value
            End Set

        End Property



        Public WriteOnly Property Monitoring As Integer

            Set(value As Integer)
                _Monitoring = value
            End Set

        End Property


        Public ReadOnly Property MetricString As String

            Get
                Return _MetricString
            End Get


        End Property


    End Class
End Namespace    

