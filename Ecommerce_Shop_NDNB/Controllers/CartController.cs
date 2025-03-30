using Ecommerce_Shop_NDNB.Models;
using Ecommerce_Shop_NDNB.Models.ViewModels;
using Ecommerce_Shop_NDNB.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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


			//Nhận phí shipping từ cookie
			var shippingPriceCookie = Request.Cookies["ShippingPrice"];
			decimal shippingPrice = 0;

			//Nhận coupon code từ cookie
			var coupon_code = Request.Cookies["CouponTitle"];

			if (shippingPriceCookie != null)
			{
				var shippingPriceJson = shippingPriceCookie;
				shippingPrice = JsonConvert.DeserializeObject<decimal>(shippingPriceJson);
			}


			CartItemViewModel cartVM = new()
			{
				CartItems = cartItems,
				GrandTotal = cartItems.Sum(x => x.Quantity * x.Price),
				ShippingCost = shippingPrice,
				CouponCode = coupon_code,
			};
			return View(cartVM);
		}


		public async Task<IActionResult> Add(int Id)
		{
			ProductModel product = await _dbContext.Products.FindAsync(Id);
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ??
				new List<CartItemModel>();
			CartItemModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();

			if (cartItem == null)
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

			if (cartItem.Quantity > 1)
			{
				--cartItem.Quantity;
			}
			else
			{
				cart.RemoveAll(c => c.ProductId == Id);
			}
			if (cart.Count == 0)
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
			ProductModel product = await _dbContext.Products.Where(p => p.Id == Id).FirstOrDefaultAsync();
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
			CartItemModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();

			if (cartItem.Quantity >= 1 && cartItem.Quantity <= product.Quantity)
			{
				++cartItem.Quantity;
			}
			else
			{
				cartItem.Quantity = product.Quantity;
				TempData["Error"] = "Số Lượng tối đa";
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

		[HttpPost]
		public async Task<IActionResult> GetShipping(ShippingModel shippingModel, string Province, string District, string Ward)
		{
			var existingShipping = await _dbContext.Shippings.FirstOrDefaultAsync(x => x.Province == Province && x.District == District && x.Ward == Ward);

			decimal shippingPrice = 0;
			if (existingShipping != null)
			{
				shippingPrice = existingShipping.Price;
			}
			else
			{
				shippingPrice = 30000;
			}
			var shippingPriceJson = JsonConvert.SerializeObject(shippingPrice);
			try
			{
				var cookieOptions = new CookieOptions
				{
					HttpOnly = true,
					Expires = DateTimeOffset.UtcNow.AddMinutes(30),
					Secure = true, //using Https
				};
				Response.Cookies.Append("ShippingPrice", shippingPriceJson, cookieOptions);

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
			return Json(new { shippingPrice });
		}

		[HttpPost]
		public IActionResult DeleteShipping()
		{
			Response.Cookies.Delete("ShippingPrice");
			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> GetCoupon(CouponModel couponModel, string coupon_value)
		{
			var validCoupon = await _dbContext.Coupons
				.FirstOrDefaultAsync(x => x.Name == coupon_value && x.Quantity >= 1);

			string couponTitle = validCoupon.Name + " | " + validCoupon?.Description;

			if(couponModel != null)
			{
				TimeSpan remainingTime = validCoupon.DateEnd - DateTime.Now;
				int daysRemaining = remainingTime.Days;

				if(daysRemaining >= 0)
				{
					try
					{
						var cookieOptions = new CookieOptions
						{
							HttpOnly = true,
							Expires = DateTimeOffset.UtcNow.AddMinutes(30),
							Secure = true,
							SameSite = SameSiteMode.Strict, // Kiểm tra tính tương thích với trình duyệt
						};
						Response.Cookies.Append("CouponTitle", couponTitle, cookieOptions);
						return Ok(new {success = true, message = "Coupon applied successfully"});
					}
					catch (Exception ex) 
					{
						return Ok(new { success = false, message = "Coupon applied failed" });
					}
				}
				else
				{
					return Ok(new { success = false, message = "Coupon has Expired" });
				}
			}
			else
			{
				return Ok(new { success = false, message = "Coupon not Existed" });
			}

			return Json(new { CouponTitle = couponTitle });
		}
	}
}
