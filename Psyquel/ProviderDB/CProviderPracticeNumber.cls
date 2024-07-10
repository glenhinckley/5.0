
'--------------------------------------------------------------------
'Class Name: CProviderPracticeNumberDB
'Date: 04/10/2000
'Author: Dave Richkun
'Denis J Finkel
'Description:  MTS object designed to host methods associated with
'              data affecting the tblProviderPracticeNumber table.
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

Public Class CProviderNumDB
    Private Const CLASS_NAME As String = "CProviderNumberDB"
    Private Const TABLE_NAME As String = "tblProviderPracticeNumber"

    Private _ConnectionString As String = String.Empty


    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property


    Public Function Fetch(ByVal lngProviderID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 04/10/2000
        'Author: Dave Richkun
        'Description:  Retrieves a recordset of Practice Numbers for a specific provider
        'Parameters: lngProviderID - ID of Provider whose TaxID records will be
        '              retrieved.
        'Returns: Recordset of Provider TaxIDs
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
            .CommandText = "uspSelProviderPracticeNum"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
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

    Public Function FetchByInsID(ByVal lngProviderID As Long, ByVal lngInsuranceID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 11/10/2023
        'Author: Duane C Orth
        'Description:  Retrieves a recordset of Practice Numbers for a specific provider
        'Parameters: lngProviderID - ID of Provider whose Practice Numbers records will be retreived
        '            lngInsuranceID - Insurance Company ID
        'Returns: Recordset of Provider Practice Numbers
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
            .CommandText = "uspSelProviderPracticeNumByInsID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@InsuranceID", adInteger, adParamInput, , lngInsuranceID))
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

        FetchByInsID = rstSQL

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.Execute()
        cnnSQL = Nothing
        cmdSQL = Nothing
        rstSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function

    Public Function Insert(ByVal lngProviderID As Long, ByVal strState As String, _
                           ByVal lngPayerID As Long, ByVal strPracticeNum As String, _
                           ByVal strPracticeSecNum As String, ByVal strSubmitterID As String, ByVal blnGroup As Boolean, _
                           ByVal blnEnrolledYN As Boolean, ByVal dtEnrollDate As Date, _
                           ByVal lngSupervisorID As Long, ByVal strTaxID As String, _
                           ByVal lngTaxIDType As Long, ByVal strTaxName As String, _
                           ByVal blnInNetworkYN As Boolean, ByVal blnAcceptAssignmentYN As Boolean, _
                           ByVal blnIndNpiOnlyYN As Boolean, ByVal strNotes As String) As Long
        '--------------------------------------------------------------------
        'Date: 04/10/2000
        'Author: Dave Richkun
        'Description:  Inserts a row into the tblProviderPracticeNumber table utilizing
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
        'Returns: ID of the record inserted into tblProviderPracticeNumber
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim prmSQL As ADODB.Parameter



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspInsProviderPracticeNum"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@State", adVarChar, adParamInput, 2, IIf(strState = "", DBNull.Value, strState)))
            .Parameters.Append(.CreateParameter("@PayerID", adInteger, adParamInput, , IIf(lngPayerID <= 0, DBNull.Value, lngPayerID)))
            .Parameters.Append(.CreateParameter("@PracticeNum", adVarChar, adParamInput, 25, strPracticeNum))
            .Parameters.Append(.CreateParameter("@PracticeSecNum", adVarChar, adParamInput, 25, strPracticeSecNum))
            .Parameters.Append(.CreateParameter("@SubmitterID", adVarChar, adParamInput, 10, strSubmitterID))
            .Parameters.Append(.CreateParameter("@EnrolledYN", adVarChar, adParamInput, 1, IIf(blnEnrolledYN = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@EnrollDate", adDBTimeStamp, adParamInput, , IIf(dtEnrollDate.ToOADate() > 0, dtEnrollDate, DBNull.Value)))
            .Parameters.Append(.CreateParameter("@Notes", adVarChar, adParamInput, 255, strNotes))
            .Parameters.Append(.CreateParameter("@Group", adVarChar, adParamInput, 1, IIf(blnGroup, "Y", "N")))
            .Parameters.Append(.CreateParameter("@SupervisorID", adInteger, adParamInput, , IIf(lngSupervisorID <= 0, DBNull.Value, lngSupervisorID)))
            .Parameters.Append(.CreateParameter("@TaxID", adVarChar, adParamInput, 10, strTaxID))
            .Parameters.Append(.CreateParameter("@TaxIDType", adInteger, adParamInput, , IIf(lngTaxIDType <= 0, DBNull.Value, lngTaxIDType)))
            .Parameters.Append(.CreateParameter("@TaxName", adVarChar, adParamInput, 80, strTaxName))
            .Parameters.Append(.CreateParameter("@InNetwork", adVarChar, adParamInput, 1, IIf(blnInNetworkYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@AcceptAssignment", adVarChar, adParamInput, 1, IIf(blnAcceptAssignmentYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@IndNpiOnly", adVarChar, adParamInput, 1, IIf(blnIndNpiOnlyYN, "Y", "N")))
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

    Public Sub Update(ByVal lngProviderPracticeNumber As Long, ByVal lngProviderID As Long, _
                      ByVal strState As String, ByVal lngPayerID As Long, _
                      ByVal strPracticeNumber As String, ByVal strPracticeSecNum As String, ByVal strSubmitterID As String, _
                      ByVal blnGroup As Boolean, ByVal blnEnrolledYN As Boolean, ByVal dtEnrollDate As Date, _
                      ByVal lngSupervisorID As Long, ByVal strTaxID As String, _
                      ByVal lngTaxIDType As Long, ByVal strTaxName As String, _
                      ByVal blnInNetworkYN As Boolean, ByVal blnAcceptAssignmentYN As Boolean, _
                      ByVal blnIndNpiOnlyYN As Boolean, ByVal strNotes As String)
        '--------------------------------------------------------------------
        'Date: 04/10/2000
        'Author: Dave Richkun
        'Description:  Updates a row into the tblProviderPracticeNumber table utilizing
        '              a stored procedure.
        'Parameters: lngProviderPracticeNumber - ID of record to update
        '            lngProviderID -  ID of the Provider
        '            strState - the state to which the Provider license number applies
        '            lngPayerID - ID of the Insurance Company to which the CPC-specific
        '               number applies.
        '            strPracticeNum - The number identifying the Provider in the state or
        '               within the CPC
        '            strNotes - Additional information about the Provider-Practice number
        '               record.
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
            .CommandText = "uspUpdProviderPracticeNumber"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderPracticeNumber", adInteger, adParamInput, , lngProviderPracticeNumber))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@State", adVarChar, adParamInput, 2, IIf(strState = "", DBNull.Value, strState)))
            .Parameters.Append(.CreateParameter("@PayerID", adInteger, adParamInput, , IIf(lngPayerID <= 0, DBNull.Value, lngPayerID)))
            .Parameters.Append(.CreateParameter("@PracticeNumber", adVarChar, adParamInput, 25, strPracticeNumber))
            .Parameters.Append(.CreateParameter("@PracticeSecNum", adVarChar, adParamInput, 25, strPracticeSecNum))
            .Parameters.Append(.CreateParameter("@SubmitterID", adVarChar, adParamInput, 10, strSubmitterID))
            .Parameters.Append(.CreateParameter("@EnrolledYN", adVarChar, adParamInput, 1, IIf(blnEnrolledYN = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@EnrollDate", adDBTimeStamp, adParamInput, , IIf(dtEnrollDate.ToOADate() > 0, dtEnrollDate, DBNull.Value)))
            .Parameters.Append(.CreateParameter("@Notes", adVarChar, adParamInput, 255, strNotes))
            .Parameters.Append(.CreateParameter("@Group", adVarChar, adParamInput, 1, IIf(blnGroup, "Y", "N")))
            .Parameters.Append(.CreateParameter("@SupervisorID", adInteger, adParamInput, , IIf(lngSupervisorID <= 0, DBNull.Value, lngSupervisorID)))
            .Parameters.Append(.CreateParameter("@TaxID", adVarChar, adParamInput, 10, strTaxID))
            .Parameters.Append(.CreateParameter("@TaxIDType", adInteger, adParamInput, , IIf(lngTaxIDType <= 0, DBNull.Value, lngTaxIDType)))
            .Parameters.Append(.CreateParameter("@TaxName", adVarChar, adParamInput, 80, strTaxName))
            .Parameters.Append(.CreateParameter("@InNetwork", adVarChar, adParamInput, 1, IIf(blnInNetworkYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@AcceptAssignment", adVarChar, adParamInput, 1, IIf(blnAcceptAssignmentYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@IndNpiOnly", adVarChar, adParamInput, 1, IIf(blnIndNpiOnlyYN, "Y", "N")))
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
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        cnnSQL.Close()
        cnnSQL = Nothing

        'Signal successful completion


        Exit Sub


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.Execute()
        cmdSQL = Nothing
        cnnSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Sub


    Public Sub Delete(ByVal lngProviderID As Long)
        '--------------------------------------------------------------------
        'Date: 02/23/2000
        'Author: Dave Richkun
        'Description:  Physiy deletes all row from the tbProviderPracticeNumber
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
        strSQL = strSQL & " fldProviderID = " & lngProviderID

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
