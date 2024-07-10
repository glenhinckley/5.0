using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Manual_test_app.db
{
    public partial class frmConnectionStrings : Form
    {

        private string _ConnectionStringPackage = "Integrated Security=SSPI;Initial Catalog=glen_db;Data Source=10.1.1.120";

        public frmConnectionStrings()
        {
            InitializeComponent();
        }


        private void frmConnectionStrings_Load(object sender, EventArgs e)
        {

        }

        private void cmdNew_Click(object sender, EventArgs e)
        {

            int r = -1;
            string insert = string.Empty;




            insert = "insert into [CONSTRINGS] ";

            insert = insert + "(";


            insert = insert + "[IP], ";
            insert = insert + "[USER_NAME], ";
            insert = insert + "[PASSWD], ";
            insert = insert + "[DESCRIPTION], ";
            insert = insert + "[CATALOG], ";
            insert = insert + "[RDBMS], ";
            insert = insert + "[DEFAULT_TIMEOUT], ";
            insert = insert + "[POOLING], ";
            insert = insert + "[PERSIT_SECURITY_INFO], ";
            insert = insert + ")  values (   ";






            insert = insert + " '" + txtIP.Text + "',";   //] [int] NULL,
            insert = insert + " '" + txtUserID.Text + "',";  ///] [varchar](255) NULL,
            insert = insert + " '" + txtPasswd.Text + "',";   //] [int] NULL,
            insert = insert + " '" + txtConName.Text  + "',";   //] [varchar](255) NULL,
           insert = insert + " '" + txtIC.Text  + "',";    ///] [int] NULL,
           // insert = insert + " '" + txt + "',";    //] [varchar](255) NULL,
            //insert = insert + " '" + Convert.ToString(_CHARACTER_MAXIMUM_LENGTH) + "',";    //] [int] NULL,
            //insert = insert + " '" + Convert.ToString(_CHARACTER_OCTET_LENGTH) + "',";     //] [int] NULL,
            //insert = insert + " '" + Convert.ToString(_NUMERIC_PRECISION) + "',";     ///] [int] NULL,
            //insert = insert + " '" + Convert.ToString(_NUMERIC_PRECISION_RADIX) + "',";   //] [int] NULL,
            //insert = insert + " '" + Convert.ToString(_NUMERIC_SCALE) + "',";   //] [int] NULL,
            //insert = insert + " '" + Convert.ToString(_DATETIME_PRECISION) + "',";    //] [int] NULL,
            //insert = insert + " '" + _CHARACTER_SET_CATALOG + "',";   // ] [varchar](255) NULL,
            //insert = insert + " '" + _CHARACTER_SET_SCHEMA + "',";   // ] [varchar](255) NULL,
            //insert = insert + " '" + _CHARACTER_SET_NAME + "',";  //] [varchar](255) NULL,
            //insert = insert + " '" + _COLLATION_CATALOG + "',";    //] [varchar](255) NULL,
            //insert = insert + " '" + _COLLATION_SCHEMA + "',";    //]  // [varchar](255) NULL,
            //insert = insert + " '" + _COLLATION_NAME + "',";  //] [varchar](255) NULL,
            //insert = insert + " '" + _DOMAIN_CATALOG + "',";   //] [varchar](255) NULL,
            //insert = insert + " '" + _DOMAIN_SCHEMA + "',";    ///] [varchar](255) NULL,
            //insert = insert + " '" + _DOMAIN_NAME + "' ";   ///] [varchar](255) NULL,

            insert = insert + ")   ; SELECT SCOPE_IDENTITY()";


            using (SqlConnection con = new SqlConnection(_ConnectionStringPackage))
            {
                con.Open();
                //
                // Create new SqlCommand object.
                //
                using (SqlCommand cmd = new SqlCommand(insert, con))
                {

                    cmd.CommandType = CommandType.Text;

                    r = Convert.ToInt32(cmd.ExecuteScalar());

                }
            }










        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {

        }

        private void cmdApply_Click(object sender, EventArgs e)
        {

        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            string c = string.Empty;

            try
            {

                using (SqlConnection con = new SqlConnection(c))
                {
                    con.Open();

                }
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.Message);

            }



        }

        private void cmbEM_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbConStrings_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lnkParse_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void lnkClearText_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void lnkDecryptAndParse_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void lnkEncrypted_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }
    }
}
