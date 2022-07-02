using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SKP.Net.Web.Areas.Admin.Models.Articles
{
    public class ArticleModel
    {
        public ArticleModel()
        {
            Categories = new List<SelectListItem>();
        }
        public string RowKey { get; set; }
        [Display(Name ="Short Description"),Required]
        public string ShortDescription { get; set; }
        [Display(Name = "Full Description"), Required]
        public string FullDescription { get; set; }
        public string CustomerRowKey { get; set; }
        [Display(Name = "Active")]
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        [Display(Name = "Create Date")]
        public DateTime CreateOnUtc { get; set; }
        [Display(Name = "Update Date")]
        public DateTime UpdatedOnUtc { get; set; }
        public int DisplayOrder { get; set; }
        public int Rank { get; set; }
        public string CategoryRowKey { get; set; }
        public List<SelectListItem> Categories { get; set; }
    }
}
