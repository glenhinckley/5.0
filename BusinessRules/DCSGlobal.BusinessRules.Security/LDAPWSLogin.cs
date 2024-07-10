using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DCSGlobal.BusinessRules.CoreLibrary;
using DCSGlobal.BusinessRules.CoreLibraryII;
using DCSGlobal.BusinessRules.CoreLibraryII.Lists;

using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;


namespace DCSGlobal.BusinessRules.Security
{
    public class LDAPWSLogin : IDisposable
    {
        StringStuff ss = new StringStuff();

        bool _disposed;
        private string _EndPoint = string.Empty;
        private int _LDAPReturnCode = 0;
        private string _LDAPReturnString = string.Empty;

        ~LDAPWSLogin()
        {
            Dispose(false);
        }



        public string EndPoint
        {

            set
            {
                _EndPoint = value;
            }
        }


        public int LDAPReturnCode
        {

            get
            {
                return  _LDAPReturnCode;
            }
        }



        public string LDAPReturnString
        {

            get
            {
                return _LDAPReturnString;
            }
        }


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





        public bool Login(string UserName, string Passwd, string Client)
        {

            bool r = false;

            try
            {
                string LDAPReturn = string.Empty;
                string _strUserName = string.Empty;
                string _strPassword = string.Empty;
                string _ActiveDirectoryPath = string.Empty;
                string _ClientName = string.Empty;





                using (DBUtility dbu = new DBUtility())
                {

                    _strUserName = dbu.Encrypt(UserName);
                    _strPassword = dbu.Encrypt(Passwd);
                    _ClientName = dbu.Encrypt(Client);

                }




                using (LDAP.LDAPSoapClient au = new LDAP.LDAPSoapClient())
                {
                    au.Endpoint.Address = new System.ServiceModel.EndpointAddress(_EndPoint);
                    //AuthenticateAD(string strUserName, string strPassword, string ActiveDirectoryPath, string port)
                    LDAPReturn = au.AuthenticateAD(_strUserName, _strPassword, _ClientName);


                    _LDAPReturnCode = Convert.ToInt32(ss.ParseDemlimtedString(LDAPReturn, "^", 1));
                    _LDAPReturnString = ss.ParseDemlimtedString(LDAPReturn, "^", 2);

                    if (_LDAPReturnCode == 1)
                    {
                        r = true;
                    }


                }
            }
            catch (Exception ex)
            {

                r = false;
                _LDAPReturnCode = -100;
                _LDAPReturnString = ex.Message;

            }





            return r;

        }

    }
}
