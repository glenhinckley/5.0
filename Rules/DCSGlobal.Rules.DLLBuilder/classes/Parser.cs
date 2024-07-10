using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;

namespace DCSGlobal.Rules.DLLBuilder
{
    class Parser : IDisposable
    {





        private VBSKeyWords vbxk = new VBSKeyWords();
        private VBSOperators vbop = new VBSOperators();
        private StringStuff ss = new StringStuff();



        private enum TokenType
        {
            NA = -1,
            Identifier = 0,
            Keyword = 1,
            Separator = 2,
            Operator = 3,
            Literal = 4,
            Comment = 5,
            Parameter = 6

        }

        private string EOT = Convert.ToString((char)3);
        private string EOL = Convert.ToString((char)4);

        private string _RegexName = string.Empty;

        bool _disposed;


        private List<string> _Code = new List<string>();
        private List<string> _TokenList = new List<string>();
        private List<varField> _varField = new List<varField>();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Parser()
        {
            Dispose(false);
        }




        public Parser()
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





        public List<string> Code
        {
            get
            {
                return _Code;
            }
        }


        public int Parse(List<string> Tokens)
        {

            int r = -1;

            _TokenList.Clear();
            _TokenList = Tokens;


            r = Pass_10_FindTheReturn(_TokenList);

            r = Pass_20_FindTheSets(_TokenList);

            r = Pass_30_FindTheDims(_TokenList);

            r = Pass_40_FindTheParamters(_TokenList);

            r = Pass_50_FindUndeclared(_TokenList);


            r = Pass_100_RemoveReturns(_TokenList);

            r = Pass_90_RemoveExit(_TokenList);



            //look for end if
            r = Pass_70_LookForEndIF(_TokenList);

            if (r != 0)
            {
                r = Pass_80_CloseIFTHEN(_TokenList);
            }






            //fix returns


            r = Pass_110_PopulateRegEx(_TokenList);


            //fix regex

            return r;
        }


        private int Pass_10_FindTheReturn(List<string> Tokens)
        {

            int r = -1;

            string TokenName = string.Empty;
            string TokenValue = string.Empty;
            string ParsedLine = string.Empty;
            int LineNumber = 0;
            bool ReturnFound = false;
            bool ReturnComplete = false;


            foreach (string Line in Tokens)
            {
                TokenName = string.Empty;
                TokenValue = string.Empty;
                ParsedLine = string.Empty;

                int TokenCount = 0;

                TokenCount = Regex.Matches(Line, EOT).Count;

                int c = 1;

                LineNumber++;


                for (c = 1; c <= TokenCount; c++)
                {

                    string Token = string.Empty;

                    Token = ss.ParseDemlimtedString(Line, EOL, c);

                    TokenName = ss.ParseDemlimtedString(Token, EOT, 1);
                    TokenValue = ss.ParseDemlimtedString(Token, EOT, 2);
                    if (!ReturnComplete)
                    {
                        if (!ReturnFound)
                        {
                            if (TokenValue.ToLower() == "return")
                            {
                                ReturnFound = true;

                            }
                        }
                        else
                        {

                            if (TokenName == "literal")
                            {

                                int n;
                                bool isNumeric = int.TryParse(TokenValue, out n);


                                if (!isNumeric)
                                {
                                    varField varfield = new varField();

                                    varfield.VBName = TokenValue;
                                    varfield.Name = TokenValue;
                                    varfield.isReturn = true;

                                    AddVar(varfield);
                                    ReturnComplete = true;

                                }

                                else
                                {
                                    // find the return and 





                                }



                            }
                            else
                            {

                            }



                        }


                    }

                }
            }

            return r;

        }


