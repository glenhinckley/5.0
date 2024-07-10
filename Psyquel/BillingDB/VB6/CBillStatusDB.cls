VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 2  'RequiresTransaction
END
Attribute VB_Name = "CBillStatusDB"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CBillStatusDB
'Date: 08/30/2000
'Author: Eric Pena
'Description:  MTS object designed to host methods associated with
'              data affecting the tblBillStatus table.
'--------------------------------------------------------------------
Option Explicit
Private Const CLASS_NAME As String = "CBillStatusDB"
Private Const TABLE_NAME As String = "tblBillStatus"
Public Function Insert(ByVal strDescription As String) As Long
'--------------------------------------------------------------------
'Date: 08/30/2000
'Author: Eric Pena
'Description:  Inserts a row into the tblBillStatus table utilizing
'              a stored procedure.
'Parameters: Each parameter identifies the column value that will be inserted.
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
        .CommandText = "uspInsBillStatus"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@BillStatusID", adInteger, adParamReturnValue)
        .Parameters.Append .CreateParameter("@Description", adVarChar, adParamInput, 30, strDescription)
        .Parameters.Append .CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open(CONST_PSYQUEL_CNN)
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute , , adExecuteNoRecords

    Insert = cmdSQL.Parameters("@BillStatusID").Value
    
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
Public Sub Update(ByVal lngID As Long, ByVal strDescription As String)
'--------------------------------------------------------------------
'Date: 08/30/2000
'Author: Eric Pena
'Description:  Updates a row in the tblBillStatus table utilizin a stored procedure.
'Parameters: Each parameter identifies the column value that will be inserted.
'Returns: Null
'--------------------------------------------------------------------
    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command

    On Error GoTo ErrTrap:

    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    
    With cmdSQL
        .CommandText = "uspUpdBillStatus"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@CPCID", adInteger, adParamInput, , lngID)
        .Parameters.Append .CreateParameter("@Description", adVarChar, adParamInput, 30, strDescription)
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
    Call RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, cmdSQL.Parameters("@ErrorMsg").Value)
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    GetObjectContext.SetAbort
End Sub
Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Long, ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 08/30/2000
'Author: Eric Pena
'Description:  Flags a row in the tblBillStatus table marking the row as
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
    Dim strSQL As String
    Dim blnFlag As String

    On Error GoTo ErrTrap:

    If blnDeleted = True Then
        blnFlag = "Y"
        'Prepare the SQL statement
        strSQL = "UPDATE "
        strSQL = strSQL & TABLE_NAME
        strSQL = strSQL & " SET "
        strSQL = strSQL & " fldDisabledYN = '" & blnFlag & "', "
        strSQL = strSQL & " fldDateDisabled = '" & Now & "', "
        strSQL = strSQL & " fldDisabledBy = '" & strUserName & "' "
        strSQL = strSQL & "WHERE "
        strSQL = strSQL & "fldBillStatusID = " & lngID
    Else
        blnFlag = "N"
        'Prepare the SQL statement
        strSQL = "UPDATE "
        strSQL = strSQL & TABLE_NAME
        strSQL = strSQL & " SET "
        strSQL = strSQL & " fldDisabledYN = '" & blnFlag & "' "
        strSQL = strSQL & "WHERE "
        strSQL = strSQL & "fldBillStatusID = " & lngID
    End If

    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    cmdSQL.CommandText = strSQL
    cmdSQL.CommandType = adCmdText
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open(CONST_PSYQUEL_CNN)
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute , , adExecuteNoRecords

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
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub
Public Function Exists(ByVal strDescription As String) As Boolean
'--------------------------------------------------------------------
'Date: 08/30/2000
'Author: Eric Pena
'Description:  Determines if a CPC containing the address parameters already exists in the table.
'Parameters: All parameters are address fields to check
'Returns: True if the CPC exists, false otherwise
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------
    Dim cnnSQL As ADODB.Connection
    Dim rstSQL As ADODB.Recordset
    Dim strSQL As String
    Dim blnExists As Boolean

    On Error GoTo ErrTrap:
    
    'Prepare the SQL statement
    strSQL = "SELECT "
    strSQL = strSQL & "COUNT(*) AS TOTAL "
    strSQL = strSQL & "FROM "
    strSQL = strSQL & TABLE_NAME
    strSQL = strSQL & " WHERE "
    strSQL = strSQL & " fldDescription = '" & strDescription & "'"
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open(CONST_PSYQUEL_CNN)

    'Instantiate and populate the Recordset.
    Set rstSQL = New ADODB.Recordset
    rstSQL.CursorLocation = adUseClient
    Call rstSQL.Open(strSQL, cnnSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)

    'Disconnect the recordset, close the connection and return the recordset
    'to the calling environment.
    Set rstSQL.ActiveConnection = Nothing

    'Close the connection and free all resources
    cnnSQL.Close
    Set cnnSQL = Nothing
    
    'Get the value from the Recordset then destroy it.
    If rstSQL.Fields("TOTAL").Value > 0 Then
        Exists = True
    Else
        Exists = False
    End If
    Set rstSQL = Nothing

    'Signal successful completion
    GetObjectContext.SetComplete

   Exit Function

ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set cnnSQL = Nothing
    Set rstSQL = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Public Function Fetch(Optional blnIncludeDisabled As Boolean = False) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 08/30/2000
'Author: Eric Pena
'Description:  Returns rows from the tblPlan, tblBillStatus, and tblInsCompany tables utilizing a stored procedure
'Parameters: blnIncludeDisabled - determines if disabled records will be returned
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
         .CommandText = "uspSelBillStatus"
         .CommandType = adCmdStoredProc
         .Parameters.Append .CreateParameter("@ShowDisabled", adVarChar, adParamInput, 1, IIf(blnIncludeDisabled = True, "Y", "N"))
         .Parameters.Append .CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0)
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
     Call RaiseError(Err, cmd.Parameters("@ErrorNum").Value, CLASS_NAME)
     Set cmd = Nothing
     Set rs = Nothing
     Set cnn = Nothing
     GetObjectContext.SetAbort
 End Function
Public Function FetchByID(lngID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 08/30/2000
'Author: Eric Pena
'Description:  Returns a row from tblBillStatus utilizing a stored procedure
'Parameters: lngID - the PK of the row to return
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
         .CommandText = "uspSelBillStatusByID"
         .CommandType = adCmdStoredProc
         .Parameters.Append .CreateParameter("@ID", adInteger, adParamInput, , lngID)
         .Parameters.Append .CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0)
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

