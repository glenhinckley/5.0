
'--------------------------------------------------------------------
'Class Name: CMisdirectPmtBz
'Date: 06/11/2001
'Author: Eric Pena
'Denis J Finkel
'Description:  MTS business object designed to   methods associated
'              with the CMisdirectPmtDB class.
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
Imports Psyquel.BusinessRules.CoreLibraryIII.dbStuff

Public Class CMisdirectPmtBz

    Private _ConnectionString As String = String.Empty




    Dim strErrMessage As String = String.Empty
    Private Const CLASS_NAME As String = "CMisdirectPmtBz"
    Private Const TABLE_NAME As String = "tblMisdirectedPayment"
    Private Const CONST_POSTMP As Decimal = 120



    Private _DB As New db
    Private _MD As New ModCommon

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property


    Public Function Insert(ByVal lngBRID As Long, ByVal strCheckNum As String, ByVal dteCheckDate As Date, ByVal dteClearDate As Date, ByVal curCheckAmount As Decimal, ByVal curPostAmount As Decimal, _
                            ByVal curAmtDisallow As Decimal, ByVal curAmtPatient As Decimal, ByVal blnMultiCheck As Boolean, ByVal strAddress1 As String, _
                            ByVal strAddress2 As String, ByVal strCity As String, ByVal strState As String, ByVal strZip As String, ByVal strContactName As String, _
                            ByVal strContactPhone As String, ByVal strContactExt As String, ByVal strNotes As String, ByVal strUsername As String) As Long
        '--------------------------------------------------------------------
        'Date: 06/11/2001
        'Author: Eric Pena
        'Description:  Inserts a row into the tblMisdirectedPayment table utilizing a stored procedure.
        'Parameters: Each parameter identifies the column value that will be  inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------
        Dim objMP As New PostingDB.CMisdirectPmtDB
        Dim objBR As New BillingBz.CBRBz

        objMP.ConnectionString = _ConnectionString
        objBR.ConnectionString = _ConnectionString




        If Not VerifyData(lngBRID, curCheckAmount, strErrMessage) Then Exit Function

        Insert = objMP.Insert(lngBRID, Trim(strCheckNum), dteCheckDate, dteClearDate, curCheckAmount, curPostAmount, curAmtDisallow, curAmtPatient, blnMultiCheck, Trim(strAddress1), Trim(strAddress2), Trim(strCity), _
                             Trim(strState), _MD.NumbersOnly(strZip), Trim(strContactName), _MD.NumbersOnly(strContactPhone), _MD.NumbersOnly(strContactExt), Trim(strNotes), strUsername)

        objMP = Nothing


    End Function

    Public Sub Update(ByVal lngMisPmtID As Long, ByVal strCheckNum As String, ByVal dteCheckDate As Date, ByVal dteClearDate As Date, ByVal curCheckAmount As Decimal, ByVal curPostAmount As Decimal, _
                            ByVal curAmtDisallow As Decimal, ByVal curAmtPatient As Decimal, ByVal blnMultiCheck As Boolean, ByVal strAddress1 As String, _
                            ByVal strAddress2 As String, ByVal strCity As String, ByVal strState As String, ByVal strZip As String, ByVal strContactName As String, _
                            ByVal strContactPhone As String, ByVal strContactExt As String, ByVal strNotes As String, ByVal strUsername As String)
        '--------------------------------------------------------------------
        'Date: 06/11/2001
        'Author: Eric Pena
        'Description:  Updates a row in the tblMisdirectedPayment table utilizing a stored procedure.
        'Parameters: Each parameter identifies the column value that will be inserted.
        'Returns: Null
        '--------------------------------------------------------------------
        Dim objMP As New PostingDB.CMisdirectPmtDB

        Dim strErrMessage As String
        objMP.ConnectionString = _ConnectionString

        If Not VerifyData(1, curCheckAmount, strErrMessage) Then Exit Sub


        If objMP.isProcessed(lngMisPmtID) Then
            strErrMessage = "This entry has already been processed.  Cannot update."
            Exit Sub
        End If

        objMP.Update(lngMisPmtID, Trim(strCheckNum), dteCheckDate, dteClearDate, curCheckAmount, curPostAmount, curAmtDisallow, curAmtPatient, blnMultiCheck, Trim(strAddress1), Trim(strAddress2), Trim(strCity), _
                             Trim(strState), _MD.NumbersOnly(strZip), Trim(strContactName), _MD.NumbersOnly(strContactPhone), _MD.NumbersOnly(strContactExt), Trim(strNotes), strUsername)


    End Sub

    Public Sub Deleted(ByVal lngMisPmtID As Long, ByVal blnIsDeleted As Boolean, ByVal strDeletedBy As String)
        '--------------------------------------------------------------------
        'Date: 06/11/2001
        'Author: Eric Pena
        'Description:  Physi y deletes a row in the tblMisdirectedPayment table if it has not yet been processed.
        'Parameters: lngID - ID of the row in the table whose value will be updated.
        '            strUserName - Login name of the user responsible for marking the row as deleted.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        Dim objMP As New PostingDB.CMisdirectPmtDB

        objMP.ConnectionString = _ConnectionString
        Dim strError As String


        strError = ""


        If objMP.isProcessed(lngMisPmtID) Then
            strError = "This entry has already been processed.  Cannot delete."
            Exit Sub
        End If

        objMP.Deleted(lngMisPmtID, blnIsDeleted, strDeletedBy)


    End Sub

    Private Function VerifyData(ByVal lngBRID As Long, ByVal curAmount As Decimal, ByRef strErrMessage As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 06/11/2001
        'Author: Eric Pena
        'Description:  Verifies all required data has been provided by the user.
        'Parameters:  The values to be checked.
        'Returns: Boolean value identifying if all criteria has been satisfied.
        '--------------------------------------------------------------------
        If lngBRID < 1 Then
            strErrMessage = "A Payer is needed."
            VerifyData = False
            Exit Function
        End If

        If curAmount <= 0 Then
            strErrMessage = "A positive check amount is needed."
            VerifyData = False
            Exit Function
        End If

        'If we get here, all is well...
        VerifyData = True

    End Function

    Public Function FetchByUser(ByVal lngUserID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 06/11/2001
        'Author: Eric Pena
        'Description:  Returns rows from the tblMisdirectedPayment table utilizing a stored procedure
        'Parameters: lngUserID - user for which records will be returned
        'Returns: ADORst
        '--------------------------------------------------------------------

        Dim objMP As New PostingDB.CMisdirectPmtDB
        objMP.ConnectionString = _ConnectionString

        FetchByUser = objMP.FetchByUser(lngUserID)

        'Signal successful completion

    End Function

    Public Function FetchByEL(ByVal lngELID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 06/11/2001
        'Author: Eric Pena
        'Description:  Returns rows from the tblMisdirectedPayment table utilizing a stored procedure
        'Parameters: lngELID - PK in tblEncounterLog for which records will be returned
        'Returns: ADORst
        '--------------------------------------------------------------------

        Dim objMP As New PostingDB.CMisdirectPmtDB
        objMP.ConnectionString = _ConnectionString
        FetchByEL = objMP.FetchByEL(lngELID)


    End Function

    Public Function FetchByBR(ByVal lngBRID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 06/11/2001
        'Author: Eric Pena
        'Description:  Returns rows from the tblMisdirectedPayment table utilizing a stored procedure
        'Parameters: lngBRID - PK in tblBillingResponsibility for which records will be returned
        'Returns: ADORst
        '--------------------------------------------------------------------

        Dim objMP As New PostingDB.CMisdirectPmtDB
        objMP.ConnectionString = _ConnectionString
        FetchByBR = objMP.FetchByBR(lngBRID)


    End Function

    Public Function FetchByID(ByVal lngMPID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 06/11/2001
        'Author: Eric Pena
        'Description:  Returns rows from the tblMisdirectedPayment table utilizing a stored procedure
        'Parameters: lngMPID - PK in tblMisdirectedPayment for which records will be returned
        'Returns: ADORst
        '--------------------------------------------------------------------

        Dim objMP As New PostingDB.CMisdirectPmtDB
        objMP.ConnectionString = _ConnectionString

        FetchByID = objMP.FetchByID(lngMPID)


    End Function

    Public Sub SetAction(ByVal lngMisPmtID As Long, ByVal strActionType As String, ByVal strUsername As String)
        '--------------------------------------------------------------------
        'Date: 06/11/2001
        'Author: Eric Pena
        'Description:  Updates a row in the tblMisdirectedPayment table utilizing a stored procedure.
        'Parameters: Each parameter identifies the column value that will be inserted.
        'Returns: Null
        '--------------------------------------------------------------------
        Dim objMP As New PostingDB.CMisdirectPmtDB
        Dim strErrMessage As String
        objMP.ConnectionString = _ConnectionString

        If objMP.isProcessed(lngMisPmtID) Then
            strErrMessage = "This entry has already been processed.  Cannot update."
            Exit Sub
        End If

        objMP.SetAction(lngMisPmtID, strActionType, strUsername)
        objMP = Nothing


        Exit Sub


    End Sub

    Private Sub SetProcessed(ByVal lngMisPmtID As Long, ByVal strUsername As String)
        '--------------------------------------------------------------------
        'Date: 06/11/2001
        'Author: Eric Pena
        'Description:  Updates a row in the tblMisdirectedPayment table utilizing a stored procedure.
        'Parameters: Each parameter identifies the column value that will be inserted.
        'Returns: Null
        '--------------------------------------------------------------------
        Dim objMP As New PostingDB.CMisdirectPmtDB
        Dim strErrMessage As String
        objMP.ConnectionString = _ConnectionString


        If objMP.isProcessed(lngMisPmtID) Then
            strErrMessage = "This entry has already been processed."
            Exit Sub
        End If

        objMP.SetProcessed(lngMisPmtID, strUsername)

        objMP = Nothing

    End Sub

    Public Function FetchForProcessing() As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 06/11/2001
        'Author: Eric Pena
        'Description:  Returns rows ready for processing from the tblMisdirectedPayment table utilizing a stored procedure
        'Parameters:
        'Returns: ADORst
        '--------------------------------------------------------------------
        Dim objMP As New PostingDB.CMisdirectPmtDB

        objMP.ConnectionString = _ConnectionString
        FetchForProcessing = objMP.FetchForProcessing()


        objMP = Nothing

    End Function

    Public Function FetchForAutoAction() As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 06/11/2001
        'Author: Eric Pena
        'Description:  Returns rows ready for processing from the tblMisdirectedPayment table utilizing a stored procedure
        'Parameters:
        'Returns: ADORst
        '--------------------------------------------------------------------
        Dim objMP As New PostingDB.CMisdirectPmtDB
        objMP.ConnectionString = _ConnectionString


        FetchForAutoAction = objMP.FetchForAutoAction()


    End Function
    Public Sub Process(ByVal lngMisPmtID As Long, ByVal strUsername As String)
        '--------------------------------------------------------------------
        'Date: 06/11/2001
        'Author: Eric Pena
        'Description:  Updates a row in the tblMisdirectedPayment table utilizing a stored procedure.
        'Parameters: Each parameter identifies the column value that will be inserted.
        'Returns: Null
        '--------------------------------------------------------------------
        Dim objMP As New PostingDB.CMisdirectPmtDB
        Dim objBR As New BillingBZ.CBRBz
        Dim objTx As New PostingBz.CTxBz
        Dim objCheck As New PostingBz.CCheckBz
        Dim rstMP As New ADODB.Recordset
        Dim rstBR As New ADODB.Recordset
        Dim rstCheck As New ADODB.Recordset
        Dim lngNextBRID As Long
        Dim lngNextRPID As Long
        Dim lngNextPlanID As Long
        Dim lngPlanID As Long
        Dim lngRPID As Long
        Dim curAmtToMove As Decimal
        Dim curBalance As Decimal
        Dim curTemp As Long
        Dim lngCheckID As Long
        Dim lngNextCPCID As Long

        Dim strErrMessage As String

        objMP.ConnectionString = _ConnectionString
        objBR.ConnectionString = _ConnectionString
        objTx.ConnectionString = _ConnectionString
        objCheck.ConnectionString = _ConnectionString



        rstMP = objMP.FetchByID(lngMisPmtID)






        If rstMP.Fields("fldprocessedyn").Value = "Y" Then
            Exit Sub 'rstMP!fldprocessedyn
        End If


        rstBR = objBR.Fetch(rstMP.Fields("fldEncounterLogID").Value)
        objBR = Nothing

        lngNextBRID = rstBR.Fields("fldBRID").Value 'rstBR!fldBRID
        lngNextRPID = rstBR.Fields("fldRPID").Value 'rstBR!fldRPID
        lngNextPlanID = -1
        lngNextCPCID = -1

        rstBR.Find("fldBRID = " & rstMP.Fields("fldBRID").Value)
        If rstBR.EOF Then
            strErrMessage = "Error processing check!"
            Exit Sub
        End If

        lngRPID = rstBR.Fields("fldRPID").Value
        lngPlanID = rstBR.Fields("fldPlanID").Value

        rstBR.MoveNext()
        If Not rstBR.EOF Then
            lngNextBRID = Convert.ToDouble(rstBR.Fields("fldBird").Value)   'lngNextBRID = rstBR!fldBRID @note:updated
            lngNextRPID = Convert.ToDouble(rstBR.Fields("fldRPID").Value)   'rstBR!fldRPID @note:updated
            lngNextPlanID = Convert.ToDouble(rstBR.Fields("fldPlanID").Value) 'rstBR!fldPlanID @note:updated
            lngNextCPCID = Convert.ToDouble(rstBR.Fields("fldcPcID").Value) 'rstBR!fldcPcID @note:updated
        End If
        rstBR.MovePrevious()

        rstCheck = objCheck.FetchByCriteria(rstMP.Fields("fldInsuranceID").Value, Trim(rstMP.Fields("fldCheckNum").Value), rstMP.Fields("fldCheckDate").Value, "", -1, -1, False)
        If rstCheck.EOF Then
            'insert new check
            If rstMP.Fields("fldMultiCheckYN").Value = "Y" Then
                curTemp = rstMP.Fields("fldCheckAmount").Value
            Else
                curTemp = rstMP.Fields("fldAmount").Value
            End If
            lngCheckID = objCheck.Insert(rstMP.Fields("fldInsuranceID").Value, rstMP.Fields("fldCheckNum").Value, "000", rstMP.Fields("fldCheckDate").Value, rstMP.Fields("fldReceiveDate").Value, curTemp, rstMP.Fields("fldAddedBy").Value, rstBR.Fields("fldProviderID").Value, -1)
        Else
            'use existing check
            lngCheckID = rstCheck.Fields("fldCheckID").Value

            'make sure there's enuf $$ on the check
            If Convert.ToDecimal(rstCheck.Fields("fldTotalAmount").Value - rstCheck.Fields("fldAmtApplied").Value) < rstMP.Fields("fldAmount").Value Then
                '************************  strErrMessage = "Cannot post MP.  Check #" & rstCheck.Fields("fldCheckNum").Value & " is " & Format(Abs(rstMP.Fields("fldAmount").Value - rstCheck.Fields("fldTotalAmount").Value - rstCheck.Fields("fldAmtApplied").Value), "Decimal") & " short."
                Exit Sub
            End If
        End If
        rstCheck = Nothing
        objCheck = Nothing


        curTemp = Convert.ToDecimal(rstMP.Fields("fldAmount").Value + rstMP.Fields("fldAmtDisallow").Value)

        If curTemp <= rstBR.Fields("fldBalance").Value Then
            'can just post
            curBalance = rstBR.Fields("fldBalance").Value - curTemp


 






            ' IF GETS USED AGAIN FIX NOW WITH DTEDATERECIVED  ************************************************************************************************************************************************
            objTx.Insert(rstMP.Fields("fldBRID").Value, rstBR.Fields("fldInsuranceID").Value, rstBR.Fields("fldcPcID").Value, rstBR.Fields("fldPlanID").Value, rstBR.Fields("fldRPID").Value, rstBR.Fields("fldEncounterLogID").Value, rstBR.Fields("fldEncDetailID").Value, rstBR.Fields("fldPatientID").Value, _
                        lngNextRPID, lngNextPlanID, lngNextBRID, lngNextCPCID, _DB.IfNull(rstMP.Fields("fldActionBy").Value, strUsername), CONST_POSTMP, "P", Nothing, rstBR.Fields("fldCPTRecordID").Value, rstMP.Fields("fldAmount").Value, rstMP.Fields("fldAmtDisallow").Value, _
                        curBalance, rstMP.Fields("fldCheckNum").Value, rstMP.Fields("fldCheckDate").Value, lngCheckID, _DB.IfNull(rstMP.Fields("fldNotes").Value, ""), True, True)
        Else
            'not enuf BR.  transfer from others (last payer -> first)
            curBalance = 0
            curAmtToMove = Convert.ToDecimal(curTemp - rstBR.Fields("fldBalance").Value)
            rstBR.Filter = "fldBRID <> " & rstMP.Fields("fldBRID").Value
            If Not rstBR.EOF Then rstBR.MoveLast()

            While Not rstBR.BOF And curAmtToMove > 0
                If rstBR.Fields("fldBalance").Value >= curAmtToMove Then
                    'can move all
                    objTx.TransferResponsibility(rstBR.Fields("fldBRID").Value, rstBR.Fields("fldPlanID").Value, rstBR.Fields("fldRPID").Value, rstMP.Fields("fldBRID").Value, lngRPID, lngPlanID, curAmtToMove, rstBR.Fields("fldEncounterLogID").Value, rstBR.Fields("fldPatientID").Value, -1, -1, strUsername)
                    curAmtToMove = 0
                ElseIf rstBR.Fields("fldBalance").Value > 0 Then
                    'move what can be moved
                    objTx.TransferResponsibility(rstBR.Fields("fldBRID").Value, rstBR.Fields("fldPlanID").Value, rstBR.Fields("fldRPID").Value, rstMP.Fields("fldBRID").Value, lngRPID, lngPlanID, rstBR.Fields("fldBalance").Value, rstBR.Fields("fldEncounterLogID").Value, rstBR.Fields("fldPatientID").Value, -1, -1, strUsername)
                    curAmtToMove = Convert.ToDecimal(curAmtToMove - rstBR.Fields("fldBalance").Value)
                End If
                rstBR.MovePrevious()
            End While

            If curAmtToMove > 0 Then
                'still need to move BR - take it from the patient
                If rstBR.BOF Then
                    If Not rstBR.EOF Then rstBR.MoveNext()
                Else
                    rstBR.MoveFirst()
                End If
                objTx.TransferResponsibility(rstBR.Fields("fldBRID").Value, rstBR.Fields("fldPlanID").Value, rstBR.Fields("fldRPID").Value, rstMP.Fields("fldBRID").Value, lngRPID, lngPlanID, curAmtToMove, rstBR.Fields("fldEncounterLogID").Value, rstBR.Fields("fldPatientID").Value, 1, 246, strUsername)
            End If

            If Not rstBR.BOF Then
                rstBR.MoveFirst()
            End If

            rstBR.Filter = ""
            rstBR.Find("fldBRID = " & rstMP.Fields("fldBRID").Value)

            objTx.Insert(rstMP.Fields("fldBRID").Value, rstBR.Fields("fldInsuranceID").Value, rstBR.Fields("fldcPcID").Value, rstBR.Fields("fldPlanID").Value, rstBR.Fields("fldRPID").Value, rstBR.Fields("fldEncounterLogID").Value, rstBR.Fields("fldEncDetailID").Value, rstBR.Fields("fldPatientID").Value, _
                        lngNextRPID, lngNextPlanID, lngNextBRID, lngNextCPCID, _DB.IfNull(rstMP.Fields("fldActionBy").Value, strUsername), CONST_POSTMP, "P", Nothing, rstBR.Fields("fldCPTRecordID").Value, rstMP.Fields("fldAmount").Value, rstMP.Fields("fldAmtDisallow").Value, _
                        curBalance, rstMP.Fields("fldCheckNum").Value, rstMP.Fields("fldCheckDate").Value, lngCheckID, _DB.IfNull(rstMP.Fields("fldNotes").Value, ""), True, True)
        End If

        objMP.SetProcessed(rstMP.Fields("fldMisPmtID").Value, strUsername)

        objMP = Nothing
        objTx = Nothing
        rstMP = Nothing
        rstBR = Nothing

    End Sub



    Public Function FetchRejections() As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 04/30/2003
        'Author: Dave Richkun
        'Description:  Returns rejected MisDirected payments
        'Parameters:
        'Returns: ADO Recordset
        '--------------------------------------------------------------------

        Dim objMP As New PostingDB.CMisdirectPmtDB
        objMP.ConnectionString = _ConnectionString
        FetchRejections = objMP.FetchRejections()
        objMP = Nothing



    End Function

End Class
