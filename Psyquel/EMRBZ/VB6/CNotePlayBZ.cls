VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CNotePlayBZ"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CNotePlayBZ
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  MTS business object designed to call methods
'              associated with the CNotePlayDB class.
'--------------------------------------------------------------------
Option Explicit

Private Const CLASS_NAME As String = "CNotePlayBZ"
Private Const TABLE_NAME As String = "tblNotePlay"
Public Function FetchByID(ByVal lngPlayNoteID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Retrieves records from the tblNotePlay table.
'Parameters: lngPlayNoteID - ID number of the record to retrieve, which
'               is primary key of table.
'            lngUserID - UserID of user calling method.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    ' Declares database onject
    Dim objPlayNote As EMRDB.CNotePlayDB
    ' Declares recordset for result returned
    Dim rstSQL As ADODB.Recordset
    
    On Error GoTo ErrTrap:

    ' Instantiates database onject
    Set objPlayNote = CreateObject("EMRDB.CNotePlayDB")
    
    ' Instantiates recordset for result returned
    Set rstSQL = objPlayNote.FetchByID(lngPlayNoteID)
    
    ' Return value set for function
    Set FetchByID = rstSQL
    
    'Signal successful completion
    GetObjectContext.SetComplete
    ' Set DB object to NULL
    Set objPlayNote = Nothing
    ' Exit on success
    Exit Function
    
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objPlayNote = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Public Function FetchByPatient(ByVal lngUserID As Long, ByVal lngPatientID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Retrieves records from the tblNotePlay table.
'Parameters: lngPatientID - ID number of the record to retrieve based on the alertID.
'            lngUserID - UserID of user calling method.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------
    
    Dim objPlayNote As EMRDB.CNotePlayDB
    Dim rstSQL As ADODB.Recordset

    On Error GoTo ErrTrap:

    Set objPlayNote = CreateObject("EMRDB.CNotePlayDB")
    
    Set rstSQL = objPlayNote.FetchByPatient(lngUserID, lngPatientID)
    
    Set FetchByPatient = rstSQL
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objPlayNote = Nothing
    Exit Function
    
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objPlayNote = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Public Function FetchByEncounter(ByVal lngUserID As Long, ByVal lngEncLogID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Retrieves records from the tblNotePlay table by EncouterLogID.
'Parameters: lngEncLogID - ID number of the encounter log that the user desires.
'            lngUserID - UserID of user calling method.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------

    Dim objPlayNote As EMRDB.CNotePlayDB
    Dim rstSQL As ADODB.Recordset

    On Error GoTo ErrTrap:

    Set objPlayNote = CreateObject("EMRDB.CNotePlayDB")
    
    Set rstSQL = objPlayNote.FetchByEncounter(lngUserID, lngEncLogID)
    
    Set FetchByEncounter = rstSQL
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objPlayNote = Nothing
    Exit Function
    
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objPlayNote = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Public Function FetchNextNote(ByVal lngUserID As Long, ByVal lngPlayNoteID As Long, _
                              ByVal lngPatientID As Long, Optional ByVal lngEncounterLogID As Long = 0) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 10/06/2017
'Author: Duane C Orth
'Description:  Retrieves next record from the tblNotePlay table by EncouterLogID.
'Parameters: lngPlayNoteID - ID number of the patient note that the user desires.
'            lngPatientID - ID of the patient associated with the Patient Note
'            lngUserID - UserID of user calling method.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------

    Dim objPlayNote As EMRDB.CNotePlayDB
    Dim rstSQL As ADODB.Recordset

    On Error GoTo ErrTrap:

    Set objPlayNote = CreateObject("EMRDB.CNotePlayDB")
    
    Set rstSQL = objPlayNote.FetchNextNote(lngUserID, lngPlayNoteID, lngPatientID, lngEncounterLogID)
    
    Set FetchNextNote = rstSQL
    
    GetObjectContext.SetComplete
    Set objPlayNote = Nothing
    Exit Function
    
ErrTrap:
    Set objPlayNote = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Public Function FetchPrevNote(ByVal lngUserID As Long, ByVal lngPlayNoteID As Long, _
                              ByVal lngPatientID As Long, Optional ByVal lngEncounterLogID As Long = 0) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 10/06/2017
'Author: Duane C Orth
'Description:  Retrieves previous record from the tblNotePlay table by EncouterLogID.
'Parameters: lngPlayNoteID - ID number of the patient note that the user desires.
'            lngPatientID - ID of the patient associated with the Patient Note
'            lngUserID - UserID of user calling method.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------

    Dim objPlayNote As EMRDB.CNotePlayDB
    Dim rstSQL As ADODB.Recordset

    On Error GoTo ErrTrap:

    Set objPlayNote = CreateObject("EMRDB.CNotePlayDB")
    
    Set rstSQL = objPlayNote.FetchPrevNote(lngUserID, lngPlayNoteID, lngPatientID, lngEncounterLogID)
    
    Set FetchPrevNote = rstSQL
    
    GetObjectContext.SetComplete
    Set objPlayNote = Nothing
    Exit Function
    
ErrTrap:
    Set objPlayNote = Nothing
    GetObjectContext.SetAbort
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
                       ByVal strPlanText As String, _
                       ByVal strNoteText As String, ByVal strMipsText As String, ByVal blnLockedYN As Boolean, _
                       ByVal lngSignedBy As Long, ByVal dtDateSigned As Date, _
                       ByVal strUserName As String) As Long
                       
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Inserts a row into the tblNotePlay table utilizing
'              a stored procedure.
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'Returns: ID (Primary Key) of the row inserted.
'--------------------------------------------------------------------
    Dim objPlayNote As EMRDB.CNotePlayDB
    Dim strErrMessage As String
    
    On Error GoTo ErrTrap:
    
    ' Validate data being passed
  ''  If Not VerifyData(lngTemplateID, strNoteText, strErrMessage, lngEncounterLogID, _
  ''      lngPatientID, , strUserName) Then GoTo ErrTrap
       
    Set objPlayNote = CreateObject("EMRDB.CNotePlayDB")
    
    Insert = objPlayNote.Insert(lngPatientID, lngProviderID, lngEncounterLogID, lngClinicID, lngSupervisorID, dtNoteDate, _
                       lngDiagID1, dtApptStartDate, dtApptEndDate, strHappy, strSad, strAngry, strAfraid, strConfident, strConfused, strCurious, _
                       strIntensity, strToys, strPlayText, strVerbalText, strChildText, strTherapistText, strRoomText, strStructureText, _
                       strRealityText, strBehaviorText, lngActivity, lngPlay, lngContact, strExploreText, strRelationText, strControlText, _
                       strHelplessText, strAgressiveText, strSafetyText, strMasterText, strNurtureText, strGrievingText, strSexualText, strPlayOtherText, _
                       lngOverallSad, lngOverallAnxious, lngOverallFrustrated, lngOverallDependence, lngOverallMaturity, lngOverallFocus, _
                       lngOverallImpulsive, lngOverallSpontaneity, lngOverallIsolation, lngOverallDetachment, strProgressText, _
                       strPlanText, strNoteText, strMipsText, blnLockedYN, lngSignedBy, dtDateSigned, strUserName)
       
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objPlayNote = Nothing
    Exit Function
    
ErrTrap:
    GetObjectContext.SetAbort
    Set objPlayNote = Nothing
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
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
'Description:  Updates a row in the tblNotePlay table utilizing
'              a stored procedure.
'Parameters: Each parameter identifies the column value that will be
'              updated, except for lngUserID, which is for user
'              verification and for the fetch method, but is not passed
'              to EMRDB.CNotePlayDB.Update.
'Returns: Null
'--------------------------------------------------------------------
    
    ' Declare database object
    Dim objPlayNote As EMRDB.CNotePlayDB
    Dim rstPlayNote As ADODB.Recordset
    Dim strErrMessage As String

    On Error GoTo ErrTrap:

    ' Verify that ID number is valid; must be >= 0 (1st index==1)
    ' -- if PatientNoteID was passed, that is.
    If lngPlayNoteID < 0 Then
        strErrMessage = "PatientNoteID number provided is not valid."
        GoTo ErrTrap
    End If

    ' Instantiate database object
    Set objPlayNote = CreateObject("EMRDB.CNotePlayDB")
    Set rstPlayNote = objPlayNote.FetchByID(lngPlayNoteID)
    
    ' Is the note locked?  If so, error, don't update.
    If UCase(rstPlayNote.Fields("fldLockedYN")) = "Y" Then
        strErrMessage = "Record is locked and cannot be updated."
        GoTo ErrTrap
    End If
    
    Set rstPlayNote = Nothing
    
    ' Run database object
    objPlayNote.Update lngPlayNoteID, lngPatientID, lngProviderID, lngEncounterLogID, lngClinicID, lngSupervisorID, dtNoteDate, _
                       lngDiagID1, dtApptStartDate, dtApptEndDate, strHappy, strSad, strAngry, strAfraid, strConfident, strConfused, strCurious, _
                       strIntensity, strToys, strPlayText, strVerbalText, strChildText, strTherapistText, strRoomText, strStructureText, _
                       strRealityText, strBehaviorText, lngActivity, lngPlay, lngContact, strExploreText, strRelationText, strControlText, _
                       strHelplessText, strAgressiveText, strSafetyText, strMasterText, strNurtureText, strGrievingText, strSexualText, strPlayOtherText, _
                       lngOverallSad, lngOverallAnxious, lngOverallFrustrated, lngOverallDependence, lngOverallMaturity, lngOverallFocus, _
                       lngOverallImpulsive, lngOverallSpontaneity, lngOverallIsolation, lngOverallDetachment, strProgressText, _
                       strPlanText, strNoteText, strMipsText, blnLockedYN, lngSignedBy, dtDateSigned, strUserName
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objPlayNote = Nothing
    Exit Sub

ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    
    Set objPlayNote = Nothing
    
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Sub
Public Sub LockNote(ByVal lngPlayNoteID As Long, ByVal lngSignedBy As Long, ByVal dtDateSigned As Date)
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Flags a row in the tblNotePlay table marking the row as
'              deleted or undeleted.
'Parameters: lngPlayNoteID - ID of the row in the table whose value will be
'               updated.
'Returns: Null
'--------------------------------------------------------------------

    ' Declare database object
    Dim objPlayNote As EMRDB.CNotePlayDB
    
    ' Declare error message string
    Dim strErrMessage As String

    On Error GoTo ErrTrap:

    ' Instantiate database object
    Set objPlayNote = CreateObject("EMRDB.CNotePlayDB")
    
    ' Run database object
    objPlayNote.LockNote lngPlayNoteID, lngSignedBy, dtDateSigned
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objPlayNote = Nothing
    Exit Sub

ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    
    Set objPlayNote = Nothing
    
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Sub
Public Sub UnLockNote(ByVal lngPlayNoteID As Long, ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Flags a row in the tblNotePlay table marking the row as
'              deleted or undeleted.
'Parameters: lngPlayNoteID - ID of the row in the table whose value will be
'               updated.
'Returns: Null
'--------------------------------------------------------------------

    ' Declare database object
    Dim objPlayNote As EMRDB.CNotePlayDB
    
    ' Declare error message string
    Dim strErrMessage As String

    On Error GoTo ErrTrap:

    ' Instantiate database object
    Set objPlayNote = CreateObject("EMRDB.CNotePlayDB")
    
    ' Run database object
    objPlayNote.UnLockNote lngPlayNoteID, strUserName
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objPlayNote = Nothing
    Exit Sub

ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    
    Set objPlayNote = Nothing
    
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Sub

Public Sub Delete(ByVal strUserName As String, ByVal lngPlayNoteID As Long)
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Flags a row in the tblNotePlay table marking the row as
'              deleted or undeleted.
'Parameters: lngPlayNoteID - ID of the row in the table whose value will be
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
    Dim objPlayNote As EMRDB.CNotePlayDB
    Dim strErrMessage As String
    
    On Error GoTo ErrTrap:

    ' Instanitate DB obj
    Set objPlayNote = CreateObject("EMRDB.CNotePlayDB")
    
    'Call on valid check
    If Not (objPlayNote.FetchByID(lngPlayNoteID).RecordCount = 0) Then
        If ((objPlayNote.FetchByID(lngPlayNoteID).Fields("fldLockedYN")) = "N") Then
            ' Fire delete method
            Call objPlayNote.Delete(strUserName, lngPlayNoteID)
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
    Set objPlayNote = Nothing
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objPlayNote = Nothing
    GetObjectContext.SetAbort
    
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Sub
Private Function VerifyData(ByVal lngTemplateID As Long, ByVal strNoteText As String, _
    ByRef strErrMessage As String, Optional ByVal lngEncounterLogID As Long, _
    Optional ByVal lngPatientID As Long, Optional ByVal lngPlayNoteID As Long, _
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
    If lngPlayNoteID < 0 Then
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








