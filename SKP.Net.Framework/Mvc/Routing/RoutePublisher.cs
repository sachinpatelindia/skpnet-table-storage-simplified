using Microsoft.AspNetCore.Routing;
using System;
using System.Linq;

namespace SKP.Net.Web.Framework.Mvc.Routing
{
    public class RoutePublisher:IRoutePublisher
    {
        public virtual void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
        {
            var type = typeof(IRouteProvider);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(t => type.IsAssignableFrom(t) && !(t.IsInterface));
            var instances = types.Select(p => (IRouteProvider)Activator.CreateInstance(p))
                .OrderByDescending(p => p.Priority);

            foreach (var item in instances)
                item.RegisterRoutes(endpointRouteBuilder);
        }
    }
}
