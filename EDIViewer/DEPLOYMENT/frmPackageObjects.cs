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
using DCSGlobal.BusinessRules.db.UTYPES;

namespace Manual_test_app.DEPLOYMENT
{
    public partial class frmPackageObjects : Form
    {


        private string _ConnectionStringSource = "Data Source=10.1.1.120;Initial Catalog=al60_seton_lite_developer;Persist Security Info=True;User ID=al60_seton_lite_developer_user;Password=al60_seton_lite_developer_password";
        private string _ConnectionStringTarget = "Data Source=10.1.1.120;Initial Catalog=al60_seton_lite_developer;Persist Security Info=True;User ID=al60_seton_lite_developer_user;Password=al60_seton_lite_developer_password";


        private string _ConnectionStringPackage = "Integrated Security=SSPI;Initial Catalog=glen_db;Data Source=10.1.1.120";



        private string _dbTempStringSource = string.Empty;
        private string _dbTempStringPrivate = string.Empty;


        private int _PackageID = 0;
        private string _PackageName = string.Empty;

        StringStuff ss = new StringStuff();
        CodeInjection ci = new CodeInjection();
        logExecption log = new logExecption();



        public frmPackageObjects()
        {
            InitializeComponent();
            lblPackage.Text = "No package";


        }


        public frmPackageObjects(int PackageID, string PackageName)
        {
            InitializeComponent();
            _PackageID = PackageID;
            _PackageName = PackageName;


            lblPackage.Text = Convert.ToString(_PackageID) + " : " + _PackageName;


        }
        private void frmDBscan_Load(object sender, EventArgs e)
        {

        }

        private void cmdSourceDecode_Click(object sender, EventArgs e)
        {

        }

        private void cmdTargetDecode_Click(object sender, EventArgs e)
        {

        }

        private void cmdTableSearch_Click(object sender, EventArgs e)
        {

            string select = string.Empty;
            int cii = 1;
            try
            {



                select = "select *  from information_schema.tables  ";

                if (txtSearchTable.Text != "")
                {
                    if (chkSourceTablesStartsWith.Checked)
                    {


                        select = select + "where table_name   like   '" + txtSearchTable.Text + "%'";

                    }
                    else
                    {
                        select = select + "where table_name   like  '%" + txtSearchTable.Text + "%'";
                    }
                }

                select = select + " order by table_name";
                cii = ci.CheckInput(select);

                lstSourceTable.Items.Clear();

                using (SqlConnection con = new SqlConnection(_ConnectionStringSource))
                {
                    con.Open();
                    //
                    // Create new SqlCommand object.
                    //
                    using (SqlCommand cmd = new SqlCommand(select, con))
                    {

                        cmd.CommandType = CommandType.Text;

                        using (SqlDataReader idr = cmd.ExecuteReader())
                        {

                            if (idr.HasRows)
                            {
                                // Call Read before accessing data. 
                                while (idr.Read())
                                {


                                    if (idr["TABLE_NAME"] != System.DBNull.Value)
                                    {
                                        lstSourceTable.Items.Add(Convert.ToString((string)idr["TABLE_NAME"]));
                                    }

                                }
                            }
                        }
                    }
                }


            }

            catch (Exception ex)
            {


                tspMessage.Text = ex.Message;

            }
        }

        private void cmdTableAdd_Click(object sender, EventArgs e)
        {
            lstTargetTable.Items.Add(lstSourceTable.SelectedItem);
        }

        private void cmdTableRemove_Click(object sender, EventArgs e)
        {


            string TargetRemove = string.Empty;

            TargetRemove = lstTargetTable.SelectedItem.ToString();


            for (int n = lstTargetTable.Items.Count - 1; n >= 0; --n)
            {
                //    string removelistitem = "OBJECT";
                if (lstTargetTable.Items[n].ToString().Contains(TargetRemove))
                {
                    lstTargetTable.Items.RemoveAt(n);
                }
            }
        }

