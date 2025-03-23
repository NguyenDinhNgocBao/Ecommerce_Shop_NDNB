using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Shop_NDNB.Models
{
	public class CouponModel
	{
		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage = "Yêu cầu nhập tên mã giảm giá")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Yêu cầu nhập mổ tả")]
		public string Description { get; set; }
		public DateTime DateStart { get; set; }
		public DateTime DateEnd { get; set; }

		[Required(ErrorMessage = "Yêu cầu nhập giá")]
		public decimal Price { get; set; }

		[Required(ErrorMessage = "Yêu cầu nhập số lượng")]
		public int Quantity { get; set; }
		public int Status { get; set; }
	}
}
