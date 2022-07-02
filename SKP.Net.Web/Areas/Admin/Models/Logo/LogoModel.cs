using Microsoft.AspNetCore.Http;
using SKP.Net.Core.Domain.Media;
using System;

namespace SKP.Net.Web.Areas.Admin.Models.Logo
{
    public class LogoModel
    {
        public string RowKey { get; set; }
        public string FileName { get; set; }
        public string ImageUrl { get; set; }
        public bool Active { get; set; }
        public DateTime? CreatedOn { get; set; }
        public ImageType ImageType { get; set; }
        public IFormFile File { get; set; }
    }
}
