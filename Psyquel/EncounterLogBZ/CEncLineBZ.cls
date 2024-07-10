'--------------------------------------
'Class Name: CEncLineBz
'Date: 06/15/2017
'Author: Duane C Orth
'Denis J Finkel
'Description:  MTS business object designed to   methods associated
'              with the CEncLineDB class.
'--------------------------------------------------------------------
'Revision History:
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


Public Class CEncLineBZ
    Dim ds As New db

    Private _ConnectionString As String = String.Empty

    Private Const CLASS_NAME As String = "CEncLineBz"
    Private Const TABLE_NAME As String = "tblEncDetail"
    Private Const TX_CHARGE As String = "C"
    Private Const TX_TYPE_INS_CHG As Integer = 1



    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property







    Public Function FetchByID(ByVal lngEncDetailID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/05/2010
        'Author: Duane C Orth
        'Description:  Returns a row from the tblEncDetail table associated
        '              with a specific EncDetailID.
        'Parameters: lngEncDetailID - ID of the Encounter Detail record to retrieve.
        'Returns: Recordset of matching rows
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim obj As New EncounterLogDB.CEncLineDB

        obj.ConnectionString = _ConnectionString

        FetchByID = obj.FetchByID(lngEncDetailID)
        obj = Nothing

        'Signal successful completion


    End Function
    Public Function FetchByEncID(ByVal lngELID As Long, Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/05/2010
        'Author: Duane C Orth
        'Description:  Returns rows from the tblEncDetail table associated
        '              with a specific lngELID.
        'Parameters: lngELID - id of the Encounter to return records for
        'Returns: ADORst
        '--------------------------------------------------------------------

        Dim objEL As New EncounterLogDB.CEncLineDB

        objEL.ConnectionString = _ConnectionString
        FetchByEncID = objEL.FetchByEncID(lngELID, blnIncludeDisabled)
        objEL = Nothing


    End Function

    Public Function Insert(ByVal lngEncounterLogID As Long, ByVal lngLine As Long, ByVal lngClinicID As Long, ByVal dtDOS As Date, _
                ByVal strCPTCode As String, ByVal dblFee As Double, ByVal dblUnits As Double, ByVal strDiagCode As String, _
                ByVal strModifier1 As String, ByVal strModifier2 As String, _
                ByVal strAddOnCPTCode As String, ByVal dblAddOnFee As Double, ByVal dblAddOnUnits As Double, _
                ByVal strAddOnMod1 As String, ByVal strAddOnMod2 As String, _
                ByVal strAddOnSecCPTCode As String, ByVal dblAddOnSecFee As Double, ByVal dblAddOnSecUnits As Double, _
                ByVal strAddOnSecMod1 As String, ByVal strAddOnSecMod2 As String, _
                ByVal strAddedBy As String) As Long
        '--------------------------------------------------------------------
        'Date: 06/15/2017
        'Author: Duane C Orth
        'Description:  Inserts rows into Encounter Line Item Detail table as needed.
        'Parameters:
        '
        'Returns: Encounter Line ID of the data inserted.
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim obj As New EncounterLogDB.CEncLineDB
        Dim objEL As New EncounterLogDB.CEncounterLogDB
        '    Dim objBFact As BenefactorBz.CPatRPPlanBz
        Dim objInsTx As New BillingBz.CInsuranceTxBz
        Dim objCert As New CertificationBz.CCertificationBz
        Dim rst As New ADODB.Recordset
        Dim rstBR As New ADODB.Recordset
        Dim lngID, intCtr, lngPatientID, lngProviderID, lngPrimaryPlanID, lngPrimaryRPID, lngPrimaryCPCID, lngPrimaryInsID As Long
        Dim lngEncDetID As Long
        Dim dblAmtRespPlan As Decimal
        Dim strErrMessage As String

        Dim objCPT As New ListBZ.CCPTCodeBz
        Dim rs As New ADODB.Recordset
        Dim lngCptRecordID As Long







        obj.ConnectionString = _ConnectionString

































        rs = objCPT.Fetch(True, " fldCPTCode = '" & strCPTCode & "' ", , 0)
        objCPT = Nothing
        lngCptRecordID = ds.IfNull(rs.Fields("fldCptRecordID").Value, 0)
        rs = Nothing


        rst = objEL.FetchByID(lngEncounterLogID)
        objEL = Nothing
        lngPatientID = rst.Fields("fldPatientID").Value
        lngProviderID = rst.Fields("fldProviderID").Value

        'Ensure the patient has at least 1 active insurance plan.
        If rst.RecordCount = 0 Then
            strErrMessage = "This patient does not have any active plans.  You must add an insurance plan to the patient record before you can submit a claim."
            Exit Function
        End If

        'Store values from primary plan
        For intCtr = 1 To rst.RecordCount
            If rst.Fields("fldPlanID").Value = 331 Or _
               rst.Fields("fldOrder").Value = 2 Then
                lngPrimaryPlanID = rst.Fields("fldPlanID").Value 'Store ID of primary insurance plan
                lngPrimaryRPID = rst.Fields("fldRPID").Value  'Store ID of Resp Party
                lngPrimaryCPCID = rst.Fields("fldCPCID").Value 'Store ID of primary CPC
                lngPrimaryInsID = rst.Fields("fldInsuranceID").Value  'Store ID of primary insurance company
                Exit For
            End If
            rst.MoveNext()
        Next intCtr
        rst = Nothing
        rstBR = Nothing


        lngEncDetID = obj.Insert(lngEncounterLogID, lngLine, lngClinicID, dtDOS, strCPTCode, _
                dblFee, dblUnits, strDiagCode, strModifier1, strModifier2, strAddOnCPTCode, dblAddOnFee, dblAddOnUnits, _
                strAddOnMod1, strAddOnMod2, strAddOnSecCPTCode, dblAddOnSecFee, dblAddOnSecUnits, _
                strAddOnSecMod1, strAddOnSecMod2, strAddedBy)
        obj = Nothing


        dblAmtRespPlan = (dblFee * dblUnits) + (dblAddOnFee * dblAddOnUnits) + (dblAddOnSecFee * dblAddOnSecUnits)
        If lngPrimaryPlanID <> 331 Then     'dblAmtRespPlan <> 0 And 'removed
            'Insert the insurance company charge record
            objInsTx.Insert(lngEncounterLogID, lngEncDetID, lngPatientID, lngPrimaryRPID, lngPrimaryPlanID, _
                   lngPrimaryCPCID, lngPrimaryInsID, TX_CHARGE, TX_TYPE_INS_CHG, _
                   lngCptRecordID, dblAmtRespPlan, Date.Now, "", "01\01\1900", "", 0, 0, strAddedBy, lngProviderID)
        End If
        objInsTx = Nothing

        'Deduct certification units for all plans.  Do not deduct cert units if service offered at no charge
        If dblFee > 0 And lngPrimaryPlanID <> 331 Then

            objCert.DeductUnits(lngEncounterLogID, lngPatientID, lngProviderID, lngPrimaryPlanID, dtDOS, strCPTCode, dblUnits)
            objCert = Nothing
        End If

        If dblAddOnFee > 0 And strAddOnCPTCode > "" And lngPrimaryPlanID <> 331 Then

            objCert.DeductUnits(lngEncounterLogID, lngPatientID, lngProviderID, lngPrimaryPlanID, dtDOS, strAddOnCPTCode, dblAddOnUnits)
            objCert = Nothing
        End If

        If dblAddOnSecFee > 0 And strAddOnSecCPTCode > "" And lngPrimaryPlanID <> 331 Then
            objCert.DeductUnits(lngEncounterLogID, lngPatientID, lngProviderID, lngPrimaryPlanID, dtDOS, strAddOnSecCPTCode, dblAddOnSecUnits)
            objCert = Nothing
        End If



        Insert = lngEncDetID



    End Function
    Public Function Update(ByVal lngEncDetailID As Long, ByVal lngELID As Long, ByVal lngLine As Long, ByVal lngClinicID As Long, ByVal dtDOS As Date, _
                ByVal strCPTCode As String, ByVal dblFee As Double, ByVal dblUnits As Double, ByVal strDiagCode As String, _
                ByVal strModifier1 As String, ByVal strModifier2 As String, _
                ByVal strAddOnCPTCode As String, ByVal dblAddOnFee As Double, ByVal dblAddOnUnits As Double, _
                ByVal strAddOnMod1 As String, ByVal strAddOnMod2 As String, _
                ByVal strAddOnSecCPTCode As String, ByVal dblAddOnSecFee As Double, ByVal dblAddOnSecUnits As Double, _
                ByVal strAddOnSecMod1 As String, ByVal strAddOnSecMod2 As String, _
                ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 06/15/2017
        'Author: Duane C Orth
        'Description:  Inserts rows into Encounter Line Item Detail table as needed.
        'Parameters:
        '
        'Returns: Encounter Line ID of the data inserted.
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        Dim obj As New EncounterLogDB.CEncLineDB
        Dim objEL As New EncounterLogDB.CEncounterLogDB
        Dim objInsTx As New BillingBz.CInsuranceTxBz
        Dim objCert As New CertificationBz.CCertificationBz
        Dim objBR As New BillingBz.CBRBz
        Dim rst As New ADODB.Recordset
        Dim lngID, intCtr, lngPatientID, lngProviderID, lngPrimaryPlanID, lngPrimaryRPID, lngPrimaryCPCID, lngPrimaryInsID As Long
        Dim lngEncDetID, lngPrimaryBRID, lngTaxID, intTINType As Long
        Dim dblAmtResp, dblAmtRespPlan, dblAmtRespRP As Decimal
        Dim strErrMessage As String

        dblAmtRespRP = 0 : dblAmtRespPlan = 0


        rst = objEL.FetchByID(lngELID)
        objEL = Nothing
        lngPatientID = rst.Fields("fldPatientID").Value
        lngProviderID = rst.Fields("fldProviderID").Value

        'Store values from primary plan
        For intCtr = 1 To rst.RecordCount
            If rst.Fields("fldPlanID").Value = 331 Or _
               rst.Fields("fldOrder").Value = 2 Then
                '         lngPrimaryBRID = rst.Fields("fldBRID").Value 'Store ID of primary BRID
                lngPrimaryPlanID = rst.Fields("fldPlanID").Value 'Store ID of primary insurance plan
                Exit For
            End If
            If rst.Fields("fldOrder").Value = 1 Then dblAmtRespRP = dblAmtRespRP + rst.Fields("fldAmtResp").Value
            If rst.Fields("fldOrder").Value > 1 Then dblAmtRespPlan = dblAmtRespPlan + rst.Fields("fldAmtResp").Value
            rst.MoveNext()
        Next intCtr
        rst = Nothing
        dblAmtResp = (dblFee * dblUnits) + (dblAddOnFee * dblAddOnUnits) + (dblAddOnSecFee * dblAddOnSecUnits)


        obj.Update(lngEncDetailID, lngELID, lngLine, lngClinicID, dtDOS, strCPTCode, _
                dblFee, dblUnits, strDiagCode, strModifier1, strModifier2, strAddOnCPTCode, dblAddOnFee, dblAddOnUnits, _
                strAddOnMod1, strAddOnMod2, strAddOnSecCPTCode, dblAddOnSecFee, dblAddOnSecUnits, _
                strAddOnSecMod1, strAddOnSecMod2, strUserName)
        obj = Nothing

        'Update the primary insurance company charge record
        objInsTx.Update(lngELID, lngEncDetailID, lngPrimaryPlanID, TX_TYPE_INS_CHG, 0, dblAmtResp, Date.Now, "", "01\01\1900", "", 0, 0, strUserName)
        objInsTx = Nothing

        'Encounter Log Rolled back Certification units for the original claim
        'Deduct certification units for all plans.  Do not deduct cert units if service offered at no charge
        If dblFee > 0 Then
            objCert.DeductUnits(lngELID, lngPatientID, lngProviderID, lngPrimaryPlanID, dtDOS, strCPTCode, dblUnits)
        End If
        If dblAddOnFee > 0 And strAddOnCPTCode > "" And lngPrimaryPlanID <> 331 Then
            objCert.DeductUnits(lngELID, lngPatientID, lngProviderID, lngPrimaryPlanID, dtDOS, strAddOnCPTCode, dblAddOnUnits)
        End If
        If dblAddOnSecFee > 0 And strAddOnSecCPTCode > "" And lngPrimaryPlanID <> 331 Then
            objCert.DeductUnits(lngELID, lngPatientID, lngProviderID, lngPrimaryPlanID, dtDOS, strAddOnSecCPTCode, dblAddOnSecUnits)
        End If
        objCert = Nothing





    End Function
    Public Sub Delete(ByVal lngEncDetailID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 06/15/2017
        'Author: Duane C Orth
        'Description: 'Disables' selected record in tblEncDetail table.
        'Parameters: lngEncDetailID - ID of relationship to be Disables.
        'Returns: Nothing
        '--------------------------------------------------------------------
        Dim objCert As New CertificationBz.CCertificationBz
        Dim obj As New EncounterLogDB.CEncLineDB
        Dim strErrMessage As String


        'Rollback Cert Units, delete history from tblCertUsage

        objCert.RollBackDetLine(lngEncDetailID, strUserName)
        objCert = Nothing

        obj.Delete(lngEncDetailID, strUserName)
        obj = Nothing

    End Sub


End Class
