
'--------------------------------------------------------------------
'Class Name: CRPCredDB
'Date: 10/19/2000
'Author: Eric Pena
'Denis J Finkel
'Description:  MTS object designed to host methods associated with data affecting the tblRPProviderCredit  table.
'--------------------------------------------------------------------
'Revision History:
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

Public Class CRPCredDB



    Private _ConnectionString As String = String.Empty

    Private Const CLASS_NAME As String = "CRPCredDB"
    Private Const TABLE_NAME As String = "tblRPProviderCredit"


    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property

    Public Function StoreCredit(ByVal lngID As Long, ByVal lngELID As Long, ByVal lngRPID As Long, ByVal curAmt As Decimal) As Long
        '--------------------------------------------------------------------
        'Date: 10/19/2000
        'Author: Eric Pena
        'Description:  Modifies/Inserts a row into the tblRPProviderCredit  and tblTransaction table utilizing a stored procedure.
        'Parameters: lngid - the Id in tblRPProviderCredit to use, or -1 if a new record is to be added
        '                   lngelid - the EL to post from (will deduct from this EL)
        '                   curamt - amount to pot to credit
        'Returns: null
        '--------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim intCount As Integer


        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspStoreRPCred"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@CredID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamInput, , IIf(lngID < 0, DBNull.Value, lngID)))
            .Parameters.Append(.CreateParameter("@ELID", adInteger, adParamInput, , lngELID))
            .Parameters.Append(.CreateParameter("@RPID", adInteger, adParamInput, , lngRPID))
            .Parameters.Append(.CreateParameter("@Amt", adDecimal, adParamInput, , curAmt))
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

        StoreCredit = cmdSQL.Parameters("@CredID").Value

        'Close the connection
        cnnSQL.Close()

        'free all resources
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion

        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        cnnSQL = Nothing
        'RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME)
        cmdSQL = Nothing
        cmdSQL.Execute()
    End Function
    Public Sub ApplyCredit(ByVal lngID As Long, ByVal curAmt As Decimal)
        '--------------------------------------------------------------------
        'Date: 10/19/2000
        'Author: Eric Pena
        'Description:  Updates a row in the tblRPProviderCredit, inserts a row into the tbltransaction  table utilizing a stored procedure.
        'Parameters: lngID:  The ID of the row to be updated All other parameters identifies the column value that will be  updated.
        'Returns: Null
        '--------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspApplyRPCred"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamInput, , lngID))
            .Parameters.Append(.CreateParameter("@Amt", adDecimal, adParamInput, , curAmt))
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
        cnnSQL = Nothing
        cmdSQL.Execute()
        'RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, cmdSQL.Parameters("@ErrorMsg").Value)
        cmdSQL = Nothing
    End Sub
    Public Function FetchByID(ByVal lngID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/19/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblRPProviderCredit table ordered by patient name utilizing a stored procedure
        'Parameters: lngID - PK to return records for
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
            .CommandText = "uspSelRPCredByID"
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
    Public Function FetchByPatient(ByVal lngPatientID As Long, ByVal blnZeroBal As Boolean) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/19/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblRPProviderCredit table ordered by provider name utilizing a stored procedure
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
            .CommandText = "uspSelRPCredByPat"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@ZeroBalYN", adVarChar, adParamInput, 1, IIf(blnZeroBal, "Y", "N")))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByPatient = rs

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
    Public Function Fetch(ByVal blnZeroBal As Boolean) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/19/2000
        'Author: Eric Pena
        'Description:  Returns rows from tblRPProviderCredit determining current outstanding balances
        'Parameters:    lngpatientid - Patient ID for which to search balances, or -1 for all patients
        '                       lngproviderid - Provider ID for which to search balances, or -1 for all providers
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
            .CommandText = "uspSelRPCred"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ZeroBalYN", adVarChar, adParamInput, 1, IIf(blnZeroBal, "Y", "N")))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        Fetch = rs

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

    Public Function FetchByProvider(ByVal lngProviderID As Long, ByVal blnZeroBal As Boolean) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/19/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblRPProviderCredit table ordered by provider name utilizing a stored procedure
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
            .CommandText = "uspSelRPCredByProv"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@ZeroBalYN", adVarChar, adParamInput, 1, IIf(blnZeroBal, "Y", "N")))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByProvider = rs

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
    Public Function FetchByEL(ByVal lngELID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/19/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblRPProviderCredit table ordered by patient name utilizing a stored procedure
        'Parameters: lngID - PK to return records for
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
            .CommandText = "uspSelRPCredByEL"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ELID", adInteger, adParamInput, , lngELID))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByEL = rs

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
    Public Function FetchEligibleCredRefund(ByVal lngBRID As Long, ByVal curMinAmt As Decimal) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 6/1/2001
        'Author: Eric Pena
        'Description:  Finds a DOS for which an amount of curMinAmt can be moved to a credit balance
        'Parameters: lngBRID - BR ID of RP to post credit toward
        '               curMinAmt - amount that needs to be moved to a credit balance
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
            .CommandText = "uspSelEligCredRefund"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@BRID", adInteger, adParamInput, , lngBRID))
            .Parameters.Append(.CreateParameter("@CurAmt", adDecimal, adParamInput, , curMinAmt))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchEligibleCredRefund = rs

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

End Class

