
'--------------------------------------------------------------------
'Class Name: CMisdirectPmtBz
'Date: 06/11/2001
'Author: Eric Pena
'Denis J Finkel
'Description:  MTS object designed to host methods associated with
'              data affecting the tblMisdirectedInsPayment table.
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

Public Class CMisdirectPmtDB

    Private _ConnectionString As String = String.Empty



    Private Const CLASS_NAME As String = "CMisdirectPmtDB"
    Private Const TABLE_NAME As String = "tblMisdirectedInsPayment"
    Private Const CONST_NO_DATE As Date = #12:00:00 AM#

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property



    Public Function Insert(ByVal lngBRID As Long, ByVal strCheckNum As String, ByVal dteCheckDate As Date, ByVal dteClearDate As Date, ByVal curCheckAmount As Decimal, ByVal curPostAmount As Decimal, _
                            ByVal curAmtDisallow As Decimal, ByVal curAmtPatient As Decimal, ByVal blnMultiCheck As Boolean, ByVal strAddress1 As String, _
                            ByVal strAddress2 As String, ByVal strCity As String, ByVal strState As String, ByVal strZip As String, ByVal strContactName As String, _
                            ByVal strContactPhone As String, ByVal strContactExt As String, ByVal strNotes As String, ByVal strUserName As String) As Long
        '--------------------------------------------------------------------
        'Date: 06/11/2001
        'Author: Eric Pena
        'Description:  Inserts a row into the tblMisdirectedPayment table utilizing a stored procedure.
        'Parameters: Each parameter identifies the column value that will be  inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim intCount As Integer



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspInsMisPmt"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@MisPmtID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@BRID", adInteger, adParamInput, , lngBRID))
            .Parameters.Append(.CreateParameter("@CheckNum", adVarChar, adParamInput, 30, strCheckNum))
            .Parameters.Append(.CreateParameter("@CheckDate", adDBTimeStamp, adParamInput, , IIf(dteCheckDate.ToOADate() = 0, DBNull.Value, dteCheckDate)))
            .Parameters.Append(.CreateParameter("@ClearDate", adDBTimeStamp, adParamInput, , IIf(dteClearDate.ToOADate() = 0, DBNull.Value, dteClearDate)))
            .Parameters.Append(.CreateParameter("@CheckAmount", adDecimal, adParamInput, , curCheckAmount))
            .Parameters.Append(.CreateParameter("@PostAmount", adDecimal, adParamInput, , curPostAmount))
            .Parameters.Append(.CreateParameter("@AmtDisallow", adDecimal, adParamInput, , curAmtDisallow))
            .Parameters.Append(.CreateParameter("@AmtPatient", adDecimal, adParamInput, , curAmtPatient))
            .Parameters.Append(.CreateParameter("@MultiCheckYN", adVarChar, adParamInput, 1, IIf(blnMultiCheck, "Y", "N")))
            .Parameters.Append(.CreateParameter("@SentAddress1", adVarChar, adParamInput, 60, strAddress1))
            .Parameters.Append(.CreateParameter("@SentAddress2", adVarChar, adParamInput, 60, strAddress2))
            .Parameters.Append(.CreateParameter("@SentCity", adVarChar, adParamInput, 40, strCity))
            .Parameters.Append(.CreateParameter("@SentState", adVarChar, adParamInput, 2, strState))
            .Parameters.Append(.CreateParameter("@SentZip", adVarChar, adParamInput, 12, strZip))
            .Parameters.Append(.CreateParameter("@ContactName", adVarChar, adParamInput, 60, strContactName))
            .Parameters.Append(.CreateParameter("@ContactPhone", adVarChar, adParamInput, 15, strContactPhone))
            .Parameters.Append(.CreateParameter("@ContactExt", adVarChar, adParamInput, 10, strContactExt))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@Notes", adVarChar, adParamInput, 256, strNotes))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        Insert = cmdSQL.Parameters("@MisPmtID").Value

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
    Public Sub Update(ByVal lngMisPmtID As Long, ByVal strCheckNum As String, ByVal dteCheckDate As Date, ByVal dteClearDate As Date, ByVal curCheckAmount As Decimal, ByVal curPostAmount As Decimal, _
                            ByVal curAmtDisallow As Decimal, ByVal curAmtPatient As Decimal, ByVal blnMultiCheck As Boolean, ByVal strAddress1 As String, _
                            ByVal strAddress2 As String, ByVal strCity As String, ByVal strState As String, ByVal strZip As String, ByVal strContactName As String, _
                            ByVal strContactPhone As String, ByVal strContactExt As String, ByVal strNotes As String, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 06/11/2001
        'Author: Eric Pena
        'Description:  Updates a row in the tblMisdirectedPayment table utilizing a stored procedure.
        'Parameters: Each parameter identifies the column value that will be inserted.
        'Returns: Null
        '--------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdMisPmt"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@MisPmtID", adInteger, adParamInput, , lngMisPmtID))
            .Parameters.Append(.CreateParameter("@CheckNum", adVarChar, adParamInput, 30, strCheckNum))
            .Parameters.Append(.CreateParameter("@CheckDate", adDBTimeStamp, adParamInput, , IIf(dteCheckDate = CONST_NO_DATE, DBNull.Value, dteCheckDate)))
            .Parameters.Append(.CreateParameter("@ClearDate", adDBTimeStamp, adParamInput, , IIf(dteClearDate = CONST_NO_DATE, DBNull.Value, dteClearDate)))
            .Parameters.Append(.CreateParameter("@CheckAmount", adDecimal, adParamInput, , curCheckAmount))
            .Parameters.Append(.CreateParameter("@PostAmount", adDecimal, adParamInput, , curPostAmount))
            .Parameters.Append(.CreateParameter("@AmtDisallow", adDecimal, adParamInput, , curAmtDisallow))
            .Parameters.Append(.CreateParameter("@AmtPatient", adDecimal, adParamInput, , curAmtPatient))
            .Parameters.Append(.CreateParameter("@MultiCheckYN", adVarChar, adParamInput, 1, IIf(blnMultiCheck, "Y", "N")))
            .Parameters.Append(.CreateParameter("@SentAddress1", adVarChar, adParamInput, 60, strAddress1))
            .Parameters.Append(.CreateParameter("@SentAddress2", adVarChar, adParamInput, 60, strAddress2))
            .Parameters.Append(.CreateParameter("@SentCity", adVarChar, adParamInput, 40, strCity))
            .Parameters.Append(.CreateParameter("@SentState", adVarChar, adParamInput, 2, strState))
            .Parameters.Append(.CreateParameter("@SentZip", adVarChar, adParamInput, 12, strZip))
            .Parameters.Append(.CreateParameter("@ContactName", adVarChar, adParamInput, 60, strContactName))
            .Parameters.Append(.CreateParameter("@ContactPhone", adVarChar, adParamInput, 15, strContactPhone))
            .Parameters.Append(.CreateParameter("@ContactExt", adVarChar, adParamInput, 10, strContactExt))
            .Parameters.Append(.CreateParameter("@Notes", adVarChar, adParamInput, 256, strNotes))
            .Parameters.Append(.CreateParameter("@ModBy", adVarChar, adParamInput, 30, strUserName))
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
        'RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, cmdSQL.Parameters("@ErrorMsg").Value)
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()
    End Sub
    Public Sub Deleted(ByVal lngID As Long, ByVal blnIsDeleted As Boolean, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 06/11/2001
        'Author: Eric Pena
        'Description:  Flags a row in the tblMisdirectedInsPayment table marking the row as
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



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspDelMisPmt"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@MisPmtID", adInteger, adParamInput, , lngID))
            .Parameters.Append(.CreateParameter("@DeletedYN", adVarChar, adParamInput, 1, IIf(blnIsDeleted, "Y", "N")))
            .Parameters.Append(.CreateParameter("@DeletedBy", adVarChar, adParamInput, 30, strUserName))
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
        'RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, cmdSQL.Parameters("@ErrorMsg").Value)
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()
    End Sub
    Public Function FetchByUser(ByVal lngUserID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 06/11/2001
        'Author: Eric Pena
        'Description:  Returns rows from the tblMisdirectedPayment table utilizing a stored procedure
        'Parameters: lngUserID - user for which records will be returned
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
            .CommandText = "uspSelMisdirectedPaymentsByUser"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, 1, lngUserID))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByUser = rs

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
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
    Public Function FetchByEL(ByVal lngELID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 06/11/2001
        'Author: Eric Pena
        'Description:  Returns rows from the tblMisdirectedPayment table utilizing a stored procedure
        'Parameters: lngELID - PK in tblEncounterLog for which records will be returned
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
            .CommandText = "uspSelMisPmtByEL"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamInput, , lngELID))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByEL = rs

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
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
    Public Function FetchByID(ByVal lngMPID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 06/11/2001
        'Author: Eric Pena
        'Description:  Returns rows from the tblMisdirectedPayment table utilizing a stored procedure
        'Parameters: lngMPID - PK in tblMisdirectedPayment for which records will be returned
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
            .CommandText = "uspSelMisPmtByID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamInput, , lngMPID))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByID = rs

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
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
    Public Function FetchByBR(ByVal lngBRID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 06/11/2001
        'Author: Eric Pena
        'Description:  Returns rows from the tblMisdirectedPayment table utilizing a stored procedure
        'Parameters: lngBRID - PK in tblBillingResponsibility for which records will be returned
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
            .CommandText = "uspSelMisPmtByBR"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamInput, , lngBRID))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByBR = rs

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
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
    Public Function isProcessed(ByVal lngMisPmtID As Long) As Boolean
        '--------------------------------------------------------------------
        'Date: 06/11/2001
        'Author: Eric Pena
        'Description:  Lets the er know if a given Mis. Pmt. has been processed
        'Parameters: lngMisPmtID - The ID of the MP Entry
        'Returns: T if the record is processed, F otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim strSQL As String

        isProcessed = False



        'Prepare the SQL statement.
        strSQL = "SELECT fldProcessedYN FROM tblMisdirectedInsPayment WHERE fldMisPmtID = " & lngMisPmtID & " "

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
        If rstSQL.RecordCount = 0 Then GoTo EMPTY_RECORDSET
        If rstSQL.Fields("fldProcessedYN").Value = "Y" Then isProcessed = True

