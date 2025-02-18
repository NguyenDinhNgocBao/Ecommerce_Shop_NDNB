using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Shop_NDNB.Models
{
    public class ShippingModel
    {
        [Key]
        public int Id { get; set; }
        public decimal Price { get; set; }  
        public string Ward {  get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
    }
}
