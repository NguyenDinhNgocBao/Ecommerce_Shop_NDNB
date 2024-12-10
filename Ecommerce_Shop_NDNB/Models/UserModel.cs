using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Shop_NDNB.Models
{
	public class UserModel
	{
		[Key]
		[Required]
		public int Id { get; set; }
		[Required(ErrorMessage ="Nhập Tên đăng nhập")]
		public string UserName { get; set; }
		[Required(ErrorMessage ="Nhập Email"),EmailAddress]
		public string Email { get; set; }
		[DataType(DataType.Password), Required(ErrorMessage ="Nhập Mật Khẩu")]
		public string Password { get; set; }
	}
}
