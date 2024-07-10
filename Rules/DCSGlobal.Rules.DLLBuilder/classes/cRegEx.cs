using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;

namespace DCSGlobal.Rules.DLLBuilder
{
    class cRegEx : IDisposable
    {

        private string _RegexName = string.Empty;

        bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~cRegEx()
        {
            Dispose(false);
        }




        public cRegEx()
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


        public string RegexName
        {
            get
            {
                return _RegexName;
            }
        }

        /// <summary>
        /// returns the patteren and the regex name
        /// </summary>
        /// <param name="RuleLine"></param>
        /// <returns></returns>
        public string Pattern(string RuleLine)
        {




            string pattern = string.Empty;
            string rxName = string.Empty;
            //
            //Match match = regex.Match(RuleLine);

            rxName = ss.ParseDemlimtedString(RuleLine, "=", 1);
            rxName = ss.ParseDemlimtedString(RuleLine, ".", 1);
            pattern = ss.ParseDemlimtedString(RuleLine, "=", 2);

            pattern = pattern.Trim();

            pattern = pattern.Replace("\"", "");

            _RegexName = rxName.Trim();


            return pattern;
        }






        public bool IgnoreCase(string RuleLine)
        {






            bool _ignorecase = false;
            string _ignorecaseText = string.Empty;
            string rxName = string.Empty;
            //
            //Match match = regex.Match(RuleLine);

            rxName = ss.ParseDemlimtedString(RuleLine, "=", 1);
            rxName = ss.ParseDemlimtedString(RuleLine, ".", 1);
            _ignorecaseText = ss.ParseDemlimtedString(RuleLine, "=", 2);


            _ignorecaseText = _ignorecaseText.Trim();
            _ignorecaseText = _ignorecaseText.ToLower();


            if (_ignorecaseText == "true")
            {
                _ignorecase = true;

            }



            _RegexName = rxName.Trim();
                   

            return _ignorecase;
        }






        public string Test(string RuleLine)
        {

            string r = string.Empty;


            string TempRuleline = string.Empty;
            //
            //Match match = regex.Match(RuleLine);


            string[] Ewords = RuleLine.Split(' ');


            foreach (string w in Ewords)
            {

                string _w = w.ToLower();



                if (_w.Contains(".test"))
                {

                    ///objreg.test(lname)
                    string rxTestName = string.Empty;
                    string rxTestTest = string.Empty;
                    string rxTestValue = string.Empty;
                    string rxName = string.Empty;
                    string rxTestMatchName = string.Empty;

                    _RegexName = ss.ParseDemlimtedString(w, ".", 1);
                    rxTestTest = ss.ParseDemlimtedString(w, ".", 2);
                    rxTestValue = rxTestTest;


                    //rxTestMatchName = rxTestName + "Match";

                    rxTestTest = rxTestTest.Replace("Test", "");
                    rxTestTest = rxTestTest.Replace("test", "");
                    rxTestTest = rxTestTest.Replace("(", "");
                    rxTestTest = rxTestTest.Replace(")", "");
                    r = rxTestTest;
                    //// _code.Add("Dim " + rxTestMatchName + " As Match = " + rxTestName + ".Match(_" + rxTestTest + ")");

                    //TempRuleline = TempRuleline + "  " + rxTestMatchName + ".Success ";

                }
                else
                {
                    TempRuleline = TempRuleline + " " + w + " ";

                }


            }

      //      RuleLine = TempRuleline + " ";
       //     RuleLine = TempRuleline.ToLower() + " ";

            return r;
        }
    }



}