        private int Pass_20_FindTheSets(List<string> Tokens)
        {

            int r = -1;

            string TokenName = string.Empty;
            string TokenValue = string.Empty;
            string ParsedLine = string.Empty;
            int LineNumber = 0;
            bool SetFound = false;
            bool NameFound = false;
            bool ObjectTypeFound = false;

            foreach (string Line in Tokens)
            {
                TokenName = string.Empty;
                TokenValue = string.Empty;
                ParsedLine = string.Empty;

                SetFound = false;
                NameFound = false;
                ObjectTypeFound = false;

                string SetName = string.Empty;
                string SetType = string.Empty;
                string Token = string.Empty;
                string SetObjType = string.Empty;

                int TokenCount = 0;

                TokenCount = Regex.Matches(Line, EOT).Count;

                int c = 1;

                LineNumber++;


                for (c = 1; c <= TokenCount; c++)
                {


                    Token = ss.ParseDemlimtedString(Line, EOL, c);

                    TokenName = ss.ParseDemlimtedString(Token, EOT, 1);
                    TokenValue = ss.ParseDemlimtedString(Token, EOT, 2);


                    if (TokenName.ToLower() == "set")
                    {
                        //  ReturnFound = true;
                        SetFound = true;
                    }

                    // find the name of the object

                    if (SetFound)
                    {
                        if (!NameFound)
                        {
                            if (TokenName.ToLower() == "literal")
                            {
                                NameFound = true;
                                SetName = TokenValue;
                            }
                        }

                        if (!ObjectTypeFound)
                        {
                            if (TokenName.ToLower() == "new")
                            {
                                NameFound = true;
                                SetObjType = TokenValue;
                            }
                        }
                    }
                }


                //add the set to the var list
                if (SetFound)
                {
                    varField varfield = new varField();

                    varfield.VBName = TokenValue;
                    varfield.Name = TokenValue;
                    varfield.isRegEx = true;

                    AddVar(varfield);

                }

            }




            return r;

        }


        private int Pass_30_FindTheDims(List<string> Tokens)
        {

            int r = -1;

            string TokenName = string.Empty;
            string TokenValue = string.Empty;
            string ParsedLine = string.Empty;
            int LineNumber = 0;
            bool DimFound = false;


            foreach (string Line in Tokens)
            {
                TokenName = string.Empty;
                TokenValue = string.Empty;
                ParsedLine = string.Empty;

                DimFound = false;


                string SetName = string.Empty;
                string SetType = string.Empty;
                string Token = string.Empty;
                string SetObjType = string.Empty;

                int TokenCount = 0;

                TokenCount = Regex.Matches(Line, EOT).Count;

                int c = 1;

                LineNumber++;


                for (c = 1; c <= TokenCount; c++)
                {


                    Token = ss.ParseDemlimtedString(Line, EOL, c);

                    TokenName = ss.ParseDemlimtedString(Token, EOT, 1);
                    TokenValue = ss.ParseDemlimtedString(Token, EOT, 2);


                    if (TokenValue.ToLower() == "dim")
                    {
                        //  ReturnFound = true;
                        DimFound = true;
                    }

                    // find the name of the object

                    if (DimFound)
                    {

                        if (TokenName.ToLower() == "literal")
                        {

                            SetName = TokenValue;

                            //add the set to the var list
                            if (DimFound)
                            {
                                varField varfield = new varField();

                                varfield.VBName = TokenValue;
                                varfield.Name = TokenValue;
                                varfield.isDim = true;

                                AddVar(varfield);

                            }


                        }
                    }
                }
            }
            return r;
        }



