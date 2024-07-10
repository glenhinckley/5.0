VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CRemitDB"
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

Private Const CLASS_NAME As String = "RemitDB"
Private Const TABLE_NAME As String = "tblRemittance"

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
Public Function Insert(ByVal strPayorID As String, ByVal strHealthPlanID As String, ByVal strHealthPlanName As String, _
                       ByVal dteCreationDate As Date, ByVal strBatchNumber As String, ByVal strProvider As String, ByVal strProviderNbr As String, _
                       ByVal strCheckNumber As String, ByVal dteCheckDate As Date, ByVal dblCheckAmount As Double, ByVal dblAmountPaid As Double, _
                       ByVal dblInterestAmount As Double, ByVal dteRemittanceDate As Date, ByVal dteProcessDate As Date, ByVal strPatientNumber As String, _
                       ByVal strInsuredGroupNbr As String, ByVal strInsuredPolicyID As String, ByVal strInsuredLastName As String, ByVal strInsuredFirstName As String, _
                       ByVal strInsuredMiddleInitial As String, ByVal strPatientGroupNbr As String, ByVal strPatientPolicyID As String, ByVal strPatientLastName As String, _
                       ByVal strPatientFirstName As String, ByVal strPatientMiddleInitial As String, ByVal strPatientSex As String, _
                       ByVal dtePatientDateOfBirth As Date, ByVal blnCompInsuranceFlag As Boolean, ByVal strMedicalRecordNumber As String, _
                       ByVal strMessageCode1 As String, ByVal strMessageCode2 As String, ByVal strMessageCode3 As String, ByVal strMessageCode4 As String, ByVal strMessageCode5 As String, _
                       ByVal strSupplementalInsurerName1 As String, ByVal strSupplementalInsurerID1 As String, ByVal strSupplementalInsurerName2 As String, ByVal strSupplementalInsurerID2 As String, _
                       ByVal lngInsuranceID As Long, ByVal lngProviderID As Long, ByVal lngPatientID As Long, ByVal lngEncounterLogID As Long, ByVal lngCheckID As Long, _
                       ByVal strErnFileName As String, ByVal blnPosted As Boolean, ByVal dtePostedDate As Date, ByVal strErrorMessage As String, _
                       ByVal strAddedBy As String) As Long
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
        .CommandText = "uspInsRemittance"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@RemittanceRecordID", adInteger, adParamReturnValue)
        .Parameters.Append .CreateParameter("@PayorID", adVarChar, adParamInput, 10, strPayorID)
        .Parameters.Append .CreateParameter("@HealthPlanID", adVarChar, adParamInput, 15, strHealthPlanID)
        .Parameters.Append .CreateParameter("@HealthPlanName", adVarChar, adParamInput, 50, strHealthPlanName)
        .Parameters.Append .CreateParameter("@CreationDate", adDBTimeStamp, adParamInput, , IIf(CLng(dteCreationDate) = 0, Null, dteCreationDate))
        .Parameters.Append .CreateParameter("@BatchNumber", adVarChar, adParamInput, 50, strBatchNumber)
        .Parameters.Append .CreateParameter("@Provider", adVarChar, adParamInput, 75, strProvider)
        .Parameters.Append .CreateParameter("@ProviderNbr", adVarChar, adParamInput, 15, strProviderNbr)
        .Parameters.Append .CreateParameter("@CheckNumber", adVarChar, adParamInput, 30, strCheckNumber)
        .Parameters.Append .CreateParameter("@CheckDate", adDBTimeStamp, adParamInput, , IIf(CLng(dteCheckDate) = 0, Null, dteCheckDate))
        .Parameters.Append .CreateParameter("@CheckAmount", adCurrency, adParamInput, , dblCheckAmount)
        .Parameters.Append .CreateParameter("@AmountPaid", adCurrency, adParamInput, , dblAmountPaid)
        .Parameters.Append .CreateParameter("@InterestAmount", adCurrency, adParamInput, , dblInterestAmount)
        .Parameters.Append .CreateParameter("@RemittanceDate", adDBTimeStamp, adParamInput, , IIf(CLng(dteRemittanceDate) = 0, Null, dteRemittanceDate))
        .Parameters.Append .CreateParameter("@ProcessDate", adDBTimeStamp, adParamInput, , IIf(CLng(dteProcessDate) = 0, Null, dteProcessDate))
        .Parameters.Append .CreateParameter("@PatientNumber", adVarChar, adParamInput, 20, strPatientNumber)
        .Parameters.Append .CreateParameter("@InsuredGroupNbr", adVarChar, adParamInput, 20, strInsuredGroupNbr)
        .Parameters.Append .CreateParameter("@InsuredPolicyID", adVarChar, adParamInput, 25, strInsuredPolicyID)
        .Parameters.Append .CreateParameter("@InsuredLastName", adVarChar, adParamInput, 20, strInsuredLastName)
        .Parameters.Append .CreateParameter("@InsuredFirstName", adVarChar, adParamInput, 20, strInsuredFirstName)
        .Parameters.Append .CreateParameter("@InsuredMiddleInitial", adVarChar, adParamInput, 1, strInsuredMiddleInitial)
        .Parameters.Append .CreateParameter("@PatientGroupNbr", adVarChar, adParamInput, 20, strPatientGroupNbr)
        .Parameters.Append .CreateParameter("@PatientPolicyID", adVarChar, adParamInput, 25, strPatientPolicyID)
        .Parameters.Append .CreateParameter("@PatientLastName", adVarChar, adParamInput, 20, strPatientLastName)
        .Parameters.Append .CreateParameter("@PatientFirstName", adVarChar, adParamInput, 20, strPatientFirstName)
        .Parameters.Append .CreateParameter("@PatientMiddleInitial", adVarChar, adParamInput, 1, strPatientMiddleInitial)
        .Parameters.Append .CreateParameter("@PatientSex", adVarChar, adParamInput, 1, strPatientSex)
        .Parameters.Append .CreateParameter("@PatientDateOfBirth", adDBTimeStamp, adParamInput, , IIf(CLng(dtePatientDateOfBirth) = 0, Null, dtePatientDateOfBirth))
        .Parameters.Append .CreateParameter("@CompInsuranceFlag", adVarChar, adParamInput, 1, IIf(blnCompInsuranceFlag = True, "Y", "N"))
        .Parameters.Append .CreateParameter("@MedicalRecordNumber", adVarChar, adParamInput, 50, strMedicalRecordNumber)
        .Parameters.Append .CreateParameter("@MessageCode1", adVarChar, adParamInput, 5, strMessageCode1)
        .Parameters.Append .CreateParameter("@MessageCode2", adVarChar, adParamInput, 5, strMessageCode2)
        .Parameters.Append .CreateParameter("@MessageCode3", adVarChar, adParamInput, 5, strMessageCode3)
        .Parameters.Append .CreateParameter("@MessageCode4", adVarChar, adParamInput, 5, strMessageCode4)
        .Parameters.Append .CreateParameter("@MessageCode5", adVarChar, adParamInput, 5, strMessageCode5)
        .Parameters.Append .CreateParameter("@SupplementalInsurerName1", adVarChar, adParamInput, 50, strSupplementalInsurerName1)
        .Parameters.Append .CreateParameter("@SupplementalInsurerID1", adVarChar, adParamInput, 15, strSupplementalInsurerID1)
        .Parameters.Append .CreateParameter("@SupplementalInsurerName2", adVarChar, adParamInput, 50, strSupplementalInsurerName2)
        .Parameters.Append .CreateParameter("@SupplementalInsurerID2", adVarChar, adParamInput, 15, strSupplementalInsurerID2)
        .Parameters.Append .CreateParameter("@InsuranceID", adInteger, adParamInput, , lngInsuranceID)
        .Parameters.Append .CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID)
        .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID)
        .Parameters.Append .CreateParameter("@EncounterLogID", adInteger, adParamInput, , lngEncounterLogID)
        .Parameters.Append .CreateParameter("@CheckID", adInteger, adParamInput, , lngCheckID)
        .Parameters.Append .CreateParameter("@ErnFileName", adVarChar, adParamInput, 50, strErnFileName)
        .Parameters.Append .CreateParameter("@Posted", adVarChar, adParamInput, 1, IIf(blnPosted = True, "Y", "N"))
        .Parameters.Append .CreateParameter("@PostedDate", adDBTimeStamp, adParamInput, , IIf(CLng(dtePostedDate) = 0, Null, dtePostedDate))
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

    Insert = cmdSQL.Parameters("@RemittanceRecordID").Value
    
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

