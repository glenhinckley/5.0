using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;

namespace DCSGlobal.Rules.DLLBuilder
{
    public class ParseRuleToList : IDisposable
    {

        private List<string> _list = new List<string>();
        private string _ConnectionString = string.Empty;

        private string _rule_name = string.Empty;
        private string _rule_id = string.Empty;
        private string _rule_description = string.Empty;
        private string _rule_VB = string.Empty;
        private string _rule_VBS = string.Empty;
        private string _rule_RegEXName = string.Empty;
        private StringStuff ss = new StringStuff();

        bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ParseRuleToList()
        {
            Dispose(false);
        }


        public ParseRuleToList()
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


        public string ConnectionString
        {

            set
            {

                _ConnectionString = value;
            }
        }



        public List<string> RuleList
        {

            get
            {

              return   _list;
            }
        }
        public int byString(string RuleString)
        {
            int r = -1;

            string temp = string.Empty;
            bool clist = false;
            try
            {


                     if (RuleString.Contains(":"))
                     {
                         RuleString = RuleString.Replace(":", "\r\n");
                     }










                string[] result = Regex.Split(RuleString, @"\r\n");
                foreach (string s in result) //' MessageBox.Show(s);
                {
                    if (string.IsNullOrEmpty(s) == false)
                    {
                        if (s.Contains(" _\r\n"))
                        {
                            string t = string.Empty;
                           // t = s;
                            
                            t = s.Replace(" _\r\n", " ");

                            t = s.Replace("=", " = ");
                            //t = t.Replace("(", "( ");
                            //t = t.Replace(")", " )");

                            temp = temp + t;

                        }
                        else
                        {

                            string t = string.Empty;
                            //t = s;
                            
                            t = s.Replace("=", " = ");
                            //t = t.Replace("(", "( ");
                            //t = t.Replace(")", " )");
                            temp = temp + t;
                            clist = true;

                        }

                        if (clist)
                        {
                            _list.Add(temp);
                            clist = false;
                            temp = string.Empty;

                        }

                    }




                }
                r = 0;
            }
            catch (Exception ex)
            {


            }

            _list.Add("EOF");
            return r;

        }


    }
}
