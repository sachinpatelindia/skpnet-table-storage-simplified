using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SKP.Net.Core;
using SKP.Net.Services.Articles;
using SKP.Net.Services.Authentication;
using SKP.Net.Services.Categories;
using SKP.Net.Services.Customers;
using SKP.Net.Web.Models.Articles;

namespace SKP.Net.Web.Controllers
{
    [Authorize]
    public class ArticlesController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IWorkContext _workContext;
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;
        public ArticlesController(IAuthenticationService authenticationService,
            IWorkContext workContext,
            IArticleService articleService)
        {
            _authenticationService = authenticationService;
            _workContext = workContext;
            _articleService = articleService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ArticleDetails(string articleid)
        {
           var article= _articleService.GetArticle(articleid);
            if (article is null)
                return RedirectToAction("/");
            var model = new ArticleModel
            {
                CreatedBy = article.CustomerRowKey,
                CreatedOn = article.CreateOnUtc,
                FullDescription = article.FullDescription,
                ShortDescription = article.ShortDescription,
                Rank = article.Rank
            };
            return View(model);
        }
    }
}
