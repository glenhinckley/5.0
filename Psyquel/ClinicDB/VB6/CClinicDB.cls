VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CClinicDB"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CClinicDB                                              '
'Date: 12/05/1999                                                   '
'Author: Dave Richkun                                               '
'Description:  MTS object designed to host methods associated with  '
'              data affecting the tblClinic table.                  '
'--------------------------------------------------------------------
' Revisions:                                                        '
'   03/02/2000 Segura:  Removed Fetch and added specific fetch      '
'                       methods                                     '
'--------------------------------------------------------------------
Option Explicit

Private Const CLASS_NAME As String = "CClinicDB"
Private Const TABLE_NAME As String = "tblClinic"

Public Function FetchDetail(ByVal lngID As Long, Optional blnIncludeDisabled As Boolean = False) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 03/06/2000                                                   '
'Author: Rick "Boom Boom" Segura                                    '
'Description:  Retrieves a record from the tblClinic table.         '
'Parameters: lngID - ID of clinic to fetch info for                 '
'            blnIncludeDisabled - Optional parameter that identifies'
'               if records flagged as 'Disabled' or 'De-activated'  '
'               are to be included in the record set. The default   '
'               value is False.                                     '
'Returns: Recordset of requested clinics ordered by business name   '
'--------------------------------------------------------------------

    Dim cnn As ADODB.Connection
    Dim cmd As ADODB.Command
    Dim rst As ADODB.Recordset
    Dim strShowDisabled As String
    Dim SQLErrorNum As Long
    
    On Error GoTo ErrTrap:

    Set rst = New ADODB.Recordset
    Set cmd = New ADODB.Command
    Set cnn = New ADODB.Connection
    
    'Acquire the database connection.
    cnn.Open (CONST_PSYQUEL_CNN)
    Set cmd.ActiveConnection = cnn
    
    If blnIncludeDisabled Then
        strShowDisabled = "Y"
    Else
        strShowDisabled = "N"
    End If
    
    'Create the parameter objects
    With cmd
        .CommandText = "uspSelClinicDetail"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@ClinicID", adInteger, adParamInput, , lngID)
        .Parameters.Append .CreateParameter("@ShowDisabled", adVarChar, adParamInput, 1, strShowDisabled)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If
    
    'Execute the stored procedure
    rst.CursorLocation = adUseClient
    rst.Open cmd, , adOpenForwardOnly, adLockReadOnly
    
    Set FetchDetail = rst
    
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
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

Public Function FetchByGroup(ByVal lngGroupID As Long, Optional blnIncludeDisabled As Boolean = False) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 02/03/2015                                                   '
'Author: Duane C Orth                                               '
'Description:  Retrieves records from the tblClinic table.          '
'Parameters: blnIncludeDisabled - Optional parameter that identifies'
'               if records flagged as 'Disabled' or 'De-activated'  '
'               are to be included in the record set. The default   '
'               value is False.                                     '
'Returns: Recordset of requested clinics ordered by Group ID        '
'--------------------------------------------------------------------

    Dim cnn As ADODB.Connection
    Dim cmd As ADODB.Command
    Dim rst As ADODB.Recordset
    Dim strShowDisabled As String
    Dim SQLErrorNum As Long
    
    On Error GoTo ErrTrap:

    Set rst = New ADODB.Recordset
    Set cmd = New ADODB.Command
    Set cnn = New ADODB.Connection
    
    'Acquire the database connection.
    cnn.Open (CONST_PSYQUEL_CNN)
    Set cmd.ActiveConnection = cnn
    
    If blnIncludeDisabled Then
        strShowDisabled = "Y"
    Else
        strShowDisabled = "N"
    End If
    
    'Create the parameter objects
    With cmd
        .CommandText = "uspSelClinicByGroup"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@GroupID", adInteger, adParamInput, , lngGroupID)
        .Parameters.Append .CreateParameter("@ShowDisabled", adVarChar, adParamInput, 1, strShowDisabled)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Execute the stored procedure
    rst.CursorLocation = adUseClient
    rst.Open cmd, , adOpenForwardOnly, adLockReadOnly
    
    Set FetchByGroup = rst
    
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
    Err.Raise Err.Number, Err.Source, Err.Description

End Function



Public Function FetchByName(Optional blnIncludeDisabled As Boolean = False) _
    As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 03/02/2000                                                   '
