
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using System.Data.SqlClient;


// his for the soruce code editor
using FastColoredTextBoxNS;

using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using DCSGlobal.BusinessRules.CoreLibraryII;
using DCSGlobal.BusinessRules.CoreLibrary;
using DCSGlobal.BusinessRules.Logging;

using DCSGlobal.EDI.Comunications;
using DCSGlobal.BusinessRules.CoreLibrary.dbCheckStuff;

using DCSGlobal.Rules.DCSGlobal.Rules;
using DCSGlobal.Rules.DLLBuilder;
//using DCSGlobal.Rules.DCSGlobal.Rules

//        DCSGlobal.Rules.DLL

using DCSGlobal.Rules.FireRules;


// loading DLL
using System.IO;
using System.Reflection;



namespace Manual_test_app
{
    public partial class RuleTestCompile : Form
    {
        public RuleTestCompile()
        {
            InitializeComponent();
            select = "SELECT TOP " + top + " * FROM RULE_DEF";
            txtConstring.Text = _ConnectionString;
            // txtSQL.Text = select;

        }
        //   al60_seton_lite_developer   al60_eastmaine_prod
        private int top = 100;
        private string _ConnectionString = "Data Source=10.1.1.120;Initial Catalog=al60_eastmaine_prod;Persist Security Info=True;User ID=al60_seton_lite_developer_user;Password=al60_seton_lite_developer_password";
        private string dbTempString = string.Empty;
        private string select = string.Empty;
        StringStuff ss = new StringStuff();
        CodeInjection ci = new CodeInjection();

        private DataSet _ds_getDummyPatientDataPatient = new DataSet();


        private int _RuleID = 1001;

        private Dictionary<string, string> _EventDictonary = new Dictionary<string, string>();

        private List<string> _ErrorList = new List<string>();
        private List<string> _HospCodes = new List<string>();

