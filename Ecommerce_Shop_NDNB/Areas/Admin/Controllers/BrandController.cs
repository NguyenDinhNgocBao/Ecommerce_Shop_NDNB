﻿using Ecommerce_Shop_NDNB.Models;
using Ecommerce_Shop_NDNB.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Shop_NDNB.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin,Sales")]
	public class BrandController : Controller
    {
		private readonly DB_Context db_Context;
		public BrandController(DB_Context context)
		{
			db_Context = context;
		}
		public async Task<IActionResult> Index(int pg = 1)
		{
            var brand = db_Context.Brands.AsQueryable();
            int recsCount = await brand.CountAsync();

            const int pageSize = 10; //10 items/trang
            var pager = new Paginate(recsCount, pg, pageSize);

            if (pg < 1) //page < 1;
            {
                pg = Math.Clamp(pg, 1, pager.TotalPages);//Giới hạn tối đa giá trị trang
            }

            int recSkip = (pg - 1) * pageSize; //(3 - 1) * 10; // chia ra để lấy dữ liệu vdu: ở trang 2 sẽ lấy dữ liệu từ 10 -> 20

            //category.Skip(20).Take(10).ToList()

            var data = await brand.Skip(recSkip).Take(pager.PageSize).ToListAsync();

            ViewBag.Pager = pager;

            return View(data);
        }

		#region Create
		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}
		[Route("Create")]
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
