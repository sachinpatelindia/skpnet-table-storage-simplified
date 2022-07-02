using SKP.Net.Core.Domain.Customers;
using SKP.Net.Services.Customers;
using System.Collections.Generic;
using System.Linq;

namespace SKP.Net.Web.Extensions
{
    public static class CustomerSettingsExtensions
    {
        public static IEnumerable<CustomerSettings> GetSettings(this Customer customer,ICustomerSettingService customerSetting)
        {
            return customerSetting.CustomerSettings().Where(s=>s.CutomerRowKey==customer.RowKey);
        }
    }
}
