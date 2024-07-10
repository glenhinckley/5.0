using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DCSGlobal.EDI;

namespace Manual_test_app
{
    public partial class frmEDIValidate : Form
    {
        public frmEDIValidate()
        {
            InitializeComponent();  
          
        }
          ValidateEDI v = new ValidateEDI();
        private void button1_Click(object sender, EventArgs e)
        {

            v.byString(textBox1.Text);

                





        }
    }
}
