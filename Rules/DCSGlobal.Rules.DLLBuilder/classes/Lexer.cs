using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;

namespace DCSGlobal.Rules.DLLBuilder
{
    class Lexer : IDisposable
    {

        private string _RegexName = string.Empty;

        bool _disposed;


        private List<string> _Tokens = new List<string>();
        private string _TokenizedLine = string.Empty;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Lexer()
        {
            Dispose(false);
        }




        public Lexer()
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
        private VBSOperators vbop = new VBSOperators();
        private StringStuff ss = new StringStuff();


        public List<string> Tokens
        {
            get
            {
                return _Tokens;
            }
        }


        public int Lex(List<string> RuleList)
        {

            int r = -1;
            string _l = string.Empty;
            string TokenName = string.Empty;
            bool TokenFound = false;
            bool ParameterFound = false;
            bool ParameterNameFound = false;

            string EOT = Convert.ToString((char)3);
            string EOL = Convert.ToString((char)4);
            // find the return name if one if not then we need to make a  return var 
            //some times just a retrun in the middle of no where
            foreach (string l in RuleList)
            {
                _l = l.ToLower();
                _TokenizedLine = string.Empty;
                bool firstFound = false;
                string LastWord = string.Empty;
                r = 0;
                // int ReturnsInRulelineCount = 0;




                //  string searchTerm = "return";
                //  ReurnCountMoreThanOne = ss.ContainsMoreThan(_l, 1, searchTerm, StringComparison.InvariantCultureIgnoreCase);

                _l = _l.Replace(".", " . ");

                _l = _l.Replace("(", " ( ");
                _l = _l.Replace(")", " ) ");
                _l = _l.Replace(",", " , ");
                _l = _l.Replace("'", " ' ");
                _l = _l.Replace("#", " # ");
                _l = _l.Replace("\"", " \" ");

                string[] words = _l.Split(' ');
                string t = string.Empty;


                bool ParamaterCharFound = false;
                bool QuoteCharFound = false;


                //var matchQuery = from word in words
                //                 where word.ToLowerInvariant() == searchTerm.ToLowerInvariant()
                //                 select word;

                // Count the matches, which executes the query.  
                // int wordCount = matchQuery.Count();
                // if (wordCount == 1)
                //{

                foreach (string w in words)
                {






                    TokenFound = false;
                    

                    if (w == "\"")
                    {


                        if (!QuoteCharFound)
                        {
                            TokenName = "separator";
                            TokenFound = true;
                            QuoteCharFound = true;
                        }
                        else
                        {
                        TokenName = "separator";
                            TokenFound = true;
                            QuoteCharFound = false;
                        }




                    
                    
                    }
                    

                    if (w == "#")
                    {

                        if (!ParamaterCharFound)
                        {
                            TokenName = "separator";
                            TokenFound = true;
                            ParamaterCharFound = true;
                        }
                        else
                        {
                            TokenName = "separator";
                            TokenFound = true;
                            ParamaterCharFound = false;
                        }

                   
                    }



                    if (!TokenFound)
                    {
                        if (ParamaterCharFound)
                        {
                            TokenName = "parameter";
                            TokenFound = true;
                       
                        }
                    }


                    if (!TokenFound)
                    {
                        if (QuoteCharFound)
                        {
                            TokenName = "value";
                            TokenFound = true;
                        
                        }
                    }

                    if (w == "")
                    {
                        TokenName = "whitepace";
                        TokenFound = true;
                    }

                    if (!TokenFound)
                    {
                        if (vbxk.isKeyWord(w))
                        {
                            TokenName = "keyword";
                            TokenFound = true;
                        }
                    }


                    if (!TokenFound)
                    {
                        if (w == ")")
                        {
                            TokenName = "separator";
                            TokenFound = true;
                        }
                    }

                    if (!TokenFound)
                    {
                        if (w == "(")
                        {
                            TokenName = "separator";
                            TokenFound = true;
                        }
                    }

                    if (!TokenFound)
                    {
                        if (w == ",")
                        {
                            TokenName = "separator";
                            TokenFound = true;
                        }
                    }


                    if (!TokenFound)
                    {
                        if (vbop.isOperator(w))
                        {
                            TokenName = "operator";
                            TokenFound = true;
                        }
                    }

                    if (!TokenFound)
                    {
                        if (w == "#")
                        {

                            TokenName = "separator";
                            if (ParameterFound)
                            {
                                ParameterFound = false;
                            }
                            else
                            {
                                ParameterFound = true;
                                ParameterNameFound = false;
                            }

                            TokenFound = true;
                        }
                    }


                    if (!TokenFound)
                    {
                        if (w != "'")
                        {
                            TokenName = "literal";
                            TokenFound = true;
                        }
                    }






                    if (!TokenFound)
                    {
                        if (w == "'")
                        {
                            TokenName = "comment";
                            TokenFound = true;
                        }
                    }


                    if (!firstFound)
                    {
                        firstFound = true;
                        _TokenizedLine = _TokenizedLine + TokenName + EOT + w;
                    }
                    else
                    {
                        _TokenizedLine = _TokenizedLine + EOL + TokenName + EOT + w;
                    }
                }
                _Tokens.Add(_TokenizedLine);

            }









            return r;
        }






    }
}
