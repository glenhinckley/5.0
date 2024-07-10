using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DCSGlobal.EDI;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;

namespace Manual_test_app
{
    public partial class frm276 : Form
    {
        public frm276()
        {
            InitializeComponent();

            txtConString.Text = "Data Source=10.1.1.120;Initial Catalog=al60_seton_lite_developer;Persist Security Info=True;User ID=al60_seton_lite_developer_user;Password=al60_seton_lite_developer_password";

          //  txtConString.Text = "Data Source=10.1.1.175;Initial Catalog=al60_seton_lite_developer;Persist Security Info=True;User ID=suresh;Password=suresh247";

        }

        private void button1_Click(object sender, EventArgs e)
        {



      //      EDI_5010_PARSE p = new EDI_5010_PARSE();

            StringStuff ss = new StringStuff();

            DateTime Start = DateTime.Now;
            DateTime End;




            double i = 0;

            EDI_5010_276_005010X212_DCS imp = new EDI_5010_276_005010X212_DCS();

            imp.ConnectionString = txtConString.Text;


            imp.BatchID = Convert.ToDouble("9876");

            imp.ConnectionString = txtConString.Text;

          //  list = SP.SingleEDI(txtEDI.Text);

            imp.ImportByString(txtRaw276.Text, Convert.ToDouble("9876"));

          //  i = imp.Import276(textBox1.Text, "N", Convert.ToDouble(7), "auditlogix", "510", "SOURCE_276", "00059", "MEDDATA", "1", "00027222629", "00027222629-510", 0);

            label1.Text = Convert.ToString(i);


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtConString_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
