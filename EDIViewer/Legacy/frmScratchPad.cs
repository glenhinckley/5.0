
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

using DCSGlobal.Rules.DLLBuilder;


namespace Manual_test_app
{
    public partial class frmScratchPad : Form
    {

        private string _ConnectionString = "Data Source=10.1.1.120;Initial Catalog=al60_seton_lite_developer;Persist Security Info=True;User ID=al60_seton_lite_developer_user;Password=al60_seton_lite_developer_password";


        public frmScratchPad()
        {
            InitializeComponent();
        }

        private void frmScratchPad_Load(object sender, EventArgs e)
        {





        }

        private void button1_Click(object sender, EventArgs e)
        {


            using (RuleParser p = new RuleParser())
            {
               // p.rule_name = "sdfsdf";
               // p.rule_description = "sdfsdf";
               // p.rule_id = "sdfsdf";

                p.ParseRuleVBS(textBox1.Text);


            }
        }
    }
}
