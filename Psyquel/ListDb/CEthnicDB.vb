Option Strict Off
Option Explicit On
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CEthnicDB
    '--------------------------------------------------------------------
    'Class Name: CEthnicDB
    'Date: 01/18/2000
    'Author: Rick "Boom Boom" Segura
    'Description:  MTS object designed to host methods associated with
    '              data affecting the tblEthnicity table.
    '--------------------------------------------------------------------
    ' Revision History:                                                 '
    '   08/22/2000 RS:  Made connection improvements and leak checks    '
    '--------------------------------------------------------------------
    Public Const CONST_DIRECT_CNN As String = "Provider=SQLOLEDB.1;Password=psy1234!;Persist Security Info=True;User ID=sa;Initial Catalog=PsyquelDirect;Data Source=192.168.4.25"
    Public Const CONST_PROD_CNN As String = "Provider=SQLOLEDB.1;Password=psy1234!;Persist Security Info=True;User ID=sa;Initial Catalog=PsyquelProd;Data Source=192.168.4.25"



    Private Const CLASS_NAME As String = "CEthnicDB"
    Private Const TABLE_NAME As String = "tblEthnicity"

    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False, Optional ByVal strUserWhere As String = "", Optional ByVal strOrderBy As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 01/18/2000
        'Author: Rick "Boom Boom" Segura
        'Description:  Retrieves records from the tblEthnicity table.
        'Parameters:  blnIncludeDisabled - Optional parameter that identifies if
        '               records flagged as 'Disabled' or 'De-activated' are to be
        '               included in the record set. The default value is False.
        '             strWhere - Optional 'Where' clause of the SQL statement by
        '               which records will be filtered.  If the parameter is not
        '               supplied, all records will be retrieved.
        '             strOrderBy - Optional 'Order By' clause in which retrieved
        '               records will be sorted.  The default is set to the
        '               'fldEthnicity' column
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As New ADODB.Recordset
        Dim strSQL As String
        Dim strWhere As String
        Dim blnUserWhere As Boolean

        '



        'Build SQL statement
        strSQL = "SELECT fldEthnicID, fldEthnicity, fldDisabledYN "
        strSQL = strSQL & "FROM " & TABLE_NAME

        blnUserWhere = Trim(strUserWhere) > ""

        ' Build where clause appropriately
        If blnIncludeDisabled Then 'Include disabled records

            If blnUserWhere Then ' Only use user where string
                strWhere = " WHERE " & strUserWhere
            Else ' Empty where string
                strWhere = ""
            End If

        Else 'Filter out disabled records
            strWhere = " Where fldDisabledYN = 'N' "

            If blnUserWhere Then ' Concat user where string to filtering string
                strWhere = strWhere & " AND " & strUserWhere
            Else
                ' Do nothing
            End If

        End If
        strSQL = strSQL & strWhere

        ' Build Order By clause appropriately
        strSQL = strSQL & " ORDER BY "
        If Trim(strOrderBy) > "" Then
            strSQL = strSQL & strOrderBy
        Else
            strSQL = strSQL & "fldEthnicity"
        End If



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


        Exit Function
        ' 
        '		'Signal incompletion and raise error to ing environment
        '		System.EnterpriseServicesxxxxxxxXXXXXX.ContextUtil.SetAbort()
        '		'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cnnSQL = Nothing
        '		'UPGRADE_NOTE: Object rstSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		rstSQL = Nothing
        '		Err.Raise(Err.Number, Err.Source, Err.Description)

    End Function

    Public Function Insert(ByVal strDescription As String) As Integer
        '--------------------------------------------------------------------
        'Date: 01/19/2000
        'Author: Rick "Boom Boom" Segura
        'Description:  Inserts a row into the tblEthnicity table utilizing
        '              a stored procedure.
        'Parameters: strDescription - The description of the Ethnic group
        '              that will be inserted into the table.
        'Returns: ID (Primary Key) of the row inserted
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command

        'On Error XXXXXXXXXXGoTo Err_Trap



        ' Instantiate and prepare command object
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspInsEthnicity"
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
            .Parameters.Append(.CreateParameter(, ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamReturnValue))
            .Parameters.Append(.CreateParameter("@Description", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 20, strDescription))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamOutput, , 0))
        End With

        ' Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(CONST_DIRECT_CNN)

        ' Assign the connection to the command object
        ' and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute(, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)

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
        '		Err.Raise(Err.Number, Err.Source, Err.Description)

    End Function

    Public Sub Update(ByVal lngID As Integer, ByVal strDescription As String)
        '--------------------------------------------------------------------
        'Date: 01/20/2000
        'Author: Rick "Boom Boom" Segura
        'Description:  Updates a row into the tblEthnicity table utilizing
        '              a stored procedure.
        'Parameters:  lngID - ID of the row in the table whose value will be
        '               updated.
        '             strDescription - The Ethnicity description
        '                to which the record will be changed.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        ''Instatiate and prepare the command object
        'On Error XXXXXXXXXXGoTo Err_Trap

        cmdSQL = New ADODB.Command
        With cmdSQL
            .CommandText = "uspUpdEthnicity"
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamInput, , lngID))
            .Parameters.Append(.CreateParameter("@Description", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 20, strDescription))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamOutput, , 0))
        End With

        ' Acquire the database connection
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(CONST_DIRECT_CNN)

        ' Assign the connection to the command object and
        ' execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        '' Check for errors
        'If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
        '	GoTo Err_Trap
        'End If

        ' Close the connection and free all resources
        cnnSQL.Close()
        'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmdSQL = Nothing
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing

        ' Signal successful completion


        Exit Sub
        'Err_Trap: 
        '		'Signal incompletion and raise error to ing environment
        '		System.EnterpriseServicesxxxxxxxXXXXXX.ContextUtil.SetAbort()
        '		'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cnnSQL = Nothing
        '		'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cmdSQL = Nothing
        '		Err.Raise(Err.Number, Err.Source, Err.Description)
    End Sub


    Public Function Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Integer) As Object
        '--------------------------------------------------------------------
        'Date: 01/19/2000
        'Author: Rick "Boom Boom" Segura
        'Description:  Flags a row in the tblEthnicity table, marking the row as
        '              deleted or undeleted.
        'Parameters: blnDeleted - Boolean value identifying if the record is to
        '               be deleted (True) or undeleted (False).
        '            lngID - ID of the row in the table whose value will be
        '               updated.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim strSQL As String
        Dim strFlag As String

        'On Error XXXXXXXXXXGoTo Err_Trap



        If blnDeleted Then
            strFlag = "Y"
        Else
            strFlag = "N"
        End If

        ' Build SQL statement
        strSQL = "UPDATE " & TABLE_NAME & " SET "
        strSQL = strSQL & " fldDisabledYN = '"
        strSQL = strSQL & strFlag & "' WHERE "
        strSQL = strSQL & "fldEthnicID = " & lngID

        ' Instantiate and prepare the command object.
        cmdSQL = New ADODB.Command
        cmdSQL.CommandText = strSQL
        cmdSQL.CommandType = ADODB.CommandTypeEnum.adCmdText

        ' Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(CONST_DIRECT_CNN)

        ' Assign the connection to the command object and
        ' execute the SQL statement.
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute(, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)

        ' Close the connection and free all resources
        cnnSQL.Close()
        'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmdSQL = Nothing
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing

        ' Signal a successful completion

        Exit Function


        'Err_Trap: 
        '		'Signal incompletion and raise error to ing environment
        '		System.EnterpriseServicesxxxxxxxXXXXXX.ContextUtil.SetAbort()
        '		'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cnnSQL = Nothing
        '		'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cmdSQL = Nothing
        '		Err.Raise(Err.Number, Err.Source, Err.Description)

    End Function

    Public Function Exists(ByVal strDescription As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 01/19/2000
        'Author: Rick "Boom Boom" Segura
        'Description:  Determines if an Ethnicty record identical to the
        '              strDescription parameter already exists in the table.
        'Parameters: strDescription - Ethnicity record to be checked
        'Returns: True if the name exists, false otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As New ADODB.Recordset
        Dim strSQL As String
        Dim blnExists As Boolean



        'Prepare SQL statement
        strSQL = "SELECT COUNT(*) AS TOTAL FROM "
        strSQL = strSQL & TABLE_NAME & " WHERE "
        strSQL = strSQL & "fldEthnicity = '" & strDescription & "' "



        ' Instantiate the recordset.
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = ADODB.CursorLocationEnum.adUseClient

        ' Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(CONST_DIRECT_CNN)

        'Populate the recordset
        rstSQL.Open(strSQL, cnnSQL, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText + ADODB.ExecuteOptionEnum.adAsyncFetch)

        ' Disconnect the recordset, close the connection and
        ' return the recordset to the ing environment.
        'UPGRADE_NOTE: Object rstSQL.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        rstSQL.ActiveConnection = Nothing

        ' Close the connection and free all resources.
        cnnSQL.Close()
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing

        ' Get value from the recordset then destroy it.
        If rstSQL.Fields("TOTAL").Value > 0 Then
            Exists = True
        Else
            Exists = False
        End If
        'UPGRADE_NOTE: Object rstSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        rstSQL = Nothing

        ' Signal successful completion


        Exit Function

        'Err_Trap: 
        '		'Signal incompletion and raise error to ing environment
        '		System.EnterpriseServicesxxxxxxxXXXXXX.ContextUtil.SetAbort()
        '		'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		cnnSQL = Nothing
        '		'UPGRADE_NOTE: Object rstSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '		rstSQL = Nothing
        '		Err.Raise(Err.Number, Err.Source, Err.Description)

    End Function
End Class