using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCSGlobal.BusinessRules.CoreLibraryII
{
    public class HighEntropyRamdonNumberGenerator : IDisposable
    {


      private int _Mutiplyer = 0; 

        
        bool _disposed;


        ~HighEntropyRamdonNumberGenerator()
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


        public int Mutiplyer
        {

            set
            {

                _Mutiplyer = value;
            }
        }



        public int Next()
        {
            int r = 0; 

            Random rnd = new Random();

            int seed = rnd.Next();
            r = Convert.ToInt32( Math.Truncate((rnd.NextDouble() * 100) * _Mutiplyer));

            return r;
        }



    }
}
