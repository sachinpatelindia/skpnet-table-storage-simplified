using SKP.Net.Core.Domain.Order;
using System.Collections.Generic;

namespace SKP.Net.Services.Orders
{
    public partial interface IOrderService
    {
        Order Insert(Order order);
        Order Updage(Order order);
        bool Delete(string orderId="", string customerId="");
        public Order GetOrderById(string orderId);
        public List<Order> GetOrdersByCustomerId(string customerId);
        public List<Order> Orders();


    }
}
