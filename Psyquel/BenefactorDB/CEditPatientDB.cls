
'--------------------------------------------------------------------
'Class Name: CEditPatientDB
'Date: 06/20/2000
'Author: Dave Richkun
'Description:  MTS data object designed to encapsulate logic to insert
'              and update Patient records and the dependant information i.e.
'              Responsible Parties and Plans.  In order to prevent deadlocks
'              from occuring, all database transactions are sent through a
'              single database connection.
'--------------------------------------------------------------------
'Revisions:
'   R001 :  Joshua Lockwood, 6Mar01 : Fix made on reenable (RPPlans) that
'       avoids eroneous inserts with 0 as RPPlanID value.
'
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
' Lydia Orth 

Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CEditPatientDB


    Private _DB As New dbStuff.db
    Private _MD As New dbStuff.ModCommon

    Private _ConnectionString As String = String.Empty

    Private Const CLASS_NAME As String = "CEditPatientDB"

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property


    'Public Sub Update(ByVal _Benefactor As Benefactor)

    '    '--------------------------------------------------------------------
    '    'Date: 06/13/2000
    '    'Author: Dave Richkun
    '    'Description:  Controls the update procedures for a patient and their
    '    '              Responsible Party and Plan information.  This method
    '    '              performs multiple updates to the database, and in order
    '    '              to prevent deadlocks from occuring, all database updates
    '    '              need to be contained within the same transaction context,
    '    '              This contributes to the length of this method.
    '    'Parameters: Each parameter identifies the column value that will be
    '    '              updated.
    '    '            varRPPlans - A Object array containing one or more Responsible
    '    '               Parties (RP) to associate with the Patient.  Each RP includes
    '    '               a second Object array containing one or more Plans.
    '    '            varPCP -
    '    '            1) PhysLast
    '    '            2) PhysFirst
    '    '            3) PhysMI
    '    '            4) strPhysTitle
    '    '            5) strPhysician
    '    '            6) strPhysNPI
    '    '            7) PhysID
    '    '            8) PhysPhone
    '    '            9) PhysFax
    '    'Returns: Null
    '    '--------------------------------------------------------------------

    '    Dim cnnSQL As ADODB.Connection
    '    Dim varPlans As Object
    '    Dim intRPCtr As Integer
    '    Dim intCTR As Integer
    '    Dim arrFlags() As Object
    '    ' ReDim arrFlags(10)
    '    Dim arrPcp() As Object
    '    ReDim arrPcp(10)
    '    Dim arrRef() As Object
    '    ReDim arrRef(10)
    '    Dim arrCC(0, 0) As Object
    '    ReDim arrCC(1, 4)



    '    If Not IsArray(varFlags) Or IsEmpty(varFlags) Then
    '        arrFlags(0) = ""
    '        arrFlags(1) = ""
    '        arrFlags(2) = ""
    '        arrFlags(3) = ""
    '        arrFlags(4) = ""
    '        arrFlags(5) = ""
    '        arrFlags(6) = ""
    '        arrFlags(7) = ""
    '        arrFlags(8) = ""
    '    Else
    '        arrFlags(0) = _DB.IfNull(varFlags(0), "N")
    '        arrFlags(1) = _DB.IfNull(varFlags(1), "N")
    '        arrFlags(2) = _DB.IfNull(varFlags(2), "N")
    '        arrFlags(3) = _DB.IfNull(varFlags(3), "N")
    '        arrFlags(4) = _DB.IfNull(varFlags(4), "N")
    '        arrFlags(5) = _DB.IfNull(varFlags(5), "N")
    '        arrFlags(6) = _DB.IfNull(varFlags(6), "N")
    '        arrFlags(7) = _DB.IfNull(varFlags(7), "N")
    '        arrFlags(8) = _DB.IfNull(varFlags(8), "N")
    '    End If

    '    If Not IsArray(varPCP) Or IsEmpty(varPCP) Then
    '        arrPcp(0) = ""
    '        arrPcp(1) = ""
    '        arrPcp(2) = ""
    '        arrPcp(3) = ""
    '        arrPcp(4) = ""
    '        arrPcp(5) = ""
    '        arrPcp(6) = ""
    '        arrPcp(7) = ""
    '        arrPcp(8) = ""
    '    Else
    '        arrPcp(0) = _DB.IfNull(varPCP(0), "")
    '        arrPcp(1) = _DB.IfNull(varPCP(1), "")
    '        arrPcp(2) = _DB.IfNull(varPCP(2), "")
    '        arrPcp(3) = _DB.IfNull(varPCP(3), "")
    '        arrPcp(4) = _DB.IfNull(varPCP(4), "")
    '        arrPcp(5) = _DB.IfNull(varPCP(5), "")
    '        arrPcp(6) = _DB.IfNull(varPCP(6), "")
    '        arrPcp(7) = _DB.IfNull(varPCP(7), "")
    '        arrPcp(8) = _DB.IfNull(varPCP(8), "")
    '    End If

    '    If Not IsArray(varREF) Or IsEmpty(varREF) Then
    '        arrRef(0) = ""
    '        arrRef(1) = ""
    '        arrRef(2) = ""
    '        arrRef(3) = ""
    '        arrRef(4) = ""
    '        arrRef(5) = ""
    '        arrRef(6) = "2"
    '        arrRef(7) = 0
    '        arrRef(8) = ""
    '    Else
    '        arrRef(0) = _DB.IfNull(varREF(0), "")
    '        arrRef(1) = _DB.IfNull(varREF(1), "")
    '        arrRef(2) = _DB.IfNull(varREF(2), "")
    '        arrRef(3) = _DB.IfNull(varREF(3), "")
    '        arrRef(4) = _DB.IfNull(varREF(4), "")
    '        arrRef(5) = _DB.IfNull(varREF(5), "")
    '        arrRef(6) = _DB.IfNull(varREF(6), "2")
    '        arrRef(7) = _DB.IfNull(varREF(7), 0)
    '        arrRef(8) = _DB.IfNull(varREF(8), "")
    '    End If

    '    '       arrCC(1, 3) Credit Card Info
    '    If Not IsArray(varCC) Or IsEmpty(varCC) Then
    '        arrCC(0, 0) = ""
    '        arrCC(0, 1) = ""
    '        arrCC(0, 2) = 0
    '        arrCC(0, 3) = 0
    '        arrCC(0, 4) = ""
    '        arrCC(1, 0) = ""
    '        arrCC(1, 1) = ""
    '        arrCC(1, 2) = 0
    '        arrCC(1, 3) = 0
    '        arrCC(1, 4) = ""
    '    Else
    '        arrCC(0, 0) = _DB.IfNull(varCC(0, 0), "")
    '        arrCC(0, 1) = _DB.IfNull(varCC(0, 1), "")
    '        arrCC(0, 2) = IIf(Not IsNumeric(_DB.IfNull(varCC(0, 2), 0)), 0, _DB.IfNull(varCC(0, 2), 0))
    '        arrCC(0, 3) = IIf(Not IsNumeric(_DB.IfNull(varCC(0, 3), 0)), 0, _DB.IfNull(varCC(0, 3), 0))
    '        arrCC(0, 4) = _DB.IfNull(varCC(0, 4), "")
    '        arrCC(1, 0) = _DB.IfNull(varCC(1, 0), "")
    '        arrCC(1, 1) = _DB.IfNull(varCC(1, 1), "")
    '        arrCC(1, 2) = IIf(Not IsNumeric(_DB.IfNull(varCC(1, 2), 0)), 0, _DB.IfNull(varCC(1, 2), 0))
    '        arrCC(1, 3) = IIf(Not IsNumeric(_DB.IfNull(varCC(1, 3), 0)), 0, _DB.IfNull(varCC(1, 3), 0))
    '        arrCC(1, 4) = _DB.IfNull(varCC(1, 4), "")
    '    End If

    '    ' Acquire the database connection.
    '    cnnSQL = New ADODB.Connection
    '    cnnSQL.Open(_ConnectionString)

    '    'Update the Patient record
    '    UpdateBenefactor(_Benefactor)

    '    'Insert/Update RP record(s)
    '    'The business logic is based on the following conditions:
    '    'If the value of the BenefactorID (array element[0, x]) is:
    '    'Less than zero: The patient is considered their own responsible party.
    '    '       We have already updated the Benefactor.    Array elements 1 to 30 are ignored.
    '    'Equal to zero: A new benefactor record (responsible party) will be created.
    '    'Greater than zero: An existing responsible party record whose ID is represented by the
    '    '       data value will be updated.
    '    For intRPCtr = 0 To UBound(varRPPlans, 2)
    '        If varRPPlans(0, intRPCtr) < 0 Then
    '            'Patient is their own RP; no action required
    '        ElseIf varRPPlans(0, intRPCtr) = 0 Then
    '            'Insert a new Benefactor record; Populate the first element of the array with the ID of
    '            'the new Benefactor
    '            varRPPlans(0, intRPCtr) = InsertBenefactor(varRPPlans(1, intRPCtr), _DB.IfNull(varRPPlans(2, intRPCtr), ""), _
    '                    _DB.IfNull(varRPPlans(3, intRPCtr), ""), varRPPlans(4, intRPCtr), _DB.IfNull(varRPPlans(5, intRPCtr), ""), _
    '                    _DB.IfNull(varRPPlans(6, intRPCtr), ""), _DB.IfNull(varRPPlans(7, intRPCtr), ""), _DB.IfNull(varRPPlans(8, intRPCtr), ""), _
    '                    _DB.IfNull(varRPPlans(9, intRPCtr), ""), strCountry, _DB.IfNull(varRPPlans(10, intRPCtr), ""), _DB.IfNull(varRPPlans(11, intRPCtr), ""), _DB.IfNull(varRPPlans(12, intRPCtr), ""), _
    '                    _DB.IfNull(varRPPlans(13, intRPCtr), ""), _DB.IfNull(varRPPlans(15, intRPCtr), ""), _DB.IfNull(varRPPlans(16, intRPCtr), ""), _
    '                    _DB.IfNull(varRPPlans(18, intRPCtr), ""), _DB.IfNull(varRPPlans(19, intRPCtr), ""), _DB.IfNull(varRPPlans(20, intRPCtr), ""), _
    '                    _DB.IfNull(varRPPlans(22, intRPCtr), ""), _DB.IfNull(varRPPlans(25, intRPCtr), 0), _
    '                    _DB.IfNull(varRPPlans(26, intRPCtr), 0), _DB.IfNull(varRPPlans(27, intRPCtr), ""), _DB.IfNull(varRPPlans(28, intRPCtr), ""), "", _
    '                    _DB.IfNull(varRPPlans(29, intRPCtr), ""), _DB.IfNull(varRPPlans(30, intRPCtr), ""), _DB.IfNull(varRPPlans(31, intRPCtr), ""), _
    '                    _DB.IfNull(varRPPlans(32, intRPCtr), ""), _DB.IfNull(varRPPlans(33, intRPCtr), ""), _DB.IfNull(varRPPlans(34, intRPCtr), ""), _DB.IfNull(varRPPlans(35, intRPCtr), ""), _
    '                    _DB.IfNull(varRPPlans(36, intRPCtr), ""), _DB.IfNull(varRPPlans(37, intRPCtr), ""), _DB.IfNull(varRPPlans(38, intRPCtr), ""), _
    '                    _DB.IfNull(varRPPlans(39, intRPCtr), ""), _DB.IfNull(varRPPlans(40, intRPCtr), ""), _DB.IfNull(varRPPlans(41, intRPCtr), ""), _
    '                    _DB.IfNull(varRPPlans(42, intRPCtr), ""), _DB.IfNull(varRPPlans(43, intRPCtr), ""), _DB.IfNull(varRPPlans(44, intRPCtr), ""), _
    '                    _DB.IfNull(varRPPlans(45, intRPCtr), ""), _DB.IfNull(varRPPlans(46, intRPCtr), ""), _DB.IfNull(varRPPlans(47, intRPCtr), ""), _
    '                    _DB.IfNull(varRPPlans(48, intRPCtr), ""), _DB.IfNull(varRPPlans(49, intRPCtr), ""), 0, 0, "", "", varFlags, strAddedBy, cnnSQL)

    '        ElseIf varRPPlans(0, intRPCtr) > 0 Then
    '            'Update an existing Benefactor record
    '            UpdateBenefactor(varRPPlans(0, intRPCtr), varRPPlans(1, intRPCtr), _DB.IfNull(varRPPlans(2, intRPCtr), ""), _
    '                   _DB.IfNull(varRPPlans(3, intRPCtr), ""), varRPPlans(4, intRPCtr), _DB.IfNull(varRPPlans(5, intRPCtr), ""), _
    '                   _DB.IfNull(varRPPlans(6, intRPCtr), ""), _DB.IfNull(varRPPlans(7, intRPCtr), ""), _DB.IfNull(varRPPlans(8, intRPCtr), ""), _
    '                   _DB.IfNull(varRPPlans(9, intRPCtr), ""), strCountry, _DB.IfNull(varRPPlans(10, intRPCtr), ""), _DB.IfNull(varRPPlans(11, intRPCtr), ""), _DB.IfNull(varRPPlans(12, intRPCtr), ""), _
    '                   _DB.IfNull(varRPPlans(13, intRPCtr), ""), _DB.IfNull(varRPPlans(15, intRPCtr), ""), _DB.IfNull(varRPPlans(16, intRPCtr), ""), _
    '                   _DB.IfNull(varRPPlans(18, intRPCtr), ""), _DB.IfNull(varRPPlans(19, intRPCtr), ""), _DB.IfNull(varRPPlans(20, intRPCtr), ""), _
    '                   _DB.IfNull(varRPPlans(22, intRPCtr), ""), _
    '                   _DB.IfNull(varRPPlans(25, intRPCtr), 0), _
    '                   _DB.IfNull(varRPPlans(26, intRPCtr), 0), _DB.IfNull(varRPPlans(27, intRPCtr), ""), _DB.IfNull(varRPPlans(28, intRPCtr), ""), "", _
    '                   _DB.IfNull(varRPPlans(29, intRPCtr), ""), _DB.IfNull(varRPPlans(30, intRPCtr), ""), _DB.IfNull(varRPPlans(31, intRPCtr), ""), _
    '                   _DB.IfNull(varRPPlans(32, intRPCtr), ""), _DB.IfNull(varRPPlans(33, intRPCtr), ""), _DB.IfNull(varRPPlans(34, intRPCtr), ""), _DB.IfNull(varRPPlans(35, intRPCtr), ""), _
    '                   _DB.IfNull(varRPPlans(36, intRPCtr), ""), _DB.IfNull(varRPPlans(37, intRPCtr), ""), _DB.IfNull(varRPPlans(38, intRPCtr), ""), _
    '                   _DB.IfNull(varRPPlans(39, intRPCtr), ""), _DB.IfNull(varRPPlans(40, intRPCtr), ""), _DB.IfNull(varRPPlans(41, intRPCtr), ""), _
    '                   _DB.IfNull(varRPPlans(42, intRPCtr), ""), _DB.IfNull(varRPPlans(43, intRPCtr), ""), _DB.IfNull(varRPPlans(44, intRPCtr), ""), _
    '                   _DB.IfNull(varRPPlans(45, intRPCtr), ""), _DB.IfNull(varRPPlans(46, intRPCtr), ""), _DB.IfNull(varRPPlans(47, intRPCtr), ""), _
    '                   _DB.IfNull(varRPPlans(48, intRPCtr), ""), _DB.IfNull(varRPPlans(49, intRPCtr), ""), 0, 0, "", "", varFlags, strAddedBy, cnnSQL)
    '        End If
    '    Next intRPCtr

    '    'Get the Plans from the RPPlans array and update them in the database.
    '    varPlans = CreatePlansArray(varRPPlans)

    '    UpdatePlans(_Benefactor.lngPatientID, varPlans, _Benefactor.strAddedBy, cnnSQL)
    '    DisableNegativePlans(_Benefactor.lngPatientID, _Benefactor.strAddedBy, cnnSQL)

    '    'Close and free the connection
    '    cnnSQL.Close()
    '    cnnSQL = Nothing



    '    Exit Sub


    '    'Signal incompletion and raise the error to the ing environment.
    '    cnnSQL = Nothing
    '    'cmdSQL.Execute()
    '    'err.raise(Err.Number, Err.Source, Err.Description)

    'End Sub









    'Public Sub Update(ByVal lngPatientID As Long, ByVal strLast As String, ByVal strMaiden As String, ByVal strMI As String, _
    '                ByVal strFirst As String, ByVal strNickName As String, ByVal strAddress1 As String, ByVal strAddress2 As String, _
    '                ByVal strCity As String, ByVal strState As String, ByVal strCountry As String, ByVal strZip As String, ByVal strCounty As String, _
    '                ByVal strHomePhoneDesc As String, ByVal strHomePhone As String, _
    '                ByVal strMobilePhoneDesc As String, ByVal strMobilePhone As String, _
    '                ByVal strWorkPhoneDesc As String, ByVal strWorkPhone As String, ByVal strWorkExt As String, _
    '                ByVal strEmail As String, _
    '                ByVal dteDOB As Date, ByVal strSex As String, ByVal strSSN As String, _
    '                ByVal strDLNum As String, ByVal varCC As Object, ByVal lngMarital As Long, ByVal lngEthnicity As Long, ByVal lngHHIncome As Long, _
    '                ByVal lngEmployment As Long, ByVal strEmployer As String, ByVal varPCP As Object, ByVal varREF As Object, _
    '                ByVal strEmgName As String, ByVal strEmgRelat As String, ByVal strEmgPhone As String, _
    '                ByVal strGuardianName As String, ByVal strGuardianRelat As String, ByVal strGuardianPhone As String, _
    '                ByVal strOtherPhone1 As String, ByVal strOtherPhoneDesc1 As String, ByVal strOtherPhone2 As String, ByVal strOtherPhoneDesc2 As String, ByVal strOtherPhone3 As String, ByVal strOtherPhoneDesc3 As String, _
    '                ByVal strNotes As String, ByVal strInternalID As String, ByVal lngClinicID As Long, _
    '                ByVal dteAdmitDate As Date, ByVal strUserName As String, ByVal strPassword As String, ByVal varFlags As Object, _
    '                ByVal strAddedBy As String, ByVal varRPPlans As Object)
    '    '--------------------------------------------------------------------
    '    'Date: 06/13/2000
    '    'Author: Dave Richkun
    '    'Description:  Controls the update procedures for a patient and their
    '    '              Responsible Party and Plan information.  This method
    '    '              performs multiple updates to the database, and in order
    '    '              to prevent deadlocks from occuring, all database updates
    '    '              need to be contained within the same transaction context,
    '    '              This contributes to the length of this method.
    '    'Parameters: Each parameter identifies the column value that will be
    '    '              updated.
    '    '            varRPPlans - A Object array containing one or more Responsible
    '    '               Parties (RP) to associate with the Patient.  Each RP includes
    '    '               a second Object array containing one or more Plans.
    '    '            varPCP -
    '    '            1) PhysLast
    '    '            2) PhysFirst
    '    '            3) PhysMI
    '    '            4) strPhysTitle
    '    '            5) strPhysician
    '    '            6) strPhysNPI
    '    '            7) PhysID
    '    '            8) PhysPhone
    '    '            9) PhysFax
    '    'Returns: Null
    '    '--------------------------------------------------------------------

    '    Dim cnnSQL As ADODB.Connection
    '    Dim varPlans As Object
    '    Dim intRPCtr As Integer
    '    Dim intCTR As Integer
    '    Dim arrFlags() As Object
    '    ReDim arrFlags(10)
    '    Dim arrPcp() As Object
    '    ReDim arrPcp(10)
    '    Dim arrRef() As Object
    '    ReDim arrRef(10)
    '    Dim arrCC() As Object
    '    ReDim arrCC(1, 4)



    '    If Not IsArray(varFlags) Or IsEmpty(varFlags) Then
    '        arrFlags(0) = ""
    '        arrFlags(1) = ""
    '        arrFlags(2) = ""
    '        arrFlags(3) = ""
    '        arrFlags(4) = ""
    '        arrFlags(5) = ""
    '        arrFlags(6) = ""
    '        arrFlags(7) = ""
    '        arrFlags(8) = ""
    '    Else
    '        arrFlags(0) = _DB.IfNull(varFlags(0), "N")
    '        arrFlags(1) = _DB.IfNull(varFlags(1), "N")
    '        arrFlags(2) = _DB.IfNull(varFlags(2), "N")
    '        arrFlags(3) = _DB.IfNull(varFlags(3), "N")
    '        arrFlags(4) = _DB.IfNull(varFlags(4), "N")
    '        arrFlags(5) = _DB.IfNull(varFlags(5), "N")
    '        arrFlags(6) = _DB.IfNull(varFlags(6), "N")
    '        arrFlags(7) = _DB.IfNull(varFlags(7), "N")
    '        arrFlags(8) = _DB.IfNull(varFlags(8), "N")
    '    End If

    '    If Not IsArray(varPCP) Or IsEmpty(varPCP) Then
    '        arrPcp(0) = ""
    '        arrPcp(1) = ""
    '        arrPcp(2) = ""
    '        arrPcp(3) = ""
    '        arrPcp(4) = ""
    '        arrPcp(5) = ""
    '        arrPcp(6) = ""
    '        arrPcp(7) = ""
    '        arrPcp(8) = ""
    '    Else
    '        arrPcp(0) = _DB.IfNull(varPCP(0), "")
    '        arrPcp(1) = _DB.IfNull(varPCP(1), "")
    '        arrPcp(2) = _DB.IfNull(varPCP(2), "")
    '        arrPcp(3) = _DB.IfNull(varPCP(3), "")
    '        arrPcp(4) = _DB.IfNull(varPCP(4), "")
    '        arrPcp(5) = _DB.IfNull(varPCP(5), "")
    '        arrPcp(6) = _DB.IfNull(varPCP(6), "")
    '        arrPcp(7) = _DB.IfNull(varPCP(7), "")
    '        arrPcp(8) = _DB.IfNull(varPCP(8), "")
    '    End If

    '    If Not IsArray(varREF) Or IsEmpty(varREF) Then
    '        arrRef(0) = ""
    '        arrRef(1) = ""
    '        arrRef(2) = ""
    '        arrRef(3) = ""
    '        arrRef(4) = ""
    '        arrRef(5) = ""
    '        arrRef(6) = "2"
    '        arrRef(7) = 0
    '        arrRef(8) = ""
    '    Else
    '        arrRef(0) = _DB.IfNull(varREF(0), "")
    '        arrRef(1) = _DB.IfNull(varREF(1), "")
    '        arrRef(2) = _DB.IfNull(varREF(2), "")
    '        arrRef(3) = _DB.IfNull(varREF(3), "")
    '        arrRef(4) = _DB.IfNull(varREF(4), "")
    '        arrRef(5) = _DB.IfNull(varREF(5), "")
    '        arrRef(6) = _DB.IfNull(varREF(6), "2")
    '        arrRef(7) = _DB.IfNull(varREF(7), 0)
    '        arrRef(8) = _DB.IfNull(varREF(8), "")
    '    End If

    '    '       arrCC(1, 3) Credit Card Info
    '    If Not IsArray(varCC) Or IsEmpty(varCC) Then
    '        arrCC(0, 0) = ""
    '        arrCC(0, 1) = ""
    '        arrCC(0, 2) = 0
    '        arrCC(0, 3) = 0
    '        arrCC(0, 4) = ""
    '        arrCC(1, 0) = ""
    '        arrCC(1, 1) = ""
    '        arrCC(1, 2) = 0
    '        arrCC(1, 3) = 0
    '        arrCC(1, 4) = ""
    '    Else
    '        arrCC(0, 0) = _DB.IfNull(varCC(0, 0), "")
    '        arrCC(0, 1) = _DB.IfNull(varCC(0, 1), "")
    '        arrCC(0, 2) = IIf(Not IsNumeric(_DB.IfNull(varCC(0, 2), 0)), 0, _DB.IfNull(varCC(0, 2), 0))
    '        arrCC(0, 3) = IIf(Not IsNumeric(_DB.IfNull(varCC(0, 3), 0)), 0, _DB.IfNull(varCC(0, 3), 0))
    '        arrCC(0, 4) = _DB.IfNull(varCC(0, 4), "")
    '        arrCC(1, 0) = _DB.IfNull(varCC(1, 0), "")
    '        arrCC(1, 1) = _DB.IfNull(varCC(1, 1), "")
    '        arrCC(1, 2) = IIf(Not IsNumeric(_DB.IfNull(varCC(1, 2), 0)), 0, _DB.IfNull(varCC(1, 2), 0))
    '        arrCC(1, 3) = IIf(Not IsNumeric(_DB.IfNull(varCC(1, 3), 0)), 0, _DB.IfNull(varCC(1, 3), 0))
    '        arrCC(1, 4) = _DB.IfNull(varCC(1, 4), "")
    '    End If

    '    'Acquire the database connection.
    '    cnnSQL = New ADODB.Connection
    '    cnnSQL.Open(_ConnectionString)

    '    'Update the Patient record
    '    UpdateBenefactor(lngPatientID, strLast, strMaiden, strMI, strFirst, strNickName, _
    '               strAddress1, strAddress2, strCity, strState, strCountry, strZip, strCounty, strHomePhoneDesc, strHomePhone, _
    '               strMobilePhoneDesc, strMobilePhone, strWorkPhoneDesc, strWorkPhone, strWorkExt, _
    '               strEmail, dteDOB, strSex, strSSN, _
    '               strDLNum, varCC, lngMarital, lngEthnicity, lngHHIncome, lngEmployment, strEmployer, varPCP, varREF, _
    '               strEmgName, strEmgRelat, strEmgPhone, _
    '               strGuardianName, strGuardianRelat, strGuardianPhone, _
    '               strOtherPhone1, strOtherPhoneDesc1, strOtherPhone2, strOtherPhoneDesc2, strOtherPhone3, strOtherPhoneDesc3, _
    '               strNotes, strInternalID, lngClinicID, dteAdmitDate, strUserName, strPassword, varFlags, strAddedBy, cnnSQL)

    '    'Insert/Update RP record(s)
    '    'The business logic is based on the following conditions:
    '    'If the value of the BenefactorID (array element[0, x]) is:
    '    'Less than zero: The patient is considered their own responsible party.
    '    '       We have already updated the Benefactor.    Array elements 1 to 30 are ignored.
    '    'Equal to zero: A new benefactor record (responsible party) will be created.
    '    'Greater than zero: An existing responsible party record whose ID is represented by the
    '    '       data value will be updated.
    '    For intRPCtr = 0 To UBound(varRPPlans, 2)
    '        If varRPPlans(0, intRPCtr) < 0 Then
    '            'Patient is their own RP; no action required
    '        ElseIf varRPPlans(0, intRPCtr) = 0 Then
    '            'Insert a new Benefactor record; Populate the first element of the array with the ID of
    '            'the new Benefactor
    '            varRPPlans(0, intRPCtr) = InsertBenefactor(varRPPlans(1, intRPCtr), _DB.IfNull(varRPPlans(2, intRPCtr), ""), _
    '                    _DB.IfNull(varRPPlans(3, intRPCtr), ""), varRPPlans(4, intRPCtr), _DB.IfNull(varRPPlans(5, intRPCtr), ""), _
    '                    _DB.IfNull(varRPPlans(6, intRPCtr), ""), _DB.IfNull(varRPPlans(7, intRPCtr), ""), _DB.IfNull(varRPPlans(8, intRPCtr), ""), _
    '                    _DB.IfNull(varRPPlans(9, intRPCtr), ""), strCountry, _DB.IfNull(varRPPlans(10, intRPCtr), ""), _DB.IfNull(varRPPlans(11, intRPCtr), ""), _DB.IfNull(varRPPlans(12, intRPCtr), ""), _
    '                    _DB.IfNull(varRPPlans(13, intRPCtr), ""), _DB.IfNull(varRPPlans(15, intRPCtr), ""), _DB.IfNull(varRPPlans(16, intRPCtr), ""), _
    '                    _DB.IfNull(varRPPlans(18, intRPCtr), ""), _DB.IfNull(varRPPlans(19, intRPCtr), ""), _DB.IfNull(varRPPlans(20, intRPCtr), ""), _
    '                    _DB.IfNull(varRPPlans(22, intRPCtr), ""), _DB.IfNull(varRPPlans(25, intRPCtr), 0), _
    '                    _DB.IfNull(varRPPlans(26, intRPCtr), 0), _DB.IfNull(varRPPlans(27, intRPCtr), ""), _DB.IfNull(varRPPlans(28, intRPCtr), ""), "", _
    '                    _DB.IfNull(varRPPlans(29, intRPCtr), ""), _DB.IfNull(varRPPlans(30, intRPCtr), ""), _DB.IfNull(varRPPlans(31, intRPCtr), ""), _
    '                    _DB.IfNull(varRPPlans(32, intRPCtr), ""), _DB.IfNull(varRPPlans(33, intRPCtr), ""), _DB.IfNull(varRPPlans(34, intRPCtr), ""), _DB.IfNull(varRPPlans(35, intRPCtr), ""), _
    '                    _DB.IfNull(varRPPlans(36, intRPCtr), ""), _DB.IfNull(varRPPlans(37, intRPCtr), ""), _DB.IfNull(varRPPlans(38, intRPCtr), ""), _
    '                    _DB.IfNull(varRPPlans(39, intRPCtr), ""), _DB.IfNull(varRPPlans(40, intRPCtr), ""), _DB.IfNull(varRPPlans(41, intRPCtr), ""), _
    '                    _DB.IfNull(varRPPlans(42, intRPCtr), ""), _DB.IfNull(varRPPlans(43, intRPCtr), ""), _DB.IfNull(varRPPlans(44, intRPCtr), ""), _
    '                    _DB.IfNull(varRPPlans(45, intRPCtr), ""), _DB.IfNull(varRPPlans(46, intRPCtr), ""), _DB.IfNull(varRPPlans(47, intRPCtr), ""), _
    '                    _DB.IfNull(varRPPlans(48, intRPCtr), ""), _DB.IfNull(varRPPlans(49, intRPCtr), ""), 0, 0, "", "", varFlags, strAddedBy, cnnSQL)

    '        ElseIf varRPPlans(0, intRPCtr) > 0 Then
    '            'Update an existing Benefactor record
    '            UpdateBenefactor(varRPPlans(0, intRPCtr), varRPPlans(1, intRPCtr), _DB.IfNull(varRPPlans(2, intRPCtr), ""), _
    '                   _DB.IfNull(varRPPlans(3, intRPCtr), ""), varRPPlans(4, intRPCtr), _DB.IfNull(varRPPlans(5, intRPCtr), ""), _
    '                   _DB.IfNull(varRPPlans(6, intRPCtr), ""), _DB.IfNull(varRPPlans(7, intRPCtr), ""), _DB.IfNull(varRPPlans(8, intRPCtr), ""), _
    '                   _DB.IfNull(varRPPlans(9, intRPCtr), ""), strCountry, _DB.IfNull(varRPPlans(10, intRPCtr), ""), _DB.IfNull(varRPPlans(11, intRPCtr), ""), _DB.IfNull(varRPPlans(12, intRPCtr), ""), _
    '                   _DB.IfNull(varRPPlans(13, intRPCtr), ""), _DB.IfNull(varRPPlans(15, intRPCtr), ""), _DB.IfNull(varRPPlans(16, intRPCtr), ""), _
    '                   _DB.IfNull(varRPPlans(18, intRPCtr), ""), _DB.IfNull(varRPPlans(19, intRPCtr), ""), _DB.IfNull(varRPPlans(20, intRPCtr), ""), _
    '                   _DB.IfNull(varRPPlans(22, intRPCtr), ""), _
    '                   _DB.IfNull(varRPPlans(25, intRPCtr), 0), _
    '                   _DB.IfNull(varRPPlans(26, intRPCtr), 0), _DB.IfNull(varRPPlans(27, intRPCtr), ""), _DB.IfNull(varRPPlans(28, intRPCtr), ""), "", _
    '                   _DB.IfNull(varRPPlans(29, intRPCtr), ""), _DB.IfNull(varRPPlans(30, intRPCtr), ""), _DB.IfNull(varRPPlans(31, intRPCtr), ""), _
    '                   _DB.IfNull(varRPPlans(32, intRPCtr), ""), _DB.IfNull(varRPPlans(33, intRPCtr), ""), _DB.IfNull(varRPPlans(34, intRPCtr), ""), _DB.IfNull(varRPPlans(35, intRPCtr), ""), _
    '                   _DB.IfNull(varRPPlans(36, intRPCtr), ""), _DB.IfNull(varRPPlans(37, intRPCtr), ""), _DB.IfNull(varRPPlans(38, intRPCtr), ""), _
    '                   _DB.IfNull(varRPPlans(39, intRPCtr), ""), _DB.IfNull(varRPPlans(40, intRPCtr), ""), _DB.IfNull(varRPPlans(41, intRPCtr), ""), _
    '                   _DB.IfNull(varRPPlans(42, intRPCtr), ""), _DB.IfNull(varRPPlans(43, intRPCtr), ""), _DB.IfNull(varRPPlans(44, intRPCtr), ""), _
    '                   _DB.IfNull(varRPPlans(45, intRPCtr), ""), _DB.IfNull(varRPPlans(46, intRPCtr), ""), _DB.IfNull(varRPPlans(47, intRPCtr), ""), _
    '                   _DB.IfNull(varRPPlans(48, intRPCtr), ""), _DB.IfNull(varRPPlans(49, intRPCtr), ""), 0, 0, "", "", varFlags, strAddedBy, cnnSQL)
    '        End If
    '    Next intRPCtr

    '    'Get the Plans from the RPPlans array and update them in the database.
    '    varPlans = CreatePlansArray(varRPPlans)

    '    UpdatePlans(lngPatientID, varPlans, strAddedBy, cnnSQL)
    '    DisableNegativePlans(lngPatientID, strAddedBy, cnnSQL)

    '    'Close and free the connection
    '    cnnSQL.Close()
    '    cnnSQL = Nothing



    '    Exit Sub


    '    'Signal incompletion and raise the error to the ing environment.
    '    cnnSQL = Nothing
    '    'cmdSQL.Execute()
    '    'err.raise(Err.Number, Err.Source, Err.Description)

    'End Sub

    'Public Sub AddRPPlan(ByVal lngPatientID As Long, ByVal strLast As String, ByVal strMaiden As String, ByVal strMI As String, _
    '                     ByVal strFirst As String, ByVal strNickName As String, ByVal strAddress1 As String, ByVal strAddress2 As String, _
    '                     ByVal strCity As String, ByVal strState As String, ByVal strCountry As String, ByVal strZip As String, ByVal strCounty As String, _
    '                     ByVal strHomePhoneDesc As String, ByVal strHomePhone As String, _
    '                     ByVal strMobilePhoneDesc As String, ByVal strMobilePhone As String, _
    '                     ByVal strWorkPhoneDesc As String, ByVal strWorkPhone As String, ByVal strWorkExt As String, _
    '                     ByVal dteDOB As Date, ByVal strEmail As String, _
    '                     ByVal strSex As String, ByVal strSSN As String, ByVal strDLNum As String, _
    '                     ByVal lngMarital As Long, ByVal lngEthnicity As Long, ByVal lngHHIncome As Long, _
    '                     ByVal lngEmployment As Long, ByVal strEmployer As String, _
    '                     ByVal varFlags As Object, ByVal arrPlans As Object, ByVal strAddedBy As String, Optional ByVal strUserName As String = "", Optional ByVal strPassword As String = "")
    '    '--------------------------------------------------------------------
    '    'Date: 07/28/2000
    '    'Author: Dave Richkun
    '    'Description:  Adds a single responsible party and plan record and associates
    '    '              the Responsible Party to a patient.
    '    'Parameters: Each parameter identifies the column value that will be
    '    '              updated.
    '    'Returns: Null
    '    '--------------------------------------------------------------------

    '    '   Dim objPRPP As BenefactorDB.CPatRPPlanDB
    '    Dim objPlanBenefit As BenefitsBz.CPatientBenefitBZ
    '    Dim cnnSQL As ADODB.Connection
    '    Dim lngRPID As Long
    '    Dim lngPatRPPlanID As Long
    '    Dim varPCP As Object
    '    Dim varREF As Object
    '    Dim varCC As Object



    '    'Acquire the database connection.
    '    cnnSQL = New ADODB.Connection
    '    cnnSQL.Open(_ConnectionString)

    '    'Insert the Responsible Party (RP) record
    '    lngRPID = InsertBenefactor(strLast, strMaiden, strMI, strFirst, strNickName, _
    '                  strAddress1, strAddress2, strCity, strState, strCountry, strZip, strCounty, strHomePhoneDesc, strHomePhone, _
    '                  strMobilePhoneDesc, strMobilePhone, strWorkPhoneDesc, strWorkPhone, strWorkExt, strEmail, dteDOB, _
    '                  strSex, strSSN, strDLNum, varCC, lngMarital, lngEthnicity, lngHHIncome, lngEmployment, strEmployer, _
    '                  varPCP, varREF, "", "", "", "", "", "", "", "", "", "", "", "", "", "", 0, 0, strUserName, strPassword, varFlags, strAddedBy, cnnSQL)

    '    'Insert row into tblPatRPPlan
    '    lngPatRPPlanID = InsertPlan(lngPatientID, lngRPID, arrPlans(0, 0), strAddedBy, cnnSQL)

    '    'Insert Plan Benefits information
    '    objPlanBenefit = CreateObjectXXXXXXX("BenefitsBz.CPatientBenefitBZ")
    '    objPlanBenefit.Insert(lngPatRPPlanID, arrPlans(2, 0), arrPlans(3, 0), arrPlans(4, 0), _
    '           arrPlans(5, 0), arrPlans(6, 0), arrPlans(7, 0), arrPlans(8, 0), arrPlans(9, 0), _
    '           arrPlans(10, 0), arrPlans(11, 0), arrPlans(12, 0), arrPlans(13, 0), arrPlans(14, 0), _
    '           arrPlans(15, 0), arrPlans(16, 0), arrPlans(17, 0), arrPlans(18, 0), arrPlans(19, 0), _
    '           arrPlans(20, 0), arrPlans(21, 0), arrPlans(22, 0), arrPlans(23, 0), arrPlans(24, 0), _
    '           arrPlans(25, 0), arrPlans(26, 0), arrPlans(27, 0), strAddedBy)
    '    objPlanBenefit = Nothing

    '    'Close and free the connection
    '    cnnSQL.Close()
    '    cnnSQL = Nothing



    '    Exit Sub


    '    'Signal incompletion and raise the error to the ing environment.
    '    'cmdSQL.Execute()
    '    cnnSQL = Nothing
    '    'err.raise(Err.Number, Err.Source, Err.Description)

    'End Sub


    'Public Sub AddExistingRPPlan(ByVal lngPatientID As Long, ByVal lngRPID As Long, _
    '                             ByVal arrPlans As Object, ByVal strAddedBy As String)
    '    '--------------------------------------------------------------------
    '    'Date: 11/19/2000
    '    'Author: Dave Richkun
    '    'Description:  Asoociates an existing responsible party and plan record to a patient.
    '    'Parameters: Each parameter identifies the column value that will be
    '    '              updated.
    '    'Returns: Null
    '    '--------------------------------------------------------------------

    '    Dim objPlanBenefit As BenefitsBz.CPatientBenefitBZ
    '    Dim cnnSQL As ADODB.Connection
    '    Dim lngPatRPPlanID As Long



    '    'Acquire the database connection.
    '    cnnSQL = New ADODB.Connection
    '    cnnSQL.Open(_ConnectionString)

    '    'Insert row into tblPatRPPlan
    '    lngPatRPPlanID = InsertPlan(lngPatientID, lngRPID, arrPlans(0, 0), strAddedBy, cnnSQL)

    '    'Update Plan Benefits information
    '    If lngPatRPPlanID > 0 Then  'May be a valid ID, attempt the insert  --R001
    '        objPlanBenefit = CreateObjectXXXXXXX("BenefitsBz.CPatientBenefitBZ")
    '        objPlanBenefit.Insert(lngPatRPPlanID, arrPlans(2, 0), arrPlans(3, 0), arrPlans(4, 0), _
    '           arrPlans(5, 0), arrPlans(6, 0), arrPlans(7, 0), arrPlans(8, 0), arrPlans(9, 0), _
    '           arrPlans(10, 0), arrPlans(11, 0), arrPlans(12, 0), arrPlans(13, 0), arrPlans(14, 0), _
    '           arrPlans(15, 0), arrPlans(16, 0), arrPlans(17, 0), arrPlans(18, 0), arrPlans(19, 0), _
    '           arrPlans(20, 0), arrPlans(21, 0), arrPlans(22, 0), arrPlans(23, 0), arrPlans(24, 0), _
    '           arrPlans(25, 0), arrPlans(26, 0), arrPlans(27, 0), strAddedBy)
    '        objPlanBenefit = Nothing
    '    End If

    '    'Close and free the connection
    '    cnnSQL.Close()
    '    cnnSQL = Nothing



    '    Exit Sub


    '    'Signal incompletion and raise the error to the ing environment.
    '    'cmdSQL.Execute()
    '    objPlanBenefit = Nothing
    '    cnnSQL = Nothing
    '    'err.raise(Err.Number, Err.Source, Err.Description)

    'End Sub

    '----------------------
    ' Private Routines
    '----------------------

    Private Sub UpdateBenefactor(ByVal lngPatientID As Long, ByVal strLast As String, ByVal strMaiden As String, ByVal strMI As String, _
                ByVal strFirst As String, ByVal strNickName As String, ByVal strAddress1 As String, ByVal strAddress2 As String, _
                ByVal strCity As String, ByVal strState As String, ByVal strCountry As String, ByVal strZip As String, ByVal strCounty As String, _
                ByVal strHomePhoneDesc As String, ByVal strHomePhone As String, _
                ByVal strMobilePhoneDesc As String, ByVal strMobilePhone As String, _
                ByVal strWorkPhoneDesc As String, ByVal strWorkPhone As String, ByVal strWorkExt As String, _
                ByVal strEmail As String, _
                ByVal dteDOB As Date, ByVal strSex As String, ByVal strSSN As String, ByVal strDLNum As String, ByVal varCC As Object, _
                ByVal lngMarital As Long, ByVal lngEthnicity As Long, ByVal lngHHIncome As Long, _
                ByVal lngEmployment As Long, ByVal strEmployer As String, ByVal varPCP As Object, ByVal varREF As Object, _
                ByVal strEmgName As String, ByVal strEmgRelat As String, ByVal strEmgPhone As String, _
                ByVal strGuardianName As String, ByVal strGuardianRelat As String, ByVal strGuardianPhone As String, _
                ByVal strOtherPhone1 As String, ByVal strOtherPhoneDesc1 As String, ByVal strOtherPhone2 As String, ByVal strOtherPhoneDesc2 As String, ByVal strOtherPhone3 As String, ByVal strOtherPhoneDesc3 As String, _
                ByVal strNotes As String, ByVal strInternalID As String, ByVal lngClinicID As Long, _
                ByVal dteAdmitDate As Date, ByVal strUserName As String, ByVal strPassword As String, ByVal varFlags As Object, _
                ByVal strUpdatedBy As String, ByVal cnnSQL As ADODB.Connection)


        Dim strApptReminder As String
        Dim strNoteTracking As String

        Dim cmdSQL As ADODB.Command

        Dim arrFlags() As Object
        ReDim arrFlags(10)
        Dim arrPcp() As Object
        ReDim arrPcp(10)
        Dim arrRef() As Object
        ReDim arrRef(10)
        Dim arrCC(0, 0) As Object
        ReDim arrCC(1, 4)



        If Not IsArray(varFlags) Or (varFlags.Value.varValue.IsNullorEmpty) Then
            arrFlags(0) = ""
            arrFlags(1) = ""
            arrFlags(2) = ""
            arrFlags(3) = ""
            arrFlags(4) = ""
            arrFlags(5) = ""
            arrFlags(6) = ""
            arrFlags(7) = ""
            arrFlags(8) = ""
        Else
            arrFlags(0) = _DB.IfNull(varFlags(0), "N")
            arrFlags(1) = _DB.IfNull(varFlags(1), "N")
            arrFlags(2) = _DB.IfNull(varFlags(2), "N")
            arrFlags(3) = _DB.IfNull(varFlags(3), "N")
            arrFlags(4) = _DB.IfNull(varFlags(4), "N")
            arrFlags(5) = _DB.IfNull(varFlags(5), "N")
            arrFlags(6) = _DB.IfNull(varFlags(6), "N")
            arrFlags(7) = _DB.IfNull(varFlags(7), "N")
            arrFlags(8) = _DB.IfNull(varFlags(8), "N")
        End If

        If Not IsArray(varPCP) Or (varPCP.Value.varValue.IsNullorEmpty) Then
            arrPcp(0) = ""
            arrPcp(1) = ""
            arrPcp(2) = ""
            arrPcp(3) = ""
            arrPcp(4) = ""
            arrPcp(5) = ""
            arrPcp(6) = ""
            arrPcp(7) = ""
            arrPcp(8) = ""
        Else
            arrPcp(0) = _DB.IfNull(varPCP(0), "")
            arrPcp(1) = _DB.IfNull(varPCP(1), "")
            arrPcp(2) = _DB.IfNull(varPCP(2), "")
            arrPcp(3) = _DB.IfNull(varPCP(3), "")
            arrPcp(4) = _DB.IfNull(varPCP(4), "")
            arrPcp(5) = _DB.IfNull(varPCP(5), "")
            arrPcp(6) = _DB.IfNull(varPCP(6), "")
            arrPcp(7) = _DB.IfNull(varPCP(7), "")
            arrPcp(8) = _DB.IfNull(varPCP(8), "")
        End If

        If Not IsArray(varREF) Or (varREF.Value.varValue.IsNullorEmpty) Then
            arrRef(0) = ""
            arrRef(1) = ""
            arrRef(2) = ""
            arrRef(3) = ""
            arrRef(4) = ""
            arrRef(5) = ""
            arrRef(6) = "2"
            arrRef(7) = 0
            arrRef(8) = ""
        Else
            arrRef(0) = _DB.IfNull(varREF(0), "")
            arrRef(1) = _DB.IfNull(varREF(1), "")
            arrRef(2) = _DB.IfNull(varREF(2), "")
            arrRef(3) = _DB.IfNull(varREF(3), "")
            arrRef(4) = _DB.IfNull(varREF(4), "")
            arrRef(5) = _DB.IfNull(varREF(5), "")
            arrRef(6) = _DB.IfNull(varREF(6), "2")
            arrRef(7) = _DB.IfNull(varREF(7), 0)
            arrRef(8) = _DB.IfNull(varREF(8), "")
        End If

        '       arrCC(1, 3) Credit Card Info
        If Not IsArray(varCC) Or (varCC.Value.varValue.IsNullorEmpty) Then
            arrCC(0, 0) = ""
            arrCC(0, 1) = ""
            arrCC(0, 2) = 0
            arrCC(0, 3) = 0
            arrCC(0, 4) = ""
            arrCC(1, 0) = ""
            arrCC(1, 1) = ""
            arrCC(1, 2) = 0
            arrCC(1, 3) = 0
            arrCC(1, 4) = ""
        Else
            arrCC(0, 0) = _DB.IfNull(varCC(0, 0), "")
            arrCC(0, 1) = _DB.IfNull(varCC(0, 1), "")
            arrCC(0, 2) = IIf(Not IsNumeric(_DB.IfNull(varCC(0, 2), 0)), 0, _DB.IfNull(varCC(0, 2), 0))
            arrCC(0, 3) = IIf(Not IsNumeric(_DB.IfNull(varCC(0, 3), 0)), 0, _DB.IfNull(varCC(0, 3), 0))
            arrCC(0, 4) = _DB.IfNull(varCC(0, 4), "")
            arrCC(1, 0) = _DB.IfNull(varCC(1, 0), "")
            arrCC(1, 1) = _DB.IfNull(varCC(1, 1), "")
            arrCC(1, 2) = IIf(Not IsNumeric(_DB.IfNull(varCC(1, 2), 0)), 0, _DB.IfNull(varCC(1, 2), 0))
            arrCC(1, 3) = IIf(Not IsNumeric(_DB.IfNull(varCC(1, 3), 0)), 0, _DB.IfNull(varCC(1, 3), 0))
            arrCC(1, 4) = _DB.IfNull(varCC(1, 4), "")
        End If

        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdBenefactor"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@Last", adVarChar, adParamInput, 20, strLast))
            .Parameters.Append(.CreateParameter("@Maiden", adVarChar, adParamInput, 20, strMaiden))
            .Parameters.Append(.CreateParameter("@MI", adVarChar, adParamInput, 1, strMI))
            .Parameters.Append(.CreateParameter("@First", adVarChar, adParamInput, 20, strFirst))
            .Parameters.Append(.CreateParameter("@NickName", adVarChar, adParamInput, 20, strNickName))
            .Parameters.Append(.CreateParameter("@Address1", adVarChar, adParamInput, 60, strAddress1))
            .Parameters.Append(.CreateParameter("@Address2", adVarChar, adParamInput, 60, strAddress2))
            .Parameters.Append(.CreateParameter("@City", adVarChar, adParamInput, 40, strCity))
            .Parameters.Append(.CreateParameter("@County", adVarChar, adParamInput, 40, strCounty))
            .Parameters.Append(.CreateParameter("@State", adVarChar, adParamInput, 2, strState))
            .Parameters.Append(.CreateParameter("@Country", adVarChar, adParamInput, 3, strCountry))
            .Parameters.Append(.CreateParameter("@Zip", adVarChar, adParamInput, 11, strZip))
            .Parameters.Append(.CreateParameter("@HomePhoneDesc", adVarChar, adParamInput, 20, strHomePhoneDesc))
            .Parameters.Append(.CreateParameter("@HomePhone", adVarChar, adParamInput, 15, _MD.NumbersOnly(strHomePhone)))
            .Parameters.Append(.CreateParameter("@HomePhMsgYN", adVarChar, adParamInput, 1, arrFlags(0)))
            .Parameters.Append(.CreateParameter("@MobilePhoneDesc", adVarChar, adParamInput, 20, strMobilePhoneDesc))
            .Parameters.Append(.CreateParameter("@MobilePhone", adVarChar, adParamInput, 15, _MD.NumbersOnly(strMobilePhone)))
            .Parameters.Append(.CreateParameter("@MobilePhMsgYN", adVarChar, adParamInput, 1, arrFlags(1)))
            .Parameters.Append(.CreateParameter("@TextYN", adVarChar, adParamInput, 1, arrFlags(2)))
            .Parameters.Append(.CreateParameter("@WorkPhoneDesc", adVarChar, adParamInput, 20, strWorkPhoneDesc))
            .Parameters.Append(.CreateParameter("@WorkPhone", adVarChar, adParamInput, 15, _MD.NumbersOnly(strWorkPhone)))
            .Parameters.Append(.CreateParameter("@WorkExt", adVarChar, adParamInput, 6, strWorkExt))
            .Parameters.Append(.CreateParameter("@WorkPhMsgYN", adVarChar, adParamInput, 1, arrFlags(3)))
            .Parameters.Append(.CreateParameter("@Email", adVarChar, adParamInput, 60, strEmail))
            .Parameters.Append(.CreateParameter("@EmailYN", adVarChar, adParamInput, 1, arrFlags(4)))
            .Parameters.Append(.CreateParameter("@PrintPatientStmtYN", adVarChar, adParamInput, 1, arrFlags(5)))
            .Parameters.Append(.CreateParameter("@ApptReminderYN", adVarChar, adParamInput, 1, arrFlags(6)))
            .Parameters.Append(.CreateParameter("@NoteTrackingYN", adVarChar, adParamInput, 1, arrFlags(7)))
            .Parameters.Append(.CreateParameter("@DOB", adDBTimeStamp, adParamInput, , IIf(dteDOB = #12:00:00 AM#, DBNull.Value, dteDOB)))
            .Parameters.Append(.CreateParameter("@Sex", adVarChar, adParamInput, 1, IIf(strSex = "", DBNull.Value, strSex)))
            .Parameters.Append(.CreateParameter("@SSN", adVarChar, adParamInput, 12, strSSN))
            .Parameters.Append(.CreateParameter("@DLNum", adVarChar, adParamInput, 20, strDLNum))
            .Parameters.Append(.CreateParameter("@PriCCardNbr", adVarChar, adParamInput, 50, arrCC(0, 0)))
            .Parameters.Append(.CreateParameter("@PriCCardSecurity", adVarChar, adParamInput, 5, arrCC(0, 1)))
            .Parameters.Append(.CreateParameter("@PriCCardMth", adInteger, adParamInput, , IIf(arrCC(0, 2) <= 0, DBNull.Value, arrCC(0, 2))))
            .Parameters.Append(.CreateParameter("@PriCCardYr", adInteger, adParamInput, , IIf(arrCC(0, 3) <= 0, DBNull.Value, arrCC(0, 3))))
            .Parameters.Append(.CreateParameter("@PriCCVaultID", adVarChar, adParamInput, 50, arrCC(0, 4)))
            .Parameters.Append(.CreateParameter("@SecCCardNbr", adVarChar, adParamInput, 50, arrCC(1, 0)))
            .Parameters.Append(.CreateParameter("@SecCCardSecurity", adVarChar, adParamInput, 5, arrCC(1, 1)))
            .Parameters.Append(.CreateParameter("@SecCCardMth", adInteger, adParamInput, , IIf(arrCC(1, 2) <= 0, DBNull.Value, arrCC(1, 2))))
            .Parameters.Append(.CreateParameter("@SecCCardYr", adInteger, adParamInput, , IIf(arrCC(1, 3) <= 0, DBNull.Value, arrCC(1, 3))))
            .Parameters.Append(.CreateParameter("@SecCCVaultID", adVarChar, adParamInput, 50, arrCC(1, 4)))
            .Parameters.Append(.CreateParameter("@MaritalStatusID", adInteger, adParamInput, , IIf(lngMarital <= 0, DBNull.Value, lngMarital)))
            .Parameters.Append(.CreateParameter("@EthnicityID", adInteger, adParamInput, , IIf(lngEthnicity <= 0, DBNull.Value, lngEthnicity)))
            .Parameters.Append(.CreateParameter("@EmploymentID", adInteger, adParamInput, , IIf(lngEmployment <= 0, DBNull.Value, lngEmployment)))
            .Parameters.Append(.CreateParameter("@HHIncomeID", adInteger, adParamInput, , IIf(lngHHIncome <= 0, 1, lngHHIncome)))
            .Parameters.Append(.CreateParameter("@Employer", adVarChar, adParamInput, 35, strEmployer))
            .Parameters.Append(.CreateParameter("@PhyLast", adVarChar, adParamInput, 20, arrPcp(0)))
            .Parameters.Append(.CreateParameter("@PhyFirst", adVarChar, adParamInput, 20, arrPcp(1)))
            .Parameters.Append(.CreateParameter("@PhyMI", adVarChar, adParamInput, 1, arrPcp(2)))
            .Parameters.Append(.CreateParameter("@PhyTitle", adVarChar, adParamInput, 10, arrPcp(3)))
            .Parameters.Append(.CreateParameter("@PhyName", adVarChar, adParamInput, 30, arrPcp(4)))
            .Parameters.Append(.CreateParameter("@PhyNPI", adVarChar, adParamInput, 10, arrPcp(5)))
            .Parameters.Append(.CreateParameter("@PhyID", adVarChar, adParamInput, 20, arrPcp(6)))
            .Parameters.Append(.CreateParameter("@PhyPhone", adVarChar, adParamInput, 15, _MD.NumbersOnly(arrPcp(7))))
            .Parameters.Append(.CreateParameter("@PhyFax", adVarChar, adParamInput, 15, _MD.NumbersOnly(arrPcp(8))))
            .Parameters.Append(.CreateParameter("@ReferLast", adVarChar, adParamInput, 20, arrRef(0)))
            .Parameters.Append(.CreateParameter("@ReferFirst", adVarChar, adParamInput, 20, arrRef(1)))
            .Parameters.Append(.CreateParameter("@ReferMI", adVarChar, adParamInput, 1, arrRef(2)))
            .Parameters.Append(.CreateParameter("@ReferTitle", adVarChar, adParamInput, 10, arrRef(3)))
            .Parameters.Append(.CreateParameter("@ReferID", adInteger, adParamInput, , IIf(arrRef(7) <= 0, DBNull.Value, arrRef(7))))
            .Parameters.Append(.CreateParameter("@ReferBy", adVarChar, adParamInput, 200, arrRef(4)))
            .Parameters.Append(.CreateParameter("@ReferNPI", adVarChar, adParamInput, 10, arrRef(5)))
            .Parameters.Append(.CreateParameter("@ReferType", adVarChar, adParamInput, 1, arrRef(6)))
            .Parameters.Append(.CreateParameter("@EmgName", adVarChar, adParamInput, 30, strEmgName))
            .Parameters.Append(.CreateParameter("@EmgRelat", adVarChar, adParamInput, 20, strEmgRelat))
            .Parameters.Append(.CreateParameter("@EmgPhone", adVarChar, adParamInput, 15, _MD.NumbersOnly(strEmgPhone)))
            .Parameters.Append(.CreateParameter("@GuardianName", adVarChar, adParamInput, 60, strGuardianName))
            .Parameters.Append(.CreateParameter("@GuardianRelat", adVarChar, adParamInput, 20, strGuardianRelat))
            .Parameters.Append(.CreateParameter("@GuardianPhone", adVarChar, adParamInput, 15, _MD.NumbersOnly(strGuardianPhone)))
            .Parameters.Append(.CreateParameter("@OtherPhone1", adVarChar, adParamInput, 15, _MD.NumbersOnly(strOtherPhone1)))
            .Parameters.Append(.CreateParameter("@OtherPhoneDesc1", adVarChar, adParamInput, 50, strOtherPhoneDesc1))
            .Parameters.Append(.CreateParameter("@OtherPhone2", adVarChar, adParamInput, 15, _MD.NumbersOnly(strOtherPhone2)))
            .Parameters.Append(.CreateParameter("@OtherPhoneDesc2", adVarChar, adParamInput, 50, strOtherPhoneDesc2))
            .Parameters.Append(.CreateParameter("@OtherPhone3", adVarChar, adParamInput, 15, _MD.NumbersOnly(strOtherPhone3)))
            .Parameters.Append(.CreateParameter("@OtherPhoneDesc3", adVarChar, adParamInput, 50, strOtherPhoneDesc3))
            .Parameters.Append(.CreateParameter("@Notes", adVarChar, adParamInput, 2500, strNotes))
            .Parameters.Append(.CreateParameter("@InternalID", adVarChar, adParamInput, 25, strInternalID))
            .Parameters.Append(.CreateParameter("@ClinicID", adInteger, adParamInput, , IIf(lngClinicID <= 0, DBNull.Value, lngClinicID)))
            .Parameters.Append(.CreateParameter("@AdmitDate", adDBTimeStamp, adParamInput, , IIf(dteAdmitDate = #12:00:00 AM#, DBNull.Value, dteAdmitDate)))
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@Password", adVarChar, adParamInput, 12, strPassword))
            .Parameters.Append(.CreateParameter("@UpdatedBy", adVarChar, adParamInput, 30, strUpdatedBy))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
            Exit Sub
        End If

        cmdSQL = Nothing

        'Logging to auditlog bob changed the DL action.id on now()




        Exit Sub


        cmdSQL = Nothing
        cmdSQL.Execute()
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Sub
    Public Function InsertBenefactor(ByVal strLast As String, ByVal strMaiden As String, ByVal strMI As String, _
                        ByVal strFirst As String, ByVal strNickName As String, ByVal strAddress1 As String, ByVal strAddress2 As String, _
                        ByVal strCity As String, ByVal strState As String, ByVal strCountry As String, ByVal strZip As String, ByVal strCounty As String, _
                        ByVal strHomePhoneDesc As String, ByVal strHomePhone As String, _
                        ByVal strMobilePhoneDesc As String, ByVal strMobilePhone As String, _
                        ByVal strWorkPhoneDesc As String, ByVal strWorkPhone As String, ByVal strWorkExt As String, _
                        ByVal strEmail As String, _
                        ByVal dteDOB As Date, ByVal strSex As String, ByVal strSSN As String, ByVal strDLNum As String, ByVal varCC As Object, _
                        ByVal lngMarital As Long, ByVal lngEthnicity As Long, ByVal lngHHIncome As Long, _
                        ByVal lngEmployment As Long, ByVal strEmployer As String, _
                        ByVal varPCP As Object, ByVal varREF As Object, ByVal strEmgName As String, ByVal strEmgRelat As String, ByVal strEmgPhone As String, _
                        ByVal strGuardianName As String, ByVal strGuardianRelat As String, ByVal strGuardianPhone As String, _
                        ByVal strOtherPhone1 As String, ByVal strOtherPhoneDesc1 As String, ByVal strOtherPhone2 As String, ByVal strOtherPhoneDesc2 As String, ByVal strOtherPhone3 As String, ByVal strOtherPhoneDesc3 As String, _
                        ByVal strNotes As String, ByVal strInternalID As String, ByVal lngClinicID As Long, _
                        ByVal dteAdmitDate As Date, ByVal strUserName As String, ByVal strPassword As String, ByVal varFlags As Object, _
                        ByVal strAddedBy As String, ByVal cnnSQL As ADODB.Connection, Optional ByVal lngOwnerID As Long = 0) As Long

        Dim strApptReminder As String
        Dim strNoteTracking As String

        Dim cmdSQL As ADODB.Command
        Dim lngID As Long
        Dim arrFlags() As Object
        ReDim arrFlags(10)
        Dim arrPcp() As Object
        ReDim arrPcp(10)
        Dim arrRef() As Object
        ReDim arrRef(10)
        Dim arrCC(0, 0) As Object
        ReDim arrCC(1, 4)



        If Not IsArray(varFlags) Or (varFlags.Value.varValue.IsNullorEmpty) Then
            arrFlags(0) = ""
            arrFlags(1) = ""
            arrFlags(2) = ""
            arrFlags(3) = ""
            arrFlags(4) = ""
            arrFlags(5) = ""
            arrFlags(6) = ""
            arrFlags(7) = ""
            arrFlags(8) = ""
        Else
            arrFlags(0) = _DB.IfNull(varFlags(0), "N")
            arrFlags(1) = _DB.IfNull(varFlags(1), "N")
            arrFlags(2) = _DB.IfNull(varFlags(2), "N")
            arrFlags(3) = _DB.IfNull(varFlags(3), "N")
            arrFlags(4) = _DB.IfNull(varFlags(4), "N")
            arrFlags(5) = _DB.IfNull(varFlags(5), "N")
            arrFlags(6) = _DB.IfNull(varFlags(6), "N")
            arrFlags(7) = _DB.IfNull(varFlags(7), "N")
            arrFlags(8) = _DB.IfNull(varFlags(8), "N")
        End If

        If Not IsArray(varPCP) Or (varPCP.Value.varValue.IsNullorEmpty) Then
            arrPcp(0) = ""
            arrPcp(1) = ""
            arrPcp(2) = ""
            arrPcp(3) = ""
            arrPcp(4) = ""
            arrPcp(5) = ""
            arrPcp(6) = ""
            arrPcp(7) = ""
            arrPcp(8) = ""
        Else
            arrPcp(0) = _DB.IfNull(varPCP(0), "")
            arrPcp(1) = _DB.IfNull(varPCP(1), "")
            arrPcp(2) = _DB.IfNull(varPCP(2), "")
            arrPcp(3) = _DB.IfNull(varPCP(3), "")
            arrPcp(4) = _DB.IfNull(varPCP(4), "")
            arrPcp(5) = _DB.IfNull(varPCP(5), "")
            arrPcp(6) = _DB.IfNull(varPCP(6), "")
            arrPcp(7) = _DB.IfNull(varPCP(7), "")
            arrPcp(8) = _DB.IfNull(varPCP(8), "")
        End If

        If Not IsArray(varREF) Or (varREF.Value.varValue.IsNullorEmpty) Then
            arrRef(0) = ""
            arrRef(1) = ""
            arrRef(2) = ""
            arrRef(3) = ""
            arrRef(4) = ""
            arrRef(5) = ""
            arrRef(6) = "2"
            arrRef(7) = 0
            arrRef(8) = ""
        Else
            arrRef(0) = _DB.IfNull(varREF(0), "")
            arrRef(1) = _DB.IfNull(varREF(1), "")
            arrRef(2) = _DB.IfNull(varREF(2), "")
            arrRef(3) = _DB.IfNull(varREF(3), "")
            arrRef(4) = _DB.IfNull(varREF(4), "")
            arrRef(5) = _DB.IfNull(varREF(5), "")
            arrRef(6) = _DB.IfNull(varREF(6), "2")
            arrRef(7) = _DB.IfNull(varREF(7), 0)
            arrRef(8) = _DB.IfNull(varREF(8), "")
        End If

        '       arrCC(1, 3) Credit Card Info
        If Not IsArray(varCC) Or (varCC.Value.varValue.IsNullorEmpty) Then
            arrCC(0, 0) = ""
            arrCC(0, 1) = ""
            arrCC(0, 2) = 0
            arrCC(0, 3) = 0
            arrCC(0, 4) = ""
            arrCC(1, 0) = ""
            arrCC(1, 1) = ""
            arrCC(1, 2) = 0
            arrCC(1, 3) = 0
            arrCC(1, 4) = ""
        Else
            arrCC(0, 0) = _DB.IfNull(varCC(0, 0), "")
            arrCC(0, 1) = _DB.IfNull(varCC(0, 1), "")
            arrCC(0, 2) = IIf(Not IsNumeric(_DB.IfNull(varCC(0, 2), 0)), 0, _DB.IfNull(varCC(0, 2), 0))
            arrCC(0, 3) = IIf(Not IsNumeric(_DB.IfNull(varCC(0, 3), 0)), 0, _DB.IfNull(varCC(0, 3), 0))
            arrCC(0, 4) = _DB.IfNull(varCC(0, 4), "")
            arrCC(1, 0) = _DB.IfNull(varCC(1, 0), "")
            arrCC(1, 1) = _DB.IfNull(varCC(1, 1), "")
            arrCC(1, 2) = IIf(Not IsNumeric(_DB.IfNull(varCC(1, 2), 0)), 0, _DB.IfNull(varCC(1, 2), 0))
            arrCC(1, 3) = IIf(Not IsNumeric(_DB.IfNull(varCC(1, 3), 0)), 0, _DB.IfNull(varCC(1, 3), 0))
            arrCC(1, 4) = _DB.IfNull(varCC(1, 4), "")
        End If

        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspInsBenefactor"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@BenefactorID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@OwnerID", adInteger, adParamInput, , IIf(lngOwnerID <= 0, DBNull.Value, lngOwnerID)))
            .Parameters.Append(.CreateParameter("@Last", adVarChar, adParamInput, 20, strLast))
            .Parameters.Append(.CreateParameter("@Maiden", adVarChar, adParamInput, 20, strMaiden))
            .Parameters.Append(.CreateParameter("@MI", adVarChar, adParamInput, 1, strMI))
            .Parameters.Append(.CreateParameter("@First", adVarChar, adParamInput, 20, strFirst))
            .Parameters.Append(.CreateParameter("@NickName", adVarChar, adParamInput, 20, strNickName))
            .Parameters.Append(.CreateParameter("@Address1", adVarChar, adParamInput, 60, strAddress1))
            .Parameters.Append(.CreateParameter("@Address2", adVarChar, adParamInput, 60, strAddress2))
            .Parameters.Append(.CreateParameter("@City", adVarChar, adParamInput, 40, strCity))
            .Parameters.Append(.CreateParameter("@County", adVarChar, adParamInput, 40, strCounty))
            .Parameters.Append(.CreateParameter("@State", adVarChar, adParamInput, 2, strState))
            .Parameters.Append(.CreateParameter("@Country", adVarChar, adParamInput, 3, strCountry))
            .Parameters.Append(.CreateParameter("@Zip", adVarChar, adParamInput, 12, strZip))
            .Parameters.Append(.CreateParameter("@HomePhoneDesc", adVarChar, adParamInput, 20, strHomePhoneDesc))
            .Parameters.Append(.CreateParameter("@HomePhone", adVarChar, adParamInput, 15, _MD.NumbersOnly(strHomePhone)))
            .Parameters.Append(.CreateParameter("@HomePhMsgYN", adVarChar, adParamInput, 1, arrFlags(0)))
            .Parameters.Append(.CreateParameter("@MobilePhoneDesc", adVarChar, adParamInput, 20, strMobilePhoneDesc))
            .Parameters.Append(.CreateParameter("@MobilePhone", adVarChar, adParamInput, 15, _MD.NumbersOnly(strMobilePhone)))
            .Parameters.Append(.CreateParameter("@MobilePhMsgYN", adVarChar, adParamInput, 1, arrFlags(1)))
            .Parameters.Append(.CreateParameter("@TextYN", adVarChar, adParamInput, 1, arrFlags(2)))
            .Parameters.Append(.CreateParameter("@WorkPhoneDesc", adVarChar, adParamInput, 20, strWorkPhoneDesc))
            .Parameters.Append(.CreateParameter("@WorkPhone", adVarChar, adParamInput, 15, _MD.NumbersOnly(strWorkPhone)))
            .Parameters.Append(.CreateParameter("@WorkExt", adVarChar, adParamInput, 6, strWorkExt))
            .Parameters.Append(.CreateParameter("@WorkPhMsgYN", adVarChar, adParamInput, 1, arrFlags(3)))
            .Parameters.Append(.CreateParameter("@Email", adVarChar, adParamInput, 60, strEmail))
            .Parameters.Append(.CreateParameter("@EmailYN", adVarChar, adParamInput, 1, arrFlags(4)))
            .Parameters.Append(.CreateParameter("@PrintPatientStmtYN", adVarChar, adParamInput, 1, arrFlags(5)))
            .Parameters.Append(.CreateParameter("@ApptReminderYN", adVarChar, adParamInput, 1, arrFlags(6)))
            .Parameters.Append(.CreateParameter("@NoteTrackingYN", adVarChar, adParamInput, 1, arrFlags(7)))
            .Parameters.Append(.CreateParameter("@DOB", adDBTimeStamp, adParamInput, , IIf(dteDOB = #12:00:00 AM#, DBNull.Value, dteDOB)))
            .Parameters.Append(.CreateParameter("@Sex", adVarChar, adParamInput, 1, strSex))
            .Parameters.Append(.CreateParameter("@SSN", adVarChar, adParamInput, 12, strSSN))
            .Parameters.Append(.CreateParameter("@DLNum", adVarChar, adParamInput, 20, strDLNum))
            .Parameters.Append(.CreateParameter("@PriCCardNbr", adVarChar, adParamInput, 50, arrCC(0, 0)))
            .Parameters.Append(.CreateParameter("@PriCCardSecurity", adVarChar, adParamInput, 5, arrCC(0, 1)))
            .Parameters.Append(.CreateParameter("@PriCCardMth", adInteger, adParamInput, , IIf(arrCC(0, 2) <= 0, DBNull.Value, arrCC(0, 2))))
            .Parameters.Append(.CreateParameter("@PriCCardYr", adInteger, adParamInput, , IIf(arrCC(0, 3) <= 0, DBNull.Value, arrCC(0, 3))))
            .Parameters.Append(.CreateParameter("@PriCCVaultID", adVarChar, adParamInput, 50, arrCC(0, 4)))
            .Parameters.Append(.CreateParameter("@SecCCardNbr", adVarChar, adParamInput, 50, arrCC(1, 0)))
            .Parameters.Append(.CreateParameter("@SecCCardSecurity", adVarChar, adParamInput, 5, arrCC(1, 1)))
            .Parameters.Append(.CreateParameter("@SecCCardMth", adInteger, adParamInput, , IIf(arrCC(1, 2) <= 0, DBNull.Value, arrCC(1, 2))))
            .Parameters.Append(.CreateParameter("@SecCCardYr", adInteger, adParamInput, , IIf(arrCC(1, 3) <= 0, DBNull.Value, arrCC(1, 3))))
            .Parameters.Append(.CreateParameter("@SecCCVaultID", adVarChar, adParamInput, 50, arrCC(1, 4)))
            .Parameters.Append(.CreateParameter("@MaritalStatusID", adInteger, adParamInput, , IIf(lngMarital <= 0, DBNull.Value, lngMarital)))
            .Parameters.Append(.CreateParameter("@EthnicityID", adInteger, adParamInput, , IIf(lngEthnicity <= 0, DBNull.Value, lngEthnicity)))
            .Parameters.Append(.CreateParameter("@EmploymentID", adInteger, adParamInput, , IIf(lngEmployment <= 0, DBNull.Value, lngEmployment)))
            .Parameters.Append(.CreateParameter("@HHIncomeID", adInteger, adParamInput, , IIf(lngHHIncome <= 0, 1, lngHHIncome)))
            .Parameters.Append(.CreateParameter("@Employer", adVarChar, adParamInput, 35, strEmployer))
            .Parameters.Append(.CreateParameter("@PhyLast", adVarChar, adParamInput, 20, arrPcp(0)))
            .Parameters.Append(.CreateParameter("@PhyFirst", adVarChar, adParamInput, 20, arrPcp(1)))
            .Parameters.Append(.CreateParameter("@PhyMI", adVarChar, adParamInput, 1, arrPcp(2)))
            .Parameters.Append(.CreateParameter("@PhyTitle", adVarChar, adParamInput, 10, arrPcp(3)))
            .Parameters.Append(.CreateParameter("@PhyName", adVarChar, adParamInput, 30, arrPcp(4)))
            .Parameters.Append(.CreateParameter("@PhyNPI", adVarChar, adParamInput, 10, arrPcp(5)))
            .Parameters.Append(.CreateParameter("@PhyID", adVarChar, adParamInput, 20, arrPcp(6)))
            .Parameters.Append(.CreateParameter("@PhyPhone", adVarChar, adParamInput, 15, _MD.NumbersOnly(arrPcp(7))))
            .Parameters.Append(.CreateParameter("@PhyFax", adVarChar, adParamInput, 15, _MD.NumbersOnly(arrPcp(8))))
            .Parameters.Append(.CreateParameter("@ReferLast", adVarChar, adParamInput, 20, arrRef(0)))
            .Parameters.Append(.CreateParameter("@ReferFirst", adVarChar, adParamInput, 20, arrRef(1)))
            .Parameters.Append(.CreateParameter("@ReferMI", adVarChar, adParamInput, 1, arrRef(2)))
            .Parameters.Append(.CreateParameter("@ReferTitle", adVarChar, adParamInput, 10, arrRef(3)))
            .Parameters.Append(.CreateParameter("@ReferID", adInteger, adParamInput, , IIf(arrRef(7) <= 0, DBNull.Value, arrRef(7))))
            .Parameters.Append(.CreateParameter("@ReferBy", adVarChar, adParamInput, 200, arrRef(4)))
            .Parameters.Append(.CreateParameter("@ReferNPI", adVarChar, adParamInput, 10, arrRef(5)))
            .Parameters.Append(.CreateParameter("@ReferType", adVarChar, adParamInput, 1, arrRef(6)))
            .Parameters.Append(.CreateParameter("@EmgName", adVarChar, adParamInput, 30, strEmgName))
            .Parameters.Append(.CreateParameter("@EmgRelat", adVarChar, adParamInput, 20, strEmgRelat))
            .Parameters.Append(.CreateParameter("@EmgPhone", adVarChar, adParamInput, 15, _MD.NumbersOnly(strEmgPhone)))
            .Parameters.Append(.CreateParameter("@GuardianName", adVarChar, adParamInput, 60, strGuardianName))
            .Parameters.Append(.CreateParameter("@GuardianRelat", adVarChar, adParamInput, 20, strGuardianRelat))
            .Parameters.Append(.CreateParameter("@GuardianPhone", adVarChar, adParamInput, 15, _MD.NumbersOnly(strGuardianPhone)))
            .Parameters.Append(.CreateParameter("@OtherPhone1", adVarChar, adParamInput, 15, _MD.NumbersOnly(strOtherPhone1)))
            .Parameters.Append(.CreateParameter("@OtherPhoneDesc1", adVarChar, adParamInput, 50, strOtherPhoneDesc1))
            .Parameters.Append(.CreateParameter("@OtherPhone2", adVarChar, adParamInput, 15, _MD.NumbersOnly(strOtherPhone2)))
            .Parameters.Append(.CreateParameter("@OtherPhoneDesc2", adVarChar, adParamInput, 50, strOtherPhoneDesc2))
            .Parameters.Append(.CreateParameter("@OtherPhone3", adVarChar, adParamInput, 15, _MD.NumbersOnly(strOtherPhone3)))
            .Parameters.Append(.CreateParameter("@OtherPhoneDesc3", adVarChar, adParamInput, 50, strOtherPhoneDesc3))
            .Parameters.Append(.CreateParameter("@Notes", adVarChar, adParamInput, 2500, strNotes))
            .Parameters.Append(.CreateParameter("@InternalID", adVarChar, adParamInput, 25, strInternalID))
            .Parameters.Append(.CreateParameter("@ClinicID", adInteger, adParamInput, , IIf(lngClinicID <= 0, DBNull.Value, lngClinicID)))
            .Parameters.Append(.CreateParameter("@AdmitDate", adDBTimeStamp, adParamInput, , IIf(dteAdmitDate = #12:00:00 AM#, DBNull.Value, dteAdmitDate)))
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@Password", adVarChar, adParamInput, 12, strPassword))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 20, strAddedBy))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute() '()

        lngID = cmdSQL.Parameters("@BenefactorID").Value

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
            Exit Function
        End If

        'Free resources
        cmdSQL = Nothing

        InsertBenefactor = lngID


        Exit Function


        cmdSQL = Nothing
        cmdSQL.Execute()
        'Signal incompletion and raise the error to the ing environment.
        'RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME)

    End Function


    Private Function CreatePlansArray(ByVal varRPPlans As Object) As Object
        '--------------------------------------------------------------------
        'Date: 06/15/2000
        'Author: Dave Richkun
        'Description:  Creates a single Plan array from the embedded Plan arrays
        '           contained within each RPPlan array.
        'Parameters: varRPPlans() - A reference to the RPPlans array.  The Plans
        '               are contained within the 31st element of each row of
        '               the RPPlans array.
        'Returns: An array containing all Plans embedded within the RPPlans array.
        '--------------------------------------------------------------------

        Dim varPlans(0, 0) As Object
        Dim varPlansTemp As Object
        Dim intCTR As Integer
        Dim intCtr2 As Integer
        Dim intNumPlans As Integer
        Dim intPlansLoaded As Integer



        Try
            intPlansLoaded = 0
            For intCTR = 0 To UBound(varRPPlans, 2) 'Loop through RP array
                varPlansTemp = varRPPlans(31, intCTR) 'Set reference to embedded Plan Array
                intNumPlans = UBound(varPlansTemp, 2)

                If intCTR = 0 Then
                    ReDim varPlans(2, intNumPlans)
                Else
                    ReDim Preserve varPlans(2, UBound(varPlans, 2) + intNumPlans + 1)
                End If

                For intCtr2 = 0 To UBound(varPlansTemp, 2)
                    varPlans(0, intPlansLoaded) = varRPPlans(0, intCTR) 'RPID
                    varPlans(1, intPlansLoaded) = varPlansTemp(0, intCtr2) 'Plan ID
                    varPlans(2, intPlansLoaded) = varPlansTemp(1, intCtr2) 'Order
                    intPlansLoaded = intPlansLoaded + 1
                Next intCtr2

                Erase varPlansTemp
            Next intCTR

            CreatePlansArray = varPlans


        Catch ex As Exception



        End Try





    End Function


    Private Sub UpdatePlans(ByVal lngPatientID As Long, ByVal varPlans As Object, _
                            ByVal strUserName As String, ByVal cnnSQL As ADODB.Connection)
        '--------------------------------------------------------------------
        'Date: 06/15/2000
        'Author: Dave Richkun
        'Description:  Updates Patient-RP-Plan associations to the database.
        '              As a rule, Patient-RP-Plan records are updated only if the Order
        '              value has changed, or the Patient-RP-Plan record has
        '              been disabled.  Patient-RP-Plan can not be updated if
        '              the PatientID or PlanID has changed.  These types of records
        '              are very different records are more conducive to creating
        '              new recor
        'Parameters: lngPatientID - ID of the patient whose Plans are getting
        '              updated.
        '            varPlans - Object array of RP-Plan information.  The
        '               array is expected to contain the following elements:
        '               0 - The RPID associated with the Plan
        '               1 - The Plan ID
        '               2 - The Order of plan payment priority as it pertains
        '                   to the patient.
        'Returns: Null
        '--------------------------------------------------------------------

        Dim intCTR As Integer
        Dim cmdSQL As ADODB.Command

        Try

        Catch ex As Exception

        End Try

        'Because of unique constraints on the database, first we will change
        'the Order column for all Pat-RP-Plan records associated with the
        'Patient to negative numbers.  When the plans are completed being
        'updated/inserted, those plans that still have negative Order values
        'will be disabled.
        cmdSQL = New ADODB.Command
        With cmdSQL
            .CommandText = "uspUpdPatRPPlanNegatize"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        'Iterate through the Plans array.  For each row in the array, check
        'for a matching combination in the tblPatRPPlan table.  If a match is
        'found, then the Order column is updated.  If a row is not found, then
        'a new row is inserted into tblPatRPPlan.
        For intCTR = 0 To UBound(varPlans, 2)
            If varPlans(0, intCTR) = -1 Then
                varPlans(0, intCTR) = lngPatientID
            End If

            cmdSQL = New ADODB.Command
            With cmdSQL
                .CommandText = "uspUpdPatRPPlanOrder"
                .CommandType = adCmdStoredProc
                .Parameters.Append(.CreateParameter("@Success", adInteger, adParamReturnValue))
                .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
                .Parameters.Append(.CreateParameter("@RPID", adInteger, adParamInput, , varPlans(0, intCTR)))
                .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , varPlans(1, intCTR)))
                .Parameters.Append(.CreateParameter("@Order", adInteger, adParamInput, , varPlans(2, intCTR)))
                .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
            End With
            cmdSQL.ActiveConnection = cnnSQL

            cmdSQL.Execute()
            'Check the ErrorNum parameter before deciding to commit the transaction
            If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
                Exit Sub
            End If

            If cmdSQL.Parameters("@Success").Value < 0 Then
                cmdSQL = Nothing
                cmdSQL = New ADODB.Command
                With cmdSQL
                    .CommandText = "uspInsPatRPPlan"
                    .CommandType = adCmdStoredProc
                    .Parameters.Append(.CreateParameter("@PatRPPlanID", adInteger, adParamReturnValue))
                    .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
                    .Parameters.Append(.CreateParameter("@RPID", adInteger, adParamInput, , varPlans(0, intCTR)))
                    .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , varPlans(1, intCTR)))
                    .Parameters.Append(.CreateParameter("@Order", adInteger, adParamInput, , varPlans(2, intCTR)))
                    .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 20, strUserName))
                    .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
                End With
                cmdSQL.ActiveConnection = cnnSQL

                cmdSQL.Execute()
                'Check the ErrorNum parameter before deciding to commit the transaction
                If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
                    Exit Sub
                End If
            End If
        Next intCTR

        cmdSQL = Nothing



    End Sub


    Private Sub DisableNegativePlans(ByVal lngPatientID As Long, ByVal strUserName As String, ByVal cnnSQL As ADODB.Connection)
        '--------------------------------------------------------------------
        'Date: 06/15/2000
        'Author: Dave Richkun
        'Description:  Disables all Patient-RP-Plan records associated with a
        '              specific Patient whose fldOrder column has a value less
        '              than zero.  These records are assumed to have been removed
        '              from the Patient's list of active RP-Plan recor  The
        '              fldOrder value is updated with a value less than zero during
        '              a Patient-RP-Plan update.  Plans that are still active are
        '              later updated with a value greater than zero during the
        '              Patient-RP-Update, leaving only deleted Plans with fldOrder
        '              values less than zero.
        'Parameters: lngPatientID - ID of the patient whose RP-Plan records are
        '              getting updated.
        '            strUserName - Name of the user initiating the action to disable
        '               Patient-RP-Plan recor
        'Returns: Null
        '--------------------------------------------------------------------

        Dim cmdSQL As ADODB.Command




        Try

        Catch ex As Exception

        End Try

        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        With cmdSQL
            .CommandText = "uspUpdPatRPPlanDisableNegative"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
            Exit Sub
        End If

        cmdSQL = Nothing



    End Sub


    Private Function InsertPlan(ByVal lngPatientID As Long, ByVal lngRPID As Long, ByVal lngPlanID As Long, ByVal strAddedBy As String, ByVal cnnSQL As ADODB.Connection) As Long
        '--------------------------------------------------------------------
        'Date: 07/28/2000
        'Author: Dave Richkun
        'Description:  Inserts a Patient-RP-Plan record.  This routine was causing a timeout
        '              when simply ing BenefactorBz.CPatRPPlanBz.  Moving the code to
        '               the stored procedure, and passing the connection object stopped
        '              the deadlock from occuring.  This must be researched further!!
        'Parameters: lngPatientID - ID of the patient whose RP-Plan records are
        '              getting updated.
        '            lngRPID - ID of the Responsible Party
        '            lngPlanID - ID of the plan that is being associated with the RP
        '            strUserName - Name of the user initiating the action to disable
        '               Patient-RP-Plan recor
        '            cnnSQL - ADO Connection object, opened and instantiated by the ing
        '               routine.
        'Returns: Null
        '--------------------------------------------------------------------

        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        With cmdSQL
            .CommandText = "uspInsPatRPPlan"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatRPPlanID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@RPID", adInteger, adParamInput, , lngRPID))
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID))
            .Parameters.Append(.CreateParameter("@Order", adInteger, adParamInput, , 1))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 20, strAddedBy))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
            Exit Function
        End If

        InsertPlan = cmdSQL.Parameters("@PatRPPlanID").Value

        cmdSQL = Nothing


        Exit Function


        cmdSQL.Execute()
        cmdSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function

    Public Sub Update(ByVal lngPatientID As Long, ByVal strLast As String, ByVal strMaiden As String, ByVal strMI As String, _
                ByVal strFirst As String, ByVal strNickName As String, ByVal strAddress1 As String, ByVal strAddress2 As String, _
                ByVal strCity As String, ByVal strState As String, ByVal strCountry As String, ByVal strZip As String, ByVal strCounty As String, _
                ByVal strHomePhoneDesc As String, ByVal strHomePhone As String, _
                ByVal strMobilePhoneDesc As String, ByVal strMobilePhone As String, _
                ByVal strWorkPhoneDesc As String, ByVal strWorkPhone As String, ByVal strWorkExt As String, _
                ByVal strEmail As String, _
                ByVal dteDOB As Date, ByVal strSex As String, ByVal strSSN As String, _
                ByVal strDLNum As String, ByVal varCC As Object, ByVal lngMarital As Long, ByVal lngEthnicity As Long, ByVal lngHHIncome As Long, _
                ByVal lngEmployment As Long, ByVal strEmployer As String, ByVal varPCP As Object, ByVal varREF As Object, _
                ByVal strEmgName As String, ByVal strEmgRelat As String, ByVal strEmgPhone As String, _
                ByVal strGuardianName As String, ByVal strGuardianRelat As String, ByVal strGuardianPhone As String, _
                ByVal strOtherPhone1 As String, ByVal strOtherPhoneDesc1 As String, ByVal strOtherPhone2 As String, ByVal strOtherPhoneDesc2 As String, ByVal strOtherPhone3 As String, ByVal strOtherPhoneDesc3 As String, _
                ByVal strNotes As String, ByVal strInternalID As String, ByVal lngClinicID As Long, _
                ByVal dteAdmitDate As Date, ByVal strUserName As String, ByVal strPassword As String, ByVal varFlags As Object, _
                ByVal strAddedBy As String, ByVal varRPPlans As Object)
        '--------------------------------------------------------------------
        'Date: 06/13/2000
        'Author: Dave Richkun
        'Description:  Controls the update procedures for a patient and their
        '              Responsible Party and Plan information.  This method
        '              performs multiple updates to the database, and in order
        '              to prevent deadlocks from occuring, all database updates
        '              need to be contained within the same transaction context,
        '              This contributes to the length of this method.
        'Parameters: Each parameter identifies the column value that will be
        '              updated.
        '            varRPPlans - A Variant array containing one or more Responsible
        '               Parties (RP) to associate with the Patient.  Each RP includes
        '               a second Variant array containing one or more Plans.
        '            varPCP -
        '            1) PhysLast
        '            2) PhysFirst
        '            3) PhysMI
        '            4) strPhysTitle
        '            5) strPhysician
        '            6) strPhysNPI
        '            7) PhysID
        '            8) PhysPhone
        '            9) PhysFax
        'Returns: Null
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim varPlans As Object
        Dim intRPCtr As Integer
        Dim intCTR As Integer
        Dim arrFlags() As Object
        ReDim arrFlags(10)
        Dim arrPcp() As Object
        ReDim arrPcp(10)
        Dim arrRef() As Object
        ReDim arrRef(10)
        Dim arrCC(0, 0) As Object
        ReDim arrCC(1, 4)



        If Not IsArray(varFlags) Or (varFlags.Value.varValue.IsNullorEmpty) Then
            arrFlags(0) = ""
            arrFlags(1) = ""
            arrFlags(2) = ""
            arrFlags(3) = ""
            arrFlags(4) = ""
            arrFlags(5) = ""
            arrFlags(6) = ""
            arrFlags(7) = ""
            arrFlags(8) = ""
        Else
            arrFlags(0) = _DB.IfNull(varFlags(0), "N")
            arrFlags(1) = _DB.IfNull(varFlags(1), "N")
            arrFlags(2) = _DB.IfNull(varFlags(2), "N")
            arrFlags(3) = _DB.IfNull(varFlags(3), "N")
            arrFlags(4) = _DB.IfNull(varFlags(4), "N")
            arrFlags(5) = _DB.IfNull(varFlags(5), "N")
            arrFlags(6) = _DB.IfNull(varFlags(6), "N")
            arrFlags(7) = _DB.IfNull(varFlags(7), "N")
            arrFlags(8) = _DB.IfNull(varFlags(8), "N")
        End If

        If Not IsArray(varPCP) Or (varPCP.Value.varValue.IsNullorEmpty) Then
            arrPcp(0) = ""
            arrPcp(1) = ""
            arrPcp(2) = ""
            arrPcp(3) = ""
            arrPcp(4) = ""
            arrPcp(5) = ""
            arrPcp(6) = ""
            arrPcp(7) = ""
            arrPcp(8) = ""
        Else
            arrPcp(0) = _DB.IfNull(varPCP(0), "")
            arrPcp(1) = _DB.IfNull(varPCP(1), "")
            arrPcp(2) = _DB.IfNull(varPCP(2), "")
            arrPcp(3) = _DB.IfNull(varPCP(3), "")
            arrPcp(4) = _DB.IfNull(varPCP(4), "")
            arrPcp(5) = _DB.IfNull(varPCP(5), "")
            arrPcp(6) = _DB.IfNull(varPCP(6), "")
            arrPcp(7) = _DB.IfNull(varPCP(7), "")
            arrPcp(8) = _DB.IfNull(varPCP(8), "")
        End If

        If Not IsArray(varREF) Or (varREF.Value.varValue.IsNullorEmpty) Then
            arrRef(0) = ""
            arrRef(1) = ""
            arrRef(2) = ""
            arrRef(3) = ""
            arrRef(4) = ""
            arrRef(5) = ""
            arrRef(6) = "2"
            arrRef(7) = 0
            arrRef(8) = ""
        Else
            arrRef(0) = _DB.IfNull(varREF(0), "")
            arrRef(1) = _DB.IfNull(varREF(1), "")
            arrRef(2) = _DB.IfNull(varREF(2), "")
            arrRef(3) = _DB.IfNull(varREF(3), "")
            arrRef(4) = _DB.IfNull(varREF(4), "")
            arrRef(5) = _DB.IfNull(varREF(5), "")
            arrRef(6) = _DB.IfNull(varREF(6), "2")
            arrRef(7) = _DB.IfNull(varREF(7), 0)
            arrRef(8) = _DB.IfNull(varREF(8), "")
        End If

        '       arrCC(1, 3) Credit Card Info
        If Not IsArray(varCC) Or (varCC.Value.varValue.IsNullorEmpty) Then
            arrCC(0, 0) = ""
            arrCC(0, 1) = ""
            arrCC(0, 2) = 0
            arrCC(0, 3) = 0
            arrCC(0, 4) = ""
            arrCC(1, 0) = ""
            arrCC(1, 1) = ""
            arrCC(1, 2) = 0
            arrCC(1, 3) = 0
            arrCC(1, 4) = ""
        Else
            arrCC(0, 0) = _DB.IfNull(varCC(0, 0), "")
            arrCC(0, 1) = _DB.IfNull(varCC(0, 1), "")
            arrCC(0, 2) = IIf(Not IsNumeric(_DB.IfNull(varCC(0, 2), 0)), 0, _DB.IfNull(varCC(0, 2), 0))
            arrCC(0, 3) = IIf(Not IsNumeric(_DB.IfNull(varCC(0, 3), 0)), 0, _DB.IfNull(varCC(0, 3), 0))
            arrCC(0, 4) = _DB.IfNull(varCC(0, 4), "")
            arrCC(1, 0) = _DB.IfNull(varCC(1, 0), "")
            arrCC(1, 1) = _DB.IfNull(varCC(1, 1), "")
            arrCC(1, 2) = IIf(Not IsNumeric(_DB.IfNull(varCC(1, 2), 0)), 0, _DB.IfNull(varCC(1, 2), 0))
            arrCC(1, 3) = IIf(Not IsNumeric(_DB.IfNull(varCC(1, 3), 0)), 0, _DB.IfNull(varCC(1, 3), 0))
            arrCC(1, 4) = _DB.IfNull(varCC(1, 4), "")
        End If

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        Call cnnSQL.Open(_ConnectionString)

        'Update the Patient record
        Call UpdateBenefactor(lngPatientID, strLast, strMaiden, strMI, strFirst, strNickName, _
                    strAddress1, strAddress2, strCity, strState, strCountry, strZip, strCounty, strHomePhoneDesc, strHomePhone, _
                    strMobilePhoneDesc, strMobilePhone, strWorkPhoneDesc, strWorkPhone, strWorkExt, _
                    strEmail, dteDOB, strSex, strSSN, _
                    strDLNum, varCC, lngMarital, lngEthnicity, lngHHIncome, lngEmployment, strEmployer, varPCP, varREF, _
                    strEmgName, strEmgRelat, strEmgPhone, _
                    strGuardianName, strGuardianRelat, strGuardianPhone, _
                    strOtherPhone1, strOtherPhoneDesc1, strOtherPhone2, strOtherPhoneDesc2, strOtherPhone3, strOtherPhoneDesc3, _
                    strNotes, strInternalID, lngClinicID, dteAdmitDate, strUserName, strPassword, varFlags, strAddedBy, cnnSQL)

        'Insert/Update RP record(s)
        'The business logic is based on the following conditions:
        'If the value of the BenefactorID (array element[0, x]) is:
        'Less than zero: The patient is considered their own responsible party.
        '       We have already updated the Benefactor.    Array elements 1 to 30 are ignored.
        'Equal to zero: A new benefactor record (responsible party) will be created.
        'Greater than zero: An existing responsible party record whose ID is represented by the
        '       data value will be updated.
        For intRPCtr = 0 To UBound(varRPPlans, 2)
            If varRPPlans(0, intRPCtr) < 0 Then
                'Patient is their own RP; no action required
            ElseIf varRPPlans(0, intRPCtr) = 0 Then
                'Insert a new Benefactor record; Populate the first element of the array with the ID of
                'the new Benefactor
                varRPPlans(0, intRPCtr) = InsertBenefactor(varRPPlans(1, intRPCtr), _DB.IfNull(varRPPlans(2, intRPCtr), ""), _
                        _DB.IfNull(varRPPlans(3, intRPCtr), ""), varRPPlans(4, intRPCtr), _DB.IfNull(varRPPlans(5, intRPCtr), ""), _
                        _DB.IfNull(varRPPlans(6, intRPCtr), ""), _DB.IfNull(varRPPlans(7, intRPCtr), ""), _DB.IfNull(varRPPlans(8, intRPCtr), ""), _
                        _DB.IfNull(varRPPlans(9, intRPCtr), ""), strCountry, _DB.IfNull(varRPPlans(10, intRPCtr), ""), _DB.IfNull(varRPPlans(11, intRPCtr), ""), _DB.IfNull(varRPPlans(12, intRPCtr), ""), _
                        _DB.IfNull(varRPPlans(13, intRPCtr), ""), _DB.IfNull(varRPPlans(15, intRPCtr), ""), _DB.IfNull(varRPPlans(16, intRPCtr), ""), _
                        _DB.IfNull(varRPPlans(18, intRPCtr), ""), _DB.IfNull(varRPPlans(19, intRPCtr), ""), _DB.IfNull(varRPPlans(20, intRPCtr), ""), _
                        _DB.IfNull(varRPPlans(22, intRPCtr), ""), _DB.IfNull(varRPPlans(25, intRPCtr), 0), _
                        _DB.IfNull(varRPPlans(26, intRPCtr), 0), _DB.IfNull(varRPPlans(27, intRPCtr), ""), _DB.IfNull(varRPPlans(28, intRPCtr), ""), "", _
                        _DB.IfNull(varRPPlans(29, intRPCtr), ""), _DB.IfNull(varRPPlans(30, intRPCtr), ""), _DB.IfNull(varRPPlans(31, intRPCtr), ""), _
                        _DB.IfNull(varRPPlans(32, intRPCtr), ""), _DB.IfNull(varRPPlans(33, intRPCtr), ""), _DB.IfNull(varRPPlans(34, intRPCtr), ""), _DB.IfNull(varRPPlans(35, intRPCtr), ""), _
                        _DB.IfNull(varRPPlans(36, intRPCtr), ""), _DB.IfNull(varRPPlans(37, intRPCtr), ""), _DB.IfNull(varRPPlans(38, intRPCtr), ""), _
                        _DB.IfNull(varRPPlans(39, intRPCtr), ""), _DB.IfNull(varRPPlans(40, intRPCtr), ""), _DB.IfNull(varRPPlans(41, intRPCtr), ""), _
                        _DB.IfNull(varRPPlans(42, intRPCtr), ""), _DB.IfNull(varRPPlans(43, intRPCtr), ""), _DB.IfNull(varRPPlans(44, intRPCtr), ""), _
                        _DB.IfNull(varRPPlans(45, intRPCtr), ""), _DB.IfNull(varRPPlans(46, intRPCtr), ""), _DB.IfNull(varRPPlans(47, intRPCtr), ""), _
                        _DB.IfNull(varRPPlans(48, intRPCtr), ""), _DB.IfNull(varRPPlans(49, intRPCtr), ""), 0, "01\01\1900", "", "", varFlags, strAddedBy, cnnSQL)

            ElseIf varRPPlans(0, intRPCtr) > 0 Then
                'Update an existing Benefactor record
                Call UpdateBenefactor(varRPPlans(0, intRPCtr), varRPPlans(1, intRPCtr), _DB.IfNull(varRPPlans(2, intRPCtr), ""), _
                        _DB.IfNull(varRPPlans(3, intRPCtr), ""), varRPPlans(4, intRPCtr), _DB.IfNull(varRPPlans(5, intRPCtr), ""), _
                        _DB.IfNull(varRPPlans(6, intRPCtr), ""), _DB.IfNull(varRPPlans(7, intRPCtr), ""), _DB.IfNull(varRPPlans(8, intRPCtr), ""), _
                        _DB.IfNull(varRPPlans(9, intRPCtr), ""), strCountry, _DB.IfNull(varRPPlans(10, intRPCtr), ""), _DB.IfNull(varRPPlans(11, intRPCtr), ""), _DB.IfNull(varRPPlans(12, intRPCtr), ""), _
                        _DB.IfNull(varRPPlans(13, intRPCtr), ""), _DB.IfNull(varRPPlans(15, intRPCtr), ""), _DB.IfNull(varRPPlans(16, intRPCtr), ""), _
                        _DB.IfNull(varRPPlans(18, intRPCtr), ""), _DB.IfNull(varRPPlans(19, intRPCtr), ""), _DB.IfNull(varRPPlans(20, intRPCtr), ""), _
                        _DB.IfNull(varRPPlans(22, intRPCtr), ""), _
                        _DB.IfNull(varRPPlans(25, intRPCtr), 0), _
                        _DB.IfNull(varRPPlans(26, intRPCtr), 0), _DB.IfNull(varRPPlans(27, intRPCtr), ""), _DB.IfNull(varRPPlans(28, intRPCtr), ""), "", _
                        _DB.IfNull(varRPPlans(29, intRPCtr), ""), _DB.IfNull(varRPPlans(30, intRPCtr), ""), _DB.IfNull(varRPPlans(31, intRPCtr), ""), _
                        _DB.IfNull(varRPPlans(32, intRPCtr), ""), _DB.IfNull(varRPPlans(33, intRPCtr), ""), _DB.IfNull(varRPPlans(34, intRPCtr), ""), _DB.IfNull(varRPPlans(35, intRPCtr), ""), _
                        _DB.IfNull(varRPPlans(36, intRPCtr), ""), _DB.IfNull(varRPPlans(37, intRPCtr), ""), _DB.IfNull(varRPPlans(38, intRPCtr), ""), _
                        _DB.IfNull(varRPPlans(39, intRPCtr), ""), _DB.IfNull(varRPPlans(40, intRPCtr), ""), _DB.IfNull(varRPPlans(41, intRPCtr), ""), _
                        _DB.IfNull(varRPPlans(42, intRPCtr), ""), _DB.IfNull(varRPPlans(43, intRPCtr), ""), _DB.IfNull(varRPPlans(44, intRPCtr), ""), _
                        _DB.IfNull(varRPPlans(45, intRPCtr), ""), _DB.IfNull(varRPPlans(46, intRPCtr), ""), _DB.IfNull(varRPPlans(47, intRPCtr), ""), _
                        _DB.IfNull(varRPPlans(48, intRPCtr), ""), _DB.IfNull(varRPPlans(49, intRPCtr), ""), 0, "01\01\1900", "", "", varFlags, strAddedBy, cnnSQL)
            End If
        Next intRPCtr

        'Get the Plans from the RPPlans array and update them in the database.
        varPlans = CreatePlansArray(varRPPlans)

        Call UpdatePlans(lngPatientID, varPlans, strAddedBy, cnnSQL)
        Call DisableNegativePlans(lngPatientID, strAddedBy, cnnSQL)

        'Close and free the connection
        cnnSQL.Close()
        cnnSQL = Nothing


    End Sub

End Class


