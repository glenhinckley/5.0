using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using System.Threading.Tasks;
using System.Threading;
using System.Timers;


//using DCSGlobal.API.ScriptingEngine;

namespace Manual_test_app
{
    public partial class frmScripting : Form
    {



        private static System.Timers.Timer _Recycletimer; // From System.Timers

        private static bool _ForceEOF = false;


        public frmScripting()
        {
            InitializeComponent();
        }


        // public VBRules myScript = new VBRules();


        private void cmdRunScript_Click(object sender, EventArgs e)
        {


            //this.lstOutput.Items.Clear();
            //this.myScript.setScript(txtScript.Text);
            //Hashtable hashtables = this.myScript.runScript();
            //foreach (string key in hashtables.Keys)                                                      
            //{
            //    this.lstOutput.Items.Add(string.Concat(key, " = ", hashtables[key]));
            //}

        }

        private void cmdClear_Click(object sender, EventArgs e)
        {

            //myScript.clearScriptInputs();
            //this.lstKeyPair.Items.Clear();
        }

        private void cmdKV_Click(object sender, EventArgs e)
        {
            this.addNewInput(txtKey.Text, txtValue.Text);
            this.txtKey.Text = "";
            this.txtValue.Text = "";
        }

        private void frmScripting_Load(object sender, EventArgs e)
        {

        }


        private void addNewInput(string varName, string varVal)
        {
            //this.myScript.addScriptInput(varName, varVal);
            //Hashtable inputs = this.myScript.getInputs();
            //this.lstKeyPair.Items.Clear();
            //foreach (object key in inputs.Keys)
            //{
            //    this.lstKeyPair.Items.Add(string.Concat(key, " = ", inputs[key]));
            //}
        }

        private void lstKeyPair_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lstKeyPair_DoubleClick(object sender, EventArgs e)
        {
            if (this.lstKeyPair.SelectedIndex >= 0)
            {
                this.lstKeyPair.Items.RemoveAt(this.lstKeyPair.SelectedIndex);
            }

        }


        static void _RecycleTimerElaspesed(object sender, ElapsedEventArgs e)
        {

            _ForceEOF = true;



            //   GC.Collect();
        }

        private void cmdRunTime_Click(object sender, EventArgs e)
        {




            //_Recycletimer = new System.Timers.Timer(60000);
            //_Recycletimer.Elapsed += new ElapsedEventHandler(_RecycleTimerElaspesed);
            //_Recycletimer.Enabled = true; // Enable it

            //int count = 0;



            //while (!_ForceEOF)
            //{
            //    this.myScript.setScript(txtScript.Text);
            //    Hashtable hashtables = this.myScript.runScript();
            //    foreach (string key in hashtables.Keys)
            //    {
            //        this.lstOutput.Items.Add(string.Concat(key, " = ", hashtables[key]));
            //    }

            //    //using (VBRules myLScript = new VBRules())
            //    //{
            //    //    this.lstOutput.Items.Clear();

            //    //    myLScript.setScript(txtScript.Text);
            //    //    Hashtable hashtables = myLScript.runScript();
            //    //    foreach (string key in hashtables.Keys)
            //    //    {
            //    //        this.lstOutput.Items.Add(string.Concat(key, " = ", hashtables[key]));
            //    //    }
            //    count++;
            //    // }

            //}

            //_Recycletimer.Enabled = false;
            //lblCount.Text = Convert.ToString(count);



        }




    }
}