'Author: Rick "Boom Boom" Segura                                    '
'Description:  Retrieves records from the tblClinic table.          '
'Parameters: blnIncludeDisabled - Optional parameter that identifies'
'               if records flagged as 'Disabled' or 'De-activated'  '
'               are to be included in the record set. The default   '
'               value is False.                                     '
'Returns: Recordset of requested clinics ordered by business name   '
'--------------------------------------------------------------------

    Dim cnn As ADODB.Connection
    Dim cmd As ADODB.Command
    Dim rst As ADODB.Recordset
    Dim strShowDisabled As String
    Dim SQLErrorNum As Long
    
    On Error GoTo ErrTrap:

    Set rst = New ADODB.Recordset
    Set cmd = New ADODB.Command
    Set cnn = New ADODB.Connection
    
    'Acquire the database connection.
    cnn.Open (CONST_PSYQUEL_CNN)
    Set cmd.ActiveConnection = cnn
    
    If blnIncludeDisabled Then
        strShowDisabled = "Y"
    Else
        strShowDisabled = "N"
    End If
    
    'Create the parameter objects
    With cmd
        .CommandText = "uspSelClinicByName"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@ShowDisabled", adVarChar, adParamInput, 1, strShowDisabled)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Execute the stored procedure
    rst.CursorLocation = adUseClient
    rst.Open cmd, , adOpenForwardOnly, adLockReadOnly
    
    Set FetchByName = rst
    
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
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

Public Function FetchByZip(Optional blnIncludeDisabled As Boolean = False) _
    As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 03/02/2000                                                   '
'Author: Rick "Boom Boom" Segura                                    '
'Description:  Retrieves records from the tblClinic table.          '
'Parameters: blnIncludeDisabled - Optional parameter that identifies'
'               if records flagged as 'Disabled' or 'De-activated'  '
'               are to be included in the record set. The default   '
'               value is False.                                     '
'Returns: Recordset of requested clinics ordered by zip code        '
'--------------------------------------------------------------------

    Dim cnn As ADODB.Connection
    Dim cmd As ADODB.Command
    Dim rst As ADODB.Recordset
    Dim strShowDisabled As String
    Dim SQLErrorNum As Long
    
    On Error GoTo ErrTrap:

    Set rst = New ADODB.Recordset
    Set cmd = New ADODB.Command
    Set cnn = New ADODB.Connection
    
    'Acquire the database connection.
    cnn.Open (CONST_PSYQUEL_CNN)
    Set cmd.ActiveConnection = cnn
    
    If blnIncludeDisabled Then
        strShowDisabled = "Y"
    Else
        strShowDisabled = "N"
    End If
    
    'Create the parameter objects
    With cmd
        .CommandText = "uspSelClinicByZip"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@ShowDisabled", adVarChar, adParamInput, 1, strShowDisabled)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Execute the stored procedure
    rst.CursorLocation = adUseClient
    rst.Open cmd, , adOpenForwardOnly, adLockReadOnly
    
    Set FetchByZip = rst
    
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
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

Public Function FetchByCity(Optional blnIncludeDisabled As Boolean = False) _
    As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 03/02/2000                                                   '
'Author: Rick "Boom Boom" Segura                                    '
'Description:  Retrieves records from the tblClinic table.          '
'Parameters: blnIncludeDisabled - Optional parameter that identifies'
'               if records flagged as 'Disabled' or 'De-activated'  '
'               are to be included in the record set. The default   '
'               value is False.                                     '
'Returns: Recordset of requested clinics ordered by city            '
'--------------------------------------------------------------------

    Dim cnn As ADODB.Connection
    Dim cmd As ADODB.Command
    Dim rst As ADODB.Recordset
    Dim strShowDisabled As String
    Dim SQLErrorNum As Long
    
    On Error GoTo ErrTrap:

    Set rst = New ADODB.Recordset
    Set cmd = New ADODB.Command
    Set cnn = New ADODB.Connection
    
    'Acquire the database connection.
    cnn.Open (CONST_PSYQUEL_CNN)
    Set cmd.ActiveConnection = cnn
    
    If blnIncludeDisabled Then
        strShowDisabled = "Y"
    Else
        strShowDisabled = "N"
    End If
    
    'Create the parameter objects
    With cmd
        .CommandText = "uspSelClinicByCity"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@ShowDisabled", adVarChar, adParamInput, 1, strShowDisabled)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Execute the stored procedure
    rst.CursorLocation = adUseClient
    rst.Open cmd, , adOpenForwardOnly, adLockReadOnly
    
    Set FetchByCity = rst
    
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
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

Public Function FetchByState(Optional blnIncludeDisabled As Boolean = False) _
    As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 03/02/2000                                                   '
