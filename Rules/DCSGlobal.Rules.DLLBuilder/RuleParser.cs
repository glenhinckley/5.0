using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;

namespace DCSGlobal.Rules.DLLBuilder
{
    public class RuleParser : IDisposable
    {


        // this holds the rule to list 
        private List<string> _list = new List<string>();


        // this holds the finish rule
        //private List<string> _code = new List<string>();

        private string _ConnectionString = string.Empty;
        //private string _RuleVBPrototype = string.Empty;


        //private List<regex> _Regex = new List<regex>();
        //private List<varField> _varField = new List<varField>();

        private string _rule_name = string.Empty;

        private string _rule_id = string.Empty;
        private string _rule_description = string.Empty;
        private string _rule_VB = string.Empty;
        // private string _rule_VBS = string.Empty;
        // private string _rule_RegEXName = string.Empty;


        //  private VBSKeyWords vbxk = new VBSKeyWords();
        //  private StringStuff ss = new StringStuff();

        //   private List<string> _cmd = new List<string>();

        /// <summary>
        /// 
        /// </summary>
        private List<string> _Tokens = new List<string>();



        //   private string _TempLine = string.Empty;





        private vbRules _vbRule = new vbRules();


        //  private defBuilder d = new defBuilder();
        //private RuleReturn rr = new RuleReturn();


        bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~RuleParser()
        {
            Dispose(false);
        }




        public RuleParser()
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


        public string RuleID
        {

            get
            {
                return _rule_id;
            }
            set
            {
                _rule_id = value;
            }
        }

        public string RuleName
        {
            get
            {
                return _rule_name;
            }
            set
            {
                _rule_name = value;
            }

        }


        public string RuleDescription
        {
            get
            {
                return _rule_description;
            }
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
            set
            {
                _rule_description = value;
            }

        }


        public int ParseRuleVBS(string Rule)
        {

            int r = -1;
            string rule = string.Empty;
            string mline = string.Empty;
            int ruleid = 0;

            //    ruleid = Convert.ToInt32(_rule_id);
            ruleid = ruleid * -1;


            using (ParseRuleToList p = new ParseRuleToList())
            {

                r = p.byString(Rule);
                if (r == 0)
                {
                    _list = p.RuleList;

                }

            }






            using (Lexer cfu = new Lexer())
            {
                cfu.Lex(_list);
                _Tokens = cfu.Tokens;
            }

            ///Code(_list);






            using (Parser cfu = new Parser())
            {
                cfu.Parse(_Tokens);
                //_Tokens = cfu.Tokens;
            }




            //   LexCode();




            return r;

        }



























    }



}

