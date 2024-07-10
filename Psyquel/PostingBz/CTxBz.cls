
'------------------------------------------------------------------------------------
'Class Name:    CTxBz
'Date:          07/24/2002
'Author:        Eric Pena
'Denis J Finkel
'Description:   Business object designed to   methods associated with Insurance postings (tblInsuranceTx)
'------------------------------------------------------------------------------------
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

Public Class CTxBz


    Private _ConnectionString As String = String.Empty




    Private Const CLASS_NAME As String = "CTxBz"
    Private Const TABLE_NAME As String = "tblInsuranceTx"
    Private Const CONST_PAT_ADJ As Long = 2
    Private Const CONST_PATPMTADJ_OT As Long = 19
    Private Const CONST_PATPMTADJ_CA As Long = 20
    Private Const CONST_PATPMTADJ_CK As Long = 21
    Private Const CONST_PATPMTADJ_CC As Long = 22
    Private Const CONST_WO As Integer = 129
    Private Const TX_TYPE_HCFASUBMITTED As Long = 5

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property





    Public Function Insert(ByVal lngBRID As Long, ByVal lngInsuranceID As Long, ByVal lngCPCID As Long, ByVal lngPlanID As Long, ByVal lngRPID As Long, _
                        ByVal lngELID As Long, ByVal lngEncDetailID As Long, ByVal lngPatientID As Long, ByVal lngNextRPID As Long, ByVal lngNextPlanID As Long, ByVal lngNextBRID As Long, _
                        ByVal lngNextCPCID As Long, ByVal strPostedBy As String, ByVal lngTxTypeID As Long, ByVal strType As String, ByVal dteReceiveDate As Date, _
                        ByVal lngCptRecordID As Long, ByVal curAmount As Decimal, ByVal curDisAmount As Decimal, ByVal curBalance As Decimal, ByVal strCheckNum As String, _
                        ByVal dteCheckDate As Date, ByVal lngCheckID As Long, ByVal strTxNotes As String, Optional ByVal blnNextPayer As Boolean = False, Optional ByVal blnQueueNextPayer As Boolean = False, _
                        Optional ByVal lngClaimID As Long = -1, Optional ByVal lngVersionNum As Long = -1, Optional ByVal lngClinicID As Long = -1, Optional ByVal lngNextInsuranceID As Long = -1, Optional ByVal lngNextOrder As Long = -1) As Long
        '------------------------------------------------------------------------------------
        'Date:          07/24/2002
        'Author:        Eric Pena
        'Description:   Inserts a row into tblInsuranceTx utilizing a stored procedure.
        'Parameters:    Each parameter identifies the column value that will be inserted unless specified below
        '               blnNextPayer - if true will move the remaining balance to the next responsible payer
        '               blnQueueNextPayer - if true will generate a claim for the next responsible payer
        '               lngNextBRID - BR ID in tblBillingResponsibility of the next payer in line, or -1 if there is only one payer on the claim
        '               lngNextRPID - RP ID of the next payer in line, or -1 if there is only one payer on the claim
        '               lngNextPlanID - Plan ID of the next payer in line, or -1 if the RP is next
        '               curBalance - claim balance for the payer inserting the tx (after insert)
        'Returns:       ID (Primary Key) of the row inserted
        '------------------------------------------------------------------------------------
        Dim objTx As New PostingDB.CTxDB
        Dim objClaim As New ClaimBz.CClaimBz
        Dim objInsTx As New BillingBZ.CInsuranceTxBz

        objTx.ConnectionString = _ConnectionString
        objClaim.ConnectionString = _ConnectionString
        objInsTx.ConnectionString = _ConnectionString



        Dim strErrMessage As String
        Dim lngPos As Long



        If lngNextCPCID < 0 Then
            lngPos = 2
        Else
            lngPos = -1
        End If

        'give any remaining bal to the next in line, if app
        If blnNextPayer And curBalance <> 0 Then
            If lngTxTypeID = 7 Then ' And lngNextBRID = -1    If Disallowance and balance is patients responsibility
                TransferResponsibility(lngBRID, lngPlanID, lngRPID, lngNextBRID, lngNextRPID, lngNextPlanID, curBalance, lngELID, lngPatientID, lngPos, 87, strPostedBy)
            Else
                TransferResponsibility(lngBRID, lngPlanID, lngRPID, lngNextBRID, lngNextRPID, lngNextPlanID, curBalance, lngELID, lngPatientID, lngPos, 246, strPostedBy)
            End If
        End If

        'Insert insurance transaction
        Insert = objTx.Insert(lngBRID, lngInsuranceID, lngCPCID, lngPlanID, lngRPID, lngELID, lngEncDetailID, lngPatientID, strPostedBy, lngTxTypeID, strType, dteReceiveDate, _
                 lngCptRecordID, curAmount, curDisAmount, strCheckNum, dteCheckDate, lngCheckID, strTxNotes)
        objTx = Nothing

        'generate claim for next payer, if applicable
        If blnQueueNextPayer And lngNextInsuranceID > 0 Then
            If lngClaimID > 0 Then
                objClaim.Queued(lngClaimID, True)
            Else
                'Insert 'HCFA Submitted' transaction for next payer
                objInsTx.Insert(lngELID, lngEncDetailID, lngPatientID, lngNextRPID, lngNextPlanID, _
                      lngNextCPCID, lngNextInsuranceID, "P", TX_TYPE_HCFASUBMITTED, 0, 0, dteReceiveDate, _
                      "", "01\01\1900", "", 0, 0, strPostedBy)
                objInsTx = Nothing

                'Insert the keys for the claim record
                objClaim.Insert(lngELID, lngVersionNum, lngNextRPID, lngClinicID, lngNextPlanID, lngNextCPCID, lngNextInsuranceID, lngNextOrder, strPostedBy)
            End If
            objClaim = Nothing
        End If

        objClaim = Nothing
        objTx = Nothing
        objInsTx = Nothing



    End Function
    Public Sub Delete(ByVal lngInsuranceTxID As Long, ByVal lngBRID As Long, ByVal lngELID As Long, ByVal curAmount As Decimal, _
                    ByVal lngTxTypeID As Long, ByVal lngTxGroupID As Long, ByVal strType As String, ByVal lngCheckID As Long)
        '------------------------------------------------------------------------------------
        'Date:          07/29/2002
        'Author:        Eric Pena
        'Description:   Deletes a row into tblInsuranceTx and reverts derived columns utilizing a stored procedure
        'Parameters:    lngInsuranceTxID - PK of row to delete
        '               lngBRID - PK in tblBillingresponsibility of row to recalculate derived balances
        '               lngELID - PK in tblEncounterLog of row to recalculate derived balances
        '               other parameters - information about row being deleted
        '------------------------------------------------------------------------------------
        Dim objTx As New PostingDB.CTxDB
        objTx.ConnectionString = _ConnectionString

        objTx.Delete(lngInsuranceTxID, lngBRID, lngELID, curAmount, lngTxTypeID, lngTxGroupID, strType, lngCheckID)
        objTx = Nothing

    End Sub
    Public Sub Update(ByVal lngTxID As Long, ByVal lngBRID As Long, ByVal lngInsuranceID As Long, ByVal lngCPCID As Long, ByVal lngPlanID As Long, ByVal lngRPID As Long, _
                        ByVal lngELID As Long, ByVal lngEncDetailID As Long, ByVal lngPatientID As Long, ByVal lngNextRPID As Long, ByVal lngNextPlanID As Long, ByVal lngNextBRID As Long, _
                        ByVal strPostedBy As String, ByVal lngTxTypeID As Long, ByVal lngTxGroupID As Long, ByVal strType As String, ByVal dteReceiveDate As Date, _
                        ByVal lngCptRecordID As Long, ByVal curAmount As Decimal, ByVal curDisAmount As Decimal, ByVal curBalance As Decimal, ByVal strCheckNum As String, ByVal dteCheckDate As Date, _
                        ByVal lngCheckID As Long, ByVal strTxNotes As String, ByVal curOldAmt As Decimal, ByVal lngOldCheckID As Long)
        '------------------------------------------------------------------------------------
        'Date:          07/24/2002
        'Author:        Eric Pena
        'Description:   Inserts a row into tblInsuranceTx utilizing a stored procedure.
        'Parameters:    Each parameter identifies the column value that will be inserted unless specified below
        '               blnNextPayer - if true will move the remaining balance to the next responsible payer
        '               blnQueueNextPayer - if true will generate a claim for the next responsible payer
        '               lngNextBRID - BR ID in tblBillingResponsibility of the next payer in line, or -1 if there is only one payer on the claim
        '               lngNextRPID - RP ID of the next payer in line, or -1 if there is only one payer on the claim
        '               lngNextPlanID - Plan ID of the next payer in line, or -1 if the RP is next
        '               curBalance - claim balance for the payer inserting the tx (after insert)
        'Returns:       ID (Primary Key) of the row inserted
        '------------------------------------------------------------------------------------
        Dim objTx As New PostingDB.CTxDB
        objTx.ConnectionString = _ConnectionString



        objTx.Delete(lngTxID, lngBRID, lngELID, curOldAmt, lngTxTypeID, lngTxGroupID, strType, lngOldCheckID)

        objTx.Insert(lngBRID, lngInsuranceID, lngCPCID, lngPlanID, lngRPID, lngELID, lngEncDetailID, lngPatientID, strPostedBy, lngTxTypeID, strType, dteReceiveDate, lngCptRecordID, curAmount, curDisAmount, _
                         strCheckNum, dteCheckDate, lngCheckID, strTxNotes)
        objTx = Nothing



    End Sub
    Public Function Fetch(ByVal lngELID As Long, ByVal intType As Integer) As ADODB.Recordset
        '-------------------------------------------------------------------------------------
        'Date: 10/04/00
        'Author: Eric Pena
        'Description:   Retrieves records from the tblTransaction utilizing a stored procedure.
        'Parameters:    lngELID- the bill/encounter to return records for.
        '                       intType - the type of transactions to search for:
        '                                = 1 for CHARGE transactions only
        '                                = 2 for POSTING transactions only
        '                                = anything else for ALL transactions
        'Returns: ADODB.Recordset
        '-------------------------------------------------------------------------------------

        Dim objTx As New PostingDB.CTxDB
        objTx.ConnectionString = _ConnectionString

        Fetch = objTx.Fetch(lngELID, intType)



    End Function
    Public Function FetchBRByID(ByVal lngBRID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/31/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblBillingResponsibility and tblInsCompany tables utilizing a stored procedure
        'Parameters: blnIncludeDisabled - determines if disabled records will be returned
        'Returns: ADORst
        '--------------------------------------------------------------------
        Dim objBR As New PostingDB.CTxDB


        FetchBRByID = objBR.FetchBRByID(lngBRID)


    End Function
    Public Function FetchBRByEL(ByVal lngELID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/31/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblBillingResponsibility and tblInsCompany tables utilizing a stored procedure
        'Parameters: blnIncludeDisabled - determines if disabled records will be returned
        'Returns: ADORst
        '--------------------------------------------------------------------
        Dim objBR As New PostingDB.CTxDB
        objBR.ConnectionString = _ConnectionString

        FetchBRByEL = objBR.FetchBRByEL(lngELID)


        objBR = Nothing

    End Function
    Public Sub TransferResponsibility(ByVal lngBRIDSource As Long, ByVal lngPlanID As Long, ByVal lngRPID As Long, ByVal lngBRIDDest As Long, ByVal lngNextRPID As Long, _
                ByVal lngNextPlanID As Long, ByVal curAmount As Decimal, ByVal lngELID As Long, ByVal lngPatientID As Long, ByVal lngPos As Long, ByVal lngTransferType As Long, _
                ByVal strUsername As String, Optional ByVal lngPatientApptID As Long = -1, Optional ByVal lngProviderID As Long = -1, Optional ByVal dteDOS As Date = Nothing)
        '--------------------------------------------------------------------
        'Date: 02/24/2000
        'Author: Eric Pena
        'Description:  Moves the responsibility from one entry in tblBillingresponsibility to another
        'Parameters: lngBRIDFrom -  key of the BR record to move resp. from.
        '                   lngBRIDTo -  key of the BR record to move resp. to or -1 to move all remaining balances to the next payer
        '                   curAmount  - The amount of resp. to move
        '                   lngPos  - If positive, the location (source/dest) of the RP record.  Used in determining how much to alter charge in tblPatientTx
        '--------------------------------------------------------------------
        Dim objBR As New PostingDB.CTxDB


        objBR.ConnectionString = _ConnectionString




        Dim rstBR As New ADODB.Recordset
        Dim strError As String


        strError = ""

        'check data
        If curAmount <= 0 And lngBRIDDest > 0 Then
            strError = "You must specify a positive amount to transfer"
            Exit Sub
        End If

        objBR.TransferResponsibility(lngBRIDSource, lngPlanID, lngRPID, lngBRIDDest, lngNextRPID, lngNextPlanID, curAmount, lngELID, lngPatientID, lngPos, lngTransferType, strUsername, lngPatientApptID, lngProviderID, dteDOS)



        objBR = Nothing
        rstBR = Nothing

    End Sub
    Private Function SameBill(ByVal lngBRID1 As Long, ByVal lngBRID2 As Long) As Boolean
        '--------------------------------------------------------------------
        'Date: 02/24/2000
        'Author: Eric Pena
        'Description:  Determines if two BRIDs are recorded under the same bill
        'Parameters: lngBRID1 / lngBRID2 -  key of the BR record to check
        'Returns: True if both records are under the same bill, False otherwise
        '--------------------------------------------------------------------
        Dim objBR As New PostingDB.CTxDB

        objBR.ConnectionString = _ConnectionString

        SameBill = objBR.SameBill(lngBRID1, lngBRID2)


        objBR = Nothing

    End Function
    Public Function FetchBRByPatientProvider(ByVal lngPatientID As Long, ByVal lngProviderID As Long, ByVal blnOutstandingOnlyYN As Boolean) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 9/18/2000
        'Author: Eric Pena
        'Description:  Returns rows from tblBillingResponsibility that have an outstanding balance utilizing a stored procedure
        'Parameters: lngPatientID - returns BR records for this patient only
        'Returns: ADORst
        '--------------------------------------------------------------------
        Dim objBR As New PostingDB.CTxDB

        objBR.ConnectionString = _ConnectionString



        FetchBRByPatientProvider = objBR.FetchBRByPatientProvider(lngPatientID, lngProviderID, blnOutstandingOnlyYN)


    End Function
    Public Function FetchForCancel(ByVal lngELID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 9/18/2000
        'Author: Eric Pena
        'Description:  Returns rows from tblBillingResponsibility that have an outstanding balance utilizing a stored procedure
        'Parameters: lngPatientID - returns BR records for this patient only
        'Returns: ADORst
        '--------------------------------------------------------------------
        Dim objBR As New PostingDB.CTxDB

        objBR.ConnectionString = _ConnectionString


        FetchForCancel = objBR.FetchForCancel(lngELID)


    End Function
    Public Function setAsCancelled(ByVal lngELID As Long)
        '--------------------------------------------------------------------
        'Date: 9/18/2000
        'Author: Eric Pena
        'Description:  Returns rows from tblBillingResponsibility that have an outstanding balance utilizing a stored procedure
        'Parameters: lngPatientID - returns BR records for this patient only
        'Returns: ADORst
        '--------------------------------------------------------------------
        Dim objBR As New PostingDB.CTxDB

        objBR.ConnectionString = _ConnectionString

        objBR.setAsCancelled(lngELID)


    End Function

    Public Function FetchDenialList(ByVal lngUserID As Long, _
                                    Optional ByVal blnForceIndividual As Boolean = False, _
                                    Optional ByVal strOrderby As String = "") _
                                    As ADODB.Recordset
        '----------------------------------------------------------------------------------------
        'Date: 05/16/2001                                                                       '
        'Author: Rick "Boom Boom" Segura                                                        '
        'Description:   Retrieves a recordset of Denied claims for a provider or a group of     '
        '               providers                                                               '
        'Parameters:    lngUserID - ID of Provider or ID of Office Manager                      '
        '               blnForceIndividual - In the case an Office manager is also a provider,  '
        '                   this variable forces the stored procedure to return only claims     '
        '                   where the manager is the servicing provider                         '
        '               strOrderBy - If passed, instructs the stored procedure how to order the '
        '                   returned data                                                       '
        'Returns: ADODB.Recordset                                                               '
        '----------------------------------------------------------------------------------------
        Dim objDenial As New PostingDB.CTxDB

        objDenial.ConnectionString = _ConnectionString

        FetchDenialList = objDenial.FetchDenialList(lngUserID, blnForceIndividual, strOrderby)

        objDenial = Nothing


    End Function

    Public Function FetchDenialSummary(ByVal lngBillID As Long) As ADODB.Recordset
        '----------------------------------------------------------------------------------------
        'Date: 05/22/2001                                                                       '
        'Author: Rick "Boom Boom" Segura                                                        '
        'Description:   Retrieves summary information of a bill's most recent denial transaction'
        'Parameters:    lngBillID - ID of bill to fetch information for                         '
        'Returns: ADODB.Recordset                                                               '
        '----------------------------------------------------------------------------------------

        Dim objDenial As New PostingDB.CTxDB
        objDenial.ConnectionString = _ConnectionString
  
        FetchDenialSummary = objDenial.FetchDenialSummary(lngBillID)


        objDenial = Nothing

    End Function
    Public Function FetchHoldList(ByVal lngUserID As Long, _
                                    Optional ByVal blnForceIndividual As Boolean = False, _
                                    Optional ByVal strOrderby As String = "") _
                                    As ADODB.Recordset
        '----------------------------------------------------------------------------------------
        'Date: 05/16/2001                                                                       '
        'Author: Rick "Boom Boom" Segura                                                        '
        'Description:   Retrieves a recordset of Denied claims for a provider or a group of     '
        '               providers                                                               '
        'Parameters:    lngUserID - ID of Provider or ID of Office Manager                      '
        '               blnForceIndividual - In the case an Office manager is also a provider,  '
        '                   this variable forces the stored procedure to return only claims     '
        '                   where the manager is the servicing provider                         '
        '               strOrderBy - If passed, instructs the stored procedure how to order the '
        '                   returned data                                                       '
        'Returns: ADODB.Recordset                                                               '
        '----------------------------------------------------------------------------------------
        Dim objHold As New PostingDB.CTxDB



        objHold.ConnectionString = _ConnectionString

        FetchHoldList = objHold.FetchHoldList(lngUserID, blnForceIndividual, strOrderby)

        objHold = Nothing


    End Function
    Public Function FetchPatWebPmt(ByVal lngPatientID As Long, ByVal lngUserID As Long) As ADODB.Recordset
        '-------------------------------------------------------------------------------------
        'Date: 06/13/2001
        'Author: Eric Pena
        'Description:   Retrieves patient web payments (adjustments) from tblTransaction utilizing a stored procedure.
        'Parameters:    lngPatientID - the bill/encounter to return records for.
        '               lngUserID - the ID of the user requesting payments.  If this user is a
        '                       office manager, records will be returned for all providers they service
        'Returns: ADODB.Recordset
        '-------------------------------------------------------------------------------------

        Dim objTx As New PostingDB.CTxDB

        objTx.ConnectionString = _ConnectionString



        FetchPatWebPmt = objTx.FetchPatWebPmt(lngPatientID, lngUserID)


        objTx = Nothing

    End Function
    Public Sub WOClaims(ByVal lngProviderID As Long, ByVal dteWOOnBefore As Date, ByVal strWOBy As String)
        '-------------------------------------------------------------------------------
        'Author: Eric Pena
        'Date: 06/22/2001
        'Description: This procedure writes off outstanding claims with a DOS and post
        '               date on or before the specified date for specified providers
        'Parameters: lngProviderID - provider to write claims off for
        '               dteWOOnBefore - write off claims billed/serviced on or before this date
        'Returns: Null
        '-------------------------------------------------------------------------------
        'Revision History:
        '
        '-------------------------------------------------------------------------------
        Dim obj As New PostingDB.CTxDB
        Dim rst As New ADODB.Recordset
        Dim lngCtr As Long


        obj.ConnectionString = _ConnectionString

        rst = obj.getWOData(lngProviderID, dteWOOnBefore)
        obj = Nothing

        'write everything off
        MsgBox("does nothing")
        ' how does a dll display a msgbox


        For lngCtr = 1 To rst.RecordCount
            '        Insert rst.Fields("fldBRID").Value, strWOBy, CONST_WO, rst.Fields("fldAmtOutstanding").Value, _
            '               0, "", 0, "Uncollectable Account", False, False, False
            rst.MoveNext()
        Next lngCtr


        rst = Nothing

    End Sub
    Public Function getDelinquentDenials(ByVal lngProviderID As Long, ByVal lngDaysOld As Long) As ADODB.Recordset
        'Date: 09/06/2001
        'Author: Eric Pena
        'Description:  Returns posting paramaters to use in writing off denied w/no activity in over the specified # of days
        '                   for the specified provider (-1 for all)
        'Parameters: lngProviderID - Provider to search outstanding claims for (-1 for all)
        '           lngDaysOld - # of inactivity days to filter for
        'Returns:
        '--------------------------------------------------------------------

        Dim objDenial As New PostingDB.CTxDB
        objDenial.ConnectionString = _ConnectionString

        getDelinquentDenials = objDenial.getDelinquentDenials(lngProviderID, lngDaysOld)


        objDenial = Nothing

    End Function

    Public Function FetchByID(ByVal lngTxID As Long) As ADODB.Recordset
        '-------------------------------------------------------------------------------------
        'Date: 03/11/2002
        'Author: Eric Pena
        'Description:   Retrieves records from the tblTransaction utilizing a stored procedure.
        'Parameters:    lngELID- the PK to return records for.
        'Returns: ADODB.Recordset
        '-------------------------------------------------------------------------------------
        Dim objTx As New PostingDB.CTxDB

        objTx.ConnectionString = _ConnectionString



        FetchByID = objTx.FetchByID(lngTxID)


        objTx = Nothing



    End Function


End Class
