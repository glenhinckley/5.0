using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCSGlobal.BusinessRules.Logging
{
    public class INFO : IDisposable
    {

        public string _NameSpace = "DCSGlobal.BusinessRules.Logging";
        private string _Version = string.Empty;
        public int _Build = 5000;



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

        public string NameSpace()
        {

     
                return _NameSpace;
            
        }

    }
}
