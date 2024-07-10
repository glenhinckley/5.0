using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCSGlobal.Rules.DLLBuilder
{
    class regex
    {
        public string Name { get; set; }
        public bool IgnoreCase { get; set; }
        public bool Global { get; set; }
        public string Pattern { get; set; }
        public string Replace { get; set; }
        public string Match { get; set; }
        public string Test { get; set; }
    }
}
