using Microsoft.AspNetCore.Http;
using SKP.Net.Core.Domain.Categories;
using System.ComponentModel.DataAnnotations;

namespace SKP.Net.Web.Areas.Admin.Models.Categories
{
    public class ProductModel
    {
        public string RowKey { get; set; }
        public string ParentRowKey { get; set; }
        [Display(Name ="Product Name"),Required]
        public string Name { get; set; }
        [Display(Name = "Short Description"), Required]
        public string ShortDescription { get; set; }
        [Display(Name = "Long Description"), Required]
        public string Description { get; set; }
        public bool Active { get; set; }
        public string ImageUrl { get; set; }
        [Display(Name = "Product Price"), Required]
        public double Price { get; set; }
        [Display(Name = "Proudct Image"), Required]
        public IFormFile File { get; set; }
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
