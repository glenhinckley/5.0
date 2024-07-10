using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using Microsoft.Win32;
using System.Security.Permissions;

using System.DirectoryServices.ActiveDirectory;


using System.Collections;

namespace DCSGlobal.BusinessRules.CoreLibraryII
{
    public class ldap
    {

        //int SCRIPT = 0x0001;
        //int ACCOUNTDISABLE = 0x0002;
        //int HOMEDIR_REQUIRED = 0x0008;
        //int LOCKOUT = 0x0010;
        //int PASSWD_NOTREQD = 0x0020;
        //int PASSWD_CANT_CHANGE = 0x0040;
        //int ENCRYPTED_TEXT_PWD_ALLOWED = 0x0080;
        //int TEMP_DUPLICATE_ACCOUNT = 0x0100;
        //int NORMAL_ACCOUNT = 0x0200;
        //int INTERDOMAIN_TRUST_ACCOUNT = 0x0800;
        //int WORKSTATION_TRUST_ACCOUNT = 0x1000;
        //int SERVER_TRUST_ACCOUNT = 0x2000;
        //int DONT_EXPIRE_PASSWORD = 0x10000;
        //int MNS_LOGON_ACCOUNT = 0x20000;
        //int SMARTCARD_REQUIRED = 0x40000;
        //int TRUSTED_FOR_DELEGATION = 0x80000;
        //int NOT_DELEGATED = 0x100000;
        //int USE_DES_KEY_ONLY = 0x200000;
        //int DONT_REQ_PREAUTH = 0x400000;
        //int PASSWORD_EXPIRED = 0x800000;
        //int TRUSTED_TO_AUTH_FOR_DELEGATION = 0x1000000;

        //Add this to the create account method

        //int val = (int)newUser.Properties["userAccountControl"].Value; 
        //     //newUser is DirectoryEntry object

        //newUser.Properties["userAccountControl"].Value = val | 0x80000; 
        //    //ADS_UF_TRUSTED_FOR_DELEGATION
        //        public enum objectClass
        //        {
        //            user, group, computer
        //        }
        //        public enum returnType
        //        {
        //            distinguishedName, ObjectGUID
        //        }

        //        //Example

        ///// <span class="code-SummaryComment"><summary></span>
        ///// Gets or sets a value indicating if the user account is locked out
        ///// <span class="code-SummaryComment"></summary></span>
        //public bool IsLocked
        //{
        //    get { return Convert.ToBoolean(dEntry.InvokeGet("IsAccountLocked")); }
        //    set { dEntry.InvokeSet("IsAccountLocked", value); }
        //}

        public static void Rename(string objectDn, string newName)
        {
            DirectoryEntry child = new DirectoryEntry("LDAP://" + objectDn);
            child.Rename("CN=" + newName);
        }

        public void ResetPassword(string userDn, string password)
        {
            DirectoryEntry uEntry = new DirectoryEntry(userDn);
            uEntry.Invoke("SetPassword", new object[] { password });
            uEntry.Properties["LockOutTime"].Value = 0; //unlock account

            uEntry.Close();
        }


        public void Unlock(string userDn)
        {
            try
            {
                DirectoryEntry uEntry = new DirectoryEntry(userDn);
                uEntry.Properties["LockOutTime"].Value = 0; //unlock account

                uEntry.CommitChanges(); //may not be needed but adding it anyways

                uEntry.Close();
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException E)
            {
                //DoSomethingWith --> E.Message.ToString();

            }
        }




        public void Disable(string userDn)
        {
            try
            {
                DirectoryEntry user = new DirectoryEntry(userDn);
                int val = (int)user.Properties["userAccountControl"].Value;
                user.Properties["userAccountControl"].Value = val | 0x2;
                //ADS_UF_ACCOUNTDISABLE;

                user.CommitChanges();
                user.Close();
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException E)
            {
                //DoSomethingWith --> E.Message.ToString();

            }
        }




        public void Enable(string userDn)
        {
            try
            {
                DirectoryEntry user = new DirectoryEntry(userDn);
                int val = (int)user.Properties["userAccountControl"].Value;
                user.Properties["userAccountControl"].Value = val & ~0x2;
                //ADS_UF_NORMAL_ACCOUNT;

                user.CommitChanges();
                user.Close();
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException E)
            {
                //DoSomethingWith --> E.Message.ToString();

            }
        }



        public ArrayList AttributeValuesMultiString(string attributeName, string objectDn, ArrayList valuesCollection, bool recursive)
        {
            DirectoryEntry ent = new DirectoryEntry(objectDn);
            PropertyValueCollection ValueCollection = ent.Properties[attributeName];
            IEnumerator en = ValueCollection.GetEnumerator();

            while (en.MoveNext())
            {
                if (en.Current != null)
                {
                    if (!valuesCollection.Contains(en.Current.ToString()))
                    {
                        valuesCollection.Add(en.Current.ToString());
                        if (recursive)
                        {
                            AttributeValuesMultiString(attributeName, "LDAP://" +
                            en.Current.ToString(), valuesCollection, true);
                        }
                    }
                }
            }
            ent.Close();
            ent.Dispose();
            return valuesCollection;
        }

