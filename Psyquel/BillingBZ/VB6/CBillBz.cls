VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 2  'RequiresTransaction
END
Attribute VB_Name = "CBillBz"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CBillBz
'Date: 08/31/2000
'Author: Eric Pena
'Description:  MTS business object designed to call methods associated
'              with the CCPCDB class.
'--------------------------------------------------------------------
Option Explicit
Private Const CLASS_NAME As String = "CBillBz"
Private Const TABLE_NAME As String = "tblEncounterLog"
Private Const CONST_BILLSTAT_CANCELLED As Long = 6
Private Const CONST_BILLSTAT_REBILLED As Long = 7
Private Const CONST_TXTYPE_BILLERR As Long = 18
Private Const POST_TOCR  As Long = 8
Private Const POST_FRCR  As Long = 9
Private Const POST_ADJ  As Long = 23
Private Const BILLSTAT_BILL As Integer = 2
Public Sub Insert(ByVal lngEncounterLogID As Long, ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 08/31/2000
'Author: Eric Pena
'Description:  Modifies a row in the tblEncounterLog to "create" a bill utilizing a stored procedure.
'Parameters: lngEncounterLogID - The ID of the record in tblencounterlog that will be billed
'Returns: ID (Primary Key) of the row modified.
'--------------------------------------------------------------------
    Dim objBill As BillingDB.CBillDB
    Dim lngBillID As Long
    Dim strErrMessage As String
    
    On Error GoTo ErrTrap:
    
    If Not VerifyData(0, lngEncounterLogID, -1, 0, strErrMessage) Then GoTo ErrTrap
    
    Set objBill = CreateObject("BillingDB.CBillDB")
    
    objBill.Insert lngEncounterLogID, strUserName
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objBill = Nothing
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    Set objBill = Nothing
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Sub
Public Sub Update(ByVal lngID As Long, ByVal lngPatientID As Long, ByVal dteActionDate As Date, ByVal strPatientNotes As String, ByVal strBillNotes As String, ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 08/31/2000
'Author: Eric Pena
'Description:  Updates a row in the tblEncounterLog table utilizing a stored procedure.
'Parameters: Each parameter identifies the column value that will be inserted.
'Returns: Null
'--------------------------------------------------------------------
    Dim objBill As BillingDB.CBillDB
    Dim strErrMessage As String

    On Error GoTo ErrTrap:

    Set objBill = CreateObject("BillingDB.CBillDB")
    objBill.Update lngID, lngPatientID, dteActionDate, strPatientNotes, strBillNotes, strUserName
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objBill = Nothing
    Exit Sub
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    Set objBill = Nothing
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Sub
Private Function VerifyData(ByVal lngID As Long, ByVal lngEncounterLogID As Long, ByVal lngBillStatusID As Long, ByVal dteActionDate As Date, ByRef strErrMessage As String) As Boolean
'--------------------------------------------------------------------
'Date: 08/31/2000
'Author: Eric Pena
'Description:  Verifies all required data has been provided by the user.
'Parameters:  The values to be checked.
'Returns: Boolean value identifying if all criteria has been satisfied.
'--------------------------------------------------------------------
    If lngID = 0 Then
        'a new encounter
        If lngEncounterLogID < 0 Then
            strErrMessage = "A valid Encounter is needed to Bill."
            VerifyData = False
            Exit Function
        End If
    Else
        If lngBillStatusID < 0 Then
            strErrMessage = "A valid Bill Status is required."
            VerifyData = False
            Exit Function
        End If
        If Not IsDate(dteActionDate) Then
            strErrMessage = "A valid Action Date is required."
            VerifyData = False
            Exit Function
        End If
    End If
    'Check for existance only when inserting new data
    If lngID = 0 Then
        If Exists(lngEncounterLogID) Then
            strErrMessage = "This encounter has already been billed."
            VerifyData = False
            Exit Function
        End If
    End If
    
    'If we get here, all is well...
    VerifyData = True

End Function
Public Function Exists(ByVal lngEncounterLogID As Long) As Boolean
'--------------------------------------------------------------------
'Date: 08/31/2000
'Author: Eric Pena
'Description:  Determines if the Encounter specified has already been billed
'Parameters: lngEncounterLogID - The ID in tblEncounterLog to check
'Returns: True if the name exists, false otherwise
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------
    Dim objBill As BillingDB.CBillDB
    Dim blnExists As Boolean

    On Error GoTo ErrTrap:
    
    Set objBill = CreateObject("BillingDB.CBillDB")
    blnExists = objBill.Exists(lngEncounterLogID)

    Exists = blnExists

    Set objBill = Nothing

    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Function

ErrTrap:
   'Signal incompletion and raise the error to the calling environment.
    Set objBill = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
 Public Function Fetch() As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 08/31/2000
'Author: Eric Pena
'Description:  Returns rows from tblEncounterLog utilizing a stored procedure
'Parameters: null
'Returns: ADORst
'--------------------------------------------------------------------
    Dim objBill As BillingDB.CBillDB
     
     On Error GoTo ErrTrap:
     
     Set objBill = CreateObject("BillingDB.CBillDB")
     Set Fetch = objBill.Fetch()
    
    'Signal successful completion
     GetObjectContext.SetComplete
     Set objBill = Nothing
     Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objBill = Nothing
     GetObjectContext.SetAbort
     Err.Raise Err.Number, Err.Source, Err.Description
 End Function

Public Function FetchOutstandingClaims(ByVal lngBillID As Long, ByVal lngStatusID As Long, ByVal blnEClaim As Boolean, _
        ByVal lngPatientID As Long, ByVal strPatientFirst As String, ByVal strPatientLast As String, _
        ByVal strPatientSSN As String, ByVal lngProviderID As Long, ByVal strProviderFirst As String, _
        ByVal strProviderLast As String, ByVal strPlanName As String, ByVal strPlanCity As String, _
        ByVal strPlanState As String, ByVal dteDOS1 As Date, ByVal dteDOS2 As Date, ByVal dtePost1 As Date, _
        ByVal dtePost2 As Date, ByVal dteAction1 As Date, ByVal dteAction2 As Date, ByVal curBal1 As Currency, _
        ByVal curBal2 As Currency) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 10/03/2000
'Author: Eric Pena
'Description:  Returns outstanding claims based on any combination of the specified parameters
'Parameters: all parameters are criteria to use in searching for claims
'Returns: Recordset of claims matching the criteria
'--------------------------------------------------------------------
    Dim objBill As BillingDB.CBillDB
     
    On Error GoTo ErrTrap:
    
    Set objBill = CreateObject("BillingDB.CBillDB")
    Set FetchOutstandingClaims = objBill.FetchOutstandingClaims(lngBillID, lngStatusID, blnEClaim, lngPatientID, _
               strPatientFirst, strPatientLast, strPatientSSN, lngProviderID, strProviderFirst, strProviderLast, _
               strPlanName, strPlanCity, strPlanState, dteDOS1, dteDOS2, dtePost1, dtePost2, dteAction1, _
               dteAction2, curBal1, curBal2)
    Set objBill = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Function

ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objBill = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function


Public Function FetchByID(ByVal lngBillID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 08/31/2000
'Author: Eric Pena
'Description:  Returns rows from tblEncounterLog utilizing a stored procedure
'Parameters: null
'Returns: ADORst
'--------------------------------------------------------------------
    Dim objBill As BillingDB.CBillDB
     
     On Error GoTo ErrTrap:
     
     Set objBill = CreateObject("BillingDB.CBillDB")
     Set FetchByID = objBill.FetchByID(lngBillID)
    
    'Signal successful completion
     GetObjectContext.SetComplete
     Set objBill = Nothing
     Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objBill = Nothing
     GetObjectContext.SetAbort
     Err.Raise Err.Number, Err.Source, Err.Description
 End Function
Public Function FetchPatOpeningBalByID(ByVal lngID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 09/29/2000
'Author: Eric Pena
'Description:  Returns a row from the tblPatientPaymentLog table utilizing a stored procedure
'Parameters: lngID - id of the row to return records for
'Returns: ADORst
'--------------------------------------------------------------------
     Dim objPPL As BillingDB.CBillDB
     
     On Error GoTo ErrTrap:
     
     Set objPPL = CreateObject("BillingDB.CBillDB")
     Set FetchPatOpeningBalByID = objPPL.FetchPatOpeningBalByID(lngID)
    
    'Signal successful completion
     GetObjectContext.SetComplete
     Set objPPL = Nothing
     Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objPPL = Nothing
     GetObjectContext.SetAbort
     Err.Raise Err.Number, Err.Source, Err.Description
 End Function
Public Sub VerifyIns(ByVal lngPatientID As Long, ByVal lngPlanID As Long, ByVal strCardNum As String, ByVal strGroupNum As String, _
                        ByVal lngPriCopayID As Long, ByVal lngSecCopayID As Long, ByVal lngTerCopayID As Long, _
                        ByVal dblPriCopayAmt As Currency, ByVal dblSecCopayAmt As Currency, ByVal dblTerCopayAmt As Currency, _
                        ByVal strCertNum As String, ByVal dteStart As Date, ByVal dteEnd As Date, _
                        varProvider As Variant, varCPT As Variant, ByVal blnCertReq As Boolean, ByVal strNotes As String, _
                        ByVal dblDeduct As Currency, ByVal dblDeductRem As Currency, ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 03/28/2002
'Author: Eric Pena
'Description:  Modifies a row in the tblEncounterLog to "create" a bill utilizing a stored procedure.
'Parameters: lngEncounterLogID - The ID of the record in tblencounterlog that will be billed
'Returns: ID (Primary Key) of the row modified.
'--------------------------------------------------------------------
    Dim objPatRPPlan As BenefactorBz.CPatRPPlanBz
    Dim objBenefit As BenefitsBz.CPatientBenefitBZ
    Dim objCert As CertificationBz.CCertificationBz
    Dim rst As ADODB.Recordset
    Dim lngID As Long
    Dim strCertReq As String

    If blnCertReq Then strCertReq = "Y" Else strCertReq = "N"

    On Error GoTo ErrTrap:

    'add ins
    Set objPatRPPlan = CreateObject("BenefactorBz.CPatRPPlanBz")
    Set rst = objPatRPPlan.FetchRPPlansByPat(lngPatientID)
    lngID = objPatRPPlan.Append(lngPatientID, rst.Fields("fldRPID").Value, lngPlanID, strUserName)

    'delete old ins
    objPatRPPlan.Deleted True, lngPatientID, rst.Fields("fldRPID").Value, rst.Fields("fldPlanID").Value, strUserName
    Set objPatRPPlan = Nothing

    'set bfist info
    Set objBenefit = CreateObject("BenefitsBz.CPatientBenefitBZ")
    objBenefit.Insert lngID, strCardNum, strGroupNum, strCertReq, _
            lngPriCopayID, lngSecCopayID, lngTerCopayID, dblPriCopayAmt, dblSecCopayAmt, dblTerCopayAmt, strNotes, "", _
            0, IfNull(rst.Fields("fldPlanName").Value, ""), "", "", "", "", "", IfNull(rst.Fields("fldfullname").Value, ""), "", "N", "N", "N", dblDeduct, dblDeductRem, "", strUserName
    Set objBenefit = Nothing
    Set rst = Nothing

    'create cert
    If Trim(strCertNum) <> "" Then
        Set objCert = CreateObject("CertificationBz.CCertificationBz")
        objCert.Insert lngPatientID, lngPlanID, varProvider(0), dteStart, dteEnd, strCertNum, strUserName, "", varProvider, varCPT, 14, 2
        Set objCert = Nothing
    End If

    'Signal successful completion
    GetObjectContext.SetComplete
    Exit Sub

ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    Set objPatRPPlan = Nothing
    Set objBenefit = Nothing
    Set objCert = Nothing
    Set rst = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub




'''Public Sub AdjustPatientResp(ByVal lngEncounterLogID As Long, ByVal curRespAmouont As Currency, ByVal strUserName As String)
''''--------------------------------------------------------------------
''''Date: 01/28/2002
''''Author: Eric Pena
''''Description:  Modifies RP responsible amounts on a claim
''''Parameters: lngEncounterLogID - The ID of the record in tblencounterlog that will be adjusted
''''               curRespAmouont - total the RP is to pay for the EL
''''               strUserName - user making the adjustment
''''--------------------------------------------------------------------
'''    Dim objBill As BillingBz.CBRBz
'''    Dim curAmtCharged As Currency
'''    Dim curAmtPaid As Currency
'''    Dim strError As String
'''    Dim lngSource As Long
'''    Dim curBalance As Currency
'''    Dim rst As ADODB.Recordset
'''
'''    On Error GoTo ErrTrap:
'''
'''    curAmtPaid = 0
'''    curAmtCharged = 0
'''    curBalance = 0
'''    lngSource = -1
'''
'''    Set objBill = CreateObject("BillingBz.CBRBz")
'''    Set rst = objBill.Fetch(lngEncounterLogID)
'''    If Not rst.EOF Then
'''        curAmtCharged = rst!fldAmtExpected
'''        lngSource = rst!fldBRID
'''        curBalance = curAmtCharged - curRespAmouont
'''        rst.MoveNext
'''    End If
'''
'''
'''    While Not rst.EOF
'''        'calc how much ins has paid
'''        curAmtCharged = curAmtCharged + rst!fldAmtExpected
'''        curAmtPaid = curAmtPaid + rst!fldAmtCollected
'''        rst.MoveNext
'''    Wend
'''
'''    If curRespAmouont > curAmtCharged Then
'''        strError = "Cannot assign more than what was charged for this visit.  (" & Format(curAmtCharged, "Currency") & " )."
'''        GoTo ErrTrap
'''    End If
'''    If curRespAmouont > (curAmtCharged - curAmtPaid) Then
'''        strError = "Cannot assign that much to the patient.  Insurances have already accounted for all but " & Format((curAmtCharged - curAmtPaid), "Currency") & " of the balance."
'''        GoTo ErrTrap
'''    End If
'''
'''
'''    If Not rst.BOF Then rst.MoveFirst
'''    If Not rst.EOF Then rst.MoveNext
'''
'''
'''    While curBalance <> 0 And Not rst.EOF
'''        If curBalance > 0 Then
'''            'move from pat to next plan
'''            objBill.TransferResponsibility lngSource, rst!fldBRID, curBalance, True, strUserName
'''            curBalance = 0
'''        ElseIf rst!fldAmtOutstanding <> 0 Then
'''            'move from plan to pat
'''            If Abs(curBalance) > rst!fldAmtOutstanding Then
'''                'move partial
'''                objBill.TransferResponsibility rst!fldBRID, lngSource, rst!fldAmtOutstanding, True, strUserName
'''                curBalance = CCur(curBalance + rst!fldAmtOutstanding)
'''            Else
'''                'move all
'''                objBill.TransferResponsibility rst!fldBRID, lngSource, Abs(curBalance), True, strUserName
'''                curBalance = 0
'''            End If
'''        End If
'''        rst.MoveNext
'''    Wend
'''
'''
'''    'Signal successful completion
'''    GetObjectContext.SetComplete
'''    Set objBill = Nothing
'''    Set rst = Nothing
'''    Exit Sub
'''
'''ErrTrap:
'''    'Signal incompletion and raise the error to the calling environment.  The
'''    'condition handles custom business rule checks we may have established.
'''    GetObjectContext.SetAbort
'''    Set objBill = Nothing
'''    Set rst = Nothing
'''    If Err.Number = 0 Then
'''        Err.Raise vbObjectError, CLASS_NAME, strError
'''    Else
'''        Err.Raise Err.Number, Err.Source, Err.Description
'''    End If
'''End Sub


