using Microsoft.AspNetCore.Mvc;
using SKP.Net.Core.Domain.Customers;
using SKP.Net.Storage.Operations;
using SKP.Net.Web.Areas.Admin.Models.Customers;
using System.Linq;

namespace SKP.Net.Web.Areas.Admin.Controllers
{
    public class RoleController : BaseAdminController
    {
        private readonly ITableStorage<Customer> _customerStorage;
        private readonly ITableStorage<Role> _roleStorage;
        public RoleController(ITableStorage<Customer> customerStorage,
            ITableStorage<Role> roleStorage)
        {
            _customerStorage = customerStorage;
            _roleStorage = roleStorage;
        }
        public IActionResult Index()
        {
            var model = _roleStorage.GetAll<Role>().Select(m => new RoleModel
            {
                CustomerRowKey = null,
                Active = m.Active,
                Name = m.Name
            });
            return View(model);
        }

        public IActionResult Create()
        {
            return View(new RoleModel());
        }

        [HttpPost]
        public IActionResult Create(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                _roleStorage.Insert(new Role
                {
                    Active = model.Active,
                    Name = model.Name
                });

                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
