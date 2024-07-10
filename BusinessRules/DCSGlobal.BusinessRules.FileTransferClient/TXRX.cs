
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Pipes;


using System.Security.Principal;

using System.Runtime.Serialization;
using DCSGlobal.BusinessRules.CoreLibraryII;

namespace DCSGlobal.BusinessRules.FileTransferClient
{
    public class TXRX : IDisposable
    {


        ~TXRX()
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


        private static string _Source = "DCSGlobal.BusinessRules.FileTransferClient";
        private static string _Log = "LOG";


        private Byte[] _FileData = new Byte[0];
        private Int32 _FileSize = 0;
        private string _ClientName = string.Empty;
        private string _HospCode = string.Empty;
        private int _Encrypt = 0;
        private string _FQFN = string.Empty;
        private string _FilePath = string.Empty;

        private string _FileName = string.Empty;
        private int _Protocol = 0;
        private int _SizeOnDisc = 0;

        private string _Sever = ".";
        private string _Instance = string.Empty;
        private string _ERR = string.Empty;


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


        public string Instance
        {
            get
            {
                return _Instance;
            }
            set
            {
                _Instance = value;
            }
        }


        public string ERR
        {
            get
            {
                return _ERR;
            }
            set
            {
                _ERR = value;
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
                    _FileSize = value.Length; 
                    
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



        public int FileSize
        {
            get
            {
                return _FileSize ;
            }
            set
            {
                _FileSize = value;
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

        public string FQFN
        {
            get
            {
                return _FQFN;
            }
            set
            {
                _FQFN = value;
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




        public int Put()
        {
            int r = -1;
            string _TX = string.Empty;



            _TX = Convert.ToBase64String(_FileData);


            string sss = string.Empty;

            try
            {
                NamedPipeClientStream pipeClient = new NamedPipeClientStream(_Sever, _Instance, PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.Impersonation);
                pipeClient.Connect();

                StreamString ss = new StreamString(pipeClient);

                ss.WriteString("GLEN");

                sss = ss.ReadString();

                if (sss == "ACK")
                {
                    ss.WriteString("PUT");
                }

                sss = ss.ReadString();


                if (sss == "CTS")
                {

                    ss.WriteString(Convert.ToString(_FileSize)); 
                     _SizeOnDisc = Convert.ToInt32(ss.ReadString());


                     if (_FileSize == _SizeOnDisc)
                     {
                         ss.WriteString("OK");

                         //change to actaul path
                         ss.WriteString("c:\\usr\\mmmmmmmm.txt");

                         sss = ss.ReadString();
                         if (sss == "SEND")
                         {

                             int idx = 0;
                             int _BytesSent = 0;
                             string _TXBuffer = string.Empty;



                             while (_BytesSent > _FileSize) 
                              {


                                  //// Continue to next iteration
                                  //for (i = 0; i <= 4; i++)
                                  //{
                                  //    if (i < 4)
                                  //        continue;
                                  //    Console.WriteLine(i);   // Only prints 4
                                  //} 


                                      ss.WriteString(_TX);
                              }
                             
                    //            While (InlineAssignHelper(read, r.ReadBlock(buffer, 0, buffer.Length))) > 0

                    //    For i As Integer = 0 To read - 1
                    //        If Convert.ToString(buffer(i)) = "~" Then
                    //            If Convert.ToString(buffer(i)) <> vbCr AndAlso Convert.ToString(buffer(i)) <> vbLf Then
                    //                line = line & Convert.ToString(buffer(i))
                    //            End If

                    //            If Not line = String.Empty Then

                    //                line = ss.StripCRLF(line)
                    //                l.Add(line)
                    //                line = String.Empty

                    //            End If

                    //        Else
                    //            line = line & Convert.ToString(buffer(i))
                    //        End If
                    //    Next

                    //End While


                             
                             
                             
                         

                         }

                     }
                     else
                     {

                     }

                    ss.WriteString(_FQFN);


                   

                 


                    sss = ss.ReadString();
                    if (sss == "GOODBYE")
                    {
                        r = 0;
                    }
                    else
                    {
                        r = -1;
                    }
                }

                pipeClient.Close();
            }

            catch (Exception ex)
            {
                _ERR = ex.Message.ToString();

            }






            return r;





        }

        public int Get()
        {
            int r = -1;



            return r;



        }
        public int GetToFile()
        {
            int r = -1;



            return r;



        }


        public int PutFromFile()
        {
            int r = -1;



            return r;



        }






        public int Touch()
        {
            int r = -1;

            string sss = string.Empty;

            try
            {
                NamedPipeClientStream pipeClient = new NamedPipeClientStream(_Sever, _Instance, PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.Impersonation);
                pipeClient.Connect();

                StreamString ss = new StreamString(pipeClient);

                ss.WriteString("GLEN");

                sss = ss.ReadString();

                if (sss == "ACK")
                {
                    ss.WriteString("TOUCH");
                }

                sss = ss.ReadString();


                if (sss == "CTR")
                {
                    ss.WriteString(_FQFN);
                

                _SizeOnDisc = Convert.ToInt32(ss.ReadString());

                ss.WriteString("OK");


                sss = ss.ReadString();
                if (sss == "GOODBYE")
                {
                    r = 0;
                } 
                else
                {
                    r = -1;
                }
                }

                pipeClient.Close();
            }

            catch (Exception ex)
            {
                _ERR = ex.Message.ToString();

            }






            return r;



        }





        //static void Main(string[] args)
        //{
        //    //connecting to the known pipe stream server which runs in localhost
        //    using (NamedPipeClientStream pipeClient =
        //        new NamedPipeClientStream(".", "File Transfer", PipeDirection.In))
        //    {


        //        // Connect to the pipe or wait until the pipe is available.
        //        Console.Write("Attempting to connect to File Transfer pipe...");
        //        //time out can also be specified
        //        pipeClient.Connect();

        //        Console.WriteLine("Connected to File Transfer pipe.");

        //        IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        //        //deserializing the pipe stream recieved from server
        //        DCSGlobal.FileServer.TransferFile objTransferFile = (DCSGlobal.FileServer.TransferFile)formatter.Deserialize(pipeClient);



        //        //creating the target file with name same as specified in source which comes using
        //        //file transfer object
        //        byte[] byteBuffer = new byte[1024];

        //        using (FileStream fs = new FileStream(@"c:\Test\2\" + objTransferFile.FileName, FileMode.Create, FileAccess.Write))
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
        //        Console.WriteLine("File, Received from server: {0}", objTransferFile.FileName);
        //    }
        //    Console.Write("Press Enter to continue...");
        //    Console.ReadLine();
        //}
    }
}