using Microsoft.AspNetCore.Mvc;
using SKP.Net.Core.Domain.Categories;
using SKP.Net.Services.Categories;
using SKP.Net.Services.SEO;
using SKP.Net.Web.Areas.Admin.Models.Categories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SKP.Net.Web.Areas.Admin.Controllers
{
    public class CategoryController : BaseAdminController
    {
        private readonly ICategoryService _categoryService;
        private readonly IUrlRecordService _urlRecordService;
        public CategoryController(ICategoryService categoryService, IUrlRecordService urlRecordService)
        {
            _categoryService = categoryService;
            _urlRecordService = urlRecordService;
        }
        public IActionResult Index()
        {
            var categories = _categoryService.GetCategories();
            var models = new List<CategoryModel>();
            categories.ToList().ForEach(arg =>
            models.Add(new CategoryModel
            {
                Description = arg.Description,
                DisplayOrder = arg.DisplayOrder,
                Name = arg.Name,
                ParentCategoryId = arg.ParentCategoryId,
                CreatedOnUtc = arg.CreatedOnUtc,
                UpdatedOnUtc = arg.UpdatedOnUtc,
                MetaDescription = arg.MetaDescription,
                MetaKeywords = arg.MetaKeywords,
                MetaTitle = arg.MetaTitle,
                Deleted = arg.Deleted,
                Active = arg.Active,
                CategoryTypeId = arg.CategoryTypeId,
                Published = false,
                IncludeOnTopMenu = arg.IncludeOnTopMenu,
                ShowOnHomePage = arg.ShowOnHomePage,
                SeName = _urlRecordService?.GetSeName(arg),

            }));
            return View(models);
        }

        public IActionResult Create()
        {
            var model = new CategoryModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(CategoryModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var category = new Category
                {
                    Description = model.Description,
                    DisplayOrder = model.DisplayOrder,
                    Name = model.Name,
                    ParentCategoryId = model.ParentCategoryId,
                    CreatedOnUtc = DateTime.UtcNow,
                    UpdatedOnUtc = DateTime.UtcNow,
                    MetaDescription = model.MetaDescription,
                    MetaKeywords = model.MetaKeywords,
                    MetaTitle = model.MetaTitle,
                    Deleted = false,
                    Active = true,
                    CategoryTypeId = model.CategoryTypeId,
                    Published = false,
                    IncludeOnTopMenu = true,
                    ShowOnHomePage = true,

                };
                _categoryService.Insert(category);
                var sename = _urlRecordService
             .ValidateSeName(category, category.Name, category.Name, true)
             .ToLowerInvariant();
                _urlRecordService.SaveSlug(category, sename);
                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
