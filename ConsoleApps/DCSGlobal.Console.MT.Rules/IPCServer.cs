using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Pipes;
using System.Diagnostics;
using System.IO;





namespace DCSGlobal.Console.MT.Rules
{
    public class StreamString
    {
        private Stream ioStream;
        private UnicodeEncoding streamEncoding;

        public StreamString(Stream ioStream)
        {
            this.ioStream = ioStream;
            streamEncoding = new UnicodeEncoding();
        }

        public string ReadString()
        {
            int len = 0;

            len = ioStream.ReadByte() * 256;
            len += ioStream.ReadByte();
            byte[] inBuffer = new byte[len];
            ioStream.Read(inBuffer, 0, len);

            return streamEncoding.GetString(inBuffer);
        }

        public int WriteString(string outString)
        {
            byte[] outBuffer = streamEncoding.GetBytes(outString);
            int len = outBuffer.Length;
            if (len > UInt16.MaxValue)
            {
                len = (int)UInt16.MaxValue;
            }
            ioStream.WriteByte((byte)(len / 256));
            ioStream.WriteByte((byte)(len & 255));
            ioStream.Write(outBuffer, 0, len);
            ioStream.Flush();

            return outBuffer.Length + 2;
        }
    }


    public class ReadFileToStream
    {
        private string fn;
        private StreamString ss;

        public ReadFileToStream(StreamString str, string filename)
        {
            fn = filename;
            ss = str;
        }

        public void Start()
        {
            string contents = File.ReadAllText(fn);
            ss.WriteString(contents);
        }

    }


    //    // Delegate for passing received message back to caller
    //    public delegate void DelegateMessage(string Reply);
    //    public class StreamString
    //    {
    //        private Stream ioStream;
    //        private UnicodeEncoding streamEncoding;

    //        public StreamString(Stream ioStream)
    //        {
    //            this.ioStream = ioStream;
    //            streamEncoding = new UnicodeEncoding();
    //        }

    //        public string ReadString()
    //        {
    //            int len = 0;

    //            len = ioStream.ReadByte() * 256;
    //            len += ioStream.ReadByte();
    //            byte[] inBuffer = new byte[len];
    //            ioStream.Read(inBuffer, 0, len);

    //            return streamEncoding.GetString(inBuffer);
    //        }

    //        public int WriteString(string outString)
    //        {
    //            byte[] outBuffer = streamEncoding.GetBytes(outString);
    //            int len = outBuffer.Length;
    //            if (len > UInt16.MaxValue)
    //            {
    //                len = (int)UInt16.MaxValue;
    //            }
    //            ioStream.WriteByte((byte)(len / 256));
    //            ioStream.WriteByte((byte)(len & 255));
    //            ioStream.Write(outBuffer, 0, len);
    //            ioStream.Flush();

    //            return outBuffer.Length + 2;
    //        }
    //    }

    //    // Contains the method executed in the context of the impersonated user 
    //    public class ReadFileToStream
    //    {
    //        private string fn;
    //        private StreamString ss;

    //        public ReadFileToStream(StreamString str, string filename)
    //        {
    //            fn = filename;
    //            ss = str;
    //        }

    //        public void Start()
    //        {
    //            string contents = File.ReadAllText(fn);
    //            ss.WriteString(contents);
    //        }
    //    }
     
    
    
    
    
    
    //class PipeServer
    //    {
    //        public event DelegateMessage PipeMessage;
    //        string _pipeName;

    //        public void Listen(string PipeName)
    //        {
    //            try
    //            {
    //                // Set to class level var so we can re-use in the async callback method
    //                _pipeName = PipeName;
    //                // Create the new async pipe 
    //                NamedPipeServerStream pipeServer = new NamedPipeServerStream(PipeName, PipeDirection.In, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);

    //                // Wait for a connection
    //                pipeServer.BeginWaitForConnection(new AsyncCallback(WaitForConnectionCallBack), pipeServer);
    //            }
    //            catch (Exception ex)
    //            {
    //                Debug.WriteLine(ex.Message);
    //            }
    //        }
         
    //  class IPCServer
    //        {
    //        private void WaitForConnectionCallBack(IAsyncResult iar)
    //        {
    //            try
    //            {
    //                // Get the pipe
    //                NamedPipeServerStream pipeServer = (NamedPipeServerStream)iar.AsyncState;
    //                // End waiting for the connection
    //                pipeServer.EndWaitForConnection(iar);

    //                byte[] buffer = new byte[255];

    //                // Read the incoming message
    //                pipeServer.Read(buffer, 0, 255);

    //                // Convert byte buffer to string
    //                string stringData = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
    //                Debug.WriteLine(stringData + Environment.NewLine);

    //                // Pass message back to calling form
    //                PipeMessage.Invoke(stringData);

    //                // Kill original sever and create new wait server
    //                pipeServer.Close();
    //                pipeServer = null;
    //                pipeServer = new NamedPipeServerStream(_pipeName, PipeDirection.In, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);

    //                // Recursively wait for the connection again and again....
    //                pipeServer.BeginWaitForConnection(new AsyncCallback(WaitForConnectionCallBack), pipeServer);
    //            }
    //            catch
    //            {
    //                return;
    //            }
    //        }
    //    }



    }

