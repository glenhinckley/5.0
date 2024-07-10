using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;
using System;
using System.Configuration;
using System.Data;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.IO.Pipes;
using System.Windows.Forms;




using DCSGlobal.RealAlert.Client;

using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;

using DCSGlobal.BusinessRules.Logging;
using System.Configuration;
using System.Collections;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;
using System.Data.SqlClient;
using System.Data.Sql;


namespace Manual_test_app
{
    public partial class RAServerTest : Form
    {


        private static int idx = 0;
        private static Dictionary<int, string> BatchIdTaskDictionary = new Dictionary<int, string>();

        private static int numThreads = 4;
        private static int iEDIKey = 0;
        private static int iEDICKey = 0;
        private static config c = new config();
        private static string AppName = "DCSGlobal.Eligibility.TaskConsole";
        private static Int32 MaxCount;
        private static Int32 TotalThreadCount = 0;
        private static logExecption log = new logExecption();
        private static StringStuff ss = new StringStuff();

        NamedPipe ra = new NamedPipe();
        List<string> LineList = new List<string>();

        List<string> TempLineList = new List<string>();


        public RAServerTest()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmdGo_Click(object sender, EventArgs e)
        {








        }

        private void Form1_Load(object sender, EventArgs e)
        {


            GetAppConfig();
            //  IsSingleInstance();
            AppName = c.ConsoleName;
            log.ConnectionString = c.ConnectionString;
            txtConString.Text = c.ConnectionString;
            txtInstanceName.Text = "RuleEngineTestServer";
        }



        private void GetData()
        {

            // LineList.Clear();


            lstTheBox.DataSource = null;
            cmbTheBox.DataSource = null;
            txtRawdata.Text = string.Empty;

            ra.ConnectionString = txtConString.Text;
            ra.operator_id = txtoperator_id.Text;
            ra.InstanceName = txtInstanceName.Text;
            ra.ServerName = txtIP.Text;

            string ret = string.Empty;
            string retCode = string.Empty;
            ret = ra.GetRowData();





            string OriginalMSG = string.Empty;

            retCode = ret.Substring(0, Math.Min(ret.Length, 3));

            //OriginalMSG = ss;

            switch (retCode)
            {
                case "WAT":

                    toolStripStatusLabel1.Text = "WAT";

                    break;

                case "ONF":

                    toolStripStatusLabel1.Text = "ONF";

                    break;


                case "NDD":

                    toolStripStatusLabel1.Text = "NDD";

                    break;


                case "BND":

                    toolStripStatusLabel1.Text = "BND";

                    break;


                case "TMO":

                    toolStripStatusLabel1.Text = "TMO";

                    break;


                case "RES":

                    toolStripStatusLabel1.Text = "RES";

                    ret = ret.Substring(3, ret.Length - 3);
                    txtRawdata.Text = ret;

                    string line = string.Empty;
                    int i = 1;



                    //   DAVID.PEREZFERNA-CON




                    do
                    {
                        line = "^";
                        line = ss.ParseDemlimtedString(ret, "~", i);
                        if (line != "^")
                        {
                            TempLineList.Add(line);
                        }


                        i++;

                    }
                    while (line != "^");


                    DeDupe(TempLineList);


                    lstTheBox.DataSource = LineList;

                    IList RAList = new ArrayList();

                    RAStrings RA = new RAStrings();


                    // get the populate a genrec list that we can loop throug to populate the Ilist
                    // i know this loooks awfull but its fine hsut deall with it. lambda expression are no good but it works
                    // List<string> ids = Session["RAList"] != null ? (List<string>)Session["RAList"] : null;

                    // loop to add the items to the ilist
                    if (LineList != null)
                    {
                        foreach (string id in LineList)
                        {
                            RA = new RAStrings();
                            RA.RAKEY = Convert.ToString(ss.ParseDemlimtedString(id, "|", 1));
                            RA.RAValue = id;//Convert.ToString(ss.ParseDemlimtedString(id, "|", 2));
                            RAList.Add(RA);
                        }

                    }

                    // bind the ilist to the cmb box and thats it were done
                    this.cmbTheBox.DataSource = RAList;
                    this.cmbTheBox.DisplayMember = "RAValue";
                    this.cmbTheBox.ValueMember = "RAKEY";
                    //this.cmbTheBox.DataBind();

                    KillList();
                    break;

                case "ERR":

                    toolStripStatusLabel1.Text = "ERR";

                    break;


                default:

                    toolStripStatusLabel1.Text = "UNK";
                    // s = sss;
                    break;       // break necessary on default
            }


            toolStripStatusLabel3.Text = "Last Update " + Convert.ToString(DateTime.Now);


        }



        private void DeDupe(List<string> TheList)
        {

            if (TheList != null)
            {
                foreach (string id in TheList)
                {

                    if (!LineList.Contains(id))
                    {
                        LineList.Add(id);

                    }

                }
            }

        }


