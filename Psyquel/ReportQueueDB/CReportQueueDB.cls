
'--------------------------------------------------------------------
'Class Name: CReportQueueDB
'Date: 04/26/2001
'Author: Eric Pena
'Denis J Finkel
'Description:  MTS object designed to host methods associated with data affecting the tblReportQueue  table.
'--------------------------------------------------------------------
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

Public Class CReportQueueDB


    Private _ConnectionString As String = String.Empty
    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property



    Private Const CLASS_NAME As String = "CReportQueueDB"
    Private Const TABLE_NAME As String = "tblReportQueue"





    Public Function Insert(ByVal strRptName As String, ByVal strExportType As String, _
                           ByVal strRptParams As String, ByVal strDescription As String, _
                           ByVal lngUserID As Long, ByVal strUserName As String) As Long
        '--------------------------------------------------------------------
        'Date: 04/26/2001
        'Author: Eric Pena
        'Description:  Inserts a row into the tblReportQueue table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim strTemp As String


        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspInsReportQueue"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@RptID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@RptName", adVarChar, adParamInput, 30, strRptName))
            .Parameters.Append(.CreateParameter("@ExportType", adVarChar, adParamInput, 1, strExportType))
            .Parameters.Append(.CreateParameter("@RptParams", adVarChar, adParamInput, 1000, strRptParams))
            .Parameters.Append(.CreateParameter("@Description", adVarChar, adParamInput, 40, strDescription))
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 20, strUserName))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        Insert = cmdSQL.Parameters("@RptID").Value

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then Exit Function

        'Close the connection
        cnnSQL.Close()

        'free all resources
        cmdSQL = Nothing
        cnnSQL = Nothing


    End Function

    Public Sub Process(ByVal lngID As Long)
        '--------------------------------------------------------------------
        'Date: 04/26/2001
        'Author: Eric Pena
        'Description:  Updates a row in the tblReportQueue  table utilizing a stored procedure.
        'Parameters: lngID - The ID of the row that will be updated
        'Returns: Null
        '--------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command





        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdProcessReportQueue"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@InsuranceID", adInteger, adParamInput, , lngID))
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

    End Sub
    Public Function Delete(ByVal lngRptID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 04/26/2001
        'Author: Eric Pena
        'Description:  Flags a row in the tblReportQueue  table marking the row as deleted.
        'Parameters: lngID - ID of the row in the table whose value will be updated.
        '            strUserName - Login name of the user responsible for marking the row as deleted.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim strTemp As String



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspDelReportQueue"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@RptID", adInteger, adParamInput, , lngRptID))
            .Parameters.Append(.CreateParameter("@DeletedBy", adVarChar, adParamInput, 20, strUserName))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then Exit Function

        'Close the connection
        cnnSQL.Close()

        'free all resources
        cmdSQL = Nothing
        cnnSQL = Nothing


    End Function
    Public Function Remove(ByVal lngRptID As Long)
        '--------------------------------------------------------------------
        'Date: 04/26/2001
        'Author: Eric Pena
        'Description:  Flags a row in the tblReportQueue  table marking the row as removed.
        'Parameters: lngID - ID of the row in the table whose value will be updated.
        '            strUserName - Login name of the user responsible for marking the row as deleted.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim strTemp As String



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspUpdRemoveReportQueue"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@RptID", adInteger, adParamInput, , lngRptID))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then Exit Function

        'Close the connection
        cnnSQL.Close()

        'free all resources
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion

        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        strTemp = cmdSQL.Parameters("@ErrorNum").Value
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()
        'RaiseError(Err, strTemp, CLASS_NAME)
    End Function
    Public Function FetchByUser(ByVal lngUserID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 04/26/2001
        'Author: Eric Pena
        'Description:  Retrieves records from the tblReportQueue table utilizing a stored procedure.
        'Parameters: None
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        Dim cnn As ADODB.Connection
        Dim rst As ADODB.Recordset
        Dim cmd As ADODB.Command



        rst = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelUserReports"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
        End With

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByUser = rst

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


    Public Function FetchByUserName(ByVal strUserName As String) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/02/2003
        'Author: Dave Richkun
        'Description:  Retrieves undeleted report records from tblReportQueue
        '              added by a specific user.
        'Parameters: strUserName - Name of user who added the report
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        Dim cnn As ADODB.Connection
        Dim rst As ADODB.Recordset
        Dim cmd As ADODB.Command



        rst = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelUserReportsByUserName"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName))
        End With

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByUserName = rst

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



    Public Function FetchByID(ByVal lngReportID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 04/03/2003
        'Author: Dave Richkun
        'Description:  Retrieves a single report record from the database
        'Parameters: lngReportID - ID Of report to be retrieved
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        Dim cnn As ADODB.Connection
        Dim rst As ADODB.Recordset
        Dim cmd As ADODB.Command



        rst = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelReportByID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ReportID", adInteger, adParamInput, , lngReportID))
        End With

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByID = rst

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


    Public Function FetchQueuedByUser(ByVal lngUserID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 04/26/2001
        'Author: Eric Pena
        'Description:  Retrieves records from the tblReportQueue table utilizing a stored procedure.
        'Parameters: None
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        Dim cnn As ADODB.Connection
        Dim rst As ADODB.Recordset
        Dim cmd As ADODB.Command



        rst = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelUserQueuedReports"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
        End With

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchQueuedByUser = rst

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
    Public Function FetchQueued() As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 04/26/2001
        'Author: Eric Pena
        'Description:  Retrieves records from the tblReportQueue table utilizing a stored procedure.
        'Parameters: None
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        Dim cnn As ADODB.Connection
        Dim rst As ADODB.Recordset
        Dim cmd As ADODB.Command



        rst = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelQueuedReports"
            .CommandType = adCmdStoredProc
        End With

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchQueued = rst

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
    Public Function FetchPendingRemove() As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 04/26/2001
        'Author: Eric Pena
        'Description:  Retrieves records from the tblReportQueue table utilizing a stored procedure.
        'Parameters: None
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        Dim cnn As ADODB.Connection
        Dim rst As ADODB.Recordset
        Dim cmd As ADODB.Command



        rst = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelPendRemoveRpts"
            .CommandType = adCmdStoredProc
        End With

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchPendingRemove = rst

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

End Class
