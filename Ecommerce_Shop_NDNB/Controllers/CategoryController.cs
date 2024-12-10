using Ecommerce_Shop_NDNB.Models;
using Ecommerce_Shop_NDNB.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Shop_NDNB.Controllers
{
	public class CategoryController : Controller
	{
		private readonly DB_Context _dbContext;
		public CategoryController(DB_Context dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task<IActionResult> Index(string Slug = "")
		{
			CategoryModel category = _dbContext.Categories.Where(c => c.Slug == Slug).FirstOrDefault();
			if(category == null) return RedirectToAction("Index");
			var productByCategory = _dbContext.Products.Where(p => p.CategoryId == category.Id);
			return View(await productByCategory.OrderByDescending(c => c.Id).ToListAsync());
		}
	}
}
