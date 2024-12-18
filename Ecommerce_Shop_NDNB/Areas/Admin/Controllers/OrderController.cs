using Ecommerce_Shop_NDNB.Models;
using Ecommerce_Shop_NDNB.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Shop_NDNB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Sales")]
    public class OrderController : Controller
	{
		private readonly DB_Context _dbContext;
		public OrderController(DB_Context dbContext) {
			_dbContext = dbContext;		
		}
		public async Task<IActionResult> Index()
		{
			return View(await _dbContext.Orders.OrderByDescending(p => p.Id).ToListAsync());
		}
		[HttpGet]
        public async Task<IActionResult> ViewOrder(string orderCode)
        {
			var detailOrder = await _dbContext.OrderDetails.Include(p => p.Products).Where(pr => pr.OrderCode == orderCode).ToListAsync();

            return View(detailOrder);
        }
    }
}
