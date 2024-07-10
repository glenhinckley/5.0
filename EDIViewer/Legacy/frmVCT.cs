using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DCSGlobal.EDI.Comunications;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates; 


namespace Manual_test_app
{
    public partial class frmVCT : Form
    {


        DCSGlobal.EDI.Comunications.
        EDIGetResponse ec = new EDIGetResponse();


        string TestType = string.Empty;
        string _CurrentVendor = string.Empty;
        int _isTest = 1;

        public frmVCT()
        {
            InitializeComponent();
        }

        private void cmdGo_Click(object sender, EventArgs e)
        {
             

            int i = 1;
            txtRES.Text = string.Empty;
            if (radioButton1.Checked == true)
            {
                TestType = "test";
            }
            else
            {
                TestType = "prod";
            }
            //_CurrentVendor = "TXMCD";
            
            
            _CurrentVendor =  comboBox1.SelectedItem.ToString();
            //ec.ParameterFilePath = @"C:\Clients\iPAS40\Bin\parameters.xml";
            //ec.ParameterFilePath = @"C:\ConsoleTest\parameters1.xml";
            //ec.ConnectionString = "Persist Security Info=False;User ID=suresh;Password=suresh247;Initial Catalog=al60_seton_lite_developer;Data Source=10.1.1.120;pooling=false;connection Timeout=180";
            //ec.ConnectionString = "Persist Security Info=False;User ID=al60_seton_lite_developer_user;Password=AYImyvmKCd14tMw9timGAoTVdCmIKdP6yDSSKymY7t0um+lShaMStpGRtDcLlS7O;Initial Catalog=al60_seton_lite_developer;Data Source=10.1.1.120;pooling=false;connection Timeout=180";
            //working
            //txtBatchID.Text = "8703915";
            ec.BatchID =   Convert.ToInt32(txtBatchID.Text);

            ec.EBRID =   txtEBRId.Text;
            ec.VendorRetrys = 3;
            ec.Verbose = false;
          
            //  <add key="ConnStr" value="Persist Security Info=False;User ID=console_prospect_user;Password=oN6HzQZBBy5s21wY9U50b/1N19YR+WplF7ba4XW64Lg=;Initial Catalog=al60_prospect_prod;Data Source=10.34.1.30;pooling=false;connection Timeout=180"/>	 
            //ec.ConnectionString = "Persist Security Info=False;User ID=console_prospect_user;Password=console_prospect_user_password;Initial Catalog=al60_prospect_prod;Data Source=10.1.1.120;pooling=false;connection Timeout=180";
            ec.ConnectionString = "Persist Security Info=False;User ID=al60_seton_lite_developer_user;Password=al60_seton_lite_developer_password;Initial Catalog=al60_seton_lite_developer;Data Source=10.1.1.120;pooling=false;connection Timeout=180";
            
            // WORKING- developer connection string
            //ec.ConnectionString = "Persist Security Info=False;User ID=al60_bmc_prod_user;Password=al60_bmc_prod_password;Initial Catalog=al60_bmc_prod_NEW;Data Source=10.34.1.240;pooling=false;connection Timeout=180";
            //ec.ConnectionString = "Persist Security Info=False;User ID=al60_seton_lite_developer_user;Password=al60_seton_lite_developer_password;Initial Catalog=al60_eastmaine_prod;Data Source=10.1.1.120;pooling=false;connection Timeout=180";
            //ec.ConnectionString = "Persist Security Info=False;User ID=al60_demo;Password=al60_demo_password;Initial Catalog=al60_demo;Data Source=10.1.1.120;pooling=false;connection Timeout=180";
              

                 
            
            //ec.ConnectionString = "Persist Security Info=False;User ID=al60_bonsecours_test_user;Password=al60_bonsecours_test_password;Initial Catalog=al60_bonsecours_test_epic;Data Source=10.34.1.60;pooling=false;connection Timeout=180";
            
            //ec.ConnectionString = "Persist Security Info=False;User ID=console_eastmaine_prod_user;Password=console_eastmaine_prod_password;Initial Catalog=al60_eastmaine_prod;Data Source=10.34.1.190;pooling=false;connection Timeout=180";
            
            
            //ec.ConnectionString = "Persist Security Info=False;User ID=console_bothwell_user;Password=console_bothwell_password;Initial Catalog=al60_bothwell;Data Source=10.34.1.150;pooling=false;connection Timeout=180";

            //ec.ConnectionString = "Persist Security Info=False;User ID=sgudimalla;Password=jTztHqMCXQZ3Q0P9AhSvlg==;Initial Catalog=al60_eastmaine_prod;Data Source=DCSSQL2012;pooling=false;connection Timeout=180";
           

                 ec.XMLParameters = txtXML.Text;
            //ec.ConnectionString = "Persist Security Info=False;User ID=console_newmexico_user;Password=console_newmexico_password;Initial Catalog=al60_eyenm_prod;Data Source=10.34.1.100;pooling=false;connection Timeout=180";
            //THIS IS NEBRASKA REQUEST.
            //txtReq.Text = "ISA*00*          *00*          *ZZ*DCSGLOBALEDIEXG*ZZ*MMISNEBR       *150210*1030*^*00501*590740182*0*T*>~GS*HS*DCSGLOBALEDIEXG*MMISNEBR*20150210*103000*932221455*X*005010X279A1~ST*270*589486*005010X279A1~BHT*0022*13*TRANSACTIONID123*20150514*1251~HL*1**20*1~NM1*PR*2*NEBRASKA MEDICAID*****PI*NEMEDICAID~HL*2*1*21*1~NM1*1P*2*BRYAN HOSPITAL*****XX*1528006103~HL*3*2*22*0~NM1*IL*1*GOETZ*TIFFANY****MI*50621206101~DMG*D8*19820112~DTP*291*D8*20150514~EQ*30~SE*12*589486~GE*1*589486~IEA*1*100589486~";

            //THIS IS EMDEON
            //txtReq.Text = "ISA*00*          *01*PMNADCSGL *ZZ*TPG00554809    *ZZ*EMDEON         *161214*0814*^*00501*034041300*0*P*:~GS*HS*LLX1210001*MTEXE*20161214*0814*372*X*005010X279A1~ST*270*99999*005010X279A1~BHT*0022*13*ECB7AB2E9F5A4AA*20161214*0814~HL*1**20*1~NM1*PR*2*Anthem Blue Cross of California*****PI*BCCAC~HL*2*1*21*1~NM1*1P*2*Baptist Medical Center*****XX*1578620449~HL*3*2*22*0~TRN*1*ECB7AB2E9F5A4AA*9954719251~NM1*IL*1*ZTEST*JOSEPH****MI*~DMG*D8*19830211~DTP*291*D8*20161214~SE*11*99999~GE*1*372~IEA*1*034041300~";
             ec.subscriberfirstname = "DONNELL";
            ec.subscriberlastname = "WILLIAMS";
            ec.subscriberdob = "19670702";
            ec.dateofservice = "20170630";
            string inputtype = "FlatXml";
            ec.PatientHospitalCode = "X43-MRM";
            ec.PatAcctNum = "X43";
            ec.HospCode = "MRM";
            //ec.ConnectionString = "Persist Security Info=False;User ID=console_bayhealth_prod_user;Password=console_bayhealth_prod_password;Initial Catalog=al60_bayhealth_prod;Data Source=10.34.1.100;pooling=false;connection Timeout=180";
            //ec.BatchID = 2255051;
            //_CurrentVendor = "DORADO";
            i = ec.getEligVendorEdiResponse(txtReq.Text, "00609", _CurrentVendor, "flatXML");
          
 
            if (i == 0)
            {
                txtRES.Text = ec.RES;
            }
            else
            {
                txtRES.Text = "Error";
            }

       





            //switch (_CurrentVendor)
            //{                          // Must be integer or string

            //    case "NEMEDICAID":

            //        _NEMEDICAID();

            //        break;



            //    case "IVANS":

            //        _IVANS();

            //        break;



            //    case "DORADO":
            //        _DORADO();

            //        break;


            //    case "MEDDATA":

            //        ec.getEligVendorEdiResponse(txtReq.Text,"00193","MEDDATA","");

            //        break;



            //    case "EMDEON":

            //        _EMDEON();

            //        break;




            //    case "PNT":

            //        _PNT();

            //        break;







            //    case "TMHP":

            //        _TMHP();

            //        break;


            //    default:

            //        break;       // break necessary on default
            //}





        }

