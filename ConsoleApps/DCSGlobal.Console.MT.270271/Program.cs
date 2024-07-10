using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;
using System;
using System.Configuration;
using System.Data;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.IO.Pipes;


using System.Runtime.InteropServices;   //GuidAttribute
using System.Reflection;                //Assembly
using System.Security.AccessControl;    //MutexAccessRule
using System.Security.Principal;
using System.Net;        //SecurityIdentifier




namespace DCSGlobal.Console.MT._270271
{

    class Program
    {



        private static MutexAccessRule allowEveryoneRule = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow);
        private static MutexSecurity securitySettings = new MutexSecurity();

        public static bool hasHandle = false;
        private static Mutex mutex;

        private static string AppName = string.Empty;






        public static bool IsLocalIpAddress(string host)
        {
            try
            { // get host IP addresses
                IPAddress[] hostIPs = Dns.GetHostAddresses(host);
                // get local IP addresses
                IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

                // test if any host IP equals to any local IP or to localhost
                foreach (IPAddress hostIP in hostIPs)
                {
                    // is localhost
                    if (IPAddress.IsLoopback(hostIP)) return true;
                    // is local address
                    foreach (IPAddress localIP in localIPs)
                    {
                        if (hostIP.Equals(localIP)) return true;
                    }
                }
            }
            catch { }
            return false;
        }


        /// <summary>
        /// Get computer LAN address like 192.168.1.3
        /// </summary>
        /// <returns></returns>
        private static string GetComputer_LanIP()
        {
            string strHostName = System.Net.Dns.GetHostName();

            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

            foreach (IPAddress ipAddress in ipEntry.AddressList)
            {
                if (ipAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    return ipAddress.ToString();
                }
            }

            return "-";
        }



        static void Main(string[] args)
        {
          
            // get the app name from config

            AppName = "me";
            
            
            SingleGlobalInstance(3000);

            if (!hasHandle)
            {
                Environment.Exit(0);

            }

            // begin do work


            string m = GetComputer_LanIP();



            string localComputerName = Dns.GetHostName();
           
 



            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());


            IsLocalIpAddress("localhost");        // true (loopback name)
            IsLocalIpAddress("127.0.0.1");        // true (loopback IP)
            IsLocalIpAddress("MyNotebook");       // true (my computer name)
            IsLocalIpAddress("192.168.0.1");      // true (my IP)
            IsLocalIpAddress("NonExistingName");  // false (non existing computer name)
            IsLocalIpAddress("99.0.0.1");         // false (non existing IP in my net)




            //end do work

            // realease the mutex and dsipose of it only thing left is to exit
            if (mutex != null)
            {
                if (hasHandle)
                    mutex.ReleaseMutex();
                mutex.Dispose();
            }
            Environment.Exit(0);



        }










        static void InitMutex()
        {
            string appGuid = ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false).GetValue(0)).Value.ToString();
            string mutexId = string.Format("Global\\{{{0}}}", AppName + appGuid);

            mutex = new Mutex(true, mutexId);

            var allowEveryoneRule = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow);
            var securitySettings = new MutexSecurity();
            securitySettings.AddAccessRule(allowEveryoneRule);
            mutex.SetAccessControl(securitySettings);
        }


        static void SingleGlobalInstance(int timeOut)
        {
            InitMutex();
            try
            {
                if (timeOut < 0)
                    hasHandle = mutex.WaitOne(Timeout.Infinite, false);
                else
                    hasHandle = mutex.WaitOne(timeOut, false);

                if (hasHandle == false)
                {
                    Environment.Exit(0);
                }
                //  throw new TimeoutException("Timeout waiting for exclusive access on SingleInstance");
            }
            catch (AbandonedMutexException)
            {
                hasHandle = true;
            }
        }







    }

}
