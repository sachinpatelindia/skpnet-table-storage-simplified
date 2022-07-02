using SKP.Net.Core.Domain.Customers;
using System.Collections.Generic;

namespace SKP.Net.Services.Customers
{
    public partial interface ICustomerSettingService
    {
        public void Insert(CustomerSettings settings);
        public void Update(CustomerSettings settings);
        public void Delete(CustomerSettings settings);
        CustomerSettings GetCustomerSettingById(string settingId);
        IEnumerable<CustomerSettings> CustomerSettings();
    }
}