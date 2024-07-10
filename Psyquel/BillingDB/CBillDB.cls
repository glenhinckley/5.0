''Class Name: CBillingDB
'Date: 08/30/2000
'Author: Eric Pena
'Description:  MTS object designed to host methods associated with data affecting the billing side of the tblEncounterLog table.
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


Public Class CBillDB
    Private _ConnectionString As String = String.Empty

    Private Const CLASS_NAME As String = "CBillingDB"
    Private Const TABLE_NAME As String = "tblEncounterLog"
    Private Const CONST_NO_CURR As Decimal = -666


    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property



    Public Sub Insert(ByVal lngEncounterLogID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 08/30/2000
        'Author: Eric Pena
        'Description:  Modifies a row in the tblEncounterLog table utilizing a stored procedure.
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
            .CommandText = "uspInsBill"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@EncounterLogID", adInteger, adParamInput, , lngEncounterLogID))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strUserName))
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

        'Close the connection
        cnnSQL.Close()

        'free all resources
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Sub


        'Signal incompletion and raise the error to the ing environment.
        'RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME)
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()

    End Sub

    Public Sub Update(ByVal lngID As Long, ByVal lngPatientID As Long, ByVal dteActionDate As Date, ByVal strPatientNotes As String, ByVal strBillNotes As String, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 08/30/2000
        'Author: Eric Pena
        'Description:  Updates a row in the tblEncounterLog table utilizing
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
            .CommandText = "uspUpdBill"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@BillID", adInteger, adParamInput, , lngID))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@ActionDate", adDBTimeStamp, adParamInput, , dteActionDate))
            .Parameters.Append(.CreateParameter("@PatientNotes", adVarChar, adParamInput, 1000, Trim(strPatientNotes)))
            .Parameters.Append(.CreateParameter("@BillNotes", adVarChar, adParamInput, 1000, Trim(strBillNotes)))
            .Parameters.Append(.CreateParameter("@BillModBy", adVarChar, adParamInput, 30, strUserName))
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
    Public Function Exists(ByVal lngEncounterLogID As Long) As Boolean
        '--------------------------------------------------------------------
        'Date: 08/30/2000
        'Author: Eric Pena
        'Description:  Determines if an Encounter has already been billed
        'Parameters: All parameters are address fields to check
        'Returns: True if the encounter has already been billed, false otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim strSQL As String
        Dim blnExists As Boolean



        'Prepare the SQL statement
        strSQL = "SELECT fldProcessed "
        strSQL = strSQL & "FROM "
        strSQL = strSQL & TABLE_NAME
        strSQL = strSQL & " WHERE "
        strSQL = strSQL & " fldEncounterLogID = " & lngEncounterLogID

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Instantiate and populate the Recordset.
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(strSQL, cnnSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)

        'Disconnect the recordset, close the connection and return the recordset to the ing environment.
        rstSQL.ActiveConnection = Nothing

        'Close the connection and free all resources
        cnnSQL.Close()
        cnnSQL = Nothing

        'Get the value from the Recordset then destroy it.
        If (rstSQL.Fields("fldProcessed").Value = "Y") Then
            Exists = True
        Else
            Exists = False
        End If
        rstSQL = Nothing

        'Signal successful completion

        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        cnnSQL = Nothing
        rstSQL = Nothing
        'cmdSQL.Execute()
        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function
    Public Function Fetch() As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/30/2000
        'Author: Eric Pena
        'Description:  Returns rows from tblEncounterLog that have been billed utilizing a stored procedure
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
            .CommandText = "uspSelBill"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        Fetch = rs

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

    Public Function FetchOutstandingClaims(ByVal lngBillID As Long, ByVal lngStatusID As Long, ByVal blnEClaim As Boolean, _
            ByVal lngPatientID As Long, ByVal strPatientFirst As String, ByVal strPatientLast As String, ByVal strPatientSSN As String, _
            ByVal lngProviderID As Long, ByVal strProviderFirst As String, ByVal strProviderLast As String, _
            ByVal strPlanName As String, ByVal strPlanCity As String, ByVal strPlanState As String, ByVal dteDOSFrom As Date, _
            ByVal dteDOSTo As Date, ByVal dtePostFrom As Date, ByVal dtePostTo As Date, ByVal dteActionFrom As Date, _
            ByVal dteActionTo As Date, ByVal curBalFrom As Decimal, ByVal curBalTo As Decimal) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/03/2000
        'Author: Eric Pena
        'Description:  Returns outstanding claims based on any combination of the specified parameters
        'Parameters: all parameters are criteria to use in searching for claims
        'Returns: Recordset of claims matching the criteria
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
            .CommandText = "uspSelBillByCriteria"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@BillID", adInteger, adParamInput, , lngBillID))
            .Parameters.Append(.CreateParameter("@StatusID", adInteger, adParamInput, , lngStatusID))
            .Parameters.Append(.CreateParameter("@EClaimYN", adVarChar, adParamInput, 1, IIf(blnEClaim = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@PatID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@PatFirst", adVarChar, adParamInput, 30, strPatientFirst))
            .Parameters.Append(.CreateParameter("@PatLast", adVarChar, adParamInput, 30, strPatientLast))
            .Parameters.Append(.CreateParameter("@PatSSN", adVarChar, adParamInput, 11, strPatientSSN))
            .Parameters.Append(.CreateParameter("@ProvID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@ProvFirst", adVarChar, adParamInput, 30, strProviderFirst))
            .Parameters.Append(.CreateParameter("@ProvLast", adVarChar, adParamInput, 30, strProviderLast))
            .Parameters.Append(.CreateParameter("@PlanName", adVarChar, adParamInput, 30, strPlanName))
            .Parameters.Append(.CreateParameter("@PlanCity", adVarChar, adParamInput, 40, strPlanCity))
            .Parameters.Append(.CreateParameter("@PlanState", adVarChar, adParamInput, 2, strPlanState))
            .Parameters.Append(.CreateParameter("@DOSFrom", adDBTimeStamp, adParamInput, , IIf(dteDOSFrom.ToOADate() = 0, DBNull.Value, dteDOSFrom)))
            .Parameters.Append(.CreateParameter("@DOSTo", adDBTimeStamp, adParamInput, , IIf(dteDOSTo.ToOADate() = 0, DBNull.Value, dteDOSTo)))
            .Parameters.Append(.CreateParameter("@PostFrom", adDBTimeStamp, adParamInput, , IIf(dtePostFrom.ToOADate() = 0, DBNull.Value, dtePostFrom)))
            .Parameters.Append(.CreateParameter("@PostTo", adDBTimeStamp, adParamInput, , IIf(dtePostTo.ToOADate() = 0, DBNull.Value, dtePostTo)))
            .Parameters.Append(.CreateParameter("@ActionFrom", adDBTimeStamp, adParamInput, , IIf(dteActionFrom.ToOADate() = 0, DBNull.Value, dteActionFrom)))
            .Parameters.Append(.CreateParameter("@ActionTo", adDBTimeStamp, adParamInput, , IIf(dteActionTo.ToOADate() = 0, DBNull.Value, dteActionTo)))
            .Parameters.Append(.CreateParameter("@BalFrom", adInteger, adParamInput, , curBalFrom))
            .Parameters.Append(.CreateParameter("@BalTo", adInteger, adParamInput, , curBalTo))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchOutstandingClaims = rs

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rs.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion

        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        rs = Nothing
        cnn = Nothing
        'RaiseError(Err, cmd.Parameters("@ErrorNum").Value, CLASS_NAME)
        cmd = Nothing
        'cmdSQL.Execute()
    End Function

    Public Function FetchByID(ByVal lngBillID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/30/2000
        'Author: Eric Pena
        'Description:  Returns rows from tblEncounterLog that have been billed utilizing a stored procedure
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
            .CommandText = "uspSelBillByID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@BillID", adInteger, adParamInput, , lngBillID))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
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
        rs = Nothing
        cnn = Nothing
        'RaiseError(Err, cmd.Parameters("@ErrorNum").Value, CLASS_NAME)
        cmd = Nothing
        'cmdSQL.Execute()
    End Function

    Public Function FetchPatOpeningBalByID(ByVal lngID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/29/2000
        'Author: Eric Pena
        'Description:  Returns a row from the tblPatientPaymentLog table utilizing a stored procedure
        'Parameters: lngID - id of the row to return records for
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
            .CommandText = "uspSelPatPmtLogByID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PmtLogID", adInteger, adParamInput, 1, lngID))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchPatOpeningBalByID = rs

        'Disconnect the recordset
        cmd = Nothing
        rs.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        cmd = Nothing
        rs = Nothing
        cnn = Nothing
        'cmdSQL.Execute()
        'RaiseError(Err, Err.Number, CLASS_NAME)
    End Function
    Public Sub SetPatPmtProcessed(ByVal lngID As Long, ByVal lngTxID As Long)
        '--------------------------------------------------------------------
        'Date: 08/30/2000
        'Author: Eric Pena
        'Description:  Updates a row in the tblEncounterLog table utilizing
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
            .CommandText = "uspUpdPatPmtLogProcessed"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatPmtID", adInteger, adParamInput, , lngID))
            .Parameters.Append(.CreateParameter("@TxID", adInteger, adParamInput, , lngTxID))
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

    Public Sub ReBill(ByVal lngID As Long)
        '--------------------------------------------------------------------
        'Date: 08/30/2000
        'Author: Eric Pena
        'Description:  Updates a row in the tblEncounterLog table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: Null
        '--------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        CancelBill(lngID)

        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdReBill"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@BillID", adInteger, adParamInput, , lngID))
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
        cmdSQL.Execute()
        cnnSQL = Nothing
        'RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, cmdSQL.Parameters("@ErrorMsg").Value)
        cmdSQL = Nothing
    End Sub
    Public Sub CancelBill(ByVal lngID As Long)
        '--------------------------------------------------------------------
        'Date: 08/30/2000
        'Author: Eric Pena
        'Description:  Updates a row in the tblEncounterLog table utilizing
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
            .CommandText = "uspUpdCancelBill"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@BillID", adInteger, adParamInput, , lngID))
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
        cmdSQL.Execute()
        cnnSQL = Nothing
        'RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, cmdSQL.Parameters("@ErrorMsg").Value)
        cmdSQL = Nothing
    End Sub
    Public Sub Delete(ByVal lngELID As Long, Optional ByVal blnKeepAppointment As Boolean = False)
        '--------------------------------------------------------------------
        'Date: 09/16/2001                                                   '
        'Author: Eric Pena                                   '
        'Description:  Deletes all billing records associated with the specified encounter
        'Parameters: lngELID - ID of row in tblEncounterLog to delete records for
        '--------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspDelBill"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@BillID", adInteger, adParamInput, , lngELID))
            .Parameters.Append(.CreateParameter("@KeepAppt", adVarChar, adParamInput, 1, IIf(blnKeepAppointment, "Y", "N")))
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
        'err.raise(cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, Err.Description)
    End Sub
End Class
