'------------------------------------
'Class Name: CPatientBenefitBZ
'Date: 12/20/2023
'Author: DCO
'Description:  MTS object designed to host methods associated with
'               data affecting Patient Benefits Verification table.
'--------------------------------------------------------------------
'--------------------------------------------------------------------
Option Explicit On

imports BenefactorDB
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CEligibilityTxBZ

    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CEligibilityTxBZ"

    Public Function FetchByID(ByVal lngID As Long, Optional ByVal strDataBase As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 12/20/2023
        'Author: DCO
        'Description:  Retrieves records from the tblEligibilityTx table.   '
        'Parameters: lngID - EligibilityID number to fetch from table       '
        '
        'Returns: Recordset of requested Eligibility
        '-------------------------------------------------------------------'


        Dim obj As New BenefitsDB.CEligibilityTxDB
        Dim strErrMsg As String
        Dim rstSQL As New ADODB.Recordset


        rstSQL = obj.FetchByID(lngID)

        FetchByID = rstSQL
        obj = Nothing




    End Function


    Public Function Insert(ByVal lngID As Long, ByVal lngVisitLimit As Long, ByVal lngVisitsApplied As Long, ByVal strHasCoverageYN As String, ByVal strOutOfNetworkYN As String, _
             ByVal lngNumVisit As Long, ByVal strBenefitsText As String, ByVal strNotes As String, _
             ByVal strPlanName As String, ByVal strPlanAddress As String, ByVal strPlanCity As String, ByVal strPlanState As String, ByVal strPlanZip As String, _
             ByVal dteEffDate As Date, ByVal dteTermDate As Date, ByVal strMSPCode As String, ByVal dteVerifyDate As Date, ByVal strQueuedYN As String, _
             ByRef varINN As Object, ByRef varOON As Object, ByRef varCoPay As Object, ByRef varCert As Object, _
             ByVal dteDateProcessed As Date, ByVal strUserName As String, Optional ByVal strDataBase As String = "") As Integer
        '--------------------------------------------------------------------
        'Date: 12/20/2023
        'Author: DCO
        'Description:  Inserts a row into the tblEligibilityTx table utilizing
        '              a stored procedure.
        'Parameters: All data values to be inserted into the table
        'Returns: ID (Primary Key) of the inserted row.
        '--------------------------------------------------------------------

        Dim obj As New BenefitsDB.CEligibilityTxDB
        Dim strErrMsg As String

        Dim r As Integer = 0

        obj.Insert(lngID, lngVisitLimit, lngVisitsApplied, strHasCoverageYN, strOutOfNetworkYN, _
             lngNumVisit, strBenefitsText, strNotes, strPlanName, strPlanAddress, strPlanCity, strPlanState, strPlanZip, _
             dteEffDate, dteTermDate, strMSPCode, dteVerifyDate, strQueuedYN, varINN, varOON, varCoPay, varCert, _
             dteDateProcessed, strUserName, strDataBase)
        obj = Nothing

        Return r

    End Function

    Public Sub Update(ByVal lngID As Long, ByVal lngVisitLimit As Long, ByVal lngVisitsApplied As Long, ByVal strHasCoverageYN As String, ByVal strOutOfNetworkYN As String, _
             ByVal lngNumVisit As Long, ByVal strBenefitsText As String, ByVal strNotes As String, _
             ByVal strPlanName As String, ByVal strPlanAddress As String, ByVal strPlanCity As String, ByVal strPlanState As String, ByVal strPlanZip As String, _
             ByVal dteEffDate As Date, ByVal dteTermDate As Date, ByVal strMSPCode As String, ByVal dteVerifyDate As Date, ByVal strQueuedYN As String, _
             ByRef varINN As Object, ByRef varOON As Object, ByRef varCoPay As Object, ByRef varCert As Object, _
             ByVal dteDateProcessed As Date, ByVal strUserName As String, Optional ByVal strDataBase As String = "")
        '--------------------------------------------------------------------
        'Date: 12/20/2023
        'Author: DCO
        'Description:  Updates a row into tblEligibilityTx table utilizing
        '              a stored procedure.
        'Parameters:
        '--------------------------------------------------------------------

        Dim obj As New BenefitsDB.CEligibilityTxDB
        Dim strErrMsg As String


        obj.Update(lngID, lngVisitLimit, lngVisitsApplied, strHasCoverageYN, strOutOfNetworkYN, _
             lngNumVisit, strBenefitsText, strNotes, strPlanName, strPlanAddress, strPlanCity, strPlanState, strPlanZip, _
             dteEffDate, dteTermDate, strMSPCode, dteVerifyDate, strQueuedYN, varINN, varOON, varCoPay, varCert, _
             dteDateProcessed, strUserName, strDataBase)
        obj = Nothing



    End Sub

    Private Function VerifyData(ByVal lngID As Long, _
                           ByVal dteFiscalStart As Date, ByVal dteFiscalEnd As Date, _
                           ByVal curDeductible As Decimal, ByVal strGroupDeductible As String, _
                           ByVal curDeductibleMet As Decimal, ByVal curAnnualDollarLimit As Decimal, _
                           ByVal curAmountApplied As Decimal, ByVal lngAnnualVisitLimit As Long, _
                           ByVal lngVisits As Long, ByVal strCatastrophicCap As String, _
                           ByVal curCatastrophicCapAmt As Decimal, ByVal strCertRequired As String, _
                           ByVal strNoHCFA As String, ByVal strNoInvoice As String, _
                           ByVal strOutPatientRept As String, ByVal strVerbRept As String, _
                           ByVal lngNumVisit As Long, ByVal lngDaysPrior As Long, _
                           ByVal strElectClaim As String, ByVal strRecurrCopay As String, ByVal strErrMsg As String, ByVal blnExists As Boolean) As Boolean

        VerifyData = False
        If lngID < 0 Then
            strErrMsg = "Patient RP Plan ID Not Valid"
            Exit Function
        End If

        If IsDate(dteFiscalStart) = False Or IsDate(dteFiscalEnd) = False Then
            strErrMsg = "Fiscal Start Date or Fiscal End Date Not Valid"
            Exit Function
        End If

        If dteFiscalStart > dteFiscalEnd Then
            strErrMsg = "Fiscal Start Date Greater Than Fiscal End Date"
            Exit Function
        End If

        If curDeductible < 0 Then
            strErrMsg = "Invalid Deductible"
            Exit Function
        End If

        If curDeductibleMet < 0 Then
            strErrMsg = "Invalid Deductible Met"
            Exit Function
        End If

        If Not ((strGroupDeductible <> "Y") Or (strGroupDeductible <> "y") Or _
           (strGroupDeductible <> "N") Or (strGroupDeductible <> "n")) Then
            strErrMsg = "Group Deductible Not Valid"
            Exit Function
        End If

        If curAnnualDollarLimit < 0 Then
            strErrMsg = "Invalid Annual Dollar Limit"
            Exit Function
        End If

        If curAmountApplied < 0 Then
            strErrMsg = "Invalid Amount Applied"
            Exit Function
        End If

        If curCatastrophicCapAmt < 0 Then
            strErrMsg = "Invalid Catastrophic Cap Amount"
            Exit Function
        End If

        If lngAnnualVisitLimit < 0 Then
            strErrMsg = "Annual Visit Limt Not Valid"
            Exit Function
        End If

        If lngVisits < 0 Then
            strErrMsg = "Visits Not Valid"
            Exit Function
        End If

        If Not ((strCatastrophicCap <> "Y") Or (strCatastrophicCap <> "y") Or _
           (strCatastrophicCap <> "N") Or (strCatastrophicCap <> "n")) Then
            strErrMsg = "Catastrophic Cap Not Valid"
            Exit Function
        End If

        If Not ((strCertRequired <> "Y") Or (strCertRequired <> "y") Or _
           (strCertRequired <> "N") Or (strCertRequired <> "n")) Then
            strErrMsg = "Cert Required Not Valid"
            Exit Function
        End If

        If Not ((strNoHCFA <> "Y") Or (strNoHCFA <> "y") Or _
           (strNoHCFA <> "N") Or (strNoHCFA <> "n")) Then
            strErrMsg = "No HCFA Not Valid"
            Exit Function
        End If

        If Not ((strNoInvoice <> "Y") Or (strNoInvoice <> "y") Or _
           (strNoInvoice <> "N") Or (strNoInvoice <> "n")) Then
            strErrMsg = "No Invoice Not Valid"
            Exit Function
        End If

        If Not ((strOutPatientRept <> "Y") Or (strOutPatientRept <> "y") Or _
           (strOutPatientRept <> "N") Or (strOutPatientRept <> "n")) Then
            strErrMsg = "Out Patient Report Not Valid"
            Exit Function
        End If

        If Not ((strVerbRept <> "Y") Or (strVerbRept <> "y") Or _
           (strVerbRept <> "N") Or (strVerbRept <> "n")) Then
            strErrMsg = "Verbal Report Not Valid"
            Exit Function
        End If

        If Not ((strElectClaim <> "Y") Or (strElectClaim <> "y") Or _
           (strElectClaim <> "N") Or (strElectClaim <> "n")) Then
            strErrMsg = "Elect Claim Not Valid"
            Exit Function
        End If

        If Not ((strRecurrCopay <> "Y") Or (strRecurrCopay <> "y") Or _
           (strRecurrCopay <> "N") Or (strRecurrCopay <> "n")) Then
            strErrMsg = "Recurring Copay Not Valid"
            Exit Function
        End If

        If lngNumVisit < 0 Then
            strErrMsg = "Number of Visits Not Valid"
            Exit Function
        End If

        If lngDaysPrior < 0 Then
            strErrMsg = "Days Prior Not Valid"
            Exit Function
        End If

        If Not blnExists Then
            If Exists(lngID) Then
                strErrMsg = "Patient Responsible Party Plan ID already exists"
                Exit Function
            End If

        End If

        VerifyData = True

    End Function

    Public Function Exists(ByVal lngID As Long, Optional ByVal strDataBase As String = "") As Boolean
        '--------------------------------------------------------------------
        'Date: 12/20/2023
        'Author: DCO
        'Description:  CHecks for existing entries in tblEligibilityTx.
        'Parameters: lngID - EligibilityID number to check for existing entries in table
        '
        '
        '--------------------------------------------------------------------
        Dim obj As New BenefitsDB.CEligibilityTxDB
        Dim blnExists As Boolean

        blnExists = obj.Exists(lngID, strDataBase)
        obj = Nothing
        Exists = blnExists



    End Function
End Class
