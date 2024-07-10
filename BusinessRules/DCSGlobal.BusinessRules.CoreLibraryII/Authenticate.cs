using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Win32;
using System.IO;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Security;
using System.Security.Principal;
using System.Net;
using System.DirectoryServices.Protocols;
using DCSGlobal.BusinessRules.CoreLibrary;

using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;

namespace DCSGlobal.BusinessRules.CoreLibraryII
{
    public class Authenticate : IDisposable
    {

        ~Authenticate()
        {
            Dispose(false);
        }



        StringStuff ss = new StringStuff();



        bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }




        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // free other managed objects that implement
                // IDisposable only
            }

            // release any unmanaged objects
            // set the object references to null

            _disposed = true;
        }
        private const int LDAPError_InvalidCredentials = 0x31;
        private const string Domain = "mydomain";

        private string _Error = string.Empty;



        public string Error
        {
            get
            {
                return _Error;

            }


        }



        //        If Primary LDAP server not check with Secondary LDAP for authentication  --- Return 1 success, -1 failed


        //If (logOnLDAPconnection(strUserName, strPassword, sLdapServerIP)) Then 'SUBBA-20160812 'TCP/IP
        //                        strFuncVal = "1"
        //                        sLdapServerAuthenticated = sLdapServerIP
        //                    Else
        //                        If (logOnLDAPconnectionSecServer(strUserName, strPassword, sLdapServerIPSec)) Then 
        //                            strFuncVal = "1"
        //                            sLdapServerAuthenticated = sLdapServerIPSec
        //                        Else
        //                            strFuncVal = "-1"
        //                            'lblError.Text = lblError.Text + " : " + strUserName + " a  ldap user Does Not Exist or not able to autheneticate server " + sLdapServerAuthenticated '2002 'subba-20180417
        //                            'lblError.Text = lblError.Text + " : " + strUserName
        //                            lblError.Text = strUserName + " Unable to Login(Error 40)" 'subba-20180417
        //                        End If
        //                    End If



        public bool logOnLDAPconnectionSecServer(string strUserName, string strPassword, string strLDAPserver)
        {

            bool r = false;

            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(strUserName, strPassword);
            System.DirectoryServices.Protocols.LdapDirectoryIdentifier serverId = new System.DirectoryServices.Protocols.LdapDirectoryIdentifier(strLDAPserver);
            System.DirectoryServices.Protocols.LdapConnection conn = new System.DirectoryServices.Protocols.LdapConnection(serverId, credentials);


            try
            {
                conn.Bind();
                r = true;
            }
            catch (Exception ex)
            {
                r = false;
                _Error = ex.Message;
            }


            return r;
        }

        //--=============

        //If (sIsADPortSecure = "636") Then
        //            Try
        //                Integer.TryParse(sIsADPortSecure, iPort)
        //                Dim ldpaAuth As DcsAdLib = New DcsAdLib()
        //                result = ldpaAuth.AuthenticateADSSL(strLDAPserver, strUserName, strPassword, iPort, _ErrorOut)

        //            Catch ldapEx As System.DirectoryServices.Protocols.LdapException
        //                _Error = ldapEx.Message + ":" + _ErrorOut
        //                lblError.Text = "Unable to Login(Error 46) " + "<!--" + _Error + "-->" 'ex.Message 'subba-20180417
        //                result = False
        //            End Try
        //End If








        /// <summary>
        /// subba-20160808
        /// </summary>
        /// <param name="strUserName"></param>
        /// <param name="strPasswor"></param>
        /// <param name="strLDAPserver"></param>
        /// <param name="ActiveDirectoryPath"></param>
        /// <returns>Bool  error msg is in Error Property</returns>
        public bool logOnLDAPconnection(string strUserName, string strPassword, string strLDAPserver, string ActiveDirectoryPath)
        {

            bool result = false;

            // string strAccountFilter = "sAMAccountName";
            // bool IsUserExists = false;
            string sActiveDirectoryPath = string.Empty;


            try
            {
                sActiveDirectoryPath = ActiveDirectoryPath.Trim();

                NetworkCredential networkCredential = new NetworkCredential(strUserName, strPassword);

                System.DirectoryServices.Protocols.LdapDirectoryIdentifier serverId = new LdapDirectoryIdentifier(strLDAPserver);
                System.DirectoryServices.Protocols.LdapConnection conn = new LdapConnection(serverId, networkCredential);

                conn.Bind();
                result = true;


            }
            catch (LdapException ldapException)
            {

                // public enum ResultCode
                _Error = ldapException.Message;
                // Unfortunately, invalid credentials fall into this block with a specific error code

                //if (ldapException.ErrorCode.Equals(LDAPError_InvalidCredentials))
                {

                    result = false;

                }


            }
            catch (Exception ex)
            {
                _Error = ex.Message;
                result = false;
            }


            return result;

        }


        public bool logOnLDAPconnectionMsg(string strUserName, string strPassword, string strLDAPserver, ref string strErrLdapEx)
        {

            bool result = false;

            // string strAccountFilter = "sAMAccountName";
            // bool IsUserExists = false;
            string sActiveDirectoryPath = string.Empty;
            //  bool IsUserExists = false;


            try
            {

                NetworkCredential networkCredential = new NetworkCredential(strUserName, strPassword);

                System.DirectoryServices.Protocols.LdapDirectoryIdentifier serverId = new LdapDirectoryIdentifier(strLDAPserver);
                System.DirectoryServices.Protocols.LdapConnection conn = new LdapConnection(serverId, networkCredential);

                conn.Bind();
                result = true;






            }
            catch (LdapException ldapException)
            {
                string sErr = string.Empty;
                sErr = ldapException.InnerException.Message;
                sErr = sErr + " : " + ldapException.ServerErrorMessage.ToString();

                strErrLdapEx = ldapException.Message + ":" + sErr;



                // public enum ResultCode
                _Error = ldapException.Message;
                // Unfortunately, invalid credentials fall into this block with a specific error code

                //if (ldapException.ErrorCode.Equals(LDAPError_InvalidCredentials))
                {

                    result = false;

                }


            }
            catch (Exception ex)
            {
                _Error = ex.Message;
                result = false;
            }


            return result;





        }





        public bool logOnLDAPconnection2012(string strUserName, string strPassword, string strLDAPserver, ref string sErrLdap)
        {




            bool result = false;

            // string strAccountFilter = "sAMAccountName";
            // bool IsUserExists = false;
            string sActiveDirectoryPath = string.Empty;
            bool IsUserExists = false;

            NetworkCredential networkCredential = new NetworkCredential(strUserName, strPassword);

            System.DirectoryServices.Protocols.LdapDirectoryIdentifier serverId = new LdapDirectoryIdentifier(strLDAPserver);
            System.DirectoryServices.Protocols.LdapConnection conn = new LdapConnection(serverId, networkCredential);

            try
            {
                conn.AuthType = AuthType.Negotiate;
                conn.Timeout = new TimeSpan(0, 0, 180); //'System.TimeSpan.FromSeconds(120)

                //'LdapConnection lcon = New LdapConnection
                //'		(new LdapDirectoryIdentifier((string)null, false, false));
                //'NetworkCredential nc = new NetworkCredential(Environment.UserName, 
                //'                       "MyPassword", Environment.UserDomainName);
                //'lcon.Credential = nc;
                //'lcon.AuthType = AuthType.Negotiate;
                //'// user has authenticated at this point,
                //'// as the credentials were used to login to the dc.
                //'lcon.Bind(nc);
                //'validation = true;
            }
            catch (LdapException ldapException)
            {

                // public enum ResultCode
                _Error = ldapException.Message;
                // Unfortunately, invalid credentials fall into this block with a specific error code

                //if (ldapException.ErrorCode.Equals(LDAPError_InvalidCredentials))
                {

                    result = false;

                }


            }
            catch (Exception ex)
            {
                _Error = ex.Message;
                result = false;
            }


            return result;



        }








        //    'subba-20160808
        public string logOnToAD(string strUserName, string strPassword)
        {


       //     bool result = false;
            string sReturn = string.Empty;

            // string strAccountFilter = "sAMAccountName";
            // bool IsUserExists = false;
            string sActiveDirectoryPath = string.Empty;
            //  bool IsUserExists = false;


            //    Dim strAccountFilter As String = "sAMAccountName"
            //    Dim IsUserExists As Boolean = False
            //    Dim sActiveDirectoryPath = TextBox3.Text.Trim() 'ConfigurationManager.AppSettings("ActiveDirectoryPath")
            //    Dim sReturn As String = ""
            //    Dim sErr As String = ""





            //    Try
            //        If (CheckBox1.Checked) Then
            //            If (IsAuthenticatedBySecurePort(sActiveDirectoryPath, strUserName, strPassword, sErr)) Then
            //                sReturn = "1"
            //                MsgBox(sError + " " + strUserName + " a ldapSecureWin12 user Authentication Success with Ldap domain:" + TextBox3.Text)
            //            Else
            //                sReturn = "-1"
            //                MsgBox(sError + " " + strUserName + " a ldapSecureWin12 user Does Not Exist...")
            //            End If
            //        ElseIf (CheckBox3.Checked) Then
            //            If (IsAuthenticated(sActiveDirectoryPath, strUserName, strPassword, sErr)) Then
            //                sReturn = "1"
            //                MsgBox(sError + " " + strUserName + " a ldapWin12 user Authentication Success with Ldap domain:" + TextBox3.Text)
            //            Else
            //                sReturn = "-1"
            //                MsgBox(sError + " " + strUserName + " a ldapWin12 user Does Not Exist...")
            //            End If
            //        End If

            //    Catch exDs As DirectoryServicesCOMException
            //        sError = exDs.Message + " " + exDs.StackTrace.Replace(Environment.NewLine, "__")
            //        If exDs.Message.ToLower().Contains("logon failure") Then
            //            sReturn = "-1"
            //        End If
            //    Catch ex As System.Exception
            //        sError = ex.Message + " " + ex.StackTrace.Replace(Environment.NewLine, "__")
            //        If ex.Message.ToLower().Contains("logon failure") Then
            //            sReturn = "-1"
            //        End If
            //    End Try
            return sReturn;




        }







        public bool IsAuthenticated(string domain, string username, string pwd, ref string sRetLdapErr)
        {

            bool result = false;
            string sReturn = string.Empty;

            // string strAccountFilter = "sAMAccountName";
            // bool IsUserExists = false;
            string sActiveDirectoryPath = string.Empty;
            //  bool IsUserExists = false;
            string _path = string.Empty;
            string domainAndUsername = string.Empty;

            domainAndUsername = Convert.ToString(domain + Convert.ToString(@"\")) + username;

            DirectoryEntry entry = new DirectoryEntry(_path, domainAndUsername, pwd, AuthenticationTypes.Anonymous);


            try
            {

                //        ' Bind to the native AdsObject to force authentication.
                //        Dim obj As [Object] = entry.NativeObject

                DirectorySearcher search = new DirectorySearcher(entry);


                search.ServerTimeLimit = new TimeSpan(0, 0, 180);
                search.ClientTimeout = new TimeSpan(0, 0, 180);
                search.Filter = (Convert.ToString("(SAMAccountName=") + username) + ")";
                search.PropertiesToLoad.Add("cn");

                //        Dim result As SearchResult = search.FindOne()
                //        If result Is Nothing Then
                //            obj = Nothing
                //            search.Dispose()
                //            Return False
                //        End If
                //        ' Update the new path to the user in the directory
                //        '_path = result.Path
                //        '_filterAttribute = DirectCast(result.Properties("cn")(0), [String])
                //        obj = Nothing
                search.Dispose();
                //        result = Nothing
            }
            catch
            {

                //        sError = ex.Message + ":" + ex.StackTrace + "Authentication Failed1"
                //        sRetLdapErr = sError
                result = false;

            }
            finally
            {
                entry.Dispose();

            }

            return result;

        }

        //Public Function IsAuthenticatedBySecurePort(domain As String, username As String, pwd As String, ByRef sErrLdap As String) As Boolean
        //    Dim _path As String
        //    Dim domainAndUsername As String = Convert.ToString(domain & Convert.ToString("\")) & username
        //    'Dim entry As New DirectoryEntry(_path, domainAndUsername, pwd)
        //    Dim sSimpleBindBasicAuthUser As String = ""

        //    Try
        //        sSimpleBindBasicAuthUser = TextBox3.Text.Trim()
        //    Catch ex As Exception
        //        sSimpleBindBasicAuthUser = ""  ''goes as secure
        //    End Try

        //    Dim de As New System.DirectoryServices.DirectoryEntry()
        //    de = New System.DirectoryServices.DirectoryEntry(_path, domainAndUsername, pwd, AuthenticationTypes.Secure)


        //    Try
        //        de.AuthenticationType = AuthenticationTypes.ReadonlyServer 'Subba-20160804
        //        ' Bind to the native AdsObject to force authentication.
        //        Dim obj As [Object] = de.NativeObject
        //        Dim search As New DirectorySearcher(de)
        //        search.ServerTimeLimit = New TimeSpan(0, 0, 180)
        //        search.ClientTimeout = New TimeSpan(0, 0, 180)
        //        search.Filter = (Convert.ToString("(SAMAccountName=") & username) + ")"
        //        search.PropertiesToLoad.Add("cn")


        //        Dim result As SearchResult = search.FindOne()
        //        If result Is Nothing Then
        //            obj = Nothing
        //            search.Dispose()
        //            Return False
        //        End If
        //        obj = Nothing
        //        search.Dispose()
        //        result = Nothing
        //    Catch ex As System.Exception
        //        sError = sError + "Authentication Failed:" + ex.Message + ":" + ex.StackTrace
        //        sErrLdap = sError
        //        Return False
        //    Finally
        //        de.Dispose()
        //    End Try
        //    Return True
        //End Function






















        //        'Subba-20160808
        //Function logOnToADApplachian(ByVal strUserName As String, ByVal strPassword As String) As String
        //    Dim strAccountFilter As String = "sAMAccountName"
        //    Dim IsUserExists As Boolean = False
        //    'Dim sActiveDirectoryPath = ConfigurationManager.AppSettings("ActiveDirectoryPath")
        //    Dim sActiveDirectoryPath = TextBox3.Text.Trim()
        //    Dim sReturn As String = ""

        //    ''strUserName = "activedcs"
        //    ''strPassword = "w@tauga15"
        //    'strUserName = "dcsldapuser"
        //    'strPassword = "xyzabc"
        //    'sActiveDirectoryPath = "LDAP://ldap.wataugamc.org/OU=wataugamc,DC=wataugamc,DC=org"
        //    'sActiveDirectoryPath = "LDAP://wmc-dc1.wataugamc.org/OU=wataugamc,DC=wataugamc,DC=org" '10.61.26.55
        //    ''sActiveDirectoryPath = "wmc-dc2.wataugamc.org/OU=wataugamc,DC=wataugamc,DC=org" '10.61.26.135
        //    'sActiveDirectoryPath = "wmc-dc1.wataugamc.org"
        //    'sActiveDirectoryPath = "10.61.26.55"

        //    Dim de As New System.DirectoryServices.DirectoryEntry(sActiveDirectoryPath)
        //    de.Username = strUserName
        //    de.Password = strPassword
        //    de.AuthenticationType = AuthenticationTypes.Secure

        //    'If strUserName = "s_alogix_ldap" Then
        //    '    de.AuthenticationType = AuthenticationTypes.None
        //    'Else
        //    '    de.AuthenticationType = AuthenticationTypes.Secure
        //    'End If

        //    'subba-040815-localDomain
        //    Dim PrincipalContext1 As PrincipalContext = New PrincipalContext(ContextType.Domain, sActiveDirectoryPath)
        //    Dim UserPrincipal1 As UserPrincipal = New UserPrincipal(PrincipalContext1)
        //    Dim searchUser As PrincipalSearcher = New PrincipalSearcher(UserPrincipal1)


        //    Try
        //        'Dim PrincipalContext1 As PrincipalContext = New PrincipalContext(ContextType.Domain, sActiveDirectoryPath)
        //        'Dim UserPrincipal1 As UserPrincipal = New UserPrincipal(PrincipalContext1)
        //        'Dim searchUser As PrincipalSearcher = New PrincipalSearcher(UserPrincipal1)
        //        Dim result As UserPrincipal
        //        Dim isLDapUserExist As Boolean = False
        //        Dim isLdapAuthenticated As Boolean = False

        //        For Each result In searchUser.FindAll()

        //            If (result.SamAccountName = strUserName) Then
        //                isLDapUserExist = True
        //                'lblError.Text = strUserName + " a ldap user Exist..."
        //                isLdapAuthenticated = False
        //                Try
        //                    Dim entry As DirectoryEntry = New DirectoryEntry("LDAP://" + sActiveDirectoryPath, strUserName, strPassword)
        //                    Dim nativeObject As Object = entry.NativeObject

        //                    isLdapAuthenticated = True
        //                    If (isLdapAuthenticated) Then Exit For 'subba-031715
        //                Catch ex As System.DirectoryServices.DirectoryServicesCOMException
        //                    isLdapAuthenticated = False
        //                    sError = sError + " :LDAP Authentication Failed"
        //                End Try
        //            End If
        //        Next
        //        If (isLdapAuthenticated) Then sReturn = "1"
        //        If (isLDapUserExist = False) Then sError = strUserName + " a ldap user Does Not Exist 2008..."
        //        searchUser.Dispose()
        //    Catch exDs As DirectoryServicesCOMException
        //        If exDs.Message.ToLower().Contains("logon failure") Then
        //            sReturn = "-1"
        //            sError = exDs.Message + ":" + "Logon2008 failure DirectoryServicesCOMException"
        //        End If
        //        searchUser.Dispose()
        //    Catch ex As System.Exception
        //        If ex.Message.ToLower().Contains("logon failure") Then
        //            sReturn = "-1"
        //            sError = ex.Message + ":" + "Logon2008 failure systemExceptipon"
        //        End If
        //        searchUser.Dispose()
        //    End Try

        //    '***********IPbasedLocalLDAP**subba-040815*************
        //    'subba-040815-localADip
        //    If (sReturn <> "1") Then 'subba-040815
        //        Dim authentic As Boolean = False
        //        Try
        //            Dim entry As DirectoryEntry = New DirectoryEntry("LDAP://" + sActiveDirectoryPath, strUserName, strPassword)
        //            Dim nativeObject As Object = entry.NativeObject
        //            authentic = True
        //            sReturn = "1"

        //            'dirEntry.Path = "LDAP://192.168.1.1/CN=Users;DC=Yourdomain"
        //            'dirEntry.Username = "yourdomain\sampleuser"
        //            'dirEntry.Password = "samplepassword"
        //        Catch dse As DirectoryServicesCOMException
        //            authentic = False 'dse.Message "Logon failure: unknown user name or bad password.  "
        //            sReturn = "-2"
        //            sError = dse.Message + ":" + "Logon2008 failure Directory entry - DirectoryServicesCOMException"
        //        End Try
        //    End If
        //    '******************************************************
        //    'subba-040815-ExternalLDAPip
        //    If (sReturn <> "1") Then
        //        Dim strSearch As String = strAccountFilter + "=" + strUserName
        //        Dim ds As New DirectorySearcher(de, strSearch)
        //        ds.ServerTimeLimit = New TimeSpan(0, 0, 180)
        //        ds.ClientTimeout = New TimeSpan(0, 0, 180)

        //        Try
        //            Dim sb As New System.Text.StringBuilder()
        //            Dim sr As SearchResult = ds.FindOne() 'LOOOK-ERROR ....
        //            Dim src As SearchResultCollection = ds.FindAll()
        //            Dim vc As ResultPropertyValueCollection = sr.Properties("memberOf")
        //            For Each sResultSet As System.DirectoryServices.SearchResult In src
        //                sb.AppendLine("login name: " + GetProperty(sResultSet, "cn"))
        //                ' Login Name
        //                sb.AppendLine("<BR>")
        //                sb.AppendLine("first name: " + GetProperty(sResultSet, "givenName"))
        //                ' First Name
        //                sb.AppendLine("<BR>")
        //                sb.AppendLine("middle name: " + GetProperty(sResultSet, "initials"))
        //                ' Middle Name
        //                sb.AppendLine("<BR>")
        //            Next
        //            If vc.Count > 0 Then
        //                IsUserExists = True
        //            End If
        //            If IsUserExists = True Then
        //                sError = "AdLoginSuccess"
        //            End If
        //            sReturn = "1"
        //        Catch ex As System.DirectoryServices.DirectoryServicesCOMException
        //            If ex.Message.ToLower().Contains("logon failure") Then
        //                sReturn = "-3"
        //                sError = ex.Message + ":" + "Logon2008 failure Directory entry -3 DirectoryServicesCOMException"
        //            End If
        //        Catch exp As System.Exception
        //            If exp.Message.ToLower().Contains("logon failure") Then
        //                sReturn = "-3"
        //                sError = exp.Message + ":" + "Logon2008 failure Directory entry -3 SystemException"
        //            End If
        //        End Try

        //    End If
        //    Return sReturn
        //End Function








        //        'subba-20171101
        public bool LdapUserExist2012(string activeDirectoryServerDomain, string strUserName, string strPassword, ref string sRetErr)
        {

            bool result = false;

            try
            {
                //        Dim de As DirectoryEntry = New DirectoryEntry("ldap://" + activeDirectoryServerDomain, strUserName + "@" + activeDirectoryServerDomain, strPassword, AuthenticationTypes.Secure)
                //        'Dim de As DirectoryEntry = New DirectoryEntry(activeDirectoryServerDomain, strUserName + "@" + activeDirectoryServerDomain, strPassword, AuthenticationTypes.Secure)
                //        Dim ds As DirectorySearcher = New DirectorySearcher(de)
                //        ds.FindOne()
                result = true;
            }
            catch (Exception ex)
            {
                //    Catch ex As Exception
                sRetErr = ex.Message + ":" + ex.StackTrace.ToString();
                result = false;
                //        'dnsHostName: sdcisc02.ghs.org() 
                //        'ldapServiceName: ghs.org:sdcisc02$@GHS.ORG
                //        'serverName: CN=SDCISC02,CN=Servers,CN=ISC,CN=Sites,CN=Configuration,DC=ghs,DC=org
                //        'ldapauditlogix 10.6.14.94  :636
                //        'EXHQzr0XUuJi2aUPy2xwxvTj!
            }

            return result;
        }
        //End Function
        //'subba-20171103



        //    'subba-20171103
        //Public Function LdapUserAuthTest2012CLIENT(ByVal activeDirectoryServerDomain As String, ByVal strUserName As String, ByVal strPassword As String, ByRef sRetErr As String) As Boolean

        //    Dim bResult As Boolean = False
        //    Try
        //        'Dim de As DirectoryEntry = New DirectoryEntry("ldap://" + activeDirectoryServerDomain, strUserName + "@" + activeDirectoryServerDomain, strPassword, AuthenticationTypes.Secure)
        //        ' ''Dim de As DirectoryEntry = New DirectoryEntry(activeDirectoryServerDomain, strUserName + "@" + activeDirectoryServerDomain, strPassword, AuthenticationTypes.Secure)
        //        'Dim ds As DirectorySearcher = New DirectorySearcher(de)
        //        'ds.FindOne()

        //        Dim ldapSvr As String = ""
        //        Dim ldapUser As String = ""
        //        Dim ldapPwd As String = ""

        //        'LDAP://ldaps.ghs.org/OU=ghs,DC=ghs,DC=org ?????  CN=LDAP Auditlogix,OU=Service Accounts,OU=Audit Logix,OU=GHS Services,DC=ghs,DC=org

        //        'ldapSvr = "ldaps.ghs.org:636"
        //        'ldapSvr = "10.6.14.94"
        //        'ldapUser = "uid=ldapauditlogix,dc=ghs,dc=org"
        //        'ldapUser = "ldapauditlogix"

        //        'ldapSvr = "ldaps.ghs.org"
        //        'ldapUser = "ldapauditlogix"
        //        'ldapPwd = "EXHQzr0XUuJi2aUPy2xwxvTj!"

        //        ldapSvr = activeDirectoryServerDomain
        //        ldapUser = strUserName
        //        ldapPwd = strPassword

        //        Dim connection As LdapConnection = New LdapConnection(ldapSvr)
        //        connection.Timeout = New TimeSpan(0, 0, 30)
        //        'connection.AuthType = AuthType.Basic
        //        connection.AuthType = AuthType.Basic
        //        'AuthenticationTypes()
        //        connection.SessionOptions.ProtocolVersion = 3 '//Set protocol to LDAPv3

        //        Dim credential As New NetworkCredential(ldapUser, ldapPwd)
        //        connection.Bind(credential)
        //        bResult = True
        //        'Return True
        //    Catch ldapEx As LdapException
        //        sRetErr = ldapEx.Message + "LdapErrCode:" + ldapEx.ErrorCode.ToString() + ":" + ldapEx.StackTrace.ToString()
        //        If (ldapEx.ErrorCode = 49) Then Return False

        //        'dnsHostName: sdcisc02.ghs.org() 
        //        'ldapServiceName: ghs.org:sdcisc02$@GHS.ORG
        //        'serverName: CN=SDCISC02,CN=Servers,CN=ISC,CN=Sites,CN=Configuration,DC=ghs,DC=org
        //        'ldapauditlogix 10.6.14.94  :636
        //        'EXHQzr0XUuJi2aUPy2xwxvTj!
        //        '0	LDAP_SUCCESS	Indicates the requested client operation completed successfully.

        //        bResult = False
        //    End Try
        //    Return bResult
        //End Function




        /// <summary>
        /// 
        /// </summary>
        /// <param name="strUserName"></param>
        /// <param name="strPasswor"></param>
        /// <param name="ActiveDirectoryPath"></param>
        /// <param name="port"></param>
        /// <returns>Bool  error msg is in Error Property</returns>
        public bool AuthenticateAD(string strUserName, string strPassword, string ActiveDirectoryPath, int port)
        {

            bool b = false;
            try
            {
                LdapConnection ldapConnection = new LdapConnection(new LdapDirectoryIdentifier(ActiveDirectoryPath, port));
                NetworkCredential networkCredential = new NetworkCredential(strUserName, strPassword);
                ldapConnection.SessionOptions.ProtocolVersion = 3;

                ldapConnection.AuthType = AuthType.Negotiate;

                ldapConnection.Bind(networkCredential);

                b = true;
            }
            catch (LdapException ldapException)
            {

                // public enum ResultCode
                _Error = ldapException.Message;
                // Unfortunately, invalid credentials fall into this block with a specific error code

                if (ldapException.ErrorCode.Equals(LDAPError_InvalidCredentials))
                {

                    b = false;

                }


            }



            return b;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strUserName"></param>
        /// <param name="strPasswor"></param>
        /// <param name="ActiveDirectoryPath"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool AuthenticateADSSL(string strUserName, string strPassword, string ActiveDirectoryPath, int port)
        {

            bool b = false;
            try
            {
                //'  using (var ldapConnection = new LdapConnection(ldap))

                //   using (var ldapConnection = new LdapConnection(ldap))

                //       {


                LdapConnection ldapConnection = new LdapConnection(new LdapDirectoryIdentifier(ActiveDirectoryPath, port));

                NetworkCredential networkCredential = new NetworkCredential(strUserName, strPassword);


                ldapConnection.SessionOptions.ProtocolVersion = 3;
                //  ldapConnection.AuthType = AuthType.Basic;
                ldapConnection.AuthType = AuthType.Negotiate;




                ldapConnection.SessionOptions.VerifyServerCertificate = new VerifyServerCertificateCallback((con, cer) => true);
                ldapConnection.SessionOptions.SecureSocketLayer = true;

                ldapConnection.Bind(networkCredential);
                // }

                // if the bind succeeds, the credentials are OK
                b = true;
            }
            catch (LdapException ldapException)
            {

                // public enum ResultCode
                _Error = ldapException.Message;
                // Unfortunately, invalid credentials fall into this block with a specific error code




                if (ldapException.ErrorCode.Equals(LDAPError_InvalidCredentials))
                {

                    b = false;

                }


            }



            return b;
        }
    }
}
