using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NAudio.Wave;

using NAudio.CoreAudioApi;

namespace DCSGlobal.Medical.VoiceRecoder
{
    public partial class frmListener : Form
    {

        private static bool _PhoneFound = false;
        private static bool _MicFound = false;

        public frmListener()
        {
            InitializeComponent();
        }

        private static string _UserName = string.Empty;

        public string UserName
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                _UserName = value;
            }
        }

        private void frmListener_Load(object sender, EventArgs e)
        {
            lblUserName.Text = _UserName;
            string ng = "";
            string g = "";
            string name = "";
            string m = "";

           for (int n = 0; n < WaveIn.DeviceCount; n++)
           {
               g = Convert.ToString(WaveIn.GetCapabilities(n).ProductGuid);
               name = Convert.ToString(WaveIn.GetCapabilities(n).ProductName);
               m =  Convert.ToString(WaveIn.GetCapabilities(n).ManufacturerGuid);
               ng  = Convert.ToString(WaveIn.GetCapabilities(n).NameGuid);


               //jim
               //2019470760

               // crapo they have the same guids

               if (g == "abcc5b90-c263-463b-a72f-a5bf64c86eba")
               {
                   cmdPhone.Enabled = true;
               }

            

               //list.Add(WaveIn.GetCapabilities(n).ProductName);
               // cmbSource.Items.Add(WaveIn.GetCapabilities(n).ProductName);
               // DeviceCount++;
            }


            //if (DeviceCount == 0)
            //{
            //    cmbSource.Items.Add("No Recording Device Found");

            //}



        }

        private void cmdExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void cmdPhone_Click(object sender, EventArgs e)
        {
            frmMain f = new frmMain();
            f.UserName = _UserName;
            f.Show();
        }

        private void cmdMicrophone_Click(object sender, EventArgs e)
        {

            frmMain f = new frmMain();
            f.UserName = _UserName;
            f.Show();

        }

        private void cmdOptions_Click(object sender, EventArgs e)
        {
            frmOptions fo = new frmOptions();
            fo.Show();
        }
    }
}
