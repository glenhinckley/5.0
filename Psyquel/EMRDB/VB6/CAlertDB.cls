VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 2  'RequiresTransaction
END
Attribute VB_Name = "CAlertDB"
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
'
'--------------------------------------------------------------------
Option Explicit

Private Const CLASS_NAME As String = "CAlertDB"
Private Const TABLE_NAME As String = "tblPatientAlert"

Public Function FetchByID(ByVal lngUserID As Long, ByVal lngPatientAlertID As Long, Optional blnIncludeDisabled As Boolean = False) As ADODB.Recordset
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

    ' Instantiate the objects
    Set rst = New ADODB.Recordset
    Set cmd = New ADODB.Command
    Set cnn = New ADODB.Connection
    
    'Create the parameter objects
    With cmd
        .CommandText = "uspSelPatientAlertByID"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@PatientAlertID", adInteger, adParamInput, , lngPatientAlertID)
        .Parameters.Append .CreateParameter("@IncludeDisabledYN", adVarChar, adParamInput, 1, IIf(blnIncludeDisabled = False, "N", "Y"))
        ' .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Acquire the database connection.
    cnn.Open ("DSN=PsyquelSQL")
    Set cmd.ActiveConnection = cnn

    'Execute the stored procedure
    rst.CursorLocation = adUseClient
    rst.Open cmd, , adOpenForwardOnly, adLockReadOnly

    Set FetchByID = rst
    
    'Disconnect the recordset
    rst.Close
    cnn.Close
    Set cmd.ActiveConnection = Nothing
    Set cmd = Nothing
    Set rst.ActiveConnection = Nothing
    Set cnn = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    rst.Close
    cnn.Close
    Set rst = Nothing
    Set cmd = Nothing
    Set cnn = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

Public Function FetchByPatient(ByVal lngUserID As Long, ByVal lngPatientID As Long, Optional blnIncludeDisabled As Boolean = False) As ADODB.Recordset
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

    ' Instantiate the objects
    Set rst = New ADODB.Recordset
    Set cmd = New ADODB.Command
    Set cnn = New ADODB.Connection
    
    'Create the parameter objects
    With cmd
        .CommandText = "uspSelPatientAlertByPatient"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID)
        .Parameters.Append .CreateParameter("@IncludeDisabledYN", adVarChar, adParamInput, 1, IIf(blnIncludeDisabled = False, "N", "Y"))
        ' .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Acquire the database connection.
    cnn.Open ("DSN=PsyquelSQL")
    Set cmd.ActiveConnection = cnn

    'Execute the stored procedure
    rst.CursorLocation = adUseClient
    rst.Open cmd, , adOpenForwardOnly, adLockReadOnly

    Set FetchByPatient = rst
    
    'Disconnect the recordset
    rst.Close
    cnn.Close
    Set cmd.ActiveConnection = Nothing
    Set cmd = Nothing
    Set rst.ActiveConnection = Nothing
    Set cnn = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    rst.Close
    cnn.Close
    Set rst = Nothing
    Set cmd = Nothing
    Set cnn = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Function


Public Function FetchByAlert(ByVal lngUserID As Long, ByVal lngAlertID As Long, Optional blnIncludeDisabled As Boolean = False) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 08/31/2000
'Author: Dave Nichol and Joshua Lockwood
'Description: Retrieves records from the tblPatientAlert table.  Calls
'               stored procedure uspSelPatientAlertByAlert.
'Parameters: blnIncludeDisabled - Optional parameter that identifies if
'               records flagged as 'Disabled' or 'De-activated' are to be
'               included in the record set. The default value is False.
'            lngAlertID - 'Where' clause of the SQL statement by
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

    ' Instantiate the objects
    Set rst = New ADODB.Recordset
    Set cmd = New ADODB.Command
    Set cnn = New ADODB.Connection
    
    'Create the parameter objects
    With cmd
        .CommandText = "uspSelPatientAlertByAlert"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@AlertID", adInteger, adParamInput, , lngAlertID)
        .Parameters.Append .CreateParameter("@IncludeDisabledYN", adVarChar, adParamInput, 1, IIf(blnIncludeDisabled = False, "N", "Y"))
        ' .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Acquire the database connection.
    cnn.Open ("DSN=PsyquelSQL")
    Set cmd.ActiveConnection = cnn

    'Execute the stored procedure
    rst.CursorLocation = adUseClient
    rst.Open cmd, , adOpenForwardOnly, adLockReadOnly

    Set FetchByAlert = rst
    
    'Disconnect the recordset
    rst.Close
    cnn.Close
    Set cmd.ActiveConnection = Nothing
    Set cmd = Nothing
    Set rst.ActiveConnection = Nothing
    Set cnn = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    rst.Close
    cnn.Close
    Set rst = Nothing
    Set cmd = Nothing
    Set cnn = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Function


