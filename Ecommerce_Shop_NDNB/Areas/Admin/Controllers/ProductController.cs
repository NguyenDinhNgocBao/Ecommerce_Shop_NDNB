using Ecommerce_Shop_NDNB.Models;
using Ecommerce_Shop_NDNB.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList.Extensions;

namespace Ecommerce_Shop_NDNB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Sales")]
    public class ProductController : Controller
	{
		private readonly DB_Context db_Context;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public ProductController(DB_Context context, IWebHostEnvironment webHostEnvironment)
		{
			db_Context = context;
			_webHostEnvironment = webHostEnvironment;
		}
		public async Task<IActionResult> Index(int pg = 1)
		{
            var product = db_Context.Products.Include(p => p.Category)
                .Include(p => p.Brand)
                .OrderByDescending(p => p.Id).AsQueryable();
            int recsCount = await product.CountAsync();

            const int pageSize = 10; //10 items/trang
            var pager = new Paginate(recsCount, pg, pageSize);

            if (pg < 1) //page < 1;
            {
                pg = Math.Clamp(pg, 1, pager.TotalPages);//Giới hạn tối đa giá trị trang
            }

            int recSkip = (pg - 1) * pageSize; //(3 - 1) * 10; // chia ra để lấy dữ liệu vdu: ở trang 2 sẽ lấy dữ liệu từ 10 -> 20

            //category.Skip(20).Take(10).ToList()

            var data = await product.Skip(recSkip).Take(pager.PageSize).ToListAsync();

            ViewBag.Pager = pager;

            return View(data);
            /*
            int pageSize = 10; // Số sản phẩm trên mỗi trang
            int pageNumber = page ?? 1; // Trang hiện tại, mặc định là trang 1

            var products = await db_Context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .OrderByDescending(p => p.Id)
                .ToListAsync();

            var pagedProducts = products.ToPagedList(pageNumber, pageSize);

            return View(pagedProducts);
            */
        }
        #region Create
        [HttpGet]
        public IActionResult Create()
		{
			ViewBag.Categories = new SelectList(db_Context.Categories, "Id", "Name");
            ViewBag.Brands = new SelectList(db_Context.Brands, "Id", "Name");
            return View();
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductModel product)
		{
            ViewBag.Categories = new SelectList(db_Context.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(db_Context.Brands, "Id", "Name", product.BrandId);

            product.Slug = product.Name.Replace(" ", "-");
            var slug = await db_Context.Products.FirstOrDefaultAsync(p => p.Slug == product.Slug);
            if (slug != null)
            {
                ModelState.AddModelError("", "Sản phẩm đã tồn tại");
                return View(product);
            }

            // Xử lý ảnh tải lên
            if (product.ImageUpload != null && product.ImageUpload.Length > 0)
            {
                var fileName = Path.GetFileName(product.ImageUpload.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", fileName); // Lưu ảnh vào thư mục wwwroot/images
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await product.ImageUpload.CopyToAsync(stream);
                }
                product.Image = fileName; // Lưu đường dẫn ảnh vào cơ sở dữ liệu
            }

            // Thêm sản phẩm vào cơ sở dữ liệu
            db_Context.Add(product);
            await db_Context.SaveChangesAsync();
            // Thêm thông báo thành công
            TempData["SuccessMessage"] = "Sản phẩm đã được thêm thành công!";
            return RedirectToAction("Index");

        }
        #endregion

        #region Update
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            ProductModel product = await db_Context.Products.FindAsync(id);
            ViewBag.Categories = new SelectList(db_Context.Categories, "Id", "Name");
            ViewBag.Brands = new SelectList(db_Context.Brands, "Id", "Name");
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int Id, ProductModel product)
        {
            ViewBag.Categories = new SelectList(db_Context.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(db_Context.Brands, "Id", "Name", product.BrandId);

            product.Slug = product.Name.Replace(" ", "-");
            var slug = await db_Context.Products.FirstOrDefaultAsync(p => p.Slug == product.Slug && p.Id != Id);
            if (slug != null)
            {
                ModelState.AddModelError("", "Sản phẩm đã tồn tại");
                return View(product);
            }

            var existingProduct = await db_Context.Products.FindAsync(Id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            // Kiểm tra nếu không có hình ảnh mới thì giữ lại hình ảnh cũ
            if (product.ImageUpload != null && product.ImageUpload.Length > 0)
            {
                var fileName = Path.GetFileName(product.ImageUpload.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await product.ImageUpload.CopyToAsync(stream);
                }
                existingProduct.Image = fileName; // Cập nhật ảnh mới
            }
            else
            {
                product.Image = existingProduct.Image; // Giữ lại ảnh cũ
            }

            // Cập nhật các trường khác
            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Description = product.Description;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.BrandId = product.BrandId;
            existingProduct.Slug = product.Slug;


            // Thêm sản phẩm vào cơ sở dữ liệu
            db_Context.Update(existingProduct);
            await db_Context.SaveChangesAsync();

            return RedirectToAction("Index");

        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int Id)
        {
            ProductModel product = await db_Context.Products.FindAsync(Id);
            if (product == null)
            {
                return NotFound();
            }
            if (!string.Equals(product.Image,"noname.jpg", StringComparison.OrdinalIgnoreCase))
            {
                // Xóa file ảnh khỏi thư mục wwwroot/image
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", product.Image);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            // Xóa sản phẩm khỏi cơ sở dữ liệu
            db_Context.Products.Remove(product);
            await db_Context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        #endregion
        #region Add Quantity
        [HttpGet]
        public async Task<IActionResult> AddQuantity(int Id)
        {
            var productbyQuantity = await db_Context.ProductQuantities.Where(pq => pq.ProductId == Id).ToListAsync();
            ViewBag.ProductByQuantity = productbyQuantity;
            ViewBag.Id = Id;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StoreProductQuantity(ProductQuantityModel productQuantityModel)
        {
            var product = db_Context.Products.Find(productQuantityModel.ProductId);
            if(product == null)
            {
                return NotFound();
            }
            product.Quantity += productQuantityModel.Quantity;

            productQuantityModel.Quantity = productQuantityModel.Quantity;
            productQuantityModel.ProductId = productQuantityModel.ProductId;
            productQuantityModel.DateCreate = DateTime.Now;
            db_Context.Add(productQuantityModel);
            db_Context.SaveChanges();
            return RedirectToAction("AddQuantity","Product", new { Id = productQuantityModel.ProductId });
        }
        #endregion
    }
}
