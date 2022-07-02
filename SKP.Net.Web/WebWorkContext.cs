using Microsoft.AspNetCore.Http;
using SKP.Net.Core;
using SKP.Net.Core.Domain.Customers;
using SKP.Net.Services.Authentication;
using SKP.Net.Services.Customers;
using System;
using System.Linq;

namespace SKP.Net.Web
{
    public class WebWorkContext : IWorkContext
    {
        private readonly ICustomerService _customerService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthenticationService _authenticationService;
        private Customer _cachedCustomer;
        public WebWorkContext(ICustomerService customerService,
            IHttpContextAccessor httpContextAccessor,
            IAuthenticationService authenticationService)
        {
            _customerService = customerService;
            _httpContextAccessor = httpContextAccessor;
            _authenticationService = authenticationService;
        }
        public Customer CurrentCustomer
        {
            get
            {
                if (_cachedCustomer != null)
                    return _cachedCustomer;
                Customer customer = null;

                if (customer == null || !customer.Active)
                {
                    customer = _authenticationService.GetAuthenticatedCustomer();
                    if (customer != null)
                    {
                        _cachedCustomer = customer;
                        if (!IsAdmin)
                        {
                            IsAdmin = _customerService.IsAdmin(customer);
                        }
                        return customer;
                    }

                }
                return new Customer();
                //    if (customer == null || !customer.Active)
                //    {
                //        var customerCookie = GetCustomerCookie();
                //        if (!string.IsNullOrEmpty(customerCookie))
                //        {
                //            var customerByCookie = _customerService.GetCustomers().Where(m => m.RowKey.Equals(customerCookie)).FirstOrDefault();
                //            if (customerByCookie != null)
                //                customer = customerByCookie;
                //        }
                //    }

                //    if(customer==null || !customer.Active)
                //    {
                //        customer = new _customerService.Insert(new Customer {Email="guest@skpatel.net", Active=true, Password="@guest", CreatedOnUtc=DateTime.UtcNow, AdminComment="guest cutomer", BlobStorageName=string.Empty, UpdatedOnUtc = DateTime.UtcNow });
                //    }

                //    if (customer.Active)
                //    {
                //        SetCustomerCookie(customer.RowKey);
                //        _cachedCustomer = customer;
                //    }
                //    return _cachedCustomer;
            }
            set
            {

                _cachedCustomer = value;
            }

        }

        public bool IsAdmin { get; set; }

        private string GetCustomerCookie()
        {
            var cookie = $"{SkpAuthenticationDefaults.Prefix}{SkpAuthenticationDefaults.CustomerCookie}";
            return _httpContextAccessor.HttpContext.Request?.Cookies[cookie];
        }

        private void SetCustomerCookie(string customerrowkey)
        {
            if (_httpContextAccessor.HttpContext?.Response == null || _httpContextAccessor.HttpContext.Response.HasStarted)
                return;
            var cookie = $"{SkpAuthenticationDefaults.Prefix}{SkpAuthenticationDefaults.CustomerCookie}";
            _httpContextAccessor.HttpContext.Response?.Cookies.Delete(cookie);

            var cookieExpireTime = DateTime.Now.AddHours(1);
            if (string.IsNullOrEmpty(customerrowkey))
                cookieExpireTime = DateTime.Now.AddHours(-1);

            var options = new CookieOptions
            {
                HttpOnly = true,
                Expires = cookieExpireTime,
                Secure = true
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append(cookie, customerrowkey, options);

        }
    }
}
