using Microsoft.AspNetCore.Mvc;
using SKP.Net.Core.Domain.Media;
using SKP.Net.Core.Domain.Pages;
using SKP.Net.Services.Images;
using SKP.Net.Storage.Operations;
using SKP.Net.Web.Areas.Admin.Models.PageData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SKP.Net.Web.Areas.Admin.Controllers
{
    public class PageDataController : BaseAdminController
    {
        private readonly ITableStorage<PageData> _pageDataStorage;
        private readonly ITableStorage<Image> _imageStorage;
        private readonly IImageService _imageService;
        public PageDataController(ITableStorage<PageData> pageDataStorage, IImageService imageService, ITableStorage<Image> imageStorage)
        {
            _pageDataStorage = pageDataStorage;
            _imageStorage = imageStorage;
            _imageService = imageService;
        }
        public IActionResult PageDataDetails(int pageId)
        {
            return View();
        }
        public IActionResult Index()
        {
            var models = new List<PageDataModel>();
            var pages = _pageDataStorage.GetAll<PageData>();
            foreach (var page in pages)
                models.Add(new PageDataModel
                {
                    Active = page.Active,
                    Name = page.Name,
                    PageBody = page.PageBody,
                    PageHead = page.PageHead,
                    PageType = page.PageType,
                    RowKey = page.RowKey,
                    ImageUrl=page.ImageUrl,
                    RedirectUrl=page.RedirectUrl
                });

            return View(models);
        }

        public IActionResult Create()
        {
            var model = new PageDataModel();
            return View(model);
        }
        [HttpPost]
        public IActionResult Create(PageDataModel model)
        {
            if (ModelState.IsValid)
            {
                var file = Request.Form.Files.FirstOrDefault();
                var image = _imageService.Upload<Image>(file, ImageType.PageSection);
                if (image == null)
                    return RedirectToAction("Index");
                var page = _pageDataStorage.Insert(new PageData
                {
                    ImageUrl = image.ImageUrl,
                    Active = model.Active,
                    Name = model.Name,
                    PageBody = model.PageBody,
                    PageHead = model.PageHead,
                    RedirectUrl = model.RedirectUrl,
                    PageTypeId = model.PageTypeTypeId,
                    PageType = model.PageType,
                    DisplayOrder=model.DisplayOrder
                    
                });

                image.ParentRowKey = page.RowKey;
                _imageStorage.Update(image);

                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}
