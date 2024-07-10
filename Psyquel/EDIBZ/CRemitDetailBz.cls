
'-------------------------------------------------------------------------------------
'Date: 04/20/2009
'Class Name: RemitDetailBz
'Author: Duane C Orth
'Description:   COM object designed to host methods associated with electronic
'               checks.
'--------------------------------------------------------------------------------------
Option Explicit On
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CRemitDetailBz


    Private Const CLASS_NAME As String = "CRemitDetailBz"
    Private Const TABLE_NAME As String = "tblRemittanceDetail"


    Private _ConnectionString As String = String.Empty
    Private _AppPath As String = String.Empty
    Private _EDIOutput As String = String.Empty
    Private _MachineName As String = String.Empty
    Private _UserLoginName As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property




    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False, _
                          Optional ByVal strWhere As String = "", _
                          Optional ByVal strOrderBy As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 04/20/2009
        'Author: Duane C Orth
        'Description:  Retrieves records from the tblRemittance table.
        'Parameters: blnIncludeDisabled - Optional parameter that identifies if
        '               records flagged as 'Disabled' or 'De-activated' are to be
        '               included in the record set. The default value is False.
        '            strWhere - Optional 'Where' clause of the SQL statement by
        '               which records will be filtered.  If the parameter is not
        '               supplied, all records will be retrieved.
        '            strOrderBy - Optional 'Order By' clause in which retrieved
        '               records will be sorted.  The default is set to the
        '               'fldName' column
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim rstSQL As New ADODB.Recordset
        Dim objRemitDetail As New EDIDB.CRemitDetailDB


        rstSQL = objRemitDetail.Fetch(blnIncludeDisabled, strWhere, strOrderBy)
        Fetch = rstSQL

        objRemitDetail = Nothing

    End Function

    Public Function Insert(ByVal lngRemittanceRecordID As Long, ByVal strPayorID As String, ByVal strPatientNumber As String, _
                           ByVal strLineNumber As String, ByVal strServiceLineNumber As String, ByVal dteFirstDateOfService As Date, _
                           ByVal dteLastDateOfService As Date, ByVal dteClaimReceiveDate As Date, _
                           ByVal strPlaceOfService As String, ByVal strTypeOfService As String, ByVal strProcedureCode As String, _
                           ByVal strMod1 As String, ByVal strMod2 As String, ByVal strMod3 As String, _
                           ByVal strUnits As String, ByVal dblCharge As Double, ByVal dblDisallowed As Double, ByVal dblAllowed As Double, _
                           ByVal dblDeductible As Double, ByVal dblCoinsurance As Double, ByVal dblPatientResp As Double, _
                           ByVal dblInterest As Double, ByVal dblPayment As Double, _
                           ByVal strProvider As String, ByVal strReason1 As String, ByVal strReason2 As String, ByVal strReason3 As String, _
                           ByVal strReason4 As String, ByVal strReason5 As String, ByVal strReason6 As String, ByVal strReason7 As String, _
                           ByVal blnPosted As Boolean, ByVal dtePostedDate As Date, _
                           ByVal lngEncounterLogID As Long, ByVal lngPatientID As Long, ByVal lngInsuranceID As Long, ByVal lngProviderID As Long, _
                           ByVal lngCheckID As Long, ByVal lngCheckLogID As Long, ByVal strErnFileName As String, ByVal strErrorMessage As String, ByVal strAddedBy As String) As Long
        '--------------------------------------------------------------------
        'Date: 04/20/2009
        'Author: Duane C Orth
        'Description:  Inserts a row into the tblRemittance table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------

        Dim objRemitDetail As New EDIDB.CRemitDetailDB
        Dim strErrMessage As String
        objRemitDetail.ConnectionString = _ConnectionString


        Insert = objRemitDetail.Insert(lngRemittanceRecordID, strPayorID, strPatientNumber, _
                           strLineNumber, strServiceLineNumber, dteFirstDateOfService, _
                           dteLastDateOfService, dteClaimReceiveDate, _
                           strPlaceOfService, strTypeOfService, strProcedureCode, _
                           strMod1, strMod2, strMod3, _
                           strUnits, dblCharge, dblDisallowed, dblAllowed, _
                           dblDeductible, dblCoinsurance, dblPatientResp, _
                           dblInterest, dblPayment, _
                           strProvider, strReason1, strReason2, strReason3, _
                           strReason4, strReason5, strReason6, strReason7, _
                           blnPosted, dtePostedDate, _
                           lngEncounterLogID, lngPatientID, lngInsuranceID, lngProviderID, _
                           lngCheckID, lngCheckLogID, strErnFileName, strErrorMessage, strAddedBy)
        objRemitDetail = Nothing


    End Function

    Public Sub Update(ByVal lngID As Long, ByVal lngRemittanceRecordID As Long, ByVal strPayorID As String, ByVal strPatientNumber As String, _
                           ByVal strLineNumber As String, ByVal strServiceLineNumber As String, ByVal dteFirstDateOfService As Date, _
                           ByVal dteLastDateOfService As Date, ByVal dteClaimReceiveDate As Date, _
                           ByVal strPlaceOfService As String, ByVal strTypeOfService As String, ByVal strProcedureCode As String, _
                           ByVal strMod1 As String, ByVal strMod2 As String, ByVal strMod3 As String, _
                           ByVal strUnits As String, ByVal dblCharge As Double, ByVal dblDisallowed As Double, ByVal dblAllowed As Double, _
                           ByVal dblDeductible As Double, ByVal dblCoinsurance As Double, ByVal dblPatientResp As Double, _
                           ByVal dblInterest As Double, ByVal dblPayment As Double, _
                           ByVal strProvider As String, ByVal strReason1 As String, ByVal strReason2 As String, ByVal strReason3 As String, _
                           ByVal strReason4 As String, ByVal strReason5 As String, ByVal strReason6 As String, ByVal strReason7 As String, _
                           ByVal blnPosted As Boolean, ByVal dtePostedDate As Date, _
                           ByVal lngEncounterLogID As Long, ByVal lngPatientID As Long, ByVal lngInsuranceID As Long, ByVal lngProviderID As Long, _
                           ByVal lngCheckID As Long, ByVal lngCheckLogID As Long, ByVal strErnFileName As Long, ByVal strErrorMessage As Long, ByVal strAddedBy As Long)
        '--------------------------------------------------------------------
        'Date: 04/20/2009
        'Author: Duane C Orth
        'Description:  Updates a row in the tblRemittance table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: Null
        '--------------------------------------------------------------------

        Dim objRemitDetail As New EDIDB.CRemitDetailDB
        Dim strErrMessage As String


        objRemitDetail.Update(lngID, lngRemittanceRecordID, strPayorID, strPatientNumber, _
                           strLineNumber, strServiceLineNumber, dteFirstDateOfService, _
                           dteLastDateOfService, dteClaimReceiveDate, _
                           strPlaceOfService, strTypeOfService, strProcedureCode, _
                           strMod1, strMod2, strMod3, _
                           strUnits, dblCharge, dblDisallowed, dblAllowed, _
                           dblDeductible, dblCoinsurance, dblPatientResp, _
                           dblInterest, dblPayment, _
                           strProvider, strReason1, strReason2, strReason3, _
                           strReason4, strReason5, strReason6, strReason7, _
                           blnPosted, dtePostedDate, _
                           lngEncounterLogID, lngPatientID, lngInsuranceID, lngProviderID, _
                           lngCheckID, lngCheckLogID, strErnFileName, strErrorMessage, strAddedBy)
        objRemitDetail = Nothing


    End Sub

End Class
