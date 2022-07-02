using SKP.Net.Core.Domain.Categories;
using System;

namespace SKP.Net.Web.Areas.Admin.Models.Categories
{
    public class CategoryModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string SeName { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public int DisplayOrder { get; set; }
        public Guid ParentCategoryId { get; set; }
        public int CategoryTypeId { get; set; }
        public DateTime CreatedOnUtc { get; internal set; }
        public DateTime UpdatedOnUtc { get; internal set; }
        public bool Deleted { get; internal set; }
        public bool Active { get; internal set; }
        public bool Published { get; internal set; }
        public bool IncludeOnTopMenu { get; internal set; }
        public bool ShowOnHomePage { get; internal set; }
        public CategoryType CategoryType
        {
            get => (CategoryType)CategoryTypeId;
            set => CategoryTypeId = (int)value;
        }
    }
}
