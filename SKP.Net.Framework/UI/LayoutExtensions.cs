using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SKP.Net.Web.Framework.UI
{
    public static class LayoutExtensions
    {
        public static IHtmlContent SKPTitle(this IHtmlHelper html)
        {
            //var pageBuilder=
            return new HtmlString(html.Encode(""));

        }
    }
}
