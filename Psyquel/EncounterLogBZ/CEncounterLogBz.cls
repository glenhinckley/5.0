'----------------------------------------
'Class Name: CEncounterLogBz
'Date: 03/06/2000
'Author: Eric Pena
'Denis J Finkel
'Description:  MTS business object designed to   methods associated
'              with the CEncounterLogDB class.
'--------------------------------------------------------------------
'Revision History:
'  R001 07/13/2000 Richkun: Removed   to Exists() from VerifyData().
'  R002 08/07/2000 Richkun: Added FetchForPatient() method
'  R003 08/09/2000 Richkun: Added FetchByID(), FetchByUserEnteredToday() methods
'  R004 09/22/2000 Segura: Added optional lngApptID parameter to Insert()
'  R005 09/30/2000 Richkun: Added support for patient payments
'  R006 10/03/2000 Pena: Eliminated extraneous Patient->provider check in VerifyData(), added a check to allow clinic admins to enter dupe encounters
'  R007 10/03/2000 Pena: Added an optional parameter to Intert() method to allow for old DOS to be entered iif it is for an opening balance record
'  R008 12/11/2000 Nichol: Added validation in Delete() to prevent deletion of record with associated locked patient note
'  R009 02/12/2001 Richkun: Added FetchForPatientUser() method
'  R010 02/16/2001 Pena: Added support for web patient posting types
'  R011 03/01/2001 Pena: Removed 45-day time constraint on adding old encounters
'  R012 03/01/2001 Segura:  Added FetchSeekNAssign() Method
'  R013 03/23/2001 Pena:  Added functionality for no-show billing
'  R014 08/01/2001 Richkun:  Added support for fldModifier column
'  R015 08/07/2001 Richkun:  Added support for fldReferPhy, fldReferPhyID columns
'  R016 12/27/2001 Richkun:  Changed business rule in VerifyData() where Fee must be <= 0 to < 0
'  R017 05/23/2002 Richkun:  Added support for fldAdmitDate, fldDischargeDate columns
'  R018 07/08/2002 Richkun:  Revamped entire module as part of billing redesign
'  R019 12/05/2002 Richkun:  Altered Insert() method so that 'HCFA Queued' transaction is produced for all insurance claims, even if $0.00 applied to Plan
'  R020 03/05/2003 Richkun:  Added FetchRejectionsByProvider() to retrieve provider electronic claim rejections
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






