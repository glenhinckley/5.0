
'--------------------------------------------------------------------
'Class Name: CEditUserBz                                                '
'Date: 01/03/2000                                                   '
'Author: Dave Richkun                                               '
'Description:  MTS business object designed to   methods         '
'               associated with the CEditUserDB class.                  '
'--------------------------------------------------------------------
'Revision History:                                                  '
' R001 09/22/2000 Richkun: Removed code for User-Clinic association in
'   Insert and Update  s.  Users (non-providers) are no longer associated
'   with Clinics, but with Providers instead.
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
' Lydia Orth 
Imports Psyquel.BusinessRules.CoreLibraryIII.dbStuff

Public Class CEditUserBz
    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CEditUserBz"
    Private Const FLD_CLINICID As Integer = 1
    Private Const ROLE_PROVIDER As Integer = 2
    Dim md As New ModCommon
    Public Function Insert(ByVal strUserName As String, ByVal strLastName As String, _
                           ByVal strFirstName As String, ByVal strMI As String, _
                           ByVal strSSN As String, ByVal strEmail As String, _
                           ByVal strOrigPassword As String, ByVal dtExpDate As Date, ByVal strAddedBy As String, _
                           ByVal varClinicOrProvider As Object, ByVal varRoles As Object, Optional ByVal blnVBYN As Boolean = True) As Long
        '--------------------------------------------------------------------
        'Date: 01/03/2000
        'Author: Dave Richkun
        'Description:  Inserts a row into the tblUsers table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        '            *** The varClinicOrProvider parameter will contain an array of Clinics
        '            if the user is designated as a Provider, or an arrya of Providers if the
        '            user is designated as an Office Manager. ***
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------

        Dim objUser As New ClinicDB.CEditUserDB
        Dim objUserProvider As New ClinicDB.CUserProviderDB
        Dim objUserRole As New ClinicDB.CUserRoleDB
        Dim lngUserID As Long
        Dim strErrMessage As String
        Dim intCTR As Integer
        Dim blnProvider As Boolean


        'Verify data before proceeding.
        If Not VerifyData(0, strUserName, strLastName, strFirstName, strOrigPassword, _
                          varRoles, varClinicOrProvider, strErrMessage, blnProvider) Then
            Exit Function
        End If

        'Insert the User record

        lngUserID = objUser.Insert(strUserName, strLastName, strFirstName, strMI, md.NumbersOnly(strSSN), _
                                   strEmail, strOrigPassword, dtExpDate, strAddedBy, blnVBYN)
        objUser = Nothing

        'Insert the UserRole records R003

        objUserRole.Insert(lngUserID, varRoles)
        objUserRole = Nothing

        If blnProvider = False Then
            'Insert the UserProvider records
            For intCTR = 0 To UBound(varClinicOrProvider)

                objUserProvider.Insert(lngUserID, varClinicOrProvider(intCTR), strAddedBy)
                objUserProvider = Nothing
            Next intCTR
        End If

        Insert = lngUserID


    End Function


    Public Sub Update(ByVal lngUserID As Long, ByVal strUserName As String, _
                      ByVal strLastName As String, ByVal strFirstName As String, _
                      ByVal strMI As String, ByVal strSSN As String, ByVal strEmail As String, _
                      ByVal varClinicOrProvider As Object, ByVal varRoles As Object, _
                      ByVal strUpdatedBy As String, Optional ByVal dtExpDate As Date = Nothing, Optional ByVal blnVBYN As Boolean = True)
        '--------------------------------------------------------------------
        'Date: 01/04/2000
        'Author: Dave Richkun
        'Description:  Updates a row in the tblUsers table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              Updated.
        '              *** The varClinicOrProvider parameter will contain an array
        '              of Clinics if the user is designated as a Provider, or an array
        '              of Providers if the user is designated as an Office Manager. ***
        'Returns: Null
        '--------------------------------------------------------------------

        Dim objUser As New ClinicDB.CEditUserDB
        Dim objUserProvider As New ClinicDB.CUserProviderDB
        Dim objUserRole As New ClinicDB.CUserRoleDB
        Dim strErrMessage As String
        Dim intCTR As Integer
        Dim blnProvider As Boolean


        If Not VerifyData(lngUserID, strUserName, strLastName, strFirstName, "anything", _
                          varRoles, varClinicOrProvider, strErrMessage, blnProvider) Then ' R003
            Exit Sub
        End If

        'Update the User record

        objUser.Update(lngUserID, strUserName, strLastName, strFirstName, strMI, md.NumbersOnly(strSSN), strEmail, strUpdatedBy, dtExpDate, blnVBYN) ', varRoles)  R003
        objUser = Nothing

        If blnProvider = False Then
            'Reset the UserProvider records, deleting existing associations first

            objUserProvider.DeleteUserProviders(lngUserID)

            For intCTR = 0 To UBound(varClinicOrProvider)
                objUserProvider.Insert(lngUserID, CLng(varClinicOrProvider(intCTR)), strUpdatedBy)
            Next intCTR
            objUserProvider = Nothing
        End If

        objUserRole.Insert(lngUserID, varRoles)
        objUserRole = Nothing



    End Sub
    Public Function UpdateFlags(ByVal lngID As Long, ByVal blnAdministratorYN As Boolean, _
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
        'Description:  Retrieves records from the tblProvider table.
        'Parameters:
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objUser As New ClinicDB.CEditUserDB



        objUser.UpdateFlags(lngID, blnAdministratorYN, _
                blnBillingYN, blnInsAdjustYN, _
                blnPatientNotesYN, blnGlanceAtDayYN, blnProviderBalanceYN, _
                blnStoreDocsYN, blnPsyquelPayYN, blnARPatientAgingDetailYN, _
                blnOutInsSummaryYN, blnPaidInsSummaryYN, blnARAgingProvidersYN, _
                blnBilledContactsYN, blnBillingAccountDetailYN, blnClaimCountYN, _
                blnDailyAccountActivityYN, blnPatAcctSummaryYN, _
                blnPatientPaymentLogYN, blnPatientStmtYN, blnPatientCertYN, _
                blnPayerSummaryYN, blnPostingHistoryYN, _
                blnProviderDenialYN, blnProviderIncomeYN, _
                blnSuperbillYN, blnOutstandingPatAcctYN, _
                blnPayerSessionsYN, blnCPTPayAnalysisYN, _
                blnScheduleAnalysisYN, blnWriteOffLogYN, _
                blnApptDumpYN, blnPatientListYN, _
                blnPatBirthDateYN, blnProgressNoteYN, _
                blnHcfaYN, blnUnReconChecksYN, _
                blnTimelyFilingYN, blnProvDirect, blnVBYN, strUpdatedBy)

        objUser = Nothing



    End Function

    Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Long, _
                            ByVal strDisabledBy As String, Optional ByVal dtLastActiveDate As Date = Nothing, _
                            Optional ByVal lngReasonID As Long = 0)
        '--------------------------------------------------------------------
        'Date: 01/05/2000
        'Author: Dave Richkun
        'Description:  Flags a row in the tblUser table marking the row as
        '              deleted or undeleted.  The user's related UserClinic
        '              and UserRole records are intentionally left in tact.
        'Parameters: blnDeleted - Boolean value identifying if the record is to
        '               be deleted (True) or undeleted (False).
        '            lngID - ID of the row in the table whose value will be
        '               updated.
        '            strDisabledBy - Login name of the user performing the action.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objUser As New ClinicDB.CEditUserDB
        Dim strUserName As String
        Dim dblProgID As Double



        strUserName = objUser.Deleted(blnDeleted, lngID, strDisabledBy, dtLastActiveDate, lngReasonID)



    End Sub


    Private Function VerifyData(ByVal lngUserID As Long, ByVal strUserName As String, _
                                ByVal strLastName As String, ByVal strFirstName As String, _
                                ByVal strOrigPassword As String, ByVal varRoles As Object, _
                                ByVal varClinicOrProvider As Object, ByRef strErrMessage As String, _
                                ByRef blnProvider As Boolean) As Boolean
        '--------------------------------------------------------------------
        'Date: 01/04/2000
        'Author: Dave Richkun
        'Description:  Verifies all required data has been provided by the user.
        'Parameters:  The values to be checked.
        'Returns: Boolean value identifying if all criteria has been satisfied.
        '--------------------------------------------------------------------
        Dim strObject As String
        Dim intCTR As Integer




        'what is the answer **********************************************************************************************************************
        Exit Function

        If Trim(strFirstName) = "" Then
            strErrMessage = "First Name is required."
            VerifyData = False
            Exit Function
        End If

        If Trim(strLastName) = "" Then
            strErrMessage = "Last Name is required."
            VerifyData = False
            Exit Function
        End If

        If Trim(strOrigPassword) = "" Then
            strErrMessage = "Original Password is required."
            VerifyData = False
            Exit Function
        End If

        If Trim(strUserName) = "" Then
            strErrMessage = "Login Name is required."
            VerifyData = False
            Exit Function
        End If

        strObject = "role"
        'Will cause Error 9 if array is empty
        For intCTR = 0 To UBound(varRoles)
            If CInt(varRoles(intCTR)) = ROLE_PROVIDER Then
                blnProvider = True
                Exit For
            End If
        Next intCTR

        If blnProvider = True Then
            strObject = "clinic"
            If UBound(varClinicOrProvider) Then
                'Will cause Error 9 if array is empty
            End If
        Else
            strObject = "provider"
            If UBound(varClinicOrProvider) Then
                'Will cause Error 9 if array is empty
            End If
        End If

        'Check for existance only when inserting data.
        If lngUserID = 0 And Exists(strUserName) Then
            strErrMessage = "A user with the login name of '" & strUserName & "' already exists."
            VerifyData = False
            Exit Function
        End If

        'If we get here, all is well...
        VerifyData = True

        Exit Function


        If Err.Number = 9 Then
            If strObject = "role" Then
                strErrMessage = "At least one Role must be selected."
                VerifyData = False
            Else
                'err.raise(Err.Number, , "At least 1 " & strObject & " must be selected")
            End If
        End If

    End Function


    Public Function Exists(ByVal strUserName As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 01/05/2000
        'Author: Dave Richkun
        'Description:  Determines if a UserName identical to the strUserName
        '              parameter already exists in the table.
        'Parameters: strUserName - User name to be checked
        'Returns: True if the user exists, false otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objUser As New ClinicDB.CUserDB
        Dim blnExists As Boolean


        blnExists = objUser.Exists(strUserName)

        Exists = blnExists

        objUser = Nothing



    End Function



End Class


