
'------------------------------------------------------------------------------------
'Class Name: CTxDB
'Date: 09/11/2000
'Author: Juan Castro
'Denis J Finkel
'Description:   MTS object designed to host methods associated with
'               data affecting the tblTransaction table.
'------------------------------------------------------------------------------------
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

Public Class CTxDB




    Private _ConnectionString As String = String.Empty

    Private Const CLASS_NAME As String = "CTxDB"
    Private Const TABLE_NAME As String = "tblTransaction"
    Private Const CONST_TYPE_DIS As Long = 7

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property



    Public Function Insert(ByVal lngBRID As Long, ByVal lngInsuranceID As Long, ByVal lngCPCID As Long, ByVal lngPlanID As Long, ByVal lngRPID As Long, _
                        ByVal lngELID As Long, ByVal lngEncDetailID As Long, ByVal lngPatientID As Long, ByVal strPostedBy As String, ByVal lngTxTypeID As Long, ByVal strType As String, _
                        ByVal dteReceiveDate As Date, ByVal lngCPTRecordID As Long, ByVal curAmount As Decimal, ByVal curDisAmount As Decimal, ByVal strCheckNum As String, ByVal dteCheckDate As Date, _
                        ByVal lngCheckID As Long, ByVal strTxNotes As String) As Long
        '------------------------------------------------------------------------------------
        'Date:          07/25/2002
        'Author:        Eric Pena
        'Description:   Inserts a row into the tblTransaction table utilizing a stored procedure.
        'Parameters:    Each parameter identifies the column value that will be inserted.
        'Returns:       ID (Primary Key) of the row inserted.
        '------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object
        cmdSQL = New ADODB.Command

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspInsTx"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@BRID", adInteger, adParamInput, , lngBRID))
            .Parameters.Append(.CreateParameter("@InsuranceID", adInteger, adParamInput, , lngInsuranceID))
            .Parameters.Append(.CreateParameter("@CPCID", adInteger, adParamInput, , lngCPCID))
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID))
            .Parameters.Append(.CreateParameter("@RPID", adInteger, adParamInput, , lngRPID))
            .Parameters.Append(.CreateParameter("@ELID", adInteger, adParamInput, , lngELID))
            .Parameters.Append(.CreateParameter("@EncDetailID", adInteger, adParamInput, , lngEncDetailID))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@PostedBy", adVarChar, adParamInput, 30, strPostedBy))
            .Parameters.Append(.CreateParameter("@TxTypeID", adInteger, adParamInput, , lngTxTypeID))
            .Parameters.Append(.CreateParameter("@Type", adVarChar, adParamInput, 1, strType))
            .Parameters.Append(.CreateParameter("@CPTRecordID", adInteger, adParamInput, , lngCPTRecordID))
            .Parameters.Append(.CreateParameter("@Amount", adDecimal, adParamInput, , curAmount))
            .Parameters.Append(.CreateParameter("@ReceiveDate", adDBTimeStamp, adParamInput, , IIf(dteReceiveDate.ToOADate() = 0, DBNull.Value, dteReceiveDate)))
            .Parameters.Append(.CreateParameter("@CheckNum", adVarChar, adParamInput, 30, Trim(strCheckNum)))
            .Parameters.Append(.CreateParameter("@CheckDate", adDBTimeStamp, adParamInput, , IIf(dteCheckDate.ToOADate() = 0, DBNull.Value, dteCheckDate)))
            .Parameters.Append(.CreateParameter("@CheckID", adInteger, adParamInput, , IIf(lngCheckID < 0, DBNull.Value, lngCheckID)))
            .Parameters.Append(.CreateParameter("@TXNotes", adVarChar, adParamInput, 255, Trim(strTxNotes)))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        Insert = cmdSQL.Parameters("@ID").Value

        'Check the SQLErrorNum before deciding to commit the transaction.
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Or (cmdSQL.Parameters("@SQLErrorNum").Value.IsNullorEmpty) Then
            Exit Function
        End If

        'Close the connection
        cnnSQL.Close()

        'Free all resources
        cnnSQL = Nothing
        cmdSQL = Nothing

        'post the disallow if need be
        If curDisAmount <> 0 And lngTxTypeID <> CONST_TYPE_DIS Then
            Insert(lngBRID, lngInsuranceID, lngCPCID, lngPlanID, lngRPID, lngELID, lngEncDetailID, lngPatientID, strPostedBy, CONST_TYPE_DIS, "P", dteReceiveDate, lngCPTRecordID, curDisAmount, _
                         0, strCheckNum, dteCheckDate, lngCheckID, strTxNotes)
        End If

        'Signal successful completion



    End Function
    Public Sub Delete(ByVal lngInsuranceTxID As Long, ByVal lngBRID As Long, ByVal lngELID As Long, ByVal curAmount As Decimal, _
                    ByVal lngTxTypeID As Long, ByVal lngTxGroupID As Long, ByVal strType As String, ByVal lngCheckID As Long)
        '------------------------------------------------------------------------------------
        'Date:          07/29/2002
        'Author:        Eric Pena
        'Description:   Deletes a row into tblInsuranceTx and reverts derived columns utilizing a stored procedure
        'Parameters:    lngInsuranceTxID - PK of row to delete
        '               lngBRID - PK in tblBillingresponsibility of row to recalculate derived balances
        '               lngELID - PK in tblEncounterLog of row to recalculate derived balances
        '               other parameters - information about row being deleted
        '------------------------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object
        cmdSQL = New ADODB.Command

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspDelTx"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@InsTxID", adInteger, adParamInput, , lngInsuranceTxID))
            .Parameters.Append(.CreateParameter("@BRID", adInteger, adParamInput, , lngBRID))
            .Parameters.Append(.CreateParameter("@ELID", adInteger, adParamInput, , lngELID))
            .Parameters.Append(.CreateParameter("@Amount", adDecimal, adParamInput, , curAmount))
            .Parameters.Append(.CreateParameter("@TxTypeID", adInteger, adParamInput, , lngTxTypeID))
            .Parameters.Append(.CreateParameter("@TxGroupID", adInteger, adParamInput, , lngTxGroupID))
            .Parameters.Append(.CreateParameter("@Type", adVarChar, adParamInput, 1, strType))
            .Parameters.Append(.CreateParameter("@CheckID", adInteger, adParamInput, , lngCheckID))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check the SQLErrorNum before deciding to commit the transaction.
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Or (cmdSQL.Parameters("@SQLErrorNum").Value.IsNullorEmpty) Then
            Exit Sub
        End If

        'Close the connection
        cnnSQL.Close()

        'Free all resources
        cnnSQL = Nothing
        cmdSQL = Nothing

        'Signal successful completion




    End Sub

    Public Function Fetch(ByVal lngELID As Long, ByVal intType As Integer) As ADODB.Recordset
        '-------------------------------------------------------------------------------------
        'Date: 10/04/00
        'Author: Eric Pena
        'Description:   Retrieves records from the tblTransaction utilizing a stored procedure.
        'Parameters:    lngELID- the bill/encounter to return records for.
        '                       intType - the type of transactions to search for:
        '                                = 1 for CHARGE transactions only
        '                                = 2 for POSTING transactions only
        '                                = anything else for ALL transactions
        'Returns: ADODB.Recordset
        '-------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim cmdSQL As ADODB.Command



        rstSQL = New ADODB.Recordset
        cmdSQL = New ADODB.Command
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)
        cmdSQL.ActiveConnection = cnnSQL

        'create the parameter objects
        With cmdSQL
            .CommandText = "uspSelTx"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamInput, , lngELID))
            .Parameters.Append(.CreateParameter("@RecordType", adInteger, adParamInput, , intType))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Execute the stored procedure
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        Fetch = rstSQL

        'disconnect the recordset
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        rstSQL.ActiveConnection = Nothing
        cnnSQL = Nothing

        'Signal successful completion


    End Function
    Public Function FetchBRByID(ByVal lngBRID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 5/01/2000
        'Author: Eric Pena
        'Description:  Returns rows from tblBillingResponsibility utilizing a stored procedure
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
            .CommandText = "uspSelBRByID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@BRID", adInteger, adParamInput, , lngBRID))

            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchBRByID = rs

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rs.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion


    End Function


    Public Function FetchBRByEL(ByVal lngEncounterLogID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 5/01/2000
        'Author: Eric Pena
        'Description:  Returns rows from tblBillingResponsibility utilizing a stored procedure
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
            .CommandText = "uspSelBR"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ELID", adInteger, adParamInput, , lngEncounterLogID))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchBRByEL = rs

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rs.ActiveConnection = Nothing
        cnn = Nothing


    End Function
    Public Sub TransferResponsibility(ByVal lngBRIDSource As Long, ByVal lngPlanID As Long, ByVal lngRPID As Long, ByVal lngBRIDDest As Long, ByVal lngNextRPID As Long, _
                ByVal lngNextPlanID As Long, ByVal curAmount As Decimal, ByVal lngELID As Long, ByVal lngPatientID As Long, ByVal lngPos As Long, ByVal lngTransferType As Long, _
                ByVal strUserName As String, ByVal lngPatientApptID As Long, ByVal lngProviderID As Long, ByVal dteDOS As Date)
        '--------------------------------------------------------------------
        'Date: 02/24/2000
        'Author: Eric Pena
        'Description:  Moves the responsibility from one entry in tblBillingresponsibility to another
        'Parameters: lngBRIDFrom -  key of the BR record to move resp. from.
        '                   lngBRIDTo -  key of the BR record to move resp. to or -1 to move all remaining balances to the next payer
        '                   curAmount  - The amount of resp. to move
        '                   lngPos  - If positive, the location (source/dest) of the RP record.  Used in determining how much to alter charge in tblPatientTx
        'Returns: Null
        '--------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdBRAmtExpected"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@BRIDFrom", adInteger, adParamInput, , lngBRIDSource))
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID))
            .Parameters.Append(.CreateParameter("@RPID", adInteger, adParamInput, , lngRPID))
            .Parameters.Append(.CreateParameter("@BRIDTo", adInteger, adParamInput, , lngBRIDDest))
            .Parameters.Append(.CreateParameter("@NextRPID", adInteger, adParamInput, , lngNextRPID))
            .Parameters.Append(.CreateParameter("@NextPlanID", adInteger, adParamInput, , lngNextPlanID))
            .Parameters.Append(.CreateParameter("@Amt", adDecimal, adParamInput, , curAmount))
            .Parameters.Append(.CreateParameter("@ELID", adInteger, adParamInput, , lngELID))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@Pos", adInteger, adParamInput, , lngPos))
            .Parameters.Append(.CreateParameter("@TransferType", adInteger, adParamInput, , lngTransferType))
            .Parameters.Append(.CreateParameter("@Username", adVarChar, adParamInput, 30, strUserName))

            .Parameters.Append(.CreateParameter("@PatientApptID", adInteger, adParamInput, , lngPatientApptID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@DOS", adDBTimeStamp, adParamInput, , IIf(dteDOS.ToOADate() = 0, DBNull.Value, dteDOS)))

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
    Public Function SameBill(ByVal lngBRID1 As Long, ByVal lngBRID2 As Long) As Boolean
        '--------------------------------------------------------------------
        'Date: 02/24/2000
        'Author: Eric Pena
        'Description:  Determines if two BRIDs are recorded under the same bill
        'Parameters: lngBRID1 / lngBRID2 -  key of the BR record to check
        'Returns: True if both records are under the same bill, False otherwise
        '--------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim rst As ADODB.Recordset



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        rst = New ADODB.Recordset

        With cmdSQL
            .CommandText = "uspSelBRSameBill"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@BRID", adInteger, adParamInput, , lngBRID1))
            .Parameters.Append(.CreateParameter("@BR2ID", adInteger, adParamInput, , lngBRID2))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)
        cmdSQL.ActiveConnection = cnnSQL

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        If rst.EOF And rst.BOF Then
            SameBill = False
        Else
            SameBill = True
        End If

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
            Exit Function
        End If

        'Close the connection and free all resources
        cnnSQL.Close()

        rst = Nothing
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion



    End Function

    Public Function FetchBRByPatientProvider(ByVal lngPatientID As Long, ByVal lngProviderID As Long, ByVal blnOutstandingOnlyYN As Boolean) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 9/18/2000
        'Author: Eric Pena
        'Description:  Returns rows from tblBillingResponsibility that have an outstanding balance utilizing a stored procedure
        'Parameters: lngPatientID - returns BR records for this patient only
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
            .CommandText = "uspSelBRByPatProv"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@OutstandingOnlyYN", adVarChar, adParamInput, 1, IIf(blnOutstandingOnlyYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchBRByPatientProvider = rs

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rs.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion



    End Function
    Public Function FetchForCancel(ByVal lngELID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 9/18/2000
        'Author: Eric Pena
        'Description:  Returns rows from tblBillingResponsibility that have an outstanding balance utilizing a stored procedure
        'Parameters: lngPatientID - returns BR records for this patient only
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
            .CommandText = "uspSelTxForCancel"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ELID", adInteger, adParamInput, , lngELID))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchForCancel = rs

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rs.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion



    End Function
    Public Function setAsCancelled(ByVal lngELID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 9/18/2000
        'Author: Eric Pena
        'Description:  Returns rows from tblBillingResponsibility that have an outstanding balance utilizing a stored procedure
        'Parameters: lngPatientID - returns BR records for this patient only
        'Returns: ADORst
        '--------------------------------------------------------------------
        Dim cmdSQL As ADODB.Command
        Dim cnnSQL As ADODB.Connection



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdTxSetCancel"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ELID", adInteger, adParamInput, , lngELID))
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


    End Function
    Public Function CanAdjustProvsPatBals(ByVal lngELID As Long) As Boolean
        '--------------------------------------------------------------------
        'Date: 03/23/2000
        'Author: Dave Richkun
        'Description:  Retrieves detailed information about a user
        'Parameters: lngUserID - ID of the user to retrieve info for
        '            blnIsProvider - Byref parameter that will contain True if the
        '               user is assigned the Role of Provider, otherwise False
        'Returns: ADO Recordset object containing user information
        '--------------------------------------------------------------------
        'Revision History:
        '--------------------------------------------------------------------

        Dim cnn As ADODB.Connection
        Dim cmd As ADODB.Command
        Dim rst As ADODB.Recordset



        CanAdjustProvsPatBals = False

        cmd = New ADODB.Command
        cnn = New ADODB.Connection
        rst = New ADODB.Recordset

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Prepare stored procedure parameters
        With cmd
            .CommandText = "uspSelProvLoginDateByEL"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ELID", adInteger, adParamInput, , lngELID))
        End With

        'Instantiate and populate the Recordset.
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rst.ActiveConnection = Nothing
        cnn.Close()

        cnn = Nothing
        cmd = Nothing

        If (rst.Fields("fldLastLoginDate").Value.IsNullorEmpty) Then CanAdjustProvsPatBals = True



    End Function

    Public Function FetchDenialList(ByVal lngUserID As Long, Optional ByVal blnForceIndividual As Boolean = False, Optional ByVal strOrderBy As String = "") _
                                    As ADODB.Recordset
        '----------------------------------------------------------------------------------------
        'Date: 05/16/2001                                                                       '
        'Author: Rick "Boom Boom" Segura                                                        '
        'Description:   Retrieves a recordset of Denied claims for a provider or a group of     '
        '               providers                                                               '
        'Parameters:    lngUserID - ID of Provider or ID of Office Manager                      '
        '               blnForceIndividual - In the case an Office manager is also a provider,  '
        '                   this variable forces the stored procedure to return only claims     '
        '                   where the manager is the servicing provider                         '
        '               strOrderBy - If passed, instructs the stored procedure how to order the '
        '                   returned data                                                       '
        'Returns: ADODB.Recordset                                                               '
        '----------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim cmdSQL As ADODB.Command



        rstSQL = New ADODB.Recordset
        cmdSQL = New ADODB.Command
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)
        cmdSQL.ActiveConnection = cnnSQL

        'create the parameter objects
        With cmdSQL
            .CommandText = "uspSelDenialList"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
            .Parameters.Append(.CreateParameter("@ForceIndividual", adVarChar, adParamInput, 1, IIf(blnForceIndividual, "Y", "N")))
            .Parameters.Append(.CreateParameter("@OrderBy", adVarChar, adParamInput, 250, strOrderBy))
        End With

        'Execute the stored procedure
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        FetchDenialList = rstSQL

        'disconnect the recordset
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        rstSQL.ActiveConnection = Nothing
        cnnSQL = Nothing

        'Signal successful completion



    End Function


    Public Function FetchDenialSummary(ByVal lngBillID As Long) As ADODB.Recordset
        '----------------------------------------------------------------------------------------
        'Date: 05/22/2001                                                                       '
        'Author: Rick "Boom Boom" Segura                                                        '
        'Description:   Retrieves summary information of a bill's most recent denial transaction'
        'Parameters:    lngBillID - ID of bill to fetch information for                         '
        'Returns: ADODB.Recordset                                                               '
        '----------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim cmdSQL As ADODB.Command



        rstSQL = New ADODB.Recordset
        cmdSQL = New ADODB.Command
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)
        cmdSQL.ActiveConnection = cnnSQL

        'create the parameter objects
        With cmdSQL
            .CommandText = "uspSelDenialSummary"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngBillID))
        End With

        'Execute the stored procedure
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        FetchDenialSummary = rstSQL

        'disconnect the recordset
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        rstSQL.ActiveConnection = Nothing
        cnnSQL = Nothing

        'Signal successful completion


    End Function
    Public Function FetchHoldList(ByVal lngUserID As Long, _
                                    Optional ByVal blnForceIndividual As Boolean = False, _
                                    Optional ByVal strOrderBy As String = "") _
                                    As ADODB.Recordset
        '----------------------------------------------------------------------------------------
        'Date: 05/16/2001                                                                       '
        'Author: Rick "Boom Boom" Segura                                                        '
        'Description:   Retrieves a recordset of Denied claims which are being held for a       '
        '               provider or a group of providers                                        '
        'Parameters:    lngUserID - ID of Provider or ID of Office Manager                      '
        '               blnForceIndividual - In the case an Office manager is also a provider,  '
        '                   this variable forces the stored procedure to return only claims     '
        '                   where the manager is the servicing provider                         '
        '               strOrderBy - If passed, instructs the stored procedure how to order the '
        '                   returned data                                                       '
        'Returns: ADODB.Recordset                                                               '
        '----------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim cmdSQL As ADODB.Command



        rstSQL = New ADODB.Recordset
        cmdSQL = New ADODB.Command
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)
        cmdSQL.ActiveConnection = cnnSQL

        'create the parameter objects
        With cmdSQL
            .CommandText = "uspSelHoldList"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
            .Parameters.Append(.CreateParameter("@ForceIndividual", adVarChar, adParamInput, 1, IIf(blnForceIndividual, "Y", "N")))
            .Parameters.Append(.CreateParameter("@OrderBy", adVarChar, adParamInput, 250, strOrderBy))
        End With

        'Execute the stored procedure
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        FetchHoldList = rstSQL

        'disconnect the recordset
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        rstSQL.ActiveConnection = Nothing
        cnnSQL = Nothing

        'Signal successful completion



    End Function

    Public Function FetchPatWebPmt(ByVal lngPatientID As Long, ByVal lngUserID As Long) As ADODB.Recordset
        '-------------------------------------------------------------------------------------
        'Date: 06/13/2001
        'Author: Eric Pena
        'Description:   Retrieves patient web payments (adjustments) from tblTransaction utilizing a stored procedure.
        'Parameters:    lngPatientID - the bill/encounter to return records for.
        '               lngUserID - the ID of the user requesting payments.  If this user is a
        '                       office manager, records will be returned for all providers they service
        'Returns: ADODB.Recordset
        '-------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim cmdSQL As ADODB.Command



        rstSQL = New ADODB.Recordset
        cmdSQL = New ADODB.Command
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)
        cmdSQL.ActiveConnection = cnnSQL

        'create the parameter objects
        With cmdSQL
            .CommandText = "uspSelPatWebPmt"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
        End With

        'Execute the stored procedure
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        FetchPatWebPmt = rstSQL

        'disconnect the recordset
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        rstSQL.ActiveConnection = Nothing
        cnnSQL = Nothing

        'Signal successful completion


    End Function
    Public Function getWOData(ByVal lngProviderID As Long, ByVal dteWOOnBefore As Date) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 06/22/2001
        'Author: Eric Pena
        'Description:  Returns posting paramaters to use in writing off outstanding claims billed and serviced on/before
        '                   the specified date for the specified provider
        'Parameters: lngProviderID - Provider to search outstanding claims for
        '           dteWOOnBefore - search for claims billed/serviced before this date
        'Returns:
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
            .CommandText = "uspSelWOData"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@WODate", adDBTimeStamp, adParamInput, , dteWOOnBefore))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        getWOData = rs

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rs.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion

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
        '*** This doesnt belong here, but im on a deadline.
        'TODO: move this where it belongs

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



    End Function
    Public Function getDelinquentDenials(ByVal lngProviderID As Long, ByVal lngDaysOld As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/06/2001
        'Author: Eric Pena
        'Description:  Returns posting paramaters to use in writing off denied w/no activity in over the specified # of days
        '                   for the specified provider (-1 for all)
        'Parameters: lngProviderID - Provider to search outstanding claims for (-1 for all)
        '           lngDaysOld - # of inactivity days to filter for
        'Returns:
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
            .CommandText = "uspSelDelinquentDenials"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@DaysOld", adInteger, adParamInput, , lngDaysOld))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        getDelinquentDenials = rs

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rs.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion



    End Function
    Public Function FetchByID(ByVal lngTxID As Long) As ADODB.Recordset
        '-------------------------------------------------------------------------------------
        'Date: 03/11/2002
        'Author: Eric Pena
        'Description:   Retrieves records from the tblTransaction utilizing a stored procedure.
        'Parameters:    lngELID- the PK to return records for.
        'Returns: ADODB.Recordset
        '-------------------------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim cmdSQL As ADODB.Command



        rstSQL = New ADODB.Recordset
        cmdSQL = New ADODB.Command
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)
        cmdSQL.ActiveConnection = cnnSQL

        'create the parameter objects
        With cmdSQL
            .CommandText = "uspSelTxByID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamInput, , lngTxID))
        End With

        'Execute the stored procedure
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        FetchByID = rstSQL

        'disconnect the recordset
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        rstSQL.ActiveConnection = Nothing
        cnnSQL = Nothing

        'Signal successful completion


    End Function
    Public Sub UpdateAmount(ByVal lngTxID As Long, ByVal curAmount As Decimal)
        '------------------------------------------------------------------------------------
        'Date: 03/12/2002
        'Author: Eric Pena
        'Description:   Updates a row into the tblTransaction table utilizing a stored procedure.
        'Parameters:    lngTxID - PK in tblTransaction
        '               curAmount - amt to adjust by
        'NOTE:          ONLY INTENDED FOR PATIENT PAYMENTS
        '------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object
        cmdSQL = New ADODB.Command

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspUpdTxAmt"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@Amount", adDecimal, adParamInput, , curAmount))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check the SQLErrorNum before deciding to commit the transaction.
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        'Close the connection
        cnnSQL.Close()

        'Free all resources
        cnnSQL = Nothing
        cmdSQL = Nothing

        'Signal successful completion



    End Sub
    Public Sub UpdateInfo(ByVal lngTxID As Long, ByVal lngTxTypeID As Long, ByVal strCheckNum As String, ByVal dteCheckDate As Date, ByVal strNotes As String)
        '------------------------------------------------------------------------------------
        'Date: 03/12/2002
        'Author: Eric Pena
        'Description:   Updates a row into the tblTransaction table utilizing a stored procedure.
        'Parameters:    lngTxID - PK in tblTransaction
        '               otehr params are tx info that is ok to edit (except type - cannot change to ins/pat pmt)
        '------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object
        cmdSQL = New ADODB.Command

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspUpdTxInfo"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@TxType", adInteger, adParamInput, , lngTxTypeID))
            .Parameters.Append(.CreateParameter("@CheckNum", adVarChar, adParamInput, 30, Trim(strCheckNum)))
            .Parameters.Append(.CreateParameter("@CheckDate", adDBTimeStamp, adParamInput, , IIf(CLng(dteCheckDate.ToOADate()) = 0, DBNull.Value, dteCheckDate)))
            .Parameters.Append(.CreateParameter("@Note", adVarChar, adParamInput, 255, strNotes))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check the SQLErrorNum before deciding to commit the transaction.
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        'Close the connection
        cnnSQL.Close()

        'Free all resources
        cnnSQL = Nothing
        cmdSQL = Nothing

        'Signal successful completion




    End Sub


End Class


