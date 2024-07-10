VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 2  'RequiresTransaction
END
Attribute VB_Name = "CPatientTXDB"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'----------------------------------------------------------------------------
'Class Name: CPatientTxDB
'Date: 07/09/2002
'Author: Dave Richkun
'Description:  COM+ object designed to host methods associated with patient
'              transactions.
'----------------------------------------------------------------------------
Option Explicit

Private Const CLASS_NAME As String = "CPatientTxDB"

Public Function Insert(ByVal lngELID As Long, ByVal lngPatientID As Long, _
        ByVal lngProviderID As Long, ByVal lngRPID As Long, ByVal strTxTypeCode As String, _
        ByVal lngTxTypeID As Long, ByVal dtPostDate As Date, ByVal dtRcvDate As Date, _
        ByVal dblAmount As Double, ByVal strCheckNum As String, ByVal dtCheckDate As Date, _
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
    
    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command
    
    On Error GoTo ErrTrap:
    
    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    
    'Create the parameter objects
    With cmdSQL
        .CommandText = "uspInsPatientTx"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@TxID", adInteger, adParamReturnValue)
        .Parameters.Append .CreateParameter("@ELID", adInteger, adParamInput, , IIf(lngELID > 0, lngELID, Null))
        .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID)
        .Parameters.Append .CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID)
        .Parameters.Append .CreateParameter("@RPID", adInteger, adParamInput, , lngRPID)
        .Parameters.Append .CreateParameter("@TxTypeCode", adVarChar, adParamInput, 1, strTxTypeCode)
        .Parameters.Append .CreateParameter("@TxTypeID", adInteger, adParamInput, , lngTxTypeID)
        .Parameters.Append .CreateParameter("@PostDate", adDBTimeStamp, adParamInput, , dtPostDate)
        .Parameters.Append .CreateParameter("@RcvDate", adDBTimeStamp, adParamInput, , dtRcvDate)
        .Parameters.Append .CreateParameter("@Amount", adDouble, adParamInput, , dblAmount)
        .Parameters.Append .CreateParameter("@CheckNum", adVarChar, adParamInput, 30, strCheckNum)
        .Parameters.Append .CreateParameter("@CheckDate", adDBTimeStamp, adParamInput, , IIf(dtCheckDate = 0, Null, dtCheckDate))
        .Parameters.Append .CreateParameter("@CCTxID", adVarChar, adParamInput, 60, strCCTxID)
        .Parameters.Append .CreateParameter("@TxNotes", adVarChar, adParamInput, 255, strTxNotes)
        .Parameters.Append .CreateParameter("@LockedYN", adVarChar, adParamInput, 1, "N")
        .Parameters.Append .CreateParameter("@PatApptID", adInteger, adParamInput, , IIf(lngPatApptID > 0, lngPatApptID, Null))
        .Parameters.Append .CreateParameter("@PostedBy", adVarChar, adParamInput, 30, strUserName)
        .Parameters.Append .CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open(CONST_PSYQUEL_CNN)
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute , , adExecuteNoRecords

    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If
    
    Insert = cmdSQL.Parameters("@TxID").Value
    
    'Close the connection
    cnnSQL.Close
    
    'Free all resources
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing

    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Call RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, Err.Description)
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    GetObjectContext.SetAbort

End Function


