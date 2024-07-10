VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 2  'RequiresTransaction
END
Attribute VB_Name = "CBRDB"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CBRDB
'Date: 02/24/2000
'Author: Eric Pena
'Description:  MTS object designed to host methods associated with data affecting the tblBillingResponsibility table.
'--------------------------------------------------------------------
Option Explicit
Private Const CLASS_NAME As String = "CBRDB"
Private Const TABLE_NAME As String = "tblBillingResponsibility"

Public Function Insert(ByVal lngELID As Long, ByVal lngPatientID As Long, _
            ByVal lngRPID As Long, ByVal lngPlanID As Long, ByVal lngCPCID As Long, _
            ByVal lngInsuranceID As Long, ByVal strPayerCode As String, _
            ByVal intOrder As Integer, ByVal dblAmtResp As Double, _
            ByVal dblAmtAcct As Double, ByVal lngTaxID As Long, ByVal strTIN As String, _
            ByVal intTINType As Integer, ByVal strCertNum As String) As Long
'--------------------------------------------------------------------
'Date: 07/08/2002
'Author: Dave Richkun
'Description:  Inserts a row into the tblBillingResponsibility table utilizing a stored procedure.
'Parameters:
'
'   *** TO DO: Define each parameter once procedure is finalized ****
'
'Returns: ID of the row inserted.
'--------------------------------------------------------------------
    
    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command
    
    On Error GoTo ErrTrap:
    
    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    
    'Create the parameter objects
    With cmdSQL
        .CommandText = "uspInsBR"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@BRID", adInteger, adParamReturnValue)
        .Parameters.Append .CreateParameter("@ELID", adInteger, adParamInput, , lngELID)
        .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID)
        .Parameters.Append .CreateParameter("@RPID", adInteger, adParamInput, , lngRPID)
        .Parameters.Append .CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID)
        .Parameters.Append .CreateParameter("@CPCID", adInteger, adParamInput, , lngCPCID)
        .Parameters.Append .CreateParameter("@InsuranceID", adInteger, adParamInput, , lngInsuranceID)
        .Parameters.Append .CreateParameter("@PayerCode", adVarChar, adParamInput, 1, strPayerCode)
        .Parameters.Append .CreateParameter("@Order", adInteger, adParamInput, , intOrder)
        .Parameters.Append .CreateParameter("@AmtResp", adDouble, adParamInput, , dblAmtResp)
        .Parameters.Append .CreateParameter("@AmtAcct", adDouble, adParamInput, , dblAmtAcct)
        .Parameters.Append .CreateParameter("@TaxID", adInteger, adParamInput, , IIf(lngTaxID > 0, lngTaxID, Null))
        .Parameters.Append .CreateParameter("@TIN", adVarChar, adParamInput, 30, strTIN)
        .Parameters.Append .CreateParameter("@TINType", adInteger, adParamInput, , IIf(intTINType > 0, intTINType, Null))
        .Parameters.Append .CreateParameter("@CertNum", adVarChar, adParamInput, 30, strCertNum)
        .Parameters.Append .CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open(CONST_PSYQUEL_CNN)
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute , , adExecuteNoRecords

    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If
    
    Insert = cmdSQL.Parameters("@BRID").Value
    
    'Close the connection
    cnnSQL.Close
    
    'Free all resources
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing

    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Call RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, Err.Description)
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    GetObjectContext.SetAbort
    
End Function

Public Sub Delete(ByVal lngBRID As Long)
'--------------------------------------------------------------------
'Date: 03/20/2009
'Author: Duane Orth
'Description:  Physically deletes a row in tblBillingResponsibilty table utilizing
'              a stored procedure.
'Parameters: lngTxID - ID of transaction to be deleted
'Returns: Null
'--------------------------------------------------------------------
    
    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command
    
    On Error GoTo ErrTrap:
    
    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    
    'Create the parameter objects
    With cmdSQL
        .CommandText = "uspDelBR"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@BRID", adInteger, adParamInput, , lngBRID)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open(CONST_PSYQUEL_CNN)
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute , , adExecuteNoRecords

    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If
    
    'Close the connection
    cnnSQL.Close
    
    'Free all resources
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing

    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Call RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, Err.Description)
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    GetObjectContext.SetAbort

End Sub

