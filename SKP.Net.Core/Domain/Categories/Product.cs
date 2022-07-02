using System;

namespace SKP.Net.Core.Domain.Categories
{
    public partial class Product : BaseEntity
    {
        public Product()
        {
            PartitionKey = nameof(Product);
            RowKey = Guid.NewGuid().ToString();
        }
        public string ParentRowKey { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public string ImageUrl { get; set; }
        public double Price { get; set; }
        public int ProductTypeId { get; set; }
        public int MeasureId { get; set; }
        public Measure Measure
        {
            get => (Measure)MeasureId;
            set => MeasureId = (int)value;
        }
        public ProductType ProductType
        {
            get => (ProductType)ProductTypeId;
            set => ProductTypeId = (int)value;
        }
    }
}
