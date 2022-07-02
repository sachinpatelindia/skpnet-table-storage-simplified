using Microsoft.AspNetCore.Mvc;
using SKP.Net.Core.Domain.Customers;
using SKP.Net.Services.Customers;
using SKP.Net.Web.Areas.Admin.Models.Customers;
using System;
using System.Linq;

namespace SKP.Net.Web.Areas.Admin.Controllers
{
    public class CustomersController : BaseAdminController
    {
        private readonly ICustomerService _customerService;
        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        public IActionResult Index()
        {
            var customers = _customerService.GetCustomers()
                .OrderBy(m => m.CreatedOnUtc).Select(m => new CustomerModel
                {
                    RowKey = m.RowKey,
                    CreatedOnUtc = m.CreatedOnUtc,
                    Email = m.Email,
                    Active = m.Active,
                    Password = m.Password,
                    UpdatedOnUtc = m.UpdatedOnUtc
                });
            return View(customers);
        }

        public IActionResult Create()
        {
            var model = new CustomerModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(CustomerModel model)
        {
            return View(model);
        }

        public IActionResult Edit(string rowKey)
        {
            var model = ToCutomerModel(rowKey);
            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(CustomerModel model)
        {
            if(ModelState.IsValid)
            {
                var customer = GetCustomer(model.RowKey);
                customer.Password = model.Password;
                customer.UpdatedOnUtc = DateTime.UtcNow;
                customer.Active = model.Active;
                customer.AdminComment = "Updated";
                customer.Email = model.Email;
                _customerService.Update(customer);
                return RedirectToAction("Index");
            }
            var failedModel = ToCutomerModel(model.RowKey);
            return View(failedModel);
        }

        public IActionResult Delete(string rowKey)
        {
            var customer = GetCustomer(rowKey);
            _customerService.Delete(customer);
            return RedirectToAction("Index");
        }

        private Customer GetCustomer(string rowKey)
        {
            return _customerService.GetCustomer(rowKey);
        }
        private CustomerModel ToCutomerModel(string rowKey)
        {
            var customer = GetCustomer(rowKey);
            return new CustomerModel
            {
                RowKey = customer.RowKey,
                CreatedOnUtc = customer.CreatedOnUtc,
                Email = customer.Email,
                Active = customer.Active,
                Password = customer.Password,
                UpdatedOnUtc = customer.UpdatedOnUtc

            };
        }
    }
}
