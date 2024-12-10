using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Shop_NDNB.Models
{
	public class CategoryModel
	{
		[Key]
		public int Id { get; set; }

		[Required( ErrorMessage ="Yêu cầu nhập tên Danh Mục")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Yêu cầu nhập mô tả")]
		public string Description { get; set; }

		[Required]
		public string Slug { get; set; }
		
		public int Status { get; set; }

	}
}
