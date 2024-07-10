VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 2  'RequiresTransaction
END
Attribute VB_Name = "CPatientCopayDB"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CPatientCopayDB
'Date: 10/31/2000
'Author: Morgan Travis
'Description:  MTS object designed to host methods associated with
'               data affecting Patient Copay table.
'--------------------------------------------------------------------
'Revision History:
'   R001 : Joshua Lockwood and David Nichol, 8FEB01 : Re-wrote insert
'       for DB side.
'--------------------------------------------------------------------

Public Function Insert(ByVal lngPatRPPlanID As Long, _
                       ByVal strCPTCode As String, ByVal lngProviderCredential As Long, _
                       ByVal strSeverity As String, ByVal lngVisitStart As Long, ByVal lngVisitEnd As Long, _
                       ByVal curRNCDollar As Currency, ByVal curRNCPercent As Currency, ByVal curCopayPercent As Currency, _
                       ByVal curCopay As Currency) As Long
'--------------------------------------------------------------------
'Date: 02/08/2001
'Author: Dave Nichol and Joshua Lockwood
'Description:  Inserts a row into the tblPatientCopay table utilizing
'              a stored procedure.
'Parameters: lngPatRPPlanID - Patient Responsible Party Plan ID number to insert into table
'            strCPTCode - The ten digit CPT Code that will be inserted into the table.
'            lngProviderCredential - Numerical representation of providers credentials.
'            strDSMIV - DSM-IV Code for treatment
'            lngVisitStart - Starting visit for plan info
'            lngVisitEnd - Ending visit for plan info
'            curRNCDollar - Amount paid to out of plan providers
'            curRNCPercent - Percentage paid to out of plan providers
'            curCopayPercent - Copay percent paid to in plan providers
'            curCopay - Copay amount paid to in plan providers
'Returns: Long (insertion location in table : ID)
'--------------------------------------------------------------------

    ' Declare connection, returned recordset, and command
    Dim cnn As ADODB.Connection
    Dim cmd As ADODB.Command
    Dim strErrMsg As String

    On Error GoTo ErrTrap:
    
'test :  DEBUG ONLY
strErrMsg = "Something went wrong in CPatientCopay's insert."
    
    'Create the parameter objects
    Set cmd = New ADODB.Command
    With cmd
        .CommandText = "uspInsPatientCopay"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@PatCopayID", adInteger, adParamReturnValue)
        .Parameters.Append .CreateParameter("@PatRPPlanID", adInteger, adParamInput, , lngPatRPPlanID)
        .Parameters.Append .CreateParameter("@CPTCode", adVarChar, adParamInput, 10, strCPTCode)
        .Parameters.Append .CreateParameter("@ProviderCredential", adInteger, adParamInput, , lngProviderCredential)
        .Parameters.Append .CreateParameter("@Severity", adVarChar, adParamInput, 1, strSeverity)
        .Parameters.Append .CreateParameter("@VisitStart", adInteger, adParamInput, , lngVisitStart)
        .Parameters.Append .CreateParameter("@VisitEnd", adInteger, adParamInput, , lngVisitEnd)
        .Parameters.Append .CreateParameter("@RNCDollar", adCurrency, adParamInput, , curRNCDollar)
        .Parameters.Append .CreateParameter("@RNCPercent", adCurrency, adParamInput, , curRNCPercent)
        .Parameters.Append .CreateParameter("@CopayPercent", adCurrency, adParamInput, , curCopayPercent)
        .Parameters.Append .CreateParameter("@Copay", adCurrency, adParamInput, , curCopay)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
   
    'Assign the connection to the Command object and execute the stored procedure
    Set cnn = New ADODB.Connection
    cnn.Open ("DSN=PsyquelSQL")
    Set cmd.ActiveConnection = cnn
    cmd.Execute , , adExecuteNoRecords

    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If
        
    Insert = cmd.Parameters("@PatCopayID").Value
    
    'Disconnect the recordset
    cnn.Close
    Set cmd.ActiveConnection = Nothing
    Set cmd = Nothing
    Set cnn = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    
    Set cmd.ActiveConnection = Nothing
    cnn.Close
    Set cmd = Nothing
    Set cnn = Nothing
    
'    Err.Raise Err.Number, Err.Source, Err.Description
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMsg
    Else
        Err.Raise Err.Number, CLASS_NAME, Err.Description
    End If
End Function

Public Sub Update(ByVal lngPatCopayID, ByVal strCPTCode As String, ByVal lngProviderCredential As Long, _
                       ByVal strSeverity As String, ByVal lngVisitStart As Long, ByVal lngVisitEnd As Long, _
                       ByVal curRNCDollar As Currency, ByVal curRNCPercent As Currency, ByVal curCopayPercent As Currency, _
                       ByVal curCopay As Currency)
'--------------------------------------------------------------------
'Date: 10/31/2000
'Author: Morgan Travis
'Description:  Updates a row into the tblPatientCopay table utilizing
'              a stored procedure.
'Parameters: lngPatCopayID - Patient Copay ID number to insert into table
'            strCPTCode - The ten digit CPT Code that will be inserted into the table.
'            lngProviderCredential - Numerical representation of providers credentials.
'            strDSMIV - DSM-IV Code for treatment
'            lngVisitStart - Starting visit for plan info
'            lngVisitEnd - Ending visit for plan info
'            curRNCDollar - Amount paid to out of plan providers
'            dblRNCPercent - Percentage paid to out of plan providers
'            dblCopayPercent - Copay percent paid to in plan providers
'            curCopay - Copay amount paid to in plan providers
'
'--------------------------------------------------------------------

    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command

    On Error GoTo ErrTrap:
    
    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    cmdSQL.CommandText = "uspUpdPatientCopay"
    cmdSQL.CommandType = adCmdStoredProc
    
    With cmdSQL
        .Parameters.Append .CreateParameter("@ID", adInteger, adParamInput, , lngPatCopayID)
        .Parameters.Append .CreateParameter("@CPTCode", adVarChar, adParamInput, 10, strCPTCode)
        .Parameters.Append .CreateParameter("@ProviderCredential", adInteger, adParamInput, , lngProviderCredential)
        .Parameters.Append .CreateParameter("@Severity", adVarChar, adParamInput, 1, strSeverity)
        .Parameters.Append .CreateParameter("@VisitStart", adInteger, adParamInput, , lngVisitStart)
        .Parameters.Append .CreateParameter("@VisitEnd", adInteger, adParamInput, , lngVisitEnd)
        .Parameters.Append .CreateParameter("@RNCDollar", adCurrency, adParamInput, , curRNCDollar)
        .Parameters.Append .CreateParameter("@RNCPercent", adCurrency, adParamInput, , curRNCPercent)
        .Parameters.Append .CreateParameter("@CopayPercent", adCurrency, adParamInput, , curCopayPercent)
        .Parameters.Append .CreateParameter("@Copay", adCurrency, adParamInput, , curCopay)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With

    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open("DSN=PsyquelSQL")
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute , , adExecuteNoRecords

    If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If

    'Close the connection and free all resources
    cnnSQL.Close
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing

    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    On Error GoTo 0
    GetObjectContext.SetAbort
    Set cnnSQL = Nothing
    Set cmdSQL = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub


Public Function Exists(ByVal lngPatRPPlanID As Long, _
                       ByVal strCPTCode As String, ByVal lngProviderCredential As Long, _
                       ByVal strSeverity As String, ByVal lngVisitStart As Long) As Boolean
