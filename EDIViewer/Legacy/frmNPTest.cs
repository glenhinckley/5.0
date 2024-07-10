using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Pipes;
using System.Security.Principal;
using System.IO;
using System.Threading;
using DCSGlobal.Eligibility.ProcessEligibility;

namespace Manual_test_app
{
    public partial class frmNPTest : Form
    {
        public frmNPTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string s = string.Empty;
           // string ss = string.Empty;
            
            string sss = string.Empty;
            

                    //Dim s As String = ""
       // Console.WriteLine("Taskid:" + _TaskID + "  592")
        NamedPipeClientStream pipeClient = new  NamedPipeClientStream(".", txtAppName.Text , PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.Impersonation);
       // Console.WriteLine("Taskid:" + _TaskID + "  594")
        pipeClient.Connect();
      //  Dim sss As String = ""
       
          StreamString ss = new StreamString(pipeClient);   
            
        

       // Console.WriteLine("Taskid:" + _TaskID + "  599")

        sss = ss.ReadString();
       // Console.WriteLine("Taskid:" + _TaskID + "  602")
        if (sss != "EOF") 
        {
            s = sss;
            //Dim i As String
            //i = 1;
        }
        else
        {
            s = "EOF";
        }
           // '  Console.WriteLine("Server could not be verified.")
      
        pipeClient.Close();
     //   Console.WriteLine("Taskid:" + _TaskID + " 613")

      //  ' Give the client process some time to display results before exiting.
         Thread.Sleep(4000);

         txtNP.Text = s;
       

        }
    }
}
