
'--------------------------------------------------------------------
'Class Name: CEncounterLogDB
'Date: 03/06/2000
'Author: Eric Pena
'Denis J Finkel
'Description:  MTS object designed to host methods associated with
'              data affecting the tblEncounterLog  table.
'--------------------------------------------------------------------
'Revision History:
'  R001 06/13/2000 Richkun: Fixed bug in FetchStatus() where identical
'           results were returned no matter what value was passed to
'           Processed parameter.  Improved logic to check for Array
'           before prematurely exiting function.
'  R002 07/07/2000 Richkun: Added FetchForPatient() method.
'  R003 08/09/2000 Richkun: Added FetchByID(), FetchByUserToday() methods
'  R004 09/22/2000 Segura: Added optional lngApptID parameter to Insert()
'  R005 02/12/2001 Richkun: Added FetchForPatientUser() method.
'  R006 03/01/2001 Segura:  Added FetchSeekNAssign() Method
'  R007 03/23/2001 Pena:  Added functionality for no-show billing
'  R008 08/01/2001 Richkun:  Added support for fldModifier column
'  R009 08/07/2001 Richkun:  Added support for fldReferPhy, fldReferPhyID columns
'  R010 11/26/2001 Richkun:  Altered isProcessed() to look at BillStatus column
'  R011 05/23/2002 Richkun:  Added support for fldAdmitDate, fldDischargeDate columns
'  R012 07/16/2002 Richkun:  Added Validate() method for server-side validation
'  R013 03/05/2003 Richkun:  Added FetchRejectionsByProvider() to retrieve provider electronic claim rejections
'  R013 05/13/2003 Richkun:  Added MarkUncollectable() to remove claims from denial list
'-------------------------------------------------------------------
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



