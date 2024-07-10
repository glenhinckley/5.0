Option Strict Off
Option Explicit On
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CServiceTypeDB
    '--------------------------------------------------------------------
    'Class Name: CServiceTypeDB                                              '
    'Date: 07/18/2018                                                   '
    'Author: Duane C Orth                                               '
    'Description:  MTS object designed to host methods associated with  '
    '              data affecting the tblServiceTypeCode table.                  '
    '--------------------------------------------------------------------
    ' Revision History:                                                 '
    '--------------------------------------------------------------------

    Private Const CLASS_NAME As String = "CServiceTypeDB"
    Private Const TABLE_NAME As String = "tblServiceTypeCode"
    Public Const CONST_DIRECT_CNN As String = "Provider=SQLOLEDB.1;Password=psy1234!;Persist Security Info=True;User ID=sa;Initial Catalog=PsyquelDirect;Data Source=192.168.4.25"
    Public Const CONST_PROD_CNN As String = "Provider=SQLOLEDB.1;Password=psy1234!;Persist Security Info=True;User ID=sa;Initial Catalog=PsyquelProd;Data Source=192.168.4.25"



    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/18/2018                                                   '
        'Author: Duane C Orth                                               '
        'Description:  Retrieves records from the tblServiceTypeCode table.          '
        'Parameters: blnIncludeDisabled - Optional parameter that identifies'
        '               if records flagged as 'Disabled' or 'De-activated'  '
        '               are to be included in the record set. The default   '
        '               value is False.                                     '
        'Returns: Recordset of requested Services                            '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As New ADODB.Recordset
        Dim strSQL As String

        Dim SQLErrorNum As Integer



        'Build SQL statement
        strSQL = "SELECT * "
        strSQL = strSQL & "FROM " & TABLE_NAME

        If Not blnIncludeDisabled Then
            strSQL = strSQL & " Where fldDisabledYN = 'N' "
        End If

        strSQL = strSQL & " ORDER BY fldServiceTypeCode "

        ' Instantiate the recordset
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = ADODB.CursorLocationEnum.adUseClient

        ' Establish connection
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(CONST_DIRECT_CNN)

        'Populate the recordset
        rstSQL.Open(strSQL, cnnSQL, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText + ADODB.ExecuteOptionEnum.adAsyncFetch)

        ' Disconnect the recordset, close the connection and return the
        ' recordset to the ing environment.
        'UPGRADE_NOTE: Object rstSQL.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        rstSQL.ActiveConnection = Nothing

        cnnSQL.Close()
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing
        Fetch = rstSQL

        ' Signal succesful completion



    End Function


    Public Function FetchByID(ByVal lngServiceTypeID As Integer) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/12/2001
        'Author: Duane C Orth                                               '
        'Description:  Retrieves all Providers associated with a Service
        'Parameters: lngSrvGroupID: ID of Service Group to fetch Providers for
        'Returns: Recordset of requested Service Providers
        '--------------------------------------------------------------------
        'Revision History:
        '  R003: Created
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As New ADODB.Recordset
        Dim strSQL As String

        '

        'Build SQL statement
        strSQL = "SELECT * "
        strSQL = strSQL & "FROM " & TABLE_NAME
        strSQL = strSQL & " WHERE fldServiceTypeID = " & lngServiceTypeID

        ' Instantiate the recordset
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = ADODB.CursorLocationEnum.adUseClient

        ' Establish connection
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(CONST_DIRECT_CNN)

        'Populate the recordset
        rstSQL.Open(strSQL, cnnSQL, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText + ADODB.ExecuteOptionEnum.adAsyncFetch)

        ' Disconnect the recordset, close the connection and return the
        ' recordset to the ing environment.
        'UPGRADE_NOTE: Object rstSQL.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        rstSQL.ActiveConnection = Nothing

        cnnSQL.Close()
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing
        FetchByID = rstSQL

        ' Signal succesful completion


        Exit Function

        ' 
        '		'Signal incompletion and raise the error to the ing environment.
        '		System.EnterpriseServicesxxxxxxxXXXXXX.ContextUtil.SetAbort()
        '		'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cnnSQL = Nothing
        '		'UPGRADE_NOTE: Object rstSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		rstSQL = Nothing
        '		Err.Raise(Err.Number, Err.Source, Err.Description)

    End Function


    Public Function FetchByCode(ByVal strServiceTypeCode As String) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/12/2001
        'Author: Duane C Orth                                               '
        'Description:  Retrieves all Providers associated with a Service
        'Parameters: lngSrvGroupID: ID of Service Group to fetch Providers for
        'Returns: Recordset of requested Service Providers
        '--------------------------------------------------------------------
        'Revision History:
        '  R003: Created
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As New ADODB.Recordset
        Dim strSQL As String

        '

        'Build SQL statement
        strSQL = "SELECT * "
        strSQL = strSQL & "FROM " & TABLE_NAME
        strSQL = strSQL & " WHERE fldServiceTypeCode = '" & strServiceTypeCode & "' "

        ' Instantiate the recordset
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = ADODB.CursorLocationEnum.adUseClient

        ' Establish connection
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(CONST_DIRECT_CNN)

        'Populate the recordset
        rstSQL.Open(strSQL, cnnSQL, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText + ADODB.ExecuteOptionEnum.adAsyncFetch)

        ' Disconnect the recordset, close the connection and return the
        ' recordset to the ing environment.
        'UPGRADE_NOTE: Object rstSQL.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        rstSQL.ActiveConnection = Nothing

        cnnSQL.Close()
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing
        FetchByCode = rstSQL

        ' Signal succesful completion


        Exit Function

        ' 
        '		'Signal incompletion and raise the error to the ing environment.
        '		System.EnterpriseServicesxxxxxxxXXXXXX.ContextUtil.SetAbort()
        '		'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cnnSQL = Nothing
        '		'UPGRADE_NOTE: Object rstSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		rstSQL = Nothing
        '		Err.Raise(Err.Number, Err.Source, Err.Description)

    End Function

    Public Function Insert(ByVal strServiceTypeCode As String, ByVal strDescription As String, ByVal blnUse271YN As Boolean, ByVal strAddedBy As String) As Integer
        '--------------------------------------------------------------------
        'Date: 07/18/2018
        'Author: Duane C Orth
        'Description:  Inserts a row into the tblServiceTypeCode table utilizing
        '              stored a stored procedure.
        'Parameters: strDescription - The description of the Service
        '              that will be inserted into the table.
        'Returns: ID (Primary Key) of the row inserted
        '--------------------------------------------------------------------


        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command

        '


        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        cmdSQL.CommandText = "uspInsServiceTypeCode"
        cmdSQL.CommandType = ADODB.CommandTypeEnum.adCmdStoredProc

        With cmdSQL
            .Parameters.Append(.CreateParameter(, ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamReturnValue))
            .Parameters.Append(.CreateParameter("@ServiceTypeCode", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 2, strServiceTypeCode))
            .Parameters.Append(.CreateParameter("@Description", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 50, strDescription))
            .Parameters.Append(.CreateParameter("@Use271YN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnUse271YN = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@AddedBy", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 20, strAddedBy))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(CONST_DIRECT_CNN)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
        '	GoTo ErrTrap
        'End If

        Insert = cmdSQL.Parameters(0).Value

        'Close the connection and free all resources
        cnnSQL.Close()
        'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmdSQL = Nothing
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing

        'Signal successful completion


        Exit Function

        ' 
        '		'Signal incompletion and raise the error to the ing environment.
        '		System.EnterpriseServicesxxxxxxxXXXXXX.ContextUtil.SetAbort()
        '		'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cnnSQL = Nothing
        '		'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cmdSQL = Nothing
        '		 'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME, "")

    End Function


    Public Sub Update(ByVal lngID As Integer, ByVal strServiceTypeCode As String, ByVal strDescription As String, ByVal blnUse271YN As Boolean)
        '--------------------------------------------------------------------
        'Date: 07/18/2018
        'Author: Duane C Orth
        'Description:  Updates a row into the tblServiceTypeCode table utilizing
        '              a stored procedure.
        'Parameters:  lngID - ID of the row in the table whose value will be
        '               updated.
        '             strDescription - The Service status description
        '                to which the record will be changed.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim prmID As ADODB.Parameter
        Dim prmDesc As ADODB.Parameter
        Dim prmDisabled As ADODB.Parameter

        '

        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdServiceTypeCode"
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamInput, , lngID))
            .Parameters.Append(.CreateParameter("@ServiceTypeCode", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 2, strServiceTypeCode))
            .Parameters.Append(.CreateParameter("@Description", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 50, strDescription))
            .Parameters.Append(.CreateParameter("@Use271YN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnUse271YN = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(CONST_DIRECT_CNN)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute(, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)

        'Check for errors
        'If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
        '	GoTo ErrTrap
        'End If

        'Close the connection and free all resources
        cnnSQL.Close()
        'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmdSQL = Nothing
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing

        'Signal successful completion


        Exit Sub

        ' 
        '		'Signal incompletion and raise the error to the ing environment.
        '		System.EnterpriseServicesxxxxxxxXXXXXX.ContextUtil.SetAbort()
        '		'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cnnSQL = Nothing
        '		'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cmdSQL = Nothing
        '		Err.Raise(Err.Number, Err.Source, Err.Description)

    End Sub


    Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Integer, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 07/18/2018
        'Author: Duane C Orth
        'Description:  Flags a row in the tblServiceTypeCodePeriod table marking the row as
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

        '

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
            strSQL = strSQL & "fldServiceTypeID = " & lngID
        Else
            blnFlag = "N"
            'Prepare the SQL statement
            strSQL = "UPDATE "
            strSQL = strSQL & TABLE_NAME
            strSQL = strSQL & " SET "
            strSQL = strSQL & " fldDisabledYN = '" & blnFlag & "' "
            strSQL = strSQL & "WHERE "
            strSQL = strSQL & "fldServiceTypeID = " & lngID
        End If

        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        cmdSQL.CommandText = strSQL
        cmdSQL.CommandType = ADODB.CommandTypeEnum.adCmdText

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(CONST_DIRECT_CNN)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute(, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)

        'Close the connection and free all resources
        cnnSQL.Close()
        'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmdSQL = Nothing
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing

        'Signal successful completion


        Exit Sub

        ' 
        '		'Signal incompletion and raise the error to the ing environment.
        '		System.EnterpriseServicesxxxxxxxXXXXXX.ContextUtil.SetAbort()
        '		'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cnnSQL = Nothing
        '		'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cmdSQL = Nothing
        '		Err.Raise(Err.Number, Err.Source, Err.Description)

    End Sub


    Public Function Exists(ByVal strServiceTypeCode As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 01/05/2000
        'Author: Duane C Orth
        'Description:  Determines if a Service description identical to the
        '              strDescription parameter already exists in the table.
        'Parameters: strDescription - Service name to be checked
        'Returns: True if the name exists, false otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As New ADODB.Recordset
        Dim strSQL As String
        Dim blnExists As Boolean

        '

        'Prepare the SQL statement
        strSQL = "SELECT "
        strSQL = strSQL & "COUNT(*) AS TOTAL "
        strSQL = strSQL & "FROM "
        strSQL = strSQL & TABLE_NAME
        strSQL = strSQL & " WHERE "
        strSQL = strSQL & " fldServiceTypeCode = '" & strServiceTypeCode & "' "

        'Instantiate the Recordset.
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = ADODB.CursorLocationEnum.adUseClient

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(CONST_DIRECT_CNN)

        ' Populate the recordset
        rstSQL.Open(strSQL, cnnSQL, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText + ADODB.ExecuteOptionEnum.adAsyncFetch)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        'UPGRADE_NOTE: Object rstSQL.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        rstSQL.ActiveConnection = Nothing

        'Close the connection and free all resources
        cnnSQL.Close()
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing

        'Get the value from the Recordset then destroy it.
        If rstSQL.Fields("TOTAL").Value > 0 Then
            Exists = True
        Else
            Exists = False
        End If
        'UPGRADE_NOTE: Object rstSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        rstSQL = Nothing

        'Signal successful completion


        Exit Function

        ' 
        '		'Signal incompletion and raise the error to the ing environment.
        '		System.EnterpriseServicesxxxxxxxXXXXXX.ContextUtil.SetAbort()
        '		'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cnnSQL = Nothing
        '		'UPGRADE_NOTE: Object rstSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		rstSQL = Nothing
        '		Err.Raise(Err.Number, Err.Source, Err.Description)
    End Function
End Class