EMPTY_RECORDSET:
        'Signal successful completion

        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        rstSQL = Nothing
        cnnSQL = Nothing
        'cmdSQL.Execute()
        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function
    Public Sub SetAction(ByVal lngMisPmtID As Long, ByVal strActionType As String, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 06/11/2001
        'Author: Eric Pena
        'Description:  Updates a row in the tblMisdirectedPayment table utilizing a stored procedure.
        'Parameters: Each parameter identifies the column value that will be inserted.
        'Returns: Null
        '--------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdMisPmtAction"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@MisPmtID", adInteger, adParamInput, , lngMisPmtID))
            .Parameters.Append(.CreateParameter("@ActionType", adVarChar, adParamInput, 1, strActionType))
            .Parameters.Append(.CreateParameter("@ModifiedBy", adVarChar, adParamInput, 30, strUserName))
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
        'RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, cmdSQL.Parameters("@ErrorMsg").Value)
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()
    End Sub
    Public Sub SetProcessed(ByVal lngMisPmtID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 06/11/2001
        'Author: Eric Pena
        'Description:  Updates a row in the tblMisdirectedPayment table utilizing a stored procedure.
        'Parameters: Each parameter identifies the column value that will be inserted.
        'Returns: Null
        '--------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdMisPmtProcessed"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@MisPmtID", adInteger, adParamInput, , lngMisPmtID))
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName))
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
        'RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, cmdSQL.Parameters("@ErrorMsg").Value)
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()
    End Sub
    Public Function FetchForProcessing() As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 06/11/2001
        'Author: Eric Pena
        'Description:  Returns rows ready for processing from the tblMisdirectedPayment table utilizing a stored procedure
        'Parameters:
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
            .CommandText = "uspSelMisPmtForProcess"
            .CommandType = adCmdStoredProc
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchForProcessing = rs

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
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

    Public Function FetchForAutoAction() As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 06/11/2001
        'Author: Eric Pena
        'Description:  Returns rows ready for processing from the tblMisdirectedPayment table utilizing a stored procedure
        'Parameters:
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
            .CommandText = "uspSelMisPmtForAutoAction"
            .CommandType = adCmdStoredProc
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchForAutoAction = rs

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
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


    Public Function FetchRejections() As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 04/30/2003
        'Author: Dave Richkun
        'Description:  Returns rejected MisDirected payments
        'Parameters:
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

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelMisPmtRejects"
            .CommandType = adCmdStoredProc
        End With

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchRejections = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        'RaiseError(Err, cmd.Parameters("@ErrorNum").Value, CLASS_NAME)
        cmd = Nothing
        rst = Nothing
        cnn = Nothing

    End Function

End Class
