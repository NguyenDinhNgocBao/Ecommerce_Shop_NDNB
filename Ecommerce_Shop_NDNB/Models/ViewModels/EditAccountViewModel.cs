using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Shop_NDNB.Models.ViewModels
{
    public class EditAccountViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

    }
}
