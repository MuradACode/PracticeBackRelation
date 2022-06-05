using FrontToBack.DAL;
using FrontToBack.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBackTask.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Where(n => !n.IsDeleted).ToListAsync();
            return View(products);
        }
        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products.Where(n => !n.IsDeleted && n.Id == id)
                                                        .Include(n => n.Author)
                                                        .Include(n => n.Category)
                                                        .FirstOrDefaultAsync();
            if (product is null)
            {
                return NotFound();
            }
            return View(product);
        }
        public async Task<IActionResult> Update(int id)
        {
            var product = await _context.Products.Where(n => !n.IsDeleted && n.Id == id)
                                                        .Include(n => n.Author)
                                                        .Include(n => n.Category)
                                                        .FirstOrDefaultAsync();
            if (product is null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id,
                                                string productAuthorFullname,
                                                string productCategoryName,
                                                string productName,
                                                string productDescription,
                                                decimal productCostPrice,
                                                decimal productSellPrice,
                                                string productInfoText,
                                                bool productIsNew,
                                                bool productIsFeatured,
                                                int productDiscount,
                                                IFormFile productImageFile)
        {
            var product = await _context.Products.Where(n => !n.IsDeleted && n.Id == id)
                                                        .Include(n => n.Author)
                                                        .Include(n => n.Category)
                                                        .FirstOrDefaultAsync();
            if (!productImageFile.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("ImageFile", "You can only upload an image");
                return View();
            }
            if (productImageFile.Length > 3145728)
            {
                ModelState.AddModelError("ImageFile", "You can only upload an image above than 3MB");
                return View();
            }
            string filename = productImageFile.FileName;
            if (filename.Length > 64)
            {
                filename = filename.Substring(filename.Length - 64, 64);
            }
            string newFileName = Guid.NewGuid().ToString() + filename;
            string path = Path.Combine(_env.WebRootPath, "assets/uploads/products", newFileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                product.ImageFile.CopyTo(stream);
            }
            product.Image = newFileName;
            if (product is null /*|| string.IsNullOrEmpty(authorName)*/)
            {
                return NotFound();
            }
            if (string.IsNullOrEmpty(productCategoryName) || string.IsNullOrEmpty(productName))
            {
                return NotFound();
            }
            if (productCostPrice <= 0 || productSellPrice <= 0 || productDiscount <= 0)
            {
                return NotFound();
            }
            product.Author.Fullname = productAuthorFullname.Trim();
            product.Name = productName.Trim();
            product.Description = productDescription.Trim();
            product.CostPrice = productCostPrice;
            product.SellPrice = productSellPrice;
            //product.InfoText = productInfoText.Trim();
            product.IsNew = productIsNew;
            product.IsFeatured = productIsFeatured;
            product.Discount = productDiscount;
            product.UpdatedTime = DateTime.Now;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(actionName: nameof(Details), controllerName: nameof(Product), routeValues: new { id });
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (!product.ImageFile.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("ImageFile", "You can only upload an image");
                return View();
            }
            if (product.ImageFile.Length > 3145728)
            {
                ModelState.AddModelError("ImageFile", "You can only upload an image above than 3MB");
                return View();
            }
            string filename = product.ImageFile.FileName;
            if (filename.Length > 64)
            {
                filename = filename.Substring(filename.Length - 64, 64);
            }
            string newFileName = Guid.NewGuid().ToString() + filename;
            string path = Path.Combine(_env.WebRootPath, "assets/uploads/products", newFileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                product.ImageFile.CopyTo(stream);
            }
            product.Image = newFileName;
            product.IsDeleted = false;
            product.CreatedTime = DateTime.Now;

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(actionName: nameof(Index), controllerName: nameof(Product), routeValues: new { id = product.Id });
        }
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.Where(n => !n.IsDeleted && n.Id == id)
                                                        .Include(n => n.Author)
                                                        .Include(n => n.Category)
                                                        .FirstOrDefaultAsync();
            if (product is null)
            {
                return NotFound();
            }
            product.IsDeleted = true;
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Product product)
        {
            var dbProduct = await _context.Products.Where(n => !n.IsDeleted && n.Id == product.Id)
                                                        .Include(n => n.Author)
                                                        .Include(n => n.Category)
                                                        .FirstOrDefaultAsync();

            if (dbProduct.Name.ToLower() != product.Name.Trim().ToLower())
            {
                return NotFound();
            }
            if (dbProduct is null)
            {
                return NotFound();
            }
            dbProduct.IsDeleted = true;
            _context.Products.Update(dbProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(actionName: nameof(Index), controllerName: nameof(Product));
        }
        private async Task<bool> isProductExists (int id,
                                                  string authorName,
                                                  string categoryName,
                                                  string name,
                                                  string desc,
                                                  decimal costPrice,
                                                  decimal sellPrice,
                                                  string infoText,
                                                  bool isNew,
                                                  bool isFeatured,
                                                  int discount)
        {
            var isExists = await _context.Products.Include(n => n.Author)
                                                  .Include(n => n.Category)
                                                  .AnyAsync(n => n.Author.Fullname.ToLower() == authorName.Trim().ToLower()
                                                           && n.Category.Name.ToLower() == categoryName.Trim().ToLower()
                                                           && n.Name.ToLower() == name.Trim().ToLower()
                                                           && n.Description.ToLower() == desc.Trim().ToLower()
                                                           && n.CostPrice == costPrice
                                                           && n.SellPrice == sellPrice
                                                           && n.InfoText.ToLower() == infoText.Trim().ToLower()
                                                           && n.IsNew == isNew
                                                           && n.IsFeatured == isFeatured
                                                           && n.Discount == discount);
            return isExists;
        }
    }
}
