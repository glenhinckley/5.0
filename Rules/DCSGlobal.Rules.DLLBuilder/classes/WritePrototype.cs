using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCSGlobal.Rules.DLLBuilder
{
    class cWritePrototype : IDisposable
    {


        bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~cWritePrototype()
        {
            Dispose(false);
        }




        public cWritePrototype()
        {

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



        public string WritePrototype(string RuleID, string RuleName, string RuleDescrption, List<varField> _varField)
        {
            string r = string.Empty;


            bool FirstFound = false;


            foreach (varField v in _varField)
            {


                if (v.isParamter == true)
                {
                    if (!FirstFound)
                    {
                        r = r + "_" + v.Name;
                        FirstFound = true;
                    }
                    else
                    {
                        r = r + ", _" + v.Name;



                    }
                }

            }



           r = RuleName + "(" + r + ")";


            return r;

        }
    }
}
