Option Strict Off
Option Explicit On
Imports ADODB
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CEditUserDB
    '--------------------------------------------------------------------
    'Class Name: CEditUserDB                                                '
    'Date: 04/06/2000                                                   '
    'Author: Dave Richkun                                               '
    'Description:  MTS object designed to host methods associated with  '
    '              adding and editing Users.
    '--------------------------------------------------------------------
    'Revision History:                                                  '
    '--------------------------------------------------------------------
    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CEditUserDB"

    Public Function Insert(ByVal strUserName As String, ByVal strLastName As String, ByVal strFirstName As String, ByVal strMI As String, ByVal strSSN As String, ByVal strEmail As String, ByVal strOrigPassword As String, ByVal dtExpDate As Date, ByVal strAddedBy As String, Optional ByVal blnVBYN As Boolean = True) As Integer
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



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        cmdSQL.CommandText = "uspInsUser"
        cmdSQL.CommandType = ADODB.CommandTypeEnum.adCmdStoredProc

        'Create the parameter objects
        With cmdSQL
            .Parameters.Append(.CreateParameter(, ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamReturnValue))
            .Parameters.Append(.CreateParameter("@UserName", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@LastName", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 20, strLastName))
            .Parameters.Append(.CreateParameter("@FirstName", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 20, strFirstName))
            .Parameters.Append(.CreateParameter("@MI", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, strMI))
            .Parameters.Append(.CreateParameter("@SSN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 12, strSSN))
            .Parameters.Append(.CreateParameter("@Email", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 60, strEmail))
            .Parameters.Append(.CreateParameter("@OrigPassword", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 10, strOrigPassword))
            'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
            .Parameters.Append(.CreateParameter("@PwdExpDate", ADODB.DataTypeEnum.adDBTimeStamp, ADODB.ParameterDirectionEnum.adParamInput, , IIf(dtExpDate = System.DateTime.FromOADate(0), System.DBNull.Value, dtExpDate)))
            .Parameters.Append(.CreateParameter("@AddedBy", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 30, strAddedBy))
            .Parameters.Append(.CreateParameter("@VBYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnVBYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@ErrorNum", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute() '()

        Insert = cmdSQL.Parameters(0).Value

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
            Exit Function
        End If

        'Close the connection and free all resources
        cnnSQL.Close()

        'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmdSQL = Nothing
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing



    End Function


    Public Sub Update(ByVal lngUserID As Integer, ByVal strUserName As String, ByVal strLastName As String, ByVal strFirstName As String, ByVal strMI As String, ByVal strSSN As String, ByVal strEmail As String, ByVal strUpdatedBy As String, Optional ByVal dtExpDate As Date = #12:00:00 AM#, Optional ByVal blnVBYN As Boolean = True)
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



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspUpdUser"
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
            .Parameters.Append(.CreateParameter("@UserID", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamInput, , lngUserID))
            .Parameters.Append(.CreateParameter("@UserName", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@LastName", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 20, strLastName))
            .Parameters.Append(.CreateParameter("@FirstName", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 20, strFirstName))
            .Parameters.Append(.CreateParameter("@MI", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, strMI))
            .Parameters.Append(.CreateParameter("@SSN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 12, strSSN))
            .Parameters.Append(.CreateParameter("@Email", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 60, strEmail))
            '.Parameters.Append .CreateParameter("@DfltClinicID", adInteger, adParamInput, , lngDefaultClinicID)
            '.Parameters.Append .CreateParameter("@PrimaryRoleID", adInteger, adParamInput, , lngPrimaryRoleID)
            .Parameters.Append(.CreateParameter("@UpdatedBy", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 30, strUpdatedBy))
            'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
            .Parameters.Append(.CreateParameter("@PwdExpDate", ADODB.DataTypeEnum.adDBTimeStamp, ADODB.ParameterDirectionEnum.adParamInput, , IIf(dtExpDate = System.DateTime.FromOADate(0), System.DBNull.Value, dtExpDate)))
            .Parameters.Append(.CreateParameter("@VBYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnVBYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@ErrorNum", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute(, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
            Exit Sub
        End If

        'Close the connection and free all resources
        cnnSQL.Close()

        'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmdSQL = Nothing
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing



    End Sub
    Public Sub UpdateFlags(ByVal lngID As Integer, ByVal blnAdministratorYN As Boolean, ByVal blnBillingYN As Boolean, ByVal blnInsAdjustYN As Boolean, ByVal blnPatientNotesYN As Boolean, ByVal blnGlanceAtDayYN As Boolean, ByVal blnProviderBalanceYN As Boolean, ByVal blnStoreDocsYN As Boolean, ByVal blnPsyquelPayYN As Boolean, ByVal blnARPatientAgingDetailYN As Boolean, ByVal blnOutInsSummaryYN As Boolean, ByVal blnPaidInsSummaryYN As Boolean, ByVal blnARAgingProvidersYN As Boolean, ByVal blnBilledContactsYN As Boolean, ByVal blnBillingAccountDetailYN As Boolean, ByVal blnClaimCountYN As Boolean, ByVal blnDailyAccountActivityYN As Boolean, ByVal blnPatAcctSummaryYN As Boolean, ByVal blnPatientPaymentLogYN As Boolean, ByVal blnPatientStmtYN As Boolean, ByVal blnPatientCertYN As Boolean, ByVal blnPayerSummaryYN As Boolean, ByVal blnPostingHistoryYN As Boolean, ByVal blnProviderDenialYN As Boolean, ByVal blnProviderIncomeYN As Boolean, ByVal blnSuperbillYN As Boolean, ByVal blnOutstandingPatAcctYN As Boolean, ByVal blnPayerSessionsYN As Boolean, ByVal blnCPTPayAnalysisYN As Boolean, ByVal blnScheduleAnalysisYN As Boolean, ByVal blnWriteOffLogYN As Boolean, ByVal blnApptDumpYN As Boolean, ByVal blnPatientListYN As Boolean, ByVal blnPatBirthDateYN As Boolean, ByVal blnProgressNoteYN As Boolean, ByVal blnHcfaYN As Boolean, ByVal blnUnReconChecksYN As Boolean, ByVal blnTimelyFilingYN As Boolean, ByVal blnProvDirect As Boolean, ByVal blnVBYN As Boolean, ByVal strUpdatedBy As String)
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



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        With cmdSQL
            .CommandText = "uspUpdUserFlags"
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamInput, , lngID))
            .Parameters.Append(.CreateParameter("@AdministratorYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnAdministratorYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@BillingYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnBillingYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@InsAdjustYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnInsAdjustYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@PatientNotesYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnPatientNotesYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@GlanceAtDayYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnGlanceAtDayYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@ProviderBalanceYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnProviderBalanceYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@StoreDocsYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnStoreDocsYN, "Y", "N"))) '<--
            .Parameters.Append(.CreateParameter("@PsyquelPayYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnPsyquelPayYN, "Y", "N"))) '<--

            .Parameters.Append(.CreateParameter("@ARPatientAgingDetailYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnARPatientAgingDetailYN, "Y", "N"))) '<--
            .Parameters.Append(.CreateParameter("@OutInsSummaryYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnOutInsSummaryYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@PaidInsSummaryYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnPaidInsSummaryYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@ARAgingProvidersYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnARAgingProvidersYN, "Y", "N"))) '<--
            .Parameters.Append(.CreateParameter("@BilledContactsYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnBilledContactsYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@BillingAccountDetailYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnBillingAccountDetailYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@ClaimCountYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnClaimCountYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@DailyAccountActivityYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnDailyAccountActivityYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@PatAcctSummaryYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnPatAcctSummaryYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@PatientPaymentLogYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnPatientPaymentLogYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@PatientStmtYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnPatientStmtYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@PatientCertYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnPatientCertYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@PayerSummaryYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnPayerSummaryYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@PostingHistoryYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnPostingHistoryYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@ProviderDenialYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnProviderDenialYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@ProviderIncomeYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnProviderIncomeYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@SuperbillYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnSuperbillYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@OutstandingPatAcctYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnOutstandingPatAcctYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@PayerSessionsYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnPayerSessionsYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@CPTPayAnalysisYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnCPTPayAnalysisYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@ScheduleAnalysisYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnScheduleAnalysisYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@WriteOffLogYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnWriteOffLogYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@ApptDumpYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnApptDumpYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@PatientListYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnPatientListYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@PatBirthDateYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnPatBirthDateYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@ProgressNoteYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnProgressNoteYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@HcfaYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnHcfaYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@UnReconChecksYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnUnReconChecksYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@TimelyFilingYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnTimelyFilingYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@ProvDirect", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnProvDirect, "Y", "N")))
            .Parameters.Append(.CreateParameter("@VBYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnVBYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@UserName", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 30, strUpdatedBy))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute(, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)

        'Check for errors
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        'Close the connection and free all resources
        'UPGRADE_NOTE: Object cmdSQL.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmdSQL.ActiveConnection = Nothing
        'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmdSQL = Nothing
        cnnSQL.Close()
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing


    End Sub


    Public Function Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Integer, ByVal strDisabledBy As String, Optional ByVal dtLastActiveDate As Date = #12:00:00 AM#, Optional ByVal lngReasonID As Integer = 0) As String
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

        Dim cnn As New ADODB.Connection
        Dim cmd As New ADODB.Command
        Dim SQLErrorNum As Integer
        Dim strUserName As String



        cnn.Open(_ConnectionString)

        With cmd
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
            .Parameters.Append(.CreateParameter("@UserID", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamInput, , lngID))
            .Parameters.Append(.CreateParameter("@DisabledBy", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 30, strDisabledBy))
            'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
            .Parameters.Append(.CreateParameter("@LastActiveDate", ADODB.DataTypeEnum.adDBTimeStamp, ADODB.ParameterDirectionEnum.adParamInput, , IIf(dtLastActiveDate = System.DateTime.FromOADate(0), System.DBNull.Value, dtLastActiveDate)))
            .Parameters.Append(.CreateParameter("@ReasonID", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamInput, , lngReasonID))
            .Parameters.Append(.CreateParameter("@UserName", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamOutput, 30, ""))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamOutput, , 0))
            If blnDeleted = True Then
                .CommandText = "uspDisableUser"
            Else
                .CommandText = "uspReenableUser"
            End If
            .ActiveConnection = cnn
            .Execute() ', adExecuteNoRecords

            strUserName = .Parameters("@UserName").Value
        End With

        'Close the connection and free all resources
        cnn.Close()
        'UPGRADE_NOTE: Object cmd may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmd = Nothing
        'UPGRADE_NOTE: Object cnn may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnn = Nothing

        Deleted = strUserName



    End Function
End Class