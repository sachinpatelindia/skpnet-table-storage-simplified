using Microsoft.AspNetCore.Mvc;

namespace SKP.Net.Web.Components
{
    public class SidebarViewComponent : ViewComponent
    {
        public  IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
