using SKP.Net.Core.Domain.Customers;
using SKP.Net.Core.Domain.Security;
using SKP.Net.Storage.Operations;
using System.Linq;

namespace SKP.Net.Services.Security
{
    public class PermissionService : IPermissionService
    {
        private readonly ITableStorage<PermissionRecord> _permissionRecordStorage;
        private readonly ITableStorage<Role> _roleStorage;
        private readonly ITableStorage<CustomerRole> _customerRoleStorage;
        public PermissionService(ITableStorage<PermissionRecord> permissionRecordStorage,
            ITableStorage<CustomerRole> customerRoleStorage,
            ITableStorage<Role> roleStorage)
        {
            _permissionRecordStorage = permissionRecordStorage;
            _customerRoleStorage = customerRoleStorage;
            _roleStorage = roleStorage;
        }

        public bool Authorize(Customer customer, PermissionProvider permissionProvider)
        {
            if (!customer.Active)
                return false;
            var query = from cr in _customerRoleStorage?.GetAll<CustomerRole>()
                        join role in _roleStorage?.GetAll<Role>() on cr?.RoleRowKey equals role?.RowKey
                        join permission in _permissionRecordStorage?.GetAll<PermissionRecord>() on role?.RowKey equals permission?.RoleRowKey
                        where cr?.CustomerRowKey == customer?.RowKey
                        select permission;

            if (query.Any())
                return true;

            return false;
        }
    }
}
