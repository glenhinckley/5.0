VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 2  'RequiresTransaction
END
Attribute VB_Name = "CPlanBenefitDB"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CPlanBenVerDB
'Date: 10/26/2000
'Author: Morgan Travis
'Description:  MTS object designed to host methods associated with
'               data affecting Plan Benefits table.
'--------------------------------------------------------------------
'--------------------------------------------------------------------
Option Explicit

Private Const CLASS_NAME As String = "CPlanBenefitDB"
Private Const TABLE_NAME As String = "tblPlanDefault"

Public Function Insert(ByVal lngPlanID As Long, _
                       ByVal dteFiscalStart As Date, ByVal dteFiscalEnd As Date, _
                       ByVal curDeductible As Currency, ByVal strGroupDeductible As String, _
                       ByVal curDeductibleMet As Currency, ByVal curAnnualDollarLimit As Currency, _
                       ByVal curAmountApplied As Currency, ByVal lngAnnualVisitLimit As Long, _
                       ByVal lngVisits As Long, ByVal strCatastrophicCap As String, _
                       ByVal curCatastrophicCapAmt As Currency, ByVal strCertRequired As String, _
                       ByVal strNoHCFA As String, ByVal strNoInvoice As String, _
                       ByVal strOutPatientRept As String, ByVal strVerbRept As String, _
                       ByVal lngNumVisit As Long, ByVal lngDaysPrior As Long, _
                       ByVal strElectClaim As String, ByVal strRecurrCopay As String) As Long
'--------------------------------------------------------------------
'Date: 10/26/2000
'Author: Morgan Travis
'Description:  Inserts a row into the tblPlanBenefit table utilizing
'              a stored procedure.
'Parameters:
'--------------------------------------------------------------------

    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command

    On Error GoTo ErrTrap:
    
    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    cmdSQL.CommandText = "uspInsPlanDefault"
    cmdSQL.CommandType = adCmdStoredProc
    
    With cmdSQL
        .Parameters.Append .CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID)
        .Parameters.Append .CreateParameter("@FiscalStart", adDBTimeStamp, adParamInput, , dteFiscalStart)
        .Parameters.Append .CreateParameter("@FiscalEnd", adDBTimeStamp, adParamInput, , dteFiscalEnd)
        .Parameters.Append .CreateParameter("@Deductible", adCurrency, adParamInput, , curDeductible)
        .Parameters.Append .CreateParameter("@GroupDeductible", adVarChar, adParamInput, 1, strGroupDeductible)
        .Parameters.Append .CreateParameter("@DeductibleMet", adCurrency, adParamInput, , curDeductibleMet)
        .Parameters.Append .CreateParameter("@AnnualDollarLimit", adCurrency, adParamInput, , curAnnualDollarLimit)
        .Parameters.Append .CreateParameter("@AmountApplied", adCurrency, adParamInput, , curAmountApplied)
        .Parameters.Append .CreateParameter("@AnnualVisitLimit", adInteger, adParamInput, , lngAnnualVisitLimit)
        .Parameters.Append .CreateParameter("@Visits", adInteger, adParamInput, , lngVisits)
        .Parameters.Append .CreateParameter("@CatastrophicCap", adVarChar, adParamInput, 1, strCatastrophicCap)
        .Parameters.Append .CreateParameter("@CatastrophicCapAmt", adCurrency, adParamInput, , curCatastrophicCapAmt)
        .Parameters.Append .CreateParameter("@CertRequired", adVarChar, adParamInput, 1, strCertRequired)
        .Parameters.Append .CreateParameter("@NoHCFA", adVarChar, adParamInput, 1, strNoHCFA)
        .Parameters.Append .CreateParameter("@NoInvoice", adVarChar, adParamInput, 1, strNoInvoice)
        .Parameters.Append .CreateParameter("@OutPatientRept", adVarChar, adParamInput, 1, strOutPatientRept)
        .Parameters.Append .CreateParameter("@VerbRept", adVarChar, adParamInput, 1, strVerbRept)
        .Parameters.Append .CreateParameter("@NumVisit", adInteger, adParamInput, , lngNumVisit)
        .Parameters.Append .CreateParameter("@DaysPrior", adInteger, adParamInput, , lngDaysPrior)
        .Parameters.Append .CreateParameter("@ElectClaim", adVarChar, adParamInput, 1, strElectClaim)
        .Parameters.Append .CreateParameter("@RecurrCopay", adVarChar, adParamInput, 1, strRecurrCopay)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    
    End With

    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open("DSN=PsyquelSQL")
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute ', , adExecuteNoRecords

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
    Err.Raise Err.Number, Err.Source, Err.Description
End Function

