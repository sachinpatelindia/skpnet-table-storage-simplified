using SKP.Net.Core.Domain.Articles;
using SKP.Net.Storage.Operations;
using System.Collections.Generic;
using System.Linq;
namespace SKP.Net.Services.Articles
{
    public partial class ArticleService : IArticleService
    {
        private readonly ITableStorage<Article> _articleStorage;
        private readonly ITableStorage<ArticleCategory> _articleCatgoryStorage;
        public ArticleService(ITableStorage<Article> articleStorage, ITableStorage<ArticleCategory> articleCategoryStorage)
        {
            _articleStorage = articleStorage;
            _articleCatgoryStorage = articleCategoryStorage;
        }
        public IEnumerable<Article> GetArticles()
        {
            return _articleStorage.GetAll<Article>();
        }

        public Article GetArticle(string key)
        {
            return this.GetArticles().FirstOrDefault(article=>article.RowKey==key);
        }
        public void Insert(Article article)
        {
            _articleStorage.Insert(article);
        }

        public void Update(Article article)
        {
            _articleStorage.Update(article);
        }

        public void Delete(Article article)
        {
            _articleStorage.Delete(article);
        }

        public IEnumerable<Article> GetArticleByCategoryRowKey(string categoryRowKey)
        {
            var query = from ac in _articleCatgoryStorage.GetAll<ArticleCategory>()
                        join article in _articleStorage.GetAll<Article>() on ac.ArticleRowKey equals article.RowKey
                        where ac.CategoryRowKey == categoryRowKey && article.Active
                        select article;
            return query.OrderBy(a=>a.CreateOnUtc);
        }
    }
}
