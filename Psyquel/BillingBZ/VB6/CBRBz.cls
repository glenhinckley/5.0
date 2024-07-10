VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 2  'RequiresTransaction
END
Attribute VB_Name = "CBRBz"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CBRBz
'Date: 08/31/2000
'Author: Eric Pena
'Description:  MTS business object designed to call methods associated  with the CBRDB class.
'--------------------------------------------------------------------
Option Explicit
Private Const CLASS_NAME As String = "CBRBz"
Private Const TABLE_NAME As String = "tblBillingResponsibility"

Public Function Insert(ByVal lngELID As Long, ByVal lngPatientID As Long, _
            ByVal lngRPID As Long, ByVal lngPlanID As Long, ByVal lngCPCID As Long, _
            ByVal lngInsuranceID As Long, ByVal strPayerCode As String, _
            ByVal intOrder As Integer, ByVal dblAmtResp As Double, _
            ByVal dblAmtAcct As Double, ByVal lngTaxID As Long, ByVal strTIN As String, _
            ByVal intTINType As Integer, ByVal strCertNum As String) As Long
'--------------------------------------------------------------------
'Date: 07/08/2002
'Author: Dave Richkun
'Description:  Inserts a row in the tblBillingResponsibility table utilizing a stored procedure.
'Parameters:
'
'   *** TO DO: Define each parameter once procedure is finalized ****
'
'Returns: ID of the row modified.
'--------------------------------------------------------------------
    
    Dim objBR As BillingDB.CBRDB
    
    On Error GoTo ErrTrap:
    
    Set objBR = CreateObject("BillingDB.CBRDB")
    Insert = objBR.Insert(lngELID, lngPatientID, lngRPID, lngPlanID, lngCPCID, _
            lngInsuranceID, strPayerCode, intOrder, dblAmtResp, dblAmtAcct, _
            lngTaxID, strTIN, intTINType, strCertNum)
    Set objBR = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set objBR = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

Public Sub Delete(ByVal lngBRID As Long)
'--------------------------------------------------------------------
'Date: 03/20/2009
'Author: Duane C Orth
'Description:  Physically deletes a row in tblBillingResponsibilty table utilizing
'              a stored procedure.
'Parameters: lngTxID - ID of transaction to be deleted
'Returns: Null
'--------------------------------------------------------------------
    
    Dim objBR As BillingDB.CBRDB
    
    On Error GoTo ErrTrap:
    
    Set objBR = CreateObject("BillingDB.CBRDB")
    Call objBR.Delete(lngBRID)
    Set objBR = Nothing
   
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set objBR = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description
    
End Sub

Public Sub Update(ByVal lngBRID As Long, ByVal lngRPID As Long, ByVal lngELID As Long, ByVal lngPlanID As Long, ByVal lngCPCID As Long, _
                  ByVal lngInsuranceID As Long, ByVal dblAmtResp As Double, _
                  ByVal dblAmtAcctd As Double, ByVal strTIN As String, ByVal intTINType As Integer, _
                  ByVal lngTaxID As Long, ByVal strCertNum As String)
'--------------------------------------------------------------------
'Date: 08/02/2002
'Author: Dave Richkun
'Description:  Updates a row in tblBillingResponsibility utilizing a stored procedure.
'Parameters:
'
'  ** TO DO ** - Define parameters once procedure finalized
'
'Returns: Null
'--------------------------------------------------------------------
    
    Dim objBR As BillingDB.CBRDB

    On Error GoTo ErrTrap:

    Set objBR = CreateObject("BillingDB.CBRDB")
    Call objBR.Update(lngBRID, lngRPID, lngELID, lngPlanID, lngCPCID, lngInsuranceID, dblAmtResp, dblAmtAcctd, strTIN, intTINType, lngTaxID, strCertNum)
    Set objBR = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Sub

ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set objBR = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub


Public Sub UpdatePatientAmount(ByVal lngELID As Long, ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 08/05/2002
'Author: Dave Richkun
'Description:  Updates the Amount Responsible and Amount Accountable columns
'              in tblBillingResponsibility for a given encounter.
'Parameters: lngELID - ID of the encounter log for which patient balances are
'              being updated.
'Returns: Null
'--------------------------------------------------------------------
    
    Dim objBR As BillingDB.CBRDB

    On Error GoTo ErrTrap:

    Set objBR = CreateObject("BillingDB.CBRDB")
    Call objBR.UpdatePatientAmount(lngELID, strUserName)
    Set objBR = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Sub

ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set objBR = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub


Public Sub Resubmit(ByVal lngOrigELID As Long, ByVal lngNewELID As Long)
'--------------------------------------------------------------------
'Date: 08/08/2002
'Author: Dave Richkun
'Description:  Reassigns the Encounter Log ID foreign key from the RP's billing
'              responsibility records to a new Encounter Log ID.  This
'              method is used when a claim is resubmitted and a new encounter
'              is created.  By reassigning the encounter log ID, the original
'              patient billing responsibility records are preserved when assigned
'              to the new encounter.
'Parameters:  lngOrigELID - ID of the original encounter - the ID of the encounter
'               being cancelled
'             lngNewELID - ID of the resubmitted encounter - the ID of the new
'               encounter that supercedes all previous encounters.
'Returns: Null
'--------------------------------------------------------------------

    Dim objBR As BillingDB.CBRDB

    On Error GoTo ErrTrap:

    Set objBR = CreateObject("BillingDB.CBRDB")
    Call objBR.Resubmit(lngOrigELID, lngNewELID)
    Set objBR = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Sub

ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set objBR = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub


Public Sub ZeroAll(ByVal lngELID As Long)
'--------------------------------------------------------------------
'Date: 08/05/2002
'Author: Dave Richkun
'Description:  Updates the Amount Responsible, Amount Accountable, and
'              Balance columns for all responsible parties in
'              tblBillingResponsibility for a given encounter.
'Parameters: lngELID - ID of the encounter log for which balances are
'              being updated.
'Returns: Null
'--------------------------------------------------------------------

    Dim objBR As BillingDB.CBRDB

    On Error GoTo ErrTrap:

    Set objBR = CreateObject("BillingDB.CBRDB")
    Call objBR.ZeroAll(lngELID)
    Set objBR = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Sub

ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set objBR = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub
Public Function Exists(ByVal lngELID As Long, ByVal lngRPID As Long, ByVal lngPlanID As Long, ByVal blnPayerIsPlan As Boolean) As Boolean
'--------------------------------------------------------------------
'Date: 08/31/2000
'Author: Eric Pena
'Description:  Determines if the BusinessName identical to the
'              strBusinessName parameter already exists in the table.
'Parameters: strBusinessName - Business name to be checked
'Returns: True if the name exists, false otherwise
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------
'
    Dim objBR As BillingDB.CBRDB
    Dim blnExists As Boolean

    On Error GoTo ErrTrap:

    
    Set objBR = CreateObject("BillingDB.CBRDB")
    blnExists = objBR.Exists(lngELID, lngRPID, lngPlanID, blnPayerIsPlan)

    Exists = blnExists

    Set objBR = Nothing

    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Function

ErrTrap:
   'Signal incompletion and raise the error to the calling environment.
    Set objBR = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
 Public Function Fetch(lngELID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 08/31/2000
'Author: Eric Pena
'Description:  Returns rows from the tblBillingResponsibility and tblInsCompany tables utilizing a stored procedure
'Parameters: blnIncludeDisabled - determines if disabled records will be returned
'Returns: ADORst
'--------------------------------------------------------------------
     Dim objBR As BillingDB.CBRDB
     
     On Error GoTo ErrTrap:
     
     Set objBR = CreateObject("BillingDB.CBRDB")
     Set Fetch = objBR.Fetch(lngELID)
    
    'Signal successful completion
     GetObjectContext.SetComplete
     Set objBR = Nothing
     Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objBR = Nothing
     GetObjectContext.SetAbort
     Err.Raise Err.Number, Err.Source, Err.Description
 End Function
Public Sub SwapOrder(ByVal lngID1 As Long, ByVal lngID2 As Long)
'--------------------------------------------------------------------
'Date: 02/24/2000
'Author: Eric Pena
'Description:  Swaps the fldOrder Field of two entries in tblBillingResponsibility
'Parameters: Each parameter identifies the column value that will be swapped.
'Returns: Null
'--------------------------------------------------------------------
     Dim objBR As BillingDB.CBRDB
     Dim strError As String
     
     On Error GoTo ErrTrap:
     strError = ""
     Set objBR = CreateObject("BillingDB.CBRDB")
     
     If Not SameBill(lngID1, lngID2) Then
        strError = "Both payers must be on the same bill"
        GoTo ErrTrap
     End If
     
     objBR.SwapOrder lngID1, lngID2
    
    'Signal successful completion
     GetObjectContext.SetComplete
     Set objBR = Nothing
     Exit Sub
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objBR = Nothing
     GetObjectContext.SetAbort
     If strError = "" Then
        Err.Raise Err.Number, Err.Source, Err.Description
    Else
        Err.Raise -1, CLASS_NAME, strError
    End If
 End Sub

'''Public Sub TransferResponsibility(ByVal lngBRIDSource As Long, ByVal lngBRIDDest As Long, ByVal curAmount As Currency, Optional ByVal blnAdjustBalance As Boolean = False, _
'''            Optional ByVal strUserName As String = "")
''''--------------------------------------------------------------------
''''Date: 02/24/2000
''''Author: Eric Pena
''''Description:  Moves the responsibility from one entry in tblBillingresponsibility to another
''''Parameters: lngBRIDFrom -  key of the BR record to move resp. from.
''''                   lngBRIDTo -  key of the BR record to move resp. to or -1 to move all remaining balances to the next payer
''''                   curAmount  - The amount of resp. to move
''''Returns: Null
''''--------------------------------------------------------------------
'''     Dim objBR As BillingDB.CBRDB
'''     Dim objCred As PostingBz.CRPCredBz
'''
'''     Dim rstBR As ADODB.Recordset
'''     Dim rstCred As ADODB.Recordset
'''     Dim strError As String
'''
'''     On Error GoTo ErrTrap:
'''
'''     strError = ""
'''     Set objBR = CreateObject("BillingDB.CBRDB")
'''
'''    'check data
'''    If curAmount <= 0 And lngBRIDDest > 0 Then
'''        strError = "You must specify a positive amount to transfer"
'''        GoTo ErrTrap
'''    End If
'''    If lngBRIDDest > 0 Then
'''        If Not SameBill(lngBRIDSource, lngBRIDDest) Then
'''            strError = "The source and destination payers must be on the same bill"
'''            GoTo ErrTrap
'''        End If
'''    End If
'''
'''    If objBR.isAnEAP(lngBRIDSource, lngBRIDDest) Then
'''        strError = "Cannot transfer balances when an EAP is involved!"
'''        GoTo ErrTrap
'''    End If
'''
'''     objBR.TransferResponsibility lngBRIDSource, lngBRIDDest, curAmount
'''
'''     'adjust balance, if applicable
'''     If blnAdjustBalance Then
'''        Set rstBR = objBR.FetchByID(lngBRIDSource)
'''        If Not IsNull(rstBR.Fields("fldPayerID_Plan").Value) Then
'''            Set rstBR = objBR.FetchByID(lngBRIDDest)
'''            'we do not adjust for plans - exit!
'''            If Not IsNull(rstBR.Fields("fldPayerID_Plan").Value) Then GoTo Cleanup
'''        End If
'''
'''        Set objCred = CreateObject("PostingBz.CRPCredBz")
'''
'''        If rstBR.Fields("fldAmtOutstanding").Value < 0 Then
'''            'move to credit
'''            objCred.StoreCredit -1, rstBR.Fields("fldBRID").Value, Abs(rstBR.Fields("fldAmtOutstanding").Value), strUserName
'''        ElseIf rstBR.Fields("fldAmtOutstanding").Value > 0 Then
'''            'apply a credit
'''            Set rstCred = objCred.FetchByEL(rstBR.Fields("fldencounterlogid").Value)
'''            If Not rstCred.EOF Then
'''                If rstCred.Fields("fldAmount").Value > 0 Then
'''                    If rstCred.Fields("fldAmount").Value < rstBR.Fields("fldAmtOutstanding").Value Then
'''                        objCred.ApplyCredit rstCred.Fields("fldPatProvCreditID").Value, rstBR.Fields("fldBRID").Value, rstCred.Fields("fldAmount").Value, strUserName
'''                    Else
'''                        objCred.ApplyCredit rstCred.Fields("fldPatProvCreditID").Value, rstBR.Fields("fldBRID").Value, rstBR.Fields("fldAmtOutstanding").Value, strUserName
'''                    End If
'''                End If
'''            End If
'''        End If
'''
'''        Set objCred = Nothing
'''        Set rstCred = Nothing
'''     End If
'''
'''    'Signal successful completion
'''Cleanup:
'''    GetObjectContext.SetComplete
'''    Set objBR = Nothing
'''    Set rstBR = Nothing
'''    Set objCred = Nothing
'''    Set rstCred = Nothing
'''Exit Sub
'''ErrTrap:
'''    'Signal incompletion and raise the error to the calling environment.
'''    Set objBR = Nothing
'''    Set rstBR = Nothing
'''    Set objCred = Nothing
'''    Set rstCred = Nothing
'''    GetObjectContext.SetAbort
'''    If strError = "" Then
'''       Err.Raise Err.Number, Err.Source, Err.Description
'''    Else
'''       Err.Raise -1, CLASS_NAME, strError
'''    End If
''' End Sub

Public Function SameBill(ByVal lngBRID1 As Long, ByVal lngBRID2 As Long) As Boolean
'--------------------------------------------------------------------
'Date: 02/24/2000
'Author: Eric Pena
'Description:  Determines if two BRIDs are recorded under the same bill
'Parameters: lngBRID1 / lngBRID2 -  key of the BR record to check
'Returns: True if both records are under the same bill, False otherwise
'--------------------------------------------------------------------
    Dim objBR As BillingDB.CBRDB
     
    On Error GoTo ErrTrap:
     
    Set objBR = CreateObject("BillingDB.CBRDB")
          
    SameBill = objBR.SameBill(lngBRID1, lngBRID2)
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objBR = Nothing
    Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objBR = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
 End Function
Public Function FetchByPatient(ByVal lngPatientID As Long, ByVal blnOutstandingOnlyYN As Boolean) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 9/18/2000
'Author: Eric Pena
'Description:  Returns rows from tblBillingResponsibility that have an outstanding balance utilizing a stored procedure
'Parameters: lngPatientID - returns BR records for this patient only
'Returns: ADORst
'--------------------------------------------------------------------
     Dim objBR As BillingDB.CBRDB
     
     On Error GoTo ErrTrap:
     
     Set objBR = CreateObject("BillingDB.CBRDB")
     Set FetchByPatient = objBR.FetchByPatient(lngPatientID, blnOutstandingOnlyYN)
    
    'Signal successful completion
     GetObjectContext.SetComplete
     Set objBR = Nothing
     Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objBR = Nothing
     GetObjectContext.SetAbort
     Err.Raise Err.Number, Err.Source, Err.Description
 End Function
Public Function FetchByPayerPatientBalance(ByVal lngInsuranceID As Long, ByVal dteStartDOS As Date, ByVal dteEndDOS As Date) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 9/18/2000
'Author: Eric Pena
'Description:  Returns rows from tblBillingResponsibility that have an outstanding balance utilizing a stored procedure
'Parameters: lngPayerID - returns BR records for this payer only
'                   blnPayerIsPlanYN - true if specified payer is a plan, false if it is a rp
'Returns: ADORst
'--------------------------------------------------------------------
     Dim objBR As BillingDB.CBRDB
     
     On Error GoTo ErrTrap:
     
     Set objBR = CreateObject("BillingDB.CBRDB")
     Set FetchByPayerPatientBalance = objBR.FetchByPayerPatientBalance(lngInsuranceID, dteStartDOS, dteEndDOS)
    
    'Signal successful completion
     GetObjectContext.SetComplete
     Set objBR = Nothing
     Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objBR = Nothing
     GetObjectContext.SetAbort
     Err.Raise Err.Number, Err.Source, Err.Description
 End Function
Public Function FetchByPatientProvider(ByVal lngPatientID As Long, ByVal lngProviderID As Long, ByVal blnOutstandingOnlyYN As Boolean) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 9/18/2000
'Author: Eric Pena
'Description:  Returns rows from tblBillingResponsibility that have an outstanding balance utilizing a stored procedure
'Parameters: lngPatientID - returns BR records for this patient only
'Returns: ADORst
'--------------------------------------------------------------------
     Dim objBR As BillingDB.CBRDB
     
     On Error GoTo ErrTrap:
     
     Set objBR = CreateObject("BillingDB.CBRDB")
     Set FetchByPatientProvider = objBR.FetchByPatientProvider(lngPatientID, lngProviderID, blnOutstandingOnlyYN)
    
    'Signal successful completion
     GetObjectContext.SetComplete
     Set objBR = Nothing
     Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objBR = Nothing
     GetObjectContext.SetAbort
     Err.Raise Err.Number, Err.Source, Err.Description
 End Function
 Public Function FetchByID(lngBRID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 08/31/2000
'Author: Eric Pena
'Description:  Returns rows from the tblBillingResponsibility and tblInsCompany tables utilizing a stored procedure
'Parameters: blnIncludeDisabled - determines if disabled records will be returned
'Returns: ADORst
'--------------------------------------------------------------------
     Dim objBR As BillingDB.CBRDB
     
     On Error GoTo ErrTrap:
     
     Set objBR = CreateObject("BillingDB.CBRDB")
     Set FetchByID = objBR.FetchByID(lngBRID)
    
    'Signal successful completion
     GetObjectContext.SetComplete
     Set objBR = Nothing
     Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objBR = Nothing
     GetObjectContext.SetAbort
     Err.Raise Err.Number, Err.Source, Err.Description
 End Function
Public Function isAnEAP(ByVal lngBRIDToTest1 As Long, Optional ByVal lngBRIDToTest2 As Long = -1) As Boolean
'--------------------------------------------------------------------
'Date: 03/15/2001
'Author: Eric Pena
'Description:  Checks to see if an EAP is linked to either BR record specified
'Parameters: lngBRIDToTest1/lngBRIDToTest2 -  key of the BR record to test for EAP linkage
'Returns: True if either BRID links to an EAP, False otherwise
'--------------------------------------------------------------------
     Dim objBR As BillingDB.CBRDB
     
     On Error GoTo ErrTrap:
     
     isAnEAP = False
     
     Set objBR = CreateObject("BillingDB.CBRDB")
     isAnEAP = objBR.isAnEAP(lngBRIDToTest1, lngBRIDToTest2)
    
    'Signal successful completion
     GetObjectContext.SetComplete
     Set objBR = Nothing
     Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objBR = Nothing
     GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
 End Function
Public Sub UpdateCert(ByVal lngELID As Long, ByVal lngRPID As Long, ByVal lngPlanID As Long, ByVal strCertNum As String)
'--------------------------------------------------------------------
'Date: 08/02/2002
'Author: Eric Pena
'Description:  Updates a cert in tblBillingResponsibility utilizing a stored procedure.
'Parameters:
'Returns: Null
'--------------------------------------------------------------------
    
    Dim objBR As BillingDB.CBRDB

    On Error GoTo ErrTrap:

    Set objBR = CreateObject("BillingDB.CBRDB")
    Call objBR.UpdateCert(lngELID, lngRPID, lngPlanID, strCertNum)
    Set objBR = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Sub

ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set objBR = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub


