using Microsoft.AspNetCore.Http;
using SKP.Net.Core.Domain.Categories;
using SKP.Net.Core.Domain.Media;
using SKP.Net.Services.Images;
using SKP.Net.Storage.Operations;
using System.Collections.Generic;
using System.Linq;

namespace SKP.Net.Services.Categories
{
    public partial class ProductService : IProductServics
    {

        private readonly IImageService _imageService;
        private readonly ITableStorage<Product> _productStorage;
        private readonly ITableStorage<Image> _imageStorage;
        public ProductService(IImageService imageService,
            ITableStorage<Product> productStorage,
            ITableStorage<Image> imageStorage)
        {
            _imageService = imageService;
            _productStorage = productStorage;
            _imageStorage = imageStorage;
        }
        public bool Delete(string productRowKey)
        {
            var existingProduct = GetProductById(productRowKey);
            if (existingProduct == null)
                return false;
            var image = _imageStorage.GetAll<Image>().FirstOrDefault(i => i.ParentRowKey == existingProduct.RowKey);
            _productStorage.Delete(existingProduct);
            if (image != null)
                _imageStorage.Delete(image);
            return GetProductById(productRowKey) == null;
        }

        public Product GetProductById(string productRowKey)
        {
            return _productStorage.GetAll<Product>()?.FirstOrDefault(m => m.RowKey == productRowKey);
        }

        public IList<Product> GetProducts()
        {
            return _productStorage.GetAll<Product>().ToList();
        }

        public bool Insert(Product product, IFormFile file)
        {
            if (product == null)
                return false;
            var existingProudct = _productStorage.Insert(product);
            if (file != null)
            {
                var image = _imageService.Upload<Image>(file, ImageType.Product);
                if (image != null)
                {
                    image.ParentRowKey = existingProudct.RowKey;
                    _imageStorage.Update(image);
                    existingProudct.ImageUrl = image.ImageUrl;
                    _productStorage.Update(product);
                    return GetProductById(existingProudct.RowKey) != null && image != null;
                }
                else
                {
                    _productStorage.Delete(product);
                    _imageStorage.Delete(image);
                }
            }
            return GetProductById(product.RowKey) != null;
        }

        public bool Update(Product product, IFormFile file)
        {
            if (product == null)
                return false;
            var existingProudct = _productStorage.Insert(product);
            if (file != null)
            {
                var existingImage = _imageStorage.GetAll<Image>().FirstOrDefault(i => i.ParentRowKey == existingProudct.RowKey);
                if (existingImage != null)
                {
                    _imageStorage.Delete(existingImage);
                }
                var image = _imageService.Upload<Image>(file, ImageType.Product);
                image.ParentRowKey = existingProudct.RowKey;
                _imageStorage.Update(image);
                existingProudct.ImageUrl = image.ImageUrl;
                _productStorage.Update(existingProudct);
                return GetProductById(product.RowKey) != null && image != null;
            }
            return false;
        }
    }
}
