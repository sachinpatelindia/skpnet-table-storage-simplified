using System;

namespace SKP.Net.Web.Models.Articles
{
    public class ArticleModel
    {
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public DateTime CreatedOn { get; set; }
        public int Rank { get; set; }
        public string CreatedBy { get; set; }
    }
}
