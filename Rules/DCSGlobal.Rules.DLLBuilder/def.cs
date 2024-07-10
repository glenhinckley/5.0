using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;

namespace DCSGlobal.Rules.DLLBuilder
{
    class defBuilder : IDisposable
    {

        private Dictionary<string, string> _dim_strings = new Dictionary<string, string>();
        private Dictionary<string, int> _dim_ints = new Dictionary<string, int>();
        private Dictionary<string, double> _dim_doubles = new Dictionary<string, double>();
        private Dictionary<string, string> _dim_regexs = new Dictionary<string, string>();
        private Dictionary<string, string> _dim_sets = new Dictionary<string, string>();
        private Dictionary<string, string> _parameters = new Dictionary<string, string>();
        private Dictionary<string, string> _assignment = new Dictionary<string, string>();




        private List<regex> _Regex = new List<regex>();







        private StringStuff ss = new StringStuff();


        private GetRuleElements RE = new GetRuleElements();

        private string _Line = string.Empty;

        private string _rule_name = string.Empty;
        private string _rule_id = string.Empty;
        private string _rule_description = string.Empty;
        private string _rule_VB = string.Empty;
        private string _rule_VBS = string.Empty;
        private string _rule_RegEXName = string.Empty;

        private string _ReturnLine = string.Empty;

        bool _disposed;


        private VBSKeyWords vbxk = new VBSKeyWords();


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~defBuilder()
        {
            Dispose(false);
        }




        public defBuilder()
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



        public string BuildRuleDecs()
        {
            string r = string.Empty;
            //bool FirstFound = false;


            foreach (KeyValuePair<string, string> p in _dim_strings)
            {

                //if (!FirstFound)
                //{
                //    r = r + "ByVal " + ss.ParseDemlimtedString(p, "|", 2) + " as string";
                //    FirstFound = true;
                //}
                //else
                //{
                //    r = r + " , ByVal " + ss.ParseDemlimtedString(p, "|", 2) + " as string";
                //}


                r = r + "\r\nDim " + p.Key + " as string =  string.Empty";


            }


            foreach (KeyValuePair<string, string> p in _dim_regexs)
            {

                //if (!FirstFound)
                //{
                //    r = r + "ByVal " + ss.ParseDemlimtedString(p, "|", 2) + " as string";
                //    FirstFound = true;
                //}
                //else
                //{
                //    r = r + " , ByVal " + ss.ParseDemlimtedString(p, "|", 2) + " as string";
                //}


                r = r + "\r\nDim " + p.Key + " as RegEx";


            }



            return r;

        }

        public string BuildRuleSets()
        {
            string r = string.Empty;
            //bool FirstFound = false;

            foreach (KeyValuePair<string, string> p in _dim_regexs)
            {

                //if (!FirstFound)
                //{
                //    r = r + "ByVal " + ss.ParseDemlimtedString(p, "|", 2) + " as string";
                //    FirstFound = true;
                //}
                //else
                //{
                //    r = r + " , ByVal " + ss.ParseDemlimtedString(p, "|", 2) + " as string";
                //}


                r = r + "\r\nSet " + p.Key + " New  = " + p.Value;


            }

            return r;
        }


        public string ReturnLine
        {

            get
            {
                return _ReturnLine;
            }
        }


