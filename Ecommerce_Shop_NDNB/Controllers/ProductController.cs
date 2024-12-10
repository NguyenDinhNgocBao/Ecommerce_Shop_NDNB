using Ecommerce_Shop_NDNB.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Shop_NDNB.Controllers
{
	public class ProductController : Controller
	{
		private readonly DB_Context db_Context;
		public ProductController(DB_Context context)
		{
			db_Context = context;
		}
		public IActionResult Index()
		{
			return View();
		}
		public async Task<IActionResult> Details(int Id)
		{
			if (Id == null) return RedirectToAction("Index");
			var productById = db_Context.Products.Where(p => p.Id == Id).FirstOrDefault();
			return View(productById);
		}
	}
}
