using Microsoft.AspNetCore.Http;
using SKP.Net.Core.Domain.Categories;
using System.Collections.Generic;

namespace SKP.Net.Services.Categories
{
    public partial interface IProductServics
    {
        bool Insert(Product product, IFormFile file);
        bool Update(Product product, IFormFile file);
        bool Delete(string productRowKey);
        Product GetProductById(string productRowKey);
        IList<Product> GetProducts();
    }
}
