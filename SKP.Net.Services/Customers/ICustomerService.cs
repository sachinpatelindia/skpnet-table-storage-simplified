using SKP.Net.Core.Domain.Customers;
using System.Collections.Generic;

namespace SKP.Net.Services.Customers
{
    public interface ICustomerService
    {
        Customer GetCustomer(string key);
        IEnumerable<Customer> GetCustomers();
        Customer Insert(Customer customer);
        void Update(Customer customer);
        void Delete(Customer customer);
        //Role GetRole(string customerRowKey);
        bool IsAdmin(Customer customer, bool onlyActiveRoles=true);
        bool IsGuestRole(Customer customer, bool onlyActiveRoles = true);
        bool IsRegisteredRole(Customer customer, bool onlyActiveRoles = true);
        public bool InsertRole(Customer customer, string roleName);
        public bool UpdateRole(Customer customer, string roleName);
        List<Role> GetCustomerRoles(Customer customer);
        List<Role> GetAllCustomerRoles();
        bool IsInCustomerRole(Customer customer, string systemRoleNames,bool onlyActiveRoles=true);     
    }
}