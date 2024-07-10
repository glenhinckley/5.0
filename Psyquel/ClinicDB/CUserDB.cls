
'--------------------------------------------------------------------
'Class Name: CUserDB                                                '
'Date: 01/03/2000                                                   '
'Author: Dave Richkun                                               '
'Description:  MTS object designed to host methods associated with  '
'              data affecting the tblUsers table.                   '
'--------------------------------------------------------------------
'Revision History:                                                  '
' R001 Segura 02/28/2000 Removed 1 role restriction                 '
' R002 Richkun 04/10/2000 Added FetchProvider
' R003 Richkun 06/26/2000 Added FetchMatchingBenefactors
' R004 Richkun 09/04/2000 Added CheckUserBenefactor
' R005 Richkun 10/13/2000 Added FetchArchivedPatients
' R006 Richkun 11/04/2000 Added FetchPatientAndOffMgrProviders
' R007 Richkun 11/06/2000 Added FetchPatientAndOffMgrArchiveHistory
' R008 Lockwood 03/07/2001 Added FetchPatData()
' R009 Lockwood 03/27/2001 Added FetchMatchingArchivedPatients()
' R010 Richkun 04/19/2001 Added FetchNextID()
' R011 Segura 04/24/2001 Added FetchManagersByProvider()
' R012 Richkun 05/07/2002 Added ChangeEmail() method
' R013 Richkun 03/04/2003 Added FetchCounters() method
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
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CUserDB

    Private _ConnectionString As String = String.Empty


    Private Const CLASS_NAME As String = "CUserDB"
    Private Const TABLE_NAME As String = "tblUser"

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property

    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False, Optional ByVal strWhere As String = "", Optional ByVal strOrderBy As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 01/03/2000                                                   '
        'Author: Dave Richkun                                               '
        'Description:  Retrieves records from the tblUsers table.           '
        'Parameters: blnIncludeDisabled - Optional parameter that identifies'
        '               if records flagged as 'Disabled' or 'De-activated'  '
        '               are to be included in the record set. The default   '
        '               value is False.                                     '
        '            strWhere - Optional 'Where' clause of the SQL statement'
        '               by which records will be filtered.  If the parameter'
        '               is not supplied, all records will be retrieved.     '
        '             strOrderBy - Optional 'Order By' clause in which      '
        '               retrieved records will be sorted.  The default is   '
        '               set to the fldUser' column                          '
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim strConcatWhere As String
        Dim strSQL As String


        'Prepare the SQL statement.
        strSQL = "SELECT "
        strSQL = strSQL & TABLE_NAME & ".*, "
        strSQL = strSQL & "tblProvider.fldSubmissionOnlyYN, "
        strSQL = strSQL & "tblProvider.fldProviderID "
        strSQL = strSQL & "FROM "
        strSQL = strSQL & TABLE_NAME & " LEFT OUTER JOIN tblProvider "
        strSQL = strSQL & "ON " & TABLE_NAME & ".fldUserID = tblProvider.fldProviderID"

        'Determine whether to include 'disabled' records'
        If blnIncludeDisabled = False Then
            strConcatWhere = " WHERE " & TABLE_NAME & ".fldDisabledYN = 'N' "
        End If

        'Attach user's where clause if applicable
        If Trim(strWhere) > "" Then
            If strConcatWhere > "" Then
                strSQL = strSQL & strConcatWhere & " AND "
                strSQL = strSQL & strWhere
            Else
                strSQL = strSQL & " WHERE " & strWhere
            End If
        Else
            strSQL = strSQL & strConcatWhere
        End If

        If Trim(strOrderBy) > "" Then
            strSQL = strSQL & " ORDER BY "
            strSQL = strSQL & strOrderBy
        Else
            strSQL = strSQL & " ORDER BY "
            strSQL = strSQL & "fldLastName, fldFirstName"
        End If

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
        Fetch = rstSQL



    End Function


    Public Function FetchDetail(ByVal lngUserID As Long, _
                                ByRef blnIsProvider As Boolean) As ADODB.Recordset   'R004
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

        Dim cnn As New ADODB.Connection
        Dim cmd As New ADODB.Command
        Dim rst As ADODB.Recordset
        Dim strProvider As String




        rst = New ADODB.Recordset

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        If blnIsProvider = True Then
            strProvider = "Y"
        Else
            strProvider = "N"
        End If

        'Prepare stored procedure parameters
        With cmd
            .CommandText = "uspSelUserInfo"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
            .Parameters.Append(.CreateParameter("@IsProvider", adVarChar, adParamInputOutput, 1, strProvider))
        End With

        'Instantiate and populate the Recordset.
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rst.ActiveConnection = Nothing
        cnn.Close()

        If cmd.Parameters("@IsProvider").Value = "Y" Then
            blnIsProvider = True
        Else
            blnIsProvider = False
        End If

        cnn = Nothing
        cmd = Nothing

        FetchDetail = rst


    End Function

    Public Function ValidateLogin(ByVal strUserName As String) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/07/2000
        'Author: Rick "Boom Boom" Segura
        'Description:  Validates a user, ensuring they are allowed access to
        '              the system
        'Parameters: strUserName - user's login to validate
        'Returns: Recordset of user information if allowed in system, Nothing otherwise
        '--------------------------------------------------------------------

        Dim cnn As New ADODB.Connection
        Dim cmd As New ADODB.Command
        Dim rst As ADODB.Recordset
        Dim SQLErrorNum As Long





        rst = New ADODB.Recordset

        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        With cmd
            .CommandType = adCmdStoredProc
            .CommandText = "uspValidateUser"
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)


        'Close the connection and free all resources
        rst.ActiveConnection = Nothing
        cnn.Close()
        cmd = Nothing
        cnn = Nothing

        ValidateLogin = rst


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

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim strSQL As String
        Dim blnExists As Boolean



        'Prepare the SQL statement
        strSQL = "SELECT "
        strSQL = strSQL & "COUNT(*) As TOTAL "
        strSQL = strSQL & "FROM "
        strSQL = strSQL & TABLE_NAME
        strSQL = strSQL & " WHERE "
        strSQL = strSQL & " fldUserName = '" & strUserName & "' "

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

        'Close the connection and free all resources
        cnnSQL.Close()
        cnnSQL = Nothing

        'Get the value from the Recordset then destroy it.
        If rstSQL.Fields("TOTAL").Value > 0 Then
            Exists = True
        Else
            Exists = False
        End If
        rstSQL = Nothing



    End Function


    Public Function FetchProviders(ByVal lngUserID As Long, _
                                Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset   'R002
        '--------------------------------------------------------------------
        'Date: 03/23/2000
        'Author: Dave Richkun
        'Description:  Retrieves a list of providers associated with a specific user
        'Parameters: lngUserID - ID of the user to retrieve provider records for
        '            blnIncludeDisabled - Optional parameter that identifies '
        '               if records flagged as 'Disabled' or 'De-activated'    '
        '               are to be included in the record set. The default     '
        '               value is False.                                       '
        'Returns: ADO Recordset object containing provider information
        '--------------------------------------------------------------------
        'Revision History:
        '--------------------------------------------------------------------

        Dim cnn As New ADODB.Connection
        Dim cmd As New ADODB.Command
        Dim rst As ADODB.Recordset
        Dim strShowDisabled As String



        rst = New ADODB.Recordset

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        If blnIncludeDisabled Then
            strShowDisabled = "Y"
        Else
            strShowDisabled = "N"
        End If

        'Prepare stored procedure parameters
        With cmd
            .CommandText = "uspSelUserProvider"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
            .Parameters.Append(.CreateParameter("@IncludeDisabledYN", adVarChar, adParamInput, 1, strShowDisabled))
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

        FetchProviders = rst



    End Function

    Public Function FetchGroups(ByVal lngUserID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 12/29/2004
        'Author: Duane C Orth
        'Description:  Retrieves a list of Groups associated with a specific user
        'Parameters: lngUserID - ID of the user to retrieve Group records for
        '            blnIncludeDisabled - Optional parameter that identifies '
        '               if records flagged as 'Disabled' or 'De-activated'    '
        '               are to be included in the record set. The default     '
        '               value is False.                                       '
        'Returns: ADO Recordset object containing provider information
        '--------------------------------------------------------------------
        'Revision History:
        '--------------------------------------------------------------------

        Dim cnn As New ADODB.Connection
        Dim cmd As New ADODB.Command
        Dim rst As ADODB.Recordset
        Dim strShowDisabled As String





        rst = New ADODB.Recordset

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Prepare stored procedure parameters
        With cmd
            .CommandText = "uspSelUserGroups"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
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

        FetchGroups = rst


    End Function

    Public Function FetchInsCompanys(ByVal lngUserID As Long, _
                          Optional ByVal strOrderBy As String = "") As ADODB.Recordset   'R002
        '--------------------------------------------------------------------
        'Date: 12/29/2004
        'Author: Duane C Orth
        'Description:  Retrieves a list of Insurance Companys associated with a specific user
        'Parameters: lngUserID - ID of the user to retrieve nsurance Company records for
        '            blnIncludeDisabled - Optional parameter that identifies '
        '               if records flagged as 'Disabled' or 'De-activated'    '
        '               are to be included in the record set. The default     '
        '               value is False.                                       '
        'Returns: ADO Recordset object containing provider information
        '--------------------------------------------------------------------
        'Revision History:
        '--------------------------------------------------------------------

        Dim cnn As New ADODB.Connection
        Dim cmd As New ADODB.Command
        Dim rst As ADODB.Recordset
        Dim strShowDisabled As String





        rst = New ADODB.Recordset

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Prepare stored procedure parameters
        With cmd
            .CommandText = "uspSelUserInsCompanys"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
            .Parameters.Append(.CreateParameter("@OrderBy", adVarChar, adParamInput, 125, strOrderBy))
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

        FetchInsCompanys = rst


    End Function


    Public Function FetchInsCompanysByGroup(ByVal lngGroupID As Long, Optional ByVal strOrderBy As String = "") As ADODB.Recordset   'R002
        '--------------------------------------------------------------------
        'Date: 12/29/2004
        'Author: Duane C Orth
        'Description:  Retrieves a list of Insurance Companys associated with a specific Group
        'Parameters: lngGroupID - ID of the Group to retrieve nsurance Company records for
        '            blnIncludeDisabled - Optional parameter that identifies '
        '               if records flagged as 'Disabled' or 'De-activated'    '
        '               are to be included in the record set. The default     '
        '               value is False.                                       '
        'Returns: ADO Recordset object containing provider information
        '--------------------------------------------------------------------
        'Revision History:
        '--------------------------------------------------------------------

        Dim cnn As New ADODB.Connection
        Dim cmd As New ADODB.Command
        Dim rst As ADODB.Recordset
        Dim strShowDisabled As String





        rst = New ADODB.Recordset

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Prepare stored procedure parameters
        With cmd
            .CommandText = "uspSelInsCompanysByGroup"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@GroupID", adInteger, adParamInput, , lngGroupID))
            .Parameters.Append(.CreateParameter("@OrderBy", adVarChar, adParamInput, 125, strOrderBy))
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

        FetchInsCompanysByGroup = rst



    End Function
    Public Function FetchPatients(ByVal lngUserID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/23/2000
        'Author: Dave Richkun
        'Description:  Retrieves a list of patients associated with a specific user
        'Parameters: lngUserID - ID of the user to retrieve patient records for
        'Returns: ADO Recordset object containing patient information
        '--------------------------------------------------------------------
        'Revision History:
        '--------------------------------------------------------------------

        Dim cnn As New ADODB.Connection
        Dim cmd As New ADODB.Command
        Dim rst As ADODB.Recordset





        rst = New ADODB.Recordset

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Prepare stored procedure parameters
        With cmd
            .CommandText = "uspSelUserPatients"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
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

        FetchPatients = rst



    End Function

    Public Function FetchPatData(ByVal lngUserID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/07/2001
        'Author: Joshua Lockwood
        'Description:  Gets list of patients by provider along with corresponding
        '           DSMIV codes and Tax IDs.  Is used to support ENCDE2.asp
        '           Added with R008.
        'Parameters: lngUserID - ID of the User to return Patient records for
        'Returns: recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim cnn As New ADODB.Connection
        Dim cmd As New ADODB.Command
        Dim rst As ADODB.Recordset




        rst = New ADODB.Recordset

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Prepare stored procedure parameters
        With cmd
            .CommandText = "uspSelUserPatientData"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
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

        FetchPatData = rst



    End Function


    Public Function FetchArchivedPatients(ByVal lngUserID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/13/2000
        'Author: Dave Richkun
        'Description:  Retrieves a list of archived patients associated with a
        '              specific user
        'Parameters: lngUserID - ID of the user to retrieve records for
        'Returns: ADO Recordset object containing patient information
        '--------------------------------------------------------------------
        'Revision History:
        ' R005 - Created
        '--------------------------------------------------------------------

        Dim cnn As New ADODB.Connection
        Dim cmd As New ADODB.Command
        Dim rst As ADODB.Recordset




        rst = New ADODB.Recordset

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Prepare stored procedure parameters
        With cmd
            .CommandText = "uspSelUserArchivedPatients"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
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

        FetchArchivedPatients = rst


    End Function

    Public Function FetchMatchingArchivedPatients(ByVal lngUserID As Long, ByVal strLastName As String, Optional ByVal strFirstName As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 3/27/2001
        'Author: Joshua Lockwood
        'Description:  Retrieves a list of archived patients associated with a
        '              specific user IAW fetch filter params.
        'Parameters: lngUserID - ID of the user to retrieve records for
        '            strLastName - String for patient(s) last name
        '            strFirstName - String for patient(s) first name
        'Returns: ADO Recordset object containing patient information
        '--------------------------------------------------------------------
        'Revision History:
        '--------------------------------------------------------------------

        Dim cnn As New ADODB.Connection
        Dim cmd As New ADODB.Command
        Dim rst As ADODB.Recordset




        rst = New ADODB.Recordset

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Prepare stored procedure parameters
        With cmd
            .CommandText = "uspSelMatchingArchivedPatients"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
            .Parameters.Append(.CreateParameter("@LastName", adVarChar, adParamInput, 30, strLastName))
            .Parameters.Append(.CreateParameter("@FirstName", adVarChar, adParamInput, 30, strFirstName))
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

        FetchMatchingArchivedPatients = rst







    End Function

    Public Sub UpdateLastPWChange(ByVal strUserName As String, ByVal strNewPassword As String, ByVal dtChanged As Date, Optional ByVal dtExpDate As Date = Nothing)
        '--------------------------------------------------------------------
        'Date: 04/17/2000
        'Author: Dave Richkun
        'Description:  Updates the fldLastPWChange value with the date passed.
        'Parameters: strUserName - Username of the user whose password was changed
        '            dtChanged - The date to record in the fldLastPWChange column
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        'Prepare stored procedure parameters
        With cmdSQL
            .CommandText = "uspUpdUserPWChange"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@Password", adVarChar, adParamInput, 256, strNewPassword))
            .Parameters.Append(.CreateParameter("@DateChanged", adDBTimeStamp, adParamInput, , dtChanged))
            .Parameters.Append(.CreateParameter("@PwdExpDate", adDBTimeStamp, adParamInput, , IIf(dtExpDate.ToOADate() = 0, DBNull.Value, dtExpDate)))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()



        'Close the connection and free all resources
        cnnSQL.Close()

        cmdSQL = Nothing
        cnnSQL = Nothing



    End Sub


    Public Function FetchMatchingBenefactors(ByVal lngUserID As Long, ByVal strLastName As String, Optional ByVal strFirstName As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 04/10/2000
        'Author: Dave Richkun
        'Description:  Retrieves a list of Benefactors (includes both patients and
        '              Responsible Parties) to which a specific user is associated.
        '              If the user is associated with more than one clinic, then a list of
        '              active patients for every clinic in which the user is associated
        '              is returned.
        'Parameters: lngUserID - ID of the User to return Benefactor records for
        '            strLastName - A complete string or substring that will be used to
        '               filter the list of Benefactor records returned by returning only
        '               those records whose last name matches the parameter.
        '            strFirstName - An optional complete string or substring that will be used to
        '               filter the list of Benefactor records returned by returning only
        '               those records whose first name matches the parameter.  If the
        '               FirstName parameter is provided then the Benefactor records will
        '               be filtered using both the Last Name and the First Name.
        'Returns: A recordset containing a list of Patients to which the user is associated.
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim cnn As New ADODB.Connection
        Dim cmd As New ADODB.Command
        Dim rst As ADODB.Recordset
        Dim strError As String





        rst = New ADODB.Recordset

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        If Trim(strLastName) = "" And Trim(strFirstName) = "" Then
            strError = "Both Last Name and First Name values are NULL"
            '''GoTo ErrTrap***************************************************************************************************************************************
        End If

        'Prepare stored procedure parameters
        If Trim(strLastName) > "" And Trim(strFirstName) > "" Then
            With cmd
                .CommandText = "uspSelMatchingUserBenefactorsLNameFName"
                .CommandType = adCmdStoredProc
                .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
                .Parameters.Append(.CreateParameter("@LastName", adVarChar, adParamInput, 30, strLastName))
                .Parameters.Append(.CreateParameter("@FirstName", adVarChar, adParamInput, 30, strFirstName))
            End With
        ElseIf Trim(strLastName) = "" Then
            With cmd
                .CommandText = "uspSelMatchingUserBenefactorsFName"
                .CommandType = adCmdStoredProc
                .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
                .Parameters.Append(.CreateParameter("@FirstName", adVarChar, adParamInput, 30, strFirstName))
            End With
        ElseIf Trim(strFirstName) = "" Then
            With cmd
                .CommandText = "uspSelMatchingUserBenefactorsLName"
                .CommandType = adCmdStoredProc
                .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
                .Parameters.Append(.CreateParameter("@LastName", adVarChar, adParamInput, 30, strLastName))
            End With
        End If

        'Instantiate and populate the Recordset.
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rst.ActiveConnection = Nothing
        cnn.Close()

        cnn = Nothing
        cmd = Nothing

        FetchMatchingBenefactors = rst



    End Function


    Public Function FetchMatchingPatients(ByVal lngUserID As Long, ByVal strLastName As String, Optional ByVal strFirstName As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 04/10/2000
        'Author: Dave Richkun
        'Description:  Retrieves a list of Patients to which a specific user is associated.
        '              If the user is associated with more than one clinic, then a list of
        '              active patients for every clinic in which the user is associated
        '              is returned.
        'Parameters: lngUserID - ID of the User to return Patient records for
        '            strLastName - A complete string or substring that will be used to
        '               filter the list of Patient records returned by returning only
        '               those records whose last name matches the parameter.
        '            strFirstName - An optional complete string or substring that will be used to
        '               filter the list of Patient records returned by returning only
        '               those records whose first name matches the parameter.  If the
        '               FirstName parameter is provided then the Patient records will
        '               be filtered using both the Last Name and the First Name.
        'Returns: A recordset containing a list of Patients to which the user is associated.
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim cnn As New ADODB.Connection
        Dim cmd As New ADODB.Command
        Dim rst As ADODB.Recordset



        rst = New ADODB.Recordset

        'Prepare stored procedure parameters
        With cmd
            .CommandText = "uspSelMatchingUserPatients"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
            .Parameters.Append(.CreateParameter("@LastName", adVarChar, adParamInput, 30, strLastName))
            .Parameters.Append(.CreateParameter("@FirstName", adVarChar, adParamInput, 30, strFirstName))
        End With

        'Acquire the database connection.

        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Instantiate and populate the Recordset.
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rst.ActiveConnection = Nothing
        cnn.Close()

        cnn = Nothing
        cmd = Nothing

        FetchMatchingPatients = rst


    End Function

    Public Function CheckUserBenefactor(ByVal lngUserID As Long, ByVal lngBFactID As Long) As Boolean
        '--------------------------------------------------------------------
        'Date: 09/04/2000
        'Author: Dave Richkun
        'Description:  Checks to ensure that the passed BenefactorID is associated with the
        '              passed UserID.  Used mainly when checking an ASP QueryString variable
        '              and testing to see if the user has manipulated the QueryString parameters.
        'Parameters: lngUserID - ID of the User
        '            lngBFactID - ID of the Benefactor
        'Returns: True if the User-Benefactor association is valid, False otherwise.
        '--------------------------------------------------------------------
        'Revision History:
        ' R004 Created
        '--------------------------------------------------------------------


        Dim cnn As New ADODB.Connection
        Dim cmd As New ADODB.Command
        Dim rst As ADODB.Recordset



        rst = New ADODB.Recordset

        'Prepare stored procedure parameters
        With cmd
            .CommandText = "uspSelUserBenefactor"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
            .Parameters.Append(.CreateParameter("@BFactID", adInteger, adParamInput, , lngBFactID))
        End With

        'Acquire the database connection.

        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Instantiate and populate the Recordset.
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rst.ActiveConnection = Nothing
        cnn.Close()

        cnn = Nothing
        cmd = Nothing

        If rst.RecordCount = 1 Then
            CheckUserBenefactor = True
        Else
            CheckUserBenefactor = False
        End If


    End Function


    Public Function FetchPatientAndOffMgrProviders(ByVal lngUserID As Long, ByVal lngPatientID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 11/04/2000
        'Author: Dave Richkun
        'Description:  Retrieves a list of Providers associated with both a patient and
        '              an Office Manager.
        'Parameters: lngUserID - ID of the Office Manager
        '            lngPatientID - ID of the Patient
        'Returns: A recordset containing matching records.
        '--------------------------------------------------------------------
        'Revision History:
        '  R006 Created
        '--------------------------------------------------------------------



        Dim cnn As New ADODB.Connection
        Dim cmd As New ADODB.Command
        Dim rst As ADODB.Recordset



        rst = New ADODB.Recordset

        'Prepare stored procedure parameters
        With cmd
            .CommandText = "uspSelPatientAndOffMgrProviders"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
        End With

        'Acquire the database connection.

        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Instantiate and populate the Recordset.
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rst.ActiveConnection = Nothing
        cnn.Close()

        cnn = Nothing
        cmd = Nothing

        FetchPatientAndOffMgrProviders = rst


    End Function


    Public Function FetchPatientAndOffMgrArchiveHistory(ByVal lngUserID As Long, ByVal lngPatientID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 11/06/2000
        'Author: Dave Richkun
        'Description:  Retrieves patient/provider archive history for a specific Patient.
        '              Only archive history where both the patient and Ofifce manager share
        '              common Provider associations is retrieved.
        'Parameters: lngUserID - ID of the Office Manager
        '            lngPatientID - ID of the Patient
        'Returns: A recordset containing matching records.
        '--------------------------------------------------------------------
        'Revision History:
        '  R007 Created
        '--------------------------------------------------------------------



        Dim cnn As New ADODB.Connection
        Dim cmd As New ADODB.Command
        Dim rst As ADODB.Recordset



        rst = New ADODB.Recordset

        'Prepare stored procedure parameters
        With cmd
            .CommandText = "uspSelPatientAndOffMgrArchiveHist"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
        End With

        'Acquire the database connection.

        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Instantiate and populate the Recordset.
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rst.ActiveConnection = Nothing
        cnn.Close()

        cnn = Nothing
        cmd = Nothing

        FetchPatientAndOffMgrArchiveHistory = rst


    End Function

    Public Function FetchNextID() As Long 'R010
        '--------------------------------------------------------------------
        'Date: 04/19/2001
        'Author: Dave Richkun                                               '
        'Description:  Retrieves next UserID from the database.
        'Parameters: None
        'Returns: Long integer containing next UserID                                                   '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim strSQL As String



        'Prepare the SQL statement.
        strSQL = "SELECT "
        strSQL = strSQL & "fldNextID "
        strSQL = strSQL & "FROM "
        strSQL = strSQL & "tblID "
        strSQL = strSQL & "WHERE "
        strSQL = strSQL & "fldTableName = 'tblUser'"

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
        FetchNextID = rstSQL.Fields("fldNextID").Value

        rstSQL = Nothing


    End Function

    Public Function FetchManagersByProvider(ByVal lngProviderID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 04/24/2001                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Retrieves a list of Mangers associated with Provider '
        'Parameters: lngProviderID - ID of the Provider                     '
        'Returns: A recordset containing matching records.                  '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '--------------------------------------------------------------------



        Dim cnn As New ADODB.Connection
        Dim cmd As New ADODB.Command
        Dim rst As ADODB.Recordset



        rst = New ADODB.Recordset

        'Prepare stored procedure parameters
        With cmd
            .CommandText = "uspSelOffMgrByProvider"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
        End With

        'Acquire the database connection.

        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Instantiate and populate the Recordset.
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rst.ActiveConnection = Nothing
        cnn.Close()

        cnn = Nothing
        cmd = Nothing

        FetchManagersByProvider = rst



    End Function

    Public Function FetchManagersByGroupID(ByVal lngGroupID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 04/24/2001                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Retrieves a list of Mangers associated with Provider '
        'Parameters: lngProviderID - ID of the Provider                     '
        'Returns: A recordset containing matching records.                  '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '--------------------------------------------------------------------



        Dim cnn As New ADODB.Connection
        Dim cmd As New ADODB.Command
        Dim rst As ADODB.Recordset




        rst = New ADODB.Recordset

        'Prepare stored procedure parameters
        With cmd
            .CommandText = "uspSelOffMgrByGroupID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@GroupID", adInteger, adParamInput, , lngGroupID))
        End With

        'Acquire the database connection.

        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Instantiate and populate the Recordset.
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rst.ActiveConnection = Nothing
        cnn.Close()

        cnn = Nothing
        cmd = Nothing

        FetchManagersByGroupID = rst



    End Function
    Public Sub ChangeEmail(ByVal lngUserID As Long, ByVal strEmail As String)
        '--------------------------------------------------------------------
        'Date: 05/07/2002
        'Author: Dave Richkun
        'Description:  Updates the email address for the given user ID
        'Parameters: lngUserID - ID of User
        '            strEmail - Email address of user
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History: R012
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        'Prepare stored procedure parameters
        With cmdSQL
            .CommandText = "uspUpdUserEmail"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
            .Parameters.Append(.CreateParameter("@Email", adVarChar, adParamInput, 60, strEmail))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()



        'Close the connection and free all resources
        cnnSQL.Close()

        cmdSQL = Nothing
        cnnSQL = Nothing


    End Sub


    Public Function FetchCounters(ByVal lngUserID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/05/2003
        'Author: Dave Richkun
        'Description:  Retrieves a list of counters associated with a specific user
        'Parameters: lngUserID - ID of the user to retrieve counters for
        'Returns: ADO Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '--------------------------------------------------------------------

        Dim cnn As New ADODB.Connection
        Dim cmd As New ADODB.Command
        Dim rst As ADODB.Recordset





        rst = New ADODB.Recordset

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Prepare stored procedure parameters
        With cmd
            .CommandText = "uspSelCounters"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
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

        FetchCounters = rst



    End Function

End Class

