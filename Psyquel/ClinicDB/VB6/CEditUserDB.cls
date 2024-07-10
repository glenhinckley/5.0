VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CEditUserDB"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CEditUserDB                                                '
'Date: 04/06/2000                                                   '
'Author: Dave Richkun                                               '
'Description:  MTS object designed to host methods associated with  '
'              adding and editing Users.
'--------------------------------------------------------------------
'Revision History:                                                  '
'--------------------------------------------------------------------

Option Explicit
Private Const CLASS_NAME As String = "CEditUserDB"

Public Function Insert(ByVal strUserName As String, ByVal strLastName As String, _
                  ByVal strFirstName As String, ByVal strMI As String, _
                  ByVal strSSN As String, ByVal strEmail As String, _
                  ByVal strOrigPassword As String, ByVal dtExpDate As Date, ByVal strAddedBy As String, Optional ByVal blnVBYN As Boolean = True) As Long
'--------------------------------------------------------------------
'Date: 01/03/2000                                                   '
'Author: Dave Richkun                                               '
'Description:  Inserts a row into the tblUsers table utilizing      '
'              a stored procedure.                                  '
'Parameters: Each parameter identifies the column value that will be'
'              inserted.                                            '
'Returns: ID (Primary Key) of the row inserted.                     '
'--------------------------------------------------------------------

    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command

    On Error GoTo ErrTrap:

    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    
    cmdSQL.CommandText = "uspInsUser"
    cmdSQL.CommandType = adCmdStoredProc
        
    'Create the parameter objects
    With cmdSQL
        .Parameters.Append .CreateParameter(, adInteger, adParamReturnValue)
        .Parameters.Append .CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName)
        .Parameters.Append .CreateParameter("@LastName", adVarChar, adParamInput, 20, strLastName)
        .Parameters.Append .CreateParameter("@FirstName", adVarChar, adParamInput, 20, strFirstName)
        .Parameters.Append .CreateParameter("@MI", adVarChar, adParamInput, 1, strMI)
        .Parameters.Append .CreateParameter("@SSN", adVarChar, adParamInput, 12, strSSN)
        .Parameters.Append .CreateParameter("@Email", adVarChar, adParamInput, 60, strEmail)
        .Parameters.Append .CreateParameter("@OrigPassword", adVarChar, adParamInput, 10, strOrigPassword)
        .Parameters.Append .CreateParameter("@PwdExpDate", adDBTimeStamp, adParamInput, , IIf(dtExpDate = 0, Null, dtExpDate))
        .Parameters.Append .CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strAddedBy)
        .Parameters.Append .CreateParameter("@VBYN", adVarChar, adParamInput, 1, IIf(blnVBYN, "Y", "N"))
        .Parameters.Append .CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open(CONST_PSYQUEL_CNN)
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute ', , adExecuteNoRecords

    Insert = cmdSQL.Parameters(0).Value
    
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
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Call RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME)
    
End Function


Public Sub Update(ByVal lngUserID As Long, ByVal strUserName As String, _
                  ByVal strLastName As String, ByVal strFirstName As String, _
                  ByVal strMI As String, ByVal strSSN As String, _
                  ByVal strEmail As String, ByVal strUpdatedBy As String, Optional ByVal dtExpDate As Date, Optional ByVal blnVBYN As Boolean = True)
'--------------------------------------------------------------------
'Date: 01/04/2000
'Author: Dave Richkun
'Description:  Updates a row in the tblUsers table utilizing
'              a stored procedure.
'Parameters: Each parameter identifies the column value that will be
'              Upderted.
'Returns: Null
'--------------------------------------------------------------------

    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command

    On Error GoTo ErrTrap:

    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
        
    'Create the parameter objects
    With cmdSQL
        .CommandText = "uspUpdUser"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@UserID", adInteger, adParamInput, , lngUserID)
        .Parameters.Append .CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName)
        .Parameters.Append .CreateParameter("@LastName", adVarChar, adParamInput, 20, strLastName)
        .Parameters.Append .CreateParameter("@FirstName", adVarChar, adParamInput, 20, strFirstName)
        .Parameters.Append .CreateParameter("@MI", adVarChar, adParamInput, 1, strMI)
        .Parameters.Append .CreateParameter("@SSN", adVarChar, adParamInput, 12, strSSN)
        .Parameters.Append .CreateParameter("@Email", adVarChar, adParamInput, 60, strEmail)
        '.Parameters.Append .CreateParameter("@DfltClinicID", adInteger, adParamInput, , lngDefaultClinicID)
        '.Parameters.Append .CreateParameter("@PrimaryRoleID", adInteger, adParamInput, , lngPrimaryRoleID)
        .Parameters.Append .CreateParameter("@UpdatedBy", adVarChar, adParamInput, 30, strUpdatedBy)
        .Parameters.Append .CreateParameter("@PwdExpDate", adDBTimeStamp, adParamInput, , IIf(dtExpDate = 0, Null, dtExpDate))
        .Parameters.Append .CreateParameter("@VBYN", adVarChar, adParamInput, 1, IIf(blnVBYN, "Y", "N"))
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
    GetObjectContext.SetAbort
    Call RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, cmdSQL.Parameters("@ErrorMsg").Value)
    
