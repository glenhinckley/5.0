
'--------------------------------------------------------------------
'Class Name: CIntakeTeleDB
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Database object designed to implement methods (by ing
'              stored procedures) associated with
'              data affecting the tblIntakeTeletherapy table.
'--------------------------------------------------------------------
'  Revisions:
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



Public Class CIntakeTeleDB

    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CIntakeTeleDB"
    Private Const TABLE_NAME As String = "tblIntakeTeletherapy"

    Public Function FetchByID(ByVal lngIntakeTeleID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description: Retrieves records from the tblIntakeTeletherapy table.  s
        '               stored procedure uspSelIntakeTeleByID.
        'Parameters: lngIntakeTeleID - 'Where' clause of the SQL statement by
        '               which records will be fetched.  Matches tblIntakeTeletherapy's
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
            .CommandText = "uspSelIntakeTeleByID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@IntakeTeleID", adInteger, adParamInput, , lngIntakeTeleID))
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



        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function
    Public Function FetchByPatient(ByVal lngPatientID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description: Retrieves records from the tblIntakeTeletherapy table.  s
        '               stored procedure uspSelIntakeTeleByPatient.
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
            .CommandText = "uspSelIntakeTeleByPatient"
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

    Public Function Insert(ByVal lngPatientID As Long, ByVal lngProviderID As Long, _
                           ByVal lngClinicID As Long, ByVal dtNoteDate As Date, _
                           ByVal strInTele_1a As String, ByVal strInTele_1b As String, ByVal strInTele_1c As String, _
                           ByVal strInTele_2a As String, ByVal strInTele_2b As String, ByVal strInTele_2c As String, _
                           ByVal strInTele_3a As String, ByVal strInTele_3b As String, _
                           ByVal strInTele_4a As String, ByVal strInTele_4b As String, _
                           ByVal strInTele_5a As String, ByVal strInTele_5b As String, _
                           ByVal strInTele_6a As String, ByVal strInTele_6b As String, _
                           ByVal strNoteText As String, ByVal blnLockedYN As Boolean, ByVal lngSignedBy As Long, _
                           ByVal dtDateSigned As Date, ByVal strUserName As String) As Long
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description: Inserts records into the tblIntakeTeletherapy table.  s
        '               stored procedure uspInsIntakeTele.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: Long
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection ' Declare DB connection
        Dim cmd As New ADODB.Command    ' Declare SQL statement



        ' Instantiate the objects
        ' Instantiate SQL statement

        'Create the parameter objects
        With cmd
            .CommandText = "uspInsPatientIntakeTele"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@IntakeTeleID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@ClinicID", adInteger, adParamInput, , IIf(lngClinicID <= 0, DBNull.Value, lngClinicID)))
            .Parameters.Append(.CreateParameter("@NoteDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtNoteDate.ToOADate()) = 0, Now(), dtNoteDate)))
            .Parameters.Append(.CreateParameter("@InTele_1a", adVarChar, adParamInput, 255, Left(strInTele_1a, 255)))
            .Parameters.Append(.CreateParameter("@InTele_1b", adVarChar, adParamInput, 255, Left(strInTele_1b, 255)))
            .Parameters.Append(.CreateParameter("@InTele_1c", adVarChar, adParamInput, 255, Left(strInTele_1c, 255)))
            .Parameters.Append(.CreateParameter("@InTele_2a", adVarChar, adParamInput, 255, Left(strInTele_2a, 255)))
            .Parameters.Append(.CreateParameter("@InTele_2b", adVarChar, adParamInput, 255, Left(strInTele_2b, 255)))
            .Parameters.Append(.CreateParameter("@InTele_2c", adVarChar, adParamInput, 255, Left(strInTele_2c, 255)))
            .Parameters.Append(.CreateParameter("@InTele_3a", adVarChar, adParamInput, 255, Left(strInTele_3a, 255)))
            .Parameters.Append(.CreateParameter("@InTele_3b", adVarChar, adParamInput, 255, Left(strInTele_3b, 255)))
            .Parameters.Append(.CreateParameter("@InTele_4a", adVarChar, adParamInput, 255, Left(strInTele_4a, 255)))
            .Parameters.Append(.CreateParameter("@InTele_4b", adVarChar, adParamInput, 255, Left(strInTele_4b, 255)))
            .Parameters.Append(.CreateParameter("@InTele_5a", adVarChar, adParamInput, 255, Left(strInTele_5a, 255)))
            .Parameters.Append(.CreateParameter("@InTele_5b", adVarChar, adParamInput, 255, Left(strInTele_5b, 255)))
            .Parameters.Append(.CreateParameter("@InTele_6a", adVarChar, adParamInput, 255, Left(strInTele_6a, 255)))
            .Parameters.Append(.CreateParameter("@InTele_6b", adVarChar, adParamInput, 255, Left(strInTele_6b, 255)))
            .Parameters.Append(.CreateParameter("@NoteText", adLongVarChar, adParamInput, IIf(Len(strNoteText) = 0, 1, Len(strNoteText))))
            .Parameters("@NoteText").AppendChunk(strNoteText)
            .Parameters.Append(.CreateParameter("@LockedYN", adVarChar, adParamInput, 1, IIf(blnLockedYN = False, "N", "Y")))
            .Parameters.Append(.CreateParameter("@SignedBy", adInteger, adParamInput, , IIf(lngSignedBy <= 0, DBNull.Value, lngSignedBy)))
            .Parameters.Append(.CreateParameter("@DateSigned", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDateSigned.ToOADate()) = 0, DBNull.Value, dtDateSigned)))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 30, Left(strUserName, 30)))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure

        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn
        cmd.Execute()

        Insert = cmd.Parameters(0).Value

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Function
        End If

        'Acquire the database connection.
        '  Set  ' Instantiate DB connection
        '  cnn.Open (_ConnectionString) ' open DB connection
        '  Set cmd.ActiveConnection = cnn

        ' De-allocate space for objects
        cmd.ActiveConnection = Nothing
        cnn.Close()   ' close DB connection
        cnn = Nothing
        cmd = Nothing

        'Signal successful completion




    End Function
    Public Sub Update(ByVal lngIntakeTeleID As Long, ByVal lngPatientID As Long, ByVal lngProviderID As Long, _
                           ByVal lngClinicID As Long, ByVal dtNoteDate As Date, _
                           ByVal strInTele_1a As String, ByVal strInTele_1b As String, ByVal strInTele_1c As String, _
                           ByVal strInTele_2a As String, ByVal strInTele_2b As String, ByVal strInTele_2c As String, _
                           ByVal strInTele_3a As String, ByVal strInTele_3b As String, _
                           ByVal strInTele_4a As String, ByVal strInTele_4b As String, _
                           ByVal strInTele_5a As String, ByVal strInTele_5b As String, _
                           ByVal strInTele_6a As String, ByVal strInTele_6b As String, _
                           ByVal strNoteText As String, ByVal blnLockedYN As Boolean, ByVal lngSignedBy As Long, _
                           ByVal dtDateSigned As Date, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description: Updates selected records in the tblIntakeTeletherapy table.  s
        '               stored procedure uspUpdIntakeTele.
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



        ' Instantiate SQL statement

        'Create the parameter objects
        With cmd
            .CommandText = "uspUpdPatientIntakeTele"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@IntakeTeleID", adInteger, adParamInput, , lngIntakeTeleID))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@ClinicID", adInteger, adParamInput, , IIf(lngClinicID <= 0, DBNull.Value, lngClinicID)))
            .Parameters.Append(.CreateParameter("@NoteDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtNoteDate.ToOADate()) = 0, Now(), dtNoteDate)))
            .Parameters.Append(.CreateParameter("@InTele_1a", adVarChar, adParamInput, 255, Left(strInTele_1a, 255)))
            .Parameters.Append(.CreateParameter("@InTele_1b", adVarChar, adParamInput, 255, Left(strInTele_1b, 255)))
            .Parameters.Append(.CreateParameter("@InTele_1c", adVarChar, adParamInput, 255, Left(strInTele_1c, 255)))
            .Parameters.Append(.CreateParameter("@InTele_2a", adVarChar, adParamInput, 255, Left(strInTele_2a, 255)))
            .Parameters.Append(.CreateParameter("@InTele_2b", adVarChar, adParamInput, 255, Left(strInTele_2b, 255)))
            .Parameters.Append(.CreateParameter("@InTele_2c", adVarChar, adParamInput, 255, Left(strInTele_2c, 255)))
            .Parameters.Append(.CreateParameter("@InTele_3a", adVarChar, adParamInput, 255, Left(strInTele_3a, 255)))
            .Parameters.Append(.CreateParameter("@InTele_3b", adVarChar, adParamInput, 255, Left(strInTele_3b, 255)))
            .Parameters.Append(.CreateParameter("@InTele_4a", adVarChar, adParamInput, 255, Left(strInTele_4a, 255)))
            .Parameters.Append(.CreateParameter("@InTele_4b", adVarChar, adParamInput, 255, Left(strInTele_4b, 255)))
            .Parameters.Append(.CreateParameter("@InTele_5a", adVarChar, adParamInput, 255, Left(strInTele_5a, 255)))
            .Parameters.Append(.CreateParameter("@InTele_5b", adVarChar, adParamInput, 255, Left(strInTele_5b, 255)))
            .Parameters.Append(.CreateParameter("@InTele_6a", adVarChar, adParamInput, 255, Left(strInTele_6a, 255)))
            .Parameters.Append(.CreateParameter("@InTele_6b", adVarChar, adParamInput, 255, Left(strInTele_6b, 255)))
            .Parameters.Append(.CreateParameter("@NoteText", adLongVarChar, adParamInput, IIf(Len(strNoteText) = 0, 1, Len(strNoteText))))
            .Parameters("@NoteText").AppendChunk(strNoteText)
            .Parameters.Append(.CreateParameter("@LockedYN", adVarChar, adParamInput, 1, IIf(blnLockedYN = False, "N", "Y")))
            .Parameters.Append(.CreateParameter("@SignedBy", adInteger, adParamInput, , IIf(lngSignedBy <= 0, DBNull.Value, lngSignedBy)))
            .Parameters.Append(.CreateParameter("@DateSigned", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDateSigned.ToOADate()) = 0, DBNull.Value, dtDateSigned)))
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, Left(strUserName, 30)))
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

        ' De-allocate space for objects
        cmd.ActiveConnection = Nothing
        cnn.Close()   ' close DB connection
        cmd = Nothing
        cnn = Nothing

        'Signal successful completion



    End Sub

    Public Sub LockNote(ByVal lngIntakeTeleID As Long, ByVal lngSignedBy As Long, ByVal dtDateSigned As Date)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description: Updates a selected record in the tblIntakeTeletherapy table, setting
        '               fldLockedYN to Y.  Cannot unlock record!!!  s stored
        '               procedure uspUpdLockIntakeTele.
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
            .CommandText = "uspUpdLockIntakeTele"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@IntakeTeleID", adInteger, adParamInput, , lngIntakeTeleID))
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

        ' De-allocate space for objects
        cmd.ActiveConnection = Nothing
        cnn.Close()   ' close DB connection
        cmd = Nothing
        cnn = Nothing

        'Signal successful completion



    End Sub
    Public Sub UnLockNote(ByVal lngIntakeTeleID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description: Updates a selected record in the tblIntakeTeletherapy table, setting
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
            .CommandText = "uspUpdUnLockIntakeTele"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@IntakeTeleID", adInteger, adParamInput, , lngIntakeTeleID))
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

        ' De-allocate space for objects
        cmd.ActiveConnection = Nothing
        cnn.Close()   ' close DB connection
        cmd = Nothing
        cnn = Nothing

        'Signal successful completion


    End Sub

    Public Sub Delete(ByVal strUserName As String, ByVal lngIntakeTeleID As Long)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description: Deletes selected record.  s
        '               stored procedure uspDelIntakeTele.
        'Parameters:  lngIntakeTeleID - ID of the patient.
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
            .CommandText = "uspDelIntakeTele"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@IntakeTeleID", adInteger, adParamInput, , lngIntakeTeleID))
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





