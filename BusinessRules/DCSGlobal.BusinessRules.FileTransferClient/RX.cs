using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCSGlobal.BusinessRules.FileTransferClient
{
     class RX
    {


               ~RX()
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



       public int GetbyNamedPipes()
        {
            int r = -1;



            return r;
       
       }


       public int GetbyWebService()
       {

           int r = -1;



           return r;
       }

       public int GetbyTCP()
       {

           int r = -1;



           return r;
       }










    }
}
