using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

 
namespace DCSGlobal.EDI.Comunications.MissoulaWrapper
{


    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName = "MissoulaWrapper.ICaqhCoreTransactions")]
    public interface ICaqhCoreTransactions
    {

        [System.ServiceModel.OperationContractAttribute(Action = "ValidateUser", ReplyAction = "http://tempuri.org/ICaqhCoreTransactions/ValidateLoginResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        bool ValidateLogin(string userName, string password);

        // CODEGEN: Generating message contract since the operation RealTimeTransaction is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action = "RealTimeTransaction", ReplyAction = "http://tempuri.org/ICaqhCoreTransactions/RealTimeTransactionResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        DCSGlobal.EDI.Comunications.MissoulaWrapper.RealTimeTransactionResponse RealTimeTransaction(DCSGlobal.EDI.Comunications.MissoulaWrapper.RealTimeTransactionRequest request);

        // CODEGEN: Generating message contract since the operation BatchSubmitTransaction is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action = "BatchSubmitTransaction", ReplyAction = "http://tempuri.org/ICaqhCoreTransactions/BatchSubmitTransactionResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        DCSGlobal.EDI.Comunications.MissoulaWrapper.BatchSubmitTransactionResponse BatchSubmitTransaction(DCSGlobal.EDI.Comunications.MissoulaWrapper.BatchSubmitTransactionRequest request);

        // CODEGEN: Generating message contract since the operation BatchSubmitAckRetrievalTransaction is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action = "BatchSubmitAckRetrievalTransaction", ReplyAction = "http://tempuri.org/ICaqhCoreTransactions/BatchSubmitAckRetrievalTransactionRespon" +
            "se")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        DCSGlobal.EDI.Comunications.MissoulaWrapper.BatchSubmitAckRetrievalTransactionResponse BatchSubmitAckRetrievalTransaction(DCSGlobal.EDI.Comunications.MissoulaWrapper.BatchSubmitAckRetrievalTransactionRequest request);

        // CODEGEN: Generating message contract since the operation BatchResultsRetrievalTransaction is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action = "BatchResultsRetrievalTransaction", ReplyAction = "http://tempuri.org/ICaqhCoreTransactions/BatchResultsRetrievalTransactionResponse" +
            "")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        DCSGlobal.EDI.Comunications.MissoulaWrapper.BatchResultsRetrievalTransactionResponse BatchResultsRetrievalTransaction(DCSGlobal.EDI.Comunications.MissoulaWrapper.BatchResultsRetrievalTransactionRequest request);

        // CODEGEN: Generating message contract since the operation BatchResultsAckSubmitTransaction is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action = "BatchResultsAckSubmitTransaction", ReplyAction = "http://tempuri.org/ICaqhCoreTransactions/BatchResultsAckSubmitTransactionResponse" +
            "")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        DCSGlobal.EDI.Comunications.MissoulaWrapper.BatchResultsAckSubmitTransactionResponse BatchResultsAckSubmitTransaction(DCSGlobal.EDI.Comunications.MissoulaWrapper.BatchResultsAckSubmitTransactionRequest request);
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
        public DCSGlobal.EDI.Comunications.MissoulaWrapper.COREEnvelopeRealTimeRequest COREEnvelopeRealTimeRequest;

        public RealTimeTransactionRequest()
        {
        }

        public RealTimeTransactionRequest(DCSGlobal.EDI.Comunications.MissoulaWrapper.COREEnvelopeRealTimeRequest COREEnvelopeRealTimeRequest)
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
        public DCSGlobal.EDI.Comunications.MissoulaWrapper.COREEnvelopeRealTimeResponse COREEnvelopeRealTimeResponse;

        public RealTimeTransactionResponse()
        {
        }

        public RealTimeTransactionResponse(DCSGlobal.EDI.Comunications.MissoulaWrapper.COREEnvelopeRealTimeResponse COREEnvelopeRealTimeResponse)
        {
            this.COREEnvelopeRealTimeResponse = COREEnvelopeRealTimeResponse;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1087.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.caqh.org/SOAP/WSDL/CORERule2.2.0.xsd")]
    public partial class COREEnvelopeBatchSubmission : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string payloadTypeField;

        private string processingModeField;

        private string payloadIDField;

        private int payloadLengthField;

        private string timeStampField;

        private string senderIDField;

        private string receiverIDField;

        private string cORERuleVersionField;

        private string checkSumField;

        private byte[] payloadField;

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
        public int PayloadLength
        {
            get
            {
                return this.payloadLengthField;
            }
            set
            {
                this.payloadLengthField = value;
                this.RaisePropertyChanged("PayloadLength");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 4)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 5)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 6)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 7)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 8)]
        public string CheckSum
        {
            get
            {
                return this.checkSumField;
            }
            set
            {
                this.checkSumField = value;
                this.RaisePropertyChanged("CheckSum");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "base64Binary", Order = 9)]
        public byte[] Payload
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
    public partial class COREEnvelopeBatchSubmissionResponse : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string payloadTypeField;

        private string processingModeField;

        private string payloadIDField;

        private int payloadLengthField;

        private bool payloadLengthFieldSpecified;

        private string timeStampField;

        private string senderIDField;

        private string receiverIDField;

        private string cORERuleVersionField;

        private string checkSumField;

        private byte[] payloadField;

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
        public int PayloadLength
        {
            get
            {
                return this.payloadLengthField;
            }
            set
            {
                this.payloadLengthField = value;
                this.RaisePropertyChanged("PayloadLength");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PayloadLengthSpecified
        {
            get
            {
                return this.payloadLengthFieldSpecified;
            }
            set
            {
                this.payloadLengthFieldSpecified = value;
                this.RaisePropertyChanged("PayloadLengthSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 4)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 5)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 6)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 7)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 8)]
        public string CheckSum
        {
            get
            {
                return this.checkSumField;
            }
            set
            {
                this.checkSumField = value;
                this.RaisePropertyChanged("CheckSum");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "base64Binary", Order = 9)]
        public byte[] Payload
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 10)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 11)]
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
    public partial class BatchSubmitTransactionRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.caqh.org/SOAP/WSDL/CORERule2.2.0.xsd", Order = 0)]
        public DCSGlobal.EDI.Comunications.MissoulaWrapper.COREEnvelopeBatchSubmission COREEnvelopeBatchSubmission;

        public BatchSubmitTransactionRequest()
        {
        }

        public BatchSubmitTransactionRequest(DCSGlobal.EDI.Comunications.MissoulaWrapper.COREEnvelopeBatchSubmission COREEnvelopeBatchSubmission)
        {
            this.COREEnvelopeBatchSubmission = COREEnvelopeBatchSubmission;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class BatchSubmitTransactionResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.caqh.org/SOAP/WSDL/CORERule2.2.0.xsd", Order = 0)]
        public DCSGlobal.EDI.Comunications.MissoulaWrapper.COREEnvelopeBatchSubmissionResponse COREEnvelopeBatchSubmissionResponse;

        public BatchSubmitTransactionResponse()
        {
        }

        public BatchSubmitTransactionResponse(DCSGlobal.EDI.Comunications.MissoulaWrapper.COREEnvelopeBatchSubmissionResponse COREEnvelopeBatchSubmissionResponse)
        {
            this.COREEnvelopeBatchSubmissionResponse = COREEnvelopeBatchSubmissionResponse;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1087.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.caqh.org/SOAP/WSDL/CORERule2.2.0.xsd")]
    public partial class COREEnvelopeBatchSubmissionAckRetrievalRequest : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string payloadTypeField;

        private string processingModeField;

        private string payloadIDField;

        private int payloadLengthField;

        private bool payloadLengthFieldSpecified;

        private string timeStampField;

        private string senderIDField;

        private string receiverIDField;

        private string cORERuleVersionField;

        private string checkSumField;

        private byte[] payloadField;

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
        public int PayloadLength
        {
            get
            {
                return this.payloadLengthField;
            }
            set
            {
                this.payloadLengthField = value;
                this.RaisePropertyChanged("PayloadLength");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PayloadLengthSpecified
        {
            get
            {
                return this.payloadLengthFieldSpecified;
            }
            set
            {
                this.payloadLengthFieldSpecified = value;
                this.RaisePropertyChanged("PayloadLengthSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 4)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 5)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 6)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 7)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 8)]
        public string CheckSum
        {
            get
            {
                return this.checkSumField;
            }
            set
            {
                this.checkSumField = value;
                this.RaisePropertyChanged("CheckSum");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "base64Binary", Order = 9)]
        public byte[] Payload
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
    public partial class COREEnvelopeBatchSubmissionAckRetrievalResponse : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string payloadTypeField;

        private string processingModeField;

        private string payloadIDField;

        private int payloadLengthField;

        private bool payloadLengthFieldSpecified;

        private string timeStampField;

        private string senderIDField;

        private string receiverIDField;

        private string cORERuleVersionField;

        private string checkSumField;

        private byte[] payloadField;

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
        public int PayloadLength
        {
            get
            {
                return this.payloadLengthField;
            }
            set
            {
                this.payloadLengthField = value;
                this.RaisePropertyChanged("PayloadLength");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PayloadLengthSpecified
        {
            get
            {
                return this.payloadLengthFieldSpecified;
            }
            set
            {
                this.payloadLengthFieldSpecified = value;
                this.RaisePropertyChanged("PayloadLengthSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 4)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 5)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 6)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 7)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 8)]
        public string CheckSum
        {
            get
            {
                return this.checkSumField;
            }
            set
            {
                this.checkSumField = value;
                this.RaisePropertyChanged("CheckSum");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "base64Binary", Order = 9)]
        public byte[] Payload
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 10)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 11)]
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
    public partial class BatchSubmitAckRetrievalTransactionRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.caqh.org/SOAP/WSDL/CORERule2.2.0.xsd", Order = 0)]
        public DCSGlobal.EDI.Comunications.MissoulaWrapper.COREEnvelopeBatchSubmissionAckRetrievalRequest COREEnvelopeBatchSubmissionAckRetrievalRequest;

        public BatchSubmitAckRetrievalTransactionRequest()
        {
        }

        public BatchSubmitAckRetrievalTransactionRequest(DCSGlobal.EDI.Comunications.MissoulaWrapper.COREEnvelopeBatchSubmissionAckRetrievalRequest COREEnvelopeBatchSubmissionAckRetrievalRequest)
        {
            this.COREEnvelopeBatchSubmissionAckRetrievalRequest = COREEnvelopeBatchSubmissionAckRetrievalRequest;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class BatchSubmitAckRetrievalTransactionResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.caqh.org/SOAP/WSDL/CORERule2.2.0.xsd", Order = 0)]
        public DCSGlobal.EDI.Comunications.MissoulaWrapper.COREEnvelopeBatchSubmissionAckRetrievalResponse COREEnvelopeBatchSubmissionAckRetrievalResponse;

        public BatchSubmitAckRetrievalTransactionResponse()
        {
        }

        public BatchSubmitAckRetrievalTransactionResponse(DCSGlobal.EDI.Comunications.MissoulaWrapper.COREEnvelopeBatchSubmissionAckRetrievalResponse COREEnvelopeBatchSubmissionAckRetrievalResponse)
        {
            this.COREEnvelopeBatchSubmissionAckRetrievalResponse = COREEnvelopeBatchSubmissionAckRetrievalResponse;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1087.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.caqh.org/SOAP/WSDL/CORERule2.2.0.xsd")]
    public partial class COREEnvelopeBatchResultsRetrievalRequest : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string payloadTypeField;

        private string processingModeField;

        private string payloadIDField;

        private int payloadLengthField;

        private bool payloadLengthFieldSpecified;

        private string timeStampField;

        private string senderIDField;

        private string receiverIDField;

        private string cORERuleVersionField;

        private string checkSumField;

        private byte[] payloadField;

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
        public int PayloadLength
        {
            get
            {
                return this.payloadLengthField;
            }
            set
            {
                this.payloadLengthField = value;
                this.RaisePropertyChanged("PayloadLength");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PayloadLengthSpecified
        {
            get
            {
                return this.payloadLengthFieldSpecified;
            }
            set
            {
                this.payloadLengthFieldSpecified = value;
                this.RaisePropertyChanged("PayloadLengthSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 4)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 5)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 6)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 7)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 8)]
        public string CheckSum
        {
            get
            {
                return this.checkSumField;
            }
            set
            {
                this.checkSumField = value;
                this.RaisePropertyChanged("CheckSum");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "base64Binary", Order = 9)]
        public byte[] Payload
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
    public partial class COREEnvelopeBatchResultsRetrievalResponse : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string payloadTypeField;

        private string processingModeField;

        private string payloadIDField;

        private int payloadLengthField;

        private bool payloadLengthFieldSpecified;

        private string timeStampField;

        private string senderIDField;

        private string receiverIDField;

        private string cORERuleVersionField;

        private string checkSumField;

        private byte[] payloadField;

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
        public int PayloadLength
        {
            get
            {
                return this.payloadLengthField;
            }
            set
            {
                this.payloadLengthField = value;
                this.RaisePropertyChanged("PayloadLength");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PayloadLengthSpecified
        {
            get
            {
                return this.payloadLengthFieldSpecified;
            }
            set
            {
                this.payloadLengthFieldSpecified = value;
                this.RaisePropertyChanged("PayloadLengthSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 4)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 5)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 6)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 7)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 8)]
        public string CheckSum
        {
            get
            {
                return this.checkSumField;
            }
            set
            {
                this.checkSumField = value;
                this.RaisePropertyChanged("CheckSum");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "base64Binary", Order = 9)]
        public byte[] Payload
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 10)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 11)]
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
    public partial class BatchResultsRetrievalTransactionRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.caqh.org/SOAP/WSDL/CORERule2.2.0.xsd", Order = 0)]
        public DCSGlobal.EDI.Comunications.MissoulaWrapper.COREEnvelopeBatchResultsRetrievalRequest COREEnvelopeBatchResultsRetrievalRequest;

        public BatchResultsRetrievalTransactionRequest()
        {
        }

        public BatchResultsRetrievalTransactionRequest(DCSGlobal.EDI.Comunications.MissoulaWrapper.COREEnvelopeBatchResultsRetrievalRequest COREEnvelopeBatchResultsRetrievalRequest)
        {
            this.COREEnvelopeBatchResultsRetrievalRequest = COREEnvelopeBatchResultsRetrievalRequest;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class BatchResultsRetrievalTransactionResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.caqh.org/SOAP/WSDL/CORERule2.2.0.xsd", Order = 0)]
        public DCSGlobal.EDI.Comunications.MissoulaWrapper.COREEnvelopeBatchResultsRetrievalResponse COREEnvelopeBatchResultsRetrievalResponse;

        public BatchResultsRetrievalTransactionResponse()
        {
        }

        public BatchResultsRetrievalTransactionResponse(DCSGlobal.EDI.Comunications.MissoulaWrapper.COREEnvelopeBatchResultsRetrievalResponse COREEnvelopeBatchResultsRetrievalResponse)
        {
            this.COREEnvelopeBatchResultsRetrievalResponse = COREEnvelopeBatchResultsRetrievalResponse;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1087.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.caqh.org/SOAP/WSDL/CORERule2.2.0.xsd")]
    public partial class COREEnvelopeBatchResultsAckSubmission : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string payloadTypeField;

        private string processingModeField;

        private string payloadIDField;

        private int payloadLengthField;

        private bool payloadLengthFieldSpecified;

        private string timeStampField;

        private string senderIDField;

        private string receiverIDField;

        private string cORERuleVersionField;

        private string checkSumField;

        private byte[] payloadField;

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
        public int PayloadLength
        {
            get
            {
                return this.payloadLengthField;
            }
            set
            {
                this.payloadLengthField = value;
                this.RaisePropertyChanged("PayloadLength");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PayloadLengthSpecified
        {
            get
            {
                return this.payloadLengthFieldSpecified;
            }
            set
            {
                this.payloadLengthFieldSpecified = value;
                this.RaisePropertyChanged("PayloadLengthSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 4)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 5)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 6)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 7)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 8)]
        public string CheckSum
        {
            get
            {
                return this.checkSumField;
            }
            set
            {
                this.checkSumField = value;
                this.RaisePropertyChanged("CheckSum");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "base64Binary", Order = 9)]
        public byte[] Payload
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
    public partial class COREEnvelopeBatchResultsAckSubmissionResponse : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string payloadTypeField;

        private string processingModeField;

        private string payloadIDField;

        private int payloadLengthField;

        private bool payloadLengthFieldSpecified;

        private string timeStampField;

        private string senderIDField;

        private string receiverIDField;

        private string cORERuleVersionField;

        private string checkSumField;

        private byte[] payloadField;

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
        public int PayloadLength
        {
            get
            {
                return this.payloadLengthField;
            }
            set
            {
                this.payloadLengthField = value;
                this.RaisePropertyChanged("PayloadLength");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PayloadLengthSpecified
        {
            get
            {
                return this.payloadLengthFieldSpecified;
            }
            set
            {
                this.payloadLengthFieldSpecified = value;
                this.RaisePropertyChanged("PayloadLengthSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 4)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 5)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 6)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 7)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 8)]
        public string CheckSum
        {
            get
            {
                return this.checkSumField;
            }
            set
            {
                this.checkSumField = value;
                this.RaisePropertyChanged("CheckSum");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "base64Binary", Order = 9)]
        public byte[] Payload
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 10)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 11)]
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
    public partial class BatchResultsAckSubmitTransactionRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.caqh.org/SOAP/WSDL/CORERule2.2.0.xsd", Order = 0)]
        public DCSGlobal.EDI.Comunications.MissoulaWrapper.COREEnvelopeBatchResultsAckSubmission COREEnvelopeBatchResultsAckSubmission;

        public BatchResultsAckSubmitTransactionRequest()
        {
        }

        public BatchResultsAckSubmitTransactionRequest(DCSGlobal.EDI.Comunications.MissoulaWrapper.COREEnvelopeBatchResultsAckSubmission COREEnvelopeBatchResultsAckSubmission)
        {
            this.COREEnvelopeBatchResultsAckSubmission = COREEnvelopeBatchResultsAckSubmission;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class BatchResultsAckSubmitTransactionResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.caqh.org/SOAP/WSDL/CORERule2.2.0.xsd", Order = 0)]
        public DCSGlobal.EDI.Comunications.MissoulaWrapper.COREEnvelopeBatchResultsAckSubmissionResponse COREEnvelopeBatchResultsAckSubmissionResponse;

        public BatchResultsAckSubmitTransactionResponse()
        {
        }

        public BatchResultsAckSubmitTransactionResponse(DCSGlobal.EDI.Comunications.MissoulaWrapper.COREEnvelopeBatchResultsAckSubmissionResponse COREEnvelopeBatchResultsAckSubmissionResponse)
        {
            this.COREEnvelopeBatchResultsAckSubmissionResponse = COREEnvelopeBatchResultsAckSubmissionResponse;
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ICaqhCoreTransactionsChannel : DCSGlobal.EDI.Comunications.MissoulaWrapper.ICaqhCoreTransactions, System.ServiceModel.IClientChannel
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CaqhCoreTransactionsClient : System.ServiceModel.ClientBase<DCSGlobal.EDI.Comunications.MissoulaWrapper.ICaqhCoreTransactions>, DCSGlobal.EDI.Comunications.MissoulaWrapper.ICaqhCoreTransactions
    {

        public CaqhCoreTransactionsClient()
        {
        }

        public CaqhCoreTransactionsClient(string endpointConfigurationName) :
            base(endpointConfigurationName)
        {
        }

        public CaqhCoreTransactionsClient(string endpointConfigurationName, string remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public CaqhCoreTransactionsClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public CaqhCoreTransactionsClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
        {
        }

        public bool ValidateLogin(string userName, string password)
        {
            return base.Channel.ValidateLogin(userName, password);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        DCSGlobal.EDI.Comunications.MissoulaWrapper.RealTimeTransactionResponse DCSGlobal.EDI.Comunications.MissoulaWrapper.ICaqhCoreTransactions.RealTimeTransaction(DCSGlobal.EDI.Comunications.MissoulaWrapper.RealTimeTransactionRequest request)
        {
            return base.Channel.RealTimeTransaction(request);
        }

        public DCSGlobal.EDI.Comunications.MissoulaWrapper.COREEnvelopeRealTimeResponse RealTimeTransaction(DCSGlobal.EDI.Comunications.MissoulaWrapper.COREEnvelopeRealTimeRequest COREEnvelopeRealTimeRequest)
        {
            DCSGlobal.EDI.Comunications.MissoulaWrapper.RealTimeTransactionRequest inValue = new DCSGlobal.EDI.Comunications.MissoulaWrapper.RealTimeTransactionRequest();
            inValue.COREEnvelopeRealTimeRequest = COREEnvelopeRealTimeRequest;
            DCSGlobal.EDI.Comunications.MissoulaWrapper.RealTimeTransactionResponse retVal = ((DCSGlobal.EDI.Comunications.MissoulaWrapper.ICaqhCoreTransactions)(this)).RealTimeTransaction(inValue);
            return retVal.COREEnvelopeRealTimeResponse;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        DCSGlobal.EDI.Comunications.MissoulaWrapper.BatchSubmitTransactionResponse DCSGlobal.EDI.Comunications.MissoulaWrapper.ICaqhCoreTransactions.BatchSubmitTransaction(DCSGlobal.EDI.Comunications.MissoulaWrapper.BatchSubmitTransactionRequest request)
        {
            return base.Channel.BatchSubmitTransaction(request);
        }

        public DCSGlobal.EDI.Comunications.MissoulaWrapper.COREEnvelopeBatchSubmissionResponse BatchSubmitTransaction(DCSGlobal.EDI.Comunications.MissoulaWrapper.COREEnvelopeBatchSubmission COREEnvelopeBatchSubmission)
        {
            DCSGlobal.EDI.Comunications.MissoulaWrapper.BatchSubmitTransactionRequest inValue = new DCSGlobal.EDI.Comunications.MissoulaWrapper.BatchSubmitTransactionRequest();
            inValue.COREEnvelopeBatchSubmission = COREEnvelopeBatchSubmission;
            DCSGlobal.EDI.Comunications.MissoulaWrapper.BatchSubmitTransactionResponse retVal = ((DCSGlobal.EDI.Comunications.MissoulaWrapper.ICaqhCoreTransactions)(this)).BatchSubmitTransaction(inValue);
            return retVal.COREEnvelopeBatchSubmissionResponse;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        DCSGlobal.EDI.Comunications.MissoulaWrapper.BatchSubmitAckRetrievalTransactionResponse DCSGlobal.EDI.Comunications.MissoulaWrapper.ICaqhCoreTransactions.BatchSubmitAckRetrievalTransaction(DCSGlobal.EDI.Comunications.MissoulaWrapper.BatchSubmitAckRetrievalTransactionRequest request)
        {
            return base.Channel.BatchSubmitAckRetrievalTransaction(request);
        }

        public DCSGlobal.EDI.Comunications.MissoulaWrapper.COREEnvelopeBatchSubmissionAckRetrievalResponse BatchSubmitAckRetrievalTransaction(DCSGlobal.EDI.Comunications.MissoulaWrapper.COREEnvelopeBatchSubmissionAckRetrievalRequest COREEnvelopeBatchSubmissionAckRetrievalRequest)
        {
            DCSGlobal.EDI.Comunications.MissoulaWrapper.BatchSubmitAckRetrievalTransactionRequest inValue = new DCSGlobal.EDI.Comunications.MissoulaWrapper.BatchSubmitAckRetrievalTransactionRequest();
            inValue.COREEnvelopeBatchSubmissionAckRetrievalRequest = COREEnvelopeBatchSubmissionAckRetrievalRequest;
            DCSGlobal.EDI.Comunications.MissoulaWrapper.BatchSubmitAckRetrievalTransactionResponse retVal = ((DCSGlobal.EDI.Comunications.MissoulaWrapper.ICaqhCoreTransactions)(this)).BatchSubmitAckRetrievalTransaction(inValue);
            return retVal.COREEnvelopeBatchSubmissionAckRetrievalResponse;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        DCSGlobal.EDI.Comunications.MissoulaWrapper.BatchResultsRetrievalTransactionResponse DCSGlobal.EDI.Comunications.MissoulaWrapper.ICaqhCoreTransactions.BatchResultsRetrievalTransaction(DCSGlobal.EDI.Comunications.MissoulaWrapper.BatchResultsRetrievalTransactionRequest request)
        {
            return base.Channel.BatchResultsRetrievalTransaction(request);
        }

        public DCSGlobal.EDI.Comunications.MissoulaWrapper.COREEnvelopeBatchResultsRetrievalResponse BatchResultsRetrievalTransaction(DCSGlobal.EDI.Comunications.MissoulaWrapper.COREEnvelopeBatchResultsRetrievalRequest COREEnvelopeBatchResultsRetrievalRequest)
        {
            DCSGlobal.EDI.Comunications.MissoulaWrapper.BatchResultsRetrievalTransactionRequest inValue = new DCSGlobal.EDI.Comunications.MissoulaWrapper.BatchResultsRetrievalTransactionRequest();
            inValue.COREEnvelopeBatchResultsRetrievalRequest = COREEnvelopeBatchResultsRetrievalRequest;
            DCSGlobal.EDI.Comunications.MissoulaWrapper.BatchResultsRetrievalTransactionResponse retVal = ((DCSGlobal.EDI.Comunications.MissoulaWrapper.ICaqhCoreTransactions)(this)).BatchResultsRetrievalTransaction(inValue);
            return retVal.COREEnvelopeBatchResultsRetrievalResponse;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        DCSGlobal.EDI.Comunications.MissoulaWrapper.BatchResultsAckSubmitTransactionResponse DCSGlobal.EDI.Comunications.MissoulaWrapper.ICaqhCoreTransactions.BatchResultsAckSubmitTransaction(DCSGlobal.EDI.Comunications.MissoulaWrapper.BatchResultsAckSubmitTransactionRequest request)
        {
            return base.Channel.BatchResultsAckSubmitTransaction(request);
        }

        public DCSGlobal.EDI.Comunications.MissoulaWrapper.COREEnvelopeBatchResultsAckSubmissionResponse BatchResultsAckSubmitTransaction(DCSGlobal.EDI.Comunications.MissoulaWrapper.COREEnvelopeBatchResultsAckSubmission COREEnvelopeBatchResultsAckSubmission)
        {
            DCSGlobal.EDI.Comunications.MissoulaWrapper.BatchResultsAckSubmitTransactionRequest inValue = new DCSGlobal.EDI.Comunications.MissoulaWrapper.BatchResultsAckSubmitTransactionRequest();
            inValue.COREEnvelopeBatchResultsAckSubmission = COREEnvelopeBatchResultsAckSubmission;
            DCSGlobal.EDI.Comunications.MissoulaWrapper.BatchResultsAckSubmitTransactionResponse retVal = ((DCSGlobal.EDI.Comunications.MissoulaWrapper.ICaqhCoreTransactions)(this)).BatchResultsAckSubmitTransaction(inValue);
            return retVal.COREEnvelopeBatchResultsAckSubmissionResponse;
        }
    }
}
