using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCSGlobal.Rules.DLLBuilder
{
    class VBSOperators
    {

        private List<string> _words = new List<string> { 
                                                            "+",
                                                            "_",
                                                            "*",
                                                            "/",
                                                            "%",
                                                            "^",
                                                            "=",
                                                            "<>",
                                                            ">",
                                                            "<",
                                                            ">=",
                                                            "<=",
                                                            "and",
                                                            "or",
                                                            "not",
                                                            "xor",
                                                            "&",


                };

        public bool isOperator(string word) 
        {
            bool r = false;



            if (_words.Contains(word))
            {
                r = true;
            }

            return r;

        
        }

        public bool isRuleLineOperator(string RuleLine)
        {
            bool r = false;
            string word = string.Empty;


            if (isOperator(word))
            {
                r = true;
            }

            return r;


        }


    }
}