Public Function Fetch(ByVal lngPatientID As Long, ByVal lngProviderID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 09/06/2001
'Author: Eric Pena
'Description:  Returns rows from the tblNonPostable table utilizing a stored procedure
'Parameters: PK for row in tblNonPostable that will be fetched.
'Returns: ADORst
'--------------------------------------------------------------------
     Dim rs As ADODB.Recordset
     Dim cmd As ADODB.Command
     Dim cnn As ADODB.Connection
     
     On Error GoTo ErrTrap:
     
     Set rs = New ADODB.Recordset
     Set cmd = New ADODB.Command
     Set cnn = New ADODB.Connection
     cnn.Open (CONST_PSYQUEL_CNN)
     Set cmd.ActiveConnection = cnn
     
    'Create the parameter objects
     With cmd
         .CommandText = "uspSelPatientTx"
         .CommandType = adCmdStoredProc
         .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID)
         .Parameters.Append .CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID)
     End With
    
    'Execute the stored procedure
     rs.CursorLocation = adUseClient
     rs.Open cmd, , adOpenForwardOnly, adLockReadOnly
    
     Set Fetch = rs
    
    'Disconnect the recordset
     Set cmd.ActiveConnection = Nothing
     Set cmd = Nothing
     Set rs.ActiveConnection = Nothing
     Set cnn = Nothing
     
    'Signal successful completion
     GetObjectContext.SetComplete
    
     Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
     Set cmd = Nothing
     Set rs = Nothing
     Set cnn = Nothing
     GetObjectContext.SetAbort
    Call RaiseError(Err, -1, CLASS_NAME, "SQL Server Error")
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
     Dim rst As ADODB.Recordset
     Dim cmd As ADODB.Command
     Dim cnn As ADODB.Connection
     
     On Error GoTo ErrTrap:
     
     Set rst = New ADODB.Recordset
     Set cmd = New ADODB.Command
     Set cnn = New ADODB.Connection
     
     cnn.Open (CONST_PSYQUEL_CNN)
     Set cmd.ActiveConnection = cnn

     With cmd
    'Create the parameter objects
         .CommandText = "uspSelPatientTxByID"
         .CommandType = adCmdStoredProc
         .Parameters.Append .CreateParameter("@TxID", adInteger, adParamInput, , lngTxID)
     End With
    
    'Execute the stored procedure
     rst.CursorLocation = adUseClient
     rst.Open cmd, , adOpenForwardOnly, adLockReadOnly
    
     Set FetchByID = rst
    
    'Disconnect the recordset
     Set cmd.ActiveConnection = Nothing
     Set cmd = Nothing
     Set rst.ActiveConnection = Nothing
     Set cnn = Nothing
     
    'Signal successful completion
     GetObjectContext.SetComplete
    
     Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
     Set cmd = Nothing
     Set rst = Nothing
     Set cnn = Nothing
     GetObjectContext.SetAbort
     
     Call Err.Raise(Err.Number, Err.Source, Err.Description)
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
     Dim rst As ADODB.Recordset
     Dim cmd As ADODB.Command
     Dim cnn As ADODB.Connection
     
     On Error GoTo ErrTrap:
     
     Set rst = New ADODB.Recordset
     Set cmd = New ADODB.Command
     Set cnn = New ADODB.Connection
     
     cnn.Open (CONST_PSYQUEL_CNN)
     Set cmd.ActiveConnection = cnn

     With cmd
    'Create the parameter objects
         .CommandText = "uspSelPatientTxByELID"
         .CommandType = adCmdStoredProc
         .Parameters.Append .CreateParameter("@ELID", adInteger, adParamInput, , lngELID)
     End With
    
    'Execute the stored procedure
     rst.CursorLocation = adUseClient
     rst.Open cmd, , adOpenForwardOnly, adLockReadOnly
    
     Set FetchByELID = rst
    
    'Disconnect the recordset
     Set cmd.ActiveConnection = Nothing
     Set cmd = Nothing
     Set rst.ActiveConnection = Nothing
     Set cnn = Nothing
     
    'Signal successful completion
     GetObjectContext.SetComplete
    
     Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
     Set cmd = Nothing
     Set rst = Nothing
     Set cnn = Nothing
     GetObjectContext.SetAbort
     
     Call Err.Raise(Err.Number, Err.Source, Err.Description)
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
     Dim rst As ADODB.Recordset
     Dim cmd As ADODB.Command
     Dim cnn As ADODB.Connection
     
     On Error GoTo ErrTrap:
     
     Set rst = New ADODB.Recordset
     Set cmd = New ADODB.Command
     Set cnn = New ADODB.Connection
     
     cnn.Open (CONST_PSYQUEL_CNN)
     Set cmd.ActiveConnection = cnn

     With cmd
    'Create the parameter objects
         .CommandText = "uspSelPatTxByPatUser"
         .CommandType = adCmdStoredProc
         .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID)
         .Parameters.Append .CreateParameter("@UserID", adInteger, adParamInput, , lngUserID)
     End With
    
    'Execute the stored procedure
     rst.CursorLocation = adUseClient
     rst.Open cmd, , adOpenForwardOnly, adLockReadOnly
    
     Set FetchByPatientUser = rst
    
    'Disconnect the recordset
     Set cmd.ActiveConnection = Nothing
     Set cmd = Nothing
     Set rst.ActiveConnection = Nothing
     Set cnn = Nothing
     
    'Signal successful completion
     GetObjectContext.SetComplete
    
     Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
     Set cmd = Nothing
     Set rst = Nothing
     Set cnn = Nothing
     GetObjectContext.SetAbort
     
     Call Err.Raise(Err.Number, Err.Source, Err.Description)
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
     Dim rst As ADODB.Recordset
     Dim cmd As ADODB.Command
     Dim cnn As ADODB.Connection
     
     On Error GoTo ErrTrap:
     
     Set rst = New ADODB.Recordset
     Set cmd = New ADODB.Command
     Set cnn = New ADODB.Connection
     
     cnn.Open (CONST_PSYQUEL_CNN)
     Set cmd.ActiveConnection = cnn

     With cmd
    'Create the parameter objects
         .CommandText = "uspSelPatTxByProvUser"
         .CommandType = adCmdStoredProc
         .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID)
         .Parameters.Append .CreateParameter("@UserID", adInteger, adParamInput, , lngUserID)
     End With
    
    'Execute the stored procedure
     rst.CursorLocation = adUseClient
     rst.Open cmd, , adOpenForwardOnly, adLockReadOnly
    
     Set FetchByPatientProvUser = rst
    
    'Disconnect the recordset
     Set cmd.ActiveConnection = Nothing
     Set cmd = Nothing
     Set rst.ActiveConnection = Nothing
     Set cnn = Nothing
     
    'Signal successful completion
     GetObjectContext.SetComplete
    
     Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
     Set cmd = Nothing
     Set rst = Nothing
     Set cnn = Nothing
     GetObjectContext.SetAbort
     
     Call Err.Raise(Err.Number, Err.Source, Err.Description)
 End Function


Public Sub UpdateDeleteForELID(ByVal lngELID As Long, ByVal strTxIDs As String)
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
'               those patient transactions they user wishes to delete.
'Returns: Null
'--------------------------------------------------------------------

    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command
    
    On Error GoTo ErrTrap:
    
    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    
    'Create the parameter objects
    With cmdSQL
        .CommandText = "uspUpdDelPatientTxForELID"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@ELID", adInteger, adParamInput, , lngELID)
        .Parameters.Append .CreateParameter("@TxIDs", adVarChar, adParamInput, 50, strTxIDs)
        .Parameters.Append .CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open(CONST_PSYQUEL_CNN)
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute , , adExecuteNoRecords

    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If
    
    'Close the connection
    cnnSQL.Close
    
    'Free all resources
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing

    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Call RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, Err.Description)
    
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    GetObjectContext.SetAbort

End Sub

Public Sub Update(ByVal lngTxID As Long, ByVal lngELID As Long, _
                ByVal lngPatientID As Long, ByVal lngProviderID As Long, _
                ByVal lngRPID As Long, ByVal lngTxTypeID As Long, _
                ByVal dtRcvDate As Date, ByVal dblAmount As Double, _
                ByVal strCheckNum As String, ByVal dtCheckDate As Date, _
                ByVal strTxNotes As String, ByVal blnUpdatePostDateYN As Boolean, _
                ByVal strUserName As String)
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
    
    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command
    
    On Error GoTo ErrTrap:
    
    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    
    'Create the parameter objects
    With cmdSQL
        .CommandText = "uspUpdPatientTx"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@TxID", adInteger, adParamInput, , lngTxID)
        .Parameters.Append .CreateParameter("@ELID", adInteger, adParamInput, , IIf(lngELID > 0, lngELID, Null))
        .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID)
        .Parameters.Append .CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID)
        .Parameters.Append .CreateParameter("@RPID", adInteger, adParamInput, , lngRPID)
        .Parameters.Append .CreateParameter("@TxTypeID", adInteger, adParamInput, , lngTxTypeID)
        .Parameters.Append .CreateParameter("@RcvDate", adDBTimeStamp, adParamInput, , dtRcvDate)
        .Parameters.Append .CreateParameter("@Amount", adDouble, adParamInput, , dblAmount)
        .Parameters.Append .CreateParameter("@CheckNum", adVarChar, adParamInput, 30, strCheckNum)
        .Parameters.Append .CreateParameter("@CheckDate", adDBTimeStamp, adParamInput, , IIf(dtCheckDate = 0, Null, dtCheckDate))
        .Parameters.Append .CreateParameter("@TxNotes", adVarChar, adParamInput, 255, strTxNotes)
        .Parameters.Append .CreateParameter("@UpdatePostDateYN", adVarChar, adParamInput, 1, IIf(blnUpdatePostDateYN, "Y", "N"))
        .Parameters.Append .CreateParameter("@PostedBy", adVarChar, adParamInput, 30, strUserName)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open(CONST_PSYQUEL_CNN)
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute , , adExecuteNoRecords

    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If
    
    'Close the connection
    cnnSQL.Close
    
    'Free all resources
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing

    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Call RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, Err.Description)
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    GetObjectContext.SetAbort