        private void cmdTableAddAll_Click(object sender, EventArgs e)
        {
            int x = 0;
            for (int n = lstSourceTable.Items.Count - 1; n >= 0; --n)
            {


                lstTargetTable.Items.Add(lstSourceTable.Items[x].ToString());
                //string removelistitem = "OBJECT";
                //if (listBox1.Items[n].ToString().Contains(removelistitem))
                //{
                //    listBox1.Items.RemoveAt(n);
                //}
                x++;

            }


        }

        private void cmdTableRemoveAll_Click(object sender, EventArgs e)
        {
            lstTargetTable.Items.Clear();
        }

        private void cmdSPSearch_Click(object sender, EventArgs e)
        {


            string select = string.Empty;
            int cii = 1;
            try
            {

                select = "SELECT * FROM INFORMATION_SCHEMA.ROUTINES  ";

                if (txtSearchSP.Text != "")
                {
                    if (chkSourceSPStartsWith.Checked)
                    {


                        select = select + "where specific_name   like   '" + txtSearchSP.Text + "%'";

                    }
                    else
                    {
                        select = select + "where specific_name   like  '%" + txtSearchSP.Text + "%'";
                    }
                }



                select = select + " order by specific_name";


                cii = ci.CheckInput(select);

                lstSourceSP.Items.Clear();

                using (SqlConnection con = new SqlConnection(_ConnectionStringSource))
                {
                    con.Open();
                    //
                    // Create new SqlCommand object.
                    //
                    using (SqlCommand cmd = new SqlCommand(select, con))
                    {

                        cmd.CommandType = CommandType.Text;

                        using (SqlDataReader idr = cmd.ExecuteReader())
                        {

                            if (idr.HasRows)
                            {
                                // Call Read before accessing data. 
                                while (idr.Read())
                                {


                                    if (idr["specific_name"] != System.DBNull.Value)
                                    {
                                        lstSourceSP.Items.Add(Convert.ToString((string)idr["specific_name"]));
                                    }

                                }
                            }
                        }
                    }
                }

            }

            catch (Exception ex)
            {


                tspMessage.Text = ex.Message;

            }
        }

        private void cmdSPAddAll_Click(object sender, EventArgs e)
        {
            int x = 0;
            for (int n = lstSourceSP.Items.Count - 1; n >= 0; --n)
            {


                lstTargetSP.Items.Add(lstSourceTable.Items[x].ToString());
                //string removelistitem = "OBJECT";
                //if (listBox1.Items[n].ToString().Contains(removelistitem))
                //{
                //    listBox1.Items.RemoveAt(n);
                //}
                x++;

            }
        }

        private void cmdUTYPESearch_Click(object sender, EventArgs e)
        {


            string select = string.Empty;
            int cii = 1;
            try
            {

                select = "select * from sys.types where is_user_defined = 1 and system_type_id = 243";

                if (txtSearchUTYPE.Text != "")
                {
                    if (chkSourceSPStartsWith.Checked)
                    {



                        select = "select * from sys.types  ";
                        select = select + "where name  like   '" + txtSearchUTYPE.Text + "%' ";
                        select = select + "and is_user_defined = 1 and system_type_id = 243";

                    }
                    else
                    {

                        select = "select * from sys.types  ";
                        select = select + "where name   like  '%" + txtSearchUTYPE.Text + "%' ";
                        select = select + "and is_user_defined = 1 and system_type_id = 243";
                    }
                }



                select = select + " order by name";


                cii = ci.CheckInput(select);

                lstSourceUTYPE.Items.Clear();

                using (SqlConnection con = new SqlConnection(_ConnectionStringSource))
                {
                    con.Open();
                    //
                    // Create new SqlCommand object.
                    //
                    using (SqlCommand cmd = new SqlCommand(select, con))
                    {

                        cmd.CommandType = CommandType.Text;

                        using (SqlDataReader idr = cmd.ExecuteReader())
                        {

                            if (idr.HasRows)
                            {
                                // Call Read before accessing data. 
                                while (idr.Read())
                                {


                                    if (idr["name"] != System.DBNull.Value)
                                    {
                                        lstSourceUTYPE.Items.Add(Convert.ToString((string)idr["name"]));
                                    }

                                }
                            }
                        }
                    }
                }

            }

            catch (Exception ex)
            {


                tspMessage.Text = ex.Message;

            }


        }

