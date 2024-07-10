using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCSGlobal.Rules.DLLBuilder
{
    public class varField
    {
        private string _Name = string.Empty;
        private string _VBName = string.Empty;
        private string _Type = "string";
        private bool _isParamter = false;
        private bool _isRegEx = false;
        private bool _isDim = false;
        private bool _isReturn = false;
        private object _DefaultValue;
        private bool _Trim = false;
        private bool _UCASE = false;
        private bool _LCASE = false;
        private bool _CSTR = false;
        private bool _isAssignment = false;
        private string _AssignmentName = string.Empty;


        private bool _IgnoreCase = false;
        private bool _Global = false;
        private string _Pattern = string.Empty;
        private string _Replace = string.Empty;
        private string _Match = string.Empty;
        private string _Test = string.Empty;


        public varField()
        {
            //make=Make;
            //year=Year;
        }

        public varField(string Name, string Type, bool isParamter, bool isReturn, string DefaultValue)
        {
            //make=Make;
            //year=Year;
        }
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }


        public string VBName
        {
            get
            {
                return _VBName;
            }
            set
            {
                _VBName = value;
            }
        }

        public string Type
        {
            get
            {
                return _Type;
            }
            set
            {
                _Type = value;
            }
        }

        public bool isParamter
        {
            get
            {
                return _isParamter;
            }
            set
            {
                _isParamter = value;
            }
        }

        public bool isRegEx
        {
            get
            {
                return _isRegEx;
            }
            set
            {
                _isRegEx = value;
            }
        }












        
        public bool isDim
        {
            get
            {
                return _isDim;
            }
            set
            {
                _isDim = value;
            }
        }

        public bool isReturn
        {
            get
            {
                return _isReturn;
            }
            set
            {
                _isReturn = value;
            }
        }

        public bool Trim
        {
            get
            {
                return _Trim;
            }
            set
            {
                _Trim = value;
            }
        }


        public bool UCASE
        {
            get
            {
                return _UCASE;
            }
            set
            {
                _UCASE = value;
            }
        }

        public bool LCASE
        {
            get
            {
                return _LCASE;
            }
            set
            {
                _LCASE = value;
            }
        }


        public bool CSTR
        {
            get
            {
                return _CSTR;
            }
            set
            {
                _CSTR = value;
            }
        }

        public bool isAssignment
        {
            get
            {
                return _isAssignment;
            }
            set
            {
                _isAssignment = value;
            }
        }

        public string AssignmentName
        {
            get
            {
                return _AssignmentName;
            }
            set
            {
                _AssignmentName = value;
            }
        }

        public bool IgnoreCase
        {


            get
            {
                return _IgnoreCase;
            }
            set
            {
                _IgnoreCase = value;
            }

        }
        public bool Global
        {
            get
            {
                return _Global;
            }
            set
            {
                _Global = value;
            }
        }
        
        public string Pattern
        {
            get
            {
                return _Pattern;
            }
            set
            {
                _Pattern = value;
            }
        }
        
        public string Replace
        {
            get
            {
                return _Replace;
            }
            set
            {
                _Replace = value;
            }
        }
        
        public string Match
        {
            get
            {
                return _Match;
            }
            set
            {
                _Match = value;
            }
        }
        
        
        public string Test
        {
            get
            {
                return _Test;
            }
            set
            {
                _Test = value;
            }
        }



        public object DefaultValue
        {
            get
            {
                return _DefaultValue;
            }
            set
            {
                _DefaultValue = value;
            }
        }
    }


    public class varFields : IEnumerator, IEnumerable
    {
        private varField[] varlist;
        int position = -1;

        //Create internal array in constructor.
        public varFields()
        {
            //      varlist = new var[6]
            //{
            //   //new car("Ford",1992),
            //   //new car("Fiat",1988),
            //   //new car("Buick",1932),
            //   //new car("Ford",1932),
            //   //new car("Dodge",1999),
            //   //new car("Honda",1977)
            //};
        }

        //public void Add(object item)
        //{
        //    // Let us only worry about adding the item 
        //    m_Items[freeIndex] = item;
        //    freeIndex++;
        //}


        //IEnumerator and IEnumerable require these methods.
        public IEnumerator GetEnumerator()
        {
            return (IEnumerator)this;
        }

        //IEnumerator
        public bool MoveNext()
        {
            position++;
            return (position < varlist.Length);
        }

        //IEnumerable
        public void Reset()
        { position = 0; }

        //IEnumerable
        public object Current
        {
            get { return varlist[position]; }
        }
    }

}
