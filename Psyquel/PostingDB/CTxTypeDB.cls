
'-----------------------------------------------------------------------------------
'Class Name: CTxTypeDB
'Date: 09/01/00
'Author: Juan Castro
'Denis J Finkel
'Description:   MTS object designed to host methods associated with
'               data affecting the tblTransactionType table.
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

Public Class CTxTypeDB
    Private _ConnectionString As String = String.Empty

    Private Const CLASS_NAME As String = "CTxTypeDB"
    Private Const TABLE_NAME As String = "tblTransactionType"


    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property





    Public Function Insert(ByVal strTypeName As String, ByVal lngTxGroupID As Long, _
                           ByVal strCharge As String, ByVal blnQBillYN As Boolean, ByVal blnQWareYN As Boolean, ByVal blnSystemYN As Boolean, _
                           ByVal strProvAction As String, ByVal strPsyAction As String, ByVal strExtDesc As String, ByVal strAddedBy As String) As Long
        '------------------------------------------------------------------------------------
        'Date: 09/01/00
        'Author: Juan Castro
        'Description:   Inserts a row into the tblTransactionType table utilizing a stored procedure.
        'Parameters:    Each parameter identifies the column value that will be inserted.
        'Returns:       ID (Primary Key) of the row inserted.
        '------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspInsTxType"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@TypeName", adVarChar, adParamInput, 100, strTypeName))
            .Parameters.Append(.CreateParameter("@TxGroupID", adInteger, adParamInput, , lngTxGroupID))
            .Parameters.Append(.CreateParameter("@ChargeYN", adVarChar, adParamInput, 1, strCharge))
            .Parameters.Append(.CreateParameter("@QBillYN", adVarChar, adParamInput, 1, IIf(blnQBillYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@QWareYN", adVarChar, adParamInput, 1, IIf(blnQWareYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@SystemYN", adVarChar, adParamInput, 1, IIf(blnSystemYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@ProvAction", adVarChar, adParamInput, 900, strProvAction))
            .Parameters.Append(.CreateParameter("@PsyAction", adVarChar, adParamInput, 900, strPsyAction))
            .Parameters.Append(.CreateParameter("@ExtDesc", adVarChar, adParamInput, 900, strExtDesc))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strAddedBy))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()
        Insert = cmdSQL.Parameters("@ID").Value

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Function
        End If

        'Close the connection
        cnnSQL.Close()

        'Free all resources
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing enviroment
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()
        'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME)

    End Function

    Public Function Exists(ByVal strTypeName As String) As Boolean
        '------------------------------------------------------------------------------------
        'Date: 09/01/00
        'Author: Juan Castro
        'Description:  Determines if a Type Name parameter already exists in the table.
        'Parameters:    All parameters are address fields to check
        'Returns:       True if the Type Name exists, false otherwise
        '------------------------------------------------------------------------------------

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
        strSQL = strSQL & " fldTypeName = '" & strTypeName & "' "

        'Acquire the database connection
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Instantiate and populate the recordset
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(strSQL, cnnSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)

        'Disconnect the recordset
        rstSQL.ActiveConnection = Nothing

        'Close the connection and free all resources
        cnnSQL.Close()
        cnnSQL = Nothing

        'Get the value from the recordset and then destroy it
        If rstSQL.Fields("TOTAL").Value > 0 Then
            Exists = True
        Else
            Exists = False
        End If
        rstSQL = Nothing

        'signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing enviroment.
        cnnSQL = Nothing
        rstSQL = Nothing
        'cmdSQL.Execute()
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function

    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '-------------------------------------------------------------------------------------
        'Date: 09/01/00
        'Author: Juan Castro
        'Description:   Retrieves records from the tblTransactionType table.
        'Parameters:   blnIncludeDisabled- Optional parameter that identifies if  records flagged as
        '               'Disabled' or 'De-activated' are to be included in the record set. The default value is False
        '
        'Returns: ADODB.Recordset
        '-------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim rstSQL As ADODB.Recordset



        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        cmdSQL.ActiveConnection = cnnSQL

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspSelTxType"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ShowDisabled", adVarChar, adParamInput, 1, IIf(blnIncludeDisabled = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Execute the stored procedure
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        Fetch = rstSQL

        'Disconnect the recordset
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        rstSQL.ActiveConnection = Nothing
        rstSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing enviroment.
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        rstSQL.ActiveConnection = Nothing
        rstSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()
        'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME)

    End Function

    Public Sub Update(ByVal lngTxTypeID As Long, ByVal strTypeName As String, ByVal lngTxGroupID As Long, _
                    ByVal strCharge As String, ByVal blnQBillYN As Boolean, ByVal blnQWareYN As Boolean, ByVal blnSystemYN As Boolean, _
                    ByVal strProvAction As String, ByVal strPsyAction As String, ByVal strExtDesc As String)
        '------------------------------------------------------------------------------------
        'Date:09/05/00
        'Author: Juan Castro
        'Description:   Updates a row in the tblTransactionType table utilizing a stored procedure.
        'Parameters:    Each parameter identifies the column value that will be updated.
        'Returns:       Null
        '-------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the command object
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdTxType"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamInput, , lngTxTypeID))
            .Parameters.Append(.CreateParameter("@TypeName", adVarChar, adParamInput, 100, strTypeName))
            .Parameters.Append(.CreateParameter("@TxGroupId", adInteger, adParamInput, , lngTxGroupID))
            .Parameters.Append(.CreateParameter("@ChargeYN", adVarChar, adParamInput, 1, strCharge))
            .Parameters.Append(.CreateParameter("@QBillYN", adVarChar, adParamInput, 1, IIf(blnQBillYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@QWareYN", adVarChar, adParamInput, 1, IIf(blnQWareYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@SystemYN", adVarChar, adParamInput, 1, IIf(blnSystemYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@ProvAction", adVarChar, adParamInput, 900, strProvAction))
            .Parameters.Append(.CreateParameter("@PsyAction", adVarChar, adParamInput, 900, strPsyAction))
            .Parameters.Append(.CreateParameter("@ExtDesc", adVarChar, adParamInput, 900, strExtDesc))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check the SQLErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        'Close the connection and free all resources
        cnnSQL.Close()
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Sub


        'Signal incompletion and raise the error to the ing enviroment.
        'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME)
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()

    End Sub

    Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngTxTypeID As Long, ByVal strDisabledBy As String)
        '------------------------------------------------------------------------------------
        'Date: 09/05/00
        'Author: Juan Castro
        'Description:   Flags a row in the tblTransactionGroup table marking the row as deleted or undeleted.
        'Parameters:    blnDeleted- boolean value identifying if the record is to be deleted (True)
        '                   or undeleted (False).
        '               lngTxGroupID- ID of the row in the table whose value will be updated
        '               srtDisabledBy- Login name of the user responsible for marking the row as deleted.
        'Returns:       Null
        '------------------------------------------------------------------------------------

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
            strSQL = strSQL & " fldDateDisabled = '" & Now & "' ,"
            strSQL = strSQL & " fldDisabledBy = '" & strDisabledBy & "' "
            strSQL = strSQL & " WHERE "
            strSQL = strSQL & " fldTxTypeID = " & lngTxTypeID
            'Prepare the SQL statement if blnDeleted is False
        Else
            strFlag = "N"
            strSQL = "UPDATE "
            strSQL = strSQL & TABLE_NAME
            strSQL = strSQL & " SET "
            strSQL = strSQL & " fldDisabledYN = '" & strFlag & "' "
            strSQL = strSQL & " WHERE "
            strSQL = strSQL & " fldTxTypeID = " & lngTxTypeID
        End If

        'Instantiate and prepare the command object
        cmdSQL = New ADODB.Command
        cmdSQL.CommandText = strSQL
        cmdSQL.CommandType = adCmdText

        'Acquire the database connection
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Close the connection and free all resources
        cnnSQL.Close()
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Sub


        'Signal Incompletion and raise the error to the ing enviroment.
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Sub

    Public Function FetchByID(ByVal lngID As Long) As ADODB.Recordset
        '-------------------------------------------------------------------------------------
        'Date: 08/31/00
        'Author: Juan Castro
        'Description:   Retrieves records from the tblTransactionType table.
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
            .CommandText = "uspSelTxTypeByID"
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
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        rstSQL.ActiveConnection = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()
        'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME)

    End Function

    Public Function FetchByGroupID(ByVal lngTxGroupID As Long) As ADODB.Recordset
        '-------------------------------------------------------------------------------------
        'Date: 08/31/00
        'Author: Juan Castro
        'Description:   Retrieves records from the tblTransactionType table.
        'Parameters:    lngTxGroupID - will return the transaction types that have a group associated
        '                   with them.
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
            .CommandText = "uspSelTxTypeByGroup"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@TxGroupID", adInteger, adParamInput, , lngTxGroupID))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Execute the stored procedure
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        FetchByGroupID = rstSQL

        'disconnect the recordset
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        rstSQL.ActiveConnection = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing enviroment.
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        rstSQL.ActiveConnection = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()
        'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME)

    End Function
    Public Function FetchProvPostings(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '-------------------------------------------------------------------------------------
        'Date: 02/19/2001
        'Author: Eric Pena
        'Description:   Retrieves records from the tblTransactionType table that providers may post under utilizing a stored procedure.
        'Parameters:    blnIncludeDisabled- determines if disabled records will be returned.
        'Returns: ADODB.Recordset
        '-------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim rstSQL As ADODB.Recordset



        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        cmdSQL.ActiveConnection = cnnSQL

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspSelTxTypeForProv"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ShowDisabled", adVarChar, adParamInput, 1, IIf(blnIncludeDisabled = True, "Y", "N")))
        End With

        'Execute the stored procedure
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        FetchProvPostings = rstSQL

        'Disconnect the recordset
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        rstSQL.ActiveConnection = Nothing
        rstSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing enviroment.
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        rstSQL.ActiveConnection = Nothing
        rstSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()
        'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME)

    End Function
    Public Function FetchCharges(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '-------------------------------------------------------------------------------------
        'Date: 02/19/2001
        'Author: Eric Pena
        'Description:   Retrieves records from the tblTransactionType table that providers may post under utilizing a stored procedure.
        'Parameters:    blnIncludeDisabled- determines if disabled records will be returned.
        'Returns: ADODB.Recordset
        '-------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim rstSQL As ADODB.Recordset



        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        cmdSQL.ActiveConnection = cnnSQL

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspSelTxTypeProvCharges"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ShowDisabled", adVarChar, adParamInput, 1, IIf(blnIncludeDisabled = True, "Y", "N")))
        End With

        'Execute the stored procedure
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        FetchCharges = rstSQL

        'Disconnect the recordset
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        rstSQL.ActiveConnection = Nothing
        rstSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing enviroment.
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        rstSQL.ActiveConnection = Nothing
        rstSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()
        'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME)

    End Function

    Public Function FetchAdjustments(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '-------------------------------------------------------------------------------------
        'Date: 06/23/2003
        'Author: Dave Richkun
        'Description:   Retrieves adjustments transactions from tblTransactionType
        'Parameters:    blnIncludeDisabled- determines if disabled records will be returned.
        'Returns: ADODB.Recordset
        '-------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim rstSQL As ADODB.Recordset



        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        cmdSQL.ActiveConnection = cnnSQL

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspSelTxTypeProvAdjusts"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ShowDisabled", adVarChar, adParamInput, 1, IIf(blnIncludeDisabled = True, "Y", "N")))
        End With

        'Execute the stored procedure
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        FetchAdjustments = rstSQL

        'Disconnect the recordset
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        rstSQL.ActiveConnection = Nothing
        rstSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing enviroment.
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        rstSQL.ActiveConnection = Nothing
        rstSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()
        'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME)

    End Function

End Class
