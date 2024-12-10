using Ecommerce_Shop_NDNB.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Shop_NDNB.Repository
{
	public class SeedData
	{
		public static void SeedingData(DB_Context context)
		{
		/*	
			context.Database.Migrate();
			if(!context.Products.Any())
			{
				CategoryModel Shirt = new CategoryModel { Name = "T-Shirt", Slug = "Uniqlo", Description = "T-shirt Uniqlo", Status = 1 };
				CategoryModel Pants = new CategoryModel { Name = "Pants", Slug = "Stussy", Description = "Pants Stussy", Status = 1 };
				BrandModel Nike = new BrandModel { Name = "T-Shirt", Slug = "Nike", Description = "T-shirt Nike", Status = 1 };
				BrandModel Adidas = new BrandModel { Name = "T-Shirt", Slug = "Adidas", Description = "T-shirt Adidas", Status = 1 };
				context.Products.AddRange(
					new ProductModel { Name = "Nbao", Description = "Nbao", Slug = "Nbao", Image = "1.jpg", Category = Shirt, Price = 1200, Brand = Nike },
					new ProductModel { Name = "Nbao", Description = "Nbao", Slug = "Nbao", Image = "1.jpg", Category = Pants, Price = 1200, Brand = Adidas }
				);
				context.SaveChanges();
			}
			*/
		}

		
	}
}
