Option Strict Off
Option Explicit On



Imports ADODB
Imports ADODB.DataTypeEnum
Imports ADODB.ParameterDirectionEnum
Imports ADODB.ExecuteOptionEnum

Imports ADODB.CursorTypeEnum
Imports ADODB.CursorLocationEnum
Imports ADODB.CursorOptionEnum
Imports ADODB.LockTypeEnum
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CAlertBZ
    '--------------------------------------------------------------------
    'Class Name: CAlertBz
    'Date: 09/11/2000
    'Author: David Nichol
    'Description:  COM+ business object designed to  methods associated
    '              with the CAlertDB class.
    '--------------------------------------------------------------------
    ' Revision History:
    '
    '--------------------------------------------------------------------
    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CAlertBz"
    Private Const TABLE_NAME As String = "tblAlert"

    Public Function Fetch(Optional ByRef blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/11/2000
        'Author: David Nichol
        'Description:  Retrieves records from the tblAlert table.
        'Parameters: blnIncludeDisabled - Optional parameter that identifies if
        '               records flagged as 'Disabled' or 'De-activated' are to be
        '               included in the record set. The default value is False.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim rstSQL As New ADODB.Recordset 'declare recordset
        Dim objAlert As New ListDb.CAlertDB 'declare DB object



        'Instantiate DB object


        'execute DB object fetch method, passing parameters
        rstSQL = objAlert.Fetch(blnIncludeDisabled)

        'return recordset
        Fetch = rstSQL

        'Signal successful completion


        'UPGRADE_NOTE: Object objAlert may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objAlert = Nothing

    End Function

    Public Function FetchByID(ByVal lngAlertID As Integer) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/11/2000
        'Author: Dave Nichol
        'Description:  Retrieves a single records from the tblAlert table by
        '            primary key, via CAlertDB business object, which in turn
        '            s a stored procedure.
        'Parameters: lngAlertID - ID number of row to fetch
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim rstSQL As New ADODB.Recordset 'declare recordset
        Dim objAlert As New ListDb.CAlertDB 'declare DB object




        'execute DB object's fetch method, getting row
        rstSQL = objAlert.FetchByID(lngAlertID)

        'return record
        FetchByID = rstSQL


    End Function


    Public Function Insert(ByVal strAlertText As String) As Integer
        '--------------------------------------------------------------------
        'Date: 09/11/2000
        'Author: David Nichol
        'Description:  Inserts a row into the tblAlert table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------

        Dim objAlert As New ListDb.CAlertDB 'declare database object
        Dim strErrMessage As String 'declare err message string



        'make sure data is valid
        If VerifyData(strAlertText, strErrMessage) Then


            ' DB object's Insert procedure and return result
            Insert = objAlert.Insert(strAlertText)

        End If


        'clean up object
        'UPGRADE_NOTE: Object objAlert may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objAlert = Nothing

    End Function
    Public Sub Update(ByVal lngAlertID As Integer, ByVal strAlertText As String)
        '--------------------------------------------------------------------
        'Date: 09/11/2000
        'Author: David Nichol
        'Description:  Updates a row in the tblAlert table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: Null
        '--------------------------------------------------------------------

        Dim objAlert As New ListDb.CAlertDB 'declare DB object
        Dim strErrMessage As String 'declare err message string



        'make sure parameters are valid
        If Not VerifyData(strAlertText, strErrMessage) Then
            Exit Sub
        End If


        ' db object's update method
        objAlert.Update(lngAlertID, strAlertText)


    End Sub
    Public Sub Deleted(ByVal lngAlertID As Integer, ByVal blnDeleted As Boolean)
        '--------------------------------------------------------------------
        'Date: 09/11/2000
        'Author: David Nichol
        'Description:  Flags a row in the tblAlert table marking the row as
        '              deleted or undeleted.
        'Parameters: blnDeleted - Boolean value identifying if the record is to
        '               be deleted (True) or undeleted (False).
        '            lngID - ID of the row in the table whose value will be
        '               updated.
        '            strUserName - Login name of the user responsible for
        '               marking the row as deleted.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim objAlert As New ListDb.CAlertDB






        ' database object deleted method
        objAlert.Deleted(lngAlertID, blnDeleted)

        objAlert = Nothing

    End Sub

    Private Function VerifyData(ByVal strAlertText As String, ByRef strErrMessage As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 09/11/2000
        'Author: David Nichol
        'Description:  Verifies all required data has been provided by the user.
        'Parameters:  The values to be checked.
        'Returns: Boolean value identifying if all criteria has been satisfied.
        '--------------------------------------------------------------------

        If Trim(strAlertText) = "" Then
            strErrMessage = "Alert text is required."
            VerifyData = False
            Exit Function
        End If

        'If we get here, all is well...
        VerifyData = True

    End Function
End Class