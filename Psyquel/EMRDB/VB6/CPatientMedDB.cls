VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CPatientMedDB"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CPatientMedDB
'Date: 09/07/2000
'Author: Joshua Lockwood
'Description:  Database object designed to implement methods (by calling
'              stored procedures) associated with
'              data affecting the tblPatientMedication table.
'--------------------------------------------------------------------
'  Revisions:
'   --R001 : Joshua Lockwood, 15Sep00 : Changes to avoid memory leaks
'
'--------------------------------------------------------------------
Option Explicit

Private Const CLASS_NAME As String = "CPatientMedDB"
Private Const TABLE_NAME As String = "tblPatientMedication"


Public Function FetchByID(ByVal lngPatientMedicationID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 09/07/2000
'Author: Joshua Lockwood
'Description: Retrieves records from the tblPatientMedication table.  Calls
'               stored procedure uspSelPatientMedicationByID.
'Parameters: lngPatientMedicationID - 'Where' clause of the SQL statement by
'               which records will be fetched.  Matches tblPatientMedication's
'               primary key.
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
        .CommandText = "uspSelPatientMedicationByID"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@PatientMedicationID", adInteger, adParamInput, , lngPatientMedicationID)
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
'Description: Retrieves records from the tblPatientMedication table.  Calls
'               stored procedure uspSelPatientMedicationByPatient.
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
        .CommandText = "uspSelPatientMedicationByPatient"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID)
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
Public Function Insert(ByVal lngPatientID As Long, ByVal strDrugName As String, _
                    ByVal strInstructions As String, ByVal strPrescriber As String, _
                    ByVal strUserName As String, Optional ByVal dtStartDate As Date, _
                    Optional ByVal dtEndDate As Date = 0) As Long
'--------------------------------------------------------------------
'Date: 09/07/2000
'Author: Joshua Lockwood
'Description: Inserts records into the tblPatientMedication table.  Calls
'               stored procedure uspInsPatientMedication.
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
        .CommandText = "uspInsPatientMedication"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@PatientMedID", adInteger, adParamReturnValue)
        .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID)
        .Parameters.Append .CreateParameter("@DrugName", adVarChar, adParamInput, 100, strDrugName)
        .Parameters.Append .CreateParameter("@Instructions", adVarChar, adParamInput, 100, strInstructions)
        .Parameters.Append .CreateParameter("@Prescriber", adVarChar, adParamInput, 40, strPrescriber)
        .Parameters.Append .CreateParameter("@StartDate", adDBTimeStamp, adParamInput, , IIf(dtStartDate = 0, Null, dtStartDate))
        .Parameters.Append .CreateParameter("@EndDate", adDBTimeStamp, adParamInput, , IIf(dtEndDate = 0, Null, dtEndDate))
        .Parameters.Append .CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strUserName)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
   End With
    'Assign the connection to the Command object and execute the stored procedure
    
    Set cnn = New ADODB.Connection  ' Instantiate DB connection --R001
    cnn.Open (CONST_PSYQUEL_CNN) ' open DB connection
    Set cmd.ActiveConnection = cnn
    cmd.Execute , , adExecuteNoRecords

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
    
    ' De-allocate space for objects
    cmd.ActiveConnection = Nothing
    cnn.Close   ' close DB connection
    Set cmd = Nothing
    Set cnn = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Public Sub Update(ByVal lngPatientMedicationID As Long, ByVal strDrugName As String, _
                    ByVal strInstructions As String, ByVal strPrescriber As String, _
                    ByVal strUserName As String, _
                    Optional ByVal dtStartDate As Date, Optional ByVal dtEndDate As Date)
'--------------------------------------------------------------------
'Date: 09/07/2000
'Author: Joshua Lockwood
'Description: Updates selected records in the tblPatientMedication table.  Calls
'               stored procedure uspUpdPatientMedication.
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'               strUserName is passed in case we want auditing in the future...
'                   for now, it does nothing.
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
        .CommandText = "uspUpdPatientMedication"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@PatientMedicationID", adInteger, adParamInput, , lngPatientMedicationID)
        .Parameters.Append .CreateParameter("@DrugName", adVarChar, adParamInput, 100, strDrugName)
        .Parameters.Append .CreateParameter("@Instructions", adVarChar, adParamInput, 100, strInstructions)
        .Parameters.Append .CreateParameter("@Prescriber", adVarChar, adParamInput, 40, strPrescriber)
        .Parameters.Append .CreateParameter("@StartDate", adDBTimeStamp, adParamInput, , IIf(dtStartDate = 0, Null, dtStartDate))
        .Parameters.Append .CreateParameter("@EndDate", adDBTimeStamp, adParamInput, , IIf(dtEndDate = 0, Null, dtEndDate))
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
    
    cmd.ActiveConnection = Nothing
    cnn.Close   ' close DB connection
    ' De-allocate space for objects
    Set cmd = Nothing
    Set cnn = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub

Public Sub Deleted(ByVal lngPatientMedicationID As Long, ByVal blnDisabledYN As Boolean, ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 09/07/2000
'Author: Joshua Lockwood
'Description: Deletes selected record.  Calls
'               stored procedure uspDelPatientMedication.
'Parameters:  lngPatientMedicationID - ID of the patient.
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
        .CommandText = "uspDelPatientMedication"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@PatientMedicationID", adInteger, adParamInput, , lngPatientMedicationID)
        .Parameters.Append .CreateParameter("@DisabledYN", adVarChar, adParamInput, 1, IIf(blnDisabledYN = False, "N", "Y"))
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
    ' Set rst.ActiveConnection = Nothing
    Set cnn = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    
    cmd.ActiveConnection = Nothing
    cnn.Close   ' close DB connection
    ' De-allocate space for objects
    Set cmd = Nothing
    Set cnn = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub







