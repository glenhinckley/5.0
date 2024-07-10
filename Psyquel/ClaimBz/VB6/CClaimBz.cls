VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 2  'RequiresTransaction
END
Attribute VB_Name = "CClaimBz"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'-------------------------------------------------------------------------------------
'Date: 10/10/2000
'Class Name: CClaimBz
'Author: Juan Castro
'Description:   MTS object designed to host methods associated with data affecting tblClaim table.
'--------------------------------------------------------------------------------------
'Revision History:
'  R001: Richkun 01/24/2003 - Added SubmitClaim() method with intentions of replacing Submit() method
'  R002: Richkun: 03/10/2003 - Added FetchNextBatchNumber()
'--------------------------------------------------------------------------------------

Private Const TABLE_NAME As String = "tblClaim"
Private Const CLASS_NAME As String = "CClaimBz"
Option Explicit

Public Function Insert(ByVal lngELID As Long, ByVal intVersionNum As Integer, ByVal lngRPID As Long, _
                       ByVal lngClinicID As Long, ByVal lngPlanID As Long, ByVal lngCPCID As Long, _
                       ByVal lngInsuranceID As Long, ByVal intOrder As Integer, ByVal strUserName As String) As Long
'-------------------------------------------------------------------------------------
'Date:          7/11/2003
'Author:        Eric Pena
'Description:   Creates a row in tblClaim (Claim to submit to insurance company).  If a row already exists,
'               the next version of the claim will be inserted.  (i.e., disable current version, add a row
'               with an incremented version number.
'Parameters:    lngELID - ID of DOS to generate claim for (PK in tblEncounterLog)
'               lngRPID - ID of insured (Responsible party) (FK to tblPatRPPlan
'               lngPlanID - Plan covering the specified RP (FK to same row as above in tblPatRPPlan)
'Returns:       ID (Primary Key) of the row inserted.
'-------------------------------------------------------------------------------------
      Dim objSubmit As ClaimDB.CClaimDB
      Dim strErrMessage As String
      
      On Error GoTo ErrTrap:
      
      'Call function to verify the data inputed, if false goes to ErrTrap, if True
      'call insert function in ClaimDB object
'      If Not VerifyData(0, lngELID, lngRPID, lngPlanID, strErrMessage) Then GoTo ErrTrap
      
      Set objSubmit = CreateObject("ClaimDB.CClaimDB")
      Insert = objSubmit.Insert(lngELID, intVersionNum, lngRPID, lngClinicID, lngPlanID, _
                                lngCPCID, lngInsuranceID, intOrder, strUserName)
      
      'Signal a successful completion
      GetObjectContext.SetComplete
      Set objSubmit = Nothing
      Exit Function
      
ErrTrap:
      'Signal incompletion and raise the error to the calling enviroment
      'The condition handles custom business rule checks we have established
      GetObjectContext.SetAbort
      Set objSubmit = Nothing
      If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME & ".Insert", strErrMessage
      Else
        Err.Raise Err.Number, Err.Source, Err.Description
      End If

End Function

Public Sub DeleteBeforeSubmit(ByVal lngELID As Long)
'-------------------------------------------------------------------------------------
'Date: 08/06/2002
'Author: Dave Richkun
'Description: Physically deletes a row in tblClaim using the Encounter Log ID as the
'             identifier.  This method is intended to be be called only when the
'             claim has not yet been printed to a HCFA or had an electronic claim
'             record created for it.  Since much more is involved before the row
'             can be deleted, the calling procedure is responsible for ensuring the
'             claim status is still Queued.
'Parameters:  lngELID - ID of encounter log used to identify claim to delete
'Returns: Null
'-------------------------------------------------------------------------------------
      Dim objClaim As ClaimDB.CClaimDB
      Dim strErrMessage As String
      
      On Error GoTo ErrTrap:
      
      Set objClaim = CreateObject("ClaimDB.CClaimDB")
      Call objClaim.DeleteBeforeSubmit(lngELID)
      Set objClaim = Nothing
      
      'Signal a successful completion
      GetObjectContext.SetComplete

      Exit Sub
      
ErrTrap:
      'Signal incompletion and raise the error to the calling enviroment
      'The condition handles custom business rule checks we have established
      GetObjectContext.SetAbort
      Set objClaim = Nothing
      If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME & ".DeleteBeforeSubmit", strErrMessage
      Else
        Err.Raise Err.Number, Err.Source, Err.Description
      End If