        public string CreateUserAccount(string ldapPath, string userName, string userPassword)
        {
            string oGUID = string.Empty;
            try
            {

                string connectionPrefix = "LDAP://" + ldapPath;
                DirectoryEntry dirEntry = new DirectoryEntry(connectionPrefix);
                DirectoryEntry newUser = dirEntry.Children.Add("CN=" + userName, "user");
                newUser.Properties["samAccountName"].Value = userName;
                newUser.CommitChanges();
                oGUID = newUser.Guid.ToString();

                newUser.Invoke("SetPassword", new object[] { userPassword });
                newUser.CommitChanges();
                dirEntry.Close();
                newUser.Close();
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException E)
            {
                //DoSomethingwith --> E.Message.ToString();

            }
            return oGUID;
        }

        public ArrayList Groups(string userDn, bool recursive)
        {
            ArrayList groupMemberships = new ArrayList();
            return AttributeValuesMultiString("memberOf", userDn, groupMemberships, recursive);
        }


        public ArrayList Groups()
        {
            ArrayList groups = new ArrayList();
            foreach (System.Security.Principal.IdentityReference group in System.Web.HttpContext.Current.Request.LogonUserIdentity.Groups)
            {
                groups.Add(group.Translate(typeof(System.Security.Principal.NTAccount)).ToString());
            }
            return groups;
        }





        public void AddToGroup(string userDn, string groupDn)
        {
            try
            {
                DirectoryEntry dirEntry = new DirectoryEntry("LDAP://" + groupDn);
                dirEntry.Properties["member"].Add(userDn);
                dirEntry.CommitChanges();
                dirEntry.Close();
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException E)
            {
                //doSomething with E.Message.ToString();

            }
        }



        private bool Authenticate(string userName, string password, string domain)
        {
            bool authentic = false;
            try
            {
                DirectoryEntry entry = new DirectoryEntry("LDAP://" + domain, userName, password);
                object nativeObject = entry.NativeObject;
                authentic = true;
            }
            catch (DirectoryServicesCOMException) { }
            return authentic;
        }



        //        public void Create(string ouPath, string name)
        //{
        //    if (!DirectoryEntry.Exists("LDAP://CN=" + name + "," + ouPath))
        //    {
        //        try
        //        {
        //            DirectoryEntry entry = new DirectoryEntry("LDAP://" + ouPath);
        //            DirectoryEntry group = entry.Children.Add("CN=" + name, "group");
        //            group.Properties["sAmAccountName"].Value = name;
        //            group.CommitChanges();
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e.Message.ToString());
        //        }
        //    }
        //    else { Console.WriteLine(path + " already exists"); }
        //}



        //private void init()
        //{
        //    CreateShareEntry("OU=HOME,dc=baileysoft,dc=com",
        //        "Music", @"\\192.168.2.1\Music", "mp3 Server Share");
        //    Console.ReadLine();
        //}

        //Actual Method

        public void CreateShareEntry(string ldapPath,
            string shareName, string shareUncPath, string shareDescription)
        {
            string oGUID = string.Empty;
            string connectionPrefix = "LDAP://" + ldapPath;
            DirectoryEntry directoryObject = new DirectoryEntry(connectionPrefix);
            DirectoryEntry networkShare = directoryObject.Children.Add("CN=" +
                shareName, "volume");
            networkShare.Properties["uNCName"].Value = shareUncPath;
            networkShare.Properties["Description"].Value = shareDescription;
            networkShare.CommitChanges();

            directoryObject.Close();
            networkShare.Close();
        }


        public static string ConvertGuidToDn(string GUID)
        {
            DirectoryEntry ent = new DirectoryEntry();
            String ADGuid = ent.NativeGuid;
            DirectoryEntry x = new DirectoryEntry("LDAP://{GUID=" + ADGuid + ">");
            //change the { to <>

            return x.Path.Remove(0, 7); //remove the LDAP prefix from the path

        }

        public static string ConvertGuidToOctectString(string objectGuid)
        {
            System.Guid guid = new Guid(objectGuid);
            byte[] byteGuid = guid.ToByteArray();
            string queryGuid = "";
            foreach (byte b in byteGuid)
            {
                queryGuid += @"\" + b.ToString("x2");
            }
            return queryGuid;
        }

        public string ConvertDNtoGUID(string objectDN)
        {
            //Removed logic to check existence first

            DirectoryEntry directoryObject = new DirectoryEntry(objectDN);
            return directoryObject.Guid.ToString();
        }

        //public string GetObjectDistinguishedName(objectClass objectCls,
        //    returnType returnValue, string objectName, string LdapDomain)
        //{
        //    string distinguishedName = string.Empty;
        //    string connectionPrefix = "LDAP://" + LdapDomain;
        //    DirectoryEntry entry = new DirectoryEntry(connectionPrefix);
        //    DirectorySearcher mySearcher = new DirectorySearcher(entry);

        //    switch (objectCls)
        //    {
        //        case objectClass.user:
        //            mySearcher.Filter = "(&(objectClass=user)
        //        (|(cn=" + objectName + ")(sAMAccountName=" + objectName + ")))";
        //            break;
        //        case objectClass.group:
        //            mySearcher.Filter = "(&(objectClass=group)
        //        (|(cn=" + objectName + ")(dn=" + objectName + ")))";
        //            break;
        //        case objectClass.computer:
        //            mySearcher.Filter = "(&(objectClass=computer)
        //            (|(cn=" + objectName + ")(dn=" + objectName + ")))";
        //            break;
        //    }
        //    SearchResult result = mySearcher.FindOne();

        //    if (result == null)
        //    {
        //        throw new NullReferenceException
        //        ("unable to locate the distinguishedName for the object " +
        //        objectName + " in the " + LdapDomain + " domain");
        //    }
        //    DirectoryEntry directoryObject = result.GetDirectoryEntry();
        //    if (returnValue.Equals(returnType.distinguishedName))
        //    {
        //        distinguishedName = "LDAP://" + directoryObject.Properties
        //            ["distinguishedName"].Value;
        //    }
        //    if (returnValue.Equals(returnType.ObjectGUID))
        //    {
        //        distinguishedName = directoryObject.Guid.ToString();
        //    }
        //    entry.Close();
        //    entry.Dispose();
        //    mySearcher.Dispose();
        //    return distinguishedName;
        //}

        //   public static ArrayList GetUsedAttributes(string objectDn)
        //   {
        //       DirectoryEntry objRootDSE = new DirectoryEntry("LDAP://" + objectDn);
        //       ArrayList props = new ArrayList();

        //       foreach (string strAttrName in objRootDSE.Properties.PropertyNames)
        //       {
        //           props.Add(strAttrName);
        //       }
        //       return props;
        //   }
        //   public string AttributeValuesSingleString
        //(string attributeName, string objectDn)
        //   {
        //       string strValue;
        //       DirectoryEntry ent = new DirectoryEntry(objectDn);
        //       strValue = ent.Properties[attributeName].Value.ToString();
        //       ent.Close();
        //       ent.Dispose();
        //       return strValue;
        //   }

        //  public ArrayList AttributeValuesMultiString(string attributeName,
        //string objectDn, ArrayList valuesCollection, bool recursive)
        //  {
        //      DirectoryEntry ent = new DirectoryEntry(objectDn);
        //      PropertyValueCollection ValueCollection = ent.Properties[attributeName];
        //      IEnumerator en = ValueCollection.GetEnumerator();

        //      while (en.MoveNext())
        //      {
        //          if (en.Current != null)
        //          {
        //              if (!valuesCollection.Contains(en.Current.ToString()))
        //              {
        //                  valuesCollection.Add(en.Current.ToString());
        //                  if (recursive)
        //                  {
        //                      AttributeValuesMultiString(attributeName, "LDAP://" +
        //                      en.Current.ToString(), valuesCollection, true);
        //                  }
        //              }
        //          }
        //      }
        //      ent.Close();
        //      ent.Dispose();
        //      return valuesCollection;
        //  }



        public void Move(string objectLocation, string newLocation)
        {
            //For brevity, removed existence checks

            DirectoryEntry eLocation = new DirectoryEntry("LDAP://" + objectLocation);
            DirectoryEntry nLocation = new DirectoryEntry("LDAP://" + newLocation);
            string newName = eLocation.Name;
            eLocation.MoveTo(nLocation, newName);
            nLocation.Close();
            eLocation.Close();
        }


        public static bool Exists(string objectPath)
        {
            bool found = false;
            if (DirectoryEntry.Exists("LDAP://" + objectPath))
            {
                found = true;
            }
            return found;
        }


        public ArrayList EnumerateOU(string OuDn)
        {
            ArrayList alObjects = new ArrayList();
            try
            {
                DirectoryEntry directoryObject = new DirectoryEntry("LDAP://" + OuDn);
                foreach (DirectoryEntry child in directoryObject.Children)
                {
                    string childPath = child.Path.ToString();
                    alObjects.Add(childPath.Remove(0, 7));
                    //remove the LDAP prefix from the path

                    child.Close();
                    child.Dispose();
                }
                directoryObject.Close();
                directoryObject.Dispose();
            }
            catch (DirectoryServicesCOMException e)
            {
                Console.WriteLine("An Error Occurred: " + e.Message.ToString());
            }
            return alObjects;
        }


