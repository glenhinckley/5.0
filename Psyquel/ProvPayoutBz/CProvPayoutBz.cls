'----------------------------------------
'Class Name: CProvPayoutBz
'Date: 09/29/2000
'Author: Eric Pena
'Denis J Finkel
'Description:  MTS business object designed to   methods associated with the CBillingDB class.
'--------------------------------------------------------------------
'Revision History:
'  R001: 02/12/2003 - Richkun: Added InsertPayoutRows() to separate task from LockPayout() method
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

Public Class CProvPayoutBz

    Private _ConnectionString As String = String.Empty


    Private Const CLASS_NAME As String = "CProvPayoutBz"
    Private Const TABLE_NAME As String = "tblProviderPayout"


    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property





    Public Function FetchPayoutData() As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/29/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblProviderPayout table ordered by patient name utilizing a stored procedure
        'Parameters: lngProviderID - id of the provider to return records for
        'Returns: ADORst
        '--------------------------------------------------------------------
        Dim objPP As New ProvPayoutDB.CProvPayoutDB

        objPP.ConnectionString = _ConnectionString




        FetchPayoutData = objPP.FetchPayoutData()
        objPP = Nothing



    End Function

    Public Sub Delete(ByVal dteStart As Date)
        '--------------------------------------------------------------------
        'Date: 09/29/2000
        'Author: Eric Pena
        'Description:  Physi y deletes entries in tblProviderPayout.  This can only be done if the encounter has not yet been processed.
        'Parameters: lngID - ID of the row in the table whose value will be updated.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        Dim objPP As New ProvPayoutDB.CProvPayoutDB

        objPP.ConnectionString = _ConnectionString

        objPP.Delete(dteStart)
        objPP = Nothing



    End Sub


    Public Function Insert(ByVal lngProviderID As Long, ByVal dteStart As Date, ByVal dteEnd As Date, ByVal curGrossAmt As Decimal) As Long
        '--------------------------------------------------------------------
        'Date: 09/29/2000
        'Author: Eric Pena
        'Description:  Inserts a row into the tblProviderPayout table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------

        Dim objPP As New ProvPayoutDB.CProvPayoutDB
        Dim strErrMessage As String

        objPP.ConnectionString = _ConnectionString

        If Not VerifyData(lngProviderID, dteStart, dteEnd, strErrMessage) Then Exit Function


        Insert = objPP.Insert(lngProviderID, dteStart, dteEnd, curGrossAmt)
        objPP = Nothing


    End Function

    Public Sub SetProcessed(ByVal dteStart As Date, ByVal dteEnd As Date, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 09/29/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblProviderPayout table entered today by a specific user
        'Parameters: strUserName - login name of the user whose Encounter records are being retrieved.
        'Returns: Recordset of matching rows
        '--------------------------------------------------------------------
        Dim objPP As New ProvPayoutDB.CProvPayoutDB
        Dim strErrMessage As String
        objPP.ConnectionString = _ConnectionString

        objPP.SetProcessed(dteStart, dteEnd, strUserName)
        objPP = Nothing

        'Signal successful completion

    End Sub

    Public Function FetchDates(ByVal blnPayoutHistory As Boolean) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/29/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblProviderPayout table ordered by patient name utilizing a stored procedure
        'Parameters: lngProviderID - id of the provider to return records for
        'Returns: ADORst
        '--------------------------------------------------------------------

        Dim objPP As New ProvPayoutDB.CProvPayoutDB
        objPP.ConnectionString = _ConnectionString

        FetchDates = objPP.FetchDates(blnPayoutHistory)
        objPP = Nothing



    End Function

    Public Function FetchProviderByPayout(ByVal dtePayoutStart As Date, ByVal blnPayoutHistory As Boolean) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/29/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblProviderPayout table ordered by provider name utilizing a stored procedure
        'Parameters: lngProviderID - id of the patient  to return records for
        'Returns: ADORst
        '--------------------------------------------------------------------

        Dim objPP As New ProvPayoutDB.CProvPayoutDB
        objPP.ConnectionString = _ConnectionString

        FetchProviderByPayout = objPP.FetchProviderByPayout(dtePayoutStart, blnPayoutHistory)
        objPP = Nothing

    End Function

    Public Function isProcessed(ByVal lngID As Long, Optional ByVal dteStart As Date = Nothing, Optional ByVal dteEnd As Date = Nothing) As Boolean
        '--------------------------------------------------------------------
        'Date: 09/29/2000
        'Author: Eric Pena
        'Description:  Lets the  er know if a given EncounterLog Entry has been processed
        'Parameters: lngID - The ID of the EL Entry
        'Returns: T if the association is processed, F otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objPP As New ProvPayoutDB.CProvPayoutDB
        objPP.ConnectionString = _ConnectionString

        isProcessed = objPP.isProcessed(lngID, dteStart, dteEnd)
        objPP = Nothing



    End Function

    Public Function FetchByID(ByVal lngID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/29/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblProviderPayout table ordered by patient name utilizing a stored procedure
        'Parameters: lngProviderID - id of the provider to return records for
        'Returns: ADORst
        '--------------------------------------------------------------------
        Dim objPP As New ProvPayoutDB.CProvPayoutDB
        objPP.ConnectionString = _ConnectionString

        FetchByID = objPP.FetchByID(lngID)
        objPP = Nothing



    End Function

    Public Function FetchByDate(ByVal dteStart As Date, ByVal dteEnd As Date) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/29/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblProviderPayout table ordered by patient name utilizing a stored procedure
        'Parameters: lngProviderID - id of the provider to return records for
        'Returns: ADORst
        '--------------------------------------------------------------------
        Dim objPP As New ProvPayoutDB.CProvPayoutDB
        objPP.ConnectionString = _ConnectionString

        FetchByDate = objPP.FetchByDate(dteStart, dteEnd)
        objPP = Nothing


    End Function

    Public Function AddDeduction(ByVal lngProvPayID As Long, ByVal strDescription As String, ByVal curAmount As Decimal, _
                                 ByVal lngProviderID As Long, ByVal dtAddDate As Date, ByVal strAddedBy As String) As Long
        '--------------------------------------------------------------------
        'Date: 09/29/2000
        'Author: Eric Pena
        'Description:  Inserts a row into the tblProviderPayout table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------

        Dim objPP As New ProvPayoutDB.CProvPayoutDB
        Dim strErrMessage As String
        objPP.ConnectionString = _ConnectionString

        If isProcessed(lngProvPayID) Then
            strErrMessage = "This payout has already been processed.  Cannot add."
            Exit Function
        End If

        AddDeduction = objPP.AddDeduction(lngProvPayID, strDescription, curAmount, lngProviderID, dtAddDate, strAddedBy)


        objPP = Nothing


    End Function

    Public Sub UpdateDeduction(ByVal lngProvPayDeductID As Long, ByVal lngProvPayID As Long, _
                               ByVal strDescription As String, ByVal curAmount As Decimal, _
                               ByVal dtAddDate As Date, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 01/28/2003
        'Author: Dave Richkun
        'Description:  Updates a payout deduction
        'Parameters: lngProvPayDeductID - ID of the payout deduction to be updated
        '            lngProvPayID - ID of the payout to which the deduction is associated
        '            strDescription - Description of the deduction
        '            curAmount - Amount of the deduction
        '            dtAddDate - Date on which the transaction is added (may not be system date)
        '            strUserName - Name of the user performing the update
        'Returns: Null
        '--------------------------------------------------------------------

        Dim objPP As New ProvPayoutDB.CProvPayoutDB
        objPP.ConnectionString = _ConnectionString

        objPP.UpdateDeduction(lngProvPayDeductID, lngProvPayID, strDescription, _
                                 curAmount, dtAddDate, strUserName)
        objPP = Nothing



    End Sub

    Public Sub RemoveDeduction(ByVal lngProvPayDedID As Long, ByVal lngProvPayID As Long, _
                               ByVal dblAmount As Double)
        '--------------------------------------------------------------------
        'Date: 01/13/2002
        'Author: Dave Richkun
        'Description:  Deletes a row from tblProviderPayoutDeduction and updates the amount
        '              in the parent table (tblProviderPayout) utilizing a stored procedure.
        'Parameters: lngProvPayDedID - ID of the row in tblProviderPayoutDeduction to be deleted
        '            lngProvPayID - ID of the parent row in tblProviderPayout
        '            dblAmount - Amount of the transaction being deleted
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        Dim objPP As New ProvPayoutDB.CProvPayoutDB
        objPP.ConnectionString = _ConnectionString


        If objPP.isProcessed(lngProvPayID) Then

            'err.raise(vbObjectError, CLASS_NAME, "This payout has already been processed.  Cannot delete.")
            Exit Sub

        End If

        objPP.RemoveDeduction(lngProvPayDedID, lngProvPayID, dblAmount)




    End Sub
    Private Function VerifyData(ByVal lngProviderID As Long, ByVal dteStart As Date, ByVal dteEnd As Date, ByRef strErrMessage As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 09/29/2000
        'Author: Eric Pena
        'Description:  Verifies all required data has been provided by the user.
        'Parameters:  The values to be checked.
        'Returns: Boolean value identifying if all criteria has been satisfied.
        '--------------------------------------------------------------------
        Dim rst As New ADODB.Recordset



        If lngProviderID < 0 Then
            strErrMessage = "You must specify a valid Provider."
            VerifyData = False
            Exit Function
        End If

        If dteStart > dteEnd Then
            strErrMessage = "Payout dates are invalid."
            VerifyData = False
            Exit Function
        End If

        If dteEnd > Date.Now Then

            strErrMessage = "Payout cannot extend into the current date."
            VerifyData = False
            Exit Function
        End If

        If Exists(lngProviderID, dteStart) Then
            strErrMessage = "Payout has already been generated for this provider."
            VerifyData = False
            Exit Function
        End If

        'If we get here, all is well...
        VerifyData = True
        rst = Nothing

    End Function
    Public Function FetchDecutions(ByVal lngProvPayID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/29/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblProviderPayout table ordered by patient name utilizing a stored procedure
        'Parameters: lngProviderID - id of the provider to return records for
        'Returns: ADORst
        '--------------------------------------------------------------------
        Dim objPP As New ProvPayoutDB.CProvPayoutDB
        objPP.ConnectionString = _ConnectionString

        FetchDecutions = objPP.FetchDecutions(lngProvPayID)


        objPP = Nothing



    End Function

    Private Function Exists(ByVal lngProviderID As Long, ByVal dteStart As Date) As Boolean
        '--------------------------------------------------------------------
        'Date: 09/29/2000
        'Author: Eric Pena
        'Description:  Lets the  er know if a given EncounterLog Entry has been processed
        'Parameters: lngID - The ID of the EL Entry
        'Returns: T if the association is processed, F otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objPP As New ProvPayoutDB.CProvPayoutDB
        objPP.ConnectionString = _ConnectionString

        Exists = objPP.Exists(lngProviderID, dteStart)


    End Function

    ''' DR: 12/17/2002 - Method appears to be dead.
    '''Public Function Generate(ByVal dteStart As Date, ByVal dteEnd As Date, ByRef aryProviderID() As Long, ByVal strAddedBy As String)
    ''''--------------------------------------------------------------------
    ''''Date: 09/29/2000
    ''''Author: Eric Pena
    ''''Description:  Inserts a row into the tblProviderPayout table utilizing
    ''''        objPatRPPlan.ConnectionString = _ConnectionString        a stored procedure.
    ''''Parameters: Each parameter identifies the column value that will be
    ''''              inserted.
    ''''Returns: ID (Primary Key) of the row inserted.
    ''''--------------------------------------------------------------------
    ''''Revision History:
    ''''   R000 06/21/2001 EP - Revised to reflect new PMF schedule
    ''''                       To be effective 7/1/01:
    ''''                           -   17% on fax billings
    ''''                           -   9% on billings under the ABS TIN
    ''''                           -   10% on all other billings
    ''''   R001 07/12/2001 EP - Basicly, undid R000 (fooking ops!)
    ''''--------------------------------------------------------------------
    '''    Dim strErrMessage   As String
    '''    Dim lngCtr          As Long
    '''    Dim lngID           As Long
    '''    Dim lngDedID As Long
    '''    'BEGIN R001
    '''    Dim curABS As Decimal
    '''    Dim curABSMP As Decimal
    '''    Dim curMain As Decimal
    '''    Dim curMainMP As Decimal
    '''    Dim curRefund As Decimal
    '''    Dim curNetPay As Decimal
    '''    Dim rstABS As ADODB.Recordset
    '''    Dim rstABSMP As ADODB.Recordset
    '''    Dim rstMain As ADODB.Recordset
    '''    Dim rstMainMP As ADODB.Recordset
    '''    Dim rstIOU As ADODB.Recordset
    '''    Dim blnHasIOU As Boolean
    '''
    '''    :
    '''
    '''    If Not IsDate(dteStart) Or Not IsDate(dteEnd) Then
    '''        strErrMessage = "Payout dates are invalid."
    '''       Exit Function
    '''    End If
    '''
    '''    Screen.MousePointer = vbHourglass
    '''
    '''    'make sure payout has not already been generated
    '''    For lngCtr = 0 To UBound(aryProviderID) - 1
    '''        If Not VerifyData(aryProviderID(lngCtr), dteStart, dteEnd, strErrMessage) Then Exit Function
    '''    Next lngCtr
    '''
    '''    'get data needed
    '''    FetchPayoutPayments dteStart, dteEnd, rstABS, rstABSMP, rstMain, rstMainMP
    '''
    '''
    '''    For lngCtr = 0 To UBound(aryProviderID) - 1
    '''        blnHasIOU = False
    '''        curABS = 0
    '''        If Not rstABS.BOF Then rstABS.MoveFirst
    '''        rstABS.Find "fldProviderID = " & aryProviderID(lngCtr)
    '''        If Not rstABS.EOF Then curABS = rstABS.Fields("fldAmount").Value
    '''
    '''        curABSMP = 0
    '''        If Not rstABSMP.BOF Then rstABSMP.MoveFirst
    '''        rstABSMP.Find "fldProviderID = " & aryProviderID(lngCtr)
    '''        If Not rstABSMP.EOF Then curABSMP = rstABSMP.Fields("fldAmount").Value
    '''
    '''        curMain = 0
    '''        If Not rstMain.BOF Then rstMain.MoveFirst
    '''        rstMain.Find "fldProviderID = " & aryProviderID(lngCtr)
    '''        If Not rstMain.EOF Then curMain = rstMain.Fields("fldAmount").Value
    '''
    '''        curMainMP = 0
    '''        If Not rstMainMP.BOF Then rstMainMP.MoveFirst
    '''        rstMainMP.Find "fldProviderID = " & aryProviderID(lngCtr)
    '''        If Not rstMainMP.EOF Then curMainMP = rstMainMP.Fields("fldAmount").Value
    '''
    '''        'dont add MPs (provs already got that money)
    '''        lngID = Insert(aryProviderID(lngCtr), dteStart, dteEnd, curABS + curMain)
    '''        curNetPay = CCur(curABS + curMain)
    '''
    '''
    '''        'if we posted refunds, but provider didnt make any money, store the amount to deduct in a later payout
    '''        If CCur(curABS + curMain) < 0 Then
    '''            blnHasIOU = True
    '''            'provider owes us (refund amount - PMF we deducted)
    '''            If curABS < 0 Then InsertIOU aryProviderID(lngCtr), "Refund of " & Format(Abs(curABS), "Decimal") & " (less 9%) - " & dteEnd & " payout", CCur(Abs((curABS)) - CCur(Abs((curABS)) * 0.09))
    '''            If curMain < 0 Then InsertIOU aryProviderID(lngCtr), "Refund of " & Format(Abs(curMain), "Decimal") & " (less 10%) - " & dteEnd & " payout", CCur(Abs((curMain)) - CCur(Abs((curMain)) * 0.1))
    '''        End If
    '''
    '''        'post deductions only if provider will be getting a check - otherwise the IOU will handle next time
    '''        'standard payout rules
    '''        If curABS <> 0 And Not blnHasIOU Then
    '''            AddDeduction lngID, "Psyquel ABS PMF - 10% on " & Format(curABS, "Decimal"), CCur(curABS * 0.1), strAddedBy
    '''            curNetPay = curNetPay - CCur(curABS * 0.1)
    '''        End If
    '''
    '''        If curABSMP <> 0 And Not blnHasIOU Then
    '''            If curNetPay >= CCur(curABSMP * 0.19) Then
    '''                AddDeduction lngID, "Psyquel ABS PMF - 10% (MP) on " & Format(curABSMP, "Decimal"), CCur(curABSMP * 0.1), strAddedBy
    '''                curNetPay = curNetPay - CCur(curABSMP * 0.1)
    '''            Else
    '''                InsertIOU aryProviderID(lngCtr), "ABS PMF (MP): 10% of " & Format(Abs(CCur(curABSMP)), "Decimal") & " - " & dteEnd & " payout", CCur(curABSMP * 0.1)
    '''            End If
    '''        End If
    '''
    '''        If curMain <> 0 And Not blnHasIOU Then
    '''            AddDeduction lngID, "Psyquel PMF - 10% on " & Format(curMain, "Decimal"), CCur(curMain * 0.1), strAddedBy
    '''            curNetPay = curNetPay - CCur(curMain * 0.1)
    '''        End If
    '''
    '''        If curMainMP <> 0 And Not blnHasIOU Then
    '''            If curNetPay >= CCur(curMainMP * 0.1) Then
    '''                AddDeduction lngID, "Psyquel PMF - 10% (MP) on " & Format(curMainMP, "Decimal"), CCur(curMainMP * 0.1), strAddedBy
    '''                curNetPay = curNetPay - CCur(curMainMP * 0.1)
    '''            Else
    '''                InsertIOU aryProviderID(lngCtr), "Psyquel PMF (MP): 10% of " & Format(Abs(CCur(curMainMP)), "Decimal") & " - " & dteEnd & " payout", CCur(curMainMP * 0.1)
    '''            End If
    '''        End If
    '''
    '''        'check to see if provider owes us $$
    '''        Set rstIOU = FetchIOUByProvider(aryProviderID(lngCtr))
    '''        If Not rstIOU.EOF Then
    '''            While curNetPay > 0 And Not rstIOU.EOF
    '''                If curNetPay >= rstIOU.Fields("fldIOUBal").Value Then
    '''                    'pay entire IOU
    '''                    lngDedID = AddDeduction(lngID, rstIOU.Fields("fldDescription").Value, rstIOU.Fields("fldIOUBal").Value, strAddedBy)
    '''                    PayIOU rstIOU.Fields("fldIOUID").Value, rstIOU.Fields("fldIOUBal").Value, lngDedID
    '''                    curNetPay = CCur(curNetPay - rstIOU.Fields("fldIOUBal").Value)
    '''                Else
    '''                    'pay partial IOU
    '''                    lngDedID = AddDeduction(lngID, rstIOU.Fields("fldDescription").Value, curNetPay, strAddedBy)
    '''                    PayIOU rstIOU.Fields("fldIOUID").Value, curNetPay, lngDedID
    '''                    curNetPay = 0
    '''                End If
    '''                rstIOU.MoveNext
    '''            Wend
    '''        End If
    '''    Next lngCtr
    '''
    '''    Screen.MousePointer = vbNormal
    '''
    '''    'Signal successful completion
    '''    GetObjectContext.SetComplete
    '''    Set rstABS = Nothing
    '''    Set rstABSMP = Nothing
    '''    Set rstMain = Nothing
    '''    Set rstMainMP = Nothing
    '''    Set rstIOU = Nothing
    '''    Exit Function
    '''
    '''
    '''    'Signal incompletion and raise the error to the  ing environment.  The
    '''    'condition handles custom business rule checks we may have established.
    '''    Screen.MousePointer = vbNormal
    '''    Set rstABS = Nothing
    '''    Set rstABSMP = Nothing
    '''    Set rstMain = Nothing
    '''    Set rstMainMP = Nothing
    '''    Set rstIOU = Nothing
    '''    GetObjectContext.SetAbort
    '''
    '''    If Err.Number = 0 Then
    '''       'err.raise vbObjectError, CLASS_NAME, strErrMessage
    '''    Else
    '''       'err.raise Err.Number, Err.Source, Err.Description
    '''    End If
    '''End Function

    Public Function FetchPayoutPayments(ByVal dteStart As Date, ByVal dteEnd As Date, ByRef rstABS As ADODB.Recordset, ByRef rstABSMP As ADODB.Recordset, _
                                                                    ByRef rstMain As ADODB.Recordset, ByRef rstMainMP As ADODB.Recordset)

        '--------------------------------------------------------------------
        'Date: 09/29/2000
        'Author: Eric Pena
        'Description:  Fetches totals by provider to use for payout
        'Parameters: dteStart / dteEnd - Date range to fetch postings for
        'Returns: Recordsets of gross postings by provider for the given date range
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objPP As New ProvPayoutDB.CProvPayoutDB
        objPP.ConnectionString = _ConnectionString
        objPP.FetchPayoutPayments(dteStart, dteEnd, rstABS, rstABSMP, rstMain, rstMainMP)


        objPP = Nothing


    End Function

    Public Function FetchByCriteria(ByVal lngProviderID As Long, ByVal dteStart As Date, ByVal dteEnd As Date) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/29/2000
        'Author: Eric Pena
        'Description:  Lets the  er know if a given EncounterLog Entry has been processed
        'Parameters: lngID - The ID of the EL Entry
        'Returns: T if the association is processed, F otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objPP As New ProvPayoutDB.CProvPayoutDB
        objPP.ConnectionString = _ConnectionString

        FetchByCriteria = objPP.FetchByCriteria(lngProviderID, dteStart, dteEnd)
        objPP = Nothing

    End Function

    Public Function InsertIOU(ByVal lngProviderID As Long, ByVal strDescription As String, ByVal curIOUAmt As Decimal) As Long
        '--------------------------------------------------------------------
        'Date: 08/09/2001
        'Author: Eric Pena
        'Description:  Inserts a row into the tblIOU table utilizing a stored procedure.
        'Parameters: Each parameter identifies the column value that will be inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------
        Dim objPP As New ProvPayoutDB.CProvPayoutDB
        Dim strErrMessage As String

        objPP.ConnectionString = _ConnectionString
        InsertIOU = objPP.InsertIOU(lngProviderID, strDescription, curIOUAmt)

        objPP = Nothing

    End Function
    Public Function UpdateIOU(ByVal lngIOUID As Long, ByVal lngProviderID As Long, ByVal strDescription As String, _
                ByVal curIOUAmt As Decimal, ByVal curIOUBal As Decimal) As Long
        '--------------------------------------------------------------------
        'Date: 08/09/2001
        'Author: Eric Pena
        'Description:  Updates a row into the tblIOU table utilizing a stored procedure.
        'Parameters: Each parameter identifies the column value that will be inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------
        Dim objPP As New ProvPayoutDB.CProvPayoutDB
        Dim strErrMessage As String

        objPP.ConnectionString = _ConnectionString

        UpdateIOU = objPP.UpdateIOU(lngIOUID, lngProviderID, strDescription, curIOUAmt, curIOUBal)


        objPP = Nothing

    End Function

    Public Function FetchIOUByProvider(ByVal lngProviderID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/09/2001
        'Author: Eric Pena
        'Description:  Updates a row in the tblProviderPayout  table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: Null
        '--------------------------------------------------------------------

        Dim objPP As New ProvPayoutDB.CProvPayoutDB
        objPP.ConnectionString = _ConnectionString

        FetchIOUByProvider = objPP.FetchIOUByProvider(lngProviderID)
        objPP = Nothing




    End Function

    Public Function FetchIOUByID(ByVal lngIOUID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/09/2001
        'Author: Eric Pena
        'Description:  Updates a row in the tblProviderPayout  table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: Null
        '--------------------------------------------------------------------

        Dim objPP As New ProvPayoutDB.CProvPayoutDB
        objPP.ConnectionString = _ConnectionString

        FetchIOUByID = objPP.FetchIOUByID(lngIOUID)
        objPP = Nothing


    End Function

    Public Sub PayIOU(ByVal lngIOUID As Long, ByVal curAmount As Decimal, ByVal lngPayoutDedID As Long)
        '--------------------------------------------------------------------
        'Date: 08/09/2001
        'Author: Eric Pena
        'Description:  Updates a row in the tblProviderPayout  table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: Null
        '--------------------------------------------------------------------
        Dim objPP As New ProvPayoutDB.CProvPayoutDB
        Dim rst As New ADODB.Recordset
        objPP.ConnectionString = _ConnectionString

        Dim strError As String = ""
        rst = objPP.FetchIOUByID(lngIOUID)
        If curAmount > Convert.ToDecimal(rst.Fields("fldIOUBal").Value) Then
            strError = "Cannot pay more than is owed."
            Exit Sub
        End If

        objPP.PayIOU(lngIOUID, curAmount, lngPayoutDedID)

        objPP = Nothing
        rst = Nothing

    End Sub

    Public Function FetchPayoutDatesByYear(ByVal intYear As Integer, ByVal lngProviderID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/22/2002
        'Author: Dave Richkun
        'Description:  Returns payout dates for a given year
        'Parameters: intYear - The 4-digit year for which payout dates will be retrieved
        'Returns: Recordset of payout dates
        '--------------------------------------------------------------------

        Dim objPP As New ProvPayoutDB.CProvPayoutDB
        objPP.ConnectionString = _ConnectionString

        FetchPayoutDatesByYear = objPP.FetchPayoutDatesByYear(intYear, lngProviderID)
        objPP = Nothing




    End Function

    Public Function FetchPayoutDetail(ByVal lngProvPayID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/23/2002
        'Author: Dave Richkun
        'Description:  Returns all transactions within a payout period for a provider
        'Parameters: lngProvPayID - ID of provider payout record with which payout details
        '               will be retrieved
        'Returns: Recordset of payout transactions
        '--------------------------------------------------------------------

        Dim objPP As New ProvPayoutDB.CProvPayoutDB
        objPP.ConnectionString = _ConnectionString

        FetchPayoutDetail = objPP.FetchPayoutDetail(lngProvPayID)
        objPP = Nothing


    End Function

    Public Sub Update(ByVal lngProvPayID As Long, ByVal dteEnd As Date, ByVal curGross As Decimal, ByVal curNet As Decimal)
        '--------------------------------------------------------------------
        'Date: 09/29/2000
        'Author: Eric Pena
        'Description:  Physi y deletes entries in tblProviderPayout.  This can only be done if the encounter has not yet been processed.
        'Parameters: lngID - ID of the row in the table whose value will be updated.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '--------------------------------------------------------------------
        Dim objPP As New ProvPayoutDB.CProvPayoutDB
        objPP.ConnectionString = _ConnectionString



        objPP.Update(lngProvPayID, dteEnd, curGross, curNet)
        objPP = Nothing



    End Sub
    Public Function InsertGroupPayment(ByVal lngGroupID As Long, ByVal lngUserID As Long, ByVal strPymtType As String, _
                ByVal curAmount As Decimal, ByVal strCheckNum As String, ByVal strNotes As String, _
                ByVal strAddedBy As String) As Long
        '--------------------------------------------------------------------
        'Date: 01/09/2013
        'Author: Duane C Orth
        'Description:  Inserts a row into the tblProviderPayment table utilizing
        '              a stored procedure.
        'Parameters:  lngGroupID - ID of Group making the payment to Psyquel
        '             lngUserID - ID of Off Mgr making the payment to Psyquel
        '             strPymtType - 1 character code indicating the type of payment
        '                   1-Check, 2-Cash, 3-Credit Card, 4-Other
        '             curAmount - Amount of payment
        '             strCheckNum - Check number if payment by check
        '             strNotes - Notes related to payment
        '             strAddedBy - User name of user adding payment record
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------
        '  R001: Created
        '--------------------------------------------------------------------

        Dim objPP As New ProvPayoutDB.CProvPayoutDB

        objPP.ConnectionString = _ConnectionString



        InsertGroupPayment = objPP.InsertGroupPayment(lngGroupID, lngUserID, strPymtType, curAmount, strCheckNum, _
                    strNotes, strAddedBy)
        objPP = Nothing



    End Function

    Public Function InsertProviderPayment(ByVal lngProviderID As Long, ByVal strPymtType As String, _
                ByVal curAmount As Decimal, ByVal strCheckNum As String, ByVal strNotes As String, _
                ByVal strAddedBy As String) As Long
        '--------------------------------------------------------------------
        'Date: 01/09/2003
        'Author: Dave Richkun
        'Description:  Inserts a row into the tblProviderPayment table utilizing
        '              a stored procedure.
        'Parameters:  lngProviderID - ID of provider making the payment to Psyquel
        '             strPymtType - 1 character code indicating the type of payment
        '                   1-Check, 2-Cash, 3-Credit Card, 4-Other
        '             curAmount - Amount of payment
        '             strCheckNum - Check number if payment by check
        '             strNotes - Notes related to payment
        '             strAddedBy - User name of user adding payment record
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------
        '  R001: Created
        '--------------------------------------------------------------------

        Dim objPP As New ProvPayoutDB.CProvPayoutDB
        objPP.ConnectionString = _ConnectionString

        InsertProviderPayment = objPP.InsertProviderPayment(lngProviderID, strPymtType, curAmount, strCheckNum, _
                    strNotes, strAddedBy)
        objPP = Nothing


    End Function

    Public Sub UpdateProviderPayment(ByVal lngPaymentID As Long, ByVal lngProviderID As Long, _
                ByVal strPymtType As String, ByVal curAmount As Decimal, ByVal strCheckNum As String, _
                ByVal strNotes As String, ByVal strAddedBy As String)
        '--------------------------------------------------------------------
        'Date: 01/13/2003
        'Author: Dave Richkun
        'Description:  Updates a row in tblProviderPayment table utilizing
        '              a stored procedure.
        'Parameters:  lngPaymentID - ID of provider payment that will be updated
        '             lngProviderID - ID of provider making the payment to Psyquel
        '             strPymtType - 1 character code indicating the type of payment
        '                   1-Check, 2-Cash, 3-Credit Card, 4-Other
        '             curAmount - Amount of payment
        '             strCheckNum - Check number if payment by check
        '             strNotes - Notes related to payment
        '             strAddedBy - User name of user adding payment record
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------
        '  R001: Created
        '--------------------------------------------------------------------

        Dim objPP As New ProvPayoutDB.CProvPayoutDB
        objPP.ConnectionString = _ConnectionString


        objPP.UpdateProviderPayment(lngPaymentID, lngProviderID, strPymtType, _
                  curAmount, strCheckNum, strNotes, strAddedBy)
        objPP = Nothing




    End Sub


    Public Function FetchPaymentsByCriteria(Optional ByVal blnIncludeDisabled As Boolean = False, _
                          Optional ByVal strWhere As String = "", _
                          Optional ByVal strOrderBy As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 01/03/2000
        'Author: Dave Richkun
        'Description:  Retrieves records from the tblUsers table.
        'Parameters: blnIncludeDisabled - Optional parameter that identifies if
        '               records flagged as 'Disabled' or 'De-activated' are to be
        '               included in the record set. The default value is False.
        '            strWhere - Optional 'Where' clause of the SQL statement by
        '               which records will be filtered.  If the parameter is not
        '               supplied, all records will be retrieved.
        '             strOrderBy - Optional 'Order By' clause in which retrieved
        '               records will be sorted.  The default is set to the
        '               'fldUser' column
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objPP As New ProvPayoutDB.CProvPayoutDB
        Dim rstSQL As New ADODB.Recordset

        objPP.ConnectionString = _ConnectionString

        rstSQL = objPP.FetchPaymentsByCriteria(blnIncludeDisabled, strWhere, strOrderBy)
        FetchPaymentsByCriteria = rstSQL



    End Function

    Public Function FetchPaymentsByProvider(ByVal lngProviderID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 01/13/2003
        'Author: Dave Richkun
        'Description:  Retrieves all payments associated with a provider via
        '              a stored procedure.
        'Parameters: lngProviderID - ID of provider for whom payments are retrieved
        'Returns: ADO Recordset
        '--------------------------------------------------------------------
        Dim objPP As New ProvPayoutDB.CProvPayoutDB
        objPP.ConnectionString = _ConnectionString

        FetchPaymentsByProvider = objPP.FetchPaymentsByProvider(lngProviderID)
        objPP = Nothing




    End Function


    Public Function FetchProviderPaymentByID(ByVal lngProvPymtID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 01/13/2003
        'Author: Dave Richkun
        'Description:  Retrieves a single payment record via a stored procedure.
        'Parameters: lngProvPymtID - ID of Provider Payment record to be retrieved
        'Returns: ADO Recordset
        '--------------------------------------------------------------------
        Dim objPP As New ProvPayoutDB.CProvPayoutDB
        objPP.ConnectionString = _ConnectionString

        FetchProviderPaymentByID = objPP.FetchProviderPaymentByID(lngProvPymtID)
        objPP = Nothing



    End Function


    Public Sub DeleteProviderPayment(ByVal lngProvPymtID As Long, ByVal strDeletedBy As String)
        '--------------------------------------------------------------------
        'Date: 01/13/2003
        'Author: Dave Richkun
        'Description:  Disabled a row in tblProviderPayment table utilizing
        '              a stored procedure.
        'Parameters:  lngProvPymtID - ID of row that will be disabled
        '             strDeletedBy - User name of user disabling payment record
        'Returns: Null
        '--------------------------------------------------------------------
        '  R001: Created
        '--------------------------------------------------------------------

        Dim objPP As New ProvPayoutDB.CProvPayoutDB
        objPP.ConnectionString = _ConnectionString

        objPP.DeleteProviderPayment(lngProvPymtID, strDeletedBy)
        objPP = Nothing

    End Sub

    Public Function FetchDeductionByID(ByVal lngProvPayDedID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 01/28/2003
        'Author: Dave Richkun
        'Description:  Retrieves a single payout deduction record via a stored procedure.
        'Parameters: lngProvPayDedID - ID of Provider Payment record to be retrieved
        'Returns: ADO Recordset
        '--------------------------------------------------------------------

        Dim objPP As New ProvPayoutDB.CProvPayoutDB
        objPP.ConnectionString = _ConnectionString
        FetchDeductionByID = objPP.FetchDeductionByID(lngProvPayDedID)
        objPP = Nothing



    End Function

    Public Function FetchPayoutSummaryByProvider(ByVal lngProviderID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 02/04/2003
        'Author: Dave Richkun
        'Description:  Retrieves balance owing per payout period for the specified provider.
        'Parameters: lngProviderID - ID of provider whose payout summary will be retrieved
        'Returns: ADO Recordset
        '--------------------------------------------------------------------

        Dim objPP As New ProvPayoutDB.CProvPayoutDB
        objPP.ConnectionString = _ConnectionString

        FetchPayoutSummaryByProvider = objPP.FetchPayoutSummaryByProvider(lngProviderID)
        objPP = Nothing

        'Signal successful completion


    End Function

    Public Sub InsertPayoutRows(ByVal dtPayoutStartDate As Date)
        '--------------------------------------------------------------------
        'Date: 02/12/2003
        'Author: Dave Richkun
        'Description:  Inserts rows in tblProviderPayout for all providers not already having
        '              having a row for the specified payout start date.
        'Parameters: dtPayoutStartDate - Starting date of the payout period
        'Returns: Null
        '--------------------------------------------------------------------
        Dim objPP As New ProvPayoutDB.CProvPayoutDB
        objPP.ConnectionString = _ConnectionString
 
        objPP.InsertPayoutRows(dtPayoutStartDate)
        objPP = Nothing


    End Sub


End Class
