Option Strict Off
Option Explicit On
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CDisableReasonDB
    '--------------------------------------------------------------------
    'Class Name: CDisableReasonDB                                              '
    'Date: 03/17/2022                                                   '
    'Author: Duane C Orth                                    '
    'Description:  MTS object designed to retrieve records from the     '
    '              tblDisableReason table.                                      '
    '--------------------------------------------------------------------
    ' Revision History:                                                 '
    '  R001 08/08/2000: Richkun - Added GetID() method                      '
    '  R002 08/22/2000 RS:  Made connection improvements and leak checks    '
    '  R003 03/10/2003 Richkun: Added Update(), Disable() methods
    '--------------------------------------------------------------------


    Private Const TABLE_NAME As String = "tblDisableReason"
    Private Const CLASS_NAME As String = "CDisableReasonDB"
    Public Const CONST_DIRECT_CNN As String = "Provider=SQLOLEDB.1;Password=psy1234!;Persist Security Info=True;User ID=sa;Initial Catalog=PsyquelDirect;Data Source=192.168.4.25"
    Public Const CONST_PROD_CNN As String = "Provider=SQLOLEDB.1;Password=psy1234!;Persist Security Info=True;User ID=sa;Initial Catalog=PsyquelProd;Data Source=192.168.4.25"


    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/17/2022                                                   '
        'Author: Duane C Orth                                    '
        'Description:  Retrieves records from the tblDisableReason table.          '
        'Parameters: None                                                   '
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cnn As ADODB.Connection
        Dim rst As New ADODB.Recordset
        Dim cmd As ADODB.Command

        '

        'Create the parameter objects
        cmd = New ADODB.Command

        With cmd
            .CommandText = "uspSelDisableReason"
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
            .Parameters.Append(.CreateParameter("@IncludeDisabled", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnIncludeDisabled, "Y", "N")))
        End With

        'Acquire the database connection.
        cnn = New ADODB.Connection
        cnn.Open((CONST_DIRECT_CNN))
        cmd.ActiveConnection = cnn

        'Execute the stored procedure
        rst = New ADODB.Recordset
        rst.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        rst.Open(cmd, , ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)

        Fetch = rst

        'Disconnect the recordset
        'UPGRADE_NOTE: Object cmd.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmd.ActiveConnection = Nothing
        'UPGRADE_NOTE: Object rst.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        rst.ActiveConnection = Nothing
        cnn.Close()
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
        '		cmd.ActiveConnection = Nothing
        '		'UPGRADE_NOTE: Object rst.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		rst.ActiveConnection = Nothing
        '		cnn.Close()
        '		'    Set rst = Nothing
        '		'UPGRADE_NOTE: Object cmd may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cmd = Nothing
        '		'UPGRADE_NOTE: Object cnn may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cnn = Nothing

        '		Err.Raise(Err.Number, Err.Source, Err.Description)

    End Function

    Public Function FetchByID(ByVal lngID As Integer) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/25/2000
        'Author: Dave Nichol
        'Description:  Returns a recordset for a specific Disable Reason ID, including
        '               code, description, and axis I/II flags.
        'Parameters: lngDSM_IVID - ID of row (PK) that will be retrieved
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim cnn As ADODB.Connection
        Dim rst As New ADODB.Recordset
        Dim cmd As ADODB.Command

        '

        'Create the parameter objects
        cmd = New ADODB.Command
        With cmd
            .CommandText = "uspSelDisableReasonByID"
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ReasonID", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamInput, , lngID))
        End With

        'Acquire the database connection.
        cnn = New ADODB.Connection
        cnn.Open((CONST_DIRECT_CNN))
        cmd.ActiveConnection = cnn

        'Execute the stored procedure
        rst = New ADODB.Recordset
        rst.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        rst.Open(cmd, , ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)

        FetchByID = rst

        'Disconnect the recordset
        'UPGRADE_NOTE: Object cmd.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmd.ActiveConnection = Nothing

        'UPGRADE_NOTE: Object rst.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        rst.ActiveConnection = Nothing
        cnn.Close()
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
        '		cmd.ActiveConnection = Nothing
        '		'UPGRADE_NOTE: Object rst.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		rst.ActiveConnection = Nothing
        '		cnn.Close()
        '		'UPGRADE_NOTE: Object rst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		rst = Nothing
        '		'UPGRADE_NOTE: Object cmd may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cmd = Nothing
        '		'UPGRADE_NOTE: Object cnn may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cnn = Nothing

        '		Err.Raise(Err.Number, Err.Source, Err.Description)
    End Function

    Public Function Insert(ByVal strDescription As String, ByVal strType As String) As Integer
        '--------------------------------------------------------------------
        'Date: 03/14/2000                                                   '
        'Author: Duane C Orth                                    '
        'Description:  Inserts a row into the tblDisableReason table utilizing     '
        '              a stored procedure.                                  '
        'Parameters: strDescription - The description of the Disable Reason code    '
        '              that will be inserted into the table.                '
        'Returns: ID (Primary Key) of the row inserted                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command

        'On Error XXXXXXXXXXGoTo Err_Trap

        ' Instantiate and prepare command object
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspInsDisableReason"
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
            .Parameters.Append(.CreateParameter(, ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamReturnValue))
            .Parameters.Append(.CreateParameter("@Description", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 250, strDescription))
            .Parameters.Append(.CreateParameter("@Type", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, strType))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamOutput, , 0))
        End With

        ' Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(CONST_DIRECT_CNN)

        ' Assign the connection to the command object
        ' and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute() '()

        ' Check for errors
        'If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
        '	GoTo Err_Trap
        'End If

        Insert = cmdSQL.Parameters(0).Value

        ' Close the connection an free all resources
        cnnSQL.Close()
        'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmdSQL = Nothing
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing

        ' Signal successful completion


        Exit Function

        'Err_Trap: 
        '		'Signal incompletion and raise error to ing environment
        '		System.EnterpriseServicesxxxxxxxXXXXXX.ContextUtil.SetAbort()
        '		'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cnnSQL = Nothing
        '		'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cmdSQL = Nothing
        '		Insert = -1
        '		 'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME)

    End Function


    Public Function Exists(ByVal strCode As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 05/02/2000
        'Author: Eric Pena
        'Description:  Determines if a DSMIV record identical to the
        '              strCode parameter already exists in the table.
        'Parameters: strDescription - DSMIV record to be checked
        'Returns: True if the code exists, false otherwise
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
        strSQL = strSQL & " (fldDisableReasonDesc = '" & strCode & "') "

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(CONST_DIRECT_CNN)

        'Instantiate and populate the Recordset.
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = ADODB.CursorLocationEnum.adUseClient
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
    Public Sub Update(ByVal lngID As Integer, ByVal strDescription As String, ByVal strType As String)
        '--------------------------------------------------------------------
        'Date: 03/07/2003
        'Author: Dave Richkun
        'Description:  Updates a row in tblDisableReason utilizing a stored procedure                                            '
        'Parameters: lngID - ID of diagnosis code to update
        '            strDSMCode - DSM_IV Code
        '            strDescription - Description of the diagnosis code
        '            blnAxisI - Identifies if diagnosis resides on Axis I
        '            blnAxisII - Identifies if diagnosis resides on Axis II
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command

        '

        'Create the parameter objects
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdDisableReason"
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ReasonID", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamInput, , lngID))
            .Parameters.Append(.CreateParameter("@Description", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 250, strDescription))
            .Parameters.Append(.CreateParameter("@Type", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, strType))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamOutput, , 0))
        End With

        ' Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(CONST_DIRECT_CNN)

        ' Assign the connection to the command object
        ' and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute() '()

        ' Check for errors
        'If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
        '	GoTo ErrTrap
        'End If

        ' Close the connection an free all resources
        cnnSQL.Close()
        'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmdSQL = Nothing
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing

        ' Signal successful completion


        Exit Sub

        ' 
        '		'Signal incompletion and raise error to ing environment
        '		System.EnterpriseServicesxxxxxxxXXXXXX.ContextUtil.SetAbort()
        '		'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cnnSQL = Nothing
        '		'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cmdSQL = Nothing
        '		 'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME)

    End Sub

    Public Sub Disable(ByVal lngID As Integer, ByVal blnDisabledYN As Boolean, ByVal strDisabledBy As String)
        '--------------------------------------------------------------------
        'Date: 03/07/2022
        'Author: Duane C Orth
        'Description:  Disabled a row in tblDisableReason utilizing a stored procedure                                            '
        'Parameters: lngID - ID of diagnosis code to update
        '            blnDisabledYN - Identifies if row is disabled (true) or re-enabled(false)
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command

        '

        'Create the parameter objects
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspDisableDisableReason"
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ReasonID", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamInput, , lngID))
            .Parameters.Append(.CreateParameter("@DisabledYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnDisabledYN = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@DisabledBy", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 30, strDisabledBy))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamOutput, , 0))
        End With

        ' Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(CONST_DIRECT_CNN)

        ' Assign the connection to the command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute() '()

        '' Check for errors
        'If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
        '	GoTo ErrTrap
        'End If

        ' Close the connection an free all resources
        cnnSQL.Close()
        'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmdSQL = Nothing
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing

        ' Signal successful completion


        Exit Sub

        ' 
        '		'Signal incompletion and raise error to ing environment
        '		System.EnterpriseServicesxxxxxxxXXXXXX.ContextUtil.SetAbort()
        '		'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cnnSQL = Nothing
        '		'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cmdSQL = Nothing
        '		 'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME)

    End Sub
End Class