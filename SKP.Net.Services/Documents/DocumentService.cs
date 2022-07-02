using SKP.Net.Core;
using SKP.Net.Core.Domain.Documents;
using SKP.Net.Storage.Operations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SKP.Net.Services.documents
{
    public class DocumentService : IDocumentService
    {
        private readonly IBlobStorage _blobStorage;
        private readonly ITableStorage<Document> _documentStorage;
        public DocumentService(IBlobStorage blobStorage, ITableStorage<Document> documentStorage)
        {
            _blobStorage = blobStorage;
            _documentStorage = documentStorage;
        }

        public void Delete(string containerName, string rowkey)
        {
            var document = _documentStorage.GetAll<Document>().FirstOrDefault(m => m.RowKey == rowkey);
            if (document != null)
            {
                _blobStorage.Delete(containerName, document.DocumentName);
                _documentStorage.Delete(document);
            }
        }

        public List<Document> GetDocuments<T>() where T : BaseEntity
        {
            var documents = _documentStorage.GetAll<Document>();
            return documents.Select(document => new Document
            {
                CreatedOn = document.CreatedOn,
                DocumentName = document.DocumentName,
                DocumentType = document.DocumentType,
                DocumentUrl = document.DocumentUrl,
                Active = document.Active,
                RowKey = document.RowKey,
                UpdatedOn = document.UpdatedOn,
                PartitionKey = document.PartitionKey,
                Size = document.Size
            }).ToList();

        }

        public Document Upload<T>(string filePath, string containerName, string fileName, int size, DocumentType documentType) where T : BaseEntity
        {

            using FileStream uploadFileStream = File.OpenRead(filePath);
            var fileExtension = Path.GetExtension(fileName);
            var guid = Convert.ToString(Guid.NewGuid());
            var newFileName = String.Concat(guid, fileExtension);
            var typeName = typeof(T).Name;
            var existingdocument = _documentStorage.GetAll<Document>().FirstOrDefault(m => m.DocumentName.Equals(fileName));

            if (existingdocument == null)
            {
                var documentUrl = _blobStorage.Upload(uploadFileStream, containerName, newFileName);
                return _documentStorage.Insert(new Document
                {
                    CreatedOn = DateTime.UtcNow,
                    DocumentName = fileName,
                    DocumentType = documentType,
                    DocumentUrl = documentUrl,
                    Active = false,
                    UpdatedOn = DateTime.UtcNow,
                    PartitionKey = typeName,
                    Size = size
                });
            }
            return null;
        }

    }
}