End Sub


Private Function VerifyData(ByVal lngClaimID As Long, ByVal lngELID As Long, ByVal lngRPID As Long, _
                            ByVal lngPlanID As Long, strErrMessage As String) As Boolean
'--------------------------------------------------------------------------------------
'Date: 10/11/2000
'Author: Juan Castro
'Description:       Verifies that all data has been provided by the user.
'Parameters:        Values to be verified.
'Returns:           Boolean value identifying if all required criteria has been satisfied.
'--------------------------------------------------------------------------------------
    
    If lngClaimID = 0 Then
        'A new Claim
        If lngELID < 1 Then
            strErrMessage = "A valid Encounter is needed to submit a claim."
            VerifyData = False
            Exit Function
        End If
        If lngRPID < 1 Then
            strErrMessage = "A valid Responsibile Party is required."
            VerifyData = False
            Exit Function
        End If
        
        If lngPlanID < 1 Then
            strErrMessage = "A valid Plan is required."
            VerifyData = False
            Exit Function
        End If
    End If
    
    VerifyData = True
End Function








Public Function FetchByID(ByVal lngClaimID As Long) As ADODB.Recordset
'-------------------------------------------------------------------------------------
'Date: 02/19/2003
'Author: Dave Richkun
'Description: Retrieves detailed information for a single claim
'Parameters: lngClaimID - ID of claim whose information is to be retrieved
'Returns: ADODB.Recordset
'-------------------------------------------------------------------------------------

    Dim objClaim As ClaimDB.CClaimDB
    
    On Error GoTo ErrTrap:
    
    Set objClaim = CreateObject("ClaimDB.CClaimDB")
    Set FetchByID = objClaim.FetchByID(lngClaimID)
    Set objClaim = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling enviroment.
    Set objClaim = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
    
End Function

Public Sub Update(ByVal lngClaimID As Long, ByVal blnElectClaimYN As Boolean, ByVal strFileName As String, _
                  ByVal strInsuredID As String, ByVal strPatientName As String, ByVal strPatientStreetNum As String, _
                  ByVal strPatientCity As String, ByVal strPatientState As String, ByVal strPatientZip As String, _
                  ByVal strPatientPhone As String, ByVal strPatientRelation As String, ByVal strPatientDOB As Date, _
                  ByVal strPatientSex As String, ByVal strInsuredName As String, ByVal strInsuredStreetNum As String, _
                  ByVal strInsuredCity As String, ByVal strInsuredState As String, ByVal strInsuredZip As String, _
                  ByVal strInsuredPhone As String, ByVal strMaritalStatus As String, ByVal strEmployStatus As String, _
                  ByVal strOthInsdName As String, ByVal strOthInsdGroupNum As String, ByVal dteOthInsdDOB As Date, _
                  ByVal strOthInsdSex As String, ByVal strOthInsdEmployer As String, ByVal strOthPlanName As String, _
                  ByVal blnConditionEmployYN As Boolean, ByVal blnConditionAutoYN As Boolean, ByVal blnConditionOtherYN As Boolean, _
                  ByVal strInsdGroupNum As String, ByVal dteInsdDOB As Date, ByVal strInsdSex As String, ByVal strInsdEmployer As String, _
                  ByVal strPlanName As String, ByVal blnOthInsYN As Boolean, ByVal strProviderSigned As String, ByVal strCertNum As String, _
                  ByVal strFacilityLine1 As String, ByVal strFacilityLine2 As String, ByVal strFacilityCity As String, _
                  ByVal strFacilityState As String, ByVal strFacilityZip As String, ByVal strBillingCompanyLine1 As String, _
                  ByVal strBillingCompanyCity As String, ByVal strBillingCompanyState As String, ByVal strBillingCompanyZip As String)
