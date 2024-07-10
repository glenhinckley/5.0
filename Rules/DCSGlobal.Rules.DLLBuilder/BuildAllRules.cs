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
    public class BuildAllRules : IDisposable
    {



        private logExecption log = new logExecption();
        bool _disposed;

        private string _ConnectionString = string.Empty;
        private string _FileName = Convert.ToString(Guid.NewGuid());
        private string _Path = string.Empty;
        private bool _Test = false;
        private List<string> _ErrorList = new List<string>();
        private List<string> _ReferencedAssemblieDLLs = new List<string>();
        private string _AssemblyPath = @"c:\usr\ass\";
        private string _OutputAssembly = string.Empty;

        ~BuildAllRules()
        {
            Dispose(false);
        }





        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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
                log.ConnectionString = value;
            }
        }







        public int Go()
        {
            int r = -1;


            TestRules();

            return r;
        }










        private void TestRules()
        {
            string select = "select * from rule_def where approved_status = 'A'";
            int rule_id = 0;
            string rule_name = string.Empty;
            string rule_def = string.Empty;
            string modified_by = string.Empty;
            DateTime modified_ts = DateTime.Now;
            DateTime RuleBuilt = DateTime.Now;



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


                                if (idr["rule_name"] != System.DBNull.Value)
                                {
                                    rule_name = Convert.ToString(idr["rule_name"]);
                                }


                                if (idr["rule_def"] != System.DBNull.Value)
                                {
                                    rule_def = Convert.ToString(idr["rule_def"]);
                                }



                                if (idr["modified_by"] != System.DBNull.Value)
                                {
                                    modified_by = Convert.ToString(idr["modified_by"]);
                                }

                            
                                
                                if (idr["modified_ts"] != System.DBNull.Value)
                                {
                                    modified_ts = Convert.ToDateTime(idr["modified_ts"]);
                                }

                                try
                                {
                                    string VBRule = string.Empty;
                                    string VBParse = string.Empty;
                                    bool PassFail = false;
                                    using (RuleParser p = new RuleParser())
                                    {
                                     //   p.rule_name = rule_name;
                                        //    // p.rule_description = txtRuleDescription.Text;
                                    //    p.rule_id = Convert.ToString(rule_id);
                                        p.ParseRuleVBS(rule_def);
                                     //   VBRule = p.RuleVB;
                                        //}



                                        using (BuildDLLString BDS = new BuildDLLString())
                                        {
                                            BDS.ConnectionString = _ConnectionString;
                                            BDS.BuildSingleRule(rule_id);
                                            VBRule = BDS.VBString;

                                        }


                                        PassFail = TestBuildCode(VBRule);

                                        if (!PassFail)
                                        {


                                            string CompilerList = string.Empty;


                                            foreach (string l in _ErrorList)
                                            {

                                                CompilerList = CompilerList + l + "^";
                                            }


                                            using (LogFailedRule lfr = new LogFailedRule())
                                            {
                                                lfr.ConnectionString = _ConnectionString;
                                                lfr.Insert(rule_id, "", "", "", modified_ts, DateTime.Now);

                                            }

                                        }


                                    }
                                }
                                catch (Exception ex)
                                {


                                }


                            }
                        }
                    }
                }
            }





        }








        private bool TestBuildCode(string Code)
        {

            bool _BAFSRetrunCode = false;
            _ErrorList.Clear();

            using (BuildAssemballyFromString BAFS = new BuildAssemballyFromString())
            {

                BAFS.ConnectionString = _ConnectionString;
                BAFS.Test = true;
                //BAFS.Path = @"C:\usr\Built_DLL\";
                _BAFSRetrunCode = BAFS.Go(Code);
                _ErrorList = BAFS.ErrorList;
            }
            return _BAFSRetrunCode;
        }



    }
}
