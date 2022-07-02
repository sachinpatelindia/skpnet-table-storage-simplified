using System;

namespace SKP.Net.Core.Domain.News
{
    public partial class News : BaseEntity
    {
        public News()
        {
            PartitionKey = nameof(News);
            RowKey = Guid.NewGuid().ToString();
        }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime ExpireOnUtc { get; set; }
    }
}
