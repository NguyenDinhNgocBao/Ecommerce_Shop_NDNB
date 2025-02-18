using Ecommerce_Shop_NDNB.Migrations;
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
		public async Task<IActionResult> Index(int pg = 1)
		{
            var order = _dbContext.Orders.AsQueryable();
            int recsCount = await order.CountAsync();

            const int pageSize = 10; //10 items/trang
            var pager = new Paginate(recsCount, pg, pageSize);

            if (pg < 1) //page < 1;
            {
                pg = Math.Clamp(pg, 1, pager.TotalPages);//Giới hạn tối đa giá trị trang
            }

            int recSkip = (pg - 1) * pageSize; //(3 - 1) * 10; // chia ra để lấy dữ liệu vdu: ở trang 2 sẽ lấy dữ liệu từ 10 -> 20

            //category.Skip(20).Take(10).ToList()

            var data = await order.Skip(recSkip).Take(pager.PageSize).ToListAsync();

            ViewBag.Pager = pager;

            return View(data);
            //return View(await _dbContext.Orders.OrderByDescending(p => p.Id).ToListAsync());
		}
		[HttpGet]
        public async Task<IActionResult> ViewOrder(string orderCode)
        {
			var detailOrder = await _dbContext.OrderDetails.Include(p => p.Products).Where(pr => pr.OrderCode == orderCode).ToListAsync();

            return View(detailOrder);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateOrder(string orderCode, int status)
        {
            if (string.IsNullOrEmpty(orderCode))
            {
                return BadRequest(new { message = "Mã đơn hàng không hợp lệ" });
            }

            var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.OrderCode == orderCode);
            if (order == null)
            {
                return NotFound(new { message = "Không tìm thấy đơn hàng" });
            }

            order.Status = status;
            _dbContext.Orders.Update(order);
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "Cập nhật thành công" });
        }

    }
}
