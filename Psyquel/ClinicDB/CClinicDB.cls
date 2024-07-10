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

Imports Psyquel.BusinessRules.CoreLibraryIII.dbStuff

'--------------------------------------------------------------------
'Class Name: CClinicDB                                              '
'Date: 12/05/1999                                                   '
'Author: Dave Richkun                                               '
'Description:  MTS object designed to host methods associated with  '
'              data affecting the tblClinic table.                  '
'--------------------------------------------------------------------
' Revisions:                                                        '
'   03/02/2000 Segura:  Removed Fetch and added specific fetch      '
'                       methods                                     '
'--------------------------------------------------------------------

Public Class CClinicDB
    Dim md As New ModCommon
    Dim ds As New db

    Private _ConnectionString As String = String.Empty
    Private Const CLASS_NAME As String = "CClinicDB"
    Private Const TABLE_NAME As String = "tblClinic"


    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property




    Public Function FetchDetail(ByVal lngID As Long, Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/06/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Retrieves a record from the tblClinic table.         '
        'Parameters: lngID - ID of clinic to fetch info for                 '
        '            blnIncludeDisabled - Optional parameter that identifies'
        '               if records flagged as 'Disabled' or 'De-activated'  '
        '               are to be included in the record set. The default   '
        '               value is False.                                     '
        'Returns: Recordset of requested clinics ordered by business name   '
        '--------------------------------------------------------------------

        Dim cnn As New ADODB.Connection
        Dim cmd As New ADODB.Command
        Dim rst As ADODB.Recordset
        Dim strShowDisabled As String
        Dim SQLErrorNum As Long



        rst = New ADODB.Recordset



        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        If blnIncludeDisabled Then
            strShowDisabled = "Y"
        Else
            strShowDisabled = "N"
        End If

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelClinicDetail"
            .CommandType = CommandTypeEnum.adCmdStoredProc

            .Parameters.Append(.CreateParameter("@ClinicID", adInteger, adParamInput, , lngID))
            .Parameters.Append(.CreateParameter("@ShowDisabled", adVarChar, adParamInput, 1, strShowDisabled))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With


        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchDetail = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing



    End Function

    Public Function FetchByGroup(ByVal lngGroupID As Long, Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 02/03/2015                                                   '
        'Author: Duane C Orth                                               '
        'Description:  Retrieves records from the tblClinic table.          '
        'Parameters: blnIncludeDisabled - Optional parameter that identifies'
        '               if records flagged as 'Disabled' or 'De-activated'  '
        '               are to be included in the record set. The default   '
        '               value is False.                                     '
        'Returns: Recordset of requested clinics ordered by Group ID        '
        '--------------------------------------------------------------------

        Dim cnn As New ADODB.Connection
        Dim cmd As New ADODB.Command
        Dim rst As ADODB.Recordset
        Dim strShowDisabled As String
        Dim SQLErrorNum As Long



        rst = New ADODB.Recordset



        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        If blnIncludeDisabled Then
            strShowDisabled = "Y"
        Else
            strShowDisabled = "N"
        End If

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelClinicByGroup"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@GroupID", adInteger, adParamInput, , lngGroupID))
            .Parameters.Append(.CreateParameter("@ShowDisabled", adVarChar, adParamInput, 1, strShowDisabled))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByGroup = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing


    End Function



    Public Function FetchByName(Optional ByVal blnIncludeDisabled As Boolean = False) _
        As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/02/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Retrieves records from the tblClinic table.          '
        'Parameters: blnIncludeDisabled - Optional parameter that identifies'
        '               if records flagged as 'Disabled' or 'De-activated'  '
        '               are to be included in the record set. The default   '
        '               value is False.                                     '
        'Returns: Recordset of requested clinics ordered by business name   '
        '--------------------------------------------------------------------

        Dim cnn As New ADODB.Connection
        Dim cmd As New ADODB.Command
        Dim rst As ADODB.Recordset
        Dim strShowDisabled As String
        Dim SQLErrorNum As Long



        rst = New ADODB.Recordset



        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        If blnIncludeDisabled Then
            strShowDisabled = "Y"
        Else
            strShowDisabled = "N"
        End If

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelClinicByName"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ShowDisabled", adVarChar, adParamInput, 1, strShowDisabled))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByName = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing



    End Function

    Public Function FetchByZip(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/02/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Retrieves records from the tblClinic table.          '
        'Parameters: blnIncludeDisabled - Optional parameter that identifies'
        '               if records flagged as 'Disabled' or 'De-activated'  '
        '               are to be included in the record set. The default   '
        '               value is False.                                     '
        'Returns: Recordset of requested clinics ordered by zip code        '
        '--------------------------------------------------------------------

        Dim cnn As New ADODB.Connection
        Dim cmd As New ADODB.Command
        Dim rst As ADODB.Recordset
        Dim strShowDisabled As String
        Dim SQLErrorNum As Long



        rst = New ADODB.Recordset



        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        If blnIncludeDisabled Then
            strShowDisabled = "Y"
        Else
            strShowDisabled = "N"
        End If

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelClinicByZip"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ShowDisabled", adVarChar, adParamInput, 1, strShowDisabled))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByZip = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing


    End Function

    Public Function FetchByCity(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/02/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Retrieves records from the tblClinic table.          '
        'Parameters: blnIncludeDisabled - Optional parameter that identifies'
        '               if records flagged as 'Disabled' or 'De-activated'  '
        '               are to be included in the record set. The default   '
        '               value is False.                                     '
        'Returns: Recordset of requested clinics ordered by city            '
        '--------------------------------------------------------------------

        Dim cnn As New ADODB.Connection
        Dim cmd As New ADODB.Command
        Dim rst As ADODB.Recordset
        Dim strShowDisabled As String
        Dim SQLErrorNum As Long


        rst = New ADODB.Recordset



        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        If blnIncludeDisabled Then
            strShowDisabled = "Y"
        Else
            strShowDisabled = "N"
        End If

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelClinicByCity"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ShowDisabled", adVarChar, adParamInput, 1, strShowDisabled))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByCity = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing



    End Function

    Public Function FetchByState(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/02/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Retrieves records from the tblClinic table.          '
        'Parameters: blnIncludeDisabled - Optional parameter that identifies'
        '               if records flagged as 'Disabled' or 'De-activated'  '
        '               are to be included in the record set. The default   '
        '               value is False.                                     '
        'Returns: Recordset of requested clinics ordered by state           '
        '--------------------------------------------------------------------

        Dim cnn As New ADODB.Connection
        Dim cmd As New ADODB.Command
        Dim rst As ADODB.Recordset
        Dim strShowDisabled As String
        Dim SQLErrorNum As Long



        rst = New ADODB.Recordset



        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        If blnIncludeDisabled Then
            strShowDisabled = "Y"
        Else
            strShowDisabled = "N"
        End If

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelClinicByZip"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ShowDisabled", adVarChar, adParamInput, 1, strShowDisabled))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByState = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing



    End Function

    Public Function Insert(ByVal strBusinessName As String, ByVal strAddress1 As String, _
                      ByVal strAddress2 As String, ByVal strCity As String, _
                      ByVal strState As String, ByVal strZip As String, _
                      ByVal strPhone As String, ByVal strFax As String, _
                      ByVal strSubmitterID As String, ByVal strPOSCode As String, _
                      ByVal strNPI As String, ByVal strTaxonomy As String, _
                      ByVal strAddedBy As String, ByVal strClinicType As String, _
                      ByVal strClaimType As String, ByVal blnShortageAreaYN As Boolean) As Long
        '--------------------------------------------------------------------
        'Date: 12/05/1999
        'Author: Dave Richkun
        'Description:  Inserts a row into the tblClinic table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspInsClinic"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@BusinessName", adVarChar, adParamInput, 50, strBusinessName))
            .Parameters.Append(.CreateParameter("@Address1", adVarChar, adParamInput, 60, strAddress1))
            .Parameters.Append(.CreateParameter("@Address2", adVarChar, adParamInput, 60, strAddress2))
            .Parameters.Append(.CreateParameter("@City", adVarChar, adParamInput, 40, strCity))
            .Parameters.Append(.CreateParameter("@State", adVarChar, adParamInput, 2, strState))
            .Parameters.Append(.CreateParameter("@Zip", adVarChar, adParamInput, 12, strZip))
            .Parameters.Append(.CreateParameter("@Phone", adVarChar, adParamInput, 15, md.NumbersOnly(strPhone)))
            .Parameters.Append(.CreateParameter("@Fax", adVarChar, adParamInput, 15, md.NumbersOnly(strFax)))
            .Parameters.Append(.CreateParameter("@ClinicType", adVarChar, adParamInput, 1, strClinicType))
            .Parameters.Append(.CreateParameter("@ClaimType", adVarChar, adParamInput, 1, strClaimType))
            .Parameters.Append(.CreateParameter("@SubmitterID", adVarChar, adParamInput, 16, strSubmitterID))
            .Parameters.Append(.CreateParameter("@POSCode", adVarChar, adParamInput, 2, strPOSCode))
            .Parameters.Append(.CreateParameter("@NPI", adVarChar, adParamInput, 10, strNPI))
            .Parameters.Append(.CreateParameter("@Taxonomy", adVarChar, adParamInput, 15, strTaxonomy))
            .Parameters.Append(.CreateParameter("@ShortageAreaYN", adVarChar, adParamInput, 1, IIf(blnShortageAreaYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strAddedBy))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        Insert = cmdSQL.Parameters("@ID").Value



        'Close the connection and free all resources
        cnnSQL.Close()

        cmdSQL = Nothing
        cnnSQL = Nothing



    End Function

    Public Sub Update(ByVal lngID As Long, ByVal strBusinessName As String, _
                      ByVal strAddress1 As String, ByVal strAddress2 As String, _
                      ByVal strCity As String, ByVal strState As String, _
                      ByVal strZip As String, ByVal strPhone As String, _
                      ByVal strFax As String, ByVal strSubmitterID As String, _
                      ByVal strPOSCode As String, ByVal strNPI As String, _
                      ByVal strTaxonomy As String, ByVal strClinicType As String, _
                      ByVal strClaimType As String, ByVal blnShortageAreaYN As Boolean)
        '--------------------------------------------------------------------
        'Date: 12/27/1999
        'Author: Dave Richkun
        'Description:  Updates a row in the tblClinic table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: Null
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdClinic"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ClinicID", adInteger, adParamInput, , lngID))
            .Parameters.Append(.CreateParameter("@BusinessName", adVarChar, adParamInput, 50, strBusinessName))
            .Parameters.Append(.CreateParameter("@Address1", adVarChar, adParamInput, 60, strAddress1))
            .Parameters.Append(.CreateParameter("@Address2", adVarChar, adParamInput, 60, strAddress2))
            .Parameters.Append(.CreateParameter("@City", adVarChar, adParamInput, 40, strCity))
            .Parameters.Append(.CreateParameter("@State", adVarChar, adParamInput, 2, strState))
            .Parameters.Append(.CreateParameter("@Zip", adVarChar, adParamInput, 12, strZip))
            .Parameters.Append(.CreateParameter("@Phone", adVarChar, adParamInput, 15, md.NumbersOnly(strPhone)))
            .Parameters.Append(.CreateParameter("@Fax", adVarChar, adParamInput, 15, md.NumbersOnly(strFax)))
            .Parameters.Append(.CreateParameter("@ClinicType", adVarChar, adParamInput, 1, strClinicType))
            .Parameters.Append(.CreateParameter("@ClaimType", adVarChar, adParamInput, 1, strClaimType))
            .Parameters.Append(.CreateParameter("@SubmitterID", adVarChar, adParamInput, 16, strSubmitterID))
            .Parameters.Append(.CreateParameter("@POSCode", adVarChar, adParamInput, 2, strPOSCode))
            .Parameters.Append(.CreateParameter("@NPI", adVarChar, adParamInput, 10, strNPI))
            .Parameters.Append(.CreateParameter("@Taxonomy", adVarChar, adParamInput, 15, strTaxonomy))
            .Parameters.Append(.CreateParameter("@ShortageAreaYN", adVarChar, adParamInput, 1, IIf(blnShortageAreaYN, "Y", "N")))
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


    Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 12/30/1999
        'Author: Dave Richkun
        'Description:  Flags a row in the tblClinic table marking the row as
        '              deleted or undeleted.
        'Parameters: blnDeleted - Boolean value identifying if the record is to
        '               be deleted (True) or undeleted (False).
        '            lngID - ID of the row in the table whose value will be
        '               updated.
        '            strUserName - Login name of the user responsible for
        '               marking the row as deleted.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim strSQL As String
        Dim blnFlag As String



        If blnDeleted = True Then
            blnFlag = "Y"
            'Prepare the SQL statement
            strSQL = "UPDATE "
            strSQL = strSQL & TABLE_NAME
            strSQL = strSQL & " SET "
            strSQL = strSQL & " fldDisabledYN = '" & blnFlag & "', "
            strSQL = strSQL & " fldDateDisabled = '" & Now & "', "
            strSQL = strSQL & " fldDisabledBy = '" & strUserName & "' "
            strSQL = strSQL & "WHERE "
            strSQL = strSQL & "fldClinicID = " & lngID
        Else
            blnFlag = "N"
            'Prepare the SQL statement
            strSQL = "UPDATE "
            strSQL = strSQL & TABLE_NAME
            strSQL = strSQL & " SET "
            strSQL = strSQL & " fldDisabledYN = '" & blnFlag & "' "
            strSQL = strSQL & "WHERE "
            strSQL = strSQL & "fldClinicID = " & lngID
        End If

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
        cnnSQL.Close()
        cmdSQL = Nothing
        cnnSQL = Nothing



    End Sub


    Public Function Exists(ByVal strBusinessName As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 01/05/2000
        'Author: Dave Richkun
        'Description:  Determines if a Business Name identical to the
        '              strBusiness Name parameter already exists in the table.
        'Parameters: strBusinessName - Business name to be checked
        'Returns: True if the name exists, false otherwise
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
        strSQL = strSQL & "COUNT(*) AS TOTAL "
        strSQL = strSQL & "FROM "
        strSQL = strSQL & TABLE_NAME
        strSQL = strSQL & " WHERE "
        strSQL = strSQL & " fldBusinessName = '" & ds.ParseSQL(strBusinessName) & "' "

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

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function


End Class
