using Ecommerce_Shop_NDNB.Models;
using Ecommerce_Shop_NDNB.Models.ViewModels;
using Ecommerce_Shop_NDNB.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
		public async Task<IActionResult> Details(long Id)
		{
			if (Id == null) return RedirectToAction("Index");
			var productById = db_Context.Products.Include(p => p.Evaluates).Where(p => p.Id == Id).FirstOrDefault();
			//related product
			var relatedProduct = await db_Context.Products
				.Where(p => p.CategoryId == productById.CategoryId && p.Id != productById.Id).Take(4).ToListAsync();
			ViewBag.RelatedProduct = relatedProduct;

			var viewModel = new ProductDetailViewModel
			{
				ProductDetails = productById,
				EvaluateDetails = productById.Evaluates
			};

			return View(viewModel);
		}
		[HttpPost]
		public async Task<IActionResult> Search(string search)
		{
			if (search.IsNullOrEmpty())
			{
				return RedirectToAction("Index", "Home");
			}
			var product = await db_Context.Products
				.Where(p => p.Name.Trim().ToLower().Contains(search.Trim().ToLower()) 
				|| p.Description.Contains(search)).ToListAsync();
			ViewBag.KeyWord = search;
			return View(product);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Evaluate(EvaluateModel model)
		{
			if(ModelState.IsValid)
			{
				var evaluateEntity = new EvaluateModel
				{
					ProductId = model.ProductId,
					Name = model.Name,
					Email = model.Email,
					Comment = model.Comment,
					RatingStar = model.RatingStar
				};
				db_Context.Evaluates.Add(evaluateEntity);
				await db_Context.SaveChangesAsync();
				TempData["success"] = "Thêm đánh giá thành công";
				return Redirect(Request.Headers["Referer"]);//Trả về trang trước đó
			}
			else
			{
				return RedirectToAction("Details", new {id = model.ProductId});
			}
			return Redirect(Request.Headers["Referer"]);
		}

		
		
		
		
	}
}