'--------------------------------------------------------------------
'Date: 10/31/2000
'Author: Morgan Travis
'Description:  Checks for exisiting records in tblPatientCopay
'Parameters: lngPatRPPlanID - Patient Responsible Party Plan ID number to check for existing entries in table
'            strCPTCode - The ten digit CPT Code that will be inserted into the table.
'            lngProviderCredential - The number corresponding to the providers credentials.
'
'--------------------------------------------------------------------

    Dim cnnSQL As ADODB.Connection
    Dim rstSQL As ADODB.Recordset
    Dim strSQL As String
    Dim blnExists As Boolean

    On Error GoTo ErrTrap:

    'Prepare the SQL statement
    
    strSQL = "SELECT "
    strSQL = strSQL & "COUNT(*) AS TOTAL "
    strSQL = strSQL & "FROM "
    strSQL = strSQL & TABLE_NAME
    strSQL = strSQL & " WHERE "
    strSQL = strSQL & " fldPatRPPlanID = " & lngPatRPPlanID
    strSQL = strSQL & " AND "
    strSQL = strSQL & " fldCPTCode = '" & strCPTCode & "'"
    strSQL = strSQL & " AND "
    strSQL = strSQL & " fldProviderCredential = " & lngProviderCredential
    strSQL = strSQL & " AND "
    strSQL = strSQL & " fldSeverity = '" & strSeverity & "'"
    strSQL = strSQL & " AND "
    strSQL = strSQL & " fldVisitStart = " & lngVisitStart & " "
    
    
    'Instantiate the Recordset.
    Set rstSQL = New ADODB.Recordset
    rstSQL.CursorLocation = adUseClient
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open("DSN=PsyquelSQL")
    
    ' Populate the recordset
    Call rstSQL.Open(strSQL, cnnSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
    
    'Disconnect the recordset, close the connection and return the recordset
    'to the calling environment.
    Set rstSQL.ActiveConnection = Nothing

    'Close the connection and free all resources
    cnnSQL.Close
    Set cnnSQL = Nothing

    'Get the value from the Recordset then destroy it.
    If rstSQL.Fields("TOTAL").Value > 0 Then
        Exists = True
    Else
        Exists = False
    End If
    Set rstSQL = Nothing

    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set cnnSQL = Nothing
    Set rstSQL = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Function


Public Function FetchByPatRPPlan(ByVal lngPatRPPlanID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 10/31/2000                                                   '
'Author: Morgan Travis                                              '
'Description:  Retrieves records from the tblPatientCopay table.    '
'Parameters: lngPatRpPlanID - Patient Responsible Party Plan ID     '
'            number to fetch from table                             '
'
'Returns: Recordset of requested providers
'-------------------------------------------------------------------'

    
    Dim cnn As ADODB.Connection
    Dim cmd As ADODB.Command
    Dim rst As ADODB.Recordset
    
    Dim SQLErrorNum As Long
    
    On Error GoTo ErrTrap:

    
    Set rst = New ADODB.Recordset
    Set cmd = New ADODB.Command
    Set cnn = New ADODB.Connection
    
    'Acquire the database connection.
    cnn.Open ("DSN=PsyquelSQL")
    Set cmd.ActiveConnection = cnn
    
    'Create the parameter objects
    With cmd
        .CommandText = "uspSelCopayByPatient"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@PatRPPlanID", adInteger, adParamInput, , lngPatRPPlanID)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Execute the stored procedure
    rst.CursorLocation = adUseClient
    rst.Open cmd, , adOpenForwardOnly, adLockReadOnly
    
    Set FetchByPatRPPlan = rst
    
    'Disconnect the recordset
    Set cmd.ActiveConnection = Nothing
    Set cmd = Nothing
    Set rst.ActiveConnection = Nothing
    Set cnn = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set cnn = Nothing
    Set cmd = Nothing
    Set rst = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

Public Function FetchByPatRPPlanCPTCredential(ByVal lngPatRPPlanID As Long, ByVal strCPTCode As String, _
                                 ByVal lngProviderCredential As Long, ByVal strSeverity As String) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 10/31/2000                                                   '
'Author: Morgan Travis                                              '
'Description:  Retrieves records from the tblPatientCopay table.    '
'Parameters: lngPatRpPlanID - Patient Responsible Party Plan ID     '
'            number to fetch from table                             '
'
'Returns: Recordset of requested providers
'-------------------------------------------------------------------'

    
    Dim cnn As ADODB.Connection
    Dim cmd As ADODB.Command
    Dim rst As ADODB.Recordset
    
    Dim SQLErrorNum As Long
    
    On Error GoTo ErrTrap:

    
    Set rst = New ADODB.Recordset
    Set cmd = New ADODB.Command
    Set cnn = New ADODB.Connection
    
    'Acquire the database connection.
    cnn.Open ("DSN=PsyquelSQL")
    Set cmd.ActiveConnection = cnn
    
    'Create the parameter objects
    With cmd
        .CommandText = "uspSelAllCopayByPatient"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@PatRPPlanID", adInteger, adParamInput, , lngPatRPPlanID)
        .Parameters.Append .CreateParameter("@CPTCode", adVarChar, adParamInput, 10, strCPTCode)
        .Parameters.Append .CreateParameter("@ProviderCredential", adInteger, adParamInput, , lngProviderCredential)
        .Parameters.Append .CreateParameter("@Severity", adVarChar, adParamInput, 1, strSeverity)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Execute the stored procedure
    rst.CursorLocation = adUseClient
    rst.Open cmd, , adOpenForwardOnly, adLockReadOnly
    
    Set FetchByPatRPPlanCPTCredential = rst
    
    'Disconnect the recordset
    Set cmd.ActiveConnection = Nothing
    Set cmd = Nothing
    Set rst = Nothing
    Set cnn = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set cnn = Nothing
    Set cmd = Nothing
    Set rst = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

Public Sub Delete(ByVal lngPatCopayID As Long)
'--------------------------------------------------------------------
'Date: 10/31/2000
'Author: Morgan Travis
'Description:  Deletes a row into the tblPatientCopay table utilizing
'              a stored procedure.
'Parameters: lngPatCopayID - Patient Responsible Party Plan ID number to insert into table
'
'--------------------------------------------------------------------

    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command

    On Error GoTo ErrTrap:
    
    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    cmdSQL.CommandText = "uspDelPatientCopay"
    cmdSQL.CommandType = adCmdStoredProc
    
    With cmdSQL
        .Parameters.Append .CreateParameter("@PatCopayID", adInteger, adParamInput, , lngPatCopayID)
    End With

    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open("DSN=PsyquelSQL")
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute , , adExecuteNoRecords

    If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If
    
    'Close the connection and free all resources
    cnnSQL.Close
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing

    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    On Error GoTo 0
    GetObjectContext.SetAbort
    Set cnnSQL = Nothing
    Set cmdSQL = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub

