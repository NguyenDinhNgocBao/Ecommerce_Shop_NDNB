using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce_Shop_NDNB.Models
{
	public class SliderModel
	{
		[Key]
		public int Id { get; set; }
		[Required(ErrorMessage = "Yêu cầu nhập tên Thương Hiệu")]
		public string Name { get; set; }
		[Required(ErrorMessage = "Yêu cầu nhập mô tả")]
		public string Description { get; set; }
		public int? Status { get; set; }
		public string Image { get; set; }
		[NotMapped]
		[FileExtensions]
		public IFormFile ImageUpload { get; set; }
	}
}
