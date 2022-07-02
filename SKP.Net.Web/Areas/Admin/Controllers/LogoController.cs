using Microsoft.AspNetCore.Mvc;
using SKP.Net.Core.Domain.Media;
using SKP.Net.Services.Images;
using SKP.Net.Storage.Operations;
using SKP.Net.Web.Areas.Admin.Models.Logo;
using System;
using System.Linq;

namespace SKP.Net.Web.Areas.Admin.Controllers
{
    public class LogoController : BaseAdminController
    {
        private readonly IImageService _imageService;
        private readonly ITableStorage<Image> _imageStorage;
        public LogoController(
            IImageService imageService,
            ITableStorage<Image> imageStorage)
        {
            _imageService = imageService;
            _imageStorage = imageStorage;
        }

        public IActionResult Index()
        {
            var models = _imageService.GetImages<Image>().Where(m=>m.ImageType==ImageType.Logo).Select(m => new LogoModel
            {
                FileName = m.FileName,
                ImageUrl = m.ImageUrl,
                Active = m.Active,
                CreatedOn = m.CreatedOn,
                ImageType = m.ImageType,
                RowKey=m.RowKey
            });
            return View(models);
        }

        public IActionResult Create()
        {
            var model = new LogoModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(LogoModel model)
        {
            if (ModelState.IsValid)
            {
                var file = Request.Form.Files.FirstOrDefault();
                var image = _imageService.Upload<Image>(file , ImageType.Logo);

                if (image == null)
                    return RedirectToAction("Index");
                ChangeStatus(image,model.Active);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        private void ChangeStatus(Image newImage, bool active)
        {
            var images = _imageService.GetImages<Image>().Where(m => m.ImageType == ImageType.Logo);
            if (!images.Any())
                return;
            foreach(var image in images)
            {
                image.Active = false;
                image.UpdatedOn = DateTime.UtcNow;
                _imageStorage.Update(image);
            }

            newImage.Active = active;
            newImage.CreatedOn = DateTime.UtcNow;
            newImage.UpdatedOn = DateTime.UtcNow;
            _imageStorage.Update(newImage);

        }
    }
}
