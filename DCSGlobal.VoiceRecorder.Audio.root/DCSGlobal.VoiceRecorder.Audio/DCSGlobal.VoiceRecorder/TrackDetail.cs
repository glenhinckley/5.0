using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCSGlobal.VoiceRecorder
{
    class TrackDetail
    {
    }


    public struct MyData
    {
        public int id { set; get; }
        public string AccountNumber { set; get; }
        public string MRN { set; get; }
        public string DOR { get; set; }
        public string Subject { set; get; }
        public string Detail { get; set; }
        public int Start { set; get; }
        public int Stop { get; set; }


    }
}
