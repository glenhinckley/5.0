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
    public partial class frm277 : Form
    {
        public frm277()
        {
            InitializeComponent();

            string c = string.Empty;

            c = "Persist Security Info=False;User ID=al60_seton_lite_developer_user;Password=al60_seton_lite_developer_password;Initial Catalog=al60_seton_lite_developer;Data Source=10.1.1.120;pooling=false;connection Timeout=180";

            txtConString.Text = c;//"Data Source=10.1.1.175;Initial Catalog=al60_seton_lite_developer;Persist Security Info=True;User ID=suresh;Password=suresh247";


            txtCEBRId.Text = "42";

        }

        private void frmEDI277Insert_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {





            StringPrep SP = new StringPrep();
           
            List<string> list = new List<string>();

            EDI_5010_277_005010X212_v2 imp = new EDI_5010_277_005010X212_v2();

            StringStuff ss = new StringStuff();    
 
            imp.ebr_id =  Convert.ToDouble(txtCEBRId.Text);

            imp.ConnectionString = txtConString.Text;

            imp.LOG_EDI = "Y";


            list = SP.SingleEDI(txtEDI.Text);


            lst277.DataSource = list;
       
            imp.ImportByString(txtEDI.Text, Convert.ToDouble(txtCEBRId.Text));



            DateTime Start = DateTime.Now;
            DateTime End;

             //ByVal EDI_277 As String, ByVal DELETE_FLAG_277 As String, _
             //                               ByVal CBR_ID As String, ByVal USER_ID As String, ByVal HOSP_CODE As String, ByVal SOURCE As String,
             //                               ByVal PAYOR_ID As String, ByVal VENDOR_NAME As String, _
             //                               ByVal LOG_EDI As String, ByVal BatchID As Int64


            double i = 0;



           // imp.EDI


          //  i = p.Import277(textBox1.Text, "N", Convert.ToString(8888), "auditlogix", "510", "SOURCE_AL", "00059", "MEDDATA", "Y", Convert.ToDouble(txtBatchID.Text));


           /// label1.Text = Convert.ToString(i);

           /// i277.STC;
             dgISA.DataSource = imp.ISA;
             dgGS.DataSource = imp.GS;
             dgST.DataSource = imp.ST;
             dgBHT.DataSource = imp.BHT;
             dgTRN.DataSource = imp.TRN;
             dgHL.DataSource = imp.HL;
             dgNM1.DataSource = imp.NM1;
             dgSTC.DataSource = imp.STC;


             dgREF.DataSource = imp.REF;
            // dgTS3.DataSource = imp.TS3;
            // dgTS2.DataSource = imp.TS2;
            // dgCLP.DataSource = imp.CLP;
            // dgCAS.DataSource = imp.CAS;



             dgDTP.DataSource = imp.DTP;
            // dgDMG.DataSource = imp.DMG;
            // dgMOA.DataSource = imp.MOA;
            // dgQTY.DataSource = imp.QTY;
            // dgCAS.DataSource = imp.CAS;

             dgSVC.DataSource = imp.SVC;
            // dgPLB.DataSource = imp.PLB;
            // dgAMT.DataSource = imp.AMT;
            // dgUNK.DataSource = imp.UNK;


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtConString_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtBatchID_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtEDI_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
