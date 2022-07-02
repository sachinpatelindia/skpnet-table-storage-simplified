namespace SKP.Net.Web.Models.Orders
{
    public class ShoppingCartItemModel
    {
        public string CustomerRowKey { get; set; }
        public string ProductRowKey { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double SubTotal { get; set; }
    }
}