Public Sub Update(ByVal lngBRID As Long, ByVal lngRPID As Long, ByVal lngELID As Long, ByVal lngPlanID As Long, ByVal lngCPCID As Long, _
                  ByVal lngInsuranceID As Long, ByVal dblAmtResp As Double, _
                  ByVal dblAmtAcctd As Double, ByVal strTIN As String, ByVal intTINType As Integer, _
                  ByVal lngTaxID As Long, ByVal strCertNum As String)
'--------------------------------------------------------------------
'Date: 08/02/2002
'Author: Dave Richkun
'Description:  Updates a row in tblBillingResponsibility utilizing a stored procedure.
'Parameters:
'
'  ** TO DO ** - Define parameters once procedure finalized
'
'Returns: Null
'--------------------------------------------------------------------
    
    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command

    On Error GoTo ErrTrap:
    
    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    
    With cmdSQL
        .CommandText = "uspUpdBR"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@BRID", adInteger, adParamInput, , lngBRID)
        .Parameters.Append .CreateParameter("@RPID", adInteger, adParamInput, , lngRPID)
        .Parameters.Append .CreateParameter("@ELID", adInteger, adParamInput, , lngELID)
        .Parameters.Append .CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID)
        .Parameters.Append .CreateParameter("@CPCID", adInteger, adParamInput, , lngCPCID)
        .Parameters.Append .CreateParameter("@InsuranceID", adInteger, adParamInput, , lngInsuranceID)
        .Parameters.Append .CreateParameter("@AmtResp", adDouble, adParamInput, , dblAmtResp)
        .Parameters.Append .CreateParameter("@AmtAcctd", adDouble, adParamInput, , dblAmtAcctd)
        .Parameters.Append .CreateParameter("@TIN", adVarChar, adParamInput, 30, strTIN)
        .Parameters.Append .CreateParameter("@TINType", adInteger, adParamInput, , intTINType)
        .Parameters.Append .CreateParameter("@TaxID", adInteger, adParamInput, , lngTaxID)
        .Parameters.Append .CreateParameter("@CertNum", adVarChar, adParamInput, 30, strCertNum)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open(CONST_PSYQUEL_CNN)
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute , , adExecuteNoRecords
    
    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If

    'Close the connection and free all resources
    cnnSQL.Close
    
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    
    Call RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, cmdSQL.Parameters("@ErrorMsg").Value)
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    
End Sub