        private void RuleEditor_Load(object sender, EventArgs e)
        {
            //  select = "SELECT TOP " + top + " * FROM RULE_DEF";
            //  txtConstring.Text = _ConnectionString;
            /// txtSQL.Text = select;
            /// 

            fctRULE_VBS.Language = Language.VB;

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer1_Panel2_SizeChanged(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void RuleTestCompile_Load(object sender, EventArgs e)
        {

            LoadHospCodes();
            Resize();


        }

        public List<object> LoadAssemblies(String path, String interfaceName, bool recursive)
        {
            List<object> output = new List<object>();

            String[] files = new String[0];
            if (recursive)
            {
                files = Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories);
            }
            else
            {
                files = Directory.GetFiles(path, "*.dll", SearchOption.TopDirectoryOnly);
            }

            foreach (String filename in files)
            {
                Console.WriteLine("Loading " + filename);
                try
                {
                    Assembly assembly = Assembly.LoadFrom(filename);

                    foreach (Type t in assembly.GetTypes())
                    {
                        if (t.IsClass && !t.IsAbstract && t.GetInterface(interfaceName) != null)
                        {
                            output.Add(Activator.CreateInstance(t));
                        }
                    }
                }
                catch (Exception err)
                {
                    Console.WriteLine("Loading fail:" + err.ToString());
                }
                finally
                {
                    Console.WriteLine("Loading complete");
                }
            }

            return output;
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {

            string PrefixData = string.Empty;
            string Comment = string.Empty;
            string Temp = string.Empty;
            {
                foreach (DataGridViewRow row in dataGridView2.SelectedRows)
                {

                    try
                    {
                        fctRULE_VB_DLL.Text = string.Empty;
                        fctRULE_VBS.Text = string.Empty;
                        string RULE_DEF = string.Empty;



                        RULE_DEF = row.Cells["RULE_DEF"].Value.ToString();
                        // REQ = row.Cells["REQ"].Value.ToString();

                        string RULE_VB = string.Empty;

                        fctRULE_VBS.Text = RULE_DEF;
                        //  fctRULE_VB.Text = RULE_VB;


                        _RuleID = Convert.ToInt32(row.Cells["rule_id"].Value.ToString());
                        txtRuleID.Text = row.Cells["rule_id"].Value.ToString();


                        txtRuleName.Text = row.Cells["rule_name"].Value.ToString();
                        txtRuleMessage.Text = row.Cells["rule_description"].Value.ToString();


                        if (chkAutoParse.Checked)
                        {
                            Parse();
                            using (BuildDLLString BDS = new BuildDLLString())
                            {
                                BDS.ConnectionString = _ConnectionString;
                                BDS.BuildSingleRule(Convert.ToInt32(txtRuleID.Text));
                                fctRULE_VB.Text = BDS.VBString;
                            }
                            CompileRule();
                        }
                    }
                    catch (Exception ex)
                    {
                        toolStripStatusLabel1.Text = ex.Message;
                    }
                }
            }
        }


        private void cmdSearch_Click(object sender, EventArgs e)
        {

        }


        private void GetRules()
        {

            bool first = true;
            string top = "100";

            top = "TOP " + txtTop.Text;

            int cii = 0;

            try
            {





                //if (chkAdvanced.Checked == true)
                //{
                //    // '   select SELECT top 100 * FROM SCHEDULER_LOG 

                //}


                select = "select * from rule_def";



                cii = ci.CheckInput(select);

                if (cii == 0)
                {

                    // _ConnectionString = txtConstring.Text;
                    var c = new SqlConnection(_ConnectionString); // Your Connection String here
                    var dataAdapter = new SqlDataAdapter(select, c);

                    var commandBuilder = new SqlCommandBuilder(dataAdapter);
                    var ds = new DataSet();
                    dataAdapter.Fill(ds);
                    // dataGridView2.ReadOnly = true;
                    dataGridView2.DataSource = ds.Tables[0];
                }
                else
                {
                    toolStripStatusLabel1.Text = "BAD BAD BAD, NO DONT DO IT, YOU CANT USE " + ci.KEY_WORDLIST;
                    toolStripStatusLabel1.ForeColor = System.Drawing.Color.Red;

                }
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = ex.Message;
            }

        }


        private void Parse()
        {

            using (RuleParser p = new RuleParser())
            {
                p.RuleName  = txtRuleName.Text;
                p.RuleDescription = txtRuleMessage.Text;
                p.RuleID  = txtRuleID.Text;

                p.ParseRuleVBS(fctRULE_VBS.Text);

                fctRULE_VB.Language = Language.VB;

                fctRULE_VB.Text = p.RuleVB;
            }

        }


        private void cmdTestRule_Click(object sender, EventArgs e)
        {
            Parse();
        }


        private void CompileRule()
        {

            using (BuildAssemballyFromString BAFS = new BuildAssemballyFromString())
            {
                bool _BAFSRetrunCode = false;
                BAFS.ConnectionString = _ConnectionString;
                BAFS.Test = false;
                BAFS.Path = @"C:\usr\Built_DLL\";

                _BAFSRetrunCode = BAFS.Go(fctRULE_VB_DLL.Text);



                if (_BAFSRetrunCode)
                {
                    lblCompilerResult.Text = "GOOD";
                    lblRuleDLLName.Text = BAFS.OutputAssembly;
                }
                else
                {
                    _ErrorList = BAFS.ErrorList;

                    lblCompilerResult.Text = "Failed";

                    //var list = new BindingList<string>(_ErrorList);
                    //DGV.DataSource = list;

                    //string el = string.Empty;

                    foreach (string l in _ErrorList)
                    {
                        //    el = el + l + "/n/r";

                        lstVBErrorList.Items.Add(l);
                        /// .Items.Add(l);
                    }
                    //textBox1.Text = el;

                }
            }
        }



        private void cmdCompile_Click(object sender, EventArgs e)
        {
            lblCompilerResult.Text = "Compilling";
            lblRuleDLLName.Text = "";
            lstVBErrorList.Clear();
            CompileRule();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmdRunRule_Click(object sender, EventArgs e)
        {

        }

        private void cmdDecode_Click(object sender, EventArgs e)
        {

        }

        private void cmdClear_Click(object sender, EventArgs e)
        {
            txtRuleOutputVB.Text = string.Empty;
            txtRuleOutputVBS.Text = string.Empty;
        }

        private void cmdBuild_Click(object sender, EventArgs e)
        {
            using (BuildDLLString BDS = new BuildDLLString())
            {
                BDS.ConnectionString = _ConnectionString;
                BDS.BuildSingleRule(Convert.ToInt32(txtRuleID.Text));
                fctRULE_VB_DLL.Text = BDS.VBString;

            }
        }

        private void cmdGetDataRow_Click(object sender, EventArgs e)
        {



            //getDummyPatientDataPatient

            using (DCSGlobal.Rules.DCSGlobal.Rules.DataSets ds = new DCSGlobal.Rules.DCSGlobal.Rules.DataSets())
            {

                ds.ConnectionString = _ConnectionString;
                _ds_getDummyPatientDataPatient = ds.getDummyPatientDataPatient(txtPatientNumber.Text, Convert.ToString(cmbHospCode.SelectedItem));
            }



            // dataGridView2.ReadOnly = true;
            dvgRow.DataSource = _ds_getDummyPatientDataPatient.Tables["DUMMY_PATIENT"];







            //    dvgRow


        }

        private void cmdGetEvents_Click(object sender, EventArgs e)
        {
            using (RunViaVBS GE = new RunViaVBS())
            {
                GE.ConnectionString = _ConnectionString;
                _EventDictonary = GE.getPatientEvents("pid", "hospcode");
            }
        }


        private void LoadHospCodes()
        {


            using (DCSGlobal.Rules.DCSGlobal.Rules.DataSets ds = new DCSGlobal.Rules.DCSGlobal.Rules.DataSets())
            {
                ds.ConnectionString = _ConnectionString;
                _HospCodes = ds.GetHospCodes(txtUserID.Text);
            }



            foreach (string hosp_code in _HospCodes)
            {
                cmbHospCode.Items.Add(hosp_code);
            }



        }

        private void cmdVerifyRule_Click(object sender, EventArgs e)
        {
            if (chkVBS.Checked)
            {

                using (RunViaVBS vbs = new RunViaVBS())
                {
                    txtRuleOutputVBS.Text = vbs.testRulesFireCall(fctRuleText.Text);

                }
            }

            int r = 0;

            if (chkVB.Checked)
            {

                Dictionary<int, int> _RuleResults = new Dictionary<int, int>();
                //    DCSGlobal.Rules.DCSGlobal.Rules.GeneratedDLL.modFireRulesAddrVB
                //using (DCSGlobal.Rules.DCSGlobal.Rules.GeneratedDLL.modFireRulesAddrVB3 vb = new DCSGlobal.Rules.DCSGlobal.Rules.GeneratedDLL.modFireRulesAddrVB3())
                //{

                //    //if (_ds_getDummyPatientDataPatient.Tables.Count > 0)
                //    //{
                //    //    DataTable dtPatientData = new DataTable(_ds_getDummyPatientDataPatient.Tables[0].ToString());

                //    foreach (DataTable dtPatientData in _ds_getDummyPatientDataPatient.Tables)
                //    {

                //        foreach (DataRow row in dtPatientData.Rows)
                //        {

                //            vb.DR = row;
                //            r = vb.RunRules();
                //            _RuleResults = vb.RuleResults;
                //        }


                //        if (_RuleResults.ContainsKey(_RuleID))
                //        {
                //            int value = _RuleResults[1001];
                //            txtRuleOutputVB.Text = Convert.ToString(value);
                //        }


                //        // foreach (DataRow drPatient in dtPatientData) ;
                //    }

                //}

                //     ////            ee = r.LNAME(txtRuleDescription.Text);

                //     ////            txtRuleName.Text = Convert.ToString(ee);
                //     string DLL = string.Empty;

                //     DLL = lblRuleDLLName.Text;
                // //    System.Reflection.Assembly myDllAssembly = System.Reflection.Assembly.LoadFile(DLL);


                ////     Type clsType = myDllAssembly.GetType("DCSGlobal.Rules.DCSGlobal.Rules.GeneratedDLL.modFireRulesAddrVB");

                //     ////Create the instance of the class
                //   //  object clsInstance = Activator.CreateInstance(clsType, null);

                //     ////Calls the WriteDefault method of AClass
                //     ////clsType.InvokeMember("GO", BindingFlags.InvokeMethod, null, clsInstance, null);

                //     ////Calls the  methods
                //     //Object ret = clsType.InvokeMember("RunRule", BindingFlags.InvokeMethod, null, clsInstance, new object[] { txtRuleDescription.Text });

                //     //txtRuleName.Text = Convert.ToString(ret);


                //     var mDLL = Assembly.LoadFile(DLL);

                //     foreach (Type type in mDLL.GetExportedTypes())
                //     {
                //         var c = Activator.CreateInstance(type);
                //        // type.InvokeMember("Output", BindingFlags.InvokeMethod, null, c, new object[] { @"Hello" });
                //     }


                //     foreach (Type type in mDLL.GetExportedTypes())
                //     {
                //         dynamic c = Activator.CreateInstance(type);
                //        // c.Output(@"Hello");
                //     }

            }



        }

        private void cmdSearch_Click_1(object sender, EventArgs e)
        {
            GetRules();
        }

        private void cmdBuildVBS_Click(object sender, EventArgs e)
        {
            //using (RunViaVBS vbs = new RunViaVBS())
            //{
            //    fctRuleText.Text = vbs.PrepRuleVBS(fctRULE_VBS.Text, _ds_getDummyPatientDataPatient);
            //}


        }

        private void RuleTestCompile_ResizeEnd(object sender, EventArgs e)
        {
            Resize();

        }






        private void RuleTestCompile_SizeChanged(object sender, EventArgs e)
        {






        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            Resize();
        }

        private void Resize()
        {

            int _height = this.Height;
            int _free_height = splitContainer1.Height - 26;
            int _width = this.Width;

            int _tbcRuleResults_Height = tbcRuleResults.Height;

            int _tbcRuleResults_Top = tbcRuleResults.Top;




            splitContainer1.Width = (_width - 52);
            dataGridView2.Width = (_width - pnlSearch.Width - 50);
            pnlSearch.Left = dataGridView2.Width + 30;
            txtRuleMessage.Width = _width - 63;
            splitContainer1.Height = (_height - (_tbcRuleResults_Height) - (splitContainer1.Top + 100));
            tbcRuleResults.Top = splitContainer1.Bottom + 10;
            txtRuleName.Width = (_width - txtRuleName.Left - 40);











            // fctRULE_VBS    --vbs top raw rule
            // fctRuleText    --


            //fctRULE_VB    -- this the top on VB
            //fctRULE_VB_DLL    -- this the top on VB

            int _PanelHeight = splitContainer1.Panel2.Height;
            int _PanelSplitHeight = 1;

            int _Panel1Width = splitContainer1.Panel1.Width;
            int _Panel2Width = splitContainer1.Panel2.Width;



            int _top1 = 50;
            int _top2 = 0;

            try
            {

                _top2 = (_PanelHeight - _top1) / 2;

            }
            catch
            {

            }

            _PanelSplitHeight = ((_PanelHeight - _top1) / 2) - 25;

            fctRULE_VBS.Width = splitContainer1.Panel1.Width - 20;
            fctRULE_VBS.Height = _PanelSplitHeight;
            fctRULE_VBS.Top = _top1;
            fctRULE_VBS.BackColor = System.Drawing.Color.Beige;

            fctRuleText.Width = splitContainer1.Panel1.Width - 20;
            fctRuleText.Height = _PanelSplitHeight;
            fctRuleText.Top = _top2 + 50;

            fctRULE_VB.Left = 10;
            fctRULE_VB.Width = splitContainer1.Panel2.Width - 20;
            fctRULE_VB.Height = _PanelSplitHeight;
            fctRULE_VB.Top = _top1;

            fctRULE_VB_DLL.Left = 10;
            fctRULE_VB_DLL.Width = splitContainer1.Panel2.Width - 20;
            fctRULE_VB_DLL.Height = _PanelSplitHeight;
            fctRULE_VB_DLL.Top = _top2 + 50;
        }

        private void dvgRow_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lstVBErrorList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                String text = lstVBErrorList.SelectedItems[0].Text;


                int _Line = 1;

                text = ss.ParseDemlimtedString(text, ":", 2);

                text = ss.ParseDemlimtedString(text, ".vb", 2);

                text = text.Replace("(", "");

                text = text.Replace(")", "");

                text = ss.ParseDemlimtedString(text, ",", 1);

                _Line = Convert.ToInt32(text);

                fctRULE_VB_DLL.Navigate(_Line);

            }
            catch
            {


            }


            // C:\Users\ghinckley\AppData\Local\Temp\ouveatl2.0.vb(6013,0) : error BC30183: Keyword is not valid as an identifier.


            //  string text = lstVBErrorList.GetItemText(lstVBErrorList.SelectedItem);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BuildAllRules b = new BuildAllRules();
            b.ConnectionString = _ConnectionString;
            b.Go();
        }

        private void cmdLoad_Click(object sender, EventArgs e)
        {
            byte[] b = File.ReadAllBytes(lblRuleDLLName.Text);
            Assembly a = Assembly.Load(b);

            List<RuleResult> rr = new List<RuleResult>();

            Assembly assembly = Assembly.LoadFrom(lblRuleDLLName.Text);
            Type type = assembly.GetType("DCSGlobal.Rules.GeneratedDLL.modFireRulesAddrVB");
            object instance = Activator.CreateInstance(type); //creates an instance of that class


            PropertyInfo RuleMessages_PropertyInfo = type.GetProperty("RuleMessages");
            PropertyInfo RuleResults_PropertyInfo = type.GetProperty("RuleResults");
            PropertyInfo DR_PropertyInfo = type.GetProperty("DR");

           

            Dictionary<int, string> RuleMessages = (Dictionary<int, string>)RuleMessages_PropertyInfo.GetValue(instance, null);


                foreach (DataRow row in _ds_getDummyPatientDataPatient.Tables["DUMMY_PATIENT"].Rows)
                {

                    DR_PropertyInfo.SetValue(instance, row, null);
                    object res = type.GetMethod("RunRules").Invoke(instance, new object[] { });
                    Dictionary<int, int> RuleResults = (Dictionary<int, int>)RuleResults_PropertyInfo.GetValue(instance, null);



                    foreach (KeyValuePair<int, int> rule in RuleResults)
                    {
                        RuleResult _rr = new RuleResult();
                        string MsgReturnVal = string.Empty;
                        _rr.RuleID = rule.Key;
                        _rr.RuleReturn = rule.Value;

                        RuleMessages.TryGetValue(rule.Key, out MsgReturnVal);

                        _rr.RuleMessage = MsgReturnVal;


                        rr.Add(_rr);
                    
                    }
                    
                    
                    
                    txtRuleOutputVB.Text = Convert.ToString(res);
                
                }

        }
    }
}
