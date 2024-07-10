VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 2  'RequiresTransaction
END
Attribute VB_Name = "CMisdirectPmtDB"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CMisdirectPmtBz
'Date: 06/11/2001
'Author: Eric Pena
'Description:  MTS object designed to host methods associated with
'              data affecting the tblMisdirectedInsPayment table.
'--------------------------------------------------------------------
Option Explicit
Private Const CLASS_NAME As String = "CMisdirectPmtDB"
Private Const TABLE_NAME As String = "tblMisdirectedInsPayment"
Private Const CONST_NO_DATE As Date = #12:00:00 AM#
Public Function Insert(ByVal lngBRID As Long, ByVal strCheckNum As String, ByVal dteCheckDate As Date, ByVal dteClearDate As Date, ByVal curCheckAmount As Currency, ByVal curPostAmount As Currency, _
                        ByVal curAmtDisallow As Currency, ByVal curAmtPatient As Currency, ByVal blnMultiCheck As Boolean, ByVal strAddress1 As String, _
                        ByVal strAddress2 As String, ByVal strCity As String, ByVal strState As String, ByVal strZip As String, ByVal strContactName As String, _
                        ByVal strContactPhone As String, ByVal strContactExt As String, ByVal strNotes As String, ByVal strUserName As String) As Long
