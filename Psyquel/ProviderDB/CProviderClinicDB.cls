
'--------------------------------------------------------------------
'Class Name: CProviderClinicDB
'Date: 02/14/2000
'Author: Rick "Boom Boom" Segura
'Denis J Finkel
'Description:  MTS object designed to host methods associated with
'              data affecting the tblProviderClinic table.
'--------------------------------------------------------------------
' Revision History:
'   R001 03/24/2000 Richkun: Added DisableProviderClinics to auto-disabled
'         Provider-Clinics that may have been de-selected from a list of
'         formerly active clinics
'   R002 08/23/2000 RS: Leak checks and improvements
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

Public Class CProviderClinicDB

    Private _ConnectionString As String = String.Empty
    Private Const CLASS_NAME As String = "CProviderClinicDB"
    Private Const TABLE_NAME As String = "tblProviderClinic"
    Private Const CLINIC_TABLE_NAME As String = "tblClinic"
    Private Const FLD_CLINICID As Integer = 1

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property





    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False, _
                          Optional ByVal strUserWhere As String = "", _
                          Optional ByVal strOrderBy As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 02/14/2000
        'Author: Rick "Boom Boom" Segura
        'Description:  Retrieves records from the tblProviderClinic table.
        'Parameters:  blnIncludeDisabled - Optional parameter that identifies if
        '               records flagged as 'Disabled' or 'De-activated' are to be
        '               included in the record set. The default value is False.
        '             strWhere - Optional 'Where' clause of the SQL statement by
        '               which records will be filtered.  If the parameter is not
        '               supplied, all records will be retrieved.
        '             strOrderBy - Optional 'Order By' clause in which retrieved
        '               records will be sorted.  The default is set to the
        '               'fldProviderClinicID' column
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim strSQL As String
        Dim strWhere As String
        Dim blnUserWhere As Boolean





        'Prepare the SQL statement.
        strSQL = "SELECT "
        strSQL = strSQL & "fldProviderClinicID, "
        strSQL = strSQL & "fldProviderID, "
        strSQL = strSQL & " fldClinicID, "
        strSQL = strSQL & " fldDisabledYN, "
        strSQL = strSQL & " fldDateDisabled, "
        strSQL = strSQL & " fldDateAdded, "
        strSQL = strSQL & " fldAddedBy, "
        strSQL = strSQL & " fldAllowPatSched, "
        strSQL = strSQL & " fldAcceptNewPat, "
        strSQL = strSQL & " fldPointOfContact "
        strSQL = strSQL & " FROM "
        strSQL = strSQL & TABLE_NAME


        'Attach user's where clause if applicable
        blnUserWhere = Trim(strUserWhere) > ""

        ' Build where clause appropriately
        If blnIncludeDisabled Then  'Include disabled records

            If blnUserWhere Then    ' Only use user where string
                strWhere = " WHERE " & strUserWhere
            Else ' Empty where string
                strWhere = ""
            End If

        Else    'Filter out disabled records
            strWhere = " Where fldDisabledYN = 'N' "

            If blnUserWhere Then ' Concat user where string to filtering string
                strWhere = strWhere & " AND " & strUserWhere
            Else
                ' Do nothing
            End If

        End If
        strSQL = strSQL & strWhere

        If Trim(strOrderBy) > "" Then
            strSQL = strSQL & " ORDER BY "
            strSQL = strSQL & strOrderBy
        Else
            strSQL = strSQL & " ORDER BY "
            strSQL = strSQL & "fldProviderID, "
            strSQL = strSQL & "fldClinicID "
        End If

        'Instantiate the Recordset.
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = adUseClient

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Populate the recordset
        rstSQL.Open(strSQL, cnnSQL, adOpenForwardOnly, adLockReadOnly, adCmdText + adAsyncFetch)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rstSQL.ActiveConnection = Nothing

        cnnSQL.Close()
        cnnSQL = Nothing
        Fetch = rstSQL

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        rstSQL = Nothing
        cnnSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function
    Public Function FetchProvidersByClinic(ByVal lngClinicID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/10/2010
        'Author: Duane C Orth
        'Description: Retrieves providers who are assigned to a specific clinic.
        'Parameters: ClinicID - Clinic ID.
        '            includeDisabled - also include disabled records on true.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As ADODB.Connection ' Declare DB connection
        Dim rst As ADODB.Recordset  ' Declare recordset
        Dim cmd As ADODB.Command    ' Declare SQL statement



        'Create the parameter objects
        cmd = New ADODB.Command     ' Instantiate SQL statement
        With cmd
            .CommandText = "uspSelProvidersByClinic"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ClinicID", adInteger, adParamInput, , lngClinicID))
        End With

        'Acquire the database connection.
        cnn = New ADODB.Connection  ' Instantiate DB connection
        cnn.Open(_ConnectionString) ' open DB connection
        cmd.ActiveConnection = cnn

        'Set cursor and execute
        rst = New ADODB.Recordset   ' Instantiate recordset
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)   ' open recordset

        ' Cannot close recordset here...close by client
        FetchProvidersByClinic = rst ' Set return value

        ' De-allocate space for objects
        cmd.ActiveConnection = Nothing
        rst.ActiveConnection = Nothing
        cnn.Close()   ' close DB connection
        cnn = Nothing
        cmd = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()

        cnn.Close()   ' close DB connection
        cmd.ActiveConnection = Nothing
        rst.Close()   ' close the recordset --R003
        rst.ActiveConnection = Nothing

        ' De-allocate space for objects
        rst = Nothing
        cmd = Nothing
        cnn = Nothing

        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function


    Public Function Insert(ByVal lngProviderID As Long, ByVal lngClinicID As Long, _
                           ByVal strAddedBy As String, ByVal strAllowPatSched As String, _
                           ByVal strAcceptNewPat As String, ByVal strPOC As String) As Long
        '--------------------------------------------------------------------
        'Date: 02/14/2000
        'Author: Rick "Boom Boom" Segura
        'Description:  Inserts a row into the tblProviderClinic table utilizing
        '              a stored procedure.
        'Parameters: lngProviderID - internal provider ID
        '            lngClinicID - internal clinic ID
        '            strAddedBy - login of person making this association
        '            strAllowPatSched -  Allow patients to scedule via web flag
        '            strAcceptNewPat - accepting new patients flag
        '            strPOC - Point of contact
        'Returns: new record ID
        '--------------------------------------------------------------------


        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim prmSQL As ADODB.Parameter





        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspInsProviderClinic"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@ClinicID", adInteger, adParamInput, , lngClinicID))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strAddedBy))
            .Parameters.Append(.CreateParameter("@AllowPatSched", adVarChar, adParamInput, 1, strAllowPatSched))
            .Parameters.Append(.CreateParameter("@AcceptNewPat", adVarChar, adParamInput, 1, strAcceptNewPat))
            .Parameters.Append(.CreateParameter("@POC", adVarChar, adParamInput, 30, strPOC))
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

    Public Sub Update(ByVal lngID As Long, _
                      ByVal strAllowPatSched As String, _
                      ByVal strAcceptNewPat As String, ByVal strPOC As String)
        '--------------------------------------------------------------------
        'Date: 02/14/2000
        'Author: Rick "Boom Boom" Segura
        'Description:  Updates a row into the tblProviderClinic table utilizing
        '              a stored procedure.
        'Parameters: lngID - ID of record to update
        '            strAllowPatSched -  "Allow patients to schedule via web" flag
        '            strAcceptNewPat - "accepting new patients" flag
        '            strPOC - Point of contact
        '            strDisabledYN - association disabled flag
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
            .CommandText = "uspUpdProviderClinic"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@lngID", adInteger, adParamInput, , lngID))
            .Parameters.Append(.CreateParameter("@AllowPatSched", adVarChar, adParamInput, 1, strAllowPatSched))
            .Parameters.Append(.CreateParameter("@AcceptNewPat", adVarChar, adParamInput, 1, strAcceptNewPat))
            .Parameters.Append(.CreateParameter("@POC", adVarChar, adParamInput, 30, strPOC))
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
        cmdSQL.ActiveConnection = Nothing
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

    Public Function Exists(ByVal lngProviderID As Long, ByVal lngClinicID As Long) As Long
        '--------------------------------------------------------------------
        'Date: 02/14/2000
        'Author: Rick "Boom Boom" Segura
        'Description:  Determines provider-clinic association already exists
        '              in tblProviderClinic
        'Parameters: lngProviderID - internal provider ID being sought
        '            lngClinicID - internal clinic ID being sought
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
        strSQL = strSQL & " fldProviderClinicID"
        strSQL = strSQL & " FROM "
        strSQL = strSQL & TABLE_NAME
        strSQL = strSQL & " WHERE "
        strSQL = strSQL & " fldProviderID = "
        strSQL = strSQL & lngProviderID
        strSQL = strSQL & " AND fldClinicID = "
        strSQL = strSQL & lngClinicID

        'Instantiate the Recordset.
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = adUseClient

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Populate the recordset
        rstSQL.Open(strSQL, cnnSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rstSQL.ActiveConnection = Nothing

        'Close the connection and free all resources
        cnnSQL.Close()
        cnnSQL = Nothing

        'Evaluate recordcount of Recordset then destroy it.
        If rstSQL.RecordCount = 1 Then
            Exists = rstSQL.Fields("fldProviderClinicID").Value
        Else
            Exists = -1
        End If

        rstSQL.ActiveConnection = Nothing
        rstSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        cnnSQL = Nothing
        rstSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function

    Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Long)
        '--------------------------------------------------------------------
        'Date: 02/23/2000
        'Author: Rick "Boom Boom" Segura
        'Description:  Flags a row in the tbProviderClinic table marking the row as
        '              deleted or undeleted.
        'Parameters: blnDeleted - Boolean value identifying if the record is to
        '               be deleted (True) or undeleted (False).
        '            lngID - ID of the row in the table whose value will be
        '               updated.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim strSQL As String
        Dim blnFlag As String





        If blnDeleted = True Then
            blnFlag = "Y"
        Else
            blnFlag = "N"
        End If

        'Prepare the SQL statement
        strSQL = "UPDATE "
        strSQL = strSQL & TABLE_NAME
        strSQL = strSQL & " SET "
        strSQL = strSQL & " fldDisabledYN = '" & blnFlag & "', "
        strSQL = strSQL & " fldDateDisabled = '" & Date.Now & "' "
        strSQL = strSQL & " WHERE "
        strSQL = strSQL & " fldProviderClinicID = " & lngID

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

    Public Sub DisableProviderClinics(ByVal varClinics As Object, ByVal lngProviderID As Long)
        '--------------------------------------------------------------------
        'Date: 03/29/2000
        'Author: Dave Richkun
        'Description:  Marks one or more Provider-Clinic records as disabled.
        'Parameters: varClinics - A 2-dimensional array that contains ClinicIDs of
        '               Provider-Clinic records NOT to delete.  This is a 2-dimensional
        '               array only because the business object uses other values within
        '               it to perform other processing.  For the purpose of this function
        '               we are only interested in the the ClinicIDs.
        '            lngProviderID - ID of the provider whose clinic associations will be
        '               be disabled.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        ' R001 - Created
        '--------------------------------------------------------------------


        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim strSQL As String
        Dim strClinics As String
        Dim intCTR As Integer





        'Build the Clinic string
        For intCTR = 0 To UBound(varClinics, 2)
            strClinics = strClinics & varClinics(FLD_CLINICID, intCTR)
            If intCTR < UBound(varClinics, 2) Then
                strClinics = strClinics & ", "
            End If
        Next intCTR

        If Trim(strClinics) > "" Then
            'Prepare the SQL statement
            strSQL = "UPDATE "
            strSQL = strSQL & TABLE_NAME
            strSQL = strSQL & " SET "
            strSQL = strSQL & " fldDisabledYN = 'Y', "
            strSQL = strSQL & " fldDateDisabled = '" & Date.Now & "' "
            strSQL = strSQL & " WHERE "
            strSQL = strSQL & " fldProviderID = " & lngProviderID & " AND "
            strSQL = strSQL & " fldClinicID NOT IN (" & strClinics & ")"

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
            cmdSQL = Nothing
            cnnSQL.Close()
            cnnSQL = Nothing
        End If

        'Signal successful completion


        Exit Sub


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.Execute()
        cmdSQL = Nothing
        cnnSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Sub

End Class