Public Sub UpdatePatientAmount(ByVal lngELID As Long, ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 08/05/2002
'Author: Dave Richkun
'Description:  Updates the Amount Responsible and Amount Accountable columns
'              for patient records in tblBillingResponsibility for a given encounter
'              so the balance is consistent with transactions in tblPatientTx.
'Parameters: lngELID - ID of the encounter log for which patient balances are
'              being updated.
'Returns: Null
'--------------------------------------------------------------------

    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command

    On Error GoTo ErrTrap:
    
    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    
    With cmdSQL
        .CommandText = "uspUpdBRPatAmt"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@ELID", adInteger, adParamInput, , lngELID)
        .Parameters.Append .CreateParameter("@Username", adVarChar, adParamInput, 30, strUserName)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open(CONST_PSYQUEL_CNN)
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute , , adExecuteNoRecords
    
    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If

    'Close the connection and free all resources
    cnnSQL.Close
    
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    
    Call Err.Raise(Err.Number, Err.Source, Err.Description)
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing

End Sub


Public Sub Resubmit(ByVal lngOrigELID As Long, ByVal lngNewELID As Long)
'--------------------------------------------------------------------
'Date: 08/08/2002
'Author: Dave Richkun
'Description:  Reassigns the Encounter Log ID foreign key from the RP's billing
'              responsibility records to a new Encounter Log ID.  This
'              method is used when a claim is resubmitted and a new encounter
'              is created.  By reassigning the encounter log ID, the original
'              patient billing responsibility records are preserved when assigned
'              to the new encounter.
'Parameters:  lngOrigELID - ID of the original encounter - the ID of the encounter
'               being cancelled
'             lngNewELID - ID of the resubmitted encounter - the ID of the new
'               encounter that supercedes all previous encounters.
'Returns: Null
'--------------------------------------------------------------------

    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command

    On Error GoTo ErrTrap:
    
    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    
    With cmdSQL
        .CommandText = "uspUpdBRResubmit"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@OrigELID", adInteger, adParamInput, , lngOrigELID)
        .Parameters.Append .CreateParameter("@NewELID", adInteger, adParamInput, , lngNewELID)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open(CONST_PSYQUEL_CNN)
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute , , adExecuteNoRecords
    
    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If

    'Close the connection and free all resources
    cnnSQL.Close
    
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    
    Call Err.Raise(Err.Number, Err.Source, Err.Description)
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing

End Sub


Public Sub ZeroAll(ByVal lngELID As Long)
'--------------------------------------------------------------------
'Date: 08/05/2002
'Author: Dave Richkun
'Description:  Updates the Amount Responsible, Amount Accountable, and
'              Balance columns for all responsible parties in
'              tblBillingResponsibility for a given encounter.
'Parameters: lngELID - ID of the encounter log for which balances are
'              being updated.
'Returns: Null
'--------------------------------------------------------------------

    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command

    On Error GoTo ErrTrap:
    
    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    
    With cmdSQL
        .CommandText = "uspUpdBRZeroAll"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@ELID", adInteger, adParamInput, , lngELID)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open(CONST_PSYQUEL_CNN)
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute , , adExecuteNoRecords
    
    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If

    'Close the connection and free all resources
    cnnSQL.Close
    
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    
    Call Err.Raise(Err.Number, Err.Source, Err.Description)
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing

End Sub
Public Function Exists(ByVal lngELID As Long, ByVal lngRPID As Long, ByVal lngPlanID As Long, ByVal blnPayerIsPlan As Boolean) As Boolean
'--------------------------------------------------------------------
'Date: 02/24/2000
'Author: Eric Pena
'Description:  Determines if a BR Record containing the given parameters already exists in the table.
'Parameters: All parameters are address fields to check
'Returns: True if the BR exists, false otherwise
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------
    Dim cnnSQL As ADODB.Connection
    Dim rstSQL As ADODB.Recordset
    Dim strSQL As String
    Dim blnExists As Boolean
    Dim lngPayerID As Long

    On Error GoTo ErrTrap:

    'Prepare the SQL statement
    strSQL = "SELECT "
    strSQL = strSQL & "COUNT(*) AS TOTAL "
    strSQL = strSQL & "FROM "
    strSQL = strSQL & TABLE_NAME
    strSQL = strSQL & " WHERE "
    strSQL = strSQL & " fldEncounterLogID = " & lngELID & " AND "
    If blnPayerIsPlan Then
        strSQL = strSQL & " fldPayerID_Plan = " & lngPlanID
    Else
        strSQL = strSQL & " fldPayerID_RP = " & lngRPID
    End If
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open(CONST_PSYQUEL_CNN)

    'Instantiate and populate the Recordset.
    Set rstSQL = New ADODB.Recordset
    rstSQL.CursorLocation = adUseClient
    Call rstSQL.Open(strSQL, cnnSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)

    'Disconnect the recordset, close the connection and return the recordset
    'to the calling environment.
    Set rstSQL.ActiveConnection = Nothing

    'Close the connection and free all resources
    cnnSQL.Close
    Set cnnSQL = Nothing
    'Get the value from the Recordset then destroy it.
    If rstSQL.Fields("TOTAL").Value > 0 Then
        Exists = True
    Else
        Exists = False
    End If
    Set rstSQL = Nothing

    'Signal successful completion
    GetObjectContext.SetComplete

   Exit Function

ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set cnnSQL = Nothing
    Set rstSQL = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Public Function Fetch(ByVal lngEncounterLogID As Long) As ADODB.Recordset
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
     
     On Error GoTo ErrTrap:
     
     Set rs = New ADODB.Recordset
     Set cmd = New ADODB.Command
     Set cnn = New ADODB.Connection
     cnn.Open (CONST_PSYQUEL_CNN)
     Set cmd.ActiveConnection = cnn
     
    'Create the parameter objects
     With cmd
         .CommandText = "uspSelBR"
         .CommandType = adCmdStoredProc
         .Parameters.Append .CreateParameter("@ELID", adInteger, adParamInput, , lngEncounterLogID)
         .Parameters.Append .CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0)
     End With
    
    'Execute the stored procedure
     rs.CursorLocation = adUseClient
     rs.Open cmd, , adOpenForwardOnly, adLockReadOnly
    
     Set Fetch = rs
    
    'Disconnect the recordset
     Set cmd.ActiveConnection = Nothing
     Set cmd = Nothing
     Set rs.ActiveConnection = Nothing
     Set cnn = Nothing
     
    'Signal successful completion
     GetObjectContext.SetComplete
    
     Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
     Call RaiseError(Err, cmd.Parameters("@ErrorNum").Value, CLASS_NAME)
     Set cmd = Nothing
     Set rs = Nothing
     Set cnn = Nothing
     GetObjectContext.SetAbort
 End Function
