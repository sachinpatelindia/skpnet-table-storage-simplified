using SKP.Net.Core;
using SKP.Net.Core.Domain.Documents;
using System.Collections.Generic;

namespace SKP.Net.Services.documents
{
    public interface IDocumentService
    {
        void Delete(string containerName, string rowkey);
        List<Document> GetDocuments<T>() where T : BaseEntity;
        Document Upload<T>(string filePath, string containerName, string fileName, int size, DocumentType documentType) where T : BaseEntity;
    }
}