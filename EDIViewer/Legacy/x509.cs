using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;
//using Microsoft.Web.Services3.Design;
//using Microsoft.Web.Services3;

namespace Manual_test_app
{
    public partial class x509 : Form
    {
        public x509()
        {
            InitializeComponent();
        }

        private void x509_Load(object sender, EventArgs e)
        {
                 //Microsoft.Web.Services3.Soap12 s = new Soap12();

                

        }






  


         


        public class SecurityHeader : System.ServiceModel.Channels.MessageHeader
        {
            public string userName;
            public string password;

            protected override void OnWriteStartHeader(System.Xml.XmlDictionaryWriter writer, System.ServiceModel.Channels.MessageVersion messageVersion)
            {
                writer.WriteStartElement("wsse", Name, Namespace);
                writer.WriteXmlnsAttribute("wsse", Namespace);
            
            
            
            }

            protected override void OnWriteHeaderContents(System.Xml.XmlDictionaryWriter writer, System.ServiceModel.Channels.MessageVersion messageVersion)
            {
                writer.WriteStartElement("wsse", "UsernameToken", Namespace);

                writer.WriteStartElement("wsse", "Username", Namespace);
                writer.WriteValue(userName);
                writer.WriteEndElement();

                writer.WriteStartElement("wsse", "Password", Namespace);
                writer.WriteValue(password);
                writer.WriteEndElement();

                writer.WriteEndElement();

            }

            public override string Name
            {
                get { return "Security"; }
            }

            public override string Namespace
            {
                get { return "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"; }
            }
        }



    }
}