End Sub
Public Sub UpdateFlags(ByVal lngID As Long, ByVal blnAdministratorYN As Boolean, _
            ByVal blnBillingYN As Boolean, ByVal blnInsAdjustYN As Boolean, _
            ByVal blnPatientNotesYN As Boolean, ByVal blnGlanceAtDayYN As Boolean, ByVal blnProviderBalanceYN As Boolean, _
            ByVal blnStoreDocsYN As Boolean, ByVal blnPsyquelPayYN As Boolean, ByVal blnARPatientAgingDetailYN As Boolean, _
            ByVal blnOutInsSummaryYN As Boolean, ByVal blnPaidInsSummaryYN As Boolean, ByVal blnARAgingProvidersYN As Boolean, _
            ByVal blnBilledContactsYN As Boolean, ByVal blnBillingAccountDetailYN As Boolean, _
            ByVal blnClaimCountYN As Boolean, ByVal blnDailyAccountActivityYN As Boolean, _
            ByVal blnPatAcctSummaryYN As Boolean, ByVal blnPatientPaymentLogYN As Boolean, _
            ByVal blnPatientStmtYN As Boolean, ByVal blnPatientCertYN As Boolean, _
            ByVal blnPayerSummaryYN As Boolean, ByVal blnPostingHistoryYN As Boolean, _
            ByVal blnProviderDenialYN As Boolean, ByVal blnProviderIncomeYN As Boolean, _
            ByVal blnSuperbillYN As Boolean, ByVal blnOutstandingPatAcctYN As Boolean, _
            ByVal blnPayerSessionsYN As Boolean, ByVal blnCPTPayAnalysisYN As Boolean, _
            ByVal blnScheduleAnalysisYN As Boolean, ByVal blnWriteOffLogYN As Boolean, _
            ByVal blnApptDumpYN As Boolean, ByVal blnPatientListYN As Boolean, _
            ByVal blnPatBirthDateYN As Boolean, ByVal blnProgressNoteYN As Boolean, _
            ByVal blnHcfaYN As Boolean, ByVal blnUnReconChecksYN As Boolean, _
            ByVal blnTimelyFilingYN As Boolean, ByVal blnProvDirect As Boolean, _
            ByVal blnVBYN As Boolean, ByVal strUpdatedBy As String)
