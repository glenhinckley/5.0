VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CNoteProgressDB"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CNoteProgressDB
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Database object designed to implement methods (by calling
'              stored procedures) associated with
'              data affecting the tblNoteProgress table.
'--------------------------------------------------------------------
'  Revisions:
'--------------------------------------------------------------------
Option Explicit

Private Const CLASS_NAME As String = "CNoteProgressDB"
Private Const TABLE_NAME As String = "tblNoteProgress"

Public Function FetchByID(ByVal lngProgressNoteID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description: Retrieves records from the tblNoteProgress table.  Calls
'               stored procedure uspSelPatientProgressNoteByID.
'Parameters: lngProgressNoteID - 'Where' clause of the SQL statement by
'               which records will be fetched.  Matches tblNoteProgress's
'               primary key.
'             lngUserID - UserID of the user performing the fetch.  Not
'               really used currently.
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
        .CommandText = "uspSelPatientProgressNoteByID"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@ProgressNoteID", adInteger, adParamInput, , lngProgressNoteID)
    End With
    
     'Acquire the database connection.
    Set cnn = New ADODB.Connection  ' Instantiate DB connection
    cnn.Open (CONST_PSYQUEL_CNN) ' open DB connection
    Set cmd.ActiveConnection = cnn
    
    'Set cursor and execute
    Set rst = New ADODB.Recordset   ' Instantiate recordset
    rst.CursorLocation = adUseClient
    rst.Open cmd, , adOpenForwardOnly, adLockReadOnly   ' open recordset

    ' Cannot close recordset here --R001
    ' De-allocate space for objects
    Set cmd.ActiveConnection = Nothing
    Set rst.ActiveConnection = Nothing
    cnn.Close   ' close DB connection
    Set cnn = Nothing
    Set cmd = Nothing
    
    Set FetchByID = rst ' Set return value
   
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
Public Function FetchByPatient(ByVal lngUserID As Long, ByVal lngPatientID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description: Retrieves records from the tblNoteProgress table.  Calls
'               stored procedure uspSelPatientProgressNoteByPatient.
'Parameters: lngPatientID - 'Where' clause of the SQL statement by
'               which records will be fetched.
'            lngUserID - UserID of the user performing the fetch.  Not
'               really used currently.
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
        .CommandText = "uspSelPatientProgressNoteByPatient"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID)
    End With
    
     'Acquire the database connection.
    Set cnn = New ADODB.Connection  ' Instantiate DB connection
    cnn.Open (CONST_PSYQUEL_CNN) ' open DB connection
    Set cmd.ActiveConnection = cnn
    
    'Set cursor and execute
    Set rst = New ADODB.Recordset   ' Instantiate recordset
    rst.CursorLocation = adUseClient
    rst.Open cmd, , adOpenForwardOnly, adLockReadOnly   ' open recordset

    
    ' Cannot close recordset here
    ' De-allocate space for objects
    Set cmd.ActiveConnection = Nothing
    Set rst.ActiveConnection = Nothing
    cnn.Close   ' close DB connection
    Set cnn = Nothing
    Set cmd = Nothing
    
    Set FetchByPatient = rst ' Set return value
   
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

Public Function FetchByEncounter(ByVal lngUserID As Long, ByVal lngEncLogID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description: Retrieves records from the tblNoteProgress table.  Calls
'               stored procedure uspSelPatientProgressNoteByEncLog.
'Parameters: lngEncLogID - 'Where' clause of the SQL statement by
'               which records will be fetched.
'             lngUserID - UserID of the user performing the fetch.  Not
'               really used currently.
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

    ' Instantiate the objects
    Set cmd = New ADODB.Command     ' Instantiate SQL statement
    
    'Create the parameter objects
    With cmd
        .CommandText = "uspSelPatientProgressNoteByEncLog"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@EncounterLogID", adInteger, adParamInput, , lngEncLogID)
    End With
    
     'Acquire the database connection.
    Set cnn = New ADODB.Connection  ' Instantiate DB connection
    cnn.Open (CONST_PSYQUEL_CNN) ' open DB connection
    Set cmd.ActiveConnection = cnn
    
    'Set cursor and execute
    Set rst = New ADODB.Recordset   ' Instantiate recordset
    rst.CursorLocation = adUseClient
    rst.Open cmd, , adOpenForwardOnly, adLockReadOnly   ' open recordset

    
    'Disconnect the recordset
    ' Cannot close recordset here
    'rst.Close   ' close recordset
    
    ' De-allocate space for objects
    'Set cmd.ActiveConnection = Nothing
    Set rst.ActiveConnection = Nothing
    cnn.Close   ' close DB connection
    Set cnn = Nothing
    Set cmd = Nothing
    
    Set FetchByEncounter = rst ' Set return value
   
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


Public Function FetchNextNote(ByVal lngUserID As Long, ByVal lngProgressNoteID As Long, _
                              ByVal lngPatientID As Long, Optional ByVal lngEncLogID As Long = 0) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 10/05/2000
'Author: David Nichol
'Description: Retrieves records from the tblPatientNote table.  Calls
'               stored procedure uspSelNextPatientNote.
'Parameters: lngProgressNoteID - 'Where' clause of the SQL statement by
'               which records will be fetched.  Returns row with next
'               highest patientNoteID but same patientID.
'            lngPatientID - select row for this patient
'            lngUserID - UserID of the user performing the fetch.  Not
'               really used currently.
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

    ' Instantiate the objects
    Set cmd = New ADODB.Command     ' Instantiate SQL statement
    
    'Create the parameter objects
    With cmd
        .CommandText = "uspSelNextPatientNoteProgress"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@ProgressNoteID", adInteger, adParamInput, , lngProgressNoteID)
        .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID)
        .Parameters.Append .CreateParameter("@EncounterLogID", adInteger, adParamInput, , lngEncLogID)
    End With
    
     'Acquire the database connection.
    Set cnn = New ADODB.Connection  ' Instantiate DB connection
    cnn.Open (CONST_PSYQUEL_CNN) ' open DB connection
    Set cmd.ActiveConnection = cnn
    
    'Set cursor and execute
    Set rst = New ADODB.Recordset   ' Instantiate recordset
    rst.CursorLocation = adUseClient
    rst.Open cmd, , adOpenForwardOnly, adLockReadOnly   ' open recordset

    
    'Disconnect the recordset
    ' Cannot close recordset here
    'rst.Close   ' close recordset
    
    ' De-allocate space for objects
    Set cmd.ActiveConnection = Nothing
    Set rst.ActiveConnection = Nothing
    cnn.Close   ' close DB connection
    Set cnn = Nothing
    Set cmd = Nothing
    
    Set FetchNextNote = rst ' Set return value
   
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

Public Function FetchPrevNote(ByVal lngUserID As Long, ByVal lngProgressNoteID As Long, ByVal lngPatientID As Long, Optional ByVal lngEncLogID As Long = 0) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 10/05/2000
'Author: David Nichol
'Description: Retrieves records from the tblPatientNote table.  Calls
'               stored procedure uspSelPrevPatientNote.
'Parameters: lngProgressNoteID - 'Where' clause of the SQL statement by
'               which records will be fetched.  Returns row with next
'               lowest patientNoteID but same patientID.
'            lngPatientID - select row for this patient
'            lngUserID - UserID of the user performing the fetch.  Not
'               really used currently.
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

    ' Instantiate the objects
    Set cmd = New ADODB.Command     ' Instantiate SQL statement
    
    'Create the parameter objects
    With cmd
        .CommandText = "uspSelPrevPatientNoteProgress"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@ProgressNoteID", adInteger, adParamInput, , lngProgressNoteID)
        .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID)
        .Parameters.Append .CreateParameter("@EncounterLogID", adInteger, adParamInput, , lngEncLogID)
    End With
    
     'Acquire the database connection.
    Set cnn = New ADODB.Connection  ' Instantiate DB connection
    cnn.Open (CONST_PSYQUEL_CNN) ' open DB connection
    Set cmd.ActiveConnection = cnn
    
    'Set cursor and execute
    Set rst = New ADODB.Recordset   ' Instantiate recordset
    rst.CursorLocation = adUseClient
    rst.Open cmd, , adOpenForwardOnly, adLockReadOnly   ' open recordset

    
    'Disconnect the recordset
    ' Cannot close recordset here
    'rst.Close   ' close recordset
    
    ' De-allocate space for objects
    Set cmd.ActiveConnection = Nothing
    Set rst.ActiveConnection = Nothing
    cnn.Close   ' close DB connection
    Set cnn = Nothing
    Set cmd = Nothing
    
    Set FetchPrevNote = rst ' Set return value
   
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

Public Function Insert(ByVal lngPatientID As Long, ByVal lngProviderID As Long, _
                       ByVal lngEncounterLogID As Long, ByVal lngClinicID As Long, _
                       ByVal lngSupervisorID As Long, ByVal dtNoteDate As Date, _
                       ByVal lngDiagID1 As Long, ByVal dtApptStartDate As Date, _
                       ByVal dtApptEndDate As Date, ByVal strComplaintText As String, _
                       ByVal strDiagnosisText As String, ByVal strSTGText As String, _
                       ByVal strLTGText As String, ByVal strSessionText As String, _
                       ByVal strProgressText As String, ByVal strGoalsText As String, _
                       ByVal strNoteText As String, ByVal strMipsText As String, ByVal blnLockedYN As Boolean, _
                       ByVal lngSignedBy As Long, ByVal dtDateSigned As Date, _
                       ByVal strUserName As String) As Long
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description: Inserts records into the tblNoteProgress table.  Calls
'               stored procedure uspInsPatientProgressNote.
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'Returns: Long
'--------------------------------------------------------------------

    ' Declare connection, returned recordset, and command
    Dim cnn As ADODB.Connection ' Declare DB connection
    Dim cmd As ADODB.Command    ' Declare SQL statement

    On Error GoTo ErrTrap:

    ' Instantiate the objects
    Set cmd = New ADODB.Command     ' Instantiate SQL statement
    
    'Create the parameter objects
    With cmd
        .CommandText = "uspInsPatientNoteProgress"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@ProgressNoteID", adInteger, adParamReturnValue)
        .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID)
        .Parameters.Append .CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID)
        .Parameters.Append .CreateParameter("@EncLogID", adInteger, adParamInput, , IIf(lngEncounterLogID <= 0, Null, lngEncounterLogID))
        .Parameters.Append .CreateParameter("@ClinicID", adInteger, adParamInput, , IIf(lngClinicID <= 0, Null, lngClinicID))
        .Parameters.Append .CreateParameter("@SupervisorID", adInteger, adParamInput, , IIf(lngSupervisorID <= 0, Null, lngSupervisorID))
        .Parameters.Append .CreateParameter("@NoteDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtNoteDate) = 0, Now(), dtNoteDate))
        .Parameters.Append .CreateParameter("@DiagID1", adInteger, adParamInput, , IIf(lngDiagID1 <= 0, Null, lngDiagID1))
        .Parameters.Append .CreateParameter("@ApptStartDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtApptStartDate) = 0, Null, dtApptStartDate))
        .Parameters.Append .CreateParameter("@ApptEndDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtApptEndDate) = 0, Null, dtApptEndDate))
        .Parameters.Append .CreateParameter("@ComplaintText", adLongVarChar, adParamInput, IIf(Len(strComplaintText) = 0, 1, Len(strComplaintText)))
        .Parameters("@ComplaintText").AppendChunk strComplaintText
        .Parameters.Append .CreateParameter("@DiagnosisText", adLongVarChar, adParamInput, IIf(Len(strDiagnosisText) = 0, 1, Len(strDiagnosisText)))
        .Parameters("@DiagnosisText").AppendChunk strDiagnosisText
        .Parameters.Append .CreateParameter("@STGText", adLongVarChar, adParamInput, IIf(Len(strSTGText) = 0, 1, Len(strSTGText)))
        .Parameters("@STGText").AppendChunk strSTGText
        .Parameters.Append .CreateParameter("@LTGText", adLongVarChar, adParamInput, IIf(Len(strLTGText) = 0, 1, Len(strLTGText)))
        .Parameters("@LTGText").AppendChunk strLTGText
        .Parameters.Append .CreateParameter("@SessionText", adLongVarChar, adParamInput, IIf(Len(strSessionText) = 0, 1, Len(strSessionText)))
        .Parameters("@SessionText").AppendChunk strSessionText
        .Parameters.Append .CreateParameter("@ProgressText", adLongVarChar, adParamInput, IIf(Len(strProgressText) = 0, 1, Len(strProgressText)))
        .Parameters("@ProgressText").AppendChunk strProgressText
        .Parameters.Append .CreateParameter("@GoalsText", adLongVarChar, adParamInput, IIf(Len(strGoalsText) = 0, 1, Len(strGoalsText)))
        .Parameters("@GoalsText").AppendChunk strGoalsText
        .Parameters.Append .CreateParameter("@NoteText", adLongVarChar, adParamInput, IIf(Len(strNoteText) = 0, 1, Len(strNoteText)))
        .Parameters("@NoteText").AppendChunk strNoteText
        .Parameters.Append .CreateParameter("@MipsText", adVarChar, adParamInput, 128, Left(strMipsText, 128))
        .Parameters.Append .CreateParameter("@LockedYN", adVarChar, adParamInput, 1, IIf(blnLockedYN = False, "N", "Y"))
        .Parameters.Append .CreateParameter("@SignedBy", adInteger, adParamInput, , IIf(lngSignedBy <= 0, Null, lngSignedBy))
        .Parameters.Append .CreateParameter("@DateSigned", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDateSigned) = 0, Null, dtDateSigned))
        .Parameters.Append .CreateParameter("@AddedBy", adVarChar, adParamInput, 30, Left(strUserName, 30))
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cnn = New ADODB.Connection
    cnn.Open (CONST_PSYQUEL_CNN)
    Set cmd.ActiveConnection = cnn
    cmd.Execute , , adExecuteNoRecords
    
    Insert = cmd.Parameters(0).Value
    
    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If
        
     'Acquire the database connection.
   ' Set cnn = New ADODB.Connection  ' Instantiate DB connection
   ' cnn.Open (CONST_PSYQUEL_CNN) ' open DB connection
   ' Set cmd.ActiveConnection = cnn
  
    ' De-allocate space for objects
    Set cmd.ActiveConnection = Nothing
    cnn.Close   ' close DB connection
    Set cnn = Nothing
    Set cmd = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Function
   
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    
    ' De-allocate space for objects
    Set cmd.ActiveConnection = Nothing  ' --R001
    cnn.Close   ' close DB connection
    Set cmd = Nothing
    Set cnn = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Public Sub Update(ByVal lngProgressNoteID As Long, ByVal lngPatientID As Long, ByVal lngProviderID As Long, _
                       ByVal lngEncounterLogID As Long, ByVal lngClinicID As Long, _
                       ByVal lngSupervisorID As Long, ByVal dtNoteDate As Date, _
                       ByVal lngDiagID1 As Long, ByVal dtApptStartDate As Date, _
                       ByVal dtApptEndDate As Date, ByVal strComplaintText As String, _
                       ByVal strDiagnosisText As String, ByVal strSTGText As String, _
                       ByVal strLTGText As String, ByVal strSessionText As String, _
                       ByVal strProgressText As String, ByVal strGoalsText As String, _
                       ByVal strNoteText As String, ByVal strMipsText As String, ByVal blnLockedYN As Boolean, _
                       ByVal lngSignedBy As Long, ByVal dtDateSigned As Date, _
                       ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description: Updates selected records in the tblNoteProgress table.  Calls
'               stored procedure uspUpdPatientProgressNote.
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'              lngProgressNoteID lngPatientID lngApptID lngTemplateID lngProviderID strNoteText blnLockedYN
'Returns: Long
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    ' Declare connection, returned recordset, and command
    Dim cnn As ADODB.Connection ' Declare DB connection
    Dim cmd As ADODB.Command    ' Declare SQL statement

    On Error GoTo ErrTrap:

    Set cmd = New ADODB.Command     ' Instantiate SQL statement
    
    'Create the parameter objects
    With cmd
        .CommandText = "uspUpdPatientNoteProgress"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@ProgressNoteID", adInteger, adParamInput, , lngProgressNoteID)
        .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID)
        .Parameters.Append .CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID)
        .Parameters.Append .CreateParameter("@EncLogID", adInteger, adParamInput, , IIf(lngEncounterLogID <= 0, Null, lngEncounterLogID))
        .Parameters.Append .CreateParameter("@ClinicID", adInteger, adParamInput, , IIf(lngClinicID <= 0, Null, lngClinicID))
        .Parameters.Append .CreateParameter("@SupervisorID", adInteger, adParamInput, , IIf(lngSupervisorID <= 0, Null, lngSupervisorID))
        .Parameters.Append .CreateParameter("@NoteDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtNoteDate) = 0, Now(), dtNoteDate))
        .Parameters.Append .CreateParameter("@DiagID1", adInteger, adParamInput, , IIf(lngDiagID1 <= 0, Null, lngDiagID1))
        .Parameters.Append .CreateParameter("@ApptStartDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtApptStartDate) = 0, Null, dtApptStartDate))
        .Parameters.Append .CreateParameter("@ApptEndDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtApptEndDate) = 0, Null, dtApptEndDate))
        .Parameters.Append .CreateParameter("@ComplaintText", adLongVarChar, adParamInput, IIf(Len(strComplaintText) = 0, 1, Len(strComplaintText)))
        .Parameters("@ComplaintText").AppendChunk strComplaintText
        .Parameters.Append .CreateParameter("@DiagnosisText", adLongVarChar, adParamInput, IIf(Len(strDiagnosisText) = 0, 1, Len(strDiagnosisText)))
        .Parameters("@DiagnosisText").AppendChunk strDiagnosisText
        .Parameters.Append .CreateParameter("@STGText", adLongVarChar, adParamInput, IIf(Len(strSTGText) = 0, 1, Len(strSTGText)))
        .Parameters("@STGText").AppendChunk strSTGText
        .Parameters.Append .CreateParameter("@LTGText", adLongVarChar, adParamInput, IIf(Len(strLTGText) = 0, 1, Len(strLTGText)))
        .Parameters("@LTGText").AppendChunk strLTGText
        .Parameters.Append .CreateParameter("@SessionText", adLongVarChar, adParamInput, IIf(Len(strSessionText) = 0, 1, Len(strSessionText)))
        .Parameters("@SessionText").AppendChunk strSessionText
        .Parameters.Append .CreateParameter("@ProgressText", adLongVarChar, adParamInput, IIf(Len(strProgressText) = 0, 1, Len(strProgressText)))
        .Parameters("@ProgressText").AppendChunk strProgressText
        .Parameters.Append .CreateParameter("@GoalsText", adLongVarChar, adParamInput, IIf(Len(strGoalsText) = 0, 1, Len(strGoalsText)))
        .Parameters("@GoalsText").AppendChunk strGoalsText
        .Parameters.Append .CreateParameter("@NoteText", adLongVarChar, adParamInput, IIf(Len(strNoteText) = 0, 1, Len(strNoteText)))
        .Parameters("@NoteText").AppendChunk strNoteText
        .Parameters.Append .CreateParameter("@MipsText", adVarChar, adParamInput, 128, Left(strMipsText, 128))
        .Parameters.Append .CreateParameter("@LockedYN", adVarChar, adParamInput, 1, IIf(blnLockedYN = False, "N", "Y"))
        .Parameters.Append .CreateParameter("@SignedBy", adInteger, adParamInput, , IIf(lngSignedBy <= 0, Null, lngSignedBy))
        .Parameters.Append .CreateParameter("@DateSigned", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDateSigned) = 0, Null, dtDateSigned))
        .Parameters.Append .CreateParameter("@UserName", adVarChar, adParamInput, 30, Left(strUserName, 30))
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
   
    'Assign the connection to the Command object and execute the stored procedure
    Set cnn = New ADODB.Connection  ' Instantiate DB connection
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
    
    ' De-allocate space for objects
    cmd.ActiveConnection = Nothing  ' --R001
    cnn.Close   ' close DB connection
    Set cmd = Nothing
    Set cnn = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub

Public Sub LockNote(ByVal lngProgressNoteID As Long, ByVal lngSignedBy As Long, ByVal dtDateSigned As Date)
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description: Updates a selected record in the tblNoteProgress table, setting
'               fldLockedYN to Y.  Cannot unlock record!!!  Calls stored
'               procedure uspUpdLockPatientProgressNote.
'Parameters:  Passes the ID of the record to update.
'Returns: Long
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    ' Declare connection, returned recordset, and command
    Dim cnn As ADODB.Connection ' Declare DB connection
    Dim cmd As ADODB.Command    ' Declare SQL statement

    On Error GoTo ErrTrap:

    Set cmd = New ADODB.Command     ' Instantiate SQL statement
    
    'Create the parameter objects
    With cmd
        .CommandText = "uspUpdLockPatientProgressNote"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@ProgressNoteID", adInteger, adParamInput, , lngProgressNoteID)
        .Parameters.Append .CreateParameter("@SignedBy", adInteger, adParamInput, , IIf(lngSignedBy <= 0, Null, lngSignedBy))
        .Parameters.Append .CreateParameter("@DateSigned", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDateSigned) = 0, Null, dtDateSigned))
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
   
    'Assign the connection to the Command object and execute the stored procedure
    Set cnn = New ADODB.Connection  ' Instantiate DB connection
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
    
    ' De-allocate space for objects
    cmd.ActiveConnection = Nothing  ' --R001
    cnn.Close   ' close DB connection
    Set cmd = Nothing
    Set cnn = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub
