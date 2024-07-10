
'--------------------------------------------------------------------
'Class Name: CCertificationDB                                       '
'Date: 03/02/2000                                                   '
'Author: Rick "Boom Boom" Segura                                    '
'Description:  MTS object designed to host methods associated with  '
'              data affecting the tblCertification table.           '
'--------------------------------------------------------------------
' Revision History:
'  R001 02/14/2001 Richkun: Added FetchPatientUserCertDetails()
'  R002 03/21/2001 Segura: Added FetchPatientUserArchivedCertDetails()
'  R003 10/17/2001 Richkun: Added FetchPatientCertsDetailByPatProvCPT()
'  R004 02/06/2002 Richkun: Extended fldCertNumber from 20 to 25 characters
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
' Lydia Orth 


Public Class CCertificationDB

    Private Const CLASS_NAME As String = "CCertificationDB"
    Private Const TABLE_NAME As String = "tblCertification"

    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property

    Public Function Insert(ByVal lngPatID As Long, _
                    ByVal lngPlanID As Long, ByVal lngProviderID As Long, ByVal dteStart As Date, _
                    ByVal dteEnd As Date, ByVal strCertNum As String, _
                    ByVal strAddedBy As String, ByVal strNote As String, _
                    ByVal lngDaysPrior As Long, ByVal lngUnitsPrior As Long) As Long
        '--------------------------------------------------------------------
        'Date: 03/07/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Inserts a single record into the tblCertification    '
        '               table                                               '
        'Parameters: lngPatID -  ID of authorized patient                   '
        '            lngPlanID - ID of authorizing plan                     '
        '            dteStart - cert Start date                             '
        '            dteEnd - cert End Date                                 '
        '            strCertNum - plan assigned certnumber                  '
        '            strAddedBy - name of user entering cert                '
        '            strNotes -  User notes on cert                         '
        '            lngDaysPrior - Days Prior(Hot List)                    '
        '            lngUnitsPrior - Units prior(Hot List)                  '
        'Returns: ID of record just inserted(on success)                    '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection
        Dim lngErrNum As Long
        Dim strErrMsg As String



        cnn = New ADODB.Connection
        cmd = New ADODB.Command

        cnn.Open(_ConnectionString)

        'Create the parameter objects
        With cmd
            .CommandText = "uspInsCertification"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatID))
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@StartDate", adDBTimeStamp, adParamInput, , dteStart))
            .Parameters.Append(.CreateParameter("@EndDate", adDBTimeStamp, adParamInput, , dteEnd))
            .Parameters.Append(.CreateParameter("@CertNumber", adVarChar, adParamInput, 30, strCertNum))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strAddedBy))
            .Parameters.Append(.CreateParameter("@Note", adVarChar, adParamInput, 1000, strNote))
            .Parameters.Append(.CreateParameter("@DaysPrior", adInteger, adParamInput, , lngDaysPrior))
            .Parameters.Append(.CreateParameter("@UnitsPrior", adInteger, adParamInput, , lngUnitsPrior))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        cmd.ActiveConnection = cnn
        cmd.Execute() ', , adExecuteNoRecords

        Insert = cmd.Parameters("@ID").Value

        'Check the ErrorNum parameter before deciding to commit the transaction
        If (cmd.Parameters("@ErrorNum").Value.IsNullorEmpty) Then
            lngErrNum = vbObjectError
            strErrMsg = "Procedure uspInsCertification has failed."
        End If

        If cmd.Parameters("@ErrorNum").Value <> 0 Then
            Exit Function
        End If

        cnn = Nothing
        cmd = Nothing


    End Function

    Public Sub Delete(ByVal lngCertID As Long)
        '--------------------------------------------------------------------
        'Date: 03/09/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Deletes an entire cert (including all associated     '
        '              records in tblCertProvider, tblCertUnit and          '
        '              tblCertCPTcode                                       '
        'Parameters: lngCertID -  ID of cert to be deleted                  '
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        cnn = New ADODB.Connection
        cmd = New ADODB.Command

        cnn.Open(_ConnectionString)
        'Create the parameter objects
        With cmd
            .CommandText = "uspDelCert"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@CertID", adInteger, adParamInput, , lngCertID))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
            'Assign the connection to the Command object and execute the stored procedure
        End With

        cmd.ActiveConnection = cnn
        cmd.Execute() ', , adExecuteNoRecords
        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmd.Parameters("@ErrorNum").Value <> 0 Then
            Exit Sub
        End If

        cnn = Nothing
        cmd = Nothing

    End Sub


    Public Sub Deleted(ByVal strFlag As String, ByVal lngCertID As Long)
        '--------------------------------------------------------------------
        'Date: 03/21/2001                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Sets the disabled flag of a certification row        '
        'Parameters: strFlag - value to set flag to                         '
        '            lngCertID -  ID of cert to be disabled                 '
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        cnn = New ADODB.Connection
        cmd = New ADODB.Command

        cnn.Open(_ConnectionString)

        'Create the parameter objects
        With cmd
            .CommandText = "uspDelCertification"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@CertID", adInteger, adParamInput, , lngCertID))
            .Parameters.Append(.CreateParameter("@Flag", adVarChar, adParamInput, 1, strFlag))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))

            'Assign the connection to the Command object and execute the stored procedure
            .ActiveConnection = cnn
            .Execute() ', , adExecuteNoRecords

            'Check the ErrorNum parameter before deciding to commit the transaction
            If .Parameters("@ErrorNum").Value <> 0 Then
                Exit Sub
            End If

        End With

        cnn = Nothing
        cmd = Nothing


    End Sub

    Public Sub DeleteDetail(ByVal lngCertID As Long)
        '--------------------------------------------------------------------
        'Date: 03/09/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Deletes a cert's dtail information (all associated   '
        '              records in tblCertProvider, tblCertUnit and          '
        '              tblCertCPTcode                                       '
        'Parameters: lngCertID -  ID of cert to delete detail of            '
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection




        cnn = New ADODB.Connection
        cmd = New ADODB.Command

        cnn.Open(_ConnectionString)

        'Create the parameter objects
        With cmd
            .CommandText = "uspDelCertDetail"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@CertID", adInteger, adParamInput, , lngCertID))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))

            'Assign the connection to the Command object and execute the stored procedure
            .ActiveConnection = cnn
            .Execute() ', , adExecuteNoRecords

            'Check the ErrorNum parameter before deciding to commit the transaction
            If .Parameters("@ErrorNum").Value <> 0 Then
                Exit Sub
            End If

        End With

        cnn = Nothing
        cmd = Nothing

    End Sub

    Public Function FetchCertDetail(ByVal lngCertID As Long) As Collection
        '--------------------------------------------------------------------
        'Date: 03/09/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Retrieves recordset containing detailed information  '
        '              of the cert having the ID given as the argument      '
        'Parameters: lngCertID -  ID of cert being described                '
        'Returns: Recordstets of detailed information                       '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection
        Dim rst As ADODB.Recordset
        Dim colRst As New Collection
        Dim intErrorType As Integer
        Dim intCtr As Integer

        intErrorType = 0



        cnn = New ADODB.Connection
        cmd = New ADODB.Command
        rst = New ADODB.Recordset

        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelCertDetail"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@CertID", adInteger, adParamInput, , lngCertID))
        End With

        '  Instantiate recordset and populate
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        'Now test each recordset and populate collection
        For intCtr = 1 To 4
            If rst Is Nothing Then
                intErrorType = 1
                Exit Function
            Else
                colRst.Add(rst, CStr(intCtr))
                rst = rst.NextRecordset
            End If
        Next

        'Set rst = Nothing
        'cnn.Close
        cnn = Nothing

        FetchCertDetail = colRst

        cmd = Nothing


    End Function

    Public Sub Update(ByVal lngCertID As Long, ByVal lngPatientID As Long, _
                    ByVal lngPlanID As Long, ByVal lngProviderID As Long, ByVal dteStart As Date, _
                    ByVal dteEnd As Date, ByVal strCertNum As String, _
                    ByVal strNote As String, ByVal strAddedBy As String, _
                    ByVal lngDaysPrior As Long, ByVal lngUnitsPrior As Long)
        '--------------------------------------------------------------------
        'Date: 03/10/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Updates a single record into the tblCertification    '
        '               table                                               '
        'Parameters: lngCertID -  ID of cert to update                      '
        '            lngPatID -  ID of authorized patient                   '
        '            lngPlanID - ID of authorizing plan                     '
        '            dteStart - cert Start date                             '
        '            dteEnd - cert End Date                                 '
        '            strCertNum - plan assigned certnumber                  '
        '            strAddedBy - name of user entering cert                '
        '            strNotes -  User notes on cert                         '
        '            lngDaysPrior - Days Prior(Hot List)                    '
        '            lngUnitsPrior - Units prior(Hot List)                  '
        'Returns: NULL                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection
        Dim lngErrNum As Long
        Dim strErrMsg As String



        cnn = New ADODB.Connection
        cmd = New ADODB.Command

        cnn.Open(_ConnectionString)

        'Create the parameter objects
        With cmd
            .CommandText = "uspUpdCertification"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@CertID", adInteger, adParamInput, , lngCertID))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@StartDate", adDBTimeStamp, adParamInput, , dteStart))
            .Parameters.Append(.CreateParameter("@EndDate", adDBTimeStamp, adParamInput, , dteEnd))
            .Parameters.Append(.CreateParameter("@CertNumber", adVarChar, adParamInput, 30, strCertNum))
            .Parameters.Append(.CreateParameter("@Note", adVarChar, adParamInput, 1000, strNote))
            .Parameters.Append(.CreateParameter("@DaysPrior", adInteger, adParamInput, , lngDaysPrior))
            .Parameters.Append(.CreateParameter("@UnitsPrior", adInteger, adParamInput, , lngUnitsPrior))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strAddedBy))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        cmd.ActiveConnection = cnn
        cmd.Execute() ', , adExecuteNoRecords

        'Check the ErrorNum parameter before deciding to commit the transaction
        If (cmd.Parameters("@ErrorNum").Value.IsNullorEmpty) Then
            lngErrNum = Err.Number
            strErrMsg = "Procedure uspUpdCertification has failed."
            Exit Sub
        End If

        If cmd.Parameters("@ErrorNum").Value <> 0 Then
            Exit Sub
        End If

        cnn = Nothing
        cmd = Nothing

    End Sub

    Public Function FetchPatientCerts(ByVal lngPatID As Long, _
                                      ByVal blnShowDisabled As Boolean) _
                                      As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/09/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Retrieves recordset of certs for a particular patient'
        'Parameters: lngPatID - ID of Patient to fetch certs for            '
        '            blnShowDisabled - flag designated whether or not to    '
        '               inactive certs                                      '
        'Returns: Recordstets of detailed information                       '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection
        Dim rst As ADODB.Recordset




        cnn = New ADODB.Connection
        cmd = New ADODB.Command
        rst = New ADODB.Recordset

        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelPatientCerts"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatID", adInteger, adParamInput, , lngPatID))
        End With

        '  Instantiate recordset and populate
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchPatientCerts = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing



    End Function


    Public Function FetchHotListByManager(ByVal lngManagerID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 02/16/2001                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Retrieves the Certification Hot List for a given     '
        '             Manager ID                                           '
        'Parameters: lngProviderID - ID of Provider                         '
        'Returns: Recordsets of detailed information                        '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection
        Dim rst As ADODB.Recordset



        cnn = New ADODB.Connection
        cmd = New ADODB.Command
        rst = New ADODB.Recordset

        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelCertHotListByManager"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProvID", adInteger, adParamInput, , lngManagerID))
        End With

        '  Instantiate recordset and populate
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchHotListByManager = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing



    End Function

    Public Function FetchHotListByProvider(ByVal lngProviderID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 02/15/2001                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Retrieves the Certification Hot List for a given     '
        '             Provider ID                                           '
        'Parameters: lngProviderID - ID of Provider                         '
        'Returns: Recordsets of detailed information                        '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection
        Dim rst As ADODB.Recordset



        cnn = New ADODB.Connection
        cmd = New ADODB.Command
        rst = New ADODB.Recordset

        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelCertHotListByProvider"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProvID", adInteger, adParamInput, , lngProviderID))
        End With

        '  Instantiate recordset and populate
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchHotListByProvider = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing



    End Function

    Public Function FetchPatientCertsDetail(ByVal lngPatID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/21/2000                                                   '
        'Author: Dave Richkun
        'Description:  Retrieves recordset of certs for a particular patient
        '              Includes associated providers, and CPT Codes/units.
        'Parameters: lngPatID - ID of Patient to fetch certs for            '
        'Returns: Recordset of detailed cert information                       '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection
        Dim rst As ADODB.Recordset




        cnn = New ADODB.Connection
        cmd = New ADODB.Command
        rst = New ADODB.Recordset

        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelPatientCertsDetail"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatID", adInteger, adParamInput, , lngPatID))
        End With

        '  Instantiate recordset and populate
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        rst.ActiveConnection = Nothing
        cnn.Close()
        cnn = Nothing
        cmd = Nothing

        FetchPatientCertsDetail = rst



    End Function


    Public Function GetCertID(ByVal lngPatID As Long, _
                             ByVal lngPlanID As Long, _
                             ByVal lngProviderID As Long, _
                             ByVal dteDate As Date, _
                             ByVal strCPTCode As String) As Long
        '--------------------------------------------------------------------
        'Date: 04/10/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description: Determines if a DOS is covered by an authorization of '
        '             specific plan                                         '
        'Parameters:  lngPatID - Patient ID                                 '
        '             lngPlanID  -Plan ID                                   '
        '             lngProviderID - ProviderID                            '
        '             lngDate - Date of service                             '
        '             strCPTCode - CPT Code                                 '
        'Returns: The ID of the authorization that covers the DOS           '
        '         -1 otherwise                                              '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        cnn = New ADODB.Connection
        cmd = New ADODB.Command

        cnn.Open(_ConnectionString)

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelCertID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@CertID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@DOS", adDBTimeStamp, adParamInput, , dteDate))
            .Parameters.Append(.CreateParameter("@CPTCode", adVarChar, adParamInput, 10, strCPTCode))
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID))

            'Assign the connection to the Command object and execute the stored procedure
            .ActiveConnection = cnn
            .Execute() ', , adExecuteNoRecords

            GetCertID = .Parameters("@CertID").Value
            GetCertID = IIf(GetCertID > 0, GetCertID, -1)
        End With

        cmd = Nothing
        cnn = Nothing

    End Function

    Public Function GetCertNumber(ByVal lngCertID As Long) As String
        '--------------------------------------------------------------------
        'Date: 06/27/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description: Returns the Cert Number associated with a cert ID     '
        'Parameters:  lngCertID - ID of cert being sought                   '
        'Returns: Cert Number if entered, Empty string otherwise            '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------


        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim strSQL As String
        Dim rstSQL As ADODB.Recordset
        Dim varNum As Object





        strSQL = "SELECT "
        strSQL = strSQL & " fldCertNumber "
        strSQL = strSQL & " FROM "
        strSQL = strSQL & " tblCertification "
        strSQL = strSQL & " WHERE "
        strSQL = strSQL & " fldCertID = " & lngCertID


        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        cmdSQL.CommandText = strSQL
        cmdSQL.CommandType = adCmdText

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Instantiate and populate the Recordset.
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(strSQL, cnnSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)

        varNum = rstSQL.Fields("fldCertNumber").Value
        If varNum.IsNullorEmpty Then varNum = ""
        GetCertNumber = CStr(varNum)

        'Close the connection and free all resources
        rstSQL.ActiveConnection = Nothing
        rstSQL = Nothing
        cnnSQL.Close()
        cmdSQL = Nothing
        cnnSQL = Nothing


    End Function

    Public Function GetCertPlanName(ByVal lngPlanID As Long) As String
        '--------------------------------------------------------------------
        'Date: 06/27/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description: Returns the Cert Plan Name associated with a cert ID  '
        'Parameters:  lngPlanID - ID of plan being sought                   '
        'Returns: Plan name                                                 '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------


        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim strSQL As String
        Dim rstSQL As ADODB.Recordset
        Dim varName As Object





        strSQL = "SELECT "
        strSQL = strSQL & " fldPlanName "
        strSQL = strSQL & " FROM "
        strSQL = strSQL & " tblPlan "
        strSQL = strSQL & " WHERE "
        strSQL = strSQL & " fldPlanID = " & lngPlanID


        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        cmdSQL.CommandText = strSQL
        cmdSQL.CommandType = adCmdText

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Instantiate and populate the Recordset.
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(strSQL, cnnSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)

        varName = rstSQL.Fields("fldPlanName").Value
        If varName.IsNullorEmpty Then varName = ""
        GetCertPlanName = CStr(varName)

        'Close the connection and free all resources
        rstSQL.ActiveConnection = Nothing
        rstSQL = Nothing
        cnnSQL.Close()
        cmdSQL = Nothing
        cnnSQL = Nothing


    End Function

    '-- Begin R001 ---
    Public Function FetchPatientUserCertsDetail(ByVal lngPatientID As Long, _
                    ByVal lngUserID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 02/14/2001
        'Author: Dave Richkun
        'Description:  Returns certifications associated with a specific patient
        '              and within the context of the logged in user.  This method
        '              takes into consideraton whether the patient in question is
        '              shared between Providers.  If a patient is shared then
        '              certification records for all shared Providers
        '              are returned.  If the patient is not shared, then only records
        '              associated with the current user are returned.
        'Parameters: lngPatientID - ID of Patient to retrieve certs for            '
        '            lngUserID - ID of the user making the  to this method.
        'Returns: Recordset of detailed cert information                       '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection
        Dim rst As ADODB.Recordset



        cnn = New ADODB.Connection
        cmd = New ADODB.Command
        rst = New ADODB.Recordset

        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelPatientUserCertsDetail"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
        End With

        '  Instantiate recordset and populate
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        rst.ActiveConnection = Nothing
        cnn.Close()
        cnn = Nothing
        cmd = Nothing

        FetchPatientUserCertsDetail = rst


    End Function
    '-- End R001 ---

    '-- Begin R002 ---
    Public Function FetchPatientUserArchivedCertsDetail(ByVal lngPatientID As Long, _
                    ByVal lngUserID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/22/2001                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Returns archived certifications associated with a specific patient
        '              and within the context of the logged in user.  This method
        '              takes into consideraton whether the patient in question is
        '              shared between Providers.  If a patient is shared then
        '              certification records for all shared Providers
        '              are returned.  If the patient is not shared, then only records
        '              associated with the current user are returned.
        'Parameters: lngPatientID - ID of Patient to retrieve certs for            '
        '            lngUserID - ID of the user making the  to this method.
        'Returns: Recordset of detailed cert information                       '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection
        Dim rst As ADODB.Recordset



        cnn = New ADODB.Connection
        cmd = New ADODB.Command
        rst = New ADODB.Recordset

        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelPatientUserArchivedCertsDetail"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
        End With

        '  Instantiate recordset and populate
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        rst.ActiveConnection = Nothing
        cnn.Close()
        cnn = Nothing
        cmd = Nothing

        FetchPatientUserArchivedCertsDetail = rst



    End Function
    '-- End R002 ---

    Public Sub RemoveFromHotList(ByVal lngCertID As Long)
        '--------------------------------------------------------------------
        'Date: 07/16/2001                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Sets the fldUnitsPrior and fldDaysPrior filed values '
        '              equal to 0, effectively removing the cert from the   '
        '              Hot List.                                            '
        'Parameters: lngCertID - ID of the cert to remove                   '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        cnn = New ADODB.Connection
        cmd = New ADODB.Command

        cnn.Open(_ConnectionString)

        'Create the parameter objects
        With cmd
            .CommandText = "uspUpdRemoveCertFromHotList"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@CertID", adInteger, adParamInput, , lngCertID))
            'Assign the connection to the Command object and execute the stored procedure
            .ActiveConnection = cnn
            .Execute()

        End With

        cnn = Nothing
        cmd = Nothing


    End Sub


    Public Function FetchPatientCertsDetailByPatProvCPT(ByVal lngPatientID As Long, _
            ByVal lngProviderID As Long, ByVal strCPTCode As String) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/17/2001
        'Author: Dave Richkun
        'Description:  Retrieves a recordset containing detailed certification
        '              information belonging associated with a patient, provider,
        '              CPT code combination.  This function was specifiy
        '              designed to assist in the Cerification check when making
        '              patient appointments
        'Parameters: lngPatientID - ID of patient for whom appointment was made
        '            lngProviderID - ID of Provider with whom appointment was made
        '            strCPTCode - CPT Code with which appointment was made
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection
        Dim rst As ADODB.Recordset



        cnn = New ADODB.Connection
        cmd = New ADODB.Command
        rst = New ADODB.Recordset

        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelCertDetailByPatProvCPT"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@CPTCode", adVarChar, adParamInput, 10, strCPTCode))
        End With

        'Instantiate recordset and populate
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        rst.ActiveConnection = Nothing
        cnn.Close()
        cnn = Nothing
        cmd = Nothing

        FetchPatientCertsDetailByPatProvCPT = rst


    End Function


    Public Function FetchQualifyingCerts(ByVal lngPatientID As Long, ByVal lngProviderID As Long, _
                            ByVal lngPlanID As Long, ByVal dtDOS As Date, _
                            ByVal strCPTCode As String, ByVal dblUnits As Double) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/09/2002
        'Author: Dave Richkun
        'Description:  Retrieves active qualifying certifications.  This procedure is
        '              designed to retrieve certifications that will have certification
        '              units deducted from them resulting from service provided.
        '              The certifications returned match encounter-related information
        '              passed in the parameter set.
        'Parameters: lngPatientID - ID of patient associated with certifications
        '            lngProviderID - ID of provider owning certification
        '            dtDOS - Date on which provider serviced patient
        '            strCPTCode - CPT Code associated with service
        '            dblUnits - The number of units applied on the date of service
        'Returns: Recordset of qualifying certifications
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection
        Dim rst As ADODB.Recordset



        cnn = New ADODB.Connection
        cmd = New ADODB.Command
        rst = New ADODB.Recordset

        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelQualifyingCerts"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID))
            .Parameters.Append(.CreateParameter("@DOS", adDBTimeStamp, adParamInput, , dtDOS))
            .Parameters.Append(.CreateParameter("@CPTCode", adVarChar, adParamInput, 10, strCPTCode))
            .Parameters.Append(.CreateParameter("@Units", adDouble, adParamInput, , dblUnits))
        End With

        'Instantiate recordset and populate
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchQualifyingCerts = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing



    End Function


    Public Sub DeductUnits(ByVal lngEncounterLogID As Long, ByVal lngPatientID As Long, _
                ByVal lngProviderID As Long, ByVal lngPlanID As Long, ByVal dtDOS As Date, _
                ByVal strCPTCode As String, ByVal dblUnits As Double)
        '--------------------------------------------------------------------
        'Date: 07/09/2002
        'Author: Dave Richkun
        'Description: Deducts certification units from all active patient certifications
        '             based on matching criteria
        'Parameters: lngEncounterLogID - ID of EncounterLog consuming certification units
        '            lngPatientID - ID of patient owning certifications and for whom service was performed
        '            lngProviderID - ID of servicing provider
        '            lngPlanID - ID of plan associated with certification
        '            dtDOS - Date the service was performed
        '            strCPTCode - CPT Code associated with the service
        '            dblUnits - Number of units applied at the time of service
        'Returns: Null
        '--------------------------------------------------------------------

        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        cmd = New ADODB.Command

        'Create the parameter objects
        With cmd
            .CommandText = "uspUpdDeductCertUnits"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ELID", adInteger, adParamInput, , lngEncounterLogID))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID))
            .Parameters.Append(.CreateParameter("@DOS", adDBTimeStamp, adParamInput, , dtDOS))
            .Parameters.Append(.CreateParameter("@CPTCode", adVarChar, adParamInput, 10, strCPTCode))
            .Parameters.Append(.CreateParameter("@Units", adDouble, adParamInput, , dblUnits))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        cnn = New ADODB.Connection
        cnn.Open(_ConnectionString)

        cmd.ActiveConnection = cnn
        cmd.Execute() ', , adExecuteNoRecords

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        cnn = Nothing
        cmd = Nothing

    End Sub


    Public Function FetchUnCertedPlans(ByVal lngPatientID As Long, ByVal lngProviderID As Long, _
                    ByVal lngPlanID As Long, ByVal dtDOS As Date, _
                    ByVal strCPTCode As String, ByVal dblUnits As Double) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/16/2002
        'Author: Dave Richkun
        'Description:  Retrieves a list of patient plans that require certification but
        '            do not have a matching certification matching Date of Service
        '            parameters i.e. show all patient plans that do not match qualifying
        '            certifications.
        'Parameters: lngPatientID - ID of patient associated with certifications/DOS
        '            lngProviderID - ID of provider owning certification
        '            dtDOS - Date on which provider serviced patient
        '            strCPTCode - CPT Code associated with service
        '            dblUnits - The number of units applied on the date of service
        'Returns: Recordset of qualifying certifications
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection
        Dim rst As ADODB.Recordset



        cnn = New ADODB.Connection
        cmd = New ADODB.Command
        rst = New ADODB.Recordset

        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelNoCert"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID))
            .Parameters.Append(.CreateParameter("@DOS", adDBTimeStamp, adParamInput, , dtDOS))
            .Parameters.Append(.CreateParameter("@CPTCode", adVarChar, adParamInput, 10, strCPTCode))
            .Parameters.Append(.CreateParameter("@Units", adDouble, adParamInput, , dblUnits))
        End With

        'Instantiate recordset and populate
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchUnCertedPlans = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing



    End Function


End Class
