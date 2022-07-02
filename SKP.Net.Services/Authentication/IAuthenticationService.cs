using SKP.Net.Core.Domain.Customers;

namespace SKP.Net.Services.Authentication
{
    public partial interface IAuthenticationService
    {
        void SignIn(Customer customer, bool isPersistent);
        void SignOut();
        Customer GetAuthenticatedCustomer();
    }
}