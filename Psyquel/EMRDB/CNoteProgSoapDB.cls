
'--------------------------------------------------------------------
'Class Name: CNoteProgSoapDB
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Database object designed to implement methods (by ing
'              stored procedures) associated with
'              data affecting the tblNoteProgSoap table.
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


Public Class CNoteProgSoapDB
    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CNoteProgSoapDB"
    Private Const TABLE_NAME As String = "tblNoteProgSoap"

    Public Function FetchByID(ByVal lngProgSoapNoteID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description: Retrieves records from the tblNoteProgSoap table.  s
        '               stored procedure uspSelPatientProgSoapNoteByID.
        'Parameters: lngProgSoapNoteID - 'Where' clause of the SQL statement by
        '               which records will be fetched.  Matches tblNoteProgSoap's
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
            .CommandText = "uspSelPatientProgSoapNoteByID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProgSoapNoteID", adInteger, adParamInput, , lngProgSoapNoteID))
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
        'Description: Retrieves records from the tblNoteProgSoap table.  s
        '               stored procedure uspSelPatientProgSoapNoteByPatient.
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
            .CommandText = "uspSelPatientProgSoapNoteByPatient"
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
        'Description: Retrieves records from the tblNoteProgSoap table.  s
        '               stored procedure uspSelPatientProgSoapNoteByEncLog.
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
            .CommandText = "uspSelPatientProgSoapNoteByEncLog"
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

    Public Function FetchNextNote(ByVal lngUserID As Long, ByVal lngProgSoapNoteID As Long, _
                                  ByVal lngPatientID As Long, Optional ByVal lngEncLogID As Long = 0) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/05/2000
        'Author: David Nichol
        'Description: Retrieves records from the tblPatientNote table.  s
        '               stored procedure uspSelNextPatientNote.
        'Parameters: lngProgSoapNoteID - 'Where' clause of the SQL statement by
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
            .CommandText = "uspSelNextPatientNoteProgSoap"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProgSoapNoteID", adInteger, adParamInput, , lngProgSoapNoteID))
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


    End Function

    Public Function FetchPrevNote(ByVal lngUserID As Long, ByVal lngProgSoapNoteID As Long, ByVal lngPatientID As Long, Optional ByVal lngEncLogID As Long = 0) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/05/2000
        'Author: David Nichol
        'Description: Retrieves records from the tblPatientNote table.  s
        '               stored procedure uspSelPrevPatientNote.
        'Parameters: lngProgSoapNoteID - 'Where' clause of the SQL statement by
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
            .CommandText = "uspSelPrevPatientNoteProgSoap"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProgSoapNoteID", adInteger, adParamInput, , lngProgSoapNoteID))
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
                           ByVal dtApptEndDate As Date, ByVal strPlanText As String, _
                           ByVal strPurpose As String, ByVal strClientText As String, _
                           ByVal strSiHi As String, ByVal strSiHiText As String, _
                           ByVal strTherapistText As String, ByVal strDiagChange As String, _
                           ByVal strDiagChangeText As String, ByVal strIntervention As String, _
                           ByVal strAppearance As String, ByVal strSpeech As String, _
                           ByVal strMotor As String, ByVal strHygiene As String, _
                           ByVal strThoughts As String, ByVal strAffect As String, _
                           ByVal strCognitive As String, ByVal strMedicationText As String, _
                           ByVal strPatientText As String, ByVal strRecommend As String, _
                           ByVal strCommentText As String, ByVal strPhysicalText As String, ByVal strLabsText As String, _
                           ByVal strNoteText As String, ByVal strMipsText As String, ByVal blnLockedYN As Boolean, _
                           ByVal lngSignedBy As Long, ByVal dtDateSigned As Date, _
                           ByVal strUserName As String) As Long
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description: Inserts records into the tblNoteProgSoap table.  s
        '               stored procedure uspInsPatientProgSoapNote.
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
            .CommandText = "uspInsPatientNoteProgSoap"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProgSoapNoteID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@EncLogID", adInteger, adParamInput, , IIf(lngEncounterLogID <= 0, DBNull.Value, lngEncounterLogID)))
            .Parameters.Append(.CreateParameter("@ClinicID", adInteger, adParamInput, , IIf(lngClinicID <= 0, DBNull.Value, lngClinicID)))
            .Parameters.Append(.CreateParameter("@SupervisorID", adInteger, adParamInput, , IIf(lngSupervisorID <= 0, DBNull.Value, lngSupervisorID)))
            .Parameters.Append(.CreateParameter("@NoteDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtNoteDate.ToOADate()) = 0, Now(), dtNoteDate)))
            .Parameters.Append(.CreateParameter("@DiagID1", adInteger, adParamInput, , IIf(lngDiagID1 <= 0, DBNull.Value, lngDiagID1)))
            .Parameters.Append(.CreateParameter("@ApptStartDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtApptStartDate.ToOADate()) = 0, DBNull.Value, dtApptStartDate)))
            .Parameters.Append(.CreateParameter("@ApptEndDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtApptEndDate.ToOADate()) = 0, DBNull.Value, dtApptEndDate)))
            .Parameters.Append(.CreateParameter("@PlanText", adLongVarChar, adParamInput, IIf(Len(strPlanText) = 0, 1, Len(strPlanText))))
            .Parameters("@PlanText").AppendChunk(strPlanText)
            .Parameters.Append(.CreateParameter("@Purpose", adVarChar, adParamInput, 10, Left(strPurpose, 10)))
            .Parameters.Append(.CreateParameter("@ClientText", adLongVarChar, adParamInput, IIf(Len(strClientText) = 0, 1, Len(strClientText))))
            .Parameters("@ClientText").AppendChunk(strClientText)
            .Parameters.Append(.CreateParameter("@SiHi", adVarChar, adParamInput, 10, Left(strSiHi, 10)))
            .Parameters.Append(.CreateParameter("@SiHiText", adVarChar, adParamInput, 255, Left(strSiHiText, 255)))
            .Parameters.Append(.CreateParameter("@TherapistText", adLongVarChar, adParamInput, IIf(Len(strTherapistText) = 0, 1, Len(strTherapistText))))
            .Parameters("@TherapistText").AppendChunk(strTherapistText)
            .Parameters.Append(.CreateParameter("@DiagChange", adVarChar, adParamInput, 10, Left(strDiagChange, 10)))
            .Parameters.Append(.CreateParameter("@DiagChangeText", adVarChar, adParamInput, 255, Left(strDiagChangeText, 255)))
            .Parameters.Append(.CreateParameter("@Intervention", adVarChar, adParamInput, 20, Left(strIntervention, 20)))
            .Parameters.Append(.CreateParameter("@Appearance", adVarChar, adParamInput, 10, Left(strAppearance, 10)))
            .Parameters.Append(.CreateParameter("@Speech", adVarChar, adParamInput, 10, Left(strSpeech, 10)))
            .Parameters.Append(.CreateParameter("@Motor", adVarChar, adParamInput, 10, Left(strMotor, 10)))
            .Parameters.Append(.CreateParameter("@Hygiene", adVarChar, adParamInput, 10, Left(strHygiene, 10)))
            .Parameters.Append(.CreateParameter("@Thoughts", adVarChar, adParamInput, 20, Left(strThoughts, 20)))
            .Parameters.Append(.CreateParameter("@Affect", adVarChar, adParamInput, 20, Left(strAffect, 20)))
            .Parameters.Append(.CreateParameter("@Cognitive", adVarChar, adParamInput, 10, Left(strCognitive, 10)))
            .Parameters.Append(.CreateParameter("@MedicationText", adLongVarChar, adParamInput, IIf(Len(strMedicationText) = 0, 1, Len(strMedicationText))))
            .Parameters("@MedicationText").AppendChunk(strMedicationText)
            .Parameters.Append(.CreateParameter("@PatientText", adLongVarChar, adParamInput, IIf(Len(strPatientText) = 0, 1, Len(strPatientText))))
            .Parameters("@PatientText").AppendChunk(strPatientText)
            .Parameters.Append(.CreateParameter("@Recommend", adVarChar, adParamInput, 10, Left(strRecommend, 10)))
            .Parameters.Append(.CreateParameter("@CommentText", adLongVarChar, adParamInput, IIf(Len(strCommentText) = 0, 1, Len(strCommentText))))
            .Parameters("@CommentText").AppendChunk(strCommentText)
            .Parameters.Append(.CreateParameter("@PhysicalText", adLongVarChar, adParamInput, IIf(Len(strPhysicalText) = 0, 1, Len(strPhysicalText))))
            .Parameters("@PhysicalText").AppendChunk(strPhysicalText)
            .Parameters.Append(.CreateParameter("@LabsText", adLongVarChar, adParamInput, IIf(Len(strLabsText) = 0, 1, Len(strLabsText))))
            .Parameters("@LabsText").AppendChunk(strLabsText)
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
        '   Set  ' Instantiate DB connection
        '   cnn.Open (_ConnectionString) ' open DB connection
        '   Set cmd.ActiveConnection = cnn

        ' De-allocate space for objects
        cmd.ActiveConnection = Nothing
        cnn.Close()   ' close DB connection
        cnn = Nothing
        cmd = Nothing

        'Signal successful completion



    End Function
    Public Sub Update(ByVal lngProgSoapNoteID As Long, ByVal lngPatientID As Long, ByVal lngProviderID As Long, _
                           ByVal lngEncounterLogID As Long, ByVal lngClinicID As Long, _
                           ByVal lngSupervisorID As Long, ByVal dtNoteDate As Date, _
                           ByVal lngDiagID1 As Long, ByVal dtApptStartDate As Date, _
                           ByVal dtApptEndDate As Date, ByVal strPlanText As String, _
                           ByVal strPurpose As String, ByVal strClientText As String, _
                           ByVal strSiHi As String, ByVal strSiHiText As String, _
                           ByVal strTherapistText As String, ByVal strDiagChange As String, _
                           ByVal strDiagChangeText As String, ByVal strIntervention As String, _
                           ByVal strAppearance As String, ByVal strSpeech As String, _
                           ByVal strMotor As String, ByVal strHygiene As String, _
                           ByVal strThoughts As String, ByVal strAffect As String, _
                           ByVal strCognitive As String, ByVal strMedicationText As String, _
                           ByVal strPatientText As String, ByVal strRecommend As String, _
                           ByVal strCommentText As String, ByVal strPhysicalText As String, ByVal strLabsText As String, _
                           ByVal strNoteText As String, ByVal strMipsText As String, ByVal blnLockedYN As Boolean, _
                           ByVal lngSignedBy As Long, ByVal dtDateSigned As Date, _
                           ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description: Updates selected records in the tblNoteProgSoap table.  s
        '               stored procedure uspUpdPatientProgSoapNote.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        '              lngProgSoapNoteID lngPatientID lngApptID lngTemplateID lngProviderID strNoteText blnLockedYN
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
            .CommandText = "uspUpdPatientNoteProgSoap"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProgSoapNoteID", adInteger, adParamInput, , lngProgSoapNoteID))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@EncLogID", adInteger, adParamInput, , IIf(lngEncounterLogID <= 0, DBNull.Value, lngEncounterLogID)))
            .Parameters.Append(.CreateParameter("@ClinicID", adInteger, adParamInput, , IIf(lngClinicID <= 0, DBNull.Value, lngClinicID)))
            .Parameters.Append(.CreateParameter("@SupervisorID", adInteger, adParamInput, , IIf(lngSupervisorID <= 0, DBNull.Value, lngSupervisorID)))
            .Parameters.Append(.CreateParameter("@NoteDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtNoteDate.ToOADate()) = 0, Now(), dtNoteDate)))
            .Parameters.Append(.CreateParameter("@DiagID1", adInteger, adParamInput, , IIf(lngDiagID1 <= 0, DBNull.Value, lngDiagID1)))
            .Parameters.Append(.CreateParameter("@ApptStartDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtApptStartDate.ToOADate()) = 0, DBNull.Value, dtApptStartDate)))
            .Parameters.Append(.CreateParameter("@ApptEndDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtApptEndDate.ToOADate()) = 0, DBNull.Value, dtApptEndDate)))
            .Parameters.Append(.CreateParameter("@PlanText", adLongVarChar, adParamInput, IIf(Len(strPlanText) = 0, 1, Len(strPlanText))))
            .Parameters("@PlanText").AppendChunk(strPlanText)
            .Parameters.Append(.CreateParameter("@Purpose", adVarChar, adParamInput, 10, Left(strPurpose, 10)))
            .Parameters.Append(.CreateParameter("@ClientText", adLongVarChar, adParamInput, IIf(Len(strClientText) = 0, 1, Len(strClientText))))
            .Parameters("@ClientText").AppendChunk(strClientText)
            .Parameters.Append(.CreateParameter("@SiHi", adVarChar, adParamInput, 10, Left(strSiHi, 10)))
            .Parameters.Append(.CreateParameter("@SiHiText", adVarChar, adParamInput, 255, Left(strSiHiText, 255)))
            .Parameters.Append(.CreateParameter("@TherapistText", adLongVarChar, adParamInput, IIf(Len(strTherapistText) = 0, 1, Len(strTherapistText))))
            .Parameters("@TherapistText").AppendChunk(strTherapistText)
            .Parameters.Append(.CreateParameter("@DiagChange", adVarChar, adParamInput, 10, Left(strDiagChange, 10)))
            .Parameters.Append(.CreateParameter("@DiagChangeText", adVarChar, adParamInput, 255, Left(strDiagChangeText, 255)))
            .Parameters.Append(.CreateParameter("@Intervention", adVarChar, adParamInput, 20, Left(strIntervention, 20)))
            .Parameters.Append(.CreateParameter("@Appearance", adVarChar, adParamInput, 10, Left(strAppearance, 10)))
            .Parameters.Append(.CreateParameter("@Speech", adVarChar, adParamInput, 10, Left(strSpeech, 10)))
            .Parameters.Append(.CreateParameter("@Motor", adVarChar, adParamInput, 10, Left(strMotor, 10)))
            .Parameters.Append(.CreateParameter("@Hygiene", adVarChar, adParamInput, 10, Left(strHygiene, 10)))
            .Parameters.Append(.CreateParameter("@Thoughts", adVarChar, adParamInput, 20, Left(strThoughts, 20)))
            .Parameters.Append(.CreateParameter("@Affect", adVarChar, adParamInput, 20, Left(strAffect, 20)))
            .Parameters.Append(.CreateParameter("@Cognitive", adVarChar, adParamInput, 10, Left(strCognitive, 10)))
            .Parameters.Append(.CreateParameter("@MedicationText", adLongVarChar, adParamInput, IIf(Len(strMedicationText) = 0, 1, Len(strMedicationText))))
            .Parameters("@MedicationText").AppendChunk(strMedicationText)
            .Parameters.Append(.CreateParameter("@PatientText", adLongVarChar, adParamInput, IIf(Len(strPatientText) = 0, 1, Len(strPatientText))))
            .Parameters("@PatientText").AppendChunk(strPatientText)
            .Parameters.Append(.CreateParameter("@Recommend", adVarChar, adParamInput, 10, Left(strRecommend, 10)))
            .Parameters.Append(.CreateParameter("@CommentText", adLongVarChar, adParamInput, IIf(Len(strCommentText) = 0, 1, Len(strCommentText))))
            .Parameters("@CommentText").AppendChunk(strCommentText)
            .Parameters.Append(.CreateParameter("@PhysicalText", adLongVarChar, adParamInput, IIf(Len(strPhysicalText) = 0, 1, Len(strPhysicalText))))
            .Parameters("@PhysicalText").AppendChunk(strPhysicalText)
            .Parameters.Append(.CreateParameter("@LabsText", adLongVarChar, adParamInput, IIf(Len(strLabsText) = 0, 1, Len(strLabsText))))
            .Parameters("@LabsText").AppendChunk(strLabsText)
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

    Public Sub LockNote(ByVal lngProgSoapNoteID As Long, ByVal lngSignedBy As Long, ByVal dtDateSigned As Date)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description: Updates a selected record in the tblNoteProgSoap table, setting
        '               fldLockedYN to Y.  Cannot unlock record!!!  s stored
        '               procedure uspUpdLockPatientProgSoapNote.
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
            .CommandText = "uspUpdLockPatientProgSoapNote"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProgSoapNoteID", adInteger, adParamInput, , lngProgSoapNoteID))
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


        'err.raise(Err.Number, Err.Source, Err.Description)
    End Sub
    Public Sub UnLockNote(ByVal lngProgSoapNoteID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description: Updates a selected record in the tblNoteProgSoap table, setting
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
            .CommandText = "uspUpdUnLockPatientProgSoapNote"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProgSoapNoteID", adInteger, adParamInput, , lngProgSoapNoteID))
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


    Public Sub Delete(ByVal strUserName As String, ByVal lngProgSoapNoteID As Long)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description: Deletes selected record.  s
        '               stored procedure uspDelPatientProgSoapNote.
        'Parameters:  lngProgSoapNoteID - ID of the patient.
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
            .CommandText = "uspDelPatientProgSoapNote"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProgSoapNoteID", adInteger, adParamInput, , lngProgSoapNoteID))
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
