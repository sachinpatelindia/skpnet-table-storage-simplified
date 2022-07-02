using System;

namespace SKP.Net.Core.Domain.Articles
{
    public partial class Article : BaseEntity
    {
        public Article()
        {
            PartitionKey = nameof(Article);
            RowKey = Guid.NewGuid().ToString();
        }
        public string CustomerRowKey { get; set; }
        public string ArticleCategoryId { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public bool Active { get; set; }
        public bool Published { get; set; }
        public bool ShowOnHomePage { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreateOnUtc{get;set;}
        public DateTime UpdatedOnUtc { get; set; }
        public int Rank { get; set; }
        public int DisplayOrder { get; set; }
    }
}
