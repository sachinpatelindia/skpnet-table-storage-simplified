using Microsoft.AspNetCore.Http;
using SKP.Net.Core;
using SKP.Net.Core.Domain.Media;
using System.Collections.Generic;

namespace SKP.Net.Services.Images
{
    public interface IImageService
    {
        Image Upload<T>(IFormFile file, ImageType imageType, string fileNamePrefix = "") where T : BaseEntity;
        void Delete(string containerName, string rowkey);
        List<Image> GetImages<T>() where T : BaseEntity;
    }
}