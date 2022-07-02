using Microsoft.AspNetCore.Mvc;
using SKP.Net.Core.Domain.Media;
using SKP.Net.Services.Images;
using SKP.Net.Web.Areas.Admin.Models.Media;
using System.Linq;
namespace SKP.Net.Web.Areas.Admin.Controllers
{
    public class ImageController : BaseAdminController
    {
        private readonly IImageService _imageService;
        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        public IActionResult Index()
        {
            var models = _imageService.GetImages<Image>().Select(m => new ImageModel
            {
                FileName = m.FileName,
                ImageUrl = m.ImageUrl,
                Active = m.Active,
                CreatedOn=m.CreatedOn,
                RowKey = m.RowKey,
                Size=m.Size,
                UpdatedOn = m.UpdatedOn.Value,
                ImageType = m.ImageType.ToString()
            });
            return View(models);
        }

        public IActionResult Delete(string rowkey)
        {
            _imageService.Delete("skpatel", rowkey);
            return RedirectToAction("Index");
        }
    }
}
