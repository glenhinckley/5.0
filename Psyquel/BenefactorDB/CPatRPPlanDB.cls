
'--------------------------------------------------------------------
'Class Name: CPatRPPlanDB
'Date: 02/06/2000
'Author: Eric Pena
'Description:  MTS object designed to allow insertions into the tblPatRPPlan table.
'--------------------------------------------------------------------
'Revision History:
' R001 - 11/07/2000 Travis: Moved FetchPatRPPlanRule to CPatRPPlanRuleDB
' R002 - 06/15/2000 Richkun: Altered IsEnabled() interface to return
'       ID of found record, rather than a boolean value.
' R003 - 06/15/2000 Richkun:  Altered SetOrder() to use Stored Procedure
'           that will re-enable plans that may have been disabled.
' R004 - 07/24/2000 Richkun: Added Append() method.
' R005 - 07/26/2000 Richkun: Altered Deleted() to use stored procedure
' R006 - 11/07/2000 Travis: Moved Update to CPatRPPlanRuleDB
' R007 - 01/12/2001 Nichol: Added ReorderPlan()
' R008 - 03/22/2001 Nichol: Added ExistsPatRPPlan()
' R009 - 05/16/2001 Richkun: Added FetchDisabledRPPlansByPatient()
' R010 - 01/24/2002 Richkun: Added FetchPatientRPNames()
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
' Lydia Orth 


Imports Psyquel.BusinessRules.CoreLibraryIII


Public Class CPatRPPlanDB

    Private _DB As New dbStuff.db
    Private _MD As New dbStuff.ModCommon

    Private _ConnectionString As String = String.Empty

    Private Const TABLE_NAME As String = "tblPatRPPlan"
    Private Const CLASS_NAME As String = "CPatRPPlanDB"


    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property





    Public Function FetchPlans(ByVal lngPatientID As Long, ByVal lngRPID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 02/06/2000
        'Author: Eric Pena
        'Description:  Returns a ADODB.Recordset containing a list of all Plans to which a Patient-RP combo is associated.
        'Parameters: lngPatientID - The ID of the Patient
        '                    lngRPID - The ID of the RP
        'Returns: An ADODB.Recordset containing a list of all plans and levels  to which a Patient-RP combo is associated.
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim strSQL As String
        Dim strList As String
        Dim intCTR As Integer



        'Prepare the SQL statement.
        strSQL = "SELECT "
        strSQL = strSQL & "fldPlanID, "
        strSQL = strSQL & "fldOrder, "
        strSQL = strSQL & "fldDisabledYN "
        strSQL = strSQL & "FROM "
        strSQL = strSQL & "tblPatRPPlan "
        strSQL = strSQL & " WHERE "
        strSQL = strSQL & "fldPatientID = " & lngPatientID & " AND "
        strSQL = strSQL & "fldRPID = " & lngRPID

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

        FetchPlans = rstSQL

        'Signal successful completion


        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        rstSQL.ActiveConnection = Nothing
        rstSQL = Nothing
        cnnSQL = Nothing
        'cmdSQL.Execute()
        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function
    Public Function ExistsPatRPPlan(ByVal lngPatientID As Long, ByVal lngRPID As Long, ByVal lngPlanID As Long, _
                                Optional ByVal blnIncludeDisabled As Boolean = False) As Long
        '--------------------------------------------------------------------
        'Date: 3/21/2001
        'Author: David Nichol
        'Description:  If an patient/RP/plan combination already exists, returns its ID in tblPatientRPPlan.
        '               Otherwise it returns 0.
        'Parameters: lngPatientID - patient ID
        '            lngRPID - responsible party ID
        '            lngPlanID - plan ID
        '            blnIncludeDisabled - Y/N as to whether to include disabled records, defaulting to false
        'Returns: Long - ID of the existing record, or 0
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim strSQL As String
        Dim strList As String
        Dim intCTR As Integer



        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        With cmdSQL
            .CommandText = "uspExistsPatRPPlan"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatRPPlanID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@RPID", adInteger, adParamInput, , lngRPID))
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID))
            .Parameters.Append(.CreateParameter("@IncludeDisabledYN", adVarChar, adParamInput, 1, IIf(blnIncludeDisabled = True, "Y", "N")))
        End With
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        ExistsPatRPPlan = cmdSQL.Parameters("@PatRPPlanID").Value
        cnnSQL.Close()
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion

        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()
        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function

    Public Function IsEnabled(ByVal lngPatientID As Long, ByVal lngRPID As Long, ByVal lngPlanID As Long) As Long
        '--------------------------------------------------------------------
        'Date: 02/06/2000
        'Author: Eric Pena
        'Description:  Lets the er know if a given patient/rp/plan association is active
        'Parameters: lngPatientID - The ID of the Patient
        '            lngRPID The ID of the rp
        '            lngPlanID - The ID of the plan
        'Returns: T if the association is enabled/exists, F otherwise
        '--------------------------------------------------------------------
        'Revision History:
        ' R002 Richkun
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim cmdSQL As ADODB.Command





        rstSQL = New ADODB.Recordset
        cmdSQL = New ADODB.Command
        cnnSQL = New ADODB.Connection

        cnnSQL.Open(_ConnectionString)
        cmdSQL.ActiveConnection = cnnSQL

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspSelPatRPPlan"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatPlanRPID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@RPID", adInteger, adParamInput, , lngRPID))
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID))
        End With

        'Execute the stored procedure
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        IsEnabled = cmdSQL.Parameters("@PatRPPlanID").Value

        'Disconnect the recordset
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        rstSQL.ActiveConnection = Nothing
        cnnSQL = Nothing

        'Signal successful completion

        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        rstSQL.ActiveConnection = Nothing
        rstSQL = Nothing
        cnnSQL = Nothing

        cmdSQL.Execute()
        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function

    Public Function Insert(ByVal lngPatientID As Long, ByVal lngRPID As Long, ByVal lngPlanID As Long, ByVal intOrder As Integer, ByVal strAddedBy As String) As Long
        '--------------------------------------------------------------------
        'Date: 03/01/2000
        'Author: Eric Pena
        'Description:  Creates a billing association for the given patient-rp-plan combonation
        'Parameters: lngPatientID - The ID of the Patient
        '            lngRPID - The ID of the RP
        '            lngPlanID - The ID of the Plan
        'Returns: T if the association is enabled/exists, F otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim strSQL As String
        Dim intCTR As Integer
        Dim lngID As Long





        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        With cmdSQL
            .CommandText = "uspInsPatRPPlan"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatRPPlanID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@RPID", adInteger, adParamInput, , lngRPID))
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID))
            .Parameters.Append(.CreateParameter("@Order", adInteger, adParamInput, , intOrder))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 20, strAddedBy))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
            Exit Function
        End If

        Insert = cmdSQL.Parameters("@PatRPPlanID").Value
        cnnSQL.Close()
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion

        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()
        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function


    Public Function Append(ByVal lngPatientID As Long, ByVal lngRPID As Long, ByVal lngPlanID As Long, ByVal strAddedBy As String) As Long
        '--------------------------------------------------------------------
        'Date: 07/24/2000
        'Author: Dave Richkun
        'Description:  Creates a new Patient-RP-Plan combination.  The Order value
        '              is automatiy assigned the next available number based on
        '              existing plan assignments.
        'Parameters: lngPatientID - The ID of the Patient
        '            lngRPID - The ID of the RP
        '            lngPlanID - The ID of the Plan
        'Returns: ID of the new row inserted into tblPatRPPlan
        '--------------------------------------------------------------------
        'Revision History:
        '  R004 - Created
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim strSQL As String
        Dim intCTR As Integer
        Dim lngID As Long





        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        With cmdSQL
            .CommandText = "uspAppendPatRPPlan"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatRPPlanID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@RPID", adInteger, adParamInput, , lngRPID))
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 20, strAddedBy))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
            Exit Function
        End If

        Append = cmdSQL.Parameters("@PatRPPlanID").Value

        cnnSQL.Close()
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.Execute()
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        cnnSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function

    Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngPatientID As Long, ByVal lngRPID As Long, ByVal lngPlanID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 03/01/2000
        'Author: Eric Pena
        'Description:  Flags a row in the tblBenefactor table marking the row as
        '              deleted or undeleted.
        'Parameters: blnDeleted - Boolean value identifying if the record is to
        '               be deleted (True) or undeleted (False).
        '                  lngPatientID - The ID of the Patient
        '                  lngRPID - The ID of the RP
        '                  lngPlanID - The ID of the Plan
        '               strUserName - Login name of the user responsible for
        '               marking the row as deleted.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim strSQL As String





        'Instantiate the Command object.
        cmdSQL = New ADODB.Command

        If blnDeleted = True Then
            cmdSQL.CommandType = adCmdStoredProc
            cmdSQL.CommandText = "uspDisablePatRPPLan"
            cmdSQL.Parameters.Append(cmdSQL.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            cmdSQL.Parameters.Append(cmdSQL.CreateParameter("@RPID", adInteger, adParamInput, , lngRPID))
            cmdSQL.Parameters.Append(cmdSQL.CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID))
            cmdSQL.Parameters.Append(cmdSQL.CreateParameter("@DisabledBy", adVarChar, adParamInput, 30, strUserName))
            cmdSQL.Parameters.Append(cmdSQL.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        Else
            cmdSQL.CommandType = adCmdStoredProc
            cmdSQL.CommandText = "uspEnablePatRPPLan"
            cmdSQL.Parameters.Append(cmdSQL.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            cmdSQL.Parameters.Append(cmdSQL.CreateParameter("@RPID", adInteger, adParamInput, , lngRPID))
            cmdSQL.Parameters.Append(cmdSQL.CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID))
            cmdSQL.Parameters.Append(cmdSQL.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End If

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

        Exit Sub


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.Execute()
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        cnnSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)
    End Sub

    Public Sub DeleteRP(ByVal lngPatientID As Long, ByVal lngRPID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 07/31/2000
        'Author: Dave Richkun
        'Description:  Marks every Plan record associated with a Patient's Responsible
        '              Party as disabled.
        'Parameters:  lngPatientID - The ID of the Patient
        '             lngRPID - The ID of the RP
        '             strUserName - Login name of the user responsible for
        '               marking the rows as deleted.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim strSQL As String





        'Instantiate the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandType = adCmdStoredProc
            .CommandText = "uspDisableRP"
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@RPID", adInteger, adParamInput, , lngRPID))
            .Parameters.Append(.CreateParameter("@DisabledBy", adVarChar, adParamInput, 30, strUserName))
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

        Exit Sub


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Sub




    Public Function SetOrder(ByVal lngPatientID As Long, ByVal lngRPID As Long, _
                             ByVal lngPlanID As Long, ByVal intOrder As Integer) As Boolean
        '--------------------------------------------------------------------
        'Date: 02/06/2000
        'Author: Eric Pena
        'Description:  Changes the order of the given Patient-RP-Plan combo association above
        'Parameters: lngPatientID - The ID of the Patient
        '                  lngRPID - The ID of the RP
        '                  lngPlanID - The ID of the Plan
        '                  intOrder - The order to assign to the association
        'Returns: True if the record was updated, False otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '  R003 Richkun: Altered to use stored procedure.
        '--------------------------------------------------------------------


        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command





        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        With cmdSQL
            .CommandText = "uspUpdPatRPPlanOrder"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@Success", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@RPID", adInteger, adParamInput, , lngRPID))
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID))
            .Parameters.Append(.CreateParameter("@OrderNum", adInteger, adParamInput, , intOrder))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
            Exit Function
        End If

        If cmdSQL.Parameters("@Success").Value = 0 Then
            SetOrder = True
        End If

        cnnSQL.Close()
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Function


        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        cnnSQL = Nothing
        SetOrder = False
        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.Execute()
        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function
    '--------------------------------------------------------------------------------------
    '-- R006
    '--------------------------------------------------------------------------------------


    Public Function FetchRPsByPat(ByVal intPatient As Long, Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 3/08/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblBenefactor table utilizing a stored procedure
        'Parameters: blnIncludeDisabled - determines if disabled records will be returned
        '                   All other parameters represent column values to search for
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
            .CommandText = "uspSelPatientRPs"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , intPatient))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)
        FetchRPsByPat = rs

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rs.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion

        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rs.ActiveConnection = Nothing
        rs = Nothing
        cnn = Nothing
        'RaiseError(Err, Err.Number, CLASS_NAME)
    End Function
    Public Function FetchPlansByRP(ByVal intRPID As Long, Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 3/08/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblBenefactor table utilizing a stored procedure
        'Parameters: blnIncludeDisabled - determines if disabled records will be returned
        '                   All other parameters represent column values to search for
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
            .CommandText = "uspSelRPPlans"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@RPID", adInteger, adParamInput, , intRPID))
            .Parameters.Append(.CreateParameter("@ShowDisabled", adVarChar, adParamInput, 1, IIf(blnIncludeDisabled = True, "Y", "N")))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)
        FetchPlansByRP = rs

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

    Public Function FetchRPPlansByPat(ByVal lngPatientID As Long, Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 3/08/2000
        'Author: Eric Pena
        'Description:  Returns plan information for a specific patient
        'Parameters: lngPatientID - ID of patient whose plans will be retrieved
        '            blnIncludeDisabled - determines if disabled plans will be included in
        '                   the resultset
        'Returns: Recordset of patient plans
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
            .CommandText = "uspSelPatientRPPlans"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@ShowDisabled", adVarChar, adParamInput, 1, IIf(blnIncludeDisabled = True, "Y", "N")))
        End With

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchRPPlansByPat = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion

        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        rst = Nothing
        cnn = Nothing

        'cmdSQL.Execute()
        'RaiseError(Err, Err.Number, CLASS_NAME, Err.Description)

    End Function


    Public Function FetchDisabledRPPlansByPatient(ByVal lngPatientID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 05/16/2001
        'Author: Dave Richkun
        'Description:  Returns disabled Patient-RP-Plan rows from the tblPatRPPlan table
        '              utilizing a stored procedure
        'Parameters: lngPatientID - ID of the patient whose disabled palns will be retrieved
        'Returns: ADO Recordset
        '--------------------------------------------------------------------
        '  R009 - Created
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
            .CommandText = "uspSelDisabledPatientRPPlans"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
        End With

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)
        FetchDisabledRPPlansByPatient = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion

        Exit Function


        'cmdSQL.Execute()
        'Signal incompletion and raise the error to the ing environment.
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        rst = Nothing
        cnn = Nothing
        'RaiseError(Err, Err.Number, CLASS_NAME)

    End Function


    '++++++++++++              R001         +++++++++++++++++++++'

    '-------------------------- Begin R007 ----------------------------

    Public Function ReorderPlan(ByVal lngPatientID As Long, ByVal intOldPriority As Integer, _
                             ByVal intNewPriority As Integer) As Boolean
        '--------------------------------------------------------------------
        'Date: 01/12/2001
        'Author: Dave Nichol
        'Description:  s SP that changes the order of the a Patient-RP-Plan
        '   combo association, then adjusts the order of the other pat-rpplan
        '   records as needed.
        'Parameters: lngPatientID - The ID of the Patient
        '            lngOldPriority - The current order # of the plan
        '            lngNewPriority - The new order # of the plan
        'Returns: True if the record was updated, False otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        With cmdSQL
            .CommandText = "uspUpdPlanOrder"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@Success", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@OldPriority", adInteger, adParamInput, , intOldPriority))
            .Parameters.Append(.CreateParameter("@NewPriority", adInteger, adParamInput, , intNewPriority))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
            Exit Function
        End If

        If cmdSQL.Parameters("@Success").Value > 0 Then
            ReorderPlan = True
        End If

        cnnSQL.Close()
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Function


        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        cnnSQL = Nothing
        ReorderPlan = False
        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.Execute()
        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function
    '-------------------------- End R007 ----------------------------


    Public Function FetchPatientRPNames(ByVal lngPatientID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 01/24/2002
        'Author: Dave Richkun
        'Description:  Retrieves list of Responsible Party names associated with a patient
        'Parameters: lngPatientID - ID of the Patient
        'Returns: Recordset of RP names
        '--------------------------------------------------------------------
        'Revision History:
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
            .CommandText = "uspSelPatientRPNames"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
        End With

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)
        FetchPatientRPNames = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion

        Exit Function


        'cmdSQL.Execute()
        'Signal incompletion and raise the error to the ing environment.
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        rst = Nothing
        cnn = Nothing
        '    'RaiseError(Err, Err.Number, CLASS_NAME)
    End Function


End Class
