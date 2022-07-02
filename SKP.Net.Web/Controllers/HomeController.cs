using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SKP.Net.Core;
using SKP.Net.Core.Domain.Categories;
using SKP.Net.Core.Domain.News;
using SKP.Net.Core.Domain.Order;
using SKP.Net.Core.Domain.Pages;
using SKP.Net.Services.Authentication;
using SKP.Net.Services.Images;
using SKP.Net.Storage.Operations;
using SKP.Net.Web.Areas.Admin.Models.Categories;
using SKP.Net.Web.Models;
using SKP.Net.Web.Models.Home;
using SKP.Net.Web.Models.Orders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SKP.Net.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITableStorage<News> _newsStorage;
        private readonly IImageService _imageService;
        private readonly ITableStorage<PageData> _pageDataStorage;
        private readonly ITableStorage<Product> _productStorage;
        private readonly ITableStorage<ShoppingCartItem> _shoppingCartStorage;
        private readonly IWorkContext _workContext;
        private readonly IAuthenticationService _authenticationService;
        public HomeController(ILogger<HomeController> logger, 
            IWorkContext workContext,
            IAuthenticationService authenticationService,
            ITableStorage<News> newsStorage,
            IImageService imageService,
            ITableStorage<Product> productStorage,
            ITableStorage<ShoppingCartItem> shoppingCartStorage,
            ITableStorage<PageData> pageDataStorage)
        {
            _logger = logger;
            _imageService = imageService;
            _newsStorage = newsStorage;
            _pageDataStorage = pageDataStorage;
            _productStorage = productStorage;
            _shoppingCartStorage = shoppingCartStorage;
            _workContext = workContext;
            _authenticationService = authenticationService;
        }
        public IActionResult Index()
        {
            var user = _authenticationService.GetAuthenticatedCustomer();
            var model = GetHomeModel();
            return View(model);
        }

        private HomeViewModel GetHomeModel()
        {
            var model = new HomeViewModel();
            model.PageSections.Add(new Section
            {
                Description = "Bootstrap is a free and open-source CSS framework directed at responsive, mobile-first front-end web development. It contains CSS- and JavaScript-based design templates for typography, forms, buttons, navigation, and other interface components.",
                Title = "Starter projects 1",
                DisplayOrder = 1,
                Url = "/home/terms",
            });

            model.PageSections.Add(new Section
            {
                Description = "Bootstrap is a free and open-source CSS framework directed at responsive, mobile-first front-end web development. It contains CSS- and JavaScript-based design templates for typography, forms, buttons, navigation, and other interface components.",
                Title = "Starter projects 2",
                DisplayOrder = 2,
                Url = "/home/terms",
            });

            model.PageSections.Add(new Section
            {
                Description = "Bootstrap is a free and open-source CSS framework directed at responsive, mobile-first front-end web development. It contains CSS- and JavaScript-based design templates for typography, forms, buttons, navigation, and other interface components.",
                Title = "Starter projects 3",
                DisplayOrder = 3,
                Url = "/home/terms",
            });

            model.PageSections.Add(new Section
            {
                Description = "Bootstrap is a free and open-source CSS framework directed at responsive, mobile-first front-end web development. It contains CSS- and JavaScript-based design templates for typography, forms, buttons, navigation, and other interface components.",
                Title = "Starter projects 4",
                DisplayOrder = 4,
                Url = "/home/terms",
            });
            model.Carousels = GetCarousels();
            model.WelcomeSection = WelcomeSection();
            model.News = GetNews();
            model.FeaturedProducts = GetFeaturedProducts();
            model.ShoppingCartItems = GetCartItems();
            return model;
        }

        private Carousel WelcomeSection()
        {
            var page = _pageDataStorage.GetAll<PageData>().FirstOrDefault(m => m.PageType == PageDataType.Welcome && m.Active);
            if (page == null)
                return new Carousel();
            return new Carousel
            {
                Description = page.PageBody,
                Active = page.Active,
                Title = page.PageHead,
                Url = page.ImageUrl,
                RedirectUrl=page.RedirectUrl              
            };
        }

        public Section GetNews()
        {
            var news = _newsStorage.GetAll<News>().FirstOrDefault(m => m.Active);

            if (news == null)
                return new Section { };

            TimeSpan timediff = news.ExpireOnUtc.Date - DateTime.UtcNow.Date;

            if (timediff.Days > 0)
            {
                return new Section
                {
                    Description = news.Description,
                    Title = news.Title,
                    Url = news.Url,
                    Active = news.Active
                };
            }
            return new Section { };
        }
        
        private List<ShoppingCartItemModel> GetCartItems()
        {
            if (_workContext.CurrentCustomer == null)
                return new List<ShoppingCartItemModel>();

            var shoppingCartItems= _shoppingCartStorage.GetAll<ShoppingCartItem>().ToList();
            var products = _productStorage.GetAll<Product>().ToList();

            var query = from cart in shoppingCartItems
                        join product in products on cart.ProductRowKey equals product.RowKey
                        where cart.CustomerRowKey==_workContext.CurrentCustomer.RowKey
                        select new ShoppingCartItemModel
                        {
                            CustomerRowKey = cart.CustomerRowKey,
                            ProductName = product.Name,
                            Quantity = cart.Quantity,
                            ImageUrl = product.ImageUrl,
                            UnitPrice = product.Price,
                            SubTotal = product.Price * cart.Quantity

                        };
            return query.OrderBy(m=>m.ProductName).ToList();
        }

        private List<ProductModel> GetFeaturedProducts()
        {
            var products = _productStorage.GetAll<Product>();
            return products.Where(m => m.Active).Select(m =>
                   new ProductModel
                   {
                       Description = m.Description,
                       RowKey = m.RowKey,
                       ParentRowKey = m.ParentRowKey,
                       ImageUrl = m.ImageUrl,
                       Name = m.Name,
                       Price = m.Price,
                       Measure=m.Measure

                   }).ToList();
        }


        private List<Carousel> GetCarousels()
        {
            var carousels = _pageDataStorage.GetAll<PageData>().Where(p=>p.PageType==PageDataType.Carousel);
            var models = new List<Carousel>();
            carousels.ToList().ForEach(arg =>
            models.Add(new Carousel
            {
                Description = arg.PageBody,
                Active = arg.Active,
                Title = arg.PageHead,
                Url = arg.ImageUrl,
                RedirectUrl=arg.RedirectUrl,
                DisplayOrder=arg.DisplayOrder
                
            }));
            return models;
        }

        [Route("AboutUs")]
        public IActionResult AboutUs()
        {
            return View();
        }
        [Route("Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }
        [Route("Terms")]
        public IActionResult Terms()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
