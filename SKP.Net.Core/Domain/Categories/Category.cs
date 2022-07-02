using System;

namespace SKP.Net.Core.Domain.Categories
{
    public partial class Category : BaseEntity
    {
        public Category()
        {
            PartitionKey = nameof(Category);
            RowKey = Guid.NewGuid().ToString();
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryTypeId { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public Guid ParentCategoryId { get; set; }
        public bool ShowOnHomePage { get; set; }
        public bool IncludeOnTopMenu { get; set; }
        public bool Published { get; set; }
        public bool Deleted { get; set; }
        public int DisplayOrder { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime UpdatedOnUtc { get; set; }
        public CategoryType CategoryType
        {
            get => (CategoryType)CategoryTypeId;
            set => CategoryTypeId = (int)value;
        }
    }
}
