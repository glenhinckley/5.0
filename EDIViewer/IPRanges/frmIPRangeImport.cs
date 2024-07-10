using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;








using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;

using System.Configuration;

using System.Data.SqlClient;
using System.Diagnostics;

using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using DCSGlobal.BusinessRules.CoreLibraryII;
using DCSGlobal.BusinessRules.CoreLibrary;
using DCSGlobal.BusinessRules.Logging;
using DCSGlobal.BusinessRules.CoreLibrary.dbCheckStuff;

namespace Manual_test_app.IPRanges
{
    public partial class frmIPRangeImport : Form
    {

        private string _ConnectionStringSource = "Data Source=10.1.1.120;Initial Catalog=al60_seton_lite_developer;Persist Security Info=True;User ID=al60_seton_lite_developer_user;Password=al60_seton_lite_developer_password";



        public frmIPRangeImport()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            StringStuff ss = new StringStuff();

            text = textBox1.Text;


            string[] parts = text.Split('~');
            foreach (string value in parts)
            {
                //  name,alpha-2,alpha-3,country-code,iso_3166-2,region,sub-region,
                //intermediate-region,region-code,sub-region-code,intermediate-region-code


                string COUNTRY_CODE_ID = string.Empty;
                string COUNTRY_NAME = string.Empty;
                string ALPHA_2 = string.Empty;
                string ALPHA_3 = string.Empty;
                string COUNTRY_CODE = string.Empty;
                string ISO_3166_2 = string.Empty;
                string REGION = string.Empty;
                string SUB_REGION = string.Empty;
                string INTERMEDIATE_REGION = string.Empty;
                string REGION_CODE = string.Empty;
                string SUB_REGION_CODE = string.Empty;
                string INTERMEDIATE_REGION_CODE = string.Empty;

               
                COUNTRY_NAME = ss.ParseDemlimtedStringEDI(value, ",", 1);
                ALPHA_2 = ss.ParseDemlimtedStringEDI(value, ",", 2);
                ALPHA_3 = ss.ParseDemlimtedStringEDI(value, ",", 3);
                COUNTRY_CODE = ss.ParseDemlimtedStringEDI(value, ",", 4);
                ISO_3166_2 = ss.ParseDemlimtedStringEDI(value, ",", 5);
                REGION = ss.ParseDemlimtedStringEDI(value, ",", 6);
                SUB_REGION = ss.ParseDemlimtedStringEDI(value, ",", 7);
                INTERMEDIATE_REGION = ss.ParseDemlimtedStringEDI(value, ",", 8);
                REGION_CODE = ss.ParseDemlimtedStringEDI(value, ",", 9);
                SUB_REGION_CODE = ss.ParseDemlimtedStringEDI(value, ",", 10);
                INTERMEDIATE_REGION_CODE = ss.ParseDemlimtedStringEDI(value, ",", 11);


                COUNTRY_CODE_ID.Replace("\"", "");
                COUNTRY_NAME.Replace("\"", "");
                ALPHA_2.Replace("\"", "");
                ALPHA_3.Replace("\"", "");
                COUNTRY_CODE.Replace("\"", "");
                ISO_3166_2.Replace("\"", "");
                REGION.Replace("\"", "");
                SUB_REGION.Replace("\"", "");
                INTERMEDIATE_REGION.Replace("\"", "");
                REGION_CODE.Replace("\"", "");
                SUB_REGION_CODE.Replace("\"", "");
                INTERMEDIATE_REGION_CODE.Replace("\"", "");


                string insert = string.Empty;
                //    insert = "insert into TABLES (TABLE_NAME,PACKAGE_ID) values ('" + TableName + "', " + Convert.ToString(PackageID) + ") ; SELECT SCOPE_IDENTITY()";


                insert = insert + "insert into [tbl_IP_BLOCK_COUNTRY_CODES] (";
     //           insert = insert + "[COUNTRY_CODE_ID],";
                insert = insert + "[COUNTRY_NAME],";
                insert = insert + "[ALPHA_2],";
                insert = insert + "[ALPHA_3],";
                insert = insert + "[COUNTRY_CODE],";
                insert = insert + "[ISO_3166-2],";
                insert = insert + "[REGION],";
                insert = insert + "[SUB_REGION],";
                insert = insert + "[INTERMEDIATE_REGION],";
                insert = insert + "[REGION_CODE],";
                insert = insert + "[SUB_REGION_CODE],";
                insert = insert + "[INTERMEDIATE_REGION_CODE]";

                insert = insert + ") values  (";

           //     insert = insert + "'" + COUNTRY_CODE_ID + "', ";
                insert = insert + "'" + COUNTRY_NAME + "', ";
                insert = insert + "'" + ALPHA_2 + "', ";
                insert = insert + "'" + ALPHA_3 + "', ";
                insert = insert + "'" + COUNTRY_CODE + "', ";
                insert = insert + "'" + ISO_3166_2 + "', ";
                insert = insert + "'" + REGION + "', ";
                insert = insert + "'" + SUB_REGION + "', ";
                insert = insert + "'" + INTERMEDIATE_REGION + "', ";
                insert = insert + "'" + REGION_CODE + "', ";
                insert = insert + "'" + SUB_REGION_CODE + "', ";
                insert = insert + "'" + INTERMEDIATE_REGION_CODE + "' ";



                insert = insert + ")";







                using (SqlConnection con = new SqlConnection(_ConnectionStringSource))
                {
                    con.Open();
                    //
                    // Create new SqlCommand object.
                    //
                    using (SqlCommand cmd = new SqlCommand(insert, con))
                    {

                        cmd.CommandType = CommandType.Text;

                        cmd.ExecuteNonQuery();

                    }
                }



            }





        }
    }
}
