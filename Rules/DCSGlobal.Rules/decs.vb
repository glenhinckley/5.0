Option Strict On
Option Explicit On











Imports DCSGlobal.BusinessRules.Logging





Namespace DCSGlobal.Rules




    Public Class decs

        Dim _SQL_CONNECTION_STRING As String = String.Empty
        Dim _scriptCtl As MSScriptControl.ScriptControl = New MSScriptControl.ScriptControl
        Dim _scriptVBValue As String = String.Empty
        Dim _strHospCode As String = String.Empty
        Dim _sErrLogPath As String = String.Empty
        Dim _sDebugMode As String = String.Empty
        Dim _sRuleDebugMode As String = String.Empty
        Dim _sRunMode As String = String.Empty
        Dim _sContents As String = String.Empty
        Dim _sRaProtocol As String = String.Empty
        Dim _sStartTime As String = String.Empty
        Dim _strRuleContextID As String = String.Empty


        Dim _iRowsToProcess As Integer = 0
        Dim _iTotalRulesToFire As Integer = 0
        Dim _iCmdTimeout As Integer = 180   'subba-080125-yymmdd
        Dim _colNameVal As System.Collections.Specialized.NameValueCollection
        Dim _htRuleMsgs As Hashtable = New Hashtable
        ' property WithEvents Srv As DCSErrorService.ServerClass
        '   property Const ENCRYPT_DECRYPT_KEY As String = "&%#@?,:*#$$($#$#(%%%(%($(#$#(@(##)@$$(*%"
        Dim _RULE_MESSAGE_DELIMETER As String = "," 'subba-020108
        Dim _RA_TIMEOUT_MINUTES As String = "1"
        Dim _sAddrUspsFormat As String = String.Empty    'subba-012109
        Dim _sAddrUspsFormatErr As String = String.Empty 'subba-012109
        Dim _sAddrSvcPingLogMin As String = String.Empty 'subba-022409
        Dim _iAddrSvcPingLogMin As Integer = 10

        'subba-062912-ucla-060508----------
        'Private LICENSE_KEY As String = "21"
        Dim _CUSTOMER_ID As String = String.Empty
        Dim _LICENSE_KEY As String = String.Empty
        Dim _iTemp As Integer = 0
        Dim _sTemp1 As String = String.Empty
        Dim _sTemp2 As String = String.Empty
        Dim _sTemp3 As String = String.Empty
        Dim _isAttemptsSuccess As Boolean = False

        Dim _sCfgUspGetAllData As String = String.Empty 'subba-022409
        Dim _sGetAllDataSp As String = String.Empty
        Dim _sCfgUspInsertSchedulerlog As String = String.Empty
        Dim _sCfgUspGetRuleMsg As String = String.Empty
        Dim _sCfgUspRulesToFire As String = String.Empty
        Dim _sCfgUspApplyPatAuditTrial As String = String.Empty
        Dim _sCfgUspTankAddress As String = String.Empty
        Dim _sCfgUspAddCorrectAddress As String = String.Empty
        Dim _sCfgUspAddMultipleAddress As String = String.Empty
        Dim _sCfgUspFormatAddress As String = String.Empty
        Dim _sCfgUspAddressValidationTrail As String = String.Empty
        Dim _sCfgUspGetAllTankById As String = String.Empty
        Dim _sCfgUspRuleInsert As String = String.Empty
        Dim _sCfgUspRuleResultDelete As String = String.Empty
        Dim _sCfgUspRuleInsertByXmlTank As String = String.Empty
        Dim _sCfgUspRuleInsertDebug As String = String.Empty
        Dim _sConsoleProgName As String = String.Empty

        Dim _sCfgAddrSvcUrl As String = "https://address.auditlogix.net/dcsaddress.asmx" 'subba-062312
        Dim _sCfgAddrSvcUrl1 As String = String.Empty 'subba-062312
        Dim _sCfgAddrSvcUrl2 As String = String.Empty 'subba-062312
        Dim _sCfgAddrSvcUrl3 As String = String.Empty 'subba-062312
        Dim _sCfgAddrSvcLoop As String = String.Empty 'subba-062312





        Dim _connStr As String = String.Empty
        Dim _connStrEncrypted As String = String.Empty
        Dim _sFileReadErr As String = String.Empty
        Dim _sLoggedIP As String = String.Empty
        Dim _sResultAll As String = String.Empty
        Dim _sRaClientIP As String = String.Empty
        Dim _sRaServerPort As String = String.Empty
        Dim _sRuleSuccess As String = String.Empty
        Dim _sRaVersion As String = String.Empty
        Dim _sGetInsertRaDebugFlag As String = String.Empty
        Dim _sGetRaMsgDelimeter As String = String.Empty
        Dim _useEncryptionConnStr As String = String.Empty
        Dim _bAvsRunFlag As String = String.Empty     'NOTUSED ___Dim sCfgApplyToTank As String = "usp_apply_patient_audit_trail"   'subba-080125-yymmdd

        Dim _isAesAddrSuccess As Boolean = False
        Dim _strVerifyAddr As String = String.Empty
        Dim _strPat_hosp_code As String = String.Empty
        Dim _sAddressType As String = String.Empty
        Dim _strPatient_street_address As String = String.Empty
        Dim _strPatient_address2 As String = String.Empty
        Dim _strPatient_city As String = String.Empty
        Dim _strPatient_state As String = String.Empty
        Dim _strPatient_zip_code As String
        Dim _sAddrXmlOut As String = String.Empty
        Dim _sAddrTypeFilter As String = String.Empty
        Dim _sAddrGetMultipleAddress As String = String.Empty



        Dim _strPatientAccount As String = String.Empty
        Dim _strOperatorId As String = String.Empty
        Dim _strRuleDef As String = String.Empty
        Dim _strRuleID As String = "0"
        Dim _strRuleName As String = String.Empty
        Dim _strPid As String = String.Empty

        Dim strContextDesc As String = "short summary"


        Dim _dtStart_ts As DateTime
        Dim _dtEnd_ts As DateTime




    End Class
End Namespace



