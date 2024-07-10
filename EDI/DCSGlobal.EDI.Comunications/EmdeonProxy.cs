 

namespace EmdeonPxy
{
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.57.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "AWSSoap", Namespace = "https://ra.emdeon.com/AstWebService/")]
    public partial class AWS : System.Web.Services.Protocols.SoapHttpClientProtocol
    {

        private System.Threading.SendOrPostCallback RunTransactionOperationCompleted;

        private bool useDefaultCredentialsSetExplicitly;

        /// <remarks/>
        public AWS()
        {
            //this.Url = global::DCSGlobal.EDI.Comunications.Properties.Settings.Default.DCSGlobal_EDI_Comunications_com_emdeon_ra_AWS;
            if ((this.IsLocalFileSystemWebService(this.Url) == true))
            {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else
            {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }

        public new string Url
        {
            get
            {
                return base.Url;
            }
            set
            {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true)
                            && (this.useDefaultCredentialsSetExplicitly == false))
                            && (this.IsLocalFileSystemWebService(value) == false)))
                {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }

        public new bool UseDefaultCredentials
        {
            get
            {
                return base.UseDefaultCredentials;
            }
            set
            {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }

        /// <remarks/>
        public event RunTransactionCompletedEventHandler RunTransactionCompleted;

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("https://ra.emdeon.com/AstWebService/RunTransaction", RequestNamespace = "https://ra.emdeon.com/AstWebService/", ResponseNamespace = "https://ra.emdeon.com/AstWebService/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary")]
        public byte[] RunTransaction(string UserName, string Password, [System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary")] byte[] Request)
        {
            object[] results = this.Invoke("RunTransaction", new object[] {
                        UserName,
                        Password,
                        Request});
            return ((byte[])(results[0]));
        }

        /// <remarks/>
        public void RunTransactionAsync(string UserName, string Password, byte[] Request)
        {
            this.RunTransactionAsync(UserName, Password, Request, null);
        }

        /// <remarks/>
        public void RunTransactionAsync(string UserName, string Password, byte[] Request, object userState)
        {
            if ((this.RunTransactionOperationCompleted == null))
            {
                this.RunTransactionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRunTransactionOperationCompleted);
            }
            this.InvokeAsync("RunTransaction", new object[] {
                        UserName,
                        Password,
                        Request}, this.RunTransactionOperationCompleted, userState);
        }

        private void OnRunTransactionOperationCompleted(object arg)
        {
            if ((this.RunTransactionCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.RunTransactionCompleted(this, new RunTransactionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        public new void CancelAsync(object userState)
        {
            base.CancelAsync(userState);
        }

        private bool IsLocalFileSystemWebService(string url)
        {
            if (((url == null)
                        || (url == string.Empty)))
            {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024)
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0)))
            {
                return true;
            }
            return false;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.57.0")]
    public delegate void RunTransactionCompletedEventHandler(object sender, RunTransactionCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.57.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class RunTransactionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal RunTransactionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public byte[] Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((byte[])(this.results[0]));
            }
        }
    }
}

 