
'--------------------------------------------------------------------
'Class Name: CAlertBZ
'Date: 08/31/2000
'Author: David Nichol and Joshua Lockwood
'Description:  MTS business object designed to   methods
'              associated with the CPatientAlertDB class.
'--------------------------------------------------------------------
'Revision History:
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


Public Class CPatientAlertBZ


    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CAlertBz"
    Private Const PATIENTALERT_TABLE As String = "tblPatientAlert"

    Public Function FetchByID(ByVal lngPatientAlertID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/31/2000
        'Author: Dave Nichol and Joshua Lockwood
        'Description:  Retrieves records from the tblPatientAlert table.
        'Parameters: lngPatientAlertID - ID number of the record to retrieve, which
        '               is primary key of table.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------

        ' Declares database onject
        Dim objPatientAlert As New EMRDB.CPatientAlertDB
        ' Declares recordset for result returned
        Dim rstSQL As New ADODB.Recordset



        ' Instantiates recordset for result returned
        rstSQL = objPatientAlert.FetchByID(lngPatientAlertID)

        ' Return value set for function
        FetchByID = rstSQL



        ' Set DB object to NULL
        objPatientAlert = Nothing


    End Function

    Public Function FetchByAlert(ByVal strAlertText As String, Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/31/2000
        'Author: Dave Nichol and Joshua Lockwood
        'Description:  Retrieves records from the tblPatientAlert table.
        'Parameters: blnIncludeDisabled - Optional parameter that identifies if
        '               records flagged as 'Disabled' or 'De-activated' are to be
        '               included in the record set. The default value is False.
        '            strAlertText - Text of actual alert.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------

        Dim objPatientAlert As New EMRDB.CPatientAlertDB
        Dim rstSQL As New ADODB.Recordset


        rstSQL = objPatientAlert.FetchByAlert(strAlertText, blnIncludeDisabled)
        FetchByAlert = rstSQL

   
        objPatientAlert = Nothing

    End Function


    Public Function FetchByPatient(ByVal lngPatientID As Long, Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/31/2000
        'Author: Dave Nichol and Joshua Lockwood
        'Description:  Retrieves records from the tblPatientAlert table.
        'Parameters: blnIncludeDisabled - Optional parameter that identifies if
        '               records flagged as 'Disabled' or 'De-activated' are to be
        '               included in the record set. The default value is False.
        '            lngPatientID - ID number of the patient whose alerts to retrieve.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objPatientAlert As New EMRDB.CPatientAlertDB
        Dim rstSQL As New ADODB.Recordset

        rstSQL = objPatientAlert.FetchByPatient(lngPatientID, blnIncludeDisabled)
        FetchByPatient = rstSQL


        objPatientAlert = Nothing

    End Function

    Public Function Insert(ByVal lngPatientID As Long, ByVal strAlertText As String, ByVal strUserName As String) As Long
        '--------------------------------------------------------------------
        'Date: 08/31/2000
        'Author: David Nichol and Joshua Lockwood
        'Description:  Inserts a row into the tblPatientAlert table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------

        ' Declare DB object (connection)
        Dim objPatientAlert As New EMRDB.CPatientAlertDB
        ' Declare buffer for err msg
        Dim strErrMessage As String


        ' Validate data being passed
        If Not VerifyData(lngPatientID, strAlertText, strUserName, strErrMessage) Then Exit Function
        ' Instantiate DB obj (connection)

        Insert = objPatientAlert.Insert(lngPatientID, strAlertText, strUserName)
        'Signal successful completion

        objPatientAlert = Nothing
        ' Exit on success

    End Function

    Public Function Update(ByVal lngPatientAlertID As Long, ByVal lngPatientID As Long, ByVal strAlertText As String, ByVal strUserName As String) As Long
        '--------------------------------------------------------------------
        'Date: 08/31/2000
        'Author: David Nichol and Joshua Lockwood
        'Description:  Updates a row in the tblPatientAlert table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: Null
        '--------------------------------------------------------------------


        Dim objPatientAlert As New EMRDB.CPatientAlertDB
        Dim strErrMessage As String


        If Not VerifyData(lngPatientID, strAlertText, strUserName, strErrMessage, lngPatientAlertID) Then Exit Function

        objPatientAlert.Update(lngPatientAlertID, lngPatientID, strAlertText, strUserName)

        objPatientAlert = Nothing

    End Function

    Public Sub Deleted(ByVal lngPatientAlertID As Long, ByVal blnDisabled As Boolean, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 08/31/2000
        'Author: David Nichol and Joshua Lockwood
        'Description:  Flags a row in the tblPatientAlert table marking the row as
        '              deleted or undeleted.
        'Parameters: blnDisabledYN - Boolean value identifying if the record is to
        '               be deleted (True) or undeleted (False).
        '            lngPatientAlertID - ID of the row in the table whose value will be
        '               updated.
        '            strUserName - Login name of the user responsible for
        '               marking the row as deleted.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        ' Declare DB obj
        Dim objPatientAlert As New EMRDB.CPatientAlertDB


        ' Fire delete method
        objPatientAlert.Deleted(lngPatientAlertID, blnDisabled, strUserName)

        objPatientAlert = Nothing

    End Sub

    Private Function VerifyData(ByVal lngPatientID As Long, ByVal strAlertText As String, _
        ByVal strUserName As String, ByRef strErrMessage As String, _
        Optional ByVal lngPatientAlertID As Long = 0) As Boolean
        '--------------------------------------------------------------------
        'Date: 08/31/2000
        'Author: David Nichol and Joshua Lockwood
        'Description:  Verifies all required data has been provided by the user.
        'Parameters:  The values to be checked.
        'Returns: Boolean value identifying if all criteria has been satisfied.
        '--------------------------------------------------------------------
        ' Check to see if username provided is not null
        If Trim(strUserName) = "" Then
            strErrMessage = "A user name is required."
            VerifyData = False
            Exit Function
        End If

        ' Verify that ID number is valid; must be >= 0 (1st index==1)
        If lngPatientID < 0 Then
            strErrMessage = "PatientID number provided is not valid."
            VerifyData = False
            Exit Function
        End If

        ' Make sure that alert string is not empty
        If Trim(strAlertText) = "" Then
            strErrMessage = "No text was provided for the alert."
            VerifyData = False
            Exit Function
        End If

        ' Verify that ID number is valid; must be >= 0 (1st index==1)
        If lngPatientAlertID < 0 Then
            strErrMessage = "AlertID number provided is not valid."
            VerifyData = False
            Exit Function
        End If

        'If we get here, all is well...
        VerifyData = True

    End Function

End Class

