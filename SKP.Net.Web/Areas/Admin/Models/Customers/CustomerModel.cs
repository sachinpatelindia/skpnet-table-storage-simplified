using System;

namespace SKP.Net.Web.Areas.Admin.Models.Customers
{
    public class CustomerModel
    {
        public string RowKey { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
        public DateTime? CreatedOnUtc { get; set; }
        public DateTime? UpdatedOnUtc { get; set; }
    }
}
