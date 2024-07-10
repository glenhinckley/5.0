using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DCSGlobal.BusinessRules.FileTransferClient
{
    class b64
    {

        public byte[] DecodeBase64(string DecodeME)
        {
            byte[] rebin = Convert.FromBase64String(DecodeME);
            return rebin;
        }


        public string EncodeBase64(byte[] EncodeME)
        {
            string s = Convert.ToBase64String(EncodeME);
            return s;

        }



    }

}
