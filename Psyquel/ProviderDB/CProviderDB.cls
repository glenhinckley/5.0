
'--------------------------------------------------------------------
'Class Name: CProviderDB                                            '
'Date: 02/02/2000                                                   '
'Author: Rick "Boom Boom" Segura                                    '
'Denis J Finkel
'Description:  MTS object designed to host methods associated with  '
'              data affecting the tblProvider table.                '
'--------------------------------------------------------------------
' Revision History:
'   R001: 08/23/2000 Segura: Leak checks and improvements
'   R002: 05/07/2001 Richkun: Added UpdateProfile() method
'   R003: 09/10/2001 Richkun: Added support for tblProvider.fldServiceGroupID
'   R004: 09/13/2001 Richkun: Added support for tblProvider.fldStartDate
'   R005: 09/26/2001 Richkun: Added support for Provider Intake Profiles
'   R006: 02/06/2003 Richkun: Added methods to retrieve IOUs and Payments
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
Imports System.Runtime.CompilerServices

Public Class CProviderDB

    Private _ConnectionString As String = String.Empty
    Private _ConnectionStringADODB As String = String.Empty


    Private Const CLASS_NAME As String = "CProviderDB"
    Private Const TABLE_NAME As String = "tblProvider"
    Private Const LINKING_TABLE_NAME As String = "tblProviderClinic"





    Dim md As New ModCommon

    Dim ds As New db
    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property


    Public WriteOnly Property ConnectionStringADODB() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionStringADODB = value

        End Set
    End Property


    Public Function GetListOfProviderIDsByGroupID(ByVal ProviderGroupID As Integer) As List(Of Integer)


        Dim r As List(Of Integer)










    End Function










    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False,
                          Optional ByVal strUserWhere As String = "",
                          Optional ByVal strOrderBy As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 02/02/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Retrieves records from the tblProvider table.        '
        'Parameters:  blnIncludeDisabled - Optional parameter that          '
        '               identifies if records flagged as 'Disabled' or      '
        '               'De-activated' are to be included in the record set.'
        '               The default value is False.                         '
        '             strWhere - Optional 'Where' clause of the SQL statement
        '               by which records will be filtered.  If the parameter'
        '               is not supplied, all records will be retrieved.     '
        '             strOrderBy - Optional 'Order By' clause in which      '
        '               retrieved records will be sorted.  The default is set
        '               to the 'fldLast', 'fldFirst', 'fldMI' column        '
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim strSQL As String
        Dim strWhere As String
        Dim blnUserWhere As Boolean



        'Prepare the SQL statement.
        strSQL = "SELECT "
        strSQL = strSQL & "A.*, C.fldProviderType, B.fldFirstName, B.fldLastName, B.fldMI, "
        strSQL = strSQL & "B.fldSSN, B.fldEmail, B.fldOrigPassword, B.fldPassword, B.fldDisabledYN "
        strSQL = strSQL & " FROM "
        strSQL = strSQL & TABLE_NAME & " AS A INNER JOIN "
        strSQL = strSQL & "tblUser AS B ON A.fldProviderID = B.fldUserID INNER JOIN "
        strSQL = strSQL & "tblProviderType C ON A.fldRecognizedCredential = C.fldProviderTypeID "
        strSQL = strSQL & "WHERE "
        strSQL = strSQL & "A.fldProviderID > 0 "

        'Attach user's where clause if applicable
        blnUserWhere = Trim(strUserWhere) > ""

        ' Build where clause appropriately
        If blnIncludeDisabled Then  'Include disabled records
            If blnUserWhere Then    ' Only use user where string
                strWhere = " AND " & strUserWhere
            Else ' Empty where string
                strWhere = ""
            End If

        Else    'Filter out disabled records
            strWhere = " AND B.fldDisabledYN = 'N' "

            If blnUserWhere Then ' Concat user where string to filtering string
                strWhere = strWhere & " AND " & strUserWhere
            Else
                ' Do nothing
            End If

        End If
        strSQL = strSQL & strWhere

        If Trim(strOrderBy) > "" Then
            strSQL = strSQL & " ORDER BY "
            strSQL = strSQL & strOrderBy
        Else
            strSQL = strSQL & " ORDER BY "
            strSQL = strSQL & "fldLastName, "
            strSQL = strSQL & "fldFirstName, "
            strSQL = strSQL & "fldMI "
        End If

        'Instantiate the Recordset.
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = adUseClient

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Populate the recordset
        rstSQL.Open(strSQL, cnnSQL, adOpenForwardOnly, adLockReadOnly, adCmdText + adAsyncFetch)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rstSQL.ActiveConnection = Nothing
        Fetch = rstSQL
        cnnSQL.Close()
        cnnSQL = Nothing


        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        Fetch = rstSQL
        cnnSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function

    Public Function Insert(ByVal lngUserID As Long, ByVal strAddress1 As String, ByVal strAddress2 As String, _
                           ByVal strCity As String, ByVal strState As String, ByVal strZip As String, _
                           ByVal strMailAddress1 As String, ByVal strMailAddress2 As String, _
                           ByVal strMailCity As String, ByVal strMailState As String, ByVal strMailZip As String, _
                           ByVal dtBirthDate As Date, ByVal strNPI As String, ByVal strDEA As String, ByVal strTaxonomy As String, _
                           ByVal strPhone1 As String, ByVal strPhone2 As String, ByVal strFax As String, _
                           ByVal strBilling As String, ByVal varDegreeCredential As Object, _
                           ByVal varRecognizedCredential As Object, ByVal strStampLine1 As String, _
                           ByVal strStampLine2 As String, ByVal strStampline3 As String, _
                           ByVal varSignature As Object, ByVal lngLockBoxID As Long, ByVal lngSupervisorID As Long, _
                           ByVal lngServiceGroupID As Long, ByVal lngGroupID As Long, _
                           ByVal strCheckTo As String, ByVal dtStartDate As Date, ByVal dblSubmitRate As Double, _
                           ByVal dblCollectRate As Double, ByVal dblCollectPercent As Double, ByVal strSpecialPriceReason As String, _
                           ByVal intFreeSubmits As Integer, ByVal dtTrialEndDate As Date, ByVal strPatStmtYNE As String, _
                           ByVal strApptReminder As String, ByVal strAchSiteID As String, ByVal strAchSiteKey As String, ByVal strAchAPIKey As String, _
                           ByVal lngServiceID As Long, ByVal strServicePeriod As String, ByVal dtServiceStartDate As Date, _
                           ByVal dtServiceEndDate As Date, ByVal dblServiceAmount As Double, ByVal lngSalesRepID As Long, _
                           ByVal lngMarketSourceID As Long, ByVal lngSchInterval As Long, ByVal dtSchStartTime As Date, _
                           ByVal dtSchEndTime As Date, ByVal strWebSite As String, ByVal strVirtualClientURL As String, ByVal strVirtualPatientURL As String, _
                           ByVal strNotes As String, ByVal strAddedBy As String) As Long
        '--------------------------------------------------------------------
        'Date: 02/02/2000                                                   '
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
        '            strMailAddress1 - "" Address line #1                   '
        '            strMailAddress2 - "" Address line #2                   '
        '            strMailCity - "" City                                  '
        '            strMailState - "" State                                '
        '            strMailZip - "" Zip                                    '
        '            strPhone1 - "" Phone #1                                '
        '            strPhone2 - "" Phone #2                                '
        '            strFax - "" Fax #                                      '
        '            strPager - "" Pager #                                  '
        '            strEmail - "" E-mail Address                           '
        '            strSSN - "" Social Security #                          '
        '            varDegreeCredential - "" scholastic credential         '
        '            varRecognizedCredential - "" Industry recognized       '
        '                                       credential                  '
        '            strStampLine1 - "" stamp line 1                        '
        '            strStampLine2 - "" stamp line 2                        '
        '            strStampLine3 - "" stamp line 3                        '
        '            varSignature - "" signature file                       '
        '            strAddedBy - logon of user who added provider to system'
        '            strDisabledYN - Provider's deleted flag                '
        'Returns: ID (Primary Key) of the row inserted                      '
        '--------------------------------------------------------------------


        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim prmSQL As ADODB.Parameter



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspInsProvider"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
            .Parameters.Append(.CreateParameter("@Address1", adVarChar, adParamInput, 60, strAddress1))
            .Parameters.Append(.CreateParameter("@Address2", adVarChar, adParamInput, 60, strAddress2))
            .Parameters.Append(.CreateParameter("@City", adVarChar, adParamInput, 20, strCity))
            .Parameters.Append(.CreateParameter("@State", adVarChar, adParamInput, 2, strState))
            .Parameters.Append(.CreateParameter("@Zip", adVarChar, adParamInput, 11, md.NumbersOnly(strZip)))
            .Parameters.Append(.CreateParameter("@MailAddress1", adVarChar, adParamInput, 60, strMailAddress1))
            .Parameters.Append(.CreateParameter("@MailAddress2", adVarChar, adParamInput, 60, strMailAddress2))
            .Parameters.Append(.CreateParameter("@MailCity", adVarChar, adParamInput, 20, strMailCity))
            .Parameters.Append(.CreateParameter("@MailState", adVarChar, adParamInput, 2, strMailState))
            .Parameters.Append(.CreateParameter("@MailZip", adVarChar, adParamInput, 11, md.NumbersOnly(strMailZip)))
            .Parameters.Append(.CreateParameter("@BirthDate", adDBTimeStamp, adParamInput, , IIf(dtBirthDate.ToOADate() = 0, DBNull.Value, dtBirthDate)))
            .Parameters.Append(.CreateParameter("@NPI", adVarChar, adParamInput, 10, strNPI))
            .Parameters.Append(.CreateParameter("@DEA", adVarChar, adParamInput, 10, strDEA))
            .Parameters.Append(.CreateParameter("@Taxonomy", adVarChar, adParamInput, 10, strTaxonomy))
            .Parameters.Append(.CreateParameter("@Phone1", adVarChar, adParamInput, 14, md.NumbersOnly(strPhone1)))
            .Parameters.Append(.CreateParameter("@Phone2", adVarChar, adParamInput, 14, md.NumbersOnly(strPhone2)))
            .Parameters.Append(.CreateParameter("@Fax", adVarChar, adParamInput, 14, md.NumbersOnly(strFax)))
            .Parameters.Append(.CreateParameter("@Billing", adVarChar, adParamInput, 14, md.NumbersOnly(strBilling)))
            .Parameters.Append(.CreateParameter("@DegreeCredential", adInteger, adParamInput, , varDegreeCredential))
            .Parameters.Append(.CreateParameter("@RecognizedCredential", adInteger, adParamInput, , varRecognizedCredential))
            .Parameters.Append(.CreateParameter("@StampLine1", adVarChar, adParamInput, 50, strStampLine1))
            .Parameters.Append(.CreateParameter("@StampLine2", adVarChar, adParamInput, 50, strStampLine2))
            .Parameters.Append(.CreateParameter("@StampLine3", adVarChar, adParamInput, 50, strStampline3))

            If Len(varSignature) <> 0 Then  ' Determine if an image was passed
                .Parameters.Append(.CreateParameter("@Signature", adLongVarBinary, adParamInput, UBound(varSignature) + 1))
                .Parameters("@Signature").AppendChunk(varSignature)
            Else
                .Parameters.Append(.CreateParameter("@Signature", adLongVarBinary, adParamInput, -1, DBNull.Value))
            End If

            .Parameters.Append(.CreateParameter("@LockBoxID", adInteger, adParamInput, , lngLockBoxID))
            .Parameters.Append(.CreateParameter("@SupervisorID", adInteger, adParamInput, , IIf(lngSupervisorID < 0, DBNull.Value, lngSupervisorID)))
            .Parameters.Append(.CreateParameter("@ServiceGroupID", adInteger, adParamInput, , IIf(lngServiceGroupID <= 0, DBNull.Value, lngServiceGroupID)))
            .Parameters.Append(.CreateParameter("@GroupID", adInteger, adParamInput, , IIf(lngGroupID <= 0, DBNull.Value, lngGroupID)))
            .Parameters.Append(.CreateParameter("@CheckTo", adVarChar, adParamInput, 60, strCheckTo))
            .Parameters.Append(.CreateParameter("@StartDate", adDBTimeStamp, adParamInput, , IIf(dtStartDate.ToOADate() = 0, DBNull.Value, dtStartDate)))
            .Parameters.Append(.CreateParameter("@SubmitRate", adDouble, adParamInput, , dblSubmitRate))
            .Parameters.Append(.CreateParameter("@CollectRate", adDouble, adParamInput, , dblCollectRate))
            .Parameters.Append(.CreateParameter("@CollectPercent", adDouble, adParamInput, , dblCollectPercent))
            .Parameters.Append(.CreateParameter("@SpecialPriceReason", adVarChar, adParamInput, 50, strSpecialPriceReason))
            .Parameters.Append(.CreateParameter("@FreeSubmits", adInteger, adParamInput, , intFreeSubmits))
            .Parameters.Append(.CreateParameter("@FreeTrialEndDate", adDBTimeStamp, adParamInput, , IIf(dtTrialEndDate.ToOADate() = 0, DBNull.Value, dtTrialEndDate)))
            .Parameters.Append(.CreateParameter("@ServiceID", adInteger, adParamInput, , IIf(lngServiceID <= 0, DBNull.Value, lngServiceID)))
            .Parameters.Append(.CreateParameter("@ServicePeriod", adVarChar, adParamInput, 1, IIf(strServicePeriod = "", DBNull.Value, strServicePeriod)))
            .Parameters.Append(.CreateParameter("@ServiceStartDate", adDBTimeStamp, adParamInput, , IIf(dtServiceStartDate.ToOADate() = 0, DBNull.Value, dtServiceStartDate)))
            .Parameters.Append(.CreateParameter("@ServiceEndDate", adDBTimeStamp, adParamInput, , IIf(dtServiceEndDate.ToOADate() = 0, DBNull.Value, dtServiceEndDate)))
            .Parameters.Append(.CreateParameter("@ServiceAmount", adDouble, adParamInput, , dblServiceAmount))
            .Parameters.Append(.CreateParameter("@SalesRepID", adInteger, adParamInput, , IIf(lngSalesRepID <= 0, DBNull.Value, lngSalesRepID)))
            .Parameters.Append(.CreateParameter("@MarketSourceID", adInteger, adParamInput, , IIf(lngMarketSourceID <= 0, DBNull.Value, lngMarketSourceID)))
            .Parameters.Append(.CreateParameter("@SchInterval", adInteger, adParamInput, , IIf(lngSchInterval <= 0, DBNull.Value, lngSchInterval)))
            .Parameters.Append(.CreateParameter("@SchStartTime", adDBTimeStamp, adParamInput, , IIf(dtSchStartTime.ToOADate() = 0, DBNull.Value, dtSchStartTime)))
            .Parameters.Append(.CreateParameter("@SchEndTime", adDBTimeStamp, adParamInput, , IIf(dtSchEndTime.ToOADate() = 0, DBNull.Value, dtSchEndTime)))
            .Parameters.Append(.CreateParameter("@WebSite", adVarChar, adParamInput, 125, strWebSite))
            .Parameters.Append(.CreateParameter("@VirtualClientURL", adVarChar, adParamInput, 125, strVirtualClientURL))
            .Parameters.Append(.CreateParameter("@VirtualPatientURL", adVarChar, adParamInput, 125, strVirtualPatientURL))

            .Parameters.Append(.CreateParameter("@PatStmtYNE", adVarChar, adParamInput, 1, strPatStmtYNE))
            .Parameters.Append(.CreateParameter("@ApptReminder", adVarChar, adParamInput, 1, strApptReminder))
            .Parameters.Append(.CreateParameter("@AchSiteID", adVarChar, adParamInput, 20, strAchSiteID))

            .Parameters.Append(.CreateParameter("@AchSiteKey", adLongVarChar, adParamInput, IIf(Len(strAchSiteKey) = 0, 1, Len(strAchSiteKey))))
            .Parameters("@AchSiteKey").AppendChunk(strAchSiteKey)
            .Parameters.Append(.CreateParameter("@AchAPIKey", adLongVarChar, adParamInput, IIf(Len(strAchAPIKey) = 0, 1, Len(strAchAPIKey))))
            .Parameters("@AchAPIKey").AppendChunk(strAchAPIKey)

            .Parameters.Append(.CreateParameter("@Notes", adVarChar, adParamInput, 255, strNotes))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strAddedBy))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check for errors
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Function
        End If

        Insert = cmdSQL.Parameters(0).Value

        'Close the connection and free all resources
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        cnnSQL.Close()
        cnnSQL = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.Execute()
        cmdSQL = Nothing
        cnnSQL = Nothing
        'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME)

    End Function

    Public Sub Update(ByVal lngID As Long, ByVal strAddress1 As String, ByVal strAddress2 As String, _
                           ByVal strCity As String, ByVal strState As String, ByVal strZip As String, _
                           ByVal strMailAddress1 As String, ByVal strMailAddress2 As String, _
                           ByVal strMailCity As String, ByVal strMailState As String, ByVal strMailZip As String, _
                           ByVal dtBirthDate As Date, ByVal strNPI As String, ByVal strDEA As String, ByVal strTaxonomy As String, _
                           ByVal strPhone1 As String, ByVal strPhone2 As String, ByVal strFax As String, _
                           ByVal strBilling As String, ByVal varDegreeCredential As Object, _
                           ByVal varRecognizedCredential As Object, ByVal strStampLine1 As String, _
                           ByVal strStampLine2 As String, ByVal strStampline3 As String, _
                           ByVal varSignature As Object, ByVal lngLockBoxID As Long, ByVal lngSupervisorID As Long, _
                           ByVal lngServiceGroupID As Long, ByVal lngGroupID As Long, _
                           ByVal strCheckTo As String, ByVal dtStartDate As Date, ByVal dblSubmitRate As Double, _
                           ByVal dblCollectRate As Double, ByVal dblCollectPercent As Double, ByVal dtAutoCloseDate As Date, ByVal strSpecialPriceReason As String, _
                           ByVal intFreeSubmits As Integer, ByVal intFreeRemain As Integer, ByVal dtTrialEndDate As Date, ByVal strPatStmtYNE As String, _
                           ByVal strApptReminder As String, ByVal strAchSiteID As String, ByVal strAchSiteKey As String, ByVal strAchAPIKey As String, _
                           ByVal lngServiceID As Long, ByVal strServicePeriod As String, ByVal dtServiceStartDate As Date, _
                           ByVal dtServiceEndDate As Date, ByVal dblServiceAmount As Double, ByVal lngSalesRepID As Long, _
                           ByVal lngMarketSourceID As Long, ByVal lngSchInterval As Long, ByVal dtSchStartTime As Date, _
                           ByVal dtSchEndTime As Date, ByVal strWebSite As String, ByVal strVirtualClientURL As String, ByVal strVirtualPatientURL As String, _
                           ByVal strNotes As String, ByVal strUpdatedBy As String)

        '--------------------------------------------------------------------
        'Date: 02/02/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Updates a row into the tblProvider table utilizing   '
        '              a stored procedure.                                  '
        'Parameters: lngID - ID of the row in the table whose value will be '
        '                    updated.                                       '
        '            strAddress1 - "" Address line #1                       '
        '            strAddress2 - "" Address line #2                       '
        '            strCity - "" City                                      '
        '            strState - "" State                                    '
        '            strZip - "" Zip                                        '
        '            strPhone1 - "" Phone #1                                '
        '            strPhone2 - "" Phone #2                                '
        '            strFax - "" Fax #                                      '
        '            strPager - "" Pager #                                  '
        '            varDegreeCredential - "" scholastic credential         '
        '            varRecognizedCredential - "" Industry recognized       '
        '                                           credential              '
        '            strStampLine1 - "" stamp line 1                        '
        '            strStampLine2 - "" stamp line 2                        '
        '            strStampLine3 - "" stamp line 3                        '
        '            varSignature - "" signature file                       '
        '            strDisabledYN - "" deleted flag                        '
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
            .CommandText = "uspUpdProvider"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamInput, , lngID))
            .Parameters.Append(.CreateParameter("@Address1", adVarChar, adParamInput, 60, strAddress1))
            .Parameters.Append(.CreateParameter("@Address2", adVarChar, adParamInput, 60, strAddress2))
            .Parameters.Append(.CreateParameter("@City", adVarChar, adParamInput, 20, strCity))
            .Parameters.Append(.CreateParameter("@State", adVarChar, adParamInput, 2, strState))
            .Parameters.Append(.CreateParameter("@Zip", adVarChar, adParamInput, 11, md.NumbersOnly(strZip)))
            .Parameters.Append(.CreateParameter("@MailAddress1", adVarChar, adParamInput, 60, strMailAddress1))
            .Parameters.Append(.CreateParameter("@MailAddress2", adVarChar, adParamInput, 60, strMailAddress2))
            .Parameters.Append(.CreateParameter("@MailCity", adVarChar, adParamInput, 20, strMailCity))
            .Parameters.Append(.CreateParameter("@MailState", adVarChar, adParamInput, 2, strMailState))
            .Parameters.Append(.CreateParameter("@MailZip", adVarChar, adParamInput, 11, md.NumbersOnly(strMailZip)))
            .Parameters.Append(.CreateParameter("@BirthDate", adDBTimeStamp, adParamInput, , IIf(dtBirthDate.ToOADate() = 0, DBNull.Value, dtBirthDate)))
            .Parameters.Append(.CreateParameter("@NPI", adVarChar, adParamInput, 10, strNPI))
            .Parameters.Append(.CreateParameter("@DEA", adVarChar, adParamInput, 10, strDEA))
            .Parameters.Append(.CreateParameter("@Taxonomy", adVarChar, adParamInput, 11, strTaxonomy))
            .Parameters.Append(.CreateParameter("@Phone1", adVarChar, adParamInput, 14, md.NumbersOnly(strPhone1)))
            .Parameters.Append(.CreateParameter("@Phone2", adVarChar, adParamInput, 14, md.NumbersOnly(strPhone2)))
            .Parameters.Append(.CreateParameter("@Fax", adVarChar, adParamInput, 14, md.NumbersOnly(strFax)))
            .Parameters.Append(.CreateParameter("@Billing", adVarChar, adParamInput, 14, md.NumbersOnly(strBilling)))
            .Parameters.Append(.CreateParameter("@DegreeCredential", adInteger, adParamInput, , varDegreeCredential))
            .Parameters.Append(.CreateParameter("@RecognizedCredential", adInteger, adParamInput, , varRecognizedCredential))
            .Parameters.Append(.CreateParameter("@StampLine1", adVarChar, adParamInput, 50, strStampLine1))
            .Parameters.Append(.CreateParameter("@StampLine2", adVarChar, adParamInput, 50, strStampLine2))
            .Parameters.Append(.CreateParameter("@StampLine3", adVarChar, adParamInput, 50, strStampline3))

            If Len(varSignature) <> 0 Then  ' Determine if an image was passed
                .Parameters.Append(.CreateParameter("@Signature", adLongVarBinary, adParamInput, UBound(varSignature) + 1))
                .Parameters("@Signature").AppendChunk(varSignature)
            Else
                .Parameters.Append(.CreateParameter("@Signature", adLongVarBinary, adParamInput, -1, DBNull.Value))
            End If
            .Parameters.Append(.CreateParameter("@LockBoxID", adInteger, adParamInput, , lngLockBoxID))
            .Parameters.Append(.CreateParameter("@SupervisorID", adInteger, adParamInput, , IIf(lngSupervisorID < 0, DBNull.Value, lngSupervisorID)))
            .Parameters.Append(.CreateParameter("@ServiceGroupID", adInteger, adParamInput, , IIf(lngServiceGroupID <= 0, DBNull.Value, lngServiceGroupID)))
            .Parameters.Append(.CreateParameter("@GroupID", adInteger, adParamInput, , IIf(lngGroupID <= 0, DBNull.Value, lngGroupID)))
            .Parameters.Append(.CreateParameter("@CheckTo", adVarChar, adParamInput, 60, strCheckTo))
            .Parameters.Append(.CreateParameter("@StartDate", adDBTimeStamp, adParamInput, , IIf(dtStartDate.ToOADate() = 0, DBNull.Value, dtStartDate)))
            .Parameters.Append(.CreateParameter("@SubmitRate", adDouble, adParamInput, , dblSubmitRate))
            .Parameters.Append(.CreateParameter("@CollectRate", adDouble, adParamInput, , dblCollectRate))
            .Parameters.Append(.CreateParameter("@CollectPercent", adDouble, adParamInput, , dblCollectPercent))
            .Parameters.Append(.CreateParameter("@SpecialPriceReason", adVarChar, adParamInput, 50, strSpecialPriceReason))
            .Parameters.Append(.CreateParameter("@AutoCloseDate", adDBTimeStamp, adParamInput, , IIf(dtAutoCloseDate.ToOADate() = 0, DBNull.Value, dtAutoCloseDate)))
            .Parameters.Append(.CreateParameter("@FreeSubmits", adInteger, adParamInput, , intFreeSubmits))
            .Parameters.Append(.CreateParameter("@FreeRemain", adInteger, adParamInput, , intFreeRemain))
            .Parameters.Append(.CreateParameter("@FreeTrialEndDate", adDBTimeStamp, adParamInput, , IIf(dtTrialEndDate.ToOADate() = 0, DBNull.Value, dtTrialEndDate)))
            .Parameters.Append(.CreateParameter("@ServiceID", adInteger, adParamInput, , IIf(lngServiceID <= 0, DBNull.Value, lngServiceID)))
            .Parameters.Append(.CreateParameter("@ServicePeriod", adVarChar, adParamInput, 1, strServicePeriod))
            .Parameters.Append(.CreateParameter("@ServiceStartDate", adDBTimeStamp, adParamInput, , IIf(dtServiceStartDate.ToOADate() = 0, DBNull.Value, dtServiceStartDate)))
            .Parameters.Append(.CreateParameter("@ServiceEndDate", adDBTimeStamp, adParamInput, , IIf(dtServiceEndDate.ToOADate() = 0, DBNull.Value, dtServiceEndDate)))
            .Parameters.Append(.CreateParameter("@ServiceAmount", adDouble, adParamInput, , dblServiceAmount))
            .Parameters.Append(.CreateParameter("@SalesRepID", adInteger, adParamInput, , IIf(lngSalesRepID <= 0, DBNull.Value, lngSalesRepID)))
            .Parameters.Append(.CreateParameter("@MarketSourceID", adInteger, adParamInput, , IIf(lngMarketSourceID <= 0, DBNull.Value, lngMarketSourceID)))
            .Parameters.Append(.CreateParameter("@SchInterval", adInteger, adParamInput, , IIf(lngSchInterval <= 0, DBNull.Value, lngSchInterval)))
            .Parameters.Append(.CreateParameter("@SchStartTime", adDBTimeStamp, adParamInput, , IIf(dtSchStartTime.ToOADate() = 0, DBNull.Value, dtSchStartTime)))
            .Parameters.Append(.CreateParameter("@SchEndTime", adDBTimeStamp, adParamInput, , IIf(dtSchEndTime.ToOADate() = 0, DBNull.Value, dtSchEndTime)))
            .Parameters.Append(.CreateParameter("@WebSite", adVarChar, adParamInput, 125, strWebSite))
            .Parameters.Append(.CreateParameter("@VirtualClientURL", adVarChar, adParamInput, 125, strVirtualClientURL))
            .Parameters.Append(.CreateParameter("@VirtualPatientURL", adVarChar, adParamInput, 125, strVirtualPatientURL))

            .Parameters.Append(.CreateParameter("@PatStmtYNE", adVarChar, adParamInput, 1, strPatStmtYNE))
            .Parameters.Append(.CreateParameter("@ApptReminder", adVarChar, adParamInput, 1, strApptReminder))
            .Parameters.Append(.CreateParameter("@AchSiteID", adVarChar, adParamInput, 20, strAchSiteID))

            .Parameters.Append(.CreateParameter("@AchSiteKey", adLongVarChar, adParamInput, IIf(Len(strAchSiteKey) = 0, 1, Len(strAchSiteKey))))
            .Parameters("@AchSiteKey").AppendChunk(strAchSiteKey)
            .Parameters.Append(.CreateParameter("@AchAPIKey", adLongVarChar, adParamInput, IIf(Len(strAchAPIKey) = 0, 1, Len(strAchAPIKey))))
            .Parameters("@AchAPIKey").AppendChunk(strAchAPIKey)

            .Parameters.Append(.CreateParameter("@Notes", adVarChar, adParamInput, 255, strNotes))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check for errors
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        'Close the connection and free all resources
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        cnnSQL.Close()
        cnnSQL = Nothing

        'Signal successful completion


        Exit Sub


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.Execute()
        cmdSQL = Nothing
        cnnSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

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
            .CommandText = "uspUpdProviderFlags"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamInput, , lngID))
            .Parameters.Append(.CreateParameter("@ProvDirectYN", adVarChar, adParamInput, 1, IIf(blnProvDirectYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@BulkCheckYN", adVarChar, adParamInput, 1, IIf(blnBulkCheckYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@CheckToYN", adVarChar, adParamInput, 1, IIf(blnCheckToYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@SubmissionOnlyYN", adVarChar, adParamInput, 1, IIf(blnSubmissionOnlyYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@PayOnlineYN", adVarChar, adParamInput, 1, IIf(blnPayOnlineYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@PatSchedYN", adVarChar, adParamInput, 1, IIf(blnPatSchedYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@PatIntakeYN", adVarChar, adParamInput, 1, IIf(blnPatIntakeYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@PatCreditCardYN", adVarChar, adParamInput, 1, IIf(blnPatCreditCardYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@SpecialHandlingYN", adVarChar, adParamInput, 1, IIf(blnSpecialHandlingYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@PrintDiagYN", adVarChar, adParamInput, 1, IIf(blnPrintDiagYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@UseNotesYN", adVarChar, adParamInput, 1, IIf(blnUseNotesYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@eScribeYN", adVarChar, adParamInput, 1, IIf(blneScribeYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@MipsYN", adVarChar, adParamInput, 1, IIf(blnMipsYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@SpecialPriceYN", adVarChar, adParamInput, 1, IIf(blnSpecialPriceYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@FreeTrialYN", adVarChar, adParamInput, 1, IIf(blnFreeTrialYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@CCardYN", adVarChar, adParamInput, 1, IIf(blnCCardYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@CCRequiredYN", adVarChar, adParamInput, 1, IIf(blnCCRequiredYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@ICheckYN", adVarChar, adParamInput, 1, IIf(blnICheckYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@PatNoteReqYNR", adVarChar, adParamInput, 1, strPatNoteReqYNR))
            .Parameters.Append(.CreateParameter("@EncDetailYN", adVarChar, adParamInput, 1, IIf(blnEncDetailYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@VirtualSessionYN", adVarChar, adParamInput, 1, IIf(blnVirtualSessionYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@PatOnlinePayYN", adVarChar, adParamInput, 1, IIf(blnPatOnlinePayYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, strUpdatedBy))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check for errors
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        'Close the connection and free all resources
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        cnnSQL.Close()
        cnnSQL = Nothing

        'Signal successful completion


        Exit Sub


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.Execute()
        cmdSQL = Nothing
        cnnSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Sub


    Public Function Exists(ByVal strLast As String, ByVal strFirst As String, _
                           ByVal strMI As String, _
                           ByVal lngDegreeCredential As Long, _
                           ByVal lngRecognizedCredential As Long, _
                           ByVal strSSN As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 02/02/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Determines if a provider record identical to the     '
        '              name parameters is already associated to the clinic  '
        '              whose ID matches lngClinic.                          '
        'Parameters: strLast - Provider's Last Name                         '
        '            strFirst - "" First Name                               '
        '            strMI - "" Middle Initial                              '
        '            lngDegreeCredential - provider's degree credential     '
        '            lngRecognizedCredential - provider's industry-         '
        '               recognized credential strSSN - provider's SSN       '
        'Returns: True if the association exists, false otherwise           '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim strSQL As String
        Dim blnExists As Boolean



        'Prepare the SQL statement
        strSQL = "SELECT "
        strSQL = strSQL & " fldProviderID "
        strSQL = strSQL & " FROM "
        strSQL = strSQL & TABLE_NAME
        strSQL = strSQL & " WHERE "
        strSQL = strSQL & " fldLast = '"
        strSQL = strSQL & strLast
        strSQL = strSQL & "' AND fldFirst = '"
        strSQL = strSQL & strFirst & "' "

        If Trim(strMI) > "" Then
            strSQL = strSQL & " AND "
            strSQL = strSQL & " fldMI = '"
            strSQL = strSQL & strMI & "' "
        Else
            strSQL = strSQL & " AND "
            strSQL = strSQL & " (fldMI = '' "
            strSQL = strSQL & " OR "
            strSQL = strSQL & " fldMI = NULL) "
        End If

        strSQL = strSQL & " AND "
        strSQL = strSQL & " fldDegreeCredential = "
        strSQL = strSQL & lngDegreeCredential
        strSQL = strSQL & " AND "
        strSQL = strSQL & " fldRecognizedCredential = "
        strSQL = strSQL & lngRecognizedCredential

        If CleanNumber(strSSN) > "" Then
            strSQL = strSQL & " AND "
            strSQL = strSQL & " fldSSN = '"
            strSQL = strSQL & strSSN
            strSQL = strSQL & "'"
        End If

        'Instantiate the Recordset.
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = adUseClient

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'populate the recordset
        rstSQL.Open(strSQL, cnnSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rstSQL.ActiveConnection = Nothing
        cnnSQL.Close()
        cnnSQL = Nothing

        'Evaluate recordcount of Recordset then destroy it.
        If rstSQL.RecordCount > 0 Then
            Exists = True
        Else
            Exists = False
        End If

        rstSQL = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        cnnSQL = Nothing
        rstSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function

    Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Long, _
                       ByVal strDeletedBy As String)
        '--------------------------------------------------------------------
        'Date: 02/06/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Flags a row in the tbProviderr table marking the row '
        '              as deleted or undeleted.                             '
        'Parameters: blnDeleted - Boolean value identifying if the record is'
        '               to be deleted (True) or undeleted (False).          '
        '            lngID - ID of the row in the table whose value will be '
        '               updated.                                            '
        '            strDeletedBy - Login name of the user performing the   '
        '               action.                                             '
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim strSQL As String
        Dim blnFlag As String



        If blnDeleted = True Then
            blnFlag = "Y"
        Else
            blnFlag = "N"
        End If

        'Prepare the SQL statement
        strSQL = "UPDATE "
        strSQL = strSQL & TABLE_NAME
        strSQL = strSQL & " SET "
        strSQL = strSQL & " fldDisabledYN = '" & blnFlag & "', "
        strSQL = strSQL & " fldDateDisabled = '" & Date.Now & "', "
        strSQL = strSQL & " fldDisabledBy = '" & strDeletedBy & "' "
        strSQL = strSQL & " WHERE "
        strSQL = strSQL & " fldProviderID = " & lngID

        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        cmdSQL.CommandText = strSQL
        cmdSQL.CommandType = adCmdText

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Close the connection and free all resources
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        cnnSQL.Close()
        cnnSQL = Nothing

        'Signal successful completion


        Exit Sub


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.Execute()
        cmdSQL = Nothing
        cnnSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Sub

    Public Function OkToAssociate(ByVal lngID As Long, _
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
        '           strMI - Provider's MI                                   '
        '           lngClinicID - ID of clinic to associate with provider   '
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim strSQL As String



        ' Prepare SQL statement
        strSQL = "SELECT "
        strSQL = strSQL & "tblUser.fldDisabledYN "
        strSQL = strSQL & "FROM "
        strSQL = strSQL & LINKING_TABLE_NAME & " INNER JOIN "
        strSQL = strSQL & "tblUser ON "
        strSQL = strSQL & LINKING_TABLE_NAME & ".fldProviderID = tblUser.fldUserID "
        strSQL = strSQL & "WHERE "
        strSQL = strSQL & "tblUser.fldLastName = '" & ds.ParseSQL(strLast) & "' AND "
        strSQL = strSQL & "tblUser.fldFirstName = '" & ds.ParseSQL(strFirst) & "' AND "

        If Trim(strMI) > "" Then
            strSQL = strSQL & "tblUser.fldMI = '" & strMI & "' "
        Else
            strSQL = strSQL & "("
            strSQL = strSQL & "tblUser.fldMI = NULL "
            strSQL = strSQL & "OR "
            strSQL = strSQL & "tblUser.fldMI = ''"
            strSQL = strSQL & ") "
        End If

        strSQL = strSQL & "AND "
        strSQL = strSQL & LINKING_TABLE_NAME & ".fldProviderID = " & lngID & " AND "
        strSQL = strSQL & LINKING_TABLE_NAME & ".fldClinicID = " & lngClinicID

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Instantiate and populate the Recordset.
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(strSQL, cnnSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rstSQL.ActiveConnection = Nothing
        cnnSQL.Close()
        cnnSQL = Nothing

        'Evaluate recordcount of Recordset then destroy it.
        If rstSQL.RecordCount > 0 Then
            OkToAssociate = False
        Else
            OkToAssociate = True
        End If

        rstSQL = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        cnnSQL = Nothing
        rstSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function


    Public Function FetchClinics(ByVal lngProviderID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/07/2000
        'Author: Dave Richkun
        'Description:  Retrieves a recordset of Clinics for a specific provider
        'Parameters: lngProviderID - ID of Provider whose ClinicID records will be
        '              retrieved.
        'Returns: Recordset of Provider Clinics
        '--------------------------------------------------------------------


        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim rstSQL As ADODB.Recordset



        cmdSQL = New ADODB.Command
        rstSQL = New ADODB.Recordset

        ' Prepare stored procedure parameters
        With cmdSQL
            .CommandText = "uspSelProviderClinics"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
        End With

        cnnSQL = New ADODB.Connection
        'Acquire the database connection.
        cnnSQL.Open(_ConnectionString)
        cmdSQL.ActiveConnection = cnnSQL

        'Instantiate and populate the Recordset.
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rstSQL.ActiveConnection = Nothing
        cnnSQL.Close()
        cnnSQL = Nothing

        FetchClinics = rstSQL

        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.Execute()
        rstSQL = Nothing
        cmdSQL = Nothing
        cnnSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function

    Public Function FetchCPTCodes(ByVal lngProviderID As Long, _
                          Optional ByVal blnIncludeDisabled As Boolean = False, _
                          Optional ByVal strUserWhere As String = "", _
                          Optional ByVal strOrderBy As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/05/2004
        'Author: Duane C Orth
        'Description:  Retrieves a recordset of Clinics for a specific provider
        'Parameters: lngProviderID - ID of Provider whose C records will be
        '              retrieved.
        'Returns: Recordset of Provider CPTCodes
        '--------------------------------------------------------------------


        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim rstSQL As ADODB.Recordset



        cmdSQL = New ADODB.Command
        rstSQL = New ADODB.Recordset

        ' Prepare stored procedure parameters
        With cmdSQL
            .CommandText = "uspSelProviderCPTCode"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@IncludeDisabled", adVarChar, adParamInput, 1, IIf(blnIncludeDisabled, "Y", "N")))
            .Parameters.Append(.CreateParameter("@Where", adVarChar, adParamInput, 250, strUserWhere))
            .Parameters.Append(.CreateParameter("@Order", adVarChar, adParamInput, 25, strOrderBy))
        End With

        cnnSQL = New ADODB.Connection
        'Acquire the database connection.
        cnnSQL.Open(_ConnectionString)
        cmdSQL.ActiveConnection = cnnSQL

        'Instantiate and populate the Recordset.
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rstSQL.ActiveConnection = Nothing
        cnnSQL.Close()
        cnnSQL = Nothing

        FetchCPTCodes = rstSQL

        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.Execute()
        rstSQL = Nothing
        cmdSQL = Nothing
        cnnSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function

    Public Sub UpdateProfile(ByVal lngID As Long, ByVal strAddress1 As String, _
                      ByVal strAddress2 As String, ByVal strCity As String, _
                      ByVal strState As String, ByVal strZip As String, _
                      ByVal strPhone1 As String, ByVal strPhone2 As String, _
                      ByVal strFax As String, ByVal strBilling As String, _
                      ByVal strEmail As String, ByVal strUpdatedBy As String)
        '--------------------------------------------------------------------
        'Date: 05/07/2001                                                   '
        'Author: Dave Richkun
        'Description:  Updates Provider demographics in  tblProvider table
        '              utilizing a stored procedure.
        'Parameters: lngID - ID of the Provider whose demographics are being updated.
        '            Others - obvious
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '  R002 - Created
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        With cmdSQL
            .CommandText = "uspUpdProviderProfile"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamInput, , lngID))
            .Parameters.Append(.CreateParameter("@Address1", adVarChar, adParamInput, 60, strAddress1))
            .Parameters.Append(.CreateParameter("@Address2", adVarChar, adParamInput, 60, strAddress2))
            .Parameters.Append(.CreateParameter("@City", adVarChar, adParamInput, 20, strCity))
            .Parameters.Append(.CreateParameter("@State", adVarChar, adParamInput, 2, strState))
            .Parameters.Append(.CreateParameter("@Zip", adVarChar, adParamInput, 11, md.NumbersOnly(strZip)))
            .Parameters.Append(.CreateParameter("@Phone1", adVarChar, adParamInput, 14, md.NumbersOnly(strPhone1)))
            .Parameters.Append(.CreateParameter("@Phone2", adVarChar, adParamInput, 14, md.NumbersOnly(strPhone2)))
            .Parameters.Append(.CreateParameter("@Fax", adVarChar, adParamInput, 14, md.NumbersOnly(strFax)))
            .Parameters.Append(.CreateParameter("@Billing", adVarChar, adParamInput, 14, md.NumbersOnly(strBilling)))
            .Parameters.Append(.CreateParameter("@Email", adVarChar, adParamInput, 60, strEmail))
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, strUpdatedBy))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check for errors
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        'Close the connection and free all resources
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        cnnSQL.Close()
        cnnSQL = Nothing

        'Signal successful completion


        Exit Sub


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.Execute()
        cmdSQL = Nothing
        cnnSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Sub

    Public Sub UpdateGoogleRefresh(ByVal lngID As Long, ByVal strGoogleAPIRefresh As String)
        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        With cmdSQL
            .CommandText = "uspUpdProviderGoogleAPI"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamInput, , lngID))
            .Parameters.Append(.CreateParameter("@GoogleAPIRefresh", adVarChar, adParamInput, 300, strGoogleAPIRefresh))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check for errors
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        'Close the connection and free all resources
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        cnnSQL.Close()
        cnnSQL = Nothing

        'Signal successful completion


        Exit Sub


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.Execute()
        cmdSQL = Nothing
        cnnSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Sub
    Public Sub InsertIntakeProfile(ByVal lngProviderID As Long, ByVal strSunYN As String, _
            ByVal intSunStartTime As Integer, ByVal intSunEndTime As Integer, ByVal strMonYN As String, _
            ByVal intMonStartTime As Integer, ByVal intMonEndTime As Integer, ByVal strTueYN As String, _
            ByVal intTueStartTime As Integer, ByVal intTueEndTime As Integer, ByVal strWedYN As String, _
            ByVal intWedStartTime As Integer, ByVal intWedEndTime As Integer, ByVal strThuYN As String, _
            ByVal intThuStartTime As Integer, ByVal intThuEndTime As Integer, ByVal strFriYN As String, _
            ByVal intFriStartTime As Integer, ByVal intFriEndTime As Integer, ByVal strSatYN As String, _
            ByVal intSatStartTime As Integer, ByVal intSatEndTime As Integer, ByVal strGender As String, _
            ByVal lngEthnicity As Long, ByVal strAddedBy As String)
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
        '            strUpdatedBy - Name of user inserting the record
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '  R005 - Created
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.  The Times are prefixed with the date of 01/01/1970 in order to support
        'the SMALLDATETIME datatype in SQL Server.
        cmdSQL = New ADODB.Command
        With cmdSQL
            .CommandText = "uspInsIntakeProfile"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@SunYN", adVarChar, adParamInput, 1, strSunYN))
            .Parameters.Append(.CreateParameter("@SunStartTime", adDBTimeStamp, adParamInput, , IIf(intSunStartTime > 0, CDate("01/01/1970 " & intSunStartTime & ":00:00"), DBNull.Value)))
            .Parameters.Append(.CreateParameter("@SunEndTime", adDBTimeStamp, adParamInput, , IIf(intSunEndTime > 0, CDate("01/01/1970 " & intSunEndTime & ":00:00"), DBNull.Value)))
            .Parameters.Append(.CreateParameter("@MonYN", adVarChar, adParamInput, 1, strMonYN))
            .Parameters.Append(.CreateParameter("@MonStartTime", adDBTimeStamp, adParamInput, , IIf(intMonStartTime > 0, CDate("01/01/1970 " & intMonStartTime & ":00:00"), DBNull.Value)))
            .Parameters.Append(.CreateParameter("@MonEndTime", adDBTimeStamp, adParamInput, , IIf(intMonEndTime > 0, CDate("01/01/1970 " & intMonEndTime & ":00:00"), DBNull.Value)))
            .Parameters.Append(.CreateParameter("@TueYN", adVarChar, adParamInput, 1, strTueYN))
            .Parameters.Append(.CreateParameter("@TueStartTime", adDBTimeStamp, adParamInput, , IIf(intTueStartTime > 0, CDate("01/01/1970 " & intTueStartTime & ":00:00"), DBNull.Value)))
            .Parameters.Append(.CreateParameter("@TueEndTime", adDBTimeStamp, adParamInput, , IIf(intTueEndTime > 0, CDate("01/01/1970 " & intTueEndTime & ":00:00"), DBNull.Value)))
            .Parameters.Append(.CreateParameter("@WedYN", adVarChar, adParamInput, 1, strWedYN))
            .Parameters.Append(.CreateParameter("@WedStartTime", adDBTimeStamp, adParamInput, , IIf(intWedStartTime > 0, CDate("01/01/1970 " & intWedStartTime & ":00:00"), DBNull.Value)))
            .Parameters.Append(.CreateParameter("@WedEndTime", adDBTimeStamp, adParamInput, , IIf(intWedEndTime > 0, CDate("01/01/1970 " & intWedEndTime & ":00:00"), DBNull.Value)))
            .Parameters.Append(.CreateParameter("@ThuYN", adVarChar, adParamInput, 1, strThuYN))
            .Parameters.Append(.CreateParameter("@ThuStartTime", adDBTimeStamp, adParamInput, , IIf(intThuStartTime > 0, CDate("01/01/1970 " & intThuStartTime & ":00:00"), DBNull.Value)))
            .Parameters.Append(.CreateParameter("@ThuEndTime", adDBTimeStamp, adParamInput, , IIf(intThuEndTime > 0, CDate("01/01/1970 " & intThuEndTime & ":00:00"), DBNull.Value)))
            .Parameters.Append(.CreateParameter("@FriYN", adVarChar, adParamInput, 1, strFriYN))
            .Parameters.Append(.CreateParameter("@FriStartTime", adDBTimeStamp, adParamInput, , IIf(intFriStartTime > 0, CDate("01/01/1970 " & intFriStartTime & ":00:00"), DBNull.Value)))
            .Parameters.Append(.CreateParameter("@FriEndTime", adDBTimeStamp, adParamInput, , IIf(intFriEndTime > 0, CDate("01/01/1970 " & intFriEndTime & ":00:00"), DBNull.Value)))
            .Parameters.Append(.CreateParameter("@SatYN", adVarChar, adParamInput, 1, strSatYN))
            .Parameters.Append(.CreateParameter("@SatStartTime", adDBTimeStamp, adParamInput, , IIf(intSatStartTime > 0, CDate("01/01/1970 " & intSatStartTime & ":00:00"), DBNull.Value)))
            .Parameters.Append(.CreateParameter("@SatEndTime", adDBTimeStamp, adParamInput, , IIf(intSatEndTime > 0, CDate("01/01/1970 " & intSatEndTime & ":00:00"), DBNull.Value)))
            .Parameters.Append(.CreateParameter("@Gender", adVarChar, adParamInput, 1, IIf(strGender > "", strGender, DBNull.Value)))
            .Parameters.Append(.CreateParameter("@Ethnicity", adInteger, adParamInput, 1, IIf(lngEthnicity > 0, lngEthnicity, DBNull.Value)))
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, strAddedBy))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check for errors
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        'Close the connection and free all resources
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        cnnSQL.Close()
        cnnSQL = Nothing

        'Signal successful completion


        Exit Sub


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.Execute()
        cmdSQL = Nothing
        cnnSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Sub

    Public Sub UpdateIntakeProfile(ByVal lngProviderID As Long, ByVal strSunYN As String, _
            ByVal intSunStartTime As Integer, ByVal intSunEndTime As Integer, ByVal strMonYN As String, _
            ByVal intMonStartTime As Integer, ByVal intMonEndTime As Integer, ByVal strTueYN As String, _
            ByVal intTueStartTime As Integer, ByVal intTueEndTime As Integer, ByVal strWedYN As String, _
            ByVal intWedStartTime As Integer, ByVal intWedEndTime As Integer, ByVal strThuYN As String, _
            ByVal intThuStartTime As Integer, ByVal intThuEndTime As Integer, ByVal strFriYN As String, _
            ByVal intFriStartTime As Integer, ByVal intFriEndTime As Integer, ByVal strSatYN As String, _
            ByVal intSatStartTime As Integer, ByVal intSatEndTime As Integer, ByVal strGender As String, _
            ByVal lngEthnicity As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 09/25/2001                                                   '
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
        '  R005 - Created
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.  The Times are prefixed with the date of 01/01/1970 in order to support
        'the SMALLDATETIME datatype in SQL Server.
        cmdSQL = New ADODB.Command
        With cmdSQL
            .CommandText = "uspUpdIntakeProfile"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@SunYN", adVarChar, adParamInput, 1, strSunYN))
            .Parameters.Append(.CreateParameter("@SunStartTime", adDBTimeStamp, adParamInput, , IIf(intSunStartTime > 0, CDate("01/01/1970 " & intSunStartTime & ":00:00"), DBNull.Value)))
            .Parameters.Append(.CreateParameter("@SunEndTime", adDBTimeStamp, adParamInput, , IIf(intSunEndTime > 0, CDate("01/01/1970 " & intSunEndTime & ":00:00"), DBNull.Value)))
            .Parameters.Append(.CreateParameter("@MonYN", adVarChar, adParamInput, 1, strMonYN))
            .Parameters.Append(.CreateParameter("@MonStartTime", adDBTimeStamp, adParamInput, , IIf(intMonStartTime > 0, CDate("01/01/1970 " & intMonStartTime & ":00:00"), DBNull.Value)))
            .Parameters.Append(.CreateParameter("@MonEndTime", adDBTimeStamp, adParamInput, , IIf(intMonEndTime > 0, CDate("01/01/1970 " & intMonEndTime & ":00:00"), DBNull.Value)))
            .Parameters.Append(.CreateParameter("@TueYN", adVarChar, adParamInput, 1, strTueYN))
            .Parameters.Append(.CreateParameter("@TueStartTime", adDBTimeStamp, adParamInput, , IIf(intTueStartTime > 0, CDate("01/01/1970 " & intTueStartTime & ":00:00"), DBNull.Value)))
            .Parameters.Append(.CreateParameter("@TueEndTime", adDBTimeStamp, adParamInput, , IIf(intTueEndTime > 0, CDate("01/01/1970 " & intTueEndTime & ":00:00"), DBNull.Value)))
            .Parameters.Append(.CreateParameter("@WedYN", adVarChar, adParamInput, 1, strWedYN))
            .Parameters.Append(.CreateParameter("@WedStartTime", adDBTimeStamp, adParamInput, , IIf(intWedStartTime > 0, CDate("01/01/1970 " & intWedStartTime & ":00:00"), DBNull.Value)))
            .Parameters.Append(.CreateParameter("@WedEndTime", adDBTimeStamp, adParamInput, , IIf(intWedEndTime > 0, CDate("01/01/1970 " & intWedEndTime & ":00:00"), DBNull.Value)))
            .Parameters.Append(.CreateParameter("@ThuYN", adVarChar, adParamInput, 1, strThuYN))
            .Parameters.Append(.CreateParameter("@ThuStartTime", adDBTimeStamp, adParamInput, , IIf(intThuStartTime > 0, CDate("01/01/1970 " & intThuStartTime & ":00:00"), DBNull.Value)))
            .Parameters.Append(.CreateParameter("@ThuEndTime", adDBTimeStamp, adParamInput, , IIf(intThuEndTime > 0, CDate("01/01/1970 " & intThuEndTime & ":00:00"), DBNull.Value)))
            .Parameters.Append(.CreateParameter("@FriYN", adVarChar, adParamInput, 1, strFriYN))
            .Parameters.Append(.CreateParameter("@FriStartTime", adDBTimeStamp, adParamInput, , IIf(intFriStartTime > 0, CDate("01/01/1970 " & intFriStartTime & ":00:00"), DBNull.Value)))
            .Parameters.Append(.CreateParameter("@FriEndTime", adDBTimeStamp, adParamInput, , IIf(intFriEndTime > 0, CDate("01/01/1970 " & intFriEndTime & ":00:00"), DBNull.Value)))
            .Parameters.Append(.CreateParameter("@SatYN", adVarChar, adParamInput, 1, strSatYN))
            .Parameters.Append(.CreateParameter("@SatStartTime", adDBTimeStamp, adParamInput, , IIf(intSatStartTime > 0, CDate("01/01/1970 " & intSatStartTime & ":00:00"), DBNull.Value)))
            .Parameters.Append(.CreateParameter("@SatEndTime", adDBTimeStamp, adParamInput, , IIf(intSatEndTime > 0, CDate("01/01/1970 " & intSatEndTime & ":00:00"), DBNull.Value)))
            .Parameters.Append(.CreateParameter("@Gender", adVarChar, adParamInput, 1, IIf(strGender > "", strGender, DBNull.Value)))
            .Parameters.Append(.CreateParameter("@Ethnicity", adInteger, adParamInput, 1, IIf(lngEthnicity > 0, lngEthnicity, DBNull.Value)))
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check for errors
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        'Close the connection and free all resources
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        cnnSQL.Close()
        cnnSQL = Nothing

        'Signal successful completion


        Exit Sub


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.Execute()
        cmdSQL = Nothing
        cnnSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Sub


    Public Sub IntakeOptOut(ByVal lngProviderID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 09/25/2001                                                   '
        'Author: Dave Richkun
        'Description:  Updates a Provider's profile, setting the flag that allows them to
        '               unsubscribe to Intake Services.
        'Parameters: lngProviderID - ID of the Provider whose profile will be updated
        '            strUpdatedBy - Name of user updating the record
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '  R005 - Created
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        With cmdSQL
            .CommandText = "uspIntakeOptOut"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check for errors
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        'Close the connection and free all resources
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        cnnSQL.Close()
        cnnSQL = Nothing

        'Signal successful completion


        Exit Sub


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.Execute()
        cmdSQL = Nothing
        cnnSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Sub

    Public Function FetchIntakeTimes(ByVal lngProviderID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/27/2001
        'Author: Dave Richkun
        'Description:  Retrieves a recordset containing Providers established Intake times
        'Parameters: lngProviderID - ID of Provider whose time record will be retrieved.
        'Returns: Recordset of Provider Intake times
        '--------------------------------------------------------------------
        '  R005: Created
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim rstSQL As ADODB.Recordset



        cmdSQL = New ADODB.Command
        rstSQL = New ADODB.Recordset

        ' Prepare stored procedure parameters
        With cmdSQL
            .CommandText = "uspSelProviderIntakeTimes"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
        End With

        cnnSQL = New ADODB.Connection
        'Acquire the database connection.
        cnnSQL.Open(_ConnectionString)
        cmdSQL.ActiveConnection = cnnSQL

        'Instantiate and populate the Recordset.
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rstSQL.ActiveConnection = Nothing
        cnnSQL.Close()
        cnnSQL = Nothing

        FetchIntakeTimes = rstSQL

        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.Execute()
        rstSQL = Nothing
        cmdSQL = Nothing
        cnnSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function


    Public Function FetchSpecialties(ByVal lngProviderID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/27/2001
        'Author: Dave Richkun
        'Description:  Retrieves a recordset containing Provider specialties
        'Parameters: lngProviderID - ID of Provider whose specialties will be retrieved.
        'Returns: Recordset of Provider Specialties
        '--------------------------------------------------------------------
        '  R005: Created
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim rstSQL As ADODB.Recordset



        cmdSQL = New ADODB.Command
        rstSQL = New ADODB.Recordset

        ' Prepare stored procedure parameters
        With cmdSQL
            .CommandText = "uspSelProviderSpecialties"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
        End With

        cnnSQL = New ADODB.Connection
        'Acquire the database connection.
        cnnSQL.Open(_ConnectionString)
        cmdSQL.ActiveConnection = cnnSQL

        'Instantiate and populate the Recordset.
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rstSQL.ActiveConnection = Nothing
        cnnSQL.Close()
        cnnSQL = Nothing

        FetchSpecialties = rstSQL

        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.Execute()
        rstSQL = Nothing
        cmdSQL = Nothing
        cnnSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function

    Public Function FetchIntakeProfileOpts(ByVal lngProviderID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/27/2001
        'Author: Dave Richkun
        'Description:  Retrieves a recordset containing optional (non-required) Provider Intake data
        'Parameters: lngProviderID - ID of Provider whose optional Profile data will be retrieved.
        'Returns: Recordset of Provider Options
        '--------------------------------------------------------------------
        '  R005: Created
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim rstSQL As ADODB.Recordset



        cmdSQL = New ADODB.Command
        rstSQL = New ADODB.Recordset

        ' Prepare stored procedure parameters
        With cmdSQL
            .CommandText = "uspSelProviderIntakeOpts"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
        End With

        cnnSQL = New ADODB.Connection
        'Acquire the database connection.
        cnnSQL.Open(_ConnectionString)
        cmdSQL.ActiveConnection = cnnSQL

        'Instantiate and populate the Recordset.
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rstSQL.ActiveConnection = Nothing
        cnnSQL.Close()
        cnnSQL = Nothing

        FetchIntakeProfileOpts = rstSQL

        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.Execute()
        rstSQL = Nothing
        cmdSQL = Nothing
        cnnSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

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
        '  R005: Created
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim rstSQL As ADODB.Recordset



        cmdSQL = New ADODB.Command
        rstSQL = New ADODB.Recordset

        ' Prepare stored procedure parameters
        With cmdSQL
            .CommandText = "uspSelProviderIntakeExists"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
        End With

        cnnSQL = New ADODB.Connection
        'Acquire the database connection.
        cnnSQL.Open(_ConnectionString)
        cmdSQL.ActiveConnection = cnnSQL

        'Instantiate and populate the Recordset.
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rstSQL.ActiveConnection = Nothing
        cnnSQL.Close()
        cnnSQL = Nothing

        If rstSQL.RecordCount > 0 Then
            IntakeProfileExists = True
        Else
            IntakeProfileExists = False
        End If

        rstSQL = Nothing
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.Execute()
        rstSQL = Nothing
        cmdSQL = Nothing
        cnnSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function
    Public Function FetchProviderPatientPortal(ByVal lngProviderID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 05/25/2020
        'Author: Duane C Orth
        'Description:  Retrieves a recordset containing optional (non-required) Provider Intake data
        'Parameters: lngProviderID - ID of Provider whose optional Profile data will be retrieved.
        'Returns: Recordset of Provider Options
        '--------------------------------------------------------------------
        '  R005: Created
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim rstSQL As ADODB.Recordset



        cmdSQL = New ADODB.Command
        rstSQL = New ADODB.Recordset

        ' Prepare stored procedure parameters
        With cmdSQL
            .CommandText = "uspSelProviderPatientPortal"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
        End With

        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)
        cmdSQL.ActiveConnection = cnnSQL

        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        rstSQL.ActiveConnection = Nothing
        cnnSQL.Close()
        cnnSQL = Nothing

        FetchProviderPatientPortal = rstSQL

        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.Execute()
        rstSQL = Nothing
        cmdSQL = Nothing
        cnnSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

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

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        With cmdSQL
            .CommandText = "uspUpdReduceProvDenyCtr"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check for errors
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        'Close the connection and free all resources
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        cnnSQL.Close()
        cnnSQL = Nothing

        'Signal successful completion


        Exit Sub


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.Execute()
        cmdSQL = Nothing
        cnnSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

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

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim rstSQL As ADODB.Recordset



        cmdSQL = New ADODB.Command
        rstSQL = New ADODB.Recordset

        ' Prepare stored procedure parameters
        With cmdSQL
            .CommandText = "uspRptProvIOU"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection

        cnnSQL.Open(_ConnectionString)
        cmdSQL.ActiveConnection = cnnSQL

        'Instantiate and populate the Recordset.
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rstSQL.ActiveConnection = Nothing
        cnnSQL.Close()

        cnnSQL = Nothing
        cmdSQL = Nothing
        cnnSQL = Nothing

        FetchIOUs = rstSQL

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.Execute()

        rstSQL = Nothing
        cmdSQL = Nothing
        cnnSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function


    Public Function FetchPayments(ByVal lngProviderID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 02/06/2003
        'Author: Dave Richkun
        'Description:  Retrieves a recordset of payments for a specific provider
        'Parameters: lngProviderID - ID of Provider whose payment records will be
        '              retrieved.
        'Returns: Recordset of IOUs
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim rstSQL As ADODB.Recordset



        cmdSQL = New ADODB.Command
        rstSQL = New ADODB.Recordset

        ' Prepare stored procedure parameters
        With cmdSQL
            .CommandText = "uspRptProvPymts"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection

        cnnSQL.Open(_ConnectionString)
        cmdSQL.ActiveConnection = cnnSQL

        'Instantiate and populate the Recordset.
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rstSQL.ActiveConnection = Nothing
        cnnSQL.Close()

        cnnSQL = Nothing
        cmdSQL = Nothing
        cnnSQL = Nothing

        FetchPayments = rstSQL

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.Execute()

        rstSQL = Nothing
        cmdSQL = Nothing
        cnnSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function

    Public Function FetchByID(ByVal lngProviderID As Long) As ADODB.Recordset

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim rstSQL As ADODB.Recordset



        cmdSQL = New ADODB.Command
        rstSQL = New ADODB.Recordset

        ' Prepare stored procedure parameters
        With cmdSQL
            .CommandText = "uspSelProviderInfo"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        cnnSQL = New ADODB.Connection
        'Acquire the database connection.
        cnnSQL.Open(_ConnectionString)
        cmdSQL.ActiveConnection = cnnSQL

        'Instantiate and populate the Recordset.
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rstSQL.ActiveConnection = Nothing
        cnnSQL.Close()
        cnnSQL = Nothing

        FetchByID = rstSQL

        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.Execute()
        rstSQL = Nothing
        cmdSQL = Nothing
        cnnSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function
    Public Function CleanNumber(ByVal varString) As String
        '-----------------------------------------------------------------------------------
        'Author: Rick "Boom Boom" Segura
        'Date: 02/14/2000
        'Description: This procedure returns a given string of numbers less any number
        '             format and symbols
        'Returns: Null
        '-----------------------------------------------------------------------------------
        'Revision History:
        '
        '-----------------------------------------------------------------------------------
        Dim str As String

        If Not IsDBNull(varString) Then
            ' Remove mask characters for string comparison
            str = varString
            str = Replace(str, "(", "")
            str = Replace(str, ")", "")
            str = Replace(str, "-", "")
            str = Replace(str, "#", "")
            str = Replace(str, "%", "")
            str = Replace(str, "$", "")
            str = Replace(str, " ", "")

            CleanNumber = str
        Else
            CleanNumber = ""
        End If

    End Function
End Class
