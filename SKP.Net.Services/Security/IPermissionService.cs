using SKP.Net.Core.Domain.Customers;
using SKP.Net.Core.Domain.Security;

namespace SKP.Net.Services.Security
{
    public interface IPermissionService
    {
        bool Authorize(Customer customer, PermissionProvider permissionProvider);
    }
}