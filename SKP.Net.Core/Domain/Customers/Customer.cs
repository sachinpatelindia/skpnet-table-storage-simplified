using System;

namespace SKP.Net.Core.Domain.Customers
{
    public partial class Customer : BaseEntity
    {
        public Customer()
        {
            PartitionKey = nameof(Customer);
            RowKey = Guid.NewGuid().ToString();
        }
        public string AdminComment { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string OTP { get; set; }
        public int OTPCount { get; set; }
        public string IPAddress { get; set; }
        public bool Active { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime UpdatedOnUtc { get; set; }
    }
}