Public Sub UnLockNote(ByVal lngProgressNoteID As Long, ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description: Updates a selected record in the tblNoteProgress table, setting
'               fldLockedYN to N.
'Parameters:  Passes the ID of the record to update.
'Returns: Long
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    ' Declare connection, returned recordset, and command
    Dim cnn As ADODB.Connection ' Declare DB connection
    Dim cmd As ADODB.Command    ' Declare SQL statement

    On Error GoTo ErrTrap:

    Set cmd = New ADODB.Command     ' Instantiate SQL statement
    
    'Create the parameter objects
    With cmd
        .CommandText = "uspUpdUnLockPatientProgressNote"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@ProgressNoteID", adInteger, adParamInput, , lngProgressNoteID)
        .Parameters.Append .CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
   
    'Assign the connection to the Command object and execute the stored procedure
    Set cnn = New ADODB.Connection  ' Instantiate DB connection
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
    
    ' De-allocate space for objects
    cmd.ActiveConnection = Nothing  ' --R001
    cnn.Close   ' close DB connection
    Set cmd = Nothing
    Set cnn = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub


Public Sub Delete(ByVal strUserName As String, ByVal lngProgressNoteID As Long)
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description: Deletes selected record.  Calls
'               stored procedure uspDelPatientProgressNote.
'Parameters:  lngProgressNoteID - ID of the patient.
'             strUserName - username of user deleting data : not used now
'Returns: Long
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    ' Declare connection, returned recordset, and command
    Dim cnn As ADODB.Connection ' Declare DB connection
    Dim cmd As ADODB.Command    ' Declare SQL statement

    On Error GoTo ErrTrap:

    ' Instantiate the objects
    ' Set rst = New ADODB.Recordset   ' Instantiate recordset
    Set cmd = New ADODB.Command     ' Instantiate SQL statement
    Set cnn = New ADODB.Connection  ' Instantiate DB connection
    
    'Create the parameter objects
    With cmd
        .CommandText = "uspDelPatientProgressNote"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@ProgressNoteID", adInteger, adParamInput, , lngProgressNoteID)
        .Parameters.Append .CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Assign the connection to the Command object and execute the stored procedure
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
    
    ' De-allocate space for objects
    cmd.ActiveConnection = Nothing
    cnn.Close
    Set cmd = Nothing
    Set cnn = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub




