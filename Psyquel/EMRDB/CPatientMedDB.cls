
'--------------------------------------------------------------------
'Class Name: CPatientMedDB
'Date: 09/07/2000
'Author: Joshua Lockwood
'Description:  Database object designed to implement methods (by ing
'              stored procedures) associated with
'              data affecting the tblPatientMedication table.
'--------------------------------------------------------------------
'  Revisions:
'   --R001 : Joshua Lockwood, 15Sep00 : Changes to avoid memory leaks
'
'--------------------------------------------------------------------
Option Explicit On
Option Strict On


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

Public Class CPatientMedDB
    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CPatientMedDB"
    Private Const TABLE_NAME As String = "tblPatientMedication"


    Public Function FetchByID(ByVal lngPatientMedicationID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/07/2000
        'Author: Joshua Lockwood
        'Description: Retrieves records from the tblPatientMedication table.  s
        '               stored procedure uspSelPatientMedicationByID.
        'Parameters: lngPatientMedicationID - 'Where' clause of the SQL statement by
        '               which records will be fetched.  Matches tblPatientMedication's
        '               primary key.
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
            .CommandText = "uspSelPatientMedicationByID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientMedicationID", adInteger, adParamInput, , lngPatientMedicationID))
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
        cnn.Close()   ' close DB connection
        cmd = Nothing
        cnn = Nothing

        'Signal successful completion


    End Function

    Public Function FetchByPatient(ByVal lngPatientID As Long, Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/07/2000
        'Author: Joshua Lockwood
        'Description: Retrieves records from the tblPatientMedication table.  s
        '               stored procedure uspSelPatientMedicationByPatient.
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
            .CommandText = "uspSelPatientMedicationByPatient"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
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
        cnn.Close()   ' close DB connection
        cmd = Nothing
        cnn = Nothing

        'Signal successful completion



    End Function
    Public Function Insert(ByVal lngPatientID As Long, ByVal strDrugName As String, _
                        ByVal strInstructions As String, ByVal strPrescriber As String, _
                        ByVal strUserName As String, Optional ByVal dtStartDate As Date = Nothing, _
                        Optional ByVal dtEndDate As Date = Nothing) As Long
        '--------------------------------------------------------------------
        'Date: 09/07/2000
        'Author: Joshua Lockwood
        'Description: Inserts records into the tblPatientMedication table.  s
        '               stored procedure uspInsPatientMedication.
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
            .CommandText = "uspInsPatientMedication"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientMedID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@DrugName", adVarChar, adParamInput, 100, strDrugName))
            .Parameters.Append(.CreateParameter("@Instructions", adVarChar, adParamInput, 100, strInstructions))
            .Parameters.Append(.CreateParameter("@Prescriber", adVarChar, adParamInput, 40, strPrescriber))
            .Parameters.Append(.CreateParameter("@StartDate", adDBTimeStamp, adParamInput, , IIf(dtStartDate.ToOADate() = 0, DBNull.Value, dtStartDate)))
            .Parameters.Append(.CreateParameter("@EndDate", adDBTimeStamp, adParamInput, , IIf(dtEndDate.ToOADate() = 0, DBNull.Value, dtEndDate)))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With
        'Assign the connection to the Command object and execute the stored procedure

        ' Instantiate DB connection --R001
        cnn.Open(_ConnectionString) ' open DB connection
        cmd.ActiveConnection = cnn
        cmd.Execute()

        Insert = CLng(cmd.Parameters(0).Value)

        'Check the ErrorNum parameter before deciding to commit the transaction

        Dim t As Integer
        t = CInt(cmd.Parameters("@SQLErrorNum").Value)


        If t <> 0 Then
            Exit Function
        End If

        ' De-allocate space for objects
        cmd.ActiveConnection = Nothing
        cnn.Close()   ' close DB connection
        cmd = Nothing
        cnn = Nothing

        'Signal successful completion


    End Function
    Public Sub Update(ByVal lngPatientMedicationID As Long, ByVal strDrugName As String, _
                        ByVal strInstructions As String, ByVal strPrescriber As String, _
                        ByVal strUserName As String, _
                        Optional ByVal dtStartDate As Date = Nothing, Optional ByVal dtEndDate As Date = Nothing)
        '--------------------------------------------------------------------
        'Date: 09/07/2000
        'Author: Joshua Lockwood
        'Description: Updates selected records in the tblPatientMedication table.  s
        '               stored procedure uspUpdPatientMedication.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        '               strUserName is passed in case we want auditing in the future...
        '                   for now, it does nothing.
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
            .CommandText = "uspUpdPatientMedication"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientMedicationID", adInteger, adParamInput, , lngPatientMedicationID))
            .Parameters.Append(.CreateParameter("@DrugName", adVarChar, adParamInput, 100, strDrugName))
            .Parameters.Append(.CreateParameter("@Instructions", adVarChar, adParamInput, 100, strInstructions))
            .Parameters.Append(.CreateParameter("@Prescriber", adVarChar, adParamInput, 40, strPrescriber))
            .Parameters.Append(.CreateParameter("@StartDate", adDBTimeStamp, adParamInput, , IIf(dtStartDate.ToOADate() = 0, DBNull.Value, dtStartDate)))
            .Parameters.Append(.CreateParameter("@EndDate", adDBTimeStamp, adParamInput, , IIf(dtEndDate.ToOADate() = 0, DBNull.Value, dtEndDate)))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        ' Instantiate DB connection --R001
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        cmd.Execute()


        Dim t As Integer = 0

        t = CInt(cmd.Parameters("@SQLErrorNum").Value)


        'Check the ErrorNum parameter before deciding to commit the transaction
        If t <> 0 Then
            Exit Sub
        End If

        ' De-allocate space for objects
        cmd.ActiveConnection = Nothing
        cnn.Close()   ' close DB connection
        cmd = Nothing
        cnn = Nothing

        'Signal successful completion



    End Sub

    Public Sub Deleted(ByVal lngPatientMedicationID As Long, ByVal blnDisabledYN As Boolean, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 09/07/2000
        'Author: Joshua Lockwood
        'Description: Deletes selected record.  s
        '               stored procedure uspDelPatientMedication.
        'Parameters:  lngPatientMedicationID - ID of the patient.
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
            .CommandText = "uspDelPatientMedication"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientMedicationID", adInteger, adParamInput, , lngPatientMedicationID))
            .Parameters.Append(.CreateParameter("@DisabledYN", adVarChar, adParamInput, 1, IIf(blnDisabledYN = False, "N", "Y")))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        ' Instantiate DB connection --R001
        cnn.Open(_ConnectionString) ' open DB connection
        cmd.ActiveConnection = cnn
        cmd.Execute()

        'Check the ErrorNum parameter before deciding to commit the transaction

        Dim t As Integer = 0
        t = CInt(cmd.Parameters("@SQLErrorNum").Value)
        If t <> 0 Then
            Exit Sub
        End If

        cnn.Close()   ' close DB connection
        ' De-allocate space for objects
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        ' Set rst.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion



    End Sub


End Class





