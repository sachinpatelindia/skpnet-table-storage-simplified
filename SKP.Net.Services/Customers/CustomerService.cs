using SKP.Net.Core.Domain.Customers;
using SKP.Net.Storage.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
namespace SKP.Net.Services.Customers
{
    public partial class CustomerService : ICustomerService
    {
        private readonly ITableStorage<Customer> _customerStorage;
        private readonly ITableStorage<Role> _roleStorage;
        private readonly ITableStorage<CustomerRole> _customerRoleStorage;
        public CustomerService(ITableStorage<Customer> customerStorage,
            ITableStorage<CustomerRole> customerRoleStorage,
            ITableStorage<Role> roleStorage)
        {
            _customerStorage = customerStorage;
            _customerRoleStorage = customerRoleStorage;
            _roleStorage = roleStorage;
        }
        public IEnumerable<Customer> GetCustomers()
        {
            return _customerStorage.GetAll<Customer>();
        }

        public Customer GetCustomer(string key)
        {
            return _customerStorage.Get<Customer>(key);
        }
        public Customer Insert(Customer customer)
        {
            _customerStorage.Insert(customer);
            return customer;
        }

        public void Update(Customer customer)
        {
            _customerStorage.Update(customer);
        }

        public void Delete(Customer customer)
        {
            _customerStorage.Delete(customer);
        }

        public bool IsInCustomerRole(Customer customer, string systemRoleNames, bool onlyActiveRoles = true)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));
            if (string.IsNullOrEmpty(systemRoleNames))
                throw new ArgumentNullException(nameof(systemRoleNames));
            var role = GetCustomerRoles(customer);
            return role?.Any(m => m.Name == systemRoleNames && m.Active==onlyActiveRoles) ?? false;
        }

        public bool IsAdmin(Customer customer, bool onlyActiveRoles = true)
        {
            return IsInCustomerRole(customer, RoleNames.AdminRole);
        }

        public bool IsGuestRole(Customer customer, bool onlyActiveRoles = true)
        {
            return IsInCustomerRole(customer, RoleNames.GuestRole);
        }
        public bool IsRegisteredRole(Customer customer, bool onlyActiveRoles = true)
        {
            return IsInCustomerRole(customer, RoleNames.RegisteredRole);
        }

        public List<Role> GetCustomerRoles(Customer customer)
        {
            var query = from cr in _customerRoleStorage.GetAll<CustomerRole>().ToList()
                        join role in _roleStorage.GetAll<Role>() on cr.RoleRowKey equals role.RowKey
                        where cr.CustomerRowKey.Equals(customer.RowKey)
                        select role;
            return query.ToList();

        }

        public List<Role> GetAllCustomerRoles()
        {
            var query = from role in _roleStorage.GetAll<Role>()
                        orderby role.Name
                        select role;
            return query.ToList();
        }

        public virtual bool InsertRole(Customer customer, string roleName)
        {
            var role = _roleStorage.GetAll<Role>().Where(m => m.Name.Equals(RoleNames.GuestRole)).FirstOrDefault();
            var result = _customerRoleStorage.Insert(new CustomerRole { CustomerRowKey = customer.RowKey, RoleRowKey = role.RowKey });
            if (result != null)
                return true;
            return false;
        }

        public virtual bool UpdateRole(Customer customer, string roleName)
        {
            var role = _roleStorage.GetAll<Role>().Where(m => m.Name.Equals(roleName)).FirstOrDefault();
            var existingRole = _customerRoleStorage.GetAll<CustomerRole>().FirstOrDefault(c => c.CustomerRowKey == customer.RowKey);
            existingRole.RoleRowKey = role.RowKey;
            var result = _customerRoleStorage.Update(existingRole);
            if (result != null)
                return true;
            return false;
        }
    }
}