'Author: Rick "Boom Boom" Segura                                    '
'Description:  Retrieves records from the tblClinic table.          '
'Parameters: blnIncludeDisabled - Optional parameter that identifies'
'               if records flagged as 'Disabled' or 'De-activated'  '
'               are to be included in the record set. The default   '
'               value is False.                                     '
'Returns: Recordset of requested clinics ordered by state           '
'--------------------------------------------------------------------

    Dim cnn As ADODB.Connection
    Dim cmd As ADODB.Command
    Dim rst As ADODB.Recordset
    Dim strShowDisabled As String
    Dim SQLErrorNum As Long
    
    On Error GoTo ErrTrap:

    Set rst = New ADODB.Recordset
    Set cmd = New ADODB.Command
    Set cnn = New ADODB.Connection
    
    'Acquire the database connection.
    cnn.Open (CONST_PSYQUEL_CNN)
    Set cmd.ActiveConnection = cnn
    
    If blnIncludeDisabled Then
        strShowDisabled = "Y"
    Else
        strShowDisabled = "N"
    End If
    
    'Create the parameter objects
    With cmd
        .CommandText = "uspSelClinicByZip"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@ShowDisabled", adVarChar, adParamInput, 1, strShowDisabled)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Execute the stored procedure
    rst.CursorLocation = adUseClient
    rst.Open cmd, , adOpenForwardOnly, adLockReadOnly
    
    Set FetchByState = rst
    
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
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

Public Function Insert(ByVal strBusinessName As String, ByVal strAddress1 As String, _
                  ByVal strAddress2 As String, ByVal strCity As String, _
                  ByVal strState As String, ByVal strZip As String, _
                  ByVal strPhone As String, ByVal strFax As String, _
                  ByVal strSubmitterID As String, ByVal strPOSCode As String, _
                  ByVal strNPI As String, ByVal strTaxonomy As String, _
                  ByVal strAddedBy As String, ByVal strClinicType As String, _
                  ByVal strClaimType As String, ByVal blnShortageAreaYN As Boolean) As Long
'--------------------------------------------------------------------
'Date: 12/05/1999
'Author: Dave Richkun
'Description:  Inserts a row into the tblClinic table utilizing
'              a stored procedure.
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'Returns: ID (Primary Key) of the row inserted.
'--------------------------------------------------------------------

    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command

    On Error GoTo ErrTrap:

    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    
    'Create the parameter objects
    With cmdSQL
        .CommandText = "uspInsClinic"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@ID", adInteger, adParamReturnValue)
        .Parameters.Append .CreateParameter("@BusinessName", adVarChar, adParamInput, 50, strBusinessName)
        .Parameters.Append .CreateParameter("@Address1", adVarChar, adParamInput, 60, strAddress1)
        .Parameters.Append .CreateParameter("@Address2", adVarChar, adParamInput, 60, strAddress2)
        .Parameters.Append .CreateParameter("@City", adVarChar, adParamInput, 40, strCity)
        .Parameters.Append .CreateParameter("@State", adVarChar, adParamInput, 2, strState)
        .Parameters.Append .CreateParameter("@Zip", adVarChar, adParamInput, 12, strZip)
        .Parameters.Append .CreateParameter("@Phone", adVarChar, adParamInput, 15, NumbersOnly(strPhone))
        .Parameters.Append .CreateParameter("@Fax", adVarChar, adParamInput, 15, NumbersOnly(strFax))
        .Parameters.Append .CreateParameter("@ClinicType", adVarChar, adParamInput, 1, strClinicType)
        .Parameters.Append .CreateParameter("@ClaimType", adVarChar, adParamInput, 1, strClaimType)
        .Parameters.Append .CreateParameter("@SubmitterID", adVarChar, adParamInput, 16, strSubmitterID)
        .Parameters.Append .CreateParameter("@POSCode", adVarChar, adParamInput, 2, strPOSCode)
        .Parameters.Append .CreateParameter("@NPI", adVarChar, adParamInput, 10, strNPI)
        .Parameters.Append .CreateParameter("@Taxonomy", adVarChar, adParamInput, 15, strTaxonomy)
        .Parameters.Append .CreateParameter("@ShortageAreaYN", adVarChar, adParamInput, 1, IIf(blnShortageAreaYN, "Y", "N"))
        .Parameters.Append .CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strAddedBy)
        .Parameters.Append .CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open(CONST_PSYQUEL_CNN)
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute , , adExecuteNoRecords

    Insert = cmdSQL.Parameters("@ID").Value
    
    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If

    'Close the connection and free all resources
    cnnSQL.Close
    
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing

    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Call RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME)
    
End Function

Public Sub Update(ByVal lngID As Long, ByVal strBusinessName As String, _
                  ByVal strAddress1 As String, ByVal strAddress2 As String, _
                  ByVal strCity As String, ByVal strState As String, _
                  ByVal strZip As String, ByVal strPhone As String, _
                  ByVal strFax As String, ByVal strSubmitterID As String, _
                  ByVal strPOSCode As String, ByVal strNPI As String, _
                  ByVal strTaxonomy As String, ByVal strClinicType As String, _
                  ByVal strClaimType As String, ByVal blnShortageAreaYN As Boolean)
