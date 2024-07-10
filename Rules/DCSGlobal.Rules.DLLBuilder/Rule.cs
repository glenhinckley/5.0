using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCSGlobal.Rules.DLLBuilder
{
    class Rule
    {
        public Int64  RuleId { get; set; }
        public string RuleName { get; set; }
        public string RuleStatus { get; set; }
        public string RuleAppovedStatus { get; set; }
        public string RuleNote { get; set; }
        public string RuleDescription { get; set; }
        public string RuleDef { get; set; }
        public string RuleVBBody { get; set; }
        public string RuleVBBodyNote { get; set; }
        public string RuleVBPrototype { get; set; }
        public string RuleVBPrototypeNote { get; set; }
    }
}
