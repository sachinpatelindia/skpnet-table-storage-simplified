using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using SKP.Net.Services.SEO;
using SKP.Net.Web.Framework.Mvc.Routing;
using System.Threading.Tasks;

namespace SKP.Net.Web.Framework.Mvc
{
    public class SlugRouteTransformer : DynamicRouteValueTransformer
    {
        public IUrlRecordService _urlRecordService;
        public SlugRouteTransformer(IUrlRecordService urlRecordService)
        {
            this._urlRecordService = urlRecordService;
        }
        public override ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
        {
            if (values == null)
                return new ValueTask<RouteValueDictionary>(values);

            if (!values.TryGetValue("SeName", out var slugValue) || string.IsNullOrEmpty(slugValue as string))
                return new ValueTask<RouteValueDictionary>(values);

            var slug = slugValue as string;
            var urlRecord = _urlRecordService.GetBySlug(slug);
            if (urlRecord == null)
                return new ValueTask<RouteValueDictionary>(values);

            var pathBase = httpContext.Request.PathBase;

            if (!urlRecord.IsActive)
            {
                return new ValueTask<RouteValueDictionary>(values);
            }

            switch (urlRecord.EntityName.ToLowerInvariant())
            {
                case "article":
                    values[RoutePathDefaults.ControllerFieldKey] = "Articles";
                    values[RoutePathDefaults.ActionFieldKey] = "ArticleDetails";
                    values[RoutePathDefaults.ArticleIdFieldKey] = urlRecord.EntityRowKey;
                    values[RoutePathDefaults.SeNameFieldKey] = urlRecord.Slug;
                    break;

                case "category":
                    values[RoutePathDefaults.ControllerFieldKey] = "Category";
                    values[RoutePathDefaults.ActionFieldKey] = "CategoryDetails";
                    values[RoutePathDefaults.CategoryIdFieldKey] = urlRecord.EntityRowKey;
                    values[RoutePathDefaults.SeNameFieldKey] = urlRecord.Slug;
                    break;
                default:
                    break;
            }

            return new ValueTask<RouteValueDictionary>(values);
        }
    }
}
