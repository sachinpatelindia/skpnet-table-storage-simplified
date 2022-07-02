using Microsoft.AspNetCore.Mvc;
using SKP.Net.Services.Orders;
using SKP.Net.Web.Areas.Admin.Models.Orders;
using System.Linq;
namespace SKP.Net.Web.Areas.Admin.Controllers
{
    public class OrdersController : BaseAdminController
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public IActionResult Index()
        {
            var orderModel = _orderService.Orders().Select(o => new OrderAdminModel
            {
                OrderRowKey=o.RowKey,
                CreatedOn = o.CreatedOn,
                OrderNumber = o.OrderNumber,
                OrderTotal = o.OrderTotal
            });
            return View(orderModel);
        }

        public IActionResult Delete(string orderRowKey)
        {
            if (string.IsNullOrEmpty(orderRowKey))
                return RedirectToAction("Index");
            _orderService.Delete(orderId:orderRowKey);
            return RedirectToAction("Index");
        }
    }
}
