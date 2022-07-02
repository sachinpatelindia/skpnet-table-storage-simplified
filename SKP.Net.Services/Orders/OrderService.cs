using SKP.Net.Core.Domain.Order;
using SKP.Net.Storage.Operations;
using System.Collections.Generic;
using System.Linq;
namespace SKP.Net.Services.Orders
{
    public partial class OrderService : IOrderService
    {
        private readonly ITableStorage<Order> _orderStorage;
        private readonly ITableStorage<OrderItem> _orderItemStorage;
        public OrderService(ITableStorage<Order> orderStorage,
          ITableStorage<OrderItem> orderItemStorage)
        {
            _orderStorage = orderStorage;
            _orderItemStorage = orderItemStorage;
        }
        public bool Delete(string orderId = "", string customerId = "")
        {
            if (string.IsNullOrEmpty(orderId) && string.IsNullOrEmpty(customerId))
                return false;
            var orders = _orderStorage.GetAll<Order>();
            if (!string.IsNullOrEmpty(orderId))
                orders = orders.Where(o => o.RowKey == orderId);
            if (!string.IsNullOrEmpty(customerId))
                orders = orders.Where(o => o.CustomerRowKey == customerId);

            foreach (var order in orders)
            {
                var result = _orderStorage.Delete(order);
                if (result != null)
                {
                    var orderItems = _orderItemStorage.GetAll<OrderItem>().Where(oi => oi.OrderRowKey == result.RowKey);
                    foreach (var item in orderItems)
                    {
                        _orderItemStorage.Delete(item);
                    }
                }
            }
            return true;
        }

        public Order GetOrderById(string orderId)
        {
           return _orderStorage.GetAll<Order>().FirstOrDefault(o=>o.RowKey==orderId);
        }

        public List<Order> GetOrdersByCustomerId(string customerId)
        {
            return _orderStorage.GetAll<Order>().Where(o => o.CustomerRowKey == customerId).ToList();
        }

        public Order Insert(Order order)
        {
            if (order == null)
                return null;
            return _orderStorage.Insert(order);
        }

        public List<Order> Orders()
        {
            return _orderStorage.GetAll<Order>().ToList();
        }

        public Order Updage(Order order)
        {
            if (order == null)
                return null;
            return _orderStorage.Update(order);
        }
    }
}
