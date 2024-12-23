using Ecommerce_Shop_NDNB.Models;
using Ecommerce_Shop_NDNB.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public async Task<IActionResult> Index()
        {
			if (User.Identity.IsAuthenticated)
			{
				// Lấy thông tin người dùng
				var user = await _userManage.GetUserAsync(User);

				// Tạo đối tượng UserModel từ thông tin người dùng
				var model = new UserModel
				{
					UserName = user.UserName,
					Email = await _userManage.GetEmailAsync(user),
					Phone = user.PhoneNumber,
					RoleId = user.RoleId // Nếu bạn có trường này trong User
				};
				return View(model);
			}
			return RedirectToAction("Login");
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

        #region
        [HttpGet("Account/Edit")]
        public async Task<IActionResult> Edit()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Lấy thông tin người dùng
                var user = await _userManage.GetUserAsync(User);

                // Tạo đối tượng UserModel từ thông tin người dùng
                var model = new EditAccountViewModel
                {
                    UserName = user.UserName,
                    Email = await _userManage.GetEmailAsync(user),
                    Phone = user.PhoneNumber
                };
                return View(model);
            }
            return RedirectToAction("Index");
        }

        [HttpPost("Account/Edit")]
        public async Task<IActionResult> Edit(EditAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManage.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound();
                }

                // Cập nhật thông tin người dùng
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.PhoneNumber = model.Phone;
                
                // Xử lý thay đổi mật khẩu
                if (!string.IsNullOrEmpty(model.CurrentPassword) && !string.IsNullOrEmpty(model.NewPassword))
                {
                    // Kiểm tra mật khẩu hiện tại
                    var passwordCheck = await _userManage.CheckPasswordAsync(user, model.CurrentPassword);
                    if (!passwordCheck)
                    {
                        ModelState.AddModelError("CurrentPassword", "Mật khẩu hiện tại không chính xác.");
                        return View(model);
                    }

                    // Thay đổi mật khẩu
                    var passwordChangeResult = await _userManage.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                    if (!passwordChangeResult.Succeeded)
                    {
                        foreach (var error in passwordChangeResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View(model);
                    }
                }
                
                // Lưu các thay đổi
                var result = await _userManage.UpdateAsync(user);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Thông tin đã được cập nhật thành công!";
                    return RedirectToAction("Index", "Account");
                }
                
            }
            return View(model);
        }
        #endregion
    }
}
