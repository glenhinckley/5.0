
using System;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using System.Data.SqlClient;
using FastColoredTextBoxNS;

using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using DCSGlobal.BusinessRules.CoreLibraryII;
using DCSGlobal.BusinessRules.CoreLibrary;
using DCSGlobal.BusinessRules.Logging;

using DCSGlobal.EDI.Comunications;
using DCSGlobal.BusinessRules.CoreLibrary.dbCheckStuff;

using DCSGlobal.Rules.TestDLL.RuleEngine;
using DCSGlobal.Rules.DLLBuilder;

namespace Manual_test_app.Rules
{
    public partial class RuleConverterVBStoVB : Form
    {
        public RuleConverterVBStoVB()
        {
            InitializeComponent();
        }



        private int top = 100;
        private string _ConnectionString = "Data Source=10.1.1.120;Initial Catalog=al60_eastmaine_prod;Persist Security Info=True;User ID=al60_seton_lite_developer_user;Password=al60_seton_lite_developer_password";
        private string dbTempString = string.Empty;
        private string select = string.Empty;
        StringStuff ss = new StringStuff();
        CodeInjection ci = new CodeInjection();


        private void RuleEditor_Load(object sender, EventArgs e)
        {
            select = "SELECT TOP " + top + " * FROM RULE_DEF";
            txtConstring.Text = _ConnectionString;
            txtSQL.Text = select;
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer1_Panel2_SizeChanged(object sender, EventArgs e)
        {
            fctRULE_VBS.Width = splitContainer1.Panel1.Width;
            //   lblValidateREQ.Width = splitContainer1.Panel1.Width - 126;
            fctRULE_VB.Width = splitContainer1.Panel2.Width -5;
            //   lblValidateRES.Width = splitContainer1.Panel2.Width - 126;

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cmdSearch_Click(object sender, EventArgs e)
        {

        }

        private void cmdCheckPoint_Click(object sender, EventArgs e)
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


                select = txtSQL.Text;



                cii = ci.CheckInput(select);

                if (cii == 0)
                {

                    _ConnectionString = txtConstring.Text;
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
                        fctRULE_VB.Text = string.Empty;
                        fctRULE_VBS.Text = string.Empty;
                        string RULE_DEF = string.Empty;



                        RULE_DEF = row.Cells["RULE_DEF"].Value.ToString();
                        // REQ = row.Cells["REQ"].Value.ToString();

                        string RULE_VB = string.Empty;

                        fctRULE_VBS.Language = Language.VB;


                        fctRULE_VBS.Text = RULE_DEF;
                        //  fctRULE_VB.Text = RULE_VB;


                        txtRuleID.Text = row.Cells["rule_id"].Value.ToString();
                        txtRuleName.Text = row.Cells["rule_name"].Value.ToString();
                        txtRuleDescription.Text = row.Cells["rule_description"].Value.ToString();


                        if (chkAutoParse.Checked)
                        {
                            Parse();

                        }

                        //lblREQTimeStamp.Text = row.Cells["ST"].Value.ToString();
                        //lblRESTimeStamp.Text = row.Cells["ST"].Value.ToString();
                        //txtVendor.Text = row.Cells["VENDOR_NAME"].Value.ToString();



                        //    Temp = row.Cells["ExceptionStackTrace"].Value.ToString();


                        //      bool contains = Temp.Contains("DCS_ED_LOG");


                        //if (contains)
                        //{

                        //    PrefixData = ss.ParseDemlimtedString(Temp, "=", 1);
                        //    Comment = ss.ParseDemlimtedString(Temp, "=", 2);


                        //    lblPFV.Text = ss.ParseDemlimtedString(Temp, "|", 1);

                        //    lblHN.Text = ss.ParseDemlimtedString(Temp, "|", 2);
                        //    lblAN.Text = ss.ParseDemlimtedString(Temp, "|", 3);
                        //    txtPath.Text = ss.ParseDemlimtedString(Temp, "|", 4);
                        //    lblIP.Text = ss.ParseDemlimtedString(Temp, "|", 5);
                        //    txtESM.Text = ss.ParseDemlimtedString(Temp, "|", 6); ;
                        //    txtES.Text = ss.ParseDemlimtedString(Temp, "|", 6); ;
                        //}
                        //else
                        //{
                        //    txtESM.Text = Temp;
                        //    txtES.Text = Temp;


                        //    lblPFV.Text = "Prefix Data Not Found";

                        //    lblHN.Text = "";
                        //    lblAN.Text = "";
                        //    txtPath.Text = "";
                        //    lblIP.Text = "";
                        //    //txtC.Text = Temp;
                        //}

                    }
                    catch (Exception ex)
                    {
                        toolStripStatusLabel1.Text = ex.Message;
                    }

                }


            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cmdTestRule_Click(object sender, EventArgs e)
        {


            //            Assembly assembly = Assembly.LoadFrom("MyNice.dll");

            //Type type = assembly.GetType("MyType");

            //object instanceOfMyType = Activator.CreateInstance(type);

            //            int ee = 0;

            //            RuleEngine r = new RuleEngine();


            //            ee = r.LNAME(txtRuleDescription.Text);

            //            txtRuleName.Text = Convert.ToString(ee);
            string DLL = string.Empty;

            DLL = "C:\\temp\\DCSGlobal.Rules.TestDLL.dll";
            System.Reflection.Assembly myDllAssembly = System.Reflection.Assembly.LoadFile(DLL);


            Type clsType = myDllAssembly.GetType("DCSGlobal.Rules.TestDLL.RuleEngine.Go");

            //Create the instance of the class
            object clsInstance = Activator.CreateInstance(clsType, null);

            //Calls the WriteDefault method of AClass
            //clsType.InvokeMember("GO", BindingFlags.InvokeMethod, null, clsInstance, null);

            //Calls the  methods
            Object ret = clsType.InvokeMember("RunRule", BindingFlags.InvokeMethod, null, clsInstance, new object[] { txtRuleDescription.Text });

            txtRuleName.Text = Convert.ToString(ret);


        }

        private void Parse()
        {

            using (RuleParser p = new RuleParser())
            {
                p.rule_name = txtRuleName.Text;
                p.rule_description = txtRuleDescription.Text;
                p.rule_id = txtRuleID.Text;

                p.ParseRuleVBS(fctRULE_VBS.Text);


                fctRULE_VB.Text = p.RuleVB;
            }

        }


        private void cmdPtoVB_Click(object sender, EventArgs e)
        {

            
        }

        private void RuleConverterVBStoVB_ResizeEnd(object sender, EventArgs e)
        {
            int _height = this.Height;
            int _width = this.Width;

            splitContainer1.Width = (_width - 52);
            dataGridView2.Width = (_width - 52);

            splitContainer1.Height = (_height -  splitContainer1.Top  - 50);


            fctRULE_VBS.Width = splitContainer1.Panel1.Width;
            fctRULE_VBS.Height  = splitContainer1.Panel1.Height-10;
            //   lblValidateREQ.Width = splitContainer1.Panel1.Width - 126;
            fctRULE_VB.Width = splitContainer1.Panel2.Width - 10;
            fctRULE_VB.Height = splitContainer1.Panel2.Height - 10;
            //   lblValidateRES.Width = splitContainer1.Panel2.Width - 126;

            txtRuleName.Width = (_width - txtRuleName.Left - 40);
            txtRuleDescription.Width = (_width - txtRuleName.Left - 40);
            pnlSearch.Left = _width - 130;
            pnlParserCMDGroup.Left = _width - 420;
            txtSQL.Width = pnlSearch.Left - 40;
        }

        private void RuleConverterVBStoVB_SizeChanged(object sender, EventArgs e)
        {
            RuleConverterVBStoVB_ResizeEnd(sender, e);
        }

        private void cmdSearch_Click_1(object sender, EventArgs e)
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


                select = txtSQL.Text;



                cii = ci.CheckInput(select);

                if (cii == 0)
                {

                    _ConnectionString = txtConstring.Text;
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

        private void cmdPtoVB_Click_1(object sender, EventArgs e)
        {
            Parse();
        }

        private void cmdBuildDll_Click(object sender, EventArgs e)
        {
           // BuildDllStringOutput f = new BuildDllStringOutput();
          //  f.Show();

        }

        private void cmdTestRule_Click_1(object sender, EventArgs e)
        {

        }

        private void cmdCompileRule_Click(object sender, EventArgs e)
        {
            BuildAssemballyFromString compiler = new BuildAssemballyFromString();
            compiler.Go(fctRULE_VB.Text);


        }
    }
}
