
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
    public partial class BuildDllStringOutput : Form
    {
        public BuildDllStringOutput()
        {
            InitializeComponent();
        }

        private string _ConnectionString = "Data Source=10.1.1.120;Initial Catalog=al60_eastmaine_prod;Persist Security Info=True;User ID=al60_seton_lite_developer_user;Password=al60_seton_lite_developer_password";



        private void BuildDllStringOutput_Load(object sender, EventArgs e)
        {


            using (BuildDLLString p = new BuildDLLString())
            {

                p.BuildAllRules();

                fctRULE_VB.Text = p.VBString;
            }

        }
    }
}
