using Ecommerce_Shop_NDNB.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Shop_NDNB.Repository
{
	public class DB_Context : IdentityDbContext<AppUserModel>
	{
		public DB_Context(DbContextOptions<DB_Context> options):base(options) { }
		public DbSet<BrandModel> Brands { get; set; }
		public DbSet<SliderModel> Sliders { get; set; }
		public DbSet<ProductModel> Products { get; set; }
		public DbSet<CategoryModel> Categories { get; set; }
		public DbSet<OrderModel> Orders { get; set; }
		public DbSet<OrderDetails> OrderDetails { get; set; }
		public DbSet<EvaluateModel> Evaluates { get; set; }
		public DbSet<ContactModel> Contacts { get; set; }
		public DbSet<CompareModel> Compares { get; set; }
		public DbSet<WishListModel> WishLists { get; set; }
		public DbSet<ProductQuantityModel> ProductQuantities { get; set; }
		public DbSet<ShippingModel> Shippings { get; set; }
		public DbSet<CouponModel> Coupons { get; set; }

	}
}
