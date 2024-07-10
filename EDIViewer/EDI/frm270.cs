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
using System.Data.SqlClient;
using DCSGlobal.EDI.Comunications;

namespace Manual_test_app
{
    public partial class frm270 : Form
    {



        //Import270 imp270 = new Import270();
        //Import270 imp270 = new Import270();
        Import271 imp270 = new Import271();
  
        StringStuff ss = new StringStuff();

        Parse objP = new Parse();

        public frm270()
        {
            InitializeComponent();
        }

        private void cmdParse_Click(object sender, EventArgs e)
        {
            int re = -1;




            imp270.EDI = rtEDI.Text;
            imp270.ConnectionString = txtConString.Text;
            try
            {
                imp270.BatchID = Convert.ToDouble(txtBatchID.Text);
                re = imp270.Import();

                if (re == 0)
                {
                    dgEDI.DataSource = imp270.EDI_271;
                    dgENV.DataSource = imp270.ENVELOP;

                    dgISA.DataSource = imp270.ISA;
                    dgGS.DataSource = imp270.GS;
                    dgST.DataSource = imp270.ST;
                    dgBST.DataSource = imp270.BHT;

                    dgHL20.DataSource = imp270.HL01;
                    dgHL21.DataSource = imp270.HL02;
                    dgHL22.DataSource = imp270.HL03;
                    dgHL23.DataSource = imp270.HL04;
                    dgAAA.DataSource = imp270.AAA;

                    dgEB.DataSource = imp270.EB;
                    dgAMT.DataSource = imp270.AMT;
                    dgDMG.DataSource = imp270.DMG;
                    dgDTP.DataSource = imp270.DTP;
                    dgEQ.DataSource = imp270.EQ;
                    dgHSD.DataSource = imp270.HSD;
                    dgHL.DataSource = imp270.HL;
                    dgIII.DataSource = imp270.III;
                    dgINS.DataSource = imp270.INS;

                    dgcMSG.DataSource = imp270.CACHE_MSG;
                    dgtMSG.DataSource = imp270.DTdistinctDT;
                    dgMSG.DataSource = imp270.MSG;

                    dgN3.DataSource = imp270.N3;
                    dgN4.DataSource = imp270.meddd;
                    dgNM1.DataSource = imp270.NM1;
                    dgPER.DataSource = imp270.PER;
                    dgPRV.DataSource = imp270.PRV;
                    dgREF.DataSource = imp270.REF;
                    dgTRN.DataSource = imp270.TRN;

                    dgUNK.DataSource = imp270.UNK;
                    tblEDI.TabIndex = 1;
                    button2.Enabled = true;
                }
                tlsStatusReturn.Width = 200;
                tlsStatusReturn.Text = "return code: " + Convert.ToString(re);

                tlsTime.Width = 200;
                tlsTime.Text = "Total time: " + Convert.ToString(imp270.ExecutionTime.TotalSeconds.ToString("0.000000"));
                tlsLoopCount.Text = "Loop count: " + Convert.ToString(imp270.EBLoopCount);

                stlChars.Text = imp270.chars;
            }
            catch (Exception ex)
            {

                tblEDI.SelectTab("tbpERR");
                txtERR.Text = ex.Message;
                //    imp.BatchID = 0;
            }
        }
        private void cmdParse_Click1111()
        {


            int re = -1;





            imp270.EDI = rtEDI.Text;
            imp270.ConnectionString = txtConString.Text;
            try
            {
                imp270.BatchID = Convert.ToDouble(txtBatchID.Text);
                re = imp270.Import();

                if (re == 0)
                {
                    dgEDI.DataSource = imp270.EDI_271;
                    dgENV.DataSource = imp270.ENVELOP;

                    dgISA.DataSource = imp270.ISA;
                    dgGS.DataSource = imp270.GS;
                    dgST.DataSource = imp270.ST;
                    dgBST.DataSource = imp270.BHT;

                    dgHL20.DataSource = imp270.HL01;
                    dgHL21.DataSource = imp270.HL02;
                    dgHL22.DataSource = imp270.HL03;
                    dgHL23.DataSource = imp270.HL04;
                    dgAAA.DataSource = imp270.AAA;

                    dgEB.DataSource = imp270.EB;
                    dgAMT.DataSource = imp270.AMT;
                    dgDMG.DataSource = imp270.DMG;
                    dgDTP.DataSource = imp270.DTP;
                    dgEQ.DataSource = imp270.EQ;
                    dgHSD.DataSource = imp270.HSD;
                    dgHL.DataSource = imp270.HL;
                    dgIII.DataSource = imp270.III;
                    dgINS.DataSource = imp270.INS;

                    dgcMSG.DataSource = imp270.CACHE_MSG;
                    dgtMSG.DataSource = imp270.DTdistinctDT;
                    dgMSG.DataSource = imp270.MSG;

                    dgN3.DataSource = imp270.N3;
                    dgN4.DataSource = imp270.meddd;
                    dgNM1.DataSource = imp270.NM1;
                    dgPER.DataSource = imp270.PER;
                    dgPRV.DataSource = imp270.PRV;
                    dgREF.DataSource = imp270.REF;
                    dgTRN.DataSource = imp270.TRN;

                    dgUNK.DataSource = imp270.UNK;
                    tblEDI.TabIndex = 1;
                    button2.Enabled = true;
                }
                tlsStatusReturn.Width = 200;
                tlsStatusReturn.Text = "return code: " + Convert.ToString(re);

                tlsTime.Width = 200;
                tlsTime.Text = "Total time: " + Convert.ToString(imp270.ExecutionTime.TotalSeconds.ToString("0.000000"));
                tlsLoopCount.Text = "Loop count: " + Convert.ToString(imp270.EBLoopCount);

                stlChars.Text = imp270.chars;
            }
            catch (Exception ex)
            {

                tblEDI.SelectTab("tbpERR");
                txtERR.Text = ex.Message;
            //    imp.BatchID = 0;
            }

        }

