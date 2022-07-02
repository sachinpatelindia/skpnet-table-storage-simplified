using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SKP.Net.Core.Domain.Tracking;
using SKP.Net.Services.Customers;
using SKP.Net.Storage.Operations;

namespace SKP.Net.Web.Framework.Mvc.Filters
{
    public sealed class VerifyAuthenticatedUserAttribute : TypeFilterAttribute
    {
        public VerifyAuthenticatedUserAttribute() : base(typeof(VerifyAuthenticatedUserFilter))
        {

        }
        private class VerifyAuthenticatedUserFilter : IAuthorizationFilter
        {
            private readonly ICustomerService _customerService;
            private readonly ITableStorage<WebsiteVisitor> _webSiteVisitor;
            private readonly IAuthenticationService _authenticationService;

            public VerifyAuthenticatedUserFilter(ITableStorage<WebsiteVisitor> webSiteVisitor,
                IAuthenticationService authenticationService,
                ICustomerService customerService,
               IHttpContextAccessor httpContextAccessor)
            {
                _webSiteVisitor = webSiteVisitor;
                _customerService = customerService;
                _authenticationService = authenticationService;
            }
            public void OnAuthorization(AuthorizationFilterContext context)
            {
                //if (context == null)
                //    throw new ArgumentNullException(nameof(context));
                //if (context.Filters.Any(arg => arg is VerifyAuthenticatedUserFilter))
                //    if (context.HttpContext.User.Identity.IsAuthenticated)
                //        context.Result = new RedirectToRouteResult("signout");
            }
        }
    }
}
