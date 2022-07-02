using Microsoft.AspNetCore.Mvc;

namespace SKP.Net.Web.Components
{
    public class TopNavigationViewComponent : ViewComponent
    {
        public  IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
