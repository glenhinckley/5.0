Option Explicit On




Imports ListBZ
Imports Psyquel.BusinessRules.CoreLibraryIII
'--------------------------------------------------------------------
'Class Name: CPlanFeeAllowableBZ
'Date: 10/31/2000
'Author: Morgan Travis
'Description:  MTS object designed to host methods associated with
'               data affecting Plan Fee Allowable table.
'--------------------------------------------------------------------
'   R000 - epena - 12/8/00 - fixed object so that is works with CPlanFeeAllowableDB
'--------------------------------------------------------------------

Public Class CPlanFeeAllowableBZ
    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CPlanFeeAllowableBZ"

    Public Function Insert(ByVal lngPlanID As Long, ByVal strCPTCode As String, ByVal lngProviderCredential As Long, ByVal curFeeAllowable As Decimal) As Long

        '--------------------------------------------------------------------
        'Date: 10/31/2000
        'Author: Morgan Travis
        'Description:  Inserts a row into the tblPlanFeeAllowable table utilizing
        '              a stored procedure.
        'Parameters: lngPlanID -  Plan ID number to insert into table
        '            strCPTCode - The ten digit CPT Code that will be inserted into the table.
        '            lngProviderCredential - Numerical representation of providers credentials.
        '            curFeeAllowable - Amount paid to provider
        '--------------------------------------------------------------------

        Dim objPlanFeeAllowable As New BenefitsDB.CPlanFeeAllowableDB
        Dim strErrMsg As String


        If VerifyData(lngPlanID, strCPTCode, lngProviderCredential, curFeeAllowable, strErrMsg, 0) Then
            Insert = objPlanFeeAllowable.Insert(lngPlanID, strCPTCode, lngProviderCredential, curFeeAllowable)
        Else
            Exit Function
        End If
        objPlanFeeAllowable = Nothing


    End Function
    Public Function Update(ByVal lngPlanFeeID As Long, ByVal curFeeAllowable As Decimal) As Long 'r000
        '--------------------------------------------------------------------
        'Date: 10/31/2000
        'Author: Morgan Travis
        'Description:  Updates a row into the tblPlanCopay table utilizing
        '              a stored procedure.
        'Parameters: lngPlanFeeID -  Plan Fee ID number to insert into table
        '            curFeeAllowable - Amount paid to provider
        '            lngPlanID - Plan ID
        '            strCPTode - CPT Code
        '--------------------------------------------------------------------

        Dim objPlanFeeAllowable As New BenefitsDB.CPlanFeeAllowableDB
        Dim strErrMsg As String


        If curFeeAllowable < 0 Then
            strErrMsg = "Fee allowables must be positive amounts"
            Exit Function
        End If

        objPlanFeeAllowable.Update(lngPlanFeeID, curFeeAllowable)

        objPlanFeeAllowable = Nothing




    End Function


    Public Function Exists(ByVal lngPlanID As Long, ByVal strCPTCode As String, ByVal lngProviderCredential As Long) As Boolean
        '--------------------------------------------------------------------
        'Date: 10/31/2000
        'Author: Morgan Travis
        'Description:  Checks for existing entries into tblPlanCopay
        'Parameters: lngPlanFeeID - Plan Fee ID number to check for existing entries in table
        '
        '
        '--------------------------------------------------------------------
        Dim objPlanFeeAllowable As New BenefitsDB.CPlanFeeAllowableDB
        Dim blnExists As Boolean

        blnExists = objPlanFeeAllowable.Exists(lngPlanID, strCPTCode, lngProviderCredential)
        objPlanFeeAllowable = Nothing
        Exists = blnExists


    End Function

    Public Function FetchByPlanCPTCredential(ByVal lngPlanID As Long, ByVal strCPTCode As String, ByVal lngProviderCredential As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/31/2000                                                   '
        'Author: Morgan Travis                                              '
        'Description:  Retrieves records from the tblPlanFeeAllowable table.     '
        'Parameters: lngPlanID -  Plan ID number
        '            strCPTCode - The ten digit CPT Code.
        '            lngProviderCredential - Numerical representation of providers credentials.
        '
        'Returns: Recordset of requested providers
        '-------------------------------------------------------------------'

        Dim objPlanFeeAllowable As New BenefitsDB.CPlanFeeAllowableDB
        Dim rstSQL As ADODB.Recordset

        rstSQL = objPlanFeeAllowable.FetchByPlanCPTCredential(lngPlanID, strCPTCode, lngProviderCredential)

        FetchByPlanCPTCredential = rstSQL
        objPlanFeeAllowable = Nothing




    End Function

    Public Function FetchByPlan(ByVal lngPlanID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/31/2000                                                   '
        'Author: Morgan Travis                                              '
        'Description:  Retrieves records from the tblPlanFeeAllowable table.     '
        'Parameters: lngPlanID -  Plan ID number
        '
        'Returns: Recordset of requested providers
        '-------------------------------------------------------------------'

        Dim objPlanFeeAllowable As New BenefitsDB.CPlanFeeAllowableDB
        Dim rstSQL As New ADODB.Recordset




        FetchByPlan = rstSQL
        objPlanFeeAllowable = Nothing




    End Function

    Private Function VerifyData(ByVal lngPlanID As Long, ByVal strCPTCode As String, ByVal lngProviderCredential As Long, _
                                ByVal curFeeAllowable As Decimal, ByRef strErrMsg As String, ByVal lngPlanFeeID As Long) As Boolean

        Dim objCPT As New ListBz.CCPTCodeBz

        VerifyData = False
        If lngPlanFeeID = 0 Then

            If lngPlanID < 1 Then
                strErrMsg = "Plan RP Plan ID Not Valid"
                Exit Function
            End If

            If lngProviderCredential < 1 Then
                strErrMsg = "Provider Credential ID Not Valid"
                Exit Function
            End If


            If Not objCPT.Exists(strCPTCode) Then
                strErrMsg = "Invalid CPT Code"
                Exit Function
            End If

            If curFeeAllowable < 0 Then
                strErrMsg = "Invalid amount"
                Exit Function
            End If

            If Exists(lngPlanID, strCPTCode, lngProviderCredential) Then
                strErrMsg = "Provider/CPT Code combination already exists"
                Exit Function
            End If
            VerifyData = True
        Else
            If curFeeAllowable < 0 Then
                strErrMsg = "Invalid amount"
                Exit Function
            End If
            VerifyData = True
        End If



    End Function

End Class
