using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DCSGlobal.BusinessRules.CoreLibraryII
{
 
    public class  SchemaDetails //: IDisposable 






    {

        public void _2()
        {


            string constr = "";
            string query = "SELECT TOP 10 ContactName, City, Country FROM Customers";
            query += " GO ";
            query += "SELECT TOP 10 (FirstName + ' ' + LastName) EmployeeName, City, Country FROM Employees";

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataSet ds = new DataSet())
                        {
                            sda.Fill(ds);
                          /* 
                           * gvCustomers.DataSource = ds.Tables[0];
                            gvCustomers.DataBind();
                            gvEmployees.DataSource = ds.Tables[1];
                            gvEmployees.DataBind();
                           */
                        }
                    }
                }
            }
        }



        private void GetMaxLenght(string TableName)
        {
            string connetionString = null;
            SqlConnection sqlCnn ;
            SqlCommand sqlCmd ;
            string sql = null;

            connetionString = "Data Source=ServerName;Initial Catalog=DatabaseName;User ID=UserName;Password=Password";
            sql = "Select * from product";

            sqlCnn = new SqlConnection(connetionString);
            try
            {
                sqlCnn.Open();
                sqlCmd = new SqlCommand(sql, sqlCnn);
                SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                DataTable schemaTable = sqlReader.GetSchemaTable();

                foreach (DataRow row in schemaTable.Rows)
                {
                    foreach (DataColumn column in schemaTable.Columns)
                    {
                     //   column.MaxLength 
                        
                        // MessageBox.Show (string.Format("{0} = {1}", column.ColumnName, row[column]));
                    }
                }
                sqlReader.Close();
                sqlCmd.Dispose();
                sqlCnn.Close();
            }
            catch (Exception ex)
            {
               // MessageBox.Show("Can not open connection ! ");
            }
        }
    }
}

