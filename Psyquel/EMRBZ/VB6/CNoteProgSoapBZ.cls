VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CNoteProgSoapBZ"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CNoteProgSoapBZ
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  MTS business object designed to call methods
'              associated with the CNoteProgSoapDB class.
'--------------------------------------------------------------------
Option Explicit

Private Const CLASS_NAME As String = "CNoteProgSoapBZ"
Private Const TABLE_NAME As String = "tblNoteProgSoap"
Public Function FetchByID(ByVal lngProgSoapNoteID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Retrieves records from the tblNoteProgSoap table.
'Parameters: lngProgSoapNoteID - ID number of the record to retrieve, which
'               is primary key of table.
'            lngUserID - UserID of user calling method.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    ' Declares database onject
    Dim objProgSoapNote As EMRDB.CNoteProgSoapDB
    ' Declares recordset for result returned
    Dim rstSQL As ADODB.Recordset
    
    On Error GoTo ErrTrap:

    ' Instantiates database onject
    Set objProgSoapNote = CreateObject("EMRDB.CNoteProgSoapDB")
    
    ' Instantiates recordset for result returned
    Set rstSQL = objProgSoapNote.FetchByID(lngProgSoapNoteID)
    
    ' Return value set for function
    Set FetchByID = rstSQL
    
    'Signal successful completion
    GetObjectContext.SetComplete
    ' Set DB object to NULL
    Set objProgSoapNote = Nothing
    ' Exit on success
    Exit Function
    
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objProgSoapNote = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Public Function FetchByPatient(ByVal lngUserID As Long, ByVal lngPatientID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Retrieves records from the tblNoteProgSoap table.
'Parameters: lngPatientID - ID number of the record to retrieve based on the alertID.
'            lngUserID - UserID of user calling method.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------
    
    Dim objProgSoapNote As EMRDB.CNoteProgSoapDB
    Dim rstSQL As ADODB.Recordset

    On Error GoTo ErrTrap:

    Set objProgSoapNote = CreateObject("EMRDB.CNoteProgSoapDB")
    
    Set rstSQL = objProgSoapNote.FetchByPatient(lngUserID, lngPatientID)
    
    Set FetchByPatient = rstSQL
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objProgSoapNote = Nothing
    Exit Function
    
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objProgSoapNote = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Public Function FetchByEncounter(ByVal lngUserID As Long, ByVal lngEncLogID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Retrieves records from the tblNoteProgSoap table by EncouterLogID.
'Parameters: lngEncLogID - ID number of the encounter log that the user desires.
'            lngUserID - UserID of user calling method.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------

    Dim objProgSoapNote As EMRDB.CNoteProgSoapDB
    Dim rstSQL As ADODB.Recordset

    On Error GoTo ErrTrap:

    Set objProgSoapNote = CreateObject("EMRDB.CNoteProgSoapDB")
    
    Set rstSQL = objProgSoapNote.FetchByEncounter(lngUserID, lngEncLogID)
    
    Set FetchByEncounter = rstSQL
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objProgSoapNote = Nothing
    Exit Function
    
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objProgSoapNote = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Public Function FetchNextNote(ByVal lngUserID As Long, ByVal lngProgSoapNoteID As Long, _
                              ByVal lngPatientID As Long, Optional ByVal lngEncounterLogID As Long = 0) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 10/06/2017
'Author: Duane C Orth
'Description:  Retrieves next record from the tblNoteProgSoap table by EncouterLogID.
'Parameters: lngProgSoapNoteID - ID number of the patient note that the user desires.
'            lngPatientID - ID of the patient associated with the Patient Note
'            lngUserID - UserID of user calling method.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------

    Dim objProgSoapNote As EMRDB.CNoteProgSoapDB
    Dim rstSQL As ADODB.Recordset

    On Error GoTo ErrTrap:

    Set objProgSoapNote = CreateObject("EMRDB.CNoteProgSoapDB")
    
    Set rstSQL = objProgSoapNote.FetchNextNote(lngUserID, lngProgSoapNoteID, lngPatientID, lngEncounterLogID)
    
    Set FetchNextNote = rstSQL
    
    GetObjectContext.SetComplete
    Set objProgSoapNote = Nothing
    Exit Function
    
ErrTrap:
    Set objProgSoapNote = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Public Function FetchPrevNote(ByVal lngUserID As Long, ByVal lngProgSoapNoteID As Long, _
                              ByVal lngPatientID As Long, Optional ByVal lngEncounterLogID As Long = 0) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 10/06/2017
'Author: Duane C Orth
'Description:  Retrieves previous record from the tblNoteProgSoap table by EncouterLogID.
'Parameters: lngProgSoapNoteID - ID number of the patient note that the user desires.
'            lngPatientID - ID of the patient associated with the Patient Note
'            lngUserID - UserID of user calling method.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------

    Dim objProgSoapNote As EMRDB.CNoteProgSoapDB
    Dim rstSQL As ADODB.Recordset

    On Error GoTo ErrTrap:

    Set objProgSoapNote = CreateObject("EMRDB.CNoteProgSoapDB")
    
    Set rstSQL = objProgSoapNote.FetchPrevNote(lngUserID, lngProgSoapNoteID, lngPatientID, lngEncounterLogID)
    
    Set FetchPrevNote = rstSQL
    
    GetObjectContext.SetComplete
    Set objProgSoapNote = Nothing
    Exit Function
    
ErrTrap:
    Set objProgSoapNote = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Public Function Insert(ByVal lngPatientID As Long, ByVal lngProviderID As Long, _
                       ByVal lngEncounterLogID As Long, ByVal lngClinicID As Long, _
                       ByVal lngSupervisorID As Long, ByVal dtNoteDate As Date, _
                       ByVal lngDiagID1 As Long, ByVal dtApptStartDate As Date, _
                       ByVal dtApptEndDate As Date, ByVal strPlanText As String, _
                       ByVal strPurpose As String, ByVal strClientText As String, _
                       ByVal strSiHi As String, ByVal strSiHiText As String, _
                       ByVal strTherapistText As String, ByVal strDiagChange As String, _
                       ByVal strDiagChangeText As String, ByVal strIntervention As String, _
                       ByVal strAppearance As String, ByVal strSpeech As String, _
                       ByVal strMotor As String, ByVal strHygiene As String, _
                       ByVal strThoughts As String, ByVal strAffect As String, _
                       ByVal strCognitive As String, ByVal strMedicationText As String, _
                       ByVal strPatientText As String, ByVal strRecommend As String, _
                       ByVal strCommentText As String, ByVal strPhysicalText As String, ByVal strLabsText As String, _
                       ByVal strNoteText As String, ByVal strMipsText As String, ByVal blnLockedYN As Boolean, _
                       ByVal lngSignedBy As Long, ByVal dtDateSigned As Date, _
                       ByVal strUserName As String) As Long
                       
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Inserts a row into the tblNoteProgSoap table utilizing
'              a stored procedure.
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'Returns: ID (Primary Key) of the row inserted.
'--------------------------------------------------------------------
    Dim objProgSoapNote As EMRDB.CNoteProgSoapDB
    Dim strErrMessage As String
    
    On Error GoTo ErrTrap:
    
    ' Validate data being passed
  ''  If Not VerifyData(lngTemplateID, strNoteText, strErrMessage, lngEncounterLogID, _
  ''      lngPatientID, , strUserName) Then GoTo ErrTrap
       
    Set objProgSoapNote = CreateObject("EMRDB.CNoteProgSoapDB")
    
    Insert = objProgSoapNote.Insert(lngPatientID, lngProviderID, lngEncounterLogID, lngClinicID, lngSupervisorID, dtNoteDate, _
                       lngDiagID1, dtApptStartDate, dtApptEndDate, strPlanText, strPurpose, strClientText, strSiHi, strSiHiText, _
                       strTherapistText, strDiagChange, strDiagChangeText, strIntervention, strAppearance, strSpeech, _
                       strMotor, strHygiene, strThoughts, strAffect, strCognitive, strMedicationText, strPatientText, _
                       strRecommend, strCommentText, strPhysicalText, strLabsText, strNoteText, strMipsText, blnLockedYN, lngSignedBy, dtDateSigned, strUserName)
       
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objProgSoapNote = Nothing
    Exit Function
    
ErrTrap:
    GetObjectContext.SetAbort
    Set objProgSoapNote = Nothing
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Function
Public Sub Update(ByVal lngProgSoapNoteID As Long, ByVal lngPatientID As Long, ByVal lngProviderID As Long, _
                       ByVal lngEncounterLogID As Long, ByVal lngClinicID As Long, _
                       ByVal lngSupervisorID As Long, ByVal dtNoteDate As Date, _
                       ByVal lngDiagID1 As Long, ByVal dtApptStartDate As Date, _
                       ByVal dtApptEndDate As Date, ByVal strPlanText As String, _
                       ByVal strPurpose As String, ByVal strClientText As String, _
                       ByVal strSiHi As String, ByVal strSiHiText As String, _
                       ByVal strTherapistText As String, ByVal strDiagChange As String, _
                       ByVal strDiagChangeText As String, ByVal strIntervention As String, _
                       ByVal strAppearance As String, ByVal strSpeech As String, _
                       ByVal strMotor As String, ByVal strHygiene As String, _
                       ByVal strThoughts As String, ByVal strAffect As String, _
                       ByVal strCognitive As String, ByVal strMedicationText As String, _
                       ByVal strPatientText As String, ByVal strRecommend As String, _
                       ByVal strCommentText As String, ByVal strPhysicalText As String, ByVal strLabsText As String, _
                       ByVal strNoteText As String, ByVal strMipsText As String, ByVal blnLockedYN As Boolean, _
                       ByVal lngSignedBy As Long, ByVal dtDateSigned As Date, ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Updates a row in the tblNoteProgSoap table utilizing
'              a stored procedure.
'Parameters: Each parameter identifies the column value that will be
'              updated, except for lngUserID, which is for user
'              verification and for the fetch method, but is not passed
'              to EMRDB.CNoteProgSoapDB.Update.
'Returns: Null
'--------------------------------------------------------------------
    
    ' Declare database object
    Dim objProgSoapNote As EMRDB.CNoteProgSoapDB
    
    ' Declare recordset for fetch note
    Dim rstProgSoapNote As ADODB.Recordset
    
    ' Declare error message string
    Dim strErrMessage As String

    On Error GoTo ErrTrap:

    ' Verify that ID number is valid; must be >= 0 (1st index==1)
    ' -- if PatientNoteID was passed, that is.
    If lngProgSoapNoteID < 0 Then
        strErrMessage = "PatientNoteID number provided is not valid."
        GoTo ErrTrap
    End If

    ' Instantiate database object
    Set objProgSoapNote = CreateObject("EMRDB.CNoteProgSoapDB")
    
    ' Get the appropriate record
    Set rstProgSoapNote = objProgSoapNote.FetchByID(lngProgSoapNoteID)
    
    ' Is the note locked?  If so, error, don't update.
    If UCase(rstProgSoapNote.Fields("fldLockedYN")) = "Y" Then
        strErrMessage = "Record is locked and cannot be updated."
        GoTo ErrTrap
    End If
    
    Set rstProgSoapNote = Nothing
    
    ' Run database object
    objProgSoapNote.Update lngProgSoapNoteID, lngPatientID, lngProviderID, lngEncounterLogID, lngClinicID, lngSupervisorID, dtNoteDate, _
                       lngDiagID1, dtApptStartDate, dtApptEndDate, strPlanText, strPurpose, strClientText, strSiHi, strSiHiText, _
                       strTherapistText, strDiagChange, strDiagChangeText, strIntervention, strAppearance, strSpeech, _
                       strMotor, strHygiene, strThoughts, strAffect, strCognitive, strMedicationText, strPatientText, _
                       strRecommend, strCommentText, strPhysicalText, strLabsText, strNoteText, strMipsText, blnLockedYN, lngSignedBy, dtDateSigned, strUserName
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objProgSoapNote = Nothing
    Exit Sub

ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    
    Set objProgSoapNote = Nothing
    
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Sub
Public Sub LockNote(ByVal lngProgSoapNoteID As Long, ByVal lngSignedBy As Long, ByVal dtDateSigned As Date)
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Flags a row in the tblNoteProgSoap table marking the row as
'              deleted or undeleted.
'Parameters: lngProgSoapNoteID - ID of the row in the table whose value will be
'               updated.
'Returns: Null
'--------------------------------------------------------------------

    ' Declare database object
    Dim objProgSoapNote As EMRDB.CNoteProgSoapDB
    
    ' Declare error message string
    Dim strErrMessage As String

    On Error GoTo ErrTrap:

    ' Instantiate database object
    Set objProgSoapNote = CreateObject("EMRDB.CNoteProgSoapDB")
    
    ' Run database object
    objProgSoapNote.LockNote lngProgSoapNoteID, lngSignedBy, dtDateSigned
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objProgSoapNote = Nothing
    Exit Sub

ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    
    Set objProgSoapNote = Nothing
    
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Sub
Public Sub UnLockNote(ByVal lngProgSoapNoteID As Long, ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Flags a row in the tblNoteProgSoap table marking the row as
'              deleted or undeleted.
'Parameters: lngProgSoapNoteID - ID of the row in the table whose value will be
'               updated.
'Returns: Null
'--------------------------------------------------------------------

    ' Declare database object
    Dim objProgSoapNote As EMRDB.CNoteProgSoapDB
    
    ' Declare error message string
    Dim strErrMessage As String

    On Error GoTo ErrTrap:

    ' Instantiate database object
    Set objProgSoapNote = CreateObject("EMRDB.CNoteProgSoapDB")
    
    ' Run database object
    objProgSoapNote.UnLockNote lngProgSoapNoteID, strUserName
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objProgSoapNote = Nothing
    Exit Sub

ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    
    Set objProgSoapNote = Nothing
    
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Sub
Public Sub Delete(ByVal strUserName As String, ByVal lngProgSoapNoteID As Long)
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Flags a row in the tblNoteProgSoap table marking the row as
'              deleted or undeleted.
'Parameters: lngProgSoapNoteID - ID of the row in the table whose value will be
'               updated.
'            strUserName - Login name of the user responsible for
'               marking the row as deleted.
'            lngUserID - UserID must be passed to verify LockedYN
'Returns: Null
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    ' Declare DB obj
    Dim objProgSoapNote As EMRDB.CNoteProgSoapDB
    Dim strErrMessage As String
    
    On Error GoTo ErrTrap:

    ' Instanitate DB obj
    Set objProgSoapNote = CreateObject("EMRDB.CNoteProgSoapDB")
    
    'Call on valid check
    If Not (objProgSoapNote.FetchByID(lngProgSoapNoteID).RecordCount = 0) Then
        If ((objProgSoapNote.FetchByID(lngProgSoapNoteID).Fields("fldLockedYN")) = "N") Then
            ' Fire delete method
            Call objProgSoapNote.Delete(strUserName, lngProgSoapNoteID)
        Else
            ' Tell user that record is locked
            strErrMessage = "The record you are trying to delete is locked."
            GoTo ErrTrap
        End If
    Else
        'Tell user that record doesn't exist
        strErrMessage = "The record you are trying to delete does not exist."
        GoTo ErrTrap
    End If
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objProgSoapNote = Nothing
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objProgSoapNote = Nothing
    GetObjectContext.SetAbort
    
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Sub
Private Function VerifyData(ByVal lngTemplateID As Long, ByVal strNoteText As String, _
    ByRef strErrMessage As String, Optional ByVal lngEncounterLogID As Long, _
    Optional ByVal lngPatientID As Long, Optional ByVal lngProgSoapNoteID As Long, _
    Optional ByVal strUserName As String) As Boolean
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Verifies all required data has been provided by the user.
'              Last two params are optional to take into account the
'              different needs for update and insert.  Boolean value doesn't
'              be validated because all bool values are acceptable.
'Parameters:  The values to be checked.
'Returns: Boolean value identifying if all criteria has been satisfied.
'--------------------------------------------------------------------
'

    ' Verify that the TemplateID is valid; must be >= 0 (1st index==1)
    If lngTemplateID < 0 Then
        strErrMessage = "TemplateID number provided is not valid."
        VerifyData = False
        Exit Function
    End If
    
    ' Verify that something was entered into patientNotes text field.
    If Trim(strNoteText) = "" Then
        strErrMessage = "No information was entered for the Patient Note."
        VerifyData = False
        Exit Function
    End If
   
    ' Verify that ID number is valid; must be >= 0 (1st index==1)
    ' -- if PatientNoteID was passed, that is.
    If lngProgSoapNoteID < 0 Then
        strErrMessage = "PatientNoteID number provided is not valid."
        VerifyData = False
        Exit Function
    End If
  
    If lngEncounterLogID < 0 Then
        strErrMessage = "EncounterLogID number provided is not valid."
        VerifyData = False
        Exit Function
    End If
  
    If lngPatientID < 0 Then
        strErrMessage = "PatientID number provided is not valid."
        VerifyData = False
        Exit Function
    End If
  
    ' Check to see if username provided is not null--if it's provided
    ' First check to see if username was passed as a parameter
    If Trim(strNoteText) = "" Then
        strErrMessage = "A user name is required."
        VerifyData = False
        Exit Function
    End If
    
    'If we get here, all is well...
    VerifyData = True

End Function








