﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DCSGlobal.Medical.VoiceRecoder.srRegister {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ServiceData", Namespace="http://wsdl.dcsglobal.com/")]
    [System.SerializableAttribute()]
    public partial class ServiceData : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AuthField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false)]
        public string ID {
            get {
                return this.IDField;
            }
            set {
                if ((object.ReferenceEquals(this.IDField, value) != true)) {
                    this.IDField = value;
                    this.RaisePropertyChanged("ID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string Auth {
            get {
                return this.AuthField;
            }
            set {
                if ((object.ReferenceEquals(this.AuthField, value) != true)) {
                    this.AuthField = value;
                    this.RaisePropertyChanged("Auth");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://wsdl.dcsglobal.com/", ConfigurationName="srRegister.RegisterPager1Soap")]
    public interface RegisterPager1Soap {
        
        // CODEGEN: Generating message contract since element name TestConnectionResult from namespace http://wsdl.dcsglobal.com/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://wsdl.dcsglobal.com/TestConnection", ReplyAction="*")]
        DCSGlobal.Medical.VoiceRecoder.srRegister.TestConnectionResponse TestConnection(DCSGlobal.Medical.VoiceRecoder.srRegister.TestConnectionRequest request);
        
        // CODEGEN: Generating message contract since element name SD from namespace http://wsdl.dcsglobal.com/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://wsdl.dcsglobal.com/ActivationStatus", ReplyAction="*")]
        DCSGlobal.Medical.VoiceRecoder.srRegister.ActivationStatusResponse ActivationStatus(DCSGlobal.Medical.VoiceRecoder.srRegister.ActivationStatusRequest request);
        
        // CODEGEN: Generating message contract since element name PagerHardwareID from namespace http://wsdl.dcsglobal.com/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://wsdl.dcsglobal.com/RegisterPager", ReplyAction="*")]
        DCSGlobal.Medical.VoiceRecoder.srRegister.RegisterPagerResponse RegisterPager(DCSGlobal.Medical.VoiceRecoder.srRegister.RegisterPagerRequest request);
        
        // CODEGEN: Generating message contract since element name PagerHardwareID from namespace http://wsdl.dcsglobal.com/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://wsdl.dcsglobal.com/GetPagerID", ReplyAction="*")]
        DCSGlobal.Medical.VoiceRecoder.srRegister.GetPagerIDResponse GetPagerID(DCSGlobal.Medical.VoiceRecoder.srRegister.GetPagerIDRequest request);
        
        // CODEGEN: Generating message contract since element name PagerHardwareID from namespace http://wsdl.dcsglobal.com/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://wsdl.dcsglobal.com/ActivatePager", ReplyAction="*")]
        DCSGlobal.Medical.VoiceRecoder.srRegister.ActivatePagerResponse ActivatePager(DCSGlobal.Medical.VoiceRecoder.srRegister.ActivatePagerRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class TestConnectionRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="TestConnection", Namespace="http://wsdl.dcsglobal.com/", Order=0)]
        public DCSGlobal.Medical.VoiceRecoder.srRegister.TestConnectionRequestBody Body;
        
        public TestConnectionRequest() {
        }
        
        public TestConnectionRequest(DCSGlobal.Medical.VoiceRecoder.srRegister.TestConnectionRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class TestConnectionRequestBody {
        
        public TestConnectionRequestBody() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class TestConnectionResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="TestConnectionResponse", Namespace="http://wsdl.dcsglobal.com/", Order=0)]
        public DCSGlobal.Medical.VoiceRecoder.srRegister.TestConnectionResponseBody Body;
        
        public TestConnectionResponse() {
        }
        
        public TestConnectionResponse(DCSGlobal.Medical.VoiceRecoder.srRegister.TestConnectionResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://wsdl.dcsglobal.com/")]
    public partial class TestConnectionResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string TestConnectionResult;
        
        public TestConnectionResponseBody() {
        }
        
        public TestConnectionResponseBody(string TestConnectionResult) {
            this.TestConnectionResult = TestConnectionResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ActivationStatusRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ActivationStatus", Namespace="http://wsdl.dcsglobal.com/", Order=0)]
        public DCSGlobal.Medical.VoiceRecoder.srRegister.ActivationStatusRequestBody Body;
        
        public ActivationStatusRequest() {
        }
        
        public ActivationStatusRequest(DCSGlobal.Medical.VoiceRecoder.srRegister.ActivationStatusRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://wsdl.dcsglobal.com/")]
    public partial class ActivationStatusRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public int PagerKey;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public DCSGlobal.Medical.VoiceRecoder.srRegister.ServiceData SD;
        
        public ActivationStatusRequestBody() {
        }
        
        public ActivationStatusRequestBody(int PagerKey, DCSGlobal.Medical.VoiceRecoder.srRegister.ServiceData SD) {
            this.PagerKey = PagerKey;
            this.SD = SD;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ActivationStatusResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ActivationStatusResponse", Namespace="http://wsdl.dcsglobal.com/", Order=0)]
        public DCSGlobal.Medical.VoiceRecoder.srRegister.ActivationStatusResponseBody Body;
        
        public ActivationStatusResponse() {
        }
        
        public ActivationStatusResponse(DCSGlobal.Medical.VoiceRecoder.srRegister.ActivationStatusResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://wsdl.dcsglobal.com/")]
    public partial class ActivationStatusResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string ActivationStatusResult;
        
        public ActivationStatusResponseBody() {
        }
        
        public ActivationStatusResponseBody(string ActivationStatusResult) {
            this.ActivationStatusResult = ActivationStatusResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class RegisterPagerRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="RegisterPager", Namespace="http://wsdl.dcsglobal.com/", Order=0)]
        public DCSGlobal.Medical.VoiceRecoder.srRegister.RegisterPagerRequestBody Body;
        
        public RegisterPagerRequest() {
        }
        
        public RegisterPagerRequest(DCSGlobal.Medical.VoiceRecoder.srRegister.RegisterPagerRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://wsdl.dcsglobal.com/")]
    public partial class RegisterPagerRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string PagerHardwareID;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string HOS_CODE;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string Passwd;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public DCSGlobal.Medical.VoiceRecoder.srRegister.ServiceData SD;
        
        public RegisterPagerRequestBody() {
        }
        
        public RegisterPagerRequestBody(string PagerHardwareID, string HOS_CODE, string Passwd, DCSGlobal.Medical.VoiceRecoder.srRegister.ServiceData SD) {
            this.PagerHardwareID = PagerHardwareID;
            this.HOS_CODE = HOS_CODE;
            this.Passwd = Passwd;
            this.SD = SD;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class RegisterPagerResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="RegisterPagerResponse", Namespace="http://wsdl.dcsglobal.com/", Order=0)]
        public DCSGlobal.Medical.VoiceRecoder.srRegister.RegisterPagerResponseBody Body;
        
        public RegisterPagerResponse() {
        }
        
        public RegisterPagerResponse(DCSGlobal.Medical.VoiceRecoder.srRegister.RegisterPagerResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://wsdl.dcsglobal.com/")]
    public partial class RegisterPagerResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public int RegisterPagerResult;
        
        public RegisterPagerResponseBody() {
        }
        
        public RegisterPagerResponseBody(int RegisterPagerResult) {
            this.RegisterPagerResult = RegisterPagerResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetPagerIDRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetPagerID", Namespace="http://wsdl.dcsglobal.com/", Order=0)]
        public DCSGlobal.Medical.VoiceRecoder.srRegister.GetPagerIDRequestBody Body;
        
        public GetPagerIDRequest() {
        }
        
        public GetPagerIDRequest(DCSGlobal.Medical.VoiceRecoder.srRegister.GetPagerIDRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://wsdl.dcsglobal.com/")]
    public partial class GetPagerIDRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string PagerHardwareID;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public DCSGlobal.Medical.VoiceRecoder.srRegister.ServiceData SD;
        
        public GetPagerIDRequestBody() {
        }
        
        public GetPagerIDRequestBody(string PagerHardwareID, DCSGlobal.Medical.VoiceRecoder.srRegister.ServiceData SD) {
            this.PagerHardwareID = PagerHardwareID;
            this.SD = SD;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetPagerIDResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetPagerIDResponse", Namespace="http://wsdl.dcsglobal.com/", Order=0)]
        public DCSGlobal.Medical.VoiceRecoder.srRegister.GetPagerIDResponseBody Body;
        
        public GetPagerIDResponse() {
        }
        
        public GetPagerIDResponse(DCSGlobal.Medical.VoiceRecoder.srRegister.GetPagerIDResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://wsdl.dcsglobal.com/")]
    public partial class GetPagerIDResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public int GetPagerIDResult;
        
        public GetPagerIDResponseBody() {
        }
        
        public GetPagerIDResponseBody(int GetPagerIDResult) {
            this.GetPagerIDResult = GetPagerIDResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ActivatePagerRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ActivatePager", Namespace="http://wsdl.dcsglobal.com/", Order=0)]
        public DCSGlobal.Medical.VoiceRecoder.srRegister.ActivatePagerRequestBody Body;
        
        public ActivatePagerRequest() {
        }
        
        public ActivatePagerRequest(DCSGlobal.Medical.VoiceRecoder.srRegister.ActivatePagerRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://wsdl.dcsglobal.com/")]
    public partial class ActivatePagerRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string PagerHardwareID;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public DCSGlobal.Medical.VoiceRecoder.srRegister.ServiceData SD;
        
        public ActivatePagerRequestBody() {
        }
        
        public ActivatePagerRequestBody(string PagerHardwareID, DCSGlobal.Medical.VoiceRecoder.srRegister.ServiceData SD) {
            this.PagerHardwareID = PagerHardwareID;
            this.SD = SD;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ActivatePagerResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ActivatePagerResponse", Namespace="http://wsdl.dcsglobal.com/", Order=0)]
        public DCSGlobal.Medical.VoiceRecoder.srRegister.ActivatePagerResponseBody Body;
        
        public ActivatePagerResponse() {
        }
        
        public ActivatePagerResponse(DCSGlobal.Medical.VoiceRecoder.srRegister.ActivatePagerResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://wsdl.dcsglobal.com/")]
    public partial class ActivatePagerResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public int ActivatePagerResult;
        
        public ActivatePagerResponseBody() {
        }
        
        public ActivatePagerResponseBody(int ActivatePagerResult) {
            this.ActivatePagerResult = ActivatePagerResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface RegisterPager1SoapChannel : DCSGlobal.Medical.VoiceRecoder.srRegister.RegisterPager1Soap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class RegisterPager1SoapClient : System.ServiceModel.ClientBase<DCSGlobal.Medical.VoiceRecoder.srRegister.RegisterPager1Soap>, DCSGlobal.Medical.VoiceRecoder.srRegister.RegisterPager1Soap {
        
        public RegisterPager1SoapClient() {
        }
        
        public RegisterPager1SoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public RegisterPager1SoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public RegisterPager1SoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public RegisterPager1SoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        DCSGlobal.Medical.VoiceRecoder.srRegister.TestConnectionResponse DCSGlobal.Medical.VoiceRecoder.srRegister.RegisterPager1Soap.TestConnection(DCSGlobal.Medical.VoiceRecoder.srRegister.TestConnectionRequest request) {
            return base.Channel.TestConnection(request);
        }
        
        public string TestConnection() {
            DCSGlobal.Medical.VoiceRecoder.srRegister.TestConnectionRequest inValue = new DCSGlobal.Medical.VoiceRecoder.srRegister.TestConnectionRequest();
            inValue.Body = new DCSGlobal.Medical.VoiceRecoder.srRegister.TestConnectionRequestBody();
            DCSGlobal.Medical.VoiceRecoder.srRegister.TestConnectionResponse retVal = ((DCSGlobal.Medical.VoiceRecoder.srRegister.RegisterPager1Soap)(this)).TestConnection(inValue);
            return retVal.Body.TestConnectionResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        DCSGlobal.Medical.VoiceRecoder.srRegister.ActivationStatusResponse DCSGlobal.Medical.VoiceRecoder.srRegister.RegisterPager1Soap.ActivationStatus(DCSGlobal.Medical.VoiceRecoder.srRegister.ActivationStatusRequest request) {
            return base.Channel.ActivationStatus(request);
        }
        
        public string ActivationStatus(int PagerKey, DCSGlobal.Medical.VoiceRecoder.srRegister.ServiceData SD) {
            DCSGlobal.Medical.VoiceRecoder.srRegister.ActivationStatusRequest inValue = new DCSGlobal.Medical.VoiceRecoder.srRegister.ActivationStatusRequest();
            inValue.Body = new DCSGlobal.Medical.VoiceRecoder.srRegister.ActivationStatusRequestBody();
            inValue.Body.PagerKey = PagerKey;
            inValue.Body.SD = SD;
            DCSGlobal.Medical.VoiceRecoder.srRegister.ActivationStatusResponse retVal = ((DCSGlobal.Medical.VoiceRecoder.srRegister.RegisterPager1Soap)(this)).ActivationStatus(inValue);
            return retVal.Body.ActivationStatusResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        DCSGlobal.Medical.VoiceRecoder.srRegister.RegisterPagerResponse DCSGlobal.Medical.VoiceRecoder.srRegister.RegisterPager1Soap.RegisterPager(DCSGlobal.Medical.VoiceRecoder.srRegister.RegisterPagerRequest request) {
            return base.Channel.RegisterPager(request);
        }
        
        public int RegisterPager(string PagerHardwareID, string HOS_CODE, string Passwd, DCSGlobal.Medical.VoiceRecoder.srRegister.ServiceData SD) {
            DCSGlobal.Medical.VoiceRecoder.srRegister.RegisterPagerRequest inValue = new DCSGlobal.Medical.VoiceRecoder.srRegister.RegisterPagerRequest();
            inValue.Body = new DCSGlobal.Medical.VoiceRecoder.srRegister.RegisterPagerRequestBody();
            inValue.Body.PagerHardwareID = PagerHardwareID;
            inValue.Body.HOS_CODE = HOS_CODE;
            inValue.Body.Passwd = Passwd;
            inValue.Body.SD = SD;
            DCSGlobal.Medical.VoiceRecoder.srRegister.RegisterPagerResponse retVal = ((DCSGlobal.Medical.VoiceRecoder.srRegister.RegisterPager1Soap)(this)).RegisterPager(inValue);
            return retVal.Body.RegisterPagerResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        DCSGlobal.Medical.VoiceRecoder.srRegister.GetPagerIDResponse DCSGlobal.Medical.VoiceRecoder.srRegister.RegisterPager1Soap.GetPagerID(DCSGlobal.Medical.VoiceRecoder.srRegister.GetPagerIDRequest request) {
            return base.Channel.GetPagerID(request);
        }
        
        public int GetPagerID(string PagerHardwareID, DCSGlobal.Medical.VoiceRecoder.srRegister.ServiceData SD) {
            DCSGlobal.Medical.VoiceRecoder.srRegister.GetPagerIDRequest inValue = new DCSGlobal.Medical.VoiceRecoder.srRegister.GetPagerIDRequest();
            inValue.Body = new DCSGlobal.Medical.VoiceRecoder.srRegister.GetPagerIDRequestBody();
            inValue.Body.PagerHardwareID = PagerHardwareID;
            inValue.Body.SD = SD;
            DCSGlobal.Medical.VoiceRecoder.srRegister.GetPagerIDResponse retVal = ((DCSGlobal.Medical.VoiceRecoder.srRegister.RegisterPager1Soap)(this)).GetPagerID(inValue);
            return retVal.Body.GetPagerIDResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        DCSGlobal.Medical.VoiceRecoder.srRegister.ActivatePagerResponse DCSGlobal.Medical.VoiceRecoder.srRegister.RegisterPager1Soap.ActivatePager(DCSGlobal.Medical.VoiceRecoder.srRegister.ActivatePagerRequest request) {
            return base.Channel.ActivatePager(request);
        }
        
        public int ActivatePager(string PagerHardwareID, DCSGlobal.Medical.VoiceRecoder.srRegister.ServiceData SD) {
            DCSGlobal.Medical.VoiceRecoder.srRegister.ActivatePagerRequest inValue = new DCSGlobal.Medical.VoiceRecoder.srRegister.ActivatePagerRequest();
            inValue.Body = new DCSGlobal.Medical.VoiceRecoder.srRegister.ActivatePagerRequestBody();
            inValue.Body.PagerHardwareID = PagerHardwareID;
            inValue.Body.SD = SD;
            DCSGlobal.Medical.VoiceRecoder.srRegister.ActivatePagerResponse retVal = ((DCSGlobal.Medical.VoiceRecoder.srRegister.RegisterPager1Soap)(this)).ActivatePager(inValue);
            return retVal.Body.ActivatePagerResult;
        }
    }
}
