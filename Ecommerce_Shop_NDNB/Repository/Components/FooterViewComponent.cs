using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Shop_NDNB.Repository.Components
{
	public class FooterViewComponent : ViewComponent
	{
		private readonly DB_Context _dbContext;
		public FooterViewComponent(DB_Context context)
		{
			_dbContext = context;
		}
		public async Task<IViewComponentResult> InvokeAsync() => View(await _dbContext.Contacts.FirstOrDefaultAsync());
	}
}
