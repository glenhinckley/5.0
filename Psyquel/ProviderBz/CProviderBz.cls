
'--------------------------------------------------------------------
'Class Name: CProviderBz
'Date: 02/03/2000
'Author: Rick "Boom Boom" Segura
'Denis J Finkel
'Description:  MTS business object designed to   methods
'              associated with the CProviderDB class.
'--------------------------------------------------------------------
' R001 03/24/2000 Richkun: Included logic to auto-disable Provider-Clinics
'           that may have been removed from a list of previously active
'           Provider-Clinics
' R002 04/11/2000 Richkun: Added support for TaxIDs and Practice Numbers
' R003 04/28/2000 Segura: Deleted method now also  s on User.Deleted
' R004 08/24/2000 Segura:  Leak checks on code optimizations
' R005 05/07/2001 Richkun:  Added UpdateProfile() method
' R006 07/18/2001 Pena:  Updated Insert() and Update() to take in fldSupervisorID,
'                       fldChkToOtherYN, and fldCheckTo
' R007 08/23/2001 Pena: Modified R002 to not delete TaxIDs on update (orphans rows in tblEncounterLog)
' R008 09/13/2001 Richkun: Added support for tblProvider.fldStartDate
' R009 09/25/2001 Richkun: Added support for Provider Intake Profiles
'--------------------------------------------------------------------
Option Explicit On

Imports Psyquel.BusinessRules.CoreLibraryIII.dbStuff
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

Public Class CProviderBZ
    Private Const CLASS_NAME As String = "CProviderBz"
    Private Const PROVIDER_TABLE As String = "tblProvider"
    Private Const PROVIDERCLINIC_TABLE As String = "tblProviderClinic"
    Private Const FLD_PROVIDERCLINICID As Integer = 0
    Private Const FLD_CLINICID As Integer = 1
    Private Const FLD_BUSINESSNAME As Integer = 2
    Private Const FLD_POINTOFCONTACT As Integer = 3
    Private Const FLD_ACCEPTNEWPAT As Integer = 4
    Private Const FLD_ALLOWPATSCHED As Integer = 5
    Private md As New ModCommon
    'R001
    Private Const FLD_TAXID As Integer = 0
    Private Const FLD_TAXID_TYPE As Integer = 1
    Private Const FLD_TAXID_NOTES As Integer = 2
    Private Const FLD_TAXID_DIRTY As Integer = 3
    Private Const FLD_TAXID_ID As Integer = 4
    Private Const FLD_TAXID_GROUP As Integer = 5

    Private Const FLD_PRACTICE_STATE As Integer = 0
    Private Const FLD_PRACTICE_INSCO As Integer = 1
    Private Const FLD_PRACTICE_NUM As Integer = 2
    Private Const FLD_PRACTICE_GROUP As Integer = 3
    Private Const FLD_PRACTICE_ENROLLED_YN As Integer = 4
    Private Const FLD_PRACTICE_ENROLLDATE As Integer = 5
    Private Const FLD_PRACTICE_NOTES As Integer = 6
    Private Const FLD_PRACTICE_SECNUM As Integer = 7
    Private Const FLD_PRACTICE_SUBMITTERID As Integer = 8
    Private Const FLD_PRACTICE_SUPERVISORID As Integer = 9
    Private Const FLD_PRACTICE_TAXID As Integer = 10
    Private Const FLD_PRACTICE_TAXIDTYPE As Integer = 11
    Private Const FLD_PRACTICE_EINDESC As Integer = 12
    Private Const FLD_PRACTICE_INNETWORKYN As Integer = 13
    Private Const FLD_PRACTICE_ACKASSIGNYN As Integer = 14
    Private Const FLD_PRACTICE_INDIVIDUALNPIONLYYN As Integer = 15


    Private _ConnectionString As String = String.Empty



    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property










    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False, _
                          Optional ByVal strWhere As String = "", _
                          Optional ByVal strOrderBy As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 02/03/2000
        'Author: Rick "Boom Boom" Segura
        'Description:  Retrieves records from the tblProvider table.
        'Parameters:  blnIncludeDisabled - Optional parameter that identifies if
        '               records flagged as 'Disabled' or 'De-activated' are to be
        '               included in the record set. The default value is False.
        '             strWhere - Optional 'Where' clause of the SQL statement by
        '               which records will be filtered.  If the parameter is not
        '               supplied, all records will be retrieved.
        '             strOrderBy - Optional 'Order By' clause in which retrieved
        '               records will be sorted.  The default is set to the
        '               'fldMaritalStatus' column
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objProvider As New ProviderDB.CProviderDB
        Dim rstSQL As New ADODB.Recordset
        objProvider.ConnectionString = _ConnectionString

        rstSQL = objProvider.Fetch(blnIncludeDisabled, strWhere, strOrderBy)
        Fetch = rstSQL

        objProvider = Nothing

        'Signal successful completion
    End Function

    Public Function Insert(ByVal strUserName As String, ByVal strLast As String, _
            ByVal strFirst As String, ByVal strMI As String, _
            ByVal strAddress1 As String, ByVal strAddress2 As String, _
            ByVal strCity As String, ByVal strState As String, _
            ByVal strZip As String, ByVal strMailAddress1 As String, _
            ByVal strMailAddress2 As String, ByVal strMailCity As String, _
            ByVal strMailState As String, ByVal strMailZip As String, _
            ByVal dtBirthDate As Date, ByVal strNPI As String, ByVal strDEA As String, ByVal strTaxonomy As String, _
            ByVal strPhone1 As String, ByVal strPhone2 As String, _
            ByVal strFax As String, ByVal strBilling As String, _
            ByVal strEmail As String, ByVal strSSN As String, _
            ByVal strOrigPassword As String, ByVal dtExpDate As Date, ByVal varDegreeCredential As Object, ByVal varRecognizedCredential As Object, _
            ByVal varSignature As Object, _
            ByVal strAddedBy As String, ByVal varRoles As Object, _
            ByVal varClinics As Object, ByVal varTaxIDs As Object, _
            ByVal varPracticeNums As Object, ByVal varSettings As Object, ByVal lngLockBoxID As Long, _
            ByVal lngSupervisorID As Long, ByVal lngServiceGroupID As Long, ByVal lngGroupID As Long, _
            ByVal strCheckTo As String, ByVal dtStartDate As Date, ByVal blnVBYN As Boolean, _
            ByVal dblSubmitRate As Double, ByVal dblCollectRate As Double, ByVal dblCollectPercent As Double, ByVal intFreeSubmits As Integer, _
            ByVal lngServiceID As Long, ByVal strServicePeriod As String, ByVal dtServiceStartDate As Date, _
            ByVal dtServiceEndDate As Date, ByVal dblServiceAmount As Double, ByVal lngSalesRepID As Long, _
            ByVal lngMarketSourceID As Long, ByVal lngSchInterval As Long, ByVal dtSchStartTime As Date, _
            ByVal dtSchEndTime As Date, ByVal strWebSite As String, ByVal strNotes As String) As Long

        '--------------------------------------------------------------------
        'Date: 02/03/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Inserts a row into the tblProvider table utilizing   '
        '              a stored procedure.                                  '
        'Parameters: strLast - Provider's Last Name                         '
        '            strFirst - "" First Name                               '
        '            strMI - "" Middle Initial                              '
        '            strAddress1 - "" Address line #1                       '
        '            strAddress2 - "" Address line #2                       '
        '            strCity - "" City                                      '
        '            strState - "" State                                    '
        '            strZip - "" Zip                                        '
        '            strPhone1 - "" Phone #1                                '
        '            strPhone2 - "" Phone #2                                '
        '            strFax - "" Fax #                                      '
        '            strPager - "" Pager #                                  '
        '            strEmail - "" E-mail Address                           '
        '            strSSN - "" Social Security #                          '
        '            varDegreeCredential - "" scholastic credential         '
        '            varRecognizedCredential - "" Industry recognized credential
        '            strStampLine1 - "" stamp line 1                        '
        '            strStampLine2 - "" stamp line 2                        '
        '            strStampLine3 - "" stamp line 3                        '
        '            varSignature - "" signature file                       '
        '            varClinics - provider-clinic info                      '
        '            strAddedby - Login ID of user adding provider          '
        'Returns: ID (Primary Key) of the row inserted                      '
        '--------------------------------------------------------------------
        ' Revision History:                                                 '
        '   R001 - Added varTaxIDs and varPracticeNums parameters           '
        '--------------------------------------------------------------------

        Dim objProvider As New ProviderDB.CProviderDB
        Dim objUser As New ClinicBZ.CEditUserBz
        Dim objProviderClinic As New ProviderDB.CProviderClinicDB
        Dim objProviderTaxID As New ProviderDB.CProviderTaxIDDB
        Dim objProviderNum As New ProviderDB.CProviderNumDB
        Dim lngCount As Long
        Dim lngUserID As Long
        Dim lngInsCompanyID As Long
        Dim strErrMessage As String
        Dim strPatStmtYNE, strApptReminder, strAchSiteID, strAchSiteKey, strAchAPIKey, strStampLine1, strStampLine2, strStampline3 As String
        Dim strVirtualClientURL, strVirtualPatientURL, strSpecialPriceReason As String
        Dim dtTrialEndDate As Date


        objProvider.ConnectionString = _ConnectionString
        objUser.ConnectionString = _ConnectionString
        objProviderClinic.ConnectionString = _ConnectionString
        objProviderTaxID.ConnectionString = _ConnectionString
        objProviderNum.ConnectionString = _ConnectionString

        'Verify data before proceeding.
        If Not VerifyData(0, strLast, strFirst, strMI, strOrigPassword, varDegreeCredential, _
                          varRecognizedCredential, varClinics, varTaxIDs, lngServiceGroupID, _
                          lngGroupID, strErrMessage) Then
            Exit Function
        End If

        'Insert a row into tblUser
        lngUserID = objUser.Insert(strUserName, strLast, strFirst, strMI, strSSN, _
                                   strEmail, strOrigPassword, dtExpDate, strAddedBy, varClinics, varRoles, blnVBYN)
        objUser = Nothing

        If IsArray(varSettings) Then
            strPatStmtYNE = varSettings(0)
            strApptReminder = varSettings(1)
            strAchSiteID = varSettings(2)
            strAchSiteKey = varSettings(3)
            strAchAPIKey = varSettings(4)
            strStampLine1 = varSettings(5)
            strStampLine2 = varSettings(6)
            strStampline3 = varSettings(7)
            strVirtualClientURL = varSettings(8)
            strVirtualPatientURL = varSettings(9)
            strSpecialPriceReason = varSettings(10)
            dtTrialEndDate = varSettings(11)
        End If
        'Insert a row into tblProvider
        objProvider.Insert(lngUserID, strAddress1, strAddress2, strCity, strState, md.NumbersOnly(strZip), strMailAddress1, strMailAddress2, _
                              strMailCity, strMailState, md.NumbersOnly(strMailZip), dtBirthDate, strNPI, strDEA, strTaxonomy, _
                              md.NumbersOnly(strPhone1), md.NumbersOnly(strPhone2), md.NumbersOnly(strFax), _
                              md.NumbersOnly(strBilling), varDegreeCredential, varRecognizedCredential, _
                              strStampLine1, strStampLine2, strStampline3, varSignature, lngLockBoxID, _
                              lngSupervisorID, lngServiceGroupID, lngGroupID, _
                              Trim(strCheckTo), dtStartDate, dblSubmitRate, dblCollectRate, dblCollectPercent, strSpecialPriceReason, _
                              intFreeSubmits, dtTrialEndDate, strPatStmtYNE, strApptReminder, strAchSiteID, strAchSiteKey, strAchAPIKey, lngServiceID, _
                              strServicePeriod, dtServiceStartDate, _
                              dtServiceEndDate, dblServiceAmount, lngSalesRepID, lngMarketSourceID, lngSchInterval, dtSchStartTime, _
                              dtSchEndTime, strWebSite, strVirtualClientURL, strVirtualPatientURL, strNotes, strAddedBy)

        objProvider = Nothing

        'Insert row(s) into tblProviderClinic
        For lngCount = 0 To UBound(varClinics, 2)
            If OkToAssociate(lngUserID, strLast, strFirst, strMI, varClinics(FLD_CLINICID, lngCount)) Then
                objProviderClinic.Insert(lngUserID, varClinics(FLD_CLINICID, lngCount), strAddedBy, _
                    varClinics(FLD_ALLOWPATSCHED, lngCount), varClinics(FLD_ACCEPTNEWPAT, lngCount), _
                    varClinics(FLD_POINTOFCONTACT, lngCount))
                objProviderClinic = Nothing
            End If
        Next

        'R001 - Insert row(s) into tblProviderTaxID
        If IsArray(varTaxIDs) Then
            For lngCount = 0 To UBound(varTaxIDs, 1)
                objProviderTaxID.Insert(lngUserID, varTaxIDs(lngCount, FLD_TAXID), _
                      varTaxIDs(lngCount, FLD_TAXID_TYPE), varTaxIDs(lngCount, FLD_TAXID_NOTES), varTaxIDs(lngCount, FLD_TAXID_GROUP))
            Next
            objProviderTaxID = Nothing
        End If

        'R001 - Insert row(s) into tblProviderPracticeNums
        If IsArray(varPracticeNums) Then
            For lngCount = 0 To UBound(varPracticeNums, 1)
                If varPracticeNums(lngCount, FLD_PRACTICE_INSCO) = "" Then
                    lngInsCompanyID = -1
                Else
                    lngInsCompanyID = CLng(varPracticeNums(lngCount, FLD_PRACTICE_INSCO))
                End If

                objProviderNum.Insert(lngUserID, varPracticeNums(lngCount, FLD_PRACTICE_STATE), _
              lngInsCompanyID, varPracticeNums(lngCount, FLD_PRACTICE_NUM), varPracticeNums(lngCount, FLD_PRACTICE_SECNUM), varPracticeNums(lngCount, FLD_PRACTICE_SUBMITTERID), _
              IIf(varPracticeNums(lngCount, FLD_PRACTICE_GROUP) = "Yes", True, False), IIf(varPracticeNums(lngCount, FLD_PRACTICE_ENROLLED_YN) = "Yes", True, False), _
              IIf(IsDate(varPracticeNums(lngCount, FLD_PRACTICE_ENROLLDATE)), varPracticeNums(lngCount, FLD_PRACTICE_ENROLLDATE), 0), _
              CLng(varPracticeNums(lngCount, FLD_PRACTICE_SUPERVISORID)), varPracticeNums(lngCount, FLD_PRACTICE_TAXID), CLng(varPracticeNums(lngCount, FLD_PRACTICE_TAXIDTYPE)), _
              varPracticeNums(lngCount, FLD_PRACTICE_EINDESC), IIf(varPracticeNums(lngCount, FLD_PRACTICE_INNETWORKYN) = "Yes", True, False), _
              IIf(varPracticeNums(lngCount, FLD_PRACTICE_ACKASSIGNYN) = "Yes", True, False), IIf(varPracticeNums(lngCount, FLD_PRACTICE_INDIVIDUALNPIONLYYN) = "Yes", True, False), _
              varPracticeNums(lngCount, FLD_PRACTICE_NOTES))

            Next
            objProviderNum = Nothing
        End If



        Insert = lngUserID

        objProvider = Nothing
        objUser = Nothing
        objProviderClinic = Nothing
        objProviderTaxID = Nothing
        objProviderNum = Nothing


    End Function

    Public Sub Update(ByVal lngID As Long, ByVal strLast As String, ByVal strFirst As String, _
                      ByVal strUserName As String, ByVal strMI As String, ByVal strAddress1 As String, _
                      ByVal strAddress2 As String, ByVal strCity As String, ByVal strState As String, _
                      ByVal strZip As String, ByVal strMailAddress1 As String, _
                      ByVal strMailAddress2 As String, ByVal strMailCity As String, _
                      ByVal strMailState As String, ByVal strMailZip As String, _
                      ByVal dtBirthDate As Date, ByVal strNPI As String, ByVal strDEA As String, ByVal strTaxonomy As String, _
                      ByVal strPhone1 As String, ByVal strPhone2 As String, _
                      ByVal strFax As String, ByVal strBilling As String, ByVal strEmail As String, _
                      ByVal strSSN As String, ByVal dtExpDate As Date, ByVal varDegreeCredential As Object, _
                      ByVal varRecognizedCredential As Object, ByVal varSignature As Object, _
                      ByVal varRoles As Object, ByVal varClinics As Object, ByVal varTaxIDs As Object, _
                      ByVal varPracticeNums As Object, ByVal varSettings As Object, ByVal lngLockBoxID As Long, ByVal lngSupervisorID As Long, _
                      ByVal lngServiceGroupID As Long, ByVal lngGroupID As Long, _
                      ByVal strCheckTo As String, ByVal dtStartDate As Date, ByVal dblSubmitRate As Double, _
                      ByVal dblCollectRate As Double, ByVal dblCollectPercent As Double, ByVal dtAutoCloseDate As Date, ByVal intFreeSubmits As Integer, _
                      ByVal intFreeRemain As Integer, ByVal lngServiceID As Long, ByVal strServicePeriod As String, ByVal dtServiceStartDate As Date, _
                      ByVal dtServiceEndDate As Date, ByVal dblServiceAmount As Double, ByVal lngSalesRepID As Long, _
                      ByVal lngMarketSourceID As Long, ByVal lngSchInterval As Long, ByVal dtSchStartTime As Date, _
                      ByVal dtSchEndTime As Date, ByVal strWebSite As String, _
                      ByVal strNotes As String, ByVal strUpdatedBy As String)
        '--------------------------------------------------------------------
        'Date: 02/03/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Updates a row into the tblProvider table.            '
        'Parameters: lngID - ID of the row in the table whose value will be '
        '                    updated.                                       '
        '            lngID - ID of provider to update                       '
        '            strLast - Provider's Last Name                         '
        '            strFirst - "" First Name                               '
        '            strMI - "" Middle Initial                              '
        '            strAddress1 - "" Address line #1                       '
        '            strAddress2 - "" Address line #2                       '
        '            strCity - "" City                                      '
        '            strState - "" State                                    '
        '            strZip - "" Zip                                        '
        '            strPhone1 - "" Phone #1                                '
        '            strPhone2 - "" Phone #2                                '
        '            strFax - "" Fax #                                      '
        '            strPager - "" Pager #                                  '
        '            strEmail - "" E-mail Address                           '
        '            strSSN - "" Social Security #                          '
        '            varDegreeCredential - "" scholastic credential         '
        '            varRecognizedCredential - "" Industry recognized credential
        '            varClinics - associated clinic info                    '
        '            strUpdatedBy - Login ID of user adding provider        '
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        ' R003 - Added varTaxIDs and varPracticeNums parameters             '
        '--------------------------------------------------------------------
        Dim objProvider As New ProviderDB.CProviderDB
        Dim objUser As New ClinicBZ.CEditUserBz
        Dim objUserClinic As New ClinicDB.CUserClinicDB
        Dim objProviderClinic As New ProviderDB.CProviderClinicDB
        Dim objProviderTaxID As New ProviderDB.CProviderTaxIDDB
        Dim objProviderNumber As New ProviderDB.CProviderNumDB
        Dim strErrMessage As String
        Dim lngCount As Long
        Dim lngProviderClinic As Long
        Dim lngInsCompanyID As Integer
        Dim arrTaxIDs As Object
        Dim blnVBYN As Boolean
        Dim strPatStmtYNE, strApptReminder, strAchSiteID, strAchSiteKey, strAchAPIKey, strStampLine1, strStampLine2, strStampline3 As String
        Dim strVirtualClientURL, strVirtualPatientURL, strSpecialPriceReason As String
        Dim dtTrialEndDate As Date


        objProvider.ConnectionString = _ConnectionString
        objUser.ConnectionString = _ConnectionString
        objUserClinic.ConnectionString = _ConnectionString
        objProviderClinic.ConnectionString = _ConnectionString
        objProviderTaxID.ConnectionString = _ConnectionString
        objProviderNumber.ConnectionString = _ConnectionString


        'Verify data before proceeding.
        If Not VerifyData(lngID, strLast, strFirst, strMI, "anything", _
                          varDegreeCredential, varRecognizedCredential, _
                          varClinics, varTaxIDs, lngServiceGroupID, lngGroupID, strErrMessage) Then
            Exit Sub
        End If

        'Update the user record - Providers are users.
        objUser.Update(lngID, strUserName, strLast, strFirst, strMI, strSSN, _
                     strEmail, varClinics, varRoles, strUpdatedBy, dtExpDate, blnVBYN)
        objUser = Nothing


        If IsArray(varSettings) Then
            strPatStmtYNE = varSettings(0)
            strApptReminder = varSettings(1)
            strAchSiteID = varSettings(2)
            strAchSiteKey = varSettings(3)
            strAchAPIKey = varSettings(4)
            strStampLine1 = varSettings(5)
            strStampLine2 = varSettings(6)
            strStampline3 = varSettings(7)
            strVirtualClientURL = varSettings(8)
            strVirtualPatientURL = varSettings(9)
            strSpecialPriceReason = varSettings(10)
            dtTrialEndDate = varSettings(11)
        End If

        'Update the provider record
        objProvider.Update(lngID, strAddress1, strAddress2, strCity, strState, md.NumbersOnly(strZip), strMailAddress1, strMailAddress2, _
                              strMailCity, strMailState, md.NumbersOnly(strMailZip), dtBirthDate, strNPI, strDEA, strTaxonomy, _
                              md.NumbersOnly(strPhone1), md.NumbersOnly(strPhone2), md.NumbersOnly(strFax), _
                              md.NumbersOnly(strBilling), varDegreeCredential, varRecognizedCredential, _
                              strStampLine1, strStampLine2, strStampline3, varSignature, lngLockBoxID, _
                              lngSupervisorID, lngServiceGroupID, lngGroupID, _
                              Trim(strCheckTo), dtStartDate, dblSubmitRate, dblCollectRate, dblCollectPercent, dtAutoCloseDate, strSpecialPriceReason, _
                              intFreeSubmits, intFreeRemain, dtTrialEndDate, strPatStmtYNE, strApptReminder, strAchSiteID, strAchSiteKey, strAchAPIKey, lngServiceID, _
                              strServicePeriod, dtServiceStartDate, _
                              dtServiceEndDate, dblServiceAmount, lngSalesRepID, lngMarketSourceID, lngSchInterval, dtSchStartTime, _
                              dtSchEndTime, strWebSite, strVirtualClientURL, strVirtualPatientURL, strNotes, strUpdatedBy)
        objProvider = Nothing

        ' Update User/Provider Clinic info
        objUserClinic.DeleteUserClinics(lngID) 'Disable all USER clinic records

        For lngCount = 0 To UBound(varClinics, 2)
            ' Insert every instance of clinic into tblUserClinic
            ' COMMENTED OUT BECAUSE THIS IS A SECOND   TO uspInsUserClinic
            ' THE FIRST   IS IN THE UPDATE USER  
            '  objUserClinic.Insert(lngID, varClinics(FLD_CLINICID, lngCount))

            ' PROVIDER clinic update/insert
            lngProviderClinic = ClinicExists(lngID, varClinics(FLD_CLINICID, lngCount))
            If lngProviderClinic > 0 Then
                ' we're updating an existing record
                UpdateClinic(lngProviderClinic, _
                     varClinics(FLD_ALLOWPATSCHED, lngCount), _
                     varClinics(FLD_ACCEPTNEWPAT, lngCount), _
                     varClinics(FLD_POINTOFCONTACT, lngCount))
            Else
                ' We inserting a new entry
                If OkToAssociate(lngID, strLast, strFirst, strMI, varClinics(FLD_CLINICID, lngCount)) Then
                    InsertClinic(lngID, varClinics(FLD_CLINICID, lngCount), _
                          strUpdatedBy, varClinics(FLD_ALLOWPATSCHED, lngCount), _
                          varClinics(FLD_ACCEPTNEWPAT, lngCount), _
                          varClinics(FLD_POINTOFCONTACT, lngCount))
                End If
            End If
        Next
        objUserClinic = Nothing

        'R001 - Disable Provider-Clinic records that were not included in the varClinics array.
        'These records were de-selected from the list.
        objProviderClinic.DisableProviderClinics(varClinics, lngID)
        objProviderClinic = Nothing

        'Begin R002 - Delete all Provider TaxID Records and then insert a record for each row in the updated array
        If IsArray(varTaxIDs) Then

            ' Delete all Tax IDs not in our list
            For lngCount = 0 To UBound(varTaxIDs, 1)
                Select Case varTaxIDs(lngCount, FLD_TAXID_DIRTY)
                    Case "", Nothing, "U"
                        If IsArray(arrTaxIDs) Then
                            ReDim Preserve arrTaxIDs(UBound(arrTaxIDs) + 1)
                        Else
                            ReDim arrTaxIDs(0)
                        End If
                        arrTaxIDs(UBound(arrTaxIDs)) = varTaxIDs(lngCount, FLD_TAXID_ID)
                    Case Else
                End Select
            Next

            'r007
            objProviderTaxID.Delete(lngID, arrTaxIDs)

            ' Insert/Update and ignore Tax IDs here
            For lngCount = 0 To UBound(varTaxIDs, 1)
                Select Case varTaxIDs(lngCount, 3)
                    Case "I"
                        objProviderTaxID.Insert(lngID, varTaxIDs(lngCount, FLD_TAXID), _
                      varTaxIDs(lngCount, FLD_TAXID_TYPE), varTaxIDs(lngCount, FLD_TAXID_NOTES), varTaxIDs(lngCount, FLD_TAXID_GROUP))
                    Case "U"
                        objProviderTaxID.Update(CLng(varTaxIDs(lngCount, FLD_TAXID_ID)), lngID, _
                          varTaxIDs(lngCount, FLD_TAXID), varTaxIDs(lngCount, FLD_TAXID_TYPE), _
                          varTaxIDs(lngCount, FLD_TAXID_NOTES), varTaxIDs(lngCount, FLD_TAXID_GROUP))
                    Case Else
                End Select
            Next

            objProviderTaxID = Nothing
        Else
            If varTaxIDs.IsNullorEmpty Then
                'Remove all Tax ID's i.e. the list is empty
                objProviderTaxID.DeleteAll(lngID)
                objProviderTaxID = Nothing
            End If
        End If

        objProviderNumber.Delete(lngID)
        If IsArray(varPracticeNums) Then
            For lngCount = 0 To UBound(varPracticeNums, 1)
                If varPracticeNums(lngCount, FLD_PRACTICE_INSCO) = "" Then
                    lngInsCompanyID = -1
                Else
                    lngInsCompanyID = CLng(varPracticeNums(lngCount, FLD_PRACTICE_INSCO))
                End If

                objProviderNumber.Insert(lngID, varPracticeNums(lngCount, FLD_PRACTICE_STATE), _
              lngInsCompanyID, varPracticeNums(lngCount, FLD_PRACTICE_NUM), varPracticeNums(lngCount, FLD_PRACTICE_SECNUM), varPracticeNums(lngCount, FLD_PRACTICE_SUBMITTERID), _
              IIf(varPracticeNums(lngCount, FLD_PRACTICE_GROUP) = "Yes", True, False), _
              IIf(varPracticeNums(lngCount, FLD_PRACTICE_ENROLLED_YN) = "Yes", True, False), _
              IIf(IsDate(varPracticeNums(lngCount, FLD_PRACTICE_ENROLLDATE)), varPracticeNums(lngCount, FLD_PRACTICE_ENROLLDATE), 0), _
              CLng(varPracticeNums(lngCount, FLD_PRACTICE_SUPERVISORID)), varPracticeNums(lngCount, FLD_PRACTICE_TAXID), CLng(varPracticeNums(lngCount, FLD_PRACTICE_TAXIDTYPE)), _
              varPracticeNums(lngCount, FLD_PRACTICE_EINDESC), IIf(varPracticeNums(lngCount, FLD_PRACTICE_INNETWORKYN) = "Yes", True, False), _
              IIf(varPracticeNums(lngCount, FLD_PRACTICE_ACKASSIGNYN) = "Yes", True, False), IIf(varPracticeNums(lngCount, FLD_PRACTICE_INDIVIDUALNPIONLYYN) = "Yes", True, False), _
              varPracticeNums(lngCount, FLD_PRACTICE_NOTES))
            Next
        End If
        objProviderNumber = Nothing
        'End R002

        objProvider = Nothing
        objUser = Nothing
        objProviderClinic = Nothing
        objProviderTaxID = Nothing
        objProviderNumber = Nothing


    End Sub

    Public Sub UpdateProviderFlags(ByVal lngID As Long, ByVal blnProvDirectYN As Boolean, _
                ByVal blnBulkCheckYN As Boolean, ByVal blnCheckToYN As Boolean, _
                ByVal blnSubmissionOnlyYN As Boolean, ByVal blnPayOnlineYN As Boolean, _
                ByVal blnPatSchedYN As Boolean, ByVal blnPatIntakeYN As Boolean, ByVal blnPatCreditCardYN As Boolean, _
                ByVal blnSpecialHandlingYN As Boolean, ByVal blnPrintDiagYN As Boolean, _
                ByVal blnUseNotesYN As Boolean, ByVal blneScribeYN As Boolean, ByVal blnMipsYN As Boolean, _
                ByVal blnSpecialPriceYN As Boolean, ByVal blnFreeTrialYN As Boolean, _
                ByVal blnCCardYN As Boolean, ByVal blnCCRequiredYN As Boolean, ByVal blnICheckYN As Boolean, _
                ByVal strPatNoteReqYNR As String, ByVal blnEncDetailYN As Boolean, _
                ByVal blnVirtualSessionYN As Boolean, ByVal blnPatOnlinePayYN As Boolean, ByVal strUpdatedBy As String)
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

        Dim objProvider As New ProviderDB.CProviderDB
        objProvider.ConnectionString = _ConnectionString


        objProvider.UpdateProviderFlags(lngID, blnProvDirectYN, blnBulkCheckYN, blnCheckToYN, _
              blnSubmissionOnlyYN, blnPayOnlineYN, blnPatSchedYN, blnPatIntakeYN, blnPatCreditCardYN, _
              blnSpecialHandlingYN, blnPrintDiagYN, blnUseNotesYN, blneScribeYN, blnMipsYN, _
              blnSpecialPriceYN, blnFreeTrialYN, blnCCardYN, blnCCRequiredYN, blnICheckYN, _
              strPatNoteReqYNR, blnEncDetailYN, blnVirtualSessionYN, blnPatOnlinePayYN, strUpdatedBy)

        objProvider = Nothing




    End Sub

    Public Function Exists(ByVal strLast As String, ByVal strFirst As String, _
                           ByVal strMI As String, _
                           ByVal lngDegreeCredential As Long, _
                           ByVal lngRecognizedCredential As Long, _
                           ByVal strSSN As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 02/03/2000
        'Author: Rick "Boom Boom" Segura
        'Description:  Determines if a provider record identical to the
        '              name parameters is already associated to the clinic
        '              whose ID matches lngClinic.
        'Parameters: strLast - Provider's Last Name
        '            strFirst - "" First Name
        '            strMI - "" Middle Initial
        '            lngDegreeCredential - provider's degree credential
        '            lngRecognizedCredential - provider's industry-recognized credential
        '            strSSN - provider's SSN
        'Returns: True if the association exists, false otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        Dim objProvider As New ProviderDB.CProviderDB
        Dim blnExists As Boolean
        objProvider.ConnectionString = _ConnectionString

        blnExists = objProvider.Exists(strLast, strFirst, strMI, _
                    lngDegreeCredential, lngRecognizedCredential, strSSN)

        Exists = blnExists


        objProvider = Nothing


    End Function

    Private Function VerifyData(ByVal lngID As Long, ByVal strLast As String, _
                        ByVal strFirst As String, ByVal strMI As String, _
                        ByVal strOrigPassword As String, _
                        ByVal varDegreeCredential As Object, _
                        ByVal varRecognizedCredential As Object, _
                        ByVal varClinics As Object, ByVal varTaxIDs As Object, _
                        ByVal lngServiceGroupID As Long, ByVal lngGroupID As Long, ByRef strErrMessage As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 02/03/2000
        'Author: Rick "Boom Boom" Segura
        'Description:  Verifies all required data has been provided by the user.
        'Parameters:  The values to be checked.
        'Returns: Boolean value identifying if all criteria has been satisfied.
        '--------------------------------------------------------------------

        If Trim(strLast) = "" Then
            strErrMessage = "Last name is required."
            VerifyData = False
            Exit Function
        End If

        If Trim(strFirst) = "" Then
            strErrMessage = "First name is required."
            VerifyData = False
            Exit Function
        End If

        If Trim(strOrigPassword) = "" Then
            strErrMessage = "Original Password is required."
            VerifyData = False
            Exit Function
        End If

        If (varDegreeCredential.IsNullorEmpty) Then
            strErrMessage = "Scholastic credential is required."
            VerifyData = False
            Exit Function
        End If

        If (varRecognizedCredential.IsNullorEmpty) Then
            strErrMessage = "Industry recognized credential is required."
            VerifyData = False
            Exit Function
        End If

        If lngServiceGroupID <= 0 Then
            strErrMessage = "Service Group ID is required."
            VerifyData = False
            Exit Function
        End If

        If varClinics.IsNullorEmpty Then
            strErrMessage = "Provider must be associated with at least one clinic."
            VerifyData = False
            Exit Function
        ElseIf varClinics.IsNullorEmpty Then
            strErrMessage = "Provider must be associated with at least one clinic."
            VerifyData = False
            Exit Function
        End If

        If varTaxIDs.IsNullorEmpty And lngGroupID < 0 Then
            strErrMessage = "Provider must have at least one Tax ID."
            VerifyData = False
            Exit Function
        ElseIf varTaxIDs.IsNullorEmpty And lngGroupID < 0 Then
            strErrMessage = "Provider must have at least one Tax ID."
            VerifyData = False
            Exit Function
        End If

        'If we get here, all is well...
        VerifyData = True

    End Function

    Public Function Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Long, _
                       ByVal strDeletedBy As String) As Integer
        '--------------------------------------------------------------------
        'Date: 02/06/2000
        'Author: Rick "Boom Boom" Segura
        'Description:  Flags a row in the tbProvider table marking the row as
        '              deleted or undeleted.
        'Parameters: blnDeleted - Boolean value identifying if the record is to
        '               be deleted (True) or undeleted (False).
        '            lngID - ID of the row in the table whose value will be
        '               updated.
        '            strDeletedBy - Login name of the user performing the action.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objProvider As New ProviderDB.CProviderDB
        Dim objUser As New ClinicBZ.CEditUserBz

        objProvider.ConnectionString = _ConnectionString
        objUser.ConnectionString = _ConnectionString



        Deleted = 0 ' Assume no errors

        objProvider.Deleted(blnDeleted, lngID, strDeletedBy)
        objUser.Deleted(blnDeleted, lngID, strDeletedBy)

        ' Signal successful completion


        objProvider = Nothing
        objUser = Nothing



    End Function

    Public Function FetchClinics(ByVal lngProviderID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 02/03/2000
        'Author: Rick "Boom Boom" Segura
        'Description:  Retrieves a recordset associated clinics and related
        '              info for the given provider ID
        'Parameters:  lngID - ID of provider to search info for
        'Returns: Recordset of requested record(s)
        '--------------------------------------------------------------------
        'Revision History:
        '   02/24/2000 - Renamed, modified, and moved to CProviderBZ
        '--------------------------------------------------------------------

        Dim objProvider As New ProviderDB.CProviderDB
        Dim rstSQL As New ADODB.Recordset

        objProvider.ConnectionString = _ConnectionString

        rstSQL = objProvider.FetchClinics(lngProviderID)

        objProvider = Nothing

        FetchClinics = rstSQL




    End Function
    Public Function FetchProvidersByClinic(ByVal lngClinicID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/10/2010
        'Author: Rick Duane C Orth
        'Description:  Retrieves a recordset associated clinics and related
        '              info for the given provider ID
        'Parameters:  lngID - ID of provider to search info for
        'Returns: Recordset of requested record(s)
        '--------------------------------------------------------------------
        'Revision History:
        '   02/24/2000 - Renamed, modified, and moved to CProviderBZ
        '--------------------------------------------------------------------

        Dim objProviderClinic As New ProviderDB.CProviderClinicDB
        Dim rstSQL As New ADODB.Recordset

        objProviderClinic.ConnectionString = _ConnectionString

        rstSQL = objProviderClinic.FetchProvidersByClinic(lngClinicID)

        objProviderClinic = Nothing

        FetchProvidersByClinic = rstSQL



    End Function

    Public Function InsertClinic(ByVal lngProviderID As Long, ByVal lngClinicID As Long, _
                           ByVal strAddedBy As String, ByVal strAllowPatSched As String, _
                           ByVal strAcceptNewPat As String, ByVal strPOC As String) As Long
        '--------------------------------------------------------------------
        'Date: 02/03/2000
        'Author: Rick "Boom Boom" Segura
        'Description:  Inserts a row into the tblProvider table utilizing
        '              a stored procedure.
        'Parameters: lngProviderID - internal provider ID
        '            lngClinicID - internal clinic ID
        '            strAddedBy - login of person making this association
        '            strAllowPatSched -  Allow patients to scedule via web flag
        '            strAcceptNewPat - accepting new patients flag
        '            strPOC - Point of contact
        'Returns: Nothing
        '--------------------------------------------------------------------
        'Revision History:
        '  02/24/2000 - Renamed and moved to CProviderBZ
        '--------------------------------------------------------------------

        Dim objProviderClinic As New ProviderDB.CProviderClinicDB
        Dim objUserClinic As New ClinicDB.CUserClinicDB
        Dim lngID As Long
        Dim strErrMessage As String



        objProviderClinic.ConnectionString = _ConnectionString
        objUserClinic.ConnectionString = _ConnectionString



        lngID = objProviderClinic.Insert(lngProviderID, lngClinicID, _
                                         strAddedBy, strAllowPatSched, _
                                         strAcceptNewPat, strPOC)
        objProviderClinic = Nothing


        objUserClinic.Insert(lngProviderID, lngClinicID)
        objUserClinic = Nothing



        InsertClinic = lngID


    End Function

    Public Sub UpdateClinic(ByVal lngID As Long, _
                      ByVal strAllowPatSched As String, _
                      ByVal strAcceptNewPat As String, ByVal strPOC As String)
        '--------------------------------------------------------------------
        'Date: 02/14/2000
        'Author: Rick "Boom Boom" Segura
        'Description:  Updates a row into the tblProviderClinic table.
        'Parameters: lngProviderID - internal provider ID
        '            lngClinicID - internal clinic ID
        '            strAllowPatSched -  "Allow patients to schedule via web" flag
        '            strAcceptNewPat - "accepting new patients" flag
        '            strPOC - Point of contact
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '   02/24/2000 - Renamed and moved to CProviderBZ
        '--------------------------------------------------------------------

        Dim objProviderClinic As New ProviderDB.CProviderClinicDB
        Dim strErrMessage As String

        objProviderClinic.ConnectionString = _ConnectionString



        objProviderClinic.Update(lngID, strAllowPatSched, _
                                    strAcceptNewPat, strPOC)

        objProviderClinic = Nothing




    End Sub

    Public Function ClinicExists(ByVal lngProviderID As Long, _
                                 ByVal lngClinicID As Long) As Long
        '--------------------------------------------------------------------
        'Date: 02/14/2000
        'Author: Rick "Boom Boom" Segura
        'Description:  Determines provider-clinic association already exists
        '              in tblProviderClinic
        'Parameters: lngProviderID - internal provider ID being sought
        '            lngClinicID - internal clinic ID being sought
        'Returns: True if the association exists, false otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '  02/24/2000 - Renamed and moved to CProviderBZ
        '--------------------------------------------------------------------

        Dim objProviderClinic As New ProviderDB.CProviderClinicDB
        Dim blnExists As Boolean
        Dim lngProviderClinic As Long

        objProviderClinic.ConnectionString = _ConnectionString

        lngProviderClinic = objProviderClinic.Exists(lngProviderID, lngClinicID)

        If lngProviderClinic > 0 Then
            ClinicExists = lngProviderClinic
        Else
            ClinicExists = -1
        End If



        objProviderClinic = Nothing


    End Function

    Public Sub ClinicDeleted(ByVal blnDeleted As Boolean, ByVal lngID As Long)
        'Date: 02/23/2000
        'Author: Rick "Boom Boom" Segura
        'Description:  Flags a row in the tbProvider table marking the row as
        '              deleted or undeleted.
        'Parameters: blnDeleted - Boolean value identifying if the record is to
        '               be deleted (True) or undeleted (False).
        '            lngID - ID of the row in the table whose value will be
        '               updated.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '  02/24/2000 - Renamed and moved to CProviderBZ
        '--------------------------------------------------------------------

        Dim objProviderClinic As New ProviderDB.CProviderClinicDB
        objProviderClinic.ConnectionString = _ConnectionString

        objProviderClinic = Nothing



    End Sub

    Public Function FetchCPTCodes(ByVal lngProviderID As Long, _
                          Optional ByVal blnIncludeDisabled As Boolean = False, _
                          Optional ByVal strUserWhere As String = "", _
                          Optional ByVal strOrderBy As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/05/2004
        'Author: Duane C Orth
        'Description:  Retrieves a recordset associated CPTCodes and related
        '              info for the given provider ID
        'Parameters:  lngID - ID of provider to search info for
        'Returns: Recordset of requested record(s)
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objProvider As New ProviderDB.CProviderDB
        Dim rstSQL As New ADODB.Recordset

        objProvider.ConnectionString = _ConnectionString

        rstSQL = objProvider.FetchCPTCodes(lngProviderID, blnIncludeDisabled, strUserWhere, strOrderBy)

        objProvider = Nothing

        FetchCPTCodes = rstSQL

    End Function

    Public Function FetchTaxIDs(ByVal lngID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 04/12/2000
        'Author: Dave Richkun
        'Description:  Retrieves a recordset of associated TaxIDs for the
        '              given provider ID
        'Parameters:  lngID - ID of provider to search info for
        'Returns: Recordset of requested record(s)
        '--------------------------------------------------------------------
        'Revision History:
        '  R002 - Created
        '--------------------------------------------------------------------

        Dim objProviderTaxID As New ProviderDB.CProviderTaxIDDB
        Dim rstSQL As New ADODB.Recordset

        objProviderTaxID.ConnectionString = _ConnectionString




        rstSQL = objProviderTaxID.Fetch(lngID)

        objProviderTaxID = Nothing

        FetchTaxIDs = rstSQL



        rstSQL = Nothing



    End Function

    Public Function FetchPrimaryTaxID(ByVal lngProviderID As Long, ByVal lngPatientID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/06/2001
        'Author: Chris Dereadt
        'Description:  Retrieves a recordset of Primary TaxIDs for the
        '              given provider ID
        'Parameters:  lngProviderID - ID of provider to search info for
        '              lngPatientID - ID of patient association
        'Returns: Recordset of requested record(s)
        '--------------------------------------------------------------------
        'Revision History:
        '  R002 - Created
        '--------------------------------------------------------------------

        Dim objProviderPrimaryTaxID As New ProviderDB.CProviderTaxIDDB
        Dim rstSQL As New ADODB.Recordset



        objProviderPrimaryTaxID.ConnectionString = _ConnectionString


        rstSQL = objProviderPrimaryTaxID.FetchPrimaryTaxID(lngProviderID, lngPatientID)

        objProviderPrimaryTaxID = Nothing

        FetchPrimaryTaxID = rstSQL

        rstSQL = Nothing



    End Function
    Public Function FetchPracticeNumbers(ByVal lngID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 04/12/2000                                                   '
        'Author: Dave Richkun                                               '
        'Description:  Retrieves a recordset of associated Practice Numbers '
        '               for the given provider ID                           '
        'Parameters:  lngID - ID of provider to search info for             '
        'Returns: Recordset of requested record(s)                          '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '  R002 - Created                                                   '
        '--------------------------------------------------------------------

        Dim objProviderNum As New ProviderDB.CProviderNumDB
        Dim rstSQL As New ADODB.Recordset
        objProviderNum.ConnectionString = _ConnectionString


        rstSQL = objProviderNum.Fetch(lngID)
        objProviderNum = Nothing

        FetchPracticeNumbers = rstSQL


    End Function
    Public Function FetchPracticeNumbersByInsID(ByVal lngID As Long, ByVal lngInsID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 04/12/2000                                                   '
        'Author: Dave Richkun                                               '
        'Description:  Retrieves a recordset of associated Practice Numbers '
        '               for the given provider ID                           '
        'Parameters:  lngID - ID of provider to search info for             '
        'Returns: Recordset of requested record(s)                          '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '  R002 - Created                                                   '
        '--------------------------------------------------------------------

        Dim objProviderNum As New ProviderDB.CProviderNumDB
        Dim rstSQL As New ADODB.Recordset

        objProviderNum.ConnectionString = _ConnectionString

        rstSQL = objProviderNum.FetchByInsID(lngID, lngInsID)
        objProviderNum = Nothing

        FetchPracticeNumbersByInsID = rstSQL


    End Function


    Public Function FetchPlanTaxID(ByVal lngProviderID As Long, ByVal lngPlanID As Long) As Long
        '--------------------------------------------------------------------
        'Date: 11/05/2003                                                   '
        'Author: Duane C Orth
        'Description:  Fetches taxID for the given provider-plan association
        'Parameters:  lngProviderID - ID of provider to search info for
        '                       Long - ID of plan  to search info for
        '
        'Returns: Provider Plan Tax ID as Long
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '--------------------------------------------------------------------

        Dim objProv As New ProviderDB.CProviderPlanTaxIDDB
        Dim lngProvPlanTaxID As Long

        objProv.ConnectionString = _ConnectionString

        'find the id in tblproviderplantaxid for the given prov-plan pair
        FetchPlanTaxID = objProv.Fetch(lngProviderID, lngPlanID)



        objProv = Nothing

    End Function
    Private Function OkToAssociate(ByVal lngID As Long, _
                                   ByVal strLast As String, _
                                   ByVal strFirst As String, _
                                   ByVal strMI As String, _
                                   ByVal lngClinicID As Long) As Boolean
        '--------------------------------------------------------------------
        'Date: 04/28/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Determines if a provider-clinic association is valid '
        '                  to create                                        '
        'Parameters: strLast - Provider's last name                         '
        '            strFirst - Provider's First name                       '
        '            strMI - Provider's MI                                  '
        '            lngClinicID - ID of clinic to associate with provider  '
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim objProv As New ProviderDB.CProviderDB

        objProv.ConnectionString = _ConnectionString

        OkToAssociate = objProv.OkToAssociate(lngID, strLast, strFirst, strMI, lngClinicID)



        objProv = Nothing

    End Function

    Public Sub AssignPlanTaxDefault(ByVal lngProviderID As Long, ByVal lngPlanID As Long, ByVal lngTaxID As Long)
        '--------------------------------------------------------------------
        'Date: 06/20/2000                                                   '
        'Author: Eric Pena
        'Description:  Assigns the default taxID for the given provider-plan association
        'Parameters:  lngProviderID - ID of provider to search info for
        '                       Long - ID of plan  to search info for
        '                       strTaxID - tax ID to use
        'Returns: null
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '--------------------------------------------------------------------

        Dim objProv As New ProviderDB.CProviderPlanTaxIDDB
        Dim objProvTax As New ProviderDB.CProviderTaxIDDB
        Dim lngProvPlanTaxID As Long
        Dim lngProviderTaxID As Long
        Dim rstSQL As New ADODB.Recordset



        'find the id in tblproviderplantaxid for the given prov-plan pair
        lngProvPlanTaxID = objProv.Fetch(lngProviderID, lngPlanID)
        'find the key for the current taxid string
        rstSQL = objProvTax.Fetch(lngProviderID)
        objProvTax = Nothing
        Do While Not rstSQL.EOF
            If rstSQL.Fields("fldTaxID").Value = lngTaxID Then Exit Do
            rstSQL.MoveNext()
        Loop
        'invalid data.
        If rstSQL.EOF Then
            Exit Sub
        End If


        If lngProvPlanTaxID > -1 Then
            'a row exists.
            'assign the new id as default
            lngProviderTaxID = rstSQL.Fields("fldProviderTaxID").Value
            objProv.Update(lngProvPlanTaxID, lngProviderTaxID)
        Else
            'nonexistant pair.  create
            objProv.Insert(lngPlanID, rstSQL.Fields("fldProviderTaxID").Value)
        End If


        rstSQL = Nothing
        objProv = Nothing

        rstSQL = Nothing
        objProv = Nothing
        objProvTax = Nothing

    End Sub

    Public Sub UpdateProfile(ByVal lngID As Long, ByVal strAddress1 As String, _
                      ByVal strAddress2 As String, ByVal strCity As String, _
                      ByVal strState As String, ByVal strZip As String, _
                      ByVal strPhone1 As String, ByVal strPhone2 As String, _
                      ByVal strFax As String, ByVal strBilling As String, _
                      ByVal strEmail As String, ByVal strUpdatedBy As String)
        '--------------------------------------------------------------------
        'Date: 05/07/2001                                                   '
        'Author: Dave Richkun
        'Description:  Updates a single Provider's demographic information in tblProvider.
        'Parameters: lngID - ID of the Provider whose demographics will be updated
        '            Others - values of demographics that will be updated
        '            strUpdatedBy - Name of user making the change
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '  R005 - Created
        '--------------------------------------------------------------------

        Dim objProvider As New ProviderDB.CProviderDB
        objProvider.ConnectionString = _ConnectionString

        objProvider.UpdateProfile(lngID, strAddress1, strAddress2, _
                        strCity, strState, md.NumbersOnly(strZip), _
                        md.NumbersOnly(strPhone1), md.NumbersOnly(strPhone2), _
                        md.NumbersOnly(strFax), md.NumbersOnly(strBilling), _
                        strEmail, strUpdatedBy)
        objProvider = Nothing





    End Sub

    Public Sub UpdateGoogleRefresh(ByVal lngID As Long, ByVal strGoogleAPIRefresh As String)

        '--------------------------------------------------------------------
        'Date: 03/17/2022                                                   '
        'Author: Jacob Orth
        'Description:  Updates a single Provider's demographic information in tblProvider.
        'Parameters: lngID - ID of the Provider whose demographics will be updated
        '            strGoogleAPIRefresh - Used to sync with Google Calendar
        '
        'Returns: Null                                                      '
        '--------------------------------------------------------------------

        Dim objProvider As New ProviderDB.CProviderDB
        objProvider.ConnectionString = _ConnectionString

        objProvider.UpdateGoogleRefresh(lngID, strGoogleAPIRefresh)

        objProvider = Nothing



    End Sub

    Public Sub InsertIntakeProfile(ByVal lngProviderID As Long, ByVal strSunYN As String, _
            ByVal intSunStartTime As Integer, ByVal intSunEndTime As Integer, ByVal strMonYN As String, _
            ByVal intMonStartTime As Integer, ByVal intMonEndTime As Integer, ByVal strTueYN As String, _
            ByVal intTueStartTime As Integer, ByVal intTueEndTime As Integer, ByVal strWedYN As String, _
            ByVal intWedStartTime As Integer, ByVal intWedEndTime As Integer, ByVal strThuYN As String, _
            ByVal intThuStartTime As Integer, ByVal intThuEndTime As Integer, ByVal strFriYN As String, _
            ByVal intFriStartTime As Integer, ByVal intFriEndTime As Integer, ByVal strSatYN As String, _
            ByVal intSatStartTime As Integer, ByVal intSatEndTime As Integer, ByVal strGender As String, _
            ByVal lngEthnicity As Long, ByVal varSpecialties As Object, ByVal strAddedBy As String)
        '--------------------------------------------------------------------
        'Date: 09/25/2001                                                   '
        'Author: Dave Richkun
        'Description:  Inserts information to create a profile for Provider's
        '              opting in to use Intake services.
        'Parameters: lngProviderID - ID of the Provider whose profile will be created
        '            strSunYN - identifies if Provider accepts new patients on Sunday
        '            intSunStartTime - identifies starting hour (based on 24 hour clock) that
        '               Provider has designated for new patient appointments
        '            intSunEndTime - identifies ending hour (based on 24 hour clock) that
        '               Provider has designated for new patient appointments
        '            ....identical for Mon-Sat parameters....
        '            strGender - Provider's gender
        '            strEthnicity - Provider's ethnicity
        '            varSpecialties - Array of Specialty IDs associated with the Provider
        '            strUpdatedBy - Name of user inserting the record
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '  R009 - Created
        '--------------------------------------------------------------------

        Dim objProvider As New ProviderDB.CProviderDB
        Dim objSpec As ProviderDB.CProviderSpecialtyDB
        Dim intCount As New Integer
        Dim strErrMessage As String



        objProvider.ConnectionString = _ConnectionString
        objSpec.ConnectionString = _ConnectionString



        objProvider.InsertIntakeProfile(lngProviderID, strSunYN, intSunStartTime, _
              intSunEndTime, strMonYN, intMonStartTime, intMonEndTime, strTueYN, intTueStartTime, _
              intTueEndTime, strWedYN, intWedStartTime, intWedEndTime, strThuYN, intThuStartTime, _
              intThuEndTime, strFriYN, intFriStartTime, intFriEndTime, strSatYN, intSatStartTime, _
              intSatEndTime, strGender, lngEthnicity, strAddedBy)
        objProvider = Nothing

        'Insert row(s) into tblProviderSpecialty
        For intCount = 0 To UBound(varSpecialties)
            objSpec.Insert(lngProviderID, CLng(varSpecialties(intCount)))
            objSpec = Nothing
        Next


        objProvider = Nothing
        objSpec = Nothing


    End Sub


    Public Sub UpdateIntakeProfile(ByVal lngProviderID As Long, ByVal strSunYN As String, _
            ByVal intSunStartTime As Integer, ByVal intSunEndTime As Integer, ByVal strMonYN As String, _
            ByVal intMonStartTime As Integer, ByVal intMonEndTime As Integer, ByVal strTueYN As String, _
            ByVal intTueStartTime As Integer, ByVal intTueEndTime As Integer, ByVal strWedYN As String, _
            ByVal intWedStartTime As Integer, ByVal intWedEndTime As Integer, ByVal strThuYN As String, _
            ByVal intThuStartTime As Integer, ByVal intThuEndTime As Integer, ByVal strFriYN As String, _
            ByVal intFriStartTime As Integer, ByVal intFriEndTime As Integer, ByVal strSatYN As String, _
            ByVal intSatStartTime As Integer, ByVal intSatEndTime As Integer, ByVal strGender As String, _
            ByVal lngEthnicity As Long, ByVal varSpecialties As Object, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 09/25/2001
        'Author: Dave Richkun
        'Description:  Updates a Provider's Intake Profile
        'Parameters: lngProviderID - ID of the Provider whose profile will be updated
        '            strSunYN - identifies if Provider accepts new patients on Sunday
        '            intSunStartTime - identifies starting hour (based on 24 hour clock) that
        '               Provider has designated for new patient appointments
        '            intSunEndTime - identifies ending hour (based on 24 hour clock) that
        '               Provider has designated for new patient appointments
        '            ....identical for Mon-Sat parameters....
        '            strGender - Provider's gender
        '            strEthnicity - Provider's ethnicity
        '            strUserName - Name of user making the change
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '  R009 - Created
        '--------------------------------------------------------------------

        Dim objProvider As New ProviderDB.CProviderDB
        Dim objSpec As New ProviderDB.CProviderSpecialtyDB
        Dim rst As New ADODB.Recordset
        Dim intCtr1 As Integer
        Dim intCtr2 As Integer
        Dim blnFound As Boolean
        Dim strErrMessage As String



        objProvider.ConnectionString = _ConnectionString
        objSpec.ConnectionString = _ConnectionString




        objProvider.UpdateIntakeProfile(lngProviderID, strSunYN, intSunStartTime, _
              intSunEndTime, strMonYN, intMonStartTime, intMonEndTime, strTueYN, intTueStartTime, _
              intTueEndTime, strWedYN, intWedStartTime, intWedEndTime, strThuYN, intThuStartTime, _
              intThuEndTime, strFriYN, intFriStartTime, intFriEndTime, strSatYN, intSatStartTime, _
              intSatEndTime, strGender, lngEthnicity, strUserName)

        'Retrieve current Provider Specialties, and disable if no longer included, or insert if new
        'specialties have been added.
        rst = objProvider.FetchSpecialties(lngProviderID)
        objProvider = Nothing

        For intCtr1 = 1 To rst.RecordCount
            blnFound = False
            For intCtr2 = 0 To UBound(varSpecialties)
                If rst.Fields("fldSpecialtyID").Value = varSpecialties(intCtr2) Then
                    'Match is found
                    blnFound = True
                    Exit For

                End If
            Next intCtr2

            If blnFound = False Then
                'Delete The Specialty
                objSpec.Deleted(True, lngProviderID, rst.Fields("fldSpecialtyID").Value, strUserName)
                objSpec = Nothing
            End If

            rst.MoveNext()
        Next intCtr1

        'Look for new Specialties and insert them.
        rst.MoveFirst()
        For intCtr1 = 0 To UBound(varSpecialties)
            blnFound = False
            For intCtr2 = 1 To rst.RecordCount
                If Not rst.EOF Then
                    If varSpecialties(intCtr1) = rst.Fields("fldSpecialtyID").Value Then
                        'Match is found
                        blnFound = True
                        Exit For
                    End If
                    rst.MoveNext()
                Else
                    blnFound = False
                    Exit For
                End If
            Next intCtr2

            If blnFound = False Then
                'Delete The Specialty
                objSpec.Insert(lngProviderID, CLng(varSpecialties(intCtr1)))
                objSpec = Nothing
            End If

            rst.MoveFirst()
        Next intCtr1



    End Sub


    Public Sub IntakeOptOut(ByVal lngProviderID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 09/25/2001                                                   '
        'Author: Dave Richkun
        'Description:  Updates a Provider's profile, setting the flag that allows them to
        '               unsubscribe to Intake Services.
        'Parameters: lngProviderID - ID of the Provider whose profile will be updated
        '            strUserName - Name of user updating the record
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '  R009 - Created
        '--------------------------------------------------------------------

        Dim objProvider As New ProviderDB.CProviderDB

        objProvider.ConnectionString = _ConnectionString



        objProvider.IntakeOptOut(lngProviderID, strUserName)
        objProvider = Nothing



    End Sub

    Public Function FetchIntakeTimes(ByVal lngProviderID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/27/2001
        'Author: Dave Richkun
        'Description:  Retrieves a recordset containing Providers established Intake times
        'Parameters: lngProviderID - ID of Provider whose time record will be retrieved.
        'Returns: Recordset of Provider Intake times
        '--------------------------------------------------------------------
        '  R009: Created
        '--------------------------------------------------------------------
        Dim objProvider As New ProviderDB.CProviderDB
        Dim rstSQL As New ADODB.Recordset
        objProvider.ConnectionString = _ConnectionString


        rstSQL = objProvider.FetchIntakeTimes(lngProviderID)
        FetchIntakeTimes = rstSQL

        objProvider = Nothing



    End Function
    Public Function FetchProviderPatientPortal(ByVal lngProviderID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 05/25/2020
        'Author: Duane C Orth
        'Description:  Retrieves a recordset containing Providers established Intake times
        'Parameters: lngProviderID - ID of Provider whose time record will be retrieved.
        'Returns: Recordset of Provider Intake times
        '--------------------------------------------------------------------
        '  R009: Created
        '--------------------------------------------------------------------
        Dim objProvider As New ProviderDB.CProviderDB
        Dim rstSQL As New ADODB.Recordset

        objProvider.ConnectionString = _ConnectionString


        rstSQL = objProvider.FetchProviderPatientPortal(lngProviderID)
        FetchProviderPatientPortal = rstSQL

        objProvider = Nothing




    End Function


    Public Function FetchSpecialties(ByVal lngProviderID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/27/2001
        'Author: Dave Richkun
        'Description:  Retrieves a recordset containing Provider specialties
        'Parameters: lngProviderID - ID of Provider whose specialties will be retrieved.
        'Returns: Recordset of Provider Specialties
        '--------------------------------------------------------------------
        '  R009: Created
        '--------------------------------------------------------------------

        Dim objProvider As New ProviderDB.CProviderDB

        Dim rstSQL As New ADODB.Recordset
        objProvider.ConnectionString = _ConnectionString


        rstSQL = objProvider.FetchSpecialties(lngProviderID)
        FetchSpecialties = rstSQL

        objProvider = Nothing



    End Function

    Public Function FetchIntakeProfileOpts(ByVal lngProviderID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/27/2001
        'Author: Dave Richkun
        'Description:  Retrieves a recordset containing optional (non-required) Provider Intake data
        'Parameters: lngProviderID - ID of Provider whose optional Profile data will be retrieved.
        'Returns: Recordset of Provider Options
        '--------------------------------------------------------------------
        '  R009: Created
        '--------------------------------------------------------------------

        Dim objProvider As New ProviderDB.CProviderDB
        Dim rstSQL As New ADODB.Recordset

        objProvider.ConnectionString = _ConnectionString

        rstSQL = objProvider.FetchIntakeProfileOpts(lngProviderID)
        FetchIntakeProfileOpts = rstSQL

        objProvider = Nothing



    End Function


    Public Function IntakeProfileExists(ByVal lngProviderID As Long) As Boolean
        '--------------------------------------------------------------------
        'Date: 10/01/2001
        'Author: Dave Richkun
        'Description:  Returns a boolean identifying if Intake Profile for a Provider exists,
        '              regardless if it has been previously disabled or not.
        'Parameters: lngProviderID - ID of Provider whose Profile data will be searched for
        'Returns: True if Profile exists, otherwise False
        '--------------------------------------------------------------------
        '  R009: Created
        '--------------------------------------------------------------------

        Dim objProvider As New ProviderDB.CProviderDB

        objProvider.ConnectionString = _ConnectionString

        IntakeProfileExists = objProvider.IntakeProfileExists(lngProviderID)

        objProvider = Nothing


    End Function


    Public Sub DecrementDenialCounter(ByVal lngProviderID As Long)
        '--------------------------------------------------------------------
        'Date: 10/17/2002
        'Author: Dave Richkun
        'Description:  Decrements a providers denial counter by one
        'Parameters: lngProviderID - ID of Provider whose Denial counter will be decremented
        'Returns: Null
        '--------------------------------------------------------------------
        ' Revision History:
        '
        '--------------------------------------------------------------------

        Dim objProvider As New ProviderDB.CProviderDB
        objProvider.ConnectionString = _ConnectionString


        objProvider.DecrementDenialCounter(lngProviderID)
        objProvider = Nothing



    End Sub

    Public Function FetchIOUs(ByVal lngProviderID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 02/06/2003
        'Author: Dave Richkun
        'Description:  Retrieves a recordset of IOU charges for a specific provider
        'Parameters: lngProviderID - ID of Provider whose IOU records will be
        '              retrieved.
        'Returns: Recordset of IOUs
        '--------------------------------------------------------------------

        Dim objProvider As New ProviderDB.CProviderDB
        objProvider.ConnectionString = _ConnectionString
        FetchIOUs = objProvider.FetchIOUs(lngProviderID)
        objProvider = Nothing



    End Function

    Public Function FetchPayments(ByVal lngProviderID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 02/06/2003
        'Author: Dave Richkun
        'Description:  Retrieves a recordset of payments for a specific provider
        'Parameters: lngProviderID - ID of Provider whose payment records will be
        '              retrieved.
        'Returns: Recordset of payments
        '--------------------------------------------------------------------

        Dim objProvider As New ProviderDB.CProviderDB

        objProvider.ConnectionString = _ConnectionString
        FetchPayments = objProvider.FetchPayments(lngProviderID)
        objProvider = Nothing



    End Function

    Public Function FetchByID(ByVal lngProviderID As Long) As ADODB.Recordset

        Dim objProvider As New ProviderDB.CProviderDB
        Dim rstSQL As New ADODB.Recordset
        objProvider.ConnectionString = _ConnectionString

        rstSQL = objProvider.FetchByID(lngProviderID)

        objProvider = Nothing

        FetchByID = rstSQL



    End Function

End Class
