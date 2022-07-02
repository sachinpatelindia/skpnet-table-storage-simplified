using Azure.Storage.Blobs.Models;
using System.Collections.Generic;
using System.IO;

namespace SKP.Net.Storage.Operations
{
    public interface IBlobStorage
    {
        string GetBlobUrl(string containerName, string fileName);
        List<BlobItem> GetBlobs(string containerName);
        string Upload(Stream stream, string containerName, string fileName);
        void Delete(string containerName,string blob);
    }
}