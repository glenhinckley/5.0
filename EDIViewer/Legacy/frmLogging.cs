using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DCSGlobal.BusinessRules.Logging;

namespace Manual_test_app
{
    public partial class frmLogging : Form
    {

        int i = 0;

        public frmLogging()
        {
            InitializeComponent();
        }

        private void frmLogging_Load(object sender, EventArgs e)
        {
            string c = "Data Source=10.1.1.175;Initial Catalog=al60_seton_lite_developer;Persist Security Info=True;User ID=suresh;Password=suresh247";

        }

        private void button1_Click(object sender, EventArgs e)
        {
                    string c = "Data Source=10.1.1.175;Initial Catalog=al60_seton_lite_developer;Persist Security Info=True;User ID=suresh;Password=suresh247";

                    using (SchedulerLog _SL = new SchedulerLog())
            {
                

                
                _SL.ConnectionString = c;

              textBox1.Text = Convert.ToString(  _SL.AddLogEntry("adfdsafasdf", "adsfasdfadsfsadfasddfasddf"));



            }






        }

        private void button2_Click(object sender, EventArgs e)
        {
            string c = "Data Source=10.1.1.175;Initial Catalog=al60_seton_lite_developer;Persist Security Info=True;User ID=suresh;Password=suresh247";

            using (SchedulerLog _AL = new SchedulerLog())
            {



                _AL.ConnectionString = c;

                _AL.UpdateLogEntry("adfdsafasdf", "This the end",Convert.ToInt32(textBox1.Text));
            }

        }
    }
}
