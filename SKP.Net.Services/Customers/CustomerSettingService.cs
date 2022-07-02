using SKP.Net.Core.Domain.Customers;
using SKP.Net.Storage.Operations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SKP.Net.Services.Customers
{
    public partial class CustomerSettingService : ICustomerSettingService
    {
        private readonly ITableStorage<CustomerSettings> _customerSettings;
        public CustomerSettingService(ITableStorage<CustomerSettings> customerSettings)
        {
            _customerSettings = customerSettings;
        }
        public IEnumerable<CustomerSettings> CustomerSettings()
        {
            return _customerSettings.GetAll<CustomerSettings>();
        }

        public void Delete(CustomerSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));
            _customerSettings.Delete(settings);
        }

        public CustomerSettings GetCustomerSettingById(string settingId)
        {
            return _customerSettings.GetAll<CustomerSettings>().FirstOrDefault(s=>s.RowKey==settingId);
        }

        public void Insert(CustomerSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));
            _customerSettings.Insert(settings);
        }

        public void Update(CustomerSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));
            _customerSettings.Update(settings);
        }
    }
}
