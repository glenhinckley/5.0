
'--------------------------------------------------------------------
'Class Name: CIntakeAdultDB
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Database object designed to implement methods (by ing
'              stored procedures) associated with
'              data affecting the tblIntakeAdult table.
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

Public Class CIntakeAdultDB


    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CIntakeAdultDB"
    Private Const TABLE_NAME As String = "tblIntakeAdult"


    Public Function FetchByID(ByVal lngIntakeAdultID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description: Retrieves records from the tblIntakeAdult table.  s
        '               stored procedure uspSelIntakeAdultByID.
        'Parameters: lngIntakeAdultID - 'Where' clause of the SQL statement by
        '               which records will be fetched.  Matches tblIntakeAdult's
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
            .CommandText = "uspSelIntakeAdultByID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@IntakeAdultID", adInteger, adParamInput, , lngIntakeAdultID))
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
    Public Function FetchByPatient(ByVal lngPatientID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description: Retrieves records from the tblIntakeAdult table.  s
        '               stored procedure uspSelIntakeAdultByPatient.
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
            .CommandText = "uspSelIntakeAdultByPatient"
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
    Public Function Insert(ByVal lngPatientID As Long, ByVal lngProviderID As Long, ByVal lngClinicID As Long, ByVal dtNoteDate As Date, ByVal strChiefConcern As String, ByVal strPCPClinic As String, _
                           ByVal strPCPClinicPhone As String, ByVal strPCPName As String, ByVal strPCPAddress As String, ByVal blnContactPCPYN As Boolean, ByVal strEmployer As String, ByVal strWorkPhone As String, _
                           ByVal strWorkAddress As String, ByVal strOccupation As String, ByVal strDurantion As String, ByVal strRestrictions As String, ByVal strSpouse As String, ByVal strChildren As String, _
                           ByVal blnPastTreatYN As Boolean, ByVal strPastTreatType As String, ByVal strPastTreatWhen1 As String, ByVal strPastTreatWhom1 As String, ByVal strPastTreatWhat1 As String, _
                           ByVal strPastTreatResult1 As String, ByVal strPastTreatWhen2 As String, ByVal strPastTreatWhom2 As String, ByVal strPastTreatWhat2 As String, ByVal strPastTreatResult2 As String, _
                           ByVal blnPastMedsYN As Boolean, ByVal strPastMedsWhen1 As String, ByVal strPastMedsWhom1 As String, ByVal strPastMedsWhat1 As String, ByVal strPastMedsResult1 As String, _
                           ByVal strPastMedsWhen2 As String, ByVal strPastMedsWhom2 As String, ByVal strPastMedsWhat2 As String, ByVal strPastMedsResult2 As String, ByVal strSymptoms As String, _
                           ByVal strRelationship As String, ByVal strFamily As String, ByVal strWorkPerformance As String, ByVal strFriendships As String, ByVal strFinancial As String, ByVal strHealth As String, _
                           ByVal strStress As String, ByVal strMood As String, ByVal strAppetite As String, ByVal strSleep As String, ByVal strSexual As String, ByVal strAlcohol As String, _
                           ByVal strConcentrate As String, ByVal strAnger As String, _
                           ByVal blnLockedYN As Boolean, ByVal lngSignedBy As Long, _
                           ByVal dtDateSigned As Date, ByVal strUserName As String) As Long

        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description: Inserts records into the tblIntakeAdult table.  s
        '               stored procedure uspInsIntakeAdult.
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
            .CommandText = "uspInsPatientIntakeAdult"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@IntakeAdultID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@ClinicID", adInteger, adParamInput, , IIf(lngClinicID <= 0, DBNull.Value, lngClinicID)))
            .Parameters.Append(.CreateParameter("@NoteDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtNoteDate.ToOADate()) = 0, Now(), dtNoteDate)))

            .Parameters.Append(.CreateParameter("@ChiefConcern", adVarChar, adParamInput, 512, Left(strChiefConcern, 512)))
            .Parameters.Append(.CreateParameter("@PCPClinic", adVarChar, adParamInput, 128, Left(strPCPClinic, 128)))
            .Parameters.Append(.CreateParameter("@PCPClinicPhone", adVarChar, adParamInput, 16, Left(strPCPClinicPhone, 16)))
            .Parameters.Append(.CreateParameter("@PCPName", adVarChar, adParamInput, 128, Left(strPCPName, 128)))
            .Parameters.Append(.CreateParameter("@PCPAddress", adVarChar, adParamInput, 256, Left(strPCPAddress, 256)))
            .Parameters.Append(.CreateParameter("@ContactPCPYN", adVarChar, adParamInput, 1, IIf(blnContactPCPYN = False, "N", "Y")))
            .Parameters.Append(.CreateParameter("@Employer", adVarChar, adParamInput, 128, Left(strEmployer, 128)))
            .Parameters.Append(.CreateParameter("@WorkPhone", adVarChar, adParamInput, 16, Left(strWorkPhone, 16)))
            .Parameters.Append(.CreateParameter("@WorkAddress", adVarChar, adParamInput, 256, Left(strWorkAddress, 256)))
            .Parameters.Append(.CreateParameter("@Occupation", adVarChar, adParamInput, 128, Left(strOccupation, 128)))
            .Parameters.Append(.CreateParameter("@Durantion", adVarChar, adParamInput, 128, Left(strDurantion, 128)))
            .Parameters.Append(.CreateParameter("@Restrictions", adVarChar, adParamInput, 128, Left(strRestrictions, 128)))
            .Parameters.Append(.CreateParameter("@Spouse", adVarChar, adParamInput, 256, Left(strSpouse, 256)))
            .Parameters.Append(.CreateParameter("@Children", adVarChar, adParamInput, 256, Left(strChildren, 256)))
            .Parameters.Append(.CreateParameter("@PastTreatYN", adVarChar, adParamInput, 1, IIf(blnPastTreatYN = False, "N", "Y")))
            .Parameters.Append(.CreateParameter("@PastTreatType", adVarChar, adParamInput, 4, Left(strPastTreatType, 4)))
            .Parameters.Append(.CreateParameter("@PastTreatWhen1", adVarChar, adParamInput, 128, Left(strPastTreatWhen1, 128)))
            .Parameters.Append(.CreateParameter("@PastTreatWhom1", adVarChar, adParamInput, 128, Left(strPastTreatWhom1, 128)))
            .Parameters.Append(.CreateParameter("@PastTreatWhat1", adVarChar, adParamInput, 128, Left(strPastTreatWhat1, 128)))
            .Parameters.Append(.CreateParameter("@PastTreatResult1", adVarChar, adParamInput, 256, Left(strPastTreatResult1, 256)))
            .Parameters.Append(.CreateParameter("@PastTreatWhen2", adVarChar, adParamInput, 128, Left(strPastTreatWhen2, 128)))
            .Parameters.Append(.CreateParameter("@PastTreatWhom2", adVarChar, adParamInput, 128, Left(strPastTreatWhom2, 128)))
            .Parameters.Append(.CreateParameter("@PastTreatWhat2", adVarChar, adParamInput, 128, Left(strPastTreatWhat2, 128)))
            .Parameters.Append(.CreateParameter("@PastTreatResult2", adVarChar, adParamInput, 256, Left(strPastTreatResult2, 256)))
            .Parameters.Append(.CreateParameter("@PastMedsYN", adVarChar, adParamInput, 1, IIf(blnPastMedsYN = False, "N", "Y")))
            .Parameters.Append(.CreateParameter("@PastMedsWhen1", adVarChar, adParamInput, 128, Left(strPastMedsWhen1, 128)))
            .Parameters.Append(.CreateParameter("@PastMedsWhom1", adVarChar, adParamInput, 128, Left(strPastMedsWhom1, 128)))
            .Parameters.Append(.CreateParameter("@PastMedsWhat1", adVarChar, adParamInput, 128, Left(strPastMedsWhat1, 128)))
            .Parameters.Append(.CreateParameter("@PastMedsResult1", adVarChar, adParamInput, 256, Left(strPastMedsResult1, 256)))
            .Parameters.Append(.CreateParameter("@PastMedsWhen2", adVarChar, adParamInput, 128, Left(strPastMedsWhen2, 128)))
            .Parameters.Append(.CreateParameter("@PastMedsWhom2", adVarChar, adParamInput, 128, Left(strPastMedsWhom2, 128)))
            .Parameters.Append(.CreateParameter("@PastMedsWhat2", adVarChar, adParamInput, 128, Left(strPastMedsWhat2, 128)))
            .Parameters.Append(.CreateParameter("@PastMedsResult2", adVarChar, adParamInput, 256, Left(strPastMedsResult2, 256)))
            .Parameters.Append(.CreateParameter("@Symptoms", adVarChar, adParamInput, 256, Left(strSymptoms, 256)))
            .Parameters.Append(.CreateParameter("@Relationship", adVarChar, adParamInput, 16, Left(strRelationship, 16)))
            .Parameters.Append(.CreateParameter("@Family", adVarChar, adParamInput, 16, Left(strFamily, 16)))
            .Parameters.Append(.CreateParameter("@WorkPerformance", adVarChar, adParamInput, 16, Left(strWorkPerformance, 16)))
            .Parameters.Append(.CreateParameter("@Friendships", adVarChar, adParamInput, 16, Left(strFriendships, 16)))
            .Parameters.Append(.CreateParameter("@Financial", adVarChar, adParamInput, 16, Left(strFinancial, 16)))
            .Parameters.Append(.CreateParameter("@Health", adVarChar, adParamInput, 16, Left(strHealth, 16)))
            .Parameters.Append(.CreateParameter("@Stress", adVarChar, adParamInput, 16, Left(strStress, 16)))
            .Parameters.Append(.CreateParameter("@Mood", adVarChar, adParamInput, 16, Left(strMood, 16)))
            .Parameters.Append(.CreateParameter("@Appetite", adVarChar, adParamInput, 16, Left(strAppetite, 16)))
            .Parameters.Append(.CreateParameter("@Sleep", adVarChar, adParamInput, 16, Left(strSleep, 16)))
            .Parameters.Append(.CreateParameter("@Sexual", adVarChar, adParamInput, 16, Left(strSexual, 16)))
            .Parameters.Append(.CreateParameter("@Alcohol", adVarChar, adParamInput, 16, Left(strAlcohol, 16)))
            .Parameters.Append(.CreateParameter("@Concentrate", adVarChar, adParamInput, 16, Left(strConcentrate, 16)))
            .Parameters.Append(.CreateParameter("@Anger", adVarChar, adParamInput, 16, Left(strAnger, 16)))

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

    Public Sub Update(ByVal lngIntakeAdultID As Long, ByVal lngPatientID As Long, ByVal lngProviderID As Long, ByVal lngClinicID As Long, ByVal dtNoteDate As Date, ByVal strChiefConcern As String, ByVal strPCPClinic As String, _
                           ByVal strPCPClinicPhone As String, ByVal strPCPName As String, ByVal strPCPAddress As String, ByVal blnContactPCPYN As Boolean, ByVal strEmployer As String, ByVal strWorkPhone As String, _
                           ByVal strWorkAddress As String, ByVal strOccupation As String, ByVal strDurantion As String, ByVal strRestrictions As String, ByVal strSpouse As String, ByVal strChildren As String, _
                           ByVal blnPastTreatYN As Boolean, ByVal strPastTreatType As String, ByVal strPastTreatWhen1 As String, ByVal strPastTreatWhom1 As String, ByVal strPastTreatWhat1 As String, _
                           ByVal strPastTreatResult1 As String, ByVal strPastTreatWhen2 As String, ByVal strPastTreatWhom2 As String, ByVal strPastTreatWhat2 As String, ByVal strPastTreatResult2 As String, _
                           ByVal blnPastMedsYN As Boolean, ByVal strPastMedsWhen1 As String, ByVal strPastMedsWhom1 As String, ByVal strPastMedsWhat1 As String, ByVal strPastMedsResult1 As String, _
                           ByVal strPastMedsWhen2 As String, ByVal strPastMedsWhom2 As String, ByVal strPastMedsWhat2 As String, ByVal strPastMedsResult2 As String, ByVal strSymptoms As String, _
                           ByVal strRelationship As String, ByVal strFamily As String, ByVal strWorkPerformance As String, ByVal strFriendships As String, ByVal strFinancial As String, ByVal strHealth As String, _
                           ByVal strStress As String, ByVal strMood As String, ByVal strAppetite As String, ByVal strSleep As String, ByVal strSexual As String, ByVal strAlcohol As String, _
                           ByVal strConcentrate As String, ByVal strAnger As String, _
                           ByVal blnLockedYN As Boolean, ByVal lngSignedBy As Long, _
                           ByVal dtDateSigned As Date, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description: Updates selected records in the tblIntakeAdult table.  s
        '               stored procedure uspUpdIntakeAdult.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        '              lngIntakeAdultID lngPatientID lngApptID lngTemplateID lngProviderID strNoteText blnLockedYN
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
            .CommandText = "uspUpdPatientIntakeAdult"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@IntakeAdultID", adInteger, adParamInput, , lngIntakeAdultID))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@ClinicID", adInteger, adParamInput, , IIf(lngClinicID <= 0, DBNull.Value, lngClinicID)))
            .Parameters.Append(.CreateParameter("@NoteDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtNoteDate.ToOADate()) = 0, Now(), dtNoteDate)))

            .Parameters.Append(.CreateParameter("@ChiefConcern", adVarChar, adParamInput, 512, Left(strChiefConcern, 512)))
            .Parameters.Append(.CreateParameter("@PCPClinic", adVarChar, adParamInput, 128, Left(strPCPClinic, 128)))
            .Parameters.Append(.CreateParameter("@PCPClinicPhone", adVarChar, adParamInput, 16, Left(strPCPClinicPhone, 16)))
            .Parameters.Append(.CreateParameter("@PCPName", adVarChar, adParamInput, 128, Left(strPCPName, 128)))
            .Parameters.Append(.CreateParameter("@PCPAddress", adVarChar, adParamInput, 256, Left(strPCPAddress, 256)))
            .Parameters.Append(.CreateParameter("@ContactPCPYN", adVarChar, adParamInput, 1, IIf(blnContactPCPYN = False, "N", "Y")))
            .Parameters.Append(.CreateParameter("@Employer", adVarChar, adParamInput, 128, Left(strEmployer, 128)))
            .Parameters.Append(.CreateParameter("@WorkPhone", adVarChar, adParamInput, 16, Left(strWorkPhone, 16)))
            .Parameters.Append(.CreateParameter("@WorkAddress", adVarChar, adParamInput, 256, Left(strWorkAddress, 256)))
            .Parameters.Append(.CreateParameter("@Occupation", adVarChar, adParamInput, 128, Left(strOccupation, 128)))
            .Parameters.Append(.CreateParameter("@Durantion", adVarChar, adParamInput, 128, Left(strDurantion, 128)))
            .Parameters.Append(.CreateParameter("@Restrictions", adVarChar, adParamInput, 128, Left(strRestrictions, 128)))
            .Parameters.Append(.CreateParameter("@Spouse", adVarChar, adParamInput, 256, Left(strSpouse, 256)))
            .Parameters.Append(.CreateParameter("@Children", adVarChar, adParamInput, 256, Left(strChildren, 256)))
            .Parameters.Append(.CreateParameter("@PastTreatYN", adVarChar, adParamInput, 1, IIf(blnPastTreatYN = False, "N", "Y")))
            .Parameters.Append(.CreateParameter("@PastTreatType", adVarChar, adParamInput, 4, Left(strPastTreatType, 4)))
            .Parameters.Append(.CreateParameter("@PastTreatWhen1", adVarChar, adParamInput, 128, Left(strPastTreatWhen1, 128)))
            .Parameters.Append(.CreateParameter("@PastTreatWhom1", adVarChar, adParamInput, 128, Left(strPastTreatWhom1, 128)))
            .Parameters.Append(.CreateParameter("@PastTreatWhat1", adVarChar, adParamInput, 128, Left(strPastTreatWhat1, 128)))
            .Parameters.Append(.CreateParameter("@PastTreatResult1", adVarChar, adParamInput, 256, Left(strPastTreatResult1, 256)))
            .Parameters.Append(.CreateParameter("@PastTreatWhen2", adVarChar, adParamInput, 128, Left(strPastTreatWhen2, 128)))
            .Parameters.Append(.CreateParameter("@PastTreatWhom2", adVarChar, adParamInput, 128, Left(strPastTreatWhom2, 128)))
            .Parameters.Append(.CreateParameter("@PastTreatWhat2", adVarChar, adParamInput, 128, Left(strPastTreatWhat2, 128)))
            .Parameters.Append(.CreateParameter("@PastTreatResult2", adVarChar, adParamInput, 256, Left(strPastTreatResult2, 256)))
            .Parameters.Append(.CreateParameter("@PastMedsYN", adVarChar, adParamInput, 1, IIf(blnPastMedsYN = False, "N", "Y")))
            .Parameters.Append(.CreateParameter("@PastMedsWhen1", adVarChar, adParamInput, 128, Left(strPastMedsWhen1, 128)))
            .Parameters.Append(.CreateParameter("@PastMedsWhom1", adVarChar, adParamInput, 128, Left(strPastMedsWhom1, 128)))
            .Parameters.Append(.CreateParameter("@PastMedsWhat1", adVarChar, adParamInput, 128, Left(strPastMedsWhat1, 128)))
            .Parameters.Append(.CreateParameter("@PastMedsResult1", adVarChar, adParamInput, 256, Left(strPastMedsResult1, 256)))
            .Parameters.Append(.CreateParameter("@PastMedsWhen2", adVarChar, adParamInput, 128, Left(strPastMedsWhen2, 128)))
            .Parameters.Append(.CreateParameter("@PastMedsWhom2", adVarChar, adParamInput, 128, Left(strPastMedsWhom2, 128)))
            .Parameters.Append(.CreateParameter("@PastMedsWhat2", adVarChar, adParamInput, 128, Left(strPastMedsWhat2, 128)))
            .Parameters.Append(.CreateParameter("@PastMedsResult2", adVarChar, adParamInput, 256, Left(strPastMedsResult2, 256)))
            .Parameters.Append(.CreateParameter("@Symptoms", adVarChar, adParamInput, 256, Left(strSymptoms, 256)))
            .Parameters.Append(.CreateParameter("@Relationship", adVarChar, adParamInput, 16, Left(strRelationship, 16)))
            .Parameters.Append(.CreateParameter("@Family", adVarChar, adParamInput, 16, Left(strFamily, 16)))
            .Parameters.Append(.CreateParameter("@WorkPerformance", adVarChar, adParamInput, 16, Left(strWorkPerformance, 16)))
            .Parameters.Append(.CreateParameter("@Friendships", adVarChar, adParamInput, 16, Left(strFriendships, 16)))
            .Parameters.Append(.CreateParameter("@Financial", adVarChar, adParamInput, 16, Left(strFinancial, 16)))
            .Parameters.Append(.CreateParameter("@Health", adVarChar, adParamInput, 16, Left(strHealth, 16)))
            .Parameters.Append(.CreateParameter("@Stress", adVarChar, adParamInput, 16, Left(strStress, 16)))
            .Parameters.Append(.CreateParameter("@Mood", adVarChar, adParamInput, 16, Left(strMood, 16)))
            .Parameters.Append(.CreateParameter("@Appetite", adVarChar, adParamInput, 16, Left(strAppetite, 16)))
            .Parameters.Append(.CreateParameter("@Sleep", adVarChar, adParamInput, 16, Left(strSleep, 16)))
            .Parameters.Append(.CreateParameter("@Sexual", adVarChar, adParamInput, 16, Left(strSexual, 16)))
            .Parameters.Append(.CreateParameter("@Alcohol", adVarChar, adParamInput, 16, Left(strAlcohol, 16)))
            .Parameters.Append(.CreateParameter("@Concentrate", adVarChar, adParamInput, 16, Left(strConcentrate, 16)))
            .Parameters.Append(.CreateParameter("@Anger", adVarChar, adParamInput, 16, Left(strAnger, 16)))

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

    Public Sub UpdateText(ByVal lngIntakeAdultID As Long, ByVal lngPatientID As Long, _
                          ByVal blnAlcoholCurrentYN As Boolean, ByVal strAlcoholUsage As String, ByVal strAlcoholDays As String, _
                          ByVal blnAlcoholHistoryYN As Boolean, ByVal blnAlcoholConcernsYN As Boolean, ByVal blnDrugsCurrentYN As Boolean, _
                          ByVal blnDrugHistoryYN As Boolean, ByVal blnDrugFamilyHistoryYN As Boolean, _
                          ByVal strDrugFamilyHistory As String, ByVal strOther As String, ByVal strNoteText As String)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description: Updates selected records in the tblIntakeAdult table.  s
        '               stored procedure uspUpdIntakeAdult.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        '              lngIntakeAdultID lngPatientID lngApptID lngTemplateID lngProviderID strNoteText blnLockedYN
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
            .CommandText = "uspUpdPatientIntakeAdultText"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@IntakeAdultID", adInteger, adParamInput, , lngIntakeAdultID))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@AlcoholCurrentYN", adVarChar, adParamInput, 1, IIf(blnAlcoholCurrentYN = False, "N", "Y")))
            .Parameters.Append(.CreateParameter("@AlcoholUsage", adVarChar, adParamInput, 128, Left(strAlcoholUsage, 128)))
            .Parameters.Append(.CreateParameter("@AlcoholDays", adVarChar, adParamInput, 128, Left(strAlcoholDays, 128)))
            .Parameters.Append(.CreateParameter("@AlcoholHistoryYN", adVarChar, adParamInput, 1, IIf(blnAlcoholHistoryYN = False, "N", "Y")))
            .Parameters.Append(.CreateParameter("@AlcoholConcernsYN", adVarChar, adParamInput, 1, IIf(blnAlcoholConcernsYN = False, "N", "Y")))
            .Parameters.Append(.CreateParameter("@DrugsCurrentYN", adVarChar, adParamInput, 1, IIf(blnDrugsCurrentYN = False, "N", "Y")))
            .Parameters.Append(.CreateParameter("@DrugHistoryYN", adVarChar, adParamInput, 1, IIf(blnDrugHistoryYN = False, "N", "Y")))
            .Parameters.Append(.CreateParameter("@DrugFamilyHistoryYN", adVarChar, adParamInput, 1, IIf(blnDrugFamilyHistoryYN = False, "N", "Y")))
            .Parameters.Append(.CreateParameter("@DrugFamilyHistory", adLongVarChar, adParamInput, IIf(Len(strDrugFamilyHistory) = 0, 1, Len(strDrugFamilyHistory))))
            .Parameters("@DrugFamilyHistory").AppendChunk(strDrugFamilyHistory)
            .Parameters.Append(.CreateParameter("@Other", adLongVarChar, adParamInput, IIf(Len(strOther) = 0, 1, Len(strOther))))
            .Parameters("@Other").AppendChunk(strOther)
            .Parameters.Append(.CreateParameter("@NoteText", adLongVarChar, adParamInput, IIf(Len(strNoteText) = 0, 1, Len(strNoteText))))
            .Parameters("@NoteText").AppendChunk(strNoteText)
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

    Public Sub LockNote(ByVal lngIntakeAdultID As Long, ByVal lngSignedBy As Long, ByVal dtDateSigned As Date)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description: Updates a selected record in the tblIntakeAdult table, setting
        '               fldLockedYN to Y.  Cannot unlock record!!!  s stored
        '               procedure uspUpdLockIntakeAdult.
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
            .CommandText = "uspUpdLockIntakeAdult"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@IntakeAdultID", adInteger, adParamInput, , lngIntakeAdultID))
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
    Public Sub UnLockNote(ByVal lngIntakeAdultID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description: Updates a selected record in the tblIntakeAdult table, setting
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
            .CommandText = "uspUpdUnLockIntakeAdult"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@IntakeAdultID", adInteger, adParamInput, , lngIntakeAdultID))
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



        'err.raise(Err.Number, Err.Source, Err.Description)
    End Sub

    Public Sub Delete(ByVal strUserName As String, ByVal lngIntakeAdultID As Long)
        '--------------------------------------------------------------------
        'Date: 07/12/2017
        'Author: Duane C Orth
        'Description: Deletes selected record.  s
        '               stored procedure uspDelIntakeAdult.
        'Parameters:  lngIntakeAdultID - ID of the patient.
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
            .CommandText = "uspDelIntakeAdult"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@IntakeAdultID", adInteger, adParamInput, , lngIntakeAdultID))
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


