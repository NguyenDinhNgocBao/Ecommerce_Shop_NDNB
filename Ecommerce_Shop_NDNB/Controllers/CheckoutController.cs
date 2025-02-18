using Ecommerce_Shop_NDNB.Areas.Admin.Repository;
using Ecommerce_Shop_NDNB.Models;
using Ecommerce_Shop_NDNB.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace Ecommerce_Shop_NDNB.Controllers
{
	public class CheckoutController : Controller
	{
		private readonly DB_Context _dbContext;
		private readonly IEmailSender _emailSender;
		public CheckoutController(DB_Context dbContext, IEmailSender emailSender)
		{
			_dbContext = dbContext;
			_emailSender = emailSender;
		}
		[HttpGet]
		public async Task<IActionResult> Checkout()
		{
			var userEmail = User.FindFirstValue(ClaimTypes.Email);
			if (userEmail == null) {
				return RedirectToAction("Login","Account");
			}	
			else
			{
				var orderCode = Guid.NewGuid().ToString();
				var orderItem = new OrderModel();
				orderItem.OrderCode = orderCode;
				orderItem.UserName = userEmail;
				orderItem.Status = 1;
				orderItem.CreatedDate = DateTime.Now;
				_dbContext.Add(orderItem);
				_dbContext.SaveChanges();
				List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ??
				new List<CartItemModel>();
				foreach(var cartItem in cartItems)
				{
					var orderDetails = new OrderDetails();
					orderDetails.UserName = userEmail;
					orderDetails.OrderCode = orderCode;
					orderDetails.ProductId = cartItem.ProductId;
					orderDetails.Price = cartItem.Price;
					orderDetails.Quantity = cartItem.Quantity;

					//update product quantity
					var product =  _dbContext.Products.Where(p => p.Id == cartItem.ProductId).FirstOrDefault();
					product.Quantity -= cartItem.Quantity;
					product.Sold += cartItem.Quantity;
					_dbContext.Update(product);

					_dbContext.Add(orderDetails);
					_dbContext.SaveChanges();
				}
				HttpContext.Session.Remove("Cart");

				//Send Mail Order khi thành công
				var receiver = userEmail;
				var subject = "Đặt hành thành công";
				var mesage = "Đặt hàng thành công, Chờ giao nhé";
				await _emailSender.SendEmailAsync(receiver, subject, mesage);

				TempData["success"] = "Checkout thành công, vui lòng chờ duyệt đơn hàng";
				return RedirectToAction("Index", "Cart");
			}
		}
	}
}
