VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CRemitDetailDB"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'-------------------------------------------------------------------------------------
'Date: 04/20/2009
'Class Name: RemitDB
'Author: Duane C Orth
'Description:   COM object designed to host methods associated with electronic
'               checks.
'--------------------------------------------------------------------------------------
Option Explicit

Private Const CLASS_NAME As String = "RemitDetailDB"
Private Const TABLE_NAME As String = "tblRemittanceDetail"

Public Function Fetch(Optional blnIncludeDisabled As Boolean = False, _
                      Optional ByVal strWhere As String, _
                      Optional ByVal strOrderBy As String) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 04/20/2009
'Author: Duane C Orth
'Description:  Retrieves records from the tblRemittance table.
'Parameters: blnIncludeDisabled - Optional parameter that identifies if
'               records flagged as 'Disabled' or 'De-activated' are to be
'               included in the record set. The default value is False.
'            strWhere - Optional 'Where' clause of the SQL statement by
'               which records will be filtered.  If the parameter is not
'               supplied, all records will be retrieved.
'             strOrderBy - Optional 'Order By' clause in which retrieved
'               records will be sorted.  The default is set to the
'               'fldName' column
'Returns: ADODB.Recordset
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------
    
    Dim cnnSQL As ADODB.Connection
    Dim rstSQL As ADODB.Recordset
    Dim strDfltWhere As String
    Dim strSQL As String

    On Error GoTo ErrTrap:

    'Prepare the SQL statement.
    strSQL = "SELECT "
    strSQL = strSQL & TABLE_NAME & ".* "
    strSQL = strSQL & "FROM "
    strSQL = strSQL & TABLE_NAME & " "
    
    'Determine whether to include 'disabled' records'
    If blnIncludeDisabled = False Then
        strDfltWhere = " WHERE fldDisabledYN = 'N' "
    End If
    
    'Attach user's where clause if applicable
    strSQL = strSQL & strDfltWhere
    
    If Trim(strWhere > "") Then If blnIncludeDisabled Then strSQL = strSQL & " WHERE " & strWhere & " " Else strSQL = strSQL & " AND " & strWhere & " "
    
    If Trim(strOrderBy) > "" Then
        strSQL = strSQL & " ORDER BY "
        strSQL = strSQL & strOrderBy
    Else
        strSQL = strSQL & " ORDER BY "
        strSQL = strSQL & "fldInsName "
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
    
    cnnSQL.Close
    Set cnnSQL = Nothing
    Set Fetch = rstSQL
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set rstSQL.ActiveConnection = Nothing
    Set rstSQL = Nothing
    Set cnnSQL = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Function
Public Function Insert(ByVal lngRemittanceRecordID As Long, ByVal strPayorID As String, ByVal strPatientNumber As String, _
                       ByVal strLineNumber As String, ByVal strServiceLineNumber As String, ByVal dteFirstDateOfService As Date, _
                       ByVal dteLastDateOfService As Date, ByVal dteClaimReceiveDate As Date, _
                       ByVal strPlaceOfService As String, ByVal strTypeOfService As String, ByVal strProcedureCode As String, _
                       ByVal strMod1 As String, ByVal strMod2 As String, ByVal strMod3 As String, _
                       ByVal strUnits As String, ByVal dblCharge As Double, ByVal dblDisallowed As Double, ByVal dblAllowed As Double, _
                       ByVal dblDeductible As Double, ByVal dblCoinsurance As Double, ByVal dblPatientResp As Double, _
                       ByVal dblInterest As Double, ByVal dblPayment As Double, _
                       ByVal strProvider As String, ByVal strReason1 As String, ByVal strReason2 As String, ByVal strReason3 As String, _
                       ByVal strReason4 As String, ByVal strReason5 As String, ByVal strReason6 As String, ByVal strReason7 As String, _
                       ByVal blnPosted As Boolean, ByVal dtePostedDate As Date, _
                       ByVal lngEncounterLogID As Long, ByVal lngPatientID As Long, ByVal lngInsuranceID As Long, ByVal lngProviderID As Long, _
                       ByVal lngCheckID As Long, ByVal lngCheckLogID As Long, ByVal strErnFileName, ByVal strErrorMessage, ByVal strAddedBy) As Long
                       
                       
