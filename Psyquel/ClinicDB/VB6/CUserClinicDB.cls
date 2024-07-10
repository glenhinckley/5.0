VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CUserClinicDB"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CUserClinicIns                                         '
'Date: 01/04/2000                                                   '
'Author: Dave Richkun                                               '
'Description:  MTS object designed to allow insertions into the     '
'              tblUserClinic table.                                 '
'--------------------------------------------------------------------
'Revision History:                                                  '
'   R001 Richkun 02/03/2000 - Added FetchClinics method             '
'   R002 Segura 02/28/2000 - Removed Update method; not required    '
'--------------------------------------------------------------------

Option Explicit

Private Const TABLE_NAME As String = "tblUserClinic"
Private Const CLASS_NAME As String = "CUserClinicIns"

Public Function FetchClinicsByUserID(ByVal lngUserID As Long) _
                    As ADODB.Recordset  'R001
'--------------------------------------------------------------------
'Date: 01/10/2000                                                   '
'Author: Dave Richkun                                               '
'Description:  Returns a variant array containing a list of all     '
'              clinics to which a user is associated.               '
'Parameters: lngUserID - The ID of the user                         '
'Returns: A variant array containing a list of all clinics to which '
'         a user is associated.                                     '
'--------------------------------------------------------------------
'Revision History:                                                  '
'                                                                   '
'--------------------------------------------------------------------

    Dim cmd As ADODB.Command
    Dim cnn As ADODB.Connection
    Dim rst As ADODB.Recordset
    Dim str As String

    On Error GoTo ErrTrap:

    Set cmd = New ADODB.Command
    Set cnn = New ADODB.Connection
    Set rst = New ADODB.Recordset
    
    'Acquire the database connection.
    Call cnn.Open(CONST_PSYQUEL_CNN)
    Set cmd.ActiveConnection = cnn
    
    ' Prepare stored procedure parameters
    With cmd
        .CommandText = "uspSelUserClinics"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@UserID", adInteger, adParamInput, , lngUserID)
    End With
    
    'Instantiate and populate the Recordset.
    rst.CursorLocation = adUseClient
    rst.Open cmd, , adOpenForwardOnly, adLockReadOnly
    
    'Disconnect the recordset, close the connection and return the recordset
    'to the calling environment.
    Set rst.ActiveConnection = Nothing
    cnn.Close
    Set cnn = Nothing
    
    Set FetchClinicsByUserID = rst
    
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


Public Function FetchClinicsByUserName(ByVal strUserName As String) _
                    As ADODB.Recordset 'R002
'--------------------------------------------------------------------
'Date: 02/14/2000                                                   '
'Author: Dave Richkun                                               '
'Description:  Returns a recordset containing a list of all clinics '
'              to which a user is associated.                       '
'Parameters: strUserName - The network login name of the user       '
'Returns: A recordset containing a list of all clinics to which a   '
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
    strSQL = strSQL & "tblClinic.fldClinicID, "
    strSQL = strSQL & "tblClinic.fldBusinessName "
    strSQL = strSQL & "FROM "
    strSQL = strSQL & "tblClinic,  "
    strSQL = strSQL & "tblUserClinic,  "
    strSQL = strSQL & "tblUsers  "
    strSQL = strSQL & " WHERE "
    strSQL = strSQL & "tblClinic.fldClinicID = tblUserClinic.fldClinicID AND  "
    strSQL = strSQL & "tblUserClinic.fldUserID = tblUsers.fldUserID AND  "
    strSQL = strSQL & "tblUsers.fldUserName = '" & strUserName & "'"
    
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
    
    Set FetchClinicsByUserName = rstSQL
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set FetchClinicsByUserName = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

Public Sub Insert(ByVal lngUserID As Long, ByVal lngClinicID As Long)
'--------------------------------------------------------------------
'Date: 01/04/2000                                                   '
'Author: Dave Richkun                                               '
'Description:  Inserts one or more rows into the tblUserClinic      '
'               table.                                              '
'Parameters: lngUserID - ID of the user who is being associated to  '
'               one or more clinics                                 '
'            lngClinicID - ID of the clinic being associated with a
'               user
'Returns: Null                                                      '
'--------------------------------------------------------------------

    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command
    Dim strSQL As String
    Dim intCTR As Integer

    On Error GoTo ErrTrap:
    
    Set cmdSQL = New ADODB.Command
    cmdSQL.CommandType = adCmdStoredProc
            
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open(CONST_PSYQUEL_CNN)
            
    With cmdSQL
        .CommandText = "uspInsUserClinic"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@UserID", adInteger, adParamInput, , lngUserID)
        .Parameters.Append .CreateParameter("@ClinicID", adInteger, adParamInput, , lngClinicID)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With

    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute ', adExecuteNoRecords

    ' Check for those ungodly errors
    If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If
    
    'Close the connection and free all resources
    cnnSQL.Close
    
EMPTY_ARRAY:
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing

    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Sub
    
ErrTrap:
    'The Clinic array may be empty.
    If Err.Number = 9 Then
        GoTo EMPTY_ARRAY
    Else
        'Signal incompletion and raise the error to the calling environment.
        GetObjectContext.SetAbort
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Sub


Public Sub DeleteUserClinics(ByVal lngUserID As Long)
'--------------------------------------------------------------------
'Date: 03/28/2000                                                   '
'Author: Dave Richkun                                               '
'Description:  Deletes all clinics associated with a user
'Parameters: lngUserID - ID of the user who's clinic associatons are
'               being deleted
'Returns: Null                                                      '
'--------------------------------------------------------------------

    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command

    On Error GoTo ErrTrap:
    
    Set cmdSQL = New ADODB.Command
    cmdSQL.CommandType = adCmdStoredProc
            
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open(CONST_PSYQUEL_CNN)
            
    With cmdSQL
        .CommandText = "uspDelUserClinics"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@UserID", adInteger, adParamInput, , lngUserID)
    End With

    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute

    'Close the connection and free all resources
    cnnSQL.Close
    
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing

    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub




''Public Sub Update(ByVal lngUserID As Long, ByVal varClinics As Variant)
'--------------------------------------------------------------------
'Date: 01/04/2000
'Author: Dave Richkun
'Description:  Updates a row into the tblUsers table, and inserts rows
'              into the tblUserClinics table.
'Parameters: Each parameter identifies the column values that will be
'              Updated.
'Returns: Null
'--------------------------------------------------------------------
''
''    Dim objUserClinic As ClinicDB.CUserClinicDB
''    Dim strErrMessage As String
''
''   On Error GoTo ErrTrap:
''
''    'Update the UserClinic records
''    Set objUserClinic = CreateObject("ClinicDB.CUserClinicDB")
''    Call objUserClinic.Insert(lngUserID, varClinics)
''    Set objUserClinic = Nothing
''
''    'Signal successful completion
''    GetObjectContext.SetComplete
''
''    Exit Sub
''
''ErrTrap:
''   'Signal incompletion and raise the error to the calling environment.
''    GetObjectContext.SetAbort
''    Err.Raise Err.Number, Err.Source, Err.Description
''
''End Sub





