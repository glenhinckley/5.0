using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Data;
using System.Data.OleDb;
using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using DCSGlobal.BusinessRules.Logging;







namespace DCSGlobal.BusinessRules.Logging
{

    public class Email : IDisposable
    {


        StringStuff objSS = new StringStuff();
        logExecption log = new logExecption();


        string sConnectionString = "";
        private string _Server;
        private string _ServerAccount;
        private string _ServerPassword;
        private string _SendFrom;
        private string _SendTo;

        public string ConnectionString
        {

            set
            {

                sConnectionString = value;
                log.ConnectionString = value;
            }
        }



                ~Email()
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

        public string SendTo
        {
            get
            {
                return _SendTo;
            }
            set
            {

                _SendTo = value;
            }
        }

        public string SendFrom
        {
            get
            {
                return _SendFrom;
            }
            set
            {

                _SendFrom = value;
            }
        }

        public string Server
        {
            get
            {
                return _Server;
            }
            set
            {

                _Server = value;
            }
        }

        public string ServerAccount
        {
            get
            {
                return _ServerAccount;
            }
            set
            {
                _ServerAccount = value;
            }
        }

        public string ServerPassword
        {
            get
            {
                return _ServerPassword;
            }
            set
            {
                _ServerPassword = value;

            }
        }

       
        
        public void QASupportNotification(string _MSG, string _title, string _ProgramName)
        {



            try
            {
                string msg;
        

                MailMessage message = new MailMessage();
                message.IsBodyHtml = true;
                message.From = new MailAddress(_SendFrom );
                                                                                                               
                message.Subject = _ProgramName + ": " + _title;
                message.To.Add(new MailAddress(_SendTo ));


                msg = "<p>Time sent " + Convert.ToString(DateTime.Now) + "</p>";

                message.Body = msg + _MSG;
                SmtpClient client = new SmtpClient(_Server);

               // client.Port = 50000;
              //  client.Credentials = new System.Net.NetworkCredential(_ServerAccount, _ServerPassword );
                //client.EnableSsl = false;




                client.Send(message);
                log.ExceptionDetails(_ProgramName + ": Send Email", _MSG);
            }
            catch (Exception ex)
            {

                log.ExceptionDetails(_ProgramName + ": Send Email Failure", ex);
                log.ExceptionDetails(_ProgramName + ": Send Email Orginal MSG", _MSG);
            }
        }







        public void SendGenericEmail(string _MSG, string _title, string _ProgramName)
        {



            try
            {
                string msg;


                MailMessage message = new MailMessage();
                message.IsBodyHtml = true;
                message.From = new MailAddress(_SendFrom);

                message.Subject = _ProgramName + ": " + _title;
                message.To.Add(new MailAddress(_SendTo));


                msg = "<p>Time sent " + Convert.ToString(DateTime.Now) + "</p>";

                message.Body = msg + _MSG;
                SmtpClient client = new SmtpClient(_Server);

                // client.Port = 50000;
                //  client.Credentials = new System.Net.NetworkCredential(_ServerAccount, _ServerPassword );
                //client.EnableSsl = false;




                client.Send(message);
                log.ExceptionDetails(_ProgramName + ": Send Email", _MSG);
            }
            catch (Exception ex)
            {

                log.ExceptionDetails(_ProgramName + ": Send Email Failure", ex);
                log.ExceptionDetails(_ProgramName + ": Send Email Orginal MSG", _MSG);
            }
        }


   
   }




}
