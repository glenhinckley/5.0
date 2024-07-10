using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace DCSGlobal.Medical.VoiceRecoder
{
    class AppSettings : IDisposable
    {
        ~AppSettings()
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


        Dictionary<string, string> _strings = new Dictionary<string, string>();

        public bool TestURLEndPoint(string EndPoint)
        {

            bool b = false;

            Uri u = new Uri(EndPoint);
            int ii = 0;
            try
            {
                using (srRegister.RegisterPager1SoapClient srR = new srRegister.RegisterPager1SoapClient())
                {
                    srR.Endpoint.Address = new System.ServiceModel.EndpointAddress(u + "RegisterPager.asmx");
                    ii = Convert.ToInt32(srR.TestConnection());
                }
                if (ii == 1)
                {

                    b = true;
                }
            }
            catch (Exception ex)
            {

            }


            return b;

        }

        public void GetAll()
        {
            BuildString();
            LoadString();

        }
        public string EndPointURI
        {
            get
            {
                return _strings["EndPointURI"].ToString();
            }
            set
            {
                _strings["EndPointURI"] = value;
                SaveStrings();

            }
        }


        public string FilePath
        {
            get
            {
                return _strings["FilePath"].ToString();
            }
            set
            {
                _strings["FilePath"] = value;
                SaveStrings();

            }
        }

        public int PhoneLevel
        {
            get
            {
                return Convert.ToInt32(_strings["PhoneLevel"].ToString());
            }
            set
            {

                _strings["PhoneLevel"] = Convert.ToString(value);
                SaveStrings();

            }
        }

        public int MicLevel
        {
            get
            {
                return Convert.ToInt32(_strings["MicLevel"].ToString());
            }
            set
            {
                _strings["MicLevel"] = Convert.ToString(value);
                SaveStrings();
            }
        }

     
        
        public string ClientName
        {
            get
            {

                return "TEST_CN";
                //return _strings["FilePath"].ToString();
            }
            set
            {
                //_strings["FilePath"] = value;
               // SaveStrings();

            }
        }


        public string RootDirectory
        {
            get
            {

                return @"c:\webdir_data\";
                //return _strings["FilePath"].ToString();
            }
            set
            {
                //_strings["FilePath"] = value;
                // SaveStrings();

            }
        }


        public string ClientDirectory
        {
            get
            {

                return @"audio\";
                //return _strings["FilePath"].ToString();
            }
            set
            {
                //_strings["FilePath"] = value;
                // SaveStrings();

            }
        }

        public void ResetPager()
        {

            Application.UserAppDataRegistry.SetValue("PhoneLevel", "50");
            Application.UserAppDataRegistry.SetValue("MicLevel", "50");
            Application.UserAppDataRegistry.SetValue("FilePath", @"c:\usr");

            Application.UserAppDataRegistry.SetValue("DISCLAIM", string.Empty);
            Application.UserAppDataRegistry.SetValue("DISCLAIM_HTML", string.Empty);
            Application.UserAppDataRegistry.SetValue("HOSP_COPY", string.Empty);
            Application.UserAppDataRegistry.SetValue("HOSP_CODE", string.Empty);
            Application.UserAppDataRegistry.SetValue("HOSP_TITLE", string.Empty);
            Application.UserAppDataRegistry.SetValue("PAGERKEY", "0");
            Application.UserAppDataRegistry.SetValue("FIRSTRUN", "n");
            Application.UserAppDataRegistry.SetValue("isActive", "0");
            Application.UserAppDataRegistry.SetValue("EndPointURI", string.Empty);
            Application.UserAppDataRegistry.SetValue("PASSWD", string.Empty);
            Application.UserAppDataRegistry.SetValue("HOSP_ABOUT_TITLE", string.Empty);
            Application.UserAppDataRegistry.SetValue("HOSP_ABOUT_COPY", string.Empty);
            Application.UserAppDataRegistry.SetValue("HOSP_ABOUT_COPY_URL", string.Empty);
            Application.UserAppDataRegistry.SetValue("cmdWaitTime", string.Empty);
            Application.UserAppDataRegistry.SetValue("cmdAbout", string.Empty);
            Application.UserAppDataRegistry.SetValue("cmdDisclaimer", string.Empty);
            Application.UserAppDataRegistry.SetValue("MSGClearCode", string.Empty);
            Application.UserAppDataRegistry.SetValue("HW", string.Empty);



        }
        private void BuildString()
        {



            _strings.Add("PhoneLevel", string.Empty);
            _strings.Add("MicLevel", string.Empty);
            _strings.Add("FilePath", string.Empty);



            _strings.Add("ACTION", string.Empty);
            _strings.Add("PAGER_TITLE", string.Empty);
            _strings.Add("PAGER_MSG", string.Empty);
            _strings.Add("SURVERY_URI", string.Empty);
            _strings.Add("SURVERY_TITLE", string.Empty);
            _strings.Add("EST", string.Empty);
            _strings.Add("DISCLAIM", string.Empty);
            _strings.Add("DISCLAIM_HTML", string.Empty);
            _strings.Add("HOSP_COPY_TITLE", string.Empty);
            _strings.Add("HOSP_COPY_TEXT", string.Empty);
            _strings.Add("HOSP_ABOUT", string.Empty);
            _strings.Add("HOSP_TITLE", string.Empty);
            _strings.Add("PAGERKEY", "0");
            _strings.Add("FIRSTRUN", string.Empty);
            _strings.Add("isActive", string.Empty);
            _strings.Add("HOSP_CODE", string.Empty);
            _strings.Add("EndPointURI", string.Empty);
            _strings.Add("PASSWD", string.Empty);
            _strings.Add("HOSP_ABOUT_TITLE", string.Empty);
            _strings.Add("HOSP_ABOUT_COPY", string.Empty);
            _strings.Add("HOSP_ABOUT_HTML", string.Empty);
            _strings.Add("cmdWaitTime", string.Empty);
            _strings.Add("cmdAbout", string.Empty);
            _strings.Add("cmdDisclaimer", string.Empty);
            _strings.Add("cmdSurvey", string.Empty);
            _strings.Add("Colors", string.Empty);
            _strings.Add("HW", string.Empty);
            _strings.Add("MSG_ID", string.Empty);
            _strings.Add("POLL_TIME", string.Empty);
            _strings.Add("MSGClearCode", string.Empty);



        }


        private void LoadString()
        {

            if (Application.UserAppDataRegistry.GetValue("FilePath") != null)
            {
                _strings["FilePath"] = Application.UserAppDataRegistry.GetValue("FilePath", @"c:\usr").ToString();
            }
            else
            {
                _strings["FilePath"] = @"c:\usr";
            }



            if (Application.UserAppDataRegistry.GetValue("PhoneLevel") != null)
            {
                _strings["PhoneLevel"] = Application.UserAppDataRegistry.GetValue("PhoneLevel", "50").ToString();
            }
            else
            {
                _strings["PhoneLevel"] = "50";
            }


            if (Application.UserAppDataRegistry.GetValue("MicLevel") != null)
            {
                _strings["MicLevel"] = Application.UserAppDataRegistry.GetValue("MicLevel", "50").ToString();
            }
            else
            {
                _strings["MicLevel"] = "50";
            }



            if (Application.UserAppDataRegistry.GetValue("EST") != null)
            {
                _strings["EST"] = Application.UserAppDataRegistry.GetValue("EST", "").ToString();
            }
            else
            {
                _strings["EST"] = "";
            }




            if (Application.UserAppDataRegistry.GetValue("EndPointURI") != null)
            {
                _strings["EndPointURI"] = Application.UserAppDataRegistry.GetValue("EndPointURI", "").ToString();
            }
            else
            {
                _strings["EndPointURI"] = "";
            }




            if (Application.UserAppDataRegistry.GetValue("DISCLAIM") != null)
            {
                _strings["DISCLAIM"] = Application.UserAppDataRegistry.GetValue("DISCLAIM", "").ToString();
            }
            else
            {
                _strings["DISCLAIM"] = "";
            }




            if (Application.UserAppDataRegistry.GetValue("DISCLAIM_HTML") != null)
            {
                _strings["DISCLAIM_HTML"] = Application.UserAppDataRegistry.GetValue("DISCLAIM_HTML", "").ToString();
            }
            else
            {
                _strings["DISCLAIM_HTML"] = "";
            }


            if (Application.UserAppDataRegistry.GetValue("HOSP_COPY_TITLE") != null)
            {
                _strings["HOSP_COPY_TITLE"] = Application.UserAppDataRegistry.GetValue("HOSP_COPY_TITLE", "").ToString();
            }
            else
            {
                _strings["HOSP_COPY_TITLE"] = "";
            }



            if (Application.UserAppDataRegistry.GetValue("HOSP_COPY_TEXT") != null)
            {
                _strings["HOSP_COPY_TEXT"] = Application.UserAppDataRegistry.GetValue("HOSP_COPY_TEXT", "").ToString();
            }
            else
            {
                _strings["HOSP_COPY_TEXT"] = "";
            }


            if (Application.UserAppDataRegistry.GetValue("HOSP_ABOUT") != null)
            {
                _strings["HOSP_ABOUT"] = Application.UserAppDataRegistry.GetValue("HOSP_ABOUT", "").ToString();
            }
            else
            {
                _strings["HOSP_ABOUT"] = "";
            }




            if (Application.UserAppDataRegistry.GetValue("HOSP_ABOUT_HTML") != null)
            {
                _strings["HOSP_ABOUT_HTML"] = Application.UserAppDataRegistry.GetValue("HOSP_ABOUT_HTML", "n").ToString();
            }
            else
            {
                _strings["HOSP_ABOUT_HTML"] = "";
            }


            if (Application.UserAppDataRegistry.GetValue("HOSP_TITLE") != null)
            {
                _strings["HOSP_TITLE"] = Application.UserAppDataRegistry.GetValue("HOSP_TITLE", "").ToString();
            }
            else
            {
                _strings["HOSP_TITLE"] = "";
            }


            if (Application.UserAppDataRegistry.GetValue("PAGERKEY") != null)
            {
                _strings["PAGERKEY"] = Application.UserAppDataRegistry.GetValue("PAGERKEY", "0").ToString();
            }
            else
            {
                _strings["PAGERKEY"] = "0";
            }



            if (Application.UserAppDataRegistry.GetValue("isActive") != null)
            {
                _strings["isActive"] = Application.UserAppDataRegistry.GetValue("isActive", "0").ToString();
            }
            else
            {
                _strings["isActive"] = "0";
            }



            if (Application.UserAppDataRegistry.GetValue("FIRSTRUN") != null)
            {
                _strings["FIRSTRUN"] = Application.UserAppDataRegistry.GetValue("FIRSTRUN", "n").ToString();
            }
            else
            {
                _strings["FIRSTRUN"] = "";
            }



            if (Application.UserAppDataRegistry.GetValue("HOSP_CODE") != null)
            {
                _strings["HOSP_CODE"] = Application.UserAppDataRegistry.GetValue("HOSP_CODE", "").ToString();
            }
            else
            {
                _strings["HOSP_CODE"] = "";
            }




            if (Application.UserAppDataRegistry.GetValue("HOSP_ABOUT_TITLE") != null)
            {
                _strings["HOSP_ABOUT_TITLE"] = Application.UserAppDataRegistry.GetValue("HOSP_ABOUT_TITLE", "").ToString();
            }
            else
            {
                _strings["HOSP_ABOUT_TITLE"] = "";
            }




            if (Application.UserAppDataRegistry.GetValue("HOSP_ABOUT_COPY") != null)
            {
                _strings["HOSP_ABOUT_COPY"] = Application.UserAppDataRegistry.GetValue("HOSP_ABOUT_COPY", "").ToString();
            }
            else
            {
                _strings["HOSP_ABOUT_COPY"] = "";
            }


            if (Application.UserAppDataRegistry.GetValue("HOSP_ABOUT_COPY") != null)
            {
                _strings["HOSP_ABOUT_COPY"] = Application.UserAppDataRegistry.GetValue("HOSP_ABOUT_COPY", "").ToString();
            }
            else
            {
                _strings["HOSP_ABOUT_COPY"] = "";
            }



            if (Application.UserAppDataRegistry.GetValue("cmdWaitTime") != null)
            {
                _strings["cmdWaitTime"] = Application.UserAppDataRegistry.GetValue("cmdWaitTime", "Wait Time").ToString();
            }
            else
            {
                _strings["cmdWaitTime"] = "Wait Time";
            }


            if (Application.UserAppDataRegistry.GetValue("cmdAbout") != null)
            {
                _strings["cmdAbout"] = Application.UserAppDataRegistry.GetValue("cmdAbout", "About").ToString();
            }
            else
            {
                _strings["cmdAbout"] = "About";
            }



            if (Application.UserAppDataRegistry.GetValue("cmdDisclaimer") != null)
            {
                _strings["cmdDisclaimer"] = Application.UserAppDataRegistry.GetValue("cmdDisclaimer", "Disclaimer").ToString();
            }
            else
            {
                _strings["cmdDisclaimer"] = "Disclaimer";
            }


            if (Application.UserAppDataRegistry.GetValue("cmdSurvey") != null)
            {
                _strings["cmdSurvey"] = Application.UserAppDataRegistry.GetValue("cmdSurvey", "Survey").ToString();
            }
            else
            {
                _strings["cmdSurvey"] = "Survey";
            }



            if (Application.UserAppDataRegistry.GetValue("Colors") != null)
            {
                _strings["Colors"] = Application.UserAppDataRegistry.GetValue("Colors", "blue").ToString();
            }
            else
            {
                _strings["Colors"] = "blue";
            }



            if (Application.UserAppDataRegistry.GetValue("HW") != null)
            {
                _strings["HW"] = Application.UserAppDataRegistry.GetValue("HW", "").ToString();
            }
            else
            {
                _strings["HW"] = "";
            }



            if (Application.UserAppDataRegistry.GetValue("POLL_TIME") != null)
            {
                _strings["POLL_TIME"] = Application.UserAppDataRegistry.GetValue("POLL_TIME", "30000").ToString();
            }
            else
            {
                _strings["POLL_TIME"] = "30000";
            }


            if (Application.UserAppDataRegistry.GetValue("PASSWD") != null)
            {
                _strings["PASSWD"] = Application.UserAppDataRegistry.GetValue("PASSWD", "42").ToString();
            }
            else
            {
                _strings["PASSWD"] = "42";
            }


            if (Application.UserAppDataRegistry.GetValue("MSGClearCode") != null)
            {
                _strings["MSGClearCode"] = Application.UserAppDataRegistry.GetValue("MSGClearCode", "42").ToString();
            }
            else
            {
                _strings["MSGClearCode"] = "42";
            }




        }


        private void SaveStrings()
        {





            if (_strings.ContainsKey("FilePath") == true)
            {
                Application.UserAppDataRegistry.SetValue("FilePath", _strings["FilePath"].ToString());
            }



            if (_strings.ContainsKey("PhoneLevel") == true)
            {
                Application.UserAppDataRegistry.SetValue("PhoneLevel", _strings["PhoneLevel"].ToString());
            }



            if (_strings.ContainsKey("MicLevel") == true)
            {
                Application.UserAppDataRegistry.SetValue("MicLevel", _strings["MicLevel"].ToString());
            }




            if (_strings.ContainsKey("EST") == true)
            {
                Application.UserAppDataRegistry.SetValue("EST", _strings["EST"].ToString());
            }








            if (_strings.ContainsKey("isActive") == true)
            {
                Application.UserAppDataRegistry.SetValue("isActive", _strings["isActive"].ToString());
            }






            if (_strings.ContainsKey("EndPointURI") == true)
            {
                Application.UserAppDataRegistry.SetValue("EndPointURI", _strings["EndPointURI"].ToString());
            }


            if (_strings.ContainsKey("PAGERKEY") == true)
            {
                Application.UserAppDataRegistry.SetValue("PAGERKEY", _strings["PAGERKEY"].ToString());
            }


            if (_strings.ContainsKey("HOSP_CODE") == true)
            {
                Application.UserAppDataRegistry.SetValue("HOSP_CODE", _strings["HOSP_CODE"].ToString());
            }







            if (_strings.ContainsKey("DISCLAIM") == true)
            {
                Application.UserAppDataRegistry.SetValue("DISCLAIM", _strings["DISCLAIM"].ToString());
            }


            if (_strings.ContainsKey("DISCLAIM_HTML") == true)
            {
                Application.UserAppDataRegistry.SetValue("DISCLAIM_HTML", _strings["DISCLAIM_HTML"].ToString());
            }



            if (_strings.ContainsKey("HOSP_COPY_TITLE") == true)
            {
                Application.UserAppDataRegistry.SetValue("HOSP_COPY_TITLE", _strings["HOSP_COPY_TITLE"].ToString());
            }


            if (_strings.ContainsKey("HOSP_COPY_TEXT") == true)
            {
                Application.UserAppDataRegistry.SetValue("HOSP_COPY_TEXT", _strings["HOSP_COPY_TEXT"].ToString());
            }




            if (_strings.ContainsKey("HOSP_TITLE") == true)
            {
                Application.UserAppDataRegistry.SetValue("HOSP_TITLE", _strings["HOSP_TITLE"].ToString());
            }


            if (_strings.ContainsKey("HOSP_ABOUT") == true)
            {
                Application.UserAppDataRegistry.SetValue("HOSP_ABOUT", _strings["HOSP_ABOUT"].ToString());
            }


            if (_strings.ContainsKey("HOSP_ABOUT_HTML") == true)
            {
                Application.UserAppDataRegistry.SetValue("HOSP_ABOUT_HTML", _strings["HOSP_ABOUT_HTML"].ToString());
            }


            if (_strings.ContainsKey("HOSP_ABOUT_TITLE") == true)
            {
                Application.UserAppDataRegistry.SetValue("HOSP_ABOUT_TITLE", _strings["HOSP_ABOUT_TITLE"].ToString());
            }


            if (_strings.ContainsKey("HOSP_ABOUT_COPY") == true)
            {
                Application.UserAppDataRegistry.SetValue("HOSP_ABOUT_COPY", _strings["HOSP_ABOUT_COPY"].ToString());
            }


            if (_strings.ContainsKey("HOSP_ABOUT_HTML") == true)
            {
                Application.UserAppDataRegistry.SetValue("HOSP_ABOUT_HTML", _strings["HOSP_ABOUT_HTML"].ToString());
            }



            if (_strings.ContainsKey("cmdWaitTime") == true)
            {
                Application.UserAppDataRegistry.SetValue("cmdWaitTime", _strings["cmdWaitTime"].ToString());
            }




            if (_strings.ContainsKey("cmdAbout") == true)
            {
                Application.UserAppDataRegistry.SetValue("cmdAbout", _strings["cmdAbout"].ToString());
            }


            if (_strings.ContainsKey("cmdDisclaimer") == true)
            {
                Application.UserAppDataRegistry.SetValue("cmdDisclaimer", _strings["cmdDisclaimer"].ToString());
            }


            if (_strings.ContainsKey("Colors") == true)
            {
                Application.UserAppDataRegistry.SetValue("Colors", _strings["Colors"].ToString());
            }



            if (_strings.ContainsKey("POLL_TIME") == true)
            {
                Application.UserAppDataRegistry.SetValue("POLL_TIME", _strings["POLL_TIME"].ToString());
            }


            if (_strings.ContainsKey("MSGClearCode") == true)
            {
                Application.UserAppDataRegistry.SetValue("MSGClearCode", _strings["MSGClearCode"].ToString());
            }




        }


        private const string _Protocol = "dcs";
        private const string _ProtocolHandler = "url.dcs";

        private static readonly string _launch = string.Format("{0}{1}{0} {0}%1{0}", (char)34, Application.ExecutablePath);

        private static readonly Version _win8Version = new Version(6, 2, 9200, 0);

        private static readonly bool _isWin8 = Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version >= _win8Version;

        internal static void Register()
        {
            if (_isWin8) RegisterWin8();
            else RegisterWin7();
        }

        private static void RegisterWin7()
        {
            var regKey = Registry.ClassesRoot.CreateSubKey(_Protocol);

            regKey.CreateSubKey("DefaultIcon").SetValue(null, string.Format("{0}{1},1{0}", (char)34, Application.ExecutablePath));

            regKey.SetValue(null, "URL:dcs Protocol");
            regKey.SetValue("URL Protocol", "");

            regKey = regKey.CreateSubKey(@"shell\open\command");
            regKey.SetValue(null, _launch);
        }

        private static void RegisterWin8()
        {
            RegisterWin7();

            var regKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Classes").CreateSubKey(_ProtocolHandler);

            regKey.SetValue(null, _Protocol);

            regKey.CreateSubKey("DefaultIcon").SetValue(null, string.Format("{0}{1},1{0}", (char)34, Application.ExecutablePath));

            regKey.CreateSubKey(@"shell\open\command").SetValue(null, _launch);

            Registry.LocalMachine.CreateSubKey(string.Format(@"SOFTWARE\{0}\{1}\Capabilities\ApplicationDescription\URLAssociations",
                Application.CompanyName, Application.ProductName)).SetValue(_Protocol, _ProtocolHandler);

            Registry.LocalMachine.CreateSubKey(@"SOFTWARE\RegisteredApplications").SetValue(Application.ProductName, string.Format(@"SOFTWARE\{0}\Capabilities", Application.ProductName));
        }

        internal static void Unregister()
        {
            if (!_isWin8)
            {
                Registry.ClassesRoot.DeleteSubKeyTree("dcs", false);
                return;
            }

            Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Classes").DeleteSubKeyTree(_ProtocolHandler, false);
            Registry.LocalMachine.DeleteSubKeyTree(string.Format(@"SOFTWARE\{0}\{1}", Application.CompanyName, Application.ProductName));
            Registry.LocalMachine.CreateSubKey(@"SOFTWARE\RegisteredApplications").DeleteValue(Application.ProductName);
        }
    }
}


