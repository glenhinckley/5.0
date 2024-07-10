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


Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CPatApptDB
    '--------------------------------------------------------------------
    'Class Name: CPatApptDB                                             '
    'Date: 08/25/2000                                                   '
    'Author: Rick "Boom Boom" Segura                                    '
    'Description:  MTS object designed to host methods associated with  '
    '              data affecting the tblPatAppt table.                 '
    '--------------------------------------------------------------------
    ' Revision History:
    '  R001: Richkun 11/06/2001: Added FetchCheckInDetails() method
    '  R002: Richkun 02/13/2002: Added FetchByApptPatient() method
    '  R003: Richkun 02/15/2002: Added Reset() method
    '--------------------------------------------------------------------

    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property


    Private Const CLASS_NAME As String = "CPatApptDB"
    Private Const TABLE_NAME As String = "tblPatientAppt"

    '--------------------------------------------------------------------
    ' Public Methods
    '--------------------------------------------------------------------

    Public Function Insert(ByVal lngApptID As Long, ByVal lngPatientID As Long) As Long
        '--------------------------------------------------------------------
        'Date: 07/13/2002
        'Author: Dave Richkun
        'Description:  Inserts a record into tblPatientAppt
        'Parameters:  lngApptID - ID of parent appointment
        '             lngPatientID - ID of patient
        'Returns:  ID of row inserted on success, -1 on failure
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '--------------------------------------------------------------------
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        cnn = New ADODB.Connection
        cmd = New ADODB.Command

        'Create the parameter objects
        With cmd
            .CommandText = "uspInsPatAppt"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@ApptID", adInteger, adParamInput, , lngApptID))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn
        cmd.Execute() '()

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmd.Parameters("@ErrorNum").Value <> 0 Then
            Exit Function
        End If

        Insert = cmd.Parameters("@ID").Value

        'Free resources
        cnn = Nothing
        cmd = Nothing



    End Function

    Public Function Update(ByVal lngID As Long, ByVal lngApptID As Long, _
                           ByVal lngPatientID As Long)
        '--------------------------------------------------------------------
        'Date: 08/25/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Updates a row into the tblPatAppt table utilizing    '
        '              a stored procedure.                                  '
        'Parameters:  lngID - ID of the row in the table whose value will be'
        '               updated.                                            '
        '             lngApptID - The Appointment ID of the patient/appointment
        '               association to which the record will be changed.    '
        '             lngPatientID - The Patient ID of the patient/appointment
        '               association to which the record will be changed.    '
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '--------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim prmID As ADODB.Parameter
        Dim prmDesc As ADODB.Parameter
        Dim prmDisabled As ADODB.Parameter
        Dim varParamErr As Object



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdPatAppt"
            .CommandType = adCmdStoredProc

            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamInput, , lngID))
            .Parameters.Append(.CreateParameter("@ApptID", adInteger, adParamInput, , lngApptID))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()



        'Close the connection and free all resources
        cnnSQL.Close()
        cmdSQL = Nothing
        cnnSQL = Nothing



    End Function

    Public Function FetchByAppt(ByVal lngApptID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/28/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Retrieves records from tblPatAppt that are associated'
        '               with the appointment whose ID matches the parameter '
        'Parameters: lngApptID - ID of the appointment to find related      '
        '               records for                                         '
        'Returns: Recordset from tblPatientAppt                             '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '--------------------------------------------------------------------
        Dim cnn As ADODB.Connection
        Dim cmd As ADODB.Command
        Dim rst As ADODB.Recordset
        Dim SQLErrorNum As Long



        rst = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelPatientApptByAppt"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ApptID", adInteger, adParamInput, , lngApptID))
        End With

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByAppt = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing


    End Function

    Public Function FetchByID(ByVal lngPatApptID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/31/2000
        'Author: Dave Richkun
        'Description: Returns information associated with a patient appt record
        'Parameters:  lngPatApptID - System ID of the record to return
        'Returns:  Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '--------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim rstSQL As ADODB.Recordset
        Dim strSQL As String
        Dim blnExists As Boolean



        rstSQL = New ADODB.Recordset
        cmdSQL = New ADODB.Command
        cnnSQL = New ADODB.Connection

        'Acquire the database connection.
        cnnSQL.Open(_ConnectionString)
        cmdSQL.ActiveConnection = cnnSQL

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspSelPatApptByID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatApptID", adInteger, adParamInput, , lngPatApptID))
        End With

        'Execute the stored procedure
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        FetchByID = rstSQL

        'Disconnect the recordset
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        rstSQL.ActiveConnection = Nothing
        cnnSQL = Nothing


    End Function


    Public Function FetchByApptPatient(ByVal lngApptID As Long, ByVal lngPatientID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 02/13/2002
        'Author: Dave Richkun
        'Description:  Retrieves appointment information using ApptID, PatientID
        'Parameters: lngApptID - ID of the appointment
        '            lngPatientID - ID of the patient
        'Returns: Recordset
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '--------------------------------------------------------------------
        Dim cnn As ADODB.Connection
        Dim cmd As ADODB.Command
        Dim rst As ADODB.Recordset
        Dim SQLErrorNum As Long



        rst = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelApptByApptIDPatient"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ApptID", adInteger, adParamInput, , lngApptID))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
        End With

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByApptPatient = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing



    End Function



    Public Function FetchMissingRec(ByVal lngApptID As Long, ByVal strINClause As String) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/31/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description: Retrieves a recordset of PatAppt records associated   '
        '               with an appointment, but are not in the list given  '
        '               passed in the second parameter                      '
        'Parameters:  lngApptID - ID of the appointment being compared      '
        '             strInClause - Concatenated string of PatAppt IDs that '
        '               are to remain associated with the appointment       '
        'Returns: Recordset of PatAppt records to be considered for deletion'
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '--------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim strSQL As String
        Dim blnExists As Boolean


        'Prepare the SQL statement
        strSQL = "SELECT "
        strSQL = strSQL & " fldPatApptID, "
        strSQL = strSQL & " fldApptStatusID, "
        strSQL = strSQL & " fldPatientID "
        strSQL = strSQL & " FROM "
        strSQL = strSQL & TABLE_NAME
        strSQL = strSQL & " WHERE "
        strSQL = strSQL & " fldApptID = " & lngApptID
        strSQL = strSQL & " AND "
        strSQL = strSQL & " fldPatApptID NOT IN ("
        strSQL = strSQL & strINClause & ")"

        'Instantiate the Recordset.
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = adUseClient

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        ' Populate the recordset
        rstSQL.Open(strSQL, cnnSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)

        FetchMissingRec = rstSQL

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rstSQL.ActiveConnection = Nothing

        'Close the connection and free all resources
        cnnSQL.Close()
        cnnSQL = Nothing
        rstSQL = Nothing



    End Function

    Public Function Delete(ByVal lngApptID As Long, ByVal lngPatientID As Long)
        '--------------------------------------------------------------------
        'Date: 08/25/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Deletes an  record into tblPatAppt                   '
        'Parameters:  lngApptID - System ID of appointment associated with  '
        '               the patient.                                        '
        '             lngPatientID - System ID of the patient associated    '
        '               with the appointment.                               '
        'Returns:  Null                                                     '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '--------------------------------------------------------------------
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection
        Dim varParamErr As Object


        cnn = New ADODB.Connection
        cmd = New ADODB.Command

        'Create the parameter objects
        With cmd
            .CommandText = "uspDelPatAppt"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ApptID", adInteger, adParamInput, , lngApptID))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))

            'Assign the connection to the Command object and execute the stored procedure
            cnn.Open(_ConnectionString)
            .ActiveConnection = cnn
            .Execute() '()
            cnn = Nothing

            cmd = Nothing
        End With



    End Function

    Public Function Exists(ByVal lngApptID As Long, ByVal lngPatientID As Long) As Boolean
        '--------------------------------------------------------------------
        'Date: 08/25/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description: Determines if an Appointment/Patient association      '
        '               exists in tblPatAppointment                         '
        'Parameters:  lngApptID - System ID of appointment associated with  '
        '               the patient.                                        '
        '             lngPatientID - System ID of the patient associated    '
        '               with the appointment.                               '
        'Returns:  TRUE if association is found, FALSE otherwise            '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '--------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim strSQL As String
        Dim blnExists As Boolean


        'Prepare the SQL statement
        strSQL = "SELECT "
        strSQL = strSQL & "COUNT(*) AS TOTAL "
        strSQL = strSQL & "FROM "
        strSQL = strSQL & TABLE_NAME
        strSQL = strSQL & " WHERE "
        strSQL = strSQL & " fldApptID = " & lngApptID
        strSQL = strSQL & " AND "
        strSQL = strSQL & " fldPatientID = " & lngPatientID

        'Instantiate the Recordset.
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = adUseClient

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        ' Populate the recordset
        rstSQL.Open(strSQL, cnnSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rstSQL.ActiveConnection = Nothing

        'Close the connection and free all resources
        cnnSQL.Close()
        cnnSQL = Nothing

        'Get the value from the Recordset then destroy it.
        If rstSQL.Fields("TOTAL").Value > 0 Then
            Exists = True
        Else
            Exists = False
        End If
        rstSQL = Nothing


    End Function

    Public Sub ChangeStatus(ByVal lngPatApptID As Long, ByVal lngApptStatusID As Long, _
            ByVal strNoShowFlag As String, ByVal dblCancelFee As Double, _
            ByVal strCancelExplain As String, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 09/01/2000
        'Author: Dave Richkun
        'Description:  Updates the status of a single Patient-Appointment record
        'Parameters:  lngPatApptID - ID of the row in tblPatientAppt whose status value will be updated
        '             lngApptStatusID - The value representing the updated appointment status
        '             strNoShowFlag - If appointment status is 'No-Show', represents the user's decision
        '                   to manage the No-Show.  May be one of:
        '                    N - Do not Bill for No-Show
        '                    P - Bill patient for No-Show
        '                    I - Bill Insurance Company for No-Show
        '             dblCancelFee - If appointment status is 'Cancelled', represents the amount
        '                   charged to patient as Cancellation Fee
        '             strCancelExplain - If appointment is 'Cancelled', represents the user's explanation
        '                   for cancellation (assumed to be given by patient)
        '             strUserName - User performng action
        'Returns:  Null
        '--------------------------------------------------------------------
        'Revision History:
        '  03/15/2001 Segura:  Added new parameter(strNoShowFlag)
        '--------------------------------------------------------------------
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        cnn = New ADODB.Connection
        cmd = New ADODB.Command

        'Create the parameter objects
        With cmd
            .CommandText = "uspUpdPatApptStatus"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatApptID", adInteger, adParamInput, , lngPatApptID))
            .Parameters.Append(.CreateParameter("@ApptStatusID", adInteger, adParamInput, , lngApptStatusID))
            .Parameters.Append(.CreateParameter("@NoShowFlag", adVarChar, adParamInput, 1, IIf(strNoShowFlag = "", DBNull.Value, strNoShowFlag)))
            .Parameters.Append(.CreateParameter("@CancelFee", adDecimal, adParamInput, , dblCancelFee))
            .Parameters.Append(.CreateParameter("@CancelExplain", adVarChar, adParamInput, 90, IIf(strCancelExplain = "", DBNull.Value, strCancelExplain)))
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn
        cmd.Execute() '()
        cnn = Nothing


        cmd = Nothing


    End Sub

    Public Sub ChangeStatusByApptPat(ByVal lngApptID As Long, _
                                     ByVal lngPatientID As Long, _
                                     ByVal lngStatusID As Long, _
                                     ByVal strNoShowFlag As String, _
                                     ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 10/02/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Updates the value of the Status ID of a single       '
        '               Patient/Appointment record                          '
        'Parameters:  lngApptID - ID of the associated Appointment          '
        '             lngPatientID - ID of the associated Patient           '
        '             lngStatusID - The new value of the Status ID          '
        '             strNoShowFlag -  NoShow Flag                          '
        '             strUserName - User Login Name                         '
        'Returns:   Nothing                                                 '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '  03/15/2001 Segura:  Added new parameter(strNoShowFlag)           '
        '--------------------------------------------------------------------
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection
        Dim varParamErr As Object



        cnn = New ADODB.Connection
        cmd = New ADODB.Command

        'Create the parameter objects
        With cmd
            .CommandText = "uspUpdPatApptStatusByApptPat"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ApptID", adInteger, adParamInput, , lngApptID))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@StatusID", adInteger, adParamInput, , lngStatusID))
            If strNoShowFlag > "" Then
                .Parameters.Append(.CreateParameter("@NoShowFlag", adVarChar, adParamInput, 1, strNoShowFlag))
            Else
                .Parameters.Append(.CreateParameter("@NoShowFlag", adVarChar, adParamInput, 1, DBNull.Value))
            End If
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))

            'Assign the connection to the Command object and execute the stored procedure
            cnn.Open(_ConnectionString)
            .ActiveConnection = cnn
            .Execute() '()
            cnn = Nothing

            'Check the ErrorNum parameter before deciding to commit the transaction
            If .Parameters("@ErrorNum").Value <> 0 Then
                varParamErr = .Parameters("@ErrorNum").Value
                Exit Sub
            End If
            cmd = Nothing
        End With



        Exit Sub

        'Signal incompletion and raise the error to the ing environment.
        '   cmdSQL.Execute()
        cnn = Nothing
        cmd = Nothing
        If Err.Number <> 0 Then
            'err.raise(Err.Number, Err.Source, Err.Description)
        Else
            'RaiseError(Err, varParamErr, CLASS_NAME)
        End If
    End Sub

    Public Sub DeleteByID(ByVal lngPatApptID As Long)
        '--------------------------------------------------------------------
        'Date: 10/02/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Physiy deletes a record from tblPatientAppt      '
        'Parameters:  lngPatApptID - ID of the record to delete             '
        'Returns:   Nothing                                                 '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '--------------------------------------------------------------------
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection
        Dim varParamErr As Object



        cnn = New ADODB.Connection
        cmd = New ADODB.Command

        'Create the parameter objects
        With cmd
            .CommandText = "uspDelPatApptByID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatApptID", adInteger, adParamInput, , lngPatApptID))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))

            'Assign the connection to the Command object and execute the stored procedure
            cnn.Open(_ConnectionString)
            .ActiveConnection = cnn
            .Execute() '()
            cnn = Nothing

            'Check the ErrorNum parameter before deciding to commit the transaction
            If .Parameters("@ErrorNum").Value <> 0 Then
                varParamErr = .Parameters("@ErrorNum").Value
                Exit Sub
            End If
            cmd = Nothing
        End With



        Exit Sub

        'Signal incompletion and raise the error to the ing environment.
        ' cmdSQL.Execute()
        cnn = Nothing
        cmd = Nothing
        If Err.Number <> 0 Then
            'err.raise(Err.Number, Err.Source, Err.Description)
        Else
            'RaiseError(Err, varParamErr, CLASS_NAME)
        End If
    End Sub


    Public Function FetchCheckInDetails(ByVal lngPatApptID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 11/06/2001
        'Author: Dave Richkun
        'Description:  Retrieves detailed Appointment information for a specific
        '              Patient-Appointment ID
        'Parameters: lngPatApptID - ID of the patient-appointment
        'Returns: ADO Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '  R001: Created
        '--------------------------------------------------------------------
        Dim cnn As ADODB.Connection
        Dim cmd As ADODB.Command
        Dim rst As ADODB.Recordset



        rst = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelCheckInDetails"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatApptID", adInteger, adParamInput, , lngPatApptID))
        End With

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchCheckInDetails = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        '  cmdSQL.Execute()
        cnn = Nothing
        cmd = Nothing
        rst = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function
    Public Function UpdateUnbilledCounter(ByVal lngProviderID As Long, ByVal lngValue As Long)
        '--------------------------------------------------------------------
        'Date: 01/09/2002                                                  '
        'Author: Eric Pena                                   '
        'Description:  Updates the unbilled counter in tblProvider
        'Parameters:  lngProviderID - Provider whose counter will be updated
        '             lngValue - Number to inc(dec)rement by (+- 1)
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '--------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim prmID As ADODB.Parameter
        Dim prmDesc As ADODB.Parameter
        Dim prmDisabled As ADODB.Parameter
        Dim varParamErr As Object



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdCtr_Unbilled"
            .CommandType = adCmdStoredProc

            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@Value", adInteger, adParamInput, , lngValue))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check for errors
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            varParamErr = cmdSQL.Parameters("@SQLErrorNum").Value
            Exit Function
        End If

        'Close the connection and free all resources
        cnnSQL.Close()
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.Execute()
        cnnSQL = Nothing
        cmdSQL = Nothing
        If Err.Number <> 0 Then
            'err.raise(Err.Number, Err.Source, Err.Description)
        Else
            'RaiseError(Err, varParamErr, CLASS_NAME)
        End If

    End Function


    Public Function Reset(ByVal lngPatApptID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 02/15/2002
        'Author: Dave Richkun
        'Description:  Resets a patient appointment to a status of 'Attended' and
        '              reverses any prior Cancellation and No-Show cancellations
        '              applied against the patient.
        'Parameters:  lngPatApptID - ID of the patient appointmnt
        '             strUserName - User name of the user initiating the method
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '--------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdResetAppt"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatApptID", adInteger, adParamInput, , lngPatApptID))
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check for errors
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Function
        End If

        'Close the connection and free all resources
        cnnSQL.Close()
        cmdSQL = Nothing
        cnnSQL = Nothing


    End Function




    '--------------------------------------------------------------------
    ' PrivateMethods    +++++++++++++++++++++++++++++++++++++++++++++++++
    '--------------------------------------------------------------------


End Class