        public void DeleteTrust(string sourceForestName, string targetForestName)
        {
            Forest sourceForest = Forest.GetForest(new DirectoryContext(DirectoryContextType.Forest, sourceForestName));

            Forest targetForest = Forest.GetForest(new DirectoryContext(DirectoryContextType.Forest, targetForestName));

            // delete forest trust

            sourceForest.DeleteTrustRelationship(targetForest);
        }


        public void CreateTrust(string sourceForestName, string targetForestName)
        {
            Forest sourceForest = Forest.GetForest(new DirectoryContext(DirectoryContextType.Forest, sourceForestName));

            Forest targetForest = Forest.GetForest(new DirectoryContext(DirectoryContextType.Forest, targetForestName));

            // create an inbound forest trust

            sourceForest.CreateTrustRelationship(targetForest, TrustDirection.Outbound);
        }


        public static ArrayList EnumerateDomainControllers()
        {
            ArrayList alDcs = new ArrayList();
            Domain domain = Domain.GetCurrentDomain();
            foreach (DomainController dc in domain.DomainControllers)
            {
                alDcs.Add(dc.Name);
            }
            return alDcs;
        }


        //public static ArrayList EnumerateDomains()
        //{
        //    ArrayList alGCs = new ArrayList();
        //    Forest currentForest = Forest.GetCurrentForest();
        //    foreach (GlobalCatalog gc in currentForest.GlobalCatalogs)
        //    {
        //        alGCs.Add(gc.Name);
        //    }
        //    return alGCs;
        //}


        //public static ArrayList EnumerateDomains()
        //{
        //    ArrayList alDomains = new ArrayList();
        //    Forest currentForest = Forest.GetCurrentForest();
        //    DomainCollection myDomains = currentForest.Domains;

        //    foreach (Domain objDomain in myDomains)
        //    {
        //        alDomains.Add(objDomain.Name);
        //    }
        //    return alDomains;
        //}



        public static void Rename(string server, string userName, string password, string objectDn, string newName)
        {
            DirectoryEntry child = new DirectoryEntry("LDAP://" + server + "/" + objectDn, userName, password);
            child.Rename("CN=" + newName);
        }


        public static string FriendlyDomainToLdapDomain(string friendlyDomainName)
        {
            string ldapPath = null;
            try
            {
                DirectoryContext objContext = new DirectoryContext(DirectoryContextType.Domain, friendlyDomainName);
                Domain objDomain = Domain.GetDomain(objContext);
                ldapPath = objDomain.Name;
            }
            catch (DirectoryServicesCOMException e)
            {
                ldapPath = e.Message.ToString();
            }
            return ldapPath;
        }










        /// <summary>
        /// Came from suba console
        /// </summary>
        /// <param name="ldap"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="port"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool AuthenticateADSSL(string ldap, string username, string password, int port, ref string error)
        {
            bool b = false;
            string _Error = "";
            int LDAPError_InvalidCredentials = 0x31;

            try
            {
                LdapConnection ldapConnection = new LdapConnection(new LdapDirectoryIdentifier(ldap, port)); //636
                var networkCredential = new NetworkCredential(username, password);

                if (port == 636)
                {
                    ldapConnection.SessionOptions.ProtocolVersion = 3;
                    //  ldapConnection.AuthType = AuthType.Basic;
                    ldapConnection.AuthType = AuthType.Negotiate;
                    ldapConnection.SessionOptions.VerifyServerCertificate = new VerifyServerCertificateCallback((con, cer) => true);
                    ldapConnection.SessionOptions.SecureSocketLayer = true;
                }
                //ldapConnection.SessionOptions.ProtocolVersion = 3;
                ////  ldapConnection.AuthType = AuthType.Basic;
                //ldapConnection.AuthType = AuthType.Negotiate;

                //ldapConnection.SessionOptions.VerifyServerCertificate = new VerifyServerCertificateCallback((con, cer) => true);
                //ldapConnection.SessionOptions.SecureSocketLayer = true;

                ldapConnection.Bind(networkCredential);

                b = true; // if the bind succeeds, the credentials are OK
                error = "";
            }
            catch (LdapException ldapException)
            {
                _Error = ldapException.Message;
                error = _Error;
                // Unfortunately, invalid credentials fall into this block with a specific error code
                Console.WriteLine("FAILED6-AuthenticateADSSL-port:" + port.ToString() + ":" + ldapException.Message + "  " + ldapException.StackTrace);
                if (ldapException.ErrorCode.Equals(LDAPError_InvalidCredentials))
                {
                    b = false;
                }
            }

            return b;
        }
    }
}