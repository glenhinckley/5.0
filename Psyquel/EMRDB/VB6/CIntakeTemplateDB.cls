VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CIntakeTemplateDB"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CIntakeTemplateDB
'Date: 05/25/2020
'Author: Duane C Orth
'Description:  Database object designed to implement methods (by calling
'              stored procedures) associated with
'              data affecting the tblProviderIntakeTemplate and
'              tblIntakeTemplateItem table.
'--------------------------------------------------------------------
'  Revisions:
'
'--------------------------------------------------------------------
Option Explicit

Private Const CLASS_NAME As String = "CIntakeTemplateDB"
Private Const TABLE_NAME As String = "tblProviderIntakeTemplate"
Public Function FetchByID(ByVal lngTemplateID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 05/25/2020
'Author: Duane C Orth
'Description: Retrieves records from the tblProviderIntakeTemplate table.  Calls
'               stored procedure uspSelTemplateByID.
'Parameters: lngTemplateID - 'Where' clause of the SQL statement by
'               which records will be fetched.  Matches tblProviderIntakeTemplate's
'               primary key.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------
'   --R001

    Dim cnn As ADODB.Connection ' Declare DB connection
    Dim rst As ADODB.Recordset  ' Declare recordset
    Dim cmd As ADODB.Command    ' Declare SQL statement

    On Error GoTo ErrTrap:

    Set cmd = New ADODB.Command     ' Instantiate SQL statement
    With cmd
        .CommandText = "uspSelIntakeTemplateByID"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@TemplateID", adInteger, adParamInput, , lngTemplateID)
    End With
        
    Set cnn = New ADODB.Connection  ' Instantiate DB connection
    cnn.Open (CONST_PSYQUEL_CNN) ' open DB connection
    Set cmd.ActiveConnection = cnn
    
    Set rst = New ADODB.Recordset   ' Instantiate recordset
    rst.CursorLocation = adUseClient
    rst.Open cmd, , adOpenForwardOnly, adLockReadOnly   ' open recordset

    Set cmd.ActiveConnection = Nothing
    Set rst.ActiveConnection = Nothing
    cnn.Close   ' close DB connection
    Set cnn = Nothing
    Set cmd = Nothing
    
    Set FetchByID = rst ' Set return value
   
    GetObjectContext.SetComplete
    Exit Function
    
ErrTrap:
    GetObjectContext.SetAbort
    
    cnn.Close   ' close DB connection
    Set cmd.ActiveConnection = Nothing
    rst.Close   ' close the recordset --R003
    Set rst.ActiveConnection = Nothing
    
    Set rst = Nothing
    Set cmd = Nothing
    Set cnn = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Public Function FetchByItemID(ByVal lngItemID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 05/25/2020
'Author: Duane C Orth
'Description: Retrieves records from the tblTemplateQuestion table.  Calls
'               stored procedure uspSelTemplateItemByID.
'Parameters: lngItemID - 'Where' clause of the SQL statement by
'               which records will be fetched.  Matches tblTemplateQuestion's
'               primary key.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------
'   --R001

    ' Declare connection, returned recordset, and command
    Dim cnn As ADODB.Connection ' Declare DB connection
    Dim rst As ADODB.Recordset  ' Declare recordset
    Dim cmd As ADODB.Command    ' Declare SQL statement

    On Error GoTo ErrTrap:

    'Create the parameter objects
    Set cmd = New ADODB.Command     ' Instantiate SQL statement
    With cmd
        .CommandText = "uspSelIntakeTemplateItemByID"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@ItemID", adInteger, adParamInput, , lngItemID)
    End With
    
    'Acquire the database connection.
    Set cnn = New ADODB.Connection  ' Instantiate DB connection
    cnn.Open (CONST_PSYQUEL_CNN) ' open DB connection
    Set cmd.ActiveConnection = cnn
    
    'Set cursor and execute
    Set rst = New ADODB.Recordset   ' Instantiate recordset
    rst.CursorLocation = adUseClient
    rst.Open cmd, , adOpenForwardOnly, adLockReadOnly   ' open recordset

    
    ' Cannot close recordset here...close by client
    
    ' De-allocate space for objects
    Set cmd.ActiveConnection = Nothing
    Set rst.ActiveConnection = Nothing
    cnn.Close   ' close DB connection
    Set cnn = Nothing
    Set cmd = Nothing
    
    Set FetchByItemID = rst ' Set return value
   
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    
    cnn.Close   ' close DB connection
    Set cmd.ActiveConnection = Nothing
    rst.Close   ' close the recordset --R003
    Set rst.ActiveConnection = Nothing
    
    ' De-allocate space for objects
    Set rst = Nothing
    Set cmd = Nothing
    Set cnn = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Public Function FetchByProvider(ByVal lngProviderID As Long, Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 05/25/2020
'Author: Dave Nichol and Duane C Orth
'Description:  Retrieves records from the tblProviderIntakeTemplate table. Calls
'               stored procedure uspSelTemplateByUser
'Parameters: lngUserID - UserID of user calling method.
'            blnIncludeDisabled - boolean value -- whether to include
'            disabled rows (true) or not (false).
'Returns: ADODB.Recordset
'--------------------------------------------------------------------

    ' Declare connection, returned recordset, and command
    Dim cnn As ADODB.Connection ' Declare DB connection
    Dim rst As ADODB.Recordset  ' Declare recordset
    Dim cmd As ADODB.Command    ' Declare SQL statement

    On Error GoTo ErrTrap:
    
    'Create the parameter objects
    Set cmd = New ADODB.Command     ' Instantiate SQL statement --R003
    With cmd
        .CommandText = "uspSelIntakeTemplateByProvider"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID)
        .Parameters.Append .CreateParameter("@IncludeDisabledYN", adVarChar, adParamInput, 1, IIf(blnIncludeDisabled = False, "N", "Y"))
    End With
    
    Set cnn = New ADODB.Connection  ' Instantiate DB connection --R003
    cnn.Open (CONST_PSYQUEL_CNN) ' open DB connection
    Set cmd.ActiveConnection = cnn

    Set rst = New ADODB.Recordset   ' Instantiate recordset --R003
    rst.CursorLocation = adUseClient
    rst.Open cmd, , adOpenForwardOnly, adLockReadOnly   ' open recordset
    
    Set cmd.ActiveConnection = Nothing
    Set rst.ActiveConnection = Nothing
    cnn.Close   ' close DB connection
    Set cmd = Nothing
    Set cnn = Nothing
    
    Set FetchByProvider = rst    ' Set return value
    GetObjectContext.SetComplete

    Exit Function
    
ErrTrap:
    GetObjectContext.SetAbort
    cnn.Close   ' close DB connection
    Set cmd.ActiveConnection = Nothing
    rst.Close   ' close the recordset --R003
    Set rst.ActiveConnection = Nothing
    
    Set rst = Nothing
    Set cmd = Nothing
    Set cnn = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Public Function InsertProviderIntakeTemplate(ByVal lngUserID As Long, ByVal strTemplateName As String, _
                       ByVal strTemplateDesc As String, ByVal strUserName As String) As Long
'--------------------------------------------------------------------
'Date: 05/25/2020
'Author: Duane C Orth
'Description:  Inserts a row into the tblProviderIntakeTemplate table utilizing
'              a stored procedure.
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'   lngUserID strTemplateName strTemplateDesc strUserName SQLErrorNum
'Returns: ID (Primary Key) of the row inserted.
'--------------------------------------------------------------------
' R002 -

    Dim cnn As ADODB.Connection ' Declare DB connection
    Dim cmd As ADODB.Command    ' Declare SQL statement

    On Error GoTo ErrTrap:

    Set cmd = New ADODB.Command     ' Instantiate SQL statement --R003
    With cmd
        .CommandText = "uspInsProviderIntakeTemplate"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@TemplateID", adInteger, adParamReturnValue)
        .Parameters.Append .CreateParameter("@UserID", adInteger, adParamInput, , lngUserID)
        .Parameters.Append .CreateParameter("@TemplateName", adVarChar, adParamInput, 30, strTemplateName)
        .Parameters.Append .CreateParameter("@TemplateDesc", adVarChar, adParamInput, 128, strTemplateDesc)
        .Parameters.Append .CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strUserName)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    Set cnn = New ADODB.Connection  ' Instantiate DB connection
    Call cnn.Open(CONST_PSYQUEL_CNN)  ' open DB connection
    Set cmd.ActiveConnection = cnn
    cmd.Execute ', , adExecuteNoRecords

    InsertProviderIntakeTemplate = cmd.Parameters("@TemplateID").Value
    
    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If
        
    cnn.Close   ' close DB connection
    Set cmd.ActiveConnection = Nothing 'Close ADO stuff explicitly --R003
    Set cmd = Nothing
    Set cnn = Nothing
    
    GetObjectContext.SetComplete

    Exit Function
    
ErrTrap:
    GetObjectContext.SetAbort
    
    cnn.Close   ' --R003
    Set cmd.ActiveConnection = Nothing
   
    Set cmd = Nothing
    Set cnn = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Public Function InsertIntakeTemplateItem(ByVal lngTemplateID As Long, _
                ByVal strItemText As String, ByVal strPossibleAnswers As String, _
                ByVal strDisplayText As String, ByVal strType As String, _
                ByVal strHTMLName As String, ByVal lngItemNumber As Long, _
                ByVal lngRows As Long, ByVal lngColumn As Long, _
                ByVal lngChars As Long, ByVal strBold As String, _
                ByVal strUnderline As String, ByVal strItalic As String, _
                ByVal strAlignment As String, ByVal strIndent As String, _
                ByVal strCommitted As String, ByVal strUserName As String) As Long
'--------------------------------------------------------------------
'Date: 05/25/2020
'Author: Duane C Orth
'Description:  Inserts a row into the tblIntakeTemplateItem table utilizing
'              a stored procedure.  New with revision R004
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'Returns: ID (Primary Key) of the row inserted.
'--------------------------------------------------------------------
' R004 - Added with new revision

    ' Declare connection, returned recordset, and command
    Dim cnn As ADODB.Connection ' Declare DB connection
    Dim cmd As ADODB.Command    ' Declare SQL statement

    On Error GoTo ErrTrap:

    'Create the parameter objects
    Set cmd = New ADODB.Command     ' Instantiate SQL statement
    With cmd
        .CommandText = "uspInsIntakeTemplateItem"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@TemplateID", adInteger, adParamInput, , lngTemplateID)
        .Parameters.Append .CreateParameter("@ItemText", adVarChar, adParamInput, 200, strItemText)
        .Parameters.Append .CreateParameter("@PossibleAnswers", adVarChar, adParamInput, 200, strPossibleAnswers)
        .Parameters.Append .CreateParameter("@DisplayText", adVarChar, adParamInput, 350, strDisplayText)
        .Parameters.Append .CreateParameter("@Type", adVarChar, adParamInput, 1, strType)
        .Parameters.Append .CreateParameter("@HTMLName", adVarChar, adParamInput, 20, strHTMLName)
        .Parameters.Append .CreateParameter("@ItemNumber", adInteger, adParamInput, , lngItemNumber)
        .Parameters.Append .CreateParameter("@Rows", adInteger, adParamInput, , lngRows)
        .Parameters.Append .CreateParameter("@Column", adInteger, adParamInput, , lngColumn)
        .Parameters.Append .CreateParameter("@Chars", adInteger, adParamInput, , lngChars)
        .Parameters.Append .CreateParameter("@Bold", adVarChar, adParamInput, 1, strBold)
        .Parameters.Append .CreateParameter("@Underline", adVarChar, adParamInput, 1, strUnderline)
        .Parameters.Append .CreateParameter("@Italic", adVarChar, adParamInput, 1, strItalic)
        .Parameters.Append .CreateParameter("@Alignment", adVarChar, adParamInput, 1, strAlignment)
        .Parameters.Append .CreateParameter("@Indent", adVarChar, adParamInput, 1, strIndent)
        .Parameters.Append .CreateParameter("@CommittedYN", adVarChar, adParamInput, 1, strCommitted)
        .Parameters.Append .CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strUserName)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    Set cnn = New ADODB.Connection  ' Instantiate DB connection
    Call cnn.Open(CONST_PSYQUEL_CNN)  ' open DB connection
    Set cmd.ActiveConnection = cnn
    cmd.Execute , , adExecuteNoRecords

    If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If
        
    InsertIntakeTemplateItem = cmd.Parameters(0).Value
    
    cnn.Close   ' close DB connection
    Set cmd.ActiveConnection = Nothing 'Close ADO stuff explicitly
    Set cmd = Nothing
    Set cnn = Nothing
    
    GetObjectContext.SetComplete

    Exit Function
    
