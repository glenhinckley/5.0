using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCSGlobal.BusinessRules.CoreLibraryII.Lists
{
   public  class ldap
    {

       private string _Client = string.Empty;
       private string _EndPoint = string.Empty;
       private string _Port = string.Empty;


        public string Client
        {
            get
            {
                return _Client;
            }
            set
            {
                _Client = value;
            }

        }

        public string EndPoint
        {
            get
            {
                return _EndPoint;
            }
            set
            {
                _EndPoint = value;
            }

        }

        public string Port
        {
            get
            {
                return _Port;
            }
            set
            {
                _Port = value;
            }

        }

    }
}
