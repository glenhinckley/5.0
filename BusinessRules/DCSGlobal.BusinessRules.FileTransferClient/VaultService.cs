using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DCSGlobal.BusinessRules.Logging;
using System.Configuration;
using System.IO;

namespace DCSGlobal.BusinessRules.FileTransferClient
{
    public class VaultService : IDisposable
    {


        private bool DCS_VAULT_OverWriteIfFileExist = false;
        private string DCS_VAULT_PassUserCredentials = string.Empty;
        private string DCS_VAULT_UserName = string.Empty;
        private string DCS_VAULT_Password = string.Empty;
        private string DCS_VAULT_Domain = string.Empty;
        private string DCS_VAULT_ERRORFOLDER = string.Empty;
        private string DCS_VAULT_AlternateEndPointAddress = string.Empty;




        bool _disposed;

        public VaultService(bool DCSVAULTOverWriteIfFileExist, string DCSVAULTPassUserCredentials, string DCSVAULTUserName,
            string DCSVAULTPassword, string DCSVAULTDomain, string DCSVAULTERRORFOLDER, string DCSVAULTAlternateEndPointAddress)
        {
            DCS_VAULT_OverWriteIfFileExist = DCSVAULTOverWriteIfFileExist;
            DCS_VAULT_PassUserCredentials = DCSVAULTPassUserCredentials;
            DCS_VAULT_UserName = DCSVAULTUserName;
            DCS_VAULT_Password = DCSVAULTPassword;
            DCS_VAULT_Domain = DCSVAULTDomain;
            DCS_VAULT_ERRORFOLDER = DCSVAULTERRORFOLDER;
            DCS_VAULT_AlternateEndPointAddress = DCSVAULTAlternateEndPointAddress;
        }

        ~VaultService()
        {
            Dispose(false);
        }

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


        private DCS_WCF_VaultService.FileTransferClient GetClientInstance(int Attempt)
        {
            try
            {
                DCS_WCF_VaultService.FileTransferClient ClientObject = new DCS_WCF_VaultService.FileTransferClient();
                if ((Attempt == 2))
                {
                    ClientObject.Endpoint.Address = new System.ServiceModel.EndpointAddress(new Uri(DCS_VAULT_AlternateEndPointAddress), ClientObject.Endpoint.Address.Identity, ClientObject.Endpoint.Address.Headers);
                }
                if ((DCS_VAULT_PassUserCredentials == "1"))
                {
                    ClientObject.ClientCredentials.Windows.ClientCredential.UserName = DCS_VAULT_UserName;
                    ClientObject.ClientCredentials.Windows.ClientCredential.Domain = DCS_VAULT_Domain;
                    ClientObject.ClientCredentials.Windows.ClientCredential.Password = DCSVaultEncryptDecrypt.Decrypt(DCS_VAULT_Password);
                }
                return ClientObject;
            }
            catch
            {
                throw;
            }
        }



        public byte[] DCSVaultGetFile(string ClientName, string HospCode, string RootDirectory, string ClientDirectroy, string FilePath, string XMLParams)
        {
            DCS_WCF_VaultService.FileTransferClient _ftc = default(DCS_WCF_VaultService.FileTransferClient);
            try
            {
                _ftc = GetClientInstance(1);
                return _ftc.GetFile(ClientName, HospCode, RootDirectory, ClientDirectroy, FilePath, XMLParams);
            }
            catch (Exception ex1)
            {
                try
                {
                    _ftc = GetClientInstance(2);
                    return _ftc.GetFile(ClientName, HospCode, RootDirectory, ClientDirectroy, FilePath, XMLParams);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }
        public void DCSVaultSaveFile(byte[] InputFile, string ClientName, string HospCode, string RootDirectory, string ClientDirectroy, string FilePath, string XMLParams)
        {
            bool overwirte = false;
            if ((DCS_VAULT_OverWriteIfFileExist == true))
            {
                overwirte = true;
            }
            DCS_WCF_VaultService.FileTransferClient _ftc = default(DCS_WCF_VaultService.FileTransferClient);
            try
            {
                _ftc = GetClientInstance(1);
                _ftc.SaveFile(InputFile, ClientName, HospCode, RootDirectory, ClientDirectroy, FilePath, overwirte, XMLParams);
            }
            catch (Exception ex1)
            {
                try
                {
                    _ftc = GetClientInstance(2);
                    _ftc.SaveFile(InputFile, ClientName, HospCode, RootDirectory, ClientDirectroy, FilePath, overwirte, XMLParams);
                }
                catch (Exception ex)
                {
                    CopyTovaultError(InputFile, FilePath);
                    throw ex;
                }
            }

        }

        private void CopyTovaultError(byte[] InputFile, string filePath)
        {
            try
            {
                if ((!Directory.Exists(Path.GetDirectoryName(DCS_VAULT_ERRORFOLDER + "\\" + filePath))))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(DCS_VAULT_ERRORFOLDER + "\\" + filePath));
                }
                File.WriteAllBytes(DCS_VAULT_ERRORFOLDER + "\\" + filePath, InputFile);
            }
            catch
            {
                throw;
            }
        }


    }
}
