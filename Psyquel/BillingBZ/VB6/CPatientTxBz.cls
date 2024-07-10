VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 2  'RequiresTransaction
END
Attribute VB_Name = "CPatientTxBz"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'----------------------------------------------------------------------------
'Class Name: CPatientTxBz
'Date: 07/09/2002
'Author: Dave Richkun
'Description:  COM+ object designed to host methods associated with patient
'              transactions.
'----------------------------------------------------------------------------
Option Explicit

Public Function Insert(ByVal lngELID As Long, ByVal lngPatientID As Long, _
        ByVal lngProviderID As Long, ByVal lngRPID As Long, ByVal strTxTypeCode As String, _
        ByVal lngTxTypeID As Long, ByVal dtPostDate As Date, ByVal dtRcvDate As Date, _
        ByVal dblAmount As Double, ByVal strCheckNum As String, ByVal dtcheckDate As Date, _
        ByVal strCCTxID As String, ByVal strTxNotes As String, ByVal lngPatApptID As Long, ByVal strUserName As String) As Long
'--------------------------------------------------------------------
'Date: 07/09/2002
'Author: Dave Richkun
'Description:  Inserts a row into the tblPatientTx table utilizing a stored procedure.
'Parameters:
'
'   *** TO DO: Define each parameter once procedure is finalized ****
'
'Returns: ID of the row inserted.
'--------------------------------------------------------------------
    Dim objTx As BillingDB.CPatientTXDB
    Dim objBR As BillingBz.CBRBz
    
    On Error GoTo ErrTrap:
    
    Set objTx = CreateObject("BillingDB.CPatientTXDB")
    Insert = objTx.Insert(lngELID, lngPatientID, lngProviderID, lngRPID, strTxTypeCode, _
            lngTxTypeID, dtPostDate, dtRcvDate, dblAmount, strCheckNum, dtcheckDate, _
            strCCTxID, strTxNotes, lngPatApptID, strUserName)
    Set objTx = Nothing
    
    'If the transaction is associated with a claim, update Billing Responsibility
    If lngELID > 0 Then
        Set objBR = CreateObject("BillingBz.CBRBz")
        Call objBR.UpdatePatientAmount(lngELID, strUserName)
        Set objBR = Nothing
    End If
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set objTx = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

Public Function Fetch(ByVal lngPatientID As Long, Optional ByVal lngProviderID As Long = -1) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 09/06/2001
'Author: Eric Pena
'Description:  Returns rows from the tblNonPostable table utilizing a stored procedure
'Parameters: PK for row in tblNonPostable that will be fetched.
'Returns: ADORst
'--------------------------------------------------------------------
    
     Dim objPatTx As BillingDB.CPatientTXDB
     
     On Error GoTo ErrTrap:
     
     Set objPatTx = CreateObject("BillingDB.CPatientTXDB")
     Set Fetch = objPatTx.Fetch(lngPatientID, lngProviderID)
    
    'Signal successful completion
     GetObjectContext.SetComplete
     Set objPatTx = Nothing
     Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objPatTx = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
 
 End Function

