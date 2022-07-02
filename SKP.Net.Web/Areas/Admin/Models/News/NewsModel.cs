using System;
using System.ComponentModel.DataAnnotations;

namespace SKP.Net.Web.Areas.Admin.Models.News
{
    public class NewsModel
    {
        public string RowKey { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public bool Active { get; set; }
        public DateTime? CreatedOnUtc { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? ExpireOnUtc { get; set; }
    }
}
