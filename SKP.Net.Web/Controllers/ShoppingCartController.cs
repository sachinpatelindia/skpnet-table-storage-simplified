using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SKP.Net.Core;
using SKP.Net.Core.Domain.Categories;
using SKP.Net.Core.Domain.Customers;
using SKP.Net.Core.Domain.Messages;
using SKP.Net.Core.Domain.Order;
using SKP.Net.Services.Customers;
using SKP.Net.Services.Messages;
using SKP.Net.Storage.Operations;
using SKP.Net.Web.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SKP.Net.Web.Controllers
{
    [Authorize]
    public class ShoppingCartController : BaseController
    {
        private readonly ITableStorage<ShoppingCartItem> _shoppingCartStorage;
        private readonly ITableStorage<Product> _productStorage;
        private readonly IWorkContext _workContext;
        private readonly IMessageSender _messageSender;
        private readonly ICustomerSettingService _customerSettingServices;
        public ShoppingCartController(
            ITableStorage<ShoppingCartItem> shoppingCartStorage,
            IWorkContext workContext,
            IMessageSender messageSender,
            ICustomerSettingService customerSetting,
            ITableStorage<Product> productStorage)
        {
            _shoppingCartStorage = shoppingCartStorage;
            _productStorage = productStorage;
            _workContext = workContext;
            _messageSender = messageSender;
            _customerSettingServices = customerSetting;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Route("cart")]
        public IActionResult Cart()
        {
            var model = GetShoppingCartItemsByCustomer();
            return View(model);
        }

        [Authorize]
        public IActionResult AddToCart(string productid, string customerrowkey)
        {
            if (string.IsNullOrEmpty(productid))
                throw new ArgumentNullException(nameof(productid));
            if (string.IsNullOrEmpty(customerrowkey))
                throw new ArgumentNullException(nameof(customerrowkey));

            var shoppingCartItems = _shoppingCartStorage.GetAll<ShoppingCartItem>().Where(m => m.ProductRowKey == productid && m.CustomerRowKey == customerrowkey).FirstOrDefault();
            if (shoppingCartItems.Quantity > 2)
                return RedirectToAction("index", "home");
            if (shoppingCartItems == null)
            {
                _shoppingCartStorage.Insert(new ShoppingCartItem
                {
                    CustomerRowKey = customerrowkey,
                    Quantity = 1,
                    ProductRowKey = productid
                });
              //  _messageSender.SendMesage(TemplateType.Order, "sachin@skpatel.net", "New order", "New Shopping cart item added by user :" + _workContext.CurrentCustomer.Email);
                return RedirectToAction("index", "home");
            }
            shoppingCartItems.Quantity = shoppingCartItems.Quantity + 1;
            _shoppingCartStorage.Update(shoppingCartItems);
            var customer = _customerSettingServices.CustomerSettings().FirstOrDefault(s => s.PurchasedServiceId == shoppingCartItems.RowKey);
            if (customer == null)
                _customerSettingServices.Insert(new CustomerSettings { Active = true, CreatedOnUtc = DateTime.UtcNow, Deleted = false, CutomerRowKey = customerrowkey, UpdatedOnUtc = DateTime.UtcNow, Url = "/cart", Name = "Shopping Cart", PurchasedServiceName = "Shopping Cart", PurchasedServiceId = shoppingCartItems.RowKey });
            return RedirectToAction("index", "home");
        }


        private List<ShoppingCartItemModel> GetCartItems()
        {
            var shoppingCartItems = _shoppingCartStorage.GetAll<ShoppingCartItem>().ToList();
            var products = _productStorage.GetAll<Product>().ToList();

            var query = from cart in shoppingCartItems
                        join product in products on cart.ProductRowKey equals product.RowKey
                        select new ShoppingCartItemModel
                        {
                            CustomerRowKey = cart.CustomerRowKey,
                            ProductName = product.Name,
                            Quantity = cart.Quantity,
                            ImageUrl = product.ImageUrl,
                            UnitPrice = product.Price,
                            SubTotal = product.Price * cart.Quantity

                        };
            return query.OrderBy(m => m.ProductName).ToList();
        }

        public List<ShoppingCartItemModel> GetShoppingCartItemsByCustomer()
        {
            var customer = _workContext.CurrentCustomer;
            if (customer == null)
                throw new InvalidOperationException(nameof(customer));
            var cartItems = GetCartItems().Where(m => m.CustomerRowKey.Equals(customer.RowKey));
            return cartItems.ToList();
        }
    }
}
