
'----------------------------------------------------------------------------
'Class Name: CPatientTxBz
'Date: 07/09/2002
'Author: Dave Richkun
'Description:  COM+ object designed to host methods associated with patient
'              transactions.
'----------------------------------------------------------------------------
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

Public Class CPatientTxBz
    Private _ConnectionString As String = String.Empty


    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property




    Public Function Insert(ByVal lngELID As Long, ByVal lngPatientID As Long, _
            ByVal lngProviderID As Long, ByVal lngRPID As Long, ByVal strTxTypeCode As String, _
            ByVal lngTxTypeID As Long, ByVal dtPostDate As Date, ByVal dtRcvDate As Date, _
            ByVal dblAmount As Double, ByVal strCheckNum As String, ByVal dtcheckDate As Date, _
            ByVal strCCTxID As String, ByVal strTxNotes As String, ByVal lngPatApptID As Long, ByVal strUserName As String) As Long
        '--------------------------------------------------------------------
        'Date: 07/09/2002
        'Author: Dave Richkun
        'Description:  Inserts a row into the tblPatientTx table utilizing a stored procedure.
        'Parameters:
        '
        '   *** TO DO: Define each parameter once procedure is finalized ****
        '
        'Returns: ID of the row inserted.
        '--------------------------------------------------------------------
        Dim objTx As New BillingDB.CPatientTXDB
        Dim objBR As New BillingBZ.CBRBz


        Insert = objTx.Insert(lngELID, lngPatientID, lngProviderID, lngRPID, strTxTypeCode, _
                lngTxTypeID, dtPostDate, dtRcvDate, dblAmount, strCheckNum, dtcheckDate, _
                strCCTxID, strTxNotes, lngPatApptID, strUserName)
        objTx = Nothing

        'If the transaction is associated with a claim, update Billing Responsibility
        If lngELID > 0 Then

            objBR.UpdatePatientAmount(lngELID, strUserName)
            objBR = Nothing
        End If



    End Function

    Public Function Fetch(ByVal lngPatientID As Long, Optional ByVal lngProviderID As Long = -1) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/06/2001
        'Author: Eric Pena
        'Description:  Returns rows from the tblNonPostable table utilizing a stored procedure
        'Parameters: PK for row in tblNonPostable that will be fetched.
        'Returns: ADORst
        '--------------------------------------------------------------------

        Dim objPatTx As New BillingDB.CPatientTXDB


        Fetch = objPatTx.Fetch(lngPatientID, lngProviderID)

        objPatTx = Nothing
 
    End Function

    Public Function FetchByID(ByVal lngTxID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/29/2002
        'Author: Dave Richkun
        'Description:  Returns detailed patient transaction information
        '              associated with a specific transaction
        'Parameters: lngTxID - ID of patient transaction record to which
        '               transactions are associated
        'Returns: Recordset of patient payment details
        '--------------------------------------------------------------------

        Dim objPatTx As New BillingDB.CPatientTXDB


        FetchByID = objPatTx.FetchByID(lngTxID)
        objPatTx = Nothing



    End Function


    Public Function FetchByELID(ByVal lngELID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/23/2002
        'Author: Dave Richkun
        'Description:  Returns detailed patient transaction information
        '              associated with an encounter record
        'Parameters: lngELID - ID of encounter record to which transactions are associated
        'Returns: Recordset of patient payment details
        '--------------------------------------------------------------------

        Dim objPatTx As New BillingDB.CPatientTXDB

        FetchByELID = objPatTx.FetchByELID(lngELID)
        objPatTx = Nothing


    End Function


    Public Function FetchByPatientUser(ByVal lngPatientID As Long, ByVal lngUserID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/26/2002
        'Author: Dave Richkun
        'Description:  Returns patient transaction information associated with a
        '              patient, considering the user executing the method.  Results
        '              may vary whether the user is a provider or an Office Manager.
        'Parameters: lngPatientID - ID of patient to which transactions are associated
        '            lngUserID - ID of user executing this method
        'Returns: Recordset of patient transactions
        '--------------------------------------------------------------------
        Dim objPatTx As New BillingDB.CPatientTXDB


        FetchByPatientUser = objPatTx.FetchByPatientUser(lngPatientID, lngUserID)
        objPatTx = Nothing


    End Function


    Public Function FetchByPatientProvUser(ByVal lngPatientID As Long, ByVal lngUserID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/28/2002
        'Author: Dave Richkun
        'Description:  Returns patient transaction information associated with a
        '              patient ordered by Providers serviced by the user.
        'Parameters: lngPatientID - ID of patient to which transactions are associated
        '            lngUserID - ID of user executing this method
        'Returns: Recordset of patient transactions
        '--------------------------------------------------------------------
        Dim objPatTx As New BillingDB.CPatientTXDB


        FetchByPatientProvUser = objPatTx.FetchByPatientProvUser(lngPatientID, lngUserID)
        objPatTx = Nothing



    End Function


    Public Sub UpdateDeleteForELID(ByVal lngELID As Long, ByVal varTxIDs As Object)
        '--------------------------------------------------------------------
        'Date: 07/25/2002
        'Author: Dave Richkun
        'Description:  Physi y deletes transaction rows where the TxID is included
        '              in the varTxIDs array.  Any transactions associated with the
        '              claim that are not included in the array, have the ELID foreign
        '              key updated to NULL.  This method is  ed when a user is electing
        '              to delete a claim.  The user is prompted to identify which
        '              patient transactions are to be deleted, and which transactions they
        '              would like to keep.
        'Parameters: lngELID - ID of encounter record to which transactions are associated
        '             varTxIDs - A single dimensional Object array containing the IDs of
        '               those patient transactions they user wishes to delete.  If the user
        '               does not want to delete any transactions this method still expects
        '               the array to hold one element with a value of zero.
        'Returns: Null
        '--------------------------------------------------------------------

        Dim objPatTx As New BillingDB.CPatientTXDB
        Dim intCTR As Integer
        Dim strTxIDs As String

        'Build a string of Tx IDs so that the list of IDs will syntacti y fit within
        'an SQL 'IN' clause
        For intCTR = 0 To UBound(varTxIDs)
            strTxIDs = strTxIDs & varTxIDs(intCTR)
            If intCTR < UBound(varTxIDs) Then
                'Append a comma
                strTxIDs = strTxIDs & ","
            End If
        Next intCTR

        If strTxIDs > "" Then
            objPatTx.UpdateDeleteForELID(lngELID, strTxIDs)
            objPatTx = Nothing
        End If



    End Sub

    Public Sub Update(ByVal lngTxID As Long, ByVal lngELID As Long, _
                    ByVal lngPatientID As Long, ByVal lngProviderID As Long, _
                    ByVal lngRPID As Long, ByVal lngTxTypeID As Long, _
                    ByVal dtRcvDate As Date, ByVal dblAmount As Double, _
                    ByVal strCheckNum As String, ByVal dtcheckDate As Date, _
                    ByVal strTxNotes As String, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 07/29/2002
        'Author: Dave Richkun
        'Description:  Updates a row in tblPatientTx table utilizing a stored procedure.
        'Parameters:
        '
        '   *** TO DO: Define each parameter once procedure is finalized ****
        '
        'Returns: Null
        '--------------------------------------------------------------------

        Dim objPatTx As New BillingDB.CPatientTXDB
        Dim rst As New ADODB.Recordset
        Dim lngID As Long
        Dim blnUpdatePostDateYN As Boolean
        Dim objTx As New BillingDB.CPatientTXDB
        Dim objBR As New BillingBZ.CBRBz

        rst = objPatTx.FetchByID(lngTxID)
        objPatTx = Nothing

        'if the transaction amount was not changed then donot update the posting date
        If (rst.Fields("fldAmount").Value <> dblAmount) Then
            blnUpdatePostDateYN = True
        Else
            blnUpdatePostDateYN = False
        End If

        objTx.Update(lngTxID, lngELID, lngPatientID, lngProviderID, lngRPID, _
                lngTxTypeID, dtRcvDate, dblAmount, strCheckNum, dtcheckDate, _
                strTxNotes, blnUpdatePostDateYN, strUserName)
        objTx = Nothing

        'If the transaction is associated with a claim, update Billing Responsibility
        If lngELID > 0 Then
            objBR.UpdatePatientAmount(lngELID, strUserName)
            objBR = Nothing
        End If



    End Sub


    Public Sub Delete(ByVal lngTxID As Long, ByVal lngELID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 07/29/2002
        'Author: Dave Richkun
        'Description:  Physi y deletes a row in tblPatientTx table utilizing
        '              a stored procedure.
        'Parameters: lngTxID - ID of transaction to be deleted
        '            lngELID - ID of encounter to which the transaction is associated
        'Returns: Null
        '--------------------------------------------------------------------

        Dim objTx As New BillingDB.CPatientTXDB
        Dim objBR As New BillingBZ.CBRBz


        objTx.Delete(lngTxID, strUserName)
        objTx = Nothing

        'If the transaction is associated with a claim, update Billing Responsibility
        If lngELID > 0 Then

            objBR.UpdatePatientAmount(lngELID, strUserName)
            objBR = Nothing
        End If



    End Sub


    Public Sub UpdateSelfPayCharge(ByVal lngELID As Long, ByVal dblAmount As Double, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 08/06/2002
        'Author: Dave Richkun
        'Description:  Updates the patient charge record associated with a self-pay claim.
        'Parameters: lngELID - ID of self-pay encounter
        '            dblAmount - The amount of the updated charge
        '            strUserName - The name of the user performing the update
        'Returns: Null
        '--------------------------------------------------------------------

        Dim objTx As New BillingDB.CPatientTXDB


        objTx.UpdateSelfPayCharge(lngELID, dblAmount, strUserName)
        objTx = Nothing


    End Sub



    Public Sub Resubmit(ByVal lngOrigELID As Long, ByVal lngNewELID As Long)
        '--------------------------------------------------------------------
        'Date: 08/09/2002
        'Author: Dave Richkun
        'Description:  Re-assigns the Encounter Log ID foreign key from one set
        '              of patient transactions to a new Encounter Log ID.  This
        '              method is used when a claim is resubmitted and a new encounter
        '              is created.  By reassigning the encounter log ID, the original
        '              patient transactions are preserved when assigned to the new
        '              encounter.
        'Parameters:  lngOrigELID - ID of the original encounter - the ID of the encounter
        '               being cancelled
        '             lngNewELID - ID of the resubmitted encounter - the ID of the new
        '               encounter that supercedes all previous encounters.
        'Returns: Null
        '--------------------------------------------------------------------

        Dim objTx As New BillingDB.CPatientTXDB


        objTx.Resubmit(lngOrigELID, lngNewELID)
        objTx = Nothing



    End Sub


    Public Sub EditFromSchedule(ByVal lngPatientTxID As Long, ByVal lngPatApptID As Long, _
            ByVal lngApptID As Long, ByVal lngApptStatusID As Long, ByVal dtApptDateTime As Date, _
            ByVal intDuration As Integer, ByVal strCPTCode As String, ByVal strNote As String, _
            ByVal lngClinicID As Long, ByVal strNoShowFlag As String, ByVal strCancelExplain As String, _
            ByVal lngPatientID As Long, ByVal lngProviderID As Long, ByVal lngRPID As Long, _
            ByVal strTxTypeCode As String, ByVal lngTxTypeID As Long, ByVal dtPostDate As Date, _
            ByVal dtRcvDate As Date, ByVal dblAmount As Double, ByVal strCheckNum As String, _
            ByVal dtcheckDate As Date, ByVal strCCTxID As String, ByVal strTxNotes As String, ByVal blnRecurYN As Boolean, _
            ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 08/23/2002
        'Author: Dave Richkun
        'Description:  Updates the status of a patient appointment and inserts a transaction
        '              row into tblPatientTx table utilizing a stored procedure.
        '              This procedure is  ed when the transaction originates from the
        '              Schedule or Check-In page.  The encounter has not yet been created when
        '              the transaction originates from the these pages.  The transaction will
        '              be linked with the encounter via the PatApptID once it is created.
        'Parameters:
        '
        '   *** TO DO: Define each parameter once procedure is finalized ****
        '
        'Returns: ID of the row inserted.
        '--------------------------------------------------------------------

        '  Dim objPatAppt As New ApptBZ.CPatApptBZ
        ' Dim objAppt As New ApptDB.CApptDB
        Dim rst As New ADODB.Recordset
        Dim rstTemp As New ADODB.Recordset
        Dim lngNewPatApptID As Long
        Dim lngNewApptID As Long
        Dim intCTR As Integer
        Dim lngTempPatientID As Long

        Const APPT_TYPE_PATIENT = 1




        'If the appointment is a recurring appointment, we must make an instance of it
        If blnRecurYN = True Then
            'Insert a new patient appointment record for each patient in the appointment.
            rstTemp = FetchPatientApptByID(lngApptID)

            'Create a new one-time patient appointment to replace this instance.
            lngNewApptID = InsertSingle(lngProviderID, lngClinicID, APPT_TYPE_PATIENT, _
                dtApptDateTime, DateAdd("n", intDuration, dtApptDateTime), intDuration, _
                strCPTCode, "", strNote, strUserName)

            lngTempPatientID = -1
            'Ensure only one patient record is inserted; multiple rows will be returned if the
            'patient has more than one plan.
            For intCTR = 1 To rstTemp.RecordCount
                If lngTempPatientID <> rstTemp.Fields("fldPatientID").Value Then
                    '     lngNewPatApptID = objPatAppt.Insert(lngNewApptID, rstTemp.Fields("fldPatientID").Value)
                    '************************************************************************************************************************************

                    If rstTemp.Fields("fldPatientID").Value = lngPatientID Then
                        lngPatApptID = lngNewPatApptID
                    End If

                    lngTempPatientID = rstTemp.Fields("fldPatientID").Value
                End If

                rstTemp.MoveNext()
            Next
            rstTemp = Nothing

            'Insert a row into tblRecurApptExc so the recurring appointment and the new one-time
            'appointment do not appear as conflicts.
            InsertRecurApptExc(lngApptID, DateValue(dtApptDateTime), strUserName)
        Else
            'Update the appointment - the user may have changed appointment details at the
            'same time the transaction was entered.
            Update(lngApptID, lngProviderID, lngClinicID, APPT_TYPE_PATIENT, _
                    dtApptDateTime, DateAdd("n", intDuration, dtApptDateTime), intDuration, _
                    strCPTCode, "", strNote, strUserName)
        End If

        'objAppt = Nothing

        'Update the appointment status





        '************************************************************************************************************************************

        '************************************************************************************************************************************



        '************************************************************************************************************************************


        '  objPatAppt.ChangeStatus(lngPatApptID, lngApptStatusID, strNoShowFlag, 0, strCancelExplain, strUserName)

        '  objPatAppt = Nothing

        'We delete the old transaction because the user may change a payment into a
        'charge i.e. change payment to No-Show and vice-versa.  We must ensure that
        'numbers remain accurate.
        If lngPatientTxID > 0 Then
            Delete(lngPatientTxID, -1, strUserName)
        End If

        'Insert/Update the patient transaction
        If dblAmount > 0 Or Not String.IsNullOrEmpty(strTxNotes) Then
            Insert(-1, lngPatientID, lngProviderID, lngRPID, strTxTypeCode, lngTxTypeID, _
                  Date.Now, dtRcvDate, dblAmount, strCheckNum, CDate("01/01/1900 12:00:00"), strCCTxID, strTxNotes, _
                  lngPatApptID, strUserName)
        End If



    End Sub


    Public Function FetchByPatApptID(ByVal lngPatApptID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/27/2002
        'Author: Dave Richkun
        'Description:  Returns patient transaction information associated with a
        '              patient appointment.
        'Parameters: lngPatApptID - ID of patient appointment to which transactions are associated
        'Returns: Recordset of patient transactions
        '--------------------------------------------------------------------

        Dim objPatTx As New BillingDB.CPatientTXDB


        FetchByPatApptID = objPatTx.FetchByPatApptID(lngPatApptID)
        objPatTx = Nothing



    End Function


    Public Function ResetPatientAppt(ByVal lngPatApptID As Long, ByVal aryTxs As Object, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 08/27/2002
        'Author: Dave Richkun
        'Description:  Resets a patient appointment to a status of 'Attended' and
        '              deletes any related Cancellation and No-Show patient transactions
        '              the user selects.
        'Parameters:  lngPatApptID - ID of the patient appointment
        '             aryPatTxs - Single dimensional array of patient transaction IDs
        '               selected by the user for deletion.
        '             strUserName - User name of the user initiating the method
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '--------------------------------------------------------------------

        'Dim objAppt As New ApptDB.CPatApptDB
        Dim intCTR As Integer


        Reset(lngPatApptID, strUserName)
        '  objAppt = Nothing

        'Delete patient transactions
        If IsArray(aryTxs) Then
            For intCTR = 0 To UBound(aryTxs)
                If aryTxs(intCTR) > 0 Then
                    Delete(aryTxs(intCTR), -1, strUserName)
                End If
            Next
        End If
        ' afldskfjlaskfj

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
    Public Function FetchPatientApptByID(ByVal lngApptID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/12/2002
        'Author: Dave Richkun
        'Description:   Retrieves a recordset of detailed patient and plan information
        '               associated with a scheduled patient appointment
        'Parameters:    lngApptID - ID of the appointment
        'Returns:   Recordset of patient and plan information associated with appointment
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '--------------------------------------------------------------------
        Dim cnn As ADODB.Connection
        Dim cmd As ADODB.Command
        Dim rst As ADODB.Recordset

        Dim SQLErrorNum As Long

        '

        cmd = New ADODB.Command
        cnn = New ADODB.Connection

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelApptPatientsPlans"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ApptID", adInteger, adParamInput, , lngApptID))
        End With

        'Execute the stored procedure
        rst = New ADODB.Recordset
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchPatientApptByID = rst

        'Disconnect the recordset and clean house
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing

        '        'Signal successful completion
        '        

        '        Exit Function

        '
        '        'Signal incompletion and raise the error to the ing environment.
        '         cmdSQL.Execute()
        '        cnn = Nothing
        '        cmd = Nothing
        '        rst = Nothing

        '       'err.raise(Err.Number, Err.Source, Err.Description)
    End Function
    Public Function InsertSingle(ByVal lngProviderID As Long, ByVal lngClinicID As Object, _
                       ByVal lngCategoryID As Long, ByVal dteStartDateTime As Date, _
                       ByVal dteEndDateTime As Date, ByVal lngDuration As Long, _
                       ByVal strCPTCode As String, ByVal strDescription As String, _
                       ByVal strNote As String, ByVal strUserName As String) As Long
        '--------------------------------------------------------------------
        'Date: 07/13/2002
        'Author: Dave Richkun
        'Description:  Inserts an appointment record into tblAppointment via a stored procedure
        'Parameters:  lngProviderID - ID of the provider whose schedule the appointment will appear
        '             lngClinicID - ID of the place of service where the appointment is scheduled
        '             lngCategoryID - ID of the category identifying if appointment is 'Patient' or 'Block'
        '             dteStartDateTime - Start Date and Time of the appointment
        '             dteEndDateTime - End Date and Time of the appointment
        '             lngDuration - Appointment length in minutes
        '             strCPTCode - The CPTCode associated with a patient appointment
        '             strDescription - Short description of the appointment
        '             strNote - A longer description pertaining to the appointment
        '             strUserName - Name of user responsible for adding appointment
        'Returns: ID of appointment ID on success, -1 on failure
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '--------------------------------------------------------------------
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        cnn = New ADODB.Connection
        cmd = New ADODB.Command

        'Create the parameter objects
        With cmd
            .CommandText = "uspInsAppointment"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@ClinicID", adInteger, adParamInput, , IIf(lngClinicID = 0, vbNull, lngClinicID)))
            .Parameters.Append(.CreateParameter("@CategoryID", adInteger, adParamInput, , lngCategoryID))
            .Parameters.Append(.CreateParameter("@StartDateTime", adDBTimeStamp, adParamInput, , dteStartDateTime))
            .Parameters.Append(.CreateParameter("@EndDateTime", adDBTimeStamp, adParamInput, , dteEndDateTime))
            .Parameters.Append(.CreateParameter("@Duration", adInteger, adParamInput, , lngDuration))
            .Parameters.Append(.CreateParameter("@CPTCode", adVarChar, adParamInput, 10, IIf(strCPTCode = "", vbNull, strCPTCode)))
            .Parameters.Append(.CreateParameter("@Description", adVarChar, adParamInput, 30, strDescription))
            .Parameters.Append(.CreateParameter("@Note", adVarChar, adParamInput, 256, strNote))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn
        cmd.Execute() '()
        cnn = Nothing



        InsertSingle = cmd.Parameters("@ID").Value

        cnn = Nothing
        cmd = Nothing



    End Function
    Public Sub InsertRecurApptExc(ByVal lngApptID As Long, ByVal dtApptDate As Date, _
                              ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 10/09/2001
        'Author: Dave Richkun
        'Description:  Inserts a record into tblRecurApptExc using a stored procedure                           '
        'Parameters:  lngApptID - ID of the recurring appointment where an exception is being made
        '             dtApptDate - The date of the appointment exception
        '             strUserName - Name of user recording the exception
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '--------------------------------------------------------------------
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection


        cnn = New ADODB.Connection
        cmd = New ADODB.Command

        'Create the parameter objects
        With cmd
            .CommandText = "uspInsRecurApptExc"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ApptID", adInteger, adParamInput, , lngApptID))
            .Parameters.Append(.CreateParameter("@ApptDate", adDBTimeStamp, adParamInput, , DateValue(dtApptDate)))
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn
        cmd.Execute() '()
        cnn = Nothing


        cnn = Nothing
        cmd = Nothing



    End Sub


    Public Function Update(ByVal lngApptID As Long, ByVal lngProviderID As Long, _
        ByVal lngClinicID As Long, ByVal lngCategoryID As Long, ByVal dteStartDateTime As Date, _
        ByVal dteEndDateTime As Date, ByVal lngDuration As Long, ByVal strCPTCode As String, _
        ByVal strDescription As String, ByVal strNote As String, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 08/30/2000
        'Author: Dave Richkun
        'Description:  Updates an appointment record in tblAppointment via a stored procedure
        'Parameters:
        'lngProviderID lngApptID - System ID of the provide for whom the appointment is for.
        '              lngProviderID - ID of the provider whose schedule the appointment will appear
        '              lngClinicID - ID of the place of service where the appointment is scheduled
        '              lngCategoryID - ID of the category identifying if appointment is 'Patient' or 'Block'
        '              dteStartDateTime - Start Date and Time of the appointment
        '              dteEndDateTime - End Date and Time of the appointment
        '              lngDuration - Appointment length in minutes
        '              strCPTCode - The CPTCode associated with a patient appointment
        '              strDescription - Short description of the appointment
        '              strNote - A longer description pertaining to the appointment
        '              strUserName - Name of user responsible for adding appointment
        'Returns:  Nothing                                                  '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '--------------------------------------------------------------------
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        cnn = New ADODB.Connection
        cmd = New ADODB.Command

        'Create the parameter objects
        With cmd
            .CommandText = "uspUpdAppointment"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ApptID", adInteger, adParamInput, , lngApptID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@ClinicID", adInteger, adParamInput, , IIf(lngClinicID = 0, DBNull.Value, lngClinicID)))
            .Parameters.Append(.CreateParameter("@CategoryID", adInteger, adParamInput, , lngCategoryID))
            .Parameters.Append(.CreateParameter("@StartDateTime", adDBTimeStamp, adParamInput, , dteStartDateTime))
            .Parameters.Append(.CreateParameter("@EndDateTime", adDBTimeStamp, adParamInput, , dteEndDateTime))
            .Parameters.Append(.CreateParameter("@Duration", adInteger, adParamInput, , lngDuration))
            .Parameters.Append(.CreateParameter("@CPTCode", adVarChar, adParamInput, 10, IIf(strCPTCode = "", DBNull.Value, strCPTCode)))
            .Parameters.Append(.CreateParameter("@Description", adVarChar, adParamInput, 30, strDescription))
            .Parameters.Append(.CreateParameter("@Note", adVarChar, adParamInput, 256, strNote))
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn
        cmd.Execute() '()
        cnn = Nothing



        cnn = Nothing
        cmd = Nothing


    End Function



End Class
