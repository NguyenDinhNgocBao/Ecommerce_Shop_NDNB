using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Shop_NDNB.Models
{
    public class ProductQuantityModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập số lượng sản phẩm")]
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public DateTime DateCreate { get; set; }
        
    }
}
