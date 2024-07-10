using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using System.Configuration;

using System.Data.SqlClient;
using System.Diagnostics;

using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using DCSGlobal.BusinessRules.CoreLibraryII;
using DCSGlobal.BusinessRules.CoreLibrary;
using DCSGlobal.BusinessRules.Logging;
using DCSGlobal.BusinessRules.CoreLibrary.dbCheckStuff;


namespace Manual_test_app.DEPLOYMENT
{
    public partial class frmNewPackage : Form
    {
        private int _PackageID = 0;

        private string _ConnectionStringPackage = "Integrated Security=SSPI;Initial Catalog=glen_db;Data Source=10.1.1.120";

        public frmNewPackage()
        {
            InitializeComponent();
        }

        private void cndSaveNewPackage_Click(object sender, EventArgs e)
        {
            string insert = string.Empty;

            Newpackage();
        }


        private int Newpackage()
        {



            int r = -1;
            string insert = string.Empty;

            insert = "insert into PACKAGES (PACKAGE_NAME,PACKAGE_DESCRIPTION) values ('" + txtPackageName.Text + "', '" + txtPackageDescription.Text + "'); SELECT SCOPE_IDENTITY()";

            using (SqlConnection con = new SqlConnection(_ConnectionStringPackage))
            {
                con.Open();
                //
                // Create new SqlCommand object.
                //
                using (SqlCommand cmd = new SqlCommand(insert, con))
                {

                    cmd.CommandType = CommandType.Text;

                    r = Convert.ToInt32(cmd.ExecuteScalar());

                }
            }
            _PackageID = r;


            cmdBuildPackage.Enabled = true;
            return r;
        }

        private void frmNewPackage_Load(object sender, EventArgs e)
        {


        }

        private void cmdBuildPackage_Click(object sender, EventArgs e)
        {
            frmPackageObjects fp = new frmPackageObjects(_PackageID, txtPackageName.Text  );
            fp.Show();
        }
    }
}
