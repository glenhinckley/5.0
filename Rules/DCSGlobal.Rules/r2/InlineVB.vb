
Option Explicit On

'Imports System.Collections.Specialized
'Imports System.Text.RegularExpressions
'Imports System.Collections.Generic
Imports System.Data.SqlClient
'Imports System.Configuration
'Imports System.Net.Security
Imports System.Threading
'Imports System.Timers
'Imports System.Web
'Imports System.Net
Imports System.IO
Imports System.Data
'Imports System.Data.SqlTypes
'Imports System.Reflection

'Imports System.Diagnostics
'Imports System.Globalization


Imports System.IO.Pipes
Imports System.Security.Principal
Imports System


'Imports System.Collections
Imports System.Text

Imports DCSGlobal.BusinessRules.Logging
Imports DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff
Imports DCSGlobal.BusinessRules.CoreLibraryII
Imports DCSGlobal.RealAlert.DCSGlobal.RealAlert

Namespace FireRules





    Public Class InlineVB




        Implements IDisposable



        Private log As New logExecption()
        Private sch As New SchedulerLog()
        Private Met As New Metrics()

        Private _LogFilePath As String = String.Empty


        Private _Verbose As Integer = 1

        Private _LastMSG As String = String.Empty
        Private _iMaxRowsToProcess As Integer = 0

        Private iRowsToProcess As Integer = 0
        Private _dsRulesToFire As New DataSet
        Private _TotalRulesToFire As Integer = 0

        Private _tguid As Guid = Guid.Empty
        Private _sguid As String = String.Empty

        Private _JobID As Integer = 0
        Private _BatchID As Integer = 0

        Private c As New Config()

        Private _ConnectionString As String = String.Empty

        Private disposedValue As Boolean ' To detect redundant calls
        Private dbTempString As String = String.Empty

        Private _htRuleMsgs As Hashtable

        Private _AddressFilter As String = String.Empty
        Private _GetMultipleAddress As String = String.Empty
        Private _RULE_MESSAGE_DELIMETER As String = String.Empty


        Private _RUN_RULES As String = "N"
        Private _Guid As New Guid

        Private _Metrics As Integer = 0
        Private _Monitoring As Integer = 0
        Private _MetricString As String = String.Empty

        Private _useDLL As Boolean = False
        Private _dll As String = String.Empty


        Dim _id As Integer = 0

        Dim _AppName As String = "ProcessEligilbilty class libary v13.1"

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                End If

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
                Met.ConnectionString = value
            End Set
        End Property

        Public WriteOnly Property useDLL As Boolean

            Set(value As Boolean)
                _useDLL = value
            End Set
        End Property



        Public WriteOnly Property dsRulesToFire As DataSet

            Set(value As DataSet)
                _dsRulesToFire = value
            End Set
        End Property


        Public Function Go(ByVal number As Integer) As Integer

            Dim r As Integer = -1

            Met.AddMetric("begin inline.go for " + Convert.ToString(number))
            '  Console.WriteLine("b1")
            '    SendRowsToProcess("BEGIN")
            'Console.WriteLine("b2")

            _LastMSG = ("go entered for " + Convert.ToString(number))

            If (_Verbose = 1) Then



                ' log.ExceptionDetails(_AppName, _LastMSG)
                '   Console.WriteLine(_LastMSG)



            End If
            'Console.WriteLine("10")

            'get rules to fire


            If (_sguid = "") Then
                _sguid = "999"
                '   Console.WriteLine(_LastMSG)


            Else

                ' take the number from the first sp 

                Dim _PATIENT_AUDIT_TRAIL_ID As Integer = 0
                Dim _PAT_HOSP_CODE As String = String.Empty
                Dim _PID As String = String.Empty
                Dim _PATIENT_ID As String = String.Empty
                Dim _EVENT_DETAIL As String = String.Empty
                Dim _EVENT_TYPE As String = String.Empty
                Dim _CLIENT_FACILITY_CODE As String = String.Empty
                Dim _CREATE_DATE As String = String.Empty
                Dim _ADMITTING_DATE As String = String.Empty
                Dim _DISCHARGE_DATE As String = String.Empty
                Dim _MED_RECORD_NUMBER As String = String.Empty
                Dim _RUN_AVS_OUT As String = String.Empty





                If (_Verbose = 1) Then

                    _LastMSG = ("Calling " + c.USP_GET_PAT_AUDIT_TRAIL_HL7_ROWS + " for number  " + Convert.ToString(number))

                    '  log.ExceptionDetails(_AppName, _LastMSG)
                    '      Console.WriteLine(_LastMSG)



                End If

                ' Console.WriteLine("13")

                Using con As New SqlConnection(_ConnectionString)
                    '    Console.WriteLine("14")
                    con.Open()
                    Using cmd As New SqlCommand(c.USP_GET_PAT_AUDIT_TRAIL_HL7_ROWS, con)
                        '       Console.WriteLine("15")
                        cmd.Parameters.AddWithValue("@PATIENT_AUDIT_TRAIL_ID", number)

                        cmd.CommandType = CommandType.StoredProcedure
                        Using idr = cmd.ExecuteReader()
                            '          Console.WriteLine("16")

                            If idr.HasRows Then
                                '             Console.WriteLine("17")
                                Dim i As Integer

                                Do While idr.Read
                                    '                Console.WriteLine("18")

                                    If (_Verbose = 1) Then

                                        _LastMSG = ("Start row  " + Convert.ToString(number) + " for number " + Convert.ToString(number))

                                        'log.ExceptionDetails(_AppName, _LastMSG)
                                        '                   Console.WriteLine(_LastMSG)



                                    End If


                                    i = Convert.ToInt32(idr.Item("PATIENT_AUDIT_TRAIL_ID"))

                                    Try

                                        _PATIENT_AUDIT_TRAIL_ID = i
                                        _PAT_HOSP_CODE = String.Empty
                                        _PID = String.Empty
                                        _PATIENT_ID = String.Empty
                                        _EVENT_DETAIL = String.Empty
                                        _EVENT_TYPE = String.Empty
                                        _CLIENT_FACILITY_CODE = String.Empty
                                        _CREATE_DATE = String.Empty
                                        _ADMITTING_DATE = String.Empty
                                        _DISCHARGE_DATE = String.Empty
                                        _MED_RECORD_NUMBER = String.Empty
                                        _RUN_AVS_OUT = String.Empty



                                        If Not IsDBNull(idr.Item("PATIENT_AUDIT_TRAIL_ID")) Then
                                            _PATIENT_AUDIT_TRAIL_ID = Convert.ToInt32(idr.Item("PATIENT_AUDIT_TRAIL_ID"))
                                        End If


                                        If Not IsDBNull(idr.Item("PAT_HOSP_CODE")) Then
                                            _PAT_HOSP_CODE = Convert.ToString(idr.Item("PAT_HOSP_CODE"))
                                        End If

                                        If Not IsDBNull(idr.Item("PID")) Then
                                            _PID = Convert.ToString(idr.Item("PID"))
                                        End If


                                        If Not IsDBNull(idr.Item("PATIENT_ID")) Then
                                            _PATIENT_ID = Convert.ToString(idr.Item("PATIENT_ID"))
                                        End If


                                        If Not IsDBNull(idr.Item("EVENT_DETAIL")) Then
                                            _EVENT_DETAIL = Convert.ToString(idr.Item("EVENT_DETAIL"))
                                        End If


                                        If Not IsDBNull(idr.Item("EVENT_TYPE")) Then
                                            _EVENT_TYPE = Convert.ToString(idr.Item("EVENT_TYPE"))
                                        End If

                                        If Not IsDBNull(idr.Item("CLIENT_FACILITY_CODE")) Then
                                            _CLIENT_FACILITY_CODE = Convert.ToString(idr.Item("CLIENT_FACILITY_CODE"))
                                        End If

                                        If Not IsDBNull(idr.Item("CREATE_DATE")) Then
                                            _CREATE_DATE = Convert.ToString(idr.Item("CREATE_DATE"))
                                        End If



                                        If Not IsDBNull(idr.Item("ADMITTING_DATE")) Then
                                            _ADMITTING_DATE = Convert.ToString(idr.Item("ADMITTING_DATE"))
                                        End If


                                        If Not IsDBNull(idr.Item("DISCHARGE_DATE")) Then
                                            _DISCHARGE_DATE = Convert.ToString(idr.Item("DISCHARGE_DATE"))
                                        End If

                                        'Try
                                        '    If Not IsDBNull(idr.Item("MED_RECORD_NUMBER")) Then
                                        '        _MED_RECORD_NUMBER = Convert.ToString(idr.Item("MED_RECORD_NUMBER"))
                                        '    End If
                                        'Catch ex As Exception

                                        '    _MED_RECORD_NUMBER = ""

                                        'End Try


                                        'Try
                                        '    If Not IsDBNull(idr.Item("RUN_RULES")) Then
                                        '        _RUN_RULES = Convert.ToString(idr.Item("RUN_RULES"))
                                        '    End If
                                        'Catch ex As Exception
                                        '    _RUN_RULES = ""
                                        'End Try


                                        'Try
                                        '    If Not IsDBNull(idr.Item("RUN_AVS_OUT")) Then
                                        '        _RUN_AVS_OUT = Convert.ToString(idr.Item("RUN_AVS_OUT"))
                                        '    End If
                                        'Catch ex As Exception
                                        '    _RUN_AVS_OUT = ""
                                        'End Try

                                        r = 0

                                    Catch sx As SqlException
                                        log.ExceptionDetails("MOD Fire rules paramter failed for PATIENT_AUDIT_TRAIL_ID" + Convert.ToString(i), sx)
                                        r = -2
                                        _sguid = "999"

                                    Catch ex As Exception
                                        log.ExceptionDetails("MOD Fire rules paramter failed for PATIENT_AUDIT_TRAIL_ID" + Convert.ToString(i), ex)
                                        r = -3
                                        _sguid = "999"
                                    End Try


                                    ' If (_RUN_RULES = "Y") Then


                                    Try

                                        ' iRowsToProcess  this count  ++



                                        If (r = 0) Then

                                            iRowsToProcess = iRowsToProcess + 1


                                            Using mfr As New modFireRulesAddrVB()

                                                '  mfr.UspGetAllDataHL7Rows = c.CfgUspGetAllDataHL7Rows   ' //SUBBA-20160713
                                                ' mfr.PATIENT_AUDIT_TRAIL_ID = CInt(_id)
                                                mfr.ConnectionString = _ConnectionString
                                                mfr.ContextID = c.cfgContextID
                                                mfr.HospCode = c.HospCode '= ConfigurationManager.AppSettings["cfgHospCode"]
                                                mfr.ErrorLogPath = c.errorLogPath ' = ConfigurationManager.AppSettings["errorLogPath"]
                                                mfr.DebugMode = c.debugMode  '// = ConfigurationManager.AppSettings["debugMode"]
                                                'mfr.RuleDebugMode = c.ruleDebugMode ' = ConfigurationManager.AppSettings["ruleDebugMode"]
                                                mfr.runMode = c.runMode ' = ConfigurationManager.AppSettings["runMode"]
                                                mfr.resultAll = c.resultAll '  **** = ConfigurationManager.AppSettings["resultAll"]
                                                mfr.raClientIP = c.raClientIP '= ConfigurationManager.AppSettings["raClientIP"]
                                                mfr.RuleSuccess = c.ruleSuccess ' = ConfigurationManager.AppSettings["ruleSuccess"]
                                                mfr.AddressRunMode = c.addressRunMode '= ConfigurationManager.AppSettings["addressRunMode"]
                                                mfr.addrSvcUrl1 = c.addrSvcUrl1 ' = ConfigurationManager.AppSettings["addrSvcUrl1"]
                                                mfr.addrSvcUrl2 = c.addrSvcUrl2 ' = ConfigurationManager.AppSettings["isParseVBorDB"]
                                                mfr.addrSvcPingLogMin = c.addrSvcPingLogMin ' = ConfigurationManager.AppSettings["addrSvcPingLogMin"]
                                                mfr.UspGetAllData = c.CfgUspGetAllData ' = ConfigurationManager.AppSettings["CfgUspGetAllData"]
                                                '   mfr.UspGetAllDataHL7Rows = c.CfgUspGetAllDataHL7Rows
                                                mfr.UspInsertSchedulerlog = c.CfgUspInsertSchedulerlog ' = ConfigurationManager.AppSettings["CfgUspInsertSchedulerlog"]
                                                mfr.UspGetRuleMsg = c.CfgUspGetRuleMsg '= ConfigurationManager.AppSettings["CfgUspGetRuleMsg"]
                                                mfr.UspRulesToFire = c.CfgUspRulesToFire    '// = ConfigurationManager.AppSettings["CfgUspRulesToFire"]
                                                mfr.UspApplyPatAuditTrial = c.CfgUspApplyPatAuditTrial ' = ConfigurationManager.AppSettings["CfgUspApplyPatAuditTrial"]
                                                mfr.UspTankAddress = c.CfgUspTankAddress
                                                mfr.UspAddCorrectAddress = c.CfgUspAddCorrectAddress ' = ConfigurationManager.AppSettings["CfgUspAddCorrectAddress"]
                                                mfr.UspAddMultipleAddress = c.CfgUspAddMultipleAddress
                                                mfr.UspFormatAddress = c.CfgUspFormatAddress
                                                mfr.UspAddressValidationTrail = c.CfgUspAddressValidationTrail
                                                mfr.UspGetAllTankById = c.CfgUspGetAllTankById
                                                mfr.UspRuleInsert = c.CfgUspRuleInsert
                                                mfr.UspRuleResultDelete = c.CfgUspRuleResultDelete
                                                mfr.UspRuleInsertByXmlTank = c.CfgUspRuleInsertByXmlTank '
                                                mfr.UspRuleInsertDebug = c.CfgUspRuleInsertDebug
                                                mfr.ruleDisplayLimit = c.ruleDisplayLimit ' = ConfigurationManager.AppSettings["ruleDisplayLimit"]
                                                mfr.CustomerID = c.customerID ' = ConfigurationManager.AppSettings["customerID"]
                                                mfr.LicenseKey = c.licenseKey '= ConfigurationManager.AppSettings["licenseKey"]
                                                mfr.AppName = _AppName
                                                mfr.RULE_MESSAGE_DELIMETER = _RULE_MESSAGE_DELIMETER
                                                mfr.TotalRulesToFire = _TotalRulesToFire

                                                mfr.PATIENT_AUDIT_TRAIL_ID = _PATIENT_AUDIT_TRAIL_ID
                                                mfr.PAT_HOSP_CODE = _PAT_HOSP_CODE
                                                mfr.PID = _PID
                                                mfr.PATIENT_ID = _PATIENT_ID
                                                mfr.EVENT_DETAIL = _EVENT_DETAIL
                                                '
                                                mfr.EVENT_TYPE = _EVENT_TYPE
                                                '
                                                mfr.CLIENT_FACILITY_CODE = _CLIENT_FACILITY_CODE
                                                mfr.CREATE_DATE = _CREATE_DATE
                                                '
                                                mfr.ADMITTING_DATE = _ADMITTING_DATE
                                                mfr.DISCHARGE_DATE = _DISCHARGE_DATE
                                                '

                                                mfr.MED_RECORD_NUMBER = _MED_RECORD_NUMBER
                                                mfr.RUN_AVS_OUT = _RUN_AVS_OUT


                                                mfr.AddressFilter = _AddressFilter
                                                mfr.GetMultipleAddress = _GetMultipleAddress

                                                mfr.dsRulesToFire = _dsRulesToFire
                                                mfr.iMaxRowsToProcess = _iMaxRowsToProcess
                                                mfr.JobID = _JobID


                                                mfr.Verbose = _Verbose
                                                mfr.LogFilePath = _LogFilePath

                                                '' need to put the code here to use new rules

                                                mfr.htRuleMsgs = _htRuleMsgs


                                                If (_useDLL) Then


                                                    If (_Verbose = 1) Then

                                                        _LastMSG = ("Going to MFR  " + Convert.ToString(number) + " for number " + Convert.ToString(number))

                                                        'log.ExceptionDetails(_AppName, _LastMSG)
                                                        '   Console.WriteLine(_LastMSG)



                                                    End If





                                                    mfr.Metrics = _Metrics

                                                    _Guid = Met.AddMetricPair("CODE", "go to run rule for PATIENT_AUDIT_TRAIL_ID: " + Convert.ToString(i))


                                                    mfr.Go(i) 'SUBBA-20160713

                                                    Met.AddMetric("CODE", "go to run rule for PATIENT_AUDIT_TRAIL_ID: " + Convert.ToString(i), _Guid)
                                                    _Guid = Guid.Empty



                                                    r = 0
                                                    If (_Verbose = 1) Then

                                                        _LastMSG = ("Returning from MFR   " + Convert.ToString(number) + " for number " + Convert.ToString(number))

                                                        'log.ExceptionDetails(_AppName, _LastMSG)
                                                        '    Console.WriteLine(_LastMSG)



                                                    End If
                                                Else
                                                    If (_Verbose = 1) Then

                                                        _LastMSG = ("Going to MFR  " + Convert.ToString(number) + " for number " + Convert.ToString(number))

                                                        'log.ExceptionDetails(_AppName, _LastMSG)
                                                        '   Console.WriteLine(_LastMSG)



                                                    End If





                                                    mfr.Metrics = _Metrics

                                                    _Guid = Met.AddMetricPair("CODE", "go to run rule for PATIENT_AUDIT_TRAIL_ID: " + Convert.ToString(i))


                                                    mfr.Go(i) 'SUBBA-20160713

                                                    Met.AddMetric("CODE", "go to run rule for PATIENT_AUDIT_TRAIL_ID: " + Convert.ToString(i), _Guid)
                                                    _Guid = Guid.Empty



                                                    r = 0
                                                    If (_Verbose = 1) Then

                                                        _LastMSG = ("Returning from MFR   " + Convert.ToString(number) + " for number " + Convert.ToString(number))

                                                        'log.ExceptionDetails(_AppName, _LastMSG)
                                                        '    Console.WriteLine(_LastMSG)



                                                    End If



                                                End If

                                                ' Console.WriteLine("1hhhhhh")
                                            End Using
                                        End If


                                    Catch sx As SqlException

                                        _sguid = "999"

                                        log.ExceptionDetails(_AppName + "Inline failed", sx)
                                        'Console.WriteLine("2.1")
                                        r = -4

                                    Catch ex As Exception


                                        _sguid = "999"

                                        '    Console.WriteLine("_PATIENT_AUDIT_TRAIL_ID  " + Convert.ToString(_PATIENT_AUDIT_TRAIL_ID))
                                        '   Console.WriteLine("_PAT_HOSP_CODE  " + _PAT_HOSP_CODE) ' = String.Empty
                                        '  Console.WriteLine("pid  " + _PID) ' = String.Empty
                                        ' Console.WriteLine("_PATIENT_ID" + _PATIENT_ID) ' = String.Empty
                                        ' Console.WriteLine("_EVENT_DETAIL  " + _EVENT_DETAIL) ' = String.Empty
                                        'Console.WriteLine("_EVENT_TYPE  " + _EVENT_TYPE) ' = String.Empty
                                        ' Console.WriteLine("_CLIENT_FACILITY_CODE  " + _CLIENT_FACILITY_CODE) '= String.Empty
                                        ' Console.WriteLine("_CREATE_DATE  " + _CREATE_DATE) ' = String.Empty
                                        ' Console.WriteLine("_ADMITTING_DATE " + _ADMITTING_DATE) ' = String.Empty
                                        ' Console.WriteLine("_DISCHARGE_DATE" + _DISCHARGE_DATE) ' = String.Empty
                                        ' Console.WriteLine("_MED_RECORD_NUMBER" + _MED_RECORD_NUMBER) ' = String.Empty
                                        ' Console.WriteLine("_RUN_AVS_OUT" + _RUN_AVS_OUT) ' = String.Empty

                                        If _Verbose = 1 Then
                                            '    System.Console.WriteLine("\n\rPress Any Key to Continue")
                                            '  System.Console.ReadKey()
                                        End If



                                        log.ExceptionDetails(_AppName + "Inline failed", ex)
                                        ' Console.WriteLine("2")
                                        r = -4
                                    End Try
                                    '   End If
                                Loop
                            End If
                        End Using

                    End Using
                End Using

                GC.Collect()

                'deincrimate the task count via ipc
                ' increment a task counter via IPC
            End If
            Dim ii As Integer = 0


            While (True)
                Dim rr As Integer = -1
                '  Console.WriteLine("3")
                rr = SendRowsToProcess("END|" + _sguid)
                '  Console.WriteLine("4")
                ' rr = SendRowsToProcess("END|" + Convert.ToString(number) + "^" + Convert.ToString(_JobID))
                If (rr = 0) Then Exit While

                If (ii = 5) Then
                    Exit While
                    log.ExceptionDetails(_AppName, "gave up after five tries to reach the pipe")
                    '     Console.WriteLine("5")
                End If

                ii += 1
            End While


            ' Console.WriteLine("6")



            Met.AddMetric("end inline.go for " + Convert.ToString(number))

            ' Console.WriteLine("7")
            If _Metrics >= 1 Then
                Met.FlushMetricStringToED()
            End If

            ' Console.WriteLine("8")

            Return r


        End Function

        Private Function SendRowsToProcess(ByVal MSG As String) As Integer


            Dim s As Integer = 0
            Try

                Dim pipeClient As New NamedPipeClientStream(".", _AppName, PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.Impersonation)

                pipeClient.Connect()

                Dim ss As New StreamString(pipeClient)

                ss.WriteString(MSG)
                pipeClient.Close()

                ss = Nothing

            Catch ex As Exception
                s = -1
                '  log.ExceptionDetails(_AppName, ex)
            End Try

            Return s


        End Function



        Public WriteOnly Property htRuleMsgs As Hashtable

            Set(value As Hashtable)
                _htRuleMsgs = value
            End Set

        End Property




        Public WriteOnly Property DLL As String

            Set(value As String)
                _dll = value
            End Set

        End Property


        Public WriteOnly Property Metrics As Integer

            Set(value As Integer)
                _Metrics = value
            End Set

        End Property

        Public WriteOnly Property TotalRulesToFire As Integer
            Set(value As Integer)
                _TotalRulesToFire = value
            End Set
        End Property




        Public WriteOnly Property Monitoring As Integer

            Set(value As Integer)
                _Monitoring = value
            End Set

        End Property



        Public WriteOnly Property MetricString As String

            Set(value As String)
                _MetricString = value
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

        Public WriteOnly Property BatchID As Integer

            Set(value As Integer)
                _BatchID = value
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



        Public WriteOnly Property ConsoleName As String

            Set(value As String)
                c.ConsoleName = value
            End Set

        End Property

        Public WriteOnly Property getAllDataSp As String

            Set(value As String)
                c.getAllDataSp = value
            End Set

        End Property

        Public WriteOnly Property Connectionas As String

            Set(value As String)
                c.Connectionas = value
            End Set

        End Property

        Public WriteOnly Property CommandTimeOut As String

            Set(value As String)
                c.CommandTimeOut = value
            End Set

        End Property

        Public WriteOnly Property SYNC_TIMEOUT As String

            Set(value As String)
                c.SYNC_TIMEOUT = value
            End Set

        End Property



        Public WriteOnly Property SUBMISSION_TIMEOUT As String

            Set(value As String)
                c.SUBMISSION_TIMEOUT = value
            End Set

        End Property


        Public WriteOnly Property ErrorLog As String

            Set(value As String)
                c.ErrorLog = value
            End Set

        End Property



        Public WriteOnly Property isParseVBorDB As String

            Set(value As String)
                c.isParseVBorDB = value
            End Set

        End Property


        Public WriteOnly Property isEmdeonLookUp As String

            Set(value As String)
                c.isEmdeonLookUp = value
            End Set

        End Property



        Public WriteOnly Property reRunEligAttempts As String

            Set(value As String)
                c.reRunEligAttempts = value
            End Set

        End Property

        Public WriteOnly Property uspEdiRequest As String

            Set(value As String)
                c.uspEdiRequest = value
            End Set

        End Property

        Public WriteOnly Property uspEdiDbImport As String

            Set(value As String)
                c.uspEdiDbImport = value
            End Set

        End Property


        Public WriteOnly Property LogPath As String

            Set(value As String)
                c.LogPath = value
            End Set

        End Property


        Public WriteOnly Property ClientSettingsProviderServiceUri As String

            Set(value As String)
                c.ClientSettingsProviderServiceUri = value
            End Set

        End Property


        Public WriteOnly Property HospCode As String

            Set(value As String)
                c.HospCode = value
            End Set

        End Property

        Public WriteOnly Property USP_GET_PAT_AUDIT_TRAIL_HL7_ROWS As String

            Set(value As String)
                c.USP_GET_PAT_AUDIT_TRAIL_HL7_ROWS = value
            End Set

        End Property


        Public WriteOnly Property USP_GET_PAT_AUDIT_TRAIL_HL7_STARTER_IDS As String

            Set(value As String)
                c.USP_GET_PAT_AUDIT_TRAIL_HL7_STARTER_IDS = value
            End Set

        End Property



        Public WriteOnly Property cfgContextID As String

            Set(value As String)
                c.cfgContextID = value
            End Set

        End Property



        Public WriteOnly Property cfgHospCode As String

            Set(value As String)
                c.cfgHospCode = value
            End Set

        End Property


        Public WriteOnly Property errorLogPath As String

            Set(value As String)
                c.errorLogPath = value
            End Set

        End Property


        Public WriteOnly Property debugMode As String

            Set(value As String)
                c.debugMode = value
            End Set

        End Property

        Public WriteOnly Property runMode As String

            Set(value As String)
                c.runMode = value
            End Set

        End Property



        Public WriteOnly Property resultAll As String

            Set(value As String)
                c.resultAll = value
            End Set

        End Property

        Public WriteOnly Property ruleSuccess As String

            Set(value As String)
                c.ruleSuccess = value
            End Set

        End Property

        Public WriteOnly Property raClientIP As String

            Set(value As String)
                c.raClientIP = value
            End Set

        End Property

        Public WriteOnly Property ruleDisplayLimit As String

            Set(value As String)
                c.ruleDisplayLimit = value
            End Set

        End Property



        Public WriteOnly Property CfgUspGetAllData As String

            Set(value As String)
                c.CfgUspGetAllData = value
            End Set

        End Property


        Public WriteOnly Property CfgUspGetAllDataHL7Rows As String

            Set(value As String)
                c.CfgUspGetAllDataHL7Rows = value
            End Set

        End Property


        Public WriteOnly Property CfgUspInsertSchedulerlog As String

            Set(value As String)
                c.CfgUspInsertSchedulerlog = value
            End Set

        End Property


        Public WriteOnly Property CfgUspGetRuleMsg As String

            Set(value As String)
                c.CfgUspGetRuleMsg = value
            End Set

        End Property


        Public WriteOnly Property CfgUspRulesToFire As String

            Set(value As String)
                c.CfgUspRulesToFire = value
            End Set

        End Property


        Public WriteOnly Property CfgUspApplyPatAuditTrial As String

            Set(value As String)
                c.CfgUspApplyPatAuditTrial = value
            End Set

        End Property

        Public WriteOnly Property CfgUspTankAddress As String

            Set(value As String)
                c.CfgUspTankAddress = value
            End Set

        End Property



        Public WriteOnly Property CfgUspAddCorrectAddress As String

            Set(value As String)
                c.CfgUspAddCorrectAddress = value
            End Set

        End Property


        Public WriteOnly Property CfgUspAddMultipleAddress As String

            Set(value As String)
                c.CfgUspAddMultipleAddress = value
            End Set

        End Property


        Public WriteOnly Property CfgUspFormatAddress As String

            Set(value As String)
                c.CfgUspFormatAddress = value
            End Set

        End Property


        Public WriteOnly Property CfgUspAddressValidationTrail As String

            Set(value As String)
                c.CfgUspAddressValidationTrail = value
            End Set

        End Property

        Public WriteOnly Property CfgUspGetAllTankById As String

            Set(value As String)
                c.CfgUspGetAllTankById = value
            End Set

        End Property


        Public WriteOnly Property CfgUspRuleInsert As String

            Set(value As String)
                c.CfgUspRuleInsert = value
            End Set

        End Property

        Public WriteOnly Property CfgUspRuleResultDelete As String

            Set(value As String)
                c.CfgUspRuleResultDelete = value
            End Set

        End Property

        Public WriteOnly Property CfgUspRuleInsertByXmlTank As String

            Set(value As String)
                c.CfgUspRuleInsertByXmlTank = value
            End Set

        End Property


        Public WriteOnly Property CfgUspRuleInsertDebug As String

            Set(value As String)
                c.CfgUspRuleInsertDebug = value
            End Set

        End Property


        Public WriteOnly Property addrSvcUrl1 As String

            Set(value As String)
                c.addrSvcUrl1 = value
            End Set

        End Property


        Public WriteOnly Property addrSvcUrl2 As String

            Set(value As String)
                c.addrSvcUrl2 = value
            End Set

        End Property


        Public WriteOnly Property AddressRunMode As String

            Set(value As String)
                c.addressRunMode = value
            End Set

        End Property



        Public WriteOnly Property addrSvcPingLogMin As String

            Set(value As String)
                c.addrSvcPingLogMin = value
            End Set

        End Property




        Public WriteOnly Property customerID As String

            Set(value As String)
                c.customerID = value
            End Set

        End Property


        Public WriteOnly Property licenseKey As String

            Set(value As String)
                c.licenseKey = value
            End Set

        End Property


        Public WriteOnly Property ruleDebugMode As String

            Set(value As String)
                c.ruleDebugMode = value
            End Set

        End Property




        Public WriteOnly Property DeadLockRetrys As Integer

            Set(value As Integer)
                c.DeadLockRetrys = value
            End Set

        End Property



        Public WriteOnly Property Verbose As Integer

            Set(value As Integer)
                _Verbose = value
            End Set
        End Property


        Public WriteOnly Property tguid As Guid
            Set(value As Guid)
                _tguid = value
            End Set
        End Property

        Public WriteOnly Property sguid As String
            Set(value As String)
                _sguid = value
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

    End Class

End Namespace