'--------------------------------------------------------------------
'Date: 12/27/1999
'Author: Dave Richkun
'Description:  Updates a row in the tblClinic table utilizing
'              a stored procedure.
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'Returns: Null
'--------------------------------------------------------------------

    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command

    On Error GoTo ErrTrap:

    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    
    With cmdSQL
        .CommandText = "uspUpdClinic"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@ClinicID", adInteger, adParamInput, , lngID)
        .Parameters.Append .CreateParameter("@BusinessName", adVarChar, adParamInput, 50, strBusinessName)
        .Parameters.Append .CreateParameter("@Address1", adVarChar, adParamInput, 60, strAddress1)
        .Parameters.Append .CreateParameter("@Address2", adVarChar, adParamInput, 60, strAddress2)
        .Parameters.Append .CreateParameter("@City", adVarChar, adParamInput, 40, strCity)
        .Parameters.Append .CreateParameter("@State", adVarChar, adParamInput, 2, strState)
        .Parameters.Append .CreateParameter("@Zip", adVarChar, adParamInput, 12, strZip)
        .Parameters.Append .CreateParameter("@Phone", adVarChar, adParamInput, 15, NumbersOnly(strPhone))
        .Parameters.Append .CreateParameter("@Fax", adVarChar, adParamInput, 15, NumbersOnly(strFax))
        .Parameters.Append .CreateParameter("@ClinicType", adVarChar, adParamInput, 1, strClinicType)
        .Parameters.Append .CreateParameter("@ClaimType", adVarChar, adParamInput, 1, strClaimType)
        .Parameters.Append .CreateParameter("@SubmitterID", adVarChar, adParamInput, 16, strSubmitterID)
        .Parameters.Append .CreateParameter("@POSCode", adVarChar, adParamInput, 2, strPOSCode)
        .Parameters.Append .CreateParameter("@NPI", adVarChar, adParamInput, 10, strNPI)
        .Parameters.Append .CreateParameter("@Taxonomy", adVarChar, adParamInput, 15, strTaxonomy)
        .Parameters.Append .CreateParameter("@ShortageAreaYN", adVarChar, adParamInput, 1, IIf(blnShortageAreaYN, "Y", "N"))
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open(CONST_PSYQUEL_CNN)
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute , , adExecuteNoRecords

    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If

    'Close the connection and free all resources
    cnnSQL.Close
    
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing

    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Call RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME)
    
End Sub


Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Long, _
                   ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 12/30/1999
'Author: Dave Richkun
'Description:  Flags a row in the tblClinic table marking the row as
'              deleted or undeleted.
'Parameters: blnDeleted - Boolean value identifying if the record is to
'               be deleted (True) or undeleted (False).
'            lngID - ID of the row in the table whose value will be
'               updated.
'            strUserName - Login name of the user responsible for
'               marking the row as deleted.
'Returns: Null
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command
    Dim strSQL As String
    Dim blnFlag As String

    On Error GoTo ErrTrap:

    If blnDeleted = True Then
        blnFlag = "Y"
        'Prepare the SQL statement
        strSQL = "UPDATE "
        strSQL = strSQL & TABLE_NAME
        strSQL = strSQL & " SET "
        strSQL = strSQL & " fldDisabledYN = '" & blnFlag & "', "
        strSQL = strSQL & " fldDateDisabled = '" & Now & "', "
        strSQL = strSQL & " fldDisabledBy = '" & strUserName & "' "
        strSQL = strSQL & "WHERE "
        strSQL = strSQL & "fldClinicID = " & lngID
    Else
        blnFlag = "N"
        'Prepare the SQL statement
        strSQL = "UPDATE "
        strSQL = strSQL & TABLE_NAME
        strSQL = strSQL & " SET "
        strSQL = strSQL & " fldDisabledYN = '" & blnFlag & "' "
        strSQL = strSQL & "WHERE "
        strSQL = strSQL & "fldClinicID = " & lngID
    End If

    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    cmdSQL.CommandText = strSQL
    cmdSQL.CommandType = adCmdText
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open(CONST_PSYQUEL_CNN)
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute , , adExecuteNoRecords

    'Close the connection and free all resources
    cnnSQL.Close
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing

    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub


Public Function Exists(strBusinessName As String) As Boolean
'--------------------------------------------------------------------
'Date: 01/05/2000
'Author: Dave Richkun
'Description:  Determines if a Business Name identical to the
'              strBusiness Name parameter already exists in the table.
'Parameters: strBusinessName - Business name to be checked
'Returns: True if the name exists, false otherwise
'--------------------------------------------------------------------
'Revision History:
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
    strSQL = strSQL & " fldBusinessName = '" & ParseSQL(strBusinessName) & "' "
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open(CONST_PSYQUEL_CNN)
    
    'Instantiate and populate the Recordset.
    Set rstSQL = New ADODB.Recordset
    rstSQL.CursorLocation = adUseClient
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
    Err.Raise Err.Number, Err.Source, Err.Description
End Function



