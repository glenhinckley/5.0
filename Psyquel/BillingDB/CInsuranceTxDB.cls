
'----------------------------------------------------------------------------
'Class Name: CInsuranceTxDB
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

Public Class CInsuranceTxDB


    Private _ConnectionString As String = String.Empty

    Private Const CLASS_NAME As String = "CInsuranceTxDB"





    Public Function Insert(ByVal lngELID As Long, ByVal lngEncDetailID As Long, ByVal lngPatientID As Long, _
            ByVal lngRPID As Long, ByVal lngPlanID As Long, ByVal lngCPCID As Long, _
            ByVal lngInsuranceID As Long, ByVal strTxTypeCode As String, ByVal lngTxTypeID As Long, _
            ByVal lngCptRecordID As Long, ByVal dblAmount As Double, ByVal dtRcvDate As Date, _
            ByVal strCheckNum As String, ByVal dtCheckDate As Date, ByVal strTxNotes As String, ByVal dblPMF As Double, _
            ByVal lngCheckID As Long, ByVal strUserName As String, Optional ByVal lngUserID As Integer = 0) As Long
        '--------------------------------------------------------------------
        'Date: 07/09/2002
        'Author: Dave Richkun
        'Description:  Inserts a row into the tblInsuranceTx table utilizing a stored procedure.
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
            .CommandText = "uspInsInsurTx"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@TxID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@ELID", adInteger, adParamInput, , lngELID))
            .Parameters.Append(.CreateParameter("@EncDetailID", adInteger, adParamInput, , lngEncDetailID))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@RPID", adInteger, adParamInput, , lngRPID))
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID))
            .Parameters.Append(.CreateParameter("@CPCID", adInteger, adParamInput, , IIf(lngCPCID > 0, lngCPCID, DBNull.Value)))
            .Parameters.Append(.CreateParameter("@InsuranceID", adInteger, adParamInput, , lngInsuranceID))
            .Parameters.Append(.CreateParameter("@TxTypeCode", adVarChar, adParamInput, 1, strTxTypeCode))
            .Parameters.Append(.CreateParameter("@TxTypeID", adInteger, adParamInput, , lngTxTypeID))
            .Parameters.Append(.CreateParameter("@CptRecordID", adInteger, adParamInput, , lngCptRecordID))
            .Parameters.Append(.CreateParameter("@Amount", adDouble, adParamInput, , dblAmount))
            .Parameters.Append(.CreateParameter("@RcvDate", adDBTimeStamp, adParamInput, , dtRcvDate))
            .Parameters.Append(.CreateParameter("@CheckNum", adVarChar, adParamInput, 30, strCheckNum))
            .Parameters.Append(.CreateParameter("@CheckDate", adDBTimeStamp, adParamInput, , IIf(dtCheckDate.ToOADate() = 0, DBNull.Value, dtCheckDate)))
            .Parameters.Append(.CreateParameter("@TxNotes", adVarChar, adParamInput, 255, strTxNotes))
            .Parameters.Append(.CreateParameter("@PMF", adDouble, adParamInput, , dblPMF))
            .Parameters.Append(.CreateParameter("@CheckID", adInteger, adParamInput, , IIf(lngCheckID = 0, DBNull.Value, lngCheckID)))
            .Parameters.Append(.CreateParameter("@PostedBy", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , IIf(lngUserID > 0, lngUserID, DBNull.Value)))
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
        cmdSQL.Execute()

        'RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, Err.Description)
        cmdSQL = Nothing
        cnnSQL = Nothing

    End Function

    Public Sub Update(ByVal lngELID As Long, ByVal lngEncDetailID As Long, ByVal lngPlanID As Long, _
            ByVal lngTxTypeID As Long, ByVal lngCptRecordID As Long, ByVal dblAmount As Double, _
            ByVal dtRcvDate As Date, ByVal strCheckNum As String, _
            ByVal dtCheckDate As Date, ByVal strTxNotes As String, _
            ByVal dblPMF As Double, ByVal lngCheckID As Long, _
            ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 08/02/2002
        'Author: Dave Richkun
        'Description:  Updates a row in tblInsuranceTx table utilizing a stored procedure.
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
            .CommandText = "uspUpdInsurTx"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ELID", adInteger, adParamInput, , lngELID))
            .Parameters.Append(.CreateParameter("@EncDetailID", adInteger, adParamInput, , lngEncDetailID))
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID))
            .Parameters.Append(.CreateParameter("@TxTypeID", adInteger, adParamInput, , lngTxTypeID))
            .Parameters.Append(.CreateParameter("@CptRecordID", adInteger, adParamInput, , lngCptRecordID))
            .Parameters.Append(.CreateParameter("@Amount", adDouble, adParamInput, , dblAmount))
            .Parameters.Append(.CreateParameter("@ReceiveDate", adDBTimeStamp, adParamInput, , IIf(dtRcvDate.ToOADate() = 0, Date.Now, dtRcvDate)))
            .Parameters.Append(.CreateParameter("@CheckNum", adVarChar, adParamInput, 30, strCheckNum))
            .Parameters.Append(.CreateParameter("@CheckDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtCheckDate.ToOADate()) = 0, DBNull.Value, dtCheckDate)))
            .Parameters.Append(.CreateParameter("@TxNotes", adVarChar, adParamInput, 255, strTxNotes))
            .Parameters.Append(.CreateParameter("@CollectedPMF", adDouble, adParamInput, , dblPMF))
            .Parameters.Append(.CreateParameter("@CheckID", adInteger, adParamInput, , IIf(lngCheckID = 0, DBNull.Value, lngCheckID)))
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
        cmdSQL.Execute()

        'RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, Err.Description)
        cmdSQL = Nothing
        cnnSQL = Nothing

    End Sub


    Public Sub CancelAfterSubmit(ByVal lngELID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 08/08/2002
        'Author: Dave Richkun
        'Description:  Inserts an offsetting transaction into tblInsuranceTx that offsets
        '              the insurance balance owing to a zero amount for a particular encounter.
        'Parameters:  lngELID - ID of the encounter for which insurance transactions are being
        '                zeroed out.
        '             strUserName - Name of user performing the cancellation
        'Returns: Null
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspInsZeroOutInsurTx"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ELID", adInteger, adParamInput, , lngELID))
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
        cmdSQL.Execute()

        'RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, Err.Description)
        cmdSQL = Nothing
        cnnSQL = Nothing

    End Sub

    Public Sub Resubmit(ByVal lngOrigELID As Long, ByVal lngNewELID As Long)
        '--------------------------------------------------------------------
        'Date: 08/09/2002
        'Author: Dave Richkun
        'Description:  Reassigns the Encounter Log ID foreign key from one set
        '              of insurance transactions to a new Encounter Log ID.  This
        '              method is used when a claim is resubmitted and a new encounter
        '              is created.  By reassigning the encounter log ID, the original
        '              insurance transactions are preserved when assigned to the new
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
            .CommandText = "uspUpdInsTxResubmit"
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


End Class
