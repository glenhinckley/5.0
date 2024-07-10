VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CIntakeChildDB"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CIntakeChildDB
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Database object designed to implement methods (by calling
'              stored procedures) associated with
'              data affecting the tblIntakeChild table.
'--------------------------------------------------------------------
'  Revisions:
'--------------------------------------------------------------------
Option Explicit

Private Const CLASS_NAME As String = "CIntakeChildDB"
Private Const TABLE_NAME As String = "tblIntakeChild"

Public Function FetchByID(ByVal lngIntakeChildID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description: Retrieves records from the tblIntakeChild table.  Calls
'               stored procedure uspSelIntakeChildByID.
'Parameters: lngIntakeChildID - 'Where' clause of the SQL statement by
'               which records will be fetched.  Matches tblIntakeChild's
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
        .CommandText = "uspSelIntakeChildByID"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@IntakeChildID", adInteger, adParamInput, , lngIntakeChildID)
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
Public Function FetchByPatient(ByVal lngPatientID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description: Retrieves records from the tblIntakeChild table.  Calls
'               stored procedure uspSelIntakeChildByPatient.
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
        .CommandText = "uspSelIntakeChildByPatient"
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

Public Function Insert(ByVal lngPatientID As Long, ByVal lngProviderID As Long, ByVal lngClinicID As Long, ByVal dtNoteDate As Date, _
                       ByVal dtReferralDate As Date, ByVal strReferredBy As String, ByVal blnNewsletterYN As Boolean, ByVal strChiefConcern As String, ByVal strSeverity As String, _
                       ByVal strGrade As String, ByVal strSchool As String, ByVal strAcademic As String, ByVal strBehavior As String, ByVal strMaritalStatus As String, _
                       ByVal strMaritalLength As String, ByVal strPersonAtHome As String, ByVal strFather As String, ByVal strMother As String, ByVal strStepParents As String, _
                       ByVal strSiblings1 As String, ByVal strSiblings2 As String, ByVal strSiblings3 As String, ByVal strSiblings4 As String, ByVal strSiblings5 As String, _
                       ByVal strActivities As String, ByVal strBirthVaginal As String, ByVal strBirthVaginalComp As String, ByVal strPregnancy As String, ByVal strPregnancySubstance As String, _
                       ByVal strSleep As String, ByVal strAnger As String, ByVal strMilestones As String, ByVal strDoctors As String, ByVal strMedicalCare As String, ByVal strMedicalIssues As String, _
                       ByVal strMedications As String, ByVal strMedicalHistory As String, ByVal strHistory As String, ByVal strHistoryOther As String, ByVal strAlcoholHistory As String, _
                       ByVal strNeglectHistory As String, ByVal strBehaviorHistory As String, ByVal strCommunityHistory As String, ByVal strPsychHistory1 As String, ByVal strPsychHistory2 As String, _
                       ByVal strLegalHistory As String, _
                       ByVal strNoteText As String, ByVal blnLockedYN As Boolean, ByVal lngSignedBy As Long, _
                       ByVal dtDateSigned As Date, ByVal strUserName As String) As Long
                       
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description: Inserts records into the tblIntakeChild table.  Calls
'               stored procedure uspInsIntakeChild.
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
        .CommandText = "uspInsPatientIntakeChild"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@IntakeChildID", adInteger, adParamReturnValue)
        .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID)
        .Parameters.Append .CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID)
        .Parameters.Append .CreateParameter("@ClinicID", adInteger, adParamInput, , IIf(lngClinicID <= 0, Null, lngClinicID))
        .Parameters.Append .CreateParameter("@NoteDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtNoteDate) = 0, Now(), dtNoteDate))
        .Parameters.Append .CreateParameter("@ReferralDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtReferralDate) = 0, Null, dtReferralDate))
        .Parameters.Append .CreateParameter("@ReferredBy", adVarChar, adParamInput, 256, Left(strReferredBy, 256))
        .Parameters.Append .CreateParameter("@NewsletterYN", adVarChar, adParamInput, 1, IIf(blnNewsletterYN = False, "N", "Y"))
        .Parameters.Append .CreateParameter("@ChiefConcern", adVarChar, adParamInput, 256, Left(strChiefConcern, 256))
        .Parameters.Append .CreateParameter("@Severity", adVarChar, adParamInput, 256, Left(strSeverity, 4))
        .Parameters.Append .CreateParameter("@Grade", adVarChar, adParamInput, 256, Left(strGrade, 256))
        .Parameters.Append .CreateParameter("@School", adVarChar, adParamInput, 256, Left(strSchool, 256))
        .Parameters.Append .CreateParameter("@Academic", adVarChar, adParamInput, 256, Left(strAcademic, 256))
        .Parameters.Append .CreateParameter("@Behavior", adVarChar, adParamInput, 256, Left(strBehavior, 256))
        .Parameters.Append .CreateParameter("@MaritalStatus", adVarChar, adParamInput, 256, Left(strMaritalStatus, 256))
        .Parameters.Append .CreateParameter("@MaritalLength", adVarChar, adParamInput, 256, Left(strMaritalLength, 256))
        .Parameters.Append .CreateParameter("@PersonAtHome", adVarChar, adParamInput, IIf(Len(strPersonAtHome) = 0, 1, Len(strPersonAtHome)))
        .Parameters("@PersonAtHome").AppendChunk strPersonAtHome
        .Parameters.Append .CreateParameter("@Father", adVarChar, adParamInput, IIf(Len(strFather) = 0, 1, Len(strFather)))
        .Parameters("@Father").AppendChunk strFather
        .Parameters.Append .CreateParameter("@Mother", adVarChar, adParamInput, IIf(Len(strMother) = 0, 1, Len(strMother)))
        .Parameters("@Mother").AppendChunk strMother
        .Parameters.Append .CreateParameter("@StepParents", adVarChar, adParamInput, IIf(Len(strStepParents) = 0, 1, Len(strStepParents)))
        .Parameters("@StepParents").AppendChunk strStepParents
        .Parameters.Append .CreateParameter("@Siblings1", adVarChar, adParamInput, 256, Left(strSiblings1, 256))
        .Parameters.Append .CreateParameter("@Siblings2", adVarChar, adParamInput, 256, Left(strSiblings2, 256))
        .Parameters.Append .CreateParameter("@Siblings3", adVarChar, adParamInput, 256, Left(strSiblings3, 256))
        .Parameters.Append .CreateParameter("@Siblings4", adVarChar, adParamInput, 256, Left(strSiblings4, 256))
        .Parameters.Append .CreateParameter("@Siblings5", adVarChar, adParamInput, 256, Left(strSiblings5, 256))
        .Parameters.Append .CreateParameter("@Activities", adVarChar, adParamInput, IIf(Len(strActivities) = 0, 1, Len(strActivities)))
        .Parameters("@Activities").AppendChunk strActivities
        .Parameters.Append .CreateParameter("@BirthVaginal", adVarChar, adParamInput, 256, Left(strBirthVaginal, 256))
        .Parameters.Append .CreateParameter("@BirthVaginalComp", adVarChar, adParamInput, 256, Left(strBirthVaginalComp, 256))
        .Parameters.Append .CreateParameter("@Pregnancy", adVarChar, adParamInput, 256, Left(strPregnancy, 256))
        .Parameters.Append .CreateParameter("@PregnancySubstance", adVarChar, adParamInput, 256, Left(strPregnancySubstance, 256))
        .Parameters.Append .CreateParameter("@Sleep", adVarChar, adParamInput, 256, Left(strSleep, 256))
        .Parameters.Append .CreateParameter("@Anger", adVarChar, adParamInput, 256, Left(strAnger, 256))
        .Parameters.Append .CreateParameter("@Milestones", adVarChar, adParamInput, 256, Left(strMilestones, 256))
        .Parameters.Append .CreateParameter("@Doctors", adVarChar, adParamInput, 256, Left(strDoctors, 256))
        .Parameters.Append .CreateParameter("@MedicalCare", adVarChar, adParamInput, 256, Left(strMedicalCare, 256))
        .Parameters.Append .CreateParameter("@MedicalIssues", adVarChar, adParamInput, 256, Left(strMedicalIssues, 256))
        .Parameters.Append .CreateParameter("@Medications", adVarChar, adParamInput, 256, Left(strMedications, 256))
        .Parameters.Append .CreateParameter("@MedicalHistory", adVarChar, adParamInput, 256, Left(strMedicalHistory, 256))
        .Parameters.Append .CreateParameter("@History", adVarChar, adParamInput, 256, Left(strHistory, 128))
        .Parameters.Append .CreateParameter("@HistoryOther", adVarChar, adParamInput, 256, Left(strHistoryOther, 256))
        .Parameters.Append .CreateParameter("@AlcoholHistory", adVarChar, adParamInput, IIf(Len(strAlcoholHistory) = 0, 1, Len(strAlcoholHistory)))
        .Parameters("@AlcoholHistory").AppendChunk strAlcoholHistory
        .Parameters.Append .CreateParameter("@NeglectHistory", adVarChar, adParamInput, IIf(Len(strNeglectHistory) = 0, 1, Len(strNeglectHistory)))
        .Parameters("@NeglectHistory").AppendChunk strNeglectHistory
        .Parameters.Append .CreateParameter("@BehaviorHistory", adVarChar, adParamInput, IIf(Len(strBehaviorHistory) = 0, 1, Len(strBehaviorHistory)))
        .Parameters("@BehaviorHistory").AppendChunk strBehaviorHistory
        .Parameters.Append .CreateParameter("@CommunityHistory", adVarChar, adParamInput, IIf(Len(strCommunityHistory) = 0, 1, Len(strCommunityHistory)))
        .Parameters("@CommunityHistory").AppendChunk strCommunityHistory
        .Parameters.Append .CreateParameter("@PsychHistory1", adVarChar, adParamInput, IIf(Len(strPsychHistory1) = 0, 1, Len(strPsychHistory1)))
        .Parameters("@PsychHistory1").AppendChunk strPsychHistory1
        .Parameters.Append .CreateParameter("@PsychHistory2", adVarChar, adParamInput, IIf(Len(strPsychHistory2) = 0, 1, Len(strPsychHistory2)))
        .Parameters("@PsychHistory2").AppendChunk strPsychHistory2
        .Parameters.Append .CreateParameter("@LegalHistory", adVarChar, adParamInput, IIf(Len(strLegalHistory) = 0, 1, Len(strLegalHistory)))
        .Parameters("@LegalHistory").AppendChunk strLegalHistory
        .Parameters.Append .CreateParameter("@NoteText", adLongVarChar, adParamInput, IIf(Len(strNoteText) = 0, 1, Len(strNoteText)))
        .Parameters("@NoteText").AppendChunk strNoteText
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
  '  Set cnn = New ADODB.Connection  ' Instantiate DB connection
  '  cnn.Open (CONST_PSYQUEL_CNN) ' open DB connection
  '  Set cmd.ActiveConnection = cnn
  
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
Public Sub Update(ByVal lngIntakeChildID As Long, ByVal lngPatientID As Long, ByVal lngProviderID As Long, ByVal lngClinicID As Long, ByVal dtNoteDate As Date, _
                       ByVal dtReferralDate As Date, ByVal strReferredBy As String, ByVal blnNewsletterYN As Boolean, ByVal strChiefConcern As String, ByVal strSeverity As String, _
                       ByVal strGrade As String, ByVal strSchool As String, ByVal strAcademic As String, ByVal strBehavior As String, ByVal strMaritalStatus As String, _
                       ByVal strMaritalLength As String, ByVal strPersonAtHome As String, ByVal strFather As String, ByVal strMother As String, ByVal strStepParents As String, _
                       ByVal strSiblings1 As String, ByVal strSiblings2 As String, ByVal strSiblings3 As String, ByVal strSiblings4 As String, ByVal strSiblings5 As String, _
                       ByVal strActivities As String, ByVal strBirthVaginal As String, ByVal strBirthVaginalComp As String, ByVal strPregnancy As String, ByVal strPregnancySubstance As String, _
                       ByVal strSleep As String, ByVal strAnger As String, ByVal strMilestones As String, ByVal strDoctors As String, ByVal strMedicalCare As String, ByVal strMedicalIssues As String, _
                       ByVal strMedications As String, ByVal strMedicalHistory As String, ByVal strHistory As String, ByVal strHistoryOther As String, ByVal strAlcoholHistory As String, _
                       ByVal strNeglectHistory As String, ByVal strBehaviorHistory As String, ByVal strCommunityHistory As String, ByVal strPsychHistory1 As String, ByVal strPsychHistory2 As String, _
                       ByVal strLegalHistory As String, _
                       ByVal strNoteText As String, ByVal blnLockedYN As Boolean, ByVal lngSignedBy As Long, _
                       ByVal dtDateSigned As Date, ByVal strUserName As String)
                       
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description: Updates selected records in the tblIntakeChild table.  Calls
'               stored procedure uspUpdIntakeChild.
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

    Set cmd = New ADODB.Command     ' Instantiate SQL statement
    
    'Create the parameter objects
    With cmd
        .CommandText = "uspUpdPatientIntakeChild"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@IntakeChildID", adInteger, adParamInput, , lngIntakeChildID)
        .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID)
        .Parameters.Append .CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID)
        .Parameters.Append .CreateParameter("@ClinicID", adInteger, adParamInput, , IIf(lngClinicID <= 0, Null, lngClinicID))
        .Parameters.Append .CreateParameter("@NoteDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtNoteDate) = 0, Now(), dtNoteDate))
        .Parameters.Append .CreateParameter("@ReferralDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtReferralDate) = 0, Null, dtReferralDate))
        .Parameters.Append .CreateParameter("@ReferredBy", adVarChar, adParamInput, 256, Left(strReferredBy, 256))
        .Parameters.Append .CreateParameter("@NewsletterYN", adVarChar, adParamInput, 1, IIf(blnNewsletterYN = False, "N", "Y"))
        .Parameters.Append .CreateParameter("@ChiefConcern", adVarChar, adParamInput, 256, Left(strChiefConcern, 256))
        .Parameters.Append .CreateParameter("@Severity", adVarChar, adParamInput, 256, Left(strSeverity, 4))
        .Parameters.Append .CreateParameter("@Grade", adVarChar, adParamInput, 256, Left(strGrade, 256))
        .Parameters.Append .CreateParameter("@School", adVarChar, adParamInput, 256, Left(strSchool, 256))
        .Parameters.Append .CreateParameter("@Academic", adVarChar, adParamInput, 256, Left(strAcademic, 256))
        .Parameters.Append .CreateParameter("@Behavior", adVarChar, adParamInput, 256, Left(strBehavior, 256))
        .Parameters.Append .CreateParameter("@MaritalStatus", adVarChar, adParamInput, 256, Left(strMaritalStatus, 256))
        .Parameters.Append .CreateParameter("@MaritalLength", adVarChar, adParamInput, 256, Left(strMaritalLength, 256))
        .Parameters.Append .CreateParameter("@PersonAtHome", adVarChar, adParamInput, IIf(Len(strPersonAtHome) = 0, 1, Len(strPersonAtHome)))
        .Parameters("@PersonAtHome").AppendChunk strPersonAtHome
        .Parameters.Append .CreateParameter("@Father", adVarChar, adParamInput, IIf(Len(strFather) = 0, 1, Len(strFather)))
        .Parameters("@Father").AppendChunk strFather
        .Parameters.Append .CreateParameter("@Mother", adVarChar, adParamInput, IIf(Len(strMother) = 0, 1, Len(strMother)))
        .Parameters("@Mother").AppendChunk strMother
        .Parameters.Append .CreateParameter("@StepParents", adVarChar, adParamInput, IIf(Len(strStepParents) = 0, 1, Len(strStepParents)))
        .Parameters("@StepParents").AppendChunk strStepParents
        .Parameters.Append .CreateParameter("@Siblings1", adVarChar, adParamInput, 256, Left(strSiblings1, 256))
        .Parameters.Append .CreateParameter("@Siblings2", adVarChar, adParamInput, 256, Left(strSiblings2, 256))
        .Parameters.Append .CreateParameter("@Siblings3", adVarChar, adParamInput, 256, Left(strSiblings3, 256))
        .Parameters.Append .CreateParameter("@Siblings4", adVarChar, adParamInput, 256, Left(strSiblings4, 256))
        .Parameters.Append .CreateParameter("@Siblings5", adVarChar, adParamInput, 256, Left(strSiblings5, 256))
        .Parameters.Append .CreateParameter("@Activities", adVarChar, adParamInput, IIf(Len(strActivities) = 0, 1, Len(strActivities)))
        .Parameters("@Activities").AppendChunk strActivities
        .Parameters.Append .CreateParameter("@BirthVaginal", adVarChar, adParamInput, 256, Left(strBirthVaginal, 256))
        .Parameters.Append .CreateParameter("@BirthVaginalComp", adVarChar, adParamInput, 256, Left(strBirthVaginalComp, 256))
        .Parameters.Append .CreateParameter("@Pregnancy", adVarChar, adParamInput, 256, Left(strPregnancy, 256))
        .Parameters.Append .CreateParameter("@PregnancySubstance", adVarChar, adParamInput, 256, Left(strPregnancySubstance, 256))
        .Parameters.Append .CreateParameter("@Sleep", adVarChar, adParamInput, 256, Left(strSleep, 256))
        .Parameters.Append .CreateParameter("@Anger", adVarChar, adParamInput, 256, Left(strAnger, 256))
        .Parameters.Append .CreateParameter("@Milestones", adVarChar, adParamInput, 256, Left(strMilestones, 256))
        .Parameters.Append .CreateParameter("@Doctors", adVarChar, adParamInput, 256, Left(strDoctors, 256))
        .Parameters.Append .CreateParameter("@MedicalCare", adVarChar, adParamInput, 256, Left(strMedicalCare, 256))
        .Parameters.Append .CreateParameter("@MedicalIssues", adVarChar, adParamInput, 256, Left(strMedicalIssues, 256))
        .Parameters.Append .CreateParameter("@Medications", adVarChar, adParamInput, 256, Left(strMedications, 256))
        .Parameters.Append .CreateParameter("@MedicalHistory", adVarChar, adParamInput, 256, Left(strMedicalHistory, 256))
        .Parameters.Append .CreateParameter("@History", adVarChar, adParamInput, 256, Left(strHistory, 128))
        .Parameters.Append .CreateParameter("@HistoryOther", adVarChar, adParamInput, 256, Left(strHistoryOther, 256))
        .Parameters.Append .CreateParameter("@AlcoholHistory", adVarChar, adParamInput, IIf(Len(strAlcoholHistory) = 0, 1, Len(strAlcoholHistory)))
        .Parameters("@AlcoholHistory").AppendChunk strAlcoholHistory
        .Parameters.Append .CreateParameter("@NeglectHistory", adVarChar, adParamInput, IIf(Len(strNeglectHistory) = 0, 1, Len(strNeglectHistory)))
        .Parameters("@NeglectHistory").AppendChunk strNeglectHistory
        .Parameters.Append .CreateParameter("@BehaviorHistory", adVarChar, adParamInput, IIf(Len(strBehaviorHistory) = 0, 1, Len(strBehaviorHistory)))
        .Parameters("@BehaviorHistory").AppendChunk strBehaviorHistory
        .Parameters.Append .CreateParameter("@CommunityHistory", adVarChar, adParamInput, IIf(Len(strCommunityHistory) = 0, 1, Len(strCommunityHistory)))
        .Parameters("@CommunityHistory").AppendChunk strCommunityHistory
        .Parameters.Append .CreateParameter("@PsychHistory1", adVarChar, adParamInput, IIf(Len(strPsychHistory1) = 0, 1, Len(strPsychHistory1)))
        .Parameters("@PsychHistory1").AppendChunk strPsychHistory1
        .Parameters.Append .CreateParameter("@PsychHistory2", adVarChar, adParamInput, IIf(Len(strPsychHistory2) = 0, 1, Len(strPsychHistory2)))
        .Parameters("@PsychHistory2").AppendChunk strPsychHistory2
        .Parameters.Append .CreateParameter("@LegalHistory", adVarChar, adParamInput, IIf(Len(strLegalHistory) = 0, 1, Len(strLegalHistory)))
        .Parameters("@LegalHistory").AppendChunk strLegalHistory
        .Parameters.Append .CreateParameter("@NoteText", adLongVarChar, adParamInput, IIf(Len(strNoteText) = 0, 1, Len(strNoteText)))
        .Parameters("@NoteText").AppendChunk strNoteText
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

Public Sub LockNote(ByVal lngIntakeChildID As Long, ByVal lngSignedBy As Long, ByVal lngSignedByRP As Long, ByVal dtDateSigned As Date)
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description: Updates a selected record in the tblIntakeChild table, setting
'               fldLockedYN to Y.  Cannot unlock record!!!  Calls stored
'               procedure uspUpdLockIntakeChild.
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
        .CommandText = "uspUpdLockIntakeChild"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@IntakeChildID", adInteger, adParamInput, , lngIntakeChildID)
        .Parameters.Append .CreateParameter("@SignedBy", adInteger, adParamInput, , IIf(lngSignedBy <= 0, Null, lngSignedBy))
        .Parameters.Append .CreateParameter("@SignedByRP", adInteger, adParamInput, , IIf(lngSignedByRP <= 0, Null, lngSignedByRP))
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
Public Sub UnLockNote(ByVal lngIntakeChildID As Long, ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description: Updates a selected record in the tblIntakeChild table, setting
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
        .CommandText = "uspUpdUnLockIntakeChild"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@IntakeChildID", adInteger, adParamInput, , lngIntakeChildID)
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

Public Sub Delete(ByVal strUserName As String, ByVal lngIntakeChildID As Long)
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description: Deletes selected record.  Calls
'               stored procedure uspDelIntakeChild.
'Parameters:  lngIntakeChildID - ID of the patient.
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
        .CommandText = "uspDelIntakeChild"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@IntakeChildID", adInteger, adParamInput, , lngIntakeChildID)
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






