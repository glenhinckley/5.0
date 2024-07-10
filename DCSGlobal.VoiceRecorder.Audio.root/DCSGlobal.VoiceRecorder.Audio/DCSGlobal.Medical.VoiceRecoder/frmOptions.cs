using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.IO;

using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using DCSGlobal.BusinessRules.CoreLibraryII;

using NAudio.CoreAudioApi;



namespace DCSGlobal.Medical.VoiceRecoder
{
    public partial class frmOptions : Form
    {

        MachineID objMID = new MachineID();
        string d = string.Empty;


        StringStuff ss = new StringStuff();
        int _PagerKey;


        AppSettings ap = new AppSettings();

        static string _HardwareID = string.Empty;

        private int l = 100;
        private int t = 100;

        private string location = string.Empty;


        public frmOptions()
        {
            InitializeComponent();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdApply_Click(object sender, EventArgs e)
        {

        }

        private void cmOK_Click(object sender, EventArgs e)
        {



            ap.FilePath = txtTempFolder.Text.TrimEnd(Path.DirectorySeparatorChar);




            ap.EndPointURI = txtEndPoint.Text;
            this.Close();
        }


        private void MovePanels(string PanelName)
        {

            foreach (Control c in this.Controls)
            {
                if (c is Panel)
                {

                    if (c.Name == PanelName)
                    {
                        c.Left = 12;
                        c.Top = 93;
                    }
                    else
                    {
                        c.Left = 12;
                        c.Top = 5000;
                    }


                }
                // Do stuff here ;]
            }
        }





        private void cmdServer_Click(object sender, EventArgs e)
        {

            MovePanels("pnlServer");




        }

        private void cmdLevels_Click(object sender, EventArgs e)
        {

            MovePanels("pnlLevels");



        }

        private void cmdPaths_Click(object sender, EventArgs e)
        {


            MovePanels("pnlLevels");
            pnlLevels.Top = 12;
            pnlLevels.Left = 5000;

            pnlServer.Top = 12;
            pnlServer.Left = 5000;

            pnlPaths.Top = 12;
            pnlPaths.Left = 93;


        }

        private void frmOptions_Load(object sender, EventArgs e)
        {
            ap.GetAll();


            MMDeviceEnumerator enu = new MMDeviceEnumerator();
            var devices = enu.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);
            cmbDeviceList.Items.AddRange(devices.ToArray());

            pnlLevels.Top = 12;
            pnlLevels.Left = 93;

            pnlServer.Top = 12;
            pnlServer.Left = 5000;

            pnlPaths.Top = 12;
            pnlPaths.Left = 5000;

            //559, 409

            this.Left = Convert.ToInt32(Properties.Settings.Default["frmOptionsLocation_Left"]);
            this.Top = Convert.ToInt32(Properties.Settings.Default["frmOptionsLocation_Top"]);
            this.Size = new Size(559, 409);

            try
            {
                txtTempFolder.Text = ap.FilePath;
                txtEndPoint.Text = ap.EndPointURI;
            }

            catch (Exception ex)
            { }






        }

        private void frmOptions_LocationChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default["frmOptionsLocation_Left"] = this.Left;
            Properties.Settings.Default["frmOptionsLocation_Top"] = this.Top;
            Properties.Settings.Default.Save(); // Saves settings in application configuration file
        }

        private void timer1_Tick(object sender, EventArgs e)
        {


            if (cmbDeviceList.SelectedItem != null)
            {
                var device = (MMDevice)cmbDeviceList.SelectedItem;
                progressBar1.Value = (int)(Math.Round(device.AudioMeterInformation.MasterPeakValue * 100));


            }
        }

        private void cmbDeviceList_SelectedIndexChanged(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void pnlLevels_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cmdBrowse_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {

                txtTempFolder.Text = folderBrowserDialog1.SelectedPath;
                //
                // The user selected a folder and pressed the OK button.
                // We print the number of files found.
                //


            }
        }





        private void cmdReset_Click(object sender, EventArgs e)
        {
            ap.ResetPager();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void cmdTestServer_Click(object sender, EventArgs e)
        {
            if (ap.TestURLEndPoint(txtEndPoint.Text))
            {
                lblEPPF.ForeColor = System.Drawing.Color.Green;
                lblEPPF.Text = "PASS";
                lblServerTestStatus.Text = "Connection Test Complete";
            }
            else
            {
                lblServerTestStatus.Text = "Connection Test Failed";
            }
        }



        private void cmdRegister_Click(object sender, EventArgs e)
        {

        }

        public bool SetupEndPoint()
        {
            bool b = false;


            return b;

        }



    }


}
