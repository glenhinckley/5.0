using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

using System.Data.SqlClient;
using System.Diagnostics;

using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using DCSGlobal.BusinessRules.CoreLibraryII;
using DCSGlobal.BusinessRules.CoreLibrary;
using DCSGlobal.BusinessRules.Logging;
using DCSGlobal.BusinessRules.CoreLibrary.dbCheckStuff;

using DCSGlobal.BusinessRules.db.Tables;

namespace Manual_test_app.DEPLOYMENT
{
    public partial class frmDeployPackage : Form
    {


        private string _ConnectionStringSource = "Data Source=10.1.1.120;Initial Catalog=al60_seton_lite_developer;Persist Security Info=True;User ID=al60_seton_lite_developer_user;Password=al60_seton_lite_developer_password";
        private string _ConnectionStringTarget = "Data Source=10.1.1.120;Initial Catalog=al60_seton_lite_developer;Persist Security Info=True;User ID=al60_seton_lite_developer_user;Password=al60_seton_lite_developer_password";


        private string _ConnectionStringPackage = "Integrated Security=SSPI;Initial Catalog=glen_db;Data Source=10.1.1.120";



        private string _dbTempStringSource = string.Empty;
        private string _dbTempStringPrivate = string.Empty;


        private int _PackageID = 0;
        private string _PackageName = string.Empty;



        private string _HTMLReport = string.Empty;

        StringStuff ss = new StringStuff();
        CodeInjection ci = new CodeInjection();
        logExecption log = new logExecption();




        public frmDeployPackage()
        {
            InitializeComponent();
        }

        private void frmDeployPackage_Load(object sender, EventArgs e)
        {





            txtConstring.Text = _ConnectionStringTarget;





            using (SqlConnection conn = new SqlConnection(_ConnectionStringPackage))
            {
                try
                {
                    string query = "select * from PACKAGES";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    conn.Open();
                    DataSet ds = new DataSet();
                    da.Fill(ds, "Fleet");
                    cmbPackages.DisplayMember = "PACKAGE_NAME";
                    cmbPackages.ValueMember = "PACKAGE_ID";
                    cmbPackages.DataSource = ds.Tables["Fleet"];
                }
                catch (Exception ex)
                {
                    // write exception info to log or anything else
                    MessageBox.Show("Error occured!");
                }
            }


        }

        private void cmbPackages_SelectedIndexChanged(object sender, EventArgs e)
        {
            tslSelectedPackage.Text = cmbPackages.SelectedText;
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void cmdGo_Click(object sender, EventArgs e)
        {

            int PackageID = Convert.ToInt32(cmbPackages.SelectedValue);



            _HTMLReport = _HTMLReport + "<html>";
            _HTMLReport = _HTMLReport + "<body>";
            _HTMLReport = _HTMLReport + "<h1>Target Status</h1>";


            _HTMLReport = _HTMLReport + "<hr/>";

            _HTMLReport = _HTMLReport + "<br/>";

            _HTMLReport = _HTMLReport + "<h2>Tables</h2>";


            using (SqlConnection con = new SqlConnection(_ConnectionStringPackage))
            {
                con.Open();
                //
                // Create new SqlCommand object.
                //

                string PackageTables = "Select * from [TABLES] where [PACKAGE_ID] = " + Convert.ToInt32(PackageID);
                using (SqlCommand cmd = new SqlCommand(PackageTables, con))
                {

                    cmd.CommandType = CommandType.Text;

                    using (SqlDataReader idr = cmd.ExecuteReader())
                    {

                        if (idr.HasRows)
                        {
                            // Call Read before accessing data. 
                            while (idr.Read())
                            {

                                string TableName = string.Empty;
                                int TableID = 0;


                                if (idr["TABLE_NAME"] != System.DBNull.Value)
                                {
                                    TableName = Convert.ToString((string)idr["TABLE_NAME"]);
                                }



                                if (idr["TABLE_ID"] != System.DBNull.Value)
                                {
                                    TableID = Convert.ToInt32((int)idr["TABLE_ID"]);
                                }


                                using (TableMaintance tm = new TableMaintance())
                                {
                                    int TableFound = 0;

                                    tm.ConnectionStringTarget = txtConstring.Text;
                                    TableFound = tm.LookForTable(TableName);

                                    if (TableFound == 1)
                                    {
                                        _HTMLReport = _HTMLReport + "<h3>Table  " + TableName + "  Found</h3>";
                                    }
                                    else
                                    {
                                        _HTMLReport = _HTMLReport + "<h3>Table  " + TableName + "  Not Found</h3>";
                                    }


                                    _HTMLReport = _HTMLReport + "   <table style=\"width:100%\">";

                                    if (TableFound == 1)
                                    {
                                        _HTMLReport = _HTMLReport + "  <caption>Columns for " + TableName + "</caption>";

                                        _HTMLReport = _HTMLReport + "  <tr>";
                                        _HTMLReport = _HTMLReport + " <th colspan=\"2\">COLUMN_NAME</th>";
                                               
                                        _HTMLReport = _HTMLReport + "  </tr>";



                                        string PackageColumns = "Select * from [COLUMNS] where [PACKAGE_ID] = " + Convert.ToInt32(PackageID);

                                        PackageColumns = PackageColumns + "and  [TABLE_ID] = " + Convert.ToInt32(TableID);

                                        using (SqlConnection conCL = new SqlConnection(_ConnectionStringPackage))
                                        {
                                            conCL.Open();
                                            using (SqlCommand cmdCL = new SqlCommand(PackageColumns, conCL))
                                            {

                                                cmdCL.CommandType = CommandType.Text;

                                                using (SqlDataReader idrCL = cmdCL.ExecuteReader())
                                                {

                                                    if (idrCL.HasRows)
                                                    {
                                                        // Call Read before accessing data. 
                                                        while (idrCL.Read())
                                                        {

                                                            string ColumnName = string.Empty;
                                                            int ColumnID = 0;


                                                            if (idrCL["COLUMN_NAME"] != System.DBNull.Value)
                                                            {
                                                                ColumnName = Convert.ToString((string)idrCL["COLUMN_NAME"]);
                                                            }


                                                            if (idrCL["COLUMN_ID"] != System.DBNull.Value)
                                                            {
                                                                ColumnID = Convert.ToInt32((int)idrCL["COLUMN_ID"]);
                                                            }


                                                            _HTMLReport = _HTMLReport + "  <tr>";
                                                            {
                                                                _HTMLReport = _HTMLReport + " <td colspan=\"2\" >" + ColumnName + "</td>";
                                                            
                                                            
                                                            
                                                            
                                                            
                                                            
                                                            
                                                            }
                                                            _HTMLReport = _HTMLReport + "  </tr>";


                                                        }
                                                    }
                                                }
                                            }
                                        }

                                    }
                                    _HTMLReport = _HTMLReport + "</table>";

                                }
                                //if (idr["TABLE_NAME"] != System.DBNull.Value)
                                //{
                                //    lstSourceTable.Items.Add(Convert.ToString((string)idr["TABLE_NAME"]));
                                //}

                            }
                        }
                    }
                }
            }



            _HTMLReport = _HTMLReport + "</body>";
            _HTMLReport = _HTMLReport + "</html>";





            wbReport.Navigate("about:blank");

            if (wbReport.Document != null)
            {

                wbReport.Document.Write(string.Empty);

            }

            wbReport.DocumentText = _HTMLReport;


        }
    }
}
