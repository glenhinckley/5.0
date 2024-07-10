
'--------------------------------------------------------------------
'Class Name: CNoPostDB
'Date: 09/06/2001
'Author: Eric Pena
'Description:  MTS object designed to host methods associated with
'              data affecting the tblNonPostable table.
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


Public Class CNoPostDB


    Private _ConnectionString As String = String.Empty

    Private Const CLASS_NAME As String = "CNoPostDB"
    Private Const TABLE_NAME As String = "tblNonPostable"

    Public Function Insert(ByVal lngCheckID As Long, ByVal curPostAmount As Decimal, ByVal curAmtDisallow As Decimal, ByVal curAmtPatient As Decimal, _
                            ByVal lngPatientID As Long, ByVal lngProviderID As Long, ByVal lngInsuranceID As Long, ByVal dteDOS As Date, ByVal strCPT As String, _
                            ByVal strReason As String, ByVal lngDupeCheck As Long, ByVal dteAction As Date, ByVal strAction As String, ByVal lngClaimID As Long, _
                            ByVal strDupeCheckNum As String, ByVal dteDupeCheckDate As Date, ByVal strNotes As String, ByVal strUserName As String) As Long
        '--------------------------------------------------------------------
        'Date: 09/06/2001
        'Author: Eric Pena
        'Description:  Inserts a row into the tblNonPostable table utilizing a stored procedure.
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
            .CommandText = "uspInsNonPostable"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@NPID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@CheckID", adInteger, adParamInput, , IIf(lngCheckID < 0, DBNull.Value, lngCheckID)))
            .Parameters.Append(.CreateParameter("@PostAmount", adDecimal, adParamInput, , curPostAmount))
            .Parameters.Append(.CreateParameter("@AmtDisallow", adDecimal, adParamInput, , curAmtDisallow))
            .Parameters.Append(.CreateParameter("@AmtPatient", adDecimal, adParamInput, , curAmtPatient))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , IIf(lngPatientID < 0, DBNull.Value, lngPatientID)))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , IIf(lngProviderID < 0, DBNull.Value, lngProviderID)))
            .Parameters.Append(.CreateParameter("@InsuranceID", adInteger, adParamInput, , IIf(lngInsuranceID < 0, DBNull.Value, lngInsuranceID)))
            .Parameters.Append(.CreateParameter("@DOS", adDBTimeStamp, adParamInput, , IIf(dteDOS.ToOADate() = 0, DBNull.Value, dteDOS)))
            .Parameters.Append(.CreateParameter("@CPT", adVarChar, adParamInput, 5, IIf(strCPT = "", DBNull.Value, strCPT)))

            .Parameters.Append(.CreateParameter("@Reason", adVarChar, adParamInput, 50, strReason))
            .Parameters.Append(.CreateParameter("@DupeCheckID", adInteger, adParamInput, , IIf(lngDupeCheck < 0, DBNull.Value, lngDupeCheck)))
            .Parameters.Append(.CreateParameter("@ActionDate", adDBTimeStamp, adParamInput, , IIf(dteAction.ToOADate() = 0, DBNull.Value, dteAction)))
            .Parameters.Append(.CreateParameter("@Action", adVarChar, adParamInput, 50, strAction))
            .Parameters.Append(.CreateParameter("@ClaimID", adInteger, adParamInput, , IIf(lngClaimID < 0, DBNull.Value, lngClaimID)))
            .Parameters.Append(.CreateParameter("@DupeCheckNum", adVarChar, adParamInput, 20, strDupeCheckNum))
            .Parameters.Append(.CreateParameter("@DupeCheckDate", adDBTimeStamp, adParamInput, , IIf(dteDupeCheckDate.ToOADate() = 0, DBNull.Value, dteDupeCheckDate)))

            .Parameters.Append(.CreateParameter("@Notes", adVarChar, adParamInput, 256, strNotes))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        Insert = cmdSQL.Parameters("@NPID").Value

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
    Public Sub Update(ByVal lngNPID As Long, ByVal lngOldCheckID As Long, ByVal curOldAmt As Decimal, ByVal lngCheckID As Long, ByVal curPostAmount As Decimal, ByVal curAmtDisallow As Decimal, ByVal curAmtPatient As Decimal, _
                            ByVal lngPatientID As Long, ByVal lngProviderID As Long, ByVal lngInsuranceID As Long, ByVal dteDOS As Date, ByVal strCPT As String, _
                            ByVal strReason As String, ByVal lngDupeCheck As Long, ByVal dteAction As Date, ByVal strAction As String, ByVal lngClaimID As Long, _
                            ByVal strDupeCheckNum As String, ByVal dteDupeCheckDate As Date, ByVal strNotes As String, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 09/06/2001
        'Author: Eric Pena
        'Description:  Updates a row in the tblNonPostable table utilizing a stored procedure.
        'Parameters: Each parameter identifies the column value that will be inserted.
        'Returns: Null
        '--------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdNonPostable"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@NPID", adInteger, adParamInput, , lngNPID))
            .Parameters.Append(.CreateParameter("@OldCheckID", adInteger, adParamInput, , lngOldCheckID))
            .Parameters.Append(.CreateParameter("@OldAmount", adDecimal, adParamInput, , curOldAmt))
            .Parameters.Append(.CreateParameter("@CheckID", adInteger, adParamInput, , lngCheckID))
            .Parameters.Append(.CreateParameter("@PostAmount", adDecimal, adParamInput, , curPostAmount))
            .Parameters.Append(.CreateParameter("@AmtDisallow", adDecimal, adParamInput, , curAmtDisallow))
            .Parameters.Append(.CreateParameter("@AmtPatient", adDecimal, adParamInput, , curAmtPatient))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , IIf(lngPatientID < 0, DBNull.Value, lngPatientID)))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , IIf(lngProviderID < 0, DBNull.Value, lngProviderID)))
            .Parameters.Append(.CreateParameter("@InsuranceID", adInteger, adParamInput, , IIf(lngInsuranceID < 0, DBNull.Value, lngInsuranceID)))
            .Parameters.Append(.CreateParameter("@DOS", adDBTimeStamp, adParamInput, , IIf(dteDOS.ToOADate() = 0, DBNull.Value, dteDOS)))
            .Parameters.Append(.CreateParameter("@CPT", adVarChar, adParamInput, 5, IIf(strCPT = "", DBNull.Value, strCPT)))

            .Parameters.Append(.CreateParameter("@Reason", adVarChar, adParamInput, 50, strReason))
            .Parameters.Append(.CreateParameter("@DupeCheckID", adInteger, adParamInput, , IIf(lngDupeCheck < 0, DBNull.Value, lngDupeCheck)))
            .Parameters.Append(.CreateParameter("@ActionDate", adDBTimeStamp, adParamInput, , IIf(dteAction.ToOADate() = 0, DBNull.Value, dteAction)))
            .Parameters.Append(.CreateParameter("@Action", adVarChar, adParamInput, 50, strAction))
            .Parameters.Append(.CreateParameter("@ClaimID", adInteger, adParamInput, , IIf(lngClaimID < 0, DBNull.Value, lngClaimID)))
            .Parameters.Append(.CreateParameter("@DupeCheckNum", adVarChar, adParamInput, 20, strDupeCheckNum))
            .Parameters.Append(.CreateParameter("@DupeCheckDate", adDBTimeStamp, adParamInput, , IIf(dteDupeCheckDate.ToOADate() = 0, DBNull.Value, dteDupeCheckDate)))

            .Parameters.Append(.CreateParameter("@Notes", adVarChar, adParamInput, 256, strNotes))
            .Parameters.Append(.CreateParameter("@Username", adVarChar, adParamInput, 30, strUserName))
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



    End Sub
    Public Sub Process(ByVal lngNPID As Long, ByVal blnIsRefunded As Boolean, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 09/06/2001
        'Author: Eric Pena
        'Description:  Processed a row in the tblNonPostable table
        'Parameters: lngNPID - PK of the row in the tblNonPostable table to update
        '               blnIsRefunded - If true, will mark the row as refunded.  false will mark the row as posted
        'Returns: Null
        '--------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdNonPostableAction"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@NPID", adInteger, adParamInput, , lngNPID))
            .Parameters.Append(.CreateParameter("@RefundYN", adVarChar, adParamInput, 1, IIf(blnIsRefunded, "Y", "N")))
            .Parameters.Append(.CreateParameter("@ActionBy", adVarChar, adParamInput, 30, strUserName))
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


    End Sub
    Public Sub Delete(ByVal lngID As Long, ByVal lngOldCheckID As Long, ByVal curOldAmt As Decimal, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 09/06/2001
        'Author: Eric Pena
        'Description:  Deletes a row in the tblNonPostable table
        'Parameters: lngID - ID of the row in the table whose value will be deleted.
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
            .CommandText = "uspDelNonPostable"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@NPID", adInteger, adParamInput, , lngID))
            .Parameters.Append(.CreateParameter("@OldCheckID", adInteger, adParamInput, , lngOldCheckID))
            .Parameters.Append(.CreateParameter("@OldAmount", adDecimal, adParamInput, , curOldAmt))
            .Parameters.Append(.CreateParameter("@Username", adVarChar, adParamInput, 30, strUserName))
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


    End Sub
    Public Function FetchByID(ByVal lngNPID As Long) As ADODB.Recordset
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
            .CommandText = "uspSelNonPostableByID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamInput, , lngNPID))
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
        cmd = Nothing
        rs = Nothing
        cnn = Nothing
        'cmdSQL.Execute()
        'RaiseError(Err, -1, CLASS_NAME, "SQL Server Error")
    End Function
    Public Function FetchByCriteria(ByVal dteStartAdded As Date, ByVal dteEndAdded As Date, ByVal dteStartAction As Date, ByVal dteEndAction As Date, _
                                        ByVal strAction As String, ByVal lngPatientID As Long, ByVal strCheckNum As String, ByVal lngInsuranceID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/06/2001
        'Author: Eric Pena
        'Description:  Returns rows from the tblNonPostable table utilizing a stored procedure
        'Parameters: Each parameter identifies the column value that will be filtered.
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
            .CommandText = "uspSelNonPostableByCriteria"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@StartAdded", adDBTimeStamp, adParamInput, , IIf(dteStartAdded.ToOADate() = 0, DBNull.Value, dteStartAdded)))
            .Parameters.Append(.CreateParameter("@EndAdded", adDBTimeStamp, adParamInput, , IIf(dteEndAdded.ToOADate() = 0, DBNull.Value, dteEndAdded)))
            .Parameters.Append(.CreateParameter("@Action", adVarChar, adParamInput, 50, strAction))
            .Parameters.Append(.CreateParameter("@StartAction", adDBTimeStamp, adParamInput, , IIf(dteStartAction.ToOADate() = 0, DBNull.Value, dteStartAction)))
            .Parameters.Append(.CreateParameter("@EndAction", adDBTimeStamp, adParamInput, , IIf(dteEndAction.ToOADate() = 0, DBNull.Value, dteEndAction)))

            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@CheckNum", adVarChar, adParamInput, 30, strCheckNum))
            .Parameters.Append(.CreateParameter("@InsuranceID", adInteger, adParamInput, , lngInsuranceID))

        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByCriteria = rs

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rs.ActiveConnection = Nothing
        cnn = Nothing


    End Function
    Public Function isProcessed(ByVal lngNPID As Long) As Boolean
        '--------------------------------------------------------------------
        'Date: 09/06/2001
        'Author: Eric Pena
        'Description:  Lets the er know if a given Mis. Pmt. has been processed
        'Parameters: lngMisPmtID - The ID of the MP Entry
        'Returns: T if the record is processed, F otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        Dim rstSQL As ADODB.Recordset

        isProcessed = False



        rstSQL = FetchByID(lngNPID)

        If Not rstSQL.EOF Then
            If rstSQL.Fields("fldPostedYN").Value = "Y" Or rstSQL.Fields("fldRefundYN").Value = "Y" Then isProcessed = True
        End If

        'Signal successful completion

        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        rstSQL = Nothing
        'cmdSQL.Execute()
        'RaiseError(Err, -1, CLASS_NAME, "SQL Server Error")
    End Function
End Class
