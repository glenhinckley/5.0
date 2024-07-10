VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 2  'RequiresTransaction
END
Attribute VB_Name = "CRPCredDB"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CRPCredDB
'Date: 10/19/2000
'Author: Eric Pena
'Description:  MTS object designed to host methods associated with data affecting the tblRPProviderCredit  table.
'--------------------------------------------------------------------
'Revision History:
'-------------------------------------------------------------------
Option Explicit

Private Const CLASS_NAME As String = "CRPCredDB"
Private Const TABLE_NAME As String = "tblRPProviderCredit"
Public Function StoreCredit(ByVal lngID As Long, ByVal lngELID As Long, ByVal lngRPID As Long, ByVal curAmt As Currency)
'--------------------------------------------------------------------
'Date: 10/19/2000
'Author: Eric Pena
'Description:  Modifies/Inserts a row into the tblRPProviderCredit  and tblTransaction table utilizing a stored procedure.
'Parameters: lngid - the Id in tblRPProviderCredit to use, or -1 if a new record is to be added
'                   lngelid - the EL to post from (will deduct from this EL)
'                   curamt - amount to pot to credit
'Returns: null
'--------------------------------------------------------------------
    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command
    Dim intCount As Integer
    On Error GoTo ErrTrap:
    
    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    'Create the parameter objects
    With cmdSQL
        .CommandText = "uspStoreRPCred"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@ID", adInteger, adParamInput, , IIf(lngID < 0, Null, lngID))
        .Parameters.Append .CreateParameter("@ELID", adInteger, adParamInput, , lngELID)
        .Parameters.Append .CreateParameter("@RPID", adInteger, adParamInput, , lngRPID)
        .Parameters.Append .CreateParameter("@Amt", adCurrency, adParamInput, , curAmt)
        .Parameters.Append .CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0)
    End With
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection

    Call cnnSQL.Open("DSN=PsyquelSQL")
    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute ', , adExecuteNoRecords

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
    Set cnnSQL = Nothing
    Call RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME)
    Set cmdSQL = Nothing
    GetObjectContext.SetAbort
End Function
Public Sub ApplyCredit(ByVal lngID As Long, ByVal curAmt As Currency)
'--------------------------------------------------------------------
'Date: 10/19/2000
'Author: Eric Pena
'Description:  Updates a row in the tblRPProviderCredit, inserts a row into the tbltransaction  table utilizing a stored procedure.
'Parameters: lngID:  The ID of the row to be updated All other parameters identifies the column value that will be  updated.
'Returns: Null
'--------------------------------------------------------------------
    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command
    
    On Error GoTo ErrTrap:
    
    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    
    With cmdSQL
        .CommandText = "uspApplyRPCred"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@ID", adInteger, adParamInput, , lngID)
        .Parameters.Append .CreateParameter("@Amt", adCurrency, adParamInput, , curAmt)
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
    Set cnnSQL = Nothing
    GetObjectContext.SetAbort
    Call RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, cmdSQL.Parameters("@ErrorMsg").Value)
    Set cmdSQL = Nothing
End Sub
Public Function FetchByID(ByVal lngID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 10/19/2000
'Author: Eric Pena
'Description:  Returns rows from the tblRPProviderCredit table ordered by patient name utilizing a stored procedure
'Parameters: lngID - PK to return records for
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
         .CommandText = "uspSelRPCredByID"
         .CommandType = adCmdStoredProc
         .Parameters.Append .CreateParameter("@ID", adInteger, adParamInput, , lngID)
     End With
    
    'Execute the stored procedure
     rs.CursorLocation = adUseClient
     rs.Open cmd, , adOpenForwardOnly, adLockReadOnly
    
     Set FetchByID = rs
    
    'Disconnect the recordset
     Set cmd = Nothing
     Set cnn = Nothing
     Set rs.ActiveConnection = Nothing
    'Signal successful completion
     GetObjectContext.SetComplete
    
     Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
     Set cmd = Nothing
     Set rs = Nothing
     Set cnn = Nothing
     GetObjectContext.SetAbort
     Call RaiseError(Err, Err.Number, CLASS_NAME)
 End Function
Public Function FetchByPatient(ByVal lngPatientID As Long, ByVal blnZeroBal As Boolean) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 10/19/2000
'Author: Eric Pena
'Description:  Returns rows from the tblRPProviderCredit table ordered by provider name utilizing a stored procedure
'Parameters: lngProviderID - id of the patient  to return records for
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
         .CommandText = "uspSelRPCredByPat"
         .CommandType = adCmdStoredProc
         .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID)
         .Parameters.Append .CreateParameter("@ZeroBalYN", adVarChar, adParamInput, 1, IIf(blnZeroBal, "Y", "N"))
     End With
     
    'Execute the stored procedure
     rs.CursorLocation = adUseClient
     rs.Open cmd, , adOpenForwardOnly, adLockReadOnly
    
     Set FetchByPatient = rs
    
    'Disconnect the recordset
     Set cmd = Nothing
     Set rs.ActiveConnection = Nothing
     Set cnn = Nothing
     
    'Signal successful completion
     GetObjectContext.SetComplete
    
     Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
     Set cmd.ActiveConnection = Nothing
     Set cmd = Nothing
     Set rs.ActiveConnection = Nothing
     Set rs = Nothing
     Set cnn = Nothing
     GetObjectContext.SetAbort
     Call RaiseError(Err, Err.Number, CLASS_NAME)
 End Function
Public Function Fetch(ByVal blnZeroBal As Boolean) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 10/19/2000
'Author: Eric Pena
'Description:  Returns rows from tblRPProviderCredit determining current outstanding balances
'Parameters:    lngpatientid - Patient ID for which to search balances, or -1 for all patients
'                       lngproviderid - Provider ID for which to search balances, or -1 for all providers
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
         .CommandText = "uspSelRPCred"
         .CommandType = adCmdStoredProc
         .Parameters.Append .CreateParameter("@ZeroBalYN", adVarChar, adParamInput, 1, IIf(blnZeroBal, "Y", "N"))
         .Parameters.Append .CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0)
     End With
    
    'Execute the stored procedure
     rs.CursorLocation = adUseClient
     rs.Open cmd, , adOpenForwardOnly, adLockReadOnly
    
     Set Fetch = rs
    
    'Disconnect the recordset
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

Public Function FetchByProvider(ByVal lngProviderID As Long, ByVal blnZeroBal As Boolean) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 10/19/2000
'Author: Eric Pena
'Description:  Returns rows from the tblRPProviderCredit table ordered by provider name utilizing a stored procedure
'Parameters: lngProviderID - id of the patient  to return records for
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
         .CommandText = "uspSelRPCredByProv"
         .CommandType = adCmdStoredProc
         .Parameters.Append .CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID)
         .Parameters.Append .CreateParameter("@ZeroBalYN", adVarChar, adParamInput, 1, IIf(blnZeroBal, "Y", "N"))
     End With
     
    'Execute the stored procedure
     rs.CursorLocation = adUseClient
     rs.Open cmd, , adOpenForwardOnly, adLockReadOnly
    
     Set FetchByProvider = rs
    
    'Disconnect the recordset
     Set cmd = Nothing
     Set rs.ActiveConnection = Nothing
     Set cnn = Nothing
     
    'Signal successful completion
     GetObjectContext.SetComplete
    
     Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
     Set cmd.ActiveConnection = Nothing
     Set cmd = Nothing
     Set rs.ActiveConnection = Nothing
     Set rs = Nothing
     Set cnn = Nothing
     GetObjectContext.SetAbort
     Call RaiseError(Err, Err.Number, CLASS_NAME)
 End Function

