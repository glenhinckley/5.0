
'--------------------------------------------------------------------
'Class Name: CPatBenVerBZ
'Date: 10/26/2000
'Author: Morgan Travis
'Description:  MTS object designed to host methods associated with
'               data affecting Patient Benefits Verification table.
'--------------------------------------------------------------------
'--------------------------------------------------------------------
Option Explicit On
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CPatBenVerBZ
   
    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CPatBenVerBZ"


    Public Function Insert(ByVal lngPatRPPlanID As Long, ByVal strUser As String, ByVal dteDateTime As Date, ByVal strContact As String, ByVal strNotes As String) As Long
        '--------------------------------------------------------------------
        'Date: 10/26/2000
        'Author: Morgan Travis
        'Description:  Inserts a row into the tblPatientBenefitVerfication table utilizing
        '              a stored procedure.
        'Parameters: lngPatRPPlanID - Patient Resposible Party Plan ID number to insert into table
        '            strUser - System User that will be inserted into the table.
        '            dteDateTime - The date/time of the entry that will be inserted into the table.
        '            strContact - The Name of the person contacted about the verification
        '            strNotes - Notes about the verification
        '--------------------------------------------------------------------

        Dim objPatBenVer As New BenefitsDB.CPatBEnVerDB
        Dim strErrMsg As String

        If VerifyData(strContact, strErrMsg) Then
            objPatBenVer.Insert(lngPatRPPlanID, strUser, dteDateTime, strContact, strNotes)
        Else
            Exit Function
        End If
        objPatBenVer = Nothing



    End Function

    Public Function FetchByPatRPPlan(ByVal lngPatRPPlanID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/26/2000                                                   '
        'Author: Morgan Travis                                              '
        'Description:  Retrieves records from the tblPatientBenefitVerfication table.     '
        'Parameters: lngProviderID - Provider ID number to fetch from table '
        '
        'Returns: Recordset of requested providers
        '-------------------------------------------------------------------'


        Dim objPatBenVer As New BenefitsDB.CPatBEnVerDB
        Dim strErrMsg As String
        Dim rstSQL As New ADODB.Recordset()


        rstSQL = objPatBenVer.FetchByPatRPPlan(lngPatRPPlanID)

        FetchByPatRPPlan = rstSQL
        objPatBenVer = Nothing



    End Function



    Public Function FetchPendingVerif(ByVal lngUserID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/26/2000                                                   '
        'Author: Morgan Travis                                              '
        'Description:  Retrieves records from the tblPatientBenefitVerfication table.     '
        'Parameters: lngUserID - Provider ID number to fetch from table '
        '
        'Returns: Recordset of requested providers
        '-------------------------------------------------------------------'


        Dim objPatBenVer As New BenefitsDB.CPatBEnVerDB
        Dim strErrMsg As String
        Dim rstSQL As ADODB.Recordset

        rstSQL = objPatBenVer.FetchPendingVerif(lngUserID)

        FetchPendingVerif = rstSQL
        objPatBenVer = Nothing



    End Function

    Private Function VerifyData(ByVal strContact As String, ByRef strErrMsg As String) As Boolean

        VerifyData = False
        If strContact = "" Then
            strErrMsg = "Contact Not Valid"
            Exit Function
        End If

        VerifyData = True

    End Function

End Class