Public Function Update(ByVal lngPlanID As Long, _
                       ByVal dteFiscalStart As Date, ByVal dteFiscalEnd As Date, _
                       ByVal curDeductible As Currency, ByVal strGroupDeductible As String, _
                       ByVal curDeductibleMet As Currency, ByVal curAnnualDollarLimit As Currency, _
                       ByVal curAmountApplied As Currency, ByVal lngAnnualVisitLimit As Long, _
                       ByVal lngVisits As Long, ByVal strCatastrophicCap As String, _
                       ByVal curCatastrophicCapAmt As Currency, ByVal strCertRequired As String, _
                       ByVal strNoHCFA As String, ByVal strNoInvoice As String, _
                       ByVal strOutPatientRept As String, ByVal strVerbRept As String, _
                       ByVal lngNumVisit As Long, ByVal lngDaysPrior As Long, _
                       ByVal strElectClaim As String, ByVal strRecurrCopay As String) As Long
'--------------------------------------------------------------------
'Date: 10/26/2000
'Author: Morgan Travis
'Description:  Updates a row into the tblPlanBenefit table utilizing
'              a stored procedure.
'Parameters:
'--------------------------------------------------------------------

    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command

    On Error GoTo ErrTrap:
    
    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    cmdSQL.CommandText = "uspUpdPlanDefault"
    cmdSQL.CommandType = adCmdStoredProc
    
    With cmdSQL
        .Parameters.Append .CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID)
        .Parameters.Append .CreateParameter("@FiscalStart", adDBTimeStamp, adParamInput, , dteFiscalStart)
        .Parameters.Append .CreateParameter("@FiscalEnd", adDBTimeStamp, adParamInput, , dteFiscalEnd)
        .Parameters.Append .CreateParameter("@Deductible", adCurrency, adParamInput, , curDeductible)
        .Parameters.Append .CreateParameter("@GroupDeductible", adVarChar, adParamInput, 1, strGroupDeductible)
        .Parameters.Append .CreateParameter("@DeductibleMet", adCurrency, adParamInput, , curDeductibleMet)
        .Parameters.Append .CreateParameter("@AnnualDollarLimit", adCurrency, adParamInput, , curAnnualDollarLimit)
        .Parameters.Append .CreateParameter("@AmountApplied", adCurrency, adParamInput, , curAmountApplied)
        .Parameters.Append .CreateParameter("@AnnualVisitLimit", adInteger, adParamInput, , lngAnnualVisitLimit)
        .Parameters.Append .CreateParameter("@Visits", adInteger, adParamInput, , lngVisits)
        .Parameters.Append .CreateParameter("@CatastrophicCap", adVarChar, adParamInput, 1, strCatastrophicCap)
        .Parameters.Append .CreateParameter("@CatastrophicCapAmt", adCurrency, adParamInput, , curCatastrophicCapAmt)
        .Parameters.Append .CreateParameter("@CertRequired", adVarChar, adParamInput, 1, strCertRequired)
        .Parameters.Append .CreateParameter("@NoHCFA", adVarChar, adParamInput, 1, strNoHCFA)
        .Parameters.Append .CreateParameter("@NoInvoice", adVarChar, adParamInput, 1, strNoInvoice)
        .Parameters.Append .CreateParameter("@OutPatientRept", adVarChar, adParamInput, 1, strOutPatientRept)
        .Parameters.Append .CreateParameter("@VerbRept", adVarChar, adParamInput, 1, strVerbRept)
        .Parameters.Append .CreateParameter("@NumVisit", adInteger, adParamInput, , lngNumVisit)
        .Parameters.Append .CreateParameter("@DaysPrior", adInteger, adParamInput, , lngDaysPrior)
        .Parameters.Append .CreateParameter("@ElectClaim", adVarChar, adParamInput, 1, strElectClaim)
        .Parameters.Append .CreateParameter("@RecurrCopay", adVarChar, adParamInput, 1, strRecurrCopay)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    
    End With

    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open("DSN=PsyquelSQL")
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute ', , adExecuteNoRecords

    If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If
    
    Update = cmdSQL.Parameters(0).Value

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
    Err.Raise Err.Number, Err.Source, Err.Description
End Function



Public Function FetchByPlan(ByVal lngPlanID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 10/26/2000                                                   '
'Author: Morgan Travis                                              '
'Description:  Retrieves records from the tblPatientBenefitVerfication table.     '
'Parameters: lngPlanID - Plan ID number to fetch from table '
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
        .CommandText = "uspSelPlanDefaultByPlanID"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@Plan", adInteger, adParamInput, , lngPlanID)
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

Public Function Exists(ByVal lngPlanID As Long)

'--------------------------------------------------------------------
'Date: 11/02/2000
'Author: Morgan Travis
'Description:  Checks for existing entries in tblPatientBenefit
'Parameters: lngPlanID - Plan ID number to check for existing entries in table
'
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



