using Ecommerce_Shop_NDNB.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Shop_NDNB.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin,Sales")]
	public class CouponController : Controller
	{
		private readonly DB_Context db_Context;
		public CouponController(DB_Context context)
		{
			db_Context = context;
		}
		public async Task<IActionResult> Index()
		{
			return View();
		}
	}
}
