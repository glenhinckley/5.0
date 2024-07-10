using System;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;



namespace DCSGlobal.BusinessRules.CoreLibraryII
{
    public class NetworkInfo : IDisposable
    {

        ~NetworkInfo()
        {
            Dispose(false);
        }



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



        /// <summary>
        /// Get host name
        /// </summary>
        /// <returns></returns>
        public static string GetHostName()
        {
            string strHostName = System.Net.Dns.GetHostName();

            return strHostName;
        }



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


        /// <summary>
        /// Get computer LAN address like 192.168.1.3
        /// </summary>
        /// <returns></returns>
        public static string GetLanIP()
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

        public Dictionary<string,string> ShowInterfaceSummary()
        {
            Dictionary<string, string> r = new Dictionary<string, string>();




            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in interfaces)
            {

                r.Add(adapter.Id, Convert.ToString(adapter.Name));
                //Console.WriteLine("Name: {0}", adapter.Name);
                //Console.WriteLine(adapter.Description);
                //Console.WriteLine(String.Empty.PadLeft(adapter.Description.Length, '='));
                //Console.WriteLine("  Interface type .......................... : {0}", adapter.NetworkInterfaceType);
                //Console.WriteLine("  Operational status ...................... : {0}",
                //    adapter.OperationalStatus);
                //string versions = "";

                //// Create a display string for the supported IP versions.
                //if (adapter.Supports(NetworkInterfaceComponent.IPv4))
                //{
                //    versions = "IPv4";
                //}
                //if (adapter.Supports(NetworkInterfaceComponent.IPv6))
                //{
                //    if (versions.Length > 0)
                //    {
                //        versions += " ";
                //    }
                //    versions += "IPv6";
                //}
                //Console.WriteLine("  IP version .............................. : {0}", versions);
                //Console.WriteLine();
            }
            Console.WriteLine();

            return r;
        
        }

    }
}
