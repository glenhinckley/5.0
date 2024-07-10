
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class Benefactor



    Private _lngPatientID As Long = 0  '
    Private _strLast As String = String.Empty  '
    Private _strMaiden As String = String.Empty '
    Private _strMI As String = String.Empty '
    Private _strFirst As String = String.Empty '
    Private _strNickName As String = String.Empty '
    Private _strAddress1 As String = String.Empty '
    Private _strAddress2 As String = String.Empty
    Private _strCity As String = String.Empty '
    Private _strState As String = String.Empty
    Private _strCountry As String = String.Empty
    Private _strZip As String = String.Empty
    Private _strCounty As String = String.Empty
    Private _strHomePhoneDesc As String = String.Empty '
    Private _strHomePhone As String = String.Empty
    Private _strMobilePhoneDesc As String = String.Empty
    Private _strMobilePhone As String = String.Empty
    Private _strWorkPhoneDesc As String = String.Empty
    Private _strWorkPhone As String = String.Empty
    Private _strWorkExt As String = String.Empty '
    Private _strEmail As String = String.Empty
    Private _dteDOB As Date
    Private _strSex As String = String.Empty
    Private _strSSN As String = String.Empty
    Private _strDLNum As String = String.Empty
    Private _varCC As Object   '***************************Credit Card ?
    Private _lngMarital As Long = 0
    Private _lngEthnicity As Long = 0
    Private _lngHHIncome As Long = 0
    Private _lngEmployment As Long = 0 '
    Private _strEmployer As String = String.Empty
    Private _varPCP As Object  '***************************
    Private _varREF As Object  '*************************** 
    Private _strEmgName As String = String.Empty
    Private _strEmgRelat As String = String.Empty
    Private _strEmgPhone As String = String.Empty '
    Private _strGuardianName As String = String.Empty
    Private _strGuardianRelat As String = String.Empty
    Private _strGuardianPhone As String = String.Empty '
    Private _strOtherPhone1 As String = String.Empty
    Private _strOtherPhoneDesc1 As String = String.Empty
    Private _strOtherPhone2 As String = String.Empty '
    Private _strOtherPhoneDesc2 As String = String.Empty
    Private _strOtherPhone3 As String = String.Empty
    Private _strOtherPhoneDesc3 As String = String.Empty
    Private _strNotes As String = String.Empty '
    Private _strInternalID As String = String.Empty
    Private _lngClinicID As Long = 0
    Private _dteAdmitDate As Date
    Private _strUserName As String = String.Empty
    Private _strPassword As String = String.Empty
    Private _varFlags As Object
    Private _strAddedBy As String = String.Empty
    Private _varRPPlans As Object



    Public Property lngPatientID() As String
        Set(ByVal value As String)
            _lngPatientID = value
        End Set
        Get
            Return _lngPatientID
        End Get
    End Property

    Public Property strLast() As String
        Set(ByVal value As String)
            _strLast = value
        End Set
        Get
            Return _strLast
        End Get
    End Property


    Public Property strMaiden() As String
        Set(ByVal value As String)
            _strMaiden = value
        End Set
        Get
            Return _strMaiden
        End Get
    End Property

    Public Property strMI() As String
        Set(ByVal value As String)
            _strMI = value
        End Set
        Get
            Return _strMI
        End Get
    End Property


    Public Property strFirst() As String
        Set(ByVal value As String)
            _strFirst = value
        End Set
        Get
            Return _strFirst
        End Get
    End Property


    Public Property strNickName() As String
        Set(ByVal value As String)
            _strNickName = value
        End Set
        Get
            Return _strNickName
        End Get
    End Property

    Public Property strAddress1() As String
        Set(ByVal value As String)
            _strAddress1 = value
        End Set
        Get
            Return _strAddress1
        End Get
    End Property

    Public Property strAddress2() As String
        Set(ByVal value As String)
            _strAddress2 = value
        End Set
        Get
            Return _strAddress2
        End Get
    End Property

    Public Property strCity() As String
        Set(ByVal value As String)
            _strCity = value
        End Set
        Get
            Return _strCity
        End Get
    End Property

    Public Property strState() As String
        Set(ByVal value As String)
            _strLast = value
        End Set
        Get
            Return _strLast
        End Get
    End Property




    Public Property strCountry() As String
        Set(ByVal value As String)
            _strCountry = value
        End Set
        Get
            Return _strCountry
        End Get
    End Property

    Public Property strZip() As Long
        Set(ByVal value As Long)
            _strLast = value
        End Set
        Get
            Return _strLast
        End Get
    End Property

    Public Property strCounty() As String
        Set(ByVal value As String)
            _strCounty = value
        End Set
        Get
            Return _strCounty
        End Get
    End Property


    Public Property strHomePhoneDesc() As String
        Set(ByVal value As String)
            _strHomePhoneDesc = value
        End Set
        Get
            Return _strHomePhoneDesc
        End Get
    End Property



    Public Property strHomePhone() As String
        Set(ByVal value As String)
            _strHomePhone = value
        End Set
        Get
            Return _strHomePhone
        End Get
    End Property



    Public Property strMobilePhoneDesc() As String
        Set(ByVal value As String)
            _strMobilePhoneDesc = value
        End Set
        Get
            Return _strMobilePhoneDesc
        End Get
    End Property



    Public Property strMobilePhone() As String
        Set(ByVal value As String)
            _strMobilePhone = value
        End Set
        Get
            Return _strMobilePhone
        End Get
    End Property




    Public Property strWorkPhoneDesc() As String
        Set(ByVal value As String)
            _strWorkPhoneDesc = value
        End Set
        Get
            Return _strWorkPhoneDesc
        End Get
    End Property



    Public Property strWorkPhone() As String
        Set(ByVal value As String)
            _strWorkPhone = value
        End Set
        Get
            Return _strWorkPhone
        End Get
    End Property



    Public Property strWorkExt() As String
        Set(ByVal value As String)
            _strWorkExt = value
        End Set
        Get
            Return _strWorkExt
        End Get
    End Property



    Public Property strEmail() As String
        Set(ByVal value As String)
            _strEmail = value
        End Set
        Get
            Return _strEmail
        End Get
    End Property


    Public Property dteDOB() As Date
        Set(ByVal value As Date)
            _dteDOB = value
        End Set
        Get
            Return _dteDOB
        End Get
    End Property





    Public Property strSex() As String
        Set(ByVal value As String)
            _strSex = value
        End Set
        Get
            Return _strSex
        End Get
    End Property




    Public Property strSSN() As String
        Set(ByVal value As String)
            _strSSN = value
        End Set
        Get
            Return _strSSN
        End Get
    End Property

    Public Property strDLNum() As Long
        Set(ByVal value As Long)
            _strDLNum = value
        End Set
        Get
            Return _strDLNum
        End Get
    End Property


    Public Property varCC() As Long
        Set(ByVal value As Long)
            _varCC = value
        End Set
        Get
            Return _varCC
        End Get
    End Property

    Public Property lngMarital() As Long
        Set(ByVal value As Long)
            _lngMarital = value
        End Set
        Get
            Return _lngMarital
        End Get
    End Property




    Public Property lngEthnicity() As Long
        Set(ByVal value As Long)
            _lngEthnicity = value
        End Set
        Get
            Return _lngEthnicity
        End Get
    End Property


    Public Property lngHHIncome() As Long
        Set(ByVal value As Long)
            _lngHHIncome = value
        End Set
        Get
            Return _lngHHIncome
        End Get
    End Property




    Public Property lngEmployment() As Long
        Set(ByVal value As Long)
            _lngEmployment = value
        End Set
        Get
            Return _lngEmployment
        End Get
    End Property



    Public Property strEmployer() As String
        Set(ByVal value As String)
            _strEmployer = value
        End Set
        Get
            Return _strEmployer
        End Get
    End Property

    Public Property varPCP() As Object
        Set(ByVal value As Object)
            _varPCP = value
        End Set
        Get
            Return _varPCP
        End Get
    End Property

    Public Property varREF() As Object
        Set(ByVal value As Object)
            _varREF = value
        End Set
        Get
            Return _varREF
        End Get
    End Property


    Public Property strEmgName() As String
        Set(ByVal value As String)
            _strEmgName = value
        End Set
        Get
            Return _strEmgName
        End Get
    End Property

    Public Property strEmgRelat() As String
        Set(ByVal value As String)
            _strEmgRelat = value
        End Set
        Get
            Return _strEmgRelat
        End Get
    End Property

    Public Property strEmgPhone() As String
        Set(ByVal value As String)
            _strEmgPhone = value
        End Set
        Get
            Return _strEmgPhone
        End Get
    End Property

    Public Property strGuardianName() As String
        Set(ByVal value As String)
            _strGuardianName = value
        End Set
        Get
            Return _strGuardianName
        End Get
    End Property


    Public Property strGuardianRelat() As String
        Set(ByVal value As String)
            _strGuardianRelat = value
        End Set
        Get
            Return _strGuardianRelat
        End Get
    End Property


    Public Property strGuardianPhone() As String
        Set(ByVal value As String)
            _strGuardianPhone = value
        End Set
        Get
            Return _strGuardianPhone
        End Get
    End Property

    Public Property strOtherPhone1() As String
        Set(ByVal value As String)
            _strOtherPhone1 = value
        End Set
        Get
            Return _strOtherPhone1
        End Get
    End Property

    Public Property strOtherPhoneDesc1() As String
        Set(ByVal value As String)
            _strOtherPhoneDesc1 = value
        End Set
        Get
            Return _strOtherPhoneDesc1
        End Get
    End Property


    Public Property strOtherPhone2() As String
        Set(ByVal value As String)
            _strOtherPhone2 = value
        End Set
        Get
            Return _strOtherPhone2
        End Get
    End Property

    Public Property strOtherPhoneDesc2() As String
        Set(ByVal value As String)
            _strOtherPhoneDesc2 = value
        End Set
        Get
            Return _strOtherPhoneDesc2
        End Get
    End Property


    Public Property strOtherPhone3() As String
        Set(ByVal value As String)
            _strOtherPhone3 = value
        End Set
        Get
            Return _strOtherPhone3
        End Get
    End Property


    Public Property strOtherPhoneDesc3() As String
        Set(ByVal value As String)
            _strOtherPhoneDesc3 = value
        End Set
        Get
            Return _strOtherPhoneDesc3
        End Get
    End Property

    Public Property strNotes() As String
        Set(ByVal value As String)
            _strNotes = value
        End Set
        Get
            Return _strNotes
        End Get
    End Property


    Public Property strInternalID() As String
        Set(ByVal value As String)
            _strInternalID = value
        End Set
        Get
            Return _strInternalID
        End Get
    End Property


    Public Property lngClinicID() As Long
        Set(ByVal value As Long)
            _lngClinicID = value
        End Set
        Get
            Return _lngClinicID
        End Get
    End Property

    Public Property dteAdmitDate() As Date
        Set(ByVal value As Date)
            _dteAdmitDate = value
        End Set
        Get
            Return _dteAdmitDate
        End Get
    End Property


    Public Property strUserName() As String
        Set(ByVal value As String)
            _strUserName = value
        End Set
        Get
            Return _strUserName
        End Get
    End Property

    Public Property strPassword() As String
        Set(ByVal value As String)
            _strPassword = value
        End Set
        Get
            Return _strPassword
        End Get
    End Property

    Public Property varFlags() As Object
        Set(ByVal value As Object)
            _varFlags = value
        End Set
        Get
            Return _varFlags
        End Get
    End Property

    Public Property strAddedBy() As String
        Set(ByVal value As String)
            _strAddedBy = value
        End Set
        Get
            Return _strAddedBy
        End Get
    End Property
    Public Property varRPPlans() As Object
        Set(ByVal value As Object)
            _varRPPlans = value
        End Set
        Get
            Return _varRPPlans
        End Get
    End Property

End Class
