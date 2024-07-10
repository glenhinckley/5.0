VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CEditUserBz"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CEditUserBz                                                '
'Date: 01/03/2000                                                   '
'Author: Dave Richkun                                               '
'Description:  MTS business object designed to call methods         '
'               associated with the CEditUserDB class.                  '
'--------------------------------------------------------------------
'Revision History:                                                  '
' R001 09/22/2000 Richkun: Removed code for User-Clinic association in
'   Insert and Update calls.  Users (non-providers) are no longer associated
'   with Clinics, but with Providers instead.
'--------------------------------------------------------------------
Option Explicit

Private Const CLASS_NAME As String = "CEditUserBz"
Private Const FLD_CLINICID As Integer = 1
Private Const ROLE_PROVIDER As Integer = 2

Public Function Insert(ByVal strUserName As String, ByVal strLastName As String, _
                       ByVal strFirstName As String, ByVal strMI As String, _
                       ByVal strSSN As String, ByVal strEmail As String, _
                       ByVal strOrigPassword As String, ByVal dtExpDate As Date, ByVal strAddedBy As String, _
                       ByVal varClinicOrProvider As Variant, ByVal varRoles As Variant, Optional ByVal blnVBYN As Boolean = True) As Long
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

    Dim objUser As ClinicDB.CEditUserDB
    Dim objUserProvider As ClinicDB.CUserProviderDB
    Dim objUserRole As ClinicDB.CUserRoleDB
    Dim lngUserID As Long
    Dim strErrMessage As String
    Dim intCTR As Integer
    Dim blnProvider As Boolean
    
    On Error GoTo ErrTrap:

    'Verify data before proceeding.
    If Not VerifyData(0, strUserName, strLastName, strFirstName, strOrigPassword, _
                      varRoles, varClinicOrProvider, strErrMessage, blnProvider) Then
        GoTo ErrTrap
    End If
    
    'Insert the User record
    Set objUser = CreateObject("ClinicDB.CEditUserDB")
    lngUserID = objUser.Insert(strUserName, strLastName, strFirstName, strMI, NumbersOnly(strSSN), _
                               strEmail, strOrigPassword, dtExpDate, strAddedBy, blnVBYN)
    Set objUser = Nothing
                  
    'Insert the UserRole records R003
    Set objUserRole = CreateObject("ClinicDB.CUserRoleDB")
    Call objUserRole.Insert(lngUserID, varRoles)
    Set objUserRole = Nothing
    
    If blnProvider = False Then
        'Insert the UserProvider records
        For intCTR = 0 To UBound(varClinicOrProvider)
            Set objUserProvider = CreateObject("ClinicDB.CUserProviderDB")
            Call objUserProvider.Insert(lngUserID, varClinicOrProvider(intCTR), strAddedBy)
            Set objUserProvider = Nothing
        Next intCTR
    End If

    Insert = lngUserID
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    On Error GoTo 0
    GetObjectContext.SetAbort
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
    
End Function


Public Sub Update(ByVal lngUserID As Long, ByVal strUserName As String, _
                  ByVal strLastName As String, ByVal strFirstName As String, _
                  ByVal strMI As String, ByVal strSSN As String, ByVal strEmail As String, _
                  ByVal varClinicOrProvider As Variant, ByVal varRoles As Variant, _
                  ByVal strUpdatedBy As String, Optional ByVal dtExpDate As Date, Optional ByVal blnVBYN As Boolean = True)
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

    Dim objUser As ClinicDB.CEditUserDB
    Dim objUserProvider As ClinicDB.CUserProviderDB
    Dim objUserRole As ClinicDB.CUserRoleDB
    Dim strErrMessage As String
    Dim intCTR As Integer
    Dim blnProvider As Boolean

    On Error GoTo ErrTrap:

    If Not VerifyData(lngUserID, strUserName, strLastName, strFirstName, "anything", _
                      varRoles, varClinicOrProvider, strErrMessage, blnProvider) Then ' R003
        GoTo ErrTrap
    End If
    
    'Update the User record
    Set objUser = CreateObject("ClinicDB.CEditUserDB")
    Call objUser.Update(lngUserID, strUserName, strLastName, strFirstName, strMI, _
                        NumbersOnly(strSSN), strEmail, strUpdatedBy, dtExpDate, blnVBYN) ', varRoles)  R003
    Set objUser = Nothing

    If blnProvider = False Then
        'Reset the UserProvider records, deleting existing associations first
        Set objUserProvider = CreateObject("ClinicDB.CUserProviderDB")
        Call objUserProvider.DeleteUserProviders(lngUserID)

        For intCTR = 0 To UBound(varClinicOrProvider)
            Call objUserProvider.Insert(lngUserID, CLng(varClinicOrProvider(intCTR)), strUpdatedBy)
        Next intCTR
        Set objUserProvider = Nothing
    End If
    
    'Insert the UserRole records R003
    Set objUserRole = CreateObject("ClinicDB.CUserRoleDB")
    Call objUserRole.Insert(lngUserID, varRoles)
    Set objUserRole = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Sub
    
ErrTrap:
    On Error GoTo 0
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
    
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
    
    Dim objUser As ClinicDB.CEditUserDB
    
    On Error GoTo ErrTrap:
    
    Set objUser = CreateObject("ClinicDB.CEditUserDB")
    
    Call objUser.UpdateFlags(lngID, blnAdministratorYN, _
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
    
    Set objUser = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set objUser = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Long, _
                        ByVal strDisabledBy As String, Optional ByVal dtLastActiveDate As Date, _
                        Optional ByVal lngReasonID As Long)
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

    Dim objUser As ClinicDB.CEditUserDB
    Dim strUserName As String
    Dim dblProgID As Double

    On Error GoTo ErrTrap:

    Set objUser = CreateObject("ClinicDB.CEditUserDB")
    
    strUserName = objUser.Deleted(blnDeleted, lngID, strDisabledBy, dtLastActiveDate, lngReasonID)

    'Signal successful completion
    GetObjectContext.SetComplete

    Set objUser = Nothing

    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub


Private Function VerifyData(ByVal lngUserID As Long, ByVal strUserName As String, _
                            ByVal strLastName As String, ByVal strFirstName As String, _
                            ByVal strOrigPassword As String, ByVal varRoles As Variant, _
                            ByVal varClinicOrProvider As Variant, ByRef strErrMessage As String, _
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
     
    On Error GoTo ErrTrap:

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

ErrTrap:
    If Err.Number = 9 Then
        If strObject = "role" Then
            strErrMessage = "At least one Role must be selected."
            VerifyData = False
        Else
            Err.Raise Err.Number, , "At least 1 " & strObject & " must be selected"
        End If
    End If
    
End Function


Public Function Exists(strUserName As String) As Boolean
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

    Dim objUser As ClinicDB.CUserDB
    Dim blnExists As Boolean

    On Error GoTo ErrTrap:

    Set objUser = CreateObject("ClinicDB.CUserDB")
    blnExists = objUser.Exists(strUserName)

    Exists = blnExists

    Set objUser = Nothing

    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Function





