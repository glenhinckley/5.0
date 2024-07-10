using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;

namespace DCSGlobal.Rules.DLLBuilder
{
    class cFindUndeclaredDIMS : IDisposable
    {


        bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~cFindUndeclaredDIMS()
        {
            Dispose(false);
        }




        public cFindUndeclaredDIMS()
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

        private List<varField> FindUndeclaredDIMS(List<string> RuleList, List<varField> _varField)
        {
            int i = 0;
            string dim = string.Empty;
            List<varField> r = new List<varField>() ;


            foreach (string l in RuleList)
            {

                string RuleLine = l;
                string _RuleLine = l.ToLower();

                bool isKnown = false;
                string VarParamter = string.Empty;
                string VarParamterWord = _RuleLine;
                string VarParamterWordLower = string.Empty;



                // find vars  	"[=#;^A-Za-z0-9;#]"
                //fname=#patient_first_name#
                Regex regex = new Regex(@"#(.*?)#");
                Match match = regex.Match(_RuleLine);
                VarParamter = match.Value;
                VarParamter = VarParamter.Replace("#", "");


                if (_RuleLine.Contains("=") && match.Success)
                {

                    dim = string.Empty;
                    dim = ss.ParseDemlimtedString(RuleLine, "=", 1);


                    if (!string.IsNullOrEmpty(dim))
                    {

                        foreach (varField v in _varField)
                        {
                            if (dim == v.Name)
                            {
                                isKnown = true;
                            }
                        }



                        // look for trim and cstr and stuff like that

                        //VarName = VarName;
                        if (!isKnown)
                        {
                            // VarParamterWordLower = VarParamterWord.ToLower();
                            varField varfield = new varField();


                            varfield.Name = dim;
                            varfield.isParamter = false;
                            varfield.isAssignment = true;
                            varfield.AssignmentName = VarParamter;

                            r.Add(varfield);

                        }
                    }



                }



            }


            return r;


        }
    }
}
