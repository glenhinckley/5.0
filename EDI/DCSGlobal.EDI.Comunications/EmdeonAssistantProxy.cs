
using System;
using System.Web.Services;
using System.Diagnostics;
using System.Web.Services.Protocols;
using System.Xml.Serialization;
using System.ComponentModel;

namespace com.emdeon.assistant
{


    /// <remarks/>
    // CODEGEN: The optional WSDL extension element 'PolicyReference' from namespace 'http://schemas.xmlsoap.org/ws/2004/09/policy' was not handled.
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "BasicHttpBinding_IAssistant", Namespace = "http://tempuri.org/")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MarshalByRefObject))]
    public partial class Assistant : System.Web.Services.Protocols.SoapHttpClientProtocol
    {

        private System.Threading.SendOrPostCallback DtoTransactionSearchOperationCompleted;

        private System.Threading.SendOrPostCallback BuildDtoResponseByMrnAccountOperationCompleted;

        private System.Threading.SendOrPostCallback BuildDtoResponseByDateOperationCompleted;

        private System.Threading.SendOrPostCallback BuildDtoResponseByPatientOperationCompleted;

        private System.Threading.SendOrPostCallback ProcessRealTimeOperationCompleted;

        private System.Threading.SendOrPostCallback GetAllActiveMapHashesOperationCompleted;

        private System.Threading.SendOrPostCallback GetMapOperationCompleted;

        private System.Threading.SendOrPostCallback SaveConnectDirectDetailsOperationCompleted;

        private bool useDefaultCredentialsSetExplicitly;

        /// <remarks/>
        public Assistant()
        {
            this.Url = "https://assistant.emdeon.com/Services/Assistant";  //global::TEST.Properties.Settings.Default.TEST_com_emdeon_assistant_Assistant;
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
        public event DtoTransactionSearchCompletedEventHandler DtoTransactionSearchCompleted;

        /// <remarks/>
        public event BuildDtoResponseByMrnAccountCompletedEventHandler BuildDtoResponseByMrnAccountCompleted;

        /// <remarks/>
        public event BuildDtoResponseByDateCompletedEventHandler BuildDtoResponseByDateCompleted;

        /// <remarks/>
        public event BuildDtoResponseByPatientCompletedEventHandler BuildDtoResponseByPatientCompleted;

        /// <remarks/>
        public event ProcessRealTimeCompletedEventHandler ProcessRealTimeCompleted;

        /// <remarks/>
        public event GetAllActiveMapHashesCompletedEventHandler GetAllActiveMapHashesCompleted;

        /// <remarks/>
        public event GetMapCompletedEventHandler GetMapCompleted;

        /// <remarks/>
        public event SaveConnectDirectDetailsCompletedEventHandler SaveConnectDirectDetailsCompleted;

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/IAssistant/DtoTransactionSearch", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public DtoResponse DtoTransactionSearch([System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] DtoRequest req)
        {
            object[] results = this.Invoke("DtoTransactionSearch", new object[] {
                        req});
            return ((DtoResponse)(results[0]));
        }

        /// <remarks/>
        public void DtoTransactionSearchAsync(DtoRequest req)
        {
            this.DtoTransactionSearchAsync(req, null);
        }

        /// <remarks/>
        public void DtoTransactionSearchAsync(DtoRequest req, object userState)
        {
            if ((this.DtoTransactionSearchOperationCompleted == null))
            {
                this.DtoTransactionSearchOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDtoTransactionSearchOperationCompleted);
            }
            this.InvokeAsync("DtoTransactionSearch", new object[] {
                        req}, this.DtoTransactionSearchOperationCompleted, userState);
        }

        private void OnDtoTransactionSearchOperationCompleted(object arg)
        {
            if ((this.DtoTransactionSearchCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DtoTransactionSearchCompleted(this, new DtoTransactionSearchCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/IAssistant/BuildDtoResponseByMrnAccount", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public DtoResponse BuildDtoResponseByMrnAccount([System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string apiKey, [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string mrn, [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string accountNumber, System.DateTime transactionDate, [System.Xml.Serialization.XmlIgnoreAttribute()] bool transactionDateSpecified)
        {
            object[] results = this.Invoke("BuildDtoResponseByMrnAccount", new object[] {
                        apiKey,
                        mrn,
                        accountNumber,
                        transactionDate,
                        transactionDateSpecified});
            return ((DtoResponse)(results[0]));
        }

        /// <remarks/>
        public void BuildDtoResponseByMrnAccountAsync(string apiKey, string mrn, string accountNumber, System.DateTime transactionDate, bool transactionDateSpecified)
        {
            this.BuildDtoResponseByMrnAccountAsync(apiKey, mrn, accountNumber, transactionDate, transactionDateSpecified, null);
        }

        /// <remarks/>
        public void BuildDtoResponseByMrnAccountAsync(string apiKey, string mrn, string accountNumber, System.DateTime transactionDate, bool transactionDateSpecified, object userState)
        {
            if ((this.BuildDtoResponseByMrnAccountOperationCompleted == null))
            {
                this.BuildDtoResponseByMrnAccountOperationCompleted = new System.Threading.SendOrPostCallback(this.OnBuildDtoResponseByMrnAccountOperationCompleted);
            }
            this.InvokeAsync("BuildDtoResponseByMrnAccount", new object[] {
                        apiKey,
                        mrn,
                        accountNumber,
                        transactionDate,
                        transactionDateSpecified}, this.BuildDtoResponseByMrnAccountOperationCompleted, userState);
        }

        private void OnBuildDtoResponseByMrnAccountOperationCompleted(object arg)
        {
            if ((this.BuildDtoResponseByMrnAccountCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.BuildDtoResponseByMrnAccountCompleted(this, new BuildDtoResponseByMrnAccountCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/IAssistant/BuildDtoResponseByDate", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public DtoResponse BuildDtoResponseByDate([System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string apiKey, System.DateTime transactionDateBegin, [System.Xml.Serialization.XmlIgnoreAttribute()] bool transactionDateBeginSpecified, System.DateTime transactionDateEnd, [System.Xml.Serialization.XmlIgnoreAttribute()] bool transactionDateEndSpecified)
        {
            object[] results = this.Invoke("BuildDtoResponseByDate", new object[] {
                        apiKey,
                        transactionDateBegin,
                        transactionDateBeginSpecified,
                        transactionDateEnd,
                        transactionDateEndSpecified});
            return ((DtoResponse)(results[0]));
        }

        /// <remarks/>
        public void BuildDtoResponseByDateAsync(string apiKey, System.DateTime transactionDateBegin, bool transactionDateBeginSpecified, System.DateTime transactionDateEnd, bool transactionDateEndSpecified)
        {
            this.BuildDtoResponseByDateAsync(apiKey, transactionDateBegin, transactionDateBeginSpecified, transactionDateEnd, transactionDateEndSpecified, null);
        }

        /// <remarks/>
        public void BuildDtoResponseByDateAsync(string apiKey, System.DateTime transactionDateBegin, bool transactionDateBeginSpecified, System.DateTime transactionDateEnd, bool transactionDateEndSpecified, object userState)
        {
            if ((this.BuildDtoResponseByDateOperationCompleted == null))
            {
                this.BuildDtoResponseByDateOperationCompleted = new System.Threading.SendOrPostCallback(this.OnBuildDtoResponseByDateOperationCompleted);
            }
            this.InvokeAsync("BuildDtoResponseByDate", new object[] {
                        apiKey,
                        transactionDateBegin,
                        transactionDateBeginSpecified,
                        transactionDateEnd,
                        transactionDateEndSpecified}, this.BuildDtoResponseByDateOperationCompleted, userState);
        }

        private void OnBuildDtoResponseByDateOperationCompleted(object arg)
        {
            if ((this.BuildDtoResponseByDateCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.BuildDtoResponseByDateCompleted(this, new BuildDtoResponseByDateCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/IAssistant/BuildDtoResponseByPatient", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public DtoResponse BuildDtoResponseByPatient([System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string apiKey, [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string patientLastName, [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string patientFirstName, System.DateTime patientDob, [System.Xml.Serialization.XmlIgnoreAttribute()] bool patientDobSpecified, System.DateTime transactionDate, [System.Xml.Serialization.XmlIgnoreAttribute()] bool transactionDateSpecified)
        {
            object[] results = this.Invoke("BuildDtoResponseByPatient", new object[] {
                        apiKey,
                        patientLastName,
                        patientFirstName,
                        patientDob,
                        patientDobSpecified,
                        transactionDate,
                        transactionDateSpecified});
            return ((DtoResponse)(results[0]));
        }

        /// <remarks/>
        public void BuildDtoResponseByPatientAsync(string apiKey, string patientLastName, string patientFirstName, System.DateTime patientDob, bool patientDobSpecified, System.DateTime transactionDate, bool transactionDateSpecified)
        {
            this.BuildDtoResponseByPatientAsync(apiKey, patientLastName, patientFirstName, patientDob, patientDobSpecified, transactionDate, transactionDateSpecified, null);
        }

        /// <remarks/>
        public void BuildDtoResponseByPatientAsync(string apiKey, string patientLastName, string patientFirstName, System.DateTime patientDob, bool patientDobSpecified, System.DateTime transactionDate, bool transactionDateSpecified, object userState)
        {
            if ((this.BuildDtoResponseByPatientOperationCompleted == null))
            {
                this.BuildDtoResponseByPatientOperationCompleted = new System.Threading.SendOrPostCallback(this.OnBuildDtoResponseByPatientOperationCompleted);
            }
            this.InvokeAsync("BuildDtoResponseByPatient", new object[] {
                        apiKey,
                        patientLastName,
                        patientFirstName,
                        patientDob,
                        patientDobSpecified,
                        transactionDate,
                        transactionDateSpecified}, this.BuildDtoResponseByPatientOperationCompleted, userState);
        }

        private void OnBuildDtoResponseByPatientOperationCompleted(object arg)
        {
            if ((this.BuildDtoResponseByPatientCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.BuildDtoResponseByPatientCompleted(this, new BuildDtoResponseByPatientCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/IAssistant/ProcessRealTime", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public DtoStatusResponse ProcessRealTime([System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string apiKey, [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string request)
        {
            object[] results = this.Invoke("ProcessRealTime", new object[] {
                        apiKey,
                        request});
            return ((DtoStatusResponse)(results[0]));
        }

        /// <remarks/>
        public void ProcessRealTimeAsync(string apiKey, string request)
        {
            this.ProcessRealTimeAsync(apiKey, request, null);
        }

        /// <remarks/>
        public void ProcessRealTimeAsync(string apiKey, string request, object userState)
        {
            if ((this.ProcessRealTimeOperationCompleted == null))
            {
                this.ProcessRealTimeOperationCompleted = new System.Threading.SendOrPostCallback(this.OnProcessRealTimeOperationCompleted);
            }
            this.InvokeAsync("ProcessRealTime", new object[] {
                        apiKey,
                        request}, this.ProcessRealTimeOperationCompleted, userState);
        }

        private void OnProcessRealTimeOperationCompleted(object arg)
        {
            if ((this.ProcessRealTimeCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ProcessRealTimeCompleted(this, new ProcessRealTimeCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/IAssistant/GetAllActiveMapHashes", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public MapHashResponse GetAllActiveMapHashes([System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string apikey)
        {
            object[] results = this.Invoke("GetAllActiveMapHashes", new object[] {
                        apikey});
            return ((MapHashResponse)(results[0]));
        }

        /// <remarks/>
        public void GetAllActiveMapHashesAsync(string apikey)
        {
            this.GetAllActiveMapHashesAsync(apikey, null);
        }

        /// <remarks/>
        public void GetAllActiveMapHashesAsync(string apikey, object userState)
        {
            if ((this.GetAllActiveMapHashesOperationCompleted == null))
            {
                this.GetAllActiveMapHashesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetAllActiveMapHashesOperationCompleted);
            }
            this.InvokeAsync("GetAllActiveMapHashes", new object[] {
                        apikey}, this.GetAllActiveMapHashesOperationCompleted, userState);
        }

        private void OnGetAllActiveMapHashesOperationCompleted(object arg)
        {
            if ((this.GetAllActiveMapHashesCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetAllActiveMapHashesCompleted(this, new GetAllActiveMapHashesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/IAssistant/GetMap", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public MemoryStream GetMap([System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string apiKey, [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] MapHashItem requestedMap)
        {
            object[] results = this.Invoke("GetMap", new object[] {
                        apiKey,
                        requestedMap});
            return ((MemoryStream)(results[0]));
        }

        /// <remarks/>
        public void GetMapAsync(string apiKey, MapHashItem requestedMap)
        {
            this.GetMapAsync(apiKey, requestedMap, null);
        }

        /// <remarks/>
        public void GetMapAsync(string apiKey, MapHashItem requestedMap, object userState)
        {
            if ((this.GetMapOperationCompleted == null))
            {
                this.GetMapOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetMapOperationCompleted);
            }
            this.InvokeAsync("GetMap", new object[] {
                        apiKey,
                        requestedMap}, this.GetMapOperationCompleted, userState);
        }

        private void OnGetMapOperationCompleted(object arg)
        {
            if ((this.GetMapCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetMapCompleted(this, new GetMapCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/IAssistant/SaveConnectDirectDetails", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string SaveConnectDirectDetails([System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string apiKey, [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string ipPort, [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string direction, System.DateTime transactionDate, [System.Xml.Serialization.XmlIgnoreAttribute()] bool transactionDateSpecified, [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string appVersion)
        {
            object[] results = this.Invoke("SaveConnectDirectDetails", new object[] {
                        apiKey,
                        ipPort,
                        direction,
                        transactionDate,
                        transactionDateSpecified,
                        appVersion});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void SaveConnectDirectDetailsAsync(string apiKey, string ipPort, string direction, System.DateTime transactionDate, bool transactionDateSpecified, string appVersion)
        {
            this.SaveConnectDirectDetailsAsync(apiKey, ipPort, direction, transactionDate, transactionDateSpecified, appVersion, null);
        }

        /// <remarks/>
        public void SaveConnectDirectDetailsAsync(string apiKey, string ipPort, string direction, System.DateTime transactionDate, bool transactionDateSpecified, string appVersion, object userState)
        {
            if ((this.SaveConnectDirectDetailsOperationCompleted == null))
            {
                this.SaveConnectDirectDetailsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSaveConnectDirectDetailsOperationCompleted);
            }
            this.InvokeAsync("SaveConnectDirectDetails", new object[] {
                        apiKey,
                        ipPort,
                        direction,
                        transactionDate,
                        transactionDateSpecified,
                        appVersion}, this.SaveConnectDirectDetailsOperationCompleted, userState);
        }

        private void OnSaveConnectDirectDetailsOperationCompleted(object arg)
        {
            if ((this.SaveConnectDirectDetailsCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SaveConnectDirectDetailsCompleted(this, new SaveConnectDirectDetailsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1067.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.datacontract.org/2004/07/Emdeon.Assistant.Model.Api")]
    public partial class DtoRequest
    {

        private string apiKeyField;

        private DtoSearchField[] dtoSearchFieldsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ApiKey
        {
            get
            {
                return this.apiKeyField;
            }
            set
            {
                this.apiKeyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable = true)]
        public DtoSearchField[] DtoSearchFields
        {
            get
            {
                return this.dtoSearchFieldsField;
            }
            set
            {
                this.dtoSearchFieldsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1067.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.datacontract.org/2004/07/Emdeon.Assistant.Model.Api")]
    public partial class DtoSearchField
    {

        private SearchFields fieldTypeField;

        private string valueField;

        /// <remarks/>
        public SearchFields FieldType
        {
            get
            {
                return this.fieldTypeField;
            }
            set
            {
                this.fieldTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1067.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.datacontract.org/2004/07/Emdeon.Assistant.Model.Api")]
    public enum SearchFields
    {

        /// <remarks/>
        AccountNumber,

        /// <remarks/>
        MedicalRecordNumber,

        /// <remarks/>
        PatientLastName,

        /// <remarks/>
        PatientFirstName,

        /// <remarks/>
        PatientDateOfBirth,

        /// <remarks/>
        RecipientId,

        /// <remarks/>
        DateOfService,

        /// <remarks/>
        EmdeonPayerCode,
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Stream))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MemoryStream))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1067.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.datacontract.org/2004/07/System")]
    public partial class MarshalByRefObject
    {

        private object @__identityField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public object @__identity
        {
            get
            {
                return this.@__identityField;
            }
            set
            {
                this.@__identityField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MemoryStream))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1067.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.datacontract.org/2004/07/System.IO")]
    public partial class Stream : MarshalByRefObject
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1067.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.datacontract.org/2004/07/System.IO")]
    public partial class MemoryStream : Stream
    {

        private byte[] _bufferField;

        private int _capacityField;

        private bool _expandableField;

        private bool _exposableField;

        private bool _isOpenField;

        private int _lengthField;

        private int _originField;

        private int _positionField;

        private bool _writableField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary", IsNullable = true)]
        public byte[] _buffer
        {
            get
            {
                return this._bufferField;
            }
            set
            {
                this._bufferField = value;
            }
        }

        /// <remarks/>
        public int _capacity
        {
            get
            {
                return this._capacityField;
            }
            set
            {
                this._capacityField = value;
            }
        }

        /// <remarks/>
        public bool _expandable
        {
            get
            {
                return this._expandableField;
            }
            set
            {
                this._expandableField = value;
            }
        }

        /// <remarks/>
        public bool _exposable
        {
            get
            {
                return this._exposableField;
            }
            set
            {
                this._exposableField = value;
            }
        }

        /// <remarks/>
        public bool _isOpen
        {
            get
            {
                return this._isOpenField;
            }
            set
            {
                this._isOpenField = value;
            }
        }

        /// <remarks/>
        public int _length
        {
            get
            {
                return this._lengthField;
            }
            set
            {
                this._lengthField = value;
            }
        }

        /// <remarks/>
        public int _origin
        {
            get
            {
                return this._originField;
            }
            set
            {
                this._originField = value;
            }
        }

        /// <remarks/>
        public int _position
        {
            get
            {
                return this._positionField;
            }
            set
            {
                this._positionField = value;
            }
        }

        /// <remarks/>
        public bool _writable
        {
            get
            {
                return this._writableField;
            }
            set
            {
                this._writableField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1067.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.datacontract.org/2004/07/Emdeon.Assistant.Model.Assistant.Maps")]
    public partial class MapHashItem
    {

        private string facilityNameField;

        private string fileNameField;

        private string hashField;

        private string mapIdField;

        private string idField;

        private string refField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string FacilityName
        {
            get
            {
                return this.facilityNameField;
            }
            set
            {
                this.facilityNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string FileName
        {
            get
            {
                return this.fileNameField;
            }
            set
            {
                this.fileNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string Hash
        {
            get
            {
                return this.hashField;
            }
            set
            {
                this.hashField = value;
            }
        }

        /// <remarks/>
        public string MapId
        {
            get
            {
                return this.mapIdField;
            }
            set
            {
                this.mapIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://schemas.microsoft.com/2003/10/Serialization/", DataType = "ID")]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://schemas.microsoft.com/2003/10/Serialization/", DataType = "IDREF")]
        public string Ref
        {
            get
            {
                return this.refField;
            }
            set
            {
                this.refField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1067.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.datacontract.org/2004/07/Emdeon.Assistant.Model.Assistant.Response" +
        "s")]
    public partial class MapHashResponse
    {

        private int errorCodeField;

        private bool errorCodeFieldSpecified;

        private MapHashItem[] hashesField;

        private string messageField;

        private string idField;

        private string refField;

        /// <remarks/>
        public int ErrorCode
        {
            get
            {
                return this.errorCodeField;
            }
            set
            {
                this.errorCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ErrorCodeSpecified
        {
            get
            {
                return this.errorCodeFieldSpecified;
            }
            set
            {
                this.errorCodeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable = true)]
        [System.Xml.Serialization.XmlArrayItemAttribute(Namespace = "http://schemas.datacontract.org/2004/07/Emdeon.Assistant.Model.Assistant.Maps")]
        public MapHashItem[] Hashes
        {
            get
            {
                return this.hashesField;
            }
            set
            {
                this.hashesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string Message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://schemas.microsoft.com/2003/10/Serialization/", DataType = "ID")]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://schemas.microsoft.com/2003/10/Serialization/", DataType = "IDREF")]
        public string Ref
        {
            get
            {
                return this.refField;
            }
            set
            {
                this.refField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1067.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.datacontract.org/2004/07/Emdeon.Assistant.Model.Import")]
    public partial class DtoStatusResponse
    {

        private string responseField;

        private int statusCodeField;

        private bool statusCodeFieldSpecified;

        private string statusMessageField;

        private string idField;

        private string refField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string Response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }

        /// <remarks/>
        public int StatusCode
        {
            get
            {
                return this.statusCodeField;
            }
            set
            {
                this.statusCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool StatusCodeSpecified
        {
            get
            {
                return this.statusCodeFieldSpecified;
            }
            set
            {
                this.statusCodeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string StatusMessage
        {
            get
            {
                return this.statusMessageField;
            }
            set
            {
                this.statusMessageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://schemas.microsoft.com/2003/10/Serialization/", DataType = "ID")]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://schemas.microsoft.com/2003/10/Serialization/", DataType = "IDREF")]
        public string Ref
        {
            get
            {
                return this.refField;
            }
            set
            {
                this.refField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1067.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.datacontract.org/2004/07/Emdeon.Assistant.Model.Api")]
    public partial class DtoMetaData
    {

        private string nameField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1067.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.datacontract.org/2004/07/Emdeon.Assistant.Model.Api")]
    public partial class DtoTransaction
    {

        private DtoMetaData[] dtoMetaDatasField;

        private string requestStringField;

        private string responseStringField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable = true)]
        public DtoMetaData[] DtoMetaDatas
        {
            get
            {
                return this.dtoMetaDatasField;
            }
            set
            {
                this.dtoMetaDatasField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string RequestString
        {
            get
            {
                return this.requestStringField;
            }
            set
            {
                this.requestStringField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ResponseString
        {
            get
            {
                return this.responseStringField;
            }
            set
            {
                this.responseStringField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1067.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.datacontract.org/2004/07/Emdeon.Assistant.Model.Api")]
    public partial class DtoResponse
    {

        private DtoTransaction[] dtoTransactionsField;

        private string statusMessageField;

        private int transactionCountField;

        private bool transactionCountFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable = true)]
        public DtoTransaction[] DtoTransactions
        {
            get
            {
                return this.dtoTransactionsField;
            }
            set
            {
                this.dtoTransactionsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string StatusMessage
        {
            get
            {
                return this.statusMessageField;
            }
            set
            {
                this.statusMessageField = value;
            }
        }

        /// <remarks/>
        public int TransactionCount
        {
            get
            {
                return this.transactionCountField;
            }
            set
            {
                this.transactionCountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TransactionCountSpecified
        {
            get
            {
                return this.transactionCountFieldSpecified;
            }
            set
            {
                this.transactionCountFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void DtoTransactionSearchCompletedEventHandler(object sender, DtoTransactionSearchCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DtoTransactionSearchCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal DtoTransactionSearchCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public DtoResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((DtoResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void BuildDtoResponseByMrnAccountCompletedEventHandler(object sender, BuildDtoResponseByMrnAccountCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class BuildDtoResponseByMrnAccountCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal BuildDtoResponseByMrnAccountCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public DtoResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((DtoResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void BuildDtoResponseByDateCompletedEventHandler(object sender, BuildDtoResponseByDateCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class BuildDtoResponseByDateCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal BuildDtoResponseByDateCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public DtoResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((DtoResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void BuildDtoResponseByPatientCompletedEventHandler(object sender, BuildDtoResponseByPatientCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class BuildDtoResponseByPatientCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal BuildDtoResponseByPatientCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public DtoResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((DtoResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void ProcessRealTimeCompletedEventHandler(object sender, ProcessRealTimeCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ProcessRealTimeCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal ProcessRealTimeCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public DtoStatusResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((DtoStatusResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void GetAllActiveMapHashesCompletedEventHandler(object sender, GetAllActiveMapHashesCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetAllActiveMapHashesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetAllActiveMapHashesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public MapHashResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((MapHashResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void GetMapCompletedEventHandler(object sender, GetMapCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetMapCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetMapCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public MemoryStream Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((MemoryStream)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void SaveConnectDirectDetailsCompletedEventHandler(object sender, SaveConnectDirectDetailsCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SaveConnectDirectDetailsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal SaveConnectDirectDetailsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
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

 