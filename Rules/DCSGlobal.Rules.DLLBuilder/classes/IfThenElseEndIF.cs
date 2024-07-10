using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCSGlobal.Rules.DLLBuilder
{
    class IfThenElseEndIF
    {

        private string _IfClause;
        private string _ElseClause;


        private string _IfAction;
        private string _ElseAction;

        private List<string> _OutCode = new List<string>();
        private List<string> _InCode = new List<string>();




        public List<string> Code
        {
            get
            {
                return _OutCode;
            }

        }

        public string AddLine
        {
            set
            {
                _InCode.Add(value);
            }


        }




        public int ProcessIfThen()
        {
            int r = -1;


            return r;
        }



    }
}
