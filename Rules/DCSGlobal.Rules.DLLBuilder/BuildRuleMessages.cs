using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


using System.Data.SqlClient;
using System.Data;

using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using DCSGlobal.BusinessRules.Logging;

namespace DCSGlobal.Rules.DLLBuilder
{
   public  class BuildRuleMessages : IDisposable
    {

       private Dictionary<int,string> _RuleMessages = new Dictionary<int,string>();
       private string _ConnectionString = string.Empty;
        bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~BuildRuleMessages()
        {
            Dispose(false);
        }




        public BuildRuleMessages()
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

    
       public Dictionary<int,string>  GetRuleMessages()
        {

            string select = "select rule_id, rule_description  from rule_def";
            int rule_id = 0;
            string rule_description  = string.Empty;





            using (SqlConnection con = new SqlConnection(_ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(select, con))
                {
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataReader idr = cmd.ExecuteReader())
                    {
                        if (idr.HasRows)
                        {
                            while (idr.Read())
                            {
                                if (idr["rule_id"] != System.DBNull.Value)
                                {
                                    rule_id = Convert.ToInt32(idr["rule_id"]);
                                }


                                if (idr["rule_description"] != System.DBNull.Value)
                                {
                                    rule_description = Convert.ToString(idr["rule_description"]);
                                }

                                _RuleMessages.Add(rule_id, rule_description);
                            
                            
                            }
                        }
                    }
                }
            }

            return _RuleMessages;

        }

    }
}
