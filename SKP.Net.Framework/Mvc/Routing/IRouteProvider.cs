using Microsoft.AspNetCore.Routing;

namespace SKP.Net.Web.Framework.Mvc.Routing
{
    public partial interface IRouteProvider
    {
        void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder);
        int Priority { get; }
    }
}
