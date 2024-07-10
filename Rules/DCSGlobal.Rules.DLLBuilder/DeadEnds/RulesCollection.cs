using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCSGlobal.Rules.DLLBuilder
{
    public class RulesCollection
    {

    private string _parameter;

    public RulesCollection(string parameter)
    {
      _parameter = parameter;
    }
    public IEnumerator GetEnumerator()
    {
        return new RulesEnumerator(_parameter);
    }




    }
}
