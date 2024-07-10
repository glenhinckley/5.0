VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CUserBz"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CUserBz                                                '
'Date: 01/03/2000                                                   '
'Author: Dave Richkun                                               '
'Description:  MTS business object designed to call methods         '
'               associated with the CUserDB class.                  '
'--------------------------------------------------------------------
'Revision History:                                                  '
'   R001 Richkun 02/03/2000 Added FetchClinics method               '
'   R002 Richkun 02/16/2000 Added FetchClinicsByUserName method;    '
'           renamed FetchClinics to FetchClinicsByUserID            '
'   R003 Segura 02/28/2000 Removed 1 role restriction               '
'   R004 Richkun 03/23/2000 Added FetchDetail()
'   R005 Richkun 04/10/2000 Added FetchProviders()
'   R006 Richkun 04/17/2000 Added ChangePassword()
'   R007 Richkun 06/26/2000 Added FetchMatchingBenefactors()
'   R008 Richkun 06/26/2000 Added FetchMatchingPatients()
'   R009 Richkun 09/04/2000 Added CheckUserProvider(), CheckUserBenefactor()
'   R010 Richkun 10/13/2000 Added FetchArchivedPatients()
'   R011 Richkun 11/04/2000 Added FetchPatientAndOffMgrProviders()
'   R012 Richkun 11/06/2000 Added FetchPatientAndOffMgrArchiveHistory()
'   R013 Lockwood 03/07/2001 Added FetchPatData()
'   R014 Lockwood 03/27/2001 Added FetchMatchingArchivedPatients()
'   R015 Richkun 04/19/2001 Added FetchNextID()
'   R016 Richkun 05/07/2002 Added ChangeEmail()
'--------------------------------------------------------------------
Option Explicit

Private Const CLASS_NAME As String = "CUserBz"

Private Type USER_INFO_1003
   usri1003_password As Long
End Type

Public Function Fetch(Optional blnIncludeDisabled As Boolean = False, _
                      Optional ByVal strWhere As String, _
                      Optional ByVal strOrderBy As String) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 01/03/2000
'Author: Dave Richkun
'Description:  Retrieves records from the tblUsers table.
'Parameters: blnIncludeDisabled - Optional parameter that identifies if
'               records flagged as 'Disabled' or 'De-activated' are to be
'               included in the record set. The default value is False.
'            strWhere - Optional 'Where' clause of the SQL statement by
'               which records will be filtered.  If the parameter is not
'               supplied, all records will be retrieved.
'             strOrderBy - Optional 'Order By' clause in which retrieved
'               records will be sorted.  The default is set to the
'               'fldUser' column
'Returns: Null
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    Dim objUser As ClinicDB.CUserDB
    Dim rstSQL As ADODB.Recordset

    On Error GoTo ErrTrap:

    Set objUser = CreateObject("ClinicDB.CUserDB")
    
    Set rstSQL = objUser.Fetch(blnIncludeDisabled, strWhere, strOrderBy)
    Set Fetch = rstSQL
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Set objUser = Nothing

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Function


Public Function FetchDetail(ByVal lngUserID As Long, _
                            ByRef blnIsProvider As Boolean) As ADODB.Recordset   'R004
'--------------------------------------------------------------------
'Date: 03/23/2000
'Author: Dave Richkun
'Description:  Retrieves detailed information about a user
'Parameters: lngUserID - ID of the user to retrieve info for
'            blnIsProvider - Byref parameter that will contain True if the
'               user is assigned the Role of Provider, otherwise False
'Returns: ADO Recordset object containing user information
'--------------------------------------------------------------------
'Revision History:
'--------------------------------------------------------------------

    Dim objUser As ClinicDB.CUserDB

    On Error GoTo ErrTrap:

    Set objUser = CreateObject("ClinicDB.CUserDB")
    
    Set FetchDetail = objUser.FetchDetail(lngUserID, blnIsProvider)
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Set objUser = Nothing

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Function



