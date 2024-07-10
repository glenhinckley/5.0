
'--------------------------------------------------------------------
'Class Name: CProviderTaxIDDB
'Date: 04/10/2000
'Author: Dave Richkun
'Denis J Finkel
'Description:  MTS object designed to host methods associated with
'              data affecting the tblProviderTaxID table.
'--------------------------------------------------------------------
' Revision History:
'  08/23/2000 RS: Leak checks and improvements
'  06/01/2003 Richkun: Added DeleteAll() method
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

Public Class CProviderTaxIDDB
    Private _ConnectionString As String = String.Empty

    Private Const CLASS_NAME As String = "CProviderTaxIDDB"
    Private Const TABLE_NAME As String = "tblProviderTaxID"


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
        'Description:  Retrieves a recordset of TaxIDs for a specific provider
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

        ' Prepare stored procedure parameters
        With cmdSQL
            .CommandText = "uspSelProviderTaxID"
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

        Fetch = rstSQL

        cmdSQL = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.Execute()
        cnnSQL = Nothing
        cmdSQL = Nothing
        rstSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function

    Public Function FetchPrimaryTaxID(ByVal lngProviderID As Long, ByVal lngPatientID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/06/2001
        'Author: Chris Dereadt
        'Description:  Retrieves a recordset of the Primary TaxIDs for a specific provider
        'Parameters: lngProviderID - ID of Provider whose TaxID records will be
        '               retrieved
        '            lngPatientID - ID of Patient associated with taxID
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

        ' Prepare stored procedure parameters
        With cmdSQL
            .CommandText = "uspSelPrimaryTaxID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
        End With

        'Instantiate and populate the Recordset.
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rstSQL.ActiveConnection = Nothing
        cnnSQL.Close()
        cnnSQL = Nothing

        FetchPrimaryTaxID = rstSQL

        cmdSQL = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.Execute()
        cnnSQL = Nothing
        cmdSQL = Nothing
        rstSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function


    Public Function Insert(ByVal lngProviderID As Long, ByVal strTaxID As String, _
                           ByVal intTaxIDType As Integer, ByVal strNotes As String, ByVal strGroup As String) As Long
        '--------------------------------------------------------------------
        'Date: 04/10/2000
        'Author: Dave Richkun
        'Description:  Inserts a row into the tblProviderTaxID table utilizing
        '              a stored procedure.
        'Parameters: lngProviderID - ID of Provider associated with the TaxID
        '            strTaxID - the TaxID assigned to the Provider
        '            intTaxIDType - ID representing the type of TaxID.  Must be
        '               one of:
        '                    1 - Individual
        '                    2 - Group
        '                    3 - EIN (Employee Identifiaction Number)
        '            strNotes - Additional information about the ProviderTaxID
        '               record
        'Returns: ID of the record inserted into tblProviderTaxID
        '--------------------------------------------------------------------


        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim prmSQL As ADODB.Parameter





        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspInsProviderTaxID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@TaxID", adVarChar, adParamInput, 25, strTaxID))
            .Parameters.Append(.CreateParameter("@TaxIDType", adInteger, adParamInput, , intTaxIDType))
            .Parameters.Append(.CreateParameter("@Notes", adVarChar, adParamInput, 80, strNotes))
            .Parameters.Append(.CreateParameter("@Group", adVarChar, adParamInput, 50, strGroup))
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

    Public Sub Update(ByVal lngProviderTaxID As Long, ByVal lngProviderID As Long, _
                      ByVal strTaxID As String, ByVal intTaxIDType As Integer, _
                      ByVal strNotes As String, ByVal strGroup As String)

        '--------------------------------------------------------------------
        'Date: 04/10/2000
        'Author: Dave Richkun
        'Description:  Updates a row into the tblProviderTaxID table utilizing
        '              a stored procedure.
        'Parameters: lngProviderTaxID - ID of record to update
        '            lngProviderID -  ID of the Provider
        '            strTaxID - The TaxID number
        '            intTaxIDType - ID representing the type of TaxID.  Must be
        '               one of:
        '                    1 - Individual
        '                    2 - Group
        '                    3 - EIN (Employee Identifiaction Number)
        '            strNotes - Additional comments
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
            .CommandText = "uspUpdProviderTaxID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderTaxID", adInteger, adParamInput, , lngProviderTaxID))
            '.Parameters.Append .CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@TaxID", adVarChar, adParamInput, 25, strTaxID))
            .Parameters.Append(.CreateParameter("@TaxIDType", adInteger, adParamInput, , intTaxIDType))
            .Parameters.Append(.CreateParameter("@Notes", adVarChar, adParamInput, 80, strNotes))
            .Parameters.Append(.CreateParameter("@Group", adVarChar, adParamInput, 50, strGroup))
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


    Public Function Exists(ByVal lngProviderID As Long, ByVal strTaxID As String, _
                           ByVal intTaxIDType As Integer) As Long
        '--------------------------------------------------------------------
        'Date: 04/10/2000
        'Author: Dave Richkun
        'Description:  Determines if a Provider-TaxID association already exists
        '              in tblProviderTaxID
        'Parameters: lngProviderID - internal provider ID being sought
        '            strTaxID - Tax ID number being sought
        '            intTaxIDType - Type of Tax ID being sought.  See 'Insert' or 'Update'
        '               for applicable values.
        'Returns: If an association exists, the ID of the association record, -1 otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim strSQL As String
        Dim blnExists As Boolean





        'Prepare the SQL statement
        strSQL = "SELECT "
        strSQL = strSQL & " fldProviderTaxID"
        strSQL = strSQL & " FROM "
        strSQL = strSQL & TABLE_NAME
        strSQL = strSQL & " WHERE "
        strSQL = strSQL & " fldProviderID = " & lngProviderID & " AND"
        strSQL = strSQL & " fldTaxIDType = " & intTaxIDType & " AND"
        strSQL = strSQL & " fldTaxID = '" & strTaxID & "'"

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

        'Close the connection and free all resources
        cnnSQL.Close()
        cnnSQL = Nothing

        'Evaluate recordcount of Recordset then destroy it.
        If rstSQL.RecordCount = 1 Then
            Exists = rstSQL.Fields("fldProviderTaxID").Value
        Else
            Exists = -1
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

    Public Sub Delete(ByVal lngProviderID As Long, ByVal varTaxIDList As Object)
        '--------------------------------------------------------------------
        'Date: 05/02/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Deletes all Tax IDs NOT found in the passedarray list'
        'Parameters: varTaxIDList - List of Tax IDs NOT to be deleted(1D)   '
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim strSQL As String
        Dim strTaxIDs As String
        Dim intCTR As Integer



        'Build the Tax ID string
        If IsArray(varTaxIDList) Then
            For intCTR = 0 To UBound(varTaxIDList)
                strTaxIDs = strTaxIDs & varTaxIDList(intCTR)
                If intCTR < UBound(varTaxIDList) Then
                    strTaxIDs = strTaxIDs & ", "
                End If
            Next intCTR
            strTaxIDs = " AND tblProviderTaxID.fldProviderTaxID NOT IN (" & strTaxIDs & ")"
        Else
            strTaxIDs = ""
        End If
        'Prepare the SQL statement
        strSQL = "DELETE "
        strSQL = strSQL & " tblProviderPlanTaxID "
        strSQL = strSQL & " FROM "
        strSQL = strSQL & " tblProviderPlanTaxID "
        strSQL = strSQL & " INNER JOIN "
        strSQL = strSQL & " tblProviderTaxID "
        strSQL = strSQL & " ON "
        strSQL = strSQL & " tblProviderTaxID.fldProviderTaxID = tblProviderPlanTaxID.fldProviderTaxID "
        strSQL = strSQL & " WHERE "
        strSQL = strSQL & " tblProviderTaxID.fldProviderID = " & lngProviderID
        strSQL = strSQL & strTaxIDs

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

        strSQL = "UPDATE "
        strSQL = strSQL & " tblProviderTaxID "
        strSQL = strSQL & " SET "
        strSQL = strSQL & " fldDisabledYN = 'Y' "
        strSQL = strSQL & " WHERE "
        strSQL = strSQL & " fldProviderID = " & lngProviderID
        strSQL = strSQL & strTaxIDs

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


    Public Sub DeleteAll(ByVal lngProviderID As Long)
        '--------------------------------------------------------------------
        'Date: 06/01/2003
        'Author: Dave Richkun
        'Description:  Deletes/Disabeld all Tax IDs associated with a provider
        'Parameters: lngProviderID - ID of provider whose Tax IDs will be disabled
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
            .CommandText = "uspUpdDisableAllProviderTaxIDs"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
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
