using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SKP.Net.Core;
using SKP.Net.Core.Domain.Articles;
using SKP.Net.Services.Articles;
using SKP.Net.Services.Categories;
using SKP.Net.Services.SEO;
using SKP.Net.Storage.Operations;
using SKP.Net.Web.Areas.Admin.Models.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
namespace SKP.Net.Web.Areas.Admin.Controllers
{
    public class ArticlesController : BaseAdminController
    {
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;
        private readonly IWorkContext _workContext;
        private readonly IUrlRecordService _urlRecordService;
        private ITableStorage<ArticleCategory> _articleCategory;
        public ArticlesController(IArticleService articleService,
            IWorkContext workContext,
            ICategoryService categoryService,
            ITableStorage<ArticleCategory> articleCategory,
            IUrlRecordService urlRecordService)
        {
            _articleService = articleService;
            _workContext = workContext;
            _urlRecordService = urlRecordService;
            _categoryService = categoryService;
            _articleCategory = articleCategory;
        }
        // GET: ArticleController
        public ActionResult Index()
        {
            var articles = _articleService.GetArticles()
                .Where(article => article.Active && !article.Deleted)
                .Select(article => this.PrepareArticleModel(article, article.RowKey, false))
                .OrderBy(article => article.CreateOnUtc);
            return View(articles);
        }

        // GET: ArticleController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ArticleController/Create
        public ActionResult Create()
        {
            var model = this.PrepareArticleModel(new Article(), "", true);
            return View(model);
        }

        // POST: ArticleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ArticleModel model)
        {
            if (ModelState.IsValid)
            {
                var article = new Article
                {
                    Active = false,
                    ArticleCategoryId = "",
                    CreateOnUtc = DateTime.UtcNow,
                    CustomerRowKey = _workContext.CurrentCustomer.RowKey,
                    ShortDescription = model.ShortDescription,
                    FullDescription = model.FullDescription,
                    UpdatedOnUtc = DateTime.UtcNow,
                    Deleted = false,
                };
                _articleService.Insert(article);
                var sename = _urlRecordService
                    .ValidateSeName(article, article.ShortDescription, article.ShortDescription, true)
                    .ToLowerInvariant();
                _articleCategory.Insert(new ArticleCategory { ArticleRowKey = article.RowKey, CategoryRowKey = model.CategoryRowKey });
                _urlRecordService.SaveSlug(article, sename);
                return RedirectToAction("index");
            }
            var loadModel = this.PrepareArticleModel(new Article(), "", true);
            return View(loadModel);
        }

        // GET: ArticleController/Edit/5
        public ActionResult Edit(string articleRowKey)
        {
            var article = _articleService.GetArticle(articleRowKey.Trim());
            if (article == null)
                throw new ArgumentException(nameof(articleRowKey));

            var model = PrepareArticleModel(article, article.ArticleCategoryId, true);
            return View(model);
        }

        // POST: ArticleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ArticleModel model)
        {
            var article = _articleService.GetArticle(model.RowKey.Trim());
            if (article == null)
                throw new ArgumentException(nameof(model));
            article.FullDescription = model.FullDescription;
            article.Active = model.Active;
            article.ArticleCategoryId = model.CategoryRowKey;
            _articleService.Update(article);
            var updatedModel=this.PrepareArticleModel(article, article.ArticleCategoryId, true);
            return View(updatedModel);
        }

        // GET: ArticleController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ArticleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private ArticleModel PrepareArticleModel(Article article, string categoryId, bool loadCategories = true)
        {
            List<SelectListItem> categories = new List<SelectListItem>();
            if (loadCategories)
            {
                categories = _categoryService.GetCategories().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.RowKey,
                    Selected = c.RowKey == categoryId
                }).ToList();
            }
            var model = new ArticleModel
            {
                Active = article.Active,
                CreateOnUtc = article.CreateOnUtc,
                CustomerRowKey = article.CustomerRowKey,
                Deleted = article.Deleted,
                DisplayOrder = article.DisplayOrder,
                FullDescription = article.FullDescription,
                Rank = article.Rank,
                RowKey = article.RowKey,
                ShortDescription = article.ShortDescription,
                UpdatedOnUtc = article.UpdatedOnUtc,
                Categories = categories
            };
            return model;
        }
    }
}
