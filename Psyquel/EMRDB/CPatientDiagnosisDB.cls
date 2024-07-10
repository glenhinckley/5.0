
'--------------------------------------------------------------------
'Class Name: CPatientDiagnosisDB
'Date: 09/07/2000
'Author: Joshua Lockwood
'Description:  Database object designed to implement methods (by ing
'              stored procedures) associated with
'              data affecting the tblPatientDiagnosis table.
'--------------------------------------------------------------------
'  Revisions:
'   --R001 : Joshua Lockwood, 15Sep00 : Changes to avoid memory leaks
'   --R002 : Joshua Lockwood, 11Oct00 : TEXT to VARCHAR(256) for ins and upd
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


Public Class CPatientDiagnosisDB

    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CPatientDiagnosisDB"
    Private Const TABLE_NAME As String = "tblPatientDiagnosis"


    Public Function FetchByID(ByVal lngPatientDiagnosisID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/07/2000
        'Author: Joshua Lockwood
        'Description: Retrieves records from the tblPatientDiagnosis table.  s
        '               stored procedure uspSelPatientDiagnosisByID.
        'Parameters: lngPatientDiagnosisID - 'Where' clause of the SQL statement by
        '               which records will be fetched.  Matches tblPatientDiagnosis's
        '               primary key.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '   ' --R001 : Joshua Lockwood, 15Sep00 : Changes to fix memory leaks
        '
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection ' Declare DB connection
        Dim rst As New ADODB.Recordset  ' Declare recordset
        Dim cmd As New ADODB.Command    ' Declare SQL statement



        'Create the parameter objects
        ' Instantiate SQL statement --R001
        With cmd
            .CommandText = "uspSelPatientDiagnosisByID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientDiagnosisID", adInteger, adParamInput, , lngPatientDiagnosisID))
        End With

        'Acquire the database connection.
        ' Instantiate DB connection --R001
        cnn.Open(_ConnectionString) ' open DB connection
        cmd.ActiveConnection = cnn

        'Execute the stored procedure
        ' Instantiate recordset --R001
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)   ' open recordset

        FetchByID = rst ' Set return value

        ' De-allocate space for objects
        cmd.ActiveConnection = Nothing
        rst.ActiveConnection = Nothing
        'Cannot close recordset in DB object  --R001
        cnn.Close()   ' close DB connection
        cmd = Nothing
        cnn = Nothing

        'Signal successful completion


    End Function

    Public Function FetchByPatient(ByVal lngPatientID As Long, Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/07/2000
        'Author: Joshua Lockwood
        'Description: Retrieves records from the tblPatientDiagnosis table.  s
        '               stored procedure uspSelPatientDiagnosisByPatient.
        'Parameters: lngPatientID - 'Where' clause of the SQL statement by
        '               which records will be fetched.
        '            blnIncludeDisabled - Flag used to control visibility of "Deleted" items.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection ' Declare DB connection
        Dim rst As New ADODB.Recordset  ' Declare recordset
        Dim cmd As New ADODB.Command    ' Declare SQL statement



        'Create the parameter objects
        ' Instantiate SQL statement --R001
        With cmd
            .CommandText = "uspSelPatientDiagnosisByPatient"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@IncludeDisabledYN", adVarChar, adParamInput, 1, IIf(blnIncludeDisabled = False, "N", "Y")))
        End With

        'Acquire the database connection.
        ' Instantiate DB connection --R001
        cnn.Open(_ConnectionString) ' open DB connection
        cmd.ActiveConnection = cnn

        'Execute the stored procedure
        ' Instantiate recordset --R001
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)   ' open recordset

        FetchByPatient = rst    ' Set return value

        ' De-allocate space for objects
        cmd.ActiveConnection = Nothing
        rst.ActiveConnection = Nothing
        ' Cannot close recordset in DB object  --R001
        cnn.Close()   ' close DB connection
        cmd = Nothing
        cnn = Nothing

        'Signal successful completion



    End Function
    Public Function Insert(ByVal lngPatientID As Long, ByVal strUserName As String, _
                           Optional ByVal lngAxisIa As Long = 0, Optional ByVal lngAxisIb As Long = 0, _
                           Optional ByVal lngAxisIc As Long = 0, Optional ByVal lngAxisId As Long = 0, _
                           Optional ByVal lngAxisIe As Long = 0, Optional ByVal lngAxisIf As Long = 0, _
                           Optional ByVal lngAxisIg As Long = 0, Optional ByVal lngAxisIh As Long = 0, _
                           Optional ByVal strDxNotes As String = "" _
                        ) As Long
        '--------------------------------------------------------------------
        'Date: 09/07/2000
        'Author: Joshua Lockwood
        'Description: Inserts records into the tblPatientDiagnosis table.  s
        '               stored procedure uspInsPatientDiagnosis.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: Long
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection ' Declare DB connection
        Dim cmd As New ADODB.Command    ' Declare SQL statement



        'Create the parameter objects
        ' Instantiate SQL statement --R001
        With cmd
            .CommandText = "uspInsPatientDiagnosis"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientDiagnosisID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@AxisIa", adInteger, adParamInput, , IIf(lngAxisIa = 0, DBNull.Value, lngAxisIa)))
            .Parameters.Append(.CreateParameter("@AxisIb", adInteger, adParamInput, , IIf(lngAxisIb = 0, DBNull.Value, lngAxisIb)))
            .Parameters.Append(.CreateParameter("@AxisIc", adInteger, adParamInput, , IIf(lngAxisIc = 0, DBNull.Value, lngAxisIc)))
            .Parameters.Append(.CreateParameter("@AxisId", adInteger, adParamInput, , IIf(lngAxisId = 0, DBNull.Value, lngAxisId)))
            .Parameters.Append(.CreateParameter("@AxisIe", adInteger, adParamInput, , IIf(lngAxisIe = 0, DBNull.Value, lngAxisIe)))
            .Parameters.Append(.CreateParameter("@AxisIf", adInteger, adParamInput, , IIf(lngAxisIf = 0, DBNull.Value, lngAxisIf)))
            .Parameters.Append(.CreateParameter("@AxisIg", adInteger, adParamInput, , IIf(lngAxisIg = 0, DBNull.Value, lngAxisIg)))
            .Parameters.Append(.CreateParameter("@AxisIh", adInteger, adParamInput, , IIf(lngAxisIh = 0, DBNull.Value, lngAxisIh)))
            '    .Parameters.Append .CreateParameter("@AxisIIa", adInteger, adParamInput, , IIf(lngAxisIIa = 0, Null, lngAxisIIa))
            '    .Parameters.Append .CreateParameter("@AxisIIaModifier", adVarChar, adParamInput, 20, strAxisIIaModifier)
            '    .Parameters.Append .CreateParameter("@AxisIIb", adInteger, adParamInput, , IIf(lngAxisIIb = 0, Null, lngAxisIIb))
            '    .Parameters.Append .CreateParameter("@AxisIIbModifier", adVarChar, adParamInput, 20, strAxisIIbModifier)
            '    .Parameters.Append .CreateParameter("@AxisIII", adVarChar, adParamInput, 100, strAxisIII)
            '    .Parameters.Append .CreateParameter("@AxisIV", adVarChar, adParamInput, 100, strAxisIV)
            '    .Parameters.Append .CreateParameter("@AxisV", adVarChar, adParamInput, 50, strAxisV)
            ' R002
            '.Parameters.Append .CreateParameter("@DxNotes", adLongVarChar, adParamInput, Len(strDxNotes))
            '.Parameters("@DxNotes").AppendChunk strDxNotes
            .Parameters.Append(.CreateParameter("@DxNotes", adVarChar, adParamInput, 256, strDxNotes))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        ' Instantiate DB connection --R001
        cnn.Open(_ConnectionString) ' open DB connection
        cmd.ActiveConnection = cnn
        cmd.Execute() '()

        Insert = cmd.Parameters(0).Value

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Function
        End If


        ' De-allocate space for objects
        cmd.ActiveConnection = Nothing
        cnn.Close()   ' close DB connection
        cmd = Nothing
        cnn = Nothing

        'Signal successful completion



    End Function
    Public Sub Update(ByVal lngPatientDiagnosisID As Long, _
                           Optional ByVal lngAxisIa As Long = 0, Optional ByVal lngAxisIb As Long = 0, _
                           Optional ByVal lngAxisIc As Long = 0, Optional ByVal lngAxisId As Long = 0, _
                           Optional ByVal lngAxisIe As Long = 0, Optional ByVal lngAxisIf As Long = 0, _
                           Optional ByVal lngAxisIg As Long = 0, Optional ByVal lngAxisIh As Long = 0, _
                           Optional ByVal strDxNotes As String = "")
        '--------------------------------------------------------------------
        'Date: 09/07/2000
        'Author: Joshua Lockwood
        'Description: Updates selected records in the tblPatientDiagnosis table.  s
        '               stored procedure uspUpdPatientDiagnosis.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: Long
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection ' Declare DB connection
        Dim cmd As New ADODB.Command    ' Declare SQL statement



        'Create the parameter objects
        ' Instantiate SQL statement --R001
        With cmd
            .CommandText = "uspUpdPatientDiagnosis"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientDiagnosisID", adInteger, adParamInput, , lngPatientDiagnosisID))
            .Parameters.Append(.CreateParameter("@AxisIa", adInteger, adParamInput, , IIf(lngAxisIa = 0, DBNull.Value, lngAxisIa)))
            .Parameters.Append(.CreateParameter("@AxisIb", adInteger, adParamInput, , IIf(lngAxisIb = 0, DBNull.Value, lngAxisIb)))
            .Parameters.Append(.CreateParameter("@AxisIc", adInteger, adParamInput, , IIf(lngAxisIc = 0, DBNull.Value, lngAxisIc)))
            .Parameters.Append(.CreateParameter("@AxisId", adInteger, adParamInput, , IIf(lngAxisId = 0, DBNull.Value, lngAxisId)))
            .Parameters.Append(.CreateParameter("@AxisIe", adInteger, adParamInput, , IIf(lngAxisIe = 0, DBNull.Value, lngAxisIe)))
            .Parameters.Append(.CreateParameter("@AxisIf", adInteger, adParamInput, , IIf(lngAxisIf = 0, DBNull.Value, lngAxisIf)))
            .Parameters.Append(.CreateParameter("@AxisIg", adInteger, adParamInput, , IIf(lngAxisIg = 0, DBNull.Value, lngAxisIg)))
            .Parameters.Append(.CreateParameter("@AxisIh", adInteger, adParamInput, , IIf(lngAxisIh = 0, DBNull.Value, lngAxisIh)))
            '     .Parameters.Append .CreateParameter("@AxisIIa", adInteger, adParamInput, , IIf(lngAxisIIa = 0, Null, lngAxisIIa))
            '     .Parameters.Append .CreateParameter("@AxisIIaModifier", adVarChar, adParamInput, 20, strAxisIIaModifier)
            '     .Parameters.Append .CreateParameter("@AxisIIb", adInteger, adParamInput, , IIf(lngAxisIIb = 0, Null, lngAxisIIb))
            '     .Parameters.Append .CreateParameter("@AxisIIbModifier", adVarChar, adParamInput, 20, strAxisIIbModifier)
            '     .Parameters.Append .CreateParameter("@AxisIII", adVarChar, adParamInput, 100, strAxisIII)
            '     .Parameters.Append .CreateParameter("@AxisIV", adVarChar, adParamInput, 100, strAxisIV)
            '     .Parameters.Append .CreateParameter("@AxisV", adVarChar, adParamInput, 50, strAxisV)
            ' R002
            '.Parameters.Append .CreateParameter("@DxNotes", adLongVarChar, adParamInput, Len(strDxNotes))
            '.Parameters("@DxNotes").AppendChunk strDxNotes
            .Parameters.Append(.CreateParameter("@DxNotes", adVarChar, adParamInput, 256, strDxNotes))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        ' Instantiate DB connection --R001
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn
        cmd.Execute()

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        cnn.Close()   ' close DB connection

        ' De-allocate space for objects
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        cnn = Nothing


    End Sub

    Public Sub Deleted(ByVal lngPatientDiagnosisID As Long, Optional ByVal blnIncludeDisabled As Boolean = False)
        '--------------------------------------------------------------------
        'Date: 09/07/2000
        'Author: Joshua Lockwood
        'Description: Deletes selected record.  s
        '               stored procedure uspDelPatientDiagnosis.
        'Parameters:  lngPatientDiagnosisID - ID of the patient.
        '             blnIncludeDisabled - delete flag y to delete n to undel
        'Returns: Long
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection ' Declare DB connection
        Dim cmd As New ADODB.Command    ' Declare SQL statement



        'Create the parameter objects
        ' Instantiate SQL statement --R001
        With cmd
            .CommandText = "uspDelPatientDiagnosis"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientDiagnosisID", adInteger, adParamInput, , lngPatientDiagnosisID))
            .Parameters.Append(.CreateParameter("@IncludeDisabledYN", adVarChar, adParamInput, 1, IIf(blnIncludeDisabled = False, "N", "Y")))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        ' Instantiate DB connection --R001
        cnn.Open(_ConnectionString) ' open DB connection
        cmd.ActiveConnection = cnn
        cmd.Execute()

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        cnn.Close()   ' close DB connection
        ' De-allocate space for objects
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        cnn = Nothing

        'Signal successful completion



    End Sub


End Class