Public Sub Update(ByVal lngID As Long, ByVal strPayorID As String, ByVal strHealthPlanID As String, ByVal strHealthPlanName As String, _
                  ByVal dteCreationDate As Date, ByVal strBatchNumber As String, ByVal strProvider As String, ByVal strProviderNbr As String, _
                  ByVal strCheckNumber As String, ByVal detCheckDate As Date, ByVal dblCheckAmount As Double, ByVal dblAmountPaid As Double, _
                  ByVal dblInterestAmount As Double, ByVal dteRemittanceDate As Date, ByVal dteProcessDate As Date, ByVal strPatientNumber As String, _
                  ByVal strInsuredGroupNbr As String, ByVal strInsuredPolicyID As String, ByVal strInsuredLastName As String, ByVal strInsuredFirstName As String, _
                  ByVal strInsuredMiddleInitial As String, ByVal strPatientGroupNbr As String, ByVal strPatientPolicyID As String, ByVal strPatientLastName As String, _
                  ByVal strPatientFirstName As String, ByVal strPatientMiddleInitial As String, ByVal strPatientSex As String, _
                  ByVal dtePatientDateOfBirth As Date, ByVal blnCompInsuranceFlag As Boolean, ByVal strMedicalRecordNumber As String, _
                  ByVal strMessageCode1 As String, ByVal strMessageCode2 As String, ByVal strMessageCode3 As String, ByVal strMessageCode4 As String, ByVal strMessageCode5 As String, _
                  ByVal strSupplementalInsurerName1 As String, ByVal strSupplementalInsurerID1 As String, ByVal strSupplementalInsurerName2 As String, ByVal strSupplementalInsurerID2 As String, _
                  ByVal lngInsuranceID As Long, ByVal lngProviderID As Long, ByVal lngPatientID As Long, ByVal lngEncounterLogID As Long, ByVal lngCheckID As Long, _
                  ByVal strErnFileName As String, ByVal blnPosted As Boolean, ByVal dtePostedDate As Date, ByVal strErrorMessage As String, _
                  ByVal strAddedBy As String)
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
        .CommandText = "uspUpdRemittance"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@RemittanceRecordID", adInteger, adParamInput, , lngID)
        .Parameters.Append .CreateParameter("@PayorID", adVarChar, adParamInput, 10, strPayorID)
        .Parameters.Append .CreateParameter("@HealthPlanID", adVarChar, adParamInput, 15, strHealthPlanID)
        .Parameters.Append .CreateParameter("@HealthPlanName", adVarChar, adParamInput, 50, strHealthPlanName)
        .Parameters.Append .CreateParameter("@CreationDate", adDBTimeStamp, adParamInput, , IIf(CLng(dteCreationDate) = 0, Null, dteCreationDate))
        .Parameters.Append .CreateParameter("@BatchNumber", adVarChar, adParamInput, 50, strBatchNumber)
        .Parameters.Append .CreateParameter("@Provider", adVarChar, adParamInput, 75, strProvider)
        .Parameters.Append .CreateParameter("@ProviderNbr", adVarChar, adParamInput, 15, strProviderNbr)
        .Parameters.Append .CreateParameter("@CheckNumber", adVarChar, adParamInput, 30, strCheckNumber)
        .Parameters.Append .CreateParameter("@CheckDate", adDBTimeStamp, adParamInput, , detCheckDate)
        .Parameters.Append .CreateParameter("@CheckAmount", adCurrency, adParamInput, , dblCheckAmount)
        .Parameters.Append .CreateParameter("@AmountPaid", adCurrency, adParamInput, , dblAmountPaid)
        .Parameters.Append .CreateParameter("@InterestAmount", adCurrency, adParamInput, , dblInterestAmount)
        .Parameters.Append .CreateParameter("@RemittanceDate", adDBTimeStamp, adParamInput, , IIf(CLng(dteRemittanceDate) = 0, Null, dteRemittanceDate))
        .Parameters.Append .CreateParameter("@ProcessDate", adDBTimeStamp, adParamInput, , IIf(CLng(dteProcessDate) = 0, Null, dteProcessDate))
        .Parameters.Append .CreateParameter("@PatientNumber", adVarChar, adParamInput, 20, strPatientNumber)
        .Parameters.Append .CreateParameter("@InsuredGroupNbr", adVarChar, adParamInput, 20, strInsuredGroupNbr)
        .Parameters.Append .CreateParameter("@InsuredPolicyID", adVarChar, adParamInput, 25, strInsuredPolicyID)
        .Parameters.Append .CreateParameter("@InsuredLastName", adVarChar, adParamInput, 20, strInsuredLastName)
        .Parameters.Append .CreateParameter("@InsuredFirstName", adVarChar, adParamInput, 20, strInsuredFirstName)
        .Parameters.Append .CreateParameter("@InsuredMiddleInitial", adVarChar, adParamInput, 1, strInsuredMiddleInitial)
        .Parameters.Append .CreateParameter("@PatientGroupNbr", adVarChar, adParamInput, 20, strPatientGroupNbr)
        .Parameters.Append .CreateParameter("@PatientPolicyID", adVarChar, adParamInput, 25, strPatientPolicyID)
        .Parameters.Append .CreateParameter("@PatientLastName", adVarChar, adParamInput, 20, strPatientLastName)
        .Parameters.Append .CreateParameter("@PatientFirstName", adVarChar, adParamInput, 20, strPatientFirstName)
        .Parameters.Append .CreateParameter("@PatientMiddleInitial", adVarChar, adParamInput, 1, strPatientMiddleInitial)
        .Parameters.Append .CreateParameter("@PatientSex", adVarChar, adParamInput, 1, strPatientSex)
        .Parameters.Append .CreateParameter("@PatientDateOfBirth", adDBTimeStamp, adParamInput, , IIf(CLng(dtePatientDateOfBirth) = 0, Null, dtePatientDateOfBirth))
        .Parameters.Append .CreateParameter("@CompInsuranceFlag", adVarChar, adParamInput, 1, IIf(blnCompInsuranceFlag = True, "Y", "N"))
        .Parameters.Append .CreateParameter("@MedicalRecordNumber", adVarChar, adParamInput, 50, strMedicalRecordNumber)
        .Parameters.Append .CreateParameter("@MessageCode1", adVarChar, adParamInput, 5, strMessageCode1)
        .Parameters.Append .CreateParameter("@MessageCode2", adVarChar, adParamInput, 5, strMessageCode2)
        .Parameters.Append .CreateParameter("@MessageCode3", adVarChar, adParamInput, 5, strMessageCode3)
        .Parameters.Append .CreateParameter("@MessageCode4", adVarChar, adParamInput, 5, strMessageCode4)
        .Parameters.Append .CreateParameter("@MessageCode5", adVarChar, adParamInput, 5, strMessageCode5)
        .Parameters.Append .CreateParameter("@SupplementalInsurerName1", adVarChar, adParamInput, 50, strSupplementalInsurerName1)
        .Parameters.Append .CreateParameter("@SupplementalInsurerID1", adVarChar, adParamInput, 15, strSupplementalInsurerID1)
        .Parameters.Append .CreateParameter("@SupplementalInsurerName2", adVarChar, adParamInput, 50, strSupplementalInsurerName2)
        .Parameters.Append .CreateParameter("@SupplementalInsurerID2", adVarChar, adParamInput, 15, strSupplementalInsurerID2)
        .Parameters.Append .CreateParameter("@InsuranceID", adInteger, adParamInput, , lngInsuranceID)
        .Parameters.Append .CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID)
        .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID)
        .Parameters.Append .CreateParameter("@EncounterLogID", adInteger, adParamInput, , lngEncounterLogID)
        .Parameters.Append .CreateParameter("@CheckID", adInteger, adParamInput, , lngCheckID)
        .Parameters.Append .CreateParameter("@ErnFileName", adVarChar, adParamInput, 50, strErnFileName)
        .Parameters.Append .CreateParameter("@Posted", adVarChar, adParamInput, 1, IIf(blnPosted = True, "Y", "N"))
        .Parameters.Append .CreateParameter("@PostedDate", adDBTimeStamp, adParamInput, , IIf(CLng(dtePostedDate) = 0, Null, dtePostedDate))
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
