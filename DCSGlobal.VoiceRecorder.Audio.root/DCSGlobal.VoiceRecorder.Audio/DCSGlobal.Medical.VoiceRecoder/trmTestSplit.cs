using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NAudio;
using NAudio.CoreAudioApi;
using NAudio.MediaFoundation;
using NAudio.Mixer;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;




namespace DCSGlobal.Medical.VoiceRecoder
{
    public partial class trmTestSplit : Form
    {

        private INAudioDemoPlugin currentPlugin;

        public trmTestSplit()
        {
            InitializeComponent();
        }
        private AudioFileReader wave = null;
        private WaveOut outputSound = null;


        private void trmTestSplit_Load(object sender, EventArgs e)
        {

            // use reflection to find all the demos
            var demos = ReflectionHelper.CreateAllInstancesOf<INAudioDemoPlugin>().OrderBy(d => d.Name);


            listBoxDemos.DisplayMember = "Name";
            foreach (var demo in demos)
            {
                listBoxDemos.Items.Add(demo);
            }

            Text += ((System.Runtime.InteropServices.Marshal.SizeOf(IntPtr.Zero) == 8) ? " (x64)" : " (x86)");



        }
        private void OnListBoxDemosDoubleClick(object sender, EventArgs e)
        {
            // OnLoadDemoClick(sender, e);
        }

        private void DisposeCurrentDemo()
        {
            if (panelDemo.Controls.Count <= 0) return;
            panelDemo.Controls[0].Dispose();
            panelDemo.Controls.Clear();
            GC.Collect();
        }

        private void buttonLoadDemo_Click(object sender, EventArgs e)
        {
            var plugin = (INAudioDemoPlugin)listBoxDemos.SelectedItem;
            if (plugin == currentPlugin) return;
            currentPlugin = plugin;
            DisposeCurrentDemo();
            var control = plugin.CreatePanel();
            control.Dock = DockStyle.Fill;
            panelDemo.Controls.Add(control);
        }
    }
}
