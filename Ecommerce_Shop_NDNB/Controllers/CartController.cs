using Ecommerce_Shop_NDNB.Models;
using Ecommerce_Shop_NDNB.Models.ViewModels;
using Ecommerce_Shop_NDNB.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Shop_NDNB.Controllers
{
	public class CartController : Controller
	{
		private readonly DB_Context _dbContext;
		public CartController(DB_Context context)
		{
			_dbContext = context;
		}
		public IActionResult Index()
		{
			List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ??
				new List<CartItemModel>();
			CartItemViewModel cartVM = new()
			{
				CartItems = cartItems,
				GrandTotal = cartItems.Sum(x => x.Quantity * x.Price)
			};
			return View(cartVM);
		}

		public IActionResult Checkout()
		{
			return View();
		}

		public async Task<IActionResult> Add(int Id)
		{
			ProductModel product = await _dbContext.Products.FindAsync(Id);
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ??
				new List<CartItemModel>();
			CartItemModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();

			if(cartItem == null)
			{
				cart.Add(new CartItemModel(product));
			}
			else
			{
				cartItem.Quantity++;
			}
			HttpContext.Session.SetJson("Cart", cart);

            // Trả về JSON để dùng cho AJAX
            return Json(new { success = true, message = "Thêm sản phẩm vào giỏ hàng thành công" });
        }

		public async Task<IActionResult> Decrease(int Id) 
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
			CartItemModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();

			if(cartItem.Quantity > 1)
			{
				--cartItem.Quantity;
			}
			else
			{
				cart.RemoveAll(c => c.ProductId == Id);
			}
			if(cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.SetJson("Cart", cart);
			}

			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Increase(int Id)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
			CartItemModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();

			if (cartItem.Quantity >= 1)
			{
				++cartItem.Quantity;
			}
			HttpContext.Session.SetJson("Cart", cart);

			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Remove(int Id)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
			cart.Where(c => c.ProductId == Id);

			cart.RemoveAll(c => c.ProductId == Id);
			if (cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.SetJson("Cart", cart);
			}

			// Logic để xóa sản phẩm 
			TempData["SuccessMessage"] = "Sản phẩm đã được xóa khỏi giỏ hàng thành công";

			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Clear()
		{
			HttpContext.Session.Remove("Cart");
			// Logic để xóa tất cả sản phẩm trong giỏ hàng
			TempData["SuccessMessage"] = "Tất cả sản phẩm đã được xóa";
			return RedirectToAction("Index");
		}

	}
}
