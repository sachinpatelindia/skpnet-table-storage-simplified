using Microsoft.AspNetCore.Mvc;
using SKP.Net.Web.Filters;
using SKP.Net.Web.Framework.Mvc.Filters;

namespace SKP.Net.Web.Areas.Admin.Controllers
{

    [Area("Admin")]
    [AdminAccess]
    [SaveIpAddress]
    public class BaseAdminController : Controller
    {
    }
}