'--------------------------------------------------------------------
'Date: 06/11/2001
'Author: Eric Pena
'Description:  Inserts a row into the tblMisdirectedPayment table utilizing a stored procedure.
'Parameters: Each parameter identifies the column value that will be  inserted.
'Returns: ID (Primary Key) of the row inserted.
'--------------------------------------------------------------------
    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command
    Dim intCount As Integer
    
    On Error GoTo ErrTrap:
    
    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    
    'Create the parameter objects
    With cmdSQL
        .CommandText = "uspInsMisPmt"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@MisPmtID", adInteger, adParamReturnValue)
        .Parameters.Append .CreateParameter("@BRID", adInteger, adParamInput, , lngBRID)
        .Parameters.Append .CreateParameter("@CheckNum", adVarChar, adParamInput, 30, strCheckNum)
        .Parameters.Append .CreateParameter("@CheckDate", adDBTimeStamp, adParamInput, , IIf(dteCheckDate = 0, Null, dteCheckDate))
        .Parameters.Append .CreateParameter("@ClearDate", adDBTimeStamp, adParamInput, , IIf(dteClearDate = 0, Null, dteClearDate))
        .Parameters.Append .CreateParameter("@CheckAmount", adCurrency, adParamInput, , curCheckAmount)
        .Parameters.Append .CreateParameter("@PostAmount", adCurrency, adParamInput, , curPostAmount)
        .Parameters.Append .CreateParameter("@AmtDisallow", adCurrency, adParamInput, , curAmtDisallow)
        .Parameters.Append .CreateParameter("@AmtPatient", adCurrency, adParamInput, , curAmtPatient)
        .Parameters.Append .CreateParameter("@MultiCheckYN", adVarChar, adParamInput, 1, IIf(blnMultiCheck, "Y", "N"))
        .Parameters.Append .CreateParameter("@SentAddress1", adVarChar, adParamInput, 60, strAddress1)
        .Parameters.Append .CreateParameter("@SentAddress2", adVarChar, adParamInput, 60, strAddress2)
        .Parameters.Append .CreateParameter("@SentCity", adVarChar, adParamInput, 40, strCity)
        .Parameters.Append .CreateParameter("@SentState", adVarChar, adParamInput, 2, strState)
        .Parameters.Append .CreateParameter("@SentZip", adVarChar, adParamInput, 12, strZip)
        .Parameters.Append .CreateParameter("@ContactName", adVarChar, adParamInput, 60, strContactName)
        .Parameters.Append .CreateParameter("@ContactPhone", adVarChar, adParamInput, 15, strContactPhone)
        .Parameters.Append .CreateParameter("@ContactExt", adVarChar, adParamInput, 10, strContactExt)
        .Parameters.Append .CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strUserName)
        .Parameters.Append .CreateParameter("@Notes", adVarChar, adParamInput, 256, strNotes)
        .Parameters.Append .CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open("DSN=PsyquelSQL")
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute , , adExecuteNoRecords

    Insert = cmdSQL.Parameters("@MisPmtID").Value
    
    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If
        
    'Close the connection
    cnnSQL.Close
    
    'free all resources
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing

    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Call RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME)
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    GetObjectContext.SetAbort
End Function
Public Sub Update(ByVal lngMisPmtID As Long, ByVal strCheckNum As String, ByVal dteCheckDate As Date, ByVal dteClearDate As Date, ByVal curCheckAmount As Currency, ByVal curPostAmount As Currency, _
                        ByVal curAmtDisallow As Currency, ByVal curAmtPatient As Currency, ByVal blnMultiCheck As Boolean, ByVal strAddress1 As String, _
                        ByVal strAddress2 As String, ByVal strCity As String, ByVal strState As String, ByVal strZip As String, ByVal strContactName As String, _
                        ByVal strContactPhone As String, ByVal strContactExt As String, ByVal strNotes As String, ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 06/11/2001
'Author: Eric Pena
'Description:  Updates a row in the tblMisdirectedPayment table utilizing a stored procedure.
'Parameters: Each parameter identifies the column value that will be inserted.
'Returns: Null
'--------------------------------------------------------------------
    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command

    On Error GoTo ErrTrap:

    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    
    With cmdSQL
        .CommandText = "uspUpdMisPmt"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@MisPmtID", adInteger, adParamInput, , lngMisPmtID)
        .Parameters.Append .CreateParameter("@CheckNum", adVarChar, adParamInput, 30, strCheckNum)
        .Parameters.Append .CreateParameter("@CheckDate", adDBTimeStamp, adParamInput, , IIf(dteCheckDate = CONST_NO_DATE, Null, dteCheckDate))
        .Parameters.Append .CreateParameter("@ClearDate", adDBTimeStamp, adParamInput, , IIf(dteClearDate = CONST_NO_DATE, Null, dteClearDate))
        .Parameters.Append .CreateParameter("@CheckAmount", adCurrency, adParamInput, , curCheckAmount)
        .Parameters.Append .CreateParameter("@PostAmount", adCurrency, adParamInput, , curPostAmount)
        .Parameters.Append .CreateParameter("@AmtDisallow", adCurrency, adParamInput, , curAmtDisallow)
        .Parameters.Append .CreateParameter("@AmtPatient", adCurrency, adParamInput, , curAmtPatient)
        .Parameters.Append .CreateParameter("@MultiCheckYN", adVarChar, adParamInput, 1, IIf(blnMultiCheck, "Y", "N"))
        .Parameters.Append .CreateParameter("@SentAddress1", adVarChar, adParamInput, 60, strAddress1)
        .Parameters.Append .CreateParameter("@SentAddress2", adVarChar, adParamInput, 60, strAddress2)
        .Parameters.Append .CreateParameter("@SentCity", adVarChar, adParamInput, 40, strCity)
        .Parameters.Append .CreateParameter("@SentState", adVarChar, adParamInput, 2, strState)
        .Parameters.Append .CreateParameter("@SentZip", adVarChar, adParamInput, 12, strZip)
        .Parameters.Append .CreateParameter("@ContactName", adVarChar, adParamInput, 60, strContactName)
        .Parameters.Append .CreateParameter("@ContactPhone", adVarChar, adParamInput, 15, strContactPhone)
        .Parameters.Append .CreateParameter("@ContactExt", adVarChar, adParamInput, 10, strContactExt)
        .Parameters.Append .CreateParameter("@Notes", adVarChar, adParamInput, 256, strNotes)
        .Parameters.Append .CreateParameter("@ModBy", adVarChar, adParamInput, 30, strUserName)
        .Parameters.Append .CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open("DSN=PsyquelSQL")
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute , , adExecuteNoRecords
    
    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If

    'Close the connection and free all resources
    cnnSQL.Close
    
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Call RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, cmdSQL.Parameters("@ErrorMsg").Value)
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    GetObjectContext.SetAbort
End Sub
Public Sub Deleted(ByVal lngID As Long, ByVal blnIsDeleted As Boolean, ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 06/11/2001
'Author: Eric Pena
'Description:  Flags a row in the tblMisdirectedInsPayment table marking the row as
'              deleted or undeleted.
'Parameters: blnDeleted - Boolean value identifying if the record is to
'               be deleted (True) or undeleted (False).
'            lngID - ID of the row in the table whose value will be
'               updated.
'            strUserName - Login name of the user responsible for
'               marking the row as deleted.
'Returns: Null
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------
    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command

    On Error GoTo ErrTrap:

    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    
    With cmdSQL
        .CommandText = "uspDelMisPmt"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@MisPmtID", adInteger, adParamInput, , lngID)
        .Parameters.Append .CreateParameter("@DeletedYN", adVarChar, adParamInput, 1, IIf(blnIsDeleted, "Y", "N"))
        .Parameters.Append .CreateParameter("@DeletedBy", adVarChar, adParamInput, 30, strUserName)
        .Parameters.Append .CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open("DSN=PsyquelSQL")
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute , , adExecuteNoRecords
    
    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If

    'Close the connection and free all resources
    cnnSQL.Close
    
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Call RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, cmdSQL.Parameters("@ErrorMsg").Value)
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    GetObjectContext.SetAbort
End Sub
 Public Function FetchByUser(ByVal lngUserID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 06/11/2001
'Author: Eric Pena
'Description:  Returns rows from the tblMisdirectedPayment table utilizing a stored procedure
'Parameters: lngUserID - user for which records will be returned
'Returns: ADORst
'--------------------------------------------------------------------
     Dim rs As ADODB.Recordset
     Dim cmd As ADODB.Command
     Dim cnn As ADODB.Connection
     
     On Error GoTo ErrTrap:
     
     Set rs = New ADODB.Recordset
     Set cmd = New ADODB.Command
     Set cnn = New ADODB.Connection
     cnn.Open ("DSN=PsyquelSQL")
     Set cmd.ActiveConnection = cnn
     
    'Create the parameter objects
     With cmd
         .CommandText = "uspSelMisdirectedPaymentsByUser"
         .CommandType = adCmdStoredProc
         .Parameters.Append .CreateParameter("@UserID", adInteger, adParamInput, 1, lngUserID)
     End With
    
    'Execute the stored procedure
     rs.CursorLocation = adUseClient
     rs.Open cmd, , adOpenForwardOnly, adLockReadOnly
    
     Set FetchByUser = rs
    
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
     Call RaiseError(Err, cmd.Parameters("@ErrorNum").Value, CLASS_NAME)
     Set cmd = Nothing
     Set rs = Nothing
     Set cnn = Nothing
     GetObjectContext.SetAbort
 End Function
Public Function FetchByEL(ByVal lngELID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 06/11/2001
'Author: Eric Pena
'Description:  Returns rows from the tblMisdirectedPayment table utilizing a stored procedure
'Parameters: lngELID - PK in tblEncounterLog for which records will be returned
'Returns: ADORst
'--------------------------------------------------------------------
     Dim rs As ADODB.Recordset
     Dim cmd As ADODB.Command
     Dim cnn As ADODB.Connection
     
     On Error GoTo ErrTrap:
     
     Set rs = New ADODB.Recordset
     Set cmd = New ADODB.Command
     Set cnn = New ADODB.Connection
     cnn.Open ("DSN=PsyquelSQL")
     Set cmd.ActiveConnection = cnn
     
    'Create the parameter objects
     With cmd
         .CommandText = "uspSelMisPmtByEL"
         .CommandType = adCmdStoredProc
         .Parameters.Append .CreateParameter("@ID", adInteger, adParamInput, , lngELID)
     End With
    
    'Execute the stored procedure
     rs.CursorLocation = adUseClient
     rs.Open cmd, , adOpenForwardOnly, adLockReadOnly
    
     Set FetchByEL = rs
    
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
     Call RaiseError(Err, cmd.Parameters("@ErrorNum").Value, CLASS_NAME)
     Set cmd = Nothing
     Set rs = Nothing
     Set cnn = Nothing
     GetObjectContext.SetAbort
 End Function
Public Function FetchByID(ByVal lngMPID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 06/11/2001
'Author: Eric Pena
'Description:  Returns rows from the tblMisdirectedPayment table utilizing a stored procedure
'Parameters: lngMPID - PK in tblMisdirectedPayment for which records will be returned
'Returns: ADORst
'--------------------------------------------------------------------
     Dim rs As ADODB.Recordset
     Dim cmd As ADODB.Command
     Dim cnn As ADODB.Connection
     
     On Error GoTo ErrTrap:
     
     Set rs = New ADODB.Recordset
     Set cmd = New ADODB.Command
     Set cnn = New ADODB.Connection
     cnn.Open ("DSN=PsyquelSQL")
     Set cmd.ActiveConnection = cnn
     
    'Create the parameter objects
     With cmd
         .CommandText = "uspSelMisPmtByID"
         .CommandType = adCmdStoredProc
         .Parameters.Append .CreateParameter("@ID", adInteger, adParamInput, , lngMPID)
     End With
    
    'Execute the stored procedure
     rs.CursorLocation = adUseClient
     rs.Open cmd, , adOpenForwardOnly, adLockReadOnly
    
     Set FetchByID = rs
    
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
     Call RaiseError(Err, cmd.Parameters("@ErrorNum").Value, CLASS_NAME)
     Set cmd = Nothing
     Set rs = Nothing
     Set cnn = Nothing
     GetObjectContext.SetAbort
 End Function
Public Function FetchByBR(ByVal lngBRID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 06/11/2001
'Author: Eric Pena
'Description:  Returns rows from the tblMisdirectedPayment table utilizing a stored procedure
'Parameters: lngBRID - PK in tblBillingResponsibility for which records will be returned
'Returns: ADORst
'--------------------------------------------------------------------
     Dim rs As ADODB.Recordset
     Dim cmd As ADODB.Command
     Dim cnn As ADODB.Connection
     
     On Error GoTo ErrTrap:
     
     Set rs = New ADODB.Recordset
     Set cmd = New ADODB.Command
     Set cnn = New ADODB.Connection
     cnn.Open ("DSN=PsyquelSQL")
     Set cmd.ActiveConnection = cnn
     
    'Create the parameter objects
     With cmd
         .CommandText = "uspSelMisPmtByBR"
         .CommandType = adCmdStoredProc
         .Parameters.Append .CreateParameter("@ID", adInteger, adParamInput, , lngBRID)
     End With
    
    'Execute the stored procedure
     rs.CursorLocation = adUseClient
     rs.Open cmd, , adOpenForwardOnly, adLockReadOnly
    
     Set FetchByBR = rs
    
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
     Call RaiseError(Err, cmd.Parameters("@ErrorNum").Value, CLASS_NAME)
     Set cmd = Nothing
     Set rs = Nothing
     Set cnn = Nothing
     GetObjectContext.SetAbort
 End Function
Public Function isProcessed(ByVal lngMisPmtID As Long) As Boolean
'--------------------------------------------------------------------
'Date: 06/11/2001
'Author: Eric Pena
'Description:  Lets the caller know if a given Mis. Pmt. has been processed
'Parameters: lngMisPmtID - The ID of the MP Entry
'Returns: T if the record is processed, F otherwise
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------
    
    Dim cnnSQL As ADODB.Connection
    Dim rstSQL As ADODB.Recordset
    Dim strSQL As String
    
    isProcessed = False
    
    On Error GoTo ErrTrap:
    
    'Prepare the SQL statement.
    strSQL = "SELECT fldProcessedYN FROM tblMisdirectedInsPayment WHERE fldMisPmtID = " & lngMisPmtID & " "
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open("DSN=PsyquelSQL")
    
    'Instantiate and populate the Recordset.
    Set rstSQL = New ADODB.Recordset
    rstSQL.CursorLocation = adUseClient
    Call rstSQL.Open(strSQL, cnnSQL, adOpenForwardOnly, adLockReadOnly, adCmdText + adAsyncFetch)
    
    'Disconnect the recordset, close the connection and return the recordset
    'to the calling environment.
    Set rstSQL.ActiveConnection = Nothing
    cnnSQL.Close
    Set cnnSQL = Nothing
    
    'set to null if there are no current associations (new Benefactor)
    If rstSQL.RecordCount = 0 Then GoTo EMPTY_RECORDSET
    If rstSQL.Fields("fldProcessedYN").Value = "Y" Then isProcessed = True
    
EMPTY_RECORDSET:
    'Signal successful completion
    GetObjectContext.SetComplete
    Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set rstSQL = Nothing
    Set cnnSQL = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Public Sub SetAction(ByVal lngMisPmtID As Long, ByVal strActionType As String, ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 06/11/2001
'Author: Eric Pena
'Description:  Updates a row in the tblMisdirectedPayment table utilizing a stored procedure.
'Parameters: Each parameter identifies the column value that will be inserted.
'Returns: Null
'--------------------------------------------------------------------
    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command

    On Error GoTo ErrTrap:

    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    
    With cmdSQL
        .CommandText = "uspUpdMisPmtAction"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@MisPmtID", adInteger, adParamInput, , lngMisPmtID)
        .Parameters.Append .CreateParameter("@ActionType", adVarChar, adParamInput, 1, strActionType)
        .Parameters.Append .CreateParameter("@ModifiedBy", adVarChar, adParamInput, 30, strUserName)
        .Parameters.Append .CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open("DSN=PsyquelSQL")
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute , , adExecuteNoRecords
    
    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If

    'Close the connection and free all resources
    cnnSQL.Close
    
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Call RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, cmdSQL.Parameters("@ErrorMsg").Value)
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    GetObjectContext.SetAbort
End Sub
Public Sub SetProcessed(ByVal lngMisPmtID As Long, ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 06/11/2001
'Author: Eric Pena
'Description:  Updates a row in the tblMisdirectedPayment table utilizing a stored procedure.
'Parameters: Each parameter identifies the column value that will be inserted.
'Returns: Null
'--------------------------------------------------------------------
    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command

    On Error GoTo ErrTrap:

    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    
    With cmdSQL
        .CommandText = "uspUpdMisPmtProcessed"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@MisPmtID", adInteger, adParamInput, , lngMisPmtID)
        .Parameters.Append .CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName)
        .Parameters.Append .CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open("DSN=PsyquelSQL")
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute , , adExecuteNoRecords
    
    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If

    'Close the connection and free all resources
    cnnSQL.Close
    
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Call RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, cmdSQL.Parameters("@ErrorMsg").Value)
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    GetObjectContext.SetAbort
End Sub
Public Function FetchForProcessing() As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 06/11/2001
'Author: Eric Pena
'Description:  Returns rows ready for processing from the tblMisdirectedPayment table utilizing a stored procedure
'Parameters:
'Returns: ADORst
'--------------------------------------------------------------------
     Dim rs As ADODB.Recordset
     Dim cmd As ADODB.Command
     Dim cnn As ADODB.Connection
     
     On Error GoTo ErrTrap:
     
     Set rs = New ADODB.Recordset
     Set cmd = New ADODB.Command
     Set cnn = New ADODB.Connection
     cnn.Open ("DSN=PsyquelSQL")
     Set cmd.ActiveConnection = cnn
     
    'Create the parameter objects
     With cmd
         .CommandText = "uspSelMisPmtForProcess"
         .CommandType = adCmdStoredProc
     End With
    
    'Execute the stored procedure
     rs.CursorLocation = adUseClient
     rs.Open cmd, , adOpenForwardOnly, adLockReadOnly
    
     Set FetchForProcessing = rs
    
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
     Call RaiseError(Err, cmd.Parameters("@ErrorNum").Value, CLASS_NAME)
     Set cmd = Nothing
     Set rs = Nothing
     Set cnn = Nothing
     GetObjectContext.SetAbort
 End Function
Public Function FetchForAutoAction() As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 06/11/2001
'Author: Eric Pena
'Description:  Returns rows ready for processing from the tblMisdirectedPayment table utilizing a stored procedure
'Parameters:
'Returns: ADORst
'--------------------------------------------------------------------
     Dim rs As ADODB.Recordset
     Dim cmd As ADODB.Command
     Dim cnn As ADODB.Connection
     
     On Error GoTo ErrTrap:
     
     Set rs = New ADODB.Recordset
     Set cmd = New ADODB.Command
     Set cnn = New ADODB.Connection
     cnn.Open ("DSN=PsyquelSQL")
     Set cmd.ActiveConnection = cnn
     
    'Create the parameter objects
     With cmd
         .CommandText = "uspSelMisPmtForAutoAction"
         .CommandType = adCmdStoredProc
     End With
    
    'Execute the stored procedure
     rs.CursorLocation = adUseClient
     rs.Open cmd, , adOpenForwardOnly, adLockReadOnly
    
     Set FetchForAutoAction = rs
    
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
     Call RaiseError(Err, cmd.Parameters("@ErrorNum").Value, CLASS_NAME)
     Set cmd = Nothing
     Set rs = Nothing
     Set cnn = Nothing
     GetObjectContext.SetAbort
 End Function
