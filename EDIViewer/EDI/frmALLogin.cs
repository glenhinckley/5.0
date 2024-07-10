using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DCSGlobal.BusinessRules.Security;

using DCSGlobal.BusinessRules.CoreLibraryII;
using DCSGlobal.BusinessRules.CoreLibrary;

namespace Manual_test_app
{
    public partial class frmALLogin : Form
    {
        public frmALLogin()
        {
            InitializeComponent();
        
        
        
        }





        DCSEncrypt d = new DCSEncrypt();


        private void cmdLogin_Click(object sender, EventArgs e)
        {
            int r = -1;
            string dbTempString = string.Empty;
            using (User u = new User())
            {
               
                u.CommandString = Settings1.Default.ALLoginCommandString;



                using (DBUtility dbu = new DBUtility())
                {
                    dbTempString = dbu.getConnectionString(Settings1.Default.ConString);
                }
                u.ConnectionString = dbTempString;
               r = u.checkValidUserAl(txtUserName.Text, txtPasswd.Text, "", "");
               
                if (r != 0)
                {

                    lblError.Text = "Fail";
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {


   
            lblError.Text = d.DecryptPasswd(txtEncrypt.Text, "&%#@?,:*");
            txtPlain.Text = d.DecryptPasswd(txtEncrypt.Text, "&%#@?,:*");
        }

        private void button2_Click(object sender, EventArgs e)
        {


            lblError.Text = d.EncryptPasswd(txtPlain.Text, "&%#@?,:*");

        }

        private void frmALLogin_Load(object sender, EventArgs e)
        {

        }
    }
}
