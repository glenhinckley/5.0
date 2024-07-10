using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;

namespace DCSGlobal.Rules.DLLBuilder
{
    class cFindTheReturn : IDisposable 
    {

        
        bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~cFindTheReturn()
        {
            Dispose(false);
        }




        public cFindTheReturn()
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


        private string _ReturnName = string.Empty;

        public string ReturnName
        {
            get
            {
              return   _ReturnName; 
            }
        }

        public int FindTheReturn(List<string> RuleList)
        {
            int r = -1;
            string _l = string.Empty;

           // rr.Found = false;

            //   List<string> RuleReturn = new List<string>();
            //  List<string> RuleReturnNotFound = new List<string>();

            //  RuleReturn = RuleList;
            //  RuleReturnNotFound = RuleList;




            // find the return name if one if not then we need to make a  return var 
            //some times just a retrun in the middle of no where
            foreach (string l in RuleList)
            {
                _l = l.ToLower();

                if (_l.Contains("return"))
                {

                    r = 0;
                    int ReturnsInRulelineCount = 0;
               



                    string searchTerm = "return";
                    //  ReurnCountMoreThanOne = ss.ContainsMoreThan(_l, 1, searchTerm, StringComparison.InvariantCultureIgnoreCase);



                    string[] words = _l.Split(' ');

                    var matchQuery = from word in words
                                     where word.ToLowerInvariant() == searchTerm.ToLowerInvariant()
                                     select word;

                    // Count the matches, which executes the query.  
                    // int wordCount = matchQuery.Count();
                    // if (wordCount == 1)
                    //{

                    foreach (string w in words)
                    {
                        if (w.ToLower() == "return")
                        {
                            ReturnsInRulelineCount++;

                        }

                    }



                    if (ReturnsInRulelineCount == 1)
                    {
                     

                    //    // check for is numeric

                        string TestVar = string.Empty;

                        TestVar = ss.ParseDemlimtedString(l, " ", 2);

                        int n;
                        bool isNumeric = int.TryParse(TestVar, out n);


                        if (!isNumeric)
                        {

                            _ReturnName = ss.ParseDemlimtedString(l, " ", 2);
                            r = 1;

                        }
                    }


                }

            }

            return r;

        }
    }
}
