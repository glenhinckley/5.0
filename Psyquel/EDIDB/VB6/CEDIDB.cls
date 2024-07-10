VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CEDIDB"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'-------------------------------------------------------------------------------------
'Date: 01/07/2003
'Class Name: CEDIBz
'Author: Dave Richkun
'Description:   COM object designed to host methods associated with electronic
'               data exchange.
'--------------------------------------------------------------------------------------
Option Explicit

Private Const CLASS_NAME As String = "CEDIBz"

Public Function FetchElectClaims(ByVal strWhere As String, Optional ByVal strDataBase As String) As ADODB.Recordset
'--------------------------------------------------------------------------------------
'Date: 10/10/2002
'Author: Dave Richkun
'Description: Retrieves records from tblClaim matching the criteria specified in
'             the strWhere parametetr.
'Parameters: strWhere - String portion of SQL 'where' clause that identifies the
'              claim selection criteria
'Returns: ADODB.Recordset
'--------------------------------------------------------------------------------------

    Dim cnnSQL As ADODB.Connection
    Dim rstSQL As ADODB.Recordset
    Dim cmdSQL As ADODB.Command
    Dim strConnect As String
    Dim strSQL As String
    
    On Error GoTo ErrTrap:
    
    Set rstSQL = New ADODB.Recordset
    Set cmdSQL = New ADODB.Command
    Set cnnSQL = New ADODB.Connection
      
    Select Case strDataBase
          Case "Test"
               cnnSQL.Open (CONST_TEST_CNN)
          Case "PsyquelDirect"
               cnnSQL.Open (CONST_DIRECT_CNN)
          Case Else
               cnnSQL.Open (CONST_PSYQUEL_CNN)
    End Select
    
    Set cmdSQL.ActiveConnection = cnnSQL
    
    'Create the parameter objects
    With cmdSQL
        strSQL = "SELECT * FROM vwClaim_EDI "
        strSQL = strSQL & strWhere
        strSQL = strSQL & " ORDER BY fldClaimType, fldClearingHouseID, SubmitterID, fldPayerCode, fldInsuranceID, fldInsType, fldProviderLastName, fldProviderFirstName, fldRPID, fldPatientID, fldEncounterLogID "
    End With
    
    cmdSQL.CommandText = strSQL
    cmdSQL.CommandType = adCmdText
    
    'Execute the stored procedure
    rstSQL.CursorLocation = adUseClient
    rstSQL.Open cmdSQL, , adOpenForwardOnly, adLockReadOnly
    
    Set FetchElectClaims = rstSQL
    
    'Disconnect the recordset
    Set cmdSQL.ActiveConnection = Nothing
    Set cmdSQL = Nothing
    Set rstSQL.ActiveConnection = Nothing
    Set cnnSQL = Nothing
    
    GetObjectContext.SetComplete
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling enviroment.
    Set cmdSQL.ActiveConnection = Nothing
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    GetObjectContext.SetAbort
    
    Call RaiseError(Err, Err.Number, CLASS_NAME)
End Function

Public Function GetNextFileNumber() As ADODB.Recordset
'--------------------------------------------------------------------------------------
'Date: 12/06/2003
'Author: Dave Richkun
'Description: Returns the file number and the starting transaction (BHT) number
'             for a new E-claim file.
'Parameters: None
'Returns: ADO Recordset containing new file number and starting transaction number.
'--------------------------------------------------------------------------------------

     Dim rst As ADODB.Recordset
     Dim cmd As ADODB.Command
     Dim cnn As ADODB.Connection
     
     On Error GoTo ErrTrap:
     
     Set rst = New ADODB.Recordset
     Set cmd = New ADODB.Command
     Set cnn = New ADODB.Connection
     
     cnn.Open (CONST_PSYQUEL_CNN)
     Set cmd.ActiveConnection = cnn
     
    'Create the parameter objects
     With cmd
         .CommandText = "uspSelNextEFile"
         .CommandType = adCmdStoredProc
     End With
    
    'Execute the stored procedure
     rst.CursorLocation = adUseClient
     rst.Open cmd, , adOpenForwardOnly, adLockReadOnly
    
     Set GetNextFileNumber = rst
    
    'Disconnect the recordset
     Set cmd.ActiveConnection = Nothing
     Set cmd = Nothing
     Set rst.ActiveConnection = Nothing
     Set cnn = Nothing
     
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


Public Sub Insert(ByVal lngFileNumber As Long, ByVal strFileName As String, _
                       ByVal lngStartTxNum As Long, ByVal lngEndTxNum As Long, _
                       ByVal strUserName As String)
'--------------------------------------------------------------------------------------
'Date: 12/07/2003
'Author: Dave Richkun
'Description: Inserts a row into tblEFile via a stored procedure
'Parameters: lngFileNumber - ID of the file
'            lngFileName - The name of the file
'            lngStartTxNum - Starting transaction number within the file
'            lngEndTxNum - Ending transaction number within the file
'            strUserName - Name of the user running the procedure
'Returns: Null
'--------------------------------------------------------------------------------------

    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command
    
    On Error GoTo ErrTrap:
    
    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    
    'Create the parameter objects
    With cmdSQL
        .CommandText = "uspInsEFile"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@EFileID", adInteger, adParamInput, , lngFileNumber)
        .Parameters.Append .CreateParameter("@FileName", adVarChar, adParamInput, 20, strFileName)
        .Parameters.Append .CreateParameter("@StartTxNum", adInteger, adParamInput, , lngStartTxNum)
        .Parameters.Append .CreateParameter("@EndTxNum", adInteger, adParamInput, , lngEndTxNum)
        .Parameters.Append .CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strUserName)
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

    GetObjectContext.SetComplete
    Exit Sub

ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    
    GetObjectContext.SetAbort
    Call RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME)