'--------------------------------------------------------------------
'Date: 04/20/2009
'Author: Duane C Orth
'Description:  Inserts a row into the tblRemittance table utilizing
'              a stored procedure.
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'Returns: ID (Primary Key) of the row inserted.
'--------------------------------------------------------------------
    
    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command
    Dim intCount As Integer
    
    On Error GoTo ErrTrap:
    
    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    'Create the parameter objects
    With cmdSQL
        .CommandText = "uspInsRemittanceDetail"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@RemittanceDetailRecordID", adInteger, adParamReturnValue)
        .Parameters.Append .CreateParameter("@RemittanceRecordID", adInteger, adParamInput, , lngRemittanceRecordID)
        .Parameters.Append .CreateParameter("@PayorID", adVarChar, adParamInput, 10, strPayorID)
        .Parameters.Append .CreateParameter("@PatientNumber", adVarChar, adParamInput, 17, strPatientNumber)
        .Parameters.Append .CreateParameter("@LineNumber", adVarChar, adParamInput, 17, strLineNumber)
        .Parameters.Append .CreateParameter("@ServiceLineNumber", adVarChar, adParamInput, 3, strServiceLineNumber)
        .Parameters.Append .CreateParameter("@FirstDateOfService", adDBTimeStamp, adParamInput, , IIf(CLng(dteFirstDateOfService) = 0, Null, dteFirstDateOfService))
        .Parameters.Append .CreateParameter("@LastDateOfService", adDBTimeStamp, adParamInput, , IIf(CLng(dteLastDateOfService) = 0, Null, dteLastDateOfService))
        .Parameters.Append .CreateParameter("@ClaimReceiveDate", adDBTimeStamp, adParamInput, , IIf(CLng(dteClaimReceiveDate) = 0, Null, dteClaimReceiveDate))
        .Parameters.Append .CreateParameter("@PlaceOfService", adVarChar, adParamInput, 2, strPlaceOfService)
        .Parameters.Append .CreateParameter("@TypeOfService", adVarChar, adParamInput, 2, strTypeOfService)
        .Parameters.Append .CreateParameter("@ProcedureCode", adVarChar, adParamInput, 5, strProcedureCode)
        .Parameters.Append .CreateParameter("@Mod1", adVarChar, adParamInput, 2, strMod1)
        .Parameters.Append .CreateParameter("@Mod2", adVarChar, adParamInput, 2, strMod2)
        .Parameters.Append .CreateParameter("@Mod3", adVarChar, adParamInput, 5, strMod3)
        .Parameters.Append .CreateParameter("@Units", adVarChar, adParamInput, 4, strUnits)
        .Parameters.Append .CreateParameter("@Charge", adCurrency, adParamInput, , dblCharge)
        .Parameters.Append .CreateParameter("@Disallowed", adCurrency, adParamInput, , dblDisallowed)
        .Parameters.Append .CreateParameter("@Allowed", adCurrency, adParamInput, , dblAllowed)
        .Parameters.Append .CreateParameter("@Deductible", adCurrency, adParamInput, , dblDeductible)
        .Parameters.Append .CreateParameter("@Coinsurance", adCurrency, adParamInput, , dblCoinsurance)
        .Parameters.Append .CreateParameter("@PatientResp", adCurrency, adParamInput, , dblPatientResp)
        .Parameters.Append .CreateParameter("@Interest", adCurrency, adParamInput, , dblInterest)
        .Parameters.Append .CreateParameter("@Payment", adCurrency, adParamInput, , dblPayment)
        .Parameters.Append .CreateParameter("@Provider", adVarChar, adParamInput, 15, strProvider)
        .Parameters.Append .CreateParameter("@Reason1", adVarChar, adParamInput, 6, strReason1)
        .Parameters.Append .CreateParameter("@Reason2", adVarChar, adParamInput, 6, strReason2)
        .Parameters.Append .CreateParameter("@Reason3", adVarChar, adParamInput, 6, strReason3)
        .Parameters.Append .CreateParameter("@Reason4", adVarChar, adParamInput, 6, strReason4)
        .Parameters.Append .CreateParameter("@Reason5", adVarChar, adParamInput, 6, strReason5)
        .Parameters.Append .CreateParameter("@Reason6", adVarChar, adParamInput, 6, strReason6)
        .Parameters.Append .CreateParameter("@Reason7", adVarChar, adParamInput, 6, strReason7)
        .Parameters.Append .CreateParameter("@Posted", adVarChar, adParamInput, 1, IIf(blnPosted = True, "Y", "N"))
        .Parameters.Append .CreateParameter("@PostedDate", adDBTimeStamp, adParamInput, , IIf(CLng(dtePostedDate) = 0, Null, dtePostedDate))
        .Parameters.Append .CreateParameter("@EncounterLogID", adInteger, adParamInput, , lngEncounterLogID)
        .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID)
        .Parameters.Append .CreateParameter("@InsuranceID", adInteger, adParamInput, , lngInsuranceID)
        .Parameters.Append .CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID)
        .Parameters.Append .CreateParameter("@CheckID", adInteger, adParamInput, , lngCheckID)
        .Parameters.Append .CreateParameter("@CheckLogID", adInteger, adParamInput, , lngCheckLogID)
        .Parameters.Append .CreateParameter("@ErnFileName", adVarChar, adParamInput, 50, strErnFileName)
        .Parameters.Append .CreateParameter("@ErrorMessage", adVarChar, adParamInput, 255, strErrorMessage)
        .Parameters.Append .CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strAddedBy)
        .Parameters.Append .CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open(CONST_PSYQUEL_CNN)
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute , , adExecuteNoRecords

    Insert = cmdSQL.Parameters("@RemittanceDetailRecordID").Value
    
    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If
        
    'Close the connection
    cnnSQL.Close
    
    'free all resources
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing

    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    GetObjectContext.SetAbort
    Call RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME)
    
