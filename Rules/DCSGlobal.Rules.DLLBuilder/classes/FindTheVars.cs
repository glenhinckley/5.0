using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;

namespace DCSGlobal.Rules.DLLBuilder
{
    class cFindTheVars : IDisposable
    {


        bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~cFindTheVars()
        {
            Dispose(false);
        }




        public cFindTheVars()
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

        public List<varField> FindTheVars(string RuleID, List<string> RuleList, List<varField> _varField)
        {
       
            List<varField> r = new List<varField>();

            // find the return name if one if not then we need to make a  return var 
            //some times just a retrun in the middle of no where
            foreach (string l in RuleList)
            {


                if (l.Contains("=") && l.Contains("#"))
                {


                    bool isKnown = false;
                    string VarParamter = string.Empty;
                    //string VarParamterWord = w;
                    // string VarParamterWordLower = string.Empty;

                    // find vars  	"[=#;^A-Za-z0-9;#]"
                    //fname=#patient_first_name#
                    Regex regex = new Regex(@"#(.*?)#");
                    Match match = regex.Match(l);
                    VarParamter = match.Value;
                    VarParamter = VarParamter.Replace("#", "");


                    if (match.Success)
                    {
                        string[] words = l.Split(' ');

                        bool isKeyWord = false;


                        foreach (string w in words)
                        {
                            if (vbxk.isKeyWord(w))
                            {
                                isKeyWord = true;
                            }
                        }




                        if (!isKeyWord)
                        {
                        int c = 0;

                        c = Regex.Matches(l, "=").Count;


                            if (c == 1)
                            {
                                
                                string  var = string.Empty;
                                string ass = string.Empty;

                                var = ss.ParseDemlimtedString(l, "=", 1);
                                ass = ss.ParseDemlimtedString(l, "=", 2);

                                varField varfield = new varField();

                                varfield.VBName = "_" + RuleID + "_" + var;
                                varfield.Name = var;
                                varfield.isParamter = false;
                                varfield.isAssignment = true;
                                varfield.isDim = true;
                                varfield.AssignmentName = VarParamter;

                                r.Add(varfield);
                            
                            }


                        }



                    }







                }

            }

            return r;

        }



    }
}