        private int Pass_40_FindTheParamters(List<string> Tokens)
        {

            int r = -1;

            string TokenName = string.Empty;
            string TokenValue = string.Empty;
            string ParsedLine = string.Empty;
            int LineNumber = 0;
            bool ParameterFound = false;
            bool NameFound = false;


            foreach (string Line in Tokens)
            {
                TokenName = string.Empty;
                TokenValue = string.Empty;
                ParsedLine = string.Empty;

                ParameterFound = false;
                NameFound = false;


                string SetName = string.Empty;
                string SetType = string.Empty;
                string Token = string.Empty;
                string SetObjType = string.Empty;

                int TokenCount = 0;

                TokenCount = Regex.Matches(Line, EOT).Count;

                int c = 1;

                LineNumber++;


                for (c = 1; c <= TokenCount; c++)
                {


                    Token = ss.ParseDemlimtedString(Line, EOL, c);

                    TokenName = ss.ParseDemlimtedString(Token, EOT, 1);
                    TokenValue = ss.ParseDemlimtedString(Token, EOT, 2);


                    //if (TokenName.ToLower() == "parameter")
                    //{
                    //    //  ReturnFound = true;
                    //    ParameterFound = true;
                    //}

                    //// find the name of the object

                    //if (ParameterFound)
                    //{
                    //if (!NameFound)
                    //{
                    if (TokenName.ToLower() == "parameter")
                    {
                        NameFound = true;
                        SetName = TokenValue;
                        //add the set to the var list

                        varField varfield = new varField();

                        varfield.VBName = TokenValue;
                        varfield.Name = TokenValue;
                        varfield.isParamter = true;

                        AddVar(varfield);




                    }
                }


                //    }
                // }




            }




            return r;

        }


        private int Pass_50_FindUndeclared(List<string> Tokens)
        {

            int r = -1;

            string TokenName = string.Empty;
            string TokenValue = string.Empty;
            string ParsedLine = string.Empty;
            int LineNumber = 0;
            bool SetFound = false;
            bool NameFound = false;
            bool ObjectTypeFound = false;

            foreach (string Line in Tokens)
            {
                TokenName = string.Empty;
                TokenValue = string.Empty;
                ParsedLine = string.Empty;

                SetFound = false;
                NameFound = false;
                ObjectTypeFound = false;

                string SetName = string.Empty;
                string SetType = string.Empty;
                string Token = string.Empty;
                string SetObjType = string.Empty;

                int TokenCount = 0;

                TokenCount = Regex.Matches(Line, EOT).Count;

                int c = 1;

                LineNumber++;


                for (c = 1; c <= TokenCount; c++)
                {


                    Token = ss.ParseDemlimtedString(Line, EOL, c);

                    TokenName = ss.ParseDemlimtedString(Token, EOT, 1);
                    TokenValue = ss.ParseDemlimtedString(Token, EOT, 2);


                    if (TokenName.ToLower() == "set")
                    {
                        //  ReturnFound = true;
                        SetFound = true;
                    }

                    // find the name of the object

                    if (SetFound)
                    {
                        if (!NameFound)
                        {
                            if (TokenName.ToLower() == "literal")
                            {
                                NameFound = true;
                                SetName = TokenValue;
                            }
                        }

                        if (!ObjectTypeFound)
                        {
                            if (TokenName.ToLower() == "new")
                            {
                                NameFound = true;
                                SetObjType = TokenValue;
                            }
                        }
                    }
                }


                //add the set to the var list
                if (SetFound)
                {
                    varField varfield = new varField();

                    varfield.VBName = TokenValue;
                    varfield.Name = TokenValue;
                    varfield.isRegEx = true;

                    // AddVar(varfield);

                }

            }




            return r;

        }