        static void GetAppConfig()
        {
            try
            {
                c.ConnectionString = ConfigurationManager.AppSettings["ConnStr"];
                c.CommandTimeOut = ConfigurationManager.AppSettings["CommandTimeOut"];
                c.SYNC_TIMEOUT = ConfigurationManager.AppSettings["SYNC_TIMEOUT"];
                c.SUBMISSION_TIMEOUT = ConfigurationManager.AppSettings["SUBMISSION_TIMEOUT"];
                c.getAllDataSp = ConfigurationManager.AppSettings["getAllDataSp"];
                c.ErrorLog = ConfigurationManager.AppSettings["ErrorLog"];
                c.isParseVBorDB = ConfigurationManager.AppSettings["isParseVBorDB"];
                c.isEmdeonLookUp = ConfigurationManager.AppSettings["isEmdeonLookUp"];
                c.reRunEligAttempts = ConfigurationManager.AppSettings["reRunEligAttempts"];
                c.uspEdiRequest = ConfigurationManager.AppSettings["uspEdiRequest"];
                c.uspEdiDbImport = ConfigurationManager.AppSettings["uspEdiDbImport"];
                c.LogPath = ConfigurationManager.AppSettings["LogPath"];
                c.ConsoleName = ConfigurationManager.AppSettings["ConsoleName"];
                c.MaxCount = ConfigurationManager.AppSettings["MaxCount"];
                c.MaxThreads = ConfigurationManager.AppSettings["MaxThreads"];
                c.ThreadsON = ConfigurationManager.AppSettings["ThreadsON"];
                c.WaitForEnterToExit = ConfigurationManager.AppSettings["WaitForEnterToExit"];
                c.NoDataTimeOut = ConfigurationManager.AppSettings["CommandTimeOut"];



            }
            catch (Exception ex)
            {

                Console.WriteLine("Unable to laod Configuration Data app exited");
                Environment.Exit(0);
            }



        }


        private void KillList()
        {
            //USP_REAL_ALERT_50_CLIENT @INPUT VARCHAR(1000)

            //USP_REAL_ALERT_50_CLIENT '40010516016|JHS|766203|2015-03-04 04:45:05.200|40010516016-JHS|MERCEDES MONTERREY|4348215|ANDREA.BASTIDAS'

            using (SqlConnection con = new SqlConnection(c.ConnectionString))
            {

                con.Open();

                if (LineList != null)
                {
                    foreach (string id in LineList)
                    {
                        using (SqlCommand cmd = new SqlCommand("USP_REAL_ALERT_50_CLIENT", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@INPUT", id);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }


        private void cmdKillList_Click(object sender, EventArgs e)
        {


          //  KillList();
        //
            //SqlConnection db = new SqlConnection(c.ConnectionString);
            ////  SqlTransaction transaction;

            //db.Open();

            //try
            //{

            //    if (LineList != null)
            //    {
            //        foreach (string id in LineList)
            //        {
            //            new SqlCommand("USP_REAL_ALERT_50_CLIENT " + "'" + id + "'", db).ExecuteNonQuery();
            //        }

            //    }
            //    // foreach(r in rows) 



            //}
            //catch (SqlException sqlError)
            //{

            //    //sqlError.

            //}
            //catch (Exception ex)
            //{

            //}

            //db.Close();


        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void tmGo_Tick(object sender, EventArgs e)
        {
           // GetData();


        }

        private void cmdRun_Click(object sender, EventArgs e)
        {
            tmGo.Enabled = true;
            toolStripStatusLabel2.Text = "Timer Enabled";
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void txtConString_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// builds a memory stream
        /// </summary>
        /// <returns></returns>
        static MemoryStream BuildStream()
        {

            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            //writer.Write((System.Text.Encoding.UTF8.GetBytes(GetNextERBID())));
            //   writer.Write(GetNextERBID());

            writer.Flush();
            stream.Position = 0;



            return stream;

        }

        private void cmdWCFLogin_Click(object sender, EventArgs e)
        {


            int r = -1;




            using (sr.SampleServiceClient s = new sr.SampleServiceClient())
            {

                r = s.Login(txtoperator_id.Text, txtHospCode.Text);


            }


         
            
            
            //int r = -1;

            //using (WCF s = new WCF())
            //{
            //    r = s.Login(txtoperator_id.Text, txtHospCode.Text);

            //}


            txtRawdata.Text = Convert.ToString(r);

        }

        private void cmdWCFGet_Click(object sender, EventArgs e)
        {

            string r = "ClientError";

            //using (WCF s = new WCF())
            //{
            //    r = s.GetRealMessages(txtoperator_id.Text, txtHospCode.Text);

            //}


            using (sr.SampleServiceClient s = new sr.SampleServiceClient())
            {

                r = s.GetRealMessages(txtoperator_id.Text, txtHospCode.Text, txtoperator_id.Text);


            }


            txtRawdata.Text = Convert.ToString(r);



        }

        private void cmdWCFKillList_Click(object sender, EventArgs e)
        {

           int r = -1;

            using(WCF s = new WCF())
            {
              r =  s.KillList(txtRawdata.Text);


            }


            txtRawdata.Text = Convert.ToString(r);

        }


        }

    }


    class RAStrings
    {
        private string _RAKEY;
        private string _RAValue;

        public string RAValue
        {
            get { return _RAValue; }
            set { _RAValue = value; }
        }

        public string RAKEY
        {
            get { return _RAKEY; }
            set { _RAKEY = value; }
        }
    }



