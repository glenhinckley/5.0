using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;


using DCSGlobal.EDI;
using DCSGlobal.BusinessRules.Logging;

namespace Manual_test_app
{
    public partial class frmFileFix : Form
    {
        public frmFileFix()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {


            FilePrep fp = new FilePrep();

            //  FilePrep ff = new FileFix835();

          //  int size = -1;
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {

                textBox1.Text = openFileDialog1.FileName;
                //  txtOutFile.Text = openFileDialog1.FileName;

                string file = openFileDialog1.FileName;
                try
                {



                    fp.SingleFile(textBox1.Text, "C:\\usr\\fixed.837");


                }
                catch (IOException)
                {
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {


            ValidateEDI fp = new ValidateEDI();

            //  FilePrep ff = new FileFix835();

           // int size = -1;
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {

                textBox2.Text = openFileDialog1.FileName;
                //  txtOutFile.Text = openFileDialog1.FileName;

                string file = openFileDialog1.FileName;
                try
                {

                 
                    fp.byFile( "" , textBox2.Text, "EDI");


                }
                catch (IOException)
                {
                }


            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            string c = "Data Source=10.1.1.175;Initial Catalog=al60_seton_lite_dev2;Persist Security Info=True;User ID=suresh;Password=suresh247";


            SchedulerLog sl = new SchedulerLog();


            sl.ConnectionString = c;

            
          // label1.Text = Convert.ToString(sl.LogEntry(textBox3.Text, textBox4.Text));

        }

        private void button4_Click(object sender, EventArgs e)
        {


            StringPrep s = new StringPrep();
            s.SingleEDI(textBox5.Text);


        }

        private void frmFileFix_Load(object sender, EventArgs e)
        {

            string sTimeStamp;
            sTimeStamp = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:fffZ");

            label2.Text = sTimeStamp;









        }




    }
}
