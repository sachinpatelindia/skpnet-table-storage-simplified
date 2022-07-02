using System;

namespace SKP.Net.Core.Domain.Order
{
    public partial class ShoppingCartItem : BaseEntity
    {
        public ShoppingCartItem()
        {
            PartitionKey = nameof(ShoppingCartItem);
            RowKey = Guid.NewGuid().ToString();
        }
        public string CustomerRowKey { get; set; }
        public string ProductRowKey { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
