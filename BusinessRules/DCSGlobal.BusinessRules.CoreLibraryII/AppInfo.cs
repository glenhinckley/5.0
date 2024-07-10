using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DCSGlobal.BusinessRules.CoreLibraryII
{
    public class AppInfo   : IDisposable
    {

      /// <summary>
      /// 
      /// </summary>
// private [INSTANCE_NAME



        ~AppInfo()
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


        public int Get()
        {
            int i = 0;


            return i;

        }


        //    System.Reflection.Assembly.GetExecutingAssembly().Location1

        //Combine that with System.IO.Path.GetDirectoryName if all you want is the directory.

    }
}