'--------------------------------------------------------------------
'Date: 12/04/2007                                                   '
'Author: Duane C Orth                                               '
'Description:  Updates a row into the tblProvider table utilizing   '
'              a stored procedure.                                  '
'Parameters: lngID - ID of the row in the table whose value will be '
'                    updated.                                       '
'Returns: Null                                                      '
'--------------------------------------------------------------------
'Revision History:                                                  '
'                                                                   '
'--------------------------------------------------------------------
    
    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command

    On Error GoTo ErrTrap:

    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    With cmdSQL
        .CommandText = "uspUpdUserFlags"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@ID", adInteger, adParamInput, , lngID)
        .Parameters.Append .CreateParameter("@AdministratorYN", adVarChar, adParamInput, 1, IIf(blnAdministratorYN, "Y", "N"))
        .Parameters.Append .CreateParameter("@BillingYN", adVarChar, adParamInput, 1, IIf(blnBillingYN, "Y", "N"))
        .Parameters.Append .CreateParameter("@InsAdjustYN", adVarChar, adParamInput, 1, IIf(blnInsAdjustYN, "Y", "N"))
        .Parameters.Append .CreateParameter("@PatientNotesYN", adVarChar, adParamInput, 1, IIf(blnPatientNotesYN, "Y", "N"))
        .Parameters.Append .CreateParameter("@GlanceAtDayYN", adVarChar, adParamInput, 1, IIf(blnGlanceAtDayYN, "Y", "N"))
        .Parameters.Append .CreateParameter("@ProviderBalanceYN", adVarChar, adParamInput, 1, IIf(blnProviderBalanceYN, "Y", "N"))
        .Parameters.Append .CreateParameter("@StoreDocsYN", adVarChar, adParamInput, 1, IIf(blnStoreDocsYN, "Y", "N"))  '<--
        .Parameters.Append .CreateParameter("@PsyquelPayYN", adVarChar, adParamInput, 1, IIf(blnPsyquelPayYN, "Y", "N"))  '<--
        
        .Parameters.Append .CreateParameter("@ARPatientAgingDetailYN", adVarChar, adParamInput, 1, IIf(blnARPatientAgingDetailYN, "Y", "N"))  '<--
        .Parameters.Append .CreateParameter("@OutInsSummaryYN", adVarChar, adParamInput, 1, IIf(blnOutInsSummaryYN, "Y", "N"))
        .Parameters.Append .CreateParameter("@PaidInsSummaryYN", adVarChar, adParamInput, 1, IIf(blnPaidInsSummaryYN, "Y", "N"))
        .Parameters.Append .CreateParameter("@ARAgingProvidersYN", adVarChar, adParamInput, 1, IIf(blnARAgingProvidersYN, "Y", "N"))  '<--
        .Parameters.Append .CreateParameter("@BilledContactsYN", adVarChar, adParamInput, 1, IIf(blnBilledContactsYN, "Y", "N"))
        .Parameters.Append .CreateParameter("@BillingAccountDetailYN", adVarChar, adParamInput, 1, IIf(blnBillingAccountDetailYN, "Y", "N"))
        .Parameters.Append .CreateParameter("@ClaimCountYN", adVarChar, adParamInput, 1, IIf(blnClaimCountYN, "Y", "N"))
        .Parameters.Append .CreateParameter("@DailyAccountActivityYN", adVarChar, adParamInput, 1, IIf(blnDailyAccountActivityYN, "Y", "N"))
        .Parameters.Append .CreateParameter("@PatAcctSummaryYN", adVarChar, adParamInput, 1, IIf(blnPatAcctSummaryYN, "Y", "N"))
        .Parameters.Append .CreateParameter("@PatientPaymentLogYN", adVarChar, adParamInput, 1, IIf(blnPatientPaymentLogYN, "Y", "N"))
        .Parameters.Append .CreateParameter("@PatientStmtYN", adVarChar, adParamInput, 1, IIf(blnPatientStmtYN, "Y", "N"))
        .Parameters.Append .CreateParameter("@PatientCertYN", adVarChar, adParamInput, 1, IIf(blnPatientCertYN, "Y", "N"))
        .Parameters.Append .CreateParameter("@PayerSummaryYN", adVarChar, adParamInput, 1, IIf(blnPayerSummaryYN, "Y", "N"))
        .Parameters.Append .CreateParameter("@PostingHistoryYN", adVarChar, adParamInput, 1, IIf(blnPostingHistoryYN, "Y", "N"))
        .Parameters.Append .CreateParameter("@ProviderDenialYN", adVarChar, adParamInput, 1, IIf(blnProviderDenialYN, "Y", "N"))
        .Parameters.Append .CreateParameter("@ProviderIncomeYN", adVarChar, adParamInput, 1, IIf(blnProviderIncomeYN, "Y", "N"))
        .Parameters.Append .CreateParameter("@SuperbillYN", adVarChar, adParamInput, 1, IIf(blnSuperbillYN, "Y", "N"))
        .Parameters.Append .CreateParameter("@OutstandingPatAcctYN", adVarChar, adParamInput, 1, IIf(blnOutstandingPatAcctYN, "Y", "N"))
        .Parameters.Append .CreateParameter("@PayerSessionsYN", adVarChar, adParamInput, 1, IIf(blnPayerSessionsYN, "Y", "N"))
        .Parameters.Append .CreateParameter("@CPTPayAnalysisYN", adVarChar, adParamInput, 1, IIf(blnCPTPayAnalysisYN, "Y", "N"))
        .Parameters.Append .CreateParameter("@ScheduleAnalysisYN", adVarChar, adParamInput, 1, IIf(blnScheduleAnalysisYN, "Y", "N"))
        .Parameters.Append .CreateParameter("@WriteOffLogYN", adVarChar, adParamInput, 1, IIf(blnWriteOffLogYN, "Y", "N"))
        .Parameters.Append .CreateParameter("@ApptDumpYN", adVarChar, adParamInput, 1, IIf(blnApptDumpYN, "Y", "N"))
        .Parameters.Append .CreateParameter("@PatientListYN", adVarChar, adParamInput, 1, IIf(blnPatientListYN, "Y", "N"))
        .Parameters.Append .CreateParameter("@PatBirthDateYN", adVarChar, adParamInput, 1, IIf(blnPatBirthDateYN, "Y", "N"))
        .Parameters.Append .CreateParameter("@ProgressNoteYN", adVarChar, adParamInput, 1, IIf(blnProgressNoteYN, "Y", "N"))
        .Parameters.Append .CreateParameter("@HcfaYN", adVarChar, adParamInput, 1, IIf(blnHcfaYN, "Y", "N"))
        .Parameters.Append .CreateParameter("@UnReconChecksYN", adVarChar, adParamInput, 1, IIf(blnUnReconChecksYN, "Y", "N"))
        .Parameters.Append .CreateParameter("@TimelyFilingYN", adVarChar, adParamInput, 1, IIf(blnTimelyFilingYN, "Y", "N"))
        .Parameters.Append .CreateParameter("@ProvDirect", adVarChar, adParamInput, 1, IIf(blnProvDirect, "Y", "N"))
        .Parameters.Append .CreateParameter("@VBYN", adVarChar, adParamInput, 1, IIf(blnVBYN, "Y", "N"))
        .Parameters.Append .CreateParameter("@UserName", adVarChar, adParamInput, 30, strUpdatedBy)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open(CONST_PSYQUEL_CNN)
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute , , adExecuteNoRecords

    'Check for errors
    If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If

    'Close the connection and free all resources
    Set cmdSQL.ActiveConnection = Nothing
    Set cmdSQL = Nothing
    cnnSQL.Close
    Set cnnSQL = Nothing

    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub


