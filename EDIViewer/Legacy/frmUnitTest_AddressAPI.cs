using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DCSGlobal.Rules.FireRules;

namespace Manual_test_app
{
    public partial class frmUnitTest_AddressAPI : Form
    {


        modFireRulesAddr s = new modFireRulesAddr();


        public frmUnitTest_AddressAPI()
        {
            InitializeComponent();
        }

        private void frmUnitTest_AddressAPI_Load(object sender, EventArgs e)
        {

        }

        private void cmdSwap_Click(object sender, EventArgs e)
        {




                 s.ValidateAddressBySwappingAddressFields(1,"sdfsdfsdfsdf", "5812 victor", "", "Dallas","TX","75093",
                                                            "stjoe","HqKKW2UGhzF5Xm6RhbL+vg==", 1231231, 234234,  "c",24234234);
                                                            
                                                            
      




        }






















    }
}
