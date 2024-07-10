using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;



using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using DCSGlobal.BusinessRules.CoreLibraryII;
using DCSGlobal.BusinessRules.CoreLibrary;
using DCSGlobal.BusinessRules.Logging;
using DCSGlobal.BusinessRules.CoreLibrary.dbCheckStuff;


namespace Manual_test_app.LinQ
{
    public partial class LinQPad : Form
    {

        private string _ConnectionString = string.Empty;

        private StringStuff ss = new StringStuff();
        private CodeInjection ci = new CodeInjection();
        private DataSet ds = new DataSet();
        
        public LinQPad()
        {
            InitializeComponent();

            _ConnectionString = Config._instance.ConnectionString;

        }

        private void LinQPad_Load(object sender, EventArgs e)
        {
           
        }




        private void button1_Click(object sender, EventArgs e)
        {
            string connetionString = null;
            SqlConnection sqlCnn;
            SqlCommand sqlCmd;
            string sql = null;

            connetionString = "Data Source=ServerName;Initial Catalog=DatabaseName;User ID=UserName;Password=Password";
            sql = "";

            sqlCnn = new SqlConnection(connetionString);
            try
            {
                sqlCnn.Open();
                sqlCmd = new SqlCommand(sql, sqlCnn);
                SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                while (sqlReader.Read())
                {
                    MessageBox.Show("From first SQL - " + sqlReader.GetValue(0) + " - " + sqlReader.GetValue(1));
                }

                sqlReader.NextResult();

                while (sqlReader.Read())
                {
                    MessageBox.Show("From second SQL - " + sqlReader.GetValue(0) + " - " + sqlReader.GetValue(1));
                }

                sqlReader.NextResult();

                while (sqlReader.Read())
                {
                    MessageBox.Show("From third SQL - " + sqlReader.GetValue(0) + " - " + sqlReader.GetValue(1));
                }

                sqlReader.Close();
                sqlCmd.Dispose();
                sqlCnn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection ! ");
            }
        }

        private void cmdRunQuery_Click(object sender, EventArgs e)
        {
            ds.Clear();
            int cii = 0;
            string Select = string.Empty;
            int ResultSet = 0;

            ResultSet = Convert.ToInt32(txtRS.Text);


            Select = txtSQL.Text;
            cii = ci.CheckInput(Select);
          
            if (cii == 0)
            {
                var c = new SqlConnection(_ConnectionString); // Your Connection String here
                var dataAdapter = new SqlDataAdapter(Select, c);

                var commandBuilder = new SqlCommandBuilder(dataAdapter);
             
                dataAdapter.Fill(ds);
                // dataGridView2.ReadOnly = true;
                dataGridView2.DataSource = ds.Tables[ResultSet];
            }
            else
            {
                tslCS.Text = "BAD BAD BAD, NO DONT DO IT, YOU CANT USE " + ci.KEY_WORDLIST;
                tslCS.ForeColor = System.Drawing.Color.Red;

            }
        }

        private void cmdRunLinq_Click(object sender, EventArgs e)
        {
            //// Fill the DataSet.
            //DataSet ds = new DataSet();
            //ds.Locale = CultureInfo.InvariantCulture;
            //FillDataSet(ds);

            //DataTable orders = ds.Tables["SalesOrderHeader"];
        }
    }
}