ErrTrap:
    GetObjectContext.SetAbort
    
    cnn.Close
    Set cmd.ActiveConnection = Nothing
   
    Set cmd = Nothing
    Set cnn = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Public Sub UpdateProviderIntakeTemplate(ByVal lngTemplateID As Long, _
        ByVal strTemplateName As String, ByVal strTemplateDesc As String)
'--------------------------------------------------------------------
'Date: 05/25/2020
'Author: Duane C Orth
'Description: Updates selected records in the tblProviderIntakeTemplate table.  Calls
'               stored procedure uspUpdProviderIntakeTemplate.
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'   lngTemplateID strTemplateName strTemplateDesc SQLErrorNum
'Returns: Null
'--------------------------------------------------------------------

    Dim cnn As ADODB.Connection ' Declare DB connection
    Dim cmd As ADODB.Command    ' Declare SQL statement

    On Error GoTo ErrTrap:

    Set cmd = New ADODB.Command
    With cmd
        .CommandText = "uspUpdProviderIntakeTemplate"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@TemplateID", adInteger, adParamInput, , lngTemplateID)
        .Parameters.Append .CreateParameter("@TemplateName", adVarChar, adParamInput, 30, strTemplateName)
        .Parameters.Append .CreateParameter("@TemplateDesc", adVarChar, adParamInput, 128, strTemplateDesc)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
   
    Set cnn = New ADODB.Connection  ' Instantiate DB connection --R003
    cnn.Open (CONST_PSYQUEL_CNN) ' open DB connection
    Set cmd.ActiveConnection = cnn
    cmd.Execute , , adExecuteNoRecords

    If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If
        
    Set cmd.ActiveConnection = Nothing
    cnn.Close   ' close DB connection
    Set cmd = Nothing
    Set cnn = Nothing
    
    GetObjectContext.SetComplete

    Exit Sub
    
