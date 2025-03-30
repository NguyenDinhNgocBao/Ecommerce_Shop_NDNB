using Ecommerce_Shop_NDNB.Models;
using Ecommerce_Shop_NDNB.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Shop_NDNB.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin,Sales")]
	public class CouponController : Controller
	{
		private readonly DB_Context db_Context;
		public CouponController(DB_Context context)
		{
			db_Context = context;
		}

        public async Task<IActionResult> Index()
        {
            var coupon_list = await db_Context.Coupons.ToListAsync();   
            ViewBag.CouponList = coupon_list;
            return View();
        }

        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> Create(CouponModel coupon)
        {
            // Thêm sản phẩm vào cơ sở dữ liệu
            db_Context.Add(coupon);
            await db_Context.SaveChangesAsync();
            // Thêm thông báo thành công
            TempData["SuccessMessage"] = "Khuyến mãi đã được thêm thành công!";
            TempData.Keep("SuccessMessage");
            return RedirectToAction("Index");

        }
    }
}
