using Ecommerce_Shop_NDNB.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Shop_NDNB.Repository
{
	public class DB_Context : IdentityDbContext<AppUserModel>
	{
		public DB_Context(DbContextOptions<DB_Context> options):base(options) { }
		public DbSet<BrandModel> Brands { get; set; }
		public DbSet<ProductModel> Products { get; set; }
		public DbSet<CategoryModel> Categories { get; set; }
		public DbSet<OrderModel> Orders { get; set; }
		public DbSet<OrderDetails> OrderDetails { get; set; }
    }
}
