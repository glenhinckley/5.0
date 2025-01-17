﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Manual_test_app.LDAP {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://dcsglobal.com/", ConfigurationName="LDAP.LDAPSoap")]
    public interface LDAPSoap {
        
        // CODEGEN: Generating message contract since element name strUserName from namespace http://dcsglobal.com/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://dcsglobal.com/AuthenticateAD_Full", ReplyAction="*")]
        Manual_test_app.LDAP.AuthenticateAD_FullResponse AuthenticateAD_Full(Manual_test_app.LDAP.AuthenticateAD_FullRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://dcsglobal.com/AuthenticateAD_Full", ReplyAction="*")]
        System.Threading.Tasks.Task<Manual_test_app.LDAP.AuthenticateAD_FullResponse> AuthenticateAD_FullAsync(Manual_test_app.LDAP.AuthenticateAD_FullRequest request);
        
        // CODEGEN: Generating message contract since element name strUserName from namespace http://dcsglobal.com/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://dcsglobal.com/AuthenticateAD", ReplyAction="*")]
        Manual_test_app.LDAP.AuthenticateADResponse AuthenticateAD(Manual_test_app.LDAP.AuthenticateADRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://dcsglobal.com/AuthenticateAD", ReplyAction="*")]
        System.Threading.Tasks.Task<Manual_test_app.LDAP.AuthenticateADResponse> AuthenticateADAsync(Manual_test_app.LDAP.AuthenticateADRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class AuthenticateAD_FullRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="AuthenticateAD_Full", Namespace="http://dcsglobal.com/", Order=0)]
        public Manual_test_app.LDAP.AuthenticateAD_FullRequestBody Body;
        
        public AuthenticateAD_FullRequest() {
        }
        
        public AuthenticateAD_FullRequest(Manual_test_app.LDAP.AuthenticateAD_FullRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://dcsglobal.com/")]
    public partial class AuthenticateAD_FullRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string strUserName;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string strPassword;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string EndPoint;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string port;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string ClientName;
        
        public AuthenticateAD_FullRequestBody() {
        }
        
        public AuthenticateAD_FullRequestBody(string strUserName, string strPassword, string EndPoint, string port, string ClientName) {
            this.strUserName = strUserName;
            this.strPassword = strPassword;
            this.EndPoint = EndPoint;
            this.port = port;
            this.ClientName = ClientName;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class AuthenticateAD_FullResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="AuthenticateAD_FullResponse", Namespace="http://dcsglobal.com/", Order=0)]
        public Manual_test_app.LDAP.AuthenticateAD_FullResponseBody Body;
        
        public AuthenticateAD_FullResponse() {
        }
        
        public AuthenticateAD_FullResponse(Manual_test_app.LDAP.AuthenticateAD_FullResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://dcsglobal.com/")]
    public partial class AuthenticateAD_FullResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string AuthenticateAD_FullResult;
        
        public AuthenticateAD_FullResponseBody() {
        }
        
        public AuthenticateAD_FullResponseBody(string AuthenticateAD_FullResult) {
            this.AuthenticateAD_FullResult = AuthenticateAD_FullResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class AuthenticateADRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="AuthenticateAD", Namespace="http://dcsglobal.com/", Order=0)]
        public Manual_test_app.LDAP.AuthenticateADRequestBody Body;
        
        public AuthenticateADRequest() {
        }
        
        public AuthenticateADRequest(Manual_test_app.LDAP.AuthenticateADRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://dcsglobal.com/")]
    public partial class AuthenticateADRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string strUserName;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string strPassword;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string ClientName;
        
        public AuthenticateADRequestBody() {
        }
        
        public AuthenticateADRequestBody(string strUserName, string strPassword, string ClientName) {
            this.strUserName = strUserName;
            this.strPassword = strPassword;
            this.ClientName = ClientName;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class AuthenticateADResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="AuthenticateADResponse", Namespace="http://dcsglobal.com/", Order=0)]
        public Manual_test_app.LDAP.AuthenticateADResponseBody Body;
        
        public AuthenticateADResponse() {
        }
        
        public AuthenticateADResponse(Manual_test_app.LDAP.AuthenticateADResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://dcsglobal.com/")]
    public partial class AuthenticateADResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string AuthenticateADResult;
        
        public AuthenticateADResponseBody() {
        }
        
        public AuthenticateADResponseBody(string AuthenticateADResult) {
            this.AuthenticateADResult = AuthenticateADResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface LDAPSoapChannel : Manual_test_app.LDAP.LDAPSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class LDAPSoapClient : System.ServiceModel.ClientBase<Manual_test_app.LDAP.LDAPSoap>, Manual_test_app.LDAP.LDAPSoap {
        
        public LDAPSoapClient() {
        }
        
        public LDAPSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public LDAPSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LDAPSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LDAPSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Manual_test_app.LDAP.AuthenticateAD_FullResponse Manual_test_app.LDAP.LDAPSoap.AuthenticateAD_Full(Manual_test_app.LDAP.AuthenticateAD_FullRequest request) {
            return base.Channel.AuthenticateAD_Full(request);
        }
        
        public string AuthenticateAD_Full(string strUserName, string strPassword, string EndPoint, string port, string ClientName) {
            Manual_test_app.LDAP.AuthenticateAD_FullRequest inValue = new Manual_test_app.LDAP.AuthenticateAD_FullRequest();
            inValue.Body = new Manual_test_app.LDAP.AuthenticateAD_FullRequestBody();
            inValue.Body.strUserName = strUserName;
            inValue.Body.strPassword = strPassword;
            inValue.Body.EndPoint = EndPoint;
            inValue.Body.port = port;
            inValue.Body.ClientName = ClientName;
            Manual_test_app.LDAP.AuthenticateAD_FullResponse retVal = ((Manual_test_app.LDAP.LDAPSoap)(this)).AuthenticateAD_Full(inValue);
            return retVal.Body.AuthenticateAD_FullResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<Manual_test_app.LDAP.AuthenticateAD_FullResponse> Manual_test_app.LDAP.LDAPSoap.AuthenticateAD_FullAsync(Manual_test_app.LDAP.AuthenticateAD_FullRequest request) {
            return base.Channel.AuthenticateAD_FullAsync(request);
        }
        
        public System.Threading.Tasks.Task<Manual_test_app.LDAP.AuthenticateAD_FullResponse> AuthenticateAD_FullAsync(string strUserName, string strPassword, string EndPoint, string port, string ClientName) {
            Manual_test_app.LDAP.AuthenticateAD_FullRequest inValue = new Manual_test_app.LDAP.AuthenticateAD_FullRequest();
            inValue.Body = new Manual_test_app.LDAP.AuthenticateAD_FullRequestBody();
            inValue.Body.strUserName = strUserName;
            inValue.Body.strPassword = strPassword;
            inValue.Body.EndPoint = EndPoint;
            inValue.Body.port = port;
            inValue.Body.ClientName = ClientName;
            return ((Manual_test_app.LDAP.LDAPSoap)(this)).AuthenticateAD_FullAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Manual_test_app.LDAP.AuthenticateADResponse Manual_test_app.LDAP.LDAPSoap.AuthenticateAD(Manual_test_app.LDAP.AuthenticateADRequest request) {
            return base.Channel.AuthenticateAD(request);
        }
        
        public string AuthenticateAD(string strUserName, string strPassword, string ClientName) {
            Manual_test_app.LDAP.AuthenticateADRequest inValue = new Manual_test_app.LDAP.AuthenticateADRequest();
            inValue.Body = new Manual_test_app.LDAP.AuthenticateADRequestBody();
            inValue.Body.strUserName = strUserName;
            inValue.Body.strPassword = strPassword;
            inValue.Body.ClientName = ClientName;
            Manual_test_app.LDAP.AuthenticateADResponse retVal = ((Manual_test_app.LDAP.LDAPSoap)(this)).AuthenticateAD(inValue);
            return retVal.Body.AuthenticateADResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<Manual_test_app.LDAP.AuthenticateADResponse> Manual_test_app.LDAP.LDAPSoap.AuthenticateADAsync(Manual_test_app.LDAP.AuthenticateADRequest request) {
            return base.Channel.AuthenticateADAsync(request);
        }
        
        public System.Threading.Tasks.Task<Manual_test_app.LDAP.AuthenticateADResponse> AuthenticateADAsync(string strUserName, string strPassword, string ClientName) {
            Manual_test_app.LDAP.AuthenticateADRequest inValue = new Manual_test_app.LDAP.AuthenticateADRequest();
            inValue.Body = new Manual_test_app.LDAP.AuthenticateADRequestBody();
            inValue.Body.strUserName = strUserName;
            inValue.Body.strPassword = strPassword;
            inValue.Body.ClientName = ClientName;
            return ((Manual_test_app.LDAP.LDAPSoap)(this)).AuthenticateADAsync(inValue);
        }
    }
}