        public int NewLine(string RuleLine)
        {

            int LineProcessed = 0;
            //string r = "NA";


            // we test ever line to see if it either just a replacment of ## or and something else to do with declaring vars
            // if we find a match for a rule then we process it and return true




            bool LineMatch = false;

            bool isAssigment = false;
            string _regexValue = string.Empty;
            string _RuleLine = string.Empty;




            //get line typw
            string RuleLineFirstWord = string.Empty;
            string ActionPath = "NA";



            _RuleLine = RuleLine.ToLower();






            RuleLineFirstWord = ss.ParseDemlimtedString(_RuleLine, " ", 1);

            if (vbxk.isKeyWord(RuleLineFirstWord))
            {
                ActionPath = "NA";
                LineMatch = true;



                ReplaceRuleElements(RuleLine);
                LineProcessed = 1;
            }


            // (?<=\=#)(.*?)(?=\#)

            // find vars  	"[=#;^A-Za-z0-9;#]"
            //fname=#patient_first_name#

            Regex regex = new Regex(@"#(.*?)#");
            Match match = regex.Match(RuleLine);





            // does line contain =
            if (match.Success)
            {


                _regexValue = match.Value;
                _regexValue = _regexValue.Replace("#", "");

                isAssigment = RuleLine.Contains("=");

            }

            if (!LineMatch)
            {
                if (_RuleLine.Contains("=") && _RuleLine.Contains("set") && _RuleLine.Contains("new"))
                {


                    if (_RuleLine.Contains("regex"))
                    {
                   
                    
                    
                    
                    }

                    else
                    {
                        ActionPath = "SETNEW";
                        LineMatch = true;
                    }

                }
            }


            if (!LineMatch)
            {
                if (_RuleLine.Contains("=") && _RuleLine.Contains("set") && _RuleLine.Contains("nothing"))
                {
                    ActionPath = "SETNOTHING";
                    //  LineProcessed = 0;
                    LineMatch = true;
                }
            }



            if (!LineMatch)
            {
                if (_RuleLine.Contains("=") && _RuleLine.Contains("set"))
                {
                    ActionPath = "SET";
                    LineMatch = true;
                }
            }


            if (!LineMatch)
            {
                if (_RuleLine.Contains("=") && _RuleLine.Contains("#"))
                {
                    ActionPath = "ASSIGNMENT";
                    LineMatch = true;
                }
            }


            if (!LineMatch)
            {
                if (_RuleLine.Contains("dim"))
                {
                    ActionPath = "DIM";
                    LineMatch = true;
                }
            }





            //if ((dim.Contains("Reg")))
            //{
            //    dim_line = "Dim " + dim + " As Regex = New Regex()";
            //    _rule_RegEXName = dim;
            //}
            //else
            //{
            //    dim_line = "Dim " + dim + " As String = string.Empty";
            //}

            switch (ActionPath)
            {

                case "SETNEW":





                    ParseSETNEW(RuleLine);
                    LineProcessed = 2;
                    break;





                case "SETNOTHING":


                    if (CheckSetNothing(RuleLine))
                    {
                        LineProcessed = 2;
                    }


                    break;







                case "SET":




                    break;


                case "DIM":

                    ParseDIMS(RuleLine);
                    LineProcessed = 2;

                    break;



                case "ASSIGNMENT":

                    string source = string.Empty;
                    string target = string.Empty;


                    source = ss.ParseDemlimtedString(RuleLine, "=", 1);
                    target = _regexValue;

                    if (!isDimKnown(source))
                    {
                        AddDim(source);
                    }


                    if (!isParameterKnown(target))
                    {
                        AddParameter(target);
                    }


                    AddAssignment(source, target);

                    LineProcessed = 2;

                    break;


                case "NA":


                    break;


            }

            //var q = _words.Any(w => RuleLine.Contains(w));
            //if (q)
            //{
            //    var d = _parameters.Any(w => RuleLine.Contains(w));
            //    if (!d)
            //    {
            //        var s = match.Value;
            //        BuildParameters(s);
            //        LineProcessed = true;

            return LineProcessed;

        }






        private bool isParameterKnown(string Var)
        {

            bool b = false;

            if (_parameters.ContainsKey(Var))
            {
                b = true;
            }

            return b;

        }



        private bool isAssigned(string Source, string Target)
        {

            bool b = false;

            if (_assignment.ContainsKey(Source) && _assignment.ContainsValue(Target))
            {
                b = true;
            }



            return b;

        }


        private bool isDimKnown(string Var)
        {

            bool b = false;




            if (_dim_strings.ContainsKey(Var))
            {
                b = true;
            }

            return b;

        }

