using System;

namespace SKP.Net.Web.Areas.Admin.Models.Orders
{
    public class OrderAdminModel
    {
        public string OrderRowKey { get; set; }
        public string OrderNumber { get; set; }
        public decimal OrderTotal { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
