
'--------------------------------------------------------------------
'Class Name: CCheckInBZ                                             '
'Date: 11/16/2000                                                   '
'Author: Chris Dereadt                                              '
'Description:  MTS business object designed to   methods         '
'              associated with the Check in classes.                '
'--------------------------------------------------------------------
Option Explicit On
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CCheckInBZ
    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CCheckInBZ"

    '--------------------------------------------------------------------
    ' Public Methods    +++++++++++++++++++++++++++++++++++++++++++++++++
    '--------------------------------------------------------------------

    Public Function Fetch(ByVal lngID As Long, ByVal dteStartDate As Date, ByVal dteEndDate As Date) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 11/16/2000                                                   '
        'Author: Chris Dereadt                                              '
        'Description: Fetches all information about appointment check in    '
        'Parameters:  lngID - ID of the provider / office manager           '
        '             dteStartDate - Start date of the search date range    '
        '             dteEnsdDate - Start End of the search date range      '
        'Returns:   ADODB Recordset of Check in info                        '
        '--------------------------------------------------------------------
        Dim objCheckIn As New BenefitsDB.CCheckInDB
        Dim rst As New ADODB.Recordset

        ' Populate the recordset
        rst = objCheckIn.Fetch(lngID, dteStartDate, dteEndDate)

        Fetch = rst

        rst = Nothing

        objCheckIn = Nothing



    End Function

    Public Function FetchAmountOwed(ByVal lngPatRPPlanID As Long, ByVal lngPatientID As Long, ByVal lngRPID As Long, _
                                    ByVal lngPlanID As Long, ByVal lngProviderID As Long, ByVal strCPTCode As String, _
                                    ByVal strCPTCode2 As String, ByVal lngProviderCredential As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 01/15/2001                                                   '
        'Author: Morgan Travis                                              '
        'Description: Fetches all information amount owed                   '
        'Parameters:        '
        'Returns:   ADODB Recordset of Check in info                        '
        '--------------------------------------------------------------------
        Dim objCheckIn As New BenefitsDB.CCheckInDB
        Dim rst As New ADODB.Recordset


        ' Populate the recordset
        rst = objCheckIn.FetchAmountOwed(lngPatRPPlanID, lngPatientID, lngRPID, lngPlanID, lngProviderID, strCPTCode, strCPTCode2, lngProviderCredential)

        FetchAmountOwed = rst

        rst = Nothing

        objCheckIn = Nothing



    End Function

    Public Function FetchCopayInfo(ByVal lngPatientID As Long, ByVal lngProviderID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 02/12/2001                                                   '
        'Author: Chris Dereadt                                              '
        'Description:   Fetches Copay information                           '
        'Parameters:  lngPatientID - Id for the patient                     '
        'Returns:   ADODB Recordset of Check in info                        '
        '--------------------------------------------------------------------
        Dim objCheckIn As New BenefitsDB.CCheckInDB
        Dim rst As New ADODB.Recordset



        ' Populate the recordset
        rst = objCheckIn.FetchCopayInfo(lngPatientID, lngProviderID)

        FetchCopayInfo = rst

        rst = Nothing
        objCheckIn = Nothing

 
    End Function

    Public Function FetchCopayData(ByVal lngProviderID As Long, ByVal dteStartDate As Date, ByVal dteEndDate As Date, ByVal lngPatientID As Long) As Collection
        '--------------------------------------------------------------------
        'Date: 03/08/2001                                                   '
        'Author: Chris Dereadt                                              '
        'Description:   Fetches Copay information                           '
        'Parameters:  lngPatientID - Id for the patient                     '
        'Returns:   ADODB Recordset of Check in info                        '
        '--------------------------------------------------------------------
        Dim objCheckInData As New BenefitsDB.CCheckInDB
        Dim col As New Collection



        ' Populate the recordset
        col = objCheckInData.FetchCopayData(lngProviderID, dteStartDate, dteEndDate, lngPatientID)

        FetchCopayData = col


        objCheckInData = Nothing
        col = Nothing



    End Function



End Class


