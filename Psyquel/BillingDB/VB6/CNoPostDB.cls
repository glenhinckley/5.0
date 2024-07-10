VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 2  'RequiresTransaction
END
Attribute VB_Name = "CNoPostDB"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CNoPostDB
'Date: 09/06/2001
'Author: Eric Pena
'Description:  MTS object designed to host methods associated with
'              data affecting the tblNonPostable table.
'--------------------------------------------------------------------
Option Explicit
Private Const CLASS_NAME As String = "CNoPostDB"
Private Const TABLE_NAME As String = "tblNonPostable"
Public Function Insert(ByVal lngCheckID As Long, ByVal curPostAmount As Currency, ByVal curAmtDisallow As Currency, ByVal curAmtPatient As Currency, _
                        ByVal lngPatientID As Long, ByVal lngProviderID As Long, ByVal lngInsuranceID As Long, ByVal dteDOS As Date, ByVal strCPT As String, _
                        ByVal strReason As String, ByVal lngDupeCheck As Long, ByVal dteAction As Date, ByVal strAction As String, ByVal lngClaimID As Long, _
                        ByVal strDupeCheckNum As String, ByVal dteDupeCheckDate As Date, ByVal strNotes As String, ByVal strUserName As String) As Long
'--------------------------------------------------------------------
'Date: 09/06/2001
'Author: Eric Pena
'Description:  Inserts a row into the tblNonPostable table utilizing a stored procedure.
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
        .CommandText = "uspInsNonPostable"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@NPID", adInteger, adParamReturnValue)
        .Parameters.Append .CreateParameter("@CheckID", adInteger, adParamInput, , IIf(lngCheckID < 0, Null, lngCheckID))
        .Parameters.Append .CreateParameter("@PostAmount", adCurrency, adParamInput, , curPostAmount)
        .Parameters.Append .CreateParameter("@AmtDisallow", adCurrency, adParamInput, , curAmtDisallow)
        .Parameters.Append .CreateParameter("@AmtPatient", adCurrency, adParamInput, , curAmtPatient)
        .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , IIf(lngPatientID < 0, Null, lngPatientID))
        .Parameters.Append .CreateParameter("@ProviderID", adInteger, adParamInput, , IIf(lngProviderID < 0, Null, lngProviderID))
        .Parameters.Append .CreateParameter("@InsuranceID", adInteger, adParamInput, , IIf(lngInsuranceID < 0, Null, lngInsuranceID))
        .Parameters.Append .CreateParameter("@DOS", adDBTimeStamp, adParamInput, , IIf(dteDOS = 0, Null, dteDOS))
        .Parameters.Append .CreateParameter("@CPT", adVarChar, adParamInput, 5, IIf(strCPT = "", Null, strCPT))
        
        .Parameters.Append .CreateParameter("@Reason", adVarChar, adParamInput, 50, strReason)
        .Parameters.Append .CreateParameter("@DupeCheckID", adInteger, adParamInput, , IIf(lngDupeCheck < 0, Null, lngDupeCheck))
        .Parameters.Append .CreateParameter("@ActionDate", adDBTimeStamp, adParamInput, , IIf(dteAction = 0, Null, dteAction))
        .Parameters.Append .CreateParameter("@Action", adVarChar, adParamInput, 50, strAction)
        .Parameters.Append .CreateParameter("@ClaimID", adInteger, adParamInput, , IIf(lngClaimID < 0, Null, lngClaimID))
        .Parameters.Append .CreateParameter("@DupeCheckNum", adVarChar, adParamInput, 20, strDupeCheckNum)
        .Parameters.Append .CreateParameter("@DupeCheckDate", adDBTimeStamp, adParamInput, , IIf(dteDupeCheckDate = 0, Null, dteDupeCheckDate))
        
        .Parameters.Append .CreateParameter("@Notes", adVarChar, adParamInput, 256, strNotes)
        .Parameters.Append .CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strUserName)
        .Parameters.Append .CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open(CONST_PSYQUEL_CNN)
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute , , adExecuteNoRecords

    Insert = cmdSQL.Parameters("@NPID").Value
    
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
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    GetObjectContext.SetAbort
    Call RaiseError(Err, -1, CLASS_NAME, "SQL Server Error")