ErrTrap:
    GetObjectContext.SetAbort
    
    Set cmd.ActiveConnection = Nothing
    cnn.Close
    Set cmd = Nothing
    Set cnn = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub
Public Sub UpdateIntakeTemplateItem(ByVal lngTemplateID As Long, ByVal lngItemID As Long, _
                ByVal strItemText As String, ByVal strPossibleAnswers As String, _
                ByVal strDisplayText As String, ByVal strType As String, _
                ByVal strHTMLName As String, ByVal lngItemNumber As Long, _
                ByVal lngRows As Long, ByVal lngColumn As Long, _
                ByVal lngChars As Long, ByVal strBold As String, _
                ByVal strUnderline As String, ByVal strItalic As String, _
                ByVal strAlignment As String, ByVal strIndent As String, _
                ByVal strCommitted As String)
'--------------------------------------------------------------------
'Date: 05/25/2020
'Author: Duane C Orth
'Description: Updates selected records in the tblTemplateItem table.  Calls
'               stored procedure uspUpdTemplateItem.  New with revision R004
'Parameters: Each parameter identifies the column value that will be
'              updated.
'
'Returns: Null
'--------------------------------------------------------------------

    ' Declare connection, returned recordset, and command
    Dim cnn As ADODB.Connection ' Declare DB connection
    Dim cmd As ADODB.Command    ' Declare SQL statement

    On Error GoTo ErrTrap:

    'Create the parameter objects
    Set cmd = New ADODB.Command     ' Instantiate SQL statement --R002
    With cmd
        .CommandText = "uspUpdIntakeTemplateItem"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@ItemID", adInteger, adParamInput, , lngItemID)
        .Parameters.Append .CreateParameter("@TemplateID", adInteger, adParamInput, , lngTemplateID)
        .Parameters.Append .CreateParameter("@ItemText", adVarChar, adParamInput, 200, strItemText)
        .Parameters.Append .CreateParameter("@strPossibleAnswers", adVarChar, adParamInput, 200, strPossibleAnswers)
        .Parameters.Append .CreateParameter("@strDisplayText", adVarChar, adParamInput, 350, strDisplayText)
        .Parameters.Append .CreateParameter("@strType", adVarChar, adParamInput, 1, strType)
        .Parameters.Append .CreateParameter("@strHTMLName", adVarChar, adParamInput, 20, strHTMLName)
        .Parameters.Append .CreateParameter("@ItemNumber", adInteger, adParamInput, , lngItemNumber)
        .Parameters.Append .CreateParameter("@Rows", adInteger, adParamInput, , lngRows)
        .Parameters.Append .CreateParameter("@Column", adInteger, adParamInput, , lngColumn)
        .Parameters.Append .CreateParameter("@Chars", adInteger, adParamInput, , lngChars)
        .Parameters.Append .CreateParameter("@Bold", adVarChar, adParamInput, 1, strBold)
        .Parameters.Append .CreateParameter("@Underline", adVarChar, adParamInput, 1, strUnderline)
        .Parameters.Append .CreateParameter("@Italic", adVarChar, adParamInput, 1, strItalic)
        .Parameters.Append .CreateParameter("@Alignment", adVarChar, adParamInput, 1, strAlignment)
        .Parameters.Append .CreateParameter("@Indent", adVarChar, adParamInput, 1, strIndent)
        .Parameters.Append .CreateParameter("@CommittedYN", adVarChar, adParamInput, 1, strCommitted)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
   
    'Assign the connection to the Command object and execute the stored procedure
    Set cnn = New ADODB.Connection  ' Instantiate DB connection --R003
    cnn.Open (CONST_PSYQUEL_CNN) ' open DB connection
    Set cmd.ActiveConnection = cnn
    cmd.Execute , , adExecuteNoRecords

    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If
        
    ' De-allocate space for objects
    Set cmd.ActiveConnection = Nothing
    cnn.Close   ' close DB connection
    Set cmd = Nothing
    Set cnn = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    
    Set cmd.ActiveConnection = Nothing
    cnn.Close
    Set cmd = Nothing
    Set cnn = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub
Public Sub UpdateItemNumber(ByVal lngTemplateID As Long, ByVal lngOldItemNumber As Long, _
                            ByVal lngNewItemNumber As Long)
'--------------------------------------------------------------------
'Date: 11/02/2000
'Author: Duane C Orth
'Description: Updates value of ItemNumber in tblTemplateItem for
'              the selected ItemID.
'Parameters: Each parameter identifies the column value that will be
'              updated.
'Returns: Null
'--------------------------------------------------------------------

    ' Declare connection, returned recordset, and command
    Dim cnn As ADODB.Connection ' Declare DB connection
    Dim cmd As ADODB.Command    ' Declare SQL statement

    On Error GoTo ErrTrap:

    'Create the parameter objects
    Set cmd = New ADODB.Command     ' Instantiate SQL statement --R002
    With cmd
        .CommandText = "uspUpdTemplateItemOrder"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@TemplateID", adInteger, adParamInput, , lngTemplateID)
        .Parameters.Append .CreateParameter("@OldItemNumber", adInteger, adParamInput, , lngOldItemNumber)
        .Parameters.Append .CreateParameter("@NewItemNumber", adInteger, adParamInput, , lngNewItemNumber)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
   
    'Assign the connection to the Command object and execute the stored procedure
    Set cnn = New ADODB.Connection  ' Instantiate DB connection --R003
    cnn.Open (CONST_PSYQUEL_CNN) ' open DB connection
    Set cmd.ActiveConnection = cnn
    cmd.Execute , , adExecuteNoRecords

    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If
        
    ' De-allocate space for objects
    Set cmd.ActiveConnection = Nothing
    cnn.Close   ' close DB connection
    Set cmd = Nothing
    Set cnn = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    
    Set cmd.ActiveConnection = Nothing
    cnn.Close
    Set cmd = Nothing
    Set cnn = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub
Public Sub CommitTemplate(ByVal lngTemplateID As Long)
'--------------------------------------------------------------------
'Date: 05/25/2020
'Author: Duane C Orth
'Description: Commits Items associated with template ID.
'   New with revision R004
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'
'Returns: Null
'--------------------------------------------------------------------

    ' Declare connection, returned recordset, and command
    Dim cnn As ADODB.Connection ' Declare DB connection
    Dim cmd As ADODB.Command    ' Declare SQL statement

    On Error GoTo ErrTrap:

    'Create the parameter objects
    Set cmd = New ADODB.Command     ' Instantiate SQL statement --R002
    With cmd
        .CommandText = "uspUpdCommitTemplate"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@TemplateID", adInteger, adParamInput, , lngTemplateID)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
   
    'Assign the connection to the Command object and execute the stored procedure
    Set cnn = New ADODB.Connection  ' Instantiate DB connection --R003
    cnn.Open (CONST_PSYQUEL_CNN) ' open DB connection
    Set cmd.ActiveConnection = cnn
    cmd.Execute , , adExecuteNoRecords

    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If
        
    ' De-allocate space for objects
    Set cmd.ActiveConnection = Nothing
    cnn.Close   ' close DB connection
    Set cmd = Nothing
    Set cnn = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    
    Set cmd.ActiveConnection = Nothing
    cnn.Close
    Set cmd = Nothing
    Set cnn = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub

Public Sub CopyTemplate(ByVal lngProviderID As Long, ByVal lngTemplateID As Long)
'--------------------------------------------------------------------

    ' Declare connection, returned recordset, and command
    Dim cnn As ADODB.Connection ' Declare DB connection
    Dim cmd As ADODB.Command    ' Declare SQL statement

    On Error GoTo ErrTrap:
    
    'Create the parameter objects
    Set cmd = New ADODB.Command     ' Instantiate SQL statement --R002
    With cmd
        .CommandText = "uspCopyNoteTemplate"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID)
        .Parameters.Append .CreateParameter("@TemplateID", adInteger, adParamInput, , lngTemplateID)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cnn = New ADODB.Connection  ' Instantiate DB connection --R002
    cnn.Open (CONST_PSYQUEL_CNN) ' open DB connection
    Set cmd.ActiveConnection = cnn
    cmd.Execute , , adExecuteNoRecords

    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If
        
    ' De-allocate space for objects
    cnn.Close   ' close DB connection
    Set cmd.ActiveConnection = Nothing
    Set cmd = Nothing
    Set cnn = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    
    ' De-allocate space for objects
    cnn.Close   ' close DB connection
    Set cmd.ActiveConnection = Nothing
    Set cmd = Nothing
    Set cnn = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub

Public Sub PurgeTemplate(ByVal lngTemplateID As Long)
'--------------------------------------------------------------------
'Date: 05/25/2020
'Author: Duane C Orth
'Description:  Deletes all the Items that have not been committed
'Parameters: lngTemplateID - ID of the row in the table whose value will be
'               deleted.
'Returns: Null
'--------------------------------------------------------------------

    ' Declare connection, returned recordset, and command
    Dim cnn As ADODB.Connection ' Declare DB connection
    Dim cmd As ADODB.Command    ' Declare SQL statement

    On Error GoTo ErrTrap:
    
    'Create the parameter objects
    Set cmd = New ADODB.Command     ' Instantiate SQL statement --R002
    With cmd
        .CommandText = "uspDelPurgeTemplate"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@TemplateID", adInteger, adParamInput, , lngTemplateID)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cnn = New ADODB.Connection  ' Instantiate DB connection --R002
    cnn.Open (CONST_PSYQUEL_CNN) ' open DB connection
    Set cmd.ActiveConnection = cnn
    cmd.Execute , , adExecuteNoRecords

    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If
        
    ' De-allocate space for objects
    cnn.Close   ' close DB connection
    Set cmd.ActiveConnection = Nothing
    Set cmd = Nothing
    Set cnn = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    
    ' De-allocate space for objects
    cnn.Close   ' close DB connection
    Set cmd.ActiveConnection = Nothing
    Set cmd = Nothing
    Set cnn = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub

Public Sub DeleteProviderIntakeTemplate(ByVal lngTemplateID As Long, Optional ByVal blnDisabled As Boolean = False)
'--------------------------------------------------------------------
'Date: 05/25/2020
'Author: Duane C Orth
'Description:  Flags a row in the tblProviderIntakeTemplate table marking the row as
'              deleted or undeleted.  Calls stored procedure uspDelTemplate
'Parameters: lngTemplateID - ID of the row in the table whose value will be
'               deleted.
'            blnIncludeDisabled - boolean value -- whether to include
'            disabled rows (true) or not (false).
'Returns: Null
'--------------------------------------------------------------------

    ' Declare connection, returned recordset, and command
    Dim cnn As ADODB.Connection ' Declare DB connection
    Dim cmd As ADODB.Command    ' Declare SQL statement

    On Error GoTo ErrTrap:
    
    'Create the parameter objects
    Set cmd = New ADODB.Command     ' Instantiate SQL statement --R002
    With cmd
        .CommandText = "uspDelTemplate"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@TemplateID", adInteger, adParamInput, , lngTemplateID)
        .Parameters.Append .CreateParameter("@DisabledYN", adVarChar, adParamInput, 1, IIf(blnDisabled = False, "N", "Y"))
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cnn = New ADODB.Connection  ' Instantiate DB connection --R002
    cnn.Open (CONST_PSYQUEL_CNN) ' open DB connection
    Set cmd.ActiveConnection = cnn
    cmd.Execute , , adExecuteNoRecords

    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If
        
    ' De-allocate space for objects
    cnn.Close   ' close DB connection
    Set cmd.ActiveConnection = Nothing
    Set cmd = Nothing
    Set cnn = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    
    ' De-allocate space for objects
    cnn.Close   ' close DB connection
    Set cmd.ActiveConnection = Nothing
    Set cmd = Nothing
    Set cnn = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub


Public Sub DeleteIntakeTemplateItem(ByVal lngItemID As Long)
'--------------------------------------------------------------------
'Date: 09/28/2000
'Author: Duane C Orth
'Description:  Deletes row from tblTemplateItem.  Calls stored procedure
'   uspDelTemplateItem
'Parameters: lngTemplateID - ID of the row in the table whose value will be
'               deleted.
'Returns: Null
'--------------------------------------------------------------------

    ' Declare connection, returned recordset, and command
    Dim cnn As ADODB.Connection ' Declare DB connection
    Dim cmd As ADODB.Command    ' Declare SQL statement

    On Error GoTo ErrTrap:
    
    'Create the parameter objects
    Set cmd = New ADODB.Command     ' Instantiate SQL statement --R002
    With cmd
        .CommandText = "uspDelTemplateItem"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@ItemID", adInteger, adParamInput, , lngItemID)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cnn = New ADODB.Connection  ' Instantiate DB connection --R002
    cnn.Open (CONST_PSYQUEL_CNN) ' open DB connection
    Set cmd.ActiveConnection = cnn
    cmd.Execute , , adExecuteNoRecords

    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If
        
    ' De-allocate space for objects
    cnn.Close   ' close DB connection
    Set cmd.ActiveConnection = Nothing
    Set cmd = Nothing
    Set cnn = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    
    ' De-allocate space for objects
    cnn.Close   ' close DB connection
    Set cmd.ActiveConnection = Nothing
    Set cmd = Nothing
    Set cnn = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub








