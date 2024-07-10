VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CPatientDiagnosisDB"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CPatientDiagnosisDB
'Date: 09/07/2000
'Author: Joshua Lockwood
'Description:  Database object designed to implement methods (by calling
'              stored procedures) associated with
'              data affecting the tblPatientDiagnosis table.
'--------------------------------------------------------------------
'  Revisions:
'   --R001 : Joshua Lockwood, 15Sep00 : Changes to avoid memory leaks
'   --R002 : Joshua Lockwood, 11Oct00 : TEXT to VARCHAR(256) for ins and upd
'
'--------------------------------------------------------------------
Option Explicit

Private Const CLASS_NAME As String = "CPatientDiagnosisDB"
Private Const TABLE_NAME As String = "tblPatientDiagnosis"


Public Function FetchByID(ByVal lngPatientDiagnosisID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 09/07/2000
'Author: Joshua Lockwood
'Description: Retrieves records from the tblPatientDiagnosis table.  Calls
'               stored procedure uspSelPatientDiagnosisByID.
'Parameters: lngPatientDiagnosisID - 'Where' clause of the SQL statement by
'               which records will be fetched.  Matches tblPatientDiagnosis's
'               primary key.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------
'Revision History:
'   ' --R001 : Joshua Lockwood, 15Sep00 : Changes to fix memory leaks
'
'--------------------------------------------------------------------

    ' Declare connection, returned recordset, and command
    Dim cnn As ADODB.Connection ' Declare DB connection
    Dim rst As ADODB.Recordset  ' Declare recordset
    Dim cmd As ADODB.Command    ' Declare SQL statement

    On Error GoTo ErrTrap:

    'Create the parameter objects
    Set cmd = New ADODB.Command     ' Instantiate SQL statement --R001
    With cmd
        .CommandText = "uspSelPatientDiagnosisByID"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@PatientDiagnosisID", adInteger, adParamInput, , lngPatientDiagnosisID)
    End With
    
    'Acquire the database connection.
    Set cnn = New ADODB.Connection  ' Instantiate DB connection --R001
    cnn.Open (CONST_PSYQUEL_CNN) ' open DB connection
    Set cmd.ActiveConnection = cnn

    'Execute the stored procedure
    Set rst = New ADODB.Recordset   ' Instantiate recordset --R001
    rst.CursorLocation = adUseClient
    rst.Open cmd, , adOpenForwardOnly, adLockReadOnly   ' open recordset

    Set FetchByID = rst ' Set return value
    
    ' De-allocate space for objects
    Set cmd.ActiveConnection = Nothing
    Set rst.ActiveConnection = Nothing
    'Cannot close recordset in DB object  --R001
    cnn.Close   ' close DB connection
    Set cmd = Nothing
    Set cnn = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    
    Set cmd.ActiveConnection = Nothing  ' --R001
    Set rst.ActiveConnection = Nothing  ' --R001
    rst.Close   ' close recordset
    cnn.Close   ' close DB connection
    
    ' De-allocate space for objects
    Set rst = Nothing
    Set cmd = Nothing
    Set cnn = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description
End Function

Public Function FetchByPatient(ByVal lngPatientID As Long, Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 09/07/2000
'Author: Joshua Lockwood
'Description: Retrieves records from the tblPatientDiagnosis table.  Calls
'               stored procedure uspSelPatientDiagnosisByPatient.
'Parameters: lngPatientID - 'Where' clause of the SQL statement by
'               which records will be fetched.
'            blnIncludeDisabled - Flag used to control visibility of "Deleted" items.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    ' Declare connection, returned recordset, and command
    Dim cnn As ADODB.Connection ' Declare DB connection
    Dim rst As ADODB.Recordset  ' Declare recordset
    Dim cmd As ADODB.Command    ' Declare SQL statement

    On Error GoTo ErrTrap:
    
    'Create the parameter objects
    Set cmd = New ADODB.Command     ' Instantiate SQL statement --R001
    With cmd
        .CommandText = "uspSelPatientDiagnosisByPatient"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID)
        .Parameters.Append .CreateParameter("@IncludeDisabledYN", adVarChar, adParamInput, 1, IIf(blnIncludeDisabled = False, "N", "Y"))
    End With
    
    'Acquire the database connection.
    Set cnn = New ADODB.Connection  ' Instantiate DB connection --R001
    cnn.Open (CONST_PSYQUEL_CNN) ' open DB connection
    Set cmd.ActiveConnection = cnn

    'Execute the stored procedure
    Set rst = New ADODB.Recordset   ' Instantiate recordset --R001
    rst.CursorLocation = adUseClient
    rst.Open cmd, , adOpenForwardOnly, adLockReadOnly   ' open recordset

    Set FetchByPatient = rst    ' Set return value
    
    ' De-allocate space for objects
    Set cmd.ActiveConnection = Nothing
    Set rst.ActiveConnection = Nothing
    ' Cannot close recordset in DB object  --R001
    cnn.Close   ' close DB connection
    Set cmd = Nothing
    Set cnn = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    
    Set cmd.ActiveConnection = Nothing  ' --R001
    Set rst.ActiveConnection = Nothing  ' --R001
    rst.Close   ' close recordset
    cnn.Close   ' close DB connection
    
    ' De-allocate space for objects
    Set rst = Nothing
    Set cmd = Nothing
    Set cnn = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Public Function Insert(ByVal lngPatientID As Long, ByVal strUserName As String, _
                       Optional ByVal lngAxisIa As Long, Optional ByVal lngAxisIb As Long, _
                       Optional ByVal lngAxisIc As Long, Optional ByVal lngAxisId As Long, _
                       Optional ByVal lngAxisIe As Long, Optional ByVal lngAxisIf As Long, _
                       Optional ByVal lngAxisIg As Long, Optional ByVal lngAxisIh As Long, _
                       Optional ByVal strDxNotes As String _
                    ) As Long
'--------------------------------------------------------------------
'Date: 09/07/2000
'Author: Joshua Lockwood
'Description: Inserts records into the tblPatientDiagnosis table.  Calls
'               stored procedure uspInsPatientDiagnosis.
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'Returns: Long
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    ' Declare connection, returned recordset, and command
    Dim cnn As ADODB.Connection ' Declare DB connection
    Dim cmd As ADODB.Command    ' Declare SQL statement

    On Error GoTo ErrTrap:
    
    'Create the parameter objects
    Set cmd = New ADODB.Command     ' Instantiate SQL statement --R001
    With cmd
        .CommandText = "uspInsPatientDiagnosis"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@PatientDiagnosisID", adInteger, adParamReturnValue)
        .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID)
        .Parameters.Append .CreateParameter("@AxisIa", adInteger, adParamInput, , IIf(lngAxisIa = 0, Null, lngAxisIa))
        .Parameters.Append .CreateParameter("@AxisIb", adInteger, adParamInput, , IIf(lngAxisIb = 0, Null, lngAxisIb))
        .Parameters.Append .CreateParameter("@AxisIc", adInteger, adParamInput, , IIf(lngAxisIc = 0, Null, lngAxisIc))
        .Parameters.Append .CreateParameter("@AxisId", adInteger, adParamInput, , IIf(lngAxisId = 0, Null, lngAxisId))
        .Parameters.Append .CreateParameter("@AxisIe", adInteger, adParamInput, , IIf(lngAxisIe = 0, Null, lngAxisIe))
        .Parameters.Append .CreateParameter("@AxisIf", adInteger, adParamInput, , IIf(lngAxisIf = 0, Null, lngAxisIf))
        .Parameters.Append .CreateParameter("@AxisIg", adInteger, adParamInput, , IIf(lngAxisIg = 0, Null, lngAxisIg))
        .Parameters.Append .CreateParameter("@AxisIh", adInteger, adParamInput, , IIf(lngAxisIh = 0, Null, lngAxisIh))
    '    .Parameters.Append .CreateParameter("@AxisIIa", adInteger, adParamInput, , IIf(lngAxisIIa = 0, Null, lngAxisIIa))
    '    .Parameters.Append .CreateParameter("@AxisIIaModifier", adVarChar, adParamInput, 20, strAxisIIaModifier)
    '    .Parameters.Append .CreateParameter("@AxisIIb", adInteger, adParamInput, , IIf(lngAxisIIb = 0, Null, lngAxisIIb))
    '    .Parameters.Append .CreateParameter("@AxisIIbModifier", adVarChar, adParamInput, 20, strAxisIIbModifier)
    '    .Parameters.Append .CreateParameter("@AxisIII", adVarChar, adParamInput, 100, strAxisIII)
    '    .Parameters.Append .CreateParameter("@AxisIV", adVarChar, adParamInput, 100, strAxisIV)
    '    .Parameters.Append .CreateParameter("@AxisV", adVarChar, adParamInput, 50, strAxisV)
        ' R002
        '.Parameters.Append .CreateParameter("@DxNotes", adLongVarChar, adParamInput, Len(strDxNotes))
        '.Parameters("@DxNotes").AppendChunk strDxNotes
        .Parameters.Append .CreateParameter("@DxNotes", adVarChar, adParamInput, 256, strDxNotes)
        .Parameters.Append .CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strUserName)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cnn = New ADODB.Connection  ' Instantiate DB connection --R001
    cnn.Open (CONST_PSYQUEL_CNN) ' open DB connection
    Set cmd.ActiveConnection = cnn
    cmd.Execute ', , adExecuteNoRecords

    Insert = cmd.Parameters(0).Value
    
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

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    
    Set cmd.ActiveConnection = Nothing  ' --R001
    cnn.Close   ' close DB connection
    ' De-allocate space for objects
    Set cmd = Nothing
    Set cnn = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Public Sub Update(ByVal lngPatientDiagnosisID As Long, _
                       Optional ByVal lngAxisIa As Long, Optional ByVal lngAxisIb As Long, _
                       Optional ByVal lngAxisIc As Long, Optional ByVal lngAxisId As Long, _
                       Optional ByVal lngAxisIe As Long, Optional ByVal lngAxisIf As Long, _
                       Optional ByVal lngAxisIg As Long, Optional ByVal lngAxisIh As Long, _
                       Optional ByVal strDxNotes As String)
'--------------------------------------------------------------------
'Date: 09/07/2000
'Author: Joshua Lockwood
'Description: Updates selected records in the tblPatientDiagnosis table.  Calls
'               stored procedure uspUpdPatientDiagnosis.
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'Returns: Long
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    ' Declare connection, returned recordset, and command
    Dim cnn As ADODB.Connection ' Declare DB connection
    Dim cmd As ADODB.Command    ' Declare SQL statement

    On Error GoTo ErrTrap:
    
    'Create the parameter objects
    Set cmd = New ADODB.Command     ' Instantiate SQL statement --R001
    With cmd
        .CommandText = "uspUpdPatientDiagnosis"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@PatientDiagnosisID", adInteger, adParamInput, , lngPatientDiagnosisID)
        .Parameters.Append .CreateParameter("@AxisIa", adInteger, adParamInput, , IIf(lngAxisIa = 0, Null, lngAxisIa))
        .Parameters.Append .CreateParameter("@AxisIb", adInteger, adParamInput, , IIf(lngAxisIb = 0, Null, lngAxisIb))
        .Parameters.Append .CreateParameter("@AxisIc", adInteger, adParamInput, , IIf(lngAxisIc = 0, Null, lngAxisIc))
        .Parameters.Append .CreateParameter("@AxisId", adInteger, adParamInput, , IIf(lngAxisId = 0, Null, lngAxisId))
        .Parameters.Append .CreateParameter("@AxisIe", adInteger, adParamInput, , IIf(lngAxisIe = 0, Null, lngAxisIe))
        .Parameters.Append .CreateParameter("@AxisIf", adInteger, adParamInput, , IIf(lngAxisIf = 0, Null, lngAxisIf))
        .Parameters.Append .CreateParameter("@AxisIg", adInteger, adParamInput, , IIf(lngAxisIg = 0, Null, lngAxisIg))
        .Parameters.Append .CreateParameter("@AxisIh", adInteger, adParamInput, , IIf(lngAxisIh = 0, Null, lngAxisIh))
   '     .Parameters.Append .CreateParameter("@AxisIIa", adInteger, adParamInput, , IIf(lngAxisIIa = 0, Null, lngAxisIIa))
   '     .Parameters.Append .CreateParameter("@AxisIIaModifier", adVarChar, adParamInput, 20, strAxisIIaModifier)
   '     .Parameters.Append .CreateParameter("@AxisIIb", adInteger, adParamInput, , IIf(lngAxisIIb = 0, Null, lngAxisIIb))
   '     .Parameters.Append .CreateParameter("@AxisIIbModifier", adVarChar, adParamInput, 20, strAxisIIbModifier)
   '     .Parameters.Append .CreateParameter("@AxisIII", adVarChar, adParamInput, 100, strAxisIII)
   '     .Parameters.Append .CreateParameter("@AxisIV", adVarChar, adParamInput, 100, strAxisIV)
   '     .Parameters.Append .CreateParameter("@AxisV", adVarChar, adParamInput, 50, strAxisV)
        ' R002
        '.Parameters.Append .CreateParameter("@DxNotes", adLongVarChar, adParamInput, Len(strDxNotes))
        '.Parameters("@DxNotes").AppendChunk strDxNotes
        .Parameters.Append .CreateParameter("@DxNotes", adVarChar, adParamInput, 256, strDxNotes)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
   
    'Assign the connection to the Command object and execute the stored procedure
    Set cnn = New ADODB.Connection  ' Instantiate DB connection --R001
    cnn.Open (CONST_PSYQUEL_CNN)
    Set cmd.ActiveConnection = cnn
    cmd.Execute , , adExecuteNoRecords

    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If
        
    cnn.Close   ' close DB connection
        
    ' De-allocate space for objects
    Set cmd.ActiveConnection = Nothing
    Set cmd = Nothing
    Set cnn = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    
    cmd.ActiveConnection = Nothing
    'cnn.Close   ' close DB connection
    ' De-allocate space for objects
    Set cmd = Nothing
    Set cnn = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub

Public Sub Deleted(ByVal lngPatientDiagnosisID As Long, Optional ByVal blnIncludeDisabled As Boolean = False)
'--------------------------------------------------------------------
'Date: 09/07/2000
'Author: Joshua Lockwood
'Description: Deletes selected record.  Calls
'               stored procedure uspDelPatientDiagnosis.
'Parameters:  lngPatientDiagnosisID - ID of the patient.
'             blnIncludeDisabled - delete flag y to delete n to undel
'Returns: Long
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    ' Declare connection, returned recordset, and command
    Dim cnn As ADODB.Connection ' Declare DB connection
    Dim cmd As ADODB.Command    ' Declare SQL statement

    On Error GoTo ErrTrap:
    
    'Create the parameter objects
    Set cmd = New ADODB.Command     ' Instantiate SQL statement --R001
    With cmd
        .CommandText = "uspDelPatientDiagnosis"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@PatientDiagnosisID", adInteger, adParamInput, , lngPatientDiagnosisID)
        .Parameters.Append .CreateParameter("@IncludeDisabledYN", adVarChar, adParamInput, 1, IIf(blnIncludeDisabled = False, "N", "Y"))
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cnn = New ADODB.Connection  ' Instantiate DB connection --R001
    cnn.Open (CONST_PSYQUEL_CNN) ' open DB connection
    Set cmd.ActiveConnection = cnn
    cmd.Execute , , adExecuteNoRecords

    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If
        
    cnn.Close   ' close DB connection
    ' De-allocate space for objects
    Set cmd.ActiveConnection = Nothing
    Set cmd = Nothing
    Set cnn = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    
    cnn.Close   ' close DB connection
    cmd.ActiveConnection = Nothing
    ' De-allocate space for objects
    Set cmd = Nothing
    Set cnn = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub





