using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
//using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

using System.IO;
using System.Net;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;

namespace Manual_test_app
{
    public partial class frmMain : Form
    {



        public frmMain()
        {
            InitializeComponent();
        }



        private void cmdREQ_Click(object sender, EventArgs e)
        {

        }

        private void cmdEDIParse_Click(object sender, EventArgs e)
        {
            // frm270 f270 = new frm270();
            // f270.MdiParent(frmMain);

            // f270.Show();
        }

        private void VendorCommunitcationsTestForm_Click(object sender, EventArgs e)
        {
            frmVCT frmVendorCommunitcationsTestForm = new frmVCT();

            frmVendorCommunitcationsTestForm.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmd835_Click(object sender, EventArgs e)
        {


            //frm835 f835 = new frm835();
            //f835.Show();

        }

        private void cmdFileFix_Click(object sender, EventArgs e)
        {
            frmFileFix ff = new frmFileFix();
            ff.Show();
        }

        private void tsb270_Click(object sender, EventArgs e)
        {

            //  frm270 f270 = new frm270();
            // f270.MdiParent(frmMain);

            // f270.Show();




        }

        private void tsb835_Click(object sender, EventArgs e)
        {

            //   frm835 f835 = new frm835();
            //  f835.Show();
        }

        private void tsb837_Click(object sender, EventArgs e)
        {

        }

        private void tsb278_Click(object sender, EventArgs e)
        {


            //  frmEDI278 f278 = new frmEDI278();
            //  f278.Show();


        }

        private void tsb276_Click(object sender, EventArgs e)
        {

            //    frm276 f276 = new frm276();

            //  f276.Show();



        }

        private void tsb277_Click(object sender, EventArgs e)
        {


            //  frm277 f277 = new frm277();

            //  f277.Show();

        }

        private void cmdEmail_Click(object sender, EventArgs e)
        {

            // SendEmail se = new SendEmail();
            // se.Show();



        }


        private  void Get45or451FromRegistry()
        {
            using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\"))
            {
                int releaseKey = Convert.ToInt32(ndpKey.GetValue("Release"));
                if (true)
                {
                    toolStripStatusLabel1.Text = (".NET Version: " + CheckFor45DotVersion(releaseKey) + "   Release Key:  " + Convert.ToString(releaseKey));
                }
            }
        }

        private void DisplayRuntimeVersion()
        {



        }


        // Checking the version using >= will enable forward compatibility,  
        // however you should always compile your code on newer versions of 
        // the framework to ensure your app works the same. 
        private  string CheckFor45DotVersion(int releaseKey)
        {



            if (releaseKey >= 461808)
            {
                return "4.7.2 or later";
            }
            if (releaseKey >= 461308)
            {
                return "4.7.1 or later";
            }

            if (releaseKey >= 460798)
            {
                return "4.7 or later";
            }
            if (releaseKey >= 394802)
            {
                return "4.6.2 or later";
            }
            if (releaseKey >= 394254)
            {
                return "4.6.1 or later";
            }
            if (releaseKey >= 393295)
            {
                return "4.6 or later";
            }

            if (releaseKey >= 393273)
            {
                return "4.6 RC or later";
            }
            if ((releaseKey >= 379893))
            {
                return "4.5.2 or later";
            }
            if ((releaseKey >= 378675))
            {
                return "4.5.1 or later";
            }
            if ((releaseKey >= 378389))
            {
                return "4.5 or later";
            }
            // This line should never execute. A non-null release key should mean 
            // that 4.5 or later is installed. 
            return "No 4.5 or later version detected";

        }




        private void frmMain_Load(object sender, EventArgs e)
        {
            Get45or451FromRegistry();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //   recrisiveserch r = new recrisiveserch();
            // r.Show();

        }

        private void cmdID_Click(object sender, EventArgs e)
        {

            // IDTest it = new IDTest();

            //  it.Show();

        }

        private void cmdTableWatcher_Click(object sender, EventArgs e)
        {





        }

        private void button2_Click(object sender, EventArgs e)
        {
            EventLog el = new EventLog();

            el.Show();


        }

        private void cmdXMLTester_Click(object sender, EventArgs e)
        {
            //XMLTester xm = new XMLTester();
            //xm.Show();
        }

        private void cmd278_Click(object sender, EventArgs e)
        {
            //  frm278 f278 = new frm278();
            // f278.Show();
        }

        private void cmdValidateEDI_Click(object sender, EventArgs e)
        {
            frmEDIValidate val = new frmEDIValidate();
            val.Show();
        }

        private void cmdLogging_Click(object sender, EventArgs e)
        {
            // frmLogging l = new frmLogging();
            // l.Show();
        }

        private void impersonateUserToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmInpersonate frmI = new frmInpersonate();
            frmI.Show();


        }

        private void cmdTA1_Click(object sender, EventArgs e)
        {
            // frmTA1 f = new frmTA1();
            //  f.Show();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            string _FilePath = string.Empty;
            string _FileName = string.Empty;
            string _RenameFileName = string.Empty;
            string _DropboxFolderName = string.Empty;
            try
            {
                _FilePath = ConfigurationManager.AppSettings["UploadFilePath"];
                _DropboxFolderName = ConfigurationManager.AppSettings["DropboxFolderName"];
                writefile("Upload Started");
                //Read files from upload folder
                string[] files = Directory.GetFiles(_FilePath, "*.dat");
                for (int i = 0; i < files.Length; i++)
                {
                    files[i] = Path.GetFileName(files[i]);
                    writefile("File Name:" + files[i]);
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://edi.tmhp.com/" + files[i]);
                    request.Method = WebRequestMethods.Ftp.UploadFile;
                    request.Credentials = new NetworkCredential("146223261", "8ll0tm24");


                    _FileName = _FilePath + files[i];
                    StreamReader sourceStream = new StreamReader(_FileName);
                    byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                    sourceStream.Close();
                    request.ContentLength = fileContents.Length;

                    writefile("File Name Path:" + _FileName);
                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(fileContents, 0, fileContents.Length);
                    requestStream.Close();
                    //                    
                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                    Console.WriteLine("Upload File Complete-" + response.StatusDescription);
                    writefile("Upload Status Code: " + response.StatusCode);
                    writefile("Upload Status Description: " + response.StatusDescription);
                    response.Close();
                    writefile("Upload Completed");

                    //////rename a file
                    //using (FtpWebRequest ftp = new FtpWebRequest.WebRequest(host, _params.Username, _params.Password))
                    //{ 

                    //}
                    //if (ftp.FileExists(_params.FinalLocation))
                    //{
                    //    ftp.RemoveFile(_params.FinalLocation);
                    //}

                    //ftp.RenameFile(target, _params.FinalLocation);

                    writefile("File Rename Started");

                    FtpWebRequest requestMove = (FtpWebRequest)WebRequest.Create("ftp://edi.tmhp.com/" + files[i]);
                    requestMove.Method = WebRequestMethods.Ftp.Rename;
                    requestMove.Credentials = new NetworkCredential("146223261", "8ll0tm24");
                    _RenameFileName = "/" + _DropboxFolderName + "/" + files[i];
                    writefile("Rename File Name: " + _RenameFileName);
                    requestMove.RenameTo = "/" + _DropboxFolderName + "/" + files[i];
                    requestMove.GetResponse();
                    writefile("File Rename End");
                    writefile("File Successfully Renamed and moved to dropbox folder");
                }
            }
            catch (Exception ex)
            {
                writefile(ex.Message.ToString());
            }
        }


        private void writefile(string strText)
        {
            using (StreamWriter sw = File.AppendText(@"C:\Programs\EDITool\UploadTool\Log.txt"))
            {
                sw.WriteLine(strText);
            }

        }

        private void button3_Click(object sender, System.EventArgs e)
        {
            int r = -1;
            string _RES = string.Empty;
            string _REQ = string.Empty;
            string SQL_CONNECTION_STRING = string.Empty;
            string _XMLParameters = string.Empty;
            int i = -1;
            SQL_CONNECTION_STRING = "";

            _XMLParameters = @"<?xml version='1.0' encoding='utf-8' ?><parameter>  <vendor name='AVAILITY'>    <wsdl>https://qa-apps.availity.com/availity/B2BHCTransactionServlet</wsdl>    <username></username>    <password></password>    <apiKey></apiKey>    <ContentType>application/x-www-form-urlencoded</ContentType>    <Method>POST</Method>    <ServiceTimeOut>120000</ServiceTimeOut>    <TimeToWait>150000</TimeToWait>    <LogToREQRES>0</LogToREQRES>  </vendor>  <vendor name='IVANS'>    <wsdl>https://eligibilityone.abilitynetwork.com/EligibilityOne.asmx</wsdl>    <username>8DC00005</username>    <password>DC$Gl0bal//2013</password>    <apiKey></apiKey>    <ContentType></ContentType>    <Method>POST</Method>    <ServiceTimeOut>120000</ServiceTimeOut>    <TimeToWait>150000</TimeToWait>    <LogToREQRES>0</LogToREQRES>    <ClientId>f9e52687-a2e0-4d9c-8591-59516c9fd026</ClientId>    <PreAuthenticate>true</PreAuthenticate>  </vendor>  <vendor name='POST-N-TRACK'>    <wsdl>https://realtime1.post-n-track.com/realtime/request_x12.aspx</wsdl>    <username>PNTUserName</username>    <password>PNTPassword</password>    <apiKey></apiKey>    <ContentType>application/x-www-form-urlencoded</ContentType>    <Method>POST</Method>    <ServiceTimeOut>120000</ServiceTimeOut>    <TimeToWait>150000</TimeToWait>    <LogToREQRES>0</LogToREQRES>  </vendor>  <vendor name='MEDDATA'>    <wsdl>https://services.meddatahealth.com/submissionportal/submissionportal.asmx?wsdl</wsdl>    <username>2019275</username>    <password>fT~H31]m</password>    <apiKey></apiKey>    <ContentType></ContentType>    <Method></Method>    <ServiceTimeOut>120000</ServiceTimeOut>    <TimeToWait>150000</TimeToWait>    <LogToREQRES>0</LogToREQRES>    <RequestFormat>FlatXml</RequestFormat>    <ResponseFormat>Edi</ResponseFormat>  </vendor>  <vendor name='VISIONSHARE'>    <VS_PROXY_PORT>80</VS_PROXY_PORT>    <VS_PROXY_SERVER></VS_PROXY_SERVER>    <VS_HOST_ADDRESS>204.11.46.227</VS_HOST_ADDRESS>    <VS_SES_PORT>4090</VS_SES_PORT>    <username>dcsgwpnt</username>    <password>Dx9m3fHvy</password>    <connection>SYNC_WELLPOINT</connection>    <VS_SES_PORT1>4091</VS_SES_PORT1>    <username1>dcsghets</username1>    <password1>dcsg821z</password1>    <connection1>SYNC_HETS_PROD</connection1>  </vendor>  <vendor name='DORADO'>    <wsdl>https://api.doradosystems.com/rt/validate?api_key=yccQdicTcceNbLQbPesciCrKH</wsdl>    <username></username>    <password></password>    <apiKey></apiKey>    <ContentType>application/x-www-form-urlencoded</ContentType>    <Method>POST</Method>    <ServiceTimeOut>120000</ServiceTimeOut>    <TimeToWait>150000</TimeToWait>    <LogToREQRES>0</LogToREQRES>  </vendor>  <vendor name='EMDEON'>    <wsdl>https://ra.emdeon.com/astwebservice/aws.asmx</wsdl>    <username>PMNADCSGL</username>    <password>TMURwvb3</password>    <apiKey></apiKey>    <ContentType>application/x-www-form-urlencoded</ContentType>    <Method>POST</Method>    <ServiceTimeOut>240000</ServiceTimeOut>    <TimeToWait>150000</TimeToWait>    <LogToREQRES>0</LogToREQRES>  </vendor>    <vendor name='NEBRASKA'>    <wsdl>https://COREWEB-DHHS.NE.GOV:10800/soap</wsdl>    <ContentType>text/xml;charset='utf-8'</ContentType>    <Accept>text/xml</Accept>    <Method>POST</Method>    <ServiceTimeOut>600000</ServiceTimeOut>    <TimeToWait>150000</TimeToWait>    <LogToREQRES>0</LogToREQRES>  </vendor>  <vendor name='TXMCD'>    <wsdl>http://edi-web.tmhp.com/TMHP/Request</wsdl>    <ContentType>text/xml;charset='utf-8'</ContentType>    <Accept>application/soap+xml</Accept>    <Method>POST</Method>    <ServiceTimeOut>600000</ServiceTimeOut>    <TimeToWait>150000</TimeToWait>    <LogToREQRES>0</LogToREQRES>  </vendor>      <vendor name='PARAMOUNTEDI'>    <wsdl>https://phcedi.promedica.org:50043/x12/realtime/soap?request_type=X12_270_Request_005010X279A1</wsdl>    <username></username>    <password></password>    <apiKey></apiKey>    <ContentType>text/xml;charset='utf-8'</ContentType>    <Accept>text/xml</Accept>    <Method>POST</Method>    <ServiceTimeOut>600000</ServiceTimeOut>    <TimeToWait>150000</TimeToWait>    <LogToREQRES>0</LogToREQRES>  </vendor> <vendor name='DCSTXMCD'>    <wsdl>http://10.34.1.206:9900/TxMcd.asmx</wsdl>    <ContentType>text/xml;charset='utf-8'</ContentType>    <Accept>application/soap+xml</Accept>    <Method>POST</Method>    <ServiceTimeOut>600000</ServiceTimeOut>    <TimeToWait>150000</TimeToWait>    <LogToREQRES>0</LogToREQRES>  </vendor>  </parameter>";

            DCSGlobal.EDI.Comunications.EDIGetResponse ge = new DCSGlobal.EDI.Comunications.EDIGetResponse();
            ge.ConnectionString = SQL_CONNECTION_STRING;
            ge.XMLParameters = _XMLParameters;
            _REQ = "ISA*00*          *00*          *ZZ*146223261      *ZZ*617591011C21T  *160715*1142*^*00501*100589486*0*P*:~GS*HS*146223261*617591011C21T*20160715*114200*589486*X*005010X279A1~ST*270*589486*005010X279A1~BHT*0022*13*12X34*20160715*114200~HL*1**20*1~NM1*PR*2*TEXAS MEDICAID*****PI*10730~HL*2*1*21*1~NM1*1P*2*RCHP*****XX*1063411767~HL*3*2*22*0~NM1*IL*1*HARRIS*ANDY*J***MI*5275629852~DMG*D8*197909232*M~DTP*291*D8*20160725~EQ*30~SE*12*589486~GE*1*589486~IEA*1*100589486~";
            i = ge.getEligVendorEdiResponse(_REQ, "00007", "TXMCD", "EDI");
            if (i == 0)
            {
                _RES = ge.RES;
            }
            else
            {
                _RES = "";
            }

            //objRes.XMLParameters = @"<?xml version='1.0' encoding='utf-8' ?><parameter>  <vendor name='AVAILITY'>    <wsdl>https://qa-apps.availity.com/availity/B2BHCTransactionServlet</wsdl>    <username></username>    <password></password>    <apiKey></apiKey>    <ContentType>application/x-www-form-urlencoded</ContentType>    <Method>POST</Method>    <ServiceTimeOut>120000</ServiceTimeOut>    <TimeToWait>150000</TimeToWait>    <LogToREQRES>0</LogToREQRES>  </vendor>  <vendor name='IVANS'>    <wsdl>https://eligibilityone.abilitynetwork.com/EligibilityOne.asmx</wsdl>    <username>8DC00005</username>    <password>DC$Gl0bal//2013</password>    <apiKey></apiKey>    <ContentType></ContentType>    <Method>POST</Method>    <ServiceTimeOut>120000</ServiceTimeOut>    <TimeToWait>150000</TimeToWait>    <LogToREQRES>0</LogToREQRES>    <ClientId>f9e52687-a2e0-4d9c-8591-59516c9fd026</ClientId>    <PreAuthenticate>true</PreAuthenticate>  </vendor>  <vendor name='POST-N-TRACK'>    <wsdl>https://realtime1.post-n-track.com/realtime/request_x12.aspx</wsdl>    <username>PNTUserName</username>    <password>PNTPassword</password>    <apiKey></apiKey>    <ContentType>application/x-www-form-urlencoded</ContentType>    <Method>POST</Method>    <ServiceTimeOut>120000</ServiceTimeOut>    <TimeToWait>150000</TimeToWait>    <LogToREQRES>0</LogToREQRES>  </vendor>  <vendor name='MEDDATA'>    <wsdl>https://services.meddatahealth.com/submissionportal/submissionportal.asmx?wsdl</wsdl>    <username>2019275</username>    <password>fT~H31]m</password>    <apiKey></apiKey>    <ContentType></ContentType>    <Method></Method>    <ServiceTimeOut>120000</ServiceTimeOut>    <TimeToWait>150000</TimeToWait>    <LogToREQRES>0</LogToREQRES>    <RequestFormat>FlatXml</RequestFormat>    <ResponseFormat>Edi</ResponseFormat>  </vendor>  <vendor name='VISIONSHARE'>    <VS_PROXY_PORT>80</VS_PROXY_PORT>    <VS_PROXY_SERVER></VS_PROXY_SERVER>    <VS_HOST_ADDRESS>204.11.46.227</VS_HOST_ADDRESS>    <VS_SES_PORT>4090</VS_SES_PORT>    <username>dcsgwpnt</username>    <password>Dx9m3fHvy</password>    <connection>SYNC_WELLPOINT</connection>    <VS_SES_PORT1>4091</VS_SES_PORT1>    <username1>dcsghets</username1>    <password1>dcsg821z</password1>    <connection1>SYNC_HETS_PROD</connection1>  </vendor>  <vendor name='DORADO'>    <wsdl>https://api.doradosystems.com/rt/validate?api_key=yccQdicTcceNbLQbPesciCrKH</wsdl>    <username></username>    <password></password>    <apiKey></apiKey>    <ContentType>application/x-www-form-urlencoded</ContentType>    <Method>POST</Method>    <ServiceTimeOut>120000</ServiceTimeOut>    <TimeToWait>150000</TimeToWait>    <LogToREQRES>0</LogToREQRES>  </vendor>  <vendor name='EMDEON'>    <wsdl>https://ra.emdeon.com/astwebservice/aws.asmx</wsdl>    <username>PMNADCSGL</username>    <password>TMURwvb3</password>    <apiKey></apiKey>    <ContentType>application/x-www-form-urlencoded</ContentType>    <Method>POST</Method>    <ServiceTimeOut>240000</ServiceTimeOut>    <TimeToWait>150000</TimeToWait>    <LogToREQRES>0</LogToREQRES>  </vendor>    <vendor name='NEBRASKA'>    <wsdl>https://COREWEB-DHHS.NE.GOV:10800/soap</wsdl>    <ContentType>text/xml;charset='utf-8'</ContentType>    <Accept>text/xml</Accept>    <Method>POST</Method>    <ServiceTimeOut>600000</ServiceTimeOut>    <TimeToWait>150000</TimeToWait>    <LogToREQRES>0</LogToREQRES>  </vendor>  <vendor name='TXMCD'>    <wsdl>http://edi-web.tmhp.com/TMHP/Request</wsdl>    <ContentType>text/xml;charset='utf-8'</ContentType>    <Accept>application/soap+xml</Accept>    <Method>POST</Method>    <ServiceTimeOut>600000</ServiceTimeOut>    <TimeToWait>150000</TimeToWait>    <LogToREQRES>0</LogToREQRES>  </vendor>      <vendor name='PARAMOUNTEDI'>    <wsdl>https://phcedi.promedica.org:50043/x12/realtime/soap?request_type=X12_270_Request_005010X279A1</wsdl>    <username></username>    <password></password>    <apiKey></apiKey>    <ContentType>text/xml;charset='utf-8'</ContentType>    <Accept>text/xml</Accept>    <Method>POST</Method>    <ServiceTimeOut>600000</ServiceTimeOut>    <TimeToWait>150000</TimeToWait>    <LogToREQRES>0</LogToREQRES>  </vendor> <vendor name='DCSTXMCD'>    <wsdl>http://10.34.1.206:9900/TxMcd.asmx</wsdl>    <ContentType>text/xml;charset='utf-8'</ContentType>    <Accept>application/soap+xml</Accept>    <Method>POST</Method>    <ServiceTimeOut>600000</ServiceTimeOut>    <TimeToWait>150000</TimeToWait>    <LogToREQRES>0</LogToREQRES>  </vendor>  </parameter>";

        }
        static int GetXMLParameters()
        {
            int r = -1;
            string dbTempString = string.Empty;
            string _XMLParameters = string.Empty;
            try
            {

                using (SqlConnection con = new SqlConnection(dbTempString))
                {
                    con.Open();
                    //
                    // Create new SqlCommand object.
                    //
                    using (SqlCommand cmd = new SqlCommand("usp_get_system_preferences", con))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@moduleName", "EDIGetResponse");
                        cmd.Parameters.AddWithValue("@lovName ", "_XMLParameters");

                        using (SqlDataReader idr = cmd.ExecuteReader())
                        {
                            //  count = idr.VisibleFieldCount;




                            if (idr.HasRows)
                            {
                                while (idr.Read())
                                {

                                    _XMLParameters = Convert.ToString(idr["lov_value"]);
                                    r = 0;
                                }
                            }
                        }
                    }
                }

            }
            catch (SqlException sx)
            {

                //log.ExceptionDetails(_appName, sx);
            }

            catch (Exception ex)
            {

                //log.ExceptionDetails(_appName, ex);
            }


            return r;

        }

        private void cmd277_Click(object sender, System.EventArgs e)
        {
            //   frm277 f277 = new frm277();

            // f277.Show();
        }

        private void cmdFileTransfer_Click(object sender, System.EventArgs e)
        {
            // frmBYTEFileTransfer f = new frmBYTEFileTransfer();
            // f.Show();
        }

        private void button8_Click(object sender, System.EventArgs e)
        {
            //  ClearScript fcs = new ClearScript();
            // fcs.Show();
        }

        private void toolStripMenuItem1_Click(object sender, System.EventArgs e)
        {
            //  frmRamdomNumberGenerator f = new frmRamdomNumberGenerator();
            // f.Show();
        }

        private void cmdSE_Click(object sender, System.EventArgs e)
        {
            //  frmScripting f = new frmScripting();
            //  f.Show();
        }

        private void button10_Click(object sender, System.EventArgs e)
        {
            // frmNPTest f = new frmNPTest();
            //  f.Show();
        }

        private void txtRI_Click(object sender, System.EventArgs e)
        {
            //string strUrl ="https://www.eohhs.ri.gov/portal/WebUploadFromClient";
            string strUrl = "https://www.ctmedicalprogram/test/secure/WebDirectoryDownloadFromClient";
            //string strFileNames =@"C:\App\EdiVendors.root\EdiVendors\EdiVendors\bin\Debug\test1.dat#";
            //string[] uploadFiles = strFileNames.Split("#");
            string strResponse = string.Empty;

            // strResponse = UploadFilesToRemoteUrl(strUrl);
            uploadFile();


        }
        //public static string UploadFilesToRemoteUrl(string url, string[] files, NameValueCollection formFields = null)


