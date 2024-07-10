using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace Manual_test_app.Rules
{
    public partial class RuleResults : Form
    {


     

        private string _ConnectionString = "Data Source=10.1.1.120;Initial Catalog=al60_eastmaine_prod;Persist Security Info=True;User ID=al60_seton_lite_developer_user;Password=al60_seton_lite_developer_password";


        public RuleResults()
        {
            InitializeComponent();
        }

        private void cmdRetriveEvents_Click(object sender, EventArgs e)
        {

            //
            using (SqlConnection con = new SqlConnection(_ConnectionString))
            {
                con.Open();
                //
                // Create new SqlCommand object.
                //
                using (SqlCommand cmd = new SqlCommand("usp_get_patient_events", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@patient_number", SqlDbType.VarChar).Value = txtPatNumber.Text;
                    cmd.Parameters.Add("@hospital_code", SqlDbType.VarChar).Value = cmbHOSP_CODE.SelectedText;

                    //   cmd.ExecuteNonQuery();//


                    using (SqlDataReader idr = cmd.ExecuteReader())
                    {
                        if (idr.HasRows)
                        {

                            while (idr.Read())
                            {


                            }

                        }

                    }
                    //    If idr.HasRows Then
                    //        While idr.Read
                    //            r.Add(idr.Item("hospital_code").ToString())
                    //        End While
                    //    End If
                    //End Using


                    // Invoke ExecuteReader method.
                    // 
                    // r = 0;
                }
                con.Close();
            }

        }

        private void RuleResults_Load(object sender, EventArgs e)
        {

        }

        private void cmdDecode_Click(object sender, EventArgs e)
        {

        }



        private void LoadForm()
        {






        }

        //            Dim SQLConn As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection
        //Dim sqlComm As SqlCommand

        //' Dim sqlString As String

        //Dim sqlReader As SqlDataReader
        //Dim eventlstItem As ListItem

        //Try
        //    SQLConn.ConnectionString = HttpContext.Current.Application("ConnectionString") 'Global.GlbConnStr
        //    SQLConn.Open()

        //    ''sqlString = "usp_get_patient_events"
        //    sqlComm = New SqlCommand("usp_get_patient_events", SQLConn)
        //    sqlComm.CommandType = CommandType.StoredProcedure
        //    sqlComm.CommandTimeout = CInt(ConfigurationManager.AppSettings("CommandTimeOut"))


        //    sqlComm.Parameters.Add("@patient_number", SqlDbType.VarChar)
        //    sqlComm.Parameters("@patient_number").Direction = ParameterDirection.Input
        //    sqlComm.Parameters("@patient_number").Value = txtAcctCode.Text

        //    sqlComm.Parameters.Add("@hospital_code", SqlDbType.VarChar)
        //    sqlComm.Parameters("@hospital_code").Direction = ParameterDirection.Input
        //    sqlComm.Parameters("@hospital_code").Value = ddlHospCode.SelectedValue

        //    sqlReader = sqlComm.ExecuteReader()

        //    ddlPatientEvents.Items.Clear()

        //    eventlstItem = New ListItem("Current Event", "0")
        //    ddlPatientEvents.Items.Add(eventlstItem)

        //    While sqlReader.Read()
        //        If Not IsDBNull(Trim(sqlReader.GetValue(0))) Then
        //            eventlstItem = New ListItem(Global.GeneralFuncs.ReplaceQuote(Trim(sqlReader.GetValue(1)), "'", "''"), Global.GeneralFuncs.ReplaceQuote(Trim(sqlReader.GetValue(0)), "'", "''"))
        //            ddlPatientEvents.Items.Add(eventlstItem)
        //            'dicHospCodes.Add(Global.GeneralFuncs.ReplaceQuote(Trim(hospReader.GetValue(0)), "'", "''"), Global.GeneralFuncs.ReplaceQuote(Trim(hospReader.GetValue(0)), "'", "''"))
        //        End If
        //    End While

        //    'Session("dicHospCodes") = dicHospCodes

        //Catch ex As System.Exception
        //    If Application("debug") = "Y" Then
        //        Response.Write(ex.Message)
        //    Else
        //        Session("stackTrace") = ex.StackTrace
        //        Response.Redirect("../qaGeneral/qaErrors.aspx?errID=1001&Msg=" & ex.Message.Replace(Environment.NewLine, " ") & "&Pg=validateRule.aspx&Md=Rules&Pr=getPatientEvents")
        //    End If
        //Finally
        //    SQLConn.Close()
        //    SQLConn = Nothing
        //    sqlComm = Nothing
        //    sqlReader = Nothing
        //End Try
    }
}

