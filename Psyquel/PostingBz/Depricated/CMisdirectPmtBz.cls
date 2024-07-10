
'--------------------------------------------------------------------
'Class Name: CMisdirectPmtBz
'Date: 06/11/2001
'Author: Eric Pena
'Description:  MTS business object designed to call methods associated
'              with the CMisdirectPmtDB class.
'--------------------------------------------------------------------
Option Explicit
Private Const CLASS_NAME As String = "CMisdirectPmtBz"
Private Const TABLE_NAME As String = "tblMisdirectedPayment"
Private Const CONST_POSTMP As Currency = 120

Public Function Insert(ByVal lngBRID As Long, ByVal strCheckNum As String, ByVal dteCheckDate As Date, ByVal dteClearDate As Date, ByVal curCheckAmount As Currency, ByVal curPostAmount As Currency, _
                        ByVal curAmtDisallow As Currency, ByVal curAmtPatient As Currency, ByVal blnMultiCheck As Boolean, ByVal strAddress1 As String, _
                        ByVal strAddress2 As String, ByVal strCity As String, ByVal strState As String, ByVal strZip As String, ByVal strContactName As String, _
                        ByVal strContactPhone As String, ByVal strContactExt As String, ByVal strNotes As String, ByVal strUsername As String) As Long
'--------------------------------------------------------------------
'Date: 06/11/2001
'Author: Eric Pena
'Description:  Inserts a row into the tblMisdirectedPayment table utilizing a stored procedure.
'Parameters: Each parameter identifies the column value that will be  inserted.
'Returns: ID (Primary Key) of the row inserted.
'--------------------------------------------------------------------
    Dim objMP As PostingDB.CMisdirectPmtDB
    Dim objBR As BillingBz.CBRBz
    
    Dim strErrMessage As String
    On Error GoTo ErrTrap:
    
    If Not VerifyData(lngBRID, curCheckAmount, strErrMessage) Then GoTo ErrTrap
    Set objMP = CreateObject("PostingDB.CMisdirectPmtDB")
    
    Insert = objMP.Insert(lngBRID, Trim(strCheckNum), dteCheckDate, dteClearDate, curCheckAmount, curPostAmount, curAmtDisallow, curAmtPatient, blnMultiCheck, Trim(strAddress1), Trim(strAddress2), Trim(strCity), _
                         Trim(strState), NumbersOnly(strZip), Trim(strContactName), NumbersOnly(strContactPhone), NumbersOnly(strContactExt), Trim(strNotes), strUsername)
    
    Set objMP = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    If strErrMessage = "" Then strErrMessage = Err.Description
    Set objMP = Nothing
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, strErrMessage
    End If
End Function

Public Sub Update(ByVal lngMisPmtID As Long, ByVal strCheckNum As String, ByVal dteCheckDate As Date, ByVal dteClearDate As Date, ByVal curCheckAmount As Currency, ByVal curPostAmount As Currency, _
                        ByVal curAmtDisallow As Currency, ByVal curAmtPatient As Currency, ByVal blnMultiCheck As Boolean, ByVal strAddress1 As String, _
                        ByVal strAddress2 As String, ByVal strCity As String, ByVal strState As String, ByVal strZip As String, ByVal strContactName As String, _
                        ByVal strContactPhone As String, ByVal strContactExt As String, ByVal strNotes As String, ByVal strUsername As String)
'--------------------------------------------------------------------
'Date: 06/11/2001
'Author: Eric Pena
'Description:  Updates a row in the tblMisdirectedPayment table utilizing a stored procedure.
'Parameters: Each parameter identifies the column value that will be inserted.
'Returns: Null
'--------------------------------------------------------------------
    Dim objMP As PostingDB.CMisdirectPmtDB
    Dim strErrMessage As String

    On Error GoTo ErrTrap:
    
    If Not VerifyData(1, curCheckAmount, strErrMessage) Then GoTo ErrTrap
    
    Set objMP = CreateObject("PostingDB.CMisdirectPmtDB")
    
    If objMP.isProcessed(lngMisPmtID) Then
        strErrMessage = "This entry has already been processed.  Cannot update."
        GoTo ErrTrap
    End If
    
    objMP.Update lngMisPmtID, Trim(strCheckNum), dteCheckDate, dteClearDate, curCheckAmount, curPostAmount, curAmtDisallow, curAmtPatient, blnMultiCheck, Trim(strAddress1), Trim(strAddress2), Trim(strCity), _
                         Trim(strState), NumbersOnly(strZip), Trim(strContactName), NumbersOnly(strContactPhone), NumbersOnly(strContactExt), Trim(strNotes), strUsername
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objMP = Nothing
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    Set objMP = Nothing
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Sub

Public Sub Deleted(ByVal lngMisPmtID As Long, ByVal blnIsDeleted As Boolean, ByVal strDeletedBy As String)
'--------------------------------------------------------------------
'Date: 06/11/2001
'Author: Eric Pena
'Description:  Physically deletes a row in the tblMisdirectedPayment table if it has not yet been processed.
'Parameters: lngID - ID of the row in the table whose value will be updated.
'            strUserName - Login name of the user responsible for marking the row as deleted.
'Returns: Null
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------
    Dim objMP As PostingDB.CMisdirectPmtDB
    Dim strError As String
    
    On Error GoTo ErrTrap:
    
    strError = ""
    
    
    Set objMP = CreateObject("PostingDB.CMisdirectPmtDB")
    
    If objMP.isProcessed(lngMisPmtID) Then
        strError = "This entry has already been processed.  Cannot delete."
        GoTo ErrTrap
    End If
    
    Call objMP.Deleted(lngMisPmtID, blnIsDeleted, strDeletedBy)

    'Signal successful completion
    GetObjectContext.SetComplete

    Set objMP = Nothing

    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objMP = Nothing
    GetObjectContext.SetAbort
    If strError = "" Then strError = Err.Description
    Err.Raise Err.Number, Err.Source, strError
End Sub

Private Function VerifyData(ByVal lngBRID As Long, ByVal curAmount As Currency, ByRef strErrMessage As String) As Boolean
'--------------------------------------------------------------------
'Date: 06/11/2001
'Author: Eric Pena
'Description:  Verifies all required data has been provided by the user.
'Parameters:  The values to be checked.
'Returns: Boolean value identifying if all criteria has been satisfied.
'--------------------------------------------------------------------
    If lngBRID < 1 Then
        strErrMessage = "A Payer is needed."
        VerifyData = False
        Exit Function
    End If
    
    If curAmount <= 0 Then
        strErrMessage = "A positive check amount is needed."
        VerifyData = False
        Exit Function
    End If
    
    'If we get here, all is well...
    VerifyData = True

End Function

