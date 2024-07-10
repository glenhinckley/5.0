
'--------------------------------------------------------------------
'Class Name: CProvPayoutDB
'Date: 09/29/2000
'Author: Eric Pena
'Denis J Finkel
'Description:  MTS object designed to host methods associated with data affecting the tblProviderPayout  table.
'--------------------------------------------------------------------
'Revision History:
'  R001: 01/09/2002 Richkun:  Added methods to support inserting of Provider Payments
'           to support new 'Provider Direct' offering
'  R002: 02/12/2003 - Richkun: Added InsertPayoutRows() to separate task from LockPayout() method
'-------------------------------------------------------------------
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

Public Class CProvPayoutDB
    Private _ConnectionString As String = String.Empty

    Private Const CLASS_NAME As String = "CProvPayoutDB"
    Private Const TABLE_NAME As String = "tblProviderPayout"

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property





    Public Function FetchPayoutData() As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/29/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblProviderPayout table ordered by patient name utilizing a stored procedure
        'Parameters: lngProviderID - id of the provider to return records for
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
            .CommandText = "uspSelProvPayData"
            .CommandType = adCmdStoredProc
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchPayoutData = rs

        'Disconnect the recordset
        cmd = Nothing
        cnn = Nothing
        rs.ActiveConnection = Nothing
        'Signal successful completion


        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        cmd = Nothing
        rs = Nothing
        cnn = Nothing
        'cmdSQL.Execute()
        'RaiseError(Err, Err.Number, CLASS_NAME)
    End Function

    Public Sub Delete(ByVal dteStart As Date)
        '--------------------------------------------------------------------
        'Date: 09/29/2000
        'Author: Eric Pena
        'Description:  Updates a row in the tblProviderPayout  table utilizing
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
            .CommandText = "uspDelProvPay"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@Start", adDBTimeStamp, adParamInput, , dteStart))
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
        'RaiseError(Err, Err.Number, CLASS_NAME, Err.Description)
    End Sub
    Public Function Insert(ByVal lngProviderID As Long, ByVal dteStart As Date, ByVal dteEnd As Date, ByVal curGrossAmt As Decimal) As Long
        '--------------------------------------------------------------------
        'Date: 09/29/2000
        'Author: Eric Pena
        'Description:  Inserts a row into the tblProviderPayout table utilizing
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
            .CommandText = "uspInsProvPay"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatPmtLogID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@DateStart", adDBTimeStamp, adParamInput, , dteStart))
            .Parameters.Append(.CreateParameter("@DateEnd", adDBTimeStamp, adParamInput, , dteEnd))
            .Parameters.Append(.CreateParameter("@GrossAmt", adDecimal, adParamInput, , curGrossAmt))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection

        cnnSQL.Open(_ConnectionString)
        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute() '()

        Insert = cmdSQL.Parameters("@PatPmtLogID").Value
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

    Public Sub SetProcessed(ByVal dteStart As Date, ByVal dteEnd As Date, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 09/29/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblProviderPayout table entered today by a specific user
        'Parameters: strUserName - login name of the user whose Encounter records are being retrieved.
        'Returns: Recordset of matching rows
        '--------------------------------------------------------------------
        Dim rst As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        cmd = New ADODB.Command
        'Create the parameter objects
        With cmd
            .CommandText = "uspSetProvPayProcessed"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@Start", adDBTimeStamp, adParamInput, , dteStart))
            .Parameters.Append(.CreateParameter("@End", adDBTimeStamp, adParamInput, , dteEnd))
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName))
        End With

        'Acquire the database connection.
        cnn = New ADODB.Connection
        cnn.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmd.ActiveConnection = cnn
        cmd.Execute()

        'Close the connection and free all resources
        cnn.Close()

        cmd = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Sub

        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        cmd = Nothing
        cnn = Nothing
        rst = Nothing
        'RaiseError(Err, Err.Number, CLASS_NAME)

    End Sub

    Public Function FetchDates(ByVal blnPayoutHistory As Boolean) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/29/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblProviderPayout table ordered by patient name utilizing a stored procedure
        'Parameters: lngProviderID - id of the provider to return records for
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
            .CommandText = "uspSelProvPayDate"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@HistoryView", adVarChar, adParamInput, 1, IIf(blnPayoutHistory, "Y", "N")))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchDates = rs

        'Disconnect the recordset
        cmd = Nothing
        cnn = Nothing
        rs.ActiveConnection = Nothing
        'Signal successful completion


        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        cmd = Nothing
        rs = Nothing
        cnn = Nothing
        'cmdSQL.Execute()
        'RaiseError(Err, Err.Number, CLASS_NAME)
    End Function
    Public Function FetchProviderByPayout(ByVal dtePayoutStart As Date, ByVal blnPayoutHistory As Boolean) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/29/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblProviderPayout table ordered by patient name utilizing a stored procedure
        'Parameters: lngProviderID - id of the provider to return records for
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
            .CommandText = "uspSelProvByPayDate"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PayoutStart", adDBTimeStamp, adParamInput, , IIf(dtePayoutStart.ToOADate() = 0, DBNull.Value, dtePayoutStart)))
            .Parameters.Append(.CreateParameter("@HistoryView", adVarChar, adParamInput, 1, IIf(blnPayoutHistory, "Y", "N")))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchProviderByPayout = rs

        'Disconnect the recordset
        cmd = Nothing
        cnn = Nothing
        rs.ActiveConnection = Nothing
        'Signal successful completion


        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        cmd = Nothing
        rs = Nothing
        cnn = Nothing
        'cmdSQL.Execute()
        'RaiseError(Err, Err.Number, CLASS_NAME)
    End Function
    Public Function isProcessed(ByVal lngID As Long, Optional ByVal dteStart As Date = Nothing, Optional ByVal dteEnd As Date = Nothing) As Boolean
        '--------------------------------------------------------------------
        'Date: 09/29/2000
        'Author: Eric Pena
        'Description:  Lets the er know if a given EncounterLog Entry has been processed
        'Parameters: lngID - The ID of the EL Entry
        'Returns: T if the association is processed, F otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim strSQL As String
        isProcessed = False



        'Prepare the SQL statement.
        strSQL = "SELECT fldProcessedYN "
        strSQL = strSQL & "FROM "
        strSQL = strSQL & "tblProviderPayout "
        strSQL = strSQL & " WHERE "
        If (dteStart.ToOADate() = 0 And dteEnd.ToOADate() = 0) Then
            strSQL = strSQL & "tblProviderPayout.fldProvPayID = " & lngID
        Else
            isProcessed = True
            strSQL = strSQL & "fldDateStart = '" & dteStart & "' AND fldDateEnd = '" & dteEnd & "' AND fldProcessedYN = 'N'"
        End If

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Instantiate and populate the Recordset.
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(strSQL, cnnSQL, adOpenForwardOnly, adLockReadOnly, adCmdText + adAsyncFetch)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rstSQL.ActiveConnection = Nothing
        cnnSQL.Close()
        cnnSQL = Nothing

        'set to null if there are no current associations (new Benefactor)
        If rstSQL.RecordCount = 0 Then
            GoTo EMPTY_RECORDSET
        End If
        If rstSQL.Fields("fldProcessedYN").Value = "Y" Then isProcessed = True Else isProcessed = False

