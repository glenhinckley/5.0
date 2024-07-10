
'--------------------------------------------------------------------
'Class Name: CIntakeReleaseBZ
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  MTS business object designed to   methods
'              associated with the CIntakeReleaseDB class.
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



Public Class CIntakeReleaseBZ


    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CIntakeReleaseBZ"
    Private Const TABLE_NAME As String = "tblIntakeRelease"

    Public Function FetchByID(ByVal lngIntakeReleaseID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Retrieves records from the tblIntakeRelease table.
        'Parameters: lngIntakeReleaseID - ID number of the record to retrieve, which
        '               is primary key of table.
        '            lngUserID - UserID of user  ing method.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        ' Declares database onject
        Dim obj As New EMRDB.CIntakeReleaseDB
        ' Declares recordset for result returned
        Dim rstSQL As New ADODB.Recordset



        ' Instantiates recordset for result returned
        rstSQL = obj.FetchByID(lngIntakeReleaseID)

        ' Return value set for function
        FetchByID = rstSQL


        obj = Nothing

    End Function
    Public Function FetchByPatient(ByVal lngPatientID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Retrieves records from the tblIntakeRelease table.
        'Parameters: lngPatientID - ID number of the record to retrieve based on the alertID.
        '            lngUserID - UserID of user  ing method.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim obj As New EMRDB.CIntakeReleaseDB
        Dim rstSQL As New ADODB.Recordset


        rstSQL = obj.FetchByPatient(lngPatientID)

        FetchByPatient = rstSQL


        obj = Nothing

    End Function
    Public Function Insert(ByVal lngPatientID As Long, ByVal lngProviderID As Long, _
                           ByVal lngClinicID As Long, ByVal dtNoteDate As Date, ByVal strRelease As String, _
                           ByVal strReleaseName As String, ByVal strReleaseAddress As String, ByVal strReleasePhone As String, _
                           ByVal strReleaseFax As String, ByVal strReleasePHI As String, ByVal blnReleaseYN As Boolean, _
                           ByVal strNoteText As String, ByVal blnLockedYN As Boolean, ByVal lngSignedBy As Long, _
                           ByVal dtDateSigned As Date, ByVal strUserName As String) As Long

        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Inserts a row into the tblIntakeRelease table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------
        Dim obj As New EMRDB.CIntakeReleaseDB
        Dim strErrMessage As String



        Insert = obj.Insert(lngPatientID, lngProviderID, lngClinicID, dtNoteDate, _
                           strRelease, strReleaseName, strReleaseAddress, strReleasePhone, strReleaseFax, strReleasePHI, blnReleaseYN, _
                           strNoteText, blnLockedYN, lngSignedBy, dtDateSigned, strUserName)


        obj = Nothing

    End Function
    Public Sub Update(ByVal lngIntakeReleaseID As Long, ByVal lngPatientID As Long, ByVal lngProviderID As Long, _
                           ByVal lngClinicID As Long, ByVal dtNoteDate As Date, ByVal strRelease As String, _
                           ByVal strReleaseName As String, ByVal strReleaseAddress As String, ByVal strReleasePhone As String, _
                           ByVal strReleaseFax As String, ByVal strReleasePHI As String, ByVal blnReleaseYN As Boolean, _
                           ByVal strNoteText As String, ByVal blnLockedYN As Boolean, ByVal lngSignedBy As Long, _
                           ByVal dtDateSigned As Date, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Updates a row in the tblIntakeRelease table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              updated, except for lngUserID, which is for user
        '              verification and for the fetch method, but is not passed
        '              to EMRDB.CIntakeReleaseDB.Update.
        'Returns: Null
        '--------------------------------------------------------------------

        ' Declare database object
        Dim obj As New EMRDB.CIntakeReleaseDB

        ' Declare recordset for fetch note
        Dim rstIntakeRelease As New ADODB.Recordset

        ' Declare error message string
        Dim strErrMessage As String

        ' Verify that ID number is valid; must be >= 0 (1st index==1)
        ' -- if PatientNoteID was passed, that is.
        If lngIntakeReleaseID < 0 Then
            strErrMessage = "IntakeReleaseID number provided is not valid."
            Exit Sub
        End If



        ' Get the appropriate record
        rstIntakeRelease = obj.FetchByID(lngIntakeReleaseID)


        Dim YN As String = String.Empty

        YN = rstIntakeRelease.Fields("fldLockedYN").Value


        ' Is the note locked?  If so, error, don't update.
        If UCase(YN) = "Y" Then
            strErrMessage = "Record is locked and cannot be updated."
            Exit Sub
        End If

        rstIntakeRelease = Nothing

        ' Run database object
        obj.Update(lngIntakeReleaseID, lngPatientID, lngProviderID, lngClinicID, dtNoteDate, _
                           strRelease, strReleaseName, strReleaseAddress, strReleasePhone, strReleaseFax, strReleasePHI, blnReleaseYN, _
                           strNoteText, blnLockedYN, lngSignedBy, dtDateSigned, strUserName)


        obj = Nothing

    End Sub
    Public Sub LockNote(ByVal lngIntakeReleaseID As Long, ByVal lngSignedBy As Long, ByVal dtDateSigned As Date)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Flags a row in the tblIntakeRelease table marking the row as
        '              deleted or undeleted.
        'Parameters: lngIntakeReleaseID - ID of the row in the table whose value will be
        '               updated.
        'Returns: Null
        '--------------------------------------------------------------------

        ' Declare database object
        Dim obj As New EMRDB.CIntakeReleaseDB

        ' Declare error message string
        Dim strErrMessage As String



        ' Run database object
        obj.LockNote(lngIntakeReleaseID, lngSignedBy, dtDateSigned)

        obj = Nothing

    End Sub
    Public Sub UnLockNote(ByVal lngIntakeReleaseID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Flags a row in the tblIntakeRelease table marking the row as
        '              deleted or undeleted.
        'Parameters: lngIntakeReleaseID - ID of the row in the table whose value will be
        '               updated.
        'Returns: Null
        '--------------------------------------------------------------------

        ' Declare database object
        Dim obj As New EMRDB.CIntakeReleaseDB

        ' Declare error message string
        Dim strErrMessage As String



        ' Run database object
        obj.UnLockNote(lngIntakeReleaseID, strUserName)


        obj = Nothing

    End Sub
    Public Sub Delete(ByVal strUserName As String, ByVal lngIntakeReleaseID As Long)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description:  Flags a row in the tblIntakeRelease table marking the row as
        '              deleted or undeleted.
        'Parameters: lngIntakeReleaseID - ID of the row in the table whose value will be
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
        Dim obj As New EMRDB.CIntakeReleaseDB
        Dim strErrMessage As String



        '  on valid check
        If Not (obj.FetchByID(lngIntakeReleaseID).RecordCount = 0) Then
            If ((obj.FetchByID(lngIntakeReleaseID).Fields("fldLockedYN")) = "N") Then
                ' Fire delete method
                obj.Delete(strUserName, lngIntakeReleaseID)
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
        Optional ByVal lngPatientID As Long = 0, Optional ByVal lngIntakeReleaseID As Long = 0, _
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
        If lngIntakeReleaseID < 0 Then
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






