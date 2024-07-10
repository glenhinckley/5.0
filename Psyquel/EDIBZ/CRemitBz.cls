
'-------------------------------------------------------------------------------------
'Date: 04/20/2009
'Class Name: RemitBz
'Author: Duane C Orth
'Description:   COM object designed to host methods associated with electronic
'               checks.
'--------------------------------------------------------------------------------------
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
Imports Psyquel.BusinessRules.CoreLibraryIII
' Lydia Orth 


Public Class CRemitBZ


    Private Const CLASS_NAME As String = "CRemitBz"
    Private Const TABLE_NAME As String = "tblRemittance"

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


        Dim rstSQL As ADODB.Recordset
        Dim objRemit As New EDIDB.CRemitDB

        rstSQL = objRemit.Fetch(blnIncludeDisabled, strWhere, strOrderBy)
        Fetch = rstSQL

        objRemit = Nothing


    End Function

    Public Function Insert(ByVal strPayorID As String, ByVal strHealthPlanID As String, ByVal strHealthPlanName As String, _
                           ByVal dteCreationDate As Date, ByVal strBatchNumber As String, ByVal strProvider As String, ByVal strProviderNbr As String, _
                           ByVal strCheckNumber As String, ByVal detCheckDate As Date, ByVal dblCheckAmount As Double, ByVal dblAmountPaid As Double, _
                           ByVal dblInterestAmount As Double, ByVal dteRemittanceDate As Date, ByVal dteProcessDate As Date, ByVal strPatientNumber As String, _
                           ByVal strInsuredGroupNbr As String, ByVal strInsuredPolicyID As String, ByVal strInsuredLastName As String, ByVal strInsuredFirstName As String, _
                           ByVal strInsuredMiddleInitial As String, ByVal strPatientGroupNbr As String, ByVal strPatientPolicyID As String, ByVal strPatientLastName As String, _
                           ByVal strPatientFirstName As String, ByVal strPatientMiddleInitial As String, ByVal strPatientSex As String, _
                           ByVal dtePatientDateOfBirth As Date, ByVal blnCompInsuranceFlag As Boolean, ByVal strMedicalRecordNumber As String, _
                           ByVal strMessageCode1 As String, ByVal strMessageCode2 As String, ByVal strMessageCode3 As String, ByVal strMessageCode4 As String, ByVal strMessageCode5 As String, _
                           ByVal strSupplementalInsurerName1 As String, ByVal strSupplementalInsurerID1 As String, ByVal strSupplementalInsurerName2 As String, ByVal strSupplementalInsurerID2 As String, _
                           ByVal lngInsuranceID As Long, ByVal lngProviderID As Long, ByVal lngPatientID As Long, ByVal lngEncounterLogID As Long, ByVal lngCheckID As Long, _
                           ByVal strErnFileName As String, ByVal blnPosted As Boolean, ByVal dtePostedDate As Date, ByVal strErrorMessage As String, _
                           ByVal strAddedBy As String) As Long
        '--------------------------------------------------------------------
        'Date: 04/20/2009
        'Author: Duane C Orth
        'Description:  Inserts a row into the tblRemittance table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------

        Dim objRemit As New EDIDB.CRemitDB
        Dim strErrMessage As String

        Insert = objRemit.Insert(strPayorID, strHealthPlanID, strHealthPlanName, _
                           dteCreationDate, strBatchNumber, strProvider, strProviderNbr, _
                           strCheckNumber, detCheckDate, dblCheckAmount, dblAmountPaid, _
                           dblInterestAmount, dteRemittanceDate, dteProcessDate, strPatientNumber, _
                           strInsuredGroupNbr, strInsuredPolicyID, strInsuredLastName, strInsuredFirstName, _
                           strInsuredMiddleInitial, strPatientGroupNbr, strPatientPolicyID, strPatientLastName, _
                           strPatientFirstName, strPatientMiddleInitial, strPatientSex, _
                           dtePatientDateOfBirth, blnCompInsuranceFlag, strMedicalRecordNumber, _
                           strMessageCode1, strMessageCode2, strMessageCode3, strMessageCode4, strMessageCode5, _
                           strSupplementalInsurerName1, strSupplementalInsurerID1, strSupplementalInsurerName2, strSupplementalInsurerID2, _
                           lngInsuranceID, lngProviderID, lngPatientID, lngEncounterLogID, lngCheckID, _
                           strErnFileName, blnPosted, dtePostedDate, strErrorMessage, _
                           strAddedBy)
        objRemit = Nothing


    End Function

    Public Sub Update(ByVal lngID As Long, ByVal strPayorID As String, ByVal strHealthPlanID As String, ByVal strHealthPlanName As String, _
                      ByVal dteCreationDate As Date, ByVal strBatchNumber As String, ByVal strProvider As String, ByVal strProviderNbr As String, _
                      ByVal strCheckNumber As String, ByVal detCheckDate As Date, ByVal dblCheckAmount As Double, ByVal dblAmountPaid As Double, _
                      ByVal dblInterestAmount As Double, ByVal dteRemittanceDate As Date, ByVal dteProcessDate As Date, ByVal strPatientNumber As String, _
                      ByVal strInsuredGroupNbr As String, ByVal strInsuredPolicyID As String, ByVal strInsuredLastName As String, ByVal strInsuredFirstName As String, _
                      ByVal strInsuredMiddleInitial As String, ByVal strPatientGroupNbr As String, ByVal strPatientPolicyID As String, ByVal strPatientLastName As String, _
                      ByVal strPatientFirstName As String, ByVal strPatientMiddleInitial As String, ByVal strPatientSex As String, _
                      ByVal dtePatientDateOfBirth As Date, ByVal blnCompInsuranceFlag As Boolean, ByVal strMedicalRecordNumber As String, _
                      ByVal strMessageCode1 As String, ByVal strMessageCode2 As String, ByVal strMessageCode3 As String, ByVal strMessageCode4 As String, ByVal strMessageCode5 As String, _
                      ByVal strSupplementalInsurerName1 As String, ByVal strSupplementalInsurerID1 As String, ByVal strSupplementalInsurerName2 As String, ByVal strSupplementalInsurerID2 As String, _
                      ByVal lngInsuranceID As Long, ByVal lngProviderID As Long, ByVal lngPatientID As Long, ByVal lngEncounterLogID As Long, ByVal lngCheckID As Long, _
                      ByVal strErnFileName As String, ByVal blnPosted As Boolean, ByVal dtePostedDate As Date, ByVal strErrorMessage As String, _
                      ByVal strAddedBy As String)
        '--------------------------------------------------------------------
        'Date: 04/20/2009
        'Author: Duane C Orth
        'Description:  Updates a row in the tblRemittance table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: Null
        '--------------------------------------------------------------------

        Dim objRemit As New EDIDB.CRemitDB
        Dim strErrMessage As String

        objRemit.Update(lngID, strPayorID, strHealthPlanID, strHealthPlanName, _
                           dteCreationDate, strBatchNumber, strProvider, strProviderNbr, _
                           strCheckNumber, detCheckDate, dblCheckAmount, dblAmountPaid, _
                           dblInterestAmount, dteRemittanceDate, dteProcessDate, strPatientNumber, _
                           strInsuredGroupNbr, strInsuredPolicyID, strInsuredLastName, strInsuredFirstName, _
                           strInsuredMiddleInitial, strPatientGroupNbr, strPatientPolicyID, strPatientLastName, _
                           strPatientFirstName, strPatientMiddleInitial, strPatientSex, _
                           dtePatientDateOfBirth, blnCompInsuranceFlag, strMedicalRecordNumber, _
                           strMessageCode1, strMessageCode2, strMessageCode3, strMessageCode4, strMessageCode5, _
                           strSupplementalInsurerName1, strSupplementalInsurerID1, strSupplementalInsurerName2, strSupplementalInsurerID2, _
                           lngInsuranceID, lngProviderID, lngPatientID, lngEncounterLogID, lngCheckID, _
                           strErnFileName, blnPosted, dtePostedDate, strErrorMessage, _
                           strAddedBy)
        objRemit = Nothing

    End Sub

End Class
