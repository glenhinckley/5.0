
'--------------------------------------------------------------------
'Class Name: CCheckLogDB
'Date: 12/28/2004
'Author: Duane C Orth
'Denis J Finkel
'Description:  MTS object designed to host methods associated with
'              data affecting the tblPayerCheckLog table.
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

Imports Psyquel.BusinessRules.CoreLibraryIII.dbStuff

Public Class CCheckLogDB
    Dim ds As New db
    Private _ConnectionString As String = String.Empty
    Private Const CLASS_NAME As String = "CCheckLogDB"
    Private Const TABLE_NAME As String = "tblPayerCheckLog"
    Private Const CONST_NO_DATE As Date = #12:00:00 AM#



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
        'Date: 12/28/2004
        'Author: Duane C Orth
        'Description:  Retrieves records from the tblPayerCheckLog table.
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
            .CommandText = "uspSelPayerCheckLog"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ShowDisabledYN", adVarChar, adParamInput, 1, IIf(blnIncludeDisabled = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@Where", adVarChar, adParamInput, 250, strWhere))
            .Parameters.Append(.CreateParameter("@Order", adVarChar, adParamInput, 25, strOrderBy))
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


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        rst = Nothing
        cmd = Nothing
        cnn = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function

    Public Function Insert(ByVal lngCheckID As Long, _
                           ByVal intInsuranceID As Long, ByVal intProviderID As Long, _
                           ByVal intPatientID As Long, ByVal intEncounterLogID As Long, _
                           ByVal lngEncDetailID As Long, ByVal lngCPTRecordID As Long, _
                           ByVal blnPost As Boolean, ByVal dtePostDate As Date, _
                           ByVal curPayment As Decimal, ByVal intPmtTxTypeID As Long, _
                           ByVal curDisallowance As Decimal, ByVal intDisTxTypeID As Long, _
                           ByVal curDeductable As Decimal, ByVal intDedTxTypeID As Long, _
                           ByVal curCoInsurance As Decimal, ByVal intCoInsTxTypeID As Long, _
                           ByVal curRefund As Decimal, ByVal intRefTxTypeID As Long, _
                           ByVal curDenial As Decimal, ByVal intDenTxTypeID As Long, _
                           ByVal strNotes As String, ByVal intNoteTxTypeID As Long, _
                           ByVal blnPrintHcfa As Boolean, ByVal intPlanID As Long, _
                           ByVal strAddedBy As String) As Long
        '--------------------------------------------------------------------
        'Date: 12/28/2004
        'Author: Duane C Orth
        'Description:  Inserts a row into the tblPayerCheckLog table utilizing
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
            .CommandText = "uspInsPayerCheckLog"
            .CommandType = adCmdStoredProc

            .Parameters.Append(.CreateParameter("@PayerCheckLogID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@CheckID", adInteger, adParamInput, , lngCheckID))
            .Parameters.Append(.CreateParameter("@InsuranceID", adInteger, adParamInput, , intInsuranceID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , intProviderID))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , intPatientID))
            .Parameters.Append(.CreateParameter("@EncounterLogID", adInteger, adParamInput, , intEncounterLogID))
            .Parameters.Append(.CreateParameter("@EncDetailID", adInteger, adParamInput, , lngEncDetailID))
            .Parameters.Append(.CreateParameter("@CPTRecordID", adInteger, adParamInput, , lngCPTRecordID))
            .Parameters.Append(.CreateParameter("@PostYN", adVarChar, adParamInput, 1, IIf(blnPost = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@PostDate", adDBTimeStamp, adParamInput, , IIf(dtePostDate.ToOADate() = 0, DBNull.Value, IIf(dtePostDate = CONST_NO_DATE, DBNull.Value, dtePostDate))))
            .Parameters.Append(.CreateParameter("@Payment", adDecimal, adParamInput, , curPayment))
            .Parameters.Append(.CreateParameter("@PmtTxTypeID", adInteger, adParamInput, , intPmtTxTypeID))
            .Parameters.Append(.CreateParameter("@Disallowance", adDecimal, adParamInput, , curDisallowance))
            .Parameters.Append(.CreateParameter("@DisTxTypeID", adInteger, adParamInput, , intDisTxTypeID))
            .Parameters.Append(.CreateParameter("@Deductable", adDecimal, adParamInput, , curDeductable))
            .Parameters.Append(.CreateParameter("@DedTxTypeID", adInteger, adParamInput, , intDedTxTypeID))
            '    .Parameters.Append .CreateParameter("@CoInsurance", adDecimal, adParamInput, , curCoInsurance)
            '    .Parameters.Append .CreateParameter("@CoInsTxTypeID", adInteger, adParamInput, , intCoInsTxTypeID)
            .Parameters.Append(.CreateParameter("@Refund", adDecimal, adParamInput, , curRefund))
            .Parameters.Append(.CreateParameter("@RefTxTypeID", adInteger, adParamInput, , intRefTxTypeID))
            .Parameters.Append(.CreateParameter("@Denial", adDecimal, adParamInput, , curDenial))
            .Parameters.Append(.CreateParameter("@DenTxTypeID", adInteger, adParamInput, , intDenTxTypeID))
            .Parameters.Append(.CreateParameter("@Notes", adVarChar, adParamInput, 512, strNotes))
            .Parameters.Append(.CreateParameter("@NoteTxTypeID", adInteger, adParamInput, , intNoteTxTypeID))
            .Parameters.Append(.CreateParameter("@PrintHcfaYN", adVarChar, adParamInput, 1, IIf(blnPrintHcfa = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , intPlanID))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strAddedBy))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))

        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        Insert = cmdSQL.Parameters("@PayerCheckLogID").Value

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
                           ByVal lngCheckID As Long, _
                           ByVal intInsuranceID As Long, ByVal intProviderID As Long, _
                           ByVal intPatientID As Long, ByVal intEncounterLogID As Long, _
                           ByVal lngEncDetailID As Long, ByVal lngCPTRecordID As Long, _
                           ByVal blnPost As Boolean, ByVal dtePostDate As Date, _
                           ByVal curPayment As Decimal, ByVal intPmtTxTypeID As Long, _
                           ByVal curDisallowance As Decimal, ByVal intDisTxTypeID As Long, _
                           ByVal curDeductable As Decimal, ByVal intDedTxTypeID As Long, _
                           ByVal curCoInsurance As Decimal, ByVal intCoInsTxTypeID As Long, _
                           ByVal curRefund As Decimal, ByVal intRefTxTypeID As Long, _
                           ByVal curDenial As Decimal, ByVal intDenTxTypeID As Long, _
                           ByVal strNotes As String, ByVal intNoteTxTypeID As Long, _
                           ByVal blnPrintHcfa As Boolean, ByVal intPlanID As Long)
        '--------------------------------------------------------------------
        'Date: 12/28/2004
        'Author: Duane C Orth
        'Description:  Updates a row in the tblPayerCheckLog table utilizing
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
            .CommandText = "uspUpdPayerCheckLog"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PayerCheckLogID", adInteger, adParamInput, , lngID))
            .Parameters.Append(.CreateParameter("@CheckID", adInteger, adParamInput, , lngCheckID))
            .Parameters.Append(.CreateParameter("@InsuranceID", adInteger, adParamInput, , intInsuranceID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , intProviderID))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , intPatientID))
            .Parameters.Append(.CreateParameter("@EncounterLogID", adInteger, adParamInput, , intEncounterLogID))
            .Parameters.Append(.CreateParameter("@EncDetailID", adInteger, adParamInput, , lngEncDetailID))
            .Parameters.Append(.CreateParameter("@CPTRecordID", adInteger, adParamInput, , lngCPTRecordID))
            .Parameters.Append(.CreateParameter("@PostYN", adVarChar, adParamInput, 1, IIf(blnPost = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@PostDate", adDBTimeStamp, adParamInput, , IIf(dtePostDate.ToOADate() = 0, DBNull.Value, IIf(dtePostDate = CONST_NO_DATE, DBNull.Value, dtePostDate))))
            .Parameters.Append(.CreateParameter("@Payment", adDecimal, adParamInput, , curPayment))
            .Parameters.Append(.CreateParameter("@PmtTxTypeID", adInteger, adParamInput, , intPmtTxTypeID))
            .Parameters.Append(.CreateParameter("@Disallowance", adDecimal, adParamInput, , curDisallowance))
            .Parameters.Append(.CreateParameter("@DisTxTypeID", adInteger, adParamInput, , intDisTxTypeID))
            .Parameters.Append(.CreateParameter("@Deductable", adDecimal, adParamInput, , curDeductable))
            .Parameters.Append(.CreateParameter("@DedTxTypeID", adInteger, adParamInput, , intDedTxTypeID))
            '     .Parameters.Append .CreateParameter("@CoInsurance", adDecimal, adParamInput, , curCoInsurance)
            '     .Parameters.Append .CreateParameter("@CoInsTxTypeID", adInteger, adParamInput, , intCoInsTxTypeID)
            .Parameters.Append(.CreateParameter("@Refund", adDecimal, adParamInput, , curRefund))
            .Parameters.Append(.CreateParameter("@RefTxTypeID", adInteger, adParamInput, , intRefTxTypeID))
            .Parameters.Append(.CreateParameter("@Denial", adDecimal, adParamInput, , curDenial))
            .Parameters.Append(.CreateParameter("@DenTxTypeID", adInteger, adParamInput, , intDenTxTypeID))
            .Parameters.Append(.CreateParameter("@Notes", adVarChar, adParamInput, 512, strNotes))
            .Parameters.Append(.CreateParameter("@NoteTxTypeID", adInteger, adParamInput, , intNoteTxTypeID))
            .Parameters.Append(.CreateParameter("@PrintHcfaYN", adVarChar, adParamInput, 1, IIf(blnPrintHcfa = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , intPlanID))
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
    Public Sub UpdatePosting(ByVal lngID As Long, _
                           ByVal lngCheckID As Long, _
                           ByVal blnPost As Boolean, _
                           ByVal dtePostDate As Date)
        '--------------------------------------------------------------------
        'Date: 12/28/2004
        'Author: Duane C Orth
        'Description:  Updates a row in the tblPayerCheckLog table utilizing
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
            .CommandText = "uspUpdPayerCheckLogPosting"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PayerCheckLogID", adInteger, adParamInput, , lngID))
            .Parameters.Append(.CreateParameter("@CheckID", adInteger, adParamInput, , lngCheckID))
            .Parameters.Append(.CreateParameter("@PostYN", adVarChar, adParamInput, 1, IIf(blnPost = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@PostDate", adDBTimeStamp, adParamInput, , IIf(dtePostDate.ToOADate() = 0, DBNull.Value, IIf(dtePostDate = CONST_NO_DATE, DBNull.Value, dtePostDate))))
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
    Public Sub Deleted(ByVal blnDeleted As Boolean, _
                        ByVal lngID As Long, _
                        ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 12/28/2004
        'Author: Duane C Orth
        'Description:  Flags a row in the tblPayerCheckLog table marking the row as
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
            strSQL = strSQL & "fldPayerCheckLogID = " & lngID
        Else
            blnFlag = "N"
            'Prepare the SQL statement
            strSQL = "UPDATE "
            strSQL = strSQL & TABLE_NAME
            strSQL = strSQL & " SET "
            strSQL = strSQL & " fldDisabledYN = '" & blnFlag & "' "
            strSQL = strSQL & "WHERE "
            strSQL = strSQL & "fldPayerCheckLogID = " & lngID
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
    Public Function Exists(ByVal strDescription As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 12/28/2004
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
        strSQL = strSQL & " fldDescription = '" & ds.ParseSQL(strDescription) & "' "

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

End Class

