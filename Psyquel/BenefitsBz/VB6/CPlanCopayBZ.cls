VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 2  'RequiresTransaction
END
Attribute VB_Name = "CPlanCopayBZ"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True

'--------------------------------------------------------------------
'Class Name: CPlanCopayBZ
'Date: 10/31/2000
'Author: Morgan Travis
'Description:  MTS object designed to host methods associated with
'               data affecting Plan Copay table.
'--------------------------------------------------------------------
Option Explicit

Private Const CLASS_NAME As String = "CPlanCopayBZ"

Public Function Insert(ByVal lngPlanID As Long, _
                       ByVal strCPTCode As String, ByVal lngProviderCredential As Long, _
                       ByVal strDSMIV As String, ByVal lngVisitStart As Long, ByVal lngVisitEnd As Long, _
                       ByVal curRNCDollar As Currency, ByVal dblRNCPercent As Double, ByVal dblCopayPercent As Double, _
                       ByVal curCopay As Currency)
'--------------------------------------------------------------------
'Date: 10/31/2000
'Author: Morgan Travis
'Description:  Inserts a row into the tblPlanCopay table utilizing
'              a stored procedure.
'Parameters: lngPlanID -  Plan ID number to insert into table
'            strCPTCode - The ten digit CPT Code that will be inserted into the table.
'            lngProviderCredential - Numerical representation of providers credentials.
'            strDSMIV - DSM-IV Code for treatment
'            lngVisitStart - Starting visit for plan info
'            lngVisitEnd - Ending visit for plan info
'            curRNCDollar - Amount paid to out of plan providers
'            dblRNCPercent - Percentage paid to out of plan providers
'            dblCopayPercent - Copay percent paid to in plan providers
'            curCopay - Copay amount paid to in plan providers
'--------------------------------------------------------------------

    Dim objPlanCopay As BenefitsDB.CPlanCopayDB
    Dim strErrMsg As String
    
    On Error GoTo ErrTrap:
    
    Set objPlanCopay = CreateObject("BenefitsDB.CPlanCopayDB")
    If VerifyData(lngPlanID, strCPTCode, lngProviderCredential, strDSMIV, lngVisitStart, lngVisitEnd, curRNCDollar, dblRNCPercent, dblCopayPercent, curCopay, strErrMsg, 0) Then
        Call objPlanCopay.Insert(lngPlanID, strCPTCode, lngProviderCredential, strDSMIV, lngVisitStart, lngVisitEnd, curRNCDollar, dblRNCPercent, dblCopayPercent, curCopay)
    Else
        GoTo ErrTrap
    End If
    Set objPlanCopay = Nothing
    
    GetObjectContext.SetComplete
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    On Error GoTo 0
    GetObjectContext.SetAbort
    Set objPlanCopay = Nothing
    
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMsg
    Else
        Err.Raise Err.Number, CLASS_NAME, Err.Description
    End If
End Function


Public Function Update(ByVal lngPlanID As Long, _
                       ByVal strCPTCode As String, ByVal lngProviderCredential As Long, _
                       ByVal strDSMIV As String, ByVal lngVisitStart As Long, ByVal lngVisitEnd As Long, _
                       ByVal curRNCDollar As Currency, ByVal dblRNCPercent As Double, ByVal dblCopayPercent As Double, _
                       ByVal curCopay As Currency, ByVal lngPlanCopayID As Long)
'--------------------------------------------------------------------
'Date: 10/31/2000
'Author: Morgan Travis
'Description:  Updates a row into the tblPlanCopay table utilizing
'              a stored procedure.
'Parameters: lngPlanID -  Plan ID number to insert into table
'            strCPTCode - The ten digit CPT Code that will be inserted into the table.
'            lngProviderCredential - Numerical representation of providers credentials.
'            strDSMIV - DSM-IV Code for treatment
'            lngVisitStart - Starting visit for plan info
'            lngVisitEnd - Ending visit for plan info
'            curRNCDollar - Amount paid to out of plan providers
'            dblRNCPercent - Percentage paid to out of plan providers
'            dblCopayPercent - Copay percent paid to in plan providers
'            curCopay - Copay amount paid to in plan providers
'
'--------------------------------------------------------------------
    
    Dim objPlanCopay As BenefitsDB.CPlanCopayDB
    Dim strErrMsg As String
    
    On Error GoTo ErrTrap:
    
    Set objPlanCopay = CreateObject("BenefitsDB.CPlanCopayDB")
    If VerifyData(lngPlanID, strCPTCode, lngProviderCredential, strDSMIV, lngVisitStart, lngVisitEnd, curRNCDollar, dblRNCPercent, dblCopayPercent, curCopay, strErrMsg, lngPlanCopayID) Then
        Call objPlanCopay.Update(lngPlanCopayID, strDSMIV, lngVisitStart, lngVisitEnd, curRNCDollar, dblRNCPercent, dblCopayPercent, curCopay)
    Else
        GoTo ErrTrap
    End If
    Set objPlanCopay = Nothing
    
    GetObjectContext.SetComplete
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    On Error GoTo 0
    GetObjectContext.SetAbort
    Set objPlanCopay = Nothing
    
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMsg
    Else
        Err.Raise Err.Number, CLASS_NAME, Err.Description
    End If
    

End Function


Public Function Exists(ByVal lngPlanID As Long, _
                       ByVal strCPTCode As String, ByVal lngProviderCredential As Long) As Boolean
