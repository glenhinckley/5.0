
'--------------------------------------------------------------------
'Class Name: CPatientNoteDB
'Date: 09/01/2000
'Author: Joshua Lockwood
'Description:  Database object designed to implement methods (by ing
'              stored procedures) associated with
'              data affecting the tblPatientNote table.
'--------------------------------------------------------------------
'  Revisions:
'   --R001 : Joshua Lockwood, 15Sep2000 : Changes to avoid memory leaks
'   --R002 : David Nichol, 01Mar2001 : Added FetchNoteData
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

Public Class CPatientNoteDB

    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CPatientNoteDB"
    Private Const TABLE_NAME As String = "tblPatientNote"

    Public Function FetchByID(ByVal lngPatientNoteID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/01/2000
        'Author: Joshua Lockwood
        'Description: Retrieves records from the tblPatientNote table.  s
        '               stored procedure uspSelPatientNoteByID.
        'Parameters: lngPatientNoteID - 'Where' clause of the SQL statement by
        '               which records will be fetched.  Matches tblPatientNote's
        '               primary key.
        '             lngUserID - UserID of the user performing the fetch.  Not
        '               really used currently.
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
            .CommandText = "uspSelPatientNoteByID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientNoteID", adInteger, adParamInput, , lngPatientNoteID))
        End With

        'Acquire the database connection.
        ' Instantiate DB connection
        cnn.Open(_ConnectionString) ' open DB connection
        cmd.ActiveConnection = cnn

        'Set cursor and execute
        ' Instantiate recordset
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)   ' open recordset

        ' Cannot close recordset here --R001
        ' De-allocate space for objects
        cmd.ActiveConnection = Nothing
        rst.ActiveConnection = Nothing
        cnn.Close()   ' close DB connection
        cnn = Nothing
        cmd = Nothing

        FetchByID = rst ' Set return value

        'Signal successful completion


    End Function
    Public Function FetchByPatient(ByVal lngUserID As Long, ByVal lngPatientID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/01/2000
        'Author: Joshua Lockwood
        'Description: Retrieves records from the tblPatientNote table.  s
        '               stored procedure uspSelPatientNoteByPatient.
        'Parameters: lngPatientID - 'Where' clause of the SQL statement by
        '               which records will be fetched.
        '            lngUserID - UserID of the user performing the fetch.  Not
        '               really used currently.
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
            .CommandText = "uspSelPatientNoteByPatient"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
        End With

        'Acquire the database connection.
        ' Instantiate DB connection
        cnn.Open(_ConnectionString) ' open DB connection
        cmd.ActiveConnection = cnn

        'Set cursor and execute
        ' Instantiate recordset
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)   ' open recordset


        ' Cannot close recordset here
        ' De-allocate space for objects
        cmd.ActiveConnection = Nothing
        rst.ActiveConnection = Nothing
        cnn.Close()   ' close DB connection
        cnn = Nothing
        cmd = Nothing

        FetchByPatient = rst ' Set return value

        'Signal successful completion




    End Function

    Public Function FetchByAppt(ByVal lngUserID As Long, ByVal lngApptID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/01/2000
        'Author: Joshua Lockwood
        'Description: Retrieves records from the tblPatientNote table.  s
        '               stored procedure uspSelPatientNoteByAppt.
        'Parameters: lngApptID - 'Where' clause of the SQL statement by
        '               which records will be fetched.
        '             lngUserID - UserID of the user performing the fetch.  Not
        '               really used currently.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection ' Declare DB connection
        Dim rst As New ADODB.Recordset  ' Declare recordset
        Dim cmd As New ADODB.Command    ' Declare SQL statement



        ' Instantiate the objects
        ' Instantiate SQL statement

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelPatientNoteByAppt"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ApptID", adInteger, adParamInput, , lngApptID))
        End With

        'Acquire the database connection.
        ' Instantiate DB connection
        cnn.Open(_ConnectionString) ' open DB connection
        cmd.ActiveConnection = cnn

        'Set cursor and execute
        ' Instantiate recordset
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)   ' open recordset


        'Disconnect the recordset
        ' Cannot close recordset here
        'rst.Close   ' close recordset

        ' De-allocate space for objects
        cmd.ActiveConnection = Nothing
        rst.ActiveConnection = Nothing
        cnn.Close()   ' close DB connection
        cnn = Nothing
        cmd = Nothing

        FetchByAppt = rst ' Set return value

        'Signal successful completion



    End Function

    Public Function FetchByEncounter(ByVal lngUserID As Long, ByVal lngEncLogID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/05/2000
        'Author: David Nichol
        'Description: Retrieves records from the tblPatientNote table.  s
        '               stored procedure uspSelPatientNoteByEncLog.
        'Parameters: lngEncLogID - 'Where' clause of the SQL statement by
        '               which records will be fetched.
        '             lngUserID - UserID of the user performing the fetch.  Not
        '               really used currently.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection ' Declare DB connection
        Dim rst As New ADODB.Recordset  ' Declare recordset
        Dim cmd As New ADODB.Command    ' Declare SQL statement



        ' Instantiate the objects
        ' Instantiate SQL statement

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelPatientNoteByEncLog"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@EncounterLogID", adInteger, adParamInput, , lngEncLogID))
        End With

        'Acquire the database connection.
        ' Instantiate DB connection
        cnn.Open(_ConnectionString) ' open DB connection
        cmd.ActiveConnection = cnn

        'Set cursor and execute
        ' Instantiate recordset
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)   ' open recordset


        'Disconnect the recordset
        ' Cannot close recordset here
        'rst.Close   ' close recordset

        ' De-allocate space for objects
        'Set cmd.ActiveConnection = Nothing
        rst.ActiveConnection = Nothing
        cnn.Close()   ' close DB connection
        cnn = Nothing
        cmd = Nothing

        FetchByEncounter = rst ' Set return value

        'Signal successful completion




    End Function
    Public Function FetchNoteData(ByVal lngEncLogID As Long, ByVal lngUserID As Long) As Collection
        '--------------------------------------------------------------------
        'Date: 02/28/2001
        'Author: David Nichol
        'Description: s uspSelPatientNoteData, which retrieves a set of
        '               recordsets with all the note data.
        'Parameters:  lngEncLogID - 'Where' clause of the SQL statement by
        '               which records will be fetched.
        '             lngUserID - UserID of the user performing the fetch.  Not
        '               really used currently.
        'Returns: collection of ADODB.Recordsets
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection ' Declare DB connection
        Dim rst As New ADODB.Recordset  ' Declare recordset
        Dim cmd As New ADODB.Command    ' Declare SQL statement
        Dim col As New Collection   ' Declare collection of recordsets
        Dim intCtr As Integer       ' counter




        ' Instantiate the objects
        ' Instantiate SQL statement

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelPatientNoteData"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@EncounterLogID", adInteger, adParamInput, , lngEncLogID))
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
        End With

        'Acquire the database connection.
        ' Instantiate DB connection
        cnn.Open(_ConnectionString) ' open DB connection
        cmd.ActiveConnection = cnn

        'Set cursor and execute
        ' Instantiate recordset
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)   ' open recordset

        'test each recordset and add it to the collection
        intCtr = 0
        While Not (rst Is Nothing)
            intCtr = intCtr + 1
            col.Add(rst, CStr(intCtr))
            rst = rst.NextRecordset
        End While

        ' De-allocate space for objects
        cmd.ActiveConnection = Nothing

        FetchNoteData = col ' Set return value

        '    cnn.Close   ' close DB connection
        cnn = Nothing
        cmd = Nothing

        'Signal successful completion



    End Function



    Public Function FetchNextNote(ByVal lngUserID As Long, ByVal lngPatientNoteID As Long, _
                                  ByVal lngPatientID As Long, Optional ByVal lngEncLogID As Long = 0) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/05/2000
        'Author: David Nichol
        'Description: Retrieves records from the tblPatientNote table.  s
        '               stored procedure uspSelNextPatientNote.
        'Parameters: lngPatientNoteID - 'Where' clause of the SQL statement by
        '               which records will be fetched.  Returns row with next
        '               highest patientNoteID but same patientID.
        '            lngPatientID - select row for this patient
        '            lngUserID - UserID of the user performing the fetch.  Not
        '               really used currently.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection ' Declare DB connection
        Dim rst As New ADODB.Recordset  ' Declare recordset
        Dim cmd As New ADODB.Command    ' Declare SQL statement



        ' Instantiate the objects
        ' Instantiate SQL statement

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelNextPatientNote"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientNoteID", adInteger, adParamInput, , lngPatientNoteID))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            '     .Parameters.Append .CreateParameter("@EncounterLogID", adInteger, adParamInput, , lngEncLogID)
        End With

        'Acquire the database connection.
        ' Instantiate DB connection
        cnn.Open(_ConnectionString) ' open DB connection
        cmd.ActiveConnection = cnn

        'Set cursor and execute
        ' Instantiate recordset
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)   ' open recordset


        'Disconnect the recordset
        ' Cannot close recordset here
        'rst.Close   ' close recordset

        ' De-allocate space for objects
        cmd.ActiveConnection = Nothing
        rst.ActiveConnection = Nothing
        cnn.Close()   ' close DB connection
        cnn = Nothing
        cmd = Nothing

        FetchNextNote = rst ' Set return value

        'Signal successful completion




        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function

    Public Function FetchPrevNote(ByVal lngUserID As Long, ByVal lngPatientNoteID As Long, ByVal lngPatientID As Long, Optional ByVal lngEncLogID As Long = 0) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/05/2000
        'Author: David Nichol
        'Description: Retrieves records from the tblPatientNote table.  s
        '               stored procedure uspSelPrevPatientNote.
        'Parameters: lngPatientNoteID - 'Where' clause of the SQL statement by
        '               which records will be fetched.  Returns row with next
        '               lowest patientNoteID but same patientID.
        '            lngPatientID - select row for this patient
        '            lngUserID - UserID of the user performing the fetch.  Not
        '               really used currently.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection ' Declare DB connection
        Dim rst As New ADODB.Recordset  ' Declare recordset
        Dim cmd As New ADODB.Command    ' Declare SQL statement



        ' Instantiate the objects
        ' Instantiate SQL statement

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelPrevPatientNote"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientNoteID", adInteger, adParamInput, , lngPatientNoteID))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@EncounterLogID", adInteger, adParamInput, , lngEncLogID))
        End With

        'Acquire the database connection.
        ' Instantiate DB connection
        cnn.Open(_ConnectionString) ' open DB connection
        cmd.ActiveConnection = cnn

        'Set cursor and execute
        ' Instantiate recordset
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)   ' open recordset


        'Disconnect the recordset
        ' Cannot close recordset here
        'rst.Close   ' close recordset

        ' De-allocate space for objects
        cmd.ActiveConnection = Nothing
        rst.ActiveConnection = Nothing
        cnn.Close()   ' close DB connection
        cnn = Nothing
        cmd = Nothing

        FetchPrevNote = rst ' Set return value

        'Signal successful completion



    End Function

    Public Function Insert(ByVal lngPatientID As Long, ByVal lngTemplateID As Long, _
                           ByVal lngEncounterLogID As Long, ByVal lngProviderID As Long, ByVal dtNoteDate As Date, _
                           ByVal strNoteTitle As String, ByVal strNoteText As String, ByVal strMipsText As String, ByVal lngSupervisorID As Long, _
                           ByVal lngSignedBy As Long, ByVal dtDateSigned As Date, _
                           ByVal blnLockedYN As Boolean, ByVal strUserName As String) As Long
        '--------------------------------------------------------------------
        'Date: 09/01/2000
        'Author: Joshua Lockwood
        'Description: Inserts records into the tblPatientNote table.  s
        '               stored procedure uspInsPatientNote.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: Long
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection ' Declare DB connection
        Dim cmd As New ADODB.Command    ' Declare SQL statement



        ' Instantiate the objects
        ' Set     ' Instantiate recordset
        ' Instantiate SQL statement
        ' Instantiate DB connection

        'Create the parameter objects
        With cmd
            .CommandText = "uspInsPatientNote"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientNoteID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@TemplateID", adInteger, adParamInput, , IIf(lngTemplateID = 0, DBNull.Value, lngTemplateID)))
            .Parameters.Append(.CreateParameter("@EncLogID", adInteger, adParamInput, , lngEncounterLogID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@NoteDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtNoteDate.ToOADate()) = 0, Now(), dtNoteDate)))
            .Parameters.Append(.CreateParameter("@NoteTitle", adVarChar, adParamInput, 255, strNoteTitle))
            ' Next two lines enter String data into TEXT field using ADO
            .Parameters.Append(.CreateParameter("@NoteText", adLongVarChar, adParamInput, Len(strNoteText)))
            .Parameters("@NoteText").AppendChunk(strNoteText)
            .Parameters.Append(.CreateParameter("@MipsText", adVarChar, adParamInput, 128, Left(strMipsText, 128)))
            .Parameters.Append(.CreateParameter("@SupervisorID", adInteger, adParamInput, , IIf(lngSupervisorID <= 0, DBNull.Value, lngSupervisorID)))
            .Parameters.Append(.CreateParameter("@SignedBy", adInteger, adParamInput, , IIf(lngSignedBy <= 0, DBNull.Value, lngSignedBy)))
            .Parameters.Append(.CreateParameter("@DateSigned", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDateSigned.ToOADate()) = 0, DBNull.Value, dtDateSigned)))
            .Parameters.Append(.CreateParameter("@LockedYN", adVarChar, adParamInput, 1, IIf(blnLockedYN = False, "N", "Y")))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        cnn.Open(_ConnectionString) ' open DB connection
        cmd.ActiveConnection = cnn
        cmd.Execute()

        Insert = cmd.Parameters(0).Value

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Function
        End If

        'Acquire the database connection.
        ' Instantiate DB connection
        cnn.Open(_ConnectionString) ' open DB connection
        cmd.ActiveConnection = cnn

        ' De-allocate space for objects
        cmd.ActiveConnection = Nothing
        cnn.Close()   ' close DB connection
        cnn = Nothing
        cmd = Nothing

        'Signal successful completion


    End Function
    Public Sub Update(ByVal lngPatientNoteID As Long, ByVal lngTemplateID As Long, ByVal dtNoteDate As Date, _
                           ByVal strNoteTitle As String, ByVal strNoteText As String, ByVal strMipsText As String, ByVal lngSupervisorID As Long)
        '--------------------------------------------------------------------
        'Date: 09/01/2000
        'Author: Joshua Lockwood
        'Description: Updates selected records in the tblPatientNote table.  s
        '               stored procedure uspUpdPatientNote.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        '              lngPatientNoteID lngPatientID lngApptID lngTemplateID lngProviderID strNoteText blnLockedYN
        'Returns: Long
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection ' Declare DB connection
        Dim cmd As New ADODB.Command    ' Declare SQL statement



        ' Instantiate SQL statement

        'Create the parameter objects
        With cmd
            .CommandText = "uspUpdPatientNote"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientNoteID", adInteger, adParamInput, , lngPatientNoteID))
            .Parameters.Append(.CreateParameter("@TemplateID", adInteger, adParamInput, , IIf(lngTemplateID = 0, DBNull.Value, lngTemplateID)))
            .Parameters.Append(.CreateParameter("@NoteDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtNoteDate.ToOADate()) = 0, Now(), dtNoteDate)))
            .Parameters.Append(.CreateParameter("@NoteTitle", adVarChar, adParamInput, 255, strNoteTitle))
            ' Next two lines enter String data into TEXT field using ADO
            .Parameters.Append(.CreateParameter("@NoteText", adLongVarChar, adParamInput, Len(strNoteText)))
            .Parameters("@NoteText").AppendChunk(strNoteText)
            .Parameters.Append(.CreateParameter("@MipsText", adVarChar, adParamInput, 128, Left(strMipsText, 128)))
            .Parameters.Append(.CreateParameter("@SupervisorID", adInteger, adParamInput, , IIf(lngSupervisorID <= 0, DBNull.Value, lngSupervisorID)))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        ' Instantiate DB connection
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

        'Signal successful completion


    End Sub

    Public Sub LockNote(ByVal lngPatientNoteID As Long, ByVal lngSignedBy As Long, ByVal dtDateSigned As Date)
        '--------------------------------------------------------------------
        'Date: 10/05/2000
        'Author: Joshua Lockwood & David Nichol
        'Description: Updates a selected record in the tblPatientNote table, setting
        '               fldLockedYN to Y.  Cannot unlock record!!!  s stored
        '               procedure uspUpdLockPatientNote.
        'Parameters:  Passes the ID of the record to update.
        'Returns: Long
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection ' Declare DB connection
        Dim cmd As New ADODB.Command    ' Declare SQL statement



        ' Instantiate SQL statement

        'Create the parameter objects
        With cmd
            .CommandText = "uspUpdLockPatientNote"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientNoteID", adInteger, adParamInput, , lngPatientNoteID))
            .Parameters.Append(.CreateParameter("@SignedBy", adInteger, adParamInput, , IIf(lngSignedBy <= 0, DBNull.Value, lngSignedBy)))
            .Parameters.Append(.CreateParameter("@DateSigned", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDateSigned.ToOADate()) = 0, DBNull.Value, dtDateSigned)))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        ' Instantiate DB connection
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

        'Signal successful completion


    End Sub

    Public Sub UnLockNote(ByVal lngPatientNoteID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 10/05/2000
        'Author: Joshua Lockwood & David Nichol
        'Description: Updates a selected record in the tblPatientNote table, setting
        '               fldLockedYN to N.
        'Parameters:  Passes the ID of the record to update.
        'Returns: Long
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection ' Declare DB connection
        Dim cmd As New ADODB.Command    ' Declare SQL statement



        ' Instantiate SQL statement

        'Create the parameter objects
        With cmd
            .CommandText = "uspUpdUnLockPatientNote"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientNoteID", adInteger, adParamInput, , lngPatientNoteID))
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        ' Instantiate DB connection
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

        'Signal successful completion



    End Sub
    Public Sub Delete(ByVal strUserName As String, ByVal lngPatientNoteID As Long)
        '--------------------------------------------------------------------
        'Date: 09/01/2000
        'Author: Joshua Lockwood
        'Description: Deletes selected record.  s
        '               stored procedure uspDelPatientNote.
        'Parameters:  lngPatientNoteID - ID of the patient.
        '             strUserName - username of user deleting data : not used now
        'Returns: Long
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection ' Declare DB connection
        Dim cmd As New ADODB.Command    ' Declare SQL statement



        ' Instantiate the objects
        ' Set     ' Instantiate recordset
        ' Instantiate SQL statement
        ' Instantiate DB connection

        'Create the parameter objects
        With cmd
            .CommandText = "uspDelPatientNote"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientNoteID", adInteger, adParamInput, , lngPatientNoteID))
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        cnn.Open(_ConnectionString) ' open DB connection
        cmd.ActiveConnection = cnn
        cmd.Execute()

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        ' De-allocate space for objects
        cmd.ActiveConnection = Nothing
        cnn.Close()   ' close DB connection
        cmd = Nothing
        cnn = Nothing

        'Signal successful completion




    End Sub
End Class