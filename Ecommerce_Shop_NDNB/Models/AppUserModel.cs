using Microsoft.AspNetCore.Identity;

namespace Ecommerce_Shop_NDNB.Models
{
    public class AppUserModel : IdentityUser
    {
        public String Occupation {  get; set; }
    }
}
