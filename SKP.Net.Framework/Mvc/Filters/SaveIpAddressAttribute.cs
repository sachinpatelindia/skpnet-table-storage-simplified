using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SKP.Net.Core.Domain.Tracking;
using SKP.Net.Services.Authentication;
using SKP.Net.Services.Customers;
using SKP.Net.Storage.Operations;
using System;
using System.Linq;
namespace SKP.Net.Web.Framework.Mvc.Filters
{
    public sealed class SaveIpAddressAttribute : TypeFilterAttribute
    {
        public SaveIpAddressAttribute() : base(typeof(SaveIpAddressFilter))
        {

        }
        private class SaveIpAddressFilter : IActionFilter
        {
            private readonly ICustomerService _customerService;
            private readonly ITableStorage<WebsiteVisitor> _webSiteVisitor;
            private readonly IAuthenticationService _authenticationService;

            public SaveIpAddressFilter(ITableStorage<WebsiteVisitor> webSiteVisitor,
                IAuthenticationService authenticationService,
                ICustomerService customerService)
            {
                _webSiteVisitor = webSiteVisitor;
                _customerService = customerService;
                _authenticationService = authenticationService;
            }
            public void OnActionExecuted(ActionExecutedContext context)
            {

            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
                var user = _authenticationService.GetAuthenticatedCustomer();
                if (user == null)
                    return;
                var ipAddress = context.HttpContext.Connection.RemoteIpAddress.ToString();
                bool isBlockedUser = _webSiteVisitor.GetAll<WebsiteVisitor>().Where(w=>w.IsBlocked && w.IPAddress== ipAddress).Any();
                if (isBlockedUser)
                {
                    context.Result = new BadRequestResult();
                    return;
                }
    
                if ((_customerService.IsRegisteredRole(user) || _customerService.IsGuestRole(user)) && !_customerService.IsAdmin(user))
                {
                   var vistor= _webSiteVisitor.Insert(new WebsiteVisitor
                    {
                        IPAddress = ipAddress,
                        IsBlocked = false,
                        UserId = context.HttpContext.User.Identity.Name,
                        RouteName = context.HttpContext.Request.Path,
                        PageUrl = context.HttpContext.Request.Path,
                        HostName = context.HttpContext.Request.Host.Value,
                        VisitedDate = DateTime.UtcNow
                    });
       
                }
            }
        }
    }
}
