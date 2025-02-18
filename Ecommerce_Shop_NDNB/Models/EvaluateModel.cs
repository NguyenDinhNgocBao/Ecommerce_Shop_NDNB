using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Shop_NDNB.Models
{
	public class EvaluateModel
	{
		[Key]
		public int Id { get; set; }

		public int ProductId { get; set; }
		[Required, MinLength(4, ErrorMessage = "Yêu cầu nhập đánh giá")]
		public string Comment { get; set; }
		[Required, MinLength(4, ErrorMessage = "Yêu cầu nhập tên")]
		public string Name { get; set; }
		[Required, MinLength(4, ErrorMessage = "Yêu cầu nhập email")]
		public string Email { get; set; }
		public string RatingStar { get; set; }

		[ForeignKey("ProductId")]
		public ProductModel Product { get; set; }
	}
}