        private void cmdHR_Click(object sender, EventArgs e)
        {
            string r;

            r = rtEDI.Text;
            r = ss.ReplaceRowDelimiter(r, "~");
            rtEDI.Text = r;


            r = rtREQ.Text;
            r = ss.ReplaceRowDelimiter(r, "~");
            rtREQ.Text = r;


            r = rtRES.Text;
            r = ss.ReplaceRowDelimiter(r, "~");
            rtRES.Text = r;

        }

        private void button2_Click(object sender, EventArgs e)
        {

            long i270 = 0;

            i270 = imp270.Comit();

            if (i270 == 0)
            {
                label3.Text = "Commit Success : " + imp270.ErrorMessage;

            }
            else
            {

                label3.Text = "Commit Error";
                tblEDI.SelectTab("tbpERR");
                txtERR.Text = imp270.ErrorMessage;

            }
        }

        private void frm270_Load(object sender, EventArgs e)
        {
         //   txtConString.Text = RES.ghinckley;


            //<add key="ConnStr" value="Persist Security Info=False;User ID=console_user;Password=INRJbRzBEAuHhwn5X7vm+gCiGlgFRadqITayysuPha0=;Initial Catalog=al60_eastmaine_prod;Data Source=DCSSQL2012;pooling=false;connection Timeout=180" />
            //txtConString.Text =""Persist Security Info=False;User ID=console_eastmaine_prod_user;Password=G2OkLYvwjP4lbrcgdYRorQ6O9CsRSdCnsuPu64CCCgA=;Initial Catalog=al60_eastmaine_prod;Data Source=10.34.1.190;pooling=false;connection Timeout=180"
            //<add key="ConnStr" value="Persist Security Info=False;User ID=console_eastmaine_prod_user;Password=G2OkLYvwjP4lbrcgdYRorQ6O9CsRSdCnsuPu64CCCgA=;Initial Catalog=al60_eastmaine_prod;Data Source=10.34.1.190;pooling=false;connection Timeout=180"/>
            //txtConString.Text = "Data Source=10.1.1.120;Initial Catalog=al60_eastmaine_prod;Persist Security Info=True;User ID=al60_seton_lite_developer_user;Password=al60_seton_lite_developer_password";
            // txtConString.Text = "Data Source=10.34.1.190;Initial Catalog=al60_eastmaine_prod;Persist Security Info=True;User ID=console_user;Password=console_password";
            //txtConString.Text = "Data Source=10.34.1.190;Initial Catalog=al60_dallas_children_prod;Persist Security Info=True;User ID=console_dallas_children_user;Password=console_dallas_children_password";


            txtConString.Text = "Persist Security Info=False;User ID=al60_seton_lite_developer_user;Password=al60_seton_lite_developer_password;Initial Catalog=al60_seton_lite_developer;Data Source=10.1.1.120;pooling=false;connection Timeout=180";



            //"Persist Security Info=False;User ID=console_dallas_children_user;Password=D/y5WJeUTwlhNL31C9ty4Q29P7BdEvz+8GW/rJb7adSzcQTGGKWXfL547IemVXlE;Initial Catalog=al60_dallas_children_prod;Data Source=10.34.1.190;pooling=false;connection Timeout=180"

        }

        private void button1_Click(object sender, EventArgs e)
        {


          //  PostNTrack p = new PostNTrack();


       //     rtRES.Text = p.ProcessPNT(rtREQ.Text);


        }

        private void tbpREQ_Click(object sender, EventArgs e)
        {
        
        }

        private void txtConString_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //using(Import270v2 i = new Import270v2())
            //{

            //    i.ConnectionString = txtConString.Text;
            //    i.ImportByString( rtEDI.Text, 42);




            //}
        }

 
    }
}
