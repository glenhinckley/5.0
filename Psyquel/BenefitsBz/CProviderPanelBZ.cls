

'--------------------------------------------------------------------
'Class Name: CPlanCopayBZ
'Date: 10/31/2000
'Author: Morgan Travis
'Description:  MTS object designed to host methods associated with
'               data affecting Plan Copay table.
'--------------------------------------------------------------------
Option Explicit On
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CProviderPanelBZ

    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CProviderPanelBZ"

    Public Function Insert(ByVal lngPlanID As Long, ByVal lngProviderID As Long) As Integer
        '--------------------------------------------------------------------
        'Date: 10/31/2000
        'Author: Morgan Travis
        'Description:  Inserts a row into the tblProviderPlan table utilizing
        '              a stored procedure.
        'Parameters: lngPlanID -  Plan ID number to insert into table
        '            lngProviderID - Provider ID number.
        '
        '--------------------------------------------------------------------

        Dim objProviderPlan As New BenefitsDB.CProviderPanelDB
        Dim strErrMsg As String


        If VerifyData(lngPlanID, lngProviderID, strErrMsg) Then
            Insert = objProviderPlan.Insert(lngPlanID, lngProviderID)
        Else
            Exit Function
        End If
        objProviderPlan = Nothing


    End Function


    Public Function Delete(ByVal lngProviderPanelID As Long, ByVal lngProviderID As Long, ByVal lngPlanID As Long) As Integer
        '--------------------------------------------------------------------
        'Date: 10/31/2000
        'Author: Morgan Travis
        'Description:  Delete a row into the tblProviderPlan table utilizing
        '              a stored procedure.
        'Parameters: lngProviderPanelID -  Provider Panel ID number to delete from table
        '
        '
        '--------------------------------------------------------------------

        Dim objProviderPlan As New BenefitsDB.CProviderPanelDB
        Dim strErrMsg As String


        If Exists(lngProviderID, lngPlanID) Then
            objProviderPlan.Delete(lngProviderPanelID)
        Else
            Exit Function
        End If
        objProviderPlan = Nothing




    End Function


    Public Function Exists(ByVal lngProviderID As Long, ByVal lngPlanID As Long) As Boolean
        '--------------------------------------------------------------------
        'Date: 10/31/2000
        'Author: Morgan Travis
        'Description:  Checks for existing entries into tblPlanCopay
        'Parameters: lngProviderPanelID - Provider Panel ID Number.
        '
        '--------------------------------------------------------------------
        Dim objProviderPlan As New BenefitsDB.CProviderPanelDB
        Dim blnExists As Boolean


        blnExists = objProviderPlan.Exists(lngProviderID, lngPlanID)
        objProviderPlan = Nothing
        Exists = blnExists


    End Function

    Public Function FetchByPlan(ByVal lngPlanID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/31/2000                                                   '
        'Author: Morgan Travis                                              '
        'Description:  Retrieves records from the tblProviderPanel table.   '
        'Parameters: lngPlanID -  Plan ID number to match
        '
        '
        'Returns: Recordset of requested providers
        '-------------------------------------------------------------------'

        Dim objProviderPlan As New BenefitsDB.CProviderPanelDB
        Dim rstSQL As New ADODB.Recordset


        rstSQL = objProviderPlan.FetchByPlan(lngPlanID)

        FetchByPlan = rstSQL
        objProviderPlan = Nothing




    End Function

    Public Function FetchByProvider(ByVal lngProviderID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/31/2000                                                   '
        'Author: Morgan Travis                                              '
        'Description:  Retrieves records from the tblProviderPanel table.   '
        'Parameters: lngProviderID -  Provider ID number to match
        '
        '
        'Returns: Recordset of requested providers
        '-------------------------------------------------------------------'

        Dim objProviderPlan As New BenefitsDB.CProviderPanelDB
        Dim rstSQL As ADODB.Recordset


        rstSQL = objProviderPlan.FetchByProvider(lngProviderID)

        FetchByProvider = rstSQL
        objProviderPlan = Nothing



    End Function

    Public Function FetchByPlanProvider(ByVal lngPlanID As Long, ByVal lngProviderID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/31/2000                                                   '
        'Author: Morgan Travis                                              '
        'Description:  Retrieves records from the tblProviderPanel table.   '
        'Parameters: lngPlanID -  Plan ID number to match
        '
        '
        'Returns: Recordset of requested providers
        '-------------------------------------------------------------------'

        Dim objProviderPlan As New BenefitsDB.CProviderPanelDB
        Dim rstSQL As ADODB.Recordset

        rstSQL = objProviderPlan.FetchByPlanProvider(lngPlanID, lngProviderID)

        FetchByPlanProvider = rstSQL
        objProviderPlan = Nothing



    End Function


    Private Function VerifyData(ByVal lngPlanID As Long, ByVal lngProviderID As Long, ByRef strErrMsg As String) As Boolean

        VerifyData = False

        If lngPlanID < 1 Then
            strErrMsg = "Plan ID Not Valid"
            Exit Function
        End If

        If lngProviderID < 1 Then
            strErrMsg = "Provider ID Not Valid"
            Exit Function
        End If

        If Exists(lngProviderID, lngPlanID) Then
            strErrMsg = "Provider/Plan combination already exists"
            Exit Function
        End If

        VerifyData = True

    End Function


End Class

