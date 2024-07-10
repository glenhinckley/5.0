
'--------------------------------------------------------------------
'Class Name: CNoteSoapBZ
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  MTS business object designed to   methods
'              associated with the CNoteSoapDB class.
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

Public Class CNoteSoapBZ



    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CNoteSoapBZ"
    Private Const TABLE_NAME As String = "tblNoteSoap"

    Public Function FetchByID(ByVal lngSoapNoteID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Retrieves records from the tblNoteSoap table.
        'Parameters: lngSoapNoteID - ID number of the record to retrieve, which
        '               is primary key of table.
        '            lngUserID - UserID of user  ing method.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        ' Declares database onject
        Dim objSoapNote As New EMRDB.CNoteSoapDB
        ' Declares recordset for result returned
        Dim rstSQL As New ADODB.Recordset



        ' Instantiates recordset for result returned
        rstSQL = objSoapNote.FetchByID(lngSoapNoteID)

        ' Return value set for function
        FetchByID = rstSQL

        objSoapNote = Nothing

    End Function
    Public Function FetchByPatient(ByVal lngUserID As Long, ByVal lngPatientID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Retrieves records from the tblNoteSoap table.
        'Parameters: lngPatientID - ID number of the record to retrieve based on the alertID.
        '            lngUserID - UserID of user  ing method.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objSoapNote As New EMRDB.CNoteSoapDB
        Dim rstSQL As New ADODB.Recordset


        rstSQL = objSoapNote.FetchByPatient(lngUserID, lngPatientID)

        FetchByPatient = rstSQL


        objSoapNote = Nothing

    End Function
    Public Function FetchByEncounter(ByVal lngUserID As Long, ByVal lngEncLogID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Retrieves records from the tblNoteSoap table by EncouterLogID.
        'Parameters: lngEncLogID - ID number of the encounter log that the user desires.
        '            lngUserID - UserID of user  ing method.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------

        Dim objSoapNote As New EMRDB.CNoteSoapDB
        Dim rstSQL As New ADODB.Recordset


        rstSQL = objSoapNote.FetchByEncounter(lngUserID, lngEncLogID)

        FetchByEncounter = rstSQL

        objSoapNote = Nothing

    End Function
    Public Function FetchNextNote(ByVal lngUserID As Long, ByVal lngSoapNoteID As Long, _
                                  ByVal lngPatientID As Long, Optional ByVal lngEncounterLogID As Long = 0) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/06/2017
        'Author: Duane C Orth
        'Description:  Retrieves next record from the tblNoteSoap table by EncouterLogID.
        'Parameters: lngSoapNoteID - ID number of the patient note that the user desires.
        '            lngPatientID - ID of the patient associated with the Patient Note
        '            lngUserID - UserID of user  ing method.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------

        Dim objSoapNote As New EMRDB.CNoteSoapDB
        Dim rstSQL As New ADODB.Recordset


        rstSQL = objSoapNote.FetchNextNote(lngUserID, lngSoapNoteID, lngPatientID, lngEncounterLogID)

        FetchNextNote = rstSQL

        objSoapNote = Nothing

    End Function
    Public Function FetchPrevNote(ByVal lngUserID As Long, ByVal lngSoapNoteID As Long, _
                                  ByVal lngPatientID As Long, Optional ByVal lngEncounterLogID As Long = 0) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/06/2017
        'Author: Duane C Orth
        'Description:  Retrieves previous record from the tblNoteSoap table by EncouterLogID.
        'Parameters: lngSoapNoteID - ID number of the patient note that the user desires.
        '            lngPatientID - ID of the patient associated with the Patient Note
        '            lngUserID - UserID of user  ing method.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------

        Dim objSoapNote As New EMRDB.CNoteSoapDB
        Dim rstSQL As New ADODB.Recordset



        rstSQL = objSoapNote.FetchPrevNote(lngUserID, lngSoapNoteID, lngPatientID, lngEncounterLogID)

        FetchPrevNote = rstSQL

        objSoapNote = Nothing

    End Function
    Public Function Insert(ByVal lngPatientID As Long, ByVal lngProviderID As Long, _
                           ByVal lngEncounterLogID As Long, ByVal lngClinicID As Long, _
                           ByVal lngSupervisorID As Long, ByVal dtNoteDate As Date, _
                           ByVal lngDiagID1 As Long, ByVal dtApptStartDate As Date, _
                           ByVal dtApptEndDate As Date, ByVal strSubjectText As String, _
                           ByVal strObjectText As String, ByVal strAssessText As String, _
                           ByVal strPlanText As String, ByVal strNoteText As String, _
                           ByVal strMipsText As String, ByVal blnLockedYN As Boolean, _
                           ByVal lngSignedBy As Long, ByVal dtDateSigned As Date, _
                           ByVal strUserName As String) As Long

        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Inserts a row into the tblNoteSoap table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------
        Dim objSoapNote As New EMRDB.CNoteSoapDB
        Dim strErrMessage As String


        Insert = objSoapNote.Insert(lngPatientID, lngProviderID, lngEncounterLogID, lngClinicID, lngSupervisorID, dtNoteDate, _
                           lngDiagID1, dtApptStartDate, dtApptEndDate, strSubjectText, strObjectText, strAssessText, strPlanText, _
                           strNoteText, strMipsText, blnLockedYN, lngSignedBy, dtDateSigned, strUserName)

        objSoapNote = Nothing

    End Function
    Public Sub Update(ByVal lngSoapNoteID As Long, ByVal lngPatientID As Long, ByVal lngProviderID As Long, _
                           ByVal lngEncounterLogID As Long, ByVal lngClinicID As Long, _
                           ByVal lngSupervisorID As Long, ByVal dtNoteDate As Date, _
                           ByVal lngDiagID1 As Long, ByVal dtApptStartDate As Date, _
                           ByVal dtApptEndDate As Date, ByVal strSubjectText As String, _
                           ByVal strObjectText As String, ByVal strAssessText As String, _
                           ByVal strPlanText As String, ByVal strNoteText As String, _
                           ByVal strMipsText As String, ByVal blnLockedYN As Boolean, _
                           ByVal lngSignedBy As Long, ByVal dtDateSigned As Date, _
                           ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Updates a row in the tblNoteSoap table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              updated, except for lngUserID, which is for user
        '              verification and for the fetch method, but is not passed
        '              to EMRDB.CNoteSoapDB.Update.
        'Returns: Null
        '--------------------------------------------------------------------

        ' Declare database object
        Dim objSoapNote As New EMRDB.CNoteSoapDB

        ' Declare recordset for fetch note
        Dim rstSoapNote As New ADODB.Recordset
        ' Declare error message string
        Dim strErrMessage As String


        ' Verify that ID number is valid; must be >= 0 (1st index==1)
        ' -- if PatientNoteID was passed, that is.
        If lngSoapNoteID < 0 Then
            strErrMessage = "PatientNoteID number provided is not valid."
            Exit Sub
        End If


        ' Get the appropriate record
        rstSoapNote = objSoapNote.FetchByID(lngSoapNoteID)

        ' Is the note locked?  If so, error, don't update.
        If UCase(rstSoapNote.Fields("fldLockedYN")) = "Y" Then
            strErrMessage = "Record is locked and cannot be updated."
            Exit Sub
        End If

        rstSoapNote = Nothing

        ' Run database object
        objSoapNote.Update(lngSoapNoteID, lngPatientID, lngProviderID, lngEncounterLogID, lngClinicID, lngSupervisorID, dtNoteDate, _
                           lngDiagID1, dtApptStartDate, dtApptEndDate, strSubjectText, strObjectText, strAssessText, strPlanText, _
                           strNoteText, strMipsText, blnLockedYN, lngSignedBy, dtDateSigned, strUserName)

        objSoapNote = Nothing

    End Sub
    Public Sub LockNote(ByVal lngSoapNoteID As Long, ByVal lngSignedBy As Long, ByVal dtDateSigned As Date)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Flags a row in the tblNoteSoap table marking the row as
        '              deleted or undeleted.
        'Parameters: lngSoapNoteID - ID of the row in the table whose value will be
        '               updated.
        'Returns: Null
        '--------------------------------------------------------------------

        ' Declare database object
        Dim objSoapNote As New EMRDB.CNoteSoapDB

        ' Declare error message string
        Dim strErrMessage As String


        ' Run database object
        objSoapNote.LockNote(lngSoapNoteID, lngSignedBy, dtDateSigned)

        objSoapNote = Nothing

    End Sub
    Public Sub UnLockNote(ByVal lngSoapNoteID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Flags a row in the tblNoteSoap table marking the row as
        '              deleted or undeleted.
        'Parameters: lngSoapNoteID - ID of the row in the table whose value will be
        '               updated.
        'Returns: Null
        '--------------------------------------------------------------------

        ' Declare database object
        Dim objSoapNote As New EMRDB.CNoteSoapDB



        ' Instantiate database object

        ' Run database object
        objSoapNote.UnLockNote(lngSoapNoteID, strUserName)

        objSoapNote = Nothing

    End Sub
    Public Sub Delete(ByVal strUserName As String, ByVal lngSoapNoteID As Long)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Flags a row in the tblNoteSoap table marking the row as
        '              deleted or undeleted.
        'Parameters: lngSoapNoteID - ID of the row in the table whose value will be
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
        Dim objSoapNote As New EMRDB.CNoteSoapDB
        Dim strErrMessage As String




        '  on valid check
        If Not (objSoapNote.FetchByID(lngSoapNoteID).RecordCount = 0) Then
            If ((objSoapNote.FetchByID(lngSoapNoteID).Fields("fldLockedYN")) = "N") Then
                ' Fire delete method
                objSoapNote.Delete(strUserName, lngSoapNoteID)
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

        objSoapNote = Nothing

    End Sub
    Private Function VerifyData(ByVal lngTemplateID As Long, ByVal strNoteText As String, _
        ByRef strErrMessage As String, Optional ByVal lngEncounterLogID As Long = 0, _
        Optional ByVal lngPatientID As Long = 0, Optional ByVal lngSoapNoteID As Long = 0, _
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
        If lngSoapNoteID < 0 Then
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