        private int Pass_70_LookForEndIF(List<string> Tokens)
        {

            int r = -1;


            List<string> _Tokens = new List<string>();

            string TokenName = string.Empty;
            string TokenValue = string.Empty;
            string ParsedLine = string.Empty;

            int ifCount = 0;
            int endCount = 0;


            foreach (string Line in Tokens)
            {
                TokenName = string.Empty;
                TokenValue = string.Empty;
                ParsedLine = string.Empty;


                string LineToLower = string.Empty;

                string SetName = string.Empty;
                string SetType = string.Empty;
                string Token = string.Empty;
                string SetObjType = string.Empty;

                string _TokenizedLine = string.Empty;

                int TokenCount = 0;

                TokenCount = Regex.Matches(Line, EOT).Count;

                int c = 1;

                LineToLower = Line.ToLower();


                for (c = 1; c <= TokenCount; c++)
                {


                    Token = ss.ParseDemlimtedString(Line, EOL, c);

                    TokenName = ss.ParseDemlimtedString(Token, EOT, 1);
                    TokenValue = ss.ParseDemlimtedString(Token, EOT, 2);
                    //

                    //if (IfFound && TokenName == "keyword" && TokenValue.ToLower() == "return")
                    //{

                    //    _TokenizedLine = TokenName + EOT + "endif" + EOL;
                    //    _Tokens.Insert(LineNumber, _TokenizedLine);
                    //    _TokenizedLine = string.Empty;
                    //    IfFound = false;
                    //}


                    //if (IfFound && TokenName == "keyword" && TokenValue.ToLower() == "if")
                    //{

                    //    _TokenizedLine = TokenName + EOT + "endif" + EOL;
                    //    _Tokens.Insert(LineNumber, _TokenizedLine);
                    //    _TokenizedLine = string.Empty;
                    //    IfFound = false;
                    //}


                    if (TokenName == "keyword" && TokenValue.ToLower() == "if")
                    {
                        ifCount++;
                    }


                    if (TokenName == "keyword" && TokenValue.ToLower() == "end")
                    {
                        endCount++;
                    }



                }

            }


            if ((endCount * 2) == ifCount)
            {
                r = 0;
            }


            return r;

        }


        private int Pass_80_CloseIFTHEN(List<string> Tokens)
        {

            int r = -1;


            List<string> _Tokens = new List<string>();

            string TokenName = string.Empty;
            string TokenValue = string.Empty;
            string ParsedLine = string.Empty;
            int LineNumber = 0;
            bool IfFound = false;
            //  bool ThenFound = false;





            /// bool ObjectTypeFound = false;

            foreach (string Line in Tokens)
            {
                TokenName = string.Empty;
                TokenValue = string.Empty;
                ParsedLine = string.Empty;


                string LineToLower = string.Empty;

                //  IfFound = false;
                // ThenFound = false;
                //  ObjectTypeFound = false;

                string SetName = string.Empty;
                string SetType = string.Empty;
                string Token = string.Empty;
                string SetObjType = string.Empty;

                string _TokenizedLine = string.Empty;

                int TokenCount = 0;

                TokenCount = Regex.Matches(Line, EOT).Count;

                int c = 1;

                LineToLower = Line.ToLower();


                for (c = 1; c <= TokenCount; c++)
                {


                    Token = ss.ParseDemlimtedString(Line, EOL, c);

                    TokenName = ss.ParseDemlimtedString(Token, EOT, 1);
                    TokenValue = ss.ParseDemlimtedString(Token, EOT, 2);
                    //

                    if (IfFound && TokenName == "keyword" && TokenValue.ToLower() == "return")
                    {

                        _TokenizedLine = TokenName + EOT + "endif" + EOL;
                        _Tokens.Insert(LineNumber, _TokenizedLine);
                        _TokenizedLine = string.Empty;
                        IfFound = false;
                    }


                    if (IfFound && TokenName == "keyword" && TokenValue.ToLower() == "if")
                    {

                        _TokenizedLine = TokenName + EOT + "endif" + EOL;
                        _Tokens.Insert(LineNumber, _TokenizedLine);
                        _TokenizedLine = string.Empty;
                        IfFound = false;
                    }


                    if (TokenName == "keyword" && TokenValue.ToLower() == "if")
                    {


                        IfFound = true;


                        if (LineToLower.Contains("then"))
                        {

                        }

                        if (LineToLower.Contains("else"))
                        {

                        }
                        if (LineToLower.Contains("exit"))
                        {


                            _TokenizedLine = TokenName + EOT + "endif" + EOL;

                            _Tokens.Insert(LineNumber, _TokenizedLine);
                            _TokenizedLine = string.Empty;
                            IfFound = false;

                        }
                        if (LineToLower.Contains("return"))
                        {
                            _TokenizedLine = TokenName + EOT + "endif" + EOL;
                            _Tokens.Insert(LineNumber, _TokenizedLine);
                            _TokenizedLine = string.Empty;
                            IfFound = false;
                        }

                        // is there a return in the line
                        // is there a then in the line
                        // is there a elses
                        // is there an exit

                    }




                }

                _Tokens.Add(Line);
                LineNumber++;
            }


            _TokenList.Clear();
            _TokenList = _Tokens;

            return r;

        }


