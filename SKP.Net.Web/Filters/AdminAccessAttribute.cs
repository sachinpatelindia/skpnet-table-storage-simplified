using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SKP.Net.Core;
using SKP.Net.Core.Domain.Security;
using SKP.Net.Services.Security;
using System;
using System.Linq;

namespace SKP.Net.Web.Filters
{
    public sealed class AdminAccessAttribute : TypeFilterAttribute
    {
        private readonly bool _ignoreFilter;
        public AdminAccessAttribute(bool ignore = false) : base(typeof(AdminAccessFilter))
        {
            _ignoreFilter = ignore;
            Arguments = new object[] { ignore };
        }

        public bool IgnoreFilter => _ignoreFilter;
        private class AdminAccessFilter : IAuthorizationFilter
        {
            private readonly bool _ignoreFilter;
            private readonly IPermissionService _permissionService;
            private readonly IWorkContext _workcontext;

            public AdminAccessFilter(bool ignoreFilter, IPermissionService permissionService, IWorkContext workContext)
            {
                _ignoreFilter = ignoreFilter;
                _permissionService = permissionService;
                _workcontext = workContext;
            }
            public void OnAuthorization(AuthorizationFilterContext context)
            {
                if (context == null)
                    throw new ArgumentNullException(nameof(context));

                var actionFilter = context.ActionDescriptor.FilterDescriptors
                    .Where(fd => fd.Scope == FilterScope.Action)
                    .Select(fd=>fd.Filter).OfType<AdminAccessAttribute>().FirstOrDefault();

                if (actionFilter?.IgnoreFilter ?? _ignoreFilter)
                    return;

                if (context.Filters.Any(f => f is AdminAccessFilter))
                {
                    if (!_permissionService.Authorize(_workcontext.CurrentCustomer, PermissionProvider.AccessAdminArea))
                    {
                        context.Result = new ChallengeResult();
                    }
                }
            }
        }
    }
}
