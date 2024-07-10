Option Strict Off
Option Explicit On



Imports ADODB.DataTypeEnum
Imports ADODB.ParameterDirectionEnum
Imports ADODB.ExecuteOptionEnum

Imports ADODB.CursorTypeEnum
Imports ADODB.CursorLocationEnum
Imports ADODB.CursorOptionEnum
Imports ADODB.LockTypeEnum

Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CAlertDB
    '--------------------------------------------------------------------
    'Class Name: CAlertDB
    'Date: 02/24/2000
    'Author: Dave Nichol
    'Description:  MTS object designed to host methods associated with
    '              data affecting the tblAlert  table.
    '--------------------------------------------------------------------
    '  R001 07/06/2000 Richkun: Added FetchByName() method
    '--------------------------------------------------------------------

    Private Const CLASS_NAME As String = "CAlertDB"
    Private Const TABLE_NAME As String = "tblAlert"
    Private Const ALERTTEXT_LEN As Short = 20 'max length of strAlertText


    Public Const CONST_DIRECT_CNN As String = "Provider=SQLOLEDB.1;Password=psy1234!;Persist Security Info=True;User ID=sa;Initial Catalog=PsyquelDirect;Data Source=192.168.4.25"
    Public Const CONST_PROD_CNN As String = "Provider=SQLOLEDB.1;Password=psy1234!;Persist Security Info=True;User ID=sa;Initial Catalog=PsyquelProd;Data Source=192.168.4.25"


    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/11/2000
        'Author: Dave Nichol
        'Description:  Retrieves records from the tblAlert table utilizing
        '              a stored procedure.
        'Parameters: blnIncludeDisabled -- optional parameter (default=False)
        '               determining whether disabled rows should be returned.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        ' --R001 : Joshua Lockwood, 19Sep00 : Modifications made to improve resource handling
        '
        '--------------------------------------------------------------------

        'declare some variables
        Dim cnn As ADODB.Connection
        Dim rst As New ADODB.Recordset
        Dim cmd As ADODB.Command

        '

        'initialize some variables :this part was moved ...
        'BigDave wants instantiation of objs to be closest to area used

        'Create the parameter objects
        cmd = New ADODB.Command 'moved --R001
        With cmd
            .CommandText = "uspSelAlert"
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ShowDisabledYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnIncludeDisabled = False, "N", "Y")))
        End With

        'Acquire the database connection.
        cnn = New ADODB.Connection 'moved --R001
        cnn.Open((CONST_DIRECT_CNN))
        cmd.ActiveConnection = cnn

        'Execute the stored procedure
        rst = New ADODB.Recordset 'moved --R001
        rst.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        rst.Open(cmd, , ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)

        Fetch = rst

        'Disconnect the recordset
        'UPGRADE_NOTE: Object cmd.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmd.ActiveConnection = Nothing
        'UPGRADE_NOTE: Object rst.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        rst.ActiveConnection = Nothing 'moved --R001
        cnn.Close() ' --R001
        'UPGRADE_NOTE: Object cmd may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmd = Nothing
        'UPGRADE_NOTE: Object cnn may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnn = Nothing

        'Signal successful completion


        Exit Function

        ' 
        '		'Signal incompletion and raise the error to the ing environment.
        '		System.EnterpriseServicesxxxxxxxXXXXXX.ContextUtil.SetAbort()

        '		'UPGRADE_NOTE: Object cmd.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cmd.ActiveConnection = Nothing ' --R001
        '		'UPGRADE_NOTE: Object rst.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		rst.ActiveConnection = Nothing ' --R001
        '		cnn.Close() ' --R001
        '		'UPGRADE_NOTE: Object rst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		rst = Nothing
        '		'UPGRADE_NOTE: Object cmd may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cmd = Nothing
        '		'UPGRADE_NOTE: Object cnn may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cnn = Nothing

        '		Err.Raise(Err.Number, Err.Source, Err.Description)
    End Function
    Public Function FetchByID(ByVal lngAlertID As Integer) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/11/2000
        'Author: Dave Nichol
        'Description:  Retrieves a record from the tblAlert table utilizing
        '              a stored procedure by primary key.
        'Parameters: blnIncludeDisabled -- optional parameter (default=False)
        '               determining whether disabled rows should be returned.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        'declare some variables
        Dim cnn As ADODB.Connection
        Dim rst As New ADODB.Recordset
        Dim cmd As ADODB.Command

        '

        'Create the parameter objects
        cmd = New ADODB.Command 'moved --R001
        With cmd
            .CommandText = "uspSelAlertByID"
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
            .Parameters.Append(.CreateParameter("@AlertID", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamInput, , lngAlertID))
        End With

        'Acquire the database connection.
        cnn = New ADODB.Connection 'moved --R001
        cnn.Open((CONST_DIRECT_CNN))
        cmd.ActiveConnection = cnn

        'Execute the stored procedure
        rst = New ADODB.Recordset 'moved --R001
        rst.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        rst.Open(cmd, , ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)

        FetchByID = rst

        'Disconnect the recordset
        'UPGRADE_NOTE: Object cmd.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmd.ActiveConnection = Nothing
        'UPGRADE_NOTE: Object rst.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        rst.ActiveConnection = Nothing 'moved --R001
        cnn.Close() ' --R001
        'UPGRADE_NOTE: Object cmd may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmd = Nothing
        'UPGRADE_NOTE: Object cnn may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnn = Nothing

        'Signal successful completion


        Exit Function

        ' 
        '		'Signal incompletion and raise the error to the ing environment.
        '		System.EnterpriseServicesxxxxxxxXXXXXX.ContextUtil.SetAbort()

        '		'UPGRADE_NOTE: Object cmd.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cmd.ActiveConnection = Nothing ' --R001
        '		'UPGRADE_NOTE: Object rst.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		rst.ActiveConnection = Nothing ' --R001
        '		cnn.Close() ' --R001
        '		'UPGRADE_NOTE: Object rst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		rst = Nothing
        '		'UPGRADE_NOTE: Object cmd may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cmd = Nothing
        '		'UPGRADE_NOTE: Object cnn may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cnn = Nothing

        '		Err.Raise(Err.Number, Err.Source, Err.Description)
    End Function


    Public Function Insert(ByVal strAlertText As String) As Integer
        '--------------------------------------------------------------------
        'Date: 09/11/2000
        'Author: Dave Nichol
        'Description:  Inserts a row into the tblAlert table utilizing
        '              a stored procedure.
        'Parameters: strAlertText -- name/description of the alert
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------

        'Declare connection, command, and counter
        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim intCount As Short

        '

        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspInsAlert"
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
            .Parameters.Append(.CreateParameter("@AlertID", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamReturnValue)) 'R001, this is how to get return params
            .Parameters.Append(.CreateParameter("@AlertText", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, ALERTTEXT_LEN, strAlertText))
            .Parameters.Append(.CreateParameter("@ErrorNum", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(CONST_DIRECT_CNN)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute(, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)

        Insert = cmdSQL.Parameters(0).Value ' --R001 Changed to index(0) to get return param

        ''Check the ErrorNum parameter before deciding to commit the transaction
        'If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
        '	GoTo ErrTrap
        'End If

        'Close the connection
        'UPGRADE_NOTE: Object cmdSQL.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmdSQL.let_ActiveConnection(Nothing) ' --R001
        cnnSQL.Close()

        'free all resources
        'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmdSQL = Nothing
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing

        'Signal successful completion


        Exit Function

        ' 
        '		System.EnterpriseServicesxxxxxxxXXXXXX.ContextUtil.SetAbort() '--R001 moved for consistency

        '		'Signal incompletion and raise the error to the ing environment.
        '		'UPGRADE_NOTE: Object cmdSQL.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cmdSQL.let_ActiveConnection(Nothing) '--R001
        '		cnnSQL.Close() '--R001
        '		'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cmdSQL = Nothing
        '		'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cnnSQL = Nothing

        '		'   'RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME) 'hmmm, not too sure of this
    End Function
    Public Sub Update(ByVal lngAlertID As Integer, ByVal strAlertText As String)
        '--------------------------------------------------------------------
        'Date: 09/11/2000
        'Author: Dave Nichol
        'Description:  Updates a row in the tblAlert  table utilizing
        '              a stored procedure.
        'Parameters: lngAlertID -- the ID of the row to be updated
        '            strAlertText -- the alert text to update
        'Returns: Null
        '--------------------------------------------------------------------

        'declare connection and command variables
        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command

        '

        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdAlert"
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
            .Parameters.Append(.CreateParameter("@AlertID", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamInput, , lngAlertID))
            .Parameters.Append(.CreateParameter("@AlertText", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, ALERTTEXT_LEN, strAlertText))
            .Parameters.Append(.CreateParameter("@ErrorNum", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(CONST_DIRECT_CNN)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute(, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)

        ''Check the ErrorNum parameter before deciding to commit the transaction
        'If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
        '	GoTo ErrTrap
        'End If

        'Close the connection and free all resources
        'UPGRADE_NOTE: Object cmdSQL.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmdSQL.let_ActiveConnection(Nothing) '--R001
        cnnSQL.Close()

        'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmdSQL = Nothing
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing

        'Signal successful completion


        Exit Sub

        ' 
        '		System.EnterpriseServicesxxxxxxxXXXXXX.ContextUtil.SetAbort() 'moved for consistency

        '		'Signal incompletion and raise the error to the ing environment.
        '		'UPGRADE_NOTE: Object cmdSQL.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cmdSQL.let_ActiveConnection(Nothing) '--R001
        '		cnnSQL.Close() '--R001
        '		'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cmdSQL = Nothing
        '		'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cnnSQL = Nothing

        '		'    'RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, cmdSQL.Parameters("@ErrorMsg").Value)
    End Sub
    Public Sub Deleted(ByVal lngAlertID As Integer, ByVal blnDeleted As Boolean)
        '--------------------------------------------------------------------
        'Date: 09/11/2000
        'Author: Dave Nichol
        'Description:  Disables/enables a record in the tblAlert table utilizing
        '              a stored procedure.
        'Parameters: lngAlertID -- the ID of the row to be disabled/enabled
        '            blnDeleted -- true/false: whether row should be disabled or
        '                          enabled
        'Returns: Null
        '--------------------------------------------------------------------

        'declare connection and command variables
        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command

        '

        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspDelAlert"
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
            .Parameters.Append(.CreateParameter("@AlertID", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamInput, , lngAlertID))
            .Parameters.Append(.CreateParameter("@DisableYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnDeleted = False, "N", "Y")))
            .Parameters.Append(.CreateParameter("@ErrorNum", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(CONST_DIRECT_CNN)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute(, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)

        ''Check the ErrorNum parameter before deciding to commit the transaction
        'If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
        '	GoTo ErrTrap
        'End If

        'Close the connection and free all resources
        'UPGRADE_NOTE: Object cmdSQL.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmdSQL.let_ActiveConnection(Nothing) '--R001
        cnnSQL.Close()

        'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmdSQL = Nothing
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing

        'Signal successful completion


        Exit Sub

        ' 
        '		System.EnterpriseServicesxxxxxxxXXXXXX.ContextUtil.SetAbort() 'moved for consistency

        '		'Signal incompletion and raise the error to the ing environment.
        '		'UPGRADE_NOTE: Object cmdSQL.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cmdSQL.let_ActiveConnection(Nothing) '--R001
        '		cnnSQL.Close() '--R001
        '		'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cmdSQL = Nothing
        '		'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cnnSQL = Nothing

        '		'  'RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, cmdSQL.Parameters("@ErrorMsg").Value)
    End Sub
End Class