using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Data;
using System.Net; //Include this namespace 

namespace DCSGlobal.BusinessRules.CoreLibraryII
{
    public class MachineID : IDisposable
    {

        ~MachineID()
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
        /// returns the machine to pass to a db
        /// </summary>
        public string GetExecutingAssembly
        {
            get
            {
                return System.Reflection.Assembly.GetExecutingAssembly().Location;
            }
            set
            {
            }
        }


        public string GetExecutingAssemblyPath
        {
            get
            {
                return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            }
            set
            {
            }
        }


        
        public string GetHostName
        {
            get
            {
                return  Dns.GetHostName(); // Retrive the Name of HOST  
            }
            set
            {
            }
        }



                
        public string GetIPAddress
        {
            get
            {
                 string hostName = Dns.GetHostName();
               
                return   Dns.GetHostByName(hostName).AddressList[0].ToString();   // Retrive the Name of HOST  
            }
            set
            {
            }
        }






        /// <summary>
        /// returns the machine DCS Global ID to pass to a db
        /// </summary>
        public string MachineUID
        {
            get
            {
                return CPUID() + MotherBoardID();
            }
            set
            {
            }
        }

        public string CPUID()
        {

            ManagementObjectCollection mbsList = null;
            ManagementObjectSearcher mbs = new ManagementObjectSearcher("Select * From Win32_processor");
            mbsList = mbs.Get();
            string id = string.Empty;



            foreach (ManagementObject mo in mbsList)
            {
                id = mo["ProcessorID"].ToString();
            }

            return id;
        }


        public string DiscID()
        {

            ManagementObject dsk = new ManagementObject(@"win32_logicaldisk.deviceid=""c:""");
            dsk.Get();
            string id = string.Empty;


            id = dsk["VolumeSerialNumber"].ToString();

            return id;

        }


        public string MotherBoardID()
        {
            ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
            ManagementObjectCollection moc = mos.Get();

            string serial = "";


            foreach (ManagementObject mo in moc)
            {
                serial = (string)mo["SerialNumber"];
            }


            return serial;

        }


    }
}