Public Class CEncounterLogDB

    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property



    Private Const CLASS_NAME As String = "CEncounterLogDB"
    Private Const TABLE_NAME As String = "tblEncounterLog"

    Public Function Fetch(Optional ByVal blnIncludeProcessed As Boolean = False, Optional ByVal strWhere As String = "", Optional ByVal strOrderBy As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/06/2000
        'Author: Eric Pena
        'Description:  Retrieves records from the tblEncounterLog  table.
        'Parameters: blnIncludeDisabled - Optional parameter that identifies if
        '               records flagged as 'Disabled' or 'De-activated' are to be
        '               included in the record set. The default value is False.
        '            strWhere - Optional 'Where' clause of the SQL statement by
        '               which records will be filtered.  If the parameter is not
        '               supplied, all records will be retrieved.
        '             strOrderBy - Optional 'Order By' clause in which retrieved
        '               records will be sorted.  The default is set to the
        '               'fldEncounterLogID' column
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
        If blnIncludeProcessed = False Then
            strDfltWhere = " WHERE fldProcessed = 'N' "
        End If

        'Attach user's where clause if applicable
        strSQL = strSQL & strDfltWhere

        If Trim(strWhere > "") Then If blnIncludeProcessed Then strSQL = strSQL & " WHERE " & strWhere & " " Else strSQL = strSQL & " AND " & strWhere & " "

        If Trim(strOrderBy) > "" Then
            strSQL = strSQL & " ORDER BY "
            strSQL = strSQL & strOrderBy
        Else
            strSQL = strSQL & " ORDER BY "
            strSQL = strSQL & "fldEncounterLogID "
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
        cnnSQL.ActiveConnection = Nothing
        cnnSQL = Nothing
        rstSQL.ActiveConnection = Nothing
        rstSQL = Nothing

        'cmdSQL.Execute()
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function

    Public Function Insert(ByVal intVersionNum As Integer, ByVal lngPatientID As Long, _
            ByVal lngProviderID As Long, ByVal lngClinicID As Long, ByVal dtDOS As Date, _
            ByVal strCPTCode As String, ByVal strModifier1 As String, ByVal strModifier2 As String, _
            ByVal strDSMIV As String, ByVal dblFee As Double, ByVal dblUnits As Double, _
            ByVal strAddOnCPTCode As String, ByVal strAddOnMod1 As String, ByVal strAddOnMod2 As String, _
            ByVal dblAddOnFee As Double, ByVal dblAddOnUnits As Double, _
            ByVal strAddOnSecCPTCode As String, ByVal strAddOnSecMod1 As String, ByVal strAddOnSecMod2 As String, _
            ByVal dblAddOnSecFee As Double, ByVal dblAddOnSecUnits As Double, _
            ByVal strReferPhyLast As String, ByVal strReferPhyFirst As String, ByVal strReferPhyMI As String, _
            ByVal strReferPhyTitle As String, ByVal strReferPhy As String, ByVal strReferPhyNPI As String, _
            ByVal strReferPhyID As String, ByVal dtAdmitDate As Date, ByVal dtDischargeDate As Date, ByVal dtDateOfOnset As Date, _
            ByVal strCondEmpYN As String, ByVal strCondAutoAccYN As String, ByVal strCondAutoAccState As String, _
            ByVal strOtherAccYN As String, ByVal strDoNotBillYN As String, ByVal streScribeYN As String, _
            ByVal intClaimStatus As Integer, ByVal dblRPBalance As Double, _
            ByVal dblPlanBalance As Double, ByVal lngPatApptID As Long, ByVal lngPrevELID As Long, _
            ByVal strAddedBy As String) As Long
        '--------------------------------------------------------------------
        'Date: 07/08/2002
        'Author: Dave Richkun
        'Description:  Inserts a row into the tblEncounterLog table utilizing
        '              a stored procedure.
        'Parameters:
        '       *** TO DO *** Define parameters when procedure finalized
        '
        'Returns: ID of the row inserted.
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspInsEncounterLog"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@EncounterLogID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@VersionNum", adInteger, adParamInput, , intVersionNum))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@ClinicID", adInteger, adParamInput, , lngClinicID))
            .Parameters.Append(.CreateParameter("@DOS", adDBTimeStamp, adParamInput, , dtDOS))
            .Parameters.Append(.CreateParameter("@CPTCode", adVarChar, adParamInput, 10, strCPTCode))
            .Parameters.Append(.CreateParameter("@Modifier1", adVarChar, adParamInput, 2, UCase(strModifier1)))
            .Parameters.Append(.CreateParameter("@Modifier2", adVarChar, adParamInput, 2, UCase(strModifier2)))
            .Parameters.Append(.CreateParameter("@DSMIVCode", adVarChar, adParamInput, 10, strDSMIV))
            .Parameters.Append(.CreateParameter("@Fee", adDecimal, adParamInput, , dblFee))
            .Parameters.Append(.CreateParameter("@Units", adDouble, adParamInput, , dblUnits))
            .Parameters.Append(.CreateParameter("@AddOnCPTCode", adVarChar, adParamInput, 10, strAddOnCPTCode))
            .Parameters.Append(.CreateParameter("@AddOnMod1", adVarChar, adParamInput, 2, UCase(strAddOnMod1)))
            .Parameters.Append(.CreateParameter("@AddOnMod2", adVarChar, adParamInput, 2, UCase(strAddOnMod2)))
            .Parameters.Append(.CreateParameter("@AddOnFee", adDecimal, adParamInput, , dblAddOnFee))
            .Parameters.Append(.CreateParameter("@AddOnUnits", adDouble, adParamInput, , dblAddOnUnits))
            .Parameters.Append(.CreateParameter("@AddOnSecCPTCode", adVarChar, adParamInput, 10, strAddOnSecCPTCode))
            .Parameters.Append(.CreateParameter("@AddOnSecMod1", adVarChar, adParamInput, 2, UCase(strAddOnSecMod1)))
            .Parameters.Append(.CreateParameter("@AddOnSecMod2", adVarChar, adParamInput, 2, UCase(strAddOnSecMod2)))
            .Parameters.Append(.CreateParameter("@AddOnSecFee", adDecimal, adParamInput, , dblAddOnSecFee))
            .Parameters.Append(.CreateParameter("@AddOnSecUnits", adDouble, adParamInput, , dblAddOnSecUnits))
            .Parameters.Append(.CreateParameter("@ReferPhyLast", adVarChar, adParamInput, 30, strReferPhyLast))
            .Parameters.Append(.CreateParameter("@ReferPhyFirst", adVarChar, adParamInput, 20, strReferPhyFirst))
            .Parameters.Append(.CreateParameter("@ReferPhyMI", adVarChar, adParamInput, 1, strReferPhyMI))
            .Parameters.Append(.CreateParameter("@ReferPhyTitle", adVarChar, adParamInput, 10, strReferPhyTitle))
            .Parameters.Append(.CreateParameter("@ReferPhyName", adVarChar, adParamInput, 50, strReferPhy))
            .Parameters.Append(.CreateParameter("@ReferPhyNPI", adVarChar, adParamInput, 10, strReferPhyNPI))
            .Parameters.Append(.CreateParameter("@ReferPhyID", adVarChar, adParamInput, 15, strReferPhyID))
            .Parameters.Append(.CreateParameter("@AdmitDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtAdmitDate.ToOADate()) = 0, DBNull.Value, dtAdmitDate)))
            .Parameters.Append(.CreateParameter("@DischargeDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDischargeDate.ToOADate()) = 0, DBNull.Value, dtDischargeDate)))
            .Parameters.Append(.CreateParameter("@DateOfOnset", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDateOfOnset.ToOADate()) = 0, DBNull.Value, dtDateOfOnset)))
            .Parameters.Append(.CreateParameter("@CondEmplYN", adVarChar, adParamInput, 1, strCondEmpYN))
            .Parameters.Append(.CreateParameter("@CondAutoAccYN", adVarChar, adParamInput, 1, strCondAutoAccYN))
            .Parameters.Append(.CreateParameter("@CondAutoAccStateYN", adVarChar, adParamInput, 2, IIf(strCondAutoAccState = "", DBNull.Value, strCondAutoAccState)))
            .Parameters.Append(.CreateParameter("@CondOtherAccYN", adVarChar, adParamInput, 1, strOtherAccYN))
            .Parameters.Append(.CreateParameter("@CondDoNotBillYN", adVarChar, adParamInput, 1, strDoNotBillYN))
            .Parameters.Append(.CreateParameter("@CondeScribeYN", adVarChar, adParamInput, 1, streScribeYN))
            .Parameters.Append(.CreateParameter("@ClaimStatus", adInteger, adParamInput, , intClaimStatus))
            .Parameters.Append(.CreateParameter("@RPBal", adDouble, adParamInput, , dblRPBalance))
            .Parameters.Append(.CreateParameter("@PlanBal", adDouble, adParamInput, , dblPlanBalance))
            .Parameters.Append(.CreateParameter("@PatApptID", adInteger, adParamInput, , IIf(lngPatApptID < 1, DBNull.Value, lngPatApptID)))
            .Parameters.Append(.CreateParameter("@PrevELID", adInteger, adParamInput, , IIf(lngPrevELID < 1, DBNull.Value, lngPrevELID)))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strAddedBy))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With
        'Acquire the database connection.
        cnnSQL = New ADODB.Connection

        cnnSQL.Open(_ConnectionString)
        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        Insert = cmdSQL.Parameters("@EncounterLogID").Value
        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
            Exit Function
        End If

        'Close the connection
        cnnSQL.Close()

        'Free all resources
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion

        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()

        'RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME)
    End Function

    Public Sub Update(ByVal lngELID As Long, ByVal lngPatientID As Long, _
                ByVal lngProviderID As Long, ByVal lngClinicID As Long, ByVal dtDOS As Date, _
                ByVal strCPTCode As String, ByVal strModifier1 As String, ByVal strModifier2 As String, _
                ByVal strDSMIV As String, ByVal dblFee As Double, ByVal dblUnits As Double, _
                ByVal strAddOnCPTCode As String, ByVal strAddOnMod1 As String, ByVal strAddOnMod2 As String, _
                ByVal dblAddOnFee As Double, ByVal dblAddOnUnits As Double, _
                ByVal strAddOnSecCPTCode As String, ByVal strAddOnSecMod1 As String, ByVal strAddOnSecMod2 As String, _
                ByVal dblAddOnSecFee As Double, ByVal dblAddOnSecUnits As Double, _
                ByVal strReferPhyLast As String, ByVal strReferPhyFirst As String, ByVal strReferPhyMI As String, _
                ByVal strReferPhyTitle As String, ByVal strReferPhy As String, ByVal strReferPhyNPI As String, _
                ByVal strReferPhyID As String, ByVal dtAdmitDate As Date, ByVal dtDischargeDate As Date, ByVal dtDateOfOnset As Date, _
                ByVal strCondEmpYN As String, ByVal strCondAutoAccYN As String, ByVal strCondAutoAccState As String, _
                ByVal strOtherAccYN As String, ByVal strDoNotBillYN As String, ByVal streScribeYN As String, _
                ByVal intClaimStatus As Integer, ByVal dblRPBalance As Double, _
                ByVal dblPlanBalance As Double, ByVal lngPatApptID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 08/02/2002
        'Author: Dave Richkun
        'Description:  Updates a row in the tblEncounterLog table utilizing a stored procedure.
        'Parameters:
        '
        '  *** TO DO:  Define all parameters when procedure finalized ****
        '
        'Returns: Null
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdEncounterLog"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@EncounterLogID", adInteger, adParamInput, , lngELID))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@ClinicID", adInteger, adParamInput, , lngClinicID))
            .Parameters.Append(.CreateParameter("@DOS", adDBTimeStamp, adParamInput, , dtDOS))
            .Parameters.Append(.CreateParameter("@CPTCode", adVarChar, adParamInput, 10, strCPTCode))
            .Parameters.Append(.CreateParameter("@Modifier1", adVarChar, adParamInput, 2, UCase(strModifier1)))
            .Parameters.Append(.CreateParameter("@Modifier2", adVarChar, adParamInput, 2, UCase(strModifier2)))
            .Parameters.Append(.CreateParameter("@DSM_IV", adVarChar, adParamInput, 10, strDSMIV))
            .Parameters.Append(.CreateParameter("@Fee", adDecimal, adParamInput, , dblFee))
            .Parameters.Append(.CreateParameter("@Units", adDouble, adParamInput, , dblUnits))
            .Parameters.Append(.CreateParameter("@AddOnCPTCode", adVarChar, adParamInput, 10, strAddOnCPTCode))
            .Parameters.Append(.CreateParameter("@AddOnMod1", adVarChar, adParamInput, 2, UCase(strAddOnMod1)))
            .Parameters.Append(.CreateParameter("@AddOnMod2", adVarChar, adParamInput, 2, UCase(strAddOnMod2)))
            .Parameters.Append(.CreateParameter("@AddOnFee", adDecimal, adParamInput, , dblAddOnFee))
            .Parameters.Append(.CreateParameter("@AddOnUnits", adDouble, adParamInput, , dblAddOnUnits))
            .Parameters.Append(.CreateParameter("@AddOnSecCPTCode", adVarChar, adParamInput, 10, strAddOnSecCPTCode))
            .Parameters.Append(.CreateParameter("@AddOnSecMod1", adVarChar, adParamInput, 2, UCase(strAddOnSecMod1)))
            .Parameters.Append(.CreateParameter("@AddOnSecMod2", adVarChar, adParamInput, 2, UCase(strAddOnSecMod2)))
            .Parameters.Append(.CreateParameter("@AddOnSecFee", adDecimal, adParamInput, , dblAddOnSecFee))
            .Parameters.Append(.CreateParameter("@AddOnSecUnits", adDouble, adParamInput, , dblAddOnSecUnits))
            .Parameters.Append(.CreateParameter("@ReferPhyLast", adVarChar, adParamInput, 30, strReferPhyLast))
            .Parameters.Append(.CreateParameter("@ReferPhyFirst", adVarChar, adParamInput, 20, strReferPhyFirst))
            .Parameters.Append(.CreateParameter("@ReferPhyMI", adVarChar, adParamInput, 1, strReferPhyMI))
            .Parameters.Append(.CreateParameter("@ReferPhyTitle", adVarChar, adParamInput, 10, strReferPhyTitle))
            .Parameters.Append(.CreateParameter("@ReferPhyName", adVarChar, adParamInput, 50, strReferPhy))
            .Parameters.Append(.CreateParameter("@ReferPhyNPI", adVarChar, adParamInput, 10, strReferPhyNPI))
            .Parameters.Append(.CreateParameter("@ReferPhyID", adVarChar, adParamInput, 15, strReferPhyID))
            .Parameters.Append(.CreateParameter("@AdmitDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtAdmitDate.ToOADate()) = 0, DBNull.Value, dtAdmitDate)))
            .Parameters.Append(.CreateParameter("@DischargeDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDischargeDate.ToOADate()) = 0, DBNull.Value, dtDischargeDate)))
            .Parameters.Append(.CreateParameter("@DateOfOnset", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDateOfOnset.ToOADate()) = 0, DBNull.Value, dtDateOfOnset)))
            .Parameters.Append(.CreateParameter("@CondEmpYN", adVarChar, adParamInput, 1, strCondEmpYN))
            .Parameters.Append(.CreateParameter("@CondAutoAccYN", adVarChar, adParamInput, 1, strCondAutoAccYN))
            .Parameters.Append(.CreateParameter("@CondAutoAccState", adVarChar, adParamInput, 2, IIf(strCondAutoAccState > "", strCondAutoAccState, DBNull.Value)))
            .Parameters.Append(.CreateParameter("@CondAccOtherYN", adVarChar, adParamInput, 1, strOtherAccYN))
            .Parameters.Append(.CreateParameter("@CondDoNotBillYN", adVarChar, adParamInput, 1, strDoNotBillYN))
            .Parameters.Append(.CreateParameter("@CondeScribeYN", adVarChar, adParamInput, 1, streScribeYN))
            .Parameters.Append(.CreateParameter("@ClaimStatus", adInteger, adParamInput, , intClaimStatus))
            .Parameters.Append(.CreateParameter("@RPBal", adDouble, adParamInput, , dblRPBalance))
            .Parameters.Append(.CreateParameter("@PlanBal", adDouble, adParamInput, , dblPlanBalance))
            .Parameters.Append(.CreateParameter("@PatApptID", adInteger, adParamInput, , IIf(lngPatApptID < 1, DBNull.Value, lngPatApptID)))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        'Close the connection and free all resources
        cnnSQL.Close()

        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Sub


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL = Nothing
        cnnSQL = Nothing

        cmdSQL.Execute()
        'RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, cmdSQL.Parameters("@ErrorMsg").Value)

    End Sub


    Public Sub Disable(ByVal lngELID As Long)
        '--------------------------------------------------------------------
        'Date: 08/08/2002
        'Author: Dave Richkun
        'Description:  Disables a row in tblEncounterLog and sets the claim status
        '              to Cancelled
        'Parameters: lngELID - ID of the row in tblEncounterLog to disable
        'Returns: Null
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdEncLogDisable"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@EncounterLogID", adInteger, adParamInput, , lngELID))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        'Close the connection and free all resources
        cnnSQL.Close()

        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Sub


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL = Nothing
        cnnSQL = Nothing

        cmdSQL.Execute()
        'RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, cmdSQL.Parameters("@ErrorMsg").Value)

    End Sub




    Public Function isProcessed(ByVal lngID As Long) As Boolean
        '--------------------------------------------------------------------
        'Date: 03/06/2000
        'Author: Eric Pena
        'Description:  Lets the er know if a given EncounterLog Entry has been processed
        'Parameters: lngID - The ID of the EL Entry
        'Returns: T if the association is processed, F otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '  R010
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim strSQL As String
        isProcessed = False



        'Prepare the SQL statement.
        strSQL = "SELECT "
        strSQL = strSQL & "fldProcessed, "
        strSQL = strSQL & "fldBillStatusID "  'R010
        strSQL = strSQL & "FROM "
        strSQL = strSQL & "tblEncounterLog "
        strSQL = strSQL & " WHERE "
        strSQL = strSQL & "fldEncounterLogID = " & lngID & " "

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Instantiate and populate the Recordset.
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(strSQL, cnnSQL, adOpenForwardOnly, adLockReadOnly, adCmdText + adAsyncFetch)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rstSQL.ActiveConnection = Nothing
        cnnSQL.Close()
        cnnSQL = Nothing

        'set to null if there are no current associations (new Benefactor)
        If rstSQL.RecordCount = 0 Then
            GoTo EMPTY_RECORDSET
        End If
        If rstSQL.Fields("fldProcessed").Value = "Y" And rstSQL.Fields("fldBillStatusID").Value > 2 Then
            isProcessed = True
        End If
