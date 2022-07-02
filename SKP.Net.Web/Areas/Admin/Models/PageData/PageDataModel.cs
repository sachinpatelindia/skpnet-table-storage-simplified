using Microsoft.AspNetCore.Http;
using SKP.Net.Core.Domain.Pages;

namespace SKP.Net.Web.Areas.Admin.Models.PageData
{
    public class PageDataModel
    {
        public string RowKey { get; set; }
        public string Name { get; set; }
        public string PageHead { get; set; }
        public string PageBody { get; set; }
        public string ImageUrl { get; set; }
        public string RedirectUrl { get; set; }
        public IFormFile File { get; set; }
        public bool Active { get; set; }
        public int PageTypeTypeId { get; set; }
        public int MeasureId { get; set; }
        public int DisplayOrder { get; set; }
        public PageDataType PageType
        {
            get => (PageDataType)PageTypeTypeId;
            set => PageTypeTypeId = (int)value;
        }
    }
}