        private void uploadFile()
        {
            string MBF_URL = "https://www.eohhs.ri.gov/portal/WebUploadFromClient";
            //string MBF_URL = "https://www.ctmedicalprogram/test/secure/WebDirectoryDownloadFromClient";
            try
            {
                Random rand = new Random();
                string boundary = "----boundary" + rand.Next().ToString();
                Stream data_stream;
                byte[] header = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"file_path\"; filename=\"" + "test1.dat" + "\"\r\nContent-Type: Content-Type: text/xml\r\n\r\n");
                byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");

                // Do the request
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(MBF_URL);
                request.UserAgent = "My Toolbox";
                request.Method = "POST";
                request.KeepAlive = true;
                request.ContentType = "multipart/form-data; boundary=" + boundary;

                data_stream = request.GetRequestStream();
                data_stream.Write(header, 0, header.Length);
                string file = "test1.dat";
                long sizeInBytes = new System.IO.FileInfo("test1.dat").Length;

                string xml = string.Empty;
                xml = xml + "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                xml = xml + "<UploadRequest>";
                xml = xml + "<IdentificationHeader>";
                xml = xml + "<TradingPartnerId>710000520</TradingPartnerId>";
                xml = xml + "<UserId>710000520</UserId>";
                xml = xml + "<Password>Cowboys1</Password>";
                xml = xml + "</IdentificationHeader>";
                xml = xml + "<Transaction>";
                xml = xml + "<Function>uploadFile</Function>";
                xml = xml + "<FileName>test1.dat</FileName>";
                xml = xml + "<FileSize>" + Convert.ToString(sizeInBytes) + "</FileSize>";
                xml = xml + "</Transaction>";
                xml = xml + "</UploadRequest>";


                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(xml);
                data_stream.Write(bytes, 0, bytes.Length);

                byte[] file_bytes = System.IO.File.ReadAllBytes(file);
                data_stream.Write(file_bytes, 0, file_bytes.Length);
                data_stream.Write(trailer, 0, trailer.Length);
                data_stream.Close();

                // Read the response
                WebResponse response = request.GetResponse();
                data_stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(data_stream);
                string strRetVal = string.Empty;
                strRetVal = reader.ReadToEnd();

                if (strRetVal == "") { strRetVal = "No response :("; }

                reader.Close();
                data_stream.Close();
                response.Close();
            }
            catch (WebException ex)
            {
                string strErr = string.Empty;
                strErr = ex.Message;
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    strErr = "Status Code : " + ((HttpWebResponse)ex.Response).StatusCode;
                    strErr = strErr + "Status Description : " + ((HttpWebResponse)ex.Response).StatusDescription;

                }
            }
            catch (Exception ex)
            {
                string strErr = string.Empty;
                strErr = ex.Message;
            }

        }

        public static string UploadFilesToRemoteUrl(string url)
        {
            string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "multipart/form-data; boundary=" +
                                    boundary;
            request.Method = "POST";
            request.KeepAlive = true;

            Stream memStream = new System.IO.MemoryStream();

            var boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" +
                                                                    boundary + "\r\n");
            var endBoundaryBytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" +
                                                                        boundary + "--");
            string[] files = new string[] { @"C:\App\EdiVendors.root\EdiVendors\EdiVendors\bin\Debug\test1.dat" };

            string formdataTemplate = "\r\n--" + boundary +
                                        "\r\nContent-Disposition: form-data; name=\"textFileAttached\";\r\n\r\n{1}";

            //if (formFields != null)
            //{
            //    foreach (string key in formFields.Keys)
            //    {
            //        string formitem = string.Format(formdataTemplate, key, formFields[key]);
            //        byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
            //        memStream.Write(formitembytes, 0, formitembytes.Length);
            //    }
            //}

            string headerTemplate =
                "Content-Disposition: form-data; name=\"textFileAttached\"; filename=\"{1}\"\r\n" +
                "Content-Type: text/plain\r\n\r\n";

            for (int i = 0; i < files.Length; i++)
            {
                memStream.Write(boundarybytes, 0, boundarybytes.Length);
                var header = string.Format(headerTemplate, "uplTheFile", files[i]);
                var headerbytes = System.Text.Encoding.UTF8.GetBytes(header);

                memStream.Write(headerbytes, 0, headerbytes.Length);

                using (var fileStream = new FileStream(files[i], FileMode.Open, FileAccess.Read))
                {
                    var buffer = new byte[1024];
                    var bytesRead = 0;
                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        memStream.Write(buffer, 0, bytesRead);
                    }
                }
            }

            memStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
            request.ContentLength = memStream.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                memStream.Position = 0;
                byte[] tempBuffer = new byte[memStream.Length];
                memStream.Read(tempBuffer, 0, tempBuffer.Length);
                memStream.Close();
                requestStream.Write(tempBuffer, 0, tempBuffer.Length);
            }

            using (var response = request.GetResponse())
            {
                Stream stream2 = response.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                return reader2.ReadToEnd();
            }
        }

        private void verifyBySwapAdd1Add2ToolStripMenuItem_Click(object sender, System.EventArgs e)
        {

            frmUnitTest_AddressAPI faddsqap = new frmUnitTest_AddressAPI();
            faddsqap.Show();
        }

        private void loginToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            // frmALLogin fal = new frmALLogin();
            // fal.Show();
        }

        private void cmdRA_Click(object sender, System.EventArgs e)
        {
            //  RAServerTest ra = new RAServerTest();
            // ra.Show();
        }

        private void cmdEDI5010_Click(object sender, System.EventArgs e)
        {
            frmEDI5010 f = new frmEDI5010();
            f.Show();
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void eventLogViewerToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Logs.EDLogViewer evl = new Logs.EDLogViewer();
            evl.Show();
        }

        private void schedulerLogViewerToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Logs.SchedulerLogViewer sch = new Logs.SchedulerLogViewer();
            sch.Show();
        }

        private void thorToolStripMenuItem_Click(object sender, System.EventArgs e)
        {



        }

        private void linqPadToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            // LinQ.LinQPad f = new LinQ.LinQPad();
            //  f.Show();
        }

        private void eventLogTestToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            EventLog ev = new EventLog();
            ev.Show();
        }

        private void dbScanToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            DEPLOYMENT.frmPackageObjects dbs = new DEPLOYMENT.frmPackageObjects();
            dbs.Show();
        }

        private void mLogToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            //  Logs.MLog mlgs = new Logs.MLog();
            //  mlgs.Show();
        }

        private void ruleEditorToolStripMenuItem_Click(object sender, System.EventArgs e)
        {


            //Rules.RuleConverterVBStoVB rr = new Rules.RuleConverterVBStoVB();
            //rr.Show();
        }

        private void button1_Click_1(object sender, System.EventArgs e)
        {
            frmScratchPad l = new frmScratchPad();
            l.Show();
        }

        private void regEXToolStripMenuItem_Click(object sender, System.EventArgs e)
        {

            //  PIEBALD.RegexTester.frmRegexTester rt = new PIEBALD.RegexTester.frmRegexTester();

            //   rt.Show();

        }

        private void connectionStringsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            db.frmConnectionStrings f = new db.frmConnectionStrings();
            f.Show();
        }

        private void newPackageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DEPLOYMENT.frmNewPackage f = new DEPLOYMENT.frmNewPackage();
            f.Show();
        }

        private void deployPackageToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DEPLOYMENT.frmDeployPackage f = new DEPLOYMENT.frmDeployPackage();
            f.Show();

        }

        private void iPRangesToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Manual_test_app.IPRanges.frmIPRangeImport f = new IPRanges.frmIPRangeImport();
            f.Show();
        }

        private void cmdRules_Click(object sender, EventArgs e)
        {



            RuleTestCompile RTC = new RuleTestCompile();
            RTC.Show();


        }

        private void ruleTestorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RuleTestCompile RTC = new RuleTestCompile();
            RTC.Show();
        }

        private void ruleResultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Rules.RuleResults rr = new Rules.RuleResults();
            //rr.Show();
        }

        private void aDLoginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tools.LDAPWS ad = new Tools.LDAPWS();
            ad.Show();
        }
    }
}