Public Sub SwapOrder(ByVal lngID1 As Long, ByVal lngID2 As Long)
'--------------------------------------------------------------------
'Date: 02/24/2000
'Author: Eric Pena
'Description:  Swaps the fldOrder Field of two entries in tblBillingResponsibility
'Parameters: Each parameter identifies the column value that will be swapped.
'Returns: Null
'--------------------------------------------------------------------
    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command

    On Error GoTo ErrTrap:
   
    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    
    With cmdSQL
        .CommandText = "uspUpdBROrder"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@BR1ID", adInteger, adParamInput, , lngID1)
        .Parameters.Append .CreateParameter("@BR2ID", adInteger, adParamInput, , lngID2)
        .Parameters.Append .CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open(CONST_PSYQUEL_CNN)
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute , , adExecuteNoRecords
    
    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If

    'Close the connection and free all resources
    cnnSQL.Close
    
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Call RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, cmdSQL.Parameters("@ErrorMsg").Value)
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    GetObjectContext.SetAbort
End Sub
Public Sub TransferResponsibility(ByVal lngBRIDSource As Long, ByVal lngBRIDDest As Long, ByVal curAmount As Currency)
'--------------------------------------------------------------------
'Date: 02/24/2000
'Author: Eric Pena
'Description:  Moves the responsibility from one entry in tblBillingresponsibility to another
'Parameters: lngBRIDFrom -  key of the BR record to move resp. from.
'                   lngBRIDTo -  key of the BR record to move resp. to
'                   curAmount  - The amount of resp. to move
'Returns: Null
'--------------------------------------------------------------------
    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command

    On Error GoTo ErrTrap:
   
    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    
    With cmdSQL
        .CommandText = "uspUpdBRAmtExpected"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@BRIDFrom", adInteger, adParamInput, , lngBRIDSource)
        .Parameters.Append .CreateParameter("@BRIDTo", adInteger, adParamInput, , lngBRIDDest)
        .Parameters.Append .CreateParameter("@Amt", adCurrency, adParamInput, , curAmount)
        .Parameters.Append .CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open(CONST_PSYQUEL_CNN)
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute , , adExecuteNoRecords
    
    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If

    'Close the connection and free all resources
    cnnSQL.Close
    
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Call RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, cmdSQL.Parameters("@ErrorMsg").Value)
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    GetObjectContext.SetAbort
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

    On Error GoTo ErrTrap:
   
    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    Set rst = New ADODB.Recordset
    
    With cmdSQL
        .CommandText = "uspSelBRSameBill"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@BRID", adInteger, adParamInput, , lngBRID1)
        .Parameters.Append .CreateParameter("@BR2ID", adInteger, adParamInput, , lngBRID2)
        .Parameters.Append .CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open(CONST_PSYQUEL_CNN)
     Set cmdSQL.ActiveConnection = cnnSQL
    
    'Execute the stored procedure
     rst.CursorLocation = adUseClient
     rst.Open cmdSQL, , adOpenForwardOnly, adLockReadOnly
    
    If rst.EOF And rst.BOF Then
        SameBill = False
    Else
        SameBill = True
    End If
    
    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If

    'Close the connection and free all resources
    cnnSQL.Close
    
    Set rst = Nothing
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Call RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, cmdSQL.Parameters("@ErrorMsg").Value)
    Set rst = Nothing
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    GetObjectContext.SetAbort
End Function
Public Function FetchByPatient(ByVal lngPatientID As Long, ByVal blnOutstandingOnlyYN As Boolean) As ADODB.Recordset
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
     
     On Error GoTo ErrTrap:
     
     Set rs = New ADODB.Recordset
     Set cmd = New ADODB.Command
     Set cnn = New ADODB.Connection
     cnn.Open (CONST_PSYQUEL_CNN)
     Set cmd.ActiveConnection = cnn
     
    'Create the parameter objects
     With cmd
         .CommandText = "uspSelBRByPatient"
         .CommandType = adCmdStoredProc
         .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID)
         .Parameters.Append .CreateParameter("@OutstandingOnlyYN", adVarChar, adParamInput, 1, IIf(blnOutstandingOnlyYN, "Y", "N"))
         .Parameters.Append .CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0)
     End With
    
    'Execute the stored procedure
     rs.CursorLocation = adUseClient
     rs.Open cmd, , adOpenForwardOnly, adLockReadOnly
    
     Set FetchByPatient = rs
    
    'Disconnect the recordset
     Set cmd.ActiveConnection = Nothing
     Set cmd = Nothing
     Set rs.ActiveConnection = Nothing
     Set cnn = Nothing
     
    'Signal successful completion
     GetObjectContext.SetComplete
    
     Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
     Call RaiseError(Err, cmd.Parameters("@ErrorNum").Value, CLASS_NAME)
     Set cmd = Nothing
     Set rs = Nothing
     Set cnn = Nothing
     GetObjectContext.SetAbort
 End Function
