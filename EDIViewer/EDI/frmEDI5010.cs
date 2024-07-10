using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.Sql;

using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.Diagnostics;


using DCSGlobal.EDI;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using DCSGlobal.BusinessRules.CoreLibraryII;
using DCSGlobal.BusinessRules.CoreLibrary;
using DCSGlobal.BusinessRules.Logging;



namespace Manual_test_app
{
    public partial class frmEDI5010 : Form
    {


        //     [DllImport("test3.dll", CallingConvention = CallingConvention.Cdecl)]
        //     public static extern int add(int a, int b);
        //   public static extern int subtract(int a, int b); 


        private EDI_5010_VALIDATE ev = new EDI_5010_VALIDATE();
        private StringStuff ss = new StringStuff();

        private List<string> _EDIList = new List<string>();

       private string _ConnectionString = "Data Source=10.1.1.120;Initial Catalog=al60_seton_lite_developer;Persist Security Info=True;User ID=al60_seton_lite_developer_user;Password=al60_seton_lite_developer_password";

        //Persist Security Info=False;User ID=console_roane_general_user;Password=NQwkzBtCqyk6KHpLzL8JUoiknbw4rv/IbWlQxRgQ130=;Initial Catalog=al60_roane_general;Data Source=10.34.5.10;pooling=false;connection Timeout=180

      //  private string _ConnectionString = "Data Source=10.34.5.10;Initial Catalog=al60_roane_general;Persist Security Info=True;User ID=console_roane_general_user;Password=console_roane_general_password";
        private string dbTempString = string.Empty;



        private int _BATCH_ID = 42;


        private string _TransactionSetIdentifierCode = string.Empty;
        private string _ImplementationConventionReference = string.Empty;

        public frmEDI5010()
        {
            InitializeComponent();


        }

        private void VALIDATE()
        {
            tslState.Text = "Runing Validation";
            int v = 0;
            //   Data Source=10.34.1.190;Initial Catalog=al60_dallas_children_prod;Persist Security Info=True;User ID=al60_dallas_children_user;Password=al60_dallas_children_password



            v = ev.byString(fctEDI.Text);




            _TransactionSetIdentifierCode = ev.TransactionSetIdentifierCode;
            _ImplementationConventionReference = ev.ImplementationConventionReference;


            tslTSIC.Text = ev.TransactionSetIdentifierCode;
            tslCR.Text = ev.ImplementationConventionReference;




            tslState.Text = "Valid";
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

            VALIDATE();
        }

        private void tsbReadale_Click(object sender, EventArgs e)
        {
            string r;

            r = fctEDI.Text;
            r = ss.ReplaceRowDelimiter(r, "~");
            fctEDI.Text = r;
        }

        private void frmEDI5010_Load(object sender, EventArgs e)
        {
            txt276BatchID.Text = Convert.ToString(_BATCH_ID);
            //            _ConnectionString = "Data Source=10.34.1.190;Initial Catalog=al60_dallas_children_prod;Persist Security Info=True;User ID=al60_dallas_children_user;Password=al60_dallas_children_password";


            //          _ConnectionString = "Persist Security Info=False;User ID=al60_seton_lite_developer_user;Password=AYImyvmKCd14tMw9timGAoTVdCmIKdP6yDSSKymY7t0um+lShaMStpGRtDcLlS7O;Initial Catalog=al60_seton_lite_developer;Data Source=10.1.1.120;pooling=false;connection Timeout=180";


            txtConstring.Text = _ConnectionString;


            tslTSIC.Text = ev.TransactionSetIdentifierCode;
            tslCR.Text = ev.ImplementationConventionReference;
        }

