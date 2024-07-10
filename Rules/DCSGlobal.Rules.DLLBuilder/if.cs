using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;

namespace DCSGlobal.Rules.DLLBuilder
{
    public class if_then : IDisposable
    {
        private List<string> _words = new List<string> { "LET", "SET", "REM", "Nothing", "if", "then", "for", "else", "end if", "next" };
        private List<string> _if = new List<string> { "if", "then", "else", "end if" };
        private List<string> _list = new List<string>();


        private List<string> _code = new List<string>();

        private string _ConnectionString = string.Empty;

        private string _rule_name = string.Empty;
        private string _rule_id = string.Empty;
        private string _rule_description = string.Empty;
        private string _rule_VB = string.Empty;
        private string _rule_VBS = string.Empty;
        private string _rule_RegEXName = string.Empty;
        private StringStuff ss = new StringStuff();


        private defBuilder d = new defBuilder();

        bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~if_then()
        {
            Dispose(false);
        }




        public if_then()
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


        public string ConnectionString
        {

            set
            {

                _ConnectionString = value;
            }
        }



        public string rule_name
        {

            set
            {

                _rule_name = value;
            }
        }



        public string rule_id
        {

            set
            {

                _rule_id = value;
            }
        }


        public string rule_description
        {

            set
            {

                _rule_description = value;
            }
        }


        public string RuleVB
        {

            get
            {

                return _rule_VB;
            }
        }






    }
}
