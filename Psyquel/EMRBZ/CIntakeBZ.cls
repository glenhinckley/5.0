
'--------------------------------------------------------------------
'Class Name: CIntakeBZ
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  MTS business object designed to   methods
'              associated with the CIntakeDB class.
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


Public Class CIntakeBZ

    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CIntakeBZ"
    Private Const TABLE_NAME As String = "tblIntake"

    Public Function FetchByID(ByVal lngIntakeID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Retrieves records from the tblIntake table.
        'Parameters: lngIntakeID - ID number of the record to retrieve, which
        '               is primary key of table.
        '            lngUserID - UserID of user  ing method.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim obj As New EMRDB.CIntakeDB
        Dim rstSQL As New ADODB.Recordset


        rstSQL = obj.FetchByID(lngIntakeID)
        FetchByID = rstSQL

        obj = Nothing

    End Function
    Public Function FetchByPatient(ByVal lngPatientID As Long, Optional ByVal lngTemplateID As Long = 0) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Retrieves records from the tblIntake table.
        'Parameters: lngPatientID - ID number of the record to retrieve based on the alertID.
        '            lngUserID - UserID of user  ing method.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim obj As New EMRDB.CIntakeDB
        Dim rstSQL As New ADODB.Recordset


        rstSQL = obj.FetchByPatient(lngPatientID, lngTemplateID)
        FetchByPatient = rstSQL

        obj = Nothing

    End Function
    Public Function Insert(ByVal lngPatientID As Long, ByVal lngTemplateID As Long, ByVal lngProviderID As Long, _
                           ByVal lngClinicID As Long, ByVal strNoteTitle As String, ByVal dtNoteDate As Date, _
                           ByVal strChar_1 As String, ByVal strChar_2 As String, ByVal strChar_3 As String, ByVal strChar_4 As String, _
                           ByVal strChar_5 As String, ByVal strChar_6 As String, ByVal strChar_7 As String, ByVal strChar_8 As String, _
                           ByVal strChar_9 As String, ByVal strChar_10 As String, ByVal strChar_11 As String, ByVal strChar_12 As String, _
                           ByVal strChar_13 As String, ByVal strChar_14 As String, ByVal strChar_15 As String, ByVal strChar_16 As String, _
                           ByVal strChar_17 As String, ByVal strChar_18 As String, ByVal strChar_19 As String, ByVal strChar_20 As String, _
                           ByVal dtDate_1 As Date, ByVal dtDate_2 As Date, ByVal dtDate_3 As Date, ByVal dtDate_4 As Date, _
                           ByVal dtDate_5 As Date, ByVal dtDate_6 As Date, ByVal strText_1 As String, ByVal strText_2 As String, _
                           ByVal strNoteText As String, ByVal blnLockedYN As Boolean, ByVal lngSignedBy As Long, _
                           ByVal dtDateSigned As Date, ByVal strUserName As String) As Long
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Inserts a row into the tblIntake table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------
        Dim obj As New EMRDB.CIntakeDB
        Dim strErrMessage As String



        Insert = obj.Insert(lngPatientID, lngTemplateID, lngProviderID, lngClinicID, strNoteTitle, dtNoteDate, _
                           strChar_1, strChar_2, strChar_3, strChar_4, strChar_5, strChar_6, strChar_7, strChar_8, strChar_9, strChar_10, _
                           strChar_11, strChar_12, strChar_13, strChar_14, strChar_15, strChar_16, strChar_17, strChar_18, strChar_19, strChar_20, _
                           dtDate_1, dtDate_2, dtDate_3, dtDate_4, dtDate_5, dtDate_6, strText_1, strText_2, _
                           strNoteText, blnLockedYN, lngSignedBy, dtDateSigned, strUserName)

        obj = Nothing

    End Function
    Public Sub Update(ByVal lngIntakeID As Long, ByVal lngPatientID As Long, ByVal lngTemplateID As Long, ByVal lngProviderID As Long, _
                           ByVal lngClinicID As Long, ByVal strNoteTitle As String, ByVal dtNoteDate As Date, _
                           ByVal strChar_1 As String, ByVal strChar_2 As String, ByVal strChar_3 As String, ByVal strChar_4 As String, _
                           ByVal strChar_5 As String, ByVal strChar_6 As String, ByVal strChar_7 As String, ByVal strChar_8 As String, _
                           ByVal strChar_9 As String, ByVal strChar_10 As String, ByVal strChar_11 As String, ByVal strChar_12 As String, _
                           ByVal strChar_13 As String, ByVal strChar_14 As String, ByVal strChar_15 As String, ByVal strChar_16 As String, _
                           ByVal strChar_17 As String, ByVal strChar_18 As String, ByVal strChar_19 As String, ByVal strChar_20 As String, _
                           ByVal dtDate_1 As Date, ByVal dtDate_2 As Date, ByVal dtDate_3 As Date, ByVal dtDate_4 As Date, _
                           ByVal dtDate_5 As Date, ByVal dtDate_6 As Date, ByVal strText_1 As String, ByVal strText_2 As String, _
                           ByVal strNoteText As String, ByVal blnLockedYN As Boolean, ByVal lngSignedBy As Long, _
                           ByVal dtDateSigned As Date, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 05/26/2020
        'Author: Duane C Orth
        'Description:  Updates a row in the tblIntake table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              updated, except for lngUserID, which is for user
        '              verification and for the fetch method, but is not passed
        '              to EMRDB.CIntakeDB.Update.
        'Returns: Null
        '--------------------------------------------------------------------

        Dim obj As New EMRDB.CIntakeDB
        Dim rstIntake As New ADODB.Recordset
        Dim strErrMessage As String


        ' Verify that ID number is valid; must be >= 0 (1st index==1)
        ' -- if PatientNoteID was passed, that is.
        If lngIntakeID < 0 Then
            strErrMessage = "IntakeID number provided is not valid."
            Exit Sub
        End If

        rstIntake = obj.FetchByID(lngIntakeID)

        ' Is the note locked?  If so, error, don't update.
        If UCase(rstIntake.Fields("fldLockedYN")) = "Y" Then
            strErrMessage = "Record is locked and cannot be updated."
            Exit Sub
        End If

        rstIntake = Nothing

        ' Run database object
        obj.Update(lngIntakeID, lngPatientID, lngTemplateID, lngProviderID, lngClinicID, strNoteTitle, dtNoteDate, _
                           strChar_1, strChar_2, strChar_3, strChar_4, strChar_5, strChar_6, strChar_7, strChar_8, strChar_9, strChar_10, _
                           strChar_11, strChar_12, strChar_13, strChar_14, strChar_15, strChar_16, strChar_17, strChar_18, strChar_19, strChar_20, _
                           dtDate_1, dtDate_2, dtDate_3, dtDate_4, dtDate_5, dtDate_6, strText_1, strText_2, _
                           strNoteText, blnLockedYN, lngSignedBy, dtDateSigned, strUserName)

        obj = Nothing

    End Sub
    Public Sub LockNote(ByVal lngIntakeID As Long, ByVal lngSignedBy As Long, ByVal dtDateSigned As Date)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Flags a row in the tblIntake table marking the row as
        '              deleted or undeleted.
        'Parameters: lngIntakeID - ID of the row in the table whose value will be
        '               updated.
        'Returns: Null
        '--------------------------------------------------------------------

        Dim obj As New EMRDB.CIntakeDB
        Dim strErrMessage As String


        obj.LockNote(lngIntakeID, lngSignedBy, dtDateSigned)


        obj = Nothing

    End Sub
    Public Sub UnLockNote(ByVal lngIntakeID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Flags a row in the tblIntake table marking the row as
        '              deleted or undeleted.
        'Parameters: lngIntakeID - ID of the row in the table whose value will be
        '               updated.
        'Returns: Null
        '--------------------------------------------------------------------

        ' Declare database object
        Dim obj As New EMRDB.CIntakeDB

        ' Declare error message string
        Dim strErrMessage As String




        ' Run database object
        obj.UnLockNote(lngIntakeID, strUserName)

        obj = Nothing

    End Sub
    Public Sub Delete(ByVal strUserName As String, ByVal lngIntakeID As Long)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Flags a row in the tblIntake table marking the row as
        '              deleted or undeleted.
        'Parameters: lngIntakeID - ID of the row in the table whose value will be
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
        Dim obj As New EMRDB.CIntakeDB
        Dim strErrMessage As String


        '  on valid check
        If Not (obj.FetchByID(lngIntakeID).RecordCount = 0) Then
            If ((obj.FetchByID(lngIntakeID).Fields("fldLockedYN")) = "N") Then
                ' Fire delete method
                obj.Delete(strUserName, lngIntakeID)
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
        Optional ByVal lngPatientID As Long = 0, Optional ByVal lngIntakeID As Long = 0, _
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
        If lngIntakeID < 0 Then
            strErrMessage = "Intake NoteID number provided is not valid."
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









