
'Class Name: CProviderPlanDB
'Date: 03/05/2010
'Author: Duane C Orth
'Denis J Finkel
'Description:  MTS object designed to host methods associated with
'              data affecting the tblProviderPlan table.
'--------------------------------------------------------------------
' Revision History:
'
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

Public Class CProviderPlanDB


    Private _ConnectionString As String = String.Empty



    Private Const CLASS_NAME As String = "CProviderPlanDB"
    Private Const TABLE_NAME As String = "tblProviderPlan"


    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property





    Public Function Fetch(ByVal lngProviderID As Long, Optional ByVal blnDefault As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/05/2010
        'Author: Duane C Orth
        'Description:  Retrieves a recordset of Diagnosis's for a specific provider
        'Parameters: lngProviderID - ID of Provider whose Diagnosis records will be
        '              retrieved.
        'Returns: Recordset of Provider Diagnosis's
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

        With cmdSQL
            .CommandText = "uspSelProviderPlan"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@DefaultYN", adVarChar, adParamInput, 1, IIf(blnDefault = True, "Y", "N")))
        End With

        'Instantiate and populate the Recordset.
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rstSQL.ActiveConnection = Nothing
        cnnSQL.Close()
        cnnSQL = Nothing
        cmdSQL = Nothing

        Fetch = rstSQL

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.Execute()
        cnnSQL = Nothing
        cmdSQL = Nothing
        rstSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function

    Public Function Insert(ByVal lngProviderID As Long, _
                           ByVal lngProviderPlanID As Long, _
                           ByVal blnDefault As Boolean) As Long
        '--------------------------------------------------------------------
        'Date: 03/05/2010
        'Author: Duane C Orth
        'Description:  Inserts a row into the tblProviderPlan table utilizing
        '              a stored procedure.
        'Parameters: lngProviderID - ID of Provider associated with the TaxID
        '            strState - the state to which the Provider license number applies
        '            lngPayerID - ID of the Insurance Company to which the practice number
        '               may apply.
        '            strPracticeNum - The number identifying the Provider in the state or
        '               within the Insurance Company
        '            blnGroup - Identifies if the practice number is a group practice number
        '            blnEnrolledYN - Identifies if the provider has enrolled for electronic
        '               claim submission with the payer
        '            dtEnrollDate - The date the provider has enrolled for electronic claim
        '               submission with the payer
        '            strNotes - Additional information about the Provider-Practice number
        '               record.
        'Returns: ID of the record inserted into tblProviderPlan
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim prmSQL As ADODB.Parameter



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspInsProviderPlan"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@ProviderPlanID", adInteger, adParamInput, , lngProviderPlanID))
            .Parameters.Append(.CreateParameter("@DefaultYN", adVarChar, adParamInput, 1, IIf(blnDefault = True, "Y", "N")))
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
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        cnnSQL.Close()
        cnnSQL = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.Execute()
        cmdSQL = Nothing
        cnnSQL = Nothing
        'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME)

    End Function


    Public Sub Delete(ByVal lngProviderPlanID As Long)
        '--------------------------------------------------------------------
        'Date: 03/05/2010
        'Author: Duane C Orth
        'Description:  Physiy deletes all row from the tblProviderPlan
        '               table that are associated with a specific provider.
        'Parameters: lngProviderID - ID of the Provider whose PracticeNumbers will be
        '               deleted.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim strSQL As String
        Dim blnFlag As String



        'Prepare the SQL statement
        strSQL = "DELETE FROM "
        strSQL = strSQL & TABLE_NAME
        strSQL = strSQL & " WHERE "
        strSQL = strSQL & " fldProviderPlanID = " & lngProviderPlanID

        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        cmdSQL.CommandText = strSQL
        cmdSQL.CommandType = adCmdText

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Close the connection and free all resources
        cmdSQL.ActiveConnection = Nothing
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
End Class