Public Function Insert(ByVal lngPatientID As Long, ByVal lngAlertID As Long, ByVal strUserName As String) As Long
'--------------------------------------------------------------------
'Date: 08/31/2000
'Author: Dave Nichol and Joshua Lockwood
'Description: Inserts records into the tblPatientAlert table.  Calls
'               stored procedure uspInsPatientAlert.
'Parameters:  lngAlertID - AlertID (corresponding to type of alert).
'             lngPatientID - ID of the patient.
'             strUserName - username of user inserting data
'Returns: Long
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    ' Declare connection, returned recordset, and command
    Dim cnn As ADODB.Connection
    Dim rst As ADODB.Recordset
    Dim cmd As ADODB.Command

    On Error GoTo ErrTrap:

    ' Instantiate the objects
    Set rst = New ADODB.Recordset
    Set cmd = New ADODB.Command
    Set cnn = New ADODB.Connection
    
    'Create the parameter objects
    With cmd
        .CommandText = "uspInsPatientAlert"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID)
        .Parameters.Append .CreateParameter("@AlertID", adInteger, adParamInput, , lngAlertID)
        .Parameters.Append .CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strUserName)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Assign the connection to the Command object and execute the stored procedure
    cnn.Open ("DSN=PsyquelSQL")
    Set cmd.ActiveConnection = cnn
    cmd.Execute , , adExecuteNoRecords

    Insert = cmd.Parameters("@PatientAlertID").Value
    
    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If
        
     'Disconnect the recordset
    rst.Close
    cnn.Close
    Set cmd.ActiveConnection = Nothing
    Set cmd = Nothing
    Set rst.ActiveConnection = Nothing
    Set cnn = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    rst.Close
    cnn.Close
    Set rst = Nothing
    Set cmd = Nothing
    Set cnn = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

Public Function Update(ByVal lngPatientAlertID As Long, ByVal lngPatientID As Long, ByVal lngAlertID As Long, ByVal strUserName As String) As Long
'--------------------------------------------------------------------
'Date: 08/31/2000
'Author: Dave Nichol and Joshua Lockwood
'Description: Updates selected records in the tblPatientAlert table.  Calls
'               stored procedure uspUpdPatientAlert.
'Parameters:  lngPatientAlertID - ID of the patient.
'             lngAlertID - AlertID (corresponding to type of alert).
'             lngPatientID - ID of the patient.
'             strUserName - username of user inserting data
'Returns: Long
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    ' Declare connection, returned recordset, and command
    Dim cnn As ADODB.Connection
    Dim rst As ADODB.Recordset
    Dim cmd As ADODB.Command

    On Error GoTo ErrTrap:

    ' Instantiate the objects
    Set rst = New ADODB.Recordset
    Set cmd = New ADODB.Command
    Set cnn = New ADODB.Connection
    
    'Create the parameter objects
    With cmd
        .CommandText = "uspUpdPatientAlert"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@LastModifiedBy", adVarChar, adParamInput, 30, strUserName)
        .Parameters.Append .CreateParameter("@PatientAlertID", adInteger, adParamInput, , lngPatientAlertID)
        .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID)
        .Parameters.Append .CreateParameter("@AlertID", adInteger, adParamInput, , lngAlertID)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Assign the connection to the Command object and execute the stored procedure
    cnn.Open ("DSN=PsyquelSQL")
    Set cmd.ActiveConnection = cnn
    cmd.Execute , , adExecuteNoRecords

    Update = 0
    
    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If
        
     'Disconnect the recordset
    rst.Close
    cnn.Close
    Set cmd.ActiveConnection = Nothing
    Set cmd = Nothing
    Set rst.ActiveConnection = Nothing
    Set cnn = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    rst.Close
    cnn.Close
    Set rst = Nothing
    Set cmd = Nothing
    Set cnn = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

Public Function Deleted(ByVal lngPatientAlertID As Long, ByVal blnDisableYN As Boolean, ByVal strUserName As String)
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
    Dim rst As ADODB.Recordset
    Dim cmd As ADODB.Command

    On Error GoTo ErrTrap:

    ' Instantiate the objects
    Set rst = New ADODB.Recordset
    Set cmd = New ADODB.Command
    Set cnn = New ADODB.Connection
    
    'Create the parameter objects
    With cmd
        .CommandText = "uspDelPatientAlert"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@LastModifiedBy", adVarChar, adParamInput, 30, strUserName)
        .Parameters.Append .CreateParameter("@PatientAlertID", adInteger, adParamInput, , lngPatientAlertID)
        .Parameters.Append .CreateParameter("@DisableYN", adVarChar, adParamInput, 1, IIf(blnDisableYN = False, "N", "Y"))
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Assign the connection to the Command object and execute the stored procedure
    cnn.Open ("DSN=PsyquelSQL")
    Set cmd.ActiveConnection = cnn
    cmd.Execute , , adExecuteNoRecords

    Deleted = 0
    
    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If
        
     'Disconnect the recordset
    rst.Close
    cnn.Close
    Set cmd.ActiveConnection = Nothing
    Set cmd = Nothing
    Set rst.ActiveConnection = Nothing
    Set cnn = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    rst.Close
    cnn.Close
    Set rst = Nothing
    Set cmd = Nothing
    Set cnn = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Function