End Function

Public Sub Update(ByVal lngID As Long, ByVal lngRemittanceRecordID As Long, ByVal strPayorID As String, ByVal strPatientNumber As String, _
                       ByVal strLineNumber As String, ByVal strServiceLineNumber As String, ByVal dteFirstDateOfService As Date, _
                       ByVal dteLastDateOfService As Date, ByVal dteClaimReceiveDate As Date, _
                       ByVal strPlaceOfService As String, ByVal strTypeOfService As String, ByVal strProcedureCode As String, _
                       ByVal strMod1 As String, ByVal strMod2 As String, ByVal strMod3 As String, _
                       ByVal strUnits As String, ByVal dblCharge As Double, ByVal dblDisallowed As Double, ByVal dblAllowed As Double, _
                       ByVal dblDeductible As Double, ByVal dblCoinsurance As Double, ByVal dblPatientResp As Double, _
                       ByVal dblInterest As Double, ByVal dblPayment As Double, _
                       ByVal strProvider As String, ByVal strReason1 As String, ByVal strReason2 As String, ByVal strReason3 As String, _
                       ByVal strReason4 As String, ByVal strReason5 As String, ByVal strReason6 As String, ByVal strReason7 As String, _
                       ByVal blnPosted As Boolean, ByVal dtePostedDate As Date, _
                       ByVal lngEncounterLogID As Long, ByVal lngPatientID As Long, ByVal lngInsuranceID As Long, ByVal lngProviderID As Long, _
                       ByVal lngCheckID As Long, ByVal lngCheckLogID As Long, ByVal strErnFileName, ByVal strErrorMessage, ByVal strAddedBy)
