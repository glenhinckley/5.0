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
'---------------------------
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CBenefactorClinicBz
    '--------------------------------------------------------------------
    'Class Name: CBenefactorClinicBz
    'Date: 02/06/2000
    'Author: Eric Pena
    'Description:  MTS object designed to fetch from the tblBenefactorClinic table.
    '--------------------------------------------------------------------
    'Revision History:
    '--------------------------------------------------------------------

    Private _ConnectionString As String = String.Empty
    Private Const TABLE_NAME As String = "tblBenefactorClinic"
    Private Const CLASS_NAME As String = "CBenefactorClinicDB"

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property



    Public Function FetchClinics(ByVal lngBenefactorID As Long) As Object
        '--------------------------------------------------------------------
        'Date: 02/06/2000
        'Author: Eric Pena
        'Description:  Returns a Object array containing a list of all Clinics to which a Benefactor is associated.
        'Parameters: lngBenefactorID - The ID of the Benefactor
        'Returns: A Object array containing a list of all clinics to which a Benefactor is associated.
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objBenefactorClinic As New BenefactorDB.CBenefactorClinicDB
        objBenefactorClinic.ConnectionString = _ConnectionString

        FetchClinics = objBenefactorClinic.FetchClinics(lngBenefactorID)
        objBenefactorClinic = Nothing


    End Function
    Public Function IsEnabled(ByVal lngBenefactorID As Long, ByVal lngClinicID As Long) As Boolean
        '--------------------------------------------------------------------
        'Date: 02/06/2000
        'Author: Eric Pena
        'Description:  Lets the  er know if a given Benefactor/clinic association is active
        'Parameters: lngBenefactorID - The ID of the Benefactor
        '                  lngClinicID - The ID of the Clinic
        'Returns: T if the association is enabled/exists, F otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objBenefactorClinic As New BenefactorDB.CBenefactorClinicDB
        objBenefactorClinic.ConnectionString = _ConnectionString


        IsEnabled = objBenefactorClinic.IsEnabled(lngBenefactorID, lngClinicID)
        objBenefactorClinic = Nothing


    End Function


End Class