        private void VendorCommunitcationsTestForm_Load(object sender, EventArgs e)
        {


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {



            _CurrentVendor = comboBox1.Text;









        }



        private void _DORADO()
        {

            ec.getEligVendorEdiResponse(txtReq.Text, "", "DORADO", "");

        }



        private void _IVANS()
        {



            ec.getEligVendorEdiResponse(txtReq.Text, "", "IVANS", "");
            
            //   TexasMedicaidHealthcarePartnership tx = new TexasMedicaidHealthcarePartnership();


            //   txtRES.Text = tx.REQ("adasd");



        }

       //   DoradoSystems



        private void _EMDEON()
        {

            ec.getEligVendorEdiResponse(txtReq.Text, "", "EMDEON", "");
        
        
        }



        private void _PNT()
        {



            ec.getEligVendorEdiResponse(txtReq.Text, "", "POST-N-TRACK", "");

            //   TexasMedicaidHealthcarePartnership tx = new TexasMedicaidHealthcarePartnership();


            //   txtRES.Text = tx.REQ("adasd");



        }






        private void _MEDDATA()
        {



            ec.getEligVendorEdiResponse(txtReq.Text, "", "MEDDATA", "");
            
            //   TexasMedicaidHealthcarePartnership tx = new TexasMedicaidHealthcarePartnership();


            //   txtRES.Text = tx.REQ("adasd");



        }


        private void _TMHP()
        {

         
           //TexasMedicaidHealthcarePartnership tx = new TexasMedicaidHealthcarePartnership();


         //   txtRES.Text = tx.REQ("adasd");



        }

        private void _NEMEDICAID()
        {


     //       NEMEDICAID ne = new NEMEDICAID();


            //  ' txtRES.Text = ne.REQ("adasd");



        }

        private void txtReq_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtXML_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtBatchID_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtEBRId_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSign_Click(object sender, EventArgs e)
        {
            try
            {
                bool isCertPresent = false;
                byte[] plainData, signatureData;

                plainData = Encoding.UTF8.GetBytes(txtReq.Text);



                X509Store store = new X509Store(StoreLocation.CurrentUser);

                store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

                X509Certificate2Collection certificates = store.Certificates;

                X509Certificate2Collection foundCertificates = certificates.Find(X509FindType.FindByTimeValid, DateTime.Now, true);

                X509Certificate2Collection selectedCertificates = X509Certificate2UI.SelectFromCollection(foundCertificates,

                    "Select a Certificate.", "Select a Certificate from the following list to get information on that certificate", X509SelectionFlag.SingleSelection);



                if (selectedCertificates.Count > 0)
                {

                    X509Certificate2 cert = selectedCertificates[0];

                    //X509Certificate2 cert = new X509Certificate2();

                    //cert = SelectHarvardLocalCertificate();

                    if (cert != null)
                    {
                        ///webRequest.ClientCertificates.Add(cert);
                        ///
                        RSACryptoServiceProvider pkpriv = (RSACryptoServiceProvider)cert.PrivateKey;
                        int size = pkpriv.KeySize;  
                        //if (cert.GetCertHashString().ToLower())
                        if (cert.HasPrivateKey)
                        {
                            isCertPresent = true;
                            RSACryptoServiceProvider rsaEncryptor = (RSACryptoServiceProvider)cert.PrivateKey;

                            signatureData = rsaEncryptor.SignData(plainData, new SHA1CryptoServiceProvider());

                            txtRES.Text = Convert.ToBase64String(signatureData);

                        }

                        else
                        {

                            MessageBox.Show("the selected certificate does not contain a Private Key to use in signing data",

                                "No Private Key Available", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        }
                    }

                  
                  

                }

            }

            catch (CryptographicException ex)
            {

                MessageBox.Show(ex.Message, ex.GetType().ToString(),

                    MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, ex.GetType().ToString(),

                    MessageBoxButtons.OK, MessageBoxIcon.Error);

            }


        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            try
            {

                byte[] plainData, signatureData;

                plainData = Encoding.UTF8.GetBytes(txtReq.Text);

                signatureData = Convert.FromBase64String(txtRES.Text);



                X509Store store = new X509Store(StoreLocation.CurrentUser);

                store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);



                X509Certificate2Collection certificates = store.Certificates;

                X509Certificate2Collection foundCertificates = certificates.Find(X509FindType.FindByTimeValid, DateTime.Now, true);

                X509Certificate2Collection selectedCertificates = X509Certificate2UI.SelectFromCollection(foundCertificates,

                    "Select a Certificate.", "Select a Certificate from the following list to get information on that certificate", X509SelectionFlag.SingleSelection);



                if (selectedCertificates.Count > 0)
                {

                    X509Certificate2 cert = selectedCertificates[0];

                    //X509Certificate2 cert = new X509Certificate2();

                    //cert = SelectHarvardLocalCertificate();


                    RSACryptoServiceProvider rsaEncryptor = (RSACryptoServiceProvider)cert.PublicKey.Key;

                    if (rsaEncryptor.VerifyData(plainData, new SHA1CryptoServiceProvider(), signatureData))
                    {

                        MessageBox.Show("Your signature has been verified successfully.", "Success",

                                 MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }

                    else
                    {

                        MessageBox.Show("Your Data cannot be verified against the Signature.", "Failure",

                                 MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }

                }

            }

            catch (CryptographicException ex)
            {

                MessageBox.Show(ex.Message, ex.GetType().ToString(),

                    MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, ex.GetType().ToString(),

                    MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        public static X509Certificate2 SelectHarvardLocalCertificate()
        {
            //Dim HttpProps As String() = {"POST", "application", "octet-stream", "A null HTTP response object was returned.", "priv"}
            try
            {
                X509Store certStore = new X509Store(StoreName.My);
                certStore.Open(OpenFlags.OpenExistingOnly & OpenFlags.ReadOnly);
                X509Certificate2Collection clientCertificates = certStore.Certificates;
                foreach (X509Certificate2 oCert in clientCertificates)
                {
                    if (oCert.Issuer.Contains("HPHC2048"))
                    {
                        return oCert;
                    }
                }

            }
            catch (System.Security.Authentication.AuthenticationException ae)
            {
                //errLog.WriteEvent(ae.Message)
            }
            return null;
        }

    }






}
