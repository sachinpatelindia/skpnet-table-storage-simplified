using System;

namespace SKP.Net.Core.Domain.Documents
{
    public partial class DocumentSettings:BaseEntity
    {
        public DocumentSettings()
        {
            PartitionKey = nameof(DocumentSettings);
            RowKey = Guid.NewGuid().ToString();
        }
        public string DocumentName { get; set; }
        public bool IsFile { get; set; }

    }
}
