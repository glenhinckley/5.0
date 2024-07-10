using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCSGlobal.Rules.DLLBuilder
{
    class VBSKeyWords
    {

        private List<string> _words = new List<string> { 
                                                            "byte",
                                                            "byval",
                                                            "call",
                                                            "case",
                                                            "class",
                                                            "const",
                                                            "currency",
                                                            "debug",
                                                            "dim",
                                                            "do",
                                                            "double",
                                                            "each",
                                                            "else",
                                                            "elseif",
                                                            "empty",
                                                            "end",
                                                            "endif",
                                                            "enum",
                                                            "eqv",
                                                            "event",
                                                            "exit",
                                                            "false",
                                                            "for",
                                                            "function",
                                                            "get",
                                                            "goto",
                                                            "if",
                                                            "imp",
                                                            "implements",
                                                            "in",
                                                            "integer",
                                                            "is",
                                                             "len",
                                                            "let",
                                                            "like",
                                                            "long",
                                                            "loop",
                                                            "lset",
                                                            "me",
                                                            "mod",
                                                            "new",
                                                            "next",
                                                            "not",
                                                            "nothing",
                                                            "null",
                                                            "on",
                                                            "option",
                                                            "optional",
                                                            "or",
                                                            "paramarray",
                                                            "preserve",
                                                            "private",
                                                            "public",
                                                            "raiseevent",
                                                            "redim",
                                                            "return",
                                                            "rem",
                                                            "resume",
                                                            "rset",
                                                            "select",
                                                            "set",
                                                            "shared",
                                                            "single",
                                                            "static",
                                                            "stop",
                                                            "sub",
                                                            "then",
                                                            "to",
                                                            "trim",
                                                            "true",
                                                            "type",
                                                            "typeof",
                                                            "until",
                                                            "variant",
                                                            "wend",
                                                            "while",
                                                            "with",
                                                            "xor",
                                                            // objects
                                                            "regexp"

                };

        public bool isKeyWord(string word) 
        {
            bool r = false;



            if (_words.Contains(word))
            {
                r = true;
            }

            return r;

        
        }

        public bool isRuleLineKeyWord(string RuleLine)
        {
            bool r = false;
            string word = string.Empty;


            if (isKeyWord(word))
            {
                r = true;
            }

            return r;


        }


    }
}
