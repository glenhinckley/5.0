
'--------------------------------------------------------------------
'Class Name: CInsuranceBz
'Date: 09/11/2000
'Author: Eric Pena
'Description:  MTS business object designed to   methods associated
'              with the CInsuranceDB class.
'--------------------------------------------------------------------
' Revision History:
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
Imports Psyquel.BusinessRules.CoreLibraryIII


Public Class CBenefitsBz

    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CBenefitsBz"
    Private Const TABLE_NAME As String = ""

    Public Function FetchBenefitsByPatient(ByVal lngPatientID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/11/2000
        'Author: Eric Pena
        'Description:  Retrieves records from the tblInsCompany table utilizing a
        '            stored procedure.
        'Parameters: blnIncludeDisabled - Optional parameter that identifies if
        '               records flagged as 'Disabled' or 'De-activated' are to be
        '               included in the record set. The default value is False.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        Dim rstSQL As New ADODB.Recordset
        Dim objBene As New BenefitsDB.CBenefitsDB



        rstSQL = objBene.FetchBenefitsByPatient(lngPatientID)
        FetchBenefitsByPatient = rstSQL

        'Signal successful completion
        objBene = Nothing

    End Function


    Public Function FetchCoPayByPatientProvider(ByVal lngPatientID As Long, ByVal lngProviderID As Long) As Double
        '--------------------------------------------------------------------
        'Date: 11/07/2001
        'Author: Dave Richkun
        'Description:  Returns the CoPay amount for a patient's Primary plan
        '              based on the credentials of a Provider
        'Parameters: lngPatientID - ID of the patient
        '            lngProviderID - ID of the Provider
        'Returns: Copay amount
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objBnft As New BenefitsDB.CBenefitsDB


        FetchCoPayByPatientProvider = objBnft.FetchCoPayByPatientProvider(lngPatientID, lngProviderID)
        objBnft = Nothing

    End Function

End Class
