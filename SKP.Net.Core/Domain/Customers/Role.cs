using System;

namespace SKP.Net.Core.Domain.Customers
{
    public partial class Role : BaseEntity
    {
        public Role()
        {
            PartitionKey = nameof(Role);
            RowKey = Guid.NewGuid().ToString();
        }
        public string Name { get; set; }
        public bool Active { get; set; }

    }
}
