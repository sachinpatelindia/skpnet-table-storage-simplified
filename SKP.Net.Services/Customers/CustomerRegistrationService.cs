using SKP.Net.Core.Domain.Customers;
using SKP.Net.Storage.Operations;
using System;
using System.Linq;

namespace SKP.Net.Services.Customers
{
    public class CustomerRegistrationService : ICustomerRegistrationService
    {
        private readonly ITableStorage<Customer> _customerStorage;
        private readonly ITableStorage<Role> _roleStorage;
        private readonly ITableStorage<CustomerRole> _customerRoleStorage;
        private readonly ICustomerService _customerService;
        public CustomerRegistrationService(ITableStorage<Customer> customerStorage,
            ITableStorage<CustomerRole> customerRoleStorage,
            ICustomerService customerService,
            ITableStorage<Role> roleStorage)
        {
            _customerStorage = customerStorage;
            _roleStorage = roleStorage;
            _customerService = customerService;
            _customerRoleStorage = customerRoleStorage;
        }

        public void Register(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));
            var cutomer = _customerService.Insert(customer);
            var role = _roleStorage.GetAll<Role>().Where(m => m.Name.Equals(RoleNames.RegisteredRole)).FirstOrDefault();
            _customerRoleStorage.Insert(new CustomerRole { CustomerRowKey = customer.RowKey, RoleRowKey = role.RowKey });
        }
    }
}
