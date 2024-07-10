VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CRemitErrDB"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CInsTypeDB
'Date: 08/06/2004
'Author: Duane C Orth
'Description:  MTS object designed to host methods associated with
'              data affecting the tblInsuranceType table.
'--------------------------------------------------------------------
'
'--------------------------------------------------------------------

Option Explicit
Private Const CLASS_NAME As String = "CRemitErrDB"
Private Const TABLE_NAME As String = "tblRemittanceErrorCodes"

Public Function Fetch(Optional blnIncludeDisabled As Boolean = False, _
                      Optional ByVal strWhere As String, _
                      Optional ByVal strOrderBy As String) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 04/20/2009
'Author: Duane C Orth
'Description:  Retrieves records from the tblRemittanceErrorCodes table.
'Parameters: blnIncludeDisabled - Optional parameter that identifies if
'               records flagged as 'Disabled' or 'De-activated' are to be
'               included in the record set. The default value is False.
'            strWhere - Optional 'Where' clause of the SQL statement by
'               which records will be filtered.  If the parameter is not
'               supplied, all records will be retrieved.
'            strOrderBy - Optional 'Order By' clause in which retrieved
'               records will be sorted.  The default is set to the
'               'fldName' column
'Returns: ADODB.Recordset
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------
    
    Dim cnnSQL As ADODB.Connection
    Dim rstSQL As ADODB.Recordset
    Dim strDfltWhere As String
    Dim strSQL As String

    On Error GoTo ErrTrap:

    'Prepare the SQL statement.
    strSQL = "SELECT "
    strSQL = strSQL & TABLE_NAME & ".* "
    strSQL = strSQL & "FROM "
    strSQL = strSQL & TABLE_NAME & " "
    
    'Determine whether to include 'disabled' records'
    If blnIncludeDisabled = False Then
        strDfltWhere = " WHERE fldDisabledYN = 'N' "
    End If
    
    'Attach user's where clause if applicable
    strSQL = strSQL & strDfltWhere
    
    If Trim(strWhere > "") Then If blnIncludeDisabled Then strSQL = strSQL & " WHERE " & strWhere & " " Else strSQL = strSQL & " AND " & strWhere & " "
    
    If Trim(strOrderBy) > "" Then
        strSQL = strSQL & " ORDER BY "
        strSQL = strSQL & strOrderBy
    Else
        strSQL = strSQL & " ORDER BY "
        strSQL = strSQL & "fldInsName "
    End If


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
    
    cnnSQL.Close
    Set cnnSQL = Nothing
    Set Fetch = rstSQL
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set rstSQL.ActiveConnection = Nothing
    Set rstSQL = Nothing
    Set cnnSQL = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

Public Function Insert(ByVal strErrorCode As String, _
                       ByVal strDescription As String, _
                       ByVal blnPostError As Boolean, _
                       ByVal blnPostAmount As Boolean, _
                       ByVal blnCoInsAmount As Boolean, _
                       ByVal lngTxTypeID As Long, _
                       ByVal lngBillStatusID As Long, _
                       ByVal strAddedBy As String) As Long
'--------------------------------------------------------------------
'Date: 08/06/2004
'Author: Duane C Orth
'Description:  Inserts a row into the tblRemittanceErrorCodes table utilizing
'              a stored procedure.
'Parameters: Each parameter identifies the column value that will be
'              inserted.
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
        .CommandText = "uspInsRemittanceErrorCode"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@RemittanceErrorRecordID", adInteger, adParamReturnValue)
        .Parameters.Append .CreateParameter("@ErrorCode", adVarChar, adParamInput, 10, strErrorCode)
        .Parameters.Append .CreateParameter("@Description", adVarChar, adParamInput, 250, strDescription)
        .Parameters.Append .CreateParameter("@PostErrorYN", adVarChar, adParamInput, 1, IIf(blnPostError = True, "Y", "N"))
        .Parameters.Append .CreateParameter("@PostAmountYN", adVarChar, adParamInput, 1, IIf(blnPostAmount = True, "Y", "N"))
        .Parameters.Append .CreateParameter("@CoInsAmountYN", adVarChar, adParamInput, 1, IIf(blnCoInsAmount = True, "Y", "N"))
        .Parameters.Append .CreateParameter("@TxTypeID", adInteger, adParamInput, , lngTxTypeID)
        .Parameters.Append .CreateParameter("@BillStatusID", adInteger, adParamInput, , lngBillStatusID)
        .Parameters.Append .CreateParameter("@AddedBy", adVarChar, adParamInput, 20, strAddedBy)
        .Parameters.Append .CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open(CONST_PSYQUEL_CNN)
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute , , adExecuteNoRecords

    Insert = cmdSQL.Parameters("@RemittanceErrorRecordID").Value
    
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
    Call RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME)
    
