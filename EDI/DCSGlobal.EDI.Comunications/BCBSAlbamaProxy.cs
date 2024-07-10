using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DCSGlobal.EDI.Comunications.BCBSAlbamaProxy
{


    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://www.caqh.org/SOAP/WSDL/", ConfigurationName = "BCBSAlbama.CORETransactions")]
    public interface CORETransactions
    {

        // CODEGEN: Generating message contract since the operation RealTimeTransaction is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action = "RealTimeTransaction", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        DCSGlobal.EDI.Comunications.BCBSAlbamaProxy.RealTimeTransactionResponse RealTimeTransaction(DCSGlobal.EDI.Comunications.BCBSAlbamaProxy.RealTimeTransactionRequest request);
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1087.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.caqh.org/SOAP/WSDL/CORERule2.2.0.xsd")]
    public partial class COREEnvelopeRealTimeRequest : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string payloadTypeField;

        private string processingModeField;

        private string payloadIDField;

        private string timeStampField;

        private string senderIDField;

        private string receiverIDField;

        private string cORERuleVersionField;

        private string payloadField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public string PayloadType
        {
            get
            {
                return this.payloadTypeField;
            }
            set
            {
                this.payloadTypeField = value;
                this.RaisePropertyChanged("PayloadType");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public string ProcessingMode
        {
            get
            {
                return this.processingModeField;
            }
            set
            {
                this.processingModeField = value;
                this.RaisePropertyChanged("ProcessingMode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        public string PayloadID
        {
            get
            {
                return this.payloadIDField;
            }
            set
            {
                this.payloadIDField = value;
                this.RaisePropertyChanged("PayloadID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 3)]
        public string TimeStamp
        {
            get
            {
                return this.timeStampField;
            }
            set
            {
                this.timeStampField = value;
                this.RaisePropertyChanged("TimeStamp");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 4)]
        public string SenderID
        {
            get
            {
                return this.senderIDField;
            }
            set
            {
                this.senderIDField = value;
                this.RaisePropertyChanged("SenderID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 5)]
        public string ReceiverID
        {
            get
            {
                return this.receiverIDField;
            }
            set
            {
                this.receiverIDField = value;
                this.RaisePropertyChanged("ReceiverID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 6)]
        public string CORERuleVersion
        {
            get
            {
                return this.cORERuleVersionField;
            }
            set
            {
                this.cORERuleVersionField = value;
                this.RaisePropertyChanged("CORERuleVersion");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 7)]
        public string Payload
        {
            get
            {
                return this.payloadField;
            }
            set
            {
                this.payloadField = value;
                this.RaisePropertyChanged("Payload");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1087.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.caqh.org/SOAP/WSDL/CORERule2.2.0.xsd")]
    public partial class COREEnvelopeRealTimeResponse : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string payloadTypeField;

        private string processingModeField;

        private string payloadIDField;

        private string timeStampField;

        private string senderIDField;

        private string receiverIDField;

        private string cORERuleVersionField;

        private string payloadField;

        private string errorCodeField;

        private string errorMessageField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public string PayloadType
        {
            get
            {
                return this.payloadTypeField;
            }
            set
            {
                this.payloadTypeField = value;
                this.RaisePropertyChanged("PayloadType");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public string ProcessingMode
        {
            get
            {
                return this.processingModeField;
            }
            set
            {
                this.processingModeField = value;
                this.RaisePropertyChanged("ProcessingMode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        public string PayloadID
        {
            get
            {
                return this.payloadIDField;
            }
            set
            {
                this.payloadIDField = value;
                this.RaisePropertyChanged("PayloadID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 3)]
        public string TimeStamp
        {
            get
            {
                return this.timeStampField;
            }
            set
            {
                this.timeStampField = value;
                this.RaisePropertyChanged("TimeStamp");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 4)]
        public string SenderID
        {
            get
            {
                return this.senderIDField;
            }
            set
            {
                this.senderIDField = value;
                this.RaisePropertyChanged("SenderID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 5)]
        public string ReceiverID
        {
            get
            {
                return this.receiverIDField;
            }
            set
            {
                this.receiverIDField = value;
                this.RaisePropertyChanged("ReceiverID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 6)]
        public string CORERuleVersion
        {
            get
            {
                return this.cORERuleVersionField;
            }
            set
            {
                this.cORERuleVersionField = value;
                this.RaisePropertyChanged("CORERuleVersion");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 7)]
        public string Payload
        {
            get
            {
                return this.payloadField;
            }
            set
            {
                this.payloadField = value;
                this.RaisePropertyChanged("Payload");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 8)]
        public string ErrorCode
        {
            get
            {
                return this.errorCodeField;
            }
            set
            {
                this.errorCodeField = value;
                this.RaisePropertyChanged("ErrorCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 9)]
        public string ErrorMessage
        {
            get
            {
                return this.errorMessageField;
            }
            set
            {
                this.errorMessageField = value;
                this.RaisePropertyChanged("ErrorMessage");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class RealTimeTransactionRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.caqh.org/SOAP/WSDL/CORERule2.2.0.xsd", Order = 0)]
        public DCSGlobal.EDI.Comunications.BCBSAlbamaProxy.COREEnvelopeRealTimeRequest COREEnvelopeRealTimeRequest;

        public RealTimeTransactionRequest()
        {
        }

        public RealTimeTransactionRequest(DCSGlobal.EDI.Comunications.BCBSAlbamaProxy.COREEnvelopeRealTimeRequest COREEnvelopeRealTimeRequest)
        {
            this.COREEnvelopeRealTimeRequest = COREEnvelopeRealTimeRequest;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class RealTimeTransactionResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.caqh.org/SOAP/WSDL/CORERule2.2.0.xsd", Order = 0)]
        public DCSGlobal.EDI.Comunications.BCBSAlbamaProxy.COREEnvelopeRealTimeResponse COREEnvelopeRealTimeResponse;

        public RealTimeTransactionResponse()
        {
        }

        public RealTimeTransactionResponse(DCSGlobal.EDI.Comunications.BCBSAlbamaProxy.COREEnvelopeRealTimeResponse COREEnvelopeRealTimeResponse)
        {
            this.COREEnvelopeRealTimeResponse = COREEnvelopeRealTimeResponse;
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface CORETransactionsChannel : DCSGlobal.EDI.Comunications.BCBSAlbamaProxy.CORETransactions, System.ServiceModel.IClientChannel
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CORETransactionsClient : System.ServiceModel.ClientBase<DCSGlobal.EDI.Comunications.BCBSAlbamaProxy.CORETransactions>, DCSGlobal.EDI.Comunications.BCBSAlbamaProxy.CORETransactions
    {

        public CORETransactionsClient()
        {
        }

        public CORETransactionsClient(string endpointConfigurationName) :
            base(endpointConfigurationName)
        {
        }

        public CORETransactionsClient(string endpointConfigurationName, string remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public CORETransactionsClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public CORETransactionsClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
        {
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        DCSGlobal.EDI.Comunications.BCBSAlbamaProxy.RealTimeTransactionResponse DCSGlobal.EDI.Comunications.BCBSAlbamaProxy.CORETransactions.RealTimeTransaction(DCSGlobal.EDI.Comunications.BCBSAlbamaProxy.RealTimeTransactionRequest request)
        {
            return base.Channel.RealTimeTransaction(request);
        }

        public DCSGlobal.EDI.Comunications.BCBSAlbamaProxy.COREEnvelopeRealTimeResponse RealTimeTransaction(DCSGlobal.EDI.Comunications.BCBSAlbamaProxy.COREEnvelopeRealTimeRequest COREEnvelopeRealTimeRequest)
        {
            DCSGlobal.EDI.Comunications.BCBSAlbamaProxy.RealTimeTransactionRequest inValue = new DCSGlobal.EDI.Comunications.BCBSAlbamaProxy.RealTimeTransactionRequest();
            inValue.COREEnvelopeRealTimeRequest = COREEnvelopeRealTimeRequest;
            DCSGlobal.EDI.Comunications.BCBSAlbamaProxy.RealTimeTransactionResponse retVal = ((DCSGlobal.EDI.Comunications.BCBSAlbamaProxy.CORETransactions)(this)).RealTimeTransaction(inValue);
            return retVal.COREEnvelopeRealTimeResponse;
        }
    }
}
