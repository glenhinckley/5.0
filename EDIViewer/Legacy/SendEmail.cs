using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DCSGlobal.BusinessRules.Logging;
using System.Configuration;

namespace Manual_test_app
{
    public partial class SendEmail : Form
    {

         static AppSettingsReader app = new AppSettingsReader();
         static Email em = new Email();
         static string _SMTPServer = string.Empty;
         static string _FromMailAddress = string.Empty;
         static string _ToMailAddress = string.Empty;
         static string _ConnectionString = string.Empty;

        public SendEmail()
        {
            InitializeComponent();
        

        }

        private void cmdSend_Click(object sender, EventArgs e)
        {


            em.SendTo = txtTo.Text;
            em.SendFrom = _FromMailAddress;
            em.SendGenericEmail(txtBody.Text, txtSubject.Text, "Test APP");


        }

        //       <add key="SMTPServer" value="10.1.1.108"/>
        //<add key="FromMailAddress" value="no-reply@dcsglobal.com"/>
        //<add key="ToMailAddress" value="qasupport@dcsglobal.com"/>

      private   static void GetSettings()
        {

           Console.Write("Get Settings Start\n\r");
            try
            {

                _ConnectionString = Convert.ToString(app.GetValue("ConnStr", _ConnectionString.GetType()));
                _SMTPServer = Convert.ToString(app.GetValue("SMTPServer", _SMTPServer.GetType())); //ConfigurationSettings.AppSettings["BaseURL"].ToString();
                _FromMailAddress = Convert.ToString(app.GetValue("FromMailAddress", _FromMailAddress.GetType())); //ConfigurationSettings.AppSettings["BaseURL"].ToString();
                _ToMailAddress = Convert.ToString(app.GetValue("ToMailAddress", _ToMailAddress.GetType()));









                //  log.ConnectionString = _ConnectionString;

            }
            catch (Exception ex)
            {


                //     Console.Write("Get Settings Fail" + ex.Message + "\n\r");
                //     Environment.Exit(0);

            }
            // Console.Write("Runing as: " + _InstanceName + "\n\r");
            // Console.Write("Get Settings Complete" + "\n\r");

        }

        private void SendEmail_Load(object sender, EventArgs e)
        {
            GetSettings();



            em.ConnectionString = _ConnectionString;
            em.Server = _SMTPServer;
            txtTo.Text = _ToMailAddress;

        }


    }
}
