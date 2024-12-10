using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Shop_NDNB.Models.ViewModels
{
	public class LoginViewModel
	{
		[Key]
		[Required]
		public int Id { get; set; }
		[Required(ErrorMessage = "Nhập Tên đăng nhập")]
		public string UserName { get; set; }
		[DataType(DataType.Password), Required(ErrorMessage = "Nhập Mật Khẩu")]
		public string Password { get; set; }
		public string ReturnUrl { get; set; }
	}
}
