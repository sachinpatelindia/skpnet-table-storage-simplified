using System;

namespace SKP.Net.Core.Domain.Tracking
{
    public class WebsiteVisitor : BaseEntity
    {
        public WebsiteVisitor()
        {

            PartitionKey = nameof(WebsiteVisitor);
            RowKey = Guid.NewGuid().ToString();

        }
        public string UserId { get; set; }
        public string IPAddress { get; set; }
        public bool IsBlocked { get; set; }
        public string PageUrl { get; set; }
        public string RouteName { get; set; }
        public string HostName { get; set; }
        public DateTime VisitedDate { get; set; }
    }
}