Public Function FetchClinicsByUserID(ByVal lngUserID As Long) As ADODB.Recordset  'R001
'--------------------------------------------------------------------
'Date: 02/03/2000
'Author: Dave Richkun
'Description:  Retrieves a list of clinics to which a user is associated.
'Parameters: lngUserID - ID of the user to retrieve clinics for
'Returns: A variant array containing a list of Clinics to which
'           the user is associated.
'--------------------------------------------------------------------
'Revision History:
' R002 - Renamed method to FetchClinicsByUserID
'--------------------------------------------------------------------

    Dim objUserClinic As ClinicDB.CUserClinicDB

    On Error GoTo ErrTrap:

    Set objUserClinic = CreateObject("ClinicDB.CUserClinicDB")
    
    Set FetchClinicsByUserID = objUserClinic.FetchClinicsByUserID(lngUserID)
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Set objUserClinic = Nothing

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Function


Public Function FetchClinicsByUserName(ByVal strUserName As String) As ADODB.Recordset 'R002
'--------------------------------------------------------------------
'Date: 02/14/2000
'Author: Dave Richkun
'Description:  Retrieves a list of clinics to which a user is associated.
'Parameters: strUserName - Name of the user to retrieve clinics for
'Returns: A recordset containing a list of Clinics to which the user is associated.
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    Dim objUserClinic As ClinicDB.CUserClinicDB

    On Error GoTo ErrTrap:

    Set objUserClinic = CreateObject("ClinicDB.CUserClinicDB")
    
    Set FetchClinicsByUserName = objUserClinic.FetchClinicsByUserName(strUserName)

    
    'Signal successful completion
    GetObjectContext.SetComplete

    Set objUserClinic = Nothing

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Function


Public Function Authenticate(ByVal strUserName As String) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 01/10/2000
'Author: Dave Richkun
'Description:  Verifies the user exists and is not disabled in the
'              tblUsers table.  If the user is valid, the Last Login
'              Date is updated in the tblUser table.
'Parameters: strUserName - The username to be searched
'            varLastLoginDate - A reference parameter intended to contain
'              the date the user last logged in to the system.
'Returns: The ID associated with the UserName.
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    Dim objUser As ClinicDB.CUserDB
    Dim rstSQL As ADODB.Recordset
    Dim lngUserID As Long
    Dim lngDefaultClinicID As Long
    Dim strMessage As String

    On Error GoTo ErrTrap:

    Set objUser = CreateObject("ClinicDB.CUserDB")
    
    Set rstSQL = objUser.ValidateLogin(strUserName)
    
    If rstSQL Is Nothing Then
        strMessage = "User '" & strUserName & "' is not allowed to access the system."
        GoTo ErrTrap:
    End If

    Set Authenticate = rstSQL
    Set objUser = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    If Err.Number = 0 Then
        Err.Raise Err.Number, CLASS_NAME, strMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If

End Function

Public Function FetchRolesByUserID(ByVal lngUserID As Long) As ADODB.Recordset  'R001
'--------------------------------------------------------------------
'Date: 03/01/2000                                                   '
'Author: Rick "Boom Boom" Segura                                    '
'Description:  Retrieves a list of clinics to which a user is       '
'               associated.                                         '
'Parameters: lngUserID - ID of the user to retrieve clinics for     '
'Returns: A variant array containing a list of Clinics to which     '
'           the user is associated.                                 '
'--------------------------------------------------------------------
'Revision History:                                                  '
'                                                                   '
'--------------------------------------------------------------------

    Dim objUserRole As ClinicDB.CUserRoleDB

    On Error GoTo ErrTrap:

    Set objUserRole = CreateObject("ClinicDB.CUserRoleDB")
    
    Set FetchRolesByUserID = objUserRole.FetchRolesByUserID(lngUserID)
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Set objUserRole = Nothing

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

