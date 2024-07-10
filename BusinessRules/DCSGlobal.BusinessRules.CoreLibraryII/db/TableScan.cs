using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Configuration;
using System.Data;
//using DCSGlobal.BusinessRules.CoreLibrary;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using System.Data.SqlClient;
using System.IO;
using DCSGlobal.BusinessRules.CoreLibraryII;

namespace DCSGlobal.BusinessRules.CoreLibraryII.db
{
    public class TableScan : IDisposable
    {

        string _ConnectionString = string.Empty;

        ~TableScan()
        {
            Dispose(false);
        }

        public TableScan()
        {
        //    try
        //    {
        //        using (MachineID g = new MachineID())
        //        {

        //            _AppPath = g.GetExecutingAssemblyPath;
        //            _HostName = g.GetHostName;
        //            _ipAddress = g.GetIPAddress;
        //        }
        //        _PreFixx = _PreFixx + "|" + _HostName + "|" + AppName + "|" + _AppPath + "|" + _ipAddress + "|";
        //        _iSet = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        _iSet = false;

        //    }
        }

        public TableScan(string dbms)
        {
            //    try
            //    {
            //        using (MachineID g = new MachineID())
            //        {

            //            _AppPath = g.GetExecutingAssemblyPath;
            //            _HostName = g.GetHostName;
            //            _ipAddress = g.GetIPAddress;
            //        }
            //        _PreFixx = _PreFixx + "|" + _HostName + "|" + AppName + "|" + _AppPath + "|" + _ipAddress + "|";
            //        _iSet = true;
            //    }
            //    catch (Exception ex)
            //    {
            //        _iSet = false;

            //    }
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


        public string ConnectionString
        {

            set
            {

                _ConnectionString = value;
            }
        }

        public void ReverseEngineerTable(string TableName)
        {















        }


        public void GetTables()
        {















        }

    
    }
}
