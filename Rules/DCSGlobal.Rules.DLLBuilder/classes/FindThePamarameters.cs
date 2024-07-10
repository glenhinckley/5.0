using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;

namespace DCSGlobal.Rules.DLLBuilder
{
    class cFindThePamarameters : IDisposable
    {


        bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~cFindThePamarameters()
        {
            Dispose(false);
        }




        public cFindThePamarameters()
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

        public List<varField> FindThePamarameters(List<string> RuleList, List<varField> _varField)
        {
            List<varField> r = new List<varField>();

            // find the return name if one if not then we need to make a  return var 
            //some times just a retrun in the middle of no where
            foreach (string l in RuleList)
            {

                string[] words = l.Split(' ');


                foreach (string w in words)
                {
                    if (!vbxk.isKeyWord(w))
                    {
                        if (w.Contains("#"))
                        {

                            bool isKnown = false;
                            string VarParamter = string.Empty;
                            string VarParamterWord = w;
                            string VarParamterWordLower = string.Empty;

                            // find vars  	"[=#;^A-Za-z0-9;#]"
                            //fname=#patient_first_name#
                            Regex regex = new Regex(@"#(.*?)#");
                            Match match = regex.Match(w);
                            VarParamter = match.Value;
                            VarParamter = VarParamter.Replace("#", "");



                            foreach (varField v in _varField)
                            {
                                if (VarParamter == v.Name)
                                {
                                    isKnown = true;
                                }

                                if(!isKnown)
                                {
                                    varField varfield = new varField();

                                    varfield.isParamter = true;
                                    varfield.Name = VarParamter;
                                    varfield.DefaultValue = "string.Empty";
                                    r.Add(varfield);
                                }


                            }




                        }

                    }
                }







            }

            return r;

        }



    }
}
