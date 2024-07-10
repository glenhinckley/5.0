using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;

namespace DCSGlobal.Rules.DLLBuilder
{
    class cWriteRule : IDisposable
    {

        //private List<string> _words = new List<string> { "LET", "SET", "REM", "Nothing", "if", "then", "for", "else", "end if", "next" };
        // private List<string> _if = new List<string> { "if", "then", "else", "end if" };


        // this holds the rule to list 
        private List<string> _list = new List<string>();


        // this holds the finish rule
        private List<string> _code = new List<string>();

        private string _ConnectionString = string.Empty;
        private string _rule_VB = string.Empty;


        // private List<regex> _Regex = new List<regex>();
        // private List<varField> _varField = new List<varField>();







        private StringStuff ss = new StringStuff();

        private string _TempLine = string.Empty;

        private VBSKeyWords vbxk = new VBSKeyWords();


        //  private defBuilder d = new defBuilder();
        private RuleReturn rr = new RuleReturn();

        bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~cWriteRule()
        {
            Dispose(false);
        }




        public cWriteRule()
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


        public string RuleString
        {
            get
            {
                return _rule_VB;
            }
        }

        public int WriteRule(string _rule_id, string _rule_name, string _rule_description, List<string> RuleList, List<varField> varField)
        {
            int r = -1;

            List<varField> _varField = varField;


            _rule_name = _rule_name.Trim();

            _rule_name = _rule_name.Replace(" ", "_");

            try
            {

                _rule_VB = string.Empty;

                // BulidRuleParamter
                _rule_VB = _rule_VB + "Public Function vb_" + _rule_name;
                _rule_VB = _rule_VB + "(";
                bool FirstFound = false;

                foreach (varField v in _varField)
                {

                    if (v.isParamter)
                    {
                        if (!FirstFound)
                        {
                            _rule_VB = _rule_VB + "ByVal " + v.Name + " as " + v.Type + " ";
                            FirstFound = true;
                        }
                        else
                        {
                            _rule_VB = _rule_VB + " , ByVal " + v.Name + " as " + v.Type + " ";
                        }
                    }
                }


                //_rule_VB = _rule_VB + d.BuildRuleParamter();

                _rule_VB = _rule_VB + ") As Integer\r\n";













                _rule_VB = _rule_VB + "\r\n'rule_id = " + _rule_id;
                _rule_VB = _rule_VB + "\r\n'rule_name = " + _rule_name;
                _rule_VB = _rule_VB + "\r\n'rule_description = " + _rule_description.Replace("\"", "");
                _rule_VB = _rule_VB + "\r\n";

                _rule_VB = _rule_VB + "\r\n";


                foreach (varField v in _varField)
                {
                    if (v.isReturn)
                    {
                        _rule_VB = _rule_VB + "\r\n'Rule Return";
                        _rule_VB = _rule_VB + "\r\nDim " + v.Name + " as Integer = -" + _rule_id;
                    }
                }





                _rule_VB = _rule_VB + "\r\n";
                _rule_VB = _rule_VB + "\r\n";

                // BulidRuleDecs
                _rule_VB = _rule_VB + "\r\n'BEGIN decs***************************************************";
                _rule_VB = _rule_VB + "\r\n'*************************************************************";
                _rule_VB = _rule_VB + "\r\n";


                foreach (varField v in _varField)
                {
                    if (v.isDim)
                    {
                        _rule_VB = _rule_VB + "\r\nDim " + v.Name + " as " + v.Type + " ";
                    }
                }

                _rule_VB = _rule_VB + "\r\n";
                _rule_VB = _rule_VB + "\r\n'*************************************************************";
                _rule_VB = _rule_VB + "\r\n'END decs*****************************************************";



                _rule_VB = _rule_VB + "\r\n";
                _rule_VB = _rule_VB + "\r\n";







                // Assin the Paramter to the vars
                _rule_VB = _rule_VB + "\r\n'BEGIN Assignment*********************************************";
                _rule_VB = _rule_VB + "\r\n'*************************************************************";
                _rule_VB = _rule_VB + "\r\n";


                foreach (varField v in _varField)
                {
                    if (v.isDim)
                    {
                        _rule_VB = _rule_VB + "\r\n" + v.Name + " = " + v.AssignmentName;
                    }
                }

                _rule_VB = _rule_VB + "\r\n";
                _rule_VB = _rule_VB + "\r\n'*************************************************************";
                _rule_VB = _rule_VB + "\r\n'END Assignment***********************************************";

                _rule_VB = _rule_VB + "\r\n";
                _rule_VB = _rule_VB + "\r\n";












                // build Reg ex
                _rule_VB = _rule_VB + "\r\n'BEGIN REGEX**************************************************";
                _rule_VB = _rule_VB + "\r\n'*************************************************************";
                _rule_VB = _rule_VB + "\r\n";

                foreach (varField v in _varField)
                {
                    if (v.isRegEx)
                    {
                       
                         
                        
                        _rule_VB = _rule_VB + "\r\nDim " + v.Name + " as Regex = New Regex(\"" +  v.Pattern  +  "\")";

                        if (v.IgnoreCase)
                        {
                            _rule_VB = _rule_VB + "\r\n" + v.Name + ".Options.IgnoreCase  = True";
                        }
                        else
                        {
                            _rule_VB = _rule_VB + "\r\n" + v.Name + ".Options.IgnoreCase  = False";
                        }
                        
                        
                        
                        _rule_VB = _rule_VB + "\r\nDim match_" + v.Name + " as Match = New  " + v.Name + ".Match(" +  v.Test   +  ")";



                    }
                }

                _rule_VB = _rule_VB + "\r\n"; 
                _rule_VB = _rule_VB + "\r\n'*************************************************************";
                _rule_VB = _rule_VB + "\r\n'END REGEX****************************************************";
    

                _rule_VB = _rule_VB + "\r\n";
                _rule_VB = _rule_VB + "\r\n";



                // brgin rule body
                _rule_VB = _rule_VB + "\r\n'BEGIN Rule Body**********************************************";
                _rule_VB = _rule_VB + "\r\n'*************************************************************";
                _rule_VB = _rule_VB + "\r\n";





                foreach (string p in RuleList)
                //{
                    _rule_VB = _rule_VB + "\r\n" + p;
                //}





                foreach (varField v in _varField)
                {

                    if (v.isReturn)
                    {

                        _rule_VB = _rule_VB + "\r\n";
                        _rule_VB = _rule_VB + "\r\n'Return Value";
                        _rule_VB = _rule_VB + "\r\nReturn " + v.Name;





                    }



                }

                _rule_VB = _rule_VB + "\r\n";
                _rule_VB = _rule_VB + "\r\n'*************************************************************";
                _rule_VB = _rule_VB + "\r\n'END Rule Body************************************************";

                _rule_VB = _rule_VB + "\r\n";
                _rule_VB = _rule_VB + "\r\nEnd Function";


                r = 0;


            }
            catch (Exception ex)
            {


            }



            return r;

        }
    }
}
