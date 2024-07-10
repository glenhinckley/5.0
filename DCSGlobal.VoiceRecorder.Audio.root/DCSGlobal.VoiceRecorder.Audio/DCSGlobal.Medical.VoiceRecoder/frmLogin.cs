using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DCSGlobal.Medical.VoiceRecoder
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }


        AppSettings ap = new AppSettings();

        private void cmdLogin_Click(object sender, EventArgs e)
        {

            int i = -1;
            using (srSecurity.SecuritySoapClient sr = new srSecurity.SecuritySoapClient())
            {

                i = sr.Login(txtUserName.Text, txtPassword.Text);


                lblError.Text = Convert.ToString(i);


                if (i == 0)
                {

                    frmListener f = new frmListener();
                    f.UserName = txtUserName.Text;
                    f.Show();
                    this.Hide();


                }
                else
                {
                    lblError.ForeColor = System.Drawing.Color.Red;


                    if (i == -1)
                    {
                        lblError.Text = "Login failed: Incorrect username or password"; 
                    }
                    else
                    { 
                        lblError.Text = "Login failed: with code " + Convert.ToString(i); 
                    }


                }


            }

        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

            ap.GetAll();

            if(TestEndPoint())
            {

            }
            else
            {


            }

        }



        private bool TestEndPoint()
        {
            bool b =  false;
               b = ap.TestURLEndPoint(ap.EndPointURI);
            return b;
        }





        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            txtPassword.UseSystemPasswordChar = false;
        }

        private void cmdSeePasswd_MouseUp(object sender, MouseEventArgs e)
        {
            txtPassword.UseSystemPasswordChar = true;
        }
    }
}
