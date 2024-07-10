 
namespace DCSGlobal.EDI.Comunications.KentuckyMedicaidProxy
{


    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://HP.HPES.MED.Interfaces/RealTime", ConfigurationName = "KentuckyMedicaidProxy.RealTimeInterface")]
    public interface RealTimeInterface
    {

        [System.ServiceModel.OperationContractAttribute(Action = "http://HP.HPES.MED.Interfaces/RealTime/RealTimeInterface/GetData271277", ReplyAction = "http://HP.HPES.MED.Interfaces/RealTime/RealTimeInterface/GetData271277Response")]
        string GetData271277(string str270276);
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface RealTimeInterfaceChannel : DCSGlobal.EDI.Comunications.KentuckyMedicaidProxy.RealTimeInterface, System.ServiceModel.IClientChannel
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class RealTimeInterfaceClient : System.ServiceModel.ClientBase<DCSGlobal.EDI.Comunications.KentuckyMedicaidProxy.RealTimeInterface>, DCSGlobal.EDI.Comunications.KentuckyMedicaidProxy.RealTimeInterface
    {

        public RealTimeInterfaceClient()
        {
        }

        public RealTimeInterfaceClient(string endpointConfigurationName) :
            base(endpointConfigurationName)
        {
        }

        public RealTimeInterfaceClient(string endpointConfigurationName, string remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public RealTimeInterfaceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public RealTimeInterfaceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
        {
        }

        public string GetData271277(string str270276)
        {
            return base.Channel.GetData271277(str270276);
        }
    }
}
