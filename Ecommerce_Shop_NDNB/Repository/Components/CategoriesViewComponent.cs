using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Shop_NDNB.Repository.Components
{
	public class CategoriesViewComponent : ViewComponent
	{
		private readonly DB_Context _dbContext;
		public CategoriesViewComponent(DB_Context context)
		{
			_dbContext = context;
		}
		public async Task<IViewComponentResult> InvokeAsync() => View(await _dbContext.Categories.ToListAsync());
	}
}