Public Class CEncounterLogBz


    Private _ConnectionString As String = String.Empty



    Private Const CLASS_NAME As String = "CEncounterLogBz"
    Private Const TABLE_NAME As String = "tblEncounterLog"
    Private Const PLAN_SELFPAY As Long = 331
    Private Const INS_COMPANY_EAP As Long = 103
    Private Const PAYER_CODE_RP As String = "R"
    Private Const PAYER_CODE_INS As String = "I"
    Private Const TX_CHARGE As String = "C"
    Private Const TX_PYMT As String = "P"
    Private Const TX_TYPE_COPAY As Integer = 281
    Private Const TX_TYPE_HCFAQUEUED As Integer = 3
    Private Const TX_TYPE_HCFA_UPDATED As Integer = 285
    Private Const TX_TYPE_HCFA_CANCELLED As Integer = 286
    Private Const TX_TYPE_INS_CHG As Integer = 1
    Private Const APPT_STATUS_ATTENDED As Integer = 3
    Private Const CLAIM_STATUS_QUEUED As Integer = 1
    Private Const CLAIM_STATUS_BILLED As Integer = 2
    Private Const CLAIM_STATUS_DENIED As Integer = 4
    Private Const CLAIM_STATUS_APPEAL As Integer = 9
    Private Const CLAIM_STATUS_EAP As Integer = 14
    Private Const CLAIM_STATUS_SELF_PAY As Integer = 15
    Private Const CLAIM_STATUS_NOCHARGE As Integer = 16
    Private Const CLAIM_STATUS_SUBMITTED As Integer = 11
    Private Const CLAIM_STATUS_REJECTED As Integer = 20
    Private Const CLAIM_STATUS_PATIENT_RESP As Integer = 21
    Private Const CLAIM_STATUS_PROVIDER As Integer = 23
    Dim ds As New db


    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property



    Public Function Insert(ByVal intVersionNum As Integer, ByVal lngPatientID As Long, _
                ByVal lngProviderID As Long, ByVal lngClinicID As Long, ByVal dtDOS As Date, _
                ByVal strCPTCode As String, ByVal strModifier1 As String, ByVal strModifier2 As String, _
                ByVal strDSMIV As String, ByVal dblFee As Double, ByVal dblUnits As Double, _
                ByVal strAddOnCPTCode As String, ByVal strAddOnMod1 As String, ByVal strAddOnMod2 As String, _
                ByVal dblAddOnFee As Double, ByVal dblAddOnUnits As Double, _
                ByVal strAddOnSecCPTCode As String, ByVal strAddOnSecMod1 As String, ByVal strAddOnSecMod2 As String, _
                ByVal dblAddOnSecFee As Double, ByVal dblAddOnSecUnits As Double, _
                ByVal aryPlans As Object, ByVal strReferPhyLast As String, ByVal strReferPhyFirst As String, ByVal strReferPhyMI As String, _
                ByVal strReferPhyTitle As String, ByVal strReferPhy As String, ByVal strReferPhyNPI As String, _
                ByVal strReferPhyID As String, ByVal dtAdmitDate As Date, ByVal dtDischargeDate As Date, _
                ByVal dtDateOfOnset As Date, ByVal dblCopayChg As Double, ByVal dblCopayPaid As Double, _
                ByVal dtCopayDtRcvd As Date, ByVal strCondEmpYN As String, ByVal strCondAutoAccYN As String, _
                ByVal strCondAutoAccState As String, ByVal strOtherAccYN As String, _
                ByVal strDoNotBillYN As String, ByVal streScribeYN As String, ByVal lngTxTypeID As Long, _
                ByVal strCheckNum As String, ByVal dtCheckDate As Date, ByVal strCCTxID As String, ByVal strCopayNotes As String, _
                ByVal lngApptID As Long, ByVal lngPatApptID As Long, ByVal lngApptStatusID As Long, _
                ByVal dblAmtRespRP As Double, ByVal dblAmtRespPlan As Double, _
                ByVal blnRecur As Boolean, ByVal strNoShowFlag As String, ByVal lngPrevELID As Long, _
                ByVal lngUserID As Long, ByVal strUserName As String) As Long
        '--------------------------------------------------------------------
        'Date: 07/08/2002
        'Author: Dave Richkun
        'Description:  Inserts rows into claim-related tables as needed.  The end result of this
        '              function is to create a record of a claim, whether it be an insurance claim,
        '              a Self-Pay claim, or an EAP claim.  This method is not designed to manage
        '              appointment No-Shows (unless the No-Show is charged to the insurance company)
        '              or appointment cancellations.
        'Parameters:
        '
        '       **** TO DO ****: Define parameters when function finalized
        '
        'Returns: Encounter Log ID (aka Claim ID) of the data inserted.
        '--------------------------------------------------------------------
        Dim objEL As New EncounterLogDB.CEncounterLogDB
        Dim objBFact As New BenefactorBz.CPatRPPlanBz
        Dim objBR As New BillingBZ.CBRBz
        Dim objPatTx As New BillingBZ.CPatientTxBz
        Dim objInsTx As New BillingBZ.CInsuranceTxBz
        Dim objCert As New CertificationBz.CCertificationBz
        Dim objClaim As New ClaimBz.CClaimBz
        Dim objPatAppt As New ApptBZ.CPatApptBZ
        Dim objAppt As New ApptBZ.CApptBZ
        Dim rstBR As New ADODB.Recordset
        Dim lngELID As Long
        Dim lngPrimaryPlanID As Long
        Dim lngPrimaryRPID As Long
        Dim lngPrimaryCPCID As Long
        Dim lngPrimaryInsID As Long
        '   Dim dblAmtRespPlan As Double
        '   Dim dblAmtRespRP As Double
        Dim intOrder As Integer
        Dim strErrMsg As String
        Dim blnPrintHCFA As Boolean
        Dim intClaimStatus As Integer
        Dim intCtr As Integer
        Dim rstTemp As New ADODB.Recordset

        Dim objCPT As New ListBZ.CCPTCodeBz



        objEL.ConnectionString = _ConnectionString
        objBFact.ConnectionString = _ConnectionString
        objBR.ConnectionString = _ConnectionString
        objPatTx.ConnectionString = _ConnectionString
        objInsTx.ConnectionString = _ConnectionString
        objCert.ConnectionString = _ConnectionString
        objClaim.ConnectionString = _ConnectionString
        objInsTx.ConnectionString = _ConnectionString
        objPatAppt.ConnectionString = _ConnectionString
        objAppt.ConnectionString = _ConnectionString



        Dim rs As New ADODB.Recordset
        Dim lngCptRecordID As Long

        rs = objCPT.Fetch(True, " fldCPTCode = '" & strCPTCode & "' ", , -1)
        objCPT = Nothing
        lngCptRecordID = ds.IfNull(rs.Fields("fldCptRecordID").Value, 0)
        rs = Nothing



        'Retrieve Plans and Resp Parties associated with patient
        rstBR = objBFact.FetchRPPlansByPat(lngPatientID)
        objBFact = Nothing

        'Ensure the patient has at least 1 active insurance plan.
        If rstBR.RecordCount = 0 Then
            strErrMsg = "This patient does not have any active plans.  You must add an insurance plan to the patient record before you can submit a claim."
            Exit Function
        End If

        blnPrintHCFA = True 'Assume a HCFA will be generated.  Allow circumstances in this method to override this setting.

        'The Plans array is not guaranteed to be sorted by Order.  We must ensure the array
        'is sorted before inserting Billing Responsibility records. An error was occuring because the Plan was not found.
        aryPlans = SortPlanArray(aryPlans)

        'Store values from primary plan
        For intCtr = 1 To rstBR.RecordCount
            If rstBR.Fields("fldPatientID").Value = lngPatientID And _
               rstBR.Fields("fldPlanID").Value = CLng(aryPlans(0, 1)) Then
                lngPrimaryPlanID = rstBR.Fields("fldPlanID").Value 'Store ID of primary insurance plan
                lngPrimaryRPID = rstBR.Fields("fldRPID").Value  'Store ID of Resp Party
                lngPrimaryCPCID = rstBR.Fields("fldCPCID").Value 'Store ID of primary CPC
                lngPrimaryInsID = rstBR.Fields("fldInsuranceID").Value  'Store ID of primary insurance company
                Exit For
            End If
            rstBR.MoveNext()
        Next intCtr

        'Ensure the patient has the insurance plan that was selected.
        If lngPrimaryPlanID = 0 Then
            strErrMsg = "This patient insurance plans are not set up correctly, Insurance plans must be corrected before you can submit a claim. " & lngPatientID & ":" & lngPrimaryPlanID & ":" & CLng(aryPlans(0, 1))
            Exit Function
        End If

        'If the provider elects to offer services free of charge, then the dblFee parameter will
        'be equal to zero.  All parties are void from any responsibilty in this case, and no claim
        'will be generated.
        If dblFee = 0 Then
            dblAmtRespPlan = 0
            '     dblAmtRespRP = 0
            blnPrintHCFA = False

            'We still need to identify claim status, even if serviced at no charge
            If lngPrimaryInsID = INS_COMPANY_EAP Then
                intClaimStatus = CLAIM_STATUS_EAP
                '     dblCopayChg = 0 'Not allowed to charge patient for EAP claims
            Else
                Select Case lngPrimaryPlanID
                    Case PLAN_SELFPAY
                        intClaimStatus = CLAIM_STATUS_SELF_PAY 'Change claim status
                        If strDoNotBillYN = "Y" Then intClaimStatus = CLAIM_STATUS_NOCHARGE
                    Case Else 'Insurance claim
                        intClaimStatus = CLAIM_STATUS_NOCHARGE
                End Select
            End If
        Else
            'Identify billing responsibility for each primary party.
            If lngPrimaryInsID = INS_COMPANY_EAP Then
                'The EAP is entirely responsible for EAP claims
                '            dblAmtRespPlan = (dblFee * dblUnits) + (dblAddOnFee * dblAddOnUnits) + (dblAddOnSecFee * dblAddOnSecUnits)
                '            dblCopayChg = 0 'Not allowed to charge patient for EAP claims
                'dblCopayChg = dblFee * dblUnits                               '<--Old Code
                '            dblAmtRespRP = 0
                blnPrintHCFA = False
                intClaimStatus = CLAIM_STATUS_EAP
            Else
                Select Case lngPrimaryPlanID
                    Case PLAN_SELFPAY
                        'The Resp Party is entirely responsible for Self-Pay claims
                        '                    dblAmtRespPlan = 0
                        '                    dblAmtRespRP = (dblFee * dblUnits) + (dblAddOnFee * dblAddOnUnits) + (dblAddOnSecFee * dblAddOnSecUnits)
                        blnPrintHCFA = False
                        intClaimStatus = CLAIM_STATUS_SELF_PAY 'Change claim status
                    Case Else 'Insurance claim
                        'Only the primary plan and the primary RP are responsible for payment at this stage.
                        '                   dblAmtRespPlan = ((dblFee * dblUnits) + (dblAddOnFee * dblAddOnUnits) + (dblAddOnSecFee * dblAddOnSecUnits)) - dblCopayChg
                        '                   dblAmtRespRP = dblCopayChg - dblCopayPaid
                        If rstBR.Fields("fldNoHCFA").Value = "Y" Then
                            blnPrintHCFA = False
                        Else
                            blnPrintHCFA = True
                        End If
                        intClaimStatus = CLAIM_STATUS_QUEUED
                End Select
            End If
        End If

        'If the claim originated from a recurring appointment, we must make a 'copy' of
        'the patient appointment before we bill.
        If blnRecur = True Then
            lngApptID = objAppt.CloneInstance(lngApptID, dtDOS, strUserName)

            rstTemp = objAppt.FetchPatientApptByID(lngApptID)
            For intCtr = 1 To rstTemp.RecordCount
                If rstTemp.Fields("fldPatientID").Value = lngPatientID Then
                    lngPatApptID = rstTemp.Fields("fldPatApptID").Value
                    Exit For
                End If
            Next intCtr
            rstTemp = Nothing
            objAppt = Nothing
        End If

        'Insert the Encounter record
        lngELID = objEL.Insert(intVersionNum, lngPatientID, lngProviderID, lngClinicID, _
            dtDOS, strCPTCode, strModifier1, strModifier2, strDSMIV, dblFee, dblUnits, _
            strAddOnCPTCode, strAddOnMod1, strAddOnMod2, dblAddOnFee, dblAddOnUnits, strAddOnSecCPTCode, strAddOnSecMod1, strAddOnSecMod2, dblAddOnSecFee, dblAddOnSecUnits, _
            strReferPhyLast, strReferPhyFirst, strReferPhyMI, strReferPhyTitle, strReferPhy, strReferPhyNPI, _
            strReferPhyID, dtAdmitDate, dtDischargeDate, dtDateOfOnset, strCondEmpYN, strCondAutoAccYN, _
            strCondAutoAccState, strOtherAccYN, strDoNotBillYN, streScribeYN, intClaimStatus, dblAmtRespRP, dblAmtRespPlan, _
            lngPatApptID, lngPrevELID, strUserName)
        objEL = Nothing

        'Insert Billing Responsibilty records.
        Select Case lngPrimaryPlanID
            Case PLAN_SELFPAY
                'The primary RP is the only responsible party wrt to Self-Pay claims
                objBR.Insert(lngELID, lngPatientID, lngPrimaryRPID, lngPrimaryPlanID, _
                  lngPrimaryCPCID, lngPrimaryInsID, PAYER_CODE_RP, 1, dblAmtRespRP, _
                  dblCopayPaid, aryPlans(0, 2), aryPlans(0, 3), aryPlans(0, 4), _
                  ds.IfNull(aryPlans(0, 5), ""))
            Case Else 'Insurance claim
                'The primary RP is always first on the list.
                objBR.Insert(lngELID, lngPatientID, lngPrimaryRPID, lngPrimaryPlanID, _
                  lngPrimaryCPCID, lngPrimaryInsID, PAYER_CODE_RP, 1, dblCopayChg, _
                  dblCopayPaid, -1, "", -1, "")

                'Insert BR record for primary insurance plan
                objBR.Insert(lngELID, lngPatientID, lngPrimaryRPID, lngPrimaryPlanID, _
                  lngPrimaryCPCID, lngPrimaryInsID, PAYER_CODE_INS, 2, dblAmtRespPlan, 0, _
                  aryPlans(0, 2), aryPlans(0, 3), aryPlans(0, 4), ds.IfNull(aryPlans(0, 5), ""))

                'Create a snapshot of patient plans at the time of billing.  Assign all non-primary plans
                'a responsibility amount of zero.
                intOrder = 2
                rstBR.MoveFirst()
                Do While Not rstBR.EOF
                    If Not rstBR.EOF Then
                        If rstBR.Fields("fldInsuranceID").Value <> INS_COMPANY_EAP And rstBR.Fields("fldPlanID").Value <> lngPrimaryPlanID And rstBR.Fields("fldPlanID").Value <> PLAN_SELFPAY Then
                            intOrder = intOrder + 1
                            objBR.Insert(lngELID, lngPatientID, rstBR.Fields("fldRPID").Value, _
                              rstBR.Fields("fldPlanID").Value, rstBR.Fields("fldCPCID").Value, _
                              rstBR.Fields("fldInsuranceID").Value, PAYER_CODE_INS, intOrder, 0, 0, _
                              aryPlans(intOrder - 2, 2), aryPlans(intOrder - 2, 3), _
                              aryPlans(intOrder - 2, 4), ds.IfNull(aryPlans(intOrder - 2, 5), ""))
                        End If
                    End If
                    rstBR.MoveNext()
                Loop
        End Select

        objBR = Nothing
        rstBR = Nothing

        'Insert patient transaction records to mirror Billing Responsibility amounts.  A row
        'is created to record both the charge and the payment.
        If dblCopayChg <> 0 Then
            'Insert RP charge record
            objPatTx.Insert(lngELID, lngPatientID, lngProviderID, lngPrimaryRPID, _
                  TX_CHARGE, TX_TYPE_COPAY, Date.Now, dtDOS, dblCopayChg, "", _
                   "01\01\1900", "", "", -1, strUserName)
        End If

        If dblCopayPaid > 0 Then
            'Insert RP Payment record
            objPatTx.Insert(lngELID, lngPatientID, lngProviderID, lngPrimaryRPID, _
                  TX_PYMT, lngTxTypeID, Date.Now, dtCopayDtRcvd, dblCopayPaid, strCheckNum, _
                  dtCheckDate, strCCTxID, strCopayNotes, -1, strUserName)
        End If
        objPatTx = Nothing

        'Insert insurance transaction records to mirror Billing Responsibility amounts.
        'At this point, only information pertaining to the charge is known.

        If ((dblFee * dblUnits) + (dblAddOnFee * dblAddOnUnits) + (dblAddOnSecFee * dblAddOnSecUnits)) <> 0 Then
            'Insert the insurance company charge record
            objInsTx.Insert(lngELID, 0, lngPatientID, lngPrimaryRPID, lngPrimaryPlanID, _
                  lngPrimaryCPCID, lngPrimaryInsID, TX_CHARGE, TX_TYPE_INS_CHG, lngCptRecordID, _
                  ((dblFee * dblUnits) + (dblAddOnFee * dblAddOnUnits) + (dblAddOnSecFee * dblAddOnSecUnits)), Date.Now, "", "01\01\1900", "", 0, 0, strUserName, lngUserID)
        End If

        If blnPrintHCFA = True Then
            'Insert a zero-dollar transaction to assist with chronological events
            objInsTx.Insert(lngELID, 0, lngPatientID, lngPrimaryRPID, lngPrimaryPlanID, _
              lngPrimaryCPCID, lngPrimaryInsID, TX_PYMT, TX_TYPE_HCFAQUEUED, 0, 0, Date.Now, _
              "", "01\01\1900", "", 0, 0, strUserName, lngUserID)
        End If

        objInsTx = Nothing

        'Deduct certification units for all plans.  Do not deduct cert units if service offered at no charge
        If dblFee > 0 Then
            objCert.DeductUnits(lngELID, lngPatientID, lngProviderID, lngPrimaryPlanID, dtDOS, strCPTCode, dblUnits)
            objCert = Nothing
        End If

        If dblAddOnFee > 0 And strAddOnCPTCode > "" And lngPrimaryPlanID <> 331 Then
            objCert.DeductUnits(lngELID, lngPatientID, lngProviderID, lngPrimaryPlanID, dtDOS, strAddOnCPTCode, dblAddOnUnits)
            objCert = Nothing
        End If

        If dblAddOnSecFee > 0 And strAddOnSecCPTCode > "" And lngPrimaryPlanID <> 331 Then
            objCert.DeductUnits(lngELID, lngPatientID, lngProviderID, lngPrimaryPlanID, dtDOS, strAddOnSecCPTCode, dblAddOnSecUnits)
            objCert = Nothing
        End If

        'Insert claim snapshot for insurance claims where fee charged (i.e. no pro-bono)
        If lngPrimaryInsID <> INS_COMPANY_EAP And lngPrimaryPlanID <> PLAN_SELFPAY And dblFee > 0 And strDoNotBillYN = "N" Then
            objClaim.Insert(lngELID, intVersionNum, lngPrimaryRPID, lngClinicID, lngPrimaryPlanID, _
                              lngPrimaryCPCID, lngPrimaryInsID, 1, strUserName)
            objClaim = Nothing
        End If

        'If the claim is associated with an appointment, update the appointment status
        'to a value of 'Attended'
        If lngPatApptID > 0 Then
            objPatAppt.ChangeStatus(lngPatApptID, lngApptStatusID, strNoShowFlag, 0, "", strUserName)
            objPatAppt = Nothing
        End If

        'Return EncounterLog/Claim ID to  ing procedure
        Insert = lngELID


        objEL = Nothing
        objBFact = Nothing
        objBR = Nothing
        objPatTx = Nothing
        objInsTx = Nothing
        objCert = Nothing
        rstBR = Nothing


    End Function



    Public Sub UpdateBeforeSubmit(ByVal lngELID As Long, ByVal lngPatientID As Long, _
                ByVal lngProviderID As Long, ByVal lngClinicID As Long, ByVal dtDOS As Date, _
                ByVal strCPTCode As String, ByVal strModifier1 As String, ByVal strModifier2 As String, _
                ByVal strDSMIV As String, ByVal dblFee As Double, ByVal dblUnits As Double, _
                ByVal strAddOnCPTCode As String, ByVal strAddOnMod1 As String, ByVal strAddOnMod2 As String, ByVal dblAddOnFee As Double, ByVal dblAddOnUnits As Double, _
                ByVal strAddOnSecCPTCode As String, ByVal strAddOnSecMod1 As String, ByVal strAddOnSecMod2 As String, ByVal dblAddOnSecFee As Double, ByVal dblAddOnSecUnits As Double, _
                ByVal aryPlans As Object, ByVal strReferPhyLast As String, ByVal strReferPhyFirst As String, ByVal strReferPhyMI As String, _
                ByVal strReferPhyTitle As String, ByVal strReferPhy As String, ByVal strReferPhyNPI As String, _
                ByVal strReferPhyID As String, ByVal dtAdmitDate As Date, ByVal dtDischargeDate As Date, _
                ByVal dtDateOfOnset As Date, ByVal strCondEmpYN As String, ByVal strCondAutoAccYN As String, _
                ByVal strCondAutoAccState As String, ByVal strOtherAccYN As String, ByVal strDoNotBillYN As String, _
                ByVal streScribeYN As String, ByVal dblAmtRespRP As Double, ByVal dblAmtRespPlan As Double, _
                ByVal lngPatApptID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 08/01/2002
        'Author: Dave Richkun
        'Description:  Updates rows in claim-related tables as needed.  The end result of this
        '              method is to ensure changes to a claim are consistent across all related
        '              tables.  This method is  ed and is to be used before the claim is
        '              submitted to the payer i.e. before the HCFA is printed or the electronic
        '              claim record is generated.
        'Parameters:
        '
        '       **** TO DO ****: Define parameters when function finalized
        '
        'Returns: Null
        '--------------------------------------------------------------------
        Dim objEL As New EncounterLogDB.CEncounterLogDB
        Dim objBR As New BillingBZ.CBRBz
        Dim objPatTx As New BillingBZ.CPatientTxBz
        Dim objInsTx As New BillingBZ.CInsuranceTxBz
        Dim objCert As New CertificationBz.CCertificationBz
        Dim objClaim As New ClaimBz.CClaimBz
        Dim rstEL As New ADODB.Recordset
        Dim lngPrimaryPlanID As Long
        Dim lngPrimaryRPID As Long
        Dim lngPrimaryCPCID As Long
        Dim lngPrimaryInsID As Long
        Dim lngBRID As Long
        Dim lngClaimID As Long
        Dim intOrder As Integer
        Dim strErrMsg As String
        Dim blnPrintHCFA As Boolean
        Dim intClaimStatus As Integer
        Dim intCtr As Integer
        Dim dblAmtAcctRP As Double

        Dim objCPT As New ListBZ.CCPTCodeBz
        Dim rs As New ADODB.Recordset
        Dim lngCptRecordID As Long

        rs = objCPT.Fetch(True, " fldCPTCode = '" & strCPTCode & "' ", , -1)
        objCPT = Nothing
        lngCptRecordID = ds.IfNull(rs.Fields("fldCptRecordID").Value, 0)
        rs = Nothing



        'Ensure the claim can still be updated
        rstEL = FetchByID(lngELID)
        If rstEL.Fields("fldBillStatusID").Value <> CLAIM_STATUS_QUEUED And rstEL.Fields("fldBillStatusID").Value <> CLAIM_STATUS_EAP And _
               rstEL.Fields("fldBillStatusID").Value <> CLAIM_STATUS_SELF_PAY And rstEL.Fields("fldBillStatusID").Value <> CLAIM_STATUS_NOCHARGE And _
               rstEL.Fields("fldBillStatusID").Value <> CLAIM_STATUS_REJECTED And rstEL.Fields("fldBillStatusID").Value <> CLAIM_STATUS_PROVIDER And _
               rstEL.Fields("fldBillStatusID").Value <> CLAIM_STATUS_SUBMITTED And rstEL.Fields("fldBillStatusID").Value <> CLAIM_STATUS_DENIED Then
            rstEL = Nothing
            strErrMsg = "The claim has already been submitted to the payer.  You can not update this claim."
            Exit Sub
        End If
        lngBRID = rstEL.Fields("fldBRID").Value 'Store ID of patient responsible 'R' record
        lngPrimaryPlanID = rstEL.Fields("fldPlanID").Value 'Store ID of primary insurance plan
        lngPrimaryCPCID = rstEL.Fields("fldCPCID").Value 'Store ID of primary CPC
        lngPrimaryInsID = rstEL.Fields("fldInsuranceID").Value  'Store ID of primary insurance company
        dblAmtAcctRP = rstEL.Fields("fldAmtAcct").Value
        '    lngCptRecordID = rstEL.Fields("fldCPTRecordID").Value
        lngClaimID = 0
        If rstEL.RecordCount > 1 And rstEL.Fields("fldOrder").Value = 1 Then
            rstEL.MoveNext()
            If rstEL.Fields("fldOrder").Value = 2 And ds.IfNull(rstEL.Fields("fldClaimID").Value, 0) > 0 Then
                lngClaimID = rstEL.Fields("fldClaimID").Value
            End If
        End If
        rstEL = Nothing

        'The Plans array is not guaranteed to be sorted by Order.  We must ensure the array
        'is sorted before updating Billing Responsibility records.
        aryPlans = SortPlanArray(aryPlans)
        If lngPrimaryPlanID <> aryPlans(0, 1) Then  'User has changed the order of the Insurance Plans
        End If
        lngPrimaryInsID = aryPlans(0, 8)
        lngPrimaryCPCID = aryPlans(0, 9)
        lngPrimaryPlanID = aryPlans(0, 1)
        lngPrimaryRPID = aryPlans(0, 7)
        blnPrintHCFA = True

        'If the provider elects to offer services free of charge, then the dblFee parameter will
        'be equal to zero.  All parties are void from any responsibilty in this case, and no claim
        'will be generated.
        If dblFee = 0 Then
            dblAmtRespPlan = 0
            '        dblAmtRespRP = 0
            blnPrintHCFA = False

            'We still need to identify claim status, even if serviced at no charge
            If lngPrimaryInsID = INS_COMPANY_EAP Then
                intClaimStatus = CLAIM_STATUS_EAP
            Else
                Select Case lngPrimaryPlanID
                    Case PLAN_SELFPAY
                        intClaimStatus = CLAIM_STATUS_SELF_PAY 'Change claim status
                        If strDoNotBillYN = "Y" Then intClaimStatus = CLAIM_STATUS_NOCHARGE
                    Case Else 'Insurance claim
                        intClaimStatus = CLAIM_STATUS_NOCHARGE
                End Select
            End If
        Else
            'Identify billing responsibility amounts for each primary party.
            If lngPrimaryInsID = INS_COMPANY_EAP Then
                'The EAP is entirely responsible for EAP claims
                '            dblAmtRespPlan = (dblFee * dblUnits) + (dblAddOnFee * dblAddOnUnits) + (dblAddOnSecFee * dblAddOnSecUnits)
                '            dblAmtRespRP = 0
                blnPrintHCFA = False
                intClaimStatus = CLAIM_STATUS_EAP
            Else
                Select Case lngPrimaryPlanID
                    Case PLAN_SELFPAY
                        'The Resp Party is entirely responsible for Self-Pay claims
                        '                    dblAmtRespPlan = 0
                        '                    dblAmtRespRP = (dblFee * dblUnits) + (dblAddOnFee * dblAddOnUnits) + (dblAddOnSecFee * dblAddOnSecUnits)
                        blnPrintHCFA = False
                        intClaimStatus = CLAIM_STATUS_SELF_PAY 'Change claim status
                    Case Else 'Insurance claim
                        'Only the primary plan and the primary RP are responsible for payment at this stage.
                        '                    If ((dblFee * dblUnits) + (dblAddOnFee * dblAddOnUnits) + (dblAddOnSecFee * dblAddOnSecUnits)) <= dblAmtRespRP Then
                        '                        dblAmtRespPlan = 0
                        '                    Else
                        '                        dblAmtRespPlan = ((dblFee * dblUnits) + (dblAddOnFee * dblAddOnUnits) + (dblAddOnSecFee * dblAddOnSecUnits)) - dblAmtRespRP
                        '                    End If
                        'dblAmtRespRP remains untouched
                        'blnPrintHCFA remains untouched

                        intClaimStatus = CLAIM_STATUS_QUEUED
                End Select
            End If
        End If

        'Rollback Certification units for the original claim before Updateing the claim
        objCert.RollBack(lngELID, strUserName)

        'Update the Encounter record
        objEL.Update(lngELID, lngPatientID, lngProviderID, lngClinicID, dtDOS, _
              strCPTCode, strModifier1, strModifier2, strDSMIV, dblFee, dblUnits, _
              strAddOnCPTCode, strAddOnMod1, strAddOnMod2, dblAddOnFee, dblAddOnUnits, strAddOnSecCPTCode, strAddOnSecMod1, strAddOnSecMod2, dblAddOnSecFee, dblAddOnSecUnits, _
              strReferPhyLast, strReferPhyFirst, strReferPhyMI, strReferPhyTitle, strReferPhy, strReferPhyNPI, _
              strReferPhyID, dtAdmitDate, dtDischargeDate, dtDateOfOnset, strCondEmpYN, strCondAutoAccYN, _
              strCondAutoAccState, strOtherAccYN, strDoNotBillYN, streScribeYN, intClaimStatus, dblAmtRespRP - dblAmtAcctRP, _
              dblAmtRespPlan, lngPatApptID, strUserName)
        objEL = Nothing

        'Update Billing Responsibility record for the primary plan.  Because we are updating before
        'the HCFA has been generated, only the balance owing for the BR record associated with the
        'primary insurance plan needs to be updated.  We are not touching the BR record associated
        'with the primary RP, unless the claim is Self-Pay.
        If lngPrimaryPlanID = PLAN_SELFPAY Then
            objBR.Update(lngBRID, aryPlans(0, 7), lngELID, aryPlans(0, 1), aryPlans(0, 9), aryPlans(0, 8), dblAmtRespRP, -1, aryPlans(0, 3), aryPlans(0, 4), aryPlans(0, 2), aryPlans(0, 5))
        Else
            objBR.Update(lngBRID, aryPlans(0, 7), lngELID, aryPlans(0, 1), aryPlans(0, 9), aryPlans(0, 8), dblAmtRespRP, -1, aryPlans(0, 3), aryPlans(0, 4), aryPlans(0, 2), aryPlans(0, 5))
            objBR.Update(aryPlans(0, 6), aryPlans(0, 7), lngELID, aryPlans(0, 1), aryPlans(0, 9), aryPlans(0, 8), dblAmtRespPlan, -1, aryPlans(0, 3), aryPlans(0, 4), aryPlans(0, 2), aryPlans(0, 5))
            'Update secondary and other insurance plan rows.  The user may have changed the TaxID.
            If UBound(aryPlans, 1) >= 1 Then
                For intCtr = 1 To UBound(aryPlans, 1)
                    objBR.Update(aryPlans(intCtr, 6), aryPlans(intCtr, 7), lngELID, aryPlans(intCtr, 1), aryPlans(intCtr, 9), aryPlans(intCtr, 8), 0, -1, aryPlans(intCtr, 3), aryPlans(intCtr, 4), aryPlans(intCtr, 2), aryPlans(intCtr, 5))
                Next intCtr
            End If
        End If
        objBR = Nothing

        'We only touch patient transactions if the claim is Self-Pay
        If lngPrimaryPlanID = PLAN_SELFPAY Then
            objPatTx.UpdateSelfPayCharge(lngELID, dblAmtRespRP, strUserName)
            objPatTx = Nothing
        End If

        'Update insurance transaction records to mirror Billing Responsibility amounts.
        'At this point, only information pertaining to the charge is known.

        If blnPrintHCFA Then
            'Insert a zero-dollar 'HCFA Updated' transaction to assist with chronological events
            objInsTx.Insert(lngELID, 0, lngPatientID, lngPrimaryRPID, lngPrimaryPlanID, _
              lngPrimaryCPCID, lngPrimaryInsID, TX_PYMT, TX_TYPE_HCFA_UPDATED, lngCptRecordID, 0, Date.Now, _
              "", "01\01\1900", "", 0, 0, strUserName)
        End If

        'Update the primary insurance company charge record
        objInsTx.Update(lngELID, 0, lngPrimaryPlanID, TX_TYPE_INS_CHG, lngCptRecordID, (dblFee * dblUnits) + (dblAddOnFee * dblAddOnUnits) + (dblAddOnSecFee * dblAddOnSecUnits), Date.Now, "", "01\01\1900", "", 0, 0, strUserName)
        objInsTx = Nothing

        'Deduct certification units for all plans.  Do not deduct cert units if service offered at no charge
        If dblFee > 0 Then
            objCert.DeductUnits(lngELID, lngPatientID, lngProviderID, lngPrimaryPlanID, dtDOS, strCPTCode, dblUnits)
        End If
        If dblAddOnFee > 0 And strAddOnCPTCode > "" Then
            objCert.DeductUnits(lngELID, lngPatientID, lngProviderID, lngPrimaryPlanID, dtDOS, strAddOnCPTCode, dblAddOnUnits)
        End If
        If dblAddOnSecFee > 0 And strAddOnSecCPTCode > "" Then
            objCert.DeductUnits(lngELID, lngPatientID, lngProviderID, lngPrimaryPlanID, dtDOS, strAddOnSecCPTCode, dblAddOnSecUnits)
        End If
        objCert = Nothing

        If lngClaimID <= 0 Then
            'Delete the original claim record before creating a new claim.
            objClaim.DeleteBeforeSubmit(lngELID)

            'Insert claim snapshot for insurance claims where fee charged (i.e. no pro-bono)
            If lngPrimaryInsID <> INS_COMPANY_EAP And lngPrimaryPlanID <> PLAN_SELFPAY And dblFee > 0 And strDoNotBillYN = "N" Then
                objClaim.Insert(lngELID, 1, lngPrimaryRPID, lngClinicID, lngPrimaryPlanID, _
                              lngPrimaryCPCID, lngPrimaryInsID, 1, strUserName)
            End If
        Else    'Claim Exists
            objClaim.GenerateClaim(lngClaimID, True, strUserName)
        End If
        objClaim = Nothing


        objEL = Nothing
        objBR = Nothing
        objPatTx = Nothing
        objInsTx = Nothing
        objCert = Nothing
        objClaim = Nothing



    End Sub


    Public Function ReSubmit(ByVal lngELID As Long, ByVal intVersionNum As Integer, ByVal lngPatientID As Long, _
                ByVal lngProviderID As Long, ByVal lngClinicID As Long, ByVal dtDOS As Date, _
                ByVal strCPTCode As String, ByVal strModifier1 As String, ByVal strModifier2 As String, _
                ByVal strDSMIV As String, ByVal dblFee As Double, ByVal dblUnits As Double, _
                ByVal strAddOnCPTCode As String, ByVal strAddOnMod1 As String, ByVal strAddOnMod2 As String, ByVal dblAddOnFee As Double, ByVal dblAddOnUnits As Double, _
                ByVal strAddOnSecCPTCode As String, ByVal strAddOnSecMod1 As String, ByVal strAddOnSecMod2 As String, ByVal dblAddOnSecFee As Double, ByVal dblAddOnSecUnits As Double, _
                ByVal aryPlans As Object, ByVal strReferPhyLast As String, ByVal strReferPhyFirst As String, ByVal strReferPhyMI As String, _
                ByVal strReferPhyTitle As String, ByVal strReferPhy As String, ByVal strReferPhyNPI As String, _
                ByVal strReferPhyID As String, ByVal dtAdmitDate As Date, ByVal dtDischargeDate As Date, _
                ByVal dtDateOfOnset As Date, ByVal strCondEmpYN As String, ByVal strCondAutoAccYN As String, _
                ByVal strCondAutoAccState As String, ByVal strOtherAccYN As String, ByVal strDoNotBillYN As String, _
                ByVal streScribeYN As String, ByVal dblAmtRespRP As Double, _
                ByVal lngPatApptID As Long, ByVal strUserName As String, ByVal lngUserID As Long) As Long
        '--------------------------------------------------------------------
        'Date: 08/08/2002
        'Author: Dave Richkun
        'Description:  Cancels the original version of a claim and submits a new version. This method
        '              is  ed and is to be used after the claim has been submitted to the payer
        '              i.e. before the HCFA has been sent.
        'Parameters:
        '
        '       **** TO DO ****: Define parameters when function finalized
        '
        'Returns: ID of new encounter that supercedes all previous encounters
        '--------------------------------------------------------------------
        Dim objEL As New EncounterLogDB.CEncounterLogDB
        Dim objBR As New BillingBZ.CBRBz
        Dim objPatTx As New BillingBZ.CPatientTxBz
        Dim objInsTx As New BillingBZ.CInsuranceTxBz
        Dim objCert As New CertificationBz.CCertificationBz
        Dim objClaim As New ClaimBz.CClaimBz
        Dim rstEL As New ADODB.Recordset
        Dim lngNewELID As Long
        Dim lngPrimaryPlanID As Long
        Dim lngPrimaryRPID As Long
        Dim lngPrimaryCPCID As Long
        Dim lngPrimaryInsID As Long
        Dim lngPrimaryBRID As Long
        Dim dblAmtRespPlan As Double
        Dim intOrder As Integer
        Dim strErrMsg As String
        Dim blnPrintHCFA As Boolean
        Dim intClaimStatus As Integer



        'Ensure the claim status is still valid for this operation.
        rstEL = FetchByID(lngELID)
        If rstEL.Fields("fldBillStatusID").Value <> CLAIM_STATUS_BILLED Then
            rstEL = Nothing
            strErrMsg = "The claim status is not valid for this operation."
            Exit Function
        End If
        rstEL = Nothing

        'Store values from primary plan
        lngPrimaryInsID = aryPlans(0, 8)
        lngPrimaryPlanID = aryPlans(0, 1)
        lngPrimaryRPID = aryPlans(0, 7)
        lngPrimaryBRID = aryPlans(0, 6)
        blnPrintHCFA = True

        'Calculate balances for each responsible party
        If dblFee = 0 Then
            dblAmtRespPlan = 0
            dblAmtRespRP = 0
            blnPrintHCFA = False

            'We still need to identify claim status, even if serviced at no charge
            If lngPrimaryInsID = INS_COMPANY_EAP Then
                intClaimStatus = CLAIM_STATUS_EAP
            Else
                Select Case lngPrimaryPlanID
                    Case PLAN_SELFPAY
                        intClaimStatus = CLAIM_STATUS_SELF_PAY 'Change claim status
                        If strDoNotBillYN = "Y" Then intClaimStatus = CLAIM_STATUS_NOCHARGE
                    Case Else 'Insurance claim
                        intClaimStatus = CLAIM_STATUS_NOCHARGE
                End Select
            End If
        Else
            'Identify billing responsibility amounts for each primary party.
            If lngPrimaryInsID = INS_COMPANY_EAP Then
                'The EAP is entirely responsible for EAP claims
                dblAmtRespPlan = (dblFee * dblUnits) + (dblAddOnFee * dblAddOnUnits) + (dblAddOnSecFee * dblAddOnSecUnits)
                dblAmtRespRP = 0
                blnPrintHCFA = False
                intClaimStatus = CLAIM_STATUS_EAP
            Else
                Select Case lngPrimaryPlanID
                    Case PLAN_SELFPAY
                        'The Resp Party is entirely responsible for Self-Pay claims
                        dblAmtRespPlan = 0
                        dblAmtRespRP = (dblFee * dblUnits) + (dblAddOnFee * dblAddOnUnits) + (dblAddOnSecFee * dblAddOnSecUnits)
                        blnPrintHCFA = False
                        intClaimStatus = CLAIM_STATUS_SELF_PAY 'Change claim status
                    Case Else 'Insurance claim
                        'Only the primary plan and the primary RP are responsible for payment at this stage.
                        If ((dblFee * dblUnits) + (dblAddOnFee * dblAddOnUnits) + (dblAddOnSecFee * dblAddOnSecUnits)) <= dblAmtRespRP Then
                            dblAmtRespPlan = 0
                        Else
                            dblAmtRespPlan = ((dblFee * dblUnits) + (dblAddOnFee * dblAddOnUnits) + (dblAddOnSecFee * dblAddOnSecUnits)) - dblAmtRespRP
                        End If
                        'dblAmtRespRP remains untouched
                        'blnPrintHCFA remains untouched

                        intClaimStatus = CLAIM_STATUS_QUEUED
                End Select
            End If
        End If

        'Disable the original version of the encounter record, set status to Cancelled
        Disable(lngELID)

        'Insert the new encounter record
        lngNewELID = objEL.Insert(intVersionNum, lngPatientID, lngProviderID, lngClinicID, dtDOS, _
                    strCPTCode, strModifier1, strModifier2, strDSMIV, dblFee, dblUnits, _
                    strAddOnCPTCode, strAddOnMod1, strAddOnMod2, dblAddOnFee, dblAddOnUnits, strAddOnSecCPTCode, strAddOnSecMod1, strAddOnSecMod2, dblAddOnSecFee, dblAddOnSecUnits, _
                    strReferPhyLast, strReferPhyFirst, strReferPhyMI, strReferPhyTitle, strReferPhy, strReferPhyNPI, _
                    strReferPhyID, dtAdmitDate, dtDischargeDate, dtDateOfOnset, strCondEmpYN, strCondAutoAccYN, _
                    strCondAutoAccState, strOtherAccYN, strDoNotBillYN, streScribeYN, intClaimStatus, dblAmtRespRP, dblAmtRespPlan, _
                    lngPatApptID, lngELID, strUserName)
        objEL = Nothing

        'Update Billing Responsibility rows from disabled encounter, and point them to new
        'encounter record, updating balances as needed.  Because we are updating before
        'any insurance payment has been paid, only the BR record associated with the primary insurance
        'plan needs to be updated.  We are not touching the BR record associated with the primary RP.
        If lngPrimaryPlanID = PLAN_SELFPAY Then
            objBR.Update(lngPrimaryBRID, aryPlans(0, 7), lngNewELID, aryPlans(0, 1), aryPlans(0, 9), aryPlans(0, 8), dblAmtRespRP, -1, aryPlans(0, 3), aryPlans(0, 4), aryPlans(0, 2), aryPlans(0, 5))
        Else
            objBR.Update(lngPrimaryBRID, aryPlans(0, 7), lngNewELID, aryPlans(0, 1), aryPlans(0, 9), aryPlans(0, 8), dblAmtRespPlan, 0, aryPlans(0, 3), aryPlans(0, 4), aryPlans(0, 2), aryPlans(0, 5))
        End If

        objBR.Resubmit(lngELID, lngNewELID) 'Transfer RP responsibility records to new encounter
        objBR = Nothing

        'Update patient transactions to point to new Encounter Log ID
        objPatTx.Resubmit(lngELID, lngNewELID)
        objPatTx = Nothing

        'Update insurance transactions to point to new Encounter Log ID

        'Insert 'Claim cancelled by provider' transaction and offsetting transaction to
        'zero balances.
        objInsTx.CancelAfterSubmit(lngELID, strUserName)

        'Update billing responsibility records to point to new Encounter Log ID
        objInsTx.Resubmit(lngELID, lngNewELID)

        If blnPrintHCFA Then
            'Insert 'Claim Queued' transaction
            objInsTx.Insert(lngNewELID, 0, lngPatientID, lngPrimaryRPID, lngPrimaryPlanID, _
              lngPrimaryCPCID, lngPrimaryInsID, TX_PYMT, TX_TYPE_HCFAQUEUED, 0, 0, Date.Now, _
              "", "01\01\1900", "", 0, 0, strUserName, lngUserID)

            'Insert the insurance company charge record
            objInsTx.Insert(lngNewELID, 0, lngPatientID, lngPrimaryRPID, lngPrimaryPlanID, _
                  lngPrimaryCPCID, lngPrimaryInsID, TX_CHARGE, TX_TYPE_INS_CHG, 0, _
                  dblAmtRespPlan, Date.Now, "", "01\01\1900", "", 0, 0, strUserName)
        End If

        objInsTx = Nothing

        'Rollback Certification units for the original claim before applying them to the new claim

        objCert.RollBack(lngELID, strUserName)

        'Deduct certification units for all plans.  Do not deduct cert units if service offered at no charge
        If dblFee > 0 Then
            objCert.DeductUnits(lngNewELID, lngPatientID, lngProviderID, lngPrimaryPlanID, dtDOS, strCPTCode, dblUnits)
        End If
        objCert = Nothing

        'Disable the original claim record before creating a new claim.
        '  objClaim.DeleteBeforeSubmit(lngELID)

        'Insert claim snapshot for insurance claims where fee charged (i.e. no pro-bono)
        If lngPrimaryInsID <> INS_COMPANY_EAP And lngPrimaryPlanID <> PLAN_SELFPAY And dblFee > 0 And strDoNotBillYN = "N" Then
            objClaim.Insert(lngNewELID, intVersionNum, lngPrimaryRPID, lngClinicID, lngPrimaryPlanID, _
                               lngPrimaryCPCID, lngPrimaryInsID, 1, strUserName)
        End If
        objClaim = Nothing

        ReSubmit = lngNewELID


        objEL = Nothing
        objBR = Nothing
        objPatTx = Nothing
        objInsTx = Nothing
        objCert = Nothing
        objClaim = Nothing


    End Function


    Public Function Fetch(Optional ByVal blnIncludeProcessed As Boolean = False, Optional ByVal strWhere As String = "", Optional ByVal strOrderBy As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/06/2000
        'Author: Eric Pena
        'Description:  Retrieves records from the tblEncounterLog table.
        'Parameters: blnIncludeDisabled - Optional parameter that identifies if
        '               records flagged as 'Processed' are to be
        '               included in the record set. The default value is False.
        '            strWhere - Optional 'Where' clause of the SQL statement by
        '               which records will be filtered.  If the parameter is not
        '               supplied, all records will be retrieved.
        '             strOrderBy - Optional 'Order By' clause in which retrieved
        '               records will be sorted.  The default is set to the
        '               'fldEncounterLogID' column
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim rstSQL As New ADODB.Recordset
        Dim objEL As New EncounterLogDB.CEncounterLogDB


        rstSQL = objEL.Fetch(blnIncludeProcessed, strWhere, strOrderBy)
        objEL = Nothing

        Fetch = rstSQL


    End Function


    Public Function FetchByDOS(ByVal lngProviderID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 5/05/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblEncounterLog table ordered by Date of Service utilizing a stored procedure
        'Parameters: lngProviderID - id of the provider to return records for
        'Returns: ADORst
        '--------------------------------------------------------------------

        Dim objEL As New EncounterLogDB.CEncounterLogDB


        FetchByDOS = objEL.FetchByDOS(lngProviderID)

        objEL = Nothing

    End Function

    Public Function FetchByName(ByVal lngProviderID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 5/05/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblEncounterLog table ordered by provider name utilizing a stored procedure
        'Parameters: lngProviderID - id of the provider to return records for
        'Returns: ADORst
        '--------------------------------------------------------------------

        Dim objEL As New EncounterLogDB.CEncounterLogDB


        FetchByName = objEL.FetchByName(lngProviderID)

        objEL = Nothing

    End Function

    Public Sub DeleteBeforeSubmit(ByVal lngELID As Long, ByVal lngPatApptID As Long, _
                ByVal blnDelAppt As Boolean, ByVal lngPatNoteID As Long, _
                ByVal blnDelNote As Boolean, ByVal arrPatTxs As Object, _
                ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 07/25/2002
        'Author: Dave Richkun
        'Description:  Physi y deletes an encounter and related rows from the database.
        '            Certification units are rolled back to their state before the encounter
        '            was entered.  This method is intended to be  ed only when the
        '            claim status is Queued i.e. a HCFA has not yet been generated.
        'Parameters: lngELID - ID of the row in tblEncounterLog that will be deleted
        '            lngPatApptID - ID of the row in tblPatientAppt that will be reset/deleted
        '            blnDelAppt - Boolean identifer indicating whether to permanently delete the patient appt
        '            lngPatNoteID - ID of the row in tblPatientNote that will be deleted
        '            arrPatTxs - single dimensional array of IDs in tblPatientTx that will be deleted
        '            strUserName - Login name of user performing action
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objCert As New CertificationBz.CCertificationBz
        Dim objPatTx As New BillingBZ.CPatientTxBz
        Dim objEL As New EncounterLogDB.CEncounterLogDB
        Dim rstEL As New ADODB.Recordset
        Dim strErrMsg As String


        'Ensure we are still allowed to delete the claim
        rstEL = FetchByID(lngELID)
        If rstEL.Fields("fldBillStatusID").Value <> CLAIM_STATUS_QUEUED And rstEL.Fields("fldBillStatusID").Value <> CLAIM_STATUS_EAP And _
               rstEL.Fields("fldBillStatusID").Value <> CLAIM_STATUS_SELF_PAY And rstEL.Fields("fldBillStatusID").Value <> CLAIM_STATUS_NOCHARGE And _
               rstEL.Fields("fldBillStatusID").Value <> CLAIM_STATUS_REJECTED Then
            rstEL = Nothing
            strErrMsg = "The claim has already been submitted to the payer.  You can not update this claim."

            Exit Sub
        End If
        rstEL = Nothing

        'Rollback Cert Units, delete history from tblCertUsage
        objCert.RollBack(lngELID, strUserName)
        objCert = Nothing

        'Delete unwanted patient transactions, and update ELID pointer to NULL
        'for those patient transactions to keep
        objPatTx.UpdateDeleteForELID(lngELID, arrPatTxs)
        objPatTx = Nothing

        'Delete rows in tblClaim, tblBillingResponsibility, tblEncounterLog, and optionally
        'from tblPatientAppt and tblPatientNote
        objEL.DeleteBeforeSubmit(lngELID, lngPatApptID, blnDelAppt, lngPatNoteID, blnDelNote, strUserName)
        objEL = Nothing


        'Signal incompletion and raise the error to the  ing environment.
        objEL = Nothing
        objPatTx = Nothing
        objCert = Nothing

        If Err.Description > "" Then
            strErrMsg = Err.Description
        End If


    End Sub

    Public Sub CancelAfterSubmit(ByVal lngELID As Long, ByVal lngPatApptID As Long, _
                ByVal blnDelAppt As Boolean, ByVal lngPatNoteID As Long, _
                ByVal blnDelNote As Boolean, ByVal arrPatTxs As Object, _
                ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 08/07/2002
        'Author: Dave Richkun
        'Description:  Cancels an encounter and zeroes-out related rows from the database.
        '            Certification units are rolled back to their state before the encounter
        '            was entered.  This method is intended to be  ed only when the
        '            claim status is Billed i.e. a HCFA has been submitted to the payer.
        'Parameters: lngELID - ID of the row in tblEncounterLog that will be cancelled
        '            lngPatApptID - ID of the row in tblPatientAppt that will be reset
        '            lngPatNoteID - ID of the row in tblPatientNote that will be deleted
        '            arrPatTxs - single dimensional array of IDs in tblPatientTx that will
        '               be offset or deleted
        '            strUserName - Login name of user performing action
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objEL As New EncounterLogDB.CEncounterLogDB
        Dim objBR As New BillingBZ.CBRBz
        Dim objPatTx As New BillingBZ.CPatientTxBz
        Dim objInsTx As New BillingBZ.CInsuranceTxBz
        Dim objCert As New CertificationBz.CCertificationBz
        Dim rstEL As New ADODB.Recordset
        Dim strErrMsg As String = String.Empty

        'Ensure we are still allowed to cancel the claim.  A Queued claim is cancelled
        'only if the claim has been re-submitted i.e. the claim version number is
        'greater than 1.
        rstEL = FetchByID(lngELID)
        If rstEL.Fields("fldBillStatusID").Value <> CLAIM_STATUS_BILLED And _
           rstEL.Fields("fldBillStatusID").Value <> CLAIM_STATUS_SUBMITTED And _
           rstEL.Fields("fldBillStatusID").Value <> CLAIM_STATUS_QUEUED And _
           rstEL.Fields("fldBillStatusID").Value <> CLAIM_STATUS_DENIED And _
           rstEL.Fields("fldBillStatusID").Value <> CLAIM_STATUS_APPEAL And _
           rstEL.Fields("fldBillStatusID").Value <> CLAIM_STATUS_REJECTED And _
           rstEL.Fields("fldBillStatusID").Value <> CLAIM_STATUS_PROVIDER Then
            rstEL = Nothing
            strErrMsg = "You cannot cancel this claim because of it's current claim status."
            Exit Sub
        End If
        rstEL = Nothing

        objEL.CancelAfterSubmit(lngELID, lngPatApptID, blnDelAppt, lngPatNoteID, blnDelNote, strUserName)
        objEL = Nothing

        'Update Billing Responsibility record for the primary plan.  Amounts for both the primary plan
        'and the primary RP are set to a balance of zero.
        objBR.ZeroAll(lngELID)
        objBR = Nothing

        'Delete unwanted patient transactions, and update ELID pointer to NULL
        'for those patient transactions to keep.
        If IsArray(arrPatTxs) Then
            objPatTx.UpdateDeleteForELID(lngELID, arrPatTxs)
            objPatTx = Nothing
        End If

        objInsTx.CancelAfterSubmit(lngELID, strUserName)
        objInsTx = Nothing

        objCert.RollBack(lngELID, strUserName)
        objCert = Nothing




        objEL = Nothing
        objBR = Nothing
        objPatTx = Nothing
        objInsTx = Nothing
        objCert = Nothing



    End Sub




    Public Function FetchByProvDOS(ByVal lngProviderID As Long, ByVal dteFrom As Date, ByVal dteTo As Date) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 5/15/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblEncounterLog table ordered by provider name utilizing a stored procedure
        'Parameters: lngProviderID - id of the provider to return records for
        '                    dteFrom/dteTo - the date range of DOS to search for
        'Returns: ADORst
        '--------------------------------------------------------------------

        Dim objEL As New EncounterLogDB.CEncounterLogDB
        Dim dteTemp As Date


        If dteTo < dteFrom Then
            dteTemp = dteTo
            dteTo = dteFrom
            dteFrom = dteTemp
        End If

        FetchByProvDOS = objEL.FetchByProvDOS(lngProviderID, dteFrom, dteTo)

        objEL = Nothing
    End Function

    Public Function FetchStatus(ByVal varProviderID As Object, ByVal dteFrom As Date, ByVal dteTo As Date, Optional ByVal blnProcessed As Object = Nothing, Optional ByVal lngPatientID As Long = -1) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/06/2000
        'Author: Eric Pena
        'Description:  Retrieves records from the tblEncounterLog table.
        'Parameters: blnIncludeDisabled - Optional parameter that identifies if
        '               records flagged as 'Processed' are to be
        '               included in the record set. The default value is False.
        '            strWhere - Optional 'Where' clause of the SQL statement by
        '               which records will be filtered.  If the parameter is not
        '               supplied, all records will be retrieved.
        '             strOrderBy - Optional 'Order By' clause in which retrieved
        '               records will be sorted.  The default is set to the
        '               'fldEncounterLogID' column
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim rstSQL As New ADODB.Recordset
        Dim objEL As New EncounterLogDB.CEncounterLogDB
        Dim dteTemp As Date


        If dteTo < dteFrom Then
            dteTemp = dteTo
            dteTo = dteFrom
            dteFrom = dteTemp
        End If
        If IsArray(varProviderID) Then
            If UBound(varProviderID) < 0 Then Exit Function
        End If

        rstSQL = objEL.FetchStatus(varProviderID, dteFrom, dteTo, blnProcessed, lngPatientID)
        FetchStatus = rstSQL

        objEL = Nothing
    End Function


    Public Function FetchForPatient(ByVal lngPatientID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/07/2000
        'Author: Dave Richkun
        'Description:  Returns rows from the tblEncounterLog table associated
        '              with a specific patient.
        'Parameters: lngPatientID - ID of the patient whose Encounter records are
        '                     being retrieved.
        'Returns: Recordset of matching rows
        '--------------------------------------------------------------------

        Dim objEL As New EncounterLogDB.CEncounterLogDB

        FetchForPatient = objEL.FetchForPatient(lngPatientID)

        objEL = Nothing


    End Function


    Public Function FetchByID(ByVal lngEncLogID As Long, Optional ByVal strDataBase As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/07/2000
        'Author: Dave Richkun
        'Description:  Returns a row from the tblEncounterLog table associated
        '              with a specific Encounter.
        'Parameters: lngEncLogID - ID of the Encounter record to retrieve.
        'Returns: Recordset of matching rows
        '--------------------------------------------------------------------
        ' R003 - Created
        '--------------------------------------------------------------------

        Dim objEL As New EncounterLogDB.CEncounterLogDB


        FetchByID = objEL.FetchByID(lngEncLogID, strDataBase)

        objEL = Nothing


    End Function


    Public Function FetchByUserEnteredToday(ByVal strUserName As String) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/07/2000
        'Author: Dave Richkun
        'Description:  Returns rows from the tblEncounterLog table entered
        '              by a specific user on the system date.
        'Parameters: strUserName - Username of the user who entered the encounters
        'Returns: Recordset of matching rows
        '--------------------------------------------------------------------
        ' R003 - Created
        '--------------------------------------------------------------------

        Dim objEL As New EncounterLogDB.CEncounterLogDB



        FetchByUserEnteredToday = objEL.FetchByUserEnteredToday(strUserName)

        'Signal successful completion
        objEL = Nothing

    End Function

    Public Function FetchByHour() As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/07/2000
        'Author: Eric Pena
        'Description:  Returns rows from tblEncounterLog added by the previous hour
        'Parameters:
        'Returns: ADORst
        '--------------------------------------------------------------------
        Dim objEL As New EncounterLogDB.CEncounterLogDB

        FetchByHour = objEL.FetchByHour()

        'Signal successful completion
        objEL = Nothing


    End Function

    '-- Begin R009 --------------------
    Public Function FetchForPatientUser(ByVal lngPatientID As Long, _
                    ByVal lngUserID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 02/12/2001
        'Author: Dave Richkun
        'Description:  Returns rows from the tblEncounterLog table associated
        '              with a specific patient and within the context of the
        '              logged in user.  This method takes into consideraton whether
        '              the patient in question is shared between Providers.  If a
        '              patient is shared then Encounter records for all shared Providers
        '              are returned.  If the patient is not shared, then only records
        '              associated with the current user are returned.
        'Parameters: lngPatientID - ID of the patient whose Encounter records are
        '                     being retrieved.
        '            lngUserID - ID of the user making the   to this object.
        'Returns: Recordset of matching rows
        '--------------------------------------------------------------------

        Dim objEL As New EncounterLogDB.CEncounterLogDB


        FetchForPatientUser = objEL.FetchForPatientUser(lngPatientID, lngUserID)

        objEL = Nothing

    End Function
    '-- End R009 --------------------

    '-- Begin R012 --------------------
    Public Function FetchSeekNAssign(ByVal lngCertID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/01/2001                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Returns rows from the tblEncounterLog table associated
        '              that may qualify for a new certification.            '
        'Parameters: lngCertID - ID of the cert to check encounters against '
        'Returns: Recordset of matching rows                                '
        '--------------------------------------------------------------------

        Dim objEL As New EncounterLogDB.CEncounterLogDB
        FetchSeekNAssign = objEL.FetchSeekNAssign(lngCertID)

        objEL = Nothing
    End Function
    '-- End R012 --------------------
    Public Function FetchClaimDetailByEncID(ByVal lngEncLogID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/31/2019
        'Author: Duane C Orth
        'Description:  Returns a row from the tblEncounterLog table associated
        '              with a specific Encounter.
        'Parameters: lngEncLogID - ID of the Encounter record to retrieve.
        'Returns: Recordset of matching rows
        '--------------------------------------------------------------------

        Dim objEL As New EncounterLogDB.CEncounterLogDB


        FetchClaimDetailByEncID = objEL.FetchClaimDetailByEncID(lngEncLogID)


        objEL = Nothing


    End Function
    Public Function FetchClaimDetailByClaimID(ByVal lngClaimID As Long, Optional ByVal strDataBase As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/21/2020
        'Author: Duane C Orth
        'Description:  Returns a row from the tblEncounterLog table associated
        '              with a specific Encounter.
        'Parameters: lngClaimID - ID of the Claim record to retrieve.
        'Returns: Recordset of matching rows
        '--------------------------------------------------------------------

        Dim objEL As New EncounterLogDB.CEncounterLogDB

        FetchClaimDetailByClaimID = objEL.FetchClaimDetailByClaimID(lngClaimID, strDataBase)


        objEL = Nothing



    End Function
    Public Function ValidateCodes(ByVal strCPTCode As String, ByVal strDSMCode As String, _
                    ByVal lngPatientID As Long, ByVal lngProviderID As Long, _
                    ByVal dtDOS As Date) As Integer
        '--------------------------------------------------------------------
        'Date: 07/18/2002
        'Author: Dave Richkun
        'Description:  Performs server-side validation for Encounter/Claim entry.  This
        '              method ensures the CPT Code and DSM-IV Code are valid, and checks
        '              for duplicate encounters within a single  .  The stored procedure
        '              returns a 'bit-representation' integer that identifies the error
        '              condition based on the following 'bit' settings:
        '              0 - No errors
        '              1 - CPT Code is invalid
        '              2 - DSM-IV Code is invalid
        '              4 - Duplicate encounter exists
        'Parameters:  strCPTCode - CPT Code assigned to service date
        '             strDSMCode - DSM-IV Code assigned to patient on service date
        '             lngPatientID  - ID of patient receiving service
        '             lngProviderID - ID of provider servicing patient
        '             dtDOS - Date service was performed
        'Returns: Integer identifying error code
        '--------------------------------------------------------------------

        Dim objEL As New EncounterLogDB.CEncounterLogDB


        ValidateCodes = objEL.ValidateCodes(strCPTCode, strDSMCode, lngPatientID, _
                          lngProviderID, dtDOS)
        objEL = Nothing


    End Function


    Public Function FetchTxsByEL(ByVal lngEncounterLogID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/23/2002
        'Author: Dave Richkun
        'Description:  Returns rows from tblInsuranceTx and tblPatientTx associated
        '              with a specific Encounter.
        'Parameters: lngEncLogID - ID of the Encounter record.
        'Returns: Recordset of matching rows
        '--------------------------------------------------------------------

        Dim objEL As New EncounterLogDB.CEncounterLogDB

        FetchTxsByEL = objEL.FetchTxsByEL(lngEncounterLogID)
        objEL = Nothing



    End Function


    '-----------------------------
    '     Private Routines
    '-----------------------------

    Private Sub Update(ByVal lngELID As Long, ByVal intVersionNum As Integer, ByVal lngPatientID As Long, _
                ByVal lngProviderID As Long, ByVal lngClinicID As Long, ByVal dtDOS As Date, _
                ByVal strCPTCode As String, ByVal strModifier1 As String, ByVal strModifier2 As String, _
                ByVal strDSMIV As String, ByVal dblFee As Double, ByVal dblUnits As Double, _
                ByVal strAddOnCPTCode As String, ByVal strAddOnMod1 As String, ByVal strAddOnMod2 As String, _
                ByVal dblAddOnFee As Double, ByVal dblAddOnUnits As Double, _
                ByVal strAddOnSecCPTCode As String, ByVal strAddOnSecMod1 As String, ByVal strAddOnSecMod2 As String, _
                ByVal dblAddOnSecFee As Double, ByVal dblAddOnSecUnits As Double, _
                ByVal aryPlans As Object, ByVal strReferPhyLast As String, ByVal strReferPhyFirst As String, ByVal strReferPhyMI As String, _
                ByVal strReferPhyTitle As String, ByVal strReferPhy As String, ByVal strReferPhyNPI As String, _
                ByVal strReferPhyID As String, ByVal dtAdmitDate As Date, ByVal dtDischargeDate As Date, _
                ByVal dtDateOfOnset As Date, ByVal strCondEmpYN As String, ByVal strCondAutoAccYN As String, _
                ByVal strCondAutoAccState As String, ByVal strOtherAccYN As String, ByVal strDoNotBillYN As String, _
                ByVal streScribeYN As String, ByVal intClaimStatus As Integer, ByVal dblAmtRespRP As Double, _
                ByVal dblAmtRespPlan As Double, ByVal lngPatApptID As Long, ByVal lngPrevELID As Long, _
                ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 08/02/2002
        'Author: Dave Richkun
        'Description:  Updates a row in the tblEncounterLog table utilizing a stored procedure.
        'Parameters:
        '  *** TO DO:  Define all parameters when procedure finalized ****
        '
        'Returns: Null
        '--------------------------------------------------------------------

        Dim objEL As New EncounterLogDB.CEncounterLogDB


        objEL.Update(lngELID, lngPatientID, lngProviderID, lngClinicID, dtDOS, _
              strCPTCode, strModifier1, strModifier2, strDSMIV, dblFee, dblUnits, _
              strAddOnCPTCode, strAddOnMod1, strAddOnMod2, dblAddOnFee, dblAddOnUnits, _
              strAddOnSecCPTCode, strAddOnSecMod1, strAddOnSecMod2, dblAddOnSecFee, dblAddOnSecUnits, _
              strReferPhyLast, strReferPhyFirst, strReferPhyMI, strReferPhyTitle, strReferPhy, strReferPhyNPI, _
              strReferPhyID, dtAdmitDate, dtDischargeDate, dtDateOfOnset, strCondEmpYN, strCondAutoAccYN, _
              strCondAutoAccState, strOtherAccYN, strDoNotBillYN, streScribeYN, intClaimStatus, dblAmtRespRP, _
              dblAmtRespPlan, lngPatApptID, strUserName)
        objEL = Nothing



    End Sub


    Private Sub Disable(ByVal lngELID As Long)
        '--------------------------------------------------------------------
        'Date: 08/08/2002
        'Author: Dave Richkun
        'Description:  Disables a row in tblEncounterLog and sets the claim status
        '              to Cancelled
        'Parameters: lngELID - ID of the row in tblEncounterLog to disable
        'Returns: Null
        '--------------------------------------------------------------------

        Dim objEL As EncounterLogDB.CEncounterLogDB

        objEL.Disable(lngELID)
        objEL = Nothing



    End Sub


    Private Function SortPlanArray(ByVal varPlans As Object) As Object
        '--------------------------------------------------------------------
        'Date: 08/30/2002
        'Author: Dave Richkun
        'Description:  Sorts a 2-dimensional Plan array using bubble-sort and
        '              returns the sorted array to the  ing procedure.
        'Parameters: varPlans - The array to be sorted
        'Returns: The sorted array, ordered by Plan Order
        '--------------------------------------------------------------------

        Dim intCtr1 As Integer
        Dim intCtr2 As Integer
        Dim arrTemp(0, 9) As String

        If Not (varPlans.IsNullorEmpty) Then '
            If IsArray(varPlans) Then
                For intCtr2 = (UBound(varPlans, 1) - 1) To 0 Step -1
                    For intCtr1 = 0 To intCtr2
                        If CInt(varPlans(intCtr1, 0)) > CInt(varPlans(intCtr1 + 1, 0)) Then
                            arrTemp(0, 0) = varPlans(intCtr1, 0)
                            arrTemp(0, 1) = varPlans(intCtr1, 1)
                            arrTemp(0, 2) = varPlans(intCtr1, 2)
                            arrTemp(0, 3) = varPlans(intCtr1, 3)
                            arrTemp(0, 4) = varPlans(intCtr1, 4)
                            arrTemp(0, 5) = varPlans(intCtr1, 5)
                            arrTemp(0, 6) = varPlans(intCtr1, 6)
                            arrTemp(0, 7) = varPlans(intCtr1, 7)
                            arrTemp(0, 8) = varPlans(intCtr1, 8)
                            arrTemp(0, 9) = varPlans(intCtr1, 9)

                            varPlans(intCtr1, 0) = varPlans(intCtr1 + 1, 0)
                            varPlans(intCtr1, 1) = varPlans(intCtr1 + 1, 1)
                            varPlans(intCtr1, 2) = varPlans(intCtr1 + 1, 2)
                            varPlans(intCtr1, 3) = varPlans(intCtr1 + 1, 3)
                            varPlans(intCtr1, 4) = varPlans(intCtr1 + 1, 4)
                            varPlans(intCtr1, 5) = varPlans(intCtr1 + 1, 5)
                            varPlans(intCtr1, 6) = varPlans(intCtr1 + 1, 6)
                            varPlans(intCtr1, 7) = varPlans(intCtr1 + 1, 7)
                            varPlans(intCtr1, 8) = varPlans(intCtr1 + 1, 8)
                            varPlans(intCtr1, 9) = varPlans(intCtr1 + 1, 9)

                            varPlans(intCtr1 + 1, 0) = arrTemp(0, 0)
                            varPlans(intCtr1 + 1, 1) = arrTemp(0, 1)
                            varPlans(intCtr1 + 1, 2) = arrTemp(0, 2)
                            varPlans(intCtr1 + 1, 3) = arrTemp(0, 3)
                            varPlans(intCtr1 + 1, 4) = arrTemp(0, 4)
                            varPlans(intCtr1 + 1, 5) = arrTemp(0, 5)
                            varPlans(intCtr1 + 1, 6) = arrTemp(0, 6)
                            varPlans(intCtr1 + 1, 7) = arrTemp(0, 7)
                            varPlans(intCtr1 + 1, 8) = arrTemp(0, 8)
                            varPlans(intCtr1 + 1, 9) = arrTemp(0, 9)
                        End If
                    Next
                Next
            End If
        End If

        SortPlanArray = varPlans
    End Function

    Public Sub CorrectClaimCert(ByVal lngELID As Long, ByVal lngProviderID As Long, ByVal lngPatientID As Long, _
                    ByVal intVersionNum As Integer, ByVal lngRPID As Long, ByVal lngClinicID As Long, ByVal lngPlanID As Long, _
                    ByVal lngCPCID As Long, ByVal lngInsuranceID As Long, ByVal intOrder As Integer, _
                    ByVal strCertNum As String, ByVal strUserName As String, ByVal lngUserID As Long)
        '--------------------------------------------------------------------
        'Date: 08/07/2002
        'Author: Eric Pena
        'Description:  Updates the cert used and resubmits the claim with an updated HCFA.
        '            If a cert was used it is rolled back.
        'Parameters: lngELID - ID of the row in tblEncounterLog that will be corrected
        '            lngRPID - RPID of the row in tblBillingResponsibility for the payer denying the claim
        '            lngPlanID - PlanID of the row in tblBillingResponsibility for the payer denying the claim
        '            strCertNum - New cert number to use
        '            strUserName - Login name of user performing action
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objCert As New CertificationBz.CCertificationBz
        Dim objClaim As New ClaimBz.CClaimBz
        Dim objBR As New BillingBZ.CBRBz
        Dim objInsTx As New BillingBZ.CInsuranceTxBz
        Dim objProvider As New ProviderBz.CProviderBZ
        Dim strErrMsg As String


        'Change Encounter Status
        UpdateEncounterStatus(lngELID, CLAIM_STATUS_QUEUED)

        'Insert a zero-dollar transaction to assist with chronological events
        objInsTx.Insert(lngELID, 0, lngPatientID, lngRPID, lngPlanID, _
          lngCPCID, lngInsuranceID, TX_PYMT, TX_TYPE_HCFAQUEUED, 0, Date.Now.ToOADate(), _
          "", 0, "", 0, 0, strUserName, lngUserID)
        objInsTx = Nothing

        'Rollback this plan's certs if used
        If strCertNum <> "" Then
            objCert.RollBack(lngELID, strUserName, lngRPID, lngPlanID) ' TODO: support extra parameters
            objCert = Nothing
        End If

        'assign new certnum to tblbr
        objBR.UpdateCert(lngELID, lngRPID, lngPlanID, strCertNum)
        objBR = Nothing

        'insert updated claim
        objClaim.Insert(lngELID, intVersionNum, lngRPID, lngClinicID, lngPlanID, lngCPCID, _
                           lngInsuranceID, intOrder, strUserName)
        objClaim = Nothing

        'Decrement Provider denial counter
        objProvider.DecrementDenialCounter(lngProviderID)
        objProvider = Nothing


        objBR = Nothing
        objInsTx = Nothing
        objClaim = Nothing
        objCert = Nothing
        objProvider = Nothing


    End Sub


    Public Sub CorrectClaimCardNumber(ByVal lngELID As Long, ByVal lngProviderID As Long, ByVal lngPatientID As Long, _
            ByVal intVersionNum As Integer, ByVal lngRPID As Long, ByVal lngClinicID As Long, ByVal lngPlanID As Long, ByVal lngCPCID As Long, _
            ByVal lngInsuranceID As Long, ByVal intOrder As Integer, ByVal strCardNum As String, ByVal strUserName As String, _
            ByVal lngUserID As Long)
        '--------------------------------------------------------------------
        'Date: 08/07/2002
        'Author: Eric Pena
        'Description:  Updates the patient's insurance card information and resubmits a claim
        '              with an updated HCFA.
        'Parameters:  lngELID - ID of the claim to resubmit
        '            lngPatientID - ID of the patient whose Card number will be corrected.
        '            lngRPID - RPID of the row in tblBillingResponsibility for the payer denying the claim
        '            lngPlanID - PlanID of the row in tblBillingResponsibility for the payer denying the claim
        '            strCardNum - Upated card number
        '            strUserName - Login name of user performing action
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objClaim As New ClaimBz.CClaimBz
        Dim objBFact As New BenefactorBz.CBenefactorBz
        Dim objInsTx As New BillingBZ.CInsuranceTxBz
        Dim objProvider As New ProviderBz.CProviderBZ
        Dim strErrMsg As String


        'Update patient insurance record
        objBFact.UpdatePatientCardNumber(lngPatientID, lngPlanID, strCardNum)
        objBFact = Nothing

        'Change Encounter Status
        UpdateEncounterStatus(lngELID, CLAIM_STATUS_QUEUED)

        'Insert a zero-dollar transaction to assist with chronological events
        objInsTx.Insert(lngELID, 0, lngPatientID, lngRPID, lngPlanID, _
          lngCPCID, lngInsuranceID, TX_PYMT, TX_TYPE_HCFAQUEUED, 0, Date.Now.ToOADate(), _
          "", 0, "", 0, 0, strUserName, lngUserID)
        objInsTx = Nothing


        'Insert new claim record

        objClaim.Insert(lngELID, intVersionNum, lngRPID, lngClinicID, lngPlanID, lngCPCID, _
                           lngInsuranceID, intOrder, strUserName)
        objClaim = Nothing

        'Deduct denial counter

        objProvider.DecrementDenialCounter(lngProviderID)
        objProvider = Nothing

        'Signal successful completion

        objBFact = Nothing
        objInsTx = Nothing
        objClaim = Nothing
        objProvider = Nothing



    End Sub


    Public Sub UpdateEncounterStatus(ByVal lngELID As Long, ByVal intClaimStatus As Integer)
        '--------------------------------------------------------------------
        'Date: 10/16/2002
        'Author: Dave Richkun
        'Description:  Updates the status of an encounter.
        'Parameters:  lngELID - ID of the claim
        '             intVerNum - Version number of claim
        '             intClaimStatus - Status to which claim is being updated
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objEL As New EncounterLogDB.CEncounterLogDB


        objEL.UpdateEncounterStatus(lngELID, intClaimStatus)
        objEL = Nothing



    End Sub

    Public Function FetchRejectionsByProvider(ByVal lngProviderID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/05/2003
        'Author: Dave Richkun
        'Description:  Returns claims with a status of 'Rejected'
        'Parameters: lngProviderID - ID of the provider whose rejects are being returned.
        'Returns: ADO Recordset
        '--------------------------------------------------------------------
        Dim objEL As New EncounterLogDB.CEncounterLogDB


        FetchRejectionsByProvider = objEL.FetchRejectionsByProvider(lngProviderID)
        objEL = Nothing



    End Function

    Public Function FetchNotesMissingByProvider(ByVal lngProviderID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/05/2003
        'Author: Dave Richkun
        'Description:  Returns claims with a status of 'Rejected'
        'Parameters: lngProviderID - ID of the provider whose rejects are being returned.
        'Returns: ADO Recordset
        '--------------------------------------------------------------------
        Dim objEL As New EncounterLogDB.CEncounterLogDB

        FetchNotesMissingByProvider = objEL.FetchNotesMissingByProvider(lngProviderID)
        objEL = Nothing



    End Function

    Public Sub MarkUncollectable(ByVal lngELID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 05/13/2003
        'Author: Dave Richkun
        'Description:  Marks a claim as Uncollectable
        'Parameters:  lngELID - ID of the claim
        '             strUserName - Name of the user executing this method
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objEL As New EncounterLogDB.CEncounterLogDB

        objEL.MarkUncollectable(lngELID, strUserName)
        objEL = Nothing



    End Sub

    Public Sub UnCancelEncounter(ByVal lngELID As Long, ByVal lngPatientID As Long)
        '--------------------------------------------------------------------
        'Date: 11/02/2007
        'Author: Duane C Orth
        'Description:  UnCancels an Encounter
        'Parameters:  lngELID - ID of the claim
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objEL As New EncounterLogDB.CEncounterLogDB

        objEL.UnCancelEncounter(lngELID, lngPatientID)
        objEL = Nothing



    End Sub


    Public Sub AddSecondaryInsurance(ByVal lngELID As Long, ByVal lngPatientID As Long, ByVal intOrder As Integer)
        '--------------------------------------------------------------------
        'Date: 11/02/2007
        'Author: Duane C Orth
        'Description:  Adds the Secondary Insurance to the Encounter
        'Parameters:  lngELID - ID of the claim
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objEL As New EncounterLogDB.CEncounterLogDB

        objEL.AddSecondaryInsurance(lngELID, lngPatientID, intOrder)
        objEL = Nothing



    End Sub

End Class
