using SKP.Net.Core.Domain.Categories;
using System.Collections.Generic;

namespace SKP.Net.Services.Categories
{
    public interface ICategoryService
    {
        void Delete(Category Category);
        Category GetCategory(string key);
        IEnumerable<Category> GetCategories();
        void Insert(Category Category);
        void Update(Category Category);
    }
}