Public Function FetchByPayerPatientBalance(ByVal lngInsuranceID As Long, ByVal dteStartDOS As Date, ByVal dteEndDOS As Date) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 9/18/2000
'Author: Eric Pena
'Description:  Returns rows from tblBillingResponsibility that have an outstanding balance utilizing a stored procedure
'Parameters: lngPayerID - returns BR records for this payer only
'Returns: ADORst
'--------------------------------------------------------------------
     Dim rs As ADODB.Recordset
     Dim cmd As ADODB.Command
     Dim cnn As ADODB.Connection
     
     On Error GoTo ErrTrap:
     
     Set rs = New ADODB.Recordset
     Set cmd = New ADODB.Command
     Set cnn = New ADODB.Connection
     cnn.Open (CONST_PSYQUEL_CNN)
     Set cmd.ActiveConnection = cnn
     
    'Create the parameter objects
     With cmd
         .CommandText = "uspSelBRByPayerPatientBalance"
         .CommandType = adCmdStoredProc
         .Parameters.Append .CreateParameter("@PayerID", adInteger, adParamInput, , lngInsuranceID)
         .Parameters.Append .CreateParameter("@StartDOS", adDBTimeStamp, adParamInput, , dteStartDOS)
         .Parameters.Append .CreateParameter("@EndDOS", adDBTimeStamp, adParamInput, 1, dteEndDOS)
     End With
    
    'Execute the stored procedure
     rs.CursorLocation = adUseClient
     rs.Open cmd, , adOpenForwardOnly, adLockReadOnly
    
     Set FetchByPayerPatientBalance = rs
    
    'Disconnect the recordset
     Set cmd.ActiveConnection = Nothing
     Set cmd = Nothing
     Set rs.ActiveConnection = Nothing
     Set cnn = Nothing
     
    'Signal successful completion
     GetObjectContext.SetComplete
    
     Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
     Call RaiseError(Err, cmd.Parameters("@ErrorNum").Value, CLASS_NAME)
     Set cmd = Nothing
     Set rs = Nothing
     Set cnn = Nothing
     GetObjectContext.SetAbort
 End Function
Public Function FetchByPatientProvider(ByVal lngPatientID As Long, ByVal lngProviderID As Long, ByVal blnOutstandingOnlyYN As Boolean) As ADODB.Recordset
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
     
     On Error GoTo ErrTrap:
     
     Set rs = New ADODB.Recordset
     Set cmd = New ADODB.Command
     Set cnn = New ADODB.Connection
     cnn.Open (CONST_PSYQUEL_CNN)
     Set cmd.ActiveConnection = cnn
     
    'Create the parameter objects
     With cmd
         .CommandText = "uspSelBRByPatProv"
         .CommandType = adCmdStoredProc
         .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID)
         .Parameters.Append .CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID)
         .Parameters.Append .CreateParameter("@OutstandingOnlyYN", adVarChar, adParamInput, 1, IIf(blnOutstandingOnlyYN, "Y", "N"))
         .Parameters.Append .CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0)
     End With
    
    'Execute the stored procedure
     rs.CursorLocation = adUseClient
     rs.Open cmd, , adOpenForwardOnly, adLockReadOnly
    
     Set FetchByPatientProvider = rs
    
    'Disconnect the recordset
     Set cmd.ActiveConnection = Nothing
     Set cmd = Nothing
     Set rs.ActiveConnection = Nothing
     Set cnn = Nothing
     
    'Signal successful completion
     GetObjectContext.SetComplete
    
     Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
     Call RaiseError(Err, cmd.Parameters("@ErrorNum").Value, CLASS_NAME)
     Set cmd = Nothing
     Set rs = Nothing
     Set cnn = Nothing
     GetObjectContext.SetAbort
 End Function
