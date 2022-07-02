using SKP.Net.Core.Domain.Articles;
using System.Collections.Generic;

namespace SKP.Net.Services.Articles
{
    public interface IArticleService
    {
        void Delete(Article article);
        Article GetArticle(string key);
        IEnumerable<Article> GetArticles();
        IEnumerable<Article> GetArticleByCategoryRowKey(string categoryRowKey);
        void Insert(Article article);
        void Update(Article article);
    }
}