using SKP.Net.Core.Domain.Customers;

namespace SKP.Net.Services.Customers
{
    public interface ICustomerRegistrationService
    {
        void Register(Customer customer);
    }
}