VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CNoteClinicalDB"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CNoteClinicalDB
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Database object designed to implement methods (by calling
'              stored procedures) associated with
'              data affecting the tblNoteClinical table.
'--------------------------------------------------------------------
'  Revisions:
'--------------------------------------------------------------------
Option Explicit

Private Const CLASS_NAME As String = "CNoteClinicalDB"
Private Const TABLE_NAME As String = "tblNoteClinical"

Public Function FetchByID(ByVal lngClinicalNoteID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description: Retrieves records from the tblNoteClinical table.  Calls
'               stored procedure uspSelPatientClinicalNoteByID.
'Parameters: lngClinicalNoteID - 'Where' clause of the SQL statement by
'               which records will be fetched.  Matches tblNoteClinical's
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
        .CommandText = "uspSelPatientClinicalNoteByID"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@ClinicalNoteID", adInteger, adParamInput, , lngClinicalNoteID)
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
'Description: Retrieves records from the tblNoteClinical table.  Calls
'               stored procedure uspSelPatientClinicalNoteByPatient.
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
        .CommandText = "uspSelPatientClinicalNoteByPatient"
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
'Description: Retrieves records from the tblNoteClinical table.  Calls
'               stored procedure uspSelPatientClinicalNoteByEncLog.
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
        .CommandText = "uspSelPatientClinicalNoteByEncLog"
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

Public Function FetchNextNote(ByVal lngUserID As Long, ByVal lngClinicalNoteID As Long, _
                              ByVal lngPatientID As Long, Optional ByVal lngEncLogID As Long = 0) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 10/05/2000
'Author: David Nichol
'Description: Retrieves records from the tblPatientNote table.  Calls
'               stored procedure uspSelNextPatientNote.
'Parameters: lngClinicalNoteID - 'Where' clause of the SQL statement by
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
        .CommandText = "uspSelNextPatientNoteClinical"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@ClinicalNoteID", adInteger, adParamInput, , lngClinicalNoteID)
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

Public Function FetchPrevNote(ByVal lngUserID As Long, ByVal lngClinicalNoteID As Long, ByVal lngPatientID As Long, Optional ByVal lngEncLogID As Long = 0) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 10/05/2000
'Author: David Nichol
'Description: Retrieves records from the tblPatientNote table.  Calls
'               stored procedure uspSelPrevPatientNote.
'Parameters: lngClinicalNoteID - 'Where' clause of the SQL statement by
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
        .CommandText = "uspSelPrevPatientNoteClinical"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@ClinicalNoteID", adInteger, adParamInput, , lngClinicalNoteID)
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
                       ByVal dtApptEndDate As Date, ByVal strAppearance As String, _
                       ByVal strSpeech As String, ByVal strMotor As String, _
                       ByVal strHygiene As String, ByVal strThoughts As String, _
                       ByVal strAffect As String, ByVal strMood As String, _
                       ByVal strMoodText As String, ByVal strCognitive As String, _
                       ByVal strSuicidality As String, ByVal strSuicidalityText As String, _
                       ByVal strMedicationText As String, ByVal strClinicalText As String, _
                       ByVal strTherapeutic As String, ByVal strProgress As String, _
                       ByVal strService As String, ByVal strServiceText As String, _
                       ByVal strRecommend As String, ByVal strRecommendText As String, _
                       ByVal strNoteText As String, ByVal strMipsText As String, ByVal blnLockedYN As Boolean, _
                       ByVal lngSignedBy As Long, ByVal dtDateSigned As Date, _
                       ByVal strUserName As String) As Long
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description: Inserts records into the tblNoteClinical table.  Calls
'               stored procedure uspInsPatientClinicalNote.
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
        .CommandText = "uspInsPatientNoteClinical"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@ClinicalNoteID", adInteger, adParamReturnValue)
        .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID)
        .Parameters.Append .CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID)
        .Parameters.Append .CreateParameter("@EncLogID", adInteger, adParamInput, , IIf(lngEncounterLogID <= 0, Null, lngEncounterLogID))
        .Parameters.Append .CreateParameter("@ClinicID", adInteger, adParamInput, , IIf(lngClinicID <= 0, Null, lngClinicID))
        .Parameters.Append .CreateParameter("@SupervisorID", adInteger, adParamInput, , IIf(lngSupervisorID <= 0, Null, lngSupervisorID))
        .Parameters.Append .CreateParameter("@NoteDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtNoteDate) = 0, Now(), dtNoteDate))
        .Parameters.Append .CreateParameter("@DiagID1", adInteger, adParamInput, , IIf(lngDiagID1 <= 0, Null, lngDiagID1))
        .Parameters.Append .CreateParameter("@ApptStartDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtApptStartDate) = 0, Null, dtApptStartDate))
        .Parameters.Append .CreateParameter("@ApptEndDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtApptEndDate) = 0, Null, dtApptEndDate))
        .Parameters.Append .CreateParameter("@Appearance", adVarChar, adParamInput, 10, Left(strAppearance, 10))
        .Parameters.Append .CreateParameter("@Speech", adVarChar, adParamInput, 10, Left(strSpeech, 10))
        .Parameters.Append .CreateParameter("@Motor", adVarChar, adParamInput, 10, Left(strMotor, 10))
        .Parameters.Append .CreateParameter("@Hygiene", adVarChar, adParamInput, 10, Left(strHygiene, 10))
        .Parameters.Append .CreateParameter("@Thoughts", adVarChar, adParamInput, 20, Left(strThoughts, 20))
        .Parameters.Append .CreateParameter("@Affect", adVarChar, adParamInput, 20, Left(strAffect, 20))
        .Parameters.Append .CreateParameter("@Mood", adVarChar, adParamInput, 20, Left(strMood, 20))
        .Parameters.Append .CreateParameter("@MoodText", adVarChar, adParamInput, 255, Left(strMoodText, 255))
        .Parameters.Append .CreateParameter("@Cognitive", adVarChar, adParamInput, 10, Left(strCognitive, 10))
        .Parameters.Append .CreateParameter("@Suicidality", adVarChar, adParamInput, 10, Left(strSuicidality, 10))
        .Parameters.Append .CreateParameter("@SuicidalityText", adLongVarChar, adParamInput, IIf(Len(strSuicidalityText) = 0, 1, Len(strSuicidalityText)))
        .Parameters("@SuicidalityText").AppendChunk strSuicidalityText
        .Parameters.Append .CreateParameter("@MedicationText", adLongVarChar, adParamInput, IIf(Len(strMedicationText) = 0, 1, Len(strMedicationText)))
        .Parameters("@MedicationText").AppendChunk strMedicationText
        .Parameters.Append .CreateParameter("@ClinicalText", adLongVarChar, adParamInput, IIf(Len(strClinicalText) = 0, 1, Len(strClinicalText)))
        .Parameters("@ClinicalText").AppendChunk strClinicalText
        .Parameters.Append .CreateParameter("@Therapeutic", adVarChar, adParamInput, 30, Left(strTherapeutic, 30))
        .Parameters.Append .CreateParameter("@Progress", adVarChar, adParamInput, 10, Left(strProgress, 10))
        .Parameters.Append .CreateParameter("@Service", adVarChar, adParamInput, 30, Left(strService, 30))
        .Parameters.Append .CreateParameter("@ServiceText", adVarChar, adParamInput, 255, Left(strServiceText, 255))
        .Parameters.Append .CreateParameter("@Recommend", adVarChar, adParamInput, 10, Left(strRecommend, 10))
        .Parameters.Append .CreateParameter("@RecommendText", adLongVarChar, adParamInput, IIf(Len(strRecommendText) = 0, 1, Len(strRecommendText)))
        .Parameters("@RecommendText").AppendChunk strRecommendText
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
    Set cnn = New ADODB.Connection  ' Instantiate DB connection
    cnn.Open (CONST_PSYQUEL_CNN) ' open DB connection
    Set cmd.ActiveConnection = cnn
  
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
Public Sub Update(ByVal lngClinicalNoteID As Long, ByVal lngPatientID As Long, ByVal lngProviderID As Long, _
                       ByVal lngEncounterLogID As Long, ByVal lngClinicID As Long, _
                       ByVal lngSupervisorID As Long, ByVal dtNoteDate As Date, _
                       ByVal lngDiagID1 As Long, ByVal dtApptStartDate As Date, _
                       ByVal dtApptEndDate As Date, ByVal strAppearance As String, _
                       ByVal strSpeech As String, ByVal strMotor As String, _
                       ByVal strHygiene As String, ByVal strThoughts As String, _
                       ByVal strAffect As String, ByVal strMood As String, _
                       ByVal strMoodText As String, ByVal strCognitive As String, _
                       ByVal strSuicidality As String, ByVal strSuicidalityText As String, _
                       ByVal strMedicationText As String, ByVal strClinicalText As String, _
                       ByVal strTherapeutic As String, ByVal strProgress As String, _
                       ByVal strService As String, ByVal strServiceText As String, _
                       ByVal strRecommend As String, ByVal strRecommendText As String, _
                       ByVal strNoteText As String, ByVal strMipsText As String, ByVal blnLockedYN As Boolean, _
                       ByVal lngSignedBy As Long, ByVal dtDateSigned As Date, _
                       ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description: Updates selected records in the tblNoteClinical table.  Calls
'               stored procedure uspUpdPatientClinicalNote.
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'              lngClinicalNoteID lngPatientID lngApptID lngTemplateID lngProviderID strNoteText blnLockedYN
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
        .CommandText = "uspUpdPatientNoteClinical"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@ClinicalNoteID", adInteger, adParamInput, , lngClinicalNoteID)
        .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID)
        .Parameters.Append .CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID)
        .Parameters.Append .CreateParameter("@EncLogID", adInteger, adParamInput, , IIf(lngEncounterLogID <= 0, Null, lngEncounterLogID))
        .Parameters.Append .CreateParameter("@ClinicID", adInteger, adParamInput, , IIf(lngClinicID <= 0, Null, lngClinicID))
        .Parameters.Append .CreateParameter("@SupervisorID", adInteger, adParamInput, , IIf(lngSupervisorID <= 0, Null, lngSupervisorID))
        .Parameters.Append .CreateParameter("@NoteDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtNoteDate) = 0, Now(), dtNoteDate))
        .Parameters.Append .CreateParameter("@DiagID1", adInteger, adParamInput, , IIf(lngDiagID1 <= 0, Null, lngDiagID1))
        .Parameters.Append .CreateParameter("@ApptStartDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtApptStartDate) = 0, Null, dtApptStartDate))
        .Parameters.Append .CreateParameter("@ApptEndDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtApptEndDate) = 0, Null, dtApptEndDate))
        .Parameters.Append .CreateParameter("@Appearance", adVarChar, adParamInput, 10, Left(strAppearance, 10))
        .Parameters.Append .CreateParameter("@Speech", adVarChar, adParamInput, 10, Left(strSpeech, 10))
        .Parameters.Append .CreateParameter("@Motor", adVarChar, adParamInput, 10, Left(strMotor, 10))
        .Parameters.Append .CreateParameter("@Hygiene", adVarChar, adParamInput, 10, Left(strHygiene, 10))
        .Parameters.Append .CreateParameter("@Thoughts", adVarChar, adParamInput, 20, Left(strThoughts, 20))
        .Parameters.Append .CreateParameter("@Affect", adVarChar, adParamInput, 20, Left(strAffect, 20))
        .Parameters.Append .CreateParameter("@Mood", adVarChar, adParamInput, 20, Left(strMood, 20))
        .Parameters.Append .CreateParameter("@MoodText", adVarChar, adParamInput, 255, Left(strMoodText, 255))
        .Parameters.Append .CreateParameter("@Cognitive", adVarChar, adParamInput, 10, Left(strCognitive, 10))
        .Parameters.Append .CreateParameter("@Suicidality", adVarChar, adParamInput, 10, Left(strSuicidality, 10))
        .Parameters.Append .CreateParameter("@SuicidalityText", adLongVarChar, adParamInput, IIf(Len(strSuicidalityText) = 0, 1, Len(strSuicidalityText)))
        .Parameters("@SuicidalityText").AppendChunk strSuicidalityText
        .Parameters.Append .CreateParameter("@MedicationText", adLongVarChar, adParamInput, IIf(Len(strMedicationText) = 0, 1, Len(strMedicationText)))
        .Parameters("@MedicationText").AppendChunk strMedicationText
        .Parameters.Append .CreateParameter("@ClinicalText", adLongVarChar, adParamInput, IIf(Len(strClinicalText) = 0, 1, Len(strClinicalText)))
        .Parameters("@ClinicalText").AppendChunk strClinicalText
        .Parameters.Append .CreateParameter("@Therapeutic", adVarChar, adParamInput, 30, Left(strTherapeutic, 30))
        .Parameters.Append .CreateParameter("@Progress", adVarChar, adParamInput, 10, Left(strProgress, 10))
        .Parameters.Append .CreateParameter("@Service", adVarChar, adParamInput, 30, Left(strService, 30))
        .Parameters.Append .CreateParameter("@ServiceText", adVarChar, adParamInput, 255, Left(strServiceText, 255))
        .Parameters.Append .CreateParameter("@Recommend", adVarChar, adParamInput, 10, Left(strRecommend, 10))
        .Parameters.Append .CreateParameter("@RecommendText", adLongVarChar, adParamInput, IIf(Len(strRecommendText) = 0, 1, Len(strRecommendText)))
        .Parameters("@RecommendText").AppendChunk strRecommendText
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
    
    ' De-allocate space for objects
    cmd.ActiveConnection = Nothing  ' --R001
    cnn.Close   ' close DB connection
    Set cmd = Nothing
    Set cnn = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub

Public Sub LockNote(ByVal lngClinicalNoteID As Long, ByVal lngSignedBy As Long, ByVal dtDateSigned As Date)
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description: Updates a selected record in the tblNoteClinical table, setting
'               fldLockedYN to Y.  Cannot unlock record!!!  Calls stored
'               procedure uspUpdLockPatientClinicalNote.
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
        .CommandText = "uspUpdLockPatientClinicalNote"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@ClinicalNoteID", adInteger, adParamInput, , lngClinicalNoteID)
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
    
    ' De-allocate space for objects
    cmd.ActiveConnection = Nothing  ' --R001
    cnn.Close   ' close DB connection
    Set cmd = Nothing
    Set cnn = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub
Public Sub UnLockNote(ByVal lngClinicalNoteID As Long, ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description: Updates a selected record in the tblNoteClinical table, setting
'               fldLockedYN to N.
'Parameters:  Passes the ID of the record to update.
'Returns: Long
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    Dim cnn As ADODB.Connection ' Declare DB connection
    Dim cmd As ADODB.Command    ' Declare SQL statement

    On Error GoTo ErrTrap:

    Set cmd = New ADODB.Command     ' Instantiate SQL statement
    
    'Create the parameter objects
    With cmd
        .CommandText = "uspUpdUnLockPatientClinicalNote"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@ClinicalNoteID", adInteger, adParamInput, , lngClinicalNoteID)
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
    
    ' De-allocate space for objects
    cmd.ActiveConnection = Nothing  ' --R001
    cnn.Close   ' close DB connection
    Set cmd = Nothing
    Set cnn = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub
Public Sub Delete(ByVal strUserName As String, ByVal lngClinicalNoteID As Long)
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description: Deletes selected record.  Calls
'               stored procedure uspDelPatientClinicalNote.
'Parameters:  lngClinicalNoteID - ID of the patient.
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
        .CommandText = "uspDelPatientClinicalNote"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@ClinicalNoteID", adInteger, adParamInput, , lngClinicalNoteID)
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


