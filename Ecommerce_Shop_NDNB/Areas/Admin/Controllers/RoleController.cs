using Ecommerce_Shop_NDNB.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Shop_NDNB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly DB_Context _db_Context;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleController(DB_Context db_Context, RoleManager<IdentityRole> roleManager)
        {
            _db_Context = db_Context;   
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            return View( await _db_Context.Roles.OrderByDescending(r => r.Id).ToListAsync());
        }
        #region Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string RoleName)
        {
            var existingRole = await _db_Context.Roles.FirstOrDefaultAsync(r => r.Name == RoleName);
            if (existingRole != null)
            {
                TempData["ErrorMessage"] = "Role already exists.";
                return RedirectToAction(nameof(Index));
            }

            var role = new IdentityRole { Name = RoleName };
            await _db_Context.Roles.AddAsync(role);
            await _db_Context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Role added successfully!";
            return RedirectToAction("Index");
        }
        #endregion
        #region Update
        [HttpGet]
        public async Task<IActionResult> Update(string Id)
        {
            var role = await _roleManager.FindByIdAsync(Id);
            if (role == null)
            {
                TempData["ErrorMessage"] = "Role not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(IdentityRole model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.Id);
                if (role == null)
                {
                    TempData["ErrorMessage"] = "Role not found.";
                    return RedirectToAction(nameof(Index));
                }
                role.Name = model.Name;
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Role updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }
        #endregion
        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(string Id)
        {
            var role = await _roleManager.FindByIdAsync(Id);
            if (role == null)
            {
                TempData["ErrorMessage"] = "Role not found.";
                return RedirectToAction("Index");
            }
            await _roleManager.DeleteAsync(role);
            TempData["SuccessMessage"] = "Xóa thành công";
            return RedirectToAction("Index");

        }
        #endregion
    }
}