End Function
Public Sub Update(ByVal lngNPID As Long, ByVal lngOldCheckID As Long, ByVal curOldAmt As Currency, ByVal lngCheckID As Long, ByVal curPostAmount As Currency, ByVal curAmtDisallow As Currency, ByVal curAmtPatient As Currency, _
                        ByVal lngPatientID As Long, ByVal lngProviderID As Long, ByVal lngInsuranceID As Long, ByVal dteDOS As Date, ByVal strCPT As String, _
                        ByVal strReason As String, ByVal lngDupeCheck As Long, ByVal dteAction As Date, ByVal strAction As String, ByVal lngClaimID As Long, _
                        ByVal strDupeCheckNum As String, ByVal dteDupeCheckDate As Date, ByVal strNotes As String, ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 09/06/2001
'Author: Eric Pena
'Description:  Updates a row in the tblNonPostable table utilizing a stored procedure.
'Parameters: Each parameter identifies the column value that will be inserted.
'Returns: Null
'--------------------------------------------------------------------
    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command

    On Error GoTo ErrTrap:

    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    
    With cmdSQL
        .CommandText = "uspUpdNonPostable"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@NPID", adInteger, adParamInput, , lngNPID)
        .Parameters.Append .CreateParameter("@OldCheckID", adInteger, adParamInput, , lngOldCheckID)
        .Parameters.Append .CreateParameter("@OldAmount", adCurrency, adParamInput, , curOldAmt)
        .Parameters.Append .CreateParameter("@CheckID", adInteger, adParamInput, , lngCheckID)
        .Parameters.Append .CreateParameter("@PostAmount", adCurrency, adParamInput, , curPostAmount)
        .Parameters.Append .CreateParameter("@AmtDisallow", adCurrency, adParamInput, , curAmtDisallow)
        .Parameters.Append .CreateParameter("@AmtPatient", adCurrency, adParamInput, , curAmtPatient)
        .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , IIf(lngPatientID < 0, Null, lngPatientID))
        .Parameters.Append .CreateParameter("@ProviderID", adInteger, adParamInput, , IIf(lngProviderID < 0, Null, lngProviderID))
        .Parameters.Append .CreateParameter("@InsuranceID", adInteger, adParamInput, , IIf(lngInsuranceID < 0, Null, lngInsuranceID))
        .Parameters.Append .CreateParameter("@DOS", adDBTimeStamp, adParamInput, , IIf(dteDOS = 0, Null, dteDOS))
        .Parameters.Append .CreateParameter("@CPT", adVarChar, adParamInput, 5, IIf(strCPT = "", Null, strCPT))
        
        .Parameters.Append .CreateParameter("@Reason", adVarChar, adParamInput, 50, strReason)
        .Parameters.Append .CreateParameter("@DupeCheckID", adInteger, adParamInput, , IIf(lngDupeCheck < 0, Null, lngDupeCheck))
        .Parameters.Append .CreateParameter("@ActionDate", adDBTimeStamp, adParamInput, , IIf(dteAction = 0, Null, dteAction))
        .Parameters.Append .CreateParameter("@Action", adVarChar, adParamInput, 50, strAction)
        .Parameters.Append .CreateParameter("@ClaimID", adInteger, adParamInput, , IIf(lngClaimID < 0, Null, lngClaimID))
        .Parameters.Append .CreateParameter("@DupeCheckNum", adVarChar, adParamInput, 20, strDupeCheckNum)
        .Parameters.Append .CreateParameter("@DupeCheckDate", adDBTimeStamp, adParamInput, , IIf(dteDupeCheckDate = 0, Null, dteDupeCheckDate))
        
        .Parameters.Append .CreateParameter("@Notes", adVarChar, adParamInput, 256, strNotes)
        .Parameters.Append .CreateParameter("@Username", adVarChar, adParamInput, 30, strUserName)
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

    'Close the connection and free all resources
    cnnSQL.Close
    
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    GetObjectContext.SetAbort
    Call RaiseError(Err, -1, CLASS_NAME, "SQL Server Error")
