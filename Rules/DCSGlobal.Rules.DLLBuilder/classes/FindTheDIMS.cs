using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;

namespace DCSGlobal.Rules.DLLBuilder
{
    class cFindTheDIMS: IDisposable 
    {

        
        bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~cFindTheDIMS()
        {
            Dispose(false);
        }




        public cFindTheDIMS()
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

            public List<varField> FindTheDIMS(string RuleID, List<string> RuleList, List<varField> _varField)
            {
                int i = 0;
                string dim = string.Empty;
                List<varField> r = new List<varField>();

                foreach (string l in RuleList)
                {

                    string RuleLine = l;
                    string _RuleLine = l.ToLower();

                    if (_RuleLine.Contains("dim"))
                    {


                        RuleLine = RuleLine.Replace("dim", "");
                        RuleLine = RuleLine.Replace("Dim", "");
                        RuleLine = RuleLine.Replace("DIM", "");

                        int count = RuleLine.Split(',').Length - 1;

                        // Continue to next iteration
                        for (i = 0; i <= count + 1; i++)
                        {
                            dim = string.Empty;
                            dim = ss.ParseDemlimtedString(RuleLine, ",", i);
                            bool isKnown = false;

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

                                    varfield.VBName = "_" + RuleID + "_" + dim;
                                    varfield.Name = dim;
                                    varfield.isParamter = false;


                                    r.Add(varfield);

                                }
                            }
                        }
                    }
                }

                //r = _varField;

                return r;
            }

            ///
        }
    }