        private void tsbParse_Click(object sender, EventArgs e)
        {
            int i = -1;
            try
            {

                _BATCH_ID = Convert.ToInt32(txt276BatchID.Text);
                i = 0;
            }
            catch
            {
                tslPARSERSTATUS.ForeColor = System.Drawing.Color.Red;
                tslPARSERSTATUS.Text = "INVALID BATCH ID";

            }





            if (i == 0)
            {


                using (StringPrep sp = new StringPrep())
                {
                    _EDIList = sp.SingleEDI(fctEDI.Text);
                }




                using (EDI_5010_VALIDATE vedi = new EDI_5010_VALIDATE())
                {
                    vedi.ConnectionString = _ConnectionString;
                    vedi.byString(fctEDI.Text);

                    _TransactionSetIdentifierCode = vedi.TransactionSetIdentifierCode;
                    //AAAFailureCode = vedi.AAAFailureCode;


                    _ImplementationConventionReference = vedi.ImplementationConventionReference;
                }




                // Every case must end with break or goto case
                switch (_TransactionSetIdentifierCode)
                {                          // Must be integer or string
                    case "270":
                        tslPARSERSTATUS.Text = "PARSING";
                        using (EDI_270_005010X279A1 edi = new EDI_270_005010X279A1())
                        {
                            edi.ConnectionString = _ConnectionString;

                            // edi.BATCH_ID = _BATCH_ID; 
                            edi.Import(_EDIList);
                            tslPARSERSTATUS.Text = edi.IMPORT_RETURN_STRING;
                        }

                        break;

                    case "271":
                        tslPARSERSTATUS.Text = "PARSING";
                        using (EDI_271_005010X279A1 edi = new EDI_271_005010X279A1())
                        {
                            edi.ConnectionString = _ConnectionString;

                            // edi.BATCH_ID = _BATCH_ID;
                            edi.Import(_EDIList);

                            tslPARSERSTATUS.Text = edi.IMPORT_RETURN_STRING;
                        }

                        break;

                    case "276":
                        tslPARSERSTATUS.Text = "PARSING";
                        using (EDI_5010_276_005010X212_DCS edi = new EDI_5010_276_005010X212_DCS())
                        {
                            edi.ConnectionString = _ConnectionString;
                            int r = 0;
                            edi.BATCH_ID = _BATCH_ID;
                            r = edi.Import(_EDIList);
                            tslPARSERSTATUS.Text = edi.IMPORT_RETURN_STRING;
                        }

                        break;



                    case "277":
                        tslPARSERSTATUS.Text = "PARSING";
                        using (EDI_5010_277_005010X212_v2 edi = new EDI_5010_277_005010X212_v2())
                        {
                            edi.ConnectionString = _ConnectionString;

                            edi.BATCH_ID = _BATCH_ID;
                            edi.Import(_EDIList);

                            tslPARSERSTATUS.Text = edi.IMPORT_RETURN_STRING;
                        }
                        ;
                        break;
                    case "278":
                        tslState.Text = "Not implemented for EDI " + _TransactionSetIdentifierCode;
                        break;

                    case "835":
                        tslPARSERSTATUS.Text = "PARSING";
                        using (EDI_5010_835_005010X221A1 edi = new EDI_5010_835_005010X221A1())
                        {
                            edi.ConnectionString = _ConnectionString;

                            edi.BATCH_ID = _BATCH_ID;
                            edi.Import(_EDIList);
                            tslPARSERSTATUS.Text = edi.IMPORT_RETURN_STRING;
                        }

                        break;
                    case "837":
                        tslPARSERSTATUS.Text = "PARSING";


                        switch (_ImplementationConventionReference)
                        {
                            case "005010X222A1":
                                using (EDI_5010_837P_005010X222A1_v2 edi = new EDI_5010_837P_005010X222A1_v2())
                                {
                                    edi.ConnectionString = _ConnectionString;

                                    edi.BATCH_ID = Convert.ToInt64(txt837BatchID.Text);
                                    
                                    edi.FILE_ID = Convert.ToInt64(txt837BatchID.Text);
                                    edi.Import(_EDIList);
                                    tslPARSERSTATUS.Text = edi.IMPORT_RETURN_STRING;
                                }
                                break;




                            case "005010X223A1":
                                using (EDI_5010_837I_005010X223A2_v2 edi = new EDI_5010_837I_005010X223A2_v2())
                                {
                                    edi.ConnectionString = _ConnectionString;

                                    edi.BATCH_ID = Convert.ToInt64(txt837BatchID.Text);
                                    edi.FILE_ID = Convert.ToInt64(txt837BatchID.Text);
                                    edi.Import(_EDIList);
                                    tslPARSERSTATUS.Text = edi.IMPORT_RETURN_STRING;
                                }
                                break;


                            case "005010X223A2":
                                using (EDI_5010_837I_005010X223A2_v2 edi = new EDI_5010_837I_005010X223A2_v2())
                                {
                                    edi.ConnectionString = _ConnectionString;

                                    edi.BATCH_ID = Convert.ToInt64(txt837BatchID.Text);
                                    edi.FILE_ID = Convert.ToInt64(txt837BatchID.Text);
                                    edi.Import(_EDIList);
                                    tslPARSERSTATUS.Text = edi.IMPORT_RETURN_STRING;
                                }
                                break;
                        }
                        break;


                    default:
                        tslState.Text = "No Parse Engine for EDI " + _TransactionSetIdentifierCode;
                        break;       // break necessary on default
                }


            }

        }

        private void rtEDI_TextChanged(object sender, EventArgs e)
        {
            VALIDATE();

            using (StringPrep sp = new StringPrep())
            {
                _EDIList = sp.SingleEDI(fctEDI.Text);
            }


            // Every case must end with break or goto case
            switch (_TransactionSetIdentifierCode)
            {                          // Must be integer or string
                case "270":

                    break;

                case "271":

                    tab271.Show();
                    break;

                case "276":


                    break;



                case "277":

                    ;
                    break;
                case "278":

                    break;

                case "835":



                    break;
                case "837":

                    break;

                default:
                    tslState.Text = "No Parse Engine for EDI " + _TransactionSetIdentifierCode;
                    break;       // break necessary on default
            }



        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {

        }

        private void cmdImport276_Click(object sender, EventArgs e)
        {


            using (EDI_5010_PARSE imp = new EDI_5010_PARSE())
            {
                long rBatchid = 0;

                imp.ConnectionString = txtConstring.Text;
                rBatchid = imp.Import276(fctEDI.Text, txt276DeletFlag.Text, Convert.ToDouble(txtEbrID.Text), txtUserId.Text,
                      txtHospCode.Text, txtSource.Text, txtPayerCode.Text, txtVendorName.Text, txtInsType.Text, txtAccountNumber.Text, txtPatHouseCode.Text, Convert.ToDouble(txt276BatchID.Text));

                txt276BatchID.Text = Convert.ToString(rBatchid);

            }


        }

        private void txtConstring_TextChanged(object sender, EventArgs e)
        {
            _ConnectionString = txtConstring.Text;
        }

        private void cmdDecode_Click(object sender, EventArgs e)
        {
            DBUtility dbu = new DBUtility();


            dbTempString = txtConstring.Text;

            _ConnectionString = dbu.getConnectionString(dbTempString);

            txtConstring.Text = _ConnectionString;

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            //int x = Convert.ToInt32(textBox3.Text);
            //int y = Convert.ToInt32(textBox2.Text);
            //int z = add(x, y);
            //  MessageBox.Show("Required Answer is " + Convert.ToString(z), "Answer", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmdParse270_Click(object sender, EventArgs e)
        {


            //Public Function Import270(ByVal EDI_270 As String, ByVal DELETE_FLAG_270 As String, _
            //                    ByVal EBR_ID As Double, ByVal USER_ID As String, ByVal HOSP_CODE As String, ByVal SOURCE As String, _
            //                    ByVal PAYOR_ID As String, ByVal VENDOR_NAME As String, ByVal INS_TYPE As String, _
            //                    ByVal PATIENT_NUMBER As String, ByVal PAT_HSOP_CODE As String,
            //                    ByVal BatchID As Double) As Int64

            if (chk270Use5010.Checked)
            {

                using (EDI_5010_PARSE imp = new EDI_5010_PARSE())
                {
                    long rBatchid = 0;
                    try
                    {


                        imp.ConnectionString = txtConstring.Text;
                        rBatchid = imp.Import270(fctEDI.Text,
                                                    txt270DeleteFlag.Text,
                                                    Convert.ToInt64(txt270EBRID.Text),
                                                    txt270UserId.Text,
                                                    txt270HospCode.Text,
                                                    txt270Source.Text,
                                                    txt270PayorID.Text,
                                                    txt270VendorName.Text,
                                                    txt270InsType.Text,
                                                    txt270AccountNumber.Text,
                                                    txt270PatHospCode.Text,
                                                    Convert.ToInt64(txt270BatchID.Text));

                        txtREQReturnBatchID.Text = Convert.ToString(rBatchid);


                        //     txtReturnBatchID.Text = Convert.ToString(imp.BatchID); 









                    }
                    catch (Exception ex)
                    {
                        tslCR.Text = ex.Message;
                    }
                }
            }
            else
            {


                using (Parse imp = new Parse())
                {
                    long rBatchid = 0;

                    imp.ConnectionString = txtConstring.Text;
                    rBatchid = imp.Import270(fctEDI.Text,
                                                txt270DeleteFlag.Text,
                                                Convert.ToDouble(txtEbrID.Text),
                                                txt270UserId.Text,
                                                txt270HospCode.Text,
                                                txt270Source.Text,
                                                txt270PayorID.Text,
                                                txt270VendorName.Text,
                                                txt270InsType.Text,
                                                txt270AccountNumber.Text,
                                                txtPatHouseCode.Text,
                                                Convert.ToDouble(txt276BatchID.Text));

                    txtREQReturnBatchID.Text = Convert.ToString(rBatchid);


                }
            }



        }

        private void cmd271Parse_Click(object sender, EventArgs e)
        {


            //Public Function Import271(ByVal EDI_271 As String, ByVal DELETE_FLAG_271 As String, _
            //                        ByVal EBR_ID As String, ByVal USER_ID As String, ByVal HOSP_CODE As String, ByVal SOURCE As String,
            //                        ByVal PAYOR_ID As String, ByVal VENDOR_NAME As String, _
            //                        ByVal LOG_EDI As String, ByVal BatchID As Int64) As Integer





            using (EDI_5010_PARSE imp = new EDI_5010_PARSE())
            {
                long rBatchid = 0;
                string _271DeleteFlag = "Y";
                string _271LogEDI = "Y";
                try
                {



                    if (chk271DeleteFlag.Checked)
                    { _271DeleteFlag = "Y"; }
                    else
                    { _271DeleteFlag = "N"; }


                    if (chk271LogEDI.Checked)
                    { _271LogEDI = "Y"; }
                    else
                    { _271LogEDI = "N"; }

                    imp.ConnectionString = txtConstring.Text;
                    rBatchid = imp.Import271(fctEDI.Text,
                                                _271DeleteFlag,
                                                Convert.ToInt64(txt271EBRID.Text),
                                                txt271UserId.Text,
                                                txt271HospCode.Text,
                                                txt271Source.Text,
                                                txt271PayorID.Text,
                                                txt271VendorName.Text,
                                                _271LogEDI,
                                                Convert.ToInt64(txt271GlobalBatchID.Text));

                    lbl271ReturnCode.Text = Convert.ToString(rBatchid);


                    label27.Text = imp.Status;
                }
                catch (Exception ex)
                {
                    tslCR.Text = ex.Message;
                }
            }





















        }

        private void cmdPop271_Click(object sender, EventArgs e)
        {
            txt271EBRID.Text = txt270EBRID.Text;
            txt271UserId.Text = txt270UserId.Text;
            txt271HospCode.Text = txt270HospCode.Text;
            txt271PayorID.Text = txt270PayorID.Text;
            txt271VendorName.Text = txt270VendorName.Text;
            tbpEDI.SelectTab(tab271);

        }

        private void cmdLegacyImport271_Click(object sender, EventArgs e)
        {

            Import271 i271 = new Import271();
            int r271 = -1;





            i271.BatchID = Convert.ToDouble(txt271GlobalBatchID.Text);
            i271.ConnectionString = txtConstring.Text;
            i271.EDI = fctEDI.Text;

            if (chk271DeleteFlag.Checked)
            {
                i271.DeleteFlag = "y";
            }
            i271.DeadlockRetrys = 5;

            i271.ebr_id = Convert.ToDouble(txt271EBRID.Text);
            i271.user_id = txt271UserId.Text;
            i271.source = txt271Source.Text;
            i271.hosp_code = txt271HospCode.Text;

            i271.PAYOR_ID = txt270PayorID.Text;
            i271.DeadlockRetrys = Convert.ToInt32(txt271DeadLockRetryCount.Text);
            i271.Vendor_name = txt271VendorName.Text;
            //        i271.Log_EDI = LOG_EDI
            //        i271.ServiceTypeCode = _reqServiceTypeCodes
            //        i271.NPI = _NPI
            //        'pasrgine but no comittt 
            r271 = i271.Import();

            if (r271 == 0)
            {
                r271 = i271.Comit();

            }


        }

        private void cmdLegacyParse271_Click(object sender, EventArgs e)
        {

            using (EDI_5010_PARSE imp = new EDI_5010_PARSE())
            {
                long rBatchid = 0;
                string _271DeleteFlag = "Y";
                string _271LogEDI = "Y";
                try
                {



                    if (chk271DeleteFlag.Checked)
                    { _271DeleteFlag = "Y"; }
                    else
                    { _271DeleteFlag = "N"; }


                    if (chk271LogEDI.Checked)
                    { _271LogEDI = "Y"; }
                    else
                    { _271LogEDI = "N"; }

                    imp.ConnectionString = txtConstring.Text;
                    rBatchid = imp.Import271(fctEDI.Text,
                                                _271DeleteFlag,
                                                Convert.ToInt64(txt271EBRID.Text),
                                                txt271UserId.Text,
                                                txt271HospCode.Text,
                                                txt271Source.Text,
                                                txt271PayorID.Text,
                                                txt271VendorName.Text,
                                                _271LogEDI,
                                                Convert.ToInt64(txt271GlobalBatchID.Text));

                    lbl271ReturnCode.Text = Convert.ToString(rBatchid);


                    label27.Text = imp.Status;
                }
                catch (Exception ex)
                {
                    tslCR.Text = ex.Message;
                }
            }
        }

        private void cmdImport276_Click_1(object sender, EventArgs e)
        {






        }

        private void cmd5010Import276_Click(object sender, EventArgs e)
        {
            int ImportReturnCode276 = -1;
            lbl276_5010ImportReturnCode.Text = "";


            using (EDI_5010_276_005010X212_DCS i276 = new EDI_5010_276_005010X212_DCS())
            {

                i276.ConnectionString = txtConstring.Text;
                ImportReturnCode276 = i276.ImportByString(fctEDI.Text, Convert.ToDouble(txt276BatchID.Text));

            }



            lbl276_5010ImportReturnCode.Text = "Import return code:  " + Convert.ToString(ImportReturnCode276);

        }

        private void cmd277_5010Import_Click(object sender, EventArgs e)
        {

            int ImportReturnCode277 = -1;
            lbl277_5010ImportReturnCode.Text = "";


            using (EDI_5010_277_005010X212_v2 i277 = new EDI_5010_277_005010X212_v2())
            {

                i277.ConnectionString = txtConstring.Text;
                ImportReturnCode277 = i277.ImportByString(fctEDI.Text, Convert.ToDouble(txt277BatchID.Text));

            }



            lbl277_5010ImportReturnCode.Text = "Import return code:  " + Convert.ToString(ImportReturnCode277);

        }

        private void tab270_Click(object sender, EventArgs e)
        {

        }


    }
}


