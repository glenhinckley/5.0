Option Strict Off
Option Explicit On
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CDSM_IVDB
    '--------------------------------------------------------------------
    'Class Name: CDSM_IVDB                                              '
    'Date: 02/24/2000                                                   '
    'Author: Rick "Boom Boom" Segura                                    '
    'Description:  MTS object designed to retrieve records from the     '
    '              tblDSMIV table.                                      '
    '--------------------------------------------------------------------
    ' Revision History:                                                 '
    '  R001 08/08/2000: Richkun - Added GetID() method                      '
    '  R002 08/22/2000 RS:  Made connection improvements and leak checks    '
    '  R003 03/10/2003 Richkun: Added Update(), Disable() methods
    '--------------------------------------------------------------------
    Private _ConnectionString As String = String.Empty


    Private Const TABLE_NAME As String = "tblDSM_IV"
    Private Const CLASS_NAME As String = "CDSM_IVDB"

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property


    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False, Optional ByVal strUserWhere As String = "", Optional ByVal strOrderBy As String = "", Optional ByVal lngProviderID As Integer = 0) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 02/24/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Retrieves records from the tblDSM_IV table.          '
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
            .CommandText = "uspSelDSMIVParam"
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
            .Parameters.Append(.CreateParameter("@IncludeDisabled", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnIncludeDisabled, "Y", "N")))
            .Parameters.Append(.CreateParameter("@Where", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 250, strUserWhere))
            .Parameters.Append(.CreateParameter("@Order", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 25, strOrderBy))
            'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
            '.Parameters.Append(.CreateParameter("@ProviderID", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamInput,  , IIf(lngProviderID Is System.DBNull.Value, 0, lngProviderID)))
        End With

        'Acquire the database connection.
        cnn = New ADODB.Connection
        cnn.Open((_ConnectionString))
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

    Public Function FetchByID(ByVal lngDSM_IVID As Integer) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/25/2000
        'Author: Dave Nichol
        'Description:  Returns a recordset for a specific DSM-IV ID, including
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
            .CommandText = "uspSelDSM_IVByID"
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
            .Parameters.Append(.CreateParameter("@DSM_IVID", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamInput, , lngDSM_IVID))
        End With

        'Acquire the database connection.
        cnn = New ADODB.Connection
        cnn.Open((_ConnectionString))
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

    Public Function Insert(ByVal strICD9Code As String, ByVal strICD10Code As String, ByVal strDescription As String, ByVal blnColumnHeader As Boolean, ByVal blnAxisI As Boolean, ByVal blnAxisII As Boolean) As Integer
        '--------------------------------------------------------------------
        'Date: 03/14/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Inserts a row into the tblDSM_IV table utilizing     '
        '              a stored procedure.                                  '
        'Parameters: strDescription - The description of the DSM-IV code    '
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
            .CommandText = "uspInsDSM_IV"
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
            .Parameters.Append(.CreateParameter(, ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamReturnValue))
            .Parameters.Append(.CreateParameter("@ICD9Code", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 6, strICD9Code))
            .Parameters.Append(.CreateParameter("@ICD10Code", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 10, strICD10Code))
            .Parameters.Append(.CreateParameter("@Description", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 100, strDescription))
            .Parameters.Append(.CreateParameter("@ColumnHeaderYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnColumnHeader = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@AxisI", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnAxisI = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@AxisII", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnAxisII = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamOutput, , 0))
        End With

        ' Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        ' Assign the connection to the command object
        ' and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute() '()

        '' Check for errors
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
        strSQL = strSQL & " (fldCodeIcd9 = '" & strCode & "' OR"
        strSQL = strSQL & "  fldCodeIcd10 = '" & strCode & "') "

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

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

    Public Function GetID(ByVal strCode As String) As Integer
        '--------------------------------------------------------------------
        'Date: 08/08/2000
        'Author: Dave Richkun
        'Description:  Returns the primary key for a specific DSM-IV code.
        'Parameters: strCode - DSM-IV code whose PK will be retrieved
        'Returns: ID (Primary Key) of matching DSM-IV record if found, -1 if
        '         not found.
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As New ADODB.Recordset
        Dim strSQL As String
        Dim lngID As Integer

        '

        'Prepare the SQL statement
        strSQL = "SELECT "
        strSQL = strSQL & "fldDSM_IV_ID "
        strSQL = strSQL & "FROM "
        strSQL = strSQL & TABLE_NAME
        strSQL = strSQL & " WHERE "
        strSQL = strSQL & " (fldCodeIcd9 = '" & Trim(strCode) & "' OR"
        strSQL = strSQL & "  fldCodeIcd10 = '" & Trim(strCode) & "') "

        'Instantiate the Recordset.
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = ADODB.CursorLocationEnum.adUseClient

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Populate the  recordset
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
        If rstSQL.RecordCount = 0 Then
            lngID = -1
        Else
            lngID = rstSQL.Fields("fldDSM_IV_ID").Value
        End If

        GetID = lngID
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

    Public Function GetVerificationData(ByVal strCode As String) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/11/2000
        'Author: Dave Nichol & Joshua Lockwood
        'Description:  Returns a recordset for a specific DSM-IV code, including
        '               ID of DSM-IV row, axis I/II flags.
        'Parameters: strCode - DSM-IV code whose PK will be retrieved
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
        cmd = New ADODB.Command
        With cmd
            .CommandText = "uspSelDSM_IVByCode"
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
            .Parameters.Append(.CreateParameter("@DSM_IVCode", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 10, strCode))
        End With

        'Acquire the database connection.
        cnn = New ADODB.Connection
        cnn.Open((_ConnectionString))
        cmd.ActiveConnection = cnn

        'Execute the stored procedure
        rst = New ADODB.Recordset
        rst.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        rst.Open(cmd, , ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)

        GetVerificationData = rst

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

    Public Function FetchAllDSMIV(ByVal lngPatientID As Integer) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 11/20/2000                                                   '
        'Author: Chris Dereadt                                              '
        'Description:  Returns a recordset for all DSM-IV ID, description,  '
        '              and axis.                                            '
        'Parameters: lngPatientID - ID of patient to be retrieved           '
        'Returns: ADODB.Recordset                                           '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        'Declare some variables
        Dim cnn As ADODB.Connection
        Dim rst As New ADODB.Recordset
        Dim cmd As ADODB.Command

        '

        'Create the parameter objects
        cmd = New ADODB.Command

        With cmd
            .CommandText = "uspSelPatientDiagnosisByPatient"
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientID", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamInput, , lngPatientID))
        End With

        'Acquire the database connection.
        cnn = New ADODB.Connection
        cnn.Open((_ConnectionString))
        cmd.ActiveConnection = cnn

        'Execute the stored procedure
        rst = New ADODB.Recordset
        rst.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        rst.Open(cmd, , ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)

        FetchAllDSMIV = rst

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


    Public Sub Update(ByVal lngDSMIV_ID As Integer, ByVal strICD9Code As String, ByVal strICD10Code As String, ByVal strDescription As String, ByVal blnColumnHeader As Boolean, ByVal blnAxisIYN As Boolean, ByVal blnAxisIIYN As Boolean)
        '--------------------------------------------------------------------
        'Date: 03/07/2003
        'Author: Dave Richkun
        'Description:  Updates a row in tblDSM_IV utilizing a stored procedure                                            '
        'Parameters: lngDSMIV_ID - ID of diagnosis code to update
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
            .CommandText = "uspUpdDSM_IV"
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
            .Parameters.Append(.CreateParameter("@DSM_IV_ID", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamInput, , lngDSMIV_ID))
            .Parameters.Append(.CreateParameter("@ICD9Code", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 6, strICD9Code))
            .Parameters.Append(.CreateParameter("@ICD10Code", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 10, strICD10Code))
            .Parameters.Append(.CreateParameter("@Description", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 100, strDescription))
            .Parameters.Append(.CreateParameter("@ColumnHeaderYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnColumnHeader = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@AxisIYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnAxisIYN = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@AxisIIYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnAxisIIYN = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamOutput, , 0))
        End With

        ' Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        ' Assign the connection to the command object
        ' and execute the stored procedure
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

    Public Sub Disable(ByVal lngDSMIV_ID As Integer, ByVal blnDisabledYN As Boolean)
        '--------------------------------------------------------------------
        'Date: 03/07/2003
        'Author: Dave Richkun
        'Description:  Disabled/re-enables a row in tblDSM_IV utilizing a stored procedure                                            '
        'Parameters: lngDSMIV_ID - ID of diagnosis code to update
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
            .CommandText = "uspDisableDSM_IV"
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
            .Parameters.Append(.CreateParameter("@DSM_IV_ID", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamInput, , lngDSMIV_ID))
            .Parameters.Append(.CreateParameter("@DisabledYN", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(blnDisabledYN = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamOutput, , 0))
        End With

        ' Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

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