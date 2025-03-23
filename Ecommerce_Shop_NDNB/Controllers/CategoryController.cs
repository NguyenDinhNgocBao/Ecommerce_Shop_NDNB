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
		public async Task<IActionResult> Index(string Slug = "", string sort_by = "", string startPrice = "", string endPrice ="")
		{
			//Lấy tất cả sản phẩm
			CategoryModel category = _dbContext.Categories.Where(c => c.Slug == Slug).FirstOrDefault();
			if (category == null) return RedirectToAction("Index");
			IQueryable<ProductModel> productByCategory = _dbContext.Products.Where(p => p.CategoryId == category.Id);

			#region Lấy sản phẩm theo giá trị lọc
			if (!string.IsNullOrEmpty(sort_by))
			{
				if (sort_by == "price_increase")
				{
					productByCategory = productByCategory.OrderBy(p => p.Price);
				}
				else if (sort_by == "price_decrease")
				{
					productByCategory = productByCategory.OrderByDescending(p => p.Price);
				}
				else if (sort_by == "price_newest")
				{
					productByCategory = productByCategory.OrderByDescending(p => p.Id);
				}
				else if (sort_by == "price_oldest")
				{
					productByCategory = productByCategory.OrderBy(p => p.Id);
				} 
			}

			if (!string.IsNullOrEmpty(startPrice) && !string.IsNullOrEmpty(endPrice))
			{
				if (decimal.TryParse(startPrice, out decimal startPriceValue) && decimal.TryParse(endPrice, out decimal endPriceValue))
				{
					productByCategory = productByCategory.Where(p => p.Price >= startPriceValue && p.Price <= endPriceValue);
				}
			}

			#endregion

			return View(await productByCategory.ToListAsync());
		}
	}
}
