using Microsoft.AspNetCore.Mvc;

namespace SKP.Net.Web.Controllers
{
    public class CommonController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
