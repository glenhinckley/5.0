using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace DCSGlobal.BusinessRules.FileTransferClient
{
     class TX : IDisposable
    {



        ~TX()
        {
            Dispose(false);
        }



        bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // free other managed objects that implement
                // IDisposable only
            }

            // release any unmanaged objects
            // set the object references to null

            _disposed = true;
        }


        private static string _Source = "DCSGLOBAL";
        private static string _Log = "LOG";


        private Byte[] _FileData = new Byte[0];
        private string _ClientName = string.Empty;
        private string _HospCode = string.Empty;
        private int _Encrypt = 0;
        private string _FilePath = string.Empty;
        private string _FileName = string.Empty;
        private int _Protocol = 0;
        private int _SizeOnDisc = 0;

        private string _Sever = string.Empty;

        public string Server
        {
            get
            {
                return _Sever;
            }
            set
            {
                _Sever = value;
            }
        }





        public byte[] FileData
        {
            get
            {
                return _FileData;
            }
            set
            {

                try
                {
                    Array.Resize(ref _FileData, value.Length);
                    _FileData = value;
                }
                catch (Exception ex)
                {

                }
            }
        }

        public string ClientName
        {
            get
            {
                return _ClientName;
            }
            set
            {
                _ClientName = value;
            }
        }

        public string HospCode
        {
            get
            {
                return _HospCode;
            }
            set
            {
            }
        }

        public int Encrypt
        {
            get
            {
                return _Encrypt;
            }
            set
            {
                _Encrypt = value;
            }
        }


        public int SizeOnDisc
        {
            get
            {
                return _SizeOnDisc;
            }
            set
            {
                _SizeOnDisc = value;
            }
        }


        public string FilePath
        {
            get
            {
                return _FilePath;
            }
            set
            {
                _FilePath = value;
            }
        }

        public string FileName
        {
            get
            {
                return _FileName;
            }
            set
            {
                _FileName = value;
            }
        }


        public int Protocol
        {
            get
            {
                return _Protocol;
            }
            set
            {
                _Protocol = value;
            }
        }



        //private void button1_Click(object sender, RoutedEventArgs e)
        //{
        //    using (NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "testpipe",
        //                                                                      PipeDirection.Out,
        //                                                               PipeOptions.Asynchronous))
        //    {
        //        tbStatus.Text = "Attempting to connect to pipe...";
        //        try
        //        {
        //            pipeClient.Connect(2000);
        //        }
        //        catch
        //        {
        //            MessageBox.Show("The Pipe server must be started in order to send data to it.");
        //            return;
        //        }
        //        tbStatus.Text = "Connected to pipe.";
        //        using (StreamWriter sw = new StreamWriter(pipeClient))
        //        {
        //            sw.WriteLine(tbClientText.Text);
        //        }
        //    }
        //    tbStatus.Text = "";
        //}



        //public class Pipeserver
        //{
        //    public static Window1 owner;
        //    public static Invoker ownerInvoker;
        //    public static string pipeName;
        //    private static NamedPipeServerStream pipeServer;
        //    private static readonly int BufferSize = 256;

        //    private static void SetTextbox(String text)
        //    {
        //        owner.tbox.Text = String.Concat(owner.tbox.Text, text);
        //        if (owner.tbox.ExtentHeight > owner.tbox.ViewportHeight)
        //        {
        //            owner.tbox.ScrollToEnd();
        //        }
        //    }

        //    public static void createPipeServer()
        //    {
        //        Decoder decoder = Encoding.Default.GetDecoder();
        //        Byte[] bytes = new Byte[BufferSize];
        //        char[] chars = new char[BufferSize];
        //        int numBytes = 0;
        //        StringBuilder msg = new StringBuilder();
        //        ownerInvoker.sDel = SetTextbox;

        //        try
        //        {
        //            pipeServer = new NamedPipeServerStream(pipeName, PipeDirection.In, 1,
        //                                                   PipeTransmissionMode.Message,
        //                                                   PipeOptions.Asynchronous);
        //            while (true)
        //            {
        //                pipeServer.WaitForConnection();

        //                do
        //                {
        //                    msg.Length = 0;
        //                    do
        //                    {
        //                        numBytes = pipeServer.Read(bytes, 0, BufferSize);
        //                        if (numBytes > 0)
        //                        {
        //                            int numChars = decoder.GetCharCount(bytes, 0, numBytes);
        //                            decoder.GetChars(bytes, 0, numBytes, chars, 0, false);
        //                            msg.Append(chars, 0, numChars);
        //                        }
        //                    } while (numBytes > 0 && !pipeServer.IsMessageComplete);
        //                    decoder.Reset();
        //                    if (numBytes > 0)
        //                    {
        //                        ownerInvoker.Invoke(msg.ToString());
        //                    }
        //                } while (numBytes != 0);
        //                pipeServer.Disconnect();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //        }
        //    }
        //}


        public int SendbyNamedPipes()
        {
            int r = 0;

            return r;

        }

        public int SendbyWebService()
        {
            int r = 0;

            return r;

        }


        public int SendbyTCP()
        {
            int r = 0;

            return r;

        }




        public int SaveDocument(Byte[] DocBinaryArray, string DocName)
        {
            string strdocPath = string.Empty;
            int r = -1;
            FileStream objfilestream = new FileStream(strdocPath, FileMode.Create, FileAccess.ReadWrite);
            objfilestream.Write(DocBinaryArray, 0, DocBinaryArray.Length);
            objfilestream.Close();

            return r;
        }






        public int GetDocumentLen(string strdocPath)
        {
            // string strdocPath;
            //   strdocPath = "C:\\usr\\" + DocumentName;

            FileStream objfilestream = new FileStream(strdocPath, FileMode.Open, FileAccess.Read);
            int len = (int)objfilestream.Length;
            objfilestream.Close();

            return len;
        }



        public Byte[] GetDocument(string strdocPath)
        {
            //    string strdocPath;
            // strdocPath = "C:\\usr\\" + DocumentName;

            FileStream objfilestream = new FileStream(strdocPath, FileMode.Open, FileAccess.Read);
            int len = (int)objfilestream.Length;
            Byte[] documentcontents = new Byte[len];
            objfilestream.Read(documentcontents, 0, len);
            objfilestream.Close();

            return documentcontents;
        }
    }
}
