using System;

namespace SKP.Net.Core.Domain.Documents
{
    public partial class Document:BaseEntity
    {
        public Document()
        {
            PartitionKey = nameof(Document);
            RowKey = Guid.NewGuid().ToString();
        }
        public string CustomerRowKey { get; set; }
        public string DocumentName { get; set; }
        public string DocumentUrl { get; set; }
        public bool Active { get; set; }
        public int Size { get; set; }
        public int DocumentTypeId { get; set; }
        public DocumentType DocumentType
        {
            get => (DocumentType)DocumentTypeId;
            set => DocumentTypeId = (int)value;
        }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
