using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DCSGlobal.BusinessRules.CoreLibrary;
using DCSGlobal.BusinessRules.CoreLibraryII;



namespace Manual_test_app
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            DCSEncrypt d = new DCSEncrypt();

//            label1.Text = d.DecryptPasswd(textBox1.Text, "&%#@?,:*");

        }

        private void button1_Click(object sender, EventArgs e)
        {

            string r = string.Empty; 

            using(Authenticate a = new Authenticate())
            {
                bool b;
            //    b = a.AuthenticateADSSL(txtUserName.Text, txtPasswd.Text, txtDomain.Text, txtSRV.Text);

                //if (b)
                //{ label1.Text = "t"; }
                //else
                //{ label1.Text = "f"; }

                //label2.Text = a.Error;




            }
        }
    }
}
