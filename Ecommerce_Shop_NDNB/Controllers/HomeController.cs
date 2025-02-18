using Ecommerce_Shop_NDNB.Models;
using Ecommerce_Shop_NDNB.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Ecommerce_Shop_NDNB.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly DB_Context _dbContext;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUserModel> _userManager;

        public HomeController(ILogger<HomeController> logger, DB_Context context, UserManager<AppUserModel> userManager)
        {
            _logger = logger;
            _dbContext = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var products = _dbContext.Products.Include("Category").Include("Brand").ToList();
            var sliders = _dbContext.Sliders.Where(s => s.Status == 1).ToList();
            ViewBag.Sliders = sliders;
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0,Location =ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statuscode)
        {
            if(statuscode == 404)
            {
                return View("Error");
            }
            else
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        public async Task<IActionResult> Contact()
        {
            var contact = await _dbContext.Contacts.FirstAsync();
            return View(contact);
        }
                          
        
        [HttpPost]
		public async Task<IActionResult> AddToWishList(int Id)
        {
            var user = await _userManager.GetUserAsync(User);
            var wishList = new WishListModel
            {
                ProductId = Id,
                UserId = user.Id
            };

            await _dbContext.AddAsync(wishList);
            try
            {
                await _dbContext.SaveChangesAsync();
                return Json(new { success = true, message = "Thêm sản phẩm vào danh sách yêu thích thành công" });
            }
            catch (Exception ex) {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        [HttpPost]
		public async Task<IActionResult> AddToCompare(int Id)
        {
            var user = await _userManager.GetUserAsync(User);
            var compare = new CompareModel
            {
                ProductId = Id,
                UserId = user.Id
            };

            await _dbContext.AddAsync(compare);
            try
            {
                await _dbContext.SaveChangesAsync();
                return Json(new { success = true, message = "Thêm sản phẩm vào danh sách so sánh thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

    }
}
