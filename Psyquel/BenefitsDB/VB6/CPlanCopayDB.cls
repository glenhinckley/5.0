VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 2  'RequiresTransaction
END
Attribute VB_Name = "CPlanCopayDB"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CPlanCopayDB
'Date: 10/31/2000
'Author: Morgan Travis
'Description:  MTS object designed to host methods associated with
'               data affecting Plan Copay table.
'--------------------------------------------------------------------
'--------------------------------------------------------------------
Option Explicit

Private Const CLASS_NAME As String = "CPlanCopayDB"
Private Const TABLE_NAME As String = "tblPlanCopay"

Public Function Insert(ByVal lngPlanID As Long, _
                       ByVal strCPTCode As String, ByVal lngProviderCredential As Long, _
                       ByVal strDSMIV As String, ByVal lngVisitStart As Long, ByVal lngVisitEnd As Long, _
                       ByVal curRNCDollar As Currency, ByVal dblRNCPercent As Double, ByVal dblCopayPercent As Double, _
                       ByVal curCopay As Currency)
'--------------------------------------------------------------------
'Date: 10/31/2000
'Author: Morgan Travis
'Description:  Inserts a row into the tblPlanCopay table utilizing
'              a stored procedure.
'Parameters: lngPlanID -   Plan ID number to insert into table
'            strCPTCode - The ten digit CPT Code that will be inserted into the table.
'            lngProviderCredential - Numerical representation of providers credentials.
'            strDSMIV - DSM-IV Code for treatment
'            lngVisitStart - Starting visit for plan info
'            lngVisitEnd - Ending visit for plan info
'            curRNCDollar - Amount paid to out of plan providers
'            dblRNCPercent - Percentage paid to out of plan providers
'            dblCopayPercent - Copay percent paid to in plan providers
'            curCopay - Copay amount paid to in plan providers
'--------------------------------------------------------------------

    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command

    On Error GoTo ErrTrap:
    
    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    cmdSQL.CommandText = "uspInsPlanCopay"
    cmdSQL.CommandType = adCmdStoredProc
    
    With cmdSQL
        .Parameters.Append .CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID)
        .Parameters.Append .CreateParameter("@CPTCode", adVarChar, adParamInput, 10, strCPTCode)
        .Parameters.Append .CreateParameter("@ProviderCredential", adInteger, adParamInput, , lngProviderCredential)
        .Parameters.Append .CreateParameter("@DSMIV", adVarChar, adParamInput, 1, strDSMIV)
        .Parameters.Append .CreateParameter("@VisitStart", adInteger, adParamInput, , lngVisitStart)
        .Parameters.Append .CreateParameter("@VisitEnd", adInteger, adParamInput, , lngVisitEnd)
        .Parameters.Append .CreateParameter("@RNCDollar", adCurrency, adParamInput, , curRNCDollar)
        .Parameters.Append .CreateParameter("@RNCPercent", adCurrency, adParamInput, , dblRNCPercent)
        .Parameters.Append .CreateParameter("@CopayPercent", adCurrency, adParamInput, , dblCopayPercent)
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
    
    Insert = cmdSQL.Parameters(0).Value

    'Close the connection and free all resources
    cnnSQL.Close
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing

    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    On Error GoTo 0
    GetObjectContext.SetAbort
    Set cnnSQL = Nothing
    Set cmdSQL = Nothing
    Call RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME, "")

End Function


Public Function Update(ByVal lngPlanCopayID As Long, _
                       ByVal strDSMIV As String, ByVal lngVisitStart As Long, ByVal lngVisitEnd As Long, _
                       ByVal curRNCDollar As Currency, ByVal dblRNCPercent As Double, ByVal dblCopayPercent As Double, _
                       ByVal curCopay As Currency)
'--------------------------------------------------------------------
'Date: 10/31/2000
'Author: Morgan Travis
'Description:  Updates a row into the tblPlanCopay table utilizing
'              a stored procedure.
'Parameters: lngPlanID -   Plan ID number to insert into table
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
    cmdSQL.CommandText = "uspUpdPlanCopay"
    cmdSQL.CommandType = adCmdStoredProc
    
    With cmdSQL
        .Parameters.Append .CreateParameter("@ID", adInteger, adParamInput, , lngPlanCopayID)
        .Parameters.Append .CreateParameter("@DSMIV", adVarChar, adParamInput, 1, strDSMIV)
        .Parameters.Append .CreateParameter("@VisitStart", adInteger, adParamInput, 10, lngVisitStart)
        .Parameters.Append .CreateParameter("@VisitEnd", adInteger, adParamInput, , lngVisitEnd)
        .Parameters.Append .CreateParameter("@RNCDollar", adCurrency, adParamInput, , curRNCDollar)
        .Parameters.Append .CreateParameter("@RNCPercent", adCurrency, adParamInput, , dblRNCPercent)
        .Parameters.Append .CreateParameter("@CopayPercent", adCurrency, adParamInput, , dblCopayPercent)
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
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    On Error GoTo 0
    GetObjectContext.SetAbort
    Set cnnSQL = Nothing
    Set cmdSQL = Nothing
    Call RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME, "")

End Function


Public Function Exists(ByVal lngPlanID As Long, ByVal strCPTCode As String, _
                       ByVal lngProviderCredential As Long)
'--------------------------------------------------------------------
'Date: 10/31/2000
'Author: Morgan Travis
'Description:  Checks for exisiting records in tblPlanCopay
'Parameters: lngPlanID -   Plan ID number to check for existing entries in table
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
    strSQL = strSQL & " fldPlanID = " & lngPlanID
    strSQL = strSQL & " AND "
    strSQL = strSQL & " fldCPTCode = '" & strCPTCode & "'"
    strSQL = strSQL & " AND "
    strSQL = strSQL & " fldProviderCredential = '" & lngProviderCredential & "'"
    
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


Public Function FetchByPlan(ByVal lngPlanID As Long, ByVal strCPTCode As String, _
                                 ByVal lngProviderCredential As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 10/31/2000                                                   '
'Author: Morgan Travis                                              '
'Description:  Retrieves records from the tblPlanCopay table.    '
'Parameters: lngPlanID -   Plan ID     '
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
        .CommandText = "uspSelCopayByPlan"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@ProviderID", adInteger, adParamInput, , lngPlanID)
        .Parameters.Append .CreateParameter("@CPTCode", adVarChar, adParamInput, 10, strCPTCode)
        .Parameters.Append .CreateParameter("@ProviderCredential", adInteger, adParamInput, , lngProviderCredential)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Execute the stored procedure
    rst.CursorLocation = adUseClient
    rst.Open cmd, , adOpenForwardOnly, adLockReadOnly
    
    Set FetchByPlan = rst
    
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

