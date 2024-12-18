using Ecommerce_Shop_NDNB.Models;
using Ecommerce_Shop_NDNB.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Drawing.Drawing2D;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ecommerce_Shop_NDNB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
	{
		private readonly UserManager<AppUserModel> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DB_Context _dbContext;

		public UserController(UserManager<AppUserModel> userManager, RoleManager<IdentityRole> roleManager, DB_Context dB_Context)
		{
			_userManager = userManager;
			_roleManager = roleManager;
            _dbContext = dB_Context;
		}
        [HttpGet]
		[Route("Index")]
		public async Task<IActionResult> Index()
		{
            var userWithRoles = await (from u in _dbContext.Users
                                       join ur in _dbContext.UserRoles on u.Id equals ur.UserId
                                       join r in _dbContext.Roles on ur.RoleId equals r.Id
                                       select new { User = u, RoleName = r.Name }).ToListAsync();
			return View(userWithRoles);
		}
        #region Tạo User
        [HttpGet]
		[Route("Create")]
		public async Task<IActionResult> Create()
		{
			var role = await _roleManager.Roles.ToListAsync();
			ViewBag.Roles = new SelectList(role, "Id", "Name");
			return View(new AppUserModel()); 
		}
        [HttpPost]
		[ValidateAntiForgeryToken]
        [Route("Create")]
        public async Task<IActionResult> Create(AppUserModel userModel)
        {
			if(ModelState.IsValid)
			{
                var createUserResult = await _userManager.CreateAsync(userModel, userModel.PasswordHash);//tạo user
				if(createUserResult.Succeeded)
                {
                    var createdUser = await _userManager.FindByEmailAsync(userModel.Email);//Tìm user dựa vào mail
                    var userId = createdUser.Id;//Lấy userId
                    var role = _roleManager.FindByIdAsync(userModel.RoleId);//Lấy roleId
                    //gán quyền
                    var addToRoleResult = await _userManager.AddToRoleAsync(createdUser, role.Result.Name);
                    if (!addToRoleResult.Succeeded)
                    {
                        foreach (var error in createUserResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
					return RedirectToAction("Index", "User");
                }
                else
				{
                    foreach (var error in createUserResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(userModel);
				}
            }
			else
			{
				TempData["error"] = "Lỗi";
				List<string> errors = new List<string>();
				foreach (var value in ModelState.Values)
				{
					foreach (var error in value.Errors)
					{
						errors.Add(error.ErrorMessage);
					}
				}
				string errorMessage = string.Join("\n", errors);
				return BadRequest(errorMessage);
			}
        }
		#endregion

		#region Xóa
		[HttpGet]		
		public async Task<IActionResult> Delete(string Id)
		{
            if (string.IsNullOrEmpty(Id))
            {
                return NotFound();
            }
			var user = await _userManager.FindByIdAsync(Id);
			if(user == null)
			{
				return NotFound();
			}
			var deleteResult = await _userManager.DeleteAsync(user);
            if (deleteResult.Succeeded)
            {
                TempData["success"] = "Xóa người dùng thành công!";
            }
            else
            {
                TempData["error"] = "Xóa người dùng thất bại. Vui lòng thử lại!";
            }
            return RedirectToAction("Index","User");
        }
        #endregion
        #region Update
        [Route("Update")]
        [HttpGet]
        public async Task<IActionResult> Update(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                return NotFound();
            }
            var role = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(role, "Id", "Name");
            return View(user);
        }
        
        [HttpPost]
        [Route("Update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string Id, AppUserModel userModel)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Cập nhật các thông tin cơ bản
                user.UserName = userModel.UserName;
                user.Email = userModel.Email;
                user.PhoneNumber = userModel.PhoneNumber;
                user.RoleId = userModel.RoleId;
                var updateResult = await _userManager.UpdateAsync(user);
                if (updateResult.Succeeded)
                {
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    foreach (var error in updateResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                        return View(user);
                    }
                }
            }
            var role = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(role, "Id", "Name");
            return View(user);
        }
        #endregion
    }
}
