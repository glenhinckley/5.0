﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCSGlobal.BusinessRules.CoreLibraryII
{
    public class INFO : IDisposable
    {

        private string _NameSpace = "DCSGlobal.BusinessRules.CoreLibraryII";
        private string _Version = string.Empty;
        private int _Build = 5000;
   


        bool _disposed;



        ~INFO()
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



    }
}