End Sub
Public Sub Process(ByVal lngNPID As Long, ByVal blnIsRefunded As Boolean, ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 09/06/2001
'Author: Eric Pena
'Description:  Processed a row in the tblNonPostable table
'Parameters: lngNPID - PK of the row in the tblNonPostable table to update
'               blnIsRefunded - If true, will mark the row as refunded.  false will mark the row as posted
'Returns: Null
'--------------------------------------------------------------------
    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command

    On Error GoTo ErrTrap:

    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    
    With cmdSQL
        .CommandText = "uspUpdNonPostableAction"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@NPID", adInteger, adParamInput, , lngNPID)
        .Parameters.Append .CreateParameter("@RefundYN", adVarChar, adParamInput, 1, IIf(blnIsRefunded, "Y", "N"))
        .Parameters.Append .CreateParameter("@ActionBy", adVarChar, adParamInput, 30, strUserName)
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

    'Close the connection and free all resources
    cnnSQL.Close
    
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    GetObjectContext.SetAbort
    Call RaiseError(Err, -1, CLASS_NAME, "SQL Server Error")
End Sub
Public Sub Delete(ByVal lngID As Long, ByVal lngOldCheckID As Long, ByVal curOldAmt As Currency, ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 09/06/2001
'Author: Eric Pena
'Description:  Deletes a row in the tblNonPostable table
'Parameters: lngID - ID of the row in the table whose value will be deleted.
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
        .CommandText = "uspDelNonPostable"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@NPID", adInteger, adParamInput, , lngID)
        .Parameters.Append .CreateParameter("@OldCheckID", adInteger, adParamInput, , lngOldCheckID)
        .Parameters.Append .CreateParameter("@OldAmount", adCurrency, adParamInput, , curOldAmt)
        .Parameters.Append .CreateParameter("@Username", adVarChar, adParamInput, 30, strUserName)
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

    'Close the connection and free all resources
    cnnSQL.Close
    
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    GetObjectContext.SetAbort
    Call RaiseError(Err, -1, CLASS_NAME, "SQL Server Error")
End Sub
Public Function FetchByID(ByVal lngNPID As Long) As ADODB.Recordset
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
         .CommandText = "uspSelNonPostableByID"
         .CommandType = adCmdStoredProc
         .Parameters.Append .CreateParameter("@ID", adInteger, adParamInput, , lngNPID)
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
     Set cmd = Nothing
     Set rs = Nothing
     Set cnn = Nothing
     GetObjectContext.SetAbort
    Call RaiseError(Err, -1, CLASS_NAME, "SQL Server Error")
 End Function
Public Function FetchByCriteria(ByVal dteStartAdded As Date, ByVal dteEndAdded As Date, ByVal dteStartAction As Date, ByVal dteEndAction As Date, _
                                    ByVal strAction As String, ByVal lngPatientID As Long, ByVal strCheckNum As String, ByVal lngInsuranceID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 09/06/2001
'Author: Eric Pena
'Description:  Returns rows from the tblNonPostable table utilizing a stored procedure
'Parameters: Each parameter identifies the column value that will be filtered.
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
         .CommandText = "uspSelNonPostableByCriteria"
         .CommandType = adCmdStoredProc
         .Parameters.Append .CreateParameter("@StartAdded", adDBTimeStamp, adParamInput, , IIf(dteStartAdded = 0, Null, dteStartAdded))
         .Parameters.Append .CreateParameter("@EndAdded", adDBTimeStamp, adParamInput, , IIf(dteEndAdded = 0, Null, dteEndAdded))
         .Parameters.Append .CreateParameter("@Action", adVarChar, adParamInput, 50, strAction)
         .Parameters.Append .CreateParameter("@StartAction", adDBTimeStamp, adParamInput, , IIf(dteStartAction = 0, Null, dteStartAction))
         .Parameters.Append .CreateParameter("@EndAction", adDBTimeStamp, adParamInput, , IIf(dteEndAction = 0, Null, dteEndAction))
     
         .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID)
         .Parameters.Append .CreateParameter("@CheckNum", adVarChar, adParamInput, 30, strCheckNum)
         .Parameters.Append .CreateParameter("@InsuranceID", adInteger, adParamInput, , lngInsuranceID)
     
     End With
    
    'Execute the stored procedure
     rs.CursorLocation = adUseClient
     rs.Open cmd, , adOpenForwardOnly, adLockReadOnly
    
     Set FetchByCriteria = rs
    
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
Public Function isProcessed(ByVal lngNPID As Long) As Boolean
'--------------------------------------------------------------------
'Date: 09/06/2001
'Author: Eric Pena
'Description:  Lets the caller know if a given Mis. Pmt. has been processed
'Parameters: lngMisPmtID - The ID of the MP Entry
'Returns: T if the record is processed, F otherwise
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------
    Dim rstSQL As ADODB.Recordset
    
    isProcessed = False
    
    On Error GoTo ErrTrap:
    
    Set rstSQL = FetchByID(lngNPID)
    
    If Not rstSQL.EOF Then
        If rstSQL.Fields("fldPostedYN").Value = "Y" Or rstSQL.Fields("fldRefundYN").Value = "Y" Then isProcessed = True
    End If

    'Signal successful completion
    GetObjectContext.SetComplete
    Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set rstSQL = Nothing
    GetObjectContext.SetAbort
    Call RaiseError(Err, -1, CLASS_NAME, "SQL Server Error")
End Function