End Function
Public Sub Update(ByVal lngID As Long, _
                  ByVal strErrorCode As String, _
                  ByVal strDescription As String, _
                  ByVal blnPostError As Boolean, _
                  ByVal blnPostAmount As Boolean, _
                  ByVal blnCoInsAmount As Boolean, _
                  ByVal lngTxTypeID As Long, _
                  ByVal lngBillStatusID As Long, _
                  ByVal strChangedBy As String)
'--------------------------------------------------------------------
'Date: 08/06/2004
'Author: Duane C Orth
'Description:  Updates a row in the tblRemittanceErrorCodes table utilizing
'              a stored procedure.
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'Returns: Null
'--------------------------------------------------------------------

    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command
    
    On Error GoTo ErrTrap:
    
    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    
    With cmdSQL
        .CommandText = "uspUpdRemittanceErrorCode"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@RemittanceErrorRecordID", adInteger, adParamInput, , lngID)
        .Parameters.Append .CreateParameter("@ErrorCode", adVarChar, adParamInput, 10, strErrorCode)
        .Parameters.Append .CreateParameter("@Description", adVarChar, adParamInput, 250, strDescription)
        .Parameters.Append .CreateParameter("@PostErrorYN", adVarChar, adParamInput, 1, IIf(blnPostError = True, "Y", "N"))
        .Parameters.Append .CreateParameter("@PostAmountYN", adVarChar, adParamInput, 1, IIf(blnPostAmount = True, "Y", "N"))
        .Parameters.Append .CreateParameter("@CoInsAmountYN", adVarChar, adParamInput, 1, IIf(blnCoInsAmount = True, "Y", "N"))
        .Parameters.Append .CreateParameter("@TxTypeID", adInteger, adParamInput, , lngTxTypeID)
        .Parameters.Append .CreateParameter("@BillStatusID", adInteger, adParamInput, , lngBillStatusID)
        .Parameters.Append .CreateParameter("@ChangedBy", adVarChar, adParamInput, 20, strChangedBy)
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
    Call RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, cmdSQL.Parameters("@ErrorMsg").Value)
    
End Sub
Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Long, ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 08/06/2004
'Author: Duane C Orth
'Description:  Flags a row in the tblRemittanceErrorCodes table marking the row as
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
    Dim strSQL2 As String
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
        strSQL = strSQL & "fldRemittanceErrorRecordID = " & lngID
    Else
        blnFlag = "N"
        'Prepare the SQL statement
        strSQL = "UPDATE "
        strSQL = strSQL & TABLE_NAME
        strSQL = strSQL & " SET "
        strSQL = strSQL & " fldDisabledYN = '" & blnFlag & "' "
        strSQL = strSQL & "WHERE "
        strSQL = strSQL & "fldRemittanceErrorRecordID = " & lngID
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
Public Function Exists(strErrorCode As String) As Boolean
'--------------------------------------------------------------------
'Date: 08/06/2004
'Author: Duane C Orth
'Description:  Determines if a Plan Name identical to the
'              strInsType Name parameter already exists in the table.
'Parameters: strInsType - Plan name to be checked
'Returns: True if the name exists, false otherwise
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
    strSQL = strSQL & " fldRemittanceErrorCode = '" & Trim(strErrorCode) & "' "

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
    Set rstSQL = Nothing
    Set cnnSQL = Nothing
    
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function