        private bool AddParameter(string var)
        {

            bool b = false;

            if (!_parameters.ContainsKey(var))
            {
                _parameters.Add(var, "string");

                b = true;
            }

            return b;
        }



        private bool AddDim(string varDim)
        {

            bool b = false;





            if (!_parameters.ContainsKey(varDim))
            {
                _dim_strings.Add(varDim, "string");

                b = true;
            }





            return b;
        }


        private bool AddAssignment(string Source, string Target)
        {

            bool b = false;

            if (!_assignment.ContainsKey(Source) && !_assignment.ContainsValue(Target))
            {
                _assignment.Add(Source, Target);
                b = true;
            }


            return b;
        }


        private void ParseSETNEW(string RuleLine)
        {


            string _set = string.Empty;
            string _setOPR = string.Empty;
            string _setOPA = string.Empty;
            string _new = string.Empty;
            string _newOPR = string.Empty;
            string _newOPA = string.Empty;





            // break it in two parts before the = and after 


            _set = ss.ParseDemlimtedString(RuleLine, "=", 1);
            _new = ss.ParseDemlimtedString(RuleLine, "=", 2);


            // get the set operand
            _set = _set.TrimStart();

            _setOPR = ss.ParseDemlimtedString(_set, " ", 1);
            _setOPA = ss.ParseDemlimtedString(_set, " ", 2);


            _setOPR = _setOPR.Trim();
            _setOPA = _setOPA.Trim();

            // get the new operand
            _new = _new.TrimStart();

            _newOPR = ss.ParseDemlimtedString(_new, " ", 1);
            _newOPA = ss.ParseDemlimtedString(_new, " ", 2);


            _newOPR = _newOPR.Trim();
            _newOPA = _newOPA.Trim();


            //look and see if the set operand is in any of the colections if so delete it








            // _dim_sets
            if (_dim_strings.ContainsKey(_setOPA))
            {
                _dim_strings.Remove(_setOPA);
            }
            else
            {
                // _dim_strings.Add(_setOPA, "RegExp");

            }

            // _dim_regexs
            // _dim_sets
            if (!_dim_regexs.ContainsKey(_setOPA))
            {
                _dim_regexs.Add(_setOPA, _newOPA);
            }




            //     _dim_ints = new L
            //_dim_doubles = ne
            //_dim_regexs = new
            //_dim_sets = new L
            //_parameters = new


        }





        private void AddSingleDim(string var)
        {

            _dim_strings.Add(var, "string");

        }


        private void ParseDIMS(string RuleLine)
        {
            int i = 0;
            string dim = string.Empty;





            RuleLine = RuleLine.Replace("dim", "");
            RuleLine = RuleLine.Replace("Dim", "");
            RuleLine = RuleLine.Replace("DIM", "");


            int count = RuleLine.Split(',').Length - 1;

            // Continue to next iteration
            for (i = 0; i <= count + 1; i++)
            {
                dim = string.Empty;
                dim = ss.ParseDemlimtedString(RuleLine, ",", i);

                if (!string.IsNullOrEmpty(dim))
                {
                    _dim_strings.Add(dim, "string");
                }
            }
        }







