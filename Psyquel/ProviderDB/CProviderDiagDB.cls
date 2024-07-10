
'Class Name: CProviderDiagDB
'Date: 03/05/2010
'Author: Duane C Orth
'Denis J Finkel
'Description:  MTS object designed to host methods associated with
'              data affecting the tblProviderDiagnosis table.
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


Public Class CProviderDiagDB
    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CProviderDiagDB"
    Private Const TABLE_NAME As String = "tblProviderDiagnosis"


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
            .CommandText = "uspSelProviderDiagnosis"
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
    Public Function FetchProviders(Optional ByVal blnIncludeDisabled As Boolean = False, _
                          Optional ByVal strUserWhere As String = "", _
                          Optional ByVal strOrderBy As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 11/02/2018
        'Author: Duane C Orth
        'Description:  Retrieves records from the tblProviderDiag table.
        'Parameters: None
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim strSQL As String


        'Prepare the SQL statement.
        strSQL = "SELECT A.fldProviderID, B.fldLastName, B.fldFirstName, B.fldMI, C.fldGroupID, D.fldGroupName, B.fldDisabledYN "
        strSQL = strSQL & "FROM "
        strSQL = strSQL & TABLE_NAME & " AS A LEFT OUTER JOIN "
        strSQL = strSQL & "tblUser AS B ON A.fldProviderID = B.fldUserID LEFT OUTER JOIN "
        strSQL = strSQL & "tblProvider AS C ON B.fldUserID = C.fldProviderID LEFT OUTER JOIN "
        strSQL = strSQL & "tblProviderGroup AS D ON C.fldGroupID = D.fldGroupID "

        If blnIncludeDisabled Then
            If strUserWhere > "" Then
                strUserWhere = strUserWhere + " AND A.fldDisabledYN = 'N' "
            Else
                strUserWhere = " A.fldDisabledYN = 'N' "
            End If
        Else
            If strUserWhere > "" Then
                strUserWhere = strUserWhere + " AND A.fldDisabledYN = 'N' AND B.fldDisabledYN = 'N' "
            Else
                strUserWhere = " A.fldDisabledYN = 'N' AND B.fldDisabledYN = 'N' "
            End If
        End If

        strSQL = strSQL & " WHERE " & strUserWhere
        strSQL = strSQL & " GROUP BY A.fldProviderID, B.fldLastName, B.fldFirstName, B.fldMI, B.fldMI, C.fldGroupID, D.fldGroupName, B.fldDisabledYN "

        If strOrderBy > "" Then
            strSQL = strSQL & " ORDER BY "
            strSQL = strSQL & strOrderBy
        Else
            strSQL = strSQL & " ORDER BY "
            strSQL = strSQL & "B.fldLastName "
        End If

        'Instantiate the Recordset.
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = adUseClient

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Populate the Recordset
        rstSQL.Open(strSQL, cnnSQL, adOpenForwardOnly, adLockReadOnly, adCmdText + adAsyncFetch)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rstSQL.ActiveConnection = Nothing

        cnnSQL.Close()
        cnnSQL = Nothing
        FetchProviders = rstSQL

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        cnnSQL = Nothing
        rstSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function

    Public Function Insert(ByVal lngProviderID As Long, _
                           ByVal lngDiagnosisID As Long, _
                           ByVal blnDefault As Boolean) As Long
        '--------------------------------------------------------------------
        'Date: 03/05/2010
        'Author: Duane C Orth
        'Description:  Inserts a row into the tblProviderDiagnosis table utilizing
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
        'Returns: ID of the record inserted into tblProviderDiagnosis
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim prmSQL As ADODB.Parameter



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspInsProviderDiagnosis"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@DiagnosisID", adInteger, adParamInput, , lngDiagnosisID))
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


    Public Sub Delete(ByVal lngProviderID As Long, _
                      ByVal lngDiagnosisID As Long)
        '--------------------------------------------------------------------
        'Date: 03/05/2010
        'Author: Duane C Orth
        'Description:  Physiy deletes all row from the tblProviderDiagnosis
        '               table that are associated with a specific provider.
        'Parameters: lngProviderID - ID of the Provider whose PracticeNumbers will be
        '               deleted.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim strSQL As String
        Dim blnFlag As String



        'Prepare the SQL statement
        strSQL = "DELETE FROM "
        strSQL = strSQL & TABLE_NAME
        strSQL = strSQL & " WHERE "
        strSQL = strSQL & " fldProviderID = " & lngProviderID & " AND "
        strSQL = strSQL & " fldDiagnosisID = " & lngDiagnosisID

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
