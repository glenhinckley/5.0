
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
//Imports Microsoft.VisualBasic
using System.Security.Cryptography.X509Certificates;
using System.Configuration;
using System.Net.Security;
using System.Web;
using System.Net;
using System.IO;

using DCSGlobal.BusinessRules.Logging;



namespace DCSGlobal.EDI.Comunications
{

    public class HTTPProcess : IDisposable
    {
        public string SESHost;
        public Int32 SESPort;
        public string ProxyHost;
        public Int32 ProxyPort;
        public string UserName;
        public string Password;
        public string Connection;
        public bool UseClientCert;
        private string[] HttpProps = {
		"POST",
		"application",
		"octet-stream",
		"A null HTTP response object was returned.",
		"Neutralus"

	};
        logExecption log = new logExecption();

        string appName = "ProcessEligilbilty class libary v13 HTTPProcess";

        ~HTTPProcess()
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

        public HTTPProcess(string sSESHost, Int32 nSESPort, string sProxyServer, Int32 nProxyPort, string sUserName, string sPassword, string sConnection)
        {
            SESHost = sSESHost;
            SESPort = nSESPort;
            ProxyHost = sProxyServer;
            ProxyPort = nProxyPort;
            UserName = sUserName;
            Password = sPassword;
            Connection = sConnection;
            UseClientCert = false;
        }

        public ResponseData Process(string s270Contents)
        {
            //ErrorLogger.WriteEvent("HttpProcess-ProcessMethod-BEGIN")
            ResponseData oRetVal = new ResponseData();
            oRetVal.HttpResponse = 200;


            string sURL = string.Format("https://{0}:{1}/ses/upload?username={2}&password={3}&connection={4}", SESHost, SESPort, HttpUtility.UrlEncode(UserName), HttpUtility.UrlEncode(Password), HttpUtility.UrlEncode(Connection));
            StreamWriter sw = null;
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(sURL);
            objRequest.KeepAlive = true;
            //2012-feb-01



            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(OnValidateServerCertificate);
                //ServicePointManager.ServerCertificateValidationCallback = New net.Security.RemoteCertificateValidationCallback(AddressOf OnValidateServerCertificate)
                var _with1 = objRequest;
                _with1.Method = HttpProps[0];
                _with1.ServicePoint.Expect100Continue = false;
                _with1.ContentLength = s270Contents.Length;
                _with1.ContentType = string.Format("{0}/{1}", HttpProps[1], HttpProps[2]);

                if (UseClientCert)
                {
                    X509Certificate2 cert = SelectLocalCertificate();
                    if (cert != null)
                    {
                        objRequest.ClientCertificates.Add(cert);
                    }
                }

                if (!string.IsNullOrEmpty(ProxyHost))
                {
                    objRequest.Proxy = new WebProxy(ProxyHost, ProxyPort);
                }

                sw = new StreamWriter(objRequest.GetRequestStream());
                sw.Write(s270Contents);

            }
            catch (System.Exception ex)
            {
                //errLog.WriteEvent(HttpPostException.Message) 

                log.ExceptionDetails(appName + " Process", ex);
                oRetVal.Content = ex.Message.ToString();
                //Throw New System.Exception(HttpPostException.Message, HttpPostException.InnerException)
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
            }

            HttpWebResponse objResponse = null;
            StreamReader sr = null;





            try
            {
                objResponse = (HttpWebResponse)objRequest.GetResponse();
                if (objResponse != null)
                {
                    sr = new StreamReader(objResponse.GetResponseStream());
                    oRetVal.Content = sr.ReadToEnd();

                }
                else
                {
                    log.ExceptionDetails(appName + " Process objResponse", HttpProps[3]);
                    oRetVal.Content = HttpProps[3];
                    //throw new System.Exception(HttpProps[3]);
                }

            }
            catch (System.Exception ex)
            {
                //errLog.WriteEvent(ProtocolsSoapException.Message)



                log.ExceptionDetails(appName + " Process objResponse", ex);
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
            }






            if (objResponse != null)
            {
                try
                {
                    oRetVal.HttpResponse = Convert.ToInt32(objResponse.StatusCode);

                }
                catch (System.Exception ex)
                {


                    log.ExceptionDetails(appName + " Process objResponse", ex);


                    //  Throw New System.Exception(SystemExecutionEngineException.Message, SystemExecutionEngineException.InnerException)
                }
                finally
                {
                    objResponse.Close();
                }
            }












            return oRetVal;
        }

        public X509Certificate2 SelectLocalCertificate()
        {
            try
            {
                X509Store certStore = new X509Store(StoreName.My);
                certStore.Open(OpenFlags.OpenExistingOnly & OpenFlags.ReadOnly);
                X509Certificate2Collection clientCertificates = certStore.Certificates;
                foreach (X509Certificate2 oCert in clientCertificates)
                {
                    if (oCert.Issuer.Contains(HttpProps[4]))
                    {
                        return oCert;
                    }
                }


            }
            catch (System.Security.Authentication.AuthenticationException ae)
            {
                log.ExceptionDetails(appName + " SelectLocalCertificate", ae.Message);

            }


            return null;


        }

        public static bool OnValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

    }
}




