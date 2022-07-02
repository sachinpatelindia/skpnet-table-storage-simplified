using System;

namespace SKP.Net.Core.Domain.Pages
{
    public partial class PageData : BaseEntity
    {
        public PageData()
        {

            PartitionKey = nameof(PageData);
            RowKey = Guid.NewGuid().ToString();
        }
        public string Name { get; set; }
        public string PageHead { get; set; }
        public string PageBody { get; set; }
        public string ImageUrl { get; set; }
        public bool Active { get; set; }
        public int PageTypeId { get; set; }
        public string RedirectUrl { get; set; }
        public int DisplayOrder { get; set; }
        public PageDataType PageType
        {
            get => (PageDataType)PageTypeId;
            set => PageTypeId = (int)value;
        }
    }
}