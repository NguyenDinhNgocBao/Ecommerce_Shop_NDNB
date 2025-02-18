using Ecommerce_Shop_NDNB.Models;
using Ecommerce_Shop_NDNB.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Shop_NDNB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Sales")]
    public class ContactController : Controller
    {
        private readonly DB_Context db_Context;
        public ContactController(DB_Context context)
        {
            db_Context = context;
        }
        public IActionResult Index()
        {
            var contact = db_Context.Contacts.ToList();
            return View(contact);
        }



        #region Update
        [HttpGet]
        public async Task<IActionResult> Update(int ID)
        {
            ContactModel contact = await db_Context.Contacts.FindAsync(ID);
            return View(contact);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int ID, ContactModel contact)
        {
            var existingContact = await db_Context.Contacts.FindAsync(ID);
            if(existingContact == null)
            {
                return NotFound();
            }
            // Kiểm tra nếu không có hình ảnh mới thì giữ lại hình ảnh cũ
            if (contact.ImageUpload != null && contact.ImageUpload.Length > 0)
            {
                var fileName = Path.GetFileName(contact.ImageUpload.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await contact.ImageUpload.CopyToAsync(stream);
                }
                existingContact.LogoImg = fileName; // Cập nhật ảnh mới
            }
            else
            {
                contact.LogoImg = existingContact.LogoImg; // Giữ lại ảnh cũ
            }
            // Cập nhật các trường khác
            existingContact.Name = contact.Name;
            existingContact.Map = contact.Map;
            existingContact.PhoneNumber = contact.PhoneNumber;
            existingContact.Description = contact.Description;


            // Thêm sản phẩm vào cơ sở dữ liệu
            db_Context.Update(existingContact);
            await db_Context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        #endregion
    }
}
