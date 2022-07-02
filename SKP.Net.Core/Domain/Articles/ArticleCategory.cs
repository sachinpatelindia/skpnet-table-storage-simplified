using System;

namespace SKP.Net.Core.Domain.Articles
{
    public partial class ArticleCategory:BaseEntity
    {
        public ArticleCategory()
        {
            PartitionKey = nameof(ArticleCategory);
            RowKey = Guid.NewGuid().ToString();
        }
        public string ArticleRowKey { get; set; }
        public string CategoryRowKey { get; set; }
    }
}
