using Microsoft.AspNetCore.Http;
using SKP.Net.Core;
using SKP.Net.Core.Domain.Media;
using SKP.Net.Storage.Operations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace SKP.Net.Services.Images
{
    public class ImageService : IImageService
    {
        private readonly IBlobStorage _blobStorage;
        private readonly ITableStorage<Image> _imageStorage;
        public ImageService(IBlobStorage blobStorage, ITableStorage<Image> imageStorage)
        {
            _blobStorage = blobStorage;
            _imageStorage = imageStorage;
        }

        public void Delete(string containerName, string rowkey)
        {
            var image = _imageStorage.GetAll<Image>().FirstOrDefault(m => m.RowKey == rowkey);
            if (image != null)
            {
                _blobStorage.Delete(containerName, image.FileName);
                _imageStorage.Delete(image);
            }
        }

        public List<Image> GetImages<T>() where T : BaseEntity
        {
            var imageList = new List<Image>();
            var images = _imageStorage.GetAll<Image>();
            foreach (var image in images)
            {
                imageList.Add(new Image
                {
                    CreatedOn = image.CreatedOn,
                    FileName = image.FileName,
                    ImageType = image.ImageType,
                    ImageUrl = image.ImageUrl,
                    Active = image.Active,
                    RowKey = image.RowKey,
                    UpdatedOn = image.UpdatedOn,
                    PartitionKey = image.PartitionKey,
                    Size = image.Size
                });
            }
            return imageList;
        }

        public Image Upload<T>(IFormFile file, ImageType imageType, string fileNamePrefix = "") where T : BaseEntity
        {
            using var memStream = new MemoryStream();
            file.CopyTo(memStream);
            memStream.Position = 0;
            var fileName = file.FileName;
            long size = file.Length;

            var fileExtension = Path.GetExtension(fileName);
            var guid = Convert.ToString(Guid.NewGuid());
            var newFileName = $"{fileNamePrefix }" + String.Concat(guid, fileExtension);
            var typeName = typeof(T).Name;

            var imageUrl = _blobStorage.Upload(memStream, "skpatel", newFileName.Trim());
            return _imageStorage.Insert(new Image
            {
                CreatedOn = DateTime.UtcNow,
                FileName = fileName,
                ImageType = imageType,
                ImageUrl = imageUrl,
                Active = false,
                UpdatedOn = DateTime.UtcNow,
                PartitionKey = typeName,
                Size = size
            });
        }
    }
}