EMPTY_RECORDSET:
        'Signal successful completion

        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        rstSQL = Nothing
        cnnSQL = Nothing
        'cmdSQL.Execute()
        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function

    Public Function FetchByDOS(ByVal lngProviderID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 5/05/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblEncounterLog table ordered by date of svc utilizing a stored procedure
        'Parameters: lngProviderID - id of the provider to return records for
        'Returns: ADORst
        '--------------------------------------------------------------------
        Dim rs As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        rs = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelProviderEncountersByDOS"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, 1, lngProviderID))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByDOS = rs

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rs.ActiveConnection = Nothing
        cnn = Nothing
        'Signal successful completion


        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rs.ActiveConnection = Nothing
        rs = Nothing
        cnn = Nothing
        'cmdSQL.Execute()
        'RaiseError(Err, Err.Number, CLASS_NAME)
    End Function

    Public Function FetchByName(ByVal lngProviderID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 5/05/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblEncounterLog table ordered by provider name utilizing a stored procedure
        'Parameters: lngProviderID - id of the provider to return records for
        'Returns: ADORst
        '--------------------------------------------------------------------
        Dim rs As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection




        rs = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelProviderEncountersByName"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, 1, lngProviderID))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByName = rs

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rs.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rs.ActiveConnection = Nothing
        rs = Nothing
        cnn = Nothing
        'cmdSQL.Execute()
        'RaiseError(Err, Err.Number, CLASS_NAME)
    End Function

    Public Sub Delete(ByVal lngID As Long)
        '--------------------------------------------------------------------
        'Date: 5/10/2000
        'Author: Eric Pena
        'Description:  Physiy deletes entries in tblEncounterLog.  This can only be done if the encounter has not yet been processed.
        'Parameters: lngID - ID of the row in the table whose value will be updated.
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
            .CommandText = "uspDelEncounterLog"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@EncounterLogID", adInteger, adParamInput, , lngID))
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


        Exit Sub


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()
        'RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, cmdSQL.Parameters("@ErrorMsg").Value)
    End Sub

    Public Sub DeleteBeforeSubmit(ByVal lngELID As Long, ByVal lngPatApptID As Long, _
                    ByVal blnDelAppt As Boolean, ByVal lngPatNoteID As Long, _
                    ByVal blnDelNote As Boolean, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 07/26/2002
        'Author: Dave Richkun
        'Description:  Physiy deletes an encounter and related rows from the database.
        'Parameters: lngELID - ID of the row in tblEncounterLog that will be deleted
        '            lngPatApptID - ID of the row in tblPatientAppt that will be deleted
        '            blnDelAppt - Identifies if patient appoint is to be deleted rather than reset
        '            lngPatNoteID - ID of the row in tblPatientNote that will be deleted
        '            strUserName - Name of user executing this method
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
            .CommandText = "uspDelEncounterLogBeforeSubmit"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@EncounterLogID", adInteger, adParamInput, , lngELID))
            .Parameters.Append(.CreateParameter("@PatApptID", adInteger, adParamInput, , lngPatApptID))
            .Parameters.Append(.CreateParameter("@DelPatAppt", adVarChar, adParamInput, 1, IIf(blnDelAppt = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@PatNoteID", adInteger, adParamInput, , lngPatNoteID))
            .Parameters.Append(.CreateParameter("@DelPatNote", adVarChar, adParamInput, 1, IIf(blnDelNote = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName))
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


        Exit Sub


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()
        'RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, cmdSQL.Parameters("@ErrorMsg").Value)

    End Sub


    Public Sub CancelAfterSubmit(ByVal lngELID As Long, ByVal lngPatApptID As Long, _
               ByVal blnDelAppt As Boolean, ByVal lngPatNoteID As Long, _
               ByVal blnDelNote As Boolean, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 07/26/2002
        'Author: Dave Richkun
        'Description:  Cancels an encounter and updates/deletes related rows from the database.
        'Parameters: lngELID - ID of the row in tblEncounterLog that will be cancelled
        '            lngPatApptID - ID of the row in tblPatientAppt that will be reset
        '            blnDelAppt - Identifies if patient appoint is to be deleted rather than reset
        '            lngPatNoteID - ID of the row in tblPatientNote that will be deleted
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
            .CommandText = "uspUpdCancelEncLogAfterSubmit"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@EncounterLogID", adInteger, adParamInput, , lngELID))
            .Parameters.Append(.CreateParameter("@PatApptID", adInteger, adParamInput, , lngPatApptID))
            .Parameters.Append(.CreateParameter("@DelPatAppt", adVarChar, adParamInput, 1, IIf(blnDelAppt = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@PatNoteID", adInteger, adParamInput, , lngPatNoteID))
            .Parameters.Append(.CreateParameter("@DelPatNote", adVarChar, adParamInput, 1, IIf(blnDelNote = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName))
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


        Exit Sub


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()
        'RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, cmdSQL.Parameters("@ErrorMsg").Value)

    End Sub





    Public Function FetchByProvDOS(ByVal lngProviderID As Long, ByVal dteFrom As Date, ByVal dteTo As Date) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 5/15/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblEncounterLog table ordered by provider name utilizing a stored procedure
        'Parameters: lngProviderID - id of the provider to return records for
        '                    dteFrom/dteTo - the date range of DOS to search for
        'Returns: ADORst
        '--------------------------------------------------------------------
        Dim rs As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        rs = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelEncountersByProvDOS"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, 1, lngProviderID))
            .Parameters.Append(.CreateParameter("@FromDate", adDBTimeStamp, adParamInput, , dteFrom))
            .Parameters.Append(.CreateParameter("@ToDate", adDBTimeStamp, adParamInput, , dteTo))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByProvDOS = rs

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rs.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rs.ActiveConnection = Nothing
        rs = Nothing
        cnn = Nothing
        'cmdSQL.Execute()
        'RaiseError(Err, Err.Number, CLASS_NAME)
    End Function

    Public Function FetchStatus(ByVal varProviderID As Object, ByVal dteFrom As Date, ByVal dteTo As Date, Optional ByVal blnProcessed As Object = "", Optional ByVal lngPatientID As Long = -1) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/06/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblEncounterLog table ordered by date of svc utilizing a stored procedure
        'Parameters: varProviderID - array of ids of the providers to return records for
        '                    dtefrom/dteto - date range for DOS to search for
        '                    blnProcessed - true if filter for only processed encounters
        '                    dtefrom/lngPatientID - optional patient id to filter for
        'Returns: ADORst
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim strDfltWhere As String
        Dim strSQL As String
        Dim intCtr As Integer



        'Prepare the SQL statement.
        strSQL = "SELECT "
        strSQL = strSQL & TABLE_NAME & ".fldEncounterLogID, "
        strSQL = strSQL & TABLE_NAME & ".fldProviderID, "
        strSQL = strSQL & TABLE_NAME & ".fldUserName, "
        strSQL = strSQL & TABLE_NAME & ".fldDateAdded, "
        strSQL = strSQL & TABLE_NAME & ".fldPatientID, "
        strSQL = strSQL & "fldLast, "
        strSQL = strSQL & "fldFirst, "
        strSQL = strSQL & "fldLast + ', ' + fldfirst AS fldFullName, "
        strSQL = strSQL & TABLE_NAME & ".fldClinicID, "
        strSQL = strSQL & "fldBusinessName, "
        strSQL = strSQL & TABLE_NAME & ".fldDOS, "
        strSQL = strSQL & TABLE_NAME & ".fldCPTCode, "
        strSQL = strSQL & TABLE_NAME & ".fldModifier, "
        strSQL = strSQL & TABLE_NAME & ".fldModifier2, "
        strSQL = strSQL & TABLE_NAME & ".fldDSM_IV, "
        strSQL = strSQL & "fldCode, "
        strSQL = strSQL & TABLE_NAME & ".fldFee, "
        strSQL = strSQL & TABLE_NAME & ".fldUnits, "
        strSQL = strSQL & TABLE_NAME & ".fldProcessed, "
        strSQL = strSQL & TABLE_NAME & ".fldDateProcessed, "
        strSQL = strSQL & TABLE_NAME & ".fldFreeTextCertNum, "
        strSQL = strSQL & TABLE_NAME & ".fldReferPhy, "
        strSQL = strSQL & TABLE_NAME & ".fldReferPhyID, "
        strSQL = strSQL & TABLE_NAME & ".fldAdmitDate, "
        strSQL = strSQL & TABLE_NAME & ".fldDischargeDate, "
        strSQL = strSQL & TABLE_NAME & ".fldDateOfOnset "
        strSQL = strSQL & "FROM tblEncounterLog, tblBenefactor, tblClinic, tblDSM_IV "
        strSQL = strSQL & "WHERE (fldPatientID = fldBenefactorID AND tblEncounterLog.fldClinicID = tblClinic.fldClinicID AND fldDSM_IV = fldDSM_IV_ID "
        strSQL = strSQL & "AND fldDOS between '" & dteFrom & "' AND '" & dteTo & "' "
        If lngPatientID > 0 Then
            strSQL = strSQL & "AND FldPatientID = " & lngPatientID
        End If



        If Not IsDBNull(blnProcessed) Then

            If blnProcessed Then
                strSQL = strSQL & " AND fldProcessed <> 'N' " 'R001
            Else 'R001
                strSQL = strSQL & " AND fldProcessed = 'N' "
            End If


        End If


        'If Not IsMissing(blnProcessed) Then
        '    'R001 - If the blnProcessed parameter is NULL then return all records, regardless
        '    'of their status.
        '    If blnProcessed Then
        '        strSQL = strSQL & " AND fldProcessed <> 'N' " 'R001
        '    Else 'R001
        '        strSQL = strSQL & " AND fldProcessed = 'N' "
        '    End If
        'End If

        If IsArray(varProviderID) Then 'R001
            For intCtr = 0 To UBound(varProviderID)
                If (varProviderID(intCtr).IsNullorEmpty) Or (varProviderID(intCtr)) = "" Then GoTo nextpass
                If intCtr = 0 Then
                    strSQL = strSQL & ") AND ( "
                Else
                    strSQL = strSQL & "OR  "
                End If
                strSQL = strSQL & "fldProviderID = " & varProviderID(intCtr) & " "
nextpass:
            Next intCtr

            strSQL = strSQL & ")  ORDER BY fldFullName, fldDOS"
        Else 'R001
            strSQL = strSQL & " AND fldProviderID = " & varProviderID & ")" 'R001
        End If 'R001

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

        FetchStatus = rstSQL
        'Signal successful completion


        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        rstSQL.ActiveConnection = Nothing
        rstSQL = Nothing
        cnnSQL = Nothing
        'cmdSQL.Execute()
        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function


    Public Function FetchForPatient(ByVal lngPatientID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/07/2000
        'Author: Dave Richkun
        'Description:  Returns rows from the tblEncounterLog table associated
        '              with a specific patient.
        'Parameters: lngPatientID - ID of the patient whose Encounter records are
        '                     being retrieved.
        'Returns: Recordset of matching rows
        '--------------------------------------------------------------------
        Dim rst As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        cmd = New ADODB.Command
        'Create the parameter objects
        With cmd
            .CommandText = "uspSelPatientEncounters"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
        End With

        cnn = New ADODB.Connection
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Execute the stored procedure
        rst = New ADODB.Recordset
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchForPatient = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        rst.ActiveConnection = Nothing
        cmd = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        cmd = Nothing
        cnn = Nothing
        rst = Nothing
        'RaiseError(Err, Err.Number, CLASS_NAME)

    End Function

    Public Function FetchByID(ByVal lngEncLogID As Long, Optional ByVal strDataBase As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/07/2000
        'Author: Dave Richkun
        'Description:  Returns a row from the tblEncounterLog table associated
        '              with a specific Encounter.
        'Parameters: lngEncLogID - ID of the Encounter record to retrieve.
        'Returns: Recordset of matching rows
        '--------------------------------------------------------------------
        Dim rst As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        cmd = New ADODB.Command
        'Create the parameter objects
        With cmd
            .CommandText = "uspSelEncounterByID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@EncLogID", adInteger, adParamInput, , lngEncLogID))
        End With

        cnn = New ADODB.Connection

        cnn.Open(_ConnectionString)

        cmd.ActiveConnection = cnn

        'Execute the stored procedure
        rst = New ADODB.Recordset
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByID = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        rst.ActiveConnection = Nothing
        cmd = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        cmd = Nothing
        cnn = Nothing
        rst = Nothing
        'RaiseError(Err, Err.Number, CLASS_NAME)

    End Function


    Public Function FetchByUserEnteredToday(ByVal strUserName As String) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/07/2000
        'Author: Dave Richkun
        'Description:  Returns rows from the tblEncounterLog table entered
        '              by a specific user on the system date.
        'Parameters: strUserName - Username of the user who entered the encounters
        'Returns: Recordset of matching rows
        '--------------------------------------------------------------------
        Dim rst As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        cmd = New ADODB.Command
        'Create the parameter objects
        With cmd
            .CommandText = "uspSelEncounterByUserToday"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName))
        End With

        cnn = New ADODB.Connection
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Execute the stored procedure
        rst = New ADODB.Recordset
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByUserEnteredToday = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        rst.ActiveConnection = Nothing
        cmd = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        cmd = Nothing
        cnn = Nothing
        rst = Nothing
        'RaiseError(Err, Err.Number, CLASS_NAME)

    End Function

    Public Function FetchByHour() As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/07/2000
        'Author: Eric Pena
        'Description:  Returns rows from tblEncounterLog added by the previous hour
        'Parameters:
        'Returns: ADORst
        '--------------------------------------------------------------------
        Dim rs As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        rs = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelEncounterByHour"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByHour = rs

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rs.ActiveConnection = Nothing
        cnn = Nothing
        'Signal successful completion


        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        'RaiseError(Err, cmd.Parameters("@ErrorNum").Value, CLASS_NAME)
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rs.ActiveConnection = Nothing
        rs = Nothing
        cnn = Nothing
        'cmdSQL.Execute()
    End Function

    '--Begin R005-----------------
    Public Function FetchForPatientUser(ByVal lngPatientID As Long, _
                        ByVal lngUserID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 02/12/2001
        'Author: Dave Richkun
        'Description:  Returns rows from the tblEncounterLog table associated
        '              with a specific patient and within the context of the
        '              logged in user.  This method takes into consideraton whether
        '              the patient in question is shared between Providers.  If a
        '              patient is shared then Encounter records for all shared Providers
        '              are returned.  If the patient is not shared, then only records
        '              associated with the current user are returned.
        'Parameters: lngPatientID - ID of the patient whose Encounter records are
        '                     being retrieved.
        '            lngUserID - ID of the user making the  to this object.
        'Returns: Recordset of matching rows
        '--------------------------------------------------------------------
        Dim rst As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        cmd = New ADODB.Command
        'Create the parameter objects
        With cmd
            .CommandText = "uspSelPatientEncountersByUser"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
        End With

        cnn = New ADODB.Connection
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Execute the stored procedure
        rst = New ADODB.Recordset
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchForPatientUser = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        rst.ActiveConnection = Nothing
        cmd = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        cmd = Nothing
        cnn = Nothing
        rst = Nothing
        'RaiseError(Err, Err.Number, CLASS_NAME)

    End Function
    '--End R005-----------------

    '--Begin R006-----------------
    Public Function FetchSeekNAssign(ByVal lngCertID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/01/2001                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Returns rows from the tblEncounterLog table associated
        '              that may qualify for a new certification.            '
        'Parameters: lngCertID - ID of the cert to check encounters against '
        'Returns: Recordset of matching rows                                '
        '--------------------------------------------------------------------
        Dim rst As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        cmd = New ADODB.Command
        'Create the parameter objects
        With cmd
            .CommandText = "uspSelEncountersSeekNAssign"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@CertID", adInteger, adParamInput, , lngCertID))
        End With

        cnn = New ADODB.Connection
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Execute the stored procedure
        rst = New ADODB.Recordset
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchSeekNAssign = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        rst.ActiveConnection = Nothing
        cmd = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        cmd = Nothing
        cnn = Nothing
        rst = Nothing
        'RaiseError(Err, Err.Number, CLASS_NAME)

    End Function
    '--End R006-----------------

    Public Function FetchBRByEL(ByVal lngEncounterLogID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 02/06/2002
        'Author: Dave Richkun
        'Description:  Returns rows from tblBillingresponsibility associated with a specific Encounter.
        'Parameters: lngEncLogID - ID of the Encounter record to retrieve.
        'Returns: Recordset of matching rows
        '--------------------------------------------------------------------
        Dim rs As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        rs = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelBR"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ELID", adInteger, adParamInput, , lngEncounterLogID))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchBRByEL = rs

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rs.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        'RaiseError(Err, cmd.Parameters("@ErrorNum").Value, CLASS_NAME)
        cmd = Nothing
        rs = Nothing
        cnn = Nothing
        'cmdSQL.Execute()
    End Function

    Public Function FetchClaimDetailByEncID(ByVal lngEncLogID As Long, Optional ByVal strDataBase As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/07/2019
        'Author: Duane C Orth
        'Description:  Returns a row from the tblEncounterLog table associated
        '              with a specific Encounter.
        'Parameters: lngEncLogID - ID of the Encounter record to retrieve.
        'Returns: Recordset of matching rows
        '--------------------------------------------------------------------
        Dim rst As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        cmd = New ADODB.Command
        'Create the parameter objects
        With cmd
            .CommandText = "uspRptClaimDetailByEncID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ELID", adInteger, adParamInput, , lngEncLogID))
        End With

        cnn = New ADODB.Connection
 
        cnn.Open(_ConnectionString)

        cmd.ActiveConnection = cnn

        'Execute the stored procedure
        rst = New ADODB.Recordset
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchClaimDetailByEncID = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        rst.ActiveConnection = Nothing
        cmd = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        cmd = Nothing
        cnn = Nothing
        rst = Nothing
        'RaiseError(Err, Err.Number, CLASS_NAME)

    End Function
    Public Function FetchClaimDetailByClaimID(ByVal lngClaimID As Long, Optional ByVal strDataBase As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/20/2020
        'Author: Duane C Orth
        'Description:  Returns a row from the tblEncounterLog table associated
        '              with a specific Encounter.
        'Parameters: lngClaimID - ID of the Encounter record to retrieve.
        'Returns: Recordset of matching rows
        '--------------------------------------------------------------------
        Dim rst As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        cmd = New ADODB.Command
        'Create the parameter objects
        With cmd
            .CommandText = "uspRptClaimDetailByClaimID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ClaimID", adInteger, adParamInput, , lngClaimID))
        End With

        cnn = New ADODB.Connection

        cnn.Open(_ConnectionString)

        cmd.ActiveConnection = cnn

        'Execute the stored procedure
        rst = New ADODB.Recordset
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchClaimDetailByClaimID = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        rst.ActiveConnection = Nothing
        cmd = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function

        'Signal incompletion and raise the error to the ing environment.

        cmd = Nothing
        cnn = Nothing
        rst = Nothing
        'RaiseError(Err, Err.Number, CLASS_NAME)

    End Function
    Public Function ValidateCodes(ByVal strCPTCode As String, ByVal strDSMCode As String, ByVal lngPatientID As Long, ByVal lngProviderID As Long, ByVal dtDOS As Date) As Integer
        '--------------------------------------------------------------------
        'Date: 07/18/2002
        'Author: Dave Richkun
        'Description:  Performs server-side validation for Encounter/Claim entry.  This
        '              method ensures the CPT Code and DSM-IV Code are valid, and checks
        '              for duplicate encounters within a single .  The stored procedure
        '              returns a 'bit-representation' integer that identifies the error
        '              condition based on the following 'bit' settings:
        '              0 - No errors
        '              1 - CPT Code is invalid
        '              2 - DSM-IV Code is invalid
        '              4 - Duplicate encounter exists
        'Parameters:  strCPTCode - CPT Code assigned to service date
        '             strDSMCode - DSM-IV Code assigned to patient on service date
        '             lngPatientID  - ID of patient receiving service
        '             lngProviderID - ID of provider servicing patient
        '             dtDOS - Date service was performed
        'Returns: Integer identifying error code
        '--------------------------------------------------------------------
        Dim rst As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection
        Dim lngErrorCode As Long



        rst = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection

        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelELogValidation"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ErrorCode", adInteger, adParamReturnValue, , lngErrorCode))
            .Parameters.Append(.CreateParameter("@CPTCode", adVarChar, adParamInput, 10, Trim(strCPTCode)))
            .Parameters.Append(.CreateParameter("@DSMIVCode", adVarChar, adParamInput, 10, Trim(strDSMCode)))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@DOS", adDBTimeStamp, adParamInput, , dtDOS))
        End With

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        lngErrorCode = cmd.Parameters("@ErrorCode").Value

        Return lngErrorCode




    End Function


    Public Function FetchTxsByEL(ByVal lngEncounterLogID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/23/2002
        'Author: Dave Richkun
        'Description:  Returns rows from tblInsuranceTx and tblPatientTx associated
        '              with a specific Encounter.
        'Parameters: lngEncLogID - ID of the Encounter record.
        'Returns: Recordset of matching rows
        '--------------------------------------------------------------------
        Dim rst As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        rst = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection

        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelTxsByELID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ELID", adInteger, adParamInput, , lngEncounterLogID))
        End With

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)



        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing


        Return rst



    End Function

    Public Sub UpdateEncounterStatus(ByVal lngELID As Long, ByVal intClaimStatus As Integer)
        '--------------------------------------------------------------------
        'Date: 10/16/2002
        'Author: Dave Richkun
        'Description:  Updates the status of an encounter.
        'Parameters:  lngELID - ID of the claim
        '             intVerNum - Version number of claim
        '             intClaimStatus - Status to which claim is being updated
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
            .CommandText = "uspUpdEncounterLogStatus"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@EncounterLogID", adInteger, adParamInput, , lngELID))
            .Parameters.Append(.CreateParameter("@StatusID", adInteger, adParamInput, , intClaimStatus))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()


        cnnSQL.Close()

        cmdSQL = Nothing
        cnnSQL = Nothing

 

    End Sub


    Public Function FetchRejectionsByProvider(ByVal lngProviderID As Long) As ADODB.Recordset 'R013
        '--------------------------------------------------------------------
        'Date: 03/05/2003
        'Author: Dave Richkun
        'Description:  Returns claims with a status of 'Rejected'
        'Parameters: lngProviderID - ID of the provider whose rejects are being returned.
        'Returns: ADO Recordset
        '--------------------------------------------------------------------

        Dim rst As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        rst = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection

        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelRejectionsByProvider"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
        End With

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)



        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion
        Return rst

        Exit Function

    End Function

    Public Function FetchNotesMissingByProvider(ByVal lngProviderID As Long) As ADODB.Recordset 'R013
        '--------------------------------------------------------------------
        'Date: 03/05/2003
        'Author: Dave Richkun
        'Description:  Returns claims with a status of 'Rejected'
        'Parameters: lngProviderID - ID of the provider whose rejects are being returned.
        'Returns: ADO Recordset
        '--------------------------------------------------------------------

        Dim rst As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        rst = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection

        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelNotesMissingByProvider"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
        End With

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)



        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing


        Return rst





    End Function

    Public Sub MarkUncollectable(ByVal lngELID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 05/13/2003
        'Author: Dave Richkun
        'Description:  Marks a claim as Uncollectable
        'Parameters:  lngELID - ID of the claim
        '             strUserName - Name of the user executing this method
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
            .CommandText = "uspUpdClaimUncollectable"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@EncounterLogID", adInteger, adParamInput, , lngELID))
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Close the connection and free all resources
        cnnSQL.Close()

        cmdSQL = Nothing
        cnnSQL = Nothing


    End Sub

    Public Sub UnCancelEncounter(ByVal lngELID As Long, ByVal lngPatientID As Long)
        '--------------------------------------------------------------------
        'Date: 11/02/2007
        'Author: Duane C Orth
        'Description:  Uncancels and Encounter
        'Parameters:  lngELID - ID of the Encounter
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
            .CommandText = "uspUnCancelEncounter"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@EncounterLogID", adInteger, adParamInput, , lngELID))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()



        'Close the connection and free all resources
        cnnSQL.Close()

        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion




    End Sub
    Public Sub AddSecondaryInsurance(ByVal lngELID As Long, ByVal lngPatientID As Long, ByVal intOrder As Integer)
        '--------------------------------------------------------------------
        'Date: 11/02/2007
        'Author: Duane C Orth
        'Description:  Adds the Secondary Insurance to the Encounter
        'Parameters:  lngELID - ID of the Encounter
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
            .CommandText = "uspAddSecondaryInsurance"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@EncounterLogID", adInteger, adParamInput, , lngELID))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@Order", adInteger, adParamInput, , intOrder))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Close the connection and free all resources
        cnnSQL.Close()

        cmdSQL = Nothing
        cnnSQL = Nothing


    End Sub

End Class