        private int Pass_90_RemoveExit(List<string> Tokens)
        {

            int r = -1;


            List<string> _Tokens = new List<string>();

            string TokenName = string.Empty;
            string TokenValue = string.Empty;
            string ParsedLine = string.Empty;
            int LineNumber = 0;
            bool ExitFound = false;
            bool KillLine = false;







            foreach (string Line in Tokens)
            {
                TokenName = string.Empty;
                TokenValue = string.Empty;
                ParsedLine = string.Empty;


                string LineToLower = string.Empty;



                string SetName = string.Empty;
                string SetType = string.Empty;
                string Token = string.Empty;
                string SetObjType = string.Empty;

                string _TokenizedLine = string.Empty;

                int TokenCount = 0;

                TokenCount = Regex.Matches(Line, EOT).Count;

                int c = 1;

                LineToLower = Line.ToLower();


                for (c = 1; c <= TokenCount; c++)
                {


                    Token = ss.ParseDemlimtedString(Line, EOL, c);

                    TokenName = ss.ParseDemlimtedString(Token, EOT, 1);
                    TokenValue = ss.ParseDemlimtedString(Token, EOT, 2);
                    //

                    if (TokenName == "keyword" && TokenValue.ToLower() == "exit")
                    {
                        ExitFound = true;
                    }

                    if (ExitFound && TokenName == "keyword" && TokenValue.ToLower() == "function")
                    {
                        KillLine = true;
                    }


                }

                if (!KillLine)
                {
                    _Tokens.Add(Line);
                }

                LineNumber++;
            }


            _TokenList.Clear();
            _TokenList = _Tokens;

            return r;

        }


        private int Pass_100_RemoveReturns(List<string> Tokens)
        {

            int r = -1;


            List<string> _Tokens = new List<string>();

            string TokenName = string.Empty;
            string TokenValue = string.Empty;
            string ParsedLine = string.Empty;
            int LineNumber = 0;
            bool ReturnFound = false;
            bool KillLine = false;







            foreach (string Line in Tokens)
            {
                TokenName = string.Empty;
                TokenValue = string.Empty;
                ParsedLine = string.Empty;


                string LineToLower = string.Empty;



                string SetName = string.Empty;
                string SetType = string.Empty;
                string Token = string.Empty;
                string SetObjType = string.Empty;

                string _TokenizedLine = string.Empty;

                int TokenCount = 0;



                TokenCount = Regex.Matches(Line, EOT).Count;

                int c = 1;

                LineToLower = Line.ToLower();

                if (LineToLower.Contains("return"))
                {
                    string TempLine = string.Empty;

                    for (c = 1; c <= TokenCount; c++)
                    {


                        Token = ss.ParseDemlimtedString(Line, EOL, c);

                        TokenName = ss.ParseDemlimtedString(Token, EOT, 1);
                        TokenValue = ss.ParseDemlimtedString(Token, EOT, 2);




                        // i only to see if the token value = return and return not found

                        if (!ReturnFound)
                        {
                            if (TokenName == "keyword" && TokenValue.ToLower() == "return")
                            {


                                ReturnFound = true;


                            }
                            else
                            {
                                TempLine = TempLine + Token;
                            }
                        }



                        if (ReturnFound)
                        { //is the return value a know var or numeric or a value in quotes.
                            // set found flag for value
                            // check to se if any tokens after literal(return value)
                            //if not kill the line



                            // token count > 2
                            bool ReturnExist = false;
                            string ReturnName = string.Empty;

                            // LOOK TO SEE if the return has been found
                            foreach (varField v in _varField)
                            {
                                if (v.isReturn)
                                {
                                    ReturnName = v.Name;
                                    ReturnExist = true;
                                    // set a flag so we know the return exists
                                }
                            }


                            // no return was found so...
                            if (!ReturnExist)
                            {
                                // if flag unknow add it to ar list
                                varField v = new varField();
                                v.isReturn = true;
                                v.Name = "ReturnE";
                                ReturnName = "ReturnE";
                                AddVar(v);
                            }


                            if (TokenName == "literal")
                            {

                                int Num;

                                bool isNum = int.TryParse(TokenValue, out Num);


                                if (isNum)
                                {

                                    // we need to find the return var and set it = to the num


                                    TempLine = TempLine + EOL + "literal" + EOT + ReturnName + "operator" + EOT;




                                }



                                // check token count if 2 then do nothing
                                //if(TokenCount != 2)
                                //{
                                //    ReturnFound = false;

                                //}
                                //    else
                                //    {

                                //        // its an int so find the return and set it = to the int

                                //        //EOL + TokenName + EOT + w;
                                //        //TokenName = "literal";

                                //        TempLine = TempLine + "literal" +  TokenValue;
                                //    }

                                //    //  Response.Write("Not an interger");

                                //}
                            }



                        }
                    }
                }
                else
                {
                    _Tokens.Add(Line);

                }


                LineNumber++;
            }


            _TokenList.Clear();
            _TokenList = _Tokens;

            return r;

        }