End Sub

Public Sub EstablishXRef(ByVal lngClaimID As Long, ByVal lngEncounterLogID As Long, _
                         ByVal lngEFileID As Long, ByVal strEFileName As String, Optional ByVal strDataBase As String)
'--------------------------------------------------------------------------------------
'Date: 01/21/2003
'Author: Dave Richkun
'Description: Establishes a cross-reference between a Psyquel claim and an electronically
'             submitted claim.
'Parameters: lngClaimID - ID of the row in tblClaim that will be updated
'            lngEncounterLogID - ID of the encounter log in tblClaim that will be updated
'            lngEFileID - ID of the number assigned to the electronic claim
'            strEFileName - The name of the file containing the electronic claim
'Returns: Null
'--------------------------------------------------------------------------------------

    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command
    
    On Error GoTo ErrTrap:
    
    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    'Create the parameter objects
    With cmdSQL
        .CommandText = "uspUpdClaimXRef"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@ClaimID", adInteger, adParamInput, , lngClaimID)
        .Parameters.Append .CreateParameter("@ELID", adInteger, adParamInput, , lngEncounterLogID)
        .Parameters.Append .CreateParameter("@EFileID", adInteger, adParamInput, , lngEFileID)
        .Parameters.Append .CreateParameter("@FileName", adVarChar, adParamInput, 30, strEFileName)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
      
    Select Case strDataBase
          Case "Test"
               cnnSQL.Open (CONST_TEST_CNN)
          Case "PsyquelDirect"
               cnnSQL.Open (CONST_DIRECT_CNN)
          Case Else
               cnnSQL.Open (CONST_PSYQUEL_CNN)
    End Select
    
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

    GetObjectContext.SetComplete
    Exit Sub

ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    
    GetObjectContext.SetAbort
    Call RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME)

End Sub


Public Sub UpdateQueuedStatus(ByVal strEFileName As String)
'--------------------------------------------------------------------------------------
'Date: 02/25/2003
'Author: Dave Richkun
'Description: Updates the Queued status in tblClaim for all claims submitted under a
'             particular file name.
'Parameters: lngEFileID - ID of the file under which claims have been electronically submitted
'            strEFileName - The name of the file containing the electronic claims whose Queued
'               status will be updated
'Returns: Null
'--------------------------------------------------------------------------------------

    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command
    
    On Error GoTo ErrTrap:
    
    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    
    'Create the parameter objects
    With cmdSQL
        .CommandText = "uspUpdClaimQueued"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@FileName", adVarChar, adParamInput, 30, strEFileName)
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

    GetObjectContext.SetComplete
    Exit Sub

ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    
    GetObjectContext.SetAbort
    Call RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME)

End Sub

Public Sub UpdateACK(ByVal lngEFileID As Long, _
                     ByVal lngStartTxNum As Long, ByVal lngEndTxNum As Long, _
                     ByVal strAK1 As String, ByVal strAK2 As String, _
                     ByVal strAKDetail As String, _
                     ByVal strAK5 As String, ByVal strAK9 As String)
'--------------------------------------------------------------------------------------
'Date: 11/01/2007
'Author: Duane C Orth
'Description: Updates the Acknoledegement fields in tblEFile for all claims submitted under a
'             particular file id.
'Parameters: lngEFileID - ID of the file under which claims have been electronically submitted
'            lngStartTxNum - Starting transaction number within the file
'            lngEndTxNum - Ending transaction number within the file
'Returns: Null
'--------------------------------------------------------------------------------------

    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command
    
    On Error GoTo ErrTrap:
    
    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    
    'Create the parameter objects
    With cmdSQL
        .CommandText = "uspUpdAck"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@EFileID", adInteger, adParamInput, , lngEFileID)
        .Parameters.Append .CreateParameter("@StartTxNum", adInteger, adParamInput, , lngStartTxNum)
        .Parameters.Append .CreateParameter("@EndTxNum", adInteger, adParamInput, , lngEndTxNum)
        .Parameters.Append .CreateParameter("@Ak1", adVarChar, adParamInput, 20, strAK1)
        .Parameters.Append .CreateParameter("@Ak2", adVarChar, adParamInput, 20, strAK2)
        .Parameters.Append .CreateParameter("@AkDetail", adVarChar, adParamInput, 512, strAKDetail)
        .Parameters.Append .CreateParameter("@Ak5", adVarChar, adParamInput, 20, strAK5)
        .Parameters.Append .CreateParameter("@Ak9", adVarChar, adParamInput, 20, strAK9)
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

    GetObjectContext.SetComplete
    Exit Sub

ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    
    GetObjectContext.SetAbort
    Call RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME)

End Sub


