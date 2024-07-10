
'--------------------------------------------------------------------
'Class Name: CAlertDB
'Date: 08/31/2000
'Author: Dave Nichol and Joshua Lockwood
'Description:  Business object designed to host methods associated with
'              data affecting the tblPatientAlert  table.
'--------------------------------------------------------------------
'  Revisions:
'   --R001 : Joshua Lockwood, 15Sep00 : Changes to guard against
'           memory leaks
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
' Lydia Orth


Public Class CPatientAlertDB

    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CAlertDB"
    Private Const TABLE_NAME As String = "tblPatientAlert"

    Public Function FetchByID(ByVal lngPatientAlertID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/31/2000
        'Author: Dave Nichol and Joshua Lockwood
        'Description: Retrieves records from the tblPatientAlert table.  s
        '               stored procedure uspSelPatientAlertByID.
        'Parameters: blnIncludeDisabled - Optional parameter that identifies if
        '               records flagged as 'Disabled' or 'De-activated' are to be
        '               included in the record set. The default value is False.
        '            lngPatientAlertID - 'Where' clause of the SQL statement by
        '               which records will be fetched.  Matches tblPatientAlert's
        '               primary key.
        '             lngUserID - UserID of the user performing the fetch.  Not
        '               really used currently.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection
        Dim rst As ADODB.Recordset
        Dim cmd As New ADODB.Command



        'Create the parameter objects
        'Instantiate SQL statement --R001
        With cmd
            .CommandText = "uspSelPatientAlertByID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientAlertID", adInteger, adParamInput, , lngPatientAlertID))
        End With

        'Acquire the database connection.
        'Instantiate DB connection --R001
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Execute the stored procedure
        'Instantiate recordset --R001
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByID = rst

        rst.ActiveConnection = Nothing  '--R001
        cmd.ActiveConnection = Nothing
        cnn.Close()
        cmd = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()

        rst.ActiveConnection = Nothing ' --R001 ... can't do?
        rst.Close()   ' --R001 ... can't do?
        cmd.ActiveConnection = Nothing
        cnn.Close()

        rst = Nothing
        cmd = Nothing
        cnn = Nothing

        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function

    Public Function FetchByPatient(ByVal lngPatientID As Long, Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/31/2000
        'Author: Dave Nichol and Joshua Lockwood
        'Description: Retrieves records from the tblPatientAlert table.  s
        '               stored procedure uspSelPatientAlertByID.
        'Parameters: blnIncludeDisabled - Optional parameter that identifies if
        '               records flagged as 'Disabled' or 'De-activated' are to be
        '               included in the record set. The default value is False.
        '            lngPatientID - 'Where' clause of the SQL statement by
        '               which records will be fetched.
        '             lngUserID - UserID of the user performing the fetch.  Not
        '               really used currently.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection
        Dim rst As ADODB.Recordset
        Dim cmd As New ADODB.Command




        'Create the parameter objects
        ' Instantiate SQL stament --R001
        With cmd
            .CommandText = "uspSelPatientAlertByPatient"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@IncludeDisabled", adVarChar, adParamInput, 1, IIf(blnIncludeDisabled = False, "N", "Y")))
        End With

        'Acquire the database connection.
        ' --R001
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Execute the stored procedure
        ' --R001
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByPatient = rst

        'Disconnect the recordset
        rst.ActiveConnection = Nothing
        cmd.ActiveConnection = Nothing
        cnn.Close()
        cmd = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()

        rst.ActiveConnection = Nothing
        cmd.ActiveConnection = Nothing
        rst.Close()   ' --R001
        cnn.Close()
        rst = Nothing
        cmd = Nothing
        cnn = Nothing

        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function


    Public Function FetchByAlert(ByVal strAlertText As String, Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/31/2000
        'Author: Dave Nichol and Joshua Lockwood
        'Description: Retrieves records from the tblPatientAlert table.  s
        '               stored procedure uspSelPatientAlertByAlert.
        'Parameters: blnIncludeDisabled - Optional parameter that identifies if
        '               records flagged as 'Disabled' or 'De-activated' are to be
        '               included in the record set. The default value is False.
        '            strAlertText - Text of the alert itself.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection
        Dim rst As ADODB.Recordset
        Dim cmd As New ADODB.Command



        'Create the parameter objects
        ' --R001
        With cmd
            .CommandText = "uspSelPatientAlertByAlert"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@AlertText", adVarChar, adParamInput, 256, strAlertText))
            .Parameters.Append(.CreateParameter("@IncludeDisabled", adVarChar, adParamInput, 1, IIf(blnIncludeDisabled = False, "N", "Y")))
        End With

        'Acquire the database connection.
        ' --R001
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Execute the stored procedure
        ' --R001
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByAlert = rst

        'Disconnect the recordset
        rst.ActiveConnection = Nothing
        cmd.ActiveConnection = Nothing
        cnn.Close()
        cmd = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()

        rst.ActiveConnection = Nothing
        cmd.ActiveConnection = Nothing
        rst.Close()
        cnn.Close()
        rst = Nothing
        cmd = Nothing
        cnn = Nothing

        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function


    Public Function Insert(ByVal lngPatientID As Long, ByVal strAlertText As String, ByVal strUserName As String) As Long
        '--------------------------------------------------------------------
        'Date: 08/31/2000
        'Author: Dave Nichol and Joshua Lockwood
        'Description: Inserts records into the tblPatientAlert table.  s
        '               stored procedure uspInsPatientAlert.
        'Parameters:  strAlertText - Text of the alert itself.
        '             lngPatientID - ID of the patient.
        '             strUserName - username of user inserting data
        'Returns: Long
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection
        Dim cmd As New ADODB.Command



        'Create the parameter objects
        ' --R001
        With cmd
            .CommandText = "uspInsPatientAlert"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientAlertID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@AlertText", adVarChar, adParamInput, 256, strAlertText))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure

        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn
        cmd.Execute()

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Function
        End If

        Insert = cmd.Parameters(0).Value

        'Disconnect the recordset
        cnn.Close()
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()

        cmd.ActiveConnection = Nothing
        cnn.Close()
        cmd = Nothing
        cnn = Nothing

        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function

    Public Function Update(ByVal lngPatientAlertID As Long, ByVal lngPatientID As Long, ByVal strAlertText As String, ByVal strUserName As String) As Long
        '--------------------------------------------------------------------
        'Date: 08/31/2000
        'Author: Dave Nichol and Joshua Lockwood
        'Description: Updates selected records in the tblPatientAlert table.  s
        '               stored procedure uspUpdPatientAlert.
        'Parameters:  lngPatientAlertID - ID of the patient.
        '             lstrAlertText - Text of the alert itself.
        '             lngPatientID - ID of the patient.
        '             strUserName - username of user inserting data
        'Returns: Long
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection
        Dim cmd As New ADODB.Command




        'Create the parameter objects
        ' --R001
        With cmd
            .CommandText = "uspUpdPatientAlert"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientAlertID", adInteger, adParamInput, , lngPatientAlertID))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@AlertText", adVarChar, adParamInput, 256, strAlertText))
            .Parameters.Append(.CreateParameter("@LastModifiedBy", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        ' --R001
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn
        cmd.Execute()

        Update = 0

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Function
        End If

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cnn.Close()
        cmd = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()

        cmd.ActiveConnection = Nothing
        cnn.Close()
        cmd = Nothing
        cnn = Nothing

        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function

    Public Function Deleted(ByVal lngPatientAlertID As Long, ByVal blnDisabled As Boolean, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 08/31/2000
        'Author: Dave Nichol and Joshua Lockwood
        'Description: Updates selected records in the tblPatientAlert table.  s
        '               stored procedure uspUpdPatientAlert.
        'Parameters:  lngPatientAlertID - ID of the patient.
        '             blnDisableYN - boolean Y if row is disabled, N if row is active.
        '             strUserName - username of user inserting data
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection
        Dim cmd As New ADODB.Command




        'Create the parameter objects
        ' --R001
        With cmd
            .CommandText = "uspDelPatientAlert"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientAlertID", adInteger, adParamInput, , lngPatientAlertID))
            .Parameters.Append(.CreateParameter("@DisabledYN", adVarChar, adParamInput, 1, IIf(blnDisabled = False, "N", "Y")))
            .Parameters.Append(.CreateParameter("@LastModifiedBy", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        ' --R001
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn
        cmd.Execute()

        Deleted = 0

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Function
        End If

        'Disconnect the recordset
        cnn.Close()
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        cnn = Nothing

        'Signal successful completion

    End Function



End Class

