VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CPatientAlertDB"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CAlertDB
'Date: 08/31/2000
'Author: Dave Nichol and Joshua Lockwood
'Description:  Business object designed to host methods associated with
'              data affecting the tblPatientAlert  table.
'--------------------------------------------------------------------
'  Revisions:
'   --R001 : Joshua Lockwood, 15Sep00 : Changes to guard against
'           memory leaks
'
'--------------------------------------------------------------------
Option Explicit

Private Const CLASS_NAME As String = "CAlertDB"
Private Const TABLE_NAME As String = "tblPatientAlert"

Public Function FetchByID(ByVal lngPatientAlertID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 08/31/2000
'Author: Dave Nichol and Joshua Lockwood
'Description: Retrieves records from the tblPatientAlert table.  Calls
'               stored procedure uspSelPatientAlertByID.
'Parameters: blnIncludeDisabled - Optional parameter that identifies if
'               records flagged as 'Disabled' or 'De-activated' are to be
'               included in the record set. The default value is False.
'            lngPatientAlertID - 'Where' clause of the SQL statement by
'               which records will be fetched.  Matches tblPatientAlert's
'               primary key.
'             lngUserID - UserID of the user performing the fetch.  Not
'               really used currently.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    ' Declare connection, returned recordset, and command
    Dim cnn As ADODB.Connection
    Dim rst As ADODB.Recordset
    Dim cmd As ADODB.Command

    On Error GoTo ErrTrap:
   
    'Create the parameter objects
    Set cmd = New ADODB.Command 'Instantiate SQL statement --R001
    With cmd
        .CommandText = "uspSelPatientAlertByID"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@PatientAlertID", adInteger, adParamInput, , lngPatientAlertID)
    End With
    
    'Acquire the database connection.
    Set cnn = New ADODB.Connection  'Instantiate DB connection --R001
    cnn.Open (CONST_PSYQUEL_CNN)
    Set cmd.ActiveConnection = cnn

    'Execute the stored procedure
    Set rst = New ADODB.Recordset   'Instantiate recordset --R001
    rst.CursorLocation = adUseClient
    rst.Open cmd, , adOpenForwardOnly, adLockReadOnly

    Set FetchByID = rst
    
    Set rst.ActiveConnection = Nothing  '--R001
    Set cmd.ActiveConnection = Nothing
    cnn.Close
    Set cmd = Nothing
    Set cnn = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    
    Set rst.ActiveConnection = Nothing ' --R001 ... can't do?
    rst.Close   ' --R001 ... can't do?
    Set cmd.ActiveConnection = Nothing
    cnn.Close
    
    Set rst = Nothing
    Set cmd = Nothing
    Set cnn = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description
End Function

Public Function FetchByPatient(ByVal lngPatientID As Long, Optional blnIncludeDisabled As Boolean = False) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 08/31/2000
'Author: Dave Nichol and Joshua Lockwood
'Description: Retrieves records from the tblPatientAlert table.  Calls
'               stored procedure uspSelPatientAlertByID.
'Parameters: blnIncludeDisabled - Optional parameter that identifies if
'               records flagged as 'Disabled' or 'De-activated' are to be
'               included in the record set. The default value is False.
'            lngPatientID - 'Where' clause of the SQL statement by
'               which records will be fetched.
'             lngUserID - UserID of the user performing the fetch.  Not
'               really used currently.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    ' Declare connection, returned recordset, and command
    Dim cnn As ADODB.Connection
    Dim rst As ADODB.Recordset
    Dim cmd As ADODB.Command

    On Error GoTo ErrTrap:

    
    'Create the parameter objects
    Set cmd = New ADODB.Command ' Instantiate SQL stament --R001
    With cmd
        .CommandText = "uspSelPatientAlertByPatient"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID)
        .Parameters.Append .CreateParameter("@IncludeDisabled", adVarChar, adParamInput, 1, IIf(blnIncludeDisabled = False, "N", "Y"))
    End With
    
    'Acquire the database connection.
    Set cnn = New ADODB.Connection  ' --R001
    cnn.Open (CONST_PSYQUEL_CNN)
    Set cmd.ActiveConnection = cnn

    'Execute the stored procedure
    Set rst = New ADODB.Recordset   ' --R001
    rst.CursorLocation = adUseClient
    rst.Open cmd, , adOpenForwardOnly, adLockReadOnly

    Set FetchByPatient = rst
    
    'Disconnect the recordset
    Set rst.ActiveConnection = Nothing
    Set cmd.ActiveConnection = Nothing
    cnn.Close
    Set cmd = Nothing
    Set cnn = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    
    Set rst.ActiveConnection = Nothing
    Set cmd.ActiveConnection = Nothing
    rst.Close   ' --R001
    cnn.Close
    Set rst = Nothing
    Set cmd = Nothing
    Set cnn = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description