'-------------------------------------------------------------------------------------
'Date: 10/17/00
'Author: Juan Castro
'Description:   Updates a row in tblClaim table utilizing a stored procedure.
'Parameters:    Each parameter identifies the column value that will be inserted
'Returns:       Null
'-------------------------------------------------------------------------------------

    Dim objSubmit As ClaimDB.CClaimDB
    Dim strErrMessage As String
    
    On Error GoTo ErrTrap:
    
    If Not VerifyData(lngClaimID, 0, 0, 0, strErrMessage) Then GoTo ErrTrap
    
    Set objSubmit = CreateObject("ClaimDB.CClaimDB")
    objSubmit.Update lngClaimID, blnElectClaimYN, strFileName, strInsuredID, _
                     strPatientName, strPatientStreetNum, strPatientCity, strPatientState, _
                     strPatientZip, strPatientPhone, strPatientRelation, strPatientDOB, strPatientSex, _
                     strInsuredName, strInsuredStreetNum, strInsuredCity, strInsuredState, _
                     strInsuredZip, strInsuredPhone, strMaritalStatus, strEmployStatus, _
                     strOthInsdName, strOthInsdGroupNum, dteOthInsdDOB, strOthInsdSex, _
                     strOthInsdEmployer, strOthPlanName, blnConditionEmployYN, blnConditionAutoYN, _
                     blnConditionOtherYN, strInsdGroupNum, dteInsdDOB, strInsdSex, strInsdEmployer, _
                     strPlanName, blnOthInsYN, strProviderSigned, strCertNum, strFacilityLine1, strFacilityLine2, _
                     strFacilityCity, strFacilityState, strFacilityZip, strBillingCompanyLine1, _
                     strBillingCompanyCity, strBillingCompanyState, strBillingCompanyZip
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objSubmit = Nothing
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise  the error to the calling enviroment. The condition
    'handles custome business rule checks we may have established.
    GetObjectContext.SetAbort
    Set objSubmit = Nothing
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
                        
End Sub

Public Sub SetClaimMethod(ByVal lngClaimID As Long, ByVal blnElectClaimYN As Boolean, _
                               ByVal strFileName As String)
'--------------------------------------------------------------------------------------
'Date: 10/16/00
'Author: Juan Castro
'Description:   Modifies the fldElectClaimYN and fldFileName fields in tbl Claim
'Parameters:    Each parameter identifies the column value that will be updated.
'Returns:       Null
'--------------------------------------------------------------------------------------
    
    Dim objSubmit As ClaimDB.CClaimDB
    Dim strErrMessage As String
    
    On Error GoTo ErrTrap:
    
    If Not VerifyData(lngClaimID, 0, 0, 0, strErrMessage) Then GoTo ErrTrap
    
    Set objSubmit = CreateObject("ClaimDB.CClaimDB")
    
    objSubmit.SetClaimMethod lngClaimID, blnElectClaimYN, strFileName
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objSubmit = Nothing
    Exit Sub
    
ErrTrap:
    'Signal Completion and raise the error to the calling enviroment. The condition
    'handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    Set objSubmit = Nothing
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
    
End Sub

Public Sub Queued(ByVal lngClaimID As Long, ByVal blnQueuedYN As Boolean)
'--------------------------------------------------------------------------------------
'Date: 10/16/2000
'Author: Juan Castro
'Description:   Updates the fldQueuedYN field in tblClaim table.
'Parameters:    blnQueuedYN - identifies the column value that wil be updated.
'Returns:       Null
'--------------------------------------------------------------------------------------

    Dim objSubmit As ClaimDB.CClaimDB
    Dim strErrMessage As String
    
    On Error GoTo ErrTrap:
    
    If Not VerifyData(lngClaimID, 0, 0, 0, strErrMessage) Then GoTo ErrTrap
    
    Set objSubmit = CreateObject("ClaimDB.CClaimDB")
    
    objSubmit.Queued lngClaimID, blnQueuedYN
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objSubmit = Nothing
    Exit Sub
    
ErrTrap:
    'Signal Completion and raise the error to the calling enviroment. The condition
    'handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    Set objSubmit = Nothing
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
    
End Sub

Public Sub Resubmittal(ByVal lngClaimID As Long, ByVal blnResubmittal As Boolean)
'--------------------------------------------------------------------------------------
'Date: 10/16/2000
'Author: Juan Castro
'Description:   Updates a fldResubmittalYN field in tblClaim table.
'Parameters:    blnResubmittalYN - identifies the column value that wil be updated.
'Returns:       Null
'--------------------------------------------------------------------------------------

    Dim objSubmit As ClaimDB.CClaimDB
    Dim strErrMessage As String
    
    On Error GoTo ErrTrap:
    
    If Not VerifyData(lngClaimID, 0, 0, 0, strErrMessage) Then GoTo ErrTrap
    
    Set objSubmit = CreateObject("ClaimDB.CClaimDB")
    
    objSubmit.Resubmittal lngClaimID, blnResubmittal
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objSubmit = Nothing
    Exit Sub
    
