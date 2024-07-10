using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;

namespace DCSGlobal.Rules.DLLBuilder
{
    class cFindTheSets : IDisposable
    {


        bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~cFindTheSets()
        {
            Dispose(false);
        }




        public cFindTheSets()
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

        public List<varField> FindTheSets(List<string> RuleList, List<varField> _varField)
        {
            List<varField> r = new List<varField>();

            // find the return name if one if not then we need to make a  return var 
            //some times just a retrun in the middle of no where







            foreach (string l in RuleList)
            {


                string SetTest = string.Empty;

                SetTest = l.ToLower();


                if (SetTest.Contains("set"))
                {



                    string Set = string.Empty;
                    string AssigmentType = string.Empty;

                    string ObjectName = string.Empty;
                    string ObjectType = string.Empty;

                    Set = ss.ParseDemlimtedString(l, "=", 1);
                    AssigmentType = ss.ParseDemlimtedString(l, "=", 2);


                    ObjectName = ss.ParseDemlimtedString(Set, " ", 2);
                    ObjectType = ss.ParseDemlimtedString(AssigmentType, " ", 2);

                    bool isKnown = false;

                    foreach (varField v in _varField)
                    {
                        if (ObjectName == v.Name)
                        {
                            isKnown = true;
                        }
                    }



                    if (!isKnown)
                    {
                        varField varfield = new varField();

                        varfield.Name = ObjectName;
                        varfield.Type = ObjectType;


                        if (ObjectType == "RegExp")
                        {
                            varfield.isRegEx = true;

                        }


                        r.Add(varfield);
                    }








                }




            }
            return r;
        }


    }



}

