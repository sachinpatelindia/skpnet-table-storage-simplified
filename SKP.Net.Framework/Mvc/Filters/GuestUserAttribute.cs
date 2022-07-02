using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SKP.Net.Core.Domain.Customers;
using SKP.Net.Core.Domain.Tracking;
using SKP.Net.Services.Authentication;
using SKP.Net.Services.Customers;
using SKP.Net.Storage.Operations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SKP.Net.Web.Framework.Mvc.Filters
{

    public sealed class GuestUserAttribute : TypeFilterAttribute
    {
        public GuestUserAttribute() : base(typeof(VerifyAuthenticatedUserFilter))
        {

        }
        private class VerifyAuthenticatedUserFilter : IAuthorizationFilter
        {
            private readonly ICustomerService _customerService;
            private readonly ITableStorage<WebsiteVisitor> _webSiteVisitor;
            private readonly IAuthenticationService _authenticationService;
            private readonly ICustomerSettingService _customerSettingService;

            public VerifyAuthenticatedUserFilter(ITableStorage<WebsiteVisitor> webSiteVisitor,
                IAuthenticationService authenticationService,
                ICustomerService customerService,
                ICustomerSettingService customerSetting,
               IHttpContextAccessor httpContextAccessor)
            {
                _webSiteVisitor = webSiteVisitor;
                _customerService = customerService;
                _authenticationService = authenticationService;
                _customerSettingService = customerSetting;
            }
            public void OnAuthorization(AuthorizationFilterContext context)
            {
                if (context == null)
                    throw new ArgumentNullException(nameof(context));

                var ipAddress = context.HttpContext.Connection.RemoteIpAddress.ToString();
                bool isBlockedUser = _webSiteVisitor.GetAll<WebsiteVisitor>().Where(w => w.IsBlocked && w.IPAddress == ipAddress).Any();
                if (isBlockedUser)
                {
                    context.Result = new BadRequestResult();
                    return;
                }

                var users = _customerService.GetCustomers().Where(u => u.CreatedOnUtc.Date == DateTime.UtcNow.Date.AddDays(-1) && u.Email == "Guest");
                if (users.Any())
                {
                    users.ToList().ForEach(user =>
                    {
                        _customerService.Delete(user);
                    });
                }

                if (_authenticationService.GetAuthenticatedCustomer() == null)
                {
                    var customer = new Customer
                    {
                        Active = true,
                        AdminComment = "Guest user registration",
                        CreatedOnUtc = DateTime.UtcNow,
                        Email = "Guest",
                        IPAddress = ipAddress,
                        OTP = string.Empty,
                        OTPCount = 0,
                        UpdatedOnUtc = DateTime.UtcNow,
                        Password = "guestuser",
                    };

                    _customerService.Insert(customer);
                    _customerService.InsertRole(customer, RoleNames.GuestRole);
                    var newUser = _customerService.GetCustomer(customer.RowKey);
                    if (newUser != null)
                    {
                        var settings = new List<CustomerSettings>();
                        settings.Add(new CustomerSettings { Active = true, CreatedOnUtc = DateTime.UtcNow, Deleted = false, CutomerRowKey = newUser.RowKey, UpdatedOnUtc = DateTime.UtcNow, Url = "/profile", Name = "Profile", PurchasedServiceName = "Profile" });
                        settings.ForEach(setting =>
                        {
                            _customerSettingService.Insert(setting);
                        });
                        _authenticationService.SignIn(newUser, false);
                    }
                }
            }
        }
    }
}
