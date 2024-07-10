using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCSGlobal.BusinessRules.Logging
{
   public static class StringEXT
    {
       public static string Truncate(this string value, int maxLength)
       {

           string s = string.Empty;
        
             s = value.Replace(System.Environment.NewLine, "_");
    
           
           
           
           
           if (string.IsNullOrEmpty(s)) return s;
           return value.Length <= maxLength ? s : s.Substring(0, maxLength);
       }


    }
}
