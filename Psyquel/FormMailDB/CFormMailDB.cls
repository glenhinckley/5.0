
'--------------------------------------------------------------------
'Class Name: CFormMailDB                                            '
'Date: 02/02/2000                                                   '
'Author: Rick "Boom Boom" Segura                                    '
'Denis J Finkel
'Description:  MTS object designed to host methods associated with  '
'              data affecting the tblFormMail table.                '
'--------------------------------------------------------------------
' Revision History:
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


Public Class CFormMailDB


    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CFormMailDB"
    Private Const TABLE_NAME As String = "tblFormMail"

    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False, _
                          Optional ByVal strWhere As String = "", _
                          Optional ByVal strOrderBy As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/20/2008                                                   '
        'Author: Rick Duane C Orth                                          '
        'Description:  Retrieves records from the tblFormMail table.        '
        'Parameters:  blnIncludeDisabled - Optional parameter that          '
        '               identifies if records flagged as 'Disabled' or      '
        '               'De-activated' are to be included in the record set.'
        '               The default value is False.                         '
        '             strWhere - Optional 'Where' clause of the SQL statement
        '               by which records will be filtered.  If the parameter'
        '               is not supplied, all records will be retrieved.     '
        '             strOrderBy - Optional 'Order By' clause in which      '
        '               retrieved records will be sorted.  The default is set
        '               to the 'fldLast', 'fldFirst', 'fldMI' column        '
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim strSQL As String
        Dim blnUserWhere As Boolean



        'Prepare the SQL statement.
        strSQL = "SELECT "
        strSQL = strSQL & " A.* "
        strSQL = strSQL & " FROM "
        strSQL = strSQL & TABLE_NAME & " A "

        'Attach user's where clause if applicable
        If strWhere > "" Then
            strSQL = strSQL & "WHERE " & strWhere
        End If

        ' Build where clause appropriately
        If blnIncludeDisabled Then  'Include disabled records

        Else    'Filter out disabled records
            If InStr(1, strSQL, "WHERE") > 0 Then
                strSQL = " AND B.fldDisabledYN = 'N' "
            Else
                strSQL = strSQL & "WHERE " & " B.fldDisabledYN = 'N' "
            End If
        End If

        If Trim(strOrderBy) > "" Then
            strSQL = strSQL & " ORDER BY "
            strSQL = strSQL & strOrderBy
        Else
            strSQL = strSQL & " ORDER BY "
            strSQL = strSQL & "fldDateAdded "
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
        Fetch = rstSQL
        cnnSQL.Close()
        cnnSQL = Nothing


        'Signal successful completion



    End Function

    Public Function Insert(ByVal strRecipient As String, ByVal strBrowser As String, _
                           ByVal strFullname As String, ByVal strEmail As String, _
                           ByVal strBody As String) As Long
        '--------------------------------------------------------------------
        'Date: 09/20/2008                                                   '
        'Author: Rick Duane C Orth                                          '
        'Description:  Inserts a row into the tblFormMail table utilizing   '
        '              a stored procedure.                                  '
        'Parameters: strRecipient - Recipient                               '
        '            strBrowser - Subject                                   '
        '            strFullname - Full name                                '
        '            strEmail - "" E-mail Address                           '
        '            strBody - Body                                         '
        '            strDisabledYN - Provider's deleted flag                '
        'Returns: ID (Primary Key) of the row inserted                      '
        '--------------------------------------------------------------------


        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim prmSQL As ADODB.Parameter



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspInsFormMail"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@Recipient", adVarChar, adParamInput, 200, strRecipient))
            .Parameters.Append(.CreateParameter("@Fullname", adVarChar, adParamInput, 75, strFullname))
            .Parameters.Append(.CreateParameter("@Email", adVarChar, adParamInput, 200, strEmail))
            .Parameters.Append(.CreateParameter("@Body", adVarChar, adParamInput, 8000, strBody))
            .Parameters.Append(.CreateParameter("@Browser", adVarChar, adParamInput, 200, strBrowser))
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

        Insert = cmdSQL.Parameters("@ID").Value

        'Close the connection and free all resources
        cmdSQL.ActiveConnection = Nothing

        'Close the connection and free all resources
        cnnSQL.Close()

        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion




    End Function

    Public Sub Update(ByVal lngID As Long, ByVal strRecipient As String, ByVal strBrowser As String, ByVal strFullname As String, ByVal strEmail As String, ByVal strBody As String, ByVal strUpdatedBy As String)
        '--------------------------------------------------------------------
        'Date: 09/20/2008                                                   '
        'Author: Rick Duane C Orth                                          '
        'Description:  Updates a row into the tblFormMail table utilizing   '
        '              a stored procedure.                                  '
        'Parameters: lngID - ID of the row in the table whose value will be '
        '                    updated.                                       '
        '            strRecipient - Recipient                               '
        '            strBrowser - Subject                                   '
        '            strFullname - Full name                                '
        '            strEmail - "" E-mail Address                           '
        '            strBody - Body                                         '
        '            strDisabledYN - "" deleted flag                        '
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        With cmdSQL
            .CommandText = "uspUpdFormMail"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamInput, , lngID))
            .Parameters.Append(.CreateParameter("@Recipient", adVarChar, adParamInput, 200, strRecipient))
            .Parameters.Append(.CreateParameter("@Fullname", adVarChar, adParamInput, 75, strFullname))
            .Parameters.Append(.CreateParameter("@Email", adVarChar, adParamInput, 200, strEmail))
            .Parameters.Append(.CreateParameter("@Body", adVarChar, adParamInput, 8000, strBody))
            .Parameters.Append(.CreateParameter("@Browser", adVarChar, adParamInput, 200, strBrowser))
            .Parameters.Append(.CreateParameter("@UpdatedBy", adVarChar, adParamInput, 75, strUpdatedBy))
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



    End Sub
    Public Function Exists(ByVal strFullname As String, ByVal strEmail As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 09/20/2008                                                   '
        'Author: Rick Duane C Orth                                          '
        'Description:  Determines if a provider record identical to the     '
        '              name parameters is already associated to the clinic  '
        '              whose ID matches lngClinic.                          '
        'Parameters: strFullname - Full name                                '
        '            strEmail - "" E-mail Address                           '
        'Returns: True if the association exists, false otherwise           '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim strSQL As String
        Dim blnExists As Boolean



        'Prepare the SQL statement
        strSQL = "SELECT "
        strSQL = strSQL & " fldFormMailID "
        strSQL = strSQL & " FROM "
        strSQL = strSQL & TABLE_NAME
        strSQL = strSQL & " WHERE "
        strSQL = strSQL & " fldFullname = '"
        strSQL = strSQL & strFullname

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
        If rstSQL.RecordCount > 0 Then
            Exists = True
        Else
            Exists = False
        End If

        rstSQL = Nothing

    End Function

    Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Long, ByVal strDeletedBy As String)
        '--------------------------------------------------------------------
        'Date: 09/20/2008                                                   '
        'Author: Rick Duane C Orth                                          '
        'Description:  Flags a row in the tblFormMail table marking the row '
        '              as deleted or undeleted.                             '
        'Parameters: blnDeleted - Boolean value identifying if the record is'
        '               to be deleted (True) or undeleted (False).          '
        '            lngID - ID of the row in the table whose value will be '
        '               updated.                                            '
        '            strDeletedBy - Login name of the user performing the   '
        '               action.                                             '
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
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
        strSQL = strSQL & " fldDateDisabled = '" & Date.Now & "', "
        strSQL = strSQL & " fldDisabledBy = '" & strDeletedBy & "' "
        strSQL = strSQL & " WHERE "
        strSQL = strSQL & " fldFormMailID = " & lngID

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


    End Sub

End Class
