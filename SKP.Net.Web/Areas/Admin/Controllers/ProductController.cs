using Microsoft.AspNetCore.Mvc;
using SKP.Net.Core.Domain.Categories;
using SKP.Net.Core.Domain.Media;
using SKP.Net.Services.Categories;
using SKP.Net.Services.Images;
using SKP.Net.Storage.Operations;
using SKP.Net.Web.Areas.Admin.Models.Categories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SKP.Net.Web.Areas.Admin.Controllers
{
    public class ProductController : BaseAdminController
    {
        private readonly IProductServics _productService;
        public ProductController(IProductServics proudctService)
        {
            _productService = proudctService;
        }
        public IActionResult Index()
        {
            var products = _productService.GetProducts();
            var models = new List<ProductModel>();
            foreach (var product in products)
            {
                models.Add(new ProductModel
                {
                    Active = product.Active,
                    Description = product.Description,
                    ImageUrl = product.ImageUrl,
                    Name = product.Name,
                    Price = product.Price,
                    Measure = (Measure)product.MeasureId,
                    ProductType = (ProductType)product.ProductTypeId,
                    RowKey=product.RowKey
                });
            }
            return View(models);
        }

        public IActionResult Create()
        {
            var model = new ProductModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                var file = Request.Form.Files?.FirstOrDefault();
                var product = new Product
                {
                    Active = model.Active,
                    Name = model.Name,
                    Description = model.Description,
                    ParentRowKey = "please provide",
                    Price = model.Price,
                    ProductType = model.ProductType,
                    ProductTypeId = model.ProductTypeId,
                    Measure = model.Measure,
                    MeasureId = model.MeasureId
                };

                _productService.Insert(product, file);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(string productRowKey)
        {
            if (!string.IsNullOrEmpty(productRowKey))
                _productService.Delete(productRowKey);
            return RedirectToAction("Index");
        }
    }
}
