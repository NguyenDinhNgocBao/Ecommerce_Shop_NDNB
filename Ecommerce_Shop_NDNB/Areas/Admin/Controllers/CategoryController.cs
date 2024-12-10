using Ecommerce_Shop_NDNB.Models;
using Ecommerce_Shop_NDNB.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Shop_NDNB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CategoryController : Controller
	{
		private readonly DB_Context db_Context;
		public CategoryController(DB_Context context)
		{
			db_Context = context;
		}
		public async Task<IActionResult> Index()
		{
			return View(await db_Context.Categories.OrderByDescending(p => p.Id).ToListAsync());
		}

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryModel category)
        {

            category.Slug = category.Name.Replace(" ", "-");
            var slug = await db_Context.Categories.FirstOrDefaultAsync(p => p.Slug == category.Slug);
            if (slug != null)
            {
                ModelState.AddModelError("", "Danh mục đã tồn tại");
                return View(category);
            }
            

            // Thêm sản phẩm vào cơ sở dữ liệu
            db_Context.Add(category);
            await db_Context.SaveChangesAsync();
            // Thêm thông báo thành công
            TempData["SuccessMessage"] = "Sản phẩm đã được thêm thành công!";
            return RedirectToAction("Index");

        }
        #endregion

        #region Update
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            CategoryModel category = await db_Context.Categories.FindAsync(id);
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int Id, CategoryModel category)
        {

            category.Slug = category.Name.Replace(" ", "-");
            var slug = await db_Context.Categories.FirstOrDefaultAsync(p => p.Slug == category.Slug && p.Id != Id);
            if (slug != null)
            {
                ModelState.AddModelError("", "Sản phẩm đã tồn tại");
                return View(category);

            }

            // Tcập nhật vào cơ sở dữ liệu
            db_Context.Update(category);
            await db_Context.SaveChangesAsync();

            return RedirectToAction("Index");

        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int Id)
        {
            CategoryModel category = await db_Context.Categories.FindAsync(Id);
            if (category == null)
            {
                return NotFound();
            }
           
            
            // Xóa sản phẩm khỏi cơ sở dữ liệu
            db_Context.Categories.Remove(category);
            await db_Context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        #endregion
    }
}
