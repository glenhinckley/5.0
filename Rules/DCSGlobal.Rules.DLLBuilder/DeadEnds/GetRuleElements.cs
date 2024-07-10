using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DCSGlobal.Rules.DLLBuilder
{
    class GetRuleElements : IDisposable
    {

        private int top = 100;
        private string _ConnectionString = "Data Source=10.1.1.120;Initial Catalog=al60_seton_lite_developer;Persist Security Info=True;User ID=al60_seton_lite_developer_user;Password=al60_seton_lite_developer_password";
        private string dbTempString = string.Empty;
        private string select = string.Empty;

        private List<string> _words = new List<string>();

        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~GetRuleElements()
        {
            Dispose(false);
        }


        bool _disposed;


        public GetRuleElements()
        {
            GetElements();
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


        private void GetElements()
        {
            select = "SELECT [db_column_name]  FROM [rule_elements]";

            using (SqlConnection con = new SqlConnection(_ConnectionString))
            {
                con.Open();
                //
                // Create new SqlCommand object.
                //
                using (SqlCommand cmd = new SqlCommand(select, con))
                {

                    cmd.CommandType = CommandType.Text;

                    using (SqlDataReader idr = cmd.ExecuteReader())
                    {

                        if (idr.HasRows)
                        {



                            // Call Read before accessing data. 
                            while (idr.Read())
                            {



                                if (idr["db_column_name"] != System.DBNull.Value)
                                {
                                    _words.Add(Convert.ToString((string)idr["db_column_name"]));
                                }
                               



                            }
                        }
                    }
                }
            }

        }

        public bool isRuleElements(string word)
        {
            bool r = false;



            if (_words.Contains(word))
            {
                r = true;
            }

            return r;


        }

    }
}
