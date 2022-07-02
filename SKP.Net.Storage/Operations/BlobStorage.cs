using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using SKP.Net.Storage.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace SKP.Net.Storage.Operations
{
    public class BlobStorage : IBlobStorage
    {
        private readonly StorageAccountConnection _accountConnection;
        public BlobStorage(StorageAccountConnection connection)
        {
            _accountConnection = connection;
        }

        public string Upload(Stream stream, string containerName, string fileName)
        {
            var blobServiceClient = _accountConnection.CreateBlobStorageAccount();
            var blobContainer = blobServiceClient.GetBlobContainerClient(containerName);
            if (!blobContainer.Exists())
                blobContainer = blobServiceClient.CreateBlobContainer(containerName);
            var client = blobContainer.GetBlobClient(fileName);
            client.Upload(stream, true);
            stream.Close();
            return GetBlobUrl(containerName, fileName);
        }

        public string GetBlobUrl(string containerName, string fileName)
        {
            var blobContainer = _accountConnection.CreateBlobStorageAccount().GetBlobContainerClient(containerName);
            if (blobContainer.Exists())
            {
                var blob = blobContainer.GetBlobClient(fileName);
                var uri = GetServiceSasUriForBlob(blob);
                return uri.AbsoluteUri;
            }
            return string.Empty;
        }

        public List<BlobItem> GetBlobs(string containerName)
        {
            var blobContainer = _accountConnection.CreateBlobStorageAccount().GetBlobContainerClient(containerName);
            return blobContainer.GetBlobs().ToList();
        }
        private static Uri GetServiceSasUriForBlob(BlobClient blobClient,
            string storedPolicyName = null)
        {
            // Check whether this BlobClient object has been authorized with Shared Key.
            if (blobClient.CanGenerateSasUri)
            {
                // Create a SAS token that's valid for one hour.
                BlobSasBuilder sasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = blobClient.GetParentBlobContainerClient().Name,
                    BlobName = blobClient.Name,
                    Resource = "b"
                };

                if (storedPolicyName == null)
                {
                    sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddYears(10);
                    sasBuilder.SetPermissions(BlobSasPermissions.Read |
                        BlobSasPermissions.Write);
                }
                else
                {
                    sasBuilder.Identifier = storedPolicyName;
                }

                Uri sasUri = blobClient.GenerateSasUri(sasBuilder);
                Console.WriteLine("SAS URI for blob is: {0}", sasUri);
                Console.WriteLine();

                return sasUri;
            }
            else
            {
                Console.WriteLine(@"BlobClient must be authorized with Shared Key 
                          credentials to create a service SAS.");
                return null;
            }
        }

        public void Delete(string containerName, string blob)
        {
            try
            {
                var blobContainer = _accountConnection.CreateBlobStorageAccount().GetBlobContainerClient(containerName);
                if (blobContainer.Exists())
                    blobContainer.DeleteBlob(blob);
            }
            catch(Exception ex)
            {

            }
        }
    }
}