        public void BuildParameters(string RuleLine)
        {

            string LineToAdd = string.Empty;

            string VarName = string.Empty;
            string VarParamter = string.Empty;
            string VarParamterModifier = string.Empty;
            string VarParamterLine = string.Empty;
            string VarParamterLineLower = string.Empty;


            //Do...Loop
            //For...Next
            //For Each...Next
            //If...Then...Else
            //Select Case
            //While...Wend













            //match.

            VarName = ss.ParseDemlimtedString(RuleLine, "=", 1);
            VarParamterLine = ss.ParseDemlimtedString(RuleLine, "=", 2);

            //look for 

            // (?<=\=#)(.*?)(?=\#)

            // find vars  	"[=#;^A-Za-z0-9;#]"
            //fname=#patient_first_name#
            Regex regex = new Regex(@"#(.*?)#");
            Match match = regex.Match(VarParamterLine);
            VarParamter = match.Value;
            VarParamter = VarParamter.Replace("#", "");


            // look for trim and cstr and stuff like that

            //VarName = VarName;

            VarParamterLineLower = VarParamterLine.ToLower();



            if (VarParamterLine.Contains("trim"))
            {

                VarParamterModifier = VarParamterModifier + "|trim";


            }

            if (VarParamterLine.Contains("ucase"))
            {

                VarParamterModifier = VarParamterModifier + "|ucase";


            }


            if (VarParamterLine.Contains("lcase"))
            {


                VarParamterModifier = VarParamterModifier + "|lcase";

            }


            if (VarParamterLine.Contains("cstr"))
            {

                VarParamterModifier = VarParamterModifier + "|cstr";


            }

            LineToAdd = VarName + "|" + VarParamter + "|" + VarParamterModifier;

            _parameters.Add(LineToAdd, LineToAdd);

        }

        public string BuildRuleAssignments()
        {
            string r = string.Empty;
            //bool FirstFound = false;


            foreach (KeyValuePair<string, string> p in _assignment)
            {
                r = r + "\r\n" + p.Key + "  =  " + p.Value;
            }



            return r;

        }



        public string CheckForMatch(string RegrexMatch)
        {
            string r = string.Empty;

            RegrexMatch = RegrexMatch.Replace("#", "");


            if (!_dim_strings.ContainsKey(RegrexMatch))
            {

                AddDim(RegrexMatch);
            }


            return r;

        }


        public string BuildRuleParamter()
        {
            string r = string.Empty;
            bool FirstFound = false;


            foreach (KeyValuePair<string, string> p in _parameters)
            {




                if (!FirstFound)
                {
                    r = r + "ByVal " + p.Key + " as " + p.Value;
                    FirstFound = true;
                }
                else
                {
                    r = r + " , ByVal " + p.Key + " as " + p.Value;


                }


            }



            return r;

        }



        public string BuildRulePrototype()
        {
            string r = string.Empty;
            bool FirstFound = false;


            foreach (KeyValuePair<string, string> p in _parameters)
            {




                if (!FirstFound)
                {
                    r = r + "_" + p.Key;
                    FirstFound = true;
                }
                else
                {
                    r = r + ", _" + p.Key;



                }


            }



            return r;

        }




        private bool CheckSetNothing(string RuleLine)
        {
            bool r = true;
            string VarName = string.Empty;
            string VarParamterLine = string.Empty;



            VarName = ss.ParseDemlimtedString(RuleLine, "=", 1);
            VarParamterLine = ss.ParseDemlimtedString(RuleLine, "=", 2);


            VarParamterLine.Replace("nothing", "");
            VarParamterLine.Replace("Nothing", "");

            VarParamterLine = VarParamterLine.Trim();



            return r;
        }




        private string ReplaceRuleElements(string RuleLine)
        {

            string r = string.Empty;


            string _regexValue = string.Empty;

            // (?<=\=#)(.*?)(?=\#)

            // find vars  	"[=#;^A-Za-z0-9;#]"
            //fname=#patient_first_name#

            Regex regex = new Regex(@"#(.*?)#");
            Match match = regex.Match(RuleLine);





            // does line contain =
            if (match.Success)
            {


                _regexValue = match.Value;
                _regexValue = _regexValue.Replace("#", "");





                //check to see if its declared


                if (!isDimKnown("_" + _regexValue))
                {
                    AddDim("_" + _regexValue);
                    AddParameter(_regexValue);
                    AddAssignment("_" + _regexValue, _regexValue);
                }





                _ReturnLine = RuleLine.Replace(match.Value, "_" + _regexValue);
            }
            else
            {

                _ReturnLine = RuleLine;

            }


            return r;

        }



        public string DestroySets()
        {
            string r = string.Empty;



            foreach (KeyValuePair<string, string> p in _dim_regexs)
            {


                r = r + "\r\nSet  " + p.Key + " = nothing";





            }



            return r;

        }


    }
}
