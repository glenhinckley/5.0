Option Strict Off
Option Explicit On
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CServiceBZ
    '--------------------------------------------------------------------
    'Class Name: CServiceBz                                              '
    'Date: 09/14/2004                                                   '
    'Author: Duane C Orth                                               '
    'Description:  MTS business object designed to  methods         '
    '              associated with the CServiceDB class.                 '
    '--------------------------------------------------------------------
    'Revision History:
    '--------------------------------------------------------------------
    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CServiceBz"

    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/14/2004                                                   '
        'Author: Duane C Orth                                               '
        'Description:  Retrieves records from the tblService table.          '
        'Parameters: blnIncludeDisabled - Optional parameter that identifies'
        '              if records flagged as 'Disabled' or 'De-activated'   '
        '              are to be included in the record set. The default    '
        '              value is False.                                      '
        'Returns: Recordset of Services                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '--------------------------------------------------------------------

        Dim objService As New ListDb.CServiceDB
        Dim rstSQL As New ADODB.Recordset



        rstSQL = objService.Fetch(blnIncludeDisabled)

        Fetch = rstSQL

        'UPGRADE_NOTE: Object objService may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objService = Nothing

        'Signal successful completion

    End Function


    Public Function FetchByID(ByVal lngServiceID As Integer) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/12/2001
        'Author: Duane C Orth
        'Description:  Retrieves all Service records associated with a ServiceID
        'Parameters: lngServiceID: ID of Service Group to fetch Service for
        'Returns: Recordset of requested Service
        '--------------------------------------------------------------------
        'Revision History:
        '--------------------------------------------------------------------

        Dim objService As New ListDb.CServiceDB
        Dim rstSQL As New ADODB.Recordset



        rstSQL = objService.FetchByID(lngServiceID)

        FetchByID = rstSQL

        'UPGRADE_NOTE: Object objService may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objService = Nothing


    End Function


    Public Function Insert(ByVal strDescription As String, ByVal strUserName As String) As Integer
        '--------------------------------------------------------------------
        'Date: 09/14/2004
        'Author: Duane C Orth
        'Description:  Inserts a row into the tblService table.
        'Parameters: strDescription - The description of the Service
        '              that will be inserted into the table.
        'Returns: ID (Primary Key) of the row inserted
        '--------------------------------------------------------------------


        Dim objService As New ListDb.CServiceDB
        Dim lngID As Integer
        Dim strErrMessage As String



        'Verify data before proceeding.
        If Not VerifyData(0, strDescription, strErrMessage) Then
            Exit Function
        End If

        Insert = objService.Insert(strDescription, strUserName)


        'Release resources
        'UPGRADE_NOTE: Object objService may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objService = Nothing

        Exit Function


    End Function


    Public Sub Update(ByVal lngID As Integer, ByVal strDescription As String)
        '--------------------------------------------------------------------
        'Date: 09/14/2004
        'Author: Duane C Orth
        'Description:  Updates a row into the tblService table.
        'Parameters:  lngID - ID of the row in the table whose value will be
        '               updated.
        '             strDescription - The Service status description
        '                to which the record will be changed.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim objService As New ListDb.CServiceDB
        Dim strErrMessage As String



        'Verify data before proceeding.
        If Not VerifyData(lngID, strDescription, strErrMessage) Then
            Exit Sub
        End If

        objService.Update(lngID, strDescription)

        'UPGRADE_NOTE: Object objService may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objService = Nothing
        'Signal successful completion



    End Sub

    Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Integer, ByVal strDeletedBy As String)
        '--------------------------------------------------------------------
        'Date: 09/14/2004
        'Author: Duane C Orth
        'Description:  Flags a row in the tblServicePeriod table marking the row as
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


        Dim objService As New ListDb.CServiceDB




        objService.Deleted(blnDeleted, lngID, strDeletedBy)
        'UPGRADE_NOTE: Object objService may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objService = Nothing

    End Sub


    Public Function Exists(ByVal strDescription As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 01/05/2000
        'Author: Duane C Orth
        'Description:  Determines if a Service description identical to the
        '              strDescription parameter already exists in the table.
        'Parameters: strDescription - Service name to be checked
        'Returns: True if the name exists, false otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim objService As New ListDb.CServiceDB
        Dim blnExists As Boolean




        blnExists = objService.Exists(strDescription)
        Exists = blnExists

        'UPGRADE_NOTE: Object objService may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objService = Nothing

    End Function


    Private Function VerifyData(ByVal lngID As Integer, ByVal strDescription As String, ByRef strErrMessage As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 01/04/2000
        'Author: Duane C Orth
        'Description:  Verifies all required data has been provided by the user.
        'Parameters:  The values to be checked.
        'Returns: Boolean value identifying if all criteria has been satisfied.
        '--------------------------------------------------------------------

        If Trim(strDescription) = "" Then
            strErrMessage = "Service Name is required."
            VerifyData = False
            Exit Function
        End If

        'Check for existance only when inserting new data
        If lngID = 0 And Exists(strDescription) Then
            strErrMessage = "Service '" & strDescription & "' already exists."
            VerifyData = False
            Exit Function
        End If

        'If we get here, all is well...
        VerifyData = True

    End Function




    Public Function FetchPeriod(Optional ByVal blnIncludeDisabled As Boolean = False, Optional ByVal strWhere As String = "", Optional ByVal strOrderBy As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/14/2004
        'Author: Duane C Orth
        'Description:  Retrieves records from the tblServicePeriod table.
        'Parameters: blnIncludeDisabled - Optional parameter that identifies if
        '               records flagged as ' or 'De-activated' are to be
        '               included in the record set. The default value is False.
        '            strWhere - Optional 'Where' clause of the SQL statement by
        '               which records will be filtered.  If the parameter is not
        '               supplied, all records will be retrieved.
        '             strOrderBy - Optional 'Order By' clause in which retrieved
        '               records will be sorted.  The default is set to the
        '               'fldPlanName' column
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objService As New ListDb.CServiceDB
        Dim rstSQL As New ADODB.Recordset


        rstSQL = objService.FetchPeriod(blnIncludeDisabled, strWhere, strOrderBy)
        FetchPeriod = rstSQL

        'UPGRADE_NOTE: Object objService may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objService = Nothing

    End Function

    Public Function InsertPeriod(ByVal strServicePeriod As String, ByVal strDescription As String, ByVal strUserName As String) As Integer
        '--------------------------------------------------------------------
        'Date: 09/14/2004
        'Author: Duane C Orth
        'Description:  Inserts a row into the tblServicePeriod table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------

        Dim objService As New ListDb.CServiceDB
        Dim strErrMessage As String


        InsertPeriod = objService.InsertPeriod(strServicePeriod, strDescription, strUserName)

        'UPGRADE_NOTE: Object objService may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objService = Nothing
    End Function

    Public Sub UpdatePeriod(ByVal lngID As Integer, ByVal strServicePeriod As String, ByVal strDescription As String)
        '--------------------------------------------------------------------
        'Date: 09/14/2004
        'Author: Duane C Orth
        'Description:  Updates a row in the tblServicePeriod table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: Null
        '--------------------------------------------------------------------

        Dim objService As New ListDb.CServiceDB
        Dim strErrMessage As String



        objService.UpdatePeriod(lngID, strServicePeriod, strDescription)

        'UPGRADE_NOTE: Object objService may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objService = Nothing

    End Sub

    Public Sub DeletedPeriod(ByVal blnDeleted As Boolean, ByVal lngID As Integer, ByVal strDeletedBy As String)
        '--------------------------------------------------------------------
        'Date: 09/14/2004
        'Author: Duane C Orth
        'Description:  Flags a row in the tblServicePeriod table marking the row as
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


        Dim objService As New ListDb.CServiceDB




        objService.DeletedPeriod(blnDeleted, lngID, strDeletedBy)


    End Sub
End Class