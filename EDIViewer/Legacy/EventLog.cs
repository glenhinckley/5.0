using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DCSGlobal.BusinessRules.Logging;

namespace Manual_test_app
{
    public partial class EventLog : Form
    {
        LogToEventLog ev = new LogToEventLog();


        public EventLog()
        {
            InitializeComponent();



        }

        private void button1_Click(object sender, EventArgs e)
        {


            this.Close();



        }

        private void cmdWarning_Click(object sender, EventArgs e)
        {


            try
            {

                      ev.WriteEventWarning(txtEvent.Text, Convert.ToInt32(txtID.Text), txtDescription.Text);
            }

            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }

    
        }

        private void cmdError_Click(object sender, EventArgs e)
        {


            try
            {

                         ev.WriteEventError(txtEvent.Text, Convert.ToInt32(txtID.Text), txtDescription.Text); 
            }

            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }

        }

        private void cmdInformation_Click(object sender, EventArgs e)
        {


            try
            {
                 ev.WriteEventInformation(txtEvent.Text, Convert.ToInt32(txtID.Text), txtDescription.Text);
            }

            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
            
         
        }

        private void cmdFailureAudit_Click(object sender, EventArgs e)
        {


            try
            {

             ev.WriteEventFailureAudit(txtEvent.Text, Convert.ToInt32(txtID.Text), txtDescription.Text);
            }

            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
           
        }



    }
}
