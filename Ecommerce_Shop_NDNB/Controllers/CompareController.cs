using Ecommerce_Shop_NDNB.Models;
using Ecommerce_Shop_NDNB.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Shop_NDNB.Controllers
{
    public class CompareController : Controller
    {
        private readonly DB_Context _dbContext;
        private readonly UserManager<AppUserModel> _userManager;
        public CompareController(DB_Context context, UserManager<AppUserModel> userManager)
        {
            _dbContext = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var compare_Product = await (from c in _dbContext.Compares
                                         join p in _dbContext.Products on c.ProductId equals p.Id
                                         join u in _dbContext.Users on c.UserId equals u.Id
                                         select new
                                         {
                                             UserName = u.UserName,  // Lấy tên User
                                             ProductName = p.Name,   // Đổi tên tường minh
                                             Description = p.Description,
                                             Price = p.Price,
                                             Image = p.Image,  // Đảm bảo cột này có dữ liệu
                                             CompareId = c.Id  // ID dùng để xóa
                                         })
                          .ToListAsync();

            return View(compare_Product);
        }

        public async Task<IActionResult> DeleteCompare(int Id)
        {
            CompareModel compare = await _dbContext.Compares.FindAsync(Id);
            // Xóa sản phẩm khỏi cơ sở dữ liệu
            _dbContext.Compares.Remove(compare);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