Public Function FetchRolesByUserName(ByVal strUserName As String) As ADODB.Recordset 'R002
'--------------------------------------------------------------------
'Date: 03/01/2000                                                   '
'Author: Rick "Boom Boom" Segura                                    '
'Description:  Retrieves a list of clinics to which a user is       '
'               associated.                                         '
'Parameters: strUserName - Name of the user to retrieve clinics for '
'Returns: A recordset containing a list of Clinics to which the user'
'           is associated.                                          '
'--------------------------------------------------------------------
'Revision History:                                                  '
'                                                                   '
'--------------------------------------------------------------------

    Dim objUserRole As ClinicDB.CUserRoleDB

    On Error GoTo ErrTrap:

    Set objUserRole = CreateObject("ClinicDB.CUserRoleDB")
    
    Set FetchRolesByUserName = objUserRole.FetchRolesByUserName(strUserName)

    
    'Signal successful completion
    GetObjectContext.SetComplete

    Set objUserRole = Nothing

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Function


Public Function FetchProviders(ByVal lngUserID As Long, _
                            Optional blnIncludeDisabled As Boolean = False) As ADODB.Recordset  'R005
'--------------------------------------------------------------------
'Date: 04/10/2000
'Author: Dave Richkun
'Description:  Retrieves a list of providers to which a specific user is associated.
'              If the user is associated with more than one clinic, then a list of
'              active providers for every clinic in which the user is associated
'              is returned.
'Parameters: lngUserID - ID of the User to return Provider records for
'Returns: A recordset containing a list of Providers to which the user is associated.
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    Dim objUser As ClinicDB.CUserDB

    On Error GoTo ErrTrap:

    Set objUser = CreateObject("ClinicDB.CUserDB")
    
    Set FetchProviders = objUser.FetchProviders(lngUserID, blnIncludeDisabled)
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Set objUser = Nothing

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Function