ErrTrap:
    'Signal Completion and raise the error to the calling enviroment. The condition
    'handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    Set objSubmit = Nothing
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If

End Sub

Public Sub Submit(ByVal dtePrintDate As Date, ByVal lngClaimID As Long, ByVal strUserName As String, Optional ByVal lngPlanID As Long = -1, Optional ByVal lngCPCID As Long = -1, _
                    Optional ByVal lngInsuranceID As Long = -1, Optional ByVal dteDOSFrom As Date = 0, Optional ByVal dteDOSTo As Date = 0, _
                    Optional ByVal lngPatientID As Long = -1, Optional ByVal lngProviderID As Long = -1, Optional ByVal strResubmittalYN As String = "", _
                    Optional ByVal strQueuedYN As String = "", Optional ByVal dtePostFrom As Date = 0, Optional ByVal dtePostTo As Date = 0, _
                    Optional ByVal dteNextActionFrom As Date = 0, Optional ByVal dteNextActionTo As Date = 0, Optional ByVal dteSubmitFrom As Date = 0, _
                    Optional ByVal dteSubmitTo As Date = 0, Optional ByVal lngNum As Long = -1, Optional ByVal lngNumTo As Long = -1)
'--------------------------------------------------------------------------------------
'Date: 10/16/00
'Author: Juan Castro
'Description:   Updates fields fldQueuedYN, fldResubmittalYN, fldLastSubmitDate, and
'               fldTimesSubmitted in table tblClaim
'Parameters:    lngClaimID - identifies row to be updated
'Returns:       Null
'--------------------------------------------------------------------------------------

    Dim objSubmit As ClaimDB.CClaimDB
    Dim strErrMessage As String
    
    On Error GoTo ErrTrap:
    
    Set objSubmit = CreateObject("ClaimDB.CClaimDB")
    objSubmit.Submit dtePrintDate, lngClaimID, strUserName, lngPlanID, lngCPCID, lngInsuranceID, dteDOSFrom, dteDOSTo, lngPatientID, lngProviderID, strResubmittalYN, _
                         strQueuedYN, dtePostFrom, dtePostTo, dteNextActionFrom, dteNextActionTo, dteSubmitFrom, dteSubmitTo, lngNum, lngNumTo
    GetObjectContext.SetComplete
    
    'Signal successful completion
    Set objSubmit = Nothing
    Exit Sub
    
ErrTrap:
    'Signal Completion and raise the error to the calling enviroment. The condition
    'handles custom business rule checks we may have established.
    Set objSubmit = Nothing
    GetObjectContext.SetAbort
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Sub
Public Function FetchClaimCrit(ByVal lngPlanID As Long, ByVal lngCPCID As Long, ByVal lngInsuranceID As Long, ByVal dteDOSFrom As Date, _
                                    ByVal dteDOSTo As Date, ByVal lngPatientID As Long, ByVal lngProviderID As Long, ByVal strResubmittalYN As String, _
                                    ByVal strQueuedYN As String, ByVal dtePostFrom As Date, ByVal dtePostTo As Date, ByVal dteNextActionFrom As Date, _
                                    ByVal dteNextActionTo As Date, ByVal dteSubmitFrom As Date, ByVal dteSubmitTo As Date, ByVal lngNum As Long, ByVal lngNumTo As Long) As ADODB.Recordset
'--------------------------------------------------------------------------------------
'Date: 10/16/2000
'Author: Juan Castro
'Description:   Returns outstanding claims based on any combination of the specified parameters
'Parameters:    All parameters are criteria to use in searching for Claims
'Returns:       ADODB.Recordset
'--------------------------------------------------------------------------------------

    Dim objSubmit As ClaimDB.CClaimDB
    
    On Error GoTo ErrTrap:
    
    Set objSubmit = CreateObject("ClaimDB.CClaimDB")
    Set FetchClaimCrit = objSubmit.FetchClaimCrit(lngPlanID, lngCPCID, lngInsuranceID, dteDOSFrom, dteDOSTo, lngPatientID, lngProviderID, strResubmittalYN, _
         strQueuedYN, dtePostFrom, dtePostTo, dteNextActionFrom, dteNextActionTo, dteSubmitFrom, dteSubmitTo, lngNum, lngNumTo)
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objSubmit = Nothing
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling enviroment.
    Set objSubmit = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
    
