using System;

namespace SKP.Net.Core.Domain.Customers
{
    public partial class CustomerSettings : BaseEntity
    {
        public CustomerSettings()
        {
            PartitionKey = nameof(CustomerSettings);
            RowKey = Guid.NewGuid().ToString();
        }
        public string CutomerRowKey { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public string Url { get; set; }
        public bool Active { get; set; }
        public string PurchasedServiceId { get; set; }
        public string PurchasedServiceName { get; set; }
        public bool Deleted { get; set; }
        public DateTime UpdatedOnUtc { get; set; }
    }
}
