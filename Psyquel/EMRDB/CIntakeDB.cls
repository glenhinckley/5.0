
'--------------------------------------------------------------------
'Class Name: CIntakeDB
'Date: 05/25/2020
'Author: Duane C Orth
'Description:  Database object designed to implement methods (by ing
'              stored procedures) associated with
'              data affecting the tblIntake table.
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

Public Class CIntakeDB

    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CIntakeDB"
    Private Const TABLE_NAME As String = "tblIntake"

    Public Function FetchByID(ByVal lngIntakeID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 05/25/2020
        'Author: Duane C Orth
        'Description: Retrieves records from the tblIntake table.  s
        '               stored procedure uspSelIntakeByID.
        'Parameters: lngIntakeID - 'Where' clause of the SQL statement by
        '               which records will be fetched.  Matches tblIntake's
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



        ' Instantiate SQL statement --R001
        With cmd
            .CommandText = "uspSelIntakeByID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@IntakeID", adInteger, adParamInput, , lngIntakeID))
        End With

        ' Instantiate DB connection
        cnn.Open(_ConnectionString) ' open DB connection
        cmd.ActiveConnection = cnn

        ' Instantiate recordset
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)   ' open recordset

        cmd.ActiveConnection = Nothing
        rst.ActiveConnection = Nothing
        cnn.Close()   ' close DB connection
        cnn = Nothing
        cmd = Nothing

        FetchByID = rst ' Set return value




    End Function
    Public Function FetchByPatient(ByVal lngPatientID As Long, Optional ByVal lngTemplateID As Long = 0) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 05/25/2020
        'Author: Duane C Orth
        'Description: Retrieves records from the tblIntake table.  s
        '               stored procedure uspSelIntakeByPatient.
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



        ' Instantiate SQL statement --R001
        With cmd
            .CommandText = "uspSelIntakeByPatient"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@TemplateID", adInteger, adParamInput, , lngTemplateID))
        End With

        ' Instantiate DB connection
        cnn.Open(_ConnectionString) ' open DB connection
        cmd.ActiveConnection = cnn

        ' Instantiate recordset
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)   ' open recordset

        cmd.ActiveConnection = Nothing
        rst.ActiveConnection = Nothing
        cnn.Close()   ' close DB connection
        cnn = Nothing
        cmd = Nothing

        FetchByPatient = rst ' Set return value





    End Function
    Public Function Insert(ByVal lngPatientID As Long, ByVal lngTemplateID As Long, ByVal lngProviderID As Long, _
                           ByVal lngClinicID As Long, ByVal strNoteTitle As String, ByVal dtNoteDate As Date, _
                           ByVal strChar_1 As String, ByVal strChar_2 As String, ByVal strChar_3 As String, ByVal strChar_4 As String, _
                           ByVal strChar_5 As String, ByVal strChar_6 As String, ByVal strChar_7 As String, ByVal strChar_8 As String, _
                           ByVal strChar_9 As String, ByVal strChar_10 As String, ByVal strChar_11 As String, ByVal strChar_12 As String, _
                           ByVal strChar_13 As String, ByVal strChar_14 As String, ByVal strChar_15 As String, ByVal strChar_16 As String, _
                           ByVal strChar_17 As String, ByVal strChar_18 As String, ByVal strChar_19 As String, ByVal strChar_20 As String, _
                           ByVal dtDate_1 As Date, ByVal dtDate_2 As Date, ByVal dtDate_3 As Date, ByVal dtDate_4 As Date, _
                           ByVal dtDate_5 As Date, ByVal dtDate_6 As Date, ByVal strText_1 As String, ByVal strText_2 As String, _
                           ByVal strNoteText As String, ByVal blnLockedYN As Boolean, ByVal lngSignedBy As Long, _
                           ByVal dtDateSigned As Date, ByVal strUserName As String) As Long
        '--------------------------------------------------------------------
        'Date: 05/25/2020
        'Author: Duane C Orth
        'Description: Inserts records into the tblIntake table.  s
        '               stored procedure uspInsIntake.
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
            .CommandText = "uspInsIntake"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@IntakeID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@TemplateID", adInteger, adParamInput, , lngTemplateID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@ClinicID", adInteger, adParamInput, , IIf(lngClinicID <= 0, DBNull.Value, lngClinicID)))
            .Parameters.Append(.CreateParameter("@NoteTitle", adVarChar, adParamInput, 128, Left(strNoteTitle, 128)))
            .Parameters.Append(.CreateParameter("@NoteDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtNoteDate.ToOADate()) = 0, Now(), dtNoteDate)))
            .Parameters.Append(.CreateParameter("@Char_1", adVarChar, adParamInput, 128, Left(strChar_1, 128)))
            .Parameters.Append(.CreateParameter("@Char_2", adVarChar, adParamInput, 128, Left(strChar_2, 128)))
            .Parameters.Append(.CreateParameter("@Char_3", adVarChar, adParamInput, 128, Left(strChar_3, 128)))
            .Parameters.Append(.CreateParameter("@Char_4", adVarChar, adParamInput, 128, Left(strChar_4, 128)))
            .Parameters.Append(.CreateParameter("@Char_5", adVarChar, adParamInput, 128, Left(strChar_5, 128)))
            .Parameters.Append(.CreateParameter("@Char_6", adVarChar, adParamInput, 128, Left(strChar_6, 128)))
            .Parameters.Append(.CreateParameter("@Char_7", adVarChar, adParamInput, 128, Left(strChar_7, 128)))
            .Parameters.Append(.CreateParameter("@Char_8", adVarChar, adParamInput, 128, Left(strChar_8, 128)))
            .Parameters.Append(.CreateParameter("@Char_9", adVarChar, adParamInput, 128, Left(strChar_9, 128)))
            .Parameters.Append(.CreateParameter("@Char_10", adVarChar, adParamInput, 128, Left(strChar_10, 128)))
            .Parameters.Append(.CreateParameter("@Char_11", adVarChar, adParamInput, 128, Left(strChar_11, 128)))
            .Parameters.Append(.CreateParameter("@Char_12", adVarChar, adParamInput, 128, Left(strChar_12, 128)))
            .Parameters.Append(.CreateParameter("@Char_13", adVarChar, adParamInput, 128, Left(strChar_13, 128)))
            .Parameters.Append(.CreateParameter("@Char_14", adVarChar, adParamInput, 128, Left(strChar_14, 128)))
            .Parameters.Append(.CreateParameter("@Char_15", adVarChar, adParamInput, 128, Left(strChar_15, 128)))
            .Parameters.Append(.CreateParameter("@Char_16", adVarChar, adParamInput, 128, Left(strChar_16, 128)))
            .Parameters.Append(.CreateParameter("@Char_17", adVarChar, adParamInput, 128, Left(strChar_17, 128)))
            .Parameters.Append(.CreateParameter("@Char_18", adVarChar, adParamInput, 128, Left(strChar_18, 128)))
            .Parameters.Append(.CreateParameter("@Char_19", adVarChar, adParamInput, 128, Left(strChar_19, 128)))
            .Parameters.Append(.CreateParameter("@Char_20", adVarChar, adParamInput, 128, Left(strChar_20, 128)))
            .Parameters.Append(.CreateParameter("@Date_1", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDate_1.ToOADate()) = 0, DBNull.Value, dtDate_1)))
            .Parameters.Append(.CreateParameter("@Date_2", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDate_2.ToOADate()) = 0, DBNull.Value, dtDate_2)))
            .Parameters.Append(.CreateParameter("@Date_3", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDate_3.ToOADate()) = 0, DBNull.Value, dtDate_3)))
            .Parameters.Append(.CreateParameter("@Date_4", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDate_4.ToOADate()) = 0, DBNull.Value, dtDate_4)))
            .Parameters.Append(.CreateParameter("@Date_5", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDate_5.ToOADate()) = 0, DBNull.Value, dtDate_5)))
            .Parameters.Append(.CreateParameter("@Date_6", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDate_6.ToOADate()) = 0, DBNull.Value, dtDate_6)))
            .Parameters.Append(.CreateParameter("@Text_1", adLongVarChar, adParamInput, IIf(Len(strText_1) = 0, 1, Len(strText_1))))
            .Parameters("@Text_1").AppendChunk(strText_1)
            .Parameters.Append(.CreateParameter("@Text_2", adLongVarChar, adParamInput, IIf(Len(strText_2) = 0, 1, Len(strText_2))))
            .Parameters("@Text_2").AppendChunk(strText_2)
            .Parameters.Append(.CreateParameter("@NoteText", adLongVarChar, adParamInput, IIf(Len(strNoteText) = 0, 1, Len(strNoteText))))
            .Parameters("@NoteText").AppendChunk(strNoteText)
            .Parameters.Append(.CreateParameter("@LockedYN", adVarChar, adParamInput, 1, IIf(blnLockedYN = False, "N", "Y")))
            .Parameters.Append(.CreateParameter("@SignedBy", adInteger, adParamInput, , IIf(lngSignedBy <= 0, DBNull.Value, lngSignedBy)))
            .Parameters.Append(.CreateParameter("@DateSigned", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDateSigned.ToOADate()) = 0, DBNull.Value, dtDateSigned)))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 30, Left(strUserName, 30)))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With


        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn
        cmd.Execute()

        Insert = cmd.Parameters(0).Value

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Function
        End If

        cmd.ActiveConnection = Nothing
        cnn.Close()
        cnn = Nothing
        cmd = Nothing




    End Function
    Public Sub Update(ByVal lngIntakeID As Long, ByVal lngPatientID As Long, ByVal lngTemplateID As Long, ByVal lngProviderID As Long, _
                           ByVal lngClinicID As Long, ByVal strNoteTitle As String, ByVal dtNoteDate As Date, _
                           ByVal strChar_1 As String, ByVal strChar_2 As String, ByVal strChar_3 As String, ByVal strChar_4 As String, _
                           ByVal strChar_5 As String, ByVal strChar_6 As String, ByVal strChar_7 As String, ByVal strChar_8 As String, _
                           ByVal strChar_9 As String, ByVal strChar_10 As String, ByVal strChar_11 As String, ByVal strChar_12 As String, _
                           ByVal strChar_13 As String, ByVal strChar_14 As String, ByVal strChar_15 As String, ByVal strChar_16 As String, _
                           ByVal strChar_17 As String, ByVal strChar_18 As String, ByVal strChar_19 As String, ByVal strChar_20 As String, _
                           ByVal dtDate_1 As Date, ByVal dtDate_2 As Date, ByVal dtDate_3 As Date, ByVal dtDate_4 As Date, _
                           ByVal dtDate_5 As Date, ByVal dtDate_6 As Date, ByVal strText_1 As String, ByVal strText_2 As String, _
                           ByVal strNoteText As String, ByVal blnLockedYN As Boolean, ByVal lngSignedBy As Long, _
                           ByVal dtDateSigned As Date, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 05/25/2020
        'Author: Duane C Orth
        'Description: Updates selected records in the tblIntake table.  s
        '               stored procedure uspUpdIntake.
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
            .CommandText = "uspUpdIntake"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@IntakeID", adInteger, adParamInput, , lngIntakeID))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@TemplateID", adInteger, adParamInput, , lngTemplateID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@ClinicID", adInteger, adParamInput, , IIf(lngClinicID <= 0, DBNull.Value, lngClinicID)))
            .Parameters.Append(.CreateParameter("@NoteTitle", adVarChar, adParamInput, 128, Left(strNoteTitle, 128)))
            .Parameters.Append(.CreateParameter("@NoteDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtNoteDate.ToOADate()) = 0, Now(), dtNoteDate)))
            .Parameters.Append(.CreateParameter("@Char_1", adVarChar, adParamInput, 128, Left(strChar_1, 128)))
            .Parameters.Append(.CreateParameter("@Char_2", adVarChar, adParamInput, 128, Left(strChar_2, 128)))
            .Parameters.Append(.CreateParameter("@Char_3", adVarChar, adParamInput, 128, Left(strChar_3, 128)))
            .Parameters.Append(.CreateParameter("@Char_4", adVarChar, adParamInput, 128, Left(strChar_4, 128)))
            .Parameters.Append(.CreateParameter("@Char_5", adVarChar, adParamInput, 128, Left(strChar_5, 128)))
            .Parameters.Append(.CreateParameter("@Char_6", adVarChar, adParamInput, 128, Left(strChar_6, 128)))
            .Parameters.Append(.CreateParameter("@Char_7", adVarChar, adParamInput, 128, Left(strChar_7, 128)))
            .Parameters.Append(.CreateParameter("@Char_8", adVarChar, adParamInput, 128, Left(strChar_8, 128)))
            .Parameters.Append(.CreateParameter("@Char_9", adVarChar, adParamInput, 128, Left(strChar_9, 128)))
            .Parameters.Append(.CreateParameter("@Char_10", adVarChar, adParamInput, 128, Left(strChar_10, 128)))
            .Parameters.Append(.CreateParameter("@Char_11", adVarChar, adParamInput, 128, Left(strChar_11, 128)))
            .Parameters.Append(.CreateParameter("@Char_12", adVarChar, adParamInput, 128, Left(strChar_12, 128)))
            .Parameters.Append(.CreateParameter("@Char_13", adVarChar, adParamInput, 128, Left(strChar_13, 128)))
            .Parameters.Append(.CreateParameter("@Char_14", adVarChar, adParamInput, 128, Left(strChar_14, 128)))
            .Parameters.Append(.CreateParameter("@Char_15", adVarChar, adParamInput, 128, Left(strChar_15, 128)))
            .Parameters.Append(.CreateParameter("@Char_16", adVarChar, adParamInput, 128, Left(strChar_16, 128)))
            .Parameters.Append(.CreateParameter("@Char_17", adVarChar, adParamInput, 128, Left(strChar_17, 128)))
            .Parameters.Append(.CreateParameter("@Char_18", adVarChar, adParamInput, 128, Left(strChar_18, 128)))
            .Parameters.Append(.CreateParameter("@Char_19", adVarChar, adParamInput, 128, Left(strChar_19, 128)))
            .Parameters.Append(.CreateParameter("@Char_20", adVarChar, adParamInput, 128, Left(strChar_20, 128)))
            .Parameters.Append(.CreateParameter("@Date_1", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDate_1.ToOADate()) = 0, DBNull.Value, dtDate_1)))
            .Parameters.Append(.CreateParameter("@Date_2", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDate_2.ToOADate()) = 0, DBNull.Value, dtDate_2)))
            .Parameters.Append(.CreateParameter("@Date_3", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDate_3.ToOADate()) = 0, DBNull.Value, dtDate_3)))
            .Parameters.Append(.CreateParameter("@Date_4", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDate_4.ToOADate()) = 0, DBNull.Value, dtDate_4)))
            .Parameters.Append(.CreateParameter("@Date_5", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDate_5.ToOADate()) = 0, DBNull.Value, dtDate_5)))
            .Parameters.Append(.CreateParameter("@Date_6", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDate_6.ToOADate()) = 0, DBNull.Value, dtDate_6)))
            .Parameters.Append(.CreateParameter("@Text_1", adLongVarChar, adParamInput, IIf(Len(strText_1) = 0, 1, Len(strText_1))))
            .Parameters("@Text_1").AppendChunk(strText_1)
            .Parameters.Append(.CreateParameter("@Text_2", adLongVarChar, adParamInput, IIf(Len(strText_2) = 0, 1, Len(strText_2))))
            .Parameters("@Text_2").AppendChunk(strText_2)
            .Parameters.Append(.CreateParameter("@NoteText", adLongVarChar, adParamInput, IIf(Len(strNoteText) = 0, 1, Len(strNoteText))))
            .Parameters("@NoteText").AppendChunk(strNoteText)
            .Parameters.Append(.CreateParameter("@LockedYN", adVarChar, adParamInput, 1, IIf(blnLockedYN = False, "N", "Y")))
            .Parameters.Append(.CreateParameter("@SignedBy", adInteger, adParamInput, , IIf(lngSignedBy <= 0, DBNull.Value, lngSignedBy)))
            .Parameters.Append(.CreateParameter("@DateSigned", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDateSigned.ToOADate()) = 0, DBNull.Value, dtDateSigned)))
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, Left(strUserName, 30)))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        ' Instantiate DB connection
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn
        cmd.Execute()

        If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        cmd.ActiveConnection = Nothing
        cnn.Close()   ' close DB connection
        cmd = Nothing
        cnn = Nothing




    End Sub

    Public Sub LockNote(ByVal lngIntakeID As Long, ByVal lngSignedBy As Long, ByVal dtDateSigned As Date)
        '--------------------------------------------------------------------
        'Date: 05/25/2020
        'Author: Duane C Orth
        'Description: Updates a selected record in the tblIntake table, setting
        '               fldLockedYN to Y.  Cannot unlock record!!!  s stored
        '               procedure uspUpdLockIntake.
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
            .CommandText = "uspUpdLockIntake"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@IntakeID", adInteger, adParamInput, , lngIntakeID))
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
    Public Sub UnLockNote(ByVal lngIntakeID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 05/25/2020
        'Author: Duane C Orth
        'Description: Updates a selected record in the tblIntake table, setting
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
            .CommandText = "uspUpdUnLockIntake"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@IntakeID", adInteger, adParamInput, , lngIntakeID))
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

    Public Sub Delete(ByVal strUserName As String, ByVal lngIntakeID As Long)
        '--------------------------------------------------------------------
        'Date: 05/25/2020
        'Author: Duane C Orth
        'Description: Deletes selected record.  s
        '               stored procedure uspDelIntake.
        'Parameters:  lngIntakeID - ID of the patient.
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
            .CommandText = "uspDelIntake"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@IntakeID", adInteger, adParamInput, , lngIntakeID))
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




