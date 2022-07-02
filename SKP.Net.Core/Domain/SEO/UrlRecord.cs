using System;

namespace SKP.Net.Core.Domain.SEO
{
    public partial class UrlRecord:BaseEntity
    {
        public UrlRecord()
        {
            PartitionKey = nameof(UrlRecord);
            RowKey = Guid.NewGuid().ToString();
        }
        public string EntityRowKey { get; set; }
        public string EntityName { get; set; }
        public string Slug { get; set; }
        public bool IsActive { get; set; }
    }
}
