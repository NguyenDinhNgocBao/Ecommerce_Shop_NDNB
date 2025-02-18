using Ecommerce_Shop_NDNB.Models;
using Ecommerce_Shop_NDNB.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Shop_NDNB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Sales")]
    public class ShippingController : Controller
    {
        private readonly DB_Context db_Context;
        public ShippingController(DB_Context context)
        {
            db_Context = context;
        }
        public async Task<IActionResult> Index()
        {
            var shippingList = await db_Context.Shippings.ToListAsync();
            ViewBag.Shippings = shippingList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> StoreShipping(ShippingModel shippingModel, string Province, string District, string Ward, decimal Price)
        {
            shippingModel.Province = Province;
            shippingModel.District = District;
            shippingModel.Ward = Ward;
            shippingModel.Price = Price;

            try
            {
                var existingShipping = await db_Context.Shippings.AnyAsync(s => s.Province == Province && s.District == District && s.Ward == Ward);
                if (existingShipping)
                {
                    return Ok(new { duplicate = true, message = "Dữ liệu trùng lặp" });
                }
                db_Context.Shippings.Add(shippingModel);
                await db_Context.SaveChangesAsync();
                return Ok(new { success = true, message = "Thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        public async Task<IActionResult> Delete(int Id)
        {
            ShippingModel shipping = await db_Context.Shippings.FindAsync(Id);
            db_Context.Shippings.Remove(shipping);
            await db_Context.SaveChangesAsync();
            TempData["Success"] = "Xóa thành công";
            return RedirectToAction("Index");
        }
    }
}
