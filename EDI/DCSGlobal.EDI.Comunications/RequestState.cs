using System;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;

namespace DCSGlobal.EDI.Comunications
{
   public class RequestState
    {

        // This class stores the request state of the request.
        public WebRequest request;
        public RequestState()
        {
            request = null;
        }
    }
}
