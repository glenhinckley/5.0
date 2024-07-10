
'----------------------------------------------------------------------------
'Class Name: CPatientTxDB
'Date: 07/09/2002
'Author: Dave Richkun
'Description:  COM+ object designed to host methods associated with patient
'              transactions.
'----------------------------------------------------------------------------
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


Public Class CPatientTXDB

    Private Const CLASS_NAME As String = "CPatientTxDB"
    Private _ConnectionString As String = String.Empty


    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property


    Public Function Insert(ByVal lngELID As Long, ByVal lngPatientID As Long, _
            ByVal lngProviderID As Long, ByVal lngRPID As Long, ByVal strTxTypeCode As String, _
            ByVal lngTxTypeID As Long, ByVal dtPostDate As Date, ByVal dtRcvDate As Date, _
            ByVal dblAmount As Double, ByVal strCheckNum As String, ByVal dtCheckDate As Date, _
            ByVal strCCTxID As String, ByVal strTxNotes As String, ByVal lngPatApptID As Long, ByVal strUserName As String) As Long
        '--------------------------------------------------------------------
        'Date: 07/09/2002
        'Author: Dave Richkun
        'Description:  Inserts a row into the tblPatientTx table utilizing a stored procedure.
        'Parameters:
        '
        '   *** TO DO: Define each parameter once procedure is finalized ****
        '
        'Returns: ID of the row inserted.
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspInsPatientTx"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@TxID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@ELID", adInteger, adParamInput, , IIf(lngELID > 0, lngELID, DBNull.Value)))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@RPID", adInteger, adParamInput, , lngRPID))
            .Parameters.Append(.CreateParameter("@TxTypeCode", adVarChar, adParamInput, 1, strTxTypeCode))
            .Parameters.Append(.CreateParameter("@TxTypeID", adInteger, adParamInput, , lngTxTypeID))
            .Parameters.Append(.CreateParameter("@PostDate", adDBTimeStamp, adParamInput, , dtPostDate))
            .Parameters.Append(.CreateParameter("@RcvDate", adDBTimeStamp, adParamInput, , dtRcvDate))
            .Parameters.Append(.CreateParameter("@Amount", adDouble, adParamInput, , dblAmount))
            .Parameters.Append(.CreateParameter("@CheckNum", adVarChar, adParamInput, 30, strCheckNum))
            .Parameters.Append(.CreateParameter("@CheckDate", adDBTimeStamp, adParamInput, , IIf(dtCheckDate.ToOADate() = 0, DBNull.Value, dtCheckDate)))
            .Parameters.Append(.CreateParameter("@CCTxID", adVarChar, adParamInput, 60, strCCTxID))
            .Parameters.Append(.CreateParameter("@TxNotes", adVarChar, adParamInput, 255, strTxNotes))
            .Parameters.Append(.CreateParameter("@LockedYN", adVarChar, adParamInput, 1, "N"))
            .Parameters.Append(.CreateParameter("@PatApptID", adInteger, adParamInput, , IIf(lngPatApptID > 0, lngPatApptID, DBNull.Value)))
            .Parameters.Append(.CreateParameter("@PostedBy", adVarChar, adParamInput, 30, strUserName))
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

        Insert = cmdSQL.Parameters("@TxID").Value

        'Close the connection
        cnnSQL.Close()

        'Free all resources
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, Err.Description)
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()

    End Function


    Public Function Fetch(ByVal lngPatientID As Long, ByVal lngProviderID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/06/2001
        'Author: Eric Pena
        'Description:  Returns rows from the tblNonPostable table utilizing a stored procedure
        'Parameters: PK for row in tblNonPostable that will be fetched.
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
            .CommandText = "uspSelPatientTx"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
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
        cmd = Nothing
        rs = Nothing
        cnn = Nothing
        'cmdSQL.Execute()
        'RaiseError(Err, -1, CLASS_NAME, "SQL Server Error")
    End Function

    Public Function FetchByID(ByVal lngTxID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/29/2002
        'Author: Dave Richkun
        'Description:  Returns detailed patient transaction information
        '              associated with a specific transaction
        'Parameters: lngTxID - ID of patient transaction record to which
        '               transactions are associated
        'Returns: Recordset of patient payment details
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
            'Create the parameter objects
            .CommandText = "uspSelPatientTxByID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@TxID", adInteger, adParamInput, , lngTxID))
        End With

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


        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        cmd = Nothing
        rst = Nothing
        cnn = Nothing
        'cmdSQL.Execute()

        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function


    Public Function FetchByELID(ByVal lngELID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/23/2002
        'Author: Dave Richkun
        'Description:  Returns detailed patient transaction information
        '              associated with an encounter record
        'Parameters: lngELID - ID of encounter record to which transactions are associated
        'Returns: Recordset of patient payment details
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
            'Create the parameter objects
            .CommandText = "uspSelPatientTxByELID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ELID", adInteger, adParamInput, , lngELID))
        End With

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByELID = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        cmd = Nothing
        rst = Nothing
        cnn = Nothing
        'cmdSQL.Execute()

        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function


    Public Function FetchByPatientUser(ByVal lngPatientID As Long, ByVal lngUserID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/26/2002
        'Author: Dave Richkun
        'Description:  Returns patient transaction information associated with a
        '              patient, considering the user executing the method.  Results
        '              may vary whether the user is a provider or an Office Manager.
        'Parameters: lngPatientID - ID of patient to which transactions are associated
        '            lngUserID - ID of user executing this method
        'Returns: Recordset of patient transactions
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
            'Create the parameter objects
            .CommandText = "uspSelPatTxByPatUser"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
        End With

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByPatientUser = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        cmd = Nothing
        rst = Nothing
        cnn = Nothing
        'cmdSQL.Execute()

        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function


    Public Function FetchByPatientProvUser(ByVal lngPatientID As Long, ByVal lngUserID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/28/2002
        'Author: Dave Richkun
        'Description:  Returns patient transaction information associated with a
        '              patient ordered by Providers serviced by the user.
        'Parameters: lngPatientID - ID of patient to which transactions are associated
        '            lngUserID - ID of user executing this method
        'Returns: Recordset of patient transactions
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
            'Create the parameter objects
            .CommandText = "uspSelPatTxByProvUser"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
        End With

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByPatientProvUser = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        cmd = Nothing
        rst = Nothing
        cnn = Nothing
        'cmdSQL.Execute()

        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function


    Public Sub UpdateDeleteForELID(ByVal lngELID As Long, ByVal strTxIDs As String)
        '--------------------------------------------------------------------
        'Date: 07/25/2002
        'Author: Dave Richkun
        'Description:  Physiy deletes transaction rows where the TxID is included
        '              in the varTxIDs array.  Any transactions associated with the
        '              claim that are not included in the array, have the ELID foreign
        '              key updated to NULL.  This method is ed when a user is electing
        '              to delete a claim.  The user is prompted to identify which
        '              patient transactions are to be deleted, and which transactions they
        '              would like to keep.
        'Parameters: lngELID - ID of encounter record to which transactions are associated
        '             varTxIDs - A single dimensional Object array containing the IDs of
        '               those patient transactions they user wishes to delete.
        'Returns: Null
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspUpdDelPatientTxForELID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ELID", adInteger, adParamInput, , lngELID))
            .Parameters.Append(.CreateParameter("@TxIDs", adVarChar, adParamInput, 50, strTxIDs))
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

        'Free all resources
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Sub


        'Signal incompletion and raise the error to the ing environment.
        'RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, Err.Description)

        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()

    End Sub

    Public Sub Update(ByVal lngTxID As Long, ByVal lngELID As Long, _
                    ByVal lngPatientID As Long, ByVal lngProviderID As Long, _
                    ByVal lngRPID As Long, ByVal lngTxTypeID As Long, _
                    ByVal dtRcvDate As Date, ByVal dblAmount As Double, _
                    ByVal strCheckNum As String, ByVal dtCheckDate As Date, _
                    ByVal strTxNotes As String, ByVal blnUpdatePostDateYN As Boolean, _
                    ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 07/29/2002
        'Author: Dave Richkun
        'Description:  Updates a row in tblPatientTx table utilizing a stored procedure.
        'Parameters:
        '
        '   *** TO DO: Define each parameter once procedure is finalized ****
        '
        'Returns: Null
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspUpdPatientTx"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@TxID", adInteger, adParamInput, , lngTxID))
            .Parameters.Append(.CreateParameter("@ELID", adInteger, adParamInput, , IIf(lngELID > 0, lngELID, DBNull.Value)))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@RPID", adInteger, adParamInput, , lngRPID))
            .Parameters.Append(.CreateParameter("@TxTypeID", adInteger, adParamInput, , lngTxTypeID))
            .Parameters.Append(.CreateParameter("@RcvDate", adDBTimeStamp, adParamInput, , dtRcvDate))
            .Parameters.Append(.CreateParameter("@Amount", adDouble, adParamInput, , dblAmount))
            .Parameters.Append(.CreateParameter("@CheckNum", adVarChar, adParamInput, 30, strCheckNum))
            .Parameters.Append(.CreateParameter("@CheckDate", adDBTimeStamp, adParamInput, , IIf(dtCheckDate.ToOADate() = 0, DBNull.Value, dtCheckDate)))
            .Parameters.Append(.CreateParameter("@TxNotes", adVarChar, adParamInput, 255, strTxNotes))
            .Parameters.Append(.CreateParameter("@UpdatePostDateYN", adVarChar, adParamInput, 1, IIf(blnUpdatePostDateYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@PostedBy", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

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
        'RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, Err.Description)
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()

    End Sub


    Public Sub Delete(ByVal lngTxID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 07/29/2002
        'Author: Dave Richkun
        'Description:  Physiy deletes a row in tblPatientTx table utilizing
        '              a stored procedure.
        'Parameters: lngTxID - ID of transaction to be deleted
        'Returns: Null
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspDelPatientTx"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@TxID", adInteger, adParamInput, , lngTxID))
            .Parameters.Append(.CreateParameter("@DeletedBy", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

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
        'RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, Err.Description)
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()

    End Sub


    Public Sub UpdateSelfPayCharge(ByVal lngELID As Long, ByVal dblAmount As Double, _
                                   ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 08/06/2002
        'Author: Dave Richkun
        'Description:  Updates the patient charge record associated with a self-pay claim.
        'Parameters: lngELID - ID of self-pay encounter
        '            dblAmount - The amount of the updated charge
        '            strUserName - The name of the user performing the update
        'Returns: Null
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspUpdPatTxSelfPayChg"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ELID", adInteger, adParamInput, , lngELID))
            .Parameters.Append(.CreateParameter("@Amount", adDouble, adParamInput, , dblAmount))
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

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
        'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME, Err.Description)
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()

    End Sub


    Public Sub Resubmit(ByVal lngOrigELID As Long, ByVal lngNewELID As Long)
        '--------------------------------------------------------------------
        'Date: 08/09/2002
        'Author: Dave Richkun
        'Description:  Reassigns the Encounter Log ID foreign key from one set
        '              of patient transactions to a new Encounter Log ID.  This
        '              method is used when a claim is resubmitted and a new encounter
        '              is created.  By reassigning the encounter log ID, the original
        '              patient transactions are preserved when assigned to the new
        '              encounter.
        'Parameters:  lngOrigELID - ID of the original encounter - the ID of the encounter
        '               being cancelled
        '             lngNewELID - ID of the resubmitted encounter - the ID of the new
        '               encounter that supercedes all previous encounters.
        'Returns: Null
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspUpdPatTxResubmit"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@OrigELID", adInteger, adParamInput, , lngOrigELID))
            .Parameters.Append(.CreateParameter("@NewELID", adInteger, adParamInput, , lngNewELID))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

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
        'RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, Err.Description)
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()

    End Sub

    Public Function FetchByPatApptID(ByVal lngPatApptID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/27/2002
        'Author: Dave Richkun
        'Description:  Returns patient transaction information associated with a
        '              patient appointment.
        'Parameters: lngPatApptID - ID of patient appointment to which transactions are associated
        'Returns: Recordset of patient transactions
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
            'Create the parameter objects
            .CommandText = "uspSelPatTxByPatApptID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatApptID", adInteger, adParamInput, , lngPatApptID))
        End With

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByPatApptID = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        cmd = Nothing
        rst = Nothing
        cnn = Nothing
        'cmdSQL.Execute()

        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function

End Class
