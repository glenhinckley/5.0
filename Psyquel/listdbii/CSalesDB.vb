Option Strict Off
Option Explicit On
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CSalesDB
    '--------------------------------------------------------------------
    'Class Name: CSalesDB
    'Date: 10/27/2009
    'Author: Duane C Orth
    'Description:  MTS object designed to host methods associated with
    '              data affecting the tblSalesReps table.
    '--------------------------------------------------------------------
    ' Revision History:                                                 '
    '                                                                   '
    '--------------------------------------------------------------------

    Private Const CLASS_NAME As String = "CSalesDB"
    Private Const TABLE_NAME As String = "tblSalesRep"
    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property

    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False, Optional ByVal strWhere As String = "", Optional ByVal strOrderBy As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/27/2009
        'Author: Duane C Orth
        'Description:  Retrieves records from the tblSalesRep table.
        'Parameters: blnIncludeDisabled - Optional parameter that identifies if
        '               records flagged as 'Disabled' or 'De-activated' are to be
        '               included in the record set. The default value is False.
        '            strWhere - Optional 'Where' clause of the SQL statement by
        '               which records will be filtered.  If the parameter is not
        '               supplied, all records will be retrieved.
        '             strOrderBy - Optional 'Order By' clause in which retrieved
        '               records will be sorted.  The default is set to the
        '               'fldRole' column
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim cnnSQL As New ADODB.Connection
        Dim rstSQL As New ADODB.Recordset
        Dim strDfltWhere As String
        Dim strSQL As String



        'Prepare the SQL statement.
        strSQL = "SELECT "
        strSQL = strSQL & "* "
        strSQL = strSQL & "FROM "
        strSQL = strSQL & TABLE_NAME

        'Determine whether to include 'disabled' records'
        If blnIncludeDisabled = False Then
            strDfltWhere = " WHERE fldDisabledYN = 'N'"
        End If

        'Attach user's where clause if applicable
        If Trim(strWhere) > "" Then
            If Trim(strDfltWhere) > "" Then
                strSQL = strSQL & strDfltWhere & " AND "
                strSQL = strSQL & strWhere
            Else
                strSQL = strSQL & " WHERE "
                strSQL = strSQL & strWhere
            End If
        Else
            strSQL = strSQL & strDfltWhere
        End If

        If Trim(strOrderBy) > "" Then
            strSQL = strSQL & " ORDER BY "
            strSQL = strSQL & strOrderBy
        Else
            strSQL = strSQL & " ORDER BY "
            strSQL = strSQL & "fldSalesRepDesc"
        End If

        'Instantiate and populate the Recordset.
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = ADODB.CursorLocationEnum.adUseClient

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Populate the recordset
        rstSQL.Open(strSQL, cnnSQL, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText + ADODB.ExecuteOptionEnum.adAsyncFetch)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        'UPGRADE_NOTE: Object rstSQL.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        rstSQL.ActiveConnection = Nothing

        cnnSQL.Close()
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing
        Fetch = rstSQL



    End Function


    Public Function Insert(ByVal strSalesRepDesc As String, ByVal lngBaseCom As Integer, ByVal dblFullServiceCom As Double, ByVal dblSubmissionCom As Double) As Integer
        '--------------------------------------------------------------------
        'Date: 10/27/2009
        'Author: Duane C Orth
        'Description:  Inserts a row into the tblSalesRep table utilizing
        '              stored a stored procedure.
        'Parameters:  strSalesRepDesc - The name of the Sales Rep
        '             strDescription - The description of the Sales Rep
        '              that will be inserted into the table.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------

        Dim cnnSQL As New ADODB.Connection
        Dim cmdSQL As New ADODB.Command

        '

        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspInsSalesRep"
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
            .Parameters.Append(.CreateParameter(, ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamReturnValue))
            .Parameters.Append(.CreateParameter("@SalesRepDesc", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 50, strSalesRepDesc))
            .Parameters.Append(.CreateParameter("@BaseCom", ADODB.DataTypeEnum.adDouble, ADODB.ParameterDirectionEnum.adParamInput, , lngBaseCom))
            .Parameters.Append(.CreateParameter("@FullServiceCom", ADODB.DataTypeEnum.adDouble, ADODB.ParameterDirectionEnum.adParamInput, , dblFullServiceCom))
            .Parameters.Append(.CreateParameter("@SubmissionCom", ADODB.DataTypeEnum.adDouble, ADODB.ParameterDirectionEnum.adParamInput, , dblSubmissionCom))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute() '()

        ''Check for errors
        'If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
        '	GoTo ErrTrap
        'End If

        Insert = cmdSQL.Parameters(0).Value

        'Close the connection and free all resources
        cnnSQL.Close()
        'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmdSQL = Nothing
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing

        'Signal successful completion


        Exit Function

        ' 
        '		'Signal incompletion and raise the error to the ing environment.
        '		System.EnterpriseServicesxxxxxxxXXXXXX.ContextUtil.SetAbort()
        '		'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cnnSQL = Nothing
        '		'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cmdSQL = Nothing
        Insert = -1
        '		 'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME)

    End Function


    Public Sub Update(ByVal lngID As Integer, ByVal strSalesRepDesc As String, ByVal lngBaseCom As Integer, ByVal dblFullServiceCom As Double, ByVal dblSubmissionCom As Double)
        '--------------------------------------------------------------------
        'Date: 10/27/2009
        'Author: Duane C Orth
        'Description:  Updates a row into the tblSalesRep table utilizing
        '              a stored procedure.
        'Parameters:  lngID - ID of the row in the table whose value will be
        '               updated.
        '             strSalesRepDesc - The name given to the Role.
        '             strDescription - The Role status description
        '                to which the record will be changed.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        With cmdSQL
            .CommandText = "uspUpdSalesRep"
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamInput, , lngID))
            .Parameters.Append(.CreateParameter("@SalesRepDesc", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 50, strSalesRepDesc))
            .Parameters.Append(.CreateParameter("@BaseCom", ADODB.DataTypeEnum.adDouble, ADODB.ParameterDirectionEnum.adParamInput, , lngBaseCom))
            .Parameters.Append(.CreateParameter("@FullServiceCom", ADODB.DataTypeEnum.adDouble, ADODB.ParameterDirectionEnum.adParamInput, , dblFullServiceCom))
            .Parameters.Append(.CreateParameter("@SubmissionCom", ADODB.DataTypeEnum.adDouble, ADODB.ParameterDirectionEnum.adParamInput, , dblSubmissionCom))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute(, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)

        'Check for Errors
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        'Close the connection and free all resources
        cnnSQL.Close()
        'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmdSQL = Nothing
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing

        'Signal successful completion



    End Sub


    Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Integer, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 10/27/2009
        'Author: Duane C Orth
        'Description:  Flags a row in the tblSalesReps table marking the row as
        '              deleted or undeleted.
        'Parameters: blnDeleted - Boolean value identifying if the record is to
        '               be deleted (True) or undeleted (False).
        '            lngID - ID of the row in the table whose value will be
        '               updated.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim strSQL As String
        Dim blnFlag As String



        If blnDeleted = True Then
            blnFlag = "Y"
        Else
            blnFlag = "N"
        End If

        'Prepare the SQL statement
        strSQL = "UPDATE "
        strSQL = strSQL & TABLE_NAME
        strSQL = strSQL & " SET "
        strSQL = strSQL & " fldDisabledYN = '" & blnFlag & "', "
        strSQL = strSQL & " fldDateDisabled = '" & Now & "', "
        strSQL = strSQL & " fldDisabledBy = '" & strUserName & "' "
        strSQL = strSQL & "WHERE "
        strSQL = strSQL & "fldSalesRepID = " & lngID

        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        cmdSQL.CommandText = strSQL
        cmdSQL.CommandType = ADODB.CommandTypeEnum.adCmdText

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute(, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)

        'Close the connection and free all resources
        cnnSQL.Close()
        'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmdSQL = Nothing
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing



    End Sub


    Public Function Exists(ByVal strSalesRepDesc As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 10/27/2009
        'Author: Duane C Orth
        'Description:  Determines if a Sales Record is identical to the
        '              strSalesRepDesc parameter already exists in the table.
        'Parameters: strSalesRepDesc - Role record to be checked
        'Returns: True if the name exists, false otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As New ADODB.Recordset
        Dim strSQL As String
        Dim blnExists As Boolean



        'Prepare the SQL statement
        strSQL = "SELECT "
        strSQL = strSQL & "COUNT(*) AS TOTAL "
        strSQL = strSQL & "FROM "
        strSQL = strSQL & TABLE_NAME
        strSQL = strSQL & " WHERE "
        strSQL = strSQL & " fldSalesRepDesc = '" & strSalesRepDesc & "' "

        'Instantiate the Recordset.
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = ADODB.CursorLocationEnum.adUseClient

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Populate the recordset
        rstSQL.Open(strSQL, cnnSQL, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText + ADODB.ExecuteOptionEnum.adAsyncFetch)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        'UPGRADE_NOTE: Object rstSQL.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        rstSQL.ActiveConnection = Nothing

        'Close the connection and free all resources
        cnnSQL.Close()
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing

        'Get the value from the Recordset then destroy it.
        If rstSQL.Fields("TOTAL").Value > 0 Then
            Exists = True
        Else
            Exists = False
        End If
        'UPGRADE_NOTE: Object rstSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        rstSQL = Nothing

        'Signal successful completion

    End Function
End Class