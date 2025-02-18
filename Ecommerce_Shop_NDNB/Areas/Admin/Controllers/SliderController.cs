using Ecommerce_Shop_NDNB.Models;
using Ecommerce_Shop_NDNB.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Shop_NDNB.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin,Sales")]
	public class SliderController : Controller
	{
		
		private readonly DB_Context db_Context;
		public SliderController(DB_Context context)
		{
			db_Context = context;
		}
		public async Task<IActionResult> Index()
		{
			return View(await db_Context.Sliders.OrderByDescending(p => p.Id).ToListAsync());
		}
		[HttpGet]
		public async Task<IActionResult> Create()
		{
			return View();
		}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderModel slider)
        {
            // Xử lý ảnh tải lên
            if (slider.ImageUpload != null && slider.ImageUpload.Length > 0)
            {
                var fileName = Path.GetFileName(slider.ImageUpload.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", fileName); // Lưu ảnh vào thư mục wwwroot/images
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await slider.ImageUpload.CopyToAsync(stream);
                }
                slider.Image = fileName; // Lưu đường dẫn ảnh vào cơ sở dữ liệu
            }

            // Thêm sản phẩm vào cơ sở dữ liệu
            db_Context.Add(slider);
            await db_Context.SaveChangesAsync();
            // Thêm thông báo thành công
            TempData["SuccessMessage"] = "Sản phẩm đã được thêm thành công!";
            return RedirectToAction("Index");

        }

        #region Update
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            SliderModel slider = await db_Context.Sliders.FindAsync(id);
            return View(slider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int Id, SliderModel slider)
        {

            var existingProduct = await db_Context.Sliders.FindAsync(Id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            // Kiểm tra nếu không có hình ảnh mới thì giữ lại hình ảnh cũ
            if (slider.ImageUpload != null && slider.ImageUpload.Length > 0)
            {
                var fileName = Path.GetFileName(slider.ImageUpload.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await slider.ImageUpload.CopyToAsync(stream);
                }
                existingProduct.Image = fileName; // Cập nhật ảnh mới
            }
            else
            {
                slider.Image = existingProduct.Image; // Giữ lại ảnh cũ
            }

            // Cập nhật các trường khác
            existingProduct.Name = slider.Name;
            existingProduct.Description = slider.Description;
            existingProduct.Status = slider.Status;


            // Thêm sản phẩm vào cơ sở dữ liệu
            db_Context.Update(existingProduct);
            await db_Context.SaveChangesAsync();

            return RedirectToAction("Index");

        }
        #endregion
        #region Delete
        public async Task<IActionResult> Delete(int Id)
        {
            SliderModel slider = await db_Context.Sliders.FindAsync(Id);
            if(slider == null)
            {
                return NotFound();
            }
            if (!string.Equals(slider.Image, "noname.jpg", StringComparison.OrdinalIgnoreCase))
            {
                // Xóa file ảnh khỏi thư mục wwwroot/image
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", slider.Image);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            // Xóa sản phẩm khỏi cơ sở dữ liệu
            db_Context.Sliders.Remove(slider);
            await db_Context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        #endregion

    }
}
