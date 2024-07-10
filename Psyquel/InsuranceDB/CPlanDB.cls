
'--------------------------------------------------------------------
'Class Name: md.md.NumbersOnly(
'Date: 02/24/2000
'Author: Eric Pena
'Denis J Finkel
'Description:  MTS object designed to host methods associated with
'              data affecting the tblPlan table.
'--------------------------------------------------------------------
'  R001 07/05/2000 Richkun: Altered Fetch() method to use stored procedure
'  R002 07/06/2000 Richkun: Added SearchBy() method
'  R003 03/14/2001 Richkun: Altered Filter() method to exclude Plans whose
'           Insurance Company records are disabled.
'  R004 03/26/2001 Nichol: Added optional parameter lngMaxRows to Filter()
'           and incorporated it into the query, to only get that many rows.
'  R005 05/02/2001 Richkun: Added optional strEAP parameter to Filter()
'--------------------------------------------------------------------

Option Explicit On
Imports Psyquel.BusinessRules.CoreLibraryIII.dbStuff

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


Public Class CplanDB







    Dim md As New ModCommon
    Dim ds As New db
    Private Const INS_CO_EAP As Long = 103
    Private Const CLASS_NAME As String = "md.md.NumbersOnly("
    Private Const TABLE_NAME As String = "tblPlan"



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





    Public Enum SearchColumn 'R002
        srcInsCompanyID = 1
        SrcPlanName = 2
        SrcCity = 3
        SrcState = 4
        SrcZip = 5
        SrcPOBox = 6
        SrcPayerID = 7
    End Enum


    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 02/24/2000
        'Author: Eric Pena
        'Description:  Retrieves records from the tblPlan table.
        'Parameters: blnIncludeDisabled - Optional parameter that identifies if
        '               records flagged as ' or 'De-activated' are to be
        '               included in the record set. The default value is False.
        '            strWhere - Optional 'Where' clause of the SQL statement by
        '               which records will be filtered.  If the parameter is not
        '               supplied, all records will be retrieved.
        '             strOrderBy - Optional 'Order By' clause in which retrieved
        '               records will be sorted.  The default is set to the
        '               'fldLast' and 'fldFirst' columns
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim cnn As ADODB.Connection
        Dim rst As ADODB.Recordset
        Dim cmd As ADODB.Command





        rst = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelPlanByName"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ShowDisabledYN", adVarChar, adParamInput, 1, IIf(blnIncludeDisabled = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Function
        End If

        Fetch = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion




    End Function


    Public Function Filter(ByVal intColumn As SearchColumn, ByVal strValue As String, _
                           ByVal intSortOrder As SearchColumn, Optional ByVal strEAP As String = "N", _
                           Optional ByVal lngMaxRows As Long = 101) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/06/2000
        'Author: Dave Richkun
        'Description:  Retrieves records from the tblPlan table filtering records
        '              using the SearchColumn enumerator parameter.
        'Parameters: intColumn - An enumerator that identifies the record filter
        '               column that will be applied to the resultset
        '            strValue - Identifies the value that will be searched for - used
        '               in conjunction with the intColumn parameter
        '            intSortOrder - Identifies the order in which the results are
        '               sorted.
        '        Eg.) Filter(srcState, "TX", srcCity) will return all Plan records
        '             located within the state of Texas, ordered by City
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '  R002 Created
        '--------------------------------------------------------------------


        Dim cnn As ADODB.Connection
        Dim rst As ADODB.Recordset
        Dim strSQL As String



        strSQL = "SELECT TOP " & CStr(lngMaxRows) & " "
        strSQL = strSQL & "A.*, "
        strSQL = strSQL & "B.fldCPCName, "
        strSQL = strSQL & "B.fldAddress1, "
        strSQL = strSQL & "B.fldAddress2, "
        strSQL = strSQL & "B.fldCity, "
        strSQL = strSQL & "B.fldState, "
        strSQL = strSQL & "B.fldZip, "
        strSQL = strSQL & "B.fldPhone, "
        strSQL = strSQL & "B.fldExt, "
        strSQL = strSQL & "B.fldContact, "
        strSQL = strSQL & "B.fldPayerCode, "
        strSQL = strSQL & "C.fldInsuranceID, "
        strSQL = strSQL & "C.fldInsName, "
        strSQL = strSQL & "E.fldPlanClass "
        strSQL = strSQL & "FROM "
        strSQL = strSQL & "tblPlan AS A INNER JOIN "
        strSQL = strSQL & "tblClaimsProcCtr AS B ON A.fldCPCID = B.fldCPCID INNER JOIN "
        strSQL = strSQL & "tblInsCompany AS C ON B.fldInsuranceID = C.fldInsuranceID INNER JOIN "
        strSQL = strSQL & "tblState AS D ON B.fldState = D.fldStateCode INNER JOIN "
        strSQL = strSQL & "tblPlanClassification AS E ON A.fldPlanClassID = E.fldPlanClassID "
        strSQL = strSQL & "WHERE "
        strSQL = strSQL & "A.fldDisabledYN = 'N' AND "
        strSQL = strSQL & "C.fldDisabledYN = 'N' AND " 'R003

        'R005
        If strEAP = "Y" Then
            strSQL = strSQL & "C.fldInsuranceID = " & INS_CO_EAP & " AND "
        Else
            strSQL = strSQL & "C.fldInsuranceID <> " & INS_CO_EAP & " AND "
        End If

        'Insert filter condition
        Select Case intColumn
            Case 1
                strSQL = strSQL & "C.fldInsuranceID = " & strValue
            Case 2
                strSQL = strSQL & "A.fldPlanName LIKE '" & ds.ParseSQL(strValue) & "%'"
            Case 3
                strSQL = strSQL & "B.fldCity LIKE '" & ds.ParseSQL(strValue) & "%'"
            Case 4
                If Len(Trim(strValue)) = 2 Then
                    strSQL = strSQL & "B.fldState = '" & ds.ParseSQL(strValue) & "'"
                Else
                    strSQL = strSQL & "D.fldStateName LIKE '" & ds.ParseSQL(strValue) & "%'"
                End If
            Case 5
                strSQL = strSQL & "B.fldZip LIKE '" & md.NumbersOnly(strValue) & "%'"
            Case 6
                strSQL = strSQL & "B.fldAddress1 LIKE '%Box " & strValue & "%'"
            Case 7
                strSQL = strSQL & "B.fldPayerCode LIKE '%" & strValue & "%'"
        End Select

        strSQL = strSQL & " ORDER BY "
        'Insert order condition
        Select Case intSortOrder
            Case 1
                strSQL = strSQL & "fldCPCName, "
                strSQL = strSQL & "fldState, "
                strSQL = strSQL & "fldCity, "
                strSQL = strSQL & "fldZip "
            Case 2
                strSQL = strSQL & "fldCPCName, "
                strSQL = strSQL & "fldState, "
                strSQL = strSQL & "fldCity, "
                strSQL = strSQL & "fldZip "
            Case 3
                strSQL = strSQL & "fldCity, "
                strSQL = strSQL & "fldZip, "
                strSQL = strSQL & "fldState, "
                strSQL = strSQL & "fldCPCName "
            Case 4
                strSQL = strSQL & "fldState, "
                strSQL = strSQL & "fldCity, "
                strSQL = strSQL & "fldZip, "
                strSQL = strSQL & "fldCPCName "
            Case 5
                strSQL = strSQL & "fldZip, "
                strSQL = strSQL & "fldCity, "
                strSQL = strSQL & "fldState, "
                strSQL = strSQL & "fldCPCName "
            Case 6
                strSQL = strSQL & "fldAddress1, "
                strSQL = strSQL & "fldCPCName, "
                strSQL = strSQL & "fldState, "
                strSQL = strSQL & "fldCity "
            Case 7
                strSQL = strSQL & "fldCPCName, "
                strSQL = strSQL & "fldAddress1, "
                strSQL = strSQL & "fldState, "
                strSQL = strSQL & "fldCity "
        End Select

        rst = New ADODB.Recordset
        cnn = New ADODB.Connection

        'Acquire the database connection and run the query
        cnn.Open(_ConnectionString)
        rst.CursorLocation = adUseClient
        rst.Open(strSQL, cnn, adOpenForwardOnly, adLockReadOnly)

        Filter = rst

        'Disconnect the recordset
        rst.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion




    End Function

    Public Function FetchByID(ByVal lngPlanID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/05/2000
        'Author: Dave Richkun
        'Description:  Retrieves a specific record from the tblPlan table.
        'Parameters: lngPlanID - ID of the Plan to retrieve detail information for
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim cnn As ADODB.Connection
        Dim rst As ADODB.Recordset
        Dim cmd As ADODB.Command





        rst = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelPlanByID"
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID))
            .CommandType = adCmdStoredProc
        End With

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByID = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion



    End Function
    Public Function Insert(ByVal strName As String, ByVal lngPlanClassID As Long, _
                   ByVal lngCPCID As Long, ByVal strNotes As String, _
                   ByVal lngFileDeadline As Long, ByVal strAddedBy As String) As Long
        '--------------------------------------------------------------------
        'Date: 02/24/2000
        'Author: Eric Pena
        'Description:  Inserts a row into the tblPlan table utilizing
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
            .CommandText = "uspInsPlan"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@CPCID", adInteger, adParamInput, , lngCPCID))
            .Parameters.Append(.CreateParameter("@PlanName", adVarChar, adParamInput, 30, strName))
            .Parameters.Append(.CreateParameter("@PlanClassID", adInteger, adParamInput, , lngPlanClassID))
            .Parameters.Append(.CreateParameter("@Notes", adVarChar, adParamInput, 255, strNotes))
            .Parameters.Append(.CreateParameter("@FileDeadline", adInteger, adParamInput, , lngFileDeadline))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 20, strAddedBy))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        Insert = cmdSQL.Parameters("@PlanID").Value

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



    End Function
    Public Sub Update(ByVal lngID As Long, ByVal strName As String, _
                ByVal lngPlanClassID As Long, ByVal lngCPCID As Long, _
                ByVal strNotes As String, ByVal lngFileDeadline As Long)
        '--------------------------------------------------------------------
        'Date: 02/24/2000
        'Author: Eric Pena
        'Description:  Updates a row in the tblPlan table utilizing
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
            .CommandText = "uspUpdPlan"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , lngID))
            .Parameters.Append(.CreateParameter("@CPCID", adInteger, adParamInput, , lngCPCID))
            .Parameters.Append(.CreateParameter("@PlanName", adVarChar, adParamInput, 30, strName))
            .Parameters.Append(.CreateParameter("@PlanClassID", adInteger, adParamInput, , lngPlanClassID))
            .Parameters.Append(.CreateParameter("@Notes", adVarChar, adParamInput, 255, strNotes))
            .Parameters.Append(.CreateParameter("@FileDeadline", adInteger, adParamInput, , lngFileDeadline))
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


        Exit Sub


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()
        'RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, cmdSQL.Parameters("@ErrorMsg").Value)

    End Sub
    Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 02/24/2000
        'Author: Eric Pena
        'Description:  Flags a row in the tblPlan table marking the row as
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
            strSQL = strSQL & "fldPlanID = " & lngID
        Else
            blnFlag = "N"
            'Prepare the SQL statement
            strSQL = "UPDATE "
            strSQL = strSQL & TABLE_NAME
            strSQL = strSQL & " SET "
            strSQL = strSQL & " fldDisabledYN = '" & blnFlag & "' "
            strSQL = strSQL & "WHERE "
            strSQL = strSQL & "fldPlanID = " & lngID
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


        Exit Sub


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Sub
    Public Function Exists(ByVal strName As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 02/24/2000
        'Author: Eric Pena
        'Description:  Determines if a Plan Name identical to the
        '              strName Name parameter already exists in the table.
        'Parameters: strName - Plan name to be checked
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
        strSQL = strSQL & " fldName = '" & ds.ParseSQL(strName) & "' "

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


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        rstSQL = Nothing
        cnnSQL = Nothing

        'cmdSQL.Execute()
        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function
    Public Function FetchByName(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 3/08/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblPlan table ordered by the plan name utilizing a stored procedure
        'Parameters: blnIncludeDisabled - determines if disabled records will be returned
        'Returns: ADORst
        '--------------------------------------------------------------------
        Dim rs As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        rs = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn
        'Create the parameter objects
        With cmd
            .CommandText = "uspSelPlanByName"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ShowDisabled", adVarChar, adParamInput, 1, IIf(blnIncludeDisabled = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByName = rs

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rs.ActiveConnection = Nothing
        cnn = Nothing
        'Signal successful completion


        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rs.ActiveConnection = Nothing
        rs = Nothing
        cnn = Nothing
        'cmdSQL.Execute()
        'RaiseError(Err, cmd.Parameters("@ErrorNum").Value, CLASS_NAME)
    End Function
    Public Function FetchByZip(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 3/08/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblPlan table ordered by the CPC's zip utilizing a stored procedure
        'Parameters: blnIncludeDisabled - determines if disabled records will be returned
        'Returns: ADORst
        '--------------------------------------------------------------------
        Dim rs As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        rs = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn
        'Create the parameter objects
        With cmd
            .CommandText = "uspSelPlanByZip"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ShowDisabled", adVarChar, adParamInput, 1, IIf(blnIncludeDisabled = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByZip = rs

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rs.ActiveConnection = Nothing
        cnn = Nothing
        'Signal successful completion


        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rs.ActiveConnection = Nothing
        rs = Nothing
        cnn = Nothing
        'cmdSQL.Execute()
        'RaiseError(Err, cmd.Parameters("@ErrorNum").Value, CLASS_NAME)
    End Function
    Public Function FetchByCity(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 3/08/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblPlan table ordered by the CPC's city utilizing a stored procedure
        'Parameters: blnIncludeDisabled - determines if disabled records will be returned
        'Returns: ADORst
        '--------------------------------------------------------------------
        Dim rs As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        rs = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn
        'Create the parameter objects
        With cmd
            .CommandText = "uspSelPlanByCity"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ShowDisabled", adVarChar, adParamInput, 1, IIf(blnIncludeDisabled = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByCity = rs

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rs.ActiveConnection = Nothing
        cnn = Nothing
        'Signal successful completion


        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rs.ActiveConnection = Nothing
        rs = Nothing
        cnn = Nothing
        'cmdSQL.Execute()
        'RaiseError(Err, cmd.Parameters("@ErrorNum").Value, CLASS_NAME)
    End Function
    Public Function FetchByState(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 3/08/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblPlan table ordered by the CPC's state utilizing a stored procedure
        'Parameters: blnIncludeDisabled - determines if disabled records will be returned
        'Returns: ADORst
        '--------------------------------------------------------------------
        Dim rs As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        rs = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn
        'Create the parameter objects
        With cmd
            .CommandText = "uspSelPlanByState"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ShowDisabled", adVarChar, adParamInput, 1, IIf(blnIncludeDisabled = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByState = rs

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rs.ActiveConnection = Nothing
        cnn = Nothing
        'Signal successful completion


        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rs.ActiveConnection = Nothing
        rs = Nothing
        cnn = Nothing
        'cmdSQL.Execute()
        'RaiseError(Err, cmd.Parameters("@ErrorNum").Value, CLASS_NAME)
    End Function
    Public Function FetchByIns(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 5/01/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblPlan, tblClaimsProcCtr, and tblInsCompany tables utilizing a stored procedure
        'Parameters: blnIncludeDisabled - determines if disabled records will be returned
        'Returns: ADORst
        '--------------------------------------------------------------------
        Dim rs As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        rs = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn
        'Create the parameter objects
        With cmd
            .CommandText = "uspSelPlanByIns"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ShowDisabled", adVarChar, adParamInput, 1, IIf(blnIncludeDisabled = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByIns = rs

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rs.ActiveConnection = Nothing
        cnn = Nothing
        'Signal successful completion


        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rs.ActiveConnection = Nothing
        rs = Nothing
        cnn = Nothing
        'cmdSQL.Execute()
        'RaiseError(Err, cmd.Parameters("@ErrorNum").Value, CLASS_NAME)
    End Function


End Class






