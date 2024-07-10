
'--------------------------------------------------------------------
'Class Name: CProvFeeDB
'Date: 10/25/2000
'Author: Morgan Travis
'Description:  MTS object designed to host methods associated with
'               data affecting Provider Fee table.
'--------------------------------------------------------------------
'Revisions
'    R000 - Pena: 12/7/2000 - Modified insert and update method s to include auditing
'    R001 - Richkun: 05/08/2001 - Added FetchByID() method
'    R002 - Richkun: 05/08/2001 - Altered Update() to include CPTCode, ProviderID parameter
'    R003 - Richkun: 05/08/2001 - Added Delete() method
'    R004 - Richkun: 05/08/2001 - Altered Exists() method to search for duplicates based on
'                   conditional parameter.  Business rules are slightly different when checking
'                   for duplicates during Insert than when checking for duplicates during Update
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

Public Class CProvFeeDB





    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CProvFeeDB"
    Private Const TABLE_NAME As String = "tblProviderFee"

    Public Function Insert(ByVal lngProviderID As Long, _
                           ByVal strCPTCode As String, ByVal strCPTModifier As String, _
                           ByVal strCPTDescription As String, ByVal curFee As Decimal, _
                           ByVal curContractAmount As Decimal, ByVal blnPatSched As Boolean, _
                           ByVal lngDuration As Long, ByVal lngPlanID As Long, _
                           ByVal strNotes As String, ByVal strAddedBy As String)
        '--------------------------------------------------------------------
        'Date: 10/25/2000
        'Author: Morgan Travis
        'Description:  Inserts a row into the tblProviderFee table utilizing
        '              a stored procedure.
        'Parameters: lngProviderID - Provider ID number to insert into table
        '            strCPTCode - The ten digit CPT Code that will be inserted into the table.
        '            dblFee - The monetary amount that will be inserted into the table.
        '
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        cmdSQL.CommandText = "uspInsProvFee"
        cmdSQL.CommandType = adCmdStoredProc

        With cmdSQL
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@CPTCode", adVarChar, adParamInput, 10, strCPTCode))
            .Parameters.Append(.CreateParameter("@CPTModifier", adVarChar, adParamInput, 2, strCPTModifier)) 'R002
            .Parameters.Append(.CreateParameter("@CPTDescription", adVarChar, adParamInput, 60, strCPTDescription)) 'R002
            .Parameters.Append(.CreateParameter("@Fee", adDecimal, adParamInput, , curFee))
            .Parameters.Append(.CreateParameter("@ContractAmount", adDecimal, adParamInput, , IIf(IsDBNull(curContractAmount), 0, curContractAmount)))
            .Parameters.Append(.CreateParameter("@PatSched", adVarChar, adParamInput, 1, IIf(blnPatSched = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@Duration", adInteger, adParamInput, , lngDuration))
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , IIf(IsDBNull(lngPlanID), 0, lngPlanID)))
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

        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Function
        End If

        Insert = cmdSQL.Parameters(0).Value

        'Close the connection and free all resources
        cnnSQL.Close()
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        On Error GoTo 0
        cmdSQL.Execute()
        cnnSQL = Nothing
        cmdSQL = Nothing
        'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME, "")

    End Function

    Public Function Update(ByVal lngProviderFeeID As Long, ByVal lngProviderID As Long, _
                           ByVal strCPTCode As String, ByVal strCPTModifier As String, _
                           ByVal strCPTDescription As String, ByVal curFee As Decimal, _
                           ByVal curContractAmount As Decimal, ByVal blnPatSched As Boolean, _
                           ByVal lngDuration As Long, ByVal lngPlanID As Long, _
                           ByVal strNotes As String, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 10/25/2000
        'Author: Morgan Travis
        'Description:  Inserts a row into the tblProviderFee table utilizing
        '              a stored procedure.
        'Parameters: lngProviderFeeID - Provider Fee ID number to update table
        '            lngProviderID - ID of Provider whose fee is being updated
        '            strCPTCode - CPT Code
        '            strCPTModifier - CPT Modifier
        '            strCPTDescription - CPT Description
        '            curFee - The monetary amount that will be updated
        '            strUserName - User name identifying the user making the change
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        cmdSQL.CommandText = "uspUpdProvFee"
        cmdSQL.CommandType = adCmdStoredProc

        With cmdSQL
            .Parameters.Append(.CreateParameter("@ProviderFeeID", adInteger, adParamInput, , lngProviderFeeID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID)) 'R002
            .Parameters.Append(.CreateParameter("@CPTCode", adVarChar, adParamInput, 10, strCPTCode)) 'R002
            .Parameters.Append(.CreateParameter("@CPTModifier", adVarChar, adParamInput, 2, strCPTModifier)) 'R002
            .Parameters.Append(.CreateParameter("@CPTDescription", adVarChar, adParamInput, 60, strCPTDescription)) 'R002
            .Parameters.Append(.CreateParameter("@Fee", adDecimal, adParamInput, , curFee))
            .Parameters.Append(.CreateParameter("@ContractAmount", adDecimal, adParamInput, , IIf(IsDBNull(curContractAmount), 0, curContractAmount)))
            .Parameters.Append(.CreateParameter("@PatSched", adVarChar, adParamInput, 1, IIf(blnPatSched = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@Duration", adInteger, adParamInput, , IIf(IsDBNull(lngDuration), 0, lngDuration)))
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , IIf(IsDBNull(lngPlanID), 0, lngPlanID)))
            .Parameters.Append(.CreateParameter("@Notes", adVarChar, adParamInput, 255, strNotes))
            .Parameters.Append(.CreateParameter("@ModifiedBy", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Function
        End If

        'Close the connection and free all resources
        cnnSQL.Close()
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        On Error GoTo 0
        cmdSQL.Execute()
        cnnSQL = Nothing
        cmdSQL = Nothing
        'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME, "")

    End Function

    Public Function Exists(ByVal lngProviderID As Long, ByVal strCPTCode As String, ByVal lngPlanID As Long, Optional ByVal lngProviderFeeID As Long = 0) As Boolean
        '--------------------------------------------------------------------
        'Date: 10/25/2000
        'Author: Morgan Travis
        'Description:  Checks for duplicate entries in the tblProviderFee table
        'Parameters: lngProviderID - ID of Provider
        '            strCPTCode - CPT Code
        '            strAction - Optional identifier to determine if checking for
        '               existance during Insert or Update.  Update requires that
        '               CPTCode/ProviderID combination utilizes the same Primary Key ID
        '               in order to be successful.
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim strSQL As String
        Dim blnExists As Boolean



        'Prepare the SQL statement
        If lngProviderFeeID = 0 Then 'R004
            strSQL = "SELECT "
            strSQL = strSQL & "COUNT(*) AS TOTAL "
            strSQL = strSQL & "FROM "
            strSQL = strSQL & TABLE_NAME
            strSQL = strSQL & " WHERE "
            strSQL = strSQL & " fldProviderID = " & lngProviderID
            strSQL = strSQL & " AND"
            strSQL = strSQL & " fldPlanID = " & lngPlanID
            strSQL = strSQL & " AND"
            strSQL = strSQL & " fldCPTCode = '" & strCPTCode & "'"
        Else  'R004
            strSQL = "SELECT "
            strSQL = strSQL & "fldProviderFeeID "
            strSQL = strSQL & "FROM "
            strSQL = strSQL & TABLE_NAME
            strSQL = strSQL & " WHERE "
            strSQL = strSQL & " fldProviderID = " & IIf(IsDBNull(lngProviderID), 0, lngProviderID)
            strSQL = strSQL & " AND"
            strSQL = strSQL & " fldPlanID = " & IIf(IsDBNull(lngPlanID), 0, lngPlanID)
            strSQL = strSQL & " AND"
            strSQL = strSQL & " fldCPTCode = '" & strCPTCode & "'"
        End If

        'Instantiate the Recordset.
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = adUseClient

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        ' Populate the recordset
        rstSQL.Open(strSQL, cnnSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rstSQL.ActiveConnection = Nothing

        'Close the connection and free all resources
        cnnSQL.Close()
        cnnSQL = Nothing

        If lngProviderFeeID = 0 Then
            'This condition ed when checking for duplicate during Insert 'R004
            If rstSQL.Fields("TOTAL").Value > 0 Then
                Exists = True
            Else
                Exists = False
            End If
        Else
            'This condition ed when checking for duplicate during Update 'R004
            If Not (rstSQL Is Nothing) Then
                Do While Not (rstSQL.EOF)
                    If rstSQL.Fields("fldProviderFeeID").Value <> lngProviderFeeID Then
                        Exists = True
                        Exit Do
                    Else
                        Exists = False
                    End If
                    rstSQL.MoveNext()
                Loop
            Else
                Exists = False
            End If
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

    Public Function FetchByProvider(ByVal lngProviderID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/25/2000                                                   '
        'Author: Morgan Travis                                              '
        'Description:  Retrieves records from the tblProviderFee table.     '
        'Parameters: lngProviderID - Provider ID number to fetch from table '
        '
        'Returns: Recordset of requested providers
        '-------------------------------------------------------------------'


        Dim cnn As ADODB.Connection
        Dim cmd As ADODB.Command
        Dim rst As ADODB.Recordset

        Dim SQLErrorNum As Long




        rst = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelFeeByProvider"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByProvider = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        cnn = Nothing
        cmd = Nothing
        rst = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function

    Public Function FetchByProviderCPTCode(ByVal lngProviderID As Long, _
                                           ByVal strCPTCode As String, _
                                           ByVal lngPlanID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/25/2000                                                   '
        'Author: Morgan Travis                                              '
        'Description:  Retrieves records from the tblProviderFee table.     '
        'Parameters: lngProviderID - Provider ID number to fetch from table '
        '            strCPTCode - CPT Code to fetch from table
        'Returns: RecordSet with requested fee
        '-------------------------------------------------------------------'


        Dim cnn As ADODB.Connection
        Dim cmd As ADODB.Command
        Dim rst As ADODB.Recordset

        Dim SQLErrorNum As Long




        rst = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelFeeByProviderCPTCode"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@CPTCode", adVarChar, adParamInput, 10, strCPTCode))
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByProviderCPTCode = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        cnn = Nothing
        cmd = Nothing
        rst = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function


    Public Function FetchByID(ByVal lngProviderFeeID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 05/08/2001
        'Author: Dave Richkun
        'Description:  Retrieves a record from the tblProviderFee table.
        'Parameters: lngProviderFeeID - Value of the Primary Key identifiying the
        '               row that will be retrieved.
        'Returns: ADO Recordset
        '-------------------------------------------------------------------'
        ' R001 - Created
        '-------------------------------------------------------------------'

        Dim cnn As ADODB.Connection
        Dim cmd As ADODB.Command
        Dim rst As ADODB.Recordset

        Dim SQLErrorNum As Long



        rst = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelFeeByID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderFeeID", adInteger, adParamInput, , lngProviderFeeID))
        End With

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByID = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        cnn = Nothing
        cmd = Nothing
        rst = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function

    Public Function FetchByPlan(ByVal lngProviderID As Long, _
                                ByVal lngPlanID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/01/2005                                                   '
        'Author: Duane C Orth                                              '
        'Description:  Retrieves records from the tblProviderFee table.'
        'Parameters: lngPlanID -   Plan ID number to fetch from table       '
        'Parameters: lngProviderID - Provider ID number to fetch from table '
        '                                                                   '
        'Returns: Recordset of requested providers                          '
        '-------------------------------------------------------------------'


        Dim cnn As ADODB.Connection
        Dim cmd As ADODB.Command
        Dim rst As ADODB.Recordset

        Dim SQLErrorNum As Long




        rst = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelFeeByPlan"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByPlan = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        cnn = Nothing
        cmd = Nothing
        rst = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function


    Public Function Delete(ByVal lngProviderFeeID As Long) 'R003
        '--------------------------------------------------------------------
        'Date: 05/08/2001
        'Author: Dave Richkun
        'Description:  Deletes a row from the tblProviderFee table utilizing
        '              a stored procedure.
        'Parameters: lngProviderFeeID - ID of row to delete
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        cmdSQL.CommandText = "uspDelProvFee"
        cmdSQL.CommandType = adCmdStoredProc

        With cmdSQL
            .Parameters.Append(.CreateParameter("@ProviderFeeID", adInteger, adParamInput, , lngProviderFeeID))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Function
        End If

        'Close the connection and free all resources
        cnnSQL.Close()
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        On Error GoTo 0
        cmdSQL.Execute()
        cnnSQL = Nothing
        cmdSQL = Nothing
        'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME, "")

    End Function


End Class
