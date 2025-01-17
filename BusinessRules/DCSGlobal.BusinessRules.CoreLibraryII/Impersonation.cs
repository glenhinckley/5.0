﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;
using System.Runtime.ConstrainedExecution;
using System.Security;

namespace DCSGlobal.BusinessRules.CoreLibraryII
{

 
    // This sample demonstrates the use of the WindowsIdentity class to impersonate a user.
    // IMPORTANT NOTES:
    // This sample requests the user to enter a password on the console screen.
    // Because the console window does not support methods allowing the password to be masked,
    // it will be visible to anyone viewing the screen.
    // On Windows Vista and later this sample must be run as an administrator. 





    public class Impersonation  : IDisposable 
    {


        bool _disposed;


        ~Impersonation()
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

        
        
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword,
            int dwLogonType, int dwLogonProvider, out SafeTokenHandle phToken);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public extern static bool CloseHandle(IntPtr handle);

        // Test harness.
        // If you incorporate this code into a DLL, be sure to demand FullTrust.
        [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
        public void Main()
        {
            SafeTokenHandle safeTokenHandle;
            try
            {
             
                //// Get the user token for the specified user, domain, and password using the
                //// unmanaged LogonUser method.
                //// The local machine name can be used for the domain name to impersonate a user on this machine.
               Console.Write("Enter the name of the domain on which to log on: ");
               _domainName = Console.ReadLine();

               Console.Write("Enter the login of a user on {0} that you wish to impersonate: ", _domainName);
               _userName = Console.ReadLine();

                Console.Write("Enter the password for {0}: ", _userName);
                _passwd = Console.ReadLine();


                const int LOGON32_PROVIDER_DEFAULT = 0;
                //This parameter causes LogonUser to create a primary token.
                const int LOGON32_LOGON_INTERACTIVE = 2;

                // Call LogonUser to obtain a handle to an access token.
                bool returnValue = LogonUser(_userName, _domainName, _passwd,
                    LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT,
                    out safeTokenHandle);

                Console.WriteLine("LogonUser called.");

                if (false == returnValue)
                {
                    int ret = Marshal.GetLastWin32Error();
                    Console.WriteLine("LogonUser failed with error code : {0}", ret);
                    throw new System.ComponentModel.Win32Exception(ret);
                }
                using (safeTokenHandle)
                {
                    Console.WriteLine("Did LogonUser Succeed? " + (returnValue ? "Yes" : "No"));
                    Console.WriteLine("Value of Windows NT token: " + safeTokenHandle);

                    // Check the identity.
                    Console.WriteLine("Before impersonation: "
                        + WindowsIdentity.GetCurrent().Name);
                    // Use the token handle returned by LogonUser.
                    using (WindowsImpersonationContext impersonatedUser = WindowsIdentity.Impersonate(safeTokenHandle.DangerousGetHandle()))
                    {

                    
                        
                        
                        
                        
                       // do what ever here 
                        
                        
                        
                        
                        
                        
                        
                        
                        // Check the identity.
                        Console.WriteLine("After impersonation: "
                            + WindowsIdentity.GetCurrent().Name);
                    }
                    // Releasing the context object stops the impersonation
                    // Check the identity.
                    Console.WriteLine("After closing the context: " + WindowsIdentity.GetCurrent().Name);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred. " + ex.Message);
            }

        }
    }
    public sealed class SafeTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        private SafeTokenHandle()
            : base(true)
        {
        }

        [DllImport("kernel32.dll")]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr handle);

        protected override bool ReleaseHandle()
        {
            return CloseHandle(handle);
        }
    }
}