EMPTY_RECORDSET:

        Exit Function

    End Function
    Public Function FetchByID(ByVal lngID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/29/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblProviderPayout table ordered by patient name utilizing a stored procedure
        'Parameters: lngProviderID - id of the provider to return records for
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
            .CommandText = "uspSelProvPayByID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamInput, , lngID))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByID = rs

        'Disconnect the recordset
        cmd = Nothing
        cnn = Nothing
        rs.ActiveConnection = Nothing
        'Signal successful completion


        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        cmd = Nothing
        rs = Nothing
        cnn = Nothing
        'cmdSQL.Execute()
        'RaiseError(Err, Err.Number, CLASS_NAME)
    End Function
    Public Function FetchByDate(ByVal dteStart As Date, ByVal dteEnd As Date) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/29/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblProviderPayout table ordered by provider name utilizing a stored procedure
        'Parameters: lngProviderID - id of the patient  to return records for
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
            .CommandText = "uspSelProvPayByDate"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@DateStart", adDBTimeStamp, adParamInput, , dteStart))
            .Parameters.Append(.CreateParameter("@DateEnd", adDBTimeStamp, adParamInput, , dteEnd))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByDate = rs

        'Disconnect the recordset
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
        'RaiseError(Err, Err.Number, CLASS_NAME)
    End Function

    Public Function AddDeduction(ByVal lngProvPayID As Long, ByVal strDescription As String, _
                                 ByVal curAmount As Decimal, ByVal lngProviderID As Long, _
                                 ByVal dtAddDate As Date, ByVal strAddedBy As String) As Long
        '--------------------------------------------------------------------
        'Date: 09/29/2000
        'Author: Eric Pena
        'Description:  Returns rows from tblProviderPayout determining current outstanding balances
        'Parameters:    lngpatientid - Patient ID for which to search balances, or -1 for all patients
        '                       lngproviderid - Provider ID for which to search balances, or -1 for all providers
        'Returns: ADORst
        '--------------------------------------------------------------------
        Dim cmdSQL As ADODB.Command
        Dim cnnSQL As ADODB.Connection



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspInsProvPayDeduct"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProvPayDedID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@ProvPayID", adInteger, adParamInput, , lngProvPayID))
            .Parameters.Append(.CreateParameter("@Description", adVarChar, adParamInput, 50, Left(strDescription, 50)))
            .Parameters.Append(.CreateParameter("@Payment", adDecimal, adParamInput, , 0))
            .Parameters.Append(.CreateParameter("@CollectFee", adDecimal, adParamInput, , 0))
            .Parameters.Append(.CreateParameter("@SubmitFee", adDecimal, adParamInput, , 0))
            .Parameters.Append(.CreateParameter("@OtherFee", adDecimal, adParamInput, , curAmount))
            .Parameters.Append(.CreateParameter("@PostDate", adDBTimeStamp, adParamInput, , Date.Now))
            .Parameters.Append(.CreateParameter("@AddDate", adDBTimeStamp, adParamInput, , IIf(dtAddDate.ToOADate() = 0, DBNull.Value, dtAddDate)))
            .Parameters.Append(.CreateParameter("@DOS", adDBTimeStamp, adParamInput, , DBNull.Value))
            .Parameters.Append(.CreateParameter("@CPTCode", adVarChar, adParamInput, 10, DBNull.Value))
            .Parameters.Append(.CreateParameter("@CPTRecordID", adInteger, adParamInput, , 0))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , DBNull.Value))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strAddedBy))
            .Parameters.Append(.CreateParameter("@TxID", adInteger, adParamInput, , DBNull.Value))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection

        cnnSQL.Open(_ConnectionString)
        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute() '()

        AddDeduction = cmdSQL.Parameters("@ProvPayDedID").Value

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
        'RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME)
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()
    End Function

    Public Sub UpdateDeduction(ByVal lngProvPayDeductID As Long, ByVal lngProvPayID As Long, _
                               ByVal strDescription As String, ByVal curAmount As Decimal, _
                               ByVal dtAddDate As Date, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 01/28/2003
        'Author: Dave Richkun
        'Description:  Updates a payout deduction
        'Parameters: lngProvPayDeductID - ID of the payout deduction to be updated
        '            lngProvPayID - ID of the payout to which the deduction is associated
        '            strDescription - Description of the deduction
        '            curAmount - Amount of the deduction
        '            dtAddDate - Date on which the transaction is added (may not be system date)
        '            strUserName - Name of the user performing the update
        'Returns: Null
        '--------------------------------------------------------------------
        Dim cmdSQL As ADODB.Command
        Dim cnnSQL As ADODB.Connection



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspUpdProvPayDeduct"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProvPayDedID", adInteger, adParamInput, , lngProvPayDeductID))
            .Parameters.Append(.CreateParameter("@ProvPayID", adInteger, adParamInput, , lngProvPayID))
            .Parameters.Append(.CreateParameter("@Description", adVarChar, adParamInput, 50, Left(strDescription, 50)))
            .Parameters.Append(.CreateParameter("@AddDate", adDBTimeStamp, adParamInput, , IIf(dtAddDate.ToOADate() = 0, DBNull.Value, dtAddDate)))
            .Parameters.Append(.CreateParameter("@OtherFee", adDecimal, adParamInput, , curAmount))
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection

        cnnSQL.Open(_ConnectionString)
        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute() '()

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
            Exit Sub
        End If

        'Close the connection
        cnnSQL.Close()

        'Free all resources
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Sub

        'Signal incompletion and raise the error to the ing environment.
        'RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME)
        cmdSQL = Nothing
        cnnSQL = Nothing
        '  cmdSQL.Execute()
    End Sub




    Public Sub RemoveDeduction(ByVal lngProvPayDedID As Long, ByVal lngProvPayID As Long, _
                               ByVal dblAmount As Double)
        '--------------------------------------------------------------------
        'Date: 01/13/2002
        'Author: Dave Richkun
        'Description:  Deletes a row from tblProviderPayoutDeduction and updates the amount
        '              in the parent table (tblProviderPayout) utilizing a stored procedure.
        'Parameters: lngProvPayDedID - ID of the row in tblProviderPayoutDeduction to be deleted
        '            lngProvPayID - ID of the parent row in tblProviderPayout
        '            dblAmount - Amount of the transaction being deleted
        'Returns: Null
        '--------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspDelProvPayDeduct"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProvPayDedID", adInteger, adParamInput, , lngProvPayDedID))
            .Parameters.Append(.CreateParameter("@ProvPayID", adInteger, adParamInput, , lngProvPayID))
            .Parameters.Append(.CreateParameter("@Amount", adDecimal, adParamInput, , dblAmount))
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
        'cmdSQL.Execute()
        'RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME)

    End Sub

    Public Function FetchDecutions(ByVal lngProvPayID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/29/2000
        'Author: Eric Pena
        'Description:  Returns rows from tblProviderPayout determining current outstanding balances
        'Parameters:    null
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
            .CommandText = "uspSelProvPayDeduct"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProvPayID", adInteger, adParamInput, , lngProvPayID))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchDecutions = rs

        'Disconnect the recordset
        cmd = Nothing
        rs.ActiveConnection = Nothing
        cnn = Nothing
        'Signal successful completion


        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        'RaiseError(Err, cmd.Parameters("@ErrorNum").Value, CLASS_NAME)
        cmd = Nothing
        rs = Nothing
        cnn = Nothing
        'cmdSQL.Execute()
    End Function
    Public Function Exists(ByVal lngProviderID As Long, ByVal dteStart As Date) As Boolean
        '--------------------------------------------------------------------
        'Date: 09/29/2000
        'Author: Eric Pena
        'Description:  Lets the er know if a given EncounterLog Entry has been processed
        'Parameters: lngID - The ID of the EL Entry
        'Returns: T if the association is processed, F otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim intCount As Integer


        Exists = False

        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspSelProvPayExists"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@Exists", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@DateStart", adDBTimeStamp, adParamInput, , dteStart))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection

        cnnSQL.Open(_ConnectionString)
        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute() '()

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
            Exit Function
        End If

        If (cmdSQL.Parameters("@Exists").Value > 0) Then Exists = True

        'Close the connection
        cnnSQL.Close()

        'free all resources
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion

        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        cnnSQL = Nothing
        cmdSQL.Execute()
        'RaiseError(Err, vbObjectError, CLASS_NAME, "'This Provider already has had a payout covering some/all of this date range")
        '  cmdSQL = Nothing
    End Function
    Public Function FetchByCriteria(ByVal lngProviderID As Long, ByVal dteStart As Date, ByVal dteEnd As Date) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/29/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblProviderPayout table ordered by provider name utilizing a stored procedure
        'Parameters: lngProviderID - id of the patient  to return records for
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
            .CommandText = "uspSelProvPayByCriteria"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , IIf(lngProviderID < 0, DBNull.Value, lngProviderID)))
            .Parameters.Append(.CreateParameter("@DateStart", adDBTimeStamp, adParamInput, , IIf(dteStart.ToOADate() = 0, DBNull.Value, dteStart)))
            .Parameters.Append(.CreateParameter("@DateEnd", adDBTimeStamp, adParamInput, , IIf(dteEnd.ToOADate() = 0, DBNull.Value, dteEnd)))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByCriteria = rs

        'Disconnect the recordset
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
        ' cmdSQL.Execute()
        'RaiseError(Err, Err.Number, CLASS_NAME)
    End Function
    Public Sub FetchPayoutPayments(ByVal dteStart As Date, ByVal dteEnd As Date, ByRef rstABS As ADODB.Recordset, ByRef rstABSMP As ADODB.Recordset, ByRef rstMain As ADODB.Recordset, ByRef rstMainMP As ADODB.Recordset)

        '--------------------------------------------------------------------
        'Date: 09/29/2000
        'Author: Eric Pena
        'Description:  Fetches totals by provider to use for payout
        'Parameters: dteStart / dteEnd - Date range to fetch postings for
        'Returns: Recordsets of gross postings by provider for the given date range
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        rstABS = FetchPayoutRstABS(dteStart, dteEnd)
        rstABSMP = FetchPayoutRstABSMP(dteStart, dteEnd)
        rstMain = FetchPayoutRstMain(dteStart, dteEnd)
        rstMainMP = FetchPayoutRstMainMP(dteStart, dteEnd)


        'Signal successful completion


        Exit Sub


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        'RaiseError(Err, "SP Error", CLASS_NAME)
    End Sub
    Private Function FetchPayoutRstABS(ByVal dteStart As Date, ByVal dteEnd As Date) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/29/2000
        'Author: Eric Pena
        'Description:  Updates a row in the tblProviderPayout  table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: Null
        '--------------------------------------------------------------------
        Dim rs As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        rs = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        With cmd
            .CommandText = "uspSelProvPoutABS"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@StartDate", adDBTimeStamp, adParamInput, , dteStart))
            .Parameters.Append(.CreateParameter("@EndDate", adDBTimeStamp, adParamInput, , dteEnd))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchPayoutRstABS = rs

        'Disconnect the recordset
        cmd = Nothing
        rs.ActiveConnection = Nothing
        cnn = Nothing
        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        ' cmdSQL.Execute()
        cmd = Nothing
        rs.ActiveConnection = Nothing
        cnn = Nothing
        'RaiseError(Err, "SP Error", CLASS_NAME)
    End Function
    Private Function FetchPayoutRstABSMP(ByVal dteStart As Date, ByVal dteEnd As Date) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/29/2000
        'Author: Eric Pena
        'Description:  Updates a row in the tblProviderPayout  table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: Null
        '--------------------------------------------------------------------
        Dim rs As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        rs = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        With cmd
            .CommandText = "uspSelProvPoutMPABS"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@StartDate", adDBTimeStamp, adParamInput, , dteStart))
            .Parameters.Append(.CreateParameter("@EndDate", adDBTimeStamp, adParamInput, , dteEnd))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchPayoutRstABSMP = rs

        'Disconnect the recordset
        cmd = Nothing
        rs.ActiveConnection = Nothing
        cnn = Nothing
        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        ' cmdSQL.Execute()
        cmd = Nothing
        rs.ActiveConnection = Nothing
        cnn = Nothing
        'RaiseError(Err, "SP Error", CLASS_NAME)

    End Function
    Private Function FetchPayoutRstMain(ByVal dteStart As Date, ByVal dteEnd As Date) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/29/2000
        'Author: Eric Pena
        'Description:  Updates a row in the tblProviderPayout  table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: Null
        '--------------------------------------------------------------------
        Dim rs As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        rs = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        With cmd
            .CommandText = "uspSelProvPoutOtherGross"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@StartDate", adDBTimeStamp, adParamInput, , dteStart))
            .Parameters.Append(.CreateParameter("@EndDate", adDBTimeStamp, adParamInput, , dteEnd))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchPayoutRstMain = rs

        'Disconnect the recordset
        cmd = Nothing
        rs.ActiveConnection = Nothing
        cnn = Nothing
        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        ' cmdSQL.Execute()
        cmd = Nothing
        rs.ActiveConnection = Nothing
        cnn = Nothing
        'RaiseError(Err, "SP Error", CLASS_NAME)

    End Function
    Private Function FetchPayoutRstMainMP(ByVal dteStart As Date, ByVal dteEnd As Date) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/29/2000
        'Author: Eric Pena
        'Description:  Updates a row in the tblProviderPayout  table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: Null
        '--------------------------------------------------------------------
        Dim rs As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        rs = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        With cmd
            .CommandText = "uspSelProvPoutMPOtherGross"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@StartDate", adDBTimeStamp, adParamInput, , dteStart))
            .Parameters.Append(.CreateParameter("@EndDate", adDBTimeStamp, adParamInput, , dteEnd))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchPayoutRstMainMP = rs

        'Disconnect the recordset
        cmd = Nothing
        rs.ActiveConnection = Nothing
        cnn = Nothing
        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        '  cmdSQL.Execute()
        cmd = Nothing
        rs.ActiveConnection = Nothing
        cnn = Nothing
        'RaiseError(Err, "SP Error", CLASS_NAME)

    End Function
    Public Function InsertIOU(ByVal lngProviderID As Long, ByVal strDescription As String, ByVal curIOUAmt As Decimal) As Long
        '--------------------------------------------------------------------
        'Date: 08/09/2001
        'Author: Eric Pena
        'Description:  Inserts a row into the tblIOU table utilizing a stored procedure.
        'Parameters: Each parameter identifies the column value that will be inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim intCount As Integer


        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspInsIOU"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@IOUID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@Desc", adVarChar, adParamInput, 50, Left(strDescription, 50)))
            .Parameters.Append(.CreateParameter("@IOUAmt", adDecimal, adParamInput, , curIOUAmt))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection

        cnnSQL.Open(_ConnectionString)
        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute() '()

        InsertIOU = cmdSQL.Parameters("@IOUID").Value
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
        '   cmdSQL.Execute()
        'RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME)
    End Function
    Public Function UpdateIOU(ByVal lngIOUID As Long, ByVal lngProviderID As Long, ByVal strDescription As String, _
                ByVal curIOUAmt As Decimal, ByVal curIOUBal As Decimal) As Long
        '--------------------------------------------------------------------
        'Date: 08/09/2001
        'Author: Eric Pena
        'Description:  Inserts a row into the tblIOU table utilizing a stored procedure.
        'Parameters: Each parameter identifies the column value that will be inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim intCount As Integer


        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspUpdIOU"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamInput, , lngIOUID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@Desc", adVarChar, adParamInput, 125, Left(strDescription, 125)))
            .Parameters.Append(.CreateParameter("@IOUAmt", adDecimal, adParamInput, , curIOUAmt))
            .Parameters.Append(.CreateParameter("@IOUBal", adDecimal, adParamInput, , curIOUBal))
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
            Exit Function
        End If

        'Close the connection and free all resources
        cnnSQL.Close()

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

    Public Function FetchIOUByProvider(ByVal lngProviderID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/09/2001
        'Author: Eric Pena
        'Description:  Updates a row in the tblProviderPayout  table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: Null
        '--------------------------------------------------------------------
        Dim rs As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        rs = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        With cmd
            .CommandText = "uspSelIOU"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchIOUByProvider = rs

        'Disconnect the recordset
        cmd = Nothing
        rs.ActiveConnection = Nothing
        cnn = Nothing
        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        cmd = Nothing
        rs.ActiveConnection = Nothing
        cnn = Nothing
        'RaiseError(Err, "SP Error", CLASS_NAME)

    End Function

    Public Function FetchIOUByID(ByVal lngIOUID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/09/2001
        'Author: Eric Pena
        'Description:  Selects a row in the tblProviderPayout  table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: Null
        '--------------------------------------------------------------------
        Dim rs As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        rs = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        With cmd
            .CommandText = "uspSelIOUByID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamInput, , lngIOUID))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchIOUByID = rs

        'Disconnect the recordset
        cmd = Nothing
        rs.ActiveConnection = Nothing
        cnn = Nothing
        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        cmd = Nothing
        rs.ActiveConnection = Nothing
        cnn = Nothing
        'RaiseError(Err, "SP Error", CLASS_NAME)
    End Function
    Public Sub PayIOU(ByVal lngIOUID As Long, ByVal curAmount As Decimal, ByVal lngPayoutDedID As Long)
        '--------------------------------------------------------------------
        'Date: 08/09/2001
        'Author: Eric Pena
        'Description:  Updates a row in the tblProviderPayout  table utilizing
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
            .CommandText = "uspPayIOU"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@IOUID", adInteger, adParamInput, , lngIOUID))
            .Parameters.Append(.CreateParameter("@Amt", adDecimal, adParamInput, , curAmount))
            .Parameters.Append(.CreateParameter("@PayoutDedID", adInteger, adParamInput, , lngPayoutDedID))
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
        'RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME)

    End Sub

    Public Function FetchPayoutDatesByYear(ByVal intYear As Integer, ByVal lngProviderID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/22/2002
        'Author: Dave Richkun
        'Description:  Returns payout dates for a given year
        'Parameters: intYear - The 4-digit year for which payout dates will be retrieved
        '            lngProviderID = ID of provider for whom payout dates will be retrieved
        'Returns: Recordset of payout dates
        '--------------------------------------------------------------------
        Dim rst As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        rst = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection

        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelPayoutDatesByYear"
            .Parameters.Append(.CreateParameter("@Year", adInteger, adParamInput, , intYear))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .CommandType = adCmdStoredProc
        End With

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchPayoutDatesByYear = rst

        'Disconnect the recordset
        cmd = Nothing
        cnn = Nothing
        rst.ActiveConnection = Nothing

        'Signal successful completion


        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()

        cmd = Nothing
        rst = Nothing
        cnn = Nothing
        'RaiseError(Err, Err.Number, CLASS_NAME)
    End Function

    Public Function FetchPayoutDetail(ByVal lngProvPayID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/23/2002
        'Author: Dave Richkun
        'Description:  Returns all transactions within a payout period for a provider
        'Parameters: lngProvPayID - ID of provider payout record with which payout details
        '               will be retrieved
        'Returns: Recordset of payout transactions
        '--------------------------------------------------------------------
        Dim rst As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        rst = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection

        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelPayoutDetail"
            .Parameters.Append(.CreateParameter("@ProvPayID", adInteger, adParamInput, , lngProvPayID))
            .CommandType = adCmdStoredProc
        End With

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchPayoutDetail = rst

        'Disconnect the recordset
        cmd = Nothing
        cnn = Nothing
        rst.ActiveConnection = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()

        cmd = Nothing
        rst = Nothing
        cnn = Nothing
        'RaiseError(Err, Err.Number, CLASS_NAME)
    End Function
    Public Sub Update(ByVal lngProvPayID As Long, ByVal dteEnd As Date, ByVal curGross As Decimal, ByVal curNet As Decimal)
        '--------------------------------------------------------------------
        'Date: 09/29/2000
        'Author: Eric Pena
        'Description:  Updates a row in the tblProviderPayout  table utilizing
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
            .CommandText = "uspUpdProvPay"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamInput, , lngProvPayID))
            .Parameters.Append(.CreateParameter("@End", adDBTimeStamp, adParamInput, , dteEnd))
            .Parameters.Append(.CreateParameter("@Gross", adDecimal, adParamInput, , curGross))
            .Parameters.Append(.CreateParameter("@Net", adDecimal, adParamInput, , curNet))
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
        'RaiseError(Err, Err.Number, CLASS_NAME, Err.Description)
    End Sub
    Public Function InsertProviderPayment(ByVal lngProviderID As Long, ByVal strPymtType As String, _
                ByVal curAmount As Decimal, ByVal strCheckNum As String, ByVal strNotes As String, _
                ByVal strAddedBy As String) As Long
        '--------------------------------------------------------------------
        'Date: 01/09/2003
        'Author: Dave Richkun
        'Description:  Inserts a row into the tblProviderPayment table utilizing
        '              a stored procedure.
        'Parameters:  lngProviderID - ID of provider making the payment to Psyquel
        '             strPymtType - 1 character code indicating the type of payment
        '                   1-Check, 2-Cash, 3-Credit Card, 4-Other
        '             curAmount - Amount of payment
        '             strCheckNum - Check number if payment by check
        '             strNotes - Notes related to payment
        '             strAddedBy - User name of user adding payment record
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------
        '  R001: Created
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspInsProviderPayment"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProvPymtID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@PymtType", adVarChar, adParamInput, 1, strPymtType))
            .Parameters.Append(.CreateParameter("@Amount", adDecimal, adParamInput, , curAmount))
            .Parameters.Append(.CreateParameter("@CheckNum", adVarChar, adParamInput, 20, strCheckNum))
            .Parameters.Append(.CreateParameter("@Notes", adVarChar, adParamInput, 256, strNotes))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strAddedBy))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection

        cnnSQL.Open(_ConnectionString)
        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute() '()

        InsertProviderPayment = cmdSQL.Parameters("@ProvPymtID").Value

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


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()
        'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME)
    End Function

    Public Function InsertGroupPayment(ByVal lngGroupID As Long, ByVal lngUserID As Long, ByVal strPymtType As String, _
                ByVal curAmount As Decimal, ByVal strCheckNum As String, ByVal strNotes As String, _
                ByVal strAddedBy As String) As Long
        '--------------------------------------------------------------------
        'Date: 01/09/2003
        'Author: Duane C Orth
        'Description:  Inserts a row into the tblProviderPayment table utilizing
        '              a stored procedure.
        'Parameters:  lngProviderID - ID of provider making the payment to Psyquel
        '             strPymtType - 1 character code indicating the type of payment
        '                   1-Check, 2-Cash, 3-Credit Card, 4-Other
        '             curAmount - Amount of payment
        '             strCheckNum - Check number if payment by check
        '             strNotes - Notes related to payment
        '             strAddedBy - User name of user adding payment record
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------
        '  R001: Created
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspPostGroupCheck"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProvPymtID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@GroupID", adInteger, adParamInput, , lngGroupID))
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
            .Parameters.Append(.CreateParameter("@PymtType", adVarChar, adParamInput, 1, strPymtType))
            .Parameters.Append(.CreateParameter("@Amount", adDecimal, adParamInput, , curAmount))
            .Parameters.Append(.CreateParameter("@CheckNum", adVarChar, adParamInput, 20, strCheckNum))
            .Parameters.Append(.CreateParameter("@Notes", adVarChar, adParamInput, 256, strNotes))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strAddedBy))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection

        cnnSQL.Open(_ConnectionString)
        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute() '()

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


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()
        'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME)
    End Function
    Public Sub UpdateProviderPayment(ByVal lngPaymentID As Long, ByVal lngProviderID As Long, _
                ByVal strPymtType As String, ByVal curAmount As Decimal, ByVal strCheckNum As String, _
                ByVal strNotes As String, ByVal strAddedBy As String)
        '--------------------------------------------------------------------
        'Date: 01/13/2003
        'Author: Dave Richkun
        'Description:  Updates a row in tblProviderPayment table utilizing
        '              a stored procedure.
        'Parameters:  lngPaymentID - ID of provider payment that will be updated
        '             lngProviderID - ID of provider making the payment to Psyquel
        '             strPymtType - 1 character code indicating the type of payment
        '                   1-Check, 2-Cash, 3-Credit Card, 4-Other
        '             curAmount - Amount of payment
        '             strCheckNum - Check number if payment by check
        '             strNotes - Notes related to payment
        '             strAddedBy - User name of user adding payment record
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------
        '  R001: Created
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspUpdProviderPayment"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProvPymtID", adInteger, adParamInput, , lngPaymentID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@PymtType", adVarChar, adParamInput, 1, strPymtType))
            .Parameters.Append(.CreateParameter("@Amount", adDecimal, adParamInput, , curAmount))
            .Parameters.Append(.CreateParameter("@CheckNum", adVarChar, adParamInput, 20, strCheckNum))
            .Parameters.Append(.CreateParameter("@Notes", adVarChar, adParamInput, 256, strNotes))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strAddedBy))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection

        cnnSQL.Open(_ConnectionString)
        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute() '()

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        'Close the connection
        cnnSQL.Close()

        'Free all resources
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion

        Exit Sub


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()
        'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME)
    End Sub


    Public Function FetchPaymentsByCriteria(Optional ByVal blnIncludeDisabled As Boolean = False, _
                          Optional ByVal strWhere As String = "", _
                          Optional ByVal strOrderBy As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 01/03/2013                                                   '
        'Author: Duane C Orth                                               '
        'Description:  Retrieves records from the tblProviderPayment table. '
        'Parameters: blnIncludeDisabled - Optional parameter that identifies'
        '               if records flagged as 'Disabled' or 'De-activated'  '
        '               are to be included in the record set. The default   '
        '               value is False.                                     '
        '            strWhere - Optional 'Where' clause of the SQL statement'
        '               by which records will be filtered.  If the parameter'
        '               is not supplied, all records will be retrieved.     '
        '             strOrderBy - Optional 'Order By' clause in which      '
        '               retrieved records will be sorted.  The default is   '
        '               set to the fldUser' column                          '
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim strConcatWhere As String
        Dim strSQL As String



        'Prepare the SQL statement.
        strSQL = "SELECT "
        strSQL = strSQL & "A.*, "
        strSQL = strSQL & "B.fldLastName, "
        strSQL = strSQL & "B.fldFirstName "
        strSQL = strSQL & "FROM "
        strSQL = strSQL & "tblProviderPayment AS A LEFT OUTER JOIN "
        strSQL = strSQL & "tblUser AS B ON B.fldUserID = A.fldProviderID"

        'Determine whether to include 'disabled' records'
        If blnIncludeDisabled = False Then
            strConcatWhere = " WHERE A.fldDisabledYN = 'N' "
        End If

        'Attach user's where clause if applicable
        If Trim(strWhere) > "" Then
            If strConcatWhere > "" Then
                strSQL = strSQL & strConcatWhere & " AND "
                strSQL = strSQL & strWhere
            Else
                strSQL = strSQL & " WHERE " & strWhere
            End If
        Else
            strSQL = strSQL & strConcatWhere
        End If

        If Trim(strOrderBy) > "" Then
            strSQL = strSQL & " ORDER BY "
            strSQL = strSQL & strOrderBy
        Else
            strSQL = strSQL & " ORDER BY "
            strSQL = strSQL & "B.fldLastName, B.fldFirstName"
        End If

        Debug.Print(strSQL)
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
        FetchPaymentsByCriteria = rstSQL

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function


    Public Function FetchPaymentsByProvider(ByVal lngProviderID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 01/13/2003
        'Author: Dave Richkun
        'Description:  Retrieves all payments associated with a provider via
        '              a stored procedure.
        'Parameters: lngProviderID - ID of provider for whom payments are retrieved
        'Returns: ADO Recordset
        '--------------------------------------------------------------------
        Dim rst As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        rst = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection

        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        With cmd
            .CommandText = "uspSelProviderPayment"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
        End With

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchPaymentsByProvider = rst

        'Disconnect the recordset
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing
        'RaiseError(Err, "SP Error", CLASS_NAME)

    End Function


    Public Function FetchProviderPaymentByID(ByVal lngProvPymtID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 01/13/2003
        'Author: Dave Richkun
        'Description:  Retrieves a single payment record via a stored procedure.
        'Parameters: lngProvPymtID - ID of Provider Payment record to be retrieved
        'Returns: ADO Recordset
        '--------------------------------------------------------------------
        Dim rst As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        rst = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection

        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        With cmd
            .CommandText = "uspSelProviderPaymentByID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProvPymtID", adInteger, adParamInput, , lngProvPymtID))
        End With

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchProviderPaymentByID = rst

        'Disconnect the recordset
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing
        'RaiseError(Err, "SP Error", CLASS_NAME)

    End Function


    Public Sub DeleteProviderPayment(ByVal lngProvPymtID As Long, ByVal strDeletedBy As String)
        '--------------------------------------------------------------------
        'Date: 01/13/2003
        'Author: Dave Richkun
        'Description:  Disabled a row in tblProviderPayment table utilizing
        '              a stored procedure.
        'Parameters:  lngProvPymtID - ID of row that will be disabled
        '             strDeletedBy - User name of user disabling payment record
        'Returns: Null
        '--------------------------------------------------------------------
        '  R001: Created
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspUpdDisableProviderPayment"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProvPymtID", adInteger, adParamInput, , lngProvPymtID))
            .Parameters.Append(.CreateParameter("@DisabledBy", adVarChar, adParamInput, 30, strDeletedBy))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection

        cnnSQL.Open(_ConnectionString)
        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute() '()

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        'Close the connection
        cnnSQL.Close()

        'Free all resources
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion

        Exit Sub


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()
        'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME)
    End Sub


    Public Function FetchDeductionByID(ByVal lngProvPayDedID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 01/28/2003
        'Author: Dave Richkun
        'Description:  Retrieves a single payout deduction record via a stored procedure.
        'Parameters: lngProvPayDedID - ID of Provider Payment record to be retrieved
        'Returns: ADO Recordset
        '--------------------------------------------------------------------
        Dim rst As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        rst = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection

        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        With cmd
            .CommandText = "uspSelPayoutDeductionByID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProvPayDedID", adInteger, adParamInput, , lngProvPayDedID))
        End With

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchDeductionByID = rst

        'Disconnect the recordset
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing
        'RaiseError(Err, "SP Error", CLASS_NAME)

    End Function

    Public Function FetchPayoutSummaryByProvider(ByVal lngProviderID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 02/04/2003
        'Author: Dave Richkun
        'Description:  Retrieves balance owing per payout period for the specified provider.
        'Parameters: lngProviderID - ID of provider whose payout summary will be retrieved
        'Returns: ADO Recordset
        '--------------------------------------------------------------------
        Dim rst As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        rst = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection

        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        With cmd
            .CommandText = "uspSelProviderPayoutSummary"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
        End With

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchPayoutSummaryByProvider = rst

        'Disconnect the recordset
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing
        'RaiseError(Err, "SP Error", CLASS_NAME)

    End Function


    Public Sub InsertPayoutRows(ByVal dtPayoutStartDate As Date)
        '--------------------------------------------------------------------
        'Date: 02/12/2003
        'Author: Dave Richkun
        'Description:  Inserts rows in tblProviderPayout for all providers not already having
        '              having a row for the specified payout start date.
        'Parameters: dtPayoutStartDate - Starting date of the payout period
        'Returns: Null
        '--------------------------------------------------------------------
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        cmd = New ADODB.Command
        'Create the parameter objects
        With cmd
            .CommandText = "uspInsProvPayoutRows"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@StartDate", adDBTimeStamp, adParamInput, , dtPayoutStartDate))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnn = New ADODB.Connection
        cnn.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmd.ActiveConnection = cnn
        cmd.Execute()

        'Close the connection and free all resources
        cnn.Close()

        cmd = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Sub

        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        cmd = Nothing
        cnn = Nothing
        'RaiseError(Err, Err.Number, CLASS_NAME)

    End Sub
End Class

