using System;

namespace SKP.Net.Core.Domain.Order
{
    public partial class Order : BaseEntity
    {
        public Order()
        {
            PartitionKey = nameof(ShoppingCartItem);
            RowKey = Guid.NewGuid().ToString();
        }
        public string CustomerRowKey { get; set; }
        public string OrderNumber { get; set; }
        public decimal OrderTotal { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod
        {
            get => (PaymentMethod)PaymentMethodId;
            set => PaymentMethodId = (int)value;
        }
        public int PaymentStatusId { get; set; }
        public PaymentStatus PaymentStatus
        {
            get => (PaymentStatus)PaymentStatusId;
            set => PaymentStatusId = (int)value;
        }
    }
}
