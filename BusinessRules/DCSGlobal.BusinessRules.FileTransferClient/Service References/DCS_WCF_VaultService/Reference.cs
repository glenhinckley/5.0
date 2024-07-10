﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DCSGlobal.BusinessRules.FileTransferClient.DCS_WCF_VaultService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="DCS_WCF_VaultService.IFileTransfer")]
    public interface IFileTransfer {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFileTransfer/SaveFile", ReplyAction="http://tempuri.org/IFileTransfer/SaveFileResponse")]
        void SaveFile(byte[] inputFile, string ClientName, string HospCode, string RootDirectory, string ClientDirectory, string FilePath, bool overWrite, string XMLParams);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFileTransfer/GetFile", ReplyAction="http://tempuri.org/IFileTransfer/GetFileResponse")]
        byte[] GetFile(string ClientName, string HospCode, string RootDirectory, string ClientDirectory, string FilePath, string XMLParams);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IFileTransferChannel : DCSGlobal.BusinessRules.FileTransferClient.DCS_WCF_VaultService.IFileTransfer, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class FileTransferClient : System.ServiceModel.ClientBase<DCSGlobal.BusinessRules.FileTransferClient.DCS_WCF_VaultService.IFileTransfer>, DCSGlobal.BusinessRules.FileTransferClient.DCS_WCF_VaultService.IFileTransfer {
        
        public FileTransferClient() {
        }
        
        public FileTransferClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public FileTransferClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FileTransferClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FileTransferClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void SaveFile(byte[] inputFile, string ClientName, string HospCode, string RootDirectory, string ClientDirectory, string FilePath, bool overWrite, string XMLParams) {
            base.Channel.SaveFile(inputFile, ClientName, HospCode, RootDirectory, ClientDirectory, FilePath, overWrite, XMLParams);
        }
        
        public byte[] GetFile(string ClientName, string HospCode, string RootDirectory, string ClientDirectory, string FilePath, string XMLParams) {
            return base.Channel.GetFile(ClientName, HospCode, RootDirectory, ClientDirectory, FilePath, XMLParams);
        }
    }
}
