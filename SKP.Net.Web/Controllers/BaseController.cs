using Microsoft.AspNetCore.Mvc;
using SKP.Net.Web.Framework.Mvc.Filters;

namespace SKP.Net.Web.Controllers
{
    [VerifyAuthenticatedUser]
    [CheckStorageAccount]
    [SaveIpAddress]
    [GuestUser]
    public abstract class BaseController : Controller
    {

    }
}
