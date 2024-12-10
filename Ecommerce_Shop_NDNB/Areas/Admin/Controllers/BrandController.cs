using Ecommerce_Shop_NDNB.Models;
using Ecommerce_Shop_NDNB.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Shop_NDNB.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize]
	public class BrandController : Controller
    {
		private readonly DB_Context db_Context;
		public BrandController(DB_Context context)
		{
			db_Context = context;
		}
		public async Task<IActionResult> Index()
		{
            var brands = db_Context.Brands.ToList();
            return View(brands);
        }

		#region Create
		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(BrandModel brand)
		{

			brand.Slug = brand.Name.Replace(" ", "-");
			var slug = await db_Context.Brands.FirstOrDefaultAsync(p => p.Slug == brand.Slug);
			if (slug != null)
			{
				ModelState.AddModelError("", "Thương hiệu đã tồn tại");
				return View(brand);
			}


            // Thêm sản phẩm vào cơ sở dữ liệu
            db_Context.Add(brand);
			await db_Context.SaveChangesAsync();
			// Thêm thông báo thành công
			TempData["SuccessMessage"] = "Thương Hiệu đã được thêm thành công!";
            TempData.Keep("SuccessMessage");
            return RedirectToAction("Index");

		}
		#endregion

		#region Update
		[HttpGet]
		public async Task<IActionResult> Update(int id)
		{
			BrandModel brand = await db_Context.Brands.FindAsync(id);
			return View(brand);
		}

		[HttpPost]
		public async Task<IActionResult> Update(int Id, BrandModel brand)
		{

			brand.Slug = brand.Name.Replace(" ", "-");
			var slug = await db_Context.Brands.FirstOrDefaultAsync(p => p.Slug == brand.Slug && p.Id != Id);
			if (slug != null)
			{
				ModelState.AddModelError("", "Sản phẩm đã tồn tại");
				return View(brand);

			}

			// Tcập nhật vào cơ sở dữ liệu
			db_Context.Update(brand);
			await db_Context.SaveChangesAsync();

			return RedirectToAction("Index");

		}
		#endregion

		#region Delete
		public async Task<IActionResult> Delete(int Id)
		{
			BrandModel brand = await db_Context.Brands.FindAsync(Id);
			if (brand == null)
			{
				return NotFound();
			}


			// Xóa sản phẩm khỏi cơ sở dữ liệu
			db_Context.Brands.Remove(brand);
			await db_Context.SaveChangesAsync();

			return RedirectToAction("Index");
		}
		#endregion
	}
}
