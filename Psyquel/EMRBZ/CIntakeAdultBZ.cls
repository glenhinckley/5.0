
'--------------------------------------------------------------------
'Class Name: CIntakeAdultBZ
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  MTS business object designed to   methods
'              associated with the CIntakeAdultDB class.
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



Public Class CIntakeAdultBZ
    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CIntakeAdultBZ"
    Private Const TABLE_NAME As String = "tblIntakeAdult"


    Dim strErrMessage As String = String.Empty

    Public Function FetchByID(ByVal lngIntakeAdultID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Retrieves records from the tblIntakeAdult table.
        'Parameters: lngIntakeAdultID - ID number of the record to retrieve, which
        '               is primary key of table.
        '            lngUserID - UserID of user  ing method.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        ' Declares database onject
        Dim obj As New EMRDB.CIntakeAdultDB
        ' Declares recordset for result returned
        Dim rstSQL As New ADODB.Recordset


        ' Instantiates recordset for result returned
        rstSQL = obj.FetchByID(lngIntakeAdultID)

        ' Return value set for function
        FetchByID = rstSQL

        ' Set DB object to NULL
        obj = Nothing

    End Function
    Public Function FetchByPatient(ByVal lngPatientID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Retrieves records from the tblIntakeAdult table.
        'Parameters: lngPatientID - ID number of the record to retrieve based on the alertID.
        '            lngUserID - UserID of user  ing method.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim obj As New EMRDB.CIntakeAdultDB
        Dim rstSQL As ADODB.Recordset



        rstSQL = obj.FetchByPatient(lngPatientID)

        FetchByPatient = rstSQL

        obj = Nothing

    End Function
    Public Function Insert(ByVal lngPatientID As Long, ByVal lngProviderID As Long, ByVal lngClinicID As Long, ByVal dtNoteDate As Date, ByVal strChiefConcern As String, ByVal strPCPClinic As String, _
                           ByVal strPCPClinicPhone As String, ByVal strPCPName As String, ByVal strPCPAddress As String, ByVal blnContactPCPYN As Boolean, ByVal strEmployer As String, ByVal strWorkPhone As String, _
                           ByVal strWorkAddress As String, ByVal strOccupation As String, ByVal strDurantion As String, ByVal strRestrictions As String, ByVal strSpouse As String, ByVal strChildren As String, _
                           ByVal blnPastTreatYN As Boolean, ByVal strPastTreatType As String, ByVal strPastTreatWhen1 As String, ByVal strPastTreatWhom1 As String, ByVal strPastTreatWhat1 As String, _
                           ByVal strPastTreatResult1 As String, ByVal strPastTreatWhen2 As String, ByVal strPastTreatWhom2 As String, ByVal strPastTreatWhat2 As String, ByVal strPastTreatResult2 As String, _
                           ByVal blnPastMedsYN As Boolean, ByVal strPastMedsWhen1 As String, ByVal strPastMedsWhom1 As String, ByVal strPastMedsWhat1 As String, ByVal strPastMedsResult1 As String, _
                           ByVal strPastMedsWhen2 As String, ByVal strPastMedsWhom2 As String, ByVal strPastMedsWhat2 As String, ByVal strPastMedsResult2 As String, ByVal strSymptoms As String, _
                           ByVal strRelationship As String, ByVal strFamily As String, ByVal strWorkPerformance As String, ByVal strFriendships As String, ByVal strFinancial As String, ByVal strHealth As String, _
                           ByVal strStress As String, ByVal strMood As String, ByVal strAppetite As String, ByVal strSleep As String, ByVal strSexual As String, ByVal strAlcohol As String, _
                           ByVal strConcentrate As String, ByVal strAnger As String, _
                           ByVal blnLockedYN As Boolean, ByVal lngSignedBy As Long, _
                           ByVal dtDateSigned As Date, ByVal strUserName As String) As Long

        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Inserts a row into the tblIntakeAdult table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------
        Dim obj As New EMRDB.CIntakeAdultDB
        Dim strErrMessage As String



        Insert = obj.Insert(lngPatientID, lngProviderID, lngClinicID, dtNoteDate, strChiefConcern, strPCPClinic, _
                            strPCPClinicPhone, strPCPName, strPCPAddress, blnContactPCPYN, strEmployer, strWorkPhone, _
                            strWorkAddress, strOccupation, strDurantion, strRestrictions, strSpouse, strChildren, _
                            blnPastTreatYN, strPastTreatType, strPastTreatWhen1, strPastTreatWhom1, strPastTreatWhat1, _
                            strPastTreatResult1, strPastTreatWhen2, strPastTreatWhom2, strPastTreatWhat2, strPastTreatResult2, _
                            blnPastMedsYN, strPastMedsWhen1, strPastMedsWhom1, strPastMedsWhat1, strPastMedsResult1, _
                            strPastMedsWhen2, strPastMedsWhom2, strPastMedsWhat2, strPastMedsResult2, strSymptoms, _
                            strRelationship, strFamily, strWorkPerformance, strFriendships, strFinancial, strHealth, _
                            strStress, strMood, strAppetite, strSleep, strSexual, strAlcohol, _
                            strConcentrate, strAnger, blnLockedYN, lngSignedBy, dtDateSigned, strUserName)


        obj = Nothing

    End Function
    Public Sub Update(ByVal lngIntakeAdultID As Long, ByVal lngPatientID As Long, ByVal lngProviderID As Long, ByVal lngClinicID As Long, ByVal dtNoteDate As Date, ByVal strChiefConcern As String, ByVal strPCPClinic As String, _
                           ByVal strPCPClinicPhone As String, ByVal strPCPName As String, ByVal strPCPAddress As String, ByVal blnContactPCPYN As Boolean, ByVal strEmployer As String, ByVal strWorkPhone As String, _
                           ByVal strWorkAddress As String, ByVal strOccupation As String, ByVal strDurantion As String, ByVal strRestrictions As String, ByVal strSpouse As String, ByVal strChildren As String, _
                           ByVal blnPastTreatYN As Boolean, ByVal strPastTreatType As String, ByVal strPastTreatWhen1 As String, ByVal strPastTreatWhom1 As String, ByVal strPastTreatWhat1 As String, _
                           ByVal strPastTreatResult1 As String, ByVal strPastTreatWhen2 As String, ByVal strPastTreatWhom2 As String, ByVal strPastTreatWhat2 As String, ByVal strPastTreatResult2 As String, _
                           ByVal blnPastMedsYN As Boolean, ByVal strPastMedsWhen1 As String, ByVal strPastMedsWhom1 As String, ByVal strPastMedsWhat1 As String, ByVal strPastMedsResult1 As String, _
                           ByVal strPastMedsWhen2 As String, ByVal strPastMedsWhom2 As String, ByVal strPastMedsWhat2 As String, ByVal strPastMedsResult2 As String, ByVal strSymptoms As String, _
                           ByVal strRelationship As String, ByVal strFamily As String, ByVal strWorkPerformance As String, ByVal strFriendships As String, ByVal strFinancial As String, ByVal strHealth As String, _
                           ByVal strStress As String, ByVal strMood As String, ByVal strAppetite As String, ByVal strSleep As String, ByVal strSexual As String, ByVal strAlcohol As String, _
                           ByVal strConcentrate As String, ByVal strAnger As String, _
                           ByVal blnLockedYN As Boolean, ByVal lngSignedBy As Long, _
                           ByVal dtDateSigned As Date, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Updates a row in the tblIntakeAdult table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              updated, except for lngUserID, which is for user
        '              verification and for the fetch method, but is not passed
        '              to EMRDB.CIntakeAdultDB.Update.
        'Returns: Null
        '--------------------------------------------------------------------

        ' Declare database object
        Dim obj As New EMRDB.CIntakeAdultDB

        ' Declare recordset for fetch note
        Dim rstIntakeAdult As New ADODB.Recordset


        ' Verify that ID number is valid; must be >= 0 (1st index==1)
        ' -- if PatientNoteID was passed, that is.
        If lngIntakeAdultID < 0 Then
            strErrMessage = "PatientNoteID number provided is not valid."
            Exit Sub
        End If


        ' Get the appropriate record
        rstIntakeAdult = obj.FetchByID(lngIntakeAdultID)

        ' Is the note locked?  If so, error, don't update.
        If UCase(rstIntakeAdult.Fields("fldLockedYN")) = "Y" Then
            strErrMessage = "Record is locked and cannot be updated."
            Exit Sub
        End If

        rstIntakeAdult = Nothing

        ' Run database object
        obj.Update(lngIntakeAdultID, lngPatientID, lngProviderID, lngClinicID, dtNoteDate, strChiefConcern, strPCPClinic, _
                            strPCPClinicPhone, strPCPName, strPCPAddress, blnContactPCPYN, strEmployer, strWorkPhone, _
                            strWorkAddress, strOccupation, strDurantion, strRestrictions, strSpouse, strChildren, _
                            blnPastTreatYN, strPastTreatType, strPastTreatWhen1, strPastTreatWhom1, strPastTreatWhat1, _
                            strPastTreatResult1, strPastTreatWhen2, strPastTreatWhom2, strPastTreatWhat2, strPastTreatResult2, _
                            blnPastMedsYN, strPastMedsWhen1, strPastMedsWhom1, strPastMedsWhat1, strPastMedsResult1, _
                            strPastMedsWhen2, strPastMedsWhom2, strPastMedsWhat2, strPastMedsResult2, strSymptoms, _
                            strRelationship, strFamily, strWorkPerformance, strFriendships, strFinancial, strHealth, _
                            strStress, strMood, strAppetite, strSleep, strSexual, strAlcohol, _
                            strConcentrate, strAnger, blnLockedYN, lngSignedBy, dtDateSigned, strUserName)

        obj = Nothing

    End Sub

    Public Sub UpdateText(ByVal lngIntakeAdultID As Long, ByVal lngPatientID As Long, _
                          ByVal blnAlcoholCurrentYN As Boolean, ByVal strAlcoholUsage As String, ByVal strAlcoholDays As String, _
                          ByVal blnAlcoholHistoryYN As Boolean, ByVal blnAlcoholConcernsYN As Boolean, ByVal blnDrugsCurrentYN As Boolean, _
                          ByVal blnDrugHistoryYN As Boolean, ByVal blnDrugFamilyHistoryYN As Boolean, _
                          ByVal strDrugFamilyHistory As String, ByVal strOther As String, ByVal strNoteText As String)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Updates a row in the tblIntakeAdult table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              updated, except for lngUserID, which is for user
        '              verification and for the fetch method, but is not passed
        '              to EMRDB.CIntakeAdultDB.Update.
        'Returns: Null
        '--------------------------------------------------------------------

        ' Declare database object
        Dim obj As New EMRDB.CIntakeAdultDB

        ' Declare recordset for fetch note
        Dim rstIntakeAdult As New ADODB.Recordset

        ' Declare error message string
        Dim strErrMessage As String



        ' Verify that ID number is valid; must be >= 0 (1st index==1)
        ' -- if PatientNoteID was passed, that is.
        If lngIntakeAdultID < 0 Then
            strErrMessage = "PatientNoteID number provided is not valid."
            Exit Sub
        End If



        ' Get the appropriate record
        rstIntakeAdult = obj.FetchByID(lngIntakeAdultID)

        ' Is the note locked?  If so, error, don't update.
        If UCase(rstIntakeAdult.Fields("fldLockedYN")) = "Y" Then
            strErrMessage = "Record is locked and cannot be updated."
            Exit Sub
        End If

        rstIntakeAdult = Nothing

        ' Run database object
        obj.UpdateText(lngIntakeAdultID, lngPatientID, _
                          blnAlcoholCurrentYN, strAlcoholUsage, strAlcoholDays, _
                          blnAlcoholHistoryYN, blnAlcoholConcernsYN, blnDrugsCurrentYN, _
                          blnDrugHistoryYN, blnDrugFamilyHistoryYN, _
                          strDrugFamilyHistory, strOther, strNoteText)


        obj = Nothing

    End Sub
    Public Sub LockNote(ByVal lngIntakeAdultID As Long, ByVal lngSignedBy As Long, ByVal dtDateSigned As Date)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Flags a row in the tblIntakeAdult table marking the row as
        '              deleted or undeleted.
        'Parameters: lngIntakeAdultID - ID of the row in the table whose value will be
        '               updated.
        'Returns: Null
        '--------------------------------------------------------------------

        ' Declare database object
        Dim obj As New EMRDB.CIntakeAdultDB

        ' Declare error message string
        Dim strErrMessage As String

        ' Run database object
        obj.LockNote(lngIntakeAdultID, lngSignedBy, dtDateSigned)


        obj = Nothing

    End Sub
    Public Sub UnLockNote(ByVal lngIntakeAdultID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Flags a row in the tblIntakeAdult table marking the row as
        '              deleted or undeleted.
        'Parameters: lngIntakeAdultID - ID of the row in the table whose value will be
        '               updated.
        'Returns: Null
        '--------------------------------------------------------------------

        ' Declare database object
        Dim obj As New EMRDB.CIntakeAdultDB

        ' Declare error message string
        Dim strErrMessage As String


        ' Run database object
        obj.UnLockNote(lngIntakeAdultID, strUserName)


        obj = Nothing

    End Sub
    Public Sub Delete(ByVal strUserName As String, ByVal lngIntakeAdultID As Long)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Flags a row in the tblIntakeAdult table marking the row as
        '              deleted or undeleted.
        'Parameters: lngIntakeAdultID - ID of the row in the table whose value will be
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
        Dim obj As New EMRDB.CIntakeAdultDB
        Dim strErrMessage As String



        '  on valid check
        If Not (obj.FetchByID(lngIntakeAdultID).RecordCount = 0) Then
            If ((obj.FetchByID(lngIntakeAdultID).Fields("fldLockedYN")) = "N") Then
                ' Fire delete method
                obj.Delete(strUserName, lngIntakeAdultID)
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
        Optional ByVal lngPatientID As Long = 0, Optional ByVal lngIntakeAdultID As Long = 0, _
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
        If lngIntakeAdultID < 0 Then
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


