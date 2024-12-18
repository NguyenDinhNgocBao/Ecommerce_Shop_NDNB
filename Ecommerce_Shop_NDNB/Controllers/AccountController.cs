using Ecommerce_Shop_NDNB.Models;
using Ecommerce_Shop_NDNB.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Shop_NDNB.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUserModel> _userManage;
        private SignInManager<AppUserModel> _signinManage;
        public AccountController(SignInManager<AppUserModel> signInManager, UserManager<AppUserModel> userManager)
        {
            _signinManage = signInManager;
            _userManage = userManager;  
        }

		#region Đăng Nhập
		public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl});
        }

        [HttpPost]
		public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signinManage.PasswordSignInAsync(loginVM.UserName, loginVM.Password, false, false);//2 tham số cuối là duy trì đăng nhập và logout khi thất bại nên để false luôn
                if (result.Succeeded)
                {
                    return Redirect(loginVM.ReturnUrl ?? "/");
                }
                ModelState.AddModelError("", "Invalid UserName and Password");
            }
            return View(loginVM);
        }
		#endregion

		#region Create User
		[HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserModel user)
        {
            if (ModelState.IsValid)
            {
				AppUserModel newUser = new AppUserModel
				{
					UserName = user.UserName,
					Email = user.Email,
                    PhoneNumber = user.Phone
				};

				// Tạo tài khoản với mật khẩu
				IdentityResult result = await _userManage.CreateAsync(newUser, user.Password);

				if (result.Succeeded)
                {
                    TempData["success"] = "Tạo thành công";
                    return Redirect("/account/login");
                }
                foreach(IdentityError e in result.Errors)
                {
                    ModelState.AddModelError("", e.Description);
                }
            }
            return View(user);
        }
		#endregion

		#region Đăng xuất
        public async Task<IActionResult> Logout(string returnUrl = "/")
        {
            await _signinManage.SignOutAsync();
            return Redirect(returnUrl);
        }
		#endregion
	}
}
