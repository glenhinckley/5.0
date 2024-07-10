
'--------------------------------------------------------------------
'Class Name: CNoteInitialDB
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Database object designed to implement methods (by ing
'              stored procedures) associated with
'              data affecting the tblNoteInitial table.
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
' Lydia Orth
Imports Psyquel.BusinessRules.CoreLibraryIII.dbStuff

Public Class CNoteInitialDB

    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CNoteInitialDB"
    Private Const TABLE_NAME As String = "tblNoteInitial"
    Private _DB As New db
    Private _MD As New ModCommon

    Public Function FetchByID(ByVal lngInitialNoteID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description: Retrieves records from the tblNoteInitial table.  s
        '               stored procedure uspSelPatientInitialNoteByID.
        'Parameters: lngInitialNoteID - 'Where' clause of the SQL statement by
        '               which records will be fetched.  Matches tblNoteInitial's
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
            .CommandText = "uspSelPatientInitialNoteByID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@InitialNoteID", adInteger, adParamInput, , lngInitialNoteID))
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
        'Description: Retrieves records from the tblNoteInitial table.  s
        '               stored procedure uspSelPatientInitialNoteByPatient.
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
            .CommandText = "uspSelPatientInitialNoteByPatient"
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


    End Function

    Public Function FetchByEncounter(ByVal lngUserID As Long, ByVal lngEncLogID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description: Retrieves records from the tblNoteInitial table.  s
        '               stored procedure uspSelPatientInitialNoteByEncLog.
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
            .CommandText = "uspSelPatientInitialNoteByEncLog"
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


    Public Function FetchNextNote(ByVal lngUserID As Long, ByVal lngInitialNoteID As Long, _
                                  ByVal lngPatientID As Long, Optional ByVal lngEncLogID As Long = 0) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/05/2000
        'Author: David Nichol
        'Description: Retrieves records from the tblPatientNote table.  s
        '               stored procedure uspSelNextPatientNote.
        'Parameters: lngInitialNoteID - 'Where' clause of the SQL statement by
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
            .CommandText = "uspSelNextPatientNoteInitial"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@InitialNoteID", adInteger, adParamInput, , lngInitialNoteID))
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


    End Function

    Public Function FetchPrevNote(ByVal lngUserID As Long, ByVal lngInitialNoteID As Long, ByVal lngPatientID As Long, Optional ByVal lngEncLogID As Long = 0) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/05/2000
        'Author: David Nichol
        'Description: Retrieves records from the tblPatientNote table.  s
        '               stored procedure uspSelPrevPatientNote.
        'Parameters: lngInitialNoteID - 'Where' clause of the SQL statement by
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
            .CommandText = "uspSelPrevPatientNoteInitial"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@InitialNoteID", adInteger, adParamInput, , lngInitialNoteID))
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
                           ByVal dtApptEndDate As Date, ByVal strConcernText As String, _
                           ByVal strLegalText As String, ByVal strMedicationsText As String, _
                           ByVal strTreatmentTypeText As String, ByVal strTreatReasonText As String, _
                           ByVal strTreatPerceptionText As String, ByVal strPhysicalText As String, ByVal strLabsText As String, _
                           ByVal strEthnicText As String, ByVal strDiagnosisText As String, ByVal strPlanText As String, _
                           ByVal varData As Object, ByVal dtePriLastUseDate As Date, ByVal dteSecLastUseDate As Date, ByVal dteTerLastUseDate As Date, _
                           ByVal strNoteText As String, ByVal strMipsText As String, ByVal blnLockedYN As Boolean, _
                           ByVal lngSignedBy As Long, ByVal dtDateSigned As Date, _
                           ByVal strUserName As String) As Long
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description: Inserts records into the tblNoteInitial table.  s
        '               stored procedure uspInsPatientInitialNote.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: Long
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection ' Declare DB connection
        Dim cmd As New ADODB.Command    ' Declare SQL statement



        Dim lngCtr As Long
        Dim arrData() As Object
        ReDim arrData(60)

        If Not IsArray(varData) Or varData.IsNullorEmpty Then
            For lngCtr = 1 To UBound(arrData)
                arrData(lngCtr) = ""
            Next lngCtr
            arrData(33) = 0
            '      arrData(38) = 0
            arrData(41) = -1
            '      arrData(46) = 0
            arrData(49) = -1
            '      arrData(54) = 0
        Else
            For lngCtr = 1 To UBound(varData)
                arrData(lngCtr) = _DB.IfNull(varData(lngCtr), "")
            Next lngCtr
            arrData(33) = _DB.IfNull(varData(33), 0)
            '      arrData(38) = _DB.IfNull(varData(38), 0)
            arrData(41) = _DB.IfNull(varData(41), -1)
            '      arrData(46) = _DB.IfNull(varData(46), 0)
            arrData(49) = _DB.IfNull(varData(49), -1)
            '      arrData(54) = _DB.IfNull(varData(54), 0)
        End If

        ' Instantiate the objects
        ' Instantiate SQL statement

        'Create the parameter objects
        With cmd
            .CommandText = "uspInsPatientNoteInitial"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@InitialNoteID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@EncLogID", adInteger, adParamInput, , IIf(lngEncounterLogID <= 0, DBNull.Value, lngEncounterLogID)))
            .Parameters.Append(.CreateParameter("@ClinicID", adInteger, adParamInput, , IIf(lngClinicID <= 0, DBNull.Value, lngClinicID)))
            .Parameters.Append(.CreateParameter("@SupervisorID", adInteger, adParamInput, , IIf(lngSupervisorID <= 0, DBNull.Value, lngSupervisorID)))
            .Parameters.Append(.CreateParameter("@NoteDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtNoteDate.ToOADate()) = 0, Now(), dtNoteDate)))
            .Parameters.Append(.CreateParameter("@DiagID1", adInteger, adParamInput, , IIf(lngDiagID1 <= 0, DBNull.Value, lngDiagID1)))
            .Parameters.Append(.CreateParameter("@ApptStartDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtApptStartDate.ToOADate()) = 0, DBNull.Value, dtApptStartDate)))
            .Parameters.Append(.CreateParameter("@ApptEndDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtApptEndDate.ToOADate()) = 0, DBNull.Value, dtApptEndDate)))

            .Parameters.Append(.CreateParameter("@ConcernText", adLongVarChar, adParamInput, IIf(Len(strConcernText) = 0, 1, Len(strConcernText))))
            .Parameters("@ConcernText").AppendChunk(strConcernText)

            .Parameters.Append(.CreateParameter("@Living", adVarChar, adParamInput, 20, Left(arrData(1), 20)))          'Living,
            .Parameters.Append(.CreateParameter("@LivingText", adVarChar, adParamInput, 256, Left(arrData(2), 256)))          'LivingText,
            .Parameters.Append(.CreateParameter("@Family", adVarChar, adParamInput, 20, Left(arrData(3), 20)))          'Family,
            .Parameters.Append(.CreateParameter("@FamilyText", adVarChar, adParamInput, 256, Left(arrData(4), 256)))          'FamilyText,
            .Parameters.Append(.CreateParameter("@Peer", adVarChar, adParamInput, 20, Left(arrData(5), 20)))          'Peer,
            .Parameters.Append(.CreateParameter("@PeerText", adVarChar, adParamInput, 256, Left(arrData(6), 256)))          'PeerText,
            .Parameters.Append(.CreateParameter("@Leisure", adVarChar, adParamInput, 20, Left(arrData(7), 20)))          'Leisure,
            .Parameters.Append(.CreateParameter("@LeisureText", adVarChar, adParamInput, 256, Left(arrData(8), 256)))          'LeisureText,
            .Parameters.Append(.CreateParameter("@Development", adVarChar, adParamInput, 30, Left(arrData(9), 30)))          'Development,
            .Parameters.Append(.CreateParameter("@DevelopmentText", adVarChar, adParamInput, 256, Left(arrData(10), 256)))          'DevelopmentText,
            .Parameters.Append(.CreateParameter("@Abuse", adVarChar, adParamInput, 30, Left(arrData(11), 30)))          'Abuse,
            .Parameters.Append(.CreateParameter("@AbuseText", adVarChar, adParamInput, 256, Left(arrData(12), 256)))          'AbuseText,
            .Parameters.Append(.CreateParameter("@Spiritual", adVarChar, adParamInput, 20, Left(arrData(13), 20)))          'Spiritual,
            .Parameters.Append(.CreateParameter("@SpiritualText", adVarChar, adParamInput, 256, Left(arrData(14), 256)))          'SpiritualText,
            .Parameters.Append(.CreateParameter("@LegalText", adLongVarChar, adParamInput, IIf(Len(strLegalText) = 0, 1, Len(strLegalText))))
            .Parameters("@LegalText").AppendChunk(strLegalText)
            .Parameters.Append(.CreateParameter("@HistoryText", adVarChar, adParamInput, 512, Left(arrData(15), 256)))          'HistoryText,
            .Parameters.Append(.CreateParameter("@Education", adVarChar, adParamInput, 20, Left(arrData(16), 20)))          'Education,
            .Parameters.Append(.CreateParameter("@EducationText", adVarChar, adParamInput, 256, Left(arrData(17), 256)))          'EducationText,
            .Parameters.Append(.CreateParameter("@Occupation", adVarChar, adParamInput, 20, Left(arrData(18), 20)))          'Occupation,
            .Parameters.Append(.CreateParameter("@OccupationText", adVarChar, adParamInput, 256, Left(arrData(19), 256)))          'OccupationText,
            .Parameters.Append(.CreateParameter("@Military", adVarChar, adParamInput, 20, Left(arrData(20), 20)))          'Military,
            .Parameters.Append(.CreateParameter("@MilitaryText", adVarChar, adParamInput, 256, Left(arrData(21), 256)))          'MilitaryText,
            .Parameters.Append(.CreateParameter("@Problems", adVarChar, adParamInput, 40, Left(arrData(22), 40)))          'Problems,
            .Parameters.Append(.CreateParameter("@ProblemsText", adVarChar, adParamInput, 256, Left(arrData(23), 256)))          'ProblemsText,
            .Parameters.Append(.CreateParameter("@Allergies", adVarChar, adParamInput, 10, Left(arrData(24), 10)))          'Allergies,
            .Parameters.Append(.CreateParameter("@AllergiesText", adVarChar, adParamInput, 256, Left(arrData(25), 256)))          'AllergiesText,
            .Parameters.Append(.CreateParameter("@MedicationsText", adLongVarChar, adParamInput, IIf(Len(strMedicationsText) = 0, 1, Len(strMedicationsText))))
            .Parameters("@MedicationsText").AppendChunk(strMedicationsText)
            .Parameters.Append(.CreateParameter("@ImmediateFamily", adVarChar, adParamInput, 20, Left(arrData(26), 20)))          'ImmediateFamily,
            .Parameters.Append(.CreateParameter("@ImmediateFamilyText", adVarChar, adParamInput, 256, Left(arrData(27), 256)))          'ImmediateFamilyText,
            .Parameters.Append(.CreateParameter("@ExtendedFamily", adVarChar, adParamInput, 20, Left(arrData(28), 20)))          'ExtendedFamily,
            .Parameters.Append(.CreateParameter("@ExtendedFamilyText", adVarChar, adParamInput, 256, Left(arrData(29), 256)))          'ExtendedFamilyText,
            .Parameters.Append(.CreateParameter("@TreatmentType", adVarChar, adParamInput, 10, Left(arrData(30), 10)))          'TreatmentType,
            .Parameters.Append(.CreateParameter("@TreatmentTypeText", adLongVarChar, adParamInput, IIf(Len(strTreatmentTypeText) = 0, 1, Len(strTreatmentTypeText))))
            .Parameters("@TreatmentTypeText").AppendChunk(strTreatmentTypeText)
            .Parameters.Append(.CreateParameter("@TreatReasonText", adLongVarChar, adParamInput, IIf(Len(strTreatReasonText) = 0, 1, Len(strTreatReasonText))))
            .Parameters("@TreatReasonText").AppendChunk(strTreatReasonText)
            .Parameters.Append(.CreateParameter("@TreatPerceptionText", adLongVarChar, adParamInput, IIf(Len(strTreatPerceptionText) = 0, 1, Len(strTreatPerceptionText))))
            .Parameters("@TreatPerceptionText").AppendChunk(strTreatPerceptionText)
            .Parameters.Append(.CreateParameter("@Primary", adVarChar, adParamInput, 10, Left(arrData(31), 10)))          'Primary,
            .Parameters.Append(.CreateParameter("@PriSubstance", adVarChar, adParamInput, 64, Left(arrData(32), 64)))          'PriSubstance,
            .Parameters.Append(.CreateParameter("@PriStartAge", adInteger, adParamInput, , IIf(CLng(arrData(33)) <= 0, DBNull.Value, CLng(arrData(33)))))          'PriStartAge,
            .Parameters.Append(.CreateParameter("@PriFrequency", adVarChar, adParamInput, 64, Left(arrData(34), 64)))          'PriFrequency,
            .Parameters.Append(.CreateParameter("@PriRoute", adVarChar, adParamInput, 64, Left(arrData(35), 64)))          'PriRoute,
            .Parameters.Append(.CreateParameter("@PriDuration", adVarChar, adParamInput, 64, Left(arrData(36), 64)))          'PriDuration,
            .Parameters.Append(.CreateParameter("@PriAmtPerDay", adVarChar, adParamInput, 64, Left(arrData(37), 64)))          'PriAmtPerDay,
            .Parameters.Append(.CreateParameter("@PriLastUseDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtePriLastUseDate.ToOADate()) = 0, DBNull.Value, dtePriLastUseDate)))         'PriLastUseDate
            .Parameters.Append(.CreateParameter("@Secondary", adVarChar, adParamInput, 10, Left(arrData(39), 10)))          'Secondary,
            .Parameters.Append(.CreateParameter("@SecSubstance", adVarChar, adParamInput, 64, Left(arrData(40), 64)))          'SecSubstance,
            .Parameters.Append(.CreateParameter("@SecStartAge", adInteger, adParamInput, , IIf(CLng(arrData(41)) <= 0, DBNull.Value, CLng(arrData(41)))))          'SecStartAge
            .Parameters.Append(.CreateParameter("@SecFrequency", adVarChar, adParamInput, 64, Left(arrData(42), 64)))          'SecFrequency,
            .Parameters.Append(.CreateParameter("@SecRoute", adVarChar, adParamInput, 64, Left(arrData(43), 64)))          'SecRoute,
            .Parameters.Append(.CreateParameter("@SecDuration", adVarChar, adParamInput, 64, Left(arrData(44), 64)))          'SecDuration,
            .Parameters.Append(.CreateParameter("@SecAmtPerDay", adVarChar, adParamInput, 64, Left(arrData(45), 64)))          'SecAmtPerDay,
            .Parameters.Append(.CreateParameter("@SecLastUseDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dteSecLastUseDate.ToOADate()) = 0, DBNull.Value, dteSecLastUseDate)))          'SecLastUseDate
            .Parameters.Append(.CreateParameter("@Tertiary", adVarChar, adParamInput, 10, Left(arrData(47), 10)))          'Tertiary,
            .Parameters.Append(.CreateParameter("@TerSubstance", adVarChar, adParamInput, 64, Left(arrData(48), 64)))          'TerSubstance,
            .Parameters.Append(.CreateParameter("@TerStartAge", adInteger, adParamInput, , IIf(CLng(arrData(49)) <= 0, DBNull.Value, CLng(arrData(49)))))          'TerStartAge
            .Parameters.Append(.CreateParameter("@TerFrequency", adVarChar, adParamInput, 64, Left(arrData(50), 64)))          'TerFrequency,
            .Parameters.Append(.CreateParameter("@TerRoute", adVarChar, adParamInput, 64, Left(arrData(51), 64)))          'TerRoute,
            .Parameters.Append(.CreateParameter("@TerDuration", adVarChar, adParamInput, 64, Left(arrData(52), 64)))          'TerDuration,
            .Parameters.Append(.CreateParameter("@TerAmtPerDay", adVarChar, adParamInput, 64, Left(arrData(53), 64)))          'TerAmtPerDay,
            .Parameters.Append(.CreateParameter("@TerLastUseDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dteTerLastUseDate.ToOADate()) = 0, DBNull.Value, dteTerLastUseDate)))          'TerLastUseDate

            .Parameters.Append(.CreateParameter("@PhysicalText", adLongVarChar, adParamInput, IIf(Len(strPhysicalText) = 0, 1, Len(strPhysicalText))))
            .Parameters("@PhysicalText").AppendChunk(strPhysicalText)
            .Parameters.Append(.CreateParameter("@LabsText", adLongVarChar, adParamInput, IIf(Len(strLabsText) = 0, 1, Len(strLabsText))))
            .Parameters("@LabsText").AppendChunk(strLabsText)
            .Parameters.Append(.CreateParameter("@EthnicText", adLongVarChar, adParamInput, IIf(Len(strEthnicText) = 0, 1, Len(strEthnicText))))
            .Parameters("@EthnicText").AppendChunk(strEthnicText)
            .Parameters.Append(.CreateParameter("@DiagnosisText", adLongVarChar, adParamInput, IIf(Len(strDiagnosisText) = 0, 1, Len(strDiagnosisText))))
            .Parameters("@DiagnosisText").AppendChunk(strDiagnosisText)
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
        '    Set  ' Instantiate DB connection
        '    cnn.Open (_ConnectionString) ' open DB connection
        '    Set cmd.ActiveConnection = cnn

        ' De-allocate space for objects
        cmd.ActiveConnection = Nothing
        cnn.Close()   ' close DB connection
        cnn = Nothing
        cmd = Nothing

    End Function
    Public Sub Update(ByVal lngInitialNoteID As Long, ByVal lngPatientID As Long, ByVal lngProviderID As Long, _
                           ByVal lngEncounterLogID As Long, ByVal lngClinicID As Long, _
                           ByVal lngSupervisorID As Long, ByVal dtNoteDate As Date, _
                           ByVal lngDiagID1 As Long, ByVal dtApptStartDate As Date, _
                           ByVal dtApptEndDate As Date, ByVal strConcernText As String, _
                           ByVal strLegalText As String, ByVal strMedicationsText As String, _
                           ByVal strTreatmentTypeText As String, ByVal strTreatReasonText As String, _
                           ByVal strTreatPerceptionText As String, ByVal strPhysicalText As String, ByVal strLabsText As String, _
                           ByVal strEthnicText As String, ByVal strDiagnosisText As String, ByVal strPlanText As String, _
                           ByVal varData As Object, ByVal dtePriLastUseDate As Date, ByVal dteSecLastUseDate As Date, ByVal dteTerLastUseDate As Date, _
                           ByVal strNoteText As String, ByVal strMipsText As String, ByVal blnLockedYN As Boolean, _
                           ByVal lngSignedBy As Long, ByVal dtDateSigned As Date, _
                           ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description: Updates selected records in the tblNoteInitial table.  s
        '               stored procedure uspUpdPatientInitialNote.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        '              lngInitialNoteID lngPatientID lngApptID lngTemplateID lngProviderID strNoteText blnLockedYN
        'Returns: Long
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection ' Declare DB connection
        Dim cmd As New ADODB.Command    ' Declare SQL statement



        Dim lngCtr As Long
        Dim arrData() As Object
        ReDim arrData(60)

        If Not IsArray(varData) Or varData.IsNullorEmpty Then
            For lngCtr = 1 To UBound(arrData)
                arrData(lngCtr) = ""
            Next lngCtr
            arrData(33) = 0
            arrData(38) = 0
            arrData(41) = -1
            arrData(46) = 0
            arrData(49) = -1
            arrData(54) = 0
        Else
            For lngCtr = 1 To UBound(varData)
                arrData(lngCtr) = _DB.IfNull(varData(lngCtr), "")
            Next lngCtr
            arrData(33) = _DB.IfNull(varData(33), 0)
            arrData(38) = _DB.IfNull(varData(38), 0)
            arrData(41) = _DB.IfNull(varData(41), -1)
            arrData(46) = _DB.IfNull(varData(46), 0)
            arrData(49) = _DB.IfNull(varData(49), -1)
            arrData(54) = _DB.IfNull(varData(54), 0)
        End If

        ' Instantiate SQL statement

        'Create the parameter objects
        With cmd
            .CommandText = "uspUpdPatientNoteInitial"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@InitialNoteID", adInteger, adParamInput, , lngInitialNoteID))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@EncLogID", adInteger, adParamInput, , IIf(lngEncounterLogID <= 0, DBNull.Value, lngEncounterLogID)))
            .Parameters.Append(.CreateParameter("@ClinicID", adInteger, adParamInput, , IIf(lngClinicID <= 0, DBNull.Value, lngClinicID)))
            .Parameters.Append(.CreateParameter("@SupervisorID", adInteger, adParamInput, , IIf(lngSupervisorID <= 0, DBNull.Value, lngSupervisorID)))
            .Parameters.Append(.CreateParameter("@NoteDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtNoteDate.ToOADate()) = 0, Now(), dtNoteDate)))
            .Parameters.Append(.CreateParameter("@DiagID1", adInteger, adParamInput, , IIf(lngDiagID1 <= 0, DBNull.Value, lngDiagID1)))
            .Parameters.Append(.CreateParameter("@ApptStartDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtApptStartDate.ToOADate()) = 0, DBNull.Value, dtApptStartDate)))
            .Parameters.Append(.CreateParameter("@ApptEndDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtApptEndDate.ToOADate()) = 0, DBNull.Value, dtApptEndDate)))

            .Parameters.Append(.CreateParameter("@ConcernText", adLongVarChar, adParamInput, IIf(Len(strConcernText) = 0, 1, Len(strConcernText))))
            .Parameters("@ConcernText").AppendChunk(strConcernText)

            .Parameters.Append(.CreateParameter("@Living", adVarChar, adParamInput, 20, Left(arrData(1), 20)))          'Living,
            .Parameters.Append(.CreateParameter("@LivingText", adVarChar, adParamInput, 256, Left(arrData(2), 256)))          'LivingText,
            .Parameters.Append(.CreateParameter("@Family", adVarChar, adParamInput, 20, Left(arrData(3), 20)))          'Family,
            .Parameters.Append(.CreateParameter("@FamilyText", adVarChar, adParamInput, 256, Left(arrData(4), 256)))          'FamilyText,
            .Parameters.Append(.CreateParameter("@Peer", adVarChar, adParamInput, 20, Left(arrData(5), 20)))          'Peer,
            .Parameters.Append(.CreateParameter("@PeerText", adVarChar, adParamInput, 256, Left(arrData(6), 256)))          'PeerText,
            .Parameters.Append(.CreateParameter("@Leisure", adVarChar, adParamInput, 20, Left(arrData(7), 20)))          'Leisure,
            .Parameters.Append(.CreateParameter("@LeisureText", adVarChar, adParamInput, 256, Left(arrData(8), 256)))          'LeisureText,
            .Parameters.Append(.CreateParameter("@Development", adVarChar, adParamInput, 30, Left(arrData(9), 30)))          'Development,
            .Parameters.Append(.CreateParameter("@DevelopmentText", adVarChar, adParamInput, 256, Left(arrData(10), 256)))          'DevelopmentText,
            .Parameters.Append(.CreateParameter("@Abuse", adVarChar, adParamInput, 30, Left(arrData(11), 30)))          'Abuse,
            .Parameters.Append(.CreateParameter("@AbuseText", adVarChar, adParamInput, 256, Left(arrData(12), 256)))          'AbuseText,
            .Parameters.Append(.CreateParameter("@Spiritual", adVarChar, adParamInput, 20, Left(arrData(13), 20)))          'Spiritual,
            .Parameters.Append(.CreateParameter("@SpiritualText", adVarChar, adParamInput, 256, Left(arrData(14), 256)))          'SpiritualText,
            .Parameters.Append(.CreateParameter("@LegalText", adLongVarChar, adParamInput, IIf(Len(strLegalText) = 0, 1, Len(strLegalText))))
            .Parameters("@LegalText").AppendChunk(strLegalText)
            .Parameters.Append(.CreateParameter("@HistoryText", adVarChar, adParamInput, 512, Left(arrData(15), 256)))          'HistoryText,
            .Parameters.Append(.CreateParameter("@Education", adVarChar, adParamInput, 20, Left(arrData(16), 20)))          'Education,
            .Parameters.Append(.CreateParameter("@EducationText", adVarChar, adParamInput, 256, Left(arrData(17), 256)))          'EducationText,
            .Parameters.Append(.CreateParameter("@Occupation", adVarChar, adParamInput, 20, Left(arrData(18), 20)))          'Occupation,
            .Parameters.Append(.CreateParameter("@OccupationText", adVarChar, adParamInput, 256, Left(arrData(19), 256)))          'OccupationText,
            .Parameters.Append(.CreateParameter("@Military", adVarChar, adParamInput, 20, Left(arrData(20), 20)))          'Military,
            .Parameters.Append(.CreateParameter("@MilitaryText", adVarChar, adParamInput, 256, Left(arrData(21), 256)))          'MilitaryText,
            .Parameters.Append(.CreateParameter("@Problems", adVarChar, adParamInput, 40, Left(arrData(22), 40)))          'Problems,
            .Parameters.Append(.CreateParameter("@ProblemsText", adVarChar, adParamInput, 256, Left(arrData(23), 256)))          'ProblemsText,
            .Parameters.Append(.CreateParameter("@Allergies", adVarChar, adParamInput, 10, Left(arrData(24), 10)))          'Allergies,
            .Parameters.Append(.CreateParameter("@AllergiesText", adVarChar, adParamInput, 256, Left(arrData(25), 256)))          'AllergiesText,
            .Parameters.Append(.CreateParameter("@MedicationsText", adLongVarChar, adParamInput, IIf(Len(strMedicationsText) = 0, 1, Len(strMedicationsText))))
            .Parameters("@MedicationsText").AppendChunk(strMedicationsText)
            .Parameters.Append(.CreateParameter("@ImmediateFamily", adVarChar, adParamInput, 20, Left(arrData(26), 20)))          'ImmediateFamily,
            .Parameters.Append(.CreateParameter("@ImmediateFamilyText", adVarChar, adParamInput, 256, Left(arrData(27), 256)))          'ImmediateFamilyText,
            .Parameters.Append(.CreateParameter("@ExtendedFamily", adVarChar, adParamInput, 20, Left(arrData(28), 20)))          'ExtendedFamily,
            .Parameters.Append(.CreateParameter("@ExtendedFamilyText", adVarChar, adParamInput, 256, Left(arrData(29), 256)))          'ExtendedFamilyText,
            .Parameters.Append(.CreateParameter("@TreatmentType", adVarChar, adParamInput, 10, Left(arrData(30), 10)))          'TreatmentType,
            .Parameters.Append(.CreateParameter("@TreatmentTypeText", adLongVarChar, adParamInput, IIf(Len(strTreatmentTypeText) = 0, 1, Len(strTreatmentTypeText))))
            .Parameters("@TreatmentTypeText").AppendChunk(strTreatmentTypeText)
            .Parameters.Append(.CreateParameter("@TreatReasonText", adLongVarChar, adParamInput, IIf(Len(strTreatReasonText) = 0, 1, Len(strTreatReasonText))))
            .Parameters("@TreatReasonText").AppendChunk(strTreatReasonText)
            .Parameters.Append(.CreateParameter("@TreatPerceptionText", adLongVarChar, adParamInput, IIf(Len(strTreatPerceptionText) = 0, 1, Len(strTreatPerceptionText))))
            .Parameters("@TreatPerceptionText").AppendChunk(strTreatPerceptionText)
            .Parameters.Append(.CreateParameter("@Primary", adVarChar, adParamInput, 10, Left(arrData(31), 10)))          'Primary,
            .Parameters.Append(.CreateParameter("@PriSubstance", adVarChar, adParamInput, 64, Left(arrData(32), 64)))          'PriSubstance,
            .Parameters.Append(.CreateParameter("@PriStartAge", adInteger, adParamInput, , IIf(CLng(arrData(33)) <= 0, DBNull.Value, CLng(arrData(33)))))         'PriStartAge,
            .Parameters.Append(.CreateParameter("@PriFrequency", adVarChar, adParamInput, 64, Left(arrData(34), 64)))          'PriFrequency,
            .Parameters.Append(.CreateParameter("@PriRoute", adVarChar, adParamInput, 64, Left(arrData(35), 64)))          'PriRoute,
            .Parameters.Append(.CreateParameter("@PriDuration", adVarChar, adParamInput, 64, Left(arrData(36), 64)))          'PriDuration,
            .Parameters.Append(.CreateParameter("@PriAmtPerDay", adVarChar, adParamInput, 64, Left(arrData(37), 64)))          'PriAmtPerDay,
            .Parameters.Append(.CreateParameter("@PriLastUseDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtePriLastUseDate.ToOADate()) = 0, DBNull.Value, dtePriLastUseDate)))          'PriLastUseDate
            .Parameters.Append(.CreateParameter("@Secondary", adVarChar, adParamInput, 10, Left(arrData(39), 10)))          'Secondary,
            .Parameters.Append(.CreateParameter("@SecSubstance", adVarChar, adParamInput, 64, Left(arrData(40), 64)))          'SecSubstance,
            .Parameters.Append(.CreateParameter("@SecStartAge", adInteger, adParamInput, , IIf(CLng(arrData(41)) <= 0, DBNull.Value, CLng(arrData(41)))))          'SecStartAge
            .Parameters.Append(.CreateParameter("@SecFrequency", adVarChar, adParamInput, 64, Left(arrData(42), 64)))          'SecFrequency,
            .Parameters.Append(.CreateParameter("@SecRoute", adVarChar, adParamInput, 64, Left(arrData(43), 64)))          'SecRoute,
            .Parameters.Append(.CreateParameter("@SecDuration", adVarChar, adParamInput, 64, Left(arrData(44), 64)))          'SecDuration,
            .Parameters.Append(.CreateParameter("@SecAmtPerDay", adVarChar, adParamInput, 64, Left(arrData(45), 64)))          'SecAmtPerDay,
            .Parameters.Append(.CreateParameter("@SecLastUseDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dteSecLastUseDate.ToOADate()) = 0, DBNull.Value, dteSecLastUseDate)))          'SecLastUseDate
            .Parameters.Append(.CreateParameter("@Tertiary", adVarChar, adParamInput, 10, Left(arrData(47), 10)))          'Tertiary,
            .Parameters.Append(.CreateParameter("@TerSubstance", adVarChar, adParamInput, 64, Left(arrData(48), 64)))          'TerSubstance,
            .Parameters.Append(.CreateParameter("@TerStartAge", adInteger, adParamInput, , IIf(CLng(arrData(49)) <= 0, DBNull.Value, CLng(arrData(49)))))          'TerStartAge
            .Parameters.Append(.CreateParameter("@TerFrequency", adVarChar, adParamInput, 64, Left(arrData(50), 64)))          'TerFrequency,
            .Parameters.Append(.CreateParameter("@TerRoute", adVarChar, adParamInput, 64, Left(arrData(51), 64)))          'TerRoute,
            .Parameters.Append(.CreateParameter("@TerDuration", adVarChar, adParamInput, 64, Left(arrData(52), 64)))          'TerDuration,
            .Parameters.Append(.CreateParameter("@TerAmtPerDay", adVarChar, adParamInput, 64, Left(arrData(53), 64)))          'TerAmtPerDay,
            .Parameters.Append(.CreateParameter("@TerLastUseDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dteTerLastUseDate.ToOADate()) = 0, DBNull.Value, dteTerLastUseDate)))          'TerLastUseDate

            .Parameters.Append(.CreateParameter("@PhysicalText", adLongVarChar, adParamInput, IIf(Len(strPhysicalText) = 0, 1, Len(strPhysicalText))))
            .Parameters("@PhysicalText").AppendChunk(strPhysicalText)
            .Parameters.Append(.CreateParameter("@LabsText", adLongVarChar, adParamInput, IIf(Len(strLabsText) = 0, 1, Len(strLabsText))))
            .Parameters("@LabsText").AppendChunk(strLabsText)
            .Parameters.Append(.CreateParameter("@EthnicText", adLongVarChar, adParamInput, IIf(Len(strEthnicText) = 0, 1, Len(strEthnicText))))
            .Parameters("@EthnicText").AppendChunk(strEthnicText)
            .Parameters.Append(.CreateParameter("@DiagnosisText", adLongVarChar, adParamInput, IIf(Len(strDiagnosisText) = 0, 1, Len(strDiagnosisText))))
            .Parameters("@DiagnosisText").AppendChunk(strDiagnosisText)
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

    Public Sub UpdateExam(ByVal lngInitialNoteID As Long, ByVal lngPatientID As Long, _
                           ByVal varData As Object, ByVal dteSelfRecentDate As Date, ByVal dteOthRecentDate As Date, _
                           ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description: Updates selected fields in the tblNoteInitial record.  s
        '               stored procedure uspUpdPatientInitialNoteExam.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        '              lngInitialNoteID lngPatientID lngApptID lngTemplateID lngProviderID strNoteText blnLockedYN
        'Returns: Long
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection ' Declare DB connection
        Dim cmd As New ADODB.Command    ' Declare SQL statement



        Dim lngCtr As Long
        Dim arrData() As Object
        ReDim arrData(110)

        If Not IsArray(varData) Or varData.IsNullorEmpty Then
            For lngCtr = 1 To UBound(arrData)
                arrData(lngCtr) = ""
            Next lngCtr
            '        arrData(99) = 0
            '        arrData(106) = 0
        Else
            For lngCtr = 1 To UBound(varData)
                arrData(lngCtr) = _DB.IfNull(varData(lngCtr), "")
            Next lngCtr
            '       arrData(99) = _DB.IfNull(varData(99), 0)
            '       arrData(106) = _DB.IfNull(varData(106), 0)
        End If

        ' Instantiate SQL statement

        'Create the parameter objects
        With cmd
            .CommandText = "uspUpdPatientNoteInitialExam"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@InitialNoteID", adInteger, adParamInput, , lngInitialNoteID))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))      'TerLastUseDate
            .Parameters.Append(.CreateParameter("@Behavior", adVarChar, adParamInput, 40, Left(arrData(55), 40)))          'Behavior,
            .Parameters.Append(.CreateParameter("@BehaviorText", adVarChar, adParamInput, 256, Left(arrData(56), 256)))          'BehaviorText,
            .Parameters.Append(.CreateParameter("@Mood", adVarChar, adParamInput, 40, Left(arrData(57), 40)))          'Mood,
            .Parameters.Append(.CreateParameter("@MoodText", adVarChar, adParamInput, 256, Left(arrData(58), 256)))          'MoodText,
            .Parameters.Append(.CreateParameter("@Perception", adVarChar, adParamInput, 40, Left(arrData(59), 40)))          'Perception,
            .Parameters.Append(.CreateParameter("@PerceptionText", adVarChar, adParamInput, 256, Left(arrData(60), 256)))          'PerceptionText,
            .Parameters.Append(.CreateParameter("@Thought", adVarChar, adParamInput, 40, Left(arrData(61), 40)))          'Thought,
            .Parameters.Append(.CreateParameter("@ThoughtText", adVarChar, adParamInput, 256, Left(arrData(62), 256)))          'ThoughtText,
            .Parameters.Append(.CreateParameter("@Cognition", adVarChar, adParamInput, 10, Left(arrData(63), 10)))          'Cognition,
            .Parameters.Append(.CreateParameter("@CognitionText", adVarChar, adParamInput, 256, Left(arrData(64), 256)))          'CognitionText,
            .Parameters.Append(.CreateParameter("@Judgment", adVarChar, adParamInput, 10, Left(arrData(65), 10)))          'Judgment,
            .Parameters.Append(.CreateParameter("@JudgmentText", adVarChar, adParamInput, 256, Left(arrData(66), 256)))          'JudgmentText,
            .Parameters.Append(.CreateParameter("@Insight", adVarChar, adParamInput, 10, Left(arrData(67), 10)))          'Insight,
            .Parameters.Append(.CreateParameter("@InsightText", adVarChar, adParamInput, 256, Left(arrData(68), 256)))          'InsightText,
            .Parameters.Append(.CreateParameter("@Impulse", adVarChar, adParamInput, 10, Left(arrData(69), 10)))          'Impulse,
            .Parameters.Append(.CreateParameter("@ImpulseText", adVarChar, adParamInput, 256, Left(arrData(70), 256)))          'ImpulseText,
            .Parameters.Append(.CreateParameter("@Speech", adVarChar, adParamInput, 40, Left(arrData(71), 40)))          'Speech,
            .Parameters.Append(.CreateParameter("@SpeechText", adVarChar, adParamInput, 256, Left(arrData(72), 256)))          'SpeechText,
            .Parameters.Append(.CreateParameter("@Language", adVarChar, adParamInput, 10, Left(arrData(73), 10)))          'Language,
            .Parameters.Append(.CreateParameter("@LanguageText", adVarChar, adParamInput, 256, Left(arrData(74), 256)))          'LanguageText,
            .Parameters.Append(.CreateParameter("@Appearance", adVarChar, adParamInput, 40, Left(arrData(75), 40)))          'Appearance,
            .Parameters.Append(.CreateParameter("@AppearanceText", adVarChar, adParamInput, 256, Left(arrData(76), 256)))          'AppearanceText,
            .Parameters.Append(.CreateParameter("@Motor", adVarChar, adParamInput, 40, Left(arrData(77), 40)))          'Motor,
            .Parameters.Append(.CreateParameter("@MotorText", adVarChar, adParamInput, 256, Left(arrData(78), 256)))          'MotorText,
            .Parameters.Append(.CreateParameter("@Orientation", adVarChar, adParamInput, 40, Left(arrData(79), 40)))          'Orientation,
            .Parameters.Append(.CreateParameter("@OrientationText", adVarChar, adParamInput, 256, Left(arrData(80), 256)))          'OrientationText,
            .Parameters.Append(.CreateParameter("@Memory", adVarChar, adParamInput, 10, Left(arrData(81), 10)))          'Memory,
            .Parameters.Append(.CreateParameter("@MemoryText", adVarChar, adParamInput, 256, Left(arrData(82), 256)))          'MemoryText,
            .Parameters.Append(.CreateParameter("@ADLs", adVarChar, adParamInput, 10, Left(arrData(83), 10)))          'ADLs,
            .Parameters.Append(.CreateParameter("@ADLsDoneByText", adVarChar, adParamInput, 256, Left(arrData(84), 256)))          'ADLsDoneByText,
            .Parameters.Append(.CreateParameter("@Interation", adVarChar, adParamInput, 40, Left(arrData(85), 40)))          'Interation,
            .Parameters.Append(.CreateParameter("@InterationText", adVarChar, adParamInput, 256, Left(arrData(86), 256)))          'InterationText,
            .Parameters.Append(.CreateParameter("@Appetite", adVarChar, adParamInput, 10, Left(arrData(87), 10)))          'Appetite,
            .Parameters.Append(.CreateParameter("@AppetiteText", adVarChar, adParamInput, 256, Left(arrData(88), 256)))          'AppetiteText,
            .Parameters.Append(.CreateParameter("@Sleep", adVarChar, adParamInput, 10, Left(arrData(89), 10)))          'Sleep,
            .Parameters.Append(.CreateParameter("@SleepText", adVarChar, adParamInput, 256, Left(arrData(90), 256)))          'SleepText,
            .Parameters.Append(.CreateParameter("@RiskAssess", adVarChar, adParamInput, 10, Left(arrData(91), 10)))          'RiskAssess,
            .Parameters.Append(.CreateParameter("@CurrentRisk", adVarChar, adParamInput, 10, Left(arrData(92), 10)))          'CurrentRisk,
            .Parameters.Append(.CreateParameter("@SelfCurrAttempts", adVarChar, adParamInput, 40, Left(arrData(93), 40)))          'SelfCurrAttempts,
            .Parameters.Append(.CreateParameter("@SelfCurrAttemptsText", adVarChar, adParamInput, 256, Left(arrData(94), 256)))          'SelfCurrAttemptsText,
            .Parameters.Append(.CreateParameter("@SelfPriorAttempts", adVarChar, adParamInput, 40, Left(arrData(95), 40)))          'SelfPriorAttempts,
            .Parameters.Append(.CreateParameter("@SelfPriorAttemptsText", adVarChar, adParamInput, 256, Left(arrData(96), 256)))          'SelfPriorAttemptsText,
            .Parameters.Append(.CreateParameter("@SelfPriorGesture", adVarChar, adParamInput, 40, Left(arrData(97), 40)))          'SelfPriorGesture,
            .Parameters.Append(.CreateParameter("@SelfPriorGestureText", adVarChar, adParamInput, 256, Left(arrData(98), 256)))          'SelfPriorGestureText,
            .Parameters.Append(.CreateParameter("@SelfRecentDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dteSelfRecentDate.ToOADate()) = 0, DBNull.Value, dteSelfRecentDate)))
            .Parameters.Append(.CreateParameter("@OthCurrAttempts", adVarChar, adParamInput, 40, Left(arrData(100), 40)))          'OthCurrAttempts,
            .Parameters.Append(.CreateParameter("@OthCurrAttemptsText", adVarChar, adParamInput, 256, Left(arrData(101), 256)))          'OthCurrAttemptsText,
            .Parameters.Append(.CreateParameter("@OthPriorAttempts", adVarChar, adParamInput, 40, Left(arrData(102), 40)))          'OthPriorAttempts,
            .Parameters.Append(.CreateParameter("@OthPriorAttemptsText", adVarChar, adParamInput, 256, Left(arrData(103), 256)))          'OthPriorAttemptsText,
            .Parameters.Append(.CreateParameter("@OthPriorGesture", adVarChar, adParamInput, 40, Left(arrData(104), 40)))          'OthPriorGesture,
            .Parameters.Append(.CreateParameter("@OthPriorGestureText", adVarChar, adParamInput, 256, Left(arrData(105), 256)))          'OthPriorGestureText,
            .Parameters.Append(.CreateParameter("@OthRecentDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dteOthRecentDate.ToOADate()) = 0, DBNull.Value, dteOthRecentDate)))
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
    Public Sub LockNote(ByVal lngInitialNoteID As Long, ByVal lngSignedBy As Long, ByVal dtDateSigned As Date)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description: Updates a selected record in the tblNoteInitial table, setting
        '               fldLockedYN to Y.  Cannot unlock record!!!  s stored
        '               procedure uspUpdLockPatientInitialNote.
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
            .CommandText = "uspUpdLockPatientInitialNote"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@InitialNoteID", adInteger, adParamInput, , lngInitialNoteID))
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
    Public Sub UnLockNote(ByVal lngInitialNoteID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description: Updates a selected record in the tblNoteInitial table, setting
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
            .CommandText = "uspUpdUnLockPatientInitialNote"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@InitialNoteID", adInteger, adParamInput, , lngInitialNoteID))
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

    Public Sub Delete(ByVal strUserName As String, ByVal lngInitialNoteID As Long)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description: Deletes selected record.  s
        '               stored procedure uspDelPatientInitialNote.
        'Parameters:  lngInitialNoteID - ID of the patient.
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
            .CommandText = "uspDelPatientInitialNote"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@InitialNoteID", adInteger, adParamInput, , lngInitialNoteID))
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



    End Sub



End Class

