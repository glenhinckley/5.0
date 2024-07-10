
'------------------------------------------------------------------------------------
'Class Name: CTxGroupDB
'Date: 08/31/00
'Author: Juan Castro
'Denis J Finkel
'Description:   MTS object designed to host methods associated with
'               data affecting the tblTransactionGroup table.
'------------------------------------------------------------------------------------
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

Public Class CTxGroupDB


    Private _ConnectionString As String = String.Empty
    Private Const CLASS_NAME As String = "CTxGroupDB"
    Private Const TABLE_NAME As String = "tblTransactionGroup"

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property

    Public Function Insert(ByVal strGroupName As String, ByVal blnNoteOnlyYN As Boolean, ByVal strAddedBy As String) As Long
        '------------------------------------------------------------------------------------
        'Date: 08/31/00
        'Author: Juan Castro
        'Description:   Inserts a row into the tblTransactionGroup utilizing a stored procedure.
        'Parameters:    Each parameter identifies the column value that will be inserted.
        'Returns:       ID (Primary Key) of the row inserted.
        '------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the command object.
        cmdSQL = New ADODB.Command

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspInsTxGroup"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@GroupName", adVarChar, adParamInput, 30, strGroupName))
            .Parameters.Append(.CreateParameter("@NoteOnlyYN", adVarChar, adParamInput, 1, IIf(blnNoteOnlyYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strAddedBy))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        Insert = cmdSQL.Parameters("@ID").Value

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Function
        End If

        'Close the connection
        cnnSQL.Close()

        'free all resources
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion

        Exit Function


        'Signal incompletion and raise the error to the ing enviroment.
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()
        'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME)

    End Function

    Public Function Exists(ByVal strGroupName As String) As Boolean
        '------------------------------------------------------------------------------------
        'Date: 08/31/00
        'Author: Juan Castro
        'Description:  Determines if a Group Name parameter already exists in the table.
        'Parameters:    All parameters are address fields to check
        'Returns:       True if the Group Name exists, false otherwise
        '------------------------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim strSQL As String
        Dim blnExists As Boolean



        'Prepare the SQL Statement
        strSQL = "SELECT "
        strSQL = strSQL & "COUNT(*) AS TOTAL "
        strSQL = strSQL & "FROM "
        strSQL = strSQL & TABLE_NAME
        strSQL = strSQL & " WHERE "
        strSQL = strSQL & " fldGroupName = '" & strGroupName & "';"

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Instantiate and populate the Recordset
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(strSQL, cnnSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing enviroment.
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


        Exit Function


        'Signal incompletion and raise the error to the ing enviroment.
        cnnSQL = Nothing
        rstSQL = Nothing
        'cmdSQL.Execute()
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function

    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '-------------------------------------------------------------------------------------
        'Date: 08/31/00
        'Author: Juan Castro
        'Description:   Retrieves records from the tblTransactionGroup table.
        'Parameters:   blnIncludeDisabled- Optional parameter that identifies if  records flagged as
        '               'Disabled' or 'De-activated' are to be included in the record set. The default value is False
        '
        'Returns: ADODB.Recordset
        '-------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim cmdSQL As ADODB.Command



        rstSQL = New ADODB.Recordset
        cmdSQL = New ADODB.Command
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)
        cmdSQL.ActiveConnection = cnnSQL

        'create the parameter objects
        With cmdSQL
            .CommandText = "uspSelTxGroup"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ShowDisabled", adVarChar, adParamInput, 1, IIf(blnIncludeDisabled = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Execute the stored procedure
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        Fetch = rstSQL

        'disconnect the recordset
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        rstSQL.ActiveConnection = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing enviroment.
        'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME)
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()

    End Function

    Public Sub Update(ByVal lngTxGroupID As Long, ByVal strGroupName As String, ByVal blnNoteOnlyYN As Boolean)
        '------------------------------------------------------------------------------------
        'Date:09/01/00
        'Author: Juan Castro
        'Description:   Updates a row in the tblTransactionGroup table utilizing a stored procedure.
        'Parameters:    Each parameter identifies the column value that will be updated.
        'Returns:       Null
        '-------------------------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdTxGroup"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamInput, , lngTxGroupID))
            .Parameters.Append(.CreateParameter("@GroupName", adVarChar, adParamInput, 30, strGroupName))
            .Parameters.Append(.CreateParameter("@NoteOnlyYN", adVarChar, adParamInput, 1, IIf(blnNoteOnlyYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the command object
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check the SQLErrorNum parameter before deciding to commit the transaction.
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        'Close the connection and free all resources
        cnnSQL.Close()
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Sub


        'Signal incompletion and raise the error to the ing enviroment
        'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME)
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()

    End Sub

    Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngTxGroupID As Long, ByVal strDisabledBy As String)
        '--------------------------------------------------------------------------------------
        'Date: 09/01/00
        'Author: Juan Castro
        'Description:   Flags a row in the tblTransactionGroup table marking the row as deleted or undeleted.
        'Parameters:    blnDeleted- boolean value identifying if the record is to be deleted (True)
        '                   or undeleted (False).
        '               lngTxGroupID- ID of the row in the table whose value will be updated
        '               srtDisabledBy- Login name of the user responsible for marking the row as deleted.
        'Returns:       Null
        '---------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim strSQL As String
        Dim strFlag As String



        'Prepare the SQL statement if blnDeleted is True
        If blnDeleted = True Then
            strFlag = "Y"

            strSQL = "UPDATE "
            strSQL = strSQL & TABLE_NAME
            strSQL = strSQL & " SET "
            strSQL = strSQL & " fldDisabledYN = '" & strFlag & "', "
            strSQL = strSQL & " fldDateDisabled = '" & Now & "', "
            strSQL = strSQL & " fldDisabledBy = '" & strDisabledBy & "' "
            strSQL = strSQL & " WHERE "
            strSQL = strSQL & " fldTxGroupID = " & lngTxGroupID
            'Prepare the SQL statment if blnDeleted is False
        Else
            strFlag = "N"
            strSQL = "UPDATE "
            strSQL = strSQL & TABLE_NAME
            strSQL = strSQL & " SET "
            strSQL = strSQL & " fldDisabledYN = '" & strFlag & "',"
            strSQL = strSQL & " WHERE "
            strSQL = strSQL & " fldTxGroupID = " & lngTxGroupID
        End If

        'Instantiate and prepare the command object
        cmdSQL = New ADODB.Command
        cmdSQL.CommandText = strSQL
        cmdSQL.CommandType = adCmdText

        'Acquire the database connection
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the command object and execute procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Close the connection and free all resources
        cnnSQL.Close()
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Sub


        'Signal incompletion and raise the error to the ing enviroment
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Sub

    Public Function FetchByID(ByVal lngID As Long) As ADODB.Recordset
        '-------------------------------------------------------------------------------------
        'Date: 08/31/00
        'Author: Juan Castro
        'Description:   Retrieves records from the tblTransactionGroup table.
        'Parameters:   blnIncludeDisabled- Optional parameter that identifies if  records flagged as
        '               'Disabled' or 'De-activated' are to be included in the record set. The default value is False
        '
        'Returns: ADODB.Recordset
        '-------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim cmdSQL As ADODB.Command



        rstSQL = New ADODB.Recordset
        cmdSQL = New ADODB.Command
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)
        cmdSQL.ActiveConnection = cnnSQL

        'create the parameter objects
        With cmdSQL
            .CommandText = "uspSelTxGroupByID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamInput, , lngID))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Execute the stored procedure
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        FetchByID = rstSQL

        'disconnect the recordset
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        rstSQL.ActiveConnection = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing enviroment.
        'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME)
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        rstSQL.ActiveConnection = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()


    End Function
End Class
