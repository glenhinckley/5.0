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
    public partial class IDTest : Form
    {
        public IDTest()
        {
            InitializeComponent();
        }

        private void IDTest_Load(object sender, EventArgs e)
        {


            MachineID md = new MachineID();

            label1.Text = md.CPUID();
            label2.Text = md.DiscID();
            label3.Text = md.MotherBoardID();

        }
    }
}
