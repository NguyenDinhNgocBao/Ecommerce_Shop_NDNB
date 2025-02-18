
using Ecommerce_Shop_NDNB.Repository.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce_Shop_NDNB.Models
{
	public class ProductModel
	{
		[Key]
		public int Id { get; set; }

		[Required, MinLength(4, ErrorMessage ="Yêu cầu nhập tên sản phẩm")]
		public string Name { get; set; }

		[Required, MinLength(4, ErrorMessage = "Yêu cầu nhập mô tả sản phẩm")]
		public string Description { get; set; }

		[Required]
		public string Slug { get; set; }

		[Required(ErrorMessage ="Yêu cầu nhập giá tiền")]
		[Range(0.01, double.MaxValue)]
		public decimal Price { get; set; }
		[Required, Range(1, int.MaxValue, ErrorMessage = "Chọn một thương hiệu")]
        public int BrandId { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Chọn một danh mục")]
        public int CategoryId {  get; set; }
        public CategoryModel Category { get; set; }
        public BrandModel Brand { get; set; }
        public string Image { get; set; }
        [NotMapped]
		public IFormFile ImageUpload { get; set; }
		public EvaluateModel Evaluates { get; set; }
		public int Quantity { get; set; }
		public int Sold { get; set; }
    }
}
