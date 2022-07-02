using Microsoft.AspNetCore.Mvc;

namespace SKP.Net.Web.Controllers
{
    public class LearnController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LearnDetails(string learnId)
        {
            return View(learnId);
        }
    }
}