Public Function FetchByID(ByVal lngTxID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 07/29/2002
'Author: Dave Richkun
'Description:  Returns detailed patient transaction information
'              associated with a specific transaction
'Parameters: lngTxID - ID of patient transaction record to which
'               transactions are associated
'Returns: Recordset of patient payment details
'--------------------------------------------------------------------

     Dim objPatTx As BillingDB.CPatientTXDB
     
     On Error GoTo ErrTrap:
     
     Set objPatTx = CreateObject("BillingDB.CPatientTXDB")
     Set FetchByID = objPatTx.FetchByID(lngTxID)
     Set objPatTx = Nothing
    
    'Signal successful completion
     GetObjectContext.SetComplete

     Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objPatTx = Nothing
    GetObjectContext.SetAbort
    
    Err.Raise Err.Number, Err.Source, Err.Description

End Function


Public Function FetchByELID(ByVal lngELID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 07/23/2002
'Author: Dave Richkun
'Description:  Returns detailed patient transaction information
'              associated with an encounter record
'Parameters: lngELID - ID of encounter record to which transactions are associated
'Returns: Recordset of patient payment details
'--------------------------------------------------------------------

     Dim objPatTx As BillingDB.CPatientTXDB
     
     On Error GoTo ErrTrap:
     
     Set objPatTx = CreateObject("BillingDB.CPatientTXDB")
     Set FetchByELID = objPatTx.FetchByELID(lngELID)
     Set objPatTx = Nothing
    
    'Signal successful completion
     GetObjectContext.SetComplete

     Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objPatTx = Nothing
    GetObjectContext.SetAbort
    
    Err.Raise Err.Number, Err.Source, Err.Description

End Function


Public Function FetchByPatientUser(ByVal lngPatientID As Long, ByVal lngUserID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 07/26/2002
'Author: Dave Richkun
'Description:  Returns patient transaction information associated with a
'              patient, considering the user executing the method.  Results
'              may vary whether the user is a provider or an Office Manager.
'Parameters: lngPatientID - ID of patient to which transactions are associated
'            lngUserID - ID of user executing this method
'Returns: Recordset of patient transactions
'--------------------------------------------------------------------
     Dim objPatTx As BillingDB.CPatientTXDB
     
     On Error GoTo ErrTrap:
         
     Set objPatTx = CreateObject("BillingDB.CPatientTXDB")
     Set FetchByPatientUser = objPatTx.FetchByPatientUser(lngPatientID, lngUserID)
     Set objPatTx = Nothing
    
    'Signal successful completion
     GetObjectContext.SetComplete

     Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objPatTx = Nothing
    GetObjectContext.SetAbort
    
    Err.Raise Err.Number, Err.Source, Err.Description

End Function


Public Function FetchByPatientProvUser(ByVal lngPatientID As Long, ByVal lngUserID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 08/28/2002
'Author: Dave Richkun
'Description:  Returns patient transaction information associated with a
'              patient ordered by Providers serviced by the user.
'Parameters: lngPatientID - ID of patient to which transactions are associated
'            lngUserID - ID of user executing this method
'Returns: Recordset of patient transactions
'--------------------------------------------------------------------
     Dim objPatTx As BillingDB.CPatientTXDB
     
     On Error GoTo ErrTrap:
     
     Set objPatTx = CreateObject("BillingDB.CPatientTXDB")
     Set FetchByPatientProvUser = objPatTx.FetchByPatientProvUser(lngPatientID, lngUserID)
     Set objPatTx = Nothing
    
    'Signal successful completion
     GetObjectContext.SetComplete

     Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objPatTx = Nothing
    GetObjectContext.SetAbort
    
    Err.Raise Err.Number, Err.Source, Err.Description

End Function


Public Sub UpdateDeleteForELID(ByVal lngELID As Long, ByVal varTxIDs As Variant)
'--------------------------------------------------------------------
'Date: 07/25/2002
'Author: Dave Richkun
'Description:  Physically deletes transaction rows where the TxID is included
'              in the varTxIDs array.  Any transactions associated with the
'              claim that are not included in the array, have the ELID foreign
'              key updated to NULL.  This method is called when a user is electing
'              to delete a claim.  The user is prompted to identify which
'              patient transactions are to be deleted, and which transactions they
'              would like to keep.
'Parameters: lngELID - ID of encounter record to which transactions are associated
'             varTxIDs - A single dimensional variant array containing the IDs of
'               those patient transactions they user wishes to delete.  If the user
'               does not want to delete any transactions this method still expects
'               the array to hold one element with a value of zero.
'Returns: Null
'--------------------------------------------------------------------

     Dim objPatTx As BillingDB.CPatientTXDB
     Dim intCtr As Integer
     
     On Error GoTo ErrTrap:
     
     Dim strTxIDs As String
     
    'Build a string of Tx IDs so that the list of IDs will syntactically fit within
    'an SQL 'IN' clause
    For intCtr = 0 To UBound(varTxIDs)
        strTxIDs = strTxIDs & varTxIDs(intCtr)
        If intCtr < UBound(varTxIDs) Then
            'Append a comma
            strTxIDs = strTxIDs & ","
        End If
    Next intCtr
     
    If strTxIDs > "" Then
        Set objPatTx = CreateObject("BillingDB.CPatientTXDB")
        Call objPatTx.UpdateDeleteForELID(lngELID, strTxIDs)
        Set objPatTx = Nothing
    End If
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Sub
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objPatTx = Nothing
    GetObjectContext.SetAbort
    
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub

Public Sub Update(ByVal lngTxID As Long, ByVal lngELID As Long, _
                ByVal lngPatientID As Long, ByVal lngProviderID As Long, _
                ByVal lngRPID As Long, ByVal lngTxTypeID As Long, _
                ByVal dtRcvDate As Date, ByVal dblAmount As Double, _
                ByVal strCheckNum As String, ByVal dtcheckDate As Date, _
                ByVal strTxNotes As String, ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 07/29/2002
'Author: Dave Richkun
'Description:  Updates a row in tblPatientTx table utilizing a stored procedure.
'Parameters:
'
'   *** TO DO: Define each parameter once procedure is finalized ****
'
'Returns: Null
'--------------------------------------------------------------------
    
    Dim objPatTx As BillingDB.CPatientTXDB
    Dim rst As ADODB.Recordset
    Dim lngID As Long
    Dim blnUpdatePostDateYN As Boolean
    Dim objTx As BillingDB.CPatientTXDB
    Dim objBR As BillingBz.CBRBz
    
    On Error GoTo ErrTrap:
    
    Set objPatTx = CreateObject("BillingDB.CPatientTXDB")
    Set rst = objPatTx.FetchByID(lngTxID)
    Set objPatTx = Nothing
    
    'if the transaction amount was not changed then donot update the posting date
    If (rst.Fields("fldAmount").Value <> dblAmount) Then
        blnUpdatePostDateYN = True
    Else
        blnUpdatePostDateYN = False
    End If
    
    Set objTx = CreateObject("BillingDB.CPatientTXDB")
    Call objTx.Update(lngTxID, lngELID, lngPatientID, lngProviderID, lngRPID, _
            lngTxTypeID, dtRcvDate, dblAmount, strCheckNum, dtcheckDate, _
            strTxNotes, blnUpdatePostDateYN, strUserName)
    Set objTx = Nothing
    
    'If the transaction is associated with a claim, update Billing Responsibility
    If lngELID > 0 Then
        Set objBR = CreateObject("BillingBz.CBRBz")
        Call objBR.UpdatePatientAmount(lngELID, strUserName)
        Set objBR = Nothing
    End If
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set objTx = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub


Public Sub Delete(ByVal lngTxID As Long, ByVal lngELID As Long, ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 07/29/2002
'Author: Dave Richkun
'Description:  Physically deletes a row in tblPatientTx table utilizing
'              a stored procedure.
'Parameters: lngTxID - ID of transaction to be deleted
'            lngELID - ID of encounter to which the transaction is associated
'Returns: Null
'--------------------------------------------------------------------
    
    Dim objTx As BillingDB.CPatientTXDB
    Dim objBR As BillingBz.CBRBz
    
    On Error GoTo ErrTrap:
    
    Set objTx = CreateObject("BillingDB.CPatientTXDB")
    Call objTx.Delete(lngTxID, strUserName)
    Set objTx = Nothing
    
    'If the transaction is associated with a claim, update Billing Responsibility
    If lngELID > 0 Then
        Set objBR = CreateObject("BillingBz.CBRBz")
        Call objBR.UpdatePatientAmount(lngELID, strUserName)
        Set objBR = Nothing
    End If
   
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set objTx = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description
    
End Sub


Public Sub UpdateSelfPayCharge(ByVal lngELID As Long, ByVal dblAmount As Double, _
            ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 08/06/2002
'Author: Dave Richkun
'Description:  Updates the patient charge record associated with a self-pay claim.
'Parameters: lngELID - ID of self-pay encounter
'            dblAmount - The amount of the updated charge
'            strUserName - The name of the user performing the update
'Returns: Null
'--------------------------------------------------------------------

    Dim objTx As BillingDB.CPatientTXDB
    
    On Error GoTo ErrTrap:
    
    Set objTx = CreateObject("BillingDB.CPatientTXDB")
    Call objTx.UpdateSelfPayCharge(lngELID, dblAmount, strUserName)
    Set objTx = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set objTx = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub



Public Sub Resubmit(ByVal lngOrigELID As Long, ByVal lngNewELID As Long)
'--------------------------------------------------------------------
'Date: 08/09/2002
'Author: Dave Richkun
'Description:  Re-assigns the Encounter Log ID foreign key from one set
'              of patient transactions to a new Encounter Log ID.  This
'              method is used when a claim is resubmitted and a new encounter
'              is created.  By reassigning the encounter log ID, the original
'              patient transactions are preserved when assigned to the new
'              encounter.
'Parameters:  lngOrigELID - ID of the original encounter - the ID of the encounter
'               being cancelled
'             lngNewELID - ID of the resubmitted encounter - the ID of the new
'               encounter that supercedes all previous encounters.
'Returns: Null
'--------------------------------------------------------------------
    
    Dim objTx As BillingDB.CPatientTXDB
    
    On Error GoTo ErrTrap:
    
    Set objTx = CreateObject("BillingDB.CPatientTXDB")
    Call objTx.Resubmit(lngOrigELID, lngNewELID)
    Set objTx = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set objTx = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description
    
End Sub


Public Sub EditFromSchedule(ByVal lngPatientTxID As Long, ByVal lngPatApptID As Long, _
        ByVal lngApptID As Long, ByVal lngApptStatusID As Long, ByVal dtApptDateTime As Date, _
        ByVal intDuration As Integer, ByVal strCPTCode As String, ByVal strNote As String, _
        ByVal lngClinicID As Long, ByVal strNoShowFlag As String, ByVal strCancelExplain As String, _
        ByVal lngPatientID As Long, ByVal lngProviderID As Long, ByVal lngRPID As Long, _
        ByVal strTxTypeCode As String, ByVal lngTxTypeID As Long, ByVal dtPostDate As Date, _
        ByVal dtRcvDate As Date, ByVal dblAmount As Double, ByVal strCheckNum As String, _
        ByVal dtcheckDate As Date, ByVal strCCTxID As String, ByVal strTxNotes As String, ByVal blnRecurYN As Boolean, _
        ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 08/23/2002
'Author: Dave Richkun
'Description:  Updates the status of a patient appointment and inserts a transaction
'              row into tblPatientTx table utilizing a stored procedure.
'              This procedure is called when the transaction originates from the
'              Schedule or Check-In page.  The encounter has not yet been created when
'              the transaction originates from the these pages.  The transaction will
'              be linked with the encounter via the PatApptID once it is created.
'Parameters:
'
'   *** TO DO: Define each parameter once procedure is finalized ****
'
'Returns: ID of the row inserted.
'--------------------------------------------------------------------

    Dim objPatAppt As ApptBZ.CPatApptBZ
    Dim objAppt As ApptDB.CApptDB
    Dim rst As ADODB.Recordset
    Dim rstTemp As ADODB.Recordset
    Dim lngNewPatApptID As Long
    Dim lngNewApptID As Long
    Dim intCtr As Integer
    Dim lngTempPatientID As Long

    Const APPT_TYPE_PATIENT = 1
    
    On Error GoTo ErrTrap:

    Set objAppt = CreateObject("ApptDB.CApptDB")
    Set objPatAppt = CreateObject("ApptBz.CPatApptBz")

    'If the appointment is a recurring appointment, we must make an instance of it
    If blnRecurYN = True Then
        'Insert a new patient appointment record for each patient in the appointment.
        Set rstTemp = objAppt.FetchPatientApptByID(lngApptID)
        
        'Create a new one-time patient appointment to replace this instance.
        lngNewApptID = objAppt.InsertSingle(lngProviderID, lngClinicID, APPT_TYPE_PATIENT, _
            dtApptDateTime, DateAdd("n", intDuration, dtApptDateTime), intDuration, _
            strCPTCode, "", strNote, strUserName)

        lngTempPatientID = -1
        'Ensure only one patient record is inserted; multiple rows will be returned if the
        'patient has more than one plan.
        For intCtr = 1 To rstTemp.RecordCount
            If lngTempPatientID <> rstTemp.Fields("fldPatientID").Value Then
                lngNewPatApptID = objPatAppt.Insert(lngNewApptID, rstTemp.Fields("fldPatientID").Value)
    
                If rstTemp.Fields("fldPatientID").Value = lngPatientID Then
                    lngPatApptID = lngNewPatApptID
                End If
                
                lngTempPatientID = rstTemp.Fields("fldPatientID").Value
            End If

            rstTemp.MoveNext
        Next
        Set rstTemp = Nothing
        
        'Insert a row into tblRecurApptExc so the recurring appointment and the new one-time
        'appointment do not appear as conflicts.
        Call objAppt.InsertRecurApptExc(lngApptID, DateValue(dtApptDateTime), strUserName)
    Else
        'Update the appointment - the user may have changed appointment details at the
        'same time the transaction was entered.
        Call objAppt.Update(lngApptID, lngProviderID, lngClinicID, APPT_TYPE_PATIENT, _
                dtApptDateTime, DateAdd("n", intDuration, dtApptDateTime), intDuration, _
                strCPTCode, "", strNote, strUserName)
    End If

    Set objAppt = Nothing
    
    'Update the appointment status
    Call objPatAppt.ChangeStatus(lngPatApptID, lngApptStatusID, strNoShowFlag, _
            0, strCancelExplain, strUserName)
    
    Set objPatAppt = Nothing

    'We delete the old transaction because the user may change a payment into a
    'charge i.e. change payment to No-Show and vice-versa.  We must ensure that
    'numbers remain accurate.
    If lngPatientTxID > 0 Then
        Call Delete(lngPatientTxID, -1, strUserName)
    End If
    
    'Insert/Update the patient transaction
    If dblAmount > 0 Or Not IsNull(strTxNotes) Then
        Call Insert(-1, lngPatientID, lngProviderID, lngRPID, strTxTypeCode, lngTxTypeID, _
                Date, dtRcvDate, dblAmount, strCheckNum, CDate(0), strCCTxID, strTxNotes, _
                lngPatApptID, strUserName)
    End If
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set objPatAppt = Nothing
    Set objAppt = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub


Public Function FetchByPatApptID(ByVal lngPatApptID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 08/27/2002
'Author: Dave Richkun
'Description:  Returns patient transaction information associated with a
'              patient appointment.
'Parameters: lngPatApptID - ID of patient appointment to which transactions are associated
'Returns: Recordset of patient transactions
'--------------------------------------------------------------------

     Dim objPatTx As BillingDB.CPatientTXDB
     
     On Error GoTo ErrTrap:
     
     Set objPatTx = CreateObject("BillingDB.CPatientTXDB")
     Set FetchByPatApptID = objPatTx.FetchByPatApptID(lngPatApptID)
     Set objPatTx = Nothing
    
    'Signal successful completion
     GetObjectContext.SetComplete

     Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objPatTx = Nothing
    GetObjectContext.SetAbort
    
    Err.Raise Err.Number, Err.Source, Err.Description

End Function


Public Function ResetPatientAppt(ByVal lngPatApptID As Long, ByVal aryTxs As Variant, _
                                 ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 08/27/2002
'Author: Dave Richkun
'Description:  Resets a patient appointment to a status of 'Attended' and
'              deletes any related Cancellation and No-Show patient transactions
'              the user selects.
'Parameters:  lngPatApptID - ID of the patient appointment
'             aryPatTxs - Single dimensional array of patient transaction IDs
'               selected by the user for deletion.
'             strUserName - User name of the user initiating the method
'Returns: Null
'--------------------------------------------------------------------
'Revision History:                                                  '
'--------------------------------------------------------------------

    Dim objAppt As ApptDB.CPatApptDB
    Dim intCtr As Integer

    On Error GoTo ErrTrap:
    
    'Reset the appointment status to Scheduled
    Set objAppt = CreateObject("ApptDB.CPatApptDB")
    Call objAppt.Reset(lngPatApptID, strUserName)
    Set objAppt = Nothing
    
    'Delete patient transactions
    If IsArray(aryTxs) Then
        For intCtr = 0 To UBound(aryTxs)
            If aryTxs(intCtr) > 0 Then
                Call Delete(aryTxs(intCtr), -1, strUserName)
            End If
        Next
    End If
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    Set objAppt = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Function


