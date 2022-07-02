using Microsoft.AspNetCore.Mvc;
using SKP.Net.Web.Areas.Admin.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKP.Net.Web.Areas.Admin.Controllers
{
    public class PermissionController : BaseAdminController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            var model = new PermissionModel();
            return View(model);
        }
    }
}
