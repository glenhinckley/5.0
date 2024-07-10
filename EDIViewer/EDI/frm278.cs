using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DCSGlobal.EDI;

namespace Manual_test_app
{
    public partial class frm278 : Form
    {
        public frm278()
        {
            InitializeComponent();          

        }

        private void button1_Click(object sender, EventArgs e)
        {


            Import278 i278 = new Import278();

                   string c = "Data Source=10.1.1.175;Initial Catalog=al60_seton_lite_developer;Persist Security Info=True;User ID=suresh;Password=suresh247";

                   c  = "Persist Security Info=False;User ID=al60_seton_lite_developer_user;Password=al60_seton_lite_developer_password;Initial Catalog=al60_seton_lite_developer;Data Source=10.1.1.120;pooling=false;connection Timeout=180";


            i278.ConnectionString = c;
            i278.ImportByString(textBox1.Text, 200); 





        }

        private void button2_Click(object sender, EventArgs e)
        {
             
            Parse imp =new Parse();
                         string c = "Data Source=10.1.1.175;Initial Catalog=al60_seton_lite_developer;Persist Security Info=True;User ID=suresh;Password=suresh247";

            imp.ConnectionString = c;

            imp.Import278Request(textBox1.Text , "N", Convert.ToString(1027), "auditlogix", "510", "SOURCE_278_REQ", "00192", "POST-N-TRACK", "1", "00027060409", "00027060409-510");
        }

        private void cmd278RES_Click(object sender, EventArgs e)
        {


            Parse imp = new Parse();


            //string c = "Persist Security Info=False;User ID=console_waterbury_user;Password=console_waterbury_password;Initial Catalog=al60_waterbury;Data Source=10.34.1.130;pooling=false;connection Timeout=180";
            string c = "Persist Security Info=False;User ID=al60_hocking_valley_user;Password=al60_hocking_valley_password;Initial Catalog=al60_hocking_valley;Data Source=10.34.1.170;pooling=false;connection Timeout=180";

            //string c =  "Persist Security Info=False;User ID=al60_seton_lite_developer_user;Password=al60_seton_lite_developer_password;Initial Catalog=al60_seton_lite_developer;Data Source=10.1.1.120;pooling=false;connection Timeout=180";
            //string c = "Data Source=10.1.1.175;Initial Catalog=al60_seton_lite_developer;Persist Security Info=True;User ID=suresh;Password=suresh247";
            //string c = "Data Source=10.1.1.175;Initial Catalog=al60_seton_lite_developer;Persist Security Info=True;User ID=suresh;Password=suresh247";
            //string c = "Data Source=10.34.1.60;Initial Catalog=al60_jhs_test;Persist Security Info=True;User ID=suresh;Password=suresh247";
            //string c = "Data Source=10.34.1.140;Initial Catalog=al60_jhs_prod;Persist Security Info=True;User ID=suresh;Password=suresh247";

            imp.ConnectionString = c;


                   //Public Function Import278Response(ByVal EDI_278 As String, ByVal DELETE_FLAG_278 As String, ByVal BATCH_ID As String, _
                   //             ByVal AUTH_ID As String, ByVal USER_ID As String, ByVal HOSP_CODE As String, ByVal SOURCE As String,
                   //                 ByVal PAYOR_ID As String, ByVal VENDOR_NAME As String, ByVal INS_TYPE As String, _
                   //                 ByVal account_number As String, ByVal PatHospCode As String) As Integer



            imp.Import278Response(textBox1.Text, "N",  textBox2.Text ,  Convert.ToString(1027), "auditlogix", "510",
                "SOURCE_278_REQ", "00192", "POST-N-TRACK", "1", "00027060409", "00027060409-510");

            MessageBox.Show("Completed");


        }
    }
}
