
'--------------------------------------------------------------------
'Class Name: CNoteInitialBZ
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  MTS business object designed to   methods
'              associated with the CNoteInitialDB class.
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

Public Class CNoteInitialBZ
    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CNoteInitialBZ"
    Private Const TABLE_NAME As String = "tblNoteInitial"

    Public Function FetchByID(ByVal lngInitialNoteID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Retrieves records from the tblNoteInitial table.
        'Parameters: lngInitialNoteID - ID number of the record to retrieve, which
        '               is primary key of table.
        '            lngUserID - UserID of user  ing method.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        ' Declares database onject
        Dim objInitialNote As New EMRDB.CNoteInitialDB
        ' Declares recordset for result returned
        Dim rstSQL As New ADODB.Recordset



        ' Instantiates recordset for result returned
        rstSQL = objInitialNote.FetchByID(lngInitialNoteID)

        ' Return value set for function
        FetchByID = rstSQL

        objInitialNote = Nothing
        ' Exit on success

    End Function
    Public Function FetchByPatient(ByVal lngUserID As Long, ByVal lngPatientID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Retrieves records from the tblNoteInitial table.
        'Parameters: lngPatientID - ID number of the record to retrieve based on the alertID.
        '            lngUserID - UserID of user  ing method.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objInitialNote As New EMRDB.CNoteInitialDB
        Dim rstSQL As New ADODB.Recordset


        rstSQL = objInitialNote.FetchByPatient(lngUserID, lngPatientID)

        FetchByPatient = rstSQL

        objInitialNote = Nothing

    End Function
    Public Function FetchByEncounter(ByVal lngUserID As Long, ByVal lngEncLogID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Retrieves records from the tblNoteInitial table by EncouterLogID.
        'Parameters: lngEncLogID - ID number of the encounter log that the user desires.
        '            lngUserID - UserID of user  ing method.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------

        Dim objInitialNote As New EMRDB.CNoteInitialDB
        Dim rstSQL As New ADODB.Recordset


        rstSQL = objInitialNote.FetchByEncounter(lngUserID, lngEncLogID)

        FetchByEncounter = rstSQL

        objInitialNote = Nothing

    End Function
    Public Function FetchNextNote(ByVal lngUserID As Long, ByVal lngInitialNoteID As Long, _
                                  ByVal lngPatientID As Long, Optional ByVal lngEncounterLogID As Long = 0) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/06/2017
        'Author: Duane C Orth
        'Description:  Retrieves next record from the tblNoteInitial table by EncouterLogID.
        'Parameters: lngInitialNoteID - ID number of the patient note that the user desires.
        '            lngPatientID - ID of the patient associated with the Patient Note
        '            lngUserID - UserID of user  ing method.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------

        Dim objInitialNote As New EMRDB.CNoteInitialDB
        Dim rstSQL As New ADODB.Recordset



        rstSQL = objInitialNote.FetchNextNote(lngUserID, lngInitialNoteID, lngPatientID, lngEncounterLogID)

        FetchNextNote = rstSQL


        objInitialNote = Nothing

    End Function
    Public Function FetchPrevNote(ByVal lngUserID As Long, ByVal lngInitialNoteID As Long, _
                                  ByVal lngPatientID As Long, Optional ByVal lngEncounterLogID As Long = 0) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/06/2017
        'Author: Duane C Orth
        'Description:  Retrieves previous record from the tblNoteInitial table by EncouterLogID.
        'Parameters: lngInitialNoteID - ID number of the patient note that the user desires.
        '            lngPatientID - ID of the patient associated with the Patient Note
        '            lngUserID - UserID of user  ing method.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------

        Dim objInitialNote As New EMRDB.CNoteInitialDB
        Dim rstSQL As New ADODB.Recordset


        rstSQL = objInitialNote.FetchPrevNote(lngUserID, lngInitialNoteID, lngPatientID, lngEncounterLogID)

        FetchPrevNote = rstSQL

        objInitialNote = Nothing

    End Function
    Public Function Insert(ByVal lngPatientID As Long, ByVal lngProviderID As Long, _
                           ByVal lngEncounterLogID As Long, ByVal lngClinicID As Long, _
                           ByVal lngSupervisorID As Long, ByVal dtNoteDate As Date, _
                           ByVal lngDiagID1 As Long, ByVal dtApptStartDate As Date, _
                           ByVal dtApptEndDate As Date, ByVal strConcernText As String, _
                           ByVal strLegalText As String, ByVal strMedicationsText As String, _
                           ByVal strTreatmentTypeText As String, ByVal strTreatReasonText As String, _
                           ByVal strTreatPerceptionText As String, ByVal strPhysicalText As String, _
                           ByVal strLabsText As String, ByVal strEthnicText As String, _
                           ByVal strDiagnosisText As String, ByVal strPlanText As String, _
                           ByRef varData As Object, _
                           ByVal strNoteText As String, ByVal strMipsText As String, ByVal blnLockedYN As Boolean, _
                           ByVal lngSignedBy As Long, ByVal dtDateSigned As Date, _
                           ByVal strUserName As String) As Long

        '  ByRef varData As Object, ByRef varPCP As Object
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Inserts a row into the tblNoteInitial table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------
        Dim objInitialNote As New EMRDB.CNoteInitialDB
        Dim strErrMessage As String
        Dim dtePriLastUseDate As Date
        Dim dteSecLastUseDate As Date
        Dim dteTerLastUseDate As Date



        ' Validate data being passed
        ''  If Not VerifyData(lngTemplateID, strNoteText, strErrMessage, lngEncounterLogID, _
        ''      lngPatientID, , strUserName) Then Exit Function

        dtePriLastUseDate = CDate("01/01/1900 12:00:00")
        dteSecLastUseDate = CDate("01/01/1900 12:00:00")
        dteTerLastUseDate = CDate("01/01/1900 12:00:00")
        If IsArray(varData) And Not varData.IsNullorEmpty Then
            If IsDate(varData(38)) Then dtePriLastUseDate = CDate(varData(38))
            If IsDate(varData(46)) Then dteSecLastUseDate = CDate(varData(46))
            If IsDate(varData(54)) Then dteTerLastUseDate = CDate(varData(54))
        End If



        Insert = objInitialNote.Insert(lngPatientID, lngProviderID, lngEncounterLogID, lngClinicID, lngSupervisorID, dtNoteDate, _
                           lngDiagID1, dtApptStartDate, dtApptEndDate, strConcernText, strLegalText, strMedicationsText, strTreatmentTypeText, _
                           strTreatReasonText, strTreatPerceptionText, strPhysicalText, strLabsText, strEthnicText, strDiagnosisText, strPlanText, varData, dtePriLastUseDate, dteSecLastUseDate, _
                           dteTerLastUseDate, strNoteText, strMipsText, blnLockedYN, lngSignedBy, dtDateSigned, strUserName)


        objInitialNote = Nothing

    End Function
    Public Sub Update(ByVal lngInitialNoteID As Long, ByVal lngPatientID As Long, ByVal lngProviderID As Long, _
                           ByVal lngEncounterLogID As Long, ByVal lngClinicID As Long, _
                           ByVal lngSupervisorID As Long, ByVal dtNoteDate As Date, _
                           ByVal lngDiagID1 As Long, ByVal dtApptStartDate As Date, _
                           ByVal dtApptEndDate As Date, ByVal strConcernText As String, _
                           ByVal strLegalText As String, ByVal strMedicationsText As String, _
                           ByVal strTreatmentTypeText As String, ByVal strTreatReasonText As String, _
                           ByVal strTreatPerceptionText As String, ByVal strPhysicalText As String, _
                           ByVal strLabsText As String, ByVal strEthnicText As String, _
                           ByVal strDiagnosisText As String, ByVal strPlanText As String, _
                           ByRef varData As Object, _
                           ByVal strNoteText As String, ByVal strMipsText As String, ByVal blnLockedYN As Boolean, _
                           ByVal lngSignedBy As Long, ByVal dtDateSigned As Date, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Updates a row in the tblNoteInitial table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              updated, except for lngUserID, which is for user
        '              verification and for the fetch method, but is not passed
        '              to EMRDB.CNoteInitialDB.Update.
        'Returns: Null
        '--------------------------------------------------------------------

        ' Declare database object
        Dim objInitialNote As New EMRDB.CNoteInitialDB
        Dim rstInitialNote As New ADODB.Recordset
        Dim strErrMessage As String
        Dim dtePriLastUseDate As Date
        Dim dteSecLastUseDate As Date
        Dim dteTerLastUseDate As Date



        If lngInitialNoteID < 0 Then
            strErrMessage = "PatientNoteID number provided is not valid."
            Exit Sub
        End If

        dtePriLastUseDate = CDate("01/01/1900 12:00:00")
        dteSecLastUseDate = CDate("01/01/1900 12:00:00")
        dteTerLastUseDate = CDate("01/01/1900 12:00:00")
        If IsArray(varData) And Not varData.IsNullorEmpty Then
            If IsDate(varData(38)) Then dtePriLastUseDate = CDate(varData(38))
            If IsDate(varData(46)) Then dteSecLastUseDate = CDate(varData(46))
            If IsDate(varData(54)) Then dteTerLastUseDate = CDate(varData(54))
        End If


        rstInitialNote = objInitialNote.FetchByID(lngInitialNoteID)

        ' Is the note locked?  If so, error, don't update.
        If UCase(rstInitialNote.Fields("fldLockedYN")) = "Y" Then
            strErrMessage = "Record is locked and cannot be updated."
            Exit Sub
        End If

        rstInitialNote = Nothing

        ' Run database object
        objInitialNote.Update(lngInitialNoteID, lngPatientID, lngProviderID, lngEncounterLogID, lngClinicID, lngSupervisorID, dtNoteDate, _
                           lngDiagID1, dtApptStartDate, dtApptEndDate, strConcernText, strLegalText, strMedicationsText, strTreatmentTypeText, _
                           strTreatReasonText, strTreatPerceptionText, strPhysicalText, strLabsText, strEthnicText, strDiagnosisText, strPlanText, varData, dtePriLastUseDate, dteSecLastUseDate, _
                           dteTerLastUseDate, strNoteText, strMipsText, blnLockedYN, lngSignedBy, dtDateSigned, strUserName)


        objInitialNote = Nothing

    End Sub

    Public Sub UpdateExam(ByVal lngInitialNoteID As Long, ByVal lngPatientID As Long, ByRef varData As Object, ByVal lngUserID As Long, ByVal strUserName As String)

        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Updates a row in the tblNoteInitial table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              updated, except for lngUserID, which is for user
        '              verification and for the fetch method, but is not passed
        '              to EMRDB.CNoteInitialDB.Update.
        'Returns: Null
        '--------------------------------------------------------------------

        ' Declare database object
        Dim objInitialNote As New EMRDB.CNoteInitialDB
        Dim rstInitialNote As New ADODB.Recordset
        Dim strErrMessage As String
        Dim dteSelfRecentDate As Date
        Dim dteOthRecentDate As Date
        Dim lngCtr As Long
        Dim arrData() As Object
        ReDim arrData(110)



        If lngInitialNoteID < 0 Then
            strErrMessage = "PatientNoteID number provided is not valid."
            Exit Sub
        End If

        If Not IsArray(varData) Or varData.IsNullorEmpty Then
            dteSelfRecentDate = CDate("01/01/1900 12:00:00")
            dteOthRecentDate = CDate("01/01/1900 12:00:00")
        Else
            dteSelfRecentDate = CDate("01/01/1900 12:00:00")
            dteOthRecentDate = CDate("01/01/1900 12:00:00")
            If IsDate(varData(99)) Then dteSelfRecentDate = CDate(varData(99))
            If IsDate(varData(106)) Then dteOthRecentDate = CDate(varData(106))
        End If

        ' Instantiate database object
        rstInitialNote = objInitialNote.FetchByID(lngInitialNoteID)

        ' Is the note locked?  If so, error, don't update.
        If UCase(rstInitialNote.Fields("fldLockedYN").Value) = "Y" Then
            strErrMessage = "Record is locked and cannot be updated."
            Exit Sub
        End If

        rstInitialNote = Nothing

        ' Run database object
        objInitialNote.UpdateExam(lngInitialNoteID, lngPatientID, varData, dteSelfRecentDate, dteOthRecentDate, strUserName)


        objInitialNote = Nothing

    End Sub
    Public Sub LockNote(ByVal lngInitialNoteID As Long, ByVal lngSignedBy As Long, ByVal dtDateSigned As Date)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Flags a row in the tblNoteInitial table marking the row as
        '              deleted or undeleted.
        'Parameters: lngInitialNoteID - ID of the row in the table whose value will be
        '               updated.
        'Returns: Null
        '--------------------------------------------------------------------

        ' Declare database object
        Dim objInitialNote As New EMRDB.CNoteInitialDB

        ' Declare error message string
        Dim strErrMessage As String



        ' Run database object
        objInitialNote.LockNote(lngInitialNoteID, lngSignedBy, dtDateSigned)


        objInitialNote = Nothing

    End Sub
    Public Sub UnLockNote(ByVal lngInitialNoteID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Flags a row in the tblNoteInitial table marking the row as
        '              deleted or undeleted.
        'Parameters: lngInitialNoteID - ID of the row in the table whose value will be
        '               updated.
        'Returns: Null
        '--------------------------------------------------------------------

        ' Declare database object
        Dim objInitialNote As New EMRDB.CNoteInitialDB

        ' Declare error message string
        Dim strErrMessage As String


        ' Run database object
        objInitialNote.UnLockNote(lngInitialNoteID, strUserName)

        objInitialNote = Nothing

    End Sub
    Public Sub Delete(ByVal strUserName As String, ByVal lngInitialNoteID As Long)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Flags a row in the tblNoteInitial table marking the row as
        '              deleted or undeleted.
        'Parameters: lngInitialNoteID - ID of the row in the table whose value will be
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
        Dim objInitialNote As New EMRDB.CNoteInitialDB
        Dim strErrMessage As String



        '  on valid check
        If Not (objInitialNote.FetchByID(lngInitialNoteID).RecordCount = 0) Then
            If ((objInitialNote.FetchByID(lngInitialNoteID).Fields("fldLockedYN")) = "N") Then
                ' Fire delete method
                objInitialNote.Delete(strUserName, lngInitialNoteID)
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


        objInitialNote = Nothing

    End Sub
    Private Function VerifyData(ByVal lngTemplateID As Long, ByVal strNoteText As String, _
        ByRef strErrMessage As String, Optional ByVal lngEncounterLogID As Long = 0, _
        Optional ByVal lngPatientID As Long = 0, Optional ByVal lngInitialNoteID As Long = 0, _
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
        If lngInitialNoteID < 0 Then
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