Public Function FetchByID(ByVal lngBRID As Long) As ADODB.Recordset
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
     
     On Error GoTo ErrTrap:
     
     Set rs = New ADODB.Recordset
     Set cmd = New ADODB.Command
     Set cnn = New ADODB.Connection
     cnn.Open (CONST_PSYQUEL_CNN)
     Set cmd.ActiveConnection = cnn
     
    'Create the parameter objects
     With cmd
         .CommandText = "uspSelBRByID"
         .CommandType = adCmdStoredProc
         .Parameters.Append .CreateParameter("@BRID", adInteger, adParamInput, , lngBRID)
         .Parameters.Append .CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0)
     End With
    
    'Execute the stored procedure
     rs.CursorLocation = adUseClient
     rs.Open cmd, , adOpenForwardOnly, adLockReadOnly
    
     Set FetchByID = rs
    
    'Disconnect the recordset
     Set cmd.ActiveConnection = Nothing
     Set cmd = Nothing
     Set rs.ActiveConnection = Nothing
     Set cnn = Nothing
     
    'Signal successful completion
     GetObjectContext.SetComplete
    
     Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
     Call RaiseError(Err, cmd.Parameters("@ErrorNum").Value, CLASS_NAME)
     Set cmd = Nothing
     Set rs = Nothing
     Set cnn = Nothing
     GetObjectContext.SetAbort
 End Function
Public Function isAnEAP(ByVal lngBRIDToTest1 As Long, ByVal lngBRIDToTest2 As Long) As Boolean
'--------------------------------------------------------------------
'Date: 03/15/2001
'Author: Eric Pena
'Description:  Checks to see if an EAP is linked to either BR record specified
'Parameters: lngBRIDToTest1/lngBRIDToTest2 -  key of the BR record to test for EAP linkage
'Returns: True if either BRID links to an EAP, False otherwise
'--------------------------------------------------------------------
    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command
    Dim rst As ADODB.Recordset

    On Error GoTo ErrTrap:
    
    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    Set rst = New ADODB.Recordset
    
    With cmdSQL
        .CommandText = "uspSelEAPByBR"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@BRID", adInteger, adParamInput, , lngBRIDToTest1)
        .Parameters.Append .CreateParameter("@BR2ID", adInteger, adParamInput, , lngBRIDToTest2)
    End With
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open(CONST_PSYQUEL_CNN)
     Set cmdSQL.ActiveConnection = cnnSQL
    
    'Execute the stored procedure
     rst.CursorLocation = adUseClient
     rst.Open cmdSQL, , adOpenForwardOnly, adLockReadOnly
    
    If rst.EOF And rst.BOF Then
        isAnEAP = False
    Else
        isAnEAP = True
    End If

    'Close the connection and free all resources
    cnnSQL.Close
    
    Set rst = Nothing
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set rst = Nothing
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    Call RaiseError(Err, Err.Number, CLASS_NAME, Err.Description)
End Function
Public Sub UpdateCert(ByVal lngELID As Long, ByVal lngRPID As Long, ByVal lngPlanID As Long, ByVal strCertNum As String)
'--------------------------------------------------------------------
'Date: 08/02/2002
'Author: Eric Pena
'Description:  Updates a cert in tblBillingResponsibility utilizing a stored procedure.
'Parameters:
'Returns: Null
'--------------------------------------------------------------------
    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command

    On Error GoTo ErrTrap:
    
    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    
    With cmdSQL
        .CommandText = "uspUpdBRCert"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@ELID", adInteger, adParamInput, , lngELID)
        .Parameters.Append .CreateParameter("@RPID", adInteger, adParamInput, , lngRPID)
        .Parameters.Append .CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID)
        .Parameters.Append .CreateParameter("@CertNum", adVarChar, adParamInput, 30, strCertNum)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open(CONST_PSYQUEL_CNN)
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute , , adExecuteNoRecords
    
    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If

    'Close the connection and free all resources
    cnnSQL.Close
    
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    
    Call RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, cmdSQL.Parameters("@ErrorMsg").Value)
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    
End Sub




