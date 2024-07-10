
'--------------------------------------------------------------------
'Class Name: CNoteSoapDB
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Database object designed to implement methods (by ing
'              stored procedures) associated with
'              data affecting the tblNoteSoap table.
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

Public Class CNoteSoapDB

    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CNoteSoapDB"
    Private Const TABLE_NAME As String = "tblNoteSoap"

    Public Function FetchByID(ByVal lngSoapNoteID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description: Retrieves records from the tblNoteSoap table.  s
        '               stored procedure uspSelPatientSoapNoteByID.
        'Parameters: lngSoapNoteID - 'Where' clause of the SQL statement by
        '               which records will be fetched.  Matches tblNoteSoap's
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
            .CommandText = "uspSelPatientSoapNoteByID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@SoapNoteID", adInteger, adParamInput, , lngSoapNoteID))
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
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description: Retrieves records from the tblNoteSoap table.  s
        '               stored procedure uspSelPatientSoapNoteByPatient.
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
            .CommandText = "uspSelPatientSoapNoteByPatient"
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

    Public Function FetchByEncounter(ByVal lngUserID As Long, ByVal lngEncLogID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description: Retrieves records from the tblNoteSoap table.  s
        '               stored procedure uspSelPatientSoapNoteByEncLog.
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
            .CommandText = "uspSelPatientSoapNoteByEncLog"
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


    Public Function FetchNextNote(ByVal lngUserID As Long, ByVal lngSoapNoteID As Long, _
                                  ByVal lngPatientID As Long, Optional ByVal lngEncLogID As Long = 0) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/05/2000
        'Author: David Nichol
        'Description: Retrieves records from the tblPatientNote table.  s
        '               stored procedure uspSelNextPatientNote.
        'Parameters: lngSoapNoteID - 'Where' clause of the SQL statement by
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
            .CommandText = "uspSelNextPatientNoteSoap"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@SoapNoteID", adInteger, adParamInput, , lngSoapNoteID))
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

        FetchNextNote = rst ' Set return value

        'Signal successful completion



        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function

    Public Function FetchPrevNote(ByVal lngUserID As Long, ByVal lngSoapNoteID As Long, ByVal lngPatientID As Long, Optional ByVal lngEncLogID As Long = 0) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/05/2000
        'Author: David Nichol
        'Description: Retrieves records from the tblPatientNote table.  s
        '               stored procedure uspSelPrevPatientNote.
        'Parameters: lngSoapNoteID - 'Where' clause of the SQL statement by
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
            .CommandText = "uspSelPrevPatientNoteSoap"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@SoapNoteID", adInteger, adParamInput, , lngSoapNoteID))
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

    Public Function Insert(ByVal lngPatientID As Long, ByVal lngProviderID As Long, _
                           ByVal lngEncounterLogID As Long, ByVal lngClinicID As Long, _
                           ByVal lngSupervisorID As Long, ByVal dtNoteDate As Date, _
                           ByVal lngDiagID1 As Long, ByVal dtApptStartDate As Date, _
                           ByVal dtApptEndDate As Date, ByVal strSubjectText As String, _
                           ByVal strObjectText As String, ByVal strAssessText As String, _
                           ByVal strPlanText As String, ByVal strNoteText As String, ByVal strMipsText As String, _
                           ByVal blnLockedYN As Boolean, ByVal lngSignedBy As Long, _
                           ByVal dtDateSigned As Date, ByVal strUserName As String) As Long
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description: Inserts records into the tblNoteSoap table.  s
        '               stored procedure uspInsPatientSoapNote.
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
            .CommandText = "uspInsPatientNoteSoap"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@SoapNoteID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@EncLogID", adInteger, adParamInput, , IIf(lngEncounterLogID <= 0, DBNull.Value, lngEncounterLogID)))
            .Parameters.Append(.CreateParameter("@ClinicID", adInteger, adParamInput, , IIf(lngClinicID <= 0, DBNull.Value, lngClinicID)))
            .Parameters.Append(.CreateParameter("@SupervisorID", adInteger, adParamInput, , IIf(lngSupervisorID <= 0, DBNull.Value, lngSupervisorID)))
            .Parameters.Append(.CreateParameter("@NoteDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtNoteDate.ToOADate()) = 0, Now(), dtNoteDate)))
            .Parameters.Append(.CreateParameter("@DiagID1", adInteger, adParamInput, , IIf(lngDiagID1 <= 0, DBNull.Value, lngDiagID1)))
            .Parameters.Append(.CreateParameter("@ApptStartDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtApptStartDate.ToOADate()) = 0, DBNull.Value, dtApptStartDate)))
            .Parameters.Append(.CreateParameter("@ApptEndDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtApptEndDate.ToOADate()) = 0, DBNull.Value, dtApptEndDate)))
            .Parameters.Append(.CreateParameter("@SubjectText", adLongVarChar, adParamInput, IIf(Len(strSubjectText) = 0, 1, Len(strSubjectText))))
            .Parameters("@SubjectText").AppendChunk(strSubjectText)
            .Parameters.Append(.CreateParameter("@ObjectText", adLongVarChar, adParamInput, IIf(Len(strObjectText) = 0, 1, Len(strObjectText))))
            .Parameters("@ObjectText").AppendChunk(strObjectText)
            .Parameters.Append(.CreateParameter("@AssessText", adLongVarChar, adParamInput, IIf(Len(strAssessText) = 0, 1, Len(strAssessText))))
            .Parameters("@AssessText").AppendChunk(strAssessText)
            .Parameters.Append(.CreateParameter("@PlanText", adLongVarChar, adParamInput, IIf(Len(strPlanText) = 0, 1, Len(strPlanText))))
            .Parameters("@PlanText").AppendChunk(strPlanText)
            .Parameters.Append(.CreateParameter("@NoteText", adLongVarChar, adParamInput, IIf(Len(strNoteText) = 0, 1, Len(strNoteText))))
            .Parameters("@NoteText").AppendChunk(strNoteText)
            .Parameters.Append(.CreateParameter("@MipsText", adVarChar, adParamInput, 128, Left(strMipsText, 128)))
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
    Public Sub Update(ByVal lngSoapNoteID As Long, ByVal lngPatientID As Long, ByVal lngProviderID As Long, _
                           ByVal lngEncounterLogID As Long, ByVal lngClinicID As Long, _
                           ByVal lngSupervisorID As Long, ByVal dtNoteDate As Date, _
                           ByVal lngDiagID1 As Long, ByVal dtApptStartDate As Date, _
                           ByVal dtApptEndDate As Date, ByVal strSubjectText As String, _
                           ByVal strObjectText As String, ByVal strAssessText As String, _
                           ByVal strPlanText As String, ByVal strNoteText As String, ByVal strMipsText As String, _
                           ByVal blnLockedYN As Boolean, ByVal lngSignedBy As Long, _
                           ByVal dtDateSigned As Date, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description: Updates selected records in the tblNoteSoap table.  s
        '               stored procedure uspUpdPatientSoapNote.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        '              lngSoapNoteID lngPatientID lngApptID lngTemplateID lngProviderID strNoteText blnLockedYN
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
            .CommandText = "uspUpdPatientNoteSoap"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@SoapNoteID", adInteger, adParamInput, , lngSoapNoteID))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@EncLogID", adInteger, adParamInput, , IIf(lngEncounterLogID <= 0, DBNull.Value, lngEncounterLogID)))
            .Parameters.Append(.CreateParameter("@ClinicID", adInteger, adParamInput, , IIf(lngClinicID <= 0, DBNull.Value, lngClinicID)))
            .Parameters.Append(.CreateParameter("@SupervisorID", adInteger, adParamInput, , IIf(lngSupervisorID <= 0, DBNull.Value, lngSupervisorID)))
            .Parameters.Append(.CreateParameter("@NoteDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtNoteDate.ToOADate()) = 0, Now(), dtNoteDate)))
            .Parameters.Append(.CreateParameter("@DiagID1", adInteger, adParamInput, , IIf(lngDiagID1 <= 0, DBNull.Value, lngDiagID1)))
            .Parameters.Append(.CreateParameter("@ApptStartDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtApptStartDate.ToOADate()) = 0, DBNull.Value, dtApptStartDate)))
            .Parameters.Append(.CreateParameter("@ApptEndDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtApptEndDate.ToOADate()) = 0, DBNull.Value, dtApptEndDate)))
            .Parameters.Append(.CreateParameter("@SubjectText", adLongVarChar, adParamInput, IIf(Len(strSubjectText) = 0, 1, Len(strSubjectText))))
            .Parameters("@SubjectText").AppendChunk(strSubjectText)
            .Parameters.Append(.CreateParameter("@ObjectText", adLongVarChar, adParamInput, IIf(Len(strObjectText) = 0, 1, Len(strObjectText))))
            .Parameters("@ObjectText").AppendChunk(strObjectText)
            .Parameters.Append(.CreateParameter("@AssessText", adLongVarChar, adParamInput, IIf(Len(strAssessText) = 0, 1, Len(strAssessText))))
            .Parameters("@AssessText").AppendChunk(strAssessText)
            .Parameters.Append(.CreateParameter("@PlanText", adLongVarChar, adParamInput, IIf(Len(strPlanText) = 0, 1, Len(strPlanText))))
            .Parameters("@PlanText").AppendChunk(strPlanText)
            .Parameters.Append(.CreateParameter("@NoteText", adLongVarChar, adParamInput, IIf(Len(strNoteText) = 0, 1, Len(strNoteText))))
            .Parameters("@NoteText").AppendChunk(strNoteText)
            .Parameters.Append(.CreateParameter("@MipsText", adVarChar, adParamInput, 128, Left(strMipsText, 128)))
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

    Public Sub LockNote(ByVal lngSoapNoteID As Long, ByVal lngSignedBy As Long, ByVal dtDateSigned As Date)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description: Updates a selected record in the tblNoteSoap table, setting
        '               fldLockedYN to Y.  Cannot unlock record!!!  s stored
        '               procedure uspUpdLockPatientSoapNote.
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
            .CommandText = "uspUpdLockPatientSoapNote"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@SoapNoteID", adInteger, adParamInput, , lngSoapNoteID))
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
    Public Sub UnLockNote(ByVal lngSoapNoteID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description: Updates a selected record in the tblNoteSoap table, setting
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
            .CommandText = "uspUpdUnLockPatientSoapNote"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@SoapNoteID", adInteger, adParamInput, , lngSoapNoteID))
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

    Public Sub Delete(ByVal strUserName As String, ByVal lngSoapNoteID As Long)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description: Deletes selected record.  s
        '               stored procedure uspDelPatientSoapNote.
        'Parameters:  lngSoapNoteID - ID of the patient.
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
            .CommandText = "uspDelPatientSoapNote"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@SoapNoteID", adInteger, adParamInput, , lngSoapNoteID))
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