End Function


Public Function FetchByAlert(ByVal strAlertText As String, Optional blnIncludeDisabled As Boolean = False) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 08/31/2000
'Author: Dave Nichol and Joshua Lockwood
'Description: Retrieves records from the tblPatientAlert table.  Calls
'               stored procedure uspSelPatientAlertByAlert.
'Parameters: blnIncludeDisabled - Optional parameter that identifies if
'               records flagged as 'Disabled' or 'De-activated' are to be
'               included in the record set. The default value is False.
'            strAlertText - Text of the alert itself.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    ' Declare connection, returned recordset, and command
    Dim cnn As ADODB.Connection
    Dim rst As ADODB.Recordset
    Dim cmd As ADODB.Command

    On Error GoTo ErrTrap:
    
    'Create the parameter objects
    Set cmd = New ADODB.Command ' --R001
    With cmd
        .CommandText = "uspSelPatientAlertByAlert"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@AlertText", adVarChar, adParamInput, 256, strAlertText)
        .Parameters.Append .CreateParameter("@IncludeDisabled", adVarChar, adParamInput, 1, IIf(blnIncludeDisabled = False, "N", "Y"))
    End With
    
    'Acquire the database connection.
    Set cnn = New ADODB.Connection  ' --R001
    cnn.Open (CONST_PSYQUEL_CNN)
    Set cmd.ActiveConnection = cnn

    'Execute the stored procedure
    Set rst = New ADODB.Recordset   ' --R001
    rst.CursorLocation = adUseClient
    rst.Open cmd, , adOpenForwardOnly, adLockReadOnly

    Set FetchByAlert = rst
    
    'Disconnect the recordset
    Set rst.ActiveConnection = Nothing
    Set cmd.ActiveConnection = Nothing
    cnn.Close
    Set cmd = Nothing
    Set cnn = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    
    Set rst.ActiveConnection = Nothing
    Set cmd.ActiveConnection = Nothing
    rst.Close
    cnn.Close
    Set rst = Nothing
    Set cmd = Nothing
    Set cnn = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description

End Function


Public Function Insert(ByVal lngPatientID As Long, ByVal strAlertText As String, ByVal strUserName As String) As Long
'--------------------------------------------------------------------
'Date: 08/31/2000
'Author: Dave Nichol and Joshua Lockwood
'Description: Inserts records into the tblPatientAlert table.  Calls
'               stored procedure uspInsPatientAlert.
'Parameters:  strAlertText - Text of the alert itself.
'             lngPatientID - ID of the patient.
'             strUserName - username of user inserting data
'Returns: Long
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    ' Declare connection, returned recordset, and command
    Dim cnn As ADODB.Connection
    Dim cmd As ADODB.Command

    On Error GoTo ErrTrap:
    
    'Create the parameter objects
    Set cmd = New ADODB.Command ' --R001
    With cmd
        .CommandText = "uspInsPatientAlert"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@PatientAlertID", adInteger, adParamReturnValue)
        .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID)
        .Parameters.Append .CreateParameter("@AlertText", adVarChar, adParamInput, 256, strAlertText)
        .Parameters.Append .CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strUserName)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cnn = New ADODB.Connection
    cnn.Open (CONST_PSYQUEL_CNN)
    Set cmd.ActiveConnection = cnn
    cmd.Execute , , adExecuteNoRecords

    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If
        
    Insert = cmd.Parameters(0).Value
    
    'Disconnect the recordset
    cnn.Close
    Set cmd.ActiveConnection = Nothing
    Set cmd = Nothing
    Set cnn = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    
    Set cmd.ActiveConnection = Nothing
    cnn.Close
    Set cmd = Nothing
    Set cnn = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description
