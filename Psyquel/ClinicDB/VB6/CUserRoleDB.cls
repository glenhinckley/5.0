VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CUserRoleDB"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CUserRoleDB                                            '
'Date: 01/04/2000                                                   '
'Author: Dave Richkun                                               '
'Description:  MTS object designed to allow insertions into the     '
'              tblUserRole table.                                   '
'--------------------------------------------------------------------

Option Explicit

Private Const TABLE_NAME As String = "tblUserRole"
Private Const CLASS_NAME As String = "CUserRoleIns"

Public Sub Insert(ByVal lngUserID As Long, ByVal varRoles As Variant)
'--------------------------------------------------------------------
'Date: 01/04/2000                                                   '
'Author: Dave Richkun                                               '
'Description:  Inserts one or more rows into the tblUserRole table. '
'Parameters: lngUserID - ID of the user who is being associated to  '
'               one or more Roles                                   '
'            varRoles - Variant array containing the IDs of each    '
'               Role to which the User is associated.               '
'Returns: Null                                                      '
'--------------------------------------------------------------------

    Dim cnn As ADODB.Connection
    Dim cmd As ADODB.Command
    Dim lngRoleID As Long
    Dim intCTR As Integer

    On Error GoTo ErrTrap:
    
    'Acquire the database connection.
    Set cnn = New ADODB.Connection
    Set cmd = New ADODB.Command
    Call cnn.Open(CONST_PSYQUEL_CNN)
    
    'Delete existing UserClinic rows before adding new rows
    With cmd
        .CommandText = "uspDelUserRole"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@UserID", adInteger, adParamInput, , lngUserID)
    End With
    
    'Instantiate and prepare the Command object.
    Set cmd.ActiveConnection = cnn
    cmd.Execute ', adExecuteNoRecords
    Set cmd = Nothing
    
    If UBound(varRoles) >= 0 Then
        While intCTR <= UBound(varRoles)
        
            lngRoleID = varRoles(intCTR)
            Set cmd = New ADODB.Command
            With cmd
                .CommandText = "uspInsUserRole"
                .CommandType = adCmdStoredProc
                .Parameters.Append .CreateParameter("@UserID", adInteger, adParamInput, , lngUserID)
                .Parameters.Append .CreateParameter("@RoleID", adInteger, adParamInput, , lngRoleID)
                .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
            End With
            
            'Assign the connection to the Command object and execute the stored procedure
            Set cmd.ActiveConnection = cnn
            cmd.Execute ', adExecuteNoRecords
        
            ' Check for those ungodly errors
            If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
                GoTo ErrTrap
            End If
            
            intCTR = intCTR + 1
        Wend
        
        'Close the connection and free all resources
        cnn.Close
    End If

EMPTY_ARRAY:
    Set cmd = Nothing
    Set cnn = Nothing

    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Sub
    
ErrTrap:
    'The Role array may be empty.
    If Err.Number = 9 Then
        GoTo EMPTY_ARRAY
    Else
        'Signal incompletion and raise the error to the calling environment.
        GetObjectContext.SetAbort
        Call RaiseError(Err, cmd.Parameters("@ErrorNum").Value, CLASS_NAME, cmd.Parameters("@ErrorMsg").Value)
    End If
End Sub

Public Function FetchRolesByUserID(ByVal lngUserID As Long) _
                    As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 02/29/2000                                                   '
'Author: Rick "Boom Boom" Segura                                    '
'Description:  Returns a variant array containing a list of all     '
'              roles to which a user is associated.                 '
'Parameters: lngUserID - The ID of the user                         '
'Returns: A variant array containing a list of all roles to which   '
'         a user is associated.                                     '
'--------------------------------------------------------------------
'Revision History:                                                  '
' 03/02/2000 Segura:  Implemented uspSelUserRole                    '
'--------------------------------------------------------------------

    Dim cmd As ADODB.Command
    Dim cnn As ADODB.Connection
    Dim rst As ADODB.Recordset
    Dim strList As String
    Dim intCTR As Integer

    On Error GoTo ErrTrap:

    Set cmd = New ADODB.Command
    Set cnn = New ADODB.Connection
    Set rst = New ADODB.Recordset
    
    'Acquire the database connection.
    Call cnn.Open(CONST_PSYQUEL_CNN)
    Set cmd.ActiveConnection = cnn
    
    ' Prepare stored procedure parameters
    With cmd
        .CommandText = "uspSelUserRoles"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@UserID", adInteger, _
            adParamInput, , lngUserID)
    End With
    
    'Instantiate and populate the Recordset.
    rst.CursorLocation = adUseClient
    rst.Open cmd, , adOpenForwardOnly, adLockReadOnly
    
    'Disconnect the recordset, close the connection and return the recordset
    'to the calling environment.
    Set rst.ActiveConnection = Nothing
    cnn.Close
    Set cnn = Nothing
    
    Set FetchRolesByUserID = rst
    
    Set cmd = Nothing

    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set rst = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

Public Function FetchRolesByUserName(ByVal strUserName As String) _
                    As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 02/14/2000                                                   '
'Author: Rick "Boom Boom" Segura                                    '
'Description:  Returns a recordset containing a list of all roles   '
'              to which a user is associated.                       '
'Parameters: strUserName - The network login name of the user       '
'Returns: A recordset containing a list of all roles   to which a   '
'           user is associated.                                     '
'--------------------------------------------------------------------
'Revision History:                                                  '
'                                                                   '
'--------------------------------------------------------------------

    Dim cnnSQL As ADODB.Connection
    Dim rstSQL As ADODB.Recordset
    Dim strSQL As String
    Dim intCTR As Integer

    On Error GoTo ErrTrap:

    'Prepare the SQL statement.
    strSQL = "SELECT "
    strSQL = strSQL & "tblRole.fldRoleID, "
    strSQL = strSQL & "tblRole.fldRoleName "
    strSQL = strSQL & "FROM "
    strSQL = strSQL & "tblRole,  "
    strSQL = strSQL & "tblUserRole,  "
    strSQL = strSQL & "tblUser  "
    strSQL = strSQL & " WHERE "
    strSQL = strSQL & "tblRole.fldRoleID = tblUserRole.fldRoleID AND  "
    strSQL = strSQL & "tblUserRole.fldUserID = tblUser.fldUserID AND  "
    strSQL = strSQL & "tblUser.fldUserName = '" & strUserName & "'"
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open(CONST_PSYQUEL_CNN)
    
    'Instantiate and populate the Recordset.
    Set rstSQL = New ADODB.Recordset
    rstSQL.CursorLocation = adUseClient
    Call rstSQL.Open(strSQL, cnnSQL, adOpenForwardOnly, adLockReadOnly, _
            adCmdText + adAsyncFetch)
    
    'Disconnect the recordset, close the connection and return the recordset
    'to the calling environment.
    Set rstSQL.ActiveConnection = Nothing
    cnnSQL.Close
    Set cnnSQL = Nothing
    
    Set FetchRolesByUserName = rstSQL
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set FetchRolesByUserName = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Function
