using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using System.IO;
using System.IO.Pipes;
using System.Runtime.Serialization;
using DCSGlobal.BusinessRules.FileTransferClient;


namespace Manual_test_app
{



    public partial class frmBYTEFileTransfer : Form
    {

        private string sFile = string.Empty;


        public frmBYTEFileTransfer()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //if (checkBox1.Checked == false)
            //{
            //    MemoryStream objstreaminput = new MemoryStream();
            //    FileStream objfilestream = new FileStream(txtFileSave.Text, FileMode.Create, FileAccess.ReadWrite);

            //    localhost.FileIO myservice = new localhost.FileIO();
            //    int len = (int)myservice.GetDocumentLen(txtDNget.Text, txtDTGet.Text, txtDDGet.Text, txtHCget.Text);
            //    Byte[] mybytearray = new Byte[len];
            //    mybytearray = myservice.GetDocument(txtDNget.Text, txtDTGet.Text, txtDDGet.Text, txtHCget.Text);
            //    objfilestream.Write(mybytearray, 0, len);
            //    objfilestream.Close();
            //}
            //else
            //{


            //    //connecting to the known pipe stream server which runs in localhost
            //    using (NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "File Transfer", PipeDirection.In))
            //    {


            //        // Connect to the pipe or wait until the pipe is available.
            //        label6.Text = "Attempting to connect to File Transfer pipe...";
            //        //time out can also be specified
            //        pipeClient.Connect();

            //        label12.Text = "Connected to File Transfer pipe.";


            //        //IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            //        //deserializing the pipe stream recieved from server


            //        //tx 
            //        string myString;
            //        using (FileStream fs = new FileStream("C:\\usr\\PWThingy.rar", FileMode.Open))
            //        using (BinaryReader br = new BinaryReader(fs))
            //        {
            //            byte[] bin = br.ReadBytes(Convert.ToInt32(fs.Length));
            //            myString = Convert.ToBase64String(bin);
            //        }

            //        //rx
            //        byte[] rebin = Convert.FromBase64String(myString);
            //        using (FileStream fs2 = new FileStream("C:\\usr\\PWThingy2.rar", FileMode.Create))
            //        using (BinaryWriter bw = new BinaryWriter(fs2))
            //        {
            //            bw.Write(rebin);
            //        }



            //        //TransferFile objTransferFile = (TransferFile)formatter.Deserialize(pipeClient);

            //        //creating the target file with name same as specified in source which comes using
            //        //file transfer object
            //        byte[] byteBuffer = new byte[1024];

            //        using (FileStream fs = new FileStream(@"c:\usr\test\" + objTransferFile.FileName, FileMode.Create, FileAccess.Write))
            //        {
            //            //writing each Kbyte to the target file
            //            int numBytes = objTransferFile.FileContent.Read(byteBuffer, 0, 1024);
            //            fs.Write(byteBuffer, 0, 1024);
            //            while (numBytes > 0)
            //            {
            //                numBytes = objTransferFile.FileContent.Read(byteBuffer, 0, numBytes);
            //                fs.Write(byteBuffer, 0, numBytes);
            //            }
            //        }
            //        label13.Text = "File, Received from server: " + objTransferFile.FileName;
            //    }
            //    // Console.Write("Press Enter to continue...");
            //    // Console.ReadLine();
            //}



        }





        private void cmdTouchFile_Click(object sender, EventArgs e)
        {
            //int i = 0;

            //localhost.FileIO myservice = new localhost.FileIO();
            //i = (int)myservice.GetDocumentLen(txtDNget.Text, txtDTGet.Text, txtDDGet.Text, txtHCget.Text);

            //lblTouch.Text = Convert.ToString(i);


            using (TXRX cq = new TXRX())
            {
                int i = -1;


                cq.Server = txtNPServer.Text;
                cq.Instance = txtInstanceName.Text;

                cq.FQFN = txtFileSave.Text;


                i = cq.Touch();

                if (i == 0)
                {
                    lblTS.Text = Convert.ToString(cq.SizeOnDisc);
                }
                
                lblERR.Text = cq.ERR;

            }



        }



        private void button2_Click(object sender, EventArgs e)
        {







            //if (checkBox1.Checked)
            //{
            //    int i = 0;

                FileStream objfilestream = new FileStream(txtFileToSend.Text, FileMode.Open, FileAccess.Read);
                int len = (int)objfilestream.Length;
                Byte[] mybytearray = new Byte[len];
                objfilestream.Read(mybytearray, 0, len);


                using (TXRX cq = new TXRX())
                {
                    int i = -1;


                    cq.Server = txtNPServer.Text;
                    cq.Instance = txtInstanceName.Text;
                    cq.FileSize = len;
                    cq.FileName = txtFileToSend.Text;
                    
                    cq.FileData = mybytearray;
                    cq.FQFN = txtFileSave.Text;


                    i = cq.Put();

                    if (i == 0)
                    {
                        lblTS.Text = Convert.ToString(cq.SizeOnDisc);
                    }

                    lblERR.Text = cq.ERR;

                }
            
            
            
            
            //    //   DCSGlobal.FileIO myservice = new DCSGlobal.FileIO();

            //    localhost.FileIO myservice = new localhost.FileIO();

            //    //myservice.Url = "http://localhost:21586/fileio.asmx";


            //    i = myservice.SaveDocument(mybytearray, txtDNsend.Text, txtDTSend.Text, txtDDSend.Text, txtHSend.Text);

            //    // myservice.SaveDocument(mybytearray, sFile.Remove(0, sFile.LastIndexOf("\\") + 1));
            //    objfilestream.Close();

            //    lblReusltSebd.Text = Convert.ToString(i);
            //}

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void frmBYTEFileTransfer_Load(object sender, EventArgs e)
        {
            txtInstanceName.Text = "DCSGlobal.FileServer";
            txtNPServer.Text = ".";
        }


    }


}
