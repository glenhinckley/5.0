using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using DCSGlobal.EDI;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;


namespace Manual_test_app
{
    public partial class frm835 : Form
    {

        //FileFix835 ff = new FileFix835();
        Import835 imp = new Import835();
       // Import837i imp = new Import837i();

        StringStuff ss = new StringStuff();

        DateTime Start = DateTime.Now;
        DateTime End;


        public frm835()
        {
            InitializeComponent();

             
           //txtConString.Text = "Data Source=10.34.1.130;Initial Catalog=al60_appalachian_prod;Persist Security Info=True;User ID=suresh;Password=suresh247";
            txtConString.Text = "Data Source=10.1.1.120;Initial Catalog=al60_seton_lite_developer;Persist Security Info=True;User ID=al60_seton_lite_developer_user;Password=al60_seton_lite_developer_password";

          //  txtConString.Text = "Data Source=10.34.1.130;Initial Catalog=al60_waterbury;Persist Security Info=True;User ID=suresh;Password=suresh247";


          //  txtConString.Text = "Data Source=10.34.1.150;Initial Catalog=al60_faith_prod;Persist Security Info=True;User ID=suresh;Password=suresh247";
  
            imp.ConnectionString = txtConString.Text;

        } 
        
        


private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
{
   var percent = e.ProgressPercentage;
   //update progress bar here
}
        
        
        private void button1_Click(object sender, EventArgs e)
        {
            int size = -1;
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {

                txtInFile.Text = openFileDialog1.FileName;
                txtOutFile.Text = openFileDialog1.FileName;

                string file = openFileDialog1.FileName;
                try
                {
                    string text = File.ReadAllText(file);
                    rtRaw835.Text = text;
                }
                catch (IOException)
                {
                }
            }
        }

        private void cmdFileFix_Click(object sender, EventArgs e)
        {
            int i = 0;

           //i = ff.Go(txtInFile.Text, txtOutFile.Text);

           lblFF.Text = Convert.ToString(i);
            if (i > 1)
            {

                try
                {
                    string text = File.ReadAllText(txtOutFile.Text);
                    //rtR.Text = text;
             
                    rtxFixed835.Text =    text;
                  
                }
                catch (IOException)
                {
                }


            }
        }

        private void cmdParse_Click(object sender, EventArgs e)
        {
            int RE;

          Start = DateTime.Now;


          imp.ConnectionString = "Data Source=10.1.1.120;Initial Catalog=al60_seton_lite_developer;Persist Security Info=True;User ID=al60_seton_lite_developer_user;Password=al60_seton_lite_developer_password"; //txtConString.Text;
            imp.BatchID = Convert.ToDouble(200);
 
          RE =   imp.Import(txtOutFile.Text);

           // dgISA.DataSource = imp.ISA;
           // dgGS.DataSource = imp.GS;
           // dgST.DataSource = imp.ST;
           // dgBPR.DataSource = imp.BPR;
           // dgTRN.DataSource = imp.TRN;
           // dgN1.DataSource = imp.N1;
           // dgN3.DataSource = imp.N3;
           // dgN4.DataSource = imp.N4;
           // dgLX.DataSource = imp.LX;


           //// dgREF.DataSource = imp.REF;
           // dgTS3.DataSource = imp.TS3;
           // dgTS2.DataSource = imp.TS2;
           // dgCLP.DataSource = imp.CLP;
           // dgCAS.DataSource = imp.CAS;



           // dgDTM.DataSource = imp.DTM;
           // dgMIA.DataSource = imp.MIA;
           // dgMOA.DataSource = imp.MOA;
           // dgQTY.DataSource = imp.QTY;
           // dgCAS.DataSource = imp.CAS;

           // dgSVC.DataSource = imp.SVC;
           // dgPLB.DataSource = imp.PLB;
           // dgAMT.DataSource = imp.AMT;
           // dgUNK.DataSource = imp.UNK;

       
        
                    
                End = DateTime.Now;
               // log.ExceptionDetails("DCSGlobal.EDI.FileFix.Go", "### Overall End Time: " + [end].ToLongTimeString() + " " + InFile)
            label1.Text = "### Overall Run Time: " + Convert.ToString(End - Start);
            label3.Text = Convert.ToString(RE);
        
        
        }

        private void EDI835_Load(object sender, EventArgs e)
        {
            txtOutFile.Text = @"C:\usr\835.txt";

           // txtOutFile.Text = @"C:\usr\837.test";
        }

        private void txtConString_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtInFile_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblFF_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void txtOutFile_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void rtRaw835_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
    }
}
