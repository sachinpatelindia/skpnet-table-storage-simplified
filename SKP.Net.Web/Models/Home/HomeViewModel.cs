using SKP.Net.Web.Areas.Admin.Models.Categories;
using SKP.Net.Web.Models.Orders;
using System;
using System.Collections.Generic;

namespace SKP.Net.Web.Models.Home
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {

            PageSections = new List<Section>();
            Carousels = new List<Carousel>();
            News = new Section();
            WelcomeSection = new Carousel();
            FeaturedProducts = new List<ProductModel>();
            ShoppingCartItems = new List<ShoppingCartItemModel>();

        }

        public Carousel WelcomeSection { get; set; }
        public Section News { get; set; }
        public List<Section> PageSections { get; set; }
        public List<Carousel> Carousels { get; set; }
        public List<ProductModel> FeaturedProducts { get; set; }
        public List<ShoppingCartItemModel> ShoppingCartItems { get; set; }
    }
    public class Section
    {
        public Section()
        {
            Urls = new List<Tuple<string, string>>();
        }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageByte { get; set; }
        public string ImagePath { get; set; }
        public string Url { get; set; }
        public List<Tuple<string,string>> Urls { get; set; }
        public int DisplayOrder { get; set; }
        public bool Active { get; set; }
    }

    public class Carousel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public string CssClass { get; set; }
        public string Url { get; set; }
        public string RedirectUrl { get; set; }
        public int DisplayOrder { get; set; }
    }
}