End Function
Public Function FetchByBR(ByVal lngBRID As Long) As ADODB.Recordset
'--------------------------------------------------------------------------------------
'Date: 10/11/200
'Author: Juan Castro
'Description:       Retrieves records from tblClaim table utilizing a stored procedure.
'Parameters:        lngClaimID
'Returns:           ADODB.Recordset
'--------------------------------------------------------------------------------------

    Dim objSubmit As ClaimDB.CClaimDB
    
    On Error GoTo ErrTrap:
    
    Set objSubmit = CreateObject("ClaimDB.CClaimDB")
    Set FetchByBR = objSubmit.FetchByBR(lngBRID)
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objSubmit = Nothing
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling enviroment.
    Set objSubmit = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
    
End Function
Public Sub SetSubmitStatus(ByVal lngClaimID As Long, Optional ByVal strDataBase As String)
'--------------------------------------------------------------------------------------
'Date: 10/16/00
'Author: Juan Castro
'Description:   Updates fields fldQueuedYN, fldResubmittalYN, fldLastSubmitDate, and
'               fldTimesSubmitted in table tblClaim
'Parameters:    lngClaimID - identifies row to be updated
'Returns:       Null
'--------------------------------------------------------------------------------------

    Dim objSubmit As ClaimDB.CClaimDB
    Dim strErrMessage As String
    
    On Error GoTo ErrTrap:
    
    Set objSubmit = CreateObject("ClaimDB.CClaimDB")
    objSubmit.SetSubmitStatus lngClaimID, strDataBase
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objSubmit = Nothing
    Exit Sub
    
ErrTrap:
    'Signal Completion and raise the error to the calling enviroment. The condition
    'handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    Set objSubmit = Nothing
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Sub
Public Function FetchElectClaims(ByVal lngElectClaimID As Long, Optional ByVal strDataBase As String) As ADODB.Recordset
'--------------------------------------------------------------------------------------
'Date: 10/11/200
'Author: Juan Castro
'Description:       Retrieves records from tblClaim table utilizing a stored procedure.
'Parameters:        lngClaimID
'Returns:           ADODB.Recordset
'--------------------------------------------------------------------------------------

    Dim objSubmit As ClaimDB.CClaimDB
    
    On Error GoTo ErrTrap:
    
    Set objSubmit = CreateObject("ClaimDB.CClaimDB")
    Set FetchElectClaims = objSubmit.FetchElectClaims(lngElectClaimID, strDataBase)
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objSubmit = Nothing
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling enviroment.
    Set objSubmit = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
    
End Function
Public Function FetchOtherInsInfo(ByVal lngEncounterLogID As Long, ByVal lngOrder As Long) As ADODB.Recordset
'--------------------------------------------------------------------------------------
'Date: 10/11/200
'Author: Juan Castro
'Description:       Retrieves records from tblClaim table utilizing a stored procedure.
'Parameters:        lngClaimID
'Returns:           ADODB.Recordset
'--------------------------------------------------------------------------------------

    Dim objSubmit As ClaimDB.CClaimDB
    
    On Error GoTo ErrTrap:
    
    Set objSubmit = CreateObject("ClaimDB.CClaimDB")
    Set FetchOtherInsInfo = objSubmit.FetchOtherInsInfo(lngEncounterLogID, lngOrder)
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objSubmit = Nothing
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling enviroment.
    Set objSubmit = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
    
End Function
Public Sub CloseElectClaims()
'--------------------------------------------------------------------------------------
'Date: 10/16/00
'Author: Juan Castro
'Description:   Updates fields fldQueuedYN, fldResubmittalYN, fldLastSubmitDate, and
'               fldTimesSubmitted in table tblClaim
'Parameters:    lngClaimID - identifies row to be updated
'Returns:       Null
'--------------------------------------------------------------------------------------

    Dim objSubmit As ClaimDB.CClaimDB
    Dim strErrMessage As String
    
    On Error GoTo ErrTrap:
    
    Set objSubmit = CreateObject("ClaimDB.CClaimDB")
    objSubmit.CloseElectClaims
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objSubmit = Nothing
    Exit Sub
    
