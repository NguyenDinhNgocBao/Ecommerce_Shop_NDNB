using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce_Shop_NDNB.Models
{
    public class ContactModel
    {
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập tiêu đề")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập địa chỉ")]
        public string Map { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập số điện thoại")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập mô tả")]
        public string Description { get; set; }

        public string LogoImg { get; set; }

        [NotMapped]
        public IFormFile ImageUpload { get; set; }

    }
}
