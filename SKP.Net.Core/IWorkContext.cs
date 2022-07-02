using SKP.Net.Core.Domain.Customers;

namespace SKP.Net.Core
{
    public interface IWorkContext
    {
        Customer CurrentCustomer { get; set; }
        public bool IsAdmin { get; set; }
    }
}
