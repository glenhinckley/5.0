using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCSGlobal.Rules.DLLBuilder
{
    class vbRule
    {

        private string _Name = string.Empty;
        private string _Prototype = string.Empty;
        private int _ID = 0;
        private string _Description = string.Empty;
        private List<string> RuleList = new List<string>();



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

        public string Prototype
        {
            get
            {
                return _Prototype;
            }
            set
            {
                _Prototype = value;
            }

        }

        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
            }

        }

    }



    public class vbRules : IEnumerator, IEnumerable
    {
        private vbRules[] vbrule;
        int position = -1;
        int freeIndex = 0;
        //Create internal array in constructor.
        public vbRules()
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

        public void Add(object item)
        {
            // Let us only worry about adding the item 
            //  m_Items[freeIndex] = item;
            //  freeIndex++;
        }


        //IEnumerator and IEnumerable require these methods.
        public IEnumerator GetEnumerator()
        {
            return (IEnumerator)this;
        }

        //IEnumerator
        public bool MoveNext()
        {
            position++;
            return (position < vbrule.Length);
        }

        //IEnumerable
        public void Reset()
        { position = 0; }

        //IEnumerable
        public object Current
        {
            get { return vbrule[position]; }
        }
    }
}