End Function

Public Function Update(ByVal lngPatientAlertID As Long, ByVal lngPatientID As Long, ByVal strAlertText As String, ByVal strUserName As String) As Long
'--------------------------------------------------------------------
'Date: 08/31/2000
'Author: Dave Nichol and Joshua Lockwood
'Description: Updates selected records in the tblPatientAlert table.  Calls
'               stored procedure uspUpdPatientAlert.
'Parameters:  lngPatientAlertID - ID of the patient.
'             lstrAlertText - Text of the alert itself.
'             lngPatientID - ID of the patient.
'             strUserName - username of user inserting data
'Returns: Long
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    ' Declare connection, returned recordset, and command
    Dim cnn As ADODB.Connection
    Dim cmd As ADODB.Command

    On Error GoTo ErrTrap:

    
    'Create the parameter objects
    Set cmd = New ADODB.Command ' --R001
    With cmd
        .CommandText = "uspUpdPatientAlert"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@PatientAlertID", adInteger, adParamInput, , lngPatientAlertID)
        .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID)
        .Parameters.Append .CreateParameter("@AlertText", adVarChar, adParamInput, 256, strAlertText)
        .Parameters.Append .CreateParameter("@LastModifiedBy", adVarChar, adParamInput, 30, strUserName)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cnn = New ADODB.Connection  ' --R001
    cnn.Open (CONST_PSYQUEL_CNN)
    Set cmd.ActiveConnection = cnn
    cmd.Execute , , adExecuteNoRecords

    Update = 0
    
    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If
        
    'Disconnect the recordset
    Set cmd.ActiveConnection = Nothing
    cnn.Close
    Set cmd = Nothing
    Set cnn = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    
    Set cmd.ActiveConnection = Nothing
    cnn.Close
    Set cmd = Nothing
    Set cnn = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

Public Function Deleted(ByVal lngPatientAlertID As Long, ByVal blnDisabled As Boolean, ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 08/31/2000
'Author: Dave Nichol and Joshua Lockwood
'Description: Updates selected records in the tblPatientAlert table.  Calls
'               stored procedure uspUpdPatientAlert.
'Parameters:  lngPatientAlertID - ID of the patient.
'             blnDisableYN - boolean Y if row is disabled, N if row is active.
'             strUserName - username of user inserting data
'Returns: Null
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    ' Declare connection, returned recordset, and command
    Dim cnn As ADODB.Connection
    Dim cmd As ADODB.Command

    On Error GoTo ErrTrap:

    
    'Create the parameter objects
    Set cmd = New ADODB.Command ' --R001
    With cmd
        .CommandText = "uspDelPatientAlert"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@PatientAlertID", adInteger, adParamInput, , lngPatientAlertID)
        .Parameters.Append .CreateParameter("@DisabledYN", adVarChar, adParamInput, 1, IIf(blnDisabled = False, "N", "Y"))
        .Parameters.Append .CreateParameter("@LastModifiedBy", adVarChar, adParamInput, 30, strUserName)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cnn = New ADODB.Connection  ' --R001
    cnn.Open (CONST_PSYQUEL_CNN)
    Set cmd.ActiveConnection = cnn
    cmd.Execute , , adExecuteNoRecords

    Deleted = 0
    
    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If
        
     'Disconnect the recordset
    cnn.Close
    Set cmd.ActiveConnection = Nothing
    Set cmd = Nothing
    Set cnn = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    
    cnn.Close
    Set cmd = Nothing
    Set cnn = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description
End Function

