

'--------------------------------------------------------------------
'Class Name: CClearingHouseDB
'Date: 10/06/2004
'Author: Duane C Orth
'Denis J Finkel
'Description:  MTS object designed to host methods associated with
'              data affecting the tblClearingHouse table.
'--------------------------------------------------------------------
'
'--------------------------------------------------------------------
'kjldskjfldskjf
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
Imports Psyquel.BusinessRules.CoreLibraryIII.dbStuff

Public Class CClearingHouseDB

    Private Const CLASS_NAME As String = "CClearingHouseDB"
    Private Const TABLE_NAME As String = "tblClearingHouse"



    Private _ConnectionString As String = String.Empty
    Private _AppPath As String = String.Empty
    Private _EDIOutput As String = String.Empty
    Private _MachineName As String = String.Empty
    Private _UserLoginName As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property





    Dim ds As New db


    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False, _
                          Optional ByVal strWhere As String = "", _
                          Optional ByVal strOrderBy As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/06/2004
        'Author: Duane C Orth
        'Description:  Retrieves records from the tblClearingHouse table.
        'Parameters: blnIncludeDisabled - Optional parameter that identifies if
        '               records flagged as ' or 'De-activated' are to be
        '               included in the record set. The default value is False.
        '            strWhere - Optional 'Where' clause of the SQL statement by
        '               which records will be filtered.  If the parameter is not
        '               supplied, all records will be retrieved.
        '             strOrderBy - Optional 'Order By' clause in which retrieved
        '               records will be sorted.  The default is set to the
        '               'fldLast' and 'fldFirst' columns
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        '    Dim cnn As adodb.Connection
        '    Dim rst As adodb.Recordset
        '    Dim cmd As adodb.Command

        ':

        Dim rst As New ADODB.Recordset
        Dim cmd As New ADODB.Command
        Dim cnn As New ADODB.Connection

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelClearingHouse"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ShowDisabledYN", adVarChar, adParamInput, 1, IIf(blnIncludeDisabled = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@Where", adVarChar, adParamInput, 250, strWhere))
            .Parameters.Append(.CreateParameter("@Order", adVarChar, adParamInput, 25, strOrderBy))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Function
        End If

        Fetch = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        rst = Nothing
        cmd = Nothing
        cnn = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function

    Public Function Insert(ByVal strDescription As String, _
                           ByVal strFolder As String, _
                           ByVal strSubmitterIDQualifier As String, _
                           ByVal strSubmitterID As String, _
                           ByVal blnSubmitterIDUseYN As Boolean, _
                           ByVal strSubmitterPassword As String, _
                           ByVal blnSubmitterPasswordUseYN As Boolean, _
                           ByVal strReceiverIDQualifier As String, _
                           ByVal strReceiverID As String, _
                           ByVal strVersion As String, _
                           ByVal intAcknowledgment As Integer, _
                           ByVal strReceiverCode As String, _
                           ByVal blnReceiverPrefixYN As Boolean, _
                           ByVal strRelease As String, _
                           ByVal blnUseCrLfYN As Boolean, _
                           ByVal strEFileName As String, _
                           ByVal strPhone As String, _
                           ByVal strExt As String, _
                           ByVal strEmail As String, _
                           ByVal strContact As String, _
                           ByVal strNotes As String, _
                           ByVal strAddedBy As String) As Long
        '--------------------------------------------------------------------
        'Date: 10/06/2004
        'Author: Duane C Orth
        'Description:  Inserts a row into the tblClearingHouse table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim intCount As Integer



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspInsClearingHouse"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ClearingHouseID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@Description", adVarChar, adParamInput, 50, strDescription))
            .Parameters.Append(.CreateParameter("@Folder", adVarChar, adParamInput, 20, strFolder))
            .Parameters.Append(.CreateParameter("@SubmitterIDQualifier", adVarChar, adParamInput, 2, strSubmitterIDQualifier))
            .Parameters.Append(.CreateParameter("@SubmitterID", adVarChar, adParamInput, 20, strSubmitterID))
            .Parameters.Append(.CreateParameter("@SubmitterIDUseYN", adVarChar, adParamInput, 1, IIf(blnSubmitterIDUseYN = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@SubmitterPassword", adVarChar, adParamInput, 20, strSubmitterPassword))
            .Parameters.Append(.CreateParameter("@SubmitterPasswordUseYN", adVarChar, adParamInput, 1, IIf(blnSubmitterPasswordUseYN = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@ReceiverIDQualifier", adVarChar, adParamInput, 2, strReceiverIDQualifier))
            .Parameters.Append(.CreateParameter("@ReceiverID", adVarChar, adParamInput, 20, strReceiverID))
            .Parameters.Append(.CreateParameter("@Version", adVarChar, adParamInput, 20, strVersion))
            .Parameters.Append(.CreateParameter("@Acknowledgment", adInteger, adParamInput, , intAcknowledgment))
            .Parameters.Append(.CreateParameter("@ReceiverCode", adVarChar, adParamInput, 20, strReceiverCode))
            .Parameters.Append(.CreateParameter("@ReceiverPrefixYN", adVarChar, adParamInput, 1, IIf(blnReceiverPrefixYN = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@Release", adVarChar, adParamInput, 20, strRelease))
            .Parameters.Append(.CreateParameter("@UseCrLfYN", adVarChar, adParamInput, 1, IIf(blnUseCrLfYN = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@EFileName", adVarChar, adParamInput, 30, strEFileName))
            .Parameters.Append(.CreateParameter("@Phone", adVarChar, adParamInput, 15, strPhone))
            .Parameters.Append(.CreateParameter("@Ext", adVarChar, adParamInput, 6, strExt))
            .Parameters.Append(.CreateParameter("@Email", adVarChar, adParamInput, 60, strEmail))
            .Parameters.Append(.CreateParameter("@Contact", adVarChar, adParamInput, 30, strContact))
            .Parameters.Append(.CreateParameter("@Notes", adVarChar, adParamInput, 255, strNotes))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 20, strAddedBy))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        Insert = cmdSQL.Parameters("@ClearingHouseID").Value

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
            Exit Function
        End If

        'Close the connection
        cnnSQL.Close()

        'free all resources
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
    Public Sub Update(ByVal lngID As Long, _
                           ByVal strDescription As String, _
                           ByVal strFolder As String, _
                           ByVal strSubmitterIDQualifier As String, _
                           ByVal strSubmitterID As String, _
                           ByVal blnSubmitterIDUseYN As Boolean, _
                           ByVal strSubmitterPassword As String, _
                           ByVal blnSubmitterPasswordUseYN As Boolean, _
                           ByVal strReceiverIDQualifier As String, _
                           ByVal strReceiverID As String, _
                           ByVal strVersion As String, _
                           ByVal intAcknowledgment As Integer, _
                           ByVal strReceiverCode As String, _
                           ByVal blnReceiverPrefixYN As Boolean, _
                           ByVal strRelease As String, _
                           ByVal blnUseCrLfYN As Boolean, _
                           ByVal strEFileName As String, _
                           ByVal strPhone As String, _
                           ByVal strExt As String, _
                           ByVal strEmail As String, _
                           ByVal strContact As String, _
                           ByVal strNotes As String)
        '--------------------------------------------------------------------
        'Date: 10/06/2004
        'Author: Duane C Orth
        'Description:  Updates a row in the tblClearingHouse table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: Null
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdClearingHouse"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ClearingHouseID", adInteger, adParamInput, , lngID))
            .Parameters.Append(.CreateParameter("@Description", adVarChar, adParamInput, 50, strDescription))
            .Parameters.Append(.CreateParameter("@Folder", adVarChar, adParamInput, 20, strFolder))
            .Parameters.Append(.CreateParameter("@SubmitterIDQualifier", adVarChar, adParamInput, 2, strSubmitterIDQualifier))
            .Parameters.Append(.CreateParameter("@SubmitterID", adVarChar, adParamInput, 20, strSubmitterID))
            .Parameters.Append(.CreateParameter("@SubmitterIDUseYN", adVarChar, adParamInput, 1, IIf(blnSubmitterIDUseYN = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@SubmitterPassword", adVarChar, adParamInput, 20, strSubmitterPassword))
            .Parameters.Append(.CreateParameter("@SubmitterPasswordUseYN", adVarChar, adParamInput, 1, IIf(blnSubmitterPasswordUseYN = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@ReceiverIDQualifier", adVarChar, adParamInput, 2, strReceiverIDQualifier))
            .Parameters.Append(.CreateParameter("@ReceiverID", adVarChar, adParamInput, 20, strReceiverID))
            .Parameters.Append(.CreateParameter("@Version", adVarChar, adParamInput, 20, strVersion))
            .Parameters.Append(.CreateParameter("@Acknowledgment", adInteger, adParamInput, , intAcknowledgment))
            .Parameters.Append(.CreateParameter("@ReceiverCode", adVarChar, adParamInput, 20, strReceiverCode))
            .Parameters.Append(.CreateParameter("@ReceiverPrefixYN", adVarChar, adParamInput, 1, IIf(blnReceiverPrefixYN = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@Release", adVarChar, adParamInput, 20, strRelease))
            .Parameters.Append(.CreateParameter("@UseCrLfYN", adVarChar, adParamInput, 1, IIf(blnUseCrLfYN = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@EFileName", adVarChar, adParamInput, 30, strEFileName))
            .Parameters.Append(.CreateParameter("@Phone", adVarChar, adParamInput, 15, strPhone))
            .Parameters.Append(.CreateParameter("@Ext", adVarChar, adParamInput, 6, strExt))
            .Parameters.Append(.CreateParameter("@Email", adVarChar, adParamInput, 60, strEmail))
            .Parameters.Append(.CreateParameter("@Contact", adVarChar, adParamInput, 30, strContact))
            .Parameters.Append(.CreateParameter("@Notes", adVarChar, adParamInput, 255, strNotes))
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

    Public Sub UpdateEFile(ByVal lngID As Long, _
                           ByVal strEFileName As String)
        '--------------------------------------------------------------------
        'Date: 12/31/2004
        'Author: Duane C Orth
        'Description:  Updates Electronic Claims File Name in the tblClearingHouse table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: Null
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdClearingHouseEfile"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ClearingHouseID", adInteger, adParamInput, , lngID))
            .Parameters.Append(.CreateParameter("@EFileName", adVarChar, adParamInput, 30, strEFileName))
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


    Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 10/06/2004
        'Author: Duane C Orth
        'Description:  Flags a row in the tblClearingHouse table marking the row as
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
        Dim strSQL2 As String
        Dim blnFlag As String



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
            strSQL = strSQL & "fldClearingHouseID = " & lngID
        Else
            blnFlag = "N"
            'Prepare the SQL statement
            strSQL = "UPDATE "
            strSQL = strSQL & TABLE_NAME
            strSQL = strSQL & " SET "
            strSQL = strSQL & " fldDisabledYN = '" & blnFlag & "' "
            strSQL = strSQL & "WHERE "
            strSQL = strSQL & "fldClearingHouseID = " & lngID
        End If

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
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Sub
    Public Function Exists(ByVal strDescription As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 10/06/2004
        'Author: Duane C Orth
        'Description:  Determines if a Plan Name identical to the
        '              strInsType Name parameter already exists in the table.
        'Parameters: strInsType - Plan name to be checked
        'Returns: True if the name exists, false otherwise
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
        strSQL = strSQL & "COUNT(*) AS TOTAL "
        strSQL = strSQL & "FROM "
        strSQL = strSQL & TABLE_NAME
        strSQL = strSQL & " WHERE "
        strSQL = strSQL & " fldDescription = '" & ds.ParseSQL(strDescription) & "' "

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

        'Get the value from the Recordset then destroy it.
        If rstSQL.Fields("TOTAL").Value > 0 Then
            Exists = True
        Else
            Exists = False
        End If
        rstSQL = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        rstSQL = Nothing
        cnnSQL = Nothing

        'cmdSQL.Execute()
        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function
End Class
