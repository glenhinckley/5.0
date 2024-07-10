
'-------------------------------------------------------------------------------------
'Date: 04/20/2009
'Class Name: RemitDB
'Author: Duane C Orth
'Description:   COM object designed to host methods associated with electronic
'               checks.
'--------------------------------------------------------------------------------------
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
' Lydia Orth 

Public Class CRemitDetailDB


    Private Const CLASS_NAME As String = "RemitDetailDB"
    Private Const TABLE_NAME As String = "tblRemittanceDetail"



    Private _ConnectionString As String = String.Empty
    Private _AppPath As String = String.Empty
    Private _EDIOutput As String = String.Empty
    Private _MachineName As String = String.Empty
    Private _UserLoginName As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property




    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False, _
                          Optional ByVal strWhere As String = "", _
                          Optional ByVal strOrderBy As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 04/20/2009
        'Author: Duane C Orth
        'Description:  Retrieves records from the tblRemittance table.
        'Parameters: blnIncludeDisabled - Optional parameter that identifies if
        '               records flagged as 'Disabled' or 'De-activated' are to be
        '               included in the record set. The default value is False.
        '            strWhere - Optional 'Where' clause of the SQL statement by
        '               which records will be filtered.  If the parameter is not
        '               supplied, all records will be retrieved.
        '             strOrderBy - Optional 'Order By' clause in which retrieved
        '               records will be sorted.  The default is set to the
        '               'fldName' column
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim strDfltWhere As String
        Dim strSQL As String



        'Prepare the SQL statement.
        strSQL = "SELECT "
        strSQL = strSQL & TABLE_NAME & ".* "
        strSQL = strSQL & "FROM "
        strSQL = strSQL & TABLE_NAME & " "

        'Determine whether to include 'disabled' records'
        If blnIncludeDisabled = False Then
            strDfltWhere = " WHERE fldDisabledYN = 'N' "
        End If

        'Attach user's where clause if applicable
        strSQL = strSQL & strDfltWhere

        If Trim(strWhere > "") Then If blnIncludeDisabled Then strSQL = strSQL & " WHERE " & strWhere & " " Else strSQL = strSQL & " AND " & strWhere & " "

        If Trim(strOrderBy) > "" Then
            strSQL = strSQL & " ORDER BY "
            strSQL = strSQL & strOrderBy
        Else
            strSQL = strSQL & " ORDER BY "
            strSQL = strSQL & "fldInsName "
        End If


        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Instantiate and populate the Recordset.
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(strSQL, cnnSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rstSQL.ActiveConnection = Nothing

        cnnSQL.Close()
        cnnSQL = Nothing
        Fetch = rstSQL

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        rstSQL.ActiveConnection = Nothing
        rstSQL = Nothing
        cnnSQL = Nothing
        'cmdSQL.Execute()
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function
    Public Function Insert(ByVal lngRemittanceRecordID As Long, ByVal strPayorID As String, ByVal strPatientNumber As String, _
                           ByVal strLineNumber As String, ByVal strServiceLineNumber As String, ByVal dteFirstDateOfService As Date, _
                           ByVal dteLastDateOfService As Date, ByVal dteClaimReceiveDate As Date, _
                           ByVal strPlaceOfService As String, ByVal strTypeOfService As String, ByVal strProcedureCode As String, _
                           ByVal strMod1 As String, ByVal strMod2 As String, ByVal strMod3 As String, _
                           ByVal strUnits As String, ByVal dblCharge As Double, ByVal dblDisallowed As Double, ByVal dblAllowed As Double, _
                           ByVal dblDeductible As Double, ByVal dblCoinsurance As Double, ByVal dblPatientResp As Double, _
                           ByVal dblInterest As Double, ByVal dblPayment As Double, _
                           ByVal strProvider As String, ByVal strReason1 As String, ByVal strReason2 As String, ByVal strReason3 As String, _
                           ByVal strReason4 As String, ByVal strReason5 As String, ByVal strReason6 As String, ByVal strReason7 As String, _
                           ByVal blnPosted As Boolean, ByVal dtePostedDate As Date, _
                           ByVal lngEncounterLogID As Long, ByVal lngPatientID As Long, ByVal lngInsuranceID As Long, ByVal lngProviderID As Long, _
                           ByVal lngCheckID As Long, ByVal lngCheckLogID As Long, ByVal strErnFileName As String, ByVal strErrorMessage As String, ByVal strAddedBy As String) As Long


        '--------------------------------------------------------------------
        'Date: 04/20/2009
        'Author: Duane C Orth
        'Description:  Inserts a row into the tblRemittance table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim intCount As Integer



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspInsRemittanceDetail"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@RemittanceDetailRecordID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@RemittanceRecordID", adInteger, adParamInput, , lngRemittanceRecordID))
            .Parameters.Append(.CreateParameter("@PayorID", adVarChar, adParamInput, 10, strPayorID))
            .Parameters.Append(.CreateParameter("@PatientNumber", adVarChar, adParamInput, 17, strPatientNumber))
            .Parameters.Append(.CreateParameter("@LineNumber", adVarChar, adParamInput, 17, strLineNumber))
            .Parameters.Append(.CreateParameter("@ServiceLineNumber", adVarChar, adParamInput, 3, strServiceLineNumber))
            .Parameters.Append(.CreateParameter("@FirstDateOfService", adDBTimeStamp, adParamInput, , IIf(CLng(dteFirstDateOfService.ToOADate()) = 0, DBNull.Value, dteFirstDateOfService)))
            .Parameters.Append(.CreateParameter("@LastDateOfService", adDBTimeStamp, adParamInput, , IIf(CLng(dteLastDateOfService.ToOADate()) = 0, DBNull.Value, dteLastDateOfService)))
            .Parameters.Append(.CreateParameter("@ClaimReceiveDate", adDBTimeStamp, adParamInput, , IIf(CLng(dteClaimReceiveDate.ToOADate()) = 0, DBNull.Value, dteClaimReceiveDate)))
            .Parameters.Append(.CreateParameter("@PlaceOfService", adVarChar, adParamInput, 2, strPlaceOfService))
            .Parameters.Append(.CreateParameter("@TypeOfService", adVarChar, adParamInput, 2, strTypeOfService))
            .Parameters.Append(.CreateParameter("@ProcedureCode", adVarChar, adParamInput, 5, strProcedureCode))
            .Parameters.Append(.CreateParameter("@Mod1", adVarChar, adParamInput, 2, strMod1))
            .Parameters.Append(.CreateParameter("@Mod2", adVarChar, adParamInput, 2, strMod2))
            .Parameters.Append(.CreateParameter("@Mod3", adVarChar, adParamInput, 5, strMod3))
            .Parameters.Append(.CreateParameter("@Units", adVarChar, adParamInput, 4, strUnits))
            .Parameters.Append(.CreateParameter("@Charge", adDecimal, adParamInput, , dblCharge))
            .Parameters.Append(.CreateParameter("@Disallowed", adDecimal, adParamInput, , dblDisallowed))
            .Parameters.Append(.CreateParameter("@Allowed", adDecimal, adParamInput, , dblAllowed))
            .Parameters.Append(.CreateParameter("@Deductible", adDecimal, adParamInput, , dblDeductible))
            .Parameters.Append(.CreateParameter("@Coinsurance", adDecimal, adParamInput, , dblCoinsurance))
            .Parameters.Append(.CreateParameter("@PatientResp", adDecimal, adParamInput, , dblPatientResp))
            .Parameters.Append(.CreateParameter("@Interest", adDecimal, adParamInput, , dblInterest))
            .Parameters.Append(.CreateParameter("@Payment", adDecimal, adParamInput, , dblPayment))
            .Parameters.Append(.CreateParameter("@Provider", adVarChar, adParamInput, 15, strProvider))
            .Parameters.Append(.CreateParameter("@Reason1", adVarChar, adParamInput, 6, strReason1))
            .Parameters.Append(.CreateParameter("@Reason2", adVarChar, adParamInput, 6, strReason2))
            .Parameters.Append(.CreateParameter("@Reason3", adVarChar, adParamInput, 6, strReason3))
            .Parameters.Append(.CreateParameter("@Reason4", adVarChar, adParamInput, 6, strReason4))
            .Parameters.Append(.CreateParameter("@Reason5", adVarChar, adParamInput, 6, strReason5))
            .Parameters.Append(.CreateParameter("@Reason6", adVarChar, adParamInput, 6, strReason6))
            .Parameters.Append(.CreateParameter("@Reason7", adVarChar, adParamInput, 6, strReason7))
            .Parameters.Append(.CreateParameter("@Posted", adVarChar, adParamInput, 1, IIf(blnPosted = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@PostedDate", adDBTimeStamp, adParamInput, , IIf(CLng(dtePostedDate.ToOADate()) = 0, DBNull.Value, dtePostedDate)))
            .Parameters.Append(.CreateParameter("@EncounterLogID", adInteger, adParamInput, , lngEncounterLogID))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@InsuranceID", adInteger, adParamInput, , lngInsuranceID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@CheckID", adInteger, adParamInput, , lngCheckID))
            .Parameters.Append(.CreateParameter("@CheckLogID", adInteger, adParamInput, , lngCheckLogID))
            .Parameters.Append(.CreateParameter("@ErnFileName", adVarChar, adParamInput, 50, strErnFileName))
            .Parameters.Append(.CreateParameter("@ErrorMessage", adVarChar, adParamInput, 255, strErrorMessage))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strAddedBy))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        Insert = cmdSQL.Parameters("@RemittanceDetailRecordID").Value

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
            Exit Function
        End If

        'Close the connection
        cnnSQL.Close()

        'free all resources
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        'RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME)

    End Function

    Public Sub Update(ByVal lngID As Long, ByVal lngRemittanceRecordID As Long, ByVal strPayorID As String, ByVal strPatientNumber As String, _
                           ByVal strLineNumber As String, ByVal strServiceLineNumber As String, ByVal dteFirstDateOfService As Date, _
                           ByVal dteLastDateOfService As Date, ByVal dteClaimReceiveDate As Date, _
                           ByVal strPlaceOfService As String, ByVal strTypeOfService As String, ByVal strProcedureCode As String, _
                           ByVal strMod1 As String, ByVal strMod2 As String, ByVal strMod3 As String, _
                           ByVal strUnits As String, ByVal dblCharge As Double, ByVal dblDisallowed As Double, ByVal dblAllowed As Double, _
                           ByVal dblDeductible As Double, ByVal dblCoinsurance As Double, ByVal dblPatientResp As Double, _
                           ByVal dblInterest As Double, ByVal dblPayment As Double, _
                           ByVal strProvider As String, ByVal strReason1 As String, ByVal strReason2 As String, ByVal strReason3 As String, _
                           ByVal strReason4 As String, ByVal strReason5 As String, ByVal strReason6 As String, ByVal strReason7 As String, _
                           ByVal blnPosted As Boolean, ByVal dtePostedDate As Date, _
                           ByVal lngEncounterLogID As Long, ByVal lngPatientID As Long, ByVal lngInsuranceID As Long, ByVal lngProviderID As Long, _
                           ByVal lngCheckID As Long, ByVal lngCheckLogID As Long, ByVal strErnFileName As Long, ByVal strErrorMessage As Long, ByVal strAddedBy As Long)
        '--------------------------------------------------------------------
        'Date: 04/20/2009
        'Author: Duane C Orth
        'Description:  Updates a row in the tblRemittance table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: Null
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdRemittanceDetail"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@RemittanceDetailRecordID", adInteger, adParamInput, , lngID))
            .Parameters.Append(.CreateParameter("@RemittanceRecordID", adInteger, adParamInput, , lngRemittanceRecordID))
            .Parameters.Append(.CreateParameter("@PayorID", adVarChar, adParamInput, 10, strPayorID))
            .Parameters.Append(.CreateParameter("@PatientNumber", adVarChar, adParamInput, 17, strPatientNumber))
            .Parameters.Append(.CreateParameter("@LineNumber", adVarChar, adParamInput, 17, strLineNumber))
            .Parameters.Append(.CreateParameter("@ServiceLineNumber", adVarChar, adParamInput, 3, strServiceLineNumber))
            .Parameters.Append(.CreateParameter("@FirstDateOfService", adDBTimeStamp, adParamInput, , IIf(CLng(dteFirstDateOfService.ToOADate()) = 0, DBNull.Value, dteFirstDateOfService)))
            .Parameters.Append(.CreateParameter("@LastDateOfService", adDBTimeStamp, adParamInput, , IIf(CLng(dteLastDateOfService.ToOADate()) = 0, DBNull.Value, dteLastDateOfService)))
            .Parameters.Append(.CreateParameter("@ClaimReceiveDate", adDBTimeStamp, adParamInput, , IIf(CLng(dteClaimReceiveDate.ToOADate()) = 0, DBNull.Value, dteClaimReceiveDate)))
            .Parameters.Append(.CreateParameter("@PlaceOfService", adVarChar, adParamInput, 2, strPlaceOfService))
            .Parameters.Append(.CreateParameter("@TypeOfService", adVarChar, adParamInput, 2, strTypeOfService))
            .Parameters.Append(.CreateParameter("@ProcedureCode", adVarChar, adParamInput, 5, strProcedureCode))
            .Parameters.Append(.CreateParameter("@Mod1", adVarChar, adParamInput, 2, strMod1))
            .Parameters.Append(.CreateParameter("@Mod2", adVarChar, adParamInput, 2, strMod2))
            .Parameters.Append(.CreateParameter("@Mod3", adVarChar, adParamInput, 5, strMod3))
            .Parameters.Append(.CreateParameter("@Units", adVarChar, adParamInput, 4, strUnits))
            .Parameters.Append(.CreateParameter("@Charge", adDecimal, adParamInput, , dblCharge))
            .Parameters.Append(.CreateParameter("@Disallowed", adDecimal, adParamInput, , dblDisallowed))
            .Parameters.Append(.CreateParameter("@Allowed", adDecimal, adParamInput, , dblAllowed))
            .Parameters.Append(.CreateParameter("@Deductible", adDecimal, adParamInput, , dblDeductible))
            .Parameters.Append(.CreateParameter("@Coinsurance", adDecimal, adParamInput, , dblCoinsurance))
            .Parameters.Append(.CreateParameter("@PatientResp", adDecimal, adParamInput, , dblPatientResp))
            .Parameters.Append(.CreateParameter("@Interest", adDecimal, adParamInput, , dblInterest))
            .Parameters.Append(.CreateParameter("@Payment", adDecimal, adParamInput, , dblPayment))
            .Parameters.Append(.CreateParameter("@Provider", adVarChar, adParamInput, 15, strProvider))
            .Parameters.Append(.CreateParameter("@Reason1", adVarChar, adParamInput, 6, strReason1))
            .Parameters.Append(.CreateParameter("@Reason2", adVarChar, adParamInput, 6, strReason2))
            .Parameters.Append(.CreateParameter("@Reason3", adVarChar, adParamInput, 6, strReason3))
            .Parameters.Append(.CreateParameter("@Reason4", adVarChar, adParamInput, 6, strReason4))
            .Parameters.Append(.CreateParameter("@Reason5", adVarChar, adParamInput, 6, strReason5))
            .Parameters.Append(.CreateParameter("@Reason6", adVarChar, adParamInput, 6, strReason6))
            .Parameters.Append(.CreateParameter("@Reason7", adVarChar, adParamInput, 6, strReason7))
            .Parameters.Append(.CreateParameter("@Posted", adVarChar, adParamInput, 1, IIf(blnPosted = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@PostedDate", adDBTimeStamp, adParamInput, , IIf(CLng(dtePostedDate.ToOADate()) = 0, DBNull.Value, dtePostedDate)))
            .Parameters.Append(.CreateParameter("@EncounterLogID", adInteger, adParamInput, , lngEncounterLogID))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@InsuranceID", adInteger, adParamInput, , lngInsuranceID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@CheckID", adInteger, adParamInput, , lngCheckID))
            .Parameters.Append(.CreateParameter("@CheckLogID", adInteger, adParamInput, , lngCheckLogID))
            .Parameters.Append(.CreateParameter("@ErnFileName", adVarChar, adParamInput, 50, strErnFileName))
            .Parameters.Append(.CreateParameter("@ErrorMessage", adVarChar, adParamInput, 255, strErrorMessage))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strAddedBy))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
            Exit Sub
        End If

        'Close the connection and free all resources
        cnnSQL.Close()

        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


    End Sub
End Class
