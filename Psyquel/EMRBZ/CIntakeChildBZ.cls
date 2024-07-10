'--------------------------------------
'Class Name: CIntakeChildBZ
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  MTS business object designed to   methods
'              associated with the CIntakeChildDB class.
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

Public Class CIntakeChildBZ
    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CIntakeChildBZ"
    Private Const TABLE_NAME As String = "tblIntakeChild"

    Public Function FetchByID(ByVal lngIntakeChildID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Retrieves records from the tblIntakeChild table.
        'Parameters: lngIntakeChildID - ID number of the record to retrieve, which
        '               is primary key of table.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        ' Declares database onject
        Dim obj As New EMRDB.CIntakeChildDB
        ' Declares recordset for result returned
        Dim rstSQL As New ADODB.Recordset



        ' Instantiates recordset for result returned
        rstSQL = obj.FetchByID(lngIntakeChildID)

        ' Return value set for function
        FetchByID = rstSQL

        obj = Nothing

    End Function
    Public Function FetchByPatient(ByVal lngPatientID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Retrieves records from the tblIntakeChild table.
        'Parameters: lngPatientID - ID number of the record to retrieve based on the alertID.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim obj As New EMRDB.CIntakeChildDB
        Dim rstSQL As New ADODB.Recordset


        rstSQL = obj.FetchByPatient(lngPatientID)

        FetchByPatient = rstSQL

        obj = Nothing

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
        'Description:  Inserts a row into the tblIntakeChild table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------
        Dim obj As New EMRDB.CIntakeChildDB
        Dim strErrMessage As String



        Insert = obj.Insert(lngPatientID, lngProviderID, lngClinicID, dtNoteDate, _
                            dtReferralDate, strReferredBy, blnNewsletterYN, strChiefConcern, strSeverity, strGrade, strSchool, strAcademic, strBehavior, strMaritalStatus, _
                            strMaritalLength, strPersonAtHome, strFather, strMother, strStepParents, strSiblings1, strSiblings2, strSiblings3, strSiblings4, strSiblings5, strActivities, _
                            strBirthVaginal, strBirthVaginalComp, strPregnancy, strPregnancySubstance, strSleep, strAnger, strMilestones, strDoctors, strMedicalCare, _
                            strMedicalIssues, strMedications, strMedicalHistory, strHistory, strHistoryOther, strAlcoholHistory, strNeglectHistory, _
                            strBehaviorHistory, strCommunityHistory, strPsychHistory1, strPsychHistory2, strLegalHistory, _
                            strNoteText, blnLockedYN, lngSignedBy, dtDateSigned, strUserName)


        obj = Nothing

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
        'Description:  Updates a row in the tblIntakeChild table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              updated, except for lngUserID, which is for user
        '              verification and for the fetch method, but is not passed
        '              to EMRDB.CIntakeChildDB.Update.
        'Returns: Null
        '--------------------------------------------------------------------

        ' Declare database object
        Dim obj As New EMRDB.CIntakeChildDB

        ' Declare recordset for fetch note
        Dim rstIntakeChild As New ADODB.Recordset

        ' Declare error message string
        Dim strErrMessage As String



        ' Verify that ID number is valid; must be >= 0 (1st index==1)
        ' -- if PatientNoteID was passed, that is.
        If lngIntakeChildID < 0 Then
            strErrMessage = "PatientNoteID number provided is not valid."
            Exit Sub
        End If

        ' Get the appropriate record
        rstIntakeChild = obj.FetchByID(lngIntakeChildID)

        ' Is the note locked?  If so, error, don't update.
        If UCase(rstIntakeChild.Fields("fldLockedYN")) = "Y" Then
            strErrMessage = "Record is locked and cannot be updated."
            Exit Sub
        End If

        rstIntakeChild = Nothing

        ' Run database object
        obj.Update(lngIntakeChildID, lngPatientID, lngProviderID, lngClinicID, dtNoteDate, _
                    dtReferralDate, strReferredBy, blnNewsletterYN, strChiefConcern, strSeverity, strGrade, strSchool, strAcademic, strBehavior, strMaritalStatus, _
                    strMaritalLength, strPersonAtHome, strFather, strMother, strStepParents, strSiblings1, strSiblings2, strSiblings3, strSiblings4, strSiblings5, strActivities, _
                    strBirthVaginal, strBirthVaginalComp, strPregnancy, strPregnancySubstance, strSleep, strAnger, strMilestones, strDoctors, strMedicalCare, _
                    strMedicalIssues, strMedications, strMedicalHistory, strHistory, strHistoryOther, strAlcoholHistory, strNeglectHistory, _
                    strBehaviorHistory, strCommunityHistory, strPsychHistory1, strPsychHistory2, strLegalHistory, _
                    strNoteText, blnLockedYN, lngSignedBy, dtDateSigned, strUserName)


        obj = Nothing

    End Sub
    Public Sub LockNote(ByVal lngIntakeChildID As Long, ByVal lngSignedBy As Long, ByVal lngSignedByRP As Long, ByVal dtDateSigned As Date)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Flags a row in the tblIntakeChild table marking the row as
        '              deleted or undeleted.
        'Parameters: lngIntakeChildID - ID of the row in the table whose value will be
        '               updated.
        'Returns: Null
        '--------------------------------------------------------------------

        ' Declare database object
        Dim obj As New EMRDB.CIntakeChildDB


        ' Instantiate database object


        ' Run database object
        obj.LockNote(lngIntakeChildID, lngSignedBy, lngSignedByRP, dtDateSigned)

        obj = Nothing

    End Sub
    Public Sub UnLockNote(ByVal lngIntakeChildID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Flags a row in the tblIntakeChild table marking the row as
        '              deleted or undeleted.
        'Parameters: lngIntakeChildID - ID of the row in the table whose value will be
        '               updated.
        'Returns: Null
        '--------------------------------------------------------------------

        ' Declare database object
        Dim obj As New EMRDB.CIntakeChildDB



        ' Instantiate database object

        ' Run database object
        obj.UnLockNote(lngIntakeChildID, strUserName)

        obj = Nothing

    End Sub
    Public Sub Delete(ByVal strUserName As String, ByVal lngIntakeChildID As Long)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Flags a row in the tblIntakeChild table marking the row as
        '              deleted or undeleted.
        'Parameters: lngIntakeChildID - ID of the row in the table whose value will be
        '               updated.
        '            strUserName - Login name of the user responsible for
        '               marking the row as deleted.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        ' Declare DB obj
        Dim obj As New EMRDB.CIntakeChildDB
        Dim strErrMessage As String



        '  on valid check
        If Not (obj.FetchByID(lngIntakeChildID).RecordCount = 0) Then
            If ((obj.FetchByID(lngIntakeChildID).Fields("fldLockedYN")) = "N") Then
                ' Fire delete method
                obj.Delete(strUserName, lngIntakeChildID)
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

        obj = Nothing

    End Sub
    Private Function VerifyData(ByVal strNoteText As String, _
        ByRef strErrMessage As String, _
        Optional ByVal lngPatientID As Long = 0, Optional ByVal lngIntakeChildID As Long = 0, _
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
        ' Verify that something was entered into patientNotes text field.
        If Trim(strNoteText) = "" Then
            strErrMessage = "No information was entered for the Patient Note."
            VerifyData = False
            Exit Function
        End If

        ' Verify that ID number is valid; must be >= 0 (1st index==1)
        ' -- if PatientNoteID was passed, that is.
        If lngIntakeChildID < 0 Then
            strErrMessage = "PatientNoteID number provided is not valid."
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