Public Function FetchPatients(ByVal lngUserID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 04/10/2000
'Author: Dave Richkun
'Description:  Retrieves a list of patients to which a specific user is associated.
'              If the user is associated with more than one clinic, then a list of
'              active patients for every clinic in which the user is associated
'              is returned.
'Parameters: lngUserID - ID of the User to return Patient records for
'Returns: A recordset containing a list of Patients to which the user is associated.
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    Dim objUser As ClinicDB.CUserDB

    On Error GoTo ErrTrap:

    Set objUser = CreateObject("ClinicDB.CUserDB")
    
    Set FetchPatients = objUser.FetchPatients(lngUserID)
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Set objUser = Nothing

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Function
Public Function FetchPatData(ByVal lngUserID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 03/07/2001
'Author: Joshua Lockwood
'Description:  Gets list of patients by provider along with corresponding
'           DSMIV codes and Tax IDs.  Is used to support ENCDE2.asp
'           Added with R008.
'Parameters: lngUserID - ID of the User to return Patient records for
'Returns: recordset
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    Dim objUser As ClinicDB.CUserDB

    On Error GoTo ErrTrap:

    Set objUser = CreateObject("ClinicDB.CUserDB")
    
    Set FetchPatData = objUser.FetchPatData(lngUserID)
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Set objUser = Nothing

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

Public Function FetchArchivedPatients(ByVal lngUserID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 10/13/2000
'Author: Dave Richkun
'Description:  Retrieves a list of archived patients associated with a
'              specific user
'Parameters: lngUserID - ID of the user to retrieve records for
'Returns: ADO Recordset object containing patient information
'--------------------------------------------------------------------
'Revision History:
' R005 - Created
'--------------------------------------------------------------------

    Dim objUser As ClinicDB.CUserDB

    On Error GoTo ErrTrap:

    Set objUser = CreateObject("ClinicDB.CUserDB")
    
    Set FetchArchivedPatients = objUser.FetchArchivedPatients(lngUserID)
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Set objUser = Nothing

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Function
Public Function FetchGroups(ByVal lngUserID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 12/29/2004
'Author: Duane C Orth
'Description:  Retrieves a list of Groups to which a specific user is associated.
'              If the user is associated with more than one clinic, then a list of
'              active Groups for every clinic in which the user is associated
'              is returned.
'Parameters: lngUserID - ID of the User to return Patient records for
'Returns: A recordset containing a list of Groups to which the user is associated.
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    Dim objUser As ClinicDB.CUserDB

    On Error GoTo ErrTrap:

    Set objUser = CreateObject("ClinicDB.CUserDB")
    
    Set FetchGroups = objUser.FetchGroups(lngUserID)
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Set objUser = Nothing

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

Public Function FetchInsCompanys(ByVal lngUserID As Long, _
                      Optional ByVal strOrderBy As String) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 12/29/2004
'Author: Duane C Orth
'Description:  Retrieves a list of Insurance Companys to which a specific user is associated.
'              If the user is associated with more than one clinic, then a list of
'              active Insurance Companys for every clinic in which the user is associated
'              is returned.
'Parameters: lngUserID - ID of the User to return Patient records for
'Returns: A recordset containing a list of Insurance Companys to which the user is associated.
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    Dim objUser As ClinicDB.CUserDB

    On Error GoTo ErrTrap:

    Set objUser = CreateObject("ClinicDB.CUserDB")
    
    Set FetchInsCompanys = objUser.FetchInsCompanys(lngUserID, strOrderBy)
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Set objUser = Nothing

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Function
Public Function FetchInsCompanysByGroup(ByVal lngGroupID As Long, _
                      Optional ByVal strOrderBy As String) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 10/20/2006
'Author: Duane C Orth
'Description:  Retrieves a list of Insurance Companys to which a specific group is associated.
'              If the Group is associated with more than one clinic, then a list of
'              active Insurance Companys for every clinic in which the user is associated
'              is returned.
'Parameters: lngGroupID - ID of the Group to return Patient records for
'Returns: A recordset containing a list of Insurance Companys to which the user is associated.
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    Dim objUser As ClinicDB.CUserDB

    On Error GoTo ErrTrap:

    Set objUser = CreateObject("ClinicDB.CUserDB")
    
    Set FetchInsCompanysByGroup = objUser.FetchInsCompanysByGroup(lngGroupID, strOrderBy)
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Set objUser = Nothing

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

'Begin R007
Public Function FetchMatchingBenefactors(ByVal lngUserID As Long, ByVal strLastName As String, _
                Optional ByVal strFirstName As String) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 04/10/2000
'Author: Dave Richkun
'Description:  Retrieves a list of Benefactors (includes both patients and
'              Responsible Parties) to which a specific user is associated.
'              If the user is associated with more than one clinic, then a list of
'              active patients for every clinic in which the user is associated
'              is returned.
'Parameters: lngUserID - ID of the User to return Benefactor records for
'            strLastName - A complete string or substring that will be used to
'               filter the list of Benefactor records returned by returning only
'               those records whose last name matches the parameter.
'            strFirstName - An optional complete string or substring that will be used to
'               filter the list of Benefactor records returned by returning only
'               those records whose first name matches the parameter.  If the
'               FirstName parameter is provided then the Benefactor records will
'               be filtered using both the Last Name and the First Name.
'Returns: A recordset containing a list of Patients to which the user is associated.
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    Dim objUser As ClinicDB.CUserDB

    On Error GoTo ErrTrap:

    Set objUser = CreateObject("ClinicDB.CUserDB")
    
    Set FetchMatchingBenefactors = objUser.FetchMatchingBenefactors(lngUserID, strLastName, strFirstName)
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Set objUser = Nothing

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Function
' End R007

'Begin R006
Public Function ChangePassword(ByVal strUserName As String, ByVal strOldPassword As String, _
                               ByVal strNewPassword As String, Optional ByVal strPDCName As String, _
                               Optional ByVal dtExpDate As Date) As Integer
'--------------------------------------------------------------------
'Date: 04/17/2000                                                   '
'Author: Dave Richkun
'Description:  Uses API calls that allows a user to change their NT account password.
'Parameters: strUserName - Name of user whose password will be changed
'            strOldPassword - The user's current password
'            strNewPassword - The user's new password
'            strPDCName - the name of the Primary Domain Controller.  The machine
'               name must be prefixed with \\ for the function to work i.e. \\PSYQUEL-PDC
'Returns: 0 if the user's password was successfully changed, -1 otherwise             '
'--------------------------------------------------------------------
  
'  Dim sServer As String
'  Dim sUser As String
'  Dim sNewPass As String
'  Dim sOldPass As String
'  Dim UI1003 As USER_INFO_1003
'  Dim dwLevel As Long
'  Dim lRet As String
'  Dim sNew As String
  Dim objUser As ClinicDB.CUserDB
 
  'StrConv Functions are necessary since VB will perform UNICODE/ANSI translation
  'before passing strings to the NETAPI functions
  'sUser = StrConv(strUserName, vbUnicode)          '--0001
  'sNewPass = StrConv(strNewPassword, vbUnicode)    '--0001

  'See if this is Domain or Computer referenced
  'If Left(strPDCName, 2) = "\\" Then               '--0001
  '  sServer = StrConv(strPDCName, vbUnicode)       '--0001
  'Else                                             '--0001
    'Domain was referenced, get the Primary Domain Controller
  '  sServer = StrConv(GetPDCName(strPDCName), vbUnicode)   '--0001
  'End If                                                   '--0001

  'If strOldPassword = "" Then
     'Administrative over-ride of existing password. Does not require old password
  '   dwLevel = 1003            '--0001
  '   sNew = strNewPassword     '--0001
  '   UI1003.usri1003_password = StrPtr(sNew)                       '--0001
  '   lRet = NetUserSetInfo(sServer, sUser, dwLevel, UI1003, 0&)    '--0001
  'Else                                                             '--0001
     'Set the old password and attempt to change the user's password
  '   sOldPass = StrConv(strOldPassword, vbUnicode)
  '   lRet = NetUserChangePassword(sServer, sUser, sOldPass, sNewPass)
  'End If                                                   '--0001

  'If lRet <> 0 Then                                        '--0001
  '   ChangePassword = -1                                   '--0001
  '   Err.Raise vbObjectError + 512, CLASS_NAME, DisplaySystemError(lRet)   '--0001
  'Else                                                     '--0001
  
On Error GoTo ErrTrap:                                      '--0001
     Set objUser = CreateObject("ClinicDB.CUserDB")
     Call objUser.UpdateLastPWChange(strUserName, strNewPassword, Now, dtExpDate)
  '   Set objUser = Nothing                                '--0001
  '   ChangePassword = 0 'Success                          '--0001
  'End If                                                  '--0001

    'Signal successful completion
    GetObjectContext.SetComplete

    Set objUser = Nothing

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set objUser = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description
    
End Function
'End R006


Public Function FetchMatchingPatients(ByVal lngUserID As Long, ByVal strLastName As String, _
                Optional ByVal strFirstName As String) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 04/10/2000
'Author: Dave Richkun
'Description:  Retrieves a list of Patients to which a specific user is associated.
'              If the user is associated with more than one clinic, then a list of
'              active patients for every clinic in which the user is associated
'              is returned.
'Parameters: lngUserID - ID of the User to return Patient records for
'            strLastName - A complete string or substring that will be used to
'               filter the list of Patient records returned by returning only
'               those records whose last name matches the parameter.
'            strFirstName - An optional complete string or substring that will be used to
'               filter the list of Patient records returned by returning only
'               those records whose first name matches the parameter.  If the
'               FirstName parameter is provided then the Patient records will
'               be filtered using both the Last Name and the First Name.
'Returns: A recordset containing a list of Patients to which the user is associated.
'--------------------------------------------------------------------
'Revision History:
' R008 Created
'--------------------------------------------------------------------

    Dim objUser As ClinicDB.CUserDB

    On Error GoTo ErrTrap:

    Set objUser = CreateObject("ClinicDB.CUserDB")
    
    Set FetchMatchingPatients = objUser.FetchMatchingPatients(lngUserID, strLastName, strFirstName)
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Set objUser = Nothing

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set objUser = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Function


Public Function FetchMatchingArchivedPatients(ByVal lngUserID As Long, ByVal strLastName As String, _
                Optional ByVal strFirstName As String) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 03/27/2001
'Author: Joshua Lockwood
'Description:  Retrieves a list of archived Patients to which a specific user is associated.
'               This includes all clinics the user is associated with.
'Parameters: lngUserID - ID of the User to return Patient records for
'            strLastName - A complete string or substring that will be used to
'               filter the list of Patient records returned by returning only
'               those records whose last name matches the parameter.
'            strFirstName - An optional complete string or substring that will be used to
'               filter the list of Patient records returned by returning only
'               those records whose first name matches the parameter.  If the
'               FirstName parameter is provided then the Patient records will
'               be filtered using both the Last Name and the First Name.
'Returns: A recordset containing a list of Patients to which the user is associated.
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    Dim objUser As ClinicDB.CUserDB

    On Error GoTo ErrTrap:

    Set objUser = CreateObject("ClinicDB.CUserDB")
    
    Set FetchMatchingArchivedPatients = objUser.FetchMatchingArchivedPatients(lngUserID, strLastName, strFirstName)
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Set objUser = Nothing

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set objUser = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

Public Function CheckUserProvider(ByVal lngUserID As Long, ByVal lngProviderID As Long, _
                            Optional blnIncludeDisabled As Boolean = False) As Boolean
'--------------------------------------------------------------------
'Date: 09/04/2000
'Author: Dave Richkun
'Description:  Checks to ensure that the passed ProviderID is associated with the
'              passed UserID.  Used mainly when checking an ASP QueryString variable
'              and testing to see if the user has manipulated the QueryString parameters.
'Parameters: lngUserID - ID of the User
'            lngProviderID - ID of the Provider
'Returns: True if the User-Provider association is valid, False otherwise.
'--------------------------------------------------------------------
'Revision History:
' R009 Created
'--------------------------------------------------------------------

    Dim objUser As ClinicDB.CUserDB
    Dim rst As ADODB.Recordset

    On Error GoTo ErrTrap:

    Set objUser = CreateObject("ClinicDB.CUserDB")
    
    CheckUserProvider = False
        
    Set rst = objUser.FetchProviders(lngUserID, blnIncludeDisabled)
    Do While Not (rst.EOF)
        If rst.Fields("fldUserID").Value = lngProviderID Then
            CheckUserProvider = True
            Exit Do
        Else
            rst.MoveNext
        End If
    Loop
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Set objUser = Nothing
    Set rst = Nothing

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set objUser = Nothing
    Set rst = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Function


Public Function CheckUserBenefactor(ByVal lngUserID As Long, ByVal lngBFactID As Long) As Boolean
'--------------------------------------------------------------------
'Date: 09/04/2000
'Author: Dave Richkun
'Description:  Checks to ensure that the passed BenefactorID is associated with the
'              passed UserID.  Used mainly when checking an ASP QueryString variable
'              and testing to see if the user has manipulated the QueryString parameters.
'Parameters: lngUserID - ID of the User
'            lngBFactID - ID of the Benefactor
'Returns: True if the User-Benefactor association is valid, False otherwise.
'--------------------------------------------------------------------
'Revision History:
' R009 Created
'--------------------------------------------------------------------

    Dim objUser As ClinicDB.CUserDB

    On Error GoTo ErrTrap:

    Set objUser = CreateObject("ClinicDB.CUserDB")
    
    CheckUserBenefactor = False
    CheckUserBenefactor = objUser.CheckUserBenefactor(lngUserID, lngBFactID)
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Set objUser = Nothing

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set objUser = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Function
' End R009


Public Function FetchPatientAndOffMgrProviders(ByVal lngUserID As Long, ByVal lngPatientID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 11/04/2000
'Author: Dave Richkun
'Description:  Retrieves a list of Providers associated with both a patient and
'              an Office Manager.
'Parameters: lngUserID - ID of the Office Manager
'            lngPatientID - ID of the Patient
'Returns: A recordset containing matching records.
'--------------------------------------------------------------------
'Revision History:
'  R011 Created
'--------------------------------------------------------------------

    Dim objUser As ClinicDB.CUserDB

    On Error GoTo ErrTrap:

    Set objUser = CreateObject("ClinicDB.CUserDB")
    
    Set FetchPatientAndOffMgrProviders = objUser.FetchPatientAndOffMgrProviders(lngUserID, lngPatientID)
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Set objUser = Nothing

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set objUser = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Function


Public Function FetchPatientAndOffMgrArchiveHistory(ByVal lngUserID As Long, ByVal lngPatientID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 11/06/2000
'Author: Dave Richkun
'Description:  Retrieves a list of Providers associated with both a Patient and
'              an Office Manager.
'Parameters: lngUserID - ID of the Office Manager
'            lngPatientID - ID of the Patient
'Returns: A recordset containing matching records.
'--------------------------------------------------------------------
'Revision History:
'  R011 Created
'--------------------------------------------------------------------

    Dim objUser As ClinicDB.CUserDB

    On Error GoTo ErrTrap:

    Set objUser = CreateObject("ClinicDB.CUserDB")
    
    Set FetchPatientAndOffMgrArchiveHistory = objUser.FetchPatientAndOffMgrArchiveHistory(lngUserID, lngPatientID)
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Set objUser = Nothing

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set objUser = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Function


Public Function FetchNextID() As Long
'--------------------------------------------------------------------
'Date: 04/19/2001
'Author: Dave Richkun
'Description:  Retrieves next available UserID from database
'Parameters: None
'Returns: Long integer containing next user ID
'--------------------------------------------------------------------
'Revision History:
'  R011 Created
'--------------------------------------------------------------------

    Dim objUser As ClinicDB.CUserDB

    On Error GoTo ErrTrap:

    Set objUser = CreateObject("ClinicDB.CUserDB")
    
    FetchNextID = objUser.FetchNextID()
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Set objUser = Nothing

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set objUser = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

Public Function FetchManagersByProvider(ByVal lngProviderID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 04/24/2001                                                   '
'Author: Rick "Boom Boom" Segura                                    '
'Description:  Retrieves a list of Mangers associated with Provider '
'Parameters: lngProviderID - ID of the Provider                     '
'Returns: A recordset containing matching records.                  '
'--------------------------------------------------------------------
'Revision History:                                                  '
'--------------------------------------------------------------------

    Dim objUser As ClinicDB.CUserDB

    On Error GoTo ErrTrap:

    Set objUser = CreateObject("ClinicDB.CUserDB")
    Set FetchManagersByProvider = objUser.FetchManagersByProvider(lngProviderID)
    Set objUser = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set objUser = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

Public Function FetchManagersByGroupID(ByVal lngGroupID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 04/24/2001                                                   '
'Author: Duane C Orth                                   '
'Description:  Retrieves a list of Mangers associated with GroupID '
'Parameters: lngProviderID - ID of the Provider                     '
'Returns: A recordset containing matching records.                  '
'--------------------------------------------------------------------
'Revision History:                                                  '
'--------------------------------------------------------------------

    Dim objUser As ClinicDB.CUserDB

    On Error GoTo ErrTrap:

    Set objUser = CreateObject("ClinicDB.CUserDB")
    Set FetchManagersByGroupID = objUser.FetchManagersByGroupID(lngGroupID)
    Set objUser = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set objUser = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

Public Function ChangeEmail(ByVal lngUserID As Long, ByVal strEmail As String) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 05/07/2002
'Author: Dave Richkun
'Description:  Updates the email address for a given user
'Parameters: lngUserID - ID of the User
'            strEmail - Email address of the user
'Returns: NULL
'--------------------------------------------------------------------
'Revision History: R016
'--------------------------------------------------------------------

    Dim objUser As ClinicDB.CUserDB

    On Error GoTo ErrTrap:

    Set objUser = CreateObject("ClinicDB.CUserDB")
    
    Call objUser.ChangeEmail(lngUserID, strEmail)
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Set objUser = Nothing

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set objUser = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Function


Public Function FetchCounters(ByVal lngUserID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 03/05/2003
'Author: Dave Richkun
'Description:  Retrieves a list of counters associated with a specific user
'Parameters: lngUserID - ID of the user to retrieve counters for
'Returns: ADO Recordset
'--------------------------------------------------------------------
'Revision History:
'--------------------------------------------------------------------
    
    Dim objUser As ClinicDB.CUserDB

    On Error GoTo ErrTrap:

    Set objUser = CreateObject("ClinicDB.CUserDB")
    Set FetchCounters = objUser.FetchCounters(lngUserID)
    Set objUser = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set objUser = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

