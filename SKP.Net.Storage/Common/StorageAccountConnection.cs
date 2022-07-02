using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Microsoft.WindowsAzure.Storage;
using System;
namespace SKP.Net.Storage.Common
{
    /// <summary>
    /// Create table and connection
    /// </summary>
    public class StorageAccountConnection
    {
        private readonly string _storageAccountConnectionString;
        private CloudStorageAccount cloudStorageAccount;
        public StorageAccountConnection(string storageAccountConnectionString)
        {
            _storageAccountConnectionString = storageAccountConnectionString;
        }

        /// <summary>
        /// Create storage account
        /// </summary>
        /// <returns></returns>
        public CloudStorageAccount CreateStorageAccount()
        {
            TableSeviceClient();
            try
            {
                cloudStorageAccount = CloudStorageAccount.Parse(_storageAccountConnectionString);
            }
            catch (FormatException ex)
            {
                //Logging info
            }

            return cloudStorageAccount;
        }

        public TableServiceClient TableSeviceClient()
        {
            var serviceClient = new TableServiceClient(_storageAccountConnectionString);
            return serviceClient;
        }

        public BlobServiceClient CreateBlobStorageAccount()
        {
            return new BlobServiceClient(_storageAccountConnectionString);
        }
    }
}