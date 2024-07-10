
'--------------------------------------------------------------------
'Class Name: CPatRPPlanRuleBz
'Date: 11/07/2000
'Author: Morgan Travis
'Description:  MTS object designed to fetch from the tblPatRPPlanRule table.
'--------------------------------------------------------------------
'Revision History:                                                  '
'
'--------------------------------------------------------------------
Option Explicit On
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CPatRPPlanRuleBz

    Private _ConnectionString As String = String.Empty

    Private Const TABLE_NAME As String = "tblPatRPPlanRule"



    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property




    Public Function Update(ByVal lngPatRPPlanID As Long, ByVal strRPRelat As String, ByVal strCardNum As String, ByVal strGroupNum As String, _
                                        ByVal strMedicareHICNum As String, ByVal curAmtOfGrpDeductPd As Decimal, ByVal dteFiscalEndDate As Date, _
                                        ByVal curDeduct As Decimal, ByVal strGroupDeduct As String, ByVal curDeductRem As Decimal, _
                                        ByVal curDollarLimit As Decimal, ByVal curDollarsApplied As Decimal, ByVal intVisitLimit As Integer, _
                                        ByVal intVisitsApplied As Integer, ByVal strCatCap As String, ByVal curCatCapAmt As Decimal, _
                                        ByVal strCertReqd As String, ByVal strNoHCFA As String, ByVal strNoInvoice As String, ByVal strOutpatientRpt As String, _
                                        ByVal strVerbalRept As String, ByVal intNumVisit As Integer, ByVal intDaysPrior As Integer, ByVal strElectClaim As String, ByVal strRecurrCopay As String)
        '---------------------------------------------------------------------
        'Date: 03/01/2000
        'Author: Eric Pena
        'Description:  Creates a billing association for the given patient-rp-plan combonation
        'Parameters: lngPatRPPlanID - The ID of the key in tblPatRPPlanRule
        '                  All other fields represent columns to be updated
        'Returns: T if the association is enabled/exists, F otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objPatRPPlan As New BenefactorDB.CPatRPPlanRuleDB
        objPatRPPlan.ConnectionString = _ConnectionString

        objPatRPPlan.Update(lngPatRPPlanID, strRPRelat, strCardNum, strGroupNum, strMedicareHICNum, curAmtOfGrpDeductPd, dteFiscalEndDate, _
                                         curDeduct, strGroupDeduct, curDeductRem, curDollarLimit, curDollarsApplied, intVisitLimit, intVisitsApplied, strCatCap, _
                                         curCatCapAmt, strCertReqd, strNoHCFA, strNoInvoice, strOutpatientRpt, strVerbalRept, intNumVisit, intDaysPrior, strElectClaim, strRecurrCopay)
        'Signal successful completion
        objPatRPPlan = Nothing

    End Function


    Public Function FetchPatRPPlanRule(ByVal lngPatID As Long, _
                                    ByVal lngRPID As Long, _
                                    ByVal lngPlanID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 04/19/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Returns a row from tblPatRPPlanRule with the         '
        '              matching parameters                                  '
        'Parameters: lngPatID - ID of Patient                               '
        '            lngRPID - ID of RP                                     '
        '            lngPlanID - ID of Plan                                 '
        'Returns: ADODB Recordset if match found, otherwise Null            '
        '--------------------------------------------------------------------
        'Note:  Notice how much easier to read this format is?...Eric       '
        '-------------------------------------------------------------------'

        Dim objBenefactor As New BenefactorDB.CPatRPPlanRuleDB

        objBenefactor.ConnectionString = _ConnectionString

        FetchPatRPPlanRule = objBenefactor.FetchPatRPPlanRule(lngPatID, lngRPID, lngPlanID)

        ' Signal successful completion

        objBenefactor = Nothing


    End Function

    Public Function FetchPatRPPlanRuleByPatRPPlanID(ByVal lngPatRPPlanID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 01/10/2001                                                   '
        'Author: Morgan Travis                                              '
        'Description:  Returns a row from tblPatRPPlanRule with the         '
        '              matching parameters                                  '
        'Parameters: lngPatRPPlanID                                         '
        'Returns: ADODB Recordset if match found, otherwise Null            '
        '--------------------------------------------------------------------


        Dim objBenefactor As New BenefactorDB.CPatRPPlanRuleDB

        objBenefactor.ConnectionString = _ConnectionString

        FetchPatRPPlanRuleByPatRPPlanID = objBenefactor.FetchPatRPPlanRuleByPatRPPlanID(lngPatRPPlanID)


        objBenefactor = Nothing

    End Function

    Public Function UpdatePatientCounters(ByVal lngPatRPPlanID As Long, ByVal curAmtApplied As Decimal)
        '--------------------------------------------------------------------
        'Date: 01/10/2001
        'Author: Morgan Travis
        'Description:  Updates Patients visit,amount applied, and deductible counters
        'Parameters:
        'Returns:
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        Dim lngCounter As Long
        Dim objBene As New BenefactorDB.CPatRPPlanRuleDB
        Dim rst As ADODB.Recordset
        Dim VisitCounter As Integer
        Dim AmountApplied As Long
        Dim Deductible As Long


        objBene.ConnectionString = _ConnectionString



        rst = FetchPatRPPlanRuleByPatRPPlanID(lngPatRPPlanID)

        If curAmtApplied > 0 Then

            VisitCounter = rst.Fields("fldVisitsApplied").Value + 1
            AmountApplied = rst.Fields("fldDollarsApplied").Value + curAmtApplied
            Deductible = rst.Fields("fldDeductRem").Value - curAmtApplied

        End If

        If curAmtApplied < 0 Then

            VisitCounter = rst.Fields("fldVisitsApplied").Value - 1
            AmountApplied = rst.Fields("fldDollarsApplied").Value + curAmtApplied
            Deductible = rst.Fields("fldDeductRem").Value - curAmtApplied

        End If

        objBene.UpdatePatientCounters(lngPatRPPlanID, VisitCounter, AmountApplied, Deductible)

        objBene = Nothing

    End Function


End Class

