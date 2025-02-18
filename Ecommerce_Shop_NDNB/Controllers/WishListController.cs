using Ecommerce_Shop_NDNB.Models;
using Ecommerce_Shop_NDNB.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Shop_NDNB.Controllers
{
    public class WishListController : Controller
    {
        private readonly DB_Context _dbContext;
        private readonly UserManager<AppUserModel> _userManager;
        public WishListController(DB_Context context, UserManager<AppUserModel> userManager)
        {
            _dbContext = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var wishlist_Product = await (from w in _dbContext.WishLists
                                        join p in _dbContext.Products on w.ProductId equals p.Id
                                        join u in _dbContext.Users on w.UserId equals u.Id
                                        select new
                                        {
                                            UserName = u.UserName,  // Lấy tên User
                                            ProductName = p.Name,   // Đổi tên tường minh
                                            Description = p.Description,
                                            Price = p.Price,
                                            Image = p.Image,  // Đảm bảo cột này có dữ liệu
                                            WishListId = w.Id  // ID dùng để xóa
                                        })
                            .ToListAsync();

            return View(wishlist_Product);
        }

        public async Task<IActionResult> DeleteWishList(int Id)
        {
            WishListModel wishList = await _dbContext.WishLists.FindAsync(Id);
            // Xóa sản phẩm khỏi cơ sở dữ liệu
            _dbContext.WishLists.Remove(wishList);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
