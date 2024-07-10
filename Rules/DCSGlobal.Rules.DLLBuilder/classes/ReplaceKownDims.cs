using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;

namespace DCSGlobal.Rules.DLLBuilder
{
    class cReplaceKnownDIMS   : IDisposable
    {

        private string _RegexName = string.Empty;

        bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~cReplaceKnownDIMS()
        {
            Dispose(false);
        }




        public cReplaceKnownDIMS()
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

        private VBSKeyWords vbxk = new VBSKeyWords();
        private StringStuff ss = new StringStuff();


        public string ReplaceKnownDIMS(string RuleLine, List<varField> _varField)
        {

            string r = string.Empty;

            RuleLine = RuleLine.ToLower();

            string[] words = RuleLine.Split(' ');



            foreach (string w in words)
            {


                if (!vbxk.isKeyWord(w))
                {



                    foreach (varField v in _varField)
                    {
                        if (w.Contains(v.Name))
                        {
                            string _w = w;
                            if (_w.Contains("(") && _w.Contains(")"))
                            {
                                string _New = "_" + v.Name;

                                _w = _w.Replace(v.Name, _New);
                            }
                            r = r + _w + " ";
                        }
                    }




                }

                else
                {
                    r = r + w + " ";

                }

            }

            return r;

        }
    }
}
