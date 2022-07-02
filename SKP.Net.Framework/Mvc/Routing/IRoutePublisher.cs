using Microsoft.AspNetCore.Routing;

namespace SKP.Net.Web.Framework.Mvc.Routing
{
    public interface IRoutePublisher
    {
        void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder);
    }
}