        private int Pass_110_PopulateRegEx(List<string> Tokens)
        {

            int r = -1;


            List<string> _Tokens = new List<string>();

            string TokenName = string.Empty;
            string TokenValue = string.Empty;
            string ParsedLine = string.Empty;
            int LineNumber = 0;
            bool isRegEx = false;








            foreach (string Line in Tokens)
            {
                TokenName = string.Empty;
                TokenValue = string.Empty;
                ParsedLine = string.Empty;


                string LineToLower = string.Empty;



                string SetName = string.Empty;
                string SetType = string.Empty;
                string Token = string.Empty;
                string SetObjType = string.Empty;

                string _TokenizedLine = string.Empty;

                int TokenCount = 0;

                TokenCount = Regex.Matches(Line, EOT).Count;



                int c = 1;



                for (c = 1; c <= TokenCount; c++)
                {
                    Token = ss.ParseDemlimtedString(Line, EOL, c);

                    TokenName = ss.ParseDemlimtedString(Token, EOT, 1);
                    TokenValue = ss.ParseDemlimtedString(Token, EOT, 2);



                    if (TokenName == "literal")
                    {

                        int Num;

                        bool isNum = int.TryParse(TokenValue, out Num);


                        if (!isNum)
                        {

                            // lets look and see if this is a regex


                            foreach (varField v in _varField)
                            {
                                if (TokenValue == v.Name)
                                {
                                    isRegEx = v.isRegEx;
                                }
                            }




                        }

                    }

                    if (isRegEx)
                    {


                    }

                }


                LineNumber++;
            }


            _TokenList.Clear();
            _TokenList = _Tokens;

            return r;

        }



        private void Identifier(string Line)
        {



            //TokenName = ss.ParseDemlimtedString(Line, EOT, 1);
            //TokenValue = ss.ParseDemlimtedString(Line, EOT, 2);



        }


        private void AddVar(varField varfield)
        {
            // check to see if the var is known
            string Varname = string.Empty;
            Varname = varfield.Name;

            if (!FindVar(Varname))
            {
                _varField.Add(varfield);
            }
        }



        private bool FindVar(string literal)
        {
            bool isKnown = false;
            foreach (varField v in _varField)
            {
                if (literal == v.Name)
                {
                    isKnown = true;
                }
            }
            return isKnown;
        }

        private void AddLine(string Line)
        {
            _Code.Add(Line);
        }




    }
}