ErrTrap:
    'Signal Completion and raise the error to the calling enviroment. The condition
    'handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    Set objSubmit = Nothing
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Sub
Public Function FetchUnsentElectClaims() As ADODB.Recordset
'--------------------------------------------------------------------------------------
'Date: 10/11/200
'Author: Juan Castro
'Description:       Retrieves records from tblClaim table utilizing a stored procedure.
'Parameters:        lngClaimID
'Returns:           ADODB.Recordset
'--------------------------------------------------------------------------------------

    Dim objSubmit As ClaimDB.CClaimDB
    
    On Error GoTo ErrTrap:
    
    Set objSubmit = CreateObject("ClaimDB.CClaimDB")
    Set FetchUnsentElectClaims = objSubmit.FetchUnsentElectClaims()
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objSubmit = Nothing
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling enviroment.
    Set objSubmit = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
    
End Function
Public Sub SendElectClaim(ByVal lngElectClaimID As Long, ByVal strFileName As String, ByVal strUserName As String)
'--------------------------------------------------------------------------------------
'Date: 10/16/00
'Author: Juan Castro
'Description:   Updates fields fldQueuedYN, fldResubmittalYN, fldLastSubmitDate, and
'               fldTimesSubmitted in table tblClaim
'Parameters:    lngClaimID - identifies row to be updated
'Returns:       Null
'--------------------------------------------------------------------------------------

    Dim objSubmit As ClaimDB.CClaimDB
    Dim strErrMessage As String
    
    On Error GoTo ErrTrap:
    
    Set objSubmit = CreateObject("ClaimDB.CClaimDB")
    objSubmit.SendElectClaim lngElectClaimID, strFileName, strUserName
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objSubmit = Nothing
    Exit Sub
    
ErrTrap:
    'Signal Completion and raise the error to the calling enviroment. The condition
    'handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    Set objSubmit = Nothing
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Sub


Public Sub GenerateClaim(ByVal lngClaimID As Long, Optional ByVal blnQueueClaim As Boolean = False, Optional ByVal strUserName As String)
'--------------------------------------------------------------------------------------
'Date: 12/09/2002
'Author: Dave Richkun
'Description:   Updates tblClaim, filling all related HCFA data for a specific claim
'Parameters:    lngClaimID - ID of claim that is to be generated
'               blnQueueClaim - Identifiies if claim should be re-queued
'Returns:       Null
'--------------------------------------------------------------------------------------

    Dim objSubmit As ClaimDB.CClaimDB
    
    On Error GoTo ErrTrap:
    
    Set objSubmit = CreateObject("ClaimDB.CClaimDB")
    Call objSubmit.GenerateClaim(lngClaimID, blnQueueClaim, strUserName)
    Set objSubmit = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Sub
    
ErrTrap:
    'Signal Completion and raise the error to the calling enviroment. The condition
    'handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    Set objSubmit = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub


Public Function FetchClaimsByCriteria(ByVal strQueuedYN As String, ByVal lngELID As Long, _
                    ByVal lngPatientID As Long, ByVal strPatientLast As String, _
                    ByVal strPatientFirst As String, ByVal lngProviderID As Long, _
                    ByVal strProviderLast As String, ByVal strProviderFirst As String, _
                    ByVal lngInsuranceID As Long, ByVal lngCPCID As Long, ByVal dtDOSFrom As Date, _
                    ByVal dtDOSTo As Date, ByVal dtSubmitFrom As Date, ByVal dtSubmitTo As Date, _
                    ByVal strEClaimYN As String, Optional ByVal strDataBase As String) As ADODB.Recordset