'--------------------------------------------------------------------
'Date: 04/20/2009
'Author: Duane C Orth
'Description:  Updates a row in the tblRemittance table utilizing
'              a stored procedure.
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'Returns: Null
'--------------------------------------------------------------------
    
    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command
    
    On Error GoTo ErrTrap:
    
    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    
    With cmdSQL
        .CommandText = "uspUpdRemittanceDetail"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@RemittanceDetailRecordID", adInteger, adParamInput, , lngID)
        .Parameters.Append .CreateParameter("@RemittanceRecordID", adInteger, adParamInput, , lngRemittanceRecordID)
        .Parameters.Append .CreateParameter("@PayorID", adVarChar, adParamInput, 10, strPayorID)
        .Parameters.Append .CreateParameter("@PatientNumber", adVarChar, adParamInput, 17, strPatientNumber)
        .Parameters.Append .CreateParameter("@LineNumber", adVarChar, adParamInput, 17, strLineNumber)
        .Parameters.Append .CreateParameter("@ServiceLineNumber", adVarChar, adParamInput, 3, strServiceLineNumber)
        .Parameters.Append .CreateParameter("@FirstDateOfService", adDBTimeStamp, adParamInput, , IIf(CLng(dteFirstDateOfService) = 0, Null, dteFirstDateOfService))
        .Parameters.Append .CreateParameter("@LastDateOfService", adDBTimeStamp, adParamInput, , IIf(CLng(dteLastDateOfService) = 0, Null, dteLastDateOfService))
        .Parameters.Append .CreateParameter("@ClaimReceiveDate", adDBTimeStamp, adParamInput, , IIf(CLng(dteClaimReceiveDate) = 0, Null, dteClaimReceiveDate))
        .Parameters.Append .CreateParameter("@PlaceOfService", adVarChar, adParamInput, 2, strPlaceOfService)
        .Parameters.Append .CreateParameter("@TypeOfService", adVarChar, adParamInput, 2, strTypeOfService)
        .Parameters.Append .CreateParameter("@ProcedureCode", adVarChar, adParamInput, 5, strProcedureCode)
        .Parameters.Append .CreateParameter("@Mod1", adVarChar, adParamInput, 2, strMod1)
        .Parameters.Append .CreateParameter("@Mod2", adVarChar, adParamInput, 2, strMod2)
        .Parameters.Append .CreateParameter("@Mod3", adVarChar, adParamInput, 5, strMod3)
        .Parameters.Append .CreateParameter("@Units", adVarChar, adParamInput, 4, strUnits)
        .Parameters.Append .CreateParameter("@Charge", adCurrency, adParamInput, , dblCharge)
        .Parameters.Append .CreateParameter("@Disallowed", adCurrency, adParamInput, , dblDisallowed)
        .Parameters.Append .CreateParameter("@Allowed", adCurrency, adParamInput, , dblAllowed)
        .Parameters.Append .CreateParameter("@Deductible", adCurrency, adParamInput, , dblDeductible)
        .Parameters.Append .CreateParameter("@Coinsurance", adCurrency, adParamInput, , dblCoinsurance)
        .Parameters.Append .CreateParameter("@PatientResp", adCurrency, adParamInput, , dblPatientResp)
        .Parameters.Append .CreateParameter("@Interest", adCurrency, adParamInput, , dblInterest)
        .Parameters.Append .CreateParameter("@Payment", adCurrency, adParamInput, , dblPayment)
        .Parameters.Append .CreateParameter("@Provider", adVarChar, adParamInput, 15, strProvider)
        .Parameters.Append .CreateParameter("@Reason1", adVarChar, adParamInput, 6, strReason1)
        .Parameters.Append .CreateParameter("@Reason2", adVarChar, adParamInput, 6, strReason2)
        .Parameters.Append .CreateParameter("@Reason3", adVarChar, adParamInput, 6, strReason3)
        .Parameters.Append .CreateParameter("@Reason4", adVarChar, adParamInput, 6, strReason4)
        .Parameters.Append .CreateParameter("@Reason5", adVarChar, adParamInput, 6, strReason5)
        .Parameters.Append .CreateParameter("@Reason6", adVarChar, adParamInput, 6, strReason6)
        .Parameters.Append .CreateParameter("@Reason7", adVarChar, adParamInput, 6, strReason7)
        .Parameters.Append .CreateParameter("@Posted", adVarChar, adParamInput, 1, IIf(blnPosted = True, "Y", "N"))
        .Parameters.Append .CreateParameter("@PostedDate", adDBTimeStamp, adParamInput, , IIf(CLng(dtePostedDate) = 0, Null, dtePostedDate))
        .Parameters.Append .CreateParameter("@EncounterLogID", adInteger, adParamInput, , lngEncounterLogID)
        .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID)
        .Parameters.Append .CreateParameter("@InsuranceID", adInteger, adParamInput, , lngInsuranceID)
        .Parameters.Append .CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID)
        .Parameters.Append .CreateParameter("@CheckID", adInteger, adParamInput, , lngCheckID)
        .Parameters.Append .CreateParameter("@CheckLogID", adInteger, adParamInput, , lngCheckLogID)
        .Parameters.Append .CreateParameter("@ErnFileName", adVarChar, adParamInput, 50, strErnFileName)
        .Parameters.Append .CreateParameter("@ErrorMessage", adVarChar, adParamInput, 255, strErrorMessage)
        .Parameters.Append .CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strAddedBy)
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
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    GetObjectContext.SetAbort
    Call RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, cmdSQL.Parameters("@ErrorMsg").Value)
    
End Sub
