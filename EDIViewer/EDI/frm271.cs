using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using DCSGlobal.EDI;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using System.Data.SqlClient;


                           

namespace Manual_test_app
{
    public partial class EDIViewer : Form
    {

            Import270 imp270 = new Import270(); 
            Import271 imp = new Import271(); 
            StringStuff ss = new StringStuff();

            Parse objP = new Parse();
           
        
        public EDIViewer()
        {
            InitializeComponent();




        }

        private void button1_Click(object sender, EventArgs e)
        {


            button2.Enabled = false;
            label3.Visible = false;

            int re = 0;









            switch (cmbEDIType.Text)
            {                          // Must be integer or string
                case "270":


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
                        imp.BatchID = 0;
                    }


                    break;
                case "271":




                    txtConString.Text = "Data Source=10.1.1.120;Initial Catalog=al60_seton_lite_developer;Persist Security Info=True;User ID=al60_seton_lite_developer_user;Password=al60_seton_lite_developer_password";
                    imp.EDI = rtEDI.Text;
                    imp.ConnectionString = txtConString.Text;
                    try
                    {
                        imp.BatchID = Convert.ToDouble(txtBatchID.Text);
                        re = imp.Import();

                        if (re == 0)
                        {
                            dgEDI.DataSource = imp.EDI_271;
                            dgENV.DataSource = imp.ENVELOP;

                            dgISA.DataSource = imp.ISA;
                            dgGS.DataSource = imp.GS;
                            dgST.DataSource = imp.ST;
                            dgBST.DataSource = imp.BHT;

                            dgHL20.DataSource = imp.HL01;
                            dgHL21.DataSource = imp.HL02;
                            dgHL22.DataSource = imp.HL03;
                            dgHL23.DataSource = imp.HL04;
                            dgAAA.DataSource = imp.AAA;

                            dgEB.DataSource = imp.EB;
                            dgAMT.DataSource = imp.AMT;
                            dgDMG.DataSource = imp.DMG;
                            dgDTP.DataSource = imp.DTP;
                            dgEQ.DataSource = imp.EQ;
                            dgHSD.DataSource = imp.HSD;
                            dgHL.DataSource = imp.HL;
                            dgIII.DataSource = imp.III;
                            dgINS.DataSource = imp.INS;

                            dgcMSG.DataSource = imp.CACHE_MSG;
                            dgtMSG.DataSource = imp.DTdistinctDT;
                            dgMSG.DataSource = imp.MSG;

                            dgN3.DataSource = imp.N3;
                            dgN4.DataSource = imp.meddd;
                            dgNM1.DataSource = imp.NM1;
                            dgPER.DataSource = imp.PER;
                            dgPRV.DataSource = imp.PRV;
                            dgREF.DataSource = imp.REF;
                            dgTRN.DataSource = imp.TRN;

                            dgUNK.DataSource = imp.UNK;
                            tblEDI.TabIndex = 1;
                            button2.Enabled = true;
                        }
                        tlsStatusReturn.Width = 200;
                        tlsStatusReturn.Text = "return code: " + Convert.ToString(re);

                        tlsTime.Width = 200;
                        tlsTime.Text = "Total time: " + Convert.ToString(imp.ExecutionTime.TotalSeconds.ToString("0.000000"));
                        tlsLoopCount.Text = "Loop count: " + Convert.ToString(imp.EBLoopCount);

                        stlChars.Text = imp.chars;


  

                    }
                    catch (Exception ex)
                    {

                        tblEDI.SelectTab("tbpERR");
                        txtERR.Text = ex.Message;
                        imp.BatchID = 0;
                    }





                    break;

                default:



                    ; break;       // break necessary on default

            }
            
            
            
            
            
            

     

        }

        private void button2_Click(object sender, EventArgs e)
        {

            label3.Visible = true;
            label3.Text = "Begin Commit";

            switch (cmbEDIType.Text)
                     {                          // Must be integer or string
                case "270":



                    
                        long i270 = 0;

                        i270 = imp270.Comit();

                        if (i270 == 0)
                        {
                            label3.Text = "Commit Success : "  + imp270.ErrorMessage ;
            
                        }
                        else
                        {

                            label3.Text = "Commit Error";
                            tblEDI.SelectTab("tbpERR");
                            txtERR.Text = imp270.ErrorMessage;
            
                        }

             



                    break;
                case "271":


                        int i271 = 0;

                        i271 = imp.Comit();

                        if (i271 == 0)
                        {
                            label3.Text = "Commit Success : "  + imp.ErrorMessage ;

                            tlsOuput.Text = "Status : " + imp.Status + "  Reject_Reason_code : " + imp.Reject_Reason_code + "   LOOP_AGAIN : " + imp.LOOP_AGAIN; 
            
                        }
                        else
                        {

                            label3.Text = "Commit Error";
                            tblEDI.SelectTab("tbpERR");
                            txtERR.Text = imp.ErrorMessage;
            

                         
                        }

             


                    break;

                default:



                    ; break; 

           }
        }

        private void EDIViewer_Load(object sender, EventArgs e)
        {

            label3.Visible = false;
            txtConString.Text = RES.ghinckley;
          


        }

        private void EDIViewer_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            imp = null;
        
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
            string r;

            r = rtEDI.Text;
            r =ss.ReplaceRowDelimiter(r, "~");
            rtEDI.Text = r;
           
            
            r = rtREQ.Text;
            r = ss.ReplaceRowDelimiter(r, "~");
            rtREQ.Text = r;


            r = rtRES.Text;
            r = ss.ReplaceRowDelimiter(r, "~");
            rtRES.Text = r;

            //
        }

        private void button4_Click(object sender, EventArgs e)
        {




                      switch (cmbEDIType.Text)
                     {                          // Must be integer or string
                case "270":

                           rtEDI.Text = ss.ReplaceRowDelimiter(RES.s270, "~"); ;



                             break;


                case "271":

                             rtEDI.Text = ss.ReplaceRowDelimiter(RES.s271, "~"); ;



                             break;

                          
                          default:



                             ; break;

                     }


           
        }

        private void button5_Click(object sender, EventArgs e)
        {


            objP.ConnectionString = txtConString.Text;
            //  public int Import271(string EDI_271, string DELETE_FLAG_271, string EBR_ID, string USER_ID, string HOSP_CODE, string SOURCE, string PAYOR_ID, string VENDOR_NAME, string LOG_EDI, long BatchID);

         //   objP.Import271(rtEDI.Text, txtDeleteFlag.Text, Convert.ToDouble(txtEBR_ID.Text),
          //      txtUSER_ID.Text, txtHOSP_CODE.Text, 
            ///    txtSource.Text, 'Y','Y', 'Y', Convert.ToDouble(txtBatchID.Text));


            tlsStatusReturn.Text = "Status: " + objP.Status;
            tlsTime.Text = "Loop Again: " + objP.LOOP_AGAIN;
            tlsLoopCount.Text = "Reject Reason Code: " + objP.Reject_Reason_code; 

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {





        }

        private void button7_Click(object sender, EventArgs e)
        {








        }

    }
}
