
'--------------------------------------------------------------------
'Class Name: CInsuranceDB
'Date: 02/24/2000
'Author: Eric Pena
'Denis J Finkel
'Description:  MTS object designed to host methods associated with
'              data affecting the tblInsurance  table.
'--------------------------------------------------------------------
'  R001 07/06/2000 Richkun: Added FetchByName() method
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
Imports Psyquel.BusinessRules.CoreLibraryIII.dbStuff


Public Class CInsuranceDB

    Private Const CLASS_NAME As String = "CInsuranceDB"
    Private Const TABLE_NAME As String = "tblInsCompany"
    Dim ds As New db

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






    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False, Optional ByVal strWhere As String = "", Optional ByVal strOrderBy As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 02/24/2000
        'Author: Eric Pena
        'Description:  Retrieves records from the tblInsurance  table.
        'Parameters: blnIncludeDisabled - Optional parameter that identifies if
        '               records flagged as 'Disabled' or 'De-activated' are to be
        '               included in the record set. The default value is False.
        '            strWhere - Optional 'Where' clause of the SQL statement by
        '               which records will be filtered.  If the parameter is not
        '               supplied, all records will be retrieved.
        '             strOrderBy - Optional 'Order By' clause in which retrieved
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
        strSQL = strSQL & TABLE_NAME & ".fldInsuranceID, "
        strSQL = strSQL & TABLE_NAME & ".fldInsName, "
        strSQL = strSQL & TABLE_NAME & ".fldPayerCode, "
        strSQL = strSQL & TABLE_NAME & ".fldSendElectYN, "
        strSQL = strSQL & TABLE_NAME & ".fldEClaimEnrollYN, "
        strSQL = strSQL & TABLE_NAME & ".fldNotes, "
        strSQL = strSQL & TABLE_NAME & ".fldDateAdded, "
        strSQL = strSQL & TABLE_NAME & ".fldAddedBy, "
        strSQL = strSQL & TABLE_NAME & ".fldDisabledYN, "
        strSQL = strSQL & TABLE_NAME & ".fldDateDisabled, "
        strSQL = strSQL & TABLE_NAME & ".fldDisabledBy "
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

    Public Function FetchByName(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/06/2000
        'Author: Dave Richkun
        'Description:  Retrieves records from the tblInsurance table utilizing
        '              a stored procedure.
        'Parameters: None
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
            .CommandText = "uspSelInsCompany"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ShowDisabledYN", adVarChar, adParamInput, 1, IIf(blnIncludeDisabled = False, "N", "Y")))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByName = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        rst = Nothing
        cmd = Nothing
        cnn = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function


    Public Function Insert(ByVal strName As String, ByVal strPayerCode As String, _
                           ByVal blnElectYN As Boolean, ByVal blnEnrollYN As Boolean, _
                           ByVal strNotes As String, ByVal strUserName As String) As Long
        '--------------------------------------------------------------------
        'Date: 02/24/2000
        'Author: Eric Pena
        'Description:  Inserts a row into the tblInsurance table utilizing
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
            .CommandText = "uspInsInsCompany"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@InsuranceID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@InsName", adVarChar, adParamInput, 45, strName))
            .Parameters.Append(.CreateParameter("@PayerCode", adVarChar, adParamInput, 20, strPayerCode))
            .Parameters.Append(.CreateParameter("@ElectYN", adVarChar, adParamInput, 1, IIf(blnElectYN = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@EnrollYN", adVarChar, adParamInput, 1, IIf(blnEnrollYN = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@Notes", adVarChar, adParamInput, 255, strNotes))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 20, strUserName))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        Insert = cmdSQL.Parameters("@InsuranceID").Value

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

    Public Sub Update(ByVal lngID As Long, ByVal strName As String, _
                      ByVal strPayerCode As String, ByVal blnElectYN As Boolean, _
                      ByVal blnEnrollYN As Boolean, ByVal strNotes As String)
        '--------------------------------------------------------------------
        'Date: 02/24/2000
        'Author: Eric Pena
        'Description:  Updates a row in the tblInsurance  table utilizing
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
            .CommandText = "uspUpdInsCompany"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@InsuranceID", adInteger, adParamInput, , lngID))
            .Parameters.Append(.CreateParameter("@InsName", adVarChar, adParamInput, 45, strName))
            .Parameters.Append(.CreateParameter("@PayerCode", adVarChar, adParamInput, 20, strPayerCode))
            .Parameters.Append(.CreateParameter("@ElectYN", adVarChar, adParamInput, 1, IIf(blnElectYN = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@EnrollYN", adVarChar, adParamInput, 1, IIf(blnEnrollYN = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@Notes", adVarChar, adParamInput, 255, strNotes))
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
        'Description:  Flags a row in the tblInsurance  table marking the row as
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
            strSQL = strSQL & "fldInsuranceID = " & lngID
        Else
            blnFlag = "N"
            'Prepare the SQL statement
            strSQL = "UPDATE "
            strSQL = strSQL & TABLE_NAME
            strSQL = strSQL & " SET "
            strSQL = strSQL & " fldDisabledYN = '" & blnFlag & "' "
            strSQL = strSQL & "WHERE "
            strSQL = strSQL & "fldInsuranceID = " & lngID
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
        'Description:  Determines if an Insurance Name identical to the
        '              strName Name parameter already exists in the table.
        'Parameters: strName - Business name to be checked
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
        strSQL = strSQL & " fldInsName = '" & ds.ParseSQL(strName) & "' "

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

    Public Function FetchByPlan(ByVal lngPlanID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/06/2001
        'Author: Eric Pena
        'Description:  Retrieves records from the tblInsurance table utilizing
        '              a stored procedure.
        'Parameters: None
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
            .CommandText = "uspSelInsByPlan"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID))
        End With

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByPlan = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        rst = Nothing
        cmd = Nothing
        cnn = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function
    Public Function FetchByProvider(ByVal lngProviderID As Long, ByVal lngInsuranceID As Long, _
                                    Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 04/21/2006
        'Author: Duane C Orth
        'Description:  Retrieves records from the tblInsurance table utilizing
        '              a stored procedure.
        'Parameters: None
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
            .CommandText = "uspSelPatientByIns"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@InsuranceID", adInteger, adParamInput, , lngInsuranceID))
            .Parameters.Append(.CreateParameter("@ShowDisabled", adVarChar, adParamInput, 1, IIf(blnIncludeDisabled = False, "N", "Y")))
        End With

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByProvider = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        rst = Nothing
        cmd = Nothing
        cnn = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function
    Public Function FetchByGroup(ByVal lngGroupID As Long, ByVal lngInsuranceID As Long, _
                                    Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 04/21/2006
        'Author: Duane C Orth
        'Description:  Retrieves records from the tblInsurance table utilizing
        '              a stored procedure.
        'Parameters: None
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
            .CommandText = "uspSelPatientByGroupByIns"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@GroupID", adInteger, adParamInput, , lngGroupID))
            .Parameters.Append(.CreateParameter("@InsuranceID", adInteger, adParamInput, , lngInsuranceID))
            .Parameters.Append(.CreateParameter("@ShowDisabled", adVarChar, adParamInput, 1, IIf(blnIncludeDisabled = False, "N", "Y")))
        End With

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByGroup = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        rst = Nothing
        cmd = Nothing
        cnn = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function
    Public Function FetchByBR(ByVal lngBRID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/06/2001
        'Author: Eric Pena
        'Description:  Retrieves records from the tblInsurance table utilizing
        '              a stored procedure.
        'Parameters: None
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
            .CommandText = "uspSelInsByBR"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@BRID", adInteger, adParamInput, , lngBRID))
        End With

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByBR = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        rst = Nothing
        cmd = Nothing
        cnn = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function

End Class

