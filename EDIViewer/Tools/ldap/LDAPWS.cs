using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using System.Xml; //namespace to deal with XML documents 
using System.Xml.Linq; //namespace to deal with LINQ to XML classes
using System.Xml.XPath;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Specialized;
using System.Diagnostics;

using DCSGlobal.BusinessRules.CoreLibrary;
using DCSGlobal.BusinessRules.CoreLibraryII;
using DCSGlobal.BusinessRules.CoreLibraryII.Lists;
using DCSGlobal.BusinessRules.Security;

using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;

namespace Manual_test_app.Tools
{
    public partial class LDAPWS : Form
    {
        public LDAPWS()
        {
            InitializeComponent();
        }

        StringStuff ss = new StringStuff();
        XmlHelper xh = new XmlHelper();
        private string _Endpoint = "http://localhost:55834/LDAP.asmx";

        private void LDAPWS_Load(object sender, EventArgs e)
        {

            txtEndpoint.Text = _Endpoint;


        }



        private void cmdTest_Click(object sender, EventArgs e)
        {


            string LDAPReturn = string.Empty;
            string _strUserName = string.Empty;
            string _strPassword = string.Empty;
            string _ActiveDirectoryPath = string.Empty;
            string _ClientName = string.Empty;
            int _port = 0;
            string _EPort = string.Empty;




            using (DBUtility dbu = new DBUtility())
            {

                _strUserName = dbu.Encrypt(txtUserID_PlainText.Text);
                _strPassword = dbu.Encrypt(txtPasswd_PlainText.Text);
                _ClientName = dbu.Encrypt(txtClient_PlainText.Text);

            }

            try
            {

                _Endpoint = txtEndpoint.Text;

                using (LDAP.LDAPSoapClient au = new LDAP.LDAPSoapClient())
                {
                    au.Endpoint.Address = new System.ServiceModel.EndpointAddress(_Endpoint);
                    //AuthenticateAD(string strUserName, string strPassword, string ActiveDirectoryPath, string port)
                    LDAPReturn = au.AuthenticateAD(_strUserName, _strPassword, _ClientName);

                    txtResult.Text = ss.ParseDemlimtedString(LDAPReturn, "^", 1);
                    txtResultMsg.Text = ss.ParseDemlimtedString(LDAPReturn, "^", 2);

                }
            }
            catch (Exception ex)
            {

                txtResultMsg.Text = ex.Message;
            }


        }

        private void cmdDecrypt_Click(object sender, EventArgs e)
        {

            using (DBUtility dbu = new DBUtility())
            {
                txtUserID_PlainText.Text = dbu.Decrypt(txtUserID_Encrypted.Text);
                txtPasswd_PlainText.Text = dbu.Decrypt(txtPasswd_Encrypted.Text);
                txtServer_PlainText.Text = dbu.Decrypt(txtServer_Encrypted.Text);
                txtPort_PlainText.Text = dbu.Decrypt(txtPort_Encrypted.Text);
                txtClient_PlainText.Text = dbu.Decrypt(txtClient_Encrypted.Text);

            }


        }

        private void cmdEncypt_Click(object sender, EventArgs e)
        {
            string ClientXML = string.Empty;

            using (DBUtility dbu = new DBUtility())
            {
                txtUserID_Encrypted.Text = dbu.Encrypt(txtUserID_PlainText.Text);
                txtPasswd_Encrypted.Text = dbu.Encrypt(txtPasswd_PlainText.Text);
                txtClient_Encrypted.Text = dbu.Encrypt(txtClient_PlainText.Text);
                txtServer_Encrypted.Text = dbu.Encrypt(txtServer_PlainText.Text);
                txtPort_Encrypted.Text = dbu.Encrypt(txtPort_PlainText.Text);

            }




            ClientXML = "<client>";
            ClientXML = ClientXML + "\r\n<name>" + txtClient_PlainText.Text + "</name>";
            ClientXML = ClientXML + "\r\n<endpoint>" + txtServer_Encrypted.Text + "</endpoint>";
            ClientXML = ClientXML + "\r\n<port>" + txtPort_Encrypted.Text + "</port>";
            ClientXML = ClientXML + "\r\n</client>";


            txtClientXML.Text = ClientXML;

        }

        private void button1_Click(object sender, EventArgs e)
        {



            var clients = from nodes in System.Xml.Linq.XElement.Load("ep.xml").Elements("client") select nodes;

            if (clients != null)
            {
                foreach (var b in clients)
                {
                    listBox1.Items.Add(b.Element("name").Value.Trim());

                }
            }





            //    XElement root = XElement.Load("ep.xml");
            //    IEnumerable<XElement> address =
            //        from el in root.Elements("Address")
            //        where (string)el.Attribute("Type") == "Billing"
            //        select el;
            //    foreach (XElement el in address)
            //        Console.WriteLine(el); 




            //    XDocument cpo = XDocument.Load("ep.xml");
            //    XNamespace aw = "http://www.adventure-works.com";
            //    XElement newTree = new XElement("Root",
            //        from el in cpo.Root.Elements()
            //        where el.Name.Namespace == aw
            //        select el
            //    );
            //    Console.WriteLine(newTree);









            //    var custs = from c in XElement.Load("ep.xml").Elements("list")
            //                select c;

            //    // Execute the query
            //    foreach (var list in custs)
            //    {
            //        Console.WriteLine(list);
            //    } 

            //    //XmlSerializer serializer = new XmlSerializer(typeof(List<MyClass>));

            //    //using (FileStream stream = File.OpenWrite("filename"))
            //    //{
            //    //    List<MyClass> list = new List<MyClass>();
            //    //    serializer.Serialize(stream, list);
            //    //}

            //    //using (FileStream stream = File.OpenRead("filename"))
            //    //{
            //    //    List<MyClass> dezerializedList = (List<MyClass>)serializer.Deserialize(stream);
            //    //}
            //}

        }

        private void cmdDLL_Click(object sender, EventArgs e)
        {

            bool LoginPassFail = false;
            int ReturnCode = 0;
            string ReturnMessge = string.Empty;

            string UserName = string.Empty;
            string Passwd = string.Empty;
            string Client = string.Empty;

            UserName = txtUserID_PlainText.Text;
            Passwd = txtPasswd_PlainText.Text;
            Client = txtClient_PlainText.Text;
           
            
            using (LDAPWSLogin ld = new LDAPWSLogin())
            {
                ld.EndPoint = _Endpoint; // string you provide this the address of the ADWS you want to use
                LoginPassFail = ld.Login(UserName, Passwd, Client); // Cleint is case senistive // unencrypted the Class will do the encryption
                ReturnCode = ld.LDAPReturnCode;
                ReturnMessge = ld.LDAPReturnString;


            }

            txtResult.Text = Convert.ToString(ReturnCode);
            txtResultMsg.Text = ReturnMessge;


        }
    }
}
