
'--------------------------------------------------------------------
'Class Name: CIntakeTemplateBZ
'Date: 05/25/2020
'Author: Duane C Orth
'Description:  MTS business object designed to   methods
'              associated with the CIntakeTemplateDB class.
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

Public Class CIntakeTemplateBZ

    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CIntakeTemplateBZ"
    Private Const TABLE_NAME As String = "tblIntakeTemplateItem"

    Public Function FetchByID(ByVal lngIntakeID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 05/25/2020
        'Author: Duane C Orth
        'Description:  Retrieves records from the tblProviderIntakeTemplate and the
        '              tblIntakeTemplateItem table.
        'Parameters: lngIntakeID - ID number of the record to retrieve, which
        '               is primary key of table.
        '            lngUserID - UserID of user  ing method.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim obj As New EMRDB.CIntakeTemplateDB
        Dim rstSQL As New ADODB.Recordset

        rstSQL = obj.FetchByID(lngIntakeID)
        FetchByID = rstSQL


        obj = Nothing

    End Function
    Public Function FetchByItemID(ByVal lngItemID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 05/25/2020
        'Author: Duane C Orth
        'Description:  Retrieves records from the tblTemplateItem table.
        'Parameters: lngItemID - ID number of the record to retrieve, which
        '               is primary key of table.
        'Returns: ADODB.Recordset -- should only return one record
        '--------------------------------------------------------------------

        Dim obj As New EMRDB.CIntakeTemplateDB
        Dim rstSQL As New ADODB.Recordset



        rstSQL = obj.FetchByItemID(lngItemID)

        FetchByItemID = rstSQL


        obj = Nothing

    End Function
    Public Function FetchByProvider(ByVal lngProviderID As Long, Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 05/25/2020
        'Author: Duane C Orth
        'Description:  Retrieves records from the tblProviderIntakeTemplate table.
        'Parameters: lngProviderID - ID number of the record to retrieve
        'Returns: ADODB.Recordset -- should return all records associated with a provider
        '--------------------------------------------------------------------

        Dim obj As New EMRDB.CIntakeTemplateDB
        Dim rstSQL As New ADODB.Recordset



        rstSQL = obj.FetchByProvider(lngProviderID, blnIncludeDisabled)

        FetchByProvider = rstSQL

        obj = Nothing

    End Function
    Public Function InsertProviderIntakeTemplate(ByVal lngProviderID As Long, ByVal strTemplateName As String, _
                           ByVal strTemplateDesc As String, ByVal strUserName As String) As Long
        '--------------------------------------------------------------------
        'Date: 05/25/2020
        'Author: Duane C Orth
        'Description:  Inserts a row into the tblProviderIntakeTemplate table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------
        Dim obj As New EMRDB.CIntakeTemplateDB
        Dim strErrMessage As String


        InsertProviderIntakeTemplate = obj.InsertProviderIntakeTemplate(lngProviderID, strTemplateName, strTemplateDesc, strUserName)

        obj = Nothing

    End Function
    Public Function InsertIntakeTemplateItem(ByVal lngTemplateID As Long, _
                    ByVal strItemText As String, ByVal strPossibleAnswers As String, _
                    ByVal strDisplayText As String, ByVal strType As String, _
                    ByVal strHTMLName As String, ByVal lngItemNumber As Long, _
                    ByVal lngRows As Long, ByVal lngColumn As Long, _
                    ByVal lngChars As Long, ByVal strBold As String, _
                    ByVal strUnderline As String, ByVal strItalic As String, _
                    ByVal strAlignment As String, ByVal strIndent As String, _
                    ByVal strCommitted As String, ByVal strUserName As String) As Long

        '--------------------------------------------------------------------
        'Date: 05/25/2020
        'Author: Duane C Orth
        'Description:  Inserts a row into the tblIntakeTemplateItem table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------
        Dim obj As New EMRDB.CIntakeTemplateDB
        Dim strErrMessage As String



        InsertIntakeTemplateItem = obj.InsertIntakeTemplateItem(lngTemplateID, strItemText, strPossibleAnswers, strDisplayText, _
                           strType, strHTMLName, lngItemNumber, lngRows, lngColumn, lngChars, strBold, _
                           strUnderline, strItalic, strAlignment, strIndent, strCommitted, strUserName)

        obj = Nothing

    End Function
    Public Sub UpdateProviderIntakeTemplate(ByVal lngTemplateID As Long, _
            ByVal strTemplateName As String, ByVal strTemplateDesc As String)
        '--------------------------------------------------------------------
        'Date: 05/25/2020
        'Author: Duane C Orth
        'Description:  Updates a row in the tblProviderIntakeTemplate table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              updated, except for lngUserID, which is for user
        '              verification and for the fetch method, but is not passed
        '              to EMRDB.CIntakeTemplateDB.Update.
        'Returns: Null
        '--------------------------------------------------------------------

        Dim obj As New EMRDB.CIntakeTemplateDB
        Dim strErrMessage As String



        ' Verify that ID number is valid; must be >= 0 (1st index==1)
        ' -- if PatientNoteID was passed, that is.
        If lngTemplateID < 0 Then
            strErrMessage = "IntakeID number provided is not valid."
            Exit Sub
        End If

        obj.UpdateProviderIntakeTemplate(lngTemplateID, strTemplateName, strTemplateDesc)

        obj = Nothing

    End Sub
    Public Sub UpdateIntakeTemplateItem(ByVal lngTemplateID As Long, ByVal lngItemID As Long, _
                    ByVal strItemText As String, ByVal strPossibleAnswers As String, _
                    ByVal strDisplayText As String, ByVal strType As String, _
                    ByVal strHTMLName As String, ByVal lngItemNumber As Long, _
                    ByVal lngRows As Long, ByVal lngColumn As Long, _
                    ByVal lngChars As Long, ByVal strBold As String, _
                    ByVal strUnderline As String, ByVal strItalic As String, _
                    ByVal strAlignment As String, ByVal strIndent As String, _
                    ByVal strCommitted As String)
        '--------------------------------------------------------------------
        'Date: 05/25/2020
        'Author: Duane C Orth
        'Description:  Updates a row in the tblIntakeTemplateItem table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              updated, except for lngUserID, which is for user
        '              verification and for the fetch method, but is not passed
        '              to EMRDB.CIntakeTemplateDB.Update.
        'Returns: Null
        '--------------------------------------------------------------------

        Dim obj As New EMRDB.CIntakeTemplateDB
        Dim strErrMessage As String



        ' Verify that ID number is valid; must be >= 0 (1st index==1)
        ' -- if PatientNoteID was passed, that is.
        If lngItemID < 0 Then
            strErrMessage = "IntakeID number provided is not valid."
            Exit Sub
        End If

        obj.UpdateIntakeTemplateItem(lngTemplateID, lngItemID, strItemText, strPossibleAnswers, strDisplayText, _
                           strType, strHTMLName, lngItemNumber, lngRows, lngColumn, lngChars, strBold, _
                           strUnderline, strItalic, strAlignment, strIndent, strCommitted)

        obj = Nothing

    End Sub
    Public Sub DeleteProviderIntakeTemplate(ByVal lngTemplateID As Long)
        '--------------------------------------------------------------------
        'Date: 05/25/2020
        'Author: Duane C Orth
        'Description:  Flags a row in the tblIntakeTemplateItem table marking the row as
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

        Dim obj As New EMRDB.CIntakeTemplateDB
        Dim strErrMessage As String

        obj.DeleteProviderIntakeTemplate(lngTemplateID)

        obj = Nothing

    End Sub
    Public Sub DeleteIntakeTemplateItem(ByVal lngItemID As Long)
        '--------------------------------------------------------------------
        'Date: 05/25/2020
        'Author: Duane C Orth
        'Description:  Flags a row in the tblIntakeTemplateItem table marking the row as
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

        Dim obj As New EMRDB.CIntakeTemplateDB
        Dim strErrMessage As String

        obj.DeleteIntakeTemplateItem(lngItemID)


        obj = Nothing

    End Sub
    Private Function VerifyData(ByVal strNoteText As String, ByRef strErrMessage As String, _
        Optional ByVal lngPatientID As Long = 0, Optional ByVal lngIntakeID As Long = 0, _
        Optional ByVal strUserName As String = "") As Boolean
        '--------------------------------------------------------------------
        'Date: 05/25/2020
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
