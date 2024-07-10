VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CNotePlayDB"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CNotePlayDB
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Database object designed to implement methods (by calling
'              stored procedures) associated with
'              data affecting the tblNotePlay table.
'--------------------------------------------------------------------
'  Revisions:
'--------------------------------------------------------------------
Option Explicit

Private Const CLASS_NAME As String = "CNotePlayDB"
Private Const TABLE_NAME As String = "tblNotePlay"

Public Function FetchByID(ByVal lngPlayNoteID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description: Retrieves records from the tblNotePlay table.  Calls
'               stored procedure uspSelPatientPlayNoteByID.
'Parameters: lngPlayNoteID - 'Where' clause of the SQL statement by
'               which records will be fetched.  Matches tblNotePlay's
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
        .CommandText = "uspSelPatientPlayNoteByID"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@PlayNoteID", adInteger, adParamInput, , lngPlayNoteID)
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
'Description: Retrieves records from the tblNotePlay table.  Calls
'               stored procedure uspSelPatientPlayNoteByPatient.
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
        .CommandText = "uspSelPatientPlayNoteByPatient"
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
'Description: Retrieves records from the tblNotePlay table.  Calls
'               stored procedure uspSelPatientPlayNoteByEncLog.
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
        .CommandText = "uspSelPatientPlayNoteByEncLog"
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


Public Function FetchNextNote(ByVal lngUserID As Long, ByVal lngPlayNoteID As Long, _
                              ByVal lngPatientID As Long, Optional ByVal lngEncLogID As Long = 0) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 10/05/2000
'Author: David Nichol
'Description: Retrieves records from the tblPatientNote table.  Calls
'               stored procedure uspSelNextPatientNote.
'Parameters: lngPlayNoteID - 'Where' clause of the SQL statement by
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
        .CommandText = "uspSelNextPatientNotePlay"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@PlayNoteID", adInteger, adParamInput, , lngPlayNoteID)
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

Public Function FetchPrevNote(ByVal lngUserID As Long, ByVal lngPlayNoteID As Long, ByVal lngPatientID As Long, Optional ByVal lngEncLogID As Long = 0) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 10/05/2000
'Author: David Nichol
'Description: Retrieves records from the tblPatientNote table.  Calls
'               stored procedure uspSelPrevPatientNote.
'Parameters: lngPlayNoteID - 'Where' clause of the SQL statement by
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
        .CommandText = "uspSelPrevPatientNotePlay"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@PlayNoteID", adInteger, adParamInput, , lngPlayNoteID)
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
                       ByVal dtApptEndDate As Date, ByVal strHappy As String, _
                       ByVal strSad As String, ByVal strAngry As String, ByVal strAfraid As String, ByVal strConfident As String, _
                       ByVal strConfused As String, ByVal strCurious As String, ByVal strIntensity As String, ByVal strToys As String, _
                       ByVal strPlayText As String, ByVal strVerbalText As String, ByVal strChildText As String, ByVal strTherapistText As String, _
                       ByVal strRoomText As String, ByVal strStructureText As String, ByVal strRealityText As String, ByVal strBehaviorText As String, _
                       ByVal lngActivity As Long, ByVal lngPlay As Long, ByVal lngContact As Long, ByVal strExploreText As String, ByVal strRelationText As String, _
                       ByVal strControlText As String, ByVal strHelplessText As String, ByVal strAgressiveText As String, ByVal strSafetyText As String, _
                       ByVal strMasterText As String, ByVal strNurtureText As String, ByVal strGrievingText As String, ByVal strSexualText As String, _
                       ByVal strPlayOtherText As String, ByVal lngOverallSad As Long, ByVal lngOverallAnxious As Long, ByVal lngOverallFrustrated As Long, _
                       ByVal lngOverallDependence As Long, ByVal lngOverallMaturity As Long, ByVal lngOverallFocus As Long, ByVal lngOverallImpulsive As Long, _
                       ByVal lngOverallSpontaneity As Long, ByVal lngOverallIsolation As Long, ByVal lngOverallDetachment As Long, ByVal strProgressText As String, _
                       ByVal strPlanText As String, ByVal strNoteText As String, ByVal strMipsText As String, ByVal blnLockedYN As Boolean, _
                       ByVal lngSignedBy As Long, ByVal dtDateSigned As Date, _
                       ByVal strUserName As String) As Long
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description: Inserts records into the tblNotePlay table.  Calls
'               stored procedure uspInsPatientPlayNote.
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
        .CommandText = "uspInsPatientNotePlay"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@PlayNoteID", adInteger, adParamReturnValue)
        .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID)
        .Parameters.Append .CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID)
        .Parameters.Append .CreateParameter("@EncLogID", adInteger, adParamInput, , IIf(lngEncounterLogID <= 0, Null, lngEncounterLogID))
        .Parameters.Append .CreateParameter("@ClinicID", adInteger, adParamInput, , IIf(lngClinicID <= 0, Null, lngClinicID))
        .Parameters.Append .CreateParameter("@SupervisorID", adInteger, adParamInput, , IIf(lngSupervisorID <= 0, Null, lngSupervisorID))
        .Parameters.Append .CreateParameter("@NoteDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtNoteDate) = 0, Now(), dtNoteDate))
        .Parameters.Append .CreateParameter("@DiagID1", adInteger, adParamInput, , IIf(lngDiagID1 <= 0, Null, lngDiagID1))
        .Parameters.Append .CreateParameter("@ApptStartDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtApptStartDate) = 0, Null, dtApptStartDate))
        .Parameters.Append .CreateParameter("@ApptEndDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtApptEndDate) = 0, Null, dtApptEndDate))
        .Parameters.Append .CreateParameter("@Happy", adVarChar, adParamInput, 20, Left(strHappy, 20))
        .Parameters.Append .CreateParameter("@Sad", adVarChar, adParamInput, 20, Left(strSad, 20))
        .Parameters.Append .CreateParameter("@Angry", adVarChar, adParamInput, 20, Left(strAngry, 20))
        .Parameters.Append .CreateParameter("@Afraid", adVarChar, adParamInput, 20, Left(strAfraid, 20))
        .Parameters.Append .CreateParameter("@Confident", adVarChar, adParamInput, 10, Left(strConfident, 10))
        .Parameters.Append .CreateParameter("@Confused", adVarChar, adParamInput, 20, Left(strConfused, 20))
        .Parameters.Append .CreateParameter("@Curious", adVarChar, adParamInput, 10, Left(strCurious, 10))
        .Parameters.Append .CreateParameter("@Intensity", adVarChar, adParamInput, 20, Left(strIntensity, 20))
        .Parameters.Append .CreateParameter("@Toys", adVarChar, adParamInput, 40, Left(strToys, 40))
        .Parameters.Append .CreateParameter("@PlayText", adLongVarChar, adParamInput, IIf(Len(strPlayText) = 0, 1, Len(strPlayText)))
        .Parameters("@PlayText").AppendChunk strPlayText
        .Parameters.Append .CreateParameter("@VerbalText", adLongVarChar, adParamInput, IIf(Len(strVerbalText) = 0, 1, Len(strVerbalText)))
        .Parameters("@VerbalText").AppendChunk strVerbalText
        .Parameters.Append .CreateParameter("@ChildText", adVarChar, adParamInput, 256, Left(strChildText, 256))
        .Parameters.Append .CreateParameter("@TherapistText", adVarChar, adParamInput, 256, Left(strTherapistText, 256))
        .Parameters.Append .CreateParameter("@RoomText", adVarChar, adParamInput, 256, Left(strRoomText, 256))
        .Parameters.Append .CreateParameter("@StructureText", adVarChar, adParamInput, 256, Left(strStructureText, 256))
        .Parameters.Append .CreateParameter("@RealityText", adVarChar, adParamInput, 256, Left(strRealityText, 256))
        .Parameters.Append .CreateParameter("@BehaviorText", adVarChar, adParamInput, 256, Left(strBehaviorText, 256))
        .Parameters.Append .CreateParameter("@Activity", adInteger, adParamInput, , IIf(lngActivity < 0, Null, lngActivity))
        .Parameters.Append .CreateParameter("@Play", adInteger, adParamInput, , IIf(lngPlay < 0, Null, lngPlay))
        .Parameters.Append .CreateParameter("@Contact", adInteger, adParamInput, , IIf(lngContact < 0, Null, lngContact))
        .Parameters.Append .CreateParameter("@ExploreText", adVarChar, adParamInput, 256, Left(strExploreText, 256))
        .Parameters.Append .CreateParameter("@RelationText", adVarChar, adParamInput, 256, Left(strRelationText, 256))
        .Parameters.Append .CreateParameter("@ControlText", adVarChar, adParamInput, 256, Left(strControlText, 256))
        .Parameters.Append .CreateParameter("@HelplessText", adVarChar, adParamInput, 256, Left(strHelplessText, 256))
        .Parameters.Append .CreateParameter("@AgressiveText", adVarChar, adParamInput, 256, Left(strAgressiveText, 256))
        .Parameters.Append .CreateParameter("@SafetyText", adVarChar, adParamInput, 256, Left(strSafetyText, 256))
        .Parameters.Append .CreateParameter("@MasterText", adVarChar, adParamInput, 256, Left(strMasterText, 256))
        .Parameters.Append .CreateParameter("@NurtureText", adVarChar, adParamInput, 256, Left(strNurtureText, 256))
        .Parameters.Append .CreateParameter("@GrievingText", adVarChar, adParamInput, 256, Left(strGrievingText, 256))
        .Parameters.Append .CreateParameter("@SexualText", adVarChar, adParamInput, 256, Left(strSexualText, 256))
        .Parameters.Append .CreateParameter("@PlayOtherText", adVarChar, adParamInput, 256, Left(strPlayOtherText, 256))
        .Parameters.Append .CreateParameter("@OverallSad", adInteger, adParamInput, , IIf(lngOverallSad < 0, Null, lngOverallSad))
        .Parameters.Append .CreateParameter("@OverallAnxious", adInteger, adParamInput, , IIf(lngOverallAnxious < 0, Null, lngOverallAnxious))
        .Parameters.Append .CreateParameter("@OverallFrustrated", adInteger, adParamInput, , IIf(lngOverallFrustrated < 0, Null, lngOverallFrustrated))
        .Parameters.Append .CreateParameter("@OverallDependence", adInteger, adParamInput, , IIf(lngOverallDependence < 0, Null, lngOverallDependence))
        .Parameters.Append .CreateParameter("@OverallMaturity", adInteger, adParamInput, , IIf(lngOverallMaturity < 0, Null, lngOverallMaturity))
        .Parameters.Append .CreateParameter("@OverallFocus", adInteger, adParamInput, , IIf(lngOverallFocus < 0, Null, lngOverallFocus))
        .Parameters.Append .CreateParameter("@OverallImpulsive", adInteger, adParamInput, , IIf(lngOverallImpulsive < 0, Null, lngOverallImpulsive))
        .Parameters.Append .CreateParameter("@OverallSpontaneity", adInteger, adParamInput, , IIf(lngOverallSpontaneity < 0, Null, lngOverallSpontaneity))
        .Parameters.Append .CreateParameter("@OverallIsolation", adInteger, adParamInput, , IIf(lngOverallIsolation < 0, Null, lngOverallIsolation))
        .Parameters.Append .CreateParameter("@OverallDetachment", adInteger, adParamInput, , IIf(lngOverallDetachment < 0, Null, lngOverallDetachment))
        .Parameters.Append .CreateParameter("@ProgressText", adLongVarChar, adParamInput, IIf(Len(strProgressText) = 0, 1, Len(strProgressText)))
        .Parameters("@ProgressText").AppendChunk strProgressText
        .Parameters.Append .CreateParameter("@PlanText", adLongVarChar, adParamInput, IIf(Len(strPlanText) = 0, 1, Len(strPlanText)))
        .Parameters("@PlanText").AppendChunk strPlanText
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
Public Sub Update(ByVal lngPlayNoteID As Long, ByVal lngPatientID As Long, ByVal lngProviderID As Long, _
                       ByVal lngEncounterLogID As Long, ByVal lngClinicID As Long, _
                       ByVal lngSupervisorID As Long, ByVal dtNoteDate As Date, _
                       ByVal lngDiagID1 As Long, ByVal dtApptStartDate As Date, _
                       ByVal dtApptEndDate As Date, ByVal strHappy As String, _
                       ByVal strSad As String, ByVal strAngry As String, ByVal strAfraid As String, ByVal strConfident As String, _
                       ByVal strConfused As String, ByVal strCurious As String, ByVal strIntensity As String, ByVal strToys As String, _
                       ByVal strPlayText As String, ByVal strVerbalText As String, ByVal strChildText As String, ByVal strTherapistText As String, _
                       ByVal strRoomText As String, ByVal strStructureText As String, ByVal strRealityText As String, ByVal strBehaviorText As String, _
                       ByVal lngActivity As Long, ByVal lngPlay As Long, ByVal lngContact As Long, ByVal strExploreText As String, ByVal strRelationText As String, _
                       ByVal strControlText As String, ByVal strHelplessText As String, ByVal strAgressiveText As String, ByVal strSafetyText As String, _
                       ByVal strMasterText As String, ByVal strNurtureText As String, ByVal strGrievingText As String, ByVal strSexualText As String, _
                       ByVal strPlayOtherText As String, ByVal lngOverallSad As Long, ByVal lngOverallAnxious As Long, ByVal lngOverallFrustrated As Long, _
                       ByVal lngOverallDependence As Long, ByVal lngOverallMaturity As Long, ByVal lngOverallFocus As Long, ByVal lngOverallImpulsive As Long, _
                       ByVal lngOverallSpontaneity As Long, ByVal lngOverallIsolation As Long, ByVal lngOverallDetachment As Long, ByVal strProgressText As String, _
                       ByVal strPlanText As String, ByVal strNoteText As String, ByVal strMipsText As String, ByVal blnLockedYN As Boolean, _
                       ByVal lngSignedBy As Long, ByVal dtDateSigned As Date, _
                       ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description: Updates selected records in the tblNotePlay table.  Calls
'               stored procedure uspUpdPatientPlayNote.
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'              lngPlayNoteID lngPatientID lngApptID lngTemplateID lngProviderID strNoteText blnLockedYN
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
        .CommandText = "uspUpdPatientNotePlay"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@PlayNoteID", adInteger, adParamInput, , lngPlayNoteID)
        .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID)
        .Parameters.Append .CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID)
        .Parameters.Append .CreateParameter("@EncLogID", adInteger, adParamInput, , IIf(lngEncounterLogID <= 0, Null, lngEncounterLogID))
        .Parameters.Append .CreateParameter("@ClinicID", adInteger, adParamInput, , IIf(lngClinicID <= 0, Null, lngClinicID))
        .Parameters.Append .CreateParameter("@SupervisorID", adInteger, adParamInput, , IIf(lngSupervisorID <= 0, Null, lngSupervisorID))
        .Parameters.Append .CreateParameter("@NoteDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtNoteDate) = 0, Now(), dtNoteDate))
        .Parameters.Append .CreateParameter("@DiagID1", adInteger, adParamInput, , IIf(lngDiagID1 <= 0, Null, lngDiagID1))
        .Parameters.Append .CreateParameter("@ApptStartDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtApptStartDate) = 0, Null, dtApptStartDate))
        .Parameters.Append .CreateParameter("@ApptEndDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtApptEndDate) = 0, Null, dtApptEndDate))
        .Parameters.Append .CreateParameter("@Happy", adVarChar, adParamInput, 20, Left(strHappy, 20))
        .Parameters.Append .CreateParameter("@Sad", adVarChar, adParamInput, 20, Left(strSad, 20))
        .Parameters.Append .CreateParameter("@Angry", adVarChar, adParamInput, 20, Left(strAngry, 20))
        .Parameters.Append .CreateParameter("@Afraid", adVarChar, adParamInput, 20, Left(strAfraid, 20))
        .Parameters.Append .CreateParameter("@Confident", adVarChar, adParamInput, 10, Left(strConfident, 10))
        .Parameters.Append .CreateParameter("@Confused", adVarChar, adParamInput, 20, Left(strConfused, 20))
        .Parameters.Append .CreateParameter("@Curious", adVarChar, adParamInput, 10, Left(strCurious, 10))
        .Parameters.Append .CreateParameter("@Intensity", adVarChar, adParamInput, 20, Left(strIntensity, 20))
        .Parameters.Append .CreateParameter("@Toys", adVarChar, adParamInput, 40, Left(strToys, 40))
        .Parameters.Append .CreateParameter("@PlayText", adLongVarChar, adParamInput, IIf(Len(strPlayText) = 0, 1, Len(strPlayText)))
        .Parameters("@PlayText").AppendChunk strPlayText
        .Parameters.Append .CreateParameter("@VerbalText", adLongVarChar, adParamInput, IIf(Len(strVerbalText) = 0, 1, Len(strVerbalText)))
        .Parameters("@VerbalText").AppendChunk strVerbalText
        .Parameters.Append .CreateParameter("@ChildText", adVarChar, adParamInput, 256, Left(strChildText, 256))
        .Parameters.Append .CreateParameter("@TherapistText", adVarChar, adParamInput, 256, Left(strTherapistText, 256))
        .Parameters.Append .CreateParameter("@RoomText", adVarChar, adParamInput, 256, Left(strRoomText, 256))
        .Parameters.Append .CreateParameter("@StructureText", adVarChar, adParamInput, 256, Left(strStructureText, 256))
        .Parameters.Append .CreateParameter("@RealityText", adVarChar, adParamInput, 256, Left(strRealityText, 256))
        .Parameters.Append .CreateParameter("@BehaviorText", adVarChar, adParamInput, 256, Left(strBehaviorText, 256))
        .Parameters.Append .CreateParameter("@Activity", adInteger, adParamInput, , IIf(lngActivity < 0, Null, lngActivity))
        .Parameters.Append .CreateParameter("@Play", adInteger, adParamInput, , IIf(lngPlay < 0, Null, lngPlay))
        .Parameters.Append .CreateParameter("@Contact", adInteger, adParamInput, , IIf(lngContact < 0, Null, lngContact))
        .Parameters.Append .CreateParameter("@ExploreText", adVarChar, adParamInput, 256, Left(strExploreText, 256))
        .Parameters.Append .CreateParameter("@RelationText", adVarChar, adParamInput, 256, Left(strRelationText, 256))
        .Parameters.Append .CreateParameter("@ControlText", adVarChar, adParamInput, 256, Left(strControlText, 256))
        .Parameters.Append .CreateParameter("@HelplessText", adVarChar, adParamInput, 256, Left(strHelplessText, 256))
        .Parameters.Append .CreateParameter("@AgressiveText", adVarChar, adParamInput, 256, Left(strAgressiveText, 256))
        .Parameters.Append .CreateParameter("@SafetyText", adVarChar, adParamInput, 256, Left(strSafetyText, 256))
        .Parameters.Append .CreateParameter("@MasterText", adVarChar, adParamInput, 256, Left(strMasterText, 256))
        .Parameters.Append .CreateParameter("@NurtureText", adVarChar, adParamInput, 256, Left(strNurtureText, 256))
        .Parameters.Append .CreateParameter("@GrievingText", adVarChar, adParamInput, 256, Left(strGrievingText, 256))
        .Parameters.Append .CreateParameter("@SexualText", adVarChar, adParamInput, 256, Left(strSexualText, 256))
        .Parameters.Append .CreateParameter("@PlayOtherText", adVarChar, adParamInput, 256, Left(strPlayOtherText, 256))
        .Parameters.Append .CreateParameter("@OverallSad", adInteger, adParamInput, , IIf(lngOverallSad < 0, Null, lngOverallSad))
        .Parameters.Append .CreateParameter("@OverallAnxious", adInteger, adParamInput, , IIf(lngOverallAnxious < 0, Null, lngOverallAnxious))
        .Parameters.Append .CreateParameter("@OverallFrustrated", adInteger, adParamInput, , IIf(lngOverallFrustrated < 0, Null, lngOverallFrustrated))
        .Parameters.Append .CreateParameter("@OverallDependence", adInteger, adParamInput, , IIf(lngOverallDependence < 0, Null, lngOverallDependence))
        .Parameters.Append .CreateParameter("@OverallMaturity", adInteger, adParamInput, , IIf(lngOverallMaturity < 0, Null, lngOverallMaturity))
        .Parameters.Append .CreateParameter("@OverallFocus", adInteger, adParamInput, , IIf(lngOverallFocus < 0, Null, lngOverallFocus))
        .Parameters.Append .CreateParameter("@OverallImpulsive", adInteger, adParamInput, , IIf(lngOverallImpulsive < 0, Null, lngOverallImpulsive))
        .Parameters.Append .CreateParameter("@OverallSpontaneity", adInteger, adParamInput, , IIf(lngOverallSpontaneity < 0, Null, lngOverallSpontaneity))
        .Parameters.Append .CreateParameter("@OverallIsolation", adInteger, adParamInput, , IIf(lngOverallIsolation < 0, Null, lngOverallIsolation))
        .Parameters.Append .CreateParameter("@OverallDetachment", adInteger, adParamInput, , IIf(lngOverallDetachment < 0, Null, lngOverallDetachment))
        .Parameters.Append .CreateParameter("@ProgressText", adLongVarChar, adParamInput, IIf(Len(strProgressText) = 0, 1, Len(strProgressText)))
        .Parameters("@ProgressText").AppendChunk strProgressText
        .Parameters.Append .CreateParameter("@PlanText", adLongVarChar, adParamInput, IIf(Len(strPlanText) = 0, 1, Len(strPlanText)))
        .Parameters("@PlanText").AppendChunk strPlanText
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

Public Sub LockNote(ByVal lngPlayNoteID As Long, ByVal lngSignedBy As Long, ByVal dtDateSigned As Date)
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description: Updates a selected record in the tblNotePlay table, setting
'               fldLockedYN to Y.  Cannot unlock record!!!  Calls stored
'               procedure uspUpdLockPatientPlayNote.
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
        .CommandText = "uspUpdLockPatientPlayNote"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@PlayNoteID", adInteger, adParamInput, , lngPlayNoteID)
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
Public Sub UnLockNote(ByVal lngPlayNoteID As Long, ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description: Updates a selected record in the tblNotePlay table, setting
'               fldLockedYN to Y.  Cannot unlock record!!!  Calls stored
'               procedure uspUpdLockPatientPlayNote.
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
        .CommandText = "uspUpdUnLockPatientPlayNote"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@PlayNoteID", adInteger, adParamInput, , lngPlayNoteID)
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
Public Sub Delete(ByVal strUserName As String, ByVal lngPlayNoteID As Long)
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description: Deletes selected record.  Calls
'               stored procedure uspDelPatientPlayNote.
'Parameters:  lngPlayNoteID - ID of the patient.
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
        .CommandText = "uspDelPatientPlayNote"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@PlayNoteID", adInteger, adParamInput, , lngPlayNoteID)
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