        private void cmdUTYPEAddAll_Click(object sender, EventArgs e)
        {
            int x = 0;
            for (int n = lstSourceUTYPE.Items.Count - 1; n >= 0; --n)
            {


                lstTargetUTYPE.Items.Add(lstSourceTable.Items[x].ToString());
                //string removelistitem = "OBJECT";
                //if (listBox1.Items[n].ToString().Contains(removelistitem))
                //{
                //    listBox1.Items.RemoveAt(n);
                //}
                x++;

            }
        }

        private void cmdUTYPEAdd_Click(object sender, EventArgs e)
        {
            lstTargetUTYPE.Items.Add(lstSourceUTYPE.SelectedItem);
        }

        private void cmdUTYPERemove_Click(object sender, EventArgs e)
        {

            string TargetRemove = string.Empty;

            TargetRemove = lstTargetUTYPE.SelectedItem.ToString();


            for (int n = lstTargetUTYPE.Items.Count - 1; n >= 0; --n)
            {
                //    string removelistitem = "OBJECT";
                if (lstTargetUTYPE.Items[n].ToString().Contains(TargetRemove))
                {
                    lstTargetUTYPE.Items.RemoveAt(n);
                }
            }
        }

        private void cmdUTYPERemoveAll_Click(object sender, EventArgs e)
        {


            lstTargetUTYPE.Items.Clear();
        }

        private void cmdSPAdd_Click(object sender, EventArgs e)
        {
            lstTargetSP.Items.Add(lstSourceSP.SelectedItem);
        }

        private void cmdRemoveSP_Click(object sender, EventArgs e)
        {

            string TargetRemove = string.Empty;

            TargetRemove = lstTargetSP.SelectedItem.ToString();


            for (int n = lstTargetSP.Items.Count - 1; n >= 0; --n)
            {
                //    string removelistitem = "OBJECT";
                if (lstTargetSP.Items[n].ToString().Contains(TargetRemove))
                {
                    lstTargetSP.Items.RemoveAt(n);
                }
            }
        }

        private void cmdRemoveSPALL_Click(object sender, EventArgs e)
        {


            lstTargetSP.Items.Clear();
        }

        private void cmdSavePackage_Click(object sender, EventArgs e)
        {

            SavePackage();
        }


        private void SavePackage()
        {
            int x = 0;

          //  SavePackageTables();

            SavePackageUTYPES();


        }

        private int SavePackageTables()
        {
            int r = -1;
            int x = 0;



            for (int n = lstTargetTable.Items.Count - 1; n >= 0; --n)
            {


                string TableName = string.Empty;
                int TableID = 0;

                TableName = lstTargetTable.Items[x].ToString();
                //string removelistitem = "OBJECT";
                //if (listBox1.Items[n].ToString().Contains(removelistitem))
                //{
                //    listBox1.Items.RemoveAt(n);
                //}
                x++;


                using (TableMaintance tm = new TableMaintance())
                {

                    TableID = tm.AddTabletoPackage(TableName, _PackageID);


                }


                using (ColumnMaintance cm = new ColumnMaintance())
                {
                    cm.SaveTableColumnstoPackage(TableName, TableID, _PackageID);

                }


                r = 0;



            }

            return r;
        }

        private void SavePackageUTYPES()
        {
            int x = 0;



            for (int n = lstTargetUTYPE.Items.Count - 1; n >= 0; --n)
            {


                string UTYPEName = string.Empty;
                int UTYPEID = 0;

                UTYPEName = lstTargetUTYPE.Items[x].ToString();
                //string removelistitem = "OBJECT";
                //if (listBox1.Items[n].ToString().Contains(removelistitem))
                //{
                //    listBox1.Items.RemoveAt(n);
                //}
                x++;


                using (UTYPETables utm = new UTYPETables())
                {

                    UTYPEID = utm.AddUDTTtoPackage(UTYPEName, _PackageID);


                }


                //using (UTYPEColumnMaintance cm = new UTYPEColumnMaintance())
                //{
                //    cm.SaveTableColumnstoPackage(UTYPEName, UTYPEID, _PackageID);

                //}






            }


        }


    }
}