Public Function FetchByUser(ByVal lngUserID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 06/11/2001
'Author: Eric Pena
'Description:  Returns rows from the tblMisdirectedPayment table utilizing a stored procedure
'Parameters: lngUserID - user for which records will be returned
'Returns: ADORst
'--------------------------------------------------------------------
     
     Dim objMP As PostingDB.CMisdirectPmtDB
     
     On Error GoTo ErrTrap:
     
     
     Set objMP = CreateObject("PostingDB.CMisdirectPmtDB")
     Set FetchByUser = objMP.FetchByUser(lngUserID)
    
    'Signal successful completion
     GetObjectContext.SetComplete
     Set objMP = Nothing
     Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objMP = Nothing
     GetObjectContext.SetAbort
     Err.Raise Err.Number, Err.Source, Err.Description
End Function

Public Function FetchByEL(ByVal lngELID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 06/11/2001
'Author: Eric Pena
'Description:  Returns rows from the tblMisdirectedPayment table utilizing a stored procedure
'Parameters: lngELID - PK in tblEncounterLog for which records will be returned
'Returns: ADORst
'--------------------------------------------------------------------
     
     Dim objMP As PostingDB.CMisdirectPmtDB
     
     On Error GoTo ErrTrap:
     
     
     Set objMP = CreateObject("PostingDB.CMisdirectPmtDB")
     Set FetchByEL = objMP.FetchByEL(lngELID)
    
    'Signal successful completion
     GetObjectContext.SetComplete
     Set objMP = Nothing
     Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objMP = Nothing
     GetObjectContext.SetAbort
     Err.Raise Err.Number, Err.Source, Err.Description
End Function

Public Function FetchByBR(ByVal lngBRID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 06/11/2001
'Author: Eric Pena
'Description:  Returns rows from the tblMisdirectedPayment table utilizing a stored procedure
'Parameters: lngBRID - PK in tblBillingResponsibility for which records will be returned
'Returns: ADORst
'--------------------------------------------------------------------
     
     Dim objMP As PostingDB.CMisdirectPmtDB
     
     On Error GoTo ErrTrap:
     
     
     Set objMP = CreateObject("PostingDB.CMisdirectPmtDB")
     Set FetchByBR = objMP.FetchByBR(lngBRID)
    
    'Signal successful completion
     GetObjectContext.SetComplete
     Set objMP = Nothing
     Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objMP = Nothing
     GetObjectContext.SetAbort
     Err.Raise Err.Number, Err.Source, Err.Description
End Function

Public Function FetchByID(ByVal lngMPID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 06/11/2001
'Author: Eric Pena
'Description:  Returns rows from the tblMisdirectedPayment table utilizing a stored procedure
'Parameters: lngMPID - PK in tblMisdirectedPayment for which records will be returned
'Returns: ADORst
'--------------------------------------------------------------------
     
     Dim objMP As PostingDB.CMisdirectPmtDB
     
     On Error GoTo ErrTrap:
     
     
     Set objMP = CreateObject("PostingDB.CMisdirectPmtDB")
     Set FetchByID = objMP.FetchByID(lngMPID)
    
    'Signal successful completion
     GetObjectContext.SetComplete
     Set objMP = Nothing
     Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objMP = Nothing
     GetObjectContext.SetAbort
     Err.Raise Err.Number, Err.Source, Err.Description
 End Function

Public Sub SetAction(ByVal lngMisPmtID As Long, ByVal strActionType As String, ByVal strUsername As String)
'--------------------------------------------------------------------
'Date: 06/11/2001
'Author: Eric Pena
'Description:  Updates a row in the tblMisdirectedPayment table utilizing a stored procedure.
'Parameters: Each parameter identifies the column value that will be inserted.
'Returns: Null
'--------------------------------------------------------------------
    Dim objMP As PostingDB.CMisdirectPmtDB
    Dim strErrMessage As String

    On Error GoTo ErrTrap:
    
    Set objMP = CreateObject("PostingDB.CMisdirectPmtDB")
    
    If objMP.isProcessed(lngMisPmtID) Then
        strErrMessage = "This entry has already been processed.  Cannot update."
        GoTo ErrTrap
    End If
    
    Call objMP.SetAction(lngMisPmtID, strActionType, strUsername)
    Set objMP = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    Set objMP = Nothing
    
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Sub

Private Sub SetProcessed(ByVal lngMisPmtID As Long, ByVal strUsername As String)
'--------------------------------------------------------------------
'Date: 06/11/2001
'Author: Eric Pena
'Description:  Updates a row in the tblMisdirectedPayment table utilizing a stored procedure.
'Parameters: Each parameter identifies the column value that will be inserted.
'Returns: Null
'--------------------------------------------------------------------
    Dim objMP As PostingDB.CMisdirectPmtDB
    Dim strErrMessage As String

    On Error GoTo ErrTrap:
    
    Set objMP = CreateObject("PostingDB.CMisdirectPmtDB")
    
    If objMP.isProcessed(lngMisPmtID) Then
        strErrMessage = "This entry has already been processed."
        GoTo ErrTrap
    End If
    
    objMP.SetProcessed lngMisPmtID, strUsername
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objMP = Nothing
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    Set objMP = Nothing
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Sub

Public Function FetchForProcessing() As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 06/11/2001
'Author: Eric Pena
'Description:  Returns rows ready for processing from the tblMisdirectedPayment table utilizing a stored procedure
'Parameters:
'Returns: ADORst
'--------------------------------------------------------------------
     Dim objMP As PostingDB.CMisdirectPmtDB
     
     On Error GoTo ErrTrap:
     
     
     Set objMP = CreateObject("PostingDB.CMisdirectPmtDB")
     Set FetchForProcessing = objMP.FetchForProcessing()
    
    'Signal successful completion
     GetObjectContext.SetComplete
     Set objMP = Nothing
     Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objMP = Nothing
     GetObjectContext.SetAbort
     Err.Raise Err.Number, Err.Source, Err.Description
 End Function

Public Function FetchForAutoAction() As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 06/11/2001
'Author: Eric Pena
'Description:  Returns rows ready for processing from the tblMisdirectedPayment table utilizing a stored procedure
'Parameters:
'Returns: ADORst
'--------------------------------------------------------------------
     Dim objMP As PostingDB.CMisdirectPmtDB
     
     On Error GoTo ErrTrap:
     
     
     Set objMP = CreateObject("PostingDB.CMisdirectPmtDB")
     Set FetchForAutoAction = objMP.FetchForAutoAction()
    
    'Signal successful completion
     GetObjectContext.SetComplete
     Set objMP = Nothing
     Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objMP = Nothing
     GetObjectContext.SetAbort
     Err.Raise Err.Number, Err.Source, Err.Description
 End Function
Public Sub Process(ByVal lngMisPmtID As Long, ByVal strUsername As String)
'--------------------------------------------------------------------
'Date: 06/11/2001
'Author: Eric Pena
'Description:  Updates a row in the tblMisdirectedPayment table utilizing a stored procedure.
'Parameters: Each parameter identifies the column value that will be inserted.
'Returns: Null
'--------------------------------------------------------------------
    Dim objMP As PostingDB.CMisdirectPmtDB
    Dim objBR As BillingBz.CBRBz
    Dim objTx As PostingBz.CTxBz
    Dim objCheck As PostingBz.CCheckBz
    Dim rstMP As ADODB.Recordset
    Dim rstBR As ADODB.Recordset
    Dim rstCheck As ADODB.Recordset
    Dim lngNextBRID As Long
    Dim lngNextRPID As Long
    Dim lngNextPlanID As Long
    Dim lngPlanID As Long
    Dim lngRPID As Long
    Dim curAmtToMove As Currency
    Dim curBalance As Currency
    Dim curTemp As Long
    Dim lngCheckID As Long
    Dim lngNextCPCID As Long

    Dim strErrMessage As String

    On Error GoTo ErrTrap:

    Set objMP = CreateObject("PostingDB.CMisdirectPmtDB")

    Set rstMP = objMP.FetchByID(lngMisPmtID)
    
    If rstMP!fldprocessedyn = "Y" Then GoTo Cleanup
    
    Set objBR = CreateObject("BillingBz.CBRBz")
    Set rstBR = objBR.Fetch(rstMP!fldEncounterLogID)
    Set objBR = Nothing

    lngNextBRID = rstBR!fldBRID
    lngNextRPID = rstBR!fldRPID
    lngNextPlanID = -1
    lngNextCPCID = -1

    rstBR.Find "fldBRID = " & rstMP!fldBRID
    If rstBR.EOF Then
        strErrMessage = "Error processing check!"
        GoTo ErrTrap
    End If

    lngRPID = rstBR!fldRPID
    lngPlanID = rstBR!fldPlanID
    
    rstBR.MoveNext
    If Not rstBR.EOF Then
        lngNextBRID = rstBR!fldBRID
        lngNextRPID = rstBR!fldRPID
        lngNextPlanID = rstBR!fldPlanID
        lngNextCPCID = rstBR!fldcPcID
    End If
    rstBR.MovePrevious
    
    Set objCheck = CreateObject("PostingBz.CCheckBz")
    Set rstCheck = objCheck.FetchByCriteria(rstMP!fldInsuranceID, Trim(rstMP!fldCheckNum), rstMP!fldCheckDate, "", -1, -1, False)
    If rstCheck.EOF Then
        'insert new check
        If rstMP!fldMultiCheckYN = "Y" Then
            curTemp = rstMP!fldCheckAmount
        Else
            curTemp = rstMP!fldAmount
        End If
        lngCheckID = objCheck.Insert(rstMP!fldInsuranceID, rstMP!fldCheckNum, "000", rstMP!fldCheckDate, rstMP!fldReceiveDate, curTemp, rstMP!fldAddedBy, rstBR!fldProviderID, -1)
    Else
        'use existing check
        lngCheckID = rstCheck!fldCheckID
        
        'make sure there's enuf $$ on the check
        If CCur(rstCheck!fldTotalAmount - rstCheck!fldAmtApplied) < rstMP!fldAmount Then
            strErrMessage = "Cannot post MP.  Check #" & rstCheck!fldCheckNum & " is " & Format(Abs(rstMP!fldAmount - rstCheck!fldTotalAmount - rstCheck!fldAmtApplied), "currency") & " short."
            GoTo ErrTrap
        End If
    End If
    Set rstCheck = Nothing
    Set objCheck = Nothing
    
    
    curTemp = CCur(rstMP!fldAmount + rstMP!fldAmtDisallow)
    
    Set objTx = CreateObject("PostingBz.CTxBz")
    If curTemp <= rstBR!fldBalance Then
        'can just post
        curBalance = rstBR!fldBalance - curTemp
        
        objTx.Insert rstMP!fldBRID, rstBR!fldInsuranceID, rstBR!fldcPcID, rstBR!fldPlanID, rstBR!fldRPID, rstBR!fldEncounterLogID, rstBR!fldEncDetailID, rstBR!fldPatientID, _
                    lngNextRPID, lngNextPlanID, lngNextBRID, lngNextCPCID, IfNull(rstMP!fldActionBy, strUsername), CONST_POSTMP, "P", rstBR!fldCPTRecordID, rstMP!fldAmount, rstMP!fldAmtDisallow, _
                    curBalance, rstMP!fldCheckNum, rstMP!fldCheckDate, lngCheckID, IfNull(rstMP!fldNotes, ""), True, True
    Else
        'not enuf BR.  transfer from others (last payer -> first)
        curBalance = 0
        curAmtToMove = CCur(curTemp - rstBR!fldBalance)
        rstBR.Filter = "fldBRID <> " & rstMP!fldBRID
        If Not rstBR.EOF Then rstBR.MoveLast

        While Not rstBR.BOF And curAmtToMove > 0
            If rstBR!fldBalance >= curAmtToMove Then
                'can move all
                objTx.TransferResponsibility rstBR!fldBRID, rstBR!fldPlanID, rstBR!fldRPID, rstMP!fldBRID, lngRPID, lngPlanID, curAmtToMove, rstBR!fldEncounterLogID, rstBR!fldPatientID, -1, -1, strUsername
                curAmtToMove = 0
            ElseIf rstBR!fldBalance > 0 Then
                'move what can be moved
                objTx.TransferResponsibility rstBR!fldBRID, rstBR!fldPlanID, rstBR!fldRPID, rstMP!fldBRID, lngRPID, lngPlanID, rstBR!fldBalance, rstBR!fldEncounterLogID, rstBR!fldPatientID, -1, -1, strUsername
                curAmtToMove = CCur(curAmtToMove - rstBR!fldBalance)
            End If
            rstBR.MovePrevious
        Wend

        If curAmtToMove > 0 Then
            'still need to move BR - take it from the patient
            If rstBR.BOF Then
                If Not rstBR.EOF Then rstBR.MoveNext
            Else
                rstBR.MoveFirst
            End If
            objTx.TransferResponsibility rstBR!fldBRID, rstBR!fldPlanID, rstBR!fldRPID, rstMP!fldBRID, lngRPID, lngPlanID, curAmtToMove, rstBR!fldEncounterLogID, rstBR!fldPatientID, 1, 246, strUsername
        End If

        If Not rstBR.BOF Then
            rstBR.MoveFirst
        End If
        
        rstBR.Filter = ""
        rstBR.Find "fldBRID = " & rstMP!fldBRID

        objTx.Insert rstMP!fldBRID, rstBR!fldInsuranceID, rstBR!fldcPcID, rstBR!fldPlanID, rstBR!fldRPID, rstBR!fldEncounterLogID, rstBR!fldEncDetailID, rstBR!fldPatientID, _
                    lngNextRPID, lngNextPlanID, lngNextBRID, lngNextCPCID, IfNull(rstMP!fldActionBy, strUsername), CONST_POSTMP, "P", rstBR!fldCPTRecordID, rstMP!fldAmount, rstMP!fldAmtDisallow, _
                    curBalance, rstMP!fldCheckNum, rstMP!fldCheckDate, lngCheckID, IfNull(rstMP!fldNotes, ""), True, True
    End If

    objMP.SetProcessed rstMP!fldMisPmtID, strUsername

    'Signal successful completion
Cleanup:
    GetObjectContext.SetComplete
    Set objMP = Nothing
    Set objTx = Nothing
    Set rstMP = Nothing
    Set rstBR = Nothing
    Exit Sub

ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    Set objMP = Nothing
    Set objBR = Nothing
    Set objTx = Nothing
    Set rstMP = Nothing
    Set rstBR = Nothing
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Sub



Public Function FetchRejections() As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 04/30/2003
'Author: Dave Richkun
'Description:  Returns rejected MisDirected payments
'Parameters:
'Returns: ADO Recordset
'--------------------------------------------------------------------

    Dim objMP As PostingDB.CMisdirectPmtDB
    
    On Error GoTo ErrTrap:
     
    Set objMP = CreateObject("PostingDB.CMisdirectPmtDB")
    Set FetchRejections = objMP.FetchRejections()
    Set objMP = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Function

ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objMP = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