'--------------------------------------------------------------------------------------
'Date: 01/23/2003
'Author: Dave Richkun
'Description: Returns claims from tblClaim using dynamic criteria
'Parameters: All parameters are criteria to use in searching for claims
'Returns: ADODB.Recordset
'--------------------------------------------------------------------------------------

    Dim objClaim As ClaimDB.CClaimDB
    
    On Error GoTo ErrTrap:
    
    Set objClaim = CreateObject("ClaimDB.CClaimDB")
    Set FetchClaimsByCriteria = objClaim.FetchClaimsByCriteria(strQueuedYN, lngELID, _
                    lngPatientID, strPatientLast, strPatientFirst, lngProviderID, _
                    strProviderLast, strProviderFirst, lngInsuranceID, lngCPCID, _
                    dtDOSFrom, dtDOSTo, dtSubmitFrom, dtSubmitTo, strEClaimYN, strDataBase)
    
    Set objClaim = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling enviroment.
    Set objClaim = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Function


Public Sub SubmitClaim(ByVal dtSubmitDate As Date, ByVal lngClaimID As Long, _
                       ByVal lngELID As Long, ByVal lngPlanID As Long, _
                       ByVal lngTxTypeID As Long, ByVal lngBatchNum As Long, _
                       ByVal strUserName As String, Optional ByVal strDataBase As String)
'--------------------------------------------------------------------------------------
'Date: 01/24/2003
'Author: Dave Richkun
'Description: Records information to identify when a claim was submitted, either
'             via a printed claim or an electronic claim.
'Parameters:
'Returns: None
'--------------------------------------------------------------------------------------

    Dim objClaim As ClaimDB.CClaimDB
    
    On Error GoTo ErrTrap:
    
    Set objClaim = CreateObject("ClaimDB.CClaimDB")
    Call objClaim.SubmitClaim(dtSubmitDate, lngClaimID, lngELID, lngPlanID, lngTxTypeID, lngBatchNum, strUserName, strDataBase)
    Set objClaim = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling enviroment.
    Set objClaim = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub


Public Function FetchEFileXRef(ByVal lngEFileID As Long) As ADODB.Recordset
'-------------------------------------------------------------------------------------
'Date: 03/03/2003
'Author: Dave Richkun
'Description: Retrieves Psyquel-related claim information for a given E-Claim
'Parameters: lngEFileID - ID of the THIN claim to which we will cross-reference
'Returns: ADODB.Recordset
'-------------------------------------------------------------------------------------

    Dim objClaim As ClaimDB.CClaimDB
    
    On Error GoTo ErrTrap:
    
    Set objClaim = CreateObject("ClaimDB.CClaimDB")
    Set FetchEFileXRef = objClaim.FetchEFileXRef(lngEFileID)
    Set objClaim = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling enviroment.
    Set objClaim = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Function


Public Sub PostRejection(ByVal lngELID As Long, ByVal lngEFileID As Long, _
                         ByVal strMsg As String, ByVal strUserName As String, _
                         Optional ByVal dtePost As Date = 0, Optional ByVal lngPlanID As Long, _
                         Optional ByVal strDataBase As String)
'--------------------------------------------------------------------------------------
'Date: 03/03/2003
'Author: Dave Richkun
'Description: Posts a rejection transaction to a claim
'Parameters: lngELID - ID of encounter
'            lngEFileID - ID of the electronic claim
'            strMsg - Rejection message from the payer/clearinghouse
'Returns: Null
'--------------------------------------------------------------------------------------

    Dim objClaim As ClaimDB.CClaimDB
    
    On Error GoTo ErrTrap:
    
    Set objClaim = CreateObject("ClaimDB.CClaimDB")
    Call objClaim.PostRejection(lngELID, lngEFileID, strMsg, strUserName, dtePost, lngPlanID, strDataBase)
    Set objClaim = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling enviroment.
    Set objClaim = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub


Public Sub UndoRejection(ByVal lngELID As Long)
'--------------------------------------------------------------------------------------
'Date: 03/06/2003
'Author: Dave Richkun
'Description: Removes the last rejection transaction from a claim
'Parameters: lngELID - ID of encounter
'Returns: Null
'--------------------------------------------------------------------------------------

    Dim objClaim As ClaimDB.CClaimDB
    
    On Error GoTo ErrTrap:
    
    Set objClaim = CreateObject("ClaimDB.CClaimDB")
    Call objClaim.UndoRejection(lngELID)
    Set objClaim = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling enviroment.
    Set objClaim = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub


Public Sub PostTransaction(ByVal lngELID As Long, ByVal lngEFileID As Long, _
                           ByVal strMsg As String, ByVal strUserName As String, _
                           Optional ByVal dtePost As Date = 0, Optional ByVal lngPlanID As Long, _
                           Optional ByVal strDataBase As String)
'--------------------------------------------------------------------------------------
'Date: 03/03/2003
'Author: Dave Richkun
'Description: Posts a non-rejection transaction to a claim
'Parameters: lngELID - ID of encounter
'            lngEFileID - ID of the electronic claim
'            strMsg - Rejection message from the payer/clearinghouse
'Returns: Null
'--------------------------------------------------------------------------------------

    Dim objClaim As ClaimDB.CClaimDB
    
    On Error GoTo ErrTrap:
    
    Set objClaim = CreateObject("ClaimDB.CClaimDB")
    Call objClaim.PostTransaction(lngELID, lngEFileID, strMsg, strUserName, dtePost, lngPlanID, strDataBase)
    Set objClaim = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling enviroment.
    Set objClaim = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub

Public Sub UpdateClaimICN(ByVal lngID As Long, ByVal strICN As String)
'--------------------------------------------------------------------------------------
'Date: 01/09/2017
'Author: Duane C Orth
'Description: Update the Claims ICN Number
'Parameters: lngID - ID of claim
'            strICN - Claims ICN Number
'Returns: Null
'--------------------------------------------------------------------------------------

    Dim objClaim As ClaimDB.CClaimDB
    
    On Error GoTo ErrTrap:
    
    Set objClaim = CreateObject("ClaimDB.CClaimDB")
    Call objClaim.UpdateClaimICN(lngID, strICN)
    Set objClaim = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling enviroment.
    Set objClaim = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub

Public Sub UpdateEFileAccept(ByVal lngEFileID As Long, ByVal lngAccept As Long, _
                           ByVal lngReject As Long, ByVal strUserName As String)
'--------------------------------------------------------------------------------------
'Date: 03/03/2003
'Author: Dave Richkun
'Description: Posts a non-rejection transaction to a claim
'Parameters: lngELID - ID of encounter
'            lngEFileID - ID of the electronic claim
'            strMsg - Rejection message from the payer/clearinghouse
'Returns: Null
'--------------------------------------------------------------------------------------

    Dim objClaim As ClaimDB.CClaimDB
    
    On Error GoTo ErrTrap:
    
    Set objClaim = CreateObject("ClaimDB.CClaimDB")
    Call objClaim.UpdateEFileAccept(lngEFileID, lngAccept, lngReject, strUserName)
    Set objClaim = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling enviroment.
    Set objClaim = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub


Public Sub UpdateEFileAck(ByVal lngEFileID As Long, ByVal strAK1 As String, _
                           ByVal strAK2 As String, ByVal fldAkDetail As String, _
                           ByVal fldAK5 As String, ByVal fldAK9 As String, _
                           ByVal strUserName As String)
'--------------------------------------------------------------------------------------
'Date: 01/23/2008
'Author: Duane C Orth
'Description: Posts a Acknowledgement transaction to a claim
'Parameters: lngEFileID - ID of the electronic claim
'Returns: Null
'--------------------------------------------------------------------------------------

    Dim objClaim As ClaimDB.CClaimDB
    
    On Error GoTo ErrTrap:
    
    Set objClaim = CreateObject("ClaimDB.CClaimDB")
    Call objClaim.UpdateEFileAck(lngEFileID, strAK1, strAK2, fldAkDetail, fldAK5, fldAK9, strUserName)
    Set objClaim = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling enviroment.
    Set objClaim = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub

Public Function FetchNextBatchNumber() As Long
'-------------------------------------------------------------------------------------
'Date: 03/10/2003
'Author:Dave Richkun
'Description: Returns the next print batch number to be used when submitting a print
'             job of claims.  This method was introduced to replace the faulty method of
'             printing using the submission date.
'Parameters: None
'Returns: Batch number to be assigned to the print job.
'-------------------------------------------------------------------------------------

    Dim objClaim As ClaimDB.CClaimDB
    
    On Error GoTo ErrTrap:
    
    Set objClaim = CreateObject("ClaimDB.CClaimDB")
    FetchNextBatchNumber = objClaim.FetchNextBatchNumber()
    Set objClaim = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling enviroment.
    Set objClaim = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

