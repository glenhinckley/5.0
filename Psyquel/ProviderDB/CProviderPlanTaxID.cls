
'--------------------------------------------------------------------
'Class Name: CProviderPlanTaxIDDB
'Date: 06/20/2000
'Author: Eric Pena
'Denis J Finkel
'Description:  MTS object designed to host methods associated with
'              data affecting the tblProviderPlanTaxID table.
'--------------------------------------------------------------------
' Revision History:
'   08/23/2000 RS: Leak checks and improvements
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


Public Class CProviderPlanTaxIDDB

    Private _ConnectionString As String = String.Empty

    Private Const CLASS_NAME As String = "CProviderPlanTaxIDDB"
    Private Const TABLE_NAME As String = "tblProviderPlanTaxID"


    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property



    Public Function Fetch(ByVal lngProviderID As Long, ByVal lngPlanID As Long) As Long
        '--------------------------------------------------------------------
        'Date: 06/20/2000
        'Author: Eric Pena
        'Description:  Retrieves the id in tblProviderPlanTaxID for a given provider-plan pair
        'Parameters: lngProviderID - ID of Provider to search
        '                   lngplanid - id of the plan to search
        'Returns: id in tblproviderplanid for the given pair
        '--------------------------------------------------------------------


        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim rstSQL As ADODB.Recordset



        cmdSQL = New ADODB.Command
        cnnSQL = New ADODB.Connection
        rstSQL = New ADODB.Recordset

        'Acquire the database connection.
        cnnSQL.Open(_ConnectionString)
        cmdSQL.ActiveConnection = cnnSQL

        ' Prepare stored procedure parameters
        With cmdSQL
            .CommandText = "uspSelProviderPlanTaxID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID))
        End With

        'Instantiate and populate the Recordset.
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rstSQL.ActiveConnection = Nothing
        cnnSQL.Close()
        cnnSQL = Nothing

        If rstSQL.EOF And rstSQL.BOF Then
            Fetch = -1
        Else
            Fetch = rstSQL.Fields("fldProvPlanTaxID").Value
        End If

        cmdSQL = Nothing

        'Signal successful completion

        rstSQL = Nothing
        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.Execute()
        cmdSQL = Nothing
        cnnSQL = Nothing
        rstSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function

    Public Function Insert(ByVal lngPlanID As Long, ByVal lngProviderTaxID As Long) As Long
        '--------------------------------------------------------------------
        'Date: 06/20/2000
        'Author: Eric Pena
        'Description:  Inserts a row into the tblProviderPlanTaxID table utilizing a stored procedure.
        'Parameters: lngPlanID - ID of plan associated with the TaxID
        '            lngProviderTaxID - the ID in tblProviderTaxID to use as default for the Plan
        'Returns: ID of the record inserted into tblProviderPlanTaxID
        '--------------------------------------------------------------------


        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim prmSQL As ADODB.Parameter





        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspInsProviderPlanTaxID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID))
            .Parameters.Append(.CreateParameter("@ProviderTaxID", adInteger, adParamInput, , lngProviderTaxID))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check for errors
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
        cmdSQL.Execute()
        cmdSQL = Nothing
        cnnSQL = Nothing
        'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME)

    End Function

    Public Sub Update(ByVal lngProvPlanTaxID As Long, ByVal lngProviderTaxID As Long)

        '--------------------------------------------------------------------
        'Date: 06/20/2000
        'Author: Eric Pena
        'Description:  updates a row into the tblProviderPlanTaxID table utilizing a stored procedure.
        'Parameters: lngProvPlanTaxID - ID of row associated in tblProviderPlanID
        '            lngProviderTaxID - the ID in tblProviderTaxID to use as default for the Plan
        'Returns: null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command





        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        With cmdSQL
            .CommandText = "uspUpdProviderPlanTaxID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProvPlanTaxID", adInteger, adParamInput, , lngProvPlanTaxID))
            .Parameters.Append(.CreateParameter("@ProviderTaxID", adInteger, adParamInput, , lngProviderTaxID))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check for errors
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
        cmdSQL.Execute()
        cmdSQL = Nothing
        cnnSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)
    End Sub
    Public Function Exists(ByVal lngPlanID As Long, ByVal lngProviderTaxID As Long) As Long
        '--------------------------------------------------------------------
        'Date: 06/20/2000
        'Author: Eric Pena
        'Description:  Determines if a Provider-Plan TaxID association already exists in tblProviderPlanTaxID
        'Parameters: lngProviderID - internal provider ID being sought
        '            lngPlanID - ID in tblProviderTaxID being sought
        'Returns: If an association exists, the ID of the association record in tblProviderPlanTaxID, -1 otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '--------------------------------------------------------------------


        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim strSQL As String
        Dim blnExists As Boolean




        Exists = -1
        'Prepare the SQL statement
        strSQL = "SELECT "
        strSQL = strSQL & " fldProvPlanTaxID"
        strSQL = strSQL & " FROM "
        strSQL = strSQL & TABLE_NAME
        strSQL = strSQL & " WHERE "
        strSQL = strSQL & " fldPlanID = " & lngPlanID & " AND"
        strSQL = strSQL & " fldProviderTaxID = " & lngProviderTaxID

        'Instantiate the Recordset.
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = adUseClient

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'populate the recordset
        rstSQL.Open(strSQL, cnnSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rstSQL.ActiveConnection = Nothing
        cnnSQL.Close()
        cnnSQL = Nothing

        'Evaluate recordcount of Recordset then destroy it.
        If rstSQL.RecordCount = 1 Then
            Exists = rstSQL.Fields("fldProviderTaxID").Value
        End If

        rstSQL = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        rstSQL = Nothing
        cnnSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function

End Class
