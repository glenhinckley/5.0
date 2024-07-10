Option Strict On
Option Explicit On



'Imports System.Data
'Imports System.Data.SqlClient

'Imports System.IO

'Imports System.Text.RegularExpressions


'Imports System.Configuration
'Imports System.Collections
'Imports System.Text



'Imports System.Net




'Imports DCSGlobal.BusinessRules.Logging
'Imports DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff
'Imports DCSGlobal.BusinessRules.CoreLibraryII
'Imports DCSGlobal.RealAlert.DCSGlobal.RealAlert

Namespace FireRules




    Public Class Config





        Property htRuleMsgs As Hashtable ' = New Hashtable

        Property ConsoleName As String
        Property getAllDataSp As String
        Property Connectionas As String
        Property CommandTimeOut As String
        Property SYNC_TIMEOUT As String
        Property SUBMISSION_TIMEOUT As String
        Property ErrorLog As String
        Property isParseVBorDB As String
        Property isEmdeonLookUp As String
        Property reRunEligAttempts As String
        Property uspEdiRequest As String
        Property uspEdiDbImport As String
        Property LogPath As String
        Property ClientSettingsProviderServiceUri As String
        Public verbose As Integer
        Public dbPoll As Integer

        Public dbPollTimeMilliSeconeds As Integer


        Property HospCode As String



        Property USP_GET_PAT_AUDIT_TRAIL_HL7_ROWS As String

        Property USP_GET_PAT_AUDIT_TRAIL_HL7_STARTER_IDS As String

        Property cfgContextID As String
        Property cfgHospCode As String
        Property errorLogPath As String
        Property debugMode As String

        Property runMode As String

        Property resultAll As String
        Property ruleSuccess As String
        Property raClientIP As String
        Property ruleDisplayLimit As String




        Property CfgUspGetAllData As String
        Property CfgUspGetAllDataHL7Rows As String
        Property CfgUspInsertSchedulerlog As String
        Property CfgUspGetRuleMsg As String
        Property CfgUspRulesToFire As String
        Property CfgUspApplyPatAuditTrial As String
        Property CfgUspTankAddress As String

        Property CfgUspAddCorrectAddress As String
        Property CfgUspAddMultipleAddress As String
        Property CfgUspFormatAddress As String
        Property CfgUspAddressValidationTrail As String
        Property CfgUspGetAllTankById As String
        Property CfgUspRuleInsert As String
        Property CfgUspRuleResultDelete As String
        Property CfgUspRuleInsertByXmlTank As String
        Property CfgUspRuleInsertDebug As String



        Property addrSvcUrl1 As String
        Property addrSvcUrl2 As String
        Property addressRunMode As String
        Property addrSvcPingLogMin As String
        Property customerID As String
        Property licenseKey As String


        Property MinThreads As String
        Property MaxThreads As String
        Property MinWait As String
        Property MaxWait As String
        Property CleanupInterval As String
        Property SchedulingInterval As String



        Property MaxCount As String
        Property ThreadCount As String
        Property ThreadsON As String




        Property WaitForEnterToExit As String
        Property NoDataTimeOut As String
        Public DeadLockRetrys As Integer
        Property ruleDebugMode As String

    End Class
End Namespace
