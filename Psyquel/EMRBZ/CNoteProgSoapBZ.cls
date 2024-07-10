
'--------------------------------------------------------------------
'Class Name: CNoteProgSoapBZ
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  MTS business object designed to   methods
'              associated with the CNoteProgSoapDB class.
'--------------------------------------------------------------------
Option Explicit On


Imports ADODB.CommandTypeEnum
Imports ADODB.CompareEnum
Imports ADODB.ParameterAttributesEnum



Imports ADODB.DataTypeEnum
Imports ADODB.ParameterDirectionEnum
Imports ADODB.ExecuteOptionEnum

Imports ADODB.CursorTypeEnum
Imports ADODB.CursorLocationEnum
Imports ADODB.CursorOptionEnum
Imports ADODB.LockTypeEnum
Imports Psyquel.BusinessRules.CoreLibraryIII
' Lydia Orth 

Public Class CNoteProgSoapBZ

    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CNoteProgSoapBZ"
    Private Const TABLE_NAME As String = "tblNoteProgSoap"

    Public Function FetchByID(ByVal lngProgSoapNoteID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Retrieves records from the tblNoteProgSoap table.
        'Parameters: lngProgSoapNoteID - ID number of the record to retrieve, which
        '               is primary key of table.
        '            lngUserID - UserID of user  ing method.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        ' Declares database onject
        Dim objProgSoapNote As New EMRDB.CNoteProgSoapDB
        ' Declares recordset for result returned
        Dim rstSQL As New ADODB.Recordset


        ' Instantiates recordset for result returned
        rstSQL = objProgSoapNote.FetchByID(lngProgSoapNoteID)

        ' Return value set for function
        FetchByID = rstSQL


        objProgSoapNote = Nothing

    End Function
    Public Function FetchByPatient(ByVal lngUserID As Long, ByVal lngPatientID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Retrieves records from the tblNoteProgSoap table.
        'Parameters: lngPatientID - ID number of the record to retrieve based on the alertID.
        '            lngUserID - UserID of user  ing method.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objProgSoapNote As New EMRDB.CNoteProgSoapDB
        Dim rstSQL As New ADODB.Recordset




        rstSQL = objProgSoapNote.FetchByPatient(lngUserID, lngPatientID)

        FetchByPatient = rstSQL

        objProgSoapNote = Nothing

    End Function
    Public Function FetchByEncounter(ByVal lngUserID As Long, ByVal lngEncLogID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Retrieves records from the tblNoteProgSoap table by EncouterLogID.
        'Parameters: lngEncLogID - ID number of the encounter log that the user desires.
        '            lngUserID - UserID of user  ing method.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------

        Dim objProgSoapNote As New EMRDB.CNoteProgSoapDB
        Dim rstSQL As New ADODB.Recordset



        rstSQL = objProgSoapNote.FetchByEncounter(lngUserID, lngEncLogID)

        FetchByEncounter = rstSQL

        objProgSoapNote = Nothing

    End Function
    Public Function FetchNextNote(ByVal lngUserID As Long, ByVal lngProgSoapNoteID As Long, _
                                  ByVal lngPatientID As Long, Optional ByVal lngEncounterLogID As Long = 0) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/06/2017
        'Author: Duane C Orth
        'Description:  Retrieves next record from the tblNoteProgSoap table by EncouterLogID.
        'Parameters: lngProgSoapNoteID - ID number of the patient note that the user desires.
        '            lngPatientID - ID of the patient associated with the Patient Note
        '            lngUserID - UserID of user  ing method.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------

        Dim objProgSoapNote As New EMRDB.CNoteProgSoapDB
        Dim rstSQL As New ADODB.Recordset



        rstSQL = objProgSoapNote.FetchNextNote(lngUserID, lngProgSoapNoteID, lngPatientID, lngEncounterLogID)

        FetchNextNote = rstSQL

        objProgSoapNote = Nothing

    End Function
    Public Function FetchPrevNote(ByVal lngUserID As Long, ByVal lngProgSoapNoteID As Long, _
                                  ByVal lngPatientID As Long, Optional ByVal lngEncounterLogID As Long = 0) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/06/2017
        'Author: Duane C Orth
        'Description:  Retrieves previous record from the tblNoteProgSoap table by EncouterLogID.
        'Parameters: lngProgSoapNoteID - ID number of the patient note that the user desires.
        '            lngPatientID - ID of the patient associated with the Patient Note
        '            lngUserID - UserID of user  ing method.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------

        Dim objProgSoapNote As New EMRDB.CNoteProgSoapDB
        Dim rstSQL As New ADODB.Recordset



        rstSQL = objProgSoapNote.FetchPrevNote(lngUserID, lngProgSoapNoteID, lngPatientID, lngEncounterLogID)

        FetchPrevNote = rstSQL

        objProgSoapNote = Nothing

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
        Dim objProgSoapNote As New EMRDB.CNoteProgSoapDB
        Dim strErrMessage As String


        Insert = objProgSoapNote.Insert(lngPatientID, lngProviderID, lngEncounterLogID, lngClinicID, lngSupervisorID, dtNoteDate, _
                           lngDiagID1, dtApptStartDate, dtApptEndDate, strPlanText, strPurpose, strClientText, strSiHi, strSiHiText, _
                           strTherapistText, strDiagChange, strDiagChangeText, strIntervention, strAppearance, strSpeech, _
                           strMotor, strHygiene, strThoughts, strAffect, strCognitive, strMedicationText, strPatientText, _
                           strRecommend, strCommentText, strPhysicalText, strLabsText, strNoteText, strMipsText, blnLockedYN, lngSignedBy, dtDateSigned, strUserName)


        objProgSoapNote = Nothing

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

        ' Declare error message string
        Dim strErrMessage As String
        ' Declare database object
        Dim objProgSoapNote As New EMRDB.CNoteProgSoapDB

        ' Declare recordset for fetch note
        Dim rstProgSoapNote As New ADODB.Recordset


        ' Verify that ID number is valid; must be >= 0 (1st index==1)
        ' -- if PatientNoteID was passed, that is.
        If lngProgSoapNoteID < 0 Then
            strErrMessage = "PatientNoteID number provided is not valid."
            Exit Sub
        End If



        ' Get the appropriate record
        rstProgSoapNote = objProgSoapNote.FetchByID(lngProgSoapNoteID)

        ' Is the note locked?  If so, error, don't update.
        If UCase(rstProgSoapNote.Fields("fldLockedYN")) = "Y" Then
            strErrMessage = "Record is locked and cannot be updated."
            Exit Sub
        End If

        rstProgSoapNote = Nothing

        ' Run database object
        objProgSoapNote.Update(lngProgSoapNoteID, lngPatientID, lngProviderID, lngEncounterLogID, lngClinicID, lngSupervisorID, dtNoteDate, _
                           lngDiagID1, dtApptStartDate, dtApptEndDate, strPlanText, strPurpose, strClientText, strSiHi, strSiHiText, _
                           strTherapistText, strDiagChange, strDiagChangeText, strIntervention, strAppearance, strSpeech, _
                           strMotor, strHygiene, strThoughts, strAffect, strCognitive, strMedicationText, strPatientText, _
                           strRecommend, strCommentText, strPhysicalText, strLabsText, strNoteText, strMipsText, blnLockedYN, lngSignedBy, dtDateSigned, strUserName)


        objProgSoapNote = Nothing

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
        Dim objProgSoapNote As New EMRDB.CNoteProgSoapDB

        ' Declare error message string
        Dim strErrMessage As String


        ' Run database object
        objProgSoapNote.LockNote(lngProgSoapNoteID, lngSignedBy, dtDateSigned)


        objProgSoapNote = Nothing

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
        Dim objProgSoapNote As New EMRDB.CNoteProgSoapDB

        ' Declare error message string
        Dim strErrMessage As String



        ' Run database object
        objProgSoapNote.UnLockNote(lngProgSoapNoteID, strUserName)


        objProgSoapNote = Nothing

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
        Dim objProgSoapNote As New EMRDB.CNoteProgSoapDB
        Dim strErrMessage As String



        '  on valid check
        If Not (objProgSoapNote.FetchByID(lngProgSoapNoteID).RecordCount = 0) Then
            If ((objProgSoapNote.FetchByID(lngProgSoapNoteID).Fields("fldLockedYN")) = "N") Then
                ' Fire delete method
                objProgSoapNote.Delete(strUserName, lngProgSoapNoteID)
            Else
                ' Tell user that record is locked
                strErrMessage = "The record you are trying to delete is locked."
                Exit Sub
            End If
        Else
            'Tell user that record doesn't exist
            strErrMessage = "The record you are trying to delete does not exist."
            Exit Sub
        End If

        objProgSoapNote = Nothing

    End Sub
    Private Function VerifyData(ByVal lngTemplateID As Long, ByVal strNoteText As String, _
        ByRef strErrMessage As String, Optional ByVal lngEncounterLogID As Long = 0, _
        Optional ByVal lngPatientID As Long = 0, Optional ByVal lngProgSoapNoteID As Long = 0, _
        Optional ByVal strUserName As String = "") As Boolean
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




End Class




