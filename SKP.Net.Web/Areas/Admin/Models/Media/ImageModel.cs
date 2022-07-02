using SKP.Net.Core.Domain.Media;
using System;
using System.Collections.Generic;

namespace SKP.Net.Web.Areas.Admin.Models.Media
{
    public class ImageModel
    {
        public string RowKey { get; set; }
        public string FileName { get; set; }
        public long Size { get; set; }
        public string ImageUrl { get; set; }
        public bool Active { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ImageType { get; set; }
        public List<ImageType> ImageTypes { get; set; }
    }
}
