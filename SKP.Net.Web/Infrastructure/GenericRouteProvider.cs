using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using SKP.Net.Web.Framework.Mvc;
using SKP.Net.Web.Framework.Mvc.Routing;

namespace SKP.Net.Web.Infrastructure
{
    public partial class GenericRouteProvider : IRouteProvider
    {
        public int Priority => -10000;

        public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
        {
            var pattern = "{SeName}";
            endpointRouteBuilder.MapDynamicControllerRoute<SlugRouteTransformer>(pattern);
            endpointRouteBuilder.MapControllerRoute("Article", pattern, new { controller = "Articles", action = "ArticleDetails" });
            endpointRouteBuilder.MapControllerRoute("Learm", pattern, new { controller = "Learn", action = "LearnDetails" });
        }
    }
}
