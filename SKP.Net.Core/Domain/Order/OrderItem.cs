using System;

namespace SKP.Net.Core.Domain.Order
{
    public partial class OrderItem:BaseEntity
    {
        public OrderItem()
        {
            PartitionKey = nameof(ShoppingCartItem);
            RowKey = Guid.NewGuid().ToString();
        }
        public string OrderRowKey { get; set; }
        public string ProductRowKey { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
    }
}
