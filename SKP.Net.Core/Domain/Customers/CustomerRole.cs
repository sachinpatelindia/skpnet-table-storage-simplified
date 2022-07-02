using System;

namespace SKP.Net.Core.Domain.Customers
{
    public partial class CustomerRole : BaseEntity
    {
        public CustomerRole()
        {
            PartitionKey = nameof(CustomerRole);
            RowKey = Guid.NewGuid().ToString();
        }
        public string CustomerRowKey { get; set; }
        public string RoleRowKey { get; set; }
    }
}
