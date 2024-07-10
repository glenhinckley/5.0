using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DCSGlobal.BusinessRules.CoreLibraryII;

namespace Manual_test_app
{
    public partial class frmInpersonate : Form
    {
        public frmInpersonate()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            using (Impersonation imp = new Impersonation())
            {
                imp.DomainName = textBox1.Text;
                imp.UserName = textBox2.Text;
                imp.Password = textBox3.Text;
                imp.Main();
              
            }
          



        }
    }
}