'--------------------------------------------------------------------
'Date: 10/31/2000
'Author: Morgan Travis
'Description:  Checks for existing entries into tblPlanCopay
'Parameters: lngPlanID - Plan ID number to check for existing entries in table
'            strCPTCode - The ten digit CPT Code that will be inserted into the table.
'            lngProviderCredential - The integer associated with Providers credentials.
'
'--------------------------------------------------------------------
    Dim objPlanCopay As BenefitsDB.CPlanCopayDB
    Dim blnExists As Boolean
    
    On Error GoTo ErrTrap:
    
    Set objPlanCopay = CreateObject("BenefitsDB.CPlanCopayDB")
    blnExists = objPlanCopay.Exists(lngPlanID, strCPTCode, lngProviderCredential)
    Set objPlanCopay = Nothing
    Exists = blnExists
    GetObjectContext.SetComplete
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    On Error GoTo 0
    GetObjectContext.SetAbort
    Set objPlanCopay = Nothing
        
    Err.Raise Err.Number, CLASS_NAME, Err.Description
    

End Function

Public Function FetchByPlan(ByVal lngProviderID As Long, ByVal strCPTCode As String, _
                                 ByVal lngProviderCredential As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 10/31/2000                                                   '
'Author: Morgan Travis                                              '
'Description:  Retrieves records from the tblProviderFee table.     '
'Parameters: lngPlanID -  Plan ID number to insert into table
'            strCPTCode - The ten digit CPT Code that will be inserted into the table.
'            lngProviderCredential - Numerical representation of providers credentials.
'
'Returns: Recordset of requested providers
'-------------------------------------------------------------------'

    Dim objPlanCopay As BenefitsDB.CPlanCopayDB
    Dim rstSQL As ADODB.Recordset
    
    On Error GoTo ErrTrap:
    
    Set objPlanCopay = CreateObject("BenefitsDB.CPlanCopayDB")
    Set rstSQL = objPlanCopay.FetchByPlan(lngProviderID, strCPTCode, lngProviderCredential)
      
    Set FetchByPlan = rstSQL
    Set objPlanCopay = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    On Error GoTo 0
    GetObjectContext.SetAbort
    Set objPlanCopay = Nothing
    Set rstSQL = Nothing
    Err.Raise Err.Number, CLASS_NAME, Err.Description
    

End Function



Private Function VerifyData(ByVal lngPlanID As Long, _
                            ByVal strCPTCode As String, ByVal lngProviderCredential As Long, _
                            ByVal strDSMIV As String, ByVal lngVisitStart As Long, ByVal lngVisitEnd As Long, _
                            ByVal curRNCDollar As Currency, ByVal dblRNCPercent As Double, ByVal dblCopayPercent As Double, _
                            ByVal curCopay As Currency, ByRef strErrMsg As String, ByVal lngPlanCopayID As Long) As Boolean

    Dim objCPT As ListBz.CCPTCodeBz
        
    VerifyData = False
    If lngPlanCopayID = 0 Then
    
        If lngPlanID < 1 Then
            strErrMsg = "Plan RP Plan ID Not Valid"
            Exit Function
        End If
    
        If lngProviderCredential < 1 Then
            strErrMsg = "Provider Credential ID Not Valid"
            Exit Function
        End If
    
        If lngVisitStart < 0 Then
            strErrMsg = "Start Appointment Not Valid"
            Exit Function
        End If
    
        If lngVisitEnd < 0 Then
            strErrMsg = "Start Appointment Not Valid"
            Exit Function
        End If
    
        If curRNCDollar < 0 Then
            strErrMsg = "RNC Fee Not Valid"
            Exit Function
        End If
    
        If curCopay < 0 Then
            strErrMsg = "Copay Fee Not Valid"
            Exit Function
        End If
    
        If dblRNCPercent < 0 Or dblRNCPercent > 1 Then
            strErrMsg = "RNC Percent Not Valid"
            Exit Function
        End If
    
        If dblCopayPercent < 0 Or dblCopayPercent > 1 Then
            strErrMsg = "Copay Percent Not Valid"
            Exit Function
        End If
    
        Set objCPT = CreateObject("ListBz.CCPTCodeBz")
    
    
        If Not objCPT.Exists(strCPTCode) Then
            strErrMsg = "Invalid CPT Code"
            Exit Function
        End If
    
        If Exists(lngPlanID, strCPTCode, lngProviderCredential) Then
            strErrMsg = "Provider/CPT Code combination already exists"
            Exit Function
        End If
        VerifyData = True
    Else
        If lngVisitStart < 0 Then
            strErrMsg = "Start Appointment Not Valid"
            Exit Function
        End If
    
        If lngVisitEnd < 0 Then
            strErrMsg = "Start Appointment Not Valid"
            Exit Function
        End If
    
        If curRNCDollar < 0 Then
            strErrMsg = "RNC Fee Not Valid"
            Exit Function
        End If
    
        If curCopay < 0 Then
            strErrMsg = "Copay Fee Not Valid"
            Exit Function
        End If
    
        If dblRNCPercent < 0 Or dblRNCPercent > 1 Then
            strErrMsg = "RNC Percent Not Valid"
            Exit Function
        End If
    
        If dblCopayPercent < 0 Or dblCopayPercent > 1 Then
            strErrMsg = "Copay Percent Not Valid"
            Exit Function
        End If
        VerifyData = True
    End If

End Function

