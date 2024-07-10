﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.34209.
// 
#pragma warning disable 1591

namespace IvansPxy
{
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "EligibilityOneSoap", Namespace = "http://tempuri.org/")]
    public partial class EligibilityOne : System.Web.Services.Protocols.SoapHttpClientProtocol
    {

        private IvansWSAuthentication ivansWSAuthenticationValueField;

        private System.Threading.SendOrPostCallback SendCommercialEligibilityRequestOperationCompleted;

        private System.Threading.SendOrPostCallback SendCommercialEligibilityRequestWithReferenceIDOperationCompleted;

        private System.Threading.SendOrPostCallback SendCommercialEligibilityFormRequestOperationCompleted;

        private System.Threading.SendOrPostCallback SendCommercialEligibilityFormRequestWithReferenceIDOperationCompleted;

        private IvansCOREWSAuthentication ivansCOREWSAuthenticationValueField;

        private System.Threading.SendOrPostCallback SendEligibilityFormRequestOperationCompleted;

        private System.Threading.SendOrPostCallback SendEligibilityRequestOperationCompleted;

        private bool useDefaultCredentialsSetExplicitly;

        /// <remarks/>
        public EligibilityOne()
        {
            //this.Url = global::DCSGlobal.EDI.Comunications.Properties.Settings.Default.DCSGlobal_Eligibility_Comunications_com_ivans_limeservices_EligibilityOne;
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

        public IvansWSAuthentication IvansWSAuthenticationValue
        {
            get
            {
                return this.ivansWSAuthenticationValueField;
            }
            set
            {
                this.ivansWSAuthenticationValueField = value;
            }
        }

        public IvansCOREWSAuthentication IvansCOREWSAuthenticationValue
        {
            get
            {
                return this.ivansCOREWSAuthenticationValueField;
            }
            set
            {
                this.ivansCOREWSAuthenticationValueField = value;
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
        public event SendCommercialEligibilityRequestCompletedEventHandler SendCommercialEligibilityRequestCompleted;

        /// <remarks/>
        public event SendCommercialEligibilityRequestWithReferenceIDCompletedEventHandler SendCommercialEligibilityRequestWithReferenceIDCompleted;

        /// <remarks/>
        public event SendCommercialEligibilityFormRequestCompletedEventHandler SendCommercialEligibilityFormRequestCompleted;

        /// <remarks/>
        public event SendCommercialEligibilityFormRequestWithReferenceIDCompletedEventHandler SendCommercialEligibilityFormRequestWithReferenceIDCompleted;

        /// <remarks/>
        public event SendEligibilityFormRequestCompletedEventHandler SendEligibilityFormRequestCompleted;

        /// <remarks/>
        public event SendEligibilityRequestCompletedEventHandler SendEligibilityRequestCompleted;

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("IvansWSAuthenticationValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/SendCommercialEligibilityRequest", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string SendCommercialEligibilityRequest(Form270RealTimeData data)
        {
            object[] results = this.Invoke("SendCommercialEligibilityRequest", new object[] {
                        data});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void SendCommercialEligibilityRequestAsync(Form270RealTimeData data)
        {
            this.SendCommercialEligibilityRequestAsync(data, null);
        }

        /// <remarks/>
        public void SendCommercialEligibilityRequestAsync(Form270RealTimeData data, object userState)
        {
            if ((this.SendCommercialEligibilityRequestOperationCompleted == null))
            {
                this.SendCommercialEligibilityRequestOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSendCommercialEligibilityRequestOperationCompleted);
            }
            this.InvokeAsync("SendCommercialEligibilityRequest", new object[] {
                        data}, this.SendCommercialEligibilityRequestOperationCompleted, userState);
        }

        private void OnSendCommercialEligibilityRequestOperationCompleted(object arg)
        {
            if ((this.SendCommercialEligibilityRequestCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SendCommercialEligibilityRequestCompleted(this, new SendCommercialEligibilityRequestCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("IvansWSAuthenticationValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/SendCommercialEligibilityRequestWithReferenceID", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string SendCommercialEligibilityRequestWithReferenceID(string referenceID, Form270RealTimeData data)
        {
            object[] results = this.Invoke("SendCommercialEligibilityRequestWithReferenceID", new object[] {
                        referenceID,
                        data});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void SendCommercialEligibilityRequestWithReferenceIDAsync(string referenceID, Form270RealTimeData data)
        {
            this.SendCommercialEligibilityRequestWithReferenceIDAsync(referenceID, data, null);
        }

        /// <remarks/>
        public void SendCommercialEligibilityRequestWithReferenceIDAsync(string referenceID, Form270RealTimeData data, object userState)
        {
            if ((this.SendCommercialEligibilityRequestWithReferenceIDOperationCompleted == null))
            {
                this.SendCommercialEligibilityRequestWithReferenceIDOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSendCommercialEligibilityRequestWithReferenceIDOperationCompleted);
            }
            this.InvokeAsync("SendCommercialEligibilityRequestWithReferenceID", new object[] {
                        referenceID,
                        data}, this.SendCommercialEligibilityRequestWithReferenceIDOperationCompleted, userState);
        }

        private void OnSendCommercialEligibilityRequestWithReferenceIDOperationCompleted(object arg)
        {
            if ((this.SendCommercialEligibilityRequestWithReferenceIDCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SendCommercialEligibilityRequestWithReferenceIDCompleted(this, new SendCommercialEligibilityRequestWithReferenceIDCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("IvansWSAuthenticationValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/SendCommercialEligibilityFormRequest", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string SendCommercialEligibilityFormRequest(string form270)
        {
            object[] results = this.Invoke("SendCommercialEligibilityFormRequest", new object[] {
                        form270});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void SendCommercialEligibilityFormRequestAsync(string form270)
        {
            this.SendCommercialEligibilityFormRequestAsync(form270, null);
        }

        /// <remarks/>
        public void SendCommercialEligibilityFormRequestAsync(string form270, object userState)
        {
            if ((this.SendCommercialEligibilityFormRequestOperationCompleted == null))
            {
                this.SendCommercialEligibilityFormRequestOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSendCommercialEligibilityFormRequestOperationCompleted);
            }
            this.InvokeAsync("SendCommercialEligibilityFormRequest", new object[] {
                        form270}, this.SendCommercialEligibilityFormRequestOperationCompleted, userState);
        }

        private void OnSendCommercialEligibilityFormRequestOperationCompleted(object arg)
        {
            if ((this.SendCommercialEligibilityFormRequestCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SendCommercialEligibilityFormRequestCompleted(this, new SendCommercialEligibilityFormRequestCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("IvansWSAuthenticationValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/SendCommercialEligibilityFormRequestWithReferenceID", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string SendCommercialEligibilityFormRequestWithReferenceID(string referenceID, string form270)
        {
            object[] results = this.Invoke("SendCommercialEligibilityFormRequestWithReferenceID", new object[] {
                        referenceID,
                        form270});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void SendCommercialEligibilityFormRequestWithReferenceIDAsync(string referenceID, string form270)
        {
            this.SendCommercialEligibilityFormRequestWithReferenceIDAsync(referenceID, form270, null);
        }

        /// <remarks/>
        public void SendCommercialEligibilityFormRequestWithReferenceIDAsync(string referenceID, string form270, object userState)
        {
            if ((this.SendCommercialEligibilityFormRequestWithReferenceIDOperationCompleted == null))
            {
                this.SendCommercialEligibilityFormRequestWithReferenceIDOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSendCommercialEligibilityFormRequestWithReferenceIDOperationCompleted);
            }
            this.InvokeAsync("SendCommercialEligibilityFormRequestWithReferenceID", new object[] {
                        referenceID,
                        form270}, this.SendCommercialEligibilityFormRequestWithReferenceIDOperationCompleted, userState);
        }

        private void OnSendCommercialEligibilityFormRequestWithReferenceIDOperationCompleted(object arg)
        {
            if ((this.SendCommercialEligibilityFormRequestWithReferenceIDCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SendCommercialEligibilityFormRequestWithReferenceIDCompleted(this, new SendCommercialEligibilityFormRequestWithReferenceIDCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("IvansCOREWSAuthenticationValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/SendEligibilityFormRequest", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string SendEligibilityFormRequest(string form270, string referenceID)
        {
            object[] results = this.Invoke("SendEligibilityFormRequest", new object[] {
                        form270,
                        referenceID});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void SendEligibilityFormRequestAsync(string form270, string referenceID)
        {
            this.SendEligibilityFormRequestAsync(form270, referenceID, null);
        }

        /// <remarks/>
        public void SendEligibilityFormRequestAsync(string form270, string referenceID, object userState)
        {
            if ((this.SendEligibilityFormRequestOperationCompleted == null))
            {
                this.SendEligibilityFormRequestOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSendEligibilityFormRequestOperationCompleted);
            }
            this.InvokeAsync("SendEligibilityFormRequest", new object[] {
                        form270,
                        referenceID}, this.SendEligibilityFormRequestOperationCompleted, userState);
        }

        private void OnSendEligibilityFormRequestOperationCompleted(object arg)
        {
            if ((this.SendEligibilityFormRequestCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SendEligibilityFormRequestCompleted(this, new SendEligibilityFormRequestCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("IvansCOREWSAuthenticationValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/SendEligibilityRequest", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string SendEligibilityRequest(Form270RealTimeData data, string referenceID)
        {
            object[] results = this.Invoke("SendEligibilityRequest", new object[] {
                        data,
                        referenceID});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void SendEligibilityRequestAsync(Form270RealTimeData data, string referenceID)
        {
            this.SendEligibilityRequestAsync(data, referenceID, null);
        }

        /// <remarks/>
        public void SendEligibilityRequestAsync(Form270RealTimeData data, string referenceID, object userState)
        {
            if ((this.SendEligibilityRequestOperationCompleted == null))
            {
                this.SendEligibilityRequestOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSendEligibilityRequestOperationCompleted);
            }
            this.InvokeAsync("SendEligibilityRequest", new object[] {
                        data,
                        referenceID}, this.SendEligibilityRequestOperationCompleted, userState);
        }

        private void OnSendEligibilityRequestOperationCompleted(object arg)
        {
            if ((this.SendEligibilityRequestCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SendEligibilityRequestCompleted(this, new SendEligibilityRequestCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IvansCOREWSAuthentication))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34209")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://tempuri.org/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://tempuri.org/", IsNullable = false)]
    public partial class IvansWSAuthentication : System.Web.Services.Protocols.SoapHeader
    {

        private string userField;

        private string passwordField;

        private string clientIdField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        public string User
        {
            get
            {
                return this.userField;
            }
            set
            {
                this.userField = value;
            }
        }

        /// <remarks/>
        public string Password
        {
            get
            {
                return this.passwordField;
            }
            set
            {
                this.passwordField = value;
            }
        }

        /// <remarks/>
        public string ClientId
        {
            get
            {
                return this.clientIdField;
            }
            set
            {
                this.clientIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34209")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://tempuri.org/")]
    public partial class Form270RealTimeData
    {

        private string ivansPayerIDField;

        private string memberIDField;

        private string firstNameField;

        private string lastNameField;

        private string dateOfBirth_yyyyMMddField;

        private string genderField;

        private string sSNField;

        private string providerIDField;

        private string providerNameField;

        private string providerFirstNameField;

        private string providerMiddleNameField;

        private string providerLastNameField;

        private string providerPrefixField;

        private string providerSuffixField;

        private string dateOfService_Start_yyyyMMddField;

        private string dateOfService_End_yyyyMMddField;

        private string payerPINField;

        private string submitterIDField;

        private string receiverIDField;

        private string serviceTypeField;

        private string relationshipToSubscriberCodeField;

        /// <remarks/>
        public string IvansPayerID
        {
            get
            {
                return this.ivansPayerIDField;
            }
            set
            {
                this.ivansPayerIDField = value;
            }
        }

        /// <remarks/>
        public string MemberID
        {
            get
            {
                return this.memberIDField;
            }
            set
            {
                this.memberIDField = value;
            }
        }

        /// <remarks/>
        public string FirstName
        {
            get
            {
                return this.firstNameField;
            }
            set
            {
                this.firstNameField = value;
            }
        }

        /// <remarks/>
        public string LastName
        {
            get
            {
                return this.lastNameField;
            }
            set
            {
                this.lastNameField = value;
            }
        }

        /// <remarks/>
        public string DateOfBirth_yyyyMMdd
        {
            get
            {
                return this.dateOfBirth_yyyyMMddField;
            }
            set
            {
                this.dateOfBirth_yyyyMMddField = value;
            }
        }

        /// <remarks/>
        public string Gender
        {
            get
            {
                return this.genderField;
            }
            set
            {
                this.genderField = value;
            }
        }

        /// <remarks/>
        public string SSN
        {
            get
            {
                return this.sSNField;
            }
            set
            {
                this.sSNField = value;
            }
        }

        /// <remarks/>
        public string ProviderID
        {
            get
            {
                return this.providerIDField;
            }
            set
            {
                this.providerIDField = value;
            }
        }

        /// <remarks/>
        public string ProviderName
        {
            get
            {
                return this.providerNameField;
            }
            set
            {
                this.providerNameField = value;
            }
        }

        /// <remarks/>
        public string ProviderFirstName
        {
            get
            {
                return this.providerFirstNameField;
            }
            set
            {
                this.providerFirstNameField = value;
            }
        }

        /// <remarks/>
        public string ProviderMiddleName
        {
            get
            {
                return this.providerMiddleNameField;
            }
            set
            {
                this.providerMiddleNameField = value;
            }
        }

        /// <remarks/>
        public string ProviderLastName
        {
            get
            {
                return this.providerLastNameField;
            }
            set
            {
                this.providerLastNameField = value;
            }
        }

        /// <remarks/>
        public string ProviderPrefix
        {
            get
            {
                return this.providerPrefixField;
            }
            set
            {
                this.providerPrefixField = value;
            }
        }

        /// <remarks/>
        public string ProviderSuffix
        {
            get
            {
                return this.providerSuffixField;
            }
            set
            {
                this.providerSuffixField = value;
            }
        }

        /// <remarks/>
        public string DateOfService_Start_yyyyMMdd
        {
            get
            {
                return this.dateOfService_Start_yyyyMMddField;
            }
            set
            {
                this.dateOfService_Start_yyyyMMddField = value;
            }
        }

        /// <remarks/>
        public string DateOfService_End_yyyyMMdd
        {
            get
            {
                return this.dateOfService_End_yyyyMMddField;
            }
            set
            {
                this.dateOfService_End_yyyyMMddField = value;
            }
        }

        /// <remarks/>
        public string PayerPIN
        {
            get
            {
                return this.payerPINField;
            }
            set
            {
                this.payerPINField = value;
            }
        }

        /// <remarks/>
        public string SubmitterID
        {
            get
            {
                return this.submitterIDField;
            }
            set
            {
                this.submitterIDField = value;
            }
        }

        /// <remarks/>
        public string ReceiverID
        {
            get
            {
                return this.receiverIDField;
            }
            set
            {
                this.receiverIDField = value;
            }
        }

        /// <remarks/>
        public string ServiceType
        {
            get
            {
                return this.serviceTypeField;
            }
            set
            {
                this.serviceTypeField = value;
            }
        }

        /// <remarks/>
        public string RelationshipToSubscriberCode
        {
            get
            {
                return this.relationshipToSubscriberCodeField;
            }
            set
            {
                this.relationshipToSubscriberCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34209")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://tempuri.org/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://tempuri.org/", IsNullable = false)]
    public partial class IvansCOREWSAuthentication : IvansWSAuthentication
    {

        private string uTCDateField;

        /// <remarks/>
        public string UTCDate
        {
            get
            {
                return this.uTCDateField;
            }
            set
            {
                this.uTCDateField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    public delegate void SendCommercialEligibilityRequestCompletedEventHandler(object sender, SendCommercialEligibilityRequestCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SendCommercialEligibilityRequestCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal SendCommercialEligibilityRequestCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    public delegate void SendCommercialEligibilityRequestWithReferenceIDCompletedEventHandler(object sender, SendCommercialEligibilityRequestWithReferenceIDCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SendCommercialEligibilityRequestWithReferenceIDCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal SendCommercialEligibilityRequestWithReferenceIDCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    public delegate void SendCommercialEligibilityFormRequestCompletedEventHandler(object sender, SendCommercialEligibilityFormRequestCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SendCommercialEligibilityFormRequestCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal SendCommercialEligibilityFormRequestCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    public delegate void SendCommercialEligibilityFormRequestWithReferenceIDCompletedEventHandler(object sender, SendCommercialEligibilityFormRequestWithReferenceIDCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SendCommercialEligibilityFormRequestWithReferenceIDCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal SendCommercialEligibilityFormRequestWithReferenceIDCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    public delegate void SendEligibilityFormRequestCompletedEventHandler(object sender, SendEligibilityFormRequestCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SendEligibilityFormRequestCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal SendEligibilityFormRequestCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    public delegate void SendEligibilityRequestCompletedEventHandler(object sender, SendEligibilityRequestCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SendEligibilityRequestCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal SendEligibilityRequestCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591