End Sub


Public Sub Delete(ByVal lngTxID As Long, ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 07/29/2002
'Author: Dave Richkun
'Description:  Physically deletes a row in tblPatientTx table utilizing
'              a stored procedure.
'Parameters: lngTxID - ID of transaction to be deleted
'Returns: Null
'--------------------------------------------------------------------
    
    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command
    
    On Error GoTo ErrTrap:
    
    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    
    'Create the parameter objects
    With cmdSQL
        .CommandText = "uspDelPatientTx"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@TxID", adInteger, adParamInput, , lngTxID)
        .Parameters.Append .CreateParameter("@DeletedBy", adVarChar, adParamInput, 30, strUserName)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open(CONST_PSYQUEL_CNN)
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute , , adExecuteNoRecords

    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If
    
    'Close the connection
    cnnSQL.Close
    
    'Free all resources
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing

    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Call RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, Err.Description)
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    GetObjectContext.SetAbort

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
    
    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command
    
    On Error GoTo ErrTrap:
    
    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    
    'Create the parameter objects
    With cmdSQL
        .CommandText = "uspUpdPatTxSelfPayChg"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@ELID", adInteger, adParamInput, , lngELID)
        .Parameters.Append .CreateParameter("@Amount", adDouble, adParamInput, , dblAmount)
        .Parameters.Append .CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open(CONST_PSYQUEL_CNN)
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute , , adExecuteNoRecords

    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap:
    End If
    
    'Close the connection
    cnnSQL.Close
    
    'Free all resources
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing

    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Call RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME, Err.Description)
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    GetObjectContext.SetAbort

End Sub


Public Sub Resubmit(ByVal lngOrigELID As Long, ByVal lngNewELID As Long)
'--------------------------------------------------------------------
'Date: 08/09/2002
'Author: Dave Richkun
'Description:  Reassigns the Encounter Log ID foreign key from one set
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
    
    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command
    
    On Error GoTo ErrTrap:
    
    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    
    'Create the parameter objects
    With cmdSQL
        .CommandText = "uspUpdPatTxResubmit"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@OrigELID", adInteger, adParamInput, , lngOrigELID)
        .Parameters.Append .CreateParameter("@NewELID", adInteger, adParamInput, , lngNewELID)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open(CONST_PSYQUEL_CNN)
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute , , adExecuteNoRecords

    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If
    
    'Close the connection
    cnnSQL.Close
    
    'Free all resources
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing

    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Call RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, Err.Description)
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    GetObjectContext.SetAbort

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
     Dim rst As ADODB.Recordset
     Dim cmd As ADODB.Command
     Dim cnn As ADODB.Connection
     
     On Error GoTo ErrTrap:
     
     Set rst = New ADODB.Recordset
     Set cmd = New ADODB.Command
     Set cnn = New ADODB.Connection
     
     cnn.Open (CONST_PSYQUEL_CNN)
     Set cmd.ActiveConnection = cnn

     With cmd
    'Create the parameter objects
         .CommandText = "uspSelPatTxByPatApptID"
         .CommandType = adCmdStoredProc
         .Parameters.Append .CreateParameter("@PatApptID", adInteger, adParamInput, , lngPatApptID)
     End With
    
    'Execute the stored procedure
     rst.CursorLocation = adUseClient
     rst.Open cmd, , adOpenForwardOnly, adLockReadOnly
    
     Set FetchByPatApptID = rst
    
    'Disconnect the recordset
     Set cmd.ActiveConnection = Nothing
     Set cmd = Nothing
     Set rst.ActiveConnection = Nothing
     Set cnn = Nothing
     
    'Signal successful completion
     GetObjectContext.SetComplete
    
     Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
     Set cmd = Nothing
     Set rst = Nothing
     Set cnn = Nothing
     GetObjectContext.SetAbort
     
     Call Err.Raise(Err.Number, Err.Source, Err.Description)
End Function

