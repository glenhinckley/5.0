using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using NativeWifi;

namespace ContainerForDLLBuild
{
    class Program
    {
        static void Main(string[] args)
        {
            WlanClient client = new WlanClient();
            foreach (WlanClient.WlanInterface wlanIface in client.Interfaces)
            {
                Wlan.WlanAvailableNetwork[] networks = wlanIface.GetAvailableNetworkList(0);
                foreach (Wlan.WlanAvailableNetwork network in networks)
                {
                    Console.WriteLine("Found network with SSID {0} and Siqnal Quality {1}.", GetStringForSSID(network.dot11Ssid), network.wlanSignalQuality);
                }
            }
            //hello



            System.Console.WriteLine("\n\rPress Any Key to Exit");
            Console.ReadKey();
            System.Console.WriteLine("\n\rShutting down threads and Exiting");

        }

        static string GetStringForSSID(Wlan.Dot11Ssid ssid)
        {
            return Encoding.ASCII.GetString(ssid.SSID, 0, (int)ssid.SSIDLength);
        }







    }
}
