using SKP.Net.Core.Domain.Categories;
using SKP.Net.Storage.Operations;
using System.Collections.Generic;

namespace SKP.Net.Services.Categories
{
    public partial class CategoryService : ICategoryService
    {
        private readonly ITableStorage<Category> _categoryStorage;
        public CategoryService(ITableStorage<Category> categoryStorage)
        {
            _categoryStorage = categoryStorage;
        }
        public IEnumerable<Category> GetCategories()
        {
            return _categoryStorage.GetAll<Category>();
        }

        public Category GetCategory(string key)
        {
            return _categoryStorage.Get<Category>(key);
        }
        public void Insert(Category Category)
        {
            _categoryStorage.Insert(Category);
        }

        public void Update(Category Category)
        {
            _categoryStorage.Update(Category);
        }

        public void Delete(Category Category)
        {
            _categoryStorage.Delete(Category);
        }
    }
}
