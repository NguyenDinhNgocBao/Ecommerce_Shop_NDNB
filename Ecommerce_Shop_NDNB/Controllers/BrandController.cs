using Ecommerce_Shop_NDNB.Models;
using Ecommerce_Shop_NDNB.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Shop_NDNB.Controllers
{
	public class BrandController : Controller
	{
		private readonly DB_Context _dbContext;
		public BrandController(DB_Context dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task<IActionResult> Index(string Slug = "")
		{
			BrandModel brand = _dbContext.Brands.Where(b => b.Slug == Slug).FirstOrDefault();
			if (brand == null) return RedirectToAction("Index");
			var productByBrand = _dbContext.Products.Where(p => p.BrandId == brand.Id);
			return View(await productByBrand.OrderByDescending(b => b.Id).ToListAsync());
		}
	}
}
