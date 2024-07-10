using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;
using System.Runtime.ConstrainedExecution;
using System.Security;
using System.IO;

namespace DCSGlobal.BusinessRules.CoreLibraryII
{
    public class SecureFileIO : IDisposable
    {


        bool _disposed;


        ~SecureFileIO()
        {
            Dispose(false);
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

        private static string _userName = string.Empty;
        private static string _domainName = string.Empty;
        private static string _passwd = string.Empty;





        /// <summary>
        /// User Name
        /// </summary>
        public string UserName
        {

            set
            {

                _userName = value;

            }
        }



        /// <summary>
        /// Domain Name
        /// </summary>
        public string DomainName
        {

            set
            {

                _domainName = value;

            }
        }

        /// <summary>
        ///Password
        /// </summary>
        public string Password
        {

            set
            {

                _passwd = value;

            }
        }


        /// <summary>
        ///   CopyWithImpersonation
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Destination"></param>
        /// <returns></returns>
        public int CopyWithImpersonation(string Source, string Destination)
        {

            int r = -1;


            AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
            WindowsIdentity idnt = new WindowsIdentity(_userName, _passwd);
            WindowsImpersonationContext context = idnt.Impersonate();

            File.Copy(Source, Destination, true);

            context.Undo();


            return r;
        }

        /// <summary>
        ///  MoveWithImpersonation
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Destination"></param>
        /// <returns></returns>
        public int MoveWithImpersonation(string Source, string Destination)
        {

            int r = -1;

            AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);

            WindowsIdentity idnt = new WindowsIdentity(_userName, _passwd);

            WindowsImpersonationContext context = idnt.Impersonate();

            File.Move(Source, Destination);

            context.Undo();


            return r;
        }


        /// <summary>
        /// DeleteWithImpersonation
        /// </summary>
        /// <param name="Source"></param>
        /// <returns></returns>
        public int DeleteWithImpersonation(string Source)
        {

            int r = -1;

            AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);

            WindowsIdentity idnt = new WindowsIdentity(_userName, _passwd);

            WindowsImpersonationContext context = idnt.Impersonate();

            File.Delete(Source);

            context.Undo();


            return r;
        }



        /// <summary>
        ///   TouchWithImpersonation
        /// </summary>
        /// <param name="ThingToTouch"></param>
        /// <returns></returns>
        private int TouchWithImpersonation(string ThingToTouch)
        {

            int r = -1;
            AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);

            WindowsIdentity idnt = new WindowsIdentity(_userName, _passwd);

            WindowsImpersonationContext context = idnt.Impersonate();

            File.Copy(@"\\192.100.0.2\temp", @"D:\WorkDir\TempDir\test.txt", true);

            context.Undo();



            return r;


        }

    }
}
