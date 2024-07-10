
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

Imports Psyquel.BusinessRules.CoreLibraryIII.dbStuff




Public Class CEditPatientBz

    '--------------------------------------------------------------------
    'Class Name: CEditPatientBz
    'Date: 04/03/2000
    'Author: Dave Richkun
    'Description:  MTS business object designed to encapsulate logic to insert
    '              and update Patient records and the dependant information i.e.
    '              Responsible Parties and Plans.
    '--------------------------------------------------------------------
    'Revisions:
    '  R001 - Eric Pena - 5/4/2000 : Modified CAddPatientBz.VerifyData to
    '           coincide with BenefactorBz.VerifyData
    '  R002 11/02/2000 Richkun: Altered Insert method to include default PatientDx record
    '  R003 11/17/2000 Richkun: Altered Insert, AddRPPlan methods to include Plan Benefits
    '                  information.
    '  R004 01/03/2001 Nichol: Removed reference to BenefactorBZ.CPatientDxBz and replaced
    '                  with reference to EMRBZ.CPatientDiagnosisBz.
    '  R005 01/29/2001 Nichol: Changed default diagnoses in automati y inserted patDx
    '                  record to "diagnosis deferred"
    '  R006 02/27/2001 Richkun: Added Optional strAddedByWebUser parameter to Insert() method
    '  R007 04/02/2001 Nichol: Undid R005 -- changed default DSM-IV code to 0
    '  R008 04/18/2001 Richkun: Added support for Home and Work Phone Message columns
    '--------------------------------------------------------------------
    Dim _MD As New ModCommon
    Dim ds As New db

    Private _ConnectionString As String = String.Empty


    Private Const CLASS_NAME As String = "CEditPatientBz"

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property


    Public Function Insert(ByVal strLast As String, ByVal strMI As String, _
                      ByVal strFirst As String, ByVal strAddress1 As String, ByVal strAddress2 As String, _
                      ByVal strCity As String, ByVal strState As String, ByVal strCountry As String, ByVal strZip As String, ByVal strCounty As String, _
                      ByVal strHomePhoneDesc As String, ByVal strHomePhone As String, _
                      ByVal strMobilePhoneDesc As String, ByVal strMobilePhone As String, _
                      ByVal strWorkPhoneDesc As String, ByVal strWorkPhone As String, ByVal strWorkExt As String, _
                      ByVal strEmail As String, _
                      ByVal dteDOB As Date, ByVal strSex As String, ByVal strSSN As String, ByVal strDLNum As String, ByRef varCC As Object, _
                      ByVal lngMarital As Long, ByVal lngEthnicity As Long, ByVal lngHHIncome As Long, _
                      ByVal lngEmployment As Long, ByVal strEmployer As String, ByRef varPCP As Object, ByRef varREF As Object, _
                      ByVal strEmgName As String, ByVal strEmgRelat As String, ByVal strEmgPhone As String, _
                      ByVal strGuardianName As String, ByVal strGuardianRelat As String, ByVal strGuardianPhone As String, _
                      ByVal strOtherPhone1 As String, ByVal strOtherPhoneDesc1 As String, ByVal strOtherPhone2 As String, ByVal strOtherPhoneDesc2 As String, ByVal strOtherPhone3 As String, ByVal strOtherPhoneDesc3 As String, _
                      ByVal strNotes As String, ByVal strInternalID As String, ByVal lngClinicID As Long, _
                      ByVal dteAdmitDate As Date, ByVal strUserName As String, ByVal strPassword As String, ByRef varFlags As Object, ByVal strAddedBy As String, ByVal lngUserID As Long, ByRef varRPPlans As Object, _
                      ByRef varProviderList As Object, Optional ByVal strAddedByWebUser As String = "N") As Long

        'ByVal strMaiden As String,
        'ByVal strNickName As String,
        Dim strMaiden As String
        Dim strNickName As String
        '--------------------------------------------------------------------
        'Date: 04/03/2000
        'Author: Dave Richkun
        'Description:  Controls the insert procedures for a patient and their
        '              Responsible Party and Plan information
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        '            varRPPlans - A Object array containing one or more Responsible
        '               Parties (RP) to associate with the Patient.  Each RP includes
        '               a secondary array containing one or more Plans.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------

        Dim objBenefactor As New BenefactorBz.CBenefactorBz
        Dim objPatRPPlan As New BenefactorBz.CPatRPPlanBz
        Dim objPatProv As New BenefactorDB.CPatientProviderDB
        Dim objPatDx As New EMRBZ.CPatientDiagnosisBZ   'R004
        Dim objPatBenefit As New BenefitsBz.CPatientBenefitBZ 'R003
        Dim rstPlans As New ADODB.Recordset
        Dim strErrMessage As String
        Dim lngPatientID As Long
        Dim lngPatRPPlanID As Long
        Dim lngRPID As Long
        Dim lngPlanID As Long
        Dim intRPCtr As Integer
        Dim intPlanCtr As Integer
        Dim varPlans As Object
        Dim varCCard As Object
        Dim blnOwnRP As Boolean
        Dim blnPlanAssoc As Boolean 'R003
        Dim intCTR As Integer
        Dim lngErrNum As Long
        Dim strErrMsg As String
        Dim strRPRelat As String
        Dim lngTemp As Long

        objBenefactor.ConnectionString = _ConnectionString
        objPatRPPlan.ConnectionString = _ConnectionString
        objPatProv.ConnectionString = _ConnectionString
        objPatRPPlan.ConnectionString = _ConnectionString
        objPatDx.ConnectionString = _ConnectionString
        objPatBenefit.ConnectionString = _ConnectionString








        If Trim(strCountry) = "USA" Then strZip = _MD.NumbersOnly(strZip)
        If Not VerifyData(0, strLast, strFirst, strHomePhone, strMobilePhone, strWorkPhone, strCountry, strZip, strSSN, varPCP, strEmgPhone, varRPPlans, dteDOB, strEmail, strErrMessage) Then Exit Function 'R000


        'just in case no provider was passed (intake center support)
        lngTemp = -1
        If IsArray(varProviderList) Then lngTemp = varProviderList(0) 'varProviderList(0)::0th element is entered as Owner fo patient

        lngPatientID = objBenefactor.Insert(strLast, strMaiden, strMI, strFirst, strNickName, strAddress1, strAddress2, strCity, strState, strCountry, strZip, strCounty, _
                strHomePhoneDesc, _MD.NumbersOnly(strHomePhone), strMobilePhoneDesc, _MD.NumbersOnly(strMobilePhone), _
                strWorkPhoneDesc, _MD.NumbersOnly(strWorkPhone), strWorkExt, strEmail, dteDOB, strSex, _MD.NumbersOnly(strSSN), strDLNum, varCC, _
                lngMarital, lngEthnicity, lngHHIncome, lngEmployment, strEmployer, varPCP, varREF, strEmgName, strEmgRelat, _MD.NumbersOnly(strEmgPhone), _
                strGuardianName, strGuardianRelat, _MD.NumbersOnly(strGuardianPhone), _
                _MD.NumbersOnly(strOtherPhone1), strOtherPhoneDesc1, _MD.NumbersOnly(strOtherPhone2), strOtherPhoneDesc2, _MD.NumbersOnly(strOtherPhone3), strOtherPhoneDesc3, _
                strNotes, strInternalID, lngClinicID, dteAdmitDate, strUserName, strPassword, varFlags, strAddedBy, lngTemp)
        objBenefactor = Nothing

        'Insert a default Diagnosis record for the patient.  R002
        objPatDx.Insert(lngPatientID, strAddedBy, 0, 0, 0, 0, 0, 0, 0, 0, "")
        objPatDx = Nothing

        'Insert/Associate RPPlan record(s)
        'The business logic is based on the following conditions:
        'If the value of the BenefactorID (array element[0, x]) is:
        'Less than zero: The patient is considered their own responsible party.
        '       Array elements 1 to 30 are ignored.
        'Equal to zero: A new responsible party record will be created and associated with the patient
        'Greater than zero: An existing responsible party record whose ID is represented by the
        '       data value will be associated with the patient.  Array elements 1 to 30 are ignored.
        For intRPCtr = 0 To UBound(varRPPlans, 2)
            If varRPPlans(0, intRPCtr) < 0 Then 'Patient is their own RP
                blnOwnRP = True
                lngRPID = lngPatientID
            ElseIf varRPPlans(0, intRPCtr) = 0 Then
                'Insert the RP record
                '            Set objBenefactor = CreateObjectXXXXXXX("BenefactorBz.CBenefactorBz")
                '            lngRPID = objBenefactor.Insert(varRPPlans(1, intRPCtr), ds.IfNull(varRPPlans(2, intRPCtr), ""), _
                '                ds.IfNull(varRPPlans(3, intRPCtr), ""), varRPPlans(4, intRPCtr), ds.IfNull(varRPPlans(5, intRPCtr), ""), _
                '                ds.IfNull(varRPPlans(6, intRPCtr), ""), ds.IfNull(varRPPlans(7, intRPCtr), ""), ds.IfNull(varRPPlans(8, intRPCtr), ""), _
                '                ds.IfNull(varRPPlans(9, intRPCtr), ""), _MD.NumbersOnly(ds.IfNull(varRPPlans(10, intRPCtr), "")), _MD.NumbersOnly(ds.IfNull(varRPPlans(11, intRPCtr), "")), _
                '                ds.IfNull(varRPPlans(12, intRPCtr), "N"), ds.IfNull(varRPPlans(13, intRPCtr), ""), ds.IfNull(varRPPlans(14, intRPCtr), ""), _
                '                ds.IfNull(varRPPlans(15, intRPCtr), "N"), ds.IfNull(varRPPlans(16, intRPCtr), ""), ds.IfNull(varRPPlans(17, intRPCtr), ""), _
                '                ds.IfNull(varRPPlans(18, intRPCtr), ""), ds.IfNull(varRPPlans(19, intRPCtr), 0), ds.IfNull(varRPPlans(20, intRPCtr), 0), _
                '                _MD.NumbersOnly(ds.IfNull(varRPPlans(21, intRPCtr), 0)), ds.IfNull(varRPPlans(22, intRPCtr), 0), ds.IfNull(varRPPlans(23, intRPCtr), 0), _
                '                ds.IfNull(varRPPlans(24, intRPCtr), ""), ds.IfNull(varRPPlans(25, intRPCtr), ""), ds.IfNull(varRPPlans(26, intRPCtr), ""), _
                '                ds.IfNull(varRPPlans(27, intRPCtr), ""), ds.IfNull(varRPPlans(28, intRPCtr), ""), ds.IfNull(varRPPlans(29, intRPCtr), ""), _
                '                ds.IfNull(varRPPlans(30, intRPCtr), ""), ds.IfNull(varRPPlans(31, intRPCtr), ""), ds.IfNull(varRPPlans(32, intRPCtr), ""), _
                '                ds.IfNull(varRPPlans(33, intRPCtr), ""), "", 0, , strAddedBy)

                lngRPID = InsertBenefactor(varRPPlans(1, intRPCtr), ds.IfNull(varRPPlans(2, intRPCtr), ""), _
                    ds.IfNull(varRPPlans(3, intRPCtr), ""), varRPPlans(4, intRPCtr), ds.IfNull(varRPPlans(5, intRPCtr), ""), _
                    ds.IfNull(varRPPlans(6, intRPCtr), ""), ds.IfNull(varRPPlans(7, intRPCtr), ""), ds.IfNull(varRPPlans(8, intRPCtr), ""), _
                    ds.IfNull(varRPPlans(9, intRPCtr), ""), strCountry, ds.IfNull(varRPPlans(10, intRPCtr), ""), _
                    ds.IfNull(varRPPlans(11, intRPCtr), ""), _MD.NumbersOnly(ds.IfNull(varRPPlans(12, intRPCtr), "")), ds.IfNull(varRPPlans(13, intRPCtr), "N"), _
                    _MD.NumbersOnly(ds.IfNull(varRPPlans(15, intRPCtr), "")), ds.IfNull(varRPPlans(16, intRPCtr), "N"), _
                    _MD.NumbersOnly(ds.IfNull(varRPPlans(18, intRPCtr), "")), ds.IfNull(varRPPlans(19, intRPCtr), ""), ds.IfNull(varRPPlans(20, intRPCtr), "N"), _
                    ds.IfNull(varRPPlans(22, intRPCtr), ""), _
                    ds.IfNull(varRPPlans(25, intRPCtr), 0), _
                    ds.IfNull(varRPPlans(26, intRPCtr), ""), _MD.NumbersOnly(ds.IfNull(varRPPlans(27, intRPCtr), 0)), ds.IfNull(varRPPlans(28, intRPCtr), 0), varCCard, _
                    ds.IfNull(varRPPlans(29, intRPCtr), ""), ds.IfNull(varRPPlans(30, intRPCtr), ""), ds.IfNull(varRPPlans(31, intRPCtr), ""), _
                    ds.IfNull(varRPPlans(32, intRPCtr), ""), ds.IfNull(varRPPlans(33, intRPCtr), ""), ds.IfNull(varRPPlans(34, intRPCtr), ""), ds.IfNull(varRPPlans(35, intRPCtr), ""), _
                    ds.IfNull(varRPPlans(36, intRPCtr), ""), ds.IfNull(varRPPlans(37, intRPCtr), ""), ds.IfNull(varRPPlans(38, intRPCtr), ""), _
                    ds.IfNull(varRPPlans(39, intRPCtr), ""), ds.IfNull(varRPPlans(40, intRPCtr), ""), ds.IfNull(varRPPlans(41, intRPCtr), ""), _
                    ds.IfNull(varRPPlans(42, intRPCtr), ""), ds.IfNull(varRPPlans(43, intRPCtr), ""), ds.IfNull(varRPPlans(44, intRPCtr), ""), _
                    ds.IfNull(varRPPlans(45, intRPCtr), ""), ds.IfNull(varRPPlans(46, intRPCtr), ""), ds.IfNull(varRPPlans(47, intRPCtr), ""), _
                    ds.IfNull(varRPPlans(48, intRPCtr), ""), ds.IfNull(varRPPlans(49, intRPCtr), ""), 0, "01\01\1900", "", "", varFlags, strAddedBy)

            ElseIf varRPPlans(0, intRPCtr) > 0 Then
                'Using an existing RP
                lngRPID = varRPPlans(0, intRPCtr)
            End If

            'R003 - If the RP is not associated with the Plan, then make the association.
            varPlans = varRPPlans(51, intRPCtr) 'Extract the Plans array from the RPPlans array.
            lngPlanID = varPlans(0, 0)

            'Insert the Patient-RP-Plan relatonship, then insert the benefits information
            lngPatRPPlanID = objPatRPPlan.Insert(lngPatientID, lngRPID, lngPlanID, 1, strAddedBy)
            objPatRPPlan = Nothing

            If UBound(varPlans) > 8 Then
                strRPRelat = varPlans(9, 0)
            Else
                strRPRelat = ""
            End If

            'Insert the patient benefits information
            objPatBenefit.Insert(lngPatRPPlanID, varPlans(2, 0), varPlans(3, 0), varPlans(4, 0), _
                        varPlans(5, 0), varPlans(6, 0), varPlans(7, 0), varPlans(8, 0), strRPRelat, _
                        varPlans(10, 0), varPlans(11, 0), varPlans(12, 0), varPlans(13, 0), _
                        varPlans(14, 0), varPlans(15, 0), varPlans(16, 0), varPlans(17, 0), _
                        varPlans(18, 0), varPlans(19, 0), varPlans(20, 0), varPlans(21, 0), _
                        varPlans(22, 0), varPlans(23, 0), varPlans(24, 0), varPlans(25, 0), _
                        varPlans(26, 0), varPlans(27, 0), strAddedBy)
            objPatBenefit = Nothing

        Next intRPCtr

        'Insert Patient Provider Associations
        If IsArray(varProviderList) Then
            If strAddedByWebUser = "Y" Then
                For intCTR = 0 To UBound(varProviderList)
                    objPatProv.InsertWeb(lngPatientID, varProviderList(intCTR), lngUserID, strAddedBy)
                Next
                objPatProv = Nothing
            Else
                For intCTR = 0 To UBound(varProviderList)
                    objPatProv.Insert(lngPatientID, varProviderList(intCTR), strAddedBy)
                Next
                objPatProv = Nothing
            End If
        End If

        Insert = lngPatientID


        Exit Function


        'Signal incompletion and raise the error to the  ing environment.  The
        'condition handles custom business rule checks we may have established.
        lngErrNum = Err.Number
        strErrMsg = Err.Description

        objBenefactor = Nothing
        objPatRPPlan = Nothing
        objPatProv = Nothing
        objPatDx = Nothing
        objPatBenefit = Nothing
        rstPlans = Nothing

        If lngErrNum = 0 Then
            'err.raise(vbObjectError, CLASS_NAME, strErrMessage)
        Else
            'err.raise(lngErrNum, "BenefeactorBz.EditPatientBz", strErrMsg)
        End If

    End Function

    Public Sub Update(ByVal lngPatientID As Long, ByVal strLast As String, ByVal strMaiden As String, ByVal strMI As String, _
                      ByVal strFirst As String, ByVal strNickName As String, ByVal strAddress1 As String, ByVal strAddress2 As String, _
                      ByVal strCity As String, ByVal strState As String, ByVal strCountry As String, ByVal strZip As String, ByVal strCounty As String, _
                      ByVal strHomePhoneDesc As String, ByVal strHomePhone As String, _
                      ByVal strMobilePhoneDesc As String, ByVal strMobilePhone As String, _
                      ByVal strWorkPhoneDesc As String, ByVal strWorkPhone As String, ByVal strWorkExt As String, _
                      ByVal strEmail As String, _
                      ByVal dteDOB As Date, ByVal strSex As String, ByVal strSSN As String, ByVal strDLNum As String, ByRef varCC As Object, _
                      ByVal lngMarital As Long, ByVal lngEthnicity As Long, ByVal lngHHIncome As Long, _
                      ByVal lngEmployment As Long, ByVal strEmployer As String, ByVal varPCP As Object, ByVal varREF As Object, _
                      ByVal strEmgName As String, ByVal strEmgRelat As String, ByVal strEmgPhone As String, _
                      ByVal strGuardianName As String, ByVal strGuardianRelat As String, ByVal strGuardianPhone As String, _
                      ByVal strOtherPhone1 As String, ByVal strOtherPhoneDesc1 As String, ByVal strOtherPhone2 As String, ByVal strOtherPhoneDesc2 As String, ByVal strOtherPhone3 As String, ByVal strOtherPhoneDesc3 As String, _
                      ByVal strNotes As String, ByVal strInternalID As String, ByVal lngClinicID As Long, _
                      ByVal dteAdmitDate As Date, ByVal strUserName As String, ByVal strPassword As String, ByRef varFlags As Object, _
                      ByVal strAddedBy As String, ByVal varRPPlans As Object)

        'ByVal strMaiden As String,
        'ByVal strNickName As String,
        'Dim strMaiden As String
        'Dim strNickName As String
        '--------------------------------------------------------------------
        'Date: 06/13/2000
        'Author: Dave Richkun
        'Description:  Controls the update procedures for a patient and their
        '              Responsible Party and Plan information
        'Parameters: Each parameter identifies the column value that will be
        '              updated.
        '            varRPPlans - A Object array containing one or more Responsible
        '               Parties (RP) to associate with the Patient.  Each RP includes
        '               a second Object array containing one or more Plans.
        'Returns: Null
        '--------------------------------------------------------------------

        Dim objEditPatientDB As New BenefactorDB.CEditPatientDB
        Dim strErrMessage As String
        Dim varPlans As Object

        objEditPatientDB.ConnectionString = _ConnectionString

        If Trim(strCountry) = "USA" Then strZip = _MD.NumbersOnly(strZip)
        If Not VerifyData(0, strLast, strFirst, strHomePhone, strMobilePhone, strWorkPhone, strCountry, strZip, strSSN, varPCP, strEmgPhone, varRPPlans, dteDOB, strEmail, strErrMessage) Then Exit Sub 'R000

        objEditPatientDB.Update(lngPatientID, strLast, strMaiden, strMI, strFirst, strNickName, strAddress1, strAddress2, strCity, strState, strCountry, strZip, strCounty, strHomePhoneDesc, strHomePhone, _
                strMobilePhoneDesc, strMobilePhone, strWorkPhoneDesc, strWorkPhone, strWorkExt, strEmail, dteDOB, strSex, strSSN, strDLNum, varCC, _
                lngMarital, lngEthnicity, lngHHIncome, lngEmployment, strEmployer, varPCP, varREF, strEmgName, strEmgRelat, _MD.NumbersOnly(strEmgPhone), _
                strGuardianName, strGuardianRelat, _MD.NumbersOnly(strGuardianPhone), _
                _MD.NumbersOnly(strOtherPhone1), strOtherPhoneDesc1, _MD.NumbersOnly(strOtherPhone2), strOtherPhoneDesc2, _MD.NumbersOnly(strOtherPhone3), strOtherPhoneDesc3, _
                strNotes, strInternalID, lngClinicID, dteAdmitDate, strUserName, strPassword, varFlags, strAddedBy, varRPPlans)

        objEditPatientDB = Nothing



    End Sub


    Public Sub AddRPPlan(ByVal lngPatientID As Long, ByVal strLast As String, ByVal strMaiden As String, ByVal strMI As String, _
                         ByVal strFirst As String, ByVal strNickName As String, ByVal strAddress1 As String, ByVal strAddress2 As String, _
                         ByVal strCity As String, ByVal strState As String, ByVal strCountry As String, ByVal strZip As String, ByVal strCounty As String, _
                         ByVal strHomePhoneDesc As String, ByVal strHomePhone As String, _
                         ByVal strMobilePhoneDesc As String, ByVal strMobilePhone As String, _
                         ByVal strWorkPhoneDesc As String, ByVal strWorkPhone As String, ByVal strWorkExt As String, _
                         ByVal strEmail As String, _
                         ByVal dteDOB As Date, ByVal strSex As String, ByVal strSSN As String, _
                         ByVal strDLNum As String, ByVal lngMarital As Long, ByVal lngEthnicity As Long, _
                         ByVal lngHHIncome As Long, ByVal lngEmployment As Long, ByVal strEmployer As String, _
                         ByRef varFlags As Object, ByVal arrPlans As Object, ByVal strAddedBy As String, Optional ByVal strUserName As String = "", Optional ByVal strPassword As String = "")
        '--------------------------------------------------------------------
        'Date: 07/28/2000
        'Author: Dave Richkun
        'Description:  Adds a single responsible party and plan record and associates
        '              the Responsible Party to a patient.
        'Parameters: Each parameter identifies the column value that will be
        '              updated.
        'Returns: Null
        '--------------------------------------------------------------------

        Dim objEditPatientDB As New BenefactorBz.CEditPatientBz

        objEditPatientDB.ConnectionString = _ConnectionString

        objEditPatientDB.AddRPPlan(lngPatientID, strLast, strMaiden, strMI, strFirst, strNickName, strAddress1, strAddress2, strCity, strState, strCountry, strZip, strCounty, strHomePhoneDesc, strHomePhone, _
                strMobilePhoneDesc, strMobilePhone, strWorkPhoneDesc, strWorkPhone, strWorkExt, dteDOB, strEmail, strSex, strSSN, strDLNum, lngMarital, lngEthnicity, lngHHIncome, lngEmployment, strEmployer, _
                varFlags, arrPlans, strAddedBy, strUserName, strPassword)
        objEditPatientDB = Nothing



    End Sub


    Public Sub AddExistingRPPlan(ByVal lngPatientID As Long, ByVal lngRPID As Long, _
                                 ByVal arrPlans As Object, ByVal strAddedBy As String)
        '--------------------------------------------------------------------
        'Date: 11/19/2000
        'Author: Dave Richkun
        'Description:  Asoociates an existing responsible party and plan record to a patient.
        'Parameters: Each parameter identifies the column value that will be
        '              updated.
        'Returns: Null
        '--------------------------------------------------------------------

        Dim objEditPatientDB As New BenefactorBz.CEditPatientBz

        objEditPatientDB.ConnectionString = _ConnectionString

        objEditPatientDB.AddExistingRPPlan(lngPatientID, lngRPID, arrPlans, strAddedBy)
        objEditPatientDB = Nothing



    End Sub

    '---------------------------
    'Private Routines
    '---------------------------

    Private Function VerifyData(ByVal lngRPID As Long, ByVal strLast As String, ByVal strFirst As String, ByRef strHomePhone As String, ByRef strMobilePhone As String, ByRef strWorkPhone As String, _
                                ByVal strCountry As String, ByRef strZip As String, ByRef strSSN As String, ByVal varPCP As Object, ByRef strEmgPhone As String, ByVal varRPPlans As Object, ByVal dteDOB As Date, ByRef strEmail As String, ByRef strErrMessage As String) As Boolean  'R000
        '--------------------------------------------------------------------
        'Date: 5/4/2000
        'Author: Eric Pena
        'Description:  Verifies all required data has been provided by the user.
        'Parameters:  The values to be checked.
        'Returns: Boolean value identifying if all criteria has been satisfied.
        '--------------------------------------------------------------------
        Dim objBenefactor As New BenefactorBz.CBenefactorBz
        Dim varPlans As Object

        objBenefactor.ConnectionString = _ConnectionString

        If objBenefactor.VerifyData(lngRPID, strLast, strFirst, strHomePhone, strMobilePhone, strWorkPhone, strCountry, strZip, strSSN, varPCP, strEmgPhone, dteDOB, strEmail, strErrMessage) = False Then
            Exit Function
        End If
        objBenefactor = Nothing

        'Parse varRPPlans to ensure at least 1 Responsible Party record exists.
        If Not IsArray(varRPPlans) Then
            strErrMessage = "You must select at least 1 Responsible Party"

        End If

        If (varRPPlans.IsNullorEmpty) Then
            strErrMessage = "You must select at least 1 Responsible Party"

        End If

        'Parse varRPPlans to ensure each Responsible Party has at least 1 plan record.
        'The PlanIDs are stored in the 32nd element of the array.
        'If the lngRPID parameter is a value greater than zero, then the value
        'of the paramater represents the ID of the Responsible party, and the plans
        'already associated with the Responsible Party will be 'brought over' and
        'associated with the RP 'as is'.

        '    varPlans = varRPPlans(31, 0)
        '
        '    If Not IsArray(varPlans) Then
        '        strErrMessage = "Each Responsible Party must have at least 1 Plan"
        '        GoTo 
        '    End If
        '
        '    If IsEmpty(varPlans) Then
        '        strErrMessage = "Each Responsible Party must have at least 1 Plan"
        '        GoTo 
        '    End If

        'Ensure the 'Order' columns are unique


        ' Ensure that there is at least one provider to associate patient with
        'If IsNull(varProvList) Then
        '    strErrMessage = "Each patient must be associated with at least 1 provider."
        '    GoTo 
        'ElseIf IsEmpty(varProvList) Then
        '    strErrMessage = "Each patient must be associated with at least 1 provider."
        '    GoTo 
        'ElseIf Not IsArray(varProvList) Then
        '    strErrMessage = "Each patient must be associated with at least 1 provider."
        '    GoTo 
        'ElseIf UBound(varProvList) = 0 Then
        '    strErrMessage = "Each patient must be associated with at least 1 provider."
        '    GoTo 
        'End If



        VerifyData = True



    End Function
    Private Function InsertBenefactor(ByVal strLast As String, ByVal strMaiden As String, ByVal strMI As String, _
                        ByVal strFirst As String, ByVal strNickName As String, ByVal strAddress1 As String, ByVal strAddress2 As String, _
                        ByVal strCity As String, ByVal strState As String, ByVal strCountry As String, ByVal strZip As String, ByVal strCounty As String, _
                        ByVal strHomePhoneDesc As String, ByVal strHomePhone As String, _
                        ByVal strMobilePhoneDesc As String, ByVal strMobilePhone As String, _
                        ByVal strWorkPhoneDesc As String, ByVal strWorkPhone As String, ByVal strWorkExt As String, _
                        ByVal strEmail As String, _
                        ByVal dteDOB As Date, ByVal strSex As String, ByVal strSSN As String, _
                        ByVal strDLNum As String, ByVal varCC As Object, ByVal lngMarital As Long, ByVal lngEthnicity As Long, ByVal lngHHIncome As Long, _
                        ByVal lngEmployment As Long, ByVal strEmployer As String, ByRef varPCP As Object, ByRef varREF As Object, _
                        ByVal strEmgName As String, ByVal strEmgRelat As String, ByVal strEmgPhone As String, _
                        ByVal strGuardianName As String, ByVal strGuardianRelat As String, ByVal strGuardianPhone As String, _
                        ByVal strOtherPhone1 As String, ByVal strOtherPhoneDesc1 As String, ByVal strOtherPhone2 As String, ByVal strOtherPhoneDesc2 As String, ByVal strOtherPhone3 As String, ByVal strOtherPhoneDesc3 As String, _
                        ByVal strNotes As String, ByVal strInternalID As String, ByVal lngClinicID As Long, _
                        ByVal dteAdmitDate As Date, ByVal strUserName As String, ByVal strPassword As String, ByRef varFlags As Object, _
                        ByVal strAddedBy As String, Optional ByVal lngOwnerID As Long = 0) As Long

        Dim cnnSQL As New ADODB.Connection
        Dim cmdSQL As New ADODB.Command
        Dim lngID As Long
        Dim arrFlags(0) As Object
        ReDim arrFlags(10)
        Dim arrPcp() As Object
        ReDim arrPcp(10)
        Dim arrRef() As Object
        ReDim arrRef(10)
        Dim arrCC(0, 0) As Object
        ReDim arrCC(1, 3)



        If Not IsArray(varFlags) Or (varFlags.IsNullorEmpty) Then
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
            arrFlags(0) = ds.IfNull(varFlags(0), "N")
            arrFlags(1) = ds.IfNull(varFlags(1), "N")
            arrFlags(2) = ds.IfNull(varFlags(2), "N")
            arrFlags(3) = ds.IfNull(varFlags(3), "N")
            arrFlags(4) = ds.IfNull(varFlags(4), "N")
            arrFlags(5) = ds.IfNull(varFlags(5), "N")
            arrFlags(6) = ds.IfNull(varFlags(6), "N")
            arrFlags(7) = ds.IfNull(varFlags(7), "N")
            arrFlags(8) = ds.IfNull(varFlags(8), "N")
        End If

        If Not IsArray(varPCP) Or varPCP.IsNullorEmpty Then
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
            arrPcp(0) = ds.IfNull(varPCP(0), "")
            arrPcp(1) = ds.IfNull(varPCP(1), "")
            arrPcp(2) = ds.IfNull(varPCP(2), "")
            arrPcp(3) = ds.IfNull(varPCP(3), "")
            arrPcp(4) = ds.IfNull(varPCP(4), "")
            arrPcp(5) = ds.IfNull(varPCP(5), "")
            arrPcp(6) = ds.IfNull(varPCP(6), "")
            arrPcp(7) = ds.IfNull(varPCP(7), "")
            arrPcp(8) = ds.IfNull(varPCP(8), "")
        End If

        If Not IsArray(varREF) Or varREF.IsNullorEmpty Then
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
            arrRef(0) = ds.IfNull(varREF(0), "")
            arrRef(1) = ds.IfNull(varREF(1), "")
            arrRef(2) = ds.IfNull(varREF(2), "")
            arrRef(3) = ds.IfNull(varREF(3), "")
            arrRef(4) = ds.IfNull(varREF(4), "")
            arrRef(5) = ds.IfNull(varREF(5), "")
            arrRef(6) = ds.IfNull(varREF(6), "2")
            arrRef(7) = ds.IfNull(varREF(7), 0)
            arrRef(8) = ds.IfNull(varREF(8), "")
        End If

        '       arrCC(1, 3) Credit Card Info
        If Not IsArray(varCC) Or varCC.IsNullorEmpty Then
            arrCC(0, 0) = ""
            arrCC(0, 1) = ""
            arrCC(0, 2) = 0
            arrCC(0, 3) = 0
            arrCC(1, 0) = ""
            arrCC(1, 1) = ""
            arrCC(1, 2) = 0
            arrCC(1, 3) = 0
        Else
            arrCC(0, 0) = ds.IfNull(varCC(0, 0), "")
            arrCC(0, 1) = ds.IfNull(varCC(0, 1), "")
            arrCC(0, 2) = IIf(Not IsNumeric(ds.IfNull(varCC(0, 2), 0)), 0, ds.IfNull(varCC(0, 2), 0))
            arrCC(0, 3) = IIf(Not IsNumeric(ds.IfNull(varCC(0, 3), 0)), 0, ds.IfNull(varCC(0, 3), 0))
            arrCC(1, 0) = ds.IfNull(varCC(1, 0), "")
            arrCC(1, 1) = ds.IfNull(varCC(1, 1), "")
            arrCC(1, 2) = IIf(Not IsNumeric(ds.IfNull(varCC(1, 2), 0)), 0, ds.IfNull(varCC(1, 2), 0))
            arrCC(1, 3) = IIf(Not IsNumeric(ds.IfNull(varCC(1, 3), 0)), 0, ds.IfNull(varCC(1, 3), 0))
        End If


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
            .Parameters.Append(.CreateParameter("@SecCCardNbr", adVarChar, adParamInput, 50, arrCC(1, 0)))
            .Parameters.Append(.CreateParameter("@SecCCardSecurity", adVarChar, adParamInput, 5, arrCC(1, 1)))
            .Parameters.Append(.CreateParameter("@SecCCardMth", adInteger, adParamInput, , IIf(arrCC(1, 2) <= 0, DBNull.Value, arrCC(1, 2))))
            .Parameters.Append(.CreateParameter("@SecCCardYr", adInteger, adParamInput, , IIf(arrCC(1, 3) <= 0, DBNull.Value, arrCC(1, 3))))
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
            .Parameters.Append(.CreateParameter("@ReferBy", adVarChar, adParamInput, 60, arrRef(4)))
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
            .Parameters.Append(.CreateParameter("@Password", adVarChar, adParamInput, 20, strPassword))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strAddedBy))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        '       .Parameters.Append .CreateParameter("@PriCCardNbr", adVarChar, adParamInput, 20, arrCC(0, 0))
        '       .Parameters.Append .CreateParameter("@PriCCardSecurity", adVarChar, adParamInput, 5, arrCC(0, 1))
        '       .Parameters.Append .CreateParameter("@PriCCardMth", adInteger, adParamInput, , _MD.NumbersOnly(arrCC(0, 2)))
        '       .Parameters.Append .CreateParameter("@PriCCardYr", adInteger, adParamInput, , _MD.NumbersOnly(arrCC(0, 3)))

        '       .Parameters.Append .CreateParameter("@SecCCardNbr", adVarChar, adParamInput, 20, arrCC(1, 0))
        '       .Parameters.Append .CreateParameter("@SecCCardSecurity", adVarChar, adParamInput, 5, arrCC(1, 1))
        '       .Parameters.Append .CreateParameter("@SecCCardMth", adInteger, adParamInput, , _MD.NumbersOnly(arrCC(1, 2)))
        '       .Parameters.Append .CreateParameter("@SecCCardYr", adInteger, adParamInput, , _MD.NumbersOnly(arrCC(1, 3)))



        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute() '()

        lngID = cmdSQL.Parameters("@BenefactorID").Value

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then

        End If

        'Close the connection and free all resources
        cnnSQL.Close()

        'free all resources
        cmdSQL = Nothing
        cnnSQL = Nothing


        InsertBenefactor = lngID



    End Function
End Class
