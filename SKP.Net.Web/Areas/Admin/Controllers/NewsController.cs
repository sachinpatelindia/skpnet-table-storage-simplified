using Microsoft.AspNetCore.Mvc;
using SKP.Net.Core.Domain.News;
using SKP.Net.Services.Images;
using SKP.Net.Storage.Operations;
using SKP.Net.Web.Areas.Admin.Models.News;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SKP.Net.Web.Areas.Admin.Controllers
{
    public class NewsController : BaseAdminController
    {
        private readonly ITableStorage<News> _newsStorage;
        private readonly IBlobStorage _blobStorage;
        private readonly IImageService _imageService;
        public NewsController(ITableStorage<News> newsSTorage, IBlobStorage blobStorage, IImageService imageService)
        {
            _newsStorage = newsSTorage;
            _blobStorage = blobStorage;
            _imageService = imageService;
        }
        public IActionResult Index()
        {
            var news = _newsStorage.GetAll<News>();
            var models = new List<NewsModel>();
            news.ToList().ForEach(arg =>

            models.Add(new NewsModel
            {
                CreatedOnUtc = arg.CreatedOnUtc,
                Description = arg.Description,
                Active = arg.Active,
                Title = arg.Title,
                Url = arg.Url,
                ExpireOnUtc = arg.ExpireOnUtc,
                RowKey=arg.RowKey
            }));
            return View(models);
        }

        public IActionResult Create()
        {
            var model = new NewsModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(NewsModel model)
        {
            if (ModelState.IsValid)
            {

                foreach (var news in _newsStorage.GetAll<News>().Where(m => m.Active))
                {
                    news.Active = false;
                    _newsStorage.Update(news);
                }

                _newsStorage.Insert(new News
                {
                    CreatedOnUtc = DateTime.UtcNow,
                    Description = model.Description,
                    Active = model.Active,
                    Title = model.Title,
                    Url = model.Url,
                    ExpireOnUtc = model.ExpireOnUtc.Value
                });
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Delete(string rowKey)
        {
            var news = _newsStorage.Get<News>(rowKey);
            _newsStorage.Delete(news);
            return RedirectToAction("Index");
        }

    }
}
