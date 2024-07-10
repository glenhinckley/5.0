
'--------------------------------------------------------------------
'Class Name: CBRBz
'Date: 08/31/2000
'Author: Eric Pena
'Description:  MTS business object designed to   methods associated  with the CBRDB class.
'--------------------------------------------------------------------
Option Explicit On
Imports Psyquel.BusinessRules.CoreLibraryIII


Public Class CBRBz

    Private _ConnectionString As String = String.Empty
    Private Const CLASS_NAME As String = "CBRBz"
    Private Const TABLE_NAME As String = "tblBillingResponsibility"


    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property




    Public Function Insert(ByVal lngELID As Long, ByVal lngPatientID As Long, _
                ByVal lngRPID As Long, ByVal lngPlanID As Long, ByVal lngCPCID As Long, _
                ByVal lngInsuranceID As Long, ByVal strPayerCode As String, _
                ByVal intOrder As Integer, ByVal dblAmtResp As Double, _
                ByVal dblAmtAcct As Double, ByVal lngTaxID As Long, ByVal strTIN As String, _
                ByVal intTINType As Integer, ByVal strCertNum As String) As Long
        '--------------------------------------------------------------------
        'Date: 07/08/2002
        'Author: Dave Richkun
        'Description:  Inserts a row in the tblBillingResponsibility table utilizing a stored procedure.
        'Parameters:
        '
        '   *** TO DO: Define each parameter once procedure is finalized ****
        '
        'Returns: ID of the row modified.
        '--------------------------------------------------------------------

        Dim objBR As New BillingDB.CBRDB


        Insert = objBR.Insert(lngELID, lngPatientID, lngRPID, lngPlanID, lngCPCID, _
                lngInsuranceID, strPayerCode, intOrder, dblAmtResp, dblAmtAcct, _
                lngTaxID, strTIN, intTINType, strCertNum)
        objBR = Nothing



    End Function

    Public Sub Delete(ByVal lngBRID As Long)
        '--------------------------------------------------------------------
        'Date: 03/20/2009
        'Author: Duane C Orth
        'Description:  Physi y deletes a row in tblBillingResponsibilty table utilizing
        '              a stored procedure.
        'Parameters: lngTxID - ID of transaction to be deleted
        'Returns: Null
        '--------------------------------------------------------------------

        Dim objBR As New BillingDB.CBRDB


        objBR.Delete(lngBRID)
        objBR = Nothing


    End Sub

    Public Sub Update(ByVal lngBRID As Long, ByVal lngRPID As Long, ByVal lngELID As Long, ByVal lngPlanID As Long, ByVal lngCPCID As Long, _
                      ByVal lngInsuranceID As Long, ByVal dblAmtResp As Double, _
                      ByVal dblAmtAcctd As Double, ByVal strTIN As String, ByVal intTINType As Integer, _
                      ByVal lngTaxID As Long, ByVal strCertNum As String)
        '--------------------------------------------------------------------
        'Date: 08/02/2002
        'Author: Dave Richkun
        'Description:  Updates a row in tblBillingResponsibility utilizing a stored procedure.
        'Parameters:
        '
        '  ** TO DO ** - Define parameters once procedure finalized
        '
        'Returns: Null
        '--------------------------------------------------------------------

        Dim objBR As New BillingDB.CBRDB

        objBR.Update(lngBRID, lngRPID, lngELID, lngPlanID, lngCPCID, lngInsuranceID, dblAmtResp, dblAmtAcctd, strTIN, intTINType, lngTaxID, strCertNum)
        objBR = Nothing



    End Sub


    Public Sub UpdatePatientAmount(ByVal lngELID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 08/05/2002
        'Author: Dave Richkun
        'Description:  Updates the Amount Responsible and Amount Accountable columns
        '              in tblBillingResponsibility for a given encounter.
        'Parameters: lngELID - ID of the encounter log for which patient balances are
        '              being updated.
        'Returns: Null
        '--------------------------------------------------------------------

        Dim objBR As New BillingDB.CBRDB


        objBR.UpdatePatientAmount(lngELID, strUserName)
        objBR = Nothing



    End Sub


    Public Sub Resubmit(ByVal lngOrigELID As Long, ByVal lngNewELID As Long)
        '--------------------------------------------------------------------
        'Date: 08/08/2002
        'Author: Dave Richkun
        'Description:  Reassigns the Encounter Log ID foreign key from the RP's billing
        '              responsibility records to a new Encounter Log ID.  This
        '              method is used when a claim is resubmitted and a new encounter
        '              is created.  By reassigning the encounter log ID, the original
        '              patient billing responsibility records are preserved when assigned
        '              to the new encounter.
        'Parameters:  lngOrigELID - ID of the original encounter - the ID of the encounter
        '               being cancelled
        '             lngNewELID - ID of the resubmitted encounter - the ID of the new
        '               encounter that supercedes all previous encounters.
        'Returns: Null
        '--------------------------------------------------------------------

        Dim objBR As New BillingDB.CBRDB


        objBR.Resubmit(lngOrigELID, lngNewELID)
        objBR = Nothing



    End Sub


    Public Sub ZeroAll(ByVal lngELID As Long)
        '--------------------------------------------------------------------
        'Date: 08/05/2002
        'Author: Dave Richkun
        'Description:  Updates the Amount Responsible, Amount Accountable, and
        '              Balance columns for all responsible parties in
        '              tblBillingResponsibility for a given encounter.
        'Parameters: lngELID - ID of the encounter log for which balances are
        '              being updated.
        'Returns: Null
        '--------------------------------------------------------------------

        Dim objBR As New BillingDB.CBRDB


        objBR.ZeroAll(lngELID)
        objBR = Nothing


    End Sub
    Public Function Exists(ByVal lngELID As Long, ByVal lngRPID As Long, ByVal lngPlanID As Long, ByVal blnPayerIsPlan As Boolean) As Boolean
        '--------------------------------------------------------------------
        'Date: 08/31/2000
        'Author: Eric Pena
        'Description:  Determines if the BusinessName identical to the
        '              strBusinessName parameter already exists in the table.
        'Parameters: strBusinessName - Business name to be checked
        'Returns: True if the name exists, false otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        '
        Dim objBR As New BillingDB.CBRDB
        Dim blnExists As Boolean

        blnExists = objBR.Exists(lngELID, lngRPID, lngPlanID, blnPayerIsPlan)

        Exists = blnExists

        objBR = Nothing


    End Function
    Public Function Fetch(ByVal lngELID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/31/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblBillingResponsibility and tblInsCompany tables utilizing a stored procedure
        'Parameters: blnIncludeDisabled - determines if disabled records will be returned
        'Returns: ADORst
        '--------------------------------------------------------------------
        Dim objBR As New BillingDB.CBRDB


        Fetch = objBR.Fetch(lngELID)


        objBR = Nothing

    End Function
    Public Sub SwapOrder(ByVal lngID1 As Long, ByVal lngID2 As Long)
        '--------------------------------------------------------------------
        'Date: 02/24/2000
        'Author: Eric Pena
        'Description:  Swaps the fldOrder Field of two entries in tblBillingResponsibility
        'Parameters: Each parameter identifies the column value that will be swapped.
        'Returns: Null
        '--------------------------------------------------------------------
        Dim objBR As New BillingDB.CBRDB
        Dim strError As String


        strError = ""

        If Not SameBill(lngID1, lngID2) Then
            strError = "Both payers must be on the same bill"
            Exit Sub
        End If

        objBR.SwapOrder(lngID1, lngID2)


        objBR = Nothing

    End Sub

    '''Public Sub TransferResponsibility(ByVal lngBRIDSource As Long, ByVal lngBRIDDest As Long, ByVal curAmount As Decimal, Optional ByVal blnAdjustBalance As Boolean = False, _
    '''            Optional ByVal strUserName As String = "")
    ''''--------------------------------------------------------------------
    ''''Date: 02/24/2000
    ''''Author: Eric Pena
    ''''Description:  Moves the responsibility from one entry in tblBillingresponsibility to another
    ''''Parameters: lngBRIDFrom -  key of the BR record to move resp. from.
    ''''                   lngBRIDTo -  key of the BR record to move resp. to or -1 to move all remaining balances to the next payer
    ''''                   curAmount  - The amount of resp. to move
    ''''Returns: Null
    ''''--------------------------------------------------------------------
    '''     Dim objBR As New BillingDB.CBRDB
    '''     Dim objCred As PostingBz.CRPCredBz
    '''
    '''     Dim rstBR As ADODB.Recordset
    '''     Dim rstCred As ADODB.Recordset
    '''     Dim strError As String
    '''
    '''     :
    '''
    '''     strError = ""
    '''     Set objBR = CreateObjectXXXXXXX("BillingDB.CBRDB")
    '''
    '''    'check data
    '''    If curAmount <= 0 And lngBRIDDest > 0 Then
    '''        strError = "You must specify a positive amount to transfer"
    '''       Exit Function
    '''    End If
    '''    If lngBRIDDest > 0 Then
    '''        If Not SameBill(lngBRIDSource, lngBRIDDest) Then
    '''            strError = "The source and destination payers must be on the same bill"
    '''           Exit Function
    '''        End If
    '''    End If
    '''
    '''    If objBR.isAnEAP(lngBRIDSource, lngBRIDDest) Then
    '''        strError = "Cannot transfer balances when an EAP is involved!"
    '''       Exit Function
    '''    End If
    '''
    '''     objBR.TransferResponsibility lngBRIDSource, lngBRIDDest, curAmount
    '''
    '''     'adjust balance, if applicable
    '''     If blnAdjustBalance Then
    '''        Set rstBR = objBR.FetchByID(lngBRIDSource)
    '''        If Not IsNull(rstBR.Fields("fldPayerID_Plan").Value) Then
    '''            Set rstBR = objBR.FetchByID(lngBRIDDest)
    '''            'we do not adjust for plans - exit!
    '''            If Not IsNull(rstBR.Fields("fldPayerID_Plan").Value) Then GoTo Cleanup
    '''        End If
    '''
    '''        Set objCred = CreateObjectXXXXXXX("PostingBz.CRPCredBz")
    '''
    '''        If rstBR.Fields("fldAmtOutstanding").Value < 0 Then
    '''            'move to credit
    '''            objCred.StoreCredit -1, rstBR.Fields("fldBRID").Value, Abs(rstBR.Fields("fldAmtOutstanding").Value), strUserName
    '''        ElseIf rstBR.Fields("fldAmtOutstanding").Value > 0 Then
    '''            'apply a credit
    '''            Set rstCred = objCred.FetchByEL(rstBR.Fields("fldencounterlogid").Value)
    '''            If Not rstCred.EOF Then
    '''                If rstCred.Fields("fldAmount").Value > 0 Then
    '''                    If rstCred.Fields("fldAmount").Value < rstBR.Fields("fldAmtOutstanding").Value Then
    '''                        objCred.ApplyCredit rstCred.Fields("fldPatProvCreditID").Value, rstBR.Fields("fldBRID").Value, rstCred.Fields("fldAmount").Value, strUserName
    '''                    Else
    '''                        objCred.ApplyCredit rstCred.Fields("fldPatProvCreditID").Value, rstBR.Fields("fldBRID").Value, rstBR.Fields("fldAmtOutstanding").Value, strUserName
    '''                    End If
    '''                End If
    '''            End If
    '''        End If
    '''
    '''        Set objCred = Nothing
    '''        Set rstCred = Nothing
    '''     End If
    '''
    '''    'Signal successful completion
    '''Cleanup:
    '''    GetObjectContext.SetComplete
    '''    Set objBR = Nothing
    '''    Set rstBR = Nothing
    '''    Set objCred = Nothing
    '''    Set rstCred = Nothing
    '''Exit Sub
    '''
    '''    'Signal incompletion and raise the error to the  ing environment.
    '''    Set objBR = Nothing
    '''    Set rstBR = Nothing
    '''    Set objCred = Nothing
    '''    Set rstCred = Nothing
    '''    GetObjectContext.SetAbort
    '''    If strError = "" Then
    '''      'err.raise Err.Number, Err.Source, Err.Description
    '''    Else
    '''      'err.raise -1, CLASS_NAME, strError
    '''    End If
    ''' End Sub

    Public Function SameBill(ByVal lngBRID1 As Long, ByVal lngBRID2 As Long) As Boolean
        '--------------------------------------------------------------------
        'Date: 02/24/2000
        'Author: Eric Pena
        'Description:  Determines if two BRIDs are recorded under the same bill
        'Parameters: lngBRID1 / lngBRID2 -  key of the BR record to check
        'Returns: True if both records are under the same bill, False otherwise
        '--------------------------------------------------------------------
        Dim objBR As New BillingDB.CBRDB



        SameBill = objBR.SameBill(lngBRID1, lngBRID2)

        'Signal successful completion

        objBR = Nothing

    End Function
    Public Function FetchByPatient(ByVal lngPatientID As Long, ByVal blnOutstandingOnlyYN As Boolean) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 9/18/2000
        'Author: Eric Pena
        'Description:  Returns rows from tblBillingResponsibility that have an outstanding balance utilizing a stored procedure
        'Parameters: lngPatientID - returns BR records for this patient only
        'Returns: ADORst
        '--------------------------------------------------------------------
        Dim objBR As New BillingDB.CBRDB

        FetchByPatient = objBR.FetchByPatient(lngPatientID, blnOutstandingOnlyYN)


        objBR = Nothing

    End Function
    Public Function FetchByPayerPatientBalance(ByVal lngInsuranceID As Long, ByVal dteStartDOS As Date, ByVal dteEndDOS As Date) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 9/18/2000
        'Author: Eric Pena
        'Description:  Returns rows from tblBillingResponsibility that have an outstanding balance utilizing a stored procedure
        'Parameters: lngPayerID - returns BR records for this payer only
        '                   blnPayerIsPlanYN - true if specified payer is a plan, false if it is a rp
        'Returns: ADORst
        '--------------------------------------------------------------------
        Dim objBR As New BillingDB.CBRDB

        FetchByPayerPatientBalance = objBR.FetchByPayerPatientBalance(lngInsuranceID, dteStartDOS, dteEndDOS)

        objBR = Nothing

    End Function
    Public Function FetchByPatientProvider(ByVal lngPatientID As Long, ByVal lngProviderID As Long, ByVal blnOutstandingOnlyYN As Boolean) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 9/18/2000
        'Author: Eric Pena
        'Description:  Returns rows from tblBillingResponsibility that have an outstanding balance utilizing a stored procedure
        'Parameters: lngPatientID - returns BR records for this patient only
        'Returns: ADORst
        '--------------------------------------------------------------------
        Dim objBR As New BillingDB.CBRDB


        FetchByPatientProvider = objBR.FetchByPatientProvider(lngPatientID, lngProviderID, blnOutstandingOnlyYN)


    End Function
    Public Function FetchByID(ByVal lngBRID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/31/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblBillingResponsibility and tblInsCompany tables utilizing a stored procedure
        'Parameters: blnIncludeDisabled - determines if disabled records will be returned
        'Returns: ADORst
        '--------------------------------------------------------------------
        Dim objBR As New BillingDB.CBRDB

        FetchByID = objBR.FetchByID(lngBRID)

        objBR = Nothing

    End Function
    Public Function isAnEAP(ByVal lngBRIDToTest1 As Long, Optional ByVal lngBRIDToTest2 As Long = -1) As Boolean
        '--------------------------------------------------------------------
        'Date: 03/15/2001
        'Author: Eric Pena
        'Description:  Checks to see if an EAP is linked to either BR record specified
        'Parameters: lngBRIDToTest1/lngBRIDToTest2 -  key of the BR record to test for EAP linkage
        'Returns: True if either BRID links to an EAP, False otherwise
        '--------------------------------------------------------------------
        Dim objBR As New BillingDB.CBRDB


        isAnEAP = False

        isAnEAP = objBR.isAnEAP(lngBRIDToTest1, lngBRIDToTest2)

        objBR = Nothing

    End Function
    Public Sub UpdateCert(ByVal lngELID As Long, ByVal lngRPID As Long, ByVal lngPlanID As Long, ByVal strCertNum As String)
        '--------------------------------------------------------------------
        'Date: 08/02/2002
        'Author: Eric Pena
        'Description:  Updates a cert in tblBillingResponsibility utilizing a stored procedure.
        'Parameters:
        'Returns: Null
        '--------------------------------------------------------------------

        Dim objBR As New BillingDB.CBRDB

        objBR.UpdateCert(lngELID, lngRPID, lngPlanID, strCertNum)
        objBR = Nothing


    End Sub


End Class
