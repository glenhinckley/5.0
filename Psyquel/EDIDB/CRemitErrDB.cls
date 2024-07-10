
'--------------------------------------------------------------------
'Class Name: CInsTypeDB
'Date: 08/06/2004
'Author: Duane C Orth
'Description:  MTS object designed to host methods associated with
'              data affecting the tblInsuranceType table.
'--------------------------------------------------------------------
'
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

Public Class CRemitErrDB

    Private Const CLASS_NAME As String = "CRemitErrDB"
    Private Const TABLE_NAME As String = "tblRemittanceErrorCodes"



    Private _ConnectionString As String = String.Empty
    Private _AppPath As String = String.Empty
    Private _EDIOutput As String = String.Empty
    Private _MachineName As String = String.Empty
    Private _UserLoginName As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property






    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False, _
                          Optional ByVal strWhere As String = "", _
                          Optional ByVal strOrderBy As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 04/20/2009
        'Author: Duane C Orth
        'Description:  Retrieves records from the tblRemittanceErrorCodes table.
        'Parameters: blnIncludeDisabled - Optional parameter that identifies if
        '               records flagged as 'Disabled' or 'De-activated' are to be
        '               included in the record set. The default value is False.
        '            strWhere - Optional 'Where' clause of the SQL statement by
        '               which records will be filtered.  If the parameter is not
        '               supplied, all records will be retrieved.
        '            strOrderBy - Optional 'Order By' clause in which retrieved
        '               records will be sorted.  The default is set to the
        '               'fldName' column
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim strDfltWhere As String
        Dim strSQL As String



        'Prepare the SQL statement.
        strSQL = "SELECT "
        strSQL = strSQL & TABLE_NAME & ".* "
        strSQL = strSQL & "FROM "
        strSQL = strSQL & TABLE_NAME & " "

        'Determine whether to include 'disabled' records'
        If blnIncludeDisabled = False Then
            strDfltWhere = " WHERE fldDisabledYN = 'N' "
        End If

        'Attach user's where clause if applicable
        strSQL = strSQL & strDfltWhere

        If Trim(strWhere > "") Then If blnIncludeDisabled Then strSQL = strSQL & " WHERE " & strWhere & " " Else strSQL = strSQL & " AND " & strWhere & " "

        If Trim(strOrderBy) > "" Then
            strSQL = strSQL & " ORDER BY "
            strSQL = strSQL & strOrderBy
        Else
            strSQL = strSQL & " ORDER BY "
            strSQL = strSQL & "fldInsName "
        End If


        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Instantiate and populate the Recordset.
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(strSQL, cnnSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rstSQL.ActiveConnection = Nothing

        cnnSQL.Close()
        cnnSQL = Nothing
        Fetch = rstSQL

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        rstSQL.ActiveConnection = Nothing
        rstSQL = Nothing
        cnnSQL = Nothing
        'cmdSQL.Execute()
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function

    Public Function Insert(ByVal strErrorCode As String, _
                           ByVal strDescription As String, _
                           ByVal blnPostError As Boolean, _
                           ByVal blnPostAmount As Boolean, _
                           ByVal blnCoInsAmount As Boolean, _
                           ByVal lngTxTypeID As Long, _
                           ByVal lngBillStatusID As Long, _
                           ByVal strAddedBy As String) As Long
        '--------------------------------------------------------------------
        'Date: 08/06/2004
        'Author: Duane C Orth
        'Description:  Inserts a row into the tblRemittanceErrorCodes table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim intCount As Integer



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspInsRemittanceErrorCode"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@RemittanceErrorRecordID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@ErrorCode", adVarChar, adParamInput, 10, strErrorCode))
            .Parameters.Append(.CreateParameter("@Description", adVarChar, adParamInput, 250, strDescription))
            .Parameters.Append(.CreateParameter("@PostErrorYN", adVarChar, adParamInput, 1, IIf(blnPostError = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@PostAmountYN", adVarChar, adParamInput, 1, IIf(blnPostAmount = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@CoInsAmountYN", adVarChar, adParamInput, 1, IIf(blnCoInsAmount = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@TxTypeID", adInteger, adParamInput, , lngTxTypeID))
            .Parameters.Append(.CreateParameter("@BillStatusID", adInteger, adParamInput, , lngBillStatusID))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 20, strAddedBy))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        Insert = cmdSQL.Parameters("@RemittanceErrorRecordID").Value

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
            Exit Function
        End If

        'Close the connection
        cnnSQL.Close()

        'free all resources
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()
        'RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME)

    End Function
    Public Sub Update(ByVal lngID As Long, _
                      ByVal strErrorCode As String, _
                      ByVal strDescription As String, _
                      ByVal blnPostError As Boolean, _
                      ByVal blnPostAmount As Boolean, _
                      ByVal blnCoInsAmount As Boolean, _
                      ByVal lngTxTypeID As Long, _
                      ByVal lngBillStatusID As Long, _
                      ByVal strChangedBy As String)
        '--------------------------------------------------------------------
        'Date: 08/06/2004
        'Author: Duane C Orth
        'Description:  Updates a row in the tblRemittanceErrorCodes table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: Null
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdRemittanceErrorCode"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@RemittanceErrorRecordID", adInteger, adParamInput, , lngID))
            .Parameters.Append(.CreateParameter("@ErrorCode", adVarChar, adParamInput, 10, strErrorCode))
            .Parameters.Append(.CreateParameter("@Description", adVarChar, adParamInput, 250, strDescription))
            .Parameters.Append(.CreateParameter("@PostErrorYN", adVarChar, adParamInput, 1, IIf(blnPostError = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@PostAmountYN", adVarChar, adParamInput, 1, IIf(blnPostAmount = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@CoInsAmountYN", adVarChar, adParamInput, 1, IIf(blnCoInsAmount = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@TxTypeID", adInteger, adParamInput, , lngTxTypeID))
            .Parameters.Append(.CreateParameter("@BillStatusID", adInteger, adParamInput, , lngBillStatusID))
            .Parameters.Append(.CreateParameter("@ChangedBy", adVarChar, adParamInput, 20, strChangedBy))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
            Exit Sub
        End If

        'Close the connection and free all resources
        cnnSQL.Close()

        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion



    End Sub
    Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 08/06/2004
        'Author: Duane C Orth
        'Description:  Flags a row in the tblRemittanceErrorCodes table marking the row as
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

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim strSQL As String
        Dim strSQL2 As String
        Dim blnFlag As String



        If blnDeleted = True Then
            blnFlag = "Y"
            'Prepare the SQL statement
            strSQL = "UPDATE "
            strSQL = strSQL & TABLE_NAME
            strSQL = strSQL & " SET "
            strSQL = strSQL & " fldDisabledYN = '" & blnFlag & "', "
            strSQL = strSQL & " fldDateDisabled = '" & Now & "', "
            strSQL = strSQL & " fldDisabledBy = '" & strUserName & "' "
            strSQL = strSQL & "WHERE "
            strSQL = strSQL & "fldRemittanceErrorRecordID = " & lngID
        Else
            blnFlag = "N"
            'Prepare the SQL statement
            strSQL = "UPDATE "
            strSQL = strSQL & TABLE_NAME
            strSQL = strSQL & " SET "
            strSQL = strSQL & " fldDisabledYN = '" & blnFlag & "' "
            strSQL = strSQL & "WHERE "
            strSQL = strSQL & "fldRemittanceErrorRecordID = " & lngID
        End If

        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        cmdSQL.CommandText = strSQL
        cmdSQL.CommandType = adCmdText

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Close the connection and free all resources
        cnnSQL.Close()
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion




    End Sub
    Public Function Exists(ByVal strErrorCode As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 08/06/2004
        'Author: Duane C Orth
        'Description:  Determines if a Plan Name identical to the
        '              strInsType Name parameter already exists in the table.
        'Parameters: strInsType - Plan name to be checked
        'Returns: True if the name exists, false otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim strSQL As String
        Dim blnExists As Boolean



        'Prepare the SQL statement
        strSQL = "SELECT "
        strSQL = strSQL & "COUNT(*) AS TOTAL "
        strSQL = strSQL & "FROM "
        strSQL = strSQL & TABLE_NAME
        strSQL = strSQL & " WHERE "
        strSQL = strSQL & " fldRemittanceErrorCode = '" & Trim(strErrorCode) & "' "

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Instantiate and populate the Recordset.
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(strSQL, cnnSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rstSQL.ActiveConnection = Nothing

        'Close the connection and free all resources
        cnnSQL.Close()
        cnnSQL = Nothing

        'Get the value from the Recordset then destroy it.
        If rstSQL.Fields("TOTAL").Value > 0 Then
            Exists = True
        Else
            Exists = False
        End If
        rstSQL = Nothing

        'Signal successful completion


    End Function



End Class
