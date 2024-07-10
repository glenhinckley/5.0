Option Strict Off
Option Explicit On
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CProvGroupDB
    '--------------------------------------------------------------------
    'Class Name: CProvGroupDB
    'Date: 03/08/2002
    'Author: Eric Pena
    'Description:  MTS object designed to host methods associated with
    '              data affecting the tblProviderGroup table.
    '--------------------------------------------------------------------
    ' Revision History:                                                 '
    '   08/22/2000 RS:  Made connection improvements and leak checks    '
    '   Private Const CONST_DIRECT_CNN As String = ""
    '--------------------------------------------------------------------
    Public Const CONST_DIRECT_CNN As String = "Provider=SQLOLEDB.1;Password=psy1234!;Persist Security Info=True;User ID=sa;Initial Catalog=PsyquelDirect;Data Source=192.168.4.25"
    Public Const CONST_PROD_CNN As String = "Provider=SQLOLEDB.1;Password=psy1234!;Persist Security Info=True;User ID=sa;Initial Catalog=PsyquelProd;Data Source=192.168.4.25"



    Private Const CLASS_NAME As String = "CProvGroupDB"
    Private Const TABLE_NAME As String = "tblProviderGroup"
    Public Function Insert(ByVal strGroupName As String, ByVal strTIN As String, ByVal strNPI As String, ByVal strTaxonomy As String) As Integer
        '--------------------------------------------------------------------
        'Date: 03/08/2002
        'Author: Eric Pena
        'Description:  Inserts a row into the tblProviderGroup table utilizing
        '              a stored procedure.
        'Parameters: strGroupName - The name of the group
        '              strTIN - TIN that will be used in billing
        'Returns: ID (Primary Key) of the row inserted
        '--------------------------------------------------------------------


        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command





        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspInsProviderGroup"
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamReturnValue))
            .Parameters.Append(.CreateParameter("@GroupName", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 50, strGroupName))
            .Parameters.Append(.CreateParameter("@TaxID", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 30, strTIN))
            .Parameters.Append(.CreateParameter("@NPI", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 10, strNPI))
            .Parameters.Append(.CreateParameter("@Taxonomy", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 15, strTaxonomy))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(CONST_DIRECT_CNN)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute(, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)

        'Check for errors
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Function
        End If

        Insert = cmdSQL.Parameters(0).Value

        'Close the connection and free all resources
        cnnSQL.Close()
        'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmdSQL = Nothing
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing

        'Signal successful completion



    End Function
    Public Sub Update(ByVal lngID As Integer, ByVal strGroupName As String, ByVal strTIN As String, ByVal strNPI As String, ByVal strTaxonomy As String)
        '--------------------------------------------------------------------
        'Date: 03/08/2002
        'Author: Eric Pena
        'Description:  Updates a row into the tblProviderGroup table utilizing
        '              a stored procedure.
        'Parameters:  lngID - ID of the row in the table whose value will be
        '               updated.
        '             strGroupName - The name of the group
        '              strTIN - TIN that will be used in billing
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        With cmdSQL
            .CommandText = "uspUpdProviderGroup"
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamInput, , lngID))
            .Parameters.Append(.CreateParameter("@GroupName", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 50, strGroupName))
            .Parameters.Append(.CreateParameter("@TaxID", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 30, strTIN))
            .Parameters.Append(.CreateParameter("@NPI", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 10, strNPI))
            .Parameters.Append(.CreateParameter("@Taxonomy", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 15, strTaxonomy))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(CONST_DIRECT_CNN)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute(, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)

        'Check for errors
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        'Close the connection and free all resources
        cnnSQL.Close()
        'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmdSQL = Nothing
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing

        'Signal successful completion



    End Sub
    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/08/2002
        'Author: Eric Pena
        'Description:  Retrieves records from the tblServiceGroup table.          '
        'Parameters: blnIncludeDisabled - Optional parameter that identifies'
        '               if records flagged as 'Disabled' or 'De-activated'  '
        '               are to be included in the record set. The default   '
        '               value is False.                                     '
        'Returns: Recordset of requested ServiceGroups                            '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '--------------------------------------------------------------------


        Dim cnn As ADODB.Connection
        Dim cmd As ADODB.Command
        Dim rst As New ADODB.Recordset

        Dim strShowDisabled As String
        Dim SQLErrorNum As Integer



        rst = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection

        'Acquire the database connection.
        cnn.Open((CONST_DIRECT_CNN))
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelProviderGroup"
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ShowDisabled", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnIncludeDisabled, "Y", "N")))
        End With

        'Execute the stored procedure
        rst.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        rst.Open(cmd, , ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)

        Fetch = rst

        'Disconnect the recordset
        'UPGRADE_NOTE: Object cmd.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmd.ActiveConnection = Nothing
        'UPGRADE_NOTE: Object cmd may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmd = Nothing
        'UPGRADE_NOTE: Object rst.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        rst.ActiveConnection = Nothing
        'UPGRADE_NOTE: Object cnn may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnn = Nothing

        'Signal successful completion


    End Function
    Public Sub Deleted(ByVal lngProvGroupID As Integer, ByVal blnDeleted As Boolean, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 03/08/2002
        'Author: Eric Pena
        'Description:  Disables/enables a record in the tblAlert table utilizing
        '              a stored procedure.
        'Parameters: lngProvGroupID -- the ID of the row to be disabled/enabled
        '            blnDeleted -- true/false: whether row should be disabled or
        '                          enabled
        'Returns: Null
        '--------------------------------------------------------------------

        'declare connection and command variables
        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspDelProviderGroup"
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamInput, , lngProvGroupID))
            .Parameters.Append(.CreateParameter("@DisableYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnDeleted = False, "N", "Y")))
            .Parameters.Append(.CreateParameter("@DeletedBy", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@ErrorNum", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(CONST_DIRECT_CNN)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute(, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
            Exit Sub
        End If

        'Close the connection and free all resources
        'UPGRADE_NOTE: Object cmdSQL.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmdSQL.let_ActiveConnection(Nothing) '--R001
        cnnSQL.Close()

        'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmdSQL = Nothing
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing


    End Sub
    Public Function FetchByID(ByVal lngProvGroupID As Integer) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/08/2002
        'Author: Eric Pena
        'Description:  Retrieves all Providers associated with a ProviderGroup
        'Parameters: lngProvGroupID: ID of Provider Group to fetch Providers for
        'Returns: Recordset of requested ProviderGroup Providers
        '--------------------------------------------------------------------
        'Revision History:
        '--------------------------------------------------------------------

        Dim cnn As ADODB.Connection
        Dim cmd As ADODB.Command
        Dim rst As New ADODB.Recordset



        rst = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection

        'Acquire the database connection.
        cnn.Open((CONST_DIRECT_CNN))
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelProvidersByProviderGroup"
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderGroupID", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamInput, , lngProvGroupID))
        End With

        'Execute the stored procedure
        rst.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        rst.Open(cmd, , ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)

        FetchByID = rst

        'Disconnect the recordset
        'UPGRADE_NOTE: Object cmd.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmd.ActiveConnection = Nothing
        'UPGRADE_NOTE: Object cmd may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmd = Nothing
        'UPGRADE_NOTE: Object rst.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        rst.ActiveConnection = Nothing
        'UPGRADE_NOTE: Object cnn may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnn = Nothing

        'Signal successful completion




    End Function
End Class