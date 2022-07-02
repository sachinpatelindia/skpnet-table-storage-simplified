using System;
using System.ComponentModel.DataAnnotations;

namespace SKP.Net.Web.Areas.Admin.Models.Tracking
{
    public class TrackingModel
    {
        public string RowKey { get; set; }
        [Display(Name = "User Email")]
        public string UserId { get; set; }
        [Display(Name = "IP Address")]
        public string IPAddress { get; set; }
        [Display(Name = "User Blocked")]
        public bool IsBlocked { get; set; }
        [Display(Name = "Page Url")]
        public string PageUrl { get; set; }
        [Display(Name = "Date Visited")]
        public DateTime VisitedDate { get; set; }
    }
}