Public Function Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Long, _
                        ByVal strDisabledBy As String, Optional ByVal dtLastActiveDate As Date, _
                        Optional ByVal lngReasonID As Long) As String
'--------------------------------------------------------------------
'Date: 01/05/2000
'Author: Dave Richkun
'Description:  Flags a row in the tblUser table marking the row as
'              deleted or undeleted.  The user's related UserClinic
'              and UserRole records are intentionally left in tact.
'              The User's NT account is disabled.
'Parameters: blnDeleted - Boolean value identifying if the record is to
'               be deleted (True) or undeleted (False).
'            lngID - ID of the row in the table whose value will be
'               updated.
'            strDisabledBy - Login name of the user performing the action.
'Returns: The UserName of the account matching the lngID parameter.
'            This value is required in order to enable/disable the
'            NT account.
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    Dim cnn As ADODB.Connection
    Dim cmd As ADODB.Command
    Dim SQLErrorNum As Long
    Dim strUserName As String

    On Error GoTo ErrTrap:

    Set cnn = New ADODB.Connection
    Set cmd = New ADODB.Command
    Call cnn.Open(CONST_PSYQUEL_CNN)
    
    With cmd
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@UserID", adInteger, adParamInput, , lngID)
        .Parameters.Append .CreateParameter("@DisabledBy", adVarChar, adParamInput, 30, strDisabledBy)
        .Parameters.Append .CreateParameter("@LastActiveDate", adDBTimeStamp, adParamInput, , IIf(dtLastActiveDate = 0, Null, dtLastActiveDate))
        .Parameters.Append .CreateParameter("@ReasonID", adInteger, adParamInput, , lngReasonID)
        .Parameters.Append .CreateParameter("@UserName", adVarChar, adParamOutput, 30, "")
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
        If blnDeleted = True Then
            .CommandText = "uspDisableUser"
        Else
            .CommandText = "uspReenableUser"
        End If
        Set .ActiveConnection = cnn
        .Execute ', adExecuteNoRecords
        
        strUserName = .Parameters("@UserName").Value
    End With
    
    'Close the connection and free all resources
    cnn.Close
    Set cmd = Nothing
    Set cnn = Nothing

    Deleted = strUserName